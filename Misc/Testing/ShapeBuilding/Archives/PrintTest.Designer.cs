namespace ShapeBuilding
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
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor1 = new Northwoods.Go.Draw.GoRulerCursor();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyPrintDialog));
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor2 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor3 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor4 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor5 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor6 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor7 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor8 = new Northwoods.Go.Draw.GoRulerCursor();
            this.btnPageSetup = new System.Windows.Forms.Button();
            this.btnPrintPreview = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnSetupToFit = new System.Windows.Forms.Button();
            this.btnResizeToSheet = new System.Windows.Forms.Button();
            this.cbShowPageBreaks = new System.Windows.Forms.CheckBox();
            this.numAcross = new System.Windows.Forms.NumericUpDown();
            this.numDown = new System.Windows.Forms.NumericUpDown();
            this.myView = new ShapeBuilding.CustomPrinting.PrintableView();
            ((System.ComponentModel.ISupportInitialize)(this.numAcross)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDown)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPageSetup
            // 
            this.btnPageSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPageSetup.Location = new System.Drawing.Point(544, 0);
            this.btnPageSetup.Name = "btnPageSetup";
            this.btnPageSetup.Size = new System.Drawing.Size(138, 23);
            this.btnPageSetup.TabIndex = 1;
            this.btnPageSetup.Text = "Page Setup";
            this.btnPageSetup.UseVisualStyleBackColor = true;
            this.btnPageSetup.Click += new System.EventHandler(this.btnPageSetup_Click);
            // 
            // btnPrintPreview
            // 
            this.btnPrintPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintPreview.Location = new System.Drawing.Point(544, 58);
            this.btnPrintPreview.Name = "btnPrintPreview";
            this.btnPrintPreview.Size = new System.Drawing.Size(138, 23);
            this.btnPrintPreview.TabIndex = 1;
            this.btnPrintPreview.Text = "Print Preview";
            this.btnPrintPreview.UseVisualStyleBackColor = true;
            this.btnPrintPreview.Click += new System.EventHandler(this.btnPrintPreview_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Location = new System.Drawing.Point(544, 87);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(138, 23);
            this.btnPrint.TabIndex = 1;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnSetupToFit
            // 
            this.btnSetupToFit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetupToFit.Location = new System.Drawing.Point(544, 29);
            this.btnSetupToFit.Name = "btnSetupToFit";
            this.btnSetupToFit.Size = new System.Drawing.Size(138, 23);
            this.btnSetupToFit.TabIndex = 1;
            this.btnSetupToFit.Text = "Scaled Page Setup";
            this.btnSetupToFit.UseVisualStyleBackColor = true;
            this.btnSetupToFit.Click += new System.EventHandler(this.btnSetupToFit_Click);
            // 
            // btnResizeToSheet
            // 
            this.btnResizeToSheet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResizeToSheet.Location = new System.Drawing.Point(545, 347);
            this.btnResizeToSheet.Name = "btnResizeToSheet";
            this.btnResizeToSheet.Size = new System.Drawing.Size(138, 23);
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
            this.cbShowPageBreaks.Location = new System.Drawing.Point(565, 212);
            this.cbShowPageBreaks.Name = "cbShowPageBreaks";
            this.cbShowPageBreaks.Size = new System.Drawing.Size(117, 17);
            this.cbShowPageBreaks.TabIndex = 2;
            this.cbShowPageBreaks.Text = "Show Page Breaks";
            this.cbShowPageBreaks.UseVisualStyleBackColor = true;
            this.cbShowPageBreaks.CheckedChanged += new System.EventHandler(this.cbShowPageBreaks_CheckedChanged);
            // 
            // numAcross
            // 
            this.numAcross.Location = new System.Drawing.Point(641, 235);
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
            this.numDown.Location = new System.Drawing.Point(641, 261);
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
            goRulerCursor1.BackColor = System.Drawing.Color.White;
            goRulerCursor1.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor1.Height = 22;
            goRulerCursor1.Size = new System.Drawing.Size(1, 22);
            goRulerCursor1.Value = 0;
            goRulerCursor1.Width = 1;
            this.myView.BottomObjectCursor = goRulerCursor1;
            this.myView.BoundingHandlePenWidth = 7.443434F;
            this.myView.ContentBounds = ((System.Drawing.RectangleF)(resources.GetObject("myView.ContentBounds")));
            this.myView.ContextClickSingleSelection = false;
            this.myView.DisableKeys = Northwoods.Go.GoViewDisableKeys.None;
            this.myView.DocScale = 0.2686932F;
            this.myView.DragsRealtime = true;
            this.myView.ExternalDragDropsOnEnter = true;
            this.myView.FitAcross = 0;
            this.myView.FitDown = 0;
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
            this.myView.Name = "myView";
            this.myView.NoFocusSelectionColor = System.Drawing.Color.Chartreuse;
            this.myView.PrinterSettings = null;
            this.myView.PrintScale = 1F;
            this.myView.ResizeHandleHeight = 22.3303F;
            this.myView.ResizeHandlePenWidth = 3.721717F;
            this.myView.ResizeHandleWidth = 22.3303F;
            goRulerCursor5.BackColor = System.Drawing.Color.White;
            goRulerCursor5.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor5.Height = 22;
            goRulerCursor5.Size = new System.Drawing.Size(1, 22);
            goRulerCursor5.Value = 0;
            goRulerCursor5.Width = 1;
            this.myView.RightObjectCursor = goRulerCursor5;
            this.myView.SheetStyle = Northwoods.Go.GoViewSheetStyle.WholeSheet;
            this.myView.Size = new System.Drawing.Size(538, 382);
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
            // MyPrintDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 382);
            this.Controls.Add(this.numDown);
            this.Controls.Add(this.numAcross);
            this.Controls.Add(this.cbShowPageBreaks);
            this.Controls.Add(this.btnResizeToSheet);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnPrintPreview);
            this.Controls.Add(this.btnSetupToFit);
            this.Controls.Add(this.btnPageSetup);
            this.Controls.Add(this.myView);
            this.Name = "MyPrintDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Print Diagram";
            this.Load += new System.EventHandler(this.PrintTest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numAcross)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CustomPrinting.PrintableView myView;
        private System.Windows.Forms.Button btnPageSetup;
        private System.Windows.Forms.Button btnPrintPreview;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnSetupToFit;
        private System.Windows.Forms.Button btnResizeToSheet;
        private System.Windows.Forms.CheckBox cbShowPageBreaks;
        private System.Windows.Forms.NumericUpDown numAcross;
        private System.Windows.Forms.NumericUpDown numDown;
    }
}