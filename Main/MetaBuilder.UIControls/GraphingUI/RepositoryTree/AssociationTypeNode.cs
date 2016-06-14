using System.Collections.Generic;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.UIControls.GraphingUI.RepositoryTree
{
    public class AssociationTypeNode : ItemNode
    {

		#region Fields (2) 

        private AssociationType associationType;
        private bool autoLoadChildren;

		#endregion Fields 

		#region Constructors (1) 

        public AssociationTypeNode(AssociationType associationType)
        {
            Items = new List<IRepositoryItem>();
            Text = associationType.Name;
            AssociationType = associationType;
            TargetPanel = TargetPanelType.Relationships;
        }

		#endregion Constructors 

		#region Properties (2) 

        public AssociationType AssociationType
        {
            get { return associationType; }
            set { associationType = value; }
        }

        public bool AutoLoadChildren
        {
            get { return autoLoadChildren; }
            set { autoLoadChildren = value; }
        }

		#endregion Properties 

		#region Methods (1) 


		// Public Methods (1) 

        public void Load()
        {
            TList<ClassAssociation> classAssociations =
                DataRepository.Connections[Provider].Provider.ClassAssociationProvider.GetByAssociationTypeID(
                    associationType.pkid);
            classAssociations.Sort("ParentClass , ChildClass");
            foreach (ClassAssociation classAssociation in classAssociations)
            {
                ClassAssociationNode canode = new ClassAssociationNode(classAssociation);
                canode.Provider = Provider;
                canode.TargetPanel = TargetPanelType.Relationships;
                Nodes.Add(canode);
            }
        }


		#endregion Methods 

    }
}