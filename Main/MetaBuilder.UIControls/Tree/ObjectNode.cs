#region Copyright © 2007 - DISCON Specialists
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: ObjectNode.cs
//
#endregion

using MetaBuilder.Core;
using MetaBuilder.Meta;

namespace MetaBuilder.UIControls.Tree
{
    /// <summary>
    /// Summary description for ObjectNode.
    /// </summary>
    public class ObjectNode : MetaTreeNode
    {

		#region Fields (1) 

        private bool showemptynodes;

		#endregion Fields 

		#region Constructors (2) 

        public ObjectNode(string nodeText)
        {
            Text = nodeText;
        }

        public ObjectNode()
        {
        }

		#endregion Constructors 

		#region Properties (5) 

        public string Class
        {
            get { return ibase.GetType().Name.Replace(Variables.MetaNameSpace + ".", ""); }
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

        public int ObjectID
        {
            get
            {
                if (mbase != null)
                {
                    return mbase.pkid;
                }
                return 0;
            }
        }

        public bool ShowEmptyNodes
        {
            get { return showemptynodes; }
            set { showemptynodes = value; }
        }

		#endregion Properties 

    }
}