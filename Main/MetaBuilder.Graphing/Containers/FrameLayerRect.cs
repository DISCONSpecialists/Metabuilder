using System.Drawing;
using Northwoods.Go;
using System;

namespace MetaBuilder.Graphing.Containers
{
    [Serializable]
    public class FrameLayerRect : GoRectangle
    {
        public override void AddSelectionHandles(GoSelection sel, GoObject selectedObj)
        {
            RemoveSelectionHandles(sel);

            if (!CanResize())
            {
                sel.CreateBoundingHandle(this, selectedObj);
                return;
            }

            RectangleF rect = Bounds;

            float x2 = rect.X + (rect.Width / 2);
            float x3 = rect.X + rect.Width;

            float y2 = rect.Y + (rect.Height / 2);
            float y3 = rect.Y + rect.Height;

            // create the handles
            //sel.CreateResizeHandle(this, selectedObj, new PointF(x1, y1), TopLeft, true);
            //sel.CreateResizeHandle(this, selectedObj, new PointF(x3, y1), TopRight, true);

            sel.CreateResizeHandle(this, selectedObj, new PointF(x3, y3), BottomRight, true);
            //sel.CreateResizeHandle(this, selectedObject, new PointF(x1, y3), BottomLeft, true);
            if (CanReshape())
            {
                if (Core.Variables.Instance.ShowDeveloperItems)
                {
                    float x1 = rect.X;
                    float y1 = rect.Y;
                    sel.CreateResizeHandle(this, selectedObj, new PointF(x2, y1), MiddleTop, true);
                    sel.CreateResizeHandle(this, selectedObj, new PointF(x1, y2), MiddleLeft, true);
                }
                sel.CreateResizeHandle(this, selectedObj, new PointF(x3, y2), MiddleRight, true);
                sel.CreateResizeHandle(this, selectedObj, new PointF(x2, y3), MiddleBottom, true);
                //bottom right corner resize handle
                sel.CreateResizeHandle(this, selectedObj, new PointF(rect.X, rect.Y), NoSpot, true);
            }

        }

        protected override void OnBoundsChanged(RectangleF old)
        {
            if (Initializing)
                return;

            if (this.Document is NormalDiagram)
            {
                if (this.Height < 280)
                {
                    this.Height = 280;
                    if (this.Width < 600)
                        this.Width = 600;

                    //updatesize of frame
                    (this.Document as NormalDiagram).UpdateSize = true;
                    return;
                }
                else if (this.Width < 600)
                {
                    this.Width = 600;
                    if (this.Height < 280)
                        this.Height = 280;

                    //updatesize of frame
                    (this.Document as NormalDiagram).UpdateSize = true;
                    return;
                }
            }

            base.OnBoundsChanged(old);
        }
    }
}