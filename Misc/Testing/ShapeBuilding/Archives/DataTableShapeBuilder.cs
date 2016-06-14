using System;
using System.Collections.Generic;
using System.Drawing;
using MetaBuilder.Core;
using MetaBuilder.Graphing.Formatting;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Behaviours.Internal;
using MetaBuilder.Graphing.Shapes.Behaviours.Observers;
using MetaBuilder.Graphing.Shapes.Primitives;
using MetaBuilder.Meta;

namespace ShapeBuilding
{
    public class DataTableShapeBuilder
    {
        public GraphNode GetShape()
        {
        
            CollapsibleNode crnode = new CollapsibleNode();
            GradientRoundedRectangle ellipse = new GradientRoundedRectangle();
            GradientBehaviour gbehaviour = new GradientBehaviour();
            gbehaviour.MyBrush = new ShapeGradientBrush();
            gbehaviour.MyBrush.GradientType = GradientType.ForwardDiagonal;
            gbehaviour.MyBrush.InnerColor = Color.LightSteelBlue;
            gbehaviour.MyBrush.OuterColor = Color.White;
            ellipse.Manager.AddBehaviour(gbehaviour);
            ellipse.Width = 150;
            ellipse.Height = 50;
            ellipse.DragsNode = true;
            crnode.Add(ellipse);

            BoundLabel txt = new BoundLabel();
            txt.Name = "DataTable";
            txt.Text = "DataTable";
            txt.DragsNode = true;
            txt.Editable = true;
            txt.Selectable = true;
            txt.Resizable = true;
            txt.Alignment = 1;
            txt.Multiline = true;
            txt.Wrapping = true;
            txt.WrappingWidth = 160;
            txt.Width = ellipse.Width - 20;
            txt.AutoResizes = false;
            txt.Position = new PointF(ellipse.Left + 10, 0);
            txt.Deletable = false;
            crnode.Add(txt);

            List<QuickPort> portsToAdd = new List<QuickPort>();

            QuickPort qprt1 = new QuickPort(false);
            qprt1.Position = new PointF(ellipse.Width / 2 - qprt1.Width / 2, -4);
            portsToAdd.Add(qprt1);



            QuickPort qprt0 = new QuickPort(false);
            qprt0.Position = new PointF(25 - qprt0.Width / 2, -4);
            portsToAdd.Add(qprt0);

            QuickPort qprt2 = new QuickPort(false);
            qprt2.Position = new PointF(125 - qprt0.Width / 2, -4);
            portsToAdd.Add(qprt2);
            qprt1.OutgoingLinksDirection = 270;
            qprt1.IncomingLinksDirection = 270;
            qprt0.IncomingLinksDirection = 270;
            qprt0.OutgoingLinksDirection = 270;
            qprt2.OutgoingLinksDirection = 270;
            qprt2.IncomingLinksDirection = 270;


            QuickPort qprt3 = new QuickPort(false);
            qprt3.Position = new PointF(-4, ellipse.Height / 2);
            portsToAdd.Add(qprt3);
            qprt3.IncomingLinksDirection = 180;
            qprt3.OutgoingLinksDirection = 180;


            QuickPort qprt4 = new QuickPort(false);
            qprt4.Position = new PointF(ellipse.Width - 4, ellipse.Height / 2);
            portsToAdd.Add(qprt4);
            qprt4.IncomingLinksDirection = 0;
            qprt4.OutgoingLinksDirection = 0;

            QuickPort qprt5 = new QuickPort(false);

            portsToAdd.Add(qprt5);

            QuickPort qprt6 = new QuickPort(false);

            portsToAdd.Add(qprt6);

            QuickPort qprt7 = new QuickPort(false);

            portsToAdd.Add(qprt7);


            ellipse.Resizable = false;
            ellipse.Selectable = false;
            ellipse.AutoRescales = false;
            txt.AutoResizes = false;
            txt.AutoRescales = false;

            foreach (QuickPort prt in portsToAdd)
            {
                prt.Selectable = false;
                prt.AutoRescales = false;
                prt.Resizable = false;
                crnode.Add(prt);
            }

            crnode.CreateBody();

            crnode.ItemWidth = 150;
            crnode.ChildIndentation = 2;
            crnode.List.Position = new PointF(0, crnode.Height);


            RepeaterSection lg = new RepeaterSection();
            lg.Add(crnode.MakeItem("", "Key Attributes", true));
            lg.Insertable = true;
            lg.ChildIndentation = 2;
            crnode.List.Add(lg);
            crnode.List.Position = new PointF(0, ellipse.Bottom);

            RepeaterSection lstDesc = new RepeaterSection();
            lstDesc.Name = "Descriptives";

            lstDesc.Add(crnode.MakeItem("", "Descriptive Attributes", true));
            lstDesc.Insertable = true;
            lstDesc.ChildIndentation = 2;
            crnode.List.Add(lstDesc);
            QuickPort bottomPort = portsToAdd[portsToAdd.Count - 1];
            bottomPort.PortObject = lstDesc;
            bottomPort.OutgoingLinksDirection = 90;
            bottomPort.IncomingLinksDirection = 90;

            qprt5.OutgoingLinksDirection = 90;
            qprt5.IncomingLinksDirection = 90;

            qprt7.OutgoingLinksDirection = 90;
            qprt7.IncomingLinksDirection = 90;

            qprt6.OutgoingLinksDirection = 90;
            qprt6.IncomingLinksDirection = 90;

            qprt5.Position = new PointF(25 - qprt5.Width / 2, lstDesc.Bottom - qprt5.Height / 2);
            qprt6.Position = new PointF(60 - qprt6.Width / 2, lstDesc.Bottom - qprt6.Height / 2);
            qprt7.Position = new PointF(lstDesc.Width / 2 - qprt7.Width / 2, lstDesc.Bottom - qprt7.Height / 2);

            AnchorPositionBehaviour apbehav = new AnchorPositionBehaviour(lstDesc, qprt7, PositionLockLocation.BottomCenter);
            qprt7.Manager.AddBehaviour(apbehav);
            lstDesc.AddObserver(qprt7);

            AnchorPositionBehaviour apbehav2 = new AnchorPositionBehaviour(lstDesc, qprt5, PositionLockLocation.BottomLeft);
            qprt5.Manager.AddBehaviour(apbehav2);
            lstDesc.AddObserver(qprt5);

            AnchorPositionBehaviour apbehav3 = new AnchorPositionBehaviour(lstDesc, qprt6, PositionLockLocation.BottomCenter);
            qprt6.Manager.AddBehaviour(apbehav3);
            lstDesc.AddObserver(qprt6);
            qprt0.DragsNode = true;
            qprt1.DragsNode = true;
            qprt2.DragsNode = true;
            qprt3.DragsNode = true;
            qprt4.DragsNode = true;
            qprt5.DragsNode = true;
            qprt6.DragsNode = true;
            qprt7.DragsNode = true;

            crnode.AddHandle();
            crnode.Initialize();
            lstDesc.SetAllItemWidth(150);
            lg.SetAllItemWidth(150);
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
            rbinfo.Association.ID = 837;
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
            rbinfoD.Association.ID = 839;
            rbinfoD.Association.ParentClass = "DataTable";
            rbinfoD.Association.IsDefault = true;
            rbinfoD.BoundProperty = "Name";
            rbinfoD.Association.ChildClass = "Attribute";
            rbinfoD.Association.Caption = "Descriptive Attributes";
            //.GetAssociation("DataEntity", "Attribute", 4);//(int)MetaBuilder.Meta.LinkAssociationType.Decomposition);
            rbinfoD.RepeaterID = new Guid();
            rbinfoD.RepeaterName = rbinfoD.Association.Caption;
            crnode.RepeaterSections[1].Name = rbinfoD.Association.Caption;

            
      
            return crnode;

        }
    }
}
