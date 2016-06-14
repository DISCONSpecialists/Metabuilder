using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;
using System.Drawing;
using MetaBuilder.Graphing.Containers;

namespace ShapeBuilding.Links
{
    public class LinkController
    {
        public void InsertPoint_Command(GoLabeledLink lnk, Object sender, EventArgs e)
        {

            //GraphView v = GoContextMenu.FindView(sender as MenuItem) as GraphView;
            GoView v = lnk.View;
            if (v != null)
            {
                PointF menupt = v.LastInput.DocPoint;
                GoStroke s = lnk.RealLink;
                int i = s.GetSegmentNearPoint(menupt);
                if (s.PointsCount > 3)
                {
                    if (i < 1)
                        i = 1; // don't add to first segment
                    else if (i >= s.PointsCount - 2)
                        i = s.PointsCount - 3; // don't add to last segment
                }
                PointF closest;
                if (GoStroke.NearestPointOnLine(s.GetPoint(i), s.GetPoint(i + 1), menupt, out closest))
                {
                    v.StartTransaction();
                    s.InsertPoint(i + 1, closest);
                    if (lnk.Orthogonal) // when orthogonal, gotta insert two points
                        s.InsertPoint(i + 1, closest);
                    s.AddSelectionHandles(v.Selection, lnk); // assume selected
                    v.FinishTransaction("inserted point into link stroke");
                }
            }
        }
        public void RemoveSegment_Command(GoLabeledLink lnk, Object sender, EventArgs e)
        {
            //GraphView v = GoContextMenu.FindView(sender as MenuItem) as GraphView;
            GoView v = lnk.View;
            if (v != null)
            {
                PointF menupt = v.LastInput.DocPoint;
                GoStroke s = lnk.RealLink;
                int i = s.GetSegmentNearPoint(menupt);
                v.StartTransaction();
                if (lnk.Orthogonal)
                {
                    // will have at least 7 points
                    // don't remove either first two or last two segments
                    i = Math.Max(i, 2);
                    i = Math.Min(i, lnk.RealLink.PointsCount - 5);
                    if (i > -1)
                    {
                        PointF a = s.GetPoint(i);
                        PointF b = s.GetPoint(i + 1);
                        s.RemovePoint(i);
                        // to maintain orthogonality, gotta remove two points
                        if (s.PointsCount > i)
                        {
                            s.RemovePoint(i);
                            // now fix up following point to maintain orthogonality
                            PointF c = s.GetPoint(i);
                            if (a.X == b.X)
                            {
                                c.Y = a.Y;
                            }
                            else
                            {
                                c.X = a.X;
                            }
                            s.SetPoint(i, c);
                        }
                    }
                }
                else
                {
                    // will have at least 3 points
                    i = Math.Max(i, 1); // don't remove point 0
                    i = Math.Min(i, lnk.RealLink.PointsCount - 2); // don't remove last point
                    s.RemovePoint(i);
                }
                s.AddSelectionHandles(v.Selection, lnk); // assume selected
                v.FinishTransaction("removed point from link stroke");
            }
        }
    }
}
