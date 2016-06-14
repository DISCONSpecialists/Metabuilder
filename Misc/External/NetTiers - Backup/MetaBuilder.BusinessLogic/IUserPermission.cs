﻿using System;
using System.ComponentModel;

namespace MetaBuilder.BusinessLogic
{
	/// <summary>
	///		The data structure representation of the 'UserPermission' table via interface.
	/// </summary>
	/// <remarks>
	/// 	This struct is generated by a tool and should never be modified.
	/// </remarks>
	public interface IUserPermission 
	{
		/// <summary>			
		/// UserID : 
		/// </summary>
		/// <remarks>Member of the primary key of the underlying table "UserPermission"</remarks>
		System.Int32 UserID { get; set; }
				
		/// <summary>
		/// keep a copy of the original so it can be used for editable primary keys.
		/// </summary>
		System.Int32 OriginalUserID { get; set; }
			
		/// <summary>			
		/// WorkspaceName : 
		/// </summary>
		/// <remarks>Member of the primary key of the underlying table "UserPermission"</remarks>
		System.String WorkspaceName { get; set; }
				
		/// <summary>
		/// keep a copy of the original so it can be used for editable primary keys.
		/// </summary>
		System.String OriginalWorkspaceName { get; set; }
			
		/// <summary>			
		/// WorkspaceTypeId : 
		/// </summary>
		/// <remarks>Member of the primary key of the underlying table "UserPermission"</remarks>
		System.Int32 WorkspaceTypeId { get; set; }
				
		/// <summary>
		/// keep a copy of the original so it can be used for editable primary keys.
		/// </summary>
		System.Int32 OriginalWorkspaceTypeId { get; set; }
			
		
		
		/// <summary>
		/// PermissionID : 
		/// </summary>
		System.Int32  PermissionID  { get; set; }
			
		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		System.Object Clone();
		
		#region Data Properties

		#endregion Data Properties

	}
}


