using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.UIControls.Dialogs;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.UIControls.GraphingUI
{
    public partial class UserPermissions : Form
    {

        #region Fields (5)

        private bool authenticated;
        private bool isDirty;
        private Workspace lastWorkspace;
        private List<CustomUserPermission> permissions;
        private TList<Workspace> workspaces;

        #endregion Fields

        #region Constructors (1)

        public UserPermissions()
        {
            InitializeComponent();
            Application.Idle += new EventHandler(Application_Idle);
        }

        #endregion Constructors

        #region Properties (2)

        public bool Authenticated
        {
            get { return authenticated; }
            set { authenticated = value; }
        }

        private string provider
        {
            get { return Core.Variables.Instance.ServerProvider; }
        }

        #endregion Properties

        #region Methods (29)


        // Public Methods (3) 

        public Workspace GetCurrentWorkspace()
        {
            if (treeWorkgroups.SelectedNode != null)
            {
                Workspace ws = treeWorkgroups.SelectedNode.Tag as Workspace;
                if (ws != null)
                    return ws;
            }
            return null;
        }

        public void PromptForSave()
        {
            DialogResult res = MessageBox.Show(this,"Save Changes?", "Save Changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                SavePermissions();
            }
        }

        public void ScrollToRow(int rowIndex)
        {
            OnScroll(new ScrollEventArgs(
              ScrollEventType.LargeIncrement, rowIndex));
        }

        // Private Methods (26) 

        void Application_Idle(object sender, EventArgs e)
        {
            btnSave.Enabled = permissions != null;
            btnFind.Enabled = permissions != null;
            btnApply.Enabled = isDirty;
        }

        private void BindWorkspaces()
        {
            try
            {
                txtDomainName.Text = Environment.UserDomainName;
                List<UserPermission> perms = new List<UserPermission>();
                dataGridView1.DataSource = perms;
                dataGridView1.Refresh();
                workspaces = DataRepository.Connections[provider].Provider.WorkspaceProvider.GetAll();
                treeWorkgroups.Nodes.Clear();
                foreach (Workspace ws in workspaces)
                {
                    TreeNode node = new TreeNode();
                    node.Text = ws.Name;
                    node.Tag = ws;
                    treeWorkgroups.Nodes.Add(node);
                }
                if (treeWorkgroups.Nodes.Count > 0)
                {
                    treeWorkgroups.SelectedNode = treeWorkgroups.Nodes[0];
                }
                UpdateGridExistingPermissions();
            }
            catch
            {
                MessageBox.Show(this,"Login to repository failed. Please refer to your administrator", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            SavePermissions();
            isDirty = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            FindTextWinForms textFinder = new FindTextWinForms();
            textFinder.FoundMatch += new EventHandler(textFinder_FoundMatch);

            textFinder.SearchItems = new List<string>();
            foreach (CustomUserPermission perm in permissions)
            {
                textFinder.SearchItems.Add(perm.Username);
            }
            textFinder.ShowDialog(this);
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            if (txtDomainName.Text.Length > 0)
            {
                RefreshData(txtDomainName.Text);
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
                permissions = new List<CustomUserPermission>();

                StreamReader sreader = new StreamReader(ofd.FileName);
                while (sreader.Peek() > -1)
                {
                    permissions.Add(new CustomUserPermission(sreader.ReadLine(), false, false, false));
                }
                sreader.Close();
                dataGridView1.DataSource = permissions;
                UpdateGridExistingPermissions();
            }
        }

        private void btnNewWorkspace_Click(object sender, EventArgs e)
        {
            TreeNode newNode = new TreeNode("New Workgroup");
            Workspace ws = new Workspace();
            ws.Name = "New Workgroup";
            newNode.Tag = ws;
            treeWorkgroups.Nodes.Add(newNode);
            newNode.BeginEdit();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (permissions == null)
                Close();
            SavePermissions();
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text Files (*.txt)|*.txt";
            DialogResult res = sfd.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                StreamWriter swriter = new StreamWriter(sfd.FileName);
                foreach (CustomUserPermission uperm in permissions)
                {
                    swriter.WriteLine(uperm.Username);
                }
                swriter.Close();
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
        }

        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            //  if (GetCurrentWorkspace() != null)
            {
                if (dataGridView1.CurrentCell is DataGridViewCheckBoxCell)
                {
                    DataGridViewCheckBoxCell cell = dataGridView1.CurrentCell as DataGridViewCheckBoxCell;
                    DataGridViewCheckBoxCell othercell = null;
                    DataGridViewCheckBoxCell othercell1 = null;
                    if (cell.ColumnIndex == 1)
                    {
                        // Write
                        othercell = dataGridView1[cell.ColumnIndex + 1, cell.RowIndex] as DataGridViewCheckBoxCell;
                        othercell1 = dataGridView1[cell.ColumnIndex + 2, cell.RowIndex] as DataGridViewCheckBoxCell;
                    }
                    else if (cell.ColumnIndex == 2)
                    {
                        othercell = dataGridView1[cell.ColumnIndex - 1, cell.RowIndex] as DataGridViewCheckBoxCell;
                        othercell1 = dataGridView1[cell.ColumnIndex + 1, cell.RowIndex] as DataGridViewCheckBoxCell;
                    }
                    else if (cell.ColumnIndex == 3)
                    {
                        othercell = dataGridView1[cell.ColumnIndex - 2, cell.RowIndex] as DataGridViewCheckBoxCell;
                        othercell1 = dataGridView1[cell.ColumnIndex - 1, cell.RowIndex] as DataGridViewCheckBoxCell;
                    }

                    bool hasValue;
                    bool.TryParse(cell.EditingCellFormattedValue.ToString(), out hasValue);
                    if (hasValue)
                    {
                        if (othercell != null && othercell1 != null)
                        {
                            othercell.Value = !hasValue;
                            othercell1.Value = !hasValue;
                        }
                    }
                    isDirty = true;
                }
            }
        }

        private void DeleteNode(object sender, EventArgs e)
        {
            Workspace ws = treeWorkgroups.SelectedNode.Tag as Workspace;
            DeleteWorkspace delForm = new DeleteWorkspace();
            delForm.WorkspaceToBeDeleted = ws;
            delForm.Provider = provider;
            DialogResult res = delForm.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                UserPermissions_Load(sender, e);
            }
        }

        private void ListMembersInGroup(string provider, string domainPath, string GroupName)
        {
            try
            {
                // someValidUser and hisPwd are user credentials to bind to the DC
                //DirectoryEntry groupEntry = new DirectoryEntry(provider + domainPath + "/" + GroupName, "Deon Fourie", "timeisagame", AuthenticationTypes.Secure);
                DirectoryEntry groupEntry = new DirectoryEntry(provider + domainPath);
                permissions = new List<CustomUserPermission>();
                foreach (DirectoryEntry member in groupEntry.Children)
                {
                    if (member.SchemaClassName == "User")
                    {
                        if (member.Name.IndexOf("$") == -1 && member.Name.IndexOf("IUSR") == -1 &&
                            member.Name.IndexOf("IWAM") == -1 && member.Name.IndexOf("DSC") == -1 &&
                            member.Name != "Administrator" && member.Name.IndexOf("DCS") == -1 &&
                            member.Name.IndexOf("SQL") == -1)
                        {
                            permissions.Add(new CustomUserPermission(domainPath + "\\" + member.Name, false, false, false));
                        }
                    }
                }
                dataGridView1.DataSource = permissions;
                /*
                System.DirectoryServices.PropertyCollection pcoll = groupEntry.Properties;
                // invoke native method "members"
                MembersCollection = groupEntry.Invoke("Members") as IADsMembers;
                object[] filter = { "user" };
                MembersCollection.Filter = filter;
                // enumerate members of collection object that supports the IADsMembers interface
                // ADSI provider doesn't support count property
                try
                {
                    
                    permissions = new List<UserPermission>();
                    foreach (IADsUser member in MembersCollection)
                    {
                        if (member.Name.IndexOf("$") == -1 && member.Name.IndexOf("IUSR") == -1 && member.Name.IndexOf("IWAM") == -1 && member.Name.IndexOf("DSC") == -1 && member.Name != "Administrator" && member.Name.IndexOf("DCS")==-1 && member.Name.IndexOf("SQL")==-1)
                        {
                            permissions.Add(new UserPermission(domainPath + "\\" + member.Name, false, false,false));
                        }
                    }
                    dataGridView1.DataSource = permissions;
                }
                catch (COMException e)
                {
                    // Console.WriteLine("Error: {0}", e.Message);
                }
                 * */
            }
            catch (COMException ex)
            {
                Core.Log.WriteLog(ex.ToString());
                MessageBox.Show(this,"Cannot retrieve users in domain. Please check network connectivity and try again.");
            }
        }

        private void RefreshData(string domainName)
        {
            string domainPath;
            try
            {
                domainPath = domainName;
                ListMembersInGroup("WinNT://", domainPath, "Domain Users");
                UpdateGridExistingPermissions();
            }
            catch
            {
                domainPath = domainName;
                ListMembersInGroup("LDAP://", domainPath, "Domain Users");
                UpdateGridExistingPermissions();
            }
        }

        private void RenameNode(object sender, EventArgs e)
        {
            treeWorkgroups.SelectedNode.BeginEdit();
        }

        private void SavePermissions()
        {
            if (lastWorkspace == null)
                return;
            if (permissions == null)
                return;
            foreach (TreeNode n in treeWorkgroups.Nodes)
            {
                Workspace ws = n.Tag as Workspace;
                if (ws.pkid == 0)
                {
                    DataRepository.Connections[provider].Provider.WorkspaceProvider.Save(ws);
                }
            }
            TList<User> users = DataRepository.Connections[provider].Provider.UserProvider.GetAll();
            foreach (CustomUserPermission uperm in permissions)
            {
                User correspondingUser = null;

                foreach (User usr in users)
                {
                    if (usr.Name == uperm.Username)
                    {
                        correspondingUser = usr;
                        //Break it here so it doesn't continue to loop through the rest of the users
                        break;
                    }
                }
                if (correspondingUser == null)
                {
                    correspondingUser = new User();
                    correspondingUser.Name = uperm.Username;
                    DataRepository.Connections[provider].Provider.UserProvider.Save(correspondingUser);
                    MetaBuilder.Core.Log.WriteLog("A new user was added (" + correspondingUser.Name + " : " + correspondingUser.pkid + ") from user permissions dialog on server: " + provider);
                }
                BusinessLogic.UserPermission upermission = new BusinessLogic.UserPermission();
                upermission.UserID = correspondingUser.pkid;
                upermission.WorkspaceTypeId = lastWorkspace.WorkspaceTypeId;
                upermission.WorkspaceName = lastWorkspace.Name;
                if (uperm.Admin)
                    upermission.PermissionID = 3;
                else if (uperm.Write)
                    upermission.PermissionID = 2;
                else if (uperm.Read)
                    upermission.PermissionID = 1;
                // Use the server - permissions arent applicable to client workspaces [at this stage]
                UserPermissionKey key = new UserPermissionKey(upermission);
                DataRepository.Connections[provider].Provider.UserPermissionProvider.Delete(key);
                if (uperm.Read || uperm.Write || uperm.Admin)
                {
                    DataRepository.Connections[provider].Provider.UserPermissionProvider.Save(upermission);
                }
            }
        }

        private void SelectedWorkGroupChanged(object sender, EventArgs e)
        {
        }

        void textFinder_FoundMatch(object sender, EventArgs e)
        {
            string row = sender.ToString();
            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                if (r.Cells[0].Value.ToString() == row.ToString())
                {
                    dataGridView1.CurrentCell = r.Cells[0];
                }
            }
        }

        private void treeWorkgroups_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            Workspace ws = e.Node.Tag as Workspace;
            string oldname = ws.Name;
            if (e.Label != null)
            {
                ws.Name = e.Label;
                ws.WorkspaceTypeId = (provider == Core.Variables.Instance.ClientProvider) ? (int)WorkspaceTypeList.Client : (int)WorkspaceTypeList.Server;
                if (ws.Name.Length > 0)
                {
                    try
                    {
                        DataRepository.Connections[provider].Provider.WorkspaceProvider.Save(ws);
                    }
                    catch
                    {
                        MessageBox.Show(this,"Workspace is not empty. Create a new one and transfer the contents", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.CancelEdit = true;
                        e.Node.Text = oldname;
                    }
                }
                else
                {
                    MessageBox.Show(this,"Workspaces cannot have empty names", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.CancelEdit = true;
                    e.Node.BeginEdit();
                }
            }
        }

        private void treeWorkgroups_AfterSelect(object sender, TreeViewEventArgs e)
        {
            lastWorkspace = e.Node.Tag as Workspace;
            UpdateGridExistingPermissions();
        }

        private void treeWorkgroups_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (isDirty)
            {
                PromptForSave();
                isDirty = false;
            }
        }

        private void treeWorkgroups_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                if (treeWorkgroups.SelectedNode != null)
                {
                    if (treeWorkgroups.SelectedNode.Text != "Sandbox")
                        treeWorkgroups.SelectedNode.BeginEdit();
                }
            }
        }

        private void treeWorkgroups_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point pt = new Point(e.X, e.Y);
                treeWorkgroups.PointToClient(pt);
                TreeNode Node = treeWorkgroups.GetNodeAt(pt);
                if (Node == null) return;
                if (Node.Bounds.Contains(pt))
                {
                    treeWorkgroups.SelectedNode = Node;
                    // cannot change SANDBOX workspace
                    Workspace ws = Node.Tag as Workspace;
                    if (!(
                             (provider == Core.Variables.Instance.ClientProvider && ws.WorkspaceTypeId == (int)WorkspaceTypeList.Server)
                             || ws.WorkspaceTypeId == (int)WorkspaceTypeList.Sandbox
                             || ws.WorkspaceTypeId == (int)WorkspaceTypeList.Template
                         ))
                    {
                        ContextMenu cm = new ContextMenu();
                        cm.Tag = Node;
                        //Removed as per request
                        // cm.MenuItems.Add(new MenuItem("Rename", new EventHandler(RenameNode)));
                        cm.MenuItems.Add(new MenuItem("Delete", new EventHandler(DeleteNode)));
                        cm.Show(treeWorkgroups, pt);
                    }
                }
                /*  Point clickPoint = e.Location;
                TreeNode node = treeWorkgroups.GetNodeAt(treeWorkgroups.PointToClient(clickPoint));
                
                if (node != null)
                {
                    MessageBox.Show(this,"node found" + node.Text);
                }*/
            }
        }

        private void UpdateGridExistingPermissions()
        {
            if (permissions != null && lastWorkspace != null)
            {
                TList<BusinessLogic.UserPermission> existingPermissions = DataRepository.Connections[provider].Provider.UserPermissionProvider.GetByWorkspaceNameWorkspaceTypeId(lastWorkspace.Name, lastWorkspace.WorkspaceTypeId);
                TList<User> users = DataRepository.Connections[provider].Provider.UserProvider.GetAll();
                foreach (CustomUserPermission uperm in permissions)
                {
                    uperm.Read = false;
                    uperm.Write = false;
                    uperm.Admin = false;
                    foreach (BusinessLogic.UserPermission userPermission in existingPermissions)
                    {
                        foreach (User user in users)
                        {
                            if (user.pkid == userPermission.UserID)
                            {
                                if (uperm.Username == user.Name)
                                {
                                    if (userPermission.PermissionID == 2)
                                    {
                                        uperm.Write = true;
                                        uperm.Read = false;
                                        uperm.Admin = false;
                                    }
                                    else if (userPermission.PermissionID == 1)
                                    {
                                        uperm.Read = true;
                                        uperm.Write = false;
                                        uperm.Admin = false;
                                    }
                                    else if (userPermission.PermissionID == 3)
                                    {
                                        uperm.Read = false;
                                        uperm.Write = false;
                                        uperm.Admin = true;
                                    }
                                }
                            }
                        }
                    }
                }
                dataGridView1.Refresh();
            }

            dataGridView1.Enabled = (lastWorkspace != null);
        }

        private void UserPermissions_Load(object sender, EventArgs e)
        {
            if (!authenticated)
            {
                PromptForPassword pfp = new PromptForPassword();
                pfp.HashedPassword = "cwRkEyzlTZ1bEquScqWGTA==";
                DialogResult result = pfp.ShowDialog(this);
                if (result == DialogResult.OK && pfp.Authenticated)
                {
                    try
                    {
                        BindWorkspaces();
                        Authenticated = true;
                        Application.Idle += new EventHandler(Application_Idle);
                        btnList_Click(sender, e);
                    }
                    catch
                    {
                        MessageBox.Show(this,"Cannot connect to server - please check your Server Connection settings and try again", "Connection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                    }
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                BindWorkspaces();
            }
        }

        #endregion Methods

    }

    public class CustomUserPermission
    {

        #region Fields (4)

        private bool admin;
        private bool read;
        private string username;
        private bool write;

        #endregion Fields

        #region Constructors (1)

        public CustomUserPermission(string name, bool read, bool write, bool admin)
        {
            username = name;
            Read = read;
            Write = write;
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

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public bool Write
        {
            get { return write; }
            set { write = value; }
        }

        #endregion Properties

    }
}