

#region Copyright © 2007 - DISCON Specialists

//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: Association.cs
//

#endregion

using System;

namespace MetaBuilder.Meta
{
    [Serializable]
	public class Association
	{
        public Association Copy()
        {
            Association retval = new Association();
            if (AssociationType!=null)
                retval.AssociationType = this.AssociationType;
            retval.AssociationTypeID = this.AssociationTypeID;
            retval.Caption = this.Caption;
            retval.ChildClass = this.ChildClass;
            retval.ID = this.ID;
            retval.IsDefault = this.IsDefault;
            retval.ParentClass = this.ParentClass;
            return retval;
        }
		public Association()
		{
		}

		public int ID
		{
			get { return id; }
			set { id = value; }
		}

		private int id;

		public string ParentClass
		{
			get { return parentclass; }
			set { parentclass = value; }
		}

		private string parentclass;

		private string childclass;

		public string ChildClass
		{
			get { return childclass; }
			set { childclass = value; }
		}

        private int associationTypeId;
        public int AssociationTypeID
        {
            get { return associationTypeId; }
            set { associationTypeId = value; }
        }

        private string associationType;
        public string AssociationType
        {
            get { return associationType; }
            set { associationType = value; }

        }
        private bool isDefault;
        public bool IsDefault
        {
            get { return isDefault; }
            set { isDefault = value; }
        }
        private string caption;
        public string Caption
        {
            get { return caption; }
            set { caption = value; }
        }
        public string DisplayMember
        {
            get
            {
                return ChildClass + " (" + caption + ")";
            }
        }
	}
}