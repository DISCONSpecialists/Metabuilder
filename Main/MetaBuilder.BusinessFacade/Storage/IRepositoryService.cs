using System;
using System.Collections.Generic;
using System.Text;
using b = MetaBuilder.BusinessLogic;
namespace MetaBuilder.BusinessFacade.Storage
{
    public interface IRepositoryService
    {
        //void GetClientCopy(b.VCStatusList targetServerState, b.VCStatusList targetClientState, bool force);
        //void SetServerCopy(b.VCStatusList targetServerState, b.VCStatusList targetClientState, bool force);

        CheckInResult PutItem(b.VCStatusList targetState, RepositoryActionSpecification spec);
        CheckOutResult GetItem(b.VCStatusList targetState, ref RepositoryActionSpecification spec, bool force);
        bool IsAdmin { get;}
        List<b.VCStatusList> GetAllowedStates(b.IRepositoryItem item);
        string Name { get;set;}
    }


}
