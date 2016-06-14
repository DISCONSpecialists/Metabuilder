#region Copyright © 2007 - DISCON Specialists

//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: AssociationAdapter.cs
//

#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.Meta;

namespace MetaBuilder.DataAccessLayer.OldCode.Meta
{
    /// <summary>
    /// Summary description for AssociationAdapter.
    /// </summary>
    public class AssociationAdapter
    {
        private bool server;
        public bool Server
        {
            get { return server; }
            set { server = value; }
        }

        public AssociationAdapter(bool IsServer)
        {
            Server = IsServer;
        }
        public void DeleteGraphFileAssociationByGraphFileIDGraphFileMachine(int graphfileid, string graphfilemachine)
        {
            SqlCommand cmd = new SqlCommand("META_DeleteGraphFileAssociationsForFile", new SqlConnection(Server ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString));
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@graphfileid", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@graphfilemachine", SqlDbType.VarChar, 50));
            cmd.Parameters["@graphfileid"].Value = graphfileid;
            cmd.Parameters["@graphfilemachine"].Value = graphfilemachine;
            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }
        public List<ObjectAssociation> GetOrphanedAssociations(GraphFileKey ignoreFile)
        {

            List<ObjectAssociation> retval = new List<ObjectAssociation>();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT     CAid, ObjectId, ChildObjectID, ObjectMachine, ChildObjectMachine ");
                sb.Append("FROM         dbo.ObjectAssociation ");
                sb.Append(
                    "WHERE     ((CAST(CAid AS varchar(50)) + '|' + CAST(ObjectId AS varchar(50)) + '|' + CAST(ChildObjectID AS varchar(50)) + '|' + ObjectMachine + '|' + ChildObjectMachine) ");
                sb.Append("NOT IN");
                sb.Append(
                    "(SELECT     CAST(dbo.GraphFileAssociation.CAid AS varchar(50)) + '|' + CAST(dbo.GraphFileAssociation.ObjectID AS varchar(50)) ");
                sb.Append("+ '|' + CAST(dbo.GraphFileAssociation.ChildObjectID AS varchar(50)) ");
                sb.Append(
                    "+ '|' + dbo.GraphFileAssociation.ObjectMachine + '|' + dbo.GraphFileAssociation.ChildObjectMachine AS Expr1 ");
                sb.Append("FROM          dbo.GraphFileAssociation INNER JOIN ");
                sb.Append("dbo.GraphFile ON dbo.GraphFileAssociation.GraphFileID = dbo.GraphFile.pkid AND  ");
                sb.Append("dbo.GraphFileAssociation.GraphFileMachine = dbo.GraphFile.Machine ");
                sb.Append("WHERE      (dbo.GraphFile.IsActive = 'True') and not (dbo.GraphFile.pkid=" +
                          ignoreFile.pkid.ToString() + " and dbo.GraphFile.Machine ='" + ignoreFile.Machine + "'))) ");
                SqlCommand cmd = new SqlCommand(sb.ToString(), new SqlConnection(Server ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString));
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                SqlDataAdapter dap = new SqlDataAdapter();
                dap.SelectCommand = cmd;
                DataSet ds = new DataSet();
                dap.Fill(ds, "ObjectAssociations");

                DataView dv = ds.Tables[0].DefaultView;
                foreach (DataRowView drv in dv)
                {
                    ObjectAssociation assoc = new ObjectAssociation();
                    assoc.CAid = int.Parse(drv["CAid"].ToString());
                    assoc.ObjectID = int.Parse(drv["ObjectId"].ToString());
                    assoc.ChildObjectID = int.Parse(drv["ChildObjectID"].ToString());
                    assoc.ObjectMachine = drv["ObjectMachine"].ToString();
                    assoc.ChildObjectMachine = drv["ChildObjectMachine"].ToString();
                    retval.Add(assoc);
                }
            }
            catch
            {
            }
            return retval;
        }
        public List<ObjectAssociation> GetOrphanedAssociations()
        {

            List<ObjectAssociation> retval = new List<ObjectAssociation>();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT     CAid, ObjectId, ChildObjectID, ObjectMachine, ChildObjectMachine ");
                sb.Append("FROM         dbo.ObjectAssociation ");
                sb.Append(
                    "WHERE     ((CAST(CAid AS varchar(50)) + '|' + CAST(ObjectId AS varchar(50)) + '|' + CAST(ChildObjectID AS varchar(50)) + '|' + ObjectMachine + '|' + ChildObjectMachine) ");
                sb.Append("NOT IN");
                sb.Append(
                    "(SELECT     CAST(dbo.GraphFileAssociation.CAid AS varchar(50)) + '|' + CAST(dbo.GraphFileAssociation.ObjectID AS varchar(50)) ");
                sb.Append("+ '|' + CAST(dbo.GraphFileAssociation.ChildObjectID AS varchar(50)) ");
                sb.Append(
                    "+ '|' + dbo.GraphFileAssociation.ObjectMachine + '|' + dbo.GraphFileAssociation.ChildObjectMachine AS Expr1 ");
                sb.Append("FROM          dbo.GraphFileAssociation INNER JOIN ");
                sb.Append("dbo.GraphFile ON dbo.GraphFileAssociation.GraphFileID = dbo.GraphFile.pkid AND  ");
                sb.Append("dbo.GraphFileAssociation.GraphFileMachine = dbo.GraphFile.Machine ");
                sb.Append("WHERE      (dbo.GraphFile.IsActive = 'True'))) ");
                SqlCommand cmd = new SqlCommand(sb.ToString(), new SqlConnection(Server ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString));
                cmd.CommandType = CommandType.Text;

                SqlDataAdapter dap = new SqlDataAdapter();
                dap.SelectCommand = cmd;
                DataSet ds = new DataSet();
                dap.Fill(ds, "ObjectAssociations");

                DataView dv = ds.Tables[0].DefaultView;
                foreach (DataRowView drv in dv)
                {
                    ObjectAssociation assoc = new ObjectAssociation();
                    assoc.CAid = int.Parse(drv["CAid"].ToString());
                    assoc.ObjectID = int.Parse(drv["ObjectId"].ToString());
                    assoc.ChildObjectID = int.Parse(drv["ChildObjectID"].ToString());
                    assoc.ObjectMachine = drv["ObjectMachine"].ToString();
                    assoc.ChildObjectMachine = drv["ChildObjectMachine"].ToString();
                    retval.Add(assoc);
                }
            }
            catch
            {
            }
            return retval;

        }
        public Association GetAssociation(string ParentClass, string ChildClass, int LimitToAssociationType)
        {
            SqlCommand cmd = new SqlCommand("META_GetAssociationForParentClassAndChildClass", new SqlConnection(Server ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString));
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@ParentClass", SqlDbType.VarChar, 50));
            cmd.Parameters.Add(new SqlParameter("@ChildClass", SqlDbType.VarChar, 50));
            cmd.Parameters.Add(new SqlParameter("@DisplayMember", SqlDbType.VarChar, 150));
            cmd.Parameters["@DisplayMember"].Direction = ParameterDirection.InputOutput;
            cmd.Parameters.Add(new SqlParameter("@AssociationID", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@LimitToAssociationType", SqlDbType.Int));

            cmd.Parameters["@ParentClass"].Value = ParentClass;
            cmd.Parameters["@ChildClass"].Value = ChildClass;
            cmd.Parameters["@AssociationID"].Direction = ParameterDirection.Output;
            cmd.Parameters["@LimitToAssociationType"].Value = LimitToAssociationType;

            //string DisplayMember = cmd.Parameters["@DisplayMember"].Value.ToString();

            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.ExecuteNonQuery();

            Association retval = new Association();
            retval.ParentClass = ParentClass;
            retval.ChildClass = ChildClass;

            cmd.Connection.Close();
            try
            {
                if (cmd.Parameters["@AssociationID"].Value.ToString().Length > 0)
                {
                    retval.ID = int.Parse(cmd.Parameters["@AssociationID"].Value.ToString());
                    retval.Caption = cmd.Parameters["@DisplayMember"].Value.ToString();

                    return retval;
                }
                return null;
            }
            catch { }
            return null;
        }

        /// <summary>
        /// Returns a list of Child and Parent objects in a dataset
        /// </summary>
        /// <param name="ObjectId"></param>
        /// <returns></returns>
        public DataSet GetAssociations(int ObjectId, string ObjectMachine)
        {
            SqlCommand cmd = new SqlCommand("GetAssociationsForParent", new SqlConnection(Server ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@ParentObjectId", SqlDbType.Int));
            cmd.Parameters["@ParentObjectId"].Value = ObjectId;
            cmd.Parameters.Add(new SqlParameter("@ObjectMachine", SqlDbType.VarChar, 50));
            cmd.Parameters["@ObjectMachine"].Value = ObjectMachine;

            DataSet dsRetval = new DataSet();
            SqlDataAdapter dap = new SqlDataAdapter();
            dap.SelectCommand = cmd;
            dap.Fill(dsRetval, "Children");

            cmd = new SqlCommand("GetAssociationsForChild", new SqlConnection(Server ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@ChildObjectID", SqlDbType.Int));
            cmd.Parameters["@ChildObjectID"].Value = ObjectId;
            cmd.Parameters.Add(new SqlParameter("@ChildObjectMachine", SqlDbType.VarChar, 50));
            cmd.Parameters["@ChildObjectMachine"].Value = ObjectMachine;
            dap = new SqlDataAdapter();
            dap.SelectCommand = cmd;
            dap.Fill(dsRetval, "Parents");

            return dsRetval;
        }

        public DataSet GetAssociationDataSet()
        {
            SqlCommand cmd = new SqlCommand("META_GetAllowedClassAssociationCaptions", new SqlConnection(Server ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString));
            cmd.CommandType = CommandType.StoredProcedure;

            DataSet dsRetval = new DataSet();
            SqlDataAdapter dap = new SqlDataAdapter();
            dap.SelectCommand = cmd;
            dap.Fill(dsRetval, "Captions");

            //This table isn't useless! Used by the treeview
            cmd.CommandText = "META_GetAllowedClassAssociationTypes";
            dap.SelectCommand = cmd;
            dap.Fill(dsRetval, "AssociationTypes");


            cmd.CommandText = "META_GetAssociatedClasses";
            dap.SelectCommand = cmd;
            dap.Fill(dsRetval, "Classes");

            DataRelation relation = new DataRelation("AssociationAllowedClasses", dsRetval.Tables["Captions"].Columns["CAid"], dsRetval.Tables["Classes"].Columns["CAid"], true);
            dsRetval.Relations.Add(relation);
            return dsRetval;
        }

        public DataSet GetAssociationInformation()
        {
            SqlCommand cmd = new SqlCommand("META_GetAllowedClassAssociationCaptions", new SqlConnection(Server ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString));
            cmd.CommandType = CommandType.StoredProcedure;

            DataSet dsRetval = new DataSet();
            SqlDataAdapter dap = new SqlDataAdapter();
            cmd.CommandText = "META_GetAssociatedClasses";
            dap.SelectCommand = cmd;
            dap.Fill(dsRetval, "Classes");
            cmd.CommandText = "proc_AllowedArtifact_Get_List";
            dap.SelectCommand = cmd;
            dap.Fill(dsRetval, "ArtifactClasses");

            DataRelation relation = new DataRelation("AssociationArtifactClasses", dsRetval.Tables["Classes"].Columns["CAid"], dsRetval.Tables["ArtifactClasses"].Columns["CAid"], true);
            dsRetval.Relations.Add(relation);
            return dsRetval;
        }

        public DataTable GetAssociatedObjects(int ObjectId, string ObjectMachine)
        {
            SqlCommand cmd = new SqlCommand("META_GetAssociatedObjects", new SqlConnection(Server ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString));
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@ParentObjectId", SqlDbType.Int));
            cmd.Parameters["@ParentObjectId"].Value = ObjectId;
            cmd.Parameters.Add(new SqlParameter("@ParentObjectMachineName", SqlDbType.VarChar, 50));
            cmd.Parameters["@ParentObjectMachineName"].Value = ObjectMachine;

            DataSet dsRetval = new DataSet();
            SqlDataAdapter dap = new SqlDataAdapter();
            dap.SelectCommand = cmd;
            dap.Fill(dsRetval, "RelatedObjects");

            return dsRetval.Tables[0];
        }

        public DataSet GetArtifactData(int CAid, int ObjectId, int ChildObjectID, string ObjectMachine, string ChildMachine)
        {
            SqlCommand cmd = new SqlCommand("META_GetArtifactData", new SqlConnection(Server ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString));
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@CAid", SqlDbType.Int));
            cmd.Parameters["@CAid"].Value = CAid;
            cmd.Parameters.Add(new SqlParameter("@ObjectId", SqlDbType.Int));
            cmd.Parameters["@ObjectId"].Value = ObjectId;
            cmd.Parameters.Add(new SqlParameter("@ChildObjectID", SqlDbType.Int));
            cmd.Parameters["@ChildObjectID"].Value = ChildObjectID;

            cmd.Parameters.Add(new SqlParameter("@ObjectMachine", SqlDbType.VarChar, 50));
            cmd.Parameters["@ObjectMachine"].Value = ObjectMachine;
            cmd.Parameters.Add(new SqlParameter("@ChildMachine", SqlDbType.VarChar, 50));
            cmd.Parameters["@ChildMachine"].Value = ChildMachine;

            SqlDataAdapter dap = new SqlDataAdapter();
            dap.SelectCommand = cmd;

            DataSet retval = new DataSet();
            dap.Fill(retval);
            return retval;
        }

        public void AddArtifact(int CAid, int ObjectId, int ChildObjectID, int ArtifactID, string ObjectMachine, string ChildMachine, string ArtefactMachine)
        {
            SqlCommand cmd = new SqlCommand("META_AddArtifact", new SqlConnection(Server ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString));
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@CAid", SqlDbType.Int));
            cmd.Parameters["@CAid"].Value = CAid;

            cmd.Parameters.Add(new SqlParameter("@ObjectId", SqlDbType.Int));
            cmd.Parameters["@ObjectId"].Value = ObjectId;

            cmd.Parameters.Add(new SqlParameter("@ChildObjectID", SqlDbType.Int));
            cmd.Parameters["@ChildObjectID"].Value = ChildObjectID;

            cmd.Parameters.Add(new SqlParameter("@ArtifactID", SqlDbType.Int));
            cmd.Parameters["@ArtifactID"].Value = ArtifactID;

            cmd.Parameters.Add(new SqlParameter("@ObjectMachine", SqlDbType.VarChar, 50));
            cmd.Parameters["@ObjectMachine"].Value = ObjectMachine;
            cmd.Parameters.Add(new SqlParameter("@ChildMachine", SqlDbType.VarChar, 50));
            cmd.Parameters["@ChildMachine"].Value = ChildMachine;
            cmd.Parameters.Add(new SqlParameter("@ArtefactMachine", SqlDbType.VarChar, 50));
            cmd.Parameters["@ArtefactMachine"].Value = ArtefactMachine;

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
                UpdateAddArtefactStoredProc();
                cmd.ExecuteNonQuery();
            }

            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

        }
        private void UpdateAddArtefactStoredProc()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("ALTER PROCEDURE [dbo].[META_AddArtifact]").Append(Environment.NewLine);
            sb.Append("@CAid int,").Append(Environment.NewLine);
            sb.Append("@ObjectId int,").Append(Environment.NewLine);
            sb.Append("@ChildObjectID int, ").Append(Environment.NewLine);
            sb.Append("@ArtifactID int,").Append(Environment.NewLine);
            sb.Append("@objectmachine varchar(50),").Append(Environment.NewLine);
            sb.Append("@childmachine varchar(50),").Append(Environment.NewLine);
            sb.Append("@artefactmachine varchar(50)").Append(Environment.NewLine);
            sb.Append("as").Append(Environment.NewLine);
            sb.Append("insert into artifact(CAid, ObjectId, childObjectId, artifactObjectId,objectmachine,childobjectmachine,artefactmachine) values (@CAid, @ObjectId,@childObjectId, @artifactid,@objectmachine,@childmachine,@artefactmachine)").Append(Environment.NewLine);
            SqlCommand cmdText = new SqlCommand(sb.ToString(), new SqlConnection(Server ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString));
            cmdText.Connection.Open();
            cmdText.ExecuteNonQuery();
            cmdText.Connection.Close();
        }
        public void CalculateCriticalities()
        {
            SqlCommand cmd = new SqlCommand("META_CalculateCriticalities", new SqlConnection(Server ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString));
            cmd.CommandType = CommandType.StoredProcedure;

            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }

            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

        }

        public void AddAssociationObject(int CAid, int ObjectId, int ChildObjectID, int AssociationObjectId, string ObjectMachine, string ChildMachine, string ArtefactMachine)
        {
            SqlCommand cmd = new SqlCommand("META_AddAssociationObject", new SqlConnection(Server ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString));
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@CAid", SqlDbType.Int));
            cmd.Parameters["@CAid"].Value = CAid;

            cmd.Parameters.Add(new SqlParameter("@ObjectId", SqlDbType.Int));
            cmd.Parameters["@ObjectId"].Value = ObjectId;

            cmd.Parameters.Add(new SqlParameter("@ChildObjectID", SqlDbType.Int));
            cmd.Parameters["@ChildObjectID"].Value = ChildObjectID;

            cmd.Parameters.Add(new SqlParameter("@AssociationObjectId", SqlDbType.Int));
            cmd.Parameters["@AssociationObjectId"].Value = AssociationObjectId;

            cmd.Parameters.Add(new SqlParameter("@ObjectMachine", SqlDbType.VarChar, 50));
            cmd.Parameters["@ObjectMachine"].Value = ObjectMachine;
            cmd.Parameters.Add(new SqlParameter("@ChildMachine", SqlDbType.VarChar, 50));
            cmd.Parameters["@ChildMachine"].Value = ChildMachine;
            cmd.Parameters.Add(new SqlParameter("@ArtefactMachine", SqlDbType.VarChar, 50));
            cmd.Parameters["@ArtefactMachine"].Value = ArtefactMachine;

            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }

            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

        }

        public void RemoveArtifact(int CAid, int ObjectId, int ChildObjectID, int ArtifactID, string ObjectMachine, string ChildMachine, string ArtefactMachine)
        {
            SqlCommand cmd = new SqlCommand("META_RemoveArtifact", new SqlConnection(Server ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString));
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@CAid", SqlDbType.Int));
            cmd.Parameters["@CAid"].Value = CAid;

            cmd.Parameters.Add(new SqlParameter("@ObjectId", SqlDbType.Int));
            cmd.Parameters["@ObjectId"].Value = ObjectId;

            cmd.Parameters.Add(new SqlParameter("@ChildObjectID", SqlDbType.Int));
            cmd.Parameters["@ChildObjectID"].Value = ChildObjectID;

            cmd.Parameters.Add(new SqlParameter("@ArtifactID", SqlDbType.Int));
            cmd.Parameters["@ArtifactID"].Value = ArtifactID;

            cmd.Parameters.Add(new SqlParameter("@ObjectMachine", SqlDbType.VarChar, 50));
            cmd.Parameters["@ObjectMachine"].Value = ObjectMachine;
            cmd.Parameters.Add(new SqlParameter("@ChildMachine", SqlDbType.VarChar, 50));
            cmd.Parameters["@ChildMachine"].Value = ChildMachine;
            cmd.Parameters.Add(new SqlParameter("@ArtefactMachine", SqlDbType.VarChar, 50));
            cmd.Parameters["@ArtefactMachine"].Value = ArtefactMachine;

            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }

            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

        }

        public void ClearArtifacts(int CAid, int ObjectId, int ChildObjectID, string ObjectMachine, string ChildMachine)
        {
            if (CAid > 0)
            {
                SqlCommand cmd = new SqlCommand("META_ClearArtifacts", new SqlConnection(Server ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString));
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@CAid", SqlDbType.Int));
                cmd.Parameters["@CAid"].Value = CAid;

                cmd.Parameters.Add(new SqlParameter("@ObjectId", SqlDbType.Int));
                cmd.Parameters["@ObjectId"].Value = ObjectId;

                cmd.Parameters.Add(new SqlParameter("@ChildObjectID", SqlDbType.Int));
                cmd.Parameters["@ChildObjectID"].Value = ChildObjectID;
                cmd.Parameters.Add(new SqlParameter("@ObjectMachine", SqlDbType.VarChar, 50));
                cmd.Parameters["@ObjectMachine"].Value = ObjectMachine;
                cmd.Parameters.Add(new SqlParameter("@ChildMachine", SqlDbType.VarChar, 50));
                cmd.Parameters["@ChildMachine"].Value = ChildMachine;
                if (cmd.Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }

        }

        public int AddQuickAssociation(int ObjectId1, int ObjectId2, int AssociationTypeID, string ObjectMachine, string ChildMachine)
        {
            SqlCommand cmd = new SqlCommand("META_AddQuickAssociation", new SqlConnection(Server ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString));
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@ObjectId1", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@ObjectId2", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@AssociationTypeID", SqlDbType.Int));
            cmd.Parameters["@ObjectId1"].Value = ObjectId1;
            cmd.Parameters["@ObjectId2"].Value = ObjectId2;
            cmd.Parameters["@AssociationTypeID"].Value = AssociationTypeID;
            cmd.Parameters.Add(new SqlParameter("@CAid", SqlDbType.Int));
            cmd.Parameters["@CAid"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add(new SqlParameter("@ObjectMachine", SqlDbType.VarChar, 50));
            cmd.Parameters["@ObjectMachine"].Value = ObjectMachine;
            cmd.Parameters.Add(new SqlParameter("@ChildMachine", SqlDbType.VarChar, 50));
            cmd.Parameters["@ChildMachine"].Value = ChildMachine;

            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return int.Parse(cmd.Parameters["@CAid"].Value.ToString());

        }
    }
}