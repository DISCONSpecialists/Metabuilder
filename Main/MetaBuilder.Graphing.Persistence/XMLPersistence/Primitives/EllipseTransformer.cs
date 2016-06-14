using System;
using MetaBuilder.Graphing.Shapes.Primitives;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Primitives
{

    public class EllipseTransformer : GradientShapeTransformer
    {

		#region Constructors (1) 

        public EllipseTransformer()
            : base()
        {
            this.TransformerType = typeof(GradientEllipse);
            this.ElementName = "pmEllipse";
            this.IdAttributeUsedForSharedObjects = true;
        }

		#endregion Constructors 

		#region Methods (3) 


		// Public Methods (3) 

        public override object Allocate()
        {
            GradientEllipse shape = new GradientEllipse();
            return shape;
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
           
        }

        public override void GenerateAttributes(Object obj)
        {
            GradientEllipse shape = obj as GradientEllipse;
            base.GenerateAttributes(shape);


            //base.GenerateAttributes(obj,true);

        }


		#endregion Methods 

    }
}
