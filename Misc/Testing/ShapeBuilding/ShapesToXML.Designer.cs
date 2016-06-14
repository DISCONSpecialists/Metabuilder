namespace ShapeBuilding
{
    partial class ShapesToXML
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShapesToXML));
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor2 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor3 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor4 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor5 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor6 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor7 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor8 = new Northwoods.Go.Draw.GoRulerCursor();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtXML = new System.Windows.Forms.TextBox();
            this.btnDeserialize = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.btnSerialize = new System.Windows.Forms.Button();
            this.btnAddShape = new System.Windows.Forms.Button();
            this.graphView1 = new MetaBuilder.Graphing.Containers.GraphView();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtXML);
            this.splitContainer1.Panel1.Controls.Add(this.btnDeserialize);
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            this.splitContainer1.Panel1.Controls.Add(this.btnSerialize);
            this.splitContainer1.Panel1.Controls.Add(this.btnAddShape);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.graphView1);
            this.splitContainer1.Size = new System.Drawing.Size(821, 429);
            this.splitContainer1.SplitterDistance = 146;
            this.splitContainer1.TabIndex = 4;
            // 
            // txtXML
            // 
            this.txtXML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtXML.Location = new System.Drawing.Point(0, 89);
            this.txtXML.Multiline = true;
            this.txtXML.Name = "txtXML";
            this.txtXML.Size = new System.Drawing.Size(146, 340);
            this.txtXML.TabIndex = 5;
            // 
            // btnDeserialize
            // 
            this.btnDeserialize.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDeserialize.Location = new System.Drawing.Point(0, 66);
            this.btnDeserialize.Name = "btnDeserialize";
            this.btnDeserialize.Size = new System.Drawing.Size(146, 23);
            this.btnDeserialize.TabIndex = 4;
            this.btnDeserialize.Text = "Deserialize from XML";
            this.btnDeserialize.UseVisualStyleBackColor = true;
            this.btnDeserialize.Click += new System.EventHandler(this.btnDeserialize_Click);
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.HideSelection = false;
            this.treeView1.Location = new System.Drawing.Point(0, 66);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(146, 363);
            this.treeView1.TabIndex = 3;
            // 
            // btnSerialize
            // 
            this.btnSerialize.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSerialize.Location = new System.Drawing.Point(0, 43);
            this.btnSerialize.Name = "btnSerialize";
            this.btnSerialize.Size = new System.Drawing.Size(146, 23);
            this.btnSerialize.TabIndex = 1;
            this.btnSerialize.Text = "Serialize to XML";
            this.btnSerialize.UseVisualStyleBackColor = true;
            this.btnSerialize.Click += new System.EventHandler(this.btnSerialize_Click);
            // 
            // btnAddShape
            // 
            this.btnAddShape.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAddShape.Location = new System.Drawing.Point(0, 0);
            this.btnAddShape.Name = "btnAddShape";
            this.btnAddShape.Size = new System.Drawing.Size(146, 43);
            this.btnAddShape.TabIndex = 2;
            this.btnAddShape.Text = "Add Shape";
            this.btnAddShape.UseVisualStyleBackColor = true;
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
           // this.graphView1.ContainsMappingCells = false;
            this.graphView1.ContextClickSingleSelection = false;
            this.graphView1.DisableKeys = Northwoods.Go.GoViewDisableKeys.ArrowScroll;
            this.graphView1.DocExtentCenter = ((System.Drawing.PointF)(resources.GetObject("graphView1.DocExtentCenter")));
            this.graphView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphView1.DragsRealtime = true;
            this.graphView1.ExternalDragDropsOnEnter = true;
            this.graphView1.GridCellSizeHeight = 25F;
            this.graphView1.GridCellSizeWidth = 25F;
            this.graphView1.GridLineDashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            this.graphView1.GridMajorLineColor = System.Drawing.Color.LightGray;
            this.graphView1.GridMajorLineFrequency = new System.Drawing.Size(4, 4);
            this.graphView1.GridSnapResize = Northwoods.Go.GoViewSnapStyle.Jump;
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
            this.graphView1.Size = new System.Drawing.Size(671, 429);
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
            // 
            // ShapesToXML
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 429);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ShapesToXML";
            this.Text = "ShapesToXML";
            this.Load += new System.EventHandler(this.ShapesToXML_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button btnSerialize;
        private System.Windows.Forms.Button btnAddShape;
        private MetaBuilder.Graphing.Containers.GraphView graphView1;
        private System.Windows.Forms.TextBox txtXML;
        private System.Windows.Forms.Button btnDeserialize;
    }
}