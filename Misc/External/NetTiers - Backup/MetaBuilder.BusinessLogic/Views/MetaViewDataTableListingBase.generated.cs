﻿/*
	File generated by NetTiers templates [www.nettiers.com]
	Important: Do not modify this file. Edit the file METAViewDataTableListing.cs instead.
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
	/// An object representation of the 'METAView_DataTable_Listing' view. [No description found in the database]	
	///</summary>
	[Serializable]
	[CLSCompliant(true)]
	[ToolboxItem("METAViewDataTableListingBase")]
	public abstract partial class METAViewDataTableListingBase : System.IComparable, System.ICloneable, INotifyPropertyChanged
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
		/// Name : 
		/// </summary>
		private System.String		  _name = null;
		
		/// <summary>
		/// InitialPopulation : 
		/// </summary>
		private System.Int32?		  _initialPopulation = null;
		
		/// <summary>
		/// GrowthRateOverTime : 
		/// </summary>
		private System.String		  _growthRateOverTime = null;
		
		/// <summary>
		/// RecordSize : 
		/// </summary>
		private System.String		  _recordSize = null;
		
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
		/// GapType : 
		/// </summary>
		private System.String		  _gapType = null;
		
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
		/// Creates a new <see cref="METAViewDataTableListingBase"/> instance.
		///</summary>
		public METAViewDataTableListingBase()
		{
		}		
		
		///<summary>
		/// Creates a new <see cref="METAViewDataTableListingBase"/> instance.
		///</summary>
		///<param name="_workspaceName"></param>
		///<param name="_workspaceTypeID"></param>
		///<param name="_vCStatusID"></param>
		///<param name="_pkid"></param>
		///<param name="_machine"></param>
		///<param name="_vCMachineID"></param>
		///<param name="_name"></param>
		///<param name="_initialPopulation"></param>
		///<param name="_growthRateOverTime"></param>
		///<param name="_recordSize"></param>
		///<param name="_customField1"></param>
		///<param name="_customField2"></param>
		///<param name="_customField3"></param>
		///<param name="_gapType"></param>
		public METAViewDataTableListingBase(System.String _workspaceName, System.Int32? _workspaceTypeID, System.Int32 _vCStatusID, System.Int32 _pkid, System.String _machine, System.String _vCMachineID, System.String _name, System.Int32? _initialPopulation, System.String _growthRateOverTime, System.String _recordSize, System.String _customField1, System.String _customField2, System.String _customField3, System.String _gapType)
		{
			this._workspaceName = _workspaceName;
			this._workspaceTypeID = _workspaceTypeID;
			this._vCStatusID = _vCStatusID;
			this._pkid = _pkid;
			this._machine = _machine;
			this._vCMachineID = _vCMachineID;
			this._name = _name;
			this._initialPopulation = _initialPopulation;
			this._growthRateOverTime = _growthRateOverTime;
			this._recordSize = _recordSize;
			this._customField1 = _customField1;
			this._customField2 = _customField2;
			this._customField3 = _customField3;
			this._gapType = _gapType;
		}
		
		///<summary>
		/// A simple factory method to create a new <see cref="METAViewDataTableListing"/> instance.
		///</summary>
		///<param name="_workspaceName"></param>
		///<param name="_workspaceTypeID"></param>
		///<param name="_vCStatusID"></param>
		///<param name="_pkid"></param>
		///<param name="_machine"></param>
		///<param name="_vCMachineID"></param>
		///<param name="_name"></param>
		///<param name="_initialPopulation"></param>
		///<param name="_growthRateOverTime"></param>
		///<param name="_recordSize"></param>
		///<param name="_customField1"></param>
		///<param name="_customField2"></param>
		///<param name="_customField3"></param>
		///<param name="_gapType"></param>
		public static METAViewDataTableListing CreateMETAViewDataTableListing(System.String _workspaceName, System.Int32? _workspaceTypeID, System.Int32 _vCStatusID, System.Int32 _pkid, System.String _machine, System.String _vCMachineID, System.String _name, System.Int32? _initialPopulation, System.String _growthRateOverTime, System.String _recordSize, System.String _customField1, System.String _customField2, System.String _customField3, System.String _gapType)
		{
			METAViewDataTableListing newMETAViewDataTableListing = new METAViewDataTableListing();
			newMETAViewDataTableListing.WorkspaceName = _workspaceName;
			newMETAViewDataTableListing.WorkspaceTypeID = _workspaceTypeID;
			newMETAViewDataTableListing.VCStatusID = _vCStatusID;
			newMETAViewDataTableListing.Pkid = _pkid;
			newMETAViewDataTableListing.Machine = _machine;
			newMETAViewDataTableListing.VCMachineID = _vCMachineID;
			newMETAViewDataTableListing.Name = _name;
			newMETAViewDataTableListing.InitialPopulation = _initialPopulation;
			newMETAViewDataTableListing.GrowthRateOverTime = _growthRateOverTime;
			newMETAViewDataTableListing.RecordSize = _recordSize;
			newMETAViewDataTableListing.CustomField1 = _customField1;
			newMETAViewDataTableListing.CustomField2 = _customField2;
			newMETAViewDataTableListing.CustomField3 = _customField3;
			newMETAViewDataTableListing.GapType = _gapType;
			return newMETAViewDataTableListing;
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
		/// 	Gets or Sets the InitialPopulation property. 
		///		
		/// </summary>
		/// <value>This type is int</value>
		/// <remarks>
		/// This property can be set to null. 
		/// If this column is null, this property will return (int)0. It is up to the developer
		/// to check the value of IsInitialPopulationNull() and perform business logic appropriately.
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.Int32? InitialPopulation
		{
			get
			{
				return this._initialPopulation; 
			}
			set
			{
				if (_initialPopulation == value && InitialPopulation != null )
					return;
					
				this._initialPopulation = value;
				this._isDirty = true;
				
				OnPropertyChanged("InitialPopulation");
			}
		}
		
		/// <summary>
		/// 	Gets or Sets the GrowthRateOverTime property. 
		///		
		/// </summary>
		/// <value>This type is varchar</value>
		/// <remarks>
		/// This property can be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.String GrowthRateOverTime
		{
			get
			{
				return this._growthRateOverTime; 
			}
			set
			{
				if (_growthRateOverTime == value)
					return;
					
				this._growthRateOverTime = value;
				this._isDirty = true;
				
				OnPropertyChanged("GrowthRateOverTime");
			}
		}
		
		/// <summary>
		/// 	Gets or Sets the RecordSize property. 
		///		
		/// </summary>
		/// <value>This type is varchar</value>
		/// <remarks>
		/// This property can be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.String RecordSize
		{
			get
			{
				return this._recordSize; 
			}
			set
			{
				if (_recordSize == value)
					return;
					
				this._recordSize = value;
				this._isDirty = true;
				
				OnPropertyChanged("RecordSize");
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
			get { return "METAView_DataTable_Listing"; }
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
		///  Returns a Typed METAViewDataTableListingBase Entity 
		///</summary>
		public virtual METAViewDataTableListingBase Copy()
		{
			//shallow copy entity
			METAViewDataTableListing copy = new METAViewDataTableListing();
				copy.WorkspaceName = this.WorkspaceName;
				copy.WorkspaceTypeID = this.WorkspaceTypeID;
				copy.VCStatusID = this.VCStatusID;
				copy.Pkid = this.Pkid;
				copy.Machine = this.Machine;
				copy.VCMachineID = this.VCMachineID;
				copy.Name = this.Name;
				copy.InitialPopulation = this.InitialPopulation;
				copy.GrowthRateOverTime = this.GrowthRateOverTime;
				copy.RecordSize = this.RecordSize;
				copy.CustomField1 = this.CustomField1;
				copy.CustomField2 = this.CustomField2;
				copy.CustomField3 = this.CustomField3;
				copy.GapType = this.GapType;
			copy.AcceptChanges();
			return (METAViewDataTableListing)copy;
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
		///<returns>true if toObject is a <see cref="METAViewDataTableListingBase"/> and has the same value as this instance; otherwise, false.</returns>
		public virtual bool Equals(METAViewDataTableListingBase toObject)
		{
			if (toObject == null)
				return false;
			return Equals(this, toObject);
		}
		
		
		///<summary>
		/// Determines whether the specified <see cref="METAViewDataTableListingBase"/> instances are considered equal.
		///</summary>
		///<param name="Object1">The first <see cref="METAViewDataTableListingBase"/> to compare.</param>
		///<param name="Object2">The second <see cref="METAViewDataTableListingBase"/> to compare. </param>
		///<returns>true if Object1 is the same instance as Object2 or if both are null references or if objA.Equals(objB) returns true; otherwise, false.</returns>
		public static bool Equals(METAViewDataTableListingBase Object1, METAViewDataTableListingBase Object2)
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
			if (Object1.Name != null && Object2.Name != null )
			{
				if (Object1.Name != Object2.Name)
					equal = false;
			}
			else if (Object1.Name == null ^ Object1.Name == null )
			{
				equal = false;
			}
			if (Object1.InitialPopulation != null && Object2.InitialPopulation != null )
			{
				if (Object1.InitialPopulation != Object2.InitialPopulation)
					equal = false;
			}
			else if (Object1.InitialPopulation == null ^ Object1.InitialPopulation == null )
			{
				equal = false;
			}
			if (Object1.GrowthRateOverTime != null && Object2.GrowthRateOverTime != null )
			{
				if (Object1.GrowthRateOverTime != Object2.GrowthRateOverTime)
					equal = false;
			}
			else if (Object1.GrowthRateOverTime == null ^ Object1.GrowthRateOverTime == null )
			{
				equal = false;
			}
			if (Object1.RecordSize != null && Object2.RecordSize != null )
			{
				if (Object1.RecordSize != Object2.RecordSize)
					equal = false;
			}
			else if (Object1.RecordSize == null ^ Object1.RecordSize == null )
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
			if (Object1.GapType != null && Object2.GapType != null )
			{
				if (Object1.GapType != Object2.GapType)
					equal = false;
			}
			else if (Object1.GapType == null ^ Object1.GapType == null )
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
		public static object GetPropertyValueByName(METAViewDataTableListing entity, string propertyName)
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
				case "Name":
					return entity.Name;
				case "InitialPopulation":
					return entity.InitialPopulation;
				case "GrowthRateOverTime":
					return entity.GrowthRateOverTime;
				case "RecordSize":
					return entity.RecordSize;
				case "CustomField1":
					return entity.CustomField1;
				case "CustomField2":
					return entity.CustomField2;
				case "CustomField3":
					return entity.CustomField3;
				case "GapType":
					return entity.GapType;
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
			return GetPropertyValueByName(this as METAViewDataTableListing, propertyName);
		}
		
		///<summary>
		/// Returns a String that represents the current object.
		///</summary>
		public override string ToString()
		{
			return string.Format(System.Globalization.CultureInfo.InvariantCulture,
				"{15}{14}- WorkspaceName: {0}{14}- WorkspaceTypeID: {1}{14}- VCStatusID: {2}{14}- Pkid: {3}{14}- Machine: {4}{14}- VCMachineID: {5}{14}- Name: {6}{14}- InitialPopulation: {7}{14}- GrowthRateOverTime: {8}{14}- RecordSize: {9}{14}- CustomField1: {10}{14}- CustomField2: {11}{14}- CustomField3: {12}{14}- GapType: {13}{14}", 
				this.WorkspaceName,
				(this.WorkspaceTypeID == null) ? string.Empty : this.WorkspaceTypeID.ToString(),
			     
				this.VCStatusID,
				this.Pkid,
				this.Machine,
				(this.VCMachineID == null) ? string.Empty : this.VCMachineID.ToString(),
			     
				(this.Name == null) ? string.Empty : this.Name.ToString(),
			     
				(this.InitialPopulation == null) ? string.Empty : this.InitialPopulation.ToString(),
			     
				(this.GrowthRateOverTime == null) ? string.Empty : this.GrowthRateOverTime.ToString(),
			     
				(this.RecordSize == null) ? string.Empty : this.RecordSize.ToString(),
			     
				(this.CustomField1 == null) ? string.Empty : this.CustomField1.ToString(),
			     
				(this.CustomField2 == null) ? string.Empty : this.CustomField2.ToString(),
			     
				(this.CustomField3 == null) ? string.Empty : this.CustomField3.ToString(),
			     
				(this.GapType == null) ? string.Empty : this.GapType.ToString(),
			     
				System.Environment.NewLine, 
				this.GetType());
		}
	
	}//End Class
	
	
	/// <summary>
	/// Enumerate the METAViewDataTableListing columns.
	/// </summary>
	[Serializable]
	public enum METAViewDataTableListingColumn
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
		/// Name : 
		/// </summary>
		[EnumTextValue("Name")]
		[ColumnEnum("Name", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 255)]
		Name,
		/// <summary>
		/// InitialPopulation : 
		/// </summary>
		[EnumTextValue("InitialPopulation")]
		[ColumnEnum("InitialPopulation", typeof(System.Int32), System.Data.DbType.Int32, false, false, true)]
		InitialPopulation,
		/// <summary>
		/// GrowthRateOverTime : 
		/// </summary>
		[EnumTextValue("GrowthRateOverTime")]
		[ColumnEnum("GrowthRateOverTime", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 255)]
		GrowthRateOverTime,
		/// <summary>
		/// RecordSize : 
		/// </summary>
		[EnumTextValue("RecordSize")]
		[ColumnEnum("RecordSize", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 255)]
		RecordSize,
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
		CustomField3,
		/// <summary>
		/// GapType : 
		/// </summary>
		[EnumTextValue("GapType")]
		[ColumnEnum("GapType", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 255)]
		GapType
	}//End enum

} // end namespace
