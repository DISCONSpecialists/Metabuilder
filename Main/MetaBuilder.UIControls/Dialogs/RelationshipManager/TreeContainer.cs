using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Docking;
using MetaBuilder.Meta;
using MetaBuilder.UIControls.GraphingUI;
using dck = MetaBuilder.Docking;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.UIControls.Dialogs.RelationshipManager
{
    public delegate void DBTreeEventHandler(object sender, DBTreeEventArgs args);
    public partial class TreeContainer : DockContent
    {

        #region Fields (1)

        private bool isAddingBatch;

        #endregion Fields

        #region Constructors (1)

        public TreeContainer()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties (2)

        public bool IsAddingBatch
        {
            get { return isAddingBatch; }
            set { isAddingBatch = value; }
        }

        public TreeNodeCollection Nodes
        {
            get { return tree.Nodes; }
        }

        #endregion Properties

        #region Delegates and Events (3)

        // Events (3) 

        public event EventHandler ItemListBeginUpdate;

        public event EventHandler ItemListUpdated;

        public event DBTreeEventHandler SelectedItemsChanged;

        #endregion Delegates and Events

        #region Methods (6)

        // Public Methods (1) 

        public ObjectNode AddNode(MetaBase mb)
        {
            foreach (TreeNode treenode in tree.Nodes)
            {
                ObjectNode objnode = treenode as ObjectNode;
                MetaBase mbTag = objnode.Tag as MetaBase;

                if (mbTag.pkid == mb.pkid && mbTag.MachineName == mb.MachineName)
                    return objnode;
            }
            ObjectNode node = new ObjectNode();
            node.Tag = mb;
            node.Text = mb.ToString();
            tree.Nodes.Add(node);
            node.Checked = true;
            return node;
        }

        // Protected Methods (3) 

        protected void OnItemListBeginUpdate(object sender, EventArgs e)
        {
            if (ItemListBeginUpdate != null)
                ItemListBeginUpdate(sender, e);
        }

        protected void OnItemListUpdated(object sender, EventArgs e)
        {
            if (ItemListUpdated != null)
                ItemListUpdated(sender, e);
        }

        protected void OnSelectedItemsChanged(object sender, TreeViewEventArgs e)
        {
            if (SelectedItemsChanged != null)
            {
                DBTreeEventArgs args = new DBTreeEventArgs();
                args.Node = e.Node;
                args.IsAddingBatch = isAddingBatch;
                SelectedItemsChanged(sender, args);
            }
        }

        // Private Methods (2) 

        public void addItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnItemListBeginUpdate(sender, e);
            IsAddingBatch = true;
            ObjectFinder ofinder = new ObjectFinder(false);
            ofinder.Text += " " + TabText;
            ofinder.IncludeStatusCombo = true;
            ofinder.ExcludeStatuses.Add(VCStatusList.Obsolete);
            ofinder.ExcludeStatuses.Add(VCStatusList.MarkedForDelete);

            DialogResult result = ofinder.ShowDialog(this);
            MenuItem m_Sender = sender as MenuItem;
            ObjectNode lastNode = null;
            if (result == DialogResult.OK)
            {
                foreach (MetaBase mb in ofinder.SelectedObjects.Values)
                {
                    ObjectNode node = AddNode(mb);
                    lastNode = node;
                }
            }
            IsAddingBatch = false;
            if (lastNode != null)
                lastNode.Checked = true;
            OnItemListUpdated(sender, e);
        }

        private void tree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (!IsAddingBatch)
                OnSelectedItemsChanged(sender, e);
        }

        #endregion Methods

    }
}