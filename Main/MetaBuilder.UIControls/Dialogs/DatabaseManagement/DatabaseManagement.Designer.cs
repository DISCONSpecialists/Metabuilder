namespace MetaBuilder.UIControls.Dialogs.DatabaseManagement
{
    partial class DatabaseManagement
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
            this.btnManageObjectsInRecycler = new MetaControls.MetaButton();
            this.btnManageAssociationsInRecycler = new MetaControls.MetaButton();
            this.btnManageObjectsAddToRecycler = new MetaControls.MetaButton();
            this.btnManageAssociationsAddToRecycler = new MetaControls.MetaButton();
            this.btnManageWorkspaces = new MetaControls.MetaButton();
            this.btnWorkspaceTransfer = new MetaControls.MetaButton();
            this.btnManageDiagrams = new MetaControls.MetaButton();
            this.btnManageDuplicates = new MetaControls.MetaButton();
            this.btnEmptySandbox = new MetaControls.MetaButton();
            this.btnBackupDatabase = new MetaControls.MetaButton();
            this.btnClearDatabase = new MetaControls.MetaButton();
            this.btnClose = new MetaControls.MetaButton();
            this.btnScript = new MetaControls.MetaButton();
            this.groupBoxObjects = new System.Windows.Forms.GroupBox();
            this.btnDeleteOrphans = new MetaControls.MetaButton();
            this.btnDeleteRundundantServerObjects = new MetaControls.MetaButton();
            this.groupBoxDiagrams = new System.Windows.Forms.GroupBox();
            this.groupBoxWorkspace = new System.Windows.Forms.GroupBox();
            this.groupBoxDatabase = new System.Windows.Forms.GroupBox();
            this.groupBoxAssociations = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxServer = new System.Windows.Forms.ComboBox();
            this.groupBoxObjects.SuspendLayout();
            this.groupBoxDiagrams.SuspendLayout();
            this.groupBoxWorkspace.SuspendLayout();
            this.groupBoxDatabase.SuspendLayout();
            this.groupBoxAssociations.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnManageObjectsInRecycler
            // 
            this.btnManageObjectsInRecycler.Location = new System.Drawing.Point(6, 45);
            this.btnManageObjectsInRecycler.Name = "btnManageObjectsInRecycler";
            this.btnManageObjectsInRecycler.Size = new System.Drawing.Size(188, 20);
            this.btnManageObjectsInRecycler.TabIndex = 17;
            this.btnManageObjectsInRecycler.Text = "Manage Recycled Objects";
            this.btnManageObjectsInRecycler.Click += new System.EventHandler(this.btnManageObjectsInRecycler_Click);
            // 
            // btnManageAssociationsInRecycler
            // 
            this.btnManageAssociationsInRecycler.Location = new System.Drawing.Point(6, 45);
            this.btnManageAssociationsInRecycler.Name = "btnManageAssociationsInRecycler";
            this.btnManageAssociationsInRecycler.Size = new System.Drawing.Size(188, 20);
            this.btnManageAssociationsInRecycler.TabIndex = 17;
            this.btnManageAssociationsInRecycler.Text = "Manage Recycled Associations";
            this.btnManageAssociationsInRecycler.Click += new System.EventHandler(this.btnManageMarkedForDeleteAssociaitons_Click);
            // 
            // btnManageObjectsAddToRecycler
            // 
            this.btnManageObjectsAddToRecycler.Location = new System.Drawing.Point(6, 19);
            this.btnManageObjectsAddToRecycler.Name = "btnManageObjectsAddToRecycler";
            this.btnManageObjectsAddToRecycler.Size = new System.Drawing.Size(188, 20);
            this.btnManageObjectsAddToRecycler.TabIndex = 17;
            this.btnManageObjectsAddToRecycler.Text = "Recycle Objects";
            this.btnManageObjectsAddToRecycler.Click += new System.EventHandler(this.btnManageObjectsAddToRecycler_Click);
            // 
            // btnManageAssociationsAddToRecycler
            // 
            this.btnManageAssociationsAddToRecycler.Location = new System.Drawing.Point(6, 19);
            this.btnManageAssociationsAddToRecycler.Name = "btnManageAssociationsAddToRecycler";
            this.btnManageAssociationsAddToRecycler.Size = new System.Drawing.Size(188, 20);
            this.btnManageAssociationsAddToRecycler.TabIndex = 17;
            this.btnManageAssociationsAddToRecycler.Text = "Recycle Associations";
            this.btnManageAssociationsAddToRecycler.Click += new System.EventHandler(this.btnManageAssociationsAddToRecycler_Click);
            // 
            // btnManageWorkspaces
            // 
            this.btnManageWorkspaces.Location = new System.Drawing.Point(6, 45);
            this.btnManageWorkspaces.Name = "btnManageWorkspaces";
            this.btnManageWorkspaces.Size = new System.Drawing.Size(188, 20);
            this.btnManageWorkspaces.TabIndex = 21;
            this.btnManageWorkspaces.Text = "Workspace Manager";
            this.btnManageWorkspaces.Click += new System.EventHandler(this.btnManageWorkspaces_Click_1);
            // 
            // btnWorkspaceTransfer
            // 
            this.btnWorkspaceTransfer.Location = new System.Drawing.Point(6, 19);
            this.btnWorkspaceTransfer.Name = "btnWorkspaceTransfer";
            this.btnWorkspaceTransfer.Size = new System.Drawing.Size(188, 20);
            this.btnWorkspaceTransfer.TabIndex = 20;
            this.btnWorkspaceTransfer.Text = "Workspace Transfer";
            this.btnWorkspaceTransfer.Click += new System.EventHandler(this.btnWorkspaceTransfer_Click);
            // 
            // btnManageDiagrams
            // 
            this.btnManageDiagrams.Location = new System.Drawing.Point(6, 19);
            this.btnManageDiagrams.Name = "btnManageDiagrams";
            this.btnManageDiagrams.Size = new System.Drawing.Size(188, 20);
            this.btnManageDiagrams.TabIndex = 17;
            this.btnManageDiagrams.Text = "Manage Diagrams";
            this.btnManageDiagrams.Click += new System.EventHandler(this.btnManageDiagrams_Click);
            // 
            // btnManageDuplicates
            // 
            this.btnManageDuplicates.Location = new System.Drawing.Point(6, 123);
            this.btnManageDuplicates.Name = "btnManageDuplicates";
            this.btnManageDuplicates.Size = new System.Drawing.Size(188, 20);
            this.btnManageDuplicates.TabIndex = 17;
            this.btnManageDuplicates.Text = "Manage Duplicate Objects";
            this.btnManageDuplicates.Click += new System.EventHandler(this.btnManageDuplicates_Click);
            // 
            // btnEmptySandbox
            // 
            this.btnEmptySandbox.Location = new System.Drawing.Point(6, 71);
            this.btnEmptySandbox.Name = "btnEmptySandbox";
            this.btnEmptySandbox.Size = new System.Drawing.Size(188, 20);
            this.btnEmptySandbox.TabIndex = 17;
            this.btnEmptySandbox.Text = "Empty Sandbox Workspace";
            this.btnEmptySandbox.Click += new System.EventHandler(this.btnEmptySandbox_Click);
            // 
            // btnBackupDatabase
            // 
            this.btnBackupDatabase.Location = new System.Drawing.Point(6, 16);
            this.btnBackupDatabase.Name = "btnBackupDatabase";
            this.btnBackupDatabase.Size = new System.Drawing.Size(188, 20);
            this.btnBackupDatabase.TabIndex = 17;
            this.btnBackupDatabase.Text = "Backup Database";
            this.btnBackupDatabase.Click += new System.EventHandler(this.btnBackupDatabase_Click);
            // 
            // btnClearDatabase
            // 
            this.btnClearDatabase.Location = new System.Drawing.Point(6, 44);
            this.btnClearDatabase.Name = "btnClearDatabase";
            this.btnClearDatabase.Size = new System.Drawing.Size(188, 20);
            this.btnClearDatabase.TabIndex = 17;
            this.btnClearDatabase.Text = "Clear Database";
            this.btnClearDatabase.Click += new System.EventHandler(this.btnClearDatabase_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(363, 292);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(67, 20);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnScript
            // 
            this.btnScript.Location = new System.Drawing.Point(6, 71);
            this.btnScript.Name = "btnScript";
            this.btnScript.Size = new System.Drawing.Size(188, 20);
            this.btnScript.TabIndex = 17;
            this.btnScript.Text = "Run Script On Database";
            this.btnScript.Click += new System.EventHandler(this.btnScript_Click);
            // 
            // groupBoxObjects
            // 
            this.groupBoxObjects.Controls.Add(this.btnDeleteOrphans);
            this.groupBoxObjects.Controls.Add(this.btnManageDuplicates);
            this.groupBoxObjects.Controls.Add(this.btnDeleteRundundantServerObjects);
            this.groupBoxObjects.Controls.Add(this.btnManageObjectsAddToRecycler);
            this.groupBoxObjects.Controls.Add(this.btnManageObjectsInRecycler);
            this.groupBoxObjects.Location = new System.Drawing.Point(15, 33);
            this.groupBoxObjects.Name = "groupBoxObjects";
            this.groupBoxObjects.Size = new System.Drawing.Size(206, 153);
            this.groupBoxObjects.TabIndex = 22;
            this.groupBoxObjects.TabStop = false;
            this.groupBoxObjects.Text = "Objects";
            // 
            // btnDeleteOrphans
            // 
            this.btnDeleteOrphans.Location = new System.Drawing.Point(6, 71);
            this.btnDeleteOrphans.Name = "btnDeleteOrphans";
            this.btnDeleteOrphans.Size = new System.Drawing.Size(188, 20);
            this.btnDeleteOrphans.TabIndex = 17;
            this.btnDeleteOrphans.Text = "Delete Orphaned Objects";
            this.btnDeleteOrphans.Click += new System.EventHandler(this.btnDeleteOrphans_Click);
            // 
            // btnDeleteRundundantServerObjects
            // 
            this.btnDeleteRundundantServerObjects.Location = new System.Drawing.Point(6, 97);
            this.btnDeleteRundundantServerObjects.Name = "btnDeleteRundundantServerObjects";
            this.btnDeleteRundundantServerObjects.Size = new System.Drawing.Size(188, 20);
            this.btnDeleteRundundantServerObjects.TabIndex = 17;
            this.btnDeleteRundundantServerObjects.Text = "Delete Redundant Server Objects";
            this.btnDeleteRundundantServerObjects.Click += new System.EventHandler(this.btnDeleteRundundantServerObjects_Click);
            // 
            // groupBoxDiagrams
            // 
            this.groupBoxDiagrams.Controls.Add(this.btnManageDiagrams);
            this.groupBoxDiagrams.Location = new System.Drawing.Point(15, 273);
            this.groupBoxDiagrams.Name = "groupBoxDiagrams";
            this.groupBoxDiagrams.Size = new System.Drawing.Size(206, 48);
            this.groupBoxDiagrams.TabIndex = 23;
            this.groupBoxDiagrams.TabStop = false;
            this.groupBoxDiagrams.Text = "Diagrams";
            // 
            // groupBoxWorkspace
            // 
            this.groupBoxWorkspace.Controls.Add(this.btnWorkspaceTransfer);
            this.groupBoxWorkspace.Controls.Add(this.btnEmptySandbox);
            this.groupBoxWorkspace.Controls.Add(this.btnManageWorkspaces);
            this.groupBoxWorkspace.Location = new System.Drawing.Point(227, 33);
            this.groupBoxWorkspace.Name = "groupBoxWorkspace";
            this.groupBoxWorkspace.Size = new System.Drawing.Size(203, 102);
            this.groupBoxWorkspace.TabIndex = 24;
            this.groupBoxWorkspace.TabStop = false;
            this.groupBoxWorkspace.Text = "Workspaces";
            // 
            // groupBoxDatabase
            // 
            this.groupBoxDatabase.Controls.Add(this.btnBackupDatabase);
            this.groupBoxDatabase.Controls.Add(this.btnClearDatabase);
            this.groupBoxDatabase.Controls.Add(this.btnScript);
            this.groupBoxDatabase.Location = new System.Drawing.Point(230, 140);
            this.groupBoxDatabase.Name = "groupBoxDatabase";
            this.groupBoxDatabase.Size = new System.Drawing.Size(200, 103);
            this.groupBoxDatabase.TabIndex = 25;
            this.groupBoxDatabase.TabStop = false;
            this.groupBoxDatabase.Text = "Database";
            // 
            // groupBoxAssociations
            // 
            this.groupBoxAssociations.Controls.Add(this.btnManageAssociationsAddToRecycler);
            this.groupBoxAssociations.Controls.Add(this.btnManageAssociationsInRecycler);
            this.groupBoxAssociations.Location = new System.Drawing.Point(15, 192);
            this.groupBoxAssociations.Name = "groupBoxAssociations";
            this.groupBoxAssociations.Size = new System.Drawing.Size(206, 75);
            this.groupBoxAssociations.TabIndex = 26;
            this.groupBoxAssociations.TabStop = false;
            this.groupBoxAssociations.Text = "Associations";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Database";
            // 
            // comboBoxServer
            // 
            this.comboBoxServer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxServer.Items.AddRange(new object[] {
            Core.Variables.Instance.ClientProvider,
            Core.Variables.Instance.ServerProvider});
            this.comboBoxServer.Location = new System.Drawing.Point(80, 6);
            this.comboBoxServer.Name = "comboBoxServer";
            this.comboBoxServer.Size = new System.Drawing.Size(141, 21);
            this.comboBoxServer.TabIndex = 28;
            this.comboBoxServer.SelectedIndexChanged += new System.EventHandler(this.comboBoxServer_SelectedIndexChanged);
            // 
            // DatabaseManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 324);
            this.Controls.Add(this.comboBoxServer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBoxAssociations);
            this.Controls.Add(this.groupBoxDatabase);
            this.Controls.Add(this.groupBoxWorkspace);
            this.Controls.Add(this.groupBoxDiagrams);
            this.Controls.Add(this.groupBoxObjects);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DatabaseManagement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Database Management";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.groupBoxObjects.ResumeLayout(false);
            this.groupBoxDiagrams.ResumeLayout(false);
            this.groupBoxWorkspace.ResumeLayout(false);
            this.groupBoxDatabase.ResumeLayout(false);
            this.groupBoxAssociations.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        void MainForm_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
    
        }

        #endregion

        private MetaControls.MetaButton btnManageObjectsInRecycler;
        private MetaControls.MetaButton btnManageAssociationsInRecycler;
        private MetaControls.MetaButton btnManageObjectsAddToRecycler;
        private MetaControls.MetaButton btnManageAssociationsAddToRecycler;
        private MetaControls.MetaButton btnManageDiagrams;
        private MetaControls.MetaButton btnEmptySandbox;
        private MetaControls.MetaButton btnBackupDatabase;
        private MetaControls.MetaButton btnClearDatabase;
        private MetaControls.MetaButton btnClose;
        private MetaControls.MetaButton btnScript;
        private MetaControls.MetaButton btnManageDuplicates;
        private MetaControls.MetaButton btnWorkspaceTransfer;
        private MetaControls.MetaButton btnManageWorkspaces;
        private System.Windows.Forms.GroupBox groupBoxObjects;
        private System.Windows.Forms.GroupBox groupBoxDiagrams;
        private System.Windows.Forms.GroupBox groupBoxWorkspace;
        private System.Windows.Forms.GroupBox groupBoxDatabase;
        private System.Windows.Forms.GroupBox groupBoxAssociations;
        private MetaControls.MetaButton btnDeleteOrphans;
        private MetaControls.MetaButton btnDeleteRundundantServerObjects;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxServer;
    }
}