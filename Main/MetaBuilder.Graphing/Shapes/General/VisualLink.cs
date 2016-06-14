using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MetaBuilder.Graphing.Shapes.General
{
    [Serializable]
    public class VisualLink : GoLabeledLink
    {
        public VisualLink()
        {
        }

        private void setupLink()
        {
            AvoidsNodes = true;
            Style = GoStrokeStyle.Bezier;
            Relinkable = false;
            Shadowed = false;
            Selectable = true;
            Deletable = true;
            Reshapable = true;
            Resizable = true;
        }

        public void SetStyle(DashStyle style)
        {
            setupLink();

            Pen p = new Pen(Brushes.Black);
            p.Width = 2f;
            p.DashStyle = style;
            Pen = p;
        }

    }
}