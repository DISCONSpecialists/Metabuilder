using System;
using System.Drawing;
using MetaBuilder.Graphing.Shapes.Primitives;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Primitives
{

    public class CubeTransformer : GradientShapeTransformer
    {

		#region Constructors (1) 

        public CubeTransformer()
            : base()
        {
            this.TransformerType = typeof(GradientCube);
            this.ElementName = "pmCube";
            this.IdAttributeUsedForSharedObjects = true;
        }

		#endregion Constructors 

		#region Methods (3) 


		// Public Methods (3) 

        public override object Allocate()
        {
            GradientCube cube = new GradientCube();
            return cube;
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            GradientCube cube = (GradientCube)obj;
            cube.Depth = SizeFAttr("Depth", new SizeF(1, 1));
            cube.Perspective = (GoPerspective)Enum.Parse(typeof(GoPerspective), StringAttr("Perspective", GoPerspective.TopRight.ToString()));

        }

        public override void GenerateAttributes(Object obj)
        {
            GradientCube qp = obj as GradientCube;
            base.GenerateAttributes(qp);
            WriteAttrVal("Perspective", qp.Perspective.ToString());
            WriteAttrVal("Depth", qp.Depth);
        }


		#endregion Methods 

    }
}
