using System.Data;
using MetaBuilder.DataAccessLayer.OldCode.Diagramming;

namespace MetaBuilder.BusinessFacade.Storage
{
    public class TechniqueHelper
    {

        public DataView GetList()
        {
            TechniqueAdapter adapter = new TechniqueAdapter();
            return adapter.GetList();
        }

       
    }
}
