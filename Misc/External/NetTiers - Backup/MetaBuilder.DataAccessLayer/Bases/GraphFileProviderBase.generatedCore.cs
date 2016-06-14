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
		/// <param name="_cAid"></param>
		/// <param name="_objectID"></param>
		/// <param name="_childObjectID"></param>
		/// <param name="_objectMachine"></param>
		/// <param name="_childObjectMachine"></param>
		/// <returns>Returns a typed collection of GraphFile objects.</returns>
		public TList<GraphFile> GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation(System.Int32 _cAid, System.Int32 _objectID, System.Int32 _childObjectID, System.String _objectMachine, System.String _childObjectMachine)
		{
			int count = -1;
			return GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation(null,_cAid, _objectID, _childObjectID, _objectMachine, _childObjectMachine, 0, int.MaxValue, out count);
			
		}
		
		/// <summary>
		///		Gets MetaBuilder.BusinessLogic.GraphFile objects from the datasource by CAid in the
		///		GraphFileAssociation table. Table GraphFile is related to table ObjectAssociation
		///		through the (M:N) relationship defined in the GraphFileAssociation table.
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_cAid"></param>
		/// <param name="_objectID"></param>
		/// <param name="_childObjectID"></param>
		/// <param name="_objectMachine"></param>
		/// <param name="_childObjectMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of GraphFile objects.</returns>
		public TList<GraphFile> GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation(System.Int32 _cAid, System.Int32 _objectID, System.Int32 _childObjectID, System.String _objectMachine, System.String _childObjectMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation(null, _cAid, _objectID, _childObjectID, _objectMachine, _childObjectMachine, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets GraphFile objects from the datasource by CAid in the
		///		GraphFileAssociation table. Table GraphFile is related to table ObjectAssociation
		///		through the (M:N) relationship defined in the GraphFileAssociation table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_cAid"></param>
		/// <param name="_objectID"></param>
		/// <param name="_childObjectID"></param>
		/// <param name="_objectMachine"></param>
		/// <param name="_childObjectMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of GraphFile objects.</returns>
		public TList<GraphFile> GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation(TransactionManager transactionManager, System.Int32 _cAid, System.Int32 _objectID, System.Int32 _childObjectID, System.String _objectMachine, System.String _childObjectMachine)
		{
			int count = -1;
			return GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation(transactionManager, _cAid, _objectID, _childObjectID, _objectMachine, _childObjectMachine, 0, int.MaxValue, out count);
		}
		
		
		/// <summary>
		///		Gets GraphFile objects from the datasource by CAid in the
		///		GraphFileAssociation table. Table GraphFile is related to table ObjectAssociation
		///		through the (M:N) relationship defined in the GraphFileAssociation table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_cAid"></param>
		/// <param name="_objectID"></param>
		/// <param name="_childObjectID"></param>
		/// <param name="_objectMachine"></param>
		/// <param name="_childObjectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of GraphFile objects.</returns>
		public TList<GraphFile> GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation(TransactionManager transactionManager, System.Int32 _cAid, System.Int32 _objectID, System.Int32 _childObjectID, System.String _objectMachine, System.String _childObjectMachine,int start, int pageLength)
		{
			int count = -1;
			return GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation(transactionManager, _cAid, _objectID, _childObjectID, _objectMachine, _childObjectMachine, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets GraphFile objects from the datasource by CAid in the
		///		GraphFileAssociation table. Table GraphFile is related to table ObjectAssociation
		///		through the (M:N) relationship defined in the GraphFileAssociation table.
		/// </summary>
		/// <param name="_cAid"></param>
		/// <param name="_objectID"></param>
		/// <param name="_childObjectID"></param>
		/// <param name="_objectMachine"></param>
		/// <param name="_childObjectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of GraphFile objects.</returns>
		public TList<GraphFile> GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation(System.Int32 _cAid, System.Int32 _objectID, System.Int32 _childObjectID, System.String _objectMachine, System.String _childObjectMachine,int start, int pageLength, out int count)
		{
			
			return GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation(null, _cAid, _objectID, _childObjectID, _objectMachine, _childObjectMachine, start, pageLength, out count);
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
		/// <param name="_cAid"></param>
		/// <param name="_objectID"></param>
		/// <param name="_childObjectID"></param>
		/// <param name="_objectMachine"></param>
		/// <param name="_childObjectMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of GraphFile objects.</returns>
		public abstract TList<GraphFile> GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation(TransactionManager transactionManager,System.Int32 _cAid, System.Int32 _objectID, System.Int32 _childObjectID, System.String _objectMachine, System.String _childObjectMachine, int start, int pageLength, out int count);
		
		#endregion GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation
		
		#region GetByMetaObjectIDMachineIDFromGraphFileObject
		
		/// <summary>
		///		Gets GraphFile objects from the datasource by MetaObjectID in the
		///		GraphFileObject table. Table GraphFile is related to table MetaObject
		///		through the (M:N) relationship defined in the GraphFileObject table.
		/// </summary>
		/// <param name="_metaObjectID"></param>
		/// <param name="_machineID"></param>
		/// <returns>Returns a typed collection of GraphFile objects.</returns>
		public TList<GraphFile> GetByMetaObjectIDMachineIDFromGraphFileObject(System.Int32 _metaObjectID, System.String _machineID)
		{
			int count = -1;
			return GetByMetaObjectIDMachineIDFromGraphFileObject(null,_metaObjectID, _machineID, 0, int.MaxValue, out count);
			
		}
		
		/// <summary>
		///		Gets MetaBuilder.BusinessLogic.GraphFile objects from the datasource by MetaObjectID in the
		///		GraphFileObject table. Table GraphFile is related to table MetaObject
		///		through the (M:N) relationship defined in the GraphFileObject table.
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_metaObjectID"></param>
		/// <param name="_machineID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of GraphFile objects.</returns>
		public TList<GraphFile> GetByMetaObjectIDMachineIDFromGraphFileObject(System.Int32 _metaObjectID, System.String _machineID, int start, int pageLength)
		{
			int count = -1;
			return GetByMetaObjectIDMachineIDFromGraphFileObject(null, _metaObjectID, _machineID, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets GraphFile objects from the datasource by MetaObjectID in the
		///		GraphFileObject table. Table GraphFile is related to table MetaObject
		///		through the (M:N) relationship defined in the GraphFileObject table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_metaObjectID"></param>
		/// <param name="_machineID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of GraphFile objects.</returns>
		public TList<GraphFile> GetByMetaObjectIDMachineIDFromGraphFileObject(TransactionManager transactionManager, System.Int32 _metaObjectID, System.String _machineID)
		{
			int count = -1;
			return GetByMetaObjectIDMachineIDFromGraphFileObject(transactionManager, _metaObjectID, _machineID, 0, int.MaxValue, out count);
		}
		
		
		/// <summary>
		///		Gets GraphFile objects from the datasource by MetaObjectID in the
		///		GraphFileObject table. Table GraphFile is related to table MetaObject
		///		through the (M:N) relationship defined in the GraphFileObject table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_metaObjectID"></param>
		/// <param name="_machineID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of GraphFile objects.</returns>
		public TList<GraphFile> GetByMetaObjectIDMachineIDFromGraphFileObject(TransactionManager transactionManager, System.Int32 _metaObjectID, System.String _machineID,int start, int pageLength)
		{
			int count = -1;
			return GetByMetaObjectIDMachineIDFromGraphFileObject(transactionManager, _metaObjectID, _machineID, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets GraphFile objects from the datasource by MetaObjectID in the
		///		GraphFileObject table. Table GraphFile is related to table MetaObject
		///		through the (M:N) relationship defined in the GraphFileObject table.
		/// </summary>
		/// <param name="_metaObjectID"></param>
		/// <param name="_machineID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of GraphFile objects.</returns>
		public TList<GraphFile> GetByMetaObjectIDMachineIDFromGraphFileObject(System.Int32 _metaObjectID, System.String _machineID,int start, int pageLength, out int count)
		{
			
			return GetByMetaObjectIDMachineIDFromGraphFileObject(null, _metaObjectID, _machineID, start, pageLength, out count);
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
		/// <param name="_metaObjectID"></param>
		/// <param name="_machineID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of GraphFile objects.</returns>
		public abstract TList<GraphFile> GetByMetaObjectIDMachineIDFromGraphFileObject(TransactionManager transactionManager,System.Int32 _metaObjectID, System.String _machineID, int start, int pageLength, out int count);
		
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
			return Delete(transactionManager, key.pkid, key.Machine);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="_pkid">. Primary Key.</param>
		/// <param name="_machine">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(System.Int32 _pkid, System.String _machine)
		{
			return Delete(null, _pkid, _machine);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_pkid">. Primary Key.</param>
		/// <param name="_machine">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(TransactionManager transactionManager, System.Int32 _pkid, System.String _machine);		
		
		#endregion Delete Methods
		
		#region Get By Foreign Key Functions
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_VCStatus key.
		///		FK_GraphFile_VCStatus Description: 
		/// </summary>
		/// <param name="_VCStatusID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		public TList<GraphFile> GetByVCStatusID(System.Int32 _VCStatusID)
		{
			int count = -1;
			return GetByVCStatusID(_VCStatusID, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_VCStatus key.
		///		FK_GraphFile_VCStatus Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_VCStatusID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		/// <remarks></remarks>
		public TList<GraphFile> GetByVCStatusID(TransactionManager transactionManager, System.Int32 _VCStatusID)
		{
			int count = -1;
			return GetByVCStatusID(transactionManager, _VCStatusID, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_VCStatus key.
		///		FK_GraphFile_VCStatus Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_VCStatusID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		public TList<GraphFile> GetByVCStatusID(TransactionManager transactionManager, System.Int32 _VCStatusID, int start, int pageLength)
		{
			int count = -1;
			return GetByVCStatusID(transactionManager, _VCStatusID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_VCStatus key.
		///		fK_GraphFile_VCStatus Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_VCStatusID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		public TList<GraphFile> GetByVCStatusID(System.Int32 _VCStatusID, int start, int pageLength)
		{
			int count =  -1;
			return GetByVCStatusID(null, _VCStatusID, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_VCStatus key.
		///		fK_GraphFile_VCStatus Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_VCStatusID"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		public TList<GraphFile> GetByVCStatusID(System.Int32 _VCStatusID, int start, int pageLength,out int count)
		{
			return GetByVCStatusID(null, _VCStatusID, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_VCStatus key.
		///		FK_GraphFile_VCStatus Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_VCStatusID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		public abstract TList<GraphFile> GetByVCStatusID(TransactionManager transactionManager, System.Int32 _VCStatusID, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_FileType key.
		///		FK_GraphFile_FileType Description: 
		/// </summary>
		/// <param name="_fileTypeID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		public TList<GraphFile> GetByFileTypeID(System.Int32 _fileTypeID)
		{
			int count = -1;
			return GetByFileTypeID(_fileTypeID, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_FileType key.
		///		FK_GraphFile_FileType Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_fileTypeID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		/// <remarks></remarks>
		public TList<GraphFile> GetByFileTypeID(TransactionManager transactionManager, System.Int32 _fileTypeID)
		{
			int count = -1;
			return GetByFileTypeID(transactionManager, _fileTypeID, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_FileType key.
		///		FK_GraphFile_FileType Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_fileTypeID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		public TList<GraphFile> GetByFileTypeID(TransactionManager transactionManager, System.Int32 _fileTypeID, int start, int pageLength)
		{
			int count = -1;
			return GetByFileTypeID(transactionManager, _fileTypeID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_FileType key.
		///		fK_GraphFile_FileType Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_fileTypeID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		public TList<GraphFile> GetByFileTypeID(System.Int32 _fileTypeID, int start, int pageLength)
		{
			int count =  -1;
			return GetByFileTypeID(null, _fileTypeID, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_FileType key.
		///		fK_GraphFile_FileType Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_fileTypeID"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		public TList<GraphFile> GetByFileTypeID(System.Int32 _fileTypeID, int start, int pageLength,out int count)
		{
			return GetByFileTypeID(null, _fileTypeID, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_FileType key.
		///		FK_GraphFile_FileType Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_fileTypeID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		public abstract TList<GraphFile> GetByFileTypeID(TransactionManager transactionManager, System.Int32 _fileTypeID, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_Workspace key.
		///		FK_GraphFile_Workspace Description: 
		/// </summary>
		/// <param name="_workspaceName"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		public TList<GraphFile> GetByWorkspaceNameWorkspaceTypeId(System.String _workspaceName, System.Int32 _WorkspaceTypeId)
		{
			int count = -1;
			return GetByWorkspaceNameWorkspaceTypeId(_workspaceName, _WorkspaceTypeId, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_Workspace key.
		///		FK_GraphFile_Workspace Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_workspaceName"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		/// <remarks></remarks>
		public TList<GraphFile> GetByWorkspaceNameWorkspaceTypeId(TransactionManager transactionManager, System.String _workspaceName, System.Int32 _WorkspaceTypeId)
		{
			int count = -1;
			return GetByWorkspaceNameWorkspaceTypeId(transactionManager, _workspaceName, _WorkspaceTypeId, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_Workspace key.
		///		FK_GraphFile_Workspace Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_workspaceName"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		public TList<GraphFile> GetByWorkspaceNameWorkspaceTypeId(TransactionManager transactionManager, System.String _workspaceName, System.Int32 _WorkspaceTypeId, int start, int pageLength)
		{
			int count = -1;
			return GetByWorkspaceNameWorkspaceTypeId(transactionManager, _workspaceName, _WorkspaceTypeId, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_Workspace key.
		///		fK_GraphFile_Workspace Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_workspaceName"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		public TList<GraphFile> GetByWorkspaceNameWorkspaceTypeId(System.String _workspaceName, System.Int32 _WorkspaceTypeId, int start, int pageLength)
		{
			int count =  -1;
			return GetByWorkspaceNameWorkspaceTypeId(null, _workspaceName, _WorkspaceTypeId, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_Workspace key.
		///		fK_GraphFile_Workspace Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_workspaceName"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		public TList<GraphFile> GetByWorkspaceNameWorkspaceTypeId(System.String _workspaceName, System.Int32 _WorkspaceTypeId, int start, int pageLength,out int count)
		{
			return GetByWorkspaceNameWorkspaceTypeId(null, _workspaceName, _WorkspaceTypeId, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_GraphFile_Workspace key.
		///		FK_GraphFile_Workspace Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_workspaceName"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.GraphFile objects.</returns>
		public abstract TList<GraphFile> GetByWorkspaceNameWorkspaceTypeId(TransactionManager transactionManager, System.String _workspaceName, System.Int32 _WorkspaceTypeId, int start, int pageLength, out int count);
		
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
			return GetBypkidMachine(transactionManager, key.pkid, key.Machine, start, pageLength);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key PK_Version index.
		/// </summary>
		/// <param name="_pkid"></param>
		/// <param name="_machine"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.GraphFile"/> class.</returns>
		public MetaBuilder.BusinessLogic.GraphFile GetBypkidMachine(System.Int32 _pkid, System.String _machine)
		{
			int count = -1;
			return GetBypkidMachine(null,_pkid, _machine, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Version index.
		/// </summary>
		/// <param name="_pkid"></param>
		/// <param name="_machine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.GraphFile"/> class.</returns>
		public MetaBuilder.BusinessLogic.GraphFile GetBypkidMachine(System.Int32 _pkid, System.String _machine, int start, int pageLength)
		{
			int count = -1;
			return GetBypkidMachine(null, _pkid, _machine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Version index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_pkid"></param>
		/// <param name="_machine"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.GraphFile"/> class.</returns>
		public MetaBuilder.BusinessLogic.GraphFile GetBypkidMachine(TransactionManager transactionManager, System.Int32 _pkid, System.String _machine)
		{
			int count = -1;
			return GetBypkidMachine(transactionManager, _pkid, _machine, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Version index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_pkid"></param>
		/// <param name="_machine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.GraphFile"/> class.</returns>
		public MetaBuilder.BusinessLogic.GraphFile GetBypkidMachine(TransactionManager transactionManager, System.Int32 _pkid, System.String _machine, int start, int pageLength)
		{
			int count = -1;
			return GetBypkidMachine(transactionManager, _pkid, _machine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Version index.
		/// </summary>
		/// <param name="_pkid"></param>
		/// <param name="_machine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.GraphFile"/> class.</returns>
		public MetaBuilder.BusinessLogic.GraphFile GetBypkidMachine(System.Int32 _pkid, System.String _machine, int start, int pageLength, out int count)
		{
			return GetBypkidMachine(null, _pkid, _machine, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Version index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_pkid"></param>
		/// <param name="_machine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.GraphFile"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.GraphFile GetBypkidMachine(TransactionManager transactionManager, System.Int32 _pkid, System.String _machine, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a TList&lt;GraphFile&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="TList&lt;GraphFile&gt;"/></returns>
		public static TList<GraphFile> Fill(IDataReader reader, TList<GraphFile> rows, int start, int pageLength)
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
				
				MetaBuilder.BusinessLogic.GraphFile c = null;
				if (useEntityFactory)
				{
					key = new System.Text.StringBuilder("GraphFile")
					.Append("|").Append((System.Int32)reader[((int)GraphFileColumn.pkid - 1)])
					.Append("|").Append((System.String)reader[((int)GraphFileColumn.Machine - 1)]).ToString();
					c = EntityManager.LocateOrCreate<GraphFile>(
					key.ToString(), // EntityTrackingKey
					"GraphFile",  //Creational Type
					entityCreationFactoryType,  //Factory used to create entity
					enableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.GraphFile();
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
					c.pkid = (System.Int32)reader[((int)GraphFileColumn.pkid - 1)];
					c.MajorVersion = (System.Int32)reader[((int)GraphFileColumn.MajorVersion - 1)];
					c.MinorVersion = (System.Int32)reader[((int)GraphFileColumn.MinorVersion - 1)];
					c.ModifiedDate = (System.DateTime)reader[((int)GraphFileColumn.ModifiedDate - 1)];
					c.Notes = (reader.IsDBNull(((int)GraphFileColumn.Notes - 1)))?null:(System.String)reader[((int)GraphFileColumn.Notes - 1)];
					c.IsActive = (System.Boolean)reader[((int)GraphFileColumn.IsActive - 1)];
					c.Archived = (System.Boolean)reader[((int)GraphFileColumn.Archived - 1)];
					c.AppVersion = (System.String)reader[((int)GraphFileColumn.AppVersion - 1)];
					c.Blob = (System.Byte[])reader[((int)GraphFileColumn.Blob - 1)];
					c.WorkspaceName = (System.String)reader[((int)GraphFileColumn.WorkspaceName - 1)];
					c.FileTypeID = (System.Int32)reader[((int)GraphFileColumn.FileTypeID - 1)];
					c.PreviousVersionID = (reader.IsDBNull(((int)GraphFileColumn.PreviousVersionID - 1)))?null:(System.Int32?)reader[((int)GraphFileColumn.PreviousVersionID - 1)];
					c.Name = (System.String)reader[((int)GraphFileColumn.Name - 1)];
					c.VCStatusID = (System.Int32)reader[((int)GraphFileColumn.VCStatusID - 1)];
					c.VCMachineID = (reader.IsDBNull(((int)GraphFileColumn.VCMachineID - 1)))?null:(System.String)reader[((int)GraphFileColumn.VCMachineID - 1)];
					c.Machine = (System.String)reader[((int)GraphFileColumn.Machine - 1)];
					c.OriginalMachine = c.Machine;
					c.WorkspaceTypeId = (System.Int32)reader[((int)GraphFileColumn.WorkspaceTypeId - 1)];
					c.OriginalFileUniqueID = (System.Guid)reader[((int)GraphFileColumn.OriginalFileUniqueID - 1)];
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
			
			entity.pkid = (System.Int32)reader[((int)GraphFileColumn.pkid - 1)];
			entity.MajorVersion = (System.Int32)reader[((int)GraphFileColumn.MajorVersion - 1)];
			entity.MinorVersion = (System.Int32)reader[((int)GraphFileColumn.MinorVersion - 1)];
			entity.ModifiedDate = (System.DateTime)reader[((int)GraphFileColumn.ModifiedDate - 1)];
			entity.Notes = (reader.IsDBNull(((int)GraphFileColumn.Notes - 1)))?null:(System.String)reader[((int)GraphFileColumn.Notes - 1)];
			entity.IsActive = (System.Boolean)reader[((int)GraphFileColumn.IsActive - 1)];
			entity.Archived = (System.Boolean)reader[((int)GraphFileColumn.Archived - 1)];
			entity.AppVersion = (System.String)reader[((int)GraphFileColumn.AppVersion - 1)];
			entity.Blob = (System.Byte[])reader[((int)GraphFileColumn.Blob - 1)];
			entity.WorkspaceName = (System.String)reader[((int)GraphFileColumn.WorkspaceName - 1)];
			entity.FileTypeID = (System.Int32)reader[((int)GraphFileColumn.FileTypeID - 1)];
			entity.PreviousVersionID = (reader.IsDBNull(((int)GraphFileColumn.PreviousVersionID - 1)))?null:(System.Int32?)reader[((int)GraphFileColumn.PreviousVersionID - 1)];
			entity.Name = (System.String)reader[((int)GraphFileColumn.Name - 1)];
			entity.VCStatusID = (System.Int32)reader[((int)GraphFileColumn.VCStatusID - 1)];
			entity.VCMachineID = (reader.IsDBNull(((int)GraphFileColumn.VCMachineID - 1)))?null:(System.String)reader[((int)GraphFileColumn.VCMachineID - 1)];
			entity.Machine = (System.String)reader[((int)GraphFileColumn.Machine - 1)];
			entity.OriginalMachine = (System.String)reader["Machine"];
			entity.WorkspaceTypeId = (System.Int32)reader[((int)GraphFileColumn.WorkspaceTypeId - 1)];
			entity.OriginalFileUniqueID = (System.Guid)reader[((int)GraphFileColumn.OriginalFileUniqueID - 1)];
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
			
			entity.pkid = (System.Int32)dataRow["pkid"];
			entity.MajorVersion = (System.Int32)dataRow["MajorVersion"];
			entity.MinorVersion = (System.Int32)dataRow["MinorVersion"];
			entity.ModifiedDate = (System.DateTime)dataRow["ModifiedDate"];
			entity.Notes = Convert.IsDBNull(dataRow["Notes"]) ? null : (System.String)dataRow["Notes"];
			entity.IsActive = (System.Boolean)dataRow["IsActive"];
			entity.Archived = (System.Boolean)dataRow["Archived"];
			entity.AppVersion = (System.String)dataRow["AppVersion"];
			entity.Blob = (System.Byte[])dataRow["Blob"];
			entity.WorkspaceName = (System.String)dataRow["WorkspaceName"];
			entity.FileTypeID = (System.Int32)dataRow["FileTypeID"];
			entity.PreviousVersionID = Convert.IsDBNull(dataRow["PreviousVersionID"]) ? null : (System.Int32?)dataRow["PreviousVersionID"];
			entity.Name = (System.String)dataRow["Name"];
			entity.VCStatusID = (System.Int32)dataRow["VCStatusID"];
			entity.VCMachineID = Convert.IsDBNull(dataRow["VCMachineID"]) ? null : (System.String)dataRow["VCMachineID"];
			entity.Machine = (System.String)dataRow["Machine"];
			entity.OriginalMachine = (System.String)dataRow["Machine"];
			entity.WorkspaceTypeId = (System.Int32)dataRow["WorkspaceTypeId"];
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
		public override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.GraphFile entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, DeepSession innerList)
		{
			if(entity == null)
				return;

			#region VCStatusIDSource	
			if (CanDeepLoad(entity, "VCStatus|VCStatusIDSource", deepLoadType, innerList) 
				&& entity.VCStatusIDSource == null)
			{
				object[] pkItems = new object[1];
				pkItems[0] = entity.VCStatusID;
				VCStatus tmpEntity = EntityManager.LocateEntity<VCStatus>(EntityLocator.ConstructKeyFromPkItems(typeof(VCStatus), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.VCStatusIDSource = tmpEntity;
				else
					entity.VCStatusIDSource = DataRepository.VCStatusProvider.GetBypkid(transactionManager, entity.VCStatusID);		
				
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'VCStatusIDSource' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.VCStatusIDSource != null)
				{
					innerList.SkipChildren = true;
					DataRepository.VCStatusProvider.DeepLoad(transactionManager, entity.VCStatusIDSource, deep, deepLoadType, childTypes, innerList);
					innerList.SkipChildren = false;
				}
					
			}
			#endregion VCStatusIDSource

			#region FileTypeIDSource	
			if (CanDeepLoad(entity, "FileType|FileTypeIDSource", deepLoadType, innerList) 
				&& entity.FileTypeIDSource == null)
			{
				object[] pkItems = new object[1];
				pkItems[0] = entity.FileTypeID;
				FileType tmpEntity = EntityManager.LocateEntity<FileType>(EntityLocator.ConstructKeyFromPkItems(typeof(FileType), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.FileTypeIDSource = tmpEntity;
				else
					entity.FileTypeIDSource = DataRepository.FileTypeProvider.GetBypkid(transactionManager, entity.FileTypeID);		
				
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'FileTypeIDSource' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.FileTypeIDSource != null)
				{
					innerList.SkipChildren = true;
					DataRepository.FileTypeProvider.DeepLoad(transactionManager, entity.FileTypeIDSource, deep, deepLoadType, childTypes, innerList);
					innerList.SkipChildren = false;
				}
					
			}
			#endregion FileTypeIDSource

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
			// Deep load child collections  - Call GetBypkidMachine methods when available
			
			#region CAidObjectIDChildObjectIDObjectMachineChildObjectMachineObjectAssociationCollection_From_GraphFileAssociation
			// RelationshipType.ManyToMany
			if (CanDeepLoad(entity, "List<ObjectAssociation>|CAidObjectIDChildObjectIDObjectMachineChildObjectMachineObjectAssociationCollection_From_GraphFileAssociation", deepLoadType, innerList))
			{
				entity.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineObjectAssociationCollection_From_GraphFileAssociation = DataRepository.ObjectAssociationProvider.GetByGraphFileIDGraphFileMachineFromGraphFileAssociation(transactionManager, entity.pkid, entity.Machine);			 
		
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'CAidObjectIDChildObjectIDObjectMachineChildObjectMachineObjectAssociationCollection_From_GraphFileAssociation' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineObjectAssociationCollection_From_GraphFileAssociation != null)
				{
					deepHandles.Add("CAidObjectIDChildObjectIDObjectMachineChildObjectMachineObjectAssociationCollection_From_GraphFileAssociation",
						new KeyValuePair<Delegate, object>((DeepLoadHandle< ObjectAssociation >) DataRepository.ObjectAssociationProvider.DeepLoad,
						new object[] { transactionManager, entity.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineObjectAssociationCollection_From_GraphFileAssociation, deep, deepLoadType, childTypes, innerList }
					));
				}
			}
			#endregion
			
			
			
			#region MetaObjectIDMachineIDMetaObjectCollection_From_GraphFileObject
			// RelationshipType.ManyToMany
			if (CanDeepLoad(entity, "List<MetaObject>|MetaObjectIDMachineIDMetaObjectCollection_From_GraphFileObject", deepLoadType, innerList))
			{
				entity.MetaObjectIDMachineIDMetaObjectCollection_From_GraphFileObject = DataRepository.MetaObjectProvider.GetByGraphFileIDGraphFileMachineFromGraphFileObject(transactionManager, entity.pkid, entity.Machine);			 
		
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'MetaObjectIDMachineIDMetaObjectCollection_From_GraphFileObject' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.MetaObjectIDMachineIDMetaObjectCollection_From_GraphFileObject != null)
				{
					deepHandles.Add("MetaObjectIDMachineIDMetaObjectCollection_From_GraphFileObject",
						new KeyValuePair<Delegate, object>((DeepLoadHandle< MetaObject >) DataRepository.MetaObjectProvider.DeepLoad,
						new object[] { transactionManager, entity.MetaObjectIDMachineIDMetaObjectCollection_From_GraphFileObject, deep, deepLoadType, childTypes, innerList }
					));
				}
			}
			#endregion
			
			
			
			#region GraphFileObjectCollection
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<GraphFileObject>|GraphFileObjectCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'GraphFileObjectCollection' loaded. key " + entity.EntityTrackingKey);
				#endif 

				entity.GraphFileObjectCollection = DataRepository.GraphFileObjectProvider.GetByGraphFileIDGraphFileMachine(transactionManager, entity.pkid, entity.Machine);

				if (deep && entity.GraphFileObjectCollection.Count > 0)
				{
					deepHandles.Add("GraphFileObjectCollection",
						new KeyValuePair<Delegate, object>((DeepLoadHandle<GraphFileObject>) DataRepository.GraphFileObjectProvider.DeepLoad,
						new object[] { transactionManager, entity.GraphFileObjectCollection, deep, deepLoadType, childTypes, innerList }
					));
				}
			}		
			#endregion 
			
			
			#region GraphFileAssociationCollection
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<GraphFileAssociation>|GraphFileAssociationCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'GraphFileAssociationCollection' loaded. key " + entity.EntityTrackingKey);
				#endif 

				entity.GraphFileAssociationCollection = DataRepository.GraphFileAssociationProvider.GetByGraphFileIDGraphFileMachine(transactionManager, entity.pkid, entity.Machine);

				if (deep && entity.GraphFileAssociationCollection.Count > 0)
				{
					deepHandles.Add("GraphFileAssociationCollection",
						new KeyValuePair<Delegate, object>((DeepLoadHandle<GraphFileAssociation>) DataRepository.GraphFileAssociationProvider.DeepLoad,
						new object[] { transactionManager, entity.GraphFileAssociationCollection, deep, deepLoadType, childTypes, innerList }
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
		/// Deep Save the entire object graph of the MetaBuilder.BusinessLogic.GraphFile object with criteria based of the child 
		/// Type property array and DeepSaveType.
		/// </summary>
		/// <param name="transactionManager">The transaction manager.</param>
		/// <param name="entity">MetaBuilder.BusinessLogic.GraphFile instance</param>
		/// <param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.GraphFile Property Collection Type Array To Include or Exclude from Save</param>
		/// <param name="innerList">A Hashtable of child types for easy access.</param>
		public override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.GraphFile entity, DeepSaveType deepSaveType, System.Type[] childTypes, DeepSession innerList)
		{	
			if (entity == null)
				return false;
							
			#region Composite Parent Properties
			//Save Source Composite Properties, however, don't call deep save on them.  
			//So they only get saved a single level deep.
			
			#region VCStatusIDSource
			if (CanDeepSave(entity, "VCStatus|VCStatusIDSource", deepSaveType, innerList) 
				&& entity.VCStatusIDSource != null)
			{
				DataRepository.VCStatusProvider.Save(transactionManager, entity.VCStatusIDSource);
				entity.VCStatusID = entity.VCStatusIDSource.pkid;
			}
			#endregion 
			
			#region FileTypeIDSource
			if (CanDeepSave(entity, "FileType|FileTypeIDSource", deepSaveType, innerList) 
				&& entity.FileTypeIDSource != null)
			{
				DataRepository.FileTypeProvider.Save(transactionManager, entity.FileTypeIDSource);
				entity.FileTypeID = entity.FileTypeIDSource.pkid;
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

			#region CAidObjectIDChildObjectIDObjectMachineChildObjectMachineObjectAssociationCollection_From_GraphFileAssociation>
			if (CanDeepSave(entity.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineObjectAssociationCollection_From_GraphFileAssociation, "List<ObjectAssociation>|CAidObjectIDChildObjectIDObjectMachineChildObjectMachineObjectAssociationCollection_From_GraphFileAssociation", deepSaveType, innerList))
			{
				if (entity.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineObjectAssociationCollection_From_GraphFileAssociation.Count > 0 || entity.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineObjectAssociationCollection_From_GraphFileAssociation.DeletedItems.Count > 0)
				{
					DataRepository.ObjectAssociationProvider.Save(transactionManager, entity.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineObjectAssociationCollection_From_GraphFileAssociation); 
					deepHandles.Add("CAidObjectIDChildObjectIDObjectMachineChildObjectMachineObjectAssociationCollection_From_GraphFileAssociation",
						new KeyValuePair<Delegate, object>((DeepSaveHandle<ObjectAssociation>) DataRepository.ObjectAssociationProvider.DeepSave,
						new object[] { transactionManager, entity.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineObjectAssociationCollection_From_GraphFileAssociation, deepSaveType, childTypes, innerList }
					));
				}
			}
			#endregion 

			#region MetaObjectIDMachineIDMetaObjectCollection_From_GraphFileObject>
			if (CanDeepSave(entity.MetaObjectIDMachineIDMetaObjectCollection_From_GraphFileObject, "List<MetaObject>|MetaObjectIDMachineIDMetaObjectCollection_From_GraphFileObject", deepSaveType, innerList))
			{
				if (entity.MetaObjectIDMachineIDMetaObjectCollection_From_GraphFileObject.Count > 0 || entity.MetaObjectIDMachineIDMetaObjectCollection_From_GraphFileObject.DeletedItems.Count > 0)
				{
					DataRepository.MetaObjectProvider.Save(transactionManager, entity.MetaObjectIDMachineIDMetaObjectCollection_From_GraphFileObject); 
					deepHandles.Add("MetaObjectIDMachineIDMetaObjectCollection_From_GraphFileObject",
						new KeyValuePair<Delegate, object>((DeepSaveHandle<MetaObject>) DataRepository.MetaObjectProvider.DeepSave,
						new object[] { transactionManager, entity.MetaObjectIDMachineIDMetaObjectCollection_From_GraphFileObject, deepSaveType, childTypes, innerList }
					));
				}
			}
			#endregion 
	
			#region List<GraphFileObject>
				if (CanDeepSave(entity.GraphFileObjectCollection, "List<GraphFileObject>|GraphFileObjectCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(GraphFileObject child in entity.GraphFileObjectCollection)
					{
						if(child.GraphFileIDGraphFileMachineSource != null)
						{
								child.GraphFileID = child.GraphFileIDGraphFileMachineSource.pkid;
								child.GraphFileMachine = child.GraphFileIDGraphFileMachineSource.Machine;
						}

						if(child.MetaObjectIDMachineIDSource != null)
						{
								child.MetaObjectID = child.MetaObjectIDMachineIDSource.pkid;
								child.MachineID = child.MetaObjectIDMachineIDSource.Machine;
						}

					}

					if (entity.GraphFileObjectCollection.Count > 0 || entity.GraphFileObjectCollection.DeletedItems.Count > 0)
					{
						//DataRepository.GraphFileObjectProvider.Save(transactionManager, entity.GraphFileObjectCollection);
						
						deepHandles.Add("GraphFileObjectCollection",
						new KeyValuePair<Delegate, object>((DeepSaveHandle< GraphFileObject >) DataRepository.GraphFileObjectProvider.DeepSave,
							new object[] { transactionManager, entity.GraphFileObjectCollection, deepSaveType, childTypes, innerList }
						));
					}
				} 
			#endregion 
				
	
			#region List<GraphFileAssociation>
				if (CanDeepSave(entity.GraphFileAssociationCollection, "List<GraphFileAssociation>|GraphFileAssociationCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(GraphFileAssociation child in entity.GraphFileAssociationCollection)
					{
						if(child.GraphFileIDGraphFileMachineSource != null)
						{
								child.GraphFileID = child.GraphFileIDGraphFileMachineSource.pkid;
								child.GraphFileMachine = child.GraphFileIDGraphFileMachineSource.Machine;
						}

						if(child.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineSource != null)
						{
								child.CAid = child.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineSource.CAid;
								child.ObjectID = child.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineSource.ObjectID;
								child.ChildObjectID = child.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineSource.ChildObjectID;
								child.ObjectMachine = child.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineSource.ObjectMachine;
								child.ChildObjectMachine = child.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineSource.ChildObjectMachine;
						}

					}

					if (entity.GraphFileAssociationCollection.Count > 0 || entity.GraphFileAssociationCollection.DeletedItems.Count > 0)
					{
						//DataRepository.GraphFileAssociationProvider.Save(transactionManager, entity.GraphFileAssociationCollection);
						
						deepHandles.Add("GraphFileAssociationCollection",
						new KeyValuePair<Delegate, object>((DeepSaveHandle< GraphFileAssociation >) DataRepository.GraphFileAssociationProvider.DeepSave,
							new object[] { transactionManager, entity.GraphFileAssociationCollection, deepSaveType, childTypes, innerList }
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
		/// Composite Property for <c>Workspace</c> at WorkspaceNameWorkspaceTypeIdSource
		///</summary>
		[ChildEntityType(typeof(Workspace))]
		Workspace,
	
		///<summary>
		/// Collection of <c>GraphFile</c> as ManyToMany for ObjectAssociationCollection_From_GraphFileAssociation
		///</summary>
		[ChildEntityType(typeof(TList<ObjectAssociation>))]
		CAidObjectIDChildObjectIDObjectMachineChildObjectMachineObjectAssociationCollection_From_GraphFileAssociation,

		///<summary>
		/// Collection of <c>GraphFile</c> as ManyToMany for MetaObjectCollection_From_GraphFileObject
		///</summary>
		[ChildEntityType(typeof(TList<MetaObject>))]
		MetaObjectIDMachineIDMetaObjectCollection_From_GraphFileObject,

		///<summary>
		/// Collection of <c>GraphFile</c> as OneToMany for GraphFileObjectCollection
		///</summary>
		[ChildEntityType(typeof(TList<GraphFileObject>))]
		GraphFileObjectCollection,

		///<summary>
		/// Collection of <c>GraphFile</c> as OneToMany for GraphFileAssociationCollection
		///</summary>
		[ChildEntityType(typeof(TList<GraphFileAssociation>))]
		GraphFileAssociationCollection,
	}
	
	#endregion GraphFileChildEntityTypes
	
	#region GraphFileFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;GraphFileColumn&gt;"/> class
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
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;GraphFileColumn&gt;"/> class
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
	
	#region GraphFileSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;GraphFileColumn&gt;"/> class
	/// that is used exclusively with a <see cref="GraphFile"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class GraphFileSortBuilder : SqlSortBuilder<GraphFileColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the GraphFileSqlSortBuilder class.
		/// </summary>
		public GraphFileSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion GraphFileSortBuilder
	
} // end namespace
