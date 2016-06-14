using System;
using System.Collections.Generic;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Nodes;
using MetaBuilder.Meta;
using Northwoods.Go;
using Northwoods.Go.Xml;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Nodes.Repeaters
{
    public class RepeaterSectionTransformer : EmbeddedObjectsTransformer
    {

        #region Fields (2)

        private string BoundProperty;
        private string ChildClass;

        #endregion Fields

        #region Constructors (1)

        public RepeaterSectionTransformer()
            : base()
        {
            TransformerType = typeof(RepeaterSection);
            ElementName = "repeater";
            BodyConsumesChildElements = true;
            this.IdAttributeUsedForSharedObjects = true;

        }

        #endregion Constructors

        #region Methods (6)

        // Public Methods (6) 

        public override object Allocate()
        {
            RepeaterSection retval = new RepeaterSection();
            retval.CreateBody();
            //objectsInRepeater = new List<MetaBase>();
            return retval;
        }

        public override void ConsumeAttributes(object obj)
        {
            string headerText = StringAttr("HeaderText", "");

            CollapsingRecordNodeItem crnodeitem = new CollapsingRecordNodeItem();
            crnodeitem.Init("", headerText, true);
            crnodeitem.Editable = false;
            RepeaterSection section = obj as RepeaterSection;

            section.Name = StringAttr("Name", "");
            float width = float.Parse(StringAttr("RWidth", "0"), System.Globalization.CultureInfo.InvariantCulture);
            crnodeitem.Width = width;
            //crnodeitem.ParentSection = section;
            section.Add(crnodeitem);
            section.SetAllItemWidth(width);
            section.ChildPortsEnabled = BooleanAttr("ChildPortsEnabled", false);
            BoundProperty = StringAttr("BoundProperty", "");
            ChildClass = StringAttr("ChildClass", "");
            section.Bounds = RectangleFAttr("xy", new System.Drawing.RectangleF());

            base.ConsumeAttributes(obj);
        }

        CollapsingRecordNodeItem item;
        public override void ConsumeChild(object parent, object child)
        {
            //if (!(child is BoundLabel) && item == null)
            //    base.ConsumeChild(parent, child);

            if (child is MetaBase)
            {
                MetaBase mbC = child as MetaBase;
                try
                {
                    RepeaterSection section = parent as RepeaterSection;
                    //Item is set to a NEW item and not the one which was saved.
                    //Findshared then finds the object whose metaobject is equal to this one. When there are shallow copies it will always use the last
                    item = section.AddChildItem(ChildClass, BoundProperty);
                    item.MetaObject = child as MetaBase;
                    item.HookupEvents();
                    item.Text = item.MetaObject.ToString();
                    //item.BindToMetaObjectProperties();
                    //item.FireMetaObjectChanged(this, EventArgs.Empty);
                    //item.BindToMetaObjectProperties();
                    item.LayoutChildren(null);
                    item.MetaObject.tag = item;

                    item.Visible = true;
                    //if (mbC.ToString() != null)
                    //{
                    //    if (mbC.ToString().Length == 0)
                    //    {
                    //        section.Remove(item);
                    //    }
                    //}
                    //else
                    //{
                    //    section.Remove(item);
                    //}
                }
                catch (Exception ex)
                {
                    Core.Log.WriteLog("RepeaterSectionTransformer::ConsumeChild-Cannot consume Metabase " + ex.ToString());
                }
            }
            else if (child is BoundLabel && item != null)
            {
                //apply this formatting to the last node added!
                item.GetLabel = (child as BoundLabel);
                item = null;
            }
        }

        public override void ConsumeObjectFinish(object obj)
        {
            RepeaterSection node = obj as RepeaterSection;
            node.SetAllItemWidth(node.Width);
            base.ConsumeObjectFinish(obj);
            node.Shadowed = BooleanAttr("Shadow", false);
        }

        public override void GenerateAttributes(object obj)
        {
            RepeaterSection section = obj as RepeaterSection;
            if (obj is ExpandableLabelList)
            {
                IdAttributeUsedForSharedObjects = false;
            }
            else
            {
                WriteAttrVal("Name", section.Name);
                WriteAttrVal("HeaderText", section.Name);
                WriteAttrVal("RWidth", section.Width);
                WriteAttrVal("id", Writer.FindShared(obj));
                WriteAttrVal("ChildPortsEnabled", section.ChildPortsEnabled);

                RepeaterBindingInfo rbinfo = section.GetRepeaterBindingInfo();
                WriteAttrVal("BoundProperty", rbinfo.BoundProperty);
                WriteAttrVal("ChildClass", rbinfo.Association.ChildClass);
                WriteAttrVal("xy", section.Bounds);
                WriteAttrVal("Shadow", section.Shadowed);
            }
        }

        public override void GenerateBody(object obj)
        {
            base.GenerateBody(obj, true, false);//true);//base.GenerateBody(obj);
        }

        #endregion Methods

    }
}