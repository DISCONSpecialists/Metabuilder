using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Graphing.Controllers;
using MetaBuilder.Graphing.Shapes.General;
using MetaBuilder.Graphing.Shapes.Primitives;
using MetaBuilder.Meta;
using Northwoods.Go;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.Graphing.Shapes.Nodes.Containers
{
    [Serializable]
    public class SnappingGrid : GoGrid
    {
        public SnappingGrid()
        {
            OriginRelative = true;
            ResizesRealtime = true;
            Selectable = false;
            SnapOpaque = true;
        }

        public override void DoResize(GoView view, RectangleF origRect, PointF newPoint, int whichHandle, GoInputState evttype, SizeF min, SizeF max)
        {
            base.DoResize(view, origRect, newPoint, whichHandle, evttype, new SizeF(10, 10), max);
            PointF p = FindNearestGridPoint(newPoint, null);
            base.DoResize(view, origRect, p, whichHandle, evttype, new SizeF(10, 10), max);
            if (whichHandle != BottomRight)
                DoResize(view, Bounds, new PointF(Position.X + Width, Position.Y + Height), BottomRight, evttype,
                         new SizeF(10, 10), max);
        }
    }

    [Serializable]
    public class ValueChain : GoNode, IGoDragSnapper, IMetaNode, ILinkedContainer
    {
        #region Properties

        [NonSerialized]
        private MetaBase copiedFrom;
        public MetaBase CopiedFrom
        {
            get { return copiedFrom; }
            set { copiedFrom = value; }
        }
        private bool locked;
        public bool Locked
        {
            get { return locked; }
            set { locked = value; }
        }

        #endregion

        #region IGoDragSnapper Implementation

        public PointF SnapPoint(PointF p, GoObject obj, GoView view)
        {
            PointF loc;
            RectangleF b;
            RectangleF r;

            if (Locked)
            {
                loc = obj.Location;
                b = obj.Bounds;
                b.X += (p.X - loc.X);
                b.Y += (p.Y - loc.Y);

                if (Background != null)
                {
                    r = new RectangleF(Background.RealArrowStart.X, Background.Top,
                                       Background.Width - Background.Height / 2, Background.Height);
                    if (b.Right > r.Right)
                        p.X -= (b.Right - r.Right);
                    if (b.Left < r.Left)
                        p.X += (r.Left - b.Left);
                    if (b.Bottom > r.Bottom)
                        p.Y -= (b.Bottom - r.Bottom);
                    if (b.Top < r.Top)
                        p.Y += (r.Top - b.Top);
                }
                return p;
            }

            return p;
        }

        public bool CanSnapPoint(PointF p, GoObject obj, GoView view)
        {
            // surprisingly, don't care about the point P;
            // just if it's a child of this subgraph
            return CanView() && obj.IsChildOf(this) && (!(obj is SnappingGrid));
        }

        public bool SnapOpaque
        {
            get { return true; }
        }

        #endregion

        public ValueChain(bool createPorts)
        {
            Shadowed = false;
            AddGrid();

            Grid.Visible = true;
            Grid.Style = GoViewGridStyle.None;
            Grid.Pen = null; // new Pen(Brushes.Gray);
            Grid.CellSize = new SizeF(10, 10);

            Grid.LineColor = Color.Gray;
            Grid.LineDashStyle = DashStyle.Dot;
            Grid.MajorLineColor = Color.Gray;
            Grid.MajorLineDashStyle = DashStyle.Dot;
            //Grid.SnapDrag = GoViewSnapStyle.Jump;
            //Grid.SnapDragWhole = false;
            Grid.SnapOpaque = true;
            Grid.SnapResize = GoViewSnapStyle.Jump;
            //Grid.DragsNode = true;
            AddBackground();

            AddLabel();
            Label.Text = "Value Chain Step";
            Label.DragsNode = true;

            AddLockButton();

            if (createPorts)
                AddPorts();

            //Grid.AddObserver(this);
            ResizeBackgroundToGrid();
            RepositionPorts();
            //this.DragsNode = false;
            LayoutLabel();
            DragsNode = false;

            Locked = true;

            //MetaObject = Loader.CreateInstance("Function");
            HookupEvents();
            ObjectRelationships = new List<EmbeddedRelationship>();
            DefaultClassBindings = new Dictionary<string, ClassAssociation>();

            //base.Label.Visible = false;
        }

        private void btn_Action(object sender, GoInputEventArgs e)
        {
            Locked = !Locked;
            Button.Text = (Locked) ? "Ï" : "Ð";
            RepositionButton();
        }

        #region Positioning

        public void Reposition()
        {
            ResizeBackgroundToGrid();
            RepositionPorts();
            RepositionButton();
            LayoutLabel();
        }

        public void RepositionPorts()
        {
            NonPrintingQuickPort prtRight = FindChild("rightPort") as NonPrintingQuickPort;
            NonPrintingQuickPort prtLeft = FindChild("leftPort") as NonPrintingQuickPort;
            NonPrintingQuickPort prtTop = FindChild("topPort") as NonPrintingQuickPort;
            NonPrintingQuickPort prtBottom = FindChild("bottomPort") as NonPrintingQuickPort;

            if (prtRight != null && prtLeft != null && prtTop != null && prtBottom != null)
            {
                if (Background != null)
                {
                    prtLeft.Position = new PointF(Background.RealArrowStart.X - prtLeft.Width / 2f,
                                                  Background.RealArrowStart.Y - prtLeft.Height / 2);
                    prtRight.Position = new PointF(Background.RealArrowTip.X - prtRight.Width / 2f,
                                                   Background.RealArrowTip.Y - prtLeft.Height / 2);
                    prtTop.Position = new PointF(Background.Position.X + Background.Width / 2f, Background.Top);
                    prtBottom.Position = new PointF(Background.Position.X + Background.Width / 2f,
                                                    Background.Bottom - prtBottom.Height);
                    //Console.WriteLine(prtBottom.Shadowed);
                }
            }
        }

        private void RepositionButton()
        {
            Button.Position = new PointF(Grid.Position.X + Grid.Width - Button.Width, Grid.Position.Y);
        }

        public void ResizeBackgroundToGrid()
        {
            VCShape gradPoly = Background;
            if (gradPoly != null)
            {
                gradPoly.DrawStep();
                LayoutLabel();
                //  this.Bounds = Grid.Bounds;
            }
            if (Button != null)
                RepositionButton();
            //this.Bounds = Grid.Bounds;
        }

        public void LayoutLabel()
        {
            if (Background != null && Label != null)
            {
                Label.Position = new PointF(Background.RealArrowStart.X, Grid.Position.Y + 2);
                PointF topRightCorner = new PointF(Grid.Position.X + Grid.Width - Grid.Height / 3.5f, Grid.Position.Y);

                Label.Width = topRightCorner.X - Background.RealArrowStart.X + 4;
                Label.Height = Background.Height - (Background.Height / 4);
                Label.AutoResizes = false;
                Label.Wrapping = true;
                Label.WrappingWidth = Label.Width - 5;// Background.Width; //-2
                Label.Multiline = true;
                Label.Alignment = MiddleCenter;

                //Label.Editable = false;
            }
        }

        #endregion

        #region IMetaNode

        private bool parentIsILinkedContainer;
        public bool ParentIsILinkedContainer
        {
            get { return parentIsILinkedContainer; }
            set { parentIsILinkedContainer = value; }
        }
        [NonSerialized]
        public EventHandler _contentsChanged;
        private bool actionHooked;
        private bool copyAsShadow;
        private MetaBase metaObject;
        private string name;
        private bool requiresAttention;

        public string Name
        {
            get
            {
                if (name != null)
                {
                    if (name != string.Empty)
                        return name;
                }
                name = "VC" + Guid.NewGuid().ToString().Substring(0, 5);
                return name;
            }
            set { name = value; }
        }

        public bool CopyAsShadow
        {
            get { return copyAsShadow; }
            set { copyAsShadow = value; }
        }

        public MetaBase MetaObject
        {
            get { return metaObject; }
            set
            {
                metaObject = value;
                if (MetaObject == null)
                    return;
                BindingInfo = new BindingInfo();
                //Label.Name = "lbl";
                BindingInfo.BindingClass = MetaObject.Class;
                BindingInfo.Bindings.Add("lbl", "Name");
            }
        }

        public virtual void OnContentsChanged(object sender, EventArgs e)
        {
            if (ContentsChanged != null)
            {
                ContentsChanged(this, e);
            }
        }

        public virtual bool RequiresAttention
        {
            get { return requiresAttention; }
            set
            {
                requiresAttention = value;
                //if (value)
                //{
                //Grid.Pen = new Pen(Color.Red);
                Color c = Color.Red;
                Grid.Brush = new SolidBrush(Color.FromArgb(125, c.R, c.G, c.B));
                Grid.Printable = false;
                Grid.Visible = value;
                //}
                //else
                //{
                //    Grid.Pen = null;
                //    Grid.Visible = value;
                //}
            }
        }

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

            if (Label != null)
            {
                List<GoObject> removeThese = new List<GoObject>();
                System.Collections.IEnumerator enumerator = Label.Observers.GetEnumerator();
                while (enumerator.MoveNext())// (object o in Label.Observers.GetEnumerator())
                    if (enumerator.Current is ValueChain)
                        removeThese.Add(enumerator.Current as GoObject);

                foreach (GoObject remove in removeThese)
                    Label.RemoveObserver(remove);
                Label.AddObserver(this);
            }
            //else
            //{
            //    //when can the label ever be null?
            //    this.ToString();
            //}

            if (!actionHooked)
            {
                Button.Action += btn_Action;
                actionHooked = true;
                Button.ActionEnabled = true;
                Button.Selectable = false;
                Button.Label.FontSize = 8f;
                Button.Label.Alignment = GoObject.TopCenter;
            }
        }

        public virtual void BindToMetaObjectProperties()
        {
            if (MetaObject != null)
            {
                string moString = MetaObject.ToString();
                Label.Text = moString;
            }
            else
                Label.Text = "Value Chain Step";
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
            return null;
        }

        public void FireMetaObjectChanged(object sender, EventArgs e)
        {
            if (ContentsChanged != null)
                ContentsChanged(sender, e);
            BindToMetaObjectProperties();
            if (Core.Variables.Instance.SaveOnCreate)
                SaveToDatabase(sender, e);
        }

        public EventHandler ContentsChanged
        {
            get { return _contentsChanged; }
            set { _contentsChanged = value; }
        }

        public void RemovePorts()
        {
        }

        public void LoadDefaultClassBindings()
        {
            foreach (GoObject o in this)
            {
                if (o is IMetaNode)
                {
                    AddDefaultAssociationInfoForChildNode(o);
                }
            }
        }

        //public void LabelTextChanged(BoundLabel label, string OldText, string NewText)
        public void LabelTextChanged(BoundLabel label, string OldText, string NewText)
        {
            if (MetaObject != null)
            {
                //if (bindingInfo != null)
                //    foreach (KeyValuePair<string, string> kvpair in bindingInfo.Bindings)
                //    {
                //        if (kvpair.Key == label.Name)
                //        {
                try
                {
                    MetaObject.Set("Name", NewText);
                }
                catch
                {
                }
                //}
                //}
            }
        }
        public override void LayoutChildren(GoObject childchanged)
        {
            if (Label != null)
            {
                if (this.Label.Width < this.Background.Width - 50)
                {
                    this.Label.Width = this.Background.Width - 50;
                }
                Label.Height = Background.Height - (Background.Height / 4);
            }
            base.LayoutChildren(childchanged);
        }
        protected override void OnObservedChanged(GoObject observed, int subhint, int oldI, object oldVal, RectangleF oldRect, int newI, object newVal, RectangleF newRect)
        {
            switch (subhint)
            {
                case 1501: // Probably Boundlabel's Text that changed
                    if (observed is BoundLabel)
                    {
                        //GoCollectionEnumerator obsEnum = observed.Observers.GetEnumerator();
                        //while (obsEnum.MoveNext())
                        //{
                        // Console.WriteLine(obsEnum.Current.ToString());
                        //}
                        if (observed.ParentNode != this)
                        {
                            observed.RemoveObserver(this);
                            return;
                        }
                        BoundLabel culprit = observed as BoundLabel;
                        LabelTextChanged(culprit, (string)oldVal, (string)newVal);
                        LayoutLabel();
                    }
                    break;
            }
            //Manager.OnObservedChanged(observed, subhint, oldI, oldVal, oldRect, newI, newVal, newRect, this);
            base.OnObservedChanged(observed, subhint, oldI, oldVal, oldRect, newI, newVal, newRect);
        }

        public void SaveToDatabase(object sender, EventArgs e, string Provider)
        {
            MetaObject.SaveToRepository(Guid.NewGuid(), Provider);
        }

        public override GoObject CopyObject(GoCopyDictionary env)
        {
            env.Delayeds.Add(this);
            env.Delayeds.Add(Grid);
            GoObject retval = base.CopyObject(env);
            ValueChain node = retval as ValueChain;
            node.actionHooked = false;
            node.Locked = true;
            node.Button.Label.Text = "Ï";
            node.HookupEvents();
            node.ObjectRelationships = new List<EmbeddedRelationship>();
            if (CopyAsShadow)
            {
                node.MetaObject = MetaObject;

                node.CopyAsShadow = true;
                return node;
            }
            return node;
        }

        public override void CopyObjectDelayed(GoCopyDictionary env, GoObject newobj)
        {
            ValueChain node = newobj as ValueChain;
            if (node.MetaObject != null)
            {
                node.MetaObject.Changed -= FireMetaObjectChanged;
                node.MetaObject = null;
            }

            base.CopyObjectDelayed(env, node);
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
            //node.actionHooked = false;
            node.ObjectRelationships = new List<EmbeddedRelationship>();
            node.Locked = true;
            node.Button.Label.Text = "Ï";
            node.HookupEvents();
            HookupEvents();
        }

        public GoObject CopyAsShallow()
        {
            MetaBase mo = MetaObject;
            ValueChain gnode = Copy() as ValueChain;
            gnode.MetaObject = mo;
            return gnode;
        }

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

        #endregion

        #region Add Standard Items

        private void AddGrid()
        {
            if (Grid != null)
                return;
            SnappingGrid grid = new SnappingGrid();
            grid.Width = 300;
            grid.Height = 100;
            Add(grid);
        }

        private void AddBackground()
        {
            if (Background != null)
                return;
            VCShape gradPoly = new VCShape(Grid.Width, Grid.Height);
            //TopLeft
            gradPoly.Selectable = false;
            gradPoly.DragsNode = true;
            Add(gradPoly);
            ResizeBackgroundToGrid();
        }

        private void AddLabel()
        {
            //THERE IS ALWAYS A LABEL WHOSE TYPE CANNOTE BE CHANGED!
            //if (Label != null)
            //    return;
            BoundLabel lbl = new BoundLabel();
            lbl.Selectable = false;
            Add(lbl);
        }

        public void AddLockButton()
        {
            //What if there is already a button?
            if (Button != null)
                return;
            GoButton btn = new GoButton();
            btn.Label.FamilyName = "Webdings";
            btn.Text = "Ï";
            btn.Printable = false;
            btn.DragsNode = true;
            Add(btn);
            RepositionButton();
        }

        public void AddPorts()
        {
            NonPrintingQuickPort qprtLeft = new NonPrintingQuickPort();
            NonPrintingQuickPort qprtRight = new NonPrintingQuickPort();
            NonPrintingQuickPort qprtTop = new NonPrintingQuickPort();
            NonPrintingQuickPort qprtBottom = new NonPrintingQuickPort();

            qprtRight.IncomingLinksDirection = 0;
            qprtRight.OutgoingLinksDirection = 0;
            qprtRight.PortPosition = QuickPortHelper.QuickPortLocation.Right;
            Add(qprtRight);
            AddChildName("rightPort", qprtRight);

            qprtLeft.IncomingLinksDirection = 180;
            qprtLeft.OutgoingLinksDirection = 180;
            qprtRight.PortPosition = QuickPortHelper.QuickPortLocation.Left;
            Add(qprtLeft);
            AddChildName("leftPort", qprtLeft);

            qprtTop.OutgoingLinksDirection = 270;
            qprtTop.IncomingLinksDirection = 270;
            qprtRight.PortPosition = QuickPortHelper.QuickPortLocation.Top;
            Add(qprtTop);
            AddChildName("topPort", qprtTop);

            qprtBottom.IncomingLinksDirection = 90;
            qprtBottom.OutgoingLinksDirection = 90;
            qprtRight.PortPosition = QuickPortHelper.QuickPortLocation.Bottom;
            Add(qprtBottom);
            AddChildName("bottomPort", qprtBottom);

            RepositionPorts();
        }

        #endregion

        #region Get Accessors for Often used items

        public virtual SnappingGrid Grid
        {
            get
            {
                if (Count > 0)
                    return this[0] as SnappingGrid;
                return null;
            }
            set { }
        }

        public VCShape Background
        {
            get
            {
                if (Count > 1)
                    return this[1] as VCShape;
                return null;
            }
            set { }
        }

        [Description("MetaObject PKID"),
         CategoryAttribute("Meta Properties")]
        public int PKID
        {
            get { return MetaObject.pkid; }
        }

        [DescriptionAttribute("MetaObject Machine"),
         CategoryAttribute("Meta Properties")]
        public string MachineName
        {
            get { return MetaObject.MachineName; }
        }

        [DescriptionAttribute("MetaObject Workspace"),
         CategoryAttribute("Meta Properties")]
        public string Workspace
        {
            get { return MetaObject.WorkspaceName; }
        }

        [DescriptionAttribute("The MetaObject Class"),
         CategoryAttribute("Meta Properties")]
        public string Class
        {
            get { return MetaObject._ClassName; }
        }

        public BoundLabel Label
        {
            get
            {
                if (Count > 2)
                    return this[2] as BoundLabel;
                return null;
            }
            set { this[2] = value; }
        }

        public GoButton Button
        {
            get
            {
                if (Count > 3)
                    return this[3] as GoButton;
                return null;
            }
            set { }
        }

        #endregion

        #region ILinkedContainer Implementation

        public void PerformAddCollection(GoView view, GoCollection objectCollection)
        {
            AddCollection(objectCollection, true);
        }

        public string LabelText
        {
            get { return Label.Text; }
        }

        public bool ObjectInAcceptedRegion(GoObject o)
        {
            return Grid.Bounds.Contains(o.Bounds);

            RectangleF rect = o.Bounds;
            //return (Grid.Bounds.Width + Grid.Bounds.Right > rect.Width + rect.Right) && (Grid.Bounds.Height + Grid.Bounds.Bottom > rect.Height + rect.Bottom)
            return
                //BottomRight
                ((Grid.Bounds.Location.X + Grid.Width) >= (rect.Location.X + rect.Width)) && ((Grid.Bounds.Location.Y + Grid.Height) >= (rect.Location.Y + rect.Height))
                //TopLeft
                && (Grid.Bounds.Location.X <= (rect.Location.X)) && (Grid.Bounds.Location.Y <= (rect.Location.Y));
        }

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
                Remove(itemsToRemove[i]);
                if (View != null)
                    View.Document.Add(itemsToRemove[i]);
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

        #region Relationship Definitions

        private Dictionary<string, ClassAssociation> defaultClassBindings;
        private List<EmbeddedRelationship> objectRelationships;

        public List<EmbeddedRelationship> ObjectRelationships
        {
            get
            {
                return objectRelationships;
            }
            set
            {
                objectRelationships = value;
            }
        }

        public Dictionary<string, ClassAssociation> DefaultClassBindings
        {
            get { return defaultClassBindings; }
            set { defaultClassBindings = value; }
        }

        #endregion

        private void AddDefaultAssociationInfoForChildNode(GoObject o)
        {
            if (MetaObject != null)
            {
                if (o is IMetaNode)
                {
                    IMetaNode im = o as IMetaNode;
                    if (im.MetaObject == null)
                        return;
                }
                bool alreadyAdded = false;
                MetaBase mb = (o as IMetaNode).MetaObject;
                foreach (EmbeddedRelationship emrel in ObjectRelationships)
                {
                    if (emrel.MyMetaObject == mb)
                    {
                        alreadyAdded = true;
                    }
                }
                if (!alreadyAdded)
                {
                    EmbeddedRelationship emrel = new EmbeddedRelationship();
                    emrel.MyMetaObject = mb;
                    TList<ClassAssociation> allowedAssociations = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByChildClass(emrel.MyMetaObject._ClassName);
                    allowedAssociations.Filter = "ParentClass = '" + MetaObject._ClassName + "' AND IsActive = 'True'";
                    // set default to first, maybe there isnt one
                    if (allowedAssociations.Count > 0)
                    {
                        emrel.MyAssociation = allowedAssociations[0];

                        #region Add Class Associations for each class that is dropped

                        foreach (KeyValuePair<string, ClassAssociation> kvp in DefaultClassBindings)
                        {
                            if (kvp.Key == MetaObject._ClassName)
                            {
                                emrel.MyAssociation = kvp.Value;
                                ObjectRelationships.Add(emrel);
                                return;
                            }
                        }

                        foreach (ClassAssociation classAssoc in allowedAssociations)
                        {
                            if (classAssoc.IsDefault)
                            {
                                if (!(DefaultClassBindings.ContainsKey(classAssoc.ChildClass)))
                                    DefaultClassBindings.Add(classAssoc.ChildClass, classAssoc);
                                emrel.MyAssociation = classAssoc;
                                ObjectRelationships.Add(emrel);
                                return;
                            }
                        }
                        // at this stage, no default associations were added, because there is no default specified. Just add the first one as the default
                        if (!DefaultClassBindings.ContainsKey(emrel.MyMetaObject._ClassName))
                            DefaultClassBindings.Add(emrel.MyMetaObject._ClassName, allowedAssociations[0]);
                        emrel.MyAssociation = allowedAssociations[0];
                        ObjectRelationships.Add(emrel);
                        return;

                        #endregion
                    }
                }
            }
        }

        #endregion

        #region Overrides

        //7 January 2013 Making these shadowed looks horrible
        public override bool Shadowed
        {
            get
            {
                if (Background != null)
                    Background.Shadowed = false;
                return false;
            }
            set
            {
                if (Background != null)
                    Background.Shadowed = false;
                base.Shadowed = false;
            }
        }

        public override GoObject SelectionObject
        {
            get
            {
                if (Grid != null)
                    return Grid;
                return base.SelectionObject;
            }
        }

        public override IGoCollection AddCollection(IGoCollection coll, bool reparentLinks)
        {
            foreach (GoObject o in coll)
            {
                Add(o);
            }

            return coll;
        }

        private void RelinkPortsAfterDrop(List<LinkPortSpec> linkPortSpecs)
        {
            foreach (LinkPortSpec lps in linkPortSpecs)
            {
                lps.Relink();
                if (Contains(lps.Link as GoLink))
                {
                    // do nothing
                }
                else
                {
                    lps.Link.GoObject.Remove();
                    if (lps.Link is FishLink)
                        Add(lps.Link as FishLink);


                    if (lps.Link is QLink)
                    {
                        if (Contains(lps.Link.ToNode) && Contains(lps.Link.FromNode))
                            Add(lps.Link as QLink);
                        else
                            Document.Add(lps.Link as QLink);
                    }
                    lps.AddFishLinks(Document);
                }
            }
        }

        /* public override bool OnSelectionDropReject(GoObjectEventArgs evt, GoView view)
         {
             if (evt.GoObject != null && (!(this.Contains(evt.GoObject))) && this != evt.GoObject)
                 if (Locked)
                     return true;
                 else
                     if (!Grid.Bounds.Contains(evt.GoObject.Bounds))
                         return true;

             return base.OnSelectionDropReject(evt, view);
         }
         */

        public override void DoMove(GoView view, PointF origLoc, PointF newLoc)
        {
            base.DoMove(view, origLoc, newLoc);
            if (Grid != null)
                Position = Grid.Position;
        }

        protected override void OnChildBoundsChanged(GoObject child, RectangleF old)
        {
            if (!Initializing)
            {
                if (child == Grid)
                {
                    //Console.WriteLine("OnChildBoundsChanged - Grid");
                    ResizeBackgroundToGrid();
                    RepositionPorts();
                }
                else
                {
                    if (child == Label || child == Background || child == Button || child is NonPrintingQuickPort)
                        return;


                    if (!Locked)
                    {
                        if (Grid.Bounds.Contains(child.Bounds))
                        {
                            // still in this VC
                        }
                        else
                        {
                            Dictionary<GoObject, int> objDictionary = new Dictionary<GoObject, int>();
                            objDictionary.Add(child, 0);
                            List<LinkPortSpec> linkPortSpecs = GroupingControl.GetLinkSpecs(objDictionary);

                            if (MetaObject != null && child is IMetaNode)
                            {
                                Remove(child);
                                Document.Add(child);
                                RelinkPortsAfterDrop(linkPortSpecs);
                            }
                        }
                    }


                }
            }
            base.OnChildBoundsChanged(child, old);
        }

        #endregion

        public override void Remove(GoObject obj)
        {
            base.Remove(obj);
        }
        public override void Add(GoObject obj)
        {
            //if (obj is MappingCell)
            //    return;
            base.Add(obj);
            if (obj is IndicatorLabel)
            {
                obj.Position = new PointF(Label.Location.X + 10, Label.Location.Y);
            }
        }

    }

    [Serializable]
    public class VCShape : GradientPolygon
    {
        #region Fields (2)

        private PointF arrowStart;
        private PointF arrowTip;

        public override GoObject SelectionObject
        {
            get
            {
                ValueChain lsg = Parent as ValueChain;
                if (lsg != null)
                    return lsg;
                return base.SelectionObject;
            }
        }

        #endregion Fields

        #region Constructors (1)

        public VCShape(float w, float h)
            : base(GoPolygonStyle.Line)
        {
            Width = w;
            Height = h;

            Resizable = true;
            Brush = Brushes.Transparent;
            Pen = Pens.Black;

            //  this.Reshapable = false;
            //  DrawStep();
        }

        #endregion Constructors

        #region Properties (4)

        public PointF ArrowStart
        {
            get { return arrowStart; }
            set { arrowStart = value; }
        }

        public PointF ArrowTip
        {
            get { return arrowTip; }
            set { arrowTip = value; }
        }

        public PointF RealArrowStart
        {
            get
            {
                float H = Height;
                ValueChain lsg = Parent as ValueChain;
                PointF topLeftCorner = lsg.Grid.Position;
                return new PointF(topLeftCorner.X + H / 3.5f, topLeftCorner.Y + H / 2);
            }
        }

        public PointF RealArrowTip
        {
            get
            {
                float H = Height;
                float W = Width;
                ValueChain lsg = Parent as ValueChain;
                PointF topLeftCorner = lsg.Grid.Position;
                PointF topRightCorner = new PointF(topLeftCorner.X + W - H / 2f, topLeftCorner.Y);
                return new PointF(topRightCorner.X + H / 2f, topRightCorner.Y + H / 2f);
            }
        }

        #endregion Properties

        #region Methods (4)

        // Public Methods (4) 

        // can't get any selection handles
        public override void AddSelectionHandles(GoSelection sel, GoObject selectedObj)
        {
        }

        public override IGoHandle CreateBoundingHandle()
        {
            GoHandle h = new GoHandle();
            //RectangleF rect = Bounds; //();
            // the handle rectangle should just go around the object
            //  rect.X--;
            //  rect.Y--;
            // rect.Height += 2;
            //  rect.Width += 2;
            h.Bounds = Bounds;
            return h;
        }

        public void DrawStep()
        {
            if (Parent != null)
            {
                ValueChain vcs = Parent as ValueChain;

                float H = vcs.Grid.Height;
                float W = vcs.Grid.Width;

                ClearPoints();

                PointF topLeftCorner = vcs.Grid.Position;
                PointF topRightCorner = new PointF(topLeftCorner.X + W - H / 3.5f, topLeftCorner.Y);
                arrowTip = new PointF(topRightCorner.X + H / 3.5f, topRightCorner.Y + H / 2f);
                PointF bottomFrontCorner = new PointF(arrowTip.X - H / 3.5f, topLeftCorner.Y + H);
                PointF bottomLeftCorner = new PointF(topLeftCorner.X, topLeftCorner.Y + H);
                arrowStart = new PointF(topLeftCorner.X + H / 3.5f, topLeftCorner.Y + H / 2);

                AddPoint(topLeftCorner); // Top Left Corner
                AddPoint(topRightCorner);
                AddPoint(arrowTip);
                AddPoint(bottomFrontCorner);
                AddPoint(bottomLeftCorner);
                AddPoint(arrowStart);
            }
        }

        #endregion Methods
    }
}