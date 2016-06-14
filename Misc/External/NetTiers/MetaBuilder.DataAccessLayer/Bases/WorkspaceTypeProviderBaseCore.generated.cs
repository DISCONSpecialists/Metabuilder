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
	/// This class is the base class for any <see cref="WorkspaceTypeProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract partial class WorkspaceTypeProviderBaseCore : EntityProviderBase<MetaBuilder.BusinessLogic.WorkspaceType, MetaBuilder.BusinessLogic.WorkspaceTypeKey>
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
		public override bool Delete(TransactionManager transactionManager, MetaBuilder.BusinessLogic.WorkspaceTypeKey key)
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
		public override MetaBuilder.BusinessLogic.WorkspaceType Get(TransactionManager transactionManager, MetaBuilder.BusinessLogic.WorkspaceTypeKey key, int start, int pageLength)
		{
			return GetByPkid(transactionManager, key.Pkid, start, pageLength);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key PK_WorkspaceType index.
		/// </summary>
		/// <param name="pkid"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.WorkspaceType"/> class.</returns>
		public MetaBuilder.BusinessLogic.WorkspaceType GetByPkid(System.Int32 pkid)
		{
			int count = -1;
			return GetByPkid(null,pkid, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_WorkspaceType index.
		/// </summary>
		/// <param name="pkid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.WorkspaceType"/> class.</returns>
		public MetaBuilder.BusinessLogic.WorkspaceType GetByPkid(System.Int32 pkid, int start, int pageLength)
		{
			int count = -1;
			return GetByPkid(null, pkid, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_WorkspaceType index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="pkid"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.WorkspaceType"/> class.</returns>
		public MetaBuilder.BusinessLogic.WorkspaceType GetByPkid(TransactionManager transactionManager, System.Int32 pkid)
		{
			int count = -1;
			return GetByPkid(transactionManager, pkid, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_WorkspaceType index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="pkid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.WorkspaceType"/> class.</returns>
		public MetaBuilder.BusinessLogic.WorkspaceType GetByPkid(TransactionManager transactionManager, System.Int32 pkid, int start, int pageLength)
		{
			int count = -1;
			return GetByPkid(transactionManager, pkid, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_WorkspaceType index.
		/// </summary>
		/// <param name="pkid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.WorkspaceType"/> class.</returns>
		public MetaBuilder.BusinessLogic.WorkspaceType GetByPkid(System.Int32 pkid, int start, int pageLength, out int count)
		{
			return GetByPkid(null, pkid, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_WorkspaceType index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="pkid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.WorkspaceType"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.WorkspaceType GetByPkid(TransactionManager transactionManager, System.Int32 pkid, int start, int pageLength, out int count);
						
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key IX_WorkspaceType index.
		/// </summary>
		/// <param name="description"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.WorkspaceType"/> class.</returns>
		public MetaBuilder.BusinessLogic.WorkspaceType GetByDescription(System.String description)
		{
			int count = -1;
			return GetByDescription(null,description, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the IX_WorkspaceType index.
		/// </summary>
		/// <param name="description"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.WorkspaceType"/> class.</returns>
		public MetaBuilder.BusinessLogic.WorkspaceType GetByDescription(System.String description, int start, int pageLength)
		{
			int count = -1;
			return GetByDescription(null, description, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the IX_WorkspaceType index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="description"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.WorkspaceType"/> class.</returns>
		public MetaBuilder.BusinessLogic.WorkspaceType GetByDescription(TransactionManager transactionManager, System.String description)
		{
			int count = -1;
			return GetByDescription(transactionManager, description, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the IX_WorkspaceType index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="description"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.WorkspaceType"/> class.</returns>
		public MetaBuilder.BusinessLogic.WorkspaceType GetByDescription(TransactionManager transactionManager, System.String description, int start, int pageLength)
		{
			int count = -1;
			return GetByDescription(transactionManager, description, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the IX_WorkspaceType index.
		/// </summary>
		/// <param name="description"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.WorkspaceType"/> class.</returns>
		public MetaBuilder.BusinessLogic.WorkspaceType GetByDescription(System.String description, int start, int pageLength, out int count)
		{
			return GetByDescription(null, description, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the IX_WorkspaceType index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="description"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.WorkspaceType"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.WorkspaceType GetByDescription(TransactionManager transactionManager, System.String description, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a MetaBuilder.BusinessLogic.TList&lt;WorkspaceType&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="MetaBuilder.BusinessLogic.TList&lt;WorkspaceType&gt;"/></returns>
		public static MetaBuilder.BusinessLogic.TList<WorkspaceType> Fill(IDataReader reader, MetaBuilder.BusinessLogic.TList<WorkspaceType> rows, int start, int pageLength)
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
				
				MetaBuilder.BusinessLogic.WorkspaceType c = null;
				if (DataRepository.Provider.UseEntityFactory)
				{
					key = @"WorkspaceType" 
							+ (reader.IsDBNull(reader.GetOrdinal("pkid"))?(int)0:(System.Int32)reader["pkid"]).ToString();

					c = EntityManager.LocateOrCreate<WorkspaceType>(
						key.ToString(), // EntityTrackingKey 
						"WorkspaceType",  //Creational Type
						DataRepository.Provider.EntityCreationalFactoryType,  //Factory used to create entity
						DataRepository.Provider.EnableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.WorkspaceType();
				}
				
				if (!DataRepository.Provider.EnableEntityTracking || c.EntityState == EntityState.Added)
                {
					c.SuppressEntityEvents = true;
					c.Pkid = (System.Int32)reader["pkid"];
					c.Description = (System.String)reader["Description"];
					c.EntityTrackingKey = key;
					c.AcceptChanges();
					c.SuppressEntityEvents = false;
				}
				rows.Add(c);
			}
			return rows;
		}
		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.WorkspaceType"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.WorkspaceType"/> object to refresh.</param>
		public static void RefreshEntity(IDataReader reader, MetaBuilder.BusinessLogic.WorkspaceType entity)
		{
			if (!reader.Read()) return;
			
			entity.Pkid = (System.Int32)reader["pkid"];
			entity.Description = (System.String)reader["Description"];
			entity.AcceptChanges();
		}
		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.WorkspaceType"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.WorkspaceType"/> object.</param>
		public static void RefreshEntity(DataSet dataSet, MetaBuilder.BusinessLogic.WorkspaceType entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.Pkid = (System.Int32)dataRow["pkid"];
			entity.Description = (System.String)dataRow["Description"];
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
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.WorkspaceType"/> object to load.</param>
		/// <param name="deep">Boolean. A flag that indicates whether to recursively save all Property Collection that are descendants of this instance. If True, saves the complete object graph below this object. If False, saves this object only. </param>
		/// <param name="deepLoadType">DeepLoadType Enumeration to Include/Exclude object property collections from Load.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.WorkspaceType Property Collection Type Array To Include or Exclude from Load</param>
		/// <param name="innerList">A collection of child types for easy access.</param>
	    /// <exception cref="ArgumentNullException">entity or childTypes is null.</exception>
	    /// <exception cref="ArgumentException">deepLoadType has invalid value.</exception>
		internal override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.WorkspaceType entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, ChildEntityTypesList innerList)
		{
			if(entity == null)
				return;
			
			// Load Entity through Provider
			// Deep load child collections  - Call GetBypkid methods when available
			
			#region WorkspaceCollection
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<Workspace>", "WorkspaceCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'WorkspaceCollection' loaded.");
				#endif 

				entity.WorkspaceCollection = DataRepository.WorkspaceProvider.GetByWorkspaceTypeID(transactionManager, entity.Pkid);

				if (deep && entity.WorkspaceCollection.Count > 0)
				{
					DataRepository.WorkspaceProvider.DeepLoad(transactionManager, entity.WorkspaceCollection, deep, deepLoadType, childTypes, innerList);
				}
			}		
			#endregion 
		}
		
		#endregion 
		
		#region DeepSave Methods

		/// <summary>
		/// Deep Save the entire object graph of the MetaBuilder.BusinessLogic.WorkspaceType object with criteria based of the child 
		/// Type property array and DeepSaveType.
		/// </summary>
		/// <param name="transactionManager">The transaction manager.</param>
		/// <param name="entity">MetaBuilder.BusinessLogic.WorkspaceType instance</param>
		/// <param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.WorkspaceType Property Collection Type Array To Include or Exclude from Save</param>
		/// <param name="innerList">A Hashtable of child types for easy access.</param>
		internal override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.WorkspaceType entity, DeepSaveType deepSaveType, System.Type[] childTypes, ChildEntityTypesList innerList)
		{	
			if (entity == null)
				return false;
							
			#region Composite Parent Properties
			//Save Source Composite Properties, however, don't call deep save on them.  
			//So they only get saved a single level deep.
			#endregion Composite Parent Properties

			// Save Root Entity through Provider
			this.Save(transactionManager, entity);
			
			



			#region List<Workspace>
				if (CanDeepSave(entity, "List<Workspace>", "WorkspaceCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(Workspace child in entity.WorkspaceCollection)
					{
						child.WorkspaceTypeID = entity.Pkid;
					}
				
				if (entity.WorkspaceCollection.Count > 0 || entity.WorkspaceCollection.DeletedItems.Count > 0)
					DataRepository.WorkspaceProvider.DeepSave(transactionManager, entity.WorkspaceCollection, deepSaveType, childTypes, innerList);
				} 
			#endregion 
				

						
			return true;
		}
		#endregion
	} // end class
	
	#region WorkspaceTypeChildEntityTypes
	
	///<summary>
	/// Enumeration used to expose the different child entity types 
	/// for child properties in <c>MetaBuilder.BusinessLogic.WorkspaceType</c>
	///</summary>
	public enum WorkspaceTypeChildEntityTypes
	{

		///<summary>
		/// Collection of <c>WorkspaceType</c> as OneToMany for WorkspaceCollection
		///</summary>
		[ChildEntityType(typeof(TList<Workspace>))]
		WorkspaceCollection,
	}
	
	#endregion WorkspaceTypeChildEntityTypes
	
	#region WorkspaceTypeFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="WorkspaceType"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class WorkspaceTypeFilterBuilder : SqlFilterBuilder<WorkspaceTypeColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the WorkspaceTypeFilterBuilder class.
		/// </summary>
		public WorkspaceTypeFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the WorkspaceTypeFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public WorkspaceTypeFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the WorkspaceTypeFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public WorkspaceTypeFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion WorkspaceTypeFilterBuilder
	
	#region WorkspaceTypeParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="WorkspaceType"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class WorkspaceTypeParameterBuilder : ParameterizedSqlFilterBuilder<WorkspaceTypeColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the WorkspaceTypeParameterBuilder class.
		/// </summary>
		public WorkspaceTypeParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the WorkspaceTypeParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public WorkspaceTypeParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the WorkspaceTypeParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public WorkspaceTypeParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion WorkspaceTypeParameterBuilder
} // end namespace
