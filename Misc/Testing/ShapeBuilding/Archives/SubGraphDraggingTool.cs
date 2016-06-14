using System;
using System.Drawing;
using Northwoods.Go;
using Northwoods.Go.Draw;

namespace ShapeBuilding
{
    [Serializable]
    public class SubGraphDraggingTool : GoDrawToolDragging
    {
        public SubGraphDraggingTool(GoView view) : base(view) { }

        public override void Start()
        {
            myOriginalDragsRealtime = this.View.DragsRealtime;
            base.Start();
        }

        public override void Stop()
        {
            this.View.DragsRealtime = myOriginalDragsRealtime;
            UnhighlightSubGraph();
            base.Stop();
        }

        public virtual GoSubGraph FindSubGraph(PointF pt, GoObject skip)
        {
            foreach (GoObject obj in this.View.Document.Backwards)
            {
                GoSubGraph sg = FindSubGraph1(obj, pt, skip);
                if (sg != null)
                    return sg;
            }
            return null;
        }

        // recurse to find the front-most GoSubGraph at PT, other than SKIP itself
        private GoSubGraph FindSubGraph1(GoObject obj, PointF pt, GoObject skip)
        {
            if (!obj.CanView())
                return null;
            if (obj == skip || skip.IsChildOf(obj))
                return null;
            GoSubGraph sg = obj as GoSubGraph;
            if (sg != null)
            {
                RectangleF b = obj.Bounds;
                if (b.Contains(pt))
                {
                    foreach (GoObject o in sg)
                    {
                        GoSubGraph sub = FindSubGraph1(o, pt, skip);
                        if (sub != null)
                            return sub;
                    }
                    return sg;
                }
            }
            return null;
        }

        public override void DoDragging(GoInputState evttype)
        {
            if (this.CurrentObject == null)
                return;
            if (MustBeMoving() && this.Selection.Primary != null)
            {
                this.View.DragsRealtime = true;
                GoSubGraph sg = FindSubGraph(this.LastInput.DocPoint, this.Selection.Primary);
                if (sg != null)
                {
                    // adding to a subgraph?
                    if (evttype == GoInputState.Continue)
                    {
                        if (myTargetSubGraph != sg && this.Selection.Primary.Parent != sg)
                        {
                            UnhighlightSubGraph();
                            myTargetSubGraph = sg;
                            HighlightSubGraph(Color.Red);
                        }
                    }
                    else if (evttype == GoInputState.Finish)
                    {
                        if (this.Selection.Primary.Parent != sg)
                        {
                            // collapsed?  gotta expand first
                            bool expandfirst = !sg.IsExpanded;
                            if (expandfirst)
                                sg.Expand();
                            base.DoDragging(evttype);
                            sg.AddCollection(this.EffectiveSelection, true);
                            if (expandfirst)
                                sg.Collapse();
                            return;  // DoDragging already done
                        }
                    }
                }
                else
                {  // adding at top-level
                    UnhighlightSubGraph();
                    base.DoDragging(evttype);
                    // no dynamic feedback, while evttype == GoInputState.Continue
                    if (evttype == GoInputState.Finish)
                    {
                        this.View.Document.Layers.Default.AddCollection(this.Selection, true);
                    }
                    return;  // DoDragging already done
                }
            }
            else
            {
                this.View.DragsRealtime = myOriginalDragsRealtime;
                UnhighlightSubGraph();
            }
            base.DoDragging(evttype);
        }

        private void HighlightSubGraph(Color c)
        {
            if (myTargetSubGraph != null)
            {
                myTargetColor = myTargetSubGraph.BackgroundColor;
                myTargetSubGraph.SkipsUndoManager = true;
                myTargetSubGraph.BackgroundColor = c;
                myTargetSubGraph.SkipsUndoManager = false;
            }
        }

        private void UnhighlightSubGraph()
        {
            if (myTargetSubGraph != null)
            {
                myTargetSubGraph.SkipsUndoManager = true;
                myTargetSubGraph.BackgroundColor = myTargetColor;
                myTargetSubGraph.SkipsUndoManager = false;
                myTargetSubGraph = null;
            }
        }

        private bool myOriginalDragsRealtime = false;
        private GoSubGraph myTargetSubGraph = null;
        private Color myTargetColor = Color.Empty;
    }
}
