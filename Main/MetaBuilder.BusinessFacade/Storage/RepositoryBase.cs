using System;
using System.Collections.Generic;
using System.Text;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
using System.Data.SqlClient;
using System.Data;
namespace MetaBuilder.BusinessFacade.Storage
{
    public abstract class RepositoryBase : IRepositoryService
    {

        private Dictionary<string, b.IRepositoryItem> repositoryDictionary;
        public Dictionary<string, b.IRepositoryItem> RepositoryDictionary
        {
            get { return repositoryDictionary; }
            set { repositoryDictionary = value; }
        }

        private bool isAdmin;
        public bool IsAdmin
        {
            get { return isAdmin; }
        }

        public RepositoryBase(RepositoryType rtype)
        {
            repositoryType = rtype;
            isAdmin = false;
            repositoryDictionary = new Dictionary<string, b.IRepositoryItem>();

        }

        public RepositoryBase(RepositoryType rtype, bool Admin)
        {
            repositoryType = rtype;
            isAdmin = Admin;
            repositoryDictionary = new Dictionary<string, b.IRepositoryItem>();
        }

        private RepositoryType repositoryType;
        public RepositoryType RepositoryType
        {
            get { return repositoryType; }
            set { repositoryType = value; }
        }

        public virtual CheckOutResult GetItem(b.VCStatusList targetState, ref RepositoryActionSpecification spec , bool force)
        {
            b.IRepositoryItem item = spec.Item;
            return CheckOutResult.Fail;
        }

        public virtual CheckInResult PutItem(b.VCStatusList targetState, RepositoryActionSpecification spec)
        {
            b.IRepositoryItem item = spec.Item;
            if (!repositoryDictionary.ContainsKey(item.Pkid.ToString() + item.MachineName))
            {
                repositoryDictionary.Add(item.Pkid.ToString() + item.MachineName, item);
            }
            return CheckInResult.Fail;
        }

        public virtual List<b.VCStatusList> GetAllowedStates(b.IRepositoryItem item)
        {
            return null;
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        internal string ConnectionString;

        public b.VCStatusList GetState(b.IRepositoryItem item)
        {
            SqlCommand cmd = new SqlCommand("REPLACEME", new SqlConnection(ConnectionString));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@machine", SqlDbType.VarChar, 100));
            cmd.Parameters.Add(new SqlParameter("@VCStatusID", SqlDbType.Int));
            cmd.Parameters["@VCStatusID"].Value = 0;
            cmd.Parameters["@VCStatusID"].Direction = ParameterDirection.InputOutput;
                
            if (item is Meta.MetaBase)
            {
                cmd.CommandText = "Meta_GetStatusForMetaObject";
                Meta.MetaBase mbase = item as Meta.MetaBase;
                cmd.Parameters.Add(new SqlParameter("@pkid", SqlDbType.Int));
                cmd.Parameters["@pkid"].Value = mbase.pkid;
                cmd.Parameters["@machine"].Value = mbase.machineName;
            }

            if (item is b.ObjectAssociation)
            {
                b.ObjectAssociation oassoc = item as b.ObjectAssociation;
                
                cmd.CommandText = "Meta_GetStatusForAssociation";
                Meta.MetaBase mbase = item as Meta.MetaBase;
                cmd.Parameters.Add(new SqlParameter("@caid", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@objectid", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@childobjectid", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@objectmachine", SqlDbType.VarChar,50));
                cmd.Parameters.Add(new SqlParameter("@childobjectmachine", SqlDbType.VarChar, 50));
                cmd.Parameters["@caid"].Value = oassoc.CAid;
                cmd.Parameters["@objectid"].Value = oassoc.ObjectID;
                cmd.Parameters["@childobjectid"].Value = oassoc.ChildObjectID;
                cmd.Parameters["@objectmachine"].Value = oassoc.ObjectMachine;
                cmd.Parameters["@childobjectmachine"].Value = oassoc.ChildObjectMachine;
                cmd.Parameters["@machine"].Value = oassoc.MachineName;
            }

            if (item is b.GraphFile)
            {
                b.GraphFile file = item as b.GraphFile;
                cmd.CommandText = "Meta_GetStatusForGraphFile";
                cmd.Parameters.Add(new SqlParameter("@pkid", SqlDbType.Int));
                cmd.Parameters["@pkid"].Value = file.Pkid;
                cmd.Parameters["@machine"].Value = file.MachineName;
            }

            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();

            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

            int vcStatusID = int.Parse(cmd.Parameters["@VCStatusID"].Value.ToString());
            return (MetaBuilder.BusinessLogic.VCStatusList)Enum.Parse(typeof(b.VCStatusList), vcStatusID.ToString());
        }
        internal void SaveToDatabase(List<RepositoryActionSpecification> list)
        {
            foreach (b.IRepositoryItem item in list)
            {
                if (item is Meta.MetaBase)
                {
                    Meta.MetaBase mbase = item as Meta.MetaBase;
                    b.MetaObject mo = d.DataRepository.MetaObjectProvider.GetByPkidMachine(mbase.pkid, mbase.MachineName);
                    if (mo == null)
                    {
                        
                    }

                }

                if (item is b.GraphFile)
                {
                }

                if (item is b.ObjectAssociation)
                {
                }
            }
        }
        internal void SaveState(RepositoryActionSpecification spec, b.VCStatusList targetState)
        {
            b.IRepositoryItem item = spec.Item;
            bool IsServer = GetType() == typeof(ServerRepository);
            string provider = (IsServer) ? "Remote" : "Local";
            SqlCommand cmd = new SqlCommand("REPLACEME", new SqlConnection(ConnectionString));
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@machine", SqlDbType.VarChar, 100));
            cmd.Parameters.Add(new SqlParameter("@VCStatusID", SqlDbType.Int));
            cmd.Parameters["@VCStatusID"].Value = (int)targetState;
            cmd.Parameters.Add(new SqlParameter("@VCMachine", SqlDbType.VarChar, 50));
            cmd.Parameters["@VCMachine"].Value = Environment.MachineName;

            if (item is Meta.MetaBase)
            {
                SaveMetaObject(spec, item, provider, cmd);
            }

            if (item is b.ObjectAssociation)
            {
                SaveObjectAssociation(item, cmd);
            }

            if (item is b.GraphFile)
            {
                SaveGraphFile(spec, item, IsServer, provider, cmd);
            }

            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();

            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        private void SaveMetaObject(RepositoryActionSpecification spec, b.IRepositoryItem item, string provider, SqlCommand cmd)
        {
            cmd.CommandText = "Meta_SetStatusForMetaObject";
            Meta.MetaBase mbase = item as Meta.MetaBase;
            cmd.Parameters.Add(new SqlParameter("@pkid", SqlDbType.Int));
            cmd.Parameters["@pkid"].Value = mbase.pkid;
            cmd.Parameters["@machine"].Value = mbase.machineName;

            string oldConnectionstring = Core.Variables.Instance.ConnectionString;
            Core.Variables.Instance.ConnectionString = ConnectionString;
            int oldWorkspaceTypeID = Core.Variables.Instance.CurrentWorkspaceTypeID;
            string oldWorkspaceName = Core.Variables.Instance.CurrentWorkspaceName;

            Core.Variables.Instance.CurrentWorkspaceTypeID = spec.WorkspaceTypeID;
            Core.Variables.Instance.CurrentWorkspaceName = spec.WorkspaceName;
            mbase.Save(Guid.NewGuid());

            // if this object is checked out/in as part of a document, also add a graphfileobject row if it doesnt exist (this will only happen the first time)
            if (spec.ParentSpecFileID > 0) 
            {
                b.GraphFile gfile = d.DataRepository.Connections[provider].Provider.GraphFileProvider.GetByPkidMachine(spec.ParentSpecFileID, spec.ParentSpecMachine);
                b.GraphFileObject gfo = new MetaBuilder.BusinessLogic.GraphFileObject();
                gfo.MetaObjectID = mbase.pkid;
                gfo.MachineID = mbase.MachineName;
                gfo.GraphFileID = gfile.Pkid;
                gfo.GraphFileMachine = gfile.Machine;
                b.GraphFileObjectKey key = GetGraphFileObjectRowKey(mbase, gfile);
                if (d.DataRepository.Connections[provider].Provider.GraphFileObjectProvider.Get(key) == null)
                {
                    d.DataRepository.Connections[provider].Provider.GraphFileObjectProvider.Save(gfo);
                }
            }
            Core.Variables.Instance.CurrentWorkspaceName  = oldWorkspaceName;
            Core.Variables.Instance.CurrentWorkspaceTypeID = oldWorkspaceTypeID;
            Core.Variables.Instance.ConnectionString = oldConnectionstring;
        }


        /// <summary>
        /// Gets the key for the graphfileobject (for lookups)
        /// </summary>
        /// <param name="mbase">The mbase.</param>
        /// <param name="gfile">The gfile.</param>
        /// <returns></returns>
        private static b.GraphFileObjectKey GetGraphFileObjectRowKey(Meta.MetaBase mbase, b.GraphFile gfile)
        {
            b.GraphFileObjectKey key = new MetaBuilder.BusinessLogic.GraphFileObjectKey();
            key.GraphFileID = gfile.Pkid;
            key.GraphFileMachine = gfile.Machine;
            key.MachineID = mbase.machineName;
            key.MetaObjectID = mbase.pkid;
            return key;
        }

        private void SaveGraphFile(RepositoryActionSpecification spec, b.IRepositoryItem item, bool IsServer, string provider, SqlCommand cmd)
        {
            b.GraphFile file = item as b.GraphFile;

            b.GraphFile existingFile = d.DataRepository.Connections[provider].Provider.GraphFileProvider.GetByPkidMachine(file.Pkid, file.Machine);
            if (existingFile == null)
            {
                file = RetrieveAndInsertFile(IsServer, file, spec.WorkspaceTypeID,spec.WorkspaceName);
            }
            cmd.CommandText = "Meta_SetStatusForGraphFile";
            cmd.Parameters.Add(new SqlParameter("@pkid", SqlDbType.Int));
            cmd.Parameters["@pkid"].Value = file.Pkid;
            cmd.Parameters["@machine"].Value = file.MachineName;
        }

        private static void SaveObjectAssociation(b.IRepositoryItem item, SqlCommand cmd)
        {
            b.ObjectAssociation oassoc = item as b.ObjectAssociation;

            cmd.CommandText = "Meta_SetStatusForAssociation";
            Meta.MetaBase mbase = item as Meta.MetaBase;
            cmd.Parameters.Add(new SqlParameter("@caid", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@objectid", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@childobjectid", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@objectmachine", SqlDbType.VarChar, 50));
            cmd.Parameters.Add(new SqlParameter("@childobjectmachine", SqlDbType.VarChar, 50));
            cmd.Parameters["@caid"].Value = oassoc.CAid;
            cmd.Parameters["@objectid"].Value = oassoc.ObjectID;
            cmd.Parameters["@childobjectid"].Value = oassoc.ChildObjectID;
            cmd.Parameters["@objectmachine"].Value = oassoc.ObjectMachine;
            cmd.Parameters["@childobjectmachine"].Value = oassoc.ChildObjectMachine;
            cmd.Parameters["@machine"].Value = oassoc.MachineName;
        }

        private b.GraphFile RetrieveAndInsertFile(bool IsServer, b.GraphFile file, int WorkspaceTypeID, string WorkspaceName)
        {
            // load the file completely
            if (IsServer)
            {
                file = d.DataRepository.Connections["Local"].Provider.GraphFileProvider.GetByPkidMachine(file.Pkid, file.Machine);
            }
            
            //d.DataRepository.Connections[provider].Provider.GraphFileProvider.Insert(file);
            d.OldCode.Diagramming.TempFileGraphAdapter tfga = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();
            tfga.InsertFile(file,WorkspaceTypeID ,WorkspaceName,ConnectionString);
            return file;
        }

        
    }

    public enum RepositoryType
    {
        Client, Server
    }

}
