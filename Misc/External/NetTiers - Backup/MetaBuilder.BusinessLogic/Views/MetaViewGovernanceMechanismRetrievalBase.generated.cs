﻿/*
	File generated by NetTiers templates [www.nettiers.com]
	Important: Do not modify this file. Edit the file METAViewGovernanceMechanismRetrieval.cs instead.
*/
#region Using Directives
using System;
using System.ComponentModel;
using System.Collections;
using System.Runtime.Serialization;
using System.Xml.Serialization;
#endregion

namespace MetaBuilder.BusinessLogic
{
	///<summary>
	/// An object representation of the 'METAView_GovernanceMechanism_Retrieval' view. [No description found in the database]	
	///</summary>
	[Serializable]
	[CLSCompliant(true)]
	[ToolboxItem("METAViewGovernanceMechanismRetrievalBase")]
	public abstract partial class METAViewGovernanceMechanismRetrievalBase : System.IComparable, System.ICloneable, INotifyPropertyChanged
	{
		
		#region Variable Declarations
		
		/// <summary>
		/// WorkspaceName : 
		/// </summary>
		private System.String		  _workspaceName = string.Empty;
		
		/// <summary>
		/// WorkspaceTypeID : 
		/// </summary>
		private System.Int32?		  _workspaceTypeID = null;
		
		/// <summary>
		/// VCStatusID : 
		/// </summary>
		private System.Int32		  _vCStatusID = (int)0;
		
		/// <summary>
		/// pkid : 
		/// </summary>
		private System.Int32		  _pkid = (int)0;
		
		/// <summary>
		/// Machine : 
		/// </summary>
		private System.String		  _machine = string.Empty;
		
		/// <summary>
		/// VCMachineID : 
		/// </summary>
		private System.String		  _vCMachineID = null;
		
		/// <summary>
		/// EnvironmentInd : 
		/// </summary>
		private System.String		  _environmentInd = null;
		
		/// <summary>
		/// GovernanceMechType : 
		/// </summary>
		private System.String		  _governanceMechType = null;
		
		/// <summary>
		/// UniqueRef : 
		/// </summary>
		private System.String		  _uniqueRef = null;
		
		/// <summary>
		/// ValidityPeriod : 
		/// </summary>
		private System.String		  _validityPeriod = null;
		
		/// <summary>
		/// Description : 
		/// </summary>
		private System.String		  _description = null;
		
		/// <summary>
		/// GapType : 
		/// </summary>
		private System.String		  _gapType = null;
		
		/// <summary>
		/// CustomField1 : 
		/// </summary>
		private System.String		  _customField1 = null;
		
		/// <summary>
		/// CustomField2 : 
		/// </summary>
		private System.String		  _customField2 = null;
		
		/// <summary>
		/// CustomField3 : 
		/// </summary>
		private System.String		  _customField3 = null;
		
		/// <summary>
		/// Object that contains data to associate with this object
		/// </summary>
		private object _tag;
		
		/// <summary>
		/// Suppresses Entity Events from Firing, 
		/// useful when loading the entities from the database.
		/// </summary>
	    [NonSerialized] 
		private bool suppressEntityEvents = false;
		
		#endregion Variable Declarations
		
		#region Constructors
		///<summary>
		/// Creates a new <see cref="METAViewGovernanceMechanismRetrievalBase"/> instance.
		///</summary>
		public METAViewGovernanceMechanismRetrievalBase()
		{
		}		
		
		///<summary>
		/// Creates a new <see cref="METAViewGovernanceMechanismRetrievalBase"/> instance.
		///</summary>
		///<param name="_workspaceName"></param>
		///<param name="_workspaceTypeID"></param>
		///<param name="_vCStatusID"></param>
		///<param name="_pkid"></param>
		///<param name="_machine"></param>
		///<param name="_vCMachineID"></param>
		///<param name="_environmentInd"></param>
		///<param name="_governanceMechType"></param>
		///<param name="_uniqueRef"></param>
		///<param name="_validityPeriod"></param>
		///<param name="_description"></param>
		///<param name="_gapType"></param>
		///<param name="_customField1"></param>
		///<param name="_customField2"></param>
		///<param name="_customField3"></param>
		public METAViewGovernanceMechanismRetrievalBase(System.String _workspaceName, System.Int32? _workspaceTypeID, System.Int32 _vCStatusID, System.Int32 _pkid, System.String _machine, System.String _vCMachineID, System.String _environmentInd, System.String _governanceMechType, System.String _uniqueRef, System.String _validityPeriod, System.String _description, System.String _gapType, System.String _customField1, System.String _customField2, System.String _customField3)
		{
			this._workspaceName = _workspaceName;
			this._workspaceTypeID = _workspaceTypeID;
			this._vCStatusID = _vCStatusID;
			this._pkid = _pkid;
			this._machine = _machine;
			this._vCMachineID = _vCMachineID;
			this._environmentInd = _environmentInd;
			this._governanceMechType = _governanceMechType;
			this._uniqueRef = _uniqueRef;
			this._validityPeriod = _validityPeriod;
			this._description = _description;
			this._gapType = _gapType;
			this._customField1 = _customField1;
			this._customField2 = _customField2;
			this._customField3 = _customField3;
		}
		
		///<summary>
		/// A simple factory method to create a new <see cref="METAViewGovernanceMechanismRetrieval"/> instance.
		///</summary>
		///<param name="_workspaceName"></param>
		///<param name="_workspaceTypeID"></param>
		///<param name="_vCStatusID"></param>
		///<param name="_pkid"></param>
		///<param name="_machine"></param>
		///<param name="_vCMachineID"></param>
		///<param name="_environmentInd"></param>
		///<param name="_governanceMechType"></param>
		///<param name="_uniqueRef"></param>
		///<param name="_validityPeriod"></param>
		///<param name="_description"></param>
		///<param name="_gapType"></param>
		///<param name="_customField1"></param>
		///<param name="_customField2"></param>
		///<param name="_customField3"></param>
		public static METAViewGovernanceMechanismRetrieval CreateMETAViewGovernanceMechanismRetrieval(System.String _workspaceName, System.Int32? _workspaceTypeID, System.Int32 _vCStatusID, System.Int32 _pkid, System.String _machine, System.String _vCMachineID, System.String _environmentInd, System.String _governanceMechType, System.String _uniqueRef, System.String _validityPeriod, System.String _description, System.String _gapType, System.String _customField1, System.String _customField2, System.String _customField3)
		{
			METAViewGovernanceMechanismRetrieval newMETAViewGovernanceMechanismRetrieval = new METAViewGovernanceMechanismRetrieval();
			newMETAViewGovernanceMechanismRetrieval.WorkspaceName = _workspaceName;
			newMETAViewGovernanceMechanismRetrieval.WorkspaceTypeID = _workspaceTypeID;
			newMETAViewGovernanceMechanismRetrieval.VCStatusID = _vCStatusID;
			newMETAViewGovernanceMechanismRetrieval.Pkid = _pkid;
			newMETAViewGovernanceMechanismRetrieval.Machine = _machine;
			newMETAViewGovernanceMechanismRetrieval.VCMachineID = _vCMachineID;
			newMETAViewGovernanceMechanismRetrieval.EnvironmentInd = _environmentInd;
			newMETAViewGovernanceMechanismRetrieval.GovernanceMechType = _governanceMechType;
			newMETAViewGovernanceMechanismRetrieval.UniqueRef = _uniqueRef;
			newMETAViewGovernanceMechanismRetrieval.ValidityPeriod = _validityPeriod;
			newMETAViewGovernanceMechanismRetrieval.Description = _description;
			newMETAViewGovernanceMechanismRetrieval.GapType = _gapType;
			newMETAViewGovernanceMechanismRetrieval.CustomField1 = _customField1;
			newMETAViewGovernanceMechanismRetrieval.CustomField2 = _customField2;
			newMETAViewGovernanceMechanismRetrieval.CustomField3 = _customField3;
			return newMETAViewGovernanceMechanismRetrieval;
		}
				
		#endregion Constructors
		
		#region Properties	
		/// <summary>
		/// 	Gets or Sets the WorkspaceName property. 
		///		
		/// </summary>
		/// <value>This type is varchar</value>
		/// <remarks>
		/// This property can not be set to null. 
		/// </remarks>
		/// <exception cref="ArgumentNullException">If you attempt to set to null.</exception>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.String WorkspaceName
		{
			get
			{
				return this._workspaceName; 
			}
			set
			{
				if ( value == null )
					throw new ArgumentNullException("value", "WorkspaceName does not allow null values.");
				if (_workspaceName == value)
					return;
					
				this._workspaceName = value;
				this._isDirty = true;
				
				OnPropertyChanged("WorkspaceName");
			}
		}
		
		/// <summary>
		/// 	Gets or Sets the WorkspaceTypeID property. 
		///		
		/// </summary>
		/// <value>This type is int</value>
		/// <remarks>
		/// This property can be set to null. 
		/// If this column is null, this property will return (int)0. It is up to the developer
		/// to check the value of IsWorkspaceTypeIDNull() and perform business logic appropriately.
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.Int32? WorkspaceTypeID
		{
			get
			{
				return this._workspaceTypeID; 
			}
			set
			{
				if (_workspaceTypeID == value && WorkspaceTypeID != null )
					return;
					
				this._workspaceTypeID = value;
				this._isDirty = true;
				
				OnPropertyChanged("WorkspaceTypeID");
			}
		}
		
		/// <summary>
		/// 	Gets or Sets the VCStatusID property. 
		///		
		/// </summary>
		/// <value>This type is int</value>
		/// <remarks>
		/// This property can not be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.Int32 VCStatusID
		{
			get
			{
				return this._vCStatusID; 
			}
			set
			{
				if (_vCStatusID == value)
					return;
					
				this._vCStatusID = value;
				this._isDirty = true;
				
				OnPropertyChanged("VCStatusID");
			}
		}
		
		/// <summary>
		/// 	Gets or Sets the pkid property. 
		///		
		/// </summary>
		/// <value>This type is int</value>
		/// <remarks>
		/// This property can not be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.Int32 Pkid
		{
			get
			{
				return this._pkid; 
			}
			set
			{
				if (_pkid == value)
					return;
					
				this._pkid = value;
				this._isDirty = true;
				
				OnPropertyChanged("Pkid");
			}
		}
		
		/// <summary>
		/// 	Gets or Sets the Machine property. 
		///		
		/// </summary>
		/// <value>This type is varchar</value>
		/// <remarks>
		/// This property can not be set to null. 
		/// </remarks>
		/// <exception cref="ArgumentNullException">If you attempt to set to null.</exception>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.String Machine
		{
			get
			{
				return this._machine; 
			}
			set
			{
				if ( value == null )
					throw new ArgumentNullException("value", "Machine does not allow null values.");
				if (_machine == value)
					return;
					
				this._machine = value;
				this._isDirty = true;
				
				OnPropertyChanged("Machine");
			}
		}
		
		/// <summary>
		/// 	Gets or Sets the VCMachineID property. 
		///		
		/// </summary>
		/// <value>This type is varchar</value>
		/// <remarks>
		/// This property can be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.String VCMachineID
		{
			get
			{
				return this._vCMachineID; 
			}
			set
			{
				if (_vCMachineID == value)
					return;
					
				this._vCMachineID = value;
				this._isDirty = true;
				
				OnPropertyChanged("VCMachineID");
			}
		}
		
		/// <summary>
		/// 	Gets or Sets the EnvironmentInd property. 
		///		
		/// </summary>
		/// <value>This type is varchar</value>
		/// <remarks>
		/// This property can be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.String EnvironmentInd
		{
			get
			{
				return this._environmentInd; 
			}
			set
			{
				if (_environmentInd == value)
					return;
					
				this._environmentInd = value;
				this._isDirty = true;
				
				OnPropertyChanged("EnvironmentInd");
			}
		}
		
		/// <summary>
		/// 	Gets or Sets the GovernanceMechType property. 
		///		
		/// </summary>
		/// <value>This type is varchar</value>
		/// <remarks>
		/// This property can be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.String GovernanceMechType
		{
			get
			{
				return this._governanceMechType; 
			}
			set
			{
				if (_governanceMechType == value)
					return;
					
				this._governanceMechType = value;
				this._isDirty = true;
				
				OnPropertyChanged("GovernanceMechType");
			}
		}
		
		/// <summary>
		/// 	Gets or Sets the UniqueRef property. 
		///		
		/// </summary>
		/// <value>This type is varchar</value>
		/// <remarks>
		/// This property can be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.String UniqueRef
		{
			get
			{
				return this._uniqueRef; 
			}
			set
			{
				if (_uniqueRef == value)
					return;
					
				this._uniqueRef = value;
				this._isDirty = true;
				
				OnPropertyChanged("UniqueRef");
			}
		}
		
		/// <summary>
		/// 	Gets or Sets the ValidityPeriod property. 
		///		
		/// </summary>
		/// <value>This type is varchar</value>
		/// <remarks>
		/// This property can be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.String ValidityPeriod
		{
			get
			{
				return this._validityPeriod; 
			}
			set
			{
				if (_validityPeriod == value)
					return;
					
				this._validityPeriod = value;
				this._isDirty = true;
				
				OnPropertyChanged("ValidityPeriod");
			}
		}
		
		/// <summary>
		/// 	Gets or Sets the Description property. 
		///		
		/// </summary>
		/// <value>This type is varchar</value>
		/// <remarks>
		/// This property can be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.String Description
		{
			get
			{
				return this._description; 
			}
			set
			{
				if (_description == value)
					return;
					
				this._description = value;
				this._isDirty = true;
				
				OnPropertyChanged("Description");
			}
		}
		
		/// <summary>
		/// 	Gets or Sets the GapType property. 
		///		
		/// </summary>
		/// <value>This type is varchar</value>
		/// <remarks>
		/// This property can be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.String GapType
		{
			get
			{
				return this._gapType; 
			}
			set
			{
				if (_gapType == value)
					return;
					
				this._gapType = value;
				this._isDirty = true;
				
				OnPropertyChanged("GapType");
			}
		}
		
		/// <summary>
		/// 	Gets or Sets the CustomField1 property. 
		///		
		/// </summary>
		/// <value>This type is varchar</value>
		/// <remarks>
		/// This property can be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.String CustomField1
		{
			get
			{
				return this._customField1; 
			}
			set
			{
				if (_customField1 == value)
					return;
					
				this._customField1 = value;
				this._isDirty = true;
				
				OnPropertyChanged("CustomField1");
			}
		}
		
		/// <summary>
		/// 	Gets or Sets the CustomField2 property. 
		///		
		/// </summary>
		/// <value>This type is varchar</value>
		/// <remarks>
		/// This property can be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.String CustomField2
		{
			get
			{
				return this._customField2; 
			}
			set
			{
				if (_customField2 == value)
					return;
					
				this._customField2 = value;
				this._isDirty = true;
				
				OnPropertyChanged("CustomField2");
			}
		}
		
		/// <summary>
		/// 	Gets or Sets the CustomField3 property. 
		///		
		/// </summary>
		/// <value>This type is varchar</value>
		/// <remarks>
		/// This property can be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.String CustomField3
		{
			get
			{
				return this._customField3; 
			}
			set
			{
				if (_customField3 == value)
					return;
					
				this._customField3 = value;
				this._isDirty = true;
				
				OnPropertyChanged("CustomField3");
			}
		}
		
		
		/// <summary>
		///     Gets or sets the object that contains supplemental data about this object.
		/// </summary>
		/// <value>Object</value>
		[System.ComponentModel.Bindable(false)]
		[LocalizableAttribute(false)]
		[DescriptionAttribute("Object containing data to be associated with this object")]
		public virtual object Tag
		{
			get
			{
				return this._tag;
			}
			set
			{
				if (this._tag == value)
					return;
		
				this._tag = value;
			}
		}
	
		/// <summary>
		/// Determines whether this entity is to suppress events while set to true.
		/// </summary>
		[System.ComponentModel.Bindable(false)]
		[BrowsableAttribute(false), XmlIgnoreAttribute()]
		public bool SuppressEntityEvents
		{	
			get
			{
				return suppressEntityEvents;
			}
			set
			{
				suppressEntityEvents = value;
			}	
		}

		private bool _isDeleted = false;
		/// <summary>
		/// Gets a value indicating if object has been <see cref="MarkToDelete"/>. ReadOnly.
		/// </summary>
		[BrowsableAttribute(false), XmlIgnoreAttribute()]
		public virtual bool IsDeleted
		{
			get { return this._isDeleted; }
		}


		private bool _isDirty = false;
		/// <summary>
		///	Gets a value indicating  if the object has been modified from its original state.
		/// </summary>
		///<value>True if object has been modified from its original state; otherwise False;</value>
		[BrowsableAttribute(false), XmlIgnoreAttribute()]
		public virtual bool IsDirty
		{
			get { return this._isDirty; }
		}
		

		private bool _isNew = true;
		/// <summary>
		///	Gets a value indicating if the object is new.
		/// </summary>
		///<value>True if objectis new; otherwise False;</value>
		[BrowsableAttribute(false), XmlIgnoreAttribute()]
		public virtual bool IsNew
		{
			get { return this._isNew; }
			set { this._isNew = value; }
		}

		/// <summary>
		///		The name of the underlying database table.
		/// </summary>
		[BrowsableAttribute(false), XmlIgnoreAttribute()]
		public string ViewName
		{
			get { return "METAView_GovernanceMechanism_Retrieval"; }
		}

		
		#endregion
		
		#region Methods	
		
		/// <summary>
		/// Accepts the changes made to this object by setting each flags to false.
		/// </summary>
		public virtual void AcceptChanges()
		{
			this._isDeleted = false;
			this._isDirty = false;
			this._isNew = false;
			OnPropertyChanged(string.Empty);
		}
		
		
		///<summary>
		///  Revert all changes and restore original values.
		///  Currently not supported.
		///</summary>
		/// <exception cref="NotSupportedException">This method is not currently supported and always throws this exception.</exception>
		public virtual void CancelChanges()
		{
			throw new NotSupportedException("Method currently not Supported.");
		}
		
		///<summary>
		///   Marks entity to be deleted.
		///</summary>
		public virtual void MarkToDelete()
		{
			this._isDeleted = true;
		}
		
		#region ICloneable Members
		///<summary>
		///  Returns a Typed METAViewGovernanceMechanismRetrievalBase Entity 
		///</summary>
		public virtual METAViewGovernanceMechanismRetrievalBase Copy()
		{
			//shallow copy entity
			METAViewGovernanceMechanismRetrieval copy = new METAViewGovernanceMechanismRetrieval();
				copy.WorkspaceName = this.WorkspaceName;
				copy.WorkspaceTypeID = this.WorkspaceTypeID;
				copy.VCStatusID = this.VCStatusID;
				copy.Pkid = this.Pkid;
				copy.Machine = this.Machine;
				copy.VCMachineID = this.VCMachineID;
				copy.EnvironmentInd = this.EnvironmentInd;
				copy.GovernanceMechType = this.GovernanceMechType;
				copy.UniqueRef = this.UniqueRef;
				copy.ValidityPeriod = this.ValidityPeriod;
				copy.Description = this.Description;
				copy.GapType = this.GapType;
				copy.CustomField1 = this.CustomField1;
				copy.CustomField2 = this.CustomField2;
				copy.CustomField3 = this.CustomField3;
			copy.AcceptChanges();
			return (METAViewGovernanceMechanismRetrieval)copy;
		}
		
		///<summary>
		/// ICloneable.Clone() Member, returns the Deep Copy of this entity.
		///</summary>
		public object Clone(){
			return this.Copy();
		}
		
		///<summary>
		/// Returns a deep copy of the child collection object passed in.
		///</summary>
		public static object MakeCopyOf(object x)
		{
			if (x is ICloneable)
			{
				// Return a deep copy of the object
				return ((ICloneable)x).Clone();
			}
			else
				throw new System.NotSupportedException("Object Does Not Implement the ICloneable Interface.");
		}
		#endregion
		
		
		///<summary>
		/// Returns a value indicating whether this instance is equal to a specified object.
		///</summary>
		///<param name="toObject">An object to compare to this instance.</param>
		///<returns>true if toObject is a <see cref="METAViewGovernanceMechanismRetrievalBase"/> and has the same value as this instance; otherwise, false.</returns>
		public virtual bool Equals(METAViewGovernanceMechanismRetrievalBase toObject)
		{
			if (toObject == null)
				return false;
			return Equals(this, toObject);
		}
		
		
		///<summary>
		/// Determines whether the specified <see cref="METAViewGovernanceMechanismRetrievalBase"/> instances are considered equal.
		///</summary>
		///<param name="Object1">The first <see cref="METAViewGovernanceMechanismRetrievalBase"/> to compare.</param>
		///<param name="Object2">The second <see cref="METAViewGovernanceMechanismRetrievalBase"/> to compare. </param>
		///<returns>true if Object1 is the same instance as Object2 or if both are null references or if objA.Equals(objB) returns true; otherwise, false.</returns>
		public static bool Equals(METAViewGovernanceMechanismRetrievalBase Object1, METAViewGovernanceMechanismRetrievalBase Object2)
		{
			// both are null
			if (Object1 == null && Object2 == null)
				return true;

			// one or the other is null, but not both
			if (Object1 == null ^ Object2 == null)
				return false;

			bool equal = true;
			if (Object1.WorkspaceName != Object2.WorkspaceName)
				equal = false;
			if (Object1.WorkspaceTypeID != null && Object2.WorkspaceTypeID != null )
			{
				if (Object1.WorkspaceTypeID != Object2.WorkspaceTypeID)
					equal = false;
			}
			else if (Object1.WorkspaceTypeID == null ^ Object1.WorkspaceTypeID == null )
			{
				equal = false;
			}
			if (Object1.VCStatusID != Object2.VCStatusID)
				equal = false;
			if (Object1.Pkid != Object2.Pkid)
				equal = false;
			if (Object1.Machine != Object2.Machine)
				equal = false;
			if (Object1.VCMachineID != null && Object2.VCMachineID != null )
			{
				if (Object1.VCMachineID != Object2.VCMachineID)
					equal = false;
			}
			else if (Object1.VCMachineID == null ^ Object1.VCMachineID == null )
			{
				equal = false;
			}
			if (Object1.EnvironmentInd != null && Object2.EnvironmentInd != null )
			{
				if (Object1.EnvironmentInd != Object2.EnvironmentInd)
					equal = false;
			}
			else if (Object1.EnvironmentInd == null ^ Object1.EnvironmentInd == null )
			{
				equal = false;
			}
			if (Object1.GovernanceMechType != null && Object2.GovernanceMechType != null )
			{
				if (Object1.GovernanceMechType != Object2.GovernanceMechType)
					equal = false;
			}
			else if (Object1.GovernanceMechType == null ^ Object1.GovernanceMechType == null )
			{
				equal = false;
			}
			if (Object1.UniqueRef != null && Object2.UniqueRef != null )
			{
				if (Object1.UniqueRef != Object2.UniqueRef)
					equal = false;
			}
			else if (Object1.UniqueRef == null ^ Object1.UniqueRef == null )
			{
				equal = false;
			}
			if (Object1.ValidityPeriod != null && Object2.ValidityPeriod != null )
			{
				if (Object1.ValidityPeriod != Object2.ValidityPeriod)
					equal = false;
			}
			else if (Object1.ValidityPeriod == null ^ Object1.ValidityPeriod == null )
			{
				equal = false;
			}
			if (Object1.Description != null && Object2.Description != null )
			{
				if (Object1.Description != Object2.Description)
					equal = false;
			}
			else if (Object1.Description == null ^ Object1.Description == null )
			{
				equal = false;
			}
			if (Object1.GapType != null && Object2.GapType != null )
			{
				if (Object1.GapType != Object2.GapType)
					equal = false;
			}
			else if (Object1.GapType == null ^ Object1.GapType == null )
			{
				equal = false;
			}
			if (Object1.CustomField1 != null && Object2.CustomField1 != null )
			{
				if (Object1.CustomField1 != Object2.CustomField1)
					equal = false;
			}
			else if (Object1.CustomField1 == null ^ Object1.CustomField1 == null )
			{
				equal = false;
			}
			if (Object1.CustomField2 != null && Object2.CustomField2 != null )
			{
				if (Object1.CustomField2 != Object2.CustomField2)
					equal = false;
			}
			else if (Object1.CustomField2 == null ^ Object1.CustomField2 == null )
			{
				equal = false;
			}
			if (Object1.CustomField3 != null && Object2.CustomField3 != null )
			{
				if (Object1.CustomField3 != Object2.CustomField3)
					equal = false;
			}
			else if (Object1.CustomField3 == null ^ Object1.CustomField3 == null )
			{
				equal = false;
			}
			return equal;
		}
		
		#endregion
		
		#region IComparable Members
		///<summary>
		/// Compares this instance to a specified object and returns an indication of their relative values.
		///<param name="obj">An object to compare to this instance, or a null reference (Nothing in Visual Basic).</param>
		///</summary>
		///<returns>A signed integer that indicates the relative order of this instance and obj.</returns>
		public virtual int CompareTo(object obj)
		{
			throw new NotImplementedException();
		}
	
		#endregion
		
		#region INotifyPropertyChanged Members
		
		/// <summary>
      /// Event to indicate that a property has changed.
      /// </summary>
		[field:NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
      /// Called when a property is changed
      /// </summary>
      /// <param name="propertyName">The name of the property that has changed.</param>
		protected virtual void OnPropertyChanged(string propertyName)
		{ 
			OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}
		
		/// <summary>
      /// Called when a property is changed
      /// </summary>
      /// <param name="e">PropertyChangedEventArgs</param>
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (!SuppressEntityEvents)
			{
				if (null != PropertyChanged)
				{
					PropertyChanged(this, e);
				}
			}
		}
		
		#endregion
				
		/// <summary>
		/// Gets the property value by name.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns></returns>
		public static object GetPropertyValueByName(METAViewGovernanceMechanismRetrieval entity, string propertyName)
		{
			switch (propertyName)
			{
				case "WorkspaceName":
					return entity.WorkspaceName;
				case "WorkspaceTypeID":
					return entity.WorkspaceTypeID;
				case "VCStatusID":
					return entity.VCStatusID;
				case "Pkid":
					return entity.Pkid;
				case "Machine":
					return entity.Machine;
				case "VCMachineID":
					return entity.VCMachineID;
				case "EnvironmentInd":
					return entity.EnvironmentInd;
				case "GovernanceMechType":
					return entity.GovernanceMechType;
				case "UniqueRef":
					return entity.UniqueRef;
				case "ValidityPeriod":
					return entity.ValidityPeriod;
				case "Description":
					return entity.Description;
				case "GapType":
					return entity.GapType;
				case "CustomField1":
					return entity.CustomField1;
				case "CustomField2":
					return entity.CustomField2;
				case "CustomField3":
					return entity.CustomField3;
			}
			return null;
		}
				
		/// <summary>
		/// Gets the property value by name.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns></returns>
		public object GetPropertyValueByName(string propertyName)
		{			
			return GetPropertyValueByName(this as METAViewGovernanceMechanismRetrieval, propertyName);
		}
		
		///<summary>
		/// Returns a String that represents the current object.
		///</summary>
		public override string ToString()
		{
			return string.Format(System.Globalization.CultureInfo.InvariantCulture,
				"{16}{15}- WorkspaceName: {0}{15}- WorkspaceTypeID: {1}{15}- VCStatusID: {2}{15}- Pkid: {3}{15}- Machine: {4}{15}- VCMachineID: {5}{15}- EnvironmentInd: {6}{15}- GovernanceMechType: {7}{15}- UniqueRef: {8}{15}- ValidityPeriod: {9}{15}- Description: {10}{15}- GapType: {11}{15}- CustomField1: {12}{15}- CustomField2: {13}{15}- CustomField3: {14}{15}", 
				this.WorkspaceName,
				(this.WorkspaceTypeID == null) ? string.Empty : this.WorkspaceTypeID.ToString(),
			     
				this.VCStatusID,
				this.Pkid,
				this.Machine,
				(this.VCMachineID == null) ? string.Empty : this.VCMachineID.ToString(),
			     
				(this.EnvironmentInd == null) ? string.Empty : this.EnvironmentInd.ToString(),
			     
				(this.GovernanceMechType == null) ? string.Empty : this.GovernanceMechType.ToString(),
			     
				(this.UniqueRef == null) ? string.Empty : this.UniqueRef.ToString(),
			     
				(this.ValidityPeriod == null) ? string.Empty : this.ValidityPeriod.ToString(),
			     
				(this.Description == null) ? string.Empty : this.Description.ToString(),
			     
				(this.GapType == null) ? string.Empty : this.GapType.ToString(),
			     
				(this.CustomField1 == null) ? string.Empty : this.CustomField1.ToString(),
			     
				(this.CustomField2 == null) ? string.Empty : this.CustomField2.ToString(),
			     
				(this.CustomField3 == null) ? string.Empty : this.CustomField3.ToString(),
			     
				System.Environment.NewLine, 
				this.GetType());
		}
	
	}//End Class
	
	
	/// <summary>
	/// Enumerate the METAViewGovernanceMechanismRetrieval columns.
	/// </summary>
	[Serializable]
	public enum METAViewGovernanceMechanismRetrievalColumn
	{
		/// <summary>
		/// WorkspaceName : 
		/// </summary>
		[EnumTextValue("WorkspaceName")]
		[ColumnEnum("WorkspaceName", typeof(System.String), System.Data.DbType.AnsiString, false, false, false, 100)]
		WorkspaceName,
		/// <summary>
		/// WorkspaceTypeID : 
		/// </summary>
		[EnumTextValue("WorkspaceTypeID")]
		[ColumnEnum("WorkspaceTypeID", typeof(System.Int32), System.Data.DbType.Int32, false, false, true)]
		WorkspaceTypeID,
		/// <summary>
		/// VCStatusID : 
		/// </summary>
		[EnumTextValue("VCStatusID")]
		[ColumnEnum("VCStatusID", typeof(System.Int32), System.Data.DbType.Int32, false, false, false)]
		VCStatusID,
		/// <summary>
		/// pkid : 
		/// </summary>
		[EnumTextValue("pkid")]
		[ColumnEnum("pkid", typeof(System.Int32), System.Data.DbType.Int32, false, false, false)]
		Pkid,
		/// <summary>
		/// Machine : 
		/// </summary>
		[EnumTextValue("Machine")]
		[ColumnEnum("Machine", typeof(System.String), System.Data.DbType.AnsiString, false, false, false, 50)]
		Machine,
		/// <summary>
		/// VCMachineID : 
		/// </summary>
		[EnumTextValue("VCMachineID")]
		[ColumnEnum("VCMachineID", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 50)]
		VCMachineID,
		/// <summary>
		/// EnvironmentInd : 
		/// </summary>
		[EnumTextValue("EnvironmentInd")]
		[ColumnEnum("EnvironmentInd", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 255)]
		EnvironmentInd,
		/// <summary>
		/// GovernanceMechType : 
		/// </summary>
		[EnumTextValue("GovernanceMechType")]
		[ColumnEnum("GovernanceMechType", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 255)]
		GovernanceMechType,
		/// <summary>
		/// UniqueRef : 
		/// </summary>
		[EnumTextValue("UniqueRef")]
		[ColumnEnum("UniqueRef", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 255)]
		UniqueRef,
		/// <summary>
		/// ValidityPeriod : 
		/// </summary>
		[EnumTextValue("ValidityPeriod")]
		[ColumnEnum("ValidityPeriod", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 255)]
		ValidityPeriod,
		/// <summary>
		/// Description : 
		/// </summary>
		[EnumTextValue("Description")]
		[ColumnEnum("Description", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 255)]
		Description,
		/// <summary>
		/// GapType : 
		/// </summary>
		[EnumTextValue("GapType")]
		[ColumnEnum("GapType", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 255)]
		GapType,
		/// <summary>
		/// CustomField1 : 
		/// </summary>
		[EnumTextValue("CustomField1")]
		[ColumnEnum("CustomField1", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 255)]
		CustomField1,
		/// <summary>
		/// CustomField2 : 
		/// </summary>
		[EnumTextValue("CustomField2")]
		[ColumnEnum("CustomField2", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 255)]
		CustomField2,
		/// <summary>
		/// CustomField3 : 
		/// </summary>
		[EnumTextValue("CustomField3")]
		[ColumnEnum("CustomField3", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 255)]
		CustomField3
	}//End enum

} // end namespace
