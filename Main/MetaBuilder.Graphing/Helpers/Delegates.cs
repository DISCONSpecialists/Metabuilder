using System.Collections.Generic;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;
using System.Collections.ObjectModel;

namespace MetaBuilder.Graphing.Helpers
{
    public delegate void GraphNodesContextClickedHandler(Collection<GraphNode> nodeCollection);

    public delegate void GraphNodesSelectedHandler(Collection<GraphNode> nodeCollection);

    public delegate void LinksSelectedHandler(Collection<QLink> nodeCollection);

    public delegate void SelectionContextClickedHandler(GoSelection selection);
}