using System.Data;
using System.Data.SqlClient;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.Meta;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using d = MetaBuilder.DataAccessLayer;
using b = MetaBuilder.BusinessLogic;

namespace MetaBuilder.BusinessFacade.Storage
{
    public class WorkspaceHelper
    {
        public static void Transfer(Workspace from, Workspace to, string provider)
        {
            Reset();
            MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter tmpAdapter = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();
            foreach (GraphFile f in tmpAdapter.GetFilesByWorkspaceTypeIdWorkspaceName(from.WorkspaceTypeId, from.Name, (int)FileTypeList.Diagram, (provider == Core.Variables.Instance.ServerProvider)))//d.DataRepository.Connections[provider].Provider.GraphFileProvider.GetByMetaObjectIDMachineIDFromGraphFileObject(mObj.pkid, mObj.Machine))
            //foreach (GraphFile f in d.DataRepository.Connections[provider].Provider.GraphFileProvider.GetByWorkspaceNameWorkspaceTypeId(from.Name, from.WorkspaceTypeId))
            {
                ChangeWorkspaceForGraphFile(f, to.Name, to.WorkspaceTypeId, provider);
            }
            foreach (MetaObject o in d.DataRepository.Connections[provider].Provider.MetaObjectProvider.GetByWorkspaceNameWorkspaceTypeId(from.Name, from.WorkspaceTypeId))
            {
                ChangeWorkspaceForMetaObject(o.pkid, o.Machine, to.Name, to.WorkspaceTypeId, provider);
            }
        }

        public static void ChangeWorkspaceForGraphFile(GraphFile file, string name, int WorkspaceTypeId, string provider)
        {
            //file.Notes = "Workspace Transferred";
            //file.WorkspaceName = name;
            //file.WorkspaceTypeId = WorkspaceTypeId;
            //if (provider == Core.Variables.Instance.ServerProvider)
            //{
            //file.VCStatusID = 1;
            //file.VCMachineID = strings.GetVCIdentifier();
            //}
            DataAccessLayer.DataRepository.Connections[provider].Provider.GraphFileProvider.Save(file);

            string connString = (provider == Core.Variables.Instance.ServerProvider) ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString;
            string addedSETTINGS = ", Notes='Workspace Transferred'";
            if (provider == Core.Variables.Instance.ServerProvider)
                addedSETTINGS += ", VCStatusID=1, VCmachineID='" + strings.GetVCIdentifier() + "'"; //always check in server after transfer with current vcmachine
            SqlCommand cmd = new SqlCommand("update graphfile set workspacename='" + name + "', WorkspaceTypeId=" + WorkspaceTypeId.ToString() + addedSETTINGS + " where pkid = " + file.pkid + " and Machine = '" + file.Machine + "'", new SqlConnection(connString));
            cmd.CommandType = CommandType.Text;
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }
        public static TList<GraphFile> affectedServerFiles = new TList<GraphFile>();
        public static TList<MetaObject> affectedServerObjects = new TList<MetaObject>();
        public static void ChangeWorkspaceForMetaObject(int pkid, string machine, string name, int WorkspaceTypeId, string provider)
        {
            // Update Cache if necessary
            string connString = (provider == Core.Variables.Instance.ServerProvider) ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString;
            string addedSETTINGS = "";
            if (provider == Core.Variables.Instance.ServerProvider)
                addedSETTINGS = ", VCmachineID='" + strings.GetVCIdentifier() + "'"; //always update user to current one
            CacheManager cacheMan = CacheFactory.GetCacheManager();
            string key = pkid.ToString() + "|" + machine;
            if (cacheMan.Contains(key))
            {
                MetaBase mbase = (MetaBase)cacheMan.GetData(key);
                mbase.WorkspaceName = name;
                mbase.WorkspaceTypeId = WorkspaceTypeId;
            }

            SqlCommand cmd = new SqlCommand("update metaobject set workspacename='" + name + "', WorkspaceTypeId=" + WorkspaceTypeId.ToString() + addedSETTINGS + " where pkid = " + pkid.ToString() + " and Machine = '" + machine + "' ", new SqlConnection(connString));
            cmd.CommandType = CommandType.Text;
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

            //get this object (complex object transfer)
            MetaObject mObj = d.DataRepository.Connections[provider].Provider.MetaObjectProvider.GetBypkidMachine(pkid, machine);
            mObj.WorkspaceName = name;
            mObj.WorkspaceTypeId = WorkspaceTypeId;
            d.DataRepository.Connections[provider].Provider.MetaObjectProvider.Save(mObj);

            //when we act on the server build a list of files that this object exists on
            if (provider == Core.Variables.Instance.ServerProvider)
            {
                if (!(affectedServerObjects.Contains(mObj)))
                    affectedServerObjects.Add(mObj);
                //add the files this object is on
                MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter tmpAdapter = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();
                foreach (GraphFile file in tmpAdapter.GetFilesByObjectId(mObj.pkid, mObj.Machine, (provider == Core.Variables.Instance.ServerProvider)))//d.DataRepository.Connections[provider].Provider.GraphFileProvider.GetByMetaObjectIDMachineIDFromGraphFileObject(mObj.pkid, mObj.Machine))
                {
                    if (file.IsActive)
                        if (!(affectedServerFilesContains(file)))
                            affectedServerFiles.Add(file);
                }
            }

            if (mObj.Class == "DataEntity" || mObj.Class == "Entity" || mObj.Class == "DataTable" || mObj.Class == "DataView" || mObj.Class.ToLower().Contains("informationartefact"))
            {
                ChangeWorkspaceForChildObjects(mObj, name, WorkspaceTypeId, provider);
            }
        }
        private static bool affectedServerFilesContains(GraphFile file)
        {
            foreach (GraphFile f in affectedServerFiles)
            {
                if (f.OriginalFileUniqueID == file.OriginalFileUniqueID && f.pkid == file.pkid && f.Machine == file.Machine)
                    return true;
            }
            return false;
        }
        private static void ChangeWorkspaceForChildObjects(MetaObject obj, string name, int WorkspaceTypeId, string provider)
        {
            //get all children
            b.TList<ObjectAssociation> childObjectAssociations = d.DataRepository.Connections[provider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(obj.pkid, obj.Machine);
            childObjectAssociations.Filter = "";
            foreach (ObjectAssociation o in childObjectAssociations)
            {
                //get obj, if it is column/attribute, change
                MetaObject mObj = d.DataRepository.Connections[provider].Provider.MetaObjectProvider.GetBypkidMachine(o.ChildObjectID, o.ChildObjectMachine);
                if (mObj.Class == "DataColumn" || mObj.Class == "Attribute" || mObj.Class == "DataField" || mObj.Class == "DataAttribute")
                {
                    ChangeWorkspaceForMetaObject(mObj.pkid, mObj.Machine, name, WorkspaceTypeId, provider);
                }
            }
            //get all backwards children
            b.TList<ObjectAssociation> backwardsObjectAssociations = d.DataRepository.Connections[provider].Provider.ObjectAssociationProvider.GetByChildObjectIDChildObjectMachine(obj.pkid, obj.Machine);
            foreach (ObjectAssociation o in backwardsObjectAssociations)
            {
                MetaObject mObj = d.DataRepository.Connections[provider].Provider.MetaObjectProvider.GetBypkidMachine(o.ChildObjectID, o.ChildObjectMachine);
                if (mObj.Class == "DataColumn" || mObj.Class == "Attribute" || mObj.Class == "DataField" || mObj.Class == "DataAttribute")
                {
                    ChangeWorkspaceForMetaObject(mObj.pkid, mObj.Machine, name, WorkspaceTypeId, provider);
                }
            }
        }

        public static void DeleteGraphFile(int pkid, string machine, bool useServer)
        {
            string connString = (useServer) ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString;
            SqlCommand cmd = new SqlCommand("delete from graphfileobject where GraphFileID=" + pkid.ToString() + " and GraphFileMachine='" + machine + "'", new SqlConnection(connString));
            cmd.CommandType = CommandType.Text;
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.ExecuteNonQuery();
            cmd.CommandText = "delete from graphfileassociation where GraphFileID=" + pkid.ToString() + " and GraphFileMachine='" + machine + "'";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "delete from graphfile where pkid=" + pkid.ToString() + " and machine='" + machine + "'";
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        public static void MarkLatestDrawingsAsActive()
        {
            SqlCommand cmd = new SqlCommand("HACK_MarkDrawingsActiveFromReportingTool", new SqlConnection(Variables.Instance.ConnectionString));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 120;
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        public static void Reset()
        {
            affectedServerFiles = new TList<GraphFile>();
            affectedServerObjects = new TList<MetaObject>();
        }
    }
}