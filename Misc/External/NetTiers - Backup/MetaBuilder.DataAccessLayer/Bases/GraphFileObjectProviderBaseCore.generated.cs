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
	/// This class is the base class for any <see cref="GraphFileObjectProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract partial class GraphFileObjectProviderBaseCore : EntityProviderBase<MetaBuilder.BusinessLogic.GraphFileObject, MetaBuilder.BusinessLogic.GraphFileObjectKey>
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
		public override bool Delete(TransactionManager transactionManager, MetaBuilder.BusinessLogic.GraphFileObjectKey key)
		{
			return Delete(transactionManager, key.GraphFileID, key.MetaObjectID, key.MachineID, key.GraphFileMachine);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="graphFileID">. Primary Key.</param>
		/// <param name="metaObjectID">. Primary Key.</param>
		/// <param name="machineID">. Primary Key.</param>
		/// <param name="graphFileMachine">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(System.Int32 graphFileID, System.Int32 metaObjectID, System.String machineID, System.String graphFileMachine)
		{
			return Delete(null, graphFileID, metaObjectID, machineID, graphFileMachine);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="graphFileID">. Primary Key.</param>
		/// <param name="metaObjectID">. Primary Key.</param>
		/// <param name="machineID">. Primary Key.</param>
		/// <param name="graphFileMachine">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(TransactionManager transactionManager, System.Int32 graphFileID, System.Int32 metaObjectID, System.String machineID, System.String graphFileMachine);		
		
		#endregion Delete Methods
		
		#region Get By Foreign Key Functions
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileObject_GraphFile key.
		///		FK_GraphFileObject_GraphFile Description: 
		/// </summary>
		/// <param name="graphFileID"></param>
		/// <param name="graphFileMachine"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileObject objects.</returns>
		public MetaBuilder.BusinessLogic.TList<GraphFileObject> GetByGraphFileIDGraphFileMachine(System.Int32 graphFileID, System.String graphFileMachine)
		{
			int count = -1;
			return GetByGraphFileIDGraphFileMachine(graphFileID, graphFileMachine, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileObject_GraphFile key.
		///		FK_GraphFileObject_GraphFile Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="graphFileID"></param>
		/// <param name="graphFileMachine"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileObject objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<GraphFileObject> GetByGraphFileIDGraphFileMachine(TransactionManager transactionManager, System.Int32 graphFileID, System.String graphFileMachine)
		{
			int count = -1;
			return GetByGraphFileIDGraphFileMachine(transactionManager, graphFileID, graphFileMachine, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileObject_GraphFile key.
		///		FK_GraphFileObject_GraphFile Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="graphFileID"></param>
		/// <param name="graphFileMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileObject objects.</returns>
		public MetaBuilder.BusinessLogic.TList<GraphFileObject> GetByGraphFileIDGraphFileMachine(TransactionManager transactionManager, System.Int32 graphFileID, System.String graphFileMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByGraphFileIDGraphFileMachine(transactionManager, graphFileID, graphFileMachine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileObject_GraphFile key.
		///		fKGraphFileObjectGraphFile Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="graphFileID"></param>
		/// <param name="graphFileMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileObject objects.</returns>
		public MetaBuilder.BusinessLogic.TList<GraphFileObject> GetByGraphFileIDGraphFileMachine(System.Int32 graphFileID, System.String graphFileMachine, int start, int pageLength)
		{
			int count =  -1;
			return GetByGraphFileIDGraphFileMachine(null, graphFileID, graphFileMachine, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileObject_GraphFile key.
		///		fKGraphFileObjectGraphFile Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="graphFileID"></param>
		/// <param name="graphFileMachine"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileObject objects.</returns>
		public MetaBuilder.BusinessLogic.TList<GraphFileObject> GetByGraphFileIDGraphFileMachine(System.Int32 graphFileID, System.String graphFileMachine, int start, int pageLength,out int count)
		{
			return GetByGraphFileIDGraphFileMachine(null, graphFileID, graphFileMachine, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileObject_GraphFile key.
		///		FK_GraphFileObject_GraphFile Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="graphFileID"></param>
		/// <param name="graphFileMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileObject objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<GraphFileObject> GetByGraphFileIDGraphFileMachine(TransactionManager transactionManager, System.Int32 graphFileID, System.String graphFileMachine, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileObject_MetaObject key.
		///		FK_GraphFileObject_MetaObject Description: 
		/// </summary>
		/// <param name="metaObjectID"></param>
		/// <param name="machineID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileObject objects.</returns>
		public MetaBuilder.BusinessLogic.TList<GraphFileObject> GetByMetaObjectIDMachineID(System.Int32 metaObjectID, System.String machineID)
		{
			int count = -1;
			return GetByMetaObjectIDMachineID(metaObjectID, machineID, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileObject_MetaObject key.
		///		FK_GraphFileObject_MetaObject Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="metaObjectID"></param>
		/// <param name="machineID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileObject objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<GraphFileObject> GetByMetaObjectIDMachineID(TransactionManager transactionManager, System.Int32 metaObjectID, System.String machineID)
		{
			int count = -1;
			return GetByMetaObjectIDMachineID(transactionManager, metaObjectID, machineID, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileObject_MetaObject key.
		///		FK_GraphFileObject_MetaObject Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="metaObjectID"></param>
		/// <param name="machineID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileObject objects.</returns>
		public MetaBuilder.BusinessLogic.TList<GraphFileObject> GetByMetaObjectIDMachineID(TransactionManager transactionManager, System.Int32 metaObjectID, System.String machineID, int start, int pageLength)
		{
			int count = -1;
			return GetByMetaObjectIDMachineID(transactionManager, metaObjectID, machineID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileObject_MetaObject key.
		///		fKGraphFileObjectMetaObject Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="metaObjectID"></param>
		/// <param name="machineID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileObject objects.</returns>
		public MetaBuilder.BusinessLogic.TList<GraphFileObject> GetByMetaObjectIDMachineID(System.Int32 metaObjectID, System.String machineID, int start, int pageLength)
		{
			int count =  -1;
			return GetByMetaObjectIDMachineID(null, metaObjectID, machineID, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileObject_MetaObject key.
		///		fKGraphFileObjectMetaObject Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="metaObjectID"></param>
		/// <param name="machineID"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileObject objects.</returns>
		public MetaBuilder.BusinessLogic.TList<GraphFileObject> GetByMetaObjectIDMachineID(System.Int32 metaObjectID, System.String machineID, int start, int pageLength,out int count)
		{
			return GetByMetaObjectIDMachineID(null, metaObjectID, machineID, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileObject_MetaObject key.
		///		FK_GraphFileObject_MetaObject Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="metaObjectID"></param>
		/// <param name="machineID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileObject objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<GraphFileObject> GetByMetaObjectIDMachineID(TransactionManager transactionManager, System.Int32 metaObjectID, System.String machineID, int start, int pageLength, out int count);
		
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
		public override MetaBuilder.BusinessLogic.GraphFileObject Get(TransactionManager transactionManager, MetaBuilder.BusinessLogic.GraphFileObjectKey key, int start, int pageLength)
		{
			return GetByGraphFileIDMetaObjectIDMachineIDGraphFileMachine(transactionManager, key.GraphFileID, key.MetaObjectID, key.MachineID, key.GraphFileMachine, start, pageLength);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key PK_GraphFileObject index.
		/// </summary>
		/// <param name="graphFileID"></param>
		/// <param name="metaObjectID"></param>
		/// <param name="machineID"></param>
		/// <param name="graphFileMachine"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.GraphFileObject"/> class.</returns>
		public MetaBuilder.BusinessLogic.GraphFileObject GetByGraphFileIDMetaObjectIDMachineIDGraphFileMachine(System.Int32 graphFileID, System.Int32 metaObjectID, System.String machineID, System.String graphFileMachine)
		{
			int count = -1;
			return GetByGraphFileIDMetaObjectIDMachineIDGraphFileMachine(null,graphFileID, metaObjectID, machineID, graphFileMachine, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_GraphFileObject index.
		/// </summary>
		/// <param name="graphFileID"></param>
		/// <param name="metaObjectID"></param>
		/// <param name="machineID"></param>
		/// <param name="graphFileMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.GraphFileObject"/> class.</returns>
		public MetaBuilder.BusinessLogic.GraphFileObject GetByGraphFileIDMetaObjectIDMachineIDGraphFileMachine(System.Int32 graphFileID, System.Int32 metaObjectID, System.String machineID, System.String graphFileMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByGraphFileIDMetaObjectIDMachineIDGraphFileMachine(null, graphFileID, metaObjectID, machineID, graphFileMachine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_GraphFileObject index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="graphFileID"></param>
		/// <param name="metaObjectID"></param>
		/// <param name="machineID"></param>
		/// <param name="graphFileMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.GraphFileObject"/> class.</returns>
		public MetaBuilder.BusinessLogic.GraphFileObject GetByGraphFileIDMetaObjectIDMachineIDGraphFileMachine(TransactionManager transactionManager, System.Int32 graphFileID, System.Int32 metaObjectID, System.String machineID, System.String graphFileMachine)
		{
			int count = -1;
			return GetByGraphFileIDMetaObjectIDMachineIDGraphFileMachine(transactionManager, graphFileID, metaObjectID, machineID, graphFileMachine, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_GraphFileObject index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="graphFileID"></param>
		/// <param name="metaObjectID"></param>
		/// <param name="machineID"></param>
		/// <param name="graphFileMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.GraphFileObject"/> class.</returns>
		public MetaBuilder.BusinessLogic.GraphFileObject GetByGraphFileIDMetaObjectIDMachineIDGraphFileMachine(TransactionManager transactionManager, System.Int32 graphFileID, System.Int32 metaObjectID, System.String machineID, System.String graphFileMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByGraphFileIDMetaObjectIDMachineIDGraphFileMachine(transactionManager, graphFileID, metaObjectID, machineID, graphFileMachine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_GraphFileObject index.
		/// </summary>
		/// <param name="graphFileID"></param>
		/// <param name="metaObjectID"></param>
		/// <param name="machineID"></param>
		/// <param name="graphFileMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.GraphFileObject"/> class.</returns>
		public MetaBuilder.BusinessLogic.GraphFileObject GetByGraphFileIDMetaObjectIDMachineIDGraphFileMachine(System.Int32 graphFileID, System.Int32 metaObjectID, System.String machineID, System.String graphFileMachine, int start, int pageLength, out int count)
		{
			return GetByGraphFileIDMetaObjectIDMachineIDGraphFileMachine(null, graphFileID, metaObjectID, machineID, graphFileMachine, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_GraphFileObject index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="graphFileID"></param>
		/// <param name="metaObjectID"></param>
		/// <param name="machineID"></param>
		/// <param name="graphFileMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.GraphFileObject"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.GraphFileObject GetByGraphFileIDMetaObjectIDMachineIDGraphFileMachine(TransactionManager transactionManager, System.Int32 graphFileID, System.Int32 metaObjectID, System.String machineID, System.String graphFileMachine, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a MetaBuilder.BusinessLogic.TList&lt;GraphFileObject&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="MetaBuilder.BusinessLogic.TList&lt;GraphFileObject&gt;"/></returns>
		public static MetaBuilder.BusinessLogic.TList<GraphFileObject> Fill(IDataReader reader, MetaBuilder.BusinessLogic.TList<GraphFileObject> rows, int start, int pageLength)
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
				
				MetaBuilder.BusinessLogic.GraphFileObject c = null;
				if (DataRepository.Provider.UseEntityFactory)
				{
					key = @"GraphFileObject" 
							+ (reader.IsDBNull(reader.GetOrdinal("GraphFileID"))?(int)0:(System.Int32)reader["GraphFileID"]).ToString()
							+ (reader.IsDBNull(reader.GetOrdinal("MetaObjectID"))?(int)0:(System.Int32)reader["MetaObjectID"]).ToString()
							+ (reader.IsDBNull(reader.GetOrdinal("MachineID"))?string.Empty:(System.String)reader["MachineID"]).ToString()
							+ (reader.IsDBNull(reader.GetOrdinal("GraphFileMachine"))?string.Empty:(System.String)reader["GraphFileMachine"]).ToString();

					c = EntityManager.LocateOrCreate<GraphFileObject>(
						key.ToString(), // EntityTrackingKey 
						"GraphFileObject",  //Creational Type
						DataRepository.Provider.EntityCreationalFactoryType,  //Factory used to create entity
						DataRepository.Provider.EnableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.GraphFileObject();
				}
				
				if (!DataRepository.Provider.EnableEntityTracking || c.EntityState == EntityState.Added)
                {
					c.SuppressEntityEvents = true;
					c.GraphFileID = (System.Int32)reader["GraphFileID"];
					c.OriginalGraphFileID = c.GraphFileID; //(reader.IsDBNull(reader.GetOrdinal("GraphFileID")))?(int)0:(System.Int32)reader["GraphFileID"];
					c.MetaObjectID = (System.Int32)reader["MetaObjectID"];
					c.OriginalMetaObjectID = c.MetaObjectID; //(reader.IsDBNull(reader.GetOrdinal("MetaObjectID")))?(int)0:(System.Int32)reader["MetaObjectID"];
					c.MachineID = (System.String)reader["MachineID"];
					c.OriginalMachineID = c.MachineID; //(reader.IsDBNull(reader.GetOrdinal("MachineID")))?string.Empty:(System.String)reader["MachineID"];
					c.GraphFileMachine = (System.String)reader["GraphFileMachine"];
					c.OriginalGraphFileMachine = c.GraphFileMachine; //(reader.IsDBNull(reader.GetOrdinal("GraphFileMachine")))?string.Empty:(System.String)reader["GraphFileMachine"];
					c.EntityTrackingKey = key;
					c.AcceptChanges();
					c.SuppressEntityEvents = false;
				}
				rows.Add(c);
			}
			return rows;
		}
		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.GraphFileObject"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.GraphFileObject"/> object to refresh.</param>
		public static void RefreshEntity(IDataReader reader, MetaBuilder.BusinessLogic.GraphFileObject entity)
		{
			if (!reader.Read()) return;
			
			entity.GraphFileID = (System.Int32)reader["GraphFileID"];
			entity.OriginalGraphFileID = (System.Int32)reader["GraphFileID"];
			entity.MetaObjectID = (System.Int32)reader["MetaObjectID"];
			entity.OriginalMetaObjectID = (System.Int32)reader["MetaObjectID"];
			entity.MachineID = (System.String)reader["MachineID"];
			entity.OriginalMachineID = (System.String)reader["MachineID"];
			entity.GraphFileMachine = (System.String)reader["GraphFileMachine"];
			entity.OriginalGraphFileMachine = (System.String)reader["GraphFileMachine"];
			entity.AcceptChanges();
		}
		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.GraphFileObject"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.GraphFileObject"/> object.</param>
		public static void RefreshEntity(DataSet dataSet, MetaBuilder.BusinessLogic.GraphFileObject entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.GraphFileID = (System.Int32)dataRow["GraphFileID"];
			entity.OriginalGraphFileID = (System.Int32)dataRow["GraphFileID"];
			entity.MetaObjectID = (System.Int32)dataRow["MetaObjectID"];
			entity.OriginalMetaObjectID = (System.Int32)dataRow["MetaObjectID"];
			entity.MachineID = (System.String)dataRow["MachineID"];
			entity.OriginalMachineID = (System.String)dataRow["MachineID"];
			entity.GraphFileMachine = (System.String)dataRow["GraphFileMachine"];
			entity.OriginalGraphFileMachine = (System.String)dataRow["GraphFileMachine"];
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
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.GraphFileObject"/> object to load.</param>
		/// <param name="deep">Boolean. A flag that indicates whether to recursively save all Property Collection that are descendants of this instance. If True, saves the complete object graph below this object. If False, saves this object only. </param>
		/// <param name="deepLoadType">DeepLoadType Enumeration to Include/Exclude object property collections from Load.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.GraphFileObject Property Collection Type Array To Include or Exclude from Load</param>
		/// <param name="innerList">A collection of child types for easy access.</param>
	    /// <exception cref="ArgumentNullException">entity or childTypes is null.</exception>
	    /// <exception cref="ArgumentException">deepLoadType has invalid value.</exception>
		internal override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.GraphFileObject entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, ChildEntityTypesList innerList)
		{
			if(entity == null)
				return;

			#region GraphFileIDGraphFileMachineSource	
			if (CanDeepLoad(entity, "GraphFile", "GraphFileIDGraphFileMachineSource", deepLoadType, innerList) 
				&& entity.GraphFileIDGraphFileMachineSource == null)
			{
				object[] pkItems = new object[2];
				pkItems[0] = entity.GraphFileID;
				pkItems[1] = entity.GraphFileMachine;
				GraphFile tmpEntity = EntityManager.LocateEntity<GraphFile>(EntityLocator.ConstructKeyFromPkItems(typeof(GraphFile), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.GraphFileIDGraphFileMachineSource = tmpEntity;
				else
					entity.GraphFileIDGraphFileMachineSource = DataRepository.GraphFileProvider.GetByPkidMachine(entity.GraphFileID, entity.GraphFileMachine);
			
				if (deep && entity.GraphFileIDGraphFileMachineSource != null)
				{
					DataRepository.GraphFileProvider.DeepLoad(transactionManager, entity.GraphFileIDGraphFileMachineSource, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion GraphFileIDGraphFileMachineSource

			#region MetaObjectIDMachineIDSource	
			if (CanDeepLoad(entity, "MetaObject", "MetaObjectIDMachineIDSource", deepLoadType, innerList) 
				&& entity.MetaObjectIDMachineIDSource == null)
			{
				object[] pkItems = new object[2];
				pkItems[0] = entity.MetaObjectID;
				pkItems[1] = entity.MachineID;
				MetaObject tmpEntity = EntityManager.LocateEntity<MetaObject>(EntityLocator.ConstructKeyFromPkItems(typeof(MetaObject), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.MetaObjectIDMachineIDSource = tmpEntity;
				else
					entity.MetaObjectIDMachineIDSource = DataRepository.MetaObjectProvider.GetByPkidMachine(entity.MetaObjectID, entity.MachineID);
			
				if (deep && entity.MetaObjectIDMachineIDSource != null)
				{
					DataRepository.MetaObjectProvider.DeepLoad(transactionManager, entity.MetaObjectIDMachineIDSource, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion MetaObjectIDMachineIDSource
			
			// Load Entity through Provider
		}
		
		#endregion 
		
		#region DeepSave Methods

		/// <summary>
		/// Deep Save the entire object graph of the MetaBuilder.BusinessLogic.GraphFileObject object with criteria based of the child 
		/// Type property array and DeepSaveType.
		/// </summary>
		/// <param name="transactionManager">The transaction manager.</param>
		/// <param name="entity">MetaBuilder.BusinessLogic.GraphFileObject instance</param>
		/// <param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.GraphFileObject Property Collection Type Array To Include or Exclude from Save</param>
		/// <param name="innerList">A Hashtable of child types for easy access.</param>
		internal override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.GraphFileObject entity, DeepSaveType deepSaveType, System.Type[] childTypes, ChildEntityTypesList innerList)
		{	
			if (entity == null)
				return false;
							
			#region Composite Parent Properties
			//Save Source Composite Properties, however, don't call deep save on them.  
			//So they only get saved a single level deep.
			
			#region GraphFileIDGraphFileMachineSource
			if (CanDeepSave(entity, "GraphFile", "GraphFileIDGraphFileMachineSource", deepSaveType, innerList) 
				&& entity.GraphFileIDGraphFileMachineSource != null)
			{
				DataRepository.GraphFileProvider.Save(transactionManager, entity.GraphFileIDGraphFileMachineSource);
				entity.GraphFileID = entity.GraphFileIDGraphFileMachineSource.Pkid;
				entity.GraphFileMachine = entity.GraphFileIDGraphFileMachineSource.Machine;
			}
			#endregion 
			
			#region MetaObjectIDMachineIDSource
			if (CanDeepSave(entity, "MetaObject", "MetaObjectIDMachineIDSource", deepSaveType, innerList) 
				&& entity.MetaObjectIDMachineIDSource != null)
			{
				DataRepository.MetaObjectProvider.Save(transactionManager, entity.MetaObjectIDMachineIDSource);
				entity.MetaObjectID = entity.MetaObjectIDMachineIDSource.Pkid;
				entity.MachineID = entity.MetaObjectIDMachineIDSource.Machine;
			}
			#endregion 
			#endregion Composite Parent Properties

			// Save Root Entity through Provider
			this.Save(transactionManager, entity);
			
			
						
			return true;
		}
		#endregion
	} // end class
	
	#region GraphFileObjectChildEntityTypes
	
	///<summary>
	/// Enumeration used to expose the different child entity types 
	/// for child properties in <c>MetaBuilder.BusinessLogic.GraphFileObject</c>
	///</summary>
	public enum GraphFileObjectChildEntityTypes
	{
		
		///<summary>
		/// Composite Property for <c>GraphFile</c> at GraphFileIDGraphFileMachineSource
		///</summary>
		[ChildEntityType(typeof(GraphFile))]
		GraphFile,
			
		///<summary>
		/// Composite Property for <c>MetaObject</c> at MetaObjectIDMachineIDSource
		///</summary>
		[ChildEntityType(typeof(MetaObject))]
		MetaObject,
		}
	
	#endregion GraphFileObjectChildEntityTypes
	
	#region GraphFileObjectFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="GraphFileObject"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class GraphFileObjectFilterBuilder : SqlFilterBuilder<GraphFileObjectColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the GraphFileObjectFilterBuilder class.
		/// </summary>
		public GraphFileObjectFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the GraphFileObjectFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public GraphFileObjectFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the GraphFileObjectFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public GraphFileObjectFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion GraphFileObjectFilterBuilder
	
	#region GraphFileObjectParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="GraphFileObject"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class GraphFileObjectParameterBuilder : ParameterizedSqlFilterBuilder<GraphFileObjectColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the GraphFileObjectParameterBuilder class.
		/// </summary>
		public GraphFileObjectParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the GraphFileObjectParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public GraphFileObjectParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the GraphFileObjectParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public GraphFileObjectParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion GraphFileObjectParameterBuilder
} // end namespace
