namespace MetaBuilder.UIControls.Dialogs
{
    partial class DeleteWorkspace
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
            this.btnOK = new MetaControls.MetaButton();
            this.btnCancel = new MetaControls.MetaButton();
            this.radioMoveToWorkspace = new System.Windows.Forms.RadioButton();
            this.radioPermanentlyDelete = new System.Windows.Forms.RadioButton();
            this.comboWorkspaces = new System.Windows.Forms.ComboBox();
            this.txtDelete = new System.Windows.Forms.TextBox();
            this.lblTypeDelete = new System.Windows.Forms.Label();
            this.lblDeleteWorkspace = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(135, 171);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(216, 171);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // radioMoveToWorkspace
            // 
            this.radioMoveToWorkspace.AutoSize = true;
            this.radioMoveToWorkspace.Checked = true;
            this.radioMoveToWorkspace.Location = new System.Drawing.Point(12, 38);
            this.radioMoveToWorkspace.Name = "radioMoveToWorkspace";
            this.radioMoveToWorkspace.Size = new System.Drawing.Size(205, 17);
            this.radioMoveToWorkspace.TabIndex = 1;
            this.radioMoveToWorkspace.TabStop = true;
            this.radioMoveToWorkspace.Text = "Transfer contents into this workspace:";
            this.radioMoveToWorkspace.UseVisualStyleBackColor = true;
            this.radioMoveToWorkspace.Click += new System.EventHandler(this.radioMoveToWorkspace_Click);
            // 
            // radioPermanentlyDelete
            // 
            this.radioPermanentlyDelete.AutoSize = true;
            this.radioPermanentlyDelete.Location = new System.Drawing.Point(11, 93);
            this.radioPermanentlyDelete.Name = "radioPermanentlyDelete";
            this.radioPermanentlyDelete.Size = new System.Drawing.Size(254, 17);
            this.radioPermanentlyDelete.TabIndex = 1;
            this.radioPermanentlyDelete.Text = "Permanently delete this workspace and contents";
            this.radioPermanentlyDelete.UseVisualStyleBackColor = true;
            this.radioPermanentlyDelete.Click += new System.EventHandler(this.radioPermanentlyDelete_Click);
            // 
            // comboWorkspaces
            // 
            this.comboWorkspaces.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboWorkspaces.FormattingEnabled = true;
            this.comboWorkspaces.Location = new System.Drawing.Point(32, 61);
            this.comboWorkspaces.Name = "comboWorkspaces";
            this.comboWorkspaces.Size = new System.Drawing.Size(233, 21);
            this.comboWorkspaces.TabIndex = 2;
            this.comboWorkspaces.SelectedIndexChanged += new System.EventHandler(this.comboWorkspaces_SelectedIndexChanged);
            // 
            // txtDelete
            // 
            this.txtDelete.Enabled = false;
            this.txtDelete.Location = new System.Drawing.Point(32, 117);
            this.txtDelete.Name = "txtDelete";
            this.txtDelete.Size = new System.Drawing.Size(233, 20);
            this.txtDelete.TabIndex = 3;
            this.txtDelete.TextChanged += new System.EventHandler(this.txtDelete_TextChanged);
            // 
            // lblTypeDelete
            // 
            this.lblTypeDelete.AutoSize = true;
            this.lblTypeDelete.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.lblTypeDelete.Location = new System.Drawing.Point(32, 144);
            this.lblTypeDelete.Name = "lblTypeDelete";
            this.lblTypeDelete.Size = new System.Drawing.Size(198, 13);
            this.lblTypeDelete.TabIndex = 4;
            this.lblTypeDelete.Text = "Type \"DELETE\" in box to confirm delete";
            // 
            // lblDeleteWorkspace
            // 
            this.lblDeleteWorkspace.AutoSize = true;
            this.lblDeleteWorkspace.Location = new System.Drawing.Point(11, 13);
            this.lblDeleteWorkspace.Name = "lblDeleteWorkspace";
            this.lblDeleteWorkspace.Size = new System.Drawing.Size(93, 13);
            this.lblDeleteWorkspace.TabIndex = 5;
            this.lblDeleteWorkspace.Text = "Delete workspace";
            // 
            // DeleteWorkspace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(295, 199);
            this.ControlBox = false;
            this.Controls.Add(this.lblDeleteWorkspace);
            this.Controls.Add(this.lblTypeDelete);
            this.Controls.Add(this.txtDelete);
            this.Controls.Add(this.comboWorkspaces);
            this.Controls.Add(this.radioPermanentlyDelete);
            this.Controls.Add(this.radioMoveToWorkspace);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeleteWorkspace";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Delete Workspace";
            this.Load += new System.EventHandler(this.DeleteWorkspace_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetaControls.MetaButton btnOK;
        private MetaControls.MetaButton btnCancel;
        private System.Windows.Forms.RadioButton radioMoveToWorkspace;
        private System.Windows.Forms.RadioButton radioPermanentlyDelete;
        private System.Windows.Forms.ComboBox comboWorkspaces;
        private System.Windows.Forms.TextBox txtDelete;
        private System.Windows.Forms.Label lblTypeDelete;
        private System.Windows.Forms.Label lblDeleteWorkspace;
    }
}