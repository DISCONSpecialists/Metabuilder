using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;

using Northwoods.Go;
using Microsoft.Practices.EnterpriseLibrary.Logging;

using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Nodes;
using MetaBuilder.Graphing.Persistence;
using MetaBuilder.BusinessLogic;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Meta;
using ObjectAssociation = MetaBuilder.BusinessLogic.ObjectAssociation;
using MetaBuilder.Graphing.Shapes.Nodes.Containers;

using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.Controller.Diagram
{
    public class SaveController: System.ComponentModel.BackgroundWorker
    {
        public SaveController(System.ComponentModel.IContainer container)
            {
                container.Add(this);
            }
            private Dictionary<ClassAssociationSpec, Association> allowedAssociations;
            private Dictionary<b.GraphFileAssociationKey, GraphFileAssociation> graphFileAssocsToAdd;
            private TList<ObjectAssociation> oassocs;
            private GoView view;
            public GoView View
            {
                get { return view; }
                set { view = value; }
            }
            private TList<b.Workspace> workspaces;

            private NormalDiagram diagram;
            private string filename;

            public NormalDiagram Diagram
            {
                get { return diagram; }
                set { diagram = value; }
            }
            public string FileName
            {
                get { return filename; }
                set { filename = value; }
            }


            private void DeleteGraphFileAssociations()
            {
                AssociationHelper assHelper = new AssociationHelper();
                assHelper.DeleteGraphFileAssociationByGraphFileIDGraphFileMachine(
                    diagram.VersionManager.CurrentVersion.PKID, diagram.VersionManager.CurrentVersion.MachineName);
            }

            public bool Exists(int WorkspaceTypeId, string workspacename)
            {
                foreach (b.Workspace ws in workspaces)
                {
                    if ((ws.Name == workspacename) && (ws.WorkspaceTypeId == WorkspaceTypeId))
                        return true;

                }
                return false;
            }

            public void SaveDiagram()
            {
                #region Do Initial Work
                workspaces = d.DataRepository.WorkspaceProvider.GetAll();
                TList<GraphFileObject> fileObjects = new TList<GraphFileObject>();
                DeleteGraphFileAssociations();
                graphFileAssocsToAdd = new Dictionary<GraphFileAssociationKey, GraphFileAssociation>();
                int counter = 0;

                oassocs = new TList<ObjectAssociation>();
                #endregion
                this.ReportProgress(25, "Saving Objects");
                counter = SaveObjectsInContainer(fileObjects, counter, diagram);
                if (Directory.Exists(strings.GetPath(filename)))
                {

                    SaveMappingCells();
                    this.ReportProgress(50, "Saving Links");
                    SaveLinksInContainer(diagram);
                    #region Artifact Saving
                    int artifactcount = 0;
                    foreach (GoObject obj in diagram)
                    {
                        #region Fishlinks
                        if (obj is FishLink)
                        {
                            FishLink flink = obj as FishLink;
                            if (flink.FromPort == null || flink.ToPort == null)
                                flink.Remove();
                            else
                            {
                                IMetaNode nFrom = flink.FromNode as IMetaNode;
                                QLink lnk = ((GoPort)(flink.ToPort)).PortObject.ParentNode as QLink;
                                if (nFrom != null)
                                {
                                    if (lnk != null)
                                    {
                                        IMetaNode fromMetaNode = lnk.FromNode as IMetaNode;
                                        IMetaNode toMetaNode = lnk.ToNode as IMetaNode;
                                        // Normal nodes are already saved (afaik)
                                        if (nFrom is ArtefactNode)
                                        {
                                            ArtefactNode afnode = nFrom as ArtefactNode;

                                            if (
                                                !Exists(afnode.MetaObject.WorkspaceTypeId,
                                                        afnode.MetaObject.WorkspaceName))
                                            {
                                                if (diagram.VersionManager != null)
                                                {
                                                    DocumentVersion version = diagram.VersionManager.CurrentVersion;
                                                    afnode.MetaObject.WorkspaceName = version.WorkspaceName;
                                                    afnode.MetaObject.WorkspaceTypeId = version.WorkspaceTypeId;
                                                }
                                                else
                                                {
                                                    afnode.MetaObject.WorkspaceName =
                                                        Core.Variables.Instance.CurrentWorkspaceName;
                                                    afnode.MetaObject.WorkspaceTypeId =
                                                        Core.Variables.Instance.CurrentWorkspaceTypeId;
                                                }
                                            }
                                            afnode.MetaObject.Save(Guid.NewGuid());
                                            afnode.MetaObject.IsInDatabase = true;
                                        }


                                        Artifact art = new Artifact();
                                        art.ArtifactObjectID = nFrom.MetaObject.pkid;
                                        art.ChildObjectID = toMetaNode.MetaObject.pkid;
                                        art.ObjectID = fromMetaNode.MetaObject.pkid;
                                        art.ObjectMachine = fromMetaNode.MetaObject.machineName;
                                        art.ChildObjectMachine = toMetaNode.MetaObject.machineName;
                                        art.ArtefactMachine = nFrom.MetaObject.machineName;
                                        Association association = Singletons.GetAssociationHelper().GetAssociation(
                                            fromMetaNode.MetaObject._ClassName, toMetaNode.MetaObject._ClassName,
                                            (int)lnk.AssociationType);
                                        if (association != null)
                                        {
                                            art.CAid = association.ID;
                                            try
                                            {
                                                DataRepository.ArtifactProvider.Save(art);
                                                GraphFileObject gfo = new GraphFileObject();
                                                gfo.GraphFileID = diagram.VersionManager.CurrentVersion.PKID;
                                                gfo.MetaObjectID = art.ArtifactObjectID;
                                                gfo.MachineID = art.ArtefactMachine;
                                                gfo.GraphFileID = diagram.VersionManager.CurrentVersion.PKID;
                                                gfo.GraphFileMachine = diagram.VersionManager.CurrentVersion.MachineName;
                                                if (!fileObjects.Contains(gfo) && nFrom is ArtefactNode)
                                                {
                                                    fileObjects.Add(gfo);

                                                }
                                                artifactcount++;
                                            }
                                            catch (Exception xarts)
                                            {
                                                // Console.WriteLine(xarts.ToString());
                                            }
                                        }
                                    }

                                }
                            }
                        }

                        #endregion

                        #region Rationales
                        if (obj is Rationale)
                        {
                            // Check if it is connected to a link
                            Rationale rat = obj as Rationale;
                            if (rat.Anchor != null)
                            {
                                if (rat.Anchor.TopLevelObject is QLink)
                                {
                                    QLink slink = rat.Anchor.TopLevelObject as QLink;
                                    IMetaNode toMetaNode = slink.ToNode as IMetaNode;
                                    IMetaNode fromMetaNode = slink.FromNode as IMetaNode;
                                    Artifact art = new Artifact();

                                    art.ArtifactObjectID = rat.MetaObject.pkid;
                                    art.ChildObjectID = toMetaNode.MetaObject.pkid;
                                    art.ObjectID = fromMetaNode.MetaObject.pkid;
                                    art.ObjectMachine = fromMetaNode.MetaObject.machineName;
                                    art.ChildObjectMachine = toMetaNode.MetaObject.machineName;
                                    art.ArtefactMachine = rat.MetaObject.machineName;
                                    Association association = Singletons.GetAssociationHelper().GetAssociation(
                                        fromMetaNode.MetaObject._ClassName, toMetaNode.MetaObject._ClassName,
                                        (int)slink.AssociationType);
                                    art.CAid = association.ID;
                                    try
                                    {
                                        d.DataRepository.ArtifactProvider.Save(art);
                                    }
                                    catch
                                    {

                                    }
                                }
                                else
                                {
                                    MetaBase mbaseAnchoredTo = null;

                                    if (rat.Anchor is IMetaNode)
                                    {
                                        IMetaNode node = rat.Anchor as IMetaNode;
                                        mbaseAnchoredTo = node.MetaObject;
                                    }
                                    if (rat.Anchor is ArtefactNode)
                                    {
                                        mbaseAnchoredTo = (rat.Anchor as ArtefactNode).MetaObject;
                                    }
                                    if (mbaseAnchoredTo != null)
                                    {
                                        Association association = Singletons.GetAssociationHelper().GetAssociation(
                                            mbaseAnchoredTo._ClassName, "Rationale",
                                            4); // always a mapping
                                        b.ObjectAssociationKey oakey =
                                            new ObjectAssociationKey(association.ID, mbaseAnchoredTo.pkid,
                                                                     rat.MetaObject.pkid, mbaseAnchoredTo.MachineName,
                                                                     rat.MetaObject.machineName);
                                        b.ObjectAssociation oa = d.DataRepository.ObjectAssociationProvider.Get(oakey);
                                        if (oa == null)
                                        {
                                            oa = new ObjectAssociation();
                                            oa.CAid = oakey.CAid;
                                            oa.ObjectID = oakey.ObjectID;
                                            oa.ObjectMachine = oakey.ObjectMachine;
                                            oa.ChildObjectID = oakey.ChildObjectID;
                                            oa.ChildObjectMachine = oakey.ChildObjectMachine;
                                            d.DataRepository.ObjectAssociationProvider.Save(oa);
                                        }
                                    }
                                }

                            }
                        }
                        #endregion
                    }
                    #endregion

                    #region GraphFileX saving
                    // clear the db first
                    TList<GraphFileObject> gfos =
                        DataRepository.GraphFileObjectProvider.GetByGraphFileIDGraphFileMachine(
                            diagram.VersionManager.CurrentVersion.PKID,
                            diagram.VersionManager.CurrentVersion.MachineName);
                    DataRepository.GraphFileObjectProvider.Delete(gfos);
                    this.ReportProgress(75, "Saving Records");
                    #region GraphFileObjects
                    GraphFileManager gfm1 = new GraphFileManager();
                    gfm1.SaveToDatabase(diagram, filename, true);
                    foreach (GraphFileObject gfobject in fileObjects)
                    {

                        gfobject.GraphFileID = diagram.VersionManager.CurrentVersion.PKID;
                        gfobject.GraphFileMachine = diagram.VersionManager.CurrentVersion.MachineName;
                        b.GraphFileObjectKey gfoKey = new GraphFileObjectKey(gfobject);
                        b.GraphFileObject gfo = d.DataRepository.GraphFileObjectProvider.Get(gfoKey);
                        if (gfo == null)
                            DataRepository.GraphFileObjectProvider.Save(gfobject);
                    }
                    #endregion

                    #region Object Associations
                    //   DataRepository.ObjectAssociationProvider.Save(oassocs);
                    b.ObjectAssociation existing;
                    b.ObjectAssociationKey oaKey;
                    foreach (b.ObjectAssociation oa in oassocs)
                    {

                        oaKey = new ObjectAssociationKey(oa);
                        existing = DataRepository.ObjectAssociationProvider.Get(oaKey);
                        if (existing == null)
                            DataRepository.ObjectAssociationProvider.Save(oa);
                    }
                    #endregion

                    #region GraphFile Associations
                    /*b.GraphFileAssociationKey gfaKey;
                b.GraphFileAssociation gfa;*/
                    TList<GraphFileAssociation> gfasToAdd = new TList<GraphFileAssociation>();
                    foreach (KeyValuePair<GraphFileAssociationKey, GraphFileAssociation> kvp in graphFileAssocsToAdd)
                    {
                        bool found = false;
                        foreach (GraphFileAssociation gfaWillBeSaved in gfasToAdd)
                        {
                            if (gfaWillBeSaved.CAid ==
                                kvp.Value.CAid
                                &&
                                gfaWillBeSaved.ObjectID == kvp.Value.ObjectID
                                && gfaWillBeSaved.ChildObjectID == kvp.Value.ChildObjectID)
                            {
                                found = true;
                            }
                        }
                        if (!found)
                            gfasToAdd.Add(kvp.Value);

                    }
                    string mach = diagram.VersionManager.CurrentVersion.MachineName;
                    int pk = diagram.VersionManager.CurrentVersion.PKID;
                    foreach (GraphFileAssociation gfas in gfasToAdd)
                    {
                        GraphFileAssociationKey gfakey = new GraphFileAssociationKey(gfas);
                        gfas.GraphFileID = pk;
                        gfas.GraphFileMachine = mach;
                        if (DataRepository.GraphFileAssociationProvider.Get(gfakey) == null)
                            DataRepository.GraphFileAssociationProvider.Save(gfas);
                    }
                    //                    Console.Write("saving GFAS");

                    #endregion
                    #region GraphFile itself
                    this.ReportProgress(100, "Saving File");
                    GraphFileManager gfm = new GraphFileManager();
                    gfm.SaveToDatabase(diagram, filename, false);
                    FileUtil futil = new FileUtil();
                    futil.Save(diagram, filename);

                    #endregion
                    #endregion
                }

            }

            private TList<GraphFileAssociation> RemoveDuplicates(TList<GraphFileAssociation> assocsToTrim)
            {
                TList<GraphFileAssociation> retval = new TList<GraphFileAssociation>();
                Hashtable assocHash = new Hashtable();
                foreach (GraphFileAssociation assoc in assocsToTrim)
                {
                    GraphFileAssociationKey key = new GraphFileAssociationKey(assoc);
                    if (!assocHash.ContainsKey(key.ToString()))
                    {
                        assocHash.Add(key.ToString(), null);
                        retval.Add(assoc);
                    }
                }
                return retval;
            }

            private void SaveLinksInContainer(IGoCollection container)
            {
                if (allowedAssociations == null)
                    allowedAssociations = new Dictionary<ClassAssociationSpec, Association>();
                #region ForEach object
                foreach (GoObject gobject in container)
                {
                    if (gobject is IGoCollection)
                    {
                        SaveLinksInContainer(gobject as IGoCollection);
                    }
                    //// Console.WriteLine(gobject.ToString());

                    #region QLinks
                    if (gobject is QLink)
                    {
                        QLink lnk = gobject as QLink;
                        IMetaNode from = lnk.FromNode as IMetaNode;
                        IMetaNode to = lnk.ToNode as IMetaNode;
                        if (from != null && to != null)
                        {
                            #region Normal Links

                            if (from.MetaObject != null && to.MetaObject != null)
                            {
                                try
                                {
                                    Association assoc;
                                    ClassAssociationSpec caspec = new ClassAssociationSpec(from.MetaObject._ClassName, to.MetaObject._ClassName, (int)lnk.AssociationType);
                                    if (allowedAssociations.ContainsKey(caspec))
                                    {
                                        assoc = allowedAssociations[caspec];
                                    }
                                    else
                                    {
                                        assoc = Singletons.GetAssociationHelper().GetAssociation(from.MetaObject._ClassName,
                                                                                         to.MetaObject._ClassName,
                                                                                         (int)lnk.AssociationType);


                                        allowedAssociations.Add(caspec, null);




                                    }
                                    if (assoc != null)
                                    {

                                        ObjectAssociationKey oakey = new ObjectAssociationKey(assoc.ID,
                                            from.MetaObject.pkid, to.MetaObject.pkid, from.MetaObject.machineName, to.MetaObject.machineName);

                                        ObjectAssociation oas = d.DataRepository.ObjectAssociationProvider.Get(oakey);
                                        if (oas == null)
                                        {
                                            oas = new ObjectAssociation();
                                            oas.ObjectID = from.MetaObject.pkid;
                                            oas.ChildObjectID = to.MetaObject.pkid;
                                            oas.ObjectMachine = from.MetaObject.machineName;
                                            oas.ChildObjectMachine = to.MetaObject.machineName;
                                            oas.CAid = assoc.ID;
                                        }

                                        if (oas.VCStatusID == 0)
                                            oas.VCStatusID = (int)VCStatusList.None;
                                        bool alreadyInCollection = false;
                                        #region AddAssocs
                                        foreach (ObjectAssociation ass in oassocs)
                                        {
                                            if (ass.CAid == oas.CAid && ass.ObjectID == oas.ObjectID &&
                                                ass.ObjectMachine == oas.ObjectMachine &&
                                                ass.ChildObjectID == oas.ChildObjectID &&
                                                ass.ChildObjectMachine == oas.ChildObjectMachine)
                                            {
                                                alreadyInCollection = true;

                                            }
                                        }
                                        /*    if (
                                                DataRepository.ObjectAssociationProvider.
                                                    GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(oas.CAid,
                                                                                                                  oas.ObjectID,
                                                                                                                  oas.
                                                                                                                      ChildObjectID,
                                                                                                                  oas.
                                                                                                                      ObjectMachine,
                                                                                                                  oas.
                                                                                                                      ChildObjectMachine) ==
                                                null && (!(alreadyInCollection)))
                                                */
                                        if (!(alreadyInCollection))
                                            oassocs.Add(oas);

                                        try
                                        {
                                            if (lnk.Pen.Color == Color.Orange)
                                            {
                                                System.Drawing.Drawing2D.DashStyle dstyle = lnk.Pen.DashStyle;
                                                Pen p = new Pen(Color.Black, lnk.Pen.Width);
                                                p.DashStyle = dstyle;
                                                lnk.Pen = p;

                                            }

                                        }
                                        catch (Exception xxPen)
                                        {
                                            Console.WriteLine("Pen Error in WorkerClass:" + xxPen.ToString());
                                        }


                                        //MetaBuilder.BusinessFacade.MetaHelper.Singletons.GetObjectHelper().AddObjectAssociation(oas);
                                        /*if (DataRepository.GraphFileAssociationProvider.
                                                GetByGraphFileIDGraphFileMachineChildObjectMachineCAidObjectIDChildObjectIDObjectMachine
                                                (
                                                diagram.VersionManager.CurrentVersion.PKID,
                                                diagram.VersionManager.CurrentVersion.MachineName, oas.ChildObjectMachine,
                                                oas.ChildObjectID, oas.CAid, oas.ObjectID, oas.ObjectMachine) == null &&
                                            (!(alreadyInCollection)))
                                        {*/
                                        GraphFileAssociation gfass = new GraphFileAssociation();
                                        gfass.GraphFileID = diagram.VersionManager.CurrentVersion.PKID;
                                        gfass.GraphFileMachine = diagram.VersionManager.CurrentVersion.MachineName;
                                        gfass.CAid = oas.CAid;
                                        gfass.ObjectID = oas.ObjectID;
                                        gfass.ObjectMachine = oas.ObjectMachine;
                                        gfass.ChildObjectID = oas.ChildObjectID;
                                        gfass.ChildObjectMachine = oas.ChildObjectMachine;
                                        GraphFileAssociationKey gfakey = new GraphFileAssociationKey(gfass);
                                        if (!(graphFileAssocsToAdd.ContainsKey(gfakey)))
                                            graphFileAssocsToAdd.Add(gfakey, gfass);

                                        // }

                                        #endregion
                                    }
                                }
                                catch
                                {
                                    LogEntry entry = new LogEntry();
                                    entry.Message = "Invalid Association - not supported by the MetaModel: " +
                                                    from.MetaObject._ClassName + " to " + to.MetaObject._ClassName +
                                                    " Link: " +
                                                    lnk.AssociationType.ToString();
                                    Logger.Write(entry);
                                }
                            }

                            #endregion
                            /*
                        #region Two Way Links

                        ObjectAssociation oassocTwoWay = new ObjectAssociation();
                        if (lnk.AssociationType == LinkAssociationType.Mapping)
                        {
                            oassocTwoWay = new ObjectAssociation();
                            Association twoWayAssoc =
                                Singletons.GetAssociationHelper().GetAssociation(to.MetaObject._ClassName,
                                                                                 from.MetaObject._ClassName,
                                                                                 (int) lnk.AssociationType);
                            if (to.MetaObject.pkid > 0 && from.MetaObject.pkid > 0 && twoWayAssoc != null)
                            {
                                oassocTwoWay.ObjectID = to.MetaObject.pkid;
                                oassocTwoWay.ChildObjectID = from.MetaObject.pkid;
                                oassocTwoWay.ChildObjectMachine = from.MetaObject.machineName;
                                oassocTwoWay.ObjectMachine = to.MetaObject.machineName;
                                oassocTwoWay.CAid = twoWayAssoc.ID;
                                oassocTwoWay.VCStatusID = (int) VCStatusList.None;
                                bool alreadyAdded = false;
                                foreach (ObjectAssociation ass in oassocs)
                                {
                                    if (ass.CAid == oassocTwoWay.CAid && ass.ObjectID == oassocTwoWay.ObjectID &&
                                        ass.ObjectMachine == oassocTwoWay.ObjectMachine &&
                                        ass.ChildObjectID == oassocTwoWay.ChildObjectID &&
                                        ass.ChildObjectMachine == oassocTwoWay.ChildObjectMachine)
                                    {
                                        alreadyAdded = true;
                                       // break;
                                    }
                                }


                                    if (!(alreadyAdded))
                                    oassocs.Add(oassocTwoWay);

                              


                                if (m_sender != null) // allow unittests to run this code
                                {
                                    if (!m_sender.IsDisposed)
                                    {
                                        i++;
                                        m_sender.BeginInvoke(m_senderDelegate,
                                                             new object[]
                                                                 {m_totalMessages, i, false, "Saving Associations"});
                                    }
                                }
                            }
                            else
                            {
                                //MessageBox.Show("twoWayAssoc!");
                            }
                        }

                        #endregion*/
                        }
                    }
                    #endregion
                }
                #endregion
            }

            private void SaveMappingCells()
            {
                foreach (GoObject g in this.Diagram)
                {
                    if (g is MappingCell)
                    {
                        MappingCell mc = g as MappingCell;
                        if (mc.MetaObject != null)
                        {
                            b.TList<b.ClassAssociation> allowedAssociations =
                                d.DataRepository.ClassAssociationProvider.GetByParentClass(mc.MetaObject._ClassName);

                            List<IMetaNode> imetas = mc.GetOverlappingIMetaNodes(view);
                            foreach (IMetaNode imnode in imetas)
                            {
                                allowedAssociations.Filter = "ChildClass = '" + imnode.MetaObject._ClassName + "' and AssociationTypeID = 4";
                                if (allowedAssociations.Count > 0)
                                {
                                    b.ObjectAssociation oa = new ObjectAssociation();
                                    b.GraphFileAssociation gfassoc = new GraphFileAssociation();

                                    gfassoc.CAid = oa.CAid = allowedAssociations[0].CAid;
                                    gfassoc.ObjectID = oa.ObjectID = mc.MetaObject.pkid;
                                    gfassoc.ObjectMachine = oa.ObjectMachine = mc.MetaObject.MachineName;
                                    gfassoc.ChildObjectID = oa.ChildObjectID = imnode.MetaObject.pkid;
                                    gfassoc.ChildObjectMachine = oa.ChildObjectMachine = imnode.MetaObject.machineName;
                                    gfassoc.GraphFileMachine = this.Diagram.VersionManager.CurrentVersion.MachineName;
                                    gfassoc.GraphFileID = this.Diagram.VersionManager.CurrentVersion.PKID;
                                    b.ObjectAssociationKey oaKey = new ObjectAssociationKey(oa);
                                    ObjectAssociation oaExisting = d.DataRepository.ObjectAssociationProvider.Get(oaKey);
                                    if (oaExisting == null)
                                        d.DataRepository.ObjectAssociationProvider.Save(oa);
                                    GraphFileAssociationKey gfakey = new GraphFileAssociationKey(gfassoc);
                                    if (!graphFileAssocsToAdd.ContainsKey(gfakey))
                                    {
                                        graphFileAssocsToAdd.Add(gfakey, gfassoc);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            private int SaveNode(TList<GraphFileObject> fileObjects, int counter, GoObject gobject)
            {
                counter++;
                IMetaNode node = gobject as IMetaNode;
                if (node.BindingInfo != null && node.MetaObject != null)
                {

                    MetaBase mo = node.MetaObject;

                    mo.IsInDatabase = true;

                    SetWorkspace(mo);

                    node.SaveToDatabase(this, EventArgs.Empty);

                    GraphFileObject gfo = new GraphFileObject();
                    gfo.GraphFileID = diagram.VersionManager.CurrentVersion.PKID;
                    gfo.GraphFileMachine = diagram.VersionManager.CurrentVersion.MachineName;
                    gfo.MetaObjectID = node.MetaObject.pkid;
                    gfo.MachineID = node.MetaObject.machineName;
                    bool found = false;
                    foreach (GraphFileObject obj in fileObjects)
                    {
                        if (obj.MetaObjectID == node.MetaObject.pkid && obj.MachineID == node.MetaObject.machineName)
                        {
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        fileObjects.Add(gfo);
                    }


                }
                return counter;
            }

            private void SetWorkspace(MetaBase mo)
            {
                if (!Exists(mo.WorkspaceTypeId, mo.WorkspaceName))
                {
                    if (diagram.VersionManager != null)
                    {
                        DocumentVersion version = diagram.VersionManager.CurrentVersion;
                        mo.WorkspaceName = version.WorkspaceName;
                        mo.WorkspaceTypeId = version.WorkspaceTypeId;
                    }
                    else
                    {
                        mo.WorkspaceName = Core.Variables.Instance.CurrentWorkspaceName;
                        mo.WorkspaceTypeId = Core.Variables.Instance.CurrentWorkspaceTypeId;
                    }
                }
            }

            private int SaveObjectsInContainer(TList<GraphFileObject> fileObjects, int counter, IGoCollection collection)
            {

                foreach (GoObject gobject in collection)
                {
                    //if (gobject is IMetaNode)
                    if (gobject is IMetaNode)
                    {
                        counter = SaveNode(fileObjects, counter, gobject);
                    }
                    if (gobject is IGoCollection)
                    {
                        SaveObjectsInContainer(fileObjects, counter, gobject as IGoCollection);
                    }


                }
                if (collection is MappingCell)
                {

                    MappingCell mapCell = collection as MappingCell;

                    if (mapCell.MetaObject != null)
                    {
                        mapCell.MetaObject.Save(Guid.NewGuid());
                        if (mapCell.MetaObject.pkid > 0)
                        {
                            int pkid = mapCell.MetaObject.pkid;
                            // string WS = mapCell.MetaObject.WorkspaceName;
                            b.GraphFileObject gfo = new GraphFileObject();
                            gfo.GraphFileID = this.diagram.VersionManager.CurrentVersion.PKID;
                            gfo.GraphFileMachine = this.Diagram.VersionManager.CurrentVersion.MachineName;
                            gfo.MetaObjectID = pkid;
                            gfo.MachineID = mapCell.MetaObject.machineName;
                            fileObjects.Add(gfo);

                        }
                    }





                }
                if (collection is ILinkedContainer)
                {
                    ILinkedContainer sgnode = collection as ILinkedContainer;
                    foreach (EmbeddedRelationship er in sgnode.ObjectRelationships)
                    {

                        if (er.MyAssociation != null)
                        {
                            b.ObjectAssociation oa = new ObjectAssociation();
                            b.GraphFileAssociation gfassoc = new GraphFileAssociation();

                            if (er.MyMetaObject.pkid == 0)
                            {
                                SetWorkspace(er.MyMetaObject);
                                er.MyMetaObject.SaveToRepository(Guid.NewGuid(), "Client");
                            }
                            gfassoc.CAid = oa.CAid = er.MyAssociation.CAid;
                            gfassoc.ObjectID = oa.ObjectID = sgnode.MetaObject.pkid;
                            gfassoc.ObjectMachine = oa.ObjectMachine = sgnode.MetaObject.machineName;
                            gfassoc.ChildObjectID = oa.ChildObjectID = er.MyMetaObject.pkid;
                            gfassoc.ChildObjectMachine = oa.ChildObjectMachine = er.MyMetaObject.machineName;


                            gfassoc.GraphFileID = this.Diagram.VersionManager.CurrentVersion.PKID;
                            gfassoc.GraphFileMachine = this.Diagram.VersionManager.CurrentVersion.MachineName;

                            if ((oa.VCStatusID == 0) || (oa.VCStatusID == (int)VCStatusList.MarkedForDelete))
                                oa.VCStatusID = (int)VCStatusList.None;
                            b.ObjectAssociationKey oakey = new ObjectAssociationKey(oa);
                            ObjectAssociation oaExisting = d.DataRepository.ObjectAssociationProvider.Get(oakey);
                            if (oaExisting == null)
                                d.DataRepository.ObjectAssociationProvider.Save(oa);
                            GraphFileAssociationKey gfkey = new GraphFileAssociationKey(gfassoc);
                            if (!graphFileAssocsToAdd.ContainsKey(gfkey))
                                graphFileAssocsToAdd.Add(gfkey, gfassoc);

                        }

                    }
                }

                SaveLinksInContainer(collection);

                return counter;
            }


            #region Nested Classes (1)


            private class ClassAssociationSpec
            {

                #region Fields (3)

                private string childClass;
                private int limitToType;
                private string parentClass;

                #endregion Fields

                #region Constructors (1)

                public ClassAssociationSpec(string parentClass, string childClass, int limitToType)
                {
                    this.parentClass = parentClass;
                    this.ChildClass = childClass;
                    this.LimitToType = limitToType;
                }

                #endregion Constructors

                #region Properties (3)

                public string ChildClass
                {
                    get { return childClass; }
                    set { childClass = value; }
                }

                public int LimitToType
                {
                    get { return limitToType; }
                    set { limitToType = value; }
                }

                public string ParentClass
                {
                    get { return parentClass; }
                    set { parentClass = value; }
                }

                #endregion Properties





            }
            #endregion Nested Classes

        }
    
}
