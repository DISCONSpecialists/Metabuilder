namespace ShapeBuilding
{
    partial class Form1
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
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor2 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor3 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor4 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor5 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor6 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor7 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor8 = new Northwoods.Go.Draw.GoRulerCursor();
            this.graphView1 = new MetaBuilder.Graphing.Containers.GraphView();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // graphView1
            // 
            this.graphView1.ArrowMoveLarge = 10F;
            this.graphView1.ArrowMoveSmall = 1F;
            this.graphView1.BackColor = System.Drawing.Color.White;
            this.graphView1.BackgroundHasSheet = true;
            goRulerCursor1.BackColor = System.Drawing.Color.White;
            goRulerCursor1.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor1.Height = 22;
            goRulerCursor1.Size = new System.Drawing.Size(1, 22);
            goRulerCursor1.Value = 0;
            goRulerCursor1.Width = 1;
            this.graphView1.BottomObjectCursor = goRulerCursor1;
            this.graphView1.BoundingHandlePenWidth = 2.666667F;
            this.graphView1.ContextClickSingleSelection = false;
            this.graphView1.DisableKeys = Northwoods.Go.GoViewDisableKeys.ArrowScroll;
            this.graphView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphView1.DocScale = 0.75F;
            this.graphView1.DragsRealtime = true;
            this.graphView1.ExternalDragDropsOnEnter = true;
            this.graphView1.GridCellSizeHeight = 25F;
            this.graphView1.GridCellSizeWidth = 25F;
            this.graphView1.GridLineDashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            this.graphView1.GridMajorLineColor = System.Drawing.Color.LightGray;
            this.graphView1.GridMajorLineFrequency = new System.Drawing.Size(4, 4);
            this.graphView1.GridStyle = Northwoods.Go.GoViewGridStyle.Line;
            this.graphView1.GridUnboundedSpots = 0;
            goRulerCursor2.BackColor = System.Drawing.Color.White;
            goRulerCursor2.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor2.Height = 22;
            goRulerCursor2.Size = new System.Drawing.Size(1, 22);
            goRulerCursor2.Value = 0;
            goRulerCursor2.Width = 1;
            this.graphView1.HorizontalCenterObjectCursor = goRulerCursor2;
            goRulerCursor3.BackColor = System.Drawing.Color.White;
            goRulerCursor3.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor3.Height = 22;
            goRulerCursor3.Size = new System.Drawing.Size(1, 22);
            goRulerCursor3.Value = 0;
            goRulerCursor3.Width = 1;
            this.graphView1.HorizontalRulerMouseCursor = goRulerCursor3;
            goRulerCursor4.BackColor = System.Drawing.Color.White;
            goRulerCursor4.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor4.Height = 22;
            goRulerCursor4.Size = new System.Drawing.Size(1, 22);
            goRulerCursor4.Value = 0;
            goRulerCursor4.Width = 1;
            this.graphView1.LeftObjectCursor = goRulerCursor4;
            this.graphView1.Location = new System.Drawing.Point(0, 0);
            this.graphView1.Name = "graphView1";
            this.graphView1.NoFocusSelectionColor = System.Drawing.Color.Chartreuse;
            this.graphView1.PortGravity = 25F;
            this.graphView1.PrimarySelectionColor = System.Drawing.Color.Red;
            this.graphView1.ResizeHandleHeight = 8F;
            this.graphView1.ResizeHandlePenWidth = 1.333333F;
            this.graphView1.ResizeHandleWidth = 8F;
            goRulerCursor5.BackColor = System.Drawing.Color.White;
            goRulerCursor5.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor5.Height = 22;
            goRulerCursor5.Size = new System.Drawing.Size(1, 22);
            goRulerCursor5.Value = 0;
            goRulerCursor5.Width = 1;
            this.graphView1.RightObjectCursor = goRulerCursor5;
            this.graphView1.SecondarySelectionColor = System.Drawing.Color.Blue;
            this.graphView1.SheetStyle = Northwoods.Go.GoViewSheetStyle.SheetWidth;
            this.graphView1.ShowFrame = false;
            this.graphView1.Size = new System.Drawing.Size(811, 497);
            this.graphView1.TabIndex = 0;
            this.graphView1.Text = "graphView1";
            goRulerCursor6.BackColor = System.Drawing.Color.White;
            goRulerCursor6.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor6.Height = 22;
            goRulerCursor6.Size = new System.Drawing.Size(1, 22);
            goRulerCursor6.Value = 0;
            goRulerCursor6.Width = 1;
            this.graphView1.TopObjectCursor = goRulerCursor6;
            goRulerCursor7.BackColor = System.Drawing.Color.White;
            goRulerCursor7.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor7.Height = 22;
            goRulerCursor7.Size = new System.Drawing.Size(1, 22);
            goRulerCursor7.Value = 0;
            goRulerCursor7.Width = 1;
            this.graphView1.VerticalCenterObjectCursor = goRulerCursor7;
            goRulerCursor8.BackColor = System.Drawing.Color.White;
            goRulerCursor8.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor8.Height = 22;
            goRulerCursor8.Size = new System.Drawing.Size(1, 22);
            goRulerCursor8.Value = 0;
            goRulerCursor8.Width = 1;
            this.graphView1.VerticalRulerMouseCursor = goRulerCursor8;
            this.graphView1.ObjectGotSelection += new Northwoods.Go.GoSelectionEventHandler(this.graphView1_ObjectGotSelection_1);
            this.graphView1.ObjectLostSelection += new Northwoods.Go.GoSelectionEventHandler(this.graphView1_ObjectLostSelection);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.Location = new System.Drawing.Point(0, 43);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(178, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Drop Nodes";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Top;
            this.button2.Location = new System.Drawing.Point(0, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(178, 43);
            this.button2.TabIndex = 2;
            this.button2.Text = "Make Collapsible";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.statusStrip1);
            this.splitContainer1.Panel1.Controls.Add(this.propertyGrid1);
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            this.splitContainer1.Panel1.Controls.Add(this.button1);
            this.splitContainer1.Panel1.Controls.Add(this.button2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.graphView1);
            this.splitContainer1.Size = new System.Drawing.Size(993, 497);
            this.splitContainer1.SplitterDistance = 178;
            this.splitContainer1.TabIndex = 3;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.HideSelection = false;
            this.treeView1.Location = new System.Drawing.Point(0, 66);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(178, 431);
            this.treeView1.TabIndex = 3;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 66);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(178, 431);
            this.propertyGrid1.TabIndex = 4;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 475);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(178, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(993, 497);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        



        #endregion

        private MetaBuilder.Graphing.Containers.GraphView graphView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;

    }
}

