﻿/*
	File generated by NetTiers templates [www.nettiers.com]
	Important: Do not modify this file. Edit the file METAViewLocationRetrieval.cs instead.
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
	/// An object representation of the 'METAView_Location_Retrieval' view. [No description found in the database]	
	///</summary>
	[Serializable]
	[CLSCompliant(true)]
	[ToolboxItem("METAViewLocationRetrievalBase")]
	public abstract partial class METAViewLocationRetrievalBase : System.IComparable, System.ICloneable, INotifyPropertyChanged
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
		/// LocationType : 
		/// </summary>
		private System.String		  _locationType = null;
		
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
		/// Address : 
		/// </summary>
		private System.String		  _address = null;
		
		/// <summary>
		/// Telephone : 
		/// </summary>
		private System.String		  _telephone = null;
		
		/// <summary>
		/// Fax : 
		/// </summary>
		private System.String		  _fax = null;
		
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
		/// Creates a new <see cref="METAViewLocationRetrievalBase"/> instance.
		///</summary>
		public METAViewLocationRetrievalBase()
		{
		}		
		
		///<summary>
		/// Creates a new <see cref="METAViewLocationRetrievalBase"/> instance.
		///</summary>
		///<param name="_workspaceName"></param>
		///<param name="_workspaceTypeID"></param>
		///<param name="_vCStatusID"></param>
		///<param name="_pkid"></param>
		///<param name="_machine"></param>
		///<param name="_vCMachineID"></param>
		///<param name="_name"></param>
		///<param name="_locationType"></param>
		///<param name="_customField1"></param>
		///<param name="_customField2"></param>
		///<param name="_customField3"></param>
		///<param name="_address"></param>
		///<param name="_telephone"></param>
		///<param name="_fax"></param>
		///<param name="_gapType"></param>
		public METAViewLocationRetrievalBase(System.String _workspaceName, System.Int32? _workspaceTypeID, System.Int32 _vCStatusID, System.Int32 _pkid, System.String _machine, System.String _vCMachineID, System.String _name, System.String _locationType, System.String _customField1, System.String _customField2, System.String _customField3, System.String _address, System.String _telephone, System.String _fax, System.String _gapType)
		{
			this._workspaceName = _workspaceName;
			this._workspaceTypeID = _workspaceTypeID;
			this._vCStatusID = _vCStatusID;
			this._pkid = _pkid;
			this._machine = _machine;
			this._vCMachineID = _vCMachineID;
			this._name = _name;
			this._locationType = _locationType;
			this._customField1 = _customField1;
			this._customField2 = _customField2;
			this._customField3 = _customField3;
			this._address = _address;
			this._telephone = _telephone;
			this._fax = _fax;
			this._gapType = _gapType;
		}
		
		///<summary>
		/// A simple factory method to create a new <see cref="METAViewLocationRetrieval"/> instance.
		///</summary>
		///<param name="_workspaceName"></param>
		///<param name="_workspaceTypeID"></param>
		///<param name="_vCStatusID"></param>
		///<param name="_pkid"></param>
		///<param name="_machine"></param>
		///<param name="_vCMachineID"></param>
		///<param name="_name"></param>
		///<param name="_locationType"></param>
		///<param name="_customField1"></param>
		///<param name="_customField2"></param>
		///<param name="_customField3"></param>
		///<param name="_address"></param>
		///<param name="_telephone"></param>
		///<param name="_fax"></param>
		///<param name="_gapType"></param>
		public static METAViewLocationRetrieval CreateMETAViewLocationRetrieval(System.String _workspaceName, System.Int32? _workspaceTypeID, System.Int32 _vCStatusID, System.Int32 _pkid, System.String _machine, System.String _vCMachineID, System.String _name, System.String _locationType, System.String _customField1, System.String _customField2, System.String _customField3, System.String _address, System.String _telephone, System.String _fax, System.String _gapType)
		{
			METAViewLocationRetrieval newMETAViewLocationRetrieval = new METAViewLocationRetrieval();
			newMETAViewLocationRetrieval.WorkspaceName = _workspaceName;
			newMETAViewLocationRetrieval.WorkspaceTypeID = _workspaceTypeID;
			newMETAViewLocationRetrieval.VCStatusID = _vCStatusID;
			newMETAViewLocationRetrieval.Pkid = _pkid;
			newMETAViewLocationRetrieval.Machine = _machine;
			newMETAViewLocationRetrieval.VCMachineID = _vCMachineID;
			newMETAViewLocationRetrieval.Name = _name;
			newMETAViewLocationRetrieval.LocationType = _locationType;
			newMETAViewLocationRetrieval.CustomField1 = _customField1;
			newMETAViewLocationRetrieval.CustomField2 = _customField2;
			newMETAViewLocationRetrieval.CustomField3 = _customField3;
			newMETAViewLocationRetrieval.Address = _address;
			newMETAViewLocationRetrieval.Telephone = _telephone;
			newMETAViewLocationRetrieval.Fax = _fax;
			newMETAViewLocationRetrieval.GapType = _gapType;
			return newMETAViewLocationRetrieval;
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
		/// 	Gets or Sets the LocationType property. 
		///		
		/// </summary>
		/// <value>This type is varchar</value>
		/// <remarks>
		/// This property can be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.String LocationType
		{
			get
			{
				return this._locationType; 
			}
			set
			{
				if (_locationType == value)
					return;
					
				this._locationType = value;
				this._isDirty = true;
				
				OnPropertyChanged("LocationType");
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
		/// 	Gets or Sets the Address property. 
		///		
		/// </summary>
		/// <value>This type is varchar</value>
		/// <remarks>
		/// This property can be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.String Address
		{
			get
			{
				return this._address; 
			}
			set
			{
				if (_address == value)
					return;
					
				this._address = value;
				this._isDirty = true;
				
				OnPropertyChanged("Address");
			}
		}
		
		/// <summary>
		/// 	Gets or Sets the Telephone property. 
		///		
		/// </summary>
		/// <value>This type is varchar</value>
		/// <remarks>
		/// This property can be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.String Telephone
		{
			get
			{
				return this._telephone; 
			}
			set
			{
				if (_telephone == value)
					return;
					
				this._telephone = value;
				this._isDirty = true;
				
				OnPropertyChanged("Telephone");
			}
		}
		
		/// <summary>
		/// 	Gets or Sets the Fax property. 
		///		
		/// </summary>
		/// <value>This type is varchar</value>
		/// <remarks>
		/// This property can be set to null. 
		/// </remarks>
		[DescriptionAttribute(""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		public virtual System.String Fax
		{
			get
			{
				return this._fax; 
			}
			set
			{
				if (_fax == value)
					return;
					
				this._fax = value;
				this._isDirty = true;
				
				OnPropertyChanged("Fax");
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
			get { return "METAView_Location_Retrieval"; }
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
		///  Returns a Typed METAViewLocationRetrievalBase Entity 
		///</summary>
		public virtual METAViewLocationRetrievalBase Copy()
		{
			//shallow copy entity
			METAViewLocationRetrieval copy = new METAViewLocationRetrieval();
				copy.WorkspaceName = this.WorkspaceName;
				copy.WorkspaceTypeID = this.WorkspaceTypeID;
				copy.VCStatusID = this.VCStatusID;
				copy.Pkid = this.Pkid;
				copy.Machine = this.Machine;
				copy.VCMachineID = this.VCMachineID;
				copy.Name = this.Name;
				copy.LocationType = this.LocationType;
				copy.CustomField1 = this.CustomField1;
				copy.CustomField2 = this.CustomField2;
				copy.CustomField3 = this.CustomField3;
				copy.Address = this.Address;
				copy.Telephone = this.Telephone;
				copy.Fax = this.Fax;
				copy.GapType = this.GapType;
			copy.AcceptChanges();
			return (METAViewLocationRetrieval)copy;
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
		///<returns>true if toObject is a <see cref="METAViewLocationRetrievalBase"/> and has the same value as this instance; otherwise, false.</returns>
		public virtual bool Equals(METAViewLocationRetrievalBase toObject)
		{
			if (toObject == null)
				return false;
			return Equals(this, toObject);
		}
		
		
		///<summary>
		/// Determines whether the specified <see cref="METAViewLocationRetrievalBase"/> instances are considered equal.
		///</summary>
		///<param name="Object1">The first <see cref="METAViewLocationRetrievalBase"/> to compare.</param>
		///<param name="Object2">The second <see cref="METAViewLocationRetrievalBase"/> to compare. </param>
		///<returns>true if Object1 is the same instance as Object2 or if both are null references or if objA.Equals(objB) returns true; otherwise, false.</returns>
		public static bool Equals(METAViewLocationRetrievalBase Object1, METAViewLocationRetrievalBase Object2)
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
			if (Object1.LocationType != null && Object2.LocationType != null )
			{
				if (Object1.LocationType != Object2.LocationType)
					equal = false;
			}
			else if (Object1.LocationType == null ^ Object1.LocationType == null )
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
			if (Object1.Address != null && Object2.Address != null )
			{
				if (Object1.Address != Object2.Address)
					equal = false;
			}
			else if (Object1.Address == null ^ Object1.Address == null )
			{
				equal = false;
			}
			if (Object1.Telephone != null && Object2.Telephone != null )
			{
				if (Object1.Telephone != Object2.Telephone)
					equal = false;
			}
			else if (Object1.Telephone == null ^ Object1.Telephone == null )
			{
				equal = false;
			}
			if (Object1.Fax != null && Object2.Fax != null )
			{
				if (Object1.Fax != Object2.Fax)
					equal = false;
			}
			else if (Object1.Fax == null ^ Object1.Fax == null )
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
		public static object GetPropertyValueByName(METAViewLocationRetrieval entity, string propertyName)
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
				case "LocationType":
					return entity.LocationType;
				case "CustomField1":
					return entity.CustomField1;
				case "CustomField2":
					return entity.CustomField2;
				case "CustomField3":
					return entity.CustomField3;
				case "Address":
					return entity.Address;
				case "Telephone":
					return entity.Telephone;
				case "Fax":
					return entity.Fax;
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
			return GetPropertyValueByName(this as METAViewLocationRetrieval, propertyName);
		}
		
		///<summary>
		/// Returns a String that represents the current object.
		///</summary>
		public override string ToString()
		{
			return string.Format(System.Globalization.CultureInfo.InvariantCulture,
				"{16}{15}- WorkspaceName: {0}{15}- WorkspaceTypeID: {1}{15}- VCStatusID: {2}{15}- Pkid: {3}{15}- Machine: {4}{15}- VCMachineID: {5}{15}- Name: {6}{15}- LocationType: {7}{15}- CustomField1: {8}{15}- CustomField2: {9}{15}- CustomField3: {10}{15}- Address: {11}{15}- Telephone: {12}{15}- Fax: {13}{15}- GapType: {14}{15}", 
				this.WorkspaceName,
				(this.WorkspaceTypeID == null) ? string.Empty : this.WorkspaceTypeID.ToString(),
			     
				this.VCStatusID,
				this.Pkid,
				this.Machine,
				(this.VCMachineID == null) ? string.Empty : this.VCMachineID.ToString(),
			     
				(this.Name == null) ? string.Empty : this.Name.ToString(),
			     
				(this.LocationType == null) ? string.Empty : this.LocationType.ToString(),
			     
				(this.CustomField1 == null) ? string.Empty : this.CustomField1.ToString(),
			     
				(this.CustomField2 == null) ? string.Empty : this.CustomField2.ToString(),
			     
				(this.CustomField3 == null) ? string.Empty : this.CustomField3.ToString(),
			     
				(this.Address == null) ? string.Empty : this.Address.ToString(),
			     
				(this.Telephone == null) ? string.Empty : this.Telephone.ToString(),
			     
				(this.Fax == null) ? string.Empty : this.Fax.ToString(),
			     
				(this.GapType == null) ? string.Empty : this.GapType.ToString(),
			     
				System.Environment.NewLine, 
				this.GetType());
		}
	
	}//End Class
	
	
	/// <summary>
	/// Enumerate the METAViewLocationRetrieval columns.
	/// </summary>
	[Serializable]
	public enum METAViewLocationRetrievalColumn
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
		/// LocationType : 
		/// </summary>
		[EnumTextValue("LocationType")]
		[ColumnEnum("LocationType", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 255)]
		LocationType,
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
		/// Address : 
		/// </summary>
		[EnumTextValue("Address")]
		[ColumnEnum("Address", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 255)]
		Address,
		/// <summary>
		/// Telephone : 
		/// </summary>
		[EnumTextValue("Telephone")]
		[ColumnEnum("Telephone", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 255)]
		Telephone,
		/// <summary>
		/// Fax : 
		/// </summary>
		[EnumTextValue("Fax")]
		[ColumnEnum("Fax", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 255)]
		Fax,
		/// <summary>
		/// GapType : 
		/// </summary>
		[EnumTextValue("GapType")]
		[ColumnEnum("GapType", typeof(System.String), System.Data.DbType.AnsiString, false, false, true, 255)]
		GapType
	}//End enum

} // end namespace