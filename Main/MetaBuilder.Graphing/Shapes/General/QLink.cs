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
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Graphing.Shapes.General;

namespace MetaBuilder.Graphing.Shapes
{
    public enum InferenceType
    {
        Reflexivity, Augmentation, Transitivity, Ordinary, Error, None
    }

    public enum GapType
    {
        None = 0,
        Reuse = 1,
        Change = 2,
        Add = 3,
        Merge = 4,
        Remove = 5
    }

    [Serializable]
    public class QLink : GoLabeledLink
    {
        [NonSerialized]
        private GapType gapType;
        public GapType GapType
        {
            get { return gapType; }
            set { gapType = value; }
        }

        [NonSerialized]
        private InferenceType inferenceType;
        public InferenceType InferenceType
        {
            get { return inferenceType; }
            set { inferenceType = value; }
        }

        [NonSerialized]
        private ObjectAssociation dbAssociation;
        public ObjectAssociation DBAssociation
        {
            get
            {
                if (dbAssociation == null)
                    if (IsInDatabase)
                    {
                    }
                return dbAssociation;
            }
        }

        //this will add the link to the database but it wont change its status
        public bool IsInDatabase
        {
            get
            {
                if (!Transforming && Core.Variables.Instance.SaveOnCreate)
                {
                    if (FromNode is IMetaNode && ToNode is IMetaNode)
                    {
                        (FromNode as IMetaNode).SaveToDatabase(this, EventArgs.Empty);
                        (ToNode as IMetaNode).SaveToDatabase(this, EventArgs.Empty);

                        if (((FromNode as IMetaNode).MetaObject.IsInDatabase(Core.Variables.Instance.ClientProvider) && ((ToNode as IMetaNode).MetaObject.IsInDatabase(Core.Variables.Instance.ClientProvider))))
                        {
                            bool r = false;
                            List<Tuple> associationValues = new List<Tuple>(); ;
                            dbAssociation = LinkController.GetAssociation((FromNode as IMetaNode).MetaObject, (ToNode as IMetaNode).MetaObject, AssociationType);
                            if (dbAssociation != null)
                            {
                                r = true;
                            }
                            else
                            {
                                dbAssociation = LinkController.SaveAssociation((FromNode as IMetaNode).MetaObject, (ToNode as IMetaNode).MetaObject, AssociationType, Core.Variables.Instance.ClientProvider);
                                r = dbAssociation != null;
                            }

                            associationValues.Add(new Tuple("GapType", this.GapType.ToString()));

                            #region Association Values
                            if (r) //association in database?
                            {
                                try
                                {
                                    if (Pen.Color.ToArgb() == Color.Orange.ToArgb())
                                    {
                                        Pen p = new Pen(PenColorBeforeCompare, Pen.Width);

                                        if (PenColorBeforeCompare.ToArgb() == Color.Orange.ToArgb())// && !lnk.IsInDatabase)
                                            p = new Pen(Color.Black, Pen.Width);

                                        if (AssociationType == LinkAssociationType.SubSetOf || AssociationType == LinkAssociationType.MutuallyExclusiveLink)
                                            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

                                        Pen = p;
                                    }
                                }
                                catch (Exception xxPen)
                                {
                                    Console.WriteLine("Pen Error in QLink::IsInDatabase" + xxPen.ToString());
                                }

                                try
                                {
                                    foreach (Tuple t in associationValues)
                                    {
                                        //get current database value for oa
                                        System.Data.DataSet values = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ExecuteDataSet("PROC_ObjectAssociationValue_GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine", new object[] { dbAssociation.CAid.ToString(), dbAssociation.ObjectID.ToString(), dbAssociation.ChildObjectID.ToString(), dbAssociation.ObjectMachine.ToString(), dbAssociation.ChildObjectMachine.ToString() });
                                        if (values.Tables[0].Rows.Count >= 1) //UPDATE
                                        {
                                            //UPDATE
                                            DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ExecuteNonQuery("[PROC_ObjectAssociationValue_Update]", new object[] { dbAssociation.CAid.ToString(), dbAssociation.ObjectID.ToString(), dbAssociation.ChildObjectID.ToString(), dbAssociation.ObjectMachine.ToString(), dbAssociation.ChildObjectMachine.ToString(), t.First.ToString(), t.Second.ToString() });
                                        }
                                        else
                                        {
                                            if (t.Second.ToString() != "None")
                                            {
                                                //INSERT
                                                DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ExecuteNonQuery("[PROC_ObjectAssociationValue_Insert]", new object[] { dbAssociation.CAid.ToString(), dbAssociation.ObjectID.ToString(), dbAssociation.ChildObjectID.ToString(), dbAssociation.ObjectMachine.ToString(), dbAssociation.ChildObjectMachine.ToString(), t.First.ToString(), t.Second.ToString() });
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            #endregion

                            return r;
                        }
                    }
                }
                return false;

            }
        }

        //After we save and clear our database the color of the link will be this after we open and save the diagram again.
        //Or send the diagram to another person and they save
        private Color penColorBeforeCompare;
        public Color PenColorBeforeCompare
        {
            get
            {
                return penColorBeforeCompare;
            }
            set
            {
                penColorBeforeCompare = value;
            }
        }

        [NonSerialized]
        public bool NotInModel = false;

        //property used to skip the auto save of an association when opening a diagram

        [NonSerialized]
        private bool transforming;
        public bool Transforming
        {
            get { return transforming; }
            set { transforming = value; }
        }

        #region Meta Related Properties

        private LinkAssociationType associationType;
        [Browsable(false)]
        public LinkAssociationType AssociationType
        {
            get { return associationType; }
            set
            {
                LinkAssociationType oldVal = associationType;

                if (value == 0)
                {
                    Core.Log.WriteLog("Associationtype(OldValue:" + oldVal.ToString() + ") is being set to 0" + Environment.NewLine + Environment.StackTrace);
                }

                associationType = value;
                if (oldVal != value)
                {
                    ChangedLinkType();
                    if (IsInDatabase)
                    {
                    }
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
                        return ((IMetaNode)ToPort.GoObject.ParentNode).BindingInfo.BindingClass;
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

        //10 January 2013 ShallowCopyObjects
        private MetaBase toMetaBase;
        public MetaBase ToMetaBase
        {
            get { return toMetaBase; }
            set { toMetaBase = value; }
        }
        public IGoPort ToPortShallow; //set to shallow copy of node's port == current port
        private MetaBase fromMetaBase;
        public MetaBase FromMetaBase
        {
            get { return fromMetaBase; }
            set { fromMetaBase = value; }
        }
        public IGoPort FromPortShallow; //set to shallow copy of node's port == current port
        #endregion

        [NonSerialized]
        private bool automatedAddition = false;
        public bool AutomatedAddition
        {
            get { return automatedAddition; }
            set { automatedAddition = value; }
        }

        #region Helper Methods
        public List<IMetaNode> GetArtefacts()
        {
            List<IMetaNode> artefactNodes = new List<IMetaNode>();
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
                                if (portenum.Current.FromNode is IMetaNode)
                                {
                                    artefactNodes.Add(portenum.Current.FromNode as IMetaNode);
                                }
                            }
                        }
                    }
                }
            }
            return artefactNodes;
        }
        public List<ContextNode> GetContextArtefacts()
        {
            List<ContextNode> artefactNodes = new List<ContextNode>();
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
                                if (portenum.Current.FromNode is ContextNode)
                                {
                                    artefactNodes.Add(portenum.Current.FromNode as ContextNode);
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
            GoGroup grp = new GoGroup();
            grp.Add(p);
            this.MidLabel = grp;  // add the FishLinkPort to the FishLink
            RealLink.Orthogonal = true;
            RealLink.Style = GoStrokeStyle.RoundedLineWithJumpOvers;
            RealLink.AdjustingStyle = GoLinkAdjustingStyle.Calculate;

            //ChangedLinkType();
            PenColorBeforeCompare = Color.Black;

            ////BREAKS LINKS ?
            //this.AvoidsNodes = true;
            //RealLink.AvoidsNodes = true;

            InferenceType = InferenceType.None;
        }

        public static QLink CreateLink(GoNode n, GoNode nChild, int associationTypeID, int CAID)
        {
            if (associationTypeID <= 0 && CAID > 0)
            {
                associationTypeID = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByCAid(CAID).AssociationTypeID;
            }

            if (nChild.ParentNode == n)
                return null;

            MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation defaultFrom = (MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation)Enum.Parse(typeof(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation), Core.Variables.Instance.DefaultFromPort);
            MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation defaultTo = (MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation)Enum.Parse(typeof(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation), Core.Variables.Instance.DefaultToPort);

            GoPort prtFrom = QuickPortHelper.ReturnPort(defaultFrom, n);
            GoPort prtTo = QuickPortHelper.ReturnPort(defaultTo, nChild);

            if (prtFrom == null || prtTo == null)
                return null;
            QLink l = new QLink();
            l.FromPort = prtFrom;
            l.ToPort = prtTo;
            l.AssociationType = (LinkAssociationType)associationTypeID;
            return l;
        }

        //public static QLink CreateLink(GoNode n, GoNode nChild, int associationTypeID)
        //{
        //    if (nChild.ParentNode == n)
        //        return null;

        //    GoPort prtFrom = QuickPortHelper.ReturnPort(QuickPortHelper.QuickPortLocation.Bottom, n);
        //    GoPort prtTo = QuickPortHelper.ReturnPort(QuickPortHelper.QuickPortLocation.Left, nChild);

        //    QLink l = new QLink();
        //    l.FromPort = prtFrom;
        //    l.ToPort = prtTo;
        //    l.AssociationType = (LinkAssociationType)associationTypeID;
        //    return l;
        //}

        public static QLink CreateLink(GoNode n, GoNode nChild, int associationTypeID, QuickPortHelper.QuickPortLocation fromLocation, QuickPortHelper.QuickPortLocation toLocation)
        {
            if (nChild.ParentNode == n)
                return null;

            GoPort prtFrom = QuickPortHelper.ReturnPort(fromLocation, n);
            GoPort prtTo = QuickPortHelper.ReturnPort(toLocation, nChild);

            QLink l = new QLink();
            l.FromPort = prtFrom;
            l.ToPort = prtTo;
            l.AssociationType = (LinkAssociationType)associationTypeID;

            return l;
        }

        public static QLink CreateLink(GoNode n, GoNode nChild, int associationTypeID, GoPort prtFrom, GoPort prtTo)
        {
            if (nChild.ParentNode == n)
                return null;

            QLink l = new QLink();
            l.FromPort = prtFrom;
            l.ToPort = prtTo;
            l.AssociationType = (LinkAssociationType)associationTypeID;

            return l;
        }

        public static void MoveLink(GoNode parent, GoNode child, QuickPortHelper.QuickPortLocation fromLocation, QuickPortHelper.QuickPortLocation toLocation, QLink link)
        {
            GoPort prtFrom = QuickPortHelper.ReturnPort(fromLocation, parent);
            GoPort prtTo = QuickPortHelper.ReturnPort(toLocation, child);
            if (prtFrom != null && prtTo != null)
            {
                link.FromPort = prtFrom;
                link.ToPort = prtTo;
            }
        }
        public static void MoveLink(GoNode parent, GoNode child, GoPort prtFrom, GoPort prtTo, QLink link)
        {
            if (prtFrom != null && prtTo != null)
            {
                link.FromPort = prtFrom;
                link.ToPort = prtTo;
            }
        }

        // the FishLink group uses a FishRealLink instead of a regular GoLink
        public override GoLink CreateRealLink()
        {
            return new QRealLink();
        }

        #region ArrowHeads and such

        public void ChangedLinkType()
        {
            NotInModel = false;
            this.Initializing = true;
            RealLink.FromArrow = false;
            RealLink.ToArrow = false;

            (this.RealLink as QRealLink).calculated = false;
            if ((this.RealLink as QRealLink).toGroup != null)
            {
                (this.RealLink as QRealLink).toGroup.Remove();
            }
            if ((this.RealLink as QRealLink).fromGroup != null)
            {
                (this.RealLink as QRealLink).fromGroup.Remove();
            }

            if (MidLabel != null)
                MidLabel.Remove();
            if (ToLabel != null)
                ToLabel.Remove();
            if (FromLabel != null)
                FromLabel.Remove();

            if (MidLabel == null)
                MidLabel = new GoGroup();
            if (ToLabel == null)
                ToLabel = new GoGroup();
            if (FromLabel == null)
                FromLabel = new GoGroup();

            this.ToArrowLength = 13;
            this.ToArrowShaftLength = 12;
            this.ToArrowWidth = 13;
            this.ToArrowStyle = GoStrokeArrowheadStyle.Polygon;

            Color c = Color.Black;
            if (!(PenColorBeforeCompare.IsEmpty))
                c = PenColorBeforeCompare;

            RealLink.Brush = Brushes.White;//new SolidBrush(Color.White);
            Pen pReset = new Pen(c, 1f);
            pReset.DashStyle = DashStyle.Solid;

            //RealLink.Pen = pReset;

            RemoveCustomMidGroup();

            switch (associationType)
            {
                case LinkAssociationType.Auxiliary:
                    SetLabels(false, false, true);
                    break;
                case LinkAssociationType.Classification:
                    SetLabels(false, false, true);
                    break;
                case LinkAssociationType.ConnectedTo:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    //SetLabels(false, false, false);
                    ToArrowWidth = 10;
                    FromArrowWidth = 10;
                    ToArrowLength = 10;
                    FromArrowLength = 10;
                    ToArrowShaftLength = 8;
                    FromArrowShaftLength = 8;
                    ToArrow = true;
                    FromArrow = true;
                    break;
                case LinkAssociationType.Create:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    SetLabels(false, true, false);
                    this.ToArrow = true;
                    break;
                case LinkAssociationType.Use:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    SetLabels(false, true, false);
                    this.ToArrow = true;
                    break;
                case LinkAssociationType.Decomposition:
                    RealLink.ToArrowStyle = GoStrokeArrowheadStyle.Polygon;
                    RealLink.ToArrowLength = 0;
                    RealLink.ToArrowWidth = 13;
                    RealLink.ToArrowShaftLength = 10;
                    RealLink.ToArrow = true;
                    break;
                case LinkAssociationType.Delete:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    SetLabels(false, true, false);
                    this.ToArrow = true;
                    break;
                case LinkAssociationType.Interrupt:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    SetLabels(true, false, false);
                    this.ToArrow = true;
                    break;
                case LinkAssociationType.LeadsTo:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    break;
                case LinkAssociationType.LocatedAt:
                    break;
                case LinkAssociationType.Maintain:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    SetLabels(false, true, false);
                    this.ToArrow = true;
                    break;
                case LinkAssociationType.Mapping:
                    ToArrow = false;
                    FromArrow = false;
                    SetLabels(true, false, true);
                    break;
                case LinkAssociationType.MutuallyExclusiveLink:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    //Pen p = new Pen(c, 2f);
                    pReset.DashStyle = DashStyle.Dot;
                    //RealLink.Pen = p;
                    RealLink.ToArrow = false;
                    break;
                case LinkAssociationType.OneWayCurved:
                    break;
                case LinkAssociationType.Read:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    SetLabels(false, true, false);
                    this.ToArrow = true;
                    break;
                case LinkAssociationType.Resume:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    SetLabels(true, false, false);
                    this.ToArrow = true;
                    break;
                case LinkAssociationType.Start:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    SetLabels(true, false, false);
                    this.ToArrow = true;
                    break;
                case LinkAssociationType.Stop:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    SetLabels(true, false, false);
                    this.ToArrow = true;
                    break;
                case LinkAssociationType.SubSetOf:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    //Pen pss = new Pen(c, 2f);
                    pReset.DashStyle = DashStyle.Dot;
                    //RealLink.Pen = pss;
                    RealLink.ToArrow = true;
                    try
                    {
                        if (RealLink.FromNode != null)
                        {
                            if ((RealLink.FromNode as IMetaNode).MetaObject.Get("IsCompleteSet") != null)
                            {
                                if ((RealLink.FromNode as IMetaNode).MetaObject.Get("IsCompleteSet").ToString().ToLower() == "no")
                                {
                                    RealLink.FromArrowStyle = GoStrokeArrowheadStyle.Cross;
                                    RealLink.FromArrow = true;
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                    break;
                case LinkAssociationType.Suspend:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    SetLabels(true, false, false);
                    this.ToArrow = true;
                    break;
                case LinkAssociationType.Concurrent:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    this.ToArrow = true;
                    SetLabels(false, true, false);
                    break;
                case LinkAssociationType.NonConcurrent:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    this.ToArrow = true;
                    SetLabels(false, true, false);
                    break;
                case LinkAssociationType.Synchronise:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    this.ToArrow = true;
                    SetLabels(false, true, false);
                    break;
                case LinkAssociationType.Update:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    SetLabels(false, true, false);
                    this.ToArrow = true;
                    break;
                case LinkAssociationType.Many_To_Many:
                    this.ToArrowStyle = GoStrokeArrowheadStyle.Cross;
                    this.ToArrowShaftLength = 0;
                    this.ToArrowLength = 13;
                    this.ToArrow = false;
                    this.FromArrowStyle = GoStrokeArrowheadStyle.Cross;
                    this.FromArrowShaftLength = 0;
                    this.FromArrowLength = 13;
                    this.FromArrow = true;
                    break;
                case LinkAssociationType.One_To_Many:
                    this.ToArrowStyle = GoStrokeArrowheadStyle.Cross;
                    this.ToArrowShaftLength = 0;
                    this.ToArrowLength = 13;
                    this.ToArrow = false;
                    this.FromArrowStyle = GoStrokeArrowheadStyle.Cross;
                    this.FromArrowShaftLength = 0;
                    this.FromArrowLength = 13;
                    this.FromArrow = true;
                    break;
                case LinkAssociationType.One_To_One:
                    this.ToArrowStyle = GoStrokeArrowheadStyle.Cross;
                    this.ToArrowShaftLength = 0;
                    this.ToArrowLength = 13;
                    this.ToArrow = false;
                    this.FromArrowStyle = GoStrokeArrowheadStyle.Cross;
                    this.FromArrowShaftLength = 0;
                    this.FromArrowLength = 13;
                    this.FromArrow = true;
                    break;
                case LinkAssociationType.Zero_To_One:
                    this.ToArrowStyle = GoStrokeArrowheadStyle.Cross;
                    this.ToArrowLength = 13;
                    this.ToArrowShaftLength = 0;
                    this.ToArrowWidth = 13;
                    this.ToArrow = false;
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
                    this.ToArrow = false;
                    this.FromArrowStyle = GoStrokeArrowheadStyle.Cross;
                    this.FromArrowShaftLength = 0;
                    this.FromArrowLength = 13;
                    this.FromArrow = true;
                    break;
                //case LinkAssociationType.Dependency:
                //case LinkAssociationType.DynamicFlow:
                //    this.ToArrowStyle = GoStrokeArrowheadStyle.Polygon;
                //    RealLink.ToArrow = true;
                //    RealLink.Brush = new SolidBrush(Color.Black);
                //    break;
                case 0:
                    RealLink.ToArrow = false;
                    RealLink.FromArrow = false;
                    RealLink.Brush = Brushes.DarkRed;//new SolidBrush(Color.DarkRed);
                    RealLink.Pen = new Pen(Color.DarkGray, 5);
                    Core.Log.WriteLog(Environment.StackTrace);
                    break;
                default:
                    //Core.Log.WriteLog(associationType.ToString() + " not found defaulting to basic link brush");
                    // case LinkAssociationType.Dependencies: 
                    // case LinkAssociationType.DynamicFlow
                    // Both the same.
                    RealLink.ToArrow = true;
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    break;
            }

            if (ToLabel != null)
                ToLabel.Selectable = false;
            if (FromLabel != null)
                FromLabel.Selectable = false;
            if (MidLabel != null)
                MidLabel.Selectable = false;

            Pen = pReset;
            RealLink.Pen = pReset;

            RecreateFishPortOnMidLabel();

            this.Initializing = false;

            LayoutChildren(this);
        }

        public void RemoveCustomMidGroup()
        {
            if (midGroup != null)
            {
                midGroup.Remove();
                midGroup = null;
            }
        }
        private FishLinkPort flp;
        public FishLinkPort GetFishLinkPort
        {
            get
            {
                if (flp == null)
                    RecreateFishPortOnMidLabel();
                return flp;
            }
        }
        private void RecreateFishPortOnMidLabel()
        {
            //GoGroup grpMid;
            bool found = false;
            if (this.MidLabel is GoGroup)
            {
                //grpMid = MidLabel as GoGroup;

                foreach (GoObject oChild in (MidLabel as GoGroup))
                {
                    if (oChild is FishLinkPort)
                        found = true;
                }
            }
            else
                MidLabel = new GoGroup();

            if (!found)
            {
                if (flp == null)
                {
                    FishLinkPort p = new FishLinkPort();
                    p.PortObject = this.RealLink;
                    (MidLabel as GoGroup).Add(p);
                    flp = p;
                }
                else
                {
                    flp.Remove();
                    flp.PortObject = this.RealLink;
                    (MidLabel as GoGroup).Add(flp);
                }
                //MidLabel = grpMid;
            }
        }

        [NonSerialized]
        public GoGroup midGroup;

        private void SetLabels(bool from, bool mid, bool to)
        {
            if (from)
                FromLabel = GetLabel();

            if (mid)
            {
                midGroup = GetLabel();
                midGroup.Selectable = false;
                midGroup.Copyable = false;
                midGroup.Location = new PointF(MidLabel.Location.X - (midGroup.Width / 2), MidLabel.Location.Y - (midGroup.Height / 2));
                Add(midGroup);
            }

            if (to)
                ToLabel = GetLabel();
        }

        private GoGroup GetLabel()
        {
            ArrowBuilder arrBuilder = new ArrowBuilder();
            return arrBuilder.CreateLabel(this.associationType);
        }

        public QLink ToQLink
        {
            get
            {
                if (this.ToPort is FishNodePort)
                {
                    FishNodePort fnp = this.ToPort as FishNodePort;
                    QLink lnk = fnp.Parent.Parent as QLink; // 1st parent is the group
                    return lnk;
                }
                return null;
            }
        }

        #endregion

        #region Misc

        public void Enable_AutoRouting(object sender, EventArgs e)
        {
            AvoidsNodes = true;
            RealLink.AvoidsNodes = true;
            RealLink.AdjustingStyle = GoLinkAdjustingStyle.Calculate;
        }
        public void Disable_AutoRouting(object sender, EventArgs e)
        {
            AvoidsNodes = false;
            RealLink.AvoidsNodes = false;
            RealLink.AdjustingStyle = GoLinkAdjustingStyle.End;
        }

        #endregion

        public void StoreDeletable(TList<ClassAssociation> classAssocs, string provider)
        {
            Pen newPen = new Pen(Pen.Color, Pen.Width);
            newPen.DashStyle = Pen.DashStyle;
            if (ToNode != null && FromNode != null)
            {
                if (ToNode is IMetaNode && FromNode is IMetaNode)
                {
                    IMetaNode fromGraphNode = FromNode as IMetaNode;
                    IMetaNode toGraphNode = ToNode as IMetaNode;
                    // check for ObjectAssociation
                    MetaBase mbFrom = fromGraphNode.MetaObject;
                    MetaBase mbTo = toGraphNode.MetaObject;
                    if (mbFrom.MachineName != null && mbTo.MachineName != null && mbFrom.pkid > 0 && mbTo.pkid > 0)
                    {
                        foreach (ClassAssociation classAssoc in classAssocs)
                        {
                            if (classAssoc.ParentClass == mbFrom._ClassName && classAssoc.AssociationTypeID == (int)AssociationType && classAssoc.ChildClass == mbTo._ClassName)
                            {
                                ObjectAssociation oassoc = DataRepository.Connections[provider].Provider.ObjectAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(classAssoc.CAid, mbFrom.pkid, mbTo.pkid, mbFrom.MachineName, mbTo.MachineName);
                                if (oassoc != null)
                                {
                                    Deletable = VCStatusTool.DeletableFromDiagram(oassoc);
                                    if (VCStatusTool.IsObsoleteOrMarkedForDelete(oassoc))
                                    {
                                        newPen = new Pen(Color.Red, 2.3f);
                                    }
                                    else
                                    {
                                        if (VCStatusTool.UserHasControl(oassoc))
                                        {
                                            newPen = new Pen(Color.Black, Pen.Width);
                                        }
                                        else
                                        {
                                            newPen = new Pen(Color.Gray, Pen.Width);
                                        }
                                    }
                                }
                                else
                                {
                                    //this will never happen because context is generated from the database...
                                    newPen = new Pen(Color.Orange, 2.3f);
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        //this will never happen because context is generated from the database...
                        newPen = new Pen(Color.Orange, 2.3f);
                    }
                }
            }
            Pen = newPen;
            //Pen.DashStyle = GetDashStyle();
        }

        #region Overrides

        private void positionCustomMidLabel()
        {
            if (midGroup != null && MidLabel != null)
            {
                midGroup.Location = new PointF(MidLabel.Location.X - (midGroup.Width / 2), MidLabel.Location.Y - (midGroup.Height / 2));
            }
        }

        public override void Changed(int subhint, int oldI, object oldVal, RectangleF oldRect, int newI, object newVal, RectangleF newRect)
        {
            positionCustomMidLabel();
            base.Changed(subhint, oldI, oldVal, oldRect, newI, newVal, newRect);
        }

        //public override IGoPort ToPort
        //{
        //    get
        //    {
        //        return base.ToPort;
        //    }
        //    set
        //    {
        //        base.ToPort = value;
        //    }
        //}

        //public override IGoPort FromPort
        //{
        //    get
        //    {
        //        return base.FromPort;
        //    }
        //    set
        //    {
        //        base.FromPort = value;
        //    }
        //}

        public override string GetToolTip(GoView view)
        {
            if (NotInModel)
                return this.associationType.ToString() + " - META MODEL DOES NOT ALLOW";
            else if (associationType == 0)
                return "[BUG]This association has been created with an invalid link type." + Environment.NewLine + "Please right click on it and change the association to resolve.";
            else
                return this.associationType.ToString();
        }

        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;
                //make connected objects == to this visibility
                //convert midlabel to port if not null get fishlink and make artifact invisible
                FishLinkPort flp = null;
                foreach (GoObject o in (this.MidLabel as GoGroup))
                {
                    if (o is FishLinkPort)
                    {
                        flp = o as FishLinkPort;
                        break;
                    }
                }

                if (flp != null && flp.SourceLinksCount > 0)
                {
                    foreach (FishLink l in flp.SourceLinks)
                    {
                        l.Visible = value;
                        l.RealLink.Visible = value;
                        if (l.FromNode is ArtefactNode)
                            (l.FromNode as ArtefactNode).Visible = value;
                    }
                }
            }
        }
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

        public override GoObject CopyObject(GoCopyDictionary env)
        {
            GoObject o = base.CopyObject(env);

            foreach (GoObject ob in (o as QLink))
            {
                if (ob is GoGroup)
                {
                    //we want to only remove the custom mid group but cannot reference it so we have to 'guess' which group to remove
                    if ((ob as GoGroup).Last is FishLinkPort) //custom group does not contain this port
                        continue;
                    if ((ob as GoGroup).Last == null) //custom group will have at least 1 object
                        continue;

                    if (AssociationType == LinkAssociationType.Mapping)
                        continue;

                    ob.Remove();
                }
                //else
                //{
                //    //System.Diagnostics.Debug.WriteLine(ob.ToString());
                //}
            }

            //if ((o as QLink).midGroup != null)
            //{
            //    GoGroup g = new GoGroup();
            //    g.Selectable = false;
            //    g.Copyable = false;
            //    g.Editable = false;
            //    g.DragsNode = true;
            //    foreach (GoObject obj in midGroup)
            //    {
            //        g.Add(obj.Copy());
            //    }
            //    (o as QLink).midGroup = g;
            //    (o as QLink).Add(g);
            //    (o as QLink).LayoutMidLabel(g);
            //}
            if (associationType != 0)
            {
                this.ChangeLinkTypeButDontChangeMidGroup();
                (o as QLink).ChangeLinkTypeButDontChangeMidGroup();
            }

            //this.ChangedLinkType();
            //(o as QLink).ChangedLinkType();

            //((this as QLink).RealLink as QRealLink).ForceCalculate();// = false;
            //((o as QLink).RealLink as QRealLink).ForceCalculate();// = false;

            return o;
        }

        private void ChangeLinkTypeButDontChangeMidGroup() //this is really bad becuase we also need to change midgroup actually lol
        {
            NotInModel = false;
            this.Initializing = true;
            RealLink.FromArrow = false;
            RealLink.ToArrow = false;

            (this.RealLink as QRealLink).calculated = false;
            if ((this.RealLink as QRealLink).toGroup != null)
            {
                (this.RealLink as QRealLink).toGroup.Remove();
            }
            if ((this.RealLink as QRealLink).fromGroup != null)
            {
                (this.RealLink as QRealLink).fromGroup.Remove();
            }

            if (ToLabel != null)
                ToLabel.Remove();
            if (FromLabel != null)
                FromLabel.Remove();

            if (ToLabel == null)
                ToLabel = new GoGroup();
            if (FromLabel == null)
                FromLabel = new GoGroup();

            this.ToArrowLength = 13;
            this.ToArrowShaftLength = 12;
            this.ToArrowWidth = 13;
            this.ToArrowStyle = GoStrokeArrowheadStyle.Polygon;

            Color c = Color.Black;
            if (!(PenColorBeforeCompare.IsEmpty))
                c = PenColorBeforeCompare;

            RealLink.Brush = Brushes.White;//new SolidBrush(Color.White);
            Pen pReset = new Pen(c, 1f);
            pReset.DashStyle = DashStyle.Solid;

            RemoveCustomMidGroup();

            switch (associationType)
            {
                case LinkAssociationType.Auxiliary:
                    SetLabels(false, false, true);
                    break;
                case LinkAssociationType.Classification:
                    SetLabels(false, false, true);
                    break;
                case LinkAssociationType.ConnectedTo:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    //SetLabels(false, false, false);
                    ToArrowWidth = 10;
                    FromArrowWidth = 10;
                    ToArrowLength = 10;
                    FromArrowLength = 10;
                    ToArrowShaftLength = 8;
                    FromArrowShaftLength = 8;
                    ToArrow = true;
                    FromArrow = true;
                    break;
                case LinkAssociationType.Create:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    SetLabels(false, true, false);
                    this.ToArrow = true;
                    break;
                case LinkAssociationType.Use:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    SetLabels(false, true, false);
                    this.ToArrow = true;
                    break;
                case LinkAssociationType.Decomposition:
                    RealLink.ToArrowStyle = GoStrokeArrowheadStyle.Polygon;
                    RealLink.ToArrowLength = 0;
                    RealLink.ToArrowWidth = 13;
                    RealLink.ToArrowShaftLength = 10;
                    RealLink.ToArrow = true;
                    break;
                case LinkAssociationType.Delete:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    SetLabels(false, true, false);
                    this.ToArrow = true;
                    break;
                case LinkAssociationType.Interrupt:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    SetLabels(true, false, false);
                    this.ToArrow = true;
                    break;
                case LinkAssociationType.LeadsTo:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    break;
                case LinkAssociationType.LocatedAt:
                    break;
                case LinkAssociationType.Maintain:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    SetLabels(false, true, false);
                    this.ToArrow = true;
                    break;
                case LinkAssociationType.Mapping:
                    ToArrow = false;
                    FromArrow = false;
                    SetLabels(true, false, true);
                    break;
                case LinkAssociationType.MutuallyExclusiveLink:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    //Pen p = new Pen(c, 2f);
                    pReset.DashStyle = DashStyle.Dot;
                    //RealLink.Pen = p;
                    RealLink.ToArrow = false;
                    break;
                case LinkAssociationType.OneWayCurved:
                    break;
                case LinkAssociationType.Read:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    SetLabels(false, true, false);
                    this.ToArrow = true;
                    break;
                case LinkAssociationType.Resume:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    SetLabels(true, false, false);
                    this.ToArrow = true;
                    break;
                case LinkAssociationType.Start:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    SetLabels(true, false, false);
                    this.ToArrow = true;
                    break;
                case LinkAssociationType.Stop:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    SetLabels(true, false, false);
                    this.ToArrow = true;
                    break;
                case LinkAssociationType.SubSetOf:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    //Pen pss = new Pen(c, 2f);
                    pReset.DashStyle = DashStyle.Dot;
                    //RealLink.Pen = pss;
                    RealLink.ToArrow = true;
                    try
                    {
                        if (RealLink.FromNode != null)
                        {
                            if ((RealLink.FromNode as IMetaNode).MetaObject.Get("IsCompleteSet") != null)
                            {
                                if ((RealLink.FromNode as IMetaNode).MetaObject.Get("IsCompleteSet").ToString().ToLower() == "no")
                                {
                                    RealLink.FromArrowStyle = GoStrokeArrowheadStyle.Cross;
                                    RealLink.FromArrow = true;
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                    break;
                case LinkAssociationType.Suspend:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    SetLabels(true, false, false);
                    this.ToArrow = true;
                    break;
                case LinkAssociationType.Concurrent:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    this.ToArrow = true;
                    SetLabels(false, true, false);
                    break;
                case LinkAssociationType.NonConcurrent:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    this.ToArrow = true;
                    SetLabels(false, true, false);
                    break;
                case LinkAssociationType.Synchronise:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    this.ToArrow = true;
                    SetLabels(false, true, false);
                    break;
                case LinkAssociationType.Update:
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    SetLabels(false, true, false);
                    this.ToArrow = true;
                    break;
                case LinkAssociationType.Many_To_Many:
                    this.ToArrowStyle = GoStrokeArrowheadStyle.Cross;
                    this.ToArrowShaftLength = 0;
                    this.ToArrowLength = 13;
                    this.ToArrow = false;
                    this.FromArrowStyle = GoStrokeArrowheadStyle.Cross;
                    this.FromArrowShaftLength = 0;
                    this.FromArrowLength = 13;
                    this.FromArrow = true;
                    break;
                case LinkAssociationType.One_To_Many:
                    this.ToArrowStyle = GoStrokeArrowheadStyle.Cross;
                    this.ToArrowShaftLength = 0;
                    this.ToArrowLength = 13;
                    this.ToArrow = false;
                    this.FromArrowStyle = GoStrokeArrowheadStyle.Cross;
                    this.FromArrowShaftLength = 0;
                    this.FromArrowLength = 13;
                    this.FromArrow = true;
                    break;
                case LinkAssociationType.One_To_One:
                    this.ToArrowStyle = GoStrokeArrowheadStyle.Cross;
                    this.ToArrowShaftLength = 0;
                    this.ToArrowLength = 13;
                    this.ToArrow = false;
                    this.FromArrowStyle = GoStrokeArrowheadStyle.Cross;
                    this.FromArrowShaftLength = 0;
                    this.FromArrowLength = 13;
                    this.FromArrow = true;
                    break;
                case LinkAssociationType.Zero_To_One:
                    this.ToArrowStyle = GoStrokeArrowheadStyle.Cross;
                    this.ToArrowLength = 13;
                    this.ToArrowShaftLength = 0;
                    this.ToArrowWidth = 13;
                    this.ToArrow = false;
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
                    this.ToArrow = false;
                    this.FromArrowStyle = GoStrokeArrowheadStyle.Cross;
                    this.FromArrowShaftLength = 0;
                    this.FromArrowLength = 13;
                    this.FromArrow = true;
                    break;
                //case LinkAssociationType.Dependency:
                //case LinkAssociationType.DynamicFlow:
                //    this.ToArrowStyle = GoStrokeArrowheadStyle.Polygon;
                //    RealLink.ToArrow = true;
                //    RealLink.Brush = new SolidBrush(Color.Black);
                //    break;
                default:
                    //Core.Log.WriteLog(associationType.ToString() + " not found defaulting to basic link brush");
                    // case LinkAssociationType.Dependencies: 
                    // case LinkAssociationType.DynamicFlow
                    // Both the same.
                    RealLink.ToArrow = true;
                    RealLink.Brush = Brushes.Black;//new SolidBrush(Color.Black);
                    break;
            }

            if (ToLabel != null)
                ToLabel.Selectable = false;
            if (FromLabel != null)
                FromLabel.Selectable = false;

            Pen = pReset;
            RealLink.Pen = pReset;

            this.Initializing = false;

            LayoutChildren(this);
        }

        //public override void CopyObjectDelayed(GoCopyDictionary env, GoObject newobj)
        //{
        //    base.CopyObjectDelayed(env, newobj);
        //}
        //protected override void CopyChildren(GoGroup newgroup, GoCopyDictionary env)
        //{
        //    base.CopyChildren(newgroup, env);
        //}
        public override void Changing(int subhint)
        {
            positionCustomMidLabel();
            base.Changing(subhint);
        }

        protected override void PositionMidLabel(GoObject lab, PointF a, PointF b)
        {
            base.PositionMidLabel(lab, a, b);
            lab.Size = new SizeF(0, 0);
            positionCustomMidLabel();
        }
        protected override void LayoutMidLabel(GoObject childchanged)
        {
            base.LayoutMidLabel(childchanged);
            positionCustomMidLabel();
        }

        //public override void DoMove(GoView view, PointF origLoc, PointF newLoc)
        //{
        //    base.DoMove(view, origLoc, newLoc);
        //}
        protected override void OnBoundsChanged(RectangleF old)
        {
            base.OnBoundsChanged(old);
            positionCustomMidLabel();
        }

        public override bool Shadowed
        {
            get
            {
                return false;
                //return base.Shadowed;
            }
            set
            {
                base.Shadowed = false;
                //base.Shadowed = value;
            }
        }

        #endregion

        [NonSerialized]
        private string shallowFromN;
        public string ShallowFromN
        {
            get { return shallowFromN; }
            set { shallowFromN = value; }
        }

        [NonSerialized]
        private string shallowToN;
        public string ShallowToN
        {
            get { return shallowToN; }
            set { shallowToN = value; }
        }

        [NonSerialized]
        public string fromPortLoc = "";

        [NonSerialized]
        public string toPortLoc = "";
    }
}