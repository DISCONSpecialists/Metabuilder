using System;
using System.Drawing;
using System.Windows.Forms;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.UIControls.GraphingUI;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.UIControls.Dialogs
{
    public partial class ChooseWorkspace : Form
    {

        #region Fields (1)

        private Workspace _default;

        #endregion Fields

        #region Constructors (1)
        MetaSettings s = new MetaSettings();
        bool Server = false;
        public ChooseWorkspace(bool server)
        {
            InitializeComponent();
            Server = server;
            this.ShowInTaskbar = false;
            this.ShowIcon = false;
            if (!Server)
            {
                _default = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.WorkspaceProvider.GetByNameWorkspaceTypeId("SandBox", 1);
                buttonCancel.Enabled = true; //(!string.IsNullOrEmpty(s.GetSetting(MetaSettings.DEFAULT_WORKSPACE, "")) && s.GetSetting(MetaSettings.DEFAULT_WORKSPACE_ID, -1) > 0);
            }
            else
            {
                btnOK.Text = "Close";
                buttonCancel.Visible = true;
                btnDefault.Visible = false;
            }

            this.Text += Server ? " (Server)" : " Client";
        }

        #endregion Constructors

        #region Properties (1)

        public Workspace _Default
        {
            get { return _default; }
            set { _default = value; }
        }
        public string Provider { get { return Server ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider; } }
        #endregion Properties

        #region Methods (10)

        // Public Methods (1) 

        public Workspace GetSelectedWorkspace()
        {
            TreeNode n = treeView1.SelectedNode;
            Workspace ws = n.Tag as Workspace;
            return ws;
        }

        // Private Methods (9) 

        private void btnDefault_Click(object sender, EventArgs e)
        {
            //set variables to default selected workspace
            //Variables.Instance.CurrentWorkspaceTypeId = GetSelectedWorkspace().WorkspaceTypeId;
            //Variables.Instance.CurrentWorkspaceName = GetSelectedWorkspace().Name;
            Variables.Instance.DefaultWorkspace = GetSelectedWorkspace().Name;
            Variables.Instance.DefaultWorkspaceID = GetSelectedWorkspace().WorkspaceTypeId;

            s.PutSetting(MetaSettings.DEFAULT_WORKSPACE, Variables.Instance.DefaultWorkspace);
            s.PutSetting(MetaSettings.DEFAULT_WORKSPACE_ID, Variables.Instance.DefaultWorkspaceID);

            ChooseWorkspace_Load(sender, e);
            //DockingForm.DockForm.SetToolstripToGlobalVarWorkspace(null);
            //this.DialogResult = DialogResult.OK;
            //Close();

            // use Sandbox
            //Variables.Instance.CurrentWorkspaceName = sandbox.Name;
            //Variables.Instance.CurrentWorkspaceTypeId = sandbox.WorkspaceTypeId;
            //DockingForm.DockForm.SetToolstripToGlobalVarWorkspace(null);
            //this.DialogResult = DialogResult.OK;
            //Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            doubleClickedNodeWorkspace = null;
            TreeNode newNode = new TreeNode();
            newNode.StateImageIndex = Server ? 0 : 1;
            newNode.Text = "New Workspace";
            treeView1.Nodes.Add(newNode);
            treeView1.SelectedNode = newNode;
            newNode.BeginEdit();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                TreeNode node = treeView1.SelectedNode;
                if (node.IsEditing)
                    node.EndEdit(false);
                if (node.Text == "New Workspace")
                {
                    MessageBox.Show(this, "The name 'New Workspace' is not allowed. Please rename and try again", "Workspace Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            //try
            //{
            if (!Server)
            {
                Variables.Instance.CurrentWorkspaceTypeId = GetSelectedWorkspace().WorkspaceTypeId;
                Variables.Instance.CurrentWorkspaceName = GetSelectedWorkspace().Name;
                DockingForm.DockForm.SetToolstripToGlobalVarWorkspace(null);
            }
            else
            {
                if (addedNew)
                    MessageBox.Show(this, "New server workspaces were added which do not have permissions setup in order for users to begin using them", "Workspace Permissions", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            this.DialogResult = DialogResult.OK;
            Close();
            //}
            //catch
            //{
            //    MessageBox.Show(this,"Select a workspace", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
        }

        private void ChooseWorkspace_Load(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            TList<Workspace> workspaces = DataRepository.Connections[Provider].Provider.WorkspaceProvider.GetAll();

            foreach (Workspace ws in workspaces)
            {
                string d = "";
                if (!Server)
                {
                    if (Variables.Instance.DefaultWorkspace.Length > 0)
                    {
                        if (ws.Name == Variables.Instance.DefaultWorkspace && ws.WorkspaceTypeId == Variables.Instance.DefaultWorkspaceID)
                        {
                            d = " <Default>";
                        }
                    }
                }

                TreeNode node = new TreeNode(ws.Name + d);
                node.Tag = ws;
                treeView1.Nodes.Add(node);
                if (ws.Name == s.GetSetting(MetaSettings.DEFAULT_WORKSPACE, ""))
                    _default = ws;

                if (ws.WorkspaceTypeId == 3)
                    node.StateImageIndex = 0;
                else
                    node.StateImageIndex = 1;

                if (!Core.Variables.Instance.WorkspaceHashtable.ContainsKey(ws.Name + "#" + ws.WorkspaceTypeId))
                {
                    Core.Variables.Instance.WorkspaceHashtable.Add(ws.Name + "#" + ws.WorkspaceTypeId, null);
                }
            }
            if (_default != null)
            {
                foreach (TreeNode n in treeView1.Nodes)
                {
                    if (n.Tag == _default)
                    {
                        treeView1.SelectedNode = n;
                        return;
                    }
                }
            }
        }

        private void deleteWorkspaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Workspace ws = treeView1.SelectedNode.Tag as Workspace;
            DeleteWorkspace delForm = new DeleteWorkspace();
            delForm.WorkspaceToBeDeleted = ws;
            delForm.Provider = Provider;
            DialogResult res = delForm.ShowDialog(this);

            if (res == DialogResult.OK)
            {
                if (!Server)
                {
                    if (Core.Variables.Instance.CurrentWorkspaceName == delForm.WorkspaceToBeDeleted.Name && Core.Variables.Instance.CurrentWorkspaceTypeId == delForm.WorkspaceToBeDeleted.WorkspaceTypeId)
                    {
                        //set active workspace to default or sandbox if no default unless this was the default in which set default to sandbox
                        if (Variables.Instance.DefaultWorkspace.Length > 0)
                        {
                            if (Core.Variables.Instance.CurrentWorkspaceName != Core.Variables.Instance.DefaultWorkspace && Core.Variables.Instance.CurrentWorkspaceTypeId != Core.Variables.Instance.DefaultWorkspaceID)
                            {
                                Core.Variables.Instance.CurrentWorkspaceName = Core.Variables.Instance.DefaultWorkspace;
                                Core.Variables.Instance.CurrentWorkspaceTypeId = Core.Variables.Instance.DefaultWorkspaceID;
                            }
                            else
                            {
                                Core.Variables.Instance.CurrentWorkspaceName = "Sandbox";
                                Core.Variables.Instance.CurrentWorkspaceTypeId = 1;
                                Core.Variables.Instance.DefaultWorkspace = "Sandbox";
                                Core.Variables.Instance.DefaultWorkspaceID = 1;
                            }
                        }
                        else
                        {
                            Core.Variables.Instance.CurrentWorkspaceName = "Sandbox";
                            Core.Variables.Instance.CurrentWorkspaceTypeId = 1;
                        }
                        DockingForm.DockForm.SetToolstripToGlobalVarWorkspace(null);
                    }
                }
                ChooseWorkspace_Load(sender, e);
            }
        }

        private Workspace GetWorkspaceAtCoordinates(MouseEventArgs e)
        {
            Point pt = new Point(e.X, e.Y);
            treeView1.PointToClient(pt);
            TreeNode Node = treeView1.GetNodeAt(pt);
            if (Node == null) return null;
            if (Node.Bounds.Contains(pt))
            {
                treeView1.SelectedNode = Node;
                // cannot delete SANDBOX workspace
                Workspace ws = Node.Tag as Workspace;
                return ws;
            }
            return null;
        }

        bool addedNew = false;
        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            try
            {
                if (doubleClickedNodeWorkspace != null && e.Label != null && doubleClickedNodeWorkspace.Name != e.Label)
                {
                    //remove old hash
                    if (e.Label.Length > 0)
                    {
                        Core.Variables.Instance.WorkspaceHashtable.Remove(doubleClickedNodeWorkspace.Name + "#" + doubleClickedNodeWorkspace.WorkspaceTypeId);
                        doubleClickedNodeWorkspace.Name = e.Label;
                        //save a new workspace and set everything inside it to the new name then delete the doubleclicked one

                        DataRepository.Connections[Provider].Provider.WorkspaceProvider.Save(doubleClickedNodeWorkspace);
                        if (!Server)
                            if (!(Core.Variables.Instance.WorkspaceHashtable.Contains(doubleClickedNodeWorkspace.Name + "#" + doubleClickedNodeWorkspace.WorkspaceTypeId)))
                                Core.Variables.Instance.WorkspaceHashtable.Add(doubleClickedNodeWorkspace.Name + "#" + doubleClickedNodeWorkspace.WorkspaceTypeId, null);
                        e.Node.Tag = doubleClickedNodeWorkspace;
                    }
                    else
                    {
                        MessageBox.Show(this, "Workspaces cannot have empty names", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.CancelEdit = true;
                        e.Node.BeginEdit();
                    }
                }
                else
                {
                    Workspace ws = new Workspace();
                    if (e.Label != null)
                    {
                        ws.Name = e.Label;
                        if (ws.Name.Length > 0)
                        {
                            ws.WorkspaceTypeId = Server ? (int)WorkspaceTypeList.Server : (int)WorkspaceTypeList.Client;
                            ws.IsActive = true;

                            addedNew = true;

                            DataRepository.Connections[Provider].Provider.WorkspaceProvider.Save(ws);
                            if (!Server)
                                if (!(Core.Variables.Instance.WorkspaceHashtable.Contains(ws.Name + "#" + ws.WorkspaceTypeId)))
                                    Core.Variables.Instance.WorkspaceHashtable.Add(ws.Name + "#" + ws.WorkspaceTypeId, null);
                            e.Node.Tag = ws;
                        }
                        else
                        {
                            MessageBox.Show(this, "Workspaces cannot have empty names", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            e.CancelEdit = true;
                            e.Node.BeginEdit();
                        }
                    }
                }
            }
            catch (System.InvalidCastException ICex)
            {
                string t = e.Label;
                e.CancelEdit = true;
                //MessageBox.Show(this,"This workspace's name could not be changed because a workspace with the same name already exists.", "Cannot Change Workspace Name", MessageBoxButtons.OK, MessageBoxIcon.Information);

                foreach (TreeNode n in treeView1.Nodes)
                    if (n.Tag is Workspace)
                        if ((n.Tag as Workspace).Name == t && (n.Tag as Workspace).WorkspaceTypeId == (Server ? 3 : 2))
                        {
                            treeView1.Nodes.Remove(e.Node);
                            treeView1.SelectedNode = n;
                            break;
                        }
            }
            catch (Exception ex)
            {
                //could not save workspace name because it is bound to other objects already.
                Log.WriteLog(ex.ToString());
                e.CancelEdit = true;
                MessageBox.Show(this, "This workspace's name could not be changed because there is already data within it.", "Cannot Change Workspace Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            doubleClickedNodeWorkspace = null;
        }

        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Workspace ws = GetWorkspaceAtCoordinates(e);
                if (ws != null) // check that its not New Workspace node
                {
                    //9 October 2013
                    if (ws.WorkspaceTypeId != 1)
                    {
                        Point pt = new Point(e.X, e.Y);
                        contextMenuStrip1.Show(treeView1, pt);
                    }
                }
            }
        }
        private Workspace doubleClickedNodeWorkspace = null;
        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            doubleClickedNodeWorkspace = e.Node.Tag as Workspace;
            if (doubleClickedNodeWorkspace.Name != "Sandbox" && doubleClickedNodeWorkspace.WorkspaceTypeId != 1)
                e.Node.BeginEdit();
        }

        #endregion Methods

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}