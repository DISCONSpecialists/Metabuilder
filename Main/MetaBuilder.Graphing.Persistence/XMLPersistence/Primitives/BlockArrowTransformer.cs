using System;
using System.Drawing;
using MetaBuilder.Graphing.Shapes.Primitives;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Primitives
{

    public class BlockArrowTransformer : BaseTransformer
    {

		#region Constructors (1) 

        public BlockArrowTransformer()
            : base()
        {
            this.TransformerType = typeof(GradientValueChainStep);
            this.ElementName = "pmBlockArrow";
            this.IdAttributeUsedForSharedObjects = false;
        }

		#endregion Constructors 

		#region Methods (3) 


		// Public Methods (3) 

        public override object Allocate()
        {
            GradientValueChainStep shape = new GradientValueChainStep(1, 1, 1, 1, true);
            shape.Selectable = true;
            return shape;
        }

        public override void ConsumeAttributes(object obj)
        {
           
            GradientValueChainStep shape = (GradientValueChainStep)obj;

            string style = StringAttr("style", GoPolygonStyle.Bezier.ToString());
            Type t = typeof(GoPolygonStyle);
            shape.Style = (GoPolygonStyle)Enum.Parse(t, style);
           

          
            shape.Nose = FloatAttr("Nose", 0);
            shape.Tail = FloatAttr("Tail", 0);
            PointF[] pts = PointFArrayAttr("Points", new PointF[] { });
            shape.SetPoints(pts);
            base.ConsumeAttributes(obj);
            shape.Resizable = true;
            shape.Selectable = true;


        }

        public override void GenerateAttributes(Object obj)
        {
            GradientValueChainStep shape = obj as GradientValueChainStep;
           base.GenerateAttributes(shape);
            WriteAttrVal("Nose", shape.Nose.ToString());
            WriteAttrVal("Tail", shape.Tail.ToString());
         
        }


		#endregion Methods 

    }
}
