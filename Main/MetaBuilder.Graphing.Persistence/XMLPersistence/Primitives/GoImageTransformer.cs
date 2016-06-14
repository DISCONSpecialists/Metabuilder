using Northwoods.Go;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Primitives
{
    public class GoImageTransformer:BaseGoObjectTransformer 
    {

		#region Constructors (1) 

        public GoImageTransformer()
            : base()
        {
            this.TransformerType = typeof(GoImage);
            this.ElementName = "Image";
            this.IdAttributeUsedForSharedObjects = true;
        }

		#endregion Constructors 

		#region Methods (1) 


		// Public Methods (1) 

        public override object Allocate()
        {
            return new GoImage(); 
        }


		#endregion Methods 

    }
}
