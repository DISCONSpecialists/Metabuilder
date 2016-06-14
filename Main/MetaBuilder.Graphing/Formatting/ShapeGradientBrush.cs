using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Formatting
{
    [Serializable]
    public class ShapeGradientBrush
    {
        #region Fields (7)

        public Color BorderColor;
        public GradientType GradientType;
        public Color InnerColor;
        public bool IsLinear;
        public PointF LinearEnd;
        public PointF LinearStart;
        public Color OuterColor;

        #endregion Fields

        #region Constructors (1)

        #endregion Constructors

        #region Methods (3)

        // Public Methods (3) 

        public void Apply(GoShape shp)
        {
            switch (GradientType)
            {
                case GradientType.Ellipse: // Path gradient
                    if (shp.Width == 0 || shp.Height == 0 || InnerColor == Color.Transparent)
                    {
                        shp.Brush = null;
                        break;
                    }
                    GraphicsPath path = new GraphicsPath();
                    // Create a path that consists of a single ellipse.
                    path.AddEllipse(shp.Left, shp.Top, shp.Width, shp.Height);
                    // Use the path to construct a brush.
                    PathGradientBrush pthGrBrush = new PathGradientBrush(path);
                    // Set the color at the center of the path to blue.
                    pthGrBrush.CenterColor = InnerColor;
                    Color[] colors = { OuterColor };
                    pthGrBrush.SurroundColors = colors;
                    //if (InnerColor != Color.Transparent)
                    shp.Brush = pthGrBrush;
                    //else
                    //shp.Brush = null;
                    break;
                default: // Linear gradient
                    if (shp.Width == 0 || shp.Height == 0 || InnerColor == Color.Transparent)
                    {
                        shp.Brush = null;
                        break;
                    }
                    //{
                    //RectangleF bounds = shp.Bounds;
                    //bounds.Width += 50;
                    //if (bounds.Height > 0 && bounds.Width > 0)
                    //{

                    //if (InnerColor != Color.Transparent)
                    //{
                    LinearGradientBrush lgbrush = new LinearGradientBrush(shp.Bounds, InnerColor, OuterColor, (LinearGradientMode)Enum.Parse(typeof(LinearGradientMode), GradientType.ToString()));
                    shp.Brush = lgbrush;
                    //}
                    //else
                    //{
                    //    shp.Brush = null;
                    //}
                    //}
                    //}
                    break;
            }
        }

        public ShapeGradientBrush Copy()
        {
            ShapeGradientBrush retval = new ShapeGradientBrush();
            retval.BorderColor = BorderColor;
            retval.GradientType = GradientType;
            retval.InnerColor = InnerColor;
            retval.IsLinear = IsLinear;
            retval.LinearEnd = LinearEnd;
            retval.LinearStart = LinearStart;
            retval.OuterColor = OuterColor;
            return retval;
            /* ShapeGradientBrush clone = this.MemberwiseClone() as ShapeGradientBrush;
            return clone as ShapeGradientBrush;*/
        }

        public void Disable(GoShape shp)
        {
            shp.Brush = null;
        }

        #endregion Methods
    }
}