using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;
using Northwoods.Go.Draw;
using System.Drawing;

namespace MetaBuilder.Graphing.Layout
{
    public class RowCombine
    {
        //private GoView View;
        private GoNode ParentNode;
        public RowCombine(GoNode parentNode)//GoView view,
        {
            //View = view;
            ParentNode = parentNode;
            layout();
        }
        private void layout()
        {
            int childCount = ParentNode.Nodes.Count;
            int startCount = (childCount - (childCount / 2)) * -1;
            float mid = ParentNode.Location.X + (ParentNode.Width / 2);
            float maxWidth = 0;
            foreach (GoNode n in ParentNode.Nodes)
                maxWidth = n.Width > maxWidth ? n.Width : maxWidth;
            foreach (GoNode n in ParentNode.Nodes)
            {
                n.Location = new PointF(mid + ((maxWidth + 25) * startCount), ParentNode.Location.Y + ParentNode.Height + 50);
                startCount += 1;
                //relink
                MetaBuilder.Graphing.Shapes.QLink thisLink = null;
                foreach (GoLabeledLink l in ParentNode.Links)
                {
                    if (!(l is MetaBuilder.Graphing.Shapes.QLink))
                        break;

                    if (l.ToNode == n)
                        thisLink = (MetaBuilder.Graphing.Shapes.QLink)l;
                }
                if (thisLink != null)
                    MetaBuilder.Graphing.Shapes.QLink.MoveLink(ParentNode, n, MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Bottom, MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Top, thisLink);
            }
        }
    }

    public class ColumnCombine
    {
        //private GoView View;
        private GoNode ParentNode;
        public ColumnCombine(GoNode parentNode)//GoView view, 
        {
            //View = view;
            ParentNode = parentNode;
            layout();
        }
        private void layout()
        {
            int count = 1;
            float mid = ParentNode.Location.X + (ParentNode.Width / 2);
            foreach (GoNode n in ParentNode.Nodes)
            {
                n.Location = new PointF(mid + 25, ParentNode.Location.Y + (count * n.Height));
                count += 1;
                //relink
                MetaBuilder.Graphing.Shapes.QLink thisLink = null;
                foreach (GoLabeledLink l in ParentNode.Links)
                {
                    if (!(l is MetaBuilder.Graphing.Shapes.QLink))
                        break;

                    if (l.ToNode == n)
                        thisLink = (MetaBuilder.Graphing.Shapes.QLink)l;
                }
                if (thisLink != null)
                    MetaBuilder.Graphing.Shapes.QLink.MoveLink(ParentNode, n, MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Bottom, MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Left, thisLink);
            }
        }
    }
}