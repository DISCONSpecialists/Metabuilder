
using System;

namespace MetaBuilder.BusinessLogic
{
	/// <summary>
	/// An enum representation of the 'WorkspaceType' table. [No description found in the database]
	/// </summary>
	/// <remark>this enumeration contains the items contained in the table WorkspaceType</remark>
	[Serializable]
	public enum WorkspaceTypeList
	{
		/// <summary> 
		/// Client
		/// </summary>
		Client = 2, 

		/// <summary> 
		/// Sandbox
		/// </summary>
		Sandbox = 1, 

		/// <summary> 
		/// Server
		/// </summary>
		Server = 3, 

		/// <summary> 
		/// Template
		/// </summary>
		Template = 4

	}
}
