using System.Drawing;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Controllers
{
    public class ZoomController
    {
        #region Zoom

        private GoView myView;

        public ZoomController(GoView view)
        {
            myView = view;
        }

        // Changing the scale of the view
        public virtual void Zoom(float percent)
        {
            // This doesnt work as it should.
            if (myView is Containers.GraphView)
                (myView as Containers.GraphView).OverrideDocScaleMath = true;
            myView.DocScale = percent;
            if (myView.Selection.Primary != null)
                myView.ScrollRectangleToVisible(myView.Selection.Primary.Bounds);
            else
            {
                RectangleF rect = new RectangleF(myView.DocPosition, new SizeF(myView.DocPosition.X + 20, myView.DocPosition.Y + 20));
                myView.ScrollRectangleToVisible(rect);
            }
            if (myView is Containers.GraphView)
                (myView as Containers.GraphView).OverrideDocScaleMath = false;
        }

        public virtual void ZoomIn()
        {
            myView.DocScale += 0.01f;
        }

        public virtual void ZoomOut()
        {
            myView.DocScale -= 0.01f;
        }

        public virtual void ZoomNormal()
        {
            if (myView is Containers.GraphView)
                (myView as Containers.GraphView).OverrideDocScaleMath = true;
            myView.DocScale = 1;
            if (myView is Containers.GraphView)
                (myView as Containers.GraphView).OverrideDocScaleMath = false;
        }

        public virtual void ZoomPageWidth()
        {
        }

        public virtual void ZoomToFit()
        {
            if (myView is Containers.GraphView)
                (myView as Containers.GraphView).OverrideDocScaleMath = true;
            myView.RescaleToFit();
            if (myView is Containers.GraphView)
                (myView as Containers.GraphView).OverrideDocScaleMath = false;
        }

        #endregion
    }
}