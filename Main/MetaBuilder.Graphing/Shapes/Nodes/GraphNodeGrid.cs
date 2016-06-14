using System;
using System.Drawing;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Shapes
{
    [Serializable]
    public class GraphNodeGrid : GoGrid
    {
        #region Fields (1)

        private bool autoSize;

        #endregion Fields

        #region Constructors (1)

        #endregion Constructors

        #region Properties (2)

        public bool AutoSize
        {
            get { return autoSize; }
            set { autoSize = value; }
        }

        bool fakeprintvariable;
        public override bool Printable
        {
            get
            {
                return false;
                //return base.Printable;
            }
            set
            {
                //base.Printable = value;
                fakeprintvariable = value;
            }
        }

        #endregion Properties

        #region Methods (3)

        // Public Methods (3) 

        // the grid in a GraphNode only snaps Items
        public override bool CanSnapPoint(PointF p, GoObject obj, GoView view)
        {
            if (!(obj is ISnappableShape))
                return false;
            // do standard checks, such as whether the grid is a child of the OBJ,
            // the value of the grid's snap style, whether the OBJ intersects with the grid,
            // and whether the mouse point is in the grid
            return base.CanSnapPoint(p, obj, view);
        }

        public override PointF ComputeMove(PointF origLoc, PointF newLoc)
        {
            PointF r = base.ComputeMove(origLoc, newLoc);
            r.X = Math.Max(1, (float)Math.Round(r.X / SnapSettings.UnitSize)) * SnapSettings.UnitSize;
            r.Y = Math.Max(1, (float)Math.Round(r.Y / SnapSettings.UnitSize)) * SnapSettings.UnitSize;
            return r;
        }

        // it can be resized only in positive multiples of Item.UnitSize
        public override RectangleF ComputeResize(RectangleF origRect, PointF newPoint, int handle, SizeF min, SizeF max,
                                                 bool reshape)
        {
            if (AutoSize)
            {
                RectangleF r = base.ComputeResize(origRect, newPoint, handle, min, max, reshape);
                r.Width = Math.Max(1, (float)Math.Round(r.Width / SnapSettings.UnitSize)) * SnapSettings.UnitSize;
                r.Height = Math.Max(1, (float)Math.Round(r.Height / SnapSettings.UnitSize)) * SnapSettings.UnitSize;
                return r;
            }
            return base.ComputeResize(origRect, newPoint, handle, min, max, reshape);
        }

        #endregion Methods

        /* public override bool Selectable
        {
            get
            {
                return false;
            }
            set
            {
                // do nothing
                // base.Selectable = value;
            }
        }*/
    }
}