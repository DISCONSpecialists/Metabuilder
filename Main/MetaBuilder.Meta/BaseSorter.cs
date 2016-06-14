#region Copyright © 2007 - DISCON Specialists

//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: BaseSorter.cs
//

#endregion

using System;
using System.ComponentModel;

namespace MetaBuilder.Meta
{
	/// <summary>
	/// Summary description for BaseSorter.
	/// </summary>
	public abstract class BaseSorter : ExpandableObjectConverter
	{
		protected string[] SortOrder;

		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value, attributes);

			return properties.Sort(SortOrder);
		}
	}
}