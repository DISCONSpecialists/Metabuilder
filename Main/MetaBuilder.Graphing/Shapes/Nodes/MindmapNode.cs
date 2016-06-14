using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;
using System.Drawing;
using MetaBuilder.Graphing.Shapes.Binding.Intellisense;

namespace MetaBuilder.Graphing.Shapes.Nodes
{
    [Serializable]
    public class MindMapNode : GoBasicNode, ICloneable
    {
        public enum ShapeType
        {
            Rectangle = 1,
            RoundedRectangle = 2,
            Ellipse = 3,
            Triangle = 4,
            Diamond = 5
        }

        private string guid;
        public string GUID
        {
            get { return guid; }
            set { guid = value; }
        }

        private bool isFixedLocation;
        public bool IsFixedLocation
        {
            get { return isFixedLocation; }
            set
            {
                isFixedLocation = value;
                Shadowed = !isFixedLocation;
                Movable = !isFixedLocation;
                InvalidateViews();
            }
        }

        private ShapeType shapeType;
        public ShapeType MyShapeType
        {
            get { return shapeType; }
            set
            {
                shapeType = value;
                SetShape(value);
            }
        }

        public MindMapNode(bool withShape)
        {
            GUID = Guid.NewGuid().ToString();
            Resizable = true;
            Reshapable = true;

            Text = "New Node";
            Editable = true;
            Label.Editable = false;
            Label.Multiline = true;
            Label.Wrapping = true;
            Label.WrappingWidth = Width;
            Label.FontSize = 12f;
            Label.FamilyName = "Arial Black";
            LabelSpot = Northwoods.Go.GoText.Middle;

            Label.Wrapping = true;
            Label.WrappingWidth = 120;

            AutoResizes = true;
            IsFixedLocation = false;

            //9 January 2013 switch to enable/disable shapes
            if (withShape)
                CreateShape(Bounds);
            else
                MyShape = null;

            //createExpandButton();
            Expanded = true;
        }

        private bool expanded;
        public bool Expanded
        {
            get { return expanded; }
            set
            {
                expanded = value;
                if (expanded)
                    Pen = new Pen(new SolidBrush(Color.LightBlue));
                else
                    Pen = new Pen(new SolidBrush(Color.DarkBlue));
                InvalidateViews();
            }
        }

        private GoShape myShape;
        public GoShape MyShape
        {
            get { return myShape; }
            set
            {
                myShape = value;
                if (myShape == null)
                {
                    Shape = null;
                    Brush = new SolidBrush(Color.SkyBlue);
                    Pen = new Pen(Color.DarkBlue);
                }
                else
                {
                    Shape = myShape;
                }
            }
        }

        private MetaBuilder.Graphing.Formatting.ShapeGradientBrush shapeBrush;
        public MetaBuilder.Graphing.Formatting.ShapeGradientBrush ShapeBrush
        {
            get { return shapeBrush; }
            set
            {
                if (MyShape == null)
                    shapeBrush = null;
                else
                    shapeBrush = value;
            }
        }

        public void SetShapeColor(Color start, Color end)
        {
            if (start == Color.Transparent)
                start = Color.White;
            if (end == Color.Transparent)
                end = Color.White;

            if (MyShape != null)
            {
                Brush = new SolidBrush(Color.Transparent);
                Pen = new Pen(Color.Transparent);

                MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour gBehaviour = new MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour();
                gBehaviour.MyBrush = new MetaBuilder.Graphing.Formatting.ShapeGradientBrush();
                gBehaviour.MyBrush.GradientType = MetaBuilder.Graphing.Formatting.GradientType.Horizontal;
                gBehaviour.MyBrush.InnerColor = start;
                gBehaviour.MyBrush.OuterColor = end;
                ShapeBrush = gBehaviour.MyBrush;
                gBehaviour.AllowMultiple = false;
                gBehaviour.MyBrush.Apply(MyShape);
                (MyShape as MetaBuilder.Graphing.Shapes.Behaviours.IBehaviourShape).Manager.AddBehaviour(gBehaviour);

                Shape = MyShape;
            }
            else
            {
                Brush = new SolidBrush(start);
            }
        }
        public void SetShape(ShapeType type)
        {
            switch (type)
            {
                case ShapeType.Rectangle:
                    MetaBuilder.Graphing.Shapes.Primitives.GradientRoundedRectangle rect = new MetaBuilder.Graphing.Shapes.Primitives.GradientRoundedRectangle();
                    MyShape = rect;
                    break;
                case ShapeType.RoundedRectangle:
                    MetaBuilder.Graphing.Shapes.Primitives.GradientRoundedRectangle rectRounded = new MetaBuilder.Graphing.Shapes.Primitives.GradientRoundedRectangle();
                    rectRounded.Corner = new SizeF(30, 30);
                    MyShape = rectRounded;
                    break;
                case ShapeType.Diamond:
                    MetaBuilder.Graphing.Shapes.Primitives.GradientDiamond diamond = new MetaBuilder.Graphing.Shapes.Primitives.GradientDiamond();
                    MyShape = diamond;
                    break;
                case ShapeType.Triangle:
                    MetaBuilder.Graphing.Shapes.Primitives.GradientTriangle triangle = new MetaBuilder.Graphing.Shapes.Primitives.GradientTriangle();
                    MyShape = triangle;
                    break;
                case ShapeType.Ellipse:
                    MetaBuilder.Graphing.Shapes.Primitives.GradientEllipse ellipse = new MetaBuilder.Graphing.Shapes.Primitives.GradientEllipse();
                    MyShape = ellipse;
                    break;
            }

            Brush = new SolidBrush(Color.Transparent);
            Pen = new Pen(Color.Transparent);

            MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour gBehaviour = new MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour();
            gBehaviour.MyBrush = new MetaBuilder.Graphing.Formatting.ShapeGradientBrush();
            gBehaviour.MyBrush.GradientType = MetaBuilder.Graphing.Formatting.GradientType.Horizontal;
            gBehaviour.MyBrush.InnerColor = ShapeBrush.InnerColor;
            gBehaviour.MyBrush.OuterColor = ShapeBrush.OuterColor;
            ShapeBrush = gBehaviour.MyBrush;
            gBehaviour.AllowMultiple = false;
            gBehaviour.MyBrush.Apply(MyShape);
            (MyShape as MetaBuilder.Graphing.Shapes.Behaviours.IBehaviourShape).Manager.AddBehaviour(gBehaviour);

            Shape = MyShape;
        }

        public void CreateShape(RectangleF bounds)
        {
            Graphing.Shapes.Primitives.GradientRoundedRectangle gRect = new MetaBuilder.Graphing.Shapes.Primitives.GradientRoundedRectangle();

            gRect.Corner = new SizeF(30, 30);
            gRect.Selectable = false;
            gRect.DragsNode = true;
            gRect.Bounds = bounds;
            //gRect.Brush = new SolidBrush(Color.SkyBlue);

            MyShape = gRect;

            SetShapeColor(Color.SteelBlue, Color.White);
        }

        public override GoControl CreateEditor(GoView view)
        {
            GoControl editor = new GoControl();
            editor.ControlType = typeof(IntellisenseBox);
            editor.Position = Position;
            //editor.Size = new SizeF(350, 300);
            //editor.Resizable = true;
            editor.Position = new PointF(view.DocExtent.Right - editor.Width, view.DocExtent.Bottom - editor.Height);
            return editor;
        }

        #region ICloneable Members

        public object Clone()
        {
            return this.Copy() as MindMapNode;
        }

        #endregion
    }
}