using System;
using System.Drawing;
using MetaBuilder.Graphing.Persistence.XMLPersistence.Primitives;
using MetaBuilder.Graphing.Shapes;
namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Other
{
    public class HyperlinkTransformer : GoTextTransformer
    {

        #region Constructors (1)

        public HyperlinkTransformer()
            : base()
        {
            TransformerType = typeof(Hyperlink);
            ElementName = "hyperlink";
            IdAttributeUsedForSharedObjects = true;
            BodyConsumesChildElements = false;
        }

        #endregion Constructors

        #region Methods (3)

        // Public Methods (3) 

        public override object Allocate()
        {
            return new Hyperlink();
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            string args = StringAttr("Arguments", "");
            string linkType = StringAttr("HyperlinkType", Graphing.Shapes.HyperlinkType.URL.ToString());
            Hyperlink link = obj as Hyperlink;
            link.Arguments = args;
            link.HyperlinkType = (HyperlinkType)Enum.Parse(typeof(HyperlinkType), linkType);
            link.Text = StringAttr("LinkText", "");
            link.Bounds = RectangleFAttr("xy", new RectangleF());
            link.BookmarkName = StringAttr("BookmarkName", "");
            link.TextColor = Color.Blue;
            link.Editable = false;
            link.Underline = true;
        }

        public override void GenerateAttributes(object obj)
        {
            Hyperlink link = obj as Hyperlink;
            WriteAttrVal("Arguments", link.Arguments);
            WriteAttrVal("HyperlinkType", link.HyperlinkType.ToString());
            WriteAttrVal("LinkText", link.Text);
            WriteAttrVal("BookmarkName", link.BookmarkName);
            WriteAttrVal("xy", link.Bounds);
        }

        #endregion Methods

    }
}