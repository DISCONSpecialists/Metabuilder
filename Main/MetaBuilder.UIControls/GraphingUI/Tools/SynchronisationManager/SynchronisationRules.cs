using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.BusinessFacade.Storage.RepositoryTemp;
using MetaBuilder.BusinessLogic;

namespace MetaBuilder.UIControls.GraphingUI.Tools.SynchronisationManager
{
    public static class SynchronisationRules
    {
        private static List<RepositoryAction> rules;
        private static List<RepositoryAction> Rules
        {
            get
            {
                if (rules == null)
                {
                    rules = new List<RepositoryAction>();
                    ReadRules();
                }
                return rules;
            }
        }
        private static void ReadRules()
        {
            //Client Open/checkin
            Rules.Add(new RepositoryAction(101, Core.Variables.Instance.ClientProvider, VCStatusList.None, VCStatusList.Ignore, AllowedUserType.Any, true, true, true, "Open", VCStatusList.Ignore, VCStatusList.Ignore));
            Rules.Add(new RepositoryAction(102, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedIn, VCStatusList.Ignore, AllowedUserType.Any, true, true, true, "Open", VCStatusList.Ignore, VCStatusList.Ignore));
            Rules.Add(new RepositoryAction(103, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedOut, VCStatusList.Ignore, AllowedUserType.Same, true, true, true, "Open", VCStatusList.Ignore, VCStatusList.Ignore));
            Rules.Add(new RepositoryAction(104, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedOutRead, VCStatusList.Ignore, AllowedUserType.Any, true, true, true, "Open", VCStatusList.Ignore, VCStatusList.Ignore));
            Rules.Add(new RepositoryAction(105, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedOutRead, VCStatusList.CheckedIn, AllowedUserType.Any, false, true, true, "Check Out", VCStatusList.CheckedOut, VCStatusList.CheckedOut));
            Rules.Add(new RepositoryAction(106, Core.Variables.Instance.ClientProvider, VCStatusList.None, VCStatusList.None, AllowedUserType.Any, false, true, true, "Check In", VCStatusList.CheckedIn, VCStatusList.CheckedIn));
            Rules.Add(new RepositoryAction(107, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedOut, VCStatusList.CheckedOut, AllowedUserType.Same, false, true, true, "Check In", VCStatusList.CheckedIn, VCStatusList.CheckedIn));
            Rules.Add(new RepositoryAction(108, Core.Variables.Instance.ClientProvider, VCStatusList.MarkedForDelete, VCStatusList.CheckedOut, AllowedUserType.Same, false, true, true, "Check In (MFD)", VCStatusList.CheckedOutRead, VCStatusList.MarkedForDelete));
            Rules.Add(new RepositoryAction(109, Core.Variables.Instance.ClientProvider, VCStatusList.Ignore, VCStatusList.Locked, AllowedUserType.Any, true, true, true, "Check In", VCStatusList.CheckedOutRead, VCStatusList.Ignore));

            Rules.Add(new RepositoryAction(110, Core.Variables.Instance.ClientProvider, VCStatusList.Ignore, VCStatusList.CheckedOut, AllowedUserType.Any, false, false, true, "Force Check In", VCStatusList.Ignore, VCStatusList.CheckedIn));

            //Server
            Rules.Add(new RepositoryAction(201, Core.Variables.Instance.ServerProvider, VCStatusList.None, VCStatusList.None, AllowedUserType.Any, false, true, true, "IGNORE", VCStatusList.Ignore, VCStatusList.Ignore)); //this rule should never appear
            Rules.Add(new RepositoryAction(202, Core.Variables.Instance.ServerProvider, VCStatusList.Ignore, VCStatusList.CheckedIn, AllowedUserType.Any, false, true, true, "Check Out", VCStatusList.CheckedOut, VCStatusList.CheckedOut));
            //Rules.Add(new RepositoryAction(203, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedIn, VCStatusList.CheckedIn, AllowedUserType.Any, false, true, true, "Check Out", VCStatusList.CheckedOut, VCStatusList.CheckedOut));
            Rules.Add(new RepositoryAction(204, Core.Variables.Instance.ServerProvider, VCStatusList.Ignore, VCStatusList.CheckedIn, AllowedUserType.Any, false, false, true, "Lock", VCStatusList.Ignore, VCStatusList.Locked));
            Rules.Add(new RepositoryAction(205, Core.Variables.Instance.ServerProvider, VCStatusList.Ignore, VCStatusList.Locked, AllowedUserType.Any, false, false, true, "Un-Lock", VCStatusList.Ignore, VCStatusList.CheckedIn));
            Rules.Add(new RepositoryAction(206, Core.Variables.Instance.ServerProvider, VCStatusList.Ignore, VCStatusList.CheckedOut, AllowedUserType.Any, false, false, true, "Force Check In", VCStatusList.Ignore, VCStatusList.CheckedIn));
            Rules.Add(new RepositoryAction(207, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedOut, VCStatusList.CheckedOut, AllowedUserType.Different, true, true, true, "Check Out Read", VCStatusList.CheckedOutRead, VCStatusList.Ignore));
            Rules.Add(new RepositoryAction(208, Core.Variables.Instance.ServerProvider, VCStatusList.Ignore, VCStatusList.CheckedOut, AllowedUserType.Any, true, true, true, "Check Out Read", VCStatusList.CheckedOutRead, VCStatusList.Ignore));
            Rules.Add(new RepositoryAction(209, Core.Variables.Instance.ServerProvider, VCStatusList.Ignore, VCStatusList.CheckedIn, AllowedUserType.Any, true, true, true, "Check Out Read", VCStatusList.CheckedOutRead, VCStatusList.Ignore));
            Rules.Add(new RepositoryAction(210, Core.Variables.Instance.ServerProvider, VCStatusList.Ignore, VCStatusList.MarkedForDelete, AllowedUserType.Any, true, true, true, "Check Out Read", VCStatusList.CheckedOutRead, VCStatusList.Ignore));
            Rules.Add(new RepositoryAction(211, Core.Variables.Instance.ServerProvider, VCStatusList.Ignore, VCStatusList.Locked, AllowedUserType.Any, true, true, true, "Check Out Read", VCStatusList.CheckedOutRead, VCStatusList.Ignore));
            Rules.Add(new RepositoryAction(212, Core.Variables.Instance.ServerProvider, VCStatusList.Ignore, VCStatusList.MarkedForDelete, AllowedUserType.Any, false, false, true, "Delete", VCStatusList.Ignore, VCStatusList.Deleted));

            #region OLD
            //#region Client Rules

            //Rules.Add(new RepositoryAction(101, Core.Variables.Instance.ClientProvider, VCStatusList.PCI_Revoked, VCStatusList.PartiallyCheckedIn, true, true, false, true, true, null, "Check In (Overwrite)", VCStatusList.PartiallyCheckedIn, VCStatusList.PartiallyCheckedIn, false));
            //Rules.Add(new RepositoryAction(102, Core.Variables.Instance.ClientProvider, VCStatusList.None, VCStatusList.CheckedIn, true, true, false, false, true, null, "Check In", VCStatusList.CheckedIn, VCStatusList.CheckedIn, true));
            //Rules.Add(new RepositoryAction(103, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedOut, VCStatusList.CheckedIn, true, true, false, true, true, null, "Check In", VCStatusList.CheckedIn, VCStatusList.CheckedIn, true));
            //Rules.Add(new RepositoryAction(104, Core.Variables.Instance.ClientProvider, VCStatusList.MarkedForDelete, VCStatusList.CheckedIn, true, true, false, false, true, null, "Check In", VCStatusList.MarkedForDelete, VCStatusList.MarkedForDelete, true));
            //Rules.Add(new RepositoryAction(105, Core.Variables.Instance.ClientProvider, VCStatusList.MarkedForDelete, VCStatusList.CheckedIn, true, true, false, false, true, null, "", VCStatusList.MarkedForDelete, VCStatusList.MarkedForDelete, true));
            //Rules.Add(new RepositoryAction(106, Core.Variables.Instance.ClientProvider, VCStatusList.Locked, VCStatusList.CheckedIn, true, true, false, false, true, null, "", VCStatusList.Locked, VCStatusList.CheckedIn, false));
            //Rules.Add(new RepositoryAction(107, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedIn, VCStatusList.CheckedIn, true, true, true, false, true, null, "", VCStatusList.CheckedIn, VCStatusList.CheckedIn, false)); //Objects that are checked in must still be checkedin with the diagram (2 july 2015 : bool READ false-->true)
            ////OPEN
            ////Rules.Add(new RepositoryAction(501, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedIn, VCStatusList.CheckedIn, false, true, true, true, false, false, true,null, "Open", VCStatusList.CheckedIn, VCStatusList.CheckedIn));
            //Rules.Add(new RepositoryAction(502, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedOut, VCStatusList.CheckedOut, true, true, true, false, true, null, "Open", VCStatusList.None, VCStatusList.None, false));
            //Rules.Add(new RepositoryAction(503, Core.Variables.Instance.ClientProvider, VCStatusList.PartiallyCheckedIn, VCStatusList.PartiallyCheckedIn, true, true, true, false, true, null, "Open", VCStatusList.None, VCStatusList.None, false));
            //Rules.Add(new RepositoryAction(504, Core.Variables.Instance.ClientProvider, VCStatusList.None, VCStatusList.None, true, true, true, false, true, null, "Open", VCStatusList.None, VCStatusList.None, false));
            //Rules.Add(new RepositoryAction(505, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedOut, VCStatusList.None, true, true, true, false, true, null, "", VCStatusList.None, VCStatusList.CheckedOut, false));

            ////opening of readonly/locked/obsolete items allowed
            //Rules.Add(new RepositoryAction(515, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedOutRead, VCStatusList.CheckedOutRead, true, true, true, false, false, null, "Open", VCStatusList.None, VCStatusList.None, false));
            ////Rules.Add(new RepositoryAction(516, Core.Variables.Instance.ClientProvider, VCStatusList.Locked, VCStatusList.Locked, false, false, true, true, true, false, false,null, "Open", VCStatusList.Locked, VCStatusList.Locked));
            ////Rules.Add(new RepositoryAction(517, Core.Variables.Instance.ClientProvider, VCStatusList.Obsolete, VCStatusList.Obsolete, false, false, true, true, true, false, false,null, "Open", VCStatusList.Obsolete, VCStatusList.Obsolete));
            ////opening of readonly/locked/obsolete items allowed
            ////Rules.Add(new RepositoryAction(331, Core.Variables.Instance.ClientProvider, VCStatusList.MarkedForDelete, VCStatusList.Deleted, true, true, true, false, false, false, true,null, "Delete", VCStatusList.MarkedForDelete, VCStatusList.Deleted, false));


            //Rules.Add(new RepositoryAction(108, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedOutRead, VCStatusList.CheckedIn, true, true, true, false, true, null, "", VCStatusList.None, VCStatusList.CheckedIn, false, false)); //checking in a diagram with an object that is checked out read
            //Rules.Add(new RepositoryAction(109, Core.Variables.Instance.ClientProvider, VCStatusList.Locked, VCStatusList.CheckedOut, true, true, true, false, true, null, "", VCStatusList.None, VCStatusList.None, false));

            //Rules.Add(new RepositoryAction(110, Core.Variables.Instance.ClientProvider, VCStatusList.None, VCStatusList.CheckedOut, true, true, true, false, true, null, "", VCStatusList.None, VCStatusList.None, true));
            //Rules.Add(new RepositoryAction(111, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedIn, VCStatusList.CheckedOut, true, true, true, false, true, null, "", VCStatusList.None, VCStatusList.None, true));
            //Rules.Add(new RepositoryAction(112, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedOutRead, VCStatusList.CheckedOut, true, true, true, false, true, null, "", VCStatusList.None, VCStatusList.None, false));
            //Rules.Add(new RepositoryAction(113, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedIn, VCStatusList.None, true, true, true, false, true, null, "", VCStatusList.None, VCStatusList.None, true));
            //Rules.Add(new RepositoryAction(114, Core.Variables.Instance.ClientProvider, VCStatusList.MarkedForDelete, VCStatusList.CheckedOut, true, true, true, false, true, null, "", VCStatusList.None, VCStatusList.None, false));
            //Rules.Add(new RepositoryAction(115, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedOutRead, VCStatusList.CheckedIn, true, true, true, false, true, null, "", VCStatusList.None, VCStatusList.CheckedIn, false));

            //Rules.Add(new RepositoryAction(116, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedOut, VCStatusList.CheckedIn, true, true, true, false, true, null, "", VCStatusList.None, VCStatusList.CheckedOut, false));

            //Rules.Add(new RepositoryAction(117, Core.Variables.Instance.ClientProvider, VCStatusList.Locked, VCStatusList.CheckedIn, false, false, true, false, true, null, "", VCStatusList.Locked, VCStatusList.CheckedIn, false));

            //Rules.Add(new RepositoryAction(118, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedOutRead, VCStatusList.None, false, false, true, false, true, null, "", VCStatusList.None, VCStatusList.None, false));

            //#endregion

            //#region Server Rules

            //Rules.Add(new RepositoryAction(201, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedIn, VCStatusList.CheckedOut, true, true, false, false, false, null, "Check Out", VCStatusList.CheckedOut, VCStatusList.CheckedOut, true));
            //Rules.Add(new RepositoryAction(202, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedIn, VCStatusList.CheckedOutRead, true, true, true, false, false, null, "Check Out (Read)", VCStatusList.None, VCStatusList.CheckedOutRead, false));//, true));
            //Rules.Add(new RepositoryAction(203, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedOut, VCStatusList.CheckedOutRead, true, true, true, false, false, null, "Check Out (Read)", VCStatusList.None, VCStatusList.CheckedOutRead, false, true));
            //Rules.Add(new RepositoryAction(204, Core.Variables.Instance.ServerProvider, VCStatusList.Locked, VCStatusList.CheckedOutRead, true, true, true, false, false, null, "Check Out (Read)", VCStatusList.None, VCStatusList.Locked, false));
            //Rules.Add(new RepositoryAction(205, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedIn, VCStatusList.Locked, true, false, false, false, true, null, "Lock", VCStatusList.Locked, VCStatusList.None, true)); //Made rule cascade
            ////Rules.Add(new RepositoryAction(206, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedOut, VCStatusList.Locked, true, false, true, false, false, false, false,null, "Lock", VCStatusList.Locked, VCStatusList.None));
            ////Rules.Add(new RepositoryAction(207, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedIn, VCStatusList.Obsolete, true, true, true, false, false, false, true,null, "", VCStatusList.Obsolete, VCStatusList.None));
            ////Rules.Add(new RepositoryAction(208, Core.Variables.Instance.ServerProvider, VCStatusList.Locked, VCStatusList.Obsolete, true, false, true, false, false, false, false,null, "Mark Obsolete", VCStatusList.Obsolete, VCStatusList.None));
            ////Rules.Add(new RepositoryAction(209, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedOut, VCStatusList.Obsolete, true, false, true, false, false, false, false,null, "Mark Obsolete", VCStatusList.Obsolete, VCStatusList.None));
            //Rules.Add(new RepositoryAction(210, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedOut, VCStatusList.CheckedIn, true, false, false, false, false, null, "Force Check In", VCStatusList.CheckedIn, VCStatusList.None, true));
            //Rules.Add(new RepositoryAction(211, Core.Variables.Instance.ServerProvider, VCStatusList.Locked, VCStatusList.CheckedIn, true, false, false, false, true, null, "Unlock", VCStatusList.CheckedIn, VCStatusList.None, true)); //THIS IS THE LOOPHOLE RULE //Made rule cascade
            ////Rules.Add(new RepositoryAction(212, Core.Variables.Instance.ServerProvider, VCStatusList.Obsolete, VCStatusList.CheckedIn, true, false, true, false, false, false, true,null, "Restore (Check In)", VCStatusList.CheckedIn, VCStatusList.None));
            //Rules.Add(new RepositoryAction(213, Core.Variables.Instance.ServerProvider, VCStatusList.MarkedForDelete, VCStatusList.CheckedIn, true, true, false, false, true, null, "Restore (Check In)", VCStatusList.CheckedIn, VCStatusList.None, true));
            ////Rules.Add(new RepositoryAction(214, Core.Variables.Instance.ServerProvider, VCStatusList.MarkedForDelete, VCStatusList.Obsolete, true, false, true, false, false, false, false,null, "Mark Obsolete", VCStatusList.Obsolete, VCStatusList.None));
            //Rules.Add(new RepositoryAction(215, Core.Variables.Instance.ServerProvider, VCStatusList.Locked, VCStatusList.CheckedOut, true, true, true, false, true, null, "", VCStatusList.Locked, VCStatusList.Locked, false));
            ////Rules.Add(new RepositoryAction(216, Core.Variables.Instance.ServerProvider, VCStatusList.Obsolete, VCStatusList.CheckedOut, true, true, true, true, true, false, false,null, "Check Out (Read)", VCStatusList.Obsolete, VCStatusList.Obsolete));
            //Rules.Add(new RepositoryAction(217, Core.Variables.Instance.ServerProvider, VCStatusList.PartiallyCheckedIn, VCStatusList.CheckedIn, true, false, false, false, false, null, "Check In", VCStatusList.CheckedIn, VCStatusList.None, true));
            //Rules.Add(new RepositoryAction(218, Core.Variables.Instance.ServerProvider, VCStatusList.PartiallyCheckedIn, VCStatusList.CheckedOutRead, true, false, false, false, false, null, "Check Out (Read)", VCStatusList.None, VCStatusList.CheckedOutRead, false));//, true));
            //Rules.Add(new RepositoryAction(219, Core.Variables.Instance.ServerProvider, VCStatusList.PartiallyCheckedIn, VCStatusList.PCI_Revoked, true, true, false, true, true, null, "Check Out (Cancel PCI)", VCStatusList.PCI_Revoked, VCStatusList.PCI_Revoked, true));
            ////Rules.Add(new RepositoryAction(220, Core.Variables.Instance.ServerProvider, VCStatusList.PartiallyCheckedIn, VCStatusList.PCI_Revoked, true, true, true, false, false, false, true,null, "Check Out (Cancel PCI)", VCStatusList.PCI_Revoked, VCStatusList.PCI_Revoked,true));
            //Rules.Add(new RepositoryAction(221, Core.Variables.Instance.ServerProvider, VCStatusList.MarkedForDelete, VCStatusList.CheckedOut, true, true, true, false, false, null, "", VCStatusList.None, VCStatusList.MarkedForDelete, false));
            //Rules.Add(new RepositoryAction(222, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedOut, VCStatusList.CheckedOut, true, true, false, false, true, null, "", VCStatusList.None, VCStatusList.CheckedOutRead, false));
            //Rules.Add(new RepositoryAction(223, Core.Variables.Instance.ServerProvider, VCStatusList.MarkedForDelete, VCStatusList.CheckedOutRead, true, true, true, false, false, null, "", VCStatusList.None, VCStatusList.MarkedForDelete, false)); //Exec on server to preserve file association
            ////Rules.Add(new RepositoryAction(224, Core.Variables.Instance.ServerProvider, VCStatusList.Obsolete, VCStatusList.CheckedOutRead, false, true, true, true, true, true, false,null, "", VCStatusList.None, VCStatusList.Obsolete)); //Exec on server to preserve file association

            ////DELETE
            ////Rules.Add(new RepositoryAction(330, Core.Variables.Instance.ServerProvider, VCStatusList.Obsolete, VCStatusList.Deleted, true, true, true, false, false, false, true,null, "Delete", VCStatusList.Obsolete, VCStatusList.Deleted, false));
            ////Rules.Add(new RepositoryAction(331, Core.Variables.Instance.ClientProvider, VCStatusList.Obsolete, VCStatusList.Deleted, true, true, true, false, false, false, true,null, "Delete", VCStatusList.Obsolete, VCStatusList.Deleted, false));
            //Rules.Add(new RepositoryAction(330, Core.Variables.Instance.ServerProvider, VCStatusList.MarkedForDelete, VCStatusList.Deleted, true, false, false, false, true, null, "Delete", VCStatusList.MarkedForDelete, VCStatusList.Deleted, true, false));


            //Rules.Add(new RepositoryAction(224, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedIn, VCStatusList.CheckedIn, true, true, true, false, false, null, "", VCStatusList.None, VCStatusList.None, false, false)); // When we lock at a diagram level and objects are already checkedin
            //Rules.Add(new RepositoryAction(225, Core.Variables.Instance.ServerProvider, VCStatusList.Locked, VCStatusList.Locked, true, true, true, false, false, null, "", VCStatusList.None, VCStatusList.None, false, false)); // Locking a file which already has locked objects
            //Rules.Add(new RepositoryAction(226, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedOut, VCStatusList.Locked, true, true, true, false, false, null, "", VCStatusList.None, VCStatusList.None, false, false)); // Locking a file where objects are checked out (unless it is checked out to you and you are admin?)
            //Rules.Add(new RepositoryAction(227, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedOut, VCStatusList.CheckedOut, true, true, false, true, true, null, "", VCStatusList.None, VCStatusList.CheckedOut, true));

            //Rules.Add(new RepositoryAction(228, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedIn, VCStatusList.CheckedOut, false, false, true, false, true, null, "", VCStatusList.None, VCStatusList.CheckedOutRead, false));

            //Rules.Add(new RepositoryAction(229, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedIn, VCStatusList.MarkedForDelete, true, false, false, false, true, null, "Mark For Delete", VCStatusList.MarkedForDelete, VCStatusList.None, true));

            //Rules.Add(new RepositoryAction(230, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedOut, VCStatusList.CheckedOutRead, true, true, true, true, false, null, "Check Out (Read)", VCStatusList.None, VCStatusList.None, false, false));

            //#endregion
            #endregion
        }

        public static List<RepositoryAction> GetAvailableActions(string clientState, string serverState, PermissionList permission, bool sameUser, string provider)
        {
            List<RepositoryAction> availableActions = new List<RepositoryAction>();

            foreach (RepositoryAction rule in Rules)
            {
                if (rule.Provider != provider)
                    continue;

                if (rule.ClientState != VCStatusList.Ignore && rule.ClientState.ToString() != clientState)
                    continue;
                if (rule.ServerState != VCStatusList.Ignore && rule.ServerState.ToString() != serverState)
                    continue;

                if (rule.AllowedUsers == AllowedUserType.Same && !sameUser) //checking in after checkout
                    continue;
                if (rule.AllowedUsers == AllowedUserType.Different && sameUser) //What is this even used for?
                    continue;

                bool allowed = false;
                if (rule.AllowRead)
                {
                    if (permission == PermissionList.Read || permission == PermissionList.Write || permission == PermissionList.Delete)
                    {
                        allowed = true;
                    }
                }

                if (rule.AllowWrite)
                {
                    if (permission == PermissionList.Write || permission == PermissionList.Delete)
                    {
                        allowed = true;
                    }
                }

                if (rule.AllowDelete)
                {
                    if (permission == PermissionList.Delete)
                    {
                        allowed = true;
                    }
                }

                if (allowed)
                    if (!availableActions.Contains(rule))
                        availableActions.Add(rule);
            }

            return availableActions;
        }
        public static RepositoryAction GetAvailableActionForEmbeddedItem(VersionControlledItem embeddedItem, RepositoryAction parentAction)
        {
            //run the same rules but with more scrutiny ie:return the correct one only
            object objectToUse = embeddedItem.ReturnItemToUse();
            PermissionList embeddedPermission = GetPermissionForItem(embeddedItem.PermService, objectToUse);
            List<RepositoryAction> availableActions = GetAvailableActions(embeddedItem.ClientState.ToString(), embeddedItem.ServerState.ToString(), embeddedPermission, embeddedItem.User == Core.strings.GetVCIdentifier(), parentAction.Provider);

            RepositoryAction actionFound = null;
            foreach (RepositoryAction allowedAction in availableActions)
            {
                if (allowedAction.Number == parentAction.Number)
                    return parentAction; //runs the same rule (this will happen in most cases)

                if (allowedAction.TargetClientState == parentAction.TargetClientState)
                    if (allowedAction.TargetServerState == parentAction.TargetServerState)
                        return allowedAction;

                if (allowedAction.Caption == parentAction.Caption)
                    return allowedAction;

                //DEBUGGING FUN
                actionFound = allowedAction;
            }
            //this is bad
            return actionFound;
        }

        public static PermissionList GetPermissionForItem(PermissionService PermService, object item)
        {
            string workspacename = item is GraphFile ? (item as GraphFile).WorkspaceName : item is MetaObject ? (item as MetaObject).WorkspaceName : "";
            PermissionList permission = PermissionList.None;
            foreach (UserPermission perm in PermService.RemotePermissions.FindAll(UserPermissionColumn.WorkspaceName, workspacename))
            {
                if (((PermissionList)perm.PermissionID).ToString() == "Admin") //the admin permission is not relevant here
                    continue;
                permission = (PermissionList)perm.PermissionID;
            }

            return permission;
        }

    }

    public enum AllowedUserType
    {
        Same = 0, Different = 1, Any = 2
    }

    public class RepositoryAction
    {
        public RepositoryAction(int number, string provider, VCStatusList clientState, VCStatusList serverState, AllowedUserType allowedUser, bool allowRead, bool allowWrite, bool allowDelete, string caption, VCStatusList targetClientState, VCStatusList targetServerState)
        {
            this.number = number;
            this.provider = provider;
            this.clientState = clientState;
            this.serverState = serverState;
            this.allowedUsers = allowedUser;
            this.allowRead = allowRead;
            this.allowWrite = allowWrite;
            this.allowDelete = allowDelete;
            //this.targetState = targetState;, VCStatusList targetState
            this.caption = caption;

            //these are the two that will be used when changing data states
            this.targetServerState = targetServerState;
            this.targetClientState = targetClientState;
        }

        private string provider;
        public string Provider
        {
            get { return provider; }
            set { provider = value; }
        }

        private VCStatusList clientState;
        public VCStatusList ClientState
        {
            get { return clientState; }
            set { clientState = value; }
        }
        private VCStatusList serverState;
        public VCStatusList ServerState
        {
            get { return serverState; }
            set { serverState = value; }
        }

        private AllowedUserType allowedUsers;
        public AllowedUserType AllowedUsers
        {
            get { return allowedUsers; }
            set { allowedUsers = value; }
        }

        private bool allowRead;
        public bool AllowRead
        {
            get { return allowRead; }
            set { allowRead = value; }
        }

        private bool allowWrite;
        public bool AllowWrite
        {
            get { return allowWrite; }
            set { allowWrite = value; }
        }

        private bool allowDelete;
        public bool AllowDelete
        {
            get { return allowDelete; }
            set { allowDelete = value; }
        }

        //AllowAdmin is a different type of permission

        private int number;
        public int Number
        {
            get { return number; }
            set { number = value; }
        }

        private string caption;
        public string Caption
        {
            get { return caption; }
            set { caption = value; }
        }

        private VCStatusList targetState;
        public VCStatusList TargetState
        {
            get { return targetState; }
            set { targetState = value; }
        }

        private VCStatusList targetServerState;
        public VCStatusList TargetServerState
        {
            get { return targetServerState; }
            set { targetServerState = value; }
        }
        private VCStatusList targetClientState;
        public VCStatusList TargetClientState
        {
            get { return targetClientState; }
            set { targetClientState = value; }
        }

        public override string ToString()
        {
            return Caption;
        }

        public bool ChangeUser
        {
            get
            {
                return TargetServerState == VCStatusList.CheckedIn || TargetServerState == VCStatusList.Locked || TargetServerState == VCStatusList.CheckedOut || TargetServerState == VCStatusList.MarkedForDelete;
            }
        }

        public bool OpenAfterExecute
        {
            get
            {
                return Caption == "Check Out Read" || Caption == "Check Out" || Caption == "Open";

                return false; //dont open objects
            }
        }
    }
}