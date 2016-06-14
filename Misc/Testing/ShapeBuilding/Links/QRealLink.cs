using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;
using System.Drawing;
using System.Windows.Forms;
namespace ShapeBuilding.Links
{
    [Serializable]
    public class QRealLink : GoLink
    {
        public QRealLink() { }

        // don't show the relinking selection handle at the "from" end
        public override void AddSelectionHandles(GoSelection sel, GoObject selectedObj)
        {
            base.AddSelectionHandles(sel, selectedObj);
            IGoHandle hFrom = sel.FindHandleByID(this, GoLink.RelinkableFromHandle);
            if (hFrom != null)
            {
                //h.GoObject.Remove();
                hFrom.GoObject.Location = this.FromNode.GoObject.Location;
            }
            IGoHandle hTo = sel.FindHandleByID(this, GoLink.RelinkableToHandle);
            if (hTo != null)
            {
                //h.GoObject.Remove();
                hTo.GoObject.Location = this.ToNode.GoObject.Location;
            }
        }

        public override void CalculateArrowhead(System.Drawing.PointF anchor, System.Drawing.PointF endPoint, bool atEnd, System.Drawing.PointF[] poly)
        {

            base.CalculateArrowhead(anchor, endPoint, atEnd, poly);

            if (this.AbstractLink != null)
            {

                QLink qlnk = this.AbstractLink as QLink;
                if (qlnk != null)
                {

                    switch (qlnk.AssociationType)
                    {
                        case MetaBuilder.Meta.LinkAssociationType.One_To_Many:
                            GoGroup grp = new GoGroup();
                            qlnk.ToLabel = grp;
                            DrawCrow(qlnk, true, grp, true);
                            break;
                        case MetaBuilder.Meta.LinkAssociationType.ZeroOrMore_To_One:
                            GoGroup grpFrom = new GoGroup();
                            qlnk.FromLabel = grpFrom;
                            DrawCrow(qlnk, false, grpFrom, true);
                            AddCircle(grpFrom, FromArrowAnchorPoint);
                            break;
                        case MetaBuilder.Meta.LinkAssociationType.Many_To_Many:
                            GoGroup grpFromMM = new GoGroup();
                            qlnk.FromLabel = grpFromMM;
                            GoGroup grpToMM = new GoGroup();
                            qlnk.ToLabel = grpToMM;
                            DrawCrow(qlnk, false, grpFromMM, true);
                            DrawCrow(qlnk, true, grpToMM, true);
                            break;
                        case MetaBuilder.Meta.LinkAssociationType.Zero_To_One:
                            GoGroup grFROM = new GoGroup();
                            qlnk.FromLabel = grFROM;
                            DrawCrow(qlnk, false, grFROM, false);
                            AddCircle(grFROM, FromArrowAnchorPoint);
                            break;
                    }
                }
            }
            /*SizeF width = new SizeF(this.ToArrowShaftLength / 2, 0);
            poly[0] = this.ToArrowEndPoint - width;
            poly[2] = this.ToArrowEndPoint + width;*/
        }

        /*
         * +		((Northwoods.Go.GoStroke)(this)).ToArrowAnchorPoint	{X = 79.0 Y = 2600.0}	System.Drawing.PointF
+		((Northwoods.Go.GoStroke)(this)).ToArrowEndPoint	{X = 89.0 Y = 2600.0}	System.Drawing.PointF

         */
        private void AddCircle(GoGroup grp, PointF pos)
        {
            GoEllipse ell = new GoEllipse();
            ell.Height = 6;
            ell.Width = 6;
            ell.Center = pos;
            grp.Add(ell);
        }
        private enum LinkSegmentDirection
        {
            Up, Down, Left, Right
        }

        private LinkSegmentDirection GetArrowDirection(PointF pAnchorPoint, PointF pEndPoint)
        {
            if (pAnchorPoint.X < pEndPoint.X)
                return LinkSegmentDirection.Right;
            if (pAnchorPoint.X > pEndPoint.X)
                return LinkSegmentDirection.Left;
            if (pAnchorPoint.Y < pEndPoint.Y)
                return LinkSegmentDirection.Down;
            return LinkSegmentDirection.Up;
        }
        private LinkSegmentDirection ToDirection
        {
            get
            {
                return GetArrowDirection(ToArrowAnchorPoint, ToArrowEndPoint);
            }
        }
        private LinkSegmentDirection FromDirection
        {
            get
            {
                return GetArrowDirection(FromArrowAnchorPoint, FromArrowEndPoint);
            }
        }
        private void DrawCrow(QLink lnkParent, bool AtEnd, GoGroup grp, bool vis)
        {
            GoStroke str1 = new GoStroke();
            str1.Selectable = false;
            str1.Deletable = false;
            str1.Visible = vis;


            LinkSegmentDirection toSegmentDirection = ToDirection;
            LinkSegmentDirection fromSegmentDirection = FromDirection;


            if (AtEnd)
            {
                if (toSegmentDirection == LinkSegmentDirection.Right || toSegmentDirection == LinkSegmentDirection.Left)
                {
                    str1.AddPoint(new PointF(ToArrowEndPoint.X, ToArrowEndPoint.Y - 6f));
                    str1.AddPoint(ToArrowAnchorPoint);
                    str1.AddPoint(new PointF(ToArrowEndPoint.X, ToArrowEndPoint.Y + 6f));
                }
                if (toSegmentDirection == LinkSegmentDirection.Down || toSegmentDirection == LinkSegmentDirection.Up)
                {
                    str1.AddPoint(new PointF(ToArrowEndPoint.X - 6f, ToArrowEndPoint.Y));
                    str1.AddPoint(ToArrowAnchorPoint);
                    str1.AddPoint(new PointF(ToArrowEndPoint.X + 6f, ToArrowEndPoint.Y));
                }
                grp.Add(str1);
            }
            else
            {
                if (fromSegmentDirection == LinkSegmentDirection.Right || fromSegmentDirection == LinkSegmentDirection.Left)
                {
                    str1.AddPoint(new PointF(FromArrowEndPoint.X, FromArrowEndPoint.Y - 6f));
                    str1.AddPoint(FromArrowAnchorPoint);
                    str1.AddPoint(new PointF(FromArrowEndPoint.X, FromArrowEndPoint.Y + 6f));
                }
                if (fromSegmentDirection == LinkSegmentDirection.Down || fromSegmentDirection == LinkSegmentDirection.Up)
                {
                    str1.AddPoint(new PointF(FromArrowEndPoint.X - 6f, FromArrowEndPoint.Y));
                    str1.AddPoint(FromArrowAnchorPoint);
                    str1.AddPoint(new PointF(FromArrowEndPoint.X + 6f, FromArrowEndPoint.Y));
                }
                grp.Add(str1);
            }
        }
    }
}
