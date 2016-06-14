using System;
using System.Drawing;
using MetaBuilder.Graphing.Shapes.Primitives;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Primitives
{

    public class OctagonTransformer : GradientShapeTransformer
    {

		#region Constructors (1) 

        public OctagonTransformer()
            : base()
        {
            this.TransformerType = typeof(GradientOctagon);
            this.ElementName = "pmOctagon";
            this.IdAttributeUsedForSharedObjects = true;
        }

		#endregion Constructors 

		#region Methods (3) 


		// Public Methods (3) 

        public override object Allocate()
        {
            GradientOctagon shape = new GradientOctagon();
            return shape;
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            GradientOctagon shape = (GradientOctagon)obj;
            shape.Corner = SizeFAttr("Corner", new SizeF(0, 0));
            shape.ReshapableCorner = BooleanAttr("ReshapableCorner", true);
        }

        public override void GenerateAttributes(Object obj)
        {
            GradientOctagon shape = obj as GradientOctagon;
            base.GenerateAttributes(shape);
            WriteAttrVal("Corner", shape.Corner);
            WriteAttrVal("ReshapableCorner", shape.ReshapableCorner);
        }


		#endregion Methods 

    }
}
