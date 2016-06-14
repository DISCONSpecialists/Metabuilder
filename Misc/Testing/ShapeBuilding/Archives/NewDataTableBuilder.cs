using System;
using System.Collections.Generic;
using System.Drawing;
using MetaBuilder.Core;
using MetaBuilder.Graphing.Formatting;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Behaviours.Internal;
using MetaBuilder.Graphing.Shapes.Behaviours.Observers;
using MetaBuilder.Graphing.Shapes.Nodes;
using MetaBuilder.Graphing.Shapes.Primitives;
using MetaBuilder.Meta;
using Northwoods.Go;

namespace ShapeBuilding
{
    public class NewDataTableBuilder
    {
        public GraphNode GetShape()
        {
            CollapsibleNode crnode = new CollapsibleNode();
            crnode.EditMode = true;
            #region Ports
            List<QuickPort> portsToAdd = new List<QuickPort>();
            
          

            QuickPort leftPortTop = new QuickPort(false);
            leftPortTop.Position = new PointF(0, 22.5f);
            leftPortTop.IncomingLinksDirection = 180;
            leftPortTop.OutgoingLinksDirection = 180;
            portsToAdd.Add(leftPortTop);
            

            QuickPort topLeftPort = new QuickPort(false);
            topLeftPort.Position = new PointF(30 - topLeftPort.Width/2, 0);
            topLeftPort.IncomingLinksDirection = 270;
            topLeftPort.OutgoingLinksDirection = 270;
            portsToAdd.Add(topLeftPort);

            QuickPort topLeftPort2 = new QuickPort(false);
            topLeftPort2.Position = new PointF(60 - topLeftPort2.Width / 2, 0);
            topLeftPort2.IncomingLinksDirection = 270;
            topLeftPort2.OutgoingLinksDirection = 270;
            portsToAdd.Add(topLeftPort2);
            
            

            QuickPort topRightPort = new QuickPort(false);
            topRightPort.Position = new PointF(120 - topRightPort.Width / 2, 0);
            topRightPort.IncomingLinksDirection = 270;
            topRightPort.OutgoingLinksDirection = 270;
            portsToAdd.Add(topRightPort);


            QuickPort topRightPort2 = new QuickPort(false);
            topRightPort2.Position = new PointF(150 - topRightPort2.Width / 2, 0);
            topRightPort2.IncomingLinksDirection = 270;
            topRightPort2.OutgoingLinksDirection = 270;
            portsToAdd.Add(topRightPort2);

            

            QuickPort rightPortTop = new QuickPort(false);
            rightPortTop.Position = new PointF(170, 22.5f);
            rightPortTop.IncomingLinksDirection = 0;
            rightPortTop.OutgoingLinksDirection = 0;
            portsToAdd.Add(rightPortTop);

            QuickPort prtBottomLeftMost = new QuickPort(false);
            prtBottomLeftMost.Position = new PointF(25, 50);
            portsToAdd.Add(prtBottomLeftMost);
            prtBottomLeftMost.IncomingLinksDirection = 90;
            prtBottomLeftMost.OutgoingLinksDirection = 90;

            QuickPort prtBottomLeft = new QuickPort(false);
            prtBottomLeft.Position = new PointF(55, 50);
            portsToAdd.Add(prtBottomLeft);
            prtBottomLeft.IncomingLinksDirection = 90;
            prtBottomLeft.OutgoingLinksDirection = 90;


            QuickPort prtBottomRight = new QuickPort(false);
            prtBottomRight.Position = new PointF(115, 50);
            portsToAdd.Add(prtBottomRight);
            prtBottomRight.IncomingLinksDirection = 90;
            prtBottomRight.OutgoingLinksDirection = 90;



            QuickPort prtBottomRightMost = new QuickPort(false);
            prtBottomRightMost.Position = new PointF(145, 50);
            portsToAdd.Add(prtBottomRightMost);
            prtBottomRightMost.IncomingLinksDirection = 90;
            prtBottomRightMost.OutgoingLinksDirection = 90;

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

            GradientRoundedRectangle ellipse = new GradientRoundedRectangle();
            GradientBehaviour gbehaviour = new GradientBehaviour();
            gbehaviour.MyBrush = new ShapeGradientBrush();
            gbehaviour.MyBrush.GradientType = GradientType.Horizontal;
            gbehaviour.MyBrush.InnerColor = Color.SteelBlue;
            gbehaviour.MyBrush.OuterColor = Color.White;
            ellipse.Manager.AddBehaviour(gbehaviour);
            ellipse.Width = 170;
            ellipse.Height = 50;
            ellipse.DragsNode = true;
            ellipse.Position = new PointF(5, 5);
            crnode.Add(ellipse);

            QuickPort leftPort = new QuickPort(false);
            leftPort.Position = new PointF(0, 45);
            leftPort.OutgoingLinksDirection = 180;
            leftPort.IncomingLinksDirection = 180;
            portsToAdd.Add(leftPort);

            QuickPort topPort = new QuickPort(false);
            topPort.Position = new PointF(90 - topPort.Width / 2, 0);
            topPort.IncomingLinksDirection = 270;
            topPort.OutgoingLinksDirection = 270;
            portsToAdd.Add(topPort);
            QuickPort rightPort = new QuickPort(false);
            rightPort.Position = new PointF(170, 45);
            rightPort.IncomingLinksDirection = 0;
            rightPort.OutgoingLinksDirection = 0;
            portsToAdd.Add(rightPort);

            topPort.OutgoingLinksDirection = 270;
            topPort.IncomingLinksDirection = 270;
            /*topLeftPort.OutgoingLinksDirection = 270;
            topLeftPort.IncomingLinksDirection = 270;
            topLeftPort2.OutgoingLinksDirection = 270;
            topLeftPort2.IncomingLinksDirection = 270;
            topRightPort.OutgoingLinksDirection = 270;
            topRightPort.IncomingLinksDirection = 270;
            topRightPort2.IncomingLinksDirection = 270;
            topRightPort2.OutgoingLinksDirection = 270;
            
            leftPort.IncomingLinksDirection = 180;
            leftPort.OutgoingLinksDirection = 180;

            rightPort.OutgoingLinksDirection = 0;
            rightPort.IncomingLinksDirection = 0;*/
         

            QuickPort prtBottomMiddle = new QuickPort(false);
            prtBottomMiddle.Position = new PointF(85, 40);
            portsToAdd.Add(prtBottomMiddle);
            prtBottomMiddle.IncomingLinksDirection = 90;
            prtBottomMiddle.OutgoingLinksDirection = 90;

     

            #endregion
            crnode.BindingInfo = new BindingInfo();
            crnode.BindingInfo.BindingClass = "DataTable";
            crnode.BindingInfo.Bindings.Add("DataTable", "Name");
            crnode.CreateMetaObject(null, EventArgs.Empty);


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

            
            BoundLabel txt = new BoundLabel();
            txt.Name = "DataTable";
            txt.Text = "DataTable";
            txt.DragsNode = true;
            txt.Editable = true;
            txt.Selectable = true;
            txt.Resizable = true;
            txt.Alignment = 1;
            txt.Multiline = false;
       
            txt.Wrapping = true;
            txt.Clipping = false;
            txt.StringTrimming = StringTrimming.None;
            txt.WrappingWidth = 160;
            txt.Width = ellipse.Width - 20;
            txt.AutoResizes = false;
            txt.Position = new PointF(ellipse.Left + 5, 5);
            txt.Deletable = false;
            txt.Width = 165;
            crnode.Add(txt);
            crnode.Grid.Selectable = false;
           

            ellipse.Selectable = false;
            ellipse.Deletable = false;

            ellipse.Resizable = false;
            ellipse.Selectable = false;
            ellipse.AutoRescales = false;
            txt.AutoResizes = false;
            txt.AutoRescales = false;
            crnode.CreateBody();

            crnode.ItemWidth = 170;
            crnode.ChildIndentation = 2;
            crnode.List.Position = new PointF(5, crnode.Height);


            ExpandableLabelList lg = new ExpandableLabelList();
            lg.HeaderText.Text = "Key Columns";
            lg.HeaderText.Editable = false;
            //lg.Handle.Style = GoCollapsibleHandleStyle.PlusMinus;
            crnode.List.Add(lg);
            lg.Insertable = true;
            lg.ChildIndentation = 2;
            crnode.List.Position = new PointF(5, ellipse.Bottom);
            crnode.Grid.Selectable = false;

            ExpandableLabelList lstDesc = new ExpandableLabelList();
            lstDesc.Name = "Descriptives";
            lstDesc.HeaderText.Text = "Descriptive Columns";
      
            lstDesc.Insertable = true;
            lstDesc.ChildIndentation = 2;
            crnode.List.Add(lstDesc);
            lstDesc.HeaderText.Editable = false;


        
            leftPort.DragsNode = true;
            topPort.DragsNode = true;
            rightPort.DragsNode = true;
            

         
            crnode.AddHandle();
            crnode.Handle.Brush = Brushes.Yellow;
            crnode.Initialize();
            lstDesc.SetAllItemWidth(170);
            lg.SetAllItemWidth(170);
            crnode.EditMode = false;
            RepeaterBindingInfo rbinfo = new RepeaterBindingInfo();
            crnode.BindingInfo = new BindingInfo();
            crnode.BindingInfo.BindingClass = "DataTable";
            crnode.BindingInfo.Bindings.Add("DataTable", "Name");
            crnode.BindingInfo.RepeaterBindings.Add(rbinfo);
            // hack!
            Variables.Instance.ConnectionString = "server=.\\SqlExpress;Initial Catalog=MetaBuilder;Integrated Security=true";
            Variables.Instance.SourceCodePath = @"C:\Program Files\Discon Specialists\MetaBuilder\MetaData\SourceFiles";

            rbinfo.Association = new Association();
            rbinfo.Association.ID = 2341;
            rbinfo.Association.ParentClass = "DataTable";
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
            rbinfoD.Association.ID = 2584;
            rbinfoD.Association.ParentClass = "DataTable";
            rbinfoD.Association.IsDefault = true;
            rbinfoD.BoundProperty = "Name";
            rbinfoD.Association.ChildClass = "Attribute";
            rbinfoD.Association.Caption = "Descriptive Attributes";
            //.GetAssociation("DataEntity", "Attribute", 4);//(int)MetaBuilder.Meta.LinkAssociationType.Decomposition);
            rbinfoD.RepeaterID = new Guid();
            rbinfoD.RepeaterName = rbinfoD.Association.Caption;
            crnode.RepeaterSections[1].Name = rbinfoD.Association.Caption;


            

            GoGroup grp = new GoGroup();

            GradientRoundedRectangle grect = new GradientRoundedRectangle();

            GoText lblInitialPopulation = new GoText();
            GoText lblGrowth = new GoText();
            GoText lblRecordSize = new GoText();
            lblInitialPopulation.DragsNode = true;
            lblGrowth.DragsNode = true;
            lblRecordSize.DragsNode = true;
            lblInitialPopulation.Deletable = false;
            lblGrowth.Deletable = false;
            lblRecordSize.Deletable = false;

            BoundLabel txtInitialPopulation = new BoundLabel();
            BoundLabel txtGrowth = new BoundLabel();
            BoundLabel txtRecordSize = new BoundLabel();
            txtInitialPopulation.Deletable = false;
            txtGrowth.Deletable = false;
            txtRecordSize.Deletable = false;

            lblInitialPopulation.FontSize = 8;
            lblGrowth.FontSize = 8;
            lblRecordSize.FontSize = 8;
            /*lblInitialPopulation.Name = "init";
            lblGrowth.Name = "grw";
            lblRecordSize.Name = "rsiz";*/

            txtInitialPopulation.Name = "txtINIT";
            txtGrowth.Name = "txtGROWTH";
            txtRecordSize.Name = "txtRecordSize";

            txtInitialPopulation.FontSize = 8;
            txtGrowth.FontSize = 8;
            txtRecordSize.FontSize = 8;

            txtInitialPopulation.AutoResizes = false;
            txtGrowth.AutoResizes = false;
            txtRecordSize.AutoResizes = false;

            txtInitialPopulation.Width = 40;
            txtGrowth.Width = 40;
            txtRecordSize.Width = 40;
            txtInitialPopulation.Alignment = 2;
            txtGrowth.Alignment = 2;
            txtRecordSize.Alignment = 2;

            grp.Add(grect);
            grp.Add(lblInitialPopulation);
            grp.Add(lblGrowth);
            grp.Add(lblRecordSize);

            grp.Add(txtInitialPopulation);
            grp.Add(txtGrowth);
            grp.Add(txtRecordSize);

            crnode.List.Add(grp);
            grect.Brush = Brushes.White;//new SolidBrush(Color.White);
            grect.Size = new SizeF(170,30);
            grect.Position = new PointF(0,lstDesc.Position.Y + lstDesc.Height);
            grp.Deletable = false;
            lblGrowth.AutoResizes = true;
            lblInitialPopulation.Text = "Pop.";
            lblGrowth.Text = "Growth";
            lblRecordSize.Text = "Size";
            lblGrowth.Alignment = 1;
            txtGrowth.Alignment = 1;
            txtGrowth.AutoResizes = false;
    
            lblRecordSize.Alignment= 2;
            lblInitialPopulation.Alignment = 2;

            lblInitialPopulation.Position = new PointF(9, lstDesc.Position.Y + lstDesc.Height + 2);
       
            lblGrowth.Position = new PointF((grect.Width/2)-(lblGrowth.Width/2)+5, lstDesc.Position.Y + lstDesc.Height + 2);
            lblRecordSize.Position = new PointF(lblGrowth.Position.X + 58, lstDesc.Position.Y + lstDesc.Height + 2);

            txtInitialPopulation.Position = new PointF(lblInitialPopulation.Position.X, lblInitialPopulation.Bottom);
            txtGrowth.Position = new PointF(lblGrowth.Position.X, lblGrowth.Bottom);
            txtRecordSize.Position = new PointF(lblRecordSize.Position.X, lblRecordSize.Bottom);

            
            crnode.BindingInfo.Bindings.Add(txtInitialPopulation.Name,"InitialPopulation");
            crnode.BindingInfo.Bindings.Add(txtGrowth.Name,"GrowthRateOverTime");
            crnode.BindingInfo.Bindings.Add(txtRecordSize.Name,"RecordSize");
            lblInitialPopulation.AddObserver(crnode);
            lblRecordSize.AddObserver(crnode);
            lblGrowth.AddObserver(crnode);

            float Xcoord = 59.5f;// lblInitialPopulation.Right + ((lblGrowth.Left - lblInitialPopulation.Right) / 2) + 10;
            AddStroke(crnode, grect, Xcoord,grp);
            Xcoord = 119.5f;
            AddStroke(crnode, grect, Xcoord,grp);


            /*
            AnchorPositionBehaviour apbehav2 = new AnchorPositionBehaviour(lstDesc, grect, PositionLockLocation.BottomCenter);
            grect.Manager.AddBehaviour(apbehav2);
            lstDesc.AddObserver(grect);

            AnchorPositionBehaviour apbehav3 = new AnchorPositionBehaviour(grect, lblGR, PositionLockLocation.BottomCenter);
            lblGR.Manager.AddBehaviour(apbehav3);
            lstDesc.AddObserver(lblGR);

            AnchorPositionBehaviour apbehav4 = new AnchorPositionBehaviour(grect, lblRS, PositionLockLocation.BottomCenter);
            lblRS.Manager.AddBehaviour(apbehav4);
            lstDesc.AddObserver(lblRS);

            AnchorPositionBehaviour apbehav5 = new AnchorPositionBehaviour(grect, lblIP, PositionLockLocation.BottomCenter);
            lblIP.Manager.AddBehaviour(apbehav5);
            lstDesc.AddObserver(lblIP);*/


            AddBehaviourToBottomPort(grect,prtBottomLeft);
            AddBehaviourToBottomPort(grect, prtBottomLeftMost);
            AddBehaviourToBottomPort(grect, prtBottomMiddle);
            AddBehaviourToBottomPort(grect, prtBottomRight);
            AddBehaviourToBottomPort(grect, prtBottomRightMost);



            Color outer = Color.FromArgb(255, 51, 51, 51);

            Color inner = Color.FromArgb(255, 128, 128, 128);

            prtBottomMiddle.Brush = new SolidBrush(inner);
            prtBottomMiddle.Pen = new Pen(outer);
            topPort.Brush = new SolidBrush(inner);
            topPort.Pen = new Pen(outer);
            leftPort.Brush = new SolidBrush(inner);
            leftPort.Pen = new Pen(outer);
            rightPort.Brush = new SolidBrush(inner);
            rightPort.Pen = new Pen(outer);


            grect.Resizable = false;
            grect.Reshapable = false;
            grect.Selectable = false;
            grect.Movable = false;
            grect.Editable = false;
            grect.Deletable = false;
            grect.DragsNode = true;
            grp.Resizable = false;
            grp.Reshapable = false;
           

            lblInitialPopulation.Editable = false;
            lblGrowth.Editable = false;
            lblInitialPopulation.Editable = false;
            txtGrowth.AddObserver(crnode);
            txtInitialPopulation.AddObserver(crnode);
            txtRecordSize.AddObserver(crnode);
            crnode.EditMode = false;
            crnode.HookupEvents();
            return crnode;

        }

        private static void AddStroke(CollapsibleNode crnode, GradientRoundedRectangle grect, float Xcoord,GoGroup grp)
        {
            QuickStroke qs = new QuickStroke();
            qs.AddPoint(new PointF(Xcoord, grect.Bottom+1.0f));
            qs.AddPoint(new PointF(Xcoord, grect.Top));
            qs.Selectable = false;
            qs.Deletable = false;
            qs.DragsNode = true;
            qs.Reshapable = false;
            qs.Resizable = false;
            qs.AutoRescales = false;
            qs.InvalidateViews();
            grp.Add(qs);



          /*  AnchorPositionBehaviour apbehav1 = new AnchorPositionBehaviour(grect, qs, PositionLockLocation.TopCenter);
            qs.Manager.AddBehaviour(apbehav1);
            grect.AddObserver(qs);
            qs.DragsNode = true;*/
        }

        private static void AddBehaviourToBottomPort(GradientRoundedRectangle grect,QuickPort port)
        {
            port.Position = new PointF(port.Position.X, grect.Bottom-5);
            AnchorPositionBehaviour apbehav1 = new AnchorPositionBehaviour(grect, port, PositionLockLocation.BottomCenter);
            port.Manager.AddBehaviour(apbehav1);
            grect.AddObserver(port);
            port.DragsNode = true;
        }
    }
}
