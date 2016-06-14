namespace MetaBuilder.UIControls.GraphingUI.CustomPrinting
{
    partial class MyPrintDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor1 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor2 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor3 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor4 = new Northwoods.Go.Draw.GoRulerCursor();
            MetaBuilder.Graphing.Formatting.DiagramPrintSettings diagramPrintSettings1 = new MetaBuilder.Graphing.Formatting.DiagramPrintSettings();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyPrintDialog));
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor5 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor6 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor7 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor8 = new Northwoods.Go.Draw.GoRulerCursor();
            this.btnPrint = new MetaControls.MetaButton();
            this.btnSetupToFit = new MetaControls.MetaButton();
            this.btnResizeToSheet = new MetaControls.MetaButton();
            this.cbShowPageBreaks = new System.Windows.Forms.CheckBox();
            this.numAcross = new System.Windows.Forms.NumericUpDown();
            this.numDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblPageSize = new System.Windows.Forms.Label();
            this.radioPortrait = new System.Windows.Forms.RadioButton();
            this.radioLandscape = new System.Windows.Forms.RadioButton();
            this.grpOrientation = new System.Windows.Forms.GroupBox();
            this.grpScaling = new System.Windows.Forms.GroupBox();
            this.cbBlackAndWhite = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.numCopies = new System.Windows.Forms.NumericUpDown();
            this.radioDontScale = new System.Windows.Forms.RadioButton();
            this.radioInflate = new System.Windows.Forms.RadioButton();
            this.radioReduce = new System.Windows.Forms.RadioButton();
            this.grpPrinter = new System.Windows.Forms.GroupBox();
            this.buttonRefresh = new MetaControls.MetaButton();
            this.comboBoxPage = new System.Windows.Forms.ComboBox();
            this.comboBoxPrinter = new System.Windows.Forms.ComboBox();
            this.lblPrinterName = new System.Windows.Forms.LinkLabel();
            this.myView = new MetaBuilder.UIControls.GraphingUI.CustomPrinting.PrintableView();
            this.groupBoxMargins = new System.Windows.Forms.GroupBox();
            this.numericUpDownBottom = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownRight = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.numericUpDownTop = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.numericUpDownLeft = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numAcross)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDown)).BeginInit();
            this.grpOrientation.SuspendLayout();
            this.grpScaling.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCopies)).BeginInit();
            this.grpPrinter.SuspendLayout();
            this.groupBoxMargins.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBottom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLeft)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Location = new System.Drawing.Point(763, 477);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(131, 22);
            this.btnPrint.TabIndex = 1;
            this.btnPrint.Text = "Print";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnSetupToFit
            // 
            this.btnSetupToFit.Location = new System.Drawing.Point(96, 69);
            this.btnSetupToFit.Name = "btnSetupToFit";
            this.btnSetupToFit.Size = new System.Drawing.Size(112, 24);
            this.btnSetupToFit.TabIndex = 1;
            this.btnSetupToFit.Text = "Printer Preferences";
            this.btnSetupToFit.Click += new System.EventHandler(this.btnSetupToFit_Click);
            // 
            // btnResizeToSheet
            // 
            this.btnResizeToSheet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResizeToSheet.Location = new System.Drawing.Point(763, 451);
            this.btnResizeToSheet.Name = "btnResizeToSheet";
            this.btnResizeToSheet.Size = new System.Drawing.Size(131, 23);
            this.btnResizeToSheet.TabIndex = 1;
            this.btnResizeToSheet.Text = "Resize Frame to Sheet";
            this.btnResizeToSheet.Click += new System.EventHandler(this.btnResizeToSheet_Click);
            // 
            // cbShowPageBreaks
            // 
            this.cbShowPageBreaks.AutoSize = true;
            this.cbShowPageBreaks.Checked = true;
            this.cbShowPageBreaks.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowPageBreaks.Location = new System.Drawing.Point(44, 88);
            this.cbShowPageBreaks.Name = "cbShowPageBreaks";
            this.cbShowPageBreaks.Size = new System.Drawing.Size(117, 17);
            this.cbShowPageBreaks.TabIndex = 2;
            this.cbShowPageBreaks.Text = "Show Page Breaks";
            this.cbShowPageBreaks.UseVisualStyleBackColor = true;
            this.cbShowPageBreaks.CheckedChanged += new System.EventHandler(this.cbShowPageBreaks_CheckedChanged);
            // 
            // numAcross
            // 
            this.numAcross.Location = new System.Drawing.Point(44, 15);
            this.numAcross.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numAcross.Name = "numAcross";
            this.numAcross.Size = new System.Drawing.Size(41, 20);
            this.numAcross.TabIndex = 3;
            this.numAcross.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numAcross.ValueChanged += new System.EventHandler(this.numAcross_ValueChanged);
            // 
            // numDown
            // 
            this.numDown.Location = new System.Drawing.Point(44, 39);
            this.numDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDown.Name = "numDown";
            this.numDown.Size = new System.Drawing.Size(41, 20);
            this.numDown.TabIndex = 3;
            this.numDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDown.ValueChanged += new System.EventHandler(this.numDown_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Fit to:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(92, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Page(s) Across";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(92, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Page(s) Down";
            // 
            // lblPageSize
            // 
            this.lblPageSize.AutoSize = true;
            this.lblPageSize.Location = new System.Drawing.Point(8, 56);
            this.lblPageSize.Name = "lblPageSize";
            this.lblPageSize.Size = new System.Drawing.Size(13, 13);
            this.lblPageSize.TabIndex = 4;
            this.lblPageSize.Text = "[]";
            this.lblPageSize.Visible = false;
            // 
            // radioPortrait
            // 
            this.radioPortrait.AutoSize = true;
            this.radioPortrait.Checked = true;
            this.radioPortrait.Location = new System.Drawing.Point(11, 19);
            this.radioPortrait.Name = "radioPortrait";
            this.radioPortrait.Size = new System.Drawing.Size(58, 17);
            this.radioPortrait.TabIndex = 5;
            this.radioPortrait.TabStop = true;
            this.radioPortrait.Text = "Portrait";
            this.radioPortrait.UseVisualStyleBackColor = true;
            this.radioPortrait.Click += new System.EventHandler(this.radioOrientationChanged);
            // 
            // radioLandscape
            // 
            this.radioLandscape.AutoSize = true;
            this.radioLandscape.Location = new System.Drawing.Point(84, 19);
            this.radioLandscape.Name = "radioLandscape";
            this.radioLandscape.Size = new System.Drawing.Size(78, 17);
            this.radioLandscape.TabIndex = 5;
            this.radioLandscape.Text = "Landscape";
            this.radioLandscape.UseVisualStyleBackColor = true;
            this.radioLandscape.Click += new System.EventHandler(this.radioOrientationChanged);
            // 
            // grpOrientationes
            // 
            this.grpOrientation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpOrientation.Controls.Add(this.radioLandscape);
            this.grpOrientation.Controls.Add(this.radioPortrait);
            this.grpOrientation.Location = new System.Drawing.Point(680, 109);
            this.grpOrientation.Name = "grpOrientation";
            this.grpOrientation.Size = new System.Drawing.Size(214, 44);
            this.grpOrientation.TabIndex = 6;
            this.grpOrientation.TabStop = false;
            this.grpOrientation.Text = "Orientation";
            // 
            // grpScaling
            // 
            this.grpScaling.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpScaling.Controls.Add(this.cbBlackAndWhite);
            this.grpScaling.Controls.Add(this.label5);
            this.grpScaling.Controls.Add(this.numCopies);
            this.grpScaling.Controls.Add(this.radioDontScale);
            this.grpScaling.Controls.Add(this.radioInflate);
            this.grpScaling.Controls.Add(this.radioReduce);
            this.grpScaling.Controls.Add(this.cbShowPageBreaks);
            this.grpScaling.Controls.Add(this.label3);
            this.grpScaling.Controls.Add(this.numAcross);
            this.grpScaling.Controls.Add(this.numDown);
            this.grpScaling.Controls.Add(this.label1);
            this.grpScaling.Controls.Add(this.label2);
            this.grpScaling.Location = new System.Drawing.Point(680, 234);
            this.grpScaling.Name = "grpScaling";
            this.grpScaling.Size = new System.Drawing.Size(214, 111);
            this.grpScaling.TabIndex = 7;
            this.grpScaling.TabStop = false;
            this.grpScaling.Text = "Page Breaks";
            // 
            // cbBlackAndWhite
            // 
            this.cbBlackAndWhite.AutoSize = true;
            this.cbBlackAndWhite.Location = new System.Drawing.Point(44, 190);
            this.cbBlackAndWhite.Name = "cbBlackAndWhite";
            this.cbBlackAndWhite.Size = new System.Drawing.Size(129, 17);
            this.cbBlackAndWhite.TabIndex = 8;
            this.cbBlackAndWhite.Text = "Print Black and White";
            this.cbBlackAndWhite.UseVisualStyleBackColor = true;
            this.cbBlackAndWhite.CheckedChanged += new System.EventHandler(this.cbBlackAndWhite_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(92, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Copies";
            // 
            // numCopies
            // 
            this.numCopies.Location = new System.Drawing.Point(44, 64);
            this.numCopies.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCopies.Name = "numCopies";
            this.numCopies.Size = new System.Drawing.Size(42, 20);
            this.numCopies.TabIndex = 6;
            this.numCopies.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // radioDontScale
            // 
            this.radioDontScale.AutoSize = true;
            this.radioDontScale.Checked = true;
            this.radioDontScale.Location = new System.Drawing.Point(44, 19);
            this.radioDontScale.Name = "radioDontScale";
            this.radioDontScale.Size = new System.Drawing.Size(58, 17);
            this.radioDontScale.TabIndex = 5;
            this.radioDontScale.TabStop = true;
            this.radioDontScale.Text = "Normal";
            this.radioDontScale.UseVisualStyleBackColor = true;
            this.radioDontScale.CheckedChanged += new System.EventHandler(this.cbScaleToFit_CheckedChanged_1);
            this.radioDontScale.Visible = false;
            // 
            // radioInflate
            // 
            this.radioInflate.AutoSize = true;
            this.radioInflate.Location = new System.Drawing.Point(44, 65);
            this.radioInflate.Name = "radioInflate";
            this.radioInflate.Size = new System.Drawing.Size(120, 17);
            this.radioInflate.TabIndex = 5;
            this.radioInflate.Text = "Inflate to Paper Size";
            this.radioInflate.UseVisualStyleBackColor = true;
            this.radioInflate.CheckedChanged += new System.EventHandler(this.cbScaleToFit_CheckedChanged_1);
            this.radioInflate.Visible = false;
            // 
            // radioReduce
            // 
            this.radioReduce.AutoSize = true;
            this.radioReduce.Location = new System.Drawing.Point(44, 42);
            this.radioReduce.Name = "radioReduce";
            this.radioReduce.Size = new System.Drawing.Size(129, 17);
            this.radioReduce.TabIndex = 5;
            this.radioReduce.Text = "Reduce to Paper Size";
            this.radioReduce.UseVisualStyleBackColor = true;
            this.radioReduce.CheckedChanged += new System.EventHandler(this.cbScaleToFit_CheckedChanged_1);
            this.radioReduce.Visible = false;
            // 
            // grpPrinter
            // 
            this.grpPrinter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpPrinter.Controls.Add(this.buttonRefresh);
            this.grpPrinter.Controls.Add(this.comboBoxPage);
            this.grpPrinter.Controls.Add(this.comboBoxPrinter);
            this.grpPrinter.Controls.Add(this.lblPrinterName);
            this.grpPrinter.Controls.Add(this.btnSetupToFit);
            this.grpPrinter.Controls.Add(this.lblPageSize);
            this.grpPrinter.Location = new System.Drawing.Point(679, 12);
            this.grpPrinter.Name = "grpPrinter";
            this.grpPrinter.Size = new System.Drawing.Size(214, 96);
            this.grpPrinter.TabIndex = 8;
            this.grpPrinter.TabStop = false;
            this.grpPrinter.Text = "Printer && Page Size";
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(189, 14);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(19, 23);
            this.buttonRefresh.TabIndex = 8;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.Visible = false;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // comboBoxPage
            // 
            this.comboBoxPage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPage.FormattingEnabled = true;
            this.comboBoxPage.Location = new System.Drawing.Point(11, 42);
            this.comboBoxPage.Name = "comboBoxPage";
            this.comboBoxPage.Size = new System.Drawing.Size(197, 21);
            this.comboBoxPage.TabIndex = 7;
            this.comboBoxPage.SelectedIndexChanged += new System.EventHandler(this.comboBoxPage_SelectedIndexChanged);
            // 
            // comboBoxPrinter
            // 
            this.comboBoxPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPrinter.FormattingEnabled = true;
            this.comboBoxPrinter.Location = new System.Drawing.Point(11, 15);
            this.comboBoxPrinter.Name = "comboBoxPrinter";
            this.comboBoxPrinter.Size = new System.Drawing.Size(197, 21);
            this.comboBoxPrinter.TabIndex = 6;
            this.comboBoxPrinter.SelectedIndexChanged += new System.EventHandler(this.comboBoxPrinter_SelectedIndexChanged);
            // 
            // lblPrinterName
            // 
            this.lblPrinterName.AutoSize = true;
            this.lblPrinterName.Location = new System.Drawing.Point(8, 16);
            this.lblPrinterName.Name = "lblPrinterName";
            this.lblPrinterName.Size = new System.Drawing.Size(13, 13);
            this.lblPrinterName.TabIndex = 5;
            this.lblPrinterName.TabStop = true;
            this.lblPrinterName.Text = "[]";
            this.lblPrinterName.Visible = false;
            this.lblPrinterName.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblPrinterName_LinkClicked);
            // 
            // myView
            // 
            this.myView.AllowCopy = false;
            this.myView.AllowDelete = false;
            this.myView.AllowDragOut = false;
            this.myView.AllowDrop = false;
            this.myView.AllowEdit = false;
            this.myView.AllowInsert = false;
            this.myView.AllowKey = false;
            this.myView.AllowLink = false;
            this.myView.AllowReshape = false;
            this.myView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.myView.ArrowMoveLarge = 10F;
            this.myView.ArrowMoveSmall = 1F;
            this.myView.BackColor = System.Drawing.Color.LightGray;
            this.myView.BackgroundHasSheet = true;
            this.myView.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            goRulerCursor1.BackColor = System.Drawing.Color.White;
            goRulerCursor1.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor1.Height = 22;
            goRulerCursor1.Size = new System.Drawing.Size(1, 22);
            goRulerCursor1.Value = 0;
            goRulerCursor1.Width = 1;
            this.myView.BottomObjectCursor = goRulerCursor1;
            this.myView.BoundingHandlePenWidth = 5.253042F;
            this.myView.ContextClickSingleSelection = false;
            this.myView.DisableKeys = Northwoods.Go.GoViewDisableKeys.None;
            //this.myView.DocScale = 0.8F;
            this.myView.DragsRealtime = true;
            this.myView.ExternalDragDropsOnEnter = true;
            this.myView.GridCellSizeHeight = 25F;
            this.myView.GridCellSizeWidth = 25F;
            this.myView.GridLineDashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            this.myView.GridMajorLineColor = System.Drawing.Color.LightGray;
            this.myView.GridMajorLineFrequency = new System.Drawing.Size(4, 4);
            this.myView.GridStyle = Northwoods.Go.GoViewGridStyle.Line;
            this.myView.GridUnboundedSpots = 0;
            goRulerCursor2.BackColor = System.Drawing.Color.White;
            goRulerCursor2.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor2.Height = 22;
            goRulerCursor2.Size = new System.Drawing.Size(1, 22);
            goRulerCursor2.Value = 0;
            goRulerCursor2.Width = 1;
            this.myView.HorizontalCenterObjectCursor = goRulerCursor2;
            goRulerCursor3.BackColor = System.Drawing.Color.White;
            goRulerCursor3.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor3.Height = 22;
            goRulerCursor3.Size = new System.Drawing.Size(1, 22);
            goRulerCursor3.Value = 0;
            goRulerCursor3.Width = 1;
            this.myView.HorizontalRulerMouseCursor = goRulerCursor3;
            goRulerCursor4.BackColor = System.Drawing.Color.White;
            goRulerCursor4.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor4.Height = 22;
            goRulerCursor4.Size = new System.Drawing.Size(1, 22);
            goRulerCursor4.Value = 0;
            goRulerCursor4.Width = 1;
            this.myView.LeftObjectCursor = goRulerCursor4;
            this.myView.Location = new System.Drawing.Point(0, 0);
            diagramPrintSettings1.Adjustment = MetaBuilder.Graphing.Formatting.DiagramPrintSettings.PageAdjustment.Normal;
            diagramPrintSettings1.Cancelled = false;
            diagramPrintSettings1.ContentBounds = ((System.Drawing.RectangleF)(resources.GetObject("diagramPrintSettings1.ContentBounds")));
            diagramPrintSettings1.Copies = 0;
            diagramPrintSettings1.FitAcross = 1;
            diagramPrintSettings1.FitDown = 1;
            diagramPrintSettings1.HardMarginX = 0F;
            diagramPrintSettings1.HardMarginY = 0F;
            diagramPrintSettings1.PageSet = ((System.Drawing.Printing.PageSettings)(resources.GetObject("diagramPrintSettings1.PageSet")));
            diagramPrintSettings1.PrintSettings = ((System.Drawing.Printing.PrinterSettings)(resources.GetObject("diagramPrintSettings1.PrintSettings")));
            diagramPrintSettings1.Ratio = 1F;
            diagramPrintSettings1.SheetBounds = ((System.Drawing.RectangleF)(resources.GetObject("diagramPrintSettings1.SheetBounds")));
            diagramPrintSettings1.ShowPageBreaks = false;
            this.myView.MyPrintSettings = diagramPrintSettings1;
            this.myView.Name = "myView";
            this.myView.NoFocusSelectionColor = System.Drawing.Color.Chartreuse;
            this.myView.PrintScale = 1F;
            this.myView.ResizeHandleHeight = 15.75913F;
            this.myView.ResizeHandlePenWidth = 2.626521F;
            this.myView.ResizeHandleWidth = 15.75913F;
            goRulerCursor5.BackColor = System.Drawing.Color.White;
            goRulerCursor5.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor5.Height = 22;
            goRulerCursor5.Size = new System.Drawing.Size(1, 22);
            goRulerCursor5.Value = 0;
            goRulerCursor5.Width = 1;
            this.myView.RightObjectCursor = goRulerCursor5;
            this.myView.SheetStyle = Northwoods.Go.GoViewSheetStyle.WholeSheet;
            this.myView.ShowsNegativeCoordinates = false;
            this.myView.Size = new System.Drawing.Size(673, 511);
            this.myView.TabIndex = 0;
            this.myView.Text = "goDrawView1";
            goRulerCursor6.BackColor = System.Drawing.Color.White;
            goRulerCursor6.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor6.Height = 22;
            goRulerCursor6.Size = new System.Drawing.Size(1, 22);
            goRulerCursor6.Value = 0;
            goRulerCursor6.Width = 1;
            this.myView.TopObjectCursor = goRulerCursor6;
            goRulerCursor7.BackColor = System.Drawing.Color.White;
            goRulerCursor7.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor7.Height = 22;
            goRulerCursor7.Size = new System.Drawing.Size(1, 22);
            goRulerCursor7.Value = 0;
            goRulerCursor7.Width = 1;
            this.myView.VerticalCenterObjectCursor = goRulerCursor7;
            goRulerCursor8.BackColor = System.Drawing.Color.White;
            goRulerCursor8.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor8.Height = 22;
            goRulerCursor8.Size = new System.Drawing.Size(1, 22);
            goRulerCursor8.Value = 0;
            goRulerCursor8.Width = 1;
            this.myView.VerticalRulerMouseCursor = goRulerCursor8;
            // 
            // groupBoxMargins
            // 
            this.groupBoxMargins.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxMargins.Controls.Add(this.numericUpDownBottom);
            this.groupBoxMargins.Controls.Add(this.numericUpDownRight);
            this.groupBoxMargins.Controls.Add(this.label8);
            this.groupBoxMargins.Controls.Add(this.numericUpDownTop);
            this.groupBoxMargins.Controls.Add(this.label6);
            this.groupBoxMargins.Controls.Add(this.label7);
            this.groupBoxMargins.Controls.Add(this.numericUpDownLeft);
            this.groupBoxMargins.Controls.Add(this.label4);
            this.groupBoxMargins.Location = new System.Drawing.Point(680, 159);
            this.groupBoxMargins.Name = "groupBoxMargins";
            this.groupBoxMargins.Size = new System.Drawing.Size(214, 69);
            this.groupBoxMargins.TabIndex = 9;
            this.groupBoxMargins.TabStop = false;
            this.groupBoxMargins.Text = "Margins";
            // 
            // numericUpDownBottom
            // 
            this.numericUpDownBottom.Location = new System.Drawing.Point(158, 42);
            this.numericUpDownBottom.Maximum = new decimal(new int[] {
            75,
            0,
            0,
            0});
            this.numericUpDownBottom.Name = "numericUpDownBottom";
            this.numericUpDownBottom.Size = new System.Drawing.Size(37, 20);
            this.numericUpDownBottom.TabIndex = 1;
            this.numericUpDownBottom.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numericUpDownBottom.ValueChanged += new System.EventHandler(this.margin_ValueChanged);
            // 
            // numericUpDownRight
            // 
            this.numericUpDownRight.Location = new System.Drawing.Point(158, 20);
            this.numericUpDownRight.Maximum = new decimal(new int[] {
            75,
            0,
            0,
            0});
            this.numericUpDownRight.Name = "numericUpDownRight";
            this.numericUpDownRight.Size = new System.Drawing.Size(37, 20);
            this.numericUpDownRight.TabIndex = 1;
            this.numericUpDownRight.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numericUpDownRight.ValueChanged += new System.EventHandler(this.margin_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(112, 43);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Bottom";
            // 
            // numericUpDownTop
            // 
            this.numericUpDownTop.Location = new System.Drawing.Point(44, 41);
            this.numericUpDownTop.Maximum = new decimal(new int[] {
            75,
            0,
            0,
            0});
            this.numericUpDownTop.Name = "numericUpDownTop";
            this.numericUpDownTop.Size = new System.Drawing.Size(37, 20);
            this.numericUpDownTop.TabIndex = 1;
            this.numericUpDownTop.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numericUpDownTop.ValueChanged += new System.EventHandler(this.margin_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(112, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Right";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 41);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(26, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Top";
            // 
            // numericUpDownLeft
            // 
            this.numericUpDownLeft.Location = new System.Drawing.Point(44, 19);
            this.numericUpDownLeft.Maximum = new decimal(new int[] {
            75,
            0,
            0,
            0});
            this.numericUpDownLeft.Name = "numericUpDownLeft";
            this.numericUpDownLeft.Size = new System.Drawing.Size(37, 20);
            this.numericUpDownLeft.TabIndex = 1;
            this.numericUpDownLeft.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numericUpDownLeft.ValueChanged += new System.EventHandler(this.margin_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Left";
            // 
            // MyPrintDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(905, 511);
            this.Controls.Add(this.groupBoxMargins);
            this.Controls.Add(this.grpPrinter);
            this.Controls.Add(this.grpScaling);
            this.Controls.Add(this.grpOrientation);
            this.Controls.Add(this.btnResizeToSheet);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.myView);
            this.Name = "MyPrintDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Print Preview";
            this.Load += new System.EventHandler(this.PrintTest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numAcross)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDown)).EndInit();
            this.grpOrientation.ResumeLayout(false);
            this.grpOrientation.PerformLayout();
            this.grpScaling.ResumeLayout(false);
            this.grpScaling.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCopies)).EndInit();
            this.grpPrinter.ResumeLayout(false);
            this.grpPrinter.PerformLayout();
            this.groupBoxMargins.ResumeLayout(false);
            this.groupBoxMargins.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBottom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLeft)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CustomPrinting.PrintableView myView;
        private MetaControls.MetaButton btnPrint;
        private MetaControls.MetaButton btnSetupToFit;
        private MetaControls.MetaButton btnResizeToSheet;
        private System.Windows.Forms.CheckBox cbShowPageBreaks;
        private System.Windows.Forms.NumericUpDown numAcross;
        private System.Windows.Forms.NumericUpDown numDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblPageSize;
        private System.Windows.Forms.RadioButton radioPortrait;
        private System.Windows.Forms.RadioButton radioLandscape;
        private System.Windows.Forms.GroupBox grpOrientation;
        private System.Windows.Forms.GroupBox grpScaling;
        private System.Windows.Forms.GroupBox grpPrinter;
        private System.Windows.Forms.RadioButton radioReduce;
        private System.Windows.Forms.RadioButton radioDontScale;
        private System.Windows.Forms.NumericUpDown numCopies;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox cbBlackAndWhite;
        private System.Windows.Forms.LinkLabel lblPrinterName;
        private System.Windows.Forms.RadioButton radioInflate;
        private System.Windows.Forms.ComboBox comboBoxPage;
        private System.Windows.Forms.ComboBox comboBoxPrinter;
        private System.Windows.Forms.GroupBox groupBoxMargins;
        private System.Windows.Forms.NumericUpDown numericUpDownBottom;
        private System.Windows.Forms.NumericUpDown numericUpDownRight;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numericUpDownTop;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numericUpDownLeft;
        private System.Windows.Forms.Label label4;
        private MetaControls.MetaButton buttonRefresh;
    }
}