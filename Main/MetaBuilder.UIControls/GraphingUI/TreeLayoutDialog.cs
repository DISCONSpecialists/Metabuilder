using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Tools;
using Northwoods.Go;
using Northwoods.Go.Layout;

namespace MetaBuilder.UIControls.GraphingUI
{
    public class TreeNodeComparer : IComparer<GoLayoutTreeNode>
    {
        public int Compare(GoLayoutTreeNode x, GoLayoutTreeNode y)
        {
            if (x.GoObject is GraphNode)
            {
                GraphNode nX = x.GoObject as GraphNode;
                GraphNode nY = y.GoObject as GraphNode;
                if (nX.MetaObject != null && nY.MetaObject != null)
                {
                    return nX.MetaObject.pkid - nY.MetaObject.pkid;
                }
            }
            return 0;
        }
    }
    public class DisconLayoutTree : GoLayoutTree
    {
        protected override void SortTreeNodeChildren(GoLayoutTreeNode n)
        {
            List<GoLayoutTreeNode> nodes = new List<GoLayoutTreeNode>();
            for (int i = 0; i < n.ChildrenCount; i++)
            {
                nodes.Add(n.Children[i]);
            }
            TreeNodeComparer comp = new TreeNodeComparer();
            nodes.Sort(comp);
            n.Children = nodes.ToArray();
        }
    }

    public class RootOnlyTreeLayout : GoLayoutTree
    {

        public RootOnlyTreeLayout()
        {
            // set RootDefaults:
            this.Angle = 90;
            // set AlternateDefaults:
            this.AlternateDefaults.Angle = 0;
            this.AlternateDefaults.PortSpot = GoObject.BottomLeft;
            this.AlternateDefaults.ChildPortSpot = GoObject.MiddleLeft;
            this.AlternateDefaults.Alignment = GoLayoutTreeAlignment.Start;
        }

        protected override void AssignTreeNodeValues(GoLayoutTreeNode n)
        {
            base.AssignTreeNodeValues(n);
            if (n.Parent == null) return;
            n.NodeIndent = n.Height + 10;
            n.LayerSpacing = -n.Width + 120;
        }

        // In the future, you can get the effect of this override by
        // specifying GoLayoutTree.Style = GoLayoutTreeStyle.RootOnly
        protected override void InitializeTreeNodeValues(GoLayoutTreeNode n)
        {
            GoLayoutTreeNode mom;
            if (n.Parent == null) mom = this.RootDefaults;
            else if (n.Parent.Parent == null) mom = this.AlternateDefaults;
            else mom = n.Parent;
            n.CopyInheritedPropertiesFrom(mom);
            n.Initialized = true;  // should already be true, but make sure
        }
    }

    /// <summary>
    /// Summary description for TreeDialog.
    /// </summary>
    public class TreeLayoutDialog : Form
    {
        #region Fields (44)

        private MetaControls.MetaButton applyButton;
        private CheckBox cbAutoRelink;
        private MetaControls.MetaButton closeButton;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components = null;
        private ComboBox ddlAlternateDirection;
        private ComboBox ddlArrangement;
        private ComboBox ddlChildrenAlignment;
        private ComboBox ddlCompaction;
        private ComboBox ddlConstructionPath;
        private ComboBox ddlSorting;
        private ComboBox ddlTreeDirection;
        private ComboBox ddlTreeStyle;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private GroupBox grpAlignment;
        private GroupBox grpMaxTreeSize;
        private GroupBox grpSpacing;
        private GroupBox grpTreeDirection;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label2;
        private Label label4;
        private Label label5;
        private Label label8;
        private Label label9;
        private Label lblAlign;
        private Label lblAlternateAngle;
        private Label lblBreadthOffset;
        private Label lblDir;
        private Label lblLayerSpacing;
        private Label lblMaxWidth;
        private Label lblNodeSpacing;
        private Label lblWarning;
        private NumericUpDown nudAlternateBreadthLimit;
        private NumericUpDown nudArrangementSpacingH;
        private NumericUpDown nudArrangementSpacingW;
        private NumericUpDown nudLayerSpacing;
        private NumericUpDown nudMaxBreadth;
        private NumericUpDown nudNodeSpacing;
        private NumericUpDown nudOffset;
        private NumericUpDown nudRowSpacing;

        #endregion Fields

        public TreeLayoutDialog()
        {
            InitializeComponent();
            ddlTreeDirection.SelectedIndex = 0;
            ddlAlternateDirection.SelectedIndex = 0;
            ddlCompaction.SelectedIndex = 1;
            ddlChildrenAlignment.SelectedIndex = 1;
            ddlSorting.SelectedIndex = 0;
            ddlConstructionPath.SelectedIndex = 0;
            ddlArrangement.SelectedIndex = 0;
            ddlTreeStyle.SelectedIndex = 0;
        }

        #region Methods (12)

        // Public Methods (4) 

        public void applyButton_Click(object sender, EventArgs e)
        {
            DoLayout(DockingForm.DockForm.GetCurrentGraphView(), cbAutoRelink.Checked);
        }

        public void applyToSelectionButton_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(this,"Not implemented, but working on it");
            /* GoDocument doc = DockingForm.DockForm.GetCurrentGraphView().Document;
            GoDocument newDoc = new GoDocument();
            Hashtable goobjects = new Hashtable();
            foreach (GoObject obj in doc)
            {
                if (!(obj is GoLink))
                    goobjects.Add(obj, obj.Copy());
            }

            foreach (GoObject obj in doc)
            {
                if (obj is GoLink)
                    goobjects.Add(obj, obj.Copy());
            }

            foreach (DictionaryEntry entry in goobjects)
            {
                GoObject o = entry.Value as GoObject;
                if (!(o is GoLink))
                    newDoc.Add(o);
            }
            foreach (DictionaryEntry entry in goobjects)
            {
                GoObject o = entry.Value as GoObject;
                if (o is GoLink)
                {
                    GoLink lnk = o as GoLink;
                    newDoc.Add(lnk);
                    lnk.FromPort 
                }
            }
            TreeLayout(doc);*/
        }

        public void DoLayout(GraphView view, bool AutoRelink)
        {
            TreeLayout(view);
            if (AutoRelink)
            {
                //AutoRelinkTool autorelinktool = new AutoRelinkTool();
                GoCollection collection = new GoCollection();
                collection.AddRange(view.Document);
                //autorelinktool.RelinkCollection(collection);
            }
        }

        public bool Auto = false;
        public void TreeLayout(GraphView view)
        {
            GoDocument UseThisDocument = view.Document;
            if (view == null) return;

            //DisconLayoutTree layout = new DisconLayoutTree();
            RootOnlyTreeLayout layout = new RootOnlyTreeLayout();

            ////Was ignored
            ////layout.Document = UseThisDocument;
            ////layout.PerformLayout();
            ////return;

            // specify GoLayoutTree properties
            switch (ddlConstructionPath.SelectedIndex)
            {
                default:
                case 0:
                    layout.Path = GoLayoutTreePath.Destination;
                    break;
                case 1:
                    layout.Path = GoLayoutTreePath.Source;
                    break;
            }
            switch (ddlArrangement.SelectedIndex)
            {
                default:
                case 0:
                    layout.Arrangement = GoLayoutTreeArrangement.Vertical;
                    break;
                case 1:
                    layout.Arrangement = GoLayoutTreeArrangement.Horizontal;
                    break;
                case 2:
                    layout.Arrangement = GoLayoutTreeArrangement.FixedRoots;
                    break;
            }
            layout.ArrangementOrigin = new PointF(50, 50);
            layout.ArrangementSpacing = new SizeF((float)nudArrangementSpacingW.Value, (float)nudArrangementSpacingH.Value);
            // specify GoLayoutTree.RootDefaults properties
            switch (ddlTreeDirection.SelectedIndex)
            {
                default:
                case 0:
                    layout.Angle = 0;
                    break;
                case 1:
                    layout.Angle = 90;
                    break;
                case 2:
                    layout.Angle = 180;
                    break;
                case 3:
                    layout.Angle = 270;
                    break;
            }
            layout.BreadthLimit = (float)nudMaxBreadth.Value;
            layout.LayerSpacing = (float)nudLayerSpacing.Value;
            layout.NodeSpacing = (float)nudNodeSpacing.Value;
            layout.RowSpacing = (float)nudRowSpacing.Value;
            switch (ddlCompaction.SelectedIndex)
            {
                default:
                case 0:
                    layout.Compaction = GoLayoutTreeCompaction.None;
                    break;
                case 1:
                    layout.Compaction = GoLayoutTreeCompaction.Block;
                    break;
            }
            switch (ddlChildrenAlignment.SelectedIndex)
            {
                default:
                case 0:
                    layout.Alignment = GoLayoutTreeAlignment.CenterSubtrees;
                    break;
                case 1:
                    layout.Alignment = GoLayoutTreeAlignment.CenterChildren;
                    break;
                case 2:
                    layout.Alignment = GoLayoutTreeAlignment.Start;
                    break;
                case 3:
                    layout.Alignment = GoLayoutTreeAlignment.End;
                    break;
            }
            layout.NodeIndent = (float)nudOffset.Value;
            switch (ddlSorting.SelectedIndex)
            {
                default:
                case 0:
                    layout.Sorting = GoLayoutTreeSorting.Forwards;
                    break;
                case 1:
                    layout.Sorting = GoLayoutTreeSorting.Reverse;
                    break;
                case 2:
                    layout.Sorting = GoLayoutTreeSorting.Ascending;
                    break;
                case 3:
                    layout.Sorting = GoLayoutTreeSorting.Descending;
                    break;
            }

            // specify GoLayoutTree.AlternateDefaults properties
            layout.AlternateDefaults.CopyInheritedPropertiesFrom(layout.RootDefaults);
            switch (ddlTreeStyle.SelectedIndex)
            {
                default:
                case 0:
                    layout.Style = GoLayoutTreeStyle.Layered;
                    break;
                case 1:
                    layout.Style = GoLayoutTreeStyle.Alternating;
                    // but specify a different Angle
                    switch (ddlAlternateDirection.SelectedIndex)
                    {
                        default:
                        case 0:
                            layout.AlternateDefaults.Angle = 0;
                            break;
                        case 1:
                            layout.AlternateDefaults.Angle = 90;
                            break;
                        case 2:
                            layout.AlternateDefaults.Angle = 180;
                            break;
                        case 3:
                            layout.AlternateDefaults.Angle = 270;
                            break;
                    }
                    break;
                case 2:
                    layout.Style = GoLayoutTreeStyle.LastParents;
                    // but specify a different BreadthLimit
                    layout.AlternateDefaults.BreadthLimit = (float)nudAlternateBreadthLimit.Value;
                    break;
            }
            layout.Document = UseThisDocument;
            layout.Document.StartTransaction();
            // This removes any existing route produced by a LayeredDigraph auto layout.
            // This code is only needed in this demo application, where you can alternate
            // between different kinds of layout.
            foreach (GoObject obj in layout.Document)
            {
                GoLink link = obj as GoLink;
                if (link != null)
                {
                    link.AdjustingStyle = GoLinkAdjustingStyle.End;
                }
            }
            layout.ArrangementOrigin = new PointF(view.Width / 2, 50);

            if (Auto)
            {
                layout.Angle = 0;
            }

            layout.PerformLayout();
            layout.Document.FinishTransaction("Tree Layout");
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

        // Private Methods (7) 

        private void cbAutoRelink_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            applyButton.Focus();
            Visible = false;
        }

        private void ddlTreeStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlTreeStyle.SelectedIndex)
            {
                default:
                case 0:
                    ddlAlternateDirection.Enabled = false;
                    nudAlternateBreadthLimit.Enabled = false;
                    break;
                case 1:
                    ddlAlternateDirection.Enabled = true;
                    nudAlternateBreadthLimit.Enabled = false;
                    ddlChildrenAlignment.SelectedIndex = 2; // force Start Alignment
                    break;
                case 2:
                    ddlAlternateDirection.Enabled = false;
                    nudAlternateBreadthLimit.Enabled = true;
                    break;
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {
        }

        private void grpSpacing_Enter(object sender, EventArgs e)
        {
        }

        private void grpTreeDirection_Enter(object sender, EventArgs e)
        {
        }

        #endregion Methods

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblDir = new System.Windows.Forms.Label();
            this.ddlTreeDirection = new System.Windows.Forms.ComboBox();
            this.grpSpacing = new System.Windows.Forms.GroupBox();
            this.nudNodeSpacing = new System.Windows.Forms.NumericUpDown();
            this.lblNodeSpacing = new System.Windows.Forms.Label();
            this.nudLayerSpacing = new System.Windows.Forms.NumericUpDown();
            this.lblLayerSpacing = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ddlCompaction = new System.Windows.Forms.ComboBox();
            this.grpMaxTreeSize = new System.Windows.Forms.GroupBox();
            this.nudMaxBreadth = new System.Windows.Forms.NumericUpDown();
            this.lblMaxWidth = new System.Windows.Forms.Label();
            this.lblWarning = new System.Windows.Forms.Label();
            this.nudRowSpacing = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.grpAlignment = new System.Windows.Forms.GroupBox();
            this.nudOffset = new System.Windows.Forms.NumericUpDown();
            this.lblBreadthOffset = new System.Windows.Forms.Label();
            this.lblAlign = new System.Windows.Forms.Label();
            this.ddlChildrenAlignment = new System.Windows.Forms.ComboBox();
            this.applyButton = new MetaControls.MetaButton();
            this.closeButton = new MetaControls.MetaButton();
            this.lblAlternateAngle = new System.Windows.Forms.Label();
            this.grpTreeDirection = new System.Windows.Forms.GroupBox();
            this.ddlAlternateDirection = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ddlSorting = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ddlConstructionPath = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.nudArrangementSpacingH = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.ddlArrangement = new System.Windows.Forms.ComboBox();
            this.nudArrangementSpacingW = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.ddlTreeStyle = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.nudAlternateBreadthLimit = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.cbAutoRelink = new System.Windows.Forms.CheckBox();
            this.grpSpacing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNodeSpacing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLayerSpacing)).BeginInit();
            this.grpMaxTreeSize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxBreadth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRowSpacing)).BeginInit();
            this.grpAlignment.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudOffset)).BeginInit();
            this.grpTreeDirection.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudArrangementSpacingH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudArrangementSpacingW)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAlternateBreadthLimit)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDir
            // 
            this.lblDir.Location = new System.Drawing.Point(9, 21);
            this.lblDir.Name = "lblDir";
            this.lblDir.Size = new System.Drawing.Size(103, 16);
            this.lblDir.TabIndex = 0;
            this.lblDir.Text = "Direction:";
            // 
            // ddlTreeDirection
            // 
            this.ddlTreeDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlTreeDirection.Items.AddRange(new object[]
                                                     {
                                                         "Right",
                                                         "Down",
                                                         "Left",
                                                         "Up"
                                                     });
            this.ddlTreeDirection.SelectedIndex = 0;
            this.ddlTreeDirection.Location = new System.Drawing.Point(124, 19);
            this.ddlTreeDirection.Name = "ddlTreeDirection";
            this.ddlTreeDirection.Size = new System.Drawing.Size(88, 21);
            this.ddlTreeDirection.TabIndex = 1;
            // 
            // grpSpacing
            // 
            this.grpSpacing.Controls.Add(this.nudNodeSpacing);
            this.grpSpacing.Controls.Add(this.lblNodeSpacing);
            this.grpSpacing.Controls.Add(this.nudLayerSpacing);
            this.grpSpacing.Controls.Add(this.lblLayerSpacing);
            this.grpSpacing.Controls.Add(this.label2);
            this.grpSpacing.Controls.Add(this.ddlCompaction);
            this.grpSpacing.Location = new System.Drawing.Point(8, 64);
            this.grpSpacing.Name = "grpSpacing";
            this.grpSpacing.Size = new System.Drawing.Size(218, 120);
            this.grpSpacing.TabIndex = 1;
            this.grpSpacing.TabStop = false;
            this.grpSpacing.Text = "Spacing";
            this.grpSpacing.Enter += new System.EventHandler(this.grpSpacing_Enter);
            // 
            // nudNodeSpacing
            // 
            this.nudNodeSpacing.Location = new System.Drawing.Point(140, 24);
            this.nudNodeSpacing.Maximum = new decimal(new int[]
                                                          {
                                                              999999,
                                                              0,
                                                              0,
                                                              0
                                                          });
            this.nudNodeSpacing.Name = "nudNodeSpacing";
            this.nudNodeSpacing.Size = new System.Drawing.Size(72, 20);
            this.nudNodeSpacing.TabIndex = 1;
            this.nudNodeSpacing.Value = new decimal(new int[]
                                                        {
                                                            20,
                                                            0,
                                                            0,
                                                            0
                                                        });
            // 
            // lblNodeSpacing
            // 
            this.lblNodeSpacing.Location = new System.Drawing.Point(8, 24);
            this.lblNodeSpacing.Name = "lblNodeSpacing";
            this.lblNodeSpacing.Size = new System.Drawing.Size(96, 16);
            this.lblNodeSpacing.TabIndex = 0;
            this.lblNodeSpacing.Text = "Between Nodes:";
            // 
            // nudLayerSpacing
            // 
            this.nudLayerSpacing.Increment = new decimal(new int[]
                                                             {
                                                                 10,
                                                                 0,
                                                                 0,
                                                                 0
                                                             });
            this.nudLayerSpacing.Location = new System.Drawing.Point(140, 56);
            this.nudLayerSpacing.Maximum = new decimal(new int[]
                                                           {
                                                               999999,
                                                               0,
                                                               0,
                                                               0
                                                           });
            this.nudLayerSpacing.Name = "nudLayerSpacing";
            this.nudLayerSpacing.Size = new System.Drawing.Size(72, 20);
            this.nudLayerSpacing.TabIndex = 3;
            this.nudLayerSpacing.Value = new decimal(new int[]
                                                         {
                                                             50,
                                                             0,
                                                             0,
                                                             0
                                                         });
            // 
            // lblLayerSpacing
            // 
            this.lblLayerSpacing.Location = new System.Drawing.Point(8, 56);
            this.lblLayerSpacing.Name = "lblLayerSpacing";
            this.lblLayerSpacing.Size = new System.Drawing.Size(96, 16);
            this.lblLayerSpacing.TabIndex = 2;
            this.lblLayerSpacing.Text = "Between Layers:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Compaction:";
            // 
            // ddlCompaction
            // 
            this.ddlCompaction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCompaction.Items.AddRange(new object[]
                                                  {
                                                      "None",
                                                      "Block"
                                                  });
            this.ddlCompaction.Location = new System.Drawing.Point(124, 88);
            this.ddlCompaction.Name = "ddlCompaction";
            this.ddlCompaction.Size = new System.Drawing.Size(88, 21);
            this.ddlCompaction.TabIndex = 5;
            // 
            // grpMaxTreeSize
            // 
            this.grpMaxTreeSize.Controls.Add(this.nudMaxBreadth);
            this.grpMaxTreeSize.Controls.Add(this.lblMaxWidth);
            this.grpMaxTreeSize.Controls.Add(this.lblWarning);
            this.grpMaxTreeSize.Controls.Add(this.nudRowSpacing);
            this.grpMaxTreeSize.Controls.Add(this.label1);
            this.grpMaxTreeSize.Location = new System.Drawing.Point(232, 8);
            this.grpMaxTreeSize.Name = "grpMaxTreeSize";
            this.grpMaxTreeSize.Size = new System.Drawing.Size(208, 96);
            this.grpMaxTreeSize.TabIndex = 2;
            this.grpMaxTreeSize.TabStop = false;
            this.grpMaxTreeSize.Text = "Maximum Tree Size";
            // 
            // nudMaxBreadth
            // 
            this.nudMaxBreadth.Increment = new decimal(new int[]
                                                           {
                                                               25,
                                                               0,
                                                               0,
                                                               0
                                                           });
            this.nudMaxBreadth.Location = new System.Drawing.Point(128, 19);
            this.nudMaxBreadth.Maximum = new decimal(new int[]
                                                         {
                                                             999999,
                                                             0,
                                                             0,
                                                             0
                                                         });
            this.nudMaxBreadth.Name = "nudMaxBreadth";
            this.nudMaxBreadth.Size = new System.Drawing.Size(72, 20);
            this.nudMaxBreadth.TabIndex = 2;
            // 
            // lblMaxWidth
            // 
            this.lblMaxWidth.Location = new System.Drawing.Point(8, 21);
            this.lblMaxWidth.Name = "lblMaxWidth";
            this.lblMaxWidth.Size = new System.Drawing.Size(112, 16);
            this.lblMaxWidth.TabIndex = 0;
            this.lblMaxWidth.Text = "Maximum Width:";
            // 
            // lblWarning
            // 
            this.lblWarning.Location = new System.Drawing.Point(8, 37);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(184, 16);
            this.lblWarning.TabIndex = 1;
            this.lblWarning.Text = "(Zero if not limited)";
            // 
            // nudRowSpacing
            // 
            this.nudRowSpacing.Location = new System.Drawing.Point(128, 59);
            this.nudRowSpacing.Maximum = new decimal(new int[]
                                                         {
                                                             999999,
                                                             0,
                                                             0,
                                                             0
                                                         });
            this.nudRowSpacing.Name = "nudRowSpacing";
            this.nudRowSpacing.Size = new System.Drawing.Size(72, 20);
            this.nudRowSpacing.TabIndex = 4;
            this.nudRowSpacing.Value = new decimal(new int[]
                                                       {
                                                           25,
                                                           0,
                                                           0,
                                                           0
                                                       });
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "Row Spacing between Children:";
            // 
            // grpAlignment
            // 
            this.grpAlignment.Controls.Add(this.nudOffset);
            this.grpAlignment.Controls.Add(this.lblBreadthOffset);
            this.grpAlignment.Controls.Add(this.lblAlign);
            this.grpAlignment.Controls.Add(this.ddlChildrenAlignment);
            this.grpAlignment.Location = new System.Drawing.Point(232, 112);
            this.grpAlignment.Name = "grpAlignment";
            this.grpAlignment.Size = new System.Drawing.Size(208, 72);
            this.grpAlignment.TabIndex = 3;
            this.grpAlignment.TabStop = false;
            this.grpAlignment.Text = "Parent to Children Alignment";
            // 
            // nudOffset
            // 
            this.nudOffset.Location = new System.Drawing.Point(88, 43);
            this.nudOffset.Name = "nudOffset";
            this.nudOffset.Size = new System.Drawing.Size(56, 20);
            this.nudOffset.TabIndex = 3;
            // 
            // lblBreadthOffset
            // 
            this.lblBreadthOffset.AutoSize = true;
            this.lblBreadthOffset.Location = new System.Drawing.Point(12, 45);
            this.lblBreadthOffset.Name = "lblBreadthOffset";
            this.lblBreadthOffset.Size = new System.Drawing.Size(40, 13);
            this.lblBreadthOffset.TabIndex = 2;
            this.lblBreadthOffset.Text = "Indent:";
            // 
            // lblAlign
            // 
            this.lblAlign.Location = new System.Drawing.Point(12, 24);
            this.lblAlign.Name = "lblAlign";
            this.lblAlign.Size = new System.Drawing.Size(72, 16);
            this.lblAlign.TabIndex = 0;
            this.lblAlign.Text = "Alignment:";
            // 
            // ddlChildrenAlignment
            // 
            this.ddlChildrenAlignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlChildrenAlignment.Items.AddRange(new object[]
                                                         {
                                                             "Center Subtrees",
                                                             "Center Children",
                                                             "Start",
                                                             "End"
                                                         });
            this.ddlChildrenAlignment.Location = new System.Drawing.Point(88, 21);
            this.ddlChildrenAlignment.Name = "ddlChildrenAlignment";
            this.ddlChildrenAlignment.Size = new System.Drawing.Size(112, 21);
            this.ddlChildrenAlignment.TabIndex = 1;
            // 
            // applyButton
            // 
            this.applyButton.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.applyButton.Location = new System.Drawing.Point(16, 371);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(75, 25);
            this.applyButton.TabIndex = 8;
            this.applyButton.Text = "Apply";
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(360, 371);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 25);
            this.closeButton.TabIndex = 9;
            this.closeButton.Text = "Close";
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // lblAlternateAngle
            // 
            this.lblAlternateAngle.AutoSize = true;
            this.lblAlternateAngle.Location = new System.Drawing.Point(8, 52);
            this.lblAlternateAngle.Name = "lblAlternateAngle";
            this.lblAlternateAngle.Size = new System.Drawing.Size(97, 13);
            this.lblAlternateAngle.TabIndex = 3;
            this.lblAlternateAngle.Text = "Alternate Direction:";
            // 
            // grpTreeDirection
            // 
            this.grpTreeDirection.Controls.Add(this.lblDir);
            this.grpTreeDirection.Controls.Add(this.ddlTreeDirection);
            this.grpTreeDirection.Location = new System.Drawing.Point(8, 8);
            this.grpTreeDirection.Name = "grpTreeDirection";
            this.grpTreeDirection.Size = new System.Drawing.Size(218, 48);
            this.grpTreeDirection.TabIndex = 0;
            this.grpTreeDirection.TabStop = false;
            this.grpTreeDirection.Text = "Tree Growth Direction";
            this.grpTreeDirection.Enter += new System.EventHandler(this.grpTreeDirection_Enter);
            // 
            // ddlAlternateDirection
            // 
            this.ddlAlternateDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlAlternateDirection.Enabled = false;
            this.ddlAlternateDirection.Items.AddRange(new object[]
                                                          {
                                                              "Right",
                                                              "Down",
                                                              "Left",
                                                              "Up"
                                                          });
            this.ddlAlternateDirection.Location = new System.Drawing.Point(112, 48);
            this.ddlAlternateDirection.Name = "ddlAlternateDirection";
            this.ddlAlternateDirection.Size = new System.Drawing.Size(88, 21);
            this.ddlAlternateDirection.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ddlSorting);
            this.groupBox1.Location = new System.Drawing.Point(232, 192);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(208, 48);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Ordering of Children";
            // 
            // ddlSorting
            // 
            this.ddlSorting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlSorting.Items.AddRange(new object[]
                                               {
                                                   "Forwards Iteration",
                                                   "Backwards Iteration",
                                                   "Ascending Text Sort",
                                                   "Descending Text Sort"
                                               });
            this.ddlSorting.Location = new System.Drawing.Point(8, 16);
            this.ddlSorting.Name = "ddlSorting";
            this.ddlSorting.Size = new System.Drawing.Size(192, 21);
            this.ddlSorting.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ddlConstructionPath);
            this.groupBox2.Location = new System.Drawing.Point(8, 192);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(218, 48);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tree Construction Path";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // ddlConstructionPath
            // 
            this.ddlConstructionPath.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlConstructionPath.Items.AddRange(new object[]
                                                        {
                                                            "Destinations are Children",
                                                            "Sources are Children"
                                                        });
            this.ddlConstructionPath.Location = new System.Drawing.Point(20, 16);
            this.ddlConstructionPath.Name = "ddlConstructionPath";
            this.ddlConstructionPath.Size = new System.Drawing.Size(192, 21);
            this.ddlConstructionPath.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.nudArrangementSpacingH);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.ddlArrangement);
            this.groupBox3.Controls.Add(this.nudArrangementSpacingW);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(8, 248);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(218, 120);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Forest Arrangement";
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // nudArrangementSpacingH
            // 
            this.nudArrangementSpacingH.Increment = new decimal(new int[]
                                                                    {
                                                                        10,
                                                                        0,
                                                                        0,
                                                                        0
                                                                    });
            this.nudArrangementSpacingH.Location = new System.Drawing.Point(156, 50);
            this.nudArrangementSpacingH.Maximum = new decimal(new int[]
                                                                  {
                                                                      999,
                                                                      0,
                                                                      0,
                                                                      0
                                                                  });
            this.nudArrangementSpacingH.Name = "nudArrangementSpacingH";
            this.nudArrangementSpacingH.Size = new System.Drawing.Size(56, 20);
            this.nudArrangementSpacingH.TabIndex = 3;
            this.nudArrangementSpacingH.Value = new decimal(new int[]
                                                                {
                                                                    50,
                                                                    0,
                                                                    0,
                                                                    0
                                                                });
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "Arrangement:";
            // 
            // ddlArrangement
            // 
            this.ddlArrangement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlArrangement.Items.AddRange(new object[]
                                                   {
                                                       "Vertical",
                                                       "Horizontal",
                                                       "FixedRoots"
                                                   });
            this.ddlArrangement.Location = new System.Drawing.Point(124, 24);
            this.ddlArrangement.Name = "ddlArrangement";
            this.ddlArrangement.Size = new System.Drawing.Size(88, 21);
            this.ddlArrangement.TabIndex = 5;
            // 
            // nudArrangementSpacingW
            // 
            this.nudArrangementSpacingW.Increment = new decimal(new int[]
                                                                    {
                                                                        10,
                                                                        0,
                                                                        0,
                                                                        0
                                                                    });
            this.nudArrangementSpacingW.Location = new System.Drawing.Point(84, 50);
            this.nudArrangementSpacingW.Maximum = new decimal(new int[]
                                                                  {
                                                                      999,
                                                                      0,
                                                                      0,
                                                                      0
                                                                  });
            this.nudArrangementSpacingW.Name = "nudArrangementSpacingW";
            this.nudArrangementSpacingW.Size = new System.Drawing.Size(56, 20);
            this.nudArrangementSpacingW.TabIndex = 3;
            this.nudArrangementSpacingW.Value = new decimal(new int[]
                                                                {
                                                                    50,
                                                                    0,
                                                                    0,
                                                                    0
                                                                });
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(141, 52);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(32, 18);
            this.label9.TabIndex = 7;
            this.label9.Text = "H:";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(62, 52);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(25, 18);
            this.label11.TabIndex = 6;
            this.label11.Text = "W:";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 18);
            this.label4.TabIndex = 2;
            this.label4.Text = "Spacing:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.ddlTreeStyle);
            this.groupBox4.Controls.Add(this.lblAlternateAngle);
            this.groupBox4.Controls.Add(this.ddlAlternateDirection);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.nudAlternateBreadthLimit);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Location = new System.Drawing.Point(232, 248);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(208, 120);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Tree Style";
            // 
            // ddlTreeStyle
            // 
            this.ddlTreeStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlTreeStyle.Items.AddRange(new object[]
                                                 {
                                                     "Layered",
                                                     "Alternating",
                                                     "LastParents"
                                                 });
            this.ddlTreeStyle.Location = new System.Drawing.Point(8, 19);
            this.ddlTreeStyle.Name = "ddlTreeStyle";
            this.ddlTreeStyle.Size = new System.Drawing.Size(192, 21);
            this.ddlTreeStyle.TabIndex = 1;
            this.ddlTreeStyle.SelectedIndexChanged += new System.EventHandler(this.ddlTreeStyle_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 96);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(116, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Alternate Width Limit:";
            // 
            // nudAlternateBreadthLimit
            // 
            this.nudAlternateBreadthLimit.Enabled = false;
            this.nudAlternateBreadthLimit.Increment = new decimal(new int[]
                                                                      {
                                                                          25,
                                                                          0,
                                                                          0,
                                                                          0
                                                                      });
            this.nudAlternateBreadthLimit.Location = new System.Drawing.Point(136, 94);
            this.nudAlternateBreadthLimit.Maximum = new decimal(new int[]
                                                                    {
                                                                        999999,
                                                                        0,
                                                                        0,
                                                                        0
                                                                    });
            this.nudAlternateBreadthLimit.Name = "nudAlternateBreadthLimit";
            this.nudAlternateBreadthLimit.Size = new System.Drawing.Size(64, 20);
            this.nudAlternateBreadthLimit.TabIndex = 4;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(9, 72);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(184, 16);
            this.label10.TabIndex = 1;
            this.label10.Text = "(Alignment is set to Start)";
            // 
            // cbAutoRelink
            // 
            this.cbAutoRelink.AutoSize = true;
            this.cbAutoRelink.Location = new System.Drawing.Point(97, 374);
            this.cbAutoRelink.Name = "cbAutoRelink";
            this.cbAutoRelink.Size = new System.Drawing.Size(81, 17);
            this.cbAutoRelink.TabIndex = 10;
            this.cbAutoRelink.Text = "Auto-Relink";
            this.cbAutoRelink.UseVisualStyleBackColor = true;
            this.cbAutoRelink.CheckedChanged += new System.EventHandler(this.cbAutoRelink_CheckedChanged);
            // 
            // TreeLayoutDialog
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(445, 402);
            this.ControlBox = false;
            this.Controls.Add(this.cbAutoRelink);
            this.Controls.Add(this.grpTreeDirection);
            this.Controls.Add(this.grpMaxTreeSize);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.grpAlignment);
            this.Controls.Add(this.grpSpacing);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TreeLayoutDialog";
            this.ShowInTaskbar = true;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Tree Layout Settings";
            this.grpSpacing.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudNodeSpacing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLayerSpacing)).EndInit();
            this.grpMaxTreeSize.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxBreadth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRowSpacing)).EndInit();
            this.grpAlignment.ResumeLayout(false);
            this.grpAlignment.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudOffset)).EndInit();
            this.grpTreeDirection.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudArrangementSpacingH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudArrangementSpacingW)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAlternateBreadthLimit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}