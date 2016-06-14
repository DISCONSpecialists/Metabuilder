using MetaBuilder.PluginSDK;

namespace DisconPlugins.Hierarchy
{
    public class ReplaceShapePlugin : IPlugin
    {

        #region Properties (1)

        public string Name
        {
            get { return "Replace Shapes"; }
        }

        #endregion Properties

        #region Methods (1)


        // Public Methods (1) 

        public bool PerformAction(IPluginContext context)
        {
            try
            {
                context.CurrentGraphView.StartTransaction();
                ShapeReplacer srep = new ShapeReplacer();
                srep.MyView = context.CurrentGraphView;
                srep.Palette = context.CurrentStencil;
                srep.Execute();
                context.CurrentGraphView.FinishTransaction("Replace Shapes");
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
