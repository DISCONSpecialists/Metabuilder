using MetaBuilder.Graphing.Shapes;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Links
{
    public class QRealLinkTransformer : BaseLinkTransformer
    {

		#region Constructors (1) 

        public QRealLinkTransformer()
            : base()
        {
            this.ElementName = "reallink";
            this.TransformerType = typeof(QRealLink);
            this.BodyConsumesChildElements = false;
            this.IdAttributeUsedForSharedObjects = true;
        }

		#endregion Constructors 

		#region Methods (2) 


		// Public Methods (2) 

        public override object Allocate()
        {
            QRealLink slink = new QRealLink();
            return base.Allocate();
        }

        public override void GenerateAttributes(object obj)
        {
            base.GenerateAttributes(obj);
        }


		#endregion Methods 

    }
}
