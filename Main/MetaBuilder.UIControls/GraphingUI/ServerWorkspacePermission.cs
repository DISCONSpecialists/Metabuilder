using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using System.DirectoryServices;
using System.Runtime.InteropServices;
using System.IO;

namespace MetaBuilder.UIControls.GraphingUI
{
    public partial class ServerWorkspacePermission : Form
    {
        public ServerWorkspacePermission()
        {
            InitializeComponent();

            if (!Core.Networking.Pinger.Ping(Core.Variables.Instance.ServerConnectionString))
            {
                MessageBox.Show(this, "Unable to connect to your current synchronisation server.", "Ping", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }

            BindWorkspaces();
        }

        private List<ServerUserPermission> permissions;
        private List<ServerUserPermission> Permissions
        {
            get
            {
                return permissions;
            }
            set
            {
                permissions = value;
            }
        }

        private TList<Workspace> workspaces;
        private bool currentWorkspacePermissionHaveChanged = false;
        public Workspace CurrentWorkspace()
        {
            if (treeViewWorkspaces.SelectedNode != null)
            {
                Workspace ws = treeViewWorkspaces.SelectedNode.Tag as Workspace;
                if (ws != null)
                {
                    labelCurrentWorkspace.Text = ws.Name + " selected";
                    dataGridViewUserPermissions.Enabled = true;
                    return ws;
                }
            }
            labelCurrentWorkspace.Text = "There is currently no workspace selected";
            dataGridViewUserPermissions.Enabled = false;
            return null;
        }
        private string Provider
        {
            get { return Core.Variables.Instance.ServerProvider; }
        }

        private void BindWorkspaces()
        {
            try
            {
                txtDomainName.Text = Environment.UserDomainName;
                //List<UserPermission> perms = new List<UserPermission>();
                //dataGridViewUserPermissions.DataSource = perms;
                //dataGridViewUserPermissions.Refresh();
                workspaces = DataRepository.Connections[Provider].Provider.WorkspaceProvider.GetAll();
                treeViewWorkspaces.Nodes.Clear();

                foreach (Workspace ws in workspaces)
                {
                    if (ws.WorkspaceTypeId != 3)
                        continue;

                    TreeNode node = new TreeNode();
                    node.Text = ws.Name;
                    node.Tag = ws;
                    treeViewWorkspaces.Nodes.Add(node);
                }

                if (treeViewWorkspaces.Nodes.Count > 0)
                {
                    treeViewWorkspaces.SelectedNode = treeViewWorkspaces.Nodes[0];
                    Workspace ws = treeViewWorkspaces.Nodes[0].Tag as Workspace;
                    Permissions = new List<ServerUserPermission>();
                    TList<User> users = DataAccessLayer.DataRepository.Connections[Provider].Provider.UserProvider.GetAll();
                    users.Sort("Name");
                    string userNameAdding = "";
                    foreach (BusinessLogic.User u in users)
                    {
                        if (u.Name == userNameAdding)
                            continue;

                        userNameAdding = u.Name;
                        Permissions.Add(new ServerUserPermission(u.Name, false, false, false, false));
                    }
                    dataGridViewUserPermissions.DataSource = permissions;
                    populatePermissions();
                }

                //UpdateGridExistingPermissions();
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog("ServerWorkspacePermission::BindWorkspaces" + Environment.NewLine + ex.ToString());
                MessageBox.Show(this, "Unable to retreive workspace and user permission information from the repository." + Environment.NewLine + "Please contact your administrator.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void btnRetrieveUsers_Click(object sender, EventArgs e)
        {
            //find all users in specified domain else get current domain
            if (txtDomainName.Text.Length > 0)
            {
                RefreshData(txtDomainName.Text);
            }
            else
            {
                RefreshData(Environment.UserDomainName);
            }
        }
        private void RefreshData(string domain)
        {
            string group = txtDomainGroup.Text.Length > 0 ? txtDomainGroup.Text : "Domain Users";
            txtDomainGroup.Text = group;
            try
            {
                ListMembersInGroup("WinNT://", domain, group);
                if (Permissions.Count == 0)
                {
                    try
                    {
                        ListMembersInGroup("LDAP://", domain, group);
                    }
                    catch (Exception intEx)
                    {
                        Core.Log.WriteLog("ServerWorkspacePermission::RefreshData::Catch" + Environment.NewLine + intEx.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog("ServerWorkspacePermission::RefreshData" + Environment.NewLine + ex.ToString());
                try
                {
                    ListMembersInGroup("LDAP://", domain, group);
                }
                catch (Exception intEx)
                {
                    Core.Log.WriteLog("ServerWorkspacePermission::RefreshData::Catch" + Environment.NewLine + intEx.ToString());
                }
            }
            populatePermissions();
        }
        private void ListMembersInGroup(string provider, string domainPath, string GroupName)
        {
            try
            {
                //.Net 3.5+
                //PrincipalContext ctx = new PrincipalContext(ContextType.Domain, domainName);
                //GroupPrincipal grp = GroupPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, groupName);
                //foreach (Principal p in grp.GetMembers(false))

                string textAfterDomain = "";
                if (domainPath.Contains("\\"))
                {
                    textAfterDomain = domainPath.Substring(domainPath.IndexOf('\\')).Replace("\\", "");
                    domainPath = domainPath.Substring(0, domainPath.IndexOf('\\'));
                }
                else if (domainPath.Contains("/"))
                {
                    textAfterDomain = domainPath.Substring(domainPath.IndexOf('/')).Replace("/", "");
                    domainPath = domainPath.Substring(0, domainPath.IndexOf('/'));
                }

                DirectoryEntry fullEntry = new DirectoryEntry(provider + domainPath);

                Permissions = new List<ServerUserPermission>();
                if (domainPath.ToLower() == Environment.MachineName.ToLower()) //LOCAL STAYS THE SAME
                {
                    foreach (DirectoryEntry member in fullEntry.Children)//groupEntry.Children)
                    {
                        if (member.SchemaClassName == "User")
                        {
                            //if (member.Name.IndexOf("$") == -1 && member.Name.IndexOf("IUSR") == -1 &&
                            //    member.Name.IndexOf("IWAM") == -1 && member.Name.IndexOf("DSC") == -1 &&
                            //    member.Name != "Administrator" && member.Name.IndexOf("DCS") == -1 &&
                            //    member.Name.IndexOf("SQL") == -1)
                            {
                                if (textAfterDomain.Length > 0 && !member.Name.ToLower().Contains(textAfterDomain.ToLower()))
                                    continue;

                                Permissions.Add(new ServerUserPermission(domainPath + "\\" + member.Name, false, false, false, false));
                            }
                        }
                    }
                }
                else
                {
                    DirectoryEntry groupEntry = fullEntry.Children.Find(txtDomainGroup.Text, "group");

                    foreach (object o in (System.Collections.IEnumerable)groupEntry.Invoke("members", null))//groupEntry.Children)
                    {
                        DirectoryEntry member = new DirectoryEntry(o);
                        if (member.SchemaClassName == "User")
                        {
                            if (member.Name.IndexOf("$") == -1 && member.Name.IndexOf("IUSR") == -1 &&
                                member.Name.IndexOf("IWAM") == -1 && member.Name.IndexOf("DSC") == -1 &&
                                member.Name != "Administrator" && member.Name.IndexOf("DCS") == -1 &&
                                member.Name.IndexOf("SQL") == -1)
                            {
                                if (textAfterDomain.Length > 0 && !member.Name.ToLower().Contains(textAfterDomain.ToLower()))
                                    continue;

                                Permissions.Add(new ServerUserPermission(domainPath + "\\" + member.Name, false, false, false, false));
                            }
                        }
                    }
                }

                dataGridViewUserPermissions.DataSource = Permissions;
            }
            catch (COMException Cex)
            {
                Core.Log.WriteLog(Cex.ToString());
                if (Cex.Message.Contains("invalid dn syntax"))
                    MessageBox.Show(this, "The group specified does not exist", "Invalid Parameter", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show(this, Cex.Message.ToString(), "Communication Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog(ex.ToString());
            }
        }
        private void btnLoadList_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text Files (*.txt)|*.txt";
            ofd.Multiselect = false;
            DialogResult res = ofd.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                Permissions = new List<ServerUserPermission>();

                StreamReader sreader = new StreamReader(ofd.FileName);
                List<string> usersAddedThisTime = new List<string>();
                while (sreader.Peek() > -1)
                {
                    string name = sreader.ReadLine();

                    if (usersAddedThisTime.Contains(name))
                        continue;

                    usersAddedThisTime.Add(name);
                    Permissions.Add(new ServerUserPermission(name, false, false, false, false));
                }
                sreader.Close();
                dataGridViewUserPermissions.DataSource = Permissions;
                populatePermissions();
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text Files (*.txt)|*.txt";
            DialogResult res = sfd.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                StreamWriter swriter = new StreamWriter(sfd.FileName);
                foreach (ServerUserPermission uperm in Permissions)
                {
                    swriter.WriteLine(uperm.Username);
                }
                swriter.Close();
            }
        }

        private void btnNewWorkspace_Click(object sender, EventArgs e)
        {
            //Adds a new workspace to the treeview and selects it
            TreeNode newNode = new TreeNode("Workspace Name");
            Workspace ws = new Workspace();
            ws.Name = "Workspace Name";
            newNode.Tag = ws;
            treeViewWorkspaces.Nodes.Add(newNode);
            newNode.BeginEdit();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (currentWorkspacePermissionHaveChanged)
            {
                if (MessageBox.Show(this, "Would you like to save them before closing?", "Workspace permissions have been altered.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    saveWorkspacePermissions();
                }
            }

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void dataGridViewUserPermissions_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridViewUserPermissions.CurrentCell is DataGridViewCheckBoxCell)
            {
                DataGridViewCheckBoxCell cell = dataGridViewUserPermissions.CurrentCell as DataGridViewCheckBoxCell;

                //dataGridViewUserPermissions.Columns[cell.ColumnIndex].HeaderText.ToString();
                if (cell.ColumnIndex == 0)
                {
                    //admin
                    //Permissions[cell.RowIndex].Admin = !Permissions[cell.RowIndex].Admin;
                    currentWorkspacePermissionHaveChanged = true;
                }
                else if (cell.ColumnIndex == 1)
                {
                    Permissions[cell.RowIndex].Delete = false;
                    Permissions[cell.RowIndex].Write = false;
                    //read
                    //unmark w/d
                    //if (((bool)cell.EditedFormattedValue))
                    //{
                    //    Permissions[cell.RowIndex].Delete = false;
                    //    Permissions[cell.RowIndex].Write = false;
                    //}
                    //else
                    //{
                    //    Permissions[cell.RowIndex].Delete = false;
                    //    Permissions[cell.RowIndex].Write = false;
                    //}
                    currentWorkspacePermissionHaveChanged = true;
                }
                else if (cell.ColumnIndex == 2)
                {
                    Permissions[cell.RowIndex].Delete = false;
                    Permissions[cell.RowIndex].Read = false;
                    //write
                    //mark r
                    //unmark d
                    //if (((bool)cell.EditedFormattedValue))
                    //{
                    //    Permissions[cell.RowIndex].Read = true;
                    //    Permissions[cell.RowIndex].Delete = false;
                    //}
                    //else
                    //{
                    //    Permissions[cell.RowIndex].Read = false;
                    //    Permissions[cell.RowIndex].Delete = false;
                    //}
                    currentWorkspacePermissionHaveChanged = true;
                }
                else if (cell.ColumnIndex == 3)
                {
                    Permissions[cell.RowIndex].Read = false;
                    Permissions[cell.RowIndex].Write = false;
                    //delete
                    //mark r/w
                    //if (((bool)cell.EditedFormattedValue))
                    //{
                    //    Permissions[cell.RowIndex].Read = true;
                    //    Permissions[cell.RowIndex].Write = true;
                    //}
                    //else
                    //{
                    //    Permissions[cell.RowIndex].Read = false;
                    //    Permissions[cell.RowIndex].Write = false;
                    //}
                    currentWorkspacePermissionHaveChanged = true;
                }
            }

            if (dataGridViewUserPermissions.IsCurrentCellDirty)
                dataGridViewUserPermissions.CommitEdit(DataGridViewDataErrorContexts.Commit);

            dataGridViewUserPermissions.Refresh();
        }

        private void treeViewWorkspaces_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (currentWorkspacePermissionHaveChanged)
            {
                if (MessageBox.Show(this, "Would you like to save before selecting the new workspace?", "Workspace permissions have been altered.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    saveWorkspacePermissions();
                }
            }
        }
        private void treeViewWorkspaces_AfterSelect(object sender, TreeViewEventArgs e)
        {
            populatePermissions();
        }

        private TList<BusinessLogic.UserPermission> getExistingPermissions(string WorkspaceName, int WorkspaceTypeID)
        {
            SqlConnection conn = new SqlConnection(Core.Variables.Instance.ServerConnectionString);
            SqlCommand com = new SqlCommand("Select * from UserPermission where WorkspaceName = '" + WorkspaceName + "' AND WorkspaceTypeID = " + WorkspaceTypeID);
            com.Connection = conn;
            SqlDataAdapter dap = new SqlDataAdapter();
            dap.SelectCommand = com;
            DataSet ds = new DataSet();
            dap.Fill(ds);
            TList<BusinessLogic.UserPermission> permissions = new TList<BusinessLogic.UserPermission>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                BusinessLogic.UserPermission perm = new BusinessLogic.UserPermission();
                perm.UserID = int.Parse(row[0].ToString());
                perm.PermissionID = int.Parse(row[1].ToString());
                perm.WorkspaceName = row[2].ToString();
                perm.WorkspaceTypeId = int.Parse(row[3].ToString());

                permissions.Add(perm);
            }

            return permissions;
        }

        private void populatePermissions()
        {
            if (CurrentWorkspace() != null)
            {
                if (Permissions != null)
                {
                    TList<BusinessLogic.UserPermission> existingPermissions = getExistingPermissions(CurrentWorkspace().Name, CurrentWorkspace().WorkspaceTypeId);
                    TList<User> users = DataRepository.Connections[Provider].Provider.UserProvider.GetAll();
                    foreach (ServerUserPermission uperm in Permissions)
                    {
                        //reset this permission
                        uperm.Admin = false;
                        uperm.Delete = false;
                        uperm.Write = false;
                        uperm.Read = false;

                        foreach (User user in users)
                        {
                            foreach (BusinessLogic.UserPermission userPermission in existingPermissions)
                            {
                                if (user.pkid == userPermission.UserID)
                                {
                                    if (uperm.Username == user.Name)
                                    {
                                        if (userPermission.PermissionID == 1)
                                        {
                                            //read
                                            uperm.Read = true;
                                            uperm.Write = false;
                                            uperm.Delete = false;
                                        }
                                        else if (userPermission.PermissionID == 2)
                                        {
                                            //write
                                            uperm.Read = false;
                                            uperm.Write = true;
                                            uperm.Delete = false;
                                        }
                                        else if (userPermission.PermissionID == 3)
                                        {
                                            //admin
                                            uperm.Admin = true;
                                        }
                                        else if (userPermission.PermissionID == 4)
                                        {
                                            //delete
                                            uperm.Read = false;
                                            uperm.Write = false;
                                            uperm.Delete = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    dataGridViewUserPermissions.Refresh();
                    //permissions have been reloaded and so cannot be changed!
                    currentWorkspacePermissionHaveChanged = false;
                }
                dataGridViewUserPermissions.Visible = true;
            }
        }

        private void saveWorkspacePermissions()
        {
            //save the permissions for the current workspace
            if (CurrentWorkspace() == null)
                return;
            else
            {
                if (CurrentWorkspace().pkid == 0)
                {
                    DataRepository.Connections[Provider].Provider.WorkspaceProvider.Save(CurrentWorkspace());
                }
            }
            if (Permissions == null)
                return;
            TList<User> users = DataRepository.Connections[Provider].Provider.UserProvider.GetAll();
            List<string> usersAddedThisTime = new List<string>();
            foreach (ServerUserPermission uperm in Permissions)
            {
                User correspondingUser = null;
                foreach (User usr in users)
                {
                    if (usr.Name == uperm.Username)
                    {
                        correspondingUser = usr;
                        //delete all this users existing permissions
                        deleteAllUserWorkspacePermissions(correspondingUser.pkid);
                        //Break it here so it doesn't continue to loop through the rest of the users
                        break;
                    }
                }
                //only save a user if it has a permission assigned to it
                if (correspondingUser == null && (uperm.Read || uperm.Write || uperm.Admin || uperm.Delete))
                {
                    correspondingUser = new User();
                    correspondingUser.Name = uperm.Username;
                    correspondingUser.CreateDate = DateTime.Today;
                    correspondingUser.Password = "";
                    if (!usersAddedThisTime.Contains(uperm.Username))
                    {
                        usersAddedThisTime.Add(uperm.Username);
                        DataRepository.Connections[Provider].Provider.UserProvider.Save(correspondingUser);
                        Core.Log.WriteLog("A new user was added (" + correspondingUser.Name + " : " + correspondingUser.pkid + ") from user permissions dialog on server");
                    }
                    else
                    {
                        //username exist so no need to add it
                        continue;
                    }
                }
                //else
                //{
                //save each permission not just 1
                if (uperm.Admin)
                    saveSingleUserWorkspacePermission(3, correspondingUser.pkid);

                if (uperm.Write)
                    saveSingleUserWorkspacePermission(2, correspondingUser.pkid);

                if (uperm.Read)
                    saveSingleUserWorkspacePermission(1, correspondingUser.pkid);

                if (uperm.Delete)
                    saveSingleUserWorkspacePermission(4, correspondingUser.pkid);
                //}
            }
        }

        private void deleteAllUserWorkspacePermissions(int UserID)
        {
            foreach (BusinessLogic.UserPermission perm in DataRepository.Connections[Provider].Provider.UserPermissionProvider.GetByUserID(UserID))
            {
                if (perm.WorkspaceName == CurrentWorkspace().Name && perm.WorkspaceTypeId == CurrentWorkspace().WorkspaceTypeId)
                    DataRepository.Connections[Provider].Provider.UserPermissionProvider.Delete(perm);
            }
        }
        private void saveSingleUserWorkspacePermission(int permissionID, int UserID)
        {

            BusinessLogic.UserPermission upermission = new BusinessLogic.UserPermission();
            upermission.UserID = UserID;
            upermission.PermissionID = permissionID;
            upermission.WorkspaceName = CurrentWorkspace().Name;
            upermission.WorkspaceTypeId = CurrentWorkspace().WorkspaceTypeId;
            DataRepository.Connections[Provider].Provider.UserPermissionProvider.Save(upermission);
            Core.Log.WriteLog("A new permission was added (UserID " + UserID + " : PermissionID " + permissionID + ") from user permissions dialog on server");
        }

        private void txtDomainName_TextChanged(object sender, EventArgs e)
        {
            txtDomainGroup.Enabled = !(txtDomainName.Text.ToLower() == Environment.MachineName.ToLower());
        }

        private void btnNewUser_Click(object sender, EventArgs e)
        {
            using (Form f = new Form())
            {
                f.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                f.StartPosition = FormStartPosition.CenterScreen;
                f.Text = "Enter the name of the user.";
                f.ShowInTaskbar = false;

                TextBox txt = new TextBox();
                txt.Dock = DockStyle.Top;
                txt.Margin = new Padding(2);
                txt.TabIndex = 0;
                txt.MaxLength = 100;
                txt.Text = txtDomainName.Text + "\\";

                Button btnOkF = new Button();
                btnOkF.Dock = DockStyle.Top;
                btnOkF.Text = "OK";
                btnOkF.DialogResult = DialogResult.OK;
                btnOkF.Margin = new Padding(2);
                btnOkF.TabIndex = 1;

                Button btnCancelF = new Button();
                btnCancelF.Dock = DockStyle.Top;
                btnCancelF.Text = "Cancel";
                btnCancelF.DialogResult = DialogResult.Cancel;
                btnCancelF.Margin = new Padding(2);
                btnCancelF.TabIndex = 2;
                btnCancel.TabStop = false;

                f.AcceptButton = btnOkF;
                f.CancelButton = btnCancelF;
                f.AutoSize = false;
                f.Width = 200;
                f.Height = txt.Height + btnOK.Height + btnCancelF.Height + btnCancelF.Height + 5;

                f.Controls.Add(btnCancelF);
                f.Controls.Add(btnOkF);
                f.Controls.Add(txt);
                txt.Focus();
                txt.SelectAll();
                if (f.ShowDialog() == DialogResult.OK && ValidateNewUserName(txt.Text))
                {

                    ServerUserPermission newPerm = new ServerUserPermission(txt.Text, false, false, false, false);
                    Permissions.Insert(0, newPerm);
                }
            }
            dataGridViewUserPermissions.DataSource = null;
            dataGridViewUserPermissions.DataSource = Permissions;
        }
        public bool ValidateNewUserName(string name)
        {
            string error = "";
            if (name.Length == 0 || name.Length > 50)
                error = "length";

            if (name.Contains("@") || name.Contains("!") || name.Contains("?") || name.Contains("<") || name.Contains(">") || name.Contains("]") || name.Contains("[") || name.Contains("'") || name.Contains("\"") || name.Contains(":") || name.Contains(";") || name.Contains("/"))//)
                error = "character(s)";

            if (name == txtDomainName.Text + "\\")
                error = "name";

            foreach (ServerUserPermission perm in Permissions)
                if (perm.Username == name)
                    error = "name (already exists)";

            if (error.Length > 0)
                MessageBox.Show("The username you have entered has an invalid " + error, "Invalid username", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return error.Length == 0;
        }

        private void dataGridViewUserPermissions_DataSourceChanged(object sender, EventArgs e)
        {
            dataGridViewUserPermissions.AutoGenerateColumns = false;
            Username.DisplayIndex = 0;
            Admin.DisplayIndex = 1;
            Read.DisplayIndex = 2;
            Write.DisplayIndex = 3;
            Delete.DisplayIndex = 4;
        }
    }

    public class ServerUserPermission
    {

        #region Fields (4)

        private bool admin;
        private bool read;
        private bool write;
        private bool delete;
        private string username;

        #endregion Fields

        #region Constructors (1)

        public ServerUserPermission(string name, bool read, bool write, bool delete, bool admin)
        {
            Username = name;
            Read = read;
            Write = write;
            Delete = delete;
            Admin = admin;
        }

        #endregion Constructors

        #region Properties (4)

        public bool Admin
        {
            get { return admin; }
            set { admin = value; }
        }

        public bool Read
        {
            get { return read; }
            set { read = value; }
        }

        public bool Write
        {
            get { return write; }
            set { write = value; }
        }

        public bool Delete
        {
            get { return delete; }
            set { delete = value; }
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        #endregion Properties

    }
}