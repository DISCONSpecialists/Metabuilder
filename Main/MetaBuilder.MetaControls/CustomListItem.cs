using MetaBuilder.BusinessLogic;

namespace MetaBuilder.MetaControls
{
    public class CustomListItem
    {

		#region Fields (2) 

        private string caption;
        private object tag;

		#endregion Fields 

		#region Properties (2) 

        public string Caption
        {
            get { return caption; }
            set { caption = value; }
        }

        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

		#endregion Properties 

		#region Methods (1) 


		// Public Methods (1) 

        public override string ToString()
        {
            if (caption!=null)
                return caption;
            return base.ToString();
        }


		#endregion Methods 

    }

    public class DropdownFriendlyWorkspace : Workspace
    {

		#region Methods (2) 


		// Public Methods (2) 

        public string GetWorkspaceTypeName()
        {
            string wsType = "";
            if (WorkspaceTypeId == 3)
            {
                wsType = " [Server]";
            }
            else
            {
                wsType = " [Client]";
            }
            return wsType;
        }

        public new string ToString()
        {
            return Name + GetWorkspaceTypeName();
        }


		#endregion Methods 

    }
}
