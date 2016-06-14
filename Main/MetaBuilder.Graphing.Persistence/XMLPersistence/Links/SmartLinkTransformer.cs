using System;
using System.Drawing;
using MetaBuilder.Graphing.Persistence.XMLPersistence.Nodes;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Nodes.Containers;
using MetaBuilder.Meta;
using Northwoods.Go;
using Northwoods.Go.Xml;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Links
{
    public class QLinkTransformer : EmbeddedObjectsTransformer
    {

        #region Constructors (1)

        public QLinkTransformer()
            : base()
        {
            this.ElementName = "link";
            this.TransformerType = typeof(QLink);
            this.BodyConsumesChildElements = true;
            this.IdAttributeUsedForSharedObjects = true;
        }

        #endregion Constructors

        #region Methods (6)

        // Public Methods (6) 

        public override object Allocate()
        {
            QLink l = new QLink();
            l.ToArrow = true;

            /*GoText lab = new GoText();
            lab.Selectable = false;
            l.MidLabel = lab;*/
            return l;
        }

        public override void ConsumeAttributes(Object obj)
        {
            base.ConsumeAttributes(obj);
            QLink x = (QLink)obj;
            String fromid = StringAttr("from", null);
            if (fromid != null)
            {
                x.FromPort = this.Reader.FindShared(fromid) as GoPort;
            }
            String toid = StringAttr("to", null);
            if (toid != null)
            {
                x.ToPort = this.Reader.FindShared(toid) as GoPort;
            }

            x.RealLink.ToArrowFilled = BooleanAttr("ToArrowFilled", false);
            x.RealLink.FromArrowFilled = BooleanAttr("FromArrowFilled", false);
        }

        public override void ConsumeChild(object parent, object child)
        {
            //base.ConsumeChild(parent, child);
        }

        public override void ConsumeObjectFinish(object obj)
        {
            base.ConsumeObjectFinish(obj);

            QLink slink = obj as QLink;
            slink.Initializing = true;

            #region cnIDs

            string fromPortCNID = StringAttr("parentCN", null);
            if (fromPortCNID != null)
            {
                CollapsibleNode parentCollapsibleNode = Reader.FindShared(fromPortCNID) as CollapsibleNode;
                if (parentCollapsibleNode != null)
                {
                    MetaBase mbFromCNI = this.Reader.FindShared(StringAttr("fromMB", "")) as MetaBase;
                    if (mbFromCNI != null)
                    {
                        mbFromCNI.tag = null;
                        //find the metaobject it must link from in this parentCollapsibleNode
                        foreach (GoObject CNchild in parentCollapsibleNode.GetEnumerator())
                        {
                            if (mbFromCNI.tag != null)
                                break;
                            if (!(CNchild is CollapsingRecordNodeItemList))
                                continue;

                            foreach (GoObject section in (CNchild as CollapsingRecordNodeItemList).GetEnumerator())
                            {
                                if (mbFromCNI.tag != null)
                                    break;
                                if (!(section is RepeaterSection))
                                    continue;

                                foreach (GoObject sectionItem in (section as RepeaterSection).GetEnumerator())
                                {
                                    if (mbFromCNI.tag != null)
                                        break;

                                    if (sectionItem is CollapsingRecordNodeItem)
                                    {
                                        CollapsingRecordNodeItem CNitem = sectionItem as CollapsingRecordNodeItem;
                                        if (CNitem.IsHeader) continue;
                                        if (CNitem.MetaObject != null)
                                        {
                                            if (CNitem.MetaObject.pkid == mbFromCNI.pkid)
                                            {
                                                mbFromCNI.tag = CNitem;
                                                break;
                                            }
                                        }
                                    }
                                }

                            }
                        }

                        if (mbFromCNI.tag != null)
                        {
                            //set the port to that found nodes MBPort
                            int mbFromPortIndex = Int32Attr("fromMBPort", 0);
                            GoNode n = mbFromCNI.tag as GoNode;
                            slink.FromPort = n[mbFromPortIndex] as GoPort;
                        }
                    }
                }
            }

            string toPortCNID = StringAttr("childCN", null);
            if (toPortCNID != null)
            {
                CollapsibleNode parentCollapsibleNode = Reader.FindShared(toPortCNID) as CollapsibleNode;
                if (parentCollapsibleNode != null)
                {
                    MetaBase mbToCNI = this.Reader.FindShared(StringAttr("toMB", "")) as MetaBase;
                    if (mbToCNI != null)
                    {
                        mbToCNI.tag = null;
                        //find the metaobject it must link to in this parentCollapsibleNode
                        foreach (GoObject CNchild in parentCollapsibleNode.GetEnumerator())
                        {
                            if (mbToCNI.tag != null)
                                break;
                            if (!(CNchild is CollapsingRecordNodeItemList))
                                continue;

                            foreach (GoObject section in (CNchild as CollapsingRecordNodeItemList).GetEnumerator())
                            {
                                if (mbToCNI.tag != null)
                                    break;
                                if (!(section is RepeaterSection))
                                    continue;

                                foreach (GoObject sectionItem in (section as RepeaterSection).GetEnumerator())
                                {
                                    if (mbToCNI.tag != null)
                                        break;

                                    if (sectionItem is CollapsingRecordNodeItem)
                                    {
                                        CollapsingRecordNodeItem CNitem = sectionItem as CollapsingRecordNodeItem;
                                        if (CNitem.IsHeader) continue;
                                        if (CNitem.MetaObject != null)
                                        {
                                            if (CNitem.MetaObject.pkid == mbToCNI.pkid)
                                            {
                                                mbToCNI.tag = CNitem;
                                                break;
                                            }
                                        }
                                    }
                                }

                            }
                        }

                        if (mbToCNI.tag != null)
                        {
                            //set the port to that found nodes MBPort
                            int mbFromPortIndex = Int32Attr("toMBPort", 0);
                            GoNode n = mbToCNI.tag as GoNode;
                            slink.ToPort = n[mbFromPortIndex] as GoPort;
                        }
                    }
                }
            }

            #endregion

            #region shallowIDS (Quickports have their own IDS and therefore this is not relevant

            //if (slink.FromPort == null)
            //{
            //    //check from shallowfromcn and shallowtocn
            //    string fromPortShallowID = StringAttr("shallowFromN", null);
            //    if (fromPortShallowID != null)
            //    {
            //        //find shared object
            //        GraphNode parentGraphNode = Reader.FindShared(fromPortShallowID) as GraphNode;
            //        if (parentGraphNode != null)
            //        {
            //            MetaBase mbFromNI = this.Reader.FindShared(StringAttr("fromMB", "")) as MetaBase;
            //            if (mbFromNI != null)
            //            {
            //                //connect to mbport
            //            }
            //        }
            //    }
            //}

            //if (slink.ToPort == null)
            //{

            //}

            #endregion

            #region mbID's OLD collapsible node item load which does not care about shallow copies

            if (slink.FromPort == null)
            {
                String mbFromID = StringAttr("fromMB", "");
                if (mbFromID != "")
                {
                    MetaBase mbFrom = this.Reader.FindShared(mbFromID) as MetaBase;
                    if (mbFrom != null)
                    {
                        if (mbFrom.tag != null)
                        {
                            String mbFromPortIndex = StringAttr("fromMBPort", "");
                            GoNode n = mbFrom.tag as GoNode;
                            slink.FromPort = n[int.Parse(mbFromPortIndex)] as GoPort;
                        }
                    }
                    else
                        Reader.AddDelayedRef(obj, "mbFrom", mbFromID);

                    //if (qport.ParentNode == qport)

                    //else
                    //  slink.FromPort = qport;
                }
                else
                {
                    Reader.AddDelayedRef(obj, "mbFrom", mbFromID);
                }
            }

            if (slink.ToPort == null)
            {
                String mbToID = StringAttr("toMB", "");
                if (mbToID != "")
                {
                    MetaBase mbTo = this.Reader.FindShared(mbToID) as MetaBase;
                    if (mbTo != null)
                    {
                        if (mbTo.tag != null)
                        {
                            String mbToPortIndex = StringAttr("toMBPort", "");
                            GoNode n = mbTo.tag as GoNode;
                            slink.ToPort = n[int.Parse(mbToPortIndex)] as GoPort;
                        }
                    }
                    else
                        Reader.AddDelayedRef(obj, "mbTo", mbToID);

                    //if (qport.ParentNode == qport)

                    //else
                    //  slink.FromPort = qport;
                }
                else
                {
                    Reader.AddDelayedRef(obj, "mbTo", mbToID);
                }
            }

            #endregion

            #region portids

            if (slink.FromPort == null)
            {
                String fromPortid = StringAttr("from", null);
                if (fromPortid != null)
                {
                    QuickPort qport = this.Reader.FindShared(fromPortid) as QuickPort;
                    if (qport != null)
                    {
                        if (qport.ParentNode == qport)
                            Reader.AddDelayedRef(obj, "fromPort", fromPortid);
                        else
                            slink.FromPort = qport;
                    }
                    else
                    {
                        Reader.AddDelayedRef(obj, "fromPort", fromPortid);
                    }
                }
            }

            if (slink.ToPort == null)
            {
                String toPortid = StringAttr("to", null);
                if (toPortid != null)
                {
                    QuickPort qportTo = this.Reader.FindShared(toPortid) as QuickPort;
                    if (qportTo != null)
                    {
                        if (qportTo.ParentNode == qportTo)
                            Reader.AddDelayedRef(obj, "toPort", toPortid);
                        else
                            slink.ToPort = qportTo;
                    }
                    else
                    {
                        Reader.AddDelayedRef(obj, "toPort", toPortid);
                    }
                }
            }
            #endregion

            bool setAvoidesNodes = BooleanAttr("AvoidsNodes", false);
            if (!setAvoidesNodes)
            {
                slink.RealLink.AvoidsNodes = setAvoidesNodes;
                slink.RealLink.AdjustingStyle = (GoLinkAdjustingStyle)Enum.Parse(typeof(GoLinkAdjustingStyle), StringAttr("adjStyle", "Calculate"));
            }

            PointF[] pts = PointFArrayAttr("Points", new PointF[] { });
            slink.Relinkable = true;
            slink.RealLink.SetPoints(pts);
            //slink.Pen = new Pen(ColorAttr("penColor", Color.Black));
            slink.PenColorBeforeCompare = ColorAttr("penColor", Color.Black);

            Type tAssocType = typeof(LinkAssociationType);
            string assocType = StringAttr("type", LinkAssociationType.Mapping.ToString());
            if (assocType == "Insert")
                assocType = "Create";
            else if (assocType == "Dependencies")
                assocType = "Dependency";
            slink.Transforming = true;
            slink.AssociationType = (LinkAssociationType)Enum.Parse(tAssocType, assocType);
            slink.Transforming = false;
            //slink.ChangedLinkType();

            slink.Initializing = false;
            slink.LayoutChildren(slink);

            if (slink.ToPort == null || slink.ToPort.Node == null)
            {
                //shallowToN
                Reader.AddDelayedRef(obj, "ToPort", StringAttr("shallowToN", null));
                slink.ShallowToN = StringAttr("shallowToN", "");
                if (string.IsNullOrEmpty(slink.ShallowToN))
                    slink.ShallowToN = StringAttr("childCN", "");
                slink.toPortLoc = StringAttr("toPorLoc", "");
            }
            //else if (slink.ToPort.Node is MetaBuilder.Graphing.Shapes.Nodes.ImageNode)
            //{
            //    slink.ToPort = null;
            //    Reader.AddDelayedRef(obj, "ToPort", StringAttr("shallowToN", null));
            //    slink.ShallowToN = StringAttr("shallowToN", "");
            //    slink.toPortLoc = StringAttr("toPorLoc", "");
            //}
            if (slink.FromPort == null || slink.FromPort.Node == null)
            {
                //shallowFromN
                Reader.AddDelayedRef(obj, "FromPort", StringAttr("shallowFromN", null));
                slink.ShallowFromN = StringAttr("shallowFromN", "");
                if (string.IsNullOrEmpty(slink.ShallowFromN))
                    slink.ShallowFromN = StringAttr("parentCN", "");
                slink.fromPortLoc = StringAttr("fromPorLoc", "");
            }
            //else if (slink.FromPort.Node is MetaBuilder.Graphing.Shapes.Nodes.ImageNode)
            //{
            //    slink.FromPort = null;
            //    Reader.AddDelayedRef(obj, "FromPort", StringAttr("shallowFromN", null));
            //    slink.ShallowFromN = StringAttr("shallowFromN", "");
            //    slink.fromPortLoc = StringAttr("fromPorLoc", "");
            //}

            slink.GapType = (GapType)Enum.Parse(typeof(GapType), StringAttr("gap", "None"), false);
        }

        /*     public override void GenerateAttributes(object obj)
             {
                 base.GenerateAttributes(obj);
                 QLink slink = obj as QLink;
                 WriteAttrVal("type", slink.AssociationType.ToString());
                 GraphNode nodeFrom = slink.FromNode as GraphNode;
                 GraphNode nodeTo = slink.ToNode as GraphNode;
                 WriteAttrVal("fromNodeID", nodeFrom.Name);
                 WriteAttrVal("toNodeID", nodeTo.Name);
             }
             public override void ConsumeAttributes(object obj)
             {
                 base.ConsumeAttributes(obj);
                 QLink slink = obj as QLink;
                 Type tAssocType = typeof(MetaBuilder.Meta.LinkAssociationType);
                 string assocType = StringAttr("type",MetaBuilder.Meta.LinkAssociationType.Decomposition.ToString());
                 slink.AssociationType = (MetaBuilder.Meta.LinkAssociationType)Enum.Parse(tAssocType, MetaBuilder.Meta.LinkAssociationType.Decomposition.ToString());
            
             }*/
        public override void GenerateAttributes(Object obj)
        {
            Writer.MakeShared(obj);
            base.GenerateAttributes(obj, true);
            QLink x = (QLink)obj;
            WriteAttrVal("type", x.AssociationType.ToString());
            GoPort p = x.FromPort as GoPort;

            if (x.FromNode is MetaBuilder.Graphing.Shapes.CollapsingRecordNodeItem)
            {
                MetaBuilder.Graphing.Shapes.CollapsingRecordNodeItem clabel = x.FromNode as MetaBuilder.Graphing.Shapes.CollapsingRecordNodeItem;
                WriteAttrVal("fromMB", this.Writer.MakeShared(clabel.MetaObject));
                WriteAttrVal("fromMBPort", clabel.IndexOf(x.FromPort).ToString());

                WriteAttrVal("parentCN", (clabel.ParentNode as CollapsibleNode).MetaObject.pkid + ":" + (clabel.ParentNode as CollapsibleNode).Location.ToString());
            }
            else if (x.FromNode is MetaBuilder.Graphing.Shapes.IMetaNode)
            {
                if ((x.FromNode as IMetaNode).MetaObject != null)
                    WriteAttrVal("shallowFromN", (x.FromNode as IMetaNode).MetaObject.pkid + ":" + (x.FromNode as GoNode).Location.ToString());
            }

            if (x.ToNode is MetaBuilder.Graphing.Shapes.CollapsingRecordNodeItem)
            {
                MetaBuilder.Graphing.Shapes.CollapsingRecordNodeItem clabel = x.ToNode as MetaBuilder.Graphing.Shapes.CollapsingRecordNodeItem;
                WriteAttrVal("toMB", this.Writer.MakeShared(clabel.MetaObject));
                WriteAttrVal("toMBPort", clabel.IndexOf(x.ToPort).ToString());

                WriteAttrVal("childCN", (clabel.ParentNode as CollapsibleNode).MetaObject.pkid + ":" + (clabel.ParentNode as CollapsibleNode).Location.ToString());
            }
            else if (x.ToNode is MetaBuilder.Graphing.Shapes.IMetaNode)
            {
                if ((x.ToNode as IMetaNode).MetaObject != null)
                    WriteAttrVal("shallowToN", (x.ToNode as IMetaNode).MetaObject.pkid + ":" + (x.ToNode as GoNode).Location.ToString());
            }

            if (p != null)
            {
                if (p is SubgraphNode.CustomSubGraphPort)
                {
                    SubgraphNode.CustomSubGraphPort sgPort = p as SubgraphNode.CustomSubGraphPort;
                    WriteAttrVal("from", this.Writer.MakeShared(sgPort.ParentNode));
                }
                else
                {
                    WriteAttrVal("from", this.Writer.MakeShared(p));
                    if (p is QuickPort)
                        WriteAttrVal("fromPorLoc", (p as QuickPort).PortPosition.ToString());
                }
            }
            p = x.ToPort as GoPort;
            if (p != null)
            {
                if (p is SubgraphNode.CustomSubGraphPort)
                {
                    SubgraphNode.CustomSubGraphPort sgPort = p as SubgraphNode.CustomSubGraphPort;
                    WriteAttrVal("to", this.Writer.MakeShared(sgPort.ParentNode));
                }
                else
                {
                    WriteAttrVal("to", this.Writer.MakeShared(p));
                    if (p is QuickPort)
                        WriteAttrVal("toPorLoc", (p as QuickPort).PortPosition.ToString());
                }
            }
            //GoGroup lab = x.MidLabel as GoGroup;
            //if (lab != null)
            //{
            //    //  WriteAttrVal("Text", lab.Text);
            //}

            PointF[] points = x.RealLink.CopyPointsArray();
            WriteAttrVal("Points", points);
            WriteAttrVal("AvoidsNodes", x.RealLink.AvoidsNodes);
            WriteAttrVal("ToArrowFilled", x.RealLink.ToArrowFilled);
            WriteAttrVal("FromArrowFilled", x.RealLink.FromArrowFilled);
            WriteAttrVal("adjStyle", x.RealLink.AdjustingStyle.ToString());
            //if (!(x.PenColorBeforeCompare.IsEmpty))
            //    WriteAttrVal("penColor", x.Pen.Color);
            //else
            WriteAttrVal("penColor", x.PenColorBeforeCompare);
            WriteAttrVal("gap", x.GapType.ToString());
        }

        public override void UpdateReference(object obj, string prop, object referred)
        {
            QLink link = null;
            if (prop == "fromPort")
            {
                if (referred is QuickPort)
                {
                    QuickPort qpFrom = referred as QuickPort;

                    link = obj as QLink;
                    link.FromPort = qpFrom;
                }
                if (referred is SubgraphNode)
                {
                    SubgraphNode sgnode = referred as SubgraphNode;
                    link = obj as QLink;
                    link.FromPort = sgnode.Port;
                }
            }
            if (prop == "toPort")
            {
                if (referred is QuickPort)
                {
                    QuickPort qpTo = referred as QuickPort;
                    link = obj as QLink;
                    link.ToPort = qpTo;
                }
                if (referred is SubgraphNode)
                {
                    SubgraphNode sgnode2 = referred as SubgraphNode;
                    link = obj as QLink;
                    link.ToPort = sgnode2.Port;
                }
            }
            if (link != null)
                link.Initializing = false;
        }

        #endregion Methods

    }
}