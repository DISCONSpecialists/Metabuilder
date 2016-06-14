using System;
using System.Drawing;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Shapes
{
    [Serializable]
    public class NodeBalloon : GoBalloon
    {
        #region Constructors (1) 

        #endregion Constructors 

        #region Methods (1) 

        // Protected Methods (1) 

        protected override void PickNewAnchor(PointF p, GoView view, GoInputState evttype)
        {
            if (evttype == GoInputState.Finish)
            {
                GoObject picked = view.PickObject(true, false, p, true);
                if (picked != this && picked is IGoNode)
                {
                    // potential Anchor must be an IGoNode
                    Anchor = picked;
                    
                }
            }
        }

        #endregion Methods 
    }
}