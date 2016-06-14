using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;
using System.Drawing;
namespace ShapeBuilding.Links
{
    [Serializable]
    public class QLinkMidPort : GoPort
    {
        public QLinkMidPort()
        {
            this.Size = new SizeF(0, 0);
            this.Style = GoPortStyle.None;  // the port has no visual appearance
            this.IsValidFrom = false;  // can only draw links to this port, not from it
            this.ToSpot = NoSpot;  // the connection point is computed dynamically
        }

        // this is just the point on the line that is closest to the given point
        public PointF NearestPoint(PointF c)
        {
            GoStroke s = this.PortObject as GoStroke;
            if (s != null && s.PointsCount == 2)
            {
                PointF p;
                GoStroke.NearestPointOnLine(s.GetPoint(0), s.GetPoint(1), c, out p);
                return p;
            }
            else
            {
                return this.Center;
            }
        }

        // customize the connection point to be the nearest point to the center
        // of the "from" port
        public override PointF GetToLinkPoint(IGoLink link)
        {
            if (link.FromPort != null)
                return NearestPoint(link.FromPort.GoObject.Center);
            else
                return base.GetToLinkPoint(link);
        }
    }
}
