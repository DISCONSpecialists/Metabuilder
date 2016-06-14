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
		/// <param name="_artifactID">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(System.Int32 _artifactID)
		{
			return Delete(null, _artifactID);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_artifactID">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(TransactionManager transactionManager, System.Int32 _artifactID);		
		
		#endregion Delete Methods
		
		#region Get By Foreign Key Functions
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject key.
		///		FK_Artifact_MetaObject Description: 
		/// </summary>
		/// <param name="_objectID"></param>
		/// <param name="_objectMachine"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		public TList<Artifact> GetByObjectIDObjectMachine(System.Int32 _objectID, System.String _objectMachine)
		{
			int count = -1;
			return GetByObjectIDObjectMachine(_objectID, _objectMachine, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject key.
		///		FK_Artifact_MetaObject Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_objectID"></param>
		/// <param name="_objectMachine"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		/// <remarks></remarks>
		public TList<Artifact> GetByObjectIDObjectMachine(TransactionManager transactionManager, System.Int32 _objectID, System.String _objectMachine)
		{
			int count = -1;
			return GetByObjectIDObjectMachine(transactionManager, _objectID, _objectMachine, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject key.
		///		FK_Artifact_MetaObject Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_objectID"></param>
		/// <param name="_objectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		public TList<Artifact> GetByObjectIDObjectMachine(TransactionManager transactionManager, System.Int32 _objectID, System.String _objectMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByObjectIDObjectMachine(transactionManager, _objectID, _objectMachine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject key.
		///		fK_Artifact_MetaObject Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_objectID"></param>
		/// <param name="_objectMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		public TList<Artifact> GetByObjectIDObjectMachine(System.Int32 _objectID, System.String _objectMachine, int start, int pageLength)
		{
			int count =  -1;
			return GetByObjectIDObjectMachine(null, _objectID, _objectMachine, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject key.
		///		fK_Artifact_MetaObject Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_objectID"></param>
		/// <param name="_objectMachine"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		public TList<Artifact> GetByObjectIDObjectMachine(System.Int32 _objectID, System.String _objectMachine, int start, int pageLength,out int count)
		{
			return GetByObjectIDObjectMachine(null, _objectID, _objectMachine, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject key.
		///		FK_Artifact_MetaObject Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_objectID"></param>
		/// <param name="_objectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		public abstract TList<Artifact> GetByObjectIDObjectMachine(TransactionManager transactionManager, System.Int32 _objectID, System.String _objectMachine, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject1 key.
		///		FK_Artifact_MetaObject1 Description: 
		/// </summary>
		/// <param name="_childObjectID"></param>
		/// <param name="_childObjectMachine"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		public TList<Artifact> GetByChildObjectIDChildObjectMachine(System.Int32 _childObjectID, System.String _childObjectMachine)
		{
			int count = -1;
			return GetByChildObjectIDChildObjectMachine(_childObjectID, _childObjectMachine, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject1 key.
		///		FK_Artifact_MetaObject1 Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_childObjectID"></param>
		/// <param name="_childObjectMachine"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		/// <remarks></remarks>
		public TList<Artifact> GetByChildObjectIDChildObjectMachine(TransactionManager transactionManager, System.Int32 _childObjectID, System.String _childObjectMachine)
		{
			int count = -1;
			return GetByChildObjectIDChildObjectMachine(transactionManager, _childObjectID, _childObjectMachine, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject1 key.
		///		FK_Artifact_MetaObject1 Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_childObjectID"></param>
		/// <param name="_childObjectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		public TList<Artifact> GetByChildObjectIDChildObjectMachine(TransactionManager transactionManager, System.Int32 _childObjectID, System.String _childObjectMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByChildObjectIDChildObjectMachine(transactionManager, _childObjectID, _childObjectMachine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject1 key.
		///		fK_Artifact_MetaObject1 Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_childObjectID"></param>
		/// <param name="_childObjectMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		public TList<Artifact> GetByChildObjectIDChildObjectMachine(System.Int32 _childObjectID, System.String _childObjectMachine, int start, int pageLength)
		{
			int count =  -1;
			return GetByChildObjectIDChildObjectMachine(null, _childObjectID, _childObjectMachine, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject1 key.
		///		fK_Artifact_MetaObject1 Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_childObjectID"></param>
		/// <param name="_childObjectMachine"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		public TList<Artifact> GetByChildObjectIDChildObjectMachine(System.Int32 _childObjectID, System.String _childObjectMachine, int start, int pageLength,out int count)
		{
			return GetByChildObjectIDChildObjectMachine(null, _childObjectID, _childObjectMachine, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject1 key.
		///		FK_Artifact_MetaObject1 Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_childObjectID"></param>
		/// <param name="_childObjectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		public abstract TList<Artifact> GetByChildObjectIDChildObjectMachine(TransactionManager transactionManager, System.Int32 _childObjectID, System.String _childObjectMachine, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject2 key.
		///		FK_Artifact_MetaObject2 Description: 
		/// </summary>
		/// <param name="_artifactObjectID"></param>
		/// <param name="_artefactMachine"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		public TList<Artifact> GetByArtifactObjectIDArtefactMachine(System.Int32 _artifactObjectID, System.String _artefactMachine)
		{
			int count = -1;
			return GetByArtifactObjectIDArtefactMachine(_artifactObjectID, _artefactMachine, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject2 key.
		///		FK_Artifact_MetaObject2 Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_artifactObjectID"></param>
		/// <param name="_artefactMachine"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		/// <remarks></remarks>
		public TList<Artifact> GetByArtifactObjectIDArtefactMachine(TransactionManager transactionManager, System.Int32 _artifactObjectID, System.String _artefactMachine)
		{
			int count = -1;
			return GetByArtifactObjectIDArtefactMachine(transactionManager, _artifactObjectID, _artefactMachine, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject2 key.
		///		FK_Artifact_MetaObject2 Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_artifactObjectID"></param>
		/// <param name="_artefactMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		public TList<Artifact> GetByArtifactObjectIDArtefactMachine(TransactionManager transactionManager, System.Int32 _artifactObjectID, System.String _artefactMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByArtifactObjectIDArtefactMachine(transactionManager, _artifactObjectID, _artefactMachine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject2 key.
		///		fK_Artifact_MetaObject2 Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_artifactObjectID"></param>
		/// <param name="_artefactMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		public TList<Artifact> GetByArtifactObjectIDArtefactMachine(System.Int32 _artifactObjectID, System.String _artefactMachine, int start, int pageLength)
		{
			int count =  -1;
			return GetByArtifactObjectIDArtefactMachine(null, _artifactObjectID, _artefactMachine, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject2 key.
		///		fK_Artifact_MetaObject2 Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_artifactObjectID"></param>
		/// <param name="_artefactMachine"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		public TList<Artifact> GetByArtifactObjectIDArtefactMachine(System.Int32 _artifactObjectID, System.String _artefactMachine, int start, int pageLength,out int count)
		{
			return GetByArtifactObjectIDArtefactMachine(null, _artifactObjectID, _artefactMachine, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Artifact_MetaObject2 key.
		///		FK_Artifact_MetaObject2 Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_artifactObjectID"></param>
		/// <param name="_artefactMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Artifact objects.</returns>
		public abstract TList<Artifact> GetByArtifactObjectIDArtefactMachine(TransactionManager transactionManager, System.Int32 _artifactObjectID, System.String _artefactMachine, int start, int pageLength, out int count);
		
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
		/// <param name="_artifactID"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Artifact"/> class.</returns>
		public MetaBuilder.BusinessLogic.Artifact GetByArtifactID(System.Int32 _artifactID)
		{
			int count = -1;
			return GetByArtifactID(null,_artifactID, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Artifact index.
		/// </summary>
		/// <param name="_artifactID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Artifact"/> class.</returns>
		public MetaBuilder.BusinessLogic.Artifact GetByArtifactID(System.Int32 _artifactID, int start, int pageLength)
		{
			int count = -1;
			return GetByArtifactID(null, _artifactID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Artifact index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_artifactID"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Artifact"/> class.</returns>
		public MetaBuilder.BusinessLogic.Artifact GetByArtifactID(TransactionManager transactionManager, System.Int32 _artifactID)
		{
			int count = -1;
			return GetByArtifactID(transactionManager, _artifactID, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Artifact index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_artifactID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Artifact"/> class.</returns>
		public MetaBuilder.BusinessLogic.Artifact GetByArtifactID(TransactionManager transactionManager, System.Int32 _artifactID, int start, int pageLength)
		{
			int count = -1;
			return GetByArtifactID(transactionManager, _artifactID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Artifact index.
		/// </summary>
		/// <param name="_artifactID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Artifact"/> class.</returns>
		public MetaBuilder.BusinessLogic.Artifact GetByArtifactID(System.Int32 _artifactID, int start, int pageLength, out int count)
		{
			return GetByArtifactID(null, _artifactID, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Artifact index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_artifactID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Artifact"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.Artifact GetByArtifactID(TransactionManager transactionManager, System.Int32 _artifactID, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a TList&lt;Artifact&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="TList&lt;Artifact&gt;"/></returns>
		public static TList<Artifact> Fill(IDataReader reader, TList<Artifact> rows, int start, int pageLength)
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
				
				MetaBuilder.BusinessLogic.Artifact c = null;
				if (useEntityFactory)
				{
					key = new System.Text.StringBuilder("Artifact")
					.Append("|").Append((System.Int32)reader[((int)ArtifactColumn.ArtifactID - 1)]).ToString();
					c = EntityManager.LocateOrCreate<Artifact>(
					key.ToString(), // EntityTrackingKey
					"Artifact",  //Creational Type
					entityCreationFactoryType,  //Factory used to create entity
					enableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.Artifact();
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
					c.ArtifactID = (System.Int32)reader[((int)ArtifactColumn.ArtifactID - 1)];
					c.CAid = (System.Int32)reader[((int)ArtifactColumn.CAid - 1)];
					c.ObjectID = (System.Int32)reader[((int)ArtifactColumn.ObjectID - 1)];
					c.ChildObjectID = (System.Int32)reader[((int)ArtifactColumn.ChildObjectID - 1)];
					c.ArtifactObjectID = (System.Int32)reader[((int)ArtifactColumn.ArtifactObjectID - 1)];
					c.ObjectMachine = (System.String)reader[((int)ArtifactColumn.ObjectMachine - 1)];
					c.ChildObjectMachine = (System.String)reader[((int)ArtifactColumn.ChildObjectMachine - 1)];
					c.ArtefactMachine = (System.String)reader[((int)ArtifactColumn.ArtefactMachine - 1)];
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
			
			entity.ArtifactID = (System.Int32)reader[((int)ArtifactColumn.ArtifactID - 1)];
			entity.CAid = (System.Int32)reader[((int)ArtifactColumn.CAid - 1)];
			entity.ObjectID = (System.Int32)reader[((int)ArtifactColumn.ObjectID - 1)];
			entity.ChildObjectID = (System.Int32)reader[((int)ArtifactColumn.ChildObjectID - 1)];
			entity.ArtifactObjectID = (System.Int32)reader[((int)ArtifactColumn.ArtifactObjectID - 1)];
			entity.ObjectMachine = (System.String)reader[((int)ArtifactColumn.ObjectMachine - 1)];
			entity.ChildObjectMachine = (System.String)reader[((int)ArtifactColumn.ChildObjectMachine - 1)];
			entity.ArtefactMachine = (System.String)reader[((int)ArtifactColumn.ArtefactMachine - 1)];
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
		public override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.Artifact entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, DeepSession innerList)
		{
			if(entity == null)
				return;

			#region ObjectIDObjectMachineSource	
			if (CanDeepLoad(entity, "MetaObject|ObjectIDObjectMachineSource", deepLoadType, innerList) 
				&& entity.ObjectIDObjectMachineSource == null)
			{
				object[] pkItems = new object[2];
				pkItems[0] = entity.ObjectID;
				pkItems[1] = entity.ObjectMachine;
				MetaObject tmpEntity = EntityManager.LocateEntity<MetaObject>(EntityLocator.ConstructKeyFromPkItems(typeof(MetaObject), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.ObjectIDObjectMachineSource = tmpEntity;
				else
					entity.ObjectIDObjectMachineSource = DataRepository.MetaObjectProvider.GetBypkidMachine(transactionManager, entity.ObjectID, entity.ObjectMachine);		
				
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'ObjectIDObjectMachineSource' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.ObjectIDObjectMachineSource != null)
				{
					innerList.SkipChildren = true;
					DataRepository.MetaObjectProvider.DeepLoad(transactionManager, entity.ObjectIDObjectMachineSource, deep, deepLoadType, childTypes, innerList);
					innerList.SkipChildren = false;
				}
					
			}
			#endregion ObjectIDObjectMachineSource

			#region ChildObjectIDChildObjectMachineSource	
			if (CanDeepLoad(entity, "MetaObject|ChildObjectIDChildObjectMachineSource", deepLoadType, innerList) 
				&& entity.ChildObjectIDChildObjectMachineSource == null)
			{
				object[] pkItems = new object[2];
				pkItems[0] = entity.ChildObjectID;
				pkItems[1] = entity.ChildObjectMachine;
				MetaObject tmpEntity = EntityManager.LocateEntity<MetaObject>(EntityLocator.ConstructKeyFromPkItems(typeof(MetaObject), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.ChildObjectIDChildObjectMachineSource = tmpEntity;
				else
					entity.ChildObjectIDChildObjectMachineSource = DataRepository.MetaObjectProvider.GetBypkidMachine(transactionManager, entity.ChildObjectID, entity.ChildObjectMachine);		
				
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'ChildObjectIDChildObjectMachineSource' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.ChildObjectIDChildObjectMachineSource != null)
				{
					innerList.SkipChildren = true;
					DataRepository.MetaObjectProvider.DeepLoad(transactionManager, entity.ChildObjectIDChildObjectMachineSource, deep, deepLoadType, childTypes, innerList);
					innerList.SkipChildren = false;
				}
					
			}
			#endregion ChildObjectIDChildObjectMachineSource

			#region ArtifactObjectIDArtefactMachineSource	
			if (CanDeepLoad(entity, "MetaObject|ArtifactObjectIDArtefactMachineSource", deepLoadType, innerList) 
				&& entity.ArtifactObjectIDArtefactMachineSource == null)
			{
				object[] pkItems = new object[2];
				pkItems[0] = entity.ArtifactObjectID;
				pkItems[1] = entity.ArtefactMachine;
				MetaObject tmpEntity = EntityManager.LocateEntity<MetaObject>(EntityLocator.ConstructKeyFromPkItems(typeof(MetaObject), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.ArtifactObjectIDArtefactMachineSource = tmpEntity;
				else
					entity.ArtifactObjectIDArtefactMachineSource = DataRepository.MetaObjectProvider.GetBypkidMachine(transactionManager, entity.ArtifactObjectID, entity.ArtefactMachine);		
				
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'ArtifactObjectIDArtefactMachineSource' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.ArtifactObjectIDArtefactMachineSource != null)
				{
					innerList.SkipChildren = true;
					DataRepository.MetaObjectProvider.DeepLoad(transactionManager, entity.ArtifactObjectIDArtefactMachineSource, deep, deepLoadType, childTypes, innerList);
					innerList.SkipChildren = false;
				}
					
			}
			#endregion ArtifactObjectIDArtefactMachineSource
			
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
		/// Deep Save the entire object graph of the MetaBuilder.BusinessLogic.Artifact object with criteria based of the child 
		/// Type property array and DeepSaveType.
		/// </summary>
		/// <param name="transactionManager">The transaction manager.</param>
		/// <param name="entity">MetaBuilder.BusinessLogic.Artifact instance</param>
		/// <param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.Artifact Property Collection Type Array To Include or Exclude from Save</param>
		/// <param name="innerList">A Hashtable of child types for easy access.</param>
		public override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.Artifact entity, DeepSaveType deepSaveType, System.Type[] childTypes, DeepSession innerList)
		{	
			if (entity == null)
				return false;
							
			#region Composite Parent Properties
			//Save Source Composite Properties, however, don't call deep save on them.  
			//So they only get saved a single level deep.
			
			#region ObjectIDObjectMachineSource
			if (CanDeepSave(entity, "MetaObject|ObjectIDObjectMachineSource", deepSaveType, innerList) 
				&& entity.ObjectIDObjectMachineSource != null)
			{
				DataRepository.MetaObjectProvider.Save(transactionManager, entity.ObjectIDObjectMachineSource);
				entity.ObjectID = entity.ObjectIDObjectMachineSource.pkid;
				entity.ObjectMachine = entity.ObjectIDObjectMachineSource.Machine;
			}
			#endregion 
			
			#region ChildObjectIDChildObjectMachineSource
			if (CanDeepSave(entity, "MetaObject|ChildObjectIDChildObjectMachineSource", deepSaveType, innerList) 
				&& entity.ChildObjectIDChildObjectMachineSource != null)
			{
				DataRepository.MetaObjectProvider.Save(transactionManager, entity.ChildObjectIDChildObjectMachineSource);
				entity.ChildObjectID = entity.ChildObjectIDChildObjectMachineSource.pkid;
				entity.ChildObjectMachine = entity.ChildObjectIDChildObjectMachineSource.Machine;
			}
			#endregion 
			
			#region ArtifactObjectIDArtefactMachineSource
			if (CanDeepSave(entity, "MetaObject|ArtifactObjectIDArtefactMachineSource", deepSaveType, innerList) 
				&& entity.ArtifactObjectIDArtefactMachineSource != null)
			{
				DataRepository.MetaObjectProvider.Save(transactionManager, entity.ArtifactObjectIDArtefactMachineSource);
				entity.ArtifactObjectID = entity.ArtifactObjectIDArtefactMachineSource.pkid;
				entity.ArtefactMachine = entity.ArtifactObjectIDArtefactMachineSource.Machine;
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
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;ArtifactColumn&gt;"/> class
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
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;ArtifactColumn&gt;"/> class
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
	
	#region ArtifactSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;ArtifactColumn&gt;"/> class
	/// that is used exclusively with a <see cref="Artifact"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class ArtifactSortBuilder : SqlSortBuilder<ArtifactColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the ArtifactSqlSortBuilder class.
		/// </summary>
		public ArtifactSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion ArtifactSortBuilder
	
} // end namespace
