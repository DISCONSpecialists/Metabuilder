using MetaBuilder.PluginSDK;

namespace DisconPlugins.Hierarchy
{
    public class NumberingPlugin : IPlugin
    {

        #region Properties (1)

        public string Name
        {
            get { return "Hierarchy Numbering"; }
        }

        #endregion Properties

        #region Methods (1)

        // Public Methods (1) 

        public bool PerformAction(IPluginContext context)
        {
            Numbering numbering = Numbering.Instance;
            numbering.NumberNodes((context as IPluginContext).CurrentGraphView);
            return numbering.Failed;
        }

        #endregion Methods

    }
}
