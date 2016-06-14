using System;
using System.Windows.Forms;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.Docking;

using MetaBuilder.Meta;
using MetaBuilder.UIControls.GraphingUI;

namespace MetaBuilder.UIControls.Dialogs.RelationshipManager
{
    public partial class RelmanContainer : Form
    {

        #region Fields (4)

        private ArtifactEditor artefactEditor = new ArtifactEditor();
        private TableContainer tableContainer = new TableContainer();
        private TreeContainer treeContainerLeft = new TreeContainer();
        private TreeContainer treeContainerRight = new TreeContainer();

        #endregion Fields

        #region Constructors (1)

        public RelmanContainer()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods (11)

        // Public Methods (1) 

        public void Refresh(int CAid, int ObjectID, int ChildObjectID, string objectmachine, string childmachine)
        {
            artefactEditor.SaveArtifacts();
            artefactEditor.ClearPropertyGrid();
            artefactEditor.Refresh(CAid, ObjectID, ChildObjectID, objectmachine, childmachine);
        }

        // Private Methods (10) 

        private void menuItemCalculateFunctionCriticality_Click(object sender, EventArgs e)
        {
            CSF.CalculateCriticalities();
        }

        private void RelmanContainer_FormClosing(object sender, FormClosingEventArgs e)
        {
            artefactEditor.SaveArtifacts();
        }

        private void RelmanContainer_Load(object sender, EventArgs e)
        {
            // these need to be set before refreshchanges is fired internally, rendering the X button even though it is disabled.
            artefactEditor.TabText = "Artefacts";
            tableContainer.TabText = "Associations";
            treeContainerLeft.TabText = "Rows (Parent Objects)";
            treeContainerRight.TabText = "Columns (Child Objects)";

            treeContainerLeft.Show(dockPanel1, DockState.DockLeftAutoHide);
            artefactEditor.Show(dockPanel1, DockState.DockRightAutoHide);
            treeContainerRight.Show(dockPanel1, DockState.DockRightAutoHide);
            tableContainer.Show(dockPanel1, DockState.Document);
            tableContainer.CloseButton = false;
            tableContainer.AllowEndUserDocking = false;
            artefactEditor.CloseButton = false;

            treeContainerLeft.CloseButton = false;
            treeContainerRight.CloseButton = false;

            tableContainer.Refresh();
            tableContainer.RowItemAdded += new EventHandler(tableContainer_RowItemAdded);
            tableContainer.ColumnItemAdded += new EventHandler(tableContainer_ColumnItemAdded);

            treeContainerLeft.SelectedItemsChanged += new DBTreeEventHandler(tableContainer.treeLeft_AfterCheck);
            treeContainerRight.SelectedItemsChanged += new DBTreeEventHandler(tableContainer.treeRight_AfterCheck);
            treeContainerLeft.ItemListUpdated += new EventHandler(treeContainerLeft_ItemListUpdated);
            treeContainerRight.ItemListUpdated += new EventHandler(treeContainerRight_ItemListUpdated);
            tableContainer.SelectedAssociationChanged += new AssociationEventHandler(tableContainer_SelectedAssociationChanged);
            treeContainerRight.ItemListBeginUpdate += new EventHandler(treeContainerRight_ItemListBeginUpdate);
            treeContainerLeft.ItemListBeginUpdate += new EventHandler(treeContainerLeft_ItemListBeginUpdate);
            WindowState = DockingForm.DockForm.WindowState;
            this.FormClosing += new FormClosingEventHandler(RelmanContainer_FormClosing);

            treeContainerLeft.addItemsToolStripMenuItem_Click(this, e);
            treeContainerRight.addItemsToolStripMenuItem_Click(this, e);
        }

        private void tableContainer_ColumnItemAdded(object sender, EventArgs e)
        {
            treeContainerRight.AddNode(sender as MetaBase);
        }

        private void tableContainer_RowItemAdded(object sender, EventArgs e)
        {
            treeContainerLeft.AddNode(sender as MetaBase);
        }

        private void tableContainer_SelectedAssociationChanged(object sender, int CAid, int ObjectID, int ChildObjectID, string ObjectMachine, string ChildObjectMachine)
        {
            Refresh(CAid, ObjectID, ChildObjectID, ObjectMachine, ChildObjectMachine);
        }

        private void treeContainerLeft_ItemListBeginUpdate(object sender, EventArgs e)
        {
            tableContainer.SuspendTableLayout();
        }

        private void treeContainerLeft_ItemListUpdated(object sender, EventArgs e)
        {
            tableContainer.AddRows(treeContainerLeft.Nodes);
            tableContainer.ResumeTableLayout();
        }

        private void treeContainerRight_ItemListBeginUpdate(object sender, EventArgs e)
        {
            tableContainer.SuspendTableLayout();
        }

        private void treeContainerRight_ItemListUpdated(object sender, EventArgs e)
        {
            tableContainer.AddColumns(treeContainerRight.Nodes);
            tableContainer.ResumeTableLayout();
        }

        #endregion Methods

    }
}