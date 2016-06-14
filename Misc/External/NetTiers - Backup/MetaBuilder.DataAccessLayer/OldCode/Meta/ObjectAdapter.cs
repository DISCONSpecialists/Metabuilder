#region Copyright © 2007 - DISCON Specialists

//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: ObjectAdapter.cs
//

#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;

namespace MetaBuilder.DataAccessLayer.OldCode.Meta
{
    /// <summary>
    /// Summary description for ObjectAdapter.
    /// </summary>
    public class ObjectAdapter
    {
        public ObjectAdapter()
        {
        }


        // TODO: Add user security to this method!
        public DataView RetrieveObjects(string ClassName, int UserID)
        {
            SqlCommand cmd = new SqlCommand("select * from METAView_" + ClassName + "_Retrieval where WorkspaceTypeId=" + Variables.Instance.CurrentWorkspaceTypeId.ToString() + " and WorkspaceName='" + Variables.Instance.CurrentWorkspaceName + "'", new SqlConnection(Variables.Instance.ConnectionString));
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            SqlDataAdapter dap = new SqlDataAdapter();
            dap.SelectCommand = cmd;
            dap.Fill(ds, "Objects");
            return ds.Tables["Objects"].DefaultView;
        }

        public List<MetaObject> GetOrphanedObjects(GraphFileKey ignoreFileKey)
        {

            List<MetaObject> retval = new List<MetaObject>();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT     pkid, Machine ");
            sb.Append("FROM         MetaObject ");
            sb.Append("WHERE     ((CAST(pkid AS varchar(50)) + Machine) NOT IN ");
            sb.Append("(SELECT     CAST(dbo.GraphFileObject.MetaObjectId AS varchar(50)) + dbo.GraphFileObject.MachineID AS Expr1 ");
            sb.Append("FROM          dbo.GraphFile INNER JOIN ");
            sb.Append("dbo.GraphFileObject ON dbo.GraphFile.pkid = dbo.GraphFileObject.GraphFileID AND ");
            sb.Append("dbo.GraphFile.Machine = dbo.GraphFileObject.GraphFileMachine ");
            sb.Append("WHERE      (dbo.GraphFile.IsActive = 'True') AND NOT (dbo.GraphFile.pkid = " + ignoreFileKey.pkid.ToString() + " and dbo.GraphFile.Machine='" + ignoreFileKey.Machine + "'))) ");
            SqlCommand cmd = new SqlCommand(sb.ToString(), new SqlConnection(Variables.Instance.ConnectionString));
            cmd.CommandType = CommandType.Text;

            SqlDataAdapter dap = new SqlDataAdapter();
            dap.SelectCommand = cmd;
            DataSet ds = new DataSet();
            dap.Fill(ds, "OrphanedObjects");

            DataView dv = ds.Tables[0].DefaultView;
            foreach (DataRowView drv in dv)
            {
                MetaObject mo = new MetaObject();
                mo.pkid = int.Parse(drv["pkid"].ToString());
                mo.Machine = drv["Machine"].ToString();
                retval.Add(mo);
            }
            return retval;
        }

        // TODO: Add user security to this method!
        public DataView GetObjectsFiltered(List<string> filters)
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("select * from MetaObject where 1 = 1 ");
            foreach (string s in filters)
            {
                sbQuery.Append(" " + s);
            }
            // Console.WriteLine(sbQuery.ToString());
            SqlCommand cmd = new SqlCommand(sbQuery.ToString(), new SqlConnection(Variables.Instance.ConnectionString));
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            DataSet ds = new DataSet();
            SqlDataAdapter dap = new SqlDataAdapter();
            dap.SelectCommand = cmd;
            dap.Fill(ds, "Objects");
            return ds.Tables["Objects"].DefaultView;
        }
        public DataView GetObjectsFiltered(List<string> filters, bool Server)
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("select * from MetaObject where 1 = 1 ");
            foreach (string s in filters)
            {
                sbQuery.Append(" " + s);
            }
            if (Server)
                Log.WriteLog("(Server) GetObjectsFiltered" + Environment.NewLine + sbQuery);
            // Console.WriteLine(sbQuery.ToString());
            SqlCommand cmd = new SqlCommand(sbQuery.ToString());
            cmd.Connection = Server ? new SqlConnection(Variables.Instance.ServerConnectionString) : new SqlConnection(Variables.Instance.ConnectionString);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            DataSet ds = new DataSet();
            SqlDataAdapter dap = new SqlDataAdapter();
            dap.SelectCommand = cmd;
            dap.Fill(ds, "Objects");
            return ds.Tables["Objects"].DefaultView;
        }
        public DataView ListObjects(string ClassName, int UserID)
        {
            SqlCommand cmd = new SqlCommand("select * from METAView_" + ClassName + "_Listing where WorkspaceTypeId=" + Variables.Instance.CurrentWorkspaceTypeId.ToString() + " and WorkspaceName='" + Variables.Instance.CurrentWorkspaceName + "'", new SqlConnection(Variables.Instance.ConnectionString));
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            SqlDataAdapter dap = new SqlDataAdapter();
            dap.SelectCommand = cmd;
            dap.Fill(ds, "Objects");
            return ds.Tables["Objects"].DefaultView;
        }

        public DataView GetObjects(string ClassName, int ObjectId, string ObjectMachine, int CAid, int UserID)
        {
            SqlCommand cmd = new SqlCommand("select -1 as Associated,* from METAView_" + ClassName + "_Retrieval where Machine='" + ObjectMachine + "' and pkid in (select childObjectId from objectassociation where ObjectId=" + ObjectId.ToString() + " and CAid=" + CAid.ToString() + " and WorkspaceTypeId=" + Variables.Instance.CurrentWorkspaceTypeId.ToString() + " and WorkspaceName='" + Variables.Instance.CurrentWorkspaceName + "') and WorkspaceTypeId=" + Variables.Instance.CurrentWorkspaceTypeId.ToString() + " and WorkspaceName='" + Variables.Instance.CurrentWorkspaceName + "'" + " UNION select 0 as Associated,*  from METAView_" + ClassName + "_Retrieval where pkid not  in (select childObjectId from objectassociation where ObjectId=" + ObjectId.ToString() + " and CAid=" + CAid.ToString() + " and WorkspaceTypeId=" + Variables.Instance.CurrentWorkspaceTypeId.ToString() + " and WorkspaceName='" + Variables.Instance.CurrentWorkspaceName + "'" + " )  and WorkspaceTypeId=" + Variables.Instance.CurrentWorkspaceTypeId.ToString() + " and WorkspaceName='" + Variables.Instance.CurrentWorkspaceName + "'", new SqlConnection(Variables.Instance.ConnectionString));
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            SqlDataAdapter dap = new SqlDataAdapter();
            dap.SelectCommand = cmd;
            dap.Fill(ds, "Objects");
            return ds.Tables["Objects"].DefaultView;
        }

        public void AddObjectAssociation(ObjectAssociation assoc)
        {
            //(int ObjectId, int CAid, int ChildObjectID, string ObjectMachine, string ChildMachine,int VCStatusID, int VCMachineID)
            SqlCommand cmd = new SqlCommand("META_AddObjectAssociation", new SqlConnection(Variables.Instance.ConnectionString));
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@ObjectId", SqlDbType.Int));
            cmd.Parameters["@ObjectId"].Value = assoc.ObjectID;
            cmd.Parameters.Add(new SqlParameter("@CAid", SqlDbType.Int));
            cmd.Parameters["@CAid"].Value = assoc.CAid;
            cmd.Parameters.Add(new SqlParameter("@ChildObjectID", SqlDbType.Int));
            cmd.Parameters["@ChildObjectID"].Value = assoc.ChildObjectID;
            cmd.Parameters.Add(new SqlParameter("@ObjectMachine", SqlDbType.VarChar, 50));
            cmd.Parameters["@ObjectMachine"].Value = assoc.ObjectMachine;
            cmd.Parameters.Add(new SqlParameter("@ChildMachine", SqlDbType.VarChar, 50));
            cmd.Parameters["@ChildMachine"].Value = assoc.ChildObjectMachine;
            cmd.Parameters.Add(new SqlParameter("@VCStatusID", SqlDbType.Int));
            cmd.Parameters["@VCStatusID"].Value = (assoc.VCStatusID > 0) ? assoc.VCStatusID : 1;
            cmd.Parameters.Add(new SqlParameter("@VCMachineID", SqlDbType.VarChar, 50));
            cmd.Parameters["@VCMachineID"].Value = assoc.VCMachineID;

            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch { }
            cmd.Connection.Close();
        }

        public DataSet GetArrayDecomposedObjects(string Class, int UserID)
        {
            SqlCommand cmd = new SqlCommand("META_GetArrayObjects", new SqlConnection(Variables.Instance.ConnectionString));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Class", SqlDbType.VarChar, 50));
            cmd.Parameters["@Class"].Value = Class;
            cmd.Parameters.Add(new SqlParameter("@WorkspaceTypeId", SqlDbType.Int));
            cmd.Parameters["@WorkspaceTypeId"].Value = Variables.Instance.CurrentWorkspaceTypeId;
            cmd.Parameters.Add(new SqlParameter("@WorkspaceName", SqlDbType.VarChar, 100));
            cmd.Parameters["@WorkspaceName"].Value = Variables.Instance.CurrentWorkspaceName;

            //cmd.Parameters.Add("@Objects",SqlDbType.VarChar,8000);

            // Build a string containing each item's id, using braces to differenciate and ensure that only the required objects are returned by the database
            /*
			
            This approach didn't work (and will not work) since it only returns the top-level decompositions. Fetching all objects of this class instead
				
            StringBuilder sb = new StringBuilder();
            for (int i=0;i<ObjectIds.Length;i++)
            {
                sb.Append("[" + ObjectIds[i].ToString() + "]");
            }
            cmd.Parameters["@Objects"].Value = sb.ToString();
            */

            DataSet dsRetval = new DataSet();
            SqlDataAdapter dap = new SqlDataAdapter();
            dap.SelectCommand = cmd;
            // SPROC returns more than one datatable (and dap will create datatables as needed)
            dap.Fill(dsRetval);
            return dsRetval;
        }

        public void DeleteObject(int ObjectId, string ObjectMachine, bool useServer)
        {
            string connString = useServer ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString;
            SqlCommand cmd = new SqlCommand("META_DeleteObject", new SqlConnection(connString));
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@ObjectId", SqlDbType.Int));
            cmd.Parameters["@ObjectId"].Value = ObjectId;
            cmd.Parameters.Add(new SqlParameter("@ObjectMachine", SqlDbType.VarChar, 50));
            cmd.Parameters["@ObjectMachine"].Value = ObjectMachine;
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch
            {
                // Ensure it has the latest query for this rather intricate operation
                StringBuilder sbAlterQuery = new StringBuilder();
                sbAlterQuery.Append("ALTER PROCEDURE [dbo].[META_DeleteObject]" + Environment.NewLine);
                sbAlterQuery.Append("@ObjectId int," + Environment.NewLine);
                sbAlterQuery.Append("@ObjectMachine varchar(50)" + Environment.NewLine);
                sbAlterQuery.Append("as" + Environment.NewLine);
                sbAlterQuery.Append("set nocount on" + Environment.NewLine);
                sbAlterQuery.Append("DELETE FROM ARTIFACT WHERE OBJECTID=@OBJECTID AND OBJECTMACHINE=@OBJECTMACHINE" +
                                    Environment.NewLine);
                sbAlterQuery.Append("DELETE FROM ARTIFACT WHERE CHILDOBJECTID=@OBJECTID AND CHILDOBJECTMACHINE=@OBJECTMACHINE" + Environment.NewLine);
                sbAlterQuery.Append("DELETE FROM ARTIFACT WHERE ARTIFACTOBJECTID=@OBJECTID AND ARTEFACTMACHINE=@OBJECTMACHINE" + Environment.NewLine);
                sbAlterQuery.Append("delete from graphfileobject where metaObjectId=@ObjectId and machineid=@objectmachine" + Environment.NewLine);
                sbAlterQuery.Append("delete from graphfileassociation where  (ObjectId=@ObjectId and ObjectMachine=@objectmachine) or (childObjectId=@ObjectId and childobjectmachine=@objectmachine)" + Environment.NewLine);
                sbAlterQuery.Append("delete from objectassociation where childObjectId=@ObjectId and childobjectmachine=@objectmachine" + Environment.NewLine);
                sbAlterQuery.Append("delete from objectassociation where ObjectId=@ObjectId and objectmachine=@objectmachine" + Environment.NewLine);
                sbAlterQuery.Append("delete from objectfieldvalue where ObjectId=@ObjectId and machineid=@objectmachine" + Environment.NewLine);
                sbAlterQuery.Append("delete from MetaObject where pkid=@ObjectId  and machine=@objectmachine" + Environment.NewLine);
                SqlCommand cmdText = new SqlCommand(sbAlterQuery.ToString(), cmd.Connection);
                cmdText.ExecuteNonQuery();
                cmd.ExecuteNonQuery();
            }
            cmd.Connection.Close();
        }

    }
}