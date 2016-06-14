using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Controllers;
using MetaBuilder.Graphing.Persistence;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Primitives;
using MetaBuilder.Graphing.Utilities;
using MetaBuilder.Meta;
using MetaBuilder.UIControls;
using Northwoods.Go;
using Northwoods.Go.Draw;
using Northwoods.Go.Xml;
using ShapeBuilding.Archives;

namespace ShapeBuilding
{
    public partial class Form1 : Form
    {
        public void AddContextNode()
        {
            AutoLinkContextNode n1 = new AutoLinkContextNode();
            n1.Position = new PointF(-500, -500);
            n1.Text = "N1";
            AutoLinkContextNode n2 = new AutoLinkContextNode();
            n2.Text = "N2";
            n2.Position = new PointF(1500, 500);

            AutoLinkContextNode n3 = new AutoLinkContextNode();
            n3.Text = "AutoLinkContextNode AutoLinkContextNode AutoLinkContextNode";
            n3.Position = new PointF(-550, 550);

            QLink sl = new QLink();
            sl.FromPort = n1.Port;
            sl.ToPort = n2.Port;
            sl.AssociationType = LinkAssociationType.Mapping;


            QLink sl3 = new QLink();
            sl3.FromPort = n1.Port;
            sl3.ToPort = n3.Port;
            sl3.AssociationType = LinkAssociationType.Mapping;
            this.graphView1.Document.Add(n1);
            this.graphView1.Document.Add(n2);
            this.graphView1.Document.Add(n3);
            this.graphView1.Document.Add(sl);
            this.graphView1.Document.Add(sl3);
        }

        public void ResizeDocument()
        {
            SizeF docSize = this.graphView1.DocumentSize;
            
            PointF pSheetPosition = new PointF();
            pSheetPosition.X = graphView1.DocumentTopLeft.X - graphView1.Sheet.TopLeftMargin.Width;
            pSheetPosition.Y = graphView1.DocumentTopLeft.Y - graphView1.Sheet.TopLeftMargin.Height;
            this.graphView1.Sheet.Position = pSheetPosition;
            this.graphView1.Sheet.Size = new SizeF(docSize.Width + graphView1.Sheet.TopLeftMargin.Width + graphView1.Sheet.BottomRightMargin.Width,
                docSize.Height + graphView1.Sheet.TopLeftMargin.Height + graphView1.Sheet.BottomRightMargin.Height);
        }
        public Form1()
        {
            InitializeComponent();
            //ShapeBuilding.Archives.VC.ValueChain vc = new ShapeBuilding.Archives.VC.ValueChain();
            //this.graphView1.Document.Add(vc);
            this.graphView1.PortGravity = 2;
            this.graphView1.Grid.Style = GoViewGridStyle.None;
            this.graphView1.Grid.SnapDrag = GoViewSnapStyle.None;
            this.graphView1.Grid.SnapDragWhole = false;
            this.graphView1.Grid.SnapOpaque = false;
            this.graphView1.Grid.SnapResize = GoViewSnapStyle.None;


            GoLink lnk = new GoLink();
            lnk.Orthogonal = true;
            lnk.AvoidsNodes = true;
            this.graphView1.NewLinkPrototype = lnk;
            

            Design.MyAutoLinkNode aln = new ShapeBuilding.Design.MyAutoLinkNode();
            aln.Text = "Auto Link Node";
            this.graphView1.Document.Add(aln);
            aln.Position = new PointF(100, 100);


            Design.MyAutoLinkNode aln2 = new ShapeBuilding.Design.MyAutoLinkNode();
            aln2.Text = "Another One";
            this.graphView1.Document.Add(aln2);
            aln.Position = new PointF(200, 200);
            lnk.FromPort = aln.Port;
            lnk.ToPort = aln2.Port;

            graphView1.Document.Add(lnk);


            return;
            //AddContextNode();
            ResizeDocument();
            return;
            AddGraphDoc();
            StickyNode snode1 = new StickyNode();
            snode1.Text = "Node 1";
            this.graphView1.Document.Add(snode1);
            StickyNode snode2= new StickyNode();
            snode2.Text = "Node 1";
            this.graphView1.Document.Add(snode2);
            
            //graphView1.ReplaceMouseTool(typeof(GoToolDragging), new SubGraphDraggingTool(graphView1));
         /*   hashNodes = new Hashtable();
            AddReport();
            AddDataView();
            //AddSplitLabel();
            AddEntity();
            AddDSD();
            //AddDSD();
            // AddSplitLabel();
            //AddObject();
            graphView1.KeyUp += new KeyEventHandler(graphView1_KeyUp);*/
            //ShowPrintForm();

      
        }
        void ShowPrintForm()
        {
            Archives.CustomPrinting.MyPrintDialog mpd =
                new Archives.CustomPrinting.MyPrintDialog();
            mpd.Document = graphView1.Document;
            //mpd.DoTests();
            mpd.ShowDialog(this);
        }

        void AddEntity()
        {
            DataEntityBuilder ndtbuilder = new DataEntityBuilder();
            GoObject o = ndtbuilder.GetShape();
            o.Position = new PointF(0, 410);
            graphView1.Doc.Add(o);
        }
        void AddReport()
        {
            ReportShapeBuilder rsbuilder = new ReportShapeBuilder();
            GoObject o = rsbuilder.GetShape();
            o.Position = new PointF(0, 0);
            graphView1.Doc.Add(o);
        }

        void AddDataView()
        {
            DataViewShapeBuilder rsbuilder = new DataViewShapeBuilder();
            GoObject o = rsbuilder.GetShape();
            o.Position = new PointF(250, 0);
            graphView1.Doc.Add(o);
        }
        void graphView1_KeyUp(object sender, KeyEventArgs e)
        {
       /*     if (e.KeyCode == Keys.J)
            {
                GraphNode n = graphView1.Selection.Primary.ParentNode as GraphNode;
                if (n != null)
                {
                    //n.SaveToDatabase(sender, e);
                    n.Remove();
                    graphView1.Selection.Clear();
                    MetaBuilder.Graphing.Utilities.StorageManipulator.FileSystemManipulator.SaveObject(n, @"c:\NewJobPosition.sym");
                }
            }*/
            if (e.KeyCode == Keys.R)
            {
                GraphNode n = graphView1.Selection.Primary.ParentNode as GraphNode;
                if (n != null)
                {
                    //n.SaveToDatabase(sender, e);
                    n.Remove();
                    graphView1.Selection.Clear();
                    n.HookupEvents();
                    StorageManipulator.FileSystemManipulator.SaveObject(n, @"c:\NewReport.sym");
                }
            }
            if (e.KeyCode == Keys.V)
            {
                GraphNode n = graphView1.Selection.Primary.ParentNode as GraphNode;
                if (n != null)
                {
                    //n.SaveToDatabase(sender, e);
                    n.Remove();
                    graphView1.Selection.Clear();
                    n.HookupEvents();
                    StorageManipulator.FileSystemManipulator.SaveObject(n, @"c:\NewDataView.sym");
                }
            }
            if (e.KeyCode == Keys.D)
            {
                GraphNode n = graphView1.Selection.Primary.ParentNode as GraphNode;
                if (n != null)
                {
                    //n.SaveToDatabase(sender, e);
                    n.Remove();
                    graphView1.Selection.Clear();
                    n.HookupEvents();
                    StorageManipulator.FileSystemManipulator.SaveObject(n, @"c:\NewDataTable.sym");
                }
            }
            if (e.KeyCode == Keys.E)
            {
                GraphNode n = graphView1.Selection.Primary.ParentNode as GraphNode;
                if (n != null)
                {
                    CollapsibleNode cnode = n as CollapsibleNode;
                    GoInputEventArgs ieargs = new GoInputEventArgs();
                    //cnode.Handle.OnSingleClick(ieargs, graphView1);
                    //n.SaveToDatabase(sender, e);
                    n.Remove();
                    graphView1.Selection.Clear();
                    
                    StorageManipulator.FileSystemManipulator.SaveObject(n, @"c:\NewEntity.sym");
                }
            }
            /*#region Handle Insert key
            if (e.KeyCode == Keys.Insert)
            {
                if (graphView1.Selection.Primary != null)
                {
                    if ((graphView1.Selection.Primary.ParentNode is CollapsibleNode) || (graphView1.Selection.Primary is CollapsibleNode) || (graphView1.Selection.Primary is CollapsingRecordNodeItemList))
                    {


                        if (graphView1.Selection.Primary.Parent.Parent is RepeaterSection)
                        {
                            RepeaterSection rSection = graphView1.Selection.Primary.Parent.Parent as RepeaterSection;
                            if (rSection != null)
                            {
                                graphView1.StartTransaction();
                                rSection.AddItemFromCode();
                                graphView1.FinishTransaction("Add item");
                            }
                        }



                    }
                }
            }
            #endregion*/
        }
        void AddObject()
        {
            return;
            GraphFileManager gfman = new GraphFileManager();
            GraphNode node = StorageManipulator.FileSystemManipulator.LoadSymbol(@"H:\development\Code\DISCON MetaBuilder\AppFolder\MetaData\Symbols\Organisation Unit.sym") as GraphNode;
            graphView1.Doc.Add(node);
            GoGroupEnumerator enumerator = node.GetEnumerator();
            int i = 0;
            while (enumerator.MoveNext())
            {

                // Console.WriteLine(i.ToString() + " " + enumerator.Current.ToString());
                i++;
            }
            ShapeCollapsibleHandle chandle = new ShapeCollapsibleHandle();
            chandle.Style = GoCollapsibleHandleStyle.PlusMinus;
            node.EditMode = true;
            node.Add(chandle);
            chandle.Position = new PointF(10, 100);
            GradientRoundedRectangle rectBig = new GradientRoundedRectangle();
            rectBig.Name = "Rect";

            rectBig.Size = new SizeF(node[7].Width, 17);
            rectBig.Brush = Brushes.GhostWhite;
            rectBig.Position = new PointF(node[7].Left, node.Grid.Height - rectBig.Height - 10) ;// new PointF(10, 100);
            rectBig.Corner = new SizeF(0, 0);
            node.Add(rectBig);
            CollapsibleLabel txtReq = new CollapsibleLabel();
            CollapsibleLabel lblReq = new CollapsibleLabel();
            lblReq.Text = "Req:";
            lblReq.Selectable = false;

            CollapsibleLabel txtOcc = new CollapsibleLabel();
            CollapsibleLabel lblOcc = new CollapsibleLabel();
            lblOcc.Text = "Occ:";
            lblOcc.Selectable = false;
            CollapsibleLabel txtAva = new CollapsibleLabel();
            CollapsibleLabel lblAva = new CollapsibleLabel();
            lblAva.Text = "Avl:";
            lblAva.Selectable = false;
            
            node.Add(txtReq);
            node.Add(txtOcc);
            node.Add(txtAva);
            node.Add(lblReq);
            node.Add(lblOcc);
            node.Add(lblAva);
            
            txtReq.Editable = true;
            txtOcc.Editable = true;
            txtAva.Editable = true;

            txtOcc.EditorStyle = GoTextEditorStyle.NumericUpDown;
            txtOcc.Minimum = -10000;
            txtOcc.Maximum = 10000;
            txtAva.EditorStyle = GoTextEditorStyle.NumericUpDown;
            txtAva.Minimum = -10000;
            txtAva.Maximum = 10000;

            txtReq.EditorStyle = GoTextEditorStyle.NumericUpDown;
            txtReq.Minimum = -10000;
            txtReq.Maximum = 10000;

            lblReq.Position = new PointF(rectBig.Position.X, rectBig.Position.Y);
            txtReq.Position = new PointF(rectBig.Position.X + lblReq.Width, rectBig.Position.Y + 3f);
            lblOcc.Position = new PointF(rectBig.Position.X + lblReq.Width + 25, rectBig.Position.Y);
            txtOcc.Position = new PointF(lblOcc.Position.X + lblOcc.Width, rectBig.Position.Y + 3f);
            lblAva.Position = new PointF(lblOcc.Position.X + lblOcc.Width + 25, rectBig.Position.Y);
            txtAva.Position = new PointF(lblAva.Position.X + lblAva.Width+3, rectBig.Position.Y + 3f);

            lblReq.FontSize = 8;
            lblOcc.FontSize = 8;
            lblAva.FontSize = 8;
            txtReq.FontSize = 8;
            txtOcc.FontSize = 8;
            txtAva.FontSize = 8;
            txtReq.Text = "0";
            txtOcc.Text = "0";
            txtAva.Text = "0";
            txtReq.Name = "TotalRequired";
            txtOcc.Name = "TotalOccupied";
            txtAva.Name = "TotalAvailable";
            chandle.Collapsibles.Add(txtReq.Name);
            chandle.Collapsibles.Add(txtOcc.Name);
            chandle.Collapsibles.Add(txtAva.Name);
            chandle.Collapsibles.Add(lblAva.Name);
            chandle.Collapsibles.Add(lblOcc.Name);
            chandle.Collapsibles.Add(lblReq.Name);
            chandle.Collapsibles.Add(rectBig.Name);
            node.BindingInfo = new BindingInfo();
            node.BindingInfo.BindingClass = "JobPosition";

            //node.BindingInfo.Bindings.Add((node.FindByName("[12] as BoundLabel).Name,"Name");
            node.BindingInfo.Bindings.Add("TotalRequired", "TotalRequired");
            node.BindingInfo.Bindings.Add("TotalOccupied", "TotalOccupied");
            node.BindingInfo.Bindings.Add("TotalAvailable", "TotalAvailable");
            rectBig.Resizable = false;
            rectBig.Reshapable = false;
            
            //node[17].Remove();
            // 6 = name now 
            node[20].Height = node[14].Height - rectBig.Height;
            (node[20] as BoundLabel).Text = "JobPosition";
            (node[20] as BoundLabel).Alignment = 2;
            chandle.Position = new PointF(rectBig.Position.X - chandle.Width, rectBig.Position.Y);
            chandle.Style = GoCollapsibleHandleStyle.PlusMinus;
            chandle.Printable = false;
            chandle.Selectable = false;
            GoInputEventArgs ieargs = new GoInputEventArgs();
            chandle.Selectable = false;
            chandle.Deletable = false;
            chandle.Movable = false;
            rectBig.Selectable = false;
            rectBig.Deletable = false;
            rectBig.Movable = false;
            txtReq.Selectable = false;
            txtOcc.Selectable = false;
            txtAva.Selectable = false;
            txtReq.Deletable = false;
            txtOcc.Deletable = false;
            txtAva.Deletable = false;
            txtReq.Movable = false;
            txtOcc.Movable = false;
            txtAva.Movable = false;
            node.BindToMetaObjectProperties();
          
           // chandle.OnSingleClick(ieargs, graphView1);
            


        }

        void AddSplitLabel()
        {
            /*SplitLabel slabel = new SplitLabel();
            graphView1.Doc.Add(slabel);
            slabel.Expand();*/
            /*SplitLabelList slist = new SplitLabelList();
            graphView1.Doc.Add(slist);*/
            CoreInjector cinjector = new CoreInjector();
            cinjector.InjectCoreVariables();
          
            NewDataTableBuilder ndtbuilder = new NewDataTableBuilder();
            GoObject o = ndtbuilder.GetShape();
            o.Position = new PointF(0, 200);
            graphView1.Doc.Add(o);
        }
        private void AddSymbolDoc()
        {
            graphView1.Document = new Symbol();
            graphView1.Dock = DockStyle.Fill;
            graphView1.SheetStyle = GoViewSheetStyle.None;
            graphView1.NewLinkPrototype = new QLink();
            GraphViewController gvc = new GraphViewController();
            gvc.MyView = graphView1;
            gvc.Hookup();
            CoreInjector cinjector = new CoreInjector();
            cinjector.InjectCoreVariables();
            graphView1.Document.Clear();

        }

        private void AddGraphDoc()
        {
            NormalDiagram nd = new NormalDiagram();
            nd.CreateFrameLayer(graphView1);
            nd.DocumentFrame.Size = new SizeF(500, 500);// new SizeF(500, 969);
            graphView1.Document = nd;
            
            //nd.Update();
            graphView1.Dock = DockStyle.Fill;
            graphView1.SheetStyle = GoViewSheetStyle.Sheet;
            graphView1.NewLinkPrototype = new QLink();
            GraphViewController gvc = new GraphViewController();
            gvc.MyView = graphView1;

            gvc.Hookup();
            CoreInjector cinjector = new CoreInjector();
            cinjector.InjectCoreVariables();
            //graphView1.Document.Clear();
        }
        public void AddDSD()
        {
            NewDataTableBuilder dtsBuilder = new NewDataTableBuilder();
            GraphNode node = dtsBuilder.GetShape();
            node.Remove();
            NormalDiagram ndiagram = graphView1.Document as NormalDiagram;
            ndiagram.Add(node);
            
            //ndiagram.ShapeBindingInfo = node.BindingInfo;
           // MetaBuilder.Graphing.Utilities.StorageManipulator.FileSystemManipulator.SaveObject(node, @"c:\\dsdtemp.sym");
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {
            
            /*AddSymbolDoc();
            AddDSD();*/
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            graphView1.Document.Clear();
            /*FSDLayout.FSDShape s1 = new ShapeBuilding.FSDLayout.FSDShape();
            FSDLayout.FSDShape s2 = new ShapeBuilding.FSDLayout.FSDShape();
            FSDLayout.FSDShape s3 = new ShapeBuilding.FSDLayout.FSDShape();

            graphView1.Document.Add(s1);
            graphView1.Document.Add(s2);
            graphView1.Document.Add(s3);
            */
            return;
            NodeDropper nDropper = new NodeDropper();
            nDropper.DropNodes(graphView1);
        }

        private Hashtable hashNodes;
        private void button2_Click(object sender, EventArgs e)
        {
            SubGraphDraggingTool sgTool = new SubGraphDraggingTool(graphView1);
            graphView1.ReplaceMouseTool(typeof(GoDrawToolDragging), sgTool);
            SubGraphDraggingTool tool = (SubGraphDraggingTool)graphView1.FindMouseTool(typeof(SubGraphDraggingTool), true);
            tool.ComputeEffectiveSelection(graphView1.Selection, true);
        }

        void graphView1_LinkRelinked(object sender, GoSelectionEventArgs e)
        {
            QLink newQLink = e.GoObject as QLink;
            GraphNode fromNode = newQLink.FromNode as GraphNode;
            GraphNode toNode = newQLink.ToNode as GraphNode;
            TreeNode fromTreeNode = FindTreeNode(fromNode);
            TreeNode toTreeNode = FindTreeNode(toNode);
            IGoNode node = newQLink.FromNode;
            if ((GetNodeByTag(toTreeNode.Nodes, fromTreeNode) != null) || (GetNodeByTag(fromTreeNode.Nodes, toTreeNode) != null))
            {
                RemoveInvalidLink(newQLink);
            }
            else
                MoveTreeNode(fromTreeNode, toTreeNode);
        }

        private static void RemoveInvalidLink(QLink newQLink)
        {
            MessageBox.Show("The treeviewer does not allow recursive linking. Either close the treeviewer or validate your document.");
            newQLink.Remove();
        }

        public TreeNode GetNodeByTag(TreeNodeCollection nodes, TreeNode target)
        {
            TreeNode result = null;
            foreach (TreeNode node in nodes)
            {
                if (node == target)
                {
                    result = node;
                    break;
                }

                if (node.Nodes.Count > 0)
                {
                    result = GetNodeByTag(node.Nodes, target);
                    if (result != null)
                        break;
                }
            }
            return result;
        }

        private static void MoveTreeNode(TreeNode fromTreeNode, TreeNode toTreeNode)
        {
            GraphNode nodeTo = toTreeNode.Tag as GraphNode;
            GraphNode nodeFrom = fromTreeNode.Tag as GraphNode;
           
            toTreeNode.Remove();
            fromTreeNode.Nodes.Add(toTreeNode);
        }
        private void graphView1_DocumentChanged(object sender, GoChangedEventArgs e)
        {
            /*if (e.GoObject != null)
                // Console.WriteLine("Hint: {0} Object: {1}", new object[] { e.Hint, e.GoObject });*/
            switch (e.Hint)
            {
                case 901:
                    if (e.Object is GraphNode)
                    {
                        GraphNode graphnode = e.Object as GraphNode;
                        if (graphnode.MetaObject != null)
                        {
                            if (FindTreeNode(graphnode) == null)
                            {
                                TreeNode treenode = new TreeNode();
                                treenode.Text = graphnode.MetaObject.ToString();
                                graphnode.MetaObject.Changed += new EventHandler(MetaObject_Changed);
                                treenode.Tag = graphnode;
                                treeView1.Nodes.Add(treenode);
                                treeView1.Refresh();
                                hashNodes.Add(graphnode, treenode);
                            }
                        }
                    }
                 
                    break;
                   
                case 903:// object was  removed
                    GraphNode deletednode = e.GoObject as GraphNode;
                    if (deletednode != null)
                    {
                        TreeNode tnDeleted = FindTreeNode(deletednode);
                        if (tnDeleted != null)
                        {
                            for (int i = 0; i < tnDeleted.Nodes.Count; i++)
                            {
                                TreeNode tnChild = tnDeleted.Nodes[i];
                                tnChild.Remove();
                                treeView1.Nodes.Add(tnChild);
                            }
                            tnDeleted.Remove();
                        }
                        treeView1.Refresh();
                    }
                    
                    QLink link = e.GoObject as QLink;
                    if (link != null)
                    {
                        GraphNode node = link.FromNode as GraphNode;
                        GraphNode nodeChild = link.ToNode as GraphNode;
                        if (nodeChild != null)
                        {
                            TreeNode treeNodeParent = FindTreeNode(node);
                            if (treeNodeParent != null)
                            {
                                TreeNode treeNodeChild = FindTreeNode(nodeChild);
                                if (treeNodeChild != null && graphView1.Document.Contains(nodeChild) && (!(nodeChild.BeingRemoved)))
                                {
                                    if (!link.BeingRemoved)
                                    {
                                        treeNodeChild.Remove();
                                        treeView1.Nodes.Add(treeNodeChild);
                                        treeView1.Refresh();
                                    }
                                }
                            }
                        }
                    }

                    
                    break;
            }
           
        }

        void MetaObject_Changed(object sender, EventArgs e)
        {
            TreeNode selectedNode = FindTreeNode(sender as MetaBase);
            if (sender != null)
            {
                selectedNode.Text = sender.ToString();
                treeView1.SelectedNode = selectedNode;
            }
        }

        public TreeNode FindTreeNode(GraphNode node)
        {
            foreach (DictionaryEntry entry in hashNodes)
            {
                if (entry.Key == node)
                {
                    return entry.Value as TreeNode;
                }
            }
            return null;
        }

        public TreeNode FindTreeNode(MetaBase mbase)
        {
            foreach (DictionaryEntry entry in hashNodes)
            {
                GraphNode node = entry.Key as GraphNode;
                if (node.MetaObject == mbase)
                {
                    return entry.Value as TreeNode;
                }            
            }
            return null;

        }
        private void graphView1_ObjectGotSelection(object sender, GoSelectionEventArgs e)
        {
            if (graphView1.Selection.Count > 0)
            {
                if (graphView1.Selection.Primary is GraphNode)
                {
                    TreeNode selectedNode = FindTreeNode(graphView1.Selection.Primary as GraphNode);
                    treeView1.SelectedNode = selectedNode;
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            GraphNode node = e.Node.Tag as GraphNode;
            graphView1.Selection.Clear();
            if (node != null)
            {
                if (graphView1.Document.Contains(node))
                    graphView1.Selection.Add(node);
            }
        }

        private void graphView1_SelectionCopied(object sender, EventArgs e)
        {
            if (graphView1.Selection.Count > 0)
            {
                GoCollectionEnumerator enumerator = graphView1.Selection.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current is GraphNode)
                    {
                        GraphNode graphnode = enumerator.Current as GraphNode;
                        if (graphnode.MetaObject!=null)
                            graphnode.MetaObject.Changed += new EventHandler(MetaObject_Changed);
                    }
                }
            }
        }

        private void graphView1_ObjectEdited(object sender, GoSelectionEventArgs e)
        {
            if (e.GoObject.ParentNode is GraphNode)
            {
                GraphNode node = e.GoObject.ParentNode as GraphNode;
                MetaObject_Changed(node.MetaObject, e);
            }
        }

        private void graphView1_ObjectGotSelection_1(object sender, GoSelectionEventArgs e)
        {
           // propertyGrid1.SelectedObject = graphView1.Selection.Primary;
            //toolStripStatusLabel1.Text = graphView1.Selection.Primary.ToString();
        }

        private void graphView1_ObjectLostSelection(object sender, GoSelectionEventArgs e)
        {
            //propertyGrid1.SelectedObject = graphView1.Selection.Primary;
           // toolStripStatusLabel1.Text = graphView1.Selection.Primary.ToString();
        }

       
      
    }
}