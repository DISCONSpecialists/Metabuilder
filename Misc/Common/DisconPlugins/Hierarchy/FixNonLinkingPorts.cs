using MetaBuilder.PluginSDK;

namespace DisconPlugins.Hierarchy
{
    public class FixNonLinkingPorts : IPlugin
    {

        #region Properties (1)

        public string Name
        {
            get { return "Fix Non-Linking Ports"; }
        }

        #endregion Properties

        #region Methods (1)


        // Public Methods (1) 

        public bool PerformAction(IPluginContext context)
        {
            try
            {
                context.CurrentGraphView.StartTransaction();
                PortEnabler shremove = new PortEnabler();
                shremove.MyView = context.CurrentGraphView;
                shremove.Execute();
                context.CurrentGraphView.FinishTransaction("Remove non linking ports");
                return false;
            }
            catch
            {
                return true;
            }

        }


        #endregion Methods

    }
}
