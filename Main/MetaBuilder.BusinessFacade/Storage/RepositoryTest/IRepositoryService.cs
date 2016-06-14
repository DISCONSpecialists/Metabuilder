using System.Collections.Generic;
using MetaBuilder.BusinessLogic;
using b = MetaBuilder.BusinessLogic;

namespace MetaBuilder.BusinessFacade.Storage.RepositoryTemp
{
    public interface IRepositoryService
    {
        IPermissionService PermissionSVC { get;}
        List<ActionResult> Results { get;}
        /// <summary>
        /// Checking out an item:
        ///     1) starts a transaction
        ///      2) checks for valid permissions (server side)
        ///      3) marks it as checked out on both client and server
        ///     4) attempts to check out children (objects, relationships & artefacts)
        ///     5) ends the transaction
        ///      6) [creates the server workspace for each item on the client's machine if it doesnt already exist]
        ///      7) returns a result list
        /// </summary>
        /// <param name=Core.Variables.Instance.ServerProvider></param>
        /// <param name=Core.Variables.Instance.ClientProvider></param>
        /// <param name="items"></param>
        List<ActionResult> GeneralCommand(Repository server, Repository client, ref ItemAndRule itemAndRule);
        //void CheckOut(Repository server, Repository client, ref b.IRepositoryItem item, b.VCStatusList targetState);
        //void Force(Repository server, Repository client, ref b.IRepositoryItem item, b.VCStatusList targetState);
        event ActionResultEventHandler ResultAdded;//{get;set;}
        List<RepositoryRule> GetAvailableActions(string provider, IRepositoryItem item, bool IsAdmin, bool HasWrite, bool HasRead);
        List<RepositoryRule> GetAvailableActions(string provider);
        RepositoryRule GetValidRule(string provider, IRepositoryItem item, bool IsAdmin, bool HasWrite, bool HasRead,bool SameUser, VCStatusList targetState);
        bool LogIsWatching { get;set;}
        /// <summary>
        /// Checking in an item:
        ///     1) starts a transaction
        ///      2) checks for valid permissions (server side)
        ///      3) if the client's workspace doesnt exist on the server, it's marked as partially checked in. 
        ///         it will remain in this state until the administrator has moved it to a valid workspace.
        ///     4) marks it as checked in on both client and server (see point 3)
        ///     5) attempts to check in children (objects, relationships & artefacts) in the same manner
        ///     6) ends the transaction
        ///     7) returns a result list
        /// </summary>
        /// <param name=Core.Variables.Instance.ServerProvider></param>
        /// <param name=Core.Variables.Instance.ClientProvider></param>
        /// <param name="items"></param>
        ///void CheckIn(Repository server, Repository client, ref b.IRepositoryItem item, b.VCStatusList targetState);

        /// <summary>
        /// Checking out an item in read-only mode
        ///     1) starts a transaction
        ///      2) checks for valid permissions (server side)
        ///      3) [creates the server workspace for each item on the client's machine if it doesnt already exist]
        ///      4) marks it as checked out read on the client, given that it IS NOT CHECKED OUT by this user
        ///     5) attempts to check out children (objects, relationships & artefacts) in read only mode, given that it IS NOT CHECKED OUT already by this user
        ///     6) ends the transaction
        ///      7) returns a result list
        /// </summary>
        /// <param name=Core.Variables.Instance.ServerProvider></param>
        /// <param name=Core.Variables.Instance.ClientProvider></param>
        /// <param name="items"></param>
        //void CheckOutRead(Repository server, Repository client, ref b.IRepositoryItem item, b.VCStatusList targetState);
        // void Lock(Repository server, List<IRepositoryItem> items);
        // void Unlock(Repository server, List<IRepositoryItem> items);
        // void MarkAsObsolete(Repository server, List<IRepositoryItem> items);

        void ShutDown();
    }
}
