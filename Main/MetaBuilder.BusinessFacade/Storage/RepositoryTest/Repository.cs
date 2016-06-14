using System;
using System.Collections.Generic;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using System.Data;
using System.Data.SqlClient;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.BusinessFacade.Storage.RepositoryTemp
{
    public partial class Repository
    {
        PermissionService permissionService;
        public enum RepositoryType
        {
            Server, Client
        }

        public Repository(RepositoryType repositoryType)
        {
            this.reposType = repositoryType;
            permissionService = new PermissionService();
        }
        RepositoryType reposType;

        bool Force = false;
        public virtual ActionResult SetState(ref ItemAndRule itemAndRule, VCStatusList targetState, Workspace targetWorkspace, bool LogIsWatching)
        {
            itemAndRule.Item.State = targetState;
            IRepositoryItem item = itemAndRule.Item;
            List<ItemAndRule> embeddedRules = new List<ItemAndRule>();
            for (int i = 0; i < itemAndRule.EmbeddedItemRules.Count; i++)
            {
                embeddedRules.Add(itemAndRule.EmbeddedItemRules[i]);
            }
            ActionResult result = new ActionResult();
            if (!(item is ObjectAssociation))
            {
                Force = itemAndRule.Rule.Caption.Contains("Force");
                result = SaveState(ref item, targetState, targetWorkspace, null, LogIsWatching, itemAndRule.Rule.TargetState, itemAndRule.Rule.CurrentState, itemAndRule);
                result.TargetState = item.State.ToString();
                itemAndRule.Item = item;
            }

            if (embeddedRules.Count > 0)
            {
                //todo! subitems with partial checkins
                VCStatusList overrideChild = (targetState == VCStatusList.PartiallyCheckedIn) ? VCStatusList.PartiallyCheckedIn : targetState;

                for (int i = 0; i < embeddedRules.Count; i++)
                {
                    IRepositoryItem child = embeddedRules[i].Item;
                    VCStatusList childTargetState = (reposType == RepositoryType.Client) ? embeddedRules[i].Rule.ClientState : embeddedRules[i].Rule.ServerState;
                    if (embeddedRules[i].Rule.CurrentState == VCStatusList.CheckedOut && overrideChild == VCStatusList.CheckedOut && child.VCUser == strings.GetVCIdentifier())
                    {
                        // Console.WriteLine("Already checked out to this user");
                    }
                    else
                    {
                        bool forced = (embeddedRules[i].Rule.Caption.Contains("Force") || embeddedRules[i].Rule.Caption.Contains("Activate"));

                        if (reposType == RepositoryType.Server && (embeddedRules[i].Rule.CurrentState == VCStatusList.Obsolete || embeddedRules[i].Rule.CurrentState == VCStatusList.Locked) && forced)
                        {
                            //not getting graphfile because STATUS IS LOCKED!?

                            //Actually it is not getting graphfile because it is reading it from the client!

                            // do nothing
                            // Console.WriteLine("Do nothing");
                        }
                        else
                        {
                            if (overrideChild == VCStatusList.PartiallyCheckedIn)
                                childTargetState = VCStatusList.PartiallyCheckedIn;
                            if (childTargetState != VCStatusList.None)
                            {
                                ActionResult childResult = SaveState(ref child, childTargetState, targetWorkspace, item, LogIsWatching, embeddedRules[i].Rule.TargetState, embeddedRules[i].Rule.CurrentState, embeddedRules[i]);
                                childResult.FromState = embeddedRules[i].Rule.CurrentState.ToString();
                                childResult.TargetState = child.State.ToString();
                                if (!(child is ObjectAssociation))
                                    result.InnerResults.Add(childResult);
                            }
                            else
                            {
                                if (child.State != VCStatusList.Locked && child.State != VCStatusList.Obsolete)
                                {
                                    ActionResult childResult2 = SaveState(ref child, overrideChild, targetWorkspace, item, LogIsWatching, embeddedRules[i].Rule.TargetState, embeddedRules[i].Rule.CurrentState, embeddedRules[i]);
                                    childResult2.FromState = embeddedRules[i].Rule.CurrentState.ToString();
                                    childResult2.TargetState = child.State.ToString();
                                    if (!(child is ObjectAssociation))
                                        result.InnerResults.Add(childResult2);
                                }
                            }

                        }
                    }
                }
            }

            //15 jan 2014
            //Applies to objects which are removed from diagrams that were checked out and now are no longer embedded in diagram
            //if client=>Server Checkin Check previous server file version for objects which are no longer on current checkedin version
            if (item is GraphFile && reposType == RepositoryType.Server && targetState == VCStatusList.CheckedIn)
            {
                //get a list of files in this files workspace
                GraphFile currentServerFile = (item as GraphFile); ;
                TList<GraphFile> OldServerFiles = new TList<GraphFile>();
                MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter adapter = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();
                foreach (GraphFile oldServerFile in adapter.GetAllFilesByWorkspaceTypeIdWorkspaceName(currentServerFile.WorkspaceTypeId, currentServerFile.WorkspaceName,(int)FileTypeList.Diagram, true))
                //foreach (GraphFile oldServerFile in d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.GraphFileProvider.GetByWorkspaceNameWorkspaceTypeId(currentServerFile.WorkspaceName, currentServerFile.WorkspaceTypeId))
                {
                    if (oldServerFile.OriginalFileUniqueID == currentServerFile.OriginalFileUniqueID && oldServerFile.pkid != currentServerFile.pkid)
                        OldServerFiles.Add(oldServerFile);
                }
                if (OldServerFiles.Count > 0)
                {
                    int previousPKID = 0;
                    GraphFile lastServerVersionFile = null;
                    foreach (GraphFile oldServerFile in OldServerFiles)
                    {
                        if (previousPKID == 0 || previousPKID < oldServerFile.pkid)
                        {
                            previousPKID = oldServerFile.pkid;
                            //This is the previous server file when it was checkedout
                            lastServerVersionFile = oldServerFile;
                        }
                    }
                    if (previousPKID > 0 && lastServerVersionFile != null)
                    {
                        //Diff this files objects with the current ones and act on any objects which are different accordingly
                        TList<GraphFileObject> currentObjects = d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.GraphFileObjectProvider.GetByGraphFileIDGraphFileMachine(currentServerFile.pkid, currentServerFile.Machine);
                        foreach (GraphFileObject prevObj in d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.GraphFileObjectProvider.GetByGraphFileIDGraphFileMachine(lastServerVersionFile.pkid, lastServerVersionFile.Machine))
                        {
                            bool found = false;
                            foreach (GraphFileObject currentObj in currentObjects)
                            {
                                if (currentObj.MetaObjectID == prevObj.MetaObjectID && currentObj.MachineID == prevObj.MachineID)
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                            {
                                MetaObject obj = d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.MetaObjectProvider.GetBypkidMachine(prevObj.MetaObjectID, prevObj.MachineID);//get metaobject
                                string currentServerUser = getCurrentUser(obj, Core.Variables.Instance.ServerProvider);
                                VCStatusList currentServerState = GetServerObjectState(obj);
                                MetaObject clientObj = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetBypkidMachine(prevObj.MetaObjectID, prevObj.MachineID);

                                //check if client set to markedfordelete and is checked out on server to me
                                if (currentServerState == VCStatusList.CheckedOut && currentServerUser == Core.strings.GetVCIdentifier() && clientObj != null && GetObjectState(obj) == VCStatusList.MarkedForDelete)
                                {
                                    setMissingObjectState(8, obj, Core.Variables.Instance.ServerProvider);
                                    MarkedForDeleteItems.Add(obj);
                                    if (clientObj != null)
                                    {
                                        setMissingObjectState(1, obj, Core.Variables.Instance.ClientProvider);
                                    }
                                }
                                else if (currentServerState == VCStatusList.CheckedOut && currentServerUser == Core.strings.GetVCIdentifier()) // if it is checked out to this client
                                {
                                    //TODO : ADD client active file check
                                    bool onActiveCheckedOutFile = false;
                                    foreach (GraphFile clientFile in adapter.GetFilesByObjectId(clientObj.pkid, clientObj.Machine, false))
                                    //foreach (GraphFile clientFile in d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileProvider.GetByMetaObjectIDMachineIDFromGraphFileObject(clientObj.pkid, clientObj.Machine))
                                    {
                                        if (onActiveCheckedOutFile)
                                            break;
                                        onActiveCheckedOutFile = (clientFile.IsActive && clientFile.VCStatusID == 2);
                                    }
                                    if (!onActiveCheckedOutFile)
                                    {
                                        setMissingObjectState(1, obj, Core.Variables.Instance.ServerProvider);
                                        setMissingObjectState(1, clientObj, Core.Variables.Instance.ClientProvider);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (item is ObjectAssociation)
            {
                result = SaveState(ref item, targetState, targetWorkspace, null, LogIsWatching, itemAndRule.Rule.TargetState, itemAndRule.Rule.CurrentState, itemAndRule);
            }
            result.FromState = itemAndRule.Rule.CurrentState.ToString();
            return result;
        }

        private void setMissingObjectState(int vcStatusID, MetaObject obj, string provider)
        {
            //if (provider == Core.Variables.Instance.ServerProvider)
            //{
            //    switch (vcStatusID)
            //    {
            //        case 1: //checkin missing checkout
            //            break;
            //        case 8: //checking missing mfd
            //            break;
            //    }
            //}
            //obj.VCStatusID = vcStatusID; //Set
            //d.DataRepository.Connections[provider].Provider.MetaObjectProvider.Save(obj); //Save
            SQLUpdateObjectState(vcStatusID, obj, provider);
            //Add Result

            if (obj.Class == "PhysicalInformationArtefact" || obj.Class == "LoficalInformationArtefact" || obj.Class == "DataView" || obj.Class == "DataTable" || obj.Class == "Entity" || obj.Class == "DataEntity" || obj.Class.ToLower().Contains("informationartefact"))
            {
                //set all children to the same if they are also checkedout
                foreach (MetaObject childObj in d.DataRepository.Connections[provider].Provider.MetaObjectProvider.GetByObjectIDObjectMachineFromObjectAssociation(obj.pkid, obj.Machine))
                {
                    if (childObj.Class == "DataColumn" || childObj.Class == "Attribute" || childObj.Class == "DataField" || childObj.Class == "DataAttribute")
                    {
                        if (childObj.VCStatusID == 2 && childObj.VCMachineID == Core.strings.GetVCIdentifier()) // if it is checked out to this client
                        {
                            //childObj.VCStatusID = vcStatusID;
                            //d.DataRepository.Connections[provider].Provider.MetaObjectProvider.Save(childObj); //Save
                            SQLUpdateObjectState(vcStatusID, childObj, provider);
                        }
                    }
                }
            }
        }
        public void SQLUpdateObjectState(int vcStatusID, MetaObject obj, string provider)
        {
            string constring = provider == Core.Variables.Instance.ServerProvider ? Core.Variables.Instance.ServerConnectionString : Core.Variables.Instance.ConnectionString;
            SqlConnection conn = new SqlConnection(constring);
            SqlCommand com = new SqlCommand();
            com.Connection = conn;
            com.CommandType = CommandType.Text;
            com.CommandText = "UPDATE MetaObject SET VCStatusID = " + vcStatusID + " WHERE pkid = " + obj.pkid + " AND Machine = '" + obj.Machine + "'";
            if (conn.State != ConnectionState.Open)
                conn.Open();
            com.ExecuteNonQuery();
            conn.Close();
        }
        public void SQLUpdateObjectState(int vcStatusID, int pkid, string machine, string provider)
        {
            string constring = provider == Core.Variables.Instance.ServerProvider ? Core.Variables.Instance.ServerConnectionString : Core.Variables.Instance.ConnectionString;
            SqlConnection conn = new SqlConnection(constring);
            SqlCommand com = new SqlCommand();
            com.Connection = conn;
            com.CommandType = CommandType.Text;
            com.CommandText = "UPDATE MetaObject SET VCStatusID = " + vcStatusID + " WHERE pkid = " + pkid + " AND Machine = '" + machine + "'";
            if (conn.State != ConnectionState.Open)
                conn.Open();
            com.ExecuteNonQuery();
            conn.Close();
        }

        public string getCurrentUser(IRepositoryItem item, string provider)
        {
            string constring = provider == Core.Variables.Instance.ServerProvider ? Core.Variables.Instance.ServerConnectionString : Core.Variables.Instance.ConnectionString;
            SqlConnection conn = new SqlConnection(constring);
            SqlCommand com = new SqlCommand();
            com.Connection = conn;
            com.CommandType = CommandType.Text;
            com.CommandText = "SELECT VCMachineID FROM metaobject where pkid = " + item.pkid + " and machine = '" + item.MachineName + "'";
            if (conn.State != ConnectionState.Open)
                conn.Open();
            string returnUser = com.ExecuteScalar() as string;
            conn.Close();
            return returnUser;
        }
        public string getCurrentUser(MetaObject item, string provider)
        {
            string constring = provider == Core.Variables.Instance.ServerProvider ? Core.Variables.Instance.ServerConnectionString : Core.Variables.Instance.ConnectionString;
            SqlConnection conn = new SqlConnection(constring);
            SqlCommand com = new SqlCommand();
            com.Connection = conn;
            com.CommandType = CommandType.Text;
            com.CommandText = "SELECT VCMachineID FROM metaobject where pkid = " + item.pkid + " and machine = '" + item.Machine + "'";
            if (conn.State != ConnectionState.Open)
                conn.Open();
            string returnUser = com.ExecuteScalar() as string;
            conn.Close();
            return returnUser;
        }
    }
}