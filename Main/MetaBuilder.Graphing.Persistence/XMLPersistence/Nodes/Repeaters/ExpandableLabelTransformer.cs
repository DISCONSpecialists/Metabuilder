using System;
using MetaBuilder.Graphing.Persistence.XMLPersistence.Primitives;
using MetaBuilder.Graphing.Shapes.Nodes;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Nodes.Repeaters
{
    public class ExpandableLabelTransformer : EmbeddedObjectsTransformer
    {

		#region Constructors (1) 

        public ExpandableLabelTransformer()
            : base()
        {
            this.TransformerType = typeof(ExpandableLabel);
            this.ElementName = "expLabel";
           
        }

		#endregion Constructors 

		#region Methods (1) 


		// Public Methods (1) 

        public override object Allocate()
        {
#if DEBUG
            // Console.WriteLine("ExpandableLabel - Allocate");
#endif

                         return new ExpandableLabel();
        }


		#endregion Methods 

    }
}
