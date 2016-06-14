using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Controllers
{
    /// <summary>
    /// Primarily used to order shapes before and behind each other in a GoView
    /// </summary>
    public class ShapeOrderingControl
    {
        #region Ordering

        public static void BringForward(GoView view)
        {
            GoObject obj = view.Selection.Primary;
            IGoCollection collection;
            if (obj.Parent is GraphNode)
            {
                collection = obj.Parent as GraphNode;
            }
            else
            {
                collection = view.Document;
            }
            if (!(obj is IGoLink))
            {
                int SelectedObjectIndex = -1;
                GoLayer l = obj.Layer;
                int i = 0;
                foreach (GoObject child in collection)
                {
                    if (child == obj)
                    {
                        SelectedObjectIndex = i;
                    }
                    else
                    {
                        if (SelectedObjectIndex >= 0 && (!(child is IGoLink) || (child is FrameLayerGroup)))
                        {
                            view.StartTransaction();
                            if (obj.Parent is GraphNode)
                            {
                                GoGroup grp = obj.Parent;
                                grp.InsertAfter(child, obj);
                            }
                            else
                            {
                                l.MoveAfter(child, obj);
                            }
                            view.FinishTransaction("Bring Forward");
                            return;
                        }
                    }
                    i++;
                }
            }
        }

        public static void BringToFront(GoObject obj, GoView view)
        {
            if (obj != null && !(obj is IGoLink))
            {
                view.StartTransaction();
                // Get the current position of this object
                GoLayer l = obj.Layer;
                if (obj.Parent is GraphNode)
                {
                    GraphNode shpContainer = obj.Parent as GraphNode;
                    shpContainer.InsertAfter(null, obj);
                }
                else
                {
                    l.MoveAfter(null, obj);
                }
                view.FinishTransaction("Bring to front");
            }
        }
        public static void BringToFront(GoView view)
        {
            //GoObject obj = view.Selection.Primary;
            foreach (GoObject obj in view.Selection)
            {
                if (obj != null && !(obj is IGoLink))
                {
                    view.StartTransaction();
                    // Get the current position of this object
                    GoLayer l = obj.Layer;
                    if (obj.Parent is GraphNode)
                    {
                        GraphNode shpContainer = obj.Parent as GraphNode;
                        shpContainer.InsertAfter(null, obj);
                    }
                    else
                    {
                        l.MoveAfter(null, obj);
                    }
                    view.FinishTransaction("Bring to front");
                }
            }
        }

        public static void SendBackward(GoView view)
        {
            GoObject obj = view.Selection.Primary;
            IGoCollection collection;
            if (obj.Parent is GraphNode)
            {
                collection = obj.Parent as GraphNode;
            }
            else
            {
                collection = view.Document;
            }
            if (!(obj is IGoLink))
            {
                int SelectedObjectIndex = -1;
                GoLayer l = obj.Layer;
                int i = 0;
                foreach (GoObject child in collection.Backwards)
                {
                    if (child == obj)
                    {
                        SelectedObjectIndex = i;
                    }
                    else
                    {
                        if (SelectedObjectIndex >= 0)
                        {
                            view.StartTransaction();
                            if (obj.Parent is GraphNode && (!(child is IGoLink) || (child is FrameLayerGroup)))
                            {
                                GraphNode grp = obj.Parent as GraphNode;
                                grp.InsertBefore(child, obj);
                            }
                            else
                            {
                                if (!(child is IGoLink) || (child is FrameLayerGroup))
                                    l.MoveBefore(child, obj);
                            }
                            view.FinishTransaction("Send Backward");
                            return;
                        }
                    }
                    i++;
                }
            }
        }

        public static void SendToBack(GoView view)
        {
            //GoObject obj = view.Selection.Primary;
            foreach (GoObject obj in view.Selection)
                SendToBack(obj, view);
        }

        public static void SendToBack(GoObject obj, GoView view)
        {
            if (obj != null)// && !(obj is IGoLink))
            {
                if (view != null)
                    view.StartTransaction();
                // Get the current position of this object
                GoLayer l = obj.Layer;
                if (obj.Parent is GraphNode)
                {
                    GraphNode shpContainer = obj.Parent as GraphNode;
                    if (shpContainer.Count > 2)
                    {
                        shpContainer.InsertBefore(shpContainer[1], obj);
                    }
                }
                else
                {
                    l.MoveBefore(null, obj);
                }
                if (obj.Parent is GoGroup)
                {
                    GoGroup grp = obj.Parent;
                    GoGroupEnumerator groupEnum = grp.GetEnumerator();
                    while (groupEnum.MoveNext())
                    {
                        if (groupEnum.Current != obj)
                        {
                            l.MoveBefore(groupEnum.Current, obj);
                        }
                    }
                }
                if (view != null)
                    view.FinishTransaction("Send to back");
            }
        }

        #endregion
    }
}