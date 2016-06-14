using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;
using MetaBuilder.Meta;
using System.Drawing;
namespace ShapeBuilding.Links
{
    public class ArrowBuilder
    {
        public GoGroup  CreateLabel(LinkAssociationType linktype)
        {
            switch (linktype)
            {
                case LinkAssociationType.Mapping:
                    return CreateMappingLabel();
                case LinkAssociationType.Concurrent:
                    return CreateConcurrentLabel();
                case LinkAssociationType.Auxiliary:
                    return CreateAuxLabel();
                case LinkAssociationType.Create:
                case LinkAssociationType.Read:
                case LinkAssociationType.Delete:
                case LinkAssociationType.Update:
                case LinkAssociationType.Maintain:
                    return CreateCenterBox(linktype);
                case LinkAssociationType.Start:
                    return CreateStartLabel();
                case LinkAssociationType.Classification:
                case LinkAssociationType.Stop:
                    return CreateCircle();
                case LinkAssociationType.Suspend:
                    return CreateSuspendLabel();
                case LinkAssociationType.Resume:
                    return CreateResumeLabel();
                case LinkAssociationType.Interrupt:
                    return CreateAuxLabel();
                case LinkAssociationType.Synchronise:
                    return CreateSynchronizedLabel();
                case LinkAssociationType.NonConcurrent:
                    return CreateNonConcurrentLabel();
             }
            return null;
        }
        private GoGroup CreateMappingLabel()
        {
            GoRectangle rect = new GoRectangle();
            rect.Height = 13;
            rect.Width = 13;
            rect.Pen = new Pen(Color.Black, 0.5f);
            rect.Brush = Brushes.White;//new SolidBrush(Color.White);
            GoGroup grpMapMidlabel = new GoGroup();
            grpMapMidlabel.Add(rect);
            rect.Selectable = false;
            grpMapMidlabel.Selectable = false;
            return grpMapMidlabel;

        }
        private GoGroup CreateConcurrentLabel()
        {
            GoGroup grp = new GoGroup(); // need to use a group, more than 1 shape
            GoStroke strLeft = new GoStroke();
            strLeft.AddPoint(0, 0);
            strLeft.AddPoint(0, 12);
            grp.Add(strLeft);
            GoStroke strRight = new GoStroke();
            strRight.AddPoint(12, 0);
            strRight.AddPoint(12, 12);
            strRight.Selectable = false;
            strLeft.Selectable = false;
            grp.Add(strRight);
            grp.Selectable = false;

            return grp;
        }
        private GoGroup CreateAuxLabel()
        {
            GoGroup grp = new GoGroup();
            GoEllipse ellipse = new GoEllipse();
            ellipse.Width = 13;
            ellipse.Height = 13;
            ellipse.Brush = Brushes.White;
            ellipse.DragsNode = true;
            ellipse.Selectable = false;
            grp.Add(ellipse);
            GoStroke strLeftTopToBottomRight = new GoStroke();
            strLeftTopToBottomRight.AddPoint(ellipse.Left + 3, ellipse.Top + 3);
            strLeftTopToBottomRight.AddPoint(ellipse.Right - 3, ellipse.Bottom - 3);
            strLeftTopToBottomRight.DragsNode = true;
            strLeftTopToBottomRight.Selectable = false;
            grp.Add(strLeftTopToBottomRight);
            GoStroke strRightTopToBottomLeft = new GoStroke();
            strRightTopToBottomLeft.AddPoint(new PointF(ellipse.Right - 3, ellipse.Top + 3));
            strRightTopToBottomLeft.AddPoint(new PointF(ellipse.Left + 3, ellipse.Bottom - 3));
            strRightTopToBottomLeft.DragsNode = true;
            strRightTopToBottomLeft.Selectable = false;
            grp.Add(strRightTopToBottomLeft);
            return grp;
        }
        private GoGroup CreateCenterBox(LinkAssociationType linkType)
        {
            GoGroup grp = new GoGroup();
            GoText txt = new GoText();
            txt.Text = linkType.ToString().ToUpper();
            txt.Editable = false;
            txt.Selectable = false;
            txt.FontSize = 6.5f;
            grp.Add(txt);
            grp.Selectable = false;
            GoRoundedRectangle rect = new GoRoundedRectangle();
            rect.Pen = new Pen(Color.Gray, 1f);
            rect.Corner = new SizeF(2f, 2f);
            rect.Selectable = false;
            rect.Resizable = false;
            rect.Brush = Brushes.White;
            RectangleF grpBounds = grp.Bounds;
            grpBounds.Inflate(4, 1);
            rect.Bounds = grpBounds;
            rect.Center = grp.Center;
            txt.Center = grp.Center;
            grp.InsertBefore(txt, rect);
            return grp;
        }
        private GoGroup CreateStartLabel()
        {
            GoGroup grpEllipseStart = new GoGroup();
            GoEllipse ellipse = new GoEllipse();
            ellipse.Brush = Brushes.Black;
            ellipse.Height = 13;
            ellipse.Width = 13;
            ellipse.Selectable = false;
            grpEllipseStart.Add(ellipse);
            return grpEllipseStart;
        }
        private GoGroup CreateCircle()
        {
            GoGroup grpEllipseStart = new GoGroup();
            GoEllipse ellipse = new GoEllipse();
            ellipse.Brush = Brushes.White;
            ellipse.Height = 13;
            ellipse.Width = 13;
            ellipse.Selectable = false;
            grpEllipseStart.Add(ellipse);
            return grpEllipseStart;
        }
        private GoGroup CreateSuspendLabel()
        {
            return CreateSuspendResumeLabel(270);
        }
        private GoGroup CreateResumeLabel()
        {
            return CreateSuspendResumeLabel(90);
        }
        private GoGroup CreateSuspendResumeLabel(float angle)
        {
            GoEllipse ellipse = new GoEllipse();
            ellipse.Brush = Brushes.Black;
            ellipse.Height = 13;
            ellipse.Width = 13;
            GoGroup grpSuspendResume = new GoGroup();
            grpSuspendResume.Add(ellipse);
            GoPie pieOverlay = new GoPie();
            pieOverlay.Brush = Brushes.White;
            pieOverlay.Width = 12;
            pieOverlay.Height = 12;
            pieOverlay.StartAngle = angle;
            pieOverlay.SweepAngle = 180;
            pieOverlay.ResizableEndAngle = false;
            pieOverlay.ResizableStartAngle = false;
            pieOverlay.Reshapable = false;
            pieOverlay.Selectable = false;
            ellipse.Selectable = false;
            grpSuspendResume.Selectable = false;
            grpSuspendResume.Add(pieOverlay);
            grpSuspendResume.Selectable = false;
            return grpSuspendResume;
        }
        private GoGroup CreateSynchronizedLabel()
        {
            GoGroup grpConcurrent = CreateConcurrentLabel();
            GoText txtSync = new GoText();
            txtSync.Text = "S";
            txtSync.Selectable = false;
            txtSync.Movable = false;
            txtSync.Editable = false;
            txtSync.FontSize = 8;
            txtSync.Alignment = GoObject.Middle;
            txtSync.Center = grpConcurrent.Center;// new PointF(grpConcurrent.Position.X + grpConcurrent.Width / 2 - 4, grpConcurrent.Position.Y);
            grpConcurrent.Add(txtSync);
            return grpConcurrent;
        }
        private GoGroup CreateNonConcurrentLabel()
        {
            GoGroup grpConcurrent = CreateConcurrentLabel();
            GoStroke strDiagonal = new GoStroke();
            strDiagonal.Selectable = false;
            strDiagonal.AddPoint(grpConcurrent.Position.X + grpConcurrent.Width + 5, grpConcurrent.Position.Y);
            strDiagonal.AddPoint(grpConcurrent.Position.X - 5, grpConcurrent.Position.Y + grpConcurrent.Height);
            grpConcurrent.Add(strDiagonal);
            return grpConcurrent;
        }
      
        
    }
}
