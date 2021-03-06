﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ares.Editor.Dialogs
{
    public partial class OnlineDbResultDialog : Form
    {
        public OnlineDbResultDialog()
        {
            InitializeComponent();
        }

        private bool m_IsDownload = false;

        private static readonly String sMusicDb = "rpgmusictags.org";

        public void SetIsDownload(int nrOfFiles, int nrOfFoundFiles, int nrOfFilesWithId)
        {
            m_IsDownload = true;
            resultLabel.Text = String.Format(StringResources.DownloadOperationSuccess, sMusicDb);
            musicIdsLabel.Text = String.Format(StringResources.MusicIdStats, nrOfFilesWithId, nrOfFiles);
            secondLabel.Text = String.Format(StringResources.TagsDownloadStats, nrOfFoundFiles, nrOfFiles);
            if (nrOfFilesWithId == nrOfFiles)
            {
                addIdsLabel.Text = String.Empty;
                addIdsLabel.Visible = false;
            }
        }

        public void SetIsUpload(int nrOfFiles, int nrOfFilesWithId)
        {
            m_IsDownload = false;
            resultLabel.Text = String.Format(StringResources.UploadOperationSuccess, sMusicDb);
            musicIdsLabel.Text = String.Format(StringResources.MusicIdStats, nrOfFilesWithId, nrOfFiles);
            secondLabel.Text = String.Empty;
            secondLabel.Visible = false;
            if (nrOfFilesWithId == nrOfFiles)
            {
                addIdsLabel.Text = String.Empty;
                addIdsLabel.Visible = false;
            }
        }

        public String Log { get; set; }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (removeDialogBox.Checked)
            {
                if (m_IsDownload)
                {
                    Settings.Settings.Instance.ShowDialogAfterDownload = false;
                }
                else
                {
                    Settings.Settings.Instance.ShowDialogAfterUpload = false;
                }
                Settings.Settings.Instance.Commit();
            }
        }

        private void showLogButton_Click(object sender, EventArgs e)
        {
            try
            {
                String tempFileName = System.IO.Path.GetTempFileName() + ".txt";
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(tempFileName))
                {
                    writer.Write(Log);
                    writer.Flush();
                }
                System.Diagnostics.Process.Start(tempFileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, StringResources.Ares, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void addIdsLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            String linkTarget = "http://musicbrainz.org/doc/AcoustID";
            System.Diagnostics.Process.Start(linkTarget);
        }


    }
}
