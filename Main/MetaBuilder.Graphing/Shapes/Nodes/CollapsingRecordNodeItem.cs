using System;
using System.Collections.Generic;
using System.Drawing;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Graphing.Shapes.Behaviours;
using MetaBuilder.Meta;
using Northwoods.Go;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
using MetaBuilder.Graphing.Shapes.Nodes;
using System.Collections.ObjectModel;

namespace MetaBuilder.Graphing.Shapes
{
    [Serializable]
    public class CollapsingRecordNodeItem : GoTextNode, IMetaNode
    {
        private const float ITEMWIDTH = 169f;

        #region Fields (1)

        [NonSerialized]
        private MetaBase copiedFrom;
        public MetaBase CopiedFrom
        {
            get { return copiedFrom; }
            set { copiedFrom = value; }
        }

        #endregion Fields

        [NonSerialized]
        public EventHandler _contentsChanged;
        private BindingInfo bindingInfo;
        private MetaBase metaObject;
        [NonSerialized]
        private bool myAppearanceSaved;
        [NonSerialized]
        private bool myDrawsInsertionLine;
        [NonSerialized]
        private Brush myOldBackgroundBrush;
        [NonSerialized]
        private Brush myOldHandleBrush;
        [NonSerialized]
        private Pen myOldHandlePen;

        //[NonSerialized]
        //private Color myOldLabelColor = Color.Empty;
        //[NonSerialized]
        //private Color myOldTextColor = Color.Empty;

        private string name;

        public bool IsHeader
        {
            //DC --> DC Bug
            get { return !Editable && MetaObject == null; } // Handle != null; }
        }

        // find the handle, if any, of this item
        public GoCollapsibleHandle Handle
        {
            get
            {
                foreach (GoObject obj in this)
                {
                    GoCollapsibleHandle h = obj as GoCollapsibleHandle;
                    if (h != null)
                    {
                        return h;
                    }
                }
                return null;
            }
        }

        // gets or sets how much empty space there is to the left of the handle and icon
        public float Indentation
        {
            get { return TopLeftMargin.Width; }
            set { TopLeftMargin = new SizeF(value, TopLeftMargin.Height); }
        }

        public bool DrawsInsertionLine
        {
            get { return myDrawsInsertionLine; }
            set
            {
                bool old = myDrawsInsertionLine;
                if (old != value)
                {
                    myDrawsInsertionLine = value;
                    InvalidateViews(); // just need to repaint this item
                }
            }
        }

        public override bool Selectable
        {
            get
            {
                return IsHeader ? false : true;
            }
            set
            {
                base.Selectable = value;
            }
        }
        public override GoObject SelectionObject
        {
            get
            {
                return IsHeader ? this.ParentNode : this;
            }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        #region Constructors (1)

        public CollapsingRecordNodeItem()
        {
            Selectable = true;
            Resizable = false;
            AutoResizes = false;
            Label = new BoundLabel();
            Label.Editable = false;
            Label.Multiline = false;
            //Label.BackgroundColor = Color.Red;

            //DragsNode = true; //disable drag/drop

            AutoRescales = false;
            Resizable = false;
            TopLeftMargin = new SizeF(2, 2);
            BottomRightMargin = new SizeF(2, 2);
        }

        public void RepositionPorts()
        {
            CreatePort(MiddleRight);
        }

        #endregion Constructors

        #region Properties (3)

        // for convenience in accessing any parent CollapsingRecordNodeItemList
        public GoListGroup ParentList
        {
            get { return Parent as GoListGroup; }
        }

        public bool HasBindingInfo
        {
            get
            {
                bool retval = false;
                if (BindingInfo != null)
                {
                    retval = (BindingInfo.BindingClass != null && BindingInfo.BindingClass.Length > 0);
                }
                return retval;
            }
        }

        public bool RequiresAttention
        {
            get { return false; }
            set
            {
                // do nothing
            }
        }

        //public RepeaterSection ParentSection;

        #endregion Properties

        #region Methods (4)

        // Public Methods (2) 

        // Initialize the icon image and label text
        // If HEADER is true, add a GoCollapsibleHandle too
        public virtual void Init(String imgname, String s, bool header)
        {
            foreach (GoObject o in this)
            {
                if (o is GoPort)
                {
                    GoPort prt = o as GoPort;
                    prt.Visible = !header;
                }
            }

            Text = s;
            float space = 0;
            if (Label != null)
            {
                BoundLabel txt = Label as BoundLabel;

                txt.SetEditable(!header);
                txt.originalEditable = !header;

                txt.Multiline = false;
                txt.Wrapping = true;
                txt.WrappingWidth = ITEMWIDTH - 2;
                txt.AutoResizes = true;
                txt.Clipping = false;
                txt.FontSize = 8f;
                txt.Size = new SizeF(ITEMWIDTH - 2, Size.Height - 2);
                Deletable = !header;
                Editable = !header;
                Label.Editable = !header;
                (Label as BoundLabel).Name = "Default";
                if (!header)
                {
                    Label.Alignment = 2; // GoObject.Left;
                }
            }
            Pen = new Pen(Color.Gray);
            /*   if (header)
            {
                Brush = Brushes.Beige;
             
            }
            else
            {
                Brush = Brushes.White;
            }*/
            Background.Position = new PointF(0, 0);
            GoRectangle rect = Background as GoRectangle;
            if (header)
            {
                rect.Brush = Brushes.Beige;//new SolidBrush(Color.Beige);
            }
            else
            {
                rect.Brush = Brushes.White;//new SolidBrush(Color.White);
            }

            rect.Size = new SizeF(ITEMWIDTH, 20);
            TopLeftMargin = new SizeF(TopLeftMargin.Width + space, TopLeftMargin.Height);

            /* if (UserFlags <= 0)
            {
                List<GoObject> portsToRemove = new List<GoObject>();
                foreach (GoObject o in this)
                {
                    if (o is GoPort)
                        portsToRemove.Add(o);
                }
                for (int i = 0; i < portsToRemove.Count; i++)
                {
                    portsToRemove[i].Remove();
                }
            }*/
        }

        public void RemoveLink()
        {
            try
            {
                if (Parent is RepeaterSection)
                {
                    RepeaterSection rParent = Parent as RepeaterSection;
                    CollapsingRecordNodeItemList rGrandPa = rParent.Parent as CollapsingRecordNodeItemList;
                    int associationIndex = rGrandPa.IndexOf(rParent);
                    GraphNode gnodeParent = rGrandPa.ParentNode as GraphNode;
                    RepeaterBindingInfo rbindingInfo = gnodeParent.BindingInfo.RepeaterBindings[associationIndex];
                    ObjectAssociation oa = new ObjectAssociation();
                    oa.CAid = rbindingInfo.Association.ID;
                    oa.ObjectID = gnodeParent.MetaObject.pkid;
                    oa.ObjectMachine = gnodeParent.MetaObject.MachineName;
                    oa.ChildObjectID = MetaObject.pkid;
                    oa.ChildObjectMachine = MetaObject.MachineName;
                    DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Delete(oa);
                }
            }
            catch
            {
#if DEBUG
                // Console.WriteLine(x.ToString());
#endif
            }
        }

        // Protected Methods (2) 

        // don't need the top and bottom ports, so don't create them!
        protected override GoPort CreatePort(int spot)
        {
            if (spot == TopCenter || spot == BottomCenter)
                return null;

            GoPort prt = base.CreatePort(spot);
            //prt.Width = Label.Width + 10;
            //prt.Position = new PointF(Label.Position.X - 5, Label.Position.Y);
            return prt;
        }

        protected override void OnParentChanged(GoGroup oldgroup, GoGroup newgroup)
        {
            base.OnParentChanged(oldgroup, newgroup);

            return; // this was used when dragging & dropping items - will need revision;
            try
            {
                if (newgroup == null)
                {
                    RepeaterSection rsection = oldgroup as RepeaterSection;
                    if (rsection != null)
                    {
                        CollapsibleNode collapsibleNode = rsection.ParentNode as CollapsibleNode;
                        if (collapsibleNode.MetaObject != null && MetaObject != null)
                        {
                            RepeaterBindingInfo rptBinding = collapsibleNode.BindingInfo.GetRepeaterBindingInfo(rsection.Name);
                            ObjectAssociation oass = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(rptBinding.Association.ID, collapsibleNode.MetaObject.pkid, MetaObject.pkid, collapsibleNode.MetaObject.MachineName, MetaObject.MachineName);
                            if (oass != null)
                                DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Delete(oass);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        #endregion Methods

        #region IMetaNode Members

        //Control drag drop entity refresh and this is set to the entity?!
        public MetaBase MetaObject
        {
            get { return metaObject; }
            set
            {
                if (value == null)
                    hooked = false;
                metaObject = value;
            }
        }

        public BindingInfo BindingInfo
        {
            get
            {
                if (bindingInfo == null)
                    bindingInfo = new BindingInfo();
                return bindingInfo;
            }
            set { bindingInfo = value; }
        }

        [NonSerialized]
        private bool hooked = false;
        //[NonSerialized]
        //private List<string> hoookedList = new List<string>();
        public void HookupEvents()
        {
            //if (!hooked)
            ////    return;
            if (MetaObject != null)
            {
                //if (MetaObject.IsChangedNull())
                try
                {
                    MetaObject.Changed -= metaObject_Changed;
                }
                catch
                {
                }
                MetaObject.Changed += metaObject_Changed;
                hooked = true;
                //#if DEBUG
                //                hoookedList.Add(DateTime.Now.ToString() + Environment.NewLine + Environment.StackTrace);
                //#endif
                if (Label != null)
                {
                    Collection<GoObject> removeThese = new Collection<GoObject>();
                    System.Collections.IEnumerator enumerator = Label.Observers.GetEnumerator();
                    while (enumerator.MoveNext())// (object o in Label.Observers.GetEnumerator())
                        if (enumerator.Current is CollapsingRecordNodeItem)
                            removeThese.Add(enumerator.Current as GoObject);

                    foreach (GoObject remove in removeThese)
                        Label.RemoveObserver(remove);
                    Label.AddObserver(this);
                }
                else
                {
                    //when can the label ever be null?
                    this.ToString();
                }
            }
        }
        public void UnHookEvents()
        {
            //if (!hooked)
            ////    return;
            if (MetaObject != null)
            {
                //if (MetaObject.IsChangedNull())
                try
                {
                    MetaObject.Changed -= metaObject_Changed;
                }
                catch
                {
                }
                //MetaObject.Changed += metaObject_Changed;
                hooked = true;
                //#if DEBUG
                //                hoookedList.Add(DateTime.Now.ToString() + Environment.NewLine + Environment.StackTrace);
                //#endif
                if (Label != null)
                {
                    List<GoObject> removeThese = new List<GoObject>();
                    System.Collections.IEnumerator enumerator = Label.Observers.GetEnumerator();
                    while (enumerator.MoveNext())// (object o in Label.Observers.GetEnumerator())
                        if (enumerator.Current is CollapsingRecordNodeItem)
                            removeThese.Add(enumerator.Current as GoObject);

                    foreach (GoObject remove in removeThese)
                        Label.RemoveObserver(remove);
                    //Label.AddObserver(this);
                }
                else
                {
                    //when can the label ever be null?
                    this.ToString();
                }
            }
        }

        public void BindToMetaObjectProperties()
        {
            if (MetaObject != null)
            {
                foreach (KeyValuePair<string, string> kvp in BindingInfo.Bindings)
                {
                    foreach (GoObject o in this)
                    {
                        if (o is BoundLabel)
                        {
                            BoundLabel blo = o as BoundLabel;
                            if (blo.AutoResizes == false)
                            {
                                blo.Wrapping = true;
                                blo.WrappingWidth = blo.Width - 2;
                                blo.AutoResizes = true;
                            }

                            if (blo.Name == kvp.Key)
                            {
                                object obj = MetaObject.Get(kvp.Value);
                                if (obj != null)
                                    blo.Text = obj.ToString();
                            }
                        }
                    }
                }
                /*   try
                {
                    this.Text = metaObject.Get(BindingInfo.Bindings["Default"]).ToString();
                }
                catch
                {
                }*/
            }
        }
        public void BindMetaObjectImage()
        {
        }
        public void FireMetaObjectChanged(object sender, EventArgs e)
        {
            OnContentsChanged(sender, e);
            BindToMetaObjectProperties();
            if (Core.Variables.Instance.SaveOnCreate)
            {
                SaveToDatabase(sender, e);
                List<GoObject> remove = new List<GoObject>();
                foreach (GoObject o in this)
                    if (o is IndicatorLabel)
                        if ((o as IndicatorLabel).TextColor == Color.Red)
                        {
                            remove.Add(o);
                        }
                foreach (GoObject o in remove)
                    o.Remove();
            }
        }

        public EventHandler ContentsChanged
        {
            get { return _contentsChanged; }
            set { _contentsChanged = value; }
        }

        public virtual void OnContentsChanged(object sender, EventArgs e)
        {
            if (ContentsChanged != null)
            {
                ContentsChanged(this, e);
            }
        }

        #endregion

        protected override void OnChildBoundsChanged(GoObject child, RectangleF old)
        {
            if (child is BoundLabel)
            {
                float maxHeight = 0;
                GoGroupEnumerator groupEnum = GetEnumerator();
                while (groupEnum.MoveNext())
                {
                    if (groupEnum.Current is BoundLabel)
                        if (groupEnum.Current.Visible)
                            if (groupEnum.Current.Height > maxHeight)
                                maxHeight = groupEnum.Current.Height;
                }
                if (maxHeight > 0)
                    Background.Height = maxHeight + 5;
                //DataTable Label Fix
                child.Width = Background.Width;
            }

            base.OnChildBoundsChanged(child, old);
        }

        // make sure the GoImage is in the proper spot on the left side of the label
        public override void LayoutChildren(GoObject childchanged)
        {
            if (childchanged is IndicatorLabel || childchanged is ChangedIndicatorLabel)
                return;
            base.LayoutChildren(childchanged);
            GoObject lab = Label;
            if (lab != null)
            {
                // find the GoImage and GoCollapsibleHandle (which might not have been Add'ed to this group)
                GoObject h = null;
                foreach (GoObject child in this)
                {
                    if (child is GoCollapsibleHandle)
                    {
                        h = child;
                    }
                    /* if (child is GoPort)
                    {
                        GoPort prt = child as GoPort;
                        if (Handle != null)
                            prt.Visible = false;
                        else
                        if (prt.ToSpot == GoObject.MiddleRight)
                        {
                            prt.Position = new PointF(this.Position.X+180,prt.Position.Y);
                            prt.Brush = new SolidBrush(Color.Black);
                            prt.Visible = true;
                            GoText txt = new GoText();
                            txt.Text = "x";
                            txt.Position = prt.Position;
                            if (this.Parent!=null)
                            this.Parent.Add(txt);
                            //this.View.Document.Add(txt);
                        }
                    }*/
                }
                if (h != null)
                {
                    h.Printable = false;
                    PointF p = lab.GetSpotLocation(MiddleLeft);
                    p.X -= 2;
                    h.SetSpotLocation(MiddleRight, p);
                }
            }
        }

        // handle double-click on a header--Collapse or Expand the list
        public override bool OnDoubleClick(GoInputEventArgs evt, GoView view)
        {
            GoCollapsibleHandle h = Handle;
            if (h != null)
            {
                IGoCollapsible coll = h.FindCollapsible();
                if (coll != null && coll.Collapsible)
                {
                    if (coll.IsExpanded)
                        coll.Collapse();
                    else
                        coll.Expand();
                    return true;
                }
            }

            if (IsHeader)
            {
                CollapsibleNode pNode = this.ParentNode as CollapsibleNode;
                if (pNode.MetaObject != null)
                    if (pNode.MetaObject.State == VCStatusList.CheckedIn || pNode.MetaObject.State == VCStatusList.CheckedOutRead || pNode.MetaObject.State == VCStatusList.Locked || pNode.MetaObject.State == VCStatusList.Obsolete)
                        return base.OnDoubleClick(evt, view); //End addition of new nodes to readonly
                (ParentList as CollapsingRecordNodeItemList).AddItemFromCode();

                (ParentList as CollapsingRecordNodeItemList).lastItemSelected = null;
                ParentList.InvalidateViews();
            }
            return base.OnDoubleClick(evt, view);
        }

        // whether to draw a bright line at the bottom of the item,
        // indicating a potential drop point
        public override void Paint(Graphics g, GoView view)
        {
            if (ParentList != null)
            {
                if (ParentList is CollapsingRecordNodeItemList)
                {
                    if ((ParentList as CollapsingRecordNodeItemList).lastItemSelected == this)
                    {
                        Pen p = new Pen(new SolidBrush(Color.ForestGreen), 1);
                        PointF StartPoint = new PointF(this.Location.X - 2, this.Location.Y - 1);
                        PointF LeftTop = new PointF(StartPoint.X - 3, StartPoint.Y - 3);
                        PointF LeftBottom = new PointF(StartPoint.X - 3, StartPoint.Y + 3);
                        //g.DrawLine(p, new PointF(this.Left, this.Top + 1), new PointF(this.Right, this.Top + 1));
                        PointF[] triangle = new PointF[] { StartPoint, LeftTop, LeftBottom };
                        g.DrawPolygon(p, triangle);
                        p.Dispose();
                    }
                }
            }

            //if (this.DrawsInsertionLine)
            //{
            //    Pen p = new Pen(new SolidBrush(Color.RoyalBlue), 2);
            //    PointF StartPoint = new PointF(this.Location.X - 1, this.Location.Y - 1);
            //    PointF LeftTop = new PointF(StartPoint.X - 3, StartPoint.Y - 3);
            //    PointF LeftBottom = new PointF(StartPoint.X - 3, StartPoint.Y + 3);
            //    //g.DrawLine(p, new PointF(this.Left, this.Top + 1), new PointF(this.Right, this.Top + 1));
            //    PointF[] triangle = new PointF[] { StartPoint, LeftTop, LeftBottom };
            //    g.DrawPolygon(p, triangle);
            //    p.Dispose();
            //}


            // Bounds = ComputeBounds();
            //if (!IsHeader)
            //{
            //    g.DrawRectangle(new Pen(Brushes.Gray), new Rectangle(new Point((int)this.Position.X, (int)this.Position.Y), new Size((int)Bounds.Width - 1, (int)Bounds.Height)));
            //    g.DrawLine(new Pen(Brushes.Gray), new Point((int)this.Position.X, (int)this.Bottom - 1), new Point((int)this.Right, (int)this.Bottom - 1));
            //    this.Background = null;
            //}
            /* hACK!
            if (Label != null)
            {
                GoText txt = Label;
                if (Label.Text == "Descriptives Attributes")
                    Label.Text = "Descriptive Attributes";
            }*/

            //if (Handle != null)
            //Handle.Selectable = false;

            base.Paint(g, view);
        }

        // go up the Parent chain looking for the first CollapsingRecordNodeItemList
        // that is both Insertable and IsExpanded; an item or list can be inserted
        // right after this item's spot in the list.
        public virtual GoListGroup FindInsertableList()
        {
            GoListGroup list = ParentList;
            //while (list != null && (!list.Insertable || !list.IsExpanded))
            //{
            //    list = list.ParentList;
            //}
            return list;
        }

        // when dragging (not on mouse-over) into this item, and when
        // dropping the Selection is not rejected, have this item draw an
        // insertion line to indicate that a drop may occur here.

        GoCollection collection;
        public override bool OnEnterLeave(GoObject from, GoObject to, GoView view)
        {
            if (view.Tool is GoToolDragging)
            {
                if (view.Selection.Count > 0)
                {
                    collection = new GoCollection();
                    collection.AddRange(view.Selection);
                }
                if (ParentList != null)
                    ParentList.InvalidateViews();

                if (ParentNode != null)
                    ParentNode.InvalidateViews();

                if (!view.DoSelectionDropReject(view.LastInput))
                {
                    if (ParentList != null)
                        if (ParentList is CollapsingRecordNodeItemList)
                            (ParentList as CollapsingRecordNodeItemList).lastItemSelected = null;
                    DrawsInsertionLine = true;
                }
                else
                {
                    DrawsInsertionLine = false;
                }
            }
            return base.OnEnterLeave(from, to, view);
        }

        // Reject any drop if there's no Insertable CollapsingRecordNodeItemList,
        // or if the primary object to be dropped is not a CollapsingRecordNodeItem
        // or CollapsingRecordNodeItemList.
        public override bool OnSelectionDropReject(GoView view)
        {
            //if (!view.CanInsertObjects())
            //    return true;
            GoListGroup list = FindInsertableList();
            if (list == null)
                return true;
            if (view.Selection.Primary is CollapsingRecordNodeItemList)
                return true;

            if (view.Selection.Primary is CollapsingRecordNodeItem) //false meaning it is not being rejected
                return false;

            return true;
        }

        // handle any drop of a CollapsingRecordNodeItem or CollapsingRecordNodeItemList
        // by inserting it into the tree at that spot
        public override bool OnSelectionDropped(GoView view)
        {
            // normally want to add selected objects into this item's ParentList
            GoListGroup list = FindInsertableList();
            if (list != null)
            {
                IMetaNode parentNode = null;
                // figure out where to add the selected objects
                GoObject child = this;
                while (child != null && child.Parent != list)
                    child = child.Parent;
                if (child == null)
                    return true; // failure to find where to insert new object(s)
                // insert new objects immediately after this child
                int idx = list.IndexOf(child);
                // weed out any selected objects that aren't CollapsingRecordNodeItem[List]s
                GoCollection newcoll = new GoCollection();
                GoCollection removeCol = new GoCollection();

                if (view.Selection.Count > 0)
                {
                    collection = new GoCollection();
                    collection.AddRange(view.Selection);
                }
                //section for attributes, list for fields
                //Node-->List(?Section)-->List(?Section)-->ParentNode
                if (collection != null)
                {
                    foreach (GoObject obj in collection)
                    {
                        if (obj is CollapsingRecordNodeItem)
                        {
                            if ((obj as CollapsingRecordNodeItem).ParentList != null)
                            {
                                //try not to mix list types
                                if (list.GetType() == (obj as CollapsingRecordNodeItem).ParentList.GetType())
                                {
                                    string section = "";
                                    if ((obj as CollapsingRecordNodeItem).ParentList is RepeaterSection)
                                        section = ((obj as CollapsingRecordNodeItem).ParentList as RepeaterSection).Name;
                                    else if ((obj as CollapsingRecordNodeItem).ParentList is CollapsingRecordNodeItemList)
                                        section = ((obj as CollapsingRecordNodeItem).ParentList as CollapsingRecordNodeItemList).Name;
                                    //mark previous association for delete
                                    if (list != (obj as CollapsingRecordNodeItem).ParentList)
                                    {
                                        if (parentNode == null)
                                            parentNode = (obj as CollapsingRecordNodeItem).ParentList.Parent.Parent as IMetaNode;
                                        MarkPreviousMappingForDelete(parentNode, obj as IMetaNode, section);
                                    }

                                    if (view.LastInput.Control)
                                        newcoll.Add(obj.Copy());
                                    else
                                        newcoll.Add(obj);
                                }
                            }
                            else
                            {
                                string originalParentsClass = ((obj as CollapsingRecordNodeItem).ParentNode as IMetaNode).MetaObject.Class;
                                if ((list.ParentNode as CollapsibleNode).BindingInfo.RepeaterBindings[0].Association.ChildClass == originalParentsClass)
                                {
                                    //string section = "";
                                    //if ((obj as CollapsingRecordNodeItem).ParentList is RepeaterSection)
                                    //    section = ((obj as CollapsingRecordNodeItem).ParentList as RepeaterSection).Name;
                                    //else if ((obj as CollapsingRecordNodeItem).ParentList is CollapsingRecordNodeItemList)
                                    //    section = ((obj as CollapsingRecordNodeItem).ParentList as CollapsingRecordNodeItemList).Name;
                                    //mark previous association for delete
                                    //if (list != (obj as CollapsingRecordNodeItem).ParentList)
                                    //MarkPreviousMappingForDelete(((obj as CollapsingRecordNodeItem).ParentNode as IMetaNode), obj as IMetaNode, section);

                                    if (view.LastInput.Control)
                                        newcoll.Add(obj.Copy());
                                    else
                                        newcoll.Add(obj);
                                }
                                else
                                    removeCol.Add(obj);
                            }
                        }
                        //else if (obj is CollapsingRecordNodeItemList)
                        //    newcoll.Add(obj);
                    }
                }
                foreach (GoObject o in removeCol)
                    o.Remove();

                if (!newcoll.IsEmpty)
                {
                    // if moving an item to later in the same list, decrement the index
                    //if (newcoll.First.Parent == list && list.IndexOf(newcoll.First) < idx)
                    //{
                    //    idx--;
                    //}
                    // add them all to the LIST
                    IGoCollection addedcoll = list.AddCollection(newcoll, true); //moves item from 1 collection to another
                    foreach (GoObject obj in addedcoll)
                    {
                        if (obj.Parent == null)
                            continue;
                        obj.Shadowed = false;
                        if (obj.Parent == list)
                        {
                            list.Insert(++idx, obj);
                        }
                    }
                    // update the view's Selection
                    //view.Selection.Clear();
                    //view.Selection.AddRange(addedcoll);
                    // make sure all the children are laid out at their new positions
                    list.LayoutChildren(null);
                }
                if (parentNode != null)
                {
                    OnRefreshAllCollapsibleNodesWithSameID(parentNode, EventArgs.Empty);
                }

                return true;
            }
            else
            {
                GoCollection removeCol = new GoCollection();
                foreach (GoObject obj in view.Selection)
                    removeCol.Add(obj);
                foreach (GoObject obj in removeCol)
                    obj.Remove();
            }
            //view.Selection.Clear();
            collection = null;
            return base.OnSelectionDropped(view);
        }

        private void MarkPreviousMappingForDelete(IMetaNode parent, IMetaNode child, string section)
        {
            int caid = 0;
            //get CAID?

            TList<ClassAssociation> associations = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.Find("ParentClass='" + parent.MetaObject.Class + "' AND ChildClass='" + child.MetaObject.Class);
            foreach (ClassAssociation classAssociation in associations)
            {
                if (classAssociation.IsActive == true && classAssociation.AssociationTypeID == 4)
                {
                    if (section.Contains("Key"))
                    {
                        if (classAssociation.Caption.Contains("Key"))
                        {
                            caid = classAssociation.CAid;
                            break;
                        }
                    }
                    else if (section.Contains("Descrip"))
                    {
                        if (classAssociation.Caption.Contains("Descrip"))
                        {
                            caid = classAssociation.CAid;
                            break;
                        }
                    }
                    else
                    {
                        caid = classAssociation.CAid;
                    }
                }
            }

            ObjectAssociation existingComplexAss = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(caid, parent.MetaObject.pkid, child.MetaObject.pkid, parent.MetaObject.MachineName, child.MetaObject.MachineName);
            if (existingComplexAss != null)
            {
                existingComplexAss.State = VCStatusList.MarkedForDelete;
                DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Save(existingComplexAss);
                //inverse?
            }

        }

        [field: NonSerialized]
        public event EventHandler RefreshAllCollapsibleNodesWithSameID;
        public void OnRefreshAllCollapsibleNodesWithSameID(object sender, EventArgs e)
        {
            if (RefreshAllCollapsibleNodesWithSameID != null)
                RefreshAllCollapsibleNodesWithSameID(sender, e);
        }

        // don't add the usual selection handle(s)--instead just
        // change the color of the GoTextNode.Background shape.
        public override void AddSelectionHandles(GoSelection sel, GoObject selectedObj)
        {
            if ((selectedObj as CollapsingRecordNodeItem).ParentList == null)
            {
                selectedObj.Remove();
                return;
            }
            if (!myAppearanceSaved)
            {
                myAppearanceSaved = true;
                Color selColor = sel.View.PrimarySelectionColor;
                CollapsibleNode node = CollapsibleNode.FindCollapsingRecordNode(this);
                if (node != null && node.SelectionColor != Color.Empty)
                    selColor = node.SelectionColor;
                Color selTextColor = Color.Black;
                if (node != null && node.SelectionTextColor != Color.Empty)
                    selTextColor = node.SelectionTextColor;
                Background.SkipsUndoManager = true; // don't record any of these changes for undo/redo
                myOldBackgroundBrush = Brush;
                Brush = new SolidBrush(selColor);
                Background.SkipsUndoManager = false;
                GoText lab = Label;
                if (lab != null)
                {
                    lab.SkipsUndoManager = true;
                    //myOldTextColor = lab.TextColor;
                    //myOldLabelColor = lab.BackgroundColor;
                    //lab.TextColor = selTextColor;
                    lab.BackgroundColor = selColor;
                    lab.SkipsUndoManager = false;
                }
                GoCollapsibleHandle h = Handle;
                if (h != null)
                {
                    h.SkipsUndoManager = true;
                    myOldHandleBrush = h.Brush;
                    myOldHandlePen = h.Pen;
                    h.Brush = new SolidBrush(selColor);
                    h.Pen = new Pen(selTextColor);
                    h.SkipsUndoManager = false;
                }
            }

        }

        public override void RemoveSelectionHandles(GoSelection sel)
        {
            if (myAppearanceSaved)
            {
                myAppearanceSaved = false;
                if (Background != null)
                {
                    Background.SkipsUndoManager = true; // don't record any of these changes for undo/redo
                    Brush = myOldBackgroundBrush;
                    Background.SkipsUndoManager = false;
                }
                GoText lab = Label;
                if (lab != null)
                {
                    lab.SkipsUndoManager = true;
                    //if (myOldTextColor == Color.Empty) myOldTextColor = Color.Black;
                    //lab.TextColor = myOldTextColor;
                    //lab.BackgroundColor = myOldLabelColor;
                    lab.SkipsUndoManager = false;
                }
                GoCollapsibleHandle h = Handle;
                if (h != null)
                {
                    h.SkipsUndoManager = true;
                    h.Brush = myOldHandleBrush;
                    if (myOldHandlePen == null) myOldHandlePen = Pens.Gray;
                    h.Pen = myOldHandlePen;
                    h.SkipsUndoManager = false;
                }
            }
        }

        // CollapsingRecordNodeItem state (none that is persistent)

        public virtual void metaObject_Changed(object sender, EventArgs e)
        {
            if (ParentList != null)
                if (ParentList is CollapsingRecordNodeItemList)
                    (ParentList as CollapsingRecordNodeItemList).lastItemSelected = null;

            OnContentsChanged(sender, e);
            BindToMetaObjectProperties();

            if (Core.Variables.Instance.SaveOnCreate)
            {
                SaveToDatabase(sender, e);
            }
        }

        protected override void OnObservedChanged(GoObject observed, int subhint, int oldI, object oldVal, RectangleF oldRect, int newI, object newVal, RectangleF newRect)
        {
            switch (subhint)
            {
                case 1501: // Probably Boundlabel's Text that changed
                    if (observed is GoText)
                    {
                        GoText culprit = observed as GoText;
                        LabelTextChanged(culprit, (string)oldVal, (string)newVal);
                    }
                    break;
            }

            foreach (GoObject link in this.Links)
            {
                link.Visible = this.Visible;
            }

            base.OnObservedChanged(observed, subhint, oldI, oldVal, oldRect, newI, newVal, newRect);
        }

        public void LabelTextChanged(GoText label, string OldText, string NewText)
        {
            try
            {
                if (MetaObject != null)
                {
                    IIdentifiable lbl = label as IIdentifiable;
                    if (lbl != null)
                    {
                        foreach (KeyValuePair<string, string> kvpair in bindingInfo.Bindings)
                        {
                            if (kvpair.Key == lbl.Name)
                            {
                                MetaObject.Set(kvpair.Value, NewText);
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        public BoundLabel GetLabel
        {
            get
            {
                BoundLabel lbl = null;
                foreach (GoObject o in this)
                {
                    if (!(o is BoundLabel))
                        continue;

                    if (this is ExpandableLabel) //There are two labels
                    {
                        if ((o as BoundLabel).Name == "Name")
                            lbl = o as BoundLabel;
                    }
                    else
                    {
                        lbl = o as BoundLabel;
                    }
                }
                return lbl;
            }
            set
            {
                BoundLabel lbl = null;
                foreach (GoObject o in this)
                {
                    if (!(o is BoundLabel))
                        continue;

                    if (this is ExpandableLabel) //There are two labels
                    {
                        if ((o as BoundLabel).Name == "Name")
                            lbl = o as BoundLabel;
                    }
                    else
                    {
                        lbl = o as BoundLabel;
                    }
                }
                if (lbl != null)
                {
                    lbl.FamilyName = value.FamilyName;
                    lbl.FontSize = value.FontSize;
                    lbl.Bold = value.Bold;
                    lbl.Italic = value.Italic;
                    lbl.StrikeThrough = value.StrikeThrough;
                    lbl.Underline = value.Underline;
                    lbl.TextColor = value.TextColor;
                }
            }
        }

        public override GoObject CopyObject(GoCopyDictionary env)
        {
            //env.Delayeds.Add(this);
            env.Delayeds.Add(this);
            GoObject retval = base.CopyObject(env);
            GraphNode pnode = this.ParentNode as GraphNode;
            CollapsingRecordNodeItem node = retval as CollapsingRecordNodeItem;
            node.CopiedFrom = this.MetaObject;
            node.BindingInfo = this.BindingInfo;
            if (pnode != null)
            {
                if (pnode.CopyAsShadow)
                {
                    if (!node.IsHeader)
                    {
                        node.MetaObject = this.MetaObject;
                        node.HookupEvents();
                    }

                    if (node.ParentList == null)
                        return null;
                    return node;
                }
                else
                {
                    if (!node.IsHeader)
                    {
                        node.MetaObject = Loader.CreateInstance(this.BindingInfo.BindingClass);
                        MetaObject.CopyPropertiesToObject(node.MetaObject);
                        node.HookupEvents();
                    }

                    return node;
                }
            }
            return retval;
        }

        public override void CopyObjectDelayed(GoCopyDictionary env, GoObject newobj)
        {
            CollapsingRecordNodeItem item = newobj as CollapsingRecordNodeItem;
            GraphNode pnode = this.ParentNode as GraphNode;
            if (pnode == null)
            {
                newobj.Remove();
                return;
            }
            bool shallowcopy = pnode.CopyAsShadow;
            base.CopyObjectDelayed(env, newobj);
            if (!item.IsHeader)
            {
                item.MetaObject = null;
                if (bindingInfo != null)
                    item.BindingInfo = BindingInfo.Copy();
                if (shallowcopy)
                {
                    item.MetaObject = MetaObject;
                }
                else
                {
                    if (BindingInfo.BindingClass != null)
                    {
                        item.MetaObject = Loader.CreateInstance(BindingInfo.BindingClass);
                    }
                    else
                    {
                        CollapsibleNode cnode = ParentNode as CollapsibleNode;
                        RepeaterSection rsection = Parent as RepeaterSection;
                        foreach (RepeaterBindingInfo rbinfo in cnode.BindingInfo.RepeaterBindings)
                        {
                            if (rbinfo.RepeaterName == rsection.Name)
                            {
                                item.MetaObject = Loader.CreateInstance(rbinfo.Association.ChildClass);
                            }
                        }
                    }
                }
                item.HookupEvents();
                if (MetaObject != null && !pnode.CopyAsShadow)
                {
                    MetaObject.CopyPropertiesToObject(item.MetaObject);
                }
            }
        }

        public override void OnGotSelection(GoSelection sel)
        {
            if (IsHeader)
                return;

            if (ParentList == null)
                return;

            if (ParentList is CollapsingRecordNodeItemList)
                if ((ParentList as CollapsingRecordNodeItemList).lastItemSelected == this)
                    (ParentList as CollapsingRecordNodeItemList).lastItemSelected = null;
                else
                    (ParentList as CollapsingRecordNodeItemList).lastItemSelected = this;

            foreach (GoObject o in ParentList)
                o.InvalidateViews();

            if (ParentList.ParentNode.View != null)
                ParentList.ParentNode.View.Invalidate(new Region(ParentList.Bounds), true);
        }

        #region MetaObject

        public void CreateMetaObject(object sender, EventArgs e)
        {
            try
            {
                if (BindingInfo.BindingClass != null)
                {
                    MetaObject = Loader.CreateInstance(BindingInfo.BindingClass);
                    HookupEvents();
                }
            }
            catch
            {
            }
        }

        public void LoadMetaObject(int ID, string Machine)
        {
            if (HasBindingInfo)
            {
                if (BindingInfo.BindingClass != null)
                {
                    MetaObject = Loader.GetByID(bindingInfo.BindingClass, ID, Machine);
                    HookupEvents();
                    BindToMetaObjectProperties();
                    //FireMetaObjectChanged(null, EventArgs.Empty);
                }
            }
        }

        public virtual b.TList<b.ObjectAssociation> SaveToDatabase(object sender, EventArgs e)
        {
            SaveToDatabase(sender, e, null);
            return null;
        }

        public virtual void SaveToDatabase(object sender, EventArgs e, string Provider)
        {
            if (Provider == null)
                Provider = Core.Variables.Instance.ClientProvider;
            if (MetaObject != null)
            {
                try
                {
                    MetaObject.SaveToRepository(Guid.NewGuid(), Provider);

                    //Save up
                    if (ParentList != null && Core.Variables.Instance.SaveOnCreate)
                    {
                        if (ParentList is CollapsingRecordNodeItemList)
                        {
                            if ((ParentList as CollapsingRecordNodeItemList).ParentNode is CollapsibleNode)
                            {
                                CollapsibleNode pNode = (ParentList as CollapsingRecordNodeItemList).ParentNode as CollapsibleNode;
                                if (pNode.MetaObject == null || pNode.MetaObject.pkid == 0)
                                {
                                    Core.Log.WriteLog("Cannot bubble save from collapsiblenodeChild");
                                    return;
                                }

                                ObjectAssociation oass = new ObjectAssociation();

                                TList<ClassAssociation> associations = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.Find("ParentClass='" + pNode.MetaObject.Class + "' AND ChildClass='" + MetaObject.Class);
                                //string repeaterName = (ParentList is RepeaterSection) ? (ParentList as RepeaterSection).Name : (ParentList is CollapsingRecordNodeItemList) ? (ParentList as CollapsingRecordNodeItemList).Name : "";
                                RepeaterBindingInfo info = pNode.BindingInfo.GetRepeaterBindingInfo((ParentList is RepeaterSection) ? (ParentList as RepeaterSection).Name : (ParentList is CollapsingRecordNodeItemList) ? (ParentList as CollapsingRecordNodeItemList).Name : "");
                                foreach (ClassAssociation classAssociation in associations)
                                {
                                    if (classAssociation.IsActive == true && classAssociation.AssociationTypeID == 4)
                                    {
                                        if (info.RepeaterName.Contains("Key"))
                                        {
                                            if (classAssociation.Caption.Contains("Key"))
                                            {
                                                oass.CAid = classAssociation.CAid;
                                                break;
                                            }
                                        }
                                        else if (info.RepeaterName.Contains("Descrip"))
                                        {
                                            if (classAssociation.Caption.Contains("Descrip"))
                                            {
                                                oass.CAid = classAssociation.CAid;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            oass.CAid = classAssociation.CAid;
                                        }
                                    }
                                }

                                oass.ObjectID = pNode.MetaObject.pkid;
                                oass.ChildObjectID = MetaObject.pkid;
                                oass.ObjectMachine = pNode.MetaObject.MachineName;
                                oass.ChildObjectMachine = MetaObject.MachineName;

                                ObjectAssociation existingComplexAss = DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(oass.CAid, oass.ObjectID, oass.ChildObjectID, oass.ObjectMachine, oass.ChildObjectMachine);
                                if (existingComplexAss == null)
                                {
                                    oass.VCStatusID = 7;
                                    //assocsToSave.Add(oass);
                                    DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.Save(oass);
                                }
                                else
                                {
                                    if (existingComplexAss.State == VCStatusList.MarkedForDelete && MetaObject.WorkspaceTypeId < 3)
                                        existingComplexAss.VCStatusID = (int)VCStatusList.None;
                                    //else
                                    //    oass.VCStatusID = existingComplexAss.VCStatusID;

                                    //assocsToUpdate.Add(existingComplexAss);
                                    DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.Save(existingComplexAss);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Core.Log.WriteLog("Cannot save CollapsibleNode : " + MetaObject.ToString() + "" + Environment.NewLine + ex.ToString());
                    //metaObject._pkid = 0;
                    //MetaObject.Reset();
                    //metaObject.SaveToRepository(Guid.NewGuid(), Provider);
                }

                //List<GoObject> remove = new List<GoObject>();
                //foreach (GoObject o in this)
                //    if (o is IndicatorLabel)
                //        if ((o as IndicatorLabel).TextColor == Color.Red)
                //        {
                //            remove.Add(o);
                //        }
                //foreach (GoObject o in remove)
                //    o.Remove();
            }
        }

        private bool parentIsILinkedContainer;
        public bool ParentIsILinkedContainer
        {
            get { return parentIsILinkedContainer; }
            set { parentIsILinkedContainer = value; }
        }

        #endregion

        public override bool Shadowed
        {
            get
            {
                return base.Shadowed;
            }
            set
            {
                base.Shadowed = value;
            }
        }

    }
}