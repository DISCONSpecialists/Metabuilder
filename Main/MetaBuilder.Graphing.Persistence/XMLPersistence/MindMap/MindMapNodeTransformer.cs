using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;
using System.Drawing;
using MetaBuilder.Graphing.Shapes.Nodes;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.MindMap
{
    public class MindMapNodeTransformer : BaseGoObjectTransformer
    {
        public MindMapNodeTransformer()
            : base()
        {
            this.BodyConsumesChildElements = true;
            this.TransformerType = typeof(MindMapNode);
            this.ElementName = "mmbasicnode";
            this.IdAttributeUsedForSharedObjects = true;
        }

        public override object Allocate()
        {
            return new MindMapNode(true);
            return base.Allocate();
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            MindMapNode node = obj as MindMapNode;
            node.GUID = StringAttr("guid", Guid.NewGuid().ToString());
            node.LabelSpot = Northwoods.Go.GoText.Middle;
            node.Editable = false;
            //node.Text = StringAttr("txt", "");
            //node.PartID = Int32Attr("id", 0);
            //node.Port.PartID = node.PartID;
        }

        public override void ConsumeChild(object parent, object child)
        {
            MindMapNode node = parent as MindMapNode;
            base.ConsumeChild(parent, child);
            if (child is GoText)
            {
                node.Label = child as GoText;
                //node.Label.Editable = true;
                //node.Label.Multiline = true;
                //node.Label.Wrapping = true;
            }
        }
        public override void ConsumeObjectFinish(object obj)
        {
            base.ConsumeObjectFinish(obj);
            MindMapNode node = new MindMapNode(true);
            node.Bounds = RectangleFAttr("xy", new RectangleF());
            node.Position = new PointF(node.Bounds.X, node.Bounds.Y);
            if (StringAttr("shapeType", "").Length > 0)
                node.SetShape((MindMapNode.ShapeType)Enum.Parse(typeof(MindMapNode.ShapeType), StringAttr("shapeType", "Rectangle")));

            if (Int32Attr("brush", 0) != 0)
                node.SetShapeColor(Color.FromArgb(Int32Attr("brush", Color.SteelBlue.ToArgb())), Color.FromArgb(Int32Attr("brush", Color.SteelBlue.ToArgb())));
            else
                node.SetShapeColor(Color.FromArgb(Int32Attr("brushFrom", Color.SteelBlue.ToArgb())), Color.FromArgb(Int32Attr("brushTo", Color.White.ToArgb())));
        }

        public override void GenerateAttributes(object obj)
        {
            base.GenerateAttributes(obj);
            MindMapNode node = obj as MindMapNode;
            WriteAttrVal("guid", node.GUID);
            WriteAttrVal("txt", node.Text);

            if (node.MyShape == null)
            {
                SolidBrush x = node.Brush as SolidBrush;
                WriteAttrVal("brush", x.Color.ToArgb().ToString());
            }
            else
            {
                WriteAttrVal("brushFrom", node.ShapeBrush.InnerColor);
                WriteAttrVal("brushTo", node.ShapeBrush.OuterColor);
            }
            WriteAttrVal("nodeID", node.PartID);

            WriteAttrVal("locX", node.Bounds.X);
            WriteAttrVal("locY", node.Bounds.Y);
            WriteAttrVal("isFixed", node.IsFixedLocation);

            WriteAttrVal("sizeX", node.Bounds.Width);
            WriteAttrVal("sizeY", node.Bounds.Height);

            WriteAttrVal("fontSize", node.Label.FontSize);
            WriteAttrVal("fontFamilyName", node.Label.FamilyName);
            WriteAttrVal("wrappingWidth", node.Label.WrappingWidth);

            WriteAttrVal("shapeType", node.MyShapeType.ToString());

        }
    }
}