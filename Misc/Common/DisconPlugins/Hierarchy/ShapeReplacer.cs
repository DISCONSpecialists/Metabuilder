using System.Collections.Generic;
using System.Windows.Forms;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Tools;
using MetaBuilder.Meta;
using Northwoods.Go;

namespace DisconPlugins.Hierarchy
{
    public class ShapeReplacer
    {

		#region Fields (2) 

        private GraphView myView;
        private GraphPalette palette;

		#endregion Fields 

		#region Properties (2) 

        public GraphView MyView
        {
            get { return myView; }
            set { myView = value; }
        }

        public GraphPalette Palette
        {
            get { return palette; }
            set { palette = value; }
        }

		#endregion Properties 

		#region Methods (1) 


		// Public Methods (1) 

        public void Execute()
        {
            if (Palette != null)
            {
                if (Palette.Selection.Count == 1)
                {
                    myView.StartTransaction();
                    GraphNode newnode = palette.Selection.Primary.ParentNode as GraphNode;

                    List<GraphNode> nodesToDelete = new List<GraphNode>();
                    GoLayerCollectionObjectEnumerator nodeEnum = MyView.Document.GetEnumerator();
                    List<GraphNode> oldNodes = new List<GraphNode>();
                    while (nodeEnum.MoveNext())
                    {
                        if (nodeEnum.Current is GraphNode)
                        {
                            oldNodes.Add(nodeEnum.Current as GraphNode);
                        }
                    }
                    Dictionary<QLink, GoPort> destPorts = new Dictionary<QLink, GoPort>();
                    Dictionary<QLink, GoPort> sourcePorts = new Dictionary<QLink, GoPort>();
                    foreach (GraphNode node in oldNodes)
                    {
                        GraphNode newCopy = null;
                        if (node.BindingInfo.BindingClass != newnode.BindingInfo.BindingClass)
                        {
                            newCopy = newnode.Copy() as GraphNode;
                            newCopy.Position = node.Position;
                            MetaBase newObj = Loader.CreateInstance(newnode.BindingInfo.BindingClass);
                            newCopy.MetaObject = newObj;
                            newObj.Set("Name",node.MetaObject.Get("Name").ToString());
                            newCopy.BindToMetaObjectProperties();

                            myView.Document.Add(newCopy);
                            nodesToDelete.Add(node);

                            // leave the to port as is...
                            GoNodeLinkEnumerator outgoingLinks = node.DestinationLinks;
                            while (outgoingLinks.MoveNext())
                            {
                                GoPort fromPort = outgoingLinks.Current.FromPort as GoPort;
                                GoCollection col = new GoCollection();
                                myView.PickObjectsInRectangle(true, false, fromPort.Bounds, GoPickInRectangleStyle.AnyIntersectsBounds, col, 15);
                                bool found = false;
                                if (col.Count > 0)
                                {
                                    GoCollectionEnumerator colEnum = col.GetEnumerator();
                                    while (colEnum.MoveNext())
                                    {
                                        if (colEnum.Current is GoPort && colEnum.Current.ParentNode == newCopy)
                                        {
                                            destPorts.Add(outgoingLinks.Current as QLink, (colEnum.Current as GoPort));
                                            //destLinks.Current.FromPort = (colEnum.Current as GoPort);
                                            found = true;
                                        }
                                    }
                                }
                                if (!found)
                                    destPorts.Add(outgoingLinks.Current as QLink, newCopy.GetDefaultPort);

                            }

                            GoNodeLinkEnumerator incomingLinks = node.SourceLinks;
                            while (incomingLinks.MoveNext())
                            {
                                GoPort ToPort = incomingLinks.Current.ToPort as GoPort;
                                GoCollection col2 = new GoCollection();
                                myView.PickObjectsInRectangle(true, false, ToPort.Bounds, GoPickInRectangleStyle.AnyIntersectsBounds, col2, 15);
                                bool found2 = false;
                                if (col2.Count > 0)
                                {
                                    GoCollectionEnumerator colEnum2 = col2.GetEnumerator();
                                    while (colEnum2.MoveNext())
                                    {
                                        if (colEnum2.Current is GoPort && colEnum2.Current.ParentNode == newCopy)
                                        {
                                            sourcePorts.Add(incomingLinks.Current as QLink, (colEnum2.Current as GoPort));
                                            found2 = true;
                                        }
                                    }
                                }
                                if (!found2)
                                    sourcePorts.Add(incomingLinks.Current as QLink, newCopy.GetDefaultPort);

                            }

                        }
                    }

                    foreach (KeyValuePair<QLink, GoPort> destPortsKV in destPorts)
                    {
                        destPortsKV.Key.FromPort = destPortsKV.Value;
                    }
                    foreach (KeyValuePair<QLink, GoPort> srcPortsKV in sourcePorts)
                    {
                        srcPortsKV.Key.ToPort = srcPortsKV.Value;
                    }
                    for (int i = 0; i < nodesToDelete.Count; i++)
                    {
                        nodesToDelete[i].Remove();
                    }
                    AutoRelinkTool relinkTool = new AutoRelinkTool();
                    GoCollection collection = new GoCollection();
                    collection.AddRange(myView.Document);
                    relinkTool.RelinkCollection(collection);

                    myView.FinishTransaction("Replace Shapes");
                }
                else
                {
                    MessageBox.Show("First select a shape on the stencil, and the shapes to replace on the diagram");
                }

            }
        }


		#endregion Methods 

    }
}
