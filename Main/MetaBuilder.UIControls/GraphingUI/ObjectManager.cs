using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using MetaBuilder.BusinessFacade.Storage;
using MetaBuilder.BusinessFacade.Storage.RepositoryTemp;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Meta;
using MetaBuilder.ResourceManagement;
using MetaBuilder.SplashScreen;
using MetaBuilder.UIControls.GraphingUI.RepositoryTree;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using OpenLicense;
using OpenLicense.LicenseFile;
using d = MetaBuilder.DataAccessLayer;
using bf = MetaBuilder.BusinessFacade;
using b = MetaBuilder.BusinessLogic;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using System.IO;

namespace MetaBuilder.UIControls.GraphingUI
{
    [LicenseProvider(typeof(OpenLicenseProvider))]
    public partial class ObjectManager : Form
    {

        #region Fields (3)

        private ImageList imgList;
        private TreeNode lastNode;
        private object lastSender;
        private bool workspaceSync = true;

        #endregion Fields

        #region Constructors (1)

        public ObjectManager()
        {
            License license;
            LicenseManager.IsValid(GetType(), this, out license);
            if (license == null)
            {
                throw new ApplicationException("A suitable license file couldn't be found");
            }
            else if (((OpenLicenseFile)license).FailureReason != String.Empty)
            {
                throw new ApplicationException(((OpenLicenseFile)license).FailureReason);
            }
            InitializeComponent();
            setStatus("Initializing");
        }

        #endregion Constructors

        #region Properties (3)

        private bool Confirmed
        {
            get { return txtConfirm.Text == "CONFIRM"; }
        }

        public TreeNode LastNode
        {
            get { return lastNode; }
            set { lastNode = value; }
        }

        public object LastSender
        {
            get { return lastSender; }
            set { lastSender = value; }
        }

        #endregion Properties

        #region Methods (18)

        // Private Methods (18) 

        private void BindData(object sender, TreeViewEventArgs e)
        {
            syncPanelRelationships.IsLocal = syncPanelFiles.IsLocal = syncPanelObjects.IsLocal = (LastSender == treeLocal);
            if (LastNode is ItemNode)
            {
                syncPanelRelationships.Enabled = syncPanelObjects.Enabled = syncPanelFiles.Enabled = true;
                //syncPanelRelationships.Visible = syncPanelFiles.Visible = syncPanelObjects.Visible = true;
                ListItems(e);
            }
            else
            {
                syncPanelRelationships.Enabled = syncPanelFiles.Enabled = syncPanelObjects.Enabled = false;
                //syncPanelRelationships.Visible = syncPanelFiles.Visible = syncPanelObjects.Visible = false;
            }
            //else
            //{
            //    tabControl1.SelectedIndex = 0;
            //    tabControl1.SelectedTab = tabPageGeneral;
            //}
        }

        private void setStatus(string status)
        {
            if (InvokeRequired)
                BeginInvoke(new Action<string>(setStatus), new object[] { status });
            else
            {
                treeRemote.Enabled = treeLocal.Enabled = status == "Ready";
                labelNavPane.BackColor = status == "Ready" ? System.Drawing.SystemColors.Control : System.Drawing.Color.Red;
                labelNavPane.Text = status;
                Application.DoEvents();
            }
        }

        private void BindTree(int pageindex)
        {
            setStatus("Binding");
            treeLocal.Nodes.Clear();
            treeRemote.Nodes.Clear();
            RepositoryNode repNode = new RepositoryNode();
            if (pageindex == 0)
            {
                repNode.ConnectionString = Variables.Instance.ServerConnectionString;
                repNode.RepositoryType = Repository.RepositoryType.Server;
                treeRemote.Nodes.Add(repNode);
                List<string> wsP = new List<string>();

                if (workspaceSync) // on first load of screen. make first applciation load?
                {
                    TList<Workspace> clientSpaces = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.WorkspaceProvider.GetAll();
                    foreach (Workspace ws in clientSpaces)
                    {
                        if (ws.WorkspaceTypeId == 3)
                        {
                            try
                            {
                                //remove old server workspace from client
                                if (DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.WorkspaceProvider.GetByNameWorkspaceTypeId(ws.Name, ws.WorkspaceTypeId) == null)
                                {
                                    //check if there are files (what about objects(orphans)?)
                                    if (DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileProvider.GetByWorkspaceNameWorkspaceTypeId(ws.Name, ws.WorkspaceTypeId).Count == 0)
                                        DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.WorkspaceProvider.Delete(ws);

                                    if (Core.Variables.Instance.WorkspaceHashtable.ContainsKey(ws.Name + "#" + ws.WorkspaceTypeId))
                                    {
                                        Core.Variables.Instance.WorkspaceHashtable.Remove(ws.Name + "#" + ws.WorkspaceTypeId);
                                    }
                                }
                            }
                            catch (Exception wsEX)
                            {
                                wsP.Add(ws.Name);
                                Log.WriteLog(wsEX.ToString());
                            }
                        }
                    }
                }

                foreach (TreeNode wnode in repNode.Nodes)
                {
                    try
                    {
                        //sync to server workspaces
                        WorkspaceNode wsNode = wnode as WorkspaceNode;
                        Workspace ws = wsNode.Workspace;
                        DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.WorkspaceProvider.Save(ws);
                        if (!Core.Variables.Instance.WorkspaceHashtable.ContainsKey(ws.Name + "#" + ws.WorkspaceTypeId))
                        {
                            Core.Variables.Instance.WorkspaceHashtable.Add(ws.Name + "#" + ws.WorkspaceTypeId, null);
                        }
                    }
                    catch
                    {
                    }
                }

                if (workspaceSync)
                {
                    //note that old client workspaces which were server workspaces are no longer available and contain objects
                    if (wsP.Count > 0)
                    {
                        string wss = "";
                        foreach (string s in wsP)
                        {
                            wss += s + Environment.NewLine;
                        }

                        MessageBox.Show(this, "You have server workspaces on your client database that cannot be removed because there are still objects that exist within them. Here is a list of these workspaces." + Environment.NewLine + Environment.NewLine + wss + Environment.NewLine + "To be assured of a full syncronisation please remove the items attached to these workspaces.", "Workspace Sync Partially Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    workspaceSync = false;
                }
            }
            else
            {
                repNode.ConnectionString = Variables.Instance.ConnectionString;
                repNode.RepositoryType = Repository.RepositoryType.Client;
                treeLocal.Nodes.Add(repNode);
            }
            repNode.Load();
            updateClientWorkspaceHashTable();
            repNode.Expand(); //Expand to make workspaces visible
            setStatus("Ready");
        }

        private void updateClientWorkspaceHashTable()
        {
            TList<Workspace> clientWorkspaces = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.WorkspaceProvider.GetAll();
            foreach (Workspace ws in clientWorkspaces)
            {
                if (!Core.Variables.Instance.WorkspaceHashtable.ContainsKey(ws.Name + "#" + ws.WorkspaceTypeId))
                {
                    Core.Variables.Instance.WorkspaceHashtable.Add(ws.Name + "#" + ws.WorkspaceTypeId, null);
                }
            }
        }

        private void btnClearOrphans_Click(object sender, EventArgs e)
        {
            // server/client
            if (Confirmed)
            {
                AdminTasksManager manager = new AdminTasksManager(syncPanelFiles.IsLocal);
                manager.ClearOrphans();
            }
        }

        private void btnClearSandbox_Click(object sender, EventArgs e)
        {
            // always client specific
            if (Confirmed)
            {
                AdminTasksManager manager = new AdminTasksManager(syncPanelFiles.IsLocal);
                manager.ClearSandbox();
            }
        }

        private void btnPurgeFiles_Click(object sender, EventArgs e)
        {
            // this may be server based
            if (Confirmed)
            {
                if (syncPanelFiles.IsLocal)
                {
                    AdminTasksManager manager = new AdminTasksManager(syncPanelFiles.IsLocal);
                    manager.ClearPreviousVersions();
                }
            }
        }

        private void ListFilesInSelectedWorkspaceNode(TreeViewEventArgs e)
        {
            syncPanelObjects.CheckforDirty();
            syncPanelFiles.CheckforDirty();
            syncPanelRelationships.CheckforDirty();
            GraphFileCollectionNode gfcolnode = LastNode as GraphFileCollectionNode;
            syncPanelFiles.Items = new List<IRepositoryItem>();
            syncPanelRelationships.Items = new List<IRepositoryItem>();
            syncPanelObjects.Items = new List<IRepositoryItem>();
            List<GraphFile> files = new List<GraphFile>();
            gfcolnode.RefreshNodes();
            foreach (TreeNode n in gfcolnode.Nodes)
            {
                GraphFileNode gfnode = n as GraphFileNode;
                if (gfnode.File.IsActive)
                    syncPanelFiles.Items.Add(gfnode.File);
            }
            syncPanelFiles.RefreshItems();
            tabControl1.SelectedTab = tabPageFiles;
        }

        private void ListItems(TreeViewEventArgs e)
        {
            setStatus("Loading Data");
            syncPanelObjects.CheckforDirty();
            syncPanelFiles.CheckforDirty();
            syncPanelRelationships.CheckforDirty();
            syncPanelObjects.Items = new List<IRepositoryItem>();
            syncPanelObjects.Permissions = new List<PermissionList>();
            syncPanelRelationships.Items = new List<IRepositoryItem>();
            syncPanelFiles.Items = new List<IRepositoryItem>();
            //PermissionService permService = new PermissionService();
            Loader.FlushDataViews();
            if (e.Node is ItemNode)
            {
                ItemNode itemNode = e.Node as ItemNode;
                //TODO : Data Load
                if (itemNode.Items.Count == 0 && itemNode.GetType() != typeof(ClassCollectionNode))
                {
                    setStatus("Loading Objects");
                    itemNode.LoadChildren();
                }

                switch (itemNode.TargetPanel)
                {
                    case TargetPanelType.Diagrams:
                        syncPanelFiles.Items = itemNode.Items;
                        tabControl1.SelectedTab = tabPageFiles;
                        syncPanelFiles.Enabled = true;
                        syncPanelFiles.RefreshItems();
                        break;
                    case TargetPanelType.Objects:
                        syncPanelObjects.Items = itemNode.Items;
                        tabControl1.SelectedTab = tabPageObjects;
                        foreach (IWorkspaceItem wsItem in syncPanelObjects.Items)
                        {
                            syncPanelObjects.Permissions.Add(permissionService.GetServerPermission(wsItem.WorkspaceName, wsItem.WorkspaceTypeId));
                        }
                        syncPanelObjects.Enabled = true;
                        syncPanelObjects.RefreshItems();
                        break;
                    case TargetPanelType.Relationships:
                        syncPanelRelationships.Items = itemNode.Items;
                        syncPanelRelationships.Enabled = true;
                        tabControl1.SelectedTab = tabPageRelationships;
                        syncPanelRelationships.RefreshItems();
                        break;
                }
                if (LastNode is GraphFileNode)
                {
                    setStatus("Loading Relationships");
                    ListRelationships(e);
                }
            }
            setStatus("Ready");
        }

        private void ListObjectsInSelectedFileNode(TreeViewEventArgs e)
        {
            syncPanelObjects.CheckforDirty();
            syncPanelFiles.CheckforDirty();
            syncPanelRelationships.CheckforDirty();
            GraphFileNode gfnode = LastNode as GraphFileNode;
            List<MetaBase> mbase = new List<MetaBase>();
            RepositoryNode node = e.Node.Parent.Parent.Parent as RepositoryNode;
            TList<MetaObject> objectsInDiagram = DataRepository.Connections[node.RepositoryType.ToString()].Provider.MetaObjectProvider.GetByGraphFileIDGraphFileMachineFromGraphFileObject(gfnode.File.pkid, gfnode.File.Machine);
            syncPanelObjects.Items = new List<IRepositoryItem>();
            syncPanelRelationships.Items = new List<IRepositoryItem>();
            //string prevConnString = Variables.Instance.ConnectionString;
            //if (node.RepositoryType == Repository.RepositoryType.Server)
            //{
            //set local string to server string for loader
            bool userServer = node.RepositoryType == Repository.RepositoryType.Server ? true : false;
            //}
            //PermissionService permService = new PermissionService();
            foreach (MetaObject mo in objectsInDiagram)
            {
                MetaBase mbo = Loader.GetFromProvider(mo.pkid, mo.Machine, mo.Class, userServer);
                syncPanelObjects.Items.Add(mbo);
                syncPanelObjects.Permissions.Add(permissionService.GetServerPermission(mbo.WorkspaceName, mbo.WorkspaceTypeId));
            }
            //Variables.Instance.ConnectionString = prevConnString;
            syncPanelObjects.RefreshItems();
            tabControl1.SelectedTab = tabPageObjects;
        }

        private void ListRelationships(TreeViewEventArgs e)
        {
            List<MetaBase> mbase = new List<MetaBase>();
            string provider = "";
            if (LastSender == treeLocal)
            {
                provider = Core.Variables.Instance.ClientProvider;
            }
            else
            {
                provider = Core.Variables.Instance.ServerProvider;
            }

            bool useServer = provider == Core.Variables.Instance.ServerProvider ? true : false;

            TList<ObjectAssociation> objectAssociations = new TList<ObjectAssociation>();
            if (LastNode is ClassAssociationNode)
            {
                ClassAssociationNode classAssocNode = LastNode as ClassAssociationNode;
                objectAssociations = DataRepository.Connections[provider].Provider.ObjectAssociationProvider.GetByCAid(classAssocNode.ClassAssociation.CAid);
            }
            if (LastNode is GraphFileNode)
            {
                GraphFileNode fileNode = LastNode as GraphFileNode;
                objectAssociations = DataRepository.Connections[provider].Provider.ObjectAssociationProvider.GetByGraphFileIDGraphFileMachineFromGraphFileAssociation(fileNode.File.pkid, fileNode.File.Machine);
            }

            syncPanelRelationships.Items = new List<IRepositoryItem>();
            foreach (ObjectAssociation association in objectAssociations)
            {
                ClassAssociation classAssociation = DataRepository.Connections[provider].Provider.ClassAssociationProvider.GetByCAid(association.CAid);
                MetaBase mbParent = Loader.GetFromProvider(association.ObjectID, association.ObjectMachine, classAssociation.ParentClass, useServer);
                MetaBase mbChild = Loader.GetFromProvider(association.ChildObjectID, association.ChildObjectMachine, classAssociation.ChildClass, useServer);
                syncPanelRelationships.Items.Add(association);
            }
            syncPanelRelationships.RefreshItems();
        }

        private void ListRelationshipsInSelectedRelationshipNode(TreeViewEventArgs e)
        {
            syncPanelObjects.CheckforDirty();
            syncPanelFiles.CheckforDirty();
            syncPanelRelationships.CheckforDirty();
            ListRelationships(e);
        }

        private void navigationPane1_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetTables();
            RefreshRepository(true);
        }

        private void RefreshRepository(bool indexChanged)
        {
            //PleaseWait.ShowPleaseWaitForm();
            //Application.DoEvents();
            //PleaseWait.SetStatus("Fetching data from repository");
            if (indexChanged)
                tabControl1.SelectedIndex = 0;
            //switch (navigationPane1.SelectedIndex)
            //{
            //    case 0:
            //        tabControl1.SelectedIndex = 0;
            //        break;
            //    case 1:
            //        break;
            //}
            BindTree(navigationPane1.SelectedIndex);
            //PleaseWait.CloseForm();
            buttonRefreshRepository.Enabled = false;
        }

        private PermissionService permissionService = new PermissionService();
        private void ObjectManager_Load(object sender, EventArgs e)
        {
            syncPanelRelationships.Enabled = syncPanelFiles.Enabled = syncPanelObjects.Enabled = false;
            //syncPanelRelationships.Visible = syncPanelFiles.Visible = syncPanelObjects.Visible = false;
            PleaseWait.ShowPleaseWaitForm();
            PleaseWait.SetStatus("Synchronising Workspaces");
            setStatus("Syncing Workspaces");
            try
            {
                try
                {

                    permissionService.SynchroniseServerWorkspaces();
                    try
                    {
                        imgList = new ImageList();
                        imgList.Images.Add(Reader.PadLockImage);
                        treeRemote.ImageList = imgList;
                        treeRemote.ImageIndex = 0;
                        AdminTasksManager atm = new AdminTasksManager(true);
                        atm.MarkInactives();
                        try
                        {
                            BindTree(0);
                            //navigationPane1.SelectNavigationPage(navigationPane1.NavigationPages[0]);

                            syncPanelObjects.AllowTypeFilter = true;
                            syncPanelFiles.AllowTypeFilter = false;
                            syncPanelFiles.ActionsPerformed += new EventHandler(refreshFiles);
                            syncPanelObjects.ActionsPerformed += new EventHandler(refreshObjects);
                            syncPanelFiles.ShowOpenCheckBox = true;
                        }
                        catch (Exception xTree)
                        {
                            PleaseWait.CloseForm();
                            MessageBox.Show(this, "Error while binding tree", "Interface Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            LogEntry logEntry = new LogEntry();
                            logEntry.Title = "Error While Binding Tree";
                            logEntry.Message = xTree.ToString();
                            Logger.Write(logEntry);
                            Close();
                        }
                    }
                    catch (Exception xInactives)
                    {
                        PleaseWait.CloseForm();
                        MessageBox.Show(this, "Error while marking inactives", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LogEntry logEntry = new LogEntry();
                        logEntry.Title = "Error while marking inactives";
                        logEntry.Message = xInactives.ToString();
                        Logger.Write(logEntry);
                        Close();
                    }
                }
                catch (Exception xSyncWS)
                {
                    PleaseWait.CloseForm();
                    MessageBox.Show(this, "Error while synchronising server workspaces", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogEntry logEntry = new LogEntry();
                    logEntry.Title = "Error while synchronising server workspaces";
                    logEntry.Message = xSyncWS.ToString();
                    Logger.Write(logEntry);
                    Close();
                }
                finally
                {
                    PleaseWait.CloseForm();
                }

                syncPanelFiles.ActionsPerformed += new EventHandler(ActionsPerformed);
                syncPanelObjects.ActionsPerformed += new EventHandler(ActionsPerformed);
            }
            catch (Exception cEx)
            {
                PleaseWait.CloseForm();
                LogEntry logEntry = new LogEntry();
                logEntry.Title = "Error while synchronising server workspaces";
                logEntry.Message = cEx.ToString();
                Logger.Write(logEntry);
                Close();
            }
            setStatus("Ready");
        }

        private void ActionsPerformed(object sender, EventArgs e)
        {
            //RefreshRepository(true);
            tree_AfterSelect(LastSender, new TreeViewEventArgs(LastNode, TreeViewAction.ByMouse));
        }

        private void refreshFiles(object sender, EventArgs e)
        {
            switch (navigationPane1.SelectedIndex)
            {
                case 0:
                    GraphFileCollectionNode fileNode = LastNode as GraphFileCollectionNode;
                    lastSender = treeRemote;
                    fileNode.RefreshNodes();
                    if (fileNode != null)
                    {
                        TreeViewEventArgs args = new TreeViewEventArgs(fileNode);
                        BindData(sender, args);
                    }
                    break;
                case 1:
                    GraphFileCollectionNode fileNodeLocal = LastNode as GraphFileCollectionNode;
                    fileNodeLocal.RefreshNodes();
                    lastSender = treeLocal;
                    if (fileNodeLocal != null)
                    {
                        TreeViewEventArgs args = new TreeViewEventArgs(fileNodeLocal);
                        BindData(sender, args);
                    }
                    break;
            }
        }

        private void refreshObjects(object sender, EventArgs e)
        {
        }

        private void resetTables()
        {
            setStatus("Resetting Tables");
            syncPanelFiles.ResetTable();
            syncPanelObjects.ResetTable();
            syncPanelRelationships.ResetTable();
        }

        private void tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node is ItemNode)
            {
                resetTables();
                setStatus("Initializing");

                //ItemNode inode = e.Node as ItemNode;
                //inode.LoadChildren();
            }
            else
            {
                setStatus("Ready");
            }
            buttonRefreshRepository.Enabled = true;
            LastNode = e.Node;
            LastSender = sender;
            BindData(sender, e);
        }

        private void treeLocal_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node is ItemNode)
            {
                ItemNode itemNode = e.Node as ItemNode;
                if (itemNode.Items.Count == 0 && itemNode.GetType() != typeof(ClassCollectionNode))
                    itemNode.LoadChildren();
            }
        }

        #endregion Methods

        private void treeRemote_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeRemote.SelectedNode = e.Node;
        }

        private void ObjectManager_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            MetaBuilder.SplashScreen.PleaseWait.SetStatus("Closing");
            MetaBuilder.SplashScreen.PleaseWait.CloseForm();
            MetaBuilder.SplashScreen.PleaseWait.SetStatus("Please Wait");
        }

        private void deleteWorkspaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WorkspaceNode wsNode = treeRemote.SelectedNode as WorkspaceNode;
            deleteWorkspace(wsNode.Workspace);
            BindTree(0);
        }

        private void contextStripServer_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            return;

            //must be workspace node
            if (!(treeRemote.SelectedNode is WorkspaceNode))
            {
                e.Cancel = true;
                return;
            }
            //must have admin permission
            WorkspaceNode wsNode = treeRemote.SelectedNode as WorkspaceNode;
            //PermissionService perm = new PermissionService();
            if (permissionService.GetServerPermission(wsNode.Workspace.Name, wsNode.Workspace.WorkspaceTypeId) != PermissionList.Delete)
            {
                e.Cancel = true;
                return;
            }
        }
        private void deleteWorkspace(Workspace ws)
        {
            if (MessageBox.Show(this, "This will delete everything, including permissions from the " + ws.Name + " workspace." + Environment.NewLine + "Please note that this process is permanent and cannot be reversed." + Environment.NewLine + "It may also delete relationships within other workspaces within which some objects reside.", "Are you sure you want to delete this workspace?", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.No)
                return;

            PleaseWait.ShowPleaseWaitForm();
            PleaseWait.SetStatus("Finding all workspace metadata...");

            TList<MetaObject> objects = d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.MetaObjectProvider.GetByWorkspaceNameWorkspaceTypeId(ws.Name, ws.WorkspaceTypeId);
            TList<ObjectAssociation> associations = new TList<ObjectAssociation>();
            TList<Artifact> artifacts = new TList<Artifact>();
            TList<ObjectFieldValue> objectFieldValues = new TList<ObjectFieldValue>();
            TList<GraphFile> graphFiles = new TList<GraphFile>();
            TList<GraphFileObject> graphFileObjects = new TList<GraphFileObject>();
            TList<GraphFileAssociation> graphFileAssociations = new TList<GraphFileAssociation>();

            #region Objects in workspace
            //get associations
            foreach (MetaObject obj in objects)
            {
                //associations
                foreach (ObjectAssociation parentObj in d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(obj.pkid, obj.Machine))
                {
                    if (!associations.Contains(parentObj))
                        associations.Add(parentObj);
                }
                foreach (ObjectAssociation childObj in d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.ObjectAssociationProvider.GetByChildObjectIDChildObjectMachine(obj.pkid, obj.Machine))
                {
                    if (!associations.Contains(childObj))
                        associations.Add(childObj);
                }

                //values
                objectFieldValues.AddRange(d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.ObjectFieldValueProvider.GetByObjectIDMachineID(obj.pkid, obj.Machine));

                //gfo
                graphFileObjects.AddRange(d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.GraphFileObjectProvider.GetByMetaObjectIDMachineID(obj.pkid, obj.Machine));

                //gf
                foreach (GraphFile gf in d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.GraphFileProvider.GetByMetaObjectIDMachineIDFromGraphFileObject(obj.pkid, obj.Machine))
                    if (!graphFiles.Contains(gf) && gf.WorkspaceName == ws.Name && gf.WorkspaceTypeId == gf.WorkspaceTypeId)
                        graphFiles.Add(gf);
            }
            //get artifacts
            foreach (ObjectAssociation association in associations)
            {
                foreach (Artifact art in d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.ArtifactProvider.GetByObjectIDObjectMachine(association.ObjectID, association.ObjectMachine))
                    if (art.ChildObjectID == association.ChildObjectID && art.ChildObjectMachine == association.ChildObjectMachine)
                        if (!artifacts.Contains(art))
                        {
                            artifacts.Add(art);
                            //artifact values should be in already
                        }

                //gfa
                graphFileAssociations.AddRange(d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.GraphFileAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(association.CAid, association.ObjectID, association.ChildObjectID, association.ObjectMachine, association.ChildObjectMachine));
            }
            #endregion

            #region objects not in this workspace but on files

            foreach (GraphFile wsFile in d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.GraphFileProvider.GetByWorkspaceNameWorkspaceTypeId(ws.Name, ws.WorkspaceTypeId))
            {
                //add !gfa
                foreach (GraphFileAssociation wsFileAssociation in d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.GraphFileAssociationProvider.GetByGraphFileIDGraphFileMachine(wsFile.pkid, wsFile.Machine))
                {
                    if (!graphFileAssociations.Contains(wsFileAssociation))
                        graphFileAssociations.Add(wsFileAssociation);
                }
                //add !gfo
                foreach (GraphFileObject wsFileObject in d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.GraphFileObjectProvider.GetByGraphFileIDGraphFileMachine(wsFile.pkid, wsFile.Machine))
                {
                    if (!graphFileObjects.Contains(wsFileObject))
                        graphFileObjects.Add(wsFileObject);
                }
            }

            #endregion

            //delete gfa
            try
            {
                PleaseWait.SetStatus("Deleting Workspace - GFA");
                d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.GraphFileAssociationProvider.Delete(graphFileAssociations);
            }
            catch
            {
            }
            //deleta gfo
            try
            {
                PleaseWait.SetStatus("Deleting Workspace - GFO");
                d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.GraphFileObjectProvider.Delete(graphFileObjects);
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog(ex.ToString());
            }
            //delete gf
            try
            {
                PleaseWait.SetStatus("Deleting Workspace - GF");
                d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.GraphFileProvider.Delete(graphFiles);
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog(ex.ToString());
            }
            //delete ofv
            try
            {
                PleaseWait.SetStatus("Deleting Workspace - OFV");
                d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.ObjectFieldValueProvider.Delete(objectFieldValues);
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog(ex.ToString());
            }
            //delete art
            try
            {
                PleaseWait.SetStatus("Deleting Workspace - A");
                d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.ArtifactProvider.Delete(artifacts);
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog(ex.ToString());
            }
            //delete ass 
            try
            {
                PleaseWait.SetStatus("Deleting Workspace - ASS");
                d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.ObjectAssociationProvider.Delete(associations);
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog(ex.ToString());
            }
            //delete o
            try
            {
                PleaseWait.SetStatus("Deleting Workspace - O");
                d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.MetaObjectProvider.Delete(objects);
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog(ex.ToString());
            }

            try
            {
                //delete permissions
                PleaseWait.SetStatus("Deleting Workspace - PS");
                System.Data.SqlClient.SqlCommand com = new System.Data.SqlClient.SqlCommand();
                System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(Core.Variables.Instance.ServerConnectionString);
                com.Connection = conn;
                com.CommandText = "delete userpermission where workspacename = '" + ws.Name + "' AND workspacetypeid = " + ws.WorkspaceTypeId;
                com.CommandType = System.Data.CommandType.Text;
                conn.Open();
                com.ExecuteNonQuery();
                conn.Close();
                //delete w
                PleaseWait.SetStatus("Deleting Workspace");
                d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.WorkspaceProvider.Delete(ws);
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog(ex.ToString());
            }

            PleaseWait.SetStatus("Complete");
            PleaseWait.CloseForm();
            RefreshRepository(false);
        }

        private void buttonRefreshRepository_Click(object sender, EventArgs e)
        {
            RefreshRepository(false);
        }

    }
}