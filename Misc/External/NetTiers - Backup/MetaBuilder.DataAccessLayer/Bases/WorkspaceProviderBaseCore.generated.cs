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
		/// <param name="userID"></param>
		/// <returns>Returns a typed collection of Workspace objects.</returns>
		public TList<Workspace> GetByUserIDFromUserPermission(System.Int32 userID)
		{
			int count = -1;
			return GetByUserIDFromUserPermission(null,userID, 0, int.MaxValue, out count);
			
		}
		
		/// <summary>
		///		Gets MetaBuilder.BusinessLogic.Workspace objects from the datasource by UserID in the
		///		UserPermission table. Table Workspace is related to table User
		///		through the (M:N) relationship defined in the UserPermission table.
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="userID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of Workspace objects.</returns>
		public TList<Workspace> GetByUserIDFromUserPermission(System.Int32 userID, int start, int pageLength)
		{
			int count = -1;
			return GetByUserIDFromUserPermission(null, userID, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets Workspace objects from the datasource by UserID in the
		///		UserPermission table. Table Workspace is related to table User
		///		through the (M:N) relationship defined in the UserPermission table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="userID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of Workspace objects.</returns>
		public TList<Workspace> GetByUserIDFromUserPermission(TransactionManager transactionManager, System.Int32 userID)
		{
			int count = -1;
			return GetByUserIDFromUserPermission(transactionManager, userID, 0, int.MaxValue, out count);
		}
		
		
		/// <summary>
		///		Gets Workspace objects from the datasource by UserID in the
		///		UserPermission table. Table Workspace is related to table User
		///		through the (M:N) relationship defined in the UserPermission table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="userID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of Workspace objects.</returns>
		public TList<Workspace> GetByUserIDFromUserPermission(TransactionManager transactionManager, System.Int32 userID,int start, int pageLength)
		{
			int count = -1;
			return GetByUserIDFromUserPermission(transactionManager, userID, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets Workspace objects from the datasource by UserID in the
		///		UserPermission table. Table Workspace is related to table User
		///		through the (M:N) relationship defined in the UserPermission table.
		/// </summary>
		/// <param name="userID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of Workspace objects.</returns>
		public TList<Workspace> GetByUserIDFromUserPermission(System.Int32 userID,int start, int pageLength, out int count)
		{
			
			return GetByUserIDFromUserPermission(null, userID, start, pageLength, out count);
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
		/// <param name="userID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of Workspace objects.</returns>
		public abstract TList<Workspace> GetByUserIDFromUserPermission(TransactionManager transactionManager,System.Int32 userID, int start, int pageLength, out int count);
		
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
			return Delete(transactionManager, key.Name, key.WorkspaceTypeID);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="name">. Primary Key.</param>
		/// <param name="workspaceTypeID">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(System.String name, System.Int32 workspaceTypeID)
		{
			return Delete(null, name, workspaceTypeID);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="name">. Primary Key.</param>
		/// <param name="workspaceTypeID">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(TransactionManager transactionManager, System.String name, System.Int32 workspaceTypeID);		
		
		#endregion Delete Methods
		
		#region Get By Foreign Key Functions
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Workspace_WorkspaceType key.
		///		FK_Workspace_WorkspaceType Description: 
		/// </summary>
		/// <param name="workspaceTypeID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Workspace objects.</returns>
		public MetaBuilder.BusinessLogic.TList<Workspace> GetByWorkspaceTypeID(System.Int32 workspaceTypeID)
		{
			int count = -1;
			return GetByWorkspaceTypeID(workspaceTypeID, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Workspace_WorkspaceType key.
		///		FK_Workspace_WorkspaceType Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="workspaceTypeID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Workspace objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<Workspace> GetByWorkspaceTypeID(TransactionManager transactionManager, System.Int32 workspaceTypeID)
		{
			int count = -1;
			return GetByWorkspaceTypeID(transactionManager, workspaceTypeID, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_Workspace_WorkspaceType key.
		///		FK_Workspace_WorkspaceType Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="workspaceTypeID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Workspace objects.</returns>
		public MetaBuilder.BusinessLogic.TList<Workspace> GetByWorkspaceTypeID(TransactionManager transactionManager, System.Int32 workspaceTypeID, int start, int pageLength)
		{
			int count = -1;
			return GetByWorkspaceTypeID(transactionManager, workspaceTypeID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Workspace_WorkspaceType key.
		///		fKWorkspaceWorkspaceType Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="workspaceTypeID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Workspace objects.</returns>
		public MetaBuilder.BusinessLogic.TList<Workspace> GetByWorkspaceTypeID(System.Int32 workspaceTypeID, int start, int pageLength)
		{
			int count =  -1;
			return GetByWorkspaceTypeID(null, workspaceTypeID, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Workspace_WorkspaceType key.
		///		fKWorkspaceWorkspaceType Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="workspaceTypeID"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Workspace objects.</returns>
		public MetaBuilder.BusinessLogic.TList<Workspace> GetByWorkspaceTypeID(System.Int32 workspaceTypeID, int start, int pageLength,out int count)
		{
			return GetByWorkspaceTypeID(null, workspaceTypeID, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Workspace_WorkspaceType key.
		///		FK_Workspace_WorkspaceType Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="workspaceTypeID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Workspace objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<Workspace> GetByWorkspaceTypeID(TransactionManager transactionManager, System.Int32 workspaceTypeID, int start, int pageLength, out int count);
		
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
			return GetByNameWorkspaceTypeID(transactionManager, key.Name, key.WorkspaceTypeID, start, pageLength);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key PK_Workspace index.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="workspaceTypeID"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Workspace"/> class.</returns>
		public MetaBuilder.BusinessLogic.Workspace GetByNameWorkspaceTypeID(System.String name, System.Int32 workspaceTypeID)
		{
			int count = -1;
			return GetByNameWorkspaceTypeID(null,name, workspaceTypeID, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Workspace index.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="workspaceTypeID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Workspace"/> class.</returns>
		public MetaBuilder.BusinessLogic.Workspace GetByNameWorkspaceTypeID(System.String name, System.Int32 workspaceTypeID, int start, int pageLength)
		{
			int count = -1;
			return GetByNameWorkspaceTypeID(null, name, workspaceTypeID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Workspace index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="name"></param>
		/// <param name="workspaceTypeID"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Workspace"/> class.</returns>
		public MetaBuilder.BusinessLogic.Workspace GetByNameWorkspaceTypeID(TransactionManager transactionManager, System.String name, System.Int32 workspaceTypeID)
		{
			int count = -1;
			return GetByNameWorkspaceTypeID(transactionManager, name, workspaceTypeID, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Workspace index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="name"></param>
		/// <param name="workspaceTypeID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Workspace"/> class.</returns>
		public MetaBuilder.BusinessLogic.Workspace GetByNameWorkspaceTypeID(TransactionManager transactionManager, System.String name, System.Int32 workspaceTypeID, int start, int pageLength)
		{
			int count = -1;
			return GetByNameWorkspaceTypeID(transactionManager, name, workspaceTypeID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Workspace index.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="workspaceTypeID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Workspace"/> class.</returns>
		public MetaBuilder.BusinessLogic.Workspace GetByNameWorkspaceTypeID(System.String name, System.Int32 workspaceTypeID, int start, int pageLength, out int count)
		{
			return GetByNameWorkspaceTypeID(null, name, workspaceTypeID, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Workspace index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="name"></param>
		/// <param name="workspaceTypeID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Workspace"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.Workspace GetByNameWorkspaceTypeID(TransactionManager transactionManager, System.String name, System.Int32 workspaceTypeID, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a MetaBuilder.BusinessLogic.TList&lt;Workspace&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="MetaBuilder.BusinessLogic.TList&lt;Workspace&gt;"/></returns>
		public static MetaBuilder.BusinessLogic.TList<Workspace> Fill(IDataReader reader, MetaBuilder.BusinessLogic.TList<Workspace> rows, int start, int pageLength)
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
				
				MetaBuilder.BusinessLogic.Workspace c = null;
				if (DataRepository.Provider.UseEntityFactory)
				{
					key = @"Workspace" 
							+ (reader.IsDBNull(reader.GetOrdinal("Name"))?string.Empty:(System.String)reader["Name"]).ToString()
							+ (reader.IsDBNull(reader.GetOrdinal("WorkspaceTypeID"))?(int)0:(System.Int32)reader["WorkspaceTypeID"]).ToString();

					c = EntityManager.LocateOrCreate<Workspace>(
						key.ToString(), // EntityTrackingKey 
						"Workspace",  //Creational Type
						DataRepository.Provider.EntityCreationalFactoryType,  //Factory used to create entity
						DataRepository.Provider.EnableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.Workspace();
				}
				
				if (!DataRepository.Provider.EnableEntityTracking || c.EntityState == EntityState.Added)
                {
					c.SuppressEntityEvents = true;
					c.Pkid = (System.Int32)reader["pkid"];
					c.Name = (System.String)reader["Name"];
					c.OriginalName = c.Name; //(reader.IsDBNull(reader.GetOrdinal("Name")))?string.Empty:(System.String)reader["Name"];
					c.WorkspaceTypeID = (System.Int32)reader["WorkspaceTypeID"];
					c.OriginalWorkspaceTypeID = c.WorkspaceTypeID; //(reader.IsDBNull(reader.GetOrdinal("WorkspaceTypeID")))?(int)0:(System.Int32)reader["WorkspaceTypeID"];
					c.RequestedByUser = (reader.IsDBNull(reader.GetOrdinal("RequestedByUser")))?null:(System.String)reader["RequestedByUser"];
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
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.Workspace"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.Workspace"/> object to refresh.</param>
		public static void RefreshEntity(IDataReader reader, MetaBuilder.BusinessLogic.Workspace entity)
		{
			if (!reader.Read()) return;
			
			entity.Pkid = (System.Int32)reader["pkid"];
			entity.Name = (System.String)reader["Name"];
			entity.OriginalName = (System.String)reader["Name"];
			entity.WorkspaceTypeID = (System.Int32)reader["WorkspaceTypeID"];
			entity.OriginalWorkspaceTypeID = (System.Int32)reader["WorkspaceTypeID"];
			entity.RequestedByUser = (reader.IsDBNull(reader.GetOrdinal("RequestedByUser")))?null:(System.String)reader["RequestedByUser"];
			entity.IsActive = (reader.IsDBNull(reader.GetOrdinal("IsActive")))?null:(System.Boolean?)reader["IsActive"];
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
			
			entity.Pkid = (System.Int32)dataRow["pkid"];
			entity.Name = (System.String)dataRow["Name"];
			entity.OriginalName = (System.String)dataRow["Name"];
			entity.WorkspaceTypeID = (System.Int32)dataRow["WorkspaceTypeID"];
			entity.OriginalWorkspaceTypeID = (System.Int32)dataRow["WorkspaceTypeID"];
			entity.RequestedByUser = (Convert.IsDBNull(dataRow["RequestedByUser"]))?null:(System.String)dataRow["RequestedByUser"];
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
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.Workspace"/> object to load.</param>
		/// <param name="deep">Boolean. A flag that indicates whether to recursively save all Property Collection that are descendants of this instance. If True, saves the complete object graph below this object. If False, saves this object only. </param>
		/// <param name="deepLoadType">DeepLoadType Enumeration to Include/Exclude object property collections from Load.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.Workspace Property Collection Type Array To Include or Exclude from Load</param>
		/// <param name="innerList">A collection of child types for easy access.</param>
	    /// <exception cref="ArgumentNullException">entity or childTypes is null.</exception>
	    /// <exception cref="ArgumentException">deepLoadType has invalid value.</exception>
		internal override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.Workspace entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, ChildEntityTypesList innerList)
		{
			if(entity == null)
				return;

			#region WorkspaceTypeIDSource	
			if (CanDeepLoad(entity, "WorkspaceType", "WorkspaceTypeIDSource", deepLoadType, innerList) 
				&& entity.WorkspaceTypeIDSource == null)
			{
				object[] pkItems = new object[1];
				pkItems[0] = entity.WorkspaceTypeID;
				WorkspaceType tmpEntity = EntityManager.LocateEntity<WorkspaceType>(EntityLocator.ConstructKeyFromPkItems(typeof(WorkspaceType), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.WorkspaceTypeIDSource = tmpEntity;
				else
					entity.WorkspaceTypeIDSource = DataRepository.WorkspaceTypeProvider.GetByPkid(entity.WorkspaceTypeID);
			
				if (deep && entity.WorkspaceTypeIDSource != null)
				{
					DataRepository.WorkspaceTypeProvider.DeepLoad(transactionManager, entity.WorkspaceTypeIDSource, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion WorkspaceTypeIDSource
			
			// Load Entity through Provider
			// Deep load child collections  - Call GetByNameWorkspaceTypeID methods when available
			
			#region GraphFileCollection
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<GraphFile>", "GraphFileCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'GraphFileCollection' loaded.");
				#endif 

				entity.GraphFileCollection = DataRepository.GraphFileProvider.GetByWorkspaceNameWorkspaceTypeID(transactionManager, entity.Name, entity.WorkspaceTypeID);

				if (deep && entity.GraphFileCollection.Count > 0)
				{
					DataRepository.GraphFileProvider.DeepLoad(transactionManager, entity.GraphFileCollection, deep, deepLoadType, childTypes, innerList);
				}
			}		
			#endregion 
			
			#region UserPermissionCollection
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<UserPermission>", "UserPermissionCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'UserPermissionCollection' loaded.");
				#endif 

				entity.UserPermissionCollection = DataRepository.UserPermissionProvider.GetByWorkspaceNameWorkspaceTypeID(transactionManager, entity.Name, entity.WorkspaceTypeID);

				if (deep && entity.UserPermissionCollection.Count > 0)
				{
					DataRepository.UserPermissionProvider.DeepLoad(transactionManager, entity.UserPermissionCollection, deep, deepLoadType, childTypes, innerList);
				}
			}		
			#endregion 
			
			#region MetaObjectCollection
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<MetaObject>", "MetaObjectCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'MetaObjectCollection' loaded.");
				#endif 

				entity.MetaObjectCollection = DataRepository.MetaObjectProvider.GetByWorkspaceNameWorkspaceTypeID(transactionManager, entity.Name, entity.WorkspaceTypeID);

				if (deep && entity.MetaObjectCollection.Count > 0)
				{
					DataRepository.MetaObjectProvider.DeepLoad(transactionManager, entity.MetaObjectCollection, deep, deepLoadType, childTypes, innerList);
				}
			}		
			#endregion 
			
			#region UserCollection_From_UserPermission
			// RelationshipType.ManyToMany
			if (CanDeepLoad(entity, "List<User>", "UserCollection_From_UserPermission", deepLoadType, innerList))
			{
				entity.UserCollection_From_UserPermission = DataRepository.UserProvider.GetByWorkspaceNameWorkspaceTypeIDFromUserPermission(transactionManager, entity.Name, entity.WorkspaceTypeID);			 
		
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'UserCollection_From_UserPermission' loaded.");
				#endif 

				if (deep && entity.UserCollection_From_UserPermission.Count > 0)
				{
					DataRepository.UserProvider.DeepLoad(transactionManager, entity.UserCollection_From_UserPermission, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion
			
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
		internal override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.Workspace entity, DeepSaveType deepSaveType, System.Type[] childTypes, ChildEntityTypesList innerList)
		{	
			if (entity == null)
				return false;
							
			#region Composite Parent Properties
			//Save Source Composite Properties, however, don't call deep save on them.  
			//So they only get saved a single level deep.
			
			#region WorkspaceTypeIDSource
			if (CanDeepSave(entity, "WorkspaceType", "WorkspaceTypeIDSource", deepSaveType, innerList) 
				&& entity.WorkspaceTypeIDSource != null)
			{
				DataRepository.WorkspaceTypeProvider.Save(transactionManager, entity.WorkspaceTypeIDSource);
				entity.WorkspaceTypeID = entity.WorkspaceTypeIDSource.Pkid;
			}
			#endregion 
			#endregion Composite Parent Properties

			// Save Root Entity through Provider
			this.Save(transactionManager, entity);
			
			








			#region UserCollection_From_UserPermission>
			if (CanDeepSave(entity, "List<User>", "UserCollection_From_UserPermission", deepSaveType, innerList))
			{
				if (entity.UserCollection_From_UserPermission.Count > 0 || entity.UserCollection_From_UserPermission.DeletedItems.Count > 0)
					DataRepository.UserProvider.DeepSave(transactionManager, entity.UserCollection_From_UserPermission, deepSaveType, childTypes, innerList); 
			}
			#endregion 

			#region List<GraphFile>
				if (CanDeepSave(entity, "List<GraphFile>", "GraphFileCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(GraphFile child in entity.GraphFileCollection)
					{
						child.WorkspaceName = entity.Name;
						child.WorkspaceTypeID = entity.WorkspaceTypeID;
					}
				
				if (entity.GraphFileCollection.Count > 0 || entity.GraphFileCollection.DeletedItems.Count > 0)
					DataRepository.GraphFileProvider.DeepSave(transactionManager, entity.GraphFileCollection, deepSaveType, childTypes, innerList);
				} 
			#endregion 
				

			#region List<UserPermission>
				if (CanDeepSave(entity, "List<UserPermission>", "UserPermissionCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(UserPermission child in entity.UserPermissionCollection)
					{
						child.WorkspaceName = entity.Name;
						child.WorkspaceTypeID = entity.WorkspaceTypeID;
					}
				
				if (entity.UserPermissionCollection.Count > 0 || entity.UserPermissionCollection.DeletedItems.Count > 0)
					DataRepository.UserPermissionProvider.DeepSave(transactionManager, entity.UserPermissionCollection, deepSaveType, childTypes, innerList);
				} 
			#endregion 
				

			#region List<MetaObject>
				if (CanDeepSave(entity, "List<MetaObject>", "MetaObjectCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(MetaObject child in entity.MetaObjectCollection)
					{
						child.WorkspaceName = entity.Name;
						child.WorkspaceTypeID = entity.WorkspaceTypeID;
					}
				
				if (entity.MetaObjectCollection.Count > 0 || entity.MetaObjectCollection.DeletedItems.Count > 0)
					DataRepository.MetaObjectProvider.DeepSave(transactionManager, entity.MetaObjectCollection, deepSaveType, childTypes, innerList);
				} 
			#endregion 
				





						
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
		/// Composite Property for <c>WorkspaceType</c> at WorkspaceTypeIDSource
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
		/// Collection of <c>Workspace</c> as OneToMany for MetaObjectCollection
		///</summary>
		[ChildEntityType(typeof(TList<MetaObject>))]
		MetaObjectCollection,

		///<summary>
		/// Collection of <c>Workspace</c> as ManyToMany for UserCollection_From_UserPermission
		///</summary>
		[ChildEntityType(typeof(TList<User>))]
		UserCollection_From_UserPermission,
	}
	
	#endregion WorkspaceChildEntityTypes
	
	#region WorkspaceFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
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
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
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
} // end namespace
