using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;
using System.Drawing;
namespace ShapeBuilding
{
    [Serializable]
    public class CustomSubGraph : GoSubGraph
    {
        public CustomSubGraph()
        {
            this.Shadowed = true;
            this.BorderPen = Pens.Red;
            // wide margin and large corners
            this.Corner = new SizeF(40, 40);
            this.TopLeftMargin = new SizeF(20, 20);
            this.BottomRightMargin = new SizeF(20, 20);
            this.Movable = true;
            
        }

        // create a boxport covering the whole subgraph
        protected override GoPort CreatePort()
        {
            return null; 

            GoPort prt = new GoPort();
            
            return null;
            return new CustomSubGraphPort();
        }

        // If you want to display an image when collapsed,
        // add this override (or set CollapsedObject)
        /*
    protected override GoObject CreateCollapsedObject() {
            GoImage img = new GoImage();
          img.Selectable = false;
             img.Visible = false;
           img.Printable = false;
           img.Name = @"E:\Documents and Settings\Deon Fourie\My Documents\My Pictures\DISCON Spitbraai\DISCON Spitbraai 068.jpg";
           img.Size = new SizeF(img.Image.Width / 13, img.Image.Height / 13);
          this.CollapsedSize = img.Size;
     return img;
     }*/

        // CustomSubGraphPort has same Bounds as whole subgraph
        public override void LayoutPort()
        {
            GoPort p = this.Port;
            if (p != null && p.CanView())
            {
                RectangleF r = ComputeBorder();
                p.Bounds = r;
            }
        }


        // If you always want to position the Handle in the very top-left
        // corner of the subgraph, ignoring the Margin, override LayoutHandle
        // and call it from DoResize as follows:

        //    public override void LayoutHandle() {
        //      GoSubGraphHandle h = this.Handle;
        //      if (h != null && h.CanView()) {
        //        // keep handle up-to-date
        //        RectangleF b = ComputeBorder();
        //        h.Position = b.Location;
        //      }
        //    }

        public override void DoResize(GoView view, RectangleF origRect, PointF newPoint,
                                      int whichHandle, GoInputState evttype, SizeF min, SizeF max)
        {
            base.DoResize(view, origRect, newPoint, whichHandle, evttype, min, max);
            LayoutPort();
            //      if (this.IsExpanded) {
            //        LayoutHandle();
            //      }
        }


        // making collapsed labels shorter
        protected override void PrepareCollapse()
        {
            base.PrepareCollapse();
            if (this.Label != null)
            {
                mySavedLabelWidth = this.Label.Width;
                mySavedLabelWrappingWidth = this.Label.WrappingWidth;
                this.Label.Wrapping = false;
                this.Label.StringTrimming = StringTrimming.EllipsisCharacter;
                this.Label.Width = this.CollapsedSize.Width;
                this.Label.AutoResizes = false;
                this.Label.TextColor = Color.White;
                this.Label.Text = "Uncle Rob";
            }
        }

        // If you want the label to disappear when the subgraph is collapsed:

        //    protected override void FinishCollapse(RectangleF sgrect) {
        //      base.FinishCollapse(sgrect);
        //      this.Label.Visible = false;
        //      this.Label.Printable = false;
        //    }

        protected override void FinishExpand(PointF hpos)
        {
            base.FinishExpand(hpos);
            if (this.Label != null)
            {
                this.Label.AutoResizes = true;
                this.Label.Wrapping = true;
                this.Label.StringTrimming = StringTrimming.None;
                if (mySavedLabelWidth > -1)
                {
                    this.Label.Width = mySavedLabelWidth;
                    this.Label.WrappingWidth = mySavedLabelWrappingWidth;
                }
                this.Label.Visible = true;
                this.Label.Printable = true;
            }
        }

        // making collapsed nodes a fixed size
        public override SizeF ComputeCollapsedSize(bool visible)
        {
            SizeF sz = this.CollapsedSize;
            if (sz.Width > 10 && sz.Height > 10)
                return sz;
            else
                return base.ComputeCollapsedSize(visible);
        }

        // a Width or Height of ten or less results in the standard behavior:
        // the collapsed size is the smallest rectangle to hold all child nodes
        public SizeF CollapsedSize
        {
            get { return myCollapsedSize; }
            set
            {
                SizeF old = myCollapsedSize;
                if (old != value)
                {
                    myCollapsedSize = value;
                    Changed(ChangedCollapsedSize, 0, null, MakeRect(old), 0, null, MakeRect(value));
                    // doesn't actually change size if already collapsed
                }
            }
        }

        public override void ChangeValue(GoChangedEventArgs e, bool undo)
        {
            switch (e.SubHint)
            {
                case ChangedCollapsedSize:
                    this.CollapsedSize = e.GetSize(undo);
                    return;
                default:
                    base.ChangeValue(e, undo);
                    return;
            }
        }

        public const int ChangedCollapsedSize = GoObject.LastChangedHint + 2789;

        private float mySavedLabelWidth = -1;
        private float mySavedLabelWrappingWidth = -1;
        private SizeF myCollapsedSize = new SizeF(60, 30);  // for text Label, not for a CollapsedObject that is an Image
    }


    [Serializable]
    public class CustomSubGraphPort : GoBoxPort
    {
        public CustomSubGraphPort()
        {
            this.IsValidSelfNode = true;
            this.Style = GoPortStyle.None;
            this.LinkPointsSpread = true;
        }

        // links within the subgraph connect to the CustomSubGraphPort/GoBoxPort from the inside
        public override float GetFromLinkDir(IGoLink link)
        {
            float result = base.GetFromLinkDir(link);
            if (link.ToPort != null &&
              link.ToPort.GoObject != null &&
              link.ToPort.GoObject.IsChildOf(this.Parent))
            {
                result += 180;
                if (result > 360)
                    result -= 360;
            }
            return result;
        }

        // links within the subgraph connect to the CustomSubGraphPort/GoBoxPort from the inside
        public override float GetToLinkDir(IGoLink link)
        {
            float result = base.GetToLinkDir(link);
            if (link.FromPort != null &&
              link.FromPort.GoObject != null &&
              link.FromPort.GoObject.IsChildOf(this.Parent))
            {
                result += 180;
                if (result > 360)
                    result -= 360;
            }
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
            else
            {
                return base.GetToLinkDir(link);
            }
        }

        // assume this kind of port is "hollow", as specified by the half the parent's margin
        public override bool ContainsPoint(PointF p)
        {
            RectangleF rect = this.Bounds;
            if (!rect.Contains(p))
                return false;
            GoSubGraph sg = this.Parent as GoSubGraph;
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
            else
            {
                return true;
            }
        }
    }
}
