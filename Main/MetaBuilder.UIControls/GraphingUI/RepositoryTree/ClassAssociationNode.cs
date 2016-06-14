using System.Collections.Generic;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.UIControls.GraphingUI.RepositoryTree
{
    public class ClassAssociationNode : ItemNode
    {

		#region Fields (2) 

        private bool autoLoadChildren;
        private ClassAssociation classAssociation;

		#endregion Fields 

		#region Constructors (1) 

        public ClassAssociationNode(ClassAssociation value)
        {
            classAssociation = value;
            Items = new List<IRepositoryItem>();
            Text = value.ParentClass + " to " + value.ChildClass;
            TargetPanel = TargetPanelType.Relationships;
        }

		#endregion Constructors 

		#region Properties (2) 

        public bool AutoLoadChildren
        {
            get { return autoLoadChildren; }
            set { autoLoadChildren = value; }
        }

        public ClassAssociation ClassAssociation
        {
            get { return classAssociation; }
            set { classAssociation = value; }
        }

		#endregion Properties 

		#region Methods (1) 


		// Public Methods (1) 

        public override void LoadChildren()
        {
            TList<ObjectAssociation> associations =
                DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.GetByCAid(classAssociation.CAid);
            foreach (ObjectAssociation assoc in associations)
            {
                Items.Add(assoc);
            }
        }


		#endregion Methods 

    }
}