using System.Data;
using System.Data.SqlClient;
using MetaBuilder.Core;

namespace MetaBuilder.DataAccessLayer.OldCode.Diagramming
{
    public class TechniqueAdapter
    {

        public DataView GetList()
        {
            SqlCommand cmd = new SqlCommand("GetTechniques", new SqlConnection(Variables.Instance.ConnectionString));
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter dap = new SqlDataAdapter();
            DataSet ds = new DataSet();
            dap.SelectCommand = cmd;
            dap.Fill(ds, "Techniques");
            return ds.Tables["Techniques"].DefaultView;
        }


       


    }

    

}
