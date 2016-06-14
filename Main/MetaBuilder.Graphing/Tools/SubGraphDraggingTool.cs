using System;
using System.Drawing;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Tools
{
    [Serializable]
    public class SubGraphDraggingTool : GoToolDragging
    {
        #region Fields (3) 

        private bool myOriginalDragsRealtime;
        private Color myTargetColor = Color.Empty;
        private GoSubGraph myTargetSubGraph;

        #endregion Fields 

        #region Constructors (1) 

        public SubGraphDraggingTool(GoView view) : base(view)
        {
        }

        #endregion Constructors 

        #region Methods (7) 

        // Public Methods (4) 

        public override void DoDragging(GoInputState evttype)
        {
            if (CurrentObject == null)
                return;
            if (MustBeMoving() && Selection.Primary != null)
            {
                View.DragsRealtime = false;
                GoSubGraph sg = FindSubGraph(LastInput.DocPoint, Selection.Primary);
                if (sg != null)
                {
                    // adding to a subgraph?
                    if (evttype == GoInputState.Continue)
                    {
                        if (myTargetSubGraph != sg && Selection.Primary.Parent != sg)
                        {
                            UnhighlightSubGraph();
                            myTargetSubGraph = sg;
                            HighlightSubGraph(Color.Red);
                        }
                    }
                    else if (evttype == GoInputState.Finish)
                    {
                        if (Selection.Primary.Parent != sg)
                        {
                            // collapsed?  gotta expand first
                            bool expandfirst = !sg.IsExpanded;
                            if (expandfirst)
                                sg.Expand();
                            base.DoDragging(evttype);
                            sg.AddCollection(EffectiveSelection, true);
                            if (expandfirst)
                                sg.Collapse();
                            return; // DoDragging already done
                        }
                    }
                }
                else
                {
                    // adding at top-level
                    UnhighlightSubGraph();
                    base.DoDragging(evttype);
                    // no dynamic feedback, while evttype == GoInputState.Continue
                    if (evttype == GoInputState.Finish)
                    {
                        View.Document.Layers.Default.AddCollection(Selection, true);
                    }
                    return; // DoDragging already done
                }
            }
            else
            {
                View.DragsRealtime = myOriginalDragsRealtime;
                UnhighlightSubGraph();
            }
            base.DoDragging(evttype);
        }

        public virtual GoSubGraph FindSubGraph(PointF pt, GoObject skip)
        {
            foreach (GoObject obj in View.Document.Backwards)
            {
                GoSubGraph sg = FindSubGraph1(obj, pt, skip);
                if (sg != null)
                    return sg;
            }
            return null;
        }

        public override void Start()
        {
            myOriginalDragsRealtime = View.DragsRealtime;
            base.Start();
        }

        public override void Stop()
        {
            View.DragsRealtime = myOriginalDragsRealtime;
            UnhighlightSubGraph();
            base.Stop();
        }


        // Private Methods (3) 

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

        #endregion Methods 
    }
}