﻿/*
 Copyright (c) 2015 [Joerg Ruedenauer]
 
 This file is part of Ares.

 Ares is free software; you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation; either version 2 of the License, or
 (at your option) any later version.

 Ares is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with Ares; if not, write to the Free Software
 Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ares.ModelInfo;

namespace Ares.Editor.ElementEditors
{
    partial class TriggerEditor : EditorBase
    {
        public TriggerEditor(Ares.Data.IProject project)
        {
            InitializeComponent();
            m_Project = project;
            m_Tooltip = new ToolTip();
            m_Tooltip.SetToolTip(allCrossFadeButton, StringResources.SetFadingForAll);
            m_Tooltip.SetToolTip(allSoundsFadeButton, StringResources.SetFadingForAll);
        }

        private ToolTip m_Tooltip;

        private Ares.Data.IModeElement m_Element;
        private Ares.Data.IProject m_Project;

        public void SetElement(Ares.Data.IModeElement element)
        {
            m_Element = element;
            ElementId = element.Id;
            UpdateTitle();
            UpdateData();
            Actions.ElementChanges.Instance.AddListener(-1, Update);
            Actions.ElementChanges.Instance.AddListener(element.Id, Update);
        }

        private void Update(int elementID, Actions.ElementChanges.ChangeType changeType)
        {
            if (elementID != ElementId)
            {
                if (changeType == Actions.ElementChanges.ChangeType.Changed)
                {
                    listen = false;
                    UpdateTriggerDesc();
                    listen = true;
                }
                else if (changeType == Actions.ElementChanges.ChangeType.TriggerChanged)
                {
                    UpdateErrorProvider();
                }
            }
            else
            {
                if (changeType == Actions.ElementChanges.ChangeType.Renamed)
                    UpdateTitle();
                else if (changeType == Actions.ElementChanges.ChangeType.TriggerChanged)
                    UpdateData();
                else if (changeType == Actions.ElementChanges.ChangeType.Removed)
                    Close();
            }
        }

        private void UpdateTitle()
        {
            this.Text = String.Format(StringResources.TriggerEditorTitle, m_Element.Title);
        }

        private void UpdateData()
        {
            listen = false;

            Ares.Data.ITrigger trigger = m_Element.Trigger;
            if (trigger != null)
            {
                if (trigger.TriggerType == Data.TriggerType.Key)
                {
                    Ares.Data.IKeyTrigger keyTrigger = trigger as Ares.Data.IKeyTrigger;
                    KeysConverter converter = new KeysConverter();
                    selectKeyButton.Text = converter.ConvertToString((Keys)keyTrigger.KeyCode);
                }
                else
                {
                    selectKeyButton.Text = StringResources.NoKey;
                }
                stopMusicBox.Checked = trigger.StopMusic;
                stopSoundsBox.Checked = trigger.StopSounds;
                crossFadeButton.Checked = trigger.CrossFadeMusic;
                fadeButton.Checked = trigger.FadeMusic;
                noFadeButton.Checked = !trigger.FadeMusic && !trigger.CrossFadeMusic;
                crossFadingUpDown.Value = trigger.FadeMusicTime;
                fadeOutSoundsBox.Checked = trigger.FadeSounds;
                fadeOutSoundsUpDown.Value = trigger.FadeSoundTime;
            }
            else
            {
                selectKeyButton.Text = StringResources.NoKey;
                stopMusicBox.Checked = false;
                stopSoundsBox.Checked = false;
                crossFadeButton.Checked = false;
                fadeButton.Checked = false;
                noFadeButton.Checked = true;
                crossFadingUpDown.Value = 0;
                fadeOutSoundsBox.Checked = false;
                fadeOutSoundsBox.Enabled = false;
                fadeOutSoundsUpDown.Value = 0;
            }
            UpdateTriggerDesc();
            UpdateErrorProvider();
            hideInPlayerBox.Checked = !m_Element.IsVisibleInPlayer;

            listen = true;
        }

        private void UpdateErrorProvider()
        {
            int keyCode = GetCurrentKeyCode();
            if (keyCode == -1)
            {
                errorProvider.SetError(selectKeyButton, String.Empty);
            }
            else
            {
                errorProvider.SetError(selectKeyButton, Ares.CommonGUI.KeyChecks.GetErrorForKey(m_Project, m_Element, keyCode));
            }
        }

        private int GetCurrentKeyCode()
        {
            if (selectKeyButton.Text == StringResources.NoKey)
            {
                return -1;
            }
            else
            {
                KeysConverter converter = new KeysConverter();
                return (int)(Keys)converter.ConvertFromString(selectKeyButton.Text);
            }
        }

        private void UpdateTriggerDesc()
        {
            if (m_Element.IsEndless())
            {
                triggerDescLabel.Text = StringResources.ElementIsEndless;
            }
            else
            {
                triggerDescLabel.Text = StringResources.ElementIsSingle;
            }
            if (m_Element.AlwaysStartsMusic())
            {
                stopMusicBox.Checked = true;
                stopMusicBox.Enabled = false;
                stopMusicLabel.Visible = true;
            }
            else
            {
                stopMusicBox.Enabled = true;
                stopMusicLabel.Visible = false;
            }
            crossFadeButton.Enabled = stopMusicBox.Checked;
            fadeButton.Enabled = stopMusicBox.Checked;
            noFadeButton.Enabled = stopMusicBox.Checked;
            crossFadingUpDown.Enabled = stopMusicBox.Checked && (crossFadeButton.Checked || fadeButton.Checked);
            allCrossFadeButton.Enabled = stopMusicBox.Checked;
            fadeOutSoundsBox.Enabled = stopSoundsBox.Checked && (!stopMusicBox.Checked || !crossFadeButton.Checked);
            fadeOutSoundsUpDown.Enabled = stopSoundsBox.Checked && fadeOutSoundsBox.Enabled && fadeOutSoundsBox.Checked;
            allSoundsFadeButton.Enabled = stopSoundsBox.Checked;
        }

        private void Commit()
        {
            if (selectKeyButton.Text != StringResources.NoKey)
            {
                Ares.Data.IKeyTrigger trigger = Ares.Data.DataModule.ElementFactory.CreateKeyTrigger();
                trigger.KeyCode = GetCurrentKeyCode();
                trigger.StopMusic = stopMusicBox.Checked;
                trigger.StopSounds = stopSoundsBox.Checked;
                trigger.CrossFadeMusic = crossFadeButton.Checked;
                trigger.FadeMusic = fadeButton.Checked;
                trigger.FadeMusicTime = (Int32)crossFadingUpDown.Value;
                trigger.FadeSounds = fadeOutSoundsBox.Checked;
                trigger.FadeSoundTime = (Int32)fadeOutSoundsUpDown.Value;
                Ares.Editor.Actions.Actions.Instance.AddNew(new Ares.Editor.Actions.SetModeElementTriggerAction(m_Element, trigger), m_Project);
            }
            else
            {
                Ares.Data.ITrigger trigger = Ares.Data.DataModule.ElementFactory.CreateNoTrigger();
                trigger.StopMusic = stopMusicBox.Checked;
                trigger.StopSounds = stopSoundsBox.Checked;
                trigger.CrossFadeMusic = crossFadeButton.Checked;
                trigger.FadeMusic = fadeButton.Checked;
                trigger.FadeMusicTime = (Int32)crossFadingUpDown.Value;
                trigger.FadeSounds = fadeOutSoundsBox.Checked;
                trigger.FadeSoundTime = (Int32)fadeOutSoundsUpDown.Value;
                Ares.Editor.Actions.Actions.Instance.AddNew(new Ares.Editor.Actions.SetModeElementTriggerAction(m_Element, trigger), m_Project);
            }
        }

        private void stopSoundsBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!listen)
                return;
            Commit();
            fadeOutSoundsBox.Enabled = stopSoundsBox.Checked && (!stopMusicBox.Checked || !crossFadeButton.Checked);
            fadeOutSoundsUpDown.Enabled = stopSoundsBox.Checked && fadeOutSoundsBox.Checked;
            allSoundsFadeButton.Enabled = stopSoundsBox.Checked;
        }

        private void SelectKey()
        {
            int keyCode = 0;
            DialogResult result = Dialogs.KeyDialog.Show(this, out keyCode);
            if (result != DialogResult.Cancel)
            {
                KeysConverter converter = new KeysConverter();
                Keys key = (Keys)keyCode;
                if (key == Keys.Delete)
                {
                    selectKeyButton.Text = StringResources.NoKey;
                }
                else
                {
                    selectKeyButton.Text = converter.ConvertToString(key);
                }
                UpdateErrorProvider();
                Commit();
            }
        }

        private bool listen = true;

        private void stopMusicBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!listen)
                return;
            Commit();
            crossFadeButton.Enabled = stopMusicBox.Checked;
            fadeButton.Enabled = stopMusicBox.Checked;
            noFadeButton.Enabled = stopMusicBox.Checked;
            crossFadingUpDown.Enabled = stopMusicBox.Checked && (crossFadeButton.Checked || fadeButton.Checked);
            allCrossFadeButton.Enabled = stopMusicBox.Checked;
            fadeOutSoundsBox.Enabled = !stopMusicBox.Checked || !crossFadeButton.Checked;
            fadeOutSoundsUpDown.Enabled = stopSoundsBox.Checked && fadeOutSoundsBox.Enabled && fadeOutSoundsBox.Checked;
            if (stopMusicBox.Checked && crossFadeButton.Checked)
                fadeOutSoundsBox.Checked = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SelectKey();
        }

        private void crossFadingBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!listen)
                return;
            crossFadingUpDown.Enabled = stopMusicBox.Checked && (crossFadeButton.Checked || fadeButton.Checked);
            allCrossFadeButton.Enabled = stopMusicBox.Checked;
            if (crossFadeButton.Checked)
            {
                fadeOutSoundsBox.Checked = false;
                fadeOutSoundsBox.Enabled = false;
                fadeOutSoundsUpDown.Enabled = false;
            }
            else
            {
                fadeOutSoundsBox.Enabled = true;
                fadeOutSoundsUpDown.Enabled = stopSoundsBox.Checked && fadeOutSoundsBox.Checked;
            }
            Commit();
        }

        private void crossFadingUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!listen)
                return;
            Commit();
        }

        private void allCrossFadeButton_Click(object sender, EventArgs e)
        {
            Ares.Editor.Actions.Actions.Instance.AddNew(new Ares.Editor.Actions.SetAllTriggerMusicFadingAction(
                fadeButton.Checked, crossFadeButton.Checked, (int)crossFadingUpDown.Value, m_Project), m_Project);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!listen)
                return;
            Ares.Editor.Actions.Actions.Instance.AddNew(new Ares.Editor.Actions.SetModeElementVisibleAction(
                m_Element, !hideInPlayerBox.Checked), m_Project);
        }

        private void fadeOutSoundsBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!listen)
                return;
            fadeOutSoundsUpDown.Enabled = stopSoundsBox.Checked && fadeOutSoundsBox.Enabled && fadeOutSoundsBox.Checked;
            allSoundsFadeButton.Enabled = stopSoundsBox.Checked;
            Commit();
        }

        private void fadeOutSoundsUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!listen)
                return;
            Commit();
        }

        private void allSoundsFadeButton_Click(object sender, EventArgs e)
        {
            Ares.Editor.Actions.Actions.Instance.AddNew(new Ares.Editor.Actions.SetAllTriggerSoundFadingAction(
                fadeOutSoundsBox.Checked, (int)fadeOutSoundsUpDown.Value, m_Project), m_Project);
        }


    }
}
