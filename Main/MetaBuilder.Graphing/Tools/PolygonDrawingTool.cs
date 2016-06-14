/*
 *  Copyright © 2007 - DISCON Specialists
 *
 *
 *  
 *  
 *  
 *  
 */

using System;
using System.Drawing;
using System.Windows.Forms;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Formatting;
using MetaBuilder.Graphing.Shapes.Behaviours;
using MetaBuilder.Graphing.Shapes.Behaviours.Internal;
using MetaBuilder.Graphing.Shapes.Primitives;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Tools
{
    // TODO: Add Gradient Support
    [Serializable]
    public class PolygonDrawingTool : GoTool
    {
        #region Fields (2) 

        private GradientPolygon myPolygon;
        private GoPolygonStyle style;

        #endregion Fields 

        #region Constructors (1) 

        public PolygonDrawingTool(GraphView view) : base(view)
        {
        }

        #endregion Constructors 

        #region Properties (1) 

        public GoPolygonStyle Style
        {
            get { return style; }
            set { style = value; }
        }

        #endregion Properties 

        #region Methods (7) 

        // Public Methods (6) 

        public override void DoKeyDown()
        {
            if (LastInput.Key == Keys.Enter)
            {
                if (myPolygon != null && myPolygon.PointsCount > 2)
                {
                    StartTransaction();
                    // make the final segment go back to the first point
                    LastInput.DocPoint = myPolygon.GetPoint(0);
                    DoMouseMove();
                    // now move the Polygon from the view to the document
                    myPolygon.Remove();
                    View.Document.Add(myPolygon);
                    // and make it selected
                    View.Selection.Select(myPolygon);
                    myPolygon = null;
                    TransactionResult = "New Polygon";
                    StopTransaction();
                    StopTool();
                }
            }
            else if (LastInput.Control && LastInput.Key == Keys.Z)
            {
                // throw away the last point that the user clicked at
                if (myPolygon != null)
                {
                    int numpts = myPolygon.PointsCount;
                    if (numpts > 4)
                    {
                        myPolygon.RemovePoint(numpts - 1);
                        myPolygon.RemovePoint(numpts - 2);
                        myPolygon.RemovePoint(numpts - 3);
                    }
                }
            }
            else
            {
                base.DoKeyDown();
            }
        }

        public override void DoMouseDown()
        {
            if (myPolygon == null)
            {
                // create a new Polygon starting at the first mouse down point
                myPolygon = new GradientPolygon(style);
                SetDefaultGradientBehaviour(myPolygon);
                myPolygon.AddPoint(FirstInput.DocPoint);

                // the Polygon is temporarily a view object
                View.Layers.Default.Add(myPolygon);
            }
            myPolygon.AddPoint(LastInput.DocPoint); // first control point for Bezier
            myPolygon.AddPoint(LastInput.DocPoint); // second control point for Bezier
            myPolygon.AddPoint(LastInput.DocPoint);
        }

        public override void DoMouseMove()
        {
            if (myPolygon != null)
            {
                int numpts = myPolygon.PointsCount;
                if (numpts >= 4)
                {
                    PointF start = myPolygon.GetPoint(numpts - 4);
                    PointF end = LastInput.DocPoint;
                    myPolygon.SetPoint(numpts - 3, new PointF(2*start.X/3 + end.X/3, 2*start.Y/3 + end.Y/3));
                    myPolygon.SetPoint(numpts - 2, new PointF(start.X/3 + 2*end.X/3, start.Y/3 + 2*end.Y/3));
                    myPolygon.SetPoint(numpts - 1, end);
                }
            }
        }

        public override void DoMouseUp()
        {
            // don't call the base functionality, which assumes mouse up stops the tool
        }

        public override void Start()
        {
            if (View is GraphView)
                ((GraphView) View).RaiseProxiedDispatchMessage(this,
                                                               new DispatchMessageEventArgs(MessageType.Status,
                                                                                            "Drawing mode -- click to add points to a new Polygon, Enter or Escape to stop"));
            View.Cursor = Cursors.Cross;
        }

        public override void Stop()
        {
            if (myPolygon != null)
            {
                myPolygon.Remove();
                myPolygon = null;
            }
            View.Cursor = View.DefaultCursor;
            if (View is GraphView)
                ((GraphView) View).RaiseProxiedDispatchMessage(this,
                                                               new DispatchMessageEventArgs(MessageType.Status,
                                                                                            "Stopped Drawing mode"));
        }


        // Private Methods (1) 

        private static void SetDefaultGradientBehaviour(GoShape shape)
        {
            IBehaviourShape ibshape = shape as IBehaviourShape;
            GradientBehaviour gbehaviour = new GradientBehaviour();
            gbehaviour.MyBrush = new ShapeGradientBrush();
            gbehaviour.MyBrush.OuterColor = Color.LightGreen;
            gbehaviour.MyBrush.InnerColor = Color.LightGreen;
            gbehaviour.MyBrush.GradientType = GradientType.ForwardDiagonal;
            ibshape.Manager = new BaseShapeManager();
            gbehaviour.Owner = shape;
            ibshape.Manager.AddBehaviour(gbehaviour);
            gbehaviour.Update(shape);
        }

        #endregion Methods 
    }
}