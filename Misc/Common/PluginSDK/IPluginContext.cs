using MetaBuilder.Graphing.Containers;

namespace MetaBuilder.PluginSDK
{
    /// <summary>
    /// A public interface used to pass context to plugins
    /// </summary>
    public interface IPluginContext
    {
        GraphView CurrentGraphView { get;set;}
        GraphPalette CurrentStencil { get;set;}
    }
}
