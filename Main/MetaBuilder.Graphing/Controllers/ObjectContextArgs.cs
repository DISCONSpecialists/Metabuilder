using MetaBuilder.Graphing.Shapes;
using System;

namespace MetaBuilder.Graphing.Controllers
{
    public delegate void NodeObjectContextClickedEventHandler(object sender, NodeObjectContextArgs args);

    public class NodeObjectContextArgs : EventArgs
    {
        private IMetaNode myNode;

        private string originalDocumentName;

        public IMetaNode MyNode
        {
            get { return myNode; }
            set { myNode = value; }
        }

        public string OriginalDocumentName
        {
            get { return originalDocumentName; }
            set { originalDocumentName = value; }
        }
    }
}