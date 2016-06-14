using System.Drawing;
using MetaBuilder.Graphing.Shapes.Nodes.Containers;
using Northwoods.Go;
using System;

namespace MetaBuilder.Graphing.Shapes.General
{
    [Serializable]
    public class NonPrintingQuickPort : QuickPort
    {
        #region Constructors (1)

        public NonPrintingQuickPort()
        {
            Brush = null;
            Pen = new Pen(Color.Silver);
        }

        public override IGoNode Node
        {
            get
            {
                if (base.Node == null)
                    if (Parent is ValueChain)
                        return Parent as ValueChain;
                return base.Node;
            }
        }

        #endregion Constructors

        #region Methods (1)

        // Public Methods (1) 

        // only seen when the mouse is over it
        public override bool OnEnterLeave(GoObject from, GoObject to, GoView view)
        {
            if (from == this)
                Brush = null;
            if (to == this)
                Brush = Brushes.Gray;
            return true;
        }

        #endregion Methods
    }
}