namespace MetaBuilder.UIControls.SWOT
{
    partial class SWOT
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
            this.zgc = new ZedGraph.ZedGraphControl();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbImplications = new System.Windows.Forms.CheckedListBox();
            this.btnPrint = new MetaControls.MetaButton();
            this.printPanel = new System.Windows.Forms.Panel();
            this.btnLoadItems = new MetaControls.MetaButton();
            this.btnUnmark = new MetaControls.MetaButton();
            this.btnMark = new MetaControls.MetaButton();
            this.groupBox1.SuspendLayout();
            this.printPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // zgc
            // 
            this.zgc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.zgc.EditButtons = System.Windows.Forms.MouseButtons.Left;
            this.zgc.EditModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)));
            this.zgc.IsAutoScrollRange = false;
            this.zgc.IsEnableHEdit = false;
            this.zgc.IsEnableHPan = true;
            this.zgc.IsEnableHZoom = true;
            this.zgc.IsEnableVEdit = false;
            this.zgc.IsEnableVPan = true;
            this.zgc.IsEnableVZoom = true;
            this.zgc.IsPrintFillPage = true;
            this.zgc.IsPrintKeepAspectRatio = true;
            this.zgc.IsScrollY2 = false;
            this.zgc.IsShowContextMenu = true;
            this.zgc.IsShowCopyMessage = true;
            this.zgc.IsShowCursorValues = false;
            this.zgc.IsShowHScrollBar = false;
            this.zgc.IsShowPointValues = false;
            this.zgc.IsShowVScrollBar = false;
            this.zgc.IsSynchronizeXAxes = false;
            this.zgc.IsSynchronizeYAxes = false;
            this.zgc.IsZoomOnMouseCenter = false;
            this.zgc.LinkButtons = System.Windows.Forms.MouseButtons.Left;
            this.zgc.LinkModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)));
            this.zgc.Location = new System.Drawing.Point(0, 4);
            this.zgc.Name = "zgc";
            this.zgc.PanButtons = System.Windows.Forms.MouseButtons.Left;
            this.zgc.PanButtons2 = System.Windows.Forms.MouseButtons.Middle;
            this.zgc.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.zgc.PanModifierKeys2 = System.Windows.Forms.Keys.None;
            this.zgc.PointDateFormat = "g";
            this.zgc.PointValueFormat = "G";
            this.zgc.ScrollMaxX = 14;
            this.zgc.ScrollMaxY = 14;
            this.zgc.ScrollMaxY2 = 14;
            this.zgc.ScrollMinX = -14;
            this.zgc.ScrollMinY = -14;
            this.zgc.ScrollMinY2 = -14;
            this.zgc.Size = new System.Drawing.Size(404, 388);
            this.zgc.TabIndex = 0;
            this.zgc.ZoomButtons = System.Windows.Forms.MouseButtons.None;
            this.zgc.ZoomButtons2 = System.Windows.Forms.MouseButtons.None;
            this.zgc.ZoomModifierKeys = System.Windows.Forms.Keys.None;
            this.zgc.ZoomModifierKeys2 = System.Windows.Forms.Keys.None;
            this.zgc.ZoomStepFraction = 0.1;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(203, 52);
            this.label1.TabIndex = 2;
            this.label1.Text = "(-5,3) 1,5 \r\nImplications 1 and 5 has SWOT Rating of\r\n-5 (Strength/Weakness) and\r" +
                "\n3 (Opportunity/Threat) respectively";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(408, 291);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(224, 76);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Notation Example";
            // 
            // lbImplications
            // 
            this.lbImplications.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbImplications.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbImplications.CheckOnClick = true;
            this.lbImplications.FormattingEnabled = true;
            this.lbImplications.Location = new System.Drawing.Point(408, 32);
            this.lbImplications.Name = "lbImplications";
            this.lbImplications.Size = new System.Drawing.Size(224, 212);
            this.lbImplications.TabIndex = 5;
            this.lbImplications.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lbImplications_ItemCheck);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Location = new System.Drawing.Point(408, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(58, 23);
            this.btnPrint.TabIndex = 7;
            this.btnPrint.Text = "Print";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // printPanel
            // 
            this.printPanel.BackColor = System.Drawing.SystemColors.Control;
            this.printPanel.Controls.Add(this.groupBox1);
            this.printPanel.Controls.Add(this.zgc);
            this.printPanel.Controls.Add(this.btnLoadItems);
            this.printPanel.Controls.Add(this.btnUnmark);
            this.printPanel.Controls.Add(this.btnMark);
            this.printPanel.Controls.Add(this.btnPrint);
            this.printPanel.Controls.Add(this.lbImplications);
            this.printPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printPanel.Location = new System.Drawing.Point(0, 0);
            this.printPanel.Name = "printPanel";
            this.printPanel.Size = new System.Drawing.Size(636, 403);
            this.printPanel.TabIndex = 8;
            // 
            // btnLoadItems
            // 
            this.btnLoadItems.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadItems.Location = new System.Drawing.Point(472, 4);
            this.btnLoadItems.Name = "btnLoadItems";
            this.btnLoadItems.Size = new System.Drawing.Size(159, 23);
            this.btnLoadItems.TabIndex = 7;
            this.btnLoadItems.Text = "Load Items from Database";
            this.btnLoadItems.Click += new System.EventHandler(this.btnLoadItems_Click);
            // 
            // btnUnmark
            // 
            this.btnUnmark.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUnmark.Location = new System.Drawing.Point(524, 244);
            this.btnUnmark.Name = "btnUnmark";
            this.btnUnmark.Size = new System.Drawing.Size(104, 23);
            this.btnUnmark.TabIndex = 7;
            this.btnUnmark.Text = "Unmark All";
            this.btnUnmark.Click += new System.EventHandler(this.btnUnmark_Click);
            // 
            // btnMark
            // 
            this.btnMark.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMark.Location = new System.Drawing.Point(412, 244);
            this.btnMark.Name = "btnMark";
            this.btnMark.Size = new System.Drawing.Size(112, 23);
            this.btnMark.TabIndex = 7;
            this.btnMark.Text = "Mark All";
            this.btnMark.Click += new System.EventHandler(this.btnMark_Click);
            // 
            // SWOT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(636, 403);
            this.Controls.Add(this.printPanel);
            this.Name = "SWOT";
            this.Text = "SWOT Analysis Tool";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.printPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ZedGraph.ZedGraphControl zgc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private MetaControls.MetaButton btnPrint;
        private System.Windows.Forms.Panel printPanel;
        private MetaControls.MetaButton btnLoadItems;
        private MetaControls.MetaButton btnMark;
        private MetaControls.MetaButton btnUnmark;
        private System.Windows.Forms.CheckedListBox lbImplications;
    }
}