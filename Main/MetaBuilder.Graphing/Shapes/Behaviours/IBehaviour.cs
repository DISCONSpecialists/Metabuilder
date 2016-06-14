using Northwoods.Go;

namespace MetaBuilder.Graphing.Shapes.Behaviours
{
    public interface IBehaviour
    {
        // How many instances of this behaviour may there be on a shape?
        bool AllowMultiple { get; }
        IBehaviour Copy(GoObject observer);
    }
}