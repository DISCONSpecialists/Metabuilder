

#region Copyright © 2007 - DISCON Specialists

//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: ObjectAssociation.cs
//

#endregion

namespace MetaBuilder.BusinessFacade.MetaHelper
{
	/// <summary>
	/// Summary description for ObjectAssociation.
	/// </summary>
	public class ObjectAssociation
	{
		public ObjectAssociation()
		{
		}

		public ObjectAssociation(int CAid, int ObjectID, int ChildObjectID, string ParentClass, string ChildClass,string ParentMachine,string ChildMachine)
		{
			this.CAid = CAid;
			this.ObjectID = ObjectID;
			this.ChildObjectID = ChildObjectID;
			this.ParentClass = ParentClass;
			this.ChildClass = ChildClass;
            this.ParentMachine = ParentMachine;
            this.ChildMachine = ChildMachine;
		}

		public int ObjectID;
		public int ChildObjectID;
		public int CAid;
		public string ChildClass;
		public string ParentClass;
        public string ParentMachine;
        public string ChildMachine;
	}

}