using System;
using System.Collections.Generic;
using MetaBuilder.Graphing.Shapes;

namespace MetaBuilder.UIControls.MetaTree
{
    [Serializable]
    public class GraphNodeCollectionNode : MetaTreeNode
    {

		#region Fields (2) 

        private string _className;
        private List<GraphNode> _graphNodes;

		#endregion Fields 

		#region Constructors (1) 

        public GraphNodeCollectionNode()
        {
            _graphNodes = new List<GraphNode>();
        }

		#endregion Constructors 

		#region Properties (2) 

        public string ClassName
        {
            get { return _className; }
            set
            {
                _className = value;
                Text = "List<" + value + ">";
            }
        }

        public List<GraphNode> GraphNodes
        {
            get { return _graphNodes; }
            set { _graphNodes = value; }
        }

		#endregion Properties 

		#region Methods (1) 


		// Public Methods (1) 

        public override void LoadChildren()
        {
            Nodes.Clear();
            foreach (GraphNode node in GraphNodes)
            {
                GraphObjectNode tnNodeObject = new GraphObjectNode();
                tnNodeObject.GraphNode = node;
                tnNodeObject.Text = node.MetaObject.ToString();
                tnNodeObject.Nodes.Add(new EmptyNode());
                Nodes.Add(tnNodeObject);
            }
        }


		#endregion Methods 

    }
}