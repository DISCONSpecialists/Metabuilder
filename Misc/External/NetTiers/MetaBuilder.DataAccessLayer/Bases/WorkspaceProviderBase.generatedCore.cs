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
	/// This class is the base class for any <see cref="WorkspaceProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract partial class WorkspaceProviderBaseCore : EntityProviderBase<MetaBuilder.BusinessLogic.Workspace, MetaBuilder.BusinessLogic.WorkspaceKey>
	{		
		#region Get from Many To Many Relationship Functions
		#region GetByUserIDFromUserPermission
		
		/// <summary>
		///		Gets Workspace objects from the datasource by UserID in the
		///		UserPermission table. Table Workspace is related to table User
		///		through the (M:N) relationship defined in the UserPermission table.
		/// </summary>
		/// <param name="_userID"></param>
		/// <returns>Returns a typed collection of Workspace objects.</returns>
		public TList<Workspace> GetByUserIDFromUserPermission(System.Int32 _userID)
		{
			int count = -1;
			return GetByUserIDFromUserPermission(null,_userID, 0, int.MaxValue, out count);
			
		}
		
		/// <summary>
		///		Gets MetaBuilder.BusinessLogic.Workspace objects from the datasource by UserID in the
		///		UserPermission table. Table Workspace is related to table User
		///		through the (M:N) relationship defined in the UserPermission table.
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_userID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of Workspace objects.</returns>
		public TList<Workspace> GetByUserIDFromUserPermission(System.Int32 _userID, int start, int pageLength)
		{
			int count = -1;
			return GetByUserIDFromUserPermission(null, _userID, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets Workspace objects from the datasource by UserID in the
		///		UserPermission table. Table Workspace is related to table User
		///		through the (M:N) relationship defined in the UserPermission table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_userID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of Workspace objects.</returns>
		public TList<Workspace> GetByUserIDFromUserPermission(TransactionManager transactionManager, System.Int32 _userID)
		{
			int count = -1;
			return GetByUserIDFromUserPermission(transactionManager, _userID, 0, int.MaxValue, out count);
		}
		
		
		/// <summary>
		///		Gets Workspace objects from the datasource by UserID in the
		///		UserPermission table. Table Workspace is related to table User
		///		through the (M:N) relationship defined in the UserPermission table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_userID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of Workspace objects.</returns>
		public TList<Workspace> GetByUserIDFromUserPermission(TransactionManager transactionManager, System.Int32 _userID,int start, int pageLength)
		{
			int count = -1;
			return GetByUserIDFromUserPermission(transactionManager, _userID, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets Workspace objects from the datasource by UserID in the
		///		UserPermission table. Table Workspace is related to table User
		///		through the (M:N) relationship defined in the UserPermission table.
		/// </summary>
		/// <param name="_userID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of Workspace objects.</returns>
		public TList<Workspace> GetByUserIDFromUserPermission(System.Int32 _userID,int start, int pageLength, out int count)
		{
			
			return GetByUserIDFromUserPermission(null, _userID, start, pageLength, out count);
		}


		/// <summary>
		///		Gets Workspace objects from the datasource by UserID in the
		///		UserPermission table. Table Workspace is related to table User
		///		through the (M:N) relationship defined in the UserPermission table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <param name="_userID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of Workspace objects.</returns>
		public abstract TList<Workspace> GetByUserIDFromUserPermission(TransactionManager transactionManager,System.Int32 _userID, int start, int pageLength, out int count);
		
		#endregion GetByUserIDFromUserPermission
		
		#endregion	
		
		#region Delete Methods

		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager">A <see cref="TransactionManager"/> object.</param>
		/// <param name="key">The unique identifier of the row to delete.</param>
		/// <returns>Returns true if operation suceeded.</returns>
		public override bool Delete(TransactionManager transactionManager, MetaBuilder.BusinessLogic.WorkspaceKey key)
		{
			return Delete(transactionManager, key.Name, key.WorkspaceTypeId);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="_name">. Primary Key.</param>
		/// <param name="_WorkspaceTypeId">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(System.String _name, System.Int32 _WorkspaceTypeId)
		{
			return Delete(null, _name, _WorkspaceTypeId);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_name">. Primary Key.</param>
		/// <param name="_WorkspaceTypeId">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(TransactionManager transactionManager, System.String _name, System.Int32 _WorkspaceTypeId);		
		
		#endregion Delete Methods
		
		#region Get By Foreign Key Functions
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Workspace_WorkspaceType key.
		///		FK_Workspace_WorkspaceType Description: 
		/// </summary>
		/// <param name="_WorkspaceTypeId"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Workspace objects.</returns>
		public TList<Workspace> GetByWorkspaceTypeId(System.Int32 _WorkspaceTypeId)
		{
			int count = -1;
			return GetByWorkspaceTypeId(_WorkspaceTypeId, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Workspace_WorkspaceType key.
		///		FK_Workspace_WorkspaceType Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Workspace objects.</returns>
		/// <remarks></remarks>
		public TList<Workspace> GetByWorkspaceTypeId(TransactionManager transactionManager, System.Int32 _WorkspaceTypeId)
		{
			int count = -1;
			return GetByWorkspaceTypeId(transactionManager, _WorkspaceTypeId, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_Workspace_WorkspaceType key.
		///		FK_Workspace_WorkspaceType Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Workspace objects.</returns>
		public TList<Workspace> GetByWorkspaceTypeId(TransactionManager transactionManager, System.Int32 _WorkspaceTypeId, int start, int pageLength)
		{
			int count = -1;
			return GetByWorkspaceTypeId(transactionManager, _WorkspaceTypeId, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Workspace_WorkspaceType key.
		///		fK_Workspace_WorkspaceType Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Workspace objects.</returns>
		public TList<Workspace> GetByWorkspaceTypeId(System.Int32 _WorkspaceTypeId, int start, int pageLength)
		{
			int count =  -1;
			return GetByWorkspaceTypeId(null, _WorkspaceTypeId, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Workspace_WorkspaceType key.
		///		fK_Workspace_WorkspaceType Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Workspace objects.</returns>
		public TList<Workspace> GetByWorkspaceTypeId(System.Int32 _WorkspaceTypeId, int start, int pageLength,out int count)
		{
			return GetByWorkspaceTypeId(null, _WorkspaceTypeId, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Workspace_WorkspaceType key.
		///		FK_Workspace_WorkspaceType Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Workspace objects.</returns>
		public abstract TList<Workspace> GetByWorkspaceTypeId(TransactionManager transactionManager, System.Int32 _WorkspaceTypeId, int start, int pageLength, out int count);
		
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
		public override MetaBuilder.BusinessLogic.Workspace Get(TransactionManager transactionManager, MetaBuilder.BusinessLogic.WorkspaceKey key, int start, int pageLength)
		{
			return GetByNameWorkspaceTypeId(transactionManager, key.Name, key.WorkspaceTypeId, start, pageLength);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key PK_Workspace index.
		/// </summary>
		/// <param name="_name"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Workspace"/> class.</returns>
		public MetaBuilder.BusinessLogic.Workspace GetByNameWorkspaceTypeId(System.String _name, System.Int32 _WorkspaceTypeId)
		{
			int count = -1;
			return GetByNameWorkspaceTypeId(null,_name, _WorkspaceTypeId, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Workspace index.
		/// </summary>
		/// <param name="_name"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Workspace"/> class.</returns>
		public MetaBuilder.BusinessLogic.Workspace GetByNameWorkspaceTypeId(System.String _name, System.Int32 _WorkspaceTypeId, int start, int pageLength)
		{
			int count = -1;
			return GetByNameWorkspaceTypeId(null, _name, _WorkspaceTypeId, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Workspace index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_name"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Workspace"/> class.</returns>
		public MetaBuilder.BusinessLogic.Workspace GetByNameWorkspaceTypeId(TransactionManager transactionManager, System.String _name, System.Int32 _WorkspaceTypeId)
		{
			int count = -1;
			return GetByNameWorkspaceTypeId(transactionManager, _name, _WorkspaceTypeId, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Workspace index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_name"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Workspace"/> class.</returns>
		public MetaBuilder.BusinessLogic.Workspace GetByNameWorkspaceTypeId(TransactionManager transactionManager, System.String _name, System.Int32 _WorkspaceTypeId, int start, int pageLength)
		{
			int count = -1;
			return GetByNameWorkspaceTypeId(transactionManager, _name, _WorkspaceTypeId, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Workspace index.
		/// </summary>
		/// <param name="_name"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Workspace"/> class.</returns>
		public MetaBuilder.BusinessLogic.Workspace GetByNameWorkspaceTypeId(System.String _name, System.Int32 _WorkspaceTypeId, int start, int pageLength, out int count)
		{
			return GetByNameWorkspaceTypeId(null, _name, _WorkspaceTypeId, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Workspace index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_name"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Workspace"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.Workspace GetByNameWorkspaceTypeId(TransactionManager transactionManager, System.String _name, System.Int32 _WorkspaceTypeId, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a TList&lt;Workspace&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="TList&lt;Workspace&gt;"/></returns>
		public static TList<Workspace> Fill(IDataReader reader, TList<Workspace> rows, int start, int pageLength)
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
				
				MetaBuilder.BusinessLogic.Workspace c = null;
				if (useEntityFactory)
				{
					key = new System.Text.StringBuilder("Workspace")
					.Append("|").Append((System.String)reader[((int)WorkspaceColumn.Name - 1)])
					.Append("|").Append((System.Int32)reader[((int)WorkspaceColumn.WorkspaceTypeId - 1)]).ToString();
					c = EntityManager.LocateOrCreate<Workspace>(
					key.ToString(), // EntityTrackingKey
					"Workspace",  //Creational Type
					entityCreationFactoryType,  //Factory used to create entity
					enableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.Workspace();
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
					c.pkid = (System.Int32)reader[((int)WorkspaceColumn.pkid - 1)];
					c.Name = (System.String)reader[((int)WorkspaceColumn.Name - 1)];
					c.OriginalName = c.Name;
					c.WorkspaceTypeId = (System.Int32)reader[((int)WorkspaceColumn.WorkspaceTypeId - 1)];
					c.OriginalWorkspaceTypeId = c.WorkspaceTypeId;
					c.RequestedByUser = (reader.IsDBNull(((int)WorkspaceColumn.RequestedByUser - 1)))?null:(System.String)reader[((int)WorkspaceColumn.RequestedByUser - 1)];
					c.IsActive = (reader.IsDBNull(((int)WorkspaceColumn.IsActive - 1)))?null:(System.Boolean?)reader[((int)WorkspaceColumn.IsActive - 1)];
					c.EntityTrackingKey = key;
					c.AcceptChanges();
					c.SuppressEntityEvents = false;
				}
				rows.Add(c);
			}
		return rows;
		}		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.Workspace"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.Workspace"/> object to refresh.</param>
		public static void RefreshEntity(IDataReader reader, MetaBuilder.BusinessLogic.Workspace entity)
		{
			if (!reader.Read()) return;
			
			entity.pkid = (System.Int32)reader[((int)WorkspaceColumn.pkid - 1)];
			entity.Name = (System.String)reader[((int)WorkspaceColumn.Name - 1)];
			entity.OriginalName = (System.String)reader["Name"];
			entity.WorkspaceTypeId = (System.Int32)reader[((int)WorkspaceColumn.WorkspaceTypeId - 1)];
			entity.OriginalWorkspaceTypeId = (System.Int32)reader["WorkspaceTypeId"];
			entity.RequestedByUser = (reader.IsDBNull(((int)WorkspaceColumn.RequestedByUser - 1)))?null:(System.String)reader[((int)WorkspaceColumn.RequestedByUser - 1)];
			entity.IsActive = (reader.IsDBNull(((int)WorkspaceColumn.IsActive - 1)))?null:(System.Boolean?)reader[((int)WorkspaceColumn.IsActive - 1)];
			entity.AcceptChanges();
		}
		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.Workspace"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.Workspace"/> object.</param>
		public static void RefreshEntity(DataSet dataSet, MetaBuilder.BusinessLogic.Workspace entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.pkid = (System.Int32)dataRow["pkid"];
			entity.Name = (System.String)dataRow["Name"];
			entity.OriginalName = (System.String)dataRow["Name"];
			entity.WorkspaceTypeId = (System.Int32)dataRow["WorkspaceTypeId"];
			entity.OriginalWorkspaceTypeId = (System.Int32)dataRow["WorkspaceTypeId"];
			entity.RequestedByUser = Convert.IsDBNull(dataRow["RequestedByUser"]) ? null : (System.String)dataRow["RequestedByUser"];
			entity.IsActive = Convert.IsDBNull(dataRow["IsActive"]) ? null : (System.Boolean?)dataRow["IsActive"];
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
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.Workspace"/> object to load.</param>
		/// <param name="deep">Boolean. A flag that indicates whether to recursively save all Property Collection that are descendants of this instance. If True, saves the complete object graph below this object. If False, saves this object only. </param>
		/// <param name="deepLoadType">DeepLoadType Enumeration to Include/Exclude object property collections from Load.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.Workspace Property Collection Type Array To Include or Exclude from Load</param>
		/// <param name="innerList">A collection of child types for easy access.</param>
	    /// <exception cref="ArgumentNullException">entity or childTypes is null.</exception>
	    /// <exception cref="ArgumentException">deepLoadType has invalid value.</exception>
		public override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.Workspace entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, DeepSession innerList)
		{
			if(entity == null)
				return;

			#region WorkspaceTypeIdSource	
			if (CanDeepLoad(entity, "WorkspaceType|WorkspaceTypeIdSource", deepLoadType, innerList) 
				&& entity.WorkspaceTypeIdSource == null)
			{
				object[] pkItems = new object[1];
				pkItems[0] = entity.WorkspaceTypeId;
				WorkspaceType tmpEntity = EntityManager.LocateEntity<WorkspaceType>(EntityLocator.ConstructKeyFromPkItems(typeof(WorkspaceType), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.WorkspaceTypeIdSource = tmpEntity;
				else
					entity.WorkspaceTypeIdSource = DataRepository.WorkspaceTypeProvider.GetBypkid(transactionManager, entity.WorkspaceTypeId);		
				
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'WorkspaceTypeIdSource' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.WorkspaceTypeIdSource != null)
				{
					innerList.SkipChildren = true;
					DataRepository.WorkspaceTypeProvider.DeepLoad(transactionManager, entity.WorkspaceTypeIdSource, deep, deepLoadType, childTypes, innerList);
					innerList.SkipChildren = false;
				}
					
			}
			#endregion WorkspaceTypeIdSource
			
			//used to hold DeepLoad method delegates and fire after all the local children have been loaded.
			Dictionary<string, KeyValuePair<Delegate, object>> deepHandles = new Dictionary<string, KeyValuePair<Delegate, object>>();
			// Deep load child collections  - Call GetByNameWorkspaceTypeId methods when available
			
			#region GraphFileCollection
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<GraphFile>|GraphFileCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'GraphFileCollection' loaded. key " + entity.EntityTrackingKey);
				#endif 

				entity.GraphFileCollection = DataRepository.GraphFileProvider.GetByWorkspaceNameWorkspaceTypeId(transactionManager, entity.Name, entity.WorkspaceTypeId);

				if (deep && entity.GraphFileCollection.Count > 0)
				{
					deepHandles.Add("GraphFileCollection",
						new KeyValuePair<Delegate, object>((DeepLoadHandle<GraphFile>) DataRepository.GraphFileProvider.DeepLoad,
						new object[] { transactionManager, entity.GraphFileCollection, deep, deepLoadType, childTypes, innerList }
					));
				}
			}		
			#endregion 
			
			
			#region UserPermissionCollection
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<UserPermission>|UserPermissionCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'UserPermissionCollection' loaded. key " + entity.EntityTrackingKey);
				#endif 

				entity.UserPermissionCollection = DataRepository.UserPermissionProvider.GetByWorkspaceNameWorkspaceTypeId(transactionManager, entity.Name, entity.WorkspaceTypeId);

				if (deep && entity.UserPermissionCollection.Count > 0)
				{
					deepHandles.Add("UserPermissionCollection",
						new KeyValuePair<Delegate, object>((DeepLoadHandle<UserPermission>) DataRepository.UserPermissionProvider.DeepLoad,
						new object[] { transactionManager, entity.UserPermissionCollection, deep, deepLoadType, childTypes, innerList }
					));
				}
			}		
			#endregion 
			
			
			#region UserIDUserCollection_From_UserPermission
			// RelationshipType.ManyToMany
			if (CanDeepLoad(entity, "List<User>|UserIDUserCollection_From_UserPermission", deepLoadType, innerList))
			{
				entity.UserIDUserCollection_From_UserPermission = DataRepository.UserProvider.GetByWorkspaceNameWorkspaceTypeIdFromUserPermission(transactionManager, entity.Name, entity.WorkspaceTypeId);			 
		
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'UserIDUserCollection_From_UserPermission' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.UserIDUserCollection_From_UserPermission != null)
				{
					deepHandles.Add("UserIDUserCollection_From_UserPermission",
						new KeyValuePair<Delegate, object>((DeepLoadHandle< User >) DataRepository.UserProvider.DeepLoad,
						new object[] { transactionManager, entity.UserIDUserCollection_From_UserPermission, deep, deepLoadType, childTypes, innerList }
					));
				}
			}
			#endregion
			
			
			
			#region MetaObjectCollection
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<MetaObject>|MetaObjectCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'MetaObjectCollection' loaded. key " + entity.EntityTrackingKey);
				#endif 

				entity.MetaObjectCollection = DataRepository.MetaObjectProvider.GetByWorkspaceNameWorkspaceTypeId(transactionManager, entity.Name, entity.WorkspaceTypeId);

				if (deep && entity.MetaObjectCollection.Count > 0)
				{
					deepHandles.Add("MetaObjectCollection",
						new KeyValuePair<Delegate, object>((DeepLoadHandle<MetaObject>) DataRepository.MetaObjectProvider.DeepLoad,
						new object[] { transactionManager, entity.MetaObjectCollection, deep, deepLoadType, childTypes, innerList }
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
		/// Deep Save the entire object graph of the MetaBuilder.BusinessLogic.Workspace object with criteria based of the child 
		/// Type property array and DeepSaveType.
		/// </summary>
		/// <param name="transactionManager">The transaction manager.</param>
		/// <param name="entity">MetaBuilder.BusinessLogic.Workspace instance</param>
		/// <param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.Workspace Property Collection Type Array To Include or Exclude from Save</param>
		/// <param name="innerList">A Hashtable of child types for easy access.</param>
		public override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.Workspace entity, DeepSaveType deepSaveType, System.Type[] childTypes, DeepSession innerList)
		{	
			if (entity == null)
				return false;
							
			#region Composite Parent Properties
			//Save Source Composite Properties, however, don't call deep save on them.  
			//So they only get saved a single level deep.
			
			#region WorkspaceTypeIdSource
			if (CanDeepSave(entity, "WorkspaceType|WorkspaceTypeIdSource", deepSaveType, innerList) 
				&& entity.WorkspaceTypeIdSource != null)
			{
				DataRepository.WorkspaceTypeProvider.Save(transactionManager, entity.WorkspaceTypeIdSource);
				entity.WorkspaceTypeId = entity.WorkspaceTypeIdSource.pkid;
			}
			#endregion 
			#endregion Composite Parent Properties

			// Save Root Entity through Provider
			if (!entity.IsDeleted)
				this.Save(transactionManager, entity);
			
			//used to hold DeepSave method delegates and fire after all the local children have been saved.
			Dictionary<string, KeyValuePair<Delegate, object>> deepHandles = new Dictionary<string, KeyValuePair<Delegate, object>>();

			#region UserIDUserCollection_From_UserPermission>
			if (CanDeepSave(entity.UserIDUserCollection_From_UserPermission, "List<User>|UserIDUserCollection_From_UserPermission", deepSaveType, innerList))
			{
				if (entity.UserIDUserCollection_From_UserPermission.Count > 0 || entity.UserIDUserCollection_From_UserPermission.DeletedItems.Count > 0)
				{
					DataRepository.UserProvider.Save(transactionManager, entity.UserIDUserCollection_From_UserPermission); 
					deepHandles.Add("UserIDUserCollection_From_UserPermission",
						new KeyValuePair<Delegate, object>((DeepSaveHandle<User>) DataRepository.UserProvider.DeepSave,
						new object[] { transactionManager, entity.UserIDUserCollection_From_UserPermission, deepSaveType, childTypes, innerList }
					));
				}
			}
			#endregion 
	
			#region List<GraphFile>
				if (CanDeepSave(entity.GraphFileCollection, "List<GraphFile>|GraphFileCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(GraphFile child in entity.GraphFileCollection)
					{
						if(child.WorkspaceNameWorkspaceTypeIdSource != null)
						{
							child.WorkspaceName = child.WorkspaceNameWorkspaceTypeIdSource.Name;
							child.WorkspaceTypeId = child.WorkspaceNameWorkspaceTypeIdSource.WorkspaceTypeId;
						}
						else
						{
							child.WorkspaceName = entity.Name;
							child.WorkspaceTypeId = entity.WorkspaceTypeId;
						}

					}

					if (entity.GraphFileCollection.Count > 0 || entity.GraphFileCollection.DeletedItems.Count > 0)
					{
						//DataRepository.GraphFileProvider.Save(transactionManager, entity.GraphFileCollection);
						
						deepHandles.Add("GraphFileCollection",
						new KeyValuePair<Delegate, object>((DeepSaveHandle< GraphFile >) DataRepository.GraphFileProvider.DeepSave,
							new object[] { transactionManager, entity.GraphFileCollection, deepSaveType, childTypes, innerList }
						));
					}
				} 
			#endregion 
				
	
			#region List<UserPermission>
				if (CanDeepSave(entity.UserPermissionCollection, "List<UserPermission>|UserPermissionCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(UserPermission child in entity.UserPermissionCollection)
					{
						if(child.WorkspaceNameWorkspaceTypeIdSource != null)
						{
								child.WorkspaceName = child.WorkspaceNameWorkspaceTypeIdSource.Name;
								child.WorkspaceTypeId = child.WorkspaceNameWorkspaceTypeIdSource.WorkspaceTypeId;
						}

						if(child.UserIDSource != null)
						{
								child.UserID = child.UserIDSource.pkid;
						}

					}

					if (entity.UserPermissionCollection.Count > 0 || entity.UserPermissionCollection.DeletedItems.Count > 0)
					{
						//DataRepository.UserPermissionProvider.Save(transactionManager, entity.UserPermissionCollection);
						
						deepHandles.Add("UserPermissionCollection",
						new KeyValuePair<Delegate, object>((DeepSaveHandle< UserPermission >) DataRepository.UserPermissionProvider.DeepSave,
							new object[] { transactionManager, entity.UserPermissionCollection, deepSaveType, childTypes, innerList }
						));
					}
				} 
			#endregion 
				
	
			#region List<MetaObject>
				if (CanDeepSave(entity.MetaObjectCollection, "List<MetaObject>|MetaObjectCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(MetaObject child in entity.MetaObjectCollection)
					{
						if(child.WorkspaceNameWorkspaceTypeIdSource != null)
						{
							child.WorkspaceName = child.WorkspaceNameWorkspaceTypeIdSource.Name;
							child.WorkspaceTypeId = child.WorkspaceNameWorkspaceTypeIdSource.WorkspaceTypeId;
						}
						else
						{
							child.WorkspaceName = entity.Name;
							child.WorkspaceTypeId = entity.WorkspaceTypeId;
						}

					}

					if (entity.MetaObjectCollection.Count > 0 || entity.MetaObjectCollection.DeletedItems.Count > 0)
					{
						//DataRepository.MetaObjectProvider.Save(transactionManager, entity.MetaObjectCollection);
						
						deepHandles.Add("MetaObjectCollection",
						new KeyValuePair<Delegate, object>((DeepSaveHandle< MetaObject >) DataRepository.MetaObjectProvider.DeepSave,
							new object[] { transactionManager, entity.MetaObjectCollection, deepSaveType, childTypes, innerList }
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
	
	#region WorkspaceChildEntityTypes
	
	///<summary>
	/// Enumeration used to expose the different child entity types 
	/// for child properties in <c>MetaBuilder.BusinessLogic.Workspace</c>
	///</summary>
	public enum WorkspaceChildEntityTypes
	{
		
		///<summary>
		/// Composite Property for <c>WorkspaceType</c> at WorkspaceTypeIdSource
		///</summary>
		[ChildEntityType(typeof(WorkspaceType))]
		WorkspaceType,
	
		///<summary>
		/// Collection of <c>Workspace</c> as OneToMany for GraphFileCollection
		///</summary>
		[ChildEntityType(typeof(TList<GraphFile>))]
		GraphFileCollection,

		///<summary>
		/// Collection of <c>Workspace</c> as OneToMany for UserPermissionCollection
		///</summary>
		[ChildEntityType(typeof(TList<UserPermission>))]
		UserPermissionCollection,

		///<summary>
		/// Collection of <c>Workspace</c> as ManyToMany for UserCollection_From_UserPermission
		///</summary>
		[ChildEntityType(typeof(TList<User>))]
		UserIDUserCollection_From_UserPermission,

		///<summary>
		/// Collection of <c>Workspace</c> as OneToMany for MetaObjectCollection
		///</summary>
		[ChildEntityType(typeof(TList<MetaObject>))]
		MetaObjectCollection,
	}
	
	#endregion WorkspaceChildEntityTypes
	
	#region WorkspaceFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;WorkspaceColumn&gt;"/> class
	/// that is used exclusively with a <see cref="Workspace"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class WorkspaceFilterBuilder : SqlFilterBuilder<WorkspaceColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the WorkspaceFilterBuilder class.
		/// </summary>
		public WorkspaceFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the WorkspaceFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public WorkspaceFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the WorkspaceFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public WorkspaceFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion WorkspaceFilterBuilder
	
	#region WorkspaceParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;WorkspaceColumn&gt;"/> class
	/// that is used exclusively with a <see cref="Workspace"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class WorkspaceParameterBuilder : ParameterizedSqlFilterBuilder<WorkspaceColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the WorkspaceParameterBuilder class.
		/// </summary>
		public WorkspaceParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the WorkspaceParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public WorkspaceParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the WorkspaceParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public WorkspaceParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion WorkspaceParameterBuilder
	
	#region WorkspaceSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;WorkspaceColumn&gt;"/> class
	/// that is used exclusively with a <see cref="Workspace"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class WorkspaceSortBuilder : SqlSortBuilder<WorkspaceColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the WorkspaceSqlSortBuilder class.
		/// </summary>
		public WorkspaceSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion WorkspaceSortBuilder
	
} // end namespace
