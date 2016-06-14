using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Meta;
using MetaBuilder.MetaControls;
using MetaBuilder.UIControls.GraphingUI;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using c = Microsoft.Practices.EnterpriseLibrary.Caching;

namespace MetaBuilder.UIControls.Dialogs.DatabaseManagement
{
    public partial class ManageMarkedForDeleteObjects : Form
    {
        public TList<Workspace> ServerWorkspacesUserHasWithAdminPermission;

        #region Constructors (1)
        private bool Server = false;
        private string Provider { get { return Server ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider; } }
        public ManageMarkedForDeleteObjects(bool server, TList<Workspace> serverAdminWorkspaces)
        {
            try
            {
                Server = server;
                ServerWorkspacesUserHasWithAdminPermission = serverAdminWorkspaces;
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog(ex.ToString());
            }
        }

        #endregion Constructors

        #region Methods (6)
        // Private Methods (6) 

        private void btnDelete_Click(object sender, EventArgs e)
        {
            CacheManager cacheManager = CacheFactory.GetCacheManager();
            ObjectHelper ohelper = new ObjectHelper(Server);
            List<MetaObjectKey> objectsToRemove = new List<MetaObjectKey>();
            List<MetaBase> objectsNotToRemove = new List<MetaBase>();
            MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter fileAdapter = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();
            foreach (KeyValuePair<MetaObjectKey, MetaBase> objectEntry in objectFinderControl1.SelectedObjects)
            {
                List<MetaBase> objectDiagramsToUpdate = new List<MetaBase>();
                objectDiagramsToUpdate.Add(objectEntry.Value);
                //Delete object off all diagrams on the server
                List<MetaBase> objectsDeleted = new List<MetaBase>();
                List<GraphFile> filesToDeleteFrom = new List<GraphFile>();
                if (Server)
                {
                    //do the instance check here
                    MetaBase mb = objectEntry.Value;
                    //Get Files
                    bool occurence = false;
                    TList<GraphFile> allObjectFiles = fileAdapter.GetFilesByObjectId(mb.pkid, mb.MachineName, true);//DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.GraphFileProvider.GetByMetaObjectIDMachineIDFromGraphFileObject(mb.pkid, mb.MachineName);
                    string affectedFiles = "";
                    foreach (GraphFile oF in allObjectFiles)
                    {
                        if (oF.IsActive)
                        {
                            occurence = true;
                            if (!(filesToDeleteFrom.Contains(oF)))
                            {
                                affectedFiles += "(Workspace " + oF.WorkspaceName + " ) - " + Core.strings.GetFileNameOnly(oF.Name) + Environment.NewLine;
                                filesToDeleteFrom.Add(oF);
                            }
                        }
                    }
                    if (occurence)
                    {
                        if (MessageBox.Show(this, "All occurences of this object will be permanently deleted from the following active diagrams." + Environment.NewLine + affectedFiles, "Would you like to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        {
                            ohelper.DeleteObject(objectEntry.Key.pkid, objectEntry.Key.Machine);
                            objectsToRemove.Add(objectEntry.Key);
                            if (!objectsDeleted.Contains(mb))
                                objectsDeleted.Add(mb);
                        }
                    }
                    else
                    {
                        ohelper.DeleteObject(objectEntry.Key.pkid, objectEntry.Key.Machine);
                        objectsToRemove.Add(objectEntry.Key);
                        if (!objectsDeleted.Contains(mb))
                            objectsDeleted.Add(mb);
                    }
                }
                //delete the object
                else
                {
                    MetaObject obj = DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetBypkidMachine(objectEntry.Value.pkid, objectEntry.Value.MachineName);
                    //null not in contact with server, == in control of that object
                    if (obj.VCMachineID == null)
                    {
                        ohelper.DeleteObject(objectEntry.Key.pkid, objectEntry.Key.Machine);
                        objectsToRemove.Add(objectEntry.Key);
                    }
                    else
                    {
                        //Add to list that displayes box after action
                        objectsNotToRemove.Add(objectEntry.Value);
                    }
                }

                if (objectsDeleted.Count > 0 && Server)
                {
                    MetaBuilder.UIControls.GraphingUI.Tools.DeleteObjectsFromFiles.DeleteObjects(objectsDeleted, filesToDeleteFrom, Core.Variables.Instance.ServerProvider);
                }

                // update the cache too
                string key = objectEntry.Key.pkid.ToString() + "|" + objectEntry.Key.Machine;
                if (cacheManager.Contains(key))
                {
                    cacheManager.Remove(key);
                }
            }

            for (int i = 0; i < objectsToRemove.Count; i++)
            {
                objectFinderControl1.RemoveRow(objectsToRemove[i]);
            }

            if (objectsNotToRemove.Count > 0)
            {

                string msg = "These objects : ";
                int deletedObjects = 0;
                int objectCount = 0;
                foreach (MetaBase k in objectsNotToRemove)
                {
                    objectCount += 1;
                    try
                    {
                        //check if can connect to server
                        //check if object is null
                        if (DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.MetaObjectProvider.GetBypkidMachine(k.pkid, k.MachineName) == null)
                        {
                            ohelper.DeleteObject(k.pkid, k.MachineName);
                            //msg += Environment.NewLine + k.ToString();
                            deletedObjects += 1;
                        }
                        else
                        {
                            msg += Environment.NewLine + k.ToString();
                        }
                    }
                    catch
                    {
                        msg += Environment.NewLine + k.ToString() + " - Cannot connect to server";
                    }
                }
                if (deletedObjects != objectCount) // could not remove all objects
                {
                    msg += Environment.NewLine + "Objects that have multiple occurences on the server can only be deleted on the server.";//Synchronization needs to be performed before the objects can be deleted
                    MessageBox.Show(this, msg, "Cannot delete server objects", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        //BusinessFacade.Storage.RepositoryTemp.PermissionService serv = new MetaBuilder.BusinessFacade.Storage.RepositoryTemp.PermissionService();
        private void btnRestoreSelectedObjects_Click(object sender, EventArgs e)
        {
            CacheManager cacheManager = CacheFactory.GetCacheManager();
            //ObjectHelper ohelper = new ObjectHelper(Server);
            List<MetaObjectKey> objectsToRemove = new List<MetaObjectKey>();
            bool cantConnectToServer = false;
            List<MetaBase> cannotRestoreObjects = new List<MetaBase>();
            foreach (KeyValuePair<MetaObjectKey, MetaBase> objectEntry in objectFinderControl1.SelectedObjects)
            {
                try
                {
                    MetaObject mo = DataRepository.Connections[Provider].Provider.MetaObjectProvider.Get(objectEntry.Key);
                    if (Server) //Server management
                    {
                        mo.VCStatusID = (int)VCStatusList.CheckedIn;
                    }
                    else
                    {
                        if (mo.VCMachineID != null && mo.VCMachineID.Length > 0 && mo.VCMachineID == Core.strings.GetVCIdentifier()) //Client managing server Object (ie : Checked out)
                        {
                            //if (serv.HasAtLeastThisPermission(serv.getWorkspaceKey(mo.WorkspaceName, (int)mo.WorkspaceTypeId), PermissionList.Admin))
                            try
                            {
                                if (cantConnectToServer)
                                    continue;

                                //Get Server State (nettiers failes to get correct object)
                                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("Meta_GetStatusForMetaObject", new System.Data.SqlClient.SqlConnection(Core.Variables.Instance.ServerConnectionString));
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@machine", System.Data.SqlDbType.VarChar, 100));
                                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VCStatusID", System.Data.SqlDbType.Int));
                                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@pkid", System.Data.SqlDbType.Int));
                                cmd.Parameters["@VCStatusID"].Value = 0;
                                cmd.Parameters["@VCStatusID"].Direction = System.Data.ParameterDirection.InputOutput;
                                cmd.Parameters["@pkid"].Value = mo.pkid;
                                cmd.Parameters["@machine"].Value = mo.Machine;
                                //cmd.CommandText = "";
                                if (cmd.Connection.State != System.Data.ConnectionState.Open)
                                    cmd.Connection.Open();
                                cmd.ExecuteNonQuery();
                                int VCStatusID = int.Parse(cmd.Parameters["@VCStatusID"].Value.ToString());

                                cmd.CommandType = System.Data.CommandType.Text;
                                cmd.Parameters.Clear();
                                cmd.CommandText = "SELECT VCMachineID FROM MetaObject where pkid = " + mo.pkid + " AND Machine = '" + mo.Machine + "'";
                                string vcMachine = cmd.ExecuteScalar().ToString();

                                cmd.Connection.Close();
                                //check if checked out to you on server or is null
                                MetaObject serverMo = DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.MetaObjectProvider.GetBypkidMachine(mo.pkid, mo.Machine);
                                if (serverMo == null || (VCStatusID == 2 && vcMachine == Core.strings.GetVCIdentifier()))
                                {
                                    mo.VCStatusID = (int)VCStatusList.CheckedOut;
                                    DataRepository.Connections[Provider].Provider.MetaObjectProvider.Save(mo);
                                    objectsToRemove.Add(objectEntry.Key);

                                    // update the cache too
                                    string serverKey = objectEntry.Key.pkid.ToString() + "|" + objectEntry.Key.Machine;
                                    if (cacheManager.Contains(serverKey))
                                    {
                                        MetaBase mbase = (MetaBase)cacheManager.GetData(serverKey);
                                        mbase.State = (VCStatusList)mo.VCStatusID;
                                    }
                                }
                                else
                                {
                                    cannotRestoreObjects.Add(objectEntry.Value);
                                }
                            }
                            catch
                            {
                                cantConnectToServer = true;
                            }
                            continue;
                        }
                        else
                        {
                            mo.VCStatusID = (int)VCStatusList.None; //not related to server
                        }
                    }

                    DataRepository.Connections[Provider].Provider.MetaObjectProvider.Save(mo);
                    objectsToRemove.Add(objectEntry.Key);

                    // update the cache too
                    string key = objectEntry.Key.pkid.ToString() + "|" + objectEntry.Key.Machine;
                    if (cacheManager.Contains(key))
                    {
                        MetaBase mbase = (MetaBase)cacheManager.GetData(key);
                        mbase.State = (VCStatusList)mo.VCStatusID;
                    }
                }
                catch (Exception ex)
                {
                    Core.Log.WriteLog(ex.ToString());
                }
            }
            for (int i = 0; i < objectsToRemove.Count; i++)
            {
                objectFinderControl1.RemoveRow(objectsToRemove[i]);
            }
            if (cantConnectToServer)
                MessageBox.Show(this, "Server objects were unable to be restored.", "Unable to connect to server", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (cannotRestoreObjects.Count > 0)
            {
                string msg = "These objects : ";
                foreach (MetaBase k in cannotRestoreObjects)
                {
                    msg += Environment.NewLine + k.ToString();
                }
                msg += Environment.NewLine + " have occurences on other diagrams and can only be restored on the server";
                MessageBox.Show(this, msg, "Cannot restore server objects", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ManageMarkedForDeleteObjects_FormClosing(object sender, FormClosingEventArgs e)
        {
            objectFinderControl1.CancelThread();
            Loader.FlushDataViews();
        }

        private void ManageMarkedForDeleteObjects_Load(object sender, EventArgs e)
        {
            if (Server)
            {
                objectFinderControl1.ExcludeVCItems = false;
                objectFinderControl1.ServerMFD = true;
            }
            else
                objectFinderControl1.ExcludeVCItems = true;
            objectFinderControl1.DoInitialisation();
            objectFinderControl1.ViewInContext += new ViewInContextEventHandler(objectFinderControl1_ViewInContext);
            objectFinderControl1.OpenDiagram += new EventHandler(objectFinderControl1_OpenDiagram);
            this.FormClosing += new FormClosingEventHandler(ManageMarkedForDeleteObjects_FormClosing);
            Loader.FlushDataViews();
        }

        private void objectFinderControl1_OpenDiagram(object sender, EventArgs e)
        {
            MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter fileAdapter = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();
            GraphFileKey key = sender as GraphFileKey;
            GraphFile file = fileAdapter.GetQuickFileDetails(key.pkid, key.Machine, (Provider == Core.Variables.Instance.ServerProvider));// DataRepository.Connections[Provider].Provider.GraphFileProvider.Get(key);
            DockingForm.DockForm.OpenGraphFileFromDatabase(file, false, false);
        }

        private void objectFinderControl1_ViewInContext(MetaBase mbase)
        {
            LiteGraphViewContainer contexter = new LiteGraphViewContainer();
            contexter.Setup(mbase);
            contexter.ShowDialog(this);
        }

        #endregion Methods

    }
}