using System.Data;
using System.Data.SqlClient;
using MetaBuilder.Core;

namespace MetaBuilder.DataAccessLayer.OldCode.Tools
{
	/// <summary>
	/// Summary description for ReportAdapter.
	/// </summary>
	public class ReportAdapter
	{
		public ReportAdapter()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public void BuildADDReportCache()
		{
			SqlCommand cmd = new SqlCommand("VISIO_ADD_BuildReportingCache", new SqlConnection(Variables.Instance.ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;

			if (cmd.Connection.State != ConnectionState.Open)
			{
				cmd.Connection.Open();
			}
			cmd.ExecuteNonQuery();
			cmd.Connection.Close();
		}
	}
}