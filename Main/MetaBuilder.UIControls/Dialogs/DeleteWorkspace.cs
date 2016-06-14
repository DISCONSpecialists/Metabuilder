using System;
using System.Windows.Forms;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.DataAccessLayer.OldCode.Tools;
using MetaBuilder.Meta;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.UIControls.Dialogs
{
    public partial class DeleteWorkspace : Form
    {

        #region Fields (2)

        private string provider;
        private Workspace workspaceToBeDeleted;

        #endregion Fields

        #region Constructors (1)

        public DeleteWorkspace()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties (2)

        public string Provider
        {
            get { return provider; }
            set { provider = value; }
        }

        public Workspace WorkspaceToBeDeleted
        {
            get { return workspaceToBeDeleted; }
            set { workspaceToBeDeleted = value; }
        }

        #endregion Properties

        #region Methods (7)

        // Private Methods (7) 

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (radioPermanentlyDelete.Checked)
            {
                DoDelete();
            }
            else
            {
                Workspace targetWorkspace = comboWorkspaces.SelectedItem as Workspace;
                if (targetWorkspace != null)
                    DeleteTheWorkspace(workspaceToBeDeleted, targetWorkspace);
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void DeleteTheWorkspace(Workspace tobedeleted, Workspace targetWorkspace)
        {
            //string tempConnectionString;
            //string previousConnectionString = Variables.Instance.ConnectionString;
            //if (provider == Core.Variables.Instance.ClientProvider)
            //{
            //    tempConnectionString = Variables.Instance.ConnectionString;
            //}
            //else
            //{
            //    tempConnectionString = Variables.Instance.ServerConnectionString;
            //}
            //Variables.Instance.ConnectionString = tempConnectionString;
            int? targettypeid = null;
            string targetname = null;
            if (targetWorkspace != null)
            {
                //do proper transfer not some sql query :)
                MetaBuilder.BusinessFacade.Storage.WorkspaceHelper.Transfer(tobedeleted, targetWorkspace, Provider);

                //targettypeid = targetWorkspace.WorkspaceTypeId;
                //targetname = targetWorkspace.Name;
            }
            WorkspaceAdapter.DeleteWorkspace(tobedeleted.WorkspaceTypeId, tobedeleted.Name, targetname, targettypeid, Provider);
            //Variables.Instance.ConnectionString = previousConnectionString;
            if (Core.Variables.Instance.WorkspaceHashtable.ContainsKey(tobedeleted.Name + "#" + tobedeleted.WorkspaceTypeId))
            {
                Core.Variables.Instance.WorkspaceHashtable.Remove(tobedeleted.Name + "#" + tobedeleted.WorkspaceTypeId);
            }
            Loader.FlushDataViews();
        }

        private void DeleteWorkspace_Load(object sender, EventArgs e)
        {
            if (workspaceToBeDeleted.WorkspaceTypeId == (int)WorkspaceTypeList.Server && Provider != Core.Variables.Instance.ServerProvider)
            {
                MessageBox.Show(this, "Please note that this is a Server Workspace. You may continue, but all items currently checked out to you will need to be Checked In (Forced) by the Workspace Administrator", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            //else
            //{
            //    //data check
            //    if (d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.MetaObjectProvider.GetByWorkspaceNameWorkspaceTypeId().Count > 0)
            //    {
            //        MessageBox.Show(this,"You cannot delete this workspace because there is still data contained within it.", "Unable to Delete", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //        Close();
            //    }
            //}
            lblDeleteWorkspace.Text = "Workspace to be deleted: " + workspaceToBeDeleted.Name;
            TList<Workspace> otherWorkspaces = DataRepository.Connections[Provider].Provider.WorkspaceProvider.GetAll();
            comboWorkspaces.Items.Clear();
            foreach (Workspace ws in otherWorkspaces)
            {
                if (ws.pkid != workspaceToBeDeleted.pkid)
                {
                    comboWorkspaces.Items.Add(ws);
                }
            }
            comboWorkspaces.DisplayMember = "Name";
            comboWorkspaces.ValueMember = "pkid";

            btnOK.Enabled = false;
            radioMoveToWorkspace.Checked = false;
            radioPermanentlyDelete.Checked = false;
        }

        private void DoDelete()
        {
            if (Provider == Core.Variables.Instance.ServerProvider)
            {
                if (d.DataRepository.Connections[Provider].Provider.MetaObjectProvider.GetByWorkspaceNameWorkspaceTypeId(WorkspaceToBeDeleted.Name, WorkspaceToBeDeleted.WorkspaceTypeId).Count > 0)
                {
                    MessageBox.Show(this, "You cannot delete this workspace because it still has data in it.", "Workspace data cannot be removed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

            }
            DeleteTheWorkspace(WorkspaceToBeDeleted, null);
        }

        private void radioMoveToWorkspace_Click(object sender, EventArgs e)
        {
            comboWorkspaces.SelectedItem = null;
            radioMoveToWorkspace.Checked = true;
            radioPermanentlyDelete.Checked = false;
            txtDelete.Text = "";
            txtDelete.Enabled = false;
            btnOK.Enabled = ((comboWorkspaces.SelectedItem != null) && comboWorkspaces.SelectedItem is Workspace);
        }

        private void radioPermanentlyDelete_Click(object sender, EventArgs e)
        {
            txtDelete.Text = "";
            radioPermanentlyDelete.Checked = true;
            radioMoveToWorkspace.Checked = false;
            txtDelete.Enabled = true;
            btnOK.Enabled = txtDelete.Text == "DELETE";
        }

        #endregion Methods

        private void txtDelete_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = txtDelete.Text == "DELETE";
        }

        private void comboWorkspaces_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = ((comboWorkspaces.SelectedItem != null) && comboWorkspaces.SelectedItem is Workspace);
        }

    }
}