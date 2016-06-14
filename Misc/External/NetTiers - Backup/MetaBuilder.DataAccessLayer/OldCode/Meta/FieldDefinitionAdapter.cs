#region Copyright © 2007 - DISCON Specialists

//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: FieldDefinitionAdapter.cs
//

#endregion

using System.Data;
using System.Data.SqlClient;
using MetaBuilder.Core;

namespace MetaBuilder.DataAccessLayer.OldCode.Meta
{
	/// <summary>
	/// Summary description for FieldDefinitionAdapter.
	/// </summary>
	public class FieldDefinitionAdapter
	{
		public FieldDefinitionAdapter()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public DataTable GetFieldDefinitions(string ClassName, bool ActiveOnly)
		{
			SqlCommand cmd = new SqlCommand("META_GetFieldDefinitions", new SqlConnection(Variables.Instance.ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(new SqlParameter("@ClassName", SqlDbType.VarChar, 50));
			cmd.Parameters["@ClassName"].Value = ClassName;
            if (ActiveOnly)
            {
                cmd.Parameters.Add(new SqlParameter("@ActiveOnly", SqlDbType.Bit));
                cmd.Parameters["@ActiveOnly"].Value = 1;
            }


            DataSet ds = new DataSet();
			SqlDataAdapter dap = new SqlDataAdapter();
			dap.SelectCommand = cmd;
			dap.Fill(ds, "FieldDefs");
			return ds.Tables["FieldDefs"];
		}
	}
}