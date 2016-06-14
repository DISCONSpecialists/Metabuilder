#region Using directives

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
	/// This class is the base class for any <see cref="DomainDefinitionProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract partial class DomainDefinitionProviderBaseCore : EntityProviderBase<MetaBuilder.BusinessLogic.DomainDefinition, MetaBuilder.BusinessLogic.DomainDefinitionKey>
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
		public override bool Delete(TransactionManager transactionManager, MetaBuilder.BusinessLogic.DomainDefinitionKey key)
		{
			return Delete(transactionManager, key.Pkid);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="pkid">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(System.Int32 pkid)
		{
			return Delete(null, pkid);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="pkid">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(TransactionManager transactionManager, System.Int32 pkid);		
		
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
		public override MetaBuilder.BusinessLogic.DomainDefinition Get(TransactionManager transactionManager, MetaBuilder.BusinessLogic.DomainDefinitionKey key, int start, int pageLength)
		{
			return GetByPkid(transactionManager, key.Pkid, start, pageLength);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key PK_DomainDefinition index.
		/// </summary>
		/// <param name="pkid"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.DomainDefinition"/> class.</returns>
		public MetaBuilder.BusinessLogic.DomainDefinition GetByPkid(System.Int32 pkid)
		{
			int count = -1;
			return GetByPkid(null,pkid, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_DomainDefinition index.
		/// </summary>
		/// <param name="pkid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.DomainDefinition"/> class.</returns>
		public MetaBuilder.BusinessLogic.DomainDefinition GetByPkid(System.Int32 pkid, int start, int pageLength)
		{
			int count = -1;
			return GetByPkid(null, pkid, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_DomainDefinition index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="pkid"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.DomainDefinition"/> class.</returns>
		public MetaBuilder.BusinessLogic.DomainDefinition GetByPkid(TransactionManager transactionManager, System.Int32 pkid)
		{
			int count = -1;
			return GetByPkid(transactionManager, pkid, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_DomainDefinition index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="pkid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.DomainDefinition"/> class.</returns>
		public MetaBuilder.BusinessLogic.DomainDefinition GetByPkid(TransactionManager transactionManager, System.Int32 pkid, int start, int pageLength)
		{
			int count = -1;
			return GetByPkid(transactionManager, pkid, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_DomainDefinition index.
		/// </summary>
		/// <param name="pkid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.DomainDefinition"/> class.</returns>
		public MetaBuilder.BusinessLogic.DomainDefinition GetByPkid(System.Int32 pkid, int start, int pageLength, out int count)
		{
			return GetByPkid(null, pkid, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_DomainDefinition index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="pkid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.DomainDefinition"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.DomainDefinition GetByPkid(TransactionManager transactionManager, System.Int32 pkid, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a MetaBuilder.BusinessLogic.TList&lt;DomainDefinition&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="MetaBuilder.BusinessLogic.TList&lt;DomainDefinition&gt;"/></returns>
		public static MetaBuilder.BusinessLogic.TList<DomainDefinition> Fill(IDataReader reader, MetaBuilder.BusinessLogic.TList<DomainDefinition> rows, int start, int pageLength)
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
				
				MetaBuilder.BusinessLogic.DomainDefinition c = null;
				if (DataRepository.Provider.UseEntityFactory)
				{
					key = @"DomainDefinition" 
							+ (reader.IsDBNull(reader.GetOrdinal("pkid"))?(int)0:(System.Int32)reader["pkid"]).ToString();

					c = EntityManager.LocateOrCreate<DomainDefinition>(
						key.ToString(), // EntityTrackingKey 
						"DomainDefinition",  //Creational Type
						DataRepository.Provider.EntityCreationalFactoryType,  //Factory used to create entity
						DataRepository.Provider.EnableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.DomainDefinition();
				}
				
				if (!DataRepository.Provider.EnableEntityTracking || c.EntityState == EntityState.Added)
                {
					c.SuppressEntityEvents = true;
					c.Pkid = (System.Int32)reader["pkid"];
					c.Name = (System.String)reader["Name"];
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
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.DomainDefinition"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.DomainDefinition"/> object to refresh.</param>
		public static void RefreshEntity(IDataReader reader, MetaBuilder.BusinessLogic.DomainDefinition entity)
		{
			if (!reader.Read()) return;
			
			entity.Pkid = (System.Int32)reader["pkid"];
			entity.Name = (System.String)reader["Name"];
			entity.IsActive = (reader.IsDBNull(reader.GetOrdinal("IsActive")))?null:(System.Boolean?)reader["IsActive"];
			entity.AcceptChanges();
		}
		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.DomainDefinition"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.DomainDefinition"/> object.</param>
		public static void RefreshEntity(DataSet dataSet, MetaBuilder.BusinessLogic.DomainDefinition entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.Pkid = (System.Int32)dataRow["pkid"];
			entity.Name = (System.String)dataRow["Name"];
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
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.DomainDefinition"/> object to load.</param>
		/// <param name="deep">Boolean. A flag that indicates whether to recursively save all Property Collection that are descendants of this instance. If True, saves the complete object graph below this object. If False, saves this object only. </param>
		/// <param name="deepLoadType">DeepLoadType Enumeration to Include/Exclude object property collections from Load.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.DomainDefinition Property Collection Type Array To Include or Exclude from Load</param>
		/// <param name="innerList">A collection of child types for easy access.</param>
	    /// <exception cref="ArgumentNullException">entity or childTypes is null.</exception>
	    /// <exception cref="ArgumentException">deepLoadType has invalid value.</exception>
		internal override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.DomainDefinition entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, ChildEntityTypesList innerList)
		{
			if(entity == null)
				return;
			
			// Load Entity through Provider
			// Deep load child collections  - Call GetBypkid methods when available
			
			#region DomainDefinitionPossibleValueCollection
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<DomainDefinitionPossibleValue>", "DomainDefinitionPossibleValueCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'DomainDefinitionPossibleValueCollection' loaded.");
				#endif 

				entity.DomainDefinitionPossibleValueCollection = DataRepository.DomainDefinitionPossibleValueProvider.GetByDomainDefinitionID(transactionManager, entity.Pkid);

				if (deep && entity.DomainDefinitionPossibleValueCollection.Count > 0)
				{
					DataRepository.DomainDefinitionPossibleValueProvider.DeepLoad(transactionManager, entity.DomainDefinitionPossibleValueCollection, deep, deepLoadType, childTypes, innerList);
				}
			}		
			#endregion 
		}
		
		#endregion 
		
		#region DeepSave Methods

		/// <summary>
		/// Deep Save the entire object graph of the MetaBuilder.BusinessLogic.DomainDefinition object with criteria based of the child 
		/// Type property array and DeepSaveType.
		/// </summary>
		/// <param name="transactionManager">The transaction manager.</param>
		/// <param name="entity">MetaBuilder.BusinessLogic.DomainDefinition instance</param>
		/// <param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.DomainDefinition Property Collection Type Array To Include or Exclude from Save</param>
		/// <param name="innerList">A Hashtable of child types for easy access.</param>
		internal override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.DomainDefinition entity, DeepSaveType deepSaveType, System.Type[] childTypes, ChildEntityTypesList innerList)
		{	
			if (entity == null)
				return false;
							
			#region Composite Parent Properties
			//Save Source Composite Properties, however, don't call deep save on them.  
			//So they only get saved a single level deep.
			#endregion Composite Parent Properties

			// Save Root Entity through Provider
			this.Save(transactionManager, entity);
			
			



			#region List<DomainDefinitionPossibleValue>
				if (CanDeepSave(entity, "List<DomainDefinitionPossibleValue>", "DomainDefinitionPossibleValueCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(DomainDefinitionPossibleValue child in entity.DomainDefinitionPossibleValueCollection)
					{
						child.DomainDefinitionID = entity.Pkid;
					}
				
				if (entity.DomainDefinitionPossibleValueCollection.Count > 0 || entity.DomainDefinitionPossibleValueCollection.DeletedItems.Count > 0)
					DataRepository.DomainDefinitionPossibleValueProvider.DeepSave(transactionManager, entity.DomainDefinitionPossibleValueCollection, deepSaveType, childTypes, innerList);
				} 
			#endregion 
				

						
			return true;
		}
		#endregion
	} // end class
	
	#region DomainDefinitionChildEntityTypes
	
	///<summary>
	/// Enumeration used to expose the different child entity types 
	/// for child properties in <c>MetaBuilder.BusinessLogic.DomainDefinition</c>
	///</summary>
	public enum DomainDefinitionChildEntityTypes
	{

		///<summary>
		/// Collection of <c>DomainDefinition</c> as OneToMany for DomainDefinitionPossibleValueCollection
		///</summary>
		[ChildEntityType(typeof(TList<DomainDefinitionPossibleValue>))]
		DomainDefinitionPossibleValueCollection,
	}
	
	#endregion DomainDefinitionChildEntityTypes
	
	#region DomainDefinitionFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="DomainDefinition"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class DomainDefinitionFilterBuilder : SqlFilterBuilder<DomainDefinitionColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the DomainDefinitionFilterBuilder class.
		/// </summary>
		public DomainDefinitionFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the DomainDefinitionFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public DomainDefinitionFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the DomainDefinitionFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public DomainDefinitionFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion DomainDefinitionFilterBuilder
	
	#region DomainDefinitionParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="DomainDefinition"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class DomainDefinitionParameterBuilder : ParameterizedSqlFilterBuilder<DomainDefinitionColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the DomainDefinitionParameterBuilder class.
		/// </summary>
		public DomainDefinitionParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the DomainDefinitionParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public DomainDefinitionParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the DomainDefinitionParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public DomainDefinitionParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion DomainDefinitionParameterBuilder
} // end namespace
