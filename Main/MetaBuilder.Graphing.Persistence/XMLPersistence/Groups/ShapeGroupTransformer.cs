using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Graphing.Persistence.XMLPersistence.Nodes;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Groups
{
    public class ShapeGroupTransformer : BaseTransformer
    {

		#region Constructors (1) 

        public ShapeGroupTransformer()
            : base()
        {
            this.TransformerType = typeof(ShapeGroup);
            this.ElementName = "shapegroup";
            this.BodyConsumesChildElements = true;
            this.IdAttributeUsedForSharedObjects = true;
        }

		#endregion Constructors 

		#region Methods (2) 


		// Public Methods (2) 

        public override void ConsumeChild(object parent, object child)
        {
            if (parent is GoGroup && child is GoObject)
            {
                if (!(parent is IMetaNode))
                {
                    GoObject o = child as GoObject;
                    o.Remove();

                    GoGroup grp = parent as GoGroup;
                    grp.Add(o);
                    o.DragsNode = true;
                }
            }
        }

        public override void GenerateAttributes(object obj)
        {
            //base.GenerateAttributes(obj);
        }


		#endregion Methods 

    }

}
