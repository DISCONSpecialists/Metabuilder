using System;
using System.Runtime.Serialization;

namespace MetaBuilder.UIControls.MetaTree
{

    [Serializable]
    public class EmptyNode : MetaTreeNode
    {

        #region Constructors (2)

        public EmptyNode(SerializationInfo info, StreamingContext context)
        {
            Text = "- empty =";
        }

        public EmptyNode()
        {
            Text = "- empty -";
        }

        #endregion Constructors

    }
}