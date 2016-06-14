using System;
using System.Drawing;
using MetaBuilder.Graphing.Shapes.Primitives;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Primitives
{

    public class ParalellogramTransformer : GradientShapeTransformer
    {

		#region Constructors (1) 

        public ParalellogramTransformer()
            : base()
        {
            this.TransformerType = typeof(GradientParallelogram);
            this.ElementName = "gradientcube";
            this.IdAttributeUsedForSharedObjects = true;
        }

		#endregion Constructors 

		#region Methods (3) 


		// Public Methods (3) 

        public override object Allocate()
        {
            GradientParallelogram shape = new GradientParallelogram();
            return shape;
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            GradientParallelogram shape = (GradientParallelogram)obj;
            shape.Direction = BooleanAttr("Direction", true);
            shape.ReshapableSkew = BooleanAttr("ReshapableSkew", true);
            shape.Skew = SizeFAttr("Skew", new SizeF());
        }

        public override void GenerateAttributes(Object obj)
        {
            GradientParallelogram shape = obj as GradientParallelogram;
            base.GenerateAttributes(shape);
            WriteAttrVal("Direction", shape.Direction);
            WriteAttrVal("ReshapableSkew", shape.ReshapableSkew);
            WriteAttrVal("Skew", shape.Skew);
        }


		#endregion Methods 

    }
}
