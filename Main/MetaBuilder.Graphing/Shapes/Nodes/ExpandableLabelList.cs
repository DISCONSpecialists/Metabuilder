using System;
using System.Drawing;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Shapes.Nodes
{
    [Serializable]
    public class ExpandableLabelList : RepeaterSection
    {
        #region Fields (1)

        private bool collapsible;

        #endregion Fields

        #region Constructors (1)

        public ExpandableLabelList()
        {
            AddHeader();
            //Movable = true;
            Collapse();
        }

        #endregion Constructors

        #region Properties (6)

        private new GoCollapsibleHandle Handle
        {
            get { return Header[2] as GoCollapsibleHandle; }

        }

        private new GoGroup Header
        {
            get { return this[0] as GoGroup; }
        }

        public GoText HeaderText
        {
            get { return Header[1] as GoText; }
            set { Header[1] = value; }
        }

        public Color TextColor
        {
            get { return Color.Black; }
            set
            {
                // Label.TextColor = value;
            }
        }

        public override bool Collapsible
        {
            get { return collapsible; }
            set
            {
                collapsible = value; // do nothing
            }
        }

        public override bool IsExpanded
        {
            get { return collapsible; }
        }

        #endregion Properties

        #region Methods (4)

        // Public Methods (4) 

        public override void Collapse()
        {
            //   if (Parent != null)
            {
                //   CollapsingRecordNodeItemList list = ParentList;
                collapsible = false;
                foreach (GoObject o in this)
                {
                    if (o is IGoCollapsible && !(o is ExpandableLabelList))
                    {
                        IGoCollapsible igocol = o as IGoCollapsible;
                        igocol.Collapse();
                    }
                }
            }
            LayoutChildren(null);
            Handle.InvalidateViews();
        }

        public override void Expand()
        {
            //    if (Parent != null)
            {
                //CollapsingRecordNodeItemList list = ParentList;
                collapsible = true;
                foreach (GoObject o in this)
                {
                    //// Console.WriteLine(o.GetType().ToString());
                    if (o is IGoCollapsible && !(o is ExpandableLabelList))
                    {
                        IGoCollapsible igocol = o as IGoCollapsible;
                        igocol.Expand();
                    }
                }
            }
            LayoutChildren(null);
            Handle.InvalidateViews();
        }

        public void AddHeader()
        {
            Clear();
            GoGroup grp = new GoGroup();
            GoRoundedRectangle rect = new GoRoundedRectangle();
            rect.Corner = new SizeF(1, 1);
            rect.Selectable = false;
            rect.Deletable = false;
            rect.DragsNode = true;
            rect.Brush = Brushes.Beige;//new SolidBrush(Color.Beige);
            grp.Add(rect);
            collapsible = false;
            GoCollapsibleHandle handle = new GoCollapsibleHandle();
            handle.Style = GoCollapsibleHandleStyle.PlusMinus;
            handle.Size = new SizeF(10, 10);
            handle.Printable = false;
            grp.Selectable = false;
            GoText txtHeader = new GoText();

            txtHeader.Deletable = false;
            grp.Deletable = false;
            txtHeader.Text = "DataColumns";
            txtHeader.Editable = false;
            txtHeader.BackgroundOpaqueWhenSelected = false;
            txtHeader.Bold = false;
            txtHeader.AutoResizes = false;
            txtHeader.Height = 18;
            grp.Add(txtHeader);
            txtHeader.Width = 169.7f;
            txtHeader.Selectable = false;
            txtHeader.Resizable = false;
            txtHeader.DragsNode = true;
            txtHeader.Position = new PointF(4, 1);
            handle.Position = new PointF(txtHeader.Right - handle.Width, txtHeader.Top);
            handle.Selectable = false;
            handle.Style = GoCollapsibleHandleStyle.PlusMinus;
            handle.Brush = null;
            handle.Deletable = false;
            grp.Add(handle);
            rect.Bounds = txtHeader.Bounds;
            Add(grp);
        }

        public override GoObject AddItemFromCode()
        {
            ExpandableLabel slabel = new ExpandableLabel();
            slabel.Init("", slabel.MetaObject.ToString(), false);
            Add(slabel);
            slabel.First.Width = Header.Width;
            slabel.Selectable = true;
            slabel.Deletable = true;

            if (IsExpanded)
                slabel.Expand();
            else
                slabel.Collapse();
            return slabel;
        }

        #endregion Methods
    }
}