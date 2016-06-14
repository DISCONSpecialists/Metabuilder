using System.Windows.Forms;
using MetaBuilder.BusinessFacade.Storage.RepositoryTemp;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.UIControls.GraphingUI.RepositoryTree
{
    public class RepositoryNode : TreeNode
    {

        #region Fields (3)

        private bool autoLoadChildren;
        private string connectionString;
        private Repository.RepositoryType repostype;

        #endregion Fields

        #region Properties (3)

        public bool AutoLoadChildren
        {
            get { return autoLoadChildren; }
            set { autoLoadChildren = value; }
        }

        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        public Repository.RepositoryType RepositoryType
        {
            get { return repostype; }
            set { repostype = value; }
        }

        #endregion Properties

        #region Methods (1)

        // Public Methods (1) 

        public void Load()
        {
            Text = RepositoryType.ToString() + " Repository";
            string provider = RepositoryType.ToString();
            PermissionService permService = new PermissionService();
            //bool cannotCheckInFromSandbox = permService.SERVERCONFIG__CannotCheckInFromSandbox;

            TList<Workspace> workspaces = DataRepository.Connections[RepositoryType.ToString()].Provider.WorkspaceProvider.GetAll();
            foreach (Workspace ws in workspaces)
            {
                string def = "";
                if (provider == Core.Variables.Instance.ClientProvider)
                {
                    if (Core.Variables.Instance.DefaultWorkspace.Length > 0)
                    {
                        if (ws.Name == Core.Variables.Instance.DefaultWorkspace && ws.WorkspaceTypeId == Core.Variables.Instance.DefaultWorkspaceID)
                        {
                            def = " <Default>";
                        }
                    }
                }

                if (ws.WorkspaceTypeId == 3) //&& cannotCheckInFromSandbox //ws.Name != "Sandbox" && 
                {
                    WorkspaceNode wsNode = new WorkspaceNode(false);
                    Nodes.Add(wsNode);
                    wsNode.Load(ws);
                    wsNode.Text += def;
                }
            }
            RelationshipCollectionNode relCollectionNode = new RelationshipCollectionNode();
            relCollectionNode.Provider = RepositoryType.ToString();
            Nodes.Add(relCollectionNode);
            relCollectionNode.Load();
        }

        #endregion Methods

    }
}