#region Copyright © 2007 - DISCON Specialists

//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: ViewBuilder.cs
//

#endregion

using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using MetaBuilder.Core;

namespace MetaBuilder.Meta.Builder
{
    /// <summary>
    /// Summary description for ViewBuilder.
    /// </summary>
    public class ViewBuilder
    {
        bool Startup = false;

        private SqlConnection connection;
        public SqlConnection Connection
        {
            get { return connection; }
            set { connection = value; }
        }

        public ViewBuilder()
        {
            Connection = new SqlConnection(Variables.Instance.ConnectionString);
            SqlCommand cmdDomains = new SqlCommand("META_GetDomainDefinitions", Connection);
            cmdDomains.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter dapDomain = new SqlDataAdapter();
            dapDomain.SelectCommand = cmdDomains;
            dsDomains = new DataSet();
            dapDomain.Fill(dsDomains, "Domains");
        }
        public ViewBuilder(SqlConnection conn)
        {
            Startup = true;
            Connection = conn;

            SqlCommand cmdDomains = new SqlCommand("META_GetDomainDefinitions", Connection);
            cmdDomains.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter dapDomain = new SqlDataAdapter();
            dapDomain.SelectCommand = cmdDomains;
            dsDomains = new DataSet();
            dapDomain.Fill(dsDomains, "Domains");
        }

        private DataSet dsDomains;

        public void BuildView(string ClassName)
        {
            CreateView(ClassName, ViewType.Listing);
            CreateView(ClassName, ViewType.Retrieval);
        }

        private void CreateView(string ClassName, ViewType viewType)
        {
            DataView dvDomains = dsDomains.Tables["Domains"].DefaultView;

            string ViewName = "METAView_" + ClassName + "_" + viewType.ToString();
            string cmdtext = "select name,datatype from field where class='" + ClassName + "'";
            if (viewType == ViewType.Listing)
            {
                cmdtext += "  AND IsActive=1";
            }

            SqlCommand cmd = new SqlCommand(cmdtext, Connection);
            cmd.CommandType = CommandType.Text;

            DataSet ds = new DataSet();
            SqlDataAdapter dap = new SqlDataAdapter();
            dap.SelectCommand = cmd;

            dap.Fill(ds, "Fields");

            ArrayList fields = new ArrayList();
            System.Collections.Generic.List<string> f = new System.Collections.Generic.List<string>();
            foreach (DataRowView dr in ds.Tables["Fields"].DefaultView)
            {
                FieldDefinition fdef = new FieldDefinition();
                fdef.DataType = dr["DataType"].ToString();
                fdef.Name = dr["Name"].ToString();

                //duplicate field names
                if (f.Contains(fdef.Name))
                    continue;
                else
                    f.Add(fdef.Name);

                //Duplicate fields
                if (!(fields.Contains(fdef)))
                    fields.Add(fdef);
            }

            if (fields.Count == 0) //dont need views for classes(if that class even exists?) with no fields
                return;

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT  MetaObject.WorkspaceName,MetaObject.WorkspaceTypeId,MetaObject.VCStatusID,MetaObject.pkid,MetaObject.Machine,VCMachineID");

            string Column;
            foreach (FieldDefinition fd in fields)
            {
                switch (fd.DataType)
                {
                    case "System.String":
                        if (ClassName == "Rationale" && fd.Name == "UniqueRef")
                            Column = "ValueLongText";
                        else
                            Column = "ValueString";
                        break;
                    case "System.Int32":
                        Column = "ValueInt";
                        break;
                    case "System.Int32?":
                        Column = "ValueInt";
                        break;
                    case "System.DateTime":
                        Column = "ValueDate";
                        break;
                    case "System.Double":
                        Column = "ValueDouble";
                        break;
                    case "System.Boolean":
                        Column = "ValueBoolean";
                        break;
                    case "LongText":
                        Column = "ValueLongText";
                        break;
                    default:
                        dvDomains.RowFilter = "Name='" + fd.DataType + "'";
                        if (dvDomains.Count > 0)
                        {
                            Column = "ValueString";
                        }
                        else
                        {
                            Column = "ValueString";
                        }
                        break;
                }
                sql.Append("," + fd.Name + "Value." + Column + " as " + fd.Name);
            }
            sql.Append(" FROM MetaObject \n");

            foreach (FieldDefinition fd in fields)
            {
                sql.Append(" INNER JOIN  dbo.Field  " + fd.Name + "Field  ON MetaObject.Class = " + fd.Name + "Field.Class left outer JOIN  " + Environment.NewLine);
                sql.Append("dbo.ObjectFieldValue " + fd.Name + "Value ON MetaObject.pkid=" + fd.Name + "Value.ObjectID and MetaObject.Machine=" + fd.Name + "Value.MachineID and " + fd.Name + "Field.pkid = " + fd.Name + "Value.fieldid  " + Environment.NewLine);
            }

            sql.Append(" WHERE (MetaObject.Class = '" + ClassName + "') ");

            foreach (FieldDefinition fd in fields)
            {
                sql.Append(" AND " + fd.Name + "Field.Name ='" + fd.Name + "' ");
            }
            sql.Append(Environment.NewLine);

            StringBuilder pre = new StringBuilder();
            pre.Append("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + ViewName + "]') and OBJECTPROPERTY(id, N'IsView') = 1) drop view [dbo].[" + ViewName + "];" + Environment.NewLine);

            StringBuilder post = new StringBuilder();
            SqlCommand createviewcmd = new SqlCommand(pre.ToString(), Connection);
            createviewcmd.CommandType = CommandType.Text;

            if (createviewcmd.Connection.State != ConnectionState.Open)
            {
                createviewcmd.Connection.Open();
            }

            try
            {
                createviewcmd.ExecuteNonQuery();
                createviewcmd.CommandText = "CREATE VIEW " + ViewName + Environment.NewLine + "AS" + Environment.NewLine + sql;
                createviewcmd.ExecuteNonQuery();
            }
            catch (Exception x)
            {
                // Console.WriteLine(x.ToString());
            }
            if (!Startup)
                createviewcmd.Connection.Close();
        }
    }

    public class FieldDefinition
    {
        public string Name;
        public string DataType;

        public FieldDefinition()
        {
        }
    }

    public enum ViewType
    {
        Listing,
        Retrieval
    }
}