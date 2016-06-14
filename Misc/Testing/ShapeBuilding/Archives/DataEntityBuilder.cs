using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using MetaBuilder.Core;
using MetaBuilder.Graphing.Formatting;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Behaviours.Internal;
using MetaBuilder.Graphing.Shapes.Behaviours.Observers;
using MetaBuilder.Graphing.Shapes.Primitives;
using MetaBuilder.Meta;
using Northwoods.Go;

namespace ShapeBuilding
{
    public class DataEntityBuilder
    {
        public GraphNode GetShape()
        {
            CollapsibleNode crnode = new CollapsibleNode();

            crnode.EditMode = true;
            #region Ports
            List<QuickPort> portsToAdd = new List<QuickPort>();
            
          

            QuickPort topPortLeft = new QuickPort(false);
            topPortLeft.Position = new PointF(15, 18);
            topPortLeft.IncomingLinksDirection = 270;
            topPortLeft.OutgoingLinksDirection = 270;
            portsToAdd.Add(topPortLeft);

            QuickPort topPortLeft2 = new QuickPort(false);
            topPortLeft2.Position = new PointF(45, 4);
            topPortLeft2.OutgoingLinksDirection = 270;
            topPortLeft2.IncomingLinksDirection = 270;
            portsToAdd.Add(topPortLeft2);

            QuickPort topPortRight = new QuickPort(false);
            topPortRight.Position = new PointF(155, 18);
            topPortRight.IncomingLinksDirection = 270;
            topPortRight.OutgoingLinksDirection = 270;
            portsToAdd.Add(topPortRight);

            QuickPort topPortRight2 = new QuickPort(false);
            topPortRight2.Position = new PointF(125, 4);
            topPortRight2.IncomingLinksDirection = 270;
            topPortRight2.OutgoingLinksDirection = 270;
            portsToAdd.Add(topPortRight2);

            


         
            topPortLeft.OutgoingLinksDirection = 270;
            topPortLeft.IncomingLinksDirection = 270;
            topPortRight.OutgoingLinksDirection = 270;
            topPortRight.IncomingLinksDirection = 270;
            topPortLeft2.OutgoingLinksDirection = 270;
            topPortLeft2.IncomingLinksDirection = 270;
            topPortRight2.OutgoingLinksDirection = 270;
            topPortRight2.IncomingLinksDirection = 270;
         
            
            // Bottom port 1st appearance...
           

            QuickPort prtBottomLeftMost = new QuickPort(false);
            prtBottomLeftMost.Position = new PointF(25, 50);
            prtBottomLeftMost.OutgoingLinksDirection = 90;
            prtBottomLeftMost.IncomingLinksDirection = 90;
            portsToAdd.Add(prtBottomLeftMost);

            QuickPort prtBottomLeft = new QuickPort(false);
            prtBottomLeft.Position = new PointF(55, 50);
            prtBottomLeft.IncomingLinksDirection = 90;
            prtBottomLeft.OutgoingLinksDirection = 90;
            portsToAdd.Add(prtBottomLeft);


            QuickPort prtBottomRight = new QuickPort(false);
            prtBottomRight.Position = new PointF(115, 50);
            prtBottomRight.IncomingLinksDirection = 90;
            prtBottomRight.OutgoingLinksDirection = 90;
            portsToAdd.Add(prtBottomRight);

            QuickPort prtBottomRightMost = new QuickPort(false);
            prtBottomRightMost.Position = new PointF(145, 50);
            prtBottomRightMost.IncomingLinksDirection = 90;
            prtBottomRightMost.OutgoingLinksDirection = 90;
            portsToAdd.Add(prtBottomRightMost);


            #endregion
            crnode.BindingInfo = new BindingInfo();
            crnode.BindingInfo.BindingClass = "Entity";
            crnode.BindingInfo.Bindings.Add("Entity", "Name");
            crnode.CreateMetaObject(null, EventArgs.Empty);

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

            GradientEllipse ellipse = new GradientEllipse();
            GradientBehaviour gbehaviour = new GradientBehaviour();
            gbehaviour.MyBrush = new ShapeGradientBrush();
            gbehaviour.MyBrush.GradientType = GradientType.Horizontal;
            gbehaviour.MyBrush.InnerColor = Color.SteelBlue;
            gbehaviour.MyBrush.OuterColor = Color.White;
            ellipse.Manager.AddBehaviour(gbehaviour);
            ellipse.Width = 170;
            ellipse.Height = 100;
            ellipse.DragsNode = true;
            ellipse.Position = new PointF(5, 5);
           // ellipse.Corner = new SizeF(20, 20);
            crnode.Add(ellipse);
            BoundLabel txt = new BoundLabel();
            txt.Name = "Entity";
            txt.Text = "Entity";
            txt.DragsNode = true;
            txt.Editable = true;
            txt.Selectable = true;
            txt.Resizable = true;
            txt.Alignment = 1;
            txt.Multiline = false;
            txt.Wrapping = true;
            txt.WrappingWidth = 130;
            txt.Width = ellipse.Width - 20;
            txt.Height = ellipse.Height - 50;
            txt.AutoResizes = false;
            txt.Position = new PointF(ellipse.Left + 10, 25);
            crnode.Add(txt);

            QuickPort leftPort = new QuickPort(false);
            leftPort.Position = new PointF(0, 45);
            portsToAdd.Add(leftPort);


           
            QuickPort topPort = new QuickPort(false);
            topPort.Position = new PointF(180 / 2 - topPort.Width / 2, 0);
            portsToAdd.Add(topPort);
            QuickPort rightPort = new QuickPort(false);
            rightPort.Position = new PointF(170, 45);
            portsToAdd.Add(rightPort);
            QuickPort prtBottom = new QuickPort(false);
            prtBottom.Position = new PointF(85, 50);
            portsToAdd.Add(prtBottom);
            prtBottom.IncomingLinksDirection = 90;
            prtBottom.OutgoingLinksDirection = 90;

            topPort.OutgoingLinksDirection = 270;
            topPort.IncomingLinksDirection = 270;
            leftPort.IncomingLinksDirection = 180;
            leftPort.OutgoingLinksDirection = 180;
            rightPort.OutgoingLinksDirection = 0;
            rightPort.IncomingLinksDirection = 0;
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
                    prt.UserFlags = 5;
                    crnode.Add(prt);
                }
            }
          
            crnode.Grid.Selectable = false;

            ellipse.Selectable = false;
            ellipse.Deletable = false;

            ellipse.Resizable = false;
            ellipse.Selectable = false;
            ellipse.AutoRescales = false;
            txt.AutoResizes = false;
            txt.AutoRescales = false;
            BoundLabel lblOorE = new BoundLabel();
            ArrayList alist = new ArrayList();
            alist.Add("O");
            alist.Add("E");
            lblOorE.Text = "";
            lblOorE.Choices = alist;
            lblOorE.EditorStyle = GoTextEditorStyle.ComboBox;
            lblOorE.DropDownList = true;
            lblOorE.Position = new PointF(165, 0);
            crnode.Add(lblOorE);
            crnode.CreateBody();

            crnode.ItemWidth = 160;
            crnode.ChildIndentation = 0;
            crnode.List.Position = new PointF(10, crnode.Height-38);


            RepeaterSection lg = new RepeaterSection();
            CollapsingRecordNodeItem headeritem0 = crnode.MakeItem("", "Key Attributes", true);
            (headeritem0.Label as BoundLabel).SetEditable(false);
            lg.Add(headeritem0);
            crnode.List.Add(lg);
            lg.Insertable = true;
            lg.ChildIndentation = 2;
            crnode.List.Position = new PointF(10, ellipse.Bottom - 38);
            crnode.Grid.Selectable = false;

            RepeaterSection lstDesc = new RepeaterSection();
            CollapsingRecordNodeItem headeritem1 = crnode.MakeItem("", "Descriptive Attributes", true);
            (headeritem1.Label as BoundLabel).SetEditable(false);
            lstDesc.Add(headeritem1);
      
            lstDesc.Insertable = true;
            lstDesc.ChildIndentation = 2;
            crnode.List.Add(lstDesc);

            GradientRoundedRectangle rect = new GradientRoundedRectangle();
            rect.Brush = Brushes.Gray;
            rect.Corner = new SizeF(0, 0);
            crnode.List.Add(rect);
            rect.Width = 160;
            rect.Height = 6;
            rect.Selectable = false;
            rect.DragsNode = true;
            rect.Deletable = false;
            rect.Reshapable = false;
            rect.Resizable = false;
            rect.Pen = new Pen(Color.Gray);

            GradientBehaviour gbehaviourRect = new GradientBehaviour();
            gbehaviourRect.MyBrush = new ShapeGradientBrush();
            gbehaviourRect.MyBrush.GradientType = GradientType.Horizontal;
            gbehaviourRect.MyBrush.InnerColor = Color.SteelBlue;
            gbehaviourRect.MyBrush.OuterColor = Color.White;
            rect.Manager.AddBehaviour(gbehaviourRect);


          

            prtBottom.Position = new PointF(85, rect.Bottom);
            prtBottomLeft.Position = new PointF(55, rect.Bottom);
            prtBottomLeftMost.Position = new PointF(25, rect.Bottom);
            prtBottomRight.Position = new PointF(115, rect.Bottom);
            prtBottomRightMost.Position = new PointF(145, rect.Bottom);

            Color outer = Color.FromArgb(255, 51, 51, 51);

            Color inner = Color.FromArgb(255, 128, 128, 128);



            prtBottom.Brush = new SolidBrush(inner);
            prtBottom.Pen = new Pen(outer);
            topPort.Brush = new SolidBrush(inner);
            topPort.Pen = new Pen(outer);
            leftPort.Brush = new SolidBrush(inner);
            leftPort.Pen = new Pen(outer);
            rightPort.Brush = new SolidBrush(inner);
            rightPort.Pen = new Pen(outer);


            //bottomPort.Position = new PointF(85, lstDesc.Bottom);
          
            //ellipse.AddObserver(bottomPort);

        
            leftPort.DragsNode = true;
            topPort.DragsNode = true;
            rightPort.DragsNode = true;

         
            crnode.AddHandle();
            crnode.Initialize();
            lstDesc.SetAllItemWidth(160);
            lg.SetAllItemWidth(160);
            crnode.EditMode = false;
            RepeaterBindingInfo rbinfo = new RepeaterBindingInfo();
            crnode.BindingInfo = new BindingInfo();
            crnode.BindingInfo.BindingClass = "Entity";
            crnode.BindingInfo.Bindings.Add("Entity", "Name");
            crnode.BindingInfo.Bindings.Add(lblOorE.Name, "EntityType");
            crnode.BindingInfo.RepeaterBindings.Add(rbinfo);
            // hack!
            Variables.Instance.ConnectionString = "server=.\\SqlExpress;Initial Catalog=MetaBuilder;Integrated Security=true";
            Variables.Instance.SourceCodePath = @"E:\Program Files\Discon Specialists\MetaBuilder\MetaData\SourceFiles";

            rbinfo.Association = new Association();
            rbinfo.Association.ID = 2310;
            rbinfo.Association.ParentClass = "Entity";
            rbinfo.Association.IsDefault = true;
            rbinfo.BoundProperty = "Name";
            rbinfo.Association.ChildClass = "Attribute";
            rbinfo.Association.Caption = "Key Attributes";
            //.GetAssociation("DataEntity", "Attribute", 4);//(int)MetaBuilder.Meta.LinkAssociationType.Decomposition);
            rbinfo.RepeaterID = new Guid();
            rbinfo.RepeaterName = rbinfo.Association.Caption;
            crnode.RepeaterSections[0].Name = rbinfo.Association.Caption;


            RepeaterBindingInfo rbinfoD = new RepeaterBindingInfo();
            crnode.BindingInfo.RepeaterBindings.Add(rbinfoD);
            rbinfoD.Association = new Association();
            rbinfoD.Association.ID = 3341;
            rbinfoD.Association.ParentClass = "Entity";
            rbinfoD.Association.IsDefault = true;
            rbinfoD.BoundProperty = "Name";
            rbinfoD.Association.ChildClass = "Attribute";
            rbinfoD.Association.Caption = "Descriptive Attributes";
            //.GetAssociation("DataEntity", "Attribute", 4);//(int)MetaBuilder.Meta.LinkAssociationType.Decomposition);
            rbinfoD.RepeaterID = new Guid();
            rbinfoD.RepeaterName = rbinfoD.Association.Caption;
            crnode.RepeaterSections[1].Name = rbinfoD.Association.Caption;
            crnode.Handle.Visible = false;

            /*
            AnchorPositionBehaviour apbehav1 = new AnchorPositionBehaviour(lstDesc, bottomPort, PositionLockLocation.BottomCenter);
            bottomPort.Manager.AddBehaviour(apbehav1);
            lstDesc.AddObserver(bottomPort);*/

            prtBottom.Position = new PointF(prtBottom.Position.X, prtBottom.Position.Y - 5);
            AnchorPositionBehaviour apbehav1 = new AnchorPositionBehaviour(rect, prtBottom, PositionLockLocation.BottomCenter);
            prtBottom.Manager.AddBehaviour(apbehav1);
            rect.AddObserver(prtBottom);

            prtBottomLeft.Position = new PointF(prtBottomLeft.Position.X, prtBottom.Position.Y);
            AnchorPositionBehaviour apbehav2 = new AnchorPositionBehaviour(rect, prtBottomLeft, PositionLockLocation.BottomCenter);
            prtBottomLeft.Manager.AddBehaviour(apbehav2);
            rect.AddObserver(prtBottomLeft);

            prtBottomLeftMost.Position = new PointF(prtBottomLeftMost.Position.X, prtBottom.Position.Y);
            AnchorPositionBehaviour apbehav3 = new AnchorPositionBehaviour(rect, prtBottomLeftMost, PositionLockLocation.BottomCenter);
            prtBottomLeftMost.Manager.AddBehaviour(apbehav3);
            rect.AddObserver(prtBottomLeftMost);

            prtBottomRight.Position = new PointF(prtBottomRight.Position.X, prtBottom.Position.Y);
            AnchorPositionBehaviour apbehav4 = new AnchorPositionBehaviour(rect, prtBottomRight, PositionLockLocation.BottomCenter);
            
            prtBottomRight.Manager.AddBehaviour(apbehav4);
            rect.AddObserver(prtBottomRight);

            prtBottomRightMost.Position = new PointF(prtBottomRightMost.Position.X, prtBottom.Position.Y);
            AnchorPositionBehaviour apbehav5 = new AnchorPositionBehaviour(rect, prtBottomRightMost, PositionLockLocation.BottomCenter);
            
            prtBottomRightMost.Manager.AddBehaviour(apbehav5);
            rect.AddObserver(prtBottomRightMost);
            
            return crnode;

        }
    }
}
