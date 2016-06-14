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
			return Delete(transactionManager, key.UserID, key.WorkspaceName, key.WorkspaceTypeID);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="userID">. Primary Key.</param>
		/// <param name="workspaceName">. Primary Key.</param>
		/// <param name="workspaceTypeID">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(System.Int32 userID, System.String workspaceName, System.Int32 workspaceTypeID)
		{
			return Delete(null, userID, workspaceName, workspaceTypeID);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="userID">. Primary Key.</param>
		/// <param name="workspaceName">. Primary Key.</param>
		/// <param name="workspaceTypeID">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(TransactionManager transactionManager, System.Int32 userID, System.String workspaceName, System.Int32 workspaceTypeID);		
		
		#endregion Delete Methods
		
		#region Get By Foreign Key Functions
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_User key.
		///		FK_UserPermission_User Description: 
		/// </summary>
		/// <param name="userID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		public MetaBuilder.BusinessLogic.TList<UserPermission> GetByUserID(System.Int32 userID)
		{
			int count = -1;
			return GetByUserID(userID, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_User key.
		///		FK_UserPermission_User Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="userID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<UserPermission> GetByUserID(TransactionManager transactionManager, System.Int32 userID)
		{
			int count = -1;
			return GetByUserID(transactionManager, userID, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_User key.
		///		FK_UserPermission_User Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="userID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		public MetaBuilder.BusinessLogic.TList<UserPermission> GetByUserID(TransactionManager transactionManager, System.Int32 userID, int start, int pageLength)
		{
			int count = -1;
			return GetByUserID(transactionManager, userID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_User key.
		///		fKUserPermissionUser Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="userID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		public MetaBuilder.BusinessLogic.TList<UserPermission> GetByUserID(System.Int32 userID, int start, int pageLength)
		{
			int count =  -1;
			return GetByUserID(null, userID, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_User key.
		///		fKUserPermissionUser Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="userID"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		public MetaBuilder.BusinessLogic.TList<UserPermission> GetByUserID(System.Int32 userID, int start, int pageLength,out int count)
		{
			return GetByUserID(null, userID, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_User key.
		///		FK_UserPermission_User Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="userID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<UserPermission> GetByUserID(TransactionManager transactionManager, System.Int32 userID, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_Permission key.
		///		FK_UserPermission_Permission Description: 
		/// </summary>
		/// <param name="permissionID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		public MetaBuilder.BusinessLogic.TList<UserPermission> GetByPermissionID(System.Int32 permissionID)
		{
			int count = -1;
			return GetByPermissionID(permissionID, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_Permission key.
		///		FK_UserPermission_Permission Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="permissionID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<UserPermission> GetByPermissionID(TransactionManager transactionManager, System.Int32 permissionID)
		{
			int count = -1;
			return GetByPermissionID(transactionManager, permissionID, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_Permission key.
		///		FK_UserPermission_Permission Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="permissionID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		public MetaBuilder.BusinessLogic.TList<UserPermission> GetByPermissionID(TransactionManager transactionManager, System.Int32 permissionID, int start, int pageLength)
		{
			int count = -1;
			return GetByPermissionID(transactionManager, permissionID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_Permission key.
		///		fKUserPermissionPermission Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="permissionID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		public MetaBuilder.BusinessLogic.TList<UserPermission> GetByPermissionID(System.Int32 permissionID, int start, int pageLength)
		{
			int count =  -1;
			return GetByPermissionID(null, permissionID, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_Permission key.
		///		fKUserPermissionPermission Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="permissionID"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		public MetaBuilder.BusinessLogic.TList<UserPermission> GetByPermissionID(System.Int32 permissionID, int start, int pageLength,out int count)
		{
			return GetByPermissionID(null, permissionID, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_Permission key.
		///		FK_UserPermission_Permission Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="permissionID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<UserPermission> GetByPermissionID(TransactionManager transactionManager, System.Int32 permissionID, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_Workspace key.
		///		FK_UserPermission_Workspace Description: 
		/// </summary>
		/// <param name="workspaceName"></param>
		/// <param name="workspaceTypeID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		public MetaBuilder.BusinessLogic.TList<UserPermission> GetByWorkspaceNameWorkspaceTypeID(System.String workspaceName, System.Int32 workspaceTypeID)
		{
			int count = -1;
			return GetByWorkspaceNameWorkspaceTypeID(workspaceName, workspaceTypeID, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_Workspace key.
		///		FK_UserPermission_Workspace Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="workspaceName"></param>
		/// <param name="workspaceTypeID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<UserPermission> GetByWorkspaceNameWorkspaceTypeID(TransactionManager transactionManager, System.String workspaceName, System.Int32 workspaceTypeID)
		{
			int count = -1;
			return GetByWorkspaceNameWorkspaceTypeID(transactionManager, workspaceName, workspaceTypeID, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_Workspace key.
		///		FK_UserPermission_Workspace Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="workspaceName"></param>
		/// <param name="workspaceTypeID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		public MetaBuilder.BusinessLogic.TList<UserPermission> GetByWorkspaceNameWorkspaceTypeID(TransactionManager transactionManager, System.String workspaceName, System.Int32 workspaceTypeID, int start, int pageLength)
		{
			int count = -1;
			return GetByWorkspaceNameWorkspaceTypeID(transactionManager, workspaceName, workspaceTypeID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_Workspace key.
		///		fKUserPermissionWorkspace Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="workspaceName"></param>
		/// <param name="workspaceTypeID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		public MetaBuilder.BusinessLogic.TList<UserPermission> GetByWorkspaceNameWorkspaceTypeID(System.String workspaceName, System.Int32 workspaceTypeID, int start, int pageLength)
		{
			int count =  -1;
			return GetByWorkspaceNameWorkspaceTypeID(null, workspaceName, workspaceTypeID, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_Workspace key.
		///		fKUserPermissionWorkspace Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="workspaceName"></param>
		/// <param name="workspaceTypeID"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		public MetaBuilder.BusinessLogic.TList<UserPermission> GetByWorkspaceNameWorkspaceTypeID(System.String workspaceName, System.Int32 workspaceTypeID, int start, int pageLength,out int count)
		{
			return GetByWorkspaceNameWorkspaceTypeID(null, workspaceName, workspaceTypeID, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_UserPermission_Workspace key.
		///		FK_UserPermission_Workspace Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="workspaceName"></param>
		/// <param name="workspaceTypeID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.UserPermission objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<UserPermission> GetByWorkspaceNameWorkspaceTypeID(TransactionManager transactionManager, System.String workspaceName, System.Int32 workspaceTypeID, int start, int pageLength, out int count);
		
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
			return GetByUserIDWorkspaceNameWorkspaceTypeID(transactionManager, key.UserID, key.WorkspaceName, key.WorkspaceTypeID, start, pageLength);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key PK_UserPermission index.
		/// </summary>
		/// <param name="userID"></param>
		/// <param name="workspaceName"></param>
		/// <param name="workspaceTypeID"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.UserPermission"/> class.</returns>
		public MetaBuilder.BusinessLogic.UserPermission GetByUserIDWorkspaceNameWorkspaceTypeID(System.Int32 userID, System.String workspaceName, System.Int32 workspaceTypeID)
		{
			int count = -1;
			return GetByUserIDWorkspaceNameWorkspaceTypeID(null,userID, workspaceName, workspaceTypeID, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_UserPermission index.
		/// </summary>
		/// <param name="userID"></param>
		/// <param name="workspaceName"></param>
		/// <param name="workspaceTypeID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.UserPermission"/> class.</returns>
		public MetaBuilder.BusinessLogic.UserPermission GetByUserIDWorkspaceNameWorkspaceTypeID(System.Int32 userID, System.String workspaceName, System.Int32 workspaceTypeID, int start, int pageLength)
		{
			int count = -1;
			return GetByUserIDWorkspaceNameWorkspaceTypeID(null, userID, workspaceName, workspaceTypeID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_UserPermission index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="userID"></param>
		/// <param name="workspaceName"></param>
		/// <param name="workspaceTypeID"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.UserPermission"/> class.</returns>
		public MetaBuilder.BusinessLogic.UserPermission GetByUserIDWorkspaceNameWorkspaceTypeID(TransactionManager transactionManager, System.Int32 userID, System.String workspaceName, System.Int32 workspaceTypeID)
		{
			int count = -1;
			return GetByUserIDWorkspaceNameWorkspaceTypeID(transactionManager, userID, workspaceName, workspaceTypeID, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_UserPermission index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="userID"></param>
		/// <param name="workspaceName"></param>
		/// <param name="workspaceTypeID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.UserPermission"/> class.</returns>
		public MetaBuilder.BusinessLogic.UserPermission GetByUserIDWorkspaceNameWorkspaceTypeID(TransactionManager transactionManager, System.Int32 userID, System.String workspaceName, System.Int32 workspaceTypeID, int start, int pageLength)
		{
			int count = -1;
			return GetByUserIDWorkspaceNameWorkspaceTypeID(transactionManager, userID, workspaceName, workspaceTypeID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_UserPermission index.
		/// </summary>
		/// <param name="userID"></param>
		/// <param name="workspaceName"></param>
		/// <param name="workspaceTypeID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.UserPermission"/> class.</returns>
		public MetaBuilder.BusinessLogic.UserPermission GetByUserIDWorkspaceNameWorkspaceTypeID(System.Int32 userID, System.String workspaceName, System.Int32 workspaceTypeID, int start, int pageLength, out int count)
		{
			return GetByUserIDWorkspaceNameWorkspaceTypeID(null, userID, workspaceName, workspaceTypeID, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_UserPermission index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="userID"></param>
		/// <param name="workspaceName"></param>
		/// <param name="workspaceTypeID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.UserPermission"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.UserPermission GetByUserIDWorkspaceNameWorkspaceTypeID(TransactionManager transactionManager, System.Int32 userID, System.String workspaceName, System.Int32 workspaceTypeID, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a MetaBuilder.BusinessLogic.TList&lt;UserPermission&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="MetaBuilder.BusinessLogic.TList&lt;UserPermission&gt;"/></returns>
		public static MetaBuilder.BusinessLogic.TList<UserPermission> Fill(IDataReader reader, MetaBuilder.BusinessLogic.TList<UserPermission> rows, int start, int pageLength)
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
				
				MetaBuilder.BusinessLogic.UserPermission c = null;
				if (DataRepository.Provider.UseEntityFactory)
				{
					key = @"UserPermission" 
							+ (reader.IsDBNull(reader.GetOrdinal("UserID"))?(int)0:(System.Int32)reader["UserID"]).ToString()
							+ (reader.IsDBNull(reader.GetOrdinal("WorkspaceName"))?string.Empty:(System.String)reader["WorkspaceName"]).ToString()
							+ (reader.IsDBNull(reader.GetOrdinal("WorkspaceTypeID"))?(int)0:(System.Int32)reader["WorkspaceTypeID"]).ToString();

					c = EntityManager.LocateOrCreate<UserPermission>(
						key.ToString(), // EntityTrackingKey 
						"UserPermission",  //Creational Type
						DataRepository.Provider.EntityCreationalFactoryType,  //Factory used to create entity
						DataRepository.Provider.EnableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.UserPermission();
				}
				
				if (!DataRepository.Provider.EnableEntityTracking || c.EntityState == EntityState.Added)
                {
					c.SuppressEntityEvents = true;
					c.UserID = (System.Int32)reader["UserID"];
					c.OriginalUserID = c.UserID; //(reader.IsDBNull(reader.GetOrdinal("UserID")))?(int)0:(System.Int32)reader["UserID"];
					c.PermissionID = (System.Int32)reader["PermissionID"];
					c.WorkspaceName = (System.String)reader["WorkspaceName"];
					c.OriginalWorkspaceName = c.WorkspaceName; //(reader.IsDBNull(reader.GetOrdinal("WorkspaceName")))?string.Empty:(System.String)reader["WorkspaceName"];
					c.WorkspaceTypeID = (System.Int32)reader["WorkspaceTypeID"];
					c.OriginalWorkspaceTypeID = c.WorkspaceTypeID; //(reader.IsDBNull(reader.GetOrdinal("WorkspaceTypeID")))?(int)0:(System.Int32)reader["WorkspaceTypeID"];
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
			
			entity.UserID = (System.Int32)reader["UserID"];
			entity.OriginalUserID = (System.Int32)reader["UserID"];
			entity.PermissionID = (System.Int32)reader["PermissionID"];
			entity.WorkspaceName = (System.String)reader["WorkspaceName"];
			entity.OriginalWorkspaceName = (System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeID = (System.Int32)reader["WorkspaceTypeID"];
			entity.OriginalWorkspaceTypeID = (System.Int32)reader["WorkspaceTypeID"];
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
			entity.WorkspaceTypeID = (System.Int32)dataRow["WorkspaceTypeID"];
			entity.OriginalWorkspaceTypeID = (System.Int32)dataRow["WorkspaceTypeID"];
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
		internal override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.UserPermission entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, ChildEntityTypesList innerList)
		{
			if(entity == null)
				return;

			#region UserIDSource	
			if (CanDeepLoad(entity, "User", "UserIDSource", deepLoadType, innerList) 
				&& entity.UserIDSource == null)
			{
				object[] pkItems = new object[1];
				pkItems[0] = entity.UserID;
				User tmpEntity = EntityManager.LocateEntity<User>(EntityLocator.ConstructKeyFromPkItems(typeof(User), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.UserIDSource = tmpEntity;
				else
					entity.UserIDSource = DataRepository.UserProvider.GetByPkid(entity.UserID);
			
				if (deep && entity.UserIDSource != null)
				{
					DataRepository.UserProvider.DeepLoad(transactionManager, entity.UserIDSource, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion UserIDSource

			#region PermissionIDSource	
			if (CanDeepLoad(entity, "Permission", "PermissionIDSource", deepLoadType, innerList) 
				&& entity.PermissionIDSource == null)
			{
				object[] pkItems = new object[1];
				pkItems[0] = entity.PermissionID;
				Permission tmpEntity = EntityManager.LocateEntity<Permission>(EntityLocator.ConstructKeyFromPkItems(typeof(Permission), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.PermissionIDSource = tmpEntity;
				else
					entity.PermissionIDSource = DataRepository.PermissionProvider.GetByPkid(entity.PermissionID);
			
				if (deep && entity.PermissionIDSource != null)
				{
					DataRepository.PermissionProvider.DeepLoad(transactionManager, entity.PermissionIDSource, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion PermissionIDSource

			#region WorkspaceNameWorkspaceTypeIDSource	
			if (CanDeepLoad(entity, "Workspace", "WorkspaceNameWorkspaceTypeIDSource", deepLoadType, innerList) 
				&& entity.WorkspaceNameWorkspaceTypeIDSource == null)
			{
				object[] pkItems = new object[2];
				pkItems[0] = entity.WorkspaceName;
				pkItems[1] = entity.WorkspaceTypeID;
				Workspace tmpEntity = EntityManager.LocateEntity<Workspace>(EntityLocator.ConstructKeyFromPkItems(typeof(Workspace), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.WorkspaceNameWorkspaceTypeIDSource = tmpEntity;
				else
					entity.WorkspaceNameWorkspaceTypeIDSource = DataRepository.WorkspaceProvider.GetByNameWorkspaceTypeID(entity.WorkspaceName, entity.WorkspaceTypeID);
			
				if (deep && entity.WorkspaceNameWorkspaceTypeIDSource != null)
				{
					DataRepository.WorkspaceProvider.DeepLoad(transactionManager, entity.WorkspaceNameWorkspaceTypeIDSource, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion WorkspaceNameWorkspaceTypeIDSource
			
			// Load Entity through Provider
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
		internal override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.UserPermission entity, DeepSaveType deepSaveType, System.Type[] childTypes, ChildEntityTypesList innerList)
		{	
			if (entity == null)
				return false;
							
			#region Composite Parent Properties
			//Save Source Composite Properties, however, don't call deep save on them.  
			//So they only get saved a single level deep.
			
			#region UserIDSource
			if (CanDeepSave(entity, "User", "UserIDSource", deepSaveType, innerList) 
				&& entity.UserIDSource != null)
			{
				DataRepository.UserProvider.Save(transactionManager, entity.UserIDSource);
				entity.UserID = entity.UserIDSource.Pkid;
			}
			#endregion 
			
			#region PermissionIDSource
			if (CanDeepSave(entity, "Permission", "PermissionIDSource", deepSaveType, innerList) 
				&& entity.PermissionIDSource != null)
			{
				DataRepository.PermissionProvider.Save(transactionManager, entity.PermissionIDSource);
				entity.PermissionID = entity.PermissionIDSource.Pkid;
			}
			#endregion 
			
			#region WorkspaceNameWorkspaceTypeIDSource
			if (CanDeepSave(entity, "Workspace", "WorkspaceNameWorkspaceTypeIDSource", deepSaveType, innerList) 
				&& entity.WorkspaceNameWorkspaceTypeIDSource != null)
			{
				DataRepository.WorkspaceProvider.Save(transactionManager, entity.WorkspaceNameWorkspaceTypeIDSource);
				entity.WorkspaceName = entity.WorkspaceNameWorkspaceTypeIDSource.Name;
				entity.WorkspaceTypeID = entity.WorkspaceNameWorkspaceTypeIDSource.WorkspaceTypeID;
			}
			#endregion 
			#endregion Composite Parent Properties

			// Save Root Entity through Provider
			this.Save(transactionManager, entity);
			
			
						
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
		/// Composite Property for <c>Workspace</c> at WorkspaceNameWorkspaceTypeIDSource
		///</summary>
		[ChildEntityType(typeof(Workspace))]
		Workspace,
		}
	
	#endregion UserPermissionChildEntityTypes
	
	#region UserPermissionFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
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
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
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
} // end namespace
