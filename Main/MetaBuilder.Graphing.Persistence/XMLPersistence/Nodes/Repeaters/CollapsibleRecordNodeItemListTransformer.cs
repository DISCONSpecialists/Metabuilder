using System;
using System.Drawing;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Nodes;
using MetaBuilder.Meta;
using Northwoods.Go;
using Northwoods.Go.Xml;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Nodes.Repeaters
{
    public class CollapsibleRecordNodeItemListTransformer : EmbeddedObjectsTransformer
    {

        #region Constructors (1)

        public CollapsibleRecordNodeItemListTransformer()
            : base()
        {
            TransformerType = typeof(CollapsingRecordNodeItemList);
            ElementName = "collapsibleNodeItemList";
            BodyConsumesChildElements = true;
            IdAttributeUsedForSharedObjects = true;
        }

        #endregion Constructors

        #region Methods (5)

        // Public Methods (5) 

        public override object Allocate()
        {
            CollapsingRecordNodeItemList retval = new CollapsingRecordNodeItemList();
            return retval;
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            GoObject goObj = obj as GoObject;
            goObj.Bounds = RectangleFAttr("xy", new RectangleF());

            //   float fX = float.Parse(StringAttr("PositionX", "0"));
            //    float fY = float.Parse(StringAttr("PositionY", "0"));
            //     goObj.Position = new System.Drawing.PointF(fX, fY);
            //    goObj.Width = float.Parse(StringAttr("Width", "0"));
        }

        public override void ConsumeChild(object parent, object child)
        {
            base.ConsumeChild(parent, child);
            IGoCollection node = parent as IGoCollection;
            if (child != null)
            {
                if (child is GoObject)
                {
                    GoObject o = child as GoObject;
                    o.Remove();
                    if (o is GoText)
                    {
                        if (node is GraphNode)
                        {
                            GraphNode gnode = node as GraphNode;
                            GoText txt = o as GoText;
                            txt.Wrapping = true;
                            txt.WrappingWidth = txt.Width - 2;
                            bool originalEditmode = gnode.EditMode;
                            if (txt.Maximum != 1999)
                            {
                                gnode.EditMode = false;
                            }
                            gnode.Add(txt);
                            gnode.EditMode = originalEditmode;
                        }
                        else
                            node.Add(o);
                    }
                    else
                        node.Add(o);
                }
            }

            if (node is IMetaNode)
            {
                IMetaNode mNode = node as IMetaNode;
                if (child is MetaBase)
                {
                    mNode.MetaObject = child as MetaBase;
                    if (mNode is GraphNode)
                    {
                        GraphNode gnode = mNode as GraphNode;
                        gnode.EditMode = false;
                    }
                    mNode.BindingInfo.BoundObjectID = mNode.MetaObject.pkid;
                }
            }
        }

        public override void ConsumeObjectFinish(object obj)
        {
            //CollapsingRecordNodeItemList node = obj as CollapsingRecordNodeItemList;
            //  node.Width = float.Parse(StringAttr("Width", "0"));
            // node.SetAllItemWidth(node.Width);
            //bool expanded = BooleanAttr("expanded", true);

            //if (expanded)
            //{
            //    //  node.Visible = true;
            //    node.Expand();
            //}
            //else
            //{
            //    //   node.Visible = false;
            //    node.Collapse();
            //}
            base.ConsumeObjectFinish(obj);
        }

        public override void GenerateAttributes(object obj)
        {
            //   base.GenerateAttributes(obj);
            base.GenerateAttributes(obj, true);
            CollapsingRecordNodeItemList list = obj as CollapsingRecordNodeItemList;
            //   WriteAttrVal("Width", list.Width);
            WriteAttrVal("Expanded", list.IsExpanded);
            //WriteAttrVal("xy", list.Bounds);
            //     WriteAttrVal("PositionX", list.Position.X.ToString());
            //    WriteAttrVal("PositionY", list.Position.Y.ToString());
        }

        #endregion Methods

    }
}
