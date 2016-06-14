using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Behaviours;
using MetaBuilder.Graphing.Shapes.Behaviours.Observers;
using MetaBuilder.Graphing.Shapes.Nodes;
using MetaBuilder.Meta;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Nodes.Repeaters
{
    public class CollapsibleNodeTransformer : GraphNodeTransformer
    {

        #region Constructors (1)

        public CollapsibleNodeTransformer()
        {
            this.IdAttributeUsedForSharedObjects = true;
            this.TransformerType = typeof(CollapsibleNode);
            this.ElementName = "collapsibleNode";
            this.BodyConsumesChildElements = true;
        }

        #endregion Constructors

        #region Methods (6)

        // Public Methods (5) 

        public override object Allocate()
        {
            CollapsibleNode node = new CollapsibleNode();
            node.CreateBody();
            string boundclass = StringAttr("cls", string.Empty);
            //replaced = false;
            node = replaceShape(node, boundclass);
            return node;
        }
        private CollapsibleNode replaceShape(CollapsibleNode obj, string className)
        {
            //replaced = false;
            if (className == "DataView")
            {
                obj.CanBeAutomaticallyReplaced = true;
                return obj;
            }
            foreach (KeyValuePair<string, string> kvp in Core.Variables.Instance.RenamedClasses)
                //{
                if (kvp.Key == className)
                    obj.CanBeAutomaticallyReplaced = true;
            //    {
            //        try
            //        {
            //            GraphNode n = (Core.Variables.Instance.ReturnShape(kvp.Value) as GraphNode).Copy() as GraphNode;
            //            n.MetaObject = obj.MetaObject;
            //            n.HookupEvents();
            //            replaced = true;
            //            return n;
            //        }
            //        catch
            //        {
            //            break;
            //        }
            //    }
            //}

            return obj;
        }
        /*public override void ConsumeChild(object parent, object child)
        {
            base.ConsumeChild()
            CollapsibleNode node = parent as CollapsibleNode;
            string TypeName = child.GetType().Name;
                base.ConsumeChild(parent, child);

                if (child != null)
                {
                    if (child is GoObject)
                    {
                        GoObject oChild = child as GoObject;
                        oChild.Remove();
                        if (oChild is GoGroup || oChild is RepeaterSection)
                        {
                            node.List.Add(oChild);
                        }
                        else
                            node.Add(oChild);
                    }
                }
            if (child is MetaBase)
            {
                node.MetaObject = child as MetaBase;
                node.EditMode = false;
                node.BindingInfo.BoundObjectID = node.MetaObject.pkid;
            }
        }*/

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            CollapsibleNode colnode = obj as CollapsibleNode;
            ConsumeRepeaterBindingAttributes(colnode, obj);
            colnode.Bounds = RectangleFAttr("xy", new System.Drawing.RectangleF());

            colnode.Selectable = true;

            if (StringAttr("CN", null) != null)
                Reader.MakeShared(StringAttr("CN", null), obj);
        }

        public override void ConsumeChild(object parent, object child)
        {
            if (child is GraphNodeGrid)
                return;

            if (child is BoundLabel)
            {
                BoundLabel lbl = child as BoundLabel;
                lbl.Clipping = false;
                lbl.Deletable = false;
            }
            //CollapsibleNode retval = parent as CollapsibleNode;
            //bool expand = BooleanAttr("expanded", true);
            //if (expand)
            //{
            //    retval.Expand();
            //    retval.IsExpanded = true;
            //}
            //else
            //{
            //    retval.Collapse();
            //    retval.IsExpanded = false;
            //}
            base.ConsumeChild(parent, child);
        }

        public override void ConsumeObjectFinish(object obj)
        {
            base.ConsumeObjectFinish(obj);
            CollapsibleNode node = obj as CollapsibleNode;
            GoGroupEnumerator partEnum = node.GetEnumerator();
            float minX = node.Right;
            float minY = node.Bottom;
            GoObject culpritX = null;
            GoObject culpritY = null;
            while (partEnum.MoveNext())
            {
                if (!(partEnum.Current is GraphNodeGrid))
                {
                    if (partEnum.Current.Position.X < minX)
                    {
                        minX = partEnum.Current.Position.X;
                        culpritX = partEnum.Current;
                    }
                    if (partEnum.Current.Position.Y < minY)
                    {
                        minY = partEnum.Current.Position.Y;
                        culpritY = partEnum.Current;
                    }
                }
            }
            node.CalculateGridSize();
            node.Position = new System.Drawing.PointF(minX, minY);

            if (node.BindingInfo != null)
            {
                if (node.BindingInfo.BindingClass == "DataView" && node.MetaObject._ClassName == "DataView")
                {
                    BoundLabel lbl = node.FindByName(" - Bound Label - 3") as BoundLabel;
                    if (lbl != null)
                    {
                        object o = node.MetaObject.Get("DataViewType");
                        if (o != null)
                        {
                            lbl.Text = o.ToString();
                            lbl.Editable = true;
                            if (!(node.BindingInfo.Bindings.ContainsKey(" - Bound Label - 3")))
                                node.BindingInfo.Bindings.Add(" - Bound Label - 3", "DataViewType");
                        }
                        else
                        {
                            if (lbl.Text != "" && lbl.Text != "Type")
                            {
                                node.MetaObject.Set("DataViewType", lbl.Text);
                                if (!(node.BindingInfo.Bindings.ContainsKey(" - Bound Label - 3")))
                                    node.BindingInfo.Bindings.Add(" - Bound Label - 3", "DataViewType");
                            }
                        }
                    }
                }
            }

            if (node.Handle == null)
                node.AddHandle();
            else
                node.Handle.Visible = true;

            if (StringAttr("expanded", null) != null)
            {
                node.expandedLoadedState = BooleanAttr("expanded", true);
                if (BooleanAttr("expanded", false))
                    node.Expand();
                else
                    node.Collapse();
            }

            node.Shadowed = BooleanAttr("Shadow", false);
            //node.BindMetaObjectImage();
        }

        public override void GenerateAttributes(Object obj)
        {
            CollapsibleNode node = obj as CollapsibleNode;

            bool expanded = node.IsExpanded;
            WriteAttrVal("expanded", node.IsExpanded);
            //if (!node.IsExpanded)
            //{
            //node.ToString();
            //node.Expand();
            //}

            base.GenerateAttributes(obj);
            GenerateLabelBindings(node);
            Writer.MakeShared(obj);
            WriteAttrVal("id", Writer.FindShared(obj));
            WriteAttrVal("xy", node.Bounds);
            WriteRepeaterBindingAttributes(node);
            WriteAttrVal("CN", node.MetaObject.pkid + ":" + node.Location.ToString());
            WriteAttrVal("Shadow", node.Shadowed);

            //if (!expanded)
            //    node.Collapse();
        }

        // Private Methods (1) 

        private void GenerateLabelBindings(CollapsibleNode node)
        {
            if (node.HasBindingInfo)
            {
                WriteAttrVal("cls", node.BindingInfo.BindingClass);
                StringBuilder sbLabelBindings = new StringBuilder();
                foreach (KeyValuePair<string, string> bindingItem in node.BindingInfo.Bindings)
                {
                    if (bindingItem.Value != "-none-")
                    {
                        sbLabelBindings.Append(bindingItem.Key + "|" + bindingItem.Value + "~");
                    }
                }
                WriteAttrVal("lbls", sbLabelBindings.ToString());
            }
        }

        #endregion Methods

        #region Repeater BindingAttributes
        private void WriteRepeaterBindingAttributes(CollapsibleNode node)
        {
            List<RepeaterBindingInfo> repeaterBindings = node.BindingInfo.RepeaterBindings;
            StringBuilder strBinding = new StringBuilder();
            foreach (RepeaterBindingInfo info in repeaterBindings)
            {
                strBinding.Append(info.Association.ChildClass.ToString() + ",");
                strBinding.Append(info.RepeaterDirection.ToString() + ",");
                strBinding.Append(info.BoundProperty.ToString() + ",");
                strBinding.Append(info.Association.ID.ToString() + ",");

                RepeaterSection section = FindRepeaterByID(node, info.RepeaterID);
                if (section == null)
                    section = FindRepeaterByName(node, info.RepeaterName);
                if (section == null)
                    section = FindRepeaterByName(node, info.RepeaterID.ToString());
                if ((section == null) && (node.RepeaterSections.Count == 1))
                {
                    section = node.RepeaterSections[0];
                }
                if (section != null)
                {
                    if (section.Label != null)
                        section.Name = section.Label.Text;// Guid.NewGuid().ToString();
                    else
                    {
                        if (section is ExpandableLabelList)
                        {
                            ExpandableLabelList list = section as ExpandableLabelList;
                            section.Name = list.HeaderText.Text;
                        }
                    }
                    strBinding.Append(section.Name + ",");//"section.Label.Text + "|");
                }
                else
                    strBinding.Append(info.Association.Caption.ToString() + ",");

                strBinding.Append(info.Association.Caption.ToString() + "|");
            }
            WriteAttrVal("Binding", strBinding.ToString());
        }
        private RepeaterSection FindRepeaterByName(CollapsibleNode node, string name)
        {
            //RepeaterSection retval;

            foreach (RepeaterSection section in node.RepeaterSections)
            {
                if (section.Name == name)
                    return section;
            }
            return null;
        }
        private RepeaterSection FindRepeaterByID(CollapsibleNode node, Guid repeaterID)
        {
            if (repeaterID == Guid.Empty)
                return null;
            //RepeaterSection retval;

            foreach (RepeaterSection section in node.RepeaterSections)
            {
                if (section.RepeaterID == repeaterID)
                    return section;
            }
            return null;
        }
        const char itemChar = ',';
        const char splitChar = '|';
        private void ConsumeRepeaterBindingAttributes(CollapsibleNode colnode, object obj)
        {
            string strBinding = StringAttr("Binding", "");
            string[] repeaterInfos = strBinding.Split(splitChar);

            if (colnode.BindingInfo != null)
            {
                colnode.BindingInfo.RepeaterBindings = new List<RepeaterBindingInfo>();
                //Attribute,Forward,Name,3471,Elements,Attributes|
                for (int i = 0; i < repeaterInfos.Length - 1; i++)
                {
                    string[] items = repeaterInfos[i].Split(itemChar);

                    RepeaterBindingInfo bindingInfo = new RepeaterBindingInfo();
                    bindingInfo.BoundProperty = items[2];
                    bindingInfo.Association = new Association();
                    bindingInfo.Association.ChildClass = items[0];
                    bindingInfo.Association.ID = int.Parse(items[3]);
                    bindingInfo.RepeaterName = items[4];
                    bindingInfo.Association.Caption = items[5];
                    colnode.BindingInfo.RepeaterBindings.Add(bindingInfo);
                }
            }
        }
        #endregion
    }
}