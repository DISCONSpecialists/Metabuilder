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
	/// This class is the base class for any <see cref="UserPermissionProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract partial class UserPermissionProviderBaseCore : EntityProviderBase<MetaBuilder.BusinessLogic.UserPermission, MetaBuilder.BusinessLogic.UserPermissionKey>
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
		public override bool Delete(TransactionManager transactionManager, MetaBuilder.BusinessLogic.UserPermissionKey key)
		{
			return Delete(transactionManager, key.UserID, key.WorkspaceName, key.WorkspaceTypeId);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="_userID">. Primary Key.</param>
		/// <param name="_workspaceName">. Primary Key.</param>
		/// <param name="_WorkspaceTypeId">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(System.Int32 _userID, System.String _workspaceName, System.Int32 _WorkspaceTypeId)
		{
			return Delete(null, _userID, _workspaceName, _WorkspaceTypeId);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_userID">. Primary Key.</param>
		/// <param name="_workspaceName">. Primary Key.</param>
		/// <param name="_WorkspaceTypeId">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(TransactionManager transactionManager, System.Int32 _userID, System.String _workspaceName, System.Int32 _WorkspaceTypeId);		
		
		#endregion Delete Methods
		
		#region Get By Foreign Key Functions
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_User key.
		///		FK_UserPermission_User Description: 
		/// </summary>
		/// <param name="_userID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		public TList<UserPermission> GetByUserID(System.Int32 _userID)
		{
			int count = -1;
			return GetByUserID(_userID, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_User key.
		///		FK_UserPermission_User Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_userID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		/// <remarks></remarks>
		public TList<UserPermission> GetByUserID(TransactionManager transactionManager, System.Int32 _userID)
		{
			int count = -1;
			return GetByUserID(transactionManager, _userID, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_User key.
		///		FK_UserPermission_User Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_userID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		public TList<UserPermission> GetByUserID(TransactionManager transactionManager, System.Int32 _userID, int start, int pageLength)
		{
			int count = -1;
			return GetByUserID(transactionManager, _userID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_User key.
		///		fK_UserPermission_User Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_userID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		public TList<UserPermission> GetByUserID(System.Int32 _userID, int start, int pageLength)
		{
			int count =  -1;
			return GetByUserID(null, _userID, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_User key.
		///		fK_UserPermission_User Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_userID"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		public TList<UserPermission> GetByUserID(System.Int32 _userID, int start, int pageLength,out int count)
		{
			return GetByUserID(null, _userID, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_User key.
		///		FK_UserPermission_User Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_userID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		public abstract TList<UserPermission> GetByUserID(TransactionManager transactionManager, System.Int32 _userID, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_Permission key.
		///		FK_UserPermission_Permission Description: 
		/// </summary>
		/// <param name="_permissionID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		public TList<UserPermission> GetByPermissionID(System.Int32 _permissionID)
		{
			int count = -1;
			return GetByPermissionID(_permissionID, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_Permission key.
		///		FK_UserPermission_Permission Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_permissionID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		/// <remarks></remarks>
		public TList<UserPermission> GetByPermissionID(TransactionManager transactionManager, System.Int32 _permissionID)
		{
			int count = -1;
			return GetByPermissionID(transactionManager, _permissionID, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_Permission key.
		///		FK_UserPermission_Permission Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_permissionID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		public TList<UserPermission> GetByPermissionID(TransactionManager transactionManager, System.Int32 _permissionID, int start, int pageLength)
		{
			int count = -1;
			return GetByPermissionID(transactionManager, _permissionID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_Permission key.
		///		fK_UserPermission_Permission Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_permissionID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		public TList<UserPermission> GetByPermissionID(System.Int32 _permissionID, int start, int pageLength)
		{
			int count =  -1;
			return GetByPermissionID(null, _permissionID, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_Permission key.
		///		fK_UserPermission_Permission Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_permissionID"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		public TList<UserPermission> GetByPermissionID(System.Int32 _permissionID, int start, int pageLength,out int count)
		{
			return GetByPermissionID(null, _permissionID, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_Permission key.
		///		FK_UserPermission_Permission Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_permissionID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		public abstract TList<UserPermission> GetByPermissionID(TransactionManager transactionManager, System.Int32 _permissionID, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_Workspace key.
		///		FK_UserPermission_Workspace Description: 
		/// </summary>
		/// <param name="_workspaceName"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		public TList<UserPermission> GetByWorkspaceNameWorkspaceTypeId(System.String _workspaceName, System.Int32 _WorkspaceTypeId)
		{
			int count = -1;
			return GetByWorkspaceNameWorkspaceTypeId(_workspaceName, _WorkspaceTypeId, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_Workspace key.
		///		FK_UserPermission_Workspace Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_workspaceName"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		/// <remarks></remarks>
		public TList<UserPermission> GetByWorkspaceNameWorkspaceTypeId(TransactionManager transactionManager, System.String _workspaceName, System.Int32 _WorkspaceTypeId)
		{
			int count = -1;
			return GetByWorkspaceNameWorkspaceTypeId(transactionManager, _workspaceName, _WorkspaceTypeId, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_Workspace key.
		///		FK_UserPermission_Workspace Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_workspaceName"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		public TList<UserPermission> GetByWorkspaceNameWorkspaceTypeId(TransactionManager transactionManager, System.String _workspaceName, System.Int32 _WorkspaceTypeId, int start, int pageLength)
		{
			int count = -1;
			return GetByWorkspaceNameWorkspaceTypeId(transactionManager, _workspaceName, _WorkspaceTypeId, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_Workspace key.
		///		fK_UserPermission_Workspace Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_workspaceName"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		public TList<UserPermission> GetByWorkspaceNameWorkspaceTypeId(System.String _workspaceName, System.Int32 _WorkspaceTypeId, int start, int pageLength)
		{
			int count =  -1;
			return GetByWorkspaceNameWorkspaceTypeId(null, _workspaceName, _WorkspaceTypeId, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_Workspace key.
		///		fK_UserPermission_Workspace Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_workspaceName"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		public TList<UserPermission> GetByWorkspaceNameWorkspaceTypeId(System.String _workspaceName, System.Int32 _WorkspaceTypeId, int start, int pageLength,out int count)
		{
			return GetByWorkspaceNameWorkspaceTypeId(null, _workspaceName, _WorkspaceTypeId, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_Workspace key.
		///		FK_UserPermission_Workspace Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_workspaceName"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		public abstract TList<UserPermission> GetByWorkspaceNameWorkspaceTypeId(TransactionManager transactionManager, System.String _workspaceName, System.Int32 _WorkspaceTypeId, int start, int pageLength, out int count);
		
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
		public override MetaBuilder.BusinessLogic.UserPermission Get(TransactionManager transactionManager, MetaBuilder.BusinessLogic.UserPermissionKey key, int start, int pageLength)
		{
			return GetByUserIDWorkspaceNameWorkspaceTypeId(transactionManager, key.UserID, key.WorkspaceName, key.WorkspaceTypeId, start, pageLength);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key PK_UserPermission index.
		/// </summary>
		/// <param name="_userID"></param>
		/// <param name="_workspaceName"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.UserPermission"/> class.</returns>
		public MetaBuilder.BusinessLogic.UserPermission GetByUserIDWorkspaceNameWorkspaceTypeId(System.Int32 _userID, System.String _workspaceName, System.Int32 _WorkspaceTypeId)
		{
			int count = -1;
			return GetByUserIDWorkspaceNameWorkspaceTypeId(null,_userID, _workspaceName, _WorkspaceTypeId, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_UserPermission index.
		/// </summary>
		/// <param name="_userID"></param>
		/// <param name="_workspaceName"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.UserPermission"/> class.</returns>
		public MetaBuilder.BusinessLogic.UserPermission GetByUserIDWorkspaceNameWorkspaceTypeId(System.Int32 _userID, System.String _workspaceName, System.Int32 _WorkspaceTypeId, int start, int pageLength)
		{
			int count = -1;
			return GetByUserIDWorkspaceNameWorkspaceTypeId(null, _userID, _workspaceName, _WorkspaceTypeId, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_UserPermission index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_userID"></param>
		/// <param name="_workspaceName"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.UserPermission"/> class.</returns>
		public MetaBuilder.BusinessLogic.UserPermission GetByUserIDWorkspaceNameWorkspaceTypeId(TransactionManager transactionManager, System.Int32 _userID, System.String _workspaceName, System.Int32 _WorkspaceTypeId)
		{
			int count = -1;
			return GetByUserIDWorkspaceNameWorkspaceTypeId(transactionManager, _userID, _workspaceName, _WorkspaceTypeId, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_UserPermission index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_userID"></param>
		/// <param name="_workspaceName"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.UserPermission"/> class.</returns>
		public MetaBuilder.BusinessLogic.UserPermission GetByUserIDWorkspaceNameWorkspaceTypeId(TransactionManager transactionManager, System.Int32 _userID, System.String _workspaceName, System.Int32 _WorkspaceTypeId, int start, int pageLength)
		{
			int count = -1;
			return GetByUserIDWorkspaceNameWorkspaceTypeId(transactionManager, _userID, _workspaceName, _WorkspaceTypeId, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_UserPermission index.
		/// </summary>
		/// <param name="_userID"></param>
		/// <param name="_workspaceName"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.UserPermission"/> class.</returns>
		public MetaBuilder.BusinessLogic.UserPermission GetByUserIDWorkspaceNameWorkspaceTypeId(System.Int32 _userID, System.String _workspaceName, System.Int32 _WorkspaceTypeId, int start, int pageLength, out int count)
		{
			return GetByUserIDWorkspaceNameWorkspaceTypeId(null, _userID, _workspaceName, _WorkspaceTypeId, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_UserPermission index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_userID"></param>
		/// <param name="_workspaceName"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.UserPermission"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.UserPermission GetByUserIDWorkspaceNameWorkspaceTypeId(TransactionManager transactionManager, System.Int32 _userID, System.String _workspaceName, System.Int32 _WorkspaceTypeId, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a TList&lt;UserPermission&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="TList&lt;UserPermission&gt;"/></returns>
		public static TList<UserPermission> Fill(IDataReader reader, TList<UserPermission> rows, int start, int pageLength)
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
				
				MetaBuilder.BusinessLogic.UserPermission c = null;
				if (useEntityFactory)
				{
					key = new System.Text.StringBuilder("UserPermission")
					.Append("|").Append((System.Int32)reader[((int)UserPermissionColumn.UserID - 1)])
					.Append("|").Append((System.String)reader[((int)UserPermissionColumn.WorkspaceName - 1)])
					.Append("|").Append((System.Int32)reader[((int)UserPermissionColumn.WorkspaceTypeId - 1)]).ToString();
					c = EntityManager.LocateOrCreate<UserPermission>(
					key.ToString(), // EntityTrackingKey
					"UserPermission",  //Creational Type
					entityCreationFactoryType,  //Factory used to create entity
					enableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.UserPermission();
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
					c.UserID = (System.Int32)reader[((int)UserPermissionColumn.UserID - 1)];
					c.OriginalUserID = c.UserID;
					c.PermissionID = (System.Int32)reader[((int)UserPermissionColumn.PermissionID - 1)];
					c.WorkspaceName = (System.String)reader[((int)UserPermissionColumn.WorkspaceName - 1)];
					c.OriginalWorkspaceName = c.WorkspaceName;
					c.WorkspaceTypeId = (System.Int32)reader[((int)UserPermissionColumn.WorkspaceTypeId - 1)];
					c.OriginalWorkspaceTypeId = c.WorkspaceTypeId;
					c.EntityTrackingKey = key;
					c.AcceptChanges();
					c.SuppressEntityEvents = false;
				}
				rows.Add(c);
			}
		return rows;
		}		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.UserPermission"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.UserPermission"/> object to refresh.</param>
		public static void RefreshEntity(IDataReader reader, MetaBuilder.BusinessLogic.UserPermission entity)
		{
			if (!reader.Read()) return;
			
			entity.UserID = (System.Int32)reader[((int)UserPermissionColumn.UserID - 1)];
			entity.OriginalUserID = (System.Int32)reader["UserID"];
			entity.PermissionID = (System.Int32)reader[((int)UserPermissionColumn.PermissionID - 1)];
			entity.WorkspaceName = (System.String)reader[((int)UserPermissionColumn.WorkspaceName - 1)];
			entity.OriginalWorkspaceName = (System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeId = (System.Int32)reader[((int)UserPermissionColumn.WorkspaceTypeId - 1)];
			entity.OriginalWorkspaceTypeId = (System.Int32)reader["WorkspaceTypeId"];
			entity.AcceptChanges();
		}
		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.UserPermission"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.UserPermission"/> object.</param>
		public static void RefreshEntity(DataSet dataSet, MetaBuilder.BusinessLogic.UserPermission entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.UserID = (System.Int32)dataRow["UserID"];
			entity.OriginalUserID = (System.Int32)dataRow["UserID"];
			entity.PermissionID = (System.Int32)dataRow["PermissionID"];
			entity.WorkspaceName = (System.String)dataRow["WorkspaceName"];
			entity.OriginalWorkspaceName = (System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeId = (System.Int32)dataRow["WorkspaceTypeId"];
			entity.OriginalWorkspaceTypeId = (System.Int32)dataRow["WorkspaceTypeId"];
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
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.UserPermission"/> object to load.</param>
		/// <param name="deep">Boolean. A flag that indicates whether to recursively save all Property Collection that are descendants of this instance. If True, saves the complete object graph below this object. If False, saves this object only. </param>
		/// <param name="deepLoadType">DeepLoadType Enumeration to Include/Exclude object property collections from Load.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.UserPermission Property Collection Type Array To Include or Exclude from Load</param>
		/// <param name="innerList">A collection of child types for easy access.</param>
	    /// <exception cref="ArgumentNullException">entity or childTypes is null.</exception>
	    /// <exception cref="ArgumentException">deepLoadType has invalid value.</exception>
		public override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.UserPermission entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, DeepSession innerList)
		{
			if(entity == null)
				return;

			#region UserIDSource	
			if (CanDeepLoad(entity, "User|UserIDSource", deepLoadType, innerList) 
				&& entity.UserIDSource == null)
			{
				object[] pkItems = new object[1];
				pkItems[0] = entity.UserID;
				User tmpEntity = EntityManager.LocateEntity<User>(EntityLocator.ConstructKeyFromPkItems(typeof(User), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.UserIDSource = tmpEntity;
				else
					entity.UserIDSource = DataRepository.UserProvider.GetBypkid(transactionManager, entity.UserID);		
				
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'UserIDSource' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.UserIDSource != null)
				{
					innerList.SkipChildren = true;
					DataRepository.UserProvider.DeepLoad(transactionManager, entity.UserIDSource, deep, deepLoadType, childTypes, innerList);
					innerList.SkipChildren = false;
				}
					
			}
			#endregion UserIDSource

			#region PermissionIDSource	
			if (CanDeepLoad(entity, "Permission|PermissionIDSource", deepLoadType, innerList) 
				&& entity.PermissionIDSource == null)
			{
				object[] pkItems = new object[1];
				pkItems[0] = entity.PermissionID;
				Permission tmpEntity = EntityManager.LocateEntity<Permission>(EntityLocator.ConstructKeyFromPkItems(typeof(Permission), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.PermissionIDSource = tmpEntity;
				else
					entity.PermissionIDSource = DataRepository.PermissionProvider.GetBypkid(transactionManager, entity.PermissionID);		
				
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'PermissionIDSource' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.PermissionIDSource != null)
				{
					innerList.SkipChildren = true;
					DataRepository.PermissionProvider.DeepLoad(transactionManager, entity.PermissionIDSource, deep, deepLoadType, childTypes, innerList);
					innerList.SkipChildren = false;
				}
					
			}
			#endregion PermissionIDSource

			#region WorkspaceNameWorkspaceTypeIdSource	
			if (CanDeepLoad(entity, "Workspace|WorkspaceNameWorkspaceTypeIdSource", deepLoadType, innerList) 
				&& entity.WorkspaceNameWorkspaceTypeIdSource == null)
			{
				object[] pkItems = new object[2];
				pkItems[0] = entity.WorkspaceName;
				pkItems[1] = entity.WorkspaceTypeId;
				Workspace tmpEntity = EntityManager.LocateEntity<Workspace>(EntityLocator.ConstructKeyFromPkItems(typeof(Workspace), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.WorkspaceNameWorkspaceTypeIdSource = tmpEntity;
				else
					entity.WorkspaceNameWorkspaceTypeIdSource = DataRepository.WorkspaceProvider.GetByNameWorkspaceTypeId(transactionManager, entity.WorkspaceName, entity.WorkspaceTypeId);		
				
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'WorkspaceNameWorkspaceTypeIdSource' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.WorkspaceNameWorkspaceTypeIdSource != null)
				{
					innerList.SkipChildren = true;
					DataRepository.WorkspaceProvider.DeepLoad(transactionManager, entity.WorkspaceNameWorkspaceTypeIdSource, deep, deepLoadType, childTypes, innerList);
					innerList.SkipChildren = false;
				}
					
			}
			#endregion WorkspaceNameWorkspaceTypeIdSource
			
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
		/// Deep Save the entire object graph of the MetaBuilder.BusinessLogic.UserPermission object with criteria based of the child 
		/// Type property array and DeepSaveType.
		/// </summary>
		/// <param name="transactionManager">The transaction manager.</param>
		/// <param name="entity">MetaBuilder.BusinessLogic.UserPermission instance</param>
		/// <param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.UserPermission Property Collection Type Array To Include or Exclude from Save</param>
		/// <param name="innerList">A Hashtable of child types for easy access.</param>
		public override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.UserPermission entity, DeepSaveType deepSaveType, System.Type[] childTypes, DeepSession innerList)
		{	
			if (entity == null)
				return false;
							
			#region Composite Parent Properties
			//Save Source Composite Properties, however, don't call deep save on them.  
			//So they only get saved a single level deep.
			
			#region UserIDSource
			if (CanDeepSave(entity, "User|UserIDSource", deepSaveType, innerList) 
				&& entity.UserIDSource != null)
			{
				DataRepository.UserProvider.Save(transactionManager, entity.UserIDSource);
				entity.UserID = entity.UserIDSource.pkid;
			}
			#endregion 
			
			#region PermissionIDSource
			if (CanDeepSave(entity, "Permission|PermissionIDSource", deepSaveType, innerList) 
				&& entity.PermissionIDSource != null)
			{
				DataRepository.PermissionProvider.Save(transactionManager, entity.PermissionIDSource);
				entity.PermissionID = entity.PermissionIDSource.pkid;
			}
			#endregion 
			
			#region WorkspaceNameWorkspaceTypeIdSource
			if (CanDeepSave(entity, "Workspace|WorkspaceNameWorkspaceTypeIdSource", deepSaveType, innerList) 
				&& entity.WorkspaceNameWorkspaceTypeIdSource != null)
			{
				DataRepository.WorkspaceProvider.Save(transactionManager, entity.WorkspaceNameWorkspaceTypeIdSource);
				entity.WorkspaceName = entity.WorkspaceNameWorkspaceTypeIdSource.Name;
				entity.WorkspaceTypeId = entity.WorkspaceNameWorkspaceTypeIdSource.WorkspaceTypeId;
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
	
	#region UserPermissionChildEntityTypes
	
	///<summary>
	/// Enumeration used to expose the different child entity types 
	/// for child properties in <c>MetaBuilder.BusinessLogic.UserPermission</c>
	///</summary>
	public enum UserPermissionChildEntityTypes
	{
		
		///<summary>
		/// Composite Property for <c>User</c> at UserIDSource
		///</summary>
		[ChildEntityType(typeof(User))]
		User,
			
		///<summary>
		/// Composite Property for <c>Permission</c> at PermissionIDSource
		///</summary>
		[ChildEntityType(typeof(Permission))]
		Permission,
			
		///<summary>
		/// Composite Property for <c>Workspace</c> at WorkspaceNameWorkspaceTypeIdSource
		///</summary>
		[ChildEntityType(typeof(Workspace))]
		Workspace,
		}
	
	#endregion UserPermissionChildEntityTypes
	
	#region UserPermissionFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;UserPermissionColumn&gt;"/> class
	/// that is used exclusively with a <see cref="UserPermission"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class UserPermissionFilterBuilder : SqlFilterBuilder<UserPermissionColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the UserPermissionFilterBuilder class.
		/// </summary>
		public UserPermissionFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the UserPermissionFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public UserPermissionFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the UserPermissionFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public UserPermissionFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion UserPermissionFilterBuilder
	
	#region UserPermissionParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;UserPermissionColumn&gt;"/> class
	/// that is used exclusively with a <see cref="UserPermission"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class UserPermissionParameterBuilder : ParameterizedSqlFilterBuilder<UserPermissionColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the UserPermissionParameterBuilder class.
		/// </summary>
		public UserPermissionParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the UserPermissionParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public UserPermissionParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the UserPermissionParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public UserPermissionParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion UserPermissionParameterBuilder
	
	#region UserPermissionSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;UserPermissionColumn&gt;"/> class
	/// that is used exclusively with a <see cref="UserPermission"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class UserPermissionSortBuilder : SqlSortBuilder<UserPermissionColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the UserPermissionSqlSortBuilder class.
		/// </summary>
		public UserPermissionSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion UserPermissionSortBuilder
	
} // end namespace
