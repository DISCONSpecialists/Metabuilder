using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Northwoods.Go;
using System.Drawing;
using System.Windows.Forms;
using MetaBuilder.Graphing.Shapes.Primitives;
using MetaBuilder.Graphing.Shapes.Behaviours;

namespace MetaBuilder.Graphing.Shapes.Nodes
{
    public enum LegendType { Color, Class, ColorAndClass }

    public class LegendNode : GoListGroup
    {
        private GoText myLabel;
        public GoText MyLabel
        {
            get { return myLabel; }
            set
            {
                myLabel = value;
                myLabel.Text = "Legend";
                myLabel.DragsNode = true;
                myLabel.Selectable = true;
                myLabel.Editable = false;
                myLabel.Alignment = GoObject.Middle;
                myLabel.Deletable = false;
            }
        }

        public LegendNode(LegendType type)
        {
            Resizable = true;
            Reshapable = true;
            Selectable = true;
            Spacing = 5;
            Orientation = Orientation.Vertical;
            MyLegendType = type;
            AddedItems = new Dictionary<string, LegendItem>();
            this.BorderPen = new Pen(Color.Black, 1f);

            GoText lab = new GoText();
            Add(lab);
            MyLabel = lab;
        }

        private string getKey(GraphNode n)
        {
            switch (MyLegendType)
            {
                case LegendType.Class:
                    return n.BindingInfo.BindingClass;
                case LegendType.Color:
                    return getColor(n);
                case LegendType.ColorAndClass:
                    return n.BindingInfo.BindingClass + "-" + getColor(n);
            }

            return "";
        }
        private string getColor(GraphNode node)
        {
            foreach (GoObject shp in node.GetEnumerator())
            {
                if (shp is IBehaviourShape)
                {
                    if (shp is GoPort && shp is QuickStroke) //QuickPorts are IBehaviourShapes and we dont want that
                        continue;

                    MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour gradObject =
                        (shp as IBehaviourShape).Manager.GetExistingBehaviour(typeof(MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour)) as
                        MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour;

                    if (gradObject != null)
                        return gradObject.MyBrush.InnerColor.ToString() + "=" + gradObject.MyBrush.OuterColor.ToString();
                    else
                    {
                        return "Solid color";
                    }
                }
            }
            //cant find a gradient on this node
            return "UnKnOwN Color";
        }
        private LegendType myLegendType;
        public LegendType MyLegendType
        {
            get
            {
                return myLegendType;
            }
            set
            {
                myLegendType = value;
            }
        }

        private Dictionary<string, LegendItem> addedItems;
        public Dictionary<string, LegendItem> AddedItems { get { return addedItems; } set { addedItems = value; } }
    }

    public class LegendItem : GoListGroup
    {
        public LegendItem()
        {
            constructDefaults();
        }

        public LegendItem(GoObject obj, String keyClass, string keyColor)
        {
            constructDefaults();

            GoText lab = new GoText();
            lab.Text = "New Legend Item";

            Add(obj);
            Add(lab);

            MyObject = obj;
            MyLabel = lab;

            MyLabel.Wrapping = true;
            MyLabel.WrappingWidth = MyObject.Width * 3;

            ClassKey = keyClass;
            ColorKey = keyColor;
            Visibility = true;
        }

        private void constructDefaults()
        {
            Orientation = Orientation.Horizontal;
            Alignment = GoObject.Middle;
            Spacing = 5;
            DragsNode = true;
            Selectable = true;
            Deletable = false;
            Copyable = false;
        }

        private GoObject myObject;
        public GoObject MyObject
        {
            get { return myObject; }
            set
            {
                myObject = value;
                myObject.DragsNode = true;
                myObject.Selectable = true;
                myObject.Deletable = false;
            }
        }
        private GoText myLabel;
        public GoText MyLabel
        {
            get { return myLabel; }
            set
            {
                myLabel = value;
                myLabel.DragsNode = true;
                myLabel.Selectable = true;
                myLabel.Editable = true;
                myLabel.Alignment = GoObject.Middle;
                myLabel.Deletable = false;
            }
        }

        private bool visibility;
        public bool Visibility
        {
            get
            {
                return visibility;
            }
            set
            {
                visibility = value;
                if (myLabel != null)
                    if (visibility == true)
                    {
                        myLabel.TextColor = Color.Black;
                    }
                    else
                    {
                        myLabel.TextColor = Color.LightGray;
                    }
            }
        }

        private string classKey;
        public string ClassKey { get { return classKey.Trim(); } set { classKey = value; } }
        private string colorKey;
        public string ColorKey { get { return colorKey.Trim(); } set { colorKey = value; } }

        //private string key;
        public string Key
        {
            get
            {
                if (ClassKey != null && ClassKey.Length > 0 && ColorKey != null && ColorKey.Length > 0)
                    return ClassKey + "-" + ColorKey;
                if (ClassKey != null && ClassKey.Length > 0)
                    return ClassKey;
                if (ColorKey != null && ColorKey.Length > 0)
                    return ColorKey;

                return "I Cannot Find Your Key";
            }
        }

    }
}