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
	/// This class is the base class for any <see cref="UserProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract partial class UserProviderBaseCore : EntityProviderBase<MetaBuilder.BusinessLogic.User, MetaBuilder.BusinessLogic.UserKey>
	{		
		#region Get from Many To Many Relationship Functions
		#region GetByWorkspaceNameWorkspaceTypeIdFromUserPermission
		
		/// <summary>
		///		Gets User objects from the datasource by WorkspaceName in the
		///		UserPermission table. Table User is related to table Workspace
		///		through the (M:N) relationship defined in the UserPermission table.
		/// </summary>
		/// <param name="_workspaceName"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <returns>Returns a typed collection of User objects.</returns>
		public TList<User> GetByWorkspaceNameWorkspaceTypeIdFromUserPermission(System.String _workspaceName, System.Int32 _WorkspaceTypeId)
		{
			int count = -1;
			return GetByWorkspaceNameWorkspaceTypeIdFromUserPermission(null,_workspaceName, _WorkspaceTypeId, 0, int.MaxValue, out count);
			
		}
		
		/// <summary>
		///		Gets MetaBuilder.BusinessLogic.User objects from the datasource by WorkspaceName in the
		///		UserPermission table. Table User is related to table Workspace
		///		through the (M:N) relationship defined in the UserPermission table.
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_workspaceName"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of User objects.</returns>
		public TList<User> GetByWorkspaceNameWorkspaceTypeIdFromUserPermission(System.String _workspaceName, System.Int32 _WorkspaceTypeId, int start, int pageLength)
		{
			int count = -1;
			return GetByWorkspaceNameWorkspaceTypeIdFromUserPermission(null, _workspaceName, _WorkspaceTypeId, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets User objects from the datasource by WorkspaceName in the
		///		UserPermission table. Table User is related to table Workspace
		///		through the (M:N) relationship defined in the UserPermission table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_workspaceName"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of User objects.</returns>
		public TList<User> GetByWorkspaceNameWorkspaceTypeIdFromUserPermission(TransactionManager transactionManager, System.String _workspaceName, System.Int32 _WorkspaceTypeId)
		{
			int count = -1;
			return GetByWorkspaceNameWorkspaceTypeIdFromUserPermission(transactionManager, _workspaceName, _WorkspaceTypeId, 0, int.MaxValue, out count);
		}
		
		
		/// <summary>
		///		Gets User objects from the datasource by WorkspaceName in the
		///		UserPermission table. Table User is related to table Workspace
		///		through the (M:N) relationship defined in the UserPermission table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_workspaceName"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of User objects.</returns>
		public TList<User> GetByWorkspaceNameWorkspaceTypeIdFromUserPermission(TransactionManager transactionManager, System.String _workspaceName, System.Int32 _WorkspaceTypeId,int start, int pageLength)
		{
			int count = -1;
			return GetByWorkspaceNameWorkspaceTypeIdFromUserPermission(transactionManager, _workspaceName, _WorkspaceTypeId, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets User objects from the datasource by WorkspaceName in the
		///		UserPermission table. Table User is related to table Workspace
		///		through the (M:N) relationship defined in the UserPermission table.
		/// </summary>
		/// <param name="_workspaceName"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of User objects.</returns>
		public TList<User> GetByWorkspaceNameWorkspaceTypeIdFromUserPermission(System.String _workspaceName, System.Int32 _WorkspaceTypeId,int start, int pageLength, out int count)
		{
			
			return GetByWorkspaceNameWorkspaceTypeIdFromUserPermission(null, _workspaceName, _WorkspaceTypeId, start, pageLength, out count);
		}


		/// <summary>
		///		Gets User objects from the datasource by WorkspaceName in the
		///		UserPermission table. Table User is related to table Workspace
		///		through the (M:N) relationship defined in the UserPermission table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <param name="_workspaceName"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of User objects.</returns>
		public abstract TList<User> GetByWorkspaceNameWorkspaceTypeIdFromUserPermission(TransactionManager transactionManager,System.String _workspaceName, System.Int32 _WorkspaceTypeId, int start, int pageLength, out int count);
		
		#endregion GetByWorkspaceNameWorkspaceTypeIdFromUserPermission
		
		#endregion	
		
		#region Delete Methods

		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager">A <see cref="TransactionManager"/> object.</param>
		/// <param name="key">The unique identifier of the row to delete.</param>
		/// <returns>Returns true if operation suceeded.</returns>
		public override bool Delete(TransactionManager transactionManager, MetaBuilder.BusinessLogic.UserKey key)
		{
			return Delete(transactionManager, key.pkid);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="_pkid">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(System.Int32 _pkid)
		{
			return Delete(null, _pkid);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_pkid">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(TransactionManager transactionManager, System.Int32 _pkid);		
		
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
		public override MetaBuilder.BusinessLogic.User Get(TransactionManager transactionManager, MetaBuilder.BusinessLogic.UserKey key, int start, int pageLength)
		{
			return GetBypkid(transactionManager, key.pkid, start, pageLength);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key PK_User index.
		/// </summary>
		/// <param name="_pkid"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.User"/> class.</returns>
		public MetaBuilder.BusinessLogic.User GetBypkid(System.Int32 _pkid)
		{
			int count = -1;
			return GetBypkid(null,_pkid, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_User index.
		/// </summary>
		/// <param name="_pkid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.User"/> class.</returns>
		public MetaBuilder.BusinessLogic.User GetBypkid(System.Int32 _pkid, int start, int pageLength)
		{
			int count = -1;
			return GetBypkid(null, _pkid, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_User index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_pkid"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.User"/> class.</returns>
		public MetaBuilder.BusinessLogic.User GetBypkid(TransactionManager transactionManager, System.Int32 _pkid)
		{
			int count = -1;
			return GetBypkid(transactionManager, _pkid, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_User index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_pkid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.User"/> class.</returns>
		public MetaBuilder.BusinessLogic.User GetBypkid(TransactionManager transactionManager, System.Int32 _pkid, int start, int pageLength)
		{
			int count = -1;
			return GetBypkid(transactionManager, _pkid, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_User index.
		/// </summary>
		/// <param name="_pkid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.User"/> class.</returns>
		public MetaBuilder.BusinessLogic.User GetBypkid(System.Int32 _pkid, int start, int pageLength, out int count)
		{
			return GetBypkid(null, _pkid, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_User index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_pkid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.User"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.User GetBypkid(TransactionManager transactionManager, System.Int32 _pkid, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a TList&lt;User&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="TList&lt;User&gt;"/></returns>
		public static TList<User> Fill(IDataReader reader, TList<User> rows, int start, int pageLength)
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
				
				MetaBuilder.BusinessLogic.User c = null;
				if (useEntityFactory)
				{
					key = new System.Text.StringBuilder("User")
					.Append("|").Append((System.Int32)reader[((int)UserColumn.pkid - 1)]).ToString();
					c = EntityManager.LocateOrCreate<User>(
					key.ToString(), // EntityTrackingKey
					"User",  //Creational Type
					entityCreationFactoryType,  //Factory used to create entity
					enableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.User();
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
					c.pkid = (System.Int32)reader[((int)UserColumn.pkid - 1)];
					c.Name = (System.String)reader[((int)UserColumn.Name - 1)];
					c.Password = (System.String)reader[((int)UserColumn.Password - 1)];
					c.CreateDate = (reader.IsDBNull(((int)UserColumn.CreateDate - 1)))?null:(System.DateTime?)reader[((int)UserColumn.CreateDate - 1)];
					c.LastLogin = (reader.IsDBNull(((int)UserColumn.LastLogin - 1)))?null:(System.DateTime?)reader[((int)UserColumn.LastLogin - 1)];
					c.EntityTrackingKey = key;
					c.AcceptChanges();
					c.SuppressEntityEvents = false;
				}
				rows.Add(c);
			}
		return rows;
		}		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.User"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.User"/> object to refresh.</param>
		public static void RefreshEntity(IDataReader reader, MetaBuilder.BusinessLogic.User entity)
		{
			if (!reader.Read()) return;
			
			entity.pkid = (System.Int32)reader[((int)UserColumn.pkid - 1)];
			entity.Name = (System.String)reader[((int)UserColumn.Name - 1)];
			entity.Password = (System.String)reader[((int)UserColumn.Password - 1)];
			entity.CreateDate = (reader.IsDBNull(((int)UserColumn.CreateDate - 1)))?null:(System.DateTime?)reader[((int)UserColumn.CreateDate - 1)];
			entity.LastLogin = (reader.IsDBNull(((int)UserColumn.LastLogin - 1)))?null:(System.DateTime?)reader[((int)UserColumn.LastLogin - 1)];
			entity.AcceptChanges();
		}
		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.User"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.User"/> object.</param>
		public static void RefreshEntity(DataSet dataSet, MetaBuilder.BusinessLogic.User entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.pkid = (System.Int32)dataRow["pkid"];
			entity.Name = (System.String)dataRow["Name"];
			entity.Password = (System.String)dataRow["Password"];
			entity.CreateDate = Convert.IsDBNull(dataRow["CreateDate"]) ? null : (System.DateTime?)dataRow["CreateDate"];
			entity.LastLogin = Convert.IsDBNull(dataRow["LastLogin"]) ? null : (System.DateTime?)dataRow["LastLogin"];
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
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.User"/> object to load.</param>
		/// <param name="deep">Boolean. A flag that indicates whether to recursively save all Property Collection that are descendants of this instance. If True, saves the complete object graph below this object. If False, saves this object only. </param>
		/// <param name="deepLoadType">DeepLoadType Enumeration to Include/Exclude object property collections from Load.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.User Property Collection Type Array To Include or Exclude from Load</param>
		/// <param name="innerList">A collection of child types for easy access.</param>
	    /// <exception cref="ArgumentNullException">entity or childTypes is null.</exception>
	    /// <exception cref="ArgumentException">deepLoadType has invalid value.</exception>
		public override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.User entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, DeepSession innerList)
		{
			if(entity == null)
				return;
			
			//used to hold DeepLoad method delegates and fire after all the local children have been loaded.
			Dictionary<string, KeyValuePair<Delegate, object>> deepHandles = new Dictionary<string, KeyValuePair<Delegate, object>>();
			// Deep load child collections  - Call GetBypkid methods when available
			
			#region UserPermissionCollection
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<UserPermission>|UserPermissionCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'UserPermissionCollection' loaded. key " + entity.EntityTrackingKey);
				#endif 

				entity.UserPermissionCollection = DataRepository.UserPermissionProvider.GetByUserID(transactionManager, entity.pkid);

				if (deep && entity.UserPermissionCollection.Count > 0)
				{
					deepHandles.Add("UserPermissionCollection",
						new KeyValuePair<Delegate, object>((DeepLoadHandle<UserPermission>) DataRepository.UserPermissionProvider.DeepLoad,
						new object[] { transactionManager, entity.UserPermissionCollection, deep, deepLoadType, childTypes, innerList }
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

				entity.MetaObjectCollection = DataRepository.MetaObjectProvider.GetByUserID(transactionManager, entity.pkid);

				if (deep && entity.MetaObjectCollection.Count > 0)
				{
					deepHandles.Add("MetaObjectCollection",
						new KeyValuePair<Delegate, object>((DeepLoadHandle<MetaObject>) DataRepository.MetaObjectProvider.DeepLoad,
						new object[] { transactionManager, entity.MetaObjectCollection, deep, deepLoadType, childTypes, innerList }
					));
				}
			}		
			#endregion 
			
			
			#region WorkspaceNameWorkspaceTypeIdWorkspaceCollection_From_UserPermission
			// RelationshipType.ManyToMany
			if (CanDeepLoad(entity, "List<Workspace>|WorkspaceNameWorkspaceTypeIdWorkspaceCollection_From_UserPermission", deepLoadType, innerList))
			{
				entity.WorkspaceNameWorkspaceTypeIdWorkspaceCollection_From_UserPermission = DataRepository.WorkspaceProvider.GetByUserIDFromUserPermission(transactionManager, entity.pkid);			 
		
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'WorkspaceNameWorkspaceTypeIdWorkspaceCollection_From_UserPermission' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.WorkspaceNameWorkspaceTypeIdWorkspaceCollection_From_UserPermission != null)
				{
					deepHandles.Add("WorkspaceNameWorkspaceTypeIdWorkspaceCollection_From_UserPermission",
						new KeyValuePair<Delegate, object>((DeepLoadHandle< Workspace >) DataRepository.WorkspaceProvider.DeepLoad,
						new object[] { transactionManager, entity.WorkspaceNameWorkspaceTypeIdWorkspaceCollection_From_UserPermission, deep, deepLoadType, childTypes, innerList }
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
		/// Deep Save the entire object graph of the MetaBuilder.BusinessLogic.User object with criteria based of the child 
		/// Type property array and DeepSaveType.
		/// </summary>
		/// <param name="transactionManager">The transaction manager.</param>
		/// <param name="entity">MetaBuilder.BusinessLogic.User instance</param>
		/// <param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.User Property Collection Type Array To Include or Exclude from Save</param>
		/// <param name="innerList">A Hashtable of child types for easy access.</param>
		public override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.User entity, DeepSaveType deepSaveType, System.Type[] childTypes, DeepSession innerList)
		{	
			if (entity == null)
				return false;
							
			#region Composite Parent Properties
			//Save Source Composite Properties, however, don't call deep save on them.  
			//So they only get saved a single level deep.
			#endregion Composite Parent Properties

			// Save Root Entity through Provider
			if (!entity.IsDeleted)
				this.Save(transactionManager, entity);
			
			//used to hold DeepSave method delegates and fire after all the local children have been saved.
			Dictionary<string, KeyValuePair<Delegate, object>> deepHandles = new Dictionary<string, KeyValuePair<Delegate, object>>();

			#region WorkspaceNameWorkspaceTypeIdWorkspaceCollection_From_UserPermission>
			if (CanDeepSave(entity.WorkspaceNameWorkspaceTypeIdWorkspaceCollection_From_UserPermission, "List<Workspace>|WorkspaceNameWorkspaceTypeIdWorkspaceCollection_From_UserPermission", deepSaveType, innerList))
			{
				if (entity.WorkspaceNameWorkspaceTypeIdWorkspaceCollection_From_UserPermission.Count > 0 || entity.WorkspaceNameWorkspaceTypeIdWorkspaceCollection_From_UserPermission.DeletedItems.Count > 0)
				{
					DataRepository.WorkspaceProvider.Save(transactionManager, entity.WorkspaceNameWorkspaceTypeIdWorkspaceCollection_From_UserPermission); 
					deepHandles.Add("WorkspaceNameWorkspaceTypeIdWorkspaceCollection_From_UserPermission",
						new KeyValuePair<Delegate, object>((DeepSaveHandle<Workspace>) DataRepository.WorkspaceProvider.DeepSave,
						new object[] { transactionManager, entity.WorkspaceNameWorkspaceTypeIdWorkspaceCollection_From_UserPermission, deepSaveType, childTypes, innerList }
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
						if(child.UserIDSource != null)
						{
								child.UserID = child.UserIDSource.pkid;
						}

						if(child.WorkspaceNameWorkspaceTypeIdSource != null)
						{
								child.WorkspaceName = child.WorkspaceNameWorkspaceTypeIdSource.Name;
								child.WorkspaceTypeId = child.WorkspaceNameWorkspaceTypeIdSource.WorkspaceTypeId;
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
						if(child.UserIDSource != null)
						{
							child.UserID = child.UserIDSource.pkid;
						}
						else
						{
							child.UserID = entity.pkid;
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
	
	#region UserChildEntityTypes
	
	///<summary>
	/// Enumeration used to expose the different child entity types 
	/// for child properties in <c>MetaBuilder.BusinessLogic.User</c>
	///</summary>
	public enum UserChildEntityTypes
	{

		///<summary>
		/// Collection of <c>User</c> as OneToMany for UserPermissionCollection
		///</summary>
		[ChildEntityType(typeof(TList<UserPermission>))]
		UserPermissionCollection,

		///<summary>
		/// Collection of <c>User</c> as OneToMany for MetaObjectCollection
		///</summary>
		[ChildEntityType(typeof(TList<MetaObject>))]
		MetaObjectCollection,

		///<summary>
		/// Collection of <c>User</c> as ManyToMany for WorkspaceCollection_From_UserPermission
		///</summary>
		[ChildEntityType(typeof(TList<Workspace>))]
		WorkspaceNameWorkspaceTypeIdWorkspaceCollection_From_UserPermission,
	}
	
	#endregion UserChildEntityTypes
	
	#region UserFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;UserColumn&gt;"/> class
	/// that is used exclusively with a <see cref="User"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class UserFilterBuilder : SqlFilterBuilder<UserColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the UserFilterBuilder class.
		/// </summary>
		public UserFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the UserFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public UserFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the UserFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public UserFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion UserFilterBuilder
	
	#region UserParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;UserColumn&gt;"/> class
	/// that is used exclusively with a <see cref="User"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class UserParameterBuilder : ParameterizedSqlFilterBuilder<UserColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the UserParameterBuilder class.
		/// </summary>
		public UserParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the UserParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public UserParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the UserParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public UserParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion UserParameterBuilder
	
	#region UserSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;UserColumn&gt;"/> class
	/// that is used exclusively with a <see cref="User"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class UserSortBuilder : SqlSortBuilder<UserColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the UserSqlSortBuilder class.
		/// </summary>
		public UserSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion UserSortBuilder
	
} // end namespace
