using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using MetaBuilder.Graphing.Shapes.Primitives;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Primitives
{

    public class StrokeTransformer : GradientShapeTransformer
    {

		#region Constructors (1) 

        public StrokeTransformer()
            : base()
        {
            this.TransformerType = typeof(QuickStroke);
            this.ElementName = "pmStroke";
            this.IdAttributeUsedForSharedObjects = true;
           
        }

		#endregion Constructors 

		#region Methods (3) 


		// Public Methods (3) 

        public override object Allocate()
        {
            QuickStroke shape = new QuickStroke();
            return shape;
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            QuickStroke shape = (QuickStroke)obj;

            PointF[] points = PointFArrayAttr("Points", new PointF[] { });
           

            shape.Curviness = FloatAttr("Curviness", 0);
       
            shape.FromArrow  = BooleanAttr("FromArrow",false);
            shape.FromArrowFilled = BooleanAttr("FromArrowFilled", false);
            shape.FromArrowLength = FloatAttr("FromArrowLength", 0);
            shape.FromArrowShaftLength = FloatAttr("FromArrowShaftLength", 0);
            shape.FromArrowWidth = FloatAttr("FromArrowWidth", 0);
            Type tArrowHeadStyle = typeof(GoStrokeArrowheadStyle);
            shape.FromArrowStyle = (GoStrokeArrowheadStyle)Enum.Parse(tArrowHeadStyle, StringAttr("FromArrowStyle", GoStrokeArrowheadStyle.Cross.ToString()));

            shape.ToArrow = BooleanAttr("ToArrow", false);
            shape.ToArrowFilled = BooleanAttr("ToArrowFilled", false);
            shape.ToArrowLength = FloatAttr("ToArrowLength", 0);
            shape.ToArrowShaftLength = FloatAttr("ToArrowShaftLength", 0);
            shape.ToArrowWidth = FloatAttr("ToArrowWidth", 0);
            shape.ToArrowStyle = (GoStrokeArrowheadStyle)Enum.Parse(tArrowHeadStyle, StringAttr("ToArrowStyle", GoStrokeArrowheadStyle.Cross.ToString()));

            shape.Highlight = BooleanAttr("Highlight",false);
            Type tPenDashStyle = typeof(DashStyle);
            Type tPenDashCap = typeof(DashCap);
            
         /*   try
            {
                Color hlPenColor = ColorAttr("HighlightPenColor", Color.Black);
                float hlPenWidth = FloatAttr("HighlightPenWidth", 1);
                Pen hlPen = new Pen(hlPenColor, hlPenWidth);
                shape.HighlightPen = hlPen;
                hlPen.DashStyle = (DashStyle)Enum.Parse(tPenDashStyle, StringAttr("HighlightPenDashStyle", DashStyle.Solid.ToString()));
                hlPen.DashCap = (DashCap)Enum.Parse(tPenDashCap, StringAttr("HighlightPenDashCap", DashCap.Flat.ToString()));
            }
            catch
            {
            }*/
            shape.Bounds = RectangleFAttr("xy", new RectangleF());

            shape.Width = float.Parse(StringAttr("PenWidth", "0"), System.Globalization.CultureInfo.InvariantCulture); 
            Type tStyle = typeof(GoStrokeStyle);
            shape.Style = (GoStrokeStyle)Enum.Parse(tStyle, StringAttr("Style", GoStrokeStyle.Line.ToString()));
            shape.SetPoints(points);
        }

        public override void GenerateAttributes(Object obj)
        {
            QuickStroke shape = obj as QuickStroke;
            base.GenerateAttributes(shape);
            WriteAttrVal("Style", shape.Style.ToString());
            WriteAttrVal("Points",shape.CopyPointsArray());
            WriteAttrVal("Curviness", shape.Curviness);
            WriteAttrVal("FromArrow", shape.FromArrow);
            WriteAttrVal("FromArrowFilled", shape.FromArrowFilled);
            WriteAttrVal("FromArrowLength", shape.FromArrowLength);
            WriteAttrVal("FromArrowShaftLength", shape.FromArrowShaftLength);
            WriteAttrVal("FromArrowStyle", shape.FromArrowStyle.ToString());
            WriteAttrVal("FromArrowWidth", shape.FromArrowWidth);

            WriteAttrVal("ToArrow", shape.ToArrow);
            WriteAttrVal("ToArrowFilled", shape.ToArrowFilled);
            WriteAttrVal("ToArrowLength", shape.ToArrowLength);
            WriteAttrVal("ToArrowShaftLength", shape.ToArrowShaftLength);
            WriteAttrVal("ToArrowStyle", shape.ToArrowStyle.ToString());
            WriteAttrVal("ToArrowWidth", shape.ToArrowWidth);

            // TODO: Pen & Highlight
          /*  WriteAttrVal("Highlight", shape.Highlight);
            if (shape.HighlightPen != null)
            {
                WriteAttrVal("HighlightPenColor", shape.HighlightPen.Color);
                WriteAttrVal("HighlightPenWidth", shape.HighlightPen.Width);
                WriteAttrVal("HighlightPenDashStyle", shape.HighlightPen.DashStyle.ToString());
                WriteAttrVal("HighlightPenDashCap", shape.HighlightPen.DashCap.ToString());
            }
            */
        }


		#endregion Methods 

    }
}
