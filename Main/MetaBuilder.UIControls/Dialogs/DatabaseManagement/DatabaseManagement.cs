using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MetaBuilder.BusinessFacade.Storage;
using MetaBuilder.Core;
using d = MetaBuilder.DataAccessLayer;
using b = MetaBuilder.BusinessLogic;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using System.Collections.Generic;

namespace MetaBuilder.UIControls.Dialogs.DatabaseManagement
{
    public partial class DatabaseManagement : Form
    {

        #region Constructors (1)

        public DatabaseManagement()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods (16)

        // Private Methods (16) 

        private bool Server = false;

        private bool BackupDatabase()
        {
            bool result = false;
            SaveFileDialog sfDialog = new SaveFileDialog();
            sfDialog.Filter = "Backup Files (*.bak)|*.bak";
            sfDialog.InitialDirectory = Application.StartupPath;
            DialogResult res = sfDialog.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                StringBuilder queryBuilder = new StringBuilder();
                DialogResult resExplore;
                SqlConnection conn = new SqlConnection(Variables.Instance.ConnectionString);

                queryBuilder.Append("DECLARE @Date VARCHAR(300), @Dir VARCHAR(4000)" + Environment.NewLine);
                queryBuilder.Append("SET @Date = CONVERT(VARCHAR, GETDATE(), 112)" + Environment.NewLine);
                queryBuilder.Append("SET @Dir = '" + sfDialog.FileName + "'" + Environment.NewLine);
                queryBuilder.Append("EXEC sp_addumpdevice 'disk', 'temp_device', @Dir" + Environment.NewLine);
                queryBuilder.Append("BACKUP DATABASE " + conn.Database.ToString() + " TO temp_device WITH FORMAT" + Environment.NewLine);
                queryBuilder.Append("EXEC sp_dropdevice 'temp_device'" + Environment.NewLine);
                SqlCommand cmd = new SqlCommand(queryBuilder.ToString(), conn);
                try
                {
                    #region PermissionCheck

                    //string userName = System.Environment.MachineName +@"\Network Service";
                    //string userName = System.Environment.MachineName +@"\ASPNET";
                    string userName = "NETWORK SERVICE";
                    DirectoryInfo dInfo1 = new DirectoryInfo(Core.strings.GetPath(sfDialog.FileName));
                    DirectorySecurity dSecurity1 = dInfo1.GetAccessControl();
                    dSecurity1.AddAccessRule(new FileSystemAccessRule(userName, FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                    dInfo1.SetAccessControl(dSecurity1);

                    #endregion

                    conn.Open();
                    cmd.CommandText = queryBuilder.ToString();
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();

                    resExplore = MessageBox.Show(this, "Database (" + conn.Database + ") Backed Up Successfully. Explore the folder?", "Backup Database", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (resExplore == DialogResult.Yes)
                    {
                        Launcher.LaunchExplorerForFile(sfDialog.FileName);
                    }
                    result = true;
                }
                catch (Exception xx)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("The account that sql server runs under probably does not have" + Environment.NewLine);
                    sb.Append("sufficient permissions to use that particular folder. Using " + Environment.NewLine);
                    sb.Append("Administrative tools, open the Services Manager dialog" + Environment.NewLine);
                    sb.Append("(or type services.msc at the command line), and find the" + Environment.NewLine);
                    sb.Append("service 'SQL Server (SQLEXPRESS)' or something similar. " + Environment.NewLine);
                    sb.Append("If you click on  properties and then the logon tab, it " + Environment.NewLine);
                    sb.Append("will list the account that SQL Server runs under - usually" + Environment.NewLine);
                    sb.Append("named 'NT Network Service'." + Environment.NewLine);
                    sb.Append(Environment.NewLine);
                    sb.Append("Now you can simply change the SQL Server logon to 'local system'." + Environment.NewLine);
                    sb.Append(Environment.NewLine);
                    sb.Append("You might also have to tick the 'allow service to interact with" + Environment.NewLine);
                    sb.Append("desktop' button. This should allow the backup/restore procedure to" + Environment.NewLine);
                    sb.Append("access the files because it is running as a normal application." + Environment.NewLine);
                    sb.Append(Environment.NewLine);
                    sb.Append("If security is an issue for you, maybe you should investigate creating" + Environment.NewLine);
                    sb.Append("a special account for sql server to run under.");
                    MessageBox.Show(this, sb.ToString(), "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    result = false;
                }
                finally
                {
                    queryBuilder = null;
                    sfDialog = null;
                    cmd.Connection.Close();
                }
            }
            return result;
        }

        private void btnBackupDatabase_Click(object sender, EventArgs e)
        {
            BackupDatabase();
        }

        private void btnClearDatabase_Click(object sender, EventArgs e)
        {
            ClearDatabase(Server);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEmptySandbox_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("The following will be deleted:" + Environment.NewLine);
            sb.Append("a) Diagrams residing in the Sandbox" + Environment.NewLine);
            sb.Append("b) Objects residing in the Sandbox" + Environment.NewLine);
            sb.Append("c) Associations where the parent or child object resides within the Sandbox" + Environment.NewLine);
            sb.Append("d) Artefacts in the sandbox" + Environment.NewLine + Environment.NewLine);
            sb.Append("Do you wish to continue?" + Environment.NewLine);
            DialogResult result = MessageBox.Show(this, sb.ToString(), "Confirm Action", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                //AdminTasksManager atm = new AdminTasksManager(true);
                //atm.ClearSandbox();

                try
                {
                    List<string> cmdList = new List<string>();
                    b.TList<b.GraphFile> sandboxFiles = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileProvider.GetByWorkspaceNameWorkspaceTypeId("Sandbox", 1);
                    b.TList<b.MetaObject> sandboxObjects = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetByWorkspaceNameWorkspaceTypeId("Sandbox", 1);

                    cmdList.AddRange(createDeleteCommands(sandboxObjects));

                    foreach (b.GraphFile file in sandboxFiles)
                    {
                        cmdList.Add("DELETE FROM GraphFileAssociation WHERE GraphFileID = " + file.pkid);
                        cmdList.Add("DELETE FROM GraphFileObject WHERE GraphFileID = " + file.pkid);
                        cmdList.Add("DELETE FROM GraphFile WHERE pkid = " + file.pkid);
                    }

                    Log.WriteLog("Clear sandbox begin (" + cmdList.Count + " items)");
                    int errcount = 0;
                    foreach (string q in cmdList)
                    {
                        SqlCommand cmd = new SqlCommand(q, new SqlConnection(Variables.Instance.ConnectionString));
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection.Open();
                        try
                        {

                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception CMDex)
                        {
                            errcount += 1;
                            Log.WriteLog(Environment.NewLine + q + Environment.NewLine + CMDex.ToString(), "Clear sandbox command fail", System.Diagnostics.TraceEventType.Error);
                        }
                        cmd.Connection.Close();
                    }
                    if (errcount == 0)
                        MessageBox.Show(this, "Sandbox cleared successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                    {
                        string errString = errcount == 1 ? "(" + errcount.ToString() + ") errors have" : "(" + errcount.ToString() + ") error has";
                        MessageBox.Show(this, "Sandbox cleared successfully but " + errString + " occurred and have been logged.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteLog(ex.ToString(), "Clear sandbox fail", System.Diagnostics.TraceEventType.Error);
                    MessageBox.Show(this, "Sandbox data was unable to be collected successfully or a connection could not be established to your specified local SQL Server instance, no action was taken." + Environment.NewLine + " This error has been logged.", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private string GetObjectKey(b.MetaObject obj)
        {
            return obj.pkid + ":" + obj.Machine;
        }

        private void btnManageAssociationsAddToRecycler_Click(object sender, EventArgs e)
        {
            AddAssociationsToRecycler addToRecycler = new AddAssociationsToRecycler(Server);
            addToRecycler.ShowDialog(this);
        }

        private void btnManageDiagrams_Click(object sender, EventArgs e)
        {
            DiagramManager dgmManager = new DiagramManager(Server, ServerWorkspacesUserHasWithAdminPermission);
            dgmManager.ShowDialog(this);
            if (dgmManager.Opened)
                Close();
        }

        private void btnManageDuplicates_Click(object sender, EventArgs e)
        {
            try
            {
                FindDuplicates fdupes = new FindDuplicates(Server, ServerWorkspacesUserHasWithAdminPermission);
                fdupes.ShowDialog(this);
                if (fdupes.openAffected)
                    this.Close();
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString());
            }
        }

        private void btnManageMarkedForDeleteAssociaitons_Click(object sender, EventArgs e)
        {
            ManageMarkedForDeleteAssociations manageRecyclerAssociations = new ManageMarkedForDeleteAssociations(Server);
            manageRecyclerAssociations.ShowDialog(this);
        }

        private void btnManageObjectsAddToRecycler_Click(object sender, EventArgs e)
        {
            AddObjectsToRecycler addToRecycler = new AddObjectsToRecycler(Server, ServerWorkspacesUserHasWithAdminPermission);
            addToRecycler.ShowDialog(this);
        }

        private void btnManageObjectsInRecycler_Click(object sender, EventArgs e)
        {
            try
            {
                ManageMarkedForDeleteObjects manageRecyclerObjects = new ManageMarkedForDeleteObjects(Server, ServerWorkspacesUserHasWithAdminPermission);
                manageRecyclerObjects.ShowDialog(this);
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString());
            }
        }

        private void btnManageWorkspaces_Click(object sender, EventArgs e)
        {

        }

        private void btnScript_Click(object sender, EventArgs e)
        {
            StringBuilder msg = new StringBuilder();
            msg.Append("This function should only be used in conjunction with" + Environment.NewLine);
            msg.Append("OFFICIALLY RELEASED MetaBuilder database scripts. " + Environment.NewLine + Environment.NewLine);
            msg.Append("We do not provide technical support for errors that occur due to any other type of script being run" + Environment.NewLine);
            msg.Append(Environment.NewLine);
            msg.Append("By clicking OK, you agree to these terms.");
            DialogResult resWarning = MessageBox.Show(this, msg.ToString(), "WARNING", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (resWarning == DialogResult.OK)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "SQL Script Files (*.sql)|*.sql";
                ofd.InitialDirectory = Application.StartupPath;
                ofd.Multiselect = false;
                DialogResult res = ofd.ShowDialog(this);
                if (res == DialogResult.OK)
                {
                    FileStream fstream = File.OpenRead(ofd.FileName);
                    StreamReader sreader = new StreamReader(ofd.FileName);
                    StringBuilder sb = new StringBuilder();
                    while (sreader.Peek() > -1)
                    {
                        sb.Append(sreader.ReadLine()).Append(Environment.NewLine);
                    }
                    sreader.Close();
                    SqlCommand cmd = new SqlCommand(sb.ToString(), new SqlConnection(Variables.Instance.ConnectionString));
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection.Open();
                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show(this, "Script Successfully Run", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch
                    {
                        MessageBox.Show(this, "Error running script", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    cmd.Connection.Close();
                }
            }
        }

        private void ClearDatabase(bool server)
        {
            DialogResult res = DialogResult.OK;
            if (!server)
            {
                res = MessageBox.Show(this, "Do you want to create a backup before clearing the database? (Strongly Recommended)", "Backup Database", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (res == DialogResult.Yes)
                {
                    if (!BackupDatabase())
                        return;
                }
            }

            if (res != DialogResult.Cancel)
            {
                MetaBuilder.UIControls.GraphingUI.DockingForm.DockForm.clearing = true;

                Core.Variables.Instance.WorkspaceHashtable.Clear();
                Core.Variables.Instance.WorkspaceHashtable.Add("Sandbox#1", null);

                Core.Variables.Instance.CurrentWorkspaceName = "Sandbox";
                Core.Variables.Instance.CurrentWorkspaceTypeId = 1;
                if (Variables.Instance.DefaultWorkspace.Length > 0)
                {
                    Core.Variables.Instance.DefaultWorkspace = Core.Variables.Instance.CurrentWorkspaceName;
                    Core.Variables.Instance.DefaultWorkspaceID = Core.Variables.Instance.CurrentWorkspaceTypeId;
                }

                MessageBox.Show(this, "This operation might take a few minutes to complete." + Environment.NewLine + Environment.NewLine + "Do not capture any data before you receive the 'DataBase Cleared Successfully' message." + Environment.NewLine + Environment.NewLine + "Also note that the Sandbox is the active workspace now.", "Clear Database", MessageBoxButtons.OK, MessageBoxIcon.Information);

                MetaBuilder.UIControls.GraphingUI.DockingForm.DockForm.SetWorkspaceName("Sandbox", " [Client Workspace]", false);

                ThreadStart ts = new ThreadStart(ExecuteClearance);
                Thread t = new Thread(ts);
                t.Start();
                //ExecuteClearance();

                return;
            }

        }

        private void ExecuteClearance()
        {
            bool e = false;
            ////CREATE new 1
            //SqlCommand addProcCmd = new SqlCommand();
            //addProcCmd.Connection.ConnectionString = Core.Variables.Instance.ConnectionString;
            //addProcCmd.CommandType = CommandType.Text;
            //addProcCmd.CommandText = "String";
            //if (!addProcCmd.Connection.Open())
            //    addProcCmd.Connection.Open();
            //addProcCmd.ExecuteNonQuery();
            //addProcCmd.Connection.Close();
            SqlConnection conn = Server ? new SqlConnection(Variables.Instance.ServerConnectionString) : new SqlConnection(Variables.Instance.ConnectionString);
            SqlCommand cmd = new SqlCommand("______CLEAROBJECTSMFD", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            cmd.Parameters.Add(new SqlParameter("@IncludeVCItems", SqlDbType.Bit));
            cmd.Parameters["@IncludeVCItems"].Value = (Variables.Instance.IsServer) ? false : true; //This is a much better implementation
            //cmd.Parameters["@IncludeVCItems"].Value = false;
            cmd.Connection.Open();
            try
            {
                Log.WriteLog("Clearing database on server : " + Variables.Instance.ConnectionString);
                if (Core.Variables.Instance.IsServer && Variables.Instance.ConnectionString == Variables.Instance.ServerConnectionString)
                {
                    if (MessageBox.Show("Please note that your local server and synchronisation server are the same database." + Environment.NewLine + "Click yes if your are sure you want this database to be cleared." + Environment.NewLine + "If you are unsure about this decision, please make a backup of your databases after you click no before you continue.", "Same database", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        cmd.ExecuteNonQuery();
                        Log.WriteLog("Cleared");
                    }
                    else
                    {
                        Log.WriteLog("Clear cancelled by user");
                        e = true;
                    }
                }
                else
                {
                    cmd.ExecuteNonQuery();
                    Log.WriteLog("Cleared");
                }

                cmd.Connection.Close();
                MessageBox.Show("Database Cleared Successfully", "Clear Database", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MetaBuilder.UIControls.GraphingUI.DockingForm.DockForm.loadWorkspacesButDontSelectOne();
            }
            catch (Exception ex)
            {
                e = true;
                Log.WriteLog(ex.ToString(), "Database clear error", System.Diagnostics.TraceEventType.Error);
                MessageBox.Show("Some items could not be deleted (data may have been lost) - ensure you have no Repository Items that need checking in." + Environment.NewLine + Environment.NewLine + ex.ToString(), "Clear Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MetaBuilder.UIControls.GraphingUI.DockingForm.DockForm.clearing = false;
                return;
            }

            if (!e)
            {
                int errc = 0;
                Log.WriteLog("Start clearance attempt of all graphfiles");
                b.TList<b.GraphFile> files = d.DataRepository.Connections[(Server ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider)].Provider.GraphFileProvider.GetAll();
                foreach (b.GraphFile f in files)
                {
                    try
                    {
                        if (f.VCStatusID != 2)
                            d.DataRepository.Connections[(Server ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider)].Provider.GraphFileProvider.Delete(f);
                        else
                            Log.WriteLog(f.Name + "(" + f.OriginalFileUniqueID.ToString() + ") is still Checked-Out");
                    }
                    catch (Exception ex)
                    {
                        errc += 1;
                        //connceted to objects/associations
                        Log.WriteLog(f.Name + "(" + f.OriginalFileUniqueID.ToString() + ")" + Environment.NewLine + f.ToString() + Environment.NewLine + ex.ToString());
                    }
                }
                Log.WriteLog("GraphFile clearance completed with " + errc.ToString() + " errors");
            }
            else
            {
                Log.WriteLog("GraphFile clearance skipped due to clearance errors or by user control");
            }
            MetaBuilder.UIControls.GraphingUI.DockingForm.DockForm.clearing = false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                comboBoxServer.SelectedIndex = 0;
                //toolStripMenuItemActOnServer.Visible = canAdministrateServer();
                CacheManager cacheManager = CacheFactory.GetCacheManager();
                if (cacheManager != null)
                    cacheManager.Flush();
                enablenation();
                label1.Visible = comboBoxServer.Visible = btnDeleteRundundantServerObjects.Visible = Core.Variables.Instance.IsServer;
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog(ex.ToString());
                Close();
            }
        }

        #endregion Methods

        //9 October 2013
        b.TList<b.Workspace> ServerWorkspacesUserHasWithAdminPermission;
        bool allServerWorkspacesAdmin;
        BusinessFacade.Storage.RepositoryTemp.PermissionService perm;
        private bool canAdministrateServer()
        {
            //must be server version
            if (!Core.Variables.Instance.IsServer)
            {
                Log.WriteLog("canAdministrateServer::IsServer is false");
                return false;
            }

            //must be able to connect to server connectionstring
            if (Variables.Instance.ServerConnectionString.Length == 0)
                return false;
            else
            {
                if (!Core.Networking.Pinger.Ping(Variables.Instance.ServerConnectionString))
                {
                    MessageBox.Show(this, "Unable to connect to your current synchronisation server.", "Ping", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                SqlConnection con = new SqlConnection(Variables.Instance.ServerConnectionString);
                try
                {
                    con.Open();
                    con.Close();
                }
                catch
                {
                    Log.WriteLog("canAdministrateServer::Cannot connect to server");
                    return false;
                }
            }

            allServerWorkspacesAdmin = true;
            ServerWorkspacesUserHasWithAdminPermission = new MetaBuilder.BusinessLogic.TList<MetaBuilder.BusinessLogic.Workspace>();
            perm = new MetaBuilder.BusinessFacade.Storage.RepositoryTemp.PermissionService();
            MetaBuilder.BusinessLogic.TList<MetaBuilder.BusinessLogic.Workspace> serverWorkspaces = d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.WorkspaceProvider.GetByWorkspaceTypeId(3);
            if (serverWorkspaces.Count == 0)
                return true;

            foreach (b.Workspace w in serverWorkspaces)
            {
                if (perm.GetServerPermission(w.Name, w.WorkspaceTypeId) == MetaBuilder.BusinessLogic.PermissionList.Delete)
                {
                    ServerWorkspacesUserHasWithAdminPermission.Add(w);
                }
                else
                {
                    allServerWorkspacesAdmin = false;
                }
            }

            //return true;
            return ServerWorkspacesUserHasWithAdminPermission.Count > 0 || perm.IsSystemAdmin();
        }

        private void btnWorkspaceTransfer_Click(object sender, EventArgs e)
        {
            Meta.Loader.FlushDataViews();
            GraphingUI.WorkspaceManager manager = new MetaBuilder.UIControls.GraphingUI.WorkspaceManager(Server);
            manager.ShowDialog(this);
            manager.BringToFront();
        }

        private void enablenation()
        {
            if (Server)
            {
                btnBackupDatabase.Text = "Backup Database";
                btnWorkspaceTransfer.Enabled = allServerWorkspacesAdmin;
                if (perm != null)
                {
                    btnManageWorkspaces.Enabled = perm.IsSystemAdmin() || d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.WorkspaceProvider.GetByWorkspaceTypeId(3).Count == 0;
                    btnBackupDatabase.Enabled = perm.IsAllSystemAdmin();
                }
                btnManageDuplicates.Enabled = btnManageDiagrams.Enabled = btnManageObjectsAddToRecycler.Enabled = btnManageObjectsInRecycler.Enabled = ServerWorkspacesUserHasWithAdminPermission.Count > 0;
            }
            else
            {
                btnBackupDatabase.Text = "Backup Database";
                btnWorkspaceTransfer.Enabled = true;
                btnBackupDatabase.Enabled = btnManageWorkspaces.Enabled = true;
            }

            btnClearDatabase.Visible = !Server;
            btnDeleteRundundantServerObjects.Visible = !Server;
            btnScript.Visible = !Server;
            btnEmptySandbox.Visible = !Server;

            btnManageAssociationsInRecycler.Enabled = btnManageAssociationsAddToRecycler.Enabled = btnEmptySandbox.Enabled = btnClearDatabase.Enabled = !Server;
            Text = Server ? "Database Management (Server)" : "Database Management (Client)";
        }

        private void btnManageWorkspaces_Click_1(object sender, EventArgs e)
        {
            ChooseWorkspace cw = new ChooseWorkspace(Server);
            cw.ShowDialog(this);
        }

        private void buttonCANCEL_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private List<string> createDeleteCommands(b.TList<b.MetaObject> objects)
        {
            List<string> cmdList = new List<string>();
            foreach (b.MetaObject obj in objects)
            {
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
            }
            return cmdList;
        }

        private void btnDeleteOrphans_Click(object sender, EventArgs e)
        {
            AddObjectsToRecycler deleteOrphans = new AddObjectsToRecycler(Server, ServerWorkspacesUserHasWithAdminPermission, true);
            deleteOrphans.ShowDialog(this);

            //            string provider = Server ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider;
            //            StringBuilder sb = new StringBuilder();
            //            sb.Append("The following will be deleted:" + Environment.NewLine);
            //            sb.Append("a) Objects which are not on active diagrams" + Environment.NewLine);
            //            sb.Append("b) Associations where the parent or child object is not on active diagrams" + Environment.NewLine);
            //            sb.Append("c) Artefacts which are not on active diagrams or as a result of the above 2 operations" + Environment.NewLine + Environment.NewLine);
            //            sb.Append("Do you wish to continue?" + Environment.NewLine);
            //            DialogResult result = MessageBox.Show(this,sb.ToString(), "Confirm (" + provider + ") Action", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            //            if (result != DialogResult.Yes)
            //                return;

            //            b.TList<BusinessLogic.MetaObject> OrphanedObjects = new b.TList<b.MetaObject>();
            //            foreach (b.MetaObject mobj in d.DataRepository.Connections[provider].Provider.MetaObjectProvider.GetAll())
            //            {
            //                //cannot delte marked for delete object
            //                if (mobj.VCStatusID == (int)b.VCStatusList.MarkedForDelete)
            //                    continue;

            //                bool add = true;
            //                foreach (b.GraphFile file in d.DataRepository.Connections[provider].Provider.GraphFileProvider.GetByMetaObjectIDMachineIDFromGraphFileObject(mobj.pkid, mobj.Machine))
            //                {
            //                    if (!Server)
            //                    {
            //                        if (mobj.WorkspaceTypeId == 3)
            //                        {
            //                            //skip checkedout objects
            //                            if (mobj.VCStatusID == (int)b.VCStatusList.CheckedOut)
            //                                continue;
            //                        }
            //                    }
            //                    else
            //                    {
            //                        //Cannot delete checkedout server object
            //                        if (mobj.VCStatusID == (int)b.VCStatusList.CheckedOut)
            //                            continue;
            //                    }

            //                    //if file is active do not add
            //                    if (file.IsActive == true)
            //                        add = false;
            //                }
            //                if (add)
            //                    OrphanedObjects.Add(mobj);
            //            }
            //            List<string> cmdList = new List<string>();
            //            cmdList.AddRange(createDeleteCommands(OrphanedObjects));

            //            Log.WriteLog("Delete Orphans begin (" + cmdList.Count + " items)");
            //            int errcount = 0;
            //            foreach (string q in cmdList)
            //            {
            //#if DEBUG
            //                continue;
            //#endif
            //                SqlCommand cmd = new SqlCommand(q, new SqlConnection(Server ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString));
            //                cmd.CommandType = CommandType.Text;
            //                try
            //                {
            //                    cmd.Connection.Open();
            //                    cmd.ExecuteNonQuery();
            //                }
            //                catch (InvalidOperationException sqlEx)
            //                {
            //                    MessageBox.Show(this,"Unable to connect to the specified SQL Server instance", "Aborted", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //                    return;
            //                }
            //                catch (Exception CMDex)
            //                {
            //                    errcount += 1;
            //                    Log.WriteLog(Environment.NewLine + q + Environment.NewLine + CMDex.ToString(), "Delete Orphan command fail", System.Diagnostics.TraceEventType.Error);
            //                }
            //                cmd.Connection.Close();
            //            }
            //            if (errcount == 0)
            //                MessageBox.Show(this,"Delete Orphaned Objects successfull", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            else
            //            {
            //                string errString = errcount == 1 ? "(" + errcount.ToString() + ") errors have" : "(" + errcount.ToString() + ") error has";
            //                MessageBox.Show(this,"Delete Orphand Objects successfull but " + errString + " occurred and have been logged.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //            }

        }
        private void btnDeleteRundundantServerObjects_Click(object sender, EventArgs e)
        {
            //cannot run on server, should be invisible
            if (Server || !Core.Networking.Pinger.Ping(Variables.Instance.ServerConnectionString))
                return;

            //StringBuilder sb = new StringBuilder();
            //sb.Append("The following will be deleted:" + Environment.NewLine);
            //sb.Append("a) Objects and Artefacts which do not exist on the server" + Environment.NewLine);
            //sb.Append("b) Associations where the parent or child object does not exist on the server" + Environment.NewLine);
            ////sb.Append("c) Artefacts which are not on the server or as a result of the above 2 operations" + Environment.NewLine + Environment.NewLine);
            //sb.Append("Do you wish to continue?" + Environment.NewLine);
            //DialogResult result = MessageBox.Show(this,sb.ToString(), "Confirm Action", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            //if (result != DialogResult.Yes)
            //    return;

            SqlConnection serverCon = new SqlConnection(Variables.Instance.ServerConnectionString);
            try
            {
                serverCon.Open();
                serverCon.Close();
            }
            catch
            {
                MessageBox.Show(this, "Unable to connect to the metabuilder server database", "Aborted", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            AddObjectsToRecycler drso = new AddObjectsToRecycler(true);
            drso.ShowDialog(this);

            //            b.TList<BusinessLogic.MetaObject> RedundantObjects = new b.TList<b.MetaObject>();
            //            foreach (b.MetaObject mobj in d.DataRepository.MetaObjectProvider.GetAll())
            //            {
            //                if (mobj.WorkspaceTypeId != 3)
            //                    continue;
            //                if (mobj.VCStatusID == (int)b.VCStatusList.MarkedForDelete)
            //                    continue;
            //                if (mobj.VCStatusID == (int)b.VCStatusList.None)
            //                    continue;

            //                //check if exists on server
            //                b.MetaObject serverObject = null;
            //                try
            //                {
            //                    serverObject = d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.MetaObjectProvider.GetBypkidMachine(mobj.pkid, mobj.Machine);
            //                }
            //                catch
            //                {
            //                }
            //                //object does not exist
            //                if (serverObject == null)
            //                    RedundantObjects.Add(mobj);
            //            }
            //            List<string> cmdList = new List<string>();
            //            cmdList.AddRange(createDeleteCommands(RedundantObjects));

            //            Log.WriteLog("Delete Redundant begin (" + cmdList.Count + " items)");
            //            int errcount = 0;
            //            foreach (string q in cmdList)
            //            {
            //#if DEBUG
            //                continue;
            //#endif
            //                SqlCommand cmd = new SqlCommand(q, new SqlConnection(Variables.Instance.ConnectionString));
            //                cmd.CommandType = CommandType.Text;
            //                try
            //                {
            //                    cmd.Connection.Open();
            //                    cmd.ExecuteNonQuery();
            //                }
            //                catch (InvalidOperationException sqlEx)
            //                {
            //                    MessageBox.Show(this,"Unable to connect to the specified SQL Server instance", "Aborted", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //                    return;
            //                }
            //                catch (Exception CMDex)
            //                {
            //                    errcount += 1;
            //                    Log.WriteLog(Environment.NewLine + q + Environment.NewLine + CMDex.ToString(), "Delete Redundant command fail", System.Diagnostics.TraceEventType.Error);
            //                }
            //                cmd.Connection.Close();
            //            }
            //            if (errcount == 0)
            //                MessageBox.Show(this,"Delete Redundant Server Objects successfull", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            else
            //            {
            //                string errString = errcount == 1 ? "(" + errcount.ToString() + ") errors have" : "(" + errcount.ToString() + ") error has";
            //                MessageBox.Show(this,"Delete Redundant Server Objects successfull but " + errString + " occurred and have been logged.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //            }
        }

        private void comboBoxServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxServer.SelectedItem.ToString())
            {
                case "Client":
                    Server = false;
                    break;
                case "Server":
                    if (canAdministrateServer())
                    {
                        //actOnServer();
                        Server = true;
                    }
                    else
                    {
                        MessageBox.Show(this, "You must have R-W-D permissions on at least one server workspace to manage it on the server database or be a system administrator.", "Insufficient permission", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        comboBoxServer.SelectedIndex = 0;
                    }
                    break;
            }
            enablenation();
        }
    }
}