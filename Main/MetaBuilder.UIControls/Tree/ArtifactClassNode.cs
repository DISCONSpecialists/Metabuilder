#region Copyright © 2007 - DISCON Specialists
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: ArtifactClassNode.cs
//
#endregion
using System.Windows.Forms;

namespace MetaBuilder.UIControls.Tree
{
    /// <summary>
    /// Node containing ObjectNodes.
    /// </summary>
    public class ArtifactClassNode : TreeNode
    {

        #region Constructors (2)

        public ArtifactClassNode(string nodeText)
        {
            Text = nodeText;
        }

        public ArtifactClassNode()
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

        private int objectID;

        public int ObjectID
        {
            get { return objectID; }
            set { objectID = value; }
        }

        private int childObjectID;

        public int ChildObjectID
        {
            get { return childObjectID; }
            set { childObjectID = value; }
        }

        private string objectclass;

        public string ObjectClass
        {
            get { return objectclass; }
            set { objectclass = value; }
        }

        private string childobjectclass;

        public string ChildObjectClass
        {
            get { return childobjectclass; }
            set { childobjectclass = value; }
        }

        private string artifactclass;

        public string ArtifactClass
        {
            get { return artifactclass; }
            set { artifactclass = value; }
        }
        #endregion
    }
}