#region Copyright © 2007 - DISCON Specialists

//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: ValueMemberAttribute.cs
//

#endregion

using System;
using System.Data;
using System.Windows.Forms;

namespace MetaBuilder.Meta.Editors
{
	/// <summary>
	/// Summary description for ValueMemberAttribute.
	/// </summary>
	[AttributeUsage(AttributeTargets.All)]
	public class ValueMemberAttribute : Attribute
	{
		private string valMemb;

		public string ValuePropertyName
		{
			get { return this.valMemb; }
		}

		public void SelectByValue(ListBox lb, object val)
		{
			lb.SelectedItem = null;
			object found = null;
			foreach (object item in lb.Items)
			{
				if (GetValue(item) == val)
				{
					found = item;
				}
			}
			if (found != null)
			{
				lb.SelectedItem = found;
			}
		}

		public object GetValue(object obj)
		{
			if (this.valMemb == string.Empty)
			{
				return obj;
			}

			DataRowView drv = (DataRowView) obj;
			return drv[valMemb];

		}

		public ValueMemberAttribute(string valueMemberPropertyName)
		{
			this.valMemb = valueMemberPropertyName;
		}

	}
}