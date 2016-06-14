﻿using System;
using System.ComponentModel;

namespace MetaBuilder.BusinessLogic
{
	/// <summary>
	///		The data structure representation of the 'DomainDefinitionPossibleValue' table via interface.
	/// </summary>
	/// <remarks>
	/// 	This struct is generated by a tool and should never be modified.
	/// </remarks>
	public interface IDomainDefinitionPossibleValue 
	{
		/// <summary>			
		/// DomainDefinitionID : 
		/// </summary>
		/// <remarks>Member of the primary key of the underlying table "DomainDefinitionPossibleValue"</remarks>
		System.Int32 DomainDefinitionID { get; set; }
				
		/// <summary>
		/// keep a copy of the original so it can be used for editable primary keys.
		/// </summary>
		System.Int32 OriginalDomainDefinitionID { get; set; }
			
		/// <summary>			
		/// PossibleValue : 
		/// </summary>
		/// <remarks>Member of the primary key of the underlying table "DomainDefinitionPossibleValue"</remarks>
		System.String PossibleValue { get; set; }
				
		/// <summary>
		/// keep a copy of the original so it can be used for editable primary keys.
		/// </summary>
		System.String OriginalPossibleValue { get; set; }
			
		
		
		/// <summary>
		/// Series : 
		/// </summary>
		System.Int32  Series  { get; set; }
		
		/// <summary>
		/// Description : 
		/// </summary>
		System.String  Description  { get; set; }
		
		/// <summary>
		/// IsActive : 
		/// </summary>
		System.Boolean?  IsActive  { get; set; }
		
		/// <summary>
		/// URI_ID : 
		/// </summary>
		System.Int32?  URI_ID  { get; set; }
			
		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		System.Object Clone();
		
		#region Data Properties

		#endregion Data Properties

	}
}


