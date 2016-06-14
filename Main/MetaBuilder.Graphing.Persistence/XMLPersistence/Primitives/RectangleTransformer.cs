using System;
using System.Drawing;
using MetaBuilder.Graphing.Shapes.Primitives;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Primitives
{

    public class RectangleTransformer : GradientShapeTransformer
    {

        #region Constructors (1)

        public RectangleTransformer()
            : base()
        {
            this.TransformerType = typeof(GradientRoundedRectangle);
            this.ElementName = "pmRectangle";
            this.IdAttributeUsedForSharedObjects = true;
        }

        #endregion Constructors

        #region Methods (1)

        // Public Methods (1) 

        public override object Allocate()
        {
            GradientRoundedRectangle shape = new GradientRoundedRectangle();
            return shape;
        }

        #endregion Methods

    }
}