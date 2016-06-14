#region Copyright © 2007 - DISCON Specialists

//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: ClassDefinitionAdapter.cs
//

#endregion

using System.Data;
using System.Data.SqlClient;
using MetaBuilder.Core;

namespace MetaBuilder.DataAccessLayer.OldCode.Meta
{
	/// <summary>
	/// Summary description for ClassDefinitionAdapter.
	/// </summary>
	public class ClassDefinitionAdapter
	{
		public ClassDefinitionAdapter()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public void MarkClassDefinitionsAsDirty()
		{
			SqlCommand cmd = new SqlCommand("META_MarkClassDefinitionsAsDirty", new SqlConnection(Variables.Instance.ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;

			if (cmd.Connection.State != ConnectionState.Open)
			{
				cmd.Connection.Open();
			}
			cmd.ExecuteNonQuery();
			cmd.Connection.Close();
		}

		public void MarkClassDefinitionsAsClean()
		{
			SqlCommand cmd = new SqlCommand("META_MarkClassDefinitionsAsClean", new SqlConnection(Variables.Instance.ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;

			if (cmd.Connection.State != ConnectionState.Open)
			{
				cmd.Connection.Open();
			}
			cmd.ExecuteNonQuery();
			cmd.Connection.Close();
		}

		public DataView GetClasses(bool onlyActive)
		{
			SqlCommand cmd = new SqlCommand("META_GetClasses", new SqlConnection(Variables.Instance.ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;

            if (onlyActive)
            {
                cmd.Parameters.Add(new SqlParameter("@ActiveOnly", SqlDbType.Bit));
                cmd.Parameters["@ActiveOnly"].Value = 1;
            }
			DataSet ds = new DataSet();
			SqlDataAdapter dap = new SqlDataAdapter();
			dap.SelectCommand = cmd;
			dap.Fill(ds, "Classes");
			return ds.Tables["Classes"].DefaultView;
		}

		public DataView GetAllowedAssociateClasses(string ParentClassName, bool ActiveOnly)
		{
			SqlCommand cmd = new SqlCommand("META_GetAllowedAssociationClasses", new SqlConnection(Variables.Instance.ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(new SqlParameter("@ParentClassName", SqlDbType.VarChar, 50));
			cmd.Parameters["@ParentClassName"].Value = ParentClassName;
            if (ActiveOnly)
            {
                cmd.Parameters.Add(new SqlParameter("@ActiveOnly", SqlDbType.Bit));
                cmd.Parameters["@ActiveOnly"].Value = 1;
            }
			DataSet ds = new DataSet();
			SqlDataAdapter dap = new SqlDataAdapter();
			dap.SelectCommand = cmd;
			dap.Fill(ds, "Classes");
			return ds.Tables["Classes"].DefaultView;
		}

		public DataView GetAllowedAssociateClasses(string ParentClassName, string LimitToCaption, bool ActiveOnly)
		{
			SqlCommand cmd = new SqlCommand("META_GetAllowedAssociationClassesLimitToCaption", new SqlConnection(Variables.Instance.ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(new SqlParameter("@ParentClassName", SqlDbType.VarChar, 50));
			cmd.Parameters["@ParentClassName"].Value = ParentClassName;
			cmd.Parameters.Add(new SqlParameter("@LimitToCaption", SqlDbType.VarChar, 50));
			cmd.Parameters["@LimitToCaption"].Value = LimitToCaption;
            if (ActiveOnly)
            {
                cmd.Parameters.Add(new SqlParameter("@ActiveOnly", SqlDbType.Bit));
                cmd.Parameters["@ActiveOnly"].Value = 1;
            }
			DataSet ds = new DataSet();
			SqlDataAdapter dap = new SqlDataAdapter();
			dap.SelectCommand = cmd;
			dap.Fill(ds, "Classes");
			return ds.Tables["Classes"].DefaultView;
		}

		public DataView GetAllowedAssociateClasses(string ParentClassName, int LimitToAssociationType, bool ActiveOnly)
		{
			SqlCommand cmd = new SqlCommand("META_GetAllowedAssociationClassesLimitToAssociationType", new SqlConnection(Variables.Instance.ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(new SqlParameter("@ParentClassName", SqlDbType.VarChar, 50));
			cmd.Parameters["@ParentClassName"].Value = ParentClassName;
			cmd.Parameters.Add(new SqlParameter("@LimitToAssociationType", SqlDbType.Int));
			cmd.Parameters["@LimitToAssociationType"].Value = LimitToAssociationType;
            if (ActiveOnly)
            {
                cmd.Parameters.Add(new SqlParameter("@ActiveOnly", SqlDbType.Bit));
                cmd.Parameters["@ActiveOnly"].Value = 1;
            }

			DataSet ds = new DataSet();
			SqlDataAdapter dap = new SqlDataAdapter();
			dap.SelectCommand = cmd;
			dap.Fill(ds, "Classes");
			return ds.Tables["Classes"].DefaultView;
		}

		public DataSet GetClassesAndFields(bool ActiveOnly)
		{
			SqlCommand cmd = new SqlCommand("META_GetClasses", new SqlConnection(Variables.Instance.ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;

			DataSet ds = new DataSet();
			SqlDataAdapter dap = new SqlDataAdapter();
			dap.SelectCommand = cmd;
            if (ActiveOnly)
            {
                cmd.Parameters.Add(new SqlParameter("@ActiveOnly", SqlDbType.Bit));
                cmd.Parameters["@ActiveOnly"].Value = 1;
            }
			dap.Fill(ds, "Classes");

			cmd.CommandText = "META_GetFields";
			dap.Fill(ds, "Fields");

			DataColumn dcParent = ds.Tables["Classes"].Columns["Name"];
			DataColumn dcChild = ds.Tables["Fields"].Columns["Class"];
			DataRelation relClassField = new DataRelation("ClassField", dcParent, dcChild, false);
			ds.Relations.Add(relClassField);
			return ds;
		}

		public DataView GetCategories()
		{
			SqlCommand cmd = new SqlCommand("META_GetCategories", new SqlConnection(Variables.Instance.ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;
			DataSet ds = new DataSet();
			SqlDataAdapter dap = new SqlDataAdapter();
			dap.SelectCommand = cmd;
			dap.Fill(ds, "Categories");
			return ds.Tables["Categories"].DefaultView;
		}

	}
}