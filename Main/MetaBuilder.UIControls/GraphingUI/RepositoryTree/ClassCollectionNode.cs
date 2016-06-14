using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.UIControls.GraphingUI.RepositoryTree
{
    public class ClassCollectionNode : ItemNode
    {

        #region Fields (1)

        private bool autoLoadChildren;
        private bool workspaceTransfer = false;

        #endregion Fields

        #region Properties (1)

        public bool AutoLoadChildren
        {
            get { return autoLoadChildren; }
            set { autoLoadChildren = value; }
        }

        #endregion Properties

        public ClassCollectionNode(bool Transfer)
        {
            workspaceTransfer = Transfer;
        }

        #region Methods (2)

        // Public Methods (2) 

        public void Load()
        {
            Text = "Classes";
            Nodes.Clear();
            TList<Class> classes = DataRepository.Classes(Provider);// DataRepository.Connections[Provider].Provider.ClassProvider.GetAll();
            foreach (Class mbclass in classes)
            {
                ClassNode node = new ClassNode(mbclass, workspaceTransfer);
                node.StatusFilter = StatusFilter;
                node.WorkspaceFilter = WorkspaceFilter;
                node.Provider = Provider;
                TargetPanel = TargetPanelType.Objects;
                //node.LoadChildren();
                // if (node.Items.Count > 0)
                Nodes.Add(node);
            }
        }

        public override void LoadChildren()
        {
            TList<Class> classes = DataRepository.Classes(Provider);// DataRepository.Connections[Provider].Provider.ClassProvider.GetAll();
            foreach (Class mbclass in classes)
            {
                ClassNode node = new ClassNode(mbclass, workspaceTransfer);
                node.StatusFilter = StatusFilter;
                node.Provider = Provider;
                node.WorkspaceFilter = WorkspaceFilter;
                TargetPanel = TargetPanelType.Objects;
                //node.LoadChildren();
                // if (node.Items.Count > 0)
                //    Nodes.Add(node);
            }
        }

        #endregion Methods

    }
}