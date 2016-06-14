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
	/// This class is the base class for any <see cref="MetaObjectProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract partial class MetaObjectProviderBaseCore : EntityProviderBase<MetaBuilder.BusinessLogic.MetaObject, MetaBuilder.BusinessLogic.MetaObjectKey>
	{		
		#region Get from Many To Many Relationship Functions
		#region GetByFieldIDFromObjectFieldValue
		
		/// <summary>
		///		Gets MetaObject objects from the datasource by FieldID in the
		///		ObjectFieldValue table. Table MetaObject is related to table Field
		///		through the (M:N) relationship defined in the ObjectFieldValue table.
		/// </summary>
		/// <param name="fieldID"></param>
		/// <returns>Returns a typed collection of MetaObject objects.</returns>
		public TList<MetaObject> GetByFieldIDFromObjectFieldValue(System.Int32 fieldID)
		{
			int count = -1;
			return GetByFieldIDFromObjectFieldValue(null,fieldID, 0, int.MaxValue, out count);
			
		}
		
		/// <summary>
		///		Gets MetaBuilder.BusinessLogic.MetaObject objects from the datasource by FieldID in the
		///		ObjectFieldValue table. Table MetaObject is related to table Field
		///		through the (M:N) relationship defined in the ObjectFieldValue table.
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="fieldID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of MetaObject objects.</returns>
		public TList<MetaObject> GetByFieldIDFromObjectFieldValue(System.Int32 fieldID, int start, int pageLength)
		{
			int count = -1;
			return GetByFieldIDFromObjectFieldValue(null, fieldID, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets MetaObject objects from the datasource by FieldID in the
		///		ObjectFieldValue table. Table MetaObject is related to table Field
		///		through the (M:N) relationship defined in the ObjectFieldValue table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="fieldID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaObject objects.</returns>
		public TList<MetaObject> GetByFieldIDFromObjectFieldValue(TransactionManager transactionManager, System.Int32 fieldID)
		{
			int count = -1;
			return GetByFieldIDFromObjectFieldValue(transactionManager, fieldID, 0, int.MaxValue, out count);
		}
		
		
		/// <summary>
		///		Gets MetaObject objects from the datasource by FieldID in the
		///		ObjectFieldValue table. Table MetaObject is related to table Field
		///		through the (M:N) relationship defined in the ObjectFieldValue table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="fieldID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaObject objects.</returns>
		public TList<MetaObject> GetByFieldIDFromObjectFieldValue(TransactionManager transactionManager, System.Int32 fieldID,int start, int pageLength)
		{
			int count = -1;
			return GetByFieldIDFromObjectFieldValue(transactionManager, fieldID, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets MetaObject objects from the datasource by FieldID in the
		///		ObjectFieldValue table. Table MetaObject is related to table Field
		///		through the (M:N) relationship defined in the ObjectFieldValue table.
		/// </summary>
		/// <param name="fieldID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaObject objects.</returns>
		public TList<MetaObject> GetByFieldIDFromObjectFieldValue(System.Int32 fieldID,int start, int pageLength, out int count)
		{
			
			return GetByFieldIDFromObjectFieldValue(null, fieldID, start, pageLength, out count);
		}


		/// <summary>
		///		Gets MetaObject objects from the datasource by FieldID in the
		///		ObjectFieldValue table. Table MetaObject is related to table Field
		///		through the (M:N) relationship defined in the ObjectFieldValue table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <param name="fieldID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of MetaObject objects.</returns>
		public abstract TList<MetaObject> GetByFieldIDFromObjectFieldValue(TransactionManager transactionManager,System.Int32 fieldID, int start, int pageLength, out int count);
		
		#endregion GetByFieldIDFromObjectFieldValue
		
		#region GetByGraphFileIDGraphFileMachineFromGraphFileObject
		
		/// <summary>
		///		Gets MetaObject objects from the datasource by GraphFileID in the
		///		GraphFileObject table. Table MetaObject is related to table GraphFile
		///		through the (M:N) relationship defined in the GraphFileObject table.
		/// </summary>
		/// <param name="graphFileID"></param>
		/// <param name="graphFileMachine"></param>
		/// <returns>Returns a typed collection of MetaObject objects.</returns>
		public TList<MetaObject> GetByGraphFileIDGraphFileMachineFromGraphFileObject(System.Int32 graphFileID, System.String graphFileMachine)
		{
			int count = -1;
			return GetByGraphFileIDGraphFileMachineFromGraphFileObject(null,graphFileID, graphFileMachine, 0, int.MaxValue, out count);
			
		}
		
		/// <summary>
		///		Gets MetaBuilder.BusinessLogic.MetaObject objects from the datasource by GraphFileID in the
		///		GraphFileObject table. Table MetaObject is related to table GraphFile
		///		through the (M:N) relationship defined in the GraphFileObject table.
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="graphFileID"></param>
		/// <param name="graphFileMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of MetaObject objects.</returns>
		public TList<MetaObject> GetByGraphFileIDGraphFileMachineFromGraphFileObject(System.Int32 graphFileID, System.String graphFileMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByGraphFileIDGraphFileMachineFromGraphFileObject(null, graphFileID, graphFileMachine, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets MetaObject objects from the datasource by GraphFileID in the
		///		GraphFileObject table. Table MetaObject is related to table GraphFile
		///		through the (M:N) relationship defined in the GraphFileObject table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="graphFileID"></param>
		/// <param name="graphFileMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaObject objects.</returns>
		public TList<MetaObject> GetByGraphFileIDGraphFileMachineFromGraphFileObject(TransactionManager transactionManager, System.Int32 graphFileID, System.String graphFileMachine)
		{
			int count = -1;
			return GetByGraphFileIDGraphFileMachineFromGraphFileObject(transactionManager, graphFileID, graphFileMachine, 0, int.MaxValue, out count);
		}
		
		
		/// <summary>
		///		Gets MetaObject objects from the datasource by GraphFileID in the
		///		GraphFileObject table. Table MetaObject is related to table GraphFile
		///		through the (M:N) relationship defined in the GraphFileObject table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="graphFileID"></param>
		/// <param name="graphFileMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaObject objects.</returns>
		public TList<MetaObject> GetByGraphFileIDGraphFileMachineFromGraphFileObject(TransactionManager transactionManager, System.Int32 graphFileID, System.String graphFileMachine,int start, int pageLength)
		{
			int count = -1;
			return GetByGraphFileIDGraphFileMachineFromGraphFileObject(transactionManager, graphFileID, graphFileMachine, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets MetaObject objects from the datasource by GraphFileID in the
		///		GraphFileObject table. Table MetaObject is related to table GraphFile
		///		through the (M:N) relationship defined in the GraphFileObject table.
		/// </summary>
		/// <param name="graphFileID"></param>
		/// <param name="graphFileMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaObject objects.</returns>
		public TList<MetaObject> GetByGraphFileIDGraphFileMachineFromGraphFileObject(System.Int32 graphFileID, System.String graphFileMachine,int start, int pageLength, out int count)
		{
			
			return GetByGraphFileIDGraphFileMachineFromGraphFileObject(null, graphFileID, graphFileMachine, start, pageLength, out count);
		}


		/// <summary>
		///		Gets MetaObject objects from the datasource by GraphFileID in the
		///		GraphFileObject table. Table MetaObject is related to table GraphFile
		///		through the (M:N) relationship defined in the GraphFileObject table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <param name="graphFileID"></param>
		/// <param name="graphFileMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of MetaObject objects.</returns>
		public abstract TList<MetaObject> GetByGraphFileIDGraphFileMachineFromGraphFileObject(TransactionManager transactionManager,System.Int32 graphFileID, System.String graphFileMachine, int start, int pageLength, out int count);
		
		#endregion GetByGraphFileIDGraphFileMachineFromGraphFileObject
		
		#endregion	
		
		#region Delete Methods

		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager">A <see cref="TransactionManager"/> object.</param>
		/// <param name="key">The unique identifier of the row to delete.</param>
		/// <returns>Returns true if operation suceeded.</returns>
		public override bool Delete(TransactionManager transactionManager, MetaBuilder.BusinessLogic.MetaObjectKey key)
		{
			return Delete(transactionManager, key.Pkid, key.Machine);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="pkid">. Primary Key.</param>
		/// <param name="machine">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(System.Int32 pkid, System.String machine)
		{
			return Delete(null, pkid, machine);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="pkid">. Primary Key.</param>
		/// <param name="machine">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(TransactionManager transactionManager, System.Int32 pkid, System.String machine);		
		
		#endregion Delete Methods
		
		#region Get By Foreign Key Functions
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Object_Class key.
		///		FK_Object_Class Description: 
		/// </summary>
		/// <param name="@class"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public MetaBuilder.BusinessLogic.TList<MetaObject> GetByClass(System.String @class)
		{
			int count = -1;
			return GetByClass(@class, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Object_Class key.
		///		FK_Object_Class Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="@class"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<MetaObject> GetByClass(TransactionManager transactionManager, System.String @class)
		{
			int count = -1;
			return GetByClass(transactionManager, @class, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_Object_Class key.
		///		FK_Object_Class Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="@class"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public MetaBuilder.BusinessLogic.TList<MetaObject> GetByClass(TransactionManager transactionManager, System.String @class, int start, int pageLength)
		{
			int count = -1;
			return GetByClass(transactionManager, @class, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Object_Class key.
		///		fKObjectClass Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="@class"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public MetaBuilder.BusinessLogic.TList<MetaObject> GetByClass(System.String @class, int start, int pageLength)
		{
			int count =  -1;
			return GetByClass(null, @class, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Object_Class key.
		///		fKObjectClass Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="@class"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public MetaBuilder.BusinessLogic.TList<MetaObject> GetByClass(System.String @class, int start, int pageLength,out int count)
		{
			return GetByClass(null, @class, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Object_Class key.
		///		FK_Object_Class Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="@class"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<MetaObject> GetByClass(TransactionManager transactionManager, System.String @class, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Object_User key.
		///		FK_Object_User Description: 
		/// </summary>
		/// <param name="userID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public MetaBuilder.BusinessLogic.TList<MetaObject> GetByUserID(System.Int32 userID)
		{
			int count = -1;
			return GetByUserID(userID, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Object_User key.
		///		FK_Object_User Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="userID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<MetaObject> GetByUserID(TransactionManager transactionManager, System.Int32 userID)
		{
			int count = -1;
			return GetByUserID(transactionManager, userID, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_Object_User key.
		///		FK_Object_User Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="userID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public MetaBuilder.BusinessLogic.TList<MetaObject> GetByUserID(TransactionManager transactionManager, System.Int32 userID, int start, int pageLength)
		{
			int count = -1;
			return GetByUserID(transactionManager, userID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Object_User key.
		///		fKObjectUser Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="userID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public MetaBuilder.BusinessLogic.TList<MetaObject> GetByUserID(System.Int32 userID, int start, int pageLength)
		{
			int count =  -1;
			return GetByUserID(null, userID, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Object_User key.
		///		fKObjectUser Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="userID"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public MetaBuilder.BusinessLogic.TList<MetaObject> GetByUserID(System.Int32 userID, int start, int pageLength,out int count)
		{
			return GetByUserID(null, userID, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Object_User key.
		///		FK_Object_User Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="userID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<MetaObject> GetByUserID(TransactionManager transactionManager, System.Int32 userID, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_MetaObject_VCStatus key.
		///		FK_MetaObject_VCStatus Description: 
		/// </summary>
		/// <param name="vCStatusID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public MetaBuilder.BusinessLogic.TList<MetaObject> GetByVCStatusID(System.Int32 vCStatusID)
		{
			int count = -1;
			return GetByVCStatusID(vCStatusID, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_MetaObject_VCStatus key.
		///		FK_MetaObject_VCStatus Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="vCStatusID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<MetaObject> GetByVCStatusID(TransactionManager transactionManager, System.Int32 vCStatusID)
		{
			int count = -1;
			return GetByVCStatusID(transactionManager, vCStatusID, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_MetaObject_VCStatus key.
		///		FK_MetaObject_VCStatus Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="vCStatusID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public MetaBuilder.BusinessLogic.TList<MetaObject> GetByVCStatusID(TransactionManager transactionManager, System.Int32 vCStatusID, int start, int pageLength)
		{
			int count = -1;
			return GetByVCStatusID(transactionManager, vCStatusID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_MetaObject_VCStatus key.
		///		fKMetaObjectVCStatus Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="vCStatusID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public MetaBuilder.BusinessLogic.TList<MetaObject> GetByVCStatusID(System.Int32 vCStatusID, int start, int pageLength)
		{
			int count =  -1;
			return GetByVCStatusID(null, vCStatusID, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_MetaObject_VCStatus key.
		///		fKMetaObjectVCStatus Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="vCStatusID"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public MetaBuilder.BusinessLogic.TList<MetaObject> GetByVCStatusID(System.Int32 vCStatusID, int start, int pageLength,out int count)
		{
			return GetByVCStatusID(null, vCStatusID, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_MetaObject_VCStatus key.
		///		FK_MetaObject_VCStatus Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="vCStatusID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<MetaObject> GetByVCStatusID(TransactionManager transactionManager, System.Int32 vCStatusID, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_MetaObject_Workspace key.
		///		FK_MetaObject_Workspace Description: 
		/// </summary>
		/// <param name="workspaceName"></param>
		/// <param name="workspaceTypeID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public MetaBuilder.BusinessLogic.TList<MetaObject> GetByWorkspaceNameWorkspaceTypeID(System.String workspaceName, System.Int32? workspaceTypeID)
		{
			int count = -1;
			return GetByWorkspaceNameWorkspaceTypeID(workspaceName, workspaceTypeID, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_MetaObject_Workspace key.
		///		FK_MetaObject_Workspace Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="workspaceName"></param>
		/// <param name="workspaceTypeID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<MetaObject> GetByWorkspaceNameWorkspaceTypeID(TransactionManager transactionManager, System.String workspaceName, System.Int32? workspaceTypeID)
		{
			int count = -1;
			return GetByWorkspaceNameWorkspaceTypeID(transactionManager, workspaceName, workspaceTypeID, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_MetaObject_Workspace key.
		///		FK_MetaObject_Workspace Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="workspaceName"></param>
		/// <param name="workspaceTypeID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public MetaBuilder.BusinessLogic.TList<MetaObject> GetByWorkspaceNameWorkspaceTypeID(TransactionManager transactionManager, System.String workspaceName, System.Int32? workspaceTypeID, int start, int pageLength)
		{
			int count = -1;
			return GetByWorkspaceNameWorkspaceTypeID(transactionManager, workspaceName, workspaceTypeID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_MetaObject_Workspace key.
		///		fKMetaObjectWorkspace Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="workspaceName"></param>
		/// <param name="workspaceTypeID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public MetaBuilder.BusinessLogic.TList<MetaObject> GetByWorkspaceNameWorkspaceTypeID(System.String workspaceName, System.Int32? workspaceTypeID, int start, int pageLength)
		{
			int count =  -1;
			return GetByWorkspaceNameWorkspaceTypeID(null, workspaceName, workspaceTypeID, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_MetaObject_Workspace key.
		///		fKMetaObjectWorkspace Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="workspaceName"></param>
		/// <param name="workspaceTypeID"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public MetaBuilder.BusinessLogic.TList<MetaObject> GetByWorkspaceNameWorkspaceTypeID(System.String workspaceName, System.Int32? workspaceTypeID, int start, int pageLength,out int count)
		{
			return GetByWorkspaceNameWorkspaceTypeID(null, workspaceName, workspaceTypeID, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_MetaObject_Workspace key.
		///		FK_MetaObject_Workspace Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="workspaceName"></param>
		/// <param name="workspaceTypeID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<MetaObject> GetByWorkspaceNameWorkspaceTypeID(TransactionManager transactionManager, System.String workspaceName, System.Int32? workspaceTypeID, int start, int pageLength, out int count);
		
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
		public override MetaBuilder.BusinessLogic.MetaObject Get(TransactionManager transactionManager, MetaBuilder.BusinessLogic.MetaObjectKey key, int start, int pageLength)
		{
			return GetByPkidMachine(transactionManager, key.Pkid, key.Machine, start, pageLength);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key PK_MetaObject index.
		/// </summary>
		/// <param name="pkid"></param>
		/// <param name="machine"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.MetaObject"/> class.</returns>
		public MetaBuilder.BusinessLogic.MetaObject GetByPkidMachine(System.Int32 pkid, System.String machine)
		{
			int count = -1;
			return GetByPkidMachine(null,pkid, machine, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_MetaObject index.
		/// </summary>
		/// <param name="pkid"></param>
		/// <param name="machine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.MetaObject"/> class.</returns>
		public MetaBuilder.BusinessLogic.MetaObject GetByPkidMachine(System.Int32 pkid, System.String machine, int start, int pageLength)
		{
			int count = -1;
			return GetByPkidMachine(null, pkid, machine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_MetaObject index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="pkid"></param>
		/// <param name="machine"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.MetaObject"/> class.</returns>
		public MetaBuilder.BusinessLogic.MetaObject GetByPkidMachine(TransactionManager transactionManager, System.Int32 pkid, System.String machine)
		{
			int count = -1;
			return GetByPkidMachine(transactionManager, pkid, machine, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_MetaObject index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="pkid"></param>
		/// <param name="machine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.MetaObject"/> class.</returns>
		public MetaBuilder.BusinessLogic.MetaObject GetByPkidMachine(TransactionManager transactionManager, System.Int32 pkid, System.String machine, int start, int pageLength)
		{
			int count = -1;
			return GetByPkidMachine(transactionManager, pkid, machine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_MetaObject index.
		/// </summary>
		/// <param name="pkid"></param>
		/// <param name="machine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.MetaObject"/> class.</returns>
		public MetaBuilder.BusinessLogic.MetaObject GetByPkidMachine(System.Int32 pkid, System.String machine, int start, int pageLength, out int count)
		{
			return GetByPkidMachine(null, pkid, machine, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_MetaObject index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="pkid"></param>
		/// <param name="machine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.MetaObject"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.MetaObject GetByPkidMachine(TransactionManager transactionManager, System.Int32 pkid, System.String machine, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a MetaBuilder.BusinessLogic.TList&lt;MetaObject&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="MetaBuilder.BusinessLogic.TList&lt;MetaObject&gt;"/></returns>
		public static MetaBuilder.BusinessLogic.TList<MetaObject> Fill(IDataReader reader, MetaBuilder.BusinessLogic.TList<MetaObject> rows, int start, int pageLength)
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
				
				MetaBuilder.BusinessLogic.MetaObject c = null;
				if (DataRepository.Provider.UseEntityFactory)
				{
					key = @"MetaObject" 
							+ (reader.IsDBNull(reader.GetOrdinal("pkid"))?(int)0:(System.Int32)reader["pkid"]).ToString()
							+ (reader.IsDBNull(reader.GetOrdinal("Machine"))?string.Empty:(System.String)reader["Machine"]).ToString();

					c = EntityManager.LocateOrCreate<MetaObject>(
						key.ToString(), // EntityTrackingKey 
						"MetaObject",  //Creational Type
						DataRepository.Provider.EntityCreationalFactoryType,  //Factory used to create entity
						DataRepository.Provider.EnableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.MetaObject();
				}
				
				if (!DataRepository.Provider.EnableEntityTracking || c.EntityState == EntityState.Added)
                {
					c.SuppressEntityEvents = true;
					c.Pkid = (System.Int32)reader["pkid"];
					c.Class = (System.String)reader["Class"];
					c.WorkspaceName = (System.String)reader["WorkspaceName"];
					c.UserID = (System.Int32)reader["UserID"];
					c.Machine = (System.String)reader["Machine"];
					c.OriginalMachine = c.Machine; //(reader.IsDBNull(reader.GetOrdinal("Machine")))?string.Empty:(System.String)reader["Machine"];
					c.VCStatusID = (System.Int32)reader["VCStatusID"];
					c.VCMachineID = (reader.IsDBNull(reader.GetOrdinal("VCMachineID")))?null:(System.String)reader["VCMachineID"];
					c.WorkspaceTypeID = (reader.IsDBNull(reader.GetOrdinal("WorkspaceTypeID")))?null:(System.Int32?)reader["WorkspaceTypeID"];
					c.DateCreated = (reader.IsDBNull(reader.GetOrdinal("DateCreated")))?null:(System.DateTime?)reader["DateCreated"];
					c.LastModified = (reader.IsDBNull(reader.GetOrdinal("LastModified")))?null:(System.DateTime?)reader["LastModified"];
					c.EntityTrackingKey = key;
					c.AcceptChanges();
					c.SuppressEntityEvents = false;
				}
				rows.Add(c);
			}
			return rows;
		}
		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.MetaObject"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.MetaObject"/> object to refresh.</param>
		public static void RefreshEntity(IDataReader reader, MetaBuilder.BusinessLogic.MetaObject entity)
		{
			if (!reader.Read()) return;
			
			entity.Pkid = (System.Int32)reader["pkid"];
			entity.Class = (System.String)reader["Class"];
			entity.WorkspaceName = (System.String)reader["WorkspaceName"];
			entity.UserID = (System.Int32)reader["UserID"];
			entity.Machine = (System.String)reader["Machine"];
			entity.OriginalMachine = (System.String)reader["Machine"];
			entity.VCStatusID = (System.Int32)reader["VCStatusID"];
			entity.VCMachineID = (reader.IsDBNull(reader.GetOrdinal("VCMachineID")))?null:(System.String)reader["VCMachineID"];
			entity.WorkspaceTypeID = (reader.IsDBNull(reader.GetOrdinal("WorkspaceTypeID")))?null:(System.Int32?)reader["WorkspaceTypeID"];
			entity.DateCreated = (reader.IsDBNull(reader.GetOrdinal("DateCreated")))?null:(System.DateTime?)reader["DateCreated"];
			entity.LastModified = (reader.IsDBNull(reader.GetOrdinal("LastModified")))?null:(System.DateTime?)reader["LastModified"];
			entity.AcceptChanges();
		}
		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.MetaObject"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.MetaObject"/> object.</param>
		public static void RefreshEntity(DataSet dataSet, MetaBuilder.BusinessLogic.MetaObject entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.Pkid = (System.Int32)dataRow["pkid"];
			entity.Class = (System.String)dataRow["Class"];
			entity.WorkspaceName = (System.String)dataRow["WorkspaceName"];
			entity.UserID = (System.Int32)dataRow["UserID"];
			entity.Machine = (System.String)dataRow["Machine"];
			entity.OriginalMachine = (System.String)dataRow["Machine"];
			entity.VCStatusID = (System.Int32)dataRow["VCStatusID"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?null:(System.String)dataRow["VCMachineID"];
			entity.WorkspaceTypeID = (Convert.IsDBNull(dataRow["WorkspaceTypeID"]))?null:(System.Int32?)dataRow["WorkspaceTypeID"];
			entity.DateCreated = (Convert.IsDBNull(dataRow["DateCreated"]))?null:(System.DateTime?)dataRow["DateCreated"];
			entity.LastModified = (Convert.IsDBNull(dataRow["LastModified"]))?null:(System.DateTime?)dataRow["LastModified"];
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
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.MetaObject"/> object to load.</param>
		/// <param name="deep">Boolean. A flag that indicates whether to recursively save all Property Collection that are descendants of this instance. If True, saves the complete object graph below this object. If False, saves this object only. </param>
		/// <param name="deepLoadType">DeepLoadType Enumeration to Include/Exclude object property collections from Load.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.MetaObject Property Collection Type Array To Include or Exclude from Load</param>
		/// <param name="innerList">A collection of child types for easy access.</param>
	    /// <exception cref="ArgumentNullException">entity or childTypes is null.</exception>
	    /// <exception cref="ArgumentException">deepLoadType has invalid value.</exception>
		internal override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.MetaObject entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, ChildEntityTypesList innerList)
		{
			if(entity == null)
				return;

			#region ClassSource	
			if (CanDeepLoad(entity, "Class", "ClassSource", deepLoadType, innerList) 
				&& entity.ClassSource == null)
			{
				object[] pkItems = new object[1];
				pkItems[0] = entity.Class;
				Class tmpEntity = EntityManager.LocateEntity<Class>(EntityLocator.ConstructKeyFromPkItems(typeof(Class), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.ClassSource = tmpEntity;
				else
					entity.ClassSource = DataRepository.ClassProvider.GetByName(entity.Class);
			
				if (deep && entity.ClassSource != null)
				{
					DataRepository.ClassProvider.DeepLoad(transactionManager, entity.ClassSource, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion ClassSource

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

			#region VCStatusIDSource	
			if (CanDeepLoad(entity, "VCStatus", "VCStatusIDSource", deepLoadType, innerList) 
				&& entity.VCStatusIDSource == null)
			{
				object[] pkItems = new object[1];
				pkItems[0] = entity.VCStatusID;
				VCStatus tmpEntity = EntityManager.LocateEntity<VCStatus>(EntityLocator.ConstructKeyFromPkItems(typeof(VCStatus), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.VCStatusIDSource = tmpEntity;
				else
					entity.VCStatusIDSource = DataRepository.VCStatusProvider.GetByPkid(entity.VCStatusID);
			
				if (deep && entity.VCStatusIDSource != null)
				{
					DataRepository.VCStatusProvider.DeepLoad(transactionManager, entity.VCStatusIDSource, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion VCStatusIDSource

			#region WorkspaceNameWorkspaceTypeIDSource	
			if (CanDeepLoad(entity, "Workspace", "WorkspaceNameWorkspaceTypeIDSource", deepLoadType, innerList) 
				&& entity.WorkspaceNameWorkspaceTypeIDSource == null)
			{
				object[] pkItems = new object[2];
				pkItems[0] = entity.WorkspaceName;
				pkItems[1] = (entity.WorkspaceTypeID ?? (int)0);
				Workspace tmpEntity = EntityManager.LocateEntity<Workspace>(EntityLocator.ConstructKeyFromPkItems(typeof(Workspace), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.WorkspaceNameWorkspaceTypeIDSource = tmpEntity;
				else
					entity.WorkspaceNameWorkspaceTypeIDSource = DataRepository.WorkspaceProvider.GetByNameWorkspaceTypeID(entity.WorkspaceName, (entity.WorkspaceTypeID ?? (int)0));
			
				if (deep && entity.WorkspaceNameWorkspaceTypeIDSource != null)
				{
					DataRepository.WorkspaceProvider.DeepLoad(transactionManager, entity.WorkspaceNameWorkspaceTypeIDSource, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion WorkspaceNameWorkspaceTypeIDSource
			
			// Load Entity through Provider
			// Deep load child collections  - Call GetBypkidMachine methods when available
			
			#region ArtifactCollectionByChildObjectIDChildObjectMachine
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<Artifact>", "ArtifactCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'ArtifactCollectionByChildObjectIDChildObjectMachine' loaded.");
				#endif 

				entity.ArtifactCollectionByChildObjectIDChildObjectMachine = DataRepository.ArtifactProvider.GetByChildObjectIDChildObjectMachine(transactionManager, entity.Pkid, entity.Machine);

				if (deep && entity.ArtifactCollectionByChildObjectIDChildObjectMachine.Count > 0)
				{
					DataRepository.ArtifactProvider.DeepLoad(transactionManager, entity.ArtifactCollectionByChildObjectIDChildObjectMachine, deep, deepLoadType, childTypes, innerList);
				}
			}		
			#endregion 
			
			#region ObjectAssociationCollectionByObjectIDObjectMachine
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<ObjectAssociation>", "ObjectAssociationCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'ObjectAssociationCollectionByObjectIDObjectMachine' loaded.");
				#endif 

				entity.ObjectAssociationCollectionByObjectIDObjectMachine = DataRepository.ObjectAssociationProvider.GetByObjectIDObjectMachine(transactionManager, entity.Pkid, entity.Machine);

				if (deep && entity.ObjectAssociationCollectionByObjectIDObjectMachine.Count > 0)
				{
					DataRepository.ObjectAssociationProvider.DeepLoad(transactionManager, entity.ObjectAssociationCollectionByObjectIDObjectMachine, deep, deepLoadType, childTypes, innerList);
				}
			}		
			#endregion 
			
			#region ArtifactCollectionByObjectIDObjectMachine
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<Artifact>", "ArtifactCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'ArtifactCollectionByObjectIDObjectMachine' loaded.");
				#endif 

				entity.ArtifactCollectionByObjectIDObjectMachine = DataRepository.ArtifactProvider.GetByObjectIDObjectMachine(transactionManager, entity.Pkid, entity.Machine);

				if (deep && entity.ArtifactCollectionByObjectIDObjectMachine.Count > 0)
				{
					DataRepository.ArtifactProvider.DeepLoad(transactionManager, entity.ArtifactCollectionByObjectIDObjectMachine, deep, deepLoadType, childTypes, innerList);
				}
			}		
			#endregion 
			
			#region ObjectFieldValueCollection
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<ObjectFieldValue>", "ObjectFieldValueCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'ObjectFieldValueCollection' loaded.");
				#endif 

				entity.ObjectFieldValueCollection = DataRepository.ObjectFieldValueProvider.GetByObjectIDMachineID(transactionManager, entity.Pkid, entity.Machine);

				if (deep && entity.ObjectFieldValueCollection.Count > 0)
				{
					DataRepository.ObjectFieldValueProvider.DeepLoad(transactionManager, entity.ObjectFieldValueCollection, deep, deepLoadType, childTypes, innerList);
				}
			}		
			#endregion 
			
			#region GraphFileObjectCollection
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<GraphFileObject>", "GraphFileObjectCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'GraphFileObjectCollection' loaded.");
				#endif 

				entity.GraphFileObjectCollection = DataRepository.GraphFileObjectProvider.GetByMetaObjectIDMachineID(transactionManager, entity.Pkid, entity.Machine);

				if (deep && entity.GraphFileObjectCollection.Count > 0)
				{
					DataRepository.GraphFileObjectProvider.DeepLoad(transactionManager, entity.GraphFileObjectCollection, deep, deepLoadType, childTypes, innerList);
				}
			}		
			#endregion 
			
			#region FieldCollection_From_ObjectFieldValue
			// RelationshipType.ManyToMany
			if (CanDeepLoad(entity, "List<Field>", "FieldCollection_From_ObjectFieldValue", deepLoadType, innerList))
			{
				entity.FieldCollection_From_ObjectFieldValue = DataRepository.FieldProvider.GetByObjectIDMachineIDFromObjectFieldValue(transactionManager, entity.Pkid, entity.Machine);			 
		
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'FieldCollection_From_ObjectFieldValue' loaded.");
				#endif 

				if (deep && entity.FieldCollection_From_ObjectFieldValue.Count > 0)
				{
					DataRepository.FieldProvider.DeepLoad(transactionManager, entity.FieldCollection_From_ObjectFieldValue, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion
			
			
			#region ObjectAssociationCollectionByChildObjectIDChildObjectMachine
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<ObjectAssociation>", "ObjectAssociationCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'ObjectAssociationCollectionByChildObjectIDChildObjectMachine' loaded.");
				#endif 

				entity.ObjectAssociationCollectionByChildObjectIDChildObjectMachine = DataRepository.ObjectAssociationProvider.GetByChildObjectIDChildObjectMachine(transactionManager, entity.Pkid, entity.Machine);

				if (deep && entity.ObjectAssociationCollectionByChildObjectIDChildObjectMachine.Count > 0)
				{
					DataRepository.ObjectAssociationProvider.DeepLoad(transactionManager, entity.ObjectAssociationCollectionByChildObjectIDChildObjectMachine, deep, deepLoadType, childTypes, innerList);
				}
			}		
			#endregion 
			
			#region GraphFileCollection_From_GraphFileObject
			// RelationshipType.ManyToMany
			if (CanDeepLoad(entity, "List<GraphFile>", "GraphFileCollection_From_GraphFileObject", deepLoadType, innerList))
			{
				entity.GraphFileCollection_From_GraphFileObject = DataRepository.GraphFileProvider.GetByMetaObjectIDMachineIDFromGraphFileObject(transactionManager, entity.Pkid, entity.Machine);			 
		
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'GraphFileCollection_From_GraphFileObject' loaded.");
				#endif 

				if (deep && entity.GraphFileCollection_From_GraphFileObject.Count > 0)
				{
					DataRepository.GraphFileProvider.DeepLoad(transactionManager, entity.GraphFileCollection_From_GraphFileObject, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion
			
			
			#region ArtifactCollectionByArtifactObjectIDArtefactMachine
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<Artifact>", "ArtifactCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'ArtifactCollectionByArtifactObjectIDArtefactMachine' loaded.");
				#endif 

				entity.ArtifactCollectionByArtifactObjectIDArtefactMachine = DataRepository.ArtifactProvider.GetByArtifactObjectIDArtefactMachine(transactionManager, entity.Pkid, entity.Machine);

				if (deep && entity.ArtifactCollectionByArtifactObjectIDArtefactMachine.Count > 0)
				{
					DataRepository.ArtifactProvider.DeepLoad(transactionManager, entity.ArtifactCollectionByArtifactObjectIDArtefactMachine, deep, deepLoadType, childTypes, innerList);
				}
			}		
			#endregion 
		}
		
		#endregion 
		
		#region DeepSave Methods

		/// <summary>
		/// Deep Save the entire object graph of the MetaBuilder.BusinessLogic.MetaObject object with criteria based of the child 
		/// Type property array and DeepSaveType.
		/// </summary>
		/// <param name="transactionManager">The transaction manager.</param>
		/// <param name="entity">MetaBuilder.BusinessLogic.MetaObject instance</param>
		/// <param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.MetaObject Property Collection Type Array To Include or Exclude from Save</param>
		/// <param name="innerList">A Hashtable of child types for easy access.</param>
		internal override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.MetaObject entity, DeepSaveType deepSaveType, System.Type[] childTypes, ChildEntityTypesList innerList)
		{	
			if (entity == null)
				return false;
							
			#region Composite Parent Properties
			//Save Source Composite Properties, however, don't call deep save on them.  
			//So they only get saved a single level deep.
			
			#region ClassSource
			if (CanDeepSave(entity, "Class", "ClassSource", deepSaveType, innerList) 
				&& entity.ClassSource != null)
			{
				DataRepository.ClassProvider.Save(transactionManager, entity.ClassSource);
				entity.Class = entity.ClassSource.Name;
			}
			#endregion 
			
			#region UserIDSource
			if (CanDeepSave(entity, "User", "UserIDSource", deepSaveType, innerList) 
				&& entity.UserIDSource != null)
			{
				DataRepository.UserProvider.Save(transactionManager, entity.UserIDSource);
				entity.UserID = entity.UserIDSource.Pkid;
			}
			#endregion 
			
			#region VCStatusIDSource
			if (CanDeepSave(entity, "VCStatus", "VCStatusIDSource", deepSaveType, innerList) 
				&& entity.VCStatusIDSource != null)
			{
				DataRepository.VCStatusProvider.Save(transactionManager, entity.VCStatusIDSource);
				entity.VCStatusID = entity.VCStatusIDSource.Pkid;
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
			
			















			#region FieldCollection_From_ObjectFieldValue>
			if (CanDeepSave(entity, "List<Field>", "FieldCollection_From_ObjectFieldValue", deepSaveType, innerList))
			{
				if (entity.FieldCollection_From_ObjectFieldValue.Count > 0 || entity.FieldCollection_From_ObjectFieldValue.DeletedItems.Count > 0)
					DataRepository.FieldProvider.DeepSave(transactionManager, entity.FieldCollection_From_ObjectFieldValue, deepSaveType, childTypes, innerList); 
			}
			#endregion 


			#region GraphFileCollection_From_GraphFileObject>
			if (CanDeepSave(entity, "List<GraphFile>", "GraphFileCollection_From_GraphFileObject", deepSaveType, innerList))
			{
				if (entity.GraphFileCollection_From_GraphFileObject.Count > 0 || entity.GraphFileCollection_From_GraphFileObject.DeletedItems.Count > 0)
					DataRepository.GraphFileProvider.DeepSave(transactionManager, entity.GraphFileCollection_From_GraphFileObject, deepSaveType, childTypes, innerList); 
			}
			#endregion 


			#region List<Artifact>
				if (CanDeepSave(entity, "List<Artifact>", "ArtifactCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(Artifact child in entity.ArtifactCollectionByChildObjectIDChildObjectMachine)
					{
						child.ChildObjectID = entity.Pkid;
						child.ChildObjectMachine = entity.Machine;
					}
				
				if (entity.ArtifactCollectionByChildObjectIDChildObjectMachine.Count > 0 || entity.ArtifactCollectionByChildObjectIDChildObjectMachine.DeletedItems.Count > 0)
					DataRepository.ArtifactProvider.DeepSave(transactionManager, entity.ArtifactCollectionByChildObjectIDChildObjectMachine, deepSaveType, childTypes, innerList);
				} 
			#endregion 
				

			#region List<ObjectAssociation>
				if (CanDeepSave(entity, "List<ObjectAssociation>", "ObjectAssociationCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(ObjectAssociation child in entity.ObjectAssociationCollectionByObjectIDObjectMachine)
					{
						child.ObjectID = entity.Pkid;
						child.ObjectMachine = entity.Machine;
					}
				
				if (entity.ObjectAssociationCollectionByObjectIDObjectMachine.Count > 0 || entity.ObjectAssociationCollectionByObjectIDObjectMachine.DeletedItems.Count > 0)
					DataRepository.ObjectAssociationProvider.DeepSave(transactionManager, entity.ObjectAssociationCollectionByObjectIDObjectMachine, deepSaveType, childTypes, innerList);
				} 
			#endregion 
				

			#region List<Artifact>
				if (CanDeepSave(entity, "List<Artifact>", "ArtifactCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(Artifact child in entity.ArtifactCollectionByObjectIDObjectMachine)
					{
						child.ObjectID = entity.Pkid;
						child.ObjectMachine = entity.Machine;
					}
				
				if (entity.ArtifactCollectionByObjectIDObjectMachine.Count > 0 || entity.ArtifactCollectionByObjectIDObjectMachine.DeletedItems.Count > 0)
					DataRepository.ArtifactProvider.DeepSave(transactionManager, entity.ArtifactCollectionByObjectIDObjectMachine, deepSaveType, childTypes, innerList);
				} 
			#endregion 
				

			#region List<ObjectFieldValue>
				if (CanDeepSave(entity, "List<ObjectFieldValue>", "ObjectFieldValueCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(ObjectFieldValue child in entity.ObjectFieldValueCollection)
					{
						child.ObjectID = entity.Pkid;
						child.MachineID = entity.Machine;
					}
				
				if (entity.ObjectFieldValueCollection.Count > 0 || entity.ObjectFieldValueCollection.DeletedItems.Count > 0)
					DataRepository.ObjectFieldValueProvider.DeepSave(transactionManager, entity.ObjectFieldValueCollection, deepSaveType, childTypes, innerList);
				} 
			#endregion 
				

			#region List<GraphFileObject>
				if (CanDeepSave(entity, "List<GraphFileObject>", "GraphFileObjectCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(GraphFileObject child in entity.GraphFileObjectCollection)
					{
						child.MetaObjectID = entity.Pkid;
						child.MachineID = entity.Machine;
					}
				
				if (entity.GraphFileObjectCollection.Count > 0 || entity.GraphFileObjectCollection.DeletedItems.Count > 0)
					DataRepository.GraphFileObjectProvider.DeepSave(transactionManager, entity.GraphFileObjectCollection, deepSaveType, childTypes, innerList);
				} 
			#endregion 
				


			#region List<ObjectAssociation>
				if (CanDeepSave(entity, "List<ObjectAssociation>", "ObjectAssociationCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(ObjectAssociation child in entity.ObjectAssociationCollectionByChildObjectIDChildObjectMachine)
					{
						child.ChildObjectID = entity.Pkid;
						child.ChildObjectMachine = entity.Machine;
					}
				
				if (entity.ObjectAssociationCollectionByChildObjectIDChildObjectMachine.Count > 0 || entity.ObjectAssociationCollectionByChildObjectIDChildObjectMachine.DeletedItems.Count > 0)
					DataRepository.ObjectAssociationProvider.DeepSave(transactionManager, entity.ObjectAssociationCollectionByChildObjectIDChildObjectMachine, deepSaveType, childTypes, innerList);
				} 
			#endregion 
				


			#region List<Artifact>
				if (CanDeepSave(entity, "List<Artifact>", "ArtifactCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(Artifact child in entity.ArtifactCollectionByArtifactObjectIDArtefactMachine)
					{
						child.ArtifactObjectID = entity.Pkid;
						child.ArtefactMachine = entity.Machine;
					}
				
				if (entity.ArtifactCollectionByArtifactObjectIDArtefactMachine.Count > 0 || entity.ArtifactCollectionByArtifactObjectIDArtefactMachine.DeletedItems.Count > 0)
					DataRepository.ArtifactProvider.DeepSave(transactionManager, entity.ArtifactCollectionByArtifactObjectIDArtefactMachine, deepSaveType, childTypes, innerList);
				} 
			#endregion 
				









						
			return true;
		}
		#endregion
	} // end class
	
	#region MetaObjectChildEntityTypes
	
	///<summary>
	/// Enumeration used to expose the different child entity types 
	/// for child properties in <c>MetaBuilder.BusinessLogic.MetaObject</c>
	///</summary>
	public enum MetaObjectChildEntityTypes
	{
		
		///<summary>
		/// Composite Property for <c>Class</c> at ClassSource
		///</summary>
		[ChildEntityType(typeof(Class))]
		Class,
			
		///<summary>
		/// Composite Property for <c>User</c> at UserIDSource
		///</summary>
		[ChildEntityType(typeof(User))]
		User,
			
		///<summary>
		/// Composite Property for <c>VCStatus</c> at VCStatusIDSource
		///</summary>
		[ChildEntityType(typeof(VCStatus))]
		VCStatus,
			
		///<summary>
		/// Composite Property for <c>Workspace</c> at WorkspaceNameWorkspaceTypeIDSource
		///</summary>
		[ChildEntityType(typeof(Workspace))]
		Workspace,
	
		///<summary>
		/// Collection of <c>MetaObject</c> as OneToMany for ArtifactCollection
		///</summary>
		[ChildEntityType(typeof(TList<Artifact>))]
		ArtifactCollectionByChildObjectIDChildObjectMachine,

		///<summary>
		/// Collection of <c>MetaObject</c> as OneToMany for ObjectAssociationCollection
		///</summary>
		[ChildEntityType(typeof(TList<ObjectAssociation>))]
		ObjectAssociationCollectionByObjectIDObjectMachine,

		///<summary>
		/// Collection of <c>MetaObject</c> as OneToMany for ArtifactCollection
		///</summary>
		[ChildEntityType(typeof(TList<Artifact>))]
		ArtifactCollectionByObjectIDObjectMachine,

		///<summary>
		/// Collection of <c>MetaObject</c> as OneToMany for ObjectFieldValueCollection
		///</summary>
		[ChildEntityType(typeof(TList<ObjectFieldValue>))]
		ObjectFieldValueCollection,

		///<summary>
		/// Collection of <c>MetaObject</c> as OneToMany for GraphFileObjectCollection
		///</summary>
		[ChildEntityType(typeof(TList<GraphFileObject>))]
		GraphFileObjectCollection,

		///<summary>
		/// Collection of <c>MetaObject</c> as ManyToMany for FieldCollection_From_ObjectFieldValue
		///</summary>
		[ChildEntityType(typeof(TList<Field>))]
		FieldCollection_From_ObjectFieldValue,

		///<summary>
		/// Collection of <c>MetaObject</c> as OneToMany for ObjectAssociationCollection
		///</summary>
		[ChildEntityType(typeof(TList<ObjectAssociation>))]
		ObjectAssociationCollectionByChildObjectIDChildObjectMachine,

		///<summary>
		/// Collection of <c>MetaObject</c> as ManyToMany for GraphFileCollection_From_GraphFileObject
		///</summary>
		[ChildEntityType(typeof(TList<GraphFile>))]
		GraphFileCollection_From_GraphFileObject,

		///<summary>
		/// Collection of <c>MetaObject</c> as OneToMany for ArtifactCollection
		///</summary>
		[ChildEntityType(typeof(TList<Artifact>))]
		ArtifactCollectionByArtifactObjectIDArtefactMachine,
	}
	
	#endregion MetaObjectChildEntityTypes
	
	#region MetaObjectFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="MetaObject"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class MetaObjectFilterBuilder : SqlFilterBuilder<MetaObjectColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the MetaObjectFilterBuilder class.
		/// </summary>
		public MetaObjectFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the MetaObjectFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public MetaObjectFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the MetaObjectFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public MetaObjectFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion MetaObjectFilterBuilder
	
	#region MetaObjectParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="MetaObject"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class MetaObjectParameterBuilder : ParameterizedSqlFilterBuilder<MetaObjectColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the MetaObjectParameterBuilder class.
		/// </summary>
		public MetaObjectParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the MetaObjectParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public MetaObjectParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the MetaObjectParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public MetaObjectParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion MetaObjectParameterBuilder
} // end namespace
