using System.Collections.Generic;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using b = MetaBuilder.BusinessLogic;
using System.Collections.ObjectModel;

namespace MetaBuilder.BusinessFacade.Storage.RepositoryTemp
{
    public class RepositoryService : IRepositoryService
    {
        private List<ActionResult> results;
        public List<ActionResult> Results
        {
            get { return results; }
        }

        private IPermissionService permissionSVC;
        public IPermissionService PermissionSVC
        {
            get { return permissionSVC; }
        }

        public RepositoryService(IPermissionService permSVC)
        {
            // dependency injection
            permissionSVC = permSVC;
            results = new List<ActionResult>();
            AddResult(new ActionResult(null, VCStatusList.None, "Starting Repository Service", true));
            ReadRules();
        }
        private Collection<RepositoryRule> Rules;

        private void ReadRules()
        {
            Rules = new Collection<RepositoryRule>();

            #region Client Rules

            Rules.Add(new RepositoryRule(101, Core.Variables.Instance.ClientProvider, VCStatusList.PCI_Revoked, VCStatusList.PartiallyCheckedIn, true, true, false, true, true, this.GeneralCommand, "Check In (Overwrite)", VCStatusList.PartiallyCheckedIn, VCStatusList.PartiallyCheckedIn, false));
            Rules.Add(new RepositoryRule(102, Core.Variables.Instance.ClientProvider, VCStatusList.None, VCStatusList.CheckedIn, true, true, false, false, true, this.GeneralCommand, "Check In", VCStatusList.CheckedIn, VCStatusList.CheckedIn, true));
            Rules.Add(new RepositoryRule(103, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedOut, VCStatusList.CheckedIn, true, true, false, true, true, this.GeneralCommand, "Check In", VCStatusList.CheckedIn, VCStatusList.CheckedIn, true));
            Rules.Add(new RepositoryRule(104, Core.Variables.Instance.ClientProvider, VCStatusList.MarkedForDelete, VCStatusList.CheckedIn, true, true, false, false, true, this.GeneralCommand, "Check In", VCStatusList.MarkedForDelete, VCStatusList.MarkedForDelete, true));
            Rules.Add(new RepositoryRule(105, Core.Variables.Instance.ClientProvider, VCStatusList.MarkedForDelete, VCStatusList.CheckedIn, true, true, false, false, true, this.GeneralCommand, "", VCStatusList.MarkedForDelete, VCStatusList.MarkedForDelete, true));
            Rules.Add(new RepositoryRule(106, Core.Variables.Instance.ClientProvider, VCStatusList.Locked, VCStatusList.CheckedIn, true, true, false, false, true, this.GeneralCommand, "", VCStatusList.Locked, VCStatusList.CheckedIn, false));
            Rules.Add(new RepositoryRule(107, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedIn, VCStatusList.CheckedIn, true, true, true, false, true, this.GeneralCommand, "", VCStatusList.CheckedIn, VCStatusList.CheckedIn, false)); //Objects that are checked in must still be checkedin with the diagram (2 july 2015 : bool READ false-->true)
            //OPEN
            //Rules.Add(new RepositoryRule(501, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedIn, VCStatusList.CheckedIn, false, true, true, true, false, false, true, this.GeneralCommand, "Open", VCStatusList.CheckedIn, VCStatusList.CheckedIn));
            Rules.Add(new RepositoryRule(502, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedOut, VCStatusList.CheckedOut, true, true, true, false, true, this.GeneralCommand, "Open", VCStatusList.None, VCStatusList.None, false));
            Rules.Add(new RepositoryRule(503, Core.Variables.Instance.ClientProvider, VCStatusList.PartiallyCheckedIn, VCStatusList.PartiallyCheckedIn, true, true, true, false, true, this.GeneralCommand, "Open", VCStatusList.None, VCStatusList.None, false));
            Rules.Add(new RepositoryRule(504, Core.Variables.Instance.ClientProvider, VCStatusList.None, VCStatusList.None, true, true, true, false, true, this.GeneralCommand, "Open", VCStatusList.None, VCStatusList.None, false));
            Rules.Add(new RepositoryRule(505, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedOut, VCStatusList.None, true, true, true, false, true, this.GeneralCommand, "", VCStatusList.None, VCStatusList.CheckedOut, false));

            //opening of readonly/locked/obsolete items allowed
            Rules.Add(new RepositoryRule(515, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedOutRead, VCStatusList.CheckedOutRead, true, true, true, false, false, this.GeneralCommand, "Open", VCStatusList.None, VCStatusList.None, false));
            //Rules.Add(new RepositoryRule(516, Core.Variables.Instance.ClientProvider, VCStatusList.Locked, VCStatusList.Locked, false, false, true, true, true, false, false, this.GeneralCommand, "Open", VCStatusList.Locked, VCStatusList.Locked));
            //Rules.Add(new RepositoryRule(517, Core.Variables.Instance.ClientProvider, VCStatusList.Obsolete, VCStatusList.Obsolete, false, false, true, true, true, false, false, this.GeneralCommand, "Open", VCStatusList.Obsolete, VCStatusList.Obsolete));
            //opening of readonly/locked/obsolete items allowed
            //Rules.Add(new RepositoryRule(331, Core.Variables.Instance.ClientProvider, VCStatusList.MarkedForDelete, VCStatusList.Deleted, true, true, true, false, false, false, true, this.GeneralCommand, "Delete", VCStatusList.MarkedForDelete, VCStatusList.Deleted, false));


            Rules.Add(new RepositoryRule(108, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedOutRead, VCStatusList.CheckedIn, true, true, true, false, true, this.GeneralCommand, "", VCStatusList.None, VCStatusList.CheckedIn, false, false)); //checking in a diagram with an object that is checked out read
            Rules.Add(new RepositoryRule(109, Core.Variables.Instance.ClientProvider, VCStatusList.Locked, VCStatusList.CheckedOut, true, true, true, false, true, this.GeneralCommand, "", VCStatusList.None, VCStatusList.None, false));

            Rules.Add(new RepositoryRule(110, Core.Variables.Instance.ClientProvider, VCStatusList.None, VCStatusList.CheckedOut, true, true, true, false, true, this.GeneralCommand, "", VCStatusList.None, VCStatusList.None, true));
            Rules.Add(new RepositoryRule(111, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedIn, VCStatusList.CheckedOut, true, true, true, false, true, this.GeneralCommand, "", VCStatusList.None, VCStatusList.None, true));
            Rules.Add(new RepositoryRule(112, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedOutRead, VCStatusList.CheckedOut, true, true, true, false, true, this.GeneralCommand, "", VCStatusList.None, VCStatusList.None, false));
            Rules.Add(new RepositoryRule(113, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedIn, VCStatusList.None, true, true, true, false, true, this.GeneralCommand, "", VCStatusList.None, VCStatusList.None, true));
            Rules.Add(new RepositoryRule(114, Core.Variables.Instance.ClientProvider, VCStatusList.MarkedForDelete, VCStatusList.CheckedOut, true, true, true, false, true, this.GeneralCommand, "", VCStatusList.None, VCStatusList.None, false));
            Rules.Add(new RepositoryRule(115, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedOutRead, VCStatusList.CheckedIn, true, true, true, false, true, this.GeneralCommand, "", VCStatusList.None, VCStatusList.CheckedIn, false));

            Rules.Add(new RepositoryRule(116, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedOut, VCStatusList.CheckedIn, true, true, true, false, true, this.GeneralCommand, "", VCStatusList.None, VCStatusList.CheckedOut, false));

            Rules.Add(new RepositoryRule(117, Core.Variables.Instance.ClientProvider, VCStatusList.Locked, VCStatusList.CheckedIn, false, false, true, false, true, this.GeneralCommand, "", VCStatusList.Locked, VCStatusList.CheckedIn, false));
            
            Rules.Add(new RepositoryRule(118, Core.Variables.Instance.ClientProvider, VCStatusList.CheckedOutRead, VCStatusList.None, false, false, true, false, true, this.GeneralCommand, "", VCStatusList.None, VCStatusList.None, false));

            #endregion

            #region Server Rules

            Rules.Add(new RepositoryRule(201, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedIn, VCStatusList.CheckedOut, true, true, false, false, false, this.GeneralCommand, "Check Out", VCStatusList.CheckedOut, VCStatusList.CheckedOut, true));
            Rules.Add(new RepositoryRule(202, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedIn, VCStatusList.CheckedOutRead, true, true, true, false, false, this.GeneralCommand, "Check Out (Read)", VCStatusList.None, VCStatusList.CheckedOutRead, false));//, true));
            Rules.Add(new RepositoryRule(203, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedOut, VCStatusList.CheckedOutRead, true, true, true, false, false, this.GeneralCommand, "Check Out (Read)", VCStatusList.None, VCStatusList.CheckedOutRead, false, true));
            Rules.Add(new RepositoryRule(204, Core.Variables.Instance.ServerProvider, VCStatusList.Locked, VCStatusList.CheckedOutRead, true, true, true, false, false, this.GeneralCommand, "Check Out (Read)", VCStatusList.None, VCStatusList.Locked, false));
            Rules.Add(new RepositoryRule(205, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedIn, VCStatusList.Locked, true, false, false, false, true, this.GeneralCommand, "Lock", VCStatusList.Locked, VCStatusList.None, true)); //Made rule cascade
            //Rules.Add(new RepositoryRule(206, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedOut, VCStatusList.Locked, true, false, true, false, false, false, false, this.GeneralCommand, "Lock", VCStatusList.Locked, VCStatusList.None));
            //Rules.Add(new RepositoryRule(207, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedIn, VCStatusList.Obsolete, true, true, true, false, false, false, true, this.GeneralCommand, "", VCStatusList.Obsolete, VCStatusList.None));
            //Rules.Add(new RepositoryRule(208, Core.Variables.Instance.ServerProvider, VCStatusList.Locked, VCStatusList.Obsolete, true, false, true, false, false, false, false, this.GeneralCommand, "Mark Obsolete", VCStatusList.Obsolete, VCStatusList.None));
            //Rules.Add(new RepositoryRule(209, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedOut, VCStatusList.Obsolete, true, false, true, false, false, false, false, this.GeneralCommand, "Mark Obsolete", VCStatusList.Obsolete, VCStatusList.None));
            Rules.Add(new RepositoryRule(210, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedOut, VCStatusList.CheckedIn, true, false, false, false, false, this.GeneralCommand, "Force Check In", VCStatusList.CheckedIn, VCStatusList.None, true));
            Rules.Add(new RepositoryRule(211, Core.Variables.Instance.ServerProvider, VCStatusList.Locked, VCStatusList.CheckedIn, true, false, false, false, true, this.GeneralCommand, "Unlock", VCStatusList.CheckedIn, VCStatusList.None, true)); //THIS IS THE LOOPHOLE RULE //Made rule cascade
            //Rules.Add(new RepositoryRule(212, Core.Variables.Instance.ServerProvider, VCStatusList.Obsolete, VCStatusList.CheckedIn, true, false, true, false, false, false, true, this.GeneralCommand, "Restore (Check In)", VCStatusList.CheckedIn, VCStatusList.None));
            Rules.Add(new RepositoryRule(213, Core.Variables.Instance.ServerProvider, VCStatusList.MarkedForDelete, VCStatusList.CheckedIn, true, true, false, false, true, this.GeneralCommand, "Restore (Check In)", VCStatusList.CheckedIn, VCStatusList.None, true));
            //Rules.Add(new RepositoryRule(214, Core.Variables.Instance.ServerProvider, VCStatusList.MarkedForDelete, VCStatusList.Obsolete, true, false, true, false, false, false, false, this.GeneralCommand, "Mark Obsolete", VCStatusList.Obsolete, VCStatusList.None));
            Rules.Add(new RepositoryRule(215, Core.Variables.Instance.ServerProvider, VCStatusList.Locked, VCStatusList.CheckedOut, true, true, true, false, true, this.GeneralCommand, "", VCStatusList.Locked, VCStatusList.Locked, false));
            //Rules.Add(new RepositoryRule(216, Core.Variables.Instance.ServerProvider, VCStatusList.Obsolete, VCStatusList.CheckedOut, true, true, true, true, true, false, false, this.GeneralCommand, "Check Out (Read)", VCStatusList.Obsolete, VCStatusList.Obsolete));
            Rules.Add(new RepositoryRule(217, Core.Variables.Instance.ServerProvider, VCStatusList.PartiallyCheckedIn, VCStatusList.CheckedIn, true, false, false, false, false, this.GeneralCommand, "Check In", VCStatusList.CheckedIn, VCStatusList.None, true));
            Rules.Add(new RepositoryRule(218, Core.Variables.Instance.ServerProvider, VCStatusList.PartiallyCheckedIn, VCStatusList.CheckedOutRead, true, false, false, false, false, this.GeneralCommand, "Check Out (Read)", VCStatusList.None, VCStatusList.CheckedOutRead, false));//, true));
            Rules.Add(new RepositoryRule(219, Core.Variables.Instance.ServerProvider, VCStatusList.PartiallyCheckedIn, VCStatusList.PCI_Revoked, true, true, false, true, true, this.GeneralCommand, "Check Out (Cancel PCI)", VCStatusList.PCI_Revoked, VCStatusList.PCI_Revoked, true));
            //Rules.Add(new RepositoryRule(220, Core.Variables.Instance.ServerProvider, VCStatusList.PartiallyCheckedIn, VCStatusList.PCI_Revoked, true, true, true, false, false, false, true, this.GeneralCommand, "Check Out (Cancel PCI)", VCStatusList.PCI_Revoked, VCStatusList.PCI_Revoked,true));
            Rules.Add(new RepositoryRule(221, Core.Variables.Instance.ServerProvider, VCStatusList.MarkedForDelete, VCStatusList.CheckedOut, true, true, true, false, false, this.GeneralCommand, "", VCStatusList.None, VCStatusList.MarkedForDelete, false));
            Rules.Add(new RepositoryRule(222, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedOut, VCStatusList.CheckedOut, true, true, false, false, true, this.GeneralCommand, "", VCStatusList.None, VCStatusList.CheckedOutRead, false));
            Rules.Add(new RepositoryRule(223, Core.Variables.Instance.ServerProvider, VCStatusList.MarkedForDelete, VCStatusList.CheckedOutRead, true, true, true, false, false, this.GeneralCommand, "", VCStatusList.None, VCStatusList.MarkedForDelete, false)); //Exec on server to preserve file association
            //Rules.Add(new RepositoryRule(224, Core.Variables.Instance.ServerProvider, VCStatusList.Obsolete, VCStatusList.CheckedOutRead, false, true, true, true, true, true, false, this.GeneralCommand, "", VCStatusList.None, VCStatusList.Obsolete)); //Exec on server to preserve file association

            //DELETE
            //Rules.Add(new RepositoryRule(330, Core.Variables.Instance.ServerProvider, VCStatusList.Obsolete, VCStatusList.Deleted, true, true, true, false, false, false, true, this.GeneralCommand, "Delete", VCStatusList.Obsolete, VCStatusList.Deleted, false));
            //Rules.Add(new RepositoryRule(331, Core.Variables.Instance.ClientProvider, VCStatusList.Obsolete, VCStatusList.Deleted, true, true, true, false, false, false, true, this.GeneralCommand, "Delete", VCStatusList.Obsolete, VCStatusList.Deleted, false));
            Rules.Add(new RepositoryRule(330, Core.Variables.Instance.ServerProvider, VCStatusList.MarkedForDelete, VCStatusList.Deleted, true, false, false, false, true, this.GeneralCommand, "Delete", VCStatusList.MarkedForDelete, VCStatusList.Deleted, true, false));


            Rules.Add(new RepositoryRule(224, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedIn, VCStatusList.CheckedIn, true, true, true, false, false, this.GeneralCommand, "", VCStatusList.None, VCStatusList.None, false, false)); // When we lock at a diagram level and objects are already checkedin
            Rules.Add(new RepositoryRule(225, Core.Variables.Instance.ServerProvider, VCStatusList.Locked, VCStatusList.Locked, true, true, true, false, false, this.GeneralCommand, "", VCStatusList.None, VCStatusList.None, false, false)); // Locking a file which already has locked objects
            Rules.Add(new RepositoryRule(226, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedOut, VCStatusList.Locked, true, true, true, false, false, this.GeneralCommand, "", VCStatusList.None, VCStatusList.None, false, false)); // Locking a file where objects are checked out (unless it is checked out to you and you are admin?)
            Rules.Add(new RepositoryRule(227, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedOut, VCStatusList.CheckedOut, true, true, false, true, true, this.GeneralCommand, "", VCStatusList.None, VCStatusList.CheckedOut, true));

            Rules.Add(new RepositoryRule(228, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedIn, VCStatusList.CheckedOut, false, false, true, false, true, this.GeneralCommand, "", VCStatusList.None, VCStatusList.CheckedOutRead, false));

            Rules.Add(new RepositoryRule(229, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedIn, VCStatusList.MarkedForDelete, true, false, false, false, true, this.GeneralCommand, "Mark For Delete", VCStatusList.MarkedForDelete, VCStatusList.None, true));

            Rules.Add(new RepositoryRule(230, Core.Variables.Instance.ServerProvider, VCStatusList.CheckedOut, VCStatusList.CheckedOutRead, true, true, true, true, false, this.GeneralCommand, "Check Out (Read)", VCStatusList.None, VCStatusList.None, false, false));

            #endregion
        }

        public List<RepositoryRule> GetAvailableActions(string provider)
        {
            List<RepositoryRule> retval = new List<RepositoryRule>();
            foreach (RepositoryRule r in Rules)
            {
                if (r.Provider == provider)
                {
                    retval.Add(r);
                }
            }
            return retval;
        }
        public List<RepositoryRule> GetAvailableActions(string provider, IRepositoryItem item, bool IsAdmin, bool HasWrite, bool HasRead)
        {
            List<RepositoryRule> retval = new List<RepositoryRule>();
            foreach (RepositoryRule r in GetAvailableActions(provider))
            {
                //if (r.Provider == provider)
                //{
                VCStatusList itemState = item.State;
                if (item is ObjectAssociation)
                {
                    ObjectAssociation oassoc = item as ObjectAssociation;
                    itemState = (VCStatusList)oassoc.VCStatusID;
                }
                if (r.CurrentState == itemState)
                {
                    bool allow = false;
                    if (item is IWorkspaceItem)
                    {
                        if (r.AllowAdmin && IsAdmin)
                        {
                            allow = true;
                        }

                        if (r.AllowWriter && HasWrite)
                        {
                            allow = true;
                        }

                        if (r.AllowReader && HasRead)
                        {
                            allow = true;
                        }

                        if (r.SameUser && item.VCUser != strings.GetVCIdentifier())
                        {
                            allow = false;
                        }

                        if (r.MustBeDifferentUser && item.VCUser == strings.GetVCIdentifier())
                        {
                            allow = false;
                        }

                        /* if (provider.ToLower() == Core.Variables.Instance.ServerProvider && item.VCUser == strings.GetVCIdentifier() && r.CurrentState == VCStatusList.CheckedOut)
                             allow = false; // already checked out to this user!*/
                    }
                    else
                    {
                        if (item is b.ObjectAssociation)
                        {
                            allow = true; // associations only!
                        }
                    }
                    if (allow)
                    {
                        //Log.WriteLog("Adding Action (" + provider + ") " + r.RuleNumber);
                        if (r.RuleNumber == 229 && (item is GraphFile))
                        {
                            //disallow mfd files
                        }
                        else
                            retval.Add(r);
                    }
                }
                //}
            }
            return retval;
        }
        public RepositoryRule GetValidRule(string provider, IRepositoryItem item, bool AllowAdmin, bool AllowWriter, bool AllowReader, bool IsSameUser, VCStatusList targetState)
        {
            foreach (RepositoryRule r in GetAvailableActions(provider, item, AllowAdmin, AllowWriter, AllowReader))
            {
                if (r.TargetState == targetState && r.CurrentState == item.State)
                {
                    if (r.SameUser)
                    {
                        if (IsSameUser)
                        {
                            /*if (r.CurrentState == MetaBuilder.BusinessLogic.VCStatusList.CheckedOut)
                                return null;*/
                            //Log.WriteLog("Returned rule (" + provider + ")" + r.RuleNumber);
                            return r;
                        }
                    }
                    else
                    {
                        //Log.WriteLog("Returned rule (" + provider + ")" + r.RuleNumber);
                        return r;
                    }
                    //Log.WriteLog("Rule[" + r.RuleNumber + "] Found(" + provider + "). From " + item.State + " --> " + targetState.ToString() + " for " + item.ToString());
                    //return r;
                }
            }
            Log.WriteLog("No rule found(" + provider + "). From " + item.State + " --> " + targetState.ToString() + " for " + item.ToString());
            return null;
        }

        private bool logIsWatching;
        public bool LogIsWatching
        {
            get { return logIsWatching; }
            set { logIsWatching = value; }
        }

        public List<ActionResult> GeneralCommand(Repository server, Repository client, ref ItemAndRule itemAndRule)
        {
            results = new List<ActionResult>();
            bool isAdmin = true;
            bool forceFail = false;
            Workspace targetWorkspace = null;
            if (itemAndRule.Item is IWorkspaceItem)
            {
                IWorkspaceItem wsItem = itemAndRule.Item as IWorkspaceItem;
                WorkspaceKey wsKey = new WorkspaceKey();
                wsKey.WorkspaceTypeId = wsItem.WorkspaceTypeId;
                wsKey.Name = wsItem.WorkspaceName;
                targetWorkspace = permissionSVC.GetWorkspaceOnServer(wsKey);
                PermissionList permission = permissionSVC.GetServerPermission(wsItem.WorkspaceName, wsItem.WorkspaceTypeId);
                isAdmin = (permission == PermissionList.Delete);
                if (targetWorkspace == null && logIsWatching)
                {
                    ActionResult result = new ActionResult();
                    result.TargetState = "";
                    result.FromState = itemAndRule.Rule.CurrentState.ToString();
                    result.TargetState = itemAndRule.Rule.TargetState.ToString();
                    result.Repository = Core.Variables.Instance.ServerProvider;
                    result.Message = "Failed: Cannot access target workspace on server";
                    result.Success = false;
                    AddResult(result);
                    forceFail = true;
                }
            }

            if (!forceFail)
            {
                VCStatusList ConfigVCStatusOverrideServer = itemAndRule.Rule.ServerState;
                VCStatusList ConfigVCStatusOverrideClient = itemAndRule.Rule.ClientState;
                if (!isAdmin)
                {
                    if (permissionSVC.SERVERCONFIG__AdminMustApproveCheckins && (itemAndRule.Item.State == VCStatusList.CheckedOut || itemAndRule.Item.State == VCStatusList.None))
                    {
                        ConfigVCStatusOverrideServer = VCStatusList.PartiallyCheckedIn;
                        ConfigVCStatusOverrideClient = VCStatusList.PartiallyCheckedIn;
                    }
                }

                IRepositoryItem item = itemAndRule.Item;
                if (itemAndRule.Rule.ServerState != VCStatusList.None)
                {
                    AddResult(server.SetState(ref itemAndRule, ConfigVCStatusOverrideServer, targetWorkspace, logIsWatching));
                }

                if (itemAndRule.Rule.ClientState != VCStatusList.None)
                {
                    AddResult(client.SetState(ref itemAndRule, ConfigVCStatusOverrideClient, targetWorkspace, logIsWatching));
                }
                item.State = (itemAndRule.Rule.ServerState != VCStatusList.None) ? ConfigVCStatusOverrideServer : ConfigVCStatusOverrideClient;
            }
            return results;
        }

        /*
        public void Force(Repository server, Repository client, ref b.IRepositoryItem item, b.VCStatusList targetState)
        {
            bool isAdmin = true;
            bool forceFail = false;
            b.Workspace targetWorkspace = null;
            if (item is b.IWorkspaceItem)
            {
                b.IWorkspaceItem wsItem = item as b.IWorkspaceItem;
                b.WorkspaceKey wsKey = new MetaBuilder.BusinessLogic.WorkspaceKey();
                wsKey.WorkspaceTypeId = wsItem.WorkspaceTypeId;
                wsKey.Name = wsItem.WorkspaceName;
                targetWorkspace = permissionSVC.GetWorkspaceOnServer(wsKey);
                b.PermissionList permission = permissionSVC.GetServerPermission(wsItem.WorkspaceName, wsItem.WorkspaceTypeId);
                isAdmin = (permission == MetaBuilder.BusinessLogic.PermissionList.Admin);
                if (targetWorkspace == null && logIsWatching)
                {
                    ActionResult result = new ActionResult();
                    result.Message = "Failed: Cannot access target workspace on server";
                    result.Success = false;
                    AddResult(result);
                    forceFail = true;
                }
            }

            if ((isAdmin) && (!forceFail))
            {
                AddResult(server.SetState(ref item, targetState , targetWorkspace,logIsWatching));
                item.State = targetState;
            }
        }
        public void CheckOut(Repository server, Repository client, ref b.IRepositoryItem item, b.VCStatusList targetState)
        {
            bool hasWrite = true;
            bool isAdmin = true;
            bool forceFail = false;
            b.Workspace targetWorkspace = null;
            b.Workspace clientWorkspace = null;

            if (item is b.IWorkspaceItem)
            {
                b.IWorkspaceItem wsItem = item as b.IWorkspaceItem;
                b.WorkspaceKey wsKey = new MetaBuilder.BusinessLogic.WorkspaceKey();
                wsKey.WorkspaceTypeId = wsItem.WorkspaceTypeId;
                wsKey.Name = wsItem.WorkspaceName;

                targetWorkspace = permissionSVC.GetWorkspaceOnServer(wsKey);
                clientWorkspace = permissionSVC.GetWorkspaceOnClient(wsKey);

                b.PermissionList permission = permissionSVC.GetServerPermission(wsItem.WorkspaceName, wsItem.WorkspaceTypeId);
                isAdmin = (permission == MetaBuilder.BusinessLogic.PermissionList.Admin);
                hasWrite = (permission == MetaBuilder.BusinessLogic.PermissionList.Admin || permission == MetaBuilder.BusinessLogic.PermissionList.Write);
                
                if (targetWorkspace == null)
                {
                    ActionResult result = new ActionResult();
                    result.Message = "Failed: Cannot access target workspace on server";
                    result.Success = false;
                    AddResult(result);
                    forceFail = true;
                }
            }
            
            if ((hasWrite || isAdmin) && (!forceFail))
            {
                AddResult(server.SetState(ref item, b.VCStatusList.CheckedOut, targetWorkspace, logIsWatching));
                AddResult(client.SetState(ref item, b.VCStatusList.CheckedOut, clientWorkspace, logIsWatching));
                item.State = MetaBuilder.BusinessLogic.VCStatusList.CheckedOut;
            }
        }
        */
        /* private bool CanPerformAction(b.IRepositoryItem item, b.VCStatusList targetState,bool isAdmin, bool hasWrite)
         {
             switch (item.State)
             {
                 case b.VCStatusList.Locked:
                     if (isAdmin)
                     {
                         if (targetState == b.VCStatusList.CheckedIn
                             || targetState == b.VCStatusList.Obsolete)
                             return true;
                     }
                     if (hasWrite && targetState == b.VCStatusList.CheckedOut)
                         return true;

                     if (targetState == b.VCStatusList.CheckedOutRead && item.VCUser!= Core.Variables.Instance.UserDomainIdentifier)
                         return true;
                     break;
                 case  b.VCStatusList.CheckedOut:
                     if (isAdmin
                             && (targetState == b.VCStatusList.CheckedIn
                             || targetState == b.VCStatusList.Obsolete
                             || targetState == b.VCStatusList.Locked))
                         {
                             return true;
                         }

                     if (hasWrite && 
                         (targetState == b.VCStatusList.CheckedIn))
                     {
                         return true;
                     }

                     if (hasWrite && (targetState == b.VCStatusList.CheckedOutRead))
                     {
                         // is this user the current owner?
                         if (item.VCUser != Core.Variables.Instance.UserDomainIdentifier)
                         {
                             return false;
                         }
                     }

                     break;
                 case b.VCStatusList.CheckedIn:
                     if (isAdmin)
                     {
                         if (targetState == b.VCStatusList.CheckedOutRead)
                             return false;
                         if 
                     }

                     if (hasWrite || hasRead)
                     {
                         if (targetState == b.VCStatusList.CheckedOutRead)
                             return false;
                     }
                     if (targetState == MetaBuilder.BusinessLogic.VCStatusList.CheckedOut && (hasWrite||isAdmin))
                         return true;

                     break;
             }
             return false;
         }*/

        public event ActionResultEventHandler ResultAdded;
        protected void OnResultAdded(ActionResult result)
        {
            results.Add(result);
            if (ResultAdded != null)
                ResultAdded(result);
        }
        public void AddResult(ActionResult result)
        {
            OnResultAdded(result);
        }
        /*
        public void CheckIn(Repository server, Repository client, ref b.IRepositoryItem item, b.VCStatusList unusedtargetState)
        {
            bool hasWrite = true;
            bool isAdmin = true;
            bool forceFail = false;
            b.Workspace targetWorkspace = null;
            b.Workspace clientWorkspace = null;
            if (item is b.IWorkspaceItem)
            {
                b.IWorkspaceItem wsItem = item as b.IWorkspaceItem;
                b.WorkspaceKey wsKey = new MetaBuilder.BusinessLogic.WorkspaceKey();
                wsKey.WorkspaceTypeId = 3;
                wsKey.Name = wsItem.WorkspaceName;
                
                targetWorkspace = permissionSVC.GetWorkspaceOnServer(wsKey);
                clientWorkspace = permissionSVC.GetWorkspaceOnClient(wsKey);

                b.PermissionList permission = permissionSVC.GetServerPermission(wsItem.WorkspaceName, wsItem.WorkspaceTypeId);
                isAdmin = (permission == MetaBuilder.BusinessLogic.PermissionList.Admin);
                hasWrite = (permission == MetaBuilder.BusinessLogic.PermissionList.Admin || permission == MetaBuilder.BusinessLogic.PermissionList.Write);
                if (targetWorkspace == null)
                {

                    ActionResult result = new ActionResult();
                    result.Message = "Failed: Cannot access target workspace on server";
                    result.Success = false;
                    AddResult(result);
                    forceFail = true;
                }
            }
           
            if ((!forceFail) && (isAdmin || hasWrite))
            {
                
                b.VCStatusList ConfigVCStatusOverride = b.VCStatusList.CheckedIn;
                if (!isAdmin)
                {
                    if (permissionSVC.SERVERCONFIG__AdminMustApproveCheckins)
                    {
                        ConfigVCStatusOverride = MetaBuilder.BusinessLogic.VCStatusList.PartiallyCheckedIn;
                    }
                
                }
                // PARTIAL CHECKINS!
                if (isAdmin && item.State == MetaBuilder.BusinessLogic.VCStatusList.PartiallyCheckedIn)
                {
                    ActionResult adminResult = new ActionResult();
                    
                    if (item is b.GraphFile)
                    {
                        ActionResult graphFileResult = new ActionResult();
                        
                        b.GraphFile file = MetaBuilder.DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.GraphFileProvider.GetBypkidMachine(item.Pkid, item.MachineName);
                        file.VCStatusID = (int)b.VCStatusList.CheckedIn;
                        MetaBuilder.DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.GraphFileProvider.Update(file);
                        graphFileResult.Success = true;
                        graphFileResult.Message = file.Name + " - Checked In";
                        adminResult.InnerResults.Add(graphFileResult);
                    }

                    if (item is Meta.MetaBase)
                    {
                        
                        b.MetaObject obj = MetaBuilder.DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.MetaObjectProvider.GetBypkidMachine(item.Pkid, item.MachineName);
                        obj.VCStatusID = (int)b.VCStatusList.CheckedIn;
                        MetaBuilder.DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.MetaObjectProvider.Update(obj);
                    }

                    if (item is b.ObjectAssociation)
                    {
                        b.ObjectAssociation ass = item as b.ObjectAssociation;
                        ass.VCStatusID = (int)b.VCStatusList.CheckedIn;
                        MetaBuilder.DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.ObjectAssociationProvider.Update(ass);
                    }

                    if (item.EmbeddedItems != null)
                    {
                        List<b.IRepositoryItem> childItems = new List<MetaBuilder.BusinessLogic.IRepositoryItem>();
                        foreach (b.IRepositoryItem childItem in item.EmbeddedItems)
                        {
                            childItems.Add(childItem);

                        }
                        for (int i = 0; i < childItems.Count; i++)
                        {
                            b.IRepositoryItem child = childItems[i];
                            CheckIn(server, client, ref child,unusedtargetState);
                        }
                    }
                    adminResult.Message = "Items Checked In Successfully";
                    adminResult.Success = true;
                    AddResult(adminResult);
                }
                else
                {
                    AddResult(server.SetState(ref item, ConfigVCStatusOverride, targetWorkspace, logIsWatching));
                    AddResult(client.SetState(ref item, ConfigVCStatusOverride, clientWorkspace, logIsWatching));
                    item.State = ConfigVCStatusOverride;
                }
                
                //items[i] = item;
            }
        }
        public void CheckOutRead(Repository server, Repository client, ref b.IRepositoryItem item,b.VCStatusList unusedtargetState)
        {
            b.Workspace targetWorkspace = null;
            bool hasRead = true;
            if (item is b.IWorkspaceItem)
            {
                b.IWorkspaceItem wsItem = item as b.IWorkspaceItem;
                b.PermissionList permission = permissionSVC.GetServerPermission(wsItem.WorkspaceName, wsItem.WorkspaceTypeId);
                hasRead =       (permission == MetaBuilder.BusinessLogic.PermissionList.Read) ||
                                (permission == MetaBuilder.BusinessLogic.PermissionList.Admin) ||
                                (permission == MetaBuilder.BusinessLogic.PermissionList.Write);

                b.WorkspaceKey wsKey = new MetaBuilder.BusinessLogic.WorkspaceKey();
                wsKey.WorkspaceTypeId = wsItem.WorkspaceTypeId;
                wsKey.Name = wsItem.WorkspaceName;

                targetWorkspace = permissionSVC.GetWorkspaceOnClient(wsKey);
                targetWorkspace.Name = wsItem.WorkspaceName;
                targetWorkspace.WorkspaceTypeId = wsItem.WorkspaceTypeId;

            }
            if (hasRead)
            {
                AddResult(client.SetState(ref item, b.VCStatusList.CheckedOutRead, targetWorkspace, logIsWatching));
            }
        }
        */
        public void ShutDown()
        {
            AddResult(new ActionResult(null, VCStatusList.None, "Shutting down repository service", true));
        }
    }
}