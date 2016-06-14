using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Graphing.Controllers;
using MetaBuilder.Graphing.Shapes.Behaviours;
using MetaBuilder.Graphing.Shapes.Behaviours.Observers;
using MetaBuilder.Meta;
using Northwoods.Go;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.Graphing.Shapes.Nodes.Containers
{
    [Serializable]
    public class SubgraphNode : GoSubGraph, IShallowCopyable, ILinkedContainer, IMetaNode, IGoDragSnapper
    {
        #region ILinkedContainer Implementation

        public bool failedAdd = false;
        public void PerformAddCollection(GoView view, GoCollection objectCollection)
        {
            failedAdd = false;
            try
            {
                AddCollection(objectCollection, true);
            }
            catch (Exception ex)
            {
                failedAdd = true;
                Core.Log.WriteLog("PerformAddCollection" + Environment.NewLine + ex.ToString());
            }
        }

        public string LabelText
        {
            get
            {
                return Label.Text;
            }
        }

        public override void Expand()
        {
            base.Expand();
        }

        public override void Collapse()
        {
            base.Collapse();
        }

        public bool ObjectInAcceptedRegion(GoObject o)
        {
            if (o is MappingCell || o is ArtefactNode || o is Rationale || o.DraggingObject is ArtefactNode || o.DraggingObject is Rationale)
                return false;
            return Bounds.Contains(o.Center);
        }

        public bool Locked
        {
            get { return false; }
        }

        #endregion

        //additionalilinkedcontainerimplement
        private SubgraphNode originalParent;
        public SubgraphNode OriginalParent
        {
            get { return originalParent; }
            set { originalParent = value; }
        }

        private bool copyAsShadow;
        private string name;

        private bool isStencil;
        public bool IsStencil
        {
            get { return isStencil; }
            set
            {
                isStencil = value;
                if (isStencil)
                {
                    BindingInfo = null;
                    MetaObject = null;
                    return;
                }
            }
        }

        #region IShallowCopyable Members

        public string Name
        {
            get
            {
                if (name != null)
                {
                    if (name != string.Empty)
                        return name;
                }
                name = "SGN" + Guid.NewGuid().ToString().Substring(0, 5);
                return name;
            }
            set { name = value; }
        }

        public bool CopyAsShadow
        {
            get { return copyAsShadow; }
            set { copyAsShadow = value; }
        }

        public GoObject CopyAsShallow()
        {
            MetaBase mo = MetaObject;
            SubgraphNode node = Copy() as SubgraphNode;
            node.MetaObject = mo;
            node.HookupEvents();
            node.BindToMetaObjectProperties();
            CopyAsShadow = true;
            markAllChildrenShallowCopyable();
            CopyAsShadow = false;
            return node;
        }

        private bool parentIsILinkedContainer;
        public bool ParentIsILinkedContainer
        {
            get { return parentIsILinkedContainer; }
            set { parentIsILinkedContainer = value; }
        }

        #endregion

        [NonSerialized]
        private MetaBase copiedFrom;
        public MetaBase CopiedFrom
        {
            get { return copiedFrom; }
            set { copiedFrom = value; }
        }
        public override GoObject CopyObject(GoCopyDictionary env)
        {
            env.Delayeds.Add(this);
            GoObject retval = base.CopyObject(env);

            List<GoObject> removeThese = new List<GoObject>();
            foreach (DictionaryEntry o in env)
            {
                if (o.Value.GetType().ToString().Contains("NumberingText"))
                {
                    removeThese.Add(o.Value as GoObject);
                }
            }
            foreach (GoObject o in removeThese)
            {
                o.Remove();
                env.Remove(o);
                env.Delayeds.Remove(o);
            }
            removeThese = null;

            Hooked = false;
            markAllChildrenShallowCopyable();

            SubgraphNode node = retval as SubgraphNode;
            node.CopiedFrom = this.MetaObject;
            node.Hooked = false;
            if (CopyAsShadow)
            {
                node.MetaObject = MetaObject;
                node.HookupEvents();
                node.CopyAsShadow = true;
                return node;
            }

            rebuildObjectRelationships(retval as SubgraphNode);

            return retval;
        }

        public void markAllChildrenShallowCopyable()
        {
            foreach (GoObject o in this)
            {
                if (o is IShallowCopyable)
                {
                    (o as IShallowCopyable).CopyAsShadow = CopyAsShadow;
                    if (o is SubgraphNode)
                        (o as SubgraphNode).markAllChildrenShallowCopyable();
                }
            }
        }

        public void rebuildObjectRelationships(SubgraphNode node)
        {
            node.ObjectRelationships = new List<EmbeddedRelationship>();
            node.DefaultClassBindings = new Dictionary<string, ClassAssociation>();
            //return;

            foreach (GoObject obj in node)
            {
                if (!(obj is IMetaNode))
                    continue;
                EmbeddedRelationship emrel = new EmbeddedRelationship();

                IMetaNode childNode = obj as IMetaNode;
                MetaBase mbase = Loader.CreateInstance((obj as IMetaNode).MetaObject.Class);
                (obj as IMetaNode).MetaObject.CopyPropertiesToObject(mbase);

                childNode.MetaObject = mbase;
                childNode.HookupEvents();
                childNode.FireMetaObjectChanged(this, new EventArgs());

                emrel.MyMetaObject = childNode.MetaObject;

                TList<ClassAssociation> allowedAssociations = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByChildClass(mbase._ClassName);
                allowedAssociations.Filter = "ParentClass = '" + node.MetaObject._ClassName + "' AND IsActive = 'True'";

                if (allowedAssociations.Count > 0)
                {
                    emrel.MyAssociation = allowedAssociations[0];
                    bool added = false;

                    #region Add Class Associations for each class object that is allowed

                    foreach (ClassAssociation classAssoc in allowedAssociations)
                    {
                        if (classAssoc.IsDefault)
                        {
                            if (node.DefaultClassBindings == null)
                                node.DefaultClassBindings = new Dictionary<string, ClassAssociation>();
                            if (!(node.DefaultClassBindings.ContainsKey(classAssoc.ChildClass)))
                                node.DefaultClassBindings.Add(classAssoc.ChildClass, classAssoc);
                            emrel.MyAssociation = classAssoc;
                            node.ObjectRelationships.Add(emrel);
                            added = true;
                        }
                    }

                    // at this stage, no default associations were added, because there is no default specified. Just add the first one as the default
                    if (!added)
                    {
                        if (node.DefaultClassBindings == null)
                            node.DefaultClassBindings = new Dictionary<string, ClassAssociation>();
                        if (!node.DefaultClassBindings.ContainsKey(emrel.MyMetaObject._ClassName))
                            node.DefaultClassBindings.Add(emrel.MyMetaObject._ClassName, allowedAssociations[0]);
                        emrel.MyAssociation = allowedAssociations[0];
                        node.ObjectRelationships.Add(emrel);
                    }

                    #endregion
                }
            }
        }

        public override void CopyObjectDelayed(GoCopyDictionary env, GoObject newobj)
        {
            SubgraphNode node = newobj as SubgraphNode;
            if (node.MetaObject != null)
            {
                node.MetaObject.Changed -= FireMetaObjectChanged;
                node.MetaObject = null;
            }

            base.CopyObjectDelayed(env, node);

            List<GoObject> removeThese = new List<GoObject>();
            foreach (DictionaryEntry o in env)
            {
                if (o.Value.GetType().ToString().Contains("NumberingText"))
                {
                    removeThese.Add(o.Value as GoObject);
                }
            }
            foreach (GoObject o in removeThese)
            {
                o.Remove();
                env.Remove(o);
                env.Delayeds.Remove(o);
            }
            removeThese = null;
            if (bindingInfo != null)
                node.BindingInfo = BindingInfo.Copy();

            //node.Bounds = this.Bounds;

            if (MetaObject == null)
            {
                CreateMetaObject(this, null);
            }

            if (CopyAsShadow)
            {
                node.MetaObject = MetaObject;
                node.CopyAsShadow = true;
                CopyAsShadow = false;
            }
            else
            {
                node.CreateMetaObject(null, EventArgs.Empty);
                if (node.MetaObject != null)
                    MetaObject.CopyPropertiesToObject(node.MetaObject);
            }

            node.Hooked = false;
            node.HookupEvents();
            //node.Name = Name.Substring(0, Name.Length);

            Hooked = false;
            HookupEvents();
        }

        public RectangleF BoundsBeforeLoad;

        //use this in order to check for position problems
        public override RectangleF Bounds
        {
            get
            {
                return base.Bounds;
            }
            set
            {
                base.Bounds = value;
            }
        }

        public bool SkipRelationShipRemove;
        public override void Remove(GoObject obj)
        {
            //Undo stack
            if (!SkipRelationShipRemove)
                if (obj is IMetaNode)
                {
                    RemoveOnlyRelationship(obj as IMetaNode);
                }
            base.Remove(obj);
        }
        public override void Add(GoObject obj)
        {
            if (obj is MappingCell)// || obj is ResizableComment)
                return;
            base.Add(obj);
            Expand();
        }

        #region Fields (2)

        [NonSerialized]
        public EventHandler _contentsChanged;
        [NonSerialized]
        private bool requiresAttention;

        #endregion Fields

        #region Properties (3)

        public EventHandler ContentsChanged
        {
            get { return _contentsChanged; }
            set { _contentsChanged = value; }
        }

        public virtual bool RequiresAttention
        {
            get { return requiresAttention; }
            set
            {
                requiresAttention = value;
                if (value)
                {
                    BorderPen = new Pen(Color.Red, 1);
                }
                else
                {
                    BorderPen = null;
                }
            }
        }

        #endregion Properties

        #region Methods (6)

        // Public Methods (3) 

        public void RemoveByRelationship(EmbeddedRelationship rel)
        {
            List<GoObject> itemsToRemove = new List<GoObject>();
            List<string> classesToRemove = new List<string>();
            foreach (GoObject o in this)
            {
                if (o is IMetaNode)
                {
                    IMetaNode imn = o as IMetaNode;
                    if (imn.MetaObject == rel.MyMetaObject)
                    {
                        itemsToRemove.Add(o);
                        classesToRemove.Add(rel.MyMetaObject._ClassName);
                        ObjectRelationships.Remove(rel);
                    }
                }
            }

            for (int i = 0; i < itemsToRemove.Count; i++)
            {
                GoObject o = itemsToRemove[i];
                //sgnode.Remove(kvp.Key);
                ////    new PointF(sgnode.Bounds.Width + kvp.Key.Position.X,kvp.Key.Position.Y);
                //MyView.Document.Add(kvp.Key);

                Remove(o);
                o.Position = new PointF(Bounds.X + Bounds.Width + (o.Position.X - Bounds.X), o.Position.Y);
                if (View != null)
                {
                    View.Document.Add(o);
                }
            }

            for (int i = 0; i < classesToRemove.Count; i++)
            {
                string classToRemove = classesToRemove[i];
                bool remove = true;
                foreach (GoObject o in this)
                {
                    if (o is IMetaNode)
                    {
                        IMetaNode imn = o as IMetaNode;
                        if (imn.MetaObject._ClassName == classToRemove)
                        {
                            remove = false;
                        }
                    }
                }

                List<KeyValuePair<string, ClassAssociation>> keysToRemove =
                    new List<KeyValuePair<string, ClassAssociation>>();
                if (remove)
                {
                    foreach (KeyValuePair<string, ClassAssociation> kvp in DefaultClassBindings)
                    {
                        if (kvp.Key == classToRemove)
                        {
                            keysToRemove.Add(kvp);
                        }
                    }
                    for (int X = 0; X < keysToRemove.Count; X++)
                    {
                        DefaultClassBindings.Remove(keysToRemove[X].Key);
                    }
                }
            }
        }

        //23 January 2013
        //When remove from ilinkedcontainer remove relationship as well
        public void RemoveOnlyRelationship(IMetaNode node)
        {
            List<EmbeddedRelationship> removeableRelations = new List<EmbeddedRelationship>();
            if (ObjectRelationships != null)
                foreach (EmbeddedRelationship emRel in ObjectRelationships)
                {
                    if (emRel.MyMetaObject == node.MetaObject)
                    {
                        removeableRelations.Add(emRel);
                    }
                }
            foreach (EmbeddedRelationship emRel in removeableRelations)
                ObjectRelationships.Remove(emRel);
        }

        // Protected Methods (2) 

        protected override void CollapseChild(GoObject child, RectangleF sgrect)
        {
            UpdateLinkVisibility(child, false);
            base.CollapseChild(child, sgrect);
        }

        protected override void ExpandChild(GoObject child, PointF hpos)
        {
            UpdateLinkVisibility(child, true);
            base.ExpandChild(child, hpos);
        }

        // Private Methods (1) 

        private void UpdateLinkVisibility(GoObject child, bool visible)
        {
            if (child is GoNode)
            {
                // Hide = false
                // Show = true
                GoNode node = child as GoNode;
                GoNodeLinkEnumerator linkEnum = node.Links.GetEnumerator();
                while (linkEnum.MoveNext())
                {
                    if (linkEnum.Current is GoLink)
                    {
                        GoLink lnk = linkEnum.Current as GoLink;
                        lnk.Visible = visible;
                        //22 January 2013 - sg link load correctly ie : they dont point to the label
                        lnk.CalculateRoute();
                    }

                    if (linkEnum.Current is QLink)
                    {
                        QLink lnk = linkEnum.Current as QLink;
                        lnk.Visible = visible;
                        //22 January 2013 - sg link load correctly ie : they dont point to the label
                        lnk.CalculateRoute();
                    }
                }
            }
        }

        #endregion Methods

        //private bool expanded;
        //public bool Expanded { get { return expanded; } set { expanded = value; } }

        public SubgraphNode()
        {
            Label = new ExpandableTextBoxLabel();

            //DebugLayout("SGN Constructor()");
            ObjectRelationships = new List<EmbeddedRelationship>();
            DefaultClassBindings = new Dictionary<string, ClassAssociation>();
            //BorderPen = new Pen(Brushes.Gray);
            BackgroundColor = Color.Lavender;
            Opacity = 100;
            Label.DragsNode = true;
            Shadowed = true;
            BorderPen = Pens.Blue;
            // wide margin and large corners
            Corner = CollapsedCorner = new SizeF(20, 20);
            TopLeftMargin = CollapsedTopLeftMargin = new SizeF(20, 20);
            BottomRightMargin = CollapsedBottomRightMargin = new SizeF(20, 20);
            PickableBackground = true;
            Label.Editable = false;
            //Label.AutoRescales = true;
            Label.AutoResizes = true;
            LabelSpot = 48;
            CollapsedLabelSpot = 48;

            //Handle.Position = new PointF(SelectionObject.Left + 5, SelectionObject.Top + 5);

            //Expand();
            CollapsedCorner = Corner;
            //ComputeBounds();
            //ComputeBorder();
            Label.AddObserver(this);
        }

        protected override void OnObservedChanged(GoObject observed, int subhint, int oldI, object oldVal, RectangleF oldRect, int newI, object newVal, RectangleF newRect)
        {
            base.OnObservedChanged(observed, subhint, oldI, oldVal, oldRect, newI, newVal, newRect);
            if (observed == this.Label)
            {
                if (subhint == 1501 && MetaObject != null)
                {
                    try
                    {
                        MetaObject.Set("Name", Label.Text);
                    }
                    catch
                    {
                        Core.Log.WriteLog("SubgraphNode.OnObservedChanged : Cannot bind text to metaobject name for class " + MetaObject.Class + " for subgraph");
                    }
                }
            }
        }

        #region Positioning and Layout

        // create a boxport covering the whole subgraph
        protected override GoPort CreatePort()
        {
            return new CustomSubGraphPort();
        }

        // If you want to display an image when collapsed,
        // add this override (or set CollapsedObject)

        //    protected override GoObject CreateCollapsedObject() {
        //      GoImage img = new GoImage();
        //      img.Selectable = false;
        //      img.Visible = false;
        //      img.Printable = false;
        //      img.Name = ...
        //      img.Size = new SizeF(img.Image.Width, img.Image.Height);
        //      this.CollapsedSize = img.Size;
        //      return img;
        //    }

        // CustomSubGraphPort has same Bounds as whole subgraph

        public override void LayoutPort()
        {
            GoPort p = Port;
            if (p != null && p.CanView())
            {
                RectangleF r = ComputeBorder();
                p.Bounds = r;
                p.Bounds.Inflate(3, 3);
                //p.PortObject = this;
            }
        }

        protected override bool ComputeInsideMarginsSkip(GoObject child)
        {
            if (child is IndicatorLabel)
                return true;

            return base.ComputeInsideMarginsSkip(child);
        }
        public override RectangleF ComputeInsideMargins(GoObject ignore)
        {
            if (Handle == null || Label == null)
                return base.ComputeInsideMargins(ignore);

            foreach (GoObject o in this)
                if (o is IndicatorLabel && Handle != null)
                    o.Location = new PointF(Handle.Location.X, Handle.Location.Y + 10);

            RectangleF handleAndLabelBounds = RectangleF.Union(Handle.Bounds, Label.Bounds);
            if (!IsExpanded)
            {
                return RectangleF.Union(new RectangleF(Handle.Position, CollapsedTopLeftMargin), handleAndLabelBounds);
            }
            else
            {
                foreach (GoObject o in this)
                    if (o is IMetaNode)
                        return RectangleF.Union(base.ComputeInsideMargins(ignore), RectangleF.Union(new RectangleF(Handle.Position, CollapsedTopLeftMargin), handleAndLabelBounds));

                handleAndLabelBounds.Width += 50; //minimum width increased by 50 when there are no nodes in expanded form
                return RectangleF.Union(base.ComputeInsideMargins(ignore), RectangleF.Union(new RectangleF(Handle.Position, CollapsedTopLeftMargin), handleAndLabelBounds));
            }

            return base.ComputeInsideMargins(ignore);
        }

        // If you always want to position the Handle in the very top-left
        // corner of the subgraph, ignoring the Margin, override LayoutHandle
        // and call it from DoResize as follows:
        public override void LayoutLabel()
        {
            //if (Handle != null && Handle.CanView() && Label != null)
            //{
            //Label.Position = Handle.Position;
            Label.Position = new PointF(Handle.Position.X + 15, Handle.Position.Y);
            //Label.Alignment = (int)GoObject.Left;
            Label.AutoResizes = true;
            Label.WrappingWidth = Width - 70;
            //Label.Bordered = true;
            //}
        }

        #endregion

        #region Subgraph Logic

        public const int ChangedCollapsedSize = LastChangedHint + 2789;

        public const int AddedObjects = 123456;
        private SizeF myCollapsedSize = new SizeF(60, 30); // for text Label, not for a CollapsedObject that is an Image

        public SizeF CollapsedSize
        {
            get { return myCollapsedSize; }
            set
            {
                if (myCollapsedSize != value)
                {
                    myCollapsedSize = value;
                    Changed(ChangedCollapsedSize, 0, null, MakeRect(myCollapsedSize), 0, null, MakeRect(value));
                    // doesn't actually change size if already collapsed
                }
            }
        }

        public override bool OnSelectionDropped(GoView view)
        {
            if (!IsExpanded && !IsInDocument)
                Expand();
            return base.OnSelectionDropped(view);
        }

        /*
     public override RectangleF ComputeResize(RectangleF origRect, PointF newPoint, int handle, SizeF min, SizeF max, bool reshape)
        {
            //DebugLayout("ComputeResize");
            RectangleF insides = this.ComputeInsideMargins(Label);
            RectangleF minSize = ComputeBounds();

            return base.ComputeResize(origRect, newPoint, handle, minSize.Size, max, reshape);
        }*/
        /*
        protected override void OnBoundsChanged(RectangleF old)
        {

                base.OnBoundsChanged(old);
                LayoutHandle();
                LayoutLabel();
        
            
        }
        */
        /*public override void DoResize(GoView view, RectangleF origRect, PointF newPoint,
                                      int whichHandle, GoInputState evttype, SizeF min, SizeF max)
        {
            RectangleF minSize = ComputeBounds();

            base.DoResize(view, origRect, newPoint, whichHandle, evttype, minSize.Size, max);
            LayoutPort();
                  if (this.IsExpanded) {
                    LayoutHandle();
                  }
        }*/
        /*
        protected override void OnChildBoundsChanged(GoObject child, RectangleF old)
        {
            
            base.OnChildBoundsChanged(child, old);
            if (child!= Handle && child!=Label && child!=Port)
            {
                LayoutPort();
                if (this.IsExpanded)
                {
                    LayoutHandle();
                }
            }
          
        }*/

        protected override bool ComputeCollapsedSizeSkip(GoObject child)
        {
            if (child != Handle && child != Label)
                return false;
            return true;
        }

        // making collapsed nodes a fixed size
        public override SizeF ComputeCollapsedSize(bool visible)
        {
            base.ComputeCollapsedSize(visible);
            if (CollapsedSize.Width > 10 && CollapsedSize.Height > 10)
            {
                return CollapsedSize;
            }
            return new SizeF(10, 10);
        }

        // a Width or Height of ten or less results in the standard behavior:
        // the collapsed size is the smallest rectangle to hold all child nodes

        public override void ChangeValue(GoChangedEventArgs e, bool undo)
        {
            switch (e.SubHint)
            {
                case AddedObjects:
                    if (undo)
                    {
                        List<GoObject> objects = e.NewValue as List<GoObject>;
                        List<EmbeddedRelationship> relationshipsToRemove = new List<EmbeddedRelationship>();
                        if (objects != null)
                        {
                            for (int i = 0; i < objects.Count; i++)
                            {
                                if (objects[i] is IMetaNode)
                                {
                                    IMetaNode imNode = objects[i] as IMetaNode;

                                    foreach (EmbeddedRelationship er in ObjectRelationships)
                                    {
                                        if (er.MyMetaObject == imNode.MetaObject)
                                        {
                                            relationshipsToRemove.Add(er);
                                        }
                                    }
                                }
                                //this.Remove(objects[i]);
                                //  this.Document.Add(objects[i]);
                                if (objects[i] is GoLink)
                                {
                                    GoLink link = objects[i] as GoLink;
                                    link.UpdateRoute();
                                }
                            }
                            for (int i = 0; i < relationshipsToRemove.Count; i++)
                            {
                                ObjectRelationships.Remove(relationshipsToRemove[i]);
                            }
                        }
                        GoText lbl = e.OldValue as GoText;
                        Label.Text = lbl.Text;
                        Bounds = e.OldRect;
                    }
                    return;
                case ChangedCollapsedSize:
                    CollapsedSize = e.GetSize(undo);
                    return;
                default:
                    base.ChangeValue(e, undo);
                    return;
            }
        }

        #region Hover Highlighting

        [NonSerialized]
        public Pen HighlightPen = new Pen(Color.Blue);
        [NonSerialized]
        public Pen StandardPen = new Pen(Color.Gray);

        public void SetHighlight(bool show)
        {
            BorderPen = (show) ? HighlightPen : StandardPen;
        }

        public override void Paint(Graphics g, GoView view)
        {
            if (view.IsPrinting)
                BorderPen = StandardPen;
            if (RequiresAttention)
            {
                BorderPen = new Pen(Color.Red, 5);
            }
            else
                BorderPen = null;
            base.Paint(g, view);
        }

        public override bool OnEnterLeave(GoObject from, GoObject to, GoView view)
        {
            if (view.Selection.Count > 0)
            {
                //SetHighlight(to == this);
            }
            else
                SetHighlight(false);
            return base.OnEnterLeave(from, to, view);
        }

        #endregion

        #endregion

        #region Standard implementation of IMetaNode

        private MetaBase metaObject;
        public MetaBase MetaObject
        {
            get { return metaObject; }
            set { metaObject = value; }
        }

        [NonSerialized]
        bool Hooked = false;
        public void HookupEvents()
        {
            if (MetaObject != null)
            {
                try
                {
                    MetaObject.Changed -= FireMetaObjectChanged;
                }
                catch
                {
                }
                MetaObject.Changed += FireMetaObjectChanged;
            }

            Hooked = true;
        }

        public virtual void BindToMetaObjectProperties()
        {
            if (MetaObject != null)
            {
                string t = MetaObject.ToString();
                if (t == null)
                    return;

                //dont change the label if it is already what it should be
                if (t == Text)
                    return;

                string moString = MetaObject.ToString();
                Label.Text = moString;
            }
            else
                Label.Text = "New " + BindingInfo.BindingClass;
        }
        public void BindMetaObjectImage()
        {
        }
        public void CreateMetaObject(object sender, EventArgs args)
        {
            if (HasBindingInfo)
            {
                if (BindingInfo.BindingClass != null)
                {
                    MetaObject = Loader.CreateInstance(BindingInfo.BindingClass);
                    //7 January 2013
                    //FireMetaObjectChanged(null, EventArgs.Empty);
                }
            }
        }

        public void LoadMetaObject(int ID, string Machine)
        {
            if (HasBindingInfo)
            {
                if (BindingInfo.BindingClass != null)
                {
                    MetaObject = Loader.GetByID(bindingInfo.BindingClass, ID, Machine);
                    BindToMetaObjectProperties();
                    //FireMetaObjectChanged(null, EventArgs.Empty);
                }
            }
        }

        public b.TList<b.ObjectAssociation> SaveToDatabase(object sender, EventArgs e)
        {
            MetaObject.Save(Guid.NewGuid());
            RequiresAttention = false;
            return null;
            try
            {
                MetaObject.Save(Guid.NewGuid());
            }
            catch (Exception x)
            {
            }
            return null;
        }

        public void FireMetaObjectChanged(object sender, EventArgs e)
        {
            if (ContentsChanged != null)
                ContentsChanged(sender, e);
            BindToMetaObjectProperties();
            if (Core.Variables.Instance.SaveOnCreate)
            {
                SaveToDatabase(sender, e);
            }
        }

        public virtual void OnContentsChanged(object sender, EventArgs e)
        {
            if (ContentsChanged != null)
            {
                ContentsChanged(this, e);
            }
        }

        public void SaveToDatabase(object sender, EventArgs e, string Provider)
        {
            MetaObject.SaveToRepository(Guid.NewGuid(), Provider);

            //List<GoObject> remove = new List<GoObject>();
            //foreach (GoObject o in this)
            //    if (o is IndicatorLabel)
            //        if ((o as IndicatorLabel).TextColor == Color.Red)
            //        {
            //            remove.Add(o);
            //        }
            //foreach (GoObject o in remove)
            //    o.Remove();
            RequiresAttention = false;
        }

        #endregion

        #region Relationship Definitions

        private Dictionary<string, ClassAssociation> defaultClassBindings;
        private List<EmbeddedRelationship> objectRelationships;

        public List<EmbeddedRelationship> ObjectRelationships
        {
            get { return objectRelationships; }
            set { objectRelationships = value; }
        }

        public Dictionary<string, ClassAssociation> DefaultClassBindings
        {
            get { return defaultClassBindings; }
            set { defaultClassBindings = value; }
        }

        #endregion

        #region Binding Info

        private BindingInfo bindingInfo;

        public bool HasBindingInfo
        {
            get
            {
                bool retval = false;
                if (BindingInfo != null)
                {
                    retval = (BindingInfo.BindingClass.Length > 0);
                }
                return retval;
            }
        }

        public BindingInfo BindingInfo
        {
            get { return bindingInfo; }
            set { bindingInfo = value; }
        }

        #endregion

        #region IBehaviour Implementation

        #endregion

        #region Internal Classes

        #region Nested type: CustomSubGraphPort

        [Serializable]
        public class CustomSubGraphPort : GoBoxPort
        {
            #region RelativePosition enum

            public enum RelativePosition
            {
                LeftOfPort,
                RightOfPort,
                AbovePort,
                BelowPort
            }

            #endregion

            public CustomSubGraphPort()
            {
                IsValidSelfNode = false;
                Style = GoPortStyle.None;
                LinkPointsSpread = false;
                IsValidDuplicateLinks = false;
            }

            // links within the subgraph connect to the CustomSubGraphPort/GoBoxPort from the inside
            public override float GetFromLinkDir(IGoLink link)
            {
                float result = base.GetFromLinkDir(link);
                if (link.ToPort != null &&
                    link.ToPort.GoObject != null &&
                    link.ToPort.GoObject.IsChildOf(Parent))
                {
                    result += 180;
                    if (result > 360)
                        result -= 360;
                }
                //// Console.WriteLine("GetFromLinkDir: " + result.ToString());
                return result;
            }

            // links within the subgraph connect to the CustomSubGraphPort/GoBoxPort from the inside
            public override float GetToLinkDir(IGoLink link)
            {
                float result = base.GetToLinkDir(link);
                if (link.FromPort != null &&
                    link.FromPort.GoObject != null &&
                    link.FromPort.GoObject.IsChildOf(Parent))
                {
                    result += 180;
                    if (result > 360)
                        result -= 360;
                }
                //// Console.WriteLine("GetToLinkDir: " + result.ToString());
                return result;
            }

            // sorting links by angle shouldn't be confused by whether it's
            // inside or outside of the CustomSubGraph
            public override float GetDirection(IGoLink link)
            {
                if (link == null) return 0;
                if (link.FromPort == this)
                {
                    return base.GetFromLinkDir(link);
                }
                return base.GetToLinkDir(link);
            }

            // assume this kind of port is "hollow", as specified by the half the parent's margin
            public override bool ContainsPoint(PointF p)
            {
                RectangleF rect = Bounds;
                if (!rect.Contains(p))
                    return false;
                GoSubGraph sg = Parent as GoSubGraph;
                if (sg != null)
                {
                    SizeF tlmargin = sg.TopLeftMargin;
                    SizeF brmargin = sg.BottomRightMargin;
                    rect.X += Math.Max(1, tlmargin.Width / 2);
                    rect.Y += Math.Max(1, tlmargin.Height / 2);
                    rect.Width -= Math.Max(1, tlmargin.Width / 2 + brmargin.Width / 2);
                    rect.Height -= Math.Max(1, tlmargin.Height / 2 + brmargin.Height / 2);
                    return !rect.Contains(p);
                }
                return true;
            }
        }

        #endregion

        /// <summary>
        /// Internal class to represent an object and the subgraph's relationship to that object
        /// </summary>
        /// <summary>
        /// Internal class to store Default Association info for each embedded class
        /// </summary>
        [Serializable]
        public class DefaultClassBinding
        {
            private string className;

            private ClassAssociation myAssociation;

            public string ClassName
            {
                get { return className; }
                set { className = value; }
            }

            public ClassAssociation MyAssociation
            {
                get { return myAssociation; }
                set { myAssociation = value; }
            }
        }
        #endregion

        #region IGoDragSnapper Members

        bool IGoDragSnapper.CanSnapPoint(PointF p, GoObject obj, GoView view)
        {
            return CanView() && obj.IsChildOf(this);// && !(obj is LimitingSubGraphMarker);
        }

        bool IGoDragSnapper.SnapOpaque
        {
            get { return true; }
        }

        PointF IGoDragSnapper.SnapPoint(PointF p, GoObject obj, GoView view)
        {
            // normalize the object's Bounds as if the Location were always the Position
            PointF loc = obj.Location;
            RectangleF b = obj.Bounds;
            b.X += (p.X - loc.X);
            b.Y += (p.Y - loc.Y);

            // position the object so that it fits inside the margins
            RectangleF r = ComputeInsideMargins(this.Label);
            //if (b.Right > r.Right)
            //    p.X -= (b.Right - r.Right);
            if (b.Left < r.Left)
                p.X += (r.Left - b.Left);
            //if (b.Bottom > r.Bottom)
            //    p.Y -= (b.Bottom - r.Bottom);
            if (b.Top < (Label.Bottom + 5))
                p.Y += ((Label.Bottom + 5) - b.Top);

            return p;
        }

        #endregion
    }

    [Serializable]
    public class EmbeddedRelationship
    {
        private ClassAssociation myAssociation;
        private MetaBase myMetaObject;

        public ClassAssociation MyAssociation
        {
            get { return myAssociation; }
            set { myAssociation = value; }
        }

        public MetaBase MyMetaObject
        {
            get { return myMetaObject; }
            set { myMetaObject = value; }
        }

        public bool FromDatabase = false;
    }
}