using System;
using System.Windows.Forms;
using MetaBuilder.Graphing.Shapes.Primitives;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Primitives
{

    public class CylinderTransformer : GradientShapeTransformer
    {

		#region Constructors (1) 

        public CylinderTransformer()
            : base()
        {
            this.TransformerType = typeof(GradientCylinder);
            this.ElementName = "pmCylinder";
            this.IdAttributeUsedForSharedObjects = true;
        }

		#endregion Constructors 

		#region Methods (3) 


		// Public Methods (3) 

        public override object Allocate()
        {
            GradientCylinder shape = new GradientCylinder();
            return shape;
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            GradientCylinder shape = (GradientCylinder)obj;
            shape.Perspective = (GoPerspective)Enum.Parse(typeof(GoPerspective), StringAttr("Perspective", GoPerspective.TopRight.ToString()));
            Type tOrientation = typeof(Orientation);
            shape.Orientation = (Orientation)Enum.Parse(tOrientation, StringAttr("Orientation",Orientation.Horizontal.ToString()));
            shape.MinorRadius = FloatAttr("MinorRadius", 1);
            shape.ResizableRadius = BooleanAttr("ResizableRadius", true);

        }

        public override void GenerateAttributes(Object obj)
        {
            GradientCylinder shape = obj as GradientCylinder;
            base.GenerateAttributes(shape);
            WriteAttrVal("MinorRadius", shape.MinorRadius.ToString());
            WriteAttrVal("Orientation", shape.Orientation.ToString());
            WriteAttrVal("Perspective", shape.Perspective.ToString());
            WriteAttrVal("ResizableRadius", shape.ResizableRadius);
            
        }


		#endregion Methods 

    }
}
