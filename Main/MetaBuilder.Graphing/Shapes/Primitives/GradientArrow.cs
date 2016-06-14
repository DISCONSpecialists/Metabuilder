using System;
using System.Drawing;
using MetaBuilder.Graphing.Shapes.Behaviours;
using MetaBuilder.Graphing.Shapes.Behaviours.Internal;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Shapes.Primitives
{
    /// <summary>
    /// This shape implements a reshapable block arrow,
    /// where the user can change the thickness of the arrow,
    /// the length and width of the arrowhead, and the
    /// location of the ends of the arrow.
    /// </summary>
    /// <remarks>
    /// You must not change the number of points in this shape,
    /// nor set the Style to be GoPolygonStyle.Bezier.
    /// </remarks>
    [Serializable]
    public class GradientArrow : GoPolygon, ISnappableShape, IIdentifiable, IBehaviourShape
    {
        #region Convenience

        private const int ChangedCanonicalPoints = LastChangedHint + 100;
        public const int ArrowShapeID = 101;
        public const int ArrowWidthID = 102;
        public const int ArrowStartID = 103;
        public const int ArrowEndID = 104;

        private PointF[] myCanonicalPoints = new PointF[]
                                                 {
                                                     new PointF(-1.0f, 0.0f), // the StartPoint
                                                     new PointF(-1.0f, -0.1f),
                                                     new PointF(-0.3f, -0.1f),
                                                     new PointF(-0.3f, -0.2f),
                                                     new PointF(0.0f, 0.0f), // the EndPoint
                                                     new PointF(-0.3f, 0.2f),
                                                     new PointF(-0.3f, 0.1f),
                                                     new PointF(-1.0f, 0.1f)
                                                 };

        public GradientArrow()
        {
            // initialize to have a reasonable shape and size
            SetPoints(myCanonicalPoints);
            Size = new SizeF(100, 50);
            name = Guid.NewGuid().ToString();
            manager = new BaseShapeManager();
        }

        #region IIdentifiable Implementation

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        #endregion

        #region Behaviour Implementation

        private BaseShapeManager manager;

        public BaseShapeManager Manager
        {
            get
            {
                if (manager == null)
                    manager = new BaseShapeManager();
                return manager;
            }
            set { manager = value; }
        }

        protected override void OnObservedChanged(GoObject observed, int subhint, int oldI, object oldVal,
                                                  RectangleF oldRect, int newI, object newVal, RectangleF newRect)
        {
            Manager.OnObservedChanged(observed, subhint, oldI, oldVal, oldRect, newI, newVal, newRect, this);
            base.OnObservedChanged(observed, subhint, oldI, oldVal, oldRect, newI, newVal, newRect);
        }

        public override void Changed(int subhint, int oldI, object oldVal, RectangleF oldRect, int newI, object newVal,
                                     RectangleF newRect)
        {
            if (subhint == 1001 && Manager.Enabled)
                Manager.Changed(subhint, oldI, oldVal, oldRect, newI, newVal, newRect, this);
            base.Changed(subhint, oldI, oldVal, oldRect, newI, newVal, newRect);
        }

        #endregion

        public BaseInternalBehavior GradientBehaviour
        {
            get { return manager.GetExistingBehaviour(typeof (GradientBehaviour)) as BaseInternalBehavior; }
        }

        /// <summary>
        /// Gets and sets where the arrow starts from.
        /// </summary>
        public PointF StartPoint
        {
            get { return GetPoint(0); }
            set
            {
                SetPoint(0, value);
                ResetPoints(); // rotate and scale all the other points accordingly
            }
        }

        /// <summary>
        /// Gets and sets where the arrow points to.
        /// </summary>
        public PointF EndPoint
        {
            get { return GetPoint(4); }
            set
            {
                SetPoint(4, value);
                ResetPoints(); // rotate and scale all the other points accordingly
            }
        }

        private PointF[] ClonePoints()
        {
            return (PointF[]) myCanonicalPoints.Clone();
        }

        private void ResetPoints()
        {
            PointF[] v = ClonePoints();
            DecanonicalizePoints(v);
            v[0] = StartPoint;
            v[4] = EndPoint;
            SetPoints(v);
        }

        private void DecanonicalizePoints(PointF[] v)
        {
            PointF sp = StartPoint;
            PointF ep = EndPoint;
            float dx = ep.X - sp.X;
            float dy = ep.Y - sp.Y;
            float len = (float) Math.Sqrt(dx*dx + dy*dy);
            if (len > 0)
            {
                float cosine = dx/len;
                float sine = dy/len;
                for (int i = 0; i < v.Length; i++)
                {
                    PointF p = v[i];
                    PointF q = new PointF();
                    q.X = (cosine*p.X - sine*p.Y);
                    q.Y = (sine*p.X + cosine*p.Y);
                    q.X *= len;
                    q.Y *= len;
                    q.X += ep.X;
                    q.Y += ep.Y;
                    v[i] = q;
                }
            }
        }

        private PointF CanonicalizePoint(PointF p)
        {
            PointF sp = StartPoint;
            PointF ep = EndPoint;
            float dx = ep.X - sp.X;
            float dy = ep.Y - sp.Y;
            float len = (float) Math.Sqrt(dx*dx + dy*dy);
            PointF q = p;
            if (len > 0)
            {
                float cosine = dx/len;
                float sine = dy/len;
                p.X -= ep.X;
                p.Y -= ep.Y;
                p.X /= len;
                p.Y /= len;
                q.X = (cosine*p.X + sine*p.Y);
                q.Y = (-sine*p.X + cosine*p.Y);
            }
            return q;
        }

        /// <summary>
        /// Add special handles for reshaping the thickness of the arrow, the length of the arrowhead,
        /// and the width of the arrowhead (if Reshapable),
        /// and for repositioning either end of the arrow (if Resizable).
        /// </summary>
        /// <param name="sel"></param>
        /// <param name="selectedObj"></param>
        public override void AddSelectionHandles(GoSelection sel, GoObject selectedObj)
        {
            RemoveSelectionHandles(sel);
            IGoHandle handle;
            GoHandle goh;
            if (CanReshape())
            {
                handle = sel.CreateResizeHandle(this, selectedObj, GetPoint(2), ArrowShapeID, true);
                goh = handle.GoObject as GoHandle;
                if (goh != null)
                {
                    goh.Style = GoHandleStyle.Diamond;
                    goh.Brush = Brushes.Yellow;
                    RectangleF b = goh.Bounds;
                    b.Inflate(1, 1);
                    goh.Bounds = b;
                }
                handle = sel.CreateResizeHandle(this, selectedObj, GetPoint(3), ArrowWidthID, true);
                goh = handle.GoObject as GoHandle;
                if (goh != null)
                {
                    goh.Style = GoHandleStyle.Diamond;
                    goh.Brush = Brushes.Yellow;
                    RectangleF b = goh.Bounds;
                    b.Inflate(1, 1);
                    goh.Bounds = b;
                }
            }
            handle = sel.CreateResizeHandle(this, selectedObj, StartPoint, CanResize() ? ArrowStartID : NoHandle, true);
            goh = handle.GoObject as GoHandle;
            if (goh != null)
            {
                goh.Style = GoHandleStyle.Ellipse;
                if (CanResize()) goh.Brush = Brushes.Yellow;
            }
            handle = sel.CreateResizeHandle(this, selectedObj, EndPoint, CanResize() ? ArrowEndID : NoHandle, true);
            goh = handle.GoObject as GoHandle;
            if (goh != null)
            {
                goh.Style = GoHandleStyle.Ellipse;
                if (CanResize()) goh.Brush = Brushes.Yellow;
            }
        }

        private RectangleF CanonicalBounds()
        {
            float minX = Math.Min(myCanonicalPoints[0].X, myCanonicalPoints[4].X);
            float maxX = Math.Max(myCanonicalPoints[0].X, myCanonicalPoints[4].X);
            float minY = Math.Min(myCanonicalPoints[2].Y, myCanonicalPoints[3].Y);
            float maxY = Math.Max(myCanonicalPoints[5].Y, myCanonicalPoints[6].Y);
            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }

        public override void DoResize(GoView view, RectangleF origRect, PointF newPoint, int whichHandle,
                                      GoInputState evttype, SizeF min, SizeF max)
        {
            if (whichHandle == ArrowShapeID)
            {
                // do all calculations in canonical space, where the StartPoint is (-1,0) and the EndPoint is (0,0)
                PointF p = CanonicalizePoint(newPoint);
                RectangleF b = CanonicalBounds();
                float midY = b.Y + b.Height/2;
                // limit P to one side of the arrow
                PointF sp = myCanonicalPoints[0];
                PointF ep = myCanonicalPoints[4];
                float minX = Math.Min(sp.X, ep.X);
                float maxX = Math.Max(sp.X, ep.X);
                p.X = Math.Max(p.X, minX);
                p.X = Math.Min(p.X, maxX);
                p.Y = Math.Min(p.Y, midY);
                float mirrorY = midY + (midY - p.Y);
                // save the canonical points array, for undo/redo
                PointF[] oldCanonicalPoints = ClonePoints();
                PointF q;
                q = myCanonicalPoints[1];
                myCanonicalPoints[1] = new PointF(q.X, p.Y);
                myCanonicalPoints[2] = p;
                myCanonicalPoints[6] = new PointF(p.X, mirrorY);
                q = myCanonicalPoints[7];
                myCanonicalPoints[7] = new PointF(q.X, mirrorY);
                Changed(ChangedCanonicalPoints, 0, oldCanonicalPoints, NullRect, 0, ClonePoints(), NullRect);
                ResetPoints(); // set all of the actual polygon points
            }
            else if (whichHandle == ArrowWidthID)
            {
                // do all calculations in canonical space, where the StartPoint is (-1,0) and the EndPoint is (0,0)
                PointF p = CanonicalizePoint(newPoint);
                RectangleF b = CanonicalBounds();
                float midY = b.Y + b.Height/2;
                // limit P to one side of the arrow
                PointF sp = myCanonicalPoints[0];
                PointF ep = myCanonicalPoints[4];
                float minX = Math.Min(sp.X, ep.X);
                float maxX = Math.Max(sp.X, ep.X);
                p.X = Math.Max(p.X, minX);
                p.X = Math.Min(p.X, maxX);
                p.Y = Math.Min(p.Y, midY);
                float mirrorY = midY + (midY - p.Y);
                // save the canonical points array, for undo/redo
                PointF[] oldCanonicalPoints = ClonePoints();
                myCanonicalPoints[3] = p;
                
                myCanonicalPoints[5] = new PointF(p.X, mirrorY);
                Changed(ChangedCanonicalPoints, 0, oldCanonicalPoints, NullRect, 0, ClonePoints(), NullRect);
                ResetPoints(); // set all of the actual polygon points
            }
            else if (whichHandle == ArrowStartID)
            {
                if (newPoint != EndPoint)
                {
                    StartPoint = newPoint;
                }
            }
            else if (whichHandle == ArrowEndID)
            {
                if (newPoint != StartPoint)
                {
                    EndPoint = newPoint;
                }
            }
            else
            {
                base.DoResize(view, origRect, newPoint, whichHandle, evttype, min, max);
            }
        }

        public override void ChangeValue(GoChangedEventArgs e, bool undo)
        {
            if (e.SubHint == ChangedCanonicalPoints)
            {
                Array.Copy((PointF[]) e.GetValue(undo), myCanonicalPoints, myCanonicalPoints.Length);
            }
            else
            {
                base.ChangeValue(e, undo);
            }
        }

        public override GoObject CopyObject(GoCopyDictionary env)
        {
            env.Delayeds.Add(this);
            GradientArrow newobj = (GradientArrow) base.CopyObject(env);
            newobj.myCanonicalPoints = ClonePoints();
            return newobj;
        }

        public override void CopyObjectDelayed(GoCopyDictionary env, GoObject newobj)
        {
            GradientArrow arrow = newobj as GradientArrow;
            arrow.Name = Name.Substring(0, Name.Length);
            if (Manager != null)
                arrow.Manager = Manager.Copy(arrow);
            base.CopyObjectDelayed(env, newobj);
        }
    }

    /// <summary>
    /// Here's one way to use a BlockArrow as a link.
    /// </summary>
    /// <remarks>
    /// Another way would be to implement IGoLink, but that would be more work.
    /// </remarks>
    [Serializable]
    public class ArrowLink : QLink
    {
        public ArrowLink()
        {
            Pen = null;
            Brush = null;
            GradientArrow arr = new GradientArrow();
            arr.Selectable = false;
            arr.Resizable = false;
            arr.Reshapable = true; // let users reshape the arrow
            arr.Brush = Brushes.LightSalmon;
            MidLabel = arr;
        }

        // make sure the start and end points coincide with the link's first and last points

        // so that users can reshape the BlockArrow, let the BlockArrow
        // get selection handles; but users won't be able to relink
        public override GoObject SelectionObject
        {
            get { return MidLabel; }
        }

        protected override void LayoutMidLabel(GoObject childchanged)
        {
            GradientArrow arr = MidLabel as GradientArrow;
            GoLink l = RealLink;
            if (arr != null && l.PointsCount >= 2)
            {
                arr.StartPoint = l.GetPoint(0);
                arr.EndPoint = l.GetPoint(l.PointsCount - 1);
            }
        }
    }

    #endregion
}