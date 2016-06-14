namespace MetaBuilder.UIControls.GraphingUI
{
    partial class UserPermissions
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
            this.txtDomainName = new System.Windows.Forms.TextBox();
            this.btnList = new MetaControls.MetaButton();
            this.lblDomainName = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Username = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Read = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Write = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Admin = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnCancel = new MetaControls.MetaButton();
            this.btnOK = new MetaControls.MetaButton();
            this.btnNewWorkspace = new MetaControls.MetaButton();
            this.treeWorkgroups = new System.Windows.Forms.TreeView();
            this.btnLoadList = new MetaControls.MetaButton();
            this.btnSave = new MetaControls.MetaButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnFind = new MetaControls.MetaButton();
            this.btnApply = new MetaControls.MetaButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtDomainName
            // 
            this.txtDomainName.Location = new System.Drawing.Point(65, 21);
            this.txtDomainName.Name = "txtDomainName";
            this.txtDomainName.Size = new System.Drawing.Size(227, 20);
            this.txtDomainName.TabIndex = 0;
            // 
            // btnList
            // 
            this.btnList.Location = new System.Drawing.Point(296, 21);
            this.btnList.Name = "btnList";
            this.btnList.Size = new System.Drawing.Size(75, 23);
            this.btnList.TabIndex = 1;
            this.btnList.Text = "Retrieve...";
            this.btnList.Click += new System.EventHandler(this.btnList_Click);
            // 
            // lblDomainName
            // 
            this.lblDomainName.AutoSize = true;
            this.lblDomainName.Location = new System.Drawing.Point(16, 24);
            this.lblDomainName.Name = "lblDomainName";
            this.lblDomainName.Size = new System.Drawing.Size(43, 13);
            this.lblDomainName.TabIndex = 2;
            this.lblDomainName.Text = "Domain";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Username,
            this.Read,
            this.Write,
            this.Admin});
            this.dataGridView1.Location = new System.Drawing.Point(16, 47);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(356, 212);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseUp);
            this.dataGridView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseClick);
            // 
            // Username
            // 
            this.Username.DataPropertyName = "Username";
            this.Username.HeaderText = "Username";
            this.Username.Name = "Username";
            this.Username.ReadOnly = true;
            this.Username.Width = 200;
            // 
            // Read
            // 
            this.Read.DataPropertyName = "Read";
            this.Read.HeaderText = "Read";
            this.Read.Name = "Read";
            this.Read.Width = 50;
            // 
            // Write
            // 
            this.Write.DataPropertyName = "Write";
            this.Write.HeaderText = "Write";
            this.Write.Name = "Write";
            this.Write.Width = 50;
            // 
            // Admin
            // 
            this.Admin.DataPropertyName = "Admin";
            this.Admin.HeaderText = "Admin";
            this.Admin.Name = "Admin";
            this.Admin.Width = 50;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(464, 296);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(302, 296);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnNewWorkspace
            // 
            this.btnNewWorkspace.Location = new System.Drawing.Point(12, 296);
            this.btnNewWorkspace.Name = "btnNewWorkspace";
            this.btnNewWorkspace.Size = new System.Drawing.Size(142, 23);
            this.btnNewWorkspace.TabIndex = 6;
            this.btnNewWorkspace.Text = "New Workspace";
            this.btnNewWorkspace.Click += new System.EventHandler(this.btnNewWorkspace_Click);
            // 
            // treeWorkgroups
            // 
            this.treeWorkgroups.FullRowSelect = true;
            this.treeWorkgroups.HideSelection = false;
            this.treeWorkgroups.LabelEdit = true;
            this.treeWorkgroups.Location = new System.Drawing.Point(12, 4);
            this.treeWorkgroups.Name = "treeWorkgroups";
            this.treeWorkgroups.Size = new System.Drawing.Size(142, 288);
            this.treeWorkgroups.TabIndex = 7;
            this.treeWorkgroups.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeWorkgroups_AfterLabelEdit);
            this.treeWorkgroups.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeWorkgroups_AfterSelect);
            this.treeWorkgroups.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeWorkgroups_MouseUp);
            this.treeWorkgroups.KeyUp += new System.Windows.Forms.KeyEventHandler(this.treeWorkgroups_KeyUp);
            this.treeWorkgroups.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeWorkgroups_BeforeSelect);
            // 
            // btnLoadList
            // 
            this.btnLoadList.Location = new System.Drawing.Point(16, 260);
            this.btnLoadList.Name = "btnLoadList";
            this.btnLoadList.Size = new System.Drawing.Size(68, 23);
            this.btnLoadList.TabIndex = 1;
            this.btnLoadList.Text = "Load List";
            this.btnLoadList.Click += new System.EventHandler(this.btnLoadList_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(84, 260);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(68, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save List";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblDomainName);
            this.groupBox1.Controls.Add(this.txtDomainName);
            this.groupBox1.Controls.Add(this.btnList);
            this.groupBox1.Controls.Add(this.btnLoadList);
            this.groupBox1.Controls.Add(this.btnFind);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Location = new System.Drawing.Point(160, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(380, 288);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Users";
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(304, 260);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(68, 23);
            this.btnFind.TabIndex = 1;
            this.btnFind.Text = "Find...";
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // btnApply
            // 
            this.btnApply.Enabled = false;
            this.btnApply.Location = new System.Drawing.Point(383, 296);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 5;
            this.btnApply.Text = "Apply";
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // UserPermissions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 322);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.treeWorkgroups);
            this.Controls.Add(this.btnNewWorkspace);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserPermissions";
            this.RightToLeftLayout = true;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User Workspace Permissions";
            this.Load += new System.EventHandler(this.UserPermissions_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtDomainName;
        private MetaControls.MetaButton btnList;
        private System.Windows.Forms.Label lblDomainName;
        private System.Windows.Forms.DataGridView dataGridView1;
        private MetaControls.MetaButton btnCancel;
        private MetaControls.MetaButton btnOK;
        private System.Windows.Forms.DataGridViewTextBoxColumn Username;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Read;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Write;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Admin;
        private MetaControls.MetaButton btnNewWorkspace;
        private System.Windows.Forms.TreeView treeWorkgroups;
        private MetaControls.MetaButton btnLoadList;
        private MetaControls.MetaButton btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private MetaControls.MetaButton btnFind;
        private MetaControls.MetaButton btnApply;
    }
}