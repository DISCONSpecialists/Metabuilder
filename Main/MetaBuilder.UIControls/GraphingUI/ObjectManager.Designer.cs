namespace MetaBuilder.UIControls.GraphingUI
{
    partial class ObjectManager
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ObjectManager));
            this.navigationPane1 = new Ascend.Windows.Forms.NavigationPane();
            this.navigationPanePage1 = new Ascend.Windows.Forms.NavigationPanePage();
            this.treeRemote = new System.Windows.Forms.TreeView();
            this.contextStripServer = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteWorkspaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.navigationPanePage2 = new Ascend.Windows.Forms.NavigationPanePage();
            this.treeLocal = new System.Windows.Forms.TreeView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageFiles = new System.Windows.Forms.TabPage();
            this.syncPanelFiles = new MetaBuilder.UIControls.GraphingUI.SynchronisationPanel();
            this.tabPageObjects = new System.Windows.Forms.TabPage();
            this.syncPanelObjects = new MetaBuilder.UIControls.GraphingUI.SynchronisationPanel();
            this.tabPageRelationships = new System.Windows.Forms.TabPage();
            this.syncPanelRelationships = new MetaBuilder.UIControls.GraphingUI.SynchronisationPanel();
            this.tabPageAdministration = new System.Windows.Forms.TabPage();
            this.txtConfirm = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnClearOrphans = new MetaControls.MetaButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnClearSandbox = new MetaControls.MetaButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPurgeInactiveVersions = new MetaControls.MetaButton();
            this.label4 = new System.Windows.Forms.Label();
            this.btnPurgeFiles = new MetaControls.MetaButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.labelNavPane = new System.Windows.Forms.Label();
            this.buttonRefreshRepository = new MetaControls.MetaButton();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.button2 = new MetaControls.MetaButton();
            this.button3 = new MetaControls.MetaButton();
            this.navigationPane1.SuspendLayout();
            this.navigationPanePage1.SuspendLayout();
            this.contextStripServer.SuspendLayout();
            this.navigationPanePage2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageFiles.SuspendLayout();
            this.tabPageObjects.SuspendLayout();
            this.tabPageRelationships.SuspendLayout();
            this.tabPageAdministration.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // navigationPane1
            // 
            this.navigationPane1.AllowAddOrRemove = false;
            this.navigationPane1.AllowOptions = false;
            this.navigationPane1.ButtonActiveGradientHighColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(225)))), ((int)(((byte)(155)))));
            this.navigationPane1.ButtonActiveGradientLowColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(165)))), ((int)(((byte)(78)))));
            this.navigationPane1.ButtonBorderColor = System.Drawing.SystemColors.MenuHighlight;
            this.navigationPane1.ButtonFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.navigationPane1.ButtonForeColor = System.Drawing.SystemColors.ControlText;
            this.navigationPane1.ButtonGradientHighColor = System.Drawing.SystemColors.ButtonHighlight;
            this.navigationPane1.ButtonGradientLowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.navigationPane1.ButtonHeight = 25;
            this.navigationPane1.ButtonHighlightGradientHighColor = System.Drawing.Color.White;
            this.navigationPane1.ButtonHighlightGradientLowColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(165)))), ((int)(((byte)(78)))));
            this.navigationPane1.CaptionBorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.navigationPane1.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.navigationPane1.CaptionForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.navigationPane1.CaptionGradientHighColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.navigationPane1.CaptionGradientLowColor = System.Drawing.SystemColors.ActiveCaption;
            this.navigationPane1.CaptionGradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
            this.navigationPane1.CaptionHeight = 25;
            this.navigationPane1.Controls.Add(this.navigationPanePage1);
            this.navigationPane1.Controls.Add(this.navigationPanePage2);
            this.navigationPane1.Cursor = System.Windows.Forms.Cursors.Default;
            this.navigationPane1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navigationPane1.FooterGradientHighColor = System.Drawing.SystemColors.ButtonHighlight;
            this.navigationPane1.FooterGradientLowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.navigationPane1.FooterHeight = 20;
            this.navigationPane1.FooterHighlightGradientHighColor = System.Drawing.Color.White;
            this.navigationPane1.FooterHighlightGradientLowColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(165)))), ((int)(((byte)(78)))));
            this.navigationPane1.Location = new System.Drawing.Point(0, 23);
            this.navigationPane1.Name = "navigationPane1";
            this.navigationPane1.NavigationPages.AddRange(new Ascend.Windows.Forms.NavigationPanePage[] {
            this.navigationPanePage1,
            this.navigationPanePage2});
            this.navigationPane1.Size = new System.Drawing.Size(172, 381);
            this.navigationPane1.TabIndex = 0;
            this.navigationPane1.VisibleButtonCount = 2;
            this.navigationPane1.SelectedIndexChanged += new System.EventHandler(this.navigationPane1_SelectedIndexChanged);
            // 
            // navigationPanePage1
            // 
            this.navigationPanePage1.ActiveGradientHighColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(225)))), ((int)(((byte)(155)))));
            this.navigationPanePage1.ActiveGradientLowColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(165)))), ((int)(((byte)(78)))));
            this.navigationPanePage1.AutoScroll = true;
            this.navigationPanePage1.ButtonFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.navigationPanePage1.ButtonForeColor = System.Drawing.SystemColors.ControlText;
            this.navigationPanePage1.Controls.Add(this.treeRemote);
            this.navigationPanePage1.GradientHighColor = System.Drawing.SystemColors.ButtonHighlight;
            this.navigationPanePage1.GradientLowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.navigationPanePage1.HighlightGradientHighColor = System.Drawing.Color.White;
            this.navigationPanePage1.HighlightGradientLowColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(165)))), ((int)(((byte)(78)))));
            this.navigationPanePage1.Image = null;
            this.navigationPanePage1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.navigationPanePage1.ImageFooter = null;
            this.navigationPanePage1.ImageIndex = -1;
            this.navigationPanePage1.ImageIndexFooter = -1;
            this.navigationPanePage1.ImageKey = "";
            this.navigationPanePage1.ImageKeyFooter = "";
            this.navigationPanePage1.ImageList = null;
            this.navigationPanePage1.ImageListFooter = null;
            this.navigationPanePage1.Key = "navigationPanePage1";
            this.navigationPanePage1.Location = new System.Drawing.Point(1, 26);
            this.navigationPanePage1.Name = "navigationPanePage1";
            this.navigationPanePage1.Size = new System.Drawing.Size(170, 277);
            this.navigationPanePage1.TabIndex = 0;
            this.navigationPanePage1.Text = "Server Repository";
            this.navigationPanePage1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.navigationPanePage1.ToolTipText = null;
            // 
            // treeRemote
            // 
            this.treeRemote.ContextMenuStrip = this.contextStripServer;
            this.treeRemote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeRemote.Location = new System.Drawing.Point(0, 0);
            this.treeRemote.Name = "treeRemote";
            this.treeRemote.Size = new System.Drawing.Size(170, 277);
            this.treeRemote.TabIndex = 0;
            this.treeRemote.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tree_AfterSelect);
            this.treeRemote.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeRemote_NodeMouseClick);
            // 
            // contextStripServer
            // 
            this.contextStripServer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteWorkspaceToolStripMenuItem});
            this.contextStripServer.Name = "contextStripServer";
            this.contextStripServer.Size = new System.Drawing.Size(169, 26);
            this.contextStripServer.Opening += new System.ComponentModel.CancelEventHandler(this.contextStripServer_Opening);
            // 
            // deleteWorkspaceToolStripMenuItem
            // 
            this.deleteWorkspaceToolStripMenuItem.Name = "deleteWorkspaceToolStripMenuItem";
            this.deleteWorkspaceToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.deleteWorkspaceToolStripMenuItem.Text = "Delete Workspace";
            this.deleteWorkspaceToolStripMenuItem.Click += new System.EventHandler(this.deleteWorkspaceToolStripMenuItem_Click);
            // 
            // navigationPanePage2
            // 
            this.navigationPanePage2.ActiveGradientHighColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(225)))), ((int)(((byte)(155)))));
            this.navigationPanePage2.ActiveGradientLowColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(165)))), ((int)(((byte)(78)))));
            this.navigationPanePage2.AutoScroll = true;
            this.navigationPanePage2.ButtonFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.navigationPanePage2.ButtonForeColor = System.Drawing.SystemColors.ControlText;
            this.navigationPanePage2.Controls.Add(this.treeLocal);
            this.navigationPanePage2.GradientHighColor = System.Drawing.SystemColors.ButtonHighlight;
            this.navigationPanePage2.GradientLowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.navigationPanePage2.HighlightGradientHighColor = System.Drawing.Color.White;
            this.navigationPanePage2.HighlightGradientLowColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(165)))), ((int)(((byte)(78)))));
            this.navigationPanePage2.Image = null;
            this.navigationPanePage2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.navigationPanePage2.ImageFooter = null;
            this.navigationPanePage2.ImageIndex = -1;
            this.navigationPanePage2.ImageIndexFooter = -1;
            this.navigationPanePage2.ImageKey = "";
            this.navigationPanePage2.ImageKeyFooter = "";
            this.navigationPanePage2.ImageList = null;
            this.navigationPanePage2.ImageListFooter = null;
            this.navigationPanePage2.Key = "navigationPanePage2";
            this.navigationPanePage2.Location = new System.Drawing.Point(1, 26);
            this.navigationPanePage2.Name = "navigationPanePage2";
            this.navigationPanePage2.Size = new System.Drawing.Size(170, 277);
            this.navigationPanePage2.TabIndex = 1;
            this.navigationPanePage2.Text = "Client Repository";
            this.navigationPanePage2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.navigationPanePage2.ToolTipText = null;
            // 
            // treeLocal
            // 
            this.treeLocal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeLocal.Location = new System.Drawing.Point(0, 0);
            this.treeLocal.Name = "treeLocal";
            this.treeLocal.Size = new System.Drawing.Size(170, 277);
            this.treeLocal.TabIndex = 0;
            this.treeLocal.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tree_AfterSelect);
            this.treeLocal.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeLocal_AfterExpand);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageFiles);
            this.tabControl1.Controls.Add(this.tabPageObjects);
            this.tabControl1.Controls.Add(this.tabPageRelationships);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ItemSize = new System.Drawing.Size(42, 18);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(630, 418);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageFiles
            // 
            this.tabPageFiles.Controls.Add(this.syncPanelFiles);
            this.tabPageFiles.Location = new System.Drawing.Point(4, 22);
            this.tabPageFiles.Name = "tabPageFiles";
            this.tabPageFiles.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFiles.Size = new System.Drawing.Size(622, 392);
            this.tabPageFiles.TabIndex = 2;
            this.tabPageFiles.Text = "Diagrams";
            this.tabPageFiles.UseVisualStyleBackColor = true;
            // 
            // syncPanelFiles
            // 
            this.syncPanelFiles.AllowTypeFilter = false;
            this.syncPanelFiles.CurrentWorkspace = null;
            this.syncPanelFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syncPanelFiles.IsDirty = false;
            this.syncPanelFiles.IsLocal = false;
            this.syncPanelFiles.Items = null;
            this.syncPanelFiles.Location = new System.Drawing.Point(3, 3);
            this.syncPanelFiles.Name = "syncPanelFiles";
            this.syncPanelFiles.Permissions = null;
            this.syncPanelFiles.ShowOpenCheckBox = false;
            this.syncPanelFiles.Size = new System.Drawing.Size(186, 68);
            this.syncPanelFiles.TabIndex = 0;
            // 
            // tabPageObjects
            // 
            this.tabPageObjects.Controls.Add(this.syncPanelObjects);
            this.tabPageObjects.Location = new System.Drawing.Point(4, 22);
            this.tabPageObjects.Name = "tabPageObjects";
            this.tabPageObjects.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageObjects.Size = new System.Drawing.Size(622, 392);
            this.tabPageObjects.TabIndex = 1;
            this.tabPageObjects.Text = "Objects";
            this.tabPageObjects.UseVisualStyleBackColor = true;
            // 
            // syncPanelObjects
            // 
            this.syncPanelObjects.AllowTypeFilter = false;
            this.syncPanelObjects.CurrentWorkspace = null;
            this.syncPanelObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syncPanelObjects.IsDirty = false;
            this.syncPanelObjects.IsLocal = false;
            this.syncPanelObjects.Items = null;
            this.syncPanelObjects.Location = new System.Drawing.Point(3, 3);
            this.syncPanelObjects.Name = "syncPanelObjects";
            this.syncPanelObjects.Permissions = null;
            this.syncPanelObjects.ShowOpenCheckBox = false;
            this.syncPanelObjects.Size = new System.Drawing.Size(629, 382);
            this.syncPanelObjects.TabIndex = 0;
            // 
            // tabPageRelationships
            // 
            this.tabPageRelationships.Controls.Add(this.syncPanelRelationships);
            this.tabPageRelationships.Location = new System.Drawing.Point(4, 22);
            this.tabPageRelationships.Name = "tabPageRelationships";
            this.tabPageRelationships.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRelationships.Size = new System.Drawing.Size(622, 392);
            this.tabPageRelationships.TabIndex = 0;
            this.tabPageRelationships.Text = "Relationships";
            this.tabPageRelationships.UseVisualStyleBackColor = true;
            // 
            // syncPanelRelationships
            // 
            this.syncPanelRelationships.AllowTypeFilter = false;
            this.syncPanelRelationships.CurrentWorkspace = null;
            this.syncPanelRelationships.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syncPanelRelationships.IsDirty = false;
            this.syncPanelRelationships.IsLocal = false;
            this.syncPanelRelationships.Items = null;
            this.syncPanelRelationships.Location = new System.Drawing.Point(3, 3);
            this.syncPanelRelationships.Name = "syncPanelRelationships";
            this.syncPanelRelationships.Permissions = null;
            this.syncPanelRelationships.ShowOpenCheckBox = false;
            this.syncPanelRelationships.Size = new System.Drawing.Size(629, 382);
            this.syncPanelRelationships.TabIndex = 0;
            // 
            // tabPageAdministration
            // 
            this.tabPageAdministration.Controls.Add(this.txtConfirm);
            this.tabPageAdministration.Controls.Add(this.groupBox3);
            this.tabPageAdministration.Controls.Add(this.groupBox2);
            this.tabPageAdministration.Controls.Add(this.groupBox1);
            this.tabPageAdministration.Controls.Add(this.label4);
            this.tabPageAdministration.Controls.Add(this.btnPurgeFiles);
            this.tabPageAdministration.Location = new System.Drawing.Point(4, 22);
            this.tabPageAdministration.Name = "tabPageAdministration";
            this.tabPageAdministration.Size = new System.Drawing.Size(631, 388);
            this.tabPageAdministration.TabIndex = 4;
            this.tabPageAdministration.Text = "Administration";
            this.tabPageAdministration.UseVisualStyleBackColor = true;
            // 
            // txtConfirm
            // 
            this.txtConfirm.Location = new System.Drawing.Point(297, 301);
            this.txtConfirm.Name = "txtConfirm";
            this.txtConfirm.Size = new System.Drawing.Size(139, 20);
            this.txtConfirm.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.btnClearOrphans);
            this.groupBox3.Location = new System.Drawing.Point(15, 203);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(421, 89);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Orphaned Objects";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(359, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Orphaned Objects refer to Objects that do not exist on any active diagram. ";
            // 
            // btnClearOrphans
            // 
            this.btnClearOrphans.Location = new System.Drawing.Point(215, 60);
            this.btnClearOrphans.Name = "btnClearOrphans";
            this.btnClearOrphans.Size = new System.Drawing.Size(195, 23);
            this.btnClearOrphans.TabIndex = 0;
            this.btnClearOrphans.Text = "Clear Orphaned Objects";
            this.btnClearOrphans.Click += new System.EventHandler(this.btnClearOrphans_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.btnClearSandbox);
            this.groupBox2.Location = new System.Drawing.Point(15, 108);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(421, 89);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sandbox";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(383, 26);
            this.label5.TabIndex = 2;
            this.label5.Text = "The Sandbox is your temporary (default) workspace. Clearing this will remove all " +
                "\r\ndiagrams and objects (and related associations) from this workspace.";
            // 
            // btnClearSandbox
            // 
            this.btnClearSandbox.Location = new System.Drawing.Point(215, 60);
            this.btnClearSandbox.Name = "btnClearSandbox";
            this.btnClearSandbox.Size = new System.Drawing.Size(195, 23);
            this.btnClearSandbox.TabIndex = 0;
            this.btnClearSandbox.Text = "Clear Sandbox";
            this.btnClearSandbox.Click += new System.EventHandler(this.btnClearSandbox_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnPurgeInactiveVersions);
            this.groupBox1.Location = new System.Drawing.Point(15, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(421, 89);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Inactive Versions";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(424, 39);
            this.label2.TabIndex = 2;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // btnPurgeInactiveVersions
            // 
            this.btnPurgeInactiveVersions.Location = new System.Drawing.Point(215, 60);
            this.btnPurgeInactiveVersions.Name = "btnPurgeInactiveVersions";
            this.btnPurgeInactiveVersions.Size = new System.Drawing.Size(195, 23);
            this.btnPurgeInactiveVersions.TabIndex = 0;
            this.btnPurgeInactiveVersions.Text = "Purge Inactive Versions";
            this.btnPurgeInactiveVersions.Click += new System.EventHandler(this.btnPurgeFiles_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.DarkRed;
            this.label4.Location = new System.Drawing.Point(12, 301);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(279, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Type \"CONFIRM\" in the textbox to confirm your selection:";
            // 
            // btnPurgeFiles
            // 
            this.btnPurgeFiles.Location = new System.Drawing.Point(106, 73);
            this.btnPurgeFiles.Name = "btnPurgeFiles";
            this.btnPurgeFiles.Size = new System.Drawing.Size(195, 23);
            this.btnPurgeFiles.TabIndex = 0;
            this.btnPurgeFiles.Text = "Purge Inactive Versions of Diagrams";
            this.btnPurgeFiles.Click += new System.EventHandler(this.btnPurgeFiles_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.navigationPane1);
            this.splitContainer1.Panel1.Controls.Add(this.labelNavPane);
            this.splitContainer1.Panel1.Controls.Add(this.buttonRefreshRepository);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(806, 418);
            this.splitContainer1.SplitterDistance = 172;
            this.splitContainer1.TabIndex = 2;
            // 
            // labelNavPane
            // 
            this.labelNavPane.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelNavPane.Location = new System.Drawing.Point(0, 404);
            this.labelNavPane.Name = "labelNavPane";
            this.labelNavPane.Size = new System.Drawing.Size(172, 14);
            this.labelNavPane.TabIndex = 1;
            // 
            // buttonRefreshRepository
            // 
            this.buttonRefreshRepository.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonRefreshRepository.Location = new System.Drawing.Point(0, 0);
            this.buttonRefreshRepository.Name = "buttonRefreshRepository";
            this.buttonRefreshRepository.Size = new System.Drawing.Size(172, 23);
            this.buttonRefreshRepository.TabIndex = 2;
            this.buttonRefreshRepository.Text = "Refresh Current Repository";
            this.buttonRefreshRepository.Click += new System.EventHandler(this.buttonRefreshRepository_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(109, 219);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Confirmation:";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(112, 235);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(139, 20);
            this.textBox3.TabIndex = 1;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(112, 86);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(139, 20);
            this.textBox4.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(257, 232);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(155, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "Purge Orphaned Objects";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(257, 84);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(155, 23);
            this.button3.TabIndex = 0;
            this.button3.Text = "Purge Old Versions of Diagrams";
            // 
            // ObjectManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 418);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ObjectManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Synchronise";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ObjectManager_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ObjectManager_FormClosing);
            this.navigationPane1.ResumeLayout(false);
            this.navigationPanePage1.ResumeLayout(false);
            this.contextStripServer.ResumeLayout(false);
            this.navigationPanePage2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageFiles.ResumeLayout(false);
            this.tabPageObjects.ResumeLayout(false);
            this.tabPageRelationships.ResumeLayout(false);
            this.tabPageAdministration.ResumeLayout(false);
            this.tabPageAdministration.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Ascend.Windows.Forms.NavigationPane navigationPane1;
        private Ascend.Windows.Forms.NavigationPanePage navigationPanePage1;
        private Ascend.Windows.Forms.NavigationPanePage navigationPanePage2;
        private System.Windows.Forms.TreeView treeRemote;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageRelationships;
        private System.Windows.Forms.TabPage tabPageObjects;
        private System.Windows.Forms.TabPage tabPageFiles;
        private SynchronisationPanel syncPanelObjects;
        private SynchronisationPanel syncPanelFiles;
        private SynchronisationPanel syncPanelRelationships;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeLocal;
        private System.Windows.Forms.TabPage tabPageAdministration;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtConfirm;
        private MetaControls.MetaButton btnPurgeFiles;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private MetaControls.MetaButton button2;
        private MetaControls.MetaButton button3;
        private MetaControls.MetaButton btnPurgeInactiveVersions;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label6;
        private MetaControls.MetaButton btnClearOrphans;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private MetaControls.MetaButton btnClearSandbox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelNavPane;
        private System.Windows.Forms.ContextMenuStrip contextStripServer;
        private System.Windows.Forms.ToolStripMenuItem deleteWorkspaceToolStripMenuItem;
        private MetaControls.MetaButton buttonRefreshRepository;

    }
}