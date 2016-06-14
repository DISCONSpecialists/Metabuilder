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
	/// This class is the base class for any <see cref="ArtifactProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract partial class ArtifactProviderBaseCore : EntityProviderBase<MetaBuilder.BusinessLogic.Artifact, MetaBuilder.BusinessLogic.ArtifactKey>
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
		public override bool Delete(TransactionManager transactionManager, MetaBuilder.BusinessLogic.ArtifactKey key)
		{
			return Delete(transactionManager, key.ArtifactID);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="artifactID">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(System.Int32 artifactID)
		{
			return Delete(null, artifactID);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="artifactID">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(TransactionManager transactionManager, System.Int32 artifactID);		
		
		#endregion Delete Methods
		
		#region Get By Foreign Key Functions
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject key.
		///		FK_Artifact_MetaObject Description: 
		/// </summary>
		/// <param name="objectID"></param>
		/// <param name="objectMachine"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		public MetaBuilder.BusinessLogic.TList<Artifact> GetByObjectIDObjectMachine(System.Int32 objectID, System.String objectMachine)
		{
			int count = -1;
			return GetByObjectIDObjectMachine(objectID, objectMachine, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject key.
		///		FK_Artifact_MetaObject Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="objectID"></param>
		/// <param name="objectMachine"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<Artifact> GetByObjectIDObjectMachine(TransactionManager transactionManager, System.Int32 objectID, System.String objectMachine)
		{
			int count = -1;
			return GetByObjectIDObjectMachine(transactionManager, objectID, objectMachine, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject key.
		///		FK_Artifact_MetaObject Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="objectID"></param>
		/// <param name="objectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		public MetaBuilder.BusinessLogic.TList<Artifact> GetByObjectIDObjectMachine(TransactionManager transactionManager, System.Int32 objectID, System.String objectMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByObjectIDObjectMachine(transactionManager, objectID, objectMachine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject key.
		///		fKArtifactMetaObject Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="objectID"></param>
		/// <param name="objectMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		public MetaBuilder.BusinessLogic.TList<Artifact> GetByObjectIDObjectMachine(System.Int32 objectID, System.String objectMachine, int start, int pageLength)
		{
			int count =  -1;
			return GetByObjectIDObjectMachine(null, objectID, objectMachine, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject key.
		///		fKArtifactMetaObject Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="objectID"></param>
		/// <param name="objectMachine"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		public MetaBuilder.BusinessLogic.TList<Artifact> GetByObjectIDObjectMachine(System.Int32 objectID, System.String objectMachine, int start, int pageLength,out int count)
		{
			return GetByObjectIDObjectMachine(null, objectID, objectMachine, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject key.
		///		FK_Artifact_MetaObject Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="objectID"></param>
		/// <param name="objectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<Artifact> GetByObjectIDObjectMachine(TransactionManager transactionManager, System.Int32 objectID, System.String objectMachine, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject1 key.
		///		FK_Artifact_MetaObject1 Description: 
		/// </summary>
		/// <param name="childObjectID"></param>
		/// <param name="childObjectMachine"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		public MetaBuilder.BusinessLogic.TList<Artifact> GetByChildObjectIDChildObjectMachine(System.Int32 childObjectID, System.String childObjectMachine)
		{
			int count = -1;
			return GetByChildObjectIDChildObjectMachine(childObjectID, childObjectMachine, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject1 key.
		///		FK_Artifact_MetaObject1 Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="childObjectID"></param>
		/// <param name="childObjectMachine"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<Artifact> GetByChildObjectIDChildObjectMachine(TransactionManager transactionManager, System.Int32 childObjectID, System.String childObjectMachine)
		{
			int count = -1;
			return GetByChildObjectIDChildObjectMachine(transactionManager, childObjectID, childObjectMachine, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject1 key.
		///		FK_Artifact_MetaObject1 Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="childObjectID"></param>
		/// <param name="childObjectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		public MetaBuilder.BusinessLogic.TList<Artifact> GetByChildObjectIDChildObjectMachine(TransactionManager transactionManager, System.Int32 childObjectID, System.String childObjectMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByChildObjectIDChildObjectMachine(transactionManager, childObjectID, childObjectMachine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject1 key.
		///		fKArtifactMetaObject1 Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="childObjectID"></param>
		/// <param name="childObjectMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		public MetaBuilder.BusinessLogic.TList<Artifact> GetByChildObjectIDChildObjectMachine(System.Int32 childObjectID, System.String childObjectMachine, int start, int pageLength)
		{
			int count =  -1;
			return GetByChildObjectIDChildObjectMachine(null, childObjectID, childObjectMachine, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject1 key.
		///		fKArtifactMetaObject1 Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="childObjectID"></param>
		/// <param name="childObjectMachine"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		public MetaBuilder.BusinessLogic.TList<Artifact> GetByChildObjectIDChildObjectMachine(System.Int32 childObjectID, System.String childObjectMachine, int start, int pageLength,out int count)
		{
			return GetByChildObjectIDChildObjectMachine(null, childObjectID, childObjectMachine, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject1 key.
		///		FK_Artifact_MetaObject1 Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="childObjectID"></param>
		/// <param name="childObjectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<Artifact> GetByChildObjectIDChildObjectMachine(TransactionManager transactionManager, System.Int32 childObjectID, System.String childObjectMachine, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject2 key.
		///		FK_Artifact_MetaObject2 Description: 
		/// </summary>
		/// <param name="artifactObjectID"></param>
		/// <param name="artefactMachine"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		public MetaBuilder.BusinessLogic.TList<Artifact> GetByArtifactObjectIDArtefactMachine(System.Int32 artifactObjectID, System.String artefactMachine)
		{
			int count = -1;
			return GetByArtifactObjectIDArtefactMachine(artifactObjectID, artefactMachine, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject2 key.
		///		FK_Artifact_MetaObject2 Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="artifactObjectID"></param>
		/// <param name="artefactMachine"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<Artifact> GetByArtifactObjectIDArtefactMachine(TransactionManager transactionManager, System.Int32 artifactObjectID, System.String artefactMachine)
		{
			int count = -1;
			return GetByArtifactObjectIDArtefactMachine(transactionManager, artifactObjectID, artefactMachine, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject2 key.
		///		FK_Artifact_MetaObject2 Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="artifactObjectID"></param>
		/// <param name="artefactMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		public MetaBuilder.BusinessLogic.TList<Artifact> GetByArtifactObjectIDArtefactMachine(TransactionManager transactionManager, System.Int32 artifactObjectID, System.String artefactMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByArtifactObjectIDArtefactMachine(transactionManager, artifactObjectID, artefactMachine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject2 key.
		///		fKArtifactMetaObject2 Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="artifactObjectID"></param>
		/// <param name="artefactMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		public MetaBuilder.BusinessLogic.TList<Artifact> GetByArtifactObjectIDArtefactMachine(System.Int32 artifactObjectID, System.String artefactMachine, int start, int pageLength)
		{
			int count =  -1;
			return GetByArtifactObjectIDArtefactMachine(null, artifactObjectID, artefactMachine, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject2 key.
		///		fKArtifactMetaObject2 Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="artifactObjectID"></param>
		/// <param name="artefactMachine"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		public MetaBuilder.BusinessLogic.TList<Artifact> GetByArtifactObjectIDArtefactMachine(System.Int32 artifactObjectID, System.String artefactMachine, int start, int pageLength,out int count)
		{
			return GetByArtifactObjectIDArtefactMachine(null, artifactObjectID, artefactMachine, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject2 key.
		///		FK_Artifact_MetaObject2 Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="artifactObjectID"></param>
		/// <param name="artefactMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<Artifact> GetByArtifactObjectIDArtefactMachine(TransactionManager transactionManager, System.Int32 artifactObjectID, System.String artefactMachine, int start, int pageLength, out int count);
		
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
		public override MetaBuilder.BusinessLogic.Artifact Get(TransactionManager transactionManager, MetaBuilder.BusinessLogic.ArtifactKey key, int start, int pageLength)
		{
			return GetByArtifactID(transactionManager, key.ArtifactID, start, pageLength);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key PK_Artifact index.
		/// </summary>
		/// <param name="artifactID"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Artifact"/> class.</returns>
		public MetaBuilder.BusinessLogic.Artifact GetByArtifactID(System.Int32 artifactID)
		{
			int count = -1;
			return GetByArtifactID(null,artifactID, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Artifact index.
		/// </summary>
		/// <param name="artifactID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Artifact"/> class.</returns>
		public MetaBuilder.BusinessLogic.Artifact GetByArtifactID(System.Int32 artifactID, int start, int pageLength)
		{
			int count = -1;
			return GetByArtifactID(null, artifactID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Artifact index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="artifactID"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Artifact"/> class.</returns>
		public MetaBuilder.BusinessLogic.Artifact GetByArtifactID(TransactionManager transactionManager, System.Int32 artifactID)
		{
			int count = -1;
			return GetByArtifactID(transactionManager, artifactID, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Artifact index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="artifactID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Artifact"/> class.</returns>
		public MetaBuilder.BusinessLogic.Artifact GetByArtifactID(TransactionManager transactionManager, System.Int32 artifactID, int start, int pageLength)
		{
			int count = -1;
			return GetByArtifactID(transactionManager, artifactID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Artifact index.
		/// </summary>
		/// <param name="artifactID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Artifact"/> class.</returns>
		public MetaBuilder.BusinessLogic.Artifact GetByArtifactID(System.Int32 artifactID, int start, int pageLength, out int count)
		{
			return GetByArtifactID(null, artifactID, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Artifact index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="artifactID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Artifact"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.Artifact GetByArtifactID(TransactionManager transactionManager, System.Int32 artifactID, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a MetaBuilder.BusinessLogic.TList&lt;Artifact&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="MetaBuilder.BusinessLogic.TList&lt;Artifact&gt;"/></returns>
		public static MetaBuilder.BusinessLogic.TList<Artifact> Fill(IDataReader reader, MetaBuilder.BusinessLogic.TList<Artifact> rows, int start, int pageLength)
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
				
				MetaBuilder.BusinessLogic.Artifact c = null;
				if (DataRepository.Provider.UseEntityFactory)
				{
					key = @"Artifact" 
							+ (reader.IsDBNull(reader.GetOrdinal("ArtifactID"))?(int)0:(System.Int32)reader["ArtifactID"]).ToString();

					c = EntityManager.LocateOrCreate<Artifact>(
						key.ToString(), // EntityTrackingKey 
						"Artifact",  //Creational Type
						DataRepository.Provider.EntityCreationalFactoryType,  //Factory used to create entity
						DataRepository.Provider.EnableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.Artifact();
				}
				
				if (!DataRepository.Provider.EnableEntityTracking || c.EntityState == EntityState.Added)
                {
					c.SuppressEntityEvents = true;
					c.ArtifactID = (System.Int32)reader["ArtifactID"];
					c.CAid = (System.Int32)reader["CAid"];
					c.ObjectID = (System.Int32)reader["ObjectID"];
					c.ChildObjectID = (System.Int32)reader["ChildObjectID"];
					c.ArtifactObjectID = (System.Int32)reader["ArtifactObjectID"];
					c.ObjectMachine = (System.String)reader["ObjectMachine"];
					c.ChildObjectMachine = (System.String)reader["ChildObjectMachine"];
					c.ArtefactMachine = (System.String)reader["ArtefactMachine"];
					c.EntityTrackingKey = key;
					c.AcceptChanges();
					c.SuppressEntityEvents = false;
				}
				rows.Add(c);
			}
			return rows;
		}
		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.Artifact"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.Artifact"/> object to refresh.</param>
		public static void RefreshEntity(IDataReader reader, MetaBuilder.BusinessLogic.Artifact entity)
		{
			if (!reader.Read()) return;
			
			entity.ArtifactID = (System.Int32)reader["ArtifactID"];
			entity.CAid = (System.Int32)reader["CAid"];
			entity.ObjectID = (System.Int32)reader["ObjectID"];
			entity.ChildObjectID = (System.Int32)reader["ChildObjectID"];
			entity.ArtifactObjectID = (System.Int32)reader["ArtifactObjectID"];
			entity.ObjectMachine = (System.String)reader["ObjectMachine"];
			entity.ChildObjectMachine = (System.String)reader["ChildObjectMachine"];
			entity.ArtefactMachine = (System.String)reader["ArtefactMachine"];
			entity.AcceptChanges();
		}
		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.Artifact"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.Artifact"/> object.</param>
		public static void RefreshEntity(DataSet dataSet, MetaBuilder.BusinessLogic.Artifact entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.ArtifactID = (System.Int32)dataRow["ArtifactID"];
			entity.CAid = (System.Int32)dataRow["CAid"];
			entity.ObjectID = (System.Int32)dataRow["ObjectID"];
			entity.ChildObjectID = (System.Int32)dataRow["ChildObjectID"];
			entity.ArtifactObjectID = (System.Int32)dataRow["ArtifactObjectID"];
			entity.ObjectMachine = (System.String)dataRow["ObjectMachine"];
			entity.ChildObjectMachine = (System.String)dataRow["ChildObjectMachine"];
			entity.ArtefactMachine = (System.String)dataRow["ArtefactMachine"];
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
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.Artifact"/> object to load.</param>
		/// <param name="deep">Boolean. A flag that indicates whether to recursively save all Property Collection that are descendants of this instance. If True, saves the complete object graph below this object. If False, saves this object only. </param>
		/// <param name="deepLoadType">DeepLoadType Enumeration to Include/Exclude object property collections from Load.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.Artifact Property Collection Type Array To Include or Exclude from Load</param>
		/// <param name="innerList">A collection of child types for easy access.</param>
	    /// <exception cref="ArgumentNullException">entity or childTypes is null.</exception>
	    /// <exception cref="ArgumentException">deepLoadType has invalid value.</exception>
		internal override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.Artifact entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, ChildEntityTypesList innerList)
		{
			if(entity == null)
				return;

			#region ObjectIDObjectMachineSource	
			if (CanDeepLoad(entity, "MetaObject", "ObjectIDObjectMachineSource", deepLoadType, innerList) 
				&& entity.ObjectIDObjectMachineSource == null)
			{
				object[] pkItems = new object[2];
				pkItems[0] = entity.ObjectID;
				pkItems[1] = entity.ObjectMachine;
				MetaObject tmpEntity = EntityManager.LocateEntity<MetaObject>(EntityLocator.ConstructKeyFromPkItems(typeof(MetaObject), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.ObjectIDObjectMachineSource = tmpEntity;
				else
					entity.ObjectIDObjectMachineSource = DataRepository.MetaObjectProvider.GetByPkidMachine(entity.ObjectID, entity.ObjectMachine);
			
				if (deep && entity.ObjectIDObjectMachineSource != null)
				{
					DataRepository.MetaObjectProvider.DeepLoad(transactionManager, entity.ObjectIDObjectMachineSource, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion ObjectIDObjectMachineSource

			#region ChildObjectIDChildObjectMachineSource	
			if (CanDeepLoad(entity, "MetaObject", "ChildObjectIDChildObjectMachineSource", deepLoadType, innerList) 
				&& entity.ChildObjectIDChildObjectMachineSource == null)
			{
				object[] pkItems = new object[2];
				pkItems[0] = entity.ChildObjectID;
				pkItems[1] = entity.ChildObjectMachine;
				MetaObject tmpEntity = EntityManager.LocateEntity<MetaObject>(EntityLocator.ConstructKeyFromPkItems(typeof(MetaObject), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.ChildObjectIDChildObjectMachineSource = tmpEntity;
				else
					entity.ChildObjectIDChildObjectMachineSource = DataRepository.MetaObjectProvider.GetByPkidMachine(entity.ChildObjectID, entity.ChildObjectMachine);
			
				if (deep && entity.ChildObjectIDChildObjectMachineSource != null)
				{
					DataRepository.MetaObjectProvider.DeepLoad(transactionManager, entity.ChildObjectIDChildObjectMachineSource, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion ChildObjectIDChildObjectMachineSource

			#region ArtifactObjectIDArtefactMachineSource	
			if (CanDeepLoad(entity, "MetaObject", "ArtifactObjectIDArtefactMachineSource", deepLoadType, innerList) 
				&& entity.ArtifactObjectIDArtefactMachineSource == null)
			{
				object[] pkItems = new object[2];
				pkItems[0] = entity.ArtifactObjectID;
				pkItems[1] = entity.ArtefactMachine;
				MetaObject tmpEntity = EntityManager.LocateEntity<MetaObject>(EntityLocator.ConstructKeyFromPkItems(typeof(MetaObject), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.ArtifactObjectIDArtefactMachineSource = tmpEntity;
				else
					entity.ArtifactObjectIDArtefactMachineSource = DataRepository.MetaObjectProvider.GetByPkidMachine(entity.ArtifactObjectID, entity.ArtefactMachine);
			
				if (deep && entity.ArtifactObjectIDArtefactMachineSource != null)
				{
					DataRepository.MetaObjectProvider.DeepLoad(transactionManager, entity.ArtifactObjectIDArtefactMachineSource, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion ArtifactObjectIDArtefactMachineSource
			
			// Load Entity through Provider
		}
		
		#endregion 
		
		#region DeepSave Methods

		/// <summary>
		/// Deep Save the entire object graph of the MetaBuilder.BusinessLogic.Artifact object with criteria based of the child 
		/// Type property array and DeepSaveType.
		/// </summary>
		/// <param name="transactionManager">The transaction manager.</param>
		/// <param name="entity">MetaBuilder.BusinessLogic.Artifact instance</param>
		/// <param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.Artifact Property Collection Type Array To Include or Exclude from Save</param>
		/// <param name="innerList">A Hashtable of child types for easy access.</param>
		internal override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.Artifact entity, DeepSaveType deepSaveType, System.Type[] childTypes, ChildEntityTypesList innerList)
		{	
			if (entity == null)
				return false;
							
			#region Composite Parent Properties
			//Save Source Composite Properties, however, don't call deep save on them.  
			//So they only get saved a single level deep.
			
			#region ObjectIDObjectMachineSource
			if (CanDeepSave(entity, "MetaObject", "ObjectIDObjectMachineSource", deepSaveType, innerList) 
				&& entity.ObjectIDObjectMachineSource != null)
			{
				DataRepository.MetaObjectProvider.Save(transactionManager, entity.ObjectIDObjectMachineSource);
				entity.ObjectID = entity.ObjectIDObjectMachineSource.Pkid;
				entity.ObjectMachine = entity.ObjectIDObjectMachineSource.Machine;
			}
			#endregion 
			
			#region ChildObjectIDChildObjectMachineSource
			if (CanDeepSave(entity, "MetaObject", "ChildObjectIDChildObjectMachineSource", deepSaveType, innerList) 
				&& entity.ChildObjectIDChildObjectMachineSource != null)
			{
				DataRepository.MetaObjectProvider.Save(transactionManager, entity.ChildObjectIDChildObjectMachineSource);
				entity.ChildObjectID = entity.ChildObjectIDChildObjectMachineSource.Pkid;
				entity.ChildObjectMachine = entity.ChildObjectIDChildObjectMachineSource.Machine;
			}
			#endregion 
			
			#region ArtifactObjectIDArtefactMachineSource
			if (CanDeepSave(entity, "MetaObject", "ArtifactObjectIDArtefactMachineSource", deepSaveType, innerList) 
				&& entity.ArtifactObjectIDArtefactMachineSource != null)
			{
				DataRepository.MetaObjectProvider.Save(transactionManager, entity.ArtifactObjectIDArtefactMachineSource);
				entity.ArtifactObjectID = entity.ArtifactObjectIDArtefactMachineSource.Pkid;
				entity.ArtefactMachine = entity.ArtifactObjectIDArtefactMachineSource.Machine;
			}
			#endregion 
			#endregion Composite Parent Properties

			// Save Root Entity through Provider
			this.Save(transactionManager, entity);
			
			
						
			return true;
		}
		#endregion
	} // end class
	
	#region ArtifactChildEntityTypes
	
	///<summary>
	/// Enumeration used to expose the different child entity types 
	/// for child properties in <c>MetaBuilder.BusinessLogic.Artifact</c>
	///</summary>
	public enum ArtifactChildEntityTypes
	{
		
		///<summary>
		/// Composite Property for <c>MetaObject</c> at ObjectIDObjectMachineSource
		///</summary>
		[ChildEntityType(typeof(MetaObject))]
		MetaObject,
		}
	
	#endregion ArtifactChildEntityTypes
	
	#region ArtifactFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="Artifact"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class ArtifactFilterBuilder : SqlFilterBuilder<ArtifactColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the ArtifactFilterBuilder class.
		/// </summary>
		public ArtifactFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the ArtifactFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public ArtifactFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the ArtifactFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public ArtifactFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion ArtifactFilterBuilder
	
	#region ArtifactParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="Artifact"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class ArtifactParameterBuilder : ParameterizedSqlFilterBuilder<ArtifactColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the ArtifactParameterBuilder class.
		/// </summary>
		public ArtifactParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the ArtifactParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public ArtifactParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the ArtifactParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public ArtifactParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion ArtifactParameterBuilder
} // end namespace
