using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
using System.IO;

namespace MetaBuilder.DataAccessLayer.OldCode.Diagramming
{
    public class TempFileGraphAdapter
    {
        public TList<GraphFile> GetAllFilesByWorkspaceTypeIdWorkspaceName(int WorkspaceTypeId, string WorkspaceName, int FileTypeID, bool useServer)
        {
            string connString = (useServer) ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString;
            //SqlCommand cmd = new SqlCommand("sp_Diagramming_Files_GetListByWorkspaceIDandFileTypeID", new SqlConnection(connString));
            SqlCommand cmd = new SqlCommand("select * from graphfile where FileTypeID=" + FileTypeID.ToString() + " and WorkspaceName = '" + WorkspaceName + "' and WorkspaceTypeID = " + WorkspaceTypeId.ToString(), new SqlConnection(connString));

            //cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandType = CommandType.Text;
            //cmd.Parameters.Add(new SqlParameter("@WorkspaceTypeId", SqlDbType.Int));
            //cmd.Parameters.Add(new SqlParameter("@WorkspaceName", SqlDbType.VarChar, 100));
            //cmd.Parameters.Add(new SqlParameter("@FileTypeID", SqlDbType.Int));
            //cmd.Parameters["@WorkspaceTypeId"].Value = WorkspaceTypeId;
            //cmd.Parameters["@WorkspaceName"].Value = WorkspaceName;
            //cmd.Parameters["@FileTypeID"].Value = FileTypeID;
            SqlDataAdapter dap = new SqlDataAdapter();
            DataSet ds = new DataSet();
            dap.SelectCommand = cmd;
            dap.Fill(ds, "Files");
            TList<GraphFile> fileList = new TList<GraphFile>();
            foreach (DataRowView drvFile in ds.Tables["Files"].DefaultView)
            {
                GraphFile graphFile = new GraphFile();
                graphFile.pkid = int.Parse(drvFile["pkid"].ToString());
                graphFile.IsActive = bool.Parse(drvFile["isactive"].ToString());
                graphFile.Name = drvFile["Name"].ToString();
                graphFile.MajorVersion = int.Parse(drvFile["MajorVersion"].ToString());
                graphFile.MinorVersion = int.Parse(drvFile["MinorVersion"].ToString());
                graphFile.VCStatusID = int.Parse(drvFile["VCStatusID"].ToString());
                graphFile.FileTypeID = FileTypeID;
                graphFile.Machine = drvFile["Machine"].ToString();
                graphFile.ModifiedDate = Core.GlobalParser.ParseGlobalisedDateString(drvFile["ModifiedDate"].ToString());
                graphFile.WorkspaceName = WorkspaceName;
                graphFile.WorkspaceTypeId = WorkspaceTypeId;
                //graphFile.IsActive = true;// bool.Parse(drvFile["IsActive"].ToString());
                graphFile.OriginalFileUniqueID = new Guid(drvFile["OriginalFileUniqueID"].ToString());
                graphFile.VCMachineID = drvFile["VCMachineID"].ToString();

                try
                {
                    graphFile.Blob = (byte[])drvFile["blob"];
                }
                catch (Exception ex)
                {
                    Log.WriteLog("Cannot read byte[] for file " + graphFile.pkid + Environment.NewLine + ex.ToString());
                }
                graphFile.EntityState = EntityState.Unchanged;
                fileList.Add(graphFile);
            }
            return fileList;
        }
        public TList<GraphFile> GetFilesByWorkspaceTypeIdWorkspaceName(int WorkspaceTypeId, string WorkspaceName, int FileTypeID, bool useServer)
        {
            string connString = (useServer) ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString;
            //SqlCommand cmd = new SqlCommand("sp_Diagramming_Files_GetListByWorkspaceIDandFileTypeID", new SqlConnection(connString));
            SqlCommand cmd = new SqlCommand("select * from graphfile where FileTypeID=" + FileTypeID.ToString() + " and IsActive=1 and WorkspaceName = '" + WorkspaceName + "' and WorkspaceTypeID = " + WorkspaceTypeId.ToString(), new SqlConnection(connString));

            //cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandType = CommandType.Text;
            //cmd.Parameters.Add(new SqlParameter("@WorkspaceTypeId", SqlDbType.Int));
            //cmd.Parameters.Add(new SqlParameter("@WorkspaceName", SqlDbType.VarChar, 100));
            //cmd.Parameters.Add(new SqlParameter("@FileTypeID", SqlDbType.Int));
            //cmd.Parameters["@WorkspaceTypeId"].Value = WorkspaceTypeId;
            //cmd.Parameters["@WorkspaceName"].Value = WorkspaceName;
            //cmd.Parameters["@FileTypeID"].Value = FileTypeID;
            SqlDataAdapter dap = new SqlDataAdapter();
            DataSet ds = new DataSet();
            dap.SelectCommand = cmd;
            dap.Fill(ds, "Files");
            TList<GraphFile> fileList = new TList<GraphFile>();
            foreach (DataRowView drvFile in ds.Tables["Files"].DefaultView)
            {
                GraphFile graphFile = new GraphFile();
                graphFile.pkid = int.Parse(drvFile["pkid"].ToString());
                graphFile.Name = drvFile["Name"].ToString();
                graphFile.MajorVersion = int.Parse(drvFile["MajorVersion"].ToString());
                graphFile.MinorVersion = int.Parse(drvFile["MinorVersion"].ToString());
                graphFile.VCStatusID = int.Parse(drvFile["VCStatusID"].ToString());
                graphFile.FileTypeID = FileTypeID;
                graphFile.Machine = drvFile["Machine"].ToString();
                graphFile.ModifiedDate = Core.GlobalParser.ParseGlobalisedDateString(drvFile["ModifiedDate"].ToString());
                graphFile.WorkspaceName = WorkspaceName;
                graphFile.WorkspaceTypeId = WorkspaceTypeId;
                graphFile.IsActive = true;// bool.Parse(drvFile["IsActive"].ToString());
                graphFile.OriginalFileUniqueID = new Guid(drvFile["OriginalFileUniqueID"].ToString());
                graphFile.VCMachineID = drvFile["VCMachineID"].ToString();

                try
                {
                    graphFile.Blob = (byte[])drvFile["blob"];
                }
                catch (Exception ex)
                {
                    Log.WriteLog("Cannot read byte[] for file " + graphFile.pkid + Environment.NewLine + ex.ToString());
                }
                graphFile.EntityState = EntityState.Unchanged;
                fileList.Add(graphFile);
            }
            return fileList;
        }
        public TList<GraphFile> GetAllFilesByObjectId(int ObjectId, string machine, bool useServer)
        {
            string connString = (useServer) ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString;
            SqlCommand cmd = new SqlCommand("select graphfile.* from graphfileobject inner join graphfile on graphfileobject.graphfileid = graphfile.pkid where graphfileobject.machineid='" + machine + "' and graphfileobject.metaObjectId=" + ObjectId.ToString(), new SqlConnection(connString));

            cmd.CommandType = CommandType.Text;
            SqlDataAdapter dap = new SqlDataAdapter();
            DataSet ds = new DataSet();
            dap.SelectCommand = cmd;
            dap.Fill(ds, "Files");
            TList<GraphFile> fileList = new TList<GraphFile>();
            foreach (DataRowView drvFile in ds.Tables["Files"].DefaultView)
            {
                GraphFile graphFile = new GraphFile();
                graphFile.pkid = int.Parse(drvFile["pkid"].ToString());
                graphFile.IsActive = bool.Parse(drvFile["isactive"].ToString());
                graphFile.Name = drvFile["Name"].ToString();
                graphFile.MajorVersion = int.Parse(drvFile["MajorVersion"].ToString());
                graphFile.MinorVersion = int.Parse(drvFile["MinorVersion"].ToString());
                graphFile.VCStatusID = int.Parse(drvFile["VCStatusID"].ToString());
                graphFile.FileTypeID = int.Parse(drvFile["FileTypeID"].ToString());
                graphFile.Machine = drvFile["Machine"].ToString();
                graphFile.ModifiedDate = Core.GlobalParser.ParseGlobalisedDateString(drvFile["ModifiedDate"].ToString());
                graphFile.WorkspaceName = drvFile["WorkspaceName"].ToString();
                graphFile.WorkspaceTypeId = int.Parse(drvFile["WorkspaceTypeId"].ToString());
                //graphFile.IsActive = true;// bool.Parse(drvFile["IsActive"].ToString());
                graphFile.OriginalFileUniqueID = new Guid(drvFile["OriginalFileUniqueID"].ToString());
                graphFile.VCMachineID = drvFile["VCMachineID"].ToString();

                try
                {
                    graphFile.Blob = (byte[])drvFile["blob"];
                }
                catch (Exception ex)
                {
                    Log.WriteLog("Cannot read byte[] for file " + graphFile.pkid + Environment.NewLine + ex.ToString());
                }
                graphFile.EntityState = EntityState.Unchanged;
                fileList.Add(graphFile);
            }
            return fileList;
        }

        //public TList<GraphFile> GetAllFilesByObjectId(int ObjectId, string machine, bool useServer)
        //{
        //    string connString = (useServer) ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString;
        //    SqlCommand cmd = new SqlCommand("select graphfile.* from graphfileobject inner join graphfile on graphfileobject.graphfileid = graphfile.pkid where graphfileobject.machineid='" + machine + "' and graphfileobject.metaObjectId=" + ObjectId.ToString(), new SqlConnection(connString));

        //    cmd.CommandType = CommandType.Text;
        //    SqlDataAdapter dap = new SqlDataAdapter();
        //    DataSet ds = new DataSet();
        //    dap.SelectCommand = cmd;
        //    dap.Fill(ds, "Files");
        //    TList<GraphFile> fileList = new TList<GraphFile>();
        //    foreach (DataRowView drvFile in ds.Tables["Files"].DefaultView)
        //    {
        //        GraphFile graphFile = new GraphFile();
        //        graphFile.pkid = int.Parse(drvFile["pkid"].ToString());
        //        graphFile.IsActive = bool.Parse(drvFile["isactive"].ToString());
        //        graphFile.Name = drvFile["Name"].ToString();
        //        graphFile.MajorVersion = int.Parse(drvFile["MajorVersion"].ToString());
        //        graphFile.MinorVersion = int.Parse(drvFile["MinorVersion"].ToString());
        //        graphFile.VCStatusID = int.Parse(drvFile["VCStatusID"].ToString());
        //        graphFile.FileTypeID = int.Parse(drvFile["FileTypeID"].ToString());
        //        graphFile.Machine = drvFile["Machine"].ToString();
        //        graphFile.ModifiedDate = Core.GlobalParser.ParseGlobalisedDateString(drvFile["ModifiedDate"].ToString());
        //        graphFile.WorkspaceName = drvFile["WorkspaceName"].ToString();
        //        graphFile.WorkspaceTypeId = int.Parse(drvFile["WorkspaceTypeId"].ToString());
        //        graphFile.IsActive = true;// bool.Parse(drvFile["IsActive"].ToString());
        //        graphFile.OriginalFileUniqueID = new Guid(drvFile["OriginalFileUniqueID"].ToString());
        //        graphFile.VCMachineID = drvFile["VCMachineID"].ToString();

        //        try
        //        {
        //            graphFile.Blob = (byte[])drvFile["blob"];
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.WriteLog("Cannot read byte[] for file " + graphFile.pkid + Environment.NewLine + ex.ToString());
        //        }
        //        graphFile.EntityState = EntityState.Unchanged;
        //        fileList.Add(graphFile);
        //    }
        //    return fileList;
        //}
        public TList<GraphFile> GetFilesByObjectId(int ObjectId, string machine, bool useServer)
        {
            string connString = (useServer) ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString;
            SqlCommand cmd = new SqlCommand("select graphfile.* from graphfileobject inner join graphfile on graphfileobject.graphfileid = graphfile.pkid where graphfile.isactive=1 and graphfileobject.machineid='" + machine + "' and graphfileobject.metaObjectId=" + ObjectId.ToString(), new SqlConnection(connString));

            cmd.CommandType = CommandType.Text;
            SqlDataAdapter dap = new SqlDataAdapter();
            DataSet ds = new DataSet();
            dap.SelectCommand = cmd;
            dap.Fill(ds, "Files");
            TList<GraphFile> fileList = new TList<GraphFile>();
            foreach (DataRowView drvFile in ds.Tables["Files"].DefaultView)
            {
                GraphFile graphFile = new GraphFile();
                graphFile.pkid = int.Parse(drvFile["pkid"].ToString());
                graphFile.Name = drvFile["Name"].ToString();
                graphFile.MajorVersion = int.Parse(drvFile["MajorVersion"].ToString());
                graphFile.MinorVersion = int.Parse(drvFile["MinorVersion"].ToString());
                graphFile.VCStatusID = int.Parse(drvFile["VCStatusID"].ToString());
                graphFile.FileTypeID = int.Parse(drvFile["FileTypeID"].ToString());
                graphFile.Machine = drvFile["Machine"].ToString();
                graphFile.ModifiedDate = Core.GlobalParser.ParseGlobalisedDateString(drvFile["ModifiedDate"].ToString());
                graphFile.WorkspaceName = drvFile["WorkspaceName"].ToString();
                graphFile.WorkspaceTypeId = int.Parse(drvFile["WorkspaceTypeId"].ToString());
                graphFile.IsActive = true;// bool.Parse(drvFile["IsActive"].ToString());
                graphFile.OriginalFileUniqueID = new Guid(drvFile["OriginalFileUniqueID"].ToString());
                graphFile.VCMachineID = drvFile["VCMachineID"].ToString();

                try
                {
                    graphFile.Blob = (byte[])drvFile["blob"];
                }
                catch (Exception ex)
                {
                    Log.WriteLog("Cannot read byte[] for file " + graphFile.pkid + Environment.NewLine + ex.ToString());
                }
                graphFile.EntityState = EntityState.Unchanged;
                fileList.Add(graphFile);
            }
            return fileList;
        }

        public TList<GraphFile> GetPreviousVersions(int filepkid, string filemachine, bool useServer)
        {
            string connString = (useServer) ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString;
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT     pkid, modifieddate,FileTypeID,Name, MajorVersion, MinorVersion, VCStatusID, Machine, WorkspaceName, WorkspaceTypeId, OriginalFileUniqueID ");
            sbQuery.Append(" FROM         GraphFile WHERE   ISACTIVE=0 AND   (OriginalFileUniqueID = ");
            sbQuery.Append("(SELECT     OriginalFileUniqueID FROM          GraphFile AS GraphFile_1 ");
            sbQuery.Append("WHERE      (pkid = " + filepkid.ToString() + ") AND (Machine = '" + filemachine + "'))) AND (pkid <> " + filepkid.ToString() + ")");
            SqlCommand cmd = new SqlCommand(sbQuery.ToString(), new SqlConnection(connString));

            cmd.CommandType = CommandType.Text;
            SqlDataAdapter dap = new SqlDataAdapter();
            DataSet ds = new DataSet();
            dap.SelectCommand = cmd;
            dap.Fill(ds, "Files");
            TList<GraphFile> fileList = new TList<GraphFile>();
            foreach (DataRowView drvFile in ds.Tables["Files"].DefaultView)
            {
                GraphFile graphFile = new GraphFile();
                graphFile.pkid = int.Parse(drvFile["pkid"].ToString());
                graphFile.Name = drvFile["Name"].ToString();
                graphFile.MajorVersion = int.Parse(drvFile["MajorVersion"].ToString());
                graphFile.MinorVersion = int.Parse(drvFile["MinorVersion"].ToString());
                graphFile.VCStatusID = int.Parse(drvFile["VCStatusID"].ToString());
                graphFile.FileTypeID = int.Parse(drvFile["FileTypeID"].ToString());
                graphFile.Machine = drvFile["Machine"].ToString();
                graphFile.WorkspaceName = drvFile["WorkspaceName"].ToString();
                graphFile.WorkspaceTypeId = int.Parse(drvFile["WorkspaceTypeId"].ToString());
                graphFile.IsActive = false;// bool.Parse(drvFile["IsActive"].ToString());
                graphFile.OriginalFileUniqueID = new Guid(drvFile["OriginalFileUniqueID"].ToString());
                graphFile.ModifiedDate = Core.GlobalParser.ParseGlobalisedDateString(drvFile["ModifiedDate"].ToString());
                graphFile.EntityState = EntityState.Unchanged;
                fileList.Add(graphFile);
            }
            return fileList;
        }
        public TList<GraphFile> GetAllFilesByTypeID(int FileTypeID, bool useServer)
        {
            string connString = (useServer) ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString;
            SqlCommand cmd = new SqlCommand("select * from graphfile where FileTypeID=" + FileTypeID.ToString(), new SqlConnection(connString));

            cmd.CommandType = CommandType.Text;
            SqlDataAdapter dap = new SqlDataAdapter();
            DataSet ds = new DataSet();
            dap.SelectCommand = cmd;
            dap.Fill(ds, "Files");
            TList<GraphFile> fileList = new TList<GraphFile>();
            foreach (DataRowView drvFile in ds.Tables["Files"].DefaultView)
            {
                GraphFile graphFile = new GraphFile();
                graphFile.pkid = int.Parse(drvFile["pkid"].ToString());
                graphFile.Name = drvFile["Name"].ToString();
                graphFile.MajorVersion = int.Parse(drvFile["MajorVersion"].ToString());
                graphFile.MinorVersion = int.Parse(drvFile["MinorVersion"].ToString());
                graphFile.VCStatusID = int.Parse(drvFile["VCStatusID"].ToString());
                graphFile.FileTypeID = FileTypeID;
                graphFile.Machine = drvFile["Machine"].ToString();
                graphFile.WorkspaceName = drvFile["WorkspaceName"].ToString();
                graphFile.WorkspaceTypeId = int.Parse(drvFile["WorkspaceTypeId"].ToString());
                graphFile.IsActive = bool.Parse(drvFile["IsActive"].ToString());
                graphFile.OriginalFileUniqueID = new Guid(drvFile["OriginalFileUniqueID"].ToString());
                graphFile.ModifiedDate = Core.GlobalParser.ParseGlobalisedDateString(drvFile["ModifiedDate"].ToString());
                graphFile.VCMachineID = drvFile["VCMachineID"].ToString();
                try
                {
                    if (drvFile["PreviousVersionID"] != null)
                        graphFile.PreviousVersionID = int.Parse(drvFile["PreviousVersionID"].ToString());
                }
                catch
                {
                }

                try
                {
                    graphFile.Blob = (byte[])drvFile["blob"];
                }
                catch (Exception ex)
                {
                    Log.WriteLog("Cannot read byte[] for file " + graphFile.pkid + Environment.NewLine + ex.ToString());
                }
                graphFile.EntityState = EntityState.Unchanged;
                fileList.Add(graphFile);
            }
            return fileList;
        }

        public TList<GraphFile> GetFilesByTypeID(int FileTypeID, bool useServer)
        {
            string connString = (useServer) ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString;
            SqlCommand cmd = new SqlCommand("select * from graphfile where FileTypeID=" + FileTypeID.ToString() + " and IsActive=1", new SqlConnection(connString));

            cmd.CommandType = CommandType.Text;
            SqlDataAdapter dap = new SqlDataAdapter();
            DataSet ds = new DataSet();
            dap.SelectCommand = cmd;
            dap.Fill(ds, "Files");
            TList<GraphFile> fileList = new TList<GraphFile>();
            foreach (DataRowView drvFile in ds.Tables["Files"].DefaultView)
            {
                GraphFile graphFile = new GraphFile();
                graphFile.pkid = int.Parse(drvFile["pkid"].ToString());
                graphFile.Name = drvFile["Name"].ToString();
                graphFile.MajorVersion = int.Parse(drvFile["MajorVersion"].ToString());
                graphFile.MinorVersion = int.Parse(drvFile["MinorVersion"].ToString());
                graphFile.VCStatusID = int.Parse(drvFile["VCStatusID"].ToString());
                graphFile.FileTypeID = FileTypeID;
                graphFile.ModifiedDate = Core.GlobalParser.ParseGlobalisedDateString(drvFile["ModifiedDate"].ToString());
                graphFile.Machine = drvFile["Machine"].ToString();
                graphFile.WorkspaceName = drvFile["WorkspaceName"].ToString();
                graphFile.WorkspaceTypeId = int.Parse(drvFile["WorkspaceTypeId"].ToString());
                graphFile.IsActive = bool.Parse(drvFile["IsActive"].ToString());
                graphFile.OriginalFileUniqueID = new Guid(drvFile["OriginalFileUniqueID"].ToString());
                graphFile.EntityState = EntityState.Unchanged;
                fileList.Add(graphFile);
            }
            return fileList;
        }
        public b.GraphFileKey InsertFile(GraphFile file, int WorkspaceTypeId, string WorkspaceName, string providername)
        {
            if (file.VCStatusID == (int)VCStatusList.CheckedOut)
            {
                file.Notes = "Checked out by: " + Variables.Instance.UserDomainIdentifier;
                file.VCMachineID = Variables.Instance.UserDomainIdentifier;
            }
            file.WorkspaceName = WorkspaceName;
            file.WorkspaceTypeId = WorkspaceTypeId;

            b.GraphFile newFile = new GraphFile();
            newFile.Machine = file.Machine;

            //if (file.VCStatusID == (int)VCStatusList.CheckedIn)
            newFile.PreviousVersionID = file.pkid;
            //else
            //    newFile.PreviousVersionID = null;

            newFile.VCMachineID = file.VCMachineID;
            newFile.Name = file.Name;
            newFile.Notes = file.Notes;

            newFile.VCStatusID = file.VCStatusID;
            newFile.VCUser = file.VCUser;
            newFile.WorkspaceName = file.WorkspaceName;
            newFile.WorkspaceTypeId = file.WorkspaceTypeId;
            newFile.Blob = file.Blob;

            newFile.ModifiedDate = DateTime.Now; //file.ModifiedDate;
            newFile.FileTypeID = (int)FileTypeList.Diagram;
            newFile.MajorVersion = file.MajorVersion;
            newFile.MinorVersion = file.MinorVersion;
            newFile.OriginalFileUniqueID = file.OriginalFileUniqueID;
            newFile.OriginalMachine = file.OriginalMachine;
            //RunQuery(connectionString, "SET IDENTITY_INSERT GRAPHFILE ON");
            newFile.EntityState = EntityState.Added;
            newFile.IsActive = true;

            MarkPreviousVersionsInactive(file, providername);

            DataRepository.Connections[providername].Provider.GraphFileProvider.Save(newFile);
            //RunQuery(connectionString, "SET IDENTITY_INSERT GRAPHFILE OFF");

            /*string oldConstring = d.DataRepository.ConnectionStrings[0].ConnectionString;
            d.DataRepository.ConnectionStrings[0].ConnectionString = connectionString;
            d.DataRepository.GraphFileProvider.Save(file);
            d.DataRepository.ConnectionStrings[0].ConnectionString = oldConstring;*/

            //Set file here so it can be returned from sub as reference to savegraphfile for checking CMD to execute
            file.PreviousVersionID = file.pkid;
            file.pkid = newFile.pkid;
            file.Machine = newFile.Machine;

            return new GraphFileKey(newFile);
        }
        public void MarkPreviousVersionsInactive(GraphFile file, string providername)
        {
            StringBuilder sbQuery = new StringBuilder();
            //foreach (GraphFile dbFile in DataRepository.Connections[providername].Provider.GraphFileProvider.Find("OriginalFileUniqueID='" + file.OriginalFileUniqueID.ToString() + "'"))
            //{
            //    dbFile.IsActive = false;
            //    DataRepository.Connections[providername].Provider.GraphFileProvider.Save(dbFile);
            //}
            sbQuery.Append("UPDATE GRAPHFILE SET ISACTIVE=0  WHERE ISACTIVE=1 AND   (OriginalFileUniqueID = '" + file.OriginalFileUniqueID.ToString() + "')");

            DataRepository.Connections[providername].Provider.ExecuteNonQuery(CommandType.Text, sbQuery.ToString());
        }
        public GraphFile GetQuickFileDetails(int WorkspaceTypeId, string WorkspaceName, int FilePKID, string MachineName, bool useServer)
        {
            string connString = useServer ? Core.Variables.Instance.ServerConnectionString : Core.Variables.Instance.ConnectionString;
            SqlCommand cmd = new SqlCommand("select * from graphfile where pkid=" + FilePKID.ToString() + " and Machine='" + MachineName + "'", new SqlConnection(connString));
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter dap = new SqlDataAdapter();
            DataSet ds = new DataSet();
            dap.SelectCommand = cmd;
            dap.Fill(ds, "FileDetails");
            DataRowView drvFile = ds.Tables["FileDetails"].DefaultView[0];
            GraphFile graphFile = new GraphFile();
            graphFile.pkid = int.Parse(drvFile["pkid"].ToString());
            graphFile.IsActive = bool.Parse(drvFile["isactive"].ToString());
            graphFile.Name = drvFile["Name"].ToString();
            graphFile.MajorVersion = int.Parse(drvFile["MajorVersion"].ToString());
            graphFile.MinorVersion = int.Parse(drvFile["MinorVersion"].ToString());
            graphFile.Machine = MachineName;
            graphFile.IsActive = bool.Parse(drvFile["IsActive"].ToString());
            graphFile.OriginalFileUniqueID = new Guid(drvFile["originalfileuniqueid"].ToString());
            try
            {
                graphFile.Blob = (byte[])drvFile["blob"];
            }
            catch (Exception ex)
            {
                Log.WriteLog("Cannot read byte[] for file " + graphFile.pkid + Environment.NewLine + ex.ToString());
            }
            graphFile.EntityState = EntityState.Unchanged;
            return graphFile;
        }

        private void RunQuery(string connectionString, string Query)
        {
            SqlCommand cmd = new SqlCommand(Query, new SqlConnection(connectionString));
            cmd.CommandType = CommandType.Text;
            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }
        public GraphFile GetQuickFileDetails(int FilePKID, string MachineName, bool useServer)
        {
            string connString = useServer ? Core.Variables.Instance.ServerConnectionString : Core.Variables.Instance.ConnectionString;
            try
            {
                SqlCommand cmd = new SqlCommand("select pkid,Name,majorversion,minorversion,WorkspaceTypeId,workspacename,machine,VCStatusID,isactive,blob,originalfileuniqueid from graphfile where pkid=" + FilePKID + " and machine='" + MachineName + "'", new SqlConnection(connString));
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter dap = new SqlDataAdapter();
                DataSet ds = new DataSet();
                dap.SelectCommand = cmd;
                dap.Fill(ds, "FileDetails");
                GraphFile graphFile = null;
                if (ds.Tables["FileDetails"].Rows.Count > 0)
                {
                    DataRowView drvFile = ds.Tables["FileDetails"].DefaultView[0];
                    graphFile = new GraphFile();
                    graphFile.pkid = int.Parse(drvFile["pkid"].ToString());
                    graphFile.Name = drvFile["Name"].ToString();
                    graphFile.MajorVersion = int.Parse(drvFile["MajorVersion"].ToString());
                    graphFile.MinorVersion = int.Parse(drvFile["MinorVersion"].ToString());
                    graphFile.Machine = MachineName;
                    graphFile.VCStatusID = int.Parse(drvFile["VCStatusID"].ToString());
                    graphFile.IsActive = bool.Parse(drvFile["IsActive"].ToString());
                    graphFile.WorkspaceTypeId = int.Parse(drvFile["WorkspaceTypeId"].ToString());
                    graphFile.WorkspaceName = drvFile["WorkspaceName"].ToString();
                    graphFile.OriginalFileUniqueID = new Guid(drvFile["originalfileuniqueid"].ToString());
                    try
                    {
                        graphFile.Blob = (byte[])drvFile["blob"];
                    }
                    catch (Exception ex)
                    {
                        Log.WriteLog("Cannot read byte[] for file " + graphFile.pkid + Environment.NewLine + ex.ToString());
                    }
                }
                graphFile.EntityState = EntityState.Unchanged;
                return graphFile;
            }
            catch
            {
                return null;
            }
        }

        public GraphFile GetQuickFileDetails(string OriginalID)
        {
            string connString = Core.Variables.Instance.ConnectionString;
            try
            {
                SqlCommand cmd = new SqlCommand("select pkid,Name,majorversion,minorversion,WorkspaceTypeId,workspacename,machine,VCStatusID,isactive,blob,OriginalFileUniqueID from graphfile where OriginalFileUniqueID='" + OriginalID + "' and IsActive=1", new SqlConnection(connString));
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter dap = new SqlDataAdapter();
                DataSet ds = new DataSet();
                dap.SelectCommand = cmd;
                dap.Fill(ds, "FileDetails");
                GraphFile graphFile = null;
                if (ds.Tables["FileDetails"].Rows.Count > 0)
                {
                    DataRowView drvFile = ds.Tables["FileDetails"].DefaultView[0];
                    graphFile = new GraphFile();
                    graphFile.pkid = int.Parse(drvFile["pkid"].ToString());
                    graphFile.Name = drvFile["Name"].ToString();
                    graphFile.MajorVersion = int.Parse(drvFile["MajorVersion"].ToString());
                    graphFile.MinorVersion = int.Parse(drvFile["MinorVersion"].ToString());
                    graphFile.Machine = drvFile["Machine"].ToString();
                    graphFile.VCStatusID = int.Parse(drvFile["VCStatusID"].ToString());
                    graphFile.IsActive = bool.Parse(drvFile["IsActive"].ToString());
                    graphFile.WorkspaceTypeId = int.Parse(drvFile["WorkspaceTypeId"].ToString());
                    graphFile.WorkspaceName = drvFile["WorkspaceName"].ToString();
                    graphFile.OriginalFileUniqueID = new Guid(drvFile["OriginalFileUniqueID"].ToString());
                    try
                    {
                        graphFile.Blob = (byte[])drvFile["blob"];
                    }
                    catch (Exception ex)
                    {
                        Log.WriteLog("Cannot read byte[] for file " + graphFile.pkid + Environment.NewLine + ex.ToString());
                    }
                    graphFile.EntityState = EntityState.Unchanged;
                }
                return graphFile;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves the most active file using a files origninaluniqueid
        /// </summary>
        /// <param name="guid">The files ID that is carried with it forever</param>
        /// <param name="useServer">Decides which connection string to get the file from</param>
        /// <returns></returns>
        public GraphFile GetFileDetails(string guid, bool useServer)
        {
            string connString = useServer ? Core.Variables.Instance.ServerConnectionString : Core.Variables.Instance.ConnectionString;
            try
            {
                SqlCommand cmd = new SqlCommand("select * from graphfile where OriginalFileUniqueID = '" + guid + "' AND IsActive = 1", new SqlConnection(connString));
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter dap = new SqlDataAdapter();
                DataSet ds = new DataSet();
                dap.SelectCommand = cmd;
                dap.Fill(ds, "FileDetails");
                GraphFile graphFile = null;
                if (ds.Tables["FileDetails"].Rows.Count > 0)
                {
                    DataRowView drvFile = ds.Tables["FileDetails"].DefaultView[0];
                    graphFile = new GraphFile();
                    graphFile.pkid = int.Parse(drvFile["pkid"].ToString());
                    graphFile.Name = drvFile["Name"].ToString();
                    graphFile.MajorVersion = int.Parse(drvFile["MajorVersion"].ToString());
                    graphFile.MinorVersion = int.Parse(drvFile["MinorVersion"].ToString());
                    graphFile.Machine = drvFile["Machine"].ToString(); ;
                    graphFile.VCStatusID = int.Parse(drvFile["VCStatusID"].ToString());
                    graphFile.VCMachineID = drvFile["VCMachineID"].ToString();
                    graphFile.Notes = drvFile["Notes"].ToString();
                    graphFile.IsActive = bool.Parse(drvFile["IsActive"].ToString());
                    graphFile.WorkspaceTypeId = int.Parse(drvFile["WorkspaceTypeId"].ToString());
                    graphFile.WorkspaceName = drvFile["WorkspaceName"].ToString();
                    graphFile.AppVersion = drvFile["AppVersion"].ToString();
                    graphFile.OriginalFileUniqueID = new Guid(drvFile["originalfileuniqueid"].ToString());
                    graphFile.ModifiedDate = DateTime.Parse(drvFile["ModifiedDate"].ToString());
                    try
                    {
                        if (drvFile["PreviousVersionID"] != null)
                            graphFile.PreviousVersionID = int.Parse(drvFile["PreviousVersionID"].ToString());
                    }
                    catch
                    {
                    }
                    try
                    {
                        graphFile.Blob = (byte[])drvFile["blob"];
                    }
                    catch (Exception ex)
                    {
                        Log.WriteLog("Cannot read byte[] for file " + graphFile.pkid + Environment.NewLine + ex.ToString());
                    }
                    graphFile.EntityState = EntityState.Changed;
                    return graphFile;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Changes the name of all files having guid to the newName on the ?useServer
        /// </summary>
        /// <param name="guid">The originalFileUniqueID of the files whose name you want to change</param>
        /// <param name="useServer">Whether to use server or not</param>
        /// <param name="newName">This should be a path including the actual full file name</param>
        public void ChangeNameOfFile(string guid, bool useServer, string newName)
        {
            string connString = useServer ? Core.Variables.Instance.ServerConnectionString : Core.Variables.Instance.ConnectionString;
            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE GraphFile SET FileName = REPLACE('Name','" + newName + "','" + newName + "') WHERE OriginalFileUniqueID = '" + guid + "'", new SqlConnection(connString));
                cmd.CommandType = CommandType.Text;
                using (cmd.Connection)
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog(ex.ToString());
            }
        }
    }
}
