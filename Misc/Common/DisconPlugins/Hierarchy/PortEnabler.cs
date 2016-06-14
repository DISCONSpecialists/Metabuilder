using System;
using System.Collections.Generic;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;

namespace DisconPlugins.Hierarchy
{
    public class PortEnabler
    {

        #region Fields (1)

        private GraphView myView;

        #endregion Fields

        #region Properties (1)

        public GraphView MyView
        {
            get { return myView; }
            set { myView = value; }
        }

        #endregion Properties

        #region Methods (1)


        // Public Methods (1) 

        public void Execute()
        {

            myView.StartTransaction();
            GoLayerCollectionObjectEnumerator nodeEnum = MyView.Document.GetEnumerator();
            List<GraphNode> oldNodes = new List<GraphNode>();
            while (nodeEnum.MoveNext())
            {
                // Console.WriteLine(nodeEnum.Current.ToString());
                if (nodeEnum.Current is GraphNode)
                {
                    oldNodes.Add(nodeEnum.Current as GraphNode);
                }
            }

            myView.FinishTransaction("Replace Shapes");
        }


        #endregion Methods

    }
}
