using System;
using MetaBuilder.Graphing.Shapes;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Primitives
{

    public class HouseShapeTransformer : GradientShapeTransformer
    {

		#region Constructors (1) 

        public HouseShapeTransformer()
            : base()
        {
            this.TransformerType = typeof(HouseShape);
            this.ElementName = "pmHouse";
            this.IdAttributeUsedForSharedObjects = true;
        }

		#endregion Constructors 

		#region Methods (3) 


		// Public Methods (3) 

        public override object Allocate()
        {
            HouseShape shape = new HouseShape();
            return shape;
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            HouseShape shape = (HouseShape)obj;
            shape.Direction = Int32Attr("Direction", 0);
            shape.Top = FloatAttr("Top", 1);
        }

        public override void GenerateAttributes(Object obj)
        {
            HouseShape shape = obj as HouseShape;
            base.GenerateAttributes(shape);
            WriteAttrVal("Direction", shape.Direction);
            WriteAttrVal("Top", shape.Top);
        }


		#endregion Methods 

    }
}
