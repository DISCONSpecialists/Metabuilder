using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.DataAccessLayer.OldCode.Diagramming
{
    public class TempFileGraphAdapter
    {
        public TList<GraphFile> GetFilesByWorkspaceTypeIdWorkspaceName(int WorkspaceTypeId, string WorkspaceName, int FileTypeID, bool useServer)
        {
            string connString = (useServer) ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString;
            SqlCommand cmd = new SqlCommand("sp_Diagramming_Files_GetListByWorkspaceIDandFileTypeID", new SqlConnection(connString));

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@WorkspaceTypeId", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@WorkspaceName", SqlDbType.VarChar, 100));
            cmd.Parameters.Add(new SqlParameter("@FileTypeID", SqlDbType.Int));
            cmd.Parameters["@WorkspaceTypeId"].Value = WorkspaceTypeId;
            cmd.Parameters["@WorkspaceName"].Value = WorkspaceName;
            cmd.Parameters["@FileTypeID"].Value = FileTypeID;
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
                graphFile.WorkspaceName = WorkspaceName;
                graphFile.WorkspaceTypeId = WorkspaceTypeId;
                graphFile.IsActive = true;// bool.Parse(drvFile["IsActive"].ToString());
                graphFile.OriginalFileUniqueID = new Guid(drvFile["OriginalFileUniqueID"].ToString());

                graphFile.VCMachineID = drvFile["VCMachineID"].ToString();
                fileList.Add(graphFile);
            }
            return fileList;
        }
        public TList<GraphFile> GetFilesByObjectId(int ObjectId, string machine, bool useServer)
        {
            string connString = (useServer) ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString;
            SqlCommand cmd = new SqlCommand("select pkid,Name,graphfilemachine from graphfileobject inner join graphfile on graphfileobject.graphfileid = graphfile.pkid where graphfile.isactive=1 and graphfileobject.machineid='" + machine + "' and graphfileobject.metaObjectId=" + ObjectId.ToString(), new SqlConnection(connString));

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
                graphFile.MachineName = drvFile["graphfilemachine"].ToString();
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
                fileList.Add(graphFile);
            }
            return fileList;
        }
        public TList<GraphFile> GetAllFilesByTypeID(int FileTypeID, bool useServer)
        {
            string connString = (useServer) ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString;
            SqlCommand cmd = new SqlCommand("select pkid,Name,MajorVersion,MinorVersion,ModifiedDate,VCStatusID,IsActive,Machine,WorkspaceName,WorkspaceTypeId,OriginalFileUniqueID,VCMachineID from graphfile where FileTypeID=" + FileTypeID.ToString(), new SqlConnection(connString));

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
                graphFile.Machine = drvFile["Machine"].ToString();
                graphFile.WorkspaceName = drvFile["WorkspaceName"].ToString();
                graphFile.WorkspaceTypeId = int.Parse(drvFile["WorkspaceTypeId"].ToString());
                graphFile.IsActive = bool.Parse(drvFile["IsActive"].ToString());
                graphFile.OriginalFileUniqueID = new Guid(drvFile["OriginalFileUniqueID"].ToString());
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

            newFile.ModifiedDate = file.ModifiedDate;
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
            sbQuery.Append("UPDATE GRAPHFILE SET ISACTIVE=0  WHERE ISACTIVE=1 AND   (OriginalFileUniqueID = '" + file.OriginalFileUniqueID.ToString() + "')");

            DataRepository.Connections[providername].Provider.ExecuteNonQuery(CommandType.Text, sbQuery.ToString());
        }
        public GraphFile GetQuickFileDetails(int WorkspaceTypeId, int WorkspaceName, int FilePKID, string MachineName, bool useServer)
        {
            string connString = useServer ? Core.Variables.Instance.ServerConnectionString : Core.Variables.Instance.ConnectionString;
            SqlCommand cmd = new SqlCommand("select pkid,Name,majorversion,minorversion,WorkspaceTypeId,isactive,workspacename,machine from graphfile where pkid=" + FilePKID.ToString() + " and Machine='" + MachineName + "'", new SqlConnection(connString));
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter dap = new SqlDataAdapter();
            DataSet ds = new DataSet();
            dap.SelectCommand = cmd;
            dap.Fill(ds, "FileDetails");
            DataRowView drvFile = ds.Tables["FileDetails"].DefaultView[0];
            GraphFile graphFile = new GraphFile();
            graphFile.pkid = int.Parse(drvFile["pkid"].ToString());
            graphFile.Name = drvFile["Name"].ToString();
            graphFile.MajorVersion = int.Parse(drvFile["MajorVersion"].ToString());
            graphFile.MinorVersion = int.Parse(drvFile["MinorVersion"].ToString());
            graphFile.Machine = MachineName;
            graphFile.IsActive = bool.Parse(drvFile["IsActive"].ToString());
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
                SqlCommand cmd = new SqlCommand("select pkid,Name,majorversion,minorversion,WorkspaceTypeId,workspacename,machine,VCStatusID,isactive from graphfile where pkid=" + FilePKID + " and machine='" + MachineName + "'", new SqlConnection(connString));
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
                }
                return graphFile;
            }
            catch { return null; }
        }
    }
}
