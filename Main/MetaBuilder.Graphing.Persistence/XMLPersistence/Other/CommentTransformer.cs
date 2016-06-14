using System.Drawing;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Other
{
    public class CommentTransformer : BaseGoObjectTransformer
    {

        #region Constructors (1)

        public CommentTransformer()
            : base()
        {
            this.TransformerType = typeof(ResizableComment);
            this.ElementName = "Comment";
            this.BodyConsumesChildElements = false;
            this.IdAttributeUsedForSharedObjects = true;
        }

        #endregion Constructors

        #region Methods (4)


        // Public Methods (4) 

        public override object Allocate()
        {
            return new ResizableComment();
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            ResizableComment x = obj as ResizableComment;
            x.Text = StringAttr("txt", "");

            string sMargins = StringAttr("marginTopLeft", "");
            if (sMargins.Length > 0)
            {
                x.TopLeftMargin = SizeFAttr("marginTopLeft", new SizeF());
                x.BottomRightMargin = SizeFAttr("marginBottomRight", new SizeF());
            }
        }

        public override void ConsumeObjectFinish(object obj)
        {
            base.ConsumeObjectFinish(obj);
            ResizableComment x = obj as ResizableComment;
            string comBounds = StringAttr("comBounds", "");
            if (comBounds.Length > 0)
            {
                x.Bounds = RectangleFAttr("comBounds", new RectangleF());
            }
            else
            {
                x.Bounds = RectangleFAttr("xy", new RectangleF());
            }
            x.Position = new PointF(x.Bounds.X, x.Bounds.Y);
            x.Visible = true;
            x.Editable = false;
            x.Label.Editable = false;
            /*    PointF labelPos = new PointF(x.Bounds.X + x.TopLeftMargin.Width, x.Bounds.Y + x.TopLeftMargin.Height);

                x.Label.Bounds =
                    new RectangleF(labelPos,
                                   new SizeF(x.Bounds.Width - x.TopLeftMargin.Width - x.BottomRightMargin.Width,
                                             x.Bounds.Height - x.TopLeftMargin.Height - x.BottomRightMargin.Height));*/
            if (x.Background is GoShape)
            {
                (x.Background as GoShape).Brush = new SolidBrush(Color.FromArgb(230, 230, 250));
            }

            x.ViewerAdded = BooleanAttr("viewer", false);
        }

        public override void GenerateAttributes(object obj)
        {
            ResizableComment x = obj as ResizableComment;

            base.GenerateAttributes(obj, true);
            WriteAttrVal("txt", x.Text);
            x.Printable = Core.Variables.Instance.PrintComments;

            SizeF newSize = new SizeF(x.Label.Bounds.Width + x.TopLeftMargin.Width + x.BottomRightMargin.Width, x.Label.Bounds.Height + x.TopLeftMargin.Height + x.BottomRightMargin.Height);

            RectangleF bounds = new RectangleF(x.Position, newSize);
            WriteAttrVal("comBounds", bounds);
            SizeF marTopLeft = x.TopLeftMargin;
            SizeF marBottomRight = x.BottomRightMargin;
            WriteAttrVal("marginBottomRight", marBottomRight);
            WriteAttrVal("marginTopLeft", marTopLeft);
            WriteAttrVal("viewer", x.ViewerAdded);
        }

        #endregion Methods

    }
}
