﻿
/*
	File generated by NetTiers templates [www.nettiers.com]
	Important: Do not modify this file. Edit the file UURI.cs instead.
*/

#region using directives
using System;
using System.ComponentModel;
using System.Collections;
using System.Xml.Serialization;
using System.Runtime.Serialization;

using MetaBuilder.BusinessLogic.Validation;
#endregion

namespace MetaBuilder.BusinessLogic
{
	///<summary>
	/// An object representation of the 'UURI' table. [No description found the database]	
	///</summary>
	[Serializable]


	[DataObject, CLSCompliant(true)]
	public abstract partial class UURIBase : EntityBase, IUURI, IEntityId<UURIKey>, System.IComparable, System.ICloneable, ICloneableEx, IEditableObject, IComponent, INotifyPropertyChanged
	{		
		#region Variable Declarations
		
		/// <summary>
		///  Hold the inner data of the entity.
		/// </summary>
		private UURIEntityData entityData;
		
		/// <summary>
		/// 	Hold the original data of the entity, as loaded from the repository.
		/// </summary>
		private UURIEntityData _originalData;
		
		/// <summary>
		/// 	Hold a backup of the inner data of the entity.
		/// </summary>
		private UURIEntityData backupData; 
		
		/// <summary>
		/// 	Key used if Tracking is Enabled for the <see cref="EntityLocator" />.
		/// </summary>
		private string entityTrackingKey;
		
		/// <summary>
		/// 	Hold the parent TList&lt;entity&gt; in which this entity maybe contained.
		/// </summary>
		/// <remark>Mostly used for databinding</remark>
		[NonSerialized]
		private TList<UURI> parentCollection;
		
		private bool inTxn = false;
		
		/// <summary>
		/// Occurs when a value is being changed for the specified column.
		/// </summary>
		[field:NonSerialized]
		public event UURIEventHandler ColumnChanging;		
		
		/// <summary>
		/// Occurs after a value has been changed for the specified column.
		/// </summary>
		[field:NonSerialized]
		public event UURIEventHandler ColumnChanged;
		
		#endregion Variable Declarations
		
		#region Constructors
		///<summary>
		/// Creates a new <see cref="UURIBase"/> instance.
		///</summary>
		public UURIBase()
		{
			this.entityData = new UURIEntityData();
			this.backupData = null;
		}		
		
		///<summary>
		/// Creates a new <see cref="UURIBase"/> instance.
		///</summary>
		///<param name="_fileName"></param>
		public UURIBase(System.String _fileName)
		{
			this.entityData = new UURIEntityData();
			this.backupData = null;

			this.FileName = _fileName;
		}
		
		///<summary>
		/// A simple factory method to create a new <see cref="UURI"/> instance.
		///</summary>
		///<param name="_fileName"></param>
		public static UURI CreateUURI(System.String _fileName)
		{
			UURI newUURI = new UURI();
			newUURI.FileName = _fileName;
			return newUURI;
		}
				
		#endregion Constructors
			
		#region Properties	
		
		#region Data Properties		
		/// <summary>
		/// 	Gets or sets the pkid property. 
		///		
		/// </summary>
		/// <value>This type is int.</value>
		/// <remarks>
		/// This property can not be set to null. 
		/// </remarks>




		[ReadOnlyAttribute(false)/*, XmlIgnoreAttribute()*/, DescriptionAttribute(@""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		[DataObjectField(true, true, false)]
		public virtual System.Int32 pkid
		{
			get
			{
				return this.entityData.pkid; 
			}
			
			set
			{
				if (this.entityData.pkid == value)
					return;
					
				OnColumnChanging(UURIColumn.pkid, this.entityData.pkid);
				this.entityData.pkid = value;
				this.EntityId.pkid = value;
				if (this.EntityState == EntityState.Unchanged)
					this.EntityState = EntityState.Changed;
				OnColumnChanged(UURIColumn.pkid, this.entityData.pkid);
				OnPropertyChanged("pkid");
			}
		}
		
		/// <summary>
		/// 	Gets or sets the FileName property. 
		///		
		/// </summary>
		/// <value>This type is varchar.</value>
		/// <remarks>
		/// This property can not be set to null. 
		/// </remarks>
		/// <exception cref="ArgumentNullException">If you attempt to set to null.</exception>




		[DescriptionAttribute(@""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		[DataObjectField(false, false, false, 2000)]
		public virtual System.String FileName
		{
			get
			{
				return this.entityData.FileName; 
			}
			
			set
			{
				if (this.entityData.FileName == value)
					return;
					
				OnColumnChanging(UURIColumn.FileName, this.entityData.FileName);
				this.entityData.FileName = value;
				if (this.EntityState == EntityState.Unchanged)
					this.EntityState = EntityState.Changed;
				OnColumnChanged(UURIColumn.FileName, this.entityData.FileName);
				OnPropertyChanged("FileName");
			}
		}
		
		#endregion Data Properties		

		#region Source Foreign Key Property
				
		#endregion
		
		#region Children Collections
	
		/// <summary>
		///	Holds a collection of DomainDefinitionPossibleValue objects
		///	which are related to this object through the relation FK_DomainDefinitionPossibleValue_URI
		/// </summary>	
		[System.ComponentModel.Bindable(System.ComponentModel.BindableSupport.Yes)]
		public virtual TList<DomainDefinitionPossibleValue> DomainDefinitionPossibleValueCollection
		{
			get { return entityData.DomainDefinitionPossibleValueCollection; }
			set { entityData.DomainDefinitionPossibleValueCollection = value; }	
		}
		#endregion Children Collections
		
		#endregion
		#region Validation
		
		/// <summary>
		/// Assigns validation rules to this object based on model definition.
		/// </summary>
		/// <remarks>This method overrides the base class to add schema related validation.</remarks>
		protected override void AddValidationRules()
		{
			//Validation rules based on database schema.
			ValidationRules.AddRule( CommonRules.NotNull,
				new ValidationRuleArgs("FileName", "File Name"));
			ValidationRules.AddRule( CommonRules.StringMaxLength, 
				new CommonRules.MaxLengthRuleArgs("FileName", "File Name", 2000));
		}
   		#endregion
		
		#region Table Meta Data
		/// <summary>
		///		The name of the underlying database table.
		/// </summary>
		[BrowsableAttribute(false), XmlIgnoreAttribute()]
		public override string TableName
		{
			get { return "UURI"; }
		}
		
		/// <summary>
		///		The name of the underlying database table's columns.
		/// </summary>
		[BrowsableAttribute(false), XmlIgnoreAttribute()]
		public override string[] TableColumns
		{
			get
			{
				return new string[] {"pkid", "FileName"};
			}
		}
		#endregion 
		
		#region IEditableObject
		
		#region  CancelAddNew Event
		/// <summary>
        /// The delegate for the CancelAddNew event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		public delegate void CancelAddNewEventHandler(object sender, EventArgs e);
    
    	/// <summary>
		/// The CancelAddNew event.
		/// </summary>
		[field:NonSerialized]
		public event CancelAddNewEventHandler CancelAddNew ;

		/// <summary>
        /// Called when [cancel add new].
        /// </summary>
        public void OnCancelAddNew()
        {    
			if (!SuppressEntityEvents)
			{
	            CancelAddNewEventHandler handler = CancelAddNew ;
            	if (handler != null)
	            {    
    	            handler(this, EventArgs.Empty) ;
        	    }
	        }
        }
		#endregion 
		
		/// <summary>
		/// Begins an edit on an object.
		/// </summary>
		void IEditableObject.BeginEdit() 
	    {
	        //Console.WriteLine("Start BeginEdit");
	        if (!inTxn) 
	        {
	            this.backupData = this.entityData.Clone() as UURIEntityData;
	            inTxn = true;
	            //Console.WriteLine("BeginEdit");
	        }
	        //Console.WriteLine("End BeginEdit");
	    }
	
		/// <summary>
		/// Discards changes since the last <c>BeginEdit</c> call.
		/// </summary>
	    void IEditableObject.CancelEdit() 
	    {
	        //Console.WriteLine("Start CancelEdit");
	        if (this.inTxn) 
	        {
	            this.entityData = this.backupData;
	            this.backupData = null;
				this.inTxn = false;

				if (this.bindingIsNew)
	        	//if (this.EntityState == EntityState.Added)
	        	{
					if (this.parentCollection != null)
						this.parentCollection.Remove( (UURI) this ) ;
				}	            
	        }
	        //Console.WriteLine("End CancelEdit");
	    }
	
		/// <summary>
		/// Pushes changes since the last <c>BeginEdit</c> or <c>IBindingList.AddNew</c> call into the underlying object.
		/// </summary>
	    void IEditableObject.EndEdit() 
	    {
	        //Console.WriteLine("Start EndEdit" + this.custData.id + this.custData.lastName);
	        if (this.inTxn) 
	        {
	            this.backupData = null;
				if (this.IsDirty) 
				{
					if (this.bindingIsNew) {
						this.EntityState = EntityState.Added;
						this.bindingIsNew = false ;
					}
					else
						if (this.EntityState == EntityState.Unchanged) 
							this.EntityState = EntityState.Changed ;
				}

				this.bindingIsNew = false ;
	            this.inTxn = false;	            
	        }
	        //Console.WriteLine("End EndEdit");
	    }
	    
	    /// <summary>
        /// Gets or sets the parent collection of this current entity, if available.
        /// </summary>
        /// <value>The parent collection.</value>
	    [XmlIgnore]
		[Browsable(false)]
	    public override object ParentCollection
	    {
	        get 
	        {
	            return this.parentCollection;
	        }
	        set 
	        {
	            this.parentCollection = value as TList<UURI>;
	        }
	    }
	    
	    /// <summary>
        /// Called when the entity is changed.
        /// </summary>
	    private void OnEntityChanged() 
	    {
	        if (!SuppressEntityEvents && !inTxn && this.parentCollection != null) 
	        {
	            this.parentCollection.EntityChanged(this as UURI);
	        }
	    }


		#endregion
		
		#region ICloneable Members
		///<summary>
		///  Returns a Typed UURI Entity 
		///</summary>
		protected virtual UURI Copy(IDictionary existingCopies)
		{
			if (existingCopies == null)
			{
				// This is the root of the tree to be copied!
				existingCopies = new Hashtable();
			}

			//shallow copy entity
			UURI copy = new UURI();
			existingCopies.Add(this, copy);
			copy.SuppressEntityEvents = true;
				copy.pkid = this.pkid;
				copy.FileName = this.FileName;
			
		
			//deep copy nested objects
			copy.DomainDefinitionPossibleValueCollection = (TList<DomainDefinitionPossibleValue>) MakeCopyOf(this.DomainDefinitionPossibleValueCollection, existingCopies); 
			copy.EntityState = this.EntityState;
			copy.SuppressEntityEvents = false;
			return copy;
		}		
		
		
		
		///<summary>
		///  Returns a Typed UURI Entity 
		///</summary>
		public virtual UURI Copy()
		{
			return this.Copy(null);	
		}
		
		///<summary>
		/// ICloneable.Clone() Member, returns the Shallow Copy of this entity.
		///</summary>
		public object Clone()
		{
			return this.Copy(null);
		}
		
		///<summary>
		/// ICloneableEx.Clone() Member, returns the Shallow Copy of this entity.
		///</summary>
		public object Clone(IDictionary existingCopies)
		{
			return this.Copy(existingCopies);
		}
		
		///<summary>
		/// Returns a deep copy of the child collection object passed in.
		///</summary>
		public static object MakeCopyOf(object x)
		{
			if (x == null)
				return null;
				
			if (x is ICloneable)
			{
				// Return a deep copy of the object
				return ((ICloneable)x).Clone();
			}
			else
				throw new System.NotSupportedException("Object Does Not Implement the ICloneable Interface.");
		}
		
		///<summary>
		/// Returns a deep copy of the child collection object passed in.
		///</summary>
		public static object MakeCopyOf(object x, IDictionary existingCopies)
		{
			if (x == null)
				return null;
			
			if (x is ICloneableEx)
			{
				// Return a deep copy of the object
				return ((ICloneableEx)x).Clone(existingCopies);
			}
			else if (x is ICloneable)
			{
				// Return a deep copy of the object
				return ((ICloneable)x).Clone();
			}
			else
				throw new System.NotSupportedException("Object Does Not Implement the ICloneable or IClonableEx Interface.");
		}
		
		
		///<summary>
		///  Returns a Typed UURI Entity which is a deep copy of the current entity.
		///</summary>
		public virtual UURI DeepCopy()
		{
			return EntityHelper.Clone<UURI>(this as UURI);	
		}
		#endregion
		
		#region Methods	
			
		///<summary>
		/// Revert all changes and restore original values.
		///</summary>
		public override void CancelChanges()
		{
			IEditableObject obj = (IEditableObject) this;
			obj.CancelEdit();

			this.entityData = null;
			if (this._originalData != null)
			{
				this.entityData = this._originalData.Clone() as UURIEntityData;
			}
			else
			{
				//Since this had no _originalData, then just reset the entityData with a new one.  entityData cannot be null.
				this.entityData = new UURIEntityData();
			}
		}	
		
		/// <summary>
		/// Accepts the changes made to this object.
		/// </summary>
		/// <remarks>
		/// After calling this method, properties: IsDirty, IsNew are false. IsDeleted flag remains unchanged as it is handled by the parent List.
		/// </remarks>
		public override void AcceptChanges()
		{
			base.AcceptChanges();

			// we keep of the original version of the data
			this._originalData = null;
			this._originalData = this.entityData.Clone() as UURIEntityData;
		}
		
		#region Comparision with original data
		
		/// <summary>
		/// Determines whether the property value has changed from the original data.
		/// </summary>
		/// <param name="column">The column.</param>
		/// <returns>
		/// 	<c>true</c> if the property value has changed; otherwise, <c>false</c>.
		/// </returns>
		public bool IsPropertyChanged(UURIColumn column)
		{
			switch(column)
			{
					case UURIColumn.pkid:
					return entityData.pkid != _originalData.pkid;
					case UURIColumn.FileName:
					return entityData.FileName != _originalData.FileName;
			
				default:
					return false;
			}
		}
		
		/// <summary>
		/// Determines whether the property value has changed from the original data.
		/// </summary>
		/// <param name="columnName">The column name.</param>
		/// <returns>
		/// 	<c>true</c> if the property value has changed; otherwise, <c>false</c>.
		/// </returns>
		public override bool IsPropertyChanged(string columnName)
		{
			return 	IsPropertyChanged(EntityHelper.GetEnumValue< UURIColumn >(columnName));
		}
		
		/// <summary>
		/// Determines whether the data has changed from original.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if data has changed; otherwise, <c>false</c>.
		/// </returns>
		public bool HasDataChanged()
		{
			bool result = false;
			result = result || entityData.pkid != _originalData.pkid;
			result = result || entityData.FileName != _originalData.FileName;
			return result;
		}	
		
		///<summary>
		///  Returns a UURI Entity with the original data.
		///</summary>
		public UURI GetOriginalEntity()
		{
			if (_originalData != null)
				return CreateUURI(
				_originalData.FileName
				);
				
			return (UURI)this.Clone();
		}
		#endregion
	
	#region Value Semantics Instance Equality
        ///<summary>
        /// Returns a value indicating whether this instance is equal to a specified object using value semantics.
        ///</summary>
        ///<param name="Object1">An object to compare to this instance.</param>
        ///<returns>true if Object1 is a <see cref="UURIBase"/> and has the same value as this instance; otherwise, false.</returns>
        public override bool Equals(object Object1)
        {
			// Cast exception if Object1 is null or DbNull
			if (Object1 != null && Object1 != DBNull.Value && Object1 is UURIBase)
				return ValueEquals(this, (UURIBase)Object1);
			else
				return false;
        }

        /// <summary>
		/// Serves as a hash function for a particular type, suitable for use in hashing algorithms and data structures like a hash table.
        /// Provides a hash function that is appropriate for <see cref="UURIBase"/> class 
        /// and that ensures a better distribution in the hash table
        /// </summary>
        /// <returns>number (hash code) that corresponds to the value of an object</returns>
        public override int GetHashCode()
        {
			return this.pkid.GetHashCode() ^ 
					this.FileName.GetHashCode();
        }
		
		///<summary>
		/// Returns a value indicating whether this instance is equal to a specified object using value semantics.
		///</summary>
		///<param name="toObject">An object to compare to this instance.</param>
		///<returns>true if toObject is a <see cref="UURIBase"/> and has the same value as this instance; otherwise, false.</returns>
		public virtual bool Equals(UURIBase toObject)
		{
			if (toObject == null)
				return false;
			return ValueEquals(this, toObject);
		}
		#endregion
		
		///<summary>
		/// Determines whether the specified <see cref="UURIBase"/> instances are considered equal using value semantics.
		///</summary>
		///<param name="Object1">The first <see cref="UURIBase"/> to compare.</param>
		///<param name="Object2">The second <see cref="UURIBase"/> to compare. </param>
		///<returns>true if Object1 is the same instance as Object2 or if both are null references or if objA.Equals(objB) returns true; otherwise, false.</returns>
		public static bool ValueEquals(UURIBase Object1, UURIBase Object2)
		{
			// both are null
			if (Object1 == null && Object2 == null)
				return true;

			// one or the other is null, but not both
			if (Object1 == null ^ Object2 == null)
				return false;
				
			bool equal = true;
			if (Object1.pkid != Object2.pkid)
				equal = false;
			if (Object1.FileName != Object2.FileName)
				equal = false;
					
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
			//return this. GetPropertyName(SourceTable.PrimaryKey.MemberColumns[0]) .CompareTo(((UURIBase)obj).GetPropertyName(SourceTable.PrimaryKey.MemberColumns[0]));
		}
		
		/*
		// static method to get a Comparer object
        public static UURIComparer GetComparer()
        {
            return new UURIComparer();
        }
        */

        // Comparer delegates back to UURI
        // Employee uses the integer's default
        // CompareTo method
        /*
        public int CompareTo(Item rhs)
        {
            return this.Id.CompareTo(rhs.Id);
        }
        */

/*
        // Special implementation to be called by custom comparer
        public int CompareTo(UURI rhs, UURIColumn which)
        {
            switch (which)
            {
            	
            	
            	case UURIColumn.pkid:
            		return this.pkid.CompareTo(rhs.pkid);
            		
            		                 
            	
            	
            	case UURIColumn.FileName:
            		return this.FileName.CompareTo(rhs.FileName);
            		
            		                 
            }
            return 0;
        }
        */
	
		#endregion
		
		#region IComponent Members
		
		private ISite _site = null;

		/// <summary>
		/// Gets or Sets the site where this data is located.
		/// </summary>
		[XmlIgnore]
		[SoapIgnore]
		[Browsable(false)]
		public ISite Site
		{
			get{ return this._site; }
			set{ this._site = value; }
		}

		#endregion

		#region IDisposable Members
		
		/// <summary>
		/// Notify those that care when we dispose.
		/// </summary>
		[field:NonSerialized]
		public event System.EventHandler Disposed;

		/// <summary>
		/// Clean up. Nothing here though.
		/// </summary>
		public virtual void Dispose()
		{
			this.parentCollection = null;
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		
		/// <summary>
		/// Clean up.
		/// </summary>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				EventHandler handler = Disposed;
				if (handler != null)
					handler(this, EventArgs.Empty);
			}
		}
		
		#endregion
				
		#region IEntityKey<UURIKey> Members
		
		// member variable for the EntityId property
		private UURIKey _entityId;

		/// <summary>
		/// Gets or sets the EntityId property.
		/// </summary>
		[XmlIgnore]
		public virtual UURIKey EntityId
		{
			get
			{
				if ( _entityId == null )
				{
					_entityId = new UURIKey(this);
				}

				return _entityId;
			}
			set
			{
				if ( value != null )
				{
					value.Entity = this;
				}
				
				_entityId = value;
			}
		}
		
		#endregion
		
		#region EntityState
		/// <summary>
		///		Indicates state of object
		/// </summary>
		/// <remarks>0=Unchanged, 1=Added, 2=Changed</remarks>
		[BrowsableAttribute(false) , XmlIgnoreAttribute()]
		public override EntityState EntityState 
		{ 
			get{ return entityData.EntityState;	 } 
			set{ entityData.EntityState = value; } 
		}
		#endregion 
		
		#region EntityTrackingKey
		///<summary>
		/// Provides the tracking key for the <see cref="EntityLocator"/>
		///</summary>
		[XmlIgnore]
		public override string EntityTrackingKey
		{
			get
			{
				if(entityTrackingKey == null)
					entityTrackingKey = new System.Text.StringBuilder("UURI")
					.Append("|").Append( this.pkid.ToString()).ToString();
				return entityTrackingKey;
			}
			set
		    {
		        if (value != null)
                    entityTrackingKey = value;
		    }
		}
		#endregion 
		
		#region ToString Method
		
		///<summary>
		/// Returns a String that represents the current object.
		///</summary>
		public override string ToString()
		{
			return string.Format(System.Globalization.CultureInfo.InvariantCulture,
				"{3}{2}- pkid: {0}{2}- FileName: {1}{2}{4}", 
				this.pkid,
				this.FileName,
				System.Environment.NewLine, 
				this.GetType(),
				this.Error.Length == 0 ? string.Empty : string.Format("- Error: {0}\n",this.Error));
		}
		
		#endregion ToString Method
		
		#region Inner data class
		
	/// <summary>
	///		The data structure representation of the 'UURI' table.
	/// </summary>
	/// <remarks>
	/// 	This struct is generated by a tool and should never be modified.
	/// </remarks>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Serializable]
	internal protected class UURIEntityData : ICloneable, ICloneableEx
	{
		#region Variable Declarations
		private EntityState currentEntityState = EntityState.Added;
		
		#region Primary key(s)
		/// <summary>			
		/// pkid : 
		/// </summary>
		/// <remarks>Member of the primary key of the underlying table "UURI"</remarks>
		public System.Int32 pkid;
			
		#endregion
		
		#region Non Primary key(s)
		
		
		/// <summary>
		/// FileName : 
		/// </summary>
		public System.String		  FileName = string.Empty;
		#endregion
			
		#region Source Foreign Key Property
				
		#endregion
		#endregion Variable Declarations
	
		#region Data Properties

		#region DomainDefinitionPossibleValueCollection
		
		private TList<DomainDefinitionPossibleValue> _domainDefinitionPossibleValueURI_ID;
		
		/// <summary>
		///	Holds a collection of entity objects
		///	which are related to this object through the relation _domainDefinitionPossibleValueURI_ID
		/// </summary>
		
		public TList<DomainDefinitionPossibleValue> DomainDefinitionPossibleValueCollection
		{
			get
			{
				if (_domainDefinitionPossibleValueURI_ID == null)
				{
				_domainDefinitionPossibleValueURI_ID = new TList<DomainDefinitionPossibleValue>();
				}
	
				return _domainDefinitionPossibleValueURI_ID;
			}
			set { _domainDefinitionPossibleValueURI_ID = value; }
		}
		
		#endregion

		#endregion Data Properties
		
		#region Clone Method

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public Object Clone()
		{
			UURIEntityData _tmp = new UURIEntityData();
						
			_tmp.pkid = this.pkid;
			
			_tmp.FileName = this.FileName;
			
			#region Source Parent Composite Entities
			#endregion
		
			#region Child Collections
			//deep copy nested objects
			if (this._domainDefinitionPossibleValueURI_ID != null)
				_tmp.DomainDefinitionPossibleValueCollection = (TList<DomainDefinitionPossibleValue>) MakeCopyOf(this.DomainDefinitionPossibleValueCollection); 
			#endregion Child Collections
			
			//EntityState
			_tmp.EntityState = this.EntityState;
			
			return _tmp;
		}
		
		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone(IDictionary existingCopies)
		{
			if (existingCopies == null)
				existingCopies = new Hashtable();
				
			UURIEntityData _tmp = new UURIEntityData();
						
			_tmp.pkid = this.pkid;
			
			_tmp.FileName = this.FileName;
			
			#region Source Parent Composite Entities
			#endregion
		
			#region Child Collections
			//deep copy nested objects
			_tmp.DomainDefinitionPossibleValueCollection = (TList<DomainDefinitionPossibleValue>) MakeCopyOf(this.DomainDefinitionPossibleValueCollection, existingCopies); 
			#endregion Child Collections
			
			//EntityState
			_tmp.EntityState = this.EntityState;
			
			return _tmp;
		}
		
		#endregion Clone Method
		
		/// <summary>
		///		Indicates state of object
		/// </summary>
		/// <remarks>0=Unchanged, 1=Added, 2=Changed</remarks>
		[BrowsableAttribute(false), XmlIgnoreAttribute()]
		public EntityState	EntityState
		{
			get { return currentEntityState;  }
			set { currentEntityState = value; }
		}
	
	}//End struct











		#endregion
		
				
		
		#region Events trigger
		/// <summary>
		/// Raises the <see cref="ColumnChanging" /> event.
		/// </summary>
		/// <param name="column">The <see cref="UURIColumn"/> which has raised the event.</param>
		public virtual void OnColumnChanging(UURIColumn column)
		{
			OnColumnChanging(column, null);
			return;
		}
		
		/// <summary>
		/// Raises the <see cref="ColumnChanged" /> event.
		/// </summary>
		/// <param name="column">The <see cref="UURIColumn"/> which has raised the event.</param>
		public virtual void OnColumnChanged(UURIColumn column)
		{
			OnColumnChanged(column, null);
			return;
		} 
		
		
		/// <summary>
		/// Raises the <see cref="ColumnChanging" /> event.
		/// </summary>
		/// <param name="column">The <see cref="UURIColumn"/> which has raised the event.</param>
		/// <param name="value">The changed value.</param>
		public virtual void OnColumnChanging(UURIColumn column, object value)
		{
			if(IsEntityTracked && EntityState != EntityState.Added && !EntityManager.TrackChangedEntities)
				EntityManager.StopTracking(entityTrackingKey);
				
			if (!SuppressEntityEvents)
			{
				UURIEventHandler handler = ColumnChanging;
				if(handler != null)
				{
					handler(this, new UURIEventArgs(column, value));
				}
			}
		}
		
		/// <summary>
		/// Raises the <see cref="ColumnChanged" /> event.
		/// </summary>
		/// <param name="column">The <see cref="UURIColumn"/> which has raised the event.</param>
		/// <param name="value">The changed value.</param>
		public virtual void OnColumnChanged(UURIColumn column, object value)
		{
			if (!SuppressEntityEvents)
			{
				UURIEventHandler handler = ColumnChanged;
				if(handler != null)
				{
					handler(this, new UURIEventArgs(column, value));
				}
			
				// warn the parent list that i have changed
				OnEntityChanged();
			}
		} 
		#endregion
			
	} // End Class
	
	
	#region UURIEventArgs class
	/// <summary>
	/// Provides data for the ColumnChanging and ColumnChanged events.
	/// </summary>
	/// <remarks>
	/// The ColumnChanging and ColumnChanged events occur when a change is made to the value 
	/// of a property of a <see cref="UURI"/> object.
	/// </remarks>
	public class UURIEventArgs : System.EventArgs
	{
		private UURIColumn column;
		private object value;
		
		///<summary>
		/// Initalizes a new Instance of the UURIEventArgs class.
		///</summary>
		public UURIEventArgs(UURIColumn column)
		{
			this.column = column;
		}
		
		///<summary>
		/// Initalizes a new Instance of the UURIEventArgs class.
		///</summary>
		public UURIEventArgs(UURIColumn column, object value)
		{
			this.column = column;
			this.value = value;
		}
		
		///<summary>
		/// The UURIColumn that was modified, which has raised the event.
		///</summary>
		///<value cref="UURIColumn" />
		public UURIColumn Column { get { return this.column; } }
		
		/// <summary>
        /// Gets the current value of the column.
        /// </summary>
        /// <value>The current value of the column.</value>
		public object Value{ get { return this.value; } }

	}
	#endregion
	
	///<summary>
	/// Define a delegate for all UURI related events.
	///</summary>
	public delegate void UURIEventHandler(object sender, UURIEventArgs e);
	
	#region UURIComparer
		
	/// <summary>
	///	Strongly Typed IComparer
	/// </summary>
	public class UURIComparer : System.Collections.Generic.IComparer<UURI>
	{
		UURIColumn whichComparison;
		
		/// <summary>
        /// Initializes a new instance of the <see cref="T:UURIComparer"/> class.
        /// </summary>
		public UURIComparer()
        {            
        }               
        
        /// <summary>
        /// Initializes a new instance of the <see cref="T:UURIComparer"/> class.
        /// </summary>
        /// <param name="column">The column to sort on.</param>
        public UURIComparer(UURIColumn column)
        {
            this.whichComparison = column;
        }

		/// <summary>
        /// Determines whether the specified <c cref="UURI"/> instances are considered equal.
        /// </summary>
        /// <param name="a">The first <c cref="UURI"/> to compare.</param>
        /// <param name="b">The second <c>UURI</c> to compare.</param>
        /// <returns>true if objA is the same instance as objB or if both are null references or if objA.Equals(objB) returns true; otherwise, false.</returns>
        public bool Equals(UURI a, UURI b)
        {
            return this.Compare(a, b) == 0;
        }

		/// <summary>
        /// Gets the hash code of the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public int GetHashCode(UURI entity)
        {
            return entity.GetHashCode();
        }

        /// <summary>
        /// Performs a case-insensitive comparison of two objects of the same type and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="a">The first object to compare.</param>
        /// <param name="b">The second object to compare.</param>
        /// <returns></returns>
        public int Compare(UURI a, UURI b)
        {
        	EntityPropertyComparer entityPropertyComparer = new EntityPropertyComparer(this.whichComparison.ToString());
        	return entityPropertyComparer.Compare(a, b);
        }

		/// <summary>
        /// Gets or sets the column that will be used for comparison.
        /// </summary>
        /// <value>The comparison column.</value>
        public UURIColumn WhichComparison
        {
            get { return this.whichComparison; }
            set { this.whichComparison = value; }
        }
	}
	
	#endregion
	
	#region UURIKey Class

	/// <summary>
	/// Wraps the unique identifier values for the <see cref="UURI"/> object.
	/// </summary>
	[Serializable]
	[CLSCompliant(true)]
	public class UURIKey : EntityKeyBase
	{
		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the UURIKey class.
		/// </summary>
		public UURIKey()
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the UURIKey class.
		/// </summary>
		public UURIKey(UURIBase entity)
		{
			this.Entity = entity;

			#region Init Properties

			if ( entity != null )
			{
				this.pkid = entity.pkid;
			}

			#endregion
		}
		
		/// <summary>
		/// Initializes a new instance of the UURIKey class.
		/// </summary>
		public UURIKey(System.Int32 _pkid)
		{
			#region Init Properties

			this.pkid = _pkid;

			#endregion
		}
		
		#endregion Constructors

		#region Properties
		
		// member variable for the Entity property
		private UURIBase _entity;
		
		/// <summary>
		/// Gets or sets the Entity property.
		/// </summary>
		public UURIBase Entity
		{
			get { return _entity; }
			set { _entity = value; }
		}
		
		// member variable for the pkid property
		private System.Int32 _pkid;
		
		/// <summary>
		/// Gets or sets the pkid property.
		/// </summary>
		public System.Int32 pkid
		{
			get { return _pkid; }
			set
			{
				if ( this.Entity != null )
					this.Entity.pkid = value;
				
				_pkid = value;
			}
		}
		
		#endregion

		#region Methods
		
		/// <summary>
		/// Reads values from the supplied <see cref="IDictionary"/> object into
		/// properties of the current object.
		/// </summary>
		/// <param name="values">An <see cref="IDictionary"/> instance that contains
		/// the key/value pairs to be used as property values.</param>
		public override void Load(IDictionary values)
		{
			#region Init Properties

			if ( values != null )
			{
				pkid = ( values["pkid"] != null ) ? (System.Int32) EntityUtil.ChangeType(values["pkid"], typeof(System.Int32)) : (int)0;
			}

			#endregion
		}

		/// <summary>
		/// Creates a new <see cref="IDictionary"/> object and populates it
		/// with the property values of the current object.
		/// </summary>
		/// <returns>A collection of name/value pairs.</returns>
		public override IDictionary ToDictionary()
		{
			IDictionary values = new Hashtable();

			#region Init Dictionary

			values.Add("pkid", pkid);

			#endregion Init Dictionary

			return values;
		}
		
		///<summary>
		/// Returns a String that represents the current object.
		///</summary>
		public override string ToString()
		{
			return String.Format("pkid: {0}{1}",
								pkid,
								System.Environment.NewLine);
		}

		#endregion Methods
	}
	
	#endregion	

	#region UURIColumn Enum
	
	/// <summary>
	/// Enumerate the UURI columns.
	/// </summary>
	[Serializable]
	public enum UURIColumn : int
	{
		/// <summary>
		/// pkid : 
		/// </summary>
		[EnumTextValue("pkid")]
		[ColumnEnum("pkid", typeof(System.Int32), System.Data.DbType.Int32, true, true, false)]
		pkid = 1,
		/// <summary>
		/// FileName : 
		/// </summary>
		[EnumTextValue("FileName")]
		[ColumnEnum("FileName", typeof(System.String), System.Data.DbType.AnsiString, false, false, false, 2000)]
		FileName = 2
	}//End enum

	#endregion UURIColumn Enum

} // end namespace