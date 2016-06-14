using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;
using System.Drawing;
using System.Windows.Forms;
namespace MetaBuilder.Graphing.Shapes
{
    enum LinkSegmentDirection
    {
        Up, Down, Left, Right
    }

    [Serializable]
    public class QRealLink : GoLink
    {
        public QRealLink()
        {
            Pen pReset = new Pen(Color.Black, 1);
            pReset.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            Pen = pReset;
        }

        // don't show the relinking selection handle at the "from" end
        public override void AddSelectionHandles(GoSelection sel, GoObject selectedObj)
        {
            if (sel.View != null && sel.View.Tool is MetaBuilder.Graphing.Tools.CustomDraggingTool)
                return;
            if (sel.View.ToString() == "MetaBuilder.Graphing.Tools.ContextViewer")
                return;

            base.AddSelectionHandles(sel, selectedObj);
            QLink qlnk = AbstractLink as QLink;
            if (qlnk != null)
            {
                foreach (IGoHandle h in sel.GetHandleEnumerable(this))
                {
                    GoHandle goh = h.GoObject as GoHandle;
                    if (goh != null)
                    {
                        goh.Shadowed = false;
                        if (h.HandleID == GoLink.RelinkableFromHandle || h.HandleID == GoLink.RelinkableToHandle)
                        {
                            goh.Brush = new SolidBrush(Color.Red);
                            goh.Style = GoHandleStyle.Rectangle;
                            RectangleF bounds = goh.Bounds;
                            bounds.Inflate(bounds.Width / 10, bounds.Height / 10);
                            goh.Bounds = bounds;
                            goh.CursorName = "hand";
                            if (h.HandleID == GoLink.RelinkableFromHandle)
                            {
                                goh.Location = this.FromPort != null ? this.FromPort.GoObject.Location : goh.Location;
                            }
                            else if (h.HandleID == GoLink.RelinkableToHandle)
                            {
                                goh.Location = this.ToPort != null ? this.ToPort.GoObject.Location : goh.Location;
                            }
                        }
                        else
                        {
                            goh.Brush = new SolidBrush(Color.Green);
                            goh.Pen = new Pen(Color.Green, 1f);
                            //goh.Pen = Pens.Green;
                        }
                    }
                }
            }
        }
        public override float PickMargin
        {
            get
            {
                return base.PickMargin + 5f;
            }
        }
        //August 6 2013 (Memory leak Fix)
        [NonSerialized]
        public bool calculated = false;

        public override void OnPortChanged(IGoPort port, int subhint, int oldI, object oldVal, RectangleF oldRect, int newI, object newVal, RectangleF newRect)
        {
            base.OnPortChanged(port, subhint, oldI, oldVal, oldRect, newI, newVal, newRect);
            calculated = false;
        }

        private bool calculationRequired(QLink qLink)
        {
            switch (qLink.AssociationType)
            {
                case MetaBuilder.Meta.LinkAssociationType.One_To_Many:
                case MetaBuilder.Meta.LinkAssociationType.ZeroOrMore_To_One:
                case MetaBuilder.Meta.LinkAssociationType.Many_To_Many:
                case MetaBuilder.Meta.LinkAssociationType.Zero_To_One:
                case MetaBuilder.Meta.LinkAssociationType.One_To_One:
                    return true;
                default:
                    return false;
            }
        }

        [NonSerialized]
        public GoGroup fromGroup = null;
        [NonSerialized]
        public GoGroup toGroup = null;

        public override void CalculateArrowhead(System.Drawing.PointF anchor, System.Drawing.PointF endPoint, bool atEnd, System.Drawing.PointF[] poly)
        {
            if (this.AbstractLink != null)
            {
                QLink qlnk = this.AbstractLink as QLink;
                if (qlnk != null)
                {
                    if (!calculationRequired(qlnk))
                    {
                        if (fromGroup != null)
                        {
                            fromGroup.Remove();
                            qlnk.Remove(fromGroup);
                            fromGroup = null;
                        }
                        //qlnk.FromLabel = null;
                        if (toGroup != null)
                        {
                            toGroup.Remove();
                            qlnk.Remove(toGroup);
                            toGroup = null;
                        }
                        //qlnk.ToLabel = null;
                        calculated = true;
                        this.SkipsUndoManager = false;
                        qlnk.SkipsUndoManager = false;
                        base.CalculateArrowhead(anchor, endPoint, atEnd, poly);
                        return;
                    }

                    if (calculated)
                        return;
                    this.SkipsUndoManager = true;
                    qlnk.SkipsUndoManager = true;

                    //Clear labels
                    if (qlnk.FromLabel != null)
                        qlnk.FromLabel.Remove();// = null;
                    if (qlnk.ToLabel != null)
                        qlnk.ToLabel.Remove();// = null;

                    qlnk.FromLabel = new GoGroup();
                    if (fromGroup != null)
                    {
                        fromGroup.Remove();
                        qlnk.Remove(fromGroup);
                        fromGroup = null;
                    }
                    qlnk.ToLabel = new GoGroup();
                    if (toGroup != null)
                    {
                        toGroup.Remove();
                        qlnk.Remove(toGroup);
                        toGroup = null;
                    }

                    qlnk.RemoveCustomMidGroup();

                    qlnk.ToLabel.Selectable = false;
                    qlnk.FromLabel.Selectable = false;
                    qlnk.MidLabel.Selectable = false;
                    //reset one line
                    //if (circleStroke != null)
                    //    circleStroke.Remove();
                    ArrowBuilder ab = new ArrowBuilder();
                    switch (qlnk.AssociationType)
                    {
                        case MetaBuilder.Meta.LinkAssociationType.One_To_Many:
                            //AddCrow(qlnk.ToLabel as GoGroup, ToArrowAnchorPoint, false, false, qlnk.FromLabel as GoGroup, qlnk.ToLabel as GoGroup);
                            fromGroup = CreateLabel(fromGroup, true, true, false, false);
                            qlnk.Add(fromGroup);
                            toGroup = CreateLabel(toGroup, false, true, true, false);
                            qlnk.Add(toGroup);
                            break;
                        case MetaBuilder.Meta.LinkAssociationType.ZeroOrMore_To_One:
                            //AddCrow(qlnk.FromLabel as GoGroup, FromArrowAnchorPoint, true, true, qlnk.ToLabel as GoGroup, null);
                            fromGroup = CreateLabel(fromGroup, true, false, true, true);
                            qlnk.Add(fromGroup);
                            toGroup = CreateLabel(toGroup, false, true, false, false);
                            qlnk.Add(toGroup);
                            break;
                        case MetaBuilder.Meta.LinkAssociationType.Many_To_Many:
                            fromGroup = CreateLabel(fromGroup, true, true, true, false);
                            qlnk.Add(fromGroup);
                            toGroup = CreateLabel(toGroup, false, true, true, false);
                            qlnk.Add(toGroup);
                            //AddCrow(qlnk.FromLabel as GoGroup, FromArrowAnchorPoint, true, false, null, qlnk.FromLabel as GoGroup);
                            //AddCrow(qlnk.ToLabel as GoGroup, ToArrowAnchorPoint, false, false, null, qlnk.ToLabel as GoGroup);
                            break;
                        case MetaBuilder.Meta.LinkAssociationType.Zero_To_One:
                            fromGroup = CreateLabel(fromGroup, true, true, false, true);
                            qlnk.Add(fromGroup);
                            toGroup = CreateLabel(toGroup, false, true, false, false);
                            qlnk.Add(toGroup);
                            //AddCircle(qlnk.FromLabel as GoGroup, FromArrowAnchorPoint, qlnk.ToLabel as GoGroup);
                            break;
                        case MetaBuilder.Meta.LinkAssociationType.One_To_One:
                            fromGroup = CreateLabel(fromGroup, true, true, false, false);
                            qlnk.Add(fromGroup);
                            toGroup = CreateLabel(toGroup, false, true, false, false);
                            qlnk.Add(toGroup);
                            break;
                    }
                    calculated = true;
                }
            }
        }

        #region FAIL
        //public GoStroke circleStroke = null;
        //public GoStroke circleStrokeX = null;
        //private void AddCircle(GoGroup grp, PointF pos, GoGroup oppositeisone)
        //{
        //    GoEllipse ell = new GoEllipse();
        //    ell.Selectable = false;
        //    ell.Movable = false;
        //    ell.Deletable = false;
        //    ell.Height = 13;
        //    ell.Width = 13;
        //    ell.Center = pos;
        //    grp.Add(ell);

        //    if (oppositeisone != null)
        //    {
        //        oppositeisone.Parent.Add(GetOne(true));
        //    }
        //}
        //private void AddCrow(GoGroup grp, PointF pos, bool from, bool ismore, GoGroup oppositeisone, GoGroup thisisone)
        //{
        //    grp.Selectable = false;
        //    grp.Movable = false;
        //    grp.Deletable = false;

        //    float arrowSpacing = 7f;
        //    GoStroke strokeA = new GoStroke();
        //    strokeA.Selectable = false;
        //    strokeA.Movable = false;
        //    strokeA.Deletable = false;

        //    if (from)
        //    {
        //        switch (FromDirection)
        //        {
        //            case LinkSegmentDirection.Left:
        //            case LinkSegmentDirection.Right:
        //                strokeA.AddPoint(new PointF(FromArrowEndPoint.X, FromArrowEndPoint.Y - arrowSpacing));
        //                strokeA.AddPoint(FromArrowAnchorPoint);
        //                strokeA.AddPoint(new PointF(FromArrowEndPoint.X, FromArrowEndPoint.Y + arrowSpacing));
        //                break;
        //            case LinkSegmentDirection.Up:
        //            case LinkSegmentDirection.Down:
        //                strokeA.AddPoint(new PointF(FromArrowEndPoint.X - arrowSpacing, FromArrowEndPoint.Y));
        //                strokeA.AddPoint(FromArrowAnchorPoint);
        //                strokeA.AddPoint(new PointF(FromArrowEndPoint.X + arrowSpacing, FromArrowEndPoint.Y));
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        switch (ToDirection)
        //        {
        //            case LinkSegmentDirection.Left:
        //            case LinkSegmentDirection.Right:
        //                strokeA.AddPoint(new PointF(ToArrowEndPoint.X, ToArrowEndPoint.Y - arrowSpacing));
        //                strokeA.AddPoint(ToArrowAnchorPoint);
        //                strokeA.AddPoint(new PointF(ToArrowEndPoint.X, ToArrowEndPoint.Y + arrowSpacing));
        //                break;
        //            case LinkSegmentDirection.Up:
        //            case LinkSegmentDirection.Down:
        //                strokeA.AddPoint(new PointF(ToArrowEndPoint.X - arrowSpacing, ToArrowEndPoint.Y));
        //                strokeA.AddPoint(ToArrowAnchorPoint);
        //                strokeA.AddPoint(new PointF(ToArrowEndPoint.X + arrowSpacing, ToArrowEndPoint.Y));
        //                break;
        //        }
        //    }

        //    grp.Add(strokeA);

        //    if (ismore)
        //    {
        //        GoEllipse ell = new GoEllipse();
        //        ell.Selectable = false;
        //        ell.Movable = false;
        //        ell.Deletable = false;
        //        ell.Height = 12;
        //        ell.Width = 12;
        //        ell.Center = FromArrowAnchorPoint;
        //        grp.Add(ell);
        //    }

        //    if (oppositeisone != null)
        //    {
        //        oppositeisone.Parent.Add(GetOne(from));
        //    }

        //    if (thisisone != null)
        //    {
        //        thisisone.Parent.Add(GetOneAgain(!from));
        //    }
        //}
        //private GoObject GetOne(bool from)
        //{
        //    if (circleStroke == null)
        //        circleStroke = new GoStroke();
        //    else
        //    {
        //        //circleStroke.Remove();
        //        circleStroke = new GoStroke();
        //    }
        //    circleStroke.Selectable = false;
        //    if (!from)
        //    {
        //        if (FromDirection == LinkSegmentDirection.Up)
        //        {
        //            circleStroke.AddPoint(FromArrowEndPoint.X - 5, FromArrowEndPoint.Y - 10);
        //            circleStroke.AddPoint(FromArrowEndPoint.X, FromArrowEndPoint.Y - 10);
        //            circleStroke.AddPoint(FromArrowEndPoint.X + 5, FromArrowEndPoint.Y - 10);
        //        }
        //        else if (FromDirection == LinkSegmentDirection.Down)
        //        {
        //            circleStroke.AddPoint(FromArrowEndPoint.X - 5, FromArrowEndPoint.Y + 10);
        //            circleStroke.AddPoint(FromArrowEndPoint.X, FromArrowEndPoint.Y + 10);
        //            circleStroke.AddPoint(FromArrowEndPoint.X + 5, FromArrowEndPoint.Y + 10);
        //        }
        //        else if (FromDirection == LinkSegmentDirection.Left)
        //        {
        //            circleStroke.AddPoint(FromArrowEndPoint.X - 10, FromArrowEndPoint.Y - 5);
        //            circleStroke.AddPoint(FromArrowEndPoint.X - 10, FromArrowEndPoint.Y);
        //            circleStroke.AddPoint(FromArrowEndPoint.X - 10, FromArrowEndPoint.Y + 5);
        //        }
        //        else if (FromDirection == LinkSegmentDirection.Right)
        //        {
        //            circleStroke.AddPoint(FromArrowEndPoint.X + 10, FromArrowEndPoint.Y - 5);
        //            circleStroke.AddPoint(FromArrowEndPoint.X + 10, ToArrowEndPoint.Y);
        //            circleStroke.AddPoint(FromArrowEndPoint.X + 10, FromArrowEndPoint.Y + 5);
        //        }
        //    }
        //    else
        //    {
        //        if (ToDirection == LinkSegmentDirection.Up)
        //        {
        //            circleStroke.AddPoint(ToArrowEndPoint.X - 5, ToArrowEndPoint.Y - 10);
        //            circleStroke.AddPoint(ToArrowEndPoint.X, ToArrowEndPoint.Y - 10);
        //            circleStroke.AddPoint(ToArrowEndPoint.X + 5, ToArrowEndPoint.Y - 10);
        //        }
        //        else if (ToDirection == LinkSegmentDirection.Down)
        //        {
        //            circleStroke.AddPoint(ToArrowEndPoint.X - 5, ToArrowEndPoint.Y + 10);
        //            circleStroke.AddPoint(ToArrowEndPoint.X, ToArrowEndPoint.Y + 10);
        //            circleStroke.AddPoint(ToArrowEndPoint.X + 5, ToArrowEndPoint.Y + 10);
        //        }
        //        else if (ToDirection == LinkSegmentDirection.Left)
        //        {
        //            circleStroke.AddPoint(ToArrowEndPoint.X - 10, ToArrowEndPoint.Y - 5);
        //            circleStroke.AddPoint(ToArrowEndPoint.X - 10, ToArrowEndPoint.Y);
        //            circleStroke.AddPoint(ToArrowEndPoint.X - 10, ToArrowEndPoint.Y + 5);
        //        }
        //        else if (ToDirection == LinkSegmentDirection.Right)
        //        {
        //            circleStroke.AddPoint(ToArrowEndPoint.X + 10, ToArrowEndPoint.Y - 5);
        //            circleStroke.AddPoint(ToArrowEndPoint.X + 10, ToArrowEndPoint.Y);
        //            circleStroke.AddPoint(ToArrowEndPoint.X + 10, ToArrowEndPoint.Y + 5);
        //        }
        //    }

        //    return circleStroke;
        //}
        //private GoObject GetOneAgain(bool from)
        //{
        //    if (circleStrokeX == null)
        //        circleStrokeX = new GoStroke();
        //    else
        //    {
        //        //circleStrokeX.Remove();
        //        circleStrokeX = new GoStroke();
        //    }
        //    circleStrokeX.Selectable = false;
        //    if (!from)
        //    {
        //        if (FromDirection == LinkSegmentDirection.Up)
        //        {
        //            circleStrokeX.AddPoint(FromArrowEndPoint.X - 5, FromArrowEndPoint.Y - 10);
        //            circleStrokeX.AddPoint(FromArrowEndPoint.X, FromArrowEndPoint.Y - 10);
        //            circleStrokeX.AddPoint(FromArrowEndPoint.X + 5, FromArrowEndPoint.Y - 10);
        //        }
        //        else if (FromDirection == LinkSegmentDirection.Down)
        //        {
        //            circleStrokeX.AddPoint(FromArrowEndPoint.X - 5, FromArrowEndPoint.Y + 10);
        //            circleStrokeX.AddPoint(FromArrowEndPoint.X, FromArrowEndPoint.Y + 10);
        //            circleStrokeX.AddPoint(FromArrowEndPoint.X + 5, FromArrowEndPoint.Y + 10);
        //        }
        //        else if (FromDirection == LinkSegmentDirection.Left)
        //        {
        //            circleStrokeX.AddPoint(FromArrowEndPoint.X - 10, FromArrowEndPoint.Y - 5);
        //            circleStrokeX.AddPoint(FromArrowEndPoint.X - 10, FromArrowEndPoint.Y);
        //            circleStrokeX.AddPoint(FromArrowEndPoint.X - 10, FromArrowEndPoint.Y + 5);
        //        }
        //        else if (FromDirection == LinkSegmentDirection.Right)
        //        {
        //            circleStrokeX.AddPoint(FromArrowEndPoint.X + 10, FromArrowEndPoint.Y - 5);
        //            circleStrokeX.AddPoint(FromArrowEndPoint.X + 10, ToArrowEndPoint.Y);
        //            circleStrokeX.AddPoint(FromArrowEndPoint.X + 10, FromArrowEndPoint.Y + 5);
        //        }
        //    }
        //    else
        //    {
        //        if (ToDirection == LinkSegmentDirection.Up)
        //        {
        //            circleStrokeX.AddPoint(ToArrowEndPoint.X - 5, ToArrowEndPoint.Y - 10);
        //            circleStrokeX.AddPoint(ToArrowEndPoint.X, ToArrowEndPoint.Y - 10);
        //            circleStrokeX.AddPoint(ToArrowEndPoint.X + 5, ToArrowEndPoint.Y - 10);
        //        }
        //        else if (ToDirection == LinkSegmentDirection.Down)
        //        {
        //            circleStrokeX.AddPoint(ToArrowEndPoint.X - 5, ToArrowEndPoint.Y + 10);
        //            circleStrokeX.AddPoint(ToArrowEndPoint.X, ToArrowEndPoint.Y + 10);
        //            circleStrokeX.AddPoint(ToArrowEndPoint.X + 5, ToArrowEndPoint.Y + 10);
        //        }
        //        else if (ToDirection == LinkSegmentDirection.Left)
        //        {
        //            circleStrokeX.AddPoint(ToArrowEndPoint.X - 10, ToArrowEndPoint.Y - 5);
        //            circleStrokeX.AddPoint(ToArrowEndPoint.X - 10, ToArrowEndPoint.Y);
        //            circleStrokeX.AddPoint(ToArrowEndPoint.X - 10, ToArrowEndPoint.Y + 5);
        //        }
        //        else if (ToDirection == LinkSegmentDirection.Right)
        //        {
        //            circleStrokeX.AddPoint(ToArrowEndPoint.X + 10, ToArrowEndPoint.Y - 5);
        //            circleStrokeX.AddPoint(ToArrowEndPoint.X + 10, ToArrowEndPoint.Y);
        //            circleStrokeX.AddPoint(ToArrowEndPoint.X + 10, ToArrowEndPoint.Y + 5);
        //        }
        //    }

        //    return circleStrokeX;
        //}


        //private void DrawCrow(QLink lnkParent, bool AtEnd, GoGroup grp, bool vis)
        //{
        //    LinkSegmentDirection toSegmentDirection = ToDirection;
        //    LinkSegmentDirection fromSegmentDirection = FromDirection;

        //    grp.Selectable = false;
        //    grp.Deletable = false;

        //    float arrowSpacing = 7f;

        //    GoStroke strokeA = null;
        //    GoStroke strokeB = null;

        //    //Recalculate if direction has changed by removing group and adding it with the updated stroke
        //    //if (grp.Count == 1)
        //    //    stroke = grp[0] as GoStroke;
        //    //else
        //    //{
        //    strokeA = new GoStroke();
        //    grp.Add(strokeA);
        //    //}
        //    //stroke = grp[0] as GoStroke;

        //    if (fromSegmentDirection != previousFromDirection)
        //    {
        //        if (fromSegmentDirection == LinkSegmentDirection.Right || fromSegmentDirection == LinkSegmentDirection.Left)
        //        {
        //            strokeA.AddPoint(new PointF(FromArrowEndPoint.X, FromArrowEndPoint.Y - arrowSpacing));
        //            strokeA.AddPoint(FromArrowAnchorPoint);
        //            strokeA.AddPoint(new PointF(FromArrowEndPoint.X, FromArrowEndPoint.Y + arrowSpacing));
        //        }
        //        else if (fromSegmentDirection == LinkSegmentDirection.Down || fromSegmentDirection == LinkSegmentDirection.Up)
        //        {
        //            strokeA.AddPoint(new PointF(FromArrowEndPoint.X - arrowSpacing, FromArrowEndPoint.Y));
        //            strokeA.AddPoint(FromArrowAnchorPoint);
        //            strokeA.AddPoint(new PointF(FromArrowEndPoint.X + arrowSpacing, FromArrowEndPoint.Y));
        //        }
        //        //Console.WriteLine("From Direction changed from : " + previousFromDirection.ToString() + " to : " + fromSegmentDirection.ToString());
        //        previousFromDirection = fromSegmentDirection;
        //    }
        //    else if (toSegmentDirection != previousToDirection)
        //    {
        //        if (toSegmentDirection == LinkSegmentDirection.Right || toSegmentDirection == LinkSegmentDirection.Left)
        //        {
        //            strokeA.AddPoint(new PointF(ToArrowEndPoint.X, ToArrowEndPoint.Y - arrowSpacing));
        //            strokeA.AddPoint(ToArrowAnchorPoint);
        //            strokeA.AddPoint(new PointF(ToArrowEndPoint.X, ToArrowEndPoint.Y + arrowSpacing));
        //        }
        //        else if (toSegmentDirection == LinkSegmentDirection.Down || toSegmentDirection == LinkSegmentDirection.Up)
        //        {
        //            strokeA.AddPoint(new PointF(ToArrowEndPoint.X - arrowSpacing, ToArrowEndPoint.Y));
        //            strokeA.AddPoint(ToArrowAnchorPoint);
        //            strokeA.AddPoint(new PointF(ToArrowEndPoint.X + arrowSpacing, ToArrowEndPoint.Y));
        //        }

        //        //Console.WriteLine("To Direction changed from : " + previousToDirection.ToString() + " to : " + toSegmentDirection.ToString());
        //        previousToDirection = toSegmentDirection;
        //    }

        //    strokeB = new GoStroke();
        //    strokeB.Selectable = false;
        //    strokeB.Deletable = false;
        //    strokeB.Visible = vis;
        //    grp.Add(strokeB);
        //    if (AtEnd)
        //    {
        //        if (toSegmentDirection == LinkSegmentDirection.Right || toSegmentDirection == LinkSegmentDirection.Left)
        //        {
        //            strokeB.AddPoint(new PointF(ToArrowEndPoint.X, ToArrowEndPoint.Y - arrowSpacing));
        //            strokeB.AddPoint(ToArrowAnchorPoint);
        //            strokeB.AddPoint(new PointF(ToArrowEndPoint.X, ToArrowEndPoint.Y + arrowSpacing));
        //        }
        //        if (toSegmentDirection == LinkSegmentDirection.Down || toSegmentDirection == LinkSegmentDirection.Up)
        //        {
        //            strokeB.AddPoint(new PointF(ToArrowEndPoint.X - arrowSpacing, ToArrowEndPoint.Y));
        //            strokeB.AddPoint(ToArrowAnchorPoint);
        //            strokeB.AddPoint(new PointF(ToArrowEndPoint.X + arrowSpacing, ToArrowEndPoint.Y));
        //        }
        //    }
        //    else
        //    {
        //        if (fromSegmentDirection == LinkSegmentDirection.Right || fromSegmentDirection == LinkSegmentDirection.Left)
        //        {
        //            strokeB.AddPoint(new PointF(FromArrowEndPoint.X, FromArrowEndPoint.Y - arrowSpacing));
        //            strokeB.AddPoint(FromArrowAnchorPoint);
        //            strokeB.AddPoint(new PointF(FromArrowEndPoint.X, FromArrowEndPoint.Y + arrowSpacing));
        //        }
        //        if (fromSegmentDirection == LinkSegmentDirection.Down || fromSegmentDirection == LinkSegmentDirection.Up)
        //        {
        //            strokeB.AddPoint(new PointF(FromArrowEndPoint.X - arrowSpacing, FromArrowEndPoint.Y));
        //            strokeB.AddPoint(FromArrowAnchorPoint);
        //            strokeB.AddPoint(new PointF(FromArrowEndPoint.X + arrowSpacing, FromArrowEndPoint.Y));
        //        }
        //    }
        //    previousToDirection = toSegmentDirection;
        //    previousFromDirection = fromSegmentDirection;
        //}

        #endregion

        private GoGroup CreateLabel(GoGroup group, bool from, bool one, bool crow, bool circle)
        {
            group = new GoGroup();
            group.Selectable = false;
            group.Deletable = false;
            group.Copyable = true;
            group.Movable = false;
            group.SkipsUndoManager = true;

            float arrowSpacing = 7f;

            PointF point;
            if (from)
            {
                point = FromArrowAnchorPoint;

                if (one)
                {
                    float move = 0f;
                    if (circle)
                    {
                        move = -2f;
                    }

                    GoStroke fromONE = new GoStroke();
                    fromONE.Selectable = false;
                    fromONE.Deletable = false;
                    fromONE.Copyable = true;
                    fromONE.Movable = false;
                    fromONE.SkipsUndoManager = true;
                    if (FromDirection == LinkSegmentDirection.Up)
                    {
                        fromONE.AddPoint(point.X - 7, point.Y + 1 + move);
                        //fromONE.AddPoint(point.X, point.Y - 10);
                        fromONE.AddPoint(point.X + 7, point.Y + 1 + move);
                    }
                    else if (FromDirection == LinkSegmentDirection.Down)
                    {
                        fromONE.AddPoint(point.X - 7, point.Y - 1 - move);
                        //fromONE.AddPoint(point.X, point.Y + 10);
                        fromONE.AddPoint(point.X + 7, point.Y - 1 - move);
                    }
                    else if (FromDirection == LinkSegmentDirection.Left)
                    {
                        fromONE.AddPoint(point.X + 1 + move, point.Y - 7);
                        //fromONE.AddPoint(point.X - 10, point.Y);
                        fromONE.AddPoint(point.X + 1 + move, point.Y + 7);
                    }
                    else if (FromDirection == LinkSegmentDirection.Right)
                    {
                        fromONE.AddPoint(point.X - 1 - move, point.Y - 7);
                        //fromONE.AddPoint(point.X + 10, point.Y);
                        fromONE.AddPoint(point.X - 1 - move, point.Y + 7);
                    }
                    group.Add(fromONE);
                }

                if (crow)
                {
                    GoStroke fromCROW = new GoStroke();
                    fromCROW.Selectable = false;
                    fromCROW.Deletable = false;
                    fromCROW.Copyable = true;
                    fromCROW.Movable = false;
                    fromCROW.SkipsUndoManager = true;
                    switch (FromDirection)
                    {
                        case LinkSegmentDirection.Left:
                        case LinkSegmentDirection.Right:
                            fromCROW.AddPoint(new PointF(FromArrowEndPoint.X, FromArrowEndPoint.Y - arrowSpacing));
                            fromCROW.AddPoint(FromArrowAnchorPoint);
                            fromCROW.AddPoint(new PointF(FromArrowEndPoint.X, FromArrowEndPoint.Y + arrowSpacing));
                            break;
                        case LinkSegmentDirection.Up:
                        case LinkSegmentDirection.Down:
                            fromCROW.AddPoint(new PointF(FromArrowEndPoint.X - arrowSpacing, FromArrowEndPoint.Y));
                            fromCROW.AddPoint(FromArrowAnchorPoint);
                            fromCROW.AddPoint(new PointF(FromArrowEndPoint.X + arrowSpacing, FromArrowEndPoint.Y));
                            break;
                    }
                    group.Add(fromCROW);
                }
            }
            else
            {
                point = ToArrowAnchorPoint;

                if (one)
                {
                    GoStroke toONE = new GoStroke();
                    toONE.Selectable = false;
                    toONE.Deletable = false;
                    toONE.Copyable = true;
                    toONE.Movable = false;
                    toONE.SkipsUndoManager = true;
                    if (ToDirection == LinkSegmentDirection.Up)
                    {
                        toONE.AddPoint(point.X - 7, point.Y + 1);
                        //fromONE.AddPoint(point.X, point.Y - 10);
                        toONE.AddPoint(point.X + 7, point.Y + 1);
                    }
                    else if (ToDirection == LinkSegmentDirection.Down)
                    {
                        toONE.AddPoint(point.X - 7, point.Y - 1);
                        //fromONE.AddPoint(point.X, point.Y + 10);
                        toONE.AddPoint(point.X + 7, point.Y - 1);
                    }
                    else if (ToDirection == LinkSegmentDirection.Left)
                    {
                        toONE.AddPoint(point.X + 1, point.Y - 7);
                        //fromONE.AddPoint(point.X - 10, point.Y);
                        toONE.AddPoint(point.X + 1, point.Y + 7);
                    }
                    else if (ToDirection == LinkSegmentDirection.Right)
                    {
                        toONE.AddPoint(point.X - 1, point.Y - 7);
                        //fromONE.AddPoint(point.X + 10, point.Y);
                        toONE.AddPoint(point.X - 1, point.Y + 7);
                    }
                    group.Add(toONE);
                }

                if (crow)
                {
                    GoStroke toCROW = new GoStroke();
                    toCROW.Selectable = false;
                    toCROW.Deletable = false;
                    toCROW.Copyable = true;
                    toCROW.Movable = false;
                    toCROW.SkipsUndoManager = true;
                    switch (ToDirection)
                    {
                        case LinkSegmentDirection.Left:
                        case LinkSegmentDirection.Right:
                            toCROW.AddPoint(new PointF(ToArrowEndPoint.X, ToArrowEndPoint.Y - arrowSpacing));
                            toCROW.AddPoint(ToArrowAnchorPoint);
                            toCROW.AddPoint(new PointF(ToArrowEndPoint.X, ToArrowEndPoint.Y + arrowSpacing));
                            break;
                        case LinkSegmentDirection.Up:
                        case LinkSegmentDirection.Down:
                            toCROW.AddPoint(new PointF(ToArrowEndPoint.X - arrowSpacing, ToArrowEndPoint.Y));
                            toCROW.AddPoint(ToArrowAnchorPoint);
                            toCROW.AddPoint(new PointF(ToArrowEndPoint.X + arrowSpacing, ToArrowEndPoint.Y));
                            break;
                    }
                    group.Add(toCROW);
                }
            }

            if (circle)
            {
                GoEllipse ell = new GoEllipse();
                ell.Selectable = false;
                ell.Movable = false;
                ell.Deletable = false;
                ell.Copyable = true;
                ell.SkipsUndoManager = true;
                ell.Height = arrowSpacing * 2;
                ell.Width = arrowSpacing * 2;
                if (from)
                {
                    switch (FromDirection)
                    {
                        case LinkSegmentDirection.Left:
                            ell.Center = new PointF(point.X + arrowSpacing, point.Y);
                            break;
                        case LinkSegmentDirection.Right:
                            ell.Center = new PointF(point.X - arrowSpacing, point.Y);
                            break;
                        case LinkSegmentDirection.Up:
                            ell.Center = new PointF(point.X, point.Y + arrowSpacing);
                            break;
                        case LinkSegmentDirection.Down:
                            ell.Center = new PointF(point.X, point.Y - arrowSpacing);
                            break;
                    }
                }
                else
                {
                    switch (ToDirection)
                    {
                        case LinkSegmentDirection.Left:
                            ell.Center = new PointF(point.X - arrowSpacing, point.Y);
                            break;
                        case LinkSegmentDirection.Right:
                            ell.Center = new PointF(point.X + arrowSpacing, point.Y);
                            break;
                        case LinkSegmentDirection.Up:
                            ell.Center = new PointF(point.X, point.Y - arrowSpacing);
                            break;
                        case LinkSegmentDirection.Down:
                            ell.Center = new PointF(point.X, point.Y + arrowSpacing);
                            break;
                    }
                }

                group.Add(ell);
            }

            return group;
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

        //private LinkSegmentDirection previousToDirection;
        //private LinkSegmentDirection previousFromDirection;

        //public override GoObject CopyObject(GoCopyDictionary env)
        //{
        //    //calculated = false;

        //    return base.CopyObject(env);
        //}
        public override void CopyObjectDelayed(GoCopyDictionary env, GoObject newobj)
        {
            calculated = false;
            //(newobj as QRealLink).calculated = false;

            base.CopyObjectDelayed(env, newobj);
        }

        public override void DoResize(GoView view, RectangleF origRect, PointF newPoint, int whichHandle, GoInputState evttype, SizeF min, SizeF max)
        {
            base.DoResize(view, origRect, newPoint, whichHandle, evttype, min, max);
            AvoidsNodes = false;
            AdjustingStyle = GoLinkAdjustingStyle.End;
        }

        //public override void CalculateStroke()
        //{
        //    base.CalculateStroke();
        //}

        public void ForceCalculate()
        {
            calculated = false;
            CalculateStroke();
        }

        //public override Pen Pen
        //{
        //    get
        //    {
        //        return base.Pen;
        //    }
        //    set
        //    {
        //        //if (value.Width > 1)
        //        //    Core.Log.WriteLog("QRealLink::Width > 1" + Environment.NewLine + Environment.StackTrace.ToString());
        //        base.Pen = value;
        //    }
        //}

        public override void ChangeValue(GoChangedEventArgs e, bool undo)
        {
            try
            {
                string colorName = "";
                if (e.OldValue != null)
                {
                    foreach (System.Reflection.PropertyInfo i in e.OldValue.GetType().GetProperties())
                    {
                        if (i.PropertyType == typeof(Color))
                        {
                            colorName = i.Name;
                            break;
                        }
                    }
                }
                else if (e.NewValue != null)
                {
                    foreach (System.Reflection.PropertyInfo i in e.NewValue.GetType().GetProperties())
                    {
                        if (i.PropertyType == typeof(Color))
                        {
                            colorName = i.Name;
                            break;
                        }
                    }
                }
                if (colorName.Length > 0)
                    (Parent as QLink).PenColorBeforeCompare = undo ? ((Color)e.OldValue.GetType().GetProperty(colorName).GetValue(e.OldValue, null)) : ((Color)e.NewValue.GetType().GetProperty(colorName).GetValue(e.NewValue, null));
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.ToString());
#endif
            }

            base.ChangeValue(e, undo);
        }

        public override bool Shadowed
        {
            get
            {
                return false;
                //return base.Shadowed;
            }
            set
            {
                base.Shadowed = false;
                //base.Shadowed = value;
            }
        }
    }
}