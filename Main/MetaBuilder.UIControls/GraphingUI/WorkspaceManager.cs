using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using MetaBuilder.BusinessFacade.Storage;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Meta;
using MetaBuilder.SplashScreen;
using MetaBuilder.UIControls.Dialogs.DatabaseManagement;
using MetaBuilder.UIControls.GraphingUI.RepositoryTree;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using MetaBuilder.BusinessFacade.Storage.RepositoryTemp;

namespace MetaBuilder.UIControls.GraphingUI
{
    public partial class WorkspaceManager : Form
    {

        #region Fields (1)

        private bool lv1_mdown = false;

        #endregion Fields

        #region Constructors (1)
        private bool Server = false;
        public WorkspaceManager(bool useServer)
        {
            Server = useServer;
            if (Server)
                this.Text += " Server";
            InitializeComponent();
            this.ShowInTaskbar = false;
            enableAddButton();
        }

        #endregion Constructors

        #region Properties (2)
        private void RemindUserToUpdateFiles()
        {
            MessageBox.Show(this,
                "Workspace transfers were done... Refresh the diagrams on your filesystem" +
                Environment.NewLine + Environment.NewLine +
                "Also note that the Sandbox is the active workspace now.", "Clear Database", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        private b.Workspace SourceWorkspace
        {
            get
            {
                TreeNode tnSource = treeSource.SelectedNode;
                TreeNode tnParent = tnSource;
                while (!(tnParent is WorkspaceNode))
                {
                    tnParent = tnParent.Parent;
                }
                WorkspaceNode wsNode = tnParent as WorkspaceNode;
                return wsNode.Workspace;

            }

        }
        private Workspace TargetWorkspace
        {
            get
            {
                if (treeTarget.SelectedNode != null && treeTarget.SelectedNode.Tag is Workspace)
                    return treeTarget.SelectedNode.Tag as Workspace;
                return null;
            }
        }

        #endregion Properties

        #region Methods (14)

        // Private Methods (14) 

        private void btnAddItemsToTransfer_Click(object sender, EventArgs e)
        {
            WorkspaceNode wsNodeTarget = treeTarget.SelectedNode as WorkspaceNode;
            TreeNode currentNode = treeSource.SelectedNode;
            while (currentNode.Parent != null && !(currentNode.Parent is RepositoryNode))
            {
                currentNode = currentNode.Parent;
            }
            WorkspaceNode wsnode = currentNode as WorkspaceNode;
            if (wsnode.Workspace.pkid == wsNodeTarget.Workspace.pkid && wsnode.Workspace.Name == wsNodeTarget.Workspace.Name)
            {
                MessageBox.Show(this, "Cannot transfer to the same workspace");
                return;
            }

            //lvi.Remove();
            foreach (ListViewItem item in listSource.CheckedItems)
            {
                item.Remove();
                listTarget.Items.Add(item);
            }
            SetStatus(null);
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            TransferStuff();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void listSource_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void listSource_MouseDown(object sender, MouseEventArgs e)
        {
            lv1_mdown = true;
        }

        private void listSource_MouseMove(object sender, MouseEventArgs e)
        {
            if (!lv1_mdown) return;
            if (e.Button == MouseButtons.Right) return;
            ListViewItem[] items = new ListViewItem[listSource.SelectedItems.Count];
            int i = 0;
            foreach (ListViewItem myitem in listSource.SelectedItems)
            {
                items[i] = myitem;
                i++;
            }
            listTarget.DoDragDrop(new DataObject("System.Windows.Forms.ListViewItem()", items), DragDropEffects.Move);
        }

        private void listTarget_DragDrop(object sender, DragEventArgs e)
        {
            WorkspaceNode wsNodeTarget = treeTarget.SelectedNode as WorkspaceNode;
            TreeNode currentNode = treeSource.SelectedNode;
            while (currentNode.Parent != null)
            {
                currentNode = currentNode.Parent;
            }
            WorkspaceNode wsnode = currentNode as WorkspaceNode;
            if (wsnode.Workspace.pkid == wsNodeTarget.Workspace.pkid &&
                wsnode.Workspace.Name == wsNodeTarget.Workspace.Name)
            {
                MessageBox.Show(this, "Cannot transfer to the same workspace");
                e.Effect = DragDropEffects.None;
                lv1_mdown = false;
                return;
            }
            object o = e.Data.GetData(DataFormats.Serializable);
            ListViewItem[] items = e.Data.GetData("System.Windows.Forms.ListViewItem()") as ListViewItem[];
            //lvi.Remove();
            foreach (ListViewItem item in items)
            {
                item.Remove();
                listTarget.Items.Add(item);
            }
            lv1_mdown = false;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }

        bool ServerTransfer = false;
        //put this in a thread
        private void TransferGraphFile(GraphFile file, string provider)
        {
            log.AddMessage("Workspace Transfer");
            if (TargetWorkspace != null)
            {
                ServerTransfer = provider == Core.Variables.Instance.ServerProvider;
                WorkspaceHelper.ChangeWorkspaceForGraphFile(file, TargetWorkspace.Name, TargetWorkspace.WorkspaceTypeId, provider);
                if (file.IsActive && !(IsFileUniqueIDAffected(file.OriginalFileUniqueID.ToString())))
                    affectedFiles.Add(file);
                ActionResult itemResult = new ActionResult();
                itemResult.Success = true;
                itemResult.Repository = "Diagram";
                itemResult.Message = Core.strings.GetFileNameWithoutExtension(file.Name);
                itemResult.FromState = SourceWorkspace.Name;
                itemResult.TargetState = TargetWorkspace.Name;
                //add to result
                log.AddMessage(itemResult);
                if (cbIncludeEmbeddedObjects.Checked)
                {
                    foreach (MetaObject obj in DataRepository.Connections[provider].Provider.MetaObjectProvider.GetByGraphFileIDGraphFileMachineFromGraphFileObject(file.pkid, file.Machine))
                    {
                        ActionResult childResult = new ActionResult();
                        childResult.Repository = "Object";
                        childResult.Message = "[" + obj.Class + " ] " + Loader.GetFromProvider(obj.pkid, obj.Machine, obj.Class, ServerTransfer).ToString();
                        childResult.FromState = obj.WorkspaceName;
                        childResult.TargetState = TargetWorkspace.Name;

                        MetaObject mo = obj;
                        if (mo.WorkspaceName == SourceWorkspace.Name && mo.WorkspaceTypeId == SourceWorkspace.WorkspaceTypeId)
                        {
                            if (ServerTransfer)
                            {
                                TransferMetaObject(mo.pkid, mo.Machine, provider);
                                childResult.Message += "(" + (VCStatusList)mo.VCStatusID + ")";
                            }
                            else
                            {
                                if (mo.VCStatusID == 7)
                                    TransferMetaObject(mo.pkid, mo.Machine, provider);
                            }
                            childResult.Success = true;
                        }
                        else
                        {
                            childResult.Success = false;
                            childResult.intermediate = true;
                            childResult.Message += " - workspace differs from source workspace";
                        }

                        //add to child result
                        log.AddMessage(childResult);
                    }
                }
            }
            else
            {
                MessageBox.Show(this, "Select a target workspace first");
            }
        }
        private void TransferMetaObject(int ObjectID, string machine, string provider)
        {
            if (TargetWorkspace != null)
            {
                if (provider != Core.Variables.Instance.ServerProvider)
                {
                    MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter adapter = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();
                    //find associated graphfiles
                    TList<GraphFile> gs = adapter.GetFilesByObjectId(ObjectID, machine, (provider == Core.Variables.Instance.ServerProvider));//d.DataRepository.Connections[provider].Provider.GraphFileProvider.GetByMetaObjectIDMachineIDFromGraphFileObject(ObjectID, machine);
                    foreach (GraphFile gfO in gs)
                    {
                        ServerTransfer = false;
                        if (gfO.IsActive && !(IsFileUniqueIDAffected(gfO.OriginalFileUniqueID.ToString())) && gfO.VCStatusID != 1)
                            if (!affectedFiles.Contains(gfO))
                                affectedFiles.Add(gfO);
                    }
                }
                else
                {
                    ServerTransfer = true;
                }

                //transferobjects
                WorkspaceHelper.ChangeWorkspaceForMetaObject(ObjectID, machine, TargetWorkspace.Name, TargetWorkspace.WorkspaceTypeId, provider);
            }
            else
            {
                MessageBox.Show(this, "Select a target workspace first");
            }
        }
        //Hacks for netiers, hacks everywhere.
        private bool IsFileUniqueIDAffected(string uniqueID)
        {
            foreach (GraphFile f in affectedFiles)
                if (f.OriginalFileUniqueID.ToString() == uniqueID)
                    return true;

            return false;
        }

        private TList<GraphFile> affectedFiles = new TList<GraphFile>();
        private List<MetaBase> affectedObjects = new List<MetaBase>();

        LogDisplayer log;

        private void TransferStuff()
        {
            log = new LogDisplayer(true);
            affectedFiles = new TList<GraphFile>();
            //reset server list
            WorkspaceHelper.Reset();
            foreach (ListViewItem lvi in listTarget.Items)
            {
                object tag = lvi.Tag;
                if (tag is WorkspaceTransferTagObject)
                {
                    WorkspaceTransferTagObject tagObject = tag as WorkspaceTransferTagObject;
                    if (tagObject.Item is GraphFile)
                    {
                        TransferGraphFile(tagObject.Item as GraphFile, tagObject.Parent.Provider);
                    }
                    else if (tagObject.Item is MetaBase)
                    {
                        MetaBase mb = tagObject.Item as MetaBase;
                        TransferMetaObject(mb.pkid, mb.MachineName, tagObject.Parent.Provider);

                        ActionResult childResult = new ActionResult();
                        childResult.Repository = "Object";
                        childResult.Message = "[" + mb.Class + " ] " + mb.ToString();
                        childResult.FromState = mb.WorkspaceName;
                        childResult.TargetState = TargetWorkspace.Name;
                        childResult.Success = true;

                        //add to child result
                        log.AddMessage(childResult);

                        affectedObjects.Add(mb);
                    }
                }
            }

            if (ServerTransfer)
            {
                MetaBuilder.Graphing.Persistence.GraphFileManager gManager = new MetaBuilder.Graphing.Persistence.GraphFileManager();
                foreach (GraphFile file in WorkspaceHelper.affectedServerFiles)
                    gManager.UpdateObjectWorkspaceInFiles(file, WorkspaceHelper.affectedServerObjects, (ServerTransfer ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider));
            }

            //reset server list for memory
            WorkspaceHelper.Reset();
            listTarget.Items.Clear();

            //
            CacheManager cacheMan = CacheFactory.GetCacheManager();
            cacheMan.Flush();

            //eventlog
            BringToFront();
            log.BringToFront();

            if (affectedFiles.Count > 0)
                log.ShowDialog(this);

            if (ServerTransfer)
            {
                //Display message with information regarding the state of these diagrams
                StringBuilder serversbMessage = new StringBuilder();
                serversbMessage.Append("PLEASE READ THE FOLLOWING CAREFULLY" + Environment.NewLine);
                serversbMessage.Append("One or more diagrams were influenced by the server transfer action").Append(Environment.NewLine);
                serversbMessage.Append("They have been checked into their 'transferred to workspace' and must be checked out by clients to be able to work on them once more.").Append(Environment.NewLine);
                serversbMessage.Append("Clients who had these affected diagrams checked out will no longer be able to check in their changes.").Append(Environment.NewLine);
                serversbMessage.Append(Environment.NewLine);
                MessageBox.Show(this, serversbMessage.ToString(), "Server Diagrams Were Modified", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                Close();
                return;
            }

            //Display message to open those diagrams
            StringBuilder sbMessage = new StringBuilder();
            sbMessage.Append("PLEASE READ THE FOLLOWING INSTRUCTIONS CAREFULLY" + Environment.NewLine);
            sbMessage.Append("One or more diagrams were influenced by the transfer action").Append(Environment.NewLine);
            sbMessage.Append(Environment.NewLine);
            sbMessage.Append("They will now be opened and saved so that the workspace transfer can take effect.").Append(Environment.NewLine);
            MessageBox.Show(this, sbMessage.ToString(), "Diagrams Were Modified", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            bool opened = false;
            //Save affecteddiagrams into folder like merge objects
            foreach (GraphFile g in affectedFiles)
            {
                DockingForm.DockForm.OpenGraphFileFromDatabase(g, true, false);
                DockingForm.DockForm.GetCurrentGraphViewContainer().ForceSaveAs = true;
                DockingForm.DockForm.GetCurrentGraphViewContainer().StartSaveProcess(true);
                //DockingForm.DockForm.GetCurrentGraphViewContainer().Close();
                opened = true;
            }
            affectedFiles.Clear();
            if (opened)
                Close();
            else
                WorkspaceManager_Load(this, EventArgs.Empty);
        }

        private void treeSource_AfterSelect(object sender, TreeViewEventArgs e)
        {
            listSource.Items.Clear();
            ItemNode itemNode = e.Node as ItemNode;
            if (itemNode != null)
            {
                //if (itemNode.Nodes.Count == 0)
                itemNode.LoadChildren();
                foreach (IRepositoryItem item in itemNode.Items)
                {
                    ListViewItem lvi = new ListViewItem(item.ToString());
                    lvi.Tag = new WorkspaceTransferTagObject(item, itemNode);
                    listSource.Items.Add(lvi);
                }
            }
            listSource.Sort();
            listTarget.AllowDrop = true;
            SetStatus(null);
            enableAddButton();
        }

        private void treeTarget_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (listTarget.Items.Count > 0)
            {
                string items = listTarget.Items.Count > 1 ? " items" : " item";
                DialogResult res =
                    MessageBox.Show(this,
                        "You have a pending transfer of " + listTarget.Items.Count + items + " from" +
                        SourceWorkspace.Name + " to " + TargetWorkspace.Name +
                        "." + Environment.NewLine + " Would you like to complete that transfer?", "Pending Transfer",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                    TransferStuff();
            }
        }

        private void enableAddButton()
        {
            btnAddItemsToTransfer.Enabled = TargetWorkspace != null;

            if (treeTarget.SelectedNode != null && treeSource.SelectedNode != null)
                if (treeTarget.SelectedNode.Parent != null && treeSource.SelectedNode.Parent == null)
                {
                    btnAddItemsToTransfer.Enabled = false;
                    SetStatus("Cannot transfer items between providers");
                }
                else if (treeTarget.SelectedNode.Parent != null && treeSource.SelectedNode.Parent.Parent.Parent != null) //CLASSES IN SOURCE
                {
                    if (treeTarget.SelectedNode.Parent.Text != treeSource.SelectedNode.Parent.Parent.Parent.Text)
                    {
                        btnAddItemsToTransfer.Enabled = false;
                        SetStatus("Cannot transfer items between providers");
                    }
                }
                else if (treeTarget.SelectedNode.Parent != null && treeSource.SelectedNode.Parent.Parent != null) //DIAGRAMS IN SOURCE
                {
                    if (treeTarget.SelectedNode.Parent.Text != treeSource.SelectedNode.Parent.Parent.Text)
                    {
                        btnAddItemsToTransfer.Enabled = false;
                        SetStatus("Cannot transfer items between providers");
                    }
                }

        }

        private void WorkspaceManager_Load(object sender, EventArgs e)
        {
            PleaseWait.ShowPleaseWaitForm();
            Application.DoEvents();
            PleaseWait.SetStatus("Fetching data from repository");
            treeSource.Nodes.Clear();
            treeTarget.Nodes.Clear();
            listSource.Items.Clear();
            listTarget.Items.Clear();
            if (Server)
            {
                try
                {
                    TList<Workspace> serverWorkspaces = DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.WorkspaceProvider.GetAll();
                    BusinessFacade.Storage.RepositoryTemp.PermissionService perm = new MetaBuilder.BusinessFacade.Storage.RepositoryTemp.PermissionService();
                    foreach (Workspace ws in serverWorkspaces)
                    {
                        //You must have admin permission
                        if (perm.GetServerPermission(ws.Name, ws.WorkspaceTypeId) != PermissionList.Delete)
                            continue;
                        //Sources
                        WorkspaceNode sourceNode = new WorkspaceNode(true);
                        sourceNode.SkipFileNodes = true;
                        sourceNode.ServerWorkspaceTransferObject = true;
                        sourceNode.Load(ws);
                        if (ws.WorkspaceTypeId == 3)
                            sourceNode.StateImageIndex = 0;
                        else
                            sourceNode.StateImageIndex = 1;
                        treeSource.Nodes.Add(sourceNode);
                        //Targets
                        WorkspaceNode targetNode = new WorkspaceNode(true);
                        targetNode.Workspace = ws;
                        targetNode.SkipFileNodes = true;
                        targetNode.ServerWorkspaceTransferObject = true;
                        targetNode.Text = ws.Name;
                        targetNode.Tag = ws;
                        if (ws.WorkspaceTypeId == 3)
                            targetNode.StateImageIndex = 0;
                        else
                            targetNode.StateImageIndex = 1;
                        treeTarget.Nodes.Add(targetNode);
                    }
                }
                catch (Exception ex)
                {
                    Core.Log.WriteLog(ex.ToString());
                }
            }
            else
            {
                TList<Workspace> workspaces = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.WorkspaceProvider.GetAll();
                foreach (Workspace ws in workspaces)
                {
                    string def = "";
                    if (Core.Variables.Instance.DefaultWorkspace.Length > 0)
                    {
                        if (ws.Name == Core.Variables.Instance.DefaultWorkspace && ws.WorkspaceTypeId == Core.Variables.Instance.DefaultWorkspaceID)
                        {
                            def = " <Default>";
                        }
                    }

                    //Source workspaces can only be client or sandbox
                    //if (ws.WorkspaceTypeId == (int)WorkspaceTypeList.Client || ws.WorkspaceTypeId == (int)WorkspaceTypeList.Sandbox)
                    //{
                    WorkspaceNode wsNode = new WorkspaceNode(true);
                    wsNode.SkipFileNodes = true;
                    wsNode.Load(ws);
                    wsNode.Text += def;
                    if (ws.WorkspaceTypeId == 3)
                        wsNode.StateImageIndex = 0;
                    else
                        wsNode.StateImageIndex = 1;
                    treeSource.Nodes.Add(wsNode);
                    //}
                    // Target can be anything but a template
                    if (ws.WorkspaceTypeId != (int)WorkspaceTypeList.Template)
                    {
                        WorkspaceNode wsNodeTarget = new WorkspaceNode(true);
                        wsNodeTarget.SkipFileNodes = true;
                        wsNodeTarget.Workspace = ws;
                        wsNodeTarget.Text = ws.Name + def;
                        wsNodeTarget.Tag = ws;
                        if (ws.WorkspaceTypeId == 3)
                            wsNodeTarget.StateImageIndex = 0;
                        else
                            wsNodeTarget.StateImageIndex = 1;
                        treeTarget.Nodes.Add(wsNodeTarget);
                    }
                }
            }
            PleaseWait.SetStatus("");
            PleaseWait.CloseForm();
            BringToFront();
        }

        #endregion Methods

        private void buttonClearTargets_Click(object sender, EventArgs e)
        {
            listTarget.Items.Clear();
            if (treeSource.SelectedNode is ItemNode)
                loadItems(treeSource.SelectedNode as ItemNode);
            SetStatus(null);
        }
        private void loadItems(ItemNode itemNode)
        {
            listSource.Items.Clear();
            if (itemNode != null)
            {
                //if (itemNode.Nodes.Count == 0)
                itemNode.LoadChildren();
                foreach (IRepositoryItem item in itemNode.Items)
                {
                    ListViewItem lvi = new ListViewItem(item.ToString());
                    lvi.Tag = item;
                    listSource.Items.Add(lvi);
                }
            }
            listSource.Sort();
            listTarget.AllowDrop = true;
        }

        private void treeTarget_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SetStatus(null);
            enableAddButton();
        }

        private void listSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            enableAddButton();
        }

        private void SetStatus(string message)
        {
            if (message.Length == 0)
            {
                message = listTarget.Items.Count.ToString() + " items ready to transfer";

                labelMessage.Text = message;
            }
            else
            {
                if (message.Length > 0)
                    labelMessage.Text = message;
                else
                    labelMessage.Text = "Fetching Data...";
            }
            //Enable buttons based on selections
            btnApply.Enabled = listTarget.Items.Count > 0;
        }

        private void Select(string what)
        {
            switch (what)
            {
                case "All":
                    foreach (ListViewItem i in listSource.Items)
                        i.Checked = true;
                    break;
                case "None":
                    foreach (ListViewItem i in listSource.Items)
                        i.Checked = false;
                    break;
                case "Invert":
                    foreach (ListViewItem i in listSource.Items)
                        i.Checked = !i.Checked;
                    break;
            }
        }

        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            Select("All");
        }

        private void buttonSelectNone_Click(object sender, EventArgs e)
        {
            Select("None");
        }

        private void buttonSelectInvert_Click(object sender, EventArgs e)
        {
            Select("Invert");
        }

        private void treeTarget_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            WorkspaceNode wsNodeTarget = e.Node as WorkspaceNode;
            TreeNode currentNode = treeSource.SelectedNode;
            while (currentNode.Parent != null && !(currentNode.Parent is RepositoryNode))
            {
                currentNode = currentNode.Parent;
            }
            WorkspaceNode wsnode = currentNode as WorkspaceNode;
            if (wsnode.Workspace.pkid == wsNodeTarget.Workspace.pkid && wsnode.Workspace.Name == wsNodeTarget.Workspace.Name)
            {
                MessageBox.Show(this, "Cannot transfer to the same workspace");
                return;
            }

            //lvi.Remove();
            foreach (ListViewItem item in listSource.CheckedItems)
            {
                item.Remove();
                listTarget.Items.Add(item);
            }
            SetStatus(null);
        }
    }

    public class WorkspaceTransferTagObject
    {
        public IRepositoryItem Item;
        public ItemNode Parent;

        public WorkspaceTransferTagObject(IRepositoryItem item, ItemNode parent)
        {
            Item = item;
            Parent = parent;
        }
    }
}