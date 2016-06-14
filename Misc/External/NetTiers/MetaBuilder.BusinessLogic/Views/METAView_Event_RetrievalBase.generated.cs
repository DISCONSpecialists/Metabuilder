﻿/*
	File generated by NetTiers templates [www.nettiers.com]
	Important: Do not modify this file. Edit the file METAView_Event_Retrieval.cs instead.
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
	/// An object representation of the 'METAView_Event_Retrieval' view. [No description found in the database]	
	///</summary>
	[Serializable]
	[CLSCompliant(true)]
	[ToolboxItem("METAView_Event_RetrievalBase")]
	public abstract partial class METAView_Event_RetrievalBase : System.IComparable, System.ICloneable, INotifyPropertyChanged
	{
		
		#region Variable Declarations
		
		/// <summary>
		/// WorkspaceName : 
		/// </summary>
		private System.String		  _workspaceName = string.Empty;
		
		/// <summary>
		/// WorkspaceTypeId : 
		/// </summary>
		private System.Int32?		  _workspaceTypeId = null;
		
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
		/// Name : 
		/// </summary>
		private System.String		  _name = null;
		
		/// <summary>
		/// Abbreviation : 
		/// </summary>
		private System.String		  _abbreviation = null;
		
		/// <summary>
		/// Description : 
		/// </summary>
		private System.String		  _description = null;
		
		/// <summary>
		/// IsBusinessExternal : 
		/// </summary>
		private System.String		  _isBusinessExternal = null;
		
		/// <summary>
		/// ArchitectureStatus : 
		/// </summary>
		private System.String		  _architectureStatus = null;
		
		/// <summary>
		/// ArchitectureStatusDate : 
		/// </summary>
		private System.String		  _architectureStatusDate = null;
		
		/// <summary>
		/// DesignRationale : 
		/// </summary>
		private System.String		  _designRationale = null;
		
		/// <summary>
		/// GeneralRemarks : 
		/// </summary>
		private System.String		  _generalRemarks = null;
		
		/// <summary>
		/// GapType : 
		/// </summary>
		private System.String		  _gapType = null;
		
		/// <summary>
		/// DataSourceID : 
		/// </summary>
		private System.String		  _dataSourceID = null;
		
		/// <summary>
		/// DataSourceName : 
		/// </summary>
		private System.String		  _dataSourceName = null;
		
		/// <summary>
		/// EventType : 
		/// </summary>
		private System.String		  _eventType = null;
		
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
		/// Creates a new <see cref="METAView_Event_RetrievalBase"/> instance.
		///</summary>
		public METAView_Event_RetrievalBase()
		{
		}		
		
		///<summary>
		/// Creates a new <see cref="METAView_Event_RetrievalBase"/> instance.
		///</summary>
		///<param name="_workspaceName"></param>
		///<param name="_workspaceTypeId"></param>
		///<param name="_vCStatusID"></param>
		///<param name="_pkid"></param>
		///<param name="_machine"></param>
		///<param name="_vCMachineID"></param>
		///<param name="_name"></param>
		///<param name="_abbreviation"></param>
		///<param name="_description"></param>
		///<param name="_isBusinessExternal"></param>
		///<param name="_architectureStatus"></param>
		///<param name="_architectureStatusDate"></param>
		///<param name="_designRationale"></param>
		///<param name="_generalRemarks"></param>
		///<param name="_gapType"></param>
		///<param name="_dataSourceID"></param>
		///<param name="_dataSourceName"></param>
		///<param name="_eventType"></param>
		public METAView_Event_RetrievalBase(System.String _workspaceName, System.Int32? _workspaceTypeId, System.Int32 _vCStatusID, System.Int32 _pkid, System.String _machine, System.String _vCMachineID, System.String _name, System.String _abbreviation, System.String _description, System.String _isBusinessExternal, System.String _architectureStatus, System.String _architectureStatusDate, System.String _designRationale, System.String _generalRemarks, System.String _gapType, System.String _dataSourceID, System.String _dataSourceName, System.String _eventType)
		{
			this._workspaceName = _workspaceName;
			this._workspaceTypeId = _workspaceTypeId;
			this._vCStatusID = _vCStatusID;
			this._pkid = _pkid;
			this._machine = _machine;
			this._vCMachineID = _vCMachineID;
			this._name = _name;
			this._abbreviation = _abbreviation;
			this._description = _description;
			this._isBusinessExternal = _isBusinessExternal;
			this._architectureStatus = _architectureStatus;
			this._architectureStatusDate = _architectureStatusDate;
			this._designRationale = _designRationale;
			this._generalRemarks = _generalRemarks;
			this._gapType = _gapType;
			this._dataSourceID = _dataSourceID;
			this._dataSourceName = _dataSourceName;
			this._eventType = _eventType;
		}
		
		///<summary>
		/// A simple factory method to create a new <see cref="METAView_Event_Retrieval"/> instance.
		///</summary>
		///<param name="_workspaceName"></param>
		///<param name="_workspaceTypeId"></param>
		///<param name="_vCStatusID"></param>
		///<param name="_pkid"></param>
		///<param name="_machine"></param>
		///<param name="_vCMachineID"></param>
		///<param name="_name"></param>
		///<param name="_abbreviation"></param>
		///<param name="_description"></param>
		///<param name="_isBusinessExternal"></param>
		///<param name="_architectureStatus"></param>
		///<param name="_architectureStatusDate"></param>
		///<param name="_designRationale"></param>
		///<param name="_generalRemarks"></param>
		///<param name="_gapType"></param>
		///<param name="_dataSourceID"></param>
		///<param name="_dataSourceName"></param>
		///<param name="_eventType"></param>
		public static METAView_Event_Retrieval CreateMETAView_Event_Retrieval(System.String _workspaceName, System.Int32? _workspaceTypeId, System.Int32 _vCStatusID, System.Int32 _pkid, System.String _machine, System.String _vCMachineID, System.String _name, System.String _abbreviation, System.String _description, System.String _isBusinessExternal, System.String _architectureStatus, System.String _architectureStatusDate, System.String _designRationale, System.String _generalRemarks, System.String _gapType, System.String _dataSourceID, System.String _dataSourceName, System.String _eventType)
		{
			METAView_Event_Retrieval newMETAView_Event_Retrieval = new METAView_Event_Retrieval();
			newMETAView_Event_Retrieval.WorkspaceName = _workspaceName;
			newMETAView_Event_Retrieval.WorkspaceTypeId = _workspaceTypeId;
			newMETAView_Event_Retrieval.VCStatusID = _vCStatusID;
			newMETAView_Event_Retrieval.pkid = _pkid;
			newMETAView_Event_Retrieval.Machine = _machine;
			newMETAView_Event_Retrieval.VCMachineID = _vCMachineID;
			newMETAView_Event_Retrieval.Name = _name;
			newMETAView_Event_Retrieval.Abbreviation = _abbreviation;
			newMETAView_Event_Retrieval.Description = _description;
			newMETAView_Event_Retrieval.IsBusinessExternal = _isBusinessExternal;
			newMETAView_Event_Retrieval.ArchitectureStatus = _architectureStatus;
			newMETAView_Event_Retrieval.ArchitectureStatusDate = _architectureStatusDate;
			newMETAView_Event_Retrieval.DesignRationale = _designRationale;
			newMETAView_Event_Retrieval.GeneralRemarks = _generalRemarks;
			newMETAView_Event_Retrieval.GapType = _gapType;
			newMETAView_Event_Retrieval.DataSourceID = _dataSourceID;
			newMETAView_Event_Retrieval.DataSourceName = _dataSourceName;
			newMETAView_Event_Retrieval.EventType = _eventType;
			return newMETAView_Event_Retrieval;
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
		/// 	Gets or Sets the WorkspaceTypeId property. 
		///		
		/// </summary>
		/// <value>This type is int</value>
		/// <remarks>
		/// This property can be set to null. 
		/// If this column is null, this property will return (int)0. It is up to the developer
		/// to check the value of IsWorkspaceTypeIdNull() and perform business logic appropriately.
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.Int32? WorkspaceTypeId
		{
			get
			{
				return this._workspaceTypeId; 
			}
			set
			{
				if (_workspaceTypeId == value && WorkspaceTypeId != null )
					return;
					
				this._workspaceTypeId = value;
				this._isDirty = true;
				
				OnPropertyChanged("WorkspaceTypeId");
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
		public virtual System.Int32 pkid
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
				
				OnPropertyChanged("pkid");
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
		/// 	Gets or Sets the Name property. 
		///		
		/// </summary>
		/// <value>This type is varchar</value>
		/// <remarks>
		/// This property can be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.String Name
		{
			get
			{
				return this._name; 
			}
			set
			{
				if (_name == value)
					return;
					
				this._name = value;
				this._isDirty = true;
				
				OnPropertyChanged("Name");
			}
		}
		
		/// <summary>
		/// 	Gets or Sets the Abbreviation property. 
		///		
		/// </summary>
		/// <value>This type is varchar</value>
		/// <remarks>
		/// This property can be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.String Abbreviation
		{
			get
			{
				return this._abbreviation; 
			}
			set
			{
				if (_abbreviation == value)
					return;
					
				this._abbreviation = value;
				this._isDirty = true;
				
				OnPropertyChanged("Abbreviation");
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
		/// 	Gets or Sets the IsBusinessExternal property. 
		///		
		/// </summary>
		/// <value>This type is varchar</value>
		/// <remarks>
		/// This property can be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.String IsBusinessExternal
		{
			get
			{
				return this._isBusinessExternal; 
			}
			set
			{
				if (_isBusinessExternal == value)
					return;
					
				this._isBusinessExternal = value;
				this._isDirty = true;
				
				OnPropertyChanged("IsBusinessExternal");
			}
		}
		
		/// <summary>
		/// 	Gets or Sets the ArchitectureStatus property. 
		///		
		/// </summary>
		/// <value>This type is varchar</value>
		/// <remarks>
		/// This property can be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.String ArchitectureStatus
		{
			get
			{
				return this._architectureStatus; 
			}
			set
			{
				if (_architectureStatus == value)
					return;
					
				this._architectureStatus = value;
				this._isDirty = true;
				
				OnPropertyChanged("ArchitectureStatus");
			}
		}
		
		/// <summary>
		/// 	Gets or Sets the ArchitectureStatusDate property. 
		///		
		/// </summary>
		/// <value>This type is varchar</value>
		/// <remarks>
		/// This property can be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.String ArchitectureStatusDate
		{
			get
			{
				return this._architectureStatusDate; 
			}
			set
			{
				if (_architectureStatusDate == value)
					return;
					
				this._architectureStatusDate = value;
				this._isDirty = true;
				
				OnPropertyChanged("ArchitectureStatusDate");
			}
		}
		
		/// <summary>
		/// 	Gets or Sets the DesignRationale property. 
		///		
		/// </summary>
		/// <value>This type is varchar</value>
		/// <remarks>
		/// This property can be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.String DesignRationale
		{
			get
			{
				return this._designRationale; 
			}
			set
			{
				if (_designRationale == value)
					return;
					
				this._designRationale = value;
				this._isDirty = true;
				
				OnPropertyChanged("DesignRationale");
			}
		}
		
		/// <summary>
		/// 	Gets or Sets the GeneralRemarks property. 
		///		
		/// </summary>
		/// <value>This type is varchar</value>
		/// <remarks>
		/// This property can be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.String GeneralRemarks
		{
			get
			{
				return this._generalRemarks; 
			}
			set
			{
				if (_generalRemarks == value)
					return;
					
				this._generalRemarks = value;
				this._isDirty = true;
				
				OnPropertyChanged("GeneralRemarks");
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
		/// 	Gets or Sets the DataSourceID property. 
		///		
		/// </summary>
		/// <value>This type is varchar</value>
		/// <remarks>
		/// This property can be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.String DataSourceID
		{
			get
			{
				return this._dataSourceID; 
			}
			set
			{
				if (_dataSourceID == value)
					return;
					
				this._dataSourceID = value;
				this._isDirty = true;
				
				OnPropertyChanged("DataSourceID");
			}
		}
		
		/// <summary>
		/// 	Gets or Sets the DataSourceName property. 
		///		
		/// </summary>
		/// <value>This type is varchar</value>
		/// <remarks>
		/// This property can be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.String DataSourceName
		{
			get
			{
				return this._dataSourceName; 
			}
			set
			{
				if (_dataSourceName == value)
					return;
					
				this._dataSourceName = value;
				this._isDirty = true;
				
				OnPropertyChanged("DataSourceName");
			}
		}
		
		/// <summary>
		/// 	Gets or Sets the EventType property. 
		///		
		/// </summary>
		/// <value>This type is varchar</value>
		/// <remarks>
		/// This property can be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.String EventType
		{
			get
			{
				return this._eventType; 
			}
			set
			{
				if (_eventType == value)
					return;
					
				this._eventType = value;
				this._isDirty = true;
				
				OnPropertyChanged("EventType");
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
			get { return "METAView_Event_Retrieval"; }
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
		///  Returns a Typed METAView_Event_RetrievalBase Entity 
		///</summary>
		public virtual METAView_Event_RetrievalBase Copy()
		{
			//shallow copy entity
			METAView_Event_Retrieval copy = new METAView_Event_Retrieval();
				copy.WorkspaceName = this.WorkspaceName;
				copy.WorkspaceTypeId = this.WorkspaceTypeId;
				copy.VCStatusID = this.VCStatusID;
				copy.pkid = this.pkid;
				copy.Machine = this.Machine;
				copy.VCMachineID = this.VCMachineID;
				copy.Name = this.Name;
				copy.Abbreviation = this.Abbreviation;
				copy.Description = this.Description;
				copy.IsBusinessExternal = this.IsBusinessExternal;
				copy.ArchitectureStatus = this.ArchitectureStatus;
				copy.ArchitectureStatusDate = this.ArchitectureStatusDate;
				copy.DesignRationale = this.DesignRationale;
				copy.GeneralRemarks = this.GeneralRemarks;
				copy.GapType = this.GapType;
				copy.DataSourceID = this.DataSourceID;
				copy.DataSourceName = this.DataSourceName;
				copy.EventType = this.EventType;
			copy.AcceptChanges();
			return (METAView_Event_Retrieval)copy;
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
		///<returns>true if toObject is a <see cref="METAView_Event_RetrievalBase"/> and has the same value as this instance; otherwise, false.</returns>
		public virtual bool Equals(METAView_Event_RetrievalBase toObject)
		{
			if (toObject == null)
				return false;
			return Equals(this, toObject);
		}
		
		
		///<summary>
		/// Determines whether the specified <see cref="METAView_Event_RetrievalBase"/> instances are considered equal.
		///</summary>
		///<param name="Object1">The first <see cref="METAView_Event_RetrievalBase"/> to compare.</param>
		///<param name="Object2">The second <see cref="METAView_Event_RetrievalBase"/> to compare. </param>
		///<returns>true if Object1 is the same instance as Object2 or if both are null references or if objA.Equals(objB) returns true; otherwise, false.</returns>
		public static bool Equals(METAView_Event_RetrievalBase Object1, METAView_Event_RetrievalBase Object2)
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
			if (Object1.WorkspaceTypeId != null && Object2.WorkspaceTypeId != null )
			{
				if (Object1.WorkspaceTypeId != Object2.WorkspaceTypeId)
					equal = false;
			}
			else if (Object1.WorkspaceTypeId == null ^ Object1.WorkspaceTypeId == null )
			{
				equal = false;
			}
			if (Object1.VCStatusID != Object2.VCStatusID)
				equal = false;
			if (Object1.pkid != Object2.pkid)
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
			if (Object1.Name != null && Object2.Name != null )
			{
				if (Object1.Name != Object2.Name)
					equal = false;
			}
			else if (Object1.Name == null ^ Object1.Name == null )
			{
				equal = false;
			}
			if (Object1.Abbreviation != null && Object2.Abbreviation != null )
			{
				if (Object1.Abbreviation != Object2.Abbreviation)
					equal = false;
			}
			else if (Object1.Abbreviation == null ^ Object1.Abbreviation == null )
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
			if (Object1.IsBusinessExternal != null && Object2.IsBusinessExternal != null )
			{
				if (Object1.IsBusinessExternal != Object2.IsBusinessExternal)
					equal = false;
			}
			else if (Object1.IsBusinessExternal == null ^ Object1.IsBusinessExternal == null )
			{
				equal = false;
			}
			if (Object1.ArchitectureStatus != null && Object2.ArchitectureStatus != null )
			{
				if (Object1.ArchitectureStatus != Object2.ArchitectureStatus)
					equal = false;
			}
			else if (Object1.ArchitectureStatus == null ^ Object1.ArchitectureStatus == null )
			{
				equal = false;
			}
			if (Object1.ArchitectureStatusDate != null && Object2.ArchitectureStatusDate != null )
			{
				if (Object1.ArchitectureStatusDate != Object2.ArchitectureStatusDate)
					equal = false;
			}
			else if (Object1.ArchitectureStatusDate == null ^ Object1.ArchitectureStatusDate == null )
			{
				equal = false;
			}
			if (Object1.DesignRationale != null && Object2.DesignRationale != null )
			{
				if (Object1.DesignRationale != Object2.DesignRationale)
					equal = false;
			}
			else if (Object1.DesignRationale == null ^ Object1.DesignRationale == null )
			{
				equal = false;
			}
			if (Object1.GeneralRemarks != null && Object2.GeneralRemarks != null )
			{
				if (Object1.GeneralRemarks != Object2.GeneralRemarks)
					equal = false;
			}
			else if (Object1.GeneralRemarks == null ^ Object1.GeneralRemarks == null )
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
			if (Object1.DataSourceID != null && Object2.DataSourceID != null )
			{
				if (Object1.DataSourceID != Object2.DataSourceID)
					equal = false;
			}
			else if (Object1.DataSourceID == null ^ Object1.DataSourceID == null )
			{
				equal = false;
			}
			if (Object1.DataSourceName != null && Object2.DataSourceName != null )
			{
				if (Object1.DataSourceName != Object2.DataSourceName)
					equal = false;
			}
			else if (Object1.DataSourceName == null ^ Object1.DataSourceName == null )
			{
				equal = false;
			}
			if (Object1.EventType != null && Object2.EventType != null )
			{
				if (Object1.EventType != Object2.EventType)
					equal = false;
			}
			else if (Object1.EventType == null ^ Object1.EventType == null )
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
		public static object GetPropertyValueByName(METAView_Event_Retrieval entity, string propertyName)
		{
			switch (propertyName)
			{
				case "WorkspaceName":
					return entity.WorkspaceName;
				case "WorkspaceTypeId":
					return entity.WorkspaceTypeId;
				case "VCStatusID":
					return entity.VCStatusID;
				case "pkid":
					return entity.pkid;
				case "Machine":
					return entity.Machine;
				case "VCMachineID":
					return entity.VCMachineID;
				case "Name":
					return entity.Name;
				case "Abbreviation":
					return entity.Abbreviation;
				case "Description":
					return entity.Description;
				case "IsBusinessExternal":
					return entity.IsBusinessExternal;
				case "ArchitectureStatus":
					return entity.ArchitectureStatus;
				case "ArchitectureStatusDate":
					return entity.ArchitectureStatusDate;
				case "DesignRationale":
					return entity.DesignRationale;
				case "GeneralRemarks":
					return entity.GeneralRemarks;
				case "GapType":
					return entity.GapType;
				case "DataSourceID":
					return entity.DataSourceID;
				case "DataSourceName":
					return entity.DataSourceName;
				case "EventType":
					return entity.EventType;
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
			return GetPropertyValueByName(this as METAView_Event_Retrieval, propertyName);
		}
		
		///<summary>
		/// Returns a String that represents the current object.
		///</summary>
		public override string ToString()
		{
			return string.Format(System.Globalization.CultureInfo.InvariantCulture,
				"{19}{18}- WorkspaceName: {0}{18}- WorkspaceTypeId: {1}{18}- VCStatusID: {2}{18}- pkid: {3}{18}- Machine: {4}{18}- VCMachineID: {5}{18}- Name: {6}{18}- Abbreviation: {7}{18}- Description: {8}{18}- IsBusinessExternal: {9}{18}- ArchitectureStatus: {10}{18}- ArchitectureStatusDate: {11}{18}- DesignRationale: {12}{18}- GeneralRemarks: {13}{18}- GapType: {14}{18}- DataSourceID: {15}{18}- DataSourceName: {16}{18}- EventType: {17}{18}", 
				this.WorkspaceName,
				(this.WorkspaceTypeId == null) ? string.Empty : this.WorkspaceTypeId.ToString(),
			     
				this.VCStatusID,
				this.pkid,
				this.Machine,
				(this.VCMachineID == null) ? string.Empty : this.VCMachineID.ToString(),
			     
				(this.Name == null) ? string.Empty : this.Name.ToString(),
			     
				(this.Abbreviation == null) ? string.Empty : this.Abbreviation.ToString(),
			     
				(this.Description == null) ? string.Empty : this.Description.ToString(),
			     
				(this.IsBusinessExternal == null) ? string.Empty : this.IsBusinessExternal.ToString(),
			     
				(this.ArchitectureStatus == null) ? string.Empty : this.ArchitectureStatus.ToString(),
			     
				(this.ArchitectureStatusDate == null) ? string.Empty : this.ArchitectureStatusDate.ToString(),
			     
				(this.DesignRationale == null) ? string.Empty : this.DesignRationale.ToString(),
			     
				(this.GeneralRemarks == null) ? string.Empty : this.GeneralRemarks.ToString(),
			     
				(this.GapType == null) ? string.Empty : this.GapType.ToString(),
			     
				(this.DataSourceID == null) ? string.Empty : this.DataSourceID.ToString(),
			     
				(this.DataSourceName == null) ? string.Empty : this.DataSourceName.ToString(),
			     
				(this.EventType == null) ? string.Empty : this.EventType.ToString(),
			     
				System.Environment.NewLine, 
				this.GetType());
		}
	
	}//End Class
	
	
	/// <summary>
	/// Enumerate the METAView_Event_Retrieval columns.
	/// </summary>
	[Serializable]
	public enum METAView_Event_RetrievalColumn
	{
		/// <summary>
		/// WorkspaceName : 
		/// </summary>
		[EnumTextValue("WorkspaceName")]
		[ColumnEnum("WorkspaceName", typeof(System.String), System.Data.DbType.AnsiString, false, false, false, 100)]
		WorkspaceName,
		/// <summary>
		/// WorkspaceTypeId : 
		/// </summary>
		[EnumTextValue("WorkspaceTypeId")]
		[ColumnEnum("WorkspaceTypeId", typeof(System.Int32), System.Data.DbType.Int32, false, false, true)]
		WorkspaceTypeId,
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
		pkid,
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
		/// Name : 
		/// </summary>
		[EnumTextValue("Name")]
		[ColumnEnum("Name", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 255)]
		Name,
		/// <summary>
		/// Abbreviation : 
		/// </summary>
		[EnumTextValue("Abbreviation")]
		[ColumnEnum("Abbreviation", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 255)]
		Abbreviation,
		/// <summary>
		/// Description : 
		/// </summary>
		[EnumTextValue("Description")]
		[ColumnEnum("Description", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 255)]
		Description,
		/// <summary>
		/// IsBusinessExternal : 
		/// </summary>
		[EnumTextValue("IsBusinessExternal")]
		[ColumnEnum("IsBusinessExternal", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 255)]
		IsBusinessExternal,
		/// <summary>
		/// ArchitectureStatus : 
		/// </summary>
		[EnumTextValue("ArchitectureStatus")]
		[ColumnEnum("ArchitectureStatus", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 255)]
		ArchitectureStatus,
		/// <summary>
		/// ArchitectureStatusDate : 
		/// </summary>
		[EnumTextValue("ArchitectureStatusDate")]
		[ColumnEnum("ArchitectureStatusDate", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 255)]
		ArchitectureStatusDate,
		/// <summary>
		/// DesignRationale : 
		/// </summary>
		[EnumTextValue("DesignRationale")]
		[ColumnEnum("DesignRationale", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 255)]
		DesignRationale,
		/// <summary>
		/// GeneralRemarks : 
		/// </summary>
		[EnumTextValue("GeneralRemarks")]
		[ColumnEnum("GeneralRemarks", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 255)]
		GeneralRemarks,
		/// <summary>
		/// GapType : 
		/// </summary>
		[EnumTextValue("GapType")]
		[ColumnEnum("GapType", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 255)]
		GapType,
		/// <summary>
		/// DataSourceID : 
		/// </summary>
		[EnumTextValue("DataSourceID")]
		[ColumnEnum("DataSourceID", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 255)]
		DataSourceID,
		/// <summary>
		/// DataSourceName : 
		/// </summary>
		[EnumTextValue("DataSourceName")]
		[ColumnEnum("DataSourceName", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 255)]
		DataSourceName,
		/// <summary>
		/// EventType : 
		/// </summary>
		[EnumTextValue("EventType")]
		[ColumnEnum("EventType", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 255)]
		EventType
	}//End enum

} // end namespace
