#region Copyright © 2007 - DISCON Specialists
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: ArtifactNode.cs
//
#endregion

using MetaBuilder.Meta;

namespace MetaBuilder.UIControls.Tree
{
    /// <summary>
    /// Node containing ObjectNodes. 
    /// </summary>
    public class ArtifactNode : ArtifactClassNode
    {

		#region Constructors (1) 

        public ArtifactNode()
        {
        }

		#endregion Constructors 


        #region Properties & Accessors
        public ArtifactNode(string nodeText)
        {
            Text = nodeText;
        }

        public MetaBase mbase
        {
            get
            {
                if (Tag != null)
                {
                    return (MetaBase) Tag;
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
                    return (IMetaBase) Tag;
                }
                return null;
            }
            set { Tag = value; }
        }
        #endregion
    }
}