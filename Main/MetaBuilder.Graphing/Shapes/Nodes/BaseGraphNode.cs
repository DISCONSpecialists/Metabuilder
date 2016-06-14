using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using MetaBuilder.Graphing.Shapes.Behaviours.Internal;
using MetaBuilder.Graphing.Formatting;
using MetaBuilder.Meta;
using MetaBuilder.BusinessLogic;

namespace MetaBuilder.Graphing.Shapes.Nodes
{
    public class BaseGraphNode : GraphNode
    {
        public BaseGraphNode(MetaBase mBase)
            : base()
        {
            MetaObject = mBase;
        }

        #region Construct

        //building this. we are not adding a type
        /*
          <gnode id="127" DragsNode="false" Print="true" Reshape="true" Resize="false" Move="true" Select="true" Deletable="true" xy="20 5 180 100" cls="Function" lbls="Name|Name~">
    <qprt id="1" Reshape="false" Resize="false" Move="true" xy="55 10 10 10" P_L="TopLeft" solidbrush="-2894893" PenColor="-8355712" PenWidth="1" PenDashStyle="Solid" PenDashCap="Flat" Self="false" I="270" O="270" />
    <qprt id="13" Reshape="false" Resize="false" Move="true" xy="130 10 10 10" P_L="TopRight" solidbrush="-2894893" PenColor="-8355712" PenWidth="1" PenDashStyle="Solid" PenDashCap="Flat" Self="false" I="270" O="270" />
    <qprt id="3" Reshape="false" Resize="false" Move="true" xy="190 75 10 10" P_L="RightBottom" solidbrush="-2894893" PenColor="-8355712" PenWidth="1" PenDashStyle="Solid" PenDashCap="Flat" Self="false" I="0" O="0" />
    <qprt id="4" Reshape="false" Resize="false" Move="true" xy="190 25 10 10" P_L="RightTop" solidbrush="-2894893" PenColor="-8355712" PenWidth="1" PenDashStyle="Solid" PenDashCap="Flat" Self="false" I="0" O="0" />
    <qprt id="10" Reshape="false" Resize="false" Move="true" xy="30 95 10 10" P_L="BottomLeft" solidbrush="-2894893" PenColor="-8355712" PenWidth="1" PenDashStyle="Solid" PenDashCap="Flat" Self="false" I="90" O="90" />
    <qprt id="5" Reshape="false" Resize="false" Move="true" xy="180 95 10 10" P_L="BottomRight" solidbrush="-2894893" PenColor="-8355712" PenWidth="1" PenDashStyle="Solid" PenDashCap="Flat" Self="false" I="90" O="90" />
    <qprt id="11" Reshape="false" Resize="false" Move="true" xy="20 75 10 10" P_L="LeftBottom" solidbrush="-2894893" PenColor="-8355712" PenWidth="1" PenDashStyle="Solid" PenDashCap="Flat" Self="false" I="180" O="180" />
    <qprt id="12" Reshape="false" Resize="false" Move="true" xy="20 25 10 10" P_L="LeftTop" solidbrush="-2894893" PenColor="-8355712" PenWidth="1" PenDashStyle="Solid" PenDashCap="Flat" Self="false" I="180" O="180" />
         
    <qprt id="14" Reshape="false" Resize="false" Move="true" xy="80 10 10 10" P_L="Circumferential" solidbrush="-2894893" PenColor="-8355712" PenWidth="1" PenDashStyle="Solid" PenDashCap="Flat" Self="false" I="270" O="270" />
    <qprt id="6" Reshape="false" Resize="false" Move="true" xy="155 95 10 10" P_L="Circumferential" solidbrush="-2894893" PenColor="-8355712" PenWidth="1" PenDashStyle="Solid" PenDashCap="Flat" Self="false" I="90" O="90" />
    <qprt id="7" Reshape="false" Resize="false" Move="true" xy="130 95 10 10" P_L="Circumferential" solidbrush="-2894893" PenColor="-8355712" PenWidth="1" PenDashStyle="Solid" PenDashCap="Flat" Self="false" I="90" O="90" />
    <qprt id="8" Reshape="false" Resize="false" Move="true" xy="80 95 10 10" P_L="Circumferential" solidbrush="-2894893" PenColor="-8355712" PenWidth="1" PenDashStyle="Solid" PenDashCap="Flat" Self="false" I="90" O="90" />
    <qprt id="9" Reshape="false" Resize="false" Move="true" xy="55 95 10 10" P_L="Circumferential" solidbrush="-2894893" PenColor="-8355712" PenWidth="1" PenDashStyle="Solid" PenDashCap="Flat" Self="false" I="90" O="90" />
   
          <pmRectangle id="15" Corner="8 8" Edit="true" Print="true" Reshape="false" Resize="false" Move="true" xy="25 15 170 85" PenColor="-16777216" PenWidth="1" PenDashStyle="Solid" PenDashCap="Flat" shpName="9111c8d9-ab0b-4ac3-af1a-f4c7059d412e" grad="true" grad_bordercolor="0" grad_outercolor="-1" grad_innercolor="-12156236" grad_type="Horizontal" />
         
    <qprt id="16" Reshape="false" Resize="false" Move="true" xy="105 10 10 10" P_L="Top" solidbrush="-8355712" PenColor="-13421773" PenWidth="1" PenDashStyle="Solid" PenDashCap="Flat" Self="false" I="270" O="270" />
    <qprt id="17" Reshape="false" Resize="false" Move="true" xy="20 50 10 10" P_L="Left" solidbrush="-8355712" PenColor="-13421773" PenWidth="1" PenDashStyle="Solid" PenDashCap="Flat" Self="false" I="180" O="180" />
    <qprt id="18" Reshape="false" Resize="false" Move="true" xy="190 50 10 10" P_L="Right" solidbrush="-8355712" PenColor="-13421773" PenWidth="1" PenDashStyle="Solid" PenDashCap="Flat" Self="false" I="0" O="0" />
    <qprt id="19" Reshape="false" Resize="false" Move="true" xy="105 95 10 10" P_L="Bottom" solidbrush="-8355712" PenColor="-13421773" PenWidth="1" PenDashStyle="Solid" PenDashCap="Flat" Self="false" I="90" O="90" />
          
    <lbl id="20" font="Microsoft Sans Serif" txt="Function" align="4" clip="true" fontSz="7" max="100" min="0" lblEdit="false" trim="EllipsisCharacter" autoresize="false" edstyle="TextBox" colour="-8355712" Print="true" Reshape="false" Resize="false" Move="true" xy="145 5 50 10" name="cls_id" />
    <lbl id="22" font="Microsoft Sans Serif" txt="Function Name" align="1" clip="true" fontSz="10" max="100" min="0" lblEdit="true" trim="None" autoresize="false" wrap="true" wrapw="170" edstyle="TextBox" colour="-16777216" Edit="true" Print="true" Reshape="false" Resize="false" Move="true" xy="30 35 160 60" name="Name" />
         
    <allocationHandle id="68" Corner="0 0" DragsNode="true" Print="false" Reshape="true" Resize="false" Move="false" Deletable="true" xy="185 90 10 10" solidbrush="-256" PenColor="-12042869" PenWidth="1" PenDashStyle="Solid" PenDashCap="Flat" allocations=""/>
  </gnode>
        */

        public void Construct(string classname)
        {
            Location = new PointF(20, 5);

            Width = 180;
            Height = 100;

            CalculateGridSize();

            BindingInfo = new BindingInfo();
            BindingInfo.BindingClass = classname;
            BindingInfo.Bindings = new Dictionary<string, string>();

            string labelName = "Name"; //for class that do not have a name as the default label this will not work.
            try
            {
                TList<Field> fields = DataAccessLayer.DataRepository.ClientFieldsByClass(classname).FindAll(FieldColumn.IsActive, true);
                fields.Sort("SortOrder");
                labelName = fields[0].Name;
            }
            catch
            {
                labelName.ToString();
            }
            BindingInfo.Bindings.Add(labelName, labelName);

            //create circ ports
            createPort(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.TopLeft, false, 270, 270, new PointF(55, 10));
            createPort(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.TopRight, false, 270, 270, new PointF(130, 10));
            createPort(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.RightBottom, false, 0, 0, new PointF(190, 75));
            createPort(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.RightTop, false, 0, 0, new PointF(190, 25));
            createPort(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.BottomLeft, false, 90, 90, new PointF(30, 95));
            createPort(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.BottomRight, false, 90, 90, new PointF(180, 95));
            createPort(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.LeftBottom, false, 180, 180, new PointF(20, 75));
            createPort(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.LeftTop, false, 180, 180, new PointF(20, 25));

            createPort(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Circumferential, false, 270, 270, new PointF(80, 10));

            createPort(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Circumferential, false, 90, 90, new PointF(155, 95));
            createPort(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Circumferential, false, 90, 90, new PointF(130, 95));
            createPort(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Circumferential, false, 90, 90, new PointF(80, 95));
            createPort(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Circumferential, false, 90, 90, new PointF(55, 95));

            //create shape
            createShape();
            //create ports
            createPort(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Top, false, 270, 270, new PointF(105, 10));
            createPort(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Left, false, 180, 180, new PointF(20, 50));
            createPort(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Right, false, 0, 0, new PointF(190, 50));
            createPort(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Bottom, false, 90, 90, new PointF(105, 95));

            //create labels
            createLabel("cls_id", classname);
            createLabel(labelName, classname + " Name");
            //create allocation
            createAllocation();

            Resizable = false;
            Reshapable = false;

            EditMode = false;

            HookupEvents();
            BindToMetaObjectProperties();
        }

        void createPort(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation position, bool main, int Input, int Output, PointF location)
        {
            QuickPort p = new QuickPort();
            p.PortPosition = position;
            p.Location = location;
            p.Width = 10;
            p.Height = 10;
            p.DragsNode = true;
            p.Selectable = false;
            if (main)
                p.Brush = Brushes.Gray;//new SolidBrush(Color.Gray);
            else
                p.Brush = Brushes.DarkGray;// new SolidBrush(Color.DarkGray);

            Add(p);
        }
        void createShape()
        {
            MetaBuilder.Graphing.Shapes.Primitives.GradientRoundedRectangle r = new MetaBuilder.Graphing.Shapes.Primitives.GradientRoundedRectangle();
            r.Bounds = new RectangleF(25, 15, 170, 85);
            r.DragsNode = true;
            r.Corner = new SizeF(2, 2);
            r.Selectable = false;
            r.Resizable = false;

            GradientBehaviour gbeh = new GradientBehaviour();
            gbeh.MyBrush = new ShapeGradientBrush();
            gbeh.MyBrush.BorderColor = Color.Black;
            gbeh.MyBrush.OuterColor = Color.SkyBlue;
            gbeh.MyBrush.InnerColor = Color.White;
            gbeh.MyBrush.GradientType = GradientType.Vertical;

            r.Manager.AddBehaviour(gbeh);
            gbeh.Update(r);

            Add(r);
        }
        void createLabel(string name, string value)
        {
            BoundLabel l = new BoundLabel();
            l.Name = name;
            l.DragsNode = true;
            l.Selectable = false;
            l.AutoResizes = false;
            l.AutoRescales = false;
            l.Clipping = true;
            l.EditorStyle = Northwoods.Go.GoTextEditorStyle.TextBox;

            if (l.Name == "cls_id")
            {
                l.Location = new PointF(145, 10);
                l.Width = 50;
                l.Height = 10;

                l.Alignment = 4;
                l.FontSize = 7;
                l.TextColor = Color.Gray;
                l.Editable = false;

                l.Text = value;
            }
            else
            {
                l.Width = 140;
                l.Height = 70;
                l.Position = new PointF(Grid.Left + 10, Grid.Top + 20);

                l.Alignment = 1;
                l.FontSize = 10;
                l.Editable = true;
                l.Wrapping = true;
                l.WrappingWidth = 120;

                l.Text = value;
                l.AddObserver(this);
            }

            Add(l);
        }
        void createAllocation()
        {
            AllocationHandle handle = new AllocationHandle();
            handle.Location = new PointF(185, 90);
            handle.Width = 10;
            handle.Height = 10;
            handle.DragsNode = true;
            handle.Selectable = false;
            Add(handle);
        }

        #endregion
    }
}
