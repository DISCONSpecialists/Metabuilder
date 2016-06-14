using System.Drawing;
using System.Drawing.Drawing2D;
using Northwoods.Go;
using System;

namespace MetaBuilder.Graphing.Shapes.Primitives
{
    public class HighlightIndicator : GoRoundedRectangle
    {
        public HighlightIndicator(bool dashPen)
        {
            // false will remove the highlight

            Pen p = new Pen(Color.Orange);
            if (dashPen)
                p.DashStyle = DashStyle.DashDot;
            Pen = p;
            Pen.Width = 5;
            Selectable = false;
        }

    }
}