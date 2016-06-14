using System.Collections.Generic;
using MetaBuilder.BusinessLogic;
using b = MetaBuilder.BusinessLogic;

namespace MetaBuilder.BusinessFacade.Storage.RepositoryTemp
{
    public delegate void RepositoryEventHandler(Repository server, Repository client, ref IRepositoryItem item, VCStatusList targetState);
    public delegate List<ActionResult> GeneralRepositoryEventHandler(Repository server, Repository client, ref ItemAndRule itemAndRule);
    public delegate void ActionResultEventHandler(ActionResult result);
}
