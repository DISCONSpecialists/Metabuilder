using System.Collections.Generic;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.UIControls.GraphingUI.RepositoryTree
{
    public class RelationshipCollectionNode : ItemNode
    {

        #region Fields (1)

        private bool autoLoadChildren;

        #endregion Fields

        #region Properties (1)

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
            Items = new List<IRepositoryItem>();
            Text = "Relationships";
            Nodes.Clear();
            TList<AssociationType> associationTypes = DataRepository.Connections[Provider].Provider.AssociationTypeProvider.GetAll();
            foreach (AssociationType assType in associationTypes)
            {
                AssociationTypeNode node = new AssociationTypeNode(assType);
                node.Provider = Provider;
                node.StatusFilter = StatusFilter;
                Nodes.Add(node);
                node.Load();
            }
        }

        #endregion Methods

    }
}