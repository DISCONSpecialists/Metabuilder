/*
 *  Copyright © Northwoods Software Corporation, 2000-2006. All Rights
 *  Reserved.
 *
 *  Restricted Rights: Use, duplication, or disclosure by the U.S.
 *  Government is subject to restrictions as set forth in subparagraph
 *  (c) (1) (ii) of DFARS 252.227-7013, or in FAR 52.227-19, or in FAR
 *  52.227-14 Alt. III, as applicable.
 */

using System;
using System.Drawing;
using System.Windows.Forms;
using MetaBuilder.Core;
using MetaBuilder.Meta;
using Northwoods.Go;
using MetaBuilder.Graphing.Shapes.Nodes;
using b = MetaBuilder.BusinessLogic;
using System.Collections.Generic;
using MetaBuilder.BusinessFacade.MetaHelper;
namespace MetaBuilder.Graphing.Shapes
{
    [Serializable]
    public class ArtefactNode : GoBasicNode, IMetaNode, IShallowCopyable
    {
        private FishLink shallowFishLink;
        public FishLink ShallowFishLink
        {
            get { return shallowFishLink; }
            set { shallowFishLink = value; }
        }

        #region Fields (6)

        [NonSerialized]
        public EventHandler _contentsChanged;
        private BindingInfo bindingInfo;
        private MetaBase metaObject;
        private GoRectangle r;

        #endregion Fields

        #region Constructors (1)

        public ArtefactNode()
        {
            PathGradientRoundedRectangle rect = new PathGradientRoundedRectangle();
            rect.FringeColor = Color.Orange;

            PickableBackground = true;
            Editable = true;
            Resizable = true;
            Selectable = true;
            AutoResizes = true;

            Port.FromSpot = NoSpot;
            Port.ToSpot = NoSpot;
            Port.PortObject = Shape;

            Text = "Artefact"; // creates the Label

            //MiddleLabelMargin = new SizeF(20, 20); // without so much space around the text

            Shape = rect;

            Shape.DragsNode = true;

            Shape.Selectable = true;
            Shape.Resizable = true;
            Shape.Printable = false;
            Shape.Visible = false;

            SetLabelProperties();
        }

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

        #endregion Constructors

        #region Properties (8)

        private bool parentIsILinkedContainer;
        public bool ParentIsILinkedContainer
        {
            get { return parentIsILinkedContainer; }
            set { parentIsILinkedContainer = value; }
        }
        public override bool Copyable
        {
            get
            {
                return true; // return base.Copyable;
            }
            set { base.Copyable = value; }
        }

        public override PointF Location
        {
            get
            {
                if (View != null)
                {
                    return new PointF(View.Right - Width, View.Top);
                }
                return base.Location;
            }
            set { base.Location = value; }
        }

        public BindingInfo BindingInfo
        {
            get { return bindingInfo; }
            set { bindingInfo = value; }
        }

        public EventHandler ContentsChanged
        {
            get { return _contentsChanged; }
            set { _contentsChanged = value; }
        }

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

        public MetaBase MetaObject
        {
            get { return metaObject; }
            set
            {
                metaObject = value;
                hooked = false;
                HookupEvents();
            }
        }

        public bool RequiresAttention
        {
            get { return false; }
            set
            {
                // do nothing
                if (value == true)
                {
                    (Shape as PathGradientRoundedRectangle).FringeColor = Color.Red;
                }
                else
                {
                    (Shape as PathGradientRoundedRectangle).FringeColor = Color.Orange;
                }
            }
        }

        #endregion Properties

        [NonSerialized]
        private MetaBase copiedFrom;
        public MetaBase CopiedFrom
        {
            get { return copiedFrom; }
            set { copiedFrom = value; }
        }

        #region IIdentifiable Implementation

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        #endregion

        #region Methods (23)

        // Public Methods (18) 

        public void BindToMetaObjectProperties()
        {
            if (MetaObject != null)
            {
                if (MetaObject.ToString() != null)
                {
                    Text = MetaObject.ToString();
                    OnChildBoundsChanged(Label, Label.Bounds);
                }
            }
        }
        public void BindMetaObjectImage()
        {
        }
        public void CreateMetaObject(object sender, EventArgs e)
        {
            if (BindingInfo.BindingClass != null)
            {
                MetaObject = Loader.CreateInstance(BindingInfo.BindingClass);
                HookupEvents();
            }
        }
        public void CreateMetaObject()
        {
            if (BindingInfo.BindingClass != null)
            {
                MetaObject = Loader.CreateInstance(BindingInfo.BindingClass);
                HookupEvents();
            }
        }

        public void FireMetaObjectChanged(object sender, EventArgs e)
        {
            OnContentsChanged(sender, e);
            BindToMetaObjectProperties();
            SetLabelProperties();

            if (Core.Variables.Instance.SaveOnCreate)
            {
                SaveToDatabase(sender, e);
            }
        }

        bool hooked = false;
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
                //if (metaObject != null)
                {
                    MetaObject.Changed += FireMetaObjectChanged;
                    hooked = true;
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
                    HookupEvents();
                    FireMetaObjectChanged(null, EventArgs.Empty);
                }
            }
        }

        public virtual void OnContentsChanged(object sender, EventArgs e)
        {
            if (ContentsChanged != null)
            {
                ContentsChanged(this, e);
            }
        }

        public b.TList<b.ObjectAssociation> SaveToDatabase(object sender, EventArgs e)
        {
            if (MetaObject != null)
            {
                MetaObject.Save(Guid.NewGuid());
            }
            if (Core.Variables.Instance.SaveOnCreate)
            {
                try
                {
                    if (this.Port is FishNodePort)
                    {
                        foreach (IGoLink l in (this.Port as FishNodePort).Links.GetEnumerator())
                        {
                            //if there is more than one here?
                            if (l is FishLink)
                            {
                                if ((l as FishLink).ToQLink != null)
                                {
                                    try
                                    {
                                        MetaBuilder.BusinessLogic.Artifact art = new MetaBuilder.BusinessLogic.Artifact();
                                        art.ArtifactObjectID = MetaObject.pkid;
                                        art.ArtefactMachine = MetaObject.MachineName;
                                        art.ObjectID = (l as FishLink).ToQLink.DBAssociation.ObjectID;
                                        art.ChildObjectID = (l as FishLink).ToQLink.DBAssociation.ChildObjectID;
                                        art.ObjectMachine = (l as FishLink).ToQLink.DBAssociation.ObjectMachine;
                                        art.ChildObjectMachine = (l as FishLink).ToQLink.DBAssociation.ChildObjectMachine;
                                        art.CAid = (l as FishLink).ToQLink.DBAssociation.CAid;
                                        //if saving artifact fails then association is null/exists.

                                        //if it exists?
                                        MetaBuilder.DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ArtifactProvider.Save(art);

                                        if ((l as FishLink).ToQLink.AssociationType == LinkAssociationType.Mapping)
                                        {
                                            Association oppositeAssociation = Singletons.GetAssociationHelper().GetAssociation(((l as FishLink).ToQLink.ToNode as IMetaNode).MetaObject._ClassName, ((l as FishLink).ToQLink.FromNode as IMetaNode).MetaObject._ClassName, (int)(l as FishLink).ToQLink.AssociationType);
                                            if (oppositeAssociation != null)
                                            {
                                                MetaBuilder.BusinessLogic.Artifact twoWayArt = new MetaBuilder.BusinessLogic.Artifact();
                                                twoWayArt.ArtifactObjectID = MetaObject.pkid;
                                                twoWayArt.ArtefactMachine = MetaObject.MachineName;
                                                twoWayArt.ChildObjectID = (l as FishLink).ToQLink.DBAssociation.ObjectID;
                                                twoWayArt.ObjectID = (l as FishLink).ToQLink.DBAssociation.ChildObjectID;
                                                twoWayArt.ChildObjectMachine = (l as FishLink).ToQLink.DBAssociation.ObjectMachine;
                                                twoWayArt.ObjectMachine = (l as FishLink).ToQLink.DBAssociation.ChildObjectMachine;
                                                twoWayArt.CAid = oppositeAssociation.ID;
                                                //if saving artifact fails then association is null/exists.

                                                //if it exists?
                                                MetaBuilder.DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ArtifactProvider.Save(twoWayArt);
                                            }
                                        }
                                    }
                                    catch (Exception exInner)
                                    {
                                        Core.Log.WriteLog(exInner.ToString());
                                    }
                                }
                                else
                                {
                                    //create task
                                    l.ToString();
                                }
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Core.Log.WriteLog(ex.ToString());
                }
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
            return null;
        }

        public void AddHighLight()
        {
            Shape.Visible = true;
        }
        public void RemoveHighLight()
        {
            Shape.Visible = false;
        }

        private bool copyAsShadow;
        public bool CopyAsShadow
        {
            get { return copyAsShadow; }
            set { copyAsShadow = value; }
        }
        public GoObject CopyAsShallow()
        {
            MetaBase mo = MetaObject;
            ArtefactNode gnode = Copy() as ArtefactNode;
            gnode.MetaObject = mo;
            return gnode;
        }
        public override GoObject CopyObject(GoCopyDictionary env)
        {
            ArtefactNode newobj = (ArtefactNode)base.CopyObject(env);
            for (int o = 0; o < newobj.Count; o++)
            {
                if (newobj[o] is IndicatorLabel)
                {
                    env.Remove(newobj[o]);
                    newobj[o].Remove();
                }
            }
            newobj.CopiedFrom = this.MetaObject;
            newobj.hooked = false;
            if (newobj != null)
            {
                string className = newobj.MetaObject._ClassName;

                if (CopyAsShadow)
                {
                    newobj.MetaObject = this.MetaObject;
                }
                else
                {
                    newobj.MetaObject = Loader.CreateInstance(className);
                    if (MetaObject != null)
                        MetaObject.CopyPropertiesToObject(newobj.MetaObject);
                }

                newobj.HookupEvents();
                newobj.FireMetaObjectChanged(this, EventArgs.Empty);
            }
            return newobj;
        }

        public override GoControl CreateEditor(GoView view)
        {
            return null;
        }

        public override void LayoutChildren(GoObject childchanged)
        {
            if (childchanged is IndicatorLabel || childchanged is ChangedIndicatorLabel)
                return;
            //if (childchanged == Shape)
            //{
            //    if (Label != null)
            //    {
            //        SetLabelToShapeRelativeBounds();
            //    }
            //}

            base.LayoutChildren(childchanged);
        }

        public override bool OnEnterLeave(GoObject from, GoObject to, GoView view)
        {
            if (to == this)
            {
                AddHighLight();
                DoFishLinkHighLight(true);
            }
            else
            {
                RemoveHighLight();
                DoFishLinkHighLight(false);
            }

            return base.OnEnterLeave(from, to, view);
        }
        public override bool OnHover(GoInputEventArgs evt, GoView view)
        {
            Shape.Visible = true;
            return base.OnHover(evt, view);
        }

        public override void Paint(Graphics g, GoView view)
        {
            if (Shape != null)
            {
                if (view.IsPrinting)
                    Shape.Visible = false;
            }
            base.Paint(g, view);
        }

        public override void Add(GoObject obj)
        {
            if (Label != null)
            {
                int lblAlignment = Label.Alignment;
                base.Add(obj);
                Label.Alignment = lblAlignment;
                return;
            }
            base.Add(obj);
        }

        // Protected Methods (2) 
        // a FishNode uses a FishNodePort instead of a regular GoPort
        protected override GoPort CreatePort()
        {
            FishNodePort retval = new FishNodePort();
            retval.PortObject = r;
            retval.FromSpot = NoSpot;
            return retval;
        }

        // a FishNode uses a rectangular shape instead of an elliptical one
        protected override GoShape CreateShape(GoPort p)
        {
            return null;
            r = new GoRectangle();
            r.Brush = Brushes.Black;
            r.Printable = false;
            r.Pen = new Pen(Color.Red);
            r.Selectable = false;
            r.Visible = true;
            if (Label != null)
            {
                r.Height = Label.Height + 10;
                r.Width = Label.Width + 10;
            }
            return r;
        }
        // Private Methods (3) 
        private void DoFishLinkHighLight(bool highlighting)
        {
            if (Port != null)
            {
                FishNodePort nodePort = Port as FishNodePort;
                GoPortFilteredLinkEnumerator portLinks = nodePort.DestinationLinks.GetEnumerator();
                while (portLinks.MoveNext())
                {
                    try
                    {
                        FishLink link = portLinks.Current as FishLink;
                        if (link != null)
                        {
                            if (highlighting)
                                link.AddHighLight();
                            else
                                link.RemoveHighLight();
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }
        bool LabelPropertiesSet;
        private void SetLabelProperties()
        {
            Label.Wrapping = true;
            Label.Clipping = true;
            Label.Deletable = false;
            Label.AutoResizes = true;
            Label.Resizable = true;
            Label.Editable = false;
            Label.Multiline = true;
            LabelSpot = Middle;
            Label.Bordered = false;
            //Label.Clipping = true;
            Label.DragsNode = true;
            Label.Selectable = true;
            Label.Wrapping = true;

            //ending edit causes resize of your resized artefactnode without
            if (!LabelPropertiesSet)
            {
                Label.WrappingWidth = 150;
                Label.Width = 150;
                Label.Height = 25;
            }

            LabelPropertiesSet = true;
        }

        #endregion Methods

        //#region Nested Classes (1)

        //// define a class inheriting from CheckBox in order to handle
        //// focus, key handling, and initialization
        //private class PropertyGridControl : PropertyGrid, IGoControlObject
        //{
        //    #region Fields (2)

        //    // CheckBoxControl state
        //    private GoControl myGoControl;
        //    private GoView myGoView;

        //    #endregion Fields

        //    #region Constructors (1)

        //    // nested class
        //    public PropertyGridControl()
        //    {
        //        ToolbarVisible = false;
        //    }

        //    #endregion Constructors

        //    #region Properties (2)

        //    public GoControl GoControl
        //    {
        //        get { return myGoControl; }
        //        set
        //        {
        //            GoControl old = myGoControl;
        //            if (old != value)
        //            {
        //                myGoControl = value;
        //                if (value != null)
        //                {
        //                    ArtefactNode tog = value.EditedObject as ArtefactNode;
        //                    if (tog != null)
        //                    {
        //                        SelectedObject = tog.MetaObject;
        //                        // do CheckBox initialization dependent on the RectangleWithCheckBoxEditor
        //                        Text = "PROPERTYGRIDEDITOR";
        //                        // this.Checked = (tog.Brush == Brushes.Red);
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    public GoView GoView
        //    {
        //        get { return myGoView; }
        //        set { myGoView = value; }
        //    }

        //    #endregion Properties

        //    #region Methods (5)

        //    // Protected Methods (4) 

        //    protected override void OnLeave(EventArgs evt)
        //    {
        //        // must be done with editing
        //        GoControl.DoEndEdit(GoView);
        //        base.OnLeave(evt);
        //    }

        //    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        //    {
        //        if (keyData == (Keys.Tab | Keys.Shift))
        //        {
        //            MoveTabBack();
        //            return true;
        //        }

        //        if (keyData == Keys.F2)
        //        {
        //            return true;
        //            try
        //            {
        //                //MessageBox.Show(this,this.SelectedGridItem.Value.ToString());
        //                GridItem gi = SelectedGridItem;
        //                MetaBase mo = SelectedObject as MetaBase;

        //                Form f = new Form();
        //                f.Size = new Size(300, 200);
        //                Button btnOK = new Button();
        //                btnOK.Text = "OK";
        //                Button btnCancel = new Button();
        //                btnCancel.Text = "Cancel";

        //                TextBox txt = new TextBox();

        //                f.Controls.Add(txt);
        //                txt.Size = new Size(f.Width, f.Height - 50);
        //                txt.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left;
        //                txt.Multiline = true;
        //                object o = mo.Get(gi.PropertyDescriptor.Name);
        //                if (o != null)
        //                    txt.Text = mo.Get(gi.PropertyDescriptor.Name).ToString();
        //                f.Controls.Add(btnOK);
        //                f.Controls.Add(btnCancel);
        //                btnCancel.Location = new Point(218, 150);
        //                btnOK.Location = new Point(140, 150);
        //                btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        //                btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        //                btnCancel.DialogResult = DialogResult.Cancel;
        //                btnOK.DialogResult = DialogResult.OK;

        //                f.StartPosition = FormStartPosition.CenterScreen;

        //                f.ShowDialog(this);
        //                DialogResult res = f.DialogResult;
        //                if (res == DialogResult.OK)
        //                    mo.Set(gi.PropertyDescriptor.Name, txt.Text);

        //                ProcessTabKey(true);
        //            }
        //            catch
        //            {
        //            }
        //        }
        //        return base.ProcessCmdKey(ref msg, keyData);
        //    }

        //    protected override bool ProcessDialogKey(Keys key)
        //    {
        //        if (key == Keys.Escape)
        //        {
        //            // cancelling; in this simple example, treated the same as Enter and Tab
        //            GoControl.DoEndEdit(GoView);
        //            return true;
        //        }
        //        if (key == Keys.Enter)
        //        {
        //            // done with editing
        //            GoControl.DoEndEdit(GoView);
        //            return true;
        //        }
        //        return base.ProcessDialogKey(key);
        //    }

        //    protected override bool ProcessTabKey(bool forward)
        //    {
        //        GridItem selectedItem = SelectedGridItem;
        //        GridItem root = selectedItem.Parent;
        //        if (forward)
        //        {
        //            bool selectnextitem = false;
        //            foreach (GridItem child in root.GridItems)
        //            {
        //                if (selectnextitem)
        //                {
        //                    child.Select();
        //                    break;
        //                }
        //                if (child == selectedItem)
        //                {
        //                    selectnextitem = true;
        //                }
        //            }
        //        }
        //        return true;
        //    }

        //    // Private Methods (1) 

        //    private void MoveTabBack()
        //    {
        //        GridItem selectedItem = SelectedGridItem;
        //        GridItem root = selectedItem.Parent;
        //        int itemcount = root.GridItems.Count;
        //        bool selectnextitem = false;
        //        for (int i = itemcount - 1; i >= 0; i--)
        //        {
        //            if (selectnextitem)
        //            {
        //                root.GridItems[i].Select();
        //                break;
        //            }
        //            if (root.GridItems[i] == selectedItem)
        //            {
        //                selectnextitem = true;
        //            }
        //        }
        //    }

        //    #endregion Methods
        //}

        //#endregion Nested Classes

        /*public override void CopyObjectDelayed(GoCopyDictionary env, GoObject newobj)
        {
            ArtefactNode node = newobj as ArtefactNode;
            if (node.MetaObject != null)
            {
                node.MetaObject.Changed -= FireMetaObjectChanged;
                node.MetaObject = null;
            }


            base.CopyObjectDelayed(env, node);
            if (bindingInfo != null)
                node.BindingInfo = BindingInfo.Copy();

            if (metaObject == null)
            {
                CreateMetaObject(this, null);
            }

          
                node.CreateMetaObject(null, EventArgs.Empty);
                metaObject.CopyPropertiesToObject(node.MetaObject);


            node.HookupEvents();
            HookupEvents();
        }*/
    }

    [Serializable]
    public class FishNodePort : GoPort
    {
        public FishNodePort()
        {
            this.Size = new SizeF(0, 0);
            this.Style = GoPortStyle.None;  // the port has no visual appearance
            this.IsValidFrom = false;  // can only draw links to this port, not from it
            this.ToSpot = NoSpot;  // the connection point is computed dynamically
            this.IsValidTo = false;  // cannot draw links to it
            this.FromSpot = NoSpot;  // the connection point is computed dynamically
        }
        // can only draw a new link from this node if there aren't any links already
        public override bool CanLinkFrom()
        {
            //base.CanLinkFrom() &&
            return this.LinksCount == 0;
        }

        // links connect to this port at the point where they intersect
        // with the Shape on the line that goes to the point returned by
        // FishLinkPort.NearestPoint
        public override PointF GetFromLinkPoint(IGoLink link)
        {
            if (link != null && link.ToPort is FishLinkPort)
            {
                FishLinkPort p = (FishLinkPort)link.ToPort;
                return GetLinkPointFromPoint(p.NearestPoint(this.Center));
            }
            else
            {
                return base.GetFromLinkPoint(link);
            }
        }

        // this is just the point on the line that is closest to the given point
        public PointF NearestPoint(PointF c)
        {
            GoStroke s = this.PortObject as GoStroke;
            if (s != null && s.PointsCount == 2)
            {
                PointF p;
                GoStroke.NearestPointOnLine(s.GetPoint(0), s.GetPoint(1), c, out p);
                return p;
            }
            else
            {
                return this.Center;
            }
        }

        // customize the connection point to be the nearest point to the center
        // of the "from" port
        public override PointF GetToLinkPoint(IGoLink link)
        {
            if (link.FromPort != null)
                return NearestPoint(link.FromPort.GoObject.Center);
            else
                return base.GetToLinkPoint(link);
        }
    }

    /*
 *  Copyright © Northwoods Software Corporation, 2000-2006. All Rights
 *  Reserved.
 *
 *  Restricted Rights: Use, duplication, or disclosure by the U.S.
 *  Government is subject to restrictions as set forth in subparagraph
 *  (c) (1) (ii) of DFARS 252.227-7013, or in FAR 52.227-19, or in FAR
 *  52.227-14 Alt. III, as applicable.
 */

    [Serializable]
    public class FishLink : GoLabeledLink
    {
        public FishLink()
        {
            this.ToArrow = true;
            // add a FishLinkPort to the link
            // (but links to this port actually "connect" to the link
            //  at the point computed by FishLinkPort.GetToLinkPoint)
            this.MidLabelCentered = true;
            FishLinkPort p = new FishLinkPort();
            p.PortObject = this.RealLink;  // tell the FishLinkPort which GoStroke it should "connect" to
            this.MidLabel = p;  // add the FishLinkPort to the FishLink
            Pen = new Pen(Color.LightBlue, 2f);
            Brush = new SolidBrush(Color.LightBlue);
            Movable = false;
            Resizable = false;
            Reshapable = false;
            Relinkable = true;
            Selectable = true;
        }

        #region Properties (4)

        public IMetaNode ArtefactShallow;

        public IMetaNode Artefact
        {
            get
            {
                IMetaNode node = null;
                GoPort p = RealLink.FromPort as GoPort;
                if (p != null)
                {
                    node = p.Parent as IMetaNode;
                }
                return node;
            }
        }
        public IGoPort ArtefactPort
        {
            get
            {
                if (Artefact is ArtefactNode)
                {
                    return null;
                }
                else
                {
                    return FromPort as IGoPort;
                }
            }
        }

        public override bool Printable
        {
            get { return Variables.Instance.PrintArtefactLines; }
            set { base.Printable = value; }
        }

        public QLink ToQlinkShallow;

        public QLink ToQLink
        {
            get
            {
                GoPort p = RealLink.ToPort as GoPort;
                QLink slink = p.Parent.Parent as QLink;
                return slink;
            }
        }

        #endregion Properties

        public override bool Deletable
        {
            get
            {
                return false;
            }
            set
            {
                base.Deletable = value;
            }
        }

        #region Methods (6)

        // Public Methods (6) 

        public override void AddSelectionHandles(GoSelection sel, GoObject selectedObj)
        {
            //base.AddSelectionHandles(sel, selectedObj);
        }

        // the FishLink group uses a FishRealLink instead of a regular GoLink
        public override GoLink CreateRealLink()
        {
            return new FishRealLink();
        }

        public bool IsValidFishLink()
        {
            if (RealLink.ToPort == null && RealLink.FromPort == null)
            {
                return false;
            }
            return true;
        }

        public override bool OnEnterLeave(GoObject from, GoObject to, GoView view)
        {
            if (to == this)
            {
                AddHighLight();
                if (FromNode != null)
                {
                    IMetaNode artNode = FromNode as IMetaNode;
                    if (artNode != null)
                        if (artNode is ArtefactNode)
                        {
                            (artNode as ArtefactNode).AddHighLight();
                        }
                }
            }
            else
            {
                RemoveHighLight();
                if (FromNode != null)
                {
                    IMetaNode artNode = FromNode as IMetaNode;
                    if (artNode != null)
                        if (artNode is ArtefactNode)
                        {
                            (artNode as ArtefactNode).RemoveHighLight();
                        }
                }
            }
            return base.OnEnterLeave(from, to, view);
        }

        public void AddHighLight()
        {
            //Pen = new Pen(new SolidBrush(Color.Orange), 2f);
        }
        public void RemoveHighLight()
        {
            //Pen = new Pen(new SolidBrush(Color.LightBlue), 2f);
        }
        public override GoObject CopyObject(GoCopyDictionary env)
        {
            return base.CopyObject(env);
        }
        public override void CopyObjectDelayed(GoCopyDictionary env, GoObject newobj)
        {
            base.CopyObjectDelayed(env, newobj);
        }

        #endregion Methods

        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;
            }
        }

        private string fromArtImageNode;
        public string FromArtImageNode
        {
            get { return fromArtImageNode; }
            set { fromArtImageNode = value; }
        }

    }

    [Serializable]
    public class FishRealLink : GoLink
    {
        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;
            }
        }

        #region Constructors (1)

        public FishRealLink()
        {
            Relinkable = false;
            //18 october 2013 - So it can be copied
            Selectable = true;
        }

        #endregion Constructors

        public override bool Deletable
        {
            get
            {
                return false;
            }
            set
            {
                base.Deletable = value;
            }
        }

        public override GoObject CopyObject(GoCopyDictionary env)
        {
            return base.CopyObject(env);
        }
        public override void CopyObjectDelayed(GoCopyDictionary env, GoObject newobj)
        {
            base.CopyObjectDelayed(env, newobj);
        }

        // don't show the relinking selection handle at the "from" end
        public override void AddSelectionHandles(GoSelection sel, GoObject selectedObj)
        {
            //     FishLink flink = Parent as FishLink;
            //  flink.AddSelectionHandles(sel, selectedObj);
            RemoveSelectionHandles(sel);

            try
            {
                base.AddSelectionHandles(sel, selectedObj);
                int handleCount = sel.GetHandleCount(this);
                if (handleCount == 2)
                {
                    IGoHandle h = sel.FindHandleByID(this, RelinkableFromHandle);
                    if (h != null)
                        h.GoObject.Visible = false;
                }
            }
            catch
            {
                Remove();
            }

            // sel.CreateResizeHandle(this, selectedObj, this.GetPoint(1), RelinkableToHandle, true);
        }
    }

    [Serializable]
    public class FishLinkPort : GoPort
    {
        public FishLinkPort()
        {
            Size = new SizeF(0, 0);
            Style = GoPortStyle.None; // the port has no visual appearance
            IsValidFrom = false; // can only draw links to this port, not from it
            ToSpot = NoSpot; // the connection point is computed dynamically
        }

        // customize the connection point to be the nearest point to the center
        // of the "from" port
        public override PointF GetToLinkPoint(IGoLink link)
        {
            if (link.FromPort != null)
                return NearestPoint(link.ToPort.GoObject.Center);
            return base.GetToLinkPoint(link);
        }
        // this is just the point on the line that is closest to the given point
        public PointF NearestPoint(PointF c)
        {
            GoStroke s = this.PortObject as GoStroke;
            if (s != null && s.PointsCount == 2)
            {
                PointF p;
                GoStroke.NearestPointOnLine(s.GetPoint(0), s.GetPoint(1), c, out p);
                return p;
            }
            else
            {
                return this.Center;
            }
        }
    }

    [Serializable]
    public class ArtefactNodeLinkKey
    {
        public ArtefactNodeLinkKey(ArtefactNode n, QLink l)
        {
            Node = n;
            NodesLink = l;
        }

        public ArtefactNode Node;
        public QLink NodesLink;
    }

}