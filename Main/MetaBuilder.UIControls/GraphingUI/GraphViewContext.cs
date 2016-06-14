using MetaBuilder.Graphing.Containers;
using MetaBuilder.PluginSDK;

namespace MetaBuilder.UIControls.GraphingUI
{
    public class GraphViewContext : IPluginContext
    {

		#region Fields (2) 

        private GraphView _currentGraphView;
        private GraphPalette _currentStencil;

		#endregion Fields 

		#region Constructors (1) 

        public GraphViewContext(GraphView view, GraphPalette palette)
        {
            _currentGraphView = view;
            _currentStencil = palette;
        }

		#endregion Constructors 

		#region Properties (2) 

        public GraphView CurrentGraphView
        {
            get { return _currentGraphView; }
            set { _currentGraphView = value; }
        }

        public GraphPalette CurrentStencil
        {
            get { return _currentStencil; }
            set { _currentStencil = value; }
        }

		#endregion Properties 

    }
}