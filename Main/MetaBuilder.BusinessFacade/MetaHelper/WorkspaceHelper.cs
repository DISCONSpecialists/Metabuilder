using System.Data;
using MetaBuilder.DataAccessLayer.OldCode.Meta;

namespace MetaBuilder.BusinessFacade.MetaHelper
{
	/// <summary>
	/// Summary description for WorkspaceHelper.
	/// </summary>
	public class WorkspaceHelper
	{
		public WorkspaceHelper()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public DataView GetWorkspacesForUser(int UserID)
		{
			WorkspaceAdapter wsAdap = new WorkspaceAdapter();
			return wsAdap.GetWorkspacesForUser(UserID);
		}

		public DataView GetTemplateWorkspaces()
		{
			WorkspaceAdapter wsAdap = new WorkspaceAdapter();
			return wsAdap.GetTemplateWorkspaces();
		}
		public int CreateWorkspace(string NewWorkspaceName, int UserID, bool IsTemplate)
		{
			WorkspaceAdapter wsadapter = new WorkspaceAdapter();
			return wsadapter.CreateWorkspace(NewWorkspaceName,UserID,IsTemplate);
		}
	}
}