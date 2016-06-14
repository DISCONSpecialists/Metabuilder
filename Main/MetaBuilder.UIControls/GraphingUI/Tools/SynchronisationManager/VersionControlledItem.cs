using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.BusinessLogic;
using System.ComponentModel;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.BusinessFacade.Storage.RepositoryTemp;

namespace MetaBuilder.UIControls.GraphingUI.Tools.SynchronisationManager
{
    public class VersionControlledItem
    {
        public VersionControlledItem(PermissionService perm, object client, object server, PermissionList workspacePermission)
        {
            PermService = perm;
            ClientItem = client;
            ServerItem = server;
            Permission = workspacePermission;
        }
        private string RetrieveProvider = "";

        public string ID
        {
            get
            {
                if (ClientItem != null && ServerItem != null)
                {
                    if (ClientItem.GetType() != ServerItem.GetType())
                    {
                        return "Type Mismatch";
                    }
                    else
                    {
                        if (ReturnItemToUse() is GraphFile)
                            return (ReturnItemToUse() as GraphFile).OriginalFileUniqueID.ToString();
                        if (ReturnItemToUse() is MetaObject)
                            return (ReturnItemToUse() as MetaObject).pkid + "|" + (ReturnItemToUse() as MetaObject).Machine;

                        return "Choice";
                    }
                }
                else if (ServerItem != null)
                {
                    if (ServerItem is GraphFile)
                        return (ServerItem as GraphFile).OriginalFileUniqueID.ToString();
                    if (ServerItem is MetaObject)
                        return (ServerItem as MetaObject).pkid + "|" + (ServerItem as MetaObject).Machine;

                }
                else if (ClientItem != null)
                {
                    if (ClientItem is GraphFile)
                        return (ClientItem as GraphFile).OriginalFileUniqueID.ToString();
                    if (ClientItem is MetaObject)
                        return (ClientItem as MetaObject).pkid + "|" + (ClientItem as MetaObject).Machine;
                }
                else
                {
                    return "NULL";
                }
                return "";
            }
            //set { id = value; }
        }
        private string description = "";
        public List<ObjectFieldValue> Values;
        public string Description
        {
            get
            {
                if (ClientItem != null && ServerItem != null)
                {
                    if (ClientItem.GetType() != ServerItem.GetType())
                    {
                        return "Type Mismatch";
                    }
                }
                object o = ReturnItemToUse();
                if (o is GraphFile)
                    return Core.strings.GetFileNameOnly((o as GraphFile).Name);
                if (o is MetaObject)
                {
                    if (description.Length > 0)
                        return description;
                    try
                    {
                        description = (o as MetaObject).Class + " - ";
                        Values = new List<ObjectFieldValue>();
                        foreach (ObjectFieldValue v in DataAccessLayer.DataRepository.Connections[RetrieveProvider].Provider.ObjectFieldValueProvider.GetByObjectIDMachineID((o as MetaObject).pkid, (o as MetaObject).Machine))
                        {
                            Values.Add(v);
                            if (v.ValueString.Length == 0)
                                continue;
                            if (v.ValueLongText.Length > 0 && v.ValueLongText.Length > v.ValueString.Length)
                            {
                                description += v.ValueLongText;
                            }
                            else
                            {
                                description += v.ValueString;
                            }
                        }
                        return description;
                    }
                    catch
                    {
                    }
                }

                return "NULL";
            }
            //set { description = value; }
        }

        public string Workspace
        {
            get
            {
                if (ReturnItemToUse() is GraphFile)
                    return (ReturnItemToUse() as GraphFile).WorkspaceName;
                else if (ReturnItemToUse() is MetaObject)
                    return (ReturnItemToUse() as MetaObject).WorkspaceName;
                return "";
            }
        }

        public string User
        {
            get
            {
                if (ReturnItemToUse() is GraphFile)
                    return (ReturnItemToUse() as GraphFile).VCUser;
                else if (ReturnItemToUse() is MetaObject)
                    return ((ReturnItemToUse() as MetaObject).VCMachineID != null && (ReturnItemToUse() as MetaObject).VCMachineID.Length > 0) ? (ReturnItemToUse() as MetaObject).VCMachineID : Core.strings.GetVCIdentifier();
                return "";
            }
        }

        private object clientItem;
        [Browsable(false)]
        public object ClientItem
        {
            get { return clientItem; }
            set { clientItem = value; }
        }
        private object serverItem;
        [Browsable(false)]
        public object ServerItem
        {
            get { return serverItem; }
            set { serverItem = value; }
        }
        private PermissionList permission;
        [Browsable(false)]
        public PermissionList Permission
        {
            get { return permission; }
            set { permission = value; }
        }
        private PermissionService permService;
        [Browsable(false)]
        public PermissionService PermService
        {
            get { return permService; }
            set { permService = value; }
        }

        public TList<GraphFileObject> clientFileObjects;
        public TList<GraphFileObject> serverFileObjects;

        public string ClientState
        {
            get
            {
                if (ClientItem is GraphFile)
                    return ((VCStatusList)(ClientItem as GraphFile).VCStatusID).ToString();
                if (ClientItem is MetaObject)
                    return ((VCStatusList)(ClientItem as MetaObject).VCStatusID).ToString();

                return "None";
            }
        }
        public string ServerState
        {
            get
            {
                if (ServerItem is GraphFile)
                    return ((VCStatusList)(ServerItem as GraphFile).VCStatusID).ToString();
                if (ServerItem is MetaObject)
                    return ((VCStatusList)(ServerItem as MetaObject).VCStatusID).ToString();

                return "None";
            }
        }

        //This is the only property we can set in this class
        private VCStatusList userState;
        public VCStatusList UserState
        {
            get { return userState; }
            set { userState = value; }
        }

        //Apply rules to get allowed states
        public List<RepositoryAction> AllowedStates
        {
            get
            {
                return SynchronisationRules.GetAvailableActions(ClientState, ServerState, Permission, VCUserIsTheSame, RetrieveProvider);
            }
        }
        public bool VCUserIsTheSame
        {
            get
            {
                if (ServerItem == null || ClientItem == null)
                    return false;

                if (ReturnItemToUse() is GraphFile)
                    return (ServerItem as GraphFile).VCUser == (ClientItem as GraphFile).VCUser;
                else if (ReturnItemToUse() is MetaObject)
                    return (ServerItem as MetaObject).VCMachineID == (ClientItem as MetaObject).VCMachineID;

                return false;
            }
        }

        public object ReturnItemToUse(string provider)
        {
            RetrieveProvider = provider;
            if (RetrieveProvider == Core.Variables.Instance.ClientProvider)
                return ClientItem;

            if (RetrieveProvider == Core.Variables.Instance.ServerProvider)
                return ServerItem;

            return null;
        }
        public object ReturnItemToUse()
        {
            if (ServerItem == null && ClientItem != null)
            {
                RetrieveProvider = Core.Variables.Instance.ClientProvider; //ClientItem should be in the state of None
                ItemType = ClientItem.GetType();
                return ClientItem;
            }

            if (ServerItem != null && ClientItem != null)
            {
                ItemType = ServerItem.GetType();
                //return the newest item
                if (ServerItem is GraphFile && ClientItem is GraphFile)
                {
                    double serverVersion = double.Parse((ServerItem as GraphFile).MajorVersion.ToString() + "." + (ServerItem as GraphFile).MinorVersion.ToString());
                    double clientVersion = double.Parse((ClientItem as GraphFile).MajorVersion.ToString() + "." + (ClientItem as GraphFile).MinorVersion.ToString());
                    //Use Diagram Version?
                    if (((ServerItem as GraphFile).ModifiedDate > (ClientItem as GraphFile).ModifiedDate) && serverVersion > clientVersion)
                    {
                        RetrieveProvider = Core.Variables.Instance.ServerProvider;
                        return ServerItem;
                    }
                    else
                    {
                        RetrieveProvider = Core.Variables.Instance.ClientProvider;
                        return ClientItem;
                    }
                }
                else if (ServerItem is MetaObject && ClientItem is MetaObject)
                {
                    if ((ServerItem as MetaObject).LastModified > (ClientItem as MetaObject).LastModified)
                    {
                        RetrieveProvider = Core.Variables.Instance.ServerProvider;
                        return ServerItem;
                    }
                    else
                    {
                        RetrieveProvider = Core.Variables.Instance.ClientProvider;
                        return ClientItem;
                    }
                }
            }
            else
            {
                ItemType = ServerItem != null ? ServerItem.GetType() : ClientItem != null ? ClientItem.GetType() : null;
            }
            RetrieveProvider = Core.Variables.Instance.ServerProvider;
            return ServerItem;
        }

        private Type itemType;
        public Type ItemType
        {
            get { return itemType; }
            set { itemType = value; }
        }
        //Executes changes to both items(where necessary) in seperate threads towards UserState
        public void Execute()
        {
        }

        public bool CancelExecution
        {
            get
            {
                bool cancelExecution = false;
                object o = ReturnItemToUse();
                if (o == null)
                {
                    CancelExecutionReason = "Cannot find object to use";
                    return true;
                }
                cancelExecution = (o is GraphFile) ? (o as GraphFile).WorkspaceTypeId != 3 : (o as MetaObject).WorkspaceTypeId != 3;
                if (cancelExecution)
                    CancelExecutionReason = "Workspace(" + Workspace + ") is not a server workspace";
                return cancelExecution;
            }

        }
        private string cancelExecutionReason;
        public string CancelExecutionReason
        {
            get { return cancelExecutionReason; }
            set { cancelExecutionReason = value; }
        }

        public void LoadEmbedded()
        {
            DataAccessLayer.OldCode.Meta.ObjectAdapter objAd = new MetaBuilder.DataAccessLayer.OldCode.Meta.ObjectAdapter();

            if (EmbeddedObjects.Count > 0) //must have already run
                return;

            object parent = ReturnItemToUse();
            if (parent is GraphFile)
            {
                foreach (MetaObject obj in objAd.GetObjectsByGraphFileIDMachine((parent as GraphFile).pkid, (parent as GraphFile).Machine, (RetrieveProvider == Core.Variables.Instance.ClientProvider ? Core.Variables.Instance.ConnectionString : Core.Variables.Instance.ServerConnectionString)))//DataRepository.Connections[RetrieveProvider].Provider.MetaObjectProvider.GetByGraphFileIDGraphFileMachineFromGraphFileObject((parent as GraphFile).pkid, (parent as GraphFile).Machine))
                {
                    //if (obj.WorkspaceTypeId != 3)
                    //{
                    //    CancelExecutionReason = "Workspace(" + obj.WorkspaceName + ") is not a server workspace";
                    //    CancelExecution = true;
                    //    return;
                    //}
                    //if retrieiving from client the clientObject is actually serverobject
                    object clientObject = null;
                    try
                    {
                        //opposite of retrieve provider
                        clientObject = objAd.GetObjectByPkidMachine(obj.pkid, obj.Machine, (RetrieveProvider == Core.Variables.Instance.ClientProvider ? Core.Variables.Instance.ServerConnectionString : Core.Variables.Instance.ConnectionString));
                        //clientObject = DataRepository.Connections[].Provider.MetaObjectProvider.GetBypkidMachine();
                        //if (clientObject != null)
                        //    usedClient.Add(clientObject);
                    }
                    catch
                    {
                    }
                    VersionControlledItem embeddedItem = null;
                    if (RetrieveProvider == Core.Variables.Instance.ServerProvider)
                    {
                        embeddedItem = new VersionControlledItem(PermService, clientObject, obj, SynchronisationRules.GetPermissionForItem(PermService, obj));
                        //serverFileObjects = DataRepository.Connections[RetrieveProvider].Provider.GraphFileObjectProvider.GetByGraphFileIDGraphFileMachine((parent as GraphFile).pkid, (parent as GraphFile).Machine);
                    }
                    else if (RetrieveProvider == Core.Variables.Instance.ClientProvider)
                    {
                        embeddedItem = new VersionControlledItem(PermService, obj, clientObject, SynchronisationRules.GetPermissionForItem(PermService, clientObject));
                        //serverFileObjects = DataRepository.Connections[RetrieveProvider].Provider.GraphFileObjectProvider.GetByGraphFileIDGraphFileMachine((parent as GraphFile).pkid, (parent as GraphFile).Machine);
                    }
                    else
                    {
                        RetrieveProvider.ToString();
                    }
                    if (embeddedItem != null)
                    {
                        embeddedItem.LoadEmbedded(); //calls elseif below
                        EmbeddedObjects.Add(embeddedItem);
                        foreach (VersionControlledItem subEmbeddedItem in embeddedItem.EmbeddedObjects) //Table -- column,column,column --> next object parented to file
                        {
                            EmbeddedObjects.Add(subEmbeddedItem);
                        }
                    }
                }
                //if (RetrieveProvider == Core.Variables.Instance.ServerProvider)
                //{
                //    serverFileObjects = DataRepository.Connections[RetrieveProvider].Provider.GraphFileObjectProvider.GetByGraphFileIDGraphFileMachine((parent as GraphFile).pkid, (parent as GraphFile).Machine);
                //}
                //else if (RetrieveProvider == Core.Variables.Instance.ClientProvider)
                //{
                //    clientFileObjects = DataRepository.Connections[RetrieveProvider].Provider.GraphFileObjectProvider.GetByGraphFileIDGraphFileMachine((parent as GraphFile).pkid, (parent as GraphFile).Machine);
                //}
            }
            else if (parent is MetaObject)
            {
                MetaObject obj = parent as MetaObject;
                if (obj.Class == "DataTable" || obj.Class == "DataEntity" || obj.Class == "DataSubjectArea" || obj.Class == "PhysicalInformationArtefact" || obj.Class == "LogicalInformationArtefact" || obj.Class == "DataSchema" || obj.Class == "DataDomain")
                {
                    //load embedded children
                    foreach (MetaObject childobj in DataRepository.Connections[RetrieveProvider].Provider.MetaObjectProvider.GetByObjectIDObjectMachineFromObjectAssociation(obj.pkid, obj.Machine))
                    {
                        object clientObject = null;
                        try
                        {
                            //opposite of retrieve provider
                            clientObject = objAd.GetObjectByPkidMachine(childobj.pkid, childobj.Machine, (RetrieveProvider == Core.Variables.Instance.ClientProvider ? Core.Variables.Instance.ServerConnectionString : Core.Variables.Instance.ConnectionString));
                            //clientObject = DataRepository.Connections[(RetrieveProvider == Core.Variables.Instance.ClientProvider ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider)].Provider.MetaObjectProvider.GetBypkidMachine(childobj.pkid, childobj.Machine);
                            //if (clientObject != null)
                            //    usedClient.Add(clientObject);
                        }
                        catch
                        {
                        }
                        if (childobj.Class == "DataAttribute" || childobj.Class == "DataField" || childobj.Class == "DataValue")
                        {
                            VersionControlledItem embeddedItem = null;
                            if (RetrieveProvider == Core.Variables.Instance.ServerProvider)
                                embeddedItem = new VersionControlledItem(PermService, clientObject, childobj, SynchronisationRules.GetPermissionForItem(PermService, childobj));
                            else if (RetrieveProvider == Core.Variables.Instance.ClientProvider)
                                embeddedItem = new VersionControlledItem(PermService, childobj, clientObject, SynchronisationRules.GetPermissionForItem(PermService, childobj));
                            else
                            {
                                RetrieveProvider.ToString();
                            }
                            if (embeddedItem != null)
                            {
                                //= new VersionControlledItem(PermService, clientObject, childobj, SynchronisationRules.GetPermissionForItem(PermService, childobj));
                                //embeddedItem.LoadEmbedded(); //calls elseif below this may cause infinite loop :)
                                EmbeddedObjects.Add(embeddedItem);
                            }
                        }
                    }
                }
            }
            else
            {
                //what could it possibly be?
                parent.ToString();
            }
        }

        private List<VersionControlledItem> embeddedObjects;
        [Browsable(false)]
        public List<VersionControlledItem> EmbeddedObjects
        {
            get
            {
                if (embeddedObjects == null)
                    embeddedObjects = new List<VersionControlledItem>();
                return embeddedObjects;
            }
            set { embeddedObjects = value; }
        }

        private List<RepositoryAction> embeddedObjectActions;
        [Browsable(false)]
        public List<RepositoryAction> EmbeddedObjectActions
        {
            get
            {
                if (embeddedObjectActions == null)
                    embeddedObjectActions = new List<RepositoryAction>();
                return embeddedObjectActions;
            }
            set { embeddedObjectActions = value; }
        }

    }
}
