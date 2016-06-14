/*
 *  Copyright © Northwoods Software Corporation, 1998-2007. All Rights
 *  Reserved.
 *
 *  Restricted Rights: Use, duplication, or disclosure by the U.S.
 *  Government is subject to restrictions as set forth in subparagraph
 *  (c) (1) (ii) of DFARS 252.227-7013, or in FAR 52.227-19, or in FAR
 *  52.227-14 Alt. III, as applicable.
 */

using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Northwoods.Go;

namespace ShapeBuilding.TextToHierarchy
{
    // Each node that wants to be "sticky" needs to implement this interface
    // in order to participate in StickyDraggingTool's special behavior.
    // Each node can only be stuck to one other sticky node, but of course
    // many stickers can be stuck onto one node.
    // This policy creates a tree-like structure relationship between IStickers.
    public interface ISticker : IGoNode
    {
        // declare that S is now stuck on this node at port P
        void AddSticker(IGoPort p, ISticker s);

        // declare that S is no longer stuck on port P
        void RemoveSticker(IGoPort p, ISticker s);

        // find all the GoObject IStickers that are stuck on port P
        GoCollectionEnumerator GetStickers(IGoPort p);

        // the ISticker that this node is stuck to
        ISticker StuckTo { get; set; }
    }


    // An example node that implements ISticker.
    // This example class uses the GoObject.Observers collection to
    // maintain references to other objects.  It would be more efficient
    // to implement a separate list of objects, but it was expedient to
    // reuse the Observer mechanism instead of reimplementing it.
    [Serializable]
    public class StickyNode : GoTextNode, ISticker
    {
        // Let users resize these nodes; text changes don't resize the Background.
        public StickyNode()
        {
            this.Brush = Brushes.White;
            this.Text = "Sticky";
            this.Label.Alignment = Middle;
            this.AutoResizes = false;
            this.Size = new SizeF(100, 70);
            this.Resizable = true;
            this.Reshapable = true;
            this.ResizesRealtime = true;
        }

        // Have each port appear as a small "X".
        protected override GoPort CreatePort(int spot)
        {
            GoPort p = base.CreatePort(spot);
            p.Style = GoPortStyle.Times;
            p.AutoRescales = false;
            p.IsValidFrom = false;  // don't allow normal drawing of new links
            p.IsValidTo = false;
            return p;
        }

        // Position the ports so that they are centered on the edges,
        // rather than just inside them.
        public override void LayoutChildren(GoObject childchanged)
        {
            if (this.Initializing) return;
            base.LayoutChildren(childchanged);
            GoObject back = this.Background;
            if (back != null)
            {
                if (this.TopPort != null)
                    this.TopPort.SetSpotLocation(Middle, back, MiddleTop);
                if (this.RightPort != null)
                    this.RightPort.SetSpotLocation(Middle, back, MiddleRight);
                if (this.BottomPort != null)
                    this.BottomPort.SetSpotLocation(Middle, back, MiddleBottom);
                if (this.LeftPort != null)
                    this.LeftPort.SetSpotLocation(Middle, back, MiddleLeft);
            }
        }

        // for debugging:
        public override String GetToolTip(GoView view)
        {
            String msg = "stuck to: ";
            if (this.StuckTo != null)
                msg += ((IGoLabeledPart)this.StuckTo).Text;
            else
                msg += "(none)";
            foreach (IGoPort p in this.Ports)
            {
                foreach (GoObject x in GetStickers(p))
                {
                    IGoLabeledPart n = x as IGoLabeledPart;
                    if (n != null)
                    {
                        msg += "\n" + n.Text;
                    }
                }
            }
            return msg;
        }

        public void AddSticker(IGoPort p, ISticker s)
        {
            if (p.Node != this) throw new ArgumentException("Port isn't part of this node.");
            // use the port's GoObject.Observers collection to hold references to ISticker's
            // that are stuck on this port
            p.GoObject.AddObserver(s.GoObject);
            s.StuckTo = this;  // remember back-pointer
            // make BOLD to indicate that there are stickers
            this.Label.Bold = true;
        }

        public void RemoveSticker(IGoPort p, ISticker s)
        {
            if (p.Node != this) throw new ArgumentException("Port isn't part of this node.");
            // remove the ISticker from the port's GoObject.Observers collection
            p.GoObject.RemoveObserver(s.GoObject);
            s.StuckTo = null;
            // update the Label to indicate whether there are any
            // IStickers stuck on this node
            bool found = false;
            foreach (IGoPort x in this.Ports)
            {
                foreach (GoObject y in x.GoObject.Observers)
                {
                    if (y is ISticker)
                    {
                        found = true;
                        break;
                    }
                }
            }
            this.Label.Bold = found;
        }

        // get all the GoObjects that are stuck at the port
        public GoCollectionEnumerator GetStickers(IGoPort p)
        {
            return p.GoObject.Observers;
        }

        // Remember a "back-pointer" from a sticker to the ISticker that it is stuck on.
        public ISticker StuckTo
        {
            get
            {
                foreach (GoObject obj in this.Observers)
                {
                    ISticker s = obj as ISticker;
                    if (s != null) return s;
                }
                return null;
            }
            set
            {
                ISticker s = this.StuckTo;
                if (s != value && s != this)
                {  // can't stick to oneself!
                    if (s != null)
                    {
                        // use the node's GoObject.Observers collection to hold the back pointer
                        RemoveObserver(s.GoObject);
                        // make sure no stickee ports still think they have this ISticker stuck to them
                        foreach (IGoPort sp in s.Ports)
                        {
                            s.RemoveSticker(sp, this);
                        }
                    }
                    if (value != null)
                    {
                        // use the node's GoObject.Observers collection to hold the back pointer
                        AddObserver(value.GoObject);
                    }
                }
            }
        }
    }


    // Moving an ISticker node will automatically move any other IStickers that are
    // stuck onto the selected sticky nodes.
    // To use, replace the standard dragging tool with this one:
    //   myView.ReplaceMouseTool(typeof(GoToolDragging), new StickyDraggingTool(myView));
    //   myView.DragsRealtime = true;
    [Serializable]
    public class StickyDraggingTool : GoToolDragging
    {
        public StickyDraggingTool(GoView view) : base(view) { }

        // Produce a selection that includes all sticky nodes starting with the selected nodes
        public override GoSelection ComputeEffectiveSelection(IGoCollection sel, bool move)
        {
            GoSelection result = base.ComputeEffectiveSelection(sel, move);
            if (move) AddStickies(result);
            return result;
        }

        // Extend the collection by recursively adding all of the nodes stuck to any selected node
        public static void AddStickies(IGoCollection sel)
        {
            GoCollection coll = new GoCollection();
            foreach (GoObject obj in sel)
            {
                AddReachable(coll, obj as ISticker);
            }
            foreach (GoObject obj in coll)
            {
                sel.Add(obj);
            }
        }

        // recurse through graph of Stickers
        private static void AddReachable(GoCollection coll, ISticker s)
        {
            if (s == null) return;
            if (!coll.Contains(s.GoObject))
            {
                coll.Add(s.GoObject);
                foreach (IGoPort p in s.Ports)
                {
                    foreach (GoObject x in s.GetStickers(p))
                    {
                        ISticker s2 = x as ISticker;
                        AddReachable(coll, s2);
                    }
                }
            }
        }

        // When done dragging, make sure all the sticky nodes are connected to each other
        // if they are near to each other.
        public override void DoDragging(GoInputState evttype)
        {
            base.DoDragging(evttype);
            if (this.CurrentObject == null)
                return;
            foreach (GoObject obj in this.Selection)
            {  // not EffectiveSelection!
                ISticker b = obj as ISticker;
                if (b != null)
                {
                    // assume dragged object will no longer be stuck to another ISticker
                    b.StuckTo = null;
                }
            }
            GoCollection coll = new GoCollection();
            foreach (GoObject obj in this.Selection)
            {  // not EffectiveSelection!
                ISticker b = obj as ISticker;
                if (b != null)
                {
                    coll.Clear();
                    this.View.Document.PickObjectsInRectangle(b.GoObject.Bounds, GoPickInRectangleStyle.SelectableOnlyIntersectsBounds, coll, 999999);
                    foreach (GoObject near in coll)
                    {
                        if (near == b) continue;  // ignore the node being dragged
                        ISticker a = near as ISticker;
                        if (a != null)
                        {
                            // don't get confused by other nodes being dragged
                            if (this.EffectiveSelection.Contains(a.GoObject)) continue;

                            IGoPort ap, bp;
                            if (ShouldStick(a, b, out ap, out bp))
                            {
                                if (evttype == GoInputState.Finish)
                                {
                                    // now tell (port of) A that B is stuck to it
                                    a.AddSticker(ap, b);
                                }
                                else
                                {
                                    HighlightPort(ap);
                                    SnapToPort(ap, bp, b);
                                }
                                return;  // stop with the first successful sticky port
                            }
                        }
                    }
                }
            }
            HighlightPort(null);
        }

        // If any ports overlap, return true (and return them via the OUT parameters)
        public virtual bool ShouldStick(ISticker stickee, ISticker sticker, out IGoPort portA, out IGoPort portB)
        {
            foreach (IGoPort ap in stickee.Ports)
            {
                foreach (IGoPort bp in sticker.Ports)
                {
                    RectangleF ar = ap.GoObject.Bounds;
                    RectangleF br = bp.GoObject.Bounds;
                    if (ar.IntersectsWith(br))
                    {
                        portA = ap;
                        portB = bp;
                        return true;
                    }
                }
            }
            portA = null;
            portB = null;
            return false;
        }

        public override void Stop()
        {
            HighlightPort(null);
            base.Stop();
        }

        // Display a red highlight rectangle around the given port,
        // or hide that rectangle when the given port P is null.
        // The rectangle is not added to the document, but to the view.
        public virtual void HighlightPort(IGoPort p)
        {
            if (p != null)
            {
                // create the highlight rectangle, if needed
                if (myHighlight == null)
                {
                    GoRectangle h = new GoRectangle();
                    h.Pen = new Pen(Color.Red, 2);
                    myHighlight = h;
                }
                // show it, if not already visible
                if (!myHighlight.IsInView)
                {
                    this.View.Layers.Default.Add(myHighlight);
                }
                // position and size it according to the given port
                RectangleF r = p.GoObject.Bounds;
                r.Inflate(2, 2);
                myHighlight.Bounds = r;
            }
            else
            {
                // remove the highlight rectangle
                if (myHighlight != null)
                {
                    myHighlight.Remove();
                }
            }
        }

        // move the effective selection so that the centers of the ports are coincident
        public virtual void SnapToPort(IGoPort ap, IGoPort bp, ISticker b)
        {
            if (ap != null && bp != null && b != null)
            {
                SizeF off = SubtractPoints(ap.GoObject.Center, bp.GoObject.Center);
                this.View.MoveSelection(this.EffectiveSelection, off, false);
            }
        }

        private GoObject myHighlight = null;
    }
}
