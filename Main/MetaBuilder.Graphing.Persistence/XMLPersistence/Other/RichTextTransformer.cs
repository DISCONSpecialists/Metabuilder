using MetaBuilder.Graphing.Shapes;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Other
{
    public class RichTextTransformer : BaseGoObjectTransformer 
    {

		#region Constructors (1) 

        public RichTextTransformer():base()
        {
            this.TransformerType = typeof(RichText);
            this.IdAttributeUsedForSharedObjects = true;
            this.BodyConsumesChildElements = false;
            this.ElementName = "RTF";
        }

		#endregion Constructors 

		#region Methods (3) 

		// Public Methods (3) 

        public override object Allocate()
        {
            return new RichText();
        }

        public override void ConsumeAttributes(object obj)
        {
            RichText lbl = obj as RichText;
            lbl.Rtf = StringAttr("Content", "");
            base.ConsumeAttributes(obj);
        }

        public override void GenerateAttributes(object obj)
        {
            base.GenerateAttributes(obj,true);
            RichText lbl = obj as RichText;
            WriteAttrVal("Content", lbl.Rtf);
        }

		#endregion Methods 

    }
}
