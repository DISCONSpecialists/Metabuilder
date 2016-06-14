using System;
using System.Collections.Generic;
using System.Drawing;
using MetaBuilder.Core;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Behaviours;
using MetaBuilder.Graphing.Shapes.Behaviours.Observers;
using MetaBuilder.Graphing.Shapes.Primitives;
using MetaBuilder.Graphing.Utilities;
using MetaBuilder.Meta;
using Northwoods.Go;

namespace ShapeBuilding
{
    public class ReportShapeBuilder
    {
        public GraphNode GetShape()
        {
            CollapsibleNode crnode = new CollapsibleNode();
            GoGroup node = StorageManipulator.FileSystemManipulator.LoadSymbol(@"D:\development\Code\DISCON Metabuilder\AppFolder\MetaData\Symbols\Data_view.sym") as GoGroup;
            float f = 0f;
            float fRight = 0f;
            float fHeight = 0f;
            GoObject blueRect = null;
            foreach (GoObject o in node)
            {
                if (o is GradientRoundedRectangle)
                {
                    o.Remove();

                    f = o.Width;
                    fRight = o.Right;
                    fHeight = o.Height;
                    blueRect = o;
                }
            }
            GradientRoundedRectangle whiteRect = new GradientRoundedRectangle();
            whiteRect.Brush = Brushes.White;
            whiteRect.Height = crnode.Height;
            whiteRect.Width = 110;
            whiteRect.Position = new PointF(fRight, 10);
            whiteRect.Height = fHeight;
            whiteRect.Selectable = false;
            whiteRect.Reshapable = false;
            whiteRect.Movable = false;
            whiteRect.DragsNode = true;
            f += whiteRect.Width;
            crnode.EditMode = true;
            #region Ports
            List<QuickPort> portsToAdd = new List<QuickPort>();



            QuickPort leftPortTop = new QuickPort(false);
            leftPortTop.Position = new PointF(0, 27.5f);
            leftPortTop.IncomingLinksDirection = 180;
            leftPortTop.OutgoingLinksDirection = 180;
            portsToAdd.Add(leftPortTop);


            QuickPort topLeftPort = new QuickPort(false);
            topLeftPort.Position = new PointF(30 - topLeftPort.Width / 2, 5);
            topLeftPort.IncomingLinksDirection = 270;
            topLeftPort.OutgoingLinksDirection = 270;
            portsToAdd.Add(topLeftPort);

            QuickPort topLeftPort2 = new QuickPort(false);
            topLeftPort2.Position = new PointF(60 - topLeftPort2.Width / 2, 5);
            topLeftPort2.IncomingLinksDirection = 270;
            topLeftPort2.OutgoingLinksDirection = 270;
            portsToAdd.Add(topLeftPort2);



            QuickPort topRightPort = new QuickPort(false);
            topRightPort.Position = new PointF(120 - topRightPort.Width / 2, 5);
            topRightPort.IncomingLinksDirection = 270;
            topRightPort.OutgoingLinksDirection = 270;
            portsToAdd.Add(topRightPort);

            QuickPort topRightPort2 = new QuickPort(false);
            topRightPort2.Position = new PointF(150 - topRightPort2.Width / 2, 5);
            topRightPort2.IncomingLinksDirection = 270;
            topRightPort2.OutgoingLinksDirection = 270;
            portsToAdd.Add(topRightPort2);
            QuickPort leftBottomPort = new QuickPort(false);
            leftBottomPort.Position = new PointF(0, 72.5f);
            leftBottomPort.OutgoingLinksDirection = 180;
            leftBottomPort.IncomingLinksDirection = 180;
            portsToAdd.Add(leftBottomPort);
            crnode.AddHandle();


            QuickPort leftPort = new QuickPort(false);
            leftPort.Position = new PointF(0, 50);
            leftPort.OutgoingLinksDirection = 180;
            leftPort.IncomingLinksDirection = 180;
            portsToAdd.Add(leftPort);

            QuickPort bottomPortLeft = new QuickPort(false);
            bottomPortLeft.Position = new PointF(45 - topLeftPort2.Width / 2, whiteRect.Height + 5);
            bottomPortLeft.IncomingLinksDirection = 0;
            bottomPortLeft.OutgoingLinksDirection = 0;
            portsToAdd.Add(bottomPortLeft);

            QuickPort bottomPortLeft2 = new QuickPort(false);
            bottomPortLeft2.Position = new PointF(75 - topLeftPort2.Width / 2, whiteRect.Height + 5);
            bottomPortLeft2.IncomingLinksDirection = 0;
            bottomPortLeft2.OutgoingLinksDirection = 0;
            portsToAdd.Add(bottomPortLeft2);


            QuickPort bottomPortRight = new QuickPort(false);
            bottomPortRight.Position = new PointF(135 - topLeftPort2.Width / 2, whiteRect.Height + 5);
            bottomPortRight.IncomingLinksDirection = 0;
            bottomPortRight.OutgoingLinksDirection = 0;
            portsToAdd.Add(bottomPortRight);

            QuickPort bottomPortRight2 = new QuickPort(false);
            bottomPortRight2.Position = new PointF(165 - topLeftPort2.Width / 2, whiteRect.Height + 5);
            bottomPortRight2.IncomingLinksDirection = 0;
            bottomPortRight2.OutgoingLinksDirection = 0;
            portsToAdd.Add(bottomPortRight2);
            foreach (QuickPort prt in portsToAdd)
            {
                prt.Selectable = false;
                prt.AutoRescales = false;
                prt.Resizable = false;
                prt.Style = GoPortStyle.Rectangle;
                prt.Brush = Brushes.LightGray;
                prt.Pen = new Pen(Color.Gray);
                prt.DragsNode = true;
                prt.UserFlags = 5;
                crnode.Add(prt);
            }






            crnode.Add(whiteRect);
            crnode.Add(blueRect);
            AddWhiteStroke(crnode, whiteRect);

            portsToAdd.Clear();
            QuickPort rightPortTop = new QuickPort(false);
            rightPortTop.Position = new PointF(170, 27.5f);
            rightPortTop.IncomingLinksDirection = 0;
            rightPortTop.OutgoingLinksDirection = 0;
            portsToAdd.Add(rightPortTop);


            QuickPort rightBottomPort = new QuickPort(false);
            rightBottomPort.Position = new PointF(170, 72.5f);
            rightBottomPort.IncomingLinksDirection = 0; AddWhiteStroke(crnode, whiteRect);
            rightBottomPort.OutgoingLinksDirection = 0;
            portsToAdd.Add(rightBottomPort);
            AddListOfPorts(crnode, portsToAdd);
            QuickPort bottomPort = new QuickPort(false);
            bottomPort.Position = new PointF(105 - topLeftPort2.Width / 2, whiteRect.Height + 5);
            bottomPort.IncomingLinksDirection = 0;
            bottomPort.OutgoingLinksDirection = 0;
            portsToAdd.Add(bottomPort);

            QuickPort topPort = new QuickPort(false);
            topPort.Position = new PointF(90 - topPort.Width / 2, 5);
            topPort.IncomingLinksDirection = 270;
            topPort.OutgoingLinksDirection = 270;
            portsToAdd.Add(topPort);

            QuickPort rightPort = new QuickPort(false);
            rightPort.Position = new PointF(170, 50);
            rightPort.IncomingLinksDirection = 0;
            rightPort.OutgoingLinksDirection = 0;
            portsToAdd.Add(rightPort);

            AddListOfPorts(crnode, portsToAdd);



            topPort.OutgoingLinksDirection = 270;
            topPort.IncomingLinksDirection = 270;
            #endregion
            crnode.BindingInfo = new BindingInfo();
            crnode.BindingInfo.BindingClass = "Report";
            crnode.BindingInfo.Bindings.Add("Report", "Name");
            crnode.CreateMetaObject(null, EventArgs.Empty);

            BoundLabel txt = new BoundLabel();
            txt.Name = "Report";
            txt.Text = "Report";
            txt.DragsNode = true;
            txt.Editable = true;
            txt.Selectable = true;
            txt.Resizable = true;
            txt.Multiline = false;
            txt.Alignment = 0;
            txt.Wrapping = true;
            txt.Clipping = false;
            txt.StringTrimming = StringTrimming.None;
            txt.WrappingWidth = 160;
            txt.Width = whiteRect.Width - 20;
            txt.AutoResizes = false;
            txt.Position = new PointF(whiteRect.Left + 5, 12.5f);
            txt.Deletable = false;
            crnode.Add(txt);
            crnode.Grid.Selectable = false;

            txt.AutoResizes = false;
            txt.AutoRescales = false;
            crnode.CreateBody();

            crnode.ItemWidth = 170;
            crnode.ChildIndentation = 5;
            crnode.List.Position = new PointF(5, crnode.Height);
            RepeaterSection lg = new RepeaterSection();
            lg.ChildPortsEnabled = true;
            CollapsingRecordNodeItem headeritem0 = crnode.MakeItem("", "Report Attributes", true);
            (headeritem0.Label as BoundLabel).SetEditable(false);
            lg.Add(headeritem0);
            crnode.List.Add(lg);
            lg.Position = new PointF(40, 40);
            lg.Insertable = true;
            lg.ChildIndentation = 2;
            crnode.List.Position = new PointF(5, whiteRect.Bottom + 1);
            crnode.Grid.Selectable = false;


            leftPort.DragsNode = true;
            topPort.DragsNode = true;
            rightPort.DragsNode = true;

            AddBehaviourToBottomPort(crnode.RepeaterSections[0], bottomPortLeft);
            AddBehaviourToBottomPort(crnode.RepeaterSections[0], bottomPortLeft2);
            AddBehaviourToBottomPort(crnode.RepeaterSections[0], bottomPort);
            AddBehaviourToBottomPort(crnode.RepeaterSections[0], bottomPortRight);
            AddBehaviourToBottomPort(crnode.RepeaterSections[0], bottomPortRight2);

            crnode.Initialize();
            lg.SetAllItemWidth(170);
            crnode.EditMode = false;
            RepeaterBindingInfo rbinfo = new RepeaterBindingInfo();
            crnode.BindingInfo = new BindingInfo();
            crnode.BindingInfo.BindingClass = "Report";
            crnode.BindingInfo.Bindings.Add("Report", "Name");
            crnode.BindingInfo.RepeaterBindings.Add(rbinfo);
            // hack!
            Variables.Instance.ConnectionString = "server=.\\SqlExpress;Initial Catalog=MetaBuilder;Integrated Security=true";
            Variables.Instance.SourceCodePath = @"C:\Program Files\Discon Specialists\MetaBuilder\MetaData\SourceFiles";

            rbinfo.Association = new Association();
            rbinfo.Association.ID = 3513;
            rbinfo.Association.ParentClass = "Report";
            rbinfo.Association.IsDefault = true;
            rbinfo.BoundProperty = "Name";
            rbinfo.Association.ChildClass = "Attribute";
            rbinfo.Association.Caption = "Attributes";
            rbinfo.RepeaterID = new Guid();
            rbinfo.RepeaterName = rbinfo.Association.Caption;
            crnode.RepeaterSections[0].Name = rbinfo.Association.Caption;






            Color outer = Color.FromArgb(255, 51, 51, 51);

            Color inner = Color.FromArgb(255, 128, 128, 128);

            topPort.Brush = new SolidBrush(inner);
            topPort.Pen = new Pen(outer);
            leftPort.Brush = new SolidBrush(inner);
            leftPort.Pen = new Pen(outer);
            rightPort.Brush = new SolidBrush(inner);
            rightPort.Pen = new Pen(outer);
            crnode.EditMode = false;



            crnode.HookupEvents();

            return crnode;

        }

        private static void AddBehaviourToBottomPort(GoObject o, QuickPort port)
        {
            port.Position = new PointF(port.Position.X, o.Bottom - 5);
            AnchorPositionBehaviour apbehav1 = new AnchorPositionBehaviour(o as IIdentifiable, port, PositionLockLocation.BottomCenter);
            port.Manager.AddBehaviour(apbehav1);
            o.AddObserver(port);
            port.DragsNode = true;
        }
        private static void AddWhiteStroke(CollapsibleNode crnode, GradientRoundedRectangle whiteRect)
        {
            QuickStroke qs = new QuickStroke();
            qs.AddPoint(new PointF(whiteRect.Right, whiteRect.Top + 1));
            qs.AddPoint(new PointF(whiteRect.Right, whiteRect.Bottom - 1));
            qs.Width = 3f;
            //qs.Selectable = false;
            qs.Deletable = false;
            qs.DragsNode = true;
            qs.Reshapable = false;
            qs.Resizable = false;
            qs.AutoRescales = false;
            qs.Pen = new Pen(Color.White);
            qs.InvalidateViews();
            qs.Highlight = false;
            qs.HighlightPen = null;
            qs.Shadowed = false;
            qs.PenWidth = 3;
            crnode.Add(qs);
        }

        private static void AddListOfPorts(CollapsibleNode crnode, List<QuickPort> portsToAdd)
        {
            foreach (QuickPort prt in portsToAdd)
            {
                if (prt.UserFlags != 5)
                {
                    prt.Selectable = false;
                    prt.AutoRescales = false;
                    prt.Resizable = false;
                    prt.Style = GoPortStyle.Rectangle;
                    prt.Brush = Brushes.LightGray;
                    prt.Pen = new Pen(Color.Gray);
                    prt.DragsNode = true;
                    prt.UserFlags = 5; //signifies that it has been added already
                    crnode.Add(prt);
                }

            }
        }

        private static void AddBehaviourToBottomPort(RepeaterSection grect, QuickPort port)
        {
            port.Position = new PointF(port.Position.X, grect.Bottom - 5);
            AnchorPositionBehaviour apbehav1 = new AnchorPositionBehaviour(grect, port, PositionLockLocation.BottomCenter);
            port.Manager.AddBehaviour(apbehav1);
            grect.AddObserver(port);
            port.DragsNode = true;

        }


        private static void AddStroke(CollapsibleNode crnode, GradientRoundedRectangle grect, float Xcoord, GoGroup grp)
        {
            QuickStroke qs = new QuickStroke();
            qs.AddPoint(new PointF(Xcoord, grect.Bottom + 1.0f));
            qs.AddPoint(new PointF(Xcoord, grect.Top));
            qs.Selectable = false;
            qs.Deletable = false;
            qs.DragsNode = true;
            qs.Reshapable = false;
            qs.Resizable = false;
            qs.AutoRescales = false;
            qs.InvalidateViews();
            grp.Add(qs);


        }
    }
}
