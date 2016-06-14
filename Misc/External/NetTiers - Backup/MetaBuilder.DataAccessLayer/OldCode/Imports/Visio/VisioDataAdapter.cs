using System;
using System.Data;
using System.Data.SqlClient;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;

namespace MetaBuilder.DataAccessLayer.OldCode.Imports.Visio
{
	/// <summary>
	/// Summary description for MasterDataAdapter.
	/// </summary>
	public class VisioDataAdapter
	{
		public VisioDataAdapter()
		{
		}


		public DataSet GetVisioMasterInformation()
		{
			SqlCommand cmd = new SqlCommand("VISIO_GetVisioMasters", new SqlConnection(Variables.Instance.ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;

			SqlDataAdapter dap = new SqlDataAdapter();
			DataSet ds = new DataSet();
			dap.SelectCommand = cmd;
			dap.Fill(ds, "VisioMasters");

			cmd = new SqlCommand("VISIO_GetVisioMasterProperties", new SqlConnection(Variables.Instance.ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;

			dap.SelectCommand = cmd;
			dap.Fill(ds, "VisioMasterProperties");
			return ds;
		}

		public int SaveVisioDrawing(string DrawingName, string DrawingVersion, string DrawingType)
		{
            GraphFile gf = new GraphFile();
            gf.AppVersion = "0";
            gf.MinorVersion = 0;
            gf.MajorVersion = 0;
            gf.Name = DrawingName;
            gf.ModifiedDate = DateTime.Now;
            gf.WorkspaceName = Variables.Instance.CurrentWorkspaceName;
            gf.WorkspaceTypeId = Variables.Instance.CurrentWorkspaceTypeId;
            gf.IsActive = true;
            gf.FileTypeID = (int)FileTypeList.Diagram;
            gf.Archived = false;
            gf.VCStatusID = (int)VCStatusList.Locked;

            DataRepository.GraphFileProvider.Save(gf);
            return gf.pkid;

			SqlCommand cmd = new SqlCommand("VISIO_SaveDrawing", new SqlConnection(Variables.Instance.ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@DrawingName", SqlDbType.VarChar, 255));
			cmd.Parameters.Add(new SqlParameter("@DrawingVersion", SqlDbType.VarChar, 10));
			cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar, 10));
			cmd.Parameters.Add(new SqlParameter("@pkid", SqlDbType.Int));
			cmd.Parameters["@pkid"].Direction = ParameterDirection.Output;

			cmd.Parameters["@DrawingName"].Value = DrawingName;
			cmd.Parameters["@DrawingVersion"].Value = DrawingVersion;
			cmd.Parameters["@Type"].Value = DrawingType;
			cmd.Parameters.Add(new SqlParameter("@WorkspaceTypeId", SqlDbType.Int));
            cmd.Parameters["@WorkspaceTypeId"].Value = Variables.Instance.CurrentWorkspaceTypeId;
            cmd.Parameters.Add(new SqlParameter("@WorkspaceName", SqlDbType.VarChar,100));
            cmd.Parameters["@WorkspaceName"].Value = Variables.Instance.CurrentWorkspaceName;
			cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int));
			cmd.Parameters["@UserID"].Value = Variables.Instance.UserID;

			if (cmd.Connection.State != ConnectionState.Open)
			{
				cmd.Connection.Open();
			}
			cmd.ExecuteNonQuery();
			cmd.Connection.Close();

			int DrawingID = int.Parse(cmd.Parameters["@pkid"].Value.ToString());
			return DrawingID;

		}

		public void DeleteVisioDrawing(int DrawingID)
		{
			SqlCommand cmd = new SqlCommand("VISIO_DeleteDrawing", new SqlConnection(Variables.Instance.ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(new SqlParameter("@DrawingID", SqlDbType.Int));
			cmd.Parameters["@DrawingID"].Value = DrawingID;

			if (cmd.Connection.State != ConnectionState.Open)
			{
				cmd.Connection.Open();
			}
			cmd.ExecuteNonQuery();
			cmd.Connection.Close();
		}

		public void AddVisioDrawingAssociation(int DrawingID, int ObjectId1, int ObjectId2, int AssociationTypeID)
		{
            return;
			SqlCommand cmd = new SqlCommand("VISIO_AddDrawingAssociation", new SqlConnection(Variables.Instance.ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@DrawingID", SqlDbType.Int));
			cmd.Parameters.Add(new SqlParameter("@ObjectId1", SqlDbType.Int));
			cmd.Parameters.Add(new SqlParameter("@ObjectId2", SqlDbType.Int));
			cmd.Parameters.Add(new SqlParameter("@AssociationTypeID", SqlDbType.Int));

			cmd.Parameters["@DrawingID"].Value = DrawingID;
			cmd.Parameters["@ObjectId1"].Value = ObjectId1;
			cmd.Parameters["@ObjectId2"].Value = ObjectId2;
			cmd.Parameters["@AssociationTypeID"].Value = AssociationTypeID;

			if (cmd.Connection.State != ConnectionState.Open)
			{
				cmd.Connection.Open();
			}

			//cmd.ExecuteNonQuery();
			cmd.Connection.Close();
		}

		public void AddVisioDrawingObject(int DrawingID, int ObjectId)
		{
			SqlCommand cmd = new SqlCommand("VISIO_AddDrawingObject", new SqlConnection(Variables.Instance.ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@DrawingID", SqlDbType.Int));
			cmd.Parameters.Add(new SqlParameter("@ObjectId", SqlDbType.Int));

			cmd.Parameters["@DrawingID"].Value = DrawingID;
			cmd.Parameters["@ObjectId"].Value = ObjectId;

			if (cmd.Connection.State != ConnectionState.Open)
			{
				cmd.Connection.Open();
			}
			cmd.ExecuteNonQuery();
			cmd.Connection.Close();
		}

		public void AddVisioDrawingFlow(int DrawingID, int FromObj, int ToObj, int ArtifactID)
		{
            return;
			SqlCommand cmd = new SqlCommand("VISIO_OID_AddVisioDrawingFlow", new SqlConnection(Variables.Instance.ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@DrawingID", SqlDbType.Int));
			cmd.Parameters.Add(new SqlParameter("@FromObj", SqlDbType.Int));
			cmd.Parameters.Add(new SqlParameter("@ToObj", SqlDbType.Int));
			cmd.Parameters.Add(new SqlParameter("@ArtifactID", SqlDbType.Int));

			cmd.Parameters["@DrawingID"].Value = DrawingID;
			cmd.Parameters["@FromObj"].Value = FromObj;
			cmd.Parameters["@ToObj"].Value = ToObj;
			cmd.Parameters["@ArtifactID"].Value = ArtifactID;

			if (cmd.Connection.State != ConnectionState.Open)
			{
				cmd.Connection.Open();
			}
			cmd.ExecuteNonQuery();
			cmd.Connection.Close();
		}

		public DataView CheckForExistingDrawing(string DrawingName)
		{
			SqlCommand cmd = new SqlCommand("VISIO_CheckForExistingDrawing", new SqlConnection(Variables.Instance.ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@DrawingName", SqlDbType.VarChar, 255));
			cmd.Parameters.Add(new SqlParameter("@WorkspaceTypeId", SqlDbType.Int));
            cmd.Parameters["@WorkspaceTypeId"].Value = Variables.Instance.CurrentWorkspaceTypeId;
            cmd.Parameters["@WorkspaceName"].Value = Variables.Instance.CurrentWorkspaceName;
            cmd.Parameters.Add(new SqlParameter("@WorkspaceName", SqlDbType.VarChar, 100));

			cmd.Parameters["@DrawingName"].Value = DrawingName;


			DataSet ds = new DataSet();
			SqlDataAdapter dap = new SqlDataAdapter();
			dap.SelectCommand = cmd;
			dap.Fill(ds, "PrevDrawings");
			return ds.Tables["PrevDrawings"].DefaultView;
		}

		public void DeleteDrawing(int DrawingID)
		{
			SqlCommand cmd = new SqlCommand("VISIO_DeleteDrawing", new SqlConnection(Variables.Instance.ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;

			cmd.Parameters.Add(new SqlParameter("@DrawingID", SqlDbType.Int));
			cmd.Parameters["@DrawingID"].Value = DrawingID;

			if (cmd.Connection.State != ConnectionState.Open)
			{
				cmd.Connection.Open();
			}
			cmd.ExecuteNonQuery();
			cmd.Connection.Close();
		}

		public DataView GetOIDDrawings()
		{
			SqlCommand cmd = new SqlCommand("VISIO_OID_GetDrawings", new SqlConnection(Variables.Instance.ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(new SqlParameter("@WorkspaceTypeId", SqlDbType.Int));
            cmd.Parameters["@WorkspaceTypeId"].Value = Variables.Instance.CurrentWorkspaceTypeId;
            cmd.Parameters.Add(new SqlParameter("@WorkspaceName", SqlDbType.VarChar, 100));
            cmd.Parameters["@WorkspaceName"].Value = Variables.Instance.CurrentWorkspaceName;
			SqlDataAdapter dap = new SqlDataAdapter();
			dap.SelectCommand = cmd;
			DataSet ds = new DataSet();
			dap.Fill(ds, "Drawings");
			return ds.Tables["Drawings"].DefaultView;
		}

		public DataView GetADDDrawings()
		{
			SqlCommand cmd = new SqlCommand("VISIO_ADD_GetDrawings", new SqlConnection(Variables.Instance.ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(new SqlParameter("@WorkspaceTypeId", SqlDbType.Int));
            cmd.Parameters["@WorkspaceTypeId"].Value = Variables.Instance.CurrentWorkspaceTypeId;
            cmd.Parameters.Add(new SqlParameter("@WorkspaceName", SqlDbType.VarChar, 100));
            cmd.Parameters["@WorkspaceName"].Value = Variables.Instance.CurrentWorkspaceName;
			SqlDataAdapter dap = new SqlDataAdapter();
			dap.SelectCommand = cmd;
			DataSet ds = new DataSet();
			dap.Fill(ds, "Drawings");
			return ds.Tables["Drawings"].DefaultView;
		}

		public void BuildOIDReportingCache()
		{
			SqlCommand cmd = new SqlCommand("VISIO_OID_BuildReportingCache", new SqlConnection(Variables.Instance.ConnectionString));
			cmd.CommandType = CommandType.StoredProcedure;


			if (cmd.Connection.State != ConnectionState.Open)
			{
				cmd.Connection.Open();
			}
			//cmd.ExecuteNonQuery();
			cmd.Connection.Close();
		}
	}
}