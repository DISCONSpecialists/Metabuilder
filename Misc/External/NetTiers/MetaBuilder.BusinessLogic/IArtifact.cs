﻿using System;
using System.ComponentModel;

namespace MetaBuilder.BusinessLogic
{
	/// <summary>
	///		The data structure representation of the 'Artifact' table via interface.
	/// </summary>
	/// <remarks>
	/// 	This struct is generated by a tool and should never be modified.
	/// </remarks>
	public interface IArtifact 
	{
		/// <summary>			
		/// ArtifactID : 
		/// </summary>
		/// <remarks>Member of the primary key of the underlying table "Artifact"</remarks>
		System.Int32 ArtifactID { get; set; }
				
		
		
		/// <summary>
		/// CAid : 
		/// </summary>
		System.Int32  CAid  { get; set; }
		
		/// <summary>
		/// ObjectID : 
		/// </summary>
		System.Int32  ObjectID  { get; set; }
		
		/// <summary>
		/// ChildObjectID : 
		/// </summary>
		System.Int32  ChildObjectID  { get; set; }
		
		/// <summary>
		/// ArtifactObjectID : 
		/// </summary>
		System.Int32  ArtifactObjectID  { get; set; }
		
		/// <summary>
		/// ObjectMachine : 
		/// </summary>
		System.String  ObjectMachine  { get; set; }
		
		/// <summary>
		/// ChildObjectMachine : 
		/// </summary>
		System.String  ChildObjectMachine  { get; set; }
		
		/// <summary>
		/// ArtefactMachine : 
		/// </summary>
		System.String  ArtefactMachine  { get; set; }
			
		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		System.Object Clone();
		
		#region Data Properties

		#endregion Data Properties

	}
}


