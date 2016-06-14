using Northwoods.Go;
using System.Collections.Generic;
using System.Drawing;
using MetaBuilder.Graphing.Shapes;

namespace MetaBuilder.Graphing.Layout
{
    public class FSDNode
    {
        private float maxChildWidth = 0, maxChildHeight = 0;
        public float MaxChildWidth { get { return maxChildWidth; } }
        public float MaxChildHeight { get { return maxChildHeight; } }

        private int level;
        public int Level
        {
            get
            {
                return level;
            }
            set
            {
                level = value;
            }
        }
        bool totaled;

        private int totalNodes;
        public int TotalNodes()
        {
            if (totaled)
                return totalNodes;

            totalNodes = 0;

            foreach (FSDNode n in ChildNodes)
                totalNodes += n.TotalNodes();

            totaled = true;
            return totalNodes;
        }

        private int treeWidth;
        public int TreeWidth()
        {
            treeWidth = 0;
            foreach (FSDNode n in ChildNodes)
            {
                treeWidth += (int)MaxChildWidth + n.TreeWidth();
                break;
            }
            return treeWidth;
        }

        private List<FSDNode> childNodes;
        public List<FSDNode> ChildNodes { get { return childNodes; } }
        private GoNode myNode;
        public GoNode MyNode { get { return myNode; } set { myNode = value; } }

        public PointF MyLocation
        {
            get { return MyNode.Location; }
            set { MyNode.Location = new PointF(value.X, value.Y); }
        }

        public FSDNode(IMetaNode node)
        {
            MyNode = node as GoNode;
            Level = 1;
            buildNodes(1);
        }
        public FSDNode(IMetaNode node, int level)
        {
            MyNode = node as GoNode;
            Level = level;
            buildNodes(level);
        }
        private void buildNodes(int level)
        {
            childNodes = new List<FSDNode>();
            foreach (GoNode n in MyNode.Destinations)
            {
                if (!(n is IMetaNode))
                    continue;

                childNodes.Add(new FSDNode(n as IMetaNode, level + 1));
                maxChildHeight = maxChildHeight < n.Height ? n.Height : maxChildHeight;
                maxChildWidth = maxChildWidth < n.Width ? n.Height : maxChildWidth;
            }
        }
    }
}