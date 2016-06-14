using MetaBuilder.BusinessLogic;
using b = MetaBuilder.BusinessLogic;
namespace MetaBuilder.BusinessFacade.Storage.RepositoryTemp
{
    public interface IPermissionService
    {
        TList<Workspace> GetAccessibleServerWorkspaces();
        void SynchroniseServerWorkspaces();
        TList<Workspace> GetInaccessibleServerWorkspaces();
        PermissionList GetServerPermission(string WorkspaceName, int WorkspaceTypeId);
        Workspace GetWorkspaceOnServer(WorkspaceKey wsKey);
        Workspace GetWorkspaceOnClient(WorkspaceKey wsKey);
        // b.TList<b.Workspace> VerifyWorkspaces(b.TList<b.Workspace> workspaces); // note sure if this should be here
        bool HasAtLeastThisPermission(WorkspaceKey wsKey, PermissionList permissionRequested);


        bool SERVERCONFIG__AdminMustApproveCheckins { get;}
        bool SERVERCONFIG__CannotCheckInFromSandbox { get;}
        bool SERVERCONFIG__AutoCreateWorkspaces { get;}
        bool SERVERCONFIG__CheckForObsoleteObjectsOnCheckOut { get;}
        bool SERVERCONFIG_ObsoletesCanBeReinstated { get;}


    }
}
