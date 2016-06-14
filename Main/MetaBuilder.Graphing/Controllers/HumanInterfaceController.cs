using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Tools;
using Northwoods.Go;
using Northwoods.Go.Draw;

namespace MetaBuilder.Graphing.Controllers
{
    public class HumanInterfaceController
    {
        #region Methods (1) 

        // Public Methods (1) 

        public void ReplaceMouseTools(GraphView myView)
        {
            myView.ReplaceMouseTool(typeof (GoDrawToolDragging), new CustomDraggingTool(myView));
            myView.ReplaceMouseTool(typeof (GoToolLinkingNew), new NewLinkTool(myView));
            myView.ReplaceMouseTool(typeof (GoToolRelinking), new ReLinkTool(myView));
            myView.ReplaceMouseTool(typeof (GoDrawToolRubberBanding), new GraphViewRubberbandingTool(myView));
            myView.ReplaceMouseTool(typeof (GoToolResizing), new GraphViewResizeTool(myView));
        }

        #endregion Methods 
    }
}