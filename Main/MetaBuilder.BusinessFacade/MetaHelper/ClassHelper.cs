#region Copyright © 2007 - DISCON Specialists

//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: ClassHelper.cs
//

#endregion

using System.Data;
using MetaBuilder.DataAccessLayer.OldCode.Meta;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
namespace MetaBuilder.BusinessFacade.MetaHelper
{
	/// <summary>
	/// Summary description for ClassHelper.
	/// </summary>
	public class ClassHelper
	{
		public ClassHelper()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public DataSet GetClassesAndFields(bool ActiveOnly)
		{
			ClassDefinitionAdapter cdefAdapter = new ClassDefinitionAdapter();
            return cdefAdapter.GetClassesAndFields(ActiveOnly);
		}

        public DataView GetClasses(bool ActiveOnly)
		{
			ClassDefinitionAdapter cdefAdapter = new ClassDefinitionAdapter();
            return cdefAdapter.GetClasses(ActiveOnly);
		}

        public DataView GetAllowedAssociateClasses(string ParentClassName, bool ActiveOnly)
		{
			ClassDefinitionAdapter cdefAdapter = new ClassDefinitionAdapter();
			return cdefAdapter.GetAllowedAssociateClasses(ParentClassName,ActiveOnly);
		}

		public DataView GetAllowedAssociateClasses(string ParentClassName, string LimitToCaption,bool ActiveOnly)
		{
			ClassDefinitionAdapter cdefAdapter = new ClassDefinitionAdapter();
			return cdefAdapter.GetAllowedAssociateClasses(ParentClassName, LimitToCaption,ActiveOnly);
		}

		public DataView GetAllowedAssociateClasses(string ParentClassName, int LimitToAssociationType, bool ActiveOnly)
		{
			ClassDefinitionAdapter cdefAdapter = new ClassDefinitionAdapter();
			return cdefAdapter.GetAllowedAssociateClasses(ParentClassName, LimitToAssociationType,ActiveOnly);
		}

		public DataView GetCategories()
		{
			ClassDefinitionAdapter cdefAdapter = new ClassDefinitionAdapter();
			return cdefAdapter.GetCategories();
		}
	}
}