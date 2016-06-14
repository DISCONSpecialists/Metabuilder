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
    public partial class ChooseWorkspaceForAction : Form
    {

		#region Fields (2) 

        private Workspace sandbox;
        private b.Workspace selectedWorkspace;

		#endregion Fields 

		#region Constructors (1) 

        public ChooseWorkspaceForAction()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.ShowIcon = false;
        }

		#endregion Constructors 

		#region Properties (2) 

        public Workspace Sandbox
        {
            get { return sandbox; }
            set { sandbox = value; }
        }

        public b.Workspace SelectedWorkspace
        {
            get { return selectedWorkspace; }
            set { selectedWorkspace = value; }
        }

		#endregion Properties 

		#region Methods (6) 


		// Public Methods (1) 

        public Workspace GetSelectedWorkspace()
        {
            TreeNode n = treeView1.SelectedNode;
            Workspace ws = n.Tag as Workspace;
            return ws;
        }



		// Private Methods (5) 

        private void btnDefault_Click(object sender, EventArgs e)
        {
            // use Sandbox
            SelectedWorkspace = sandbox;
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                TreeNode node = treeView1.SelectedNode;
            }
            try
            {
                SelectedWorkspace = GetSelectedWorkspace();
                this.DialogResult = DialogResult.OK;
                Close();
            }
            catch
            {
                MessageBox.Show(this,"Select a workspace", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ChooseWorkspace_Load(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            TList<Workspace> workspaces = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.WorkspaceProvider.GetAll();
            foreach (Workspace ws in workspaces)
            {
                TreeNode node = new TreeNode(ws.Name);
                node.Tag = ws;
                treeView1.Nodes.Add(node);
                if (ws.Name == "Sandbox")
                    sandbox = ws;

                if (ws.WorkspaceTypeId == 3)
                    node.StateImageIndex = 0;
                else
                    node.StateImageIndex = 1;

            }
            if (treeView1.Nodes.Count > 0)
            {
                treeView1.SelectedNode = treeView1.Nodes[0];
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

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Workspace ws = GetWorkspaceAtCoordinates(e);
            if (ws!=null)
            {
                SelectedWorkspace = ws;
                this.DialogResult = DialogResult.OK;
                Close();
            }

        }


		#endregion Methods 

    }
}