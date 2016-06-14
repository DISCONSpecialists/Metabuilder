using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using MetaBuilder.Graphing.Shapes.Behaviours;
using MetaBuilder.Graphing.Shapes.Behaviours.Internal;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Shapes
{
    [Serializable]
    public class HouseShape : GoShape, IIdentifiable, IBehaviourShape
    {
        public const int ChangedDirection = LastChangedHint + 13103;
        // HouseShape state
        private int myDirection = MiddleRight;

        public HouseShape()
        {
            ResizesRealtime = true;
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

        #region IBehaviourShape Implementation

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
        /// Gets or sets an object spot indicating the direction that the shape is pointing toward.
        /// </summary>
        /// <value>
        /// By default this only supports <c>GoObject.MiddleTop</c>, <c>GoObject.MiddleBottom</c>,
        /// <c>GoObject.MiddleLeft</c>, and the default: <c>GoObject.MiddleRight</c>.
        /// </value>
        public virtual int Direction
        {
            get { return myDirection; }
            set
            {
                int old = myDirection;
                if (old != value)
                {
                    myDirection = value;
                    ResetPath();
                    Changed(ChangedDirection, old, null, NullRect, value, null, NullRect);
                }
            }
        }

        public override void Paint(Graphics g, GoView view)
        {
            GraphicsPath path = GetPath();
            if (Shadowed)
            {
                SizeF shadowsize = GetShadowOffset(view);
                g.TranslateTransform(shadowsize.Width, shadowsize.Height);
                if (Brush != null)
                {
                    g.FillPath(GetShadowBrush(view), path);
                }
                if (Pen != null)
                {
                    g.DrawPath(GetShadowPen(view, Pen.Width), path);
                }
                g.TranslateTransform(-shadowsize.Width, -shadowsize.Height);
            }
            if (Brush != null)
            {
                g.FillPath(Brush, path);
            }
            if (Pen != null)
            {
                g.DrawPath(Pen, path);
            }
        }

        public override GraphicsPath MakePath()
        {
            GraphicsPath path = new GraphicsPath(FillMode.Winding);
            RectangleF r = Bounds;
            if (r.Width < 1 || r.Height < 1)
            {
                path.AddRectangle(r);
            }
            else if (Direction == MiddleTop)
            {
                path.AddLine(r.X, r.Y + r.Height, r.X, r.Y + r.Height/2);
                path.AddLine(r.X, r.Y + r.Height/2, r.X + r.Width/2, r.Y);
                path.AddLine(r.X + r.Width/2, r.Y, r.X + r.Width, r.Y + r.Height/2);
                path.AddLine(r.X + r.Width, r.Y + r.Height/2, r.X + r.Width, r.Y + r.Height);
            }
            else if (Direction == MiddleBottom)
            {
                path.AddLine(r.X + r.Width, r.Y, r.X + r.Width, r.Y + r.Height/2);
                path.AddLine(r.X + r.Width, r.Y + r.Height/2, r.X + r.Width/2, r.Y + r.Height);
                path.AddLine(r.X + r.Width/2, r.Y + r.Height, r.X, r.Y + r.Height/2);
                path.AddLine(r.X, r.Y + r.Height/2, r.X, r.Y);
            }
            else if (Direction == MiddleLeft)
            {
                path.AddLine(r.X + r.Width, r.Y + r.Height, r.X + r.Width/2, r.Y + r.Height);
                path.AddLine(r.X + r.Width/2, r.Y + r.Height, r.X, r.Y + r.Height/2);
                path.AddLine(r.X, r.Y + r.Height/2, r.X + r.Width/2, r.Y);
                path.AddLine(r.X + r.Width/2, r.Y, r.X + r.Width, r.Y);
            }
            else
            {
                // default, MiddleRight
                path.AddLine(r.X, r.Y, r.X + r.Width/2, r.Y);
                path.AddLine(r.X + r.Width/2, r.Y, r.X + r.Width, r.Y + r.Height/2);
                path.AddLine(r.X + r.Width, r.Y + r.Height/2, r.X + r.Width/2, r.Y + r.Height);
                path.AddLine(r.X + r.Width/2, r.Y + r.Height, r.X, r.Y + r.Height);
            }
            path.CloseAllFigures();
            return path;
        }

        public override bool ContainsPoint(PointF p)
        {
            if (!base.ContainsPoint(p))
                return false;
            GraphicsPath path = GetPath(); // does not account for Pen Width
            bool result = path.IsVisible(p);
            return result;
        }

        public override void ChangeValue(GoChangedEventArgs e, bool undo)
        {
            switch (e.SubHint)
            {
                case ChangedDirection:
                    Direction = e.GetInt(undo);
                    return;
                default:
                    base.ChangeValue(e, undo);
                    return;
            }
        }

        public override GoObject CopyObject(GoCopyDictionary env)
        {
            env.Delayeds.Add(this);
            return base.CopyObject(env);
        }

        public override void CopyObjectDelayed(GoCopyDictionary env, GoObject newobj)
        {
            base.CopyObjectDelayed(env, newobj);
            HouseShape house = newobj as HouseShape;
            house.Name = Name.Substring(0, Name.Length);
            if (Manager != null)
                house.Manager = Manager.Copy(house);
        }
    }
}