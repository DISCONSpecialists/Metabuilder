using System;
using System.Drawing;
using MetaBuilder.Graphing.Shapes.Nodes.Containers;
using MetaBuilder.Graphing.Shapes.Primitives;

using Northwoods.Go;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Primitives
{

    public class GradientPolygonTransformer : GradientShapeTransformer
    {

		#region Constructors (1) 

        public GradientPolygonTransformer()
            : base()
        {
            this.TransformerType = typeof(GradientPolygon);
            this.ElementName = "pmPolygon";
            this.IdAttributeUsedForSharedObjects = true;
        }

		#endregion Constructors 

		#region Methods (3) 


		// Public Methods (3) 

        public override object Allocate()
        {
            GradientPolygon shape = new GradientPolygon(GoPolygonStyle.Bezier);
            return shape;
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            GradientPolygon shape = obj as GradientPolygon;

            string style = StringAttr("style", GoPolygonStyle.Bezier.ToString());
            Type t = typeof(GoPolygonStyle);
            shape.Style = (GoPolygonStyle)Enum.Parse(t, style);
               PointF[] pts = PointFArrayAttr("Points", new PointF[] { });

            shape.SetPoints(pts);

        }

        public override void GenerateAttributes(Object obj)
        {
            GradientPolygon shape = obj as GradientPolygon;
            if (obj.GetType().FullName == typeof(GradientPolygon).FullName)
                base.GenerateAttributes(obj);
            WriteAttrVal("style", shape.Style.ToString());
            WriteAttrVal("Points", shape.CopyPointsArray());
#if DEBUG
           // TraceTool.TTrace.Debug.Send("generating for polygon");
#endif

        }


		#endregion Methods 

    }

    /*
    public class ValueChainShapeTransformer : GradientShapeTransformer
    {

		#region Constructors (1) 

        public ValueChainShapeTransformer()
            : base()
        {
            this.TransformerType = typeof(ValueChainShape);
            this.ElementName = "vcShape";
            this.IdAttributeUsedForSharedObjects = true;
        }

		#endregion Constructors 

		#region Methods (3) 


		// Public Methods (3) 

        public override object Allocate()
        {
            ValueChainShape shape = new ValueChainShape(1, 1);
            return shape;
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            ValueChainShape shape = obj as ValueChainShape;

            string style = StringAttr("style", GoPolygonStyle.Bezier.ToString());
            Type t = typeof(GoPolygonStyle);
            shape.Style = (GoPolygonStyle)Enum.Parse(t, style);
            PointF[] pts = PointFArrayAttr("Points", new PointF[] { });

            shape.SetPoints(pts);
            shape.DrawStep();

        }

        public override void GenerateAttributes(Object obj)
        {
            ValueChainShape shape = obj as ValueChainShape;
#if DEBUG
         //   TraceTool.TTrace.Debug.Send("generating for valuechainshape");
#endif
            base.GenerateAttributes(obj,true);

        }


		#endregion Methods 

    }*/

}
