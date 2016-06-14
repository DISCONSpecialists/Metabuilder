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
namespace MetaBuilder.UIControls.GraphingUI
{
    public class DiagramSaveEventArgs : EventArgs
    {
        public DiagramSaveEventArgs(VCStatusList state)
        {
            currentState = state;
        }

        private VCStatusList currentState;
        public VCStatusList CurrentState { get { return currentState; } }
    }

    public class DiagramSaver : System.ComponentModel.BackgroundWorker
    {
        public DiagramSaver(System.ComponentModel.IContainer container)
        {
            container.Add(this);
        }

        private Dictionary<ClassAssociationSpec, Association> allowedAssociations;
        private Dictionary<b.GraphFileAssociationKey, GraphFileAssociation> graphFileAssocsToAdd;
        private TList<ObjectAssociation> oassocs;
        private Dictionary<ObjectAssociation, Tuple> associationValues;
        private GoView view;
        public GoView View
        {
            get { return view; }
            set { view = value; }
        }
        //private TList<b.Workspace> workspaces;

        private NormalDiagram diagram;
        private string filename;

        public NormalDiagram Diagram
        {
            get { return diagram; }
            set { diagram = value; }
        }
        public string FileName
        {
            get
            {
                if (filename.ToLower().EndsWith(".mdgm"))
                    return filename;
                else
                {
                    Log.WriteLog(filename + " - Suffixed with extension");
                    filename = filename + ".mdgm";
                    return filename;
                }
            }
            set { filename = value; }
        }

        private void DeleteGraphFileAssociations()
        {
            AssociationHelper assHelper = new AssociationHelper();
            assHelper.DeleteGraphFileAssociationByGraphFileIDGraphFileMachine(Diagram.VersionManager.CurrentVersion.PKID, Diagram.VersionManager.CurrentVersion.MachineName);
        }

        public bool Exists(int WorkspaceTypeId, string workspacename)
        {
            foreach (DictionaryEntry ws in Core.Variables.Instance.WorkspaceHashtable)
            {
                if (ws.Key.ToString() == workspacename + "#" + WorkspaceTypeId)
                    //if ((ws.Name == workspacename) && (ws.WorkspaceTypeId == WorkspaceTypeId))
                    return true;
            }
            return false;
        }

        private List<IMetaNode> getNodesInContainer(GoGroup group)
        {
            List<IMetaNode> nodes = new List<IMetaNode>();
            foreach (GoObject o in group)
            {
                if (o is IMetaNode)
                {
                    nodes.Add(o as IMetaNode);
                }
                //this is no supposed to be an else
                if (o is GoGroup)
                {
                    nodes.AddRange(getNodesInContainer(o as GoGroup));
                }
            }
            return nodes;
        }
        private GraphFileObject BuildFileObject(IMetaNode node)
        {
            GraphFileObject gfo = new GraphFileObject();
            gfo.GraphFileID = Diagram.VersionManager.CurrentVersion.PKID;
            gfo.GraphFileMachine = Diagram.VersionManager.CurrentVersion.MachineName;
            gfo.MetaObjectID = node.MetaObject.pkid;
            gfo.MachineID = node.MetaObject.MachineName;
            return gfo;
        }

        private void BuildFileAssociation(QLink link)
        {
            GraphFileAssociation gfass = new GraphFileAssociation();
            gfass.GraphFileID = Diagram.VersionManager.CurrentVersion.PKID;
            gfass.GraphFileMachine = Diagram.VersionManager.CurrentVersion.MachineName;
            gfass.CAid = link.DBAssociation.CAid;
            gfass.ObjectID = link.DBAssociation.ObjectID;
            gfass.ObjectMachine = link.DBAssociation.ObjectMachine;
            gfass.ChildObjectID = link.DBAssociation.ChildObjectID;
            gfass.ChildObjectMachine = link.DBAssociation.ChildObjectMachine;
            GraphFileAssociationKey gfakey = new GraphFileAssociationKey(gfass);
            if (!(graphFileAssocsToAdd.ContainsKey(gfakey)))
            {
                graphFileAssocsToAdd.Add(gfakey, gfass);
                if (DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileAssociationProvider.Get(gfakey) == null)
                    DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileAssociationProvider.Save(gfass);
            }
        }

        private static bool IsFileReadOnly(string file)
        {
            // Create a new FileInfo object.
            FileInfo fInfo = new FileInfo(file);
            if (fInfo == null)
                return false; //file is not readonly because file does no exist
            try
            {
                if (fInfo.IsReadOnly)
                {
                    Log.WriteLog("Trying to make " + file + " not read only");
                    fInfo.IsReadOnly = false;
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("Failed to make not read only " + Environment.NewLine + ex.ToString());
            }

            // Return the IsReadOnly property value.
            return fInfo.IsReadOnly;

        }

        public void SaveDiagram()
        {
            ReportProgress(1, "Initialized");

            if (File.Exists(FileName) && IsFileReadOnly(FileName))
            {
                ReportProgress(90, "Read Only File");
                Log.WriteLog("The file(" + FileName + ") cannot be saved because it is readonly");
                return;
            }
            if (!Directory.Exists(strings.GetPath(FileName)))
            {
                Core.Log.WriteLog(strings.GetPath(FileName) + " cannot be found. Moving to " + Core.Variables.Instance.DiagramPath);
                FileName = Core.Variables.Instance.DiagramPath + strings.GetFileNameOnly(FileName);
                if (!Directory.Exists(strings.GetPath(FileName)))
                {
                    ReportProgress(100, "Invalid Directory");
                    Core.Log.WriteLog(FileName + " - Directory does not exist. Saving (Viewer) aborted");
                    return;
                }
            }

            if (Core.Variables.Instance.IsViewer)
            {
                ReportProgress(50, "Saving File");
                FileUtil fileUtilViewer = new FileUtil();
                fileUtilViewer.Save(Diagram, FileName);
                return;
            }

            #region Do Initial Work

            //workspaces = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.WorkspaceProvider.GetAll();
            TList<GraphFileObject> fileObjects = new TList<GraphFileObject>();
            DeleteGraphFileAssociations();
            graphFileAssocsToAdd = new Dictionary<GraphFileAssociationKey, GraphFileAssociation>();
            int counter = 0;
            oassocs = new TList<ObjectAssociation>();
            associationValues = new Dictionary<ObjectAssociation, Tuple>();
            #endregion

            //if (!Directory.Exists(strings.GetPath(FileName)))
            //{
            //    ReportProgress(80, "Invalid Directory");
            //    Core.Log.WriteLog(FileName + " - Directory does not exist. Saving aborted");
            //    return;
            //}

            if (Core.Variables.Instance.SaveOnCreate)
            {
                ReportProgress(10, "Clearing Object Records");
                // clear the db first
                TList<GraphFileObject> gfosSOC = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileObjectProvider.GetByGraphFileIDGraphFileMachine(Diagram.VersionManager.CurrentVersion.PKID, Diagram.VersionManager.CurrentVersion.MachineName);
                DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileObjectProvider.Delete(gfosSOC);
                ReportProgress(20, "Saving Logical File");

                GraphFileManager graphFileManagerSOC = new GraphFileManager();
                graphFileManagerSOC.SaveToDatabase(Diagram, FileName, true);

                fileObjects = new TList<GraphFileObject>();
                graphFileAssocsToAdd = new Dictionary<GraphFileAssociationKey, GraphFileAssociation>();
                //get all objects - save missing ones
                ReportProgress(40, "Saving File Objects");
                string ddd = "";
                #region File Objects
                foreach (GoObject o in Diagram)
                {
                    if (ddd.Length == 3)
                        ddd = "";
                    ddd += ".";
                    if (o is IMetaNode)
                    {
                        if (!(o as IMetaNode).MetaObject.IsInDatabase(Core.Variables.Instance.ClientProvider))
                        {
                            if ((o as IMetaNode).MetaObject.State == VCStatusList.CheckedOutRead)
                            {
                                (o as IMetaNode).SaveToDatabase(this, new DiagramSaveEventArgs((o as IMetaNode).MetaObject.State));
                            }
                            else
                            {
                                (o as IMetaNode).SaveToDatabase(this, EventArgs.Empty);
                            }
                        }
                        SaveFileObject(BuildFileObject(o as IMetaNode));
                    }
                    if (o is GoGroup)
                    {
                        foreach (IMetaNode node in (getNodesInContainer(o as GoGroup)))
                        {
                            if (!node.MetaObject.IsInDatabase(Core.Variables.Instance.ClientProvider))
                            {
                                if (node.MetaObject.State == VCStatusList.CheckedOutRead)
                                {
                                    node.SaveToDatabase(this, new DiagramSaveEventArgs((o as IMetaNode).MetaObject.State));
                                }
                                else
                                {
                                    node.SaveToDatabase(this, EventArgs.Empty);
                                }
                            }
                            SaveFileObject(BuildFileObject(node));
                        }
                    }
                    //swimlanes in particular
                    if (o is MappingCell)
                    {
                        SaveOverlappingNodesInMappingCell(fileObjects, 0, o as MappingCell);
                    }
                    ReportProgress(40, "Collecting Objects" + ddd);
                }
                #endregion
                ReportProgress(80, "Saving File Links");
                #region File Links
                //we have to do links after because links may be loaded before objects have been saved
                //get all associations - save missing ones
                ddd = "";
                foreach (GoObject o in Diagram)
                {
                    if (ddd.Length == 3)
                        ddd = "";
                    ddd += ".";
                    if (o is QLink)
                        if ((o as QLink).IsInDatabase)
                        {
                            BuildFileAssociation(o as QLink);
                            ReportProgress(40, "Collecting Links" + ddd);
                        }
                }
                #endregion
                ReportProgress(90, "Saving Physical File");
                #region GraphFile itself
                FileUtil futilSOC = new FileUtil();
                futilSOC.Save(Diagram, FileName);

                if (graphFileManagerSOC.savedFile != null)
                    graphFileManagerSOC.SaveToDatabase(Diagram, FileName, false);
                #endregion
                ReportProgress(100, "Complete");

                Core.Log.WriteLog(FileName + " save complete");
                return;
            }
            counter = SaveObjectsInContainer(fileObjects, counter, Diagram);

            //SaveMappingCells();
            ReportProgress(40, "Retrieving Links");
            SaveLinksInContainer(Diagram);

            foreach (b.ObjectAssociation oassoc in oassocs)
                DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Save(oassoc);

            ReportProgress(50, "Saving " + associationValues.Count + " Links");
            #region Object Association Values
            bool LoggedGapTypeAssociationSaveFeature = false;
            foreach (KeyValuePair<ObjectAssociation, Tuple> kvp in associationValues)
            {
                ObjectAssociation association = kvp.Key;
                //DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Save(association);

                if (LoggedGapTypeAssociationSaveFeature)
                    break;
                b.ObjectAssociationKey oaKey;
                oaKey = new ObjectAssociationKey(association);
                b.ObjectAssociation oaValue = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Get(oaKey);
                if (oaValue == null)
                {
                    Log.WriteLog("Null assocation after just saved");
                    break;
                }
                //b.ObjectAssociation oaValue = association;

                try
                {
                    if (DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ExecuteScalar(System.Data.CommandType.Text, "select Count(Name) from sysobjects where type='P' and name='PROC_ObjectAssociationValue_GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine'").ToString() == "1")
                    {
                        //get current database value for oaValue
                        System.Data.DataSet values = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ExecuteDataSet("PROC_ObjectAssociationValue_GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine", new object[] { oaValue.CAid.ToString(), oaValue.ObjectID.ToString(), oaValue.ChildObjectID.ToString(), oaValue.ObjectMachine.ToString(), oaValue.ChildObjectMachine.ToString() });
                        if (values.Tables[0].Rows.Count >= 1) //UPDATE
                        {
                            //UPDATE
                            DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ExecuteNonQuery("[PROC_ObjectAssociationValue_Update]", new object[] { oaValue.CAid.ToString(), oaValue.ObjectID.ToString(), oaValue.ChildObjectID.ToString(), oaValue.ObjectMachine.ToString(), oaValue.ChildObjectMachine.ToString(), kvp.Value.First.ToString(), kvp.Value.Second.ToString() });
                        }
                        else
                        {
                            if (kvp.Value.Second.ToString() != "None")
                            {
                                //INSERT
                                DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ExecuteNonQuery("[PROC_ObjectAssociationValue_Insert]", new object[] { oaValue.CAid.ToString(), oaValue.ObjectID.ToString(), oaValue.ChildObjectID.ToString(), oaValue.ObjectMachine.ToString(), oaValue.ChildObjectMachine.ToString(), kvp.Value.First.ToString(), kvp.Value.Second.ToString() });
                            }
                        }
                    }
                    else
                    {
                        LoggedGapTypeAssociationSaveFeature = true;
                    }
                }
                catch (Exception ex)
                {
                    LoggedGapTypeAssociationSaveFeature = true;
                    //probably missing procedures and or table
                    Log.WriteLog("Database update required to save association property values" + Environment.NewLine + ex.ToString());
                }
            }

            #endregion

            ReportProgress(60, "Saving Artifacts");
            foreach (GoObject obj in Diagram)
            {
                #region Fishlinks
                if (obj is FishLink)
                {
                    FishLink flink = obj as FishLink;
                    if (flink.FromPort == null || flink.ToPort == null)
                        flink.Remove();
                    else
                    {
                        #region Handle Artefact
                        IMetaNode nFrom = flink.FromNode as IMetaNode;
                        QLink lnk = ((GoPort)(flink.ToPort)).PortObject.ParentNode as QLink;
                        if (nFrom != null)
                        {
                            if (lnk != null)
                            {
                                IMetaNode fromMetaNode = lnk.FromNode as IMetaNode;
                                IMetaNode toMetaNode = lnk.ToNode as IMetaNode;
                                if (fromMetaNode == null || toMetaNode == null)
                                    continue;
                                // Normal nodes are already saved (afaik)
                                if (nFrom is ArtefactNode)
                                {
                                    ArtefactNode afnode = nFrom as ArtefactNode;

                                    if (!Exists(afnode.MetaObject.WorkspaceTypeId, afnode.MetaObject.WorkspaceName))
                                    {
                                        if (Diagram.VersionManager != null)
                                        {
                                            DocumentVersion version = Diagram.VersionManager.CurrentVersion;
                                            afnode.MetaObject.WorkspaceName = version.WorkspaceName;
                                            afnode.MetaObject.WorkspaceTypeId = version.WorkspaceTypeId;
                                        }
                                        else
                                        {
                                            afnode.MetaObject.WorkspaceName = Core.Variables.Instance.CurrentWorkspaceName;
                                            afnode.MetaObject.WorkspaceTypeId = Core.Variables.Instance.CurrentWorkspaceTypeId;
                                        }
                                    }
                                    if (!afnode.MetaObject.IsSaved)
                                        afnode.MetaObject.Save(Guid.NewGuid());
                                    //afnode.MetaObject.IsInDatabase = true;
                                }

                                Artifact art = new Artifact();
                                art.ArtifactObjectID = nFrom.MetaObject.pkid;
                                art.ArtefactMachine = nFrom.MetaObject.MachineName;
                                art.ObjectID = fromMetaNode.MetaObject.pkid;
                                art.ObjectMachine = fromMetaNode.MetaObject.MachineName;
                                art.ChildObjectID = toMetaNode.MetaObject.pkid;
                                art.ChildObjectMachine = toMetaNode.MetaObject.MachineName;
                                Association association = Singletons.GetAssociationHelper().GetAssociation(fromMetaNode.MetaObject._ClassName, toMetaNode.MetaObject._ClassName, (int)lnk.AssociationType);

                                if (association != null)
                                {
                                    art.CAid = association.ID;
                                    try
                                    {
                                        DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ArtifactProvider.Save(art);
                                        GraphFileObject gfo = new GraphFileObject();
                                        gfo.GraphFileID = Diagram.VersionManager.CurrentVersion.PKID;
                                        gfo.GraphFileMachine = Diagram.VersionManager.CurrentVersion.MachineName;
                                        gfo.MetaObjectID = art.ArtifactObjectID;
                                        gfo.MachineID = art.ArtefactMachine;
                                        if (!fileObjects.Contains(gfo) && nFrom is ArtefactNode)
                                        {
                                            fileObjects.Add(gfo);
                                        }

                                        //Two way artifacts
                                        if (lnk.AssociationType == LinkAssociationType.Mapping)
                                        {
                                            Association twoWayAssociation = Singletons.GetAssociationHelper().GetAssociation(toMetaNode.MetaObject._ClassName, fromMetaNode.MetaObject._ClassName, (int)lnk.AssociationType);
                                            if (twoWayAssociation != null)
                                            {
                                                Artifact twoWayArt = new Artifact();
                                                twoWayArt.CAid = twoWayAssociation.ID;

                                                twoWayArt.ArtifactObjectID = nFrom.MetaObject.pkid;
                                                twoWayArt.ArtefactMachine = nFrom.MetaObject.MachineName;
                                                twoWayArt.ObjectID = toMetaNode.MetaObject.pkid;
                                                twoWayArt.ObjectMachine = toMetaNode.MetaObject.MachineName;
                                                twoWayArt.ChildObjectID = fromMetaNode.MetaObject.pkid;
                                                twoWayArt.ChildObjectMachine = fromMetaNode.MetaObject.MachineName;
                                                DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ArtifactProvider.Save(twoWayArt);
                                            }
                                        }
                                    }
                                    catch (Exception xarts)
                                    {
                                        Core.Log.WriteLog(xarts.ToString());
                                        // Console.WriteLine(xarts.ToString());
                                    }
                                }
                                else
                                {
                                    Log.WriteLog("Association is NULL for artifact save");
                                }
                            }

                        }
                        #endregion
                    }
                }

                #endregion

                #region Rationales
                if (obj is Rationale)
                {
                    Rationale rat = obj as Rationale;
                    if (rat.Anchor != null) //on link
                    {
                        // Check if it is connected to a link
                        if (rat.Anchor.TopLevelObject is QLink)
                        {
                            QLink slink = rat.Anchor.TopLevelObject as QLink;
                            IMetaNode toMetaNode = slink.ToNode as IMetaNode;
                            IMetaNode fromMetaNode = slink.FromNode as IMetaNode;
                            if (fromMetaNode == null || toMetaNode == null)
                                continue;
                            Artifact art = new Artifact();

                            art.ArtifactObjectID = rat.MetaObject.pkid;
                            art.ChildObjectID = toMetaNode.MetaObject.pkid;
                            art.ObjectID = fromMetaNode.MetaObject.pkid;
                            art.ObjectMachine = fromMetaNode.MetaObject.MachineName;
                            art.ChildObjectMachine = toMetaNode.MetaObject.MachineName;
                            art.ArtefactMachine = rat.MetaObject.MachineName;
                            Association association = Singletons.GetAssociationHelper().GetAssociation(fromMetaNode.MetaObject._ClassName, toMetaNode.MetaObject._ClassName, (int)slink.AssociationType);
                            if (association != null)
                            {
                                art.CAid = association.ID;
                                try
                                {
                                    d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ArtifactProvider.Save(art);
                                }
                                catch
                                {

                                }
                            }
                        }
                        else //on node
                        {
                            MetaBase mbaseAnchoredTo = null;
                            if (rat.Anchor is IMetaNode)
                            {
                                IMetaNode node = rat.Anchor as IMetaNode;
                                mbaseAnchoredTo = node.MetaObject;
                            }
                            else if (rat.Anchor is ArtefactNode)
                            {
                                mbaseAnchoredTo = (rat.Anchor as ArtefactNode).MetaObject;
                            }
                            if (mbaseAnchoredTo != null)
                            {
                                Association association = Singletons.GetAssociationHelper().GetAssociation(mbaseAnchoredTo._ClassName, "Rationale", 4); // always a mapping
                                if (association != null)
                                {
                                    b.ObjectAssociationKey oakey = new ObjectAssociationKey(association.ID, mbaseAnchoredTo.pkid, rat.MetaObject.pkid, mbaseAnchoredTo.MachineName, rat.MetaObject.MachineName);
                                    b.ObjectAssociation oa = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Get(oakey);
                                    if (oa == null)
                                    {
                                        oa = new ObjectAssociation();
                                        oa.CAid = oakey.CAid;
                                        oa.ObjectID = oakey.ObjectID;
                                        oa.ObjectMachine = oakey.ObjectMachine;
                                        oa.ChildObjectID = oakey.ChildObjectID;
                                        oa.ChildObjectMachine = oakey.ChildObjectMachine;
                                        d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Save(oa);
                                    }

                                    //Save association to diagram
                                    GraphFileAssociation rationaleGraphFileAssociation = new GraphFileAssociation();
                                    rationaleGraphFileAssociation.GraphFileID = Diagram.VersionManager.CurrentVersion.PKID;
                                    rationaleGraphFileAssociation.GraphFileMachine = Diagram.VersionManager.CurrentVersion.MachineName;

                                    rationaleGraphFileAssociation.CAid = oa.CAid;

                                    rationaleGraphFileAssociation.ObjectID = oa.ObjectID;
                                    rationaleGraphFileAssociation.ObjectMachine = oa.ObjectMachine;
                                    rationaleGraphFileAssociation.ChildObjectID = oa.ChildObjectID;
                                    rationaleGraphFileAssociation.ChildObjectMachine = oa.ChildObjectMachine;

                                    GraphFileAssociationKey gfakey = new GraphFileAssociationKey(rationaleGraphFileAssociation);
                                    if (!(graphFileAssocsToAdd.ContainsKey(gfakey)))
                                        graphFileAssocsToAdd.Add(gfakey, rationaleGraphFileAssociation);
                                }
                                else
                                {
                                    Core.Log.WriteLog("No mapping allowed from " + mbaseAnchoredTo._ClassName + " to Rationale (" + rat.MetaObject.ToString() + " remains unconnected)");
                                }
                            }
                        }
                    }
                }
                #endregion
            }

            // clear the db first
            ReportProgress(65, "Removing Records");
            TList<GraphFileObject> gfos = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileObjectProvider.GetByGraphFileIDGraphFileMachine(Diagram.VersionManager.CurrentVersion.PKID, Diagram.VersionManager.CurrentVersion.MachineName);
            DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileObjectProvider.Delete(gfos);

            ReportProgress(70, "Saving File Record");
            GraphFileManager gfm1 = new GraphFileManager();
            gfm1.SaveToDatabase(Diagram, FileName, true);

            ReportProgress(75, "Saving Object Records");
            #region GraphFileObjects

            foreach (GraphFileObject gfobject in fileObjects)
            {
                if (gfobject.MetaObjectID <= 0) //cannot save graphfileobjects which have 'unsaved' metaobjects
                    continue;

                gfobject.GraphFileID = Diagram.VersionManager.CurrentVersion.PKID;
                gfobject.GraphFileMachine = Diagram.VersionManager.CurrentVersion.MachineName;
                //b.GraphFileObjectKey gfoKey = new GraphFileObjectKey(gfobject);
                //b.GraphFileObject gfo = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileObjectProvider.Get(gfoKey);
                //if (gfo == null)
                DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileObjectProvider.Save(gfobject);
            }
            //DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileObjectProvider.Save(fileObjects);
            #endregion

            ReportProgress(80, "Saving Association Records");
            #region GraphFile Associations
            /*b.GraphFileAssociationKey gfaKey;
                b.GraphFileAssociation gfa;*/
            //TList<GraphFileAssociation> gfasToAdd = new TList<GraphFileAssociation>();
            //foreach (KeyValuePair<GraphFileAssociationKey, GraphFileAssociation> kvp in graphFileAssocsToAdd)
            //{
            //    bool found = false;
            //    foreach (GraphFileAssociation gfaWillBeSaved in gfasToAdd)
            //    {
            //        if (gfaWillBeSaved.CAid == kvp.Value.CAid && gfaWillBeSaved.ObjectID == kvp.Value.ObjectID && gfaWillBeSaved.ChildObjectID == kvp.Value.ChildObjectID)
            //            found = true;
            //    }
            //    if (!found)
            //        gfasToAdd.Add(kvp.Value);
            //}
            foreach (KeyValuePair<GraphFileAssociationKey, GraphFileAssociation> kvp in graphFileAssocsToAdd)//gfasToAdd)
            {
                kvp.Value.GraphFileID = Diagram.VersionManager.CurrentVersion.PKID;
                kvp.Value.GraphFileMachine = Diagram.VersionManager.CurrentVersion.MachineName;
                //GraphFileAssociationKey gfakey = new GraphFileAssociationKey(gfas);
                //if (DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileAssociationProvider.Get(gfakey) == null)
                DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileAssociationProvider.Save(kvp.Value);
                //if (!found)
                //gfasToAdd.Add(kvp.Value);
            }
            //DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileAssociationProvider.Save(gfasToAdd);
            //Console.Write("saving GFAS");

            #endregion

            ReportProgress(90, "Saving File");
            #region GraphFile itself

            //GraphFileManager gfm = new GraphFileManager();
            //gfm.SaveToDatabase(Diagram, FileName, false);
            FileUtil futil = new FileUtil();
            futil.Save(Diagram, FileName);

            #endregion

            ReportProgress(95, "Completing");

            Core.Log.WriteLog(FileName + " save complete");
        }

        private void SaveFileObject(GraphFileObject gfobject)
        {
            gfobject.GraphFileID = Diagram.VersionManager.CurrentVersion.PKID;
            gfobject.GraphFileMachine = Diagram.VersionManager.CurrentVersion.MachineName;
            b.GraphFileObjectKey gfoKey = new GraphFileObjectKey(gfobject);
            b.GraphFileObject gfo = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileObjectProvider.Get(gfoKey);
            if (gfo == null)
                DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileObjectProvider.Save(gfobject);
        }

        //this method does not save it collects
        private void SaveLinksInContainer(IGoCollection container)
        {
            if (allowedAssociations == null)
                allowedAssociations = new Dictionary<ClassAssociationSpec, Association>();

            foreach (GoObject gobject in container)
            {
                if (gobject is IGoCollection)
                {
                    SaveLinksInContainer(gobject as IGoCollection);
                }

                if (gobject is QLink)
                {
                    QLink lnk = gobject as QLink;
                    IMetaNode from = lnk.FromNode as IMetaNode;
                    IMetaNode to = lnk.ToNode as IMetaNode;
                    if (from != null && to != null)
                    {
                        if (from.MetaObject != null && to.MetaObject != null)
                        {
                            try
                            {
                                Association assoc = null;
                                ClassAssociationSpec caspec = new ClassAssociationSpec(from.MetaObject._ClassName, to.MetaObject._ClassName, (int)lnk.AssociationType);
                                if (allowedAssociations.ContainsKey(caspec))
                                {
                                    assoc = allowedAssociations[caspec];
                                }
                                else
                                {
                                    assoc = Singletons.GetAssociationHelper().GetAssociation(from.MetaObject._ClassName, to.MetaObject._ClassName, (int)lnk.AssociationType);
                                    allowedAssociations.Add(caspec, assoc);
                                }
                                if (assoc != null)
                                {
                                    ObjectAssociationKey oakey = new ObjectAssociationKey(assoc.ID, from.MetaObject.pkid, to.MetaObject.pkid, from.MetaObject.MachineName, to.MetaObject.MachineName);
                                    ObjectAssociation oas = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Get(oakey);
                                    if (oas == null)
                                    {
                                        oas = new ObjectAssociation();
                                        oas.ObjectID = from.MetaObject.pkid;
                                        oas.ChildObjectID = to.MetaObject.pkid;
                                        oas.ObjectMachine = from.MetaObject.MachineName;
                                        oas.ChildObjectMachine = to.MetaObject.MachineName;
                                        oas.CAid = assoc.ID;
                                    }

                                    if (oas.VCStatusID == 0)
                                        oas.VCStatusID = (int)VCStatusList.None;
                                    bool alreadyInCollection = false;

                                    #region Add Associations to list to be saved later and add values to dictionary
                                    foreach (ObjectAssociation ass in oassocs)
                                    {
                                        if (ass.CAid == oas.CAid && ass.ObjectID == oas.ObjectID && ass.ObjectMachine == oas.ObjectMachine && ass.ChildObjectID == oas.ChildObjectID && ass.ChildObjectMachine == oas.ChildObjectMachine)
                                        {
                                            alreadyInCollection = true;
                                        }
                                    }

                                    if (!(alreadyInCollection))
                                    {
                                        oassocs.Add(oas);
                                        associationValues.Add(oas, new Tuple("GapType", lnk.GapType.ToString()));
                                    }

                                    try
                                    {
                                        if (lnk.Pen.Color.ToArgb() == Color.Orange.ToArgb())
                                        {
                                            Pen p = new Pen(lnk.PenColorBeforeCompare, lnk.Pen.Width);

                                            if (lnk.PenColorBeforeCompare.ToArgb() == Color.Orange.ToArgb())// && !lnk.IsInDatabase)
                                                p = new Pen(Color.Black, lnk.Pen.Width);

                                            if (lnk.AssociationType == LinkAssociationType.SubSetOf || lnk.AssociationType == LinkAssociationType.MutuallyExclusiveLink)
                                                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

                                            lnk.Pen = p;
                                        }
                                    }
                                    catch (Exception xxPen)
                                    {
                                        Console.WriteLine("Pen Error in DiagramSaver:" + xxPen.ToString());
                                    }

                                    GraphFileAssociation gfass = new GraphFileAssociation();
                                    gfass.GraphFileID = Diagram.VersionManager.CurrentVersion.PKID;
                                    gfass.GraphFileMachine = Diagram.VersionManager.CurrentVersion.MachineName;
                                    gfass.CAid = oas.CAid;
                                    gfass.ObjectID = oas.ObjectID;
                                    gfass.ObjectMachine = oas.ObjectMachine;
                                    gfass.ChildObjectID = oas.ChildObjectID;
                                    gfass.ChildObjectMachine = oas.ChildObjectMachine;
                                    GraphFileAssociationKey gfakey = new GraphFileAssociationKey(gfass);
                                    if (!(graphFileAssocsToAdd.ContainsKey(gfakey)))
                                        graphFileAssocsToAdd.Add(gfakey, gfass);

                                    #endregion
                                }
                            }
                            catch
                            {
                                LogEntry entry = new LogEntry();
                                entry.Message = "Invalid Association - not supported by the MetaModel: " + from.MetaObject._ClassName + " to " + to.MetaObject._ClassName + " Link: " + lnk.AssociationType.ToString();
                                Logger.Write(entry);
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
                //mo.IsInDatabase = true;

                TList<ObjectAssociation> assocs = null;
                if (mo.State != VCStatusList.CheckedOutRead) //CANT SAVE OBJECT IF YOU ARE READING OBJECT  || mo.State != VCStatusList.CheckedIn
                {
                    SetWorkspace(mo);
                    assocs = node.SaveToDatabase(this, EventArgs.Empty);
                }
                else
                {
                    DiagramSaveEventArgs readOnlyEventArgs = new DiagramSaveEventArgs(mo.State);
                    //CAUSES 'BUG' WHERE USERS CANNOT LINK(Save) TO CHECKEDOUTREAD OBJECT :)
                    assocs = node.SaveToDatabase(this, readOnlyEventArgs);
                    //return counter;
                }

                GraphFileObject gfo = new GraphFileObject();
                gfo.GraphFileID = diagram.VersionManager.CurrentVersion.PKID;
                gfo.GraphFileMachine = diagram.VersionManager.CurrentVersion.MachineName;
                gfo.MetaObjectID = node.MetaObject.pkid;
                gfo.MachineID = node.MetaObject.MachineName;
                bool found = false;
                foreach (GraphFileObject obj in fileObjects)
                {
                    if (obj.MetaObjectID == node.MetaObject.pkid && obj.MachineID == node.MetaObject.MachineName)
                    {
                        found = true;
                    }
                }
                if (assocs != null)
                {
                    foreach (ObjectAssociation oas in assocs)
                    {
                        GraphFileAssociation gfass = new GraphFileAssociation();
                        gfass.GraphFileID = gfo.GraphFileID;
                        gfass.GraphFileMachine = gfo.GraphFileMachine;
                        gfass.CAid = oas.CAid;
                        gfass.ObjectID = oas.ObjectID;
                        gfass.ObjectMachine = oas.ObjectMachine;
                        gfass.ChildObjectID = oas.ChildObjectID;
                        gfass.ChildObjectMachine = oas.ChildObjectMachine;
                        GraphFileAssociationKey gfakey = new GraphFileAssociationKey(gfass);
                        if (!(graphFileAssocsToAdd.ContainsKey(gfakey))) //CANT SAVE ASSOCIATION IF YOU ARE READING OBJECT (HANDLED ABOVE)
                            graphFileAssocsToAdd.Add(gfakey, gfass);
                    }
                }
                if (!found) //CANT SAVE TO DIAGRAM IF YOU ARE READING OBJECT (HANDLED ABOVE)
                {
                    fileObjects.Add(gfo);
                }
            }
            return counter;
        }

        public void SetWorkspace(MetaBase mo)
        {
            //Bug Fix : Diagram workspace not updating to database version
            //check if object has been saved and look in database
            MetaObject dbObj = null;
            if (mo.pkid > 0)
            {
                dbObj = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetBypkidMachine(mo.pkid, mo.MachineName);
            }

            //database metaobject > diagram metaobject
            if (dbObj != null)
            {
                //Log.WriteLog(mo.pkid + " found in database with workspace " + dbObj.WorkspaceName);
                mo.WorkspaceName = dbObj.WorkspaceName;
                mo.WorkspaceTypeId = (int)dbObj.WorkspaceTypeId;
            }
            else
            {
                if (Diagram.VersionManager != null)
                {
                    DocumentVersion version = Diagram.VersionManager.CurrentVersion;
                    if (Exists(version.WorkspaceTypeId, version.WorkspaceName))
                    {
                        //Log.WriteLog(mo.pkid + " workspace set to " + version.WorkspaceName);
                        mo.WorkspaceName = version.WorkspaceName;
                        mo.WorkspaceTypeId = version.WorkspaceTypeId;
                    }
                    else
                    {
                        //Log.WriteLog("Workspace " + version.WorkspaceName + " does not exist locally");
                        mo.WorkspaceName = Core.Variables.Instance.CurrentWorkspaceName;
                        mo.WorkspaceTypeId = Core.Variables.Instance.CurrentWorkspaceTypeId;
                    }
                }
                else
                {
                    //Log.WriteLog("Diagram version is null : " + diagram.Name);
                    mo.WorkspaceName = Core.Variables.Instance.CurrentWorkspaceName;
                    mo.WorkspaceTypeId = Core.Variables.Instance.CurrentWorkspaceTypeId;
                }
            }
        }

        private int SaveObjectsInContainer(TList<GraphFileObject> fileObjects, int counter, IGoCollection collection)
        {
            string s = "";
            ReportProgress(20, "Saving Objects");
            foreach (GoObject gobject in collection)
            {
                if (s == ".....")
                    s = "";
                s += ".";
                ReportProgress(20, "Saving Objects" + s);
                if (gobject is IMetaNode)
                {
                    if ((gobject as IMetaNode).MetaObject == null || (gobject as IMetaNode).MetaObject.IsSaved)
                        continue;

                    if (gobject is CollapsibleNode)
                    {
                        foreach (GoObject gA in (gobject as CollapsibleNode))
                            if (gA is IGoCollection)
                                foreach (GoObject gB in (gA as IGoCollection))
                                    if (gB is IGoCollection)
                                        foreach (GoObject g in (gB as IGoCollection))
                                            if (g is CollapsingRecordNodeItem)
                                                if (!((g as CollapsingRecordNodeItem).IsHeader))
                                                    SetWorkspace((g as CollapsingRecordNodeItem).MetaObject);
                    }

                    //ReportProgress(20, "Saving Objects" + s); counter
                    counter = SaveNode(fileObjects, counter, gobject);
                }
                if (gobject is IGoCollection)
                {
                    counter = SaveObjectsInContainer(fileObjects, counter, gobject as IGoCollection);
                }
            }
            if (collection is MappingCell)
            {
                ReportProgress(20, "Saving Objects");
                MappingCell mapCell = collection as MappingCell;
                if (mapCell.MetaObject != null)
                {
                    //if (mapCell.MetaObject.pkid <= 0)
                    if (!mapCell.MetaObject.IsSaved)
                        mapCell.MetaObject.Save(Guid.NewGuid());

                    if (mapCell.MetaObject.pkid > 0)
                    {
                        // string WS = mapCell.MetaObject.WorkspaceName;
                        b.GraphFileObject gfo = new GraphFileObject();
                        gfo.GraphFileID = Diagram.VersionManager.CurrentVersion.PKID;
                        gfo.GraphFileMachine = Diagram.VersionManager.CurrentVersion.MachineName;
                        gfo.MetaObjectID = mapCell.MetaObject.pkid;
                        gfo.MachineID = mapCell.MetaObject.MachineName;
                        //mcell is imetanode thus we should not add it again because it was already added above
                        bool found = false;
                        foreach (GraphFileObject obj in fileObjects)
                        {
                            if (obj.MetaObjectID == mapCell.MetaObject.pkid && obj.MachineID == mapCell.MetaObject.MachineName)
                            {
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                            fileObjects.Add(gfo);
                    }
                    //Added to force addition of METAMODEL LANES
                    SaveOverlappingNodesInMappingCell(fileObjects, counter, mapCell);
                }
            }
            if (collection is ILinkedContainer)
            {
                ReportProgress(20, "Saving Objects");
                ILinkedContainer sgnode = collection as ILinkedContainer;
                if (sgnode is IMetaNode)
                {
                    IMetaNode imnode = sgnode as IMetaNode;
                    imnode.RequiresAttention = false;
                }
                foreach (EmbeddedRelationship er in sgnode.ObjectRelationships)
                {
                    if (er.MyAssociation != null)
                    {
                        b.ObjectAssociation oa = new ObjectAssociation();
                        b.GraphFileAssociation gfassoc = new GraphFileAssociation();

                        if (er.MyMetaObject.pkid == 0)
                        {
                            SetWorkspace(er.MyMetaObject);
                            er.MyMetaObject.SaveToRepository(Guid.NewGuid(), Core.Variables.Instance.ClientProvider);
                        }
                        gfassoc.CAid = oa.CAid = er.MyAssociation.CAid;
                        gfassoc.ObjectID = oa.ObjectID = sgnode.MetaObject.pkid;
                        gfassoc.ObjectMachine = oa.ObjectMachine = sgnode.MetaObject.MachineName;
                        gfassoc.ChildObjectID = oa.ChildObjectID = er.MyMetaObject.pkid;
                        gfassoc.ChildObjectMachine = oa.ChildObjectMachine = er.MyMetaObject.MachineName;

                        gfassoc.GraphFileID = Diagram.VersionManager.CurrentVersion.PKID;
                        gfassoc.GraphFileMachine = Diagram.VersionManager.CurrentVersion.MachineName;

                        if ((oa.VCStatusID == 0) || (oa.VCStatusID == (int)VCStatusList.MarkedForDelete))
                            oa.VCStatusID = (int)VCStatusList.None;
                        b.ObjectAssociationKey oakey = new ObjectAssociationKey(oa);
                        ObjectAssociation oaExisting = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Get(oakey);
                        if (oaExisting == null)
                            d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Save(oa);
                        else //added 4 january 2013
                            d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Update(oa);
                        GraphFileAssociationKey gfkey = new GraphFileAssociationKey(gfassoc);
                        if (!graphFileAssocsToAdd.ContainsKey(gfkey))
                            graphFileAssocsToAdd.Add(gfkey, gfassoc);
                    }
                }
            }

            SaveLinksInContainer(collection);

            return counter;
        }

        private void SaveOverlappingNodesInMappingCell(TList<GraphFileObject> fileObjects, int counter, MappingCell node)
        {
            List<IMetaNode> overlappingNodes = node.GetOverlappingIMetaNodes();
            foreach (IMetaNode overlappingNode in overlappingNodes)
            {
                if (overlappingNode is ArtefactNode || overlappingNode is Rationale)
                    continue;
                if (overlappingNode.MetaObject != null)
                {
                    if (!Core.Variables.Instance.SaveOnCreate)
                    {
                        ReportProgress(20, "Saving Overlapping");
                        //save node
                        if (overlappingNode.MetaObject.IsSaved == false)
                            overlappingNode.MetaObject.SaveToRepository(Guid.NewGuid(), Core.Variables.Instance.ClientProvider);
                    }
                    //find CAID

                    //TList<ClassAssociation> classa = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByParentClass(node.MetaObject.Class);
                    int CAID = 0;
                    foreach (AllowedAssociationInfo info in MetaBuilder.BusinessFacade.MetaHelper.Singletons.GetAssociationHelper().GetAllowedAssociationInfoCollection(node.MetaObject.Class, overlappingNode.MetaObject.Class))
                    {
                        //if (info.ParentClass == node.MetaObject.Class && info.ChildClass == overlappingNode.MetaObject.Class)
                        //{
                        if (CAID == 0) //set to first 1
                            CAID = info.ClassAssociationID;
                        if (info.IsDefault)
                        {
                            CAID = info.ClassAssociationID;
                            break;
                        }
                        //}
                    }
                    //CAID = MetaBuilder.BusinessFacade.MetaHelper.Singletons.GetAssociationHelper().GetAssociation(node.MetaObject.Class, overlappingNode.MetaObject.Class, 0).ID;
                    //TList<ClassAssociation> allowed = new TList<ClassAssociation>();
                    //foreach (ClassAssociation c in classa)
                    //{
                    //    if (c.ChildClass == overlappingNode.MetaObject.Class)
                    //    {
                    //        allowed.Add(c);
                    //    }
                    //}
                    //if (allowed.Count == 1)
                    //{
                    //    CAID = allowed[0].CAid;
                    //}
                    //else
                    //{
                    //    foreach (ClassAssociation c in allowed)
                    //    {
                    //        if (c.IsDefault)
                    //            CAID = c.CAid;
                    //    }
                    //    if (CAID == 0 && allowed.Count > 0) //use the first 1
                    //        CAID = allowed[0].CAid;
                    //}

                    #region association

                    if (CAID > 0)
                    {
                        //save association
                        ObjectAssociation ass = new ObjectAssociation();
                        GraphFileAssociation gfassocM = new GraphFileAssociation();
                        gfassocM.ObjectID = ass.ObjectID = node.MetaObject.pkid;
                        gfassocM.ObjectMachine = ass.ObjectMachine = node.MetaObject.MachineName;

                        gfassocM.ChildObjectID = ass.ChildObjectID = overlappingNode.MetaObject.pkid;
                        gfassocM.ChildObjectMachine = ass.ChildObjectMachine = overlappingNode.MetaObject.MachineName;

                        gfassocM.CAid = ass.CAid = CAID;

                        gfassocM.GraphFileID = Diagram.VersionManager.CurrentVersion.PKID;
                        gfassocM.GraphFileMachine = Diagram.VersionManager.CurrentVersion.MachineName;

                        if ((ass.VCStatusID == 0) || (ass.VCStatusID == (int)VCStatusList.MarkedForDelete))
                            ass.VCStatusID = (int)VCStatusList.None;
                        b.ObjectAssociationKey asskey = new ObjectAssociationKey(ass);
                        ObjectAssociation oaExisting = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Get(asskey);
                        if (oaExisting == null)
                            d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Save(ass);
                        GraphFileAssociationKey gfkeyM = new GraphFileAssociationKey(gfassocM);
                        if (!graphFileAssocsToAdd.ContainsKey(gfkeyM))
                            graphFileAssocsToAdd.Add(gfkeyM, gfassocM);

                    }
                    else
                    {
                        //Does not match metamodel
                        Log.WriteLog("DiagramSaver::Cannot associate(MappingCell) " + node.MetaObject.ToString() + "(" + node.MetaObject.Class + ") to " + overlappingNode.MetaObject.ToString() + "(" + overlappingNode.MetaObject.Class + ") because the metamodel does not allow it.");
                    }

                    #endregion

                    if (!Core.Variables.Instance.SaveOnCreate)
                    {
                        if (overlappingNode.MetaObject.pkid > 0)
                        {
                            b.GraphFileObject gfoM = new GraphFileObject();
                            gfoM.GraphFileID = Diagram.VersionManager.CurrentVersion.PKID;
                            gfoM.GraphFileMachine = Diagram.VersionManager.CurrentVersion.MachineName;
                            gfoM.MetaObjectID = overlappingNode.MetaObject.pkid;
                            gfoM.MachineID = overlappingNode.MetaObject.MachineName;

                            bool found = false;
                            foreach (GraphFileObject obj in fileObjects)
                            {
                                if (obj.MetaObjectID == overlappingNode.MetaObject.pkid && obj.MachineID == overlappingNode.MetaObject.MachineName)
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                                fileObjects.Add(gfoM);

                        }
                    }
                }
            }
        }

        protected override void OnProgressChanged(System.ComponentModel.ProgressChangedEventArgs e)
        {
            Message = e.UserState.ToString();
            amount = e.ProgressPercentage;
            base.OnProgressChanged(e);
        }

        private string message;
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
        private int amount;
        public int Amount
        {
            get { return amount; }
            set { amount = value; }
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

            public ClassAssociationSpec(string pc, string cc, int limitToType)
            {
                parentClass = pc;
                ChildClass = cc;
                LimitToType = limitToType;
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

            public override int GetHashCode()
            {
                return this.ToString().GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (obj != null && obj is ClassAssociationSpec)
                {
                    if ((obj as ClassAssociationSpec).ToString() == this.ToString())
                        return true;
                }
                return false;
                return base.Equals(obj);
            }

            public override string ToString()
            {
                return ParentClass + "-->" + LimitToType.ToString() + "-->" + ChildClass;
            }

        }
        #endregion Nested Classes

        //private void SaveMappingCells()
        //{
        //    return;
        //    foreach (GoObject g in Diagram)
        //    {
        //        if (g is MappingCell)
        //        {
        //            MappingCell mc = g as MappingCell;
        //            if (mc.MetaObject != null)
        //            {
        //                b.TList<b.ClassAssociation> allowedAssociations = d.DataRepository.ClassAssociationProvider.GetByParentClass(mc.MetaObject._ClassName);

        //                List<IMetaNode> imetas = mc.GetOverlappingIMetaNodes(view);
        //                foreach (IMetaNode imnode in imetas)
        //                {
        //                    allowedAssociations.Filter = "ChildClass = '" + imnode.MetaObject._ClassName + "' and AssociationTypeID = 4";
        //                    if (allowedAssociations.Count > 0)
        //                    {
        //                        b.ObjectAssociation oa = new ObjectAssociation();
        //                        b.GraphFileAssociation gfassoc = new GraphFileAssociation();

        //                        gfassoc.CAid = oa.CAid = allowedAssociations[0].CAid;
        //                        gfassoc.ObjectID = oa.ObjectID = mc.MetaObject.pkid;
        //                        gfassoc.ObjectMachine = oa.ObjectMachine = mc.MetaObject.MachineName;
        //                        gfassoc.ChildObjectID = oa.ChildObjectID = imnode.MetaObject.pkid;
        //                        gfassoc.ChildObjectMachine = oa.ChildObjectMachine = imnode.MetaObject.MachineName;
        //                        gfassoc.GraphFileMachine = Diagram.VersionManager.CurrentVersion.MachineName;
        //                        gfassoc.GraphFileID = Diagram.VersionManager.CurrentVersion.PKID;
        //                        b.ObjectAssociationKey oaKey = new ObjectAssociationKey(oa);
        //                        ObjectAssociation oaExisting = d.DataRepository.ObjectAssociationProvider.Get(oaKey);
        //                        if (oaExisting == null)
        //                            d.DataRepository.ObjectAssociationProvider.Save(oa);
        //                        GraphFileAssociationKey gfakey = new GraphFileAssociationKey(gfassoc);
        //                        if (!graphFileAssocsToAdd.ContainsKey(gfakey))
        //                        {
        //                            graphFileAssocsToAdd.Add(gfakey, gfassoc);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //private void SetWorkspace(MetaBase mo)
        //{
        //    if (!Exists(mo.WorkspaceTypeId, mo.WorkspaceName))
        //    {
        //        if (diagram.VersionManager != null)
        //        {
        //            DocumentVersion version = diagram.VersionManager.CurrentVersion;
        //            mo.WorkspaceName = version.WorkspaceName;
        //            mo.WorkspaceTypeId = version.WorkspaceTypeId;
        //        }
        //        else
        //        {
        //            mo.WorkspaceName = Core.Variables.Instance.CurrentWorkspaceName;
        //            mo.WorkspaceTypeId = Core.Variables.Instance.CurrentWorkspaceTypeId;
        //        }
        //    }
        //}

    }
}