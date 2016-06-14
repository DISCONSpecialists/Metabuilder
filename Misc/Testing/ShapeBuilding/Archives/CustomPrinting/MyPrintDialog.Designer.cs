namespace ShapeBuilding.Archives.CustomPrinting
{
    partial class MyPrintDialog
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
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor9 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor10 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor11 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor12 = new Northwoods.Go.Draw.GoRulerCursor();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyPrintDialog));
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor13 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor14 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor15 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor16 = new Northwoods.Go.Draw.GoRulerCursor();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnSetupToFit = new System.Windows.Forms.Button();
            this.btnResizeToSheet = new System.Windows.Forms.Button();
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
            this.radioReduce = new System.Windows.Forms.RadioButton();
            this.grpPrinter = new System.Windows.Forms.GroupBox();
            this.lblPrinterName = new System.Windows.Forms.LinkLabel();
            this.myView = new ShapeBuilding.Archives.CustomPrinting.PrintableView();
            this.radioInflate = new System.Windows.Forms.RadioButton();
            this.btnPrintPreview = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numAcross)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDown)).BeginInit();
            this.grpOrientation.SuspendLayout();
            this.grpScaling.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCopies)).BeginInit();
            this.grpPrinter.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Location = new System.Drawing.Point(583, 422);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(131, 22);
            this.btnPrint.TabIndex = 1;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnSetupToFit
            // 
            this.btnSetupToFit.Location = new System.Drawing.Point(149, 70);
            this.btnSetupToFit.Name = "btnSetupToFit";
            this.btnSetupToFit.Size = new System.Drawing.Size(59, 24);
            this.btnSetupToFit.TabIndex = 1;
            this.btnSetupToFit.Text = "Setup...";
            this.btnSetupToFit.UseVisualStyleBackColor = true;
            // 
            // btnResizeToSheet
            // 
            this.btnResizeToSheet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResizeToSheet.Location = new System.Drawing.Point(583, 366);
            this.btnResizeToSheet.Name = "btnResizeToSheet";
            this.btnResizeToSheet.Size = new System.Drawing.Size(131, 23);
            this.btnResizeToSheet.TabIndex = 1;
            this.btnResizeToSheet.Text = "Resize Frame to Sheet";
            this.btnResizeToSheet.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnResizeToSheet.UseVisualStyleBackColor = true;
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
            this.numAcross.Location = new System.Drawing.Point(44, 115);
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
            this.numDown.Location = new System.Drawing.Point(44, 139);
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
            this.label1.Location = new System.Drawing.Point(6, 115);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Fit to:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(92, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Page(s) Across";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(92, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Page(s) Down";
            // 
            // lblPageSize
            // 
            this.lblPageSize.AutoSize = true;
            this.lblPageSize.Location = new System.Drawing.Point(6, 54);
            this.lblPageSize.Name = "lblPageSize";
            this.lblPageSize.Size = new System.Drawing.Size(13, 13);
            this.lblPageSize.TabIndex = 4;
            this.lblPageSize.Text = "[]";
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
            // grpOrientation
            // 
            this.grpOrientation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpOrientation.Controls.Add(this.radioLandscape);
            this.grpOrientation.Controls.Add(this.radioPortrait);
            this.grpOrientation.Location = new System.Drawing.Point(539, 103);
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
            this.grpScaling.Location = new System.Drawing.Point(539, 153);
            this.grpScaling.Name = "grpScaling";
            this.grpScaling.Size = new System.Drawing.Size(214, 211);
            this.grpScaling.TabIndex = 7;
            this.grpScaling.TabStop = false;
            this.grpScaling.Text = "Scaling && Page Breaks";
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
            this.label5.Location = new System.Drawing.Point(92, 166);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Copies";
            // 
            // numCopies
            // 
            this.numCopies.Location = new System.Drawing.Point(44, 164);
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
            // 
            // grpPrinter
            // 
            this.grpPrinter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpPrinter.Controls.Add(this.lblPrinterName);
            this.grpPrinter.Controls.Add(this.btnSetupToFit);
            this.grpPrinter.Controls.Add(this.lblPageSize);
            this.grpPrinter.Location = new System.Drawing.Point(539, 0);
            this.grpPrinter.Name = "grpPrinter";
            this.grpPrinter.Size = new System.Drawing.Size(214, 100);
            this.grpPrinter.TabIndex = 8;
            this.grpPrinter.TabStop = false;
            this.grpPrinter.Text = "Printer && Page Size";
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
            this.myView.BackColor = System.Drawing.Color.White;
            this.myView.BackgroundHasSheet = true;
            this.myView.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            goRulerCursor9.BackColor = System.Drawing.Color.White;
            goRulerCursor9.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor9.Height = 22;
            goRulerCursor9.Size = new System.Drawing.Size(1, 22);
            goRulerCursor9.Value = 0;
            goRulerCursor9.Width = 1;
            this.myView.BottomObjectCursor = goRulerCursor9;
            this.myView.BoundingHandlePenWidth = 6.140625F;
            this.myView.ContextClickSingleSelection = false;
            this.myView.DisableKeys = Northwoods.Go.GoViewDisableKeys.None;
            this.myView.DocScale = 0.3256997F;
            this.myView.DragsRealtime = true;
            this.myView.ExternalDragDropsOnEnter = true;
            this.myView.GridCellSizeHeight = 25F;
            this.myView.GridCellSizeWidth = 25F;
            this.myView.GridLineDashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            this.myView.GridMajorLineColor = System.Drawing.Color.LightGray;
            this.myView.GridMajorLineFrequency = new System.Drawing.Size(4, 4);
            this.myView.GridStyle = Northwoods.Go.GoViewGridStyle.Line;
            this.myView.GridUnboundedSpots = 0;
            goRulerCursor10.BackColor = System.Drawing.Color.White;
            goRulerCursor10.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor10.Height = 22;
            goRulerCursor10.Size = new System.Drawing.Size(1, 22);
            goRulerCursor10.Value = 0;
            goRulerCursor10.Width = 1;
            this.myView.HorizontalCenterObjectCursor = goRulerCursor10;
            goRulerCursor11.BackColor = System.Drawing.Color.White;
            goRulerCursor11.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor11.Height = 22;
            goRulerCursor11.Size = new System.Drawing.Size(1, 22);
            goRulerCursor11.Value = 0;
            goRulerCursor11.Width = 1;
            this.myView.HorizontalRulerMouseCursor = goRulerCursor11;
            goRulerCursor12.BackColor = System.Drawing.Color.White;
            goRulerCursor12.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor12.Height = 22;
            goRulerCursor12.Size = new System.Drawing.Size(1, 22);
            goRulerCursor12.Value = 0;
            goRulerCursor12.Width = 1;
            this.myView.LeftObjectCursor = goRulerCursor12;
            this.myView.Location = new System.Drawing.Point(0, 0);
            
            this.myView.MyPrintSettings = new MetaBuilder.Graphing.Formatting.DiagramPrintSettings();
            this.myView.Name = "myView";
            this.myView.NoFocusSelectionColor = System.Drawing.Color.Chartreuse;
            this.myView.PrintScale = 1F;
            this.myView.ResizeHandleHeight = 18.42188F;
            this.myView.ResizeHandlePenWidth = 3.070313F;
            this.myView.ResizeHandleWidth = 18.42188F;
            goRulerCursor13.BackColor = System.Drawing.Color.White;
            goRulerCursor13.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor13.Height = 22;
            goRulerCursor13.Size = new System.Drawing.Size(1, 22);
            goRulerCursor13.Value = 0;
            goRulerCursor13.Width = 1;
            this.myView.RightObjectCursor = goRulerCursor13;
            this.myView.SheetStyle = Northwoods.Go.GoViewSheetStyle.WholeSheet;
            this.myView.ShowsNegativeCoordinates = false;
            this.myView.Size = new System.Drawing.Size(533, 444);
            this.myView.TabIndex = 0;
            this.myView.Text = "goDrawView1";
            goRulerCursor14.BackColor = System.Drawing.Color.White;
            goRulerCursor14.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor14.Height = 22;
            goRulerCursor14.Size = new System.Drawing.Size(1, 22);
            goRulerCursor14.Value = 0;
            goRulerCursor14.Width = 1;
            this.myView.TopObjectCursor = goRulerCursor14;
            goRulerCursor15.BackColor = System.Drawing.Color.White;
            goRulerCursor15.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor15.Height = 22;
            goRulerCursor15.Size = new System.Drawing.Size(1, 22);
            goRulerCursor15.Value = 0;
            goRulerCursor15.Width = 1;
            this.myView.VerticalCenterObjectCursor = goRulerCursor15;
            goRulerCursor16.BackColor = System.Drawing.Color.White;
            goRulerCursor16.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor16.Height = 22;
            goRulerCursor16.Size = new System.Drawing.Size(1, 22);
            goRulerCursor16.Value = 0;
            goRulerCursor16.Width = 1;
            this.myView.VerticalRulerMouseCursor = goRulerCursor16;
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
            // 
            // btnPrintPreview
            // 
            this.btnPrintPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintPreview.Location = new System.Drawing.Point(583, 394);
            this.btnPrintPreview.Name = "btnPrintPreview";
            this.btnPrintPreview.Size = new System.Drawing.Size(131, 22);
            this.btnPrintPreview.TabIndex = 1;
            this.btnPrintPreview.Text = "Print Preview";
            this.btnPrintPreview.UseVisualStyleBackColor = true;
            this.btnPrintPreview.Click += new System.EventHandler(this.btnPrintPreview_Click);
            // 
            // MyPrintDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 444);
            this.Controls.Add(this.grpPrinter);
            this.Controls.Add(this.grpScaling);
            this.Controls.Add(this.grpOrientation);
            this.Controls.Add(this.btnResizeToSheet);
            this.Controls.Add(this.btnPrintPreview);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.myView);
            this.Name = "MyPrintDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
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
            this.ResumeLayout(false);

        }

        #endregion

        private CustomPrinting.PrintableView myView;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnSetupToFit;
        private System.Windows.Forms.Button btnResizeToSheet;
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
        private System.Windows.Forms.Button btnPrintPreview;
    }
}