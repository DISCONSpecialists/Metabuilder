﻿using System;
using System.ComponentModel;

namespace MetaBuilder.BusinessLogic
{
	/// <summary>
	///		The data structure representation of the 'ClassAssociation' table via interface.
	/// </summary>
	/// <remarks>
	/// 	This struct is generated by a tool and should never be modified.
	/// </remarks>
	public interface IClassAssociation 
	{
		/// <summary>			
		/// CAid : 
		/// </summary>
		/// <remarks>Member of the primary key of the underlying table "ClassAssociation"</remarks>
		System.Int32 CAid { get; set; }
				
		
		
		/// <summary>
		/// ParentClass : 
		/// </summary>
		System.String  ParentClass  { get; set; }
		
		/// <summary>
		/// ChildClass : 
		/// </summary>
		System.String  ChildClass  { get; set; }
		
		/// <summary>
		/// AssociationTypeID : 
		/// </summary>
		System.Int32  AssociationTypeID  { get; set; }
		
		/// <summary>
		/// Caption : 
		/// </summary>
		System.String  Caption  { get; set; }
		
		/// <summary>
		/// AssociationObjectClass : 
		/// </summary>
		System.String  AssociationObjectClass  { get; set; }
		
		/// <summary>
		/// CopyIncluded : 
		/// </summary>
		System.Boolean  CopyIncluded  { get; set; }
		
		/// <summary>
		/// IsDefault : 
		/// </summary>
		System.Boolean  IsDefault  { get; set; }
		
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
		///	which are related to this object through the relation _objectAssociationCAid
		/// </summary>	
		TList<ObjectAssociation> ObjectAssociationCollection {  get;  set;}	


		/// <summary>
		///	Holds a collection of entity objects
		///	which are related to this object through the relation _allowedArtifactCAid
		/// </summary>	
		TList<AllowedArtifact> AllowedArtifactCollection {  get;  set;}	

		
		/// <summary>
		///	Holds a collection of entity objects
		///	which are related to this object through the junction table classClassCollection_From_AllowedArtifact
		/// </summary>	
		TList<Class> ClassClassCollection_From_AllowedArtifact { get; set; }	

		
		/// <summary>
		///	Holds a collection of entity objects
		///	which are related to this object through the junction table objectIDObjectMachineMetaObjectCollection_From_ObjectAssociation
		/// </summary>	
		TList<MetaObject> ObjectIDObjectMachineMetaObjectCollection_From_ObjectAssociation { get; set; }	

		
		/// <summary>
		///	Holds a collection of entity objects
		///	which are related to this object through the junction table childObjectIDChildObjectMachineMetaObjectCollection_From_ObjectAssociation
		/// </summary>	
		TList<MetaObject> ChildObjectIDChildObjectMachineMetaObjectCollection_From_ObjectAssociation { get; set; }	

		#endregion Data Properties

	}
}


