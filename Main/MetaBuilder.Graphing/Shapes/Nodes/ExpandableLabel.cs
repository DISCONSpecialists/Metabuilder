using System;
using System.Collections.Generic;
using System.Drawing;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Shapes.Nodes
{
    [Serializable]
    public class ExpandableLabel : CollapsingRecordNodeItem, IGoCollapsible
    {
        public override RectangleF Bounds
        {
            get { return base.Bounds; }
            set { base.Bounds = new RectangleF(value.X, value.Y, ITEMWIDTH, value.Height); }
        }

        #region Fields (4)

        private const float ITEMWIDTH = 169;
        //[NonSerialized]
        //private bool isSelected;
        //[NonSerialized]
        //private Color selColor;

        #endregion Fields

        #region Constructors (1)

        public ExpandableLabel()
        {
            Width = ITEMWIDTH;
            Background.Width = ITEMWIDTH;
            Background.Position = new PointF(Background.Position.X + 5, Background.Position.Y);
            CreatePort(MiddleRight);
            CreateBody();

            //DragsNode = true; //disable drag/drop

            //Movable = true;
            Init(null, "", false);
            /*  GoRectangle rect = new GoRectangle();
            rect.Width = 160;
            rect.Height = 18;
            rect.Pen = Pens.Transparent;*/
            //this.Brush = Brushes.White;
            /*Background.Height = this[2].Height;
            Background.Width = this[1].Width;*/

            Background.Visible = false;
            Pen = new Pen(Color.Transparent);
            Selectable = true;
            Resizable = false;
            Reshapable = false;
            Deletable = true;
            if (Label != null)
                Label.Visible = false;
        }

        #endregion Constructors

        #region Properties (2)

        public bool Collapsible
        {
            get
            {
                GoObject o = FindChild("DomainDef");
                if (o != null)
                    return o.Visible;
                return false;
            }
            set
            {
                // do nothign
            }
        }

        public bool IsExpanded
        {
            get
            {
                GoObject o = FindChild("DomainDef");

                if (o != null)
                    return o.Visible;
                return false;
            }
        }

        //public BoundLabel GetLabel
        //{
        //    get
        //    {
        //        foreach (GoObject o in this)
        //        {
        //            if (o is BoundLabel)
        //            {
        //                if ((o as BoundLabel).Name == "Name")
        //                    return o as BoundLabel;
        //            }
        //        }
        //        return null;
        //    }
        //    set
        //    {
        //        foreach (GoObject o in this)
        //        {
        //            if (o is BoundLabel && (o as BoundLabel).Name == "Name")
        //            {
        //                BoundLabel bndlbl = o as BoundLabel;
        //                bndlbl.FamilyName = value.FamilyName;
        //                bndlbl.FontSize = value.FontSize;
        //                bndlbl.Bold = value.Bold;
        //                bndlbl.Italic = value.Italic;
        //                bndlbl.StrikeThrough = value.StrikeThrough;
        //                bndlbl.Underline = value.Underline;
        //                bndlbl.TextColor = value.TextColor;
        //                break;
        //            }
        //        }
        //    }
        //}

        #endregion Properties

        #region Methods (9)

        // Public Methods (6) 

        public void Collapse()
        {
            BoundLabel lbl = FindChild("DomainDef") as BoundLabel;
            lbl.Visible = false;
            lbl.Printable = false;
        }

        public void Expand()
        {
            BoundLabel lbl = FindChild("DomainDef") as BoundLabel;
            lbl.Visible = true;
            lbl.Printable = true;
            //LayoutChildren(this[3]);
            //ComputeBounds();
        }

        public override void LayoutChildren(GoObject childchanged)
        {
            if (Background != null)
            {
                //this.Brush = Brushes.White;
                Background.Visible = true;
            }

            base.LayoutChildren(childchanged);


            ComputeBounds();
        }

        public override void RemoveSelectionHandles(GoSelection sel)
        {
            //isSelected = false;
            base.RemoveSelectionHandles(sel);
        }

        // Protected Methods (2) 

        protected override RectangleF ComputeBounds()
        {
            if (Collapsible)
            {
                GoObject o = FindChild("DomainDef");
                if (o != null)
                {
                    return RectangleF.Union(base.ComputeBounds(), o.Bounds);
                }
            }
            else
            {
                GoObject o = FindChild("Name");
                if (o != null)
                {
                    return RectangleF.Union(Background.Bounds, o.Bounds);
                }
            }
            return base.ComputeBounds();
        }

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
            }
            base.OnChildBoundsChanged(child, old);
        }

        // Private Methods (1) 

        private void CreateBody()
        {
            List<GoObject> objsToRemove = new List<GoObject>();
            foreach (GoObject o in this)
            {
                if (o is GoText)
                {
                    objsToRemove.Add(o);
                }
            }
            for (int i = 0; i < objsToRemove.Count; i++)
            {
                Remove(objsToRemove[i]);
            }
            BoundLabel txt1 = new BoundLabel();

            txt1.Wrapping = true;
            txt1.WrappingWidth = 158;
            txt1.Text = "DataField Name";
            txt1.AutoResizes = false;
            txt1.Width = 160;
            txt1.DragsNode = true;
            txt1.Movable = false;
            txt1.Position = new PointF(4, 4);
            txt1.Clipping = false;
            txt1.TextColor = Color.Black;
            txt1.FontSize = 8f;
            BoundLabel txt2 = new BoundLabel();

            txt2.Text = "";// "Domain Definition";
            txt1.Clipping = false;
            Add(txt1);

            txt1.Editable = true;

            Add(txt2);
            InsertAfter(Background, txt1);
            InsertAfter(Background, txt2);

            txt2.AutoResizes = false;
            txt2.Width = 160;
            txt2.DragsNode = true;
            txt2.TextColor = Color.Gray;
            txt2.Position = new PointF(txt1.Right + 20, txt1.Top);
            txt2.Editable = true;
            txt2.Movable = false;
            txt2.Name = "DomainDef";
            txt2.Visible = false;
            txt2.Wrapping = true;
            txt2.FontSize = 8f;
            txt2.WrappingWidth = 158;
            txt1.Name = "Name";
            txt1.Selectable = false;
            txt1.Deletable = false;
            txt2.Selectable = false;
            txt2.Deletable = false;
            txt1.Alignment = 2;
            txt2.Alignment = 2;
            txt1.Resizable = false;
            txt2.Resizable = false;
            Resizable = false;
            BindingInfo = new BindingInfo();
            BindingInfo.BindingClass = "DataField";
            BindingInfo.Bindings.Add("Name", "Name");
            BindingInfo.Bindings.Add("DomainDef", "DataFieldRole");
            CreateMetaObject(null, EventArgs.Empty);
            //this[0].Remove();
            OnContentsChanged(this, EventArgs.Empty);
            PickableBackground = false;
            Resizable = false; // may be set to true
            Deletable = true;
            Selectable = true;
            HookupEvents();
            txt1.AddObserver(this);
            txt2.AddObserver(this);
            AddChildName("Name", txt1);
            AddChildName("DomainDef", txt2);

            Collapse();
        }

        #endregion Methods

        // don't need the top and bottom ports, so don't create them!
        /*  protected override GoPort CreatePort(int spot)
        {
            if (spot == GoObject.TopCenter || spot == GoObject.BottomCenter)
                return null;

            if (spot == GoObject.MiddleRight)
            {
                GoPort prtRight = base.CreatePort(spot);
                
                prtRight.Position = new PointF(this.Position.X + 140,prtRight.Position.Y);
                return prtRight;
            }
            GoPort prt = base.CreatePort(spot);
            //prt.Width = Label.Width + 10;
            //prt.Position = new PointF(Label.Position.X - 5, Label.Position.Y);
            return prt;
        }*/
    }
}