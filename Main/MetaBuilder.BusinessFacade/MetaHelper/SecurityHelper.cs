using System.Collections;
using System.Data;
using MetaBuilder.DataAccessLayer.OldCode.Meta;

namespace MetaBuilder.BusinessFacade.MetaHelper
{
	/// <summary>
	/// Summary description for SecurityHelper.
	/// </summary>
	public class SecurityHelper
	{
		public SecurityHelper()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public int LoginUser(string Username, string Password)
		{
			SecurityAdapter sad = new SecurityAdapter();
			return sad.LoginUser(Username, Password);
		}

		public DataView GetWorkspacesForUser(int UserID)
		{
			SecurityAdapter sad = new SecurityAdapter();
			return sad.GetWorkspacesForUser(UserID);
		}

		public Hashtable GetWorkspacesAndPermissionsForUser(int UserID, int WorkspaceID)
		{
			SecurityAdapter sad = new SecurityAdapter();
			return sad.GetWorkspacePermissionsForUser(UserID, WorkspaceID);
		}

		public Hashtable GetGeneralPermissionsForUser(int UserID)
		{
			SecurityAdapter sad = new SecurityAdapter();
			return sad.GetGeneralPermissionsForUser(UserID);
		}

		public DataView GetUsers()
		{
			SecurityAdapter sad = new SecurityAdapter();
			return sad.GetUsers();
		}
	}
}