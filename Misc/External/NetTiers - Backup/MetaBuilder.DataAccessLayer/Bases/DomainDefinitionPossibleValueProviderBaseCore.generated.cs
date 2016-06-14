﻿#region Using directives

using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;

using System.Diagnostics;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;

#endregion

namespace MetaBuilder.DataAccessLayer.Bases
{	
	///<summary>
	/// This class is the base class for any <see cref="DomainDefinitionPossibleValueProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract partial class DomainDefinitionPossibleValueProviderBaseCore : EntityProviderBase<MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue, MetaBuilder.BusinessLogic.DomainDefinitionPossibleValueKey>
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
		public override bool Delete(TransactionManager transactionManager, MetaBuilder.BusinessLogic.DomainDefinitionPossibleValueKey key)
		{
			return Delete(transactionManager, key.DomainDefinitionID, key.PossibleValue);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="domainDefinitionID">. Primary Key.</param>
		/// <param name="possibleValue">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(System.Int32 domainDefinitionID, System.String possibleValue)
		{
			return Delete(null, domainDefinitionID, possibleValue);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="domainDefinitionID">. Primary Key.</param>
		/// <param name="possibleValue">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(TransactionManager transactionManager, System.Int32 domainDefinitionID, System.String possibleValue);		
		
		#endregion Delete Methods
		
		#region Get By Foreign Key Functions
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_DomainDefinitionPossibleValue_DomainDefinition key.
		///		FK_DomainDefinitionPossibleValue_DomainDefinition Description: 
		/// </summary>
		/// <param name="domainDefinitionID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue objects.</returns>
		public MetaBuilder.BusinessLogic.TList<DomainDefinitionPossibleValue> GetByDomainDefinitionID(System.Int32 domainDefinitionID)
		{
			int count = -1;
			return GetByDomainDefinitionID(domainDefinitionID, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_DomainDefinitionPossibleValue_DomainDefinition key.
		///		FK_DomainDefinitionPossibleValue_DomainDefinition Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="domainDefinitionID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<DomainDefinitionPossibleValue> GetByDomainDefinitionID(TransactionManager transactionManager, System.Int32 domainDefinitionID)
		{
			int count = -1;
			return GetByDomainDefinitionID(transactionManager, domainDefinitionID, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_DomainDefinitionPossibleValue_DomainDefinition key.
		///		FK_DomainDefinitionPossibleValue_DomainDefinition Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="domainDefinitionID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue objects.</returns>
		public MetaBuilder.BusinessLogic.TList<DomainDefinitionPossibleValue> GetByDomainDefinitionID(TransactionManager transactionManager, System.Int32 domainDefinitionID, int start, int pageLength)
		{
			int count = -1;
			return GetByDomainDefinitionID(transactionManager, domainDefinitionID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_DomainDefinitionPossibleValue_DomainDefinition key.
		///		fKDomainDefinitionPossibleValueDomainDefinition Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="domainDefinitionID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue objects.</returns>
		public MetaBuilder.BusinessLogic.TList<DomainDefinitionPossibleValue> GetByDomainDefinitionID(System.Int32 domainDefinitionID, int start, int pageLength)
		{
			int count =  -1;
			return GetByDomainDefinitionID(null, domainDefinitionID, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_DomainDefinitionPossibleValue_DomainDefinition key.
		///		fKDomainDefinitionPossibleValueDomainDefinition Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="domainDefinitionID"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue objects.</returns>
		public MetaBuilder.BusinessLogic.TList<DomainDefinitionPossibleValue> GetByDomainDefinitionID(System.Int32 domainDefinitionID, int start, int pageLength,out int count)
		{
			return GetByDomainDefinitionID(null, domainDefinitionID, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_DomainDefinitionPossibleValue_DomainDefinition key.
		///		FK_DomainDefinitionPossibleValue_DomainDefinition Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="domainDefinitionID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<DomainDefinitionPossibleValue> GetByDomainDefinitionID(TransactionManager transactionManager, System.Int32 domainDefinitionID, int start, int pageLength, out int count);
		
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
		public override MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue Get(TransactionManager transactionManager, MetaBuilder.BusinessLogic.DomainDefinitionPossibleValueKey key, int start, int pageLength)
		{
			return GetByDomainDefinitionIDPossibleValue(transactionManager, key.DomainDefinitionID, key.PossibleValue, start, pageLength);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key PK_DomainDefinitionPossibleValue index.
		/// </summary>
		/// <param name="domainDefinitionID"></param>
		/// <param name="possibleValue"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue"/> class.</returns>
		public MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue GetByDomainDefinitionIDPossibleValue(System.Int32 domainDefinitionID, System.String possibleValue)
		{
			int count = -1;
			return GetByDomainDefinitionIDPossibleValue(null,domainDefinitionID, possibleValue, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_DomainDefinitionPossibleValue index.
		/// </summary>
		/// <param name="domainDefinitionID"></param>
		/// <param name="possibleValue"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue"/> class.</returns>
		public MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue GetByDomainDefinitionIDPossibleValue(System.Int32 domainDefinitionID, System.String possibleValue, int start, int pageLength)
		{
			int count = -1;
			return GetByDomainDefinitionIDPossibleValue(null, domainDefinitionID, possibleValue, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_DomainDefinitionPossibleValue index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="domainDefinitionID"></param>
		/// <param name="possibleValue"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue"/> class.</returns>
		public MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue GetByDomainDefinitionIDPossibleValue(TransactionManager transactionManager, System.Int32 domainDefinitionID, System.String possibleValue)
		{
			int count = -1;
			return GetByDomainDefinitionIDPossibleValue(transactionManager, domainDefinitionID, possibleValue, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_DomainDefinitionPossibleValue index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="domainDefinitionID"></param>
		/// <param name="possibleValue"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue"/> class.</returns>
		public MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue GetByDomainDefinitionIDPossibleValue(TransactionManager transactionManager, System.Int32 domainDefinitionID, System.String possibleValue, int start, int pageLength)
		{
			int count = -1;
			return GetByDomainDefinitionIDPossibleValue(transactionManager, domainDefinitionID, possibleValue, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_DomainDefinitionPossibleValue index.
		/// </summary>
		/// <param name="domainDefinitionID"></param>
		/// <param name="possibleValue"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue"/> class.</returns>
		public MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue GetByDomainDefinitionIDPossibleValue(System.Int32 domainDefinitionID, System.String possibleValue, int start, int pageLength, out int count)
		{
			return GetByDomainDefinitionIDPossibleValue(null, domainDefinitionID, possibleValue, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_DomainDefinitionPossibleValue index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="domainDefinitionID"></param>
		/// <param name="possibleValue"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue GetByDomainDefinitionIDPossibleValue(TransactionManager transactionManager, System.Int32 domainDefinitionID, System.String possibleValue, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a MetaBuilder.BusinessLogic.TList&lt;DomainDefinitionPossibleValue&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="MetaBuilder.BusinessLogic.TList&lt;DomainDefinitionPossibleValue&gt;"/></returns>
		public static MetaBuilder.BusinessLogic.TList<DomainDefinitionPossibleValue> Fill(IDataReader reader, MetaBuilder.BusinessLogic.TList<DomainDefinitionPossibleValue> rows, int start, int pageLength)
		{
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
				
				MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue c = null;
				if (DataRepository.Provider.UseEntityFactory)
				{
					key = @"DomainDefinitionPossibleValue" 
							+ (reader.IsDBNull(reader.GetOrdinal("DomainDefinitionID"))?(int)0:(System.Int32)reader["DomainDefinitionID"]).ToString()
							+ (reader.IsDBNull(reader.GetOrdinal("PossibleValue"))?string.Empty:(System.String)reader["PossibleValue"]).ToString();

					c = EntityManager.LocateOrCreate<DomainDefinitionPossibleValue>(
						key.ToString(), // EntityTrackingKey 
						"DomainDefinitionPossibleValue",  //Creational Type
						DataRepository.Provider.EntityCreationalFactoryType,  //Factory used to create entity
						DataRepository.Provider.EnableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue();
				}
				
				if (!DataRepository.Provider.EnableEntityTracking || c.EntityState == EntityState.Added)
                {
					c.SuppressEntityEvents = true;
					c.DomainDefinitionID = (System.Int32)reader["DomainDefinitionID"];
					c.OriginalDomainDefinitionID = c.DomainDefinitionID; //(reader.IsDBNull(reader.GetOrdinal("DomainDefinitionID")))?(int)0:(System.Int32)reader["DomainDefinitionID"];
					c.PossibleValue = (System.String)reader["PossibleValue"];
					c.OriginalPossibleValue = c.PossibleValue; //(reader.IsDBNull(reader.GetOrdinal("PossibleValue")))?string.Empty:(System.String)reader["PossibleValue"];
					c.Series = (System.Int32)reader["Series"];
					c.Description = (reader.IsDBNull(reader.GetOrdinal("Description")))?null:(System.String)reader["Description"];
					c.IsActive = (reader.IsDBNull(reader.GetOrdinal("IsActive")))?null:(System.Boolean?)reader["IsActive"];
					c.EntityTrackingKey = key;
					c.AcceptChanges();
					c.SuppressEntityEvents = false;
				}
				rows.Add(c);
			}
			return rows;
		}
		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue"/> object to refresh.</param>
		public static void RefreshEntity(IDataReader reader, MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue entity)
		{
			if (!reader.Read()) return;
			
			entity.DomainDefinitionID = (System.Int32)reader["DomainDefinitionID"];
			entity.OriginalDomainDefinitionID = (System.Int32)reader["DomainDefinitionID"];
			entity.PossibleValue = (System.String)reader["PossibleValue"];
			entity.OriginalPossibleValue = (System.String)reader["PossibleValue"];
			entity.Series = (System.Int32)reader["Series"];
			entity.Description = (reader.IsDBNull(reader.GetOrdinal("Description")))?null:(System.String)reader["Description"];
			entity.IsActive = (reader.IsDBNull(reader.GetOrdinal("IsActive")))?null:(System.Boolean?)reader["IsActive"];
			entity.AcceptChanges();
		}
		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue"/> object.</param>
		public static void RefreshEntity(DataSet dataSet, MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.DomainDefinitionID = (System.Int32)dataRow["DomainDefinitionID"];
			entity.OriginalDomainDefinitionID = (System.Int32)dataRow["DomainDefinitionID"];
			entity.PossibleValue = (System.String)dataRow["PossibleValue"];
			entity.OriginalPossibleValue = (System.String)dataRow["PossibleValue"];
			entity.Series = (System.Int32)dataRow["Series"];
			entity.Description = (Convert.IsDBNull(dataRow["Description"]))?null:(System.String)dataRow["Description"];
			entity.IsActive = (Convert.IsDBNull(dataRow["IsActive"]))?null:(System.Boolean?)dataRow["IsActive"];
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
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue"/> object to load.</param>
		/// <param name="deep">Boolean. A flag that indicates whether to recursively save all Property Collection that are descendants of this instance. If True, saves the complete object graph below this object. If False, saves this object only. </param>
		/// <param name="deepLoadType">DeepLoadType Enumeration to Include/Exclude object property collections from Load.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue Property Collection Type Array To Include or Exclude from Load</param>
		/// <param name="innerList">A collection of child types for easy access.</param>
	    /// <exception cref="ArgumentNullException">entity or childTypes is null.</exception>
	    /// <exception cref="ArgumentException">deepLoadType has invalid value.</exception>
		internal override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, ChildEntityTypesList innerList)
		{
			if(entity == null)
				return;

			#region DomainDefinitionIDSource	
			if (CanDeepLoad(entity, "DomainDefinition", "DomainDefinitionIDSource", deepLoadType, innerList) 
				&& entity.DomainDefinitionIDSource == null)
			{
				object[] pkItems = new object[1];
				pkItems[0] = entity.DomainDefinitionID;
				DomainDefinition tmpEntity = EntityManager.LocateEntity<DomainDefinition>(EntityLocator.ConstructKeyFromPkItems(typeof(DomainDefinition), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.DomainDefinitionIDSource = tmpEntity;
				else
					entity.DomainDefinitionIDSource = DataRepository.DomainDefinitionProvider.GetByPkid(entity.DomainDefinitionID);
			
				if (deep && entity.DomainDefinitionIDSource != null)
				{
					DataRepository.DomainDefinitionProvider.DeepLoad(transactionManager, entity.DomainDefinitionIDSource, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion DomainDefinitionIDSource
			
			// Load Entity through Provider
		}
		
		#endregion 
		
		#region DeepSave Methods

		/// <summary>
		/// Deep Save the entire object graph of the MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue object with criteria based of the child 
		/// Type property array and DeepSaveType.
		/// </summary>
		/// <param name="transactionManager">The transaction manager.</param>
		/// <param name="entity">MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue instance</param>
		/// <param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue Property Collection Type Array To Include or Exclude from Save</param>
		/// <param name="innerList">A Hashtable of child types for easy access.</param>
		internal override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue entity, DeepSaveType deepSaveType, System.Type[] childTypes, ChildEntityTypesList innerList)
		{	
			if (entity == null)
				return false;
							
			#region Composite Parent Properties
			//Save Source Composite Properties, however, don't call deep save on them.  
			//So they only get saved a single level deep.
			
			#region DomainDefinitionIDSource
			if (CanDeepSave(entity, "DomainDefinition", "DomainDefinitionIDSource", deepSaveType, innerList) 
				&& entity.DomainDefinitionIDSource != null)
			{
				DataRepository.DomainDefinitionProvider.Save(transactionManager, entity.DomainDefinitionIDSource);
				entity.DomainDefinitionID = entity.DomainDefinitionIDSource.Pkid;
			}
			#endregion 
			#endregion Composite Parent Properties

			// Save Root Entity through Provider
			this.Save(transactionManager, entity);
			
			
						
			return true;
		}
		#endregion
	} // end class
	
	#region DomainDefinitionPossibleValueChildEntityTypes
	
	///<summary>
	/// Enumeration used to expose the different child entity types 
	/// for child properties in <c>MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue</c>
	///</summary>
	public enum DomainDefinitionPossibleValueChildEntityTypes
	{
		
		///<summary>
		/// Composite Property for <c>DomainDefinition</c> at DomainDefinitionIDSource
		///</summary>
		[ChildEntityType(typeof(DomainDefinition))]
		DomainDefinition,
		}
	
	#endregion DomainDefinitionPossibleValueChildEntityTypes
	
	#region DomainDefinitionPossibleValueFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="DomainDefinitionPossibleValue"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class DomainDefinitionPossibleValueFilterBuilder : SqlFilterBuilder<DomainDefinitionPossibleValueColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the DomainDefinitionPossibleValueFilterBuilder class.
		/// </summary>
		public DomainDefinitionPossibleValueFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the DomainDefinitionPossibleValueFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public DomainDefinitionPossibleValueFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the DomainDefinitionPossibleValueFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public DomainDefinitionPossibleValueFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion DomainDefinitionPossibleValueFilterBuilder
	
	#region DomainDefinitionPossibleValueParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="DomainDefinitionPossibleValue"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class DomainDefinitionPossibleValueParameterBuilder : ParameterizedSqlFilterBuilder<DomainDefinitionPossibleValueColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the DomainDefinitionPossibleValueParameterBuilder class.
		/// </summary>
		public DomainDefinitionPossibleValueParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the DomainDefinitionPossibleValueParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public DomainDefinitionPossibleValueParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the DomainDefinitionPossibleValueParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public DomainDefinitionPossibleValueParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion DomainDefinitionPossibleValueParameterBuilder
} // end namespace
