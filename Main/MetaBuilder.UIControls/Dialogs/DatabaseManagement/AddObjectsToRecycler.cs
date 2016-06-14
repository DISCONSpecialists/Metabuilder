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
using System.Data.SqlClient;
using System.Data;
namespace MetaBuilder.UIControls.Dialogs.DatabaseManagement
{
    public partial class AddObjectsToRecycler : Form
    {
        public TList<Workspace> ServerWorkspacesUserHasWithAdminPermission;

        #region Constructors (1)
        private bool Server = false;
        private string Provider { get { return Server ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider; } }
        public AddObjectsToRecycler(bool server, TList<Workspace> serverAdminWorkspaces)
        {
            Server = server;
            ServerWorkspacesUserHasWithAdminPermission = serverAdminWorkspaces;
            InitializeComponent();
        }

        private bool O = false;
        private bool RSO = false;
        public AddObjectsToRecycler(bool server, TList<Workspace> serverAdminWorkspaces, bool orphans)
        {
            Server = server;
            ServerWorkspacesUserHasWithAdminPermission = serverAdminWorkspaces;
            O = orphans;
            InitializeComponent();
        }

        public AddObjectsToRecycler(bool rso)
        {
            Server = false;
            ServerWorkspacesUserHasWithAdminPermission = null;
            RSO = rso;
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods (5)


        // Private Methods (5) 

        void AddObjectsToRecycler_FormClosing(object sender, FormClosingEventArgs e)
        {
            objectFinderControl1.CancelThread();
            Loader.FlushDataViews();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (O)
            {
                DeleteOrphans();
                return;
            }
            else if (RSO)
            {
                DeleteRSO();
                return;
            }
            ObjectHelper ohelper = new ObjectHelper(Server);
            CacheManager cacheManager = CacheFactory.GetCacheManager();
            List<MetaObjectKey> objectsToRemove = new List<MetaObjectKey>();
            foreach (KeyValuePair<MetaObjectKey, MetaBase> objectEntry in objectFinderControl1.SelectedObjects)
            {
                try
                {
                    MetaObject mo = DataRepository.Connections[Provider].Provider.MetaObjectProvider.Get(objectEntry.Key);
                    mo.VCStatusID = 8;
                    DataRepository.Connections[Provider].Provider.MetaObjectProvider.Save(mo);
                    objectsToRemove.Add(objectEntry.Key);

                    // update the cache too
                    string key = objectEntry.Key.pkid.ToString() + "|" + objectEntry.Key.Machine;
                    if (cacheManager.Contains(key))
                    {
                        MetaBase mbase = (MetaBase)cacheManager.GetData(key);
                        mbase.State = VCStatusList.MarkedForDelete;
                    }
                }
                catch
                {
                }
            }
            for (int i = 0; i < objectsToRemove.Count; i++)
            {
                try
                {
                    objectFinderControl1.RemoveRow(objectsToRemove[i]);
                }
                catch
                {
                }
            }
        }

        private void ManageMarkedForDeleteObjects_Load(object sender, EventArgs e)
        {
            if (O)
            {
                objectFinderControl1.Orphans = true;
                objectFinderControl1.IncludeStatusCombo = false;

                Text = "Orphaned Object Finder (" + Provider + ")";
                btnDelete.Text = "Delete Selected Objects";
            }
            else if (RSO)
            {
                objectFinderControl1.RSO = true;
                objectFinderControl1.IncludeStatusCombo = false;

                Text = "Redundant Server Object Finder (Client)";
                btnDelete.Text = "Delete Selected Objects";
            }
            else
            {
                objectFinderControl1.ExcludeStatuses.Add(VCStatusList.Obsolete);
                objectFinderControl1.ExcludeStatuses.Add(VCStatusList.MarkedForDelete);
                //objectFinderControl1.ExcludeStatuses.Add(VCStatusList.CheckedOut);
                if (!Server)
                {
                    objectFinderControl1.ExcludeStatuses.Add(VCStatusList.CheckedIn);
                    objectFinderControl1.ExcludeStatuses.Add(VCStatusList.CheckedOutRead);
                }
                //added

                objectFinderControl1.IncludeStatusCombo = true;
            }

            objectFinderControl1.AllowMultipleSelection = true;
            objectFinderControl1.DoInitialisation();
            objectFinderControl1.ViewInContext += new ViewInContextEventHandler(objectFinderControl1_ViewInContext);
            objectFinderControl1.OpenDiagram += new EventHandler(objectFinderControl1_OpenDiagram);
            Loader.FlushDataViews();
            this.FormClosing += new FormClosingEventHandler(AddObjectsToRecycler_FormClosing);
        }

        private void DeleteOrphans()
        {
            if (objectFinderControl1.SelectedObjects.Count == 0)
                return;

            List<string> cmdList = new List<string>();
            foreach (KeyValuePair<MetaObjectKey, MetaBase> objectEntry in objectFinderControl1.SelectedObjects)
            {
                cmdList.AddRange(createDeleteCommand(objectEntry.Value));
            }
            Core.Log.WriteLog("Delete Orphans begin (" + cmdList.Count + " items)");
            int errcount = 0;
            foreach (string q in cmdList)
            {
#if DEBUG
                continue;
#endif
                SqlCommand cmd = new SqlCommand(q, new SqlConnection(Server ? Core.Variables.Instance.ServerConnectionString : Core.Variables.Instance.ConnectionString));
                cmd.CommandType = CommandType.Text;
                try
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (InvalidOperationException sqlEx)
                {
                    MessageBox.Show(this,"Unable to connect to the specified SQL Server instance", "Aborted", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                catch (Exception CMDex)
                {
                    errcount += 1;
                    Core.Log.WriteLog(Environment.NewLine + q + Environment.NewLine + CMDex.ToString(), "Delete Orphan command fail", System.Diagnostics.TraceEventType.Error);
                }
                cmd.Connection.Close();
            }
            if (errcount == 0)
                MessageBox.Show(this,"Selected Orphaned Objects Deleted", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                string errString = errcount == 1 ? "(" + errcount.ToString() + ") errors have" : "(" + errcount.ToString() + ") error has";
                MessageBox.Show(this,"Selected Orphaned Objects Deleted but " + errString + " occurred and have been logged.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            Close();
            //objectFinderControl1.SelectedObjects = null;
        }
        private void DeleteRSO()
        {
            if (objectFinderControl1.SelectedObjects.Count == 0)
                return;

            List<string> cmdList = new List<string>();
            foreach (KeyValuePair<MetaObjectKey, MetaBase> objectEntry in objectFinderControl1.SelectedObjects)
            {
                cmdList.AddRange(createDeleteCommand(objectEntry.Value));
            }

            Core.Log.WriteLog("Delete Redundant begin (" + cmdList.Count + " items)");
            int errcount = 0;
            foreach (string q in cmdList)
            {
#if DEBUG
                continue;
#endif
                SqlCommand cmd = new SqlCommand(q, new SqlConnection(Core.Variables.Instance.ConnectionString));
                cmd.CommandType = CommandType.Text;
                try
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (InvalidOperationException sqlEx)
                {
                    MessageBox.Show(this,"Unable to connect to the specified SQL Server instance", "Aborted", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                catch (Exception CMDex)
                {
                    errcount += 1;
                    Core.Log.WriteLog(Environment.NewLine + q + Environment.NewLine + CMDex.ToString(), "Delete Redundant command fail", System.Diagnostics.TraceEventType.Error);
                }
                cmd.Connection.Close();
            }
            if (errcount == 0)
                MessageBox.Show(this,"Delete Redundant Server Objects successfull", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                string errString = errcount == 1 ? "(" + errcount.ToString() + ") errors have" : "(" + errcount.ToString() + ") error has";
                MessageBox.Show(this,"Delete Redundant Server Objects successfull but " + errString + " occurred and have been logged.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            Close();
        }
        private List<string> createDeleteCommand(MetaBase obj)
        {
            List<string> cmdList = new List<string>();
            //foreach (MetaObject obj in objects)
            //{
            //artifact
            cmdList.Add("DELETE FROM Artifact WHERE ltrim(rtrim(str(ObjectID)))+':'+ObjectMachine = '" + GetObjectKey(obj) + "' OR ltrim(rtrim(str(ChildObjectID)))+':'+ChildObjectMachine = '" + GetObjectKey(obj) + "' OR ltrim(rtrim(str(ArtifactObjectID)))+':'+ArtefactMachine = '" + GetObjectKey(obj) + "'");
            //association
            cmdList.Add("DELETE FROM GraphFileAssociation WHERE ltrim(rtrim(str(ObjectID)))+':'+ObjectMachine = '" + GetObjectKey(obj) + "' OR ltrim(rtrim(str(ChildObjectID)))+':'+ChildObjectMachine = '" + GetObjectKey(obj) + "'");
            cmdList.Add("DELETE FROM ObjectAssociation WHERE ltrim(rtrim(str(ObjectID)))+':'+ObjectMachine = '" + GetObjectKey(obj) + "' OR ltrim(rtrim(str(ChildObjectID)))+':'+ChildObjectMachine = '" + GetObjectKey(obj) + "'");
            //file objects
            cmdList.Add("DELETE FROM GraphFileObject WHERE ltrim(rtrim(str(MetaObjectID)))+':'+MachineID = '" + GetObjectKey(obj) + "'");
            //values
            cmdList.Add("DELETE FROM ObjectFieldValue WHERE ltrim(rtrim(str(ObjectID)))+':'+MachineID = '" + GetObjectKey(obj) + "'");
            //objects
            cmdList.Add("DELETE FROM MetaObject WHERE ltrim(rtrim(str(pkid)))+':'+Machine = '" + GetObjectKey(obj) + "'");
            //}
            return cmdList;
        }
        private string GetObjectKey(MetaBase obj)
        {
            return obj.pkid + ":" + obj.MachineName;
        }

        void objectFinderControl1_OpenDiagram(object sender, EventArgs e)
        {
            GraphFileKey key = sender as GraphFileKey;
            MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter adapter = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();
            GraphFile file = adapter.GetQuickFileDetails(key.pkid, key.Machine, (Provider == Core.Variables.Instance.ServerProvider));
            //GraphFile file = DataRepository.Connections[Provider].Provider.GraphFileProvider.Get(key);
            DockingForm.DockForm.OpenGraphFileFromDatabase(file, false, false);
        }
        void objectFinderControl1_ViewInContext(MetaBase mbase)
        {
            LiteGraphViewContainer contexter = new LiteGraphViewContainer();
            contexter.Setup(mbase);
            contexter.ShowDialog(this);
        }

        #endregion Methods

    }
}