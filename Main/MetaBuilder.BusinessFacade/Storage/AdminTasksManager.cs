using System.Data;
using System.Data.SqlClient;
using MetaBuilder.Core;

namespace MetaBuilder.BusinessFacade.Storage
{
    public class AdminTasksManager
    {
        private bool isLocal;
        public AdminTasksManager(bool Local)
        {
            isLocal = Local;
        }

        public void ClearOrphans()
        {
            RunCommand("ClearOrphans");
        }
        public void ClearPreviousVersions()
        {
            RunCommand("ClearPreviousVersions");
        }
        public void ClearSandbox()
        {
            if (isLocal)
            {
                RunCommand("ClearSandbox");
            }
        }

        private void RunCommand(string command)
        {
            string conn;
            if (isLocal)
            {
                conn = Variables.Instance.ConnectionString;
            }
            else
            {
                conn = Variables.Instance.ServerConnectionString;
            }
            SqlCommand cmd = new SqlCommand("proc_AdminTasks_" + command, new SqlConnection(conn));
            cmd.CommandType = CommandType.StoredProcedure;

            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

        }

        public void MarkInactives()
        {
            SqlCommand cmd = new SqlCommand("HACK_MarkDrawingsActiveFromReportingTool", new SqlConnection(Variables.Instance.ConnectionString));
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }
	
    }
}
