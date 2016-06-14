using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;
using Northwoods.Go.Draw;
using MetaBuilder.Graphing;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Nodes;
using MetaBuilder.Meta;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using MetaBuilder.BusinessLogic;

namespace ShapeBuilding.Links
{

    [Serializable]
    public class QLink : GoLabeledLink
    {
        #region Meta Related Properties
        private LinkAssociationType associationType;
        [Browsable(false)]
        public LinkAssociationType AssociationType
        {
            get { return associationType; }
            set
            {
                LinkAssociationType oldVal = associationType;
                associationType = value;
                if (oldVal != value)
                {
                    ChangedLinkType();
                }
            }
        }
        public string FromClass
        {
            get
            {
                string retval = null;
                if (FromPort != null)
                {
                    if (FromPort.GoObject.ParentNode is IMetaNode)
                    {
                        return ((IMetaNode)FromPort.GoObject.ParentNode).BindingInfo.BindingClass;
                    }

                    /*if (FromPort.GoObject.Parent is CollapsingRecordNodeItem)
                    {
                        CollapsingRecordNodeItem crnitem = FromPort.GoObject.Parent as CollapsingRecordNodeItem;
                        return crnitem.BindingInfo.BindingClass;
                    }

                    if (FromPort.GoObject.DraggingObject is IMetaNode)
                    {
                        IMetaNode oFrom = FromPort.GoObject.DraggingObject as IMetaNode;
                        if (oFrom.HasBindingInfo)
                            retval = oFrom.BindingInfo.BindingClass;
                    }

                    if (retval == null && FromPort.GoObject.ParentNode is IMetaNode)
                    {
                        IMetaNode oFrom2 = FromPort.GoObject.ParentNode as IMetaNode;
                        if (oFrom2.HasBindingInfo)
                            retval = oFrom2.BindingInfo.BindingClass;
                    }
                    if (FromPort.GoObject.DraggingObject is SubgraphNode.CustomSubGraphPort)
                    {
                        IMetaNode oFrom = FromPort.GoObject.ParentNode as IMetaNode;
                        if (oFrom.HasBindingInfo)
                            retval = oFrom.BindingInfo.BindingClass;
                    }
                    if (FromPort.GoObject.DraggingObject is NonPrintingQuickPort)
                    {
                        if (FromPort.GoObject.DraggingObject.Parent is ValueChain)
                        {
                            return "Function";
                        }
                    }*/
                }
                return retval;
            }
        }
        public string ToClass
        {
            get
            {
                string retval = null;
                if (ToPort != null)
                {
                    if (ToPort.GoObject.ParentNode is IMetaNode)
                    {
                        IMetaNode imn = ToPort.GoObject.ParentNode as IMetaNode;
                        return imn.BindingInfo.BindingClass;
                    }
                    /*
                    if (ToPort.GoObject.Parent is CollapsingRecordNodeItem)
                    {
                        CollapsingRecordNodeItem crnitem = ToPort.GoObject.Parent as CollapsingRecordNodeItem;
                        return crnitem.BindingInfo.BindingClass;
                    }

                    if (ToPort.GoObject.DraggingObject is IMetaNode)
                    {
                        IMetaNode oTo = ToPort.GoObject.DraggingObject as IMetaNode;
                        if (oTo.HasBindingInfo)
                            retval = oTo.BindingInfo.BindingClass;
                    }

                    if (ToPort.GoObject.ParentNode is IMetaNode)
                    {
                        IMetaNode imnTo = ToPort.GoObject.ParentNode as IMetaNode;
                        if (imnTo.HasBindingInfo)
                            retval = imnTo.BindingInfo.BindingClass;
                    }
                    if (ToPort.GoObject.DraggingObject is SubgraphNode.CustomSubGraphPort)
                    {
                        IMetaNode oTo = ToPort.GoObject.ParentNode as IMetaNode;
                        if (oTo.HasBindingInfo)
                            retval = oTo.BindingInfo.BindingClass;
                    }
                    if (ToPort.GoObject.DraggingObject is NonPrintingQuickPort)
                    {
                        if (ToPort.GoObject.ParentNode is ValueChain)
                        {
                            return "Function";
                        }
                    }*/
                }

                return retval;
            }
        }
        #endregion

        #region Helper Methods
        public List<ArtefactNode> GetArtefacts()
        {
            List<ArtefactNode> artefactNodes = new List<ArtefactNode>();
            if (MidLabel != null)
            {
                if (MidLabel is GoGroup)
                {
                    GoGroup grp = MidLabel as GoGroup;
                    foreach (GoObject o in grp)
                    {
                        if (o is GoPort)
                        {
                            GoPort prt = o as GoPort;

                            GoPortFilteredLinkEnumerator portenum = prt.SourceLinks.GetEnumerator();
                            while (portenum.MoveNext())
                            {
                                if (portenum.Current.FromNode is ArtefactNode)
                                {
                                    artefactNodes.Add(portenum.Current.FromNode as ArtefactNode);
                                }
                            }
                        }
                    }
                }
            }
            return artefactNodes;
        }
        public List<Rationale> GetRationales()
        {
            List<Rationale> rats = new List<Rationale>();
            if (MidLabel != null)
            {
                if (MidLabel is GoGroup)
                {
                    GoGroup grp = MidLabel as GoGroup;
                    foreach (GoObject o in grp)
                    {
                        if (o is GoPort)
                        {
                            GoPort prt = o as GoPort;

                            GoPortFilteredLinkEnumerator portenum = prt.SourceLinks.GetEnumerator();
                            while (portenum.MoveNext())
                            {
                                if (portenum.Current.FromNode is Rationale)
                                {
                                    rats.Add(portenum.Current.FromNode as Rationale);
                                }
                            }
                        }
                    }
                }
            }
            return rats;
        }
        #endregion

        //[field: NonSerialized]
        //public event GoObjectEventHandler AssociationTypeChange;

        public QLink()
        {
            this.ToArrow = true;
            // add a FishLinkPort to the link
            // (but links to this port actually "connect" to the link
            //  at the point computed by FishLinkPort.GetToLinkPoint)
            this.MidLabelCentered = true;
            this.ToLabelCentered = true;
            this.FromLabelCentered = true;
            FishLinkPort p = new FishLinkPort();
            p.PortObject = this.RealLink;  // tell the FishLinkPort which GoStroke it should "connect" to
            this.MidLabel = p;  // add the FishLinkPort to the FishLink
            RealLink.Orthogonal = true;
            RealLink.Style = GoStrokeStyle.RoundedLineWithJumpOvers;
        }

        // the FishLink group uses a FishRealLink instead of a regular GoLink
        public override GoLink CreateRealLink()
        {
            return new QRealLink();
        }

        #region ArrowHeads and such

        private void ChangedLinkType()
        {
            RealLink.FromArrow = false;
            RealLink.Brush = Brushes.White;//new SolidBrush(Color.White);

            switch (associationType)
            {
                case LinkAssociationType.Auxiliary:
                    SetLabels(false, false, true);
                    break;
                case LinkAssociationType.Classification:
                    SetLabels(false, false, true);
                    break;
                case LinkAssociationType.Concurrent:
                    RealLink.Brush = new SolidBrush(Color.Black);
                    SetLabels(false, true, false);
                    break;
                case LinkAssociationType.ConnectedTo:
                    RealLink.FromArrow = true;
                    RealLink.Brush = new SolidBrush(Color.Black);
                    break;
                case LinkAssociationType.Create:
                    RealLink.Brush = new SolidBrush(Color.Black);
                    SetLabels(false, true, false);
                    break;
                case LinkAssociationType.Decomposition:
                    RealLink.ToArrowStyle = GoStrokeArrowheadStyle.Polygon;
                    RealLink.ToArrowLength = 0;
                    RealLink.ToArrowWidth = 13;
                    RealLink.ToArrowShaftLength = 10;
                    break;
                case LinkAssociationType.Delete:
                    RealLink.Brush = new SolidBrush(Color.Black);
                    SetLabels(false, true, false);
                    break;
                case LinkAssociationType.Interrupt:
                    RealLink.Brush = new SolidBrush(Color.Black);
                    SetLabels(true, false, false);
                    break;
                //case LinkAssociationType.IsSupplierTo:
                //    break;
                case LinkAssociationType.LeadsTo:
                    break;
                case LinkAssociationType.LocatedAt:
                    break;
                case LinkAssociationType.Maintain:
                    RealLink.Brush = new SolidBrush(Color.Black);
                    SetLabels(false, true, false);
                    break;
                case LinkAssociationType.Many_To_Many:
                    this.ToArrowStyle = GoStrokeArrowheadStyle.Cross;
                    this.ToArrowShaftLength = 0;
                    this.ToArrowLength = 13;
                    this.FromArrowStyle = GoStrokeArrowheadStyle.Cross;
                    this.FromArrowShaftLength = 0;
                    this.FromArrowLength = 13;
                    this.FromArrow = true;
                    break;
                case LinkAssociationType.Mapping:
                    ToArrow = false;
                    FromArrow = false;
                    SetLabels(true, false, true);
                    break;
                case LinkAssociationType.MutuallyExclusiveLink:
                    Pen p = new Pen(Color.Black);
                    p.Width = 2f;
                    p.DashStyle = DashStyle.Dot;
                    RealLink.Pen = p;
                    RealLink.ToArrow = false;
                    break;
                case LinkAssociationType.NonConcurrent:
                    SetLabels(false, true, false);
                    break;
                case LinkAssociationType.One_To_Many:
                    this.ToArrowStyle = GoStrokeArrowheadStyle.Cross;
                    this.ToArrowShaftLength = 0;
                    this.ToArrowLength = 13;
                    this.FromArrowStyle = GoStrokeArrowheadStyle.Cross;
                    this.FromArrowShaftLength = 0;
                    this.FromArrowLength = 13;
                    this.FromArrow = true;
                    break;
                case LinkAssociationType.OneWayCurved:
                    break;
                case LinkAssociationType.Read:
                    RealLink.Brush = new SolidBrush(Color.Black);
                    SetLabels(false, true, false);
                    break;
                case LinkAssociationType.Resume:
                    RealLink.Brush = new SolidBrush(Color.Black);
                    SetLabels(true, false, false);
                    break;
                case LinkAssociationType.Start:
                    RealLink.Brush = new SolidBrush(Color.Black);
                    SetLabels(true, false, false);
                    break;
                case LinkAssociationType.Stop:
                    RealLink.Brush = new SolidBrush(Color.Black);
                    SetLabels(true, false, false);
                    break;
                case LinkAssociationType.SubSetOf:
                    Pen pss = new Pen(Color.Black);
                    pss.Width = 2f;
                    pss.DashStyle = DashStyle.Dot;
                    RealLink.Pen = pss;
                    RealLink.Brush = new SolidBrush(Color.Black);
                    break;
                case LinkAssociationType.Suspend:
                    RealLink.Brush = new SolidBrush(Color.Black);
                    SetLabels(true, false, false);
                    break;
                case LinkAssociationType.Synchronise:
                    SetLabels(false, true, false);
                    break;
                case LinkAssociationType.Update:
                    RealLink.Brush = new SolidBrush(Color.Black);
                    SetLabels(false, true, false);
                    break;
                case LinkAssociationType.Zero_To_One:
                    this.ToArrowLength = 15;
                    this.ToArrowShaftLength = 0;
                    this.ToArrowWidth = 15;
                    this.ToArrowStyle = GoStrokeArrowheadStyle.Cross;
                    this.FromArrowStyle = GoStrokeArrowheadStyle.Polygon;
                    this.FromArrowShaftLength = 0;
                    this.FromArrowLength = 13;
                    this.FromArrowWidth = 0;
                    this.FromArrow = true;
                    break;
                case LinkAssociationType.ZeroOrMore_To_One:
                    this.ToArrowStyle = GoStrokeArrowheadStyle.Cross;
                    this.ToArrowShaftLength = 0;
                    this.ToArrowLength = 13;
                    break;
                default:
                    // case LinkAssociationType.Dependencies: 
                    // case LinkAssociationType.DynamicFlow
                    // Both the same.
                    RealLink.ToArrow = true;
                    RealLink.Brush = new SolidBrush(Color.Black);
                    break;
            }
        }

        private void SetLabels(bool from, bool mid, bool to)
        {
            if (from)
            {
                FromLabel = GetLabel();
            }
            if (mid)
            {
                MidLabel = GetLabel();
            }

            if (to)
            {
                ToLabel = GetLabel();
            }
        }

        private GoGroup GetLabel()
        {
            ArrowBuilder arrBuilder = new ArrowBuilder();
            return arrBuilder.CreateLabel(this.associationType);
        }
        #endregion

        #region EventHandling
        /*public void ChangeAssociationType(object sender, EventArgs e)
        {
            //  System.Diagnostics.Debug.WriteLine("ChangeAssociationType");

            MenuItem mitem = sender as MenuItem;
            LinkAssociationType newType = (LinkAssociationType)mitem.Tag;
            AssociationType = newType;
            GoInputEventArgs ieargs = new GoInputEventArgs();
            GoObjectEventArgs args = new GoObjectEventArgs(this, ieargs);
            OnAssociationTypeChange(args);
        }
        public void Delete_Command(Object sender, EventArgs e)
        {
            GraphView v = GoContextMenu.FindView(sender as MenuItem) as GraphView;
            if (v != null)
            {
                v.EditDelete();
            }
        }
         public override void Unlink()
        {
            if (RealLink != null)
                RealLink.Unlink();
            base.Unlink();
            if (MidLabel != null)
                Console.WriteLine(MidLabel.ToString());
        }

        public override void OnPortChanged(IGoPort port, int subhint, int oldI, object oldVal, RectangleF oldRect,
                                           int newI, object newVal, RectangleF newRect)
        {
            if (subhint != 1001)
            {
                if (port != null)
                    if (port is QuickPort) // && (!this.Initializing))
                    {
                        TestForSelfLinks();
                        base.OnPortChanged(port, subhint, oldI, oldVal, oldRect, newI, newVal, newRect);
                        return;
                    }
            }
            base.OnPortChanged(port, subhint, oldI, oldVal, oldRect, newI, newVal, newRect);
        }
         protected virtual void OnAssociationTypeChange(GoObjectEventArgs e)
        {
            if (AssociationTypeChange != null)
            {
                AssociationTypeChange(this, e);
            }
        }
         */

        public override bool OnEnterLeave(GoObject from, GoObject to, GoView view)
        {
            if (Pen != null)
            {
                if (to == this)
                {
                    if (Pen.Color != Color.Red && Pen.Color != Color.Orange && Pen.Color != Color.Blue)
                    {
                        oldPen = Pen;
                        Pen = new Pen(Color.SlateBlue, 2.3f);
                        Pen.DashStyle = oldPen.DashStyle;
                    }
                }
                else
                {
                    if (Pen.Color != Color.Red && Pen.Color != Color.Orange && Pen.Color != Color.Blue)
                    {
                        Pen = new Pen(Color.Black, 2.3f);
                        if (oldPen != null)
                            this.Pen.DashStyle = oldPen.DashStyle;
                    }
                }
            }
            return base.OnEnterLeave(from, to, view);
        }

        private Pen oldPen;
        #endregion

        #region Misc Link Related

        public override string GetToolTip(GoView view)
        {
            return this.associationType.ToString();
        }

        #region Points Manipulation
       
        public void Enable_AutoRouting(object sender, EventArgs e)
        {
            RealLink.AvoidsNodes = true;
        }
        #endregion
        /// <summary>
        /// Moves to layer.
        /// </summary>
        /// <param name="target">The target.</param>
        public void MoveToLayer(GoLayer target)
        {
            IGoPort fromPort = FromPort;
            Remove();
            target.Add(this);
            FromPort = fromPort;
        }

        #endregion

        #region Overrides
        public override bool Deletable
        {
            get
            {
                try
                {
                    if (ToNode != null && FromNode != null)
                    {
                        if (ToNode is IMetaNode && FromNode is IMetaNode)
                        {
                            IMetaNode fromGraphNode = FromNode as IMetaNode;
                            IMetaNode toGraphNode = ToNode as IMetaNode;
                            if (fromGraphNode.MetaObject != null && toGraphNode.MetaObject != null)
                            {
                                if (fromGraphNode.MetaObject.State == VCStatusList.Locked
                                    &&
                                    toGraphNode.MetaObject.State == VCStatusList.Locked)
                                    return false;
                            }
                        }
                    }
                }
                catch
                {
                    return base.Deletable;
                }
                return base.Deletable;
            }
            set { base.Deletable = value; }
        }
        #endregion
    }
}
