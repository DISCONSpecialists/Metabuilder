﻿using System;
using System.ComponentModel;

namespace MetaBuilder.BusinessLogic
{
	/// <summary>
	///		The data structure representation of the 'AssociationType' table via interface.
	/// </summary>
	/// <remarks>
	/// 	This struct is generated by a tool and should never be modified.
	/// </remarks>
	public interface IAssociationType 
	{
		/// <summary>			
		/// pkid : 
		/// </summary>
		/// <remarks>Member of the primary key of the underlying table "AssociationType"</remarks>
		System.Int32 pkid { get; set; }
				
		
		
		/// <summary>
		/// Name : 
		/// </summary>
		System.String  Name  { get; set; }
		
		/// <summary>
		/// IsTwoWay : 
		/// </summary>
		System.Boolean?  IsTwoWay  { get; set; }
		
		/// <summary>
		/// LinkSpecification : 
		/// </summary>
		System.String  LinkSpecification  { get; set; }
			
		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		System.Object Clone();
		
		#region Data Properties


		/// <summary>
		///	Holds a collection of entity objects
		///	which are related to this object through the relation _classAssociationAssociationTypeID
		/// </summary>	
		TList<ClassAssociation> ClassAssociationCollection {  get;  set;}	

		#endregion Data Properties

	}
}


