﻿using System;
using System.ComponentModel;

namespace MetaBuilder.BusinessLogic
{
	/// <summary>
	///		The data structure representation of the 'Field' table via interface.
	/// </summary>
	/// <remarks>
	/// 	This struct is generated by a tool and should never be modified.
	/// </remarks>
	public interface IField 
	{
		/// <summary>			
		/// pkid : 
		/// </summary>
		/// <remarks>Member of the primary key of the underlying table "Field"</remarks>
		System.Int32 pkid { get; set; }
				
		
		
		/// <summary>
		/// Class : 
		/// </summary>
		System.String  Class  { get; set; }
		
		/// <summary>
		/// Name : 
		/// </summary>
		System.String  Name  { get; set; }
		
		/// <summary>
		/// DataType : 
		/// </summary>
		System.String  DataType  { get; set; }
		
		/// <summary>
		/// Category : 
		/// </summary>
		System.String  Category  { get; set; }
		
		/// <summary>
		/// Description : 
		/// </summary>
		System.String  Description  { get; set; }
		
		/// <summary>
		/// IsUnique : 
		/// </summary>
		System.Boolean?  IsUnique  { get; set; }
		
		/// <summary>
		/// SortOrder : 
		/// </summary>
		System.Int32?  SortOrder  { get; set; }
		
		/// <summary>
		/// IsActive : 
		/// </summary>
		System.Boolean?  IsActive  { get; set; }
			
		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		System.Object Clone();
		
		#region Data Properties

		
		/// <summary>
		///	Holds a collection of entity objects
		///	which are related to this object through the junction table objectIDMachineIDMetaObjectCollection_From_ObjectFieldValue
		/// </summary>	
		TList<MetaObject> ObjectIDMachineIDMetaObjectCollection_From_ObjectFieldValue { get; set; }	


		/// <summary>
		///	Holds a collection of entity objects
		///	which are related to this object through the relation _objectFieldValueFieldID
		/// </summary>	
		TList<ObjectFieldValue> ObjectFieldValueCollection {  get;  set;}	

		#endregion Data Properties

	}
}


