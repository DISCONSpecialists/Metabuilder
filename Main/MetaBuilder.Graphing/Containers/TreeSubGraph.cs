using System;
using System.Drawing;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Containers
{
    [Serializable]
    public class TreeSubGraph : GoSubGraph
    {
        #region Constructors (1) 

        public TreeSubGraph()
        {
            LabelSpot = TopLeft; // instead of centered at top
            //Opacity = 50; // no background color
            Opacity = 100;
            TopLeftMargin = new SizeF(0, 0); // instead of default 4x4
            BottomRightMargin = new SizeF(0, 0); // instead of default 4x4
            BorderPen = Pens.Gray; // instead of none by default
            //  this.BackgroundColor = Color.GhostWhite;
            Handle.Printable = false;
            BackgroundColor = Color.Lavender;
            PickableBackground = true;
            CollapsedBottomRightMargin = new SizeF(5, 5);
            CollapsedCorner = new SizeF(5, 5);
            CollapsedTopLeftMargin = new SizeF(5, 5);
            Corner = new SizeF(5, 5);
            Deletable = true;
        }

        #endregion Constructors 

        #region Methods (1) 

        // Protected Methods (1) 

        // creating a port at the handle
        protected override GoPort CreatePort()
        {
            return null;
        }

        #endregion Methods 
    }
}