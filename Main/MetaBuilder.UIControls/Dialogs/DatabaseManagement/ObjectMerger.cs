using System.Collections;
using System.Collections.Generic;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.DataAccessLayer.OldCode.Diagramming;
using MetaBuilder.DataAccessLayer.OldCode.Meta;
using MetaBuilder.Meta;
using MetaBuilder.MetaControls;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
using System.Windows.Forms;
using System;
using System.Reflection;

namespace MetaBuilder.UIControls.Dialogs.DatabaseManagement
{
    public class ObjectMerger
    {

        #region Fields (2)
        //private bool filesHaveBeenProcessed;
        public bool FilesHaveBeenProcessed
        {
            get { return filesToCheck.Count > 0; }
            //set { filesHaveBeenProcessed = value; }
        }
        public List<FileSpec> filesToCheck;
        //List<GraphFile> openedFiles;

        #endregion Fields

        private bool Server;
        public string Provider { get { return Server ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider; } }
        public void ExecuteMerge(List<DuplicateObjectSpec> specs, bool server)
        {
            Server = server;
            Hashtable hashFiles = new Hashtable();

            TempFileGraphAdapter fileAdapter = new TempFileGraphAdapter();
            List<KeyValuePair<MetaBase, MetaBase>> replacements = new List<KeyValuePair<MetaBase, MetaBase>>();
            TList<GraphFileObject> fileObjectsToAdd = new TList<GraphFileObject>();
            foreach (DuplicateObjectSpec spec in specs)
            {
                foreach (KeyValuePair<MetaBase, MetaBase> pair in spec.Replacements)
                {
                    replacements.Add(pair);
                    //Changed value to key
                    TList<GraphFile> files = fileAdapter.GetFilesByObjectId(pair.Key.pkid, pair.Key.MachineName, Server);
                    foreach (GraphFile f in files)
                    {
                        string fileKey = f.pkid.ToString() + f.Machine;
                        if (!hashFiles.ContainsKey(fileKey))
                            hashFiles.Add(fileKey, f);
                        if (Server)
                        {
                            GraphFileObject newGraphFileObject = new GraphFileObject();
                            newGraphFileObject.GraphFileID = f.pkid;
                            newGraphFileObject.GraphFileMachine = f.Machine;
                            newGraphFileObject.MetaObjectID = pair.Value.pkid;
                            newGraphFileObject.MachineID = pair.Value.MachineName;

                            if (!fileObjectsToAdd.Contains(newGraphFileObject))
                                fileObjectsToAdd.Add(newGraphFileObject);
                        }
                    }
                }
            }

            bool mergeValues = false;

            foreach (DuplicateObjectSpec spec in specs)
            {

                Dictionary<string, List<string>> mergedValues = new Dictionary<string, List<string>>(); ;
                MetaBase newObj = null;
                foreach (KeyValuePair<MetaBase, MetaBase> kvp in spec.Replacements)
                {
                    if (Core.Variables.Instance.MergeDuplicateBehaviour.Length > 0 && Core.Variables.Instance.MergeDuplicateBehaviour != "None")
                    {
                        mergeValues = true;

                        #region Build Merge Values
                        if (newObj == null)
                        {
                            newObj = kvp.Value;
                            foreach (PropertyInfo pInfo in kvp.Value.GetMetaPropertyList(false))
                            {
                                List<string> value = new List<string>();
                                object val = kvp.Value.Get(pInfo.Name);
                                if (val != null && val.ToString().Length > 0)
                                    value.Add(val.ToString());
                                mergedValues.Add(pInfo.Name, value);
                            }
                        }

                        foreach (PropertyInfo pInfo in kvp.Key.GetMetaPropertyList(false))
                        {
                            List<string> value = new List<string>();
                            object val = kvp.Key.Get(pInfo.Name);
                            if (val != null && val.ToString().Length > 0)
                            {
                                List<string> values = mergedValues[pInfo.Name];
                                if (!values.Contains(val.ToString())) //dont add duplicates
                                {
                                    values.Add(val.ToString());
                                }
                            }
                        }
                        #endregion
                    }
                    ReplaceObject(kvp.Key, kvp.Value);
                }

                if (mergeValues)
                {
                    #region Merge Values

                    //Add Merge
                    if (Core.Variables.Instance.MergeDuplicateBehaviour == "Concatenate")
                    {
                        foreach (KeyValuePair<string, List<string>> kvp in mergedValues)
                        {
                            string newValue = "";
                            if (kvp.Value.Count == 1)
                            {
                                newValue = kvp.Value[0];
                            }
                            else if (kvp.Value.Count > 1)
                            {
                                foreach (string s in kvp.Value)
                                {
                                    newValue += s + "|";
                                }
                                newValue = newValue.TrimEnd('|');
                            }
                            if (newValue.Length > 0)
                            {
                                newObj.SetWithoutChange(kvp.Key, newValue);
                            }
                        }
                    }
                    else if (Core.Variables.Instance.MergeDuplicateBehaviour == "FirstValue")
                    {
                        //First come first serve merge
                        foreach (KeyValuePair<string, List<string>> kvp in mergedValues)
                        {
                            string newValue = "";
                            foreach (string s in kvp.Value)
                            {
                                if (s.Length > 0)
                                {
                                    newValue = s;
                                    break;
                                }
                            }
                            if (newValue.Length > 0)
                            {
                                newObj.SetWithoutChange(kvp.Key, newValue);
                            }
                        }
                    }
                    else if (Core.Variables.Instance.MergeDuplicateBehaviour == "Manual")
                    {
                        //User merge
                    }
                    #endregion

                    newObj.SaveToRepository(Guid.NewGuid(), Server ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider);
                }
            }

            filesToCheck = new List<FileSpec>();
            MetaBuilder.Graphing.Persistence.GraphFileManager gfm = new MetaBuilder.Graphing.Persistence.GraphFileManager();
            MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter tmpAdapter = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();
            gfm.Server = server;
            foreach (GraphFile f in hashFiles.Values)
            {
                GraphFile fullFile = tmpAdapter.GetQuickFileDetails(f.WorkspaceTypeId, f.WorkspaceName, f.pkid, f.Machine, (Provider == Core.Variables.Instance.ServerProvider));// DataRepository.Connections[Provider].Provider.GraphFileProvider.GetBypkidMachine(f.pkid, f.Machine);
                gfm.ReplaceMetaObjectsInDatabaseFile(fullFile, replacements, Provider);
                //Check in server file
                if (Server)
                {
                    fullFile.VCStatusID = 1;
                    fullFile.VCMachineID = strings.GetVCIdentifier();
                    fullFile.Notes = "Duplicates Merged";
                    fullFile.ModifiedDate = DateTime.Now;
                    DataRepository.Connections[Provider].Provider.GraphFileProvider.Save(fullFile);
                }
                filesToCheck.Add(new FileSpec(f.pkid, strings.GetFileNameOnly(f.Name)));
            }

            //add gfo for new object for this file
            foreach (GraphFileObject graphFileObject in fileObjectsToAdd)
            {
                try
                {
                    d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.GraphFileObjectProvider.Save(graphFileObject);
                }
                catch
                {
                    //this happens when we save the same object to the same file more than once
                }
            }

            /*
            foreach (DuplicateObjectSpec spec in specs)
            {
                if (spec.Replacements.Count > 0)
                    MergeSpec(spec);
            }*/

        }
        private void ReplaceObject(MetaBase old, MetaBase newMB)
        {

            /* TASKS:
             * 1. Replace Associations where old is Parent
             * 1.1 Replace GraphFileAssociations where old is Parent
             * 2. Replace Associations where old is Child
             * 2.1 Replace GraphFileAssociations where old is Child
             * 3. Replace Artefacts where old is Artefact
             * 4. Replace Active GraphFileObjects where old exists (as IMetaObject)
             * 5. Keep these files open
             * */
            TList<ObjectAssociation> newAssocs = ReplaceAssociations(old, newMB);
            //            ReplaceOnFiles(old, newMB)

            //checkin newMB
            MetaObject obj = DataAccessLayer.DataRepository.Connections[Provider].Provider.MetaObjectProvider.GetBypkidMachine(newMB.pkid, newMB.MachineName);
            if (obj != null && Server) //THE OBJECT CAN NEVER BE NULL AND MUST ONLY AFFECT THE SERVER!
            {
                obj.VCStatusID = 1;
                obj.VCMachineID = strings.GetVCIdentifier();
                DataAccessLayer.DataRepository.Connections[Provider].Provider.MetaObjectProvider.Save(obj);
                newMB.State = (VCStatusList)obj.VCStatusID;
                newMB.VCUser = obj.VCMachineID;
            }
        }

        private TList<ObjectAssociation> ReplaceAssociations(MetaBase old, MetaBase newMB)
        {
            TList<ObjectAssociation> retval = new TList<ObjectAssociation>();
            TList<ObjectAssociation> oldAssociationsAsParent = DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(old.pkid, old.MachineName);
            TList<ObjectAssociation> oldAssociationsAsChild = DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.GetByChildObjectIDChildObjectMachine(old.pkid, old.MachineName);

            foreach (ObjectAssociation oldParent in oldAssociationsAsParent)
            {
                ReplaceAssociationsAsParent(oldParent, newMB, retval);
            }

            foreach (ObjectAssociation oldChild in oldAssociationsAsChild)
            {
                ReplaceAssociationsAsChild(newMB, oldChild, retval);
            }

            foreach (ObjectAssociation newass in retval)
            {
                try
                {
                    d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Save(newass);
                }
                catch (System.Exception ex)
                {
                    Core.Log.WriteLog("Error Merging - Save New Association " + ex.ToString() + System.Environment.NewLine + newass.ToString());
                }
            }
            ReplaceArtefacts(old, newMB, retval, oldAssociationsAsChild, oldAssociationsAsParent);
            return retval;
        }
        private void ReplaceAssociationsAsChild(MetaBase newMB, ObjectAssociation oldChild, TList<ObjectAssociation> retval)
        {
            ObjectAssociation newAssoc = new ObjectAssociation();
            newAssoc.CAid = oldChild.CAid;
            newAssoc.ChildObjectID = newMB.pkid;
            newAssoc.ChildObjectMachine = newMB.MachineName;
            newAssoc.VCMachineID = oldChild.VCMachineID;
            newAssoc.VCStatusID = oldChild.VCStatusID;
            newAssoc.VCUser = oldChild.VCUser;
            newAssoc.Series = oldChild.Series;
            newAssoc.Machine = oldChild.Machine;
            newAssoc.ObjectID = oldChild.ObjectID;
            newAssoc.ObjectMachine = oldChild.ObjectMachine;
            retval.Add(newAssoc);
            ObjectAssociation oa = DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(newAssoc.CAid, newAssoc.ObjectID, newAssoc.ChildObjectID, newAssoc.ObjectMachine, newAssoc.ChildObjectMachine);
            if (oa == null)
                DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.Save(newAssoc);

            //GFA
        }
        private void ReplaceAssociationsAsParent(ObjectAssociation oldParent, MetaBase newMB, TList<ObjectAssociation> retval)
        {
            ObjectAssociation newAssoc = new ObjectAssociation();
            newAssoc.CAid = oldParent.CAid;
            newAssoc.ChildObjectID = oldParent.ChildObjectID;
            newAssoc.ChildObjectMachine = oldParent.ChildObjectMachine;
            newAssoc.VCMachineID = oldParent.VCMachineID;
            newAssoc.VCStatusID = oldParent.VCStatusID;
            newAssoc.VCUser = oldParent.VCUser;
            newAssoc.Series = oldParent.Series;
            newAssoc.Machine = oldParent.Machine;
            newAssoc.ObjectID = newMB.pkid;
            newAssoc.ObjectMachine = newMB.MachineName;
            retval.Add(newAssoc);
            ObjectAssociation oa = DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(newAssoc.CAid, newAssoc.ObjectID, newAssoc.ChildObjectID, newAssoc.ObjectMachine, newAssoc.ChildObjectMachine);
            if (oa == null)
                DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.Save(newAssoc);

            //GFA
        }

        private void ReplaceArtefacts(MetaBase old, MetaBase newMB, TList<ObjectAssociation> newAssocs, TList<ObjectAssociation> oldAsChild, TList<ObjectAssociation> oldAsParent)
        {
            TList<Artifact> artsWhereOldIsParent = DataRepository.Connections[Provider].Provider.ArtifactProvider.GetByObjectIDObjectMachine(old.pkid, old.MachineName);
            TList<Artifact> artsWhereOldIsChild = DataRepository.Connections[Provider].Provider.ArtifactProvider.GetByChildObjectIDChildObjectMachine(old.pkid, old.MachineName);
            TList<Artifact> artsWhereOldIsArtefact = DataRepository.Connections[Provider].Provider.ArtifactProvider.GetByArtifactObjectIDArtefactMachine(old.pkid, old.MachineName);
            TList<Artifact> newArts = new TList<Artifact>();

            // Replace where from object is old
            foreach (Artifact art in artsWhereOldIsParent)
            {
                ReplaceArtefactWhereOldIsParent(newMB, art, newArts);
            }

            // Replace where TO object is old
            foreach (Artifact art in artsWhereOldIsChild)
            {
                ReplaceArtefactWhereOldIsChild(newMB, newArts, art);
            }

            // Replace where TO object is the artefact
            foreach (Artifact art in artsWhereOldIsArtefact)
            {
                ReplaceWhereOldIsArtefact(newMB, newArts, art);
            }

            // Replace diagram objects!
            /*
            foreach (b.ObjectAssociation assocAsChild in oldAsChild)
            {
                b.TList<b.GraphFileAssociation> oldGFOAssoc = d.DataRepository.GraphFileAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(assocAsChild.CAid, assocAsChild.ObjectID, assocAsChild.ChildObjectID, assocAsChild.ObjectMachine, assocAsChild.ChildObjectMachine);
                foreach (b.GraphFileAssociation gfo  in oldGFOAssoc)
                {
                    gfo.ChildObjectID = newMB.pkid;
                    gfo.ChildObjectMachine = newMB.MachineName;
                    b.GraphFileAssociation gfoExisting = d.DataRepository.GraphFileAssociationProvider.GetByGraphFileIDGraphFileMachineChildObjectMachineCAidObjectIDChildObjectIDObjectMachine(
                    if (gfoExisting==null)
                    d.DataRepository.GraphFileAssociationProvider.Save(gfo);
                }
            }
            foreach (b.ObjectAssociation assocAsChild in oldAsParent)
            {
                b.TList<b.GraphFileAssociation> oldGFOAssoc = d.DataRepository.GraphFileAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(assocAsChild.CAid, assocAsChild.ObjectID, assocAsChild.ChildObjectID, assocAsChild.ObjectMachine, assocAsChild.ChildObjectMachine);
                foreach (b.GraphFileAssociation gfo in oldGFOAssoc)
                {
                    gfo.ObjectID =  newMB.pkid;
                    gfo.ObjectMachine = newMB.MachineName;
                    b.GraphFileAssociation gfoExisting = d.DataRepository.GraphFileAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(assocAsChild.CAid, gfo.ObjectID, assocAsChild.ChildObjectID, gfo.ObjectMachine, assocAsChild.ChildObjectMachine);
                    if (gfoExisting==null)
                    d.DataRepository.GraphFileAssociationProvider.Save(gfo);
                }
            }*/

            DataRepository.Connections[Provider].Provider.ArtifactProvider.Save(newArts);
            // now just delete all old artefacts or instances in associations
            DataRepository.Connections[Provider].Provider.ArtifactProvider.Delete(artsWhereOldIsArtefact);
            DataRepository.Connections[Provider].Provider.ArtifactProvider.Delete(artsWhereOldIsChild);
            DataRepository.Connections[Provider].Provider.ArtifactProvider.Delete(artsWhereOldIsParent);

            foreach (ObjectAssociation assoc in oldAsChild)
            {
                RemoveGraphFileAssocs(assoc);
            }
            foreach (ObjectAssociation assoc in oldAsParent)
            {
                RemoveGraphFileAssocs(assoc);
            }

            DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.Delete(oldAsChild);
            DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.Delete(oldAsParent);
            TList<GraphFileObject> oldGFO = DataRepository.Connections[Provider].Provider.GraphFileObjectProvider.GetByMetaObjectIDMachineID(old.pkid, old.MachineName);
            DataRepository.Connections[Provider].Provider.GraphFileObjectProvider.Delete(oldGFO);
            TList<ObjectFieldValue> ofvals = DataRepository.Connections[Provider].Provider.ObjectFieldValueProvider.GetByObjectIDMachineID(old.pkid, old.MachineName);
            DataRepository.Connections[Provider].Provider.ObjectFieldValueProvider.Delete(ofvals);
            if (objadapter == null)
                objadapter = new ObjectAdapter();
            objadapter.DeleteObject(old.pkid, old.MachineName, Server);
            DataRepository.Connections[Provider].Provider.MetaObjectProvider.Delete(old.pkid, old.MachineName);
        }

        private ObjectAdapter objadapter;

        private void ReplaceArtefactWhereOldIsParent(MetaBase newMB, Artifact art, TList<Artifact> newArts)
        {
            Artifact artNew = new Artifact();
            artNew.CAid = art.CAid;

            artNew.ObjectID = newMB.pkid;
            artNew.ObjectMachine = newMB.MachineName;

            artNew.ChildObjectID = art.ChildObjectID;
            artNew.ChildObjectMachine = art.ChildObjectMachine;

            artNew.ArtifactObjectID = art.ArtifactObjectID;
            artNew.ArtefactMachine = art.ArtefactMachine;

            newArts.Add(artNew);
        }
        private void ReplaceArtefactWhereOldIsChild(MetaBase newMB, TList<Artifact> newArts, Artifact art)
        {
            Artifact artNew = new Artifact();

            artNew.CAid = art.CAid;

            artNew.ObjectID = art.ObjectID;
            artNew.ObjectMachine = art.ObjectMachine;

            artNew.ChildObjectID = newMB.pkid;
            artNew.ChildObjectMachine = newMB.MachineName;

            artNew.ArtifactObjectID = art.ArtifactObjectID;
            artNew.ArtefactMachine = art.ArtefactMachine;

            newArts.Add(artNew);
        }
        private void ReplaceWhereOldIsArtefact(MetaBase newMB, TList<Artifact> newArts, Artifact art)
        {
            Artifact artNew = new Artifact();
            artNew.CAid = art.CAid;

            artNew.ObjectID = art.ObjectID;
            artNew.ObjectMachine = art.ObjectMachine;

            artNew.ChildObjectID = art.ChildObjectID;
            artNew.ChildObjectMachine = art.ChildObjectMachine;

            //artNew.ArtifactObjectID = art.ArtifactObjectID;
            //artNew.ArtefactMachine = art.ArtefactMachine;

            artNew.ArtifactObjectID = newMB.pkid;
            artNew.ArtefactMachine = newMB.MachineName;

            newArts.Add(artNew);
        }
        private void RemoveGraphFileAssocs(ObjectAssociation assoc)
        {
            TList<GraphFileAssociation> fileassocs = DataRepository.Connections[Provider].Provider.GraphFileAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(assoc.CAid, assoc.ObjectID, assoc.ChildObjectID, assoc.ObjectMachine, assoc.ChildObjectMachine);
            DataRepository.Connections[Provider].Provider.GraphFileAssociationProvider.Delete(fileassocs);
        }

        #region Nested Classes (1)

        public class FileSpec
        {
            #region Fields (3)

            private string name;
            private int pkid;
            private bool replaced;

            #endregion Fields

            #region Constructors (1)

            public FileSpec(int pkid, string name)
            {
                this.Pkid = pkid;
                this.Name = name;
            }

            #endregion Constructors

            #region Properties (3)

            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            public int Pkid
            {
                get { return pkid; }
                set { pkid = value; }
            }

            public bool Replaced
            {
                get { return replaced; }
                set { replaced = value; }
            }

            #endregion Properties

        }

        #endregion Nested Classes

    }
}