using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Controllers
{
    public class FixSelfPorts
    {
        #region Constructors (1) 

        public FixSelfPorts(IGoCollection collection)
        {
            foreach (GoObject o in collection)
            {
                if (o is GraphNode)
                {
                    GraphNode node = o as GraphNode;
                    if (node.BindingInfo != null)
                    {
                        if (node.BindingInfo.BindingClass == "Object")
                        {
                            GoGroupEnumerator nodeEnum = node.GetEnumerator();
                            while (nodeEnum.MoveNext())
                            {
                                if (nodeEnum.Current is QuickPort)
                                {
                                    QuickPort qp = nodeEnum.Current as QuickPort;
                                    if (((qp.Position.X == node.Position.X + 135) &&
                                         (qp.Position.Y == node.Position.Y + 5)) ||
                                        (qp.Position.Y == node.Position.Y + 20))
                                    {
                                        qp.IsValidSelfNode = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion Constructors 
    }
}