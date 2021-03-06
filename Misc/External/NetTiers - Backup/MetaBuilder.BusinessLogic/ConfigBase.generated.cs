﻿
/*
	File generated by NetTiers templates [www.nettiers.com]
	Important: Do not modify this file. Edit the file Config.cs instead.
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
	/// An object representation of the 'Config' table. [No description found the database]	
	///</summary>
	[Serializable]


	[DataObject, CLSCompliant(true)]
	public abstract partial class ConfigBase : EntityBase, IConfig, IEntityId<ConfigKey>, System.IComparable, System.ICloneable, ICloneableEx, IEditableObject, IComponent, INotifyPropertyChanged
	{		
		#region Variable Declarations
		
		/// <summary>
		///  Hold the inner data of the entity.
		/// </summary>
		private ConfigEntityData entityData;
		
		/// <summary>
		/// 	Hold the original data of the entity, as loaded from the repository.
		/// </summary>
		private ConfigEntityData _originalData;
		
		/// <summary>
		/// 	Hold a backup of the inner data of the entity.
		/// </summary>
		private ConfigEntityData backupData; 
		
		/// <summary>
		/// 	Key used if Tracking is Enabled for the <see cref="EntityLocator" />.
		/// </summary>
		private string entityTrackingKey;
		
		/// <summary>
		/// 	Hold the parent TList&lt;entity&gt; in which this entity maybe contained.
		/// </summary>
		/// <remark>Mostly used for databinding</remark>
		[NonSerialized]
		private TList<Config> parentCollection;
		
		private bool inTxn = false;
		
		/// <summary>
		/// Occurs when a value is being changed for the specified column.
		/// </summary>
		[field:NonSerialized]
		public event ConfigEventHandler ColumnChanging;		
		
		/// <summary>
		/// Occurs after a value has been changed for the specified column.
		/// </summary>
		[field:NonSerialized]
		public event ConfigEventHandler ColumnChanged;
		
		#endregion Variable Declarations
		
		#region Constructors
		///<summary>
		/// Creates a new <see cref="ConfigBase"/> instance.
		///</summary>
		public ConfigBase()
		{
			this.entityData = new ConfigEntityData();
			this.backupData = null;
		}		
		
		///<summary>
		/// Creates a new <see cref="ConfigBase"/> instance.
		///</summary>
		///<param name="_configName"></param>
		///<param name="_configValue"></param>
		public ConfigBase(System.String _configName, System.String _configValue)
		{
			this.entityData = new ConfigEntityData();
			this.backupData = null;

			this.ConfigName = _configName;
			this.ConfigValue = _configValue;
		}
		
		///<summary>
		/// A simple factory method to create a new <see cref="Config"/> instance.
		///</summary>
		///<param name="_configName"></param>
		///<param name="_configValue"></param>
		public static Config CreateConfig(System.String _configName, System.String _configValue)
		{
			Config newConfig = new Config();
			newConfig.ConfigName = _configName;
			newConfig.ConfigValue = _configValue;
			return newConfig;
		}
				
		#endregion Constructors
			
		#region Properties	
		
		#region Data Properties		
		/// <summary>
		/// 	Gets or sets the ConfigName property. 
		///		
		/// </summary>
		/// <value>This type is varchar.</value>
		/// <remarks>
		/// This property can not be set to null. 
		/// </remarks>
		/// <exception cref="ArgumentNullException">If you attempt to set to null.</exception>




		[DescriptionAttribute(@""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		[DataObjectField(true, false, false, 50)]
		public virtual System.String ConfigName
		{
			get
			{
				return this.entityData.ConfigName; 
			}
			
			set
			{
				if (this.entityData.ConfigName == value)
					return;
					
				OnColumnChanging(ConfigColumn.ConfigName, this.entityData.ConfigName);
				this.entityData.ConfigName = value;
				this.EntityId.ConfigName = value;
				if (this.EntityState == EntityState.Unchanged)
					this.EntityState = EntityState.Changed;
				OnColumnChanged(ConfigColumn.ConfigName, this.entityData.ConfigName);
				OnPropertyChanged("ConfigName");
			}
		}
		
		/// <summary>
		/// 	Get the original value of the ConfigName property.
		///		
		/// </summary>
		/// <remarks>This is the original value of the ConfigName property.</remarks>
		/// <value>This type is varchar</value>
		[BrowsableAttribute(false)/*, XmlIgnoreAttribute()*/]
		public  virtual System.String OriginalConfigName
		{
			get { return this.entityData.OriginalConfigName; }
			set { this.entityData.OriginalConfigName = value; }
		}
		
		/// <summary>
		/// 	Gets or sets the ConfigValue property. 
		///		
		/// </summary>
		/// <value>This type is varchar.</value>
		/// <remarks>
		/// This property can not be set to null. 
		/// </remarks>
		/// <exception cref="ArgumentNullException">If you attempt to set to null.</exception>




		[DescriptionAttribute(@""), System.ComponentModel.Bindable( System.ComponentModel.BindableSupport.Yes)]
		[DataObjectField(false, false, false, 50)]
		public virtual System.String ConfigValue
		{
			get
			{
				return this.entityData.ConfigValue; 
			}
			
			set
			{
				if (this.entityData.ConfigValue == value)
					return;
					
				OnColumnChanging(ConfigColumn.ConfigValue, this.entityData.ConfigValue);
				this.entityData.ConfigValue = value;
				if (this.EntityState == EntityState.Unchanged)
					this.EntityState = EntityState.Changed;
				OnColumnChanged(ConfigColumn.ConfigValue, this.entityData.ConfigValue);
				OnPropertyChanged("ConfigValue");
			}
		}
		
		#endregion Data Properties		

		#region Source Foreign Key Property
				
		#endregion
		
		#region Children Collections
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
				new ValidationRuleArgs("ConfigName", "Config Name"));
			ValidationRules.AddRule( CommonRules.StringMaxLength, 
				new CommonRules.MaxLengthRuleArgs("ConfigName", "Config Name", 50));
			ValidationRules.AddRule( CommonRules.NotNull,
				new ValidationRuleArgs("ConfigValue", "Config Value"));
			ValidationRules.AddRule( CommonRules.StringMaxLength, 
				new CommonRules.MaxLengthRuleArgs("ConfigValue", "Config Value", 50));
		}
   		#endregion
		
		#region Table Meta Data
		/// <summary>
		///		The name of the underlying database table.
		/// </summary>
		[BrowsableAttribute(false), XmlIgnoreAttribute()]
		public override string TableName
		{
			get { return "Config"; }
		}
		
		/// <summary>
		///		The name of the underlying database table's columns.
		/// </summary>
		[BrowsableAttribute(false), XmlIgnoreAttribute()]
		public override string[] TableColumns
		{
			get
			{
				return new string[] {"ConfigName", "ConfigValue"};
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
	            this.backupData = this.entityData.Clone() as ConfigEntityData;
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
						this.parentCollection.Remove( (Config) this ) ;
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
	            this.parentCollection = value as TList<Config>;
	        }
	    }
	    
	    /// <summary>
        /// Called when the entity is changed.
        /// </summary>
	    private void OnEntityChanged() 
	    {
	        if (!SuppressEntityEvents && !inTxn && this.parentCollection != null) 
	        {
	            this.parentCollection.EntityChanged(this as Config);
	        }
	    }


		#endregion
		
		#region ICloneable Members
		///<summary>
		///  Returns a Typed Config Entity 
		///</summary>
		protected virtual Config Copy(IDictionary existingCopies)
		{
			if (existingCopies == null)
			{
				// This is the root of the tree to be copied!
				existingCopies = new Hashtable();
			}

			//shallow copy entity
			Config copy = new Config();
			existingCopies.Add(this, copy);
			copy.SuppressEntityEvents = true;
				copy.ConfigName = this.ConfigName;
					copy.OriginalConfigName = this.OriginalConfigName;
				copy.ConfigValue = this.ConfigValue;
			
		
			copy.EntityState = this.EntityState;
			copy.SuppressEntityEvents = false;
			return copy;
		}		
		
		
		
		///<summary>
		///  Returns a Typed Config Entity 
		///</summary>
		public virtual Config Copy()
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
		///  Returns a Typed Config Entity which is a deep copy of the current entity.
		///</summary>
		public virtual Config DeepCopy()
		{
			return EntityHelper.Clone<Config>(this as Config);	
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
				this.entityData = this._originalData.Clone() as ConfigEntityData;
			}
			else
			{
				//Since this had no _originalData, then just reset the entityData with a new one.  entityData cannot be null.
				this.entityData = new ConfigEntityData();
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
			this._originalData = this.entityData.Clone() as ConfigEntityData;
		}
		
		#region Comparision with original data
		
		/// <summary>
		/// Determines whether the property value has changed from the original data.
		/// </summary>
		/// <param name="column">The column.</param>
		/// <returns>
		/// 	<c>true</c> if the property value has changed; otherwise, <c>false</c>.
		/// </returns>
		public bool IsPropertyChanged(ConfigColumn column)
		{
			switch(column)
			{
					case ConfigColumn.ConfigName:
					return entityData.ConfigName != _originalData.ConfigName;
					case ConfigColumn.ConfigValue:
					return entityData.ConfigValue != _originalData.ConfigValue;
			
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
			return 	IsPropertyChanged(EntityHelper.GetEnumValue< ConfigColumn >(columnName));
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
			result = result || entityData.ConfigName != _originalData.ConfigName;
			result = result || entityData.ConfigValue != _originalData.ConfigValue;
			return result;
		}	
		
		///<summary>
		///  Returns a Config Entity with the original data.
		///</summary>
		public Config GetOriginalEntity()
		{
			if (_originalData != null)
				return CreateConfig(
				_originalData.ConfigName,
				_originalData.ConfigValue
				);
				
			return (Config)this.Clone();
		}
		#endregion
	
	#region Value Semantics Instance Equality
        ///<summary>
        /// Returns a value indicating whether this instance is equal to a specified object using value semantics.
        ///</summary>
        ///<param name="Object1">An object to compare to this instance.</param>
        ///<returns>true if Object1 is a <see cref="ConfigBase"/> and has the same value as this instance; otherwise, false.</returns>
        public override bool Equals(object Object1)
        {
			// Cast exception if Object1 is null or DbNull
			if (Object1 != null && Object1 != DBNull.Value && Object1 is ConfigBase)
				return ValueEquals(this, (ConfigBase)Object1);
			else
				return false;
        }

        /// <summary>
		/// Serves as a hash function for a particular type, suitable for use in hashing algorithms and data structures like a hash table.
        /// Provides a hash function that is appropriate for <see cref="ConfigBase"/> class 
        /// and that ensures a better distribution in the hash table
        /// </summary>
        /// <returns>number (hash code) that corresponds to the value of an object</returns>
        public override int GetHashCode()
        {
			return this.ConfigName.GetHashCode() ^ 
					this.ConfigValue.GetHashCode();
        }
		
		///<summary>
		/// Returns a value indicating whether this instance is equal to a specified object using value semantics.
		///</summary>
		///<param name="toObject">An object to compare to this instance.</param>
		///<returns>true if toObject is a <see cref="ConfigBase"/> and has the same value as this instance; otherwise, false.</returns>
		public virtual bool Equals(ConfigBase toObject)
		{
			if (toObject == null)
				return false;
			return ValueEquals(this, toObject);
		}
		#endregion
		
		///<summary>
		/// Determines whether the specified <see cref="ConfigBase"/> instances are considered equal using value semantics.
		///</summary>
		///<param name="Object1">The first <see cref="ConfigBase"/> to compare.</param>
		///<param name="Object2">The second <see cref="ConfigBase"/> to compare. </param>
		///<returns>true if Object1 is the same instance as Object2 or if both are null references or if objA.Equals(objB) returns true; otherwise, false.</returns>
		public static bool ValueEquals(ConfigBase Object1, ConfigBase Object2)
		{
			// both are null
			if (Object1 == null && Object2 == null)
				return true;

			// one or the other is null, but not both
			if (Object1 == null ^ Object2 == null)
				return false;
				
			bool equal = true;
			if (Object1.ConfigName != Object2.ConfigName)
				equal = false;
			if (Object1.ConfigValue != Object2.ConfigValue)
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
			//return this. GetPropertyName(SourceTable.PrimaryKey.MemberColumns[0]) .CompareTo(((ConfigBase)obj).GetPropertyName(SourceTable.PrimaryKey.MemberColumns[0]));
		}
		
		/*
		// static method to get a Comparer object
        public static ConfigComparer GetComparer()
        {
            return new ConfigComparer();
        }
        */

        // Comparer delegates back to Config
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
        public int CompareTo(Config rhs, ConfigColumn which)
        {
            switch (which)
            {
            	
            	
            	case ConfigColumn.ConfigName:
            		return this.ConfigName.CompareTo(rhs.ConfigName);
            		
            		                 
            	
            	
            	case ConfigColumn.ConfigValue:
            		return this.ConfigValue.CompareTo(rhs.ConfigValue);
            		
            		                 
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
				
		#region IEntityKey<ConfigKey> Members
		
		// member variable for the EntityId property
		private ConfigKey _entityId;

		/// <summary>
		/// Gets or sets the EntityId property.
		/// </summary>
		[XmlIgnore]
		public virtual ConfigKey EntityId
		{
			get
			{
				if ( _entityId == null )
				{
					_entityId = new ConfigKey(this);
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
		[BrowsableAttribute(false) ]
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
					entityTrackingKey = new System.Text.StringBuilder("Config")
					.Append("|").Append( this.ConfigName.ToString()).ToString();
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
				"{3}{2}- ConfigName: {0}{2}- ConfigValue: {1}{2}{4}", 
				this.ConfigName,
				this.ConfigValue,
				System.Environment.NewLine, 
				this.GetType(),
				this.Error.Length == 0 ? string.Empty : string.Format("- Error: {0}\n",this.Error));
		}
		
		#endregion ToString Method
		
		#region Inner data class
		
	/// <summary>
	///		The data structure representation of the 'Config' table.
	/// </summary>
	/// <remarks>
	/// 	This struct is generated by a tool and should never be modified.
	/// </remarks>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Serializable]
	internal protected class ConfigEntityData : ICloneable, ICloneableEx
	{
		#region Variable Declarations
		private EntityState currentEntityState = EntityState.Added;
		
		#region Primary key(s)
		/// <summary>			
		/// ConfigName : 
		/// </summary>
		/// <remarks>Member of the primary key of the underlying table "Config"</remarks>
		public System.String ConfigName;
			
		/// <summary>
		/// keep a copy of the original so it can be used for editable primary keys.
		/// </summary>
		public System.String OriginalConfigName;
		
		#endregion
		
		#region Non Primary key(s)
		
		
		/// <summary>
		/// ConfigValue : 
		/// </summary>
		public System.String		  ConfigValue = string.Empty;
		#endregion
			
		#region Source Foreign Key Property
				
		#endregion
		#endregion Variable Declarations
	
		#region Data Properties

		#endregion Data Properties
		
		#region Clone Method

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public Object Clone()
		{
			ConfigEntityData _tmp = new ConfigEntityData();
						
			_tmp.ConfigName = this.ConfigName;
			_tmp.OriginalConfigName = this.OriginalConfigName;
			
			_tmp.ConfigValue = this.ConfigValue;
			
			#region Source Parent Composite Entities
			#endregion
		
			#region Child Collections
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
				
			ConfigEntityData _tmp = new ConfigEntityData();
						
			_tmp.ConfigName = this.ConfigName;
			_tmp.OriginalConfigName = this.OriginalConfigName;
			
			_tmp.ConfigValue = this.ConfigValue;
			
			#region Source Parent Composite Entities
			#endregion
		
			#region Child Collections
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
		[BrowsableAttribute(false)]
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
		/// <param name="column">The <see cref="ConfigColumn"/> which has raised the event.</param>
		public virtual void OnColumnChanging(ConfigColumn column)
		{
			OnColumnChanging(column, null);
			return;
		}
		
		/// <summary>
		/// Raises the <see cref="ColumnChanged" /> event.
		/// </summary>
		/// <param name="column">The <see cref="ConfigColumn"/> which has raised the event.</param>
		public virtual void OnColumnChanged(ConfigColumn column)
		{
			OnColumnChanged(column, null);
			return;
		} 
		
		
		/// <summary>
		/// Raises the <see cref="ColumnChanging" /> event.
		/// </summary>
		/// <param name="column">The <see cref="ConfigColumn"/> which has raised the event.</param>
		/// <param name="value">The changed value.</param>
		public virtual void OnColumnChanging(ConfigColumn column, object value)
		{
			if(IsEntityTracked && EntityState != EntityState.Added && !EntityManager.TrackChangedEntities)
				EntityManager.StopTracking(entityTrackingKey);
				
			if (!SuppressEntityEvents)
			{
				ConfigEventHandler handler = ColumnChanging;
				if(handler != null)
				{
					handler(this, new ConfigEventArgs(column, value));
				}
			}
		}
		
		/// <summary>
		/// Raises the <see cref="ColumnChanged" /> event.
		/// </summary>
		/// <param name="column">The <see cref="ConfigColumn"/> which has raised the event.</param>
		/// <param name="value">The changed value.</param>
		public virtual void OnColumnChanged(ConfigColumn column, object value)
		{
			if (!SuppressEntityEvents)
			{
				ConfigEventHandler handler = ColumnChanged;
				if(handler != null)
				{
					handler(this, new ConfigEventArgs(column, value));
				}
			
				// warn the parent list that i have changed
				OnEntityChanged();
			}
		} 
		#endregion
			
	} // End Class
	
	
	#region ConfigEventArgs class
	/// <summary>
	/// Provides data for the ColumnChanging and ColumnChanged events.
	/// </summary>
	/// <remarks>
	/// The ColumnChanging and ColumnChanged events occur when a change is made to the value 
	/// of a property of a <see cref="Config"/> object.
	/// </remarks>
	public class ConfigEventArgs : System.EventArgs
	{
		private ConfigColumn column;
		private object value;
		
		///<summary>
		/// Initalizes a new Instance of the ConfigEventArgs class.
		///</summary>
		public ConfigEventArgs(ConfigColumn column)
		{
			this.column = column;
		}
		
		///<summary>
		/// Initalizes a new Instance of the ConfigEventArgs class.
		///</summary>
		public ConfigEventArgs(ConfigColumn column, object value)
		{
			this.column = column;
			this.value = value;
		}
		
		///<summary>
		/// The ConfigColumn that was modified, which has raised the event.
		///</summary>
		///<value cref="ConfigColumn" />
		public ConfigColumn Column { get { return this.column; } }
		
		/// <summary>
        /// Gets the current value of the column.
        /// </summary>
        /// <value>The current value of the column.</value>
		public object Value{ get { return this.value; } }

	}
	#endregion
	
	///<summary>
	/// Define a delegate for all Config related events.
	///</summary>
	public delegate void ConfigEventHandler(object sender, ConfigEventArgs e);
	
	#region ConfigComparer
		
	/// <summary>
	///	Strongly Typed IComparer
	/// </summary>
	public class ConfigComparer : System.Collections.Generic.IComparer<Config>
	{
		ConfigColumn whichComparison;
		
		/// <summary>
        /// Initializes a new instance of the <see cref="T:ConfigComparer"/> class.
        /// </summary>
		public ConfigComparer()
        {            
        }               
        
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ConfigComparer"/> class.
        /// </summary>
        /// <param name="column">The column to sort on.</param>
        public ConfigComparer(ConfigColumn column)
        {
            this.whichComparison = column;
        }

		/// <summary>
        /// Determines whether the specified <c cref="Config"/> instances are considered equal.
        /// </summary>
        /// <param name="a">The first <c cref="Config"/> to compare.</param>
        /// <param name="b">The second <c>Config</c> to compare.</param>
        /// <returns>true if objA is the same instance as objB or if both are null references or if objA.Equals(objB) returns true; otherwise, false.</returns>
        public bool Equals(Config a, Config b)
        {
            return this.Compare(a, b) == 0;
        }

		/// <summary>
        /// Gets the hash code of the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public int GetHashCode(Config entity)
        {
            return entity.GetHashCode();
        }

        /// <summary>
        /// Performs a case-insensitive comparison of two objects of the same type and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="a">The first object to compare.</param>
        /// <param name="b">The second object to compare.</param>
        /// <returns></returns>
        public int Compare(Config a, Config b)
        {
        	EntityPropertyComparer entityPropertyComparer = new EntityPropertyComparer(this.whichComparison.ToString());
        	return entityPropertyComparer.Compare(a, b);
        }

		/// <summary>
        /// Gets or sets the column that will be used for comparison.
        /// </summary>
        /// <value>The comparison column.</value>
        public ConfigColumn WhichComparison
        {
            get { return this.whichComparison; }
            set { this.whichComparison = value; }
        }
	}
	
	#endregion
	
	#region ConfigKey Class

	/// <summary>
	/// Wraps the unique identifier values for the <see cref="Config"/> object.
	/// </summary>
	[Serializable]
	[CLSCompliant(true)]
	public class ConfigKey : EntityKeyBase
	{
		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the ConfigKey class.
		/// </summary>
		public ConfigKey()
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the ConfigKey class.
		/// </summary>
		public ConfigKey(ConfigBase entity)
		{
			this.Entity = entity;

			#region Init Properties

			if ( entity != null )
			{
				this.ConfigName = entity.ConfigName;
			}

			#endregion
		}
		
		/// <summary>
		/// Initializes a new instance of the ConfigKey class.
		/// </summary>
		public ConfigKey(System.String _configName)
		{
			#region Init Properties

			this.ConfigName = _configName;

			#endregion
		}
		
		#endregion Constructors

		#region Properties
		
		// member variable for the Entity property
		private ConfigBase _entity;
		
		/// <summary>
		/// Gets or sets the Entity property.
		/// </summary>
		public ConfigBase Entity
		{
			get { return _entity; }
			set { _entity = value; }
		}
		
		// member variable for the ConfigName property
		private System.String _configName;
		
		/// <summary>
		/// Gets or sets the ConfigName property.
		/// </summary>
		public System.String ConfigName
		{
			get { return _configName; }
			set
			{
				if ( this.Entity != null )
					this.Entity.ConfigName = value;
				
				_configName = value;
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
				ConfigName = ( values["ConfigName"] != null ) ? (System.String) EntityUtil.ChangeType(values["ConfigName"], typeof(System.String)) : string.Empty;
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

			values.Add("ConfigName", ConfigName);

			#endregion Init Dictionary

			return values;
		}
		
		///<summary>
		/// Returns a String that represents the current object.
		///</summary>
		public override string ToString()
		{
			return String.Format("ConfigName: {0}{1}",
								ConfigName,
								System.Environment.NewLine);
		}

		#endregion Methods
	}
	
	#endregion	

	#region ConfigColumn Enum
	
	/// <summary>
	/// Enumerate the Config columns.
	/// </summary>
	[Serializable]
	public enum ConfigColumn : int
	{
		/// <summary>
		/// ConfigName : 
		/// </summary>
		[EnumTextValue("ConfigName")]
		[ColumnEnum("ConfigName", typeof(System.String), System.Data.DbType.AnsiString, true, false, false, 50)]
		ConfigName = 1,
		/// <summary>
		/// ConfigValue : 
		/// </summary>
		[EnumTextValue("ConfigValue")]
		[ColumnEnum("ConfigValue", typeof(System.String), System.Data.DbType.AnsiString, false, false, false, 50)]
		ConfigValue = 2
	}//End enum

	#endregion ConfigColumn Enum

} // end namespace
