﻿using System;
using System.ComponentModel;

namespace MetaBuilder.BusinessLogic
{
	/// <summary>
	///		The data structure representation of the 'ObjectFieldValue' table via interface.
	/// </summary>
	/// <remarks>
	/// 	This struct is generated by a tool and should never be modified.
	/// </remarks>
	public interface IObjectFieldValue 
	{
		/// <summary>			
		/// ObjectID : 
		/// </summary>
		/// <remarks>Member of the primary key of the underlying table "ObjectFieldValue"</remarks>
		System.Int32 ObjectID { get; set; }
				
		/// <summary>
		/// keep a copy of the original so it can be used for editable primary keys.
		/// </summary>
		System.Int32 OriginalObjectID { get; set; }
			
		/// <summary>			
		/// FieldID : 
		/// </summary>
		/// <remarks>Member of the primary key of the underlying table "ObjectFieldValue"</remarks>
		System.Int32 FieldID { get; set; }
				
		/// <summary>
		/// keep a copy of the original so it can be used for editable primary keys.
		/// </summary>
		System.Int32 OriginalFieldID { get; set; }
			
		/// <summary>			
		/// MachineID : 
		/// </summary>
		/// <remarks>Member of the primary key of the underlying table "ObjectFieldValue"</remarks>
		System.String MachineID { get; set; }
				
		/// <summary>
		/// keep a copy of the original so it can be used for editable primary keys.
		/// </summary>
		System.String OriginalMachineID { get; set; }
			
		
		
		/// <summary>
		/// ValueString : 
		/// </summary>
		System.String  ValueString  { get; set; }
		
		/// <summary>
		/// ValueInt : 
		/// </summary>
		System.Int32?  ValueInt  { get; set; }
		
		/// <summary>
		/// ValueDouble : 
		/// </summary>
		System.Decimal?  ValueDouble  { get; set; }
		
		/// <summary>
		/// ValueObjectID : 
		/// </summary>
		System.Int32?  ValueObjectID  { get; set; }
		
		/// <summary>
		/// ValueDate : 
		/// </summary>
		System.DateTime?  ValueDate  { get; set; }
		
		/// <summary>
		/// ValueBoolean : 
		/// </summary>
		System.Boolean?  ValueBoolean  { get; set; }
		
		/// <summary>
		/// ValueLongText : 
		/// </summary>
		System.String  ValueLongText  { get; set; }
		
		/// <summary>
		/// ValueRTF : 
		/// </summary>
		System.String  ValueRTF  { get; set; }
			
		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		System.Object Clone();
		
		#region Data Properties

		#endregion Data Properties

	}
}


