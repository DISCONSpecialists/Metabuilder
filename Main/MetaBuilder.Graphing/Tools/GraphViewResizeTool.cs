using MetaBuilder.Core;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Nodes.Containers;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Tools
{
    public class GraphViewResizeTool : GoToolResizing
    {
        #region Constructors (1)

        public GraphViewResizeTool(GoView v)
            : base(v)
        {
            View = v;
        }

        #endregion Constructors

        #region Methods (4)

        // Public Methods (3) 

        public override void Start()
        {
            if (Selection.Primary is MappingCell)
            {
                View.GridSnapResize = GoViewSnapStyle.Jump;
            }
            //October 2014 - to Set or Not to Set
            else if (Selection.Primary is QLink)
            {
                View.GridSnapResize = GoViewSnapStyle.None;
            }
            else
                ResetSnappingStyle();
            base.Start();
        }

        public override void Stop()
        {
            ResetSnappingStyle();
            base.Stop();
        }

        // Private Methods (1) 

        private void ResetSnappingStyle()
        {
            MetaSettings mS = new MetaSettings();
            View.GridSnapResize = (mS.GetSetting(MetaSettings.VIEW_SNAPRESIZE, false)) ? GoViewSnapStyle.Jump : GoViewSnapStyle.None;
        }

        #endregion Methods
    }
}