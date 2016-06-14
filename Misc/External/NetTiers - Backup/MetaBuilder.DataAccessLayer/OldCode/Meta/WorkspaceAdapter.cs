using System.Data;
using System.Data.SqlClient;
using MetaBuilder.Core;

namespace MetaBuilder.DataAccessLayer.OldCode.Meta
{
	/// <summary>
	/// Summary description for WorkspaceAdapter.
	/// </summary>
	public class WorkspaceAdapter
	{
		public WorkspaceAdapter()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public DataView GetWorkspacesForUser(int UserID)
		{
			SqlCommand cmd = new SqlCommand("META_GetWorkspacesForUser", new SqlConnection(Variables.Instance.ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int));
			cmd.Parameters["@UserID"].Value = UserID;

			SqlDataAdapter dap = new SqlDataAdapter();
			DataSet ds = new DataSet();
			dap.SelectCommand = cmd;
			dap.Fill(ds, "Workspaces");
			return ds.Tables["Workspaces"].DefaultView;
		}

		public DataView GetTemplateWorkspaces()
		{
			SqlCommand cmd = new SqlCommand("META_GetTemplateWorkspaces", new SqlConnection(Variables.Instance.ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;

			SqlDataAdapter dap = new SqlDataAdapter();
			DataSet ds = new DataSet();
			dap.SelectCommand = cmd;
			dap.Fill(ds, "TemplateWorkspaces");
			return ds.Tables["TemplateWorkspaces"].DefaultView;
		}

		public int CreateWorkspace(string NewWorkspaceName, int UserID, bool IsTemplate)
		{
			SqlCommand cmd = new SqlCommand("META_CreateWorkspace",new SqlConnection(Variables.Instance.ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@NewWorkspaceName",SqlDbType.VarChar,100));
			cmd.Parameters.Add(new SqlParameter("@UserID",SqlDbType.Int));
			cmd.Parameters.Add(new SqlParameter("@IsTemplate",SqlDbType.Bit));
			cmd.Parameters.Add(new SqlParameter("@WorkspaceID",SqlDbType.Int));
			cmd.Parameters["@WorkspaceID"].Direction = ParameterDirection.Output;

			cmd.Parameters["@NewWorkspaceName"].Value = NewWorkspaceName;
			cmd.Parameters["@UserID"].Value = UserID;
			cmd.Parameters["@Istemplate"].Value = IsTemplate;

			if (cmd.Connection.State!=ConnectionState.Open)
			{
				cmd.Connection.Open();
			}
			cmd.ExecuteNonQuery();
			return int.Parse(cmd.Parameters["@WorkspaceID"].Value.ToString());
		}
	}
}