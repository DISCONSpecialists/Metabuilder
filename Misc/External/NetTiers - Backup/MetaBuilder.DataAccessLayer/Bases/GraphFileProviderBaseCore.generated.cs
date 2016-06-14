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
	/// This class is the base class for any <see cref="GraphFileProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract partial class GraphFileProviderBaseCore : EntityProviderBase<MetaBuilder.BusinessLogic.GraphFile, MetaBuilder.BusinessLogic.GraphFileKey>
	{		
		#region Get from Many To Many Relationship Functions
		#region GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation
		
		/// <summary>
		///		Gets GraphFile objects from the datasource by CAid in the
		///		GraphFileAssociation table. Table GraphFile is related to table ObjectAssociation
		///		through the (M:N) relationship defined in the GraphFileAssociation table.
		/// </summary>
		/// <param name="cAid"></param>
		/// <param name="objectID"></param>
		/// <param name="childObjectID"></param>
		/// <param name="objectMachine"></param>
		/// <param name="childObjectMachine"></param>
		/// <returns>Returns a typed collection of GraphFile objects.</returns>
		public TList<GraphFile> GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation(System.Int32 cAid, System.Int32 objectID, System.Int32 childObjectID, System.String objectMachine, System.String childObjectMachine)
		{
			int count = -1;
			return GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation(null,cAid, objectID, childObjectID, objectMachine, childObjectMachine, 0, int.MaxValue, out count);
			
		}
		
		/// <summary>
		///		Gets MetaBuilder.BusinessLogic.GraphFile objects from the datasource by CAid in the
		///		GraphFileAssociation table. Table GraphFile is related to table ObjectAssociation
		///		through the (M:N) relationship defined in the GraphFileAssociation table.
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="cAid"></param>
		/// <param name="objectID"></param>
		/// <param name="childObjectID"></param>
		/// <param name="objectMachine"></param>
		/// <param name="childObjectMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of GraphFile objects.</returns>
		public TList<GraphFile> GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation(System.Int32 cAid, System.Int32 objectID, System.Int32 childObjectID, System.String objectMachine, System.String childObjectMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation(null, cAid, objectID, childObjectID, objectMachine, childObjectMachine, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets GraphFile objects from the datasource by CAid in the
		///		GraphFileAssociation table. Table GraphFile is related to table ObjectAssociation
		///		through the (M:N) relationship defined in the GraphFileAssociation table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="cAid"></param>
		/// <param name="objectID"></param>
		/// <param name="childObjectID"></param>
		/// <param name="objectMachine"></param>
		/// <param name="childObjectMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of GraphFile objects.</returns>
		public TList<GraphFile> GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation(TransactionManager transactionManager, System.Int32 cAid, System.Int32 objectID, System.Int32 childObjectID, System.String objectMachine, System.String childObjectMachine)
		{
			int count = -1;
			return GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation(transactionManager, cAid, objectID, childObjectID, objectMachine, childObjectMachine, 0, int.MaxValue, out count);
		}
		
		
		/// <summary>
		///		Gets GraphFile objects from the datasource by CAid in the
		///		GraphFileAssociation table. Table GraphFile is related to table ObjectAssociation
		///		through the (M:N) relationship defined in the GraphFileAssociation table.
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
		/// <returns>Returns a typed collection of GraphFile objects.</returns>
		public TList<GraphFile> GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation(TransactionManager transactionManager, System.Int32 cAid, System.Int32 objectID, System.Int32 childObjectID, System.String objectMachine, System.String childObjectMachine,int start, int pageLength)
		{
			int count = -1;
			return GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation(transactionManager, cAid, objectID, childObjectID, objectMachine, childObjectMachine, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets GraphFile objects from the datasource by CAid in the
		///		GraphFileAssociation table. Table GraphFile is related to table ObjectAssociation
		///		through the (M:N) relationship defined in the GraphFileAssociation table.
		/// </summary>
		/// <param name="cAid"></param>
		/// <param name="objectID"></param>
		/// <param name="childObjectID"></param>
		/// <param name="objectMachine"></param>
		/// <param name="childObjectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of GraphFile objects.</returns>
		public TList<GraphFile> GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation(System.Int32 cAid, System.Int32 objectID, System.Int32 childObjectID, System.String objectMachine, System.String childObjectMachine,int start, int pageLength, out int count)
		{
			
			return GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation(null, cAid, objectID, childObjectID, objectMachine, childObjectMachine, start, pageLength, out count);
		}


		/// <summary>
		///		Gets GraphFile objects from the datasource by CAid in the
		///		GraphFileAssociation table. Table GraphFile is related to table ObjectAssociation
		///		through the (M:N) relationship defined in the GraphFileAssociation table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <param name="cAid"></param>
		/// <param name="objectID"></param>
		/// <param name="childObjectID"></param>
		/// <param name="objectMachine"></param>
		/// <param name="childObjectMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of GraphFile objects.</returns>
		public abstract TList<GraphFile> GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation(TransactionManager transactionManager,System.Int32 cAid, System.Int32 objectID, System.Int32 childObjectID, System.String objectMachine, System.String childObjectMachine, int start, int pageLength, out int count);
		
		#endregion GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation
		
		#region GetByMetaObjectIDMachineIDFromGraphFileObject
		
		/// <summary>
		///		Gets GraphFile objects from the datasource by MetaObjectID in the
		///		GraphFileObject table. Table GraphFile is related to table MetaObject
		///		through the (M:N) relationship defined in the GraphFileObject table.
		/// </summary>
		/// <param name="metaObjectID"></param>
		/// <param name="machineID"></param>
		/// <returns>Returns a typed collection of GraphFile objects.</returns>
		public TList<GraphFile> GetByMetaObjectIDMachineIDFromGraphFileObject(System.Int32 metaObjectID, System.String machineID)
		{
			int count = -1;
			return GetByMetaObjectIDMachineIDFromGraphFileObject(null,metaObjectID, machineID, 0, int.MaxValue, out count);
			
		}
		
		/// <summary>
		///		Gets MetaBuilder.BusinessLogic.GraphFile objects from the datasource by MetaObjectID in the
		///		GraphFileObject table. Table GraphFile is related to table MetaObject
		///		through the (M:N) relationship defined in the GraphFileObject table.
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="metaObjectID"></param>
		/// <param name="machineID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of GraphFile objects.</returns>
		public TList<GraphFile> GetByMetaObjectIDMachineIDFromGraphFileObject(System.Int32 metaObjectID, System.String machineID, int start, int pageLength)
		{
			int count = -1;
			return GetByMetaObjectIDMachineIDFromGraphFileObject(null, metaObjectID, machineID, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets GraphFile objects from the datasource by MetaObjectID in the
		///		GraphFileObject table. Table GraphFile is related to table MetaObject
		///		through the (M:N) relationship defined in the GraphFileObject table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="metaObjectID"></param>
		/// <param name="machineID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of GraphFile objects.</returns>
		public TList<GraphFile> GetByMetaObjectIDMachineIDFromGraphFileObject(TransactionManager transactionManager, System.Int32 metaObjectID, System.String machineID)
		{
			int count = -1;
			return GetByMetaObjectIDMachineIDFromGraphFileObject(transactionManager, metaObjectID, machineID, 0, int.MaxValue, out count);
		}
		
		
		/// <summary>
		///		Gets GraphFile objects from the datasource by MetaObjectID in the
		///		GraphFileObject table. Table GraphFile is related to table MetaObject
		///		through the (M:N) relationship defined in the GraphFileObject table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="metaObjectID"></param>
		/// <param name="machineID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of GraphFile objects.</returns>
		public TList<GraphFile> GetByMetaObjectIDMachineIDFromGraphFileObject(TransactionManager transactionManager, System.Int32 metaObjectID, System.String machineID,int start, int pageLength)
		{
			int count = -1;
			return GetByMetaObjectIDMachineIDFromGraphFileObject(transactionManager, metaObjectID, machineID, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets GraphFile objects from the datasource by MetaObjectID in the
		///		GraphFileObject table. Table GraphFile is related to table MetaObject
		///		through the (M:N) relationship defined in the GraphFileObject table.
		/// </summary>
		/// <param name="metaObjectID"></param>
		/// <param name="machineID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of GraphFile objects.</returns>
		public TList<GraphFile> GetByMetaObjectIDMachineIDFromGraphFileObject(System.Int32 metaObjectID, System.String machineID,int start, int pageLength, out int count)
		{
			
			return GetByMetaObjectIDMachineIDFromGraphFileObject(null, metaObjectID, machineID, start, pageLength, out count);
		}


		/// <summary>
		///		Gets GraphFile objects from the datasource by MetaObjectID in the
		///		GraphFileObject table. Table GraphFile is related to table MetaObject
		///		through the (M:N) relationship defined in the GraphFileObject table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <param name="metaObjectID"></param>
		/// <param name="machineID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of GraphFile objects.</returns>
		public abstract TList<GraphFile> GetByMetaObjectIDMachineIDFromGraphFileObject(TransactionManager transactionManager,System.Int32 metaObjectID, System.String machineID, int start, int pageLength, out int count);
		
		#endregion GetByMetaObjectIDMachineIDFromGraphFileObject
		
		#endregion	
		
		#region Delete Methods

		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager">A <see cref="TransactionManager"/> object.</param>
		/// <param name="key">The unique identifier of the row to delete.</param>
		/// <returns>Returns true if operation suceeded.</returns>
		public override bool Delete(TransactionManager transactionManager, MetaBuilder.BusinessLogic.GraphFileKey key)
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
		/// 	Gets rows from the datasource based on the FK_GraphFile_VCStatus key.
		///		FK_GraphFile_VCStatus Description: 
		/// </summary>
		/// <param name="vCStatusID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		public MetaBuilder.BusinessLogic.TList<GraphFile> GetByVCStatusID(System.Int32 vCStatusID)
		{
			int count = -1;
			return GetByVCStatusID(vCStatusID, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_VCStatus key.
		///		FK_GraphFile_VCStatus Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="vCStatusID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<GraphFile> GetByVCStatusID(TransactionManager transactionManager, System.Int32 vCStatusID)
		{
			int count = -1;
			return GetByVCStatusID(transactionManager, vCStatusID, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_VCStatus key.
		///		FK_GraphFile_VCStatus Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="vCStatusID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		public MetaBuilder.BusinessLogic.TList<GraphFile> GetByVCStatusID(TransactionManager transactionManager, System.Int32 vCStatusID, int start, int pageLength)
		{
			int count = -1;
			return GetByVCStatusID(transactionManager, vCStatusID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_VCStatus key.
		///		fKGraphFileVCStatus Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="vCStatusID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		public MetaBuilder.BusinessLogic.TList<GraphFile> GetByVCStatusID(System.Int32 vCStatusID, int start, int pageLength)
		{
			int count =  -1;
			return GetByVCStatusID(null, vCStatusID, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_VCStatus key.
		///		fKGraphFileVCStatus Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="vCStatusID"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		public MetaBuilder.BusinessLogic.TList<GraphFile> GetByVCStatusID(System.Int32 vCStatusID, int start, int pageLength,out int count)
		{
			return GetByVCStatusID(null, vCStatusID, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_VCStatus key.
		///		FK_GraphFile_VCStatus Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="vCStatusID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<GraphFile> GetByVCStatusID(TransactionManager transactionManager, System.Int32 vCStatusID, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_FileType key.
		///		FK_GraphFile_FileType Description: 
		/// </summary>
		/// <param name="fileTypeID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		public MetaBuilder.BusinessLogic.TList<GraphFile> GetByFileTypeID(System.Int32 fileTypeID)
		{
			int count = -1;
			return GetByFileTypeID(fileTypeID, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_FileType key.
		///		FK_GraphFile_FileType Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="fileTypeID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<GraphFile> GetByFileTypeID(TransactionManager transactionManager, System.Int32 fileTypeID)
		{
			int count = -1;
			return GetByFileTypeID(transactionManager, fileTypeID, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_FileType key.
		///		FK_GraphFile_FileType Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="fileTypeID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		public MetaBuilder.BusinessLogic.TList<GraphFile> GetByFileTypeID(TransactionManager transactionManager, System.Int32 fileTypeID, int start, int pageLength)
		{
			int count = -1;
			return GetByFileTypeID(transactionManager, fileTypeID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_FileType key.
		///		fKGraphFileFileType Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="fileTypeID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		public MetaBuilder.BusinessLogic.TList<GraphFile> GetByFileTypeID(System.Int32 fileTypeID, int start, int pageLength)
		{
			int count =  -1;
			return GetByFileTypeID(null, fileTypeID, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_FileType key.
		///		fKGraphFileFileType Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="fileTypeID"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		public MetaBuilder.BusinessLogic.TList<GraphFile> GetByFileTypeID(System.Int32 fileTypeID, int start, int pageLength,out int count)
		{
			return GetByFileTypeID(null, fileTypeID, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_FileType key.
		///		FK_GraphFile_FileType Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="fileTypeID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<GraphFile> GetByFileTypeID(TransactionManager transactionManager, System.Int32 fileTypeID, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_Workspace key.
		///		FK_GraphFile_Workspace Description: 
		/// </summary>
		/// <param name="workspaceName"></param>
		/// <param name="workspaceTypeID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		public MetaBuilder.BusinessLogic.TList<GraphFile> GetByWorkspaceNameWorkspaceTypeID(System.String workspaceName, System.Int32 workspaceTypeID)
		{
			int count = -1;
			return GetByWorkspaceNameWorkspaceTypeID(workspaceName, workspaceTypeID, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_Workspace key.
		///		FK_GraphFile_Workspace Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="workspaceName"></param>
		/// <param name="workspaceTypeID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<GraphFile> GetByWorkspaceNameWorkspaceTypeID(TransactionManager transactionManager, System.String workspaceName, System.Int32 workspaceTypeID)
		{
			int count = -1;
			return GetByWorkspaceNameWorkspaceTypeID(transactionManager, workspaceName, workspaceTypeID, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_Workspace key.
		///		FK_GraphFile_Workspace Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="workspaceName"></param>
		/// <param name="workspaceTypeID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		public MetaBuilder.BusinessLogic.TList<GraphFile> GetByWorkspaceNameWorkspaceTypeID(TransactionManager transactionManager, System.String workspaceName, System.Int32 workspaceTypeID, int start, int pageLength)
		{
			int count = -1;
			return GetByWorkspaceNameWorkspaceTypeID(transactionManager, workspaceName, workspaceTypeID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_Workspace key.
		///		fKGraphFileWorkspace Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="workspaceName"></param>
		/// <param name="workspaceTypeID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		public MetaBuilder.BusinessLogic.TList<GraphFile> GetByWorkspaceNameWorkspaceTypeID(System.String workspaceName, System.Int32 workspaceTypeID, int start, int pageLength)
		{
			int count =  -1;
			return GetByWorkspaceNameWorkspaceTypeID(null, workspaceName, workspaceTypeID, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_Workspace key.
		///		fKGraphFileWorkspace Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="workspaceName"></param>
		/// <param name="workspaceTypeID"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		public MetaBuilder.BusinessLogic.TList<GraphFile> GetByWorkspaceNameWorkspaceTypeID(System.String workspaceName, System.Int32 workspaceTypeID, int start, int pageLength,out int count)
		{
			return GetByWorkspaceNameWorkspaceTypeID(null, workspaceName, workspaceTypeID, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_Workspace key.
		///		FK_GraphFile_Workspace Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="workspaceName"></param>
		/// <param name="workspaceTypeID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<GraphFile> GetByWorkspaceNameWorkspaceTypeID(TransactionManager transactionManager, System.String workspaceName, System.Int32 workspaceTypeID, int start, int pageLength, out int count);
		
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
		public override MetaBuilder.BusinessLogic.GraphFile Get(TransactionManager transactionManager, MetaBuilder.BusinessLogic.GraphFileKey key, int start, int pageLength)
		{
			return GetByPkidMachine(transactionManager, key.Pkid, key.Machine, start, pageLength);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key PK_Version index.
		/// </summary>
		/// <param name="pkid"></param>
		/// <param name="machine"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.GraphFile"/> class.</returns>
		public MetaBuilder.BusinessLogic.GraphFile GetByPkidMachine(System.Int32 pkid, System.String machine)
		{
			int count = -1;
			return GetByPkidMachine(null,pkid, machine, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Version index.
		/// </summary>
		/// <param name="pkid"></param>
		/// <param name="machine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.GraphFile"/> class.</returns>
		public MetaBuilder.BusinessLogic.GraphFile GetByPkidMachine(System.Int32 pkid, System.String machine, int start, int pageLength)
		{
			int count = -1;
			return GetByPkidMachine(null, pkid, machine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Version index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="pkid"></param>
		/// <param name="machine"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.GraphFile"/> class.</returns>
		public MetaBuilder.BusinessLogic.GraphFile GetByPkidMachine(TransactionManager transactionManager, System.Int32 pkid, System.String machine)
		{
			int count = -1;
			return GetByPkidMachine(transactionManager, pkid, machine, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Version index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="pkid"></param>
		/// <param name="machine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.GraphFile"/> class.</returns>
		public MetaBuilder.BusinessLogic.GraphFile GetByPkidMachine(TransactionManager transactionManager, System.Int32 pkid, System.String machine, int start, int pageLength)
		{
			int count = -1;
			return GetByPkidMachine(transactionManager, pkid, machine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Version index.
		/// </summary>
		/// <param name="pkid"></param>
		/// <param name="machine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.GraphFile"/> class.</returns>
		public MetaBuilder.BusinessLogic.GraphFile GetByPkidMachine(System.Int32 pkid, System.String machine, int start, int pageLength, out int count)
		{
			return GetByPkidMachine(null, pkid, machine, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Version index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="pkid"></param>
		/// <param name="machine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.GraphFile"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.GraphFile GetByPkidMachine(TransactionManager transactionManager, System.Int32 pkid, System.String machine, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a MetaBuilder.BusinessLogic.TList&lt;GraphFile&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="MetaBuilder.BusinessLogic.TList&lt;GraphFile&gt;"/></returns>
		public static MetaBuilder.BusinessLogic.TList<GraphFile> Fill(IDataReader reader, MetaBuilder.BusinessLogic.TList<GraphFile> rows, int start, int pageLength)
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
				
				MetaBuilder.BusinessLogic.GraphFile c = null;
				if (DataRepository.Provider.UseEntityFactory)
				{
					key = @"GraphFile" 
							+ (reader.IsDBNull(reader.GetOrdinal("pkid"))?(int)0:(System.Int32)reader["pkid"]).ToString()
							+ (reader.IsDBNull(reader.GetOrdinal("Machine"))?string.Empty:(System.String)reader["Machine"]).ToString();

					c = EntityManager.LocateOrCreate<GraphFile>(
						key.ToString(), // EntityTrackingKey 
						"GraphFile",  //Creational Type
						DataRepository.Provider.EntityCreationalFactoryType,  //Factory used to create entity
						DataRepository.Provider.EnableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.GraphFile();
				}
				
				if (!DataRepository.Provider.EnableEntityTracking || c.EntityState == EntityState.Added)
                {
					c.SuppressEntityEvents = true;
					c.Pkid = (System.Int32)reader["pkid"];
					c.MajorVersion = (System.Int32)reader["MajorVersion"];
					c.MinorVersion = (System.Int32)reader["MinorVersion"];
					c.ModifiedDate = (System.DateTime)reader["ModifiedDate"];
					c.Notes = (reader.IsDBNull(reader.GetOrdinal("Notes")))?null:(System.String)reader["Notes"];
					c.IsActive = (System.Boolean)reader["IsActive"];
					c.Archived = (System.Boolean)reader["Archived"];
					c.AppVersion = (System.String)reader["AppVersion"];
					c.Blob = (System.Byte[])reader["Blob"];
					c.WorkspaceName = (System.String)reader["WorkspaceName"];
					c.FileTypeID = (System.Int32)reader["FileTypeID"];
					c.PreviousVersionID = (reader.IsDBNull(reader.GetOrdinal("PreviousVersionID")))?null:(System.Int32?)reader["PreviousVersionID"];
					c.Name = (System.String)reader["Name"];
					c.VCStatusID = (System.Int32)reader["VCStatusID"];
					c.VCMachineID = (reader.IsDBNull(reader.GetOrdinal("VCMachineID")))?null:(System.String)reader["VCMachineID"];
					c.Machine = (System.String)reader["Machine"];
					c.OriginalMachine = c.Machine; //(reader.IsDBNull(reader.GetOrdinal("Machine")))?string.Empty:(System.String)reader["Machine"];
					c.WorkspaceTypeID = (System.Int32)reader["WorkspaceTypeID"];
					c.OriginalFileUniqueID = (System.Guid)reader["OriginalFileUniqueID"];
					c.EntityTrackingKey = key;
					c.AcceptChanges();
					c.SuppressEntityEvents = false;
				}
				rows.Add(c);
			}
			return rows;
		}
		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.GraphFile"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.GraphFile"/> object to refresh.</param>
		public static void RefreshEntity(IDataReader reader, MetaBuilder.BusinessLogic.GraphFile entity)
		{
			if (!reader.Read()) return;
			
			entity.Pkid = (System.Int32)reader["pkid"];
			entity.MajorVersion = (System.Int32)reader["MajorVersion"];
			entity.MinorVersion = (System.Int32)reader["MinorVersion"];
			entity.ModifiedDate = (System.DateTime)reader["ModifiedDate"];
			entity.Notes = (reader.IsDBNull(reader.GetOrdinal("Notes")))?null:(System.String)reader["Notes"];
			entity.IsActive = (System.Boolean)reader["IsActive"];
			entity.Archived = (System.Boolean)reader["Archived"];
			entity.AppVersion = (System.String)reader["AppVersion"];
			entity.Blob = (System.Byte[])reader["Blob"];
			entity.WorkspaceName = (System.String)reader["WorkspaceName"];
			entity.FileTypeID = (System.Int32)reader["FileTypeID"];
			entity.PreviousVersionID = (reader.IsDBNull(reader.GetOrdinal("PreviousVersionID")))?null:(System.Int32?)reader["PreviousVersionID"];
			entity.Name = (System.String)reader["Name"];
			entity.VCStatusID = (System.Int32)reader["VCStatusID"];
			entity.VCMachineID = (reader.IsDBNull(reader.GetOrdinal("VCMachineID")))?null:(System.String)reader["VCMachineID"];
			entity.Machine = (System.String)reader["Machine"];
			entity.OriginalMachine = (System.String)reader["Machine"];
			entity.WorkspaceTypeID = (System.Int32)reader["WorkspaceTypeID"];
			entity.OriginalFileUniqueID = (System.Guid)reader["OriginalFileUniqueID"];
			entity.AcceptChanges();
		}
		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.GraphFile"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.GraphFile"/> object.</param>
		public static void RefreshEntity(DataSet dataSet, MetaBuilder.BusinessLogic.GraphFile entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.Pkid = (System.Int32)dataRow["pkid"];
			entity.MajorVersion = (System.Int32)dataRow["MajorVersion"];
			entity.MinorVersion = (System.Int32)dataRow["MinorVersion"];
			entity.ModifiedDate = (System.DateTime)dataRow["ModifiedDate"];
			entity.Notes = (Convert.IsDBNull(dataRow["Notes"]))?null:(System.String)dataRow["Notes"];
			entity.IsActive = (System.Boolean)dataRow["IsActive"];
			entity.Archived = (System.Boolean)dataRow["Archived"];
			entity.AppVersion = (System.String)dataRow["AppVersion"];
			entity.Blob = (System.Byte[])dataRow["Blob"];
			entity.WorkspaceName = (System.String)dataRow["WorkspaceName"];
			entity.FileTypeID = (System.Int32)dataRow["FileTypeID"];
			entity.PreviousVersionID = (Convert.IsDBNull(dataRow["PreviousVersionID"]))?null:(System.Int32?)dataRow["PreviousVersionID"];
			entity.Name = (System.String)dataRow["Name"];
			entity.VCStatusID = (System.Int32)dataRow["VCStatusID"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?null:(System.String)dataRow["VCMachineID"];
			entity.Machine = (System.String)dataRow["Machine"];
			entity.OriginalMachine = (System.String)dataRow["Machine"];
			entity.WorkspaceTypeID = (System.Int32)dataRow["WorkspaceTypeID"];
			entity.OriginalFileUniqueID = (System.Guid)dataRow["OriginalFileUniqueID"];
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
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.GraphFile"/> object to load.</param>
		/// <param name="deep">Boolean. A flag that indicates whether to recursively save all Property Collection that are descendants of this instance. If True, saves the complete object graph below this object. If False, saves this object only. </param>
		/// <param name="deepLoadType">DeepLoadType Enumeration to Include/Exclude object property collections from Load.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.GraphFile Property Collection Type Array To Include or Exclude from Load</param>
		/// <param name="innerList">A collection of child types for easy access.</param>
	    /// <exception cref="ArgumentNullException">entity or childTypes is null.</exception>
	    /// <exception cref="ArgumentException">deepLoadType has invalid value.</exception>
		internal override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.GraphFile entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, ChildEntityTypesList innerList)
		{
			if(entity == null)
				return;

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

			#region FileTypeIDSource	
			if (CanDeepLoad(entity, "FileType", "FileTypeIDSource", deepLoadType, innerList) 
				&& entity.FileTypeIDSource == null)
			{
				object[] pkItems = new object[1];
				pkItems[0] = entity.FileTypeID;
				FileType tmpEntity = EntityManager.LocateEntity<FileType>(EntityLocator.ConstructKeyFromPkItems(typeof(FileType), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.FileTypeIDSource = tmpEntity;
				else
					entity.FileTypeIDSource = DataRepository.FileTypeProvider.GetByPkid(entity.FileTypeID);
			
				if (deep && entity.FileTypeIDSource != null)
				{
					DataRepository.FileTypeProvider.DeepLoad(transactionManager, entity.FileTypeIDSource, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion FileTypeIDSource

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
			// Deep load child collections  - Call GetBypkidMachine methods when available
			
			#region GraphFileObjectCollection
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<GraphFileObject>", "GraphFileObjectCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'GraphFileObjectCollection' loaded.");
				#endif 

				entity.GraphFileObjectCollection = DataRepository.GraphFileObjectProvider.GetByGraphFileIDGraphFileMachine(transactionManager, entity.Pkid, entity.Machine);

				if (deep && entity.GraphFileObjectCollection.Count > 0)
				{
					DataRepository.GraphFileObjectProvider.DeepLoad(transactionManager, entity.GraphFileObjectCollection, deep, deepLoadType, childTypes, innerList);
				}
			}		
			#endregion 
			
			#region ObjectAssociationCollection_From_GraphFileAssociation
			// RelationshipType.ManyToMany
			if (CanDeepLoad(entity, "List<ObjectAssociation>", "ObjectAssociationCollection_From_GraphFileAssociation", deepLoadType, innerList))
			{
				entity.ObjectAssociationCollection_From_GraphFileAssociation = DataRepository.ObjectAssociationProvider.GetByGraphFileIDGraphFileMachineFromGraphFileAssociation(transactionManager, entity.Pkid, entity.Machine);			 
		
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'ObjectAssociationCollection_From_GraphFileAssociation' loaded.");
				#endif 

				if (deep && entity.ObjectAssociationCollection_From_GraphFileAssociation.Count > 0)
				{
					DataRepository.ObjectAssociationProvider.DeepLoad(transactionManager, entity.ObjectAssociationCollection_From_GraphFileAssociation, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion
			
			
			#region GraphFileAssociationCollection
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<GraphFileAssociation>", "GraphFileAssociationCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'GraphFileAssociationCollection' loaded.");
				#endif 

				entity.GraphFileAssociationCollection = DataRepository.GraphFileAssociationProvider.GetByGraphFileIDGraphFileMachine(transactionManager, entity.Pkid, entity.Machine);

				if (deep && entity.GraphFileAssociationCollection.Count > 0)
				{
					DataRepository.GraphFileAssociationProvider.DeepLoad(transactionManager, entity.GraphFileAssociationCollection, deep, deepLoadType, childTypes, innerList);
				}
			}		
			#endregion 
			
			#region MetaObjectCollection_From_GraphFileObject
			// RelationshipType.ManyToMany
			if (CanDeepLoad(entity, "List<MetaObject>", "MetaObjectCollection_From_GraphFileObject", deepLoadType, innerList))
			{
				entity.MetaObjectCollection_From_GraphFileObject = DataRepository.MetaObjectProvider.GetByGraphFileIDGraphFileMachineFromGraphFileObject(transactionManager, entity.Pkid, entity.Machine);			 
		
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'MetaObjectCollection_From_GraphFileObject' loaded.");
				#endif 

				if (deep && entity.MetaObjectCollection_From_GraphFileObject.Count > 0)
				{
					DataRepository.MetaObjectProvider.DeepLoad(transactionManager, entity.MetaObjectCollection_From_GraphFileObject, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion
			
		}
		
		#endregion 
		
		#region DeepSave Methods

		/// <summary>
		/// Deep Save the entire object graph of the MetaBuilder.BusinessLogic.GraphFile object with criteria based of the child 
		/// Type property array and DeepSaveType.
		/// </summary>
		/// <param name="transactionManager">The transaction manager.</param>
		/// <param name="entity">MetaBuilder.BusinessLogic.GraphFile instance</param>
		/// <param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.GraphFile Property Collection Type Array To Include or Exclude from Save</param>
		/// <param name="innerList">A Hashtable of child types for easy access.</param>
		internal override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.GraphFile entity, DeepSaveType deepSaveType, System.Type[] childTypes, ChildEntityTypesList innerList)
		{	
			if (entity == null)
				return false;
							
			#region Composite Parent Properties
			//Save Source Composite Properties, however, don't call deep save on them.  
			//So they only get saved a single level deep.
			
			#region VCStatusIDSource
			if (CanDeepSave(entity, "VCStatus", "VCStatusIDSource", deepSaveType, innerList) 
				&& entity.VCStatusIDSource != null)
			{
				DataRepository.VCStatusProvider.Save(transactionManager, entity.VCStatusIDSource);
				entity.VCStatusID = entity.VCStatusIDSource.Pkid;
			}
			#endregion 
			
			#region FileTypeIDSource
			if (CanDeepSave(entity, "FileType", "FileTypeIDSource", deepSaveType, innerList) 
				&& entity.FileTypeIDSource != null)
			{
				DataRepository.FileTypeProvider.Save(transactionManager, entity.FileTypeIDSource);
				entity.FileTypeID = entity.FileTypeIDSource.Pkid;
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
			
			






			#region ObjectAssociationCollection_From_GraphFileAssociation>
			if (CanDeepSave(entity, "List<ObjectAssociation>", "ObjectAssociationCollection_From_GraphFileAssociation", deepSaveType, innerList))
			{
				if (entity.ObjectAssociationCollection_From_GraphFileAssociation.Count > 0 || entity.ObjectAssociationCollection_From_GraphFileAssociation.DeletedItems.Count > 0)
					DataRepository.ObjectAssociationProvider.DeepSave(transactionManager, entity.ObjectAssociationCollection_From_GraphFileAssociation, deepSaveType, childTypes, innerList); 
			}
			#endregion 


			#region MetaObjectCollection_From_GraphFileObject>
			if (CanDeepSave(entity, "List<MetaObject>", "MetaObjectCollection_From_GraphFileObject", deepSaveType, innerList))
			{
				if (entity.MetaObjectCollection_From_GraphFileObject.Count > 0 || entity.MetaObjectCollection_From_GraphFileObject.DeletedItems.Count > 0)
					DataRepository.MetaObjectProvider.DeepSave(transactionManager, entity.MetaObjectCollection_From_GraphFileObject, deepSaveType, childTypes, innerList); 
			}
			#endregion 

			#region List<GraphFileObject>
				if (CanDeepSave(entity, "List<GraphFileObject>", "GraphFileObjectCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(GraphFileObject child in entity.GraphFileObjectCollection)
					{
						child.GraphFileID = entity.Pkid;
						child.GraphFileMachine = entity.Machine;
					}
				
				if (entity.GraphFileObjectCollection.Count > 0 || entity.GraphFileObjectCollection.DeletedItems.Count > 0)
					DataRepository.GraphFileObjectProvider.DeepSave(transactionManager, entity.GraphFileObjectCollection, deepSaveType, childTypes, innerList);
				} 
			#endregion 
				


			#region List<GraphFileAssociation>
				if (CanDeepSave(entity, "List<GraphFileAssociation>", "GraphFileAssociationCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(GraphFileAssociation child in entity.GraphFileAssociationCollection)
					{
						child.GraphFileID = entity.Pkid;
						child.GraphFileMachine = entity.Machine;
					}
				
				if (entity.GraphFileAssociationCollection.Count > 0 || entity.GraphFileAssociationCollection.DeletedItems.Count > 0)
					DataRepository.GraphFileAssociationProvider.DeepSave(transactionManager, entity.GraphFileAssociationCollection, deepSaveType, childTypes, innerList);
				} 
			#endregion 
				





						
			return true;
		}
		#endregion
	} // end class
	
	#region GraphFileChildEntityTypes
	
	///<summary>
	/// Enumeration used to expose the different child entity types 
	/// for child properties in <c>MetaBuilder.BusinessLogic.GraphFile</c>
	///</summary>
	public enum GraphFileChildEntityTypes
	{
		
		///<summary>
		/// Composite Property for <c>VCStatus</c> at VCStatusIDSource
		///</summary>
		[ChildEntityType(typeof(VCStatus))]
		VCStatus,
			
		///<summary>
		/// Composite Property for <c>FileType</c> at FileTypeIDSource
		///</summary>
		[ChildEntityType(typeof(FileType))]
		FileType,
			
		///<summary>
		/// Composite Property for <c>Workspace</c> at WorkspaceNameWorkspaceTypeIDSource
		///</summary>
		[ChildEntityType(typeof(Workspace))]
		Workspace,
	
		///<summary>
		/// Collection of <c>GraphFile</c> as OneToMany for GraphFileObjectCollection
		///</summary>
		[ChildEntityType(typeof(TList<GraphFileObject>))]
		GraphFileObjectCollection,

		///<summary>
		/// Collection of <c>GraphFile</c> as ManyToMany for ObjectAssociationCollection_From_GraphFileAssociation
		///</summary>
		[ChildEntityType(typeof(TList<ObjectAssociation>))]
		ObjectAssociationCollection_From_GraphFileAssociation,

		///<summary>
		/// Collection of <c>GraphFile</c> as OneToMany for GraphFileAssociationCollection
		///</summary>
		[ChildEntityType(typeof(TList<GraphFileAssociation>))]
		GraphFileAssociationCollection,

		///<summary>
		/// Collection of <c>GraphFile</c> as ManyToMany for MetaObjectCollection_From_GraphFileObject
		///</summary>
		[ChildEntityType(typeof(TList<MetaObject>))]
		MetaObjectCollection_From_GraphFileObject,
	}
	
	#endregion GraphFileChildEntityTypes
	
	#region GraphFileFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="GraphFile"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class GraphFileFilterBuilder : SqlFilterBuilder<GraphFileColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the GraphFileFilterBuilder class.
		/// </summary>
		public GraphFileFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the GraphFileFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public GraphFileFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the GraphFileFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public GraphFileFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion GraphFileFilterBuilder
	
	#region GraphFileParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="GraphFile"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class GraphFileParameterBuilder : ParameterizedSqlFilterBuilder<GraphFileColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the GraphFileParameterBuilder class.
		/// </summary>
		public GraphFileParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the GraphFileParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public GraphFileParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the GraphFileParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public GraphFileParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion GraphFileParameterBuilder
} // end namespace
