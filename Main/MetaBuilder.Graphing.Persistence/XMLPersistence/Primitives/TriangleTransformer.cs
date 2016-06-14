using System;
using System.Drawing;
using MetaBuilder.Graphing.Shapes.Primitives;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Primitives
{

    public class TriangleTransformer : GradientShapeTransformer
    {

		#region Constructors (1) 

        public TriangleTransformer()
            : base()
        {
            this.TransformerType = typeof(GradientTriangle);
            this.ElementName = "pmTriangle";
            this.IdAttributeUsedForSharedObjects = true;
        }

		#endregion Constructors 

		#region Methods (3) 


		// Public Methods (3) 

        public override object Allocate()
        {
            GradientTriangle shape = new GradientTriangle();
            return shape;
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            GradientTriangle shape = (GradientTriangle)obj;
            shape.A = PointFAttr("A", new PointF());
            shape.B = PointFAttr("B", new PointF());
            shape.C = PointFAttr("C", new PointF());
        }

        public override void GenerateAttributes(Object obj)
        {
            GradientTriangle shape = obj as GradientTriangle;
            base.GenerateAttributes(shape);
            WriteAttrVal("A", shape.A);
            WriteAttrVal("B", shape.B);
            WriteAttrVal("C", shape.C);
        }


		#endregion Methods 

    }
}
