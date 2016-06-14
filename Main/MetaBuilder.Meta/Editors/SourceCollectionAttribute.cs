#region Copyright © 2007 - DISCON Specialists

//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: SourceCollectionAttribute.cs
//

#endregion

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using MetaBuilder.Core;

namespace MetaBuilder.Meta.Editors
{
	/// <summary>
	/// Summary description for SourceCollectionAttribute.
	/// </summary>
	[Description("Service attribute to point to the source collection."), AttributeUsage(AttributeTargets.All)]
	public class SourceCollectionAttribute : Attribute
	{
		private string srcCollName;

		public string CollectionName
		{
			get { return this.srcCollName; }
		}

		public DataView dv;

		public ICollection Collection(object instance)
		{
			if (dv == null)
			{
                SqlCommand cmd = new SqlCommand("META_GetDomainPossibleValues", new SqlConnection(Variables.Instance.ConnectionString));
				cmd.CommandType = CommandType.StoredProcedure;

                

				cmd.Parameters.Add(new SqlParameter("@DomainDefinition", SqlDbType.VarChar, 50));
				cmd.Parameters["@DomainDefinition"].Value = srcCollName;
			    cmd.Parameters.Add(new SqlParameter("@ActiveOnly", SqlDbType.Bit));
			    cmd.Parameters["@ActiveOnly"].Value = 1;

				DataSet ds = new DataSet();
				SqlDataAdapter dap = new SqlDataAdapter();
				dap.SelectCommand = cmd;
				dap.Fill(ds, "Items");
                if (ds.Tables["Items"].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables["Items"].NewRow();
                    dr["Name"] = ds.Tables["Items"].Rows[0]["Name"];
                    dr["PossibleValue"] = "";
                    dr["Description"] = "";
                    dr["Series"] = 0;
                    ds.Tables["Items"].Rows.Add(dr);
                    ds.Tables["Items"].AcceptChanges();
                }
				dv = ds.Tables["Items"].DefaultView;
			}
			return (ICollection) dv;

		}

		public SourceCollectionAttribute(string sourceCollectionPropertyName)
		{
			this.srcCollName = sourceCollectionPropertyName;
		}

	}
}