using System.Windows.Forms;
using MetaBuilder.MetaControls;

namespace MetaBuilder.UIControls.Common
{
    partial class Reports
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Reports));
            Syncfusion.Windows.Forms.Grid.GridBaseStyle gridBaseStyle1 = new Syncfusion.Windows.Forms.Grid.GridBaseStyle();
            Syncfusion.Windows.Forms.Grid.GridBaseStyle gridBaseStyle2 = new Syncfusion.Windows.Forms.Grid.GridBaseStyle();
            Syncfusion.Windows.Forms.Grid.GridBaseStyle gridBaseStyle3 = new Syncfusion.Windows.Forms.Grid.GridBaseStyle();
            Syncfusion.Windows.Forms.Grid.GridBaseStyle gridBaseStyle4 = new Syncfusion.Windows.Forms.Grid.GridBaseStyle();
            Syncfusion.Windows.Forms.Grid.GridStyleInfo gridStyleInfo1 = new Syncfusion.Windows.Forms.Grid.GridStyleInfo();
            this.tabControlReports = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.comboBoxObjectType = new System.Windows.Forms.ComboBox();
            this.labelObject = new System.Windows.Forms.Label();
            this.buttonBasicGo = new MetaBuilder.MetaControls.MetaButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkBoxIncludeOrphans = new System.Windows.Forms.CheckBox();
            this.labelAssociation = new System.Windows.Forms.Label();
            this.labelChildClass = new System.Windows.Forms.Label();
            this.labelParentClass = new System.Windows.Forms.Label();
            this.comboBoxAssociation = new System.Windows.Forms.ComboBox();
            this.comboBoxChildClass = new System.Windows.Forms.ComboBox();
            this.comboBoxParentClass = new System.Windows.Forms.ComboBox();
            this.buttonGeneralGo = new MetaBuilder.MetaControls.MetaButton();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.labelNodeDescription = new System.Windows.Forms.Label();
            this.checkBoxRemoveFileNameTrim = new System.Windows.Forms.CheckBox();
            this.treeViewPredefined = new System.Windows.Forms.TreeView();
            this.buttonPredefinedGo = new MetaBuilder.MetaControls.MetaButton();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.contextFilter = new ContextFilter(null, false);
            this.button1 = new MetaBuilder.MetaControls.MetaButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCustom = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.labelaAssociation2 = new System.Windows.Forms.Label();
            this.labelaChildChildClass = new System.Windows.Forms.Label();
            this.comboBoxaAssociation2 = new System.Windows.Forms.ComboBox();
            this.comboBoxaChildChildClass = new System.Windows.Forms.ComboBox();
            this.labelaAssociation1 = new System.Windows.Forms.Label();
            this.labelaChildClass = new System.Windows.Forms.Label();
            this.labelaParentClass = new System.Windows.Forms.Label();
            this.comboBoxaAssociation1 = new System.Windows.Forms.ComboBox();
            this.comboBoxaChildClass = new System.Windows.Forms.ComboBox();
            this.comboBoxaParentClass = new System.Windows.Forms.ComboBox();
            this.buttonAdvancedGo = new MetaBuilder.MetaControls.MetaButton();
            this.labelInfo = new System.Windows.Forms.Label();
            this.comboBoxWorkspace = new System.Windows.Forms.ComboBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSelectAll = new System.Windows.Forms.ToolStripButton();
            this.copyToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.gridDataBoundGridMain = new Syncfusion.Windows.Forms.Grid.GridDataBoundGrid();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.lblNoResults = new System.Windows.Forms.Label();
            this.bgWorker = new System.ComponentModel.BackgroundWorker();
            this.checkBoxServer = new System.Windows.Forms.CheckBox();
            this.tabControlReports.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDataBoundGridMain)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlReports
            // 
            this.tabControlReports.Controls.Add(this.tabPage1);
            this.tabControlReports.Controls.Add(this.tabPage2);
            this.tabControlReports.Controls.Add(this.tabPage4);
            this.tabControlReports.Controls.Add(this.tabPage7);
            this.tabControlReports.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlReports.Location = new System.Drawing.Point(3, 3);
            this.tabControlReports.Multiline = true;
            this.tabControlReports.Name = "tabControlReports";
            this.tabControlReports.SelectedIndex = 0;
            this.tabControlReports.Size = new System.Drawing.Size(657, 474);
            this.tabControlReports.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.comboBoxObjectType);
            this.tabPage1.Controls.Add(this.labelObject);
            this.tabPage1.Controls.Add(this.buttonBasicGo);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(649, 448);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Basic";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // comboBoxObjectType
            // 
            this.comboBoxObjectType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxObjectType.FormattingEnabled = true;
            this.comboBoxObjectType.Location = new System.Drawing.Point(110, 6);
            this.comboBoxObjectType.Name = "comboBoxObjectType";
            this.comboBoxObjectType.Size = new System.Drawing.Size(203, 21);
            this.comboBoxObjectType.TabIndex = 2;
            // 
            // labelObject
            // 
            this.labelObject.AutoSize = true;
            this.labelObject.Location = new System.Drawing.Point(8, 11);
            this.labelObject.Name = "labelObject";
            this.labelObject.Size = new System.Drawing.Size(96, 13);
            this.labelObject.TabIndex = 1;
            this.labelObject.Text = "List Object of Type";
            // 
            // buttonBasicGo
            // 
            this.buttonBasicGo.CornerRadius = new Ascend.CornerRadius(2);
            this.buttonBasicGo.GradientLowColor = System.Drawing.Color.DarkGray;
            this.buttonBasicGo.Location = new System.Drawing.Point(319, 6);
            this.buttonBasicGo.Name = "buttonBasicGo";
            this.buttonBasicGo.Size = new System.Drawing.Size(45, 23);
            this.buttonBasicGo.StayActiveAfterClick = false;
            this.buttonBasicGo.TabIndex = 0;
            this.buttonBasicGo.Text = "Go";
            this.buttonBasicGo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonBasicGo.Click += new System.EventHandler(this.buttonBasicGo_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.checkBoxIncludeOrphans);
            this.tabPage2.Controls.Add(this.labelAssociation);
            this.tabPage2.Controls.Add(this.labelChildClass);
            this.tabPage2.Controls.Add(this.labelParentClass);
            this.tabPage2.Controls.Add(this.comboBoxAssociation);
            this.tabPage2.Controls.Add(this.comboBoxChildClass);
            this.tabPage2.Controls.Add(this.comboBoxParentClass);
            this.tabPage2.Controls.Add(this.buttonGeneralGo);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(649, 448);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "General";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // checkBoxIncludeOrphans
            // 
            this.checkBoxIncludeOrphans.AutoSize = true;
            this.checkBoxIncludeOrphans.Location = new System.Drawing.Point(25, 97);
            this.checkBoxIncludeOrphans.Name = "checkBoxIncludeOrphans";
            this.checkBoxIncludeOrphans.Size = new System.Drawing.Size(232, 17);
            this.checkBoxIncludeOrphans.TabIndex = 8;
            this.checkBoxIncludeOrphans.Text = "Include Objects Which Are Not In Diagrams";
            this.checkBoxIncludeOrphans.UseVisualStyleBackColor = true;
            // 
            // labelAssociation
            // 
            this.labelAssociation.AutoSize = true;
            this.labelAssociation.Location = new System.Drawing.Point(8, 70);
            this.labelAssociation.Name = "labelAssociation";
            this.labelAssociation.Size = new System.Drawing.Size(61, 13);
            this.labelAssociation.TabIndex = 7;
            this.labelAssociation.Text = "Association";
            // 
            // labelChildClass
            // 
            this.labelChildClass.AutoSize = true;
            this.labelChildClass.Location = new System.Drawing.Point(8, 42);
            this.labelChildClass.Name = "labelChildClass";
            this.labelChildClass.Size = new System.Drawing.Size(58, 13);
            this.labelChildClass.TabIndex = 6;
            this.labelChildClass.Text = "Child Class";
            // 
            // labelParentClass
            // 
            this.labelParentClass.AutoSize = true;
            this.labelParentClass.Location = new System.Drawing.Point(8, 14);
            this.labelParentClass.Name = "labelParentClass";
            this.labelParentClass.Size = new System.Drawing.Size(66, 13);
            this.labelParentClass.TabIndex = 5;
            this.labelParentClass.Text = "Parent Class";
            // 
            // comboBoxAssociation
            // 
            this.comboBoxAssociation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAssociation.FormattingEnabled = true;
            this.comboBoxAssociation.Location = new System.Drawing.Point(80, 70);
            this.comboBoxAssociation.Name = "comboBoxAssociation";
            this.comboBoxAssociation.Size = new System.Drawing.Size(177, 21);
            this.comboBoxAssociation.TabIndex = 4;
            // 
            // comboBoxChildClass
            // 
            this.comboBoxChildClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxChildClass.FormattingEnabled = true;
            this.comboBoxChildClass.Location = new System.Drawing.Point(80, 39);
            this.comboBoxChildClass.Name = "comboBoxChildClass";
            this.comboBoxChildClass.Size = new System.Drawing.Size(177, 21);
            this.comboBoxChildClass.TabIndex = 3;
            this.comboBoxChildClass.SelectedIndexChanged += new System.EventHandler(this.comboBoxChildClass_SelectedIndexChanged);
            // 
            // comboBoxParentClass
            // 
            this.comboBoxParentClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxParentClass.FormattingEnabled = true;
            this.comboBoxParentClass.Location = new System.Drawing.Point(80, 11);
            this.comboBoxParentClass.Name = "comboBoxParentClass";
            this.comboBoxParentClass.Size = new System.Drawing.Size(177, 21);
            this.comboBoxParentClass.TabIndex = 2;
            this.comboBoxParentClass.SelectedIndexChanged += new System.EventHandler(this.comboBoxParentClass_SelectedIndexChanged);
            // 
            // buttonGeneralGo
            // 
            this.buttonGeneralGo.CornerRadius = new Ascend.CornerRadius(2);
            this.buttonGeneralGo.GradientLowColor = System.Drawing.Color.DarkGray;
            this.buttonGeneralGo.Location = new System.Drawing.Point(272, 70);
            this.buttonGeneralGo.Name = "buttonGeneralGo";
            this.buttonGeneralGo.Size = new System.Drawing.Size(45, 23);
            this.buttonGeneralGo.StayActiveAfterClick = false;
            this.buttonGeneralGo.TabIndex = 1;
            this.buttonGeneralGo.Text = "Go";
            this.buttonGeneralGo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonGeneralGo.Click += new System.EventHandler(this.buttonGeneralGo_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.labelNodeDescription);
            this.tabPage4.Controls.Add(this.checkBoxRemoveFileNameTrim);
            this.tabPage4.Controls.Add(this.treeViewPredefined);
            this.tabPage4.Controls.Add(this.buttonPredefinedGo);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(649, 448);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Predefined";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // labelNodeDescription
            // 
            this.labelNodeDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelNodeDescription.Location = new System.Drawing.Point(451, 56);
            this.labelNodeDescription.Name = "labelNodeDescription";
            this.labelNodeDescription.Size = new System.Drawing.Size(192, 383);
            this.labelNodeDescription.TabIndex = 5;
            // 
            // checkBoxRemoveFileNameTrim
            // 
            this.checkBoxRemoveFileNameTrim.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxRemoveFileNameTrim.AutoSize = true;
            this.checkBoxRemoveFileNameTrim.Enabled = false;
            this.checkBoxRemoveFileNameTrim.Location = new System.Drawing.Point(470, 32);
            this.checkBoxRemoveFileNameTrim.Name = "checkBoxRemoveFileNameTrim";
            this.checkBoxRemoveFileNameTrim.Size = new System.Drawing.Size(161, 17);
            this.checkBoxRemoveFileNameTrim.TabIndex = 4;
            this.checkBoxRemoveFileNameTrim.Text = "Remove File Name Trimming";
            this.checkBoxRemoveFileNameTrim.UseVisualStyleBackColor = true;
            this.checkBoxRemoveFileNameTrim.Visible = false;
            // 
            // treeViewPredefined
            // 
            this.treeViewPredefined.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewPredefined.Location = new System.Drawing.Point(3, 0);
            this.treeViewPredefined.Name = "treeViewPredefined";
            this.treeViewPredefined.ShowNodeToolTips = true;
            this.treeViewPredefined.Size = new System.Drawing.Size(441, 439);
            this.treeViewPredefined.TabIndex = 2;
            this.treeViewPredefined.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewPredefined_AfterSelect);
            // 
            // buttonPredefinedGo
            // 
            this.buttonPredefinedGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPredefinedGo.CornerRadius = new Ascend.CornerRadius(2);
            this.buttonPredefinedGo.GradientLowColor = System.Drawing.Color.DarkGray;
            this.buttonPredefinedGo.Location = new System.Drawing.Point(470, 3);
            this.buttonPredefinedGo.Name = "buttonPredefinedGo";
            this.buttonPredefinedGo.Size = new System.Drawing.Size(45, 23);
            this.buttonPredefinedGo.StayActiveAfterClick = false;
            this.buttonPredefinedGo.TabIndex = 1;
            this.buttonPredefinedGo.Text = "Go";
            this.buttonPredefinedGo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonPredefinedGo.Click += new System.EventHandler(this.buttonPredefinedGo_Click);
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.label1);
            this.tabPage7.Controls.Add(this.contextFilter);
            this.tabPage7.Controls.Add(this.button1);
            this.tabPage7.Controls.Add(this.txtCustom);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(649, 448);
            this.tabPage7.TabIndex = 4;
            this.tabPage7.Text = "Custom";
            this.tabPage7.UseVisualStyleBackColor = true;
            //
            // contextFilter
            //
            this.contextFilter.Name = "contextFilter";
            this.contextFilter.Dock = DockStyle.Top;
            this.contextFilter.Height = 165;
            this.contextFilter.Applied += new System.EventHandler(this.contextFilterApplied);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.CornerRadius = new Ascend.CornerRadius(2);
            this.button1.GradientLowColor = System.Drawing.Color.DarkGray;
            this.button1.Location = new System.Drawing.Point(586, 230);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(45, 23);
            this.button1.StayActiveAfterClick = false;
            this.button1.TabIndex = 2;
            this.button1.Text = "Go";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.Dock = DockStyle.Top;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 214);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "SQL Statement:";
            // 
            // txtCustom
            // 
            this.txtCustom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCustom.Location = new System.Drawing.Point(18, 200);
            this.txtCustom.Multiline = true;
            this.txtCustom.Name = "txtCustom";
            this.txtCustom.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCustom.Size = new System.Drawing.Size(560, 240);
            this.txtCustom.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.labelaAssociation2);
            this.tabPage3.Controls.Add(this.labelaChildChildClass);
            this.tabPage3.Controls.Add(this.comboBoxaAssociation2);
            this.tabPage3.Controls.Add(this.comboBoxaChildChildClass);
            this.tabPage3.Controls.Add(this.labelaAssociation1);
            this.tabPage3.Controls.Add(this.labelaChildClass);
            this.tabPage3.Controls.Add(this.labelaParentClass);
            this.tabPage3.Controls.Add(this.comboBoxaAssociation1);
            this.tabPage3.Controls.Add(this.comboBoxaChildClass);
            this.tabPage3.Controls.Add(this.comboBoxaParentClass);
            this.tabPage3.Controls.Add(this.buttonAdvancedGo);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(835, 201);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Advanced";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // labelaAssociation2
            // 
            this.labelaAssociation2.AutoSize = true;
            this.labelaAssociation2.Location = new System.Drawing.Point(484, 41);
            this.labelaAssociation2.Name = "labelaAssociation2";
            this.labelaAssociation2.Size = new System.Drawing.Size(87, 13);
            this.labelaAssociation2.TabIndex = 17;
            this.labelaAssociation2.Text = "Child Association";
            // 
            // labelaChildChildClass
            // 
            this.labelaChildChildClass.AutoSize = true;
            this.labelaChildChildClass.Location = new System.Drawing.Point(195, 41);
            this.labelaChildChildClass.Name = "labelaChildChildClass";
            this.labelaChildChildClass.Size = new System.Drawing.Size(84, 13);
            this.labelaChildChildClass.TabIndex = 16;
            this.labelaChildChildClass.Text = "Child Child Class";
            // 
            // comboBoxaAssociation2
            // 
            this.comboBoxaAssociation2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxaAssociation2.FormattingEnabled = true;
            this.comboBoxaAssociation2.Location = new System.Drawing.Point(577, 38);
            this.comboBoxaAssociation2.Name = "comboBoxaAssociation2";
            this.comboBoxaAssociation2.Size = new System.Drawing.Size(177, 21);
            this.comboBoxaAssociation2.TabIndex = 15;
            this.comboBoxaAssociation2.SelectedIndexChanged += new System.EventHandler(this.comboBoxaAssociation2_SelectedIndexChanged);
            // 
            // comboBoxaChildChildClass
            // 
            this.comboBoxaChildChildClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxaChildChildClass.FormattingEnabled = true;
            this.comboBoxaChildChildClass.Location = new System.Drawing.Point(301, 38);
            this.comboBoxaChildChildClass.Name = "comboBoxaChildChildClass";
            this.comboBoxaChildChildClass.Size = new System.Drawing.Size(177, 21);
            this.comboBoxaChildChildClass.TabIndex = 14;
            this.comboBoxaChildChildClass.SelectedIndexChanged += new System.EventHandler(this.comboBoxaChildChildClass_SelectedIndexChanged);
            // 
            // labelaAssociation1
            // 
            this.labelaAssociation1.AutoSize = true;
            this.labelaAssociation1.Location = new System.Drawing.Point(510, 14);
            this.labelaAssociation1.Name = "labelaAssociation1";
            this.labelaAssociation1.Size = new System.Drawing.Size(61, 13);
            this.labelaAssociation1.TabIndex = 13;
            this.labelaAssociation1.Text = "Association";
            // 
            // labelaChildClass
            // 
            this.labelaChildClass.AutoSize = true;
            this.labelaChildClass.Location = new System.Drawing.Point(263, 14);
            this.labelaChildClass.Name = "labelaChildClass";
            this.labelaChildClass.Size = new System.Drawing.Size(58, 13);
            this.labelaChildClass.TabIndex = 12;
            this.labelaChildClass.Text = "Child Class";
            // 
            // labelaParentClass
            // 
            this.labelaParentClass.AutoSize = true;
            this.labelaParentClass.Location = new System.Drawing.Point(8, 14);
            this.labelaParentClass.Name = "labelaParentClass";
            this.labelaParentClass.Size = new System.Drawing.Size(66, 13);
            this.labelaParentClass.TabIndex = 11;
            this.labelaParentClass.Text = "Parent Class";
            // 
            // comboBoxaAssociation1
            // 
            this.comboBoxaAssociation1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxaAssociation1.FormattingEnabled = true;
            this.comboBoxaAssociation1.Location = new System.Drawing.Point(577, 11);
            this.comboBoxaAssociation1.Name = "comboBoxaAssociation1";
            this.comboBoxaAssociation1.Size = new System.Drawing.Size(177, 21);
            this.comboBoxaAssociation1.TabIndex = 10;
            this.comboBoxaAssociation1.SelectedIndexChanged += new System.EventHandler(this.comboBoxaAssociation1_SelectedIndexChanged);
            // 
            // comboBoxaChildClass
            // 
            this.comboBoxaChildClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxaChildClass.FormattingEnabled = true;
            this.comboBoxaChildClass.Location = new System.Drawing.Point(327, 11);
            this.comboBoxaChildClass.Name = "comboBoxaChildClass";
            this.comboBoxaChildClass.Size = new System.Drawing.Size(177, 21);
            this.comboBoxaChildClass.TabIndex = 9;
            this.comboBoxaChildClass.SelectedIndexChanged += new System.EventHandler(this.comboBoxaChildClass_SelectedIndexChanged);
            // 
            // comboBoxaParentClass
            // 
            this.comboBoxaParentClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxaParentClass.FormattingEnabled = true;
            this.comboBoxaParentClass.Location = new System.Drawing.Point(80, 11);
            this.comboBoxaParentClass.Name = "comboBoxaParentClass";
            this.comboBoxaParentClass.Size = new System.Drawing.Size(177, 21);
            this.comboBoxaParentClass.TabIndex = 8;
            this.comboBoxaParentClass.SelectedIndexChanged += new System.EventHandler(this.comboBoxaParentClass_SelectedIndexChanged);
            // 
            // buttonAdvancedGo
            // 
            this.buttonAdvancedGo.CornerRadius = new Ascend.CornerRadius(2);
            this.buttonAdvancedGo.GradientLowColor = System.Drawing.Color.DarkGray;
            this.buttonAdvancedGo.Location = new System.Drawing.Point(773, 36);
            this.buttonAdvancedGo.Name = "buttonAdvancedGo";
            this.buttonAdvancedGo.Size = new System.Drawing.Size(45, 23);
            this.buttonAdvancedGo.StayActiveAfterClick = false;
            this.buttonAdvancedGo.TabIndex = 1;
            this.buttonAdvancedGo.Text = "Go";
            this.buttonAdvancedGo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonAdvancedGo.Click += new System.EventHandler(this.buttonAdvancedGo_Click);
            // 
            // labelInfo
            // 
            this.labelInfo.Location = new System.Drawing.Point(428, 140);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(81, 58);
            this.labelInfo.TabIndex = 2;
            this.labelInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelInfo.Visible = false;
            // 
            // comboBoxWorkspace
            // 
            this.comboBoxWorkspace.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWorkspace.FormattingEnabled = true;
            this.comboBoxWorkspace.Location = new System.Drawing.Point(273, 2);
            this.comboBoxWorkspace.Name = "comboBoxWorkspace";
            this.comboBoxWorkspace.Size = new System.Drawing.Size(240, 21);
            this.comboBoxWorkspace.TabIndex = 0;
            this.comboBoxWorkspace.SelectedIndexChanged += new System.EventHandler(this.comboBoxWorkspace_SelectedIndexChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripButton,
            this.saveToolStripButton,
            this.toolStripSeparator1,
            this.printToolStripButton,
            this.toolStripSeparator,
            this.toolStripButtonSelectAll,
            this.copyToolStripButton,
            this.toolStripSeparator2,
            this.toolStripLabel1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(671, 25);
            this.toolStrip1.TabIndex = 8;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // openToolStripButton
            // 
            this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripButton.Image")));
            this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripButton.Name = "openToolStripButton";
            this.openToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.openToolStripButton.Text = "&Open";
            this.openToolStripButton.Click += new System.EventHandler(this.buttonDataLoad_Click);
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton.Image")));
            this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.saveToolStripButton.Text = "&Save";
            this.saveToolStripButton.Click += new System.EventHandler(this.buttonDataSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // printToolStripButton
            // 
            this.printToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.printToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("printToolStripButton.Image")));
            this.printToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printToolStripButton.Name = "printToolStripButton";
            this.printToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.printToolStripButton.Text = "&Print";
            this.printToolStripButton.Click += new System.EventHandler(this.printToolStripButton_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonSelectAll
            // 
            this.toolStripButtonSelectAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonSelectAll.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSelectAll.Image")));
            this.toolStripButtonSelectAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSelectAll.Name = "toolStripButtonSelectAll";
            this.toolStripButtonSelectAll.Size = new System.Drawing.Size(57, 22);
            this.toolStripButtonSelectAll.Text = "Select All";
            this.toolStripButtonSelectAll.Click += new System.EventHandler(this.toolStripButtonSelectAll_Click);
            // 
            // copyToolStripButton
            // 
            this.copyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.copyToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripButton.Image")));
            this.copyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyToolStripButton.Name = "copyToolStripButton";
            this.copyToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.copyToolStripButton.Text = "&Copy";
            this.copyToolStripButton.Click += new System.EventHandler(this.copyToolStripButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(96, 22);
            this.toolStripLabel1.Text = "Workspace Filter:";
            this.toolStripLabel1.ToolTipText = "Clicking here will reset the filter";
            this.toolStripLabel1.Click += new System.EventHandler(this.toolStripLabel1_Click);
            // 
            // gridDataBoundGridMain
            // 
            this.gridDataBoundGridMain.ActivateCurrentCellBehavior = Syncfusion.Windows.Forms.Grid.GridCellActivateAction.SetCurrent;
            this.gridDataBoundGridMain.AllowDragSelectedCols = true;
            this.gridDataBoundGridMain.BackColor = System.Drawing.Color.White;
            gridBaseStyle1.Name = "Row Header";
            gridBaseStyle1.StyleInfo.BaseStyle = "Header";
            gridBaseStyle1.StyleInfo.CellType = "RowHeaderCell";
            gridBaseStyle1.StyleInfo.Enabled = true;
            gridBaseStyle1.StyleInfo.HorizontalAlignment = Syncfusion.Windows.Forms.Grid.GridHorizontalAlignment.Left;
            gridBaseStyle2.Name = "Column Header";
            gridBaseStyle2.StyleInfo.BaseStyle = "Header";
            gridBaseStyle2.StyleInfo.CellType = "ColumnHeaderCell";
            gridBaseStyle2.StyleInfo.Enabled = false;
            gridBaseStyle2.StyleInfo.Font.Bold = false;
            gridBaseStyle2.StyleInfo.HorizontalAlignment = Syncfusion.Windows.Forms.Grid.GridHorizontalAlignment.Center;
            gridBaseStyle2.StyleInfo.Interior = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.LightSteelBlue);
            gridBaseStyle3.Name = "Standard";
            gridBaseStyle3.StyleInfo.CheckBoxOptions.CheckedValue = "True";
            gridBaseStyle3.StyleInfo.CheckBoxOptions.UncheckedValue = "False";
            gridBaseStyle3.StyleInfo.Interior = new Syncfusion.Drawing.BrushInfo(System.Drawing.SystemColors.Window);
            gridBaseStyle4.Name = "Header";
            gridBaseStyle4.StyleInfo.Borders.Bottom = new Syncfusion.Windows.Forms.Grid.GridBorder(Syncfusion.Windows.Forms.Grid.GridBorderStyle.None);
            gridBaseStyle4.StyleInfo.Borders.Left = new Syncfusion.Windows.Forms.Grid.GridBorder(Syncfusion.Windows.Forms.Grid.GridBorderStyle.None);
            gridBaseStyle4.StyleInfo.Borders.Right = new Syncfusion.Windows.Forms.Grid.GridBorder(Syncfusion.Windows.Forms.Grid.GridBorderStyle.None);
            gridBaseStyle4.StyleInfo.Borders.Top = new Syncfusion.Windows.Forms.Grid.GridBorder(Syncfusion.Windows.Forms.Grid.GridBorderStyle.None);
            gridBaseStyle4.StyleInfo.CellType = "Header";
            gridBaseStyle4.StyleInfo.Font.Bold = true;
            gridBaseStyle4.StyleInfo.Interior = new Syncfusion.Drawing.BrushInfo(System.Drawing.SystemColors.Control);
            gridBaseStyle4.StyleInfo.VerticalAlignment = Syncfusion.Windows.Forms.Grid.GridVerticalAlignment.Middle;
            this.gridDataBoundGridMain.BaseStylesMap.AddRange(new Syncfusion.Windows.Forms.Grid.GridBaseStyle[] {
            gridBaseStyle1,
            gridBaseStyle2,
            gridBaseStyle3,
            gridBaseStyle4});
            this.gridDataBoundGridMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDataBoundGridMain.EnableAddNew = false;
            this.gridDataBoundGridMain.EnableEdit = false;
            this.gridDataBoundGridMain.EnableIntelliMouse = true;
            this.gridDataBoundGridMain.EnableRemove = false;
            this.gridDataBoundGridMain.ExcelLikeSelectionFrame = true;
            this.gridDataBoundGridMain.HighlightCurrentColumnHeader = true;
            this.gridDataBoundGridMain.Location = new System.Drawing.Point(3, 27);
            this.gridDataBoundGridMain.Name = "gridDataBoundGridMain";
            this.gridDataBoundGridMain.OptimizeInsertRemoveCells = true;
            this.gridDataBoundGridMain.ShowCurrentCellBorderBehavior = Syncfusion.Windows.Forms.Grid.GridShowCurrentCellBorder.GrayWhenLostFocus;
            this.gridDataBoundGridMain.Size = new System.Drawing.Size(657, 450);
            this.gridDataBoundGridMain.SmartSizeBox = false;
            this.gridDataBoundGridMain.SortBehavior = Syncfusion.Windows.Forms.Grid.GridSortBehavior.DoubleClick;
            this.gridDataBoundGridMain.TabIndex = 7;
            gridStyleInfo1.AutoSize = true;
            this.gridDataBoundGridMain.TableStyle = gridStyleInfo1;
            this.gridDataBoundGridMain.Text = "gridDataBoundGrid1";
            this.gridDataBoundGridMain.ThemesEnabled = true;
            this.gridDataBoundGridMain.UseListChangedEvent = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.AllowDrop = true;
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 25);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(671, 506);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.tabControlReports);
            this.tabPage5.Controls.Add(this.labelInfo);
            this.tabPage5.Location = new System.Drawing.Point(4, 4);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(663, 480);
            this.tabPage5.TabIndex = 0;
            this.tabPage5.Text = "Setup";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.gridDataBoundGridMain);
            this.tabPage6.Controls.Add(this.lblNoResults);
            this.tabPage6.Location = new System.Drawing.Point(4, 4);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(663, 480);
            this.tabPage6.TabIndex = 1;
            this.tabPage6.Text = "Report";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // lblNoResults
            // 
            this.lblNoResults.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblNoResults.Location = new System.Drawing.Point(3, 3);
            this.lblNoResults.Name = "lblNoResults";
            this.lblNoResults.Size = new System.Drawing.Size(657, 24);
            this.lblNoResults.TabIndex = 8;
            this.lblNoResults.Text = "The report yielded no results";
            this.lblNoResults.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNoResults.Visible = false;
            // 
            // bgWorker
            // 
            this.bgWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorker_DoWork);
            this.bgWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorker_RunWorkerCompleted);
            // 
            // checkBoxServer
            // 
            this.checkBoxServer.AutoSize = true;
            this.checkBoxServer.Location = new System.Drawing.Point(519, 4);
            this.checkBoxServer.Name = "checkBoxServer";
            this.checkBoxServer.Size = new System.Drawing.Size(79, 17);
            this.checkBoxServer.TabIndex = 10;
            this.checkBoxServer.Text = "Use Server";
            this.checkBoxServer.UseVisualStyleBackColor = false;
            // 
            // Reports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 530);
            this.Controls.Add(this.checkBoxServer);
            this.Controls.Add(this.comboBoxWorkspace);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Reports";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reports";
            this.Load += new System.EventHandler(this.Reports_Load);
            this.tabControlReports.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage7.ResumeLayout(false);
            this.tabPage7.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDataBoundGridMain)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlReports;
        private System.Windows.Forms.TabPage tabPage1;
        private MetaControls.MetaButton buttonBasicGo;
        private System.Windows.Forms.TabPage tabPage3;
        private MetaControls.MetaButton buttonAdvancedGo;
        private System.Windows.Forms.TabPage tabPage4;
        private MetaControls.MetaButton buttonPredefinedGo;
        private System.Windows.Forms.ComboBox comboBoxWorkspace;
        private System.Windows.Forms.ComboBox comboBoxObjectType;
        private System.Windows.Forms.Label labelObject;
        private System.Windows.Forms.TreeView treeViewPredefined;
        private System.Windows.Forms.Label labelaAssociation2;
        private System.Windows.Forms.Label labelaChildChildClass;
        private System.Windows.Forms.ComboBox comboBoxaAssociation2;
        private System.Windows.Forms.ComboBox comboBoxaChildChildClass;
        private System.Windows.Forms.Label labelaAssociation1;
        private System.Windows.Forms.Label labelaChildClass;
        private System.Windows.Forms.Label labelaParentClass;
        private System.Windows.Forms.ComboBox comboBoxaAssociation1;
        private System.Windows.Forms.ComboBox comboBoxaChildClass;
        private System.Windows.Forms.ComboBox comboBoxaParentClass;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label labelAssociation;
        private System.Windows.Forms.Label labelChildClass;
        private System.Windows.Forms.Label labelParentClass;
        private System.Windows.Forms.ComboBox comboBoxAssociation;
        private System.Windows.Forms.ComboBox comboBoxChildClass;
        private System.Windows.Forms.ComboBox comboBoxParentClass;
        private MetaControls.MetaButton buttonGeneralGo;
        private System.Windows.Forms.CheckBox checkBoxRemoveFileNameTrim;
        private ToolStrip toolStrip1;
        private ToolStripButton openToolStripButton;
        private ToolStripButton saveToolStripButton;
        private ToolStripButton printToolStripButton;
        private ToolStripButton copyToolStripButton;
        private ToolStripSeparator toolStripSeparator;
        private Syncfusion.Windows.Forms.Grid.GridDataBoundGrid gridDataBoundGridMain;
        private TabControl tabControl1;
        private TabPage tabPage5;
        private TabPage tabPage6;
        private TabPage tabPage7;
        private ContextFilter contextFilter;
        private MetaControls.MetaButton button1;
        private Label label1;
        private TextBox txtCustom;
        private ToolStripLabel toolStripLabel1;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton toolStripButtonSelectAll;
        private ToolStripSeparator toolStripSeparator2;
        private Label lblNoResults;
        private System.ComponentModel.BackgroundWorker bgWorker;
        private CheckBox checkBoxIncludeOrphans;
        private Label labelNodeDescription;
        private CheckBox checkBoxServer;

    }
}