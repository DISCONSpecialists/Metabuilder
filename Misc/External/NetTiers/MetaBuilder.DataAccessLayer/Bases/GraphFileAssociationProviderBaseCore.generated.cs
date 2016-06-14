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
	/// This class is the base class for any <see cref="GraphFileAssociationProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract partial class GraphFileAssociationProviderBaseCore : EntityProviderBase<MetaBuilder.BusinessLogic.GraphFileAssociation, MetaBuilder.BusinessLogic.GraphFileAssociationKey>
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
		public override bool Delete(TransactionManager transactionManager, MetaBuilder.BusinessLogic.GraphFileAssociationKey key)
		{
			return Delete(transactionManager, key.GraphFileID, key.GraphFileMachine, key.ChildObjectMachine, key.CAid, key.ObjectID, key.ChildObjectID, key.ObjectMachine);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="graphFileID">. Primary Key.</param>
		/// <param name="graphFileMachine">. Primary Key.</param>
		/// <param name="childObjectMachine">. Primary Key.</param>
		/// <param name="cAid">. Primary Key.</param>
		/// <param name="objectID">. Primary Key.</param>
		/// <param name="childObjectID">. Primary Key.</param>
		/// <param name="objectMachine">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(System.Int32 graphFileID, System.String graphFileMachine, System.String childObjectMachine, System.Int32 cAid, System.Int32 objectID, System.Int32 childObjectID, System.String objectMachine)
		{
			return Delete(null, graphFileID, graphFileMachine, childObjectMachine, cAid, objectID, childObjectID, objectMachine);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="graphFileID">. Primary Key.</param>
		/// <param name="graphFileMachine">. Primary Key.</param>
		/// <param name="childObjectMachine">. Primary Key.</param>
		/// <param name="cAid">. Primary Key.</param>
		/// <param name="objectID">. Primary Key.</param>
		/// <param name="childObjectID">. Primary Key.</param>
		/// <param name="objectMachine">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(TransactionManager transactionManager, System.Int32 graphFileID, System.String graphFileMachine, System.String childObjectMachine, System.Int32 cAid, System.Int32 objectID, System.Int32 childObjectID, System.String objectMachine);		
		
		#endregion Delete Methods
		
		#region Get By Foreign Key Functions
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileAssociation_GraphFile key.
		///		FK_GraphFileAssociation_GraphFile Description: 
		/// </summary>
		/// <param name="graphFileID"></param>
		/// <param name="graphFileMachine"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<GraphFileAssociation> GetByGraphFileIDGraphFileMachine(System.Int32 graphFileID, System.String graphFileMachine)
		{
			int count = -1;
			return GetByGraphFileIDGraphFileMachine(graphFileID, graphFileMachine, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileAssociation_GraphFile key.
		///		FK_GraphFileAssociation_GraphFile Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="graphFileID"></param>
		/// <param name="graphFileMachine"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileAssociation objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<GraphFileAssociation> GetByGraphFileIDGraphFileMachine(TransactionManager transactionManager, System.Int32 graphFileID, System.String graphFileMachine)
		{
			int count = -1;
			return GetByGraphFileIDGraphFileMachine(transactionManager, graphFileID, graphFileMachine, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileAssociation_GraphFile key.
		///		FK_GraphFileAssociation_GraphFile Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="graphFileID"></param>
		/// <param name="graphFileMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<GraphFileAssociation> GetByGraphFileIDGraphFileMachine(TransactionManager transactionManager, System.Int32 graphFileID, System.String graphFileMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByGraphFileIDGraphFileMachine(transactionManager, graphFileID, graphFileMachine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileAssociation_GraphFile key.
		///		fKGraphFileAssociationGraphFile Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="graphFileID"></param>
		/// <param name="graphFileMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<GraphFileAssociation> GetByGraphFileIDGraphFileMachine(System.Int32 graphFileID, System.String graphFileMachine, int start, int pageLength)
		{
			int count =  -1;
			return GetByGraphFileIDGraphFileMachine(null, graphFileID, graphFileMachine, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileAssociation_GraphFile key.
		///		fKGraphFileAssociationGraphFile Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="graphFileID"></param>
		/// <param name="graphFileMachine"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<GraphFileAssociation> GetByGraphFileIDGraphFileMachine(System.Int32 graphFileID, System.String graphFileMachine, int start, int pageLength,out int count)
		{
			return GetByGraphFileIDGraphFileMachine(null, graphFileID, graphFileMachine, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileAssociation_GraphFile key.
		///		FK_GraphFileAssociation_GraphFile Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="graphFileID"></param>
		/// <param name="graphFileMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileAssociation objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<GraphFileAssociation> GetByGraphFileIDGraphFileMachine(TransactionManager transactionManager, System.Int32 graphFileID, System.String graphFileMachine, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileAssociation_ObjectAssociation key.
		///		FK_GraphFileAssociation_ObjectAssociation Description: 
		/// </summary>
		/// <param name="cAid"></param>
		/// <param name="objectID"></param>
		/// <param name="childObjectID"></param>
		/// <param name="objectMachine"></param>
		/// <param name="childObjectMachine"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<GraphFileAssociation> GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(System.Int32 cAid, System.Int32 objectID, System.Int32 childObjectID, System.String objectMachine, System.String childObjectMachine)
		{
			int count = -1;
			return GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(cAid, objectID, childObjectID, objectMachine, childObjectMachine, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileAssociation_ObjectAssociation key.
		///		FK_GraphFileAssociation_ObjectAssociation Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="cAid"></param>
		/// <param name="objectID"></param>
		/// <param name="childObjectID"></param>
		/// <param name="objectMachine"></param>
		/// <param name="childObjectMachine"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileAssociation objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<GraphFileAssociation> GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(TransactionManager transactionManager, System.Int32 cAid, System.Int32 objectID, System.Int32 childObjectID, System.String objectMachine, System.String childObjectMachine)
		{
			int count = -1;
			return GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(transactionManager, cAid, objectID, childObjectID, objectMachine, childObjectMachine, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileAssociation_ObjectAssociation key.
		///		FK_GraphFileAssociation_ObjectAssociation Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="cAid"></param>
		/// <param name="objectID"></param>
		/// <param name="childObjectID"></param>
		/// <param name="objectMachine"></param>
		/// <param name="childObjectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<GraphFileAssociation> GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(TransactionManager transactionManager, System.Int32 cAid, System.Int32 objectID, System.Int32 childObjectID, System.String objectMachine, System.String childObjectMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(transactionManager, cAid, objectID, childObjectID, objectMachine, childObjectMachine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileAssociation_ObjectAssociation key.
		///		fKGraphFileAssociationObjectAssociation Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="cAid"></param>
		/// <param name="objectID"></param>
		/// <param name="childObjectID"></param>
		/// <param name="objectMachine"></param>
		/// <param name="childObjectMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<GraphFileAssociation> GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(System.Int32 cAid, System.Int32 objectID, System.Int32 childObjectID, System.String objectMachine, System.String childObjectMachine, int start, int pageLength)
		{
			int count =  -1;
			return GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(null, cAid, objectID, childObjectID, objectMachine, childObjectMachine, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileAssociation_ObjectAssociation key.
		///		fKGraphFileAssociationObjectAssociation Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="cAid"></param>
		/// <param name="objectID"></param>
		/// <param name="childObjectID"></param>
		/// <param name="objectMachine"></param>
		/// <param name="childObjectMachine"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<GraphFileAssociation> GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(System.Int32 cAid, System.Int32 objectID, System.Int32 childObjectID, System.String objectMachine, System.String childObjectMachine, int start, int pageLength,out int count)
		{
			return GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(null, cAid, objectID, childObjectID, objectMachine, childObjectMachine, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFileAssociation_ObjectAssociation key.
		///		FK_GraphFileAssociation_ObjectAssociation Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="cAid"></param>
		/// <param name="objectID"></param>
		/// <param name="childObjectID"></param>
		/// <param name="objectMachine"></param>
		/// <param name="childObjectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFileAssociation objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<GraphFileAssociation> GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(TransactionManager transactionManager, System.Int32 cAid, System.Int32 objectID, System.Int32 childObjectID, System.String objectMachine, System.String childObjectMachine, int start, int pageLength, out int count);
		
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
		public override MetaBuilder.BusinessLogic.GraphFileAssociation Get(TransactionManager transactionManager, MetaBuilder.BusinessLogic.GraphFileAssociationKey key, int start, int pageLength)
		{
			return GetByGraphFileIDGraphFileMachineChildObjectMachineCAidObjectIDChildObjectIDObjectMachine(transactionManager, key.GraphFileID, key.GraphFileMachine, key.ChildObjectMachine, key.CAid, key.ObjectID, key.ChildObjectID, key.ObjectMachine, start, pageLength);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key PK_GraphFileAssociation index.
		/// </summary>
		/// <param name="graphFileID"></param>
		/// <param name="graphFileMachine"></param>
		/// <param name="childObjectMachine"></param>
		/// <param name="cAid"></param>
		/// <param name="objectID"></param>
		/// <param name="childObjectID"></param>
		/// <param name="objectMachine"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.GraphFileAssociation"/> class.</returns>
		public MetaBuilder.BusinessLogic.GraphFileAssociation GetByGraphFileIDGraphFileMachineChildObjectMachineCAidObjectIDChildObjectIDObjectMachine(System.Int32 graphFileID, System.String graphFileMachine, System.String childObjectMachine, System.Int32 cAid, System.Int32 objectID, System.Int32 childObjectID, System.String objectMachine)
		{
			int count = -1;
			return GetByGraphFileIDGraphFileMachineChildObjectMachineCAidObjectIDChildObjectIDObjectMachine(null,graphFileID, graphFileMachine, childObjectMachine, cAid, objectID, childObjectID, objectMachine, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_GraphFileAssociation index.
		/// </summary>
		/// <param name="graphFileID"></param>
		/// <param name="graphFileMachine"></param>
		/// <param name="childObjectMachine"></param>
		/// <param name="cAid"></param>
		/// <param name="objectID"></param>
		/// <param name="childObjectID"></param>
		/// <param name="objectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.GraphFileAssociation"/> class.</returns>
		public MetaBuilder.BusinessLogic.GraphFileAssociation GetByGraphFileIDGraphFileMachineChildObjectMachineCAidObjectIDChildObjectIDObjectMachine(System.Int32 graphFileID, System.String graphFileMachine, System.String childObjectMachine, System.Int32 cAid, System.Int32 objectID, System.Int32 childObjectID, System.String objectMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByGraphFileIDGraphFileMachineChildObjectMachineCAidObjectIDChildObjectIDObjectMachine(null, graphFileID, graphFileMachine, childObjectMachine, cAid, objectID, childObjectID, objectMachine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_GraphFileAssociation index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="graphFileID"></param>
		/// <param name="graphFileMachine"></param>
		/// <param name="childObjectMachine"></param>
		/// <param name="cAid"></param>
		/// <param name="objectID"></param>
		/// <param name="childObjectID"></param>
		/// <param name="objectMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.GraphFileAssociation"/> class.</returns>
		public MetaBuilder.BusinessLogic.GraphFileAssociation GetByGraphFileIDGraphFileMachineChildObjectMachineCAidObjectIDChildObjectIDObjectMachine(TransactionManager transactionManager, System.Int32 graphFileID, System.String graphFileMachine, System.String childObjectMachine, System.Int32 cAid, System.Int32 objectID, System.Int32 childObjectID, System.String objectMachine)
		{
			int count = -1;
			return GetByGraphFileIDGraphFileMachineChildObjectMachineCAidObjectIDChildObjectIDObjectMachine(transactionManager, graphFileID, graphFileMachine, childObjectMachine, cAid, objectID, childObjectID, objectMachine, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_GraphFileAssociation index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="graphFileID"></param>
		/// <param name="graphFileMachine"></param>
		/// <param name="childObjectMachine"></param>
		/// <param name="cAid"></param>
		/// <param name="objectID"></param>
		/// <param name="childObjectID"></param>
		/// <param name="objectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.GraphFileAssociation"/> class.</returns>
		public MetaBuilder.BusinessLogic.GraphFileAssociation GetByGraphFileIDGraphFileMachineChildObjectMachineCAidObjectIDChildObjectIDObjectMachine(TransactionManager transactionManager, System.Int32 graphFileID, System.String graphFileMachine, System.String childObjectMachine, System.Int32 cAid, System.Int32 objectID, System.Int32 childObjectID, System.String objectMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByGraphFileIDGraphFileMachineChildObjectMachineCAidObjectIDChildObjectIDObjectMachine(transactionManager, graphFileID, graphFileMachine, childObjectMachine, cAid, objectID, childObjectID, objectMachine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_GraphFileAssociation index.
		/// </summary>
		/// <param name="graphFileID"></param>
		/// <param name="graphFileMachine"></param>
		/// <param name="childObjectMachine"></param>
		/// <param name="cAid"></param>
		/// <param name="objectID"></param>
		/// <param name="childObjectID"></param>
		/// <param name="objectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.GraphFileAssociation"/> class.</returns>
		public MetaBuilder.BusinessLogic.GraphFileAssociation GetByGraphFileIDGraphFileMachineChildObjectMachineCAidObjectIDChildObjectIDObjectMachine(System.Int32 graphFileID, System.String graphFileMachine, System.String childObjectMachine, System.Int32 cAid, System.Int32 objectID, System.Int32 childObjectID, System.String objectMachine, int start, int pageLength, out int count)
		{
			return GetByGraphFileIDGraphFileMachineChildObjectMachineCAidObjectIDChildObjectIDObjectMachine(null, graphFileID, graphFileMachine, childObjectMachine, cAid, objectID, childObjectID, objectMachine, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_GraphFileAssociation index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="graphFileID"></param>
		/// <param name="graphFileMachine"></param>
		/// <param name="childObjectMachine"></param>
		/// <param name="cAid"></param>
		/// <param name="objectID"></param>
		/// <param name="childObjectID"></param>
		/// <param name="objectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.GraphFileAssociation"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.GraphFileAssociation GetByGraphFileIDGraphFileMachineChildObjectMachineCAidObjectIDChildObjectIDObjectMachine(TransactionManager transactionManager, System.Int32 graphFileID, System.String graphFileMachine, System.String childObjectMachine, System.Int32 cAid, System.Int32 objectID, System.Int32 childObjectID, System.String objectMachine, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a MetaBuilder.BusinessLogic.TList&lt;GraphFileAssociation&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="MetaBuilder.BusinessLogic.TList&lt;GraphFileAssociation&gt;"/></returns>
		public static MetaBuilder.BusinessLogic.TList<GraphFileAssociation> Fill(IDataReader reader, MetaBuilder.BusinessLogic.TList<GraphFileAssociation> rows, int start, int pageLength)
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
				
				MetaBuilder.BusinessLogic.GraphFileAssociation c = null;
				if (DataRepository.Provider.UseEntityFactory)
				{
					key = @"GraphFileAssociation" 
							+ (reader.IsDBNull(reader.GetOrdinal("GraphFileID"))?(int)0:(System.Int32)reader["GraphFileID"]).ToString()
							+ (reader.IsDBNull(reader.GetOrdinal("GraphFileMachine"))?string.Empty:(System.String)reader["GraphFileMachine"]).ToString()
							+ (reader.IsDBNull(reader.GetOrdinal("ChildObjectMachine"))?string.Empty:(System.String)reader["ChildObjectMachine"]).ToString()
							+ (reader.IsDBNull(reader.GetOrdinal("CAid"))?(int)0:(System.Int32)reader["CAid"]).ToString()
							+ (reader.IsDBNull(reader.GetOrdinal("ObjectID"))?(int)0:(System.Int32)reader["ObjectID"]).ToString()
							+ (reader.IsDBNull(reader.GetOrdinal("ChildObjectID"))?(int)0:(System.Int32)reader["ChildObjectID"]).ToString()
							+ (reader.IsDBNull(reader.GetOrdinal("ObjectMachine"))?string.Empty:(System.String)reader["ObjectMachine"]).ToString();

					c = EntityManager.LocateOrCreate<GraphFileAssociation>(
						key.ToString(), // EntityTrackingKey 
						"GraphFileAssociation",  //Creational Type
						DataRepository.Provider.EntityCreationalFactoryType,  //Factory used to create entity
						DataRepository.Provider.EnableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.GraphFileAssociation();
				}
				
				if (!DataRepository.Provider.EnableEntityTracking || c.EntityState == EntityState.Added)
                {
					c.SuppressEntityEvents = true;
					c.GraphFileID = (System.Int32)reader["GraphFileID"];
					c.OriginalGraphFileID = c.GraphFileID; //(reader.IsDBNull(reader.GetOrdinal("GraphFileID")))?(int)0:(System.Int32)reader["GraphFileID"];
					c.GraphFileMachine = (System.String)reader["GraphFileMachine"];
					c.OriginalGraphFileMachine = c.GraphFileMachine; //(reader.IsDBNull(reader.GetOrdinal("GraphFileMachine")))?string.Empty:(System.String)reader["GraphFileMachine"];
					c.ChildObjectMachine = (System.String)reader["ChildObjectMachine"];
					c.OriginalChildObjectMachine = c.ChildObjectMachine; //(reader.IsDBNull(reader.GetOrdinal("ChildObjectMachine")))?string.Empty:(System.String)reader["ChildObjectMachine"];
					c.CAid = (System.Int32)reader["CAid"];
					c.OriginalCAid = c.CAid; //(reader.IsDBNull(reader.GetOrdinal("CAid")))?(int)0:(System.Int32)reader["CAid"];
					c.ObjectID = (System.Int32)reader["ObjectID"];
					c.OriginalObjectID = c.ObjectID; //(reader.IsDBNull(reader.GetOrdinal("ObjectID")))?(int)0:(System.Int32)reader["ObjectID"];
					c.ChildObjectID = (System.Int32)reader["ChildObjectID"];
					c.OriginalChildObjectID = c.ChildObjectID; //(reader.IsDBNull(reader.GetOrdinal("ChildObjectID")))?(int)0:(System.Int32)reader["ChildObjectID"];
					c.ObjectMachine = (System.String)reader["ObjectMachine"];
					c.OriginalObjectMachine = c.ObjectMachine; //(reader.IsDBNull(reader.GetOrdinal("ObjectMachine")))?string.Empty:(System.String)reader["ObjectMachine"];
					c.EntityTrackingKey = key;
					c.AcceptChanges();
					c.SuppressEntityEvents = false;
				}
				rows.Add(c);
			}
			return rows;
		}
		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.GraphFileAssociation"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.GraphFileAssociation"/> object to refresh.</param>
		public static void RefreshEntity(IDataReader reader, MetaBuilder.BusinessLogic.GraphFileAssociation entity)
		{
			if (!reader.Read()) return;
			
			entity.GraphFileID = (System.Int32)reader["GraphFileID"];
			entity.OriginalGraphFileID = (System.Int32)reader["GraphFileID"];
			entity.GraphFileMachine = (System.String)reader["GraphFileMachine"];
			entity.OriginalGraphFileMachine = (System.String)reader["GraphFileMachine"];
			entity.ChildObjectMachine = (System.String)reader["ChildObjectMachine"];
			entity.OriginalChildObjectMachine = (System.String)reader["ChildObjectMachine"];
			entity.CAid = (System.Int32)reader["CAid"];
			entity.OriginalCAid = (System.Int32)reader["CAid"];
			entity.ObjectID = (System.Int32)reader["ObjectID"];
			entity.OriginalObjectID = (System.Int32)reader["ObjectID"];
			entity.ChildObjectID = (System.Int32)reader["ChildObjectID"];
			entity.OriginalChildObjectID = (System.Int32)reader["ChildObjectID"];
			entity.ObjectMachine = (System.String)reader["ObjectMachine"];
			entity.OriginalObjectMachine = (System.String)reader["ObjectMachine"];
			entity.AcceptChanges();
		}
		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.GraphFileAssociation"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.GraphFileAssociation"/> object.</param>
		public static void RefreshEntity(DataSet dataSet, MetaBuilder.BusinessLogic.GraphFileAssociation entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.GraphFileID = (System.Int32)dataRow["GraphFileID"];
			entity.OriginalGraphFileID = (System.Int32)dataRow["GraphFileID"];
			entity.GraphFileMachine = (System.String)dataRow["GraphFileMachine"];
			entity.OriginalGraphFileMachine = (System.String)dataRow["GraphFileMachine"];
			entity.ChildObjectMachine = (System.String)dataRow["ChildObjectMachine"];
			entity.OriginalChildObjectMachine = (System.String)dataRow["ChildObjectMachine"];
			entity.CAid = (System.Int32)dataRow["CAid"];
			entity.OriginalCAid = (System.Int32)dataRow["CAid"];
			entity.ObjectID = (System.Int32)dataRow["ObjectID"];
			entity.OriginalObjectID = (System.Int32)dataRow["ObjectID"];
			entity.ChildObjectID = (System.Int32)dataRow["ChildObjectID"];
			entity.OriginalChildObjectID = (System.Int32)dataRow["ChildObjectID"];
			entity.ObjectMachine = (System.String)dataRow["ObjectMachine"];
			entity.OriginalObjectMachine = (System.String)dataRow["ObjectMachine"];
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
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.GraphFileAssociation"/> object to load.</param>
		/// <param name="deep">Boolean. A flag that indicates whether to recursively save all Property Collection that are descendants of this instance. If True, saves the complete object graph below this object. If False, saves this object only. </param>
		/// <param name="deepLoadType">DeepLoadType Enumeration to Include/Exclude object property collections from Load.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.GraphFileAssociation Property Collection Type Array To Include or Exclude from Load</param>
		/// <param name="innerList">A collection of child types for easy access.</param>
	    /// <exception cref="ArgumentNullException">entity or childTypes is null.</exception>
	    /// <exception cref="ArgumentException">deepLoadType has invalid value.</exception>
		internal override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.GraphFileAssociation entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, ChildEntityTypesList innerList)
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

			#region CAidObjectIDChildObjectIDObjectMachineChildObjectMachineSource	
			if (CanDeepLoad(entity, "ObjectAssociation", "CAidObjectIDChildObjectIDObjectMachineChildObjectMachineSource", deepLoadType, innerList) 
				&& entity.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineSource == null)
			{
				object[] pkItems = new object[5];
				pkItems[0] = entity.CAid;
				pkItems[1] = entity.ObjectID;
				pkItems[2] = entity.ChildObjectID;
				pkItems[3] = entity.ObjectMachine;
				pkItems[4] = entity.ChildObjectMachine;
				ObjectAssociation tmpEntity = EntityManager.LocateEntity<ObjectAssociation>(EntityLocator.ConstructKeyFromPkItems(typeof(ObjectAssociation), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineSource = tmpEntity;
				else
					entity.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineSource = DataRepository.ObjectAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(entity.CAid, entity.ObjectID, entity.ChildObjectID, entity.ObjectMachine, entity.ChildObjectMachine);
			
				if (deep && entity.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineSource != null)
				{
					DataRepository.ObjectAssociationProvider.DeepLoad(transactionManager, entity.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineSource, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion CAidObjectIDChildObjectIDObjectMachineChildObjectMachineSource
			
			// Load Entity through Provider
		}
		
		#endregion 
		
		#region DeepSave Methods

		/// <summary>
		/// Deep Save the entire object graph of the MetaBuilder.BusinessLogic.GraphFileAssociation object with criteria based of the child 
		/// Type property array and DeepSaveType.
		/// </summary>
		/// <param name="transactionManager">The transaction manager.</param>
		/// <param name="entity">MetaBuilder.BusinessLogic.GraphFileAssociation instance</param>
		/// <param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.GraphFileAssociation Property Collection Type Array To Include or Exclude from Save</param>
		/// <param name="innerList">A Hashtable of child types for easy access.</param>
		internal override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.GraphFileAssociation entity, DeepSaveType deepSaveType, System.Type[] childTypes, ChildEntityTypesList innerList)
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
			
			#region CAidObjectIDChildObjectIDObjectMachineChildObjectMachineSource
			if (CanDeepSave(entity, "ObjectAssociation", "CAidObjectIDChildObjectIDObjectMachineChildObjectMachineSource", deepSaveType, innerList) 
				&& entity.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineSource != null)
			{
				DataRepository.ObjectAssociationProvider.Save(transactionManager, entity.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineSource);
				entity.CAid = entity.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineSource.CAid;
				entity.ObjectID = entity.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineSource.ObjectID;
				entity.ChildObjectID = entity.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineSource.ChildObjectID;
				entity.ObjectMachine = entity.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineSource.ObjectMachine;
				entity.ChildObjectMachine = entity.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineSource.ChildObjectMachine;
			}
			#endregion 
			#endregion Composite Parent Properties

			// Save Root Entity through Provider
			this.Save(transactionManager, entity);
			
			
						
			return true;
		}
		#endregion
	} // end class
	
	#region GraphFileAssociationChildEntityTypes
	
	///<summary>
	/// Enumeration used to expose the different child entity types 
	/// for child properties in <c>MetaBuilder.BusinessLogic.GraphFileAssociation</c>
	///</summary>
	public enum GraphFileAssociationChildEntityTypes
	{
		
		///<summary>
		/// Composite Property for <c>GraphFile</c> at GraphFileIDGraphFileMachineSource
		///</summary>
		[ChildEntityType(typeof(GraphFile))]
		GraphFile,
			
		///<summary>
		/// Composite Property for <c>ObjectAssociation</c> at CAidObjectIDChildObjectIDObjectMachineChildObjectMachineSource
		///</summary>
		[ChildEntityType(typeof(ObjectAssociation))]
		ObjectAssociation,
		}
	
	#endregion GraphFileAssociationChildEntityTypes
	
	#region GraphFileAssociationFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="GraphFileAssociation"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class GraphFileAssociationFilterBuilder : SqlFilterBuilder<GraphFileAssociationColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the GraphFileAssociationFilterBuilder class.
		/// </summary>
		public GraphFileAssociationFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the GraphFileAssociationFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public GraphFileAssociationFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the GraphFileAssociationFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public GraphFileAssociationFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion GraphFileAssociationFilterBuilder
	
	#region GraphFileAssociationParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="GraphFileAssociation"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class GraphFileAssociationParameterBuilder : ParameterizedSqlFilterBuilder<GraphFileAssociationColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the GraphFileAssociationParameterBuilder class.
		/// </summary>
		public GraphFileAssociationParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the GraphFileAssociationParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public GraphFileAssociationParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the GraphFileAssociationParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public GraphFileAssociationParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion GraphFileAssociationParameterBuilder
} // end namespace
