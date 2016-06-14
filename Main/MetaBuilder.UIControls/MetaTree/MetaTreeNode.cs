using System;
using System.Windows.Forms;

namespace MetaBuilder.UIControls.MetaTree
{
    [Serializable]
    public abstract class MetaTreeNode : TreeNode
    {

		#region Methods (2) 


		// Public Methods (1) 

        public virtual void LoadChildren()
        {
        }



		// Internal Methods (1) 

        internal void ClearEmptyNodes()
        {
            if (Nodes.Count > 0)
                foreach (TreeNode n in Nodes)
                {
                    if (n is EmptyNode)
                    {
                        n.Remove();
                    }
                }
       /*     if (Nodes.Count == 0)
                Nodes.Add(new EmptyNode());*/
        }


		#endregion Methods 

    }
}