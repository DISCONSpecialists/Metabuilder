using System;
using System.Runtime.Serialization;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Meta;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.UIControls.MetaTree
{
    [Serializable]
    public class GraphObjectNode : ObjectNode
    {

		#region Fields (1) 

        private GraphNode _graphNode;

		#endregion Fields 

		#region Constructors (2) 

        public GraphObjectNode(SerializationInfo info, StreamingContext context)
        {
            //    Text = "- empty =";
        }

        public GraphObjectNode()
        {
            _graphNode = new GraphNode();
            _graphNode.MetaObject = new MetaBase();
        }

		#endregion Constructors 

		#region Properties (2) 

        public GraphNode GraphNode
        {
            get { return _graphNode; }
            set
            {
                _graphNode = value;
                if (_graphNode.MetaObject != null)
                {
                    _graphNode.MetaObject.Changed += new EventHandler(_metaObject_Changed);
                }
                if (_graphNode != null)
                {
                    if (_graphNode.MetaObject != null)
                        Text = _graphNode.MetaObject.ToString();
                    else
                        Text = "-Empty-";
                }
                else
                    Text = "-Empty-";
            }
        }

        public override MetaBase MetaObject
        {
            get { return GraphNode.MetaObject; }
            set
            {
                if (_graphNode == null)
                {
                    _graphNode = new GraphNode();
                }
                GraphNode.MetaObject = value;
                Text = GraphNode.MetaObject.ToString();
            }
        }

		#endregion Properties 

    }
}