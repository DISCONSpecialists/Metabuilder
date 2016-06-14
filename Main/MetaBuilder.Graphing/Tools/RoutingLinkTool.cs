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
using Northwoods.Go;

namespace MetaBuilder.Graphing.Tools
{
    [Serializable]
    public class RoutingLinkTool : GoTool
    {
        #region Fields (4) 

        private IGoPort myFromPort;
        private bool myHoriz = true;
        private bool myOrthogonal = true;
        private GoStroke myStroke;

        #endregion Fields 

        #region Constructors (1) 

        public RoutingLinkTool(GraphView view) : base(view)
        {
        }

        #endregion Constructors 

        #region Properties (1) 

        public bool Orthogonal
        {
            get { return myOrthogonal; }
            set { myOrthogonal = value; }
        }

        #endregion Properties 

        #region Methods (6) 

        // Public Methods (6) 

        public override void DoKeyDown()
        {
            if (LastInput.Control && LastInput.Key == Keys.Z)
            {
                // throw away the last point that the user clicked at
                if (myStroke != null)
                {
                    int numpts = myStroke.PointsCount;
                    if (numpts > 3 || (!Orthogonal && numpts > 2))
                    {
                        myHoriz = !myHoriz;
                        myStroke.RemovePoint(numpts - 1);
                        myStroke.SetPoint(numpts - 2, LastInput.DocPoint);
                        DoMouseMove(); // fix up penultimate point
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
                IGoPort port = View.PickObject(true, false, LastInput.DocPoint, false) as IGoPort;
                if (port != null && port.CanLinkFrom())
                {
                    // found the source port
                    // remember it for when we create the link
                    myFromPort = port;
                    // create a new stroke starting at or near the first mouse down point
                    GoStroke stroke = new GoStroke();
                    PointF lp = LastInput.DocPoint;
                    // try to be smart about link point
                    GoPort p = port as GoPort;
                    if (p != null)
                        lp = p.GetFromLinkPoint(null);
                    if (Orthogonal)
                    {
                        PointF firstpt = lp;
                        // add a next-to-last point that gets modified to maintain orthogonality
                        PointF secondpt = lp;
                        if (p != null)
                        {
                            switch (p.FromSpot)
                            {
                                case GoObject.MiddleLeft:
                                    myHoriz = true;
                                    secondpt = new PointF(lp.X - p.EndSegmentLength, lp.Y);
                                    break;
                                // default: <- not sure wh there is a default statement here
                                case GoObject.MiddleRight:
                                    myHoriz = true;
                                    secondpt = new PointF(lp.X + p.EndSegmentLength, lp.Y);
                                    break;
                                case GoObject.MiddleTop:
                                    myHoriz = false;
                                    secondpt = new PointF(lp.X, lp.Y - p.EndSegmentLength);
                                    break;
                                case GoObject.MiddleBottom:
                                    myHoriz = false;
                                    secondpt = new PointF(lp.X, lp.Y + p.EndSegmentLength);
                                    break;
                                
                            }
                            firstpt = p.GetLinkPointFromPoint(secondpt);
                        }
                        stroke.AddPoint(firstpt);
                        stroke.AddPoint(secondpt);
                    }
                    else
                    {
                        stroke.AddPoint(lp);
                    }
                    // the last point is modified in DoMouseMove to follow the mouse
                    stroke.AddPoint(LastInput.DocPoint);
                    // the stroke is temporarily a view object
                    View.Layers.Default.Add(stroke);
                    myStroke = stroke;
                }
            }
            else
            {
                if (myStroke != null)
                {
                    IGoPort iport = View.PickObject(true, false, LastInput.DocPoint, false) as IGoPort;
                    if (iport != null && myFromPort.IsValidLink(iport))
                    {
                        // found a destination port
                        // get rid of the stroke from the view, but keep the object for its points
                        View.Layers.Default.Remove(myStroke);
                        // create the link in the document

                        IGoLink link = View.CreateLink(myFromPort, iport);
                        // get the stroke of the real link
                        GoStroke ls = link.GoObject as GoStroke;
                        if (ls == null && link.GoObject is GoLabeledLink)
                        {
                            ls = (link.GoObject as GoLabeledLink).RealLink;
                        }
                        if (ls != null)
                        {
                            if (ls is GoLink)
                            {
                                ((GoLink) ls).Orthogonal = Orthogonal;
                            }
                            // now update the link's stroke points
                            // add all but the first and last of the points the user clicked on
                            // (the first and last specified ports)
                            PointF firstpt = ls.GetPoint(0);
                            PointF secondpt = ls.GetPoint(1);
                            bool startseg = false;
                            PointF nexttolastpt = ls.GetPoint(ls.PointsCount - 2);
                            PointF lastpt = ls.GetPoint(ls.PointsCount - 1);
                            bool endseg = false;
                            if (myFromPort is GoPort)
                            {
                                if (Orthogonal)
                                {
                                    startseg = true;
                                    PointF lssecond = ls.GetPoint(1);
                                    PointF mysecond = myStroke.GetPoint(1);
                                    PointF mythird = myStroke.GetPoint(2);
                                    if (mysecond.X == mythird.X)
                                    {
                                        mysecond.Y = lssecond.Y;
                                    }
                                    else if (mysecond.Y == mythird.Y)
                                    {
                                        mysecond.X = lssecond.X;
                                    }
                                    myStroke.SetPoint(1, mysecond);
                                }
                            }
                            if (iport is GoPort)
                            {
                                if (Orthogonal)
                                {
                                    endseg = true;
                                    // fix up the next to last drawn point to account for
                                    // any end segment in the link (rather than directly to the port)
                                    int numpts = myStroke.PointsCount;
                                    PointF lspenult = ls.GetPoint(ls.PointsCount - 2);
                                    PointF mypenult = myStroke.GetPoint(numpts - 2);
                                    PointF myante = myStroke.GetPoint(numpts - 3);
                                    if (mypenult.X == myante.X)
                                    {
                                        mypenult.Y = lspenult.Y;
                                    }
                                    else if (mypenult.Y == myante.Y)
                                    {
                                        mypenult.X = lspenult.X;
                                    }
                                    myStroke.SetPoint(numpts - 2, mypenult);
                                }
                            }
                            ls.ClearPoints();
                            ls.AddPoint(firstpt);
                            if (startseg)
                                ls.AddPoint(secondpt);
                            for (int i = 1; i < myStroke.PointsCount - 1; i++)
                            {
                                ls.AddPoint(myStroke.GetPoint(i));
                            }
                            if (endseg)
                                ls.AddPoint(nexttolastpt);
                            ls.AddPoint(lastpt);
                        }
                        myStroke = null; // now we can throw the temp view link away
                        View.Selection.Select(link.GoObject);
                        TransactionResult = "New Routed Link";
                        StopTool();
                    }
                    else
                    {
                        // keep adding points to the stroke representing the future link
                        myHoriz = !myHoriz;
                        myStroke.AddPoint(LastInput.DocPoint);
                    }
                }
            }
        }

        public override void DoMouseMove()
        {
            if (myStroke != null)
            {
                int numpts = myStroke.PointsCount;
                PointF ult = LastInput.DocPoint;
                myStroke.SetPoint(numpts - 1, ult);
                if (Orthogonal && numpts > 2)
                {
                    PointF penult = myStroke.GetPoint(numpts - 2);
                    if (myHoriz)
                    {
                        penult.X = ult.X;
                    }
                    else
                    {
                        penult.Y = ult.Y;
                    }
                    // ?? what if previous points aren't right
                    myStroke.SetPoint(numpts - 2, penult);
                }
            }
        }

        public override void DoMouseUp()
        {
            // don't call the base functionality, which resets the tool
        }

        public override void Start()
        {
            StartTransaction();
            myStroke = null;
            myFromPort = null;
            View.Cursor = Cursors.Hand;
        }

        public override void Stop()
        {
            if (myStroke != null)
            {
                View.Layers.Default.Remove(myStroke);
                myStroke = null;
            }
            myFromPort = null;
            View.Cursor = View.DefaultCursor;
            StopTransaction();
        }

        #endregion Methods 

        /*private GoLink createdLink;
        public GoLink CreatedLink
        {
            get
            {
                return createdLink;
            }
        }*/
    }
}