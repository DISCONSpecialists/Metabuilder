using System;
using System.Windows.Forms;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Meta;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.UIControls.MetaTree
{
    [Serializable]
    public class ClassAssociationCollectionNode : MetaTreeNode
    {

		#region Fields (1) 

        private ClassAssociation _classAssociation;

		#endregion Fields 

		#region Properties (4) 

        public ClassAssociation ClassAssociation
        {
            get { return _classAssociation; }
            set { _classAssociation = value;
            Text = ((LinkAssociationType)Enum.Parse(typeof(LinkAssociationType), _classAssociation.AssociationTypeID.ToString())).ToString() + " to " + _classAssociation.ChildClass;
        }
        }

        public override ContextMenuStrip ContextMenuStrip
        {
            get
            {
                if (ClassAssociation != null)
                {
                    ContextMenuStrip cmStrip = new ContextMenuStrip();
                    ToolStripMenuItem mitemAddNew = new ToolStripMenuItem();
                    mitemAddNew.Text = "Add new " + ClassAssociation.ChildClass;
                    mitemAddNew.Click += mnuAddNewObject_Click;
                    ToolStripMenuItem mitemAddExisting = new ToolStripMenuItem();
                    mitemAddExisting.Text = "Add existing " + ClassAssociation.ChildClass;
                    mitemAddExisting.Click += mnuAddExistingObject_Click;
                    cmStrip.Items.Add(mitemAddNew);
                    cmStrip.Items.Add(mitemAddExisting);
                    return cmStrip;
                }
                else
                    return base.ContextMenuStrip;
            }
            set { base.ContextMenuStrip = value; }
        }

        public string ParentMachine
        {
            get
            {
                ObjectNode parentObjectNode = Parent as ObjectNode;
                return parentObjectNode.MetaObject.MachineName;
            }
        }

        // helper property
        public int ParentObjectID
        {
            get
            {
                ObjectNode parentObjectNode = Parent as ObjectNode;
                return parentObjectNode.MetaObject.pkid;
            }
        }

		#endregion Properties 

		#region Methods (4) 


		// Public Methods (1) 

        public override void LoadChildren()
        {
            Nodes.Clear();
            TList<ObjectAssociation> associations = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(ParentObjectID, ParentMachine);
            foreach (ObjectAssociation objectAssociation in associations)
            {
                if (objectAssociation.CAid == ClassAssociation.CAid)
                {
                    ObjectNode node = new ObjectNode();
                    node.MetaObject = Loader.GetByID(ClassAssociation.ChildClass,objectAssociation.ChildObjectID,objectAssociation.ChildObjectMachine);
                    node.Nodes.Add(new EmptyNode());
                    Nodes.Add(node);
                }
            }
            ClearEmptyNodes();
        }



		// Protected Methods (2) 

        protected void mnuAddExistingObject_Click(object sender, EventArgs e)
        {
            /* Code het gekak met skoonmaak 1/maart/2007 -> hierdie word nerens gebruik nie
            MetaBuilder.UIControls.Common.OpenExisting openExisting = new MetaBuilder.UIControls.Common.OpenExisting();
            openExisting.ClassName = ClassAssociation.ChildClass;
            openExisting.SelectionChanged += new EventHandler(openExisting_SelectionChanged);
            openExisting.AllowMultipleSelection = true;
            openExisting.ShowDialog(this);
            ClearEmptyNodes();*/
        }

        protected void mnuAddNewObject_Click(object sender, EventArgs e)
        {
            ObjectNode node = new ObjectNode();
            node.MetaObject = Loader.CreateInstance(ClassAssociation.ChildClass);
            node.LoadChildren();
            Nodes.Add(node);
            ClearEmptyNodes();
        }



		// Private Methods (1) 

        private void openExisting_SelectionChanged(object sender, EventArgs e)
        {
            /* Code het gekak met skoonmaak 1/maart/2007 -> hierdie word nerens gebruik nie
            MetaBuilder.UIControls.Common.OpenExisting openExisting = sender as MetaBuilder.UIControls.Common.OpenExisting;
            if (openExisting.SelectedItems.Count > 0)
            {
                foreach (Meta.IMetaBase imb in openExisting.SelectedItems)
                {
                    ObjectNode node = new ObjectNode();
                    node.MetaObject = (Meta.MetaBase)imb;
                    node.LoadChildren();
                    Nodes.Add(node);
                }
            }*/
        }


		#endregion Methods 

    }
}