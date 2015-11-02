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
using Ares.Data;

namespace Ares.Editor.ElementEditors
{
    partial class SequentialPlaylistEditor : ContainerEditorBase
    {
        public SequentialPlaylistEditor()
        {
            InitializeComponent();
            AttachOtherEvents(playButton, stopButton);
        }

        protected override Controls.ContainerControl ContainerControl
        {
            get
            {
                return sequentialContainerControl;
            }
        }

        protected override Label BigLabel
        {
            get
            {
                return label1;
            }
        }

        public void SetPlaylist(ISequentialBackgroundMusicList playList, IProject project)
        {
            ElementId = playList.Id;
            m_Element = playList;
            repeatableControl.SetElement(playList, project);
            sequentialContainerControl.SetContainer(playList, project);
            volumeControl.SetElement(playList, project);
            label1.Text = String.Format(label1.Text, String.Format(StringResources.FileExplorerTitle, StringResources.Music));
            ElementSet(project);
        }

        protected override void DisableControls(bool allowStop)
        {
            playButton.Enabled = false;
            stopButton.Enabled = allowStop;
            sequentialContainerControl.Enabled = false;
            repeatableControl.Enabled = false;
            volumeControl.Enabled = false;
        }

        protected override void EnableControls()
        {
            playButton.Enabled = true;
            stopButton.Enabled = false;
            sequentialContainerControl.Enabled = true;
            repeatableControl.Enabled = true;
            volumeControl.Enabled = true;
        }

    }
}
