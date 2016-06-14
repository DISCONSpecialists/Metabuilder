using System;
using System.Drawing;
using Northwoods.Go.Layout;

namespace MetaBuilder.Graphing.Layout
{
    [Serializable]
    public class SimpleLDAL : GoLayoutLayeredDigraph
    {
        #region Constructors (1) 

        #endregion Constructors 

        #region Methods (1) 

        // Public Methods (1) 

        public override void PerformLayout()
        {
            if (Document == null)
                throw new InvalidOperationException("Must set the Document property to non-null");
            if (Network == null)
                Network = new GoLayoutLayeredDigraphNetwork(Document);
            RaiseProgress(0);
            int num = Network.NodeCount;
            int i = 0;
            float radius = 150;
            double angle = 360.0/num;
            foreach (GoLayoutLayeredDigraphNode node in Network.Nodes)
            {
                node.Center = new PointF(300 + radius*(float) Math.Cos(i*angle*Math.PI/180),
                                         300 + radius*(float) Math.Sin(i*angle*Math.PI/180));
                node.CommitPosition();
                i++;
            }
            RaiseProgress(1);
        }

        #endregion Methods 
    }
}