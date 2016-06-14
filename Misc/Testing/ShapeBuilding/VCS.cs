using System;
using System.Drawing;
using Northwoods.Go;

namespace ShapeBuilding
{
    [Serializable]
    public class VCS : GoPolygon
    {
        public VCS(float width, float height)
            : base()
        {
          
            this.Resizable = true;
            //  this.Reshapable = false;
            DrawStep(width, height);
        }

        private void DrawStep(float width, float height)
        {

            ClearPoints();
            AddPoint(Position);
            AddPoint(Position.X + width, Position.Y);
            PointF invisibleMidFront = new PointF(Position.X + width, Position.Y + height / 2);
            AddPoint(invisibleMidFront.X + height / 2, invisibleMidFront.Y);//Position.X + width + _nose, Position.Y + height / 2);
            AddPoint(Position.X + width, Position.Y + height);
            AddPoint(Position.X, Position.Y + height);
            PointF invisibleMidBack = new PointF(Position.X + height / 2, Position.Y + height / 2);
            AddPoint(invisibleMidBack.X, invisibleMidBack.Y);//Position.X + _tail, Position.Y + height / 2);
            Brush = Brushes.LightGreen;
            //   Reshapable = false;
        }


        public override void AddSelectionHandles(GoSelection sel, GoObject selectedObject)
        {
            RemoveSelectionHandles(sel);



            RectangleF rect = Bounds;

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
            //  if (CanReshape())
            {
                sel.CreateResizeHandle(this, selectedObject, new PointF(x2, y1), MiddleTop, true);
                sel.CreateResizeHandle(this, selectedObject, new PointF(x3, y2), MiddleRight, true);
                sel.CreateResizeHandle(this, selectedObject, new PointF(x2, y3), MiddleBottom, true);
                sel.CreateResizeHandle(this, selectedObject, new PointF(x1, y2), MiddleLeft, true);
            }
        }


        public override void DoResize(GoView view, RectangleF origRect, PointF newPoint, int whichHandle, GoInputState evttype, SizeF min, SizeF max)
        {
            base.DoResize(view, origRect, newPoint, whichHandle, evttype, min, max);
            DrawStep(this.Width, this.Height);
        }
        public override IGoHandle CreateBoundingHandle()
        {
            GoHandle h = new GoHandle();
            RectangleF rect = Bounds;//();
            // the handle rectangle should just go around the object
            rect.X--;
            rect.Y--;
            rect.Height += 2;
            rect.Width += 2;
            h.Bounds = rect;
            return h;
        }
       

        private void DoRedraw()
        {
            DrawStep(Width, Height);
        }
    }
}