/*
 *  Copyright © Northwoods Software Corporation, 1998-2006. All Rights
 *  Reserved.
 *
 *  Restricted Rights: Use, duplication, or disclosure by the U.S.
 *  Government is subject to restrictions as set forth in subparagraph
 *  (c) (1) (ii) of DFARS 252.227-7013, or in FAR 52.227-19, or in FAR
 *  52.227-14 Alt. III, as applicable.
 */

using System;
using System.ComponentModel;
using System.Windows.Forms;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Tools;
using Northwoods.Go;
using Northwoods.Go.Layout;

namespace MetaBuilder.UIControls.GraphingUI
{
    /// <summary>
    /// Summary description for LayerDialog.
    /// </summary>
    public class LayeredLayoutDialog : Form
    {

        #region Fields (29)

        private CheckBox aggressive;
        private MetaControls.MetaButton applyButton;
        private MetaControls.MetaButton closeButton;
        private TextBox column;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components = null;
        private RadioButton depthFirst;
        private RadioButton dfsin;
        private RadioButton dfsout;
        private RadioButton down;
        private RadioButton greedy;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private GroupBox groupBox5;
        private GroupBox groupBox6;
        private GroupBox groupBox7;
        private TextBox iter;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox layer;
        private RadioButton left;
        private RadioButton length;
        private RadioButton naive;
        private RadioButton right;
        private RadioButton sink;
        private RadioButton source;
        private RadioButton up;

        #endregion Fields

        #region Constructors (1)

        public LayeredLayoutDialog()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods (4)

        // Public Methods (2) 

        public void applyButton_Click(object sender, EventArgs e)
        {
            LayerLayout();
        }

        public void LayerLayout()
        {
            GraphView view = DockingForm.DockForm.GetCurrentGraphView();
            if (view == null) return;

            GoLayoutLayeredDigraph layout = new GoLayoutLayeredDigraph();

            //layout.Progress += new GoLayoutProgressEventHandler(app.LayeredDigraphProgress);

            GoLayoutLayeredDigraphCycleRemove cycle;
            GoLayoutLayeredDigraphLayering layering;
            GoLayoutLayeredDigraphInitIndices initialize;
            GoLayoutDirection direction;
            GoLayoutLayeredDigraphAggressive agr;
            if (greedy.Checked)
                cycle = GoLayoutLayeredDigraphCycleRemove.Greedy;
            else
                cycle = GoLayoutLayeredDigraphCycleRemove.DepthFirst;
            if (sink.Checked)
                layering = GoLayoutLayeredDigraphLayering.LongestPathSink;
            else if (source.Checked)
                layering = GoLayoutLayeredDigraphLayering.LongestPathSource;
            else
                layering = GoLayoutLayeredDigraphLayering.OptimalLinkLength;
            if (naive.Checked)
                initialize = GoLayoutLayeredDigraphInitIndices.Naive;
            else if (dfsout.Checked)
                initialize = GoLayoutLayeredDigraphInitIndices.DepthFirstOut;
            else
                initialize = GoLayoutLayeredDigraphInitIndices.DepthFirstIn;
            if (up.Checked)
                direction = GoLayoutDirection.Up;
            else if (down.Checked)
                direction = GoLayoutDirection.Down;
            else if (left.Checked)
                direction = GoLayoutDirection.Left;
            else
                direction = GoLayoutDirection.Right;
            if (aggressive.Checked)
                agr = GoLayoutLayeredDigraphAggressive.More;
            else
                agr = GoLayoutLayeredDigraphAggressive.Less;

            layout.LayerSpacing = int.Parse(layer.Text);
            layout.ColumnSpacing = int.Parse(column.Text);
            layout.DirectionOption = direction;
            layout.CycleRemoveOption = cycle;
            layout.LayeringOption = layering;
            layout.InitializeOption = initialize;
            layout.Iterations = int.Parse(iter.Text);
            layout.AggressiveOption = agr;

            layout.Document = view.Document;
            layout.Document.StartTransaction();
            // This allows the user to move nodes around while keeping the general shape of the links.
            // This code is only needed in this demo application, where you can alternate
            // between ForceDirected and LayeredDigraph
            //foreach (GoObject obj in view.Document)
            //{
            //    GoLink link = obj as GoLink;
            //    if (link != null)
            //    {
            //        link.AdjustingStyle = GoLinkAdjustingStyle.Calculate;
            //    }
            //}
            layout.PerformLayout();

            // Just needed for this demo application, after having set the AdjustingStyle above
            //foreach (GoObject obj in view.Document)
            //{
            //    GoLink link = obj as GoLink;
            //    if (link != null)
            //    {
            //        link.AdjustingStyle = GoLinkAdjustingStyle.Scale;
            //    }
            //}
            //AutoRelinkTool artool = new AutoRelinkTool();
            //GoCollection collection = new GoCollection();
            //collection.AddRange(view.Document);
            //artool.RelinkCollection(collection);
            view.UpdateView();

            //foreach (GoObject obj in view.Document)
            //{
            //    MetaBuilder.Graphing.Shapes.QLink link = obj as MetaBuilder.Graphing.Shapes.QLink;
            //    if (link != null)
            //    {
            //        (link.RealLink as MetaBuilder.Graphing.Shapes.QRealLink).ForceCalculate();
            //    }
            //}
            DockingForm.DockForm.GetCurrentGraphViewContainer().cropGlobal();
            layout.Document.FinishTransaction("Layered Digraph Layout");
        }

        // Protected Methods (1) 

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        // Private Methods (1) 

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion Methods

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.left = new System.Windows.Forms.RadioButton();
            this.down = new System.Windows.Forms.RadioButton();
            this.aggressive = new System.Windows.Forms.CheckBox();
            this.closeButton = new MetaControls.MetaButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.up = new System.Windows.Forms.RadioButton();
            this.dfsout = new System.Windows.Forms.RadioButton();
            this.right = new System.Windows.Forms.RadioButton();
            this.dfsin = new System.Windows.Forms.RadioButton();
            this.naive = new System.Windows.Forms.RadioButton();
            this.greedy = new System.Windows.Forms.RadioButton();
            this.layer = new System.Windows.Forms.TextBox();
            this.sink = new System.Windows.Forms.RadioButton();
            this.iter = new System.Windows.Forms.TextBox();
            this.source = new System.Windows.Forms.RadioButton();
            this.column = new System.Windows.Forms.TextBox();
            this.applyButton = new MetaControls.MetaButton();
            this.depthFirst = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.length = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // left
            // 
            this.left.Location = new System.Drawing.Point(104, 24);
            this.left.Name = "left";
            this.left.Size = new System.Drawing.Size(64, 24);
            this.left.TabIndex = 2;
            this.left.Text = "&Left";
            // 
            // down
            // 
            this.down.Location = new System.Drawing.Point(24, 48);
            this.down.Name = "down";
            this.down.Size = new System.Drawing.Size(56, 24);
            this.down.TabIndex = 1;
            this.down.Text = "&Down";
            // 
            // aggressive
            // 
            this.aggressive.Location = new System.Drawing.Point(16, 48);
            this.aggressive.Name = "aggressive";
            this.aggressive.Size = new System.Drawing.Size(88, 24);
            this.aggressive.TabIndex = 2;
            this.aggressive.Text = "Aggressive";
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(140, 216);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(72, 23);
            this.closeButton.TabIndex = 6;
            this.closeButton.Text = "Close";
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // label1
            // 
            this.label1.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.label1.Location = new System.Drawing.Point(16, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Iterations";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "Layer Spacing";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 23);
            this.label3.TabIndex = 1;
            this.label3.Text = "Column Spacing";
            // 
            // up
            // 
            this.up.Checked = true;
            this.up.Location = new System.Drawing.Point(24, 24);
            this.up.Name = "up";
            this.up.Size = new System.Drawing.Size(56, 24);
            this.up.TabIndex = 0;
            this.up.TabStop = true;
            this.up.Text = "&Up";
            // 
            // dfsout
            // 
            this.dfsout.Checked = true;
            this.dfsout.Location = new System.Drawing.Point(16, 40);
            this.dfsout.Name = "dfsout";
            this.dfsout.Size = new System.Drawing.Size(168, 24);
            this.dfsout.TabIndex = 1;
            this.dfsout.TabStop = true;
            this.dfsout.Text = "Depth First Search Outward";
            // 
            // right
            // 
            this.right.Location = new System.Drawing.Point(104, 48);
            this.right.Name = "right";
            this.right.Size = new System.Drawing.Size(64, 24);
            this.right.TabIndex = 3;
            this.right.Text = "&Right";
            // 
            // dfsin
            // 
            this.dfsin.Location = new System.Drawing.Point(16, 64);
            this.dfsin.Name = "dfsin";
            this.dfsin.Size = new System.Drawing.Size(168, 24);
            this.dfsin.TabIndex = 2;
            this.dfsin.Text = "Depth First Search Inward";
            // 
            // naive
            // 
            this.naive.Location = new System.Drawing.Point(16, 16);
            this.naive.Name = "naive";
            this.naive.Size = new System.Drawing.Size(168, 24);
            this.naive.TabIndex = 0;
            this.naive.Text = "Naive";
            // 
            // greedy
            // 
            this.greedy.Location = new System.Drawing.Point(16, 24);
            this.greedy.Name = "greedy";
            this.greedy.Size = new System.Drawing.Size(112, 24);
            this.greedy.TabIndex = 0;
            this.greedy.Text = "Greedy";
            // 
            // layer
            // 
            this.layer.Location = new System.Drawing.Point(104, 24);
            this.layer.Name = "layer";
            this.layer.Size = new System.Drawing.Size(72, 20);
            this.layer.TabIndex = 2;
            this.layer.Text = "20";
            this.layer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // sink
            // 
            this.sink.Location = new System.Drawing.Point(16, 16);
            this.sink.Name = "sink";
            this.sink.Size = new System.Drawing.Size(128, 24);
            this.sink.TabIndex = 0;
            this.sink.Text = "Longest Path Sink";
            // 
            // iter
            // 
            this.iter.Location = new System.Drawing.Point(112, 24);
            this.iter.Name = "iter";
            this.iter.Size = new System.Drawing.Size(72, 20);
            this.iter.TabIndex = 0;
            this.iter.Text = "4";
            this.iter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // source
            // 
            this.source.Location = new System.Drawing.Point(16, 40);
            this.source.Name = "source";
            this.source.Size = new System.Drawing.Size(128, 24);
            this.source.TabIndex = 1;
            this.source.Text = "Longest Path Source";
            // 
            // column
            // 
            this.column.Location = new System.Drawing.Point(104, 48);
            this.column.Name = "column";
            this.column.Size = new System.Drawing.Size(72, 20);
            this.column.TabIndex = 2;
            this.column.Text = "20";
            this.column.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point(12, 216);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(72, 23);
            this.applyButton.TabIndex = 5;
            this.applyButton.Text = "Apply";
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // depthFirst
            // 
            this.depthFirst.Checked = true;
            this.depthFirst.Location = new System.Drawing.Point(16, 48);
            this.depthFirst.Name = "depthFirst";
            this.depthFirst.Size = new System.Drawing.Size(120, 24);
            this.depthFirst.TabIndex = 1;
            this.depthFirst.TabStop = true;
            this.depthFirst.Text = "Depth First Search";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.depthFirst);
            this.groupBox1.Controls.Add(this.greedy);
            this.groupBox1.Location = new System.Drawing.Point(238, 103);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 80);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cycle Remove Options";
            this.groupBox1.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.aggressive);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.iter);
            this.groupBox2.Location = new System.Drawing.Point(238, 15);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 80);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Crossing Reduction Options";
            this.groupBox2.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.length);
            this.groupBox3.Controls.Add(this.sink);
            this.groupBox3.Controls.Add(this.source);
            this.groupBox3.Location = new System.Drawing.Point(238, 191);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 96);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Layering Options";
            this.groupBox3.Visible = false;
            // 
            // length
            // 
            this.length.Checked = true;
            this.length.Location = new System.Drawing.Point(16, 64);
            this.length.Name = "length";
            this.length.Size = new System.Drawing.Size(128, 24);
            this.length.TabIndex = 2;
            this.length.TabStop = true;
            this.length.Text = "Optimal Link Length";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.dfsout);
            this.groupBox4.Controls.Add(this.dfsin);
            this.groupBox4.Controls.Add(this.naive);
            this.groupBox4.Location = new System.Drawing.Point(238, 295);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 96);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Initialize Options";
            this.groupBox4.Visible = false;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.groupBox7);
            this.groupBox5.Controls.Add(this.groupBox6);
            this.groupBox5.Location = new System.Drawing.Point(12, 8);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(200, 200);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Layout Options";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.right);
            this.groupBox7.Controls.Add(this.left);
            this.groupBox7.Controls.Add(this.down);
            this.groupBox7.Controls.Add(this.up);
            this.groupBox7.Location = new System.Drawing.Point(8, 112);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(184, 80);
            this.groupBox7.TabIndex = 1;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Direction";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.column);
            this.groupBox6.Controls.Add(this.layer);
            this.groupBox6.Controls.Add(this.label3);
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Location = new System.Drawing.Point(8, 16);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(184, 80);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Spacing";
            // 
            // LayeredLayoutDialog
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(226, 250);
            this.ControlBox = false;
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LayeredLayoutDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Layered-Digraph Layout Settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion
    }
}