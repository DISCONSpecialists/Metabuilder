using MetaBuilder.Graphing.Containers;

namespace MetaBuilder.Graphing.Controllers
{
    public class ContextMenuController
    {
        #region Fields (1) 

        private GraphView MyView;

        #endregion Fields 

        #region Constructors (1) 

        public ContextMenuController(GraphView myView)
        {
            MyView = myView;
        }

        #endregion Constructors 
    }
}