using System;

namespace MetaBuilder.Meta.Editors
{
	/// <summary>
	/// Summary description for DisplayMemberAttribute.
	/// </summary>
	[AttributeUsage(AttributeTargets.All)]
	public class DisplayMemberAttribute : Attribute
	{
		private string dispMemb;

		public string DisplayPropertyName
		{
			get { return this.dispMemb; }
		}

		public DisplayMemberAttribute(string displayMemberPropertyName)
		{
			this.dispMemb = displayMemberPropertyName;
		}
	}
}