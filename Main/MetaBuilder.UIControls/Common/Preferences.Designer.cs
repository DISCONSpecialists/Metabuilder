namespace MetaBuilder.UIControls.Common
{
    partial class Preferences
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("General");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Canvas and Grid");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Data Validation");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Printing");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Diagramming", new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode3,
            treeNode4});
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Stencil Images");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Filters");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Database");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Advanced", new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode7,
            treeNode8});
            this.tabPreferences = new System.Windows.Forms.TabControl();
            this.tpgPrinting = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbPrintComments = new System.Windows.Forms.CheckBox();
            this.cbPrintSourceControlIndicators = new System.Windows.Forms.CheckBox();
            this.cbPrintArtefactPointers = new System.Windows.Forms.CheckBox();
            this.cbDisplayImageNodeClassName = new System.Windows.Forms.CheckBox();
            this.tpgGeneral = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.tab_General_Paths_SymbolsText = new System.Windows.Forms.TextBox();
            this.tab_General_Paths_DiagramsText = new System.Windows.Forms.TextBox();
            this.tab_General_Paths_ExportsText = new System.Windows.Forms.TextBox();
            this.tab_General_Paths_SourceText = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.checkBoxGeneralSaveOnCreate = new System.Windows.Forms.CheckBox();
            this.checkBoxGeneralVerboseLogging = new System.Windows.Forms.CheckBox();
            this.tab_General_groupBoxUserInfo = new System.Windows.Forms.GroupBox();
            this.tab_General_lblCompany = new System.Windows.Forms.Label();
            this.tab_General_txtInitials = new System.Windows.Forms.TextBox();
            this.tab_General_lblInitials = new System.Windows.Forms.Label();
            this.tab_General_lblFullname = new System.Windows.Forms.Label();
            this.tab_General_txtCompany = new System.Windows.Forms.TextBox();
            this.tab_General_txtFullname = new System.Windows.Forms.TextBox();
            this.tpgSave = new System.Windows.Forms.TabPage();
            this.tab_Save_groupBoxSaveOptions = new System.Windows.Forms.GroupBox();
            this.tab_Save_cbPromptForDocumentPropertiesOnFirstSave = new System.Windows.Forms.CheckBox();
            this.tab_Save_cbAutoCheckoutAllObjects = new System.Windows.Forms.CheckBox();
            this.tab_Save_cbPromptToPurgeMinorVersions = new System.Windows.Forms.CheckBox();
            this.tab_Save_cbAutoSaveDocuments = new System.Windows.Forms.CheckBox();
            this.tab_Save_lblAutoSaveMinutes = new System.Windows.Forms.Label();
            this.tab_Save_numericAutoSaveInterval = new System.Windows.Forms.NumericUpDown();
            this.tpgView = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.tab_View_numericDiagramHeight = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.tab_View_numericDiagramWidth = new System.Windows.Forms.NumericUpDown();
            this.cbUseShallowCopyColor = new System.Windows.Forms.CheckBox();
            this.tab_View_comboDefaultToPort = new System.Windows.Forms.ComboBox();
            this.tab_View_comboDefaultFromPort = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cbUseQuickPanel = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.numAutoSaveInterval = new System.Windows.Forms.NumericUpDown();
            this.cbAutoSave = new System.Windows.Forms.CheckBox();
            this.tab_View_GridOptions = new System.Windows.Forms.GroupBox();
            this.tab_View_lblSmoothingMode = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tab_View_lblGridCellSize = new System.Windows.Forms.Label();
            this.tab_View_cbShowGrid = new System.Windows.Forms.CheckBox();
            this.numArrowMoveSmall = new System.Windows.Forms.NumericUpDown();
            this.numArrowMoveLarge = new System.Windows.Forms.NumericUpDown();
            this.tab_View_numericGridCellSize = new System.Windows.Forms.NumericUpDown();
            this.cbSnapResize = new System.Windows.Forms.CheckBox();
            this.tab_View_cbSnapToGrid = new System.Windows.Forms.CheckBox();
            this.tab_View_comboGridSmoothing = new System.Windows.Forms.ComboBox();
            this.tpgAutocorrect = new System.Windows.Forms.TabPage();
            this.tab_AutoCorrect_lblNotImplemented = new System.Windows.Forms.Label();
            this.tab_AutoCorrect_IgnoreInternetAndFileAddresses = new System.Windows.Forms.CheckBox();
            this.tab_AutoCorrect_cbIgnoreWordsWithNumbers = new System.Windows.Forms.CheckBox();
            this.tab_AutoCorrect_cbIgnoreWordsInUppercase = new System.Windows.Forms.CheckBox();
            this.tab_AutoCorrect_cbAlwaysSuggestCorrections = new System.Windows.Forms.CheckBox();
            this.tab_AutoCorrect_cbHideSpellingErrorsInThisDocument = new System.Windows.Forms.CheckBox();
            this.tab_AutoCorrect_cbCheckSpellingAsYouType = new System.Windows.Forms.CheckBox();
            this.tab_AutoCorrect_comboSelectDictionary = new System.Windows.Forms.ComboBox();
            this.tab_AutoCorrect_cbEnableAutocorrect = new System.Windows.Forms.CheckBox();
            this.tpgDatabase = new System.Windows.Forms.TabPage();
            this.buttonBindImages = new MetaBuilder.MetaControls.MetaButton();
            this.groupBoxViews = new System.Windows.Forms.GroupBox();
            this.label24 = new System.Windows.Forms.Label();
            this.buttonRebuildViews = new MetaBuilder.MetaControls.MetaButton();
            this.tab_Database_lblSyncServerConnectionString = new MetaBuilder.MetaControls.MetaButton();
            this.tab_Database_btnLocalInstanceConnectionString = new MetaBuilder.MetaControls.MetaButton();
            this.tab_Database_txtSyncServerConnectionString = new System.Windows.Forms.TextBox();
            this.tab_Database_txtLocalInstanceConnectionString = new System.Windows.Forms.TextBox();
            this.tab_Database_lblSyncServer = new System.Windows.Forms.Label();
            this.tab_Database_lblLocalInstance = new System.Windows.Forms.Label();
            this.tpgValidation = new System.Windows.Forms.TabPage();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label19 = new System.Windows.Forms.Label();
            this.comboBoxMergeStrategy = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cbCustomSuggestion = new System.Windows.Forms.CheckBox();
            this.cbIntellisense = new System.Windows.Forms.CheckBox();
            this.cbSpellChecker = new System.Windows.Forms.CheckBox();
            this.gpRefresh = new System.Windows.Forms.GroupBox();
            this.radioRefreshNever = new System.Windows.Forms.RadioButton();
            this.radioRefreshPrompt = new System.Windows.Forms.RadioButton();
            this.radioRefreshAutomatic = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.radioCurrentWorkspace = new System.Windows.Forms.RadioButton();
            this.cbCheckDuplicatesWhileDiagramming = new System.Windows.Forms.CheckBox();
            this.radioAcrossWorkspaces = new System.Windows.Forms.RadioButton();
            this.tpgStencilImages = new System.Windows.Forms.TabPage();
            this.treeViewImages = new System.Windows.Forms.TreeView();
            this.checkedListBoxStencilImage = new System.Windows.Forms.CheckedListBox();
            this.label23 = new System.Windows.Forms.Label();
            this.buttonStencilImagesBrowse = new MetaBuilder.MetaControls.MetaButton();
            this.label21 = new System.Windows.Forms.Label();
            this.pictureBoxStencilImagesImage = new System.Windows.Forms.PictureBox();
            this.label18 = new System.Windows.Forms.Label();
            this.tabPageFilters = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.checkBoxCopyFilter = new System.Windows.Forms.CheckBox();
            this.buttonNew = new MetaBuilder.MetaControls.MetaButton();
            this.textBoxNewFilter = new System.Windows.Forms.TextBox();
            this.comboBoxFilters = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.buttonDelete = new MetaBuilder.MetaControls.MetaButton();
            this.listBoxFilters = new System.Windows.Forms.ListBox();
            this.tvSettings = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOK = new MetaBuilder.MetaControls.MetaButton();
            this.btnCancel = new MetaBuilder.MetaControls.MetaButton();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.cbCompareLinks = new System.Windows.Forms.CheckBox();
            this.tabPreferences.SuspendLayout();
            this.tpgPrinting.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tpgGeneral.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tab_General_groupBoxUserInfo.SuspendLayout();
            this.tpgSave.SuspendLayout();
            this.tab_Save_groupBoxSaveOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tab_Save_numericAutoSaveInterval)).BeginInit();
            this.tpgView.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tab_View_numericDiagramHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tab_View_numericDiagramWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAutoSaveInterval)).BeginInit();
            this.tab_View_GridOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numArrowMoveSmall)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numArrowMoveLarge)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tab_View_numericGridCellSize)).BeginInit();
            this.tpgAutocorrect.SuspendLayout();
            this.tpgDatabase.SuspendLayout();
            this.groupBoxViews.SuspendLayout();
            this.tpgValidation.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.gpRefresh.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tpgStencilImages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStencilImagesImage)).BeginInit();
            this.tabPageFilters.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabPreferences
            // 
            this.tabPreferences.Controls.Add(this.tpgPrinting);
            this.tabPreferences.Controls.Add(this.tpgGeneral);
            this.tabPreferences.Controls.Add(this.tpgSave);
            this.tabPreferences.Controls.Add(this.tpgView);
            this.tabPreferences.Controls.Add(this.tpgAutocorrect);
            this.tabPreferences.Controls.Add(this.tpgDatabase);
            this.tabPreferences.Controls.Add(this.tpgValidation);
            this.tabPreferences.Controls.Add(this.tpgStencilImages);
            this.tabPreferences.Controls.Add(this.tabPageFilters);
            this.tabPreferences.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPreferences.ItemSize = new System.Drawing.Size(0, 1);
            this.tabPreferences.Location = new System.Drawing.Point(146, 0);
            this.tabPreferences.Multiline = true;
            this.tabPreferences.Name = "tabPreferences";
            this.tabPreferences.Padding = new System.Drawing.Point(0, 0);
            this.tabPreferences.SelectedIndex = 0;
            this.tabPreferences.Size = new System.Drawing.Size(468, 305);
            this.tabPreferences.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabPreferences.TabIndex = 3;
            // 
            // tpgPrinting
            // 
            this.tpgPrinting.Controls.Add(this.groupBox3);
            this.tpgPrinting.Controls.Add(this.cbDisplayImageNodeClassName);
            this.tpgPrinting.Location = new System.Drawing.Point(4, 5);
            this.tpgPrinting.Name = "tpgPrinting";
            this.tpgPrinting.Size = new System.Drawing.Size(460, 296);
            this.tpgPrinting.TabIndex = 17;
            this.tpgPrinting.Text = "Printing";
            this.tpgPrinting.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbPrintComments);
            this.groupBox3.Controls.Add(this.cbPrintSourceControlIndicators);
            this.groupBox3.Controls.Add(this.cbPrintArtefactPointers);
            this.groupBox3.Location = new System.Drawing.Point(3, 7);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 92);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Printing Preferences";
            // 
            // cbPrintComments
            // 
            this.cbPrintComments.AutoSize = true;
            this.cbPrintComments.Location = new System.Drawing.Point(6, 19);
            this.cbPrintComments.Name = "cbPrintComments";
            this.cbPrintComments.Size = new System.Drawing.Size(99, 17);
            this.cbPrintComments.TabIndex = 0;
            this.cbPrintComments.Text = "Print Comments";
            this.cbPrintComments.UseVisualStyleBackColor = true;
            // 
            // cbPrintSourceControlIndicators
            // 
            this.cbPrintSourceControlIndicators.AutoSize = true;
            this.cbPrintSourceControlIndicators.Location = new System.Drawing.Point(6, 65);
            this.cbPrintSourceControlIndicators.Name = "cbPrintSourceControlIndicators";
            this.cbPrintSourceControlIndicators.Size = new System.Drawing.Size(169, 17);
            this.cbPrintSourceControlIndicators.TabIndex = 0;
            this.cbPrintSourceControlIndicators.Text = "Print Source Control Indicators";
            this.cbPrintSourceControlIndicators.UseVisualStyleBackColor = true;
            // 
            // cbPrintArtefactPointers
            // 
            this.cbPrintArtefactPointers.AutoSize = true;
            this.cbPrintArtefactPointers.Location = new System.Drawing.Point(6, 42);
            this.cbPrintArtefactPointers.Name = "cbPrintArtefactPointers";
            this.cbPrintArtefactPointers.Size = new System.Drawing.Size(128, 17);
            this.cbPrintArtefactPointers.TabIndex = 0;
            this.cbPrintArtefactPointers.Text = "Print Artefact Pointers";
            this.cbPrintArtefactPointers.UseVisualStyleBackColor = true;
            // 
            // cbDisplayImageNodeClassName
            // 
            this.cbDisplayImageNodeClassName.AutoSize = true;
            this.cbDisplayImageNodeClassName.Location = new System.Drawing.Point(9, 105);
            this.cbDisplayImageNodeClassName.Name = "cbDisplayImageNodeClassName";
            this.cbDisplayImageNodeClassName.Size = new System.Drawing.Size(272, 17);
            this.cbDisplayImageNodeClassName.TabIndex = 0;
            this.cbDisplayImageNodeClassName.Text = "Display Image Node\'s Class Name Above The Node";
            this.cbDisplayImageNodeClassName.UseVisualStyleBackColor = true;
            // 
            // tpgGeneral
            // 
            this.tpgGeneral.Controls.Add(this.groupBox6);
            this.tpgGeneral.Controls.Add(this.tab_General_Paths_SourceText);
            this.tpgGeneral.Controls.Add(this.label15);
            this.tpgGeneral.Controls.Add(this.checkBoxGeneralSaveOnCreate);
            this.tpgGeneral.Controls.Add(this.checkBoxGeneralVerboseLogging);
            this.tpgGeneral.Controls.Add(this.tab_General_groupBoxUserInfo);
            this.tpgGeneral.Location = new System.Drawing.Point(4, 5);
            this.tpgGeneral.Name = "tpgGeneral";
            this.tpgGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tpgGeneral.Size = new System.Drawing.Size(460, 296);
            this.tpgGeneral.TabIndex = 0;
            this.tpgGeneral.Text = "General";
            this.tpgGeneral.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label9);
            this.groupBox6.Controls.Add(this.label10);
            this.groupBox6.Controls.Add(this.label14);
            this.groupBox6.Controls.Add(this.tab_General_Paths_SymbolsText);
            this.groupBox6.Controls.Add(this.tab_General_Paths_DiagramsText);
            this.groupBox6.Controls.Add(this.tab_General_Paths_ExportsText);
            this.groupBox6.Location = new System.Drawing.Point(7, 105);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(438, 99);
            this.groupBox6.TabIndex = 24;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Default paths";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(51, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Diagrams";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 45);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(42, 13);
            this.label10.TabIndex = 17;
            this.label10.Text = "Exports";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 71);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(46, 13);
            this.label14.TabIndex = 18;
            this.label14.Text = "Symbols";
            // 
            // tab_General_Paths_SymbolsText
            // 
            this.tab_General_Paths_SymbolsText.Location = new System.Drawing.Point(94, 68);
            this.tab_General_Paths_SymbolsText.Name = "tab_General_Paths_SymbolsText";
            this.tab_General_Paths_SymbolsText.Size = new System.Drawing.Size(338, 20);
            this.tab_General_Paths_SymbolsText.TabIndex = 22;
            this.tab_General_Paths_SymbolsText.Tag = "Symbols";
            this.tab_General_Paths_SymbolsText.Click += new System.EventHandler(this.textBoxPathClicked);
            // 
            // tab_General_Paths_DiagramsText
            // 
            this.tab_General_Paths_DiagramsText.Location = new System.Drawing.Point(94, 16);
            this.tab_General_Paths_DiagramsText.Name = "tab_General_Paths_DiagramsText";
            this.tab_General_Paths_DiagramsText.Size = new System.Drawing.Size(338, 20);
            this.tab_General_Paths_DiagramsText.TabIndex = 21;
            this.tab_General_Paths_DiagramsText.Tag = "Diagrams";
            this.tab_General_Paths_DiagramsText.Click += new System.EventHandler(this.textBoxPathClicked);
            // 
            // tab_General_Paths_ExportsText
            // 
            this.tab_General_Paths_ExportsText.Location = new System.Drawing.Point(94, 42);
            this.tab_General_Paths_ExportsText.Name = "tab_General_Paths_ExportsText";
            this.tab_General_Paths_ExportsText.Size = new System.Drawing.Size(338, 20);
            this.tab_General_Paths_ExportsText.TabIndex = 23;
            this.tab_General_Paths_ExportsText.Tag = "Exports";
            this.tab_General_Paths_ExportsText.Click += new System.EventHandler(this.textBoxPathClicked);
            // 
            // tab_General_Paths_SourceText
            // 
            this.tab_General_Paths_SourceText.Location = new System.Drawing.Point(107, 270);
            this.tab_General_Paths_SourceText.Name = "tab_General_Paths_SourceText";
            this.tab_General_Paths_SourceText.Size = new System.Drawing.Size(338, 20);
            this.tab_General_Paths_SourceText.TabIndex = 20;
            this.tab_General_Paths_SourceText.Tag = "Source Files";
            this.tab_General_Paths_SourceText.Visible = false;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(9, 273);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(65, 13);
            this.label15.TabIndex = 19;
            this.label15.Text = "Source Files";
            this.label15.Visible = false;
            // 
            // checkBoxGeneralSaveOnCreate
            // 
            this.checkBoxGeneralSaveOnCreate.AutoSize = true;
            this.checkBoxGeneralSaveOnCreate.Location = new System.Drawing.Point(16, 233);
            this.checkBoxGeneralSaveOnCreate.Name = "checkBoxGeneralSaveOnCreate";
            this.checkBoxGeneralSaveOnCreate.Size = new System.Drawing.Size(256, 17);
            this.checkBoxGeneralSaveOnCreate.TabIndex = 15;
            this.checkBoxGeneralSaveOnCreate.Text = "Save Object and Links When They Are Updated";
            this.checkBoxGeneralSaveOnCreate.UseVisualStyleBackColor = true;
            // 
            // checkBoxGeneralVerboseLogging
            // 
            this.checkBoxGeneralVerboseLogging.AutoSize = true;
            this.checkBoxGeneralVerboseLogging.Location = new System.Drawing.Point(16, 210);
            this.checkBoxGeneralVerboseLogging.Name = "checkBoxGeneralVerboseLogging";
            this.checkBoxGeneralVerboseLogging.Size = new System.Drawing.Size(106, 17);
            this.checkBoxGeneralVerboseLogging.TabIndex = 15;
            this.checkBoxGeneralVerboseLogging.Text = "Verbose Logging";
            this.checkBoxGeneralVerboseLogging.UseVisualStyleBackColor = true;
            // 
            // tab_General_groupBoxUserInfo
            // 
            this.tab_General_groupBoxUserInfo.Controls.Add(this.tab_General_lblCompany);
            this.tab_General_groupBoxUserInfo.Controls.Add(this.tab_General_txtInitials);
            this.tab_General_groupBoxUserInfo.Controls.Add(this.tab_General_lblInitials);
            this.tab_General_groupBoxUserInfo.Controls.Add(this.tab_General_lblFullname);
            this.tab_General_groupBoxUserInfo.Controls.Add(this.tab_General_txtCompany);
            this.tab_General_groupBoxUserInfo.Controls.Add(this.tab_General_txtFullname);
            this.tab_General_groupBoxUserInfo.Location = new System.Drawing.Point(7, 7);
            this.tab_General_groupBoxUserInfo.Name = "tab_General_groupBoxUserInfo";
            this.tab_General_groupBoxUserInfo.Size = new System.Drawing.Size(438, 92);
            this.tab_General_groupBoxUserInfo.TabIndex = 14;
            this.tab_General_groupBoxUserInfo.TabStop = false;
            this.tab_General_groupBoxUserInfo.Text = "User Information";
            // 
            // tab_General_lblCompany
            // 
            this.tab_General_lblCompany.AutoSize = true;
            this.tab_General_lblCompany.Location = new System.Drawing.Point(6, 43);
            this.tab_General_lblCompany.Name = "tab_General_lblCompany";
            this.tab_General_lblCompany.Size = new System.Drawing.Size(51, 13);
            this.tab_General_lblCompany.TabIndex = 19;
            this.tab_General_lblCompany.Text = "Company";
            // 
            // tab_General_txtInitials
            // 
            this.tab_General_txtInitials.Location = new System.Drawing.Point(80, 66);
            this.tab_General_txtInitials.Name = "tab_General_txtInitials";
            this.tab_General_txtInitials.Size = new System.Drawing.Size(72, 20);
            this.tab_General_txtInitials.TabIndex = 4;
            this.tab_General_txtInitials.Visible = false;
            // 
            // tab_General_lblInitials
            // 
            this.tab_General_lblInitials.AutoSize = true;
            this.tab_General_lblInitials.Location = new System.Drawing.Point(6, 68);
            this.tab_General_lblInitials.Name = "tab_General_lblInitials";
            this.tab_General_lblInitials.Size = new System.Drawing.Size(36, 13);
            this.tab_General_lblInitials.TabIndex = 18;
            this.tab_General_lblInitials.Text = "Initials";
            this.tab_General_lblInitials.Visible = false;
            // 
            // tab_General_lblFullname
            // 
            this.tab_General_lblFullname.AutoSize = true;
            this.tab_General_lblFullname.Location = new System.Drawing.Point(6, 19);
            this.tab_General_lblFullname.Name = "tab_General_lblFullname";
            this.tab_General_lblFullname.Size = new System.Drawing.Size(49, 13);
            this.tab_General_lblFullname.TabIndex = 18;
            this.tab_General_lblFullname.Text = "Fullname";
            // 
            // tab_General_txtCompany
            // 
            this.tab_General_txtCompany.Location = new System.Drawing.Point(80, 40);
            this.tab_General_txtCompany.Name = "tab_General_txtCompany";
            this.tab_General_txtCompany.Size = new System.Drawing.Size(200, 20);
            this.tab_General_txtCompany.TabIndex = 5;
            this.tab_General_txtCompany.Text = "My Company";
            // 
            // tab_General_txtFullname
            // 
            this.tab_General_txtFullname.Location = new System.Drawing.Point(80, 16);
            this.tab_General_txtFullname.Name = "tab_General_txtFullname";
            this.tab_General_txtFullname.Size = new System.Drawing.Size(200, 20);
            this.tab_General_txtFullname.TabIndex = 3;
            this.tab_General_txtFullname.Text = "My Name";
            // 
            // tpgSave
            // 
            this.tpgSave.Controls.Add(this.tab_Save_groupBoxSaveOptions);
            this.tpgSave.Location = new System.Drawing.Point(4, 5);
            this.tpgSave.Name = "tpgSave";
            this.tpgSave.Size = new System.Drawing.Size(460, 296);
            this.tpgSave.TabIndex = 16;
            this.tpgSave.Text = "Save";
            this.tpgSave.UseVisualStyleBackColor = true;
            // 
            // tab_Save_groupBoxSaveOptions
            // 
            this.tab_Save_groupBoxSaveOptions.Controls.Add(this.tab_Save_cbPromptForDocumentPropertiesOnFirstSave);
            this.tab_Save_groupBoxSaveOptions.Controls.Add(this.tab_Save_cbAutoCheckoutAllObjects);
            this.tab_Save_groupBoxSaveOptions.Controls.Add(this.tab_Save_cbPromptToPurgeMinorVersions);
            this.tab_Save_groupBoxSaveOptions.Controls.Add(this.tab_Save_cbAutoSaveDocuments);
            this.tab_Save_groupBoxSaveOptions.Controls.Add(this.tab_Save_lblAutoSaveMinutes);
            this.tab_Save_groupBoxSaveOptions.Controls.Add(this.tab_Save_numericAutoSaveInterval);
            this.tab_Save_groupBoxSaveOptions.Location = new System.Drawing.Point(8, 4);
            this.tab_Save_groupBoxSaveOptions.Name = "tab_Save_groupBoxSaveOptions";
            this.tab_Save_groupBoxSaveOptions.Size = new System.Drawing.Size(408, 188);
            this.tab_Save_groupBoxSaveOptions.TabIndex = 19;
            this.tab_Save_groupBoxSaveOptions.TabStop = false;
            this.tab_Save_groupBoxSaveOptions.Text = "Save Options";
            // 
            // tab_Save_cbPromptForDocumentPropertiesOnFirstSave
            // 
            this.tab_Save_cbPromptForDocumentPropertiesOnFirstSave.AutoSize = true;
            this.tab_Save_cbPromptForDocumentPropertiesOnFirstSave.Checked = true;
            this.tab_Save_cbPromptForDocumentPropertiesOnFirstSave.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tab_Save_cbPromptForDocumentPropertiesOnFirstSave.Location = new System.Drawing.Point(8, 16);
            this.tab_Save_cbPromptForDocumentPropertiesOnFirstSave.Name = "tab_Save_cbPromptForDocumentPropertiesOnFirstSave";
            this.tab_Save_cbPromptForDocumentPropertiesOnFirstSave.Size = new System.Drawing.Size(241, 17);
            this.tab_Save_cbPromptForDocumentPropertiesOnFirstSave.TabIndex = 19;
            this.tab_Save_cbPromptForDocumentPropertiesOnFirstSave.Text = "Prompt for Document Properties on First Save";
            this.tab_Save_cbPromptForDocumentPropertiesOnFirstSave.UseVisualStyleBackColor = true;
            // 
            // tab_Save_cbAutoCheckoutAllObjects
            // 
            this.tab_Save_cbAutoCheckoutAllObjects.AutoSize = true;
            this.tab_Save_cbAutoCheckoutAllObjects.Checked = true;
            this.tab_Save_cbAutoCheckoutAllObjects.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tab_Save_cbAutoCheckoutAllObjects.Location = new System.Drawing.Point(8, 88);
            this.tab_Save_cbAutoCheckoutAllObjects.Name = "tab_Save_cbAutoCheckoutAllObjects";
            this.tab_Save_cbAutoCheckoutAllObjects.Size = new System.Drawing.Size(312, 17);
            this.tab_Save_cbAutoCheckoutAllObjects.TabIndex = 18;
            this.tab_Save_cbAutoCheckoutAllObjects.Text = "Auto-Checkout All Objects When Checking Out A Document";
            this.tab_Save_cbAutoCheckoutAllObjects.UseVisualStyleBackColor = true;
            // 
            // tab_Save_cbPromptToPurgeMinorVersions
            // 
            this.tab_Save_cbPromptToPurgeMinorVersions.AutoSize = true;
            this.tab_Save_cbPromptToPurgeMinorVersions.Checked = true;
            this.tab_Save_cbPromptToPurgeMinorVersions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tab_Save_cbPromptToPurgeMinorVersions.Location = new System.Drawing.Point(8, 64);
            this.tab_Save_cbPromptToPurgeMinorVersions.Name = "tab_Save_cbPromptToPurgeMinorVersions";
            this.tab_Save_cbPromptToPurgeMinorVersions.Size = new System.Drawing.Size(174, 17);
            this.tab_Save_cbPromptToPurgeMinorVersions.TabIndex = 16;
            this.tab_Save_cbPromptToPurgeMinorVersions.Text = "Prompt to Purge Minor Versions";
            this.tab_Save_cbPromptToPurgeMinorVersions.UseVisualStyleBackColor = true;
            // 
            // tab_Save_cbAutoSaveDocuments
            // 
            this.tab_Save_cbAutoSaveDocuments.AutoSize = true;
            this.tab_Save_cbAutoSaveDocuments.Checked = true;
            this.tab_Save_cbAutoSaveDocuments.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tab_Save_cbAutoSaveDocuments.Location = new System.Drawing.Point(8, 40);
            this.tab_Save_cbAutoSaveDocuments.Name = "tab_Save_cbAutoSaveDocuments";
            this.tab_Save_cbAutoSaveDocuments.Size = new System.Drawing.Size(160, 17);
            this.tab_Save_cbAutoSaveDocuments.TabIndex = 3;
            this.tab_Save_cbAutoSaveDocuments.Text = "Auto-Save documents every";
            this.tab_Save_cbAutoSaveDocuments.UseVisualStyleBackColor = true;
            // 
            // tab_Save_lblAutoSaveMinutes
            // 
            this.tab_Save_lblAutoSaveMinutes.AutoSize = true;
            this.tab_Save_lblAutoSaveMinutes.Location = new System.Drawing.Point(220, 44);
            this.tab_Save_lblAutoSaveMinutes.Name = "tab_Save_lblAutoSaveMinutes";
            this.tab_Save_lblAutoSaveMinutes.Size = new System.Drawing.Size(43, 13);
            this.tab_Save_lblAutoSaveMinutes.TabIndex = 7;
            this.tab_Save_lblAutoSaveMinutes.Text = "minutes";
            // 
            // tab_Save_numericAutoSaveInterval
            // 
            this.tab_Save_numericAutoSaveInterval.Location = new System.Drawing.Point(168, 40);
            this.tab_Save_numericAutoSaveInterval.Name = "tab_Save_numericAutoSaveInterval";
            this.tab_Save_numericAutoSaveInterval.Size = new System.Drawing.Size(48, 20);
            this.tab_Save_numericAutoSaveInterval.TabIndex = 15;
            // 
            // tpgView
            // 
            this.tpgView.Controls.Add(this.groupBox2);
            this.tpgView.Controls.Add(this.cbUseShallowCopyColor);
            this.tpgView.Controls.Add(this.tab_View_comboDefaultToPort);
            this.tpgView.Controls.Add(this.tab_View_comboDefaultFromPort);
            this.tpgView.Controls.Add(this.label7);
            this.tpgView.Controls.Add(this.label6);
            this.tpgView.Controls.Add(this.cbUseQuickPanel);
            this.tpgView.Controls.Add(this.label5);
            this.tpgView.Controls.Add(this.numAutoSaveInterval);
            this.tpgView.Controls.Add(this.cbAutoSave);
            this.tpgView.Controls.Add(this.tab_View_GridOptions);
            this.tpgView.Location = new System.Drawing.Point(4, 5);
            this.tpgView.Name = "tpgView";
            this.tpgView.Size = new System.Drawing.Size(460, 296);
            this.tpgView.TabIndex = 12;
            this.tpgView.Text = "View";
            this.tpgView.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.tab_View_numericDiagramHeight);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.tab_View_numericDiagramWidth);
            this.groupBox2.Location = new System.Drawing.Point(4, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(438, 40);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Default New Diagram Size in pixels";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(219, 18);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(38, 13);
            this.label17.TabIndex = 10;
            this.label17.Text = "Height";
            // 
            // tab_View_numericDiagramHeight
            // 
            this.tab_View_numericDiagramHeight.Location = new System.Drawing.Point(327, 16);
            this.tab_View_numericDiagramHeight.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.tab_View_numericDiagramHeight.Minimum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.tab_View_numericDiagramHeight.Name = "tab_View_numericDiagramHeight";
            this.tab_View_numericDiagramHeight.Size = new System.Drawing.Size(99, 20);
            this.tab_View_numericDiagramHeight.TabIndex = 9;
            this.tab_View_numericDiagramHeight.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 18);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(35, 13);
            this.label16.TabIndex = 8;
            this.label16.Text = "Width";
            // 
            // tab_View_numericDiagramWidth
            // 
            this.tab_View_numericDiagramWidth.Location = new System.Drawing.Point(114, 16);
            this.tab_View_numericDiagramWidth.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.tab_View_numericDiagramWidth.Minimum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.tab_View_numericDiagramWidth.Name = "tab_View_numericDiagramWidth";
            this.tab_View_numericDiagramWidth.Size = new System.Drawing.Size(99, 20);
            this.tab_View_numericDiagramWidth.TabIndex = 7;
            this.tab_View_numericDiagramWidth.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
            // 
            // cbUseShallowCopyColor
            // 
            this.cbUseShallowCopyColor.AutoSize = true;
            this.cbUseShallowCopyColor.Checked = true;
            this.cbUseShallowCopyColor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUseShallowCopyColor.Location = new System.Drawing.Point(10, 211);
            this.cbUseShallowCopyColor.Name = "cbUseShallowCopyColor";
            this.cbUseShallowCopyColor.Size = new System.Drawing.Size(159, 17);
            this.cbUseShallowCopyColor.TabIndex = 20;
            this.cbUseShallowCopyColor.Text = "Enable Shallow Copy Colour";
            this.cbUseShallowCopyColor.UseVisualStyleBackColor = true;
            // 
            // tab_View_comboDefaultToPort
            // 
            this.tab_View_comboDefaultToPort.FormattingEnabled = true;
            this.tab_View_comboDefaultToPort.Location = new System.Drawing.Point(316, 234);
            this.tab_View_comboDefaultToPort.Name = "tab_View_comboDefaultToPort";
            this.tab_View_comboDefaultToPort.Size = new System.Drawing.Size(121, 21);
            this.tab_View_comboDefaultToPort.TabIndex = 19;
            // 
            // tab_View_comboDefaultFromPort
            // 
            this.tab_View_comboDefaultFromPort.FormattingEnabled = true;
            this.tab_View_comboDefaultFromPort.Location = new System.Drawing.Point(105, 234);
            this.tab_View_comboDefaultFromPort.Name = "tab_View_comboDefaultFromPort";
            this.tab_View_comboDefaultFromPort.Size = new System.Drawing.Size(121, 21);
            this.tab_View_comboDefaultFromPort.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(232, 237);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Default To Port";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 237);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Default From Port";
            // 
            // cbUseQuickPanel
            // 
            this.cbUseQuickPanel.AutoSize = true;
            this.cbUseQuickPanel.Checked = true;
            this.cbUseQuickPanel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUseQuickPanel.Location = new System.Drawing.Point(10, 188);
            this.cbUseQuickPanel.Name = "cbUseQuickPanel";
            this.cbUseQuickPanel.Size = new System.Drawing.Size(120, 17);
            this.cbUseQuickPanel.TabIndex = 15;
            this.cbUseQuickPanel.Text = "Enable Quick Panel";
            this.cbUseQuickPanel.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(177, 167);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Minutes";
            // 
            // numAutoSaveInterval
            // 
            this.numAutoSaveInterval.Enabled = false;
            this.numAutoSaveInterval.Location = new System.Drawing.Point(112, 162);
            this.numAutoSaveInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numAutoSaveInterval.Name = "numAutoSaveInterval";
            this.numAutoSaveInterval.Size = new System.Drawing.Size(64, 20);
            this.numAutoSaveInterval.TabIndex = 13;
            this.numAutoSaveInterval.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // cbAutoSave
            // 
            this.cbAutoSave.AutoSize = true;
            this.cbAutoSave.Checked = true;
            this.cbAutoSave.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAutoSave.Location = new System.Drawing.Point(10, 166);
            this.cbAutoSave.Name = "cbAutoSave";
            this.cbAutoSave.Size = new System.Drawing.Size(106, 17);
            this.cbAutoSave.TabIndex = 12;
            this.cbAutoSave.Text = "AutoSave Every ";
            this.cbAutoSave.UseVisualStyleBackColor = true;
            this.cbAutoSave.CheckedChanged += new System.EventHandler(this.cbAutoSave_CheckedChanged);
            // 
            // tab_View_GridOptions
            // 
            this.tab_View_GridOptions.Controls.Add(this.tab_View_lblSmoothingMode);
            this.tab_View_GridOptions.Controls.Add(this.label4);
            this.tab_View_GridOptions.Controls.Add(this.label3);
            this.tab_View_GridOptions.Controls.Add(this.tab_View_lblGridCellSize);
            this.tab_View_GridOptions.Controls.Add(this.tab_View_cbShowGrid);
            this.tab_View_GridOptions.Controls.Add(this.numArrowMoveSmall);
            this.tab_View_GridOptions.Controls.Add(this.numArrowMoveLarge);
            this.tab_View_GridOptions.Controls.Add(this.tab_View_numericGridCellSize);
            this.tab_View_GridOptions.Controls.Add(this.cbSnapResize);
            this.tab_View_GridOptions.Controls.Add(this.tab_View_cbSnapToGrid);
            this.tab_View_GridOptions.Controls.Add(this.tab_View_comboGridSmoothing);
            this.tab_View_GridOptions.Location = new System.Drawing.Point(4, 44);
            this.tab_View_GridOptions.Name = "tab_View_GridOptions";
            this.tab_View_GridOptions.Size = new System.Drawing.Size(438, 114);
            this.tab_View_GridOptions.TabIndex = 7;
            this.tab_View_GridOptions.TabStop = false;
            this.tab_View_GridOptions.Text = "Grid Options";
            // 
            // tab_View_lblSmoothingMode
            // 
            this.tab_View_lblSmoothingMode.AutoSize = true;
            this.tab_View_lblSmoothingMode.Location = new System.Drawing.Point(4, 20);
            this.tab_View_lblSmoothingMode.Name = "tab_View_lblSmoothingMode";
            this.tab_View_lblSmoothingMode.Size = new System.Drawing.Size(87, 13);
            this.tab_View_lblSmoothingMode.TabIndex = 4;
            this.tab_View_lblSmoothingMode.Text = "Smoothing Mode";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(237, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Arrow Move Small";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(235, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Arrow Move Large";
            // 
            // tab_View_lblGridCellSize
            // 
            this.tab_View_lblGridCellSize.AutoSize = true;
            this.tab_View_lblGridCellSize.Location = new System.Drawing.Point(260, 19);
            this.tab_View_lblGridCellSize.Name = "tab_View_lblGridCellSize";
            this.tab_View_lblGridCellSize.Size = new System.Drawing.Size(69, 13);
            this.tab_View_lblGridCellSize.TabIndex = 6;
            this.tab_View_lblGridCellSize.Text = "Grid Cell Size";
            // 
            // tab_View_cbShowGrid
            // 
            this.tab_View_cbShowGrid.AutoSize = true;
            this.tab_View_cbShowGrid.Checked = true;
            this.tab_View_cbShowGrid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tab_View_cbShowGrid.Location = new System.Drawing.Point(97, 44);
            this.tab_View_cbShowGrid.Name = "tab_View_cbShowGrid";
            this.tab_View_cbShowGrid.Size = new System.Drawing.Size(75, 17);
            this.tab_View_cbShowGrid.TabIndex = 0;
            this.tab_View_cbShowGrid.Text = "Show Grid";
            this.tab_View_cbShowGrid.UseVisualStyleBackColor = true;
            // 
            // numArrowMoveSmall
            // 
            this.numArrowMoveSmall.Location = new System.Drawing.Point(346, 65);
            this.numArrowMoveSmall.Name = "numArrowMoveSmall";
            this.numArrowMoveSmall.Size = new System.Drawing.Size(48, 20);
            this.numArrowMoveSmall.TabIndex = 5;
            this.numArrowMoveSmall.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // numArrowMoveLarge
            // 
            this.numArrowMoveLarge.Location = new System.Drawing.Point(346, 41);
            this.numArrowMoveLarge.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numArrowMoveLarge.Name = "numArrowMoveLarge";
            this.numArrowMoveLarge.Size = new System.Drawing.Size(48, 20);
            this.numArrowMoveLarge.TabIndex = 5;
            this.numArrowMoveLarge.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // tab_View_numericGridCellSize
            // 
            this.tab_View_numericGridCellSize.Location = new System.Drawing.Point(346, 17);
            this.tab_View_numericGridCellSize.Name = "tab_View_numericGridCellSize";
            this.tab_View_numericGridCellSize.Size = new System.Drawing.Size(48, 20);
            this.tab_View_numericGridCellSize.TabIndex = 5;
            this.tab_View_numericGridCellSize.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // cbSnapResize
            // 
            this.cbSnapResize.AutoSize = true;
            this.cbSnapResize.Checked = true;
            this.cbSnapResize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSnapResize.Location = new System.Drawing.Point(97, 90);
            this.cbSnapResize.Name = "cbSnapResize";
            this.cbSnapResize.Size = new System.Drawing.Size(125, 17);
            this.cbSnapResize.TabIndex = 2;
            this.cbSnapResize.Text = "Resize Snaps to Grid";
            this.cbSnapResize.UseVisualStyleBackColor = true;
            this.cbSnapResize.CheckedChanged += new System.EventHandler(this.tab_View_cbSnapToGrid_CheckedChanged);
            // 
            // tab_View_cbSnapToGrid
            // 
            this.tab_View_cbSnapToGrid.AutoSize = true;
            this.tab_View_cbSnapToGrid.Checked = true;
            this.tab_View_cbSnapToGrid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tab_View_cbSnapToGrid.Location = new System.Drawing.Point(97, 67);
            this.tab_View_cbSnapToGrid.Name = "tab_View_cbSnapToGrid";
            this.tab_View_cbSnapToGrid.Size = new System.Drawing.Size(116, 17);
            this.tab_View_cbSnapToGrid.TabIndex = 2;
            this.tab_View_cbSnapToGrid.Text = "Drag Snaps to Grid";
            this.tab_View_cbSnapToGrid.UseVisualStyleBackColor = true;
            this.tab_View_cbSnapToGrid.CheckedChanged += new System.EventHandler(this.tab_View_cbSnapToGrid_CheckedChanged);
            // 
            // tab_View_comboGridSmoothing
            // 
            this.tab_View_comboGridSmoothing.FormattingEnabled = true;
            this.tab_View_comboGridSmoothing.Items.AddRange(new object[] {
            "HighSpeed",
            "HighQuality"});
            this.tab_View_comboGridSmoothing.Location = new System.Drawing.Point(97, 17);
            this.tab_View_comboGridSmoothing.Name = "tab_View_comboGridSmoothing";
            this.tab_View_comboGridSmoothing.Size = new System.Drawing.Size(121, 21);
            this.tab_View_comboGridSmoothing.TabIndex = 3;
            this.tab_View_comboGridSmoothing.Text = "HighQuality";
            // 
            // tpgAutocorrect
            // 
            this.tpgAutocorrect.Controls.Add(this.tab_AutoCorrect_lblNotImplemented);
            this.tpgAutocorrect.Controls.Add(this.tab_AutoCorrect_IgnoreInternetAndFileAddresses);
            this.tpgAutocorrect.Controls.Add(this.tab_AutoCorrect_cbIgnoreWordsWithNumbers);
            this.tpgAutocorrect.Controls.Add(this.tab_AutoCorrect_cbIgnoreWordsInUppercase);
            this.tpgAutocorrect.Controls.Add(this.tab_AutoCorrect_cbAlwaysSuggestCorrections);
            this.tpgAutocorrect.Controls.Add(this.tab_AutoCorrect_cbHideSpellingErrorsInThisDocument);
            this.tpgAutocorrect.Controls.Add(this.tab_AutoCorrect_cbCheckSpellingAsYouType);
            this.tpgAutocorrect.Controls.Add(this.tab_AutoCorrect_comboSelectDictionary);
            this.tpgAutocorrect.Controls.Add(this.tab_AutoCorrect_cbEnableAutocorrect);
            this.tpgAutocorrect.Location = new System.Drawing.Point(4, 5);
            this.tpgAutocorrect.Name = "tpgAutocorrect";
            this.tpgAutocorrect.Size = new System.Drawing.Size(460, 296);
            this.tpgAutocorrect.TabIndex = 11;
            this.tpgAutocorrect.Text = "Autocorrect";
            this.tpgAutocorrect.UseVisualStyleBackColor = true;
            // 
            // tab_AutoCorrect_lblNotImplemented
            // 
            this.tab_AutoCorrect_lblNotImplemented.AutoSize = true;
            this.tab_AutoCorrect_lblNotImplemented.BackColor = System.Drawing.Color.White;
            this.tab_AutoCorrect_lblNotImplemented.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tab_AutoCorrect_lblNotImplemented.ForeColor = System.Drawing.Color.Red;
            this.tab_AutoCorrect_lblNotImplemented.Location = new System.Drawing.Point(20, 200);
            this.tab_AutoCorrect_lblNotImplemented.Name = "tab_AutoCorrect_lblNotImplemented";
            this.tab_AutoCorrect_lblNotImplemented.Size = new System.Drawing.Size(191, 13);
            this.tab_AutoCorrect_lblNotImplemented.TabIndex = 13;
            this.tab_AutoCorrect_lblNotImplemented.Text = "Not Implemented In This Version";
            // 
            // tab_AutoCorrect_IgnoreInternetAndFileAddresses
            // 
            this.tab_AutoCorrect_IgnoreInternetAndFileAddresses.AutoSize = true;
            this.tab_AutoCorrect_IgnoreInternetAndFileAddresses.Checked = true;
            this.tab_AutoCorrect_IgnoreInternetAndFileAddresses.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tab_AutoCorrect_IgnoreInternetAndFileAddresses.Enabled = false;
            this.tab_AutoCorrect_IgnoreInternetAndFileAddresses.Location = new System.Drawing.Point(34, 175);
            this.tab_AutoCorrect_IgnoreInternetAndFileAddresses.Name = "tab_AutoCorrect_IgnoreInternetAndFileAddresses";
            this.tab_AutoCorrect_IgnoreInternetAndFileAddresses.Size = new System.Drawing.Size(182, 17);
            this.tab_AutoCorrect_IgnoreInternetAndFileAddresses.TabIndex = 7;
            this.tab_AutoCorrect_IgnoreInternetAndFileAddresses.Text = "Ignore internet and file addresses";
            this.tab_AutoCorrect_IgnoreInternetAndFileAddresses.UseVisualStyleBackColor = true;
            // 
            // tab_AutoCorrect_cbIgnoreWordsWithNumbers
            // 
            this.tab_AutoCorrect_cbIgnoreWordsWithNumbers.AutoSize = true;
            this.tab_AutoCorrect_cbIgnoreWordsWithNumbers.Checked = true;
            this.tab_AutoCorrect_cbIgnoreWordsWithNumbers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tab_AutoCorrect_cbIgnoreWordsWithNumbers.Enabled = false;
            this.tab_AutoCorrect_cbIgnoreWordsWithNumbers.Location = new System.Drawing.Point(34, 151);
            this.tab_AutoCorrect_cbIgnoreWordsWithNumbers.Name = "tab_AutoCorrect_cbIgnoreWordsWithNumbers";
            this.tab_AutoCorrect_cbIgnoreWordsWithNumbers.Size = new System.Drawing.Size(152, 17);
            this.tab_AutoCorrect_cbIgnoreWordsWithNumbers.TabIndex = 6;
            this.tab_AutoCorrect_cbIgnoreWordsWithNumbers.Text = "Ignore words with numbers";
            this.tab_AutoCorrect_cbIgnoreWordsWithNumbers.UseVisualStyleBackColor = true;
            // 
            // tab_AutoCorrect_cbIgnoreWordsInUppercase
            // 
            this.tab_AutoCorrect_cbIgnoreWordsInUppercase.AutoSize = true;
            this.tab_AutoCorrect_cbIgnoreWordsInUppercase.Checked = true;
            this.tab_AutoCorrect_cbIgnoreWordsInUppercase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tab_AutoCorrect_cbIgnoreWordsInUppercase.Enabled = false;
            this.tab_AutoCorrect_cbIgnoreWordsInUppercase.Location = new System.Drawing.Point(34, 127);
            this.tab_AutoCorrect_cbIgnoreWordsInUppercase.Name = "tab_AutoCorrect_cbIgnoreWordsInUppercase";
            this.tab_AutoCorrect_cbIgnoreWordsInUppercase.Size = new System.Drawing.Size(166, 17);
            this.tab_AutoCorrect_cbIgnoreWordsInUppercase.TabIndex = 5;
            this.tab_AutoCorrect_cbIgnoreWordsInUppercase.TabStop = false;
            this.tab_AutoCorrect_cbIgnoreWordsInUppercase.Text = "Ignore words in UPPERCASE";
            this.tab_AutoCorrect_cbIgnoreWordsInUppercase.UseVisualStyleBackColor = true;
            // 
            // tab_AutoCorrect_cbAlwaysSuggestCorrections
            // 
            this.tab_AutoCorrect_cbAlwaysSuggestCorrections.AutoSize = true;
            this.tab_AutoCorrect_cbAlwaysSuggestCorrections.Checked = true;
            this.tab_AutoCorrect_cbAlwaysSuggestCorrections.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tab_AutoCorrect_cbAlwaysSuggestCorrections.Enabled = false;
            this.tab_AutoCorrect_cbAlwaysSuggestCorrections.Location = new System.Drawing.Point(34, 103);
            this.tab_AutoCorrect_cbAlwaysSuggestCorrections.Name = "tab_AutoCorrect_cbAlwaysSuggestCorrections";
            this.tab_AutoCorrect_cbAlwaysSuggestCorrections.Size = new System.Drawing.Size(154, 17);
            this.tab_AutoCorrect_cbAlwaysSuggestCorrections.TabIndex = 4;
            this.tab_AutoCorrect_cbAlwaysSuggestCorrections.Text = "Always suggest corrections";
            this.tab_AutoCorrect_cbAlwaysSuggestCorrections.UseVisualStyleBackColor = true;
            // 
            // tab_AutoCorrect_cbHideSpellingErrorsInThisDocument
            // 
            this.tab_AutoCorrect_cbHideSpellingErrorsInThisDocument.AutoSize = true;
            this.tab_AutoCorrect_cbHideSpellingErrorsInThisDocument.Enabled = false;
            this.tab_AutoCorrect_cbHideSpellingErrorsInThisDocument.Location = new System.Drawing.Point(34, 79);
            this.tab_AutoCorrect_cbHideSpellingErrorsInThisDocument.Name = "tab_AutoCorrect_cbHideSpellingErrorsInThisDocument";
            this.tab_AutoCorrect_cbHideSpellingErrorsInThisDocument.Size = new System.Drawing.Size(195, 17);
            this.tab_AutoCorrect_cbHideSpellingErrorsInThisDocument.TabIndex = 3;
            this.tab_AutoCorrect_cbHideSpellingErrorsInThisDocument.Text = "Hide spelling errors in this document";
            this.tab_AutoCorrect_cbHideSpellingErrorsInThisDocument.UseVisualStyleBackColor = true;
            // 
            // tab_AutoCorrect_cbCheckSpellingAsYouType
            // 
            this.tab_AutoCorrect_cbCheckSpellingAsYouType.AutoSize = true;
            this.tab_AutoCorrect_cbCheckSpellingAsYouType.Checked = true;
            this.tab_AutoCorrect_cbCheckSpellingAsYouType.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tab_AutoCorrect_cbCheckSpellingAsYouType.Enabled = false;
            this.tab_AutoCorrect_cbCheckSpellingAsYouType.Location = new System.Drawing.Point(34, 55);
            this.tab_AutoCorrect_cbCheckSpellingAsYouType.Name = "tab_AutoCorrect_cbCheckSpellingAsYouType";
            this.tab_AutoCorrect_cbCheckSpellingAsYouType.Size = new System.Drawing.Size(152, 17);
            this.tab_AutoCorrect_cbCheckSpellingAsYouType.TabIndex = 2;
            this.tab_AutoCorrect_cbCheckSpellingAsYouType.Text = "Check spelling as you type";
            this.tab_AutoCorrect_cbCheckSpellingAsYouType.UseVisualStyleBackColor = true;
            // 
            // tab_AutoCorrect_comboSelectDictionary
            // 
            this.tab_AutoCorrect_comboSelectDictionary.Enabled = false;
            this.tab_AutoCorrect_comboSelectDictionary.FormattingEnabled = true;
            this.tab_AutoCorrect_comboSelectDictionary.Location = new System.Drawing.Point(34, 27);
            this.tab_AutoCorrect_comboSelectDictionary.Name = "tab_AutoCorrect_comboSelectDictionary";
            this.tab_AutoCorrect_comboSelectDictionary.Size = new System.Drawing.Size(173, 21);
            this.tab_AutoCorrect_comboSelectDictionary.TabIndex = 1;
            this.tab_AutoCorrect_comboSelectDictionary.Text = "Select Dictionary...";
            // 
            // tab_AutoCorrect_cbEnableAutocorrect
            // 
            this.tab_AutoCorrect_cbEnableAutocorrect.AutoSize = true;
            this.tab_AutoCorrect_cbEnableAutocorrect.Location = new System.Drawing.Point(15, 4);
            this.tab_AutoCorrect_cbEnableAutocorrect.Name = "tab_AutoCorrect_cbEnableAutocorrect";
            this.tab_AutoCorrect_cbEnableAutocorrect.Size = new System.Drawing.Size(121, 17);
            this.tab_AutoCorrect_cbEnableAutocorrect.TabIndex = 0;
            this.tab_AutoCorrect_cbEnableAutocorrect.Text = "Enable Auto-Correct";
            this.tab_AutoCorrect_cbEnableAutocorrect.UseVisualStyleBackColor = true;
            // 
            // tpgDatabase
            // 
            this.tpgDatabase.Controls.Add(this.buttonBindImages);
            this.tpgDatabase.Controls.Add(this.groupBoxViews);
            this.tpgDatabase.Controls.Add(this.tab_Database_lblSyncServerConnectionString);
            this.tpgDatabase.Controls.Add(this.tab_Database_btnLocalInstanceConnectionString);
            this.tpgDatabase.Controls.Add(this.tab_Database_txtSyncServerConnectionString);
            this.tpgDatabase.Controls.Add(this.tab_Database_txtLocalInstanceConnectionString);
            this.tpgDatabase.Controls.Add(this.tab_Database_lblSyncServer);
            this.tpgDatabase.Controls.Add(this.tab_Database_lblLocalInstance);
            this.tpgDatabase.Location = new System.Drawing.Point(4, 5);
            this.tpgDatabase.Name = "tpgDatabase";
            this.tpgDatabase.Padding = new System.Windows.Forms.Padding(3);
            this.tpgDatabase.Size = new System.Drawing.Size(460, 296);
            this.tpgDatabase.TabIndex = 1;
            this.tpgDatabase.Text = "Database";
            this.tpgDatabase.UseVisualStyleBackColor = true;
            // 
            // buttonBindImages
            // 
            this.buttonBindImages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBindImages.CornerRadius = new Ascend.CornerRadius(2);
            this.buttonBindImages.GradientLowColor = System.Drawing.Color.DarkGray;
            this.buttonBindImages.Location = new System.Drawing.Point(318, 182);
            this.buttonBindImages.Name = "buttonBindImages";
            this.buttonBindImages.Size = new System.Drawing.Size(94, 23);
            this.buttonBindImages.StayActiveAfterClick = false;
            this.buttonBindImages.TabIndex = 1;
            this.buttonBindImages.Text = "Rebuild Images";
            this.buttonBindImages.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonBindImages.Click += new System.EventHandler(this.buttonBindImages_Click);
            // 
            // groupBoxViews
            // 
            this.groupBoxViews.Controls.Add(this.label24);
            this.groupBoxViews.Controls.Add(this.buttonRebuildViews);
            this.groupBoxViews.Location = new System.Drawing.Point(100, 208);
            this.groupBoxViews.Name = "groupBoxViews";
            this.groupBoxViews.Size = new System.Drawing.Size(312, 82);
            this.groupBoxViews.TabIndex = 6;
            this.groupBoxViews.TabStop = false;
            this.groupBoxViews.Text = "View Generation";
            // 
            // label24
            // 
            this.label24.Dock = System.Windows.Forms.DockStyle.Top;
            this.label24.Location = new System.Drawing.Point(3, 16);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(306, 41);
            this.label24.TabIndex = 7;
            this.label24.Text = "It has been detected that your model is missing views. Click this button to rebui" +
                "ld them now";
            // 
            // buttonRebuildViews
            // 
            this.buttonRebuildViews.CornerRadius = new Ascend.CornerRadius(2);
            this.buttonRebuildViews.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonRebuildViews.GradientLowColor = System.Drawing.Color.DarkGray;
            this.buttonRebuildViews.Location = new System.Drawing.Point(3, 56);
            this.buttonRebuildViews.Name = "buttonRebuildViews";
            this.buttonRebuildViews.Size = new System.Drawing.Size(306, 23);
            this.buttonRebuildViews.StayActiveAfterClick = false;
            this.buttonRebuildViews.TabIndex = 5;
            this.buttonRebuildViews.Text = "Rebuild";
            this.buttonRebuildViews.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonRebuildViews.Click += new System.EventHandler(this.buttonRebuildViews_Click);
            // 
            // tab_Database_lblSyncServerConnectionString
            // 
            this.tab_Database_lblSyncServerConnectionString.CornerRadius = new Ascend.CornerRadius(2);
            this.tab_Database_lblSyncServerConnectionString.GradientLowColor = System.Drawing.Color.DarkGray;
            this.tab_Database_lblSyncServerConnectionString.Location = new System.Drawing.Point(419, 139);
            this.tab_Database_lblSyncServerConnectionString.Name = "tab_Database_lblSyncServerConnectionString";
            this.tab_Database_lblSyncServerConnectionString.Size = new System.Drawing.Size(25, 23);
            this.tab_Database_lblSyncServerConnectionString.StayActiveAfterClick = false;
            this.tab_Database_lblSyncServerConnectionString.TabIndex = 4;
            this.tab_Database_lblSyncServerConnectionString.Text = "...";
            this.tab_Database_lblSyncServerConnectionString.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tab_Database_lblSyncServerConnectionString.Click += new System.EventHandler(this.tab_Database_lblSyncServerConnectionString_Click);
            // 
            // tab_Database_btnLocalInstanceConnectionString
            // 
            this.tab_Database_btnLocalInstanceConnectionString.CornerRadius = new Ascend.CornerRadius(2);
            this.tab_Database_btnLocalInstanceConnectionString.GradientLowColor = System.Drawing.Color.DarkGray;
            this.tab_Database_btnLocalInstanceConnectionString.Location = new System.Drawing.Point(419, 56);
            this.tab_Database_btnLocalInstanceConnectionString.Name = "tab_Database_btnLocalInstanceConnectionString";
            this.tab_Database_btnLocalInstanceConnectionString.Size = new System.Drawing.Size(25, 23);
            this.tab_Database_btnLocalInstanceConnectionString.StayActiveAfterClick = false;
            this.tab_Database_btnLocalInstanceConnectionString.TabIndex = 3;
            this.tab_Database_btnLocalInstanceConnectionString.Text = "...";
            this.tab_Database_btnLocalInstanceConnectionString.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tab_Database_btnLocalInstanceConnectionString.Click += new System.EventHandler(this.tab_Database_btnLocalInstanceConnectionString_Click);
            // 
            // tab_Database_txtSyncServerConnectionString
            // 
            this.tab_Database_txtSyncServerConnectionString.Location = new System.Drawing.Point(100, 86);
            this.tab_Database_txtSyncServerConnectionString.Multiline = true;
            this.tab_Database_txtSyncServerConnectionString.Name = "tab_Database_txtSyncServerConnectionString";
            this.tab_Database_txtSyncServerConnectionString.Size = new System.Drawing.Size(312, 76);
            this.tab_Database_txtSyncServerConnectionString.TabIndex = 2;
            this.tab_Database_txtSyncServerConnectionString.Text = "Not applicable";
            // 
            // tab_Database_txtLocalInstanceConnectionString
            // 
            this.tab_Database_txtLocalInstanceConnectionString.Location = new System.Drawing.Point(100, 8);
            this.tab_Database_txtLocalInstanceConnectionString.Multiline = true;
            this.tab_Database_txtLocalInstanceConnectionString.Name = "tab_Database_txtLocalInstanceConnectionString";
            this.tab_Database_txtLocalInstanceConnectionString.Size = new System.Drawing.Size(312, 72);
            this.tab_Database_txtLocalInstanceConnectionString.TabIndex = 2;
            this.tab_Database_txtLocalInstanceConnectionString.Text = "E:\\Program Files\\Microsoft Visual Studio 8\\Common7\\IDE";
            // 
            // tab_Database_lblSyncServer
            // 
            this.tab_Database_lblSyncServer.Location = new System.Drawing.Point(4, 88);
            this.tab_Database_lblSyncServer.Name = "tab_Database_lblSyncServer";
            this.tab_Database_lblSyncServer.Size = new System.Drawing.Size(90, 45);
            this.tab_Database_lblSyncServer.TabIndex = 1;
            this.tab_Database_lblSyncServer.Text = "Synchronisation Server";
            // 
            // tab_Database_lblLocalInstance
            // 
            this.tab_Database_lblLocalInstance.AutoSize = true;
            this.tab_Database_lblLocalInstance.Location = new System.Drawing.Point(4, 8);
            this.tab_Database_lblLocalInstance.Name = "tab_Database_lblLocalInstance";
            this.tab_Database_lblLocalInstance.Size = new System.Drawing.Size(76, 13);
            this.tab_Database_lblLocalInstance.TabIndex = 0;
            this.tab_Database_lblLocalInstance.Text = "Local instance";
            // 
            // tpgValidation
            // 
            this.tpgValidation.Controls.Add(this.groupBox8);
            this.tpgValidation.Controls.Add(this.groupBox4);
            this.tpgValidation.Controls.Add(this.gpRefresh);
            this.tpgValidation.Controls.Add(this.groupBox5);
            this.tpgValidation.Location = new System.Drawing.Point(4, 5);
            this.tpgValidation.Name = "tpgValidation";
            this.tpgValidation.Size = new System.Drawing.Size(460, 296);
            this.tpgValidation.TabIndex = 18;
            this.tpgValidation.Text = "Validation";
            this.tpgValidation.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.label19);
            this.groupBox8.Controls.Add(this.comboBoxMergeStrategy);
            this.groupBox8.Location = new System.Drawing.Point(3, 202);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(449, 43);
            this.groupBox8.TabIndex = 14;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Merge Duplicates";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(11, 19);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(88, 13);
            this.label19.TabIndex = 1;
            this.label19.Text = "Property Strategy";
            // 
            // comboBoxMergeStrategy
            // 
            this.comboBoxMergeStrategy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMergeStrategy.FormattingEnabled = true;
            this.comboBoxMergeStrategy.Items.AddRange(new object[] {
            "None",
            "Concatenate",
            "FirstValue"});
            this.comboBoxMergeStrategy.Location = new System.Drawing.Point(138, 16);
            this.comboBoxMergeStrategy.Name = "comboBoxMergeStrategy";
            this.comboBoxMergeStrategy.Size = new System.Drawing.Size(150, 21);
            this.comboBoxMergeStrategy.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cbCustomSuggestion);
            this.groupBox4.Controls.Add(this.cbIntellisense);
            this.groupBox4.Controls.Add(this.cbSpellChecker);
            this.groupBox4.Location = new System.Drawing.Point(3, 249);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(449, 43);
            this.groupBox4.TabIndex = 14;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "General";
            // 
            // cbCustomSuggestion
            // 
            this.cbCustomSuggestion.Location = new System.Drawing.Point(259, 19);
            this.cbCustomSuggestion.Name = "cbCustomSuggestion";
            this.cbCustomSuggestion.Size = new System.Drawing.Size(133, 17);
            this.cbCustomSuggestion.TabIndex = 14;
            this.cbCustomSuggestion.Text = "Custom Suggestions";
            this.cbCustomSuggestion.UseVisualStyleBackColor = true;
            // 
            // cbIntellisense
            // 
            this.cbIntellisense.Location = new System.Drawing.Point(120, 19);
            this.cbIntellisense.Name = "cbIntellisense";
            this.cbIntellisense.Size = new System.Drawing.Size(133, 17);
            this.cbIntellisense.TabIndex = 13;
            this.cbIntellisense.Text = "Intellisense";
            this.cbIntellisense.UseVisualStyleBackColor = true;
            // 
            // cbSpellChecker
            // 
            this.cbSpellChecker.Location = new System.Drawing.Point(6, 19);
            this.cbSpellChecker.Name = "cbSpellChecker";
            this.cbSpellChecker.Size = new System.Drawing.Size(107, 17);
            this.cbSpellChecker.TabIndex = 13;
            this.cbSpellChecker.Text = "Spellchecker";
            this.cbSpellChecker.UseVisualStyleBackColor = true;
            // 
            // gpRefresh
            // 
            this.gpRefresh.Controls.Add(this.cbCompareLinks);
            this.gpRefresh.Controls.Add(this.radioRefreshNever);
            this.gpRefresh.Controls.Add(this.radioRefreshPrompt);
            this.gpRefresh.Controls.Add(this.radioRefreshAutomatic);
            this.gpRefresh.Location = new System.Drawing.Point(3, 7);
            this.gpRefresh.Name = "gpRefresh";
            this.gpRefresh.Size = new System.Drawing.Size(449, 90);
            this.gpRefresh.TabIndex = 12;
            this.gpRefresh.TabStop = false;
            this.gpRefresh.Text = "Compare and Refresh Data";
            // 
            // radioRefreshNever
            // 
            this.radioRefreshNever.AutoSize = true;
            this.radioRefreshNever.Location = new System.Drawing.Point(14, 65);
            this.radioRefreshNever.Name = "radioRefreshNever";
            this.radioRefreshNever.Size = new System.Drawing.Size(54, 17);
            this.radioRefreshNever.TabIndex = 0;
            this.radioRefreshNever.Text = "Never";
            this.radioRefreshNever.UseVisualStyleBackColor = true;
            this.radioRefreshNever.CheckedChanged += new System.EventHandler(this.radioRefreshNever_CheckedChanged);
            // 
            // radioRefreshPrompt
            // 
            this.radioRefreshPrompt.AutoSize = true;
            this.radioRefreshPrompt.Checked = true;
            this.radioRefreshPrompt.Location = new System.Drawing.Point(14, 42);
            this.radioRefreshPrompt.Name = "radioRefreshPrompt";
            this.radioRefreshPrompt.Size = new System.Drawing.Size(58, 17);
            this.radioRefreshPrompt.TabIndex = 0;
            this.radioRefreshPrompt.TabStop = true;
            this.radioRefreshPrompt.Text = "Prompt";
            this.radioRefreshPrompt.UseVisualStyleBackColor = true;
            this.radioRefreshPrompt.CheckedChanged += new System.EventHandler(this.radioRefreshPrompt_CheckedChanged);
            // 
            // radioRefreshAutomatic
            // 
            this.radioRefreshAutomatic.AutoSize = true;
            this.radioRefreshAutomatic.Location = new System.Drawing.Point(14, 19);
            this.radioRefreshAutomatic.Name = "radioRefreshAutomatic";
            this.radioRefreshAutomatic.Size = new System.Drawing.Size(72, 17);
            this.radioRefreshAutomatic.TabIndex = 0;
            this.radioRefreshAutomatic.Text = "Automatic";
            this.radioRefreshAutomatic.UseVisualStyleBackColor = true;
            this.radioRefreshAutomatic.CheckedChanged += new System.EventHandler(this.radioRefreshAutomatic_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.radioCurrentWorkspace);
            this.groupBox5.Controls.Add(this.cbCheckDuplicatesWhileDiagramming);
            this.groupBox5.Controls.Add(this.radioAcrossWorkspaces);
            this.groupBox5.Location = new System.Drawing.Point(3, 103);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(451, 93);
            this.groupBox5.TabIndex = 11;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Duplicate Checking";
            // 
            // radioCurrentWorkspace
            // 
            this.radioCurrentWorkspace.AutoSize = true;
            this.radioCurrentWorkspace.Checked = true;
            this.radioCurrentWorkspace.Location = new System.Drawing.Point(36, 68);
            this.radioCurrentWorkspace.Name = "radioCurrentWorkspace";
            this.radioCurrentWorkspace.Size = new System.Drawing.Size(117, 17);
            this.radioCurrentWorkspace.TabIndex = 0;
            this.radioCurrentWorkspace.TabStop = true;
            this.radioCurrentWorkspace.Text = "Current Workspace";
            this.radioCurrentWorkspace.UseVisualStyleBackColor = true;
            // 
            // cbCheckDuplicatesWhileDiagramming
            // 
            this.cbCheckDuplicatesWhileDiagramming.AutoSize = true;
            this.cbCheckDuplicatesWhileDiagramming.Checked = true;
            this.cbCheckDuplicatesWhileDiagramming.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCheckDuplicatesWhileDiagramming.Location = new System.Drawing.Point(15, 22);
            this.cbCheckDuplicatesWhileDiagramming.Name = "cbCheckDuplicatesWhileDiagramming";
            this.cbCheckDuplicatesWhileDiagramming.Size = new System.Drawing.Size(249, 17);
            this.cbCheckDuplicatesWhileDiagramming.TabIndex = 7;
            this.cbCheckDuplicatesWhileDiagramming.Text = "Check for Duplicate Objects in the Background";
            this.cbCheckDuplicatesWhileDiagramming.UseVisualStyleBackColor = true;
            this.cbCheckDuplicatesWhileDiagramming.CheckedChanged += new System.EventHandler(this.cbCheckDuplicatesWhileDiagramming_CheckedChanged_1);
            // 
            // radioAcrossWorkspaces
            // 
            this.radioAcrossWorkspaces.AutoSize = true;
            this.radioAcrossWorkspaces.Location = new System.Drawing.Point(36, 45);
            this.radioAcrossWorkspaces.Name = "radioAcrossWorkspaces";
            this.radioAcrossWorkspaces.Size = new System.Drawing.Size(120, 17);
            this.radioAcrossWorkspaces.TabIndex = 0;
            this.radioAcrossWorkspaces.Text = "Across Workspaces";
            this.radioAcrossWorkspaces.UseVisualStyleBackColor = true;
            // 
            // tpgStencilImages
            // 
            this.tpgStencilImages.Controls.Add(this.treeViewImages);
            this.tpgStencilImages.Controls.Add(this.checkedListBoxStencilImage);
            this.tpgStencilImages.Controls.Add(this.label23);
            this.tpgStencilImages.Controls.Add(this.buttonStencilImagesBrowse);
            this.tpgStencilImages.Controls.Add(this.label21);
            this.tpgStencilImages.Controls.Add(this.pictureBoxStencilImagesImage);
            this.tpgStencilImages.Controls.Add(this.label18);
            this.tpgStencilImages.Location = new System.Drawing.Point(4, 5);
            this.tpgStencilImages.Name = "tpgStencilImages";
            this.tpgStencilImages.Padding = new System.Windows.Forms.Padding(3);
            this.tpgStencilImages.Size = new System.Drawing.Size(460, 296);
            this.tpgStencilImages.TabIndex = 21;
            this.tpgStencilImages.Text = "tabPage2";
            this.tpgStencilImages.UseVisualStyleBackColor = true;
            // 
            // treeViewImages
            // 
            this.treeViewImages.Location = new System.Drawing.Point(10, 23);
            this.treeViewImages.Name = "treeViewImages";
            this.treeViewImages.Size = new System.Drawing.Size(220, 267);
            this.treeViewImages.TabIndex = 13;
            this.treeViewImages.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewImages_AfterSelect);
            // 
            // checkedListBoxStencilImage
            // 
            this.checkedListBoxStencilImage.FormattingEnabled = true;
            this.checkedListBoxStencilImage.Location = new System.Drawing.Point(239, 39);
            this.checkedListBoxStencilImage.Name = "checkedListBoxStencilImage";
            this.checkedListBoxStencilImage.Size = new System.Drawing.Size(208, 109);
            this.checkedListBoxStencilImage.TabIndex = 12;
            this.checkedListBoxStencilImage.ThreeDCheckBoxes = true;
            this.checkedListBoxStencilImage.SelectedIndexChanged += new System.EventHandler(this.checkedListBoxStencilImage_SelectedIndexChanged);
            this.checkedListBoxStencilImage.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxStencilImage_ItemCheck);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(236, 23);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(27, 13);
            this.label23.TabIndex = 11;
            this.label23.Text = "Link";
            // 
            // buttonStencilImagesBrowse
            // 
            this.buttonStencilImagesBrowse.CornerRadius = new Ascend.CornerRadius(2);
            this.buttonStencilImagesBrowse.GradientLowColor = System.Drawing.Color.DarkGray;
            this.buttonStencilImagesBrowse.Location = new System.Drawing.Point(382, 154);
            this.buttonStencilImagesBrowse.Name = "buttonStencilImagesBrowse";
            this.buttonStencilImagesBrowse.Size = new System.Drawing.Size(65, 23);
            this.buttonStencilImagesBrowse.StayActiveAfterClick = false;
            this.buttonStencilImagesBrowse.TabIndex = 9;
            this.buttonStencilImagesBrowse.Text = "Browse...";
            this.buttonStencilImagesBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonStencilImagesBrowse.Click += new System.EventHandler(this.buttonStencilImagesBrowse_Click);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(236, 181);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(91, 13);
            this.label21.TabIndex = 7;
            this.label21.Text = "Associated Image";
            // 
            // pictureBoxStencilImagesImage
            // 
            this.pictureBoxStencilImagesImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxStencilImagesImage.Location = new System.Drawing.Point(239, 197);
            this.pictureBoxStencilImagesImage.Name = "pictureBoxStencilImagesImage";
            this.pictureBoxStencilImagesImage.Size = new System.Drawing.Size(208, 93);
            this.pictureBoxStencilImagesImage.TabIndex = 6;
            this.pictureBoxStencilImagesImage.TabStop = false;
            this.pictureBoxStencilImagesImage.Click += new System.EventHandler(this.pictureBoxStencilImagesImage_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(7, 7);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(130, 13);
            this.label18.TabIndex = 1;
            this.label18.Text = "Stencil Image Binding";
            // 
            // tabPageFilters
            // 
            this.tabPageFilters.Controls.Add(this.groupBox7);
            this.tabPageFilters.Controls.Add(this.label8);
            this.tabPageFilters.Controls.Add(this.buttonDelete);
            this.tabPageFilters.Controls.Add(this.listBoxFilters);
            this.tabPageFilters.Location = new System.Drawing.Point(4, 5);
            this.tabPageFilters.Name = "tabPageFilters";
            this.tabPageFilters.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFilters.Size = new System.Drawing.Size(460, 296);
            this.tabPageFilters.TabIndex = 22;
            this.tabPageFilters.Tag = "tnFilterBrowser";
            this.tabPageFilters.Text = "Filters";
            this.tabPageFilters.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.checkBoxCopyFilter);
            this.groupBox7.Controls.Add(this.buttonNew);
            this.groupBox7.Controls.Add(this.textBoxNewFilter);
            this.groupBox7.Controls.Add(this.comboBoxFilters);
            this.groupBox7.Location = new System.Drawing.Point(209, 69);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(243, 73);
            this.groupBox7.TabIndex = 7;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Create New Filter";
            // 
            // checkBoxCopyFilter
            // 
            this.checkBoxCopyFilter.AutoSize = true;
            this.checkBoxCopyFilter.Location = new System.Drawing.Point(6, 19);
            this.checkBoxCopyFilter.Name = "checkBoxCopyFilter";
            this.checkBoxCopyFilter.Size = new System.Drawing.Size(101, 17);
            this.checkBoxCopyFilter.TabIndex = 3;
            this.checkBoxCopyFilter.Text = "Copy Filter From";
            this.checkBoxCopyFilter.UseVisualStyleBackColor = true;
            this.checkBoxCopyFilter.CheckedChanged += new System.EventHandler(this.checkBoxCopyFilter_CheckedChanged);
            // 
            // buttonNew
            // 
            this.buttonNew.CornerRadius = new Ascend.CornerRadius(2);
            this.buttonNew.GradientLowColor = System.Drawing.Color.DarkGray;
            this.buttonNew.Location = new System.Drawing.Point(159, 42);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(75, 23);
            this.buttonNew.StayActiveAfterClick = false;
            this.buttonNew.TabIndex = 1;
            this.buttonNew.Text = "Create";
            this.buttonNew.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
            // 
            // textBoxNewFilter
            // 
            this.textBoxNewFilter.Location = new System.Drawing.Point(6, 42);
            this.textBoxNewFilter.Name = "textBoxNewFilter";
            this.textBoxNewFilter.Size = new System.Drawing.Size(147, 20);
            this.textBoxNewFilter.TabIndex = 5;
            // 
            // comboBoxFilters
            // 
            this.comboBoxFilters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFilters.Enabled = false;
            this.comboBoxFilters.FormattingEnabled = true;
            this.comboBoxFilters.Location = new System.Drawing.Point(113, 15);
            this.comboBoxFilters.Name = "comboBoxFilters";
            this.comboBoxFilters.Size = new System.Drawing.Size(121, 21);
            this.comboBoxFilters.TabIndex = 2;
            this.comboBoxFilters.SelectedIndexChanged += new System.EventHandler(this.comboBoxFilters_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 5);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Available Filters";
            // 
            // buttonDelete
            // 
            this.buttonDelete.CornerRadius = new Ascend.CornerRadius(2);
            this.buttonDelete.Enabled = false;
            this.buttonDelete.GradientLowColor = System.Drawing.Color.DarkGray;
            this.buttonDelete.Location = new System.Drawing.Point(209, 21);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.StayActiveAfterClick = false;
            this.buttonDelete.TabIndex = 1;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // listBoxFilters
            // 
            this.listBoxFilters.FormattingEnabled = true;
            this.listBoxFilters.Location = new System.Drawing.Point(6, 21);
            this.listBoxFilters.Name = "listBoxFilters";
            this.listBoxFilters.Size = new System.Drawing.Size(197, 121);
            this.listBoxFilters.TabIndex = 0;
            this.listBoxFilters.SelectedIndexChanged += new System.EventHandler(this.listBoxFilters_SelectedIndexChanged);
            // 
            // tvSettings
            // 
            this.tvSettings.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.tvSettings.Dock = System.Windows.Forms.DockStyle.Left;
            this.tvSettings.Location = new System.Drawing.Point(0, 0);
            this.tvSettings.Name = "tvSettings";
            treeNode1.Name = "tnGeneral";
            treeNode1.Tag = "tpgGeneral";
            treeNode1.Text = "General";
            treeNode2.Name = "tnView";
            treeNode2.Tag = "tpgView";
            treeNode2.Text = "Canvas and Grid";
            treeNode3.Name = "nodeValidation";
            treeNode3.Tag = "tpgValidation";
            treeNode3.Text = "Data Validation";
            treeNode4.Name = "nodePrinting";
            treeNode4.Tag = "tpgPrinting";
            treeNode4.Text = "Printing";
            treeNode5.Name = "tnDiagramming";
            treeNode5.Tag = "tpgView";
            treeNode5.Text = "Diagramming";
            treeNode6.Name = "tnStencilImages";
            treeNode6.Tag = "tpgStencilImages";
            treeNode6.Text = "Stencil Images";
            treeNode7.Name = "tnFilterBrowser";
            treeNode7.Tag = "tabPageFilters";
            treeNode7.Text = "Filters";
            treeNode8.Name = "tnDatabase";
            treeNode8.Tag = "tpgDatabase";
            treeNode8.Text = "Database";
            treeNode9.Name = "tnAdvanced";
            treeNode9.Tag = "tpgDatabase";
            treeNode9.Text = "Advanced";
            this.tvSettings.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode5,
            treeNode9});
            this.tvSettings.Size = new System.Drawing.Size(146, 334);
            this.tvSettings.TabIndex = 2;
            this.tvSettings.Tag = "tpgImageList";
            this.tvSettings.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvSettings_AfterSelect);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(146, 305);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(468, 29);
            this.panel1.TabIndex = 4;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.CornerRadius = new Ascend.CornerRadius(2);
            this.btnOK.GradientLowColor = System.Drawing.Color.DarkGray;
            this.btnOK.Location = new System.Drawing.Point(306, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.StayActiveAfterClick = false;
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.CornerRadius = new Ascend.CornerRadius(2);
            this.btnCancel.GradientLowColor = System.Drawing.Color.DarkGray;
            this.btnCancel.Location = new System.Drawing.Point(387, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.StayActiveAfterClick = false;
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 67);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(51, 13);
            this.label11.TabIndex = 4;
            this.label11.Text = "Diagrams";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 36);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(44, 13);
            this.label12.TabIndex = 2;
            this.label12.Text = "Stencils";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 10);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(43, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "Shapes";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Controls.Add(this.checkBox2);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(260, 120);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Grid Options";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Smoothing Mode";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Grid Cell Size";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(97, 44);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(75, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Show Grid";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(97, 88);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(48, 20);
            this.numericUpDown1.TabIndex = 5;
            this.numericUpDown1.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(97, 67);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(85, 17);
            this.checkBox2.TabIndex = 2;
            this.checkBox2.Text = "Snap to Grid";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "HighSpeed",
            "Default",
            "HighQuality",
            "None",
            "AntiAlias"});
            this.comboBox1.Location = new System.Drawing.Point(97, 17);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.Text = "Default";
            // 
            // cbCompareLinks
            // 
            this.cbCompareLinks.AutoSize = true;
            this.cbCompareLinks.Location = new System.Drawing.Point(347, 65);
            this.cbCompareLinks.Name = "cbCompareLinks";
            this.cbCompareLinks.Size = new System.Drawing.Size(96, 17);
            this.cbCompareLinks.TabIndex = 1;
            this.cbCompareLinks.Text = "Compare Links";
            this.cbCompareLinks.UseVisualStyleBackColor = true;
            // 
            // Preferences
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(614, 334);
            this.ControlBox = false;
            this.Controls.Add(this.tabPreferences);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tvSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(620, 340);
            this.Name = "Preferences";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.Preferences_Load);
            this.tabPreferences.ResumeLayout(false);
            this.tpgPrinting.ResumeLayout(false);
            this.tpgPrinting.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tpgGeneral.ResumeLayout(false);
            this.tpgGeneral.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.tab_General_groupBoxUserInfo.ResumeLayout(false);
            this.tab_General_groupBoxUserInfo.PerformLayout();
            this.tpgSave.ResumeLayout(false);
            this.tab_Save_groupBoxSaveOptions.ResumeLayout(false);
            this.tab_Save_groupBoxSaveOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tab_Save_numericAutoSaveInterval)).EndInit();
            this.tpgView.ResumeLayout(false);
            this.tpgView.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tab_View_numericDiagramHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tab_View_numericDiagramWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAutoSaveInterval)).EndInit();
            this.tab_View_GridOptions.ResumeLayout(false);
            this.tab_View_GridOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numArrowMoveSmall)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numArrowMoveLarge)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tab_View_numericGridCellSize)).EndInit();
            this.tpgAutocorrect.ResumeLayout(false);
            this.tpgAutocorrect.PerformLayout();
            this.tpgDatabase.ResumeLayout(false);
            this.tpgDatabase.PerformLayout();
            this.groupBoxViews.ResumeLayout(false);
            this.tpgValidation.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.gpRefresh.ResumeLayout(false);
            this.gpRefresh.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tpgStencilImages.ResumeLayout(false);
            this.tpgStencilImages.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStencilImagesImage)).EndInit();
            this.tabPageFilters.ResumeLayout(false);
            this.tabPageFilters.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabPreferences;
        private System.Windows.Forms.TabPage tpgView;
        private System.Windows.Forms.TabPage tpgGeneral;
        private System.Windows.Forms.TabPage tpgDatabase;
        private System.Windows.Forms.TabPage tpgAutocorrect;
        private System.Windows.Forms.TreeView tvSettings;
        private System.Windows.Forms.Panel panel1;
        private MetaControls.MetaButton btnOK;
        private MetaControls.MetaButton btnCancel;
        private System.Windows.Forms.CheckBox tab_View_cbSnapToGrid;
        private System.Windows.Forms.CheckBox tab_View_cbShowGrid;
        private System.Windows.Forms.ComboBox tab_View_comboGridSmoothing;
        private System.Windows.Forms.Label tab_View_lblSmoothingMode;
        private System.Windows.Forms.ComboBox tab_AutoCorrect_comboSelectDictionary;
        private System.Windows.Forms.CheckBox tab_AutoCorrect_cbEnableAutocorrect;
        private System.Windows.Forms.CheckBox tab_AutoCorrect_IgnoreInternetAndFileAddresses;
        private System.Windows.Forms.CheckBox tab_AutoCorrect_cbIgnoreWordsWithNumbers;
        private System.Windows.Forms.CheckBox tab_AutoCorrect_cbIgnoreWordsInUppercase;
        private System.Windows.Forms.CheckBox tab_AutoCorrect_cbAlwaysSuggestCorrections;
        private System.Windows.Forms.CheckBox tab_AutoCorrect_cbHideSpellingErrorsInThisDocument;
        private System.Windows.Forms.CheckBox tab_AutoCorrect_cbCheckSpellingAsYouType;
        private System.Windows.Forms.Label tab_Database_lblSyncServer;
        private System.Windows.Forms.Label tab_Database_lblLocalInstance;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label tab_View_lblGridCellSize;
        private System.Windows.Forms.NumericUpDown tab_View_numericGridCellSize;
        private System.Windows.Forms.Label tab_AutoCorrect_lblNotImplemented;
        private System.Windows.Forms.GroupBox tab_General_groupBoxUserInfo;
        private System.Windows.Forms.TextBox tab_General_txtCompany;
        private System.Windows.Forms.TextBox tab_General_txtInitials;
        private System.Windows.Forms.TextBox tab_General_txtFullname;
        private System.Windows.Forms.Label tab_General_lblCompany;
        private System.Windows.Forms.Label tab_General_lblInitials;
        private System.Windows.Forms.Label tab_General_lblFullname;
        private System.Windows.Forms.TabPage tpgSave;
        private System.Windows.Forms.GroupBox tab_Save_groupBoxSaveOptions;
        private System.Windows.Forms.CheckBox tab_Save_cbAutoSaveDocuments;
        private System.Windows.Forms.Label tab_Save_lblAutoSaveMinutes;
        private System.Windows.Forms.NumericUpDown tab_Save_numericAutoSaveInterval;
        private System.Windows.Forms.CheckBox tab_Save_cbAutoCheckoutAllObjects;
        private System.Windows.Forms.CheckBox tab_Save_cbPromptToPurgeMinorVersions;
        private System.Windows.Forms.CheckBox tab_Save_cbPromptForDocumentPropertiesOnFirstSave;
        private System.Windows.Forms.GroupBox tab_View_GridOptions;
        private System.Windows.Forms.TextBox tab_Database_txtSyncServerConnectionString;
        private System.Windows.Forms.TextBox tab_Database_txtLocalInstanceConnectionString;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TabPage tpgPrinting;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox cbPrintComments;
        private System.Windows.Forms.CheckBox cbPrintArtefactPointers;
        private System.Windows.Forms.CheckBox cbPrintSourceControlIndicators;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numArrowMoveSmall;
        private System.Windows.Forms.NumericUpDown numArrowMoveLarge;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numAutoSaveInterval;
        private System.Windows.Forms.CheckBox cbAutoSave;
        private System.Windows.Forms.TabPage tpgValidation;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox cbCheckDuplicatesWhileDiagramming;
        private System.Windows.Forms.RadioButton radioCurrentWorkspace;
        private System.Windows.Forms.RadioButton radioAcrossWorkspaces;
        private System.Windows.Forms.GroupBox gpRefresh;
        private System.Windows.Forms.RadioButton radioRefreshNever;
        private System.Windows.Forms.RadioButton radioRefreshPrompt;
        private System.Windows.Forms.RadioButton radioRefreshAutomatic;
        private System.Windows.Forms.CheckBox cbSnapResize;
        private System.Windows.Forms.CheckBox cbSpellChecker;
        private MetaControls.MetaButton tab_Database_lblSyncServerConnectionString;
        private MetaControls.MetaButton tab_Database_btnLocalInstanceConnectionString;
        private System.Windows.Forms.CheckBox checkBoxGeneralVerboseLogging;
        private System.Windows.Forms.CheckBox cbUseQuickPanel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox tab_View_comboDefaultToPort;
        private System.Windows.Forms.ComboBox tab_View_comboDefaultFromPort;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox cbUseShallowCopyColor;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.NumericUpDown tab_View_numericDiagramHeight;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.NumericUpDown tab_View_numericDiagramWidth;
        private System.Windows.Forms.TabPage tpgStencilImages;
        private System.Windows.Forms.Label label18;
        private MetaControls.MetaButton buttonStencilImagesBrowse;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.PictureBox pictureBoxStencilImagesImage;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.CheckedListBox checkedListBoxStencilImage;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tab_General_Paths_SymbolsText;
        private System.Windows.Forms.TextBox tab_General_Paths_DiagramsText;
        private System.Windows.Forms.TextBox tab_General_Paths_ExportsText;
        private System.Windows.Forms.TextBox tab_General_Paths_SourceText;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.CheckBox cbIntellisense;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TabPage tabPageFilters;
        private System.Windows.Forms.CheckBox checkBoxCopyFilter;
        private System.Windows.Forms.ComboBox comboBoxFilters;
        private MetaControls.MetaButton buttonNew;
        private MetaControls.MetaButton buttonDelete;
        private System.Windows.Forms.ListBox listBoxFilters;
        private System.Windows.Forms.TextBox textBoxNewFilter;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBoxViews;
        private System.Windows.Forms.Label label24;
        private MetaControls.MetaButton buttonRebuildViews;
        private MetaControls.MetaButton buttonBindImages;
        private System.Windows.Forms.TreeView treeViewImages;
        private System.Windows.Forms.CheckBox cbDisplayImageNodeClassName;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.ComboBox comboBoxMergeStrategy;
        private System.Windows.Forms.CheckBox checkBoxGeneralSaveOnCreate;
        private System.Windows.Forms.CheckBox cbCustomSuggestion;
        private System.Windows.Forms.CheckBox cbCompareLinks;
    }
}