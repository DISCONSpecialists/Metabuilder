#region Using directives

using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;

using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;

#endregion

namespace MetaBuilder.DataAccessLayer.Bases
{	
	///<summary>
	/// This class is the base class for any <see cref="AssociationTypeProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract partial class AssociationTypeProviderBaseCore : EntityProviderBase<MetaBuilder.BusinessLogic.AssociationType, MetaBuilder.BusinessLogic.AssociationTypeKey>
	{		
		#region Get from Many To Many Relationship Functions
		#endregion	
		
		#region Delete Methods

		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager">A <see cref="TransactionManager"/> object.</param>
		/// <param name="key">The unique identifier of the row to delete.</param>
		/// <returns>Returns true if operation suceeded.</returns>
		public override bool Delete(TransactionManager transactionManager, MetaBuilder.BusinessLogic.AssociationTypeKey key)
		{
			return Delete(transactionManager, key.pkid);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="_pkid">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(System.Int32 _pkid)
		{
			return Delete(null, _pkid);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_pkid">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(TransactionManager transactionManager, System.Int32 _pkid);		
		
		#endregion Delete Methods
		
		#region Get By Foreign Key Functions
		#endregion

		#region Get By Index Functions
		
		/// <summary>
		/// 	Gets a row from the DataSource based on its primary key.
		/// </summary>
		/// <param name="transactionManager">A <see cref="TransactionManager"/> object.</param>
		/// <param name="key">The unique identifier of the row to retrieve.</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <returns>Returns an instance of the Entity class.</returns>
		public override MetaBuilder.BusinessLogic.AssociationType Get(TransactionManager transactionManager, MetaBuilder.BusinessLogic.AssociationTypeKey key, int start, int pageLength)
		{
			return GetBypkid(transactionManager, key.pkid, start, pageLength);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key PK_MappingType index.
		/// </summary>
		/// <param name="_pkid"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.AssociationType"/> class.</returns>
		public MetaBuilder.BusinessLogic.AssociationType GetBypkid(System.Int32 _pkid)
		{
			int count = -1;
			return GetBypkid(null,_pkid, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_MappingType index.
		/// </summary>
		/// <param name="_pkid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.AssociationType"/> class.</returns>
		public MetaBuilder.BusinessLogic.AssociationType GetBypkid(System.Int32 _pkid, int start, int pageLength)
		{
			int count = -1;
			return GetBypkid(null, _pkid, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_MappingType index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_pkid"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.AssociationType"/> class.</returns>
		public MetaBuilder.BusinessLogic.AssociationType GetBypkid(TransactionManager transactionManager, System.Int32 _pkid)
		{
			int count = -1;
			return GetBypkid(transactionManager, _pkid, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_MappingType index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_pkid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.AssociationType"/> class.</returns>
		public MetaBuilder.BusinessLogic.AssociationType GetBypkid(TransactionManager transactionManager, System.Int32 _pkid, int start, int pageLength)
		{
			int count = -1;
			return GetBypkid(transactionManager, _pkid, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_MappingType index.
		/// </summary>
		/// <param name="_pkid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.AssociationType"/> class.</returns>
		public MetaBuilder.BusinessLogic.AssociationType GetBypkid(System.Int32 _pkid, int start, int pageLength, out int count)
		{
			return GetBypkid(null, _pkid, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_MappingType index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_pkid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.AssociationType"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.AssociationType GetBypkid(TransactionManager transactionManager, System.Int32 _pkid, int start, int pageLength, out int count);
						
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key uq_assocname index.
		/// </summary>
		/// <param name="_name"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.AssociationType"/> class.</returns>
		public MetaBuilder.BusinessLogic.AssociationType GetByName(System.String _name)
		{
			int count = -1;
			return GetByName(null,_name, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the uq_assocname index.
		/// </summary>
		/// <param name="_name"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.AssociationType"/> class.</returns>
		public MetaBuilder.BusinessLogic.AssociationType GetByName(System.String _name, int start, int pageLength)
		{
			int count = -1;
			return GetByName(null, _name, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the uq_assocname index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_name"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.AssociationType"/> class.</returns>
		public MetaBuilder.BusinessLogic.AssociationType GetByName(TransactionManager transactionManager, System.String _name)
		{
			int count = -1;
			return GetByName(transactionManager, _name, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the uq_assocname index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_name"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.AssociationType"/> class.</returns>
		public MetaBuilder.BusinessLogic.AssociationType GetByName(TransactionManager transactionManager, System.String _name, int start, int pageLength)
		{
			int count = -1;
			return GetByName(transactionManager, _name, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the uq_assocname index.
		/// </summary>
		/// <param name="_name"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.AssociationType"/> class.</returns>
		public MetaBuilder.BusinessLogic.AssociationType GetByName(System.String _name, int start, int pageLength, out int count)
		{
			return GetByName(null, _name, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the uq_assocname index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_name"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.AssociationType"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.AssociationType GetByName(TransactionManager transactionManager, System.String _name, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a TList&lt;AssociationType&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="TList&lt;AssociationType&gt;"/></returns>
		public static TList<AssociationType> Fill(IDataReader reader, TList<AssociationType> rows, int start, int pageLength)
		{
			NetTiersProvider currentProvider = DataRepository.Provider;
            bool useEntityFactory = currentProvider.UseEntityFactory;
            bool enableEntityTracking = currentProvider.EnableEntityTracking;
            LoadPolicy currentLoadPolicy = currentProvider.CurrentLoadPolicy;
			Type entityCreationFactoryType = currentProvider.EntityCreationalFactoryType;
			
			// advance to the starting row
			for (int i = 0; i < start; i++)
			{
				if (!reader.Read())
				return rows; // not enough rows, just return
			}
			for (int i = 0; i < pageLength; i++)
			{
				if (!reader.Read())
					break; // we are done
					
				string key = null;
				
				MetaBuilder.BusinessLogic.AssociationType c = null;
				if (useEntityFactory)
				{
					key = new System.Text.StringBuilder("AssociationType")
					.Append("|").Append((System.Int32)reader[((int)AssociationTypeColumn.pkid - 1)]).ToString();
					c = EntityManager.LocateOrCreate<AssociationType>(
					key.ToString(), // EntityTrackingKey
					"AssociationType",  //Creational Type
					entityCreationFactoryType,  //Factory used to create entity
					enableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.AssociationType();
				}
				
				if (!enableEntityTracking ||
					c.EntityState == EntityState.Added ||
					(enableEntityTracking &&
					
						(
							(currentLoadPolicy == LoadPolicy.PreserveChanges && c.EntityState == EntityState.Unchanged) ||
							(currentLoadPolicy == LoadPolicy.DiscardChanges && c.EntityState != EntityState.Unchanged)
						)
					))
				{
					c.SuppressEntityEvents = true;
					c.pkid = (System.Int32)reader[((int)AssociationTypeColumn.pkid - 1)];
					c.Name = (reader.IsDBNull(((int)AssociationTypeColumn.Name - 1)))?null:(System.String)reader[((int)AssociationTypeColumn.Name - 1)];
					c.IsTwoWay = (reader.IsDBNull(((int)AssociationTypeColumn.IsTwoWay - 1)))?null:(System.Boolean?)reader[((int)AssociationTypeColumn.IsTwoWay - 1)];
					c.LinkSpecification = (reader.IsDBNull(((int)AssociationTypeColumn.LinkSpecification - 1)))?null:(System.String)reader[((int)AssociationTypeColumn.LinkSpecification - 1)];
					c.EntityTrackingKey = key;
					c.AcceptChanges();
					c.SuppressEntityEvents = false;
				}
				rows.Add(c);
			}
		return rows;
		}		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.AssociationType"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.AssociationType"/> object to refresh.</param>
		public static void RefreshEntity(IDataReader reader, MetaBuilder.BusinessLogic.AssociationType entity)
		{
			if (!reader.Read()) return;
			
			entity.pkid = (System.Int32)reader[((int)AssociationTypeColumn.pkid - 1)];
			entity.Name = (reader.IsDBNull(((int)AssociationTypeColumn.Name - 1)))?null:(System.String)reader[((int)AssociationTypeColumn.Name - 1)];
			entity.IsTwoWay = (reader.IsDBNull(((int)AssociationTypeColumn.IsTwoWay - 1)))?null:(System.Boolean?)reader[((int)AssociationTypeColumn.IsTwoWay - 1)];
			entity.LinkSpecification = (reader.IsDBNull(((int)AssociationTypeColumn.LinkSpecification - 1)))?null:(System.String)reader[((int)AssociationTypeColumn.LinkSpecification - 1)];
			entity.AcceptChanges();
		}
		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.AssociationType"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.AssociationType"/> object.</param>
		public static void RefreshEntity(DataSet dataSet, MetaBuilder.BusinessLogic.AssociationType entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.pkid = (System.Int32)dataRow["pkid"];
			entity.Name = Convert.IsDBNull(dataRow["Name"]) ? null : (System.String)dataRow["Name"];
			entity.IsTwoWay = Convert.IsDBNull(dataRow["IsTwoWay"]) ? null : (System.Boolean?)dataRow["IsTwoWay"];
			entity.LinkSpecification = Convert.IsDBNull(dataRow["LinkSpecification"]) ? null : (System.String)dataRow["LinkSpecification"];
			entity.AcceptChanges();
		}
		#endregion 
		
		#region DeepLoad Methods
		/// <summary>
		/// Deep Loads the <see cref="IEntity"/> object with criteria based of the child 
		/// property collections only N Levels Deep based on the <see cref="DeepLoadType"/>.
		/// </summary>
		/// <remarks>
		/// Use this method with caution as it is possible to DeepLoad with Recursion and traverse an entire object graph.
		/// </remarks>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.AssociationType"/> object to load.</param>
		/// <param name="deep">Boolean. A flag that indicates whether to recursively save all Property Collection that are descendants of this instance. If True, saves the complete object graph below this object. If False, saves this object only. </param>
		/// <param name="deepLoadType">DeepLoadType Enumeration to Include/Exclude object property collections from Load.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.AssociationType Property Collection Type Array To Include or Exclude from Load</param>
		/// <param name="innerList">A collection of child types for easy access.</param>
	    /// <exception cref="ArgumentNullException">entity or childTypes is null.</exception>
	    /// <exception cref="ArgumentException">deepLoadType has invalid value.</exception>
		public override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.AssociationType entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, DeepSession innerList)
		{
			if(entity == null)
				return;
			
			//used to hold DeepLoad method delegates and fire after all the local children have been loaded.
			Dictionary<string, KeyValuePair<Delegate, object>> deepHandles = new Dictionary<string, KeyValuePair<Delegate, object>>();
			// Deep load child collections  - Call GetBypkid methods when available
			
			#region ClassAssociationCollection
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<ClassAssociation>|ClassAssociationCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'ClassAssociationCollection' loaded. key " + entity.EntityTrackingKey);
				#endif 

				entity.ClassAssociationCollection = DataRepository.ClassAssociationProvider.GetByAssociationTypeID(transactionManager, entity.pkid);

				if (deep && entity.ClassAssociationCollection.Count > 0)
				{
					deepHandles.Add("ClassAssociationCollection",
						new KeyValuePair<Delegate, object>((DeepLoadHandle<ClassAssociation>) DataRepository.ClassAssociationProvider.DeepLoad,
						new object[] { transactionManager, entity.ClassAssociationCollection, deep, deepLoadType, childTypes, innerList }
					));
				}
			}		
			#endregion 
			
			
			//Fire all DeepLoad Items
			foreach(KeyValuePair<Delegate, object> pair in deepHandles.Values)
		    {
                pair.Key.DynamicInvoke((object[])pair.Value);
		    }
			deepHandles = null;
		}
		
		#endregion 
		
		#region DeepSave Methods

		/// <summary>
		/// Deep Save the entire object graph of the MetaBuilder.BusinessLogic.AssociationType object with criteria based of the child 
		/// Type property array and DeepSaveType.
		/// </summary>
		/// <param name="transactionManager">The transaction manager.</param>
		/// <param name="entity">MetaBuilder.BusinessLogic.AssociationType instance</param>
		/// <param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.AssociationType Property Collection Type Array To Include or Exclude from Save</param>
		/// <param name="innerList">A Hashtable of child types for easy access.</param>
		public override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.AssociationType entity, DeepSaveType deepSaveType, System.Type[] childTypes, DeepSession innerList)
		{	
			if (entity == null)
				return false;
							
			#region Composite Parent Properties
			//Save Source Composite Properties, however, don't call deep save on them.  
			//So they only get saved a single level deep.
			#endregion Composite Parent Properties

			// Save Root Entity through Provider
			if (!entity.IsDeleted)
				this.Save(transactionManager, entity);
			
			//used to hold DeepSave method delegates and fire after all the local children have been saved.
			Dictionary<string, KeyValuePair<Delegate, object>> deepHandles = new Dictionary<string, KeyValuePair<Delegate, object>>();
	
			#region List<ClassAssociation>
				if (CanDeepSave(entity.ClassAssociationCollection, "List<ClassAssociation>|ClassAssociationCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(ClassAssociation child in entity.ClassAssociationCollection)
					{
						if(child.AssociationTypeIDSource != null)
						{
							child.AssociationTypeID = child.AssociationTypeIDSource.pkid;
						}
						else
						{
							child.AssociationTypeID = entity.pkid;
						}

					}

					if (entity.ClassAssociationCollection.Count > 0 || entity.ClassAssociationCollection.DeletedItems.Count > 0)
					{
						//DataRepository.ClassAssociationProvider.Save(transactionManager, entity.ClassAssociationCollection);
						
						deepHandles.Add("ClassAssociationCollection",
						new KeyValuePair<Delegate, object>((DeepSaveHandle< ClassAssociation >) DataRepository.ClassAssociationProvider.DeepSave,
							new object[] { transactionManager, entity.ClassAssociationCollection, deepSaveType, childTypes, innerList }
						));
					}
				} 
			#endregion 
				
			//Fire all DeepSave Items
			foreach(KeyValuePair<Delegate, object> pair in deepHandles.Values)
		    {
                pair.Key.DynamicInvoke((object[])pair.Value);
		    }
			
			// Save Root Entity through Provider, if not already saved in delete mode
			if (entity.IsDeleted)
				this.Save(transactionManager, entity);
				

			deepHandles = null;
						
			return true;
		}
		#endregion
	} // end class
	
	#region AssociationTypeChildEntityTypes
	
	///<summary>
	/// Enumeration used to expose the different child entity types 
	/// for child properties in <c>MetaBuilder.BusinessLogic.AssociationType</c>
	///</summary>
	public enum AssociationTypeChildEntityTypes
	{

		///<summary>
		/// Collection of <c>AssociationType</c> as OneToMany for ClassAssociationCollection
		///</summary>
		[ChildEntityType(typeof(TList<ClassAssociation>))]
		ClassAssociationCollection,
	}
	
	#endregion AssociationTypeChildEntityTypes
	
	#region AssociationTypeFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;AssociationTypeColumn&gt;"/> class
	/// that is used exclusively with a <see cref="AssociationType"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class AssociationTypeFilterBuilder : SqlFilterBuilder<AssociationTypeColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the AssociationTypeFilterBuilder class.
		/// </summary>
		public AssociationTypeFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the AssociationTypeFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public AssociationTypeFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the AssociationTypeFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public AssociationTypeFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion AssociationTypeFilterBuilder
	
	#region AssociationTypeParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;AssociationTypeColumn&gt;"/> class
	/// that is used exclusively with a <see cref="AssociationType"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class AssociationTypeParameterBuilder : ParameterizedSqlFilterBuilder<AssociationTypeColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the AssociationTypeParameterBuilder class.
		/// </summary>
		public AssociationTypeParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the AssociationTypeParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public AssociationTypeParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the AssociationTypeParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public AssociationTypeParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion AssociationTypeParameterBuilder
	
	#region AssociationTypeSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;AssociationTypeColumn&gt;"/> class
	/// that is used exclusively with a <see cref="AssociationType"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class AssociationTypeSortBuilder : SqlSortBuilder<AssociationTypeColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the AssociationTypeSqlSortBuilder class.
		/// </summary>
		public AssociationTypeSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion AssociationTypeSortBuilder
	
} // end namespace
