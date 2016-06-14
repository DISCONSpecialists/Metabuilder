using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MetaBuilder.Graphing;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Nodes;
using MetaBuilder.Meta;

using MetaBuilder.Core.Storage;
using MetaBuilder.Core;
using MetaBuilder.Graphing.Persistence;
using MetaBuilder.BusinessLogic;

using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Controllers;
using MetaBuilder.Graphing.Tools;
using Northwoods.Go.Layout;
using Northwoods.Go;
namespace ShapeBuilding.FSDLayout
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadFSD();

        }

        private void goDrawView1_SelectionFinished(object sender, EventArgs e)
        {
            if (goDrawView1.Selection.Primary != null)
                propertyGrid1.SelectedObject = goDrawView1.Selection.Primary;
        }
        private GraphViewController viewController = null;
        public GraphViewController ViewController
        {
            get
            {
                if (viewController == null)
                {
                    viewController = new GraphViewController();
                }
                return viewController;
            }
            set { viewController = value; }
        }
        private void LoadFSD()
        {
            string filename = @"Z:\05. Sandbox\Today\FSD Layout Test.mdgm";
            /*
                        GraphViewContainer gvContainer = new GraphViewContainer(applicableSpec.FileType);
                        gvContainer.View.BeginUpdate();
                        gvContainer.LoadFile(filename);
                        gvContainer.View.EndUpdate();*/

            FileTypeList ftype = FileTypeList.Diagram;
            string ext = strings.GetFileExtension(filename);
            FileDialogSpecification spec = FilePathManager.Instance.GetSpecification(ftype);
            FileUtil futil = new FileUtil();
            object loadedObject = futil.Open(filename);
            NormalDiagram ndiagram = loadedObject as NormalDiagram;
            this.goDrawView1.Document = ndiagram;
            DocumentController docController = new DocumentController(ndiagram);
            docController.ApplyBrushes();
            ndiagram.SerializesUndoManager = false;
            ndiagram.RepositionFrame(goDrawView1);

            GoDocument UseThisDocument = goDrawView1.Document;

            //DisconLayoutTree layout = new DisconLayoutTree();
            CustomLayoutTree layout = new CustomLayoutTree();
            layout.Document = UseThisDocument;

            layout.Style = GoLayoutTreeStyle.LastParents;
            layout.Angle = 90f;
            layout.Alignment = GoLayoutTreeAlignment.CenterSubtrees;
            layout.LayerSpacing = 50f;
            layout.Compaction = GoLayoutTreeCompaction.None;
            layout.AlternateDefaults.Angle = 0;
            layout.AlternateDefaults.Alignment = GoLayoutTreeAlignment.Start;
            layout.AlternateDefaults.Compaction = GoLayoutTreeCompaction.None;
            layout.AlternateDefaults.NodeIndent = 10;
            layout.NodeIndent = 10;
            goDrawView1.SuspendLayout();
            layout.PerformLayout();
            layout.LayoutNodesAndLinks();
            layout.DoLink();
            goDrawView1.ResumeLayout();
        }

        public class CustomLayoutTree : GoLayoutTree
        {



            protected override void AssignTreeNodeValues(GoLayoutTreeNode n)
            {
                base.AssignTreeNodeValues(n);
            }

            public void DoLink()
            {
                GoLayoutTreeNetwork netw = this.Network;
                foreach (GoLayoutTreeNode n in netw.Nodes)
                {
                    Relink(n);
                }
            }
            public void Relink(GoLayoutTreeNode n)
            {


                if (n.GoObject is GraphNode)
                {
                    GraphNode gnode = n.GoObject as GraphNode;
                    IGoPort prtFrom = null;
                    IGoPort prtFromAux = null;
                    IGoPort prtTo = null;

                    if (n.Level <= 1)
                    {
                        prtFrom = GetPort(gnode, GoObject.BottomCenter);
                        prtFromAux = GetPort(gnode, GoObject.MiddleRight);
                        prtTo = GetPort(gnode, GoObject.TopCenter);
                    }
                    else
                    {
                        prtFrom = GetPort(gnode, GoObject.BottomCenter);
                        prtFromAux = GetPort(gnode, GoObject.MiddleRight);
                        prtTo = GetPort(gnode, GoObject.MiddleLeft);
                    }


                    foreach (QLink lnk in gnode.DestinationLinks)
                    {
                        if (lnk.AssociationType != LinkAssociationType.Auxiliary)
                            lnk.FromPort = prtFrom;
                        else
                            lnk.FromPort = prtFromAux;
                    }

                    foreach (QLink lnk in gnode.SourceLinks)
                    {
                        lnk.ToPort = prtTo;
                    }

                }
            }
            protected override void LayoutTree(GoLayoutTreeNode n)
            {


                base.LayoutTree(n);
            }



            private IGoPort GetPort(GraphNode n, int location)
            {
                GoCollection colItems = new GoCollection();
                PointF pf = n.GetSpotLocation(location);
                n.PickObjects(pf, false, colItems, 100);
                GoPort prt = null;
                foreach (GoObject o in colItems)
                {
                    if (o is GoPort)
                    {
                        prt = o as GoPort;
                    }
                }

                if (prt == null)
                {
                    // cant find a port
                    while (prt == null)
                    {
                        pf.Y = pf.Y - 1;
                        n.PickObjects(pf, false, colItems, 100);
                        foreach (GoObject o in colItems)
                        {
                            if (o is GoPort)
                            {
                                prt = o as GoPort;
                            }
                        }

                    }

                }
                return prt;

            }
        }


    }
}