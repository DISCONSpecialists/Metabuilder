//using MetaBuilder.UIControls.GraphingUI.CustomPrinting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Security;
using System.Windows.Forms;
using System.Xml;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.DataAccessLayer.OldCode.Diagramming;
using MetaBuilder.DataAccessLayer.OldCode.Meta;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Meta;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Northwoods.Go;
using Northwoods.Go.Layout;
using ObjectAssociation = MetaBuilder.BusinessLogic.ObjectAssociation;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.Graphing.Tools
{
    public class ContextViewer : GraphView
    {
        private List<string> myList;

        #region Fields (8)

        private readonly Hashtable addedDocs;
        private readonly Hashtable addedItems;
        private readonly Hashtable addedLinks;
        private readonly Hashtable addedShapes;

        private readonly List<string> classesOnDiagram;
        private TList<ClassAssociation> __classAssocs;
        private ContextNode _mainNode;
        private List<string> classesShown;

        private TList<ClassAssociation> classAssocs
        {
            get
            {
                if (__classAssocs == null)
                    __classAssocs = DataRepository.Connections[ProviderName].Provider.ClassAssociationProvider.GetAll();
                return __classAssocs;
            }
        }

        public ContextNode MainNode
        {
            get { return _mainNode; }
            set { _mainNode = value; }
        }

        #endregion Fields

        #region Constructors (1)

        public ContextViewer()
        {
            AllowCopy = true;
            AllowDelete = true;
            AllowEdit = true;
            AllowResize = true;
            AllowReshape = true;
            AllowMove = true;
            AllowInsert = true;
            AllowDrop = true;
            AllowLink = false;
            addedItems = new Hashtable();
            addedLinks = new Hashtable();
            addedShapes = new Hashtable();
            addedDocs = new Hashtable();
            classesShown = new List<string>();
            classesOnDiagram = new List<string>();
            __classAssocs = DataRepository.Connections[ProviderName].Provider.ClassAssociationProvider.GetAll();
            ObjectEnterLeave += ContextViewer_ObjectEnterLeave;
            this.SelectionMoved += new EventHandler(ContextViewer_SelectionMoved);

            this.OverrideDocScaleMath = true;
            DocScale = 0.9f;
            this.OverrideDocScaleMath = false;

            this.DoubleClick += new EventHandler(ContextViewer_DoubleClick);
        }

        void ContextViewer_DoubleClick(object sender, EventArgs e)
        {
#if DEBUG
            DoLayout();
#endif
        }

        #endregion Constructors

        #region Properties (2)

        public List<string> ClassesOnDiagram
        {
            get { return classesOnDiagram; }
        }

        public List<string> ClassesShown
        {
            get { return classesShown; }
            set { classesShown = value; }
        }

        #endregion Properties

        #region Methods (16)

        // Public Methods (2) 

        //public GoNode MainNode;
        public bool UseServer;

        public string ProviderName
        {
            get
            {
                if (UseServer)
                    return Core.Variables.Instance.ServerProvider;
                return Core.Variables.Instance.ClientProvider;
            }
        }

        public void ApplyFilter()
        {
            #region Toggle ContextNodes

            foreach (GoObject o in Document)
            {
                if (o is ContextNode)
                {
                    ContextNode node = o as ContextNode;
                    if (node.MetaObject != null) // prevent diagrams being filtered
                    {
                        node.Visible = classesShown.Contains(node.MetaObject._ClassName);
                    }
                }
            }

            #endregion

            #region Toggle Links

            foreach (GoObject o in Document)
            {
                if (o is QLink)
                {
                    QLink slink = o as QLink;
                    ContextNode nodeTo = slink.ToNode as ContextNode;
                    ContextNode nodeFrom = slink.FromNode as ContextNode;
                    slink.Visible = nodeTo.Visible && nodeFrom.Visible;
                    slink.RealLink.Visible = slink.Visible;
                    /*if (slink.MidLabel != null)
                    {
                        if (slink.MidLabel is GoGroup)
                        {
                            GoGroup grp = slink.MidLabel as GoGroup;
                            foreach (GoObject grpObj in grp)
                            {
                                if (grpObj is FishLinkPort)
                                {
                                    FishLinkPort prt = grpObj as FishLinkPort;
                                    GoPortLinkEnumerator fishlinkEnum = prt.Links.GetEnumerator();
                                    while (fishlinkEnum.MoveNext())
                                    {
                                        FishLink link = fishlinkEnum.Current as FishLink;
                                        link.RealLink.Visible = slink.Visible;
                                        link.Visible = slink.Visible;
                                    }
                                }
                            }
                        }
                    }*/
                }
            }

            #endregion

            #region Toggle Fishlinks

            foreach (GoObject o in Document)
            {
                if (o is FishLink)
                {
                    FishLink link = o as FishLink;
                    FishLinkPort toPort = link.ToPort as FishLinkPort;
                    GoGroup grp = toPort.Parent;
                    QLink slink = grp.Parent as QLink;
                    ContextNode nodeFrom = link.FromNode as ContextNode;
                    link.Visible = slink.Visible && nodeFrom.Visible;
                    link.RealLink.Visible = slink.Visible && nodeFrom.Visible;
                }
            }

            #endregion
        }

        ContextNode explodingNode = null;
        public void Setup(MetaBase MetaObject, bool AddNew, bool useServer)
        {
            try
            {
                UseServer = useServer;
                if (MetaObject != null)
                {
                    if (AddNew)
                    {
                        AddMainItemToDiagram(MetaObject);
                        Selection.Clear();
                        Selection.Add(MainNode);
                    }
                    else
                    {
                        classesShown = new List<string>();
                        foreach (string s in classesOnDiagram)
                        {
                            if (!ClassesShown.Contains(s))
                                classesShown.Add(s);
                        }
                        ApplyFilter();

                        //if (mainNode == null)
                        explodingNode = addedItems[MetaObject.pkid.ToString()] as ContextNode;
                        if (explodingNode == null) //it might have been opened : the key is then pkid-machine
                            explodingNode = addedItems[MetaObject.pkid.ToString() + "-" + MetaObject.MachineName.ToString()] as ContextNode;
                        if (explodingNode == null || explodingNode == MainNode)
                            return;
                        explodingNode.IsExplodedMainItem = true;
                        Selection.Clear();
                        Selection.Add(explodingNode);
                    }
                    AddRelatedObjects(MetaObject);
                    AddRelatedDiagrams(MetaObject);
                    DocScale = 1;

                    ReconnectLinks();
                    DoLayout();
                    classesShown = new List<string>();
                    foreach (string s in classesOnDiagram)
                    {
                        if (!ClassesShown.Contains(s))
                            classesShown.Add(s);
                    }

                    SheetStyle = GoViewSheetStyle.Sheet;
                    RectangleF b = ComputeDocumentBounds();
                    DocExtentCenter = new PointF(b.X + b.Width / 2, b.Y + b.Height / 2);
                    PointF c = new PointF(b.X + b.Width / 2, b.Y + b.Height / 2);
                    float s1 = 1;
                    /*if (b.Width > 0 && b.Height > 0)
      s1 = Math.Min((dispSize.Width / b.Width), (dispSize.Height / b.Height));*/
                    //if (s1 > 1) s1 = 1;
                    RescaleWithCenter(s1, c);

                    //this.SheetStyle = GoViewSheetStyle.None;
                }
                setupLegend();
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString());
            }
        }

        // Private Methods (14) 

        private void AddArtefactLink(QLink slink, ContextNode bnode)
        {
            GoGroup grp = slink.MidLabel as GoGroup;
            bnode.IsArtifact = true;
            string key = GetObjectKey((slink.FromNode as ContextNode).MetaObject) + GetObjectKey((slink.ToNode as ContextNode).MetaObject) + GetObjectKey(bnode.MetaObject);
            foreach (GoObject o in grp)
            {
                if (addedLinks.ContainsKey(key))
                    continue;
                if (o is FishLinkPort)
                {
                    FishLinkPort linkPort = o as FishLinkPort;
                    FishLink fLink = new FishLink();
                    fLink.FromPort = bnode.Port;
                    fLink.ToPort = linkPort;
                    Document.Add(fLink);
                    addedLinks.Add(key, fLink);
                }
            }
        }

        private void AddDiagram(int DiagramID, string DiagramMachine, string DiagramName, int AssociationType, ContextNode nodeToLink)
        {
            ContextNode bnode;
            if (addedItems.Contains(DiagramID.ToString() + "-" + DiagramMachine))
            {
                bnode = addedItems[DiagramID.ToString() + "-" + DiagramMachine] as ContextNode;
            }
            else
            {
                bnode = new ContextNode();
                bnode.UserObject = DiagramID.ToString() + "|" + DiagramMachine;
                bnode.Text = strings.GetFileNameOnly(DiagramName) + Environment.NewLine + "Diagram";
                bnode.Diagram = DiagramName;
                Document.Add(bnode);
                addedItems.Add(DiagramID.ToString() + "-" + DiagramMachine, bnode);
            }
            QLink slink = new QLink();
            slink.FromPort = bnode.Port;

            if (nodeToLink == null)
                if (explodingNode != null)
                    nodeToLink = explodingNode;
                else
                    nodeToLink = MainNode;

            slink.ToPort = nodeToLink.Port;
            Document.Add(slink);
            slink.AssociationType = (LinkAssociationType)AssociationType;
            // 31 OCT slink.ChangeStyle();
            slink.AvoidsNodes = true;
        }

        private QLink AddLink(int AssociationType, ContextNode nodeParent, ContextNode nodeChild)
        {
            try
            {
                string key = GetObjectKey(nodeParent.MetaObject) + GetObjectKey(nodeChild.MetaObject);
                bool ContainsLink = addedLinks.ContainsKey(key);

                if (!(ContainsLink))
                {
                    if (AssociationType == 4)
                    {
                        string tempKey = GetObjectKey(nodeChild.MetaObject) + GetObjectKey(nodeParent.MetaObject);
                        if (addedLinks.Contains(tempKey))
                            return addedLinks[tempKey] as QLink;
                        string tempKey2 = GetObjectKey(nodeParent.MetaObject) + GetObjectKey(nodeChild.MetaObject);
                        if (addedLinks.Contains(tempKey2))
                            return addedLinks[tempKey2] as QLink;
                    }
                    QLink slink = new QLink();
                    slink.AssociationType = (LinkAssociationType)AssociationType;
                    slink.FromPort = nodeParent.Port;
                    slink.ToPort = nodeChild.Port;
                    slink.Pen = new Pen(Color.Black, 1);
                    if (slink.FromNode != null && slink.ToNode != null)
                    {
                        Document.Add(slink);
                        // 31 OCT slink.ChangeStyle();
                        //slink.AvoidsNodes = true;
                        slink.StoreDeletable(GetFilteredTList(slink.AssociationType), ProviderName);

                        addedLinks.Add(key, slink);

                        //return slink;
                    }
                    //else
                    //return null;

                    #region Indicate Association Information

                    switch (nodeParent.MetaObject.Class)
                    {
                        case "DataField":
                        case "DataAttribute":
                        case "DataColumn":
                        case "Attribute":
                            //LeftIndicator.Text = "Key/Descriptive";//based on association SHOULD NEVER HAPPEN BECAUSE THEY ARE NEVER PARENTS UNLESS TRIGGERS
                            if (nodeChild.MetaObject.Class == "DataTable" || nodeChild.MetaObject.Class == "Entity" || nodeChild.MetaObject.Class == "DataEntity")
                            {
                                MetaBase pBase = nodeChild.MetaObject;
                                MetaBase cBase = nodeParent.MetaObject;
                                foreach (ObjectAssociation ass in DataRepository.Connections[ProviderName].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(pBase.pkid, pBase.MachineName))
                                {
                                    if (ass.Machine == "DB-TRIGGER" || ass.VCStatusID == 8) //dont want marked for delete items here
                                        continue;
                                    if (ass.ChildObjectID == cBase.pkid && ass.ChildObjectMachine == cBase.MachineName)
                                    {
                                        ClassAssociation clsAss = DataRepository.Connections[ProviderName].Provider.ClassAssociationProvider.GetByCAid(ass.CAid);
                                        if (clsAss.AssociationTypeID != 4)
                                            continue;
                                        string caption = clsAss.Caption;
                                        caption = caption.Replace("Primary ", "");
                                        caption = caption.Replace(" Columns", "");
                                        caption = caption.Replace(" Attributes", "");
                                        nodeParent.Text = nodeParent.MetaObject.ToString() + Environment.NewLine + caption;
                                        nodeParent.LeftIndicator.Text = nodeParent.MetaObject.Class;
                                        break;
                                    }
                                }
                            }
                            break;
                    }

                    switch (nodeChild.MetaObject.Class)
                    {
                        case "DataField":
                        case "DataAttribute":
                        case "DataColumn":
                        case "Attribute":
                            //LeftIndicator.Text = "Key/Descriptive";//based on association
                            if (nodeParent.MetaObject.Class == "DataTable" || nodeParent.MetaObject.Class == "Entity" || nodeParent.MetaObject.Class == "DataEntity")
                            {
                                MetaBase pBase = nodeParent.MetaObject;
                                MetaBase cBase = nodeChild.MetaObject;
                                foreach (ObjectAssociation ass in DataRepository.Connections[ProviderName].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(pBase.pkid, pBase.MachineName))
                                {
                                    if (ass.Machine == "DB-TRIGGER" || ass.VCStatusID == 8) //dont want marked for delete items here
                                        continue;
                                    if (ass.ChildObjectID == cBase.pkid && ass.ChildObjectMachine == cBase.MachineName)
                                    {
                                        ClassAssociation clsAss = DataRepository.Connections[ProviderName].Provider.ClassAssociationProvider.GetByCAid(ass.CAid);
                                        if (clsAss.AssociationTypeID != 4)
                                            continue;
                                        string caption = clsAss.Caption;
                                        caption = caption.Replace("Primary ", "");
                                        caption = caption.Replace(" Columns", "");
                                        caption = caption.Replace(" Attributes", "");
                                        nodeChild.Text = nodeChild.MetaObject.ToString() + Environment.NewLine + caption;
                                        nodeChild.LeftIndicator.Text = nodeChild.MetaObject.Class;
                                        break;
                                    }
                                }
                            }
                            break;
                    }

                    #endregion

                }

                return addedLinks[key] as QLink;
            }
            catch (Exception xx)
            {
                LogEntry entry = new LogEntry();
                entry.Title = "ContextViewer Exception";
                entry.Message = xx.ToString();
                entry.Message = xx.ToString();
                Logger.Write(entry);
            }
            return null;
        }

        private void AddMainItemToDiagram(MetaBase mObject)
        {
            MainNode = new ContextNode();
            //6 February 2013 - Rationales do not display text
            //if (mObject.Class == "Rationale")
            //{
            //    string t = "";
            //    if (mObject.Get("Value") != null)
            //        t = "Value:" + mObject.Get("Value").ToString();
            //    if (mObject.Get("UniqueRef") != null)
            //        t += Environment.NewLine + "UniqueRef:" + mObject.Get("UniqueRef").ToString();

            //    mainNode.Text = t + Environment.NewLine + cText;
            //}
            //else
            //MainNode.Text = mObject + Environment.NewLine + cText;

            ContextNode cnode = MainNode;
            cnode.MetaObject = mObject;
            cnode.IsMainItem = true;
            ClassesOnDiagram.Add(mObject._ClassName);
            cnode.Text = mObject + Environment.NewLine + cnode.LeftIndicator.Text;
            cnode.LeftIndicator.Text = mObject._ClassName;
            Document.Add(MainNode);
            addedItems.Add(mObject.pkid.ToString() + "-" + mObject.MachineName.ToString(), MainNode);
            //MainNode = MainNode;
        }

        private ContextNode AddObject(string ClassName, int ObjectID, string Machine)
        {
            ContextNode bnode = null;
            if (!addedItems.ContainsKey(ObjectID.ToString() + "-" + Machine))
            {
                MetaObject mo = DataRepository.Connections[ProviderName].Provider.MetaObjectProvider.GetBypkidMachine(ObjectID, Machine);
                if (mo != null)
                {
                    MetaBase mbLoaded = Loader.GetByID(mo.Class, ObjectID, Machine);
                    if (ShouldBeShown(mo.VCStatusID))
                    {
                        bnode = new ContextNode();
                        bnode.IsExplodedItem = explodingNode != null;
                        bnode.MetaObject = mbLoaded;
                        //6 February 2013 - Rationales do not display text
                        //if (ClassName == "Rationale")
                        //{
                        //    string t = "";
                        //    if (mbLoaded.Get("Value") != null)
                        //        t = "Value:" + mbLoaded.Get("Value").ToString();
                        //    if (mbLoaded.Get("UniqueRef") != null)
                        //        t += Environment.NewLine + "UniqueRef:" + mbLoaded.Get("UniqueRef").ToString();
                        //    if (mbLoaded.Get("LongDescription") != null)
                        //        t += Environment.NewLine + "UniqueRef:" + mbLoaded.Get("UniqueRef").ToString();

                        //    bnode.Text = t + Environment.NewLine + cText;
                        //}
                        //else
                        bnode.Text = mbLoaded + Environment.NewLine + bnode.LeftIndicator.Text;
                        bnode.LeftIndicator.Text = mbLoaded.Class;
                        Document.Add(bnode);
                        addedItems.Add(ObjectID.ToString() + "-" + Machine, bnode);
                    }
                }
            }
            else
            {
                bnode = addedItems[ObjectID.ToString() + "-" + Machine] as ContextNode;
            }
            if (!classesOnDiagram.Contains(ClassName))
            {
                classesOnDiagram.Add(ClassName);
            }
            return bnode;
        }

        private void AddRelatedDiagrams(MetaBase mObject)
        {
            TempFileGraphAdapter fileAdapter = new TempFileGraphAdapter();
            TList<GraphFile> files = fileAdapter.GetFilesByObjectId(mObject.pkid, mObject.MachineName, this.UseServer);
            foreach (GraphFile file in files)
            {
                if (!addedDocs.ContainsKey(file.Name))
                    addedDocs.Add(file.Name, true);
                //{
                AddDiagram(file.pkid, file.Machine, file.Name, (int)LinkAssociationType.Mapping, addedItems[mObject.pkid.ToString()] as ContextNode);

                //}
            }
        }

        private void AddRelatedObjects(MetaBase mObject)
        {
            AssociationAdapter assAdapter = new AssociationAdapter(ProviderName == Core.Variables.Instance.ServerProvider);
            DataSet dsAssociations = assAdapter.GetAssociations(mObject.pkid, mObject.MachineName);
            AssociationHelper assHelper = Singletons.GetAssociationHelper();
            int objid;
            int caid;
            string childmachname;
            ContextNode cxParentNode = addedItems[mObject.pkid.ToString() + "-" + mObject.MachineName] as ContextNode;
            foreach (DataRowView drv in dsAssociations.Tables["Parents"].DefaultView)
            {
                if (checkAssociationIsMFD(int.Parse(drv["CAID"].ToString()), int.Parse(drv["ObjectID"].ToString()), int.Parse(drv["ChildObjectID"].ToString()), drv["ObjectMachine"].ToString(), drv["ChildObjectMachine"].ToString()))
                    continue;
                if (d.DataRepository.Connections[ProviderName].Provider.ObjectAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(int.Parse(drv["CAID"].ToString()), int.Parse(drv["ObjectID"].ToString()), int.Parse(drv["ChildObjectID"].ToString()), drv["ObjectMachine"].ToString(), drv["ChildObjectMachine"].ToString()).Machine == "DB-TRIGGER")
                    continue;
                //Machine is not returned so cant check for trigger

                objid = int.Parse(drv["ObjectID"].ToString());
                childmachname = drv["ObjectMachine"].ToString();
                caid = int.Parse(drv["CAID"].ToString());
                int VCStatusid = int.Parse(drv["VCStatusID"].ToString());
                if (ShouldBeShown(VCStatusid))
                {
                    ContextNode nodeUsed = AddObject(drv["ParentClass"].ToString(), objid, drv["ObjectMachine"].ToString());
                    if (nodeUsed != null)
                    {
                        QLink slink = AddLink(int.Parse(drv["AssociationTypeID"].ToString()), nodeUsed, cxParentNode);
                        DataSet ds = assHelper.GetArtifactData(caid, objid, cxParentNode.MetaObject.pkid, cxParentNode.MetaObject.MachineName, childmachname);
                        foreach (DataRowView drvArtefact in ds.Tables[1].DefaultView)
                        {
                            ContextNode nodeUsedArt = AddObject(drvArtefact["ArtifactClass"].ToString(), int.Parse(drvArtefact["ArtifactObjectID"].ToString()), drvArtefact["ArtefactMachine"].ToString());
                            if (nodeUsedArt != null)
                                AddArtefactLink(slink, nodeUsedArt);
                        }
                    }
                }
            }
            foreach (DataRowView drv in dsAssociations.Tables["Children"].DefaultView)
            {
                if (checkAssociationIsMFD(int.Parse(drv["CAID"].ToString()), int.Parse(drv["ObjectID"].ToString()), int.Parse(drv["ChildObjectID"].ToString()), drv["ObjectMachine"].ToString(), drv["ChildObjectMachine"].ToString()))
                    continue;
                if (d.DataRepository.Connections[ProviderName].Provider.ObjectAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(int.Parse(drv["CAID"].ToString()), int.Parse(drv["ObjectID"].ToString()), int.Parse(drv["ChildObjectID"].ToString()), drv["ObjectMachine"].ToString(), drv["ChildObjectMachine"].ToString()).Machine == "DB-TRIGGER")
                    continue;

                objid = int.Parse(drv["ChildObjectID"].ToString());
                caid = int.Parse(drv["CAID"].ToString());
                childmachname = drv["ChildObjectMachine"].ToString();
                int VCStatusid2 = int.Parse(drv["VCStatusID"].ToString());
                if (ShouldBeShown(VCStatusid2))
                {
                    ContextNode nodeUsed = AddObject(drv["ChildClass"].ToString(), objid, drv["ChildObjectMachine"].ToString());
                    if (nodeUsed != null)
                    {
                        QLink slink = AddLink(int.Parse(drv["AssociationTypeID"].ToString()), cxParentNode, nodeUsed);
                        DataSet ds = assHelper.GetArtifactData(caid, cxParentNode.MetaObject.pkid, objid, childmachname, cxParentNode.MetaObject.MachineName);
                        foreach (DataRowView drvArtefact in ds.Tables[1].DefaultView)
                        {
                            ContextNode nodeUsedArt2 = AddObject(drvArtefact["ArtifactClass"].ToString(), int.Parse(drvArtefact["ArtifactObjectID"].ToString()), drvArtefact["ArtefactMachine"].ToString());
                            if (nodeUsedArt2 != null)
                                AddArtefactLink(slink, nodeUsedArt2);
                            //AddArtefactObject(slink, drvArtefact["ArtifactClass"].ToString(), int.Parse(drvArtefact["ArtifactObjectID"].ToString()),drvArtefact["ArtefactMachine"].ToString());
                        }
                    }
                }
            }
            // now, find out if this object exists as an artefact. If so, add links and hook it up visually.
            TList<Artifact> occursAsArtefactList = DataRepository.Connections[ProviderName].Provider.ArtifactProvider.GetByArtifactObjectIDArtefactMachine(cxParentNode.MetaObject.pkid, cxParentNode.MetaObject.MachineName);
            foreach (Artifact artInstance in occursAsArtefactList)
            {
                ObjectAssociation oassoc = DataRepository.Connections[ProviderName].Provider.ObjectAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(artInstance.CAid, artInstance.ObjectID, artInstance.ChildObjectID, artInstance.ObjectMachine, artInstance.ChildObjectMachine);
                ClassAssociation classAssoc = DataRepository.Connections[ProviderName].Provider.ClassAssociationProvider.GetByCAid(oassoc.CAid);
                if (oassoc.VCStatusID != (int)VCStatusList.Obsolete && oassoc.VCStatusID != (int)VCStatusList.MarkedForDelete)
                {
                    MetaObject fromObject = DataRepository.Connections[ProviderName].Provider.MetaObjectProvider.GetBypkidMachine(artInstance.ObjectID, artInstance.ObjectMachine);
                    MetaObject toObject = DataRepository.Connections[ProviderName].Provider.MetaObjectProvider.GetBypkidMachine(artInstance.ChildObjectID, artInstance.ChildObjectMachine);
                    if (ShouldBeShown(fromObject.VCStatusID) && ShouldBeShown(toObject.VCStatusID))
                    {
                        ContextNode nodeUsedFrom = AddObject(fromObject.Class, fromObject.pkid, fromObject.Machine);
                        ContextNode nodeUsedTo = AddObject(toObject.Class, toObject.pkid, toObject.Machine);
                        QLink slinkArt = AddLink(classAssoc.AssociationTypeID, nodeUsedFrom, nodeUsedTo);
                        AddArtefactLink(slinkArt, cxParentNode);
                    }
                }
            }
        }

        private bool checkAssociationIsMFD(int caid, int objectid, int childobjectid, string objectmachine, string childobjectmachine)
        {
            ObjectAssociation ass = DataRepository.Connections[ProviderName].Provider.ObjectAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(caid, objectid, childobjectid, objectmachine, childobjectmachine);
            if (ass == null)
                return false;
            else
                return ass.State == VCStatusList.MarkedForDelete ? true : false;
        }

        private void ContextViewer_ObjectEnterLeave(object sender, GoObjectEnterLeaveEventArgs e)
        {
            if (e.To is QRealLink)
            {
                QRealLink slink = e.To as QRealLink;

                QLink grp = slink.Parent as QLink;
                GoLayer l = grp.Layer;
                l.MoveAfter(null, grp);
                //slink.
            }
        }

        private Dictionary<Guid, PointF> objectsLayedOut = new Dictionary<Guid, PointF>();

        private void DoLayout()
        {
            GoLayoutLayeredDigraph glfd = new GoLayoutLayeredDigraph(); //.GoLayoutTree();
            glfd.DirectionOption = GoLayoutDirection.Right;
            glfd.LayeringOption = GoLayoutLayeredDigraphLayering.OptimalLinkLength;
            glfd.AggressiveOption = GoLayoutLayeredDigraphAggressive.More;
            glfd.ColumnSpacing = 0;
            glfd.LayerSpacing = 0;
            glfd.CycleRemoveOption = GoLayoutLayeredDigraphCycleRemove.Greedy;
            glfd.SetsPortSpots = true;
            glfd.Document = Document;
            glfd.PerformLayout();

            List<ContextNode> artifactsCalculated = new List<ContextNode>();

            foreach (GoObject o in this.Document)
            {
                if (o is ContextNode)
                {
                    ContextNode node = o as ContextNode;

                    if (node.IsArtifact)
                    {
                        if (artifactsCalculated.Contains(node))
                            continue; //already calculated don't iterate or we do extra work for no reason

                        int count = 0;
                        //node.Location = (node.Links.Current.ToPort as GoPort).Location;
                        foreach (IGoLink nodelink in (node.DestinationLinks))
                        {
                            if (!(nodelink is FishLink))
                                continue;
                            foreach (IGoLink link in nodelink.ToPort.SourceLinks)
                            {
                                if (!(link is FishLink))
                                    continue;
                                ContextNode artifactNode = link.FromNode as ContextNode;
                                if (artifactNode.IsArtifact)
                                {
                                    artifactNode.Location = new PointF((link.ToPort as GoPort).Location.X + 20, (link.ToPort as GoPort).Location.Y - 20 - ((artifactNode.Height * count) + 10));
                                    artifactsCalculated.Add(artifactNode);
                                    count += 1;
                                }
                            }
                        }

                        //do not position this to the user positioned location
                        continue;
                    }

                    if (objectsLayedOut.ContainsKey(node.ID))
                    {
                        o.Location = objectsLayedOut[node.ID];
                    }
                    else
                    {
                        //objectsLayedOut.Add((o as ContextNode).ID, (o as ContextNode).Location);
                    }
                }
            }
            artifactsCalculated.Clear();
            artifactsCalculated = null;
        }

        private void ContextViewer_SelectionMoved(object sender, EventArgs e)
        {
            foreach (GoObject o in Selection)
            {
                if (o is ContextNode)
                {
                    if (objectsLayedOut.ContainsKey((o as ContextNode).ID))
                    {
                        objectsLayedOut[(o as ContextNode).ID] = o.Location;
                    }
                    else
                    {
                        objectsLayedOut.Add((o as ContextNode).ID, (o as ContextNode).Location);
                    }
                }
            }
        }

        private TList<ClassAssociation> GetFilteredTList(LinkAssociationType linkType)
        {
            classAssocs.Filter = "AssociationTypeID = " + ((int)linkType).ToString();
            return classAssocs;
        }

        private string GetObjectKey(MetaBase mbase)
        {
            return mbase.pkid.ToString() + mbase.MachineName;
        }

        private void ReconnectLinks()
        {
            /*
                        AutoRelinkTool autorelinktool = new AutoRelinkTool();
                        GoCollection collection = new GoCollection();
                        collection.AddRange(Document);
                        autorelinktool.RelinkCollection(collection);*/
        }

        private bool ShouldBeShown(int ItemVCStatusID)
        {
            return (ItemVCStatusID != (int)VCStatusList.Obsolete || ItemVCStatusID != (int)VCStatusList.MarkedForDelete || ItemVCStatusID != (int)VCStatusList.Locked);
        }

        #endregion Methods

        #region Save Load

        public string SaveThisView()
        {
            myList = new List<string>();
            String xmlLine;

            xmlLine = "<ContextDiagram Name = \"" + Name + "\" Server = \"" + UseServer.ToString() + "\">";
            myList.Add(xmlLine);

            //Save All nodes
            foreach (GoObject obj in Document)
            {
                if (obj is ContextNode)
                {
                    ContextNode x = obj as ContextNode;
                    if (x.MetaObject == null) //this will be true for all context objects which we dont have in our database
                    {
                        if (x.UserObject == null)
                            x.UserObject = "Missing";
                        //bnode.UserObject = DiagramID.ToString() + "|" + DiagramMachine;
                        xmlLine = "<ContextNode UserObject = \"" + x.UserObject.ToString() + "\" PKID = \"" + x.Location.X.ToString() + "\" Machine = \"" + x.Location.Y.ToString() + "\" Name = \"" + x.Text + "\" Class = \"Diagram\" IsMainItem = \"0\" PositionX = \"" + x.Location.X.ToString().Replace(",", ".") + "\" PositionY = \"" + x.Location.Y.ToString().Replace(",", ".") + "\"></ContextNode>";
                        myList.Add(xmlLine);
                    }
                    else
                    {
                        //if (x.Visible)
                        {
                            if (x.IsMainItem)
                            {
                                xmlLine = "<ContextNode PKID = \"" + x.MetaObject.pkid + "\" Machine = \"" + x.MetaObject.MachineName + "\" Name = \"" + SecurityElement.Escape(x.Text) + "\" Class = \"" + x.MetaObject._ClassName + "\" IsMainItem = \"1\" PositionX = \"" + x.Location.X.ToString().Replace(",", ".") + "\" PositionY = \"" + x.Location.Y.ToString().Replace(",", ".") + "\"></ContextNode>";
                                myList.Add(xmlLine);
                            }
                            else
                            {
                                xmlLine = "<ContextNode PKID = \"" + x.MetaObject.pkid + "\" Machine = \"" + x.MetaObject.MachineName + "\" Name = \"" + SecurityElement.Escape(x.Text) + "\" Class = \"" + x.MetaObject._ClassName + "\" IsMainItem = \"0\" PositionX = \"" + x.Location.X.ToString().Replace(",", ".") + "\" PositionY = \"" + x.Location.Y.ToString().Replace(",", ".") + "\"></ContextNode>";
                                myList.Add(xmlLine);
                            }
                        }
                    }
                }
            }
            string FromNodeLink;
            String ToNodeLink;
            //Save links
            foreach (GoObject obj in Document)
            {
                //if (obj.Visible)
                {
                    if (obj is QLink)
                    {
                        #region QLinks

                        QLink Mobj = obj as QLink;
                        ContextNode x = Mobj.FromNode as ContextNode;
                        ContextNode y = Mobj.ToNode as ContextNode;

                        if (x.MetaObject != null)
                        {
                            FromNodeLink = "FromNode = \"" + x.MetaObject.pkid + "\" FromMachine = \"" + x.MetaObject.MachineName + "\"";
                        }
                        else //DiagramNodesUseLocationToSaveLoadLink
                        {
                            FromNodeLink = "FromNode = \"" + x.Location.X.ToString() + "\" FromMachine = \"" + x.Location.Y.ToString() + "\"";
                        }

                        if (y.MetaObject != null)
                        {
                            ToNodeLink = "ToNode = \"" + y.MetaObject.pkid + "\" ToMachine = \"" + y.MetaObject.MachineName + "\"";
                        }
                        else //DiagramNodesUseLocationToSaveLoadLink
                        {
                            ToNodeLink = "ToNode = \"" + x.Location.X.ToString() + "\" ToMachine = \"" + x.Location.Y.ToString() + "\"";
                        }

                        PointF[] points = ((obj as GoLabeledLink).RealLink as GoLink).CopyPointsArray();

                        xmlLine = "<QLink " + FromNodeLink + " " + ToNodeLink + " AssociationType = \"" + Mobj.AssociationType + "\" Points = \"" + setPointArray(points) + "\"></QLink>";
                        myList.Add(xmlLine);

                        #endregion
                    }
                    else if (obj is FishLink)
                    {
                        #region Artifacts

                        FishLink Mobj = obj as FishLink;
                        if (Mobj.FromNode != null)
                        {
                            ContextNode a = Mobj.FromNode as ContextNode;

                            QLink sLink = Mobj.ToQLink;
                            ContextNode x = sLink.FromNode as ContextNode;
                            ContextNode y = sLink.ToNode as ContextNode;

                            if (a != null && a.MetaObject != null)
                            {
                                xmlLine = "<FishLink>";
                                myList.Add(xmlLine);
                                xmlLine = "<Artifact ArtifactID = \"" + a.MetaObject.pkid + "\" ArtifactMachine = \"" +
                                          a.MetaObject.MachineName + "\" Class = \"" +
                                          a.MetaObject._ClassName + "\" Name = \"" + a.Text +
                                          "\" ObjectID = \"" + x.MetaObject.pkid + "\" ObjectMachine = \"" +
                                          x.MetaObject.MachineName + "\" ChildObjectID = \"" + y.MetaObject.pkid +
                                          "\" ChildObjectMachine = \"" + y.MetaObject.MachineName +
                                          "\" PositionX = \"" + a.Location.X.ToString() + "\" PositionY = \"" + a.Location.Y.ToString() + "\"></Artifact>";
                                myList.Add(xmlLine);
                                xmlLine = "</FishLink>";
                                myList.Add(xmlLine);
                            }
                        }

                        #endregion
                    }
                }
            }

            xmlLine = "</ContextDiagram>";
            myList.Add(xmlLine);

            return saveToTextFile(myList);
        }

        private string saveToTextFile(List<string> allLines)
        {
            SaveFileDialog mySaver = new SaveFileDialog();
            mySaver.Filter = "View In Context Files | *.vicxml";
            mySaver.Title = "Please enter a name for this diagram to be saved as";
            mySaver.ShowDialog(this);

            if (mySaver.FileName == string.Empty)
            {
                //MessageBox.Show(this,"The Save operation was cancelled", "Cancelled");
                return "";
            }

            // create a writer and open the file
            //if (File.Exists("c:\\" + Name.ToString() + " Context View Test.xml"))
            //    File.Delete("c:\\" + Name.ToString() + " Context View Test.xml");

            //TextWriter tw = new StreamWriter("c:\\" + Name.ToString() + " Context View Test");

            TextWriter tw = new StreamWriter(mySaver.FileName);

            foreach (string line in allLines)
            {
                //Remove invalid xml characters
                line.Replace("<", ""); //&lt?
                line.Replace(">", ""); //&gt?
                // write a line of text to the file
                tw.WriteLine(line);
            }
            // close the stream
            tw.Close();
            // message the user
            //MessageBox.Show(this,"The Save operation has completed" + Environment.NewLine + "File can be found at : " + mySaver.FileName, "Saved");
            return mySaver.FileName;
        }

        public void LoadNewView(string FileName)
        {
            try
            {
                Document.Clear();
                addedItems.Clear();
                addedLinks.Clear();
                addedShapes.Clear();
                addedDocs.Clear();

                XmlDocument doc = new XmlDocument();
                doc.Load(FileName);

                MetaBase BaseItem;
                ContextNode childNode;
                ContextNode diagramNode;

                UseServer = doc.DocumentElement.Attributes[Core.Variables.Instance.ServerProvider] != null ? bool.Parse(doc.DocumentElement.Attributes[Core.Variables.Instance.ServerProvider].Value.ToString()) : false;

                XmlNodeList contextNodes = doc.GetElementsByTagName("ContextNode");
                XmlNodeList QLink = doc.GetElementsByTagName("QLink");
                XmlNodeList fishLink = doc.GetElementsByTagName("Artifact");

                classesShown = new List<string>();
                foreach (XmlNode n in contextNodes)
                {
                    #region Nodes

                    if (int.Parse(n.Attributes["IsMainItem"].Value) == 1)
                    {
                        BaseItem = Loader.GetByID(n.Attributes["Class"].Value, int.Parse(n.Attributes["PKID"].Value), n.Attributes["Machine"].Value);
                        this.Tag = BaseItem;

                        if (BaseItem != null)
                        {
                            if (!classesOnDiagram.Contains(BaseItem.Class))
                                classesOnDiagram.Add(BaseItem.Class);
                            if (!classesShown.Contains(BaseItem.Class))
                                classesShown.Add(BaseItem.Class);
                        }

                        MainNode = new ContextNode();
                        ContextNode cnode = MainNode;
                        cnode.MetaObject = BaseItem;
                        cnode.IsMainItem = true;
                        if (!addedItems.Contains(n.Attributes["PKID"].Value + "-" + n.Attributes["Machine"].Value))
                            addedItems.Add(n.Attributes["PKID"].Value + "-" + n.Attributes["Machine"].Value, MainNode);
                        Document.Add(MainNode);
                        MainNode = cnode;

                        if (cnode.MetaObject == null)
                            MainNode.Text = n.Attributes["Name"].Value;
                        else
                        {
                            //MainNode.LeftIndicator.Text = MainNode.MetaObject.Class;

                            MainNode.Text = BaseItem + Environment.NewLine + cnode.LeftIndicator.Text;
                            MainNode.LeftIndicator.Text = BaseItem._ClassName;
                        }

                        if (n.Attributes["PositionX"] != null)
                            MainNode.Location = new PointF(float.Parse(n.Attributes["PositionX"].Value.ToString().Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture), float.Parse(n.Attributes["PositionY"].Value.ToString().Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        //pkid = 0 for legacy (must be a string because we save float coords
                        if (n.Attributes["Class"].Value.ToString().ToLower() == "diagram" || n.Attributes["PKID"].Value.ToString() == "0")
                        {
                            diagramNode = new ContextNode();
                            if (n.Attributes["UserObject"] != null)
                                diagramNode.UserObject = n.Attributes["UserObject"].Value.ToString();
                            diagramNode.Text = n.Attributes["Name"].Value;
                            if (n.Attributes["PositionX"] != null)
                                diagramNode.Location = new PointF(float.Parse(n.Attributes["PositionX"].Value.ToString().Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture), float.Parse(n.Attributes["PositionY"].Value.ToString().Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture));
                            Document.Add(diagramNode);
                            if (!addedItems.Contains(n.Attributes["PKID"].Value + "-" + n.Attributes["Machine"].Value))
                                addedItems.Add(n.Attributes["PKID"].Value + "-" + n.Attributes["Machine"].Value, diagramNode);
                        }
                        else
                        {
                            BaseItem = Loader.GetByID(n.Attributes["Class"].Value, int.Parse(n.Attributes["PKID"].Value), n.Attributes["Machine"].Value);

                            if (BaseItem != null)
                            {
                                if (!classesOnDiagram.Contains(BaseItem.Class))
                                    classesOnDiagram.Add(BaseItem.Class);
                                if (!classesShown.Contains(BaseItem.Class))
                                    classesShown.Add(BaseItem.Class);
                            }

                            childNode = new ContextNode();
                            childNode.MetaObject = BaseItem;

                            if (childNode.MetaObject == null)
                                childNode.Text = n.Attributes["Name"].Value;
                            else
                            {
                                //childNode.LeftIndicator.Text = childNode.MetaObject.Class;

                                childNode.Text = BaseItem + Environment.NewLine + childNode.LeftIndicator.Text;
                                childNode.LeftIndicator.Text = BaseItem._ClassName;
                            }

                            if (n.Attributes["PositionX"] != null)
                                childNode.Location = new PointF(float.Parse(n.Attributes["PositionX"].Value.ToString().Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture), float.Parse(n.Attributes["PositionY"].Value.ToString().Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture));

                            Document.Add(childNode);
                            if (!addedItems.Contains(n.Attributes["PKID"].Value + "-" + n.Attributes["Machine"].Value))
                                addedItems.Add(n.Attributes["PKID"].Value + "-" + n.Attributes["Machine"].Value, childNode);
                        }
                    }

                    #endregion
                }

                foreach (XmlNode n in QLink)
                {
                    #region Object Links

                    string fromNodePKID = n.Attributes["FromNode"].Value;
                    string toNodePKID = n.Attributes["ToNode"].Value;
                    string fromMachine = n.Attributes["FromMachine"].Value;
                    string toMachine = n.Attributes["ToMachine"].Value;
                    PointF[] pts = null;
                    if (n.Attributes["Points"] != null)
                        pts = getPointArray(n.Attributes["Points"].Value);
                    string key = fromNodePKID.ToString() + fromMachine + toNodePKID.ToString() + toMachine;

                    ContextNode FromNode;
                    ContextNode ToNode;

                    //Find fromnode
                    FromNode = addedItems[fromNodePKID + "-" + fromMachine] as ContextNode;
                    //Find tonode
                    ToNode = addedItems[toNodePKID + "-" + toMachine] as ContextNode;
                    if (FromNode != null && ToNode != null)
                        if (!addedLinks.ContainsKey(key))
                        {
                            if (FromNode.MetaObject != null && ToNode.MetaObject != null)
                                AddLink(DataRepository.Connections[ProviderName].Provider.AssociationTypeProvider.GetByName(n.Attributes["AssociationType"].Value).pkid, FromNode, ToNode);
                            else
                            {
                                QLink slink = new QLink();
                                slink.AssociationType = (LinkAssociationType)DataRepository.Connections[ProviderName].Provider.AssociationTypeProvider.GetByName(n.Attributes["AssociationType"].Value).pkid;
                                slink.FromPort = FromNode.Port;
                                slink.ToPort = ToNode.Port;
                                if (pts != null)
                                    (slink as GoLabeledLink).RealLink.SetPoints(pts);

                                Document.Add(slink);
                                // 31 OCT slink.ChangeStyle();
                                //slink.AvoidsNodes = true;
                                //slink.StoreDeletable(GetFilteredTList(slink.AssociationType));
                                if (!addedLinks.Contains(key))
                                    addedLinks.Add(key, slink);
                            }
                        }

                    #endregion
                }

                foreach (XmlNode n in fishLink)
                {
                    #region Artifact Links

                    //DATA
                    //<Artifact ArtifactID = \"170966\" ArtifactMachine = \"FOURIE\" Class = \"SelectorAttribute\" Name = \"Study Unit Job Category
                    //SelectorAttribute\" ObjectID = \"185239\" ObjectMachine = \"PATRICE\" ChildObjectID = \"185217\" ChildObjectMachine = \"PATRICE\"></Artifact>

                    childNode = addedItems[n.Attributes["ArtifactID"].Value + "-" + n.Attributes["ArtifactMachine"].Value] as ContextNode;
                    if (childNode != null)
                    {
                        QLink myLink = addedLinks[n.Attributes["ObjectID"].Value + n.Attributes["ObjectMachine"].Value + n.Attributes["ChildObjectID"].Value + n.Attributes["ChildObjectMachine"].Value] as QLink;
                        //Link
                        if (myLink != null)
                            AddArtefactLink(myLink, childNode);
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "An error has occurred", ex.ToString());
                Log.WriteLog("ContextViewer::LoadNewView::" + ex.ToString());
            }

            #region ContextViewerPostConfiguration Setup

            setupLegend();

            SheetStyle = GoViewSheetStyle.Sheet;
            //RectangleF b = ComputeDocumentBounds();
            //DocExtentCenter = new PointF(b.X + b.Width / 2, b.Y + b.Height / 2);
            //PointF c = new PointF(b.X + b.Width / 2, b.Y + b.Height / 2);
            //float s1 = 1;
            //RescaleWithCenter(s1, c);

            DocScale = 1f;
            //BackgroundHasSheet = false;
            if (MainNode != null)
                ScrollRectangleToVisible(MainNode.Bounds);
            ReconnectLinks();
            //DoLayout();

            #endregion
        }

        private PointF[] getPointArray(string points)
        {
            PointF[] pointsArray = new PointF[points.Split('|').Length - 1];
            int pointArrayIndex = 0;
            foreach (string pointStr in points.Split('|'))
            {
                PointF point = new PointF(0, 0);
                foreach (string coORD in pointStr.Split(':'))
                {
                    //skip strings which have no :
                    if (coORD.Length == 0)
                        continue;

                    if (point.X == 0)
                        point.X = float.Parse(coORD.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
                    else
                        point.Y = float.Parse(coORD.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
                }
                if (point.X != 0 && point.Y != 0)
                    pointsArray[pointArrayIndex] = point;
                pointArrayIndex += 1;
            }
            return pointsArray;
        }
        private string setPointArray(PointF[] points)
        {
            string strPoints = "";
            foreach (PointF p in points)
            {
                strPoints += "" + p.X.ToString().Replace(",", ".") + ":" + p.Y.ToString().Replace(",", ".") + "|";
            }
            return strPoints;
        }
        #endregion

        protected override void OnObjectSelectionDropReject(GoObjectEventArgs evt)
        {
            if (!(Selection.First is ContextNode))
            {
                evt.InputState = GoInputState.Cancel;
                return;
            }
            base.OnObjectSelectionDropReject(evt);
        }

        private Hashtable workspacesOnDiagram;
        public GoListGroup legend;
        private void setupLegend()
        {
            this.Document.Remove(legend);
            legend = null;

            workspacesOnDiagram = new Hashtable();
            workspaceColorsAdded = new List<Color>();

            legend = new GoListGroup();
            legend.Orientation = Orientation.Horizontal;
            legend.Selectable = true;
            legend.Deletable = false;
            legend.Editable = false;
            legend.Movable = true;
            legend.Printable = true;
            legend.Location = Document.TopLeft;
            legend.BorderPen = new Pen(new SolidBrush(Color.Black));

            //find workspaces on diagram
            Color color = getColor();
            foreach (DictionaryEntry entry in addedItems)
            {
                //visible again
                setNode((entry.Value as ContextNode), true);

                if ((entry.Value as ContextNode).MetaObject != null)
                {
                    if ((entry.Value as ContextNode).MetaObject.WorkspaceName != null)
                    {
                        string ws = (entry.Value as ContextNode).MetaObject.WorkspaceName;
                        if (!workspacesOnDiagram.Contains(ws))
                        {
                            color = getColor();
                            workspacesOnDiagram.Add(ws, color);
                        }
                        else
                            color = (Color)workspacesOnDiagram[(entry.Value as ContextNode).MetaObject.WorkspaceName];

                        (entry.Value as ContextNode).ToolTipText = (entry.Value as ContextNode).MetaObject.WorkspaceName;

                        //add square depiction of workspace base on color
                        (entry.Value as ContextNode).MySmallRectangle.Brush = new SolidBrush(color);
                    }
                }
                else
                {
                    if ((entry.Value as ContextNode).UserObject == null)
                        continue;

                    string k = (entry.Value as ContextNode).UserObject.ToString();
                    if (!k.Contains("|")) continue;
                    string pkid = null;
                    string machine = null;

                    //Use the userobject to get a value for the diagram and workspace of it
                    foreach (string s in k.Split('|'))
                    {
                        if (pkid == null)
                        {
                            pkid = s;
                            continue;
                        }
                        machine = s;
                    }
                    MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter adapter = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();
                    GraphFile file = adapter.GetQuickFileDetails(int.Parse(pkid), machine, (ProviderName == Core.Variables.Instance.ServerProvider)); //d.DataRepository.Connections[ProviderName].Provider.GraphFileProvider.GetBypkidMachine(int.Parse(pkid), machine);
                    if (file != null)
                    {
                        if (!workspacesOnDiagram.Contains(file.WorkspaceName))
                        {
                            color = getColor();
                            workspacesOnDiagram.Add(file.WorkspaceName, color);
                        }
                        else
                            color = (Color)workspacesOnDiagram[file.WorkspaceName];

                        (entry.Value as ContextNode).ToolTipText = file.WorkspaceName;
                        (entry.Value as ContextNode).MySmallRectangle.Brush = new SolidBrush(color);
                    }
                }
            }

            foreach (DictionaryEntry ws in workspacesOnDiagram)
            {
                legend.Add(createLegendItem(ws.Key as string, (Color)ws.Value));
            }

            //legend.Visible = workspacesOnDiagram.Count > 1;

            this.Document.Add(legend);
        }

        private void setNode(ContextNode node, bool visible)
        {
            node.Visible = visible;
            foreach (GoObject link in node.DestinationLinks)
                link.Visible = visible;
            foreach (GoObject link in node.Links)
                link.Visible = visible;
        }

        List<Color> workspaceColorsAdded;
        Random randomGen = new Random();

        KnownColor[] names;

        private Color getColor()
        {
            try
            {
                if (names == null)
                    names = (KnownColor[])Enum.GetValues(typeof(KnownColor));

                KnownColor randomColorName = names[randomGen.Next(names.Length)];
                Color randomColor = Color.FromKnownColor(randomColorName);
                if (!workspaceColorsAdded.Contains(randomColor))
                    workspaceColorsAdded.Add(randomColor);
                else
                    randomColor = getColor();

                return randomColor;
            }
            catch
            {
                return Color.LightBlue;
            }
            return Color.LightBlue;
        }

        private GoObject createLegendItem(string text, Color color)
        {
            GoListGroup legItem = new GoListGroup();
            legItem.DragsNode = true;
            legItem.Orientation = Orientation.Horizontal;
            legItem.Spacing = 5;

            GoRectangle rect = new GoRectangle();
            rect.Width = 20;
            rect.Height = 20;
            rect.Brush = new SolidBrush(color);
            rect.DragsNode = true;
            rect.Selectable = false;
            rect.Shadowed = false;
            legItem.Add(rect);

            GoText t = new GoText();
            t.Text = UseServer ? text + " (Server)" : text;
            t.DragsNode = true;
            t.Selectable = false;
            t.Shadowed = false;

            legItem.Add(t);
            return legItem;
        }

    }
}