using System;
using MetaBuilder.Graphing.Shapes.Primitives;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Primitives
{

    public class DiamondTransformer : GradientShapeTransformer
    {

		#region Constructors (1) 

        public DiamondTransformer()
            : base()
        {
            this.TransformerType = typeof(GradientDiamond );
            this.ElementName = "pmDiamond";
            this.IdAttributeUsedForSharedObjects = true;
        }

		#endregion Constructors 

		#region Methods (3) 


		// Public Methods (3) 

        public override object Allocate()
        {
            GradientDiamond shape = new GradientDiamond();
            return shape;
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);

        }

        public override void GenerateAttributes(Object obj)
        {
            GradientDiamond shape = obj as GradientDiamond;
            base.GenerateAttributes(shape);

        }


		#endregion Methods 

    }
}
