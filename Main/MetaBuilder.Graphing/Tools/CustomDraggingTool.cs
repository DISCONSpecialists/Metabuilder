using System;
using System.Drawing;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Tools
{
    [Serializable]
    public class CustomDraggingTool : GoToolDragging
    {
        #region Constructors (1)

        public CustomDraggingTool(GoView view)
            : base(view)
        {
        }

        #endregion Constructors

        #region Methods (1)

        // Public Methods (1) 

        public override void DoDragging(GoInputState evttype)
        {
            //if (this.CurrentObject == null) return;
            if (LastInput.Shift)
            {
                PointF first = FirstInput.DocPoint;
                PointF last = LastInput.DocPoint;
                if (Math.Abs(last.X - first.X) >= Math.Abs(last.Y - first.Y))
                {
                    LastInput.DocPoint = new PointF(last.X, first.Y);
                }
                else
                {
                    LastInput.DocPoint = new PointF(first.X, last.Y);
                }
                // keep ViewPoint up-to-date with DocPoint
                LastInput.ViewPoint = View.ConvertDocToView(LastInput.DocPoint);
            }

            if (CurrentObject == null)
                return;
            /*  if (Selection.Primary != null)
            {
              
                List<MappingCell> cells = FindMappingCells();
                foreach (MappingCell c in cells)
                {
                    Console.WriteLine(c.Label.Text);
                }
            }*/
            try
            {
                if (Selection != null)
                    if (Selection.Count > 0)
                        base.DoDragging(evttype);
            }
            catch
            {
            }
        }

        public override void DoDragDrop(IGoCollection coll, System.Windows.Forms.DragDropEffects allow)
        {
            //remove all from coll!

            base.DoDragDrop(coll, allow);
        }

        /*
        List<MappingCell> FindMappingCells()
        {
            GoCollectionEnumerator enumr = View.Selection.GetEnumerator();

            GoCollection colBeneath = new GoCollection();
            List<MappingCell> retval = new List<MappingCell>();
            
                View.PickObjectsInRectangle(colfalse, false, pfStart, true, colBeneath, 10000);
                GoCollectionEnumerator colEnumPickedObjects = colBeneath.GetEnumerator();
                while (colEnumPickedObjects.MoveNext())
                {
                    if (colEnumPickedObjects.Current.Parent is MappingCell)
                    {
                        retval.Add(colEnumPickedObjects.Current.Parent as MappingCell);
                    }
                    if (colEnumPickedObjects.Current.ParentNode is MappingCell)
                    {
                        retval.Add(colEnumPickedObjects.Current.ParentNode as MappingCell);
                    }
                }
                return retval;

        }*/

        public override GoSelection ComputeEffectiveSelection(IGoCollection coll, bool move)
        {
            if (move == false)
            {
                foreach (GoObject o in coll)
                {
                    if (o is MetaBuilder.Graphing.Shapes.QLink)
                    {
                        foreach (GoObject ob in (o as MetaBuilder.Graphing.Shapes.QLink))
                        {
                            if (ob is GoGroup)
                            {
                                //we want to only remove the custom mid group but cannot reference it so we have to 'guess' which group to remove
                                if ((ob as GoGroup).Last is MetaBuilder.Graphing.Shapes.FishLinkPort) //custom group does not contain this port
                                    continue;
                                if ((ob as GoGroup).Last == null) //custom group will have at least 1 object
                                    continue;
                                ob.Remove();
                            }
                            else
                            {
                                //System.Diagnostics.Debug.WriteLine(ob.ToString());
                            }
                        }
                    }
                }
            }
            return base.ComputeEffectiveSelection(coll, move);
        }

        #endregion Methods
    }
}