#region Copyright © 2007 - DISCON Specialists
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: CaptionNode.cs
//
#endregion

namespace MetaBuilder.UIControls.Tree
{
    /// <summary>
    /// Node containing ObjectNodes. 
    /// </summary>
    public class CaptionNode : MetaTreeNode
    {

		#region Constructors (2) 

        public CaptionNode(string nodeText)
        {
            Text = nodeText;
        }

        public CaptionNode()
        {
        }

		#endregion Constructors 


        #region Properties & Accessors
        private int caid;

        public int CAid
        {
            get { return caid; }
            set { caid = value; }
        }

        private string childclass;

        public string ChildClass
        {
            get { return childclass; }
            set { childclass = value; }
        }

        private string parentclass;

        public string ParentClass
        {
            get { return parentclass; }
            set { parentclass = value; }
        }

        private int associationTypeID;

        public int AssociationTypeID
        {
            get { return associationTypeID; }
            set { associationTypeID = value; }
        }
        #endregion
    }
}