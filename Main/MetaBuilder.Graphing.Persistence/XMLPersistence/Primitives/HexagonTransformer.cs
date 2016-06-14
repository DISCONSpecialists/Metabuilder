using System;
using System.Windows.Forms;
using MetaBuilder.Graphing.Shapes.Primitives;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Primitives
{

    public class HexagonTransformer : GradientShapeTransformer
    {

		#region Constructors (1) 

        public HexagonTransformer()
            : base()
        {
            this.TransformerType = typeof(GradientHexagon);
            this.ElementName = "pmHexagon";
            this.IdAttributeUsedForSharedObjects = true;
        }

		#endregion Constructors 

		#region Methods (3) 


		// Public Methods (3) 

        public override object Allocate()
        {
            GradientHexagon shape = new GradientHexagon();
            return shape;
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            GradientHexagon shape = (GradientHexagon)obj;

            shape.DistanceBottom = FloatAttr("DistanceBottom",0);
            shape.DistanceTop = FloatAttr("DistanceTop", 0);
            shape.DistanceLeft = FloatAttr("DistanceLeft", 0);
            shape.DistanceRight =FloatAttr("DistanceRight", 0);
            shape.KeepsCrosswiseSymmetry = BooleanAttr("KeepsCrosswiseSymmetry", true);
            shape.KeepsLengthwiseSymmetry = BooleanAttr("KeepsLengthwiseSymmetry", true);
            Type tOrientation = typeof(Orientation);
            shape.Orientation = (Orientation)Enum.Parse(tOrientation, StringAttr("Orientaton", Orientation.Horizontal.ToString()));
            shape.ReshapableCorner = BooleanAttr("ReshapableCorner", true);
            Type tReshapeBehaviour = typeof(GoHexagonReshapeBehavior);
            shape.ReshapeBehavior = (GoHexagonReshapeBehavior)Enum.Parse(tReshapeBehaviour, StringAttr("ReshapeBehavior", GoHexagonReshapeBehavior.CompleteSymmetry.ToString()));

        }

        public override void GenerateAttributes(Object obj)
        {
            GradientHexagon shape = obj as GradientHexagon;
            base.GenerateAttributes(shape);
            
            WriteAttrVal("DistanceBottom", shape.DistanceBottom.ToString());
            WriteAttrVal("DistanceTop", shape.DistanceTop.ToString());
            WriteAttrVal("DistanceLeft", shape.DistanceLeft.ToString());
            WriteAttrVal("DistanceRight", shape.DistanceRight.ToString());
            WriteAttrVal("KeepsCrosswiseSymmetry", shape.KeepsCrosswiseSymmetry);
            WriteAttrVal("KeepsLengthwiseSymmetry", shape.KeepsLengthwiseSymmetry);
            WriteAttrVal("Orientation", shape.Orientation.ToString());
            WriteAttrVal("ReshapableCorner", shape.ReshapableCorner.ToString());
            WriteAttrVal("ReshapeBehavior", shape.ReshapeBehavior.ToString());
        }


		#endregion Methods 

    }
}
