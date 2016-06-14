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
	/// This class is the base class for any <see cref="UserProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract partial class UserProviderBaseCore : EntityProviderBase<MetaBuilder.BusinessLogic.User, MetaBuilder.BusinessLogic.UserKey>
	{		
		#region Get from Many To Many Relationship Functions
		#region GetByWorkspaceNameWorkspaceTypeIDFromUserPermission
		
		/// <summary>
		///		Gets User objects from the datasource by WorkspaceName in the
		///		UserPermission table. Table User is related to table Workspace
		///		through the (M:N) relationship defined in the UserPermission table.
		/// </summary>
		/// <param name="workspaceName"></param>
		/// <param name="workspaceTypeID"></param>
		/// <returns>Returns a typed collection of User objects.</returns>
		public TList<User> GetByWorkspaceNameWorkspaceTypeIDFromUserPermission(System.String workspaceName, System.Int32 workspaceTypeID)
		{
			int count = -1;
			return GetByWorkspaceNameWorkspaceTypeIDFromUserPermission(null,workspaceName, workspaceTypeID, 0, int.MaxValue, out count);
			
		}
		
		/// <summary>
		///		Gets MetaBuilder.BusinessLogic.User objects from the datasource by WorkspaceName in the
		///		UserPermission table. Table User is related to table Workspace
		///		through the (M:N) relationship defined in the UserPermission table.
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="workspaceName"></param>
		/// <param name="workspaceTypeID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of User objects.</returns>
		public TList<User> GetByWorkspaceNameWorkspaceTypeIDFromUserPermission(System.String workspaceName, System.Int32 workspaceTypeID, int start, int pageLength)
		{
			int count = -1;
			return GetByWorkspaceNameWorkspaceTypeIDFromUserPermission(null, workspaceName, workspaceTypeID, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets User objects from the datasource by WorkspaceName in the
		///		UserPermission table. Table User is related to table Workspace
		///		through the (M:N) relationship defined in the UserPermission table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="workspaceName"></param>
		/// <param name="workspaceTypeID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of User objects.</returns>
		public TList<User> GetByWorkspaceNameWorkspaceTypeIDFromUserPermission(TransactionManager transactionManager, System.String workspaceName, System.Int32 workspaceTypeID)
		{
			int count = -1;
			return GetByWorkspaceNameWorkspaceTypeIDFromUserPermission(transactionManager, workspaceName, workspaceTypeID, 0, int.MaxValue, out count);
		}
		
		
		/// <summary>
		///		Gets User objects from the datasource by WorkspaceName in the
		///		UserPermission table. Table User is related to table Workspace
		///		through the (M:N) relationship defined in the UserPermission table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="workspaceName"></param>
		/// <param name="workspaceTypeID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of User objects.</returns>
		public TList<User> GetByWorkspaceNameWorkspaceTypeIDFromUserPermission(TransactionManager transactionManager, System.String workspaceName, System.Int32 workspaceTypeID,int start, int pageLength)
		{
			int count = -1;
			return GetByWorkspaceNameWorkspaceTypeIDFromUserPermission(transactionManager, workspaceName, workspaceTypeID, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets User objects from the datasource by WorkspaceName in the
		///		UserPermission table. Table User is related to table Workspace
		///		through the (M:N) relationship defined in the UserPermission table.
		/// </summary>
		/// <param name="workspaceName"></param>
		/// <param name="workspaceTypeID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of User objects.</returns>
		public TList<User> GetByWorkspaceNameWorkspaceTypeIDFromUserPermission(System.String workspaceName, System.Int32 workspaceTypeID,int start, int pageLength, out int count)
		{
			
			return GetByWorkspaceNameWorkspaceTypeIDFromUserPermission(null, workspaceName, workspaceTypeID, start, pageLength, out count);
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
		/// <param name="workspaceName"></param>
		/// <param name="workspaceTypeID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of User objects.</returns>
		public abstract TList<User> GetByWorkspaceNameWorkspaceTypeIDFromUserPermission(TransactionManager transactionManager,System.String workspaceName, System.Int32 workspaceTypeID, int start, int pageLength, out int count);
		
		#endregion GetByWorkspaceNameWorkspaceTypeIDFromUserPermission
		
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
		public override MetaBuilder.BusinessLogic.User Get(TransactionManager transactionManager, MetaBuilder.BusinessLogic.UserKey key, int start, int pageLength)
		{
			return GetByPkid(transactionManager, key.Pkid, start, pageLength);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key PK_User index.
		/// </summary>
		/// <param name="pkid"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.User"/> class.</returns>
		public MetaBuilder.BusinessLogic.User GetByPkid(System.Int32 pkid)
		{
			int count = -1;
			return GetByPkid(null,pkid, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_User index.
		/// </summary>
		/// <param name="pkid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.User"/> class.</returns>
		public MetaBuilder.BusinessLogic.User GetByPkid(System.Int32 pkid, int start, int pageLength)
		{
			int count = -1;
			return GetByPkid(null, pkid, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_User index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="pkid"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.User"/> class.</returns>
		public MetaBuilder.BusinessLogic.User GetByPkid(TransactionManager transactionManager, System.Int32 pkid)
		{
			int count = -1;
			return GetByPkid(transactionManager, pkid, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_User index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="pkid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.User"/> class.</returns>
		public MetaBuilder.BusinessLogic.User GetByPkid(TransactionManager transactionManager, System.Int32 pkid, int start, int pageLength)
		{
			int count = -1;
			return GetByPkid(transactionManager, pkid, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_User index.
		/// </summary>
		/// <param name="pkid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.User"/> class.</returns>
		public MetaBuilder.BusinessLogic.User GetByPkid(System.Int32 pkid, int start, int pageLength, out int count)
		{
			return GetByPkid(null, pkid, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_User index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="pkid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.User"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.User GetByPkid(TransactionManager transactionManager, System.Int32 pkid, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a MetaBuilder.BusinessLogic.TList&lt;User&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="MetaBuilder.BusinessLogic.TList&lt;User&gt;"/></returns>
		public static MetaBuilder.BusinessLogic.TList<User> Fill(IDataReader reader, MetaBuilder.BusinessLogic.TList<User> rows, int start, int pageLength)
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
				
				MetaBuilder.BusinessLogic.User c = null;
				if (DataRepository.Provider.UseEntityFactory)
				{
					key = @"User" 
							+ (reader.IsDBNull(reader.GetOrdinal("pkid"))?(int)0:(System.Int32)reader["pkid"]).ToString();

					c = EntityManager.LocateOrCreate<User>(
						key.ToString(), // EntityTrackingKey 
						"User",  //Creational Type
						DataRepository.Provider.EntityCreationalFactoryType,  //Factory used to create entity
						DataRepository.Provider.EnableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.User();
				}
				
				if (!DataRepository.Provider.EnableEntityTracking || c.EntityState == EntityState.Added)
                {
					c.SuppressEntityEvents = true;
					c.Pkid = (System.Int32)reader["pkid"];
					c.Name = (System.String)reader["Name"];
					c.Password = (System.String)reader["Password"];
					c.CreateDate = (reader.IsDBNull(reader.GetOrdinal("CreateDate")))?null:(System.DateTime?)reader["CreateDate"];
					c.LastLogin = (reader.IsDBNull(reader.GetOrdinal("LastLogin")))?null:(System.DateTime?)reader["LastLogin"];
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
			
			entity.Pkid = (System.Int32)reader["pkid"];
			entity.Name = (System.String)reader["Name"];
			entity.Password = (System.String)reader["Password"];
			entity.CreateDate = (reader.IsDBNull(reader.GetOrdinal("CreateDate")))?null:(System.DateTime?)reader["CreateDate"];
			entity.LastLogin = (reader.IsDBNull(reader.GetOrdinal("LastLogin")))?null:(System.DateTime?)reader["LastLogin"];
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
			
			entity.Pkid = (System.Int32)dataRow["pkid"];
			entity.Name = (System.String)dataRow["Name"];
			entity.Password = (System.String)dataRow["Password"];
			entity.CreateDate = (Convert.IsDBNull(dataRow["CreateDate"]))?null:(System.DateTime?)dataRow["CreateDate"];
			entity.LastLogin = (Convert.IsDBNull(dataRow["LastLogin"]))?null:(System.DateTime?)dataRow["LastLogin"];
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
		internal override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.User entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, ChildEntityTypesList innerList)
		{
			if(entity == null)
				return;
			
			// Load Entity through Provider
			// Deep load child collections  - Call GetBypkid methods when available
			
			#region WorkspaceCollection_From_UserPermission
			// RelationshipType.ManyToMany
			if (CanDeepLoad(entity, "List<Workspace>", "WorkspaceCollection_From_UserPermission", deepLoadType, innerList))
			{
				entity.WorkspaceCollection_From_UserPermission = DataRepository.WorkspaceProvider.GetByUserIDFromUserPermission(transactionManager, entity.Pkid);			 
		
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'WorkspaceCollection_From_UserPermission' loaded.");
				#endif 

				if (deep && entity.WorkspaceCollection_From_UserPermission.Count > 0)
				{
					DataRepository.WorkspaceProvider.DeepLoad(transactionManager, entity.WorkspaceCollection_From_UserPermission, deep, deepLoadType, childTypes, innerList);
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

				entity.UserPermissionCollection = DataRepository.UserPermissionProvider.GetByUserID(transactionManager, entity.Pkid);

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

				entity.MetaObjectCollection = DataRepository.MetaObjectProvider.GetByUserID(transactionManager, entity.Pkid);

				if (deep && entity.MetaObjectCollection.Count > 0)
				{
					DataRepository.MetaObjectProvider.DeepLoad(transactionManager, entity.MetaObjectCollection, deep, deepLoadType, childTypes, innerList);
				}
			}		
			#endregion 
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
		internal override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.User entity, DeepSaveType deepSaveType, System.Type[] childTypes, ChildEntityTypesList innerList)
		{	
			if (entity == null)
				return false;
							
			#region Composite Parent Properties
			//Save Source Composite Properties, however, don't call deep save on them.  
			//So they only get saved a single level deep.
			#endregion Composite Parent Properties

			// Save Root Entity through Provider
			this.Save(transactionManager, entity);
			
			




			#region WorkspaceCollection_From_UserPermission>
			if (CanDeepSave(entity, "List<Workspace>", "WorkspaceCollection_From_UserPermission", deepSaveType, innerList))
			{
				if (entity.WorkspaceCollection_From_UserPermission.Count > 0 || entity.WorkspaceCollection_From_UserPermission.DeletedItems.Count > 0)
					DataRepository.WorkspaceProvider.DeepSave(transactionManager, entity.WorkspaceCollection_From_UserPermission, deepSaveType, childTypes, innerList); 
			}
			#endregion 




			#region List<UserPermission>
				if (CanDeepSave(entity, "List<UserPermission>", "UserPermissionCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(UserPermission child in entity.UserPermissionCollection)
					{
						child.UserID = entity.Pkid;
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
						child.UserID = entity.Pkid;
					}
				
				if (entity.MetaObjectCollection.Count > 0 || entity.MetaObjectCollection.DeletedItems.Count > 0)
					DataRepository.MetaObjectProvider.DeepSave(transactionManager, entity.MetaObjectCollection, deepSaveType, childTypes, innerList);
				} 
			#endregion 
				



						
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
		/// Collection of <c>User</c> as ManyToMany for WorkspaceCollection_From_UserPermission
		///</summary>
		[ChildEntityType(typeof(TList<Workspace>))]
		WorkspaceCollection_From_UserPermission,

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
	}
	
	#endregion UserChildEntityTypes
	
	#region UserFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
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
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
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
} // end namespace
