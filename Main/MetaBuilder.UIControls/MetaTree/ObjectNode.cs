using System;
using System.Drawing;
using System.Windows.Forms;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Meta;
using m = MetaBuilder.Meta;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
using ObjectAssociation=MetaBuilder.BusinessLogic.ObjectAssociation;

namespace MetaBuilder.UIControls.MetaTree
{
    [Serializable]
    public class ObjectNode : MetaTreeNode
    {

		#region Fields (1) 

        private MetaBase _metaObject;

		#endregion Fields 

		#region Constructors (1) 

        public ObjectNode()
        {
            // MetaObject = new MetaBuilder.Meta.MetaBase();
        }

		#endregion Constructors 

		#region Properties (2) 

        public override ContextMenuStrip ContextMenuStrip
        {
            get
            {
                if (MetaObject != null)
                {
                    ContextMenuStrip cmStrip = new ContextMenuStrip();
                    ToolStripMenuItem mItemSave = new ToolStripMenuItem();
                    mItemSave.Text = "Save";
                    mItemSave.Click += mnuSave_Click;
                    cmStrip.Items.Add(mItemSave);
                    // check for a parent...
                    if (Parent is ClassAssociationCollectionNode)
                    {
                        ClassAssociationCollectionNode classAssocParent = Parent as ClassAssociationCollectionNode;
                        if (Parent.Parent is ObjectNode)
                        {
                            ToolStripMenuItem mItemDeleteAssociation = new ToolStripMenuItem();
                            mItemDeleteAssociation.Text = "Remove Association with " +
                                                          (Parent.Parent as ObjectNode).MetaObject.ToString();
                            mItemDeleteAssociation.Click += mItemDeleteAssociation_Click;
                            cmStrip.Items.Add(mItemDeleteAssociation);
                        }
                    }
                    ToolStripMenuItem mItemDelete = new ToolStripMenuItem();
                    mItemDelete.Text = "Delete";
                    mItemDelete.Click += mnuDelete_Click;
                    cmStrip.Items.Add(mItemDelete);
                    return cmStrip;
                }
                else
                    return base.ContextMenuStrip;
            }
            set { base.ContextMenuStrip = value; }
        }

        public virtual MetaBase MetaObject
        {
            get { return _metaObject; }
            set
            {
                bool FirstAccess = false; // only hookup changed event once
                if (_metaObject == null)
                    FirstAccess = true;
                _metaObject = value;
                if (FirstAccess)
                {
                    _metaObject.Changed += new EventHandler(_metaObject_Changed);
                }
                Text = _metaObject.ToString();
            }
        }

		#endregion Properties 

		#region Methods (6) 


		// Public Methods (1) 

        public override void LoadChildren()
        {
            Nodes.Clear();
            TList<ClassAssociation> classAssociations =                DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByParentClass(MetaObject._ClassName);
            foreach (ClassAssociation classAssociation in classAssociations)
            {
                ClassAssociationCollectionNode cacolnode = new ClassAssociationCollectionNode();
                cacolnode.ClassAssociation = classAssociation;
                Nodes.Add(cacolnode);
                cacolnode.LoadChildren();
                if (cacolnode.Nodes.Count == 0)
                {
                    Nodes.Remove(cacolnode);
                }
            }
        }



		// Protected Methods (3) 

        protected void mItemDeleteAssociation_Click(object sender, EventArgs e)
        {
            try
            {
                int CAid = 0;
                int ParentID = 0;
                if (Parent is ClassAssociationCollectionNode)
                {
                    ClassAssociationCollectionNode classAssocParent = Parent as ClassAssociationCollectionNode;
                    ObjectNode parentObjectNode = classAssocParent.Parent as ObjectNode;
                    CAid = classAssocParent.ClassAssociation.CAid;
                    ParentID = parentObjectNode.MetaObject.pkid;
                    DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Delete(CAid, ParentID, MetaObject.pkid, parentObjectNode.MetaObject.MachineName, MetaObject.MachineName);
                    classAssocParent.LoadChildren();
                }
            }
            catch
            {
            }
        }

        protected void mnuDelete_Click(object sender, EventArgs e)
        {
            DialogResult result =
                MessageBox.Show("Do you want to delete the object from the database?", "Delete Object",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Singletons.GetObjectHelper().DeleteObject(MetaObject.pkid, MetaObject.MachineName);
                if (Parent is ClassAssociationCollectionNode)
                {
                    ClassAssociationCollectionNode classAssocParent = Parent as ClassAssociationCollectionNode;
                    classAssocParent.LoadChildren();
                }
                else
                {
                    GraphNodeCollectionNode n = Parent as GraphNodeCollectionNode;
                    //Remove from the document too
                    DocumentNode docNode = n.Parent as DocumentNode;
                    ForeColor = Color.LightGray;
                    n.LoadChildren();
                }
            }
        }

        // helper property
        protected void mnuSave_Click(object sender, EventArgs e)
        {
            MetaObject.Save(Guid.NewGuid());
            ClassAssociationCollectionNode classAssocParent = Parent as ClassAssociationCollectionNode;
            if (classAssocParent != null)
            {
                // Sometimes an object lies on a diagram, but not in the database. In that case, save it first!
                ObjectNode parentObjectNode = classAssocParent.Parent as ObjectNode;
                if (parentObjectNode.MetaObject.pkid == 0)
                    parentObjectNode.MetaObject.Save(Guid.NewGuid());
                ObjectAssociation objAssoc = new ObjectAssociation();
                objAssoc.ChildObjectID = MetaObject.pkid;
                objAssoc.CAid = classAssocParent.ClassAssociation.CAid;
                objAssoc.ObjectID = classAssocParent.ParentObjectID;
                DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Save(objAssoc);
            }
            MetaObject._IsDirty = false;
            MarkDirtyIfNecessary();
        }



		// Private Methods (1) 

        private void MarkDirtyIfNecessary()
        {
            if (MetaObject._IsDirty)
            {
                Text = MetaObject.ToString() + " *";
            }
            else
            {
                Text = MetaObject.ToString();
            }
        }



		// Internal Methods (1) 

        internal void _metaObject_Changed(object sender, EventArgs e)
        {
            try
            {
                MarkDirtyIfNecessary();
            }
            catch
            {
            }
        }


		#endregion Methods 

    }
}