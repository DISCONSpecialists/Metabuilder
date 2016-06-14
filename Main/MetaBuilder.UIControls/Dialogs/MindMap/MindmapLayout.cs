using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go.Layout;
using MetaBuilder.Graphing.Shapes.Nodes;

namespace MetaBuilder.UIControls.Dialogs.MindMap
{
    public class MindmapLayout : GoLayoutForceDirected
    {
        protected override bool IsFixed(GoLayoutForceDirectedNode node)
        {
            if (node.GoObject is MindMapNode)
                return (node.GoObject as MindMapNode).IsFixedLocation;

            return false;// base.IsFixed(node);
        }
    }
}
