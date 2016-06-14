using MetaBuilder.BusinessLogic;
using Northwoods.Go;
using b = MetaBuilder.BusinessLogic;

namespace MetaBuilder.Graphing.Containers
{
    public interface IMapContainer
    {
        TList<ObjectAssociation> SaveMappings(TList<ObjectAssociation> associations, GoView view);
    }
}