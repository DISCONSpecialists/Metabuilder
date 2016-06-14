using MetaBuilder.MetaControls;
namespace MetaBuilder.UIControls.GraphingUI
{
    partial class DockingForm
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                if (license != null)
                {
                    license.Dispose();
                    license = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        public void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DockingForm));
            MetaBuilder.Docking.DockPanelSkin dockPanelSkin8 = new MetaBuilder.Docking.DockPanelSkin();
            MetaBuilder.Docking.AutoHideStripSkin autoHideStripSkin8 = new MetaBuilder.Docking.AutoHideStripSkin();
            MetaBuilder.Docking.DockPanelGradient dockPanelGradient22 = new MetaBuilder.Docking.DockPanelGradient();
            MetaBuilder.Docking.TabGradient tabGradient50 = new MetaBuilder.Docking.TabGradient();
            MetaBuilder.Docking.DockPaneStripSkin dockPaneStripSkin8 = new MetaBuilder.Docking.DockPaneStripSkin();
            MetaBuilder.Docking.DockPaneStripGradient dockPaneStripGradient8 = new MetaBuilder.Docking.DockPaneStripGradient();
            MetaBuilder.Docking.TabGradient tabGradient51 = new MetaBuilder.Docking.TabGradient();
            MetaBuilder.Docking.DockPanelGradient dockPanelGradient23 = new MetaBuilder.Docking.DockPanelGradient();
            MetaBuilder.Docking.TabGradient tabGradient52 = new MetaBuilder.Docking.TabGradient();
            MetaBuilder.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient8 = new MetaBuilder.Docking.DockPaneStripToolWindowGradient();
            MetaBuilder.Docking.TabGradient tabGradient53 = new MetaBuilder.Docking.TabGradient();
            MetaBuilder.Docking.TabGradient tabGradient54 = new MetaBuilder.Docking.TabGradient();
            MetaBuilder.Docking.DockPanelGradient dockPanelGradient24 = new MetaBuilder.Docking.DockPanelGradient();
            MetaBuilder.Docking.TabGradient tabGradient55 = new MetaBuilder.Docking.TabGradient();
            MetaBuilder.Docking.TabGradient tabGradient56 = new MetaBuilder.Docking.TabGradient();
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.menuItemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFileNew = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFileNewDiagram = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFileNewMindMapDiagram = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFileNewSymbol = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFileOpenFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFileRecent = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemViewStencils = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemViewPanAndZoom = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemViewObjectExplorer = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemViewPropertiesMeta = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemViewTaskList = new System.Windows.Forms.ToolStripMenuItem();
            this.removeHighlightsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemTools = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsPermissions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsSynchronise = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsSynchronisationManager = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsDocumentTree = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDB = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDBImport = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDatabaseImportExcelTemplates = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDatabaseImportModules = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDatabaseImportModulesFromTextAndExcelWithMappingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDatabaseImporteHPUM = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDatabaseImportObjects = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDBExport = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDatabsaeExportExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDatabaseExportHierarchyTabbed = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDatabaseExportHierarchyNumbered = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDatabaseExportHierarchyBoth = new System.Windows.Forms.ToolStripMenuItem();
            this.wordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDatabaseExportHierarchyWordNumbered = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDatabaseExportHierarchyWordBoth = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDatabaseExportAssociations = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDatabaseExportObjectContext = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDatabaseExportObjectFlow = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDBMapper = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDatabaseObjectManager = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuItemDatabaseSWOTAnalysis = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemReports = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDBDictionary = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemWindows = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemHelpQuick = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemHelpShortcuts = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFileWorkspaces = new System.Windows.Forms.ToolStripMenuItem();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.stripButtonNew = new System.Windows.Forms.ToolStripSplitButton();
            this.stripButtonNewDiagram = new System.Windows.Forms.ToolStripMenuItem();
            this.stripButtonSaveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.stripButtonNewMindMapDiagram = new System.Windows.Forms.ToolStripMenuItem();
            this.stripButtonNewSymbol = new System.Windows.Forms.ToolStripMenuItem();
            this.stripButtonOpen = new System.Windows.Forms.ToolStripButton();
            this.stripButtonHelp = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolstripSaveMode = new System.Windows.Forms.ToolStripDropDownButton();
            this.saveToDatabaseDisabledToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToDatabaseEnabledToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripPWR = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusCurrentWorkspace = new System.Windows.Forms.ToolStripStatusLabel();
            //this.toolStripStatusWorkspaceName = new System.Windows.Forms.ToolStripStatusLabel();

            this.menuItemToolsMode = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsModeClient = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsModeDeveloper = new System.Windows.Forms.ToolStripMenuItem();

            this.menuItemDatabaseServer = new System.Windows.Forms.ToolStripMenuItem();

            this.menuItemToolsLoadFromDatabase = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsLoadFromDatabaseSelectObjects = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsLoadFromDatabaseArtefactObject = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsLoadFromDBRefreshObjects = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsLoadFromDBRefreshObjectsWholeDocument = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsRefreshWholeAuto = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsRefreshWholePrompt = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsLoadFromDBRefreshObjectsSelection = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsRefreshSelectionAuto = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsRefreshSelectionPrompt = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsLoadFromDBChangedObjects = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsLoadFromDBChangedObjectsAddIndicators = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsLoadFromDBChangedObjectsRemoveIndicators = new System.Windows.Forms.ToolStripMenuItem();

            this.toolStripDropDownSelectWorkspace = new System.Windows.Forms.ToolStripDropDownButton();
            this.dockPanel1 = new MetaBuilder.Docking.DockPanel();
            this.menuItemToolsExportHierarchicalData = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.m_paletteDocker = new MetaBuilder.UIControls.GraphingUI.PaletteDocker();
            //this.m_panningWindow = new MetaBuilder.UIControls.GraphingUI.PanningDocker();
            this.m_metaPropsWindow = new MetaPropertyGridDocker();
            this.m_propertyGridWindow = new PropertyGridDocker();
            this.m_metaObjectExplorer = new MetaBuilder.UIControls.GraphingUI.Tools.Explorer.MetaObjectExplorer();
            this.m_taskDocker = new MetaBuilder.UIControls.GraphingUI.TaskDocker();
            this.HelpProvider1 = new System.Windows.Forms.HelpProvider();
            this.imageListWorkspaces = new System.Windows.Forms.ImageList(this.components);
            menuItemDatabaseImportHierarchicalData = new System.Windows.Forms.ToolStripMenuItem();
            menuItemDatabaseExportHierarchyWordTabbed = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMain.SuspendLayout();
            this.toolStripMain.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuItemDatabaseImportHierarchicalData
            // 
            menuItemDatabaseImportHierarchicalData.Name = "menuItemDatabaseImportHierarchicalData";
            menuItemDatabaseImportHierarchicalData.Size = new System.Drawing.Size(200, 22);
            menuItemDatabaseImportHierarchicalData.Text = "Textual Structure Diagram";
            menuItemDatabaseImportHierarchicalData.Click += new System.EventHandler(this.menuItemDatabaseImportHierarchicalData_Click);
            // 
            // menuItemDatabaseExportHierarchyWordTabbed
            // 
            menuItemDatabaseExportHierarchyWordTabbed.Name = "menuItemDatabaseExportHierarchyWordTabbed";
            menuItemDatabaseExportHierarchyWordTabbed.Size = new System.Drawing.Size(275, 22);
            menuItemDatabaseExportHierarchyWordTabbed.Text = "Hierarchical Data (Tabbed)";
            menuItemDatabaseExportHierarchyWordTabbed.Click += new System.EventHandler(this.menuItemDatabaseExportHierarchyWordTabbed_Click);
            // 
            // BottomToolStripPanel
            // 
            this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.BottomToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // menuMain
            // 
            //this.menuMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemFile,
            this.menuItemView,
            this.menuItemTools,
            this.menuItemDB,
            this.menuItemReports,
            this.menuItemWindows,
            this.menuItemHelp});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.MdiWindowListItem = this.menuItemWindows;
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(623, 24);
            this.menuMain.TabIndex = 0;
            this.menuMain.AllowMerge = true;
            this.menuMain.Text = "menuStrip1";
            // 
            // menuItemFile
            // 
            this.menuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemFileNewDiagram,
            this.menuItemFileNewSymbol,
            this.menuItemFileOpen,
            this.menuItemFileOpenFolder,
            this.menuItemFileRecent,
            new System.Windows.Forms.ToolStripSeparator(),
            this.menuItemToolsOptions,
            this.menuItemFileExit});
            this.menuItemFile.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.menuItemFile.MergeIndex = 0;
            this.menuItemFile.Name = "menuItemFile";
            this.menuItemFile.Size = new System.Drawing.Size(35, 20);
            this.menuItemFile.Text = "&File";
            // 
            // menuItemFileNew
            // 
            this.menuItemFileNew.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            //this.menuItemFileNewDiagram,
            //this.menuItemFileNewMindMapDiagram,
            //this.menuItemFileNewSymbol
            });
            this.menuItemFileNew.Image = ((System.Drawing.Image)(resources.GetObject("menuItemFileNew.Image")));
            this.menuItemFileNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuItemFileNew.MergeIndex = 0;
            this.menuItemFileNew.Name = "menuItemFileNew";
            this.menuItemFileNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.menuItemFileNew.Size = new System.Drawing.Size(140, 22);
            this.menuItemFileNew.Text = "&New";
            // 
            // menuItemFileNewDiagram
            // 
            this.menuItemFileNewDiagram.Name = "menuItemFileNewDiagram";
            this.menuItemFileNewDiagram.MergeIndex = 0;
            this.menuItemFileNewDiagram.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.menuItemFileNewDiagram.Size = new System.Drawing.Size(118, 22);
            this.menuItemFileNewDiagram.Image = ((System.Drawing.Image)(resources.GetObject("menuItemFileNew.Image")));
            this.menuItemFileNewDiagram.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.menuItemFileNewDiagram.Tag = "New Diagram";
            this.menuItemFileNewDiagram.Text = "&New";
            this.menuItemFileNewDiagram.Click += new System.EventHandler(this.menuItemFileNewDiagram_Click);
            // 
            // menuItemFileNewMindMapDiagram
            // 
            this.menuItemFileNewMindMapDiagram.Name = "menuItemFileNewMindMapDiagram";
            this.menuItemFileNewMindMapDiagram.Size = new System.Drawing.Size(118, 22);
            this.menuItemFileNewMindMapDiagram.Tag = "New Meta Map";
            this.menuItemFileNewMindMapDiagram.Text = "&Meta Mapper";
            this.menuItemFileNewMindMapDiagram.Click += new System.EventHandler(this.menuItemFileNewMindMapDiagram_Click);
            // 
            // menuItemFileNewSymbol
            // 
            this.menuItemFileNewSymbol.Name = "menuItemFileNewSymbol";
            this.menuItemFileNewSymbol.MergeIndex = 2;
            this.menuItemFileNewSymbol.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.menuItemFileNewSymbol.Size = new System.Drawing.Size(118, 22);
            this.menuItemFileNewSymbol.Text = "New S&ymbol";
            this.menuItemFileNewSymbol.Click += new System.EventHandler(this.menuItemFileNewSymbol_Click);
            // 
            // menuItemFileOpen
            // 
            this.menuItemFileOpen.Image = ((System.Drawing.Image)(resources.GetObject("menuItemFileOpen.Image")));
            this.menuItemFileOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuItemFileOpen.MergeIndex = 1;
            this.menuItemFileOpen.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.menuItemFileOpen.Name = "menuItemFileOpen";
            this.menuItemFileOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.menuItemFileOpen.Size = new System.Drawing.Size(140, 22);
            this.menuItemFileOpen.Text = "&Open";
            this.menuItemFileOpen.Click += new System.EventHandler(this.menuItemFileOpen_Click);
            // 
            // menuItemFileOpenFolder
            // 
            this.menuItemFileOpenFolder.Image = ((System.Drawing.Image)(resources.GetObject("menuItemFileOpen.Image")));
            this.menuItemFileOpenFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuItemFileOpenFolder.MergeIndex = 1;
            this.menuItemFileOpenFolder.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.menuItemFileOpenFolder.Name = "menuItemFileOpenFolder";
            this.menuItemFileOpenFolder.Size = new System.Drawing.Size(140, 22);
            this.menuItemFileOpenFolder.Text = "Open Folder";
            this.menuItemFileOpenFolder.Click += new System.EventHandler(this.menuItemFileOpenFolder_Click);
            // 
            // menuItemFileRecent
            // 
            this.menuItemFileRecent.MergeIndex = 2;
            this.menuItemFileRecent.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.menuItemFileRecent.Name = "menuItemFileRecent";
            this.menuItemFileRecent.Size = new System.Drawing.Size(140, 22);
            this.menuItemFileRecent.Text = "Recent";
            // 
            // menuItemFileExit
            // 
            //this.menuItemFileExit.MergeAction = System.Windows.Forms.MergeAction.Insert;
            //this.menuItemFileExit.MergeIndex = 10;
            this.menuItemFileExit.Name = "menuItemFileExit";
            this.menuItemFileExit.Size = new System.Drawing.Size(140, 22);
            this.menuItemFileExit.Text = "E&xit";
            this.menuItemFileExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // menuItemView
            // 
            this.menuItemView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemViewStencils,
            this.menuItemViewPropertiesMeta,
            this.menuItemViewTaskList,
            this.menuItemViewPanAndZoom,
            this.menuItemViewObjectExplorer});
            //this.removeHighlightsToolStripMenuItem
            this.menuItemView.Name = "menuItemView";
            this.menuItemView.Size = new System.Drawing.Size(41, 20);
            this.menuItemView.Text = "&View";
            // 
            // menuItemViewStencils
            // 
            this.menuItemViewStencils.Name = "menuItemViewStencils";
            this.menuItemViewStencils.Size = new System.Drawing.Size(199, 22);
            this.menuItemViewStencils.Text = "&Stencil Pane";
            this.menuItemViewStencils.Click += new System.EventHandler(this.menuItemViewStencils_Click);
            // 
            // menuItemViewObjectExplorer
            // 
            this.menuItemViewObjectExplorer.Name = "menuItemViewObjectExplorer";
            this.menuItemViewObjectExplorer.Size = new System.Drawing.Size(199, 22);
            this.menuItemViewObjectExplorer.Text = "&Object Explorer Pane";
            this.menuItemViewObjectExplorer.Click += new System.EventHandler(this.menuItemViewObjectExplorer_Click);
            // 
            // menuItemViewPanAndZoom
            // 
            this.menuItemViewPanAndZoom.Name = "menuItemViewPanAndZoom";
            this.menuItemViewPanAndZoom.Size = new System.Drawing.Size(199, 22);
            this.menuItemViewPanAndZoom.Text = "Pan and Zoom Window";
            this.menuItemViewPanAndZoom.Visible = false;
            this.menuItemViewPanAndZoom.Click += new System.EventHandler(this.menuItemViewPanAndZoom_Click);
            // 
            // menuItemViewPropertiesMeta
            // 
            this.menuItemViewPropertiesMeta.Name = "menuItemViewPropertiesMeta";
            this.menuItemViewPropertiesMeta.Size = new System.Drawing.Size(199, 22);
            this.menuItemViewPropertiesMeta.Text = "Meta Properties Pane";
            this.menuItemViewPropertiesMeta.Click += new System.EventHandler(this.menuItemViewPropertiesMeta_Click);
            // 
            // menuItemViewTaskList
            // 
            this.menuItemViewTaskList.Name = "menuItemViewTaskList";
            this.menuItemViewTaskList.Size = new System.Drawing.Size(199, 22);
            this.menuItemViewTaskList.Text = "Task Pane";
            this.menuItemViewTaskList.Click += new System.EventHandler(this.menuItemViewTaskList_Click);
            // 
            // removeHighlightsToolStripMenuItem
            // 
            this.removeHighlightsToolStripMenuItem.Name = "removeHighlightsToolStripMenuItem";
            this.removeHighlightsToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.removeHighlightsToolStripMenuItem.Text = "Remove Highlights";
            this.removeHighlightsToolStripMenuItem.Click += new System.EventHandler(this.removeHighlightsToolStripMenuItem_Click);
            // 
            // menuItemTools
            // 
            this.menuItemTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemFileNewMindMapDiagram,
            this.menuItemDBMapper,
            this.mnuItemDatabaseSWOTAnalysis,
            //this.menuItemToolsOptions,
            //this.menuItemToolsPermissions,
            //this.menuItemToolsSynchronise,
            this.menuItemToolsDocumentTree,
            this.menuItemToolsMode});
            this.menuItemTools.Name = "menuItemTools";
            this.menuItemTools.MergeIndex = 6;
            this.menuItemTools.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.menuItemTools.Text = "&Tools";
            // 
            // menuItemToolsOptions
            // 
            this.menuItemToolsOptions.Name = "menuItemToolsOptions";
            this.menuItemToolsOptions.Size = new System.Drawing.Size(147, 22);
            this.menuItemToolsOptions.Text = "&Options";
            this.menuItemToolsOptions.MergeIndex = 10;
            this.menuItemToolsOptions.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.menuItemToolsOptions.Click += new System.EventHandler(this.menuItemToolsOptions_Click);// 
            // menuItemToolsSynchronise
            // 
            this.menuItemToolsSynchronise.Name = "menuItemToolsSynchronise";
            this.menuItemToolsSynchronise.Size = new System.Drawing.Size(147, 22);
            this.menuItemToolsSynchronise.Text = "Synchronise All";
            this.menuItemToolsSynchronise.Click += new System.EventHandler(this.menuItemToolsSynchronise_Click);
            // 
            // menuItemToolsPermissions
            // 
            this.menuItemToolsPermissions.Name = "menuItemToolsPermissions";
            this.menuItemToolsPermissions.Size = new System.Drawing.Size(147, 22);
            this.menuItemToolsPermissions.Text = "Permissions";
            this.menuItemToolsPermissions.Click += new System.EventHandler(this.menuItemToolsPermissions_Click);
            // 
            // menuItemToolsSynchronisationManager
            // 
            this.menuItemToolsSynchronisationManager.Name = "menuItemToolsSynchronisationManager";
            this.menuItemToolsSynchronisationManager.Size = new System.Drawing.Size(147, 22);
            this.menuItemToolsSynchronisationManager.Text = "Synchronise";
            this.menuItemToolsSynchronisationManager.Click += new System.EventHandler(this.menuItemToolsSynchronisationManager_Click);
            this.menuItemToolsSynchronisationManager.MouseUp += new System.Windows.Forms.MouseEventHandler(menuItemToolsSynchronisationManager_MouseUp);
            // 
            // menuItemToolsDocumentTree
            // 
            this.menuItemToolsDocumentTree.Name = "menuItemToolsDocumentTree";
            this.menuItemToolsDocumentTree.Size = new System.Drawing.Size(147, 22);
            this.menuItemToolsDocumentTree.Text = "Document Tree";
            this.menuItemToolsDocumentTree.Visible = false;
            this.menuItemToolsDocumentTree.Click += new System.EventHandler(this.menuItemToolsDocumentTree_Click);
            //
            //menuItemToolsModeClient
            //
            this.menuItemToolsModeClient = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsModeClient.Name = "menuItemToolsModeClient";
            this.menuItemToolsModeClient.Size = new System.Drawing.Size(147, 22);
            this.menuItemToolsModeClient.Text = "&Client";
            this.menuItemToolsModeClient.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.menuItemToolsModeClient.CheckOnClick = true;
            this.menuItemToolsModeClient.Checked = true;
            this.menuItemToolsModeClient.CheckedChanged += new System.EventHandler(menuItemToolsModeClient_CheckedChanged);
            //
            //menuItemToolsModeDeveloper
            //
            this.menuItemToolsModeDeveloper = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsModeDeveloper.Name = "menuItemToolsModeDeveloper";
            this.menuItemToolsModeDeveloper.Size = new System.Drawing.Size(147, 22);
            this.menuItemToolsModeDeveloper.Text = "&Developer";
            this.menuItemToolsModeDeveloper.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.menuItemToolsModeDeveloper.CheckOnClick = true;
            this.menuItemToolsModeDeveloper.Checked = false;
            this.menuItemToolsModeDeveloper.CheckedChanged += new System.EventHandler(menuItemToolsModeDeveloper_CheckedChanged);
            //
            //menuItemToolsMode
            //
            this.menuItemToolsMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemToolsModeClient,
            this.menuItemToolsModeDeveloper});
            this.menuItemToolsMode.Name = "menuItemToolsMode";
            this.menuItemToolsMode.Size = new System.Drawing.Size(147, 22);
            this.menuItemToolsMode.Text = "&Mode";
            this.menuItemToolsMode.MergeIndex = 100;
            //this.menuItemToolsMode.MergeAction = System.Windows.Forms.MergeAction.Insert;
            // menuItemDB
            // 
            this.menuItemDB.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemToolsLoadFromDatabase,
            this.menuItemToolsLoadFromDBRefreshObjects,
            this.menuItemToolsLoadFromDBChangedObjects,
            this.menuItemDBImport,
            this.menuItemDBExport,
            //this.menuItemDBMapper,
            this.menuItemDatabaseObjectManager,
            //this.mnuItemDatabaseSWOTAnalysis,
            //this.menuItemReports,
            this.menuItemDatabaseServer,
            this.menuItemDBDictionary});
            this.menuItemDB.MergeIndex = 7;
            this.menuItemDB.Name = "menuItemDB";
            this.menuItemDB.Size = new System.Drawing.Size(65, 20);
            this.menuItemDB.Text = "&Database";
            // 
            // menuItemToolsLoadFromDatabase
            // 
            //this.menuItemToolsLoadFromDatabase.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            //this.menuItemToolsLoadFromDatabaseSelectObjects,
            //this.menuItemToolsLoadFromDatabaseArtefactObject});
            //this.menuItemToolsLoadFromDBRefreshObjects,
            //this.menuItemToolsLoadFromDBChangedObjects});
            this.menuItemToolsLoadFromDatabase.MergeIndex = 0;
            this.menuItemToolsLoadFromDatabase.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.menuItemToolsLoadFromDatabase.Name = "menuItemToolsLoadFromDatabase";
            this.menuItemToolsLoadFromDatabase.Size = new System.Drawing.Size(171, 22);
            this.menuItemToolsLoadFromDatabase.Text = "Load from Database";
            this.menuItemToolsLoadFromDatabase.Click += new System.EventHandler(this.menuItemToolsLoadFromDatabaseSelectObjects_Click);
            // 
            // menuItemToolsLoadFromDBRefreshObjects
            // 
            this.menuItemToolsLoadFromDBRefreshObjects.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemToolsLoadFromDBRefreshObjectsWholeDocument,
            this.menuItemToolsLoadFromDBRefreshObjectsSelection});
            this.menuItemToolsLoadFromDatabase.MergeIndex = 1;
            this.menuItemToolsLoadFromDatabase.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.menuItemToolsLoadFromDBRefreshObjects.Name = "menuItemToolsLoadFromDBRefreshObjects";
            this.menuItemToolsLoadFromDBRefreshObjects.Size = new System.Drawing.Size(157, 22);
            this.menuItemToolsLoadFromDBRefreshObjects.Text = "Refresh Objects";
            // 
            // menuItemToolsLoadFromDBChangedObjects
            // 
            this.menuItemToolsLoadFromDBChangedObjects.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemToolsLoadFromDBChangedObjectsAddIndicators,
            this.menuItemToolsLoadFromDBChangedObjectsRemoveIndicators});
            this.menuItemToolsLoadFromDBChangedObjects.MergeIndex = 2;
            this.menuItemToolsLoadFromDBChangedObjects.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.menuItemToolsLoadFromDBChangedObjects.Name = "menuItemToolsLoadFromDBChangedObjects";
            this.menuItemToolsLoadFromDBChangedObjects.Size = new System.Drawing.Size(157, 22);
            this.menuItemToolsLoadFromDBChangedObjects.Text = "Changed Objects";
            // 
            // menuItemToolsLoadFromDatabaseSelectObjects
            // 
            this.menuItemToolsLoadFromDatabaseSelectObjects.Name = "menuItemToolsLoadFromDatabaseSelectObjects";
            this.menuItemToolsLoadFromDatabaseSelectObjects.Size = new System.Drawing.Size(157, 22);
            this.menuItemToolsLoadFromDatabaseSelectObjects.Text = "Select Objects";
            this.menuItemToolsLoadFromDatabaseSelectObjects.Click += new System.EventHandler(this.menuItemToolsLoadFromDatabaseSelectObjects_Click);
            // 
            // menuItemToolsLoadFromDatabaseArtefactObject
            // 
            this.menuItemToolsLoadFromDatabaseArtefactObject.Name = "menuItemToolsLoadFromDatabaseArtefactObject";
            this.menuItemToolsLoadFromDatabaseArtefactObject.Size = new System.Drawing.Size(157, 22);
            this.menuItemToolsLoadFromDatabaseArtefactObject.Text = "Select Artefacts";
            this.menuItemToolsLoadFromDatabaseArtefactObject.Click += new System.EventHandler(this.menuItemToolsLoadFromDatabaseArtefactObject_Click);
            // 
            // menuItemToolsLoadFromDBRefreshObjectsWholeDocument
            // 
            this.menuItemToolsLoadFromDBRefreshObjectsWholeDocument.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemToolsRefreshWholeAuto,
            this.menuItemToolsRefreshWholePrompt});
            this.menuItemToolsLoadFromDBRefreshObjectsWholeDocument.Name = "menuItemToolsLoadFromDBRefreshObjectsWholeDocument";
            this.menuItemToolsLoadFromDBRefreshObjectsWholeDocument.Size = new System.Drawing.Size(155, 22);
            this.menuItemToolsLoadFromDBRefreshObjectsWholeDocument.Text = "Whole Document";
            // 
            // menuItemToolsRefreshWholeAuto
            // 
            this.menuItemToolsRefreshWholeAuto.Name = "menuItemToolsRefreshWholeAuto";
            this.menuItemToolsRefreshWholeAuto.Size = new System.Drawing.Size(122, 22);
            this.menuItemToolsRefreshWholeAuto.Text = "Automatic";
            this.menuItemToolsRefreshWholeAuto.Click += new System.EventHandler(this.menuItemToolsRefreshWholeAuto_Click);
            // 
            // menuItemToolsRefreshWholePrompt
            // 
            this.menuItemToolsRefreshWholePrompt.Name = "menuItemToolsRefreshWholePrompt";
            this.menuItemToolsRefreshWholePrompt.Size = new System.Drawing.Size(122, 22);
            this.menuItemToolsRefreshWholePrompt.Text = "Prompt";
            this.menuItemToolsRefreshWholePrompt.Click += new System.EventHandler(this.menuItemToolsRefreshWholePrompt_Click);
            // 
            // menuItemToolsLoadFromDBRefreshObjectsSelection
            // 
            this.menuItemToolsLoadFromDBRefreshObjectsSelection.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemToolsRefreshSelectionAuto,
            this.menuItemToolsRefreshSelectionPrompt});
            this.menuItemToolsLoadFromDBRefreshObjectsSelection.Name = "menuItemToolsLoadFromDBRefreshObjectsSelection";
            this.menuItemToolsLoadFromDBRefreshObjectsSelection.Size = new System.Drawing.Size(155, 22);
            this.menuItemToolsLoadFromDBRefreshObjectsSelection.Text = "Selection";
            // 
            // menuItemToolsRefreshSelectionAuto
            // 
            this.menuItemToolsRefreshSelectionAuto.Name = "menuItemToolsRefreshSelectionAuto";
            this.menuItemToolsRefreshSelectionAuto.Size = new System.Drawing.Size(122, 22);
            this.menuItemToolsRefreshSelectionAuto.Text = "Automatic";
            this.menuItemToolsRefreshSelectionAuto.Click += new System.EventHandler(this.menuItemToolsRefreshSelectionAuto_Click);
            // 
            // menuItemToolsRefreshSelectionPrompt
            // 
            this.menuItemToolsRefreshSelectionPrompt.Name = "menuItemToolsRefreshSelectionPrompt";
            this.menuItemToolsRefreshSelectionPrompt.Size = new System.Drawing.Size(122, 22);
            this.menuItemToolsRefreshSelectionPrompt.Text = "Prompt";
            this.menuItemToolsRefreshSelectionPrompt.Click += new System.EventHandler(this.menuItemToolsRefreshSelectionPrompt_Click);
            // 
            // menuItemToolsLoadFromDBChangedObjectsAddIndicators
            // 
            this.menuItemToolsLoadFromDBChangedObjectsAddIndicators.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.menuItemToolsLoadFromDBChangedObjectsAddIndicators.Name = "menuItemToolsLoadFromDBChangedObjectsAddIndicators";
            this.menuItemToolsLoadFromDBChangedObjectsAddIndicators.Size = new System.Drawing.Size(164, 22);
            this.menuItemToolsLoadFromDBChangedObjectsAddIndicators.Text = "Add Indicators";
            this.menuItemToolsLoadFromDBChangedObjectsAddIndicators.Click += new System.EventHandler(this.menuItemToolsLoadFromDBChangedObjectsAddIndicators_Click);
            // 
            // menuItemToolsLoadFromDBChangedObjectsRemoveIndicators
            // 
            this.menuItemToolsLoadFromDBChangedObjectsRemoveIndicators.Name = "menuItemToolsLoadFromDBChangedObjectsRemoveIndicators";
            this.menuItemToolsLoadFromDBChangedObjectsRemoveIndicators.Size = new System.Drawing.Size(164, 22);
            this.menuItemToolsLoadFromDBChangedObjectsRemoveIndicators.Text = "Remove Indicators";
            this.menuItemToolsLoadFromDBChangedObjectsRemoveIndicators.Click += new System.EventHandler(this.menuItemToolsLoadFromDBChangedObjectsRemoveIndicators_Click);
            //
            //menuItemDatabaseServer
            //
            this.menuItemDatabaseServer.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemToolsSynchronisationManager,
            this.menuItemToolsSynchronise,
            this.menuItemToolsPermissions});
            this.menuItemDatabaseServer.MergeIndex = 6;
            this.menuItemDatabaseServer.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.menuItemDatabaseServer.Name = "menuItemDatabaseServer";
            this.menuItemDatabaseServer.Size = new System.Drawing.Size(200, 22);
            this.menuItemDatabaseServer.Text = Core.Variables.Instance.ServerProvider;
            // 
            // menuItemDBImport
            // 
            this.menuItemDBImport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemDatabaseImportExcelTemplates,
            this.menuItemDatabaseImportModules,
            this.menuItemDatabaseImportHierarchicalData,
            this.menuItemDatabaseImporteHPUM,
            this.menuItemDatabaseImportObjects});

            this.menuItemDBImport.Name = "menuItemDBImport";
            this.menuItemDBImport.MergeIndex = 3;
            this.menuItemDBImport.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.menuItemDBImport.Size = new System.Drawing.Size(177, 22);
            this.menuItemDBImport.Text = "&Import";
            // 
            // menuItemDBExport
            // 
            this.menuItemDBExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemDatabsaeExportExcel,
            this.toolStripMenuItem2,
            this.wordToolStripMenuItem,
            this.menuItemDatabaseExportAssociations});
            //this.menuItemDatabaseExportObjectContext,
            //this.menuItemDatabaseExportObjectFlow});
            this.menuItemDBExport.MergeIndex = 3;
            this.menuItemDBExport.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.menuItemDBExport.Name = "menuItemDBExport";
            this.menuItemDBExport.Size = new System.Drawing.Size(177, 22);
            this.menuItemDBExport.Text = "Export";
            // 
            // menuItemDatabaseImportExcelTemplates
            // 
            this.menuItemDatabaseImportExcelTemplates.Name = "menuItemDatabaseImportExcelTemplates";
            this.menuItemDatabaseImportExcelTemplates.Size = new System.Drawing.Size(200, 22);
            this.menuItemDatabaseImportExcelTemplates.Text = "Excel Templates";
            this.menuItemDatabaseImportExcelTemplates.Click += new System.EventHandler(this.menuItemDatabaseImportExcelTemplatesImport_Click);
            // 
            // menuItemDatabaseImportModules
            // 
            this.menuItemDatabaseImportModules.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemDatabaseImportModulesFromTextAndExcelWithMappingToolStripMenuItem});
            this.menuItemDatabaseImportModules.Name = "menuItemDatabaseImportModules";
            this.menuItemDatabaseImportModules.Size = new System.Drawing.Size(200, 22);
            this.menuItemDatabaseImportModules.Text = "Modules";
            this.menuItemDatabaseImportModules.Visible = false;
            // 
            // menuItemDatabaseImportModulesFromTextAndExcelWithMappingToolStripMenuItem
            // 
            this.menuItemDatabaseImportModulesFromTextAndExcelWithMappingToolStripMenuItem.Name = "menuItemDatabaseImportModulesFromTextAndExcelWithMappingToolStripMenuItem";
            this.menuItemDatabaseImportModulesFromTextAndExcelWithMappingToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.menuItemDatabaseImportModulesFromTextAndExcelWithMappingToolStripMenuItem.Text = "From Text and Excel with Mapping";
            this.menuItemDatabaseImportModulesFromTextAndExcelWithMappingToolStripMenuItem.Click += new System.EventHandler(this.menuItemDatabaseImportModulesFromTextAndExcelWithMappingToolStripMenuItem_Click);
            // 
            // menuItemDatabaseImporteHPUM
            // 
            this.menuItemDatabaseImporteHPUM.Name = "menuItemDatabaseImporteHPUM";
            this.menuItemDatabaseImporteHPUM.Size = new System.Drawing.Size(200, 22);
            this.menuItemDatabaseImporteHPUM.Text = "Complex Excel Template";
            this.menuItemDatabaseImporteHPUM.Click += new System.EventHandler(this.menuItemDatabaseeHPUM_Click);
            // 
            // menuItemDatabaseImportObjects
            // 
            this.menuItemDatabaseImportObjects.Name = "menuItemDatabaseImportObjects";
            this.menuItemDatabaseImportObjects.Size = new System.Drawing.Size(200, 22);
            this.menuItemDatabaseImportObjects.Text = "Text Objects";
            this.menuItemDatabaseImportObjects.Click += new System.EventHandler(this.menuItemDatabaseRMBTI_Click);
            // 
            // menuItemDatabsaeExportExcel
            // 
            this.menuItemDatabsaeExportExcel.Name = "menuItemDatabsaeExportExcel";
            this.menuItemDatabsaeExportExcel.Size = new System.Drawing.Size(148, 22);
            this.menuItemDatabsaeExportExcel.Text = "Excel Template";
            this.menuItemDatabsaeExportExcel.Click += new System.EventHandler(this.menuItemDatabaseImportExcelTemplatesGenerate_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemDatabaseExportHierarchyTabbed,
            this.menuItemDatabaseExportHierarchyNumbered,
            this.menuItemDatabaseExportHierarchyBoth});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(148, 22);
            this.toolStripMenuItem2.Text = "Text";
            // 
            // menuItemDatabaseExportHierarchyTabbed
            // 
            this.menuItemDatabaseExportHierarchyTabbed.Name = "menuItemDatabaseExportHierarchyTabbed";
            this.menuItemDatabaseExportHierarchyTabbed.Size = new System.Drawing.Size(275, 22);
            this.menuItemDatabaseExportHierarchyTabbed.Text = "Hierarchical Data (Tabbed)";
            this.menuItemDatabaseExportHierarchyTabbed.Click += new System.EventHandler(this.menuItemDatabaseExportHierarchyTabbed_Click);
            // 
            // menuItemDatabaseExportHierarchyNumbered
            // 
            this.menuItemDatabaseExportHierarchyNumbered.Name = "menuItemDatabaseExportHierarchyNumbered";
            this.menuItemDatabaseExportHierarchyNumbered.Size = new System.Drawing.Size(275, 22);
            this.menuItemDatabaseExportHierarchyNumbered.Text = "Hierarchical Data (Numbered)";
            this.menuItemDatabaseExportHierarchyNumbered.Click += new System.EventHandler(this.menuItemDatabaseExportHierarchyNumbered_Click);
            // 
            // menuItemDatabaseExportHierarchyBoth
            // 
            this.menuItemDatabaseExportHierarchyBoth.Name = "menuItemDatabaseExportHierarchyBoth";
            this.menuItemDatabaseExportHierarchyBoth.Size = new System.Drawing.Size(275, 22);
            this.menuItemDatabaseExportHierarchyBoth.Text = "Hierarchical Data (Tabbed and Numbered)";
            this.menuItemDatabaseExportHierarchyBoth.Click += new System.EventHandler(this.menuItemDatabaseExportHierarchyBoth_Click);
            // 
            // wordToolStripMenuItem
            // 
            this.wordToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            menuItemDatabaseExportHierarchyWordTabbed,
            this.menuItemDatabaseExportHierarchyWordNumbered,
            this.menuItemDatabaseExportHierarchyWordBoth});
            this.wordToolStripMenuItem.Name = "wordToolStripMenuItem";
            this.wordToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.wordToolStripMenuItem.Text = "Word";
            this.wordToolStripMenuItem.Visible = false;
            // 
            // menuItemDatabaseExportHierarchyWordNumbered
            // 
            this.menuItemDatabaseExportHierarchyWordNumbered.Name = "menuItemDatabaseExportHierarchyWordNumbered";
            this.menuItemDatabaseExportHierarchyWordNumbered.Size = new System.Drawing.Size(275, 22);
            this.menuItemDatabaseExportHierarchyWordNumbered.Text = "Hierarchical Data (Numbered)";
            this.menuItemDatabaseExportHierarchyWordNumbered.Click += new System.EventHandler(this.menuItemDatabaseExportHierarchyWordNumbered_Click);
            // 
            // menuItemDatabaseExportHierarchyWordBoth
            // 
            this.menuItemDatabaseExportHierarchyWordBoth.Name = "menuItemDatabaseExportHierarchyWordBoth";
            this.menuItemDatabaseExportHierarchyWordBoth.Size = new System.Drawing.Size(275, 22);
            this.menuItemDatabaseExportHierarchyWordBoth.Text = "Hierarchical Data (Tabbed and Numbered)";
            this.menuItemDatabaseExportHierarchyWordBoth.Click += new System.EventHandler(this.menuItemDatabaseExportHierarchyWordBoth_Click);
            // 
            // menuItemDatabaseExportAssociations
            // 
            this.menuItemDatabaseExportAssociations.Name = "menuItemDatabaseExportAssociations";
            this.menuItemDatabaseExportAssociations.Size = new System.Drawing.Size(148, 22);
            this.menuItemDatabaseExportAssociations.Text = "Associations";
            this.menuItemDatabaseExportAssociations.Click += new System.EventHandler(this.menuItemDatabaseExportAssociations_Click);
            // 
            // menuItemDatabaseExportObjectContext
            // 
            this.menuItemDatabaseExportObjectContext.Name = "menuItemDatabaseExportObjectContext";
            this.menuItemDatabaseExportObjectContext.Size = new System.Drawing.Size(148, 22);
            this.menuItemDatabaseExportObjectContext.Text = "Object Context";
            this.menuItemDatabaseExportObjectContext.Click += new System.EventHandler(this.menuItemDatabaseExportObjectContext_Click);
            // 
            // menuItemDatabaseExportObjectFlow
            // 
            this.menuItemDatabaseExportObjectFlow.Name = "menuItemDatabaseExportObjectFlow";
            this.menuItemDatabaseExportObjectFlow.Size = new System.Drawing.Size(148, 22);
            this.menuItemDatabaseExportObjectFlow.Text = "Object Flows";
            this.menuItemDatabaseExportObjectFlow.Click += new System.EventHandler(this.menuItemDatabaseExportObjectFlow_Click);
            // 
            // menuItemDBMapper
            // 
            this.menuItemDBMapper.Name = "menuItemDBMapper";
            this.menuItemDBMapper.Size = new System.Drawing.Size(177, 22);
            this.menuItemDBMapper.Text = "&Relationship Manager";
            this.menuItemDBMapper.Click += new System.EventHandler(this.menuItemDBMapper_Click);
            // 
            // menuItemDatabaseObjectManager
            // 
            this.menuItemDatabaseObjectManager.Name = "menuItemDatabaseObjectManager";
            this.menuItemDatabaseObjectManager.Size = new System.Drawing.Size(177, 22);
            this.menuItemDatabaseObjectManager.Text = "Management";
            this.menuItemDatabaseObjectManager.MergeIndex = 6;
            this.menuItemDatabaseObjectManager.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.menuItemDatabaseObjectManager.Click += new System.EventHandler(this.menuItemDatabaseObjectManager_Click_1);
            // 
            // mnuItemDatabaseSWOTAnalysis
            // 
            this.mnuItemDatabaseSWOTAnalysis.Name = "mnuItemDatabaseSWOTAnalysis";
            this.mnuItemDatabaseSWOTAnalysis.Size = new System.Drawing.Size(177, 22);
            this.mnuItemDatabaseSWOTAnalysis.Text = "SWOT Analysis";
            this.mnuItemDatabaseSWOTAnalysis.Click += new System.EventHandler(this.mnuItemDatabaseSWOTAnalysis_Click);
            // 
            // menuItemReports
            // 
            this.menuItemReports.Name = "menuItemReports";
            this.menuItemReports.Size = new System.Drawing.Size(177, 22);
            this.menuItemReports.Text = "Reports";
            this.menuItemReports.Click += new System.EventHandler(this.menuItemReports_Click);
            // 
            // menuItemDBDictionary
            // 
            this.menuItemDBDictionary.Name = "menuItemDBDictionary";
            this.menuItemDBDictionary.Size = new System.Drawing.Size(177, 22);
            this.menuItemDBDictionary.Text = "&Dictionary";
            this.menuItemDBDictionary.MergeIndex = 7;
            this.menuItemDBDictionary.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.menuItemDBDictionary.Click += new System.EventHandler(this.menuItemDBDictionary_Click);
            // 
            // menuItemWindows
            // 
            this.menuItemWindows.Name = "menuItemWindows";
            this.menuItemWindows.Size = new System.Drawing.Size(62, 20);
            this.menuItemWindows.Text = "&Windows";
            // 
            // menuItemHelp
            // 
            this.menuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemHelpQuick,
            this.menuItemHelpShortcuts,
            this.menuItemHelpAbout});
            this.menuItemHelp.Name = "menuItemHelp";
            this.menuItemHelp.Size = new System.Drawing.Size(40, 20);
            this.menuItemHelp.Text = "&Help";
            // 
            // menuItemHelpQuick
            // 
            this.menuItemHelpQuick.Name = "menuItemHelpQuick";
            this.menuItemHelpQuick.Size = new System.Drawing.Size(130, 22);
            this.menuItemHelpQuick.Text = "Quick Guide";
            this.menuItemHelpQuick.Click += new System.EventHandler(this.menuItemHelpQuick_Click);
            // 
            // menuItemHelpShortcuts
            // 
            this.menuItemHelpShortcuts.Name = "menuItemHelpShortcuts";
            this.menuItemHelpShortcuts.Size = new System.Drawing.Size(130, 22);
            this.menuItemHelpShortcuts.Text = "Shortcuts";
            this.menuItemHelpShortcuts.Click += new System.EventHandler(this.menuItemHelpShortcuts_Click);
            // 
            // menuItemHelpAbout
            // 
            this.menuItemHelpAbout.Name = "menuItemHelpAbout";
            this.menuItemHelpAbout.Size = new System.Drawing.Size(130, 22);
            this.menuItemHelpAbout.Text = "&About";
            this.menuItemHelpAbout.Click += new System.EventHandler(this.menuItemHelpAbout_Click);
            // 
            // menuItemFileWorkspaces
            // 
            this.menuItemFileWorkspaces.MergeIndex = 31;
            this.menuItemFileWorkspaces.Name = "menuItemFileWorkspaces";
            this.menuItemFileWorkspaces.Size = new System.Drawing.Size(144, 22);
            this.menuItemFileWorkspaces.Text = "Workspaces";
            // 
            // LeftToolStripPanel
            // 
            this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // ContentPanel
            // 
            this.ContentPanel.AutoScroll = true;
            this.ContentPanel.Size = new System.Drawing.Size(623, 315);
            // 
            // toolStripMain
            // 
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stripButtonNewDiagram,
            this.stripButtonOpen,
            this.stripButtonSaveAll});
            //this.stripButtonHelp
            this.toolStripMain.Location = new System.Drawing.Point(0, 24);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(623, 25);
            this.toolStripMain.TabIndex = 8;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // stripButtonNew
            // 
            this.stripButtonNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripButtonNew.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            //this.stripButtonNewDiagram,
            this.stripButtonNewMindMapDiagram,
            this.stripButtonNewSymbol});
            this.stripButtonNew.Image = ((System.Drawing.Image)(resources.GetObject("stripButtonNew.Image")));
            this.stripButtonNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripButtonNew.MergeIndex = 1;
            this.stripButtonNew.Name = "stripButtonNew";
            this.stripButtonNew.Size = new System.Drawing.Size(22, 22);
            this.stripButtonNew.Text = "&New";
            this.stripButtonNew.ButtonClick += new System.EventHandler(this.menuItemFileNewDiagram_Click);
            // 
            // stripButtonNewDiagram
            // 
            this.stripButtonNewDiagram.Name = "stripButtonNewDiagram";
            this.stripButtonNewDiagram.Image = ((System.Drawing.Image)(resources.GetObject("stripButtonNew.Image")));
            this.stripButtonNewDiagram.Size = new System.Drawing.Size(22, 22);
            this.stripButtonNewDiagram.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripButtonNewDiagram.Text = "New Diagram";
            this.stripButtonNewDiagram.Click += new System.EventHandler(this.menuItemFileNewDiagram_Click);
            // 
            // stripButtonSaveAll
            // 
            this.stripButtonSaveAll.Name = "stripButtonSaveAll";
            this.stripButtonSaveAll.Image = ((System.Drawing.Image)(resources.GetObject("saveall")));
            this.stripButtonSaveAll.Size = new System.Drawing.Size(22, 22);
            this.stripButtonSaveAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripButtonSaveAll.Text = "Save All Open Diagrams";
            this.stripButtonSaveAll.ToolTipText = this.stripButtonSaveAll.Text;
            //this.stripButtonSaveAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.stripButtonSaveAll.Click += new System.EventHandler(this.stripButtonSaveAll_Click);
            // 
            // stripButtonNewMindMapDiagram
            // 
            this.stripButtonNewMindMapDiagram.Name = "stripButtonNewMindMapDiagram";
            this.stripButtonNewMindMapDiagram.Size = new System.Drawing.Size(22, 22);
            this.stripButtonNewMindMapDiagram.Text = "New MetaMap";
            this.stripButtonNewMindMapDiagram.Click += new System.EventHandler(this.menuItemFileNewMindMapDiagram_Click);
            // 
            // stripButtonNewSymbol
            // 
            this.stripButtonNewSymbol.Name = "stripButtonNewSymbol";
            this.stripButtonNewSymbol.Size = new System.Drawing.Size(22, 22);
            this.stripButtonNewSymbol.Text = "New Symbol";
            this.stripButtonNewSymbol.Click += new System.EventHandler(this.menuItemFileNewSymbol_Click);
            // 
            // stripButtonOpen
            // 
            this.stripButtonOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripButtonOpen.Image = ((System.Drawing.Image)(resources.GetObject("stripButtonOpen.Image")));
            this.stripButtonOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripButtonOpen.MergeIndex = 2;
            this.stripButtonOpen.Name = "stripButtonOpen";
            this.stripButtonOpen.Size = new System.Drawing.Size(22, 22);
            this.stripButtonOpen.Text = "&Open";
            this.stripButtonOpen.Click += new System.EventHandler(this.menuItemFileOpen_Click);
            // 
            // stripButtonHelp
            // 
            this.stripButtonHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripButtonHelp.Image = ((System.Drawing.Image)(resources.GetObject("stripButtonHelp.Image")));
            this.stripButtonHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripButtonHelp.MergeIndex = 11;
            this.stripButtonHelp.Name = "stripButtonHelp";
            this.stripButtonHelp.Size = new System.Drawing.Size(22, 22);
            this.stripButtonHelp.Text = "He&lp";
            this.stripButtonHelp.Click += new System.EventHandler(this.menuItemHelpAbout_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.statusStrip1.ImageList = imageListWorkspaces;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolstripSaveMode,
            this.toolStripPWR,
            this.statusLabel,
            this.progressBar1,
            this.toolStripStatusCurrentWorkspace,
            this.toolStripDropDownSelectWorkspace});
            //this.toolStripStatusWorkspaceName
            this.statusStrip1.Location = new System.Drawing.Point(0, 342);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(623, 22);
            this.statusStrip1.TabIndex = 14;
            this.statusStrip1.Text = "";
            // 
            // toolstripSaveMode
            // 
            this.toolstripSaveMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToDatabaseDisabledToolStripMenuItem,
            this.saveToDatabaseEnabledToolStripMenuItem});
            this.toolstripSaveMode.Image = ((System.Drawing.Image)(resources.GetObject("toolstripSaveMode.Image")));
            this.toolstripSaveMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolstripSaveMode.Name = "toolstripSaveMode";
            this.toolstripSaveMode.Size = new System.Drawing.Size(22, 22);
            this.toolstripSaveMode.Visible = false;
            // 
            // saveToDatabaseDisabledToolStripMenuItem
            // 
            this.saveToDatabaseDisabledToolStripMenuItem.CheckOnClick = true;
            this.saveToDatabaseDisabledToolStripMenuItem.Name = "saveToDatabaseDisabledToolStripMenuItem";
            this.saveToDatabaseDisabledToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.saveToDatabaseDisabledToolStripMenuItem.Text = "Save to Database Disabled";
            this.saveToDatabaseDisabledToolStripMenuItem.Visible = false;
            this.saveToDatabaseDisabledToolStripMenuItem.Click += new System.EventHandler(this.saveToDatabaseDisabledToolStripMenuItem_Click);
            // 
            // saveToDatabaseEnabledToolStripMenuItem
            // 
            this.saveToDatabaseEnabledToolStripMenuItem.Checked = true;
            this.saveToDatabaseEnabledToolStripMenuItem.CheckOnClick = true;
            this.saveToDatabaseEnabledToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.saveToDatabaseEnabledToolStripMenuItem.Name = "saveToDatabaseEnabledToolStripMenuItem";
            this.saveToDatabaseEnabledToolStripMenuItem.Size = new System.Drawing.Size(22, 22);
            this.saveToDatabaseEnabledToolStripMenuItem.Text = "Save to Database Enabled";
            this.saveToDatabaseEnabledToolStripMenuItem.Visible = false;
            this.saveToDatabaseEnabledToolStripMenuItem.Click += new System.EventHandler(this.saveToDatabaseEnabledToolStripMenuItem_Click);
            // 
            // toolStripPWR
            // 
            this.toolStripPWR.Name = "toolStripPWR";
            this.toolStripPWR.Size = new System.Drawing.Size(66, 17);
            this.toolStripPWR.Text = "Power Mode";
            this.toolStripPWR.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolStripPWR.Visible = false;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = false;
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.statusLabel.Size = new System.Drawing.Size(150, 17);
            this.statusLabel.Text = "Ready";
            this.statusLabel.Click += new System.EventHandler(statusLabel_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusCurrentWorkspace
            // 
            this.toolStripStatusCurrentWorkspace.Name = "toolStripStatusCurrentWorkspace";
            this.toolStripStatusCurrentWorkspace.Size = new System.Drawing.Size(107, 17);
            this.toolStripStatusCurrentWorkspace.Text = "Current Workspace: ";
            // 
            // toolStripStatusWorkspaceName
            // 
            //this.toolStripStatusWorkspaceName.Name = "toolStripStatusWorkspaceName";
            //this.toolStripStatusWorkspaceName.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripDropDownSelectWorkspace
            // 
            this.toolStripDropDownSelectWorkspace.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownSelectWorkspace.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownSelectWorkspace.Image")));
            this.toolStripDropDownSelectWorkspace.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownSelectWorkspace.Name = "toolStripDropDownSelectWorkspace";
            this.toolStripDropDownSelectWorkspace.Size = new System.Drawing.Size(110, 20);
            this.toolStripDropDownSelectWorkspace.Text = "Select Current Workspace";
            this.toolStripDropDownSelectWorkspace.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(toolStripDropDownSelectWorkspace_DropDownItemClicked);
            //this.toolStripDropDownSelectWorkspace.DropDown.MinimumSize = new System.Drawing.Size(300, 22);
            //this.toolStripDropDownSelectWorkspace.DropDown.MaximumSize = new System.Drawing.Size(300, 400);
            // 
            // dockPanel1
            // 
            this.dockPanel1.ActiveAutoHideContent = null;
            this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel1.DockBackColor = System.Drawing.SystemColors.Control;
            this.dockPanel1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.dockPanel1.Location = new System.Drawing.Point(0, 49);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new System.Drawing.Size(623, 293);
            dockPanelGradient22.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient22.StartColor = System.Drawing.SystemColors.ControlLight;
            autoHideStripSkin8.DockStripGradient = dockPanelGradient22;
            tabGradient50.EndColor = System.Drawing.SystemColors.Control;
            tabGradient50.StartColor = System.Drawing.SystemColors.Control;
            tabGradient50.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            autoHideStripSkin8.TabGradient = tabGradient50;
            dockPanelSkin8.AutoHideStripSkin = autoHideStripSkin8;
            tabGradient51.EndColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient51.StartColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient51.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient8.ActiveTabGradient = tabGradient51;
            dockPanelGradient23.EndColor = System.Drawing.SystemColors.Control;
            dockPanelGradient23.StartColor = System.Drawing.SystemColors.Control;
            dockPaneStripGradient8.DockStripGradient = dockPanelGradient23;
            tabGradient52.EndColor = System.Drawing.SystemColors.ControlLight;
            tabGradient52.StartColor = System.Drawing.SystemColors.ControlLight;
            tabGradient52.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient8.InactiveTabGradient = tabGradient52;
            dockPaneStripSkin8.DocumentGradient = dockPaneStripGradient8;
            tabGradient53.EndColor = System.Drawing.SystemColors.ActiveCaption;
            tabGradient53.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient53.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
            tabGradient53.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
            dockPaneStripToolWindowGradient8.ActiveCaptionGradient = tabGradient53;
            tabGradient54.EndColor = System.Drawing.SystemColors.Control;
            tabGradient54.StartColor = System.Drawing.SystemColors.Control;
            tabGradient54.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient8.ActiveTabGradient = tabGradient54;
            dockPanelGradient24.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient24.StartColor = System.Drawing.SystemColors.ControlLight;
            dockPaneStripToolWindowGradient8.DockStripGradient = dockPanelGradient24;
            tabGradient55.EndColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient55.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient55.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient55.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient8.InactiveCaptionGradient = tabGradient55;
            tabGradient56.EndColor = System.Drawing.Color.Transparent;
            tabGradient56.StartColor = System.Drawing.Color.Transparent;
            tabGradient56.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            dockPaneStripToolWindowGradient8.InactiveTabGradient = tabGradient56;
            dockPaneStripSkin8.ToolWindowGradient = dockPaneStripToolWindowGradient8;
            dockPanelSkin8.DockPaneStripSkin = dockPaneStripSkin8;
            this.dockPanel1.Skin = dockPanelSkin8;
            this.dockPanel1.TabIndex = 15;
            // 
            // menuItemToolsExportHierarchicalData
            // 
            this.menuItemToolsExportHierarchicalData.Name = "menuItemToolsExportHierarchicalData";
            this.menuItemToolsExportHierarchicalData.Size = new System.Drawing.Size(166, 22);
            this.menuItemToolsExportHierarchicalData.Text = "Hierarchical Data";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "3floppy_unmount.png");
            this.imageList1.Images.SetKeyName(1, "camera_unmount.png");
            // 
            // m_paletteDocker
            // 
            this.m_paletteDocker.ClientSize = new System.Drawing.Size(232, 432);
            this.m_paletteDocker.DockAreas = ((MetaBuilder.Docking.DockAreas)((((MetaBuilder.Docking.DockAreas.Float | MetaBuilder.Docking.DockAreas.DockLeft)
                        | MetaBuilder.Docking.DockAreas.DockRight)
                        | MetaBuilder.Docking.DockAreas.DockBottom)));
            this.m_paletteDocker.DockPanel = null;
            this.m_paletteDocker.DockState = MetaBuilder.Docking.DockState.Unknown;
            this.m_paletteDocker.FloatPane = null;
            this.m_paletteDocker.HideOnClose = true;
            this.m_paletteDocker.IsFloat = false;
            this.m_paletteDocker.IsHidden = true;
            this.m_paletteDocker.Location = new System.Drawing.Point(44, 44);
            this.m_paletteDocker.Name = "m_paletteDocker";
            this.m_paletteDocker.Pane = null;
            this.m_paletteDocker.PanelPane = null;
            this.m_paletteDocker.TabText = "Stencils";
            this.m_paletteDocker.Text = "Stencils";
            this.m_paletteDocker.Visible = false;
            this.m_paletteDocker.VisibleState = MetaBuilder.Docking.DockState.Unknown;
            this.m_paletteDocker.DockStateChanged += new System.EventHandler(this.dockedForm_DockStateChanged);
            // 
            // m_panningWindow
            // 
            //this.m_panningWindow.ClientSize = new System.Drawing.Size(213, 143);
            //this.m_panningWindow.DockPanel = null;
            //this.m_panningWindow.DockState = MetaBuilder.Docking.DockState.Unknown;
            //this.m_panningWindow.FloatPane = null;
            //this.m_panningWindow.HideOnClose = true;
            //this.m_panningWindow.IsFloat = false;
            //this.m_panningWindow.IsHidden = true;
            //this.m_panningWindow.Location = new System.Drawing.Point(66, 66);
            //this.m_panningWindow.Name = "m_panningWindow";
            //this.m_panningWindow.Pane = null;
            //this.m_panningWindow.PanelPane = null;
            //this.m_panningWindow.TabText = "Pan and Zoom";
            //this.m_panningWindow.Text = "Navigator";
            //this.m_panningWindow.Visible = false;
            //this.m_panningWindow.VisibleState = MetaBuilder.Docking.DockState.Unknown;
            //this.m_panningWindow.DockStateChanged += new System.EventHandler(this.dockedForm_DockStateChanged);
            // 
            // m_metaObjectExplorer
            // 
            this.m_metaObjectExplorer.ClientSize = new System.Drawing.Size(211, 376);
            this.m_metaObjectExplorer.DockPanel = null;
            this.m_metaObjectExplorer.DockState = MetaBuilder.Docking.DockState.Unknown;
            this.m_metaObjectExplorer.FloatPane = null;
            this.m_metaObjectExplorer.HideOnClose = true;
            this.m_metaObjectExplorer.IsFloat = false;
            this.m_metaObjectExplorer.IsHidden = true;
            this.m_metaObjectExplorer.Location = new System.Drawing.Point(110, 110);
            this.m_metaObjectExplorer.Name = "m_metaObjectExplorer";
            this.m_metaObjectExplorer.Pane = null;
            this.m_metaObjectExplorer.PanelPane = null;
            this.m_metaObjectExplorer.TabText = "Meta Object Explorer";
            this.m_metaObjectExplorer.Text = "Meta Object Explorer";
            this.m_metaObjectExplorer.Visible = false;
            this.m_metaObjectExplorer.VisibleState = MetaBuilder.Docking.DockState.Unknown;
            this.m_metaObjectExplorer.DockStateChanged += new System.EventHandler(this.dockedForm_DockStateChanged);
            // 
            // m_metaPropsWindow
            // 
            this.m_metaPropsWindow.ClientSize = new System.Drawing.Size(211, 376);
            this.m_metaPropsWindow.DockPanel = null;
            this.m_metaPropsWindow.DockState = MetaBuilder.Docking.DockState.Unknown;
            this.m_metaPropsWindow.FloatPane = null;
            this.m_metaPropsWindow.HideOnClose = true;
            this.m_metaPropsWindow.IsFloat = false;
            this.m_metaPropsWindow.IsHidden = true;
            this.m_metaPropsWindow.Location = new System.Drawing.Point(110, 110);
            this.m_metaPropsWindow.Name = "m_metaPropsWindow";
            this.m_metaPropsWindow.Pane = null;
            this.m_metaPropsWindow.PanelPane = null;
            this.m_metaPropsWindow.SelectedObject = null;
            this.m_metaPropsWindow.TabText = "Meta Properties";
            this.m_metaPropsWindow.Text = "Meta Properties";
            this.m_metaPropsWindow.Visible = false;
            this.m_metaPropsWindow.VisibleState = MetaBuilder.Docking.DockState.Unknown;
            this.m_metaPropsWindow.DockStateChanged += new System.EventHandler(this.dockedForm_DockStateChanged);
            // 
            // m_taskDocker
            // 
            this.m_taskDocker.ClientSize = new System.Drawing.Size(292, 266);
            this.m_taskDocker.DockPanel = null;
            this.m_taskDocker.DockState = MetaBuilder.Docking.DockState.Unknown;
            this.m_taskDocker.FloatPane = null;
            this.m_taskDocker.HideOnClose = true;
            this.m_taskDocker.IsFloat = false;
            this.m_taskDocker.IsHidden = true;
            this.m_taskDocker.Location = new System.Drawing.Point(132, 132);
            this.m_taskDocker.Name = "m_taskDocker";
            this.m_taskDocker.Pane = null;
            this.m_taskDocker.PanelPane = null;
            this.m_taskDocker.TabText = "Tasks";
            this.m_taskDocker.TaskLists = ((System.Collections.Generic.Dictionary<object, System.Collections.Generic.Dictionary<object, System.Collections.Generic.List<MetaBuilder.MetaControls.Tasks.TaskBase>>>)(resources.GetObject("m_taskDocker.TaskLists")));
            this.m_taskDocker.Text = "Tasks";
            this.m_taskDocker.Visible = false;
            this.m_taskDocker.VisibleState = MetaBuilder.Docking.DockState.Unknown;
            this.m_taskDocker.DockStateChanged += new System.EventHandler(this.dockedForm_DockStateChanged);
            // 
            // imageListWorkspaces
            // 
            this.imageListWorkspaces.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListWorkspaces.ImageStream")));
            this.imageListWorkspaces.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListWorkspaces.Images.SetKeyName(0, "gnome-lockscreen.png");
            this.imageListWorkspaces.Images.SetKeyName(1, "display.png");
            // 
            // DockingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(623, 364);
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStripMain);
            this.Controls.Add(this.menuMain);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuMain;
            this.Name = "DockingForm";
            this.Text = "MetaBuilder";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.DockingForm_Load);
            this.Activated += new System.EventHandler(this.DockingForm_Activated);
            this.MdiChildActivate += new System.EventHandler(this.DockingForm_MdiChildActivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DockingForm_FormClosing);
            this.TopMost = false;
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        void DockingForm_Activated(object sender, System.EventArgs e)
        {
            //System.Console.WriteLine("Activated");
            //System.Console.WriteLine(e.ToString());
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem menuItemFile;
        private System.Windows.Forms.ToolStripMenuItem menuItemFileExit;
        private System.Windows.Forms.ToolStripMenuItem menuItemView;
        private System.Windows.Forms.ToolStripMenuItem menuItemTools; //TOOLS
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsOptions;
        private System.Windows.Forms.ToolStripMenuItem menuItemWindows;
        private System.Windows.Forms.ToolStripMenuItem menuItemHelp;
        private System.Windows.Forms.ToolStripMenuItem menuItemHelpAbout;
        private System.Windows.Forms.ToolStripMenuItem menuItemFileNew;
        private System.Windows.Forms.ToolStripMenuItem menuItemFileNewDiagram;
        private System.Windows.Forms.ToolStripMenuItem menuItemFileNewMindMapDiagram;
        private System.Windows.Forms.ToolStripMenuItem menuItemFileNewSymbol;
        private System.Windows.Forms.ToolStripMenuItem menuItemFileOpen;
        private System.Windows.Forms.ToolStripMenuItem menuItemFileOpenFolder;
        private System.Windows.Forms.ToolStripButton stripButtonOpen;
        private System.Windows.Forms.ToolStripButton stripButtonHelp;
        private System.Windows.Forms.ToolStripSplitButton stripButtonNew;
        private System.Windows.Forms.ToolStripMenuItem stripButtonNewDiagram;
        private System.Windows.Forms.ToolStripMenuItem stripButtonSaveAll;
        private System.Windows.Forms.ToolStripMenuItem stripButtonNewMindMapDiagram;
        private System.Windows.Forms.ToolStripMenuItem stripButtonNewSymbol;
        private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
        private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
        private System.Windows.Forms.ToolStripContentPanel ContentPanel;
        protected System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewStencils;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewPanAndZoom;
        public System.Windows.Forms.ToolStripMenuItem menuItemFileRecent;
        private System.Windows.Forms.ToolStripMenuItem menuItemDB;
        private System.Windows.Forms.ToolStripMenuItem menuItemDBImport;
        private System.Windows.Forms.ToolStripMenuItem menuItemDBExport;
        private System.Windows.Forms.ToolStripMenuItem menuItemDBDictionary;
        private System.Windows.Forms.ToolStripMenuItem menuItemDBMapper;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewPropertiesMeta;
        //private System.Windows.Forms.ToolStripMenuItem menuItemViewPropertiesWindowDiagramming;
        private System.Windows.Forms.ToolStripMenuItem menuItemDatabaseImportExcelTemplates;
        private System.Windows.Forms.ToolStripMenuItem menuItemDatabaseImportModules;
        private System.Windows.Forms.ToolStripMenuItem menuItemDatabaseImporteHPUM;
        private System.Windows.Forms.ToolStripMenuItem menuItemDatabaseImportObjects;
        private System.Windows.Forms.ToolStripMenuItem menuItemDatabaseImportModulesFromTextAndExcelWithMappingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsExportHierarchicalData;
        private System.Windows.Forms.ToolStripMenuItem mnuItemDatabaseSWOTAnalysis;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem menuItemDatabaseExportHierarchyTabbed;
        private System.Windows.Forms.ToolStripMenuItem menuItemDatabaseExportHierarchyNumbered;
        private System.Windows.Forms.ToolStripMenuItem menuItemDatabaseExportHierarchyBoth;
        private System.Windows.Forms.ToolStripMenuItem wordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItemDatabaseExportHierarchyWordNumbered;
        private System.Windows.Forms.ToolStripMenuItem menuItemDatabaseExportHierarchyWordBoth;
        private System.Windows.Forms.ToolStripMenuItem menuItemDatabsaeExportExcel;
        private System.Windows.Forms.ToolStripMenuItem menuItemDatabaseObjectManager;
        public System.Windows.Forms.ToolStripProgressBar progressBar1;
        public System.Windows.Forms.ToolStripStatusLabel statusLabel;
        internal MetaBuilder.Docking.DockPanel dockPanel1;
        private System.Windows.Forms.ToolStripMenuItem menuItemDatabaseExportAssociations;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsPermissions;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsSynchronise;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsSynchronisationManager;
        private System.Windows.Forms.ToolStripMenuItem menuItemFileWorkspaces;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusCurrentWorkspace;
        //public System.Windows.Forms.ToolStripStatusLabel toolStripStatusWorkspaceName;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsDocumentTree;
        private System.Windows.Forms.ToolStripMenuItem menuItemDatabaseExportObjectFlow;
        private System.Windows.Forms.ToolStripMenuItem menuItemDatabaseExportObjectContext;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewTaskList;
        private System.Windows.Forms.ToolStripDropDownButton toolstripSaveMode;
        private System.Windows.Forms.ToolStripMenuItem saveToDatabaseDisabledToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToDatabaseEnabledToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripPWR;
        private System.Windows.Forms.ToolStripMenuItem menuItemReports;
        private System.Windows.Forms.ToolStripMenuItem removeHighlightsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewObjectExplorer;
        private System.Windows.Forms.ToolStripMenuItem menuItemDatabaseImportHierarchicalData;
        private System.Windows.Forms.ToolStripMenuItem menuItemDatabaseExportHierarchyWordTabbed;

        public PaletteDocker m_paletteDocker;
        //public PanningDocker m_panningWindow;
        public Tools.Explorer.MetaObjectExplorer m_metaObjectExplorer;
        public PropertyGridDocker m_propertyGridWindow;
        public MetaPropertyGridDocker m_metaPropsWindow;
        public TaskDocker m_taskDocker;
        //public HelpDocker m_helpDocker;
        private System.Windows.Forms.ToolStripMenuItem menuItemHelpShortcuts;
        private System.Windows.Forms.ToolStripMenuItem menuItemHelpQuick;

        public System.Windows.Forms.HelpProvider HelpProvider1;
        public System.Windows.Forms.ToolStripDropDownButton toolStripDropDownSelectWorkspace;
        private System.Windows.Forms.ImageList imageListWorkspaces;

        private System.Windows.Forms.ToolStripMenuItem menuItemToolsMode;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsModeClient;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsModeDeveloper;

        private System.Windows.Forms.ToolStripMenuItem menuItemDatabaseServer;

        private System.Windows.Forms.ToolStripMenuItem menuItemToolsLoadFromDatabase;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsLoadFromDBRefreshObjects;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsLoadFromDatabaseSelectObjects;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsLoadFromDBRefreshObjectsSelection;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsLoadFromDBRefreshObjectsWholeDocument;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsLoadFromDBChangedObjects;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsLoadFromDBChangedObjectsAddIndicators;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsLoadFromDBChangedObjectsRemoveIndicators;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsLoadFromDatabaseArtefactObject;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsRefreshWholeAuto;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsRefreshWholePrompt;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsRefreshSelectionAuto;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsRefreshSelectionPrompt;

    }
}