namespace MetaBuilder.UIControls.Dialogs.DatabaseManagement
{
    partial class FindDuplicates
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnStartMergeProcess = new MetaControls.MetaButton();
            this.duplicateObjectFinderControl1 = new MetaBuilder.MetaControls.DuplicateObjectFinderControl(Server, ServerWorkspacesUserHasWithAdminPermission);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.progressBarFileMerging = new System.Windows.Forms.ProgressBar();
            this.labelFileMergeProgress = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelFileMergeProgress);
            this.panel1.Controls.Add(this.progressBarFileMerging);
            this.panel1.Controls.Add(this.btnStartMergeProcess);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 359);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(601, 31);
            this.panel1.TabIndex = 1;
            // 
            // btnStartMergeProcess
            // 
            this.btnStartMergeProcess.Location = new System.Drawing.Point(3, 6);
            this.btnStartMergeProcess.Name = "btnStartMergeProcess";
            this.btnStartMergeProcess.Size = new System.Drawing.Size(179, 19);
            this.btnStartMergeProcess.TabIndex = 16;
            this.btnStartMergeProcess.Text = "Start Merge Process...";
            this.btnStartMergeProcess.Click += new System.EventHandler(this.btnStartMergeProcess_Click);
            // 
            // duplicateObjectFinderControl1
            // 
            this.duplicateObjectFinderControl1.AllowMultipleSelection = true;
            this.duplicateObjectFinderControl1.AllowSaveAndLoad = true;
            this.duplicateObjectFinderControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.duplicateObjectFinderControl1.ExcludeStatuses = null;
            this.duplicateObjectFinderControl1.ExcludeVCItems = true;
            this.duplicateObjectFinderControl1.IncludeStatusCombo = false;
            this.duplicateObjectFinderControl1.LimitToClass = null;
            this.duplicateObjectFinderControl1.LimitToStatus = -1;
            this.duplicateObjectFinderControl1.Location = new System.Drawing.Point(0, 0);
            this.duplicateObjectFinderControl1.Name = "duplicateObjectFinderControl1";
            this.duplicateObjectFinderControl1.Size = new System.Drawing.Size(601, 359);
            this.duplicateObjectFinderControl1.TabIndex = 2;
            // 
            // progressBarFileMerging
            // 
            this.progressBarFileMerging.Location = new System.Drawing.Point(188, 6);
            this.progressBarFileMerging.Name = "progressBarFileMerging";
            this.progressBarFileMerging.Size = new System.Drawing.Size(410, 19);
            this.progressBarFileMerging.TabIndex = 17;
            this.progressBarFileMerging.Visible = false;
            // 
            // labelFileMergeProgress
            // 
            this.labelFileMergeProgress.Location = new System.Drawing.Point(3, 6);
            this.labelFileMergeProgress.Name = "labelFileMergeProgress";
            this.labelFileMergeProgress.Size = new System.Drawing.Size(179, 19);
            this.labelFileMergeProgress.TabIndex = 18;
            this.labelFileMergeProgress.Visible = false;
            // 
            // FindDuplicates
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 390);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Controls.Add(this.duplicateObjectFinderControl1);
            this.Controls.Add(this.panel1);
            this.Name = "FindDuplicates";
            this.Text = "Find Duplicate Objects";
            this.Load += new System.EventHandler(this.FindDuplicates_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private MetaControls.MetaButton btnStartMergeProcess;
        private MetaBuilder.MetaControls.DuplicateObjectFinderControl duplicateObjectFinderControl1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label labelFileMergeProgress;
        private System.Windows.Forms.ProgressBar progressBarFileMerging;
    }
}