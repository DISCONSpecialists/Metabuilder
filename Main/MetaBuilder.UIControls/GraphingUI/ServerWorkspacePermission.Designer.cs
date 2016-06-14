namespace MetaBuilder.UIControls.GraphingUI
{
    partial class ServerWorkspacePermission
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
            this.treeViewWorkspaces = new System.Windows.Forms.TreeView();
            this.btnNewWorkspace = new MetaBuilder.MetaControls.MetaButton();
            this.btnOK = new MetaBuilder.MetaControls.MetaButton();
            this.btnCancel = new MetaBuilder.MetaControls.MetaButton();
            this.lblDomainName = new System.Windows.Forms.Label();
            this.txtDomainName = new System.Windows.Forms.TextBox();
            this.btnRetrieveUsers = new MetaBuilder.MetaControls.MetaButton();
            this.dataGridViewUserPermissions = new System.Windows.Forms.DataGridView();
            this.Username = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Admin = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Read = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Write = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Delete = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.labelCurrentWorkspace = new System.Windows.Forms.Label();
            this.btnLoadList = new MetaBuilder.MetaControls.MetaButton();
            this.btnSave = new MetaBuilder.MetaControls.MetaButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDomainGroup = new System.Windows.Forms.TextBox();
            this.btnNewUser = new MetaBuilder.MetaControls.MetaButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUserPermissions)).BeginInit();
            this.SuspendLayout();
            // 
            // treeViewWorkspaces
            // 
            this.treeViewWorkspaces.Location = new System.Drawing.Point(4, 2);
            this.treeViewWorkspaces.Name = "treeViewWorkspaces";
            this.treeViewWorkspaces.Size = new System.Drawing.Size(272, 302);
            this.treeViewWorkspaces.TabIndex = 0;
            this.treeViewWorkspaces.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewWorkspaces_AfterSelect);
            this.treeViewWorkspaces.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewWorkspaces_BeforeSelect);
            // 
            // btnNewWorkspace
            // 
            this.btnNewWorkspace.CornerRadius = new Ascend.CornerRadius(2);
            this.btnNewWorkspace.GradientLowColor = System.Drawing.Color.DarkGray;
            this.btnNewWorkspace.Location = new System.Drawing.Point(4, 281);
            this.btnNewWorkspace.Name = "btnNewWorkspace";
            this.btnNewWorkspace.Size = new System.Drawing.Size(142, 23);
            this.btnNewWorkspace.StayActiveAfterClick = false;
            this.btnNewWorkspace.TabIndex = 9;
            this.btnNewWorkspace.Text = "New Workspace";
            this.btnNewWorkspace.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnNewWorkspace.Visible = false;
            this.btnNewWorkspace.Click += new System.EventHandler(this.btnNewWorkspace_Click);
            // 
            // btnOK
            // 
            this.btnOK.CornerRadius = new Ascend.CornerRadius(2);
            this.btnOK.GradientLowColor = System.Drawing.Color.DarkGray;
            this.btnOK.Location = new System.Drawing.Point(539, 281);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.StayActiveAfterClick = false;
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.CornerRadius = new Ascend.CornerRadius(2);
            this.btnCancel.GradientLowColor = System.Drawing.Color.DarkGray;
            this.btnCancel.Location = new System.Drawing.Point(620, 281);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.StayActiveAfterClick = false;
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblDomainName
            // 
            this.lblDomainName.AutoSize = true;
            this.lblDomainName.Location = new System.Drawing.Point(282, 5);
            this.lblDomainName.Name = "lblDomainName";
            this.lblDomainName.Size = new System.Drawing.Size(43, 13);
            this.lblDomainName.TabIndex = 12;
            this.lblDomainName.Text = "Domain";
            // 
            // txtDomainName
            // 
            this.txtDomainName.Location = new System.Drawing.Point(331, 2);
            this.txtDomainName.Name = "txtDomainName";
            this.txtDomainName.Size = new System.Drawing.Size(127, 20);
            this.txtDomainName.TabIndex = 10;
            this.txtDomainName.TextChanged += new System.EventHandler(this.txtDomainName_TextChanged);
            // 
            // btnRetrieveUsers
            // 
            this.btnRetrieveUsers.CornerRadius = new Ascend.CornerRadius(2);
            this.btnRetrieveUsers.GradientLowColor = System.Drawing.Color.DarkGray;
            this.btnRetrieveUsers.Location = new System.Drawing.Point(637, 2);
            this.btnRetrieveUsers.Name = "btnRetrieveUsers";
            this.btnRetrieveUsers.Size = new System.Drawing.Size(58, 23);
            this.btnRetrieveUsers.StayActiveAfterClick = false;
            this.btnRetrieveUsers.TabIndex = 11;
            this.btnRetrieveUsers.Text = "Retrieve";
            this.btnRetrieveUsers.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnRetrieveUsers.Click += new System.EventHandler(this.btnRetrieveUsers_Click);
            // 
            // dataGridViewUserPermissions
            // 
            this.dataGridViewUserPermissions.AllowUserToAddRows = false;
            this.dataGridViewUserPermissions.AllowUserToDeleteRows = false;
            this.dataGridViewUserPermissions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewUserPermissions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Username,
            this.Admin,
            this.Read,
            this.Write,
            this.Delete});
            this.dataGridViewUserPermissions.Location = new System.Drawing.Point(282, 44);
            this.dataGridViewUserPermissions.MultiSelect = false;
            this.dataGridViewUserPermissions.Name = "dataGridViewUserPermissions";
            this.dataGridViewUserPermissions.RowHeadersVisible = false;
            this.dataGridViewUserPermissions.Size = new System.Drawing.Size(410, 231);
            this.dataGridViewUserPermissions.TabIndex = 13;
            this.dataGridViewUserPermissions.Visible = false;
            this.dataGridViewUserPermissions.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewUserPermissions_CellMouseUp);
            this.dataGridViewUserPermissions.DataSourceChanged += new System.EventHandler(this.dataGridViewUserPermissions_DataSourceChanged);
            // 
            // Username
            // 
            this.Username.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Username.DataPropertyName = "Username";
            this.Username.HeaderText = "Username";
            this.Username.Name = "Username";
            this.Username.ReadOnly = true;
            // 
            // Admin
            // 
            this.Admin.DataPropertyName = "Admin";
            this.Admin.HeaderText = "System Admin";
            this.Admin.Name = "Admin";
            this.Admin.Width = 50;
            // 
            // Read
            // 
            this.Read.DataPropertyName = "Read";
            this.Read.HeaderText = "R";
            this.Read.Name = "Read";
            this.Read.Width = 50;
            // 
            // Write
            // 
            this.Write.DataPropertyName = "Write";
            this.Write.HeaderText = "RW";
            this.Write.Name = "Write";
            this.Write.Width = 50;
            // 
            // Delete
            // 
            this.Delete.DataPropertyName = "Delete";
            this.Delete.HeaderText = "RWD";
            this.Delete.Name = "Delete";
            this.Delete.Width = 50;
            // 
            // labelCurrentWorkspace
            // 
            this.labelCurrentWorkspace.AutoSize = true;
            this.labelCurrentWorkspace.Location = new System.Drawing.Point(282, 27);
            this.labelCurrentWorkspace.Name = "labelCurrentWorkspace";
            this.labelCurrentWorkspace.Size = new System.Drawing.Size(201, 13);
            this.labelCurrentWorkspace.TabIndex = 14;
            this.labelCurrentWorkspace.Text = "There is currently no workspace selected";
            // 
            // btnLoadList
            // 
            this.btnLoadList.CornerRadius = new Ascend.CornerRadius(2);
            this.btnLoadList.GradientLowColor = System.Drawing.Color.DarkGray;
            this.btnLoadList.Location = new System.Drawing.Point(285, 281);
            this.btnLoadList.Name = "btnLoadList";
            this.btnLoadList.Size = new System.Drawing.Size(68, 23);
            this.btnLoadList.StayActiveAfterClick = false;
            this.btnLoadList.TabIndex = 16;
            this.btnLoadList.Text = "Load List";
            this.btnLoadList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnLoadList.Click += new System.EventHandler(this.btnLoadList_Click);
            // 
            // btnSave
            // 
            this.btnSave.CornerRadius = new Ascend.CornerRadius(2);
            this.btnSave.GradientLowColor = System.Drawing.Color.DarkGray;
            this.btnSave.Location = new System.Drawing.Point(356, 281);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(68, 23);
            this.btnSave.StayActiveAfterClick = false;
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "Save List";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(462, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Group";
            // 
            // txtDomainGroup
            // 
            this.txtDomainGroup.Location = new System.Drawing.Point(504, 3);
            this.txtDomainGroup.Name = "txtDomainGroup";
            this.txtDomainGroup.Size = new System.Drawing.Size(125, 20);
            this.txtDomainGroup.TabIndex = 17;
            this.txtDomainGroup.Text = "Domain Users";
            // 
            // btnNewUser
            // 
            this.btnNewUser.CornerRadius = new Ascend.CornerRadius(2);
            this.btnNewUser.GradientLowColor = System.Drawing.Color.DarkGray;
            this.btnNewUser.Location = new System.Drawing.Point(428, 281);
            this.btnNewUser.Name = "btnNewUser";
            this.btnNewUser.Size = new System.Drawing.Size(68, 23);
            this.btnNewUser.StayActiveAfterClick = false;
            this.btnNewUser.TabIndex = 15;
            this.btnNewUser.Text = "New User";
            this.btnNewUser.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnNewUser.Click += new System.EventHandler(this.btnNewUser_Click);
            // 
            // ServerWorkspacePermission
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 308);
            this.Controls.Add(this.btnLoadList);
            this.Controls.Add(this.btnNewUser);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.labelCurrentWorkspace);
            this.Controls.Add(this.dataGridViewUserPermissions);
            this.Controls.Add(this.lblDomainName);
            this.Controls.Add(this.txtDomainName);
            this.Controls.Add(this.btnRetrieveUsers);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.treeViewWorkspaces);
            this.Controls.Add(this.btnNewWorkspace);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDomainGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ServerWorkspacePermission";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Server Permissions";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUserPermissions)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewWorkspaces;
        private MetaControls.MetaButton btnNewWorkspace;
        private MetaControls.MetaButton btnOK;
        private MetaControls.MetaButton btnCancel;
        private System.Windows.Forms.Label lblDomainName;
        private System.Windows.Forms.TextBox txtDomainName;
        private MetaControls.MetaButton btnRetrieveUsers;
        private System.Windows.Forms.DataGridView dataGridViewUserPermissions;
        private System.Windows.Forms.Label labelCurrentWorkspace;
        private System.Windows.Forms.DataGridViewTextBoxColumn Username;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Admin;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Read;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Write;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Delete;
        private MetaControls.MetaButton btnLoadList;
        private MetaControls.MetaButton btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDomainGroup;
        private MetaBuilder.MetaControls.MetaButton btnNewUser;
    }
}