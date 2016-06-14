using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Meta;
using Northwoods.Go;
using ObjectAssociation = MetaBuilder.BusinessLogic.ObjectAssociation;
using d = MetaBuilder.DataAccessLayer;
using b = MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using System.Collections.ObjectModel;

namespace MetaBuilder.Graphing.Tools
{

    public class NewDragTool : GoToolDragging
    {
        #region Constructors (1)

        public NewDragTool(GraphView view)
            : base(view)
        {
        }

        #endregion Constructors
    }

    public class NewLinkTool : GoToolLinkingNew
    {
        #region Constructors (1)

        public NewLinkTool(GraphView view)
            : base(view)
        {
        }

        #endregion Constructors

        #region Methods (2)

        public override void DoNewLink(IGoPort fromPort, IGoPort toPort)
        {
            try
            {
                if (toPort.Node is CollapsingRecordNodeItem)
                {
                    if (!(((CollapsingRecordNodeItem)(toPort.Node)).IsHeader))
                        base.DoNewLink(fromPort, toPort);
                }
                else
                    base.DoNewLink(fromPort, toPort);
            }
            catch
            {
            }
        }

        public override bool IsValidLink(IGoPort fromPort, IGoPort toPort)
        {
            //if (fromPort.Node is MindmapNode || toPort.Node is MindmapNode)
            //{
            //    return false;
            //}

            if (fromPort is FishNodePort || fromPort is FishLinkPort)
            {
                if (toPort is QuickPort)
                {
                    return false;
                }
            }
            else if (fromPort is QuickPort)
            {
                if (toPort is FishNodePort || toPort is FishLinkPort)
                {
                    int allowed = -1;
                    IMetaNode fromNode = fromPort.Node as IMetaNode;
                    if (fromNode != null)
                    {
                        if (toPort.GoObject.Parent is FishLink) //Why do fishlinks have midlabels and HOLD ON NEW FEATURE TIME!?
                            return false;

                        if (toPort.GoObject.Parent.Parent == null) //Why do fishlinks have midlabels and HOLD ON NEW FEATURE TIME!?
                            return false;

                        IMetaNode parentNode = (toPort.GoObject.Parent.Parent as QLink).FromNode as IMetaNode;
                        if (parentNode != null)
                        {
                            IMetaNode childNode = (toPort.GoObject.Parent.Parent as QLink).ToNode as IMetaNode;
                            if (childNode != null)
                            {
                                AssociationHelper assHelper = Singletons.GetAssociationHelper();
                                allowed = assHelper.CheckForValidArtifact(parentNode.MetaObject.Class, childNode.MetaObject.Class, fromNode.MetaObject.Class, (int)(toPort.GoObject.Parent.Parent as QLink).AssociationType);
                            }
                        }
                    }
                    return allowed > 0;
                }
            }

            if (fromPort.Node is IMetaNode && toPort.Node is IMetaNode)
            {
                IMetaNode nodeFrom = fromPort.Node as IMetaNode;
                IMetaNode nodeTo = toPort.Node as IMetaNode;
                if (nodeFrom.MetaObject != null && nodeTo.MetaObject != null)
                {
                    //if (nodeFrom.MetaObject.pkid > 0 && nodeTo.MetaObject.pkid > 0) //BECAUSE NEW OBJECTS HAVE NO PKID
                    //{
                    if (VCStatusTool.IsObsoleteOrMarkedForDelete(nodeFrom.MetaObject) || VCStatusTool.IsObsoleteOrMarkedForDelete(nodeTo.MetaObject))
                        return false;
                    //if (nodeTo.MetaObject.State == VCStatusList.CheckedOutRead && nodeFrom.MetaObject.State == VCStatusList.CheckedOutRead)
                    //    return false;
                    //if (nodeTo.MetaObject.State == VCStatusList.CheckedIn && nodeFrom.MetaObject.State == VCStatusList.CheckedIn)
                    //    return false;
                    //if (nodeTo.MetaObject.State == VCStatusList.Locked && nodeFrom.MetaObject.State == VCStatusList.Locked)
                    //    return false;

                    //}

                    if (nodeTo.MetaObject != null && nodeFrom.MetaObject != null) //WEVE ALREADY CHECKED THIS ABOVE? but we have to have a base return?
                    {
                        AssociationHelper assHelper = Singletons.GetAssociationHelper();
                        if (nodeTo.MetaObject == nodeFrom.MetaObject && nodeTo.MetaObject.Class == "Object" && nodeFrom.MetaObject.Class == "Object")
                        {
                            //ALWAYS BE DYNAMIC FLOW? 19
                            // AllowedAssociationInfo info = assHelper.GetDefaultAllowedAssociationInfo(nodeFrom.MetaObject._ClassName, nodeTo.MetaObject._ClassName);
                            return true;
                        }
                        else if (nodeTo.MetaObject == nodeFrom.MetaObject && nodeTo.MetaObject.Class == "DataTable" && nodeFrom.MetaObject.Class == "DataTable")
                        {
                            //ALWAYS BE DYNAMIC FLOW? 19
                            // AllowedAssociationInfo info = assHelper.GetDefaultAllowedAssociationInfo(nodeFrom.MetaObject._ClassName, nodeTo.MetaObject._ClassName);
                            return true;
                        }
                        else
                        {
                            AllowedAssociationInfo info = assHelper.GetDefaultAllowedAssociationInfo(nodeFrom.MetaObject._ClassName, nodeTo.MetaObject._ClassName);
                            if (info != null)
                            {
                                return base.IsValidLink(fromPort, toPort);
                            }
                        }
                        return false;
                    }
                    return base.IsValidLink(fromPort, toPort);
                }
            }

            try
            {
                if (fromPort is GoPort)
                {
                    GoPort quickFrom = fromPort as GoPort;
                    if (quickFrom.ParentNode is IMetaNode)
                    {
                        IMetaNode imFrom = quickFrom.ParentNode as IMetaNode;
                        if (toPort is FishLinkPort)
                        {
                            FishLinkPort fishTo = toPort as FishLinkPort;
                            QLink slink = fishTo.Parent.Parent as QLink;
                            if (slink != null)
                            {
                                AssociationHelper assHelper = Singletons.GetAssociationHelper();
                                int valid = assHelper.CheckForValidArtifact(slink.FromClass, slink.ToClass, imFrom.BindingInfo.BindingClass, (int)slink.AssociationType);

                                if (valid > 0)
                                {
                                    return base.IsValidLink(fromPort, toPort);
                                }
                                return false;
                            }
                            return false;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
            if (toPort == null)
                return false;

            return base.IsValidLink(fromPort, toPort);
        }

        #endregion Methods

    }

    public class ReLinkTool : GoToolRelinking
    {
        #region Fields (1)

        private LinkCancelEventArgs LinkArgs;

        #endregion Fields

        #region Constructors (1)

        public ReLinkTool(GraphView view)
            : base(view)
        {
        }

        #endregion Constructors

        #region Methods (4)

        // Public Methods (4) 

        public override void DoNoRelink(IGoLink oldlink, IGoPort fromPort, IGoPort toPort)
        {
            if (LinkArgs.Association != null)
            {
                View.RaiseSelectionDeleting(LinkArgs);
                if (LinkArgs.Cancel)
                {
                    if (LinkArgs.OldLink.IsInDocument)
                        base.DoRelink(oldlink, LinkArgs.OldFromPort, LinkArgs.OldToPort);
                }
                else
                {
                    base.DoNoRelink(oldlink, fromPort, toPort);
                }
            }
            else
                base.DoNoRelink(oldlink, fromPort, toPort);
        }

        bool internalLinking = false;
        public override void DoRelink(IGoLink oldlink, IGoPort fromPort, IGoPort toPort)
        {
            bool cancel = false;

            if (LinkArgs.Association != null && (fromPort == null || toPort == null))
            {
                if (VCStatusTool.IsObsoleteOrMarkedForDelete(LinkArgs.Association))
                {
                    cancel = true;
                    LinkArgs.Cancel = true;
                }
            }

            if (oldlink is QLink)
            {
                QLink slink = oldlink as QLink;
                /* QRealLink srealLink = slink.RealLink as QRealLink;
                srealLink.CalculateRoute();
                srealLink.DrawRelationshipArrows();*/
                GoPort fPort = fromPort as GoPort;
                GoPort tPort = toPort as GoPort;
                if (fPort.ParentNode is IMetaNode && tPort.ParentNode is IMetaNode)
                {
                    IMetaNode nFrom = null;
                    GoObject oFrom = fPort;
                    while (nFrom == null)
                    {
                        if (oFrom.Parent is IMetaNode)
                        {
                            nFrom = oFrom.Parent as IMetaNode;
                        }
                        else
                        {
                            oFrom = oFrom.Parent;
                        }
                    }
                    IMetaNode nTo = null;
                    GoObject oTo = tPort;
                    while (nTo == null)
                    {
                        if (oTo.Parent is IMetaNode)
                        {
                            nTo = oTo.Parent as IMetaNode;
                        }
                        else
                        {
                            oTo = oTo.Parent;
                        }
                    }

                    if (nFrom.HasBindingInfo && nTo.HasBindingInfo)
                    {
                        AssociationHelper assHelper = Singletons.GetAssociationHelper();
                        AllowedAssociationInfo info = assHelper.GetDefaultAllowedAssociationInfo(nFrom.MetaObject._ClassName, nTo.MetaObject._ClassName);
                        if (LinkArgs.OldFromNode != null && LinkArgs.OldToNode != null)
                        {
                            if (LinkArgs.OldFromNode.BindingInfo.BindingClass != nFrom.BindingInfo.BindingClass || LinkArgs.OldToNode.BindingInfo.BindingClass != nTo.BindingInfo.BindingClass)
                            {
                                slink.AssociationType = info.LinkAssociationType;

                                // 31 OCT slink.ChangeStyle();
                                /*if (slink.MidLabel!=null)
                                    slink.MidLabel.Remove();*/
                            }
                        }
                    }
                }

                #region relink multiple links fromport if they are the from the same node

                if (!internalLinking)//false == true &&
                {
                    internalLinking = true;
                    LinkController lcontroller = new LinkController();

                    bool to = false;
                    if (LinkArgs.OldFromPort == fromPort) //from port did not change therefore must be to port
                    {
                        to = true;
                    }

                    if (!cancel)
                    {
                        Collection<QLink> links = (View as GraphView).SelectedLinks;
                        foreach (QLink l in links)
                        {
                            QLink link = l;
                            if (link != slink)
                            {
                                if (to)
                                {
                                    //if (IsValidLink(link.FromPort, toPort))
                                    {
                                        //MFD old
                                        //ObjectAssociation assoc = lcontroller.GetAssociation(link, true);
                                        //if (assoc != null)
                                        //{
                                        //    if (VCStatusTool.DeletableFromDiagram(assoc) && VCStatusTool.UserHasControl(assoc))
                                        //    {
                                        //        assoc.VCStatusID = (int)VCStatusList.MarkedForDelete;
                                        //        assoc.State = VCStatusList.MarkedForDelete;
                                        //        DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Save(assoc);
                                        //    }
                                        //}
                                        //relink
                                        DoRelink(link, link.FromPort, toPort);
                                    }
                                    //else
                                    //{
                                    //    link.ToString();
                                    //}
                                }
                                else
                                {
                                    //if (IsValidLink(fromPort, link.ToPort))
                                    {
                                        //MFD old
                                        //ObjectAssociation assoc = lcontroller.GetAssociation(link, true);
                                        //if (assoc != null)
                                        //{
                                        //    if (VCStatusTool.DeletableFromDiagram(assoc) && VCStatusTool.UserHasControl(assoc))
                                        //    {
                                        //        assoc.VCStatusID = (int)VCStatusList.MarkedForDelete;
                                        //        assoc.State = VCStatusList.MarkedForDelete;
                                        //        DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Save(assoc);
                                        //    }
                                        //}
                                        //relink
                                        DoRelink(link, fromPort, link.ToPort);
                                    }
                                    //else
                                    //{
                                    //    link.ToString();
                                    //}
                                }

                            }
                        }
                    }

                    internalLinking = false;
                }

                #endregion

            }
            if (!cancel)
                base.DoRelink(oldlink, fromPort, toPort);
            else
                base.DoRelink(oldlink, LinkArgs.OldFromPort, LinkArgs.OldToPort);
        }

        public override bool IsValidLink(IGoPort fromPort, IGoPort toPort)
        {
            if (LinkArgs != null)
                if (LinkArgs.Cancel)
                {
                    return false;
                }
            if (fromPort == null || toPort == null)
                return false;

            if (fromPort.Node is IMetaNode && toPort.Node is IMetaNode)
            {
                IMetaNode nodeFrom = fromPort.Node as IMetaNode;
                IMetaNode nodeTo = toPort.Node as IMetaNode;
                if (nodeTo.MetaObject == nodeFrom.MetaObject && nodeTo.MetaObject.Class == "Object" && nodeFrom.MetaObject.Class == "Object")
                {
                    //AllowedAssociationInfo info = assHelper.GetDefaultAllowedAssociationInfo(nodeFrom.MetaObject._ClassName, nodeTo.MetaObject._ClassName);
                    return true;
                }
                else if (nodeTo.MetaObject == nodeFrom.MetaObject && nodeTo.MetaObject.Class == "DataTable" && nodeFrom.MetaObject.Class == "DataTable")
                {
                    //AllowedAssociationInfo info = assHelper.GetDefaultAllowedAssociationInfo(nodeFrom.MetaObject._ClassName, nodeTo.MetaObject._ClassName);
                    return true;
                }

                if (nodeFrom.MetaObject != null && nodeTo.MetaObject != null)
                {
                    if (VCStatusTool.IsObsoleteOrMarkedForDelete(nodeFrom.MetaObject) || VCStatusTool.IsObsoleteOrMarkedForDelete(nodeTo.MetaObject))
                        return false;
                    //if (nodeTo.MetaObject.State == VCStatusList.CheckedOutRead && nodeFrom.MetaObject.State == VCStatusList.CheckedOutRead)
                    //    return false;
                    //if (nodeTo.MetaObject.State == VCStatusList.CheckedOutRead && nodeFrom.MetaObject.State == VCStatusList.Locked)
                    //    return false;
                    List<ClassAssociation> allowedAssociations = AssociationManager.Instance.GetAssociationsForParentAndChildClasses(nodeFrom.BindingInfo.BindingClass, nodeTo.BindingInfo.BindingClass);
                    if (allowedAssociations.Count == 0)
                        return false;
                }
            }
            if (fromPort is FishNodePort || fromPort is FishLinkPort)
            {
                if (toPort is QuickPort)
                {
                    toPort.ToString();
                    //return false;
                }
            }

            if (fromPort is GoPort)
            {
                GoPort quickFrom = fromPort as GoPort;
                if (quickFrom.ParentNode is IMetaNode)
                {
                    IMetaNode imFrom = quickFrom.ParentNode as IMetaNode;
                    if (toPort is FishLinkPort)
                    {
                        FishLinkPort fishTo = toPort as FishLinkPort;
                        QLink slink = fishTo.Parent.Parent as QLink;
                        if (slink != null)
                        {
                            AssociationHelper assHelper = Singletons.GetAssociationHelper();
                            assHelper.GetDefaultAllowedAssociationInfo(slink.FromClass, slink.ToClass);
                            int valid = assHelper.CheckForValidArtifact(slink.FromClass, slink.ToClass, imFrom.BindingInfo.BindingClass, (int)slink.AssociationType);
                            if (valid > 0)
                            {
                                return base.IsValidLink(fromPort, toPort);
                            }
                        }
                        return false;
                    }
                }
            }
            if (fromPort.GetType().ToString().ToLower().IndexOf("tempor") > -1 || toPort.GetType().ToString().ToLower().IndexOf("tempor") > -1)
                return false;

            //if (internalLinking)
            //    return true;
            //else
            return base.IsValidLink(fromPort, toPort);
        }

        public override void StartRelink(IGoLink oldlink, bool forwards, PointF dc)
        {
            bool cancel = false;
            if (oldlink is QLink)
            {
                LinkArgs = new LinkCancelEventArgs();
                LinkArgs.OldLink = oldlink as QLink;
                LinkArgs.OldFromNode = LinkArgs.OldLink.FromNode as IMetaNode;
                LinkArgs.OldToNode = LinkArgs.OldLink.ToNode as IMetaNode;
                LinkArgs.OldToPort = oldlink.ToPort as GoPort;
                LinkArgs.OldFromPort = oldlink.FromPort as GoPort;
                if (LinkArgs.OldFromNode != null && LinkArgs.OldToNode != null)
                {
                    LinkController lcontroller = new LinkController();
                    LinkArgs.Association = lcontroller.GetAssociation(LinkArgs.OldLink, true);
                    //LinkArgs.Association = LinkArgs.OldLink.GetAssociation(true);
                    if (LinkArgs.Association != null)
                    {
                        if (VCStatusTool.IsObsoleteOrMarkedForDelete(LinkArgs.Association))
                        {
                            cancel = true;
                            LinkArgs.Cancel = true;
                        }
                        else
                        {
                            LinkArgs.Cancel = false;
                        }
                    }
                    else
                    {
                        LinkArgs.Cancel = false;
                    }
                }
            }
            if (!cancel)
                base.StartRelink(oldlink, forwards, dc);
        }

        #endregion Methods

        #region Nested Classes (1)

        public class LinkCancelEventArgs : CancelEventArgs
        {
            #region Fields (6)

            private ObjectAssociation association;
            private IMetaNode oldFromNode;
            private GoPort oldFromPort;
            private QLink oldLink;
            private IMetaNode oldToNode;
            private GoPort oldToPort;

            #endregion Fields

            #region Properties (6)

            public ObjectAssociation Association
            {
                get { return association; }
                set { association = value; }
            }

            public IMetaNode OldFromNode
            {
                get { return oldFromNode; }
                set { oldFromNode = value; }
            }

            public GoPort OldFromPort
            {
                get { return oldFromPort; }
                set { oldFromPort = value; }
            }

            public QLink OldLink
            {
                get { return oldLink; }
                set { oldLink = value; }
            }

            public IMetaNode OldToNode
            {
                get { return oldToNode; }
                set { oldToNode = value; }
            }

            public GoPort OldToPort
            {
                get { return oldToPort; }
                set { oldToPort = value; }
            }

            #endregion Properties
        }

        #endregion Nested Classes
    }

    public enum Direction
    {
        NotSpecified = -1,
        North = 0,
        NorthEast = 45,
        East = 90,
        SouthEast = 135,
        South = 180,
        SouthWest = 225,
        West = 270,
        NorthWest = 315
    }
}