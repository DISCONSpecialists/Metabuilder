/*
 *  Copyright © Northwoods Software Corporation, 1998-2005. All Rights
 *  Reserved.
 *
 *  Restricted Rights: Use, duplication, or disclosure by the U.S.
 *  Government is subject to restrictions as set forth in subparagraph
 *  (c) (1) (ii) of DFARS 252.227-7013, or in FAR 52.227-19, or in FAR
 *  52.227-14 Alt. III, as applicable.
 */

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Shapes
{
    [Serializable]
    public class RotatedText : BoundLabel
    {
        #region Fields (2)

        public const int ChangedAngle = 1550;
        private float myAngle;

        #endregion Fields

        #region Constructors (1)

        public RotatedText()
        {
            Alignment = Middle;
        }

        #endregion Constructors

        #region Properties (1)

        [Category("Appearance"), DefaultValue(0)]
        public float Angle
        {
            get
            {
                GoLabeledLink ll = Parent as GoLabeledLink;
                if (ll != null && ll.MidLabel == this)
                {
                    GoLink rl = ll.RealLink;
                    int midEnd = rl.PointsCount / 2;
                    if (midEnd < 1) return myAngle;
                    PointF a = rl.GetPoint(midEnd - 1);
                    PointF b = rl.GetPoint(midEnd);
                    float angle = GoStroke.GetAngle(b.X - a.X, b.Y - a.Y);
                    if (angle > 90 && angle < 270)
                        angle -= 180;
                    return angle;
                }
                return myAngle;
            }
            set
            {
                float old = myAngle;
                if (old != value)
                {
                    myAngle = value;
                    Changed(ChangedAngle, 0, null, MakeRect(old), 0, null, MakeRect(value));
                }
            }
        }

        #endregion Properties

        #region Methods (10)

        // Public Methods (10) 

        public override void AddSelectionHandles(GoSelection sel, GoObject selectedObject)
        {
            RemoveSelectionHandles(sel);

            if (!CanResize())
            {
                sel.CreateBoundingHandle(this, selectedObject);
                return;
            }

            RectangleF rect = GetRealBounds();

            float x1 = rect.X;
            float x2 = rect.X + (rect.Width / 2);
            float x3 = rect.X + rect.Width;

            float y1 = rect.Y;
            float y2 = rect.Y + (rect.Height / 2);
            float y3 = rect.Y + rect.Height;

            // create the handles
            sel.CreateResizeHandle(this, selectedObject, new PointF(x1, y1), TopLeft, true);
            sel.CreateResizeHandle(this, selectedObject, new PointF(x3, y1), TopRight, true);
            sel.CreateResizeHandle(this, selectedObject, new PointF(x3, y3), BottomRight, true);
            sel.CreateResizeHandle(this, selectedObject, new PointF(x1, y3), BottomLeft, true);
            if (CanReshape())
            {
                sel.CreateResizeHandle(this, selectedObject, new PointF(x2, y1), MiddleTop, true);
                sel.CreateResizeHandle(this, selectedObject, new PointF(x3, y2), MiddleRight, true);
                sel.CreateResizeHandle(this, selectedObject, new PointF(x2, y3), MiddleBottom, true);
                sel.CreateResizeHandle(this, selectedObject, new PointF(x1, y2), MiddleLeft, true);
            }
        }

        public override void ChangeValue(GoChangedEventArgs e, bool undo)
        {
            switch (e.SubHint)
            {
                case ChangedAngle:
                    Angle = e.GetFloat(undo);
                    return;
                default:
                    base.ChangeValue(e, undo);
                    return;
            }
        }

        public override bool ContainedByRectangle(RectangleF r)
        {
            RectangleF b = GetRealBounds();
            return (
                       r.Width > 0 &&
                       r.Height > 0 &&
                       b.Width >= 0 &&
                       b.Height >= 0 &&
                       b.X >= r.X &&
                       b.Y >= r.Y &&
                       b.X + b.Width <= r.X + r.Width &&
                       b.Y + b.Height <= r.Y + r.Height);
        }

        public override bool ContainsPoint(PointF p)
        {
            return GetRealBounds().Contains(p);
        }

        public override IGoHandle CreateBoundingHandle()
        {
            GoHandle h = new GoHandle();
            RectangleF rect = GetRealBounds();
            // the handle rectangle should just go around the object
            rect.X--;
            rect.Y--;
            rect.Height += 2;
            rect.Width += 2;
            h.Bounds = rect;
            return h;
        }

        public override RectangleF ExpandPaintBounds(RectangleF rect, GoView view)
        {
            RectangleF b = GetRealBounds();
            return RectangleF.Union(rect, b);
        }

        public RectangleF GetRealBounds()
        {
            RectangleF bounds = Bounds;
            double angle = Angle / 180 * Math.PI;
            PointF origin = Location;
            return RotateRectangle(bounds, origin, angle);
        }

        public override void Paint(Graphics g, GoView view)
        {
            GraphicsState before = g.Save();
            PointF c = Location;
            g.TranslateTransform(c.X, c.Y);
            g.RotateTransform(Angle);
            g.TranslateTransform(-c.X, -c.Y);
            base.Paint(g, view);
            g.Restore(before);
        }

        public static PointF RotatePoint(PointF p, PointF origin, double angle)
        {
            float dx = p.X - origin.X;
            float dy = p.Y - origin.Y;
            double pangle = GoStroke.GetAngle(dx, dy) / 180 * Math.PI;
            double dist = Math.Sqrt(dx * dx + dy * dy);
            float nx = (float)(dist * Math.Cos(pangle + angle));
            float ny = (float)(dist * Math.Sin(pangle + angle));
            return new PointF(origin.X + nx, origin.Y + ny);
        }

        public static RectangleF RotateRectangle(RectangleF r, PointF origin, double angle)
        {
            PointF otl = new PointF(r.X, r.Y);
            PointF ntl = otl;
            if (otl != origin)
                ntl = RotatePoint(otl, origin, angle);

            PointF otr = new PointF(r.X + r.Width, r.Y);
            PointF ntr = otr;
            if (otr != origin)
                ntr = RotatePoint(otr, origin, angle);

            PointF obr = new PointF(r.X + r.Width, r.Y + r.Height);
            PointF nbr = obr;
            if (obr != origin)
                nbr = RotatePoint(obr, origin, angle);

            PointF obl = new PointF(r.X, r.Y + r.Height);
            PointF nbl = obl;
            if (obl != origin)
                nbl = RotatePoint(obl, origin, angle);

            float minx = Math.Min(ntl.X, Math.Min(ntr.X, Math.Min(nbr.X, nbl.X)));
            float maxx = Math.Max(ntl.X, Math.Max(ntr.X, Math.Max(nbr.X, nbl.X)));
            float miny = Math.Min(ntl.Y, Math.Min(ntr.Y, Math.Min(nbr.Y, nbl.Y)));
            float maxy = Math.Max(ntl.Y, Math.Max(ntr.Y, Math.Max(nbr.Y, nbl.Y)));

            return new RectangleF(minx, miny, maxx - minx, maxy - miny);
        }

        #endregion Methods
    }

    // A RotatedText's Bounds aren't the "real" bounds.
    // This class implements an object that contains a RotatedText,
    // and whose Bounds will be equal to the RotatedText's GetRealBounds(),
    // The initial RotatedText object will also be not Selectable and
    // will have a Middle Alignment.
    // You can then use an instance of this class inside other groups, such as GoListGroups.
    // You can access the RotatedText object through the RT property.
    [Serializable]
    public class RotatedTextHolder : GoGroup
    {
        #region Constructors (1)

        public RotatedTextHolder()
        {
            Selectable = false;
            RotatedText myRotatedText = new RotatedText();
            myRotatedText.Selectable = false;
            myRotatedText.Alignment = Middle;
            myRotatedText.AddObserver(this); // notice whenever the RotatedText's Bounds changes
            Add(myRotatedText);
        }

        #endregion Constructors

        #region Properties (1)

        public RotatedText RT
        {
            get { return (RotatedText)this[0]; }
        }

        #endregion Properties

        #region Methods (2)

        // Protected Methods (2) 

        protected override RectangleF ComputeBounds()
        {
            return RT.GetRealBounds();
        }

        protected override void OnObservedChanged(GoObject observed, int subhint, int oldI, object oldVal, RectangleF oldRect, int newI, object newVal, RectangleF newRect)
        {
            if (subhint == ChangedBounds)
            {
                InvalidBounds = true;
            }
        }

        #endregion Methods
    }
}