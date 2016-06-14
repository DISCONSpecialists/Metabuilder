/*
 *  Copyright © 2007 - DISCON Specialists
 *
 *
 *  
 *  
 *  
 *  
 */

using System;
using System.Drawing;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Tools
{
    /// <summary>
    /// This tool allows the user to drag a port around, assuming it is not automatically
    /// positioned by <see cref="GoGroup.LayoutChildren"/>.
    /// </summary>
    [Serializable]
    public class PortMovingTool : GoTool
    {
        #region Fields (3) 

        private SizeF myOffset;
        private PointF myOriginalPosition;
        private GoPort myPort;

        #endregion Fields 

        #region Constructors (1) 

        public PortMovingTool(GoView view) : base(view)
        {
        }

        #endregion Constructors 

        #region Methods (5) 

        // Public Methods (5) 

        /// <summary>
        /// See if there's a <see cref="GoPort"/> underneath the mouse--if so,
        /// remember it and its position so we can start to drag it.
        /// </summary>
        public override void DoMouseDown()
        {
            GoPort p = View.PickObject(true, false, LastInput.DocPoint, false) as GoPort;
            if (p != null)
            {
                myPort = p;
                // remember where the mouse is relative to the port's position
                myOffset = SubtractPoints(LastInput.DocPoint, p.Position);
                // and remember the port's original position, in case the user cancels by typing Escape
                myOriginalPosition = myPort.Position;
            }
            else
            {
                StopTool();
            }
        }

        /// <summary>
        /// Have the port being dragged follow the mouse.
        /// </summary>
        public override void DoMouseMove()
        {
            if (myPort != null)
            {
                PointF dp = LastInput.DocPoint;
                myPort.Position = new PointF(dp.X - myOffset.Width, dp.Y - myOffset.Height);
                if (myPort.Parent is GraphNode)
                {
                    GraphNode gnode = myPort.Parent as GraphNode;
                    myPort.Position = gnode.Grid.SnapPoint(myPort.Position, myPort, View);
                }
            }
        }

        /// <summary>
        /// Leave the port where it is, and indicate the transaction is successful.
        /// </summary>
        public override void DoMouseUp()
        {
            if (myPort != null)
            {
                myPort = null;
                TransactionResult = "moved port";
                StopTool();
            }
        }

        /// <summary>
        /// Start a transaction where the user is moving a port.
        /// </summary>
        public override void Start()
        {
            StartTransaction();
        }

        /// <summary>
        /// Stop the transaction, returning the port to its original position if
        /// the transaction is aborted.
        /// </summary>
        public override void Stop()
        {
            if (myPort != null && TransactionResult == null)
            {
                // if cancelling, put the port back
                myPort.Position = myOriginalPosition;
            }
            StopTransaction();
        }

        #endregion Methods 
    }
}