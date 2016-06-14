using System.Collections.Generic;
using System.Windows.Forms;
using MetaBuilder.BusinessLogic;
using b = MetaBuilder.BusinessLogic;
//using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.UIControls.GraphingUI.RepositoryTree
{
    public class ItemNode : TreeNode
    {

		#region Fields (6) 

        private bool autoLoadChildren;
        private List<IRepositoryItem> items;
        private string provider;
        private VCStatusList statusFilter;
        private TargetPanelType targetPanel;
        private WorkspaceKey workspaceFilter;

		#endregion Fields 

		#region Constructors (1) 

        public ItemNode()
        {
            Items = new List<IRepositoryItem>();
        }

		#endregion Constructors 

		#region Properties (6) 

        public bool AutoLoadChildren
        {
            get { return autoLoadChildren; }
            set { autoLoadChildren = value; }
        }

        public List<IRepositoryItem> Items
        {
            get { return items; }
            set { items = value; }
        }

        public string Provider
        {
            get { return provider; }
            set { provider = value; }
        }

        public VCStatusList StatusFilter
        {
            get { return statusFilter; }
            set { statusFilter = value; }
        }

        public TargetPanelType TargetPanel
        {
            get { return targetPanel; }
            set { targetPanel = value; }
        }

        public WorkspaceKey WorkspaceFilter
        {
            get { return workspaceFilter; }
            set { workspaceFilter = value; }
        }

		#endregion Properties 

		#region Methods (1) 

     
		// Public Methods (1) 

        public virtual void LoadChildren()
        {
            ////LOAD ITEMS LOL (JOKES ON ME ITS VIRTUAL)
            //if (string.IsNullOrEmpty(Provider))
            //    Provider=Core.Variables.Instance.ClientProvider;

            //foreach (b.MetaObject obj in d.DataRepository.Connections[Provider].Provider.MetaObjectProvider.GetByClass(this.Text))
            //{
            //    if (obj.WorkspaceName == workspaceFilter.Name && obj.WorkspaceTypeId == workspaceFilter.WorkspaceTypeId)
            //    {
            //        Meta.MetaBase mObj = Meta.Loader.GetFromProvider(obj.pkid, obj.Machine, obj.Class, (Provider == Core.Variables.Instance.ServerProvider));
            //        items.Add(mObj);
            //    }
            //}
        }


		#endregion Methods 

    }

    public enum TargetPanelType
    {
        Diagrams,
        Objects,
        Relationships
    }
}