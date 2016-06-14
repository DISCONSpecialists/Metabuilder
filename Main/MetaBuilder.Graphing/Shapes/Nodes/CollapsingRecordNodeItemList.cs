using System;
using System.Drawing;
using System.Windows.Forms;
using MetaBuilder.Graphing.Shapes.Behaviours;
using MetaBuilder.Graphing.Shapes.Nodes;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Shapes
{
    [Serializable]
    public class CollapsingRecordNodeItemList : GoListGroup, IGoCollapsible, IIdentifiable
    {
        #region IIdentifiable Implementation

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        #endregion

        #region Fields (8)

        public const int ChangedChildIndentation = LastChangedHint + 3765;
        public const int ChangedInsertable = LastChangedHint + 3768;
        public const int ChangedIsExpanded = LastChangedHint + 3766;
        public const int ChangedWasExpanded = LastChangedHint + 3767;
        private float myChildIndentation = 2;
        private bool myInsertable;
        // CollapsingRecordNodeItemList state
        private bool myIsExpanded = true;
        private bool myWasExpanded = true;

        #endregion Fields

        #region Constructors (1)

        private bool vertical;
        public bool Vertical
        {
            get { return vertical; }
            set
            {
                float x = 0;
                float y = 0;

                vertical = value;
                if (vertical)
                {
                    foreach (GoObject o in this)
                    {
                        if (o is CollapsingRecordNodeItemList)
                        {
                            o.Width = o.Width / 2;
                            if (x == 0)
                            {
                                x = o.Location.X;
                                y = o.Location.Y;
                            }
                            else
                            {
                                o.Location = new PointF(x + o.Width + 1, y);
                            }
                        }
                    }
                }
            }
        }

        public override Orientation Orientation
        {
            get
            {
                if (Vertical)
                    return Orientation.Horizontal;
                return base.Orientation;
            }
            set
            {
                base.Orientation = value;
            }
        }

        public CollapsingRecordNodeItemList()
        {
            Initializing = true;
            Selectable = false;

            //??? comment out the following three assignments, and change Insertable to true,
            // if you want to be able to drag around itemlists within a CollapsingRecordNode
            //Copyable = false;
            //Deletable = false;
            //DragsNode = true;
            Insertable = true;

            TopLeftMargin = new SizeF(0, 0);
            BottomRightMargin = new SizeF(0, 0);
            // create the header for this list, shown both when expanded and when collapsed
            Initializing = false;
            LayoutChildren(null);
        }

        public override void Remove(GoObject obj)
        {
            base.Remove(obj);
            LayoutChildren(this);
            ComputeBounds();
        }

        #endregion Constructors

        #region Properties (11)

        // The relative indentation for the items in this list compared to the Header.
        // This does not modify the indentation of any items unless they are Add'ed.
        public float ChildIndentation
        {
            get { return myChildIndentation; }
            set
            {
                float old = myChildIndentation;
                if (old != value && value >= 0)
                {
                    myChildIndentation = value;
                    Changed(ChangedChildIndentation, 0, null, MakeRect(old), 0, null, MakeRect(value));
                }
            }
        }

        // IGoCollapsible methods/properties

        /* public Color TextColor
        {
            get
            {
                return Label.TextColor;
            }
            set
            {
                Label.TextColor = value;
            }
        }*/

        public GoCollapsibleHandle Handle
        {
            get
            {
                CollapsingRecordNodeItem hdr = Header;
                Insertable = false;
                if (hdr != null)
                {
                    return hdr.Handle;
                }
                return null;
            }
        }

        // access to the Header and its important parts:
        public CollapsingRecordNodeItem Header
        {
            get
            {
                if (Count > 0)
                {
                    if (this[0] is CollapsingRecordNodeItem)
                        return this[0] as CollapsingRecordNodeItem;

                    if (this[0] is CollapsingRecordNodeItemList)
                    {
                        CollapsingRecordNodeItemList list = this[0] as CollapsingRecordNodeItemList;
                        if (list.Count > 0)
                            return list[0] as CollapsingRecordNodeItem;
                    }
                }
                return null;
            }
        }

        // maybe allow insertions of CollapsingRecordNodeItem[List]s
        public bool Insertable
        {
            get { return myInsertable; }
            set
            {
                bool old = myInsertable;
                if (old != value)
                {
                    myInsertable = value;
                    Changed(ChangedInsertable, 0, old, NullRect, 0, value, NullRect);
                }
            }
        }

        public GoText Label
        {
            get
            {
                CollapsingRecordNodeItem hdr = Header;
                if (hdr != null) return hdr.Label;
                return null;
            }
        }

        // for convenience in accessing any parent CollapsingRecordNodeItemList
        public CollapsingRecordNodeItemList ParentList
        {
            get { return Parent as CollapsingRecordNodeItemList; }
        }

        // when selected, the header appears to be selected, rather than the whole listgroup
        public override GoObject SelectionObject
        {
            get
            {
                if (Header != null)
                    return Header;
                return Parent;
            }
        }

        public String Text
        {
            get
            {
                GoText lab = Label;
                if (lab != null) return lab.Text;
                return "";
            }
            set
            {
                GoText lab = Label;
                if (lab != null) lab.Text = value;
            }
        }

        public bool WasExpanded
        {
            get { return myWasExpanded; }
            set
            {
                bool old = myWasExpanded;
                if (old != value)
                {
                    myWasExpanded = value;
                    Changed(ChangedWasExpanded, 0, old, NullRect, 0, value, NullRect);
                }
            }
        }

        public virtual bool Collapsible
        {
            get { return true; }
            set { }
        }

        public virtual bool IsExpanded
        {
            get { return myIsExpanded; }
        }

        #endregion Properties

        #region Methods (12)

        // Public Methods (7) 

        // when adding an Item or an ItemList, set its Width, and change its indentation appropriately

        private BindingInfo bindingInfo;

        public BindingInfo BindingInfo
        {
            get { return bindingInfo; }
            set { bindingInfo = value; }
        }

        public virtual void Collapse()
        {
            if (!IsExpanded)
                return;

            foreach (GoObject obj in this)
            {
                CollapsingRecordNodeItemList list = obj as CollapsingRecordNodeItemList;
                if (list != null && list != this)
                {
                    list.WasExpanded = list.IsExpanded;
                    list.Collapse();
                }
                obj.Visible = false;
            }
            myIsExpanded = false;
            Changed(ChangedIsExpanded, 0, true, NullRect, 0, false, NullRect);
            LayoutChildren(null);
        }

        // when expanding any nested lists, respect their .WasExpanded properties
        public virtual void Expand()
        {
            if (IsExpanded)
                return;

            foreach (GoObject obj in this)
            {
                CollapsingRecordNodeItemList list = obj as CollapsingRecordNodeItemList;
                if (list != null && list.WasExpanded && list != this)
                {
                    list.Expand();
                }
                obj.Visible = true;
            }
            myIsExpanded = true;
            Changed(ChangedIsExpanded, 0, false, NullRect, 0, true, NullRect);
            LayoutChildren(null);
        }

        public override void Changed(int subhint, int oldI, object oldVal, RectangleF oldRect, int newI, object newVal, RectangleF newRect)
        {
            if (subhint == InsertedObject && !Initializing)
            {
                GoObject obj = newVal as GoObject;
                if (obj != null)
                {
                    CollapsibleNode n = CollapsibleNode.FindCollapsingRecordNode(this);
                    if (n != null)
                    {
                        SetItemWidth(obj, n.ItemWidth);
                    }
                    SetIndentation(obj, FindTotalIndentation(this));
                }
            }
            base.Changed(subhint, oldI, oldVal, oldRect, newI, newVal, newRect);
        }

        // change state for undo/redo
        public override void ChangeValue(GoChangedEventArgs e, bool undo)
        {
            switch (e.SubHint)
            {
                case ChangedChildIndentation:
                    ChildIndentation = e.GetFloat(undo);
                    break;
                case ChangedIsExpanded:
                    myIsExpanded = (bool)e.GetValue(undo);
                    break;
                case ChangedWasExpanded:
                    WasExpanded = (bool)e.GetValue(undo);
                    break;
                case ChangedInsertable:
                    Insertable = (bool)e.GetValue(undo);
                    break;
                default:
                    base.ChangeValue(e, undo);
                    break;
            }
        }

        public void ForceComputeBounds()
        {
            Bounds = ComputeBounds();
        }

        public override void LayoutChildren(GoObject childchanged)
        {
            //    if (this.ParentNode.Initializing)
            {
                // Do nothing
            }
            //    else
            {
                if (Handle != null)
                    Handle.Printable = false;
                base.LayoutChildren(childchanged);
            }
        }

        // Position each invisible item in the same place as the previous visible item.
        // This works well for ports if the items are all the same height.
        // If the items have different heights, the ports will be at different Y coords
        // when the lists are collapsed.
        public override float LayoutItem(int i, RectangleF cell)
        {
            GoObject item = this[i];
            if (item != null && Orientation == Orientation.Vertical && !item.Visible)
            {
                item.Position = new PointF(cell.X, cell.Y - item.Height); // overlap invisible items
                //      item.Width = this.Width - 2;
                if (item is ExpandableLabel)
                {
                    ExpandableLabel exlbl = item as ExpandableLabel;
                    exlbl.Label.AutoResizes = true; // false;
                }
                return cell.Y; // don't increment vertical height
            }
            return base.LayoutItem(i, cell);
        }

        public void SetAllItemWidth(float w)
        {
            foreach (GoObject obj in this)
            {
                //  if (obj is CollapsingRecordNodeItem || obj is CollapsingRecordNodeItemList)
                SetItemWidth(obj, w);
            }
        }

        // Private Methods (2) 

        private float FindTotalIndentation(GoObject obj)
        {
            float w = 0;
            if (obj != null)
            {
                CollapsingRecordNodeItemList list = obj as CollapsingRecordNodeItemList;
                if (list != null)
                {
                    w += list.ChildIndentation;
                }
                GoObject parent = obj.Parent;
                if (!(parent is CollapsibleNode))
                {
                    w += FindTotalIndentation(parent);
                }
            }
            return w;
        }

        private void SetIndentation(GoObject obj, float w)
        {
            CollapsingRecordNodeItemList list = obj as CollapsingRecordNodeItemList;
            if (list != null)
            {
                list.SetAllIndentation(w);
            }
            else
            {
                CollapsingRecordNodeItem item = obj as CollapsingRecordNodeItem;
                if (item != null)
                {
                    item.Indentation = w;
                }
            }
        }

        public virtual GoObject AddItemFromCode()
        {
            return AddItem();
        }

        public virtual CollapsingRecordNodeItem AddItem()
        {
            RepeaterBindingInfo rbinfo = GetRepeaterBindingInfo();
            if (rbinfo != null)
            {
                CollapsingRecordNodeItem crnodeitem = AddChildItem(rbinfo.Association.ChildClass, rbinfo.BoundProperty);
                return crnodeitem;
            }
            return null;
        }

        public RepeaterBindingInfo GetRepeaterBindingInfo()
        {
            RepeaterBindingInfo rbinfo = null;
            GoObject myparent = Parent;
            while (!(myparent is CollapsibleNode))
            {
                myparent = myparent.Parent;
            }
            CollapsibleNode pnode = myparent as CollapsibleNode;
            if (Count > 0)
            {
                if (this[0] is ExpandableLabelList)
                {
                    ExpandableLabelList lst = this[0] as ExpandableLabelList;
                    if (lst.Name != null)
                        return pnode.BindingInfo.GetRepeaterBindingInfo(lst.Name);
                }

                if (this[0] is RepeaterSection)
                {
                    RepeaterSection sect = this[0] as RepeaterSection;
                    return pnode.BindingInfo.GetRepeaterBindingInfo(sect.Name);
                }

                if (this[0] is CollapsingRecordNodeItemList)
                {
                    CollapsingRecordNodeItemList list = this[0] as CollapsingRecordNodeItemList;
                    if (list.Parent is RepeaterSection)
                    {
                        RepeaterSection rsect = list.Parent as RepeaterSection;
                        string name = rsect.Name;
                        return pnode.BindingInfo.GetRepeaterBindingInfo(name);
                    }
                }
            }
            CollapsingRecordNodeItemList rsection = this;
            if (rsection.Name != null)
                return pnode.BindingInfo.GetRepeaterBindingInfo(rsection.Name);

            if (Name != null)
                rbinfo = pnode.BindingInfo.GetRepeaterBindingInfo(Name);

            if (rbinfo == null && pnode.BindingInfo.RepeaterBindings.Count == 1)
            {
                rbinfo = pnode.BindingInfo.RepeaterBindings[0];
            }
            return rbinfo;
        }

        [NonSerializedAttribute]
        public CollapsingRecordNodeItem lastItemSelected;
        public CollapsingRecordNodeItem AddChildItem(string itemClass, string boundProperty)
        {
            ////prevent adding an item if it is readonly :)
            //if (this.ParentNode is CollapsibleNode)
            //{
            //    CollapsibleNode pNode = this.ParentNode as CollapsibleNode;
            //    if (pNode.MetaObject != null)
            //    {
            //        if (pNode.MetaObject.State == MetaBuilder.BusinessLogic.VCStatusList.CheckedIn || pNode.MetaObject.State == MetaBuilder.BusinessLogic.VCStatusList.CheckedOutRead || pNode.MetaObject.State == MetaBuilder.BusinessLogic.VCStatusList.Obsolete || pNode.MetaObject.State == MetaBuilder.BusinessLogic.VCStatusList.Locked)
            //            return null;
            //    }
            //}

            CollapsingRecordNodeItem crnodeitem = new CollapsingRecordNodeItem();
            /*if (ChildPortsEnabled)
            {
                crnodeitem.UserFlags = 1;
            }*/
            crnodeitem.BindingInfo = new BindingInfo();
            crnodeitem.BindingInfo.BindingClass = itemClass;
            crnodeitem.BindingInfo.Bindings.Add("Default", boundProperty);
            crnodeitem.CreateMetaObject(this, EventArgs.Empty);
            if (crnodeitem.MetaObject.ToString() != null)
            {
                if (crnodeitem.MetaObject.ToString().Length > 0)
                {
                    crnodeitem.Init("", crnodeitem.MetaObject.ToString(), false);
                }
            }
            else
            {
                crnodeitem.Init("", crnodeitem.MetaObject.Class + " Name", false);
            }
            SetItemWidth(crnodeitem, Width);
            crnodeitem.Name = "Default";

            //indicators
            //if (crnodeitem.MetaObject != null)
            //{
            //    MetaBuilder.BusinessLogic.VCStatusList state = crnodeitem.MetaObject.State;
            //    if (state == MetaBuilder.BusinessLogic.VCStatusList.CheckedIn || state == MetaBuilder.BusinessLogic.VCStatusList.Locked || state == MetaBuilder.BusinessLogic.VCStatusList.CheckedOutRead || state == MetaBuilder.BusinessLogic.VCStatusList.Obsolete)
            //    {
            //        //I AM READONLY
            //        MetaBuilder.Graphing.Controllers.VisualIndicatorController indCont = new MetaBuilder.Graphing.Controllers.VisualIndicatorController();
            //        indCont.AddIndicator(crnodeitem.State.ToString(), Color.Gray, crnodeitem as GoGroup);
            //        crnodeitem.Editable = false;
            //        crnodeitem.Copyable = true;
            //    }
            //}
            if ((!this.Contains(lastItemSelected)))
                lastItemSelected = null;

            if (lastItemSelected != null)
                InsertBefore(lastItemSelected, crnodeitem);
            else
                Add(crnodeitem);

            return crnodeitem;
        }

        // Internal Methods (3) 

        internal float FindItemWidth()
        {
            foreach (GoObject obj in this)
            {
                CollapsingRecordNodeItemList list = obj as CollapsingRecordNodeItemList;
                if (list != null)
                {
                    float w = list.FindItemWidth();
                    if (w > 0) return w;
                }
                else
                {
                    return obj.Width;
                }
            }
            return -1;
        }

        internal void SetAllIndentation(float w)
        {
            GoObject header = Header;
            foreach (GoObject obj in this)
            {
                if (obj == header)
                    SetIndentation(obj, w);
                else
                    SetIndentation(obj, w + ChildIndentation);
            }
        }

        internal void SetItemWidth(GoObject obj, float w)
        {
            // if headerlabel is wider than the rest, adjust accordingly
            CollapsingRecordNodeItemList list = obj as CollapsingRecordNodeItemList;
            if (list != null)
            {
                list.Width = w;
                list.SetAllItemWidth(w);
            }
            else
            {
                CollapsingRecordNodeItem item = obj as CollapsingRecordNodeItem;
                if (item != null)
                {
                    item.Width = w;
                    if (item.Label != null)
                        item.Label.WrappingWidth = item.Label.Width - 2;
                    item.RepositionPorts();
                }
                else
                {
                    obj.Width = w;
                }
            }
        }

        public override bool OnSingleClick(GoInputEventArgs evt, GoView view)
        {
            return true;
        }

        #endregion Methods
    }
}