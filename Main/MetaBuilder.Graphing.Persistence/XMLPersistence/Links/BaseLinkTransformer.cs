using System.Drawing;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;
using Northwoods.Go.Xml;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Links
{
    public class BaseLinkTransformer : BaseTransformer
    {

		#region Constructors (1) 

        public BaseLinkTransformer()
            : base()
        {
            this.ElementName = "BaseLink";
        }

		#endregion Constructors 

		#region Methods (2) 


		// Public Methods (2) 

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            PointF[] pts = PointFArrayAttr("Points", new PointF[] { });
            GoLink link = obj as GoLink;
            link.Relinkable = true;
            link.SetPoints(pts);
        }

        public override void GenerateAttributes(object obj)
        {
            base.GenerateAttributes(obj);
            GoLink link = obj as GoLink;
            IGoNode nodeFrom = link.FromNode;
            IGoNode nodeTo = link.ToNode;
            PointF[] points = link.CopyPointsArray();
            WriteAttrVal("Points", points);
        }

		#endregion Methods 

    }
}