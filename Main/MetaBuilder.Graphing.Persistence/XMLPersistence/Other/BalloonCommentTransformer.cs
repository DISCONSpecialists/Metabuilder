using System;
using System.Drawing;
using System.Net.Mime;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Meta;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Other
{
    public class BalloonCommentTransformer : BaseGoObjectTransformer
    {

        #region Fields (1)

        private GoObject anchor;

        #endregion Fields

        #region Constructors (1)

        public BalloonCommentTransformer()
            : base()
        {
            this.TransformerType = typeof(ResizableBalloonComment);
            this.ElementName = "BaloonComment";
            this.BodyConsumesChildElements = false;
            this.IdAttributeUsedForSharedObjects = true;
        }

        #endregion Constructors

        #region Methods (5)

        // Public Methods (5) 

        public override object Allocate()
        {
            ResizableBalloonComment x = new ResizableBalloonComment();
            x.Reanchorable = true;
            return x;
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            ResizableBalloonComment x = obj as ResizableBalloonComment;
            //x.Label.AutoResizes = false;
            x.Text = StringAttr("txt", "");
            string sMargins = StringAttr("marginTopLeft", "");
            if (sMargins.Length > 0)
            {
                x.TopLeftMargin = SizeFAttr("marginTopLeft", new SizeF());
                x.BottomRightMargin = SizeFAttr("marginBottomRight", new SizeF());
            }
            //x.UpdateSizeFromLabelBounds();
        }

        public override void ConsumeObjectFinish(object obj)
        {
            base.ConsumeObjectFinish(obj);
            ResizableBalloonComment x = obj as ResizableBalloonComment;
            String anchorid = StringAttr("anchor", null);
            if (anchorid != null)
            {

                anchor = this.Reader.FindShared(anchorid) as GoObject;
                if (anchor == null)
                {
                    Reader.AddDelayedRef(obj, "anchor", anchorid);
                }
                else
                {
                    x.Anchor = anchor;
                }
            }

            x.Bounds = RectangleFAttr("xy", new RectangleF());
            PointF labelPos = new PointF(x.Bounds.X + x.TopLeftMargin.Width, x.Bounds.Y + x.TopLeftMargin.Height);

            x.Label.Bounds = new RectangleF(labelPos, new SizeF(x.Bounds.Width - x.TopLeftMargin.Width - x.BottomRightMargin.Width, x.Bounds.Height - x.TopLeftMargin.Height - x.BottomRightMargin.Height));

            if (!(x is MetaBuilder.Graphing.Shapes.Nodes.Rationale))
            {
                if (x.Background is GoShape)
                {
                    (x.Background as GoShape).Brush = new SolidBrush(Color.FromArgb(230, 230, 250));
                }
            }

            x.ViewerAdded = BooleanAttr("viewer", false);
        }

        public override void GenerateAttributes(object obj)
        {
            base.GenerateAttributes(obj, true);
            ResizableBalloonComment x = obj as ResizableBalloonComment;
            WriteAttrVal("txt", x.Text);

            if (x.Anchor != null)
            {
                if (x.Anchor is CollapsingRecordNodeItem)
                {
                    CollapsingRecordNodeItem item = x.Anchor as CollapsingRecordNodeItem;
                    if (item.MetaObject != null)
                        WriteAttrVal("anchor", this.Writer.MakeShared(item.MetaObject));
                }
                else
                    WriteAttrVal("anchor", this.Writer.MakeShared(x.Anchor));
            }
            SizeF marTopLeft = x.TopLeftMargin;
            SizeF marBottomRight = x.BottomRightMargin;
            WriteAttrVal("marginBottomRight", marBottomRight);
            WriteAttrVal("marginTopLeft", marTopLeft);
            WriteAttrVal("viewer", x.ViewerAdded);
            x.Printable = Core.Variables.Instance.PrintComments;
        }

        public override void UpdateReference(object obj, string prop, object referred)
        {
            if (prop == "anchor")
            {
                GoObject anchoredTo = referred as GoObject;
                if (anchoredTo != null)
                {
                    ResizableBalloonComment x = obj as ResizableBalloonComment;
                    x.Anchor = anchoredTo;
                }

                if (referred is MetaBase)
                {
                    MetaBase mbReferred = referred as MetaBase;
                    ResizableBalloonComment x = obj as ResizableBalloonComment;
                    x.Anchor = mbReferred.tag as GoObject;
                }
            }
        }

        #endregion Methods

    }
}