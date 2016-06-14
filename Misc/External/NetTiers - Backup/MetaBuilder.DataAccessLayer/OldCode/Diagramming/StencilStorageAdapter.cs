using System.Data;
using System.Data.SqlClient;
using MetaBuilder.Core;

namespace MetaBuilder.DataAccessLayer.OldCode.Diagramming
{
	/// <summary>
	/// Summary description for StencilAdapter.
	/// </summary>
	public class StencilStorageAdapter
	{
		public StencilStorageAdapter()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public DataView GetList()
		{
			SqlCommand cmd = new SqlCommand("sp_Diagramming_Stencils_GetList", new SqlConnection(Variables.Instance.ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(new SqlParameter("@StencilType", SqlDbType.VarChar, 100));
			SqlDataAdapter dap = new SqlDataAdapter();
			DataSet ds = new DataSet();
			dap.SelectCommand = cmd;
			dap.Fill(ds, "Stencils");
			return ds.Tables["Stencils"].DefaultView;
		}

		public DataView GetList(string StencilType)
		{
			SqlCommand cmd = new SqlCommand("sp_Diagramming_Stencils_GetList", new SqlConnection(Variables.Instance.ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(new SqlParameter("@StencilType", SqlDbType.VarChar, 100));
			cmd.Parameters["@StencilType"].Value = StencilType;
			SqlDataAdapter dap = new SqlDataAdapter();
			DataSet ds = new DataSet();
			dap.SelectCommand = cmd;
			dap.Fill(ds, "Stencils");
			return ds.Tables["Stencils"].DefaultView;
		}
	}
}