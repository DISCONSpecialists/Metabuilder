using System.Data;
using System.Data.SqlClient;
using MetaBuilder.Core;

namespace MetaBuilder.DataAccessLayer.OldCode.Imports.Visio
{
	/// <summary>
	/// Summary description for ADDAdapter.
	/// </summary>
	public class ADDAdapter
	{
		public ADDAdapter()
		{
		}



		public DataSet RetrieveWorkspaceEntitiesAndAttributes()
		{
			SqlCommand cmd = new SqlCommand("VISIO_ADD_RetrieveEntities", new SqlConnection(Variables.Instance.ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@WorkspaceTypeId", SqlDbType.Int));
            cmd.Parameters["@WorkspaceTypeId"].Value = Variables.Instance.CurrentWorkspaceTypeId;
            cmd.Parameters.Add(new SqlParameter("@WorkspaceName", SqlDbType.VarChar,100));
            cmd.Parameters["@WorkspaceName"].Value = Variables.Instance.CurrentWorkspaceName;

			DataSet ds = new DataSet();
			SqlDataAdapter dap = new SqlDataAdapter();
			dap.SelectCommand = cmd;
			dap.Fill(ds, "Entities");
			cmd.CommandText = "VISIO_ADD_RetrieveAttributeKeysets";
			dap.Fill(ds, "AttributeKeysets");
			cmd.CommandText = "VISIO_ADD_RetrieveAttributes";
			dap.Fill(ds, "Attributes");

			cmd.CommandText = "VISIO_ADD_RetrieveEntityAttributes";
			dap.Fill(ds, "EntityAttributes");


			return ds;
		}


	}
}