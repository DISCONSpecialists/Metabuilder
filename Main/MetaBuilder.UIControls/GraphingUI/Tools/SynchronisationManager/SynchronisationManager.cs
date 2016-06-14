using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.BusinessFacade.Storage.RepositoryTemp; //THIS MUST MOVE TO NOT TEMP NAMESPACE
using MetaBuilder.Meta;
using XPTable.Models;
using XPTable.Editors;

namespace MetaBuilder.UIControls.GraphingUI.Tools.SynchronisationManager
{
    public partial class SynchronisationManager : Form
    {
        public enum RepositoryType
        {
            Diagrams = 0, Objects = 1
        }

        private RepositoryType repoType;
        public RepositoryType RepoType
        {
            get { return repoType; }
            set { repoType = value; }
        }

        private PermissionService permissionService;
        public PermissionService PermService
        {
            get
            {
                if (permissionService == null)
                    permissionService = new PermissionService();
                return permissionService;
            }
            set
            {
                permissionService = value;
            }
        }

        private Workspace currentWorkspace;
        public Workspace CurrentWorkspace
        {
            get { return currentWorkspace; }
            set { currentWorkspace = value; }
        }

        public SynchronisationManager()
        {
            InitializeComponent();
            this.Width += 150;
            Load += new EventHandler(SynchronisationManager_Load);
        }

        private void SynchronisationManager_Load(object sender, EventArgs e)
        {
            Init();
            toolStripButtonLoadEmbedded.CheckedChanged += new EventHandler(toolStripButtonLoadEmbedded_CheckedChanged);
        }

        private void toolStripButtonLoadEmbedded_CheckedChanged(object sender, EventArgs e)
        {
            if (tableModel1.Table.SelectedIndicies.Length > 0 && toolStripButtonLoadEmbedded.Checked)
            {
                cancel = true;
                object parm = tableModel1.Rows[tableModel1.Table.SelectedIndicies[0]];
                Thread t = new Thread(new ParameterizedThreadStart(LoadEmbedded));
                cancel = false;
                Thread.Sleep(250);
                t.Start(parm);
            }
            else if (!toolStripButtonLoadEmbedded.Checked)
            {
                cancel = true;
                List<Row> rowsToRemove = new List<Row>();
                foreach (Row r in tableModel1.Rows)
                    if (r.Enabled == false)
                        rowsToRemove.Add(r);
                RemoveRange(rowsToRemove);
            }
        }

        private void Init()
        {
            UpdateStatusLabel("Synchronising Workspaces");

            PermService.SynchroniseServerWorkspaces();
            if (PermService.RemoteWorkspaces.Count <= 0)
            {
                DockingForm.DockForm.DisplayTip("Go to database management to add workspaces or contact the system administrator.", "No server workspaces available");
                Close();
            }

            RepoType = RepositoryType.Diagrams;
            toolStripButtonSwitchRepository.Text = RepoType.ToString();

            toolStripComboBoxWorkspace.Sorted = false;
            toolStripComboBoxWorkspace.ToolTipText = "Select a workspace to view its repository items";
            toolStripComboBoxWorkspace.Items.Add("All");
            foreach (Workspace workspace in PermService.RemoteWorkspaces)
            {
                if (workspace.WorkspaceTypeId != 3)
                    continue;
                toolStripComboBoxWorkspace.Items.Add(workspace);
            }
            toolStripComboBoxWorkspace.ComboBox.DisplayMember = "Name";
            toolStripComboBoxWorkspace.SelectedIndex = 0; //This line will cause loadRepository to fire a new thread for the ALL(Null workspace) workspace

            ResetFeedbackControls();

            System.Windows.Forms.Timer openFileTimer = new System.Windows.Forms.Timer();
            openFileTimer.Interval = 500;
            openFileTimer.Tick += new EventHandler(openFileTimer_Tick);
            openFileTimer.Enabled = true;
            openFileTimer.Start();
        }
        private List<VersionControlledItem> ignoreOpenItems = new List<VersionControlledItem>();
        private void openFileTimer_Tick(object sender, EventArgs e)
        {
            //open items
            if (OpenAfterExecuteItems == null || BlobsUpdated == false)
                return;

            DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter tga = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();

            foreach (VersionControlledItem itemToOpen in OpenAfterExecuteItems)
            {
                if (ignoreOpenItems.Contains(itemToOpen))
                    continue;
                ignoreOpenItems.Add(itemToOpen);
                try
                {
                    //get active file
                    GraphFile fileToOpen = tga.GetFileDetails(itemToOpen.ID, false);//DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileProvider.Find("OriginalFileUniqueID = '" + itemToOpen.ID + "'").Find(GraphFileColumn.IsActive, true);
                    if (fileToOpen != null)
                    {
                        DockingForm.DockForm.OpenGraphFileFromDatabase(fileToOpen, true, true);
                        DockingForm.DockForm.GetCurrentGraphViewContainer().Visible = true;
                        DockingForm.DockForm.GetCurrentGraphViewContainer().BringToFront();
                        DockingForm.DockForm.GetCurrentGraphViewContainer().Show();
                        DockingForm.DockForm.GetCurrentGraphViewContainer().ForceSaveAs = true;
                        DockingForm.DockForm.GetCurrentGraphViewContainer().OpeningFromServer = true;
                    }
                    else
                    {
                        //log that the file cannot be found
                        itemToOpen.ToString();
                    }
                }
                catch
                {
                    itemToOpen.ToString();
                }
            }
            if (ignoreOpenItems.Count > 0)
                Close();
        }

        public void ResetFeedbackControls()
        {
            UpdateStatusLabel("Ready");
            UpdateProgressValue(0);
            UpdateProgressTotal(100);
        }
        public void UpdateStatusLabel(string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(UpdateStatusLabel), new object[] { message });
            }
            else
            {
                buttonExecute.Enabled = toolStripComboBoxWorkspace.Enabled = tableRepository.Enabled = (message == "Ready");
                labelProgress.Text = message;
            }
        }
        public void UpdateProgressTotal(int total)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<int>(UpdateProgressTotal), new object[] { total });
            }
            else
            {
                progressBar.Maximum = total;
            }
        }
        public void UpdateProgressValue(int progress)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<int>(UpdateProgressValue), new object[] { progress });
            }
            else
            {
                progressBar.Value = progress;
            }
        }

        private void SetWorkspace()
        {
            if (toolStripComboBoxWorkspace.SelectedItem is Workspace)
                CurrentWorkspace = toolStripComboBoxWorkspace.SelectedItem as Workspace;
            else
                CurrentWorkspace = null;
        }

        private void toolStripButtonRefreshRepository_Click(object sender, EventArgs e)
        {
            //reload all items in current repository
            InitRepositoryThread();
        }
        private void toolStripComboBoxWorkspace_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            SetWorkspace();
            if (CurrentWorkspace != null)
            {
                string permString = "Permission : ";
                foreach (UserPermission perm in PermService.RemotePermissions.FindAll(UserPermissionColumn.WorkspaceName, CurrentWorkspace.Name))
                {
                    if (((PermissionList)perm.PermissionID).ToString() == "Admin")
                        continue;
                    permString += ((PermissionList)perm.PermissionID).ToString() + " ";
                }
                if (permString == "")
                    permString = "None";
                toolStripLabelWorkspacePermission.Text = permString;
            }
            else
            {
                toolStripLabelWorkspacePermission.Text = "";
            }

            InitRepositoryThread();
        }

        private delegate void SetDGValueDelegate(List<object> source);
        private void SetDGValue(List<object> source)
        {
            if (InvokeRequired)
                Invoke(new SetDGValueDelegate(SetDGValue), source);
            else
            {
                tableModel1.Rows.Clear();

                foreach (object o in source)
                {
                    Row newRow = new Row();

                    Cell cID = new Cell();
                    cID.Text = (o as VersionControlledItem).ID;
                    cID.Editable = false;
                    cID.BackColor = Color.LightGray;
                    newRow.Cells.Add(cID);
                    columnModel1.Columns[0].Width = 50;

                    Cell cWorkspace = new Cell();
                    cWorkspace.Text = (o as VersionControlledItem).Workspace;
                    cWorkspace.Editable = false;
                    cWorkspace.BackColor = Color.LightGray;
                    newRow.Cells.Add(cWorkspace);
                    columnModel1.Columns[1].Width = 75;

                    Cell cUser = new Cell();
                    cUser.Text = (o as VersionControlledItem).User;
                    cUser.Editable = false;
                    newRow.Cells.Add(cUser);
                    columnModel1.Columns[2].Width = 100;

                    Cell cDescription = new Cell();
                    cDescription.Text = (o as VersionControlledItem).Description;
                    cDescription.Editable = false;
                    newRow.Cells.Add(cDescription);
                    columnModel1.Columns[3].Width = 250;

                    Cell cClientState = new Cell();
                    cClientState.Text = (o as VersionControlledItem).ClientState;
                    cClientState.Editable = false;
                    newRow.Cells.Add(cClientState);
                    columnModel1.Columns[4].Width = 100;

                    Cell cServerState = new Cell();
                    cServerState.Text = (o as VersionControlledItem).ServerState;
                    cServerState.Editable = false;
                    newRow.Cells.Add(cServerState);
                    columnModel1.Columns[5].Width = 100;

                    Cell cUserState = new Cell();
                    cUserState.Text = "None";
                    cUserState.Editable = true;
                    newRow.Cells.Add(cUserState);
                    columnModel1.Columns[6].Width = 100;

                    newRow.Tag = (o as VersionControlledItem);
                    if ((o as VersionControlledItem).ServerItem == null)
                        newRow.ForeColor = Color.LightCoral;
                    tableModel1.Rows.Add(newRow);

                    tableRepository.Refresh();
                }
            }
        }
        private bool InitRepo = false;
        private void InitRepositoryThread()
        {
            Thread t = new Thread(InitRepository);
            t.IsBackground = true;
            t.Name = "MBir" + DateTime.Now.ToShortTimeString();
            t.Start();
        }
        private void InitRepository()
        {
            if (InitRepo)
                return;
            InitRepo = true;
            UpdateProgressTotal(100);
            UpdateProgressValue(15);
            //SetDGValue(null);
            DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter tga = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();

            List<object> usedClient = new List<object>();
            if (RepoType == RepositoryType.Diagrams)
            {
                UpdateProgressValue(30);
                UpdateStatusLabel("Loading Diagrams");
                List<object> files = new List<object>();

                TList<GraphFile> clientFiles = tga.GetAllFilesByTypeID(1, false);
                if (CurrentWorkspace != null)
                {
                    foreach (GraphFile f in tga.GetAllFilesByTypeID(1, true).FindAll(GraphFileColumn.WorkspaceTypeId, CurrentWorkspace.WorkspaceTypeId).FindAll(GraphFileColumn.WorkspaceName, CurrentWorkspace.Name))
                    {
                        if (f.IsActive == false)
                            continue;
                        object clientFile = null;
                        try
                        {
                            TList<GraphFile> clientFilesInner = clientFiles.FindAll(GraphFileColumn.OriginalFileUniqueID, f.OriginalFileUniqueID).FindAll(GraphFileColumn.IsActive, true);
                            clientFile = clientFilesInner[clientFilesInner.Count - 1];
                            if (clientFile != null)
                                usedClient.Add(f.OriginalFileUniqueID.ToString());
                        }
                        catch
                        {
                            f.ToString();
                        }

                        files.Add(new VersionControlledItem(PermService, clientFile, f, SynchronisationRules.GetPermissionForItem(PermService, f)));
                    }
                    UpdateStatusLabel("Loading Client Diagrams");
                    UpdateProgressValue(50);
                    foreach (GraphFile f in clientFiles.FindAll(GraphFileColumn.WorkspaceTypeId, CurrentWorkspace.WorkspaceTypeId).FindAll(GraphFileColumn.WorkspaceName, CurrentWorkspace.Name))
                    {
                        if (f.IsActive == false || usedClient.Contains(f.OriginalFileUniqueID.ToString()))
                            continue;
                        files.Add(new VersionControlledItem(PermService, f, null, SynchronisationRules.GetPermissionForItem(PermService, f)));
                    }
                }
                else
                {
                    foreach (GraphFile f in tga.GetAllFilesByTypeID(1, true))
                    {
                        if (f.IsActive == false)
                            continue;
                        object clientFile = null;
                        try
                        {
                            TList<GraphFile> clientFilesInner = clientFiles.FindAll(GraphFileColumn.OriginalFileUniqueID, f.OriginalFileUniqueID).FindAll(GraphFileColumn.IsActive, true);
                            clientFile = clientFilesInner[clientFilesInner.Count - 1];
                            if (clientFile != null)
                                usedClient.Add(f.OriginalFileUniqueID.ToString());
                        }
                        catch
                        {
                            f.ToString();
                        }
                        files.Add(new VersionControlledItem(PermService, clientFile, f, SynchronisationRules.GetPermissionForItem(PermService, f)));
                    }
                    UpdateStatusLabel("Loading Client Diagrams");
                    UpdateProgressValue(50);
                    foreach (GraphFile f in clientFiles.FindAll(GraphFileColumn.WorkspaceTypeId, 3))
                    {
                        if (f.IsActive == false || usedClient.Contains(f.OriginalFileUniqueID.ToString()))
                            continue;
                        files.Add(new VersionControlledItem(PermService, f, null, SynchronisationRules.GetPermissionForItem(PermService, f)));
                    }
                }

                UpdateStatusLabel("Binding");
                UpdateProgressValue(70);

                SetDGValue(files);
            }
            else if (RepoType == RepositoryType.Objects)
            {
                UpdateStatusLabel("Loading Objects");
                UpdateProgressValue(30);
                List<object> objects = new List<object>();
                TList<MetaObject> clientObjects = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetAll();
                if (CurrentWorkspace != null)
                {
                    foreach (MetaObject obj in DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.MetaObjectProvider.GetByWorkspaceNameWorkspaceTypeId(CurrentWorkspace.Name, CurrentWorkspace.WorkspaceTypeId))
                    {
                        object clientObject = null;
                        try
                        {
                            clientObject = clientObjects.FindAll(MetaObjectColumn.pkid, obj.pkid).Find(MetaObjectColumn.Machine, obj.Machine);
                            if (clientObject != null)
                                usedClient.Add(clientObject);
                        }
                        catch
                        {
                        }
                        objects.Add(new VersionControlledItem(PermService, clientObject, obj, SynchronisationRules.GetPermissionForItem(PermService, obj)));
                    }
                    UpdateStatusLabel("Loading Client Objects");
                    UpdateProgressValue(50);
                    foreach (MetaObject clientObj in clientObjects.FindAll(MetaObjectColumn.WorkspaceTypeId, CurrentWorkspace.WorkspaceTypeId).FindAll(MetaObjectColumn.WorkspaceName, CurrentWorkspace.Name))
                    {
                        if (usedClient.Contains(clientObj))
                            continue;

                        objects.Add(new VersionControlledItem(PermService, clientObj, null, SynchronisationRules.GetPermissionForItem(PermService, clientObj)));
                    }
                }
                else
                {
                    foreach (MetaObject obj in DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.MetaObjectProvider.GetAll())
                    {
                        object clientObject = null;
                        try
                        {
                            clientObject = clientObjects.FindAll(MetaObjectColumn.pkid, obj.pkid).Find(MetaObjectColumn.Machine, obj.Machine);
                            if (clientObject != null)
                                usedClient.Add(clientObject);
                        }
                        catch
                        {
                        }
                        objects.Add(new VersionControlledItem(PermService, clientObject, obj, SynchronisationRules.GetPermissionForItem(PermService, obj)));
                    }
                    UpdateStatusLabel("Loading Client Objects");
                    UpdateProgressValue(50);
                    foreach (MetaObject clientObj in clientObjects.FindAll(MetaObjectColumn.WorkspaceTypeId, 3))
                    {
                        if (usedClient.Contains(clientObj))
                            continue;

                        objects.Add(new VersionControlledItem(PermService, clientObj, null, SynchronisationRules.GetPermissionForItem(PermService, clientObj)));
                    }
                }

                UpdateStatusLabel("Binding");
                UpdateProgressValue(70);

                SetDGValue(objects);
            }

            UpdateProgressValue(90);
            InitRepo = false;

            ResetFeedbackControls();
        }

        private void toolStripButtonSwitchRepository_Click(object sender, EventArgs e)
        {
            if (InitRepo)
                return;
            //switch between object/diagram view
            RepoType = (RepoType == RepositoryType.Diagrams) ? RepositoryType.Objects : RepositoryType.Diagrams;
            toolStripButtonSwitchRepository.Text = RepoType.ToString();
            InitRepositoryThread();
        }

        VersionControlledItem itemEditing = null;
        ComboBoxCellEditor editor = null;
        private void tableRepository_BeginEditing(object sender, XPTable.Events.CellEditEventArgs e)
        {
            if (e.Editor is ComboBoxCellEditor)
            {
                e.Editor.PrepareForEditing(e.Cell, e.Table, e.CellPos, e.CellRect, true);
                (e.Editor as ComboBoxCellEditor).DropDownStyle = DropDownStyle.DropDownList;
                (e.Editor as ComboBoxCellEditor).TheListToAddItemsTo.Items.Clear();

                VersionControlledItem item = e.Table.TableModel.Rows[e.Row].Tag as VersionControlledItem;
                (e.Editor as ComboBoxCellEditor).TheListToAddItemsTo.Items.Add("None");
                foreach (RepositoryAction vc in item.AllowedStates)
                {
                    if (vc.Caption == "") //prevent blank captions
                    {
                        vc.ToString();
                    }
                    (e.Editor as ComboBoxCellEditor).TheListToAddItemsTo.Items.Add(vc);
                    //(e.Editor as ComboBoxCellEditor).TheListToAddItemsTo.DisplayMember = "Caption";
                }
                e.Editor.PrepareForEditing(e.Cell, e.Table, e.CellPos, e.CellRect, true);
                itemEditing = item;
                editor = (e.Editor as ComboBoxCellEditor);
            }
        }
        private void tableRepository_EditingStopped(object sender, XPTable.Events.CellEditEventArgs e)
        {
            if (e.Editor is ComboBoxCellEditor) //its the only cell we can edit, better safe than sorry :)
            {
                e.Cell.Tag = (e.Editor as ComboBoxCellEditor).SelectedItem;
            }
        }

        private void SynchronisationManager_SelectedIndexChanged(object sender, EventArgs e)
        {
            //stop editing

        }

        private delegate void InsertRowDG(int index, Row row);
        private void InsertRow(int index, Row row)
        {
            try
            {
                if (InvokeRequired)
                    Invoke(new InsertRowDG(InsertRow), index, row);
                else
                {
                    tableModel1.Rows.Insert(index, row);
                    tableRepository.Refresh();
                }
            }
            catch
            {
            }
        }
        private delegate void RemoveRangeDG(List<Row> rows);
        private void RemoveRange(List<Row> rows)
        {
            try
            {
                if (InvokeRequired)
                    Invoke(new RemoveRangeDG(RemoveRange), rows);
                else
                    tableModel1.Rows.RemoveRange(rows.ToArray());
            }
            catch
            {
            }
        }

        private void LoadEmbedded(object parent)
        {
            if (!(parent is Row))
                return;

            Row parentRow = parent as Row;
            UpdateProgressTotal(100);
            UpdateProgressValue(25);
            //UpdateStatusLabel("Retrieving Objects");
            (parentRow.Tag as VersionControlledItem).LoadEmbedded();
            int index = parentRow.Index;

            List<Row> rowsToRemove = new List<Row>();
            foreach (Row r in tableModel1.Rows)
                if (r.Enabled == false)
                    rowsToRemove.Add(r);
            int progress = 0;

            //UpdateStatusLabel("Loading(" + (parentRow.Tag as VersionControlledItem).EmbeddedObjects.Count.ToString() + ") Objects");
            UpdateProgressTotal((parentRow.Tag as VersionControlledItem).EmbeddedObjects.Count);
            try
            {
                foreach (VersionControlledItem subitem in (parentRow.Tag as VersionControlledItem).EmbeddedObjects)
                {
                    if (cancel)
                    {
                        ResetFeedbackControls();
                        return;
                    }

                    Row newSubRow = new Row();

                    Cell cIDsub = new Cell();
                    cIDsub.Text = (subitem as VersionControlledItem).ID;
                    cIDsub.Editable = false;
                    newSubRow.Cells.Add(cIDsub);

                    Cell cWorkspacesub = new Cell();
                    cWorkspacesub.Text = (subitem as VersionControlledItem).Workspace;
                    cWorkspacesub.Editable = false;
                    newSubRow.Cells.Add(cWorkspacesub);

                    Cell cUsersub = new Cell();
                    cUsersub.Text = (subitem as VersionControlledItem).User;
                    cUsersub.Editable = false;
                    newSubRow.Cells.Add(cUsersub);

                    Cell cDescriptionsub = new Cell();
                    cDescriptionsub.Text = (subitem as VersionControlledItem).Description;
                    cDescriptionsub.Editable = false;
                    newSubRow.Cells.Add(cDescriptionsub);

                    Cell cClientStatesub = new Cell();
                    cClientStatesub.Text = (subitem as VersionControlledItem).ClientState;
                    cClientStatesub.Editable = false;
                    newSubRow.Cells.Add(cClientStatesub);

                    Cell cServerStatesub = new Cell();
                    cServerStatesub.Text = (subitem as VersionControlledItem).ServerState;
                    cServerStatesub.Editable = false;
                    newSubRow.Cells.Add(cServerStatesub);

                    Cell cUserStatesub = new Cell();
                    cUserStatesub.Text = "";
                    cUserStatesub.Editable = true;
                    newSubRow.Cells.Add(cUserStatesub);

                    newSubRow.Tag = subitem;
                    newSubRow.Editable = false;
                    newSubRow.Enabled = false;

                    index++;
                    InsertRow(index, newSubRow);

                    progress++;
                    UpdateProgressValue(progress);
                }
            }
            catch (Exception ex)
            {
            }

            RemoveRange(rowsToRemove);

            ResetFeedbackControls();
        }

        private bool cancel = false; //used when loading embedded items and new item selected
        //private int index = -1;
        private void tableRepository_SelectionChanged(object sender, XPTable.Events.SelectionEventArgs e)
        {
            if (e.NewSelectedIndicies.Length > 0 && toolStripButtonLoadEmbedded.Checked)
            {
                cancel = true;
                object parm = e.TableModel.Rows[e.NewSelectedIndicies[0]];
                Thread t = new Thread(new ParameterizedThreadStart(LoadEmbedded));
                cancel = false;
                Thread.Sleep(250);
                t.Start(parm);
            }
            else if (!toolStripButtonLoadEmbedded.Checked)
            {
                cancel = true;
                List<Row> rowsToRemove = new List<Row>();
                foreach (Row r in tableModel1.Rows)
                    if (r.Enabled == false)
                        rowsToRemove.Add(r);
                RemoveRange(rowsToRemove);
            }
        }

        private Dictionary<VersionControlledItem, RepositoryAction> ItemAndAction;

        private void buttonExecute_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(Execute);
            t.Start();
        }

        private List<VersionControlledItem> OpenAfterExecuteItems;
        string SQLBatchClient = "";
        string SQLBatchServer = "";
        private List<MetaObject> ClientFileObjects;
        private List<MetaObject> ServerFileObjects;

        private TList<ObjectAssociation> ClientFileAssociations;
        private TList<ObjectAssociation> ServerFileAssociations;

        private List<MetaObject> ServerNoOccurencesFound;

        private bool BlobsUpdated = false;
        private void Execute()
        {
            BlobsUpdated = false;
            LogDisplayer log = new LogDisplayer();
            log.ResetDesigner();
            log.StartPosition = FormStartPosition.CenterScreen;

            OpenAfterExecuteItems = new List<VersionControlledItem>();
            SQLBatchClient = "";
            SQLBatchServer = "";

            UpdateStatusLabel("Retrieving Actions");
            //execute user chosen actions
            ItemAndAction = new Dictionary<VersionControlledItem, RepositoryAction>();
            foreach (Row r in tableModel1.Rows)
            {
                if (!r.Enabled)
                    continue; //skip all embedded items

                VersionControlledItem item = r.Tag as VersionControlledItem;
                if (r.Cells[6].Tag is RepositoryAction)
                {
                    RepositoryAction action = r.Cells[6].Tag as RepositoryAction;
                    ItemAndAction.Add(item, action);
                }
            }
            UpdateStatusLabel("Embedding Objects");
            UpdateProgressTotal(ItemAndAction.Count);
            int progress = 0;
            int totalitems = ItemAndAction.Count;
            foreach (KeyValuePair<VersionControlledItem, RepositoryAction> kvp in ItemAndAction)
            {
                kvp.Key.LoadEmbedded();
                kvp.Key.EmbeddedObjectActions.Clear();
                foreach (VersionControlledItem embedded in kvp.Key.EmbeddedObjects)
                {
                    totalitems++;
                    kvp.Key.EmbeddedObjectActions.Add(SynchronisationRules.GetAvailableActionForEmbeddedItem(embedded, kvp.Value));
                }
                UpdateProgressValue(progress++);
            }
            UpdateStatusLabel("Compiling");
            UpdateProgressTotal(totalitems);
            UpdateProgressValue(0);

            progress = 0;
            string dots = "";

            SQLBatchClient = "DECLARE @GRAPHFILEID int;" + Environment.NewLine;
            SQLBatchClient += "SET @GRAPHFILEID = -1;" + Environment.NewLine;
            SQLBatchServer = "DECLARE @GRAPHFILEID int;" + Environment.NewLine;
            SQLBatchServer += "SET @GRAPHFILEID = -1;" + Environment.NewLine;

            Dictionary<string, GraphFile> blobsToUpdate = new Dictionary<string, GraphFile>();
            foreach (KeyValuePair<VersionControlledItem, RepositoryAction> kvp in ItemAndAction)
            {
                if (kvp.Key.CancelExecution)
                {
                    SQLBatchClient = "";
                    SQLBatchServer = "";

                    log.AddMessage(kvp.Key.Description + " action cancelled.");
                    log.AddMessage(kvp.Key.CancelExecutionReason);

                    OpenAfterExecuteItems.Clear();
                    blobsToUpdate.Clear();

                    ResetFeedbackControls();

                    break;
                }

                ClientFileObjects = new List<MetaObject>();
                ServerFileObjects = new List<MetaObject>();

                ClientFileAssociations = new TList<ObjectAssociation>();
                ServerFileAssociations = new TList<ObjectAssociation>();

                ServerNoOccurencesFound = new List<MetaObject>();

                if (dots.Length == 5)
                    dots = "";
                dots += ".";
                UpdateStatusLabel("Compiling " + dots);
                int embeddedItemIndex = 0;
                foreach (VersionControlledItem embedded in kvp.Key.EmbeddedObjects)
                {
                    if (embedded.CancelExecution)
                    {
                        SQLBatchClient = "";
                        SQLBatchServer = "";
                        log.AddMessage(kvp.Key.Description);
                        log.AddMessage(embedded.Description + " action cancelled.");
                        log.AddMessage(embedded.CancelExecutionReason);

                        OpenAfterExecuteItems.Clear();
                        blobsToUpdate.Clear();

                        ResetFeedbackControls();

                        break;
                    }

                    ExecuteItemAction(embedded, kvp.Key.EmbeddedObjectActions[embeddedItemIndex], log);
                    UpdateProgressValue(progress++);
                    embeddedItemIndex++;
                }

                //remember to update blob after execution
                if (kvp.Value.OpenAfterExecute && kvp.Key.ReturnItemToUse() is GraphFile && kvp.Value.Caption != "Open")
                    blobsToUpdate.Add((kvp.Value.Provider == Core.Variables.Instance.ServerProvider) ? Core.Variables.Instance.ClientProvider : Core.Variables.Instance.ServerProvider, kvp.Key.ReturnItemToUse() as GraphFile);
                else if (kvp.Key.ReturnItemToUse() is GraphFile && kvp.Value.Caption == "Check In")
                    blobsToUpdate.Add((kvp.Value.Provider == Core.Variables.Instance.ServerProvider) ? Core.Variables.Instance.ClientProvider : Core.Variables.Instance.ServerProvider, kvp.Key.ReturnItemToUse() as GraphFile);
                else
                {
                    kvp.Value.ToString();
                    kvp.Key.ToString();
                }

                ExecuteItemAction(kvp.Key, kvp.Value, log);
                UpdateProgressValue(progress++);
                log.AddMessage("-");

                //add fileObject bindings
                SQLBatchClient += "IF @GRAPHFILEID > -1 BEGIN;" + Environment.NewLine;
                SQLBatchClient += "IF NULL = NULL SELECT NULL;" + Environment.NewLine;
                foreach (MetaObject clientObj in ClientFileObjects)
                {
                    if (clientObj == null)
                        continue;
                    SQLBatchClient += "INSERT INTO GraphFileObject (MetaObjectID, MachineID, GraphFileID, GraphFileMachine) VALUES (" + clientObj.pkid + ",'" + clientObj.Machine + "',@GRAPHFILEID,'" + Environment.MachineName + "');" + Environment.NewLine;
                }
                SQLBatchClient += "END;" + Environment.NewLine;

                SQLBatchServer += "IF @GRAPHFILEID > -1 BEGIN;" + Environment.NewLine;
                SQLBatchServer += "IF NULL = NULL SELECT NULL;" + Environment.NewLine;
                foreach (MetaObject serverObj in ServerFileObjects)
                {
                    if (serverObj == null)
                        continue;
                    SQLBatchServer += "INSERT INTO GraphFileObject (MetaObjectID, MachineID, GraphFileID, GraphFileMachine) VALUES (" + serverObj.pkid + ",'" + serverObj.Machine + "',@GRAPHFILEID,'" + Environment.MachineName + "');" + Environment.NewLine;
                }
                SQLBatchServer += "END;" + Environment.NewLine;

                //if (kvp.Key.clientFileObjects != null && kvp.Key.clientFileObjects.Count > 0)
                //{
                //    SQLBatchServer += "IF @GRAPHFILEID > -1 BEGIN;" + Environment.NewLine;
                //    SQLBatchServer += "IF NULL = NULL SELECT NULL;" + Environment.NewLine;
                //    foreach (GraphFileObject gfo in kvp.Key.clientFileObjects)
                //    {
                //        SQLBatchServer += "INSERT INTO GraphFileObject (MetaObjectID, MachineID, GraphFileID, GraphFileMachine) VALUES (" + gfo.MetaObjectID + ",'" + gfo.MachineID + "',@GRAPHFILEID,'" + gfo.MachineID + "');" + Environment.NewLine;
                //    }
                //    SQLBatchServer += "END;" + Environment.NewLine;
                //}
                //if (kvp.Key.serverFileObjects != null && kvp.Key.serverFileObjects.Count > 0)
                //{
                //    SQLBatchClient += "IF @GRAPHFILEID > -1 BEGIN;" + Environment.NewLine;
                //    SQLBatchClient += "IF NULL = NULL SELECT NULL;" + Environment.NewLine;
                //    foreach (GraphFileObject gfo in kvp.Key.serverFileObjects)
                //    {
                //        SQLBatchClient += "INSERT INTO GraphFileObject (MetaObjectID, MachineID, GraphFileID, GraphFileMachine) VALUES (" + gfo.MetaObjectID + ",'" + gfo.MachineID + "',@GRAPHFILEID,'" + gfo.MachineID + "');" + Environment.NewLine;
                //    }
                //    SQLBatchClient += "END;" + Environment.NewLine;
                //}

                foreach (ObjectAssociation association in ClientFileAssociations)
                {
                    //add association 
                    SQLBatchClient += "IF NOT EXISTS (SELECT CAid FROM ObjectAssociation WHERE CAid = " + association.CAid + " AND ObjectID = " + association.ObjectID + " AND ObjectMachine= '" + association.ObjectMachine + "' AND ChildObjectID = " + association.ChildObjectID + " AND ChildObjectMachine= '" + association.ChildObjectMachine + "') BEGIN;" + Environment.NewLine;
                    SQLBatchClient += "EXECUTE [dbo].[PROC_ObjectAssociation_Insert] " + association.CAid + "," + association.ObjectID + "," + association.ChildObjectID + ",1,'" + association.ObjectMachine + "','" + association.ChildObjectMachine + "',7,'',NULL;" + Environment.NewLine;
                    SQLBatchClient += "END;" + Environment.NewLine;
                    //SQLBatchClient += "ELSE BEGIN;" + Environment.NewLine;
                    //SQLBatchClient += "" + Environment.NewLine;
                    //SQLBatchClient += "END;" + Environment.NewLine;

                    //bind to file
                    SQLBatchClient += "IF @GRAPHFILEID > -1 BEGIN;" + Environment.NewLine;
                    SQLBatchClient += "EXECUTE [dbo].[PROC_GraphFileAssociation_Insert] @GRAPHFILEID,'" + Environment.MachineName + "','" + association.ChildObjectMachine + "'," + association.CAid + "," + association.ObjectID + "," + association.ChildObjectID + ",'" + association.ObjectMachine + "';" + Environment.NewLine;
                    SQLBatchClient += "END;" + Environment.NewLine;
                }

                foreach (ObjectAssociation association in ServerFileAssociations)
                {
                    //add association
                    SQLBatchServer += "IF NOT EXISTS (SELECT CAid FROM ObjectAssociation WHERE CAid = " + association.CAid + " AND ObjectID = " + association.ObjectID + " AND ObjectMachine= '" + association.ObjectMachine + "' AND ChildObjectID = " + association.ChildObjectID + " AND ChildObjectMachine= '" + association.ChildObjectMachine + "') BEGIN;" + Environment.NewLine;
                    SQLBatchServer += "EXECUTE [dbo].[PROC_ObjectAssociation_Insert] " + association.CAid + "," + association.ObjectID + "," + association.ChildObjectID + ",1,'" + association.ObjectMachine + "','" + association.ChildObjectMachine + "',7,'',NULL;" + Environment.NewLine;
                    SQLBatchServer += "END;" + Environment.NewLine;
                    //SQLBatchServer += "ELSE BEGIN;" + Environment.NewLine;
                    //SQLBatchServer += "" + Environment.NewLine;
                    //SQLBatchServer += "END;" + Environment.NewLine;
                    //bind to file
                    SQLBatchServer += "IF @GRAPHFILEID > -1 BEGIN;" + Environment.NewLine;
                    SQLBatchServer += "EXECUTE [dbo].[PROC_GraphFileAssociation_Insert] @GRAPHFILEID,'" + Environment.MachineName + "','" + association.ChildObjectMachine + "'," + association.CAid + "," + association.ObjectID + "," + association.ChildObjectID + ",'" + association.ObjectMachine + "';" + Environment.NewLine;
                    SQLBatchServer += "END;" + Environment.NewLine;
                }

                foreach (MetaObject o in ServerNoOccurencesFound) //not found on the diagram they were supposed to be part of
                {
                    if ((int)DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.ExecuteScalar(CommandType.Text, "SELECT COUNT(dbo.GraphFile.pkid) FROM dbo.GraphFile INNER JOIN dbo.GraphFileObject ON dbo.GraphFile.pkid = dbo.GraphFileObject.GraphFileID AND dbo.GraphFile.Machine = dbo.GraphFileObject.GraphFileMachine WHERE dbo.GraphFile.IsActive=1 AND dbo.GraphFileObject.MetaObjectID = " + o.pkid + " AND dbo.GraphFileObject.Machine = '" + o.Machine + "'") == 0)
                    {
                        //delete this object and all its associations
                        SQLBatchServer += "DELETE FROM GraphFileAssociation WHERE ObjectID = " + o.pkid + " AND ObjectMachine = '" + o.Machine + "';" + Environment.NewLine;
                        SQLBatchServer += "DELETE FROM GraphFileAssociation WHERE ChildObjectID = " + o.pkid + " AND ChildObjectMachine = '" + o.Machine + "';" + Environment.NewLine;
                        SQLBatchServer += "DELETE FROM Artifact WHERE ObjectID = " + o.pkid + " AND ObjectMachine = Machine;" + Environment.NewLine;
                        SQLBatchServer += "DELETE FROM Artifact WHERE ChildObjectID = " + o.pkid + " AND ChildObjectMachine = '" + o.Machine + "';" + Environment.NewLine;
                        SQLBatchServer += "DELETE FROM Artifact WHERE ArtifactObjectID = " + o.pkid + " AND ArtefactMachine = '" + o.Machine + "';" + Environment.NewLine;
                        SQLBatchServer += "DELETE FROM ObjectAssociation WHERE ObjectID = " + o.pkid + " AND ObjectMachine = '" + o.Machine + "';" + Environment.NewLine;
                        SQLBatchServer += "DELETE FROM ObjectAssociation WHERE ChildObjectID = " + o.pkid + " AND ChildObjectMachine = '" + o.Machine + "';" + Environment.NewLine;
                        SQLBatchServer += "DELETE FROM ObjectFieldValue WHERE ObjectID = " + o.pkid + " AND ObjectMachine = '" + o.Machine + "';" + Environment.NewLine;
                        SQLBatchServer += "DELETE FROM MetaObject WHERE ChildObjectID = " + o.pkid + " AND ChildObjectMachine = '" + o.Machine + "';" + Environment.NewLine;
                    }
                }

                //Reset variables for next file
                SQLBatchClient += "SET @GRAPHFILEID = -1;" + Environment.NewLine;
                SQLBatchServer += "SET @GRAPHFILEID = -1;" + Environment.NewLine;
            }

            UpdateStatusLabel("Executing Batch");
            ExecuteDatabase();

            UpdateStatusLabel("Uploading File Images");
            //add blobs for affected files to opposite(the provider is added to the dictionary as the opposite already) of items retrieve provider
            //the file is the retrived file from the original provider
            foreach (KeyValuePair<string, GraphFile> kvp in blobsToUpdate)
            {
                string connstring = kvp.Key == Core.Variables.Instance.ServerProvider ? Core.Variables.Instance.ServerConnectionString : Core.Variables.Instance.ConnectionString;
                SqlConnection conn = new SqlConnection(connstring);
                using (conn)
                {
                    conn.Open();
                    int newFilePkid = -1;
                    SqlCommand findPkid = new SqlCommand("SELECT TOP 1 pkid FROM GraphFile WHERE OriginalFileUniqueID = '" + kvp.Value.OriginalFileUniqueID + "' AND IsActive = 1", conn);
                    object pkidOBJ = findPkid.ExecuteScalar();
                    if (pkidOBJ != null)
                        newFilePkid = int.Parse(pkidOBJ.ToString());
                    if (newFilePkid > -1)
                    {
                        SqlCommand addBLOBL = new SqlCommand("UPDATE GRAPHFILE SET BLOB = @Blob WHERE PKID=@PKID", conn);
                        addBLOBL.Parameters.Add("@Blob", SqlDbType.Image, kvp.Value.Blob.Length).Value = kvp.Value.Blob;
                        addBLOBL.Parameters.Add("@PKID", SqlDbType.Int).Value = newFilePkid;
                        addBLOBL.ExecuteNonQuery();
                    }
                }
            }
            BlobsUpdated = true;

            UpdateStatusLabel("Finalizing");
            InitRepositoryThread();

            UpdateStatusLabel("Showing Results");
            //display log
            if (ItemAndAction.Count > 0)
                log.ShowDialog();

            ItemAndAction.Clear();

            ResetFeedbackControls();
        }
        private void ExecuteItemAction(VersionControlledItem item, RepositoryAction action, LogDisplayer log)
        {
            if (action == null)
            {
                log.AddMessage("Client " + item.Description + " [" + item.ClientState + "]-->[missing rule]");
                log.AddMessage("Server " + item.Description + " [" + item.ServerState + "]-->[missing rule]");
                return;
            }
            //run client
            ////Log--
            if (action.TargetClientState != VCStatusList.Ignore) //for when we checkin but an object is not checked out
            {
                log.AddMessage("Client " + item.Description + " [" + item.ClientState + "]-->[" + action.TargetClientState + "]");

                if (item.ItemType.ToString().Contains("GraphFile"))
                {
                    bool compiled = false;
                    if (action.TargetClientState == VCStatusList.CheckedOut || action.TargetClientState == VCStatusList.CheckedOutRead)
                    {
                        //deactive all files
                        SQLBatchClient += "UPDATE GraphFile SET IsActive = 0 WHERE OriginalFileUniqueID = '" + item.ID + "';" + Environment.NewLine;
                        //add new file (FROM SERVER TO CLIENT)
                        GraphFile file = (item.ServerItem as GraphFile);
                        ClientFileAssociations = DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.ObjectAssociationProvider.GetByGraphFileIDGraphFileMachineFromGraphFileAssociation(file.pkid, file.Machine);

                        SQLBatchClient += "EXECUTE @GRAPHFILEID = PROC_GraphFile_Insert 0," + file.MajorVersion + "," + file.MinorVersion + ",'" + DateTime.Now + "','" + action.TargetClientState.ToString() + "',1,0,'" + FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).FileVersion.ToString() + "','" + file.Blob.ToString() + "','" + file.WorkspaceName + "'," + file.FileTypeID + "," + file.pkid + ",'" + file.Name + "'," + (int)action.TargetClientState + ",'" + Core.strings.GetVCIdentifier() + "','" + Environment.MachineName + "'," + file.WorkspaceTypeId + ",'" + file.OriginalFileUniqueID + "';" + Environment.NewLine;
                        SQLBatchClient += "SET @GRAPHFILEID = (SELECT TOP 1 pkid FROM GraphFile WHERE OriginalFileUniqueID = '" + file.OriginalFileUniqueID + "' AND IsActive = 1);" + Environment.NewLine;
                        compiled = true;
                    }

                    if (!compiled)
                    {
                        if (action.ChangeUser)
                            SQLBatchClient += "UPDATE GraphFile SET VCStatusID = " + (int)action.TargetClientState + ", VCMachineID = '" + Core.strings.GetVCIdentifier() + "' WHERE OriginalFileUniqueID = '" + item.ID + "' AND IsActive = 1;" + Environment.NewLine;
                        else
                            SQLBatchClient += "UPDATE GraphFile SET VCStatusID = " + (int)action.TargetClientState + " WHERE OriginalFileUniqueID = '" + item.ID + "' AND IsActive = 1;" + Environment.NewLine;
                    }
                }
                else if (item.ItemType.ToString().Contains("MetaObject"))
                {
                    System.Threading.Thread.Sleep(20);
                    int id = int.Parse(item.ID.Split('|')[0].ToString());
                    string machine = item.ID.Split('|')[1].ToString();
                    //if it exists
                    MetaObject obj = item.ReturnItemToUse() as MetaObject;
                    if (action.TargetClientState != VCStatusList.Ignore)
                    {
                        SQLBatchClient += "IF NOT EXISTS(SELECT TOP 1 pkid FROM MetaObject WHERE pkid = " + obj.pkid + " AND Machine = '" + obj.Machine + "') BEGIN;" + Environment.NewLine;
                        SQLBatchClient += "SET IDENTITY_INSERT dbo.MetaObject ON;" + Environment.NewLine;
                        SQLBatchClient += "INSERT INTO MetaObject (pkid,[Class],[WorkspaceName],[UserID],[Machine],[VCStatusID],[VCMachineID],[WorkspaceTypeID],[DateCreated],[LastModified]) VALUES (" + obj.pkid + ",'" + obj.Class + "','" + obj.WorkspaceName + "','" + Core.Variables.Instance.UserID + "','" + obj.Machine + "'," + (int)action.TargetClientState + ",'" + Core.strings.GetVCIdentifier() + "'," + obj.WorkspaceTypeId + ",'" + obj.DateCreated + "','" + DateTime.Now + "');" + Environment.NewLine;
                        SQLBatchClient += "SET IDENTITY_INSERT dbo.MetaObject OFF;" + Environment.NewLine;
                        SQLBatchClient += "END;" + Environment.NewLine;

                        SQLBatchClient += "ELSE BEGIN;" + Environment.NewLine;
                        if (action.ChangeUser)
                            SQLBatchClient += "UPDATE MetaObject SET VCStatusID = " + (int)action.TargetClientState + ", VCMachineID = '" + Core.strings.GetVCIdentifier() + "', LastModified = '" + DateTime.Now + "' WHERE pkid = " + id + " AND Machine = '" + machine + "';" + Environment.NewLine;
                        else
                            SQLBatchClient += "UPDATE MetaObject SET VCStatusID = " + (int)action.TargetClientState + ", VCMachineID = '" + item.User + "', LastModified = '" + DateTime.Now + "' WHERE pkid = " + id + " AND Machine = '" + machine + "';" + Environment.NewLine;
                        SQLBatchClient += "END;" + Environment.NewLine;
                    }

                    if (action.TargetClientState == VCStatusList.CheckedOut || action.TargetClientState == VCStatusList.CheckedOutRead)
                    {
                        //if (action.ClientState != VCStatusList.CheckedOut)
                        {
                            SQLBatchClient += "DELETE FROM ObjectFieldValue WHERE ObjectID = " + id + " AND MachineID = '" + machine + "';" + Environment.NewLine;
                            foreach (ObjectFieldValue value in item.Values)
                            {
                                if (value.ValueString.Length > 0 || (value.ValueRTF != null && value.ValueRTF.Length > 0) || (value.ValueLongText != null && value.ValueLongText.Length > 0))
                                    SQLBatchClient += "INSERT INTO ObjectFieldValue (ObjectID,MachineID,FieldID,ValueString,ValueRTF,ValueLongText) VALUES (" + id + ",'" + machine + "','" + value.FieldID + "','" + value.ValueString + "','" + value.ValueRTF + "','" + value.ValueLongText + "');" + Environment.NewLine;
                            }
                        }
                    }

                    ClientFileObjects.Add(item.ServerItem as MetaObject);
                }
                else
                {
                    item.ToString();
                }
            }
            System.Threading.Thread.Sleep(20);
            //run server
            ////Log--
            if (action.TargetServerState != VCStatusList.Ignore)
            {
                log.AddMessage("Server " + item.Description + " [" + item.ServerState + "]-->[" + action.TargetServerState + "]");

                if (item.ItemType.ToString().Contains("GraphFile"))
                {
                    bool compiled = false;
                    if (action.TargetServerState == VCStatusList.CheckedIn)
                    {
                        if (action.ServerState != VCStatusList.Locked && action.Caption != "Force Check In")
                        {
                            if (item.ServerItem != null) //we can create a new diagram and check it in, then serveritem will be null
                            {
                                //diff graphfileobjects between items
                                //this only has to happen when we checkin from client
                                //if an object is missing and is not related(what about associations?), delete it.
                                TList<GraphFileObject> fileObjectsOnClient = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileObjectProvider.GetByGraphFileIDGraphFileMachine((item.ClientItem as GraphFile).pkid, (item.ClientItem as GraphFile).Machine);
                                foreach (MetaObject o in DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.MetaObjectProvider.GetByGraphFileIDGraphFileMachineFromGraphFileObject((item.ServerItem as GraphFile).pkid, (item.ServerItem as GraphFile).Machine))
                                {
                                    bool found = false;
                                    foreach (GraphFileObject obj in fileObjectsOnClient)
                                    {
                                        if (o.pkid == obj.MetaObjectID && o.Machine == obj.MachineID)
                                        {
                                            found = true;
                                            break;
                                        }
                                    }
                                    if (!found)
                                        ServerNoOccurencesFound.Add(o);
                                }
                            }
                            //deactive all files
                            SQLBatchServer += "UPDATE GraphFile SET IsActive = 0 WHERE OriginalFileUniqueID = '" + item.ID + "';" + Environment.NewLine;
                            //add new file (FROM CLIENT TO SERVER)
                            int newMajorVersion = (item.ClientItem as GraphFile).MajorVersion += 1;
                            GraphFile file = (item.ClientItem as GraphFile);
                            ServerFileAssociations = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByGraphFileIDGraphFileMachineFromGraphFileAssociation(file.pkid, file.Machine);

                            SQLBatchServer += "EXECUTE @GRAPHFILEID = PROC_GraphFile_Insert 0," + newMajorVersion + ",0,'" + DateTime.Now + "','" + action.TargetServerState.ToString() + "',1,0,'" + FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).FileVersion.ToString() + "','" + file.Blob.ToString() + "','" + file.WorkspaceName + "'," + file.FileTypeID + "," + file.pkid + ",'" + file.Name + "'," + (int)action.TargetClientState + ",'" + Core.strings.GetVCIdentifier() + "','" + Environment.MachineName + "'," + file.WorkspaceTypeId + ",'" + file.OriginalFileUniqueID + "';" + Environment.NewLine;
                            SQLBatchServer += "SET @GRAPHFILEID = (SELECT TOP 1 pkid FROM GraphFile WHERE OriginalFileUniqueID = '" + file.OriginalFileUniqueID + "' AND IsActive = 1);" + Environment.NewLine;
                            //SQLBatchServer += "SET @GRAPHFILEID = @@IDENTITY;" + Environment.NewLine;
                            compiled = true;
                        }
                    }
                    if (!compiled)
                    {
                        if (action.ChangeUser)
                            SQLBatchServer += "UPDATE GraphFile SET VCStatusID = " + (int)action.TargetServerState + ", VCMachineID = '" + Core.strings.GetVCIdentifier() + "' WHERE OriginalFileUniqueID = '" + item.ID + "' AND IsActive = 1;" + Environment.NewLine;
                        else
                            SQLBatchServer += "UPDATE GraphFile SET VCStatusID = " + (int)action.TargetServerState + " WHERE OriginalFileUniqueID = '" + item.ID + "' AND IsActive = 1;" + Environment.NewLine;
                    }
                }
                else if (item.ItemType.ToString().Contains("MetaObject"))
                {
                    System.Threading.Thread.Sleep(20);
                    int id = int.Parse(item.ID.Split('|')[0].ToString());
                    string machine = item.ID.Split('|')[1].ToString();
                    MetaObject obj = item.ReturnItemToUse() as MetaObject;
                    //if it exists
                    if (action.TargetServerState != VCStatusList.Ignore)
                    {
                        SQLBatchServer += "IF NOT EXISTS(SELECT TOP 1 pkid FROM MetaObject WHERE pkid = " + obj.pkid + " AND Machine = '" + obj.Machine + "') BEGIN;" + Environment.NewLine;
                        SQLBatchServer += "SET IDENTITY_INSERT dbo.MetaObject ON;" + Environment.NewLine;
                        SQLBatchServer += "INSERT INTO MetaObject ([pkid],[Class],[WorkspaceName],[UserID],[Machine],[VCStatusID],[VCMachineID],[WorkspaceTypeID],[DateCreated],[LastModified]) VALUES (" + obj.pkid + ",'" + obj.Class + "','" + obj.WorkspaceName + "','" + MetaBase.GetServerUserID() + "','" + obj.Machine + "'," + (int)action.TargetServerState + ",'" + Core.strings.GetVCIdentifier() + "'," + obj.WorkspaceTypeId + ",'" + obj.DateCreated + "','" + DateTime.Now + "');" + Environment.NewLine;
                        SQLBatchServer += "SET IDENTITY_INSERT dbo.MetaObject OFF;" + Environment.NewLine;
                        SQLBatchServer += "END;" + Environment.NewLine;

                        SQLBatchServer += "ELSE BEGIN;" + Environment.NewLine;
                        if (action.ChangeUser)
                            SQLBatchServer += "UPDATE MetaObject SET VCStatusID = " + (int)action.TargetServerState + ", VCMachineID = '" + Core.strings.GetVCIdentifier() + "', LastModified = '" + DateTime.Now + "' WHERE pkid = " + id + " AND Machine = '" + machine + "';" + Environment.NewLine;
                        else
                            SQLBatchServer += "UPDATE MetaObject SET VCStatusID = " + (int)action.TargetServerState + ", VCMachineID = '" + item.User + "', LastModified = '" + DateTime.Now + "' WHERE pkid = " + id + " AND Machine = '" + machine + "';" + Environment.NewLine;
                        SQLBatchServer += "END;" + Environment.NewLine;
                    }

                    if (action.TargetServerState == VCStatusList.CheckedIn)
                    {
                        if (action.ServerState != VCStatusList.Locked && action.Caption != "Force Check In")
                        {
                            SQLBatchServer += "DELETE FROM ObjectFieldValue WHERE ObjectID = " + id + " AND MachineID = '" + machine + "';" + Environment.NewLine;
                            foreach (ObjectFieldValue value in item.Values)
                            {
                                if (value.ValueString.Length > 0 || (value.ValueRTF != null && value.ValueRTF.Length > 0) || (value.ValueLongText != null && value.ValueLongText.Length > 0))
                                    SQLBatchServer += "INSERT INTO ObjectFieldValue (ObjectID,MachineID,FieldID,ValueString,ValueRTF,ValueLongText) VALUES (" + id + ",'" + machine + "','" + value.FieldID + "','" + value.ValueString + "','" + value.ValueRTF + "','" + value.ValueLongText + "');" + Environment.NewLine;
                            }
                        }
                    }
                    ServerFileObjects.Add(item.ClientItem as MetaObject);
                }
                else
                {
                    item.ToString();
                }
            }
            System.Threading.Thread.Sleep(20);

            if (action.Caption == "Open" && item.ItemType.ToString().Contains("MetaObject"))
            {
                //we dont want these item to add to the log when they 'open'
            }
            else
            {
                log.AddMessage(action.Provider + " " + item.Description + " [" + action.Number + "] " + action.Caption);
            }

            if (item.EmbeddedObjects.Count > 0) //clear the embedded rules on the item
                item.EmbeddedObjectActions.Clear();

            if (item.ItemType.ToString().Contains("GraphFile"))
                if (action.OpenAfterExecute)
                    OpenAfterExecuteItems.Add(item);
        }

        private void ExecuteDatabase()
        {
            if (SQLBatchServer.Length > 0)
                try
                {
                    SqlCommand command = new SqlCommand();
                    SqlConnection conn = new SqlConnection(Core.Variables.Instance.ServerConnectionString);
                    command.CommandType = CommandType.Text;
                    command.Connection = conn;
                    command.CommandText = SQLBatchServer.Replace("\r\n", "");
                    command.CommandTimeout = 0;
                    using (conn)
                    {
                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Core.Log.WriteLog("Server Batch Error, preventing client." + Environment.NewLine + ex.ToString() + Environment.NewLine + SQLBatchServer.Replace("\r\n", ""));
                }

            System.Threading.Thread.Sleep(20);

            if (SQLBatchClient.Length > 0)
                try
                {
                    SqlCommand command = new SqlCommand();
                    SqlConnection conn = new SqlConnection(Core.Variables.Instance.ConnectionString);
                    command.CommandType = CommandType.Text;
                    command.Connection = conn;
                    command.CommandText = SQLBatchClient.Replace("\r\n", "");
                    command.CommandTimeout = 0;
                    using (conn)
                    {
                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Core.Log.WriteLog(ex.ToString() + Environment.NewLine + SQLBatchClient.Replace("\r\n", ""));
                }
        }

        //string blobString = Convert.ToBase64String(file.Blob);
        //byte[] blobByte = Convert.FromBase64String(blobString);
        //bool x = blobByte == (item.ServerItem as GraphFile).Blob;

    }
}