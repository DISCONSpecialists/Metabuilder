using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Graphing.Shapes;
using Northwoods;
using Northwoods.Go;
using Northwoods.Go.Draw;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Meta;
using System.Collections.ObjectModel;

namespace MetaBuilder.Graphing.Tools
{
    public enum MetaConvertInfo
    {
        Initialise = 0,
        OK = 1,
        CannotRelink = 2,
        CannotReArtefact = 3,
    }

    public class MetaConvert
    {
        #region Common Helpers Static

        public static IGoCollection StaticCollection;
        public static GraphNode CoreObject;

        public static Dictionary<GoObject, GoObject> originalVSreplaced;
        public static GoObject returnNewObjectUsingOldAsKey(GoObject original)
        {
            GoObject ret = original;
            if (originalVSreplaced.ContainsKey(original))
                originalVSreplaced.TryGetValue(original, out ret);

            return ret;
        }

        //returns the first intersection of a port on any port on newnode, null if not found
        public static List<GoObject> getAllAchoredToObject(GoDocument doc, GoObject parent)
        {
            List<GoObject> ret = new List<GoObject>();

            foreach (GoObject o in doc)
            {
                if (!(o is GoBalloon))
                    continue;

                if ((o as GoBalloon).Anchor == parent)
                    ret.Add(o);
            }

            return ret;
        }
        public static IGoPort findPortAtPositionOfOldPortForNewNode(GoPort oldPort, GraphNode newNode)
        {
            if (oldPort == null)
                return null;

            if (oldPort.ParentNode is Shapes.Nodes.Containers.SubgraphNode)
                return (oldPort.ParentNode as Shapes.Nodes.Containers.SubgraphNode).Port as IGoPort;

            foreach (GoObject o in newNode)
            {
                if (o is QuickPort)
                {
                    GoPort p = o as GoPort;

                    //inflation to increase accuracy
                    if (p.Bounds.IntersectsWith(oldPort.Bounds))
                    {
                        return p as IGoPort;
                    }
                }
            }

            return newNode.GetDefaultPort as IGoPort;
        }

        private static Dictionary<MetaBase, MetaBase> OldToNewMetaObjects = new Dictionary<MetaBase, MetaBase>();
        public static GraphNode ConvertObject(GraphNode gNode, GraphNode coreObject, bool ConvertWithMetaObject)
        {
            GraphNode newNodeObject = coreObject.Copy() as GraphNode;
            //newNodeObject.Bounds = gNode.Bounds;
            if (gNode.Height == newNodeObject.Height)
            {
                //newNodeObject.Position = gNode.Position;
                newNodeObject.Location = gNode.Location;
            }
            else
            {
                newNodeObject.Left = gNode.Left;
                newNodeObject.Bottom = gNode.Bottom;
            }
            SwapProperties(gNode, newNodeObject);

            if (ConvertWithMetaObject)
            {
                #region Keep diagram allocation
                Collection<AllocationHandle.AllocationItem> items = null;
                foreach (GoObject o in gNode)
                    if (o is AllocationHandle)
                    {
                        items = (o as AllocationHandle).Items;
                        break;
                    }

                foreach (GoObject o in newNodeObject)
                    if (o is AllocationHandle)
                    {
                        (o as AllocationHandle).Items = items;
                        break;
                    }
                #endregion

                newNodeObject.MetaObject = gNode.MetaObject;
                newNodeObject.Shadowed = gNode.Shadowed;
                if (newNodeObject.MetaObject.Class == "SubsetIndicator")
                {
                    if (newNodeObject.MetaObject.Get("SubsetIndicatorType") == null || string.IsNullOrEmpty(newNodeObject.MetaObject.Get("SubsetIndicatorType").ToString()))
                        newNodeObject.MetaObject.Set("SubsetIndicatorType", "XOR");
                    newNodeObject.BindToMetaObjectProperties();
                }
            }
            if (ConvertShallowsToSameMetabase)
            {
                //if OldToNewMetaObjects contains gnode::metaobject
                if (OldToNewMetaObjects.ContainsKey(gNode.MetaObject))
                {
                    MetaBase usedBased = null;
                    OldToNewMetaObjects.TryGetValue(gNode.MetaObject, out usedBased);
                    if (usedBased != null)
                    {
                        newNodeObject.MetaObject = usedBased;
                        newNodeObject.Shadowed = gNode.Shadowed;
                    }
                }
                else
                {
                    //if OldToNewMetaObjects containts metaobject
                    OldToNewMetaObjects.Add(gNode.MetaObject, newNodeObject.MetaObject);
                }
            }
            newNodeObject.HookupEvents();
            newNodeObject.BindToMetaObjectProperties();

            return newNodeObject;
        }

        private static GraphNode SwapProperties(GraphNode originalNode, GraphNode newNode)
        {
            newNode.MetaObject.IgnoreRemapping = true;
            //set each property from orignial to new
            foreach (System.Reflection.PropertyInfo prop in originalNode.MetaObject.GetMetaPropertyList(true))
            {
                //get value of the property currently
                object currentValue = prop.GetValue(originalNode.MetaObject, null);
                if (currentValue != null)
                    newNode.MetaObject.SetWithoutChange(prop.Name, currentValue.ToString());
            }
            if (newNode.MetaObject.Class == "SubsetIndicator")
            {
                if (newNode.MetaObject.Get("SubsetIndicatorType") == null || string.IsNullOrEmpty(newNode.MetaObject.Get("SubsetIndicatorType").ToString()))
                    newNode.MetaObject.SetWithoutChange("SubsetIndicatorType", "XOR");
                newNode.BindToMetaObjectProperties();
            }

            GoCollection pCol = new GoCollection();
            pCol.Add(originalNode);
            Formatting.FormattingManipulator forman = new Formatting.FormattingManipulator(pCol);
            GoCollection cCol = new GoCollection();
            cCol.Add(newNode);
            forman.ApplyToSelection(cCol);

            newNode.MetaObject.IgnoreRemapping = false;
            return newNode;
        }

        #endregion

        private List<MetaConvertSpec> specification;
        private IGoCollection ObjectCollection;

        public MetaConvert(IGoCollection Collection, string classToChangeTo, GraphNode coreObject)
        {
            specification = new List<MetaConvertSpec>();
            ObjectCollection = Collection;
            StaticCollection = Collection;
            CoreObject = coreObject;
            originalVSreplaced = new Dictionary<GoObject, GoObject>();
            foreach (GoObject o in ObjectCollection)
            {
                if (!(o is IMetaNode))
                    continue;

                if (o is ArtefactNode) //graphnode as artefactnode?
                    continue;

                specification.Add(new MetaConvertSpec((o as IMetaNode), classToChangeTo));
            }
        }

        private static bool convertShallowsToSameMetabase;
        public static bool ConvertShallowsToSameMetabase
        {
            get { return convertShallowsToSameMetabase; }
            set { convertShallowsToSameMetabase = value; }
        }

        public MetaConvertInfo Result
        {
            get
            {
                foreach (MetaConvertSpec spec in specification)
                    if (spec.Result != MetaConvertInfo.OK)
                        return spec.Result;

                return MetaConvertInfo.OK;
            }
        }

        public List<string> Report()
        {
            List<string> report = new List<string>();
            foreach (MetaConvertSpec spec in specification)
            {
                if (spec.Result != MetaConvertInfo.OK)
                {
                    //report on the nodes error
                    report.Add(spec.OriginalNode.MetaObject.ToString() + " : " + spec.Result.ToString());
                    //report on each of the links errors
                    foreach (MetaConvertLink lnk in spec.Report())
                    {
                        if (lnk.Result != MetaConvertInfo.OK)
                        {
                            report.Add(lnk.ToString() + " : " + lnk.Result.ToString());
                        }

                        //internal to each link is a report on each artefacts error for that link
                        foreach (MetaConvertArtefact art in lnk.Report())
                        {
                            if (art.Result != MetaConvertInfo.OK)
                            {
                                report.Add(art.ToString() + " : " + art.Result.ToString());
                            }
                        }
                    }
                }
            }
            return report;
        }

        public void Convert(GoDocument doc, bool AsShallow)
        {
            //spec by spec
            Selection = new GoCollection();
            foreach (MetaConvertSpec spec in specification)
            {
                spec.Convert(doc, AsShallow);
                Selection.Add(spec.NewNode);
            }
        }

        private GoCollection selection;
        public GoCollection Selection
        {
            get { return selection; }
            private set { selection = value; }
        }
    }

    public class MetaConvertSpec
    {
        private MetaConvertInfo result;
        public MetaConvertInfo Result
        {
            get
            {
                if (ToLinks != null)
                    foreach (MetaConvertLink tl in ToLinks)
                        if (tl.Result != MetaConvertInfo.OK)
                            return tl.Result;

                if (FromLinks != null)
                    foreach (MetaConvertLink fl in FromLinks)
                        if (fl.Result != MetaConvertInfo.OK)
                            return fl.Result;

                return result;
            }
            private set { result = value; }
        }

        private IMetaNode originalNode;
        public IMetaNode OriginalNode
        {
            get { return originalNode; }
            set { originalNode = value; }
        }

        private GraphNode newNode;
        public GraphNode NewNode
        {
            get { return newNode; }
            private set { newNode = value; }
        }

        private List<IGoLink> fishlinksToRelink;

        public MetaConvertSpec(IMetaNode node, string classToChangeTo)
        {
            Result = MetaConvertInfo.Initialise;
            OriginalNode = node;

            ToLinks = new List<MetaConvertLink>();
            foreach (GoObject o in (node as GoNode).SourceLinks)
            {
                if (o is QLink)
                {
                    if ((o as QLink).ToNode == node)
                        ToLinks.Add(new MetaConvertLink(o as QLink, false, classToChangeTo));
                }
                else// if (o is FishLink || o is FishRealLink)
                {
                    Core.Log.WriteLog("MetaConvertSpec::" + o.GetType().ToString() + " is an unknown Source Link type and cannot be checked for relinkability and therefore has been removed.");
                }
            }
            fishlinksToRelink = new List<IGoLink>();
            FromLinks = new List<MetaConvertLink>();
            foreach (GoObject o in (node as GoNode).DestinationLinks)
            {
                if (o is QLink)
                {
                    if ((o as QLink).FromNode == node)
                        FromLinks.Add(new MetaConvertLink(o as QLink, true, classToChangeTo));
                }
                else if (o is FishLink || o is FishRealLink)
                {
                    //toport is midlabel port
                    QLink qlink = (((o as IGoLink).ToPort as GoPort).Parent.Parent as QLink);
                    int CAid = -1;

                    if (qlink != null)
                        foreach (ClassAssociation clsAss in DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByParentClass((qlink.FromNode as IMetaNode).MetaObject.Class))
                            if (clsAss.AssociationTypeID == (int)qlink.AssociationType)
                                if (clsAss.ChildClass == (qlink.ToNode as IMetaNode).MetaObject.Class)
                                    if (clsAss.IsActive == true)
                                        CAid = clsAss.CAid;

                    //if allowed artifact on link
                    bool found = false;
                    if (CAid > 0)
                        foreach (AllowedArtifact art in DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.AllowedArtifactProvider.GetByCAid(CAid))
                            if ((bool)art.IsActive == true)
                                if (art.Class == classToChangeTo)
                                    found = true;

                    if (!found)
                    {
                        Result = MetaConvertInfo.CannotReArtefact;
                    }
                    else
                    {
                        //Convert will relink to this links fromport position
                        fishlinksToRelink.Add(o as IGoLink);
                    }
                }
                else// if (o is FishLink || o is FishRealLink)
                {
                    Core.Log.WriteLog("MetaConvertSpec::" + o.GetType().ToString() + " is an unknown Destination Link type and cannot be checked for relinkability and therefore has been removed.");
                }
            }
            if (Result == MetaConvertInfo.Initialise)
                Result = MetaConvertInfo.OK;
        }

        private List<MetaConvertLink> ToLinks;
        private List<MetaConvertLink> FromLinks;

        public List<MetaConvertLink> Report()
        {
            List<MetaConvertLink> ret = new List<MetaConvertLink>();
            foreach (MetaConvertLink imc in ToLinks)
            {
                if (imc.Result != MetaConvertInfo.OK)
                {
                    ret.Add(imc);
                }
            }
            foreach (MetaConvertLink imc in ToLinks)
            {
                if (imc.Result != MetaConvertInfo.OK)
                {
                    ret.Add(imc);
                }
            }
            return ret;
        }

        public void Convert(GoDocument doc, bool AsShallow)
        {
            NewNode = MetaConvert.ConvertObject(OriginalNode as GraphNode, MetaConvert.CoreObject, AsShallow);
            if (OriginalNode is CollapsibleNode && newNode is CollapsibleNode)
            {
                CollapsibleNode cNode = newNode as CollapsibleNode;
                CollapsibleNode cOrigNode = OriginalNode as CollapsibleNode;

                //shallow the repeater items into the corresponding new items if asshallow
                int rsecNumber = -1;
                foreach (RepeaterSection rsec in cOrigNode.RepeaterSections)
                {
                    rsecNumber += 1;
                    foreach (GoObject obj in rsec)
                    {
                        if (!(obj is CollapsingRecordNodeItem) || (obj as CollapsingRecordNodeItem).IsHeader)
                            continue;
                        CollapsingRecordNodeItem item = obj as CollapsingRecordNodeItem;

                        //add this item to the same repeater section
                        int rsecNodeNumber = -1;
                        RepeaterSection rsecPrevious;
                        foreach (RepeaterSection rsecNode in cNode.RepeaterSections)
                        {
                            rsecPrevious = rsecNode;
                            rsecNodeNumber += 1;

                            if (rsecNumber == rsecNodeNumber)
                            {
                                if (rsec.BindingInfo == null || rsec.GetRepeaterBindingInfo() == null)
                                {
                                    rsec.BindingInfo = cNode.BindingInfo;
                                }

                                CollapsingRecordNodeItem i = rsecNode.AddItem();
                                if (i == null)
                                {
                                    string childclass = (cNode.MetaObject.Class == "DataTable" || cNode.MetaObject.Class == "PhysicalInformationArtefact") ? "DataField" : "DataAttribute";
                                    i = rsecNode.AddChildItem(childclass, "Name");
                                }
                                if (i != null)
                                {
                                    if (AsShallow)
                                    {
                                        if (i.MetaObject.Class == item.MetaObject.Class)
                                            i.MetaObject = item.MetaObject;
                                        else
                                        {
                                            item.MetaObject.CopyPropertiesToObject(i.MetaObject);
                                            i.MetaObject.pkid = item.MetaObject.pkid;
                                            i.MetaObject.MachineName = item.MetaObject.MachineName;
                                            i.MetaObject.WorkspaceName = item.MetaObject.WorkspaceName;
                                            i.MetaObject.WorkspaceTypeId = item.MetaObject.WorkspaceTypeId;
                                        }
                                    }
                                    else
                                    {
                                        item.MetaObject.CopyPropertiesToObject(i.MetaObject);
                                    }

                                    //relink
                                    List<QLink> parentLinks = new List<QLink>();
                                    List<QLink> childLinks = new List<QLink>();
                                    foreach (GoObject complexLink in item.Links)
                                    {
                                        if (complexLink is QLink)
                                        {
                                            QLink lnk = complexLink as QLink;
                                            if (lnk.FromNode == item)
                                                parentLinks.Add(lnk);
                                            else if (lnk.ToNode == item)
                                                childLinks.Add(lnk);
                                        }
                                    }

                                    i.HookupEvents();
                                    i.BindToMetaObjectProperties();
                                    rsecNode.Add(i);

                                    //TODO : This is a generalisation and should have calculate position based on previous position
                                    foreach (QLink link in parentLinks)
                                        link.FromPort = i.RightPort;
                                    foreach (QLink link in childLinks)
                                        link.ToPort = i.LeftPort;

                                }
                            }
                            //else
                            //{
                            //    if (rsecPrevious != null && rsecNumber > rsecNodeNumber) //this is if we want the extra.. nodes to be added as well
                            //    {
                            //        CollapsingRecordNodeItem i = rsecPrevious.AddItem();
                            //        if (AsShallow)
                            //            i.MetaObject = item.MetaObject;
                            //        else
                            //            item.MetaObject.CopyPropertiesToObject(i.MetaObject);
                            //        i.HookupEvents();
                            //        i.BindToMetaObjectProperties();
                            //        rsecPrevious.Add(i);
                            //    }
                            //}
                        }
                    }
                }
            }
            //get shape and add at old position
            if ((OriginalNode as GoObject).Parent is Shapes.Nodes.Containers.ILinkedContainer)
            {
                Shapes.Nodes.Containers.ILinkedContainer container = (OriginalNode as GoObject).Parent as Shapes.Nodes.Containers.ILinkedContainer;
                if (container is Shapes.Nodes.Containers.SubgraphNode)
                    (container as Shapes.Nodes.Containers.SubgraphNode).Add(NewNode);
                else if (container is Shapes.Nodes.Containers.ValueChain)
                    (container as Shapes.Nodes.Containers.ValueChain).Add(NewNode);
            }
            else
            {
                doc.Add(NewNode);
            }

            MetaConvert.originalVSreplaced.Add(OriginalNode as GoObject, NewNode as GoObject);

            //relink
            foreach (MetaConvertLink mcl in FromLinks)
                mcl.Convert(doc, NewNode);
            foreach (MetaConvertLink mcl in ToLinks)
                mcl.Convert(doc, NewNode);

            //reanchor
            foreach (GoObject anc in MetaConvert.getAllAchoredToObject(doc, OriginalNode as GoObject))
                (anc as GoBalloon).Anchor = NewNode;

            //refishlink
            if (fishlinksToRelink != null)
                foreach (IGoLink fishLink in fishlinksToRelink)
                    fishLink.FromPort = MetaConvert.findPortAtPositionOfOldPortForNewNode(fishLink.FromPort as GoPort, NewNode);

            //remove old shape (this line will remove links,artefacts,rationales,anchors everything)
            try
            {
                (OriginalNode as GoNode).Remove();
            }
            catch
            {
                (OriginalNode as GoNode).ToString();
            }
        }
    }

    public class MetaConvertLink
    {
        private MetaConvertInfo result;
        public MetaConvertInfo Result
        {
            get
            {
                if (Artefacts != null)
                    foreach (MetaConvertArtefact metaConvertArtefact in Artefacts)
                        if (metaConvertArtefact.Result != MetaConvertInfo.OK)
                            return metaConvertArtefact.Result;

                return result;
            }
            set
            {
                result = value;
            }
        }

        private QLink OriginalLink;
        private string ClassToChangeTo;
        private bool From;
        private int caid = -1;

        public MetaConvertLink(QLink link, bool from, string classToChangeTo)
        {
            Result = MetaConvertInfo.Initialise;
            OriginalLink = link;
            ClassToChangeTo = classToChangeTo;
            From = from;
            Artefacts = new List<MetaConvertArtefact>();

            Check();
            if (Result == MetaConvertInfo.OK)
            {
                foreach (IMetaNode node in link.GetArtefacts())
                {
                    Artefacts.Add(new MetaConvertArtefact(caid, node));
                }
            }
        }

        private List<MetaConvertArtefact> Artefacts;
        public List<MetaConvertArtefact> Report()
        {
            List<MetaConvertArtefact> ret = new List<MetaConvertArtefact>();
            foreach (MetaConvertArtefact imc in Artefacts)
            {
                if (imc.Result != MetaConvertInfo.OK)
                {
                    ret.Add(imc);
                }
            }
            return ret;
        }

        private IMetaNode FromNode { get { return OriginalLink.FromNode as IMetaNode; } }
        private IMetaNode ToNode { get { return OriginalLink.ToNode as IMetaNode; } }
        private void Check()
        {
            string from = null;
            string to = null;
            if (From)
            {
                from = ClassToChangeTo;
                if (MetaConvert.StaticCollection.Contains(ToNode as GoObject))
                    to = ClassToChangeTo;
                else
                    if (ToNode != null)
                        to = ToNode.MetaObject.Class;
            }
            else
            {
                if (MetaConvert.StaticCollection.Contains(FromNode as GoObject))
                    from = ClassToChangeTo;
                else
                    if (FromNode != null)
                        from = FromNode.MetaObject.Class;

                to = ClassToChangeTo;
            }

            if (to != null && from != null)
                foreach (ClassAssociation clsAss in DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByParentClass(from))
                    if (clsAss.ChildClass == to)
                        if (clsAss.AssociationTypeID == (int)OriginalLink.AssociationType)
                            if (clsAss.IsActive == true)
                            {
                                caid = clsAss.CAid;
                                Result = MetaConvertInfo.OK;
                                return;
                            }
                            else
                            {
                                Core.Log.WriteLog("MetaConvertLink::Check::Inactive-CAid::" + clsAss.CAid);

                                caid = clsAss.CAid;
                                Result = MetaConvertInfo.OK;
                                return;
                            }

            //if (MetaBuilder.BusinessFacade.MetaHelper.Singletons.GetAssociationHelper().GetAssociation(from, to, (int)OriginalLink.AssociationType) = null)
            Result = MetaConvertInfo.CannotRelink;
            //else
            //    Result = MetaConvertInfo.OK;
        }

        public void Convert(GoDocument doc, GraphNode newNode)
        {
            if (Result != MetaConvertInfo.OK)
            {
                if (Result == MetaConvertInfo.CannotRelink)
                    foreach (GoObject anc in MetaConvert.getAllAchoredToObject(doc, OriginalLink as GoObject))
                        anc.Remove();
                foreach (IMetaNode n in OriginalLink.GetArtefacts())
                    (n as GoObject).Remove();
                return;
            }
            QLink newLink = null;
            //recreate/reuse links based on whether it is from or not
            IGoPort fromPort = null;
            IGoPort toPort = null;

            GoNode fromNode = null;
            GoNode toNode = null;
            bool createNew = false;

            GoObject replacedLinkAlready = MetaConvert.returnNewObjectUsingOldAsKey(OriginalLink);
            if (replacedLinkAlready == OriginalLink)
            {
                createNew = true;
                if (From)
                {
                    fromNode = newNode;
                    fromPort = MetaConvert.findPortAtPositionOfOldPortForNewNode(OriginalLink.FromPort as QuickPort, newNode);

                    toNode = OriginalLink.ToNode as GoNode;
                    toPort = OriginalLink.ToPort;
                }
                else
                {
                    fromNode = OriginalLink.FromNode as GoNode;
                    fromPort = OriginalLink.FromPort;

                    toNode = newNode;
                    toPort = MetaConvert.findPortAtPositionOfOldPortForNewNode(OriginalLink.ToPort as QuickPort, newNode);
                }
            }
            else
            {
                newLink = replacedLinkAlready as QLink;

                if (From)
                {
                    newLink.FromPort = MetaConvert.findPortAtPositionOfOldPortForNewNode(newLink.FromPort as QuickPort, newNode);
                }
                else
                {
                    newLink.ToPort = MetaConvert.findPortAtPositionOfOldPortForNewNode(newLink.ToPort as QuickPort, newNode);
                }
            }

            if (createNew && toPort != null && fromPort != null)
            {
                newLink = QLink.CreateLink(fromNode, toNode, (int)OriginalLink.AssociationType, fromPort as GoPort, toPort as GoPort);

                if (newLink != null)
                {
                    newLink.RealLink.SetPoints(OriginalLink.RealLink.CopyPointsArray());
                    newLink.PenColorBeforeCompare = OriginalLink.PenColorBeforeCompare;

                    Pen p = new Pen(new SolidBrush(OriginalLink.Pen.Color));
                    p.DashStyle = OriginalLink.Pen.DashStyle;
                    newLink.Pen = p;

                    newLink.RealLink.AvoidsNodes = OriginalLink.RealLink.AvoidsNodes;
                    newLink.RealLink.AdjustingStyle = OriginalLink.RealLink.AdjustingStyle;
                    doc.Add(newLink);

                    MetaConvert.originalVSreplaced.Add(OriginalLink, newLink);

                    //reartefact link
                    foreach (MetaConvertArtefact art in Artefacts)
                        art.Convert(doc, newLink, OriginalLink);

                    //reanchor
                    foreach (GoObject anc in MetaConvert.getAllAchoredToObject(doc, OriginalLink as GoObject))
                        (anc as GoBalloon).Anchor = newLink;
                }
            }
            else
            {
                if (newLink != null)
                {
                    //make sure that the artefacts are actually allowed on newlink
                    foreach (IMetaNode n in newLink.GetArtefacts())
                    {
                        MetaConvertArtefact mca = new MetaConvertArtefact(caid, n);
                        if (mca.Result != MetaConvertInfo.OK)
                            (n as GoObject).Remove();
                    }
                }
            }
        }
    }

    public class MetaConvertArtefact
    {
        private MetaConvertInfo result;
        public MetaConvertInfo Result
        {
            get
            {
                return result;
            }
            set
            {
                result = value;
            }
        }

        private int CAid;
        private IMetaNode OriginalNode;

        public MetaConvertArtefact(int caid, IMetaNode artefactNode)
        {
            Result = MetaConvertInfo.Initialise;
            CAid = caid;
            OriginalNode = artefactNode;

            Check();
        }

        private void Check()
        {
            foreach (AllowedArtifact art in DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.AllowedArtifactProvider.GetByCAid(CAid))
                if ((bool)art.IsActive == true)
                    if (art.Class == OriginalNode.MetaObject.Class)
                    {
                        Result = MetaConvertInfo.OK;
                        return;
                    }
            Result = MetaConvertInfo.CannotReArtefact;
        }

        public void Convert(GoDocument doc, QLink newlink, QLink originalLink)
        {
            if (Result != MetaConvertInfo.OK)
            {
                foreach (GoObject anc in MetaConvert.getAllAchoredToObject(doc, OriginalNode as GoObject))
                    anc.Remove();

                if (Result == MetaConvertInfo.CannotReArtefact)
                {
                    (OriginalNode as GoObject).Remove();
                }
                return;
            }

            //if fromnode is graphnode then we must get port (if it is changing?)
            if (OriginalNode is GraphNode)
            {
                //find the fishlink
                foreach (GoObject o in (OriginalNode as GraphNode).DestinationLinks)
                {
                    if (o is FishLink)
                    {
                        FishLink fLink = o as FishLink;
                        if (fLink != null)
                            if (fLink.ToPort == originalLink.GetFishLinkPort)
                            {
                                //tonode is always midlabel port
                                fLink.ToPort = newlink.GetFishLinkPort as IGoPort;
                            }
                    }
                    else if (o is FishRealLink)
                    {
                        FishRealLink fLink = o as FishRealLink;
                        if (fLink != null)
                            if (fLink.ToPort == originalLink.GetFishLinkPort)
                            {
                                //tonode is always midlabel port
                                fLink.ToPort = newlink.GetFishLinkPort as IGoPort;
                            }
                    }
                }
            }
            else
            {
                //find the fishlink
                if (OriginalNode is ArtefactNode)
                {
                    FishLink fLink = null;
                    ArtefactNode art = OriginalNode as ArtefactNode;
                    foreach (GoObject o in art.Links)
                        if (o is FishLink)
                            fLink = o as FishLink;
                    //tonode is always midlabel port
                    if (fLink != null)
                        fLink.ToPort = newlink.GetFishLinkPort as IGoPort;
                }
            }

            foreach (GoObject anc in MetaConvert.getAllAchoredToObject(doc, OriginalNode as GoObject))
                (anc as GoBalloon).Anchor = OriginalNode as GoObject;
        }
    }
}
