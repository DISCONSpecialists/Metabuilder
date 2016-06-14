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
		/// <param name="_graphFileID">. Primary Key.</param>
		/// <param name="_metaObjectID">. Primary Key.</param>
		/// <param name="_machineID">. Primary Key.</param>
		/// <param name="_graphFileMachine">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(System.Int32 _graphFileID, System.Int32 _metaObjectID, System.String _machineID, System.String _graphFileMachine)
		{
			return Delete(null, _graphFileID, _metaObjectID, _machineID, _graphFileMachine);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_graphFileID">. Primary Key.</param>
		/// <param name="_metaObjectID">. Primary Key.</param>
		/// <param name="_machineID">. Primary Key.</param>
		/// <param name="_graphFileMachine">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(TransactionManager transactionManager, System.Int32 _graphFileID, System.Int32 _metaObjectID, System.String _machineID, System.String _graphFileMachine);		
		
		#endregion Delete Methods
		
		#region Get By Foreign Key Functions
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileObject_GraphFile key.
		///		FK_GraphFileObject_GraphFile Description: 
		/// </summary>
		/// <param name="_graphFileID"></param>
		/// <param name="_graphFileMachine"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileObject objects.</returns>
		public TList<GraphFileObject> GetByGraphFileIDGraphFileMachine(System.Int32 _graphFileID, System.String _graphFileMachine)
		{
			int count = -1;
			return GetByGraphFileIDGraphFileMachine(_graphFileID, _graphFileMachine, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileObject_GraphFile key.
		///		FK_GraphFileObject_GraphFile Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_graphFileID"></param>
		/// <param name="_graphFileMachine"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileObject objects.</returns>
		/// <remarks></remarks>
		public TList<GraphFileObject> GetByGraphFileIDGraphFileMachine(TransactionManager transactionManager, System.Int32 _graphFileID, System.String _graphFileMachine)
		{
			int count = -1;
			return GetByGraphFileIDGraphFileMachine(transactionManager, _graphFileID, _graphFileMachine, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileObject_GraphFile key.
		///		FK_GraphFileObject_GraphFile Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_graphFileID"></param>
		/// <param name="_graphFileMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileObject objects.</returns>
		public TList<GraphFileObject> GetByGraphFileIDGraphFileMachine(TransactionManager transactionManager, System.Int32 _graphFileID, System.String _graphFileMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByGraphFileIDGraphFileMachine(transactionManager, _graphFileID, _graphFileMachine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileObject_GraphFile key.
		///		fK_GraphFileObject_GraphFile Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_graphFileID"></param>
		/// <param name="_graphFileMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileObject objects.</returns>
		public TList<GraphFileObject> GetByGraphFileIDGraphFileMachine(System.Int32 _graphFileID, System.String _graphFileMachine, int start, int pageLength)
		{
			int count =  -1;
			return GetByGraphFileIDGraphFileMachine(null, _graphFileID, _graphFileMachine, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileObject_GraphFile key.
		///		fK_GraphFileObject_GraphFile Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_graphFileID"></param>
		/// <param name="_graphFileMachine"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileObject objects.</returns>
		public TList<GraphFileObject> GetByGraphFileIDGraphFileMachine(System.Int32 _graphFileID, System.String _graphFileMachine, int start, int pageLength,out int count)
		{
			return GetByGraphFileIDGraphFileMachine(null, _graphFileID, _graphFileMachine, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileObject_GraphFile key.
		///		FK_GraphFileObject_GraphFile Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_graphFileID"></param>
		/// <param name="_graphFileMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileObject objects.</returns>
		public abstract TList<GraphFileObject> GetByGraphFileIDGraphFileMachine(TransactionManager transactionManager, System.Int32 _graphFileID, System.String _graphFileMachine, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileObject_MetaObject key.
		///		FK_GraphFileObject_MetaObject Description: 
		/// </summary>
		/// <param name="_metaObjectID"></param>
		/// <param name="_machineID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileObject objects.</returns>
		public TList<GraphFileObject> GetByMetaObjectIDMachineID(System.Int32 _metaObjectID, System.String _machineID)
		{
			int count = -1;
			return GetByMetaObjectIDMachineID(_metaObjectID, _machineID, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileObject_MetaObject key.
		///		FK_GraphFileObject_MetaObject Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_metaObjectID"></param>
		/// <param name="_machineID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileObject objects.</returns>
		/// <remarks></remarks>
		public TList<GraphFileObject> GetByMetaObjectIDMachineID(TransactionManager transactionManager, System.Int32 _metaObjectID, System.String _machineID)
		{
			int count = -1;
			return GetByMetaObjectIDMachineID(transactionManager, _metaObjectID, _machineID, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileObject_MetaObject key.
		///		FK_GraphFileObject_MetaObject Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_metaObjectID"></param>
		/// <param name="_machineID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileObject objects.</returns>
		public TList<GraphFileObject> GetByMetaObjectIDMachineID(TransactionManager transactionManager, System.Int32 _metaObjectID, System.String _machineID, int start, int pageLength)
		{
			int count = -1;
			return GetByMetaObjectIDMachineID(transactionManager, _metaObjectID, _machineID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileObject_MetaObject key.
		///		fK_GraphFileObject_MetaObject Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_metaObjectID"></param>
		/// <param name="_machineID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileObject objects.</returns>
		public TList<GraphFileObject> GetByMetaObjectIDMachineID(System.Int32 _metaObjectID, System.String _machineID, int start, int pageLength)
		{
			int count =  -1;
			return GetByMetaObjectIDMachineID(null, _metaObjectID, _machineID, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileObject_MetaObject key.
		///		fK_GraphFileObject_MetaObject Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_metaObjectID"></param>
		/// <param name="_machineID"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileObject objects.</returns>
		public TList<GraphFileObject> GetByMetaObjectIDMachineID(System.Int32 _metaObjectID, System.String _machineID, int start, int pageLength,out int count)
		{
			return GetByMetaObjectIDMachineID(null, _metaObjectID, _machineID, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileObject_MetaObject key.
		///		FK_GraphFileObject_MetaObject Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_metaObjectID"></param>
		/// <param name="_machineID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileObject objects.</returns>
		public abstract TList<GraphFileObject> GetByMetaObjectIDMachineID(TransactionManager transactionManager, System.Int32 _metaObjectID, System.String _machineID, int start, int pageLength, out int count);
		
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
		/// <param name="_graphFileID"></param>
		/// <param name="_metaObjectID"></param>
		/// <param name="_machineID"></param>
		/// <param name="_graphFileMachine"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.GraphFileObject"/> class.</returns>
		public MetaBuilder.BusinessLogic.GraphFileObject GetByGraphFileIDMetaObjectIDMachineIDGraphFileMachine(System.Int32 _graphFileID, System.Int32 _metaObjectID, System.String _machineID, System.String _graphFileMachine)
		{
			int count = -1;
			return GetByGraphFileIDMetaObjectIDMachineIDGraphFileMachine(null,_graphFileID, _metaObjectID, _machineID, _graphFileMachine, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_GraphFileObject index.
		/// </summary>
		/// <param name="_graphFileID"></param>
		/// <param name="_metaObjectID"></param>
		/// <param name="_machineID"></param>
		/// <param name="_graphFileMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.GraphFileObject"/> class.</returns>
		public MetaBuilder.BusinessLogic.GraphFileObject GetByGraphFileIDMetaObjectIDMachineIDGraphFileMachine(System.Int32 _graphFileID, System.Int32 _metaObjectID, System.String _machineID, System.String _graphFileMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByGraphFileIDMetaObjectIDMachineIDGraphFileMachine(null, _graphFileID, _metaObjectID, _machineID, _graphFileMachine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_GraphFileObject index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_graphFileID"></param>
		/// <param name="_metaObjectID"></param>
		/// <param name="_machineID"></param>
		/// <param name="_graphFileMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.GraphFileObject"/> class.</returns>
		public MetaBuilder.BusinessLogic.GraphFileObject GetByGraphFileIDMetaObjectIDMachineIDGraphFileMachine(TransactionManager transactionManager, System.Int32 _graphFileID, System.Int32 _metaObjectID, System.String _machineID, System.String _graphFileMachine)
		{
			int count = -1;
			return GetByGraphFileIDMetaObjectIDMachineIDGraphFileMachine(transactionManager, _graphFileID, _metaObjectID, _machineID, _graphFileMachine, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_GraphFileObject index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_graphFileID"></param>
		/// <param name="_metaObjectID"></param>
		/// <param name="_machineID"></param>
		/// <param name="_graphFileMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.GraphFileObject"/> class.</returns>
		public MetaBuilder.BusinessLogic.GraphFileObject GetByGraphFileIDMetaObjectIDMachineIDGraphFileMachine(TransactionManager transactionManager, System.Int32 _graphFileID, System.Int32 _metaObjectID, System.String _machineID, System.String _graphFileMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByGraphFileIDMetaObjectIDMachineIDGraphFileMachine(transactionManager, _graphFileID, _metaObjectID, _machineID, _graphFileMachine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_GraphFileObject index.
		/// </summary>
		/// <param name="_graphFileID"></param>
		/// <param name="_metaObjectID"></param>
		/// <param name="_machineID"></param>
		/// <param name="_graphFileMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.GraphFileObject"/> class.</returns>
		public MetaBuilder.BusinessLogic.GraphFileObject GetByGraphFileIDMetaObjectIDMachineIDGraphFileMachine(System.Int32 _graphFileID, System.Int32 _metaObjectID, System.String _machineID, System.String _graphFileMachine, int start, int pageLength, out int count)
		{
			return GetByGraphFileIDMetaObjectIDMachineIDGraphFileMachine(null, _graphFileID, _metaObjectID, _machineID, _graphFileMachine, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_GraphFileObject index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_graphFileID"></param>
		/// <param name="_metaObjectID"></param>
		/// <param name="_machineID"></param>
		/// <param name="_graphFileMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.GraphFileObject"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.GraphFileObject GetByGraphFileIDMetaObjectIDMachineIDGraphFileMachine(TransactionManager transactionManager, System.Int32 _graphFileID, System.Int32 _metaObjectID, System.String _machineID, System.String _graphFileMachine, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a TList&lt;GraphFileObject&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="TList&lt;GraphFileObject&gt;"/></returns>
		public static TList<GraphFileObject> Fill(IDataReader reader, TList<GraphFileObject> rows, int start, int pageLength)
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
				
				MetaBuilder.BusinessLogic.GraphFileObject c = null;
				if (useEntityFactory)
				{
					key = new System.Text.StringBuilder("GraphFileObject")
					.Append("|").Append((System.Int32)reader[((int)GraphFileObjectColumn.GraphFileID - 1)])
					.Append("|").Append((System.Int32)reader[((int)GraphFileObjectColumn.MetaObjectID - 1)])
					.Append("|").Append((System.String)reader[((int)GraphFileObjectColumn.MachineID - 1)])
					.Append("|").Append((System.String)reader[((int)GraphFileObjectColumn.GraphFileMachine - 1)]).ToString();
					c = EntityManager.LocateOrCreate<GraphFileObject>(
					key.ToString(), // EntityTrackingKey
					"GraphFileObject",  //Creational Type
					entityCreationFactoryType,  //Factory used to create entity
					enableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.GraphFileObject();
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
					c.GraphFileID = (System.Int32)reader[((int)GraphFileObjectColumn.GraphFileID - 1)];
					c.OriginalGraphFileID = c.GraphFileID;
					c.MetaObjectID = (System.Int32)reader[((int)GraphFileObjectColumn.MetaObjectID - 1)];
					c.OriginalMetaObjectID = c.MetaObjectID;
					c.MachineID = (System.String)reader[((int)GraphFileObjectColumn.MachineID - 1)];
					c.OriginalMachineID = c.MachineID;
					c.GraphFileMachine = (System.String)reader[((int)GraphFileObjectColumn.GraphFileMachine - 1)];
					c.OriginalGraphFileMachine = c.GraphFileMachine;
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
			
			entity.GraphFileID = (System.Int32)reader[((int)GraphFileObjectColumn.GraphFileID - 1)];
			entity.OriginalGraphFileID = (System.Int32)reader["GraphFileID"];
			entity.MetaObjectID = (System.Int32)reader[((int)GraphFileObjectColumn.MetaObjectID - 1)];
			entity.OriginalMetaObjectID = (System.Int32)reader["MetaObjectID"];
			entity.MachineID = (System.String)reader[((int)GraphFileObjectColumn.MachineID - 1)];
			entity.OriginalMachineID = (System.String)reader["MachineID"];
			entity.GraphFileMachine = (System.String)reader[((int)GraphFileObjectColumn.GraphFileMachine - 1)];
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
		public override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.GraphFileObject entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, DeepSession innerList)
		{
			if(entity == null)
				return;

			#region GraphFileIDGraphFileMachineSource	
			if (CanDeepLoad(entity, "GraphFile|GraphFileIDGraphFileMachineSource", deepLoadType, innerList) 
				&& entity.GraphFileIDGraphFileMachineSource == null)
			{
				object[] pkItems = new object[2];
				pkItems[0] = entity.GraphFileID;
				pkItems[1] = entity.GraphFileMachine;
				GraphFile tmpEntity = EntityManager.LocateEntity<GraphFile>(EntityLocator.ConstructKeyFromPkItems(typeof(GraphFile), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.GraphFileIDGraphFileMachineSource = tmpEntity;
				else
					entity.GraphFileIDGraphFileMachineSource = DataRepository.GraphFileProvider.GetBypkidMachine(transactionManager, entity.GraphFileID, entity.GraphFileMachine);		
				
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'GraphFileIDGraphFileMachineSource' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.GraphFileIDGraphFileMachineSource != null)
				{
					innerList.SkipChildren = true;
					DataRepository.GraphFileProvider.DeepLoad(transactionManager, entity.GraphFileIDGraphFileMachineSource, deep, deepLoadType, childTypes, innerList);
					innerList.SkipChildren = false;
				}
					
			}
			#endregion GraphFileIDGraphFileMachineSource

			#region MetaObjectIDMachineIDSource	
			if (CanDeepLoad(entity, "MetaObject|MetaObjectIDMachineIDSource", deepLoadType, innerList) 
				&& entity.MetaObjectIDMachineIDSource == null)
			{
				object[] pkItems = new object[2];
				pkItems[0] = entity.MetaObjectID;
				pkItems[1] = entity.MachineID;
				MetaObject tmpEntity = EntityManager.LocateEntity<MetaObject>(EntityLocator.ConstructKeyFromPkItems(typeof(MetaObject), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.MetaObjectIDMachineIDSource = tmpEntity;
				else
					entity.MetaObjectIDMachineIDSource = DataRepository.MetaObjectProvider.GetBypkidMachine(transactionManager, entity.MetaObjectID, entity.MachineID);		
				
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'MetaObjectIDMachineIDSource' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.MetaObjectIDMachineIDSource != null)
				{
					innerList.SkipChildren = true;
					DataRepository.MetaObjectProvider.DeepLoad(transactionManager, entity.MetaObjectIDMachineIDSource, deep, deepLoadType, childTypes, innerList);
					innerList.SkipChildren = false;
				}
					
			}
			#endregion MetaObjectIDMachineIDSource
			
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
		/// Deep Save the entire object graph of the MetaBuilder.BusinessLogic.GraphFileObject object with criteria based of the child 
		/// Type property array and DeepSaveType.
		/// </summary>
		/// <param name="transactionManager">The transaction manager.</param>
		/// <param name="entity">MetaBuilder.BusinessLogic.GraphFileObject instance</param>
		/// <param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.GraphFileObject Property Collection Type Array To Include or Exclude from Save</param>
		/// <param name="innerList">A Hashtable of child types for easy access.</param>
		public override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.GraphFileObject entity, DeepSaveType deepSaveType, System.Type[] childTypes, DeepSession innerList)
		{	
			if (entity == null)
				return false;
							
			#region Composite Parent Properties
			//Save Source Composite Properties, however, don't call deep save on them.  
			//So they only get saved a single level deep.
			
			#region GraphFileIDGraphFileMachineSource
			if (CanDeepSave(entity, "GraphFile|GraphFileIDGraphFileMachineSource", deepSaveType, innerList) 
				&& entity.GraphFileIDGraphFileMachineSource != null)
			{
				DataRepository.GraphFileProvider.Save(transactionManager, entity.GraphFileIDGraphFileMachineSource);
				entity.GraphFileID = entity.GraphFileIDGraphFileMachineSource.pkid;
				entity.GraphFileMachine = entity.GraphFileIDGraphFileMachineSource.Machine;
			}
			#endregion 
			
			#region MetaObjectIDMachineIDSource
			if (CanDeepSave(entity, "MetaObject|MetaObjectIDMachineIDSource", deepSaveType, innerList) 
				&& entity.MetaObjectIDMachineIDSource != null)
			{
				DataRepository.MetaObjectProvider.Save(transactionManager, entity.MetaObjectIDMachineIDSource);
				entity.MetaObjectID = entity.MetaObjectIDMachineIDSource.pkid;
				entity.MachineID = entity.MetaObjectIDMachineIDSource.Machine;
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
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;GraphFileObjectColumn&gt;"/> class
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
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;GraphFileObjectColumn&gt;"/> class
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
	
	#region GraphFileObjectSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;GraphFileObjectColumn&gt;"/> class
	/// that is used exclusively with a <see cref="GraphFileObject"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class GraphFileObjectSortBuilder : SqlSortBuilder<GraphFileObjectColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the GraphFileObjectSqlSortBuilder class.
		/// </summary>
		public GraphFileObjectSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion GraphFileObjectSortBuilder
	
} // end namespace
