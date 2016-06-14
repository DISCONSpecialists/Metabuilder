using System.Drawing;
using System.Windows.Forms;


namespace MetaBuilder.UIControls.GraphingUI
{
    partial class LiteGraphViewContainer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LiteGraphViewContainer));
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor10 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor11 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor12 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor13 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor14 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor15 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor16 = new Northwoods.Go.Draw.GoRulerCursor();
            this.contextViewer1 = new MetaBuilder.Graphing.Tools.ContextViewer();
            this.myButton = new MetaControls.MetaButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.StripButtonOpen = new System.Windows.Forms.ToolStripButton();
            this.StripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.StripButtonPrint = new System.Windows.Forms.ToolStripButton();
            this.StripButtonWorkspaceLegend = new System.Windows.Forms.ToolStripButton();
            this.StripButtonFilter = new System.Windows.Forms.ToolStripButton();
            this.ConvertContextToDocument = new System.Windows.Forms.ToolStripButton(); 
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextViewer1
            // 
            this.contextViewer1.AllowLink = false;
            this.contextViewer1.ArrowMoveLarge = 10F;
            this.contextViewer1.ArrowMoveSmall = 1F;
            this.contextViewer1.BackColor = System.Drawing.Color.White;
            this.contextViewer1.BackgroundHasSheet = true;
            goRulerCursor9.BackColor = System.Drawing.Color.White;
            goRulerCursor9.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor9.Height = 22;
            goRulerCursor9.Size = new System.Drawing.Size(1, 22);
            goRulerCursor9.Value = 0;
            goRulerCursor9.Width = 1;
            this.contextViewer1.BottomObjectCursor = goRulerCursor9;
            this.contextViewer1.BoundingHandlePenWidth = 8F;
            this.contextViewer1.ClassesShown = ((System.Collections.Generic.List<string>)(resources.GetObject("contextViewer1.ClassesShown")));
            this.contextViewer1.ContextClickSingleSelection = false;
            this.contextViewer1.DisableKeys = Northwoods.Go.GoViewDisableKeys.ArrowScroll;
            this.contextViewer1.DocExtentCenter = ((System.Drawing.PointF)(resources.GetObject("contextViewer1.DocExtentCenter")));
            this.contextViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contextViewer1.DocScale = 0.25F;
            this.contextViewer1.DragsRealtime = true;
            this.contextViewer1.ExternalDragDropsOnEnter = true;
            this.contextViewer1.GridCellSizeHeight = 25F;
            this.contextViewer1.GridCellSizeWidth = 25F;
            this.contextViewer1.GridLineDashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            this.contextViewer1.GridMajorLineColor = System.Drawing.Color.LightGray;
            this.contextViewer1.GridMajorLineFrequency = new System.Drawing.Size(4, 4);
            this.contextViewer1.GridStyle = Northwoods.Go.GoViewGridStyle.Line;
            this.contextViewer1.GridUnboundedSpots = 0;
            goRulerCursor10.BackColor = System.Drawing.Color.White;
            goRulerCursor10.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor10.Height = 22;
            goRulerCursor10.Size = new System.Drawing.Size(1, 22);
            goRulerCursor10.Value = 0;
            goRulerCursor10.Width = 1;
            this.contextViewer1.HorizontalCenterObjectCursor = goRulerCursor10;
            goRulerCursor11.BackColor = System.Drawing.Color.White;
            goRulerCursor11.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor11.Height = 22;
            goRulerCursor11.Size = new System.Drawing.Size(1, 22);
            goRulerCursor11.Value = 0;
            goRulerCursor11.Width = 1;
            this.contextViewer1.HorizontalRulerMouseCursor = goRulerCursor11;
            goRulerCursor12.BackColor = System.Drawing.Color.White;
            goRulerCursor12.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor12.Height = 22;
            goRulerCursor12.Size = new System.Drawing.Size(1, 22);
            goRulerCursor12.Value = 0;
            goRulerCursor12.Width = 1;
            this.contextViewer1.LeftObjectCursor = goRulerCursor12;
            this.contextViewer1.Location = new System.Drawing.Point(0, 0);
            this.contextViewer1.Name = "contextViewer1";
            this.contextViewer1.NoFocusSelectionColor = System.Drawing.Color.Chartreuse;
            this.contextViewer1.PortGravity = 25F;
            this.contextViewer1.PrimarySelectionColor = System.Drawing.Color.Red;
            this.contextViewer1.ResizeHandleHeight = 24F;
            this.contextViewer1.ResizeHandlePenWidth = 4F;
            this.contextViewer1.ResizeHandleWidth = 24F;
            goRulerCursor13.BackColor = System.Drawing.Color.White;
            goRulerCursor13.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor13.Height = 22;
            goRulerCursor13.Size = new System.Drawing.Size(1, 22);
            goRulerCursor13.Value = 0;
            goRulerCursor13.Width = 1;
            this.contextViewer1.RightObjectCursor = goRulerCursor13;
            this.contextViewer1.SecondarySelectionColor = System.Drawing.Color.Blue;
            this.contextViewer1.SheetStyle = Northwoods.Go.GoViewSheetStyle.SheetWidth;
            this.contextViewer1.ShowFrame = false;
            this.contextViewer1.Size = new System.Drawing.Size(292, 273);
            this.contextViewer1.TabIndex = 0;
            this.contextViewer1.Text = "contextViewer1";
            goRulerCursor14.BackColor = System.Drawing.Color.White;
            goRulerCursor14.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor14.Height = 22;
            goRulerCursor14.Size = new System.Drawing.Size(1, 22);
            goRulerCursor14.Value = 0;
            goRulerCursor14.Width = 1;
            this.contextViewer1.TopObjectCursor = goRulerCursor14;
            goRulerCursor15.BackColor = System.Drawing.Color.White;
            goRulerCursor15.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor15.Height = 22;
            goRulerCursor15.Size = new System.Drawing.Size(1, 22);
            goRulerCursor15.Value = 0;
            goRulerCursor15.Width = 1;
            this.contextViewer1.VerticalCenterObjectCursor = goRulerCursor15;
            goRulerCursor16.BackColor = System.Drawing.Color.White;
            goRulerCursor16.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor16.Height = 22;
            goRulerCursor16.Size = new System.Drawing.Size(1, 22);
            goRulerCursor16.Value = 0;
            goRulerCursor16.Width = 1;
            this.contextViewer1.VerticalRulerMouseCursor = goRulerCursor16;
            this.contextViewer1.ObjectContextClicked += new Northwoods.Go.GoObjectEventHandler(this.contextViewer1_ObjectContextClicked);
            this.contextViewer1.ObjectSingleClicked += new Northwoods.Go.GoObjectEventHandler(this.contextViewer1_ObjectSingleClicked);
            this.contextViewer1.BackgroundContextClicked += new Northwoods.Go.GoInputEventHandler(this.contextViewer1_BackgroundContextClicked);
            this.contextViewer1.BackgroundSingleClicked += new Northwoods.Go.GoInputEventHandler(this.contextViewer1_BackgroundSingleClicked);
            // 
            // myButton
            // 
            this.myButton.Location = new System.Drawing.Point(0, 0);
            this.myButton.Name = "myButton";
            this.myButton.Size = new System.Drawing.Size(150, 25);
            this.myButton.TabIndex = 1;
            this.myButton.Text = "Click to print";
            this.myButton.Click += new System.EventHandler(this.myButton_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StripButtonSave,
            this.StripButtonOpen,
            this.StripButtonPrint,
            this.StripButtonWorkspaceLegend,
            this.StripButtonFilter,
            this.ConvertContextToDocument,});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(292, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // StripButtonSave
            // 
            this.StripButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("menuItemFileSave.Image")));
            this.StripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.StripButtonSave.Name = "StripButtonSave";
            this.StripButtonSave.Size = new System.Drawing.Size(51, 22);
            this.StripButtonSave.Text = "Save";
            this.StripButtonSave.Click += new System.EventHandler(this.StripButtonSave1_Click);
            // 
            // StripButtonOpen
            // 
            this.StripButtonOpen.Image = ((System.Drawing.Image)(resources.GetObject("menuItemFileOpen.Image")));
            this.StripButtonOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.StripButtonOpen.Name = "StripButtonOpen";
            this.StripButtonOpen.Size = new System.Drawing.Size(51, 22);
            this.StripButtonOpen.Text = "Open";
            this.StripButtonOpen.Click += new System.EventHandler(this.StripButtonOpen_Click);
            this.StripButtonOpen.Visible = false;
            // 
            // StripButtonPrint
            // 
            this.StripButtonPrint.Image = ((System.Drawing.Image)(resources.GetObject("menuItemFilePrint.Image")));
            this.StripButtonPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.StripButtonPrint.Name = "StripButtonPrint";
            this.StripButtonPrint.Size = new System.Drawing.Size(52, 22);
            this.StripButtonPrint.Text = "Print";
            this.StripButtonPrint.Click += new System.EventHandler(this.StripButtonPrint_Click);
            this.StripButtonPrint.Visible = true;
            // 
            // StripButtonWorkspaceLegend
            // 
            //this.StripButtonWorkspaceLegend.Image = ((System.Drawing.Image)(resources.GetObject("menuItemFilePrint.Image")));
            this.StripButtonWorkspaceLegend.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.StripButtonWorkspaceLegend.Name = "StripButtonWorkspaceLegend";
            this.StripButtonWorkspaceLegend.Size = new System.Drawing.Size(52, 22);
            this.StripButtonWorkspaceLegend.CheckOnClick = true;
            this.StripButtonWorkspaceLegend.Text = "Legend";
            this.StripButtonWorkspaceLegend.Checked = true;
            this.StripButtonWorkspaceLegend.Click += new System.EventHandler(this.StripButtonWorkspaceLegend_Click);
            // 
            // StripButtonFilter
            // 
            //this.StripButtonWorkspaceLegend.Image = ((System.Drawing.Image)(resources.GetObject("menuItemFilePrint.Image")));
            this.StripButtonFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.StripButtonFilter.Name = "StripButtonFilter";
            this.StripButtonFilter.Size = new System.Drawing.Size(52, 22);
            this.StripButtonFilter.CheckOnClick = true;
            this.StripButtonFilter.Text = "Filter";
            this.StripButtonFilter.Checked = true;
            this.StripButtonFilter.Click += new System.EventHandler(this.StripButtonFilter_Click);
            //
            //ConvertContextToDocument
            //
            //this.StripButtonWorkspaceLegend.Image = ((System.Drawing.Image)(resources.GetObject("menuItemFilePrint.Image")));
            this.ConvertContextToDocument.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ConvertContextToDocument.Name = "ConvertContextToDocument";
            this.ConvertContextToDocument.Text = "Convert to diagram format";
            this.ConvertContextToDocument.Click += new System.EventHandler(ConvertContextToDocument_Click);
            // 
            // LiteGraphViewContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.contextViewer1);
            //this.Controls.Add(this.myButton);
            this.Name = "LiteGraphViewContainer";
            this.TabText = "View In Context";
            this.Text = "View In Context";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
            this.MyView.DocumentChanged += new Northwoods.Go.GoChangedEventHandler(MyView_DocumentChanged);

        }
              
        #endregion

        private MetaBuilder.Graphing.Tools.ContextViewer contextViewer1;
        private MetaControls.MetaButton myButton;
        private ToolStrip toolStrip1;
        private ToolStripButton StripButtonSave;
        private ToolStripButton StripButtonOpen;
        private ToolStripButton StripButtonPrint;
        private ToolStripButton StripButtonWorkspaceLegend;
        private ToolStripButton StripButtonFilter;
        private ToolStripButton ConvertContextToDocument;
    }
}