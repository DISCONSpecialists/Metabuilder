using System.Collections;
using System.Data;
using System.Data.SqlClient;
using MetaBuilder.Core;

namespace MetaBuilder.DataAccessLayer.OldCode.Meta
{
	/// <summary>
	/// Summary description for SecurityAdapter.
	/// </summary>
	public class SecurityAdapter
	{
		public SecurityAdapter()
		{
			//
			// TODO: Add constructor logic here
			//
		}


		public int LoginUser(string Username, string Password)
		{
			SqlCommand cmd = new SqlCommand("META_UserLogin", new SqlConnection(Variables.Instance.ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.VarChar, 100));
			cmd.Parameters.Add(new SqlParameter("@Password", SqlDbType.VarChar, 100));
			cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int));
			cmd.Parameters["@UserID"].Direction = ParameterDirection.Output;

			cmd.Parameters["@Username"].Value = Username;
			cmd.Parameters["@Password"].Value = Password;

			if (cmd.Connection.State != ConnectionState.Open)
			{
				cmd.Connection.Open();
			}
			cmd.ExecuteNonQuery();
			return int.Parse(cmd.Parameters["@UserID"].Value.ToString());
		}

		public DataView GetWorkspacesForUser(int UserID)
		{
			SqlConnection conn = new SqlConnection(Variables.Instance.ConnectionString);
			SqlCommand cmdWorkspaces = new SqlCommand("META_GetWorkspacesForUser", conn);
			cmdWorkspaces.CommandType = CommandType.StoredProcedure;

			cmdWorkspaces.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int));
			cmdWorkspaces.Parameters["@UserID"].Value = UserID;

			DataSet ds = new DataSet();
			SqlDataAdapter dap = new SqlDataAdapter();
			dap.SelectCommand = cmdWorkspaces;
			dap.Fill(ds, "Workspaces");
			return ds.Tables["Workspaces"].DefaultView;


		}

		public Hashtable GetWorkspacePermissionsForUser(int UserID, int WorkspaceID)
		{
			DataSet ds = new DataSet();
			SqlDataAdapter dap = new SqlDataAdapter();
			SqlCommand cmdPermissions = new SqlCommand("META_GetWorkspacePermissions", new SqlConnection(Variables.Instance.ConnectionString));
			cmdPermissions.CommandType = CommandType.StoredProcedure;

			cmdPermissions.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int));
			cmdPermissions.Parameters["@UserID"].Value = UserID;
			cmdPermissions.Parameters.Add(new SqlParameter("@WorkspaceID", SqlDbType.Int));
			cmdPermissions.Parameters["@WorkspaceID"].Value = WorkspaceID;

			dap.SelectCommand = cmdPermissions;
			dap.Fill(ds, "Permissions");

			Hashtable HashPermissions = new Hashtable();

			foreach (DataRowView drv in ds.Tables["Permissions"].DefaultView)
			{
				int PermissionId = int.Parse(drv["PermissionId"].ToString());
				string PermissionName = drv["PermissionName"].ToString();
				HashPermissions.Add(PermissionId, PermissionName);
			}
			return HashPermissions;
		}

		public Hashtable GetGeneralPermissionsForUser(int UserID)
		{
			SqlCommand cmd = new SqlCommand("META_GetGeneralPermissions", new SqlConnection(Variables.Instance.ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int));
			cmd.Parameters["@UserID"].Value = UserID;

			DataSet ds = new DataSet();
			SqlDataAdapter dap = new SqlDataAdapter();
			dap.SelectCommand = cmd;
			dap.Fill(ds, "GeneralPermissions");

			Hashtable HashPermissions = new Hashtable();

			foreach (DataRowView drv in ds.Tables["GeneralPermissions"].DefaultView)
			{
				int PermissionId = int.Parse(drv["PermissionId"].ToString());
				string PermissionName = drv["Description"].ToString();
				HashPermissions.Add(PermissionId, PermissionName);
			}
			return HashPermissions;
		}

		public DataView GetUsers()
		{
			SqlConnection conn = new SqlConnection(Variables.Instance.ConnectionString);
			SqlCommand cmdWorkspaces = new SqlCommand("META_GetUsers", conn);
			cmdWorkspaces.CommandType = CommandType.StoredProcedure;

			DataSet ds = new DataSet();
			SqlDataAdapter dap = new SqlDataAdapter();
			dap.SelectCommand = cmdWorkspaces;
			dap.Fill(ds, "Users");
			return ds.Tables["Users"].DefaultView;
		}
	}
}