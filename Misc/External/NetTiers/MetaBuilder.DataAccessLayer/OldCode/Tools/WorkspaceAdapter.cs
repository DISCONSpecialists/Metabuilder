using System.Data;
using System.Data.SqlClient;
using MetaBuilder.Core;

namespace MetaBuilder.DataAccessLayer.OldCode.Tools
{
    public class WorkspaceAdapter
    {
        /// <summary>
        /// Deletes a workspace or moves a workspace's contents into another one
        /// </summary>
        /// <param name="workspaceID"></param>
        /// <param name="newWorkspaceID"></param>
        /// 
        public static void DeleteWorkspace(int WorkspaceTypeId, string workspaceName, string newWorkspaceName, int? newWorkspaceTypeId, string Provider)
        {
            string connString = Provider == Core.Variables.Instance.ClientProvider ? Variables.Instance.ConnectionString : Variables.Instance.ServerConnectionString;
            SqlCommand cmd = new SqlCommand("META_DeleteWorkspace", new SqlConnection(connString));
            cmd.CommandType = CommandType.StoredProcedure;

            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }

            cmd.Parameters.Add(new SqlParameter("@WorkspaceTypeId", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@WorkspaceName", SqlDbType.VarChar, 100));
            cmd.Parameters.Add(new SqlParameter("@NewWorkspaceTypeId", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@NewWorkspaceName", SqlDbType.VarChar, 100));
            cmd.Parameters["@NewWorkspaceTypeId"].IsNullable = true;
            cmd.Parameters["@NewWorkspaceName"].IsNullable = true;

            cmd.Parameters["@WorkspaceTypeId"].Value = WorkspaceTypeId;
            cmd.Parameters["@WorkspaceName"].Value = workspaceName;

            if (newWorkspaceTypeId.HasValue && newWorkspaceName != null)
            {
                cmd.Parameters["@NewWorkspaceTypeId"].Value = newWorkspaceTypeId.Value;
                cmd.Parameters["@NewWorkspaceName"].Value = newWorkspaceName;
            }
            cmd.ExecuteNonQuery();
            return;
        }
    }
}
