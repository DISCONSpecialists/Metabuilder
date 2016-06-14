using System.Drawing;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Primitives
{

    public class GoRoundedRectangleTransformer : BaseGoObjectTransformer
    {

		#region Constructors (1) 

        public GoRoundedRectangleTransformer()
            : base()
        {
            this.TransformerType = typeof(GoRoundedRectangle);
            this.ElementName = "GoRoundedRect";
            this.IdAttributeUsedForSharedObjects = true;
        }

		#endregion Constructors 

		#region Methods (3) 


		// Public Methods (3) 

        public override object Allocate()
        {
            GoRoundedRectangle obj = new GoRoundedRectangle();
            return obj;
        }

        
        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            
            GoRoundedRectangle shape = obj as GoRoundedRectangle;
            shape.Corner = SizeFAttr("Corner", new SizeF());
        }

        public override void GenerateAttributes(object obj)
        {
            GoRoundedRectangle shape = obj as GoRoundedRectangle;
            WriteAttrVal("Corner", shape.Corner);
        }


		#endregion Methods 

    }
}
