#region Copyright © 2007 - DISCON Specialists
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: ArtifactPropertiesNode.cs
//
#endregion

using System.Windows.Forms;
using MetaBuilder.Meta;

namespace MetaBuilder.UIControls.Tree
{
    /// <summary>
    /// Node containing ObjectNodes.
    /// </summary>
    public class ArtifactPropertiesNode : TreeNode
    {

        #region Constructors (2)

        public ArtifactPropertiesNode(string nodeText)
        {
            Text = nodeText;
        }

        public ArtifactPropertiesNode()
        {
        }

        #endregion Constructors

        #region Properties & Accessors
        public MetaBase mbase
        {
            get
            {
                if (Tag != null)
                {
                    return (MetaBase)Tag;
                }
                return null;
            }
        }

        public IMetaBase ibase
        {
            get
            {
                if (Tag != null)
                {
                    return (IMetaBase)Tag;
                }
                return null;
            }
            set { Tag = value; }
        }

        private string propertiesclass;

        public string PropertiesClass
        {
            get { return propertiesclass; }
            set { propertiesclass = value; }
        }
        #endregion
    }
}