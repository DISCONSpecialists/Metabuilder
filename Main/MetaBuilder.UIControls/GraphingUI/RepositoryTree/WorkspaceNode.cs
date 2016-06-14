using System.Windows.Forms;
using MetaBuilder.BusinessLogic;
using b = MetaBuilder.BusinessLogic;

namespace MetaBuilder.UIControls.GraphingUI.RepositoryTree
{
    public class WorkspaceNode : TreeNode
    {

        #region Fields (3)

        private bool autoLoadChildren;
        private bool skipFiles;
        private Workspace workspace;
        private WorkspaceKey workspaceKey;
        private bool serverWorkspaceTransferObject = false;

        #endregion Fields

        #region Constructors (1)

        private bool workspaceTransfer = false;
        public WorkspaceNode(bool Transfer)
        {
            workspaceTransfer = Transfer;
        }

        #endregion Constructors

        #region Properties (4)

        public bool AutoLoadChildren
        {
            get { return autoLoadChildren; }
            set { autoLoadChildren = value; }
        }

        public RepositoryNode RepositoryNode
        {
            get { return Parent as RepositoryNode; }
        }

        public bool SkipFileNodes
        {
            get { return skipFiles; }
            set { skipFiles = value; }
        }

        public Workspace Workspace
        {
            get { return workspace; }
            set { workspace = value; }
        }
        public WorkspaceKey WorkspaceKey
        {
            get { return workspaceKey; }
            set { workspaceKey = value; }
        }

        public bool ServerWorkspaceTransferObject
        {
            get { return serverWorkspaceTransferObject; }
            set { serverWorkspaceTransferObject = value; }
        }

        #endregion Properties

        #region Methods (1)

        // Public Methods (1) 

        public void Load(Workspace ws)
        {
            Workspace = ws;
            Text = ws.Name;
            GraphFileCollectionNode fileCollectionNode = new GraphFileCollectionNode(workspaceTransfer);
            ClassCollectionNode ccnode = new ClassCollectionNode(workspaceTransfer);
            fileCollectionNode.SkipChildren = SkipFileNodes;
            if (RepositoryNode != null)
            {
                fileCollectionNode.Provider = RepositoryNode.RepositoryType.ToString();
                ccnode.Provider = RepositoryNode.RepositoryType.ToString();
            }
            else
            {
                //if (ws.WorkspaceTypeId == 3)
                //{
                //    Text = ws.Name + " (Server)";
                //    fileCollectionNode.Provider = Core.Variables.Instance.ServerProvider;
                //    ccnode.Provider = Core.Variables.Instance.ServerProvider;
                //}
                //else
                //{
                if (ServerWorkspaceTransferObject)
                {
                    fileCollectionNode.Provider = Core.Variables.Instance.ServerProvider;
                    ccnode.Provider = Core.Variables.Instance.ServerProvider;
                }
                else
                {
                    fileCollectionNode.Provider = Core.Variables.Instance.ClientProvider;
                    ccnode.Provider = Core.Variables.Instance.ClientProvider;
                }
                //}
            }
            WorkspaceKey = new WorkspaceKey(Workspace);
            fileCollectionNode.WorkspaceFilter = WorkspaceKey;
            fileCollectionNode.RefreshNodes();
            Nodes.Add(fileCollectionNode);
            ccnode.WorkspaceFilter = WorkspaceKey;
            ccnode.Load();
            Nodes.Add(ccnode);
        }

        #endregion Methods

    }
}