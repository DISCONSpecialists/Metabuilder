using System;
using System.Collections.Generic;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer;
using System.Data;
using System.Data.SqlClient;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
namespace MetaBuilder.BusinessFacade.Storage.RepositoryTemp
{
    public class PermissionService : IPermissionService
    {
        TList<UserPermission> remotePermissions;
        public TList<UserPermission> RemotePermissions
        {
            get { return remotePermissions; }
            set { remotePermissions = value; }
        }
        TList<Workspace> remoteWorkspaces;
        public TList<Workspace> RemoteWorkspaces
        {
            get { return remoteWorkspaces; }
            set { remoteWorkspaces = value; }
        }
        TList<Workspace> localWorkspaces;
        public TList<Workspace> LocalWorkspaces
        {
            get { return localWorkspaces; }
            set { localWorkspaces = value; }
        }
        int localUserID;
        int remoteUserID;

        public bool IsAdmin()
        {
            if (remotePermissions.Count > 0)
                return GetInaccessibleServerWorkspaces().Count == 0;
            else
                return false;
        }

        public bool IsSystemAdmin()
        {
            SqlConnection conn = new SqlConnection(Core.Variables.Instance.ServerConnectionString);
            SqlCommand com = new SqlCommand("Select Count(WorkspaceName) from UserPermission where UserID = " + remoteUserID + " AND PermissionID = 3");
            com.Connection = conn;
            conn.Open();
            int sysAdminPermission = int.Parse(com.ExecuteScalar().ToString());
            conn.Close();
            return sysAdminPermission > 0;
        }

        public bool IsAllSystemAdmin()
        {
            SqlConnection conn = new SqlConnection(Core.Variables.Instance.ServerConnectionString);
            SqlCommand com = new SqlCommand("Select Count(WorkspaceName) from UserPermission where UserID = " + remoteUserID + " AND PermissionID = 3");
            com.Connection = conn;
            conn.Open();
            int sysAdminPermission = int.Parse(com.ExecuteScalar().ToString());
            conn.Close();
            if (sysAdminPermission == DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.WorkspaceProvider.GetByWorkspaceTypeId(3).Count)
                return true;

            return false;
        }

        public WorkspaceKey getWorkspaceKey(Workspace ws)
        {
            WorkspaceKey k = new WorkspaceKey();
            k.Name = ws.Name;
            k.WorkspaceTypeId = ws.WorkspaceTypeId;
            return k;
        }
        public WorkspaceKey getWorkspaceKey(string name, int id)
        {
            WorkspaceKey k = new WorkspaceKey();
            k.Name = name;
            k.WorkspaceTypeId = id;
            return k;
        }

        public bool HasAtLeastThisPermission(WorkspaceKey wsKey, PermissionList permissionRequested)
        {
            PermissionList permissionFound = this.GetServerPermission(wsKey.Name, wsKey.WorkspaceTypeId);
            if (permissionFound == PermissionList.Delete && ((permissionRequested == PermissionList.Delete) || (permissionRequested == PermissionList.Write) || (permissionRequested == PermissionList.Read)))
            {
                return true;
            }

            if (permissionFound == PermissionList.Write && ((permissionRequested == PermissionList.Write) || (permissionRequested == PermissionList.Read)))
            {
                return true;
            }

            if (permissionFound == PermissionList.Read && ((permissionRequested == PermissionList.Read)))
            {
                return true;
            }
            return false;

        }
        public PermissionService()
        {
            SetupRemoteWorkspaces();
            SetupLocalWorkspaces();
            ReadServerConfig();
        }
        private void SetupLocalWorkspaces()
        {
            localWorkspaces = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.WorkspaceProvider.GetAll();
        }
        private void SetupRemoteWorkspaces()
        {
            try
            {
                localUserID = Variables.Instance.UserID;
                TList<User> remoteUserList = DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.UserProvider.GetAll();
                bool foundUser = false;
                string localuserscomparison = Variables.Instance.UserDomainIdentifier + ":";
                foreach (User user in remoteUserList)
                {
                    string localUser = Variables.Instance.UserDomainIdentifier;
                    localuserscomparison += user.ToString() + "|";
                    if (user.Name.ToLower().Trim() == localUser.ToLower().Trim())
                    {
                        remoteUserID = user.pkid;
                        foundUser = true;
                        localuserscomparison += ";;";
                        break;
                    }
                }
                if (!foundUser)
                {
                    Log.WriteLog("PermissionServer::SetupRemoteWorkspaces::(" + Variables.Instance.UserDomainIdentifier + ") cannot be found in remoteUserList" + Environment.NewLine + localuserscomparison);
                }
                buildRemotePermissions();

                // retrieve server workspaces
                remoteWorkspaces = DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.WorkspaceProvider.GetByWorkspaceTypeId(3);
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString());
            }
        }

        private void buildRemotePermissions()
        {
            //We must use a SQL Query here becase nettiers is not in sync with the update to UserPermissions table that takes place in databaseversioning.cs which makes the entire tables columns the PK to allow multipl permissions
            remotePermissions = new TList<UserPermission>();
            SqlConnection conn = new SqlConnection(Core.Variables.Instance.ServerConnectionString);
            SqlCommand com = new SqlCommand("Select * from UserPermission where UserID = " + remoteUserID);
            com.Connection = conn;
            SqlDataAdapter dap = new SqlDataAdapter();
            dap.SelectCommand = com;
            DataSet ds = new DataSet();
            dap.Fill(ds);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                UserPermission perm = new UserPermission();
                perm.UserID = int.Parse(row[0].ToString());
                perm.PermissionID = int.Parse(row[1].ToString());
                perm.WorkspaceName = row[2].ToString();
                perm.WorkspaceTypeId = int.Parse(row[3].ToString());

                remotePermissions.Add(perm);
            }
        }

        public TList<Workspace> GetAccessibleServerWorkspaces()
        {
            TList<Workspace> accessibleWorkspaces = new TList<Workspace>();
            foreach (Workspace remoteWorkspace in remoteWorkspaces)
            {
                remotePermissions.Filter = "UserID = " + remoteUserID.ToString() + " and WorkspaceName = '" + remoteWorkspace.Name + "'";
                if (remotePermissions.Count > 0)
                {
                    accessibleWorkspaces.Add(remoteWorkspace);
                }
            }
            remotePermissions.RemoveFilter();
            return accessibleWorkspaces;
        }
        public TList<Workspace> GetInaccessibleServerWorkspaces()
        {
            TList<Workspace> inaccessibleWorkspaces = new TList<Workspace>();
            foreach (Workspace remoteWorkspace in remoteWorkspaces)
            {
                remotePermissions.Filter = "UserID = " + remoteUserID.ToString() + " and WorkspaceName = '" + remoteWorkspace.Name + "'";
                if (remotePermissions.Count == 0)
                {
                    inaccessibleWorkspaces.Add(remoteWorkspace);
                }
            }
            remotePermissions.RemoveFilter();
            return inaccessibleWorkspaces;
        }

        public void SynchroniseServerWorkspaces()
        {
            TList<Workspace> workspacesToAdd = new TList<Workspace>();
            foreach (Workspace accessibleWorkspace in GetAccessibleServerWorkspaces())
            {
                localWorkspaces.Filter = "Name = '" + accessibleWorkspace.Name + "' and WorkspaceTypeId = " + accessibleWorkspace.WorkspaceTypeId.ToString();
                if (localWorkspaces.Count == 0)
                {
                    // Console.WriteLine("Adding Server Workspace To Local... " + accessibleWorkspace.Name);
                    //if (DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.WorkspaceProvider.GetByNameWorkspaceTypeId(accessibleWorkspace.Name, accessibleWorkspace.WorkspaceTypeId) == null)
                    try
                    {
                        DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.WorkspaceProvider.Insert(accessibleWorkspace);
                    }
                    catch
                    {
                        Log.WriteLog("You have a workspace on your client (" + accessibleWorkspace.Name + ") that matches a workspace on the server however the casing of either does not match and you will therefore not have permission to synchronise over this workspace");
                    }
                }
                if (!(Core.Variables.Instance.WorkspaceHashtable.Contains(accessibleWorkspace.Name + "#" + accessibleWorkspace.WorkspaceTypeId.ToString())))
                    Core.Variables.Instance.WorkspaceHashtable.Add(accessibleWorkspace.Name + "#" + accessibleWorkspace.WorkspaceTypeId.ToString(), null);
                localWorkspaces.RemoveFilter();
            }
        }

        public PermissionList GetServerPermission(string WorkspaceName, int WorkspaceTypeId)
        {
            remotePermissions.Filter = "WorkspaceName = '" + WorkspaceName + "' and WorkspaceTypeId = " + WorkspaceTypeId.ToString();
            if (remotePermissions.Count > 0)
            {
                //return highest permission --> See point in buildRemotePermissions()
                int PermIndex = 0;
                int returnPermIndex = 0;
                int permID = 0;
                foreach (BusinessLogic.UserPermission perm in remotePermissions)
                {
                    if (perm.PermissionID != 3) //skip admin
                        if (perm.PermissionID > permID)
                        {
                            permID = perm.PermissionID;
                            returnPermIndex = PermIndex;
                        }

                    PermIndex++;
                }
                PermissionList permissionlist = (PermissionList)remotePermissions[returnPermIndex].PermissionID;
                remotePermissions.RemoveFilter();
                return permissionlist;
            }
            remotePermissions.RemoveFilter();
            return PermissionList.None;
        }

        public Workspace GetWorkspaceOnClient(WorkspaceKey key)
        {
            foreach (Workspace ws in localWorkspaces)
            {
                if (ws.Name == key.Name && ws.WorkspaceTypeId == key.WorkspaceTypeId)
                {
                    return ws;
                }
            }
            Workspace newWorkspace = new Workspace();
            newWorkspace.Name = key.Name;
            newWorkspace.WorkspaceTypeId = key.WorkspaceTypeId;
            DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.WorkspaceProvider.Insert(newWorkspace);
            SetupLocalWorkspaces();
            return GetWorkspaceOnClient(key);
        }

        public Workspace GetWorkspaceOnServer(WorkspaceKey wspaceKey)
        {
            foreach (Workspace ws in this.remoteWorkspaces)
            {

                if (ws.Name == wspaceKey.Name && ws.WorkspaceTypeId == wspaceKey.WorkspaceTypeId)
                {
                    // query this.GetAccessibleServerWorkspaces() to find out if user has access
                    TList<Workspace> accessibles = GetAccessibleServerWorkspaces();
                    if (accessibles.Contains(ws))
                        return ws;
                    return null;
                }
            }
            AddRequestedClientWorkspace(wspaceKey);
            return GetWorkspaceOnServer(wspaceKey);
        }

        private void ChangeWorkspaceType(Workspace workspace, WorkspaceTypeList newType, Repository repository)
        {
            // TODO
        }

        private void AddRequestedClientWorkspace(WorkspaceKey requestedWorkspaceKey)
        {
            Workspace requestedWorkspace = new Workspace();
            requestedWorkspace.Name = requestedWorkspaceKey.Name;
            requestedWorkspace.WorkspaceTypeId = requestedWorkspaceKey.WorkspaceTypeId;

            requestedWorkspace.RequestedByUser = Variables.Instance.UserDomainIdentifier;
            requestedWorkspace.WorkspaceTypeId = (int)WorkspaceTypeList.Server;
            requestedWorkspace.IsActive = false;
            // Console.WriteLine("Adding requested workspace:" + requestedWorkspaceKey.Name);
            DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.WorkspaceProvider.Insert(requestedWorkspace);

            // TODO: add Write permission by default to new workspace

            UserPermission defaultPermission = new UserPermission();
            defaultPermission.PermissionID = (int)PermissionList.Write;
            defaultPermission.UserID = remoteUserID;
            defaultPermission.WorkspaceName = requestedWorkspaceKey.Name;
            defaultPermission.WorkspaceTypeId = requestedWorkspaceKey.WorkspaceTypeId;
            DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.UserPermissionProvider.Insert(defaultPermission);
            SetupRemoteWorkspaces();
        }
        private void ReadServerConfig()
        {
            serverConfig = new Dictionary<string, string>();
            TList<Config> configurations = DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.ConfigProvider.GetAll();
            foreach (Config cfg in configurations)
            {
                serverConfig.Add(cfg.ConfigName, cfg.ConfigValue);
            }
        }

        private bool GetConfigBool(string configName)
        {
            if (serverConfig.ContainsKey(configName))
            {
                return bool.Parse(serverConfig[configName]);
            }
            else
            {
                return false;
            }
        }
        private Dictionary<string, string> serverConfig;
        public bool SERVERCONFIG__AdminMustApproveCheckins
        {
            get
            {
                return GetConfigBool("AdminMustApproveCheckins");
            }
        }

        public bool SERVERCONFIG__CannotCheckInFromSandbox
        {
            get
            {
                return GetConfigBool("CannotCheckInFromSandbox");
            }
        }

        public bool SERVERCONFIG__AutoCreateWorkspaces
        {
            get
            {
                return GetConfigBool("AutoCreateWorkspaces");
            }
        }

        public bool SERVERCONFIG__CheckForObsoleteObjectsOnCheckOut
        {
            get
            {
                return GetConfigBool("CheckForObsoleteObjectsOnCheckOut");
            }
        }

        public bool SERVERCONFIG_ObsoletesCanBeReinstated
        {
            get
            {
                return GetConfigBool("ObsoletesCanBeReinstated");
            }
        }

        /*public b.TList<b.Workspace> VerifyWorkspaces(b.TList<b.Workspace> workspaces)
        {
            b.TList<b.Workspace> workspacesThatDontExist = new MetaBuilder.BusinessLogic.TList<MetaBuilder.BusinessLogic.Workspace>();
            b.TList<b.Workspace> existing = d.DataRepository.Connections[reposType.ToString()].Provider.WorkspaceProvider.GetAll();

            // get the remote userid for this client user
            int RemoteUserID = 0;
            bool isClient = false;
            isClient = (reposType == RepositoryType.Client);

            b.TList<b.User> allRemoteUsers = d.DataRepository.Connections[reposType.ToString()].Provider.UserProvider.GetAll();
            allRemoteUsers.Filter = "Name = '" + Environment.UserDomainName + "\\" + Environment.UserName + "'";
            if (allRemoteUsers.Count > 0)
            {
                RemoteUserID = allRemoteUsers[0].Pkid;
            }

            b.TList<b.UserPermission> remotepermissions = d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.UserPermissionProvider.GetByUserID(RemoteUserID);

            if (RemoteUserID > 0 || isClient)
            {
                foreach (b.Workspace target in workspaces)
                {
                    existing.Filter = "Name = '" + target.Name + "' and WorkspaceTypeId = " + target.WorkspaceTypeId.ToString();
                    if (existing.Count == 0)
                    {
                        // filter for read or write or admin privileges (the user needs at least one of these to access data)
                        // get the userid on the remote machine
                        remotepermissions.Filter = "WorkspaceName = '" + target.Name + "' and WorkspaceTypeId = " + target.WorkspaceTypeId.ToString();
                        if (remotepermissions.Count > 0)
                        {
                            workspacesThatDontExist.Add(target);
                        }
                    }
                }
            }
            return workspacesThatDontExist;
        }*/
    }
}
