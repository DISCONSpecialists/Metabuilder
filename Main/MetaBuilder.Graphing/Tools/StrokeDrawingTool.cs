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
using System.Windows.Forms;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Shapes.Primitives;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Tools
{
    [Serializable]
    public class StrokeDrawingTool : GoTool
    {
        #region Fields (2) 

        private readonly GoStrokeStyle strokeStyle;
        private QuickStroke myStroke;

        #endregion Fields 

        #region Constructors (1) 

        public StrokeDrawingTool(GraphView view, GoStrokeStyle style) : base(view)
        {
            strokeStyle = style;
        }

        #endregion Constructors 

        #region Methods (6) 

        // Public Methods (6) 

        public override void DoKeyDown()
        {
            if (LastInput.Key == Keys.Enter)
            {
                if (myStroke != null && myStroke.PointsCount > 2)
                {
                    StartTransaction();
                    // remove the last point, which was following the mouse
                    myStroke.RemovePoint(myStroke.PointsCount - 1);
                    // now move the stroke from the view to the document
                    myStroke.Remove();
                    View.Document.Add(myStroke);
                    // and make it selected
                    View.Selection.Select(myStroke);
                    myStroke = null;
                    TransactionResult = "New Stroke";
                    StopTransaction();
                    StopTool();
                }
            }
            else if (LastInput.Control && LastInput.Key == Keys.Z)
            {
                // throw away the last point that the user clicked at
                if (myStroke != null)
                {
                    int numpts = myStroke.PointsCount;
                    if (numpts > 2)
                    {
                        myStroke.RemovePoint(numpts - 1);
                        myStroke.SetPoint(numpts - 2, LastInput.DocPoint);
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
            if (myStroke == null)
            {
                // create a new stroke starting at the first mouse down point
                myStroke = new QuickStroke();
                myStroke.AddPoint(FirstInput.DocPoint);
                myStroke.Style = strokeStyle;
                // the stroke is temporarily a view object
                View.Layers.Default.Add(myStroke);
            }
            myStroke.AddPoint(LastInput.DocPoint);
        }

        public override void DoMouseMove()
        {
            if (myStroke != null)
            {
                int numpts = myStroke.PointsCount;
                myStroke.SetPoint(numpts - 1, LastInput.DocPoint);
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
                                                                                            "Drawing mode -- click to add points to a new Stroke, Enter or Escape to stop"));
            View.Cursor = Cursors.Cross;
        }

        public override void Stop()
        {
            if (myStroke != null)
            {
                myStroke.Remove();
                myStroke = null;
            }
            View.Cursor = View.DefaultCursor;
            if (View is GraphView)
                ((GraphView) View).RaiseProxiedDispatchMessage(this,
                                                               new DispatchMessageEventArgs(MessageType.Status,
                                                                                            "Stopped Drawing mode"));
        }

        #endregion Methods 
    }
}