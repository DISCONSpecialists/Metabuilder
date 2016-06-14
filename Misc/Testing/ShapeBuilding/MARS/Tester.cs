using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.IO;
using MetaBuilder.BusinessLogic;
namespace ShapeBuilding.MARS
{
    public class ObjectKey
    {
    }

    public class NewMetaBase : MetaBuilder.Meta.MetaBase,ICloneable
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    class Tester
    {
        
        public void executeStatement() {
            List<MetaBuilder.BusinessLogic.MetaObjectKey> functions = new List<MetaBuilder.BusinessLogic.MetaObjectKey>();
            List<MetaBuilder.BusinessLogic.MetaObjectKey> attributes = new List<MetaBuilder.BusinessLogic.MetaObjectKey>();
            functions.Add(new MetaBuilder.BusinessLogic.MetaObjectKey(277505,"REZA-PC-681"));
            functions.Add(new MetaBuilder.BusinessLogic.MetaObjectKey(277506,"REZA-PC-129"));
            functions.Add(new MetaBuilder.BusinessLogic.MetaObjectKey(277507,"REZA-PC-487"));
            functions.Add(new MetaBuilder.BusinessLogic.MetaObjectKey(277509,"REZA-PC-487"));
            functions.Add(new MetaBuilder.BusinessLogic.MetaObjectKey(277510,"REZA-PC-143"));
            functions.Add(new MetaBuilder.BusinessLogic.MetaObjectKey(277511,"REZA-PC-143"));
            functions.Add(new MetaBuilder.BusinessLogic.MetaObjectKey(277512,"REZA-PC-996"));
            functions.Add(new MetaBuilder.BusinessLogic.MetaObjectKey(277514,"REZA-PC-996"));
            functions.Add(new MetaBuilder.BusinessLogic.MetaObjectKey(277516,"REZA-PC-701"));

            attributes.Add(new MetaBuilder.BusinessLogic.MetaObjectKey(302551, "DEON-233"));
            attributes.Add(new MetaBuilder.BusinessLogic.MetaObjectKey(302552,"DEON-355"));
            attributes.Add(new MetaBuilder.BusinessLogic.MetaObjectKey(302554,"DEON-101"));
            attributes.Add(new MetaBuilder.BusinessLogic.MetaObjectKey(302555,"DEON-460"));
            attributes.Add(new MetaBuilder.BusinessLogic.MetaObjectKey(302559,"DEON-925"));

            StringBuilder sbFunctionKeys = new StringBuilder();
            foreach (MetaObjectKey k in functions)
            {
                sbFunctionKeys.Append("'" + k.pkid.ToString() + k.Machine + "',");
            }
            StringBuilder sbAttributeKeys = new StringBuilder();
            foreach (MetaObjectKey k in attributes)
            {
                sbAttributeKeys.Append("'" + k.pkid.ToString() + k.Machine + "',");
            }


            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;


            string[] parameters = new string[functions.Count];
            for (int i = 0;i<functions.Count;i++)
            {
                parameters[i] = "@p" + i;
                cmd.Parameters.AddWithValue(parameters[i], functions[i].pkid + functions[i].Machine);
            }
               string connString = "Data Source=.\\SQLEXPRESS;Initial Catalog=MetaBuilder;Integrated Security=true;Connection Timeout=0;";
     
            cmd.Connection = new SqlConnection(connString);

            String SQL = "SELECT TOP 10 * FROM METAVIEW_FUNCTION_LISTING WHERE CAST(PKID AS VARCHAR(10)) + MACHINE IN(";
            SQL += string.Join(",", parameters) + "); ";
                cmd.CommandText = SQL;

            

           // String SQL = "SELECT TOP 10 * FROM METAVIEW_FUNCTION_LISTING WHERE CAST(PKID AS VARCHAR(10)) + MACHINE IN (" + sbFunctionKeys.ToString() + ");"
//            +
  //                 "SELECT TOP 20 * FROM METAVIEW_ORGANIZATIONALUNIT_LISTING WHERE CAST(PKID AS VARCHAR(10)) + MACHINE IN (" + sbAttributeKeys.ToString() + ");";

             //   MetaBuilder.Meta.MetaBase mb = System.AppDomain.CurrentDomain.CreateInstanceAndUnwrap(.CreateInstance("Function",, false);
                    //MetaBuilder.Meta.Loader.CreateInstance("Function");
            //object o = 
   
       if (cmd.Connection.State != ConnectionState.Open)
           cmd.Connection.Open();
            SqlDataAdapter dap = new SqlDataAdapter();
            dap.SelectCommand = cmd;

       DataSet ds = new DataSet();
            dap.Fill(ds);
            DataView dv = ds.Tables[0].DefaultView;
            foreach (DataRowView drv in dv)
            {
                Console.WriteLine(drv["Name"].ToString());
            }
            cmd.Connection.Close();
        }
        public Tester()
        {
            executeStatement();
        }

    }

   

}

/*
     SqlDataAdapter dap = new SqlDataAdapter();
    Statement stmt = con.createStatement();
    boolean results = stmt.execute(SQL);
    int rsCount = 0;

    //Loop through the available result sets.
   do {
      if(results) {
         ResultSet rs = stmt.getResultSet();
         rsCount++;

         //Show data from the result set.
         System.out.println("RESULT SET #" + rsCount);
         while (rs.next()) {
            System.out.println(rs.getString("LastName") + ", " + rs.getString("FirstName"));
         }
         rs.close();
      }
      System.out.println();
      results = stmt.getMoreResults();
      } while(results);
    stmt.close();
    }
 catch (Exception e) {
    e.printStackTrace();*/
