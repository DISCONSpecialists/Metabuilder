﻿/*
	File generated by NetTiers templates [www.nettiers.com]
	Important: Do not modify this file. Edit the file METAView_Logic_Retrieval.cs instead.
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
	/// An object representation of the 'METAView_Logic_Retrieval' view. [No description found in the database]	
	///</summary>
	[Serializable]
	[CLSCompliant(true)]
	[ToolboxItem("METAView_Logic_RetrievalBase")]
	public abstract partial class METAView_Logic_RetrievalBase : System.IComparable, System.ICloneable, INotifyPropertyChanged
	{
		
		#region Variable Declarations
		
		/// <summary>
		/// WorkspaceName : 
		/// </summary>
		private System.String		  _workspaceName = string.Empty;
		
		/// <summary>
		/// WorkspaceTypeId : 
		/// </summary>
		private System.Int32?		  _WorkspaceTypeId = null;
		
		/// <summary>
		/// VCStatusID : 
		/// </summary>
		private System.Int32		  _VCStatusID = (int)0;
		
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
		/// Creates a new <see cref="METAView_Logic_RetrievalBase"/> instance.
		///</summary>
		public METAView_Logic_RetrievalBase()
		{
		}		
		
		///<summary>
		/// Creates a new <see cref="METAView_Logic_RetrievalBase"/> instance.
		///</summary>
		///<param name="_workspaceName"></param>
		///<param name="_WorkspaceTypeId"></param>
		///<param name="_VCStatusID"></param>
		///<param name="_pkid"></param>
		///<param name="_machine"></param>
		///<param name="_vCMachineID"></param>
		///<param name="_description"></param>
		///<param name="_gapType"></param>
		///<param name="_customField1"></param>
		///<param name="_customField2"></param>
		///<param name="_customField3"></param>
		public METAView_Logic_RetrievalBase(System.String _workspaceName, System.Int32? _WorkspaceTypeId, System.Int32 _VCStatusID, System.Int32 _pkid, System.String _machine, System.String _vCMachineID, System.String _description, System.String _gapType, System.String _customField1, System.String _customField2, System.String _customField3)
		{
			this._workspaceName = _workspaceName;
			this._WorkspaceTypeId = _WorkspaceTypeId;
			this._VCStatusID = _VCStatusID;
			this._pkid = _pkid;
			this._machine = _machine;
			this._vCMachineID = _vCMachineID;
			this._description = _description;
			this._gapType = _gapType;
			this._customField1 = _customField1;
			this._customField2 = _customField2;
			this._customField3 = _customField3;
		}
		
		///<summary>
		/// A simple factory method to create a new <see cref="METAView_Logic_Retrieval"/> instance.
		///</summary>
		///<param name="_workspaceName"></param>
		///<param name="_WorkspaceTypeId"></param>
		///<param name="_VCStatusID"></param>
		///<param name="_pkid"></param>
		///<param name="_machine"></param>
		///<param name="_vCMachineID"></param>
		///<param name="_description"></param>
		///<param name="_gapType"></param>
		///<param name="_customField1"></param>
		///<param name="_customField2"></param>
		///<param name="_customField3"></param>
		public static METAView_Logic_Retrieval CreateMETAView_Logic_Retrieval(System.String _workspaceName, System.Int32? _WorkspaceTypeId, System.Int32 _VCStatusID, System.Int32 _pkid, System.String _machine, System.String _vCMachineID, System.String _description, System.String _gapType, System.String _customField1, System.String _customField2, System.String _customField3)
		{
			METAView_Logic_Retrieval newMETAView_Logic_Retrieval = new METAView_Logic_Retrieval();
			newMETAView_Logic_Retrieval.WorkspaceName = _workspaceName;
			newMETAView_Logic_Retrieval.WorkspaceTypeId = _WorkspaceTypeId;
			newMETAView_Logic_Retrieval.VCStatusID = _VCStatusID;
			newMETAView_Logic_Retrieval.pkid = _pkid;
			newMETAView_Logic_Retrieval.Machine = _machine;
			newMETAView_Logic_Retrieval.VCMachineID = _vCMachineID;
			newMETAView_Logic_Retrieval.Description = _description;
			newMETAView_Logic_Retrieval.GapType = _gapType;
			newMETAView_Logic_Retrieval.CustomField1 = _customField1;
			newMETAView_Logic_Retrieval.CustomField2 = _customField2;
			newMETAView_Logic_Retrieval.CustomField3 = _customField3;
			return newMETAView_Logic_Retrieval;
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
				return this._WorkspaceTypeId; 
			}
			set
			{
				if (_WorkspaceTypeId == value && WorkspaceTypeId != null )
					return;
					
				this._WorkspaceTypeId = value;
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
				return this._VCStatusID; 
			}
			set
			{
				if (_VCStatusID == value)
					return;
					
				this._VCStatusID = value;
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
			get { return "METAView_Logic_Retrieval"; }
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
		///  Returns a Typed METAView_Logic_RetrievalBase Entity 
		///</summary>
		public virtual METAView_Logic_RetrievalBase Copy()
		{
			//shallow copy entity
			METAView_Logic_Retrieval copy = new METAView_Logic_Retrieval();
				copy.WorkspaceName = this.WorkspaceName;
				copy.WorkspaceTypeId = this.WorkspaceTypeId;
				copy.VCStatusID = this.VCStatusID;
				copy.pkid = this.pkid;
				copy.Machine = this.Machine;
				copy.VCMachineID = this.VCMachineID;
				copy.Description = this.Description;
				copy.GapType = this.GapType;
				copy.CustomField1 = this.CustomField1;
				copy.CustomField2 = this.CustomField2;
				copy.CustomField3 = this.CustomField3;
			copy.AcceptChanges();
			return (METAView_Logic_Retrieval)copy;
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
		///<returns>true if toObject is a <see cref="METAView_Logic_RetrievalBase"/> and has the same value as this instance; otherwise, false.</returns>
		public virtual bool Equals(METAView_Logic_RetrievalBase toObject)
		{
			if (toObject == null)
				return false;
			return Equals(this, toObject);
		}
		
		
		///<summary>
		/// Determines whether the specified <see cref="METAView_Logic_RetrievalBase"/> instances are considered equal.
		///</summary>
		///<param name="Object1">The first <see cref="METAView_Logic_RetrievalBase"/> to compare.</param>
		///<param name="Object2">The second <see cref="METAView_Logic_RetrievalBase"/> to compare. </param>
		///<returns>true if Object1 is the same instance as Object2 or if both are null references or if objA.Equals(objB) returns true; otherwise, false.</returns>
		public static bool Equals(METAView_Logic_RetrievalBase Object1, METAView_Logic_RetrievalBase Object2)
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
		public static object GetPropertyValueByName(METAView_Logic_Retrieval entity, string propertyName)
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
			return GetPropertyValueByName(this as METAView_Logic_Retrieval, propertyName);
		}
		
		///<summary>
		/// Returns a String that represents the current object.
		///</summary>
		public override string ToString()
		{
			return string.Format(System.Globalization.CultureInfo.InvariantCulture,
				"{12}{11}- WorkspaceName: {0}{11}- WorkspaceTypeId: {1}{11}- VCStatusID: {2}{11}- pkid: {3}{11}- Machine: {4}{11}- VCMachineID: {5}{11}- Description: {6}{11}- GapType: {7}{11}- CustomField1: {8}{11}- CustomField2: {9}{11}- CustomField3: {10}{11}", 
				this.WorkspaceName,
				(this.WorkspaceTypeId == null) ? string.Empty : this.WorkspaceTypeId.ToString(),
			     
				this.VCStatusID,
				this.pkid,
				this.Machine,
				(this.VCMachineID == null) ? string.Empty : this.VCMachineID.ToString(),
			     
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
	/// Enumerate the METAView_Logic_Retrieval columns.
	/// </summary>
	[Serializable]
	public enum METAView_Logic_RetrievalColumn
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
