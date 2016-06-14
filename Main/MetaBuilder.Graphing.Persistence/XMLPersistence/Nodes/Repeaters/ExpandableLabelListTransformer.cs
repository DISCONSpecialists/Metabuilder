using System;
using System.Drawing;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Nodes;
using MetaBuilder.Meta;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Nodes.Repeaters
{
    public class ExpandableLabelListTransformer : EmbeddedObjectsTransformer
    {

        #region Fields (2)

        private string BoundProperty;
        private string ChildClass;

        #endregion Fields

        #region Constructors (1)

        public ExpandableLabelListTransformer()
            : base()
        {
            this.TransformerType = typeof(ExpandableLabelList);
            this.ElementName = "expandableLabelList";
            this.BodyConsumesChildElements = true;
        }

        #endregion Constructors

        #region Methods (7)

        // Public Methods (7) 

        public override object Allocate()
        {
            return new ExpandableLabelList();
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            ExpandableLabelList list = obj as ExpandableLabelList;
            list.Width = 180;
            list.AddHeader();
            list.Name = StringAttr("Name", "");
            list.HeaderText.Text = StringAttr("HeaderText", "");
            //     list.Width = float.Parse(StringAttr("Width", "0"));
            // list.SetAllItemWidth(list.Width);

            //     list.BorderPen = new Pen(ColorAttr("PenColor", Color.Black));
            //    list.Brush = new SolidBrush(ColorAttr("Brush", Color.Beige));

            BoundProperty = StringAttr("BoundProperty", "");
            ChildClass = StringAttr("ChildClass", "");

        }

        public override void ConsumeBody(object obj)
        {
            base.ConsumeBody(obj);
        }

        ExpandableLabel lbl = null;
        public override void ConsumeChild(object parent, object child)
        {
            ExpandableLabelList section = parent as ExpandableLabelList;

            if (child is MetaBase)
            {
                if (ChildClass != "" && BoundProperty != "")
                {
                    //CollapsingRecordNodeItem item = section.AddChildItem(ChildClass, BoundProperty) as CollapsingRecordNodeItem;
                    lbl = section.AddItemFromCode() as ExpandableLabel;
                    if (lbl.Label != null)
                    {
                        lbl.Label.Wrapping = true;
                        lbl.Label.WrappingWidth = lbl.Label.Width - 2;
                        lbl.AutoResizes = true;
                    }
                    //lbl.BindingInfo = new BindingInfo();
                    if (section.IsExpanded)
                        lbl.Expand();
                    lbl.MetaObject = child as MetaBase;

                    lbl.HookupEvents();
                    //lbl.BindToMetaObjectProperties();
                    //lbl.FireMetaObjectChanged(this, EventArgs.Empty);
                    section.LayoutChildren(lbl);
                    lbl.MetaObject.tag = lbl;
                }
            }
            else if (child is BoundLabel && lbl != null)
            {
                //base.ConsumeChild(parent, child);
                lbl.GetLabel = (child as BoundLabel);
                lbl = null;
            }
        }

        public override void ConsumeObjectFinish(object obj)
        {
            base.ConsumeObjectFinish(obj);
        }

        public override void GenerateAttributes(object obj)
        {
            //base.GenerateAttributes(obj);
            //base.GenerateAttributes(obj, true);
            ExpandableLabelList list = obj as ExpandableLabelList;
            WriteAttrVal("Name", list.Name);
            WriteAttrVal("HeaderText", list.HeaderText.Text);
            //WriteAttrVal("Width", list.Width);
            WriteAttrVal("Expanded", list.IsExpanded);
            /* SolidBrush sbrush = list.Brush as SolidBrush;
             WriteAttrVal("Brush", sbrush.Color);*/
            //   WriteAttrVal("PenColor", list.BorderPen.Color);

            if (list.ParentList.Parent is CollapsibleNode)
            {
                CollapsibleNode node = list.ParentList.Parent as CollapsibleNode;
                BindingInfo bindingInfo = node.BindingInfo;
                RepeaterBindingInfo rbinfo = bindingInfo.GetRepeaterBindingInfo(list.Name);

                if (rbinfo == null)
                    rbinfo = bindingInfo.GetRepeaterBindingInfo(list.RepeaterID);

                // REPEATER HACK!!!
                if (rbinfo == null)
                    rbinfo = bindingInfo.RepeaterBindings[node.RepeaterSections.IndexOf(list)];
                if (rbinfo != null)
                {
                    WriteAttrVal("BoundProperty", rbinfo.BoundProperty);
                    WriteAttrVal("ChildClass", rbinfo.Association.ChildClass);
                }

            }

        }

        public override void GenerateBody(object obj)
        {
            GenerateBody(obj, true, false);
        }

        #endregion Methods

    }
}
