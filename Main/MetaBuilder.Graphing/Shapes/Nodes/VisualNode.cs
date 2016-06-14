using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;
using System.Drawing;
using MetaBuilder.Graphing.Shapes.General;

namespace MetaBuilder.Graphing.Shapes.Nodes
{
    [Serializable]
    public class VisualNode : GoTextNode
    {
        public VisualNode() //Used on load
        {
            SetupNode(false);
        }
        public VisualNode(GoObject anchor) //Used on creation
        {
            Location = new PointF(anchor.Right + 100, anchor.Top - 25);
            Text = "Enter Text";
            SetupNode(true);
        }
        public void SetupNode(bool setupPorts)
        {
            Editable = false;
            Resizable = false;
            AutoRescales = true;
            Brush = null;
            Pen = null;
            Background = null;

            if (setupPorts)
                SetupPorts();
        }

        public void SetupPorts()
        {
            VisualPort p = new VisualPort();
            p.Location = new PointF(Location.X - 10, Location.Y + (Bounds.Height / 2));
            Port = p;
            Add(p);
        }

        protected override GoPort CreatePort(int spot)
        {
            //so we dont make the 4 ports around
            return null;
        }

        private VisualPort port;
        public VisualPort Port
        {
            get
            {
                return port;
            }
            set
            {
                port = value;
            }
        }
    }
}