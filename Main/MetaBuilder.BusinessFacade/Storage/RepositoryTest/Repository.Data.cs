using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.DataAccessLayer.OldCode.Diagramming;
using MetaBuilder.Meta;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
using ObjectAssociation = MetaBuilder.BusinessLogic.ObjectAssociation;
using TraceTool;

namespace MetaBuilder.BusinessFacade.Storage.RepositoryTemp
{
    public partial class Repository
    {
        public System.Collections.Generic.List<MetaObject> MarkedForDeleteItems;

        private string connString
        {
            get { return (reposType == RepositoryType.Server) ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString; }
        }

        public VCStatusList GetObjectState(MetaObject item)
        {
            SqlCommand cmd = new SqlCommand("Meta_GetStatusForMetaObject", new SqlConnection(Variables.Instance.ConnectionString));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@machine", SqlDbType.VarChar, 100));
            cmd.Parameters.Add(new SqlParameter("@VCStatusID", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@pkid", SqlDbType.Int));
            cmd.Parameters["@VCStatusID"].Value = 0;
            cmd.Parameters["@VCStatusID"].Direction = ParameterDirection.InputOutput;
            cmd.Parameters["@pkid"].Value = item.pkid;
            cmd.Parameters["@machine"].Value = item.Machine;
            //cmd.CommandText = "";
            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();

            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

            int VCStatusID = int.Parse(cmd.Parameters["@VCStatusID"].Value.ToString());
            return (VCStatusList)Enum.Parse(typeof(VCStatusList), VCStatusID.ToString());
        }
        public VCStatusList GetObjectState(IRepositoryItem item)
        {
            SqlCommand cmd = new SqlCommand("Meta_GetStatusForMetaObject", new SqlConnection(Variables.Instance.ConnectionString));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@machine", SqlDbType.VarChar, 100));
            cmd.Parameters.Add(new SqlParameter("@VCStatusID", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@pkid", SqlDbType.Int));
            cmd.Parameters["@VCStatusID"].Value = 0;
            cmd.Parameters["@VCStatusID"].Direction = ParameterDirection.InputOutput;
            cmd.Parameters["@pkid"].Value = item.pkid;
            cmd.Parameters["@machine"].Value = item.MachineName;
            //cmd.CommandText = "";
            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();

            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

            int VCStatusID = int.Parse(cmd.Parameters["@VCStatusID"].Value.ToString());
            return (VCStatusList)Enum.Parse(typeof(VCStatusList), VCStatusID.ToString());
        }

        public VCStatusList GetServerObjectState(IRepositoryItem item)
        {
            SqlCommand cmd = new SqlCommand("Meta_GetStatusForMetaObject", new SqlConnection(Variables.Instance.ServerConnectionString));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@machine", SqlDbType.VarChar, 100));
            cmd.Parameters.Add(new SqlParameter("@VCStatusID", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@pkid", SqlDbType.Int));
            cmd.Parameters["@VCStatusID"].Value = 0;
            cmd.Parameters["@VCStatusID"].Direction = ParameterDirection.InputOutput;
            cmd.Parameters["@pkid"].Value = item.pkid;
            cmd.Parameters["@machine"].Value = item.MachineName;
            //cmd.CommandText = "";
            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();

            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

            int VCStatusID = int.Parse(cmd.Parameters["@VCStatusID"].Value.ToString());
            return (VCStatusList)Enum.Parse(typeof(VCStatusList), VCStatusID.ToString());
        }
        public VCStatusList GetServerObjectState(MetaObject item)
        {
            SqlCommand cmd = new SqlCommand("Meta_GetStatusForMetaObject", new SqlConnection(Variables.Instance.ServerConnectionString));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@machine", SqlDbType.VarChar, 100));
            cmd.Parameters.Add(new SqlParameter("@VCStatusID", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@pkid", SqlDbType.Int));
            cmd.Parameters["@VCStatusID"].Value = 0;
            cmd.Parameters["@VCStatusID"].Direction = ParameterDirection.InputOutput;
            cmd.Parameters["@pkid"].Value = item.pkid;
            cmd.Parameters["@machine"].Value = item.Machine;
            //cmd.CommandText = "";
            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();

            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

            int VCStatusID = int.Parse(cmd.Parameters["@VCStatusID"].Value.ToString());
            return (VCStatusList)Enum.Parse(typeof(VCStatusList), VCStatusID.ToString());
        }

        public VCStatusList GetState(IRepositoryItem item)
        {
            SqlCommand cmd = new SqlCommand("REPLACEME", new SqlConnection(connString));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@machine", SqlDbType.VarChar, 100));
            cmd.Parameters.Add(new SqlParameter("@VCStatusID", SqlDbType.Int));
            cmd.Parameters["@VCStatusID"].Value = 0;
            cmd.Parameters["@VCStatusID"].Direction = ParameterDirection.InputOutput;

            if (item is MetaBase)
            {
                cmd.CommandText = "Meta_GetStatusForMetaObject";
                MetaBase mbase = item as MetaBase;
                cmd.Parameters.Add(new SqlParameter("@pkid", SqlDbType.Int));
                cmd.Parameters["@pkid"].Value = mbase.pkid;
                cmd.Parameters["@machine"].Value = mbase.MachineName;
            }

            if (item is ObjectAssociation)
            {
                ObjectAssociation oassoc = item as ObjectAssociation;

                cmd.CommandText = "Meta_GetStatusForAssociation";
                MetaBase mbase = item as MetaBase;
                cmd.Parameters.Add(new SqlParameter("@CAid", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@ObjectID", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@childObjectID", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@objectmachine", SqlDbType.VarChar, 50));
                cmd.Parameters.Add(new SqlParameter("@childobjectmachine", SqlDbType.VarChar, 50));
                cmd.Parameters["@CAid"].Value = oassoc.CAid;
                cmd.Parameters["@ObjectID"].Value = oassoc.ObjectID;
                cmd.Parameters["@childObjectID"].Value = oassoc.ChildObjectID;
                cmd.Parameters["@objectmachine"].Value = oassoc.ObjectMachine;
                cmd.Parameters["@childobjectmachine"].Value = oassoc.ChildObjectMachine;
                cmd.Parameters["@machine"].Value = oassoc.Machine;
            }

            if (item is GraphFile)
            {
                GraphFile file = item as GraphFile;
                cmd.CommandText = "Meta_GetStatusForGraphFile";
                cmd.Parameters.Add(new SqlParameter("@pkid", SqlDbType.Int));
                cmd.Parameters["@pkid"].Value = file.pkid;
                cmd.Parameters["@machine"].Value = file.Machine;
            }

            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();

            cmd.ExecuteNonQuery();
            cmd.Connection.Close();

            int VCStatusID = int.Parse(cmd.Parameters["@VCStatusID"].Value.ToString());
            return (VCStatusList)Enum.Parse(typeof(VCStatusList), VCStatusID.ToString());
        }

        internal ActionResult SaveState(ref IRepositoryItem item, VCStatusList targetState, Workspace targetWorkspace, IRepositoryItem parentItem, bool LogIsWatching, VCStatusList iTargetState, VCStatusList iCurrentState, ItemAndRule itemandrule)
        {
            //#if DEBUG
            //StringBuilder sb = new StringBuilder();
            //if (parentItem != null)
            //{
            //    sb.Append(item.ToString() + "\tPKID:" + item.pkid.ToString() + "\tTargetState:" + targetState.ToString() +
            //              "\t" + parentItem.ToString() + "\tPKID:" + parentItem.pkid.ToString());
            //}
            //else
            //{
            //    sb.Append(item.ToString() + "\tPKID:" + item.pkid.ToString() + "\tTargetState:" + targetState.ToString() +
            //              "NO PARENT");
            //}
            //TTrace.Debug.Send("Saving State" + this.reposType.ToString(), sb.ToString());
            //#endif

            if (targetWorkspace == null && item is IWorkspaceItem)
            {
                IWorkspaceItem wsItem = item as IWorkspaceItem;
                targetWorkspace = DataRepository.Connections[reposType.ToString()].Provider.WorkspaceProvider.GetByNameWorkspaceTypeId(wsItem.WorkspaceName, wsItem.WorkspaceTypeId);
            }

            ActionResult result = new ActionResult();
            bool IsServer = GetType() == typeof(Server);
            string provider = (reposType == RepositoryType.Server) ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider;

            SqlCommand cmd = new SqlCommand("REPLACEME", new SqlConnection(connString));
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@machine", SqlDbType.VarChar, 100));
            cmd.Parameters["@machine"].Value = item.MachineName;
            cmd.Parameters.Add(new SqlParameter("@VCStatusID", SqlDbType.Int));
            cmd.Parameters["@VCStatusID"].Value = (int)targetState; //WHEN THE TARGETSTATE IS 'WRONG' CHANGE IT BEFORE EXECUTING
            cmd.Parameters.Add(new SqlParameter("@VCMachine", SqlDbType.VarChar, 50));
            if (itemandrule.Rule.ChangeUser)
                cmd.Parameters["@VCMachine"].Value = strings.GetVCIdentifier();
            else
                cmd.Parameters["@VCMachine"].Value = item.VCUser;

            GraphFileKey key = null;
            if (parentItem != null && parentItem is GraphFile)
            {
                key = new GraphFileKey();
                key.Machine = parentItem.MachineName;
                key.pkid = parentItem.pkid;
            }

            if (item is MetaBase)
            {
                if (item.State != VCStatusList.MarkedForDelete)
                {
                    item.State = targetState;
                }
                try
                {
                    //check if i have permission for this workspace based on the rule's requirement. If i do not, save associations for graphfileobjectonly
                    bool permission = true;
                    //custom overriding for objects
                    //rule provider is originating panel. provider is what is being updated. state is what provider is after update
                    string message = "";
                    VCStatusList currentServerState = GetServerObjectState(item);

                    string client = Core.Variables.Instance.ClientProvider;
                    string server = Core.Variables.Instance.ServerProvider;

                    if (itemandrule.Rule.Provider == client && provider == server && itemandrule.Rule.ClientState == VCStatusList.Locked && itemandrule.Rule.ServerState == VCStatusList.Locked)
                    {
                        permission = false;
                    }
                    else if (itemandrule.Rule.Provider == server && provider == client && itemandrule.Rule.RuleNumber == 230)
                    {
                        permission = false;
                        //update client to reflect server?
                        //setMissingObjectState(1, d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetBypkidMachine(item.pkid, item.MachineName), Core.Variables.Instance.ClientProvider);
                        message = " is already checked out.";
                    }
                    else if (itemandrule.Rule.Provider == client && provider == server && itemandrule.Rule.RuleNumber == 116)
                    {
                        permission = false;
                        //update client to reflect server?
                        //setMissingObjectState(1, d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetBypkidMachine(item.pkid, item.MachineName), Core.Variables.Instance.ClientProvider);
                        message = " is checkout out to a different user.";
                    }
                    else if (itemandrule.Rule.Provider == server && provider == server && itemandrule.Rule.RuleNumber == 116)
                    {
                        permission = false;
                        //update client to reflect server?
                        //setMissingObjectState(1, d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetBypkidMachine(item.pkid, item.MachineName), Core.Variables.Instance.ClientProvider);
                        message = " is checkout out to a different user.";
                    }
                    else if (itemandrule.Rule.Provider == client && provider == server && (itemandrule.Rule.RuleNumber == 106 || itemandrule.Rule.RuleNumber == 117))
                    {
                        permission = false;
                    }
                    else if (itemandrule.Rule.Provider == client && provider == server && itemandrule.Rule.ClientState == VCStatusList.MarkedForDelete && itemandrule.Rule.ServerState == VCStatusList.MarkedForDelete)
                    {
                        if (currentServerState == VCStatusList.MarkedForDelete)
                        {
                            message = " is already marked for delete";
                            permission = false;
                        }
                    }
                    else if (provider == server && ((itemandrule.Rule.ClientState == VCStatusList.CheckedIn && itemandrule.Rule.ServerState == VCStatusList.CheckedIn) || (itemandrule.Rule.ClientState == VCStatusList.CheckedOut && itemandrule.Rule.ServerState == VCStatusList.None)))
                    {
                        //Tarrynn 3 March 2014 Added changeUser (checkedin to checkedin)
                        //server state==0 means does not exist
                        if (currentServerState == 0 && provider == server)
                        {
                            permission = true;
                        }
                        else
                        {
                            if (currentServerState != VCStatusList.CheckedOut && (item as MetaBase).State != VCStatusList.None)
                            {
                                permission = false;
                                message = " is not checked out on the server";

                                //update client to reflect server?
                                //setMissingObjectState(1, d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetBypkidMachine(item.pkid, item.MachineName), Core.Variables.Instance.ClientProvider);
                            }
                            else if (currentServerState == VCStatusList.CheckedOut && (item as MetaBase).State != VCStatusList.None)
                            {
                                if (itemandrule.Rule.SameUser)
                                {
                                    if (getCurrentUser((item as MetaBase), server) != strings.GetVCIdentifier())
                                    {
                                        permission = false;
                                        message = " is not checked out to you on the server";
                                    }
                                }
                            }
                        }
                    }

                    MetaBase mb = SaveMetaObject(item, provider, cmd, key, targetWorkspace, permission);
                    cmd.Parameters["@VCStatusID"].Value = (int)mb.State;
                    result.TargetState = targetState.ToString();
                    result.Repository = reposType.ToString();

                    //update all associations to follow this targetstate
                    //if (permission)
                    syncAssociations(mb);

                    if (LogIsWatching)
                    {
                        result.Message = permission ? mb.ToString() + " [" + mb._ClassName + "]" : mb.ToString() + " [" + mb._ClassName + "] : Workspace(" + mb.WorkspaceName + ") permission prevented execution";// changed to " + targetState.ToString() + " on " + reposType.ToString();
                        if (message != "")
                        {
                            result.Message = mb.ToString() + " [" + mb._ClassName + " ]" + message;
                        }
                        if (!permission)
                            result.intermediate = true;
                        result.Success = true;
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteLog(ex.ToString());
                    if (LogIsWatching)
                    {
                        result.Message = "Object not transferred"; //changed to " + targetState.ToString() + " on " + reposType.ToString();
                        result.Success = false;
                    }
                }
            }
            else if (item is ObjectAssociation)
            {
                ObjectAssociation oassoc = item as ObjectAssociation;
                oassoc.VCStatusID = (int)targetState;
                oassoc.VCUser = Variables.Instance.UserDomainIdentifier;
                try
                {
                    //result = SaveObjectAssociation(item, cmd, targetState, key, LogIsWatching); //refactored, less dataaccess
                    result = SaveObjectAssociationNew(item, provider, cmd, targetState, key, LogIsWatching);
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    Log.WriteLog("Association not transferred" + Environment.NewLine + ex.ToString());
                    result.Repository = reposType.ToString();
                    result.Message = "Association not transferred";
                    result.Success = false;
                }
            }
            else if (item is GraphFile)
            {
                try
                {
                    //reset value of lastkey every time a file is saved so that if the file does not save the last key will be null and not a previous diagram
                    lastKey = null;

                    result = SaveGraphFile(ref item, provider, cmd, targetWorkspace, targetState, LogIsWatching, iTargetState, iCurrentState);
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    Log.WriteLog(ex.ToString());
                    result.Message = "Diagram not transferred";
                    result.Success = false;
                }
            }

            //OVERRIDES Nettiers updates
            if (cmd.CommandText.IndexOf("REPLACEME") == -1)
            {
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            return result;
        }

        private void syncAssociations(MetaBase mb)
        {
            //all parents
            TList<ObjectAssociation> parent = DataRepository.Connections[reposType.ToString()].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(mb.pkid, mb.MachineName);
            foreach (ObjectAssociation pAss in parent)
            {
                int targetStateID = (int)mb.State;
                if (targetStateID == 3 || targetStateID == 4 || targetStateID == 8) //always set status when this happens
                {
                    pAss.VCStatusID = targetStateID;
                }
                else //only set state if other object is not one of the above
                {
                    MetaObject otherObject = DataRepository.Connections[reposType.ToString()].Provider.MetaObjectProvider.GetBypkidMachine(pAss.ChildObjectID, pAss.ChildObjectMachine);
                    if (otherObject.VCStatusID == 3 || otherObject.VCStatusID == 4 || otherObject.VCStatusID == 8)
                    {
                        //other object is MFD|LOCKED|OBSOLETE so we skip changing the status
                        pAss.VCStatusID = otherObject.VCStatusID;
                    }
                    else
                    {
                        pAss.VCStatusID = targetStateID;
                    }
                }
                pAss.VCUser = strings.GetVCIdentifier();
                DataRepository.Connections[reposType.ToString()].Provider.ObjectAssociationProvider.Save(pAss);
            }
            //all children
            TList<ObjectAssociation> child = DataRepository.Connections[reposType.ToString()].Provider.ObjectAssociationProvider.GetByChildObjectIDChildObjectMachine(mb.pkid, mb.MachineName);
            foreach (ObjectAssociation cAss in child)
            {
                int targetStateID = (int)mb.State;
                if (targetStateID == 3 || targetStateID == 4 || targetStateID == 8) //always set status when this happens
                {
                    cAss.VCStatusID = targetStateID;
                }
                else //only set state if other object is not one of the above
                {
                    MetaObject otherObject = DataRepository.Connections[reposType.ToString()].Provider.MetaObjectProvider.GetBypkidMachine(cAss.ObjectID, cAss.ObjectMachine);
                    if (otherObject.VCStatusID == 3 || otherObject.VCStatusID == 4 || otherObject.VCStatusID == 8)
                    {
                        //other object is MFD|LOCKED|OBSOLETE so we skip changing the status
                        cAss.VCStatusID = otherObject.VCStatusID;
                    }
                    else
                    {
                        cAss.VCStatusID = targetStateID;
                    }
                }
                cAss.VCUser = strings.GetVCIdentifier();
                DataRepository.Connections[reposType.ToString()].Provider.ObjectAssociationProvider.Save(cAss);
            }
        }

        //1	CheckedIn
        //2	CheckedOut
        //5	CheckedOutRead
        //4	Locked
        //8	MarkedForDelete
        //7	None
        //3	Obsolete
        //6	PartiallyCheckedIn
        //9	PCI_Revoked

        private MetaBase SaveMetaObject(IRepositoryItem item, string provider, SqlCommand cmd, GraphFileKey graphFileKey, Workspace targetWorkspace, bool hasPermission)
        {
            MetaBase mbase = item as MetaBase;
            // load the latest value
            MetaObject moCurrentInDatabase = DataRepository.Connections[provider].Provider.MetaObjectProvider.GetBypkidMachine(mbase.pkid, mbase.MachineName);
            bool foundIssues = false;

            #region permissions
            if (moCurrentInDatabase != null)
            {
                // 
                VCStatusList currentStatus = (VCStatusList)moCurrentInDatabase.VCStatusID;
                if (currentStatus == VCStatusList.CheckedIn && item.State == VCStatusList.CheckedIn)
                {
                    foundIssues = true;
                }

                if (currentStatus == VCStatusList.CheckedIn && item.State == VCStatusList.PartiallyCheckedIn)
                {
                    foundIssues = true;
                }

                if (currentStatus == VCStatusList.Locked && item.State == VCStatusList.PartiallyCheckedIn)
                {
                    foundIssues = true;
                }

                if (reposType == RepositoryType.Client)
                {
                    if (currentStatus == VCStatusList.MarkedForDelete && item.State == VCStatusList.PartiallyCheckedIn)
                    {
                        foundIssues = true;
                    }
                    if (currentStatus == VCStatusList.MarkedForDelete && item.State == VCStatusList.CheckedIn)
                    {
                        foundIssues = true;
                    }

                    if (currentStatus == VCStatusList.Obsolete && item.State == VCStatusList.CheckedIn)
                    {
                        foundIssues = false;
                    }

                    if (currentStatus == VCStatusList.Obsolete && item.State == VCStatusList.PartiallyCheckedIn)
                    {
                        foundIssues = true;
                    }
                }
            }
            #endregion

            if (hasPermission)
            {
                if (!foundIssues || reposType == RepositoryType.Client)
                {
                    cmd.CommandText = "Meta_SetStatusForMetaObject";
                    cmd.Parameters.Add(new SqlParameter("@pkid", SqlDbType.Int));
                    cmd.Parameters["@pkid"].Value = mbase.pkid;
                    cmd.Parameters["@machine"].Value = mbase.MachineName;

                    mbase.SaveToRepository(Guid.NewGuid(), reposType.ToString());

                    MetaObject mo = DataRepository.Connections[provider].Provider.MetaObjectProvider.GetBypkidMachine(mbase.pkid, mbase.MachineName);
                    if (mo != null)
                    {
                        if (mbase.State != VCStatusList.CheckedOut) //senseless?
                        {
                            mo.VCMachineID = strings.GetVCIdentifier();
                        }
                        else
                        {
                            mo.VCMachineID = null;
                        }

                        //BUG FIX : sync change 2012 may 10 (rebugged 30 may 2012)
                        if (provider == Core.Variables.Instance.ServerProvider)
                        {
                            mo.UserID = MetaBase.GetServerUserID();
                        }
                        else
                        {
                            mo.UserID = Core.Variables.Instance.UserID;
                            mo.WorkspaceName = mbase.WorkspaceName;
                            mo.WorkspaceTypeId = mbase.WorkspaceTypeId;
                        }

                        DataRepository.Connections[provider].Provider.MetaObjectProvider.Save(mo);

                        if (lastKey == null && this.reposType == RepositoryType.Client) // when we checkin objects alone
                        {
                            if (mbase.State == VCStatusList.MarkedForDelete)
                            {
                                MarkedForDeleteItems.Add(mo);
                            }
                        }
                    }
                    else
                    {
                        //Object does not exist locally
                    }

                }
            }
            //TODO VARIABLE FOR ISSUES
            // if this object is checked out/in as part of a document, also add a graphfileobject row if it doesnt exist (this will only happen the first time)
            if (lastKey != null)
            {
                //b.GraphFile gfile = d.DataRepository.Connections[provider].Provider.GraphFileProvider.GetBypkidMachine(graphFileKey.Pkid, graphFileKey.Machine);
                GraphFileObjectKey key = GetGraphFileObjectRowKey(mbase, graphFileKey);
                GraphFileObject gfo = DataRepository.Connections[provider].Provider.GraphFileObjectProvider.Get(key);
                if (gfo == null)
                {
                    gfo = new GraphFileObject();
                    gfo.MetaObjectID = mbase.pkid;
                    gfo.MachineID = mbase.MachineName;
                    gfo.GraphFileID = lastKey.pkid;
                    gfo.GraphFileMachine = lastKey.Machine;
                }
                DataRepository.Connections[provider].Provider.GraphFileObjectProvider.Save(gfo);
            }

            //Variables.Instance.CurrentWorkspaceName = oldWorkspaceName;
            //Variables.Instance.CurrentWorkspaceTypeId = oldWorkspaceTypeId;
            return mbase;
        }

        /// <summary>
        /// Gets the key for the graphfileobject (for lookups)
        /// </summary>
        /// <param name="mbase">The mbase.</param>
        /// <param name="gfile">The gfile.</param>
        /// <returns></returns>
        private static GraphFileObjectKey GetGraphFileObjectRowKey(MetaBase mbase, GraphFileKey graphFileKey)
        {
            GraphFileObjectKey key = new GraphFileObjectKey();
            key.GraphFileID = graphFileKey.pkid;
            key.GraphFileMachine = graphFileKey.Machine;
            key.MachineID = mbase.MachineName;
            key.MetaObjectID = mbase.pkid;
            return key;
        }

        private ActionResult SaveObjectAssociationNew(IRepositoryItem item, string provider, SqlCommand cmd, VCStatusList targetStatus, GraphFileKey key, bool LogIsWatching)
        {
            ActionResult retval = new ActionResult();
            ObjectAssociation oassoc = item as ObjectAssociation;
            //try find ass on server
            //string provider = reposType.ToString();
            ObjectAssociation assocInDB = DataRepository.Connections[provider].Provider.ObjectAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(oassoc.CAid, oassoc.ObjectID, oassoc.ChildObjectID, oassoc.ObjectMachine, oassoc.ChildObjectMachine);
            if (assocInDB == null)
            {
                //set to item
                assocInDB = oassoc;
            }

            //save status
            assocInDB.VCStatusID = (int)targetStatus;
            assocInDB.VCUser = strings.GetVCIdentifier();
            //save new ass with target status
            try
            {
                DataRepository.Connections[provider].Provider.ObjectAssociationProvider.Save(assocInDB);

                //check for graphfile
                if (key != null)
                {
                    GraphFileAssociationKey gfakey = new GraphFileAssociationKey(key.pkid, key.Machine, assocInDB.ChildObjectMachine, assocInDB.CAid, assocInDB.ObjectID, assocInDB.ChildObjectID, assocInDB.ObjectMachine);
                    GraphFileAssociation gfA = DataRepository.Connections[provider].Provider.GraphFileAssociationProvider.Get(gfakey);
                    if (gfA == null) //always null for new diagrams (in terms of pkid)
                    {
                        gfA = new GraphFileAssociation();
                        gfA.CAid = oassoc.CAid;
                        gfA.ChildObjectID = oassoc.ChildObjectID;
                        gfA.ChildObjectMachine = oassoc.ChildObjectMachine;
                        gfA.ObjectID = oassoc.ObjectID;
                        gfA.ObjectMachine = oassoc.ObjectMachine;
                        gfA.GraphFileID = key.pkid;
                        gfA.GraphFileMachine = key.Machine;
                    }
                    //    gfA = DataRepository.Connections[provider].Provider.GraphFileAssociationProvider.Save(gfA);
                    //}
                    //else
                    gfA = DataRepository.Connections[provider].Provider.GraphFileAssociationProvider.Save(gfA);
                }
            }
            catch
            {
                Log.WriteLog("Class Association(" + assocInDB.CAid + ") missing on " + provider);
            }

            if (LogIsWatching)
            {
                string assocTypeID = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByCAid(oassoc.CAid).Caption;
                retval.Message = assocTypeID; //  ((LinkAssociationType)assocTypeID).ToString();// +" - " + targetStatus.ToString() + " on " + reposType.ToString();
                retval.Repository = reposType.ToString();
                retval.FromState = Enum.GetName(typeof(VCStatusList), item.State); //ADDED
                retval.TargetState = Enum.GetName(typeof(VCStatusList), targetStatus);
            }
            //return
            return retval;
        }

        //Refactor
        private ActionResult SaveObjectAssociation(IRepositoryItem item, SqlCommand cmd, VCStatusList targetStatus, GraphFileKey key, bool LogIsWatching)
        {
            ActionResult retval = new ActionResult();
            ObjectAssociation oassoc = item as ObjectAssociation;
            //attempt to get oa from repository else insert it, inserting fails because it exists, then tries update, then returns value
            bool insertionFailure = InsertAssociationIfNecessary(oassoc, targetStatus);

            //if (targetStatus == MetaBuilder.BusinessLogic.VCStatusList.Obsolete)
            //{
            //    cmd.CommandText = "Meta_SetStatusForAssociation";
            //}

            //cmd.Parameters.Add(new SqlParameter("@CAid", SqlDbType.Int));
            //cmd.Parameters.Add(new SqlParameter("@ObjectID", SqlDbType.Int));
            //cmd.Parameters.Add(new SqlParameter("@childObjectID", SqlDbType.Int));
            //cmd.Parameters.Add(new SqlParameter("@objectmachine", SqlDbType.VarChar, 50));
            //cmd.Parameters.Add(new SqlParameter("@childobjectmachine", SqlDbType.VarChar, 50));
            //cmd.Parameters["@CAid"].Value = oassoc.CAid;
            //cmd.Parameters["@ObjectID"].Value = oassoc.ObjectID;
            //cmd.Parameters["@childObjectID"].Value = oassoc.ChildObjectID;
            //cmd.Parameters["@objectmachine"].Value = oassoc.ObjectMachine;
            //cmd.Parameters["@childobjectmachine"].Value = oassoc.ChildObjectMachine;
            //cmd.Parameters["@machine"].Value = key.Machine;
            //cmd.Parameters["@VCStatusID"].Value = (int)targetStatus;

            if (key != null)
            {
                //b.GraphFile gfile = d.DataRepository.Connections[provider].Provider.GraphFileProvider.GetBypkidMachine(graphFileKey.Pkid, graphFileKey.Machine);
                GraphFileAssociationKey gfakey = new GraphFileAssociationKey(key.pkid, key.Machine, oassoc.ChildObjectMachine, oassoc.CAid, oassoc.ObjectID, oassoc.ChildObjectID, oassoc.ObjectMachine);
                GraphFileAssociation gfA = DataRepository.Connections[reposType.ToString()].Provider.GraphFileAssociationProvider.Get(gfakey);
                if (gfA == null)
                {
                    gfA = new GraphFileAssociation();
                    gfA.CAid = oassoc.CAid;
                    gfA.ChildObjectID = oassoc.ChildObjectID;
                    gfA.ChildObjectMachine = oassoc.ChildObjectMachine;
                    gfA.ObjectID = oassoc.ObjectID;
                    gfA.ObjectMachine = oassoc.ObjectMachine;
                    gfA.GraphFileID = key.pkid;
                    gfA.GraphFileMachine = key.Machine;
                }

                ObjectAssociationKey assKey = new ObjectAssociationKey();
                assKey.CAid = gfakey.CAid;
                assKey.ChildObjectID = gfakey.ChildObjectID;
                assKey.ChildObjectMachine = gfakey.ChildObjectMachine;
                assKey.ObjectID = gfakey.ObjectID;
                assKey.ObjectMachine = gfakey.ObjectMachine;

                b.ObjectAssociation oassocSaved = DataRepository.Connections[reposType.ToString()].Provider.ObjectAssociationProvider.Get(assKey);

                if (oassocSaved == null || !insertionFailure)
                {
                    //ADDED
                    oassocSaved = oassoc; //Set to original 1 that was 'inserted if necessary' THIS IS SECOND TIME WE RUN SAME CODE 
                    oassocSaved.State = targetStatus;
                    if (targetStatus != VCStatusList.CheckedOutRead)
                        oassocSaved.VCUser = strings.GetVCIdentifier();

                    DataRepository.Connections[reposType.ToString()].Provider.ObjectAssociationProvider.Save(oassoc);
                }
                else  //ADDED
                {
                    oassocSaved.State = targetStatus;
                    if (targetStatus != VCStatusList.CheckedOutRead)
                        oassocSaved.VCUser = strings.GetVCIdentifier();
                    DataRepository.Connections[reposType.ToString()].Provider.ObjectAssociationProvider.Update(oassoc);
                }
                DataRepository.Connections[reposType.ToString()].Provider.GraphFileAssociationProvider.Save(gfA);
            }

            b.ObjectAssociationKey oakey = new ObjectAssociationKey(oassoc);
            ObjectAssociation oaSaved = DataRepository.Connections[reposType.ToString()].Provider.ObjectAssociationProvider.Get(oakey);
            if (oaSaved == null)
            {
                //ADDED
                oassoc.State = targetStatus;
                if (targetStatus != VCStatusList.CheckedOutRead)
                    oassoc.VCUser = strings.GetVCIdentifier();
                DataRepository.Connections[reposType.ToString()].Provider.ObjectAssociationProvider.Save(oassoc);
                //so here we add a OA but dont add it to a graphfile?
            }

            if (LogIsWatching)
            {
                int assocTypeID = DataRepository.Connections[reposType.ToString()].Provider.ClassAssociationProvider.GetByCAid(oassoc.CAid).AssociationTypeID;
                retval.Message = ((LinkAssociationType)assocTypeID).ToString();// +" - " + targetStatus.ToString() + " on " + reposType.ToString();

                if (!insertionFailure)//this is just a message so that the status doesnt go red, EVEN THOUGH Succeeds above
                    retval.Message += "   (Primary insertion failed)";

                retval.Repository = reposType.ToString();
                retval.FromState = Enum.GetName(typeof(VCStatusList), item.State); //ADDED
                retval.TargetState = Enum.GetName(typeof(VCStatusList), targetStatus);
                //retval.Success = true;
            }
            return retval;
        }

        //changed to bool added try catches
        private bool InsertAssociationIfNecessary(ObjectAssociation association, VCStatusList targetState)
        {
            // try to retrieve the assoc from the db
            ObjectAssociation assocInDB = DataRepository.Connections[reposType.ToString()].Provider.ObjectAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(
                association.CAid, association.ObjectID, association.ChildObjectID, association.ObjectMachine, association.ChildObjectMachine);
            if (assocInDB == null)
            {
                association.State = targetState;
                association.VCUser = Core.Variables.Instance.UserFullName;
                association.VCUser = strings.GetVCIdentifier();
                try
                {
                    DataRepository.Connections[reposType.ToString()].Provider.ObjectAssociationProvider.Save(association);
                    return true;
                }
                catch (Exception ex) //ASSOCIATION NOT TRANSFERRED ERROR
                {
                    Log.WriteLog("Cannot insert association in repository (" + reposType.ToString() + ") attempting to update..." + Environment.NewLine + association.ToString() + Environment.NewLine + Environment.NewLine + ex.ToString());
                    try
                    {
                        DataRepository.Connections[reposType.ToString()].Provider.ObjectAssociationProvider.Update(association);
                        return true;
                    }
                    catch
                    {
                        Log.WriteLog("Cannot update association in repository (" + reposType.ToString() + ") Action Failed" + Environment.NewLine + association.ToString());
                        return false;
                    }
                }
            }
            return true;
        }

        private ActionResult SaveGraphFile(ref IRepositoryItem item, string provider, SqlCommand cmd, Workspace targetWorkspace, VCStatusList targetState, bool LogIsWatching, VCStatusList iTargetState, VCStatusList iCurrentState)
        {
            ActionResult result = new ActionResult();
            GraphFile file = item as GraphFile;

            //string currentVCUser = file.VCMachineID; //remember who has this file originally
            file = RetrieveAndInsertFile(file, targetWorkspace, targetState, iCurrentState);
            //GraphFile newfile = RetrieveAndInsertFile(file, targetWorkspace, targetState);

            cmd.CommandText = "Meta_SetStatusForGraphFile";

            cmd.Parameters.Add(new SqlParameter("@pkid", SqlDbType.Int));
            cmd.Parameters["@pkid"].Value = file.pkid;
            cmd.Parameters["@machine"].Value = file.Machine;
            //9 October 2013 - Do not update server user when checked out read
            //if (provider == Core.Variables.Instance.ServerProvider && iTargetState == VCStatusList.CheckedOutRead)
            //    cmd.Parameters["@VCMachine"].Value = currentVCUser;

            if (provider == Core.Variables.Instance.ClientProvider)
            {
                if (file.PreviousVersionID.HasValue)
                {
                    if (file.PreviousVersionID.Value > 0)
                    {
                        //this is here so that we can update the client file using the cmd
                        //the previousversionid is the id of the file on the client that was checkedin
                        if (targetState == VCStatusList.CheckedIn)
                        {
                            cmd.Parameters["@pkid"].Value = file.PreviousVersionID;
                            file.pkid = file.PreviousVersionID.Value;
                        }
                        else
                        {

                        }
                    }
                }
            }
            else
            {
                if (file.PreviousVersionID.HasValue)
                {
                    if (file.PreviousVersionID.Value > 0)
                    {
                        if (targetState != VCStatusList.CheckedIn)
                        {

                        }
                    }
                }
            }

            //28 January 2014
            //AssociationHelper assHelper = new AssociationHelper();
            //assHelper.DeleteGraphFileAssociationByGraphFileIDGraphFileMachine(file.pkid, file.Machine);

            item = file as IRepositoryItem;

            if (LogIsWatching)
            {
                //result.Success = true;
                result.Repository = reposType.ToString();
                result.TargetState = targetState.ToString();
                result.Message = "File: " + strings.GetFileNameOnly(file.Name) + " [" + file.pkid.ToString() + " " + file.Machine + "]";// changed to " + targetState.ToString() + " on " + reposType.ToString();
            }
            return result;
        }

        private GraphFile RetrieveAndInsertFile(GraphFile file, Workspace workspace, VCStatusList targetState, VCStatusList iCurrentState)
        {
            GraphFileKey key = new GraphFileKey();
            key.pkid = file.pkid;
            key.Machine = file.Machine;

            //file.ModifiedDate = DateTime.Now;

            string readProvider = null;
            string insertProvider = null;

            #region Set Providers
            switch (targetState)
            {
                case VCStatusList.PCI_Revoked:
                    if (reposType == RepositoryType.Server)
                    {
                        readProvider = Core.Variables.Instance.ServerProvider;
                        insertProvider = Core.Variables.Instance.ClientProvider;
                    }
                    else
                    {
                        readProvider = Core.Variables.Instance.ServerProvider;
                    }
                    break;
                case VCStatusList.CheckedIn:
                    if (reposType == RepositoryType.Server) //If we are server and going to checked in at which point should we ever read from the client?
                    {
                        //BEFORE
                        //readProvider = Core.Variables.Instance.ClientProvider;
                        //insertProvider = Core.Variables.Instance.ServerProvider;

                        //AFTER
                        //locked to checked in
                        //                                                              //MFD to checked in--cannot MFD a diagram?
                        //Obsolete to checked in
                        //!!!!!!!!checkout to checked in ;; ie:Force check in!!!!!!!!!!!!
                        //PCI to checked in
                        if (iCurrentState == VCStatusList.Locked || iCurrentState == VCStatusList.MarkedForDelete || iCurrentState == VCStatusList.Obsolete || iCurrentState == VCStatusList.PartiallyCheckedIn)
                        {
                            //We are administrating a diagram on the server
                            readProvider = Core.Variables.Instance.ServerProvider;
                        }
                        else
                        {
                            if (Force) //We are forcing a diagram to checkedin on the server
                                readProvider = Core.Variables.Instance.ServerProvider;
                            else //we are checking in a diagram from the client
                                readProvider = Core.Variables.Instance.ClientProvider;
                        }
                        insertProvider = Core.Variables.Instance.ServerProvider;
                    }
                    else
                    {
                        readProvider = Core.Variables.Instance.ClientProvider;
                    }
                    break;
                case VCStatusList.CheckedOut:
                    if (reposType == RepositoryType.Server)
                    {
                        readProvider = Core.Variables.Instance.ServerProvider;
                    }
                    else
                    {
                        readProvider = Core.Variables.Instance.ServerProvider;
                        insertProvider = Core.Variables.Instance.ClientProvider;
                    }
                    break;
                case VCStatusList.CheckedOutRead:
                    if (reposType == RepositoryType.Server)
                    {
                        readProvider = Core.Variables.Instance.ServerProvider;
                    }
                    else
                    {
                        readProvider = Core.Variables.Instance.ServerProvider;
                        insertProvider = Core.Variables.Instance.ClientProvider;
                    }
                    break;
                case VCStatusList.Locked:
                    if (reposType == RepositoryType.Server)
                    {
                        readProvider = Core.Variables.Instance.ServerProvider;
                    }
                    else
                    {
                        readProvider = Core.Variables.Instance.ServerProvider;
                        insertProvider = Core.Variables.Instance.ClientProvider; //Allow inserting of locked items into client database(so one can open diagrams)
                    }
                    break;
                case VCStatusList.Obsolete:
                    if (reposType == RepositoryType.Server)
                    {
                        readProvider = Core.Variables.Instance.ServerProvider;
                    }
                    else
                    {
                        readProvider = Core.Variables.Instance.ServerProvider;
                        insertProvider = Core.Variables.Instance.ClientProvider; //Allow inserting of obsolete items into client database(so one can open diagrams)
                    }
                    break;
                case VCStatusList.PartiallyCheckedIn:
                    if (reposType == RepositoryType.Server)
                    {
                        readProvider = Core.Variables.Instance.ClientProvider;
                        insertProvider = Core.Variables.Instance.ServerProvider;
                    }
                    else
                    {
                        readProvider = Core.Variables.Instance.ClientProvider;
                    }
                    break;
            }
            #endregion

            bool UpdateOnly = (insertProvider == null);

            if (!UpdateOnly)
            {
                TempFileGraphAdapter tfga = new TempFileGraphAdapter();
                file = tfga.GetQuickFileDetails(file.pkid, file.MachineName, (readProvider == Core.Variables.Instance.ServerProvider));
                //file = DataRepository.Connections[readProvider].Provider.GraphFileProvider.GetBypkidMachine(key.pkid, key.Machine);
                //if (file == null) //try find it on the insertprovider then :) oooo bug!
                //    file = DataRepository.Connections[insertProvider].Provider.GraphFileProvider.GetBypkidMachine(key.pkid, key.Machine);
                file.Machine = key.Machine;

                BumpVersionIfNecessary(file, targetState);

                //if (insertProvider == Core.Variables.Instance.ClientProvider)
                //{
                //    if (!isFileCheckedOutOnClient(file))
                //    {
                //        file.VCStatusID = (int)targetState;
                //    }
                //}
                //else
                file.VCStatusID = (int)targetState;
                /*b.GraphFile quickFile = tfga.GetQuickFileDetails(file.WorkspaceTypeId,file.WorkspaceName,file.pkid,file.MachineName);
                if (quickFile.MajorVersion<*/
                try
                {
                    lastKey = tfga.InsertFile(file, file.WorkspaceTypeId, file.WorkspaceName, insertProvider);
                    file.pkid = lastKey.pkid;
                }
                catch (Exception ex)
                {
                    Log.WriteLog("File (" + file.Name + ") was unable to be inserted into the " + insertProvider + " via the repository" + Environment.NewLine + ex.ToString());
                    DataRepository.Connections[insertProvider].Provider.GraphFileProvider.Update(file);
                    lastKey = new GraphFileKey(file);
                }
            }
            else
            {
                BumpVersionIfNecessary(file, targetState);
            }

            return file;
        }

        private b.GraphFileKey lastKey;
        private void BumpVersionIfNecessary(GraphFile file, VCStatusList targetState)
        {
            // Baseline the document!
            if (targetState == VCStatusList.CheckedIn && reposType == RepositoryType.Server)
            {
                file.Notes = "Checked in by " + Variables.Instance.UserDomainIdentifier;
                file.IsActive = true;
                file.MajorVersion++;
                file.MinorVersion = 0;
                file.VCUser = null;
                file.ModifiedDate = DateTime.Now;
            }
        }

        private bool isFileCheckedOutOnClient(GraphFile file)
        {
            MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter adapter = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();
            foreach (GraphFile clientFile in adapter.GetAllFilesByTypeID((int)FileTypeList.Diagram, false))
            //foreach (GraphFile clientFile in d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileProvider.GetAll())
            {
                if (clientFile.IsActive && clientFile.OriginalFileUniqueID == file.OriginalFileUniqueID && clientFile.VCStatusID == (int)VCStatusList.CheckedOut)
                {
                    return true;
                }
            }
            return false;
        }
    }
}