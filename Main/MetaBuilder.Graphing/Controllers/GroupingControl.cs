using System;
using System.Collections.Generic;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Nodes.Containers;
using MetaBuilder.Graphing.Tools;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Controllers
{
    public class GroupingControl
    {
        #region Methods (2)

        // Public Methods (2) 

        public void MakeSubGraph(GoView myView)
        {
            try
            {
                GoSelection sel = myView.Selection;
                // get the selected nodes
                GoCollection coll = new GoCollection();
                // find the common parent, if any, to find new subgraph's parent
                GoObject common = null;
                // also remember the layer
                GoLayer layer = null;
                bool first = true;
                Dictionary<GoObject, int> objects = new Dictionary<GoObject, int>();

                foreach (GoObject obj in sel)
                {
                    if (obj.ParentNode != null)
                    {
                        if ((!(obj is IGoLink)) && (!(obj is FrameLayerGroup)))
                        {
                            coll.Add(obj.ParentNode); // remembering selected nodes
                            if (!(objects.ContainsKey(obj.ParentNode)))
                                objects.Add(obj.ParentNode, 0);
                            if (first)
                            {
                                first = false;
                                common = obj.ParentNode.Parent;
                                layer = obj.ParentNode.Layer;
                            }
                            else
                            {
                                common = GoObject.FindCommonParent(common, obj);
                                if (obj.Layer != layer)
                                {
                                    //MessageBox.Show(this,"Can't mix layers of objects to be grouped");
                                    return;
                                }
                            }
                        }
                    }
                }
                List<LinkPortSpec> linkPortSpecs = GetLinkSpecs(objects);
                if (coll.IsEmpty)
                {
                    //MessageBox.Show(this,"Need to select some nodes");
                }
                else
                {
                    myView.StartTransaction();
                    // determine the links that also should be grouped along with those nodes
                    SubGraphDraggingTool tool = new SubGraphDraggingTool(myView);
                    // (SubGraphDraggingTool)myView.FindMouseTool(typeof(SubGraphDraggingTool));
                    GoSelection whole = tool.ComputeEffectiveSelection(coll, true);
                    // create a subgraph and add it to the document
                    GoSubGraph sg = new TreeSubGraph();
                    sg.Text = "New Group"; //.ToString();
                    sg.Position = whole.Primary.Position;
                    // if there is a common parent, and it is a GoSubGraph, add the new one there
                    if (common is GoSubGraph)
                    {
                        ((GoSubGraph)common).Add(sg);
                    }
                    else
                    {
                        // otherwise just add as a top-level object
                        myView.Document.Add(sg);
                    }
                    // move the objects into the subgraph, and
                    // make sure all links belong to the proper subgraph or as a top-level link
                    sg.AddCollection(whole, true);
                    foreach (LinkPortSpec spec in linkPortSpecs)
                    {
                        spec.Relink();
                        spec.AddFishLinks(myView.Document);
                        //myView.Document.Add(spec.Link);
                    }
                    // afterwards, make the new subgraph the only selected object
                    sel.Select(sg);
                    myView.FinishTransaction("grouped selection");
                }
            }
            catch
            {
            }
        }

        public static void UngroupSubGraph(TreeSubGraph sg, GraphView myView)
        {
            #region Subgraph Specific

            myView.StartTransaction();
            // determine the subgraph children to orphan
            sg.Expand();
            GoCollection coll = new GoCollection();
            Dictionary<GoObject, int> objects = new Dictionary<GoObject, int>();
            foreach (GoObject obj in sg)
            {
                if (obj == sg.Handle || obj == sg.Label || obj == sg.Port)
                    continue; // skip the Handle and the Label
                GoLink templink = obj as GoLink;
                if ((templink == null) || !(templink.ToNode is GoSubGraph) &&
                    !(templink.FromNode is GoSubGraph))
                {
                    coll.Add(obj);
                    if (!objects.ContainsKey(obj))
                        objects.Add(obj, 0);
                }
            }
            List<LinkPortSpec> linkSpecs = GetLinkSpecs(objects);
            // move (re-parent) those objects to the subgraph's parent, or layer if top-level;
            // make sure all links belong to the proper subgraph or as a top-level link.
            if (sg.Parent is GoSubGraph)
            {
                GoSubGraph parent = (GoSubGraph)sg.Parent;
                parent.AddCollection(coll, true);
            }
            else
            {
                GoLayer layer = sg.Layer;
                layer.AddCollection(coll, true);
                // for top-level links, make sure they belong to the document's LinksLayer
                GoLayer linkslayer = layer.Document.LinksLayer;
                foreach (GoObject obj in coll)
                {
                    IGoLink link = obj as IGoLink;
                    if (link != null && link.GoObject.Layer != linkslayer)
                    {
                        linkslayer.Add(link.GoObject);
                    }
                }
            }
            // now we can delete the old subgraph
            sg.Remove();
            // change the selection to include the former children

            foreach (LinkPortSpec lps in linkSpecs)
            {
                lps.Relink();
                if (lps.Link is FishLink)
                    myView.Document.Add(lps.Link as FishLink);


                if (lps.Link is QLink)
                    myView.Document.Add(lps.Link as QLink);
                lps.AddFishLinks(myView.Document);
            }

            myView.FinishTransaction("ungrouped subgraph");
            #endregion
        }

        public static void UngroupSubGraph(SubgraphNode sg, GraphView myView)
        {
            #region Subgraph Specific

            myView.StartTransaction();
            // determine the subgraph children to orphan
            sg.Expand();
            GoCollection coll = new GoCollection();
            Dictionary<GoObject, int> objects = new Dictionary<GoObject, int>();
            foreach (GoObject obj in sg)
            {
                if (obj == sg.Handle || obj == sg.Label || obj == sg.Port)
                    continue; // skip the Handle and the Label
                GoLink templink = obj as GoLink;
                if ((templink == null) || !(templink.ToNode is GoSubGraph) &&
                    !(templink.FromNode is GoSubGraph))
                {
                    coll.Add(obj);
                    if (!(objects.ContainsKey(obj)))
                        objects.Add(obj, 0);
                }
            }
            List<LinkPortSpec> linkSpecs = GetLinkSpecs(objects);
            // move (re-parent) those objects to the subgraph's parent, or layer if top-level;
            // make sure all links belong to the proper subgraph or as a top-level link.
            if (sg.Parent is GoSubGraph)
            {
                GoSubGraph parent = (GoSubGraph)sg.Parent;
                parent.AddCollection(coll, true);
            }
            else
            {
                GoLayer layer = sg.Layer;
                layer.AddCollection(coll, true);
                // for top-level links, make sure they belong to the document's LinksLayer
                GoLayer linkslayer = layer.Document.LinksLayer;
                foreach (GoObject obj in coll)
                {
                    IGoLink link = obj as IGoLink;
                    if (link != null && link.GoObject.Layer != linkslayer)
                    {
                        linkslayer.Add(link.GoObject);
                    }
                }
            }
            // now we can delete the old subgraph
            sg.Remove();
            // change the selection to include the former children

            foreach (LinkPortSpec lps in linkSpecs)
            {
                lps.Relink();
                if (lps.Link is QLink)
                    myView.Document.Add(lps.Link as QLink);
                if (lps.Link is FishLink)
                    myView.Document.Add(lps.Link as FishLink);
                lps.AddFishLinks(myView.Document);
            }

            myView.FinishTransaction("ungrouped subgraph");
            #endregion
        }

        #endregion Methods

        #region Grouping

        // make an group out of the current selection
        public static void GroupAll(GoView view)
        {
            view.StartTransaction();
            view.SelectAll();
            GroupCurrentSelection(view);
            view.FinishTransaction("Group All");
        }

        // make an group out of the current selection
        public static void GroupSelection(GoView view)
        {
            view.StartTransaction();
            GroupCurrentSelection(view);
            view.FinishTransaction("Group");
        }

        private static void GroupCurrentSelection(GoView view)
        {
            IGoCollection col = view.Selection;

            ShapeGroup g = CreateGroup(col, view);

            // update the selection too, for the user's convenience
            view.Selection.Clear();
            view.Selection.Add(g);
        }

        public static ShapeGroup CreateGroup(IGoCollection col, GoView view)
        {
            ShapeGroup g = new ShapeGroup();
            Dictionary<GoObject, int> objects = new Dictionary<GoObject, int>();
            // NodePort,Link
            foreach (GoObject o in col)
            {
                if ((!(o is IGoLink)) && (!(o is FrameLayerGroup)))
                {
                    if (o is GoNode)
                    {
                        if (!(objects.ContainsKey(o)))
                            objects.Add(o, 0);
                    }
                    else
                    {
                        if (o.ParentNode != null)
                            if (!(objects.ContainsKey(o.ParentNode)))
                                objects.Add(o.ParentNode, 0);
                        //objects.Add(o.ParentNode);
                    }
                }
            }
            List<LinkPortSpec> linkPortSpecs = GetLinkSpecs(objects);
            foreach (KeyValuePair<GoObject, int> entry in objects)
            {
                if ((!(entry.Key is IGoLink)) && (!(entry.Key is FrameLayerGroup)))
                {
                    GoObject possibleAnchorForRationale = null;
                    if (entry.Key is MetaBuilder.Graphing.Shapes.Nodes.Rationale)
                    {
                        MetaBuilder.Graphing.Shapes.Nodes.Rationale rat = entry.Key as MetaBuilder.Graphing.Shapes.Nodes.Rationale;
                        possibleAnchorForRationale = rat.Anchor;
                    }

                    entry.Key.Remove();
                    g.Add(entry.Key);
                    entry.Key.DragsNode = true;

                    if (entry.Key is MetaBuilder.Graphing.Shapes.Nodes.Rationale)
                    {
                        MetaBuilder.Graphing.Shapes.Nodes.Rationale rat = entry.Key as MetaBuilder.Graphing.Shapes.Nodes.Rationale;
                        rat.Anchor = possibleAnchorForRationale;
                    }
                }
            }
            g.Movable = true;
            view.Document.Add(g);

            //relink!
            foreach (LinkPortSpec lps in linkPortSpecs)
            {
                lps.Relink();
                if (lps.Link is FishLink)
                    if (!(view.Document.Contains(lps.Link as GoObject)))
                        view.Document.Add(lps.Link as FishLink);
                if (lps.Link is QLink)
                    if (!(view.Document.Contains(lps.Link as GoObject)))
                        view.Document.Add(lps.Link as QLink);
                lps.AddFishLinks(view.Document);
            }
            return g;
        }

        public static List<LinkPortSpec> GetLinkSpecs(Dictionary<GoObject, int> objects)
        {
            List<LinkPortSpec> retval = new List<LinkPortSpec>();
            foreach (KeyValuePair<GoObject, int> entry in objects)
            {
                GoObject o = entry.Key;
                if (o is GoNode)
                {
                    GoNode node = o as GoNode;
                    GoNodeLinkEnumerator nodeLinkEnum = node.DestinationLinks.GetEnumerator();
                    if (node is GraphNode)
                    {
                        GraphNode gnod = node as GraphNode;
                        object prop = gnod.MetaObject.Get("Name");
                        if (prop != null)
                            if (prop.ToString() == "Employee")
                            {
                                // BREAK HERE
                                Console.Write("Found it");
                            }
                    }
                    while (nodeLinkEnum.MoveNext())
                    {
                        //  if (!objects.Contains(nodeLinkEnum.Current as GoObject))
                        {
                            LinkPortSpec lpspec = new LinkPortSpec(nodeLinkEnum.Current, node);
                            retval.Add(lpspec);
                        }
                    }
                    GoNodeLinkEnumerator nodeLinkEnumSrc = node.SourceLinks.GetEnumerator();
                    while (nodeLinkEnumSrc.MoveNext())
                    {
                        //  if (!objects.Contains(nodeLinkEnumSrc.Current as GoObject))
                        {
                            LinkPortSpec lpspec2 = new LinkPortSpec(nodeLinkEnumSrc.Current, node);
                            retval.Add(lpspec2);
                        }
                    }
                }



            }
            return retval;
        }

        public static Dictionary<ResizableBalloonComment, GoObject> GetRationaleSpecs(GoCollection coll)
        {
            Dictionary<ResizableBalloonComment, GoObject> retval = new Dictionary<ResizableBalloonComment, GoObject>();

            foreach (GoObject o in coll)
            {
                if (o is ResizableBalloonComment)
                {
                    ResizableBalloonComment r = o as ResizableBalloonComment;
                    r.DropID = Guid.NewGuid();
                    r.DragsNode = false;
                    if (!retval.ContainsKey(r))
                        retval.Add(r, r.Anchor);
                }


                foreach (GoObject obs in o.Observers)
                {
                    if (obs is ResizableBalloonComment)
                    {
                        ResizableBalloonComment r = obs as ResizableBalloonComment;
                        r.DropID = Guid.NewGuid();
                        r.DragsNode = false;
                        if (!retval.ContainsKey(r))
                            retval.Add(r, r.Anchor);
                    }
                }
            }
            return retval;
        }

        public static List<LinkPortSpec> GetLinkSpecs(GoCollection coll)
        {
            return GetLinkSpecs(coll.CopyArray());
        }

        public static List<LinkPortSpec> GetLinkSpecs(GoObject[] coll)
        {
            List<LinkPortSpec> retval = new List<LinkPortSpec>();
            for (int i = 0; i < coll.Length; i++)
            {
                GoObject o = coll[i];
                if (o is GoNode)
                {
                    GoNode node = o as GoNode;
                    GoNodeLinkEnumerator nodeLinkEnum = node.DestinationLinks.GetEnumerator();
                    if (node is GraphNode)
                    {
                        GraphNode gnod = node as GraphNode;
                        object prop = gnod.MetaObject.Get("Name");
                        if (prop != null)
                            if (prop.ToString() == "Employee")
                            {
                                // BREAK HERE
                                Console.Write("Found it");
                            }
                    }
                    while (nodeLinkEnum.MoveNext())
                    {
                        //  if (!objects.Contains(nodeLinkEnum.Current as GoObject))
                        {
                            LinkPortSpec lpspec = new LinkPortSpec(nodeLinkEnum.Current, node);
                            retval.Add(lpspec);
                        }
                    }
                    GoNodeLinkEnumerator nodeLinkEnumSrc = node.SourceLinks.GetEnumerator();
                    while (nodeLinkEnumSrc.MoveNext())
                    {
                        //  if (!objects.Contains(nodeLinkEnumSrc.Current as GoObject))
                        {
                            LinkPortSpec lpspec2 = new LinkPortSpec(nodeLinkEnumSrc.Current, node);
                            retval.Add(lpspec2);
                        }
                    }
                }
            }
            return retval;
        }

        // this action only works on the primary selection, which should be a group
        public static void UngroupSelection(GoView view)
        {
            view.StartTransaction();
            GoCollectionEnumerator selection = view.Selection.GetEnumerator();
            List<GoObject> selectedobjects = new List<GoObject>();
            while (selection.MoveNext())
            {
                selectedobjects.Add(selection.Current);
            }

            foreach (GoObject selobj in selectedobjects)
            {
                if (selobj is SubgraphNode)
                {
                    UngroupSubGraph(selobj as SubgraphNode, view as GraphView);
                }
                else if (selobj is TreeSubGraph)
                {
                    UngroupSubGraph(selobj as TreeSubGraph, view as GraphView);
                }
                else if (selobj is ShapeGroup || selobj.ParentNode is ShapeGroup)
                {
                    Dictionary<GoObject, int> objects = new Dictionary<GoObject, int>();
                    ShapeGroup group = selobj as ShapeGroup;
                    if (group == null)
                        group = selobj.ParentNode as ShapeGroup;

                    foreach (GoObject o in group)
                    {
                        objects.Add(o, 0);
                    }
                    List<LinkPortSpec> linkPortSpecs = GetLinkSpecs(objects);
                    foreach (KeyValuePair<GoObject, int> kvp in objects)
                    {
                        GoObject o = kvp.Key;
                        // first add all the non-links
                        if (!(o is QLink))
                        {
                            GoObject possibleAnchor = null;
                            if (o is Shapes.Nodes.Rationale)
                            {
                                Shapes.Nodes.Rationale rat = o as Shapes.Nodes.Rationale;
                                possibleAnchor = rat.Anchor;
                            }

                            group.Remove(o);
                            o.DragsNode = false;
                            try
                            {
                                view.Document.Add(o);
                                if (o is Shapes.Nodes.Rationale)
                                {
                                    Shapes.Nodes.Rationale rat = o as Shapes.Nodes.Rationale;
                                    rat.Anchor = possibleAnchor;
                                }
                            }
                            catch
                            {
                            }
                            o.Deletable = true;
                        }
                    }
                    foreach (KeyValuePair<GoObject, int> kvp in objects)
                    {
                        GoObject o = kvp.Key;
                        // now we know the nodes are on the diagram and the links can be safely added
                        if (o is QLink || o is FishLink)
                        {
                            group.Remove(o);
                            o.DragsNode = false;
                            view.Document.Add(o);
                        }
                    }
                    foreach (LinkPortSpec lps in linkPortSpecs)
                    {
                        lps.Relink();
                        if (lps.Link is QLink)
                            view.Document.Add(lps.Link as QLink);
                        if (lps.Link is FishLink)
                            view.Document.Add(lps.Link as FishLink);
                        lps.AddFishLinks(view.Document);
                    }
                    view.Document.Remove(group);
                }
            }
            view.FinishTransaction("Ungroup");
        }

        // this action works on any selection - until only GoObjects are left
        public static void UngroupAll(GoView view)
        {
            view.StartTransaction();
            GoObject first = view.Selection.Primary;
            if (first is GoGroup)
            {
                foreach (GoGroup obj in view.Selection)
                {
                    UngroupCurrentSelection(view, obj);
                }
            }
            view.FinishTransaction("Ungroup All");
        }

        private static void UngroupCurrentSelection(GoView view, GoGroup group)
        {
            GoLayer layer = group.Layer;
            // iterate through the group's objects and move them from
            // the group to the document
            Dictionary<GoObject, int> objects = new Dictionary<GoObject, int>();
            GoObject[] objs = group.CopyArray();
            foreach (GoObject obj in objs)
            {
                objects.Add(obj, 0);
            }
            List<LinkPortSpec> linkPortSpecs = GetLinkSpecs(objects);
            foreach (GoObject obj in objs)
            {
                group.Remove(obj);
                layer.Add(obj);
                // update the selection too, for the user's convenience
                obj.Selectable = true;
            }
            foreach (LinkPortSpec lps in linkPortSpecs)
            {
                lps.Relink();

                if (lps.Link is QLink)
                    view.Document.Add(lps.Link as QLink);
                lps.AddFishLinks(view.Document);
            }
            // delete the group
            layer.Remove(group);
        }

        #endregion
    }

    public class LinkPortSpec
    {
        #region Fields (6)

        private readonly List<FishLink> fishLinks;
        private readonly int fromPortIndex;
        private readonly IGoLink link;
        private readonly GoNode node;
        private readonly int toPortIndex;
        private QLink QLink;

        #endregion Fields

        #region Constructors (1)

        public LinkPortSpec(IGoLink link, GoNode node)
        {
            fishLinks = new List<FishLink>();
            this.link = link;
            this.node = node;
            if (link is QLink)
            {
                QLink slink = link as QLink;
                if (slink.MidLabel != null)
                {
                    GoGroup grp = slink.MidLabel as GoGroup;
                    foreach (GoObject o in grp)
                    {
                        if (o is FishLinkPort)
                        {
                            FishLinkPort port = o as FishLinkPort;
                            GoPortFilteredLinkEnumerator portLinkEnum = port.SourceLinks.GetEnumerator();
                            while (portLinkEnum.MoveNext())
                            {
                                if (portLinkEnum.Current is FishLink)
                                {
                                    fishLinks.Add(portLinkEnum.Current as FishLink);
                                }
                            }
                        }
                    }
                }
            }
            if (link is FishLink)
            {
                FishLink flink = link as FishLink;
                if (flink.ToPort is FishLinkPort)
                {
                    FishLinkPort prtTo = flink.ToPort as FishLinkPort;
                    QLink = prtTo.Parent.Parent as QLink;
                    FishNodePort prtFrom = flink.FromPort as FishNodePort;

                    GoNodeLinkEnumerator linkEnum = node.SourceLinks.GetEnumerator();
                    StoreLinkPortIndex(node, link, linkEnum, prtFrom, ref fromPortIndex);
                    linkEnum = node.DestinationLinks.GetEnumerator();
                    StoreLinkPortIndex(node, link, linkEnum, prtTo, ref toPortIndex);
                }
            }
            else
            {
                GoNodeLinkEnumerator linkEnum = node.DestinationLinks.GetEnumerator();
                StoreLinkPortIndex(node, link, linkEnum, link.FromPort as GoPort, ref fromPortIndex);
                linkEnum = node.SourceLinks.GetEnumerator();
                StoreLinkPortIndex(node, link, linkEnum, link.ToPort as GoPort, ref toPortIndex);
            }
        }

        #endregion Constructors

        #region Properties (1)

        public IGoLink Link
        {
            get { return link; }
        }

        #endregion Properties

        #region Methods (4)

        // Public Methods (2) 

        public void AddFishLinks(GoDocument doc)
        {
            foreach (FishLink flink in fishLinks)
            {
                if (flink.IsInDocument)
                    flink.Remove();
                doc.Add(flink);
            }
        }

        public void Relink()
        {
            if (link is QLink)
            {
                if (fromPortIndex > 0)
                {
                    DoRelink(link, fromPortIndex, node, true);
                }
                else
                {
                    DoRelink(link, toPortIndex, node, false);
                }
            }
            else
            {
                DoRelink(link, 0, node, true);
                try
                {
                    /*   if (!(link.FromPort is FishLinkPort))
                    {
                       // Turn it around
                        IGoPort prt = link.FromPort;
                        IGoPort prtTo = link.ToPort;
                        link.FromPort = prtTo;
                        link.ToPort = prt;
                    }*/
                }
                catch
                {
                }
            }
        }


        // Private Methods (2) 

        private void DoRelink(IGoLink link, int portIndex, GoNode node, bool isFrom)
        {
            // find the port
            GoPort prt = null;
            GoNodePortEnumerator portEnum = node.Ports.GetEnumerator();
            int counter = 0;
            while (portEnum.MoveNext())
            {
                if (counter == portIndex)
                {
                    prt = portEnum.Current as GoPort;
                    break;
                }
                counter++;
            }
            if (prt != null)
            {
                if (isFrom)
                {
                    if (link.FromPort == null)
                        link.FromPort = prt;
                }
                else
                {
                    if (link.ToPort == null)
                        link.ToPort = prt;
                }
            }

            GoLink lnk = link as GoLink;
            if (lnk != null)
                lnk.CalculateRoute();
        }

        private void StoreLinkPortIndex(GoNode node, IGoLink link, GoNodeLinkEnumerator linkEnum, GoPort port,
                                        ref int storeInThisVariable)
        {
            while (linkEnum.MoveNext())
            {
                if (linkEnum.Current == link)
                {
                    int counter = 0;
                    GoNodePortEnumerator portEnum = node.Ports.GetEnumerator();
                    while (portEnum.MoveNext())
                    {
                        if (portEnum.Current == port)
                        {
                            storeInThisVariable = counter;
                        }
                        counter++;
                    }
                }
            }
        }

        #endregion Methods
    }
}