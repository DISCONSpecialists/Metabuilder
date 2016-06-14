using System;
using System.Collections.Generic;
using System.Drawing;
using MetaBuilder.Graphing.Shapes.Behaviours;
using MetaBuilder.Graphing.Shapes.Behaviours.Observers;
using MetaBuilder.Meta;
using Northwoods.Go;
using MetaBuilder.Graphing.Shapes.Nodes;
using System.Collections.ObjectModel;

namespace MetaBuilder.Graphing.Shapes
{
    [Serializable]
    public class CollapsibleNode : GraphNode, IGoCollapsible, IMetaNode, IShallowCopyable
    {

        #region Fields (12)

        public const int ChangedChildIndentation = LastChangedHint + 3765;
        public const int ChangedInsertable = LastChangedHint + 3768;
        public const int ChangedIsExpanded = LastChangedHint + 3766;
        public const int ChangedSelectionColor = LastChangedHint + 3750;
        public const int ChangedSelectionTextColor = LastChangedHint + 3751;
        public const int ChangedWasExpanded = LastChangedHint + 3767;
        private float myChildIndentation = 12;
        private bool myInsertable;
        // CollapsingRecordNodeItemList state
        private bool myIsExpanded = true;
        private Color mySelectionColor = SystemColors.Highlight;
        private Color mySelectionTextColor = SystemColors.HighlightText;
        private bool myWasExpanded = true;

        public bool expandedLoadedState = true;

        #endregion Fields

        #region Constructors (1)

        public CollapsibleNode()
        {
            Selectable = true;
            PickableBackground = false;
            Resizable = false; // may be set to true
            Grid.Height = 10;
        }

        #endregion Constructors

        #region Properties (10)

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

        public GoCollapsibleHandle Handle
        {
            get
            {
                foreach (GoObject o in this)
                {
                    if (o is GoCollapsibleHandle && !(o is AllocationHandle))
                    {
                        o.Printable = false;
                        return o as GoCollapsibleHandle;
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

        // gets or sets the width of all of the CollapsingRecordNodeItems in this node, recursively
        public virtual float ItemWidth
        {
            get
            {
                if (List != null)
                    return List.FindItemWidth();
                return 0;
            }
            set
            {
                if (List != null)
                    List.SetAllItemWidth(value);
            }
        }

        public CollapsingRecordNodeItemList List
        {
            get
            {
                foreach (GoObject obj in this)
                {
                    if (obj is CollapsingRecordNodeItemList)
                        return obj as CollapsingRecordNodeItemList;
                }
                return null;
            }
        }

        // this color is used for the background when an item is selected
        public Color SelectionColor
        {
            get { return mySelectionColor; }
            set
            {
                Color old = mySelectionColor;
                if (old != value)
                {
                    mySelectionColor = value;
                    Changed(ChangedSelectionColor, 0, old, NullRect, 0, value, NullRect);
                }
            }
        }

        // this color is used for the text when an item is selected
        public Color SelectionTextColor
        {
            get { return mySelectionTextColor; }
            set
            {
                Color old = mySelectionTextColor;
                if (old != value)
                {
                    mySelectionTextColor = value;
                    Changed(ChangedSelectionTextColor, 0, old, NullRect, 0, value, NullRect);
                }
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
            set { myIsExpanded = value; }
        }

        #endregion Properties

        #region Methods (19)
        [NonSerialized]
        private MetaBase copiedFrom;
        public MetaBase CopiedFrom
        {
            get { return copiedFrom; }
            set { copiedFrom = value; }
        }

        public virtual void Collapse()
        {
            positioned = 0;
            foreach (GoObject o in this)
            {
                if (o is QuickPort)
                {
                    anchorObjectToObject(o, BoundLabels[0] as GoObject);
                }

                if (o is CollapsingRecordNodeItemList)
                {
                    CollapsingRecordNodeItemList mylist = o as CollapsingRecordNodeItemList;
                    if (!IsExpanded)
                        return;
                    mylist.WasExpanded = mylist.IsExpanded;
                    mylist.Collapse();
                    mylist.Visible = false;
                    mylist.Printable = false;
                }
            }
            List.Visible = false;
            myIsExpanded = false;
            CalculateGridSize();
            Changed(ChangedIsExpanded, 0, true, NullRect, 0, false, NullRect);
            LayoutChildren(null);
        }
        public virtual void Expand()
        {
            if (IsExpanded)
                return;
            positioned = -1;
            foreach (GoObject obj in this)
            {
                if (obj is GraphNodeGrid)
                    continue;

                CollapsingRecordNodeItemList list = obj as CollapsingRecordNodeItemList;
                if (list != null && list.WasExpanded)
                {
                    list.Expand();
                    list.Printable = true;
                    foreach (GoObject o in this)
                    {
                        if (o is QuickPort)
                        {
                            anchorObjectToObject(o, list);
                        }
                    }
                }
                //obj.Visible = true;
                //obj.Printable = false;
            }

            List.Visible = true;
            myIsExpanded = true;
            CalculateGridSize();
            Changed(ChangedIsExpanded, 0, false, NullRect, 0, true, NullRect);
            LayoutChildren(null);
        }

        private int returnHeightOfChildCollapsibleNodes()
        {
            int height = 0;
            if (List != null)
            {
                foreach (GoObject o in List)
                {
                    if (o is CollapsingRecordNodeItemList || o is RepeaterSection)
                    {
                        foreach (GoObject x in (o as GoListGroup))
                        {
                            if (x is CollapsingRecordNodeItemList || x is RepeaterSection)
                            {
                                foreach (GoObject y in (x as GoListGroup))
                                {
                                    if (y is CollapsingRecordNodeItem)
                                    {
                                        if (!(y as CollapsingRecordNodeItem).IsHeader)
                                            height += Convert.ToInt16(y.Height);
                                    }
                                }
                            }
                            else if (x is CollapsingRecordNodeItem)
                            {
                                if (!(x as CollapsingRecordNodeItem).IsHeader)
                                    height += Convert.ToInt16(x.Height);
                            }
                        }
                    }
                }
            }
            return height;
        }

        int positioned = 0;
        private void anchorObjectToObject(GoObject objecttoanchor, GoObject anchoredTo)
        {
            Type tAnchorLock = typeof(PositionLockLocation);
            IIdentifiable ident = anchoredTo as IIdentifiable;
            if (ident.Name == Guid.Empty.ToString() || ident.Name == "")
                ident.Name = Guid.NewGuid().ToString();

            IBehaviourShape behShape = objecttoanchor as IBehaviourShape;

            if (behShape.Manager.GetExistingBehaviour(typeof(AnchorPositionBehaviour)) == null)
                return;

            //now reposition it such that it touches the bottom of the node similar to the top node
            if (BindingInfo.BindingClass.Contains("Entity") || BindingInfo.BindingClass.Contains("DataEntity")) //Round shapes go here
            {
                int offset = returnHeightOfChildCollapsibleNodes();//this number is the total height of additional non header repeater items within this nodes list
                switch (positioned)
                {
                    case -1:
                        objecttoanchor.Location = new PointF(objecttoanchor.Location.X, objecttoanchor.Location.Y + 5 + offset);
                        positioned--;
                        break;
                    case -2:
                        objecttoanchor.Location = new PointF(objecttoanchor.Location.X, objecttoanchor.Location.Y + 19 + offset);
                        positioned--;
                        break;
                    case -3:
                        objecttoanchor.Location = new PointF(objecttoanchor.Location.X, objecttoanchor.Location.Y + 8 + offset);
                        positioned--;
                        break;
                    case -4:
                        objecttoanchor.Location = new PointF(objecttoanchor.Location.X, objecttoanchor.Location.Y + 8 + offset);
                        positioned--;
                        break;
                    case -5:
                        objecttoanchor.Location = new PointF(objecttoanchor.Location.X, objecttoanchor.Location.Y + 19 + offset);
                        positioned--;
                        break;
                    case 0:
                        objecttoanchor.Location = new PointF(objecttoanchor.Location.X, objecttoanchor.Location.Y - 5 - offset);
                        positioned++;
                        break;
                    case 1:
                        objecttoanchor.Location = new PointF(objecttoanchor.Location.X, objecttoanchor.Location.Y - 19 - offset);
                        positioned++;
                        break;
                    case 2:
                        objecttoanchor.Location = new PointF(objecttoanchor.Location.X, objecttoanchor.Location.Y - 8 - offset);
                        positioned++;
                        break;
                    case 3:
                        objecttoanchor.Location = new PointF(objecttoanchor.Location.X, objecttoanchor.Location.Y - 8 - offset);
                        positioned++;
                        break;
                    case 4:
                        objecttoanchor.Location = new PointF(objecttoanchor.Location.X, objecttoanchor.Location.Y - 19 - offset);
                        positioned++;
                        break;
                    //case 4:
                    //    break;
                }
            }
            else
            {
                if (positioned >= 0)
                    objecttoanchor.Location = new PointF(objecttoanchor.Location.X, objecttoanchor.Location.Y - List.Height);
                else
                    objecttoanchor.Location = new PointF(objecttoanchor.Location.X, objecttoanchor.Location.Y + List.Height);
            }

            PositionLockLocation position = (PositionLockLocation)Enum.Parse(tAnchorLock, "BottomCenter");
            AnchorPositionBehaviour apb = new AnchorPositionBehaviour(ident, objecttoanchor as GoObject, position);

            behShape.Manager.AddBehaviour(apb);
            behShape.Manager.Enabled = true;
            anchoredTo.AddObserver(objecttoanchor as GoObject);
            /*GoObject o = obj as GoObject;
            if (position == PositionLockLocation.BottomCenter 
                ||
                position == PositionLockLocation.BottomLeft 
                || position == PositionLockLocation.BottomRight)
            o.Position = new PointF(o.Position.X, anchoredTo.Bottom);*/
            if (anchoredTo is IBehaviourShape)
            {
                IBehaviourShape ibShape = anchoredTo as IBehaviourShape;
                ibShape.Manager.Enabled = true;
            }
        }

        //when collapsing and expanding the bottom ports become invisible causing links to them to be drawn to the middle of the node.
        private void addremoveBottomPorts()
        {

        }

        public void ForceComputeBounds()
        {
            List.ForceComputeBounds();
            CalculateGridSize();
            Bounds = ComputeBounds();
        }

        // Public Methods (13) 

        //private int handleIndex;
        public void AddHandle()
        {
            GoCollapsibleHandle h = new GoCollapsibleHandle();
            h.Style = GoCollapsibleHandleStyle.PlusMinus;
            h.Size = new SizeF(8, 8);
            h.Brush = Brushes.Yellow;//new SolidBrush(Color.Yellow);
            h.Pen = new Pen(Color.Black, 1);
            h.Bordered = true;
            //h.Brush = null;
            h.Position = new PointF(this.Left + 2, this.Top + 2); //relative point!!
            h.Selectable = false;
            h.Movable = false;
            h.Resizable = false;
            h.Printable = false;
            h.Visible = true;
            h.Printable = false;
            //h.Position = new PointF(this.Left - 4, this.Bottom - 4);
            Add(h);
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

        // when collapsing any nested lists, remember whether they were IsExpanded
        // in the WasExpanded property

        public override GoObject CopyObject(GoCopyDictionary env)
        {
            env.Delayeds.Add(this);
            GoObject retval = base.CopyObject(env);
            CollapsibleNode node = retval as CollapsibleNode;
            node.CopiedFrom = this.MetaObject;

            //node.BindingInfo = BindingInfo.Copy(); 

            if (node.Handle != null)
            {
                node.Handle.Visible = true;
                node.Handle.Printable = false;
            }

            HookupEvents();
            node.HookupEvents();
            if (CopyAsShadow)
            {
                node.MetaObject = MetaObject;
                node.CopyAsShadow = true;
                return node;
            }
            if (MetaObject._ClassName == "DataEntity")//MetaObject._ClassName == "Entity" || 
            {
                if (MetaObject.Get("DataEntityType") != null)//MetaObject.Get("EntityType") != null || 
                {
                    string eT = MetaObject.Get("DataEntityType").ToString();
                    if (eT == "")
                    {
                        skipautomaticsave = true;
                        node.MetaObject.Set("DataEntityType", "O");
                    }
                }
                else
                {
                    skipautomaticsave = true;
                    node.MetaObject.Set("DataEntityType", "O");
                }
            }
            return retval;
        }

        public override void CopyObjectDelayed(GoCopyDictionary env, GoObject newobj)
        {
            bool shallowcopy = CopyAsShadow;
            CollapsibleNode node = newobj as CollapsibleNode;
            if (node.MetaObject != null)
            {
                node.MetaObject.Changed -= FireMetaObjectChanged;
                node.MetaObject = null;
            }
            base.CopyObjectDelayed(env, node);
            foreach (GoObject o in node)
            {
                o.RemoveObserver(node);
                if (o is BoundLabel)
                {
                    BoundLabel lbl = o as BoundLabel;
                    lbl.RemoveObserver(this);
                    lbl.AddObserver(node);
                }
                if (o is IBehaviourShape)
                {
                    o.RemoveObserver(node);
                }
            }
            //node.Name = Guid.NewGuid().ToString();

            if (shallowcopy)
            {
                node.MetaObject = MetaObject;
                node.CopyAsShadow = true;
                CopyAsShadow = false;
            }
            else
            {
                if (BindingInfo.BindingClass != null)
                {
                    node.MetaObject = Loader.CreateInstance(BindingInfo.BindingClass);
                }
                else
                    node.MetaObject = Loader.CreateInstance(MetaObject._ClassName);
                //                node.CreateMetaObject(null, EventArgs.Empty);
                MetaObject.CopyPropertiesToObject(node.MetaObject);
            }

            if (MetaObject != null && !CopyAsShadow)
                MetaObject.CopyPropertiesToObject(node.MetaObject);

            //node.BindingInfo = BindingInfo.Copy();

            node.HookupEvents();
            node.Name = Name.Substring(0, Name.Length);
        }

        // The body is a CollapsingRecordNodeItemList, and is the only
        // child object of this GoNode.  It is referred to via the List property.
        public virtual void CreateBody()
        {
            //if (List == null)//for the inevitability that we add 2 lists -.-
            Add(new CollapsingRecordNodeItemList());
            Grid.Selectable = false;
        }

        // maintain a minimum width when resizing (if this node is Resizable)
        public override void DoResize(GoView view, RectangleF origRect, PointF newPoint, int whichHandle, GoInputState evttype, SizeF min, SizeF max)
        {
            float minw = Math.Max(50, MaxWidth(List));
            // required group width requires adding any margins
            minw += List.TopLeftMargin.Width + List.BottomRightMargin.Width;
            // respect min.Width as well as computed min value
            SizeF newmin = new SizeF(Math.Max(min.Width, minw), Math.Max(min.Height, Height));
            base.DoResize(view, origRect, newPoint, whichHandle, evttype, newmin, max);
        }

        // when expanding any nested lists, respect their .WasExpanded properties

        // find the parent CollapsingRecordNode for any child object at any nested part level
        public static CollapsibleNode FindCollapsingRecordNode(GoObject obj)
        {
            if (obj == null)
                return null;
            CollapsibleNode n = obj as CollapsibleNode;
            if (n != null)
                return n;
            return FindCollapsingRecordNode(obj.Parent);
        }

        //?? test code
        public void Initialize()
        {
            ItemWidth = 140;
        }

        // for convenience in creating new CollapsingRecordNodeItems
        public CollapsingRecordNodeItem MakeItem(String imgname, String s, bool IsHeader)
        {
            CollapsingRecordNodeItem item = new CollapsingRecordNodeItem();
            item.Init(imgname, s, IsHeader);
            return item;
        }

        public void RemoveChildren()
        {
            Collection<GoObject> objectsToRemove = new Collection<GoObject>();
            Collection<RepeaterSection> repeaterSections = RepeaterSections;
            foreach (RepeaterSection rSection in repeaterSections)
            {
                GoGroupEnumerator rsENUM = rSection.GetEnumerator();
                while (rsENUM.MoveNext())
                {
                    if (rsENUM.Current is CollapsingRecordNodeItem)
                    {
                        CollapsingRecordNodeItem item = rsENUM.Current as CollapsingRecordNodeItem;
                        if (!(item.IsHeader))
                            objectsToRemove.Add(item);
                    }
                }
            }
            for (int i = 0; i < objectsToRemove.Count; i++)
            {
                objectsToRemove[i].Remove();
            }
        }

        public void ResetItemWidth()
        {
            List.SetAllIndentation(ItemWidth);
        }

        // Protected Methods (1) 

        protected override void OnChildBoundsChanged(GoObject child, RectangleF old)
        {
            base.OnChildBoundsChanged(child, old);
            if (child != List)
            {
                ResetListPositionAndWidth();
            }
        }

        // Private Methods (4) 

        private float MaxWidth(GoObject obj)
        {
            float w = 0;
            CollapsingRecordNodeItemList list = obj as CollapsingRecordNodeItemList;
            if (list != null)
            {
                foreach (GoObject o in list)
                {
                    w = Math.Max(w, MaxWidth(o));
                }
            }
            else
            {
                if (!obj.AutoRescales)
                {
                    w = Math.Max(w, obj.Width);
                }
            }
            return w;
        }

        private void ResetListPositionAndWidth()
        {
            /*
            return;
            PointF newPosition = new PointF(100000, -100000);
            float MaxX = 0;
            float MinX = 500;
            foreach (GoObject o in this)
            {
                if (!(o is CollapsingRecordNodeItemList) && !(o is GoPort))
                {
                    if (o.Left < newPosition.X)
                        newPosition.X = o.Left;
                    if (o.Bottom > newPosition.Y)
                        newPosition.Y = o.Bottom;

                    if (o.Right > MaxX)
                        MaxX = o.Right;
                    if (o.Left < MinX)
                        MinX = o.Left;
                }
            }
            List.Position = newPosition;
            List.Width = MaxX - MinX;
            List.SetAllItemWidth(MaxX - MinX);*/
        }

        private void SetIndentation(GoObject obj, float w)
        {
            CollapsingRecordNodeItemList list = obj as CollapsingRecordNodeItemList;
            if (list != null)
            {
                list.SetAllIndentation(w);
            }
        }

        // Internal Methods (1) 

        internal void SetAllIndentation(float w)
        {
            foreach (GoObject obj in this)
            {
                SetIndentation(obj, w + ChildIndentation);
            }
        }

        public override bool OnSingleClick(GoInputEventArgs evt, GoView view)
        {
            foreach (GoObject o in List)
                if (o is CollapsingRecordNodeItemList)
                    (o as CollapsingRecordNodeItemList).lastItemSelected = null;

            return base.OnSingleClick(evt, view);
        }

        public void CopyChildrenToNode(CollapsibleNode childNode)
        {
            //clear lists in childnode
            childNode.RemoveChildren();
            Dictionary<string, List<CollapsingRecordNodeItem>> copies = new Dictionary<string, List<CollapsingRecordNodeItem>>();
            //shallow items to correct headings from this to child
            foreach (RepeaterSection rSection in RepeaterSections)
            {
                copies.Add(rSection.Name, new List<CollapsingRecordNodeItem>());
                GoGroupEnumerator rsENUM = rSection.GetEnumerator();
                while (rsENUM.MoveNext())
                {
                    if (rsENUM.Current is CollapsingRecordNodeItem)
                    {
                        CollapsingRecordNodeItem item = rsENUM.Current as CollapsingRecordNodeItem;
                        if (!(item.IsHeader))
                        {
                            copies[rSection.Name].Add(item);
                        }
                    }
                }
            }

            foreach (RepeaterSection rSection in childNode.RepeaterSections)
            {
                if (copies[rSection.Name] != null)
                {
                    foreach (CollapsingRecordNodeItem item in copies[rSection.Name])
                    {
                        CollapsingRecordNodeItem newItem = rSection.AddItem();
                        newItem.MetaObject = item.MetaObject;
                        newItem.HookupEvents();
                        newItem.BindToMetaObjectProperties();
                        newItem.Shadowed = true;
                        rSection.Add(newItem);
                    }
                }
            }
        }

        #endregion Methods
    }
}