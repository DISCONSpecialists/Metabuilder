using Northwoods.Go;
using Northwoods.Go.Draw;

namespace MetaBuilder.Graphing.Tools
{
    public class GraphViewRubberbandingTool : GoDrawToolRubberBanding
    {
        #region Constructors (1) 

        public GraphViewRubberbandingTool(GoView view)
            : base(view)
        {
            AutoScrolling = true;
        }

        #endregion Constructors 
    }
}