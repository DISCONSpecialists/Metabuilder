using System;
using System.Drawing;
using MetaBuilder.Graphing.Shapes.Primitives;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Primitives
{

    public class TrapezoidTransformer : GradientShapeTransformer
    {

		#region Constructors (1) 

        public TrapezoidTransformer()
            : base()
        {
            this.TransformerType = typeof(GradientTrapezoid);
            this.ElementName = "pmTrapezoid";
            this.IdAttributeUsedForSharedObjects = true;
           
        }

		#endregion Constructors 

		#region Methods (3) 


		// Public Methods (3) 

        public override object Allocate()
        {
            GradientTrapezoid shape = new GradientTrapezoid();
            return shape;
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            GradientTrapezoid shape = (GradientTrapezoid)obj;
            shape.A = PointFAttr("A", new PointF());
            shape.B = PointFAttr("B", new PointF());
            shape.C = PointFAttr("C", new PointF());
            shape.D = PointFAttr("D", new PointF());

        }

        public override void GenerateAttributes(Object obj)
        {
            GradientTrapezoid shape = obj as GradientTrapezoid;
            base.GenerateAttributes(shape);

            WriteAttrVal("A", shape.A);
            WriteAttrVal("B", shape.B);
            WriteAttrVal("C", shape.C);
            WriteAttrVal("D", shape.D);
        }


		#endregion Methods 

    }
}
