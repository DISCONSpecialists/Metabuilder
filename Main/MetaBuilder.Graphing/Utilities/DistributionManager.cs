using System.Collections.Generic;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Utilities
{
    public class DistributionManager
    {
        #region Shape Distribution

        private GoSelection _selection;

        public GoSelection Selection
        {
            get { return _selection; }
            set { _selection = value; }
        }

        public void DistributeShapes(GoSelection selection, DistributionType distributionType)
        {
            _selection = selection;
            if (Selection.Count <= 1)
                return;
            // easier to work with List<>
            List<GoObject> objectList = new List<GoObject>();
            GoCollectionEnumerator shapeEnumerator = Selection.GetEnumerator();
            while (shapeEnumerator.MoveNext())
            {
                if (!(shapeEnumerator.Current is GoPort) && (!(shapeEnumerator.Current is GoLink)) &&
                    (!(shapeEnumerator.Current is QLink)))
                    objectList.Add(shapeEnumerator.Current);
            }
            SortSelectionForDistributionType(objectList, distributionType);
            float left = 10000;
            float right = -31000;
            float top = 10000;
            float bottom = -31000;

            #region Set Outermost Coordinates

            foreach (GoObject o in objectList)
            {
                if (o.Left < left)
                    left = o.Left;
                if (o.Right > right)
                    right = o.Right;
                if (o.Top < top)
                    top = o.Top;
                if (o.Bottom > bottom)
                    bottom = o.Bottom;
            }

            #endregion

            int itemCount = objectList.Count;
            float ShapeHeightSum = 0f;
            float ShapeWidthSum = 0f;

            #region Get collective height & width of shapes only

            foreach (GoObject o in objectList)
            {
                switch (distributionType)
                {
                    case DistributionType.HorizontalGaps:
                        ShapeWidthSum += o.Width;
                        break;
                    case DistributionType.VerticalGaps:
                        ShapeHeightSum += o.Height;
                        break;
                }
            }

            #endregion

            float diffY = (bottom - top) - ShapeHeightSum;
            float diffX = (right - left) - ShapeWidthSum;
            float vSpacing = diffY / float.Parse((itemCount - 1).ToString(), System.Globalization.CultureInfo.InvariantCulture);
            float hSpacing = diffX / float.Parse((itemCount - 1).ToString(), System.Globalization.CultureInfo.InvariantCulture);
            float prevX = objectList[0].Right;
            float prevY = objectList[0].Bottom;
            // Sort the items according to the requested distribution type
            for (int i = 1; i < objectList.Count; i++)
            {
                //if ((objectList[i] is Graphing.Shapes.QLink))
                {
                    switch (distributionType)
                    {
                        case DistributionType.HorizontalGaps:
                            objectList[i].Left = prevX + hSpacing;
                            prevX = objectList[i].Right;
                            break;
                        case DistributionType.VerticalGaps:
                            objectList[i].Top = prevY + vSpacing;
                            prevY = objectList[i].Bottom;
                            break;
                    }
                }
            }
        }

        private void SortSelectionForDistributionType(List<GoObject> list, DistributionType distroType)
        {
            for (int outer = 0; outer <= list.Count - 2; outer++)
            {
                for (int inner = outer + 1; inner <= list.Count - 1; inner++)
                {
                    if ((list[outer].Right > list[inner].Right) && (distroType == DistributionType.HorizontalGaps))
                    {
                        SwitchObjects(outer, inner, ref list);
                        continue;
                    }
                    if ((list[outer].Bottom > list[inner].Bottom) && (distroType == DistributionType.VerticalGaps))
                    {
                        SwitchObjects(outer, inner, ref list);
                        continue;
                    }
                }
            }
            
        }

        private void SwitchObjects(int first, int second, ref List<GoObject> lst)
        {
            GoObject oTemp = lst[first];
            if (!(oTemp is GoLink) && !(lst[second] is GoLink))
            {
                lst[first] = lst[second];
                lst[second] = oTemp;
            }
        }

        #endregion
    }

    public enum DistributionType
    {
        VerticalGaps,
        HorizontalGaps
    }
}