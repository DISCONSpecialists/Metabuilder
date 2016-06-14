#region Copyright © 2007 - DISCON Specialists

//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Modification and/or tampering with the code is also strictly prohibited, and
// punishable by law.
//
// Filename: MappingCell.cs
// Author: Deon Fourie
// Last Modified: 2007-13-26
//

#endregion

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.Meta;
using Northwoods.Go;
using b = MetaBuilder.BusinessLogic;
using MetaBuilder.Graphing.Containers;
using System.Collections.ObjectModel;

namespace MetaBuilder.Graphing.Shapes.Nodes.Containers
{
    // an Item represents a product that can be part of a Display,
    // or can be "loose", in a Planogram
    [Serializable]
    public class MappingCell : GoGroup, IGoLabeledNode, IShallowCopyable, ILinkedContainer, IMetaNode
    {

        public b.TList<b.ObjectAssociation> SaveToDatabase(object sender, EventArgs e)
        {
            //return null;
            MetaObject.Save(Guid.NewGuid());
            //Collection<GoObject> remove = new Collection<GoObject>();
            //foreach (GoObject o in this)
            //{
            //    if (o is IndicatorLabel)
            //    {
            //        if ((o as IndicatorLabel).TextColor == Color.Red)
            //        {
            //            remove.Add(o);
            //        }
            //    }
            //}

            if (Core.Variables.Instance.SaveOnCreate)
            {
                //save links to all the nodes(which should also be saved) in this cell
                foreach (IMetaNode overlappingNode in GetOverlappingIMetaNodes())
                {
                    if (overlappingNode is ArtefactNode || overlappingNode is Rationale)
                        continue;
                    if (overlappingNode.MetaObject != null)
                    {
                        if (!overlappingNode.MetaObject.IsInDatabase(Core.Variables.Instance.ClientProvider))
                        {
                            overlappingNode.SaveToDatabase(this, e);
                        }

                        TList<ClassAssociation> classa = DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByParentClass(MetaObject.Class);
                        int CAID = 0;
                        TList<ClassAssociation> allowed = new TList<ClassAssociation>();
                        foreach (ClassAssociation c in classa)
                        {
                            if (c.ChildClass == overlappingNode.MetaObject.Class)
                            {
                                allowed.Add(c);
                            }
                        }
                        if (allowed.Count == 1)
                        {
                            CAID = allowed[0].CAid;
                        }
                        else
                        {
                            foreach (ClassAssociation c in allowed)
                            {
                                if (c.IsDefault)
                                {
                                    CAID = c.CAid;
                                    break;
                                }
                            }
                            if (CAID == 0 && allowed.Count > 0)
                                CAID = allowed[0].CAid;
                        }

                        #region association

                        if (CAID > 0)
                        {
                            //save association
                            ObjectAssociation ass = new ObjectAssociation();
                            ass.ObjectID = MetaObject.pkid;
                            ass.ObjectMachine = MetaObject.MachineName;
                            ass.ChildObjectID = overlappingNode.MetaObject.pkid;
                            ass.ChildObjectMachine = overlappingNode.MetaObject.MachineName;
                            ass.CAid = CAID;

                            if ((ass.VCStatusID == 0) || (ass.VCStatusID == (int)VCStatusList.MarkedForDelete))
                                ass.VCStatusID = (int)VCStatusList.None;

                            b.ObjectAssociationKey asskey = new ObjectAssociationKey(ass);
                            ObjectAssociation oaExisting = DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Get(asskey);
                            if (oaExisting == null)
                                DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Save(ass);
                        }
                        else
                        {
                            //Does not match metamodel
                            Log.WriteLog("Cannot associate(MappingCell) " + MetaObject.ToString() + "(" + MetaObject.Class + ") to " + overlappingNode.MetaObject.ToString() + "(" + overlappingNode.MetaObject.Class + ") because the metamodel does not allow it.");
                        }

                        #endregion

                    }
                }

                //removes indicator labels automatically since saved
                //foreach (GoObject o in remove)
                //    o.Remove();
            }
            return null;
        }

        #region ILinkedContainer Implementation

        private Dictionary<string, ClassAssociation> defaultClassBindings;
        private List<EmbeddedRelationship> objectRelationships;

        public void PerformAddCollection(GoView view, GoCollection objectCollection)
        {
            // no need to add anything
            foreach (GoObject o in objectCollection)
            {
                if (o.Document == null)
                    Document.Add(o);
            }
        }

        public string LabelText
        {
            get { return Label.Text; }
        }

        public bool Locked
        {
            get { return false; }
        }

        public bool ObjectInAcceptedRegion(GoObject o)
        {
            return Bounds.Contains(o.Bounds) || Bounds.IntersectsWith(o.Bounds);
        }

        public List<EmbeddedRelationship> ObjectRelationships
        {
            get { return objectRelationships; }
            set { objectRelationships = value; }
        }

        public void RemoveByRelationship(EmbeddedRelationship rel)
        {
            ObjectRelationships.Remove(rel);
        }

        public Dictionary<string, ClassAssociation> DefaultClassBindings
        {
            get { return defaultClassBindings; }
            set { defaultClassBindings = value; }
        }

        #endregion

        #region IIdentifiable Implementation

        private string name;

        public string Name
        {
            get
            {
                if (name == null)
                    name = Guid.NewGuid().ToString();
                return name;
            }
            set { name = value; }
        }

        #endregion

        #region Fields (6)

        [NonSerialized]
        public EventHandler _contentsChanged;
        [NonSerialized]
        private Brush backBrush;
        private GoRectangle background;
        private MetaBase metaObject;
        [NonSerialized]
        private bool timerStarted;
        [NonSerialized]
        private MetaBase copiedFrom;
        public MetaBase CopiedFrom
        {
            get { return copiedFrom; }
            set { copiedFrom = value; }
        }
        #endregion Fields

        #region Constructors (1)

        public MappingCell()
        {
            ObjectRelationships = new List<EmbeddedRelationship>();
            DefaultClassBindings = new Dictionary<string, ClassAssociation>();

            AutoRescales = true;
            Resizable = true;
            ResizesRealtime = true;
            Printable = true;

            BackGround = new GoRectangle();
            BackGround.Selectable = false;
            BackGround.DragsNode = false;
            //back.Size = new SizeF(UnitSize*2, UnitSize*2);
            //Color c = Color.FromArgb(20, Color.Green);
            BackGround.Brush = Brushes.Transparent;
            BackGround.Printable = true;
            BackGround.Visible = true;

            Add(BackGround);

            //PickableBackground = false;
            HeaderRectangle = CreateHeaderRectangle();
            Add(HeaderRectangle);

            lll = createLabel();
            Add(lll);
            Label.AddObserver(this);
            //label.Position = new PointF(label.Position.X + 42, label.Position.Y);

            Pen p = new Pen(Color.Black, 1);
            p.DashStyle = DashStyle.Dot;
            BackGround.Pen = p;

            if (!Initializing)
                repositionRectangle();
        }
        #endregion Constructors

        #region Properties (8)

        public GoRectangle BackGround
        {
            get { return background; }
            set { background = value; }
        }

        private ExpandableTextBoxLabel createLabel()
        {
            ExpandableTextBoxLabel l = new ExpandableTextBoxLabel();
            l.Editable = false;
            l.Selectable = false;
            l.Multiline = true;
            l.Wrapping = true;
            l.Text = "Item";
            l.FontSize = 16;
            l.Bold = true;
            l.Printable = true;
            l.DragsNode = true;
            l.AutoResizes = false;
            l.AutoRescales = false;
            l.Clipping = false;
            l.Resizable = true;
            l.Reshapable = true;
            l.Deletable = false;

            return l;
        }

        public GoRectangle CreateHeaderRectangle()
        {
            GoRectangle rect = new GoRectangle();
            rect.Brush = Brushes.WhiteSmoke;
            rect.Width = 40;
            rect.Height = this.Height;
            rect.AutoRescales = false;
            rect.Selectable = false;
            rect.DragsNode = true;

            return rect;
        }

        public override GoObject SelectionObject
        {
            get
            {
                return this;
                return base.SelectionObject;
            }
        }

        //public static float UnitSize
        //{
        //    get { return float.Parse(Variables.Instance.GridCellSize.ToString(), System.Globalization.CultureInfo.InvariantCulture); }
        //}

        #region IGoLabeledNode Members

        private GoText lll;
        public GoText Label
        {
            get { return lll; }// this[2] as GoText; }
            set { lll = value; }
        }

        //private GoText customLabel;
        //public GoText CustomLabel
        //{
        //    get { return customLabel; }
        //    set { customLabel = value; }
        //}

        public String Text
        {
            get
            {
                GoText lab = Label;
                if (lab != null)
                    return lab.Text;
                return null;
            }
            set
            {
                GoText lab = Label;
                if (lab != null)
                    //if (!(value.EndsWith("\n\n")))
                    lab.Text = value;
            }
        }

        #endregion

        #region IMetaNode Members

        public EventHandler ContentsChanged
        {
            get { return _contentsChanged; }
            set { _contentsChanged = value; }
        }

        public MetaBase MetaObject
        {
            get { return metaObject; }
            set
            {
                if (value != metaObject)
                {
                    if (metaObject != null)
                        FireMetaObjectChanged(this, null);
                    metaObject = value;
                    if (metaObject == null)
                        return;

                    BindingInfo = new BindingInfo();
                    BindingInfo.BindingClass = MetaObject.Class;
                }

                if (MetaObject != null)
                {
                    Label.Editable = false;
                    //if (CustomLabel != null)
                    //    CustomLabel.Text = metaObject.Class + " Name";
                    Label.Text = MetaObject.pkid <= 0 ? MetaObject.Class + " Name" : MetaObject.ToString();
                }

            }
        }

        private bool requiresAttention;
        public bool RequiresAttention
        {
            get { return requiresAttention; }
            set
            {
                // do nothing
                requiresAttention = value;
                //try
                //{
                //    if (value)
                //    {
                //        Color c = Color.Red;
                //        BackGround.Pen.Brush = new SolidBrush(Color.FromArgb(125, c.R, c.G, c.B));
                //        borderStroke.Pen.Brush = new SolidBrush(Color.FromArgb(125, c.R, c.G, c.B));
                //    }
                //    else
                //    {
                //        BackGround.Pen.Brush = new SolidBrush(Color.Black);
                //        borderStroke.Pen.Brush = new SolidBrush(Color.Black);
                //    }
                //}
                //catch
                //{
                //    //Invalid operationexception
                //    //only when you close and choose to save while there are duplicates on the diagram
                //}
            }
        }

        #endregion

        #endregion Properties

        private bool copyAsShadow;

        #region IShallowCopyable Members

        private bool parentIsILinkedContainer;
        public bool ParentIsILinkedContainer
        {
            get { return parentIsILinkedContainer; }
            set { parentIsILinkedContainer = value; }
        }
        public bool CopyAsShadow
        {
            get { return copyAsShadow; }
            set { copyAsShadow = value; }
        }

        public GoObject CopyAsShallow()
        {
            MetaBase mo = MetaObject;
            MappingCell gnode = Copy() as MappingCell;
            gnode.MetaObject = mo;
            return gnode;
        }

        #endregion

        public override GoObject CopyObject(GoCopyDictionary env)
        {
            MappingCell cell = new MappingCell();
            cell.Location = Location;
            cell.Position = Position;
            cell.Bounds = new RectangleF(Bounds.Location, Bounds.Size);
            cell.BackGround.Bounds = new RectangleF(BackGround.Bounds.Location, BackGround.Bounds.Size);
            cell.RectangleLocation = RectangleLocation;
            cell.RepositionRectangle(RectangleLocation);

            if (MetaObject != null)
            {
                if (!CopyAsShadow)
                {
                    cell.MetaObject = Loader.CreateInstance(this.MetaObject.Class);
                    MetaObject.CopyPropertiesToObject(cell.MetaObject);
                }
                else
                {
                    cell.MetaObject = MetaObject;
                    cell.CopyAsShadow = true;
                }

                cell.ObjectRelationships.Clear();
                cell.HookupEvents();
                cell.BindToMetaObjectProperties();
            }

            cell.Initializing = false;

            env.Delayeds.Add(cell);

            return cell;



            GoObject retval = base.CopyObject(env);

            if (MetaObject != null)
            {
                (retval as MappingCell).MetaObject = Loader.CreateInstance(this.MetaObject.Class);
                this.MetaObject.CopyPropertiesToObject((retval as MappingCell).MetaObject);
                (retval as MappingCell).ObjectRelationships.Clear();
                (retval as MappingCell).HookupEvents();
                (retval as MappingCell).BindToMetaObjectProperties();
            }

            MappingCell node = retval as MappingCell;
            node.CopiedFrom = this.MetaObject;
            if (CopyAsShadow)
            {
                if (MetaObject != null)
                {
                    node.MetaObject = MetaObject;
                    node.HookupEvents();
                    node.BindToMetaObjectProperties();
                    node.CopyAsShadow = true;

                    node.Initializing = false;
                    //node.Bounds = Bounds;
                    return node;
                }
            }

            (retval as MappingCell).Label.AddObserver((retval as MappingCell));
            (retval as MappingCell).RepositionRectangle(RectangleLocation);

            return retval;
        }
        public override void CopyObjectDelayed(GoCopyDictionary env, GoObject newobj)
        {
            if (MetaObject == null)
                return;

            MappingCell node = newobj as MappingCell;
            if (node.MetaObject != null)
            {
                node.MetaObject.Changed -= FireMetaObjectChanged;
                node.MetaObject = null;
            }

            base.CopyObjectDelayed(env, node);
            if (bindingInfo != null)
                node.BindingInfo = BindingInfo.Copy();

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
                MetaBase mbase = Loader.CreateInstance(MetaObject._ClassName);
                node.MetaObject = mbase;
                MetaObject.CopyPropertiesToObject(node.MetaObject);
            }

            node.HookupEvents();
            //node.Name = Name.Substring(0, Name.Length);
            HookupEvents();
        }

        #region Methods (10)

        // Public Methods (8) 
        public void BindToMetaObjectProperties()
        {
            RectangleF b = Bounds;
            if (MetaObject == null)
                return;
            string t = MetaObject.ToString();
            if (t == null)
                return;

            //dont change the label if it is already what it should be
            if (t == Text)
                return;

            //Text = t.Replace("\r", Environment.NewLine);
            Label.Width = HeaderRectangle.Width;
            //if (CustomLabel != null)
            //{
            //    CustomLabel.Text = t.Replace("\r", Environment.NewLine);
            //    CustomLabel.AutoRescales = false;
            //}
            //21 January 2013 - WTF?
            //if (Label.Size.Width == 50)
            //{
            //    Text = "";
            Text = MetaObject.ToString();
            //}
            //RequiresAttention = false;
            Bounds = b;
        }
        public void BindMetaObjectImage()
        {
        }
        public void OnContentsChanged(object sender, EventArgs e)
        {
            if (ContentsChanged != null)
                ContentsChanged(sender, e);
            BindToMetaObjectProperties();
        }

        public override void AddSelectionHandles(GoSelection sel, GoObject selectedObj)
        {
            sel.RemoveAllSelectionHandles();
            //base.AddSelectionHandles(sel, selectedObj);

            if (!MaxWidth)
            {
                sel.CreateResizeHandle(this, selectedObj, new PointF(this.Right, this.Top), TopRight, true);
                sel.CreateResizeHandle(this, selectedObj, new PointF(this.Right, this.Top + this.Height / 2), MiddleRight, true);
                sel.CreateResizeHandle(this, selectedObj, new PointF(this.Right, this.Bottom), BottomRight, true);
            }
            if (!MaxHeight)
            {
                sel.CreateResizeHandle(this, selectedObj, new PointF(this.Right - this.Width / 2, this.Bottom), MiddleBottom, true);
                sel.CreateResizeHandle(this, selectedObj, new PointF(this.Left, this.Bottom), MiddleBottom, true);
            }

            switch (RectangleLocation)
            {
                case "Top":
                case "Bottom":
                    if (!MaxWidth)
                    {
                        sel.CreateResizeHandle(this, selectedObj, new PointF(this.Right, HeaderRectangle.Bottom), MiddleRight, true);
                    }
                    //sel.CreateResizeHandle(this, selectedObj, new PointF(BackGround.Left, HeaderRectangle.Bottom), MiddleLeft, true);
                    break;
                case "Left"://This is a restriction carried over from System.Drawing and GDI+.
                case "Right":// System.Drawing.StringAlignment only specifies Near/Center/Far values along the horizontal axis, not the vertical axis.
                    //sel.CreateResizeHandle(this, selectedObj, new PointF(Right, Top + 60), MiddleRight, true);
                    break;
            }
        }
        public override void RemoveSelectionHandles(GoSelection sel)
        {
            base.RemoveSelectionHandles(sel);
        }
        public void DoHighlight()
        {
            if (timerStarted == false)
            {
                timerStarted = true;
                backBrush = BackGround.Brush;
                //Back.Brush = Brushes.LightSteelBlue;
                BackGround.Brush = Brushes.Silver;
                /* new SolidBrush(Color.FromArgb(
                                             (r.Next(0, 255)),
                                             (r.Next(0, 255)),
                                             (r.Next(0, 255))));*/
                Pen p = new Pen(Color.Black, 2);
                p.DashStyle = DashStyle.Dot;
                BackGround.Pen = p;
                Timer timer = new Timer();
                timer.Interval = 500;
                timer.Tick += timer_Tick;
                timer.Start();
            }
        }

        public List<IMetaNode> GetOverlappingIMetaNodes()
        {
            List<IMetaNode> retval = new List<IMetaNode>();
            GoCollection collection = new GoCollection();
            RectangleF boundsToCheckWithin = Bounds;
            float gridsize = (float)(((int)Core.Variables.Instance.GridCellSize / 2) * -1);
            boundsToCheckWithin.Inflate(gridsize, gridsize);

            if (View != null)
                View.PickObjectsInRectangle(true, false, boundsToCheckWithin, GoPickInRectangleStyle.AnyIntersectsBounds, collection, 5000);
            else if (Document != null)
                Document.PickObjectsInRectangle(boundsToCheckWithin, GoPickInRectangleStyle.AnyIntersectsBounds, collection, 5000);
            else
                return retval;

            foreach (GoObject o in collection)
            {
                if (o is IMetaNode && !(o is ArtefactNode) && !(o is Rationale))
                {
                    if (o is SubgraphNode)
                    {
                        SubgraphNode sgNode = o as SubgraphNode;
                        foreach (IMetaNode n in getNodesInsideSubgraph(sgNode))
                            retval.Add(n);
                    }
                    else
                    {
                        if (o.ParentNode == o && o != this)
                            retval.Add(o as IMetaNode);
                    }
                }
            }
            return retval;
        }

        private List<IMetaNode> getNodesInsideSubgraph(SubgraphNode subGraph)
        {
            List<IMetaNode> retval = new List<IMetaNode>();
            retval.Add(subGraph as IMetaNode);
            foreach (GoObject o in subGraph)
            {
                if (o is IMetaNode && !(o is ArtefactNode) && !(o is Rationale))
                {
                    if (o is SubgraphNode)
                    {
                        SubgraphNode sgNode = o as SubgraphNode;
                        foreach (IMetaNode n in getNodesInsideSubgraph(sgNode))
                            retval.Add(n);
                    }
                    else
                    {
                        retval.Add(o as IMetaNode);
                    }
                }
            }
            return retval;
        }

        // if the Label is trimmed or if the view has a small DocScale,
        // it's handy to be able to see the text for each Item
        public override String GetToolTip(GoView view)
        {
            return Text;
        }

        // change the natural Location to be the BottomLeft corner,
        // so that Items will "fall" onto Shelves appropriately
        /*public override PointF Location
        {
            get { return GetSpotLocation(BottomLeft); }
            set { SetSpotLocation(BottomLeft, value); }
        }*/
        // make sure the Icon and the Label are inside the Back,
        // positioned at or near the top-left corner of the Back



        // Protected Methods (1) 



        // Private Methods (1) 

        private void timer_Tick(object sender, EventArgs e)
        {
            BackGround.Pen = new Pen(Color.Black, 1f);
            BackGround.Brush = backBrush;
            Timer timer = sender as Timer;
            timer.Enabled = false;
            timerStarted = false;
        }

        #endregion Methods

        protected override void OnChildBoundsChanged(GoObject child, RectangleF old)
        {
            base.OnChildBoundsChanged(child, old);
            //if (child is GoRectangle)
            //repositionRectangle();
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
                        Log.WriteLog("MappingCell.OnObservedChanged : Cannot bind text to metaobject name for class " + MetaObject.Class + " for swimlane");
                    }
                }
                //if (subhint == 1001)
                //{
                //    if (this.BackGround.Width < Label.Width)
                //    {
                //        this.BackGround.Width = Label.Width;
                //        this.BackGround.Bounds.Inflate(4, 0);
                //    }
                //    if (this.BackGround.Height < Label.Height)
                //    {
                //        this.BackGround.Height = Label.Height;
                //        this.BackGround.Bounds.Inflate(0, 4);
                //    }
                //    this.BackGround.Position = new PointF(Label.Position.X - 2, Label.Position.Y - 2);
                //}
            }
        }
        // it can be resized only in positive multiples of Item.UnitSize
        public override RectangleF ComputeResize(RectangleF origRect, PointF newPoint, int handle, SizeF min, SizeF max, bool reshape)
        {
            RectangleF r = base.ComputeResize(origRect, newPoint, handle, min, max, reshape);

            //SizeF labelSize = Label.Size;
            //if (Label.Text == "")
            //{
            //    labelSize = new SizeF(50, 50);
            //}

            //r.Width = Math.Max(r.Width, labelSize.Width);
            //r.Height = Math.Max(r.Height, labelSize.Height);

            //if (r.Width > labelSize.Width)
            //    labelSize.Width = r.Width;
            //if (r.Height > labelSize.Height)
            //    labelSize.Height = r.Height;

            //BackGround.Bounds = r;
            repositionRectangle();
            return r;
        }

        #region Standard implementation of IMetaNode

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
            BackGround.Height = this.Height;
            //repositionRectangle();
        }

        public void CreateMetaObject(object sender, EventArgs args)
        {
            if (HasBindingInfo)
            {
                if (BindingInfo.BindingClass != null)
                {
                    MetaObject = Loader.CreateInstance(BindingInfo.BindingClass);
                    Label.Editable = false;
                    HookupEvents();
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
                    Label.Editable = false;
                    HookupEvents();
                    BindToMetaObjectProperties();
                    //FireMetaObjectChanged(null, EventArgs.Empty);
                }
            }
        }

        public void FireMetaObjectChanged(object sender, EventArgs e)
        {
            BindToMetaObjectProperties();
            if (Core.Variables.Instance.SaveOnCreate)
                SaveToDatabase(sender, e);
        }

        #endregion

        private GoStroke borderStroke;
        private void buildBorder()
        {
            //return;
            if (borderStroke == null)
            {
                borderStroke = new GoStroke();
                borderStroke.Selectable = false;
                borderStroke.DragsNode = true;
                Pen p = new Pen(Color.Black, 1f);
                p.DashStyle = DashStyle.Dot;
                borderStroke.Pen = p;
                //borderStroke.Bounds = BackGround.Bounds;
                this.Add(borderStroke);
            }
            else
            {
                borderStroke.ClearPoints();
            }

            borderStroke.AddPoint(BackGround.Left, BackGround.Top);
            borderStroke.AddPoint(BackGround.Right, BackGround.Top);
            borderStroke.AddPoint(BackGround.Right, BackGround.Bottom);
            borderStroke.AddPoint(BackGround.Left, BackGround.Bottom);
            borderStroke.AddPoint(BackGround.Left, BackGround.Top);
            //switch (RectangleLocation)
            //{
            //    case "Top":
            //        borderStroke.AddPoint(BackGround.Left, BackGround.Top);
            //        borderStroke.AddPoint(BackGround.Right, BackGround.Top);
            //        borderStroke.AddPoint(BackGround.Right, BackGround.Bottom);
            //        borderStroke.AddPoint(BackGround.Left, BackGround.Bottom);
            //        borderStroke.AddPoint(BackGround.Left, HeaderRectangle.Top);
            //        break;
            //    case "Bottom":
            //        borderStroke.AddPoint(BackGround.Left, BackGround.Top);
            //        borderStroke.AddPoint(BackGround.Right, BackGround.Top);
            //        borderStroke.AddPoint(BackGround.Right, BackGround.Bottom);
            //        borderStroke.AddPoint(BackGround.Left, BackGround.Bottom);
            //        borderStroke.AddPoint(BackGround.Left, BackGround.Top);
            //        break;
            //    case "Left":
            //        borderStroke.AddPoint(BackGround.Left, BackGround.Top);
            //        borderStroke.AddPoint(BackGround.Right, BackGround.Top);
            //        borderStroke.AddPoint(BackGround.Right, BackGround.Bottom);
            //        borderStroke.AddPoint(BackGround.Left, BackGround.Bottom);
            //        borderStroke.AddPoint(BackGround.Left, BackGround.Top);
            //        break;
            //    case "Right":
            //        borderStroke.AddPoint(BackGround.Left, BackGround.Top);
            //        borderStroke.AddPoint(BackGround.Right, BackGround.Top);
            //        borderStroke.AddPoint(BackGround.Right, BackGround.Bottom);
            //        borderStroke.AddPoint(BackGround.Left, BackGround.Bottom);
            //        borderStroke.AddPoint(BackGround.Left, BackGround.Top);
            //        break;
            //    default:
            //        borderStroke.AddPoint(BackGround.Left, BackGround.Top);
            //        borderStroke.AddPoint(BackGround.Right, BackGround.Top);
            //        borderStroke.AddPoint(BackGround.Right, BackGround.Bottom);
            //        borderStroke.AddPoint(BackGround.Left, BackGround.Bottom);
            //        borderStroke.AddPoint(BackGround.Left, BackGround.Top);
            //        break;
            //}

            if (BackGround.Visible)
            {
                borderStroke.ClearPoints();
            }
            //BackGround.Visible = true;
        }

        public string RectangleLocation = "Left";
        public void RepositionRectangle(string position)
        {
            RectangleLocation = position;
            repositionRectangle();
        }
        private void repositionRectangle()
        {
            //return;
            if (this.View != null)
            {
                this.View.Document.SkipsUndoManager = true;
                View.StartTransaction();
            }

            BackGround.Visible = false;
            //if (CustomLabel == null)
            //{
            //    CustomLabel = Label.Copy() as GoText;
            //    Add(CustomLabel);
            //}
            //else
            //{
            //    CustomLabel.TextColor = Label.TextColor;
            //    CustomLabel.FontSize = Label.FontSize;
            //    CustomLabel.Bold = Label.Bold;
            //    CustomLabel.Italic = Label.Italic;
            //    CustomLabel.FamilyName = Label.FamilyName;
            //    CustomLabel.Multiline = Label.Multiline;
            //    CustomLabel.StrikeThrough = Label.StrikeThrough;
            //    CustomLabel.StringTrimming = Label.StringTrimming;
            //    CustomLabel.Underline = Label.Underline;
            //    //CustomLabel.WrappingWidth = Label.WrappingWidth;
            //}
            HeaderRectangle.Visible = true;
            //double switch ftw -.-
            switch (RectangleLocation)
            {
                case "Top":
                case "Bottom":
                    HeaderRectangle.Size = new SizeF(BackGround.Width, 60);
                    Label.Width = HeaderRectangle.Width;
                    Label.Height = HeaderRectangle.Height;
                    Label.Wrapping = true;
                    Label.WrappingWidth = HeaderRectangle.Width;
                    break;
                case "Left"://This is a restriction carried over from System.Drawing and GDI+.
                case "Right":// System.Drawing.StringAlignment only specifies Near/Center/Far values along the horizontal axis, not the vertical axis.
                    HeaderRectangle.Size = new SizeF(140, BackGround.Height);
                    Label.Height = HeaderRectangle.Height;
                    Label.Width = HeaderRectangle.Width;
                    Label.Wrapping = true;
                    Label.WrappingWidth = HeaderRectangle.Width;
                    break;
                default:
                    HeaderRectangle.Visible = false;
                    BackGround.Visible = true;
                    Label.Height = BackGround.Height;
                    Label.Width = BackGround.Width;
                    Label.WrappingWidth = BackGround.Width;
                    Label.Wrapping = true;
                    break;
            }

            //Label.Visible = !HeaderRectangle.Visible;
            //CustomLabel.Visible = !Label.Visible;
            //CustomLabel.Alignment = GoObject.NoSpot;
            //CustomLabel.Printable = !Label.Visible;
            //Label.Printable = Label.Visible;

            switch (RectangleLocation)
            {
                case "Top":
                    HeaderRectangle.Position = new PointF(BackGround.Left, BackGround.Top);
                    break;
                case "Bottom":
                    HeaderRectangle.Position = new PointF(BackGround.Left, Bottom - 60);
                    break;
                case "Right":
                    HeaderRectangle.Position = new PointF(BackGround.Right - 140, Top);
                    break;
                case "Left":
                    HeaderRectangle.Position = new PointF(BackGround.Left, BackGround.Top);
                    break;
                default:
                    HeaderRectangle.Position = new PointF(BackGround.Left, BackGround.Top);
                    HeaderRectangle.Visible = false;
                    Label.Printable = true;
                    //CustomLabel.Printable = false;
                    break;
            }

            //Label.Location = new PointF(BackGround.Location.X + 1, BackGround.Location.Y + 1);

            Label.Location = HeaderRectangle.Location;
            Label.Left += 1;
            Label.Top += 1;

            buildBorder();

            BackGround.Printable = BackGround.Visible;

            foreach (GoObject obj in this)
            {
                if (obj is IndicatorLabel)
                {
                    obj.Position = new PointF(HeaderRectangle.Location.X + 10, HeaderRectangle.Location.Y);
                }
            }

            if (this.View != null)
            {
                View.FinishTransaction("Swimlane repositioned");
                this.View.Document.SkipsUndoManager = false;
            }
        }

        private GoRectangle headerRectangle;
        public GoRectangle HeaderRectangle
        {
            get { return headerRectangle; }
            set { headerRectangle = value; }
        }

        public override void Add(GoObject obj)
        {
            if (obj is MappingCell)
                return;
            base.Add(obj);
            if (obj is IndicatorLabel)
            {
                obj.Position = new PointF(HeaderRectangle.Location.X + 10, HeaderRectangle.Location.Y);
            }
        }

        public bool MaxWidth = false;
        public bool MaxHeight = false;

        public override void Paint(Graphics g, GoView view)
        {
            try
            {
                if (Document != null)
                {
                    if (MaxWidth)
                    {
                        if (Width == (Document as NormalDiagram).DocumentFrame.Width - this.Left + 60)
                            goto end;
                        Width = (Document as NormalDiagram).DocumentFrame.Width - this.Left + 60;
                        repositionRectangle();
                        InvalidateViews();
                    }
                    else if (MaxHeight)
                    {
                        if (Height == (Document as NormalDiagram).DocumentFrame.Height - this.Top + 60)
                            goto end;
                        Height = (Document as NormalDiagram).DocumentFrame.Height - this.Top + 60;
                        repositionRectangle();
                        InvalidateViews();
                    }

                }
            }
            catch
            {
            }

        end:
            base.Paint(g, view);

            if (Masked)
            {
                SolidBrush b = new SolidBrush(Color.FromArgb(50, 100, 100, 100));
                g.FillRectangle(b, BackGround.Location.X, BackGround.Location.Y, BackGround.Width, BackGround.Height);
            }
        }

        private bool masked;
        public bool Masked
        {
            get { return masked; }
            set { masked = value; }
        }

    }
}