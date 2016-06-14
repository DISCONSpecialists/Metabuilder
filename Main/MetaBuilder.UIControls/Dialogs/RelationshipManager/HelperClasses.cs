using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Meta;
using dck = MetaBuilder.Docking;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
using ObjectAssociation=MetaBuilder.BusinessLogic.ObjectAssociation;

namespace MetaBuilder.UIControls.Dialogs.RelationshipManager
{
    public class DBTreeEventArgs
    {

		#region Fields (2) 

        private bool isAddingBatch;
        private TreeNode node;

		#endregion Fields 

		#region Properties (2) 

        public bool IsAddingBatch
        {
            get { return isAddingBatch; }
            set { isAddingBatch = value; }
        }

        public new TreeNode Node
        {
            get { return node; }
            set { node = value; }
        }

		#endregion Properties 

    }
    public class ObjectPairAssociationSpecification
    {

		#region Fields (2) 

        public ClassAssociation classAssociationDefinition;
        public ObjectPairAssociationCollection objectPair;

		#endregion Fields 

    }

    public class ObjectPairAssociationCollection
    {

		#region Fields (4) 

        private Dictionary<Association, MetaBase> artefacts;
        private List<MatrixObjectAssociation> associations;
        private MetaBase childObject;
        private MetaBase parentObject;

		#endregion Fields 

		#region Constructors (1) 

        public ObjectPairAssociationCollection()
        {
            associations = new List<MatrixObjectAssociation>();
            artefacts = new Dictionary<Association, MetaBase>();
        }

		#endregion Constructors 

		#region Properties (4) 

        public Dictionary<Association, MetaBase> Artefacts
        {
            get { return artefacts; }
            set { artefacts = value; }
        }

        public List<MatrixObjectAssociation> Associations
        {
            get { return associations; }
            set { associations = value; }
        }

        public MetaBase ChildObject
        {
            get { return childObject; }
            set { childObject = value; }
        }

        public MetaBase ParentObject
        {
            get { return parentObject; }
            set { parentObject = value; }
        }

		#endregion Properties 

		#region Methods (1) 


		// Public Methods (1) 

        public void Load(MetaBase mbParent, MetaBase mbChild)
        {
            ParentObject = mbParent;
            ChildObject = mbChild;
            AssociationHelper asshelper = new AssociationHelper();
            DataTable dt = asshelper.GetAssociatedObjects(mbParent.pkid, mbParent.MachineName);
            DataView dv = dt.DefaultView;
            foreach (DataRowView drv in dv)
            {
                if (drv["ChildObjectID"].ToString() == ChildObject.pkid.ToString() &&
                    drv["ChildObjectMachine"].ToString() == ChildObject.MachineName)
                {
                    MatrixObjectAssociation oassoc = new MatrixObjectAssociation();
                    oassoc.ChildObjectID = childObject.pkid;
                    oassoc.ChildObjectMachine = childObject.MachineName;
                    oassoc.ObjectID = ParentObject.pkid;
                    oassoc.ObjectMachine = ParentObject.MachineName;
                    oassoc.CAid = int.Parse(drv["CAid"].ToString());
                    oassoc.AssociationTypeID = int.Parse(drv["AssociationTypeID"].ToString());
                    oassoc.State = (VCStatusList)int.Parse(drv["VCStatusID"].ToString());
                    Associations.Add(oassoc);
                }
            }
        }


		#endregion Methods 

		#region Nested Classes (1) 


        public class MatrixObjectAssociation : ObjectAssociation
        {

		#region Fields (1) 

            private int associationTypeID;

		#endregion Fields 

		#region Properties (1) 

            public int AssociationTypeID
            {
                get { return associationTypeID; }
                set { associationTypeID = value; }
            }

		#endregion Properties 

        }
		#endregion Nested Classes 

    }

    public class ObjectNode : TreeNode
    {

		#region Fields (1) 

        private MetaBase metaBaseObject;

		#endregion Fields 

		#region Properties (1) 

        public MetaBase MetaBaseObject
        {
            get { return metaBaseObject; }
            set { metaBaseObject = value; }
        }

		#endregion Properties 

    }
}
