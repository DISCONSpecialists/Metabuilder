using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.BusinessFacade.Storage.RepositoryTemp;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Meta;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using b = MetaBuilder.BusinessLogic;
using f = MetaBuilder.BusinessFacade.Storage.RepositoryTemp;
using d = MetaBuilder.DataAccessLayer;
using ObjectAssociation = MetaBuilder.BusinessLogic.ObjectAssociation;

namespace MetaBuilder.UIControls.GraphingUI
{
    public partial class SynchronisationPanel : UserControl
    {

        #region Fields (14)

        private bool allowTypeFilter;
        private Repository client;
        private bool isDirty;
        private bool isLocal;
        private List<IRepositoryItem> items;
        private LogDisplayer logDisplayer;
        private DataGridViewCellStyle noAccessStyle;
        // USED FOR OBJECTS ONLY!
        private List<PermissionList> permissions;
        private IPermissionService permissionService;
        private IRepositoryService repositoryService;
        private Repository server;
        private bool showOpenCheckbox;
        private bool suspendDataChanges;
        private DataTable table;
        private WorkspaceKey currentWorkspace;
        public WorkspaceKey CurrentWorkspace
        {
            get { return currentWorkspace; }
            set { currentWorkspace = value; }
        }

        #endregion Fields

        #region Constructors (1)

        public SynchronisationPanel()
        {
            InitializeComponent();
            dataGridView1.CellMouseEnter += new DataGridViewCellEventHandler(dataGridView1_CellMouseEnter);
            dataGridView1.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dataGridView1_EditingControlShowing);

            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(dataGridView1_CellEndEdit);
            dataGridView1.CellValidating += new DataGridViewCellValidatingEventHandler(dataGridView1_CellValidating);
            dataGridView1.DataError += new DataGridViewDataErrorEventHandler(dataGridView1_DataError);
            dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);
        }

        #endregion Constructors

        #region Properties (6)

        public bool AllowTypeFilter
        {
            get { return allowTypeFilter; }
            set { allowTypeFilter = value; }
        }

        public bool IsDirty
        {
            get { return isDirty; }
            set { isDirty = value; }
        }

        public bool IsLocal
        {
            get { return isLocal; }
            set { isLocal = value; }
        }

        public List<IRepositoryItem> Items
        {
            get { return items; }
            set { items = value; }
        }

        public List<PermissionList> Permissions
        {
            get { return permissions; }
            set { permissions = value; }
        }

        public bool ShowOpenCheckBox
        {
            get { return showOpenCheckbox; }
            set { showOpenCheckbox = value; }
        }

        #endregion Properties

        #region Delegates and Events (1)

        // Events (1) 

        public event EventHandler ActionsPerformed;

        #endregion Delegates and Events

        #region Methods (31)

        // Public Methods (4) 

        public void CheckforDirty()
        {
            if (IsDirty)
            {
                /*DialogResult res =
                    MessageBox.Show(this,"Do you want to commit the current operation?", "Commit Changes",
                                    MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                switch (res)
                {
                    case DialogResult.Yes:
                        PerformActions();
                        break;
                    case DialogResult.No:
                        // do nothing
                        break;
                }*/
            }
        }

        public string GetTypeNameOnly(Type t)
        {
            return t.Name.Substring(t.Name.LastIndexOf('.'), t.Name.Length - t.Name.LastIndexOf('.'));
        }

        public void RefreshItems()
        {
            suspendDataChanges = true;
            BuildDataSet();

            dataGridView1.DataSource = table.DefaultView;
            dataGridView1.Refresh();
            suspendDataChanges = false;
            txtType.Text = "";
            txtType.Enabled = allowTypeFilter;
            txtValue.Text = "";
            comboStatus.Items.Clear();
            comboStatus.Items.Add(VCStatusList.CheckedIn);
            comboStatus.Items.Add(VCStatusList.CheckedOut);
            comboStatus.Items.Add(VCStatusList.None);
            //comboStatus.Items.Add(VCStatusList.PartiallyCheckedIn);
            comboStatus.Items.Add(VCStatusList.CheckedOutRead);
            comboStatus.Items.Add(VCStatusList.Locked);
            //comboStatus.Items.Add(VCStatusList.Obsolete);
            comboStatus.Items.Add(VCStatusList.MarkedForDelete);
            //comboStatus.Items.Add(VCStatusList.PCI_Revoked);
            comboStatus.Text = "";
        }

        public void ResetTable()
        {
            if (table != null)
            {
                table.Clear();
                dataGridView1.DataSource = table.DefaultView;
                dataGridView1.Refresh();
            }
        }

        public void SetupGridDropdowns(bool forcheckout)
        {
            if (forcheckout)
            {
                Actions.Items.AddRange(new object[]
                                           {
                                               "Check Out in Write Mode",
                                               "Check Out in Read-Only Mode"
                                           });
                Actions.Name = "Actions";
            }
            else
            {
                Actions.Items.AddRange(new object[]
                                           {
                                               "Check In"
                                           });
                Actions.Name = "Actions";
            }
        }

        // Protected Methods (1) 

        protected void OnActionsPerformed(object sender, EventArgs e)
        {
            if (ActionsPerformed != null)
                ActionsPerformed(sender, e);
        }

        // Private Methods (26) 

        private void btnApplyFilter_Click(object sender, EventArgs e)
        {
            if (table != null)
            {
                table.DefaultView.RowFilter = "1 = 1 ";
                if (txtType.Text.Length > 0)
                    table.DefaultView.RowFilter += "AND ItemType like '%" + txtType.Text + "%'";
                if (txtValue.Text.Length > 0)
                    table.DefaultView.RowFilter += "AND Item like '%" + txtValue.Text + "%'";
                if (comboStatus.Text.Length > 0)
                    table.DefaultView.RowFilter += "AND Status= '" + comboStatus.Text + "'";
                dataGridView1.Refresh();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ParentForm.DialogResult = DialogResult.Cancel;
            ParentForm.Close();
        }

        private void btnPerformActions_Click(object sender, EventArgs e)
        {
            //Loader.FlushDataViews();
            //try
            //{
            btnPerformActions.Enabled = false;
            btnCancel.Enabled = false;
            PerformActions();
            RefreshItems();
            btnPerformActions.Enabled = true;
            btnCancel.Enabled = true;
            //}
            //catch(Exception x)
            //{
            //    MessageBox.Show(this,"An error has occurred while performing the action. It has been logged. Please refer to your server administrator");
            //    LogEntry entry = new LogEntry();
            //    entry.Message = x.ToString();
            //    Logger.Write(entry);
            //    RefreshItems();
            //    btnPerformActions.Enabled = true;
            //    btnCancel.Enabled = true;
            //}
        }

        private void BuildDataSet()
        {
            bool useServer = isLocal ? false : true;
            string provider = useServer ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider;
            int itemIndex = 0;
            bool o = false;
            bool a = false;

            Loader.FlushDataViews();

            table = new DataTable();
            table.Columns.Add(new DataColumn("ID", typeof(int)));
            table.Columns.Add(new DataColumn("Machine", typeof(string)));
            table.Columns.Add(new DataColumn("Item", typeof(string)));
            table.Columns.Add(new DataColumn("ItemType", typeof(string)));
            table.Columns.Add(new DataColumn("Status", typeof(string)));
            table.Columns.Add(new DataColumn("User", typeof(string)));

            if (Items.Count > 0 && Items[0] is GraphFile)
            {
                table.Columns.Add(new DataColumn("Modified Date", typeof(DateTime)));
                table.Columns["Modified Date"].ReadOnly = true;
            }

            table.AcceptChanges();

            this.Identifier.Width = 300;
            this.User.Visible = true;
            this.Modified.Visible = false;
            this.Status.Visible = true;
            this.Actions.Visible = true;
            this.btnPerformActions.Visible = true;
            this.btnCancel.Visible = true;
            this.Type.Visible = false;

            foreach (IRepositoryItem item in Items)
            {
                try
                {
                    DataRow dr = table.NewRow();
                    dr["ID"] = item.pkid;
                    dr["Machine"] = item.MachineName;
                    dr["Item"] = item.ToString();
                    dr["ItemType"] = item.GetType().Name;

                    #region User Identification
                    if (item.VCUser != null)
                    {
                        //GrahFiles and MetaObjects
                        if (item.VCUser.Length > 0)
                            dr["User"] = item.VCUser;
                        else
                        {
                            dr["User"] = "Unknown";
                        }
                    }
                    else
                    {
                        //associations dont work well with users?!
                        dr["User"] = "Unknown";
                    }
                    #endregion

                    if (item is MetaBase)
                    {
                        o = true;
                        try
                        {
                            PermissionList permission = permissions[itemIndex];
                            MetaBase mbase = item as MetaBase;

                            if (permission == PermissionList.None && mbase.WorkspaceTypeId == (int)WorkspaceTypeList.Server)
                            {
                                Log.WriteLog("BuildDataSet::MetaBase::[Server]AccessDenied for workspace (" + mbase.WorkspaceName + ")  @ ItemIndex:" + itemIndex.ToString() + " {Count:" + permissions.Count + "}  has " + permission.ToString());
                                dr["Item"] = "ACCESS DENIED";
                            }
                            else
                            {
                                dr["Item"] = mbase.ToString();
                            }
                        }
                        catch (Exception objectException)
                        {
                            LogEntry objlog = new LogEntry();
                            objlog.Title = "Cannot Add Object to Repository DataSet";
                            objlog.Message = objectException.ToString() + " (" + item.pkid.ToString() + "|" + item.MachineName + ")";
                            Logger.Write(objlog);
                        }
                    }
                    else if (item is ObjectAssociation)
                    {
                        a = true;
                        ObjectAssociation objAssoc = item as ObjectAssociation;
                        ClassAssociation classAssoc = DataRepository.Connections[provider].Provider.ClassAssociationProvider.GetByCAid(item.pkid);
                        MetaBase mbaseFrom = Loader.GetFromProvider(objAssoc.ObjectID, objAssoc.ObjectMachine, classAssoc.ParentClass, useServer);
                        MetaBase mbaseTo = Loader.GetFromProvider(objAssoc.ChildObjectID, objAssoc.ChildObjectMachine, classAssoc.ChildClass, useServer);
                        string assocType = ((LinkAssociationType)classAssoc.AssociationTypeID).ToString();
                        if (mbaseFrom != null && mbaseTo != null)
                            dr["Item"] = assocType + " from " + mbaseFrom.ToString() + " to " + mbaseTo.ToString();
                        else
                            dr["Item"] = assocType;
                    }
                    else if (item is GraphFile)
                    {
                        dr["Modified Date"] = (item as GraphFile).ModifiedDate.ToShortDateString() + " " + (item as GraphFile).ModifiedDate.ToShortTimeString();
                        //this.Modified.Visible = true;
                    }

                    try
                    {
                        dr["Status"] = Core.strings.GetEnumDescription(item.State);
                    }
                    catch (Exception stateException)
                    {
                        LogEntry stateLog = new LogEntry();
                        stateLog.Title = "Cannot set Object State in Repository DataSet";
                        stateLog.Message = stateException.ToString();
                        Logger.Write(stateLog);
                    }

                    table.Rows.Add(dr);
                    itemIndex++;
                }
                catch (Exception xxOverall)
                {
                    LogEntry overallLog = new LogEntry();
                    overallLog.Title = "Outer Exception";
                    overallLog.Message = xxOverall.ToString();
                    Logger.Write(overallLog);
                }
            }

            if (o)
            {
                this.Type.Visible = true;
            }
            else if (a)
            {
                this.Identifier.Width = 800;
                this.User.Visible = false;
                this.Status.Visible = false;
                this.Actions.Visible = false;
                this.Enabled = true;
                this.dataGridView1.Enabled = true;
                this.btnPerformActions.Visible = false;
                this.btnCancel.Visible = false;
            }

            table.AcceptChanges();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    if (row.Cells[i] is DataGridViewComboBoxCell)
                    {
                        DataGridViewComboBoxCell cell = row.Cells[i] as DataGridViewComboBoxCell;
                        if (cell != null)
                        {
                            try
                            {
                                cell.Value = comboBox1.SelectedText;
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue;
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (permissions != null)
                {
                    if (permissions.Count > e.RowIndex)
                    {
                        if (permissions[e.RowIndex] == PermissionList.None)
                        {
                            e.CellStyle = noAccessStyle;
                            dataGridView1.Rows[e.RowIndex].ReadOnly = true;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // ShowAppropriateDropdown(e.ColumnIndex,e.RowIndex);
        }

        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            //  ShowAppropriateDropdown(e.ColumnIndex,e.RowIndex);
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            DataGridViewComboBoxCell cell =
           dataGridView1.CurrentCell as DataGridViewComboBoxCell;

            if (cell != null && !cell.Items.Contains(e.FormattedValue))
            {
                // Insert the new value into position 0
                // in the item collection of the cell
                cell.Items.Insert(0, e.FormattedValue);
                // When setting the Value of the cell, the  
                // string is not shown until it has been
                // comitted. The code below will make sure 
                // it is committed directly.
                if (dataGridView1.IsCurrentCellDirty)
                {
                    // Ensure the inserted value will 
                    // be shown directly.
                    // First tell the DataGridView to commit 
                    // itself using the Commit context...
                    dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
                // ...then set the Value that needs 
                // to be committed in order to be displayed directly.
                cell.Value = cell.Items[0];
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue;
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            panelBottom.Refresh();
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (!suspendDataChanges)
                isDirty = true;
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridViewComboBoxEditingControl comboControl = e.Control as DataGridViewComboBoxEditingControl;
            if (comboControl != null)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);

                List<string> actions = new List<string>();
                actions.Add("None");

                DataGridViewCell cell = dataGridView1.SelectedCells[0];
                DataGridViewRow row = dataGridView1.Rows[cell.RowIndex];

                //if (cell.Value == null)
                {
                    bool canOpen = false;
                    //if (items[0] is GraphFile)
                    //    canOpen = true;

                    string statusString = dataGridView1.Rows[cell.RowIndex].Cells["Status"].Value.ToString();
                    VCStatusList itemStatus = (VCStatusList)Enum.Parse(typeof(VCStatusList), statusString);
                    IRepositoryItem item = items[cell.RowIndex];
                    IWorkspaceItem wsItem;
                    bool hasadmin = false;
                    bool haswrite = false;
                    bool hasread = false;
                    if (item is IWorkspaceItem)
                    {
                        wsItem = item as IWorkspaceItem;
                        PermissionList userPermission =
                            permissionService.GetServerPermission(wsItem.WorkspaceName, wsItem.WorkspaceTypeId);
                        if (userPermission == PermissionList.Delete)
                        {
                            hasadmin = true;
                            hasread = true;
                            haswrite = true;
                        }
                        if (userPermission == PermissionList.Write)
                        {
                            hasread = true;
                            haswrite = true;
                        }
                        if (userPermission == PermissionList.Read)
                        {
                            hasread = true;
                        }
                    }
                    else
                    {
                        wsItem = null;
                    }
                    string provider = (isLocal) ? Core.Variables.Instance.ClientProvider : Core.Variables.Instance.ServerProvider;
                    List<RepositoryRule> rules =
                        repositoryService.GetAvailableActions(provider, item, hasadmin, haswrite, hasread);
                    //if (canOpen && IsLocal && item.State != VCStatusList.CheckedIn)
                    //    actions.Add("Open");
                    if ((rules.Count == 0) && (!(canOpen && IsLocal)))
                    {
                        cell.ReadOnly = true;
                        //cell.Items.Add("No Actions Possible");
                    }
                    else
                    {
                        foreach (RepositoryRule r in rules)
                        {
                            if (r.Caption.Length > 0)
                            {
                                actions.Add(r.Caption);
                                cell.Tag = rules;
                            }
                        }
                        cell.ReadOnly = false;
                    }
                }
                //if (actions.Contains("Restore (Check In)"))
                //    actions.Add("Delete");

                comboControl.DataSource = actions;

            }
        }

        //private ActionResult EmbedMetaObject(List<ItemAndRule> itemRules, string ObjectProvider, int i, IRepositoryItem item, MetaBase mbase)
        //{
        //    bool allowAdmins = itemRules[i].Rule.AllowAdmin;
        //    bool allowWriters = itemRules[i].Rule.AllowWriter;
        //    bool allowReaders = itemRules[i].Rule.AllowReader;
        //    bool SameUser = mbase.VCUser == strings.GetVCIdentifier();
        //    //WE MUST ALWAYS EMBED OBJECTS!
        //    //WE MUST NOT EXECUTE RULES ON OBJECTS IF PERMISSIONS ARE INCORRECT
        //    //THUS BASICLY IGNORING THESE RULE VARIABLES
        //    PermissionList userPermission = permissionService.GetServerPermission(mbase.WorkspaceName, mbase.WorkspaceTypeId);
        //    bool mayExecuteBasedOnPermission = false;
        //    if (userPermission == PermissionList.Admin && allowAdmins)
        //    {
        //        mayExecuteBasedOnPermission = true;
        //    }
        //    if (userPermission == PermissionList.Write && allowWriters)
        //    {
        //        mayExecuteBasedOnPermission = true;
        //    }
        //    if (userPermission == PermissionList.Read && allowReaders)
        //    {
        //        mayExecuteBasedOnPermission = true;
        //    }
        //    if (mayExecuteBasedOnPermission)
        //    {
        //        RepositoryRule allowedRule = repositoryService.GetValidRule(ObjectProvider, mbase, allowAdmins, allowWriters, allowReaders, SameUser, itemRules[i].Rule.TargetState);
        //        if (allowedRule != null)
        //        {
        //            if (mbase.WorkspaceTypeId == (int)WorkspaceTypeList.Client)
        //            {
        //                ActionResult res = new ActionResult();
        //                res.Success = false;
        //                res.Message = mbase.ToString() + "[" + mbase.Class + "] - Workspace is not a server workspace";
        //                res.FromState = Enum.GetName(typeof(VCStatusList), mbase.State);
        //                res.TargetState = allowedRule.TargetState;
        //                res.Repository = ObjectProvider;
        //                res.Item = mbase.ToString();
        //                return res;
        //            }

        //            ItemAndRule itemAndRule = new ItemAndRule(allowedRule, mbase);
        //            itemRules[i].EmbeddedItemRules.Add(itemAndRule);
        //            //itemRules[i].Rule.EmbeddedRules.Add(allowedRule);
        //            //item.EmbeddedItems.Add(mbase);
        //            //}
        //            return null; //return null to embeddingerrors so that it does not add and prevent execution
        //        }
        //        else
        //        {
        //            ActionResult r = new ActionResult();
        //            r.Success = false;
        //            r.Message = mbase.ToString() + "[" + mbase.Class + "] - Cannot find rule or missing permission for workspace: " + mbase.WorkspaceName;
        //            r.FromState = Enum.GetName(typeof(VCStatusList), mbase.State);
        //            r.TargetState = "Skipped";
        //            r.Repository = ObjectProvider;
        //            r.Item = mbase.ToString();
        //            return r;
        //        }
        //    }
        //    else
        //    {
        //        ActionResult r = new ActionResult();
        //        r.Success = false;
        //        r.intermediate = true;
        //        r.Message = mbase.ToString() + "[" + mbase.Class + "] - Permission does not allow execution";
        //        r.FromState = Enum.GetName(typeof(VCStatusList), mbase.State);
        //        r.TargetState = "Prevented";
        //        r.Repository = ObjectProvider;
        //        r.Item = mbase.ToString();
        //        return r;
        //    }
        //}
        private ActionResult EmbedMetaObject(List<ItemAndRule> itemRules, string ObjectProvider, int i, IRepositoryItem item, MetaBase mbase)
        {
            if (mbase.WorkspaceTypeId == (int)WorkspaceTypeList.Client || mbase.WorkspaceTypeId == (int)WorkspaceTypeList.Sandbox)
            {
                ActionResult res = new ActionResult();
                res.Success = false;
                res.Message = mbase.ToString() + "[" + mbase.Class + "] - Workspace is not a server workspace";
                res.FromState = Enum.GetName(typeof(VCStatusList), mbase.State);
                res.TargetState = "Skipped";
                res.Repository = ObjectProvider;
                res.Item = mbase.ToString();
                return res;
            }
            PermissionList userPermission = permissionService.GetServerPermission(mbase.WorkspaceName, mbase.WorkspaceTypeId);
            WorkspaceKey wsKey = new WorkspaceKey();
            wsKey.Name = mbase.WorkspaceName;
            wsKey.WorkspaceTypeId = mbase.WorkspaceTypeId;
            bool isAdmin = permissionService.HasAtLeastThisPermission(wsKey, PermissionList.Delete);
            bool isWrite = permissionService.HasAtLeastThisPermission(wsKey, PermissionList.Write);
            bool isRead = permissionService.HasAtLeastThisPermission(wsKey, PermissionList.Read);
            bool SameUser = false;
            if (ObjectProvider == Core.Variables.Instance.ClientProvider)
                SameUser = strings.GetVCIdentifier() == server.getCurrentUser(mbase as IRepositoryItem, Core.Variables.Instance.ServerProvider);
            else
                SameUser = strings.GetVCIdentifier() == server.getCurrentUser(mbase as IRepositoryItem, Core.Variables.Instance.ClientProvider);

            RepositoryRule allowedRule = repositoryService.GetValidRule(ObjectProvider, mbase, isAdmin, isWrite, isRead, SameUser, itemRules[i].Rule.TargetState);
            if (allowedRule != null)
            {
                bool allowAdmins = allowedRule.AllowAdmin;
                bool allowWriters = allowedRule.AllowWriter;
                bool allowReaders = allowedRule.AllowReader;
                bool mayExecuteBasedOnPermission = false;
                if (userPermission == PermissionList.Delete && allowAdmins)
                {
                    mayExecuteBasedOnPermission = true;
                }
                if (userPermission == PermissionList.Write && allowWriters)
                {
                    mayExecuteBasedOnPermission = true;
                }
                if (userPermission == PermissionList.Read && allowReaders)
                {
                    mayExecuteBasedOnPermission = true;
                }
                string message = "";
                if (item is GraphFile)
                {
                    if (itemRules[i].Rule.Caption == "Unlock" && allowedRule.Caption != "Unlock" && allowedRule.Caption != "")
                    {
                        mayExecuteBasedOnPermission = false;
                        message = "Cannot unlock because the object is not locked";
                    }

                    if (itemRules[i].Rule.Caption == "Force Check In" && allowedRule.Caption != "Force Check In" && allowedRule.Caption != "")
                    {
                        mayExecuteBasedOnPermission = false;
                        message = "Cannot force check in because the object is not checked out";
                    }

                    if (itemRules[i].Rule.Caption == "Lock" && allowedRule.Caption != "Lock" && allowedRule.Caption != "")
                    {
                        mayExecuteBasedOnPermission = false;
                        message = "Cannot lock object because it is not checked in";
                    }
                }

                //checked when we get rule--there will be norule if this fails
                //if (allowedRule.MustBeDifferentUser && SameUser)
                //{
                //    mayExecuteBasedOnPermission = false;
                //}
                //if (allowedRule.SameUser && !SameUser)
                //{
                //    if (!allowedRule.MustBeDifferentUser)
                //        mayExecuteBasedOnPermission = false;
                //}
                VCStatusList currentServerState = client.GetServerObjectState(mbase);
                string clientString = Core.Variables.Instance.ClientProvider;
                string serverString = Core.Variables.Instance.ServerProvider;
                //if we are checking in and the server object is not checked out TO YOU and the client state is not none then you do not have permission
                if (allowedRule.Provider == clientString && ObjectProvider == clientString && ((allowedRule.ClientState == VCStatusList.CheckedIn && allowedRule.ServerState == VCStatusList.CheckedIn) || (allowedRule.ClientState == VCStatusList.CheckedOut && allowedRule.ServerState == VCStatusList.None)))
                {
                    //Tarrynn 3 March 2014 Added changeUser (checkedin to checkedin)
                    if (currentServerState != VCStatusList.CheckedOut && mbase.State != VCStatusList.None && allowedRule.ChangeUser)
                    {
                        mayExecuteBasedOnPermission = false;
                        message = " is not checked out on the server";

                        //update client to reflect server?
                        //setMissingObjectState(1, d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetBypkidMachine(item.pkid, item.MachineName), Core.Variables.Instance.ClientProvider);
                    }
                    else if (currentServerState == VCStatusList.CheckedOut && mbase.State != VCStatusList.None && allowedRule.ChangeUser)
                    {
                        if (allowedRule.SameUser)
                        {
                            if (client.getCurrentUser(mbase, serverString) != strings.GetVCIdentifier())
                            {
                                mayExecuteBasedOnPermission = false;
                                message = " is not checked out to you on the server";
                            }
                        }
                    }
                }
                else if (allowedRule.Provider == clientString && ObjectProvider == clientString && allowedRule.ClientState == VCStatusList.MarkedForDelete && allowedRule.ServerState == VCStatusList.MarkedForDelete)
                {
                    //if object is MFD and does not exist on the server
                    if (d.DataRepository.Connections[serverString].Provider.MetaObjectProvider.GetBypkidMachine(mbase.pkid, mbase.MachineName) == null)
                    {
                        message = " does not exist on the server";
                        mayExecuteBasedOnPermission = false;
                    }
                }

                if (mayExecuteBasedOnPermission)
                {
                    ItemAndRule itemAndRule = new ItemAndRule(allowedRule, mbase);
                    itemRules[i].EmbeddedItemRules.Add(itemAndRule);
                    return null; //return null to embeddingerrors so that it does not add and prevent execution
                }
                else
                {
                    ActionResult r = new ActionResult();
                    r.Success = false;
                    r.intermediate = true;
                    if (message != "")
                        r.Message = mbase.ToString() + "[" + mbase.Class + "] - " + message;
                    else
                        r.Message = mbase.ToString() + "[" + mbase.Class + "] - Permission does not allow execution";
                    r.FromState = Enum.GetName(typeof(VCStatusList), mbase.State);
                    r.TargetState = "Prevented";
                    r.Repository = ObjectProvider;
                    r.Item = mbase.ToString();
                    return r;
                }
            }
            else
            {
                ActionResult r = new ActionResult();
                r.Success = false;
                //string x = (!isAdmin && !isWrite && !isRead) ? "missing permission for workspace: " + mbase.WorkspaceName : "Cannot find rule";
                //r.Message = mbase.ToString() + "[" + mbase.Class + "(" + mbase.pkid + ":" + mbase.MachineName + ")] - Cannot find rule or missing permission for workspace: " + mbase.WorkspaceName;
                r.Message = mbase.ToString() + "[" + mbase.Class + " (" + mbase.pkid + ":" + mbase.MachineName + ")] - " + ((!isAdmin && !isWrite && !isRead) ? "Missing permission for workspace: " + mbase.WorkspaceName : "Cannot find rule");
                r.FromState = Enum.GetName(typeof(VCStatusList), mbase.State);
                r.TargetState = "Skipped";
                r.Repository = ObjectProvider;
                r.Item = mbase.ToString();
                return r;
            }
        }
        private VCStatusList GetStatusForVCStatusID(int statusID)
        {
            return (VCStatusList)statusID;
            // Enum.Parse(typeof(MetaBuilder.BusinessLogic.VCStatusList), statusID.ToString());
        }

        private TList<GraphFile> usedGetAllFileAssociations;
        private TList<GraphFileAssociation> getAllFileAssociations(GraphFile file, MetaBase mbObj, string provider)
        {
            usedGetAllFileAssociations.Add(file);
            TList<GraphFileAssociation> gfos = new TList<GraphFileAssociation>();
            foreach (GraphFileAssociation gO in d.DataRepository.Connections[provider].Provider.GraphFileAssociationProvider.GetByGraphFileIDGraphFileMachine(file.pkid, file.MachineName))
            {
                if (gO.ObjectID == mbObj.pkid && gO.ObjectMachine == mbObj.MachineName)
                {
                    gfos.Add(gO);
                }
                else if (gO.ChildObjectID == mbObj.pkid && gO.ChildObjectMachine == mbObj.MachineName)
                {
                    gfos.Add(gO);
                }
            }

            MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter adapter = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();
            foreach (GraphFile newFile in adapter.GetAllFilesByTypeID((int)FileTypeList.Diagram, (provider == Core.Variables.Instance.ServerProvider)))
            //foreach (GraphFile newFile in d.DataRepository.Connections[provider].Provider.GraphFileProvider.GetAll())
            {
                if (usedGetAllFileAssociations.Contains(newFile))
                    continue;

                GraphFile nextFile = newFile;
                if (file.PreviousVersionID != null)
                {
                    if (nextFile.pkid == file.PreviousVersionID)
                        gfos.AddRange(getAllFileAssociations(nextFile, mbObj, provider));
                }
                else
                {
                    if (nextFile.Name == file.Name)
                        gfos.AddRange(getAllFileAssociations(nextFile, mbObj, provider));
                }
            }
            return gfos;
        }
        private TList<GraphFile> usedGetAllFileObjects;
        private TList<GraphFileObject> getAllFileObjects(GraphFile file, MetaBase mbObj, string provider)
        {
            usedGetAllFileObjects.Add(file);
            TList<GraphFileObject> gfos = new TList<GraphFileObject>();
            foreach (GraphFileObject gO in d.DataRepository.Connections[provider].Provider.GraphFileObjectProvider.GetByGraphFileIDGraphFileMachine(file.pkid, file.MachineName))
            {
                if (gO.MetaObjectID == mbObj.pkid && gO.MachineID == mbObj.MachineName)
                {
                    gfos.Add(gO);
                }
            }
            MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter adapter = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();
            foreach (GraphFile newFile in adapter.GetAllFilesByTypeID((int)FileTypeList.Diagram, (provider == Core.Variables.Instance.ServerProvider)))
            //foreach (GraphFile newFile in d.DataRepository.Connections[provider].Provider.GraphFileProvider.GetAll())
            {
                if (usedGetAllFileObjects.Contains(newFile))
                    continue;

                GraphFile nextFile = newFile;
                if (file.PreviousVersionID != null)
                {
                    if (nextFile.pkid == file.PreviousVersionID)
                        gfos.AddRange(getAllFileObjects(nextFile, mbObj, provider));
                }
                else
                {
                    if (nextFile.Name == file.Name)
                        gfos.AddRange(getAllFileObjects(nextFile, mbObj, provider));
                }
            }

            return gfos;
        }

        private static int GetObjectGraphFiles(MetaObject obj, string provider)
        {
            int files = 0;
            int.TryParse(d.DataRepository.Connections[provider].Provider.GraphFileObjectProvider.GetByMetaObjectIDMachineID(obj.pkid, obj.Machine).Count.ToString(), out files);
            return files;
        }

        private void PerformActions()
        {
            SplashScreen.PleaseWait.ShowPleaseWaitForm();

            List<ActionResult> embeddingErrors = new List<ActionResult>();
            if (logDisplayer == null)
                logDisplayer = new LogDisplayer();

            SplashScreen.PleaseWait.SetStatus("Retrieving Actions" + Environment.NewLine + "This can take very long depending on the amount of actions");

            #region Get Actions
            // Get the action cell index
            int actionColumnIndex = -1;
            int syncItemIndex = -1;
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.CellType == typeof(DataGridViewComboBoxCell))
                {
                    actionColumnIndex = col.Index;
                }
            }
            List<ItemAndRule> itemRules = new List<ItemAndRule>();
            List<RepositoryEventHandler> delegatesToCall = new List<RepositoryEventHandler>();
            List<IRepositoryItem> itemsToOpen = new List<IRepositoryItem>();
            //ADDED Delete Function
            List<IRepositoryItem> itemsToDelete = new List<IRepositoryItem>();
            //bool AutoOpenFilesOnCheckOut = false;// cbAutoCheckout.Checked;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewComboBoxCell cell = row.Cells[actionColumnIndex] as DataGridViewComboBoxCell;
                if (cell.EditedFormattedValue != null)
                {
                    string actionToBeTaken = cell.EditedFormattedValue.ToString();
                    syncItemIndex = row.Index;
                    //if (cell.Value.ToString() == "Open")
                    //{
                    //    itemsToOpen.Add(items[syncItemIndex]);
                    //}

                    //ADDED Delete Function
                    if (actionToBeTaken == "Delete")
                    {
                        itemsToDelete.Add(items[syncItemIndex]);
                        continue;
                    }

                    if (cell.Tag != null && Items.Count > 0)
                    {
                        List<RepositoryRule> rules = cell.Tag as List<RepositoryRule>;
                        RepositoryRule rule = RetrieveRuleForCaption(actionToBeTaken, rules);
                        if (rule != null)
                        {
                            ItemAndRule itemRule = new ItemAndRule(rule, Items[syncItemIndex]);
                            itemRules.Add(itemRule);
                            if (rule.Caption.StartsWith("Check Out") && cbAutoCheckout.Checked && Items[syncItemIndex] is GraphFile)
                            {
                                itemsToOpen.Add(Items[syncItemIndex]);
                            }
                            else
                            {
                                if (rule.Caption.StartsWith("Open"))
                                {
                                    itemsToOpen.Add(Items[syncItemIndex]);
                                }
                            }
                        }
                    }
                    else
                    {
                        //cannot find rules for this item
                    }

                    #region OldCode
                    /* f.RepositoryEventHandler handler = null;
                   switch (actionToBeTaken)
                    {
                        case "Check Out (Write)":
                            itemsToCheckIn.Add(items[syncItemIndex]);
                            //spec.LoadWorkspace(IsLocal);
                            delegatesToCall.Add(new f.RepositoryEventHandler(repositoryService.CheckOut));
                            if (AutoOpenFilesOnCheckOut)
                                itemsToOpen.Add(items[syncItemIndex]);
                            break;
                        case "Check Out (Read)":
                            itemsToCheckIn.Add(items[syncItemIndex]);
                            delegatesToCall.Add(new f.RepositoryEventHandler(repositoryService.CheckOutRead));
                            if (AutoOpenFilesOnCheckOut)
                                itemsToOpen.Add(items[syncItemIndex]);
                            break;
                        case "Check In":
                            itemsToCheckIn.Add(items[syncItemIndex]); 
                            delegatesToCall.Add(new f.RepositoryEventHandler(repositoryService.CheckIn));
                            break;
                        case "Mark Obsolete":
                            itemsToCheckIn.Add(items[syncItemIndex]);
                            delegatesToCall.Add(new f.RepositoryEventHandler(repositoryService.MarkObsolete));
                            break;
                        case "Activate (Check in)":
                            itemsToCheckIn.Add(items[syncItemIndex]);
                            delegatesToCall.Add(new f.RepositoryEventHandler(repositoryService.CheckIn));
                            break;
                        case "Unlock (Check in)":
                            itemsToCheckIn.Add(items[syncItemIndex]);
                            delegatesToCall.Add(new f.RepositoryEventHandler(repositoryService.CheckIn));
                            break;
                        case "Lock":
                            itemsToCheckIn.Add(items[syncItemIndex]);
                            delegatesToCall.Add(new f.RepositoryEventHandler(repositoryService.LockItem));
                            break;
                        case "Force CheckIn":
                            itemsToCheckIn.Add(items[syncItemIndex]);
                            delegatesToCall.Add(new f.RepositoryEventHandler(repositoryService.CheckIn));
                            break;
                        case "No Actions Possible":
                            // do nothing
                            break;
                        case "Open":
                            itemsToOpen.Add(items[syncItemIndex]);
                            break;
                    }*/
                    #endregion
                }
            }
            #endregion

            string ObjectProvider = IsLocal ? Core.Variables.Instance.ClientProvider : Core.Variables.Instance.ServerProvider;

            Loader.FlushDataViews();
            repositoryService.LogIsWatching = cbDisplayLog.Checked;
            for (int i = 0; i < itemRules.Count; i++)
            {
                SplashScreen.PleaseWait.SetStatus("Embedding Objects");
                //No need to embed for delete (skipped above)
                if (itemsToDelete.Contains(itemRules[i].Item))
                    continue;

                //SplashScreen.PleaseWait.SetStatus("Embedding Object #" + i.ToString());
                IRepositoryItem item = itemRules[i].Item;

                #region Add GraphFile Objects to embed
                if (item is GraphFile)
                {
                    SplashScreen.PleaseWait.SetStatus("Embedding graphfile objects" + Environment.NewLine + "This can take very long depending on the amount of associated objects");
                    GraphFile file = item as GraphFile;

                    // find each item within this graphfile
                    TList<MetaObject> objectsToTransfer = DataRepository.Connections[ObjectProvider].Provider.MetaObjectProvider.GetByGraphFileIDGraphFileMachineFromGraphFileObject(file.pkid, file.Machine);
                    ActionResult gfRes = new ActionResult();
                    gfRes.Success = false;
                    gfRes.FromState = Enum.GetName(typeof(VCStatusList), file.State);
                    gfRes.TargetState = Enum.GetName(typeof(VCStatusList), VCStatusList.Skipped);
                    gfRes.Message = Core.strings.GetFileNameOnly(file.Name);
                    gfRes.Repository = ObjectProvider;
                    gfRes.Item = "File";
                    foreach (MetaObject mo in objectsToTransfer)
                    {
                        bool useServer = ObjectProvider == Core.Variables.Instance.ServerProvider;

                        MetaBase mbase = Loader.GetFromProvider(mo.pkid, mo.Machine, mo.Class, useServer);
                        //SplashScreen.PleaseWait.SetStatus("Embedding graphfile object" + Environment.NewLine + "This can take very long depending on the amount of other objects");
                        ActionResult cannotEmbed = EmbedMetaObject(itemRules, ObjectProvider, i, item, mbase);
                        if (cannotEmbed != null)
                        {
                            if (!embeddingErrors.Contains(cannotEmbed)) //Add some order to the chaos
                                embeddingErrors.Add(cannotEmbed);

                            embeddingErrors.Add(cannotEmbed);
                        }
                    }

                    TList<GraphFileAssociation> associationsToTransfer = DataRepository.Connections[ObjectProvider].Provider.GraphFileAssociationProvider.GetByGraphFileIDGraphFileMachine(file.pkid, file.Machine);
                    foreach (GraphFileAssociation gfass in associationsToTransfer)
                    {
                        ObjectAssociation ass = new ObjectAssociation();
                        ass.CAid = gfass.CAid;
                        ass.ChildObjectID = gfass.ChildObjectID;
                        ass.ChildObjectMachine = gfass.ChildObjectMachine;
                        ass.ObjectMachine = gfass.ObjectMachine;
                        ass.ObjectID = gfass.ObjectID;

                        ObjectAssociation realAss = DataRepository.Connections[ObjectProvider].Provider.ObjectAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(ass.CAid, ass.ObjectID, ass.ChildObjectID, ass.ObjectMachine, ass.ChildObjectMachine);
                        if (realAss != null)
                        {
                            ass.VCStatusID = realAss.VCStatusID;
                            ass.VCMachineID = realAss.VCMachineID;
                            ass.VCUser = realAss.VCUser;
                        }
                        if (realAss.Machine != "DB-TRIGGER")
                        {
                            //SplashScreen.PleaseWait.SetStatus("Embedding graphfile association" + Environment.NewLine + "This can take very long depending on the amount of associated objects");
                            //PermissionList userPermission = permissionService.GetServerPermission(file.WorkspaceName, file.WorkspaceTypeId);
                            RepositoryRule allowedRule = repositoryService.GetValidRule(ObjectProvider, ass, false, false, false, ass.VCUser == strings.GetVCIdentifier(), itemRules[i].Rule.TargetState);
                            if (allowedRule != null)
                            {
                                ItemAndRule itemAndRule = new ItemAndRule(allowedRule, ass);
                                itemRules[i].EmbeddedItemRules.Add(itemAndRule);
                            }
                        }
                    }
                }
                #endregion

                #region Add Association objects to embed
                if (item is ObjectAssociation)
                {
                    SplashScreen.PleaseWait.SetStatus("Embedding association objects");
                    ObjectAssociation oassoc = item as ObjectAssociation;
                    // try to locate the objects on the client
                    // locate the children
                    TList<MetaObject> children = DataRepository.Connections[ObjectProvider].Provider.MetaObjectProvider.GetByChildObjectIDChildObjectMachineFromObjectAssociation(oassoc.ChildObjectID, oassoc.ChildObjectMachine);
                    foreach (MetaObject mo in children)
                    {
                        MetaBase mbase = ObjectHelper.ConvertToMetaBase(mo, (ObjectProvider == Core.Variables.Instance.ServerProvider));
                        /*Meta.MetaBase mbase = Meta.Loader.CreateInstance(mo.Class);
                        string prevcon = Core.Variables.Instance.ConnectionString;
                        Core.Variables.Instance.ConnectionString = (ObjectProvider == Core.Variables.Instance.ClientProvider) ? prevcon : Core.Variables.Instance.ServerConnectionString;
                        mbase.LoadFromID(mo.Pkid, mo.Machine);
                        Core.Variables.Instance.ConnectionString = prevcon;*/
                        EmbedMetaObject(itemRules, ObjectProvider, i, oassoc, mbase);
                    }

                    TList<MetaObject> parents = DataRepository.Connections[ObjectProvider].Provider.MetaObjectProvider.GetByObjectIDObjectMachineFromObjectAssociation(oassoc.ObjectID, oassoc.ObjectMachine);
                    foreach (MetaObject mop in parents)
                    {
                        MetaBase mbasep = ObjectHelper.ConvertToMetaBase(mop, (ObjectProvider == Core.Variables.Instance.ServerProvider));
                        /*Meta.MetaBase mbase = Meta.Loader.CreateInstance(mo.Class);
                        string prevcon = Core.Variables.Instance.ConnectionString;
                        Core.Variables.Instance.ConnectionString = (ObjectProvider == Core.Variables.Instance.ClientProvider) ? prevcon : Core.Variables.Instance.ServerConnectionString;
                        mbase.LoadFromID(mo.Pkid, mo.Machine);
                        Core.Variables.Instance.ConnectionString = prevcon;*/
                        EmbedMetaObject(itemRules, ObjectProvider, i, oassoc, mbasep);
                    }
                }
                #endregion

                if (item is MetaBase)
                {
                    //and class is complex embed children
                    MetaBase complexMetaBase = (item as MetaBase);
                    if (complexMetaBase.Class == "DataTable" || complexMetaBase.Class == "Entity" || complexMetaBase.Class == "DataEntity" || complexMetaBase.Class == "PhysicalInformationArtefact" || complexMetaBase.Class == "LogicalInformationArtefact" || complexMetaBase.Class == "DataDomain")
                    {
                        foreach (ObjectAssociation childAss in d.DataRepository.Connections[ObjectProvider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(complexMetaBase.pkid, complexMetaBase.MachineName))
                        {
                            MetaObject childObject = d.DataRepository.Connections[ObjectProvider].Provider.MetaObjectProvider.GetBypkidMachine(childAss.ChildObjectID, childAss.ChildObjectMachine);
                            if (childObject != null)
                            {
                                if (childObject.Class == "DataColumn" || childObject.Class == "Attribute" || childObject.Class == "DataField" || childObject.Class == "DataAttribute" || childObject.Class == "DataValue")
                                {
                                    MetaBase childMetabase = ObjectHelper.ConvertToMetaBase(childObject, (ObjectProvider == Core.Variables.Instance.ServerProvider));
                                    EmbedMetaObject(itemRules, ObjectProvider, i, item, childMetabase);
                                }
                            }
                        }
                    }
                }
            }

            if (embeddingErrors.Count > 0) //and 1 is error
            {
                SplashScreen.PleaseWait.SetStatus("Preventing Execution");

                logDisplayer.Text = "Errors occurred which prevent these rules from executing";
                bool r = true;
                foreach (ActionResult resError in embeddingErrors)
                {
                    //if all these errors are inter then mustcancel = false
                    //if (r)
                    //    r = !resError.intermediate;

                    logDisplayer.AddMessage(resError);
                }

                if (r)
                {
                    SplashScreen.PleaseWait.PleaseWaitForm.SendToBack();
                    logDisplayer.ShowDialog(this);
                    SplashScreen.PleaseWait.PleaseWaitForm.BringToFront();
                    SplashScreen.PleaseWait.CloseForm();
                    logDisplayer.Dispose();
                    logDisplayer = null;

                    return;
                }
                else
                {
                    logDisplayer = new LogDisplayer();
                }
            }

            List<ActionResult> changedObjects = new List<ActionResult>();

            #region sync
            SplashScreen.PleaseWait.SetStatus("Synchronising item state before execution...");
            bool syncFailed = false;
            for (int ix = 0; ix < itemRules.Count; ix++)
            {
                int ixx = ix;
                ItemAndRule itemAndRule = itemRules[ix];
                if (!SyncObjectStateOnServer(itemAndRule))
                {
                    ActionResult skipRes = new ActionResult();
                    skipRes.Repository = itemAndRule.Rule.Provider;
                    skipRes.Message = "Skipped : state not in sync - " + itemAndRule.Item.ToString();
                    skipRes.FromState = itemAndRule.Rule.CurrentState.ToString();
                    skipRes.TargetState = "Skipped";
                    skipRes.intermediate = true;
                    logDisplayer.AddMessage(skipRes);
                    SplashScreen.PleaseWait.SetStatus("Item state is no longer in sync with the server and is therefore being skipped!");

                    syncFailed = true;
                }
            }
            #endregion

            #region Execute Changes on the objects
            client.MarkedForDeleteItems = new List<MetaObject>(); //refresh list of MarkedForDelete items so as to not reCheck if syncing alot in one go
            server.MarkedForDeleteItems = new List<MetaObject>(); //refresh list of MarkedForDelete items so as to not reCheck if syncing alot in one go
            if (!syncFailed)
            {
                for (int ix = 0; ix < itemRules.Count; ix++)
                {
                    int ixx = ix;
                    ItemAndRule itemAndRule = itemRules[ix];
                    //skip deleting items
                    if (itemsToDelete.Contains(itemAndRule.Item))
                        continue;
                    //IRepositoryItem repositoryItem = itemRules[ix].Item;
                    RepositoryRule rule = itemRules[ix].Rule;
                    SplashScreen.PleaseWait.SetStatus("Executing Action (" + itemAndRule.Rule.Caption + ") " + ((int)(ixx + 1)).ToString() + " of " + itemRules.Count.ToString());
                    List<ActionResult> results = repositoryService.GeneralCommand(server, client, ref itemAndRule);
                    if (cbDisplayLog.Checked && itemRules.Count > 0)
                    {
                        logDisplayer.AddMessage(rule.Caption);
                        foreach (ActionResult result in results)
                        {
                            logDisplayer.AddMessage(result);
                        }
                    }
                }
            }

            #endregion

            #region MarkedForDelete Sync

            if (client.MarkedForDeleteItems.Count > 0 || server.MarkedForDeleteItems.Count > 0) //these will only get populate upon checkin
            {
                List<MetaObject> MarkedForDeleteItems = new List<MetaObject>();
                if (client.MarkedForDeleteItems.Count > 0)
                    MarkedForDeleteItems.AddRange(client.MarkedForDeleteItems);
                if (server.MarkedForDeleteItems.Count > 0)
                    MarkedForDeleteItems.AddRange(server.MarkedForDeleteItems);

                SplashScreen.PleaseWait.SetStatus("Sychronising Marked For Delete Items (" + client.MarkedForDeleteItems.Count + ")");
                //check if there are occurences on active diagram files
                MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter adapter = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();
                for (int providerNumber = 1; providerNumber < 3; providerNumber++)
                {
                    string providerFromNumber = providerNumber == 1 ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider;
                    foreach (MetaObject obj in MarkedForDeleteItems)
                    {
                        TList<GraphFile> files = adapter.GetAllFilesByObjectId(obj.pkid, obj.Machine, (providerFromNumber == Core.Variables.Instance.ServerProvider));
                        if (files.Count == 0)
                        //if (d.DataRepository.Connections[providerFromNumber].Provider.GraphFileObjectProvider.GetByMetaObjectIDMachineID(obj.pkid, obj.Machine).Count == 0)
                        {
                            //object is orphaned and must be deleted (can only happen when checkin of single MFD object which was never on any diagrams)
                            SplashScreen.PleaseWait.SetStatus(providerFromNumber + " - Deleting Object (" + obj.pkid + ")");
                            DeleteObject(obj, providerFromNumber);
                        }
                        else
                        {
                            bool hasActive = false;
                            foreach (GraphFile providerFile in files)
                            //foreach (GraphFile providerFile in d.DataRepository.Connections[providerFromNumber].Provider.GraphFileProvider.GetByMetaObjectIDMachineIDFromGraphFileObject(obj.pkid, obj.Machine))
                            {
                                if (providerFile.IsActive)
                                {
                                    hasActive = true;
                                    break;
                                }
                            }
                            //no active files using this object
                            if (!hasActive)
                            {
                                SplashScreen.PleaseWait.SetStatus(providerFromNumber + " - Deleting Object (" + obj.pkid + ")");
                                DeleteObject(obj, providerFromNumber);
                            }
                            else
                            {
                                ActionResult mfdREsult = new ActionResult();
                                mfdREsult.Repository = providerFromNumber;
                                mfdREsult.Message = obj.pkid + " was marked for delete";
                                mfdREsult.Success = true;
                                mfdREsult.TargetState = VCStatusList.MarkedForDelete.ToString();
                                mfdREsult.FromState = VCStatusList.MarkedForDelete.ToString();
                                logDisplayer.AddMessage(mfdREsult);
                            }
                        }
                    }
                }
            }

            #endregion

            #region Delete graphfiles & objects

            int dI = 1;
            List<MetaBase> objectsDeleted = new List<MetaBase>();
            List<GraphFile> filesToDeleteFrom = new List<GraphFile>();
            if (!syncFailed)
            {
                foreach (IRepositoryItem repositoryItem in itemsToDelete)
                {
                    SplashScreen.PleaseWait.SetStatus("Deleting Object " + (dI).ToString() + " of " + itemsToDelete.Count.ToString());
                    if (repositoryItem is GraphFile) //You cannot do this currently because there is no way to mark a diagram for delete
                    {
                        GraphFile file = repositoryItem as GraphFile;
                        if (MessageBox.Show(this, "All objects on this diagram will be marked for delete.", "Would you like to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                            DeleteFile(file, ObjectProvider);
                    }
                    else if (repositoryItem is MetaBase)
                    {
                        MetaBase mb = repositoryItem as MetaBase;
                        //Get Files
                        bool occurence = false;
                        MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter fileAdapter = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();
                        TList<GraphFile> allObjectFiles = fileAdapter.GetFilesByObjectId(mb.pkid, mb.MachineName, (ObjectProvider == Core.Variables.Instance.ServerProvider));//d.DataRepository.Connections[ObjectProvider].Provider.GraphFileProvider.GetByMetaObjectIDMachineIDFromGraphFileObject(mb.pkid, mb.MachineName);
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
                            //"You must first remove this object from all of the following active diagrams in order to delete it","Permanently delete " + mb.ToString(), MessageBoxButtons.Ok, MessageBoxIcon.Information);
                            if (MessageBox.Show(this, "All occurences of this object will be permanently deleted from the following active diagrams." + Environment.NewLine + affectedFiles, "Would you like to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                            {
                                SplashScreen.PleaseWait.SetStatus("Deleting Object " + (dI).ToString() + " of " + itemsToDelete.Count.ToString());
                                DeleteObject(mb, ObjectProvider, null);
                                items.Remove(repositoryItem);
                                if (!objectsDeleted.Contains(mb))
                                    objectsDeleted.Add(mb);
                            }
                        }
                        else
                        {
                            DeleteObject(mb, ObjectProvider, null);
                            items.Remove(repositoryItem);
                            if (!objectsDeleted.Contains(mb))
                                objectsDeleted.Add(mb);
                        }
                    }
                    dI += 1;
                }
            }
            if (objectsDeleted.Count > 0)
            {
                SplashScreen.PleaseWait.SetStatus("Updating the Binary Large Objects (BLOB)");
                if (!Tools.DeleteObjectsFromFiles.DeleteObjects(objectsDeleted, filesToDeleteFrom, ObjectProvider))
                {
                    logDisplayer.AddMessage("Binary Large Objects were not successfully updated, please refer to the log file for more information");
                }
            }

            #endregion

            SplashScreen.PleaseWait.SetStatus("Completed");
            SplashScreen.PleaseWait.PleaseWaitForm.SendToBack();

            if (itemRules.Count > itemsToOpen.Count || !IsLocal)
            {
                //Single log for everything
                logDisplayer.AddMessage("Complete");
                logDisplayer.BringToFront();
                logDisplayer.TopMost = true;
                if (cbDisplayLog.Checked)
                {
                    logDisplayer.ShowDialog(this);
                    logDisplayer.Dispose();
                }
            }
            logDisplayer = null;

            // clear cache
            Loader.FlushDataViews();
            SplashScreen.PleaseWait.PleaseWaitForm.BringToFront();

            #region open files that were checked out

            Form f = null;
            if (!syncFailed)
            {
                if (itemsToOpen.Count > 0)
                {
                    DockingForm.DockForm.OpenedFiles.Clear();
                    SplashScreen.PleaseWait.SetStatus("Opening " + itemsToOpen.Count + " items");
                    if (Parent.Parent.Parent.Parent.Parent is Form)
                    {
                        f = Parent.Parent.Parent.Parent.Parent as Form;
                    }
                }
            }

            if (!syncFailed && itemsToOpen.Count > 0)
            {
                //TList<GraphFile> allClientfiles = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileProvider.GetAll();
                foreach (IRepositoryItem item in itemsToOpen)
                {
                    //skip not graphfiles
                    if (!(item is GraphFile))
                        continue;
                    GraphFile fileSelectedToOpen = item as GraphFile;
                    //bool opened = false;
                    //                    //find the most up to date client diagram file and open it
                    //                    foreach (GraphFile clientFile in d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileProvider.GetAll(GraphFileColumn.OriginalFileUniqueID, fileSelectedToOpen.OriginalFileUniqueID, true))
                    //                    {
                    //#if DEBUG
                    //                        //continue;
                    //#endif
                    //                        if (clientFile.IsActive) //2 Active files? nettiers cache? ALL ACTIVE ALWAYS?
                    //                        {
                    //                            //if (clientFile.OriginalFileUniqueID == fileSelectedToOpen.OriginalFileUniqueID)
                    //                            //{
                    //                            if (IsLocal)
                    //                            {
                    //                                OpenFile(clientFile);
                    //                                opened = true;
                    //                                break;
                    //                            }
                    //                            else
                    //                            {
                    //                                //if (clientFile.PreviousVersionID == fileSelectedToOpen.pkid)
                    //                                //{
                    //                                //    OpenFile(clientFile);
                    //                                //    opened = true;
                    //                                //    break;
                    //                                //}
                    //                                //else
                    //                                //{
                    //                                OpenFile(clientFile);
                    //                                opened = true;
                    //                                break;
                    //                                //}
                    //                            }
                    //                            //}
                    //                        }
                    //                    }
                    //if (!opened)
                    {
                        //Log.WriteLog(fileSelectedToOpen.ToString() + "(" + fileSelectedToOpen.pkid + "-local=" + IsLocal.ToString() + ") was not found on the client and could not be opened ==> Attempting recovery");
                        DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter tempAdapter = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();
                        GraphFile TEMPfile = tempAdapter.GetQuickFileDetails(fileSelectedToOpen.OriginalFileUniqueID.ToString());
                        if (TEMPfile != null)
                        {
                            OpenFile(TEMPfile);
                            //opened = true;
                            continue;
                        }
                        else
                        {
                            Log.WriteLog("Could not open file using recovery.");
                            //OpenFile(fileSelectedToOpen);
                            continue;
                        }
                    }
                    //continue;
                    //for (int ix = 0; ix < itemRules.Count; ix++)
                    //{
                    //    if (itemRules[ix].Item is GraphFile)
                    //    {
                    //        GraphFile file = itemRules[ix].Item as GraphFile;
                    //        GraphFile fileOld = item as GraphFile;
                    //        //9 October 2013 - changed name to GUID
                    //        if (file.OriginalFileUniqueID == fileOld.OriginalFileUniqueID)
                    //        {
                    //            OpenFile(file);
                    //            //Added to prevent opening of more than 1 of the same file!
                    //            //break;
                    //        }
                    //    }
                    //}
                }
                if (itemsToOpen != null && itemsToOpen.Count > 0)
                    DockingForm.DockForm.BringToFront();
            }

            #endregion

            isDirty = false;

            //Do not refresh when we are opening objects!
            if (f == null)
            {
                SplashScreen.PleaseWait.SetStatus("Refreshing Table");
                OnActionsPerformed(this, EventArgs.Empty);

                SplashScreen.PleaseWait.CloseForm();

                Loader.FlushDataViews();
            }
            else
            {
                SplashScreen.PleaseWait.CloseForm();
                f.Close();
            }
        }
        //15 January 2014 - Current State != Server State
        private bool SyncObjectStateOnServer(ItemAndRule itemandrule)
        {
            if (itemandrule.Rule.Provider == Core.Variables.Instance.ServerProvider)
            {
                if (itemandrule.Item is GraphFile)
                {
                    if (server.GetState(itemandrule.Item) != (VCStatusList)Enum.Parse(typeof(VCStatusList), (itemandrule.Item as GraphFile).VCStatusID.ToString()))
                    //if ((d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.GraphFileProvider.GetBypkidMachine((itemandrule.Item as GraphFile).pkid, (itemandrule.Item as GraphFile).Machine).VCStatusID != (itemandrule.Item as GraphFile).VCStatusID))
                    {
                        return false;
                    }
                }
                else if (itemandrule.Item is MetaBase)
                {
                    if (server.GetState(itemandrule.Item) != (VCStatusList)Enum.Parse(typeof(VCStatusList), (itemandrule.Item as MetaBase).State.ToString()))
                    //if ((d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.MetaObjectProvider.GetBypkidMachine((itemandrule.Item as MetaBase).pkid, (itemandrule.Item as MetaBase).MachineName).VCStatusID != (int)((itemandrule.Item as MetaBase).State)))
                    {
                        return false;
                    }
                }
            }
            //else if (itemandrule.Rule.Provider == Core.Variables.Instance.ClientProvider)
            //{
            //    if (itemandrule.Item is GraphFile)
            //    {
            //        if ((d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileProvider.GetBypkidMachine((itemandrule.Item as GraphFile).pkid, (itemandrule.Item as GraphFile).Machine).VCStatusID != (itemandrule.Item as GraphFile).VCStatusID))
            //        {
            //            return false;
            //        }
            //    }
            //    else if (itemandrule.Item is MetaBase)
            //    {
            //        if ((d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetBypkidMachine((itemandrule.Item as GraphFile).pkid, (itemandrule.Item as MetaBase).MachineName).VCStatusID != (int)((itemandrule.Item as MetaBase).State)))
            //        {
            //            return false;
            //        }
            //    }
            //}

            return true;
        }
        private void OpenFile(GraphFile file)
        {
            MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter adapter = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();
            GraphFile fileReal = adapter.GetQuickFileDetails(file.pkid, file.Machine, false);//DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileProvider.GetBypkidMachine(file.pkid, file.Machine);
            //do not refresh data here
            if (fileReal == null)
            {
                return;
            }

            DockingForm.DockForm.OpenGraphFileFromDatabase(fileReal, true, true);
            if (fileReal.VCStatusID == (int)VCStatusList.CheckedOut)
            {
                //forces data refresh
                //DockingForm.DockForm.GetCurrentGraphViewContainer().OpeningFromServer = true;
                //forces document to be saved as
                DockingForm.DockForm.GetCurrentGraphViewContainer().ForceSaveAs = true;
                //Forces automatic save
                //DockingForm.DockForm.GetCurrentGraphViewContainer().StartSaveProcess(true);
            }

            //forces data refresh and disables editing if not already checked out
            bool checkedOutOnClient = false;
            //added workspace info to decrease amount of data iterations
            foreach (GraphFile clientFile in adapter.GetAllFilesByWorkspaceTypeIdWorkspaceName(fileReal.WorkspaceTypeId, fileReal.WorkspaceName, (int)FileTypeList.Diagram, false))
            //foreach (GraphFile clientFile in d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileProvider.GetByWorkspaceNameWorkspaceTypeId(fileReal.WorkspaceName, fileReal.WorkspaceTypeId))
            {
                if (clientFile.IsActive)
                {
                    if (clientFile.OriginalFileUniqueID == fileReal.OriginalFileUniqueID && clientFile.VCStatusID == (int)VCStatusList.CheckedOut)
                    {
                        checkedOutOnClient = true;
                        break;
                    }
                }
            }

            //file!=filereal
            if (!checkedOutOnClient)
                if (fileReal.VCStatusID == (int)VCStatusList.CheckedOutRead || fileReal.VCStatusID == (int)VCStatusList.Locked)
                    DockingForm.DockForm.GetCurrentGraphViewContainer().ReadOnly = true;

            // Get the latest version of the file
            /* if (fileReal == null)
                 fileReal =
                     DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.GraphFileProvider.GetBypkidMachine(file.pkid, file.Machine);
            if (DockingForm.DockForm != null)
                 DockingForm.DockForm.OpenGraphFileFromDatabase(fileReal,false);*/
            //DockingForm.DockForm.BringToFront();
        }
        private void DeleteFile(GraphFile file, string provider)
        {
            SplashScreen.PleaseWait.SetStatus("Preparing embedded file objects");
            if (logDisplayer == null)
                logDisplayer = new LogDisplayer();
            ActionResult r;

            #region objects
            //bool ObjectSuccess = true;
            TList<GraphFileObject> gfOs = d.DataRepository.Connections[provider].Provider.GraphFileObjectProvider.GetByGraphFileIDGraphFileMachine(file.pkid, file.MachineName);
            TList<MetaObject> Os = new TList<MetaObject>();
            TList<MetaObject> linkedOs = new TList<MetaObject>();
            int objectI = 1;
            foreach (GraphFileObject graphFileObject in gfOs)
            {
                SplashScreen.PleaseWait.SetStatus("Deleting Object " + objectI + " of " + gfOs.Count);
                objectI += 1;
                MetaObject mObj = d.DataRepository.Connections[provider].Provider.MetaObjectProvider.GetBypkidMachine(graphFileObject.MetaObjectID, graphFileObject.MachineID);
                try
                {
                    bool useServer = provider.ToLower() == Core.Variables.Instance.ClientProvider ? false : true;
                    MetaBase mBase = Loader.GetFromProvider(mObj.pkid, mObj.Machine, mObj.Class, useServer);
                    //if (ObjectSuccess)
                    //ObjectSuccess = DeleteObject(mBase, provider, file);
                    DeleteObject(mBase, provider, file);
                }
                catch
                {
                    //ObjectSuccess = false;
                }
            }

            #endregion

            #region file
            //delete file
            SplashScreen.PleaseWait.SetStatus("Deleting File");
            r = new ActionResult();
            //if (ObjectSuccess)
            try
            {
                d.DataRepository.Connections[provider].Provider.GraphFileProvider.Delete(file);
                r.Success = true;
                r.Message = "Successfully deleted file";
                //Other files?
                MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter adapter = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();
                foreach (GraphFile newFile in adapter.GetAllFilesByTypeID((int)FileTypeList.Diagram, (provider == Core.Variables.Instance.ServerProvider)))
                //foreach (GraphFile newFile in d.DataRepository.Connections[provider].Provider.GraphFileProvider.GetAll())
                {
                    SplashScreen.PleaseWait.SetStatus("Deleting Inactive Files");
                    GraphFile nextFile = newFile;
                    if (file.PreviousVersionID != null)
                    {
                        if (nextFile.pkid == file.PreviousVersionID)
                            d.DataRepository.Connections[provider].Provider.GraphFileProvider.Delete(nextFile);
                    }
                    else
                    {
                        if (nextFile.Name == file.Name)
                            d.DataRepository.Connections[provider].Provider.GraphFileProvider.Delete(nextFile);
                    }
                }
            }
            catch (Exception ex)
            {
                if (r.Success == true)
                {
                    r.Message = "Successfully deleted active file - Inactive files still remain";
                }
                else
                {
                    r.Success = false;
                    r.Message = "Failed to delete error was logged";
                    Log.WriteLog(ex.ToString());
                }
            }
            //else
            //{
            //    r.Success = false;
            //    r.TargetState = "Aborted because objects still exist";
            //}

            r.Item = "File";
            r.FromState = Enum.GetName(typeof(VCStatusList), file.VCStatusID);
            r.TargetState = "Deleted";
            r.Repository = provider;
            logDisplayer.AddMessage(r);
            #endregion

            //logDisplayer.AddMessage("Finished");
            //if (cbDisplayLog.Checked)
            //    logDisplayer.ShowDialog(this);
        }
        private bool DeleteObject(MetaBase obj, string provider, GraphFile file)
        {
            //ADD ADVANCED LOG
            if (logDisplayer == null)
                logDisplayer = new LogDisplayer();
            ActionResult r = null;

            #region associations

            //get objectassociations
            TList<ObjectAssociation> oas = new TList<ObjectAssociation>();
            //parents
            oas = d.DataRepository.Connections[provider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(obj.pkid, obj.MachineName);
            //add children 
            oas.AddRange(d.DataRepository.Connections[provider].Provider.ObjectAssociationProvider.GetByChildObjectIDChildObjectMachine(obj.pkid, obj.MachineName));

            //get graph file associations
            TList<GraphFileAssociation> GFoas = new TList<GraphFileAssociation>();
            foreach (ObjectAssociation o in oas)
            {
                if (file == null)
                    GFoas.AddRange(d.DataRepository.Connections[provider].Provider.GraphFileAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(o.CAid, o.ObjectID, o.ChildObjectID, o.ObjectMachine, o.ChildObjectMachine));
                else
                {
                    usedGetAllFileAssociations = new TList<GraphFile>();
                    GFoas.AddRange(getAllFileAssociations(file, obj, provider));
                }
            }

            bool exception = false;
            //delete graphfile associations
            try
            {
                d.DataRepository.Connections[provider].Provider.GraphFileAssociationProvider.Delete(GFoas);
            }
            catch (Exception ex)
            {
                exception = true;
                Log.WriteLog(ex.ToString());
            }

            #region delete artefacts
            TList<Artifact> dArtefacts = new TList<Artifact>();
            if (!exception)
                try
                {
                    foreach (ObjectAssociation oa in oas)
                    {
                        foreach (Artifact a in d.DataRepository.Connections[provider].Provider.ArtifactProvider.GetByObjectIDObjectMachine(oa.ObjectID, oa.ObjectMachine))
                        {
                            dArtefacts.Add(a);
                        }
                        foreach (Artifact a in d.DataRepository.Connections[provider].Provider.ArtifactProvider.GetByChildObjectIDChildObjectMachine(oa.ObjectID, oa.ObjectMachine))
                        {
                            dArtefacts.Add(a);
                        }
                    }
                    //only the ones on this file
                    d.DataRepository.Connections[provider].Provider.ArtifactProvider.Delete(dArtefacts);

                    //foreach (Artifact a in dArtefacts)
                    //{
                    //    try
                    //    {
                    //        MetaObject aMobj = d.DataRepository.MetaObjectProvider.GetBypkidMachine(a.ArtifactObjectID, a.ArtefactMachine);
                    //        DeleteObject(Loader.GetByID(aMobj.Class, aMobj.pkid, aMobj.Machine), provider, null);
                    //    }
                    //    catch
                    //    {
                    //        //is on another association
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    exception = true;
                    Log.WriteLog(ex.ToString());
                }
            #endregion

            //delete associations
            if (!exception)
                try
                {
                    //only the ones on this file
                    d.DataRepository.Connections[provider].Provider.ObjectAssociationProvider.Delete(oas);
                }
                catch (Exception ex)
                {
                    exception = true;
                    Log.WriteLog(ex.ToString());
                }

            #endregion

            if (exception)
            {
                r = new ActionResult();
                r.Success = false;
                r.Item = "Associations for object pkid : " + obj.pkid + " - machine: " + obj.MachineName;
                r.FromState = Enum.GetName(typeof(VCStatusList), obj.State);
                r.TargetState = "Deleted";
                r.Message = "Failed to delete associations and corresponding diagram associations - Exception was logged";
                r.Repository = provider;

                logDisplayer.AddMessage(r);
            }
            //else dont skip object deletion
            {
                #region object

                try
                {
                    //delete graphfileobjects for this object
                    TList<GraphFileObject> gfOs = null;
                    if (file == null)
                        gfOs = d.DataRepository.Connections[provider].Provider.GraphFileObjectProvider.GetByMetaObjectIDMachineID(obj.pkid, obj.MachineName);
                    else
                    {
                        usedGetAllFileObjects = new TList<GraphFile>();
                        gfOs = getAllFileObjects(file, obj, provider);
                    }
                    d.DataRepository.Connections[provider].Provider.GraphFileObjectProvider.Delete(gfOs);

                    //delete this object's values when it is not on any diagrams
                    bool inter = false;
                    if (d.DataRepository.Connections[provider].Provider.GraphFileObjectProvider.GetByGraphFileIDGraphFileMachine(obj.pkid, obj.MachineName).Count == 0)
                    {
                        //this means it is no longer on any files and can be deleted
                        TList<ObjectFieldValue> fields = d.DataRepository.Connections[provider].Provider.ObjectFieldValueProvider.GetByObjectIDMachineID(obj.pkid, obj.MachineName);
                        d.DataRepository.Connections[provider].Provider.ObjectFieldValueProvider.Delete(fields);

                        //delete this object
                        d.DataRepository.Connections[provider].Provider.MetaObjectProvider.Delete(d.DataRepository.Connections[provider].Provider.MetaObjectProvider.GetBypkidMachine(obj.pkid, obj.MachineName));
                        inter = false;
                    }
                    else
                    {
                        inter = true;
                        //mark all those objects for delete
                        server.SQLUpdateObjectState(8, obj.pkid, obj.MachineName, Core.Variables.Instance.ServerProvider);
                    }
                    exception = false;

                    r = new ActionResult();
                    r.Success = true;
                    r.intermediate = inter;
                    r.Item = "Object pkid : " + obj.pkid + " - machine: " + obj.MachineName;
                    r.FromState = Enum.GetName(typeof(VCStatusList), obj.State);
                    r.TargetState = "Deleted";
                    r.Message = "Succesfully deleted object (" + dArtefacts.Count + " artefacts :" + GFoas.Count + " graphfileassociations :" + oas.Count + " associations" + gfOs.Count + " graphfileobjects )";
                    r.Repository = provider;
                }
                catch (Exception ex)
                {
                    //TODO Null reference here  -- > Tarrynn ? !(double artefact delete)

                    if (!exception)
                    {
                        exception = true;
                        Log.WriteLog(ex.ToString());

                        r = new ActionResult();
                        r.Success = false;
                        r.Item = "Object pkid : " + obj.pkid + " - machine: " + obj.MachineName;
                        r.FromState = Enum.GetName(typeof(VCStatusList), obj.State);
                        r.TargetState = "Deleted";
                        if (file == null)
                        {
                            r.Message = "Failed to delete object - Exception was logged";
                        }
                        else
                        {
                            r.Message = "This object cannot be deleted because it exists on other diagrams";
                            r.Success = true;
                            r.intermediate = true;
                        }
                        r.Repository = provider;
                    }
                }
                if (r != null)
                    logDisplayer.AddMessage(r);

                #endregion
            }

            return r.Success;
        }
        private bool DeleteObject(MetaObject obj, string provider)
        {
            //ADD ADVANCED LOG
            if (logDisplayer == null)
                logDisplayer = new LogDisplayer();
            ActionResult r = null;

            #region associations

            //get objectassociations
            TList<ObjectAssociation> oas = new TList<ObjectAssociation>();
            //parents
            oas = d.DataRepository.Connections[provider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(obj.pkid, obj.Machine);
            //add children 
            oas.AddRange(d.DataRepository.Connections[provider].Provider.ObjectAssociationProvider.GetByChildObjectIDChildObjectMachine(obj.pkid, obj.Machine));

            //get graph file associations
            TList<GraphFileAssociation> GFoas = new TList<GraphFileAssociation>();
            foreach (ObjectAssociation o in oas)
            {
                //if (file == null)
                GFoas.AddRange(d.DataRepository.Connections[provider].Provider.GraphFileAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(o.CAid, o.ObjectID, o.ChildObjectID, o.ObjectMachine, o.ChildObjectMachine));
                //else
                //{
                //    usedGetAllFileAssociations = new TList<GraphFile>();
                //    GFoas.AddRange(getAllFileAssociations(file, obj, provider));
                //}
            }

            bool exception = false;
            //delete graphfile associations
            try
            {
                d.DataRepository.Connections[provider].Provider.GraphFileAssociationProvider.Delete(GFoas);
            }
            catch (Exception ex)
            {
                exception = true;
                Log.WriteLog(ex.ToString());
            }

            #region delete artefacts
            TList<Artifact> dArtefacts = new TList<Artifact>();
            if (!exception)
                try
                {
                    foreach (ObjectAssociation oa in oas)
                    {
                        foreach (Artifact a in d.DataRepository.Connections[provider].Provider.ArtifactProvider.GetByObjectIDObjectMachine(oa.ObjectID, oa.ObjectMachine))
                        {
                            dArtefacts.Add(a);
                        }
                        foreach (Artifact a in d.DataRepository.Connections[provider].Provider.ArtifactProvider.GetByChildObjectIDChildObjectMachine(oa.ObjectID, oa.ObjectMachine))
                        {
                            dArtefacts.Add(a);
                        }
                    }
                    d.DataRepository.Connections[provider].Provider.ArtifactProvider.Delete(dArtefacts);

                    //foreach (Artifact a in dArtefacts)
                    //{
                    //    try
                    //    {
                    //        MetaObject aMobj = d.DataRepository.MetaObjectProvider.GetBypkidMachine(a.ArtifactObjectID, a.ArtefactMachine);
                    //        DeleteObject(Loader.GetByID(aMobj.Class, aMobj.pkid, aMobj.Machine), provider, null);
                    //    }
                    //    catch
                    //    {
                    //        //is on another association
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    exception = true;
                    Log.WriteLog(ex.ToString());
                }
            #endregion

            //delete associations
            if (!exception)
                try
                {
                    d.DataRepository.Connections[provider].Provider.ObjectAssociationProvider.Delete(oas);
                }
                catch (Exception ex)
                {
                    exception = true;
                    Log.WriteLog(ex.ToString());
                }

            #endregion

            if (exception)
            {
                r = new ActionResult();
                r.Success = false;
                r.Item = "Associations for object pkid : " + obj.pkid + " - machine: " + obj.Machine;
                r.FromState = Enum.GetName(typeof(VCStatusList), obj.VCStatusID);
                r.TargetState = "Deleted";
                r.Message = "Failed to delete associations and corresponding diagram associations - Exception was logged";
                r.Repository = provider;

                logDisplayer.AddMessage(r);
            }
            //else dont skip object deletion
            {
                #region object

                try
                {
                    //delete graphfileobjects for this object
                    TList<GraphFileObject> gfOs = null;
                    //if (file == null)
                    gfOs = d.DataRepository.Connections[provider].Provider.GraphFileObjectProvider.GetByMetaObjectIDMachineID(obj.pkid, obj.Machine);
                    //else
                    //{
                    //    usedGetAllFileObjects = new TList<GraphFile>();
                    //    gfOs = getAllFileObjects(file, obj, provider);
                    //}
                    d.DataRepository.Connections[provider].Provider.GraphFileObjectProvider.Delete(gfOs);

                    //delete this object's values when it is not on any diagrams
                    bool inter = false;
                    if (d.DataRepository.Connections[provider].Provider.GraphFileObjectProvider.GetByGraphFileIDGraphFileMachine(obj.pkid, obj.Machine).Count == 0)
                    {
                        //this means it is no longer on any files and can be deleted
                        TList<ObjectFieldValue> fields = d.DataRepository.Connections[provider].Provider.ObjectFieldValueProvider.GetByObjectIDMachineID(obj.pkid, obj.Machine);
                        d.DataRepository.Connections[provider].Provider.ObjectFieldValueProvider.Delete(fields);

                        //delete this object
                        d.DataRepository.Connections[provider].Provider.MetaObjectProvider.Delete(d.DataRepository.Connections[provider].Provider.MetaObjectProvider.GetBypkidMachine(obj.pkid, obj.Machine));
                        inter = false;
                    }
                    else
                    {
                        inter = true;
                    }
                    exception = false;

                    r = new ActionResult();
                    r.Success = true;
                    r.intermediate = inter;
                    r.Item = "Object pkid : " + obj.pkid + " - machine: " + obj.Machine;
                    r.FromState = Enum.GetName(typeof(VCStatusList), obj.VCStatusID);
                    r.TargetState = "Deleted";
                    r.Message = "Deleted (pkid:" + obj.pkid + "|machine: " + obj.Machine + ")-no active files";
                    r.Repository = provider;
                }
                catch (Exception ex)
                {
                    if (!exception)
                    {
                        exception = true;
                        Log.WriteLog(ex.ToString());

                        r = new ActionResult();
                        r.Success = false;
                        r.Item = "Object pkid : " + obj.pkid + " - machine: " + obj.Machine;
                        r.FromState = Enum.GetName(typeof(VCStatusList), obj.VCStatusID);
                        r.TargetState = "Deleted";

                        r.Message = "Failed to delete object - Exception was logged";

                        r.Repository = provider;
                    }
                }
                if (r != null)
                    logDisplayer.AddMessage(r);

                #endregion
            }

            return r.Success;
        }

        private void repositoryService_ResultAdded(ActionResult result)
        {
            if (logDisplayer != null)
            {
                logDisplayer.AddMessage(result);
            }
        }

        /// <summary>
        /// Resets the available workspace hashtable and set the current workspace to whatever was selected before
        /// </summary>
        /// //WTF
        //private static void ResetWorkspacesOnClient()
        //{
        //    int WorkspaceTypeId = Variables.Instance.CurrentWorkspaceTypeId;
        //    string workspaceName = Variables.Instance.CurrentWorkspaceName;
        //    /*CoreInjector cinjector = new CoreInjector();
        //    cinjector.InjectCoreVariables();*/
        //    Variables.Instance.CurrentWorkspaceTypeId = WorkspaceTypeId;
        //    Variables.Instance.CurrentWorkspaceName = workspaceName;
        //}

        private RepositoryRule RetrieveRuleForCaption(string s, List<RepositoryRule> rules)
        {
            foreach (RepositoryRule rule in rules)
            {
                if (rule.Caption == s)
                {
                    return rule;
                }
            }
            return null;
        }

        private void SetPossibleActionsDropdown(DataGridViewComboBoxCell cell)
        {
            if (cell.Value == null)
            {
                bool canOpen = false;
                if (items[0] is GraphFile)
                    canOpen = true;
                cell.Items.Clear();
                string statusString = dataGridView1.Rows[cell.RowIndex].Cells["Status"].Value.ToString();
                VCStatusList itemStatus = (VCStatusList)Enum.Parse(typeof(VCStatusList), statusString);
                IRepositoryItem item = items[cell.RowIndex];
                IWorkspaceItem wsItem;
                bool hasadmin = false;
                bool haswrite = false;
                bool hasread = false;
                if (item is IWorkspaceItem)
                {
                    wsItem = item as IWorkspaceItem;
                    PermissionList userPermission = permissionService.GetServerPermission(wsItem.WorkspaceName, wsItem.WorkspaceTypeId);
                    if (userPermission == PermissionList.Delete)
                    {
                        hasadmin = true;
                        hasread = true;
                        haswrite = true;
                    }
                    if (userPermission == PermissionList.Write)
                    {
                        hasread = true;
                        haswrite = true;
                    }
                    if (userPermission == PermissionList.Read)
                    {
                        hasread = true;
                    }
                }
                else
                {
                    wsItem = null;
                }
                string provider = (isLocal) ? Core.Variables.Instance.ClientProvider : Core.Variables.Instance.ServerProvider;
                List<RepositoryRule> rules = repositoryService.GetAvailableActions(provider, item, hasadmin, haswrite, hasread);
                if (canOpen && IsLocal && item.State != VCStatusList.CheckedIn)
                    cell.Items.Add("Open");
                if ((rules.Count == 0) && (!(canOpen && IsLocal)))
                {
                    cell.ReadOnly = true;
                    //cell.Items.Add("No Actions Possible");
                }
                else
                {

                    foreach (RepositoryRule r in rules)
                    {
                        if (r.Caption.Length > 0)
                        {
                            cell.Items.Add(r.Caption);
                            cell.Tag = rules;
                        }
                    }
                    cell.ReadOnly = false;
                }
            }
        }

        private void ShowAppropriateDropdown(int ColIndex, int RowIndex)
        {
            if (ColIndex >= 0 && RowIndex >= 0)
            {
                object o = dataGridView1[ColIndex, RowIndex];
                if (o is DataGridViewComboBoxCell)
                {
                    DataGridViewComboBoxCell cell = o as DataGridViewComboBoxCell;
                    SetPossibleActionsDropdown(cell);
                }
            }
        }

        private void SynchronisationPanel_Leave(object sender, EventArgs e)
        {
        }

        private void SynchronisationPanel_Load(object sender, EventArgs e)
        {
            cbAutoCheckout.Visible = false;
            permissionService = new PermissionService();
            repositoryService = new RepositoryService(permissionService);
            server = new Repository(Repository.RepositoryType.Server);
            client = new Repository(Repository.RepositoryType.Client);

            dataGridView1.CellFormatting += new DataGridViewCellFormattingEventHandler(dataGridView1_CellFormatting);
            //ResetWorkspacesOnClient();
            noAccessStyle = new DataGridViewCellStyle(dataGridView1.DefaultCellStyle);
            noAccessStyle.ForeColor = Color.Gray;
            //cbAutoCheckout.Visible = ShowOpenCheckBox;

            comboBox1.Items.Clear();

            string provider = (isLocal) ? Core.Variables.Instance.ClientProvider : Core.Variables.Instance.ServerProvider;
            List<RepositoryRule> rules = repositoryService.GetAvailableActions(provider);

            foreach (RepositoryRule r in rules)
            {
                if (r.Caption.Length > 0)
                {
                    bool found = false;
                    foreach (object o in comboBox1.Items)
                    {
                        if (o.ToString() == r.Caption)
                        {
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        comboBox1.Items.Add(r.Caption);
                        comboBox1.Tag = rules;
                    }
                }
            }

            comboBox1.SelectedIndex = 0;
        }

        #endregion Methods

        //Label overlayLabel;

        protected override void OnEnabledChanged(EventArgs e)
        {
            //if (overlayLabel == null)
            //{
            //    overlayLabel = new Label();
            //    overlayLabel.Text = "You must select a valid node in the repository tree in order to view its items.";
            //    overlayLabel.AutoSize = false;
            //    overlayLabel.BackColor = Color.FromArgb(30, 255, 0, 0);
            //    overlayLabel.ForeColor = Color.DarkGray;
            //    overlayLabel.TextAlign = ContentAlignment.MiddleCenter;
            //    overlayLabel.Font = new Font(overlayLabel.Font.FontFamily, 28);
            //    overlayLabel.Dock = DockStyle.Fill;
            //    Controls.Add(overlayLabel);
            //    overlayLabel.BringToFront();
            //}
            //base.OnEnabledChanged(e);

            //overlayLabel.Visible = !Enabled;
            dataGridView1.Visible = Enabled;
            btnPerformActions.Enabled = Enabled;
        }

    }
}