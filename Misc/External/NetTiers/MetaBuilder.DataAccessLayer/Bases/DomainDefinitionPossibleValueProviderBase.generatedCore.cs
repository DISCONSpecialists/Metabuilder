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
		/// <param name="_domainDefinitionID">. Primary Key.</param>
		/// <param name="_possibleValue">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(System.Int32 _domainDefinitionID, System.String _possibleValue)
		{
			return Delete(null, _domainDefinitionID, _possibleValue);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_domainDefinitionID">. Primary Key.</param>
		/// <param name="_possibleValue">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(TransactionManager transactionManager, System.Int32 _domainDefinitionID, System.String _possibleValue);		
		
		#endregion Delete Methods
		
		#region Get By Foreign Key Functions
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_DomainDefinitionPossibleValue_DomainDefinition key.
		///		FK_DomainDefinitionPossibleValue_DomainDefinition Description: 
		/// </summary>
		/// <param name="_domainDefinitionID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue objects.</returns>
		public TList<DomainDefinitionPossibleValue> GetByDomainDefinitionID(System.Int32 _domainDefinitionID)
		{
			int count = -1;
			return GetByDomainDefinitionID(_domainDefinitionID, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_DomainDefinitionPossibleValue_DomainDefinition key.
		///		FK_DomainDefinitionPossibleValue_DomainDefinition Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_domainDefinitionID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue objects.</returns>
		/// <remarks></remarks>
		public TList<DomainDefinitionPossibleValue> GetByDomainDefinitionID(TransactionManager transactionManager, System.Int32 _domainDefinitionID)
		{
			int count = -1;
			return GetByDomainDefinitionID(transactionManager, _domainDefinitionID, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_DomainDefinitionPossibleValue_DomainDefinition key.
		///		FK_DomainDefinitionPossibleValue_DomainDefinition Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_domainDefinitionID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue objects.</returns>
		public TList<DomainDefinitionPossibleValue> GetByDomainDefinitionID(TransactionManager transactionManager, System.Int32 _domainDefinitionID, int start, int pageLength)
		{
			int count = -1;
			return GetByDomainDefinitionID(transactionManager, _domainDefinitionID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_DomainDefinitionPossibleValue_DomainDefinition key.
		///		fK_DomainDefinitionPossibleValue_DomainDefinition Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_domainDefinitionID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue objects.</returns>
		public TList<DomainDefinitionPossibleValue> GetByDomainDefinitionID(System.Int32 _domainDefinitionID, int start, int pageLength)
		{
			int count =  -1;
			return GetByDomainDefinitionID(null, _domainDefinitionID, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_DomainDefinitionPossibleValue_DomainDefinition key.
		///		fK_DomainDefinitionPossibleValue_DomainDefinition Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_domainDefinitionID"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue objects.</returns>
		public TList<DomainDefinitionPossibleValue> GetByDomainDefinitionID(System.Int32 _domainDefinitionID, int start, int pageLength,out int count)
		{
			return GetByDomainDefinitionID(null, _domainDefinitionID, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_DomainDefinitionPossibleValue_DomainDefinition key.
		///		FK_DomainDefinitionPossibleValue_DomainDefinition Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_domainDefinitionID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue objects.</returns>
		public abstract TList<DomainDefinitionPossibleValue> GetByDomainDefinitionID(TransactionManager transactionManager, System.Int32 _domainDefinitionID, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_DomainDefinitionPossibleValue_URI key.
		///		FK_DomainDefinitionPossibleValue_URI Description: 
		/// </summary>
		/// <param name="_uRI_ID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue objects.</returns>
		public TList<DomainDefinitionPossibleValue> GetByURI_ID(System.Int32? _uRI_ID)
		{
			int count = -1;
			return GetByURI_ID(_uRI_ID, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_DomainDefinitionPossibleValue_URI key.
		///		FK_DomainDefinitionPossibleValue_URI Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_uRI_ID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue objects.</returns>
		/// <remarks></remarks>
		public TList<DomainDefinitionPossibleValue> GetByURI_ID(TransactionManager transactionManager, System.Int32? _uRI_ID)
		{
			int count = -1;
			return GetByURI_ID(transactionManager, _uRI_ID, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_DomainDefinitionPossibleValue_URI key.
		///		FK_DomainDefinitionPossibleValue_URI Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_uRI_ID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue objects.</returns>
		public TList<DomainDefinitionPossibleValue> GetByURI_ID(TransactionManager transactionManager, System.Int32? _uRI_ID, int start, int pageLength)
		{
			int count = -1;
			return GetByURI_ID(transactionManager, _uRI_ID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_DomainDefinitionPossibleValue_URI key.
		///		fK_DomainDefinitionPossibleValue_URI Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_uRI_ID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue objects.</returns>
		public TList<DomainDefinitionPossibleValue> GetByURI_ID(System.Int32? _uRI_ID, int start, int pageLength)
		{
			int count =  -1;
			return GetByURI_ID(null, _uRI_ID, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_DomainDefinitionPossibleValue_URI key.
		///		fK_DomainDefinitionPossibleValue_URI Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_uRI_ID"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue objects.</returns>
		public TList<DomainDefinitionPossibleValue> GetByURI_ID(System.Int32? _uRI_ID, int start, int pageLength,out int count)
		{
			return GetByURI_ID(null, _uRI_ID, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_DomainDefinitionPossibleValue_URI key.
		///		FK_DomainDefinitionPossibleValue_URI Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_uRI_ID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue objects.</returns>
		public abstract TList<DomainDefinitionPossibleValue> GetByURI_ID(TransactionManager transactionManager, System.Int32? _uRI_ID, int start, int pageLength, out int count);
		
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
		/// <param name="_domainDefinitionID"></param>
		/// <param name="_possibleValue"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue"/> class.</returns>
		public MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue GetByDomainDefinitionIDPossibleValue(System.Int32 _domainDefinitionID, System.String _possibleValue)
		{
			int count = -1;
			return GetByDomainDefinitionIDPossibleValue(null,_domainDefinitionID, _possibleValue, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_DomainDefinitionPossibleValue index.
		/// </summary>
		/// <param name="_domainDefinitionID"></param>
		/// <param name="_possibleValue"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue"/> class.</returns>
		public MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue GetByDomainDefinitionIDPossibleValue(System.Int32 _domainDefinitionID, System.String _possibleValue, int start, int pageLength)
		{
			int count = -1;
			return GetByDomainDefinitionIDPossibleValue(null, _domainDefinitionID, _possibleValue, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_DomainDefinitionPossibleValue index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_domainDefinitionID"></param>
		/// <param name="_possibleValue"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue"/> class.</returns>
		public MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue GetByDomainDefinitionIDPossibleValue(TransactionManager transactionManager, System.Int32 _domainDefinitionID, System.String _possibleValue)
		{
			int count = -1;
			return GetByDomainDefinitionIDPossibleValue(transactionManager, _domainDefinitionID, _possibleValue, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_DomainDefinitionPossibleValue index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_domainDefinitionID"></param>
		/// <param name="_possibleValue"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue"/> class.</returns>
		public MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue GetByDomainDefinitionIDPossibleValue(TransactionManager transactionManager, System.Int32 _domainDefinitionID, System.String _possibleValue, int start, int pageLength)
		{
			int count = -1;
			return GetByDomainDefinitionIDPossibleValue(transactionManager, _domainDefinitionID, _possibleValue, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_DomainDefinitionPossibleValue index.
		/// </summary>
		/// <param name="_domainDefinitionID"></param>
		/// <param name="_possibleValue"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue"/> class.</returns>
		public MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue GetByDomainDefinitionIDPossibleValue(System.Int32 _domainDefinitionID, System.String _possibleValue, int start, int pageLength, out int count)
		{
			return GetByDomainDefinitionIDPossibleValue(null, _domainDefinitionID, _possibleValue, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_DomainDefinitionPossibleValue index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_domainDefinitionID"></param>
		/// <param name="_possibleValue"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue GetByDomainDefinitionIDPossibleValue(TransactionManager transactionManager, System.Int32 _domainDefinitionID, System.String _possibleValue, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a TList&lt;DomainDefinitionPossibleValue&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="TList&lt;DomainDefinitionPossibleValue&gt;"/></returns>
		public static TList<DomainDefinitionPossibleValue> Fill(IDataReader reader, TList<DomainDefinitionPossibleValue> rows, int start, int pageLength)
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
				
				MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue c = null;
				if (useEntityFactory)
				{
					key = new System.Text.StringBuilder("DomainDefinitionPossibleValue")
					.Append("|").Append((System.Int32)reader[((int)DomainDefinitionPossibleValueColumn.DomainDefinitionID - 1)])
					.Append("|").Append((System.String)reader[((int)DomainDefinitionPossibleValueColumn.PossibleValue - 1)]).ToString();
					c = EntityManager.LocateOrCreate<DomainDefinitionPossibleValue>(
					key.ToString(), // EntityTrackingKey
					"DomainDefinitionPossibleValue",  //Creational Type
					entityCreationFactoryType,  //Factory used to create entity
					enableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue();
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
					c.DomainDefinitionID = (System.Int32)reader[((int)DomainDefinitionPossibleValueColumn.DomainDefinitionID - 1)];
					c.OriginalDomainDefinitionID = c.DomainDefinitionID;
					c.PossibleValue = (System.String)reader[((int)DomainDefinitionPossibleValueColumn.PossibleValue - 1)];
					c.OriginalPossibleValue = c.PossibleValue;
					c.Series = (System.Int32)reader[((int)DomainDefinitionPossibleValueColumn.Series - 1)];
					c.Description = (reader.IsDBNull(((int)DomainDefinitionPossibleValueColumn.Description - 1)))?null:(System.String)reader[((int)DomainDefinitionPossibleValueColumn.Description - 1)];
					c.IsActive = (reader.IsDBNull(((int)DomainDefinitionPossibleValueColumn.IsActive - 1)))?null:(System.Boolean?)reader[((int)DomainDefinitionPossibleValueColumn.IsActive - 1)];
					c.URI_ID = (reader.IsDBNull(((int)DomainDefinitionPossibleValueColumn.URI_ID - 1)))?null:(System.Int32?)reader[((int)DomainDefinitionPossibleValueColumn.URI_ID - 1)];
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
			
			entity.DomainDefinitionID = (System.Int32)reader[((int)DomainDefinitionPossibleValueColumn.DomainDefinitionID - 1)];
			entity.OriginalDomainDefinitionID = (System.Int32)reader["DomainDefinitionID"];
			entity.PossibleValue = (System.String)reader[((int)DomainDefinitionPossibleValueColumn.PossibleValue - 1)];
			entity.OriginalPossibleValue = (System.String)reader["PossibleValue"];
			entity.Series = (System.Int32)reader[((int)DomainDefinitionPossibleValueColumn.Series - 1)];
			entity.Description = (reader.IsDBNull(((int)DomainDefinitionPossibleValueColumn.Description - 1)))?null:(System.String)reader[((int)DomainDefinitionPossibleValueColumn.Description - 1)];
			entity.IsActive = (reader.IsDBNull(((int)DomainDefinitionPossibleValueColumn.IsActive - 1)))?null:(System.Boolean?)reader[((int)DomainDefinitionPossibleValueColumn.IsActive - 1)];
			entity.URI_ID = (reader.IsDBNull(((int)DomainDefinitionPossibleValueColumn.URI_ID - 1)))?null:(System.Int32?)reader[((int)DomainDefinitionPossibleValueColumn.URI_ID - 1)];
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
			entity.Description = Convert.IsDBNull(dataRow["Description"]) ? null : (System.String)dataRow["Description"];
			entity.IsActive = Convert.IsDBNull(dataRow["IsActive"]) ? null : (System.Boolean?)dataRow["IsActive"];
			entity.URI_ID = Convert.IsDBNull(dataRow["URI_ID"]) ? null : (System.Int32?)dataRow["URI_ID"];
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
		public override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, DeepSession innerList)
		{
			if(entity == null)
				return;

			#region DomainDefinitionIDSource	
			if (CanDeepLoad(entity, "DomainDefinition|DomainDefinitionIDSource", deepLoadType, innerList) 
				&& entity.DomainDefinitionIDSource == null)
			{
				object[] pkItems = new object[1];
				pkItems[0] = entity.DomainDefinitionID;
				DomainDefinition tmpEntity = EntityManager.LocateEntity<DomainDefinition>(EntityLocator.ConstructKeyFromPkItems(typeof(DomainDefinition), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.DomainDefinitionIDSource = tmpEntity;
				else
					entity.DomainDefinitionIDSource = DataRepository.DomainDefinitionProvider.GetBypkid(transactionManager, entity.DomainDefinitionID);		
				
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'DomainDefinitionIDSource' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.DomainDefinitionIDSource != null)
				{
					innerList.SkipChildren = true;
					DataRepository.DomainDefinitionProvider.DeepLoad(transactionManager, entity.DomainDefinitionIDSource, deep, deepLoadType, childTypes, innerList);
					innerList.SkipChildren = false;
				}
					
			}
			#endregion DomainDefinitionIDSource

			#region URI_IDSource	
			if (CanDeepLoad(entity, "UURI|URI_IDSource", deepLoadType, innerList) 
				&& entity.URI_IDSource == null)
			{
				object[] pkItems = new object[1];
				pkItems[0] = (entity.URI_ID ?? (int)0);
				UURI tmpEntity = EntityManager.LocateEntity<UURI>(EntityLocator.ConstructKeyFromPkItems(typeof(UURI), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.URI_IDSource = tmpEntity;
				else
					entity.URI_IDSource = DataRepository.UURIProvider.GetBypkid(transactionManager, (entity.URI_ID ?? (int)0));		
				
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'URI_IDSource' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.URI_IDSource != null)
				{
					innerList.SkipChildren = true;
					DataRepository.UURIProvider.DeepLoad(transactionManager, entity.URI_IDSource, deep, deepLoadType, childTypes, innerList);
					innerList.SkipChildren = false;
				}
					
			}
			#endregion URI_IDSource
			
			//used to hold DeepLoad method delegates and fire after all the local children have been loaded.
			Dictionary<string, KeyValuePair<Delegate, object>> deepHandles = new Dictionary<string, KeyValuePair<Delegate, object>>();
			
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
		/// Deep Save the entire object graph of the MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue object with criteria based of the child 
		/// Type property array and DeepSaveType.
		/// </summary>
		/// <param name="transactionManager">The transaction manager.</param>
		/// <param name="entity">MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue instance</param>
		/// <param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue Property Collection Type Array To Include or Exclude from Save</param>
		/// <param name="innerList">A Hashtable of child types for easy access.</param>
		public override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.DomainDefinitionPossibleValue entity, DeepSaveType deepSaveType, System.Type[] childTypes, DeepSession innerList)
		{	
			if (entity == null)
				return false;
							
			#region Composite Parent Properties
			//Save Source Composite Properties, however, don't call deep save on them.  
			//So they only get saved a single level deep.
			
			#region DomainDefinitionIDSource
			if (CanDeepSave(entity, "DomainDefinition|DomainDefinitionIDSource", deepSaveType, innerList) 
				&& entity.DomainDefinitionIDSource != null)
			{
				DataRepository.DomainDefinitionProvider.Save(transactionManager, entity.DomainDefinitionIDSource);
				entity.DomainDefinitionID = entity.DomainDefinitionIDSource.pkid;
			}
			#endregion 
			
			#region URI_IDSource
			if (CanDeepSave(entity, "UURI|URI_IDSource", deepSaveType, innerList) 
				&& entity.URI_IDSource != null)
			{
				DataRepository.UURIProvider.Save(transactionManager, entity.URI_IDSource);
				entity.URI_ID = entity.URI_IDSource.pkid;
			}
			#endregion 
			#endregion Composite Parent Properties

			// Save Root Entity through Provider
			if (!entity.IsDeleted)
				this.Save(transactionManager, entity);
			
			//used to hold DeepSave method delegates and fire after all the local children have been saved.
			Dictionary<string, KeyValuePair<Delegate, object>> deepHandles = new Dictionary<string, KeyValuePair<Delegate, object>>();
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
			
		///<summary>
		/// Composite Property for <c>UURI</c> at URI_IDSource
		///</summary>
		[ChildEntityType(typeof(UURI))]
		UURI,
		}
	
	#endregion DomainDefinitionPossibleValueChildEntityTypes
	
	#region DomainDefinitionPossibleValueFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;DomainDefinitionPossibleValueColumn&gt;"/> class
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
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;DomainDefinitionPossibleValueColumn&gt;"/> class
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
	
	#region DomainDefinitionPossibleValueSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;DomainDefinitionPossibleValueColumn&gt;"/> class
	/// that is used exclusively with a <see cref="DomainDefinitionPossibleValue"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class DomainDefinitionPossibleValueSortBuilder : SqlSortBuilder<DomainDefinitionPossibleValueColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the DomainDefinitionPossibleValueSqlSortBuilder class.
		/// </summary>
		public DomainDefinitionPossibleValueSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion DomainDefinitionPossibleValueSortBuilder
	
} // end namespace
