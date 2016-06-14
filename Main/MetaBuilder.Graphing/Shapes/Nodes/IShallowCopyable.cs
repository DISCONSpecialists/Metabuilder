using MetaBuilder.Graphing.Shapes.Behaviours;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Shapes.Nodes
{
    public interface IShallowCopyable : IMetaNode, IIdentifiable
    {
        bool CopyAsShadow { get; set; }
        GoObject CopyAsShallow();
        GoObject Copy();
    }
}