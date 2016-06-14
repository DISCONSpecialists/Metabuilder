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
	/// This class is the base class for any <see cref="ObjectAssociationProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract partial class ObjectAssociationProviderBaseCore : EntityProviderBase<MetaBuilder.BusinessLogic.ObjectAssociation, MetaBuilder.BusinessLogic.ObjectAssociationKey>
	{		
		#region Get from Many To Many Relationship Functions
		#region GetByGraphFileIDGraphFileMachineFromGraphFileAssociation
		
		/// <summary>
		///		Gets ObjectAssociation objects from the datasource by GraphFileID in the
		///		GraphFileAssociation table. Table ObjectAssociation is related to table GraphFile
		///		through the (M:N) relationship defined in the GraphFileAssociation table.
		/// </summary>
		/// <param name="_graphFileID"></param>
		/// <param name="_graphFileMachine"></param>
		/// <returns>Returns a typed collection of ObjectAssociation objects.</returns>
		public TList<ObjectAssociation> GetByGraphFileIDGraphFileMachineFromGraphFileAssociation(System.Int32 _graphFileID, System.String _graphFileMachine)
		{
			int count = -1;
			return GetByGraphFileIDGraphFileMachineFromGraphFileAssociation(null,_graphFileID, _graphFileMachine, 0, int.MaxValue, out count);
			
		}
		
		/// <summary>
		///		Gets MetaBuilder.BusinessLogic.ObjectAssociation objects from the datasource by GraphFileID in the
		///		GraphFileAssociation table. Table ObjectAssociation is related to table GraphFile
		///		through the (M:N) relationship defined in the GraphFileAssociation table.
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_graphFileID"></param>
		/// <param name="_graphFileMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of ObjectAssociation objects.</returns>
		public TList<ObjectAssociation> GetByGraphFileIDGraphFileMachineFromGraphFileAssociation(System.Int32 _graphFileID, System.String _graphFileMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByGraphFileIDGraphFileMachineFromGraphFileAssociation(null, _graphFileID, _graphFileMachine, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets ObjectAssociation objects from the datasource by GraphFileID in the
		///		GraphFileAssociation table. Table ObjectAssociation is related to table GraphFile
		///		through the (M:N) relationship defined in the GraphFileAssociation table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_graphFileID"></param>
		/// <param name="_graphFileMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of ObjectAssociation objects.</returns>
		public TList<ObjectAssociation> GetByGraphFileIDGraphFileMachineFromGraphFileAssociation(TransactionManager transactionManager, System.Int32 _graphFileID, System.String _graphFileMachine)
		{
			int count = -1;
			return GetByGraphFileIDGraphFileMachineFromGraphFileAssociation(transactionManager, _graphFileID, _graphFileMachine, 0, int.MaxValue, out count);
		}
		
		
		/// <summary>
		///		Gets ObjectAssociation objects from the datasource by GraphFileID in the
		///		GraphFileAssociation table. Table ObjectAssociation is related to table GraphFile
		///		through the (M:N) relationship defined in the GraphFileAssociation table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_graphFileID"></param>
		/// <param name="_graphFileMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of ObjectAssociation objects.</returns>
		public TList<ObjectAssociation> GetByGraphFileIDGraphFileMachineFromGraphFileAssociation(TransactionManager transactionManager, System.Int32 _graphFileID, System.String _graphFileMachine,int start, int pageLength)
		{
			int count = -1;
			return GetByGraphFileIDGraphFileMachineFromGraphFileAssociation(transactionManager, _graphFileID, _graphFileMachine, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets ObjectAssociation objects from the datasource by GraphFileID in the
		///		GraphFileAssociation table. Table ObjectAssociation is related to table GraphFile
		///		through the (M:N) relationship defined in the GraphFileAssociation table.
		/// </summary>
		/// <param name="_graphFileID"></param>
		/// <param name="_graphFileMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of ObjectAssociation objects.</returns>
		public TList<ObjectAssociation> GetByGraphFileIDGraphFileMachineFromGraphFileAssociation(System.Int32 _graphFileID, System.String _graphFileMachine,int start, int pageLength, out int count)
		{
			
			return GetByGraphFileIDGraphFileMachineFromGraphFileAssociation(null, _graphFileID, _graphFileMachine, start, pageLength, out count);
		}


		/// <summary>
		///		Gets ObjectAssociation objects from the datasource by GraphFileID in the
		///		GraphFileAssociation table. Table ObjectAssociation is related to table GraphFile
		///		through the (M:N) relationship defined in the GraphFileAssociation table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <param name="_graphFileID"></param>
		/// <param name="_graphFileMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of ObjectAssociation objects.</returns>
		public abstract TList<ObjectAssociation> GetByGraphFileIDGraphFileMachineFromGraphFileAssociation(TransactionManager transactionManager,System.Int32 _graphFileID, System.String _graphFileMachine, int start, int pageLength, out int count);
		
		#endregion GetByGraphFileIDGraphFileMachineFromGraphFileAssociation
		
		#endregion	
		
		#region Delete Methods

		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager">A <see cref="TransactionManager"/> object.</param>
		/// <param name="key">The unique identifier of the row to delete.</param>
		/// <returns>Returns true if operation suceeded.</returns>
		public override bool Delete(TransactionManager transactionManager, MetaBuilder.BusinessLogic.ObjectAssociationKey key)
		{
			return Delete(transactionManager, key.CAid, key.ObjectID, key.ChildObjectID, key.ObjectMachine, key.ChildObjectMachine);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="_cAid">. Primary Key.</param>
		/// <param name="_objectID">. Primary Key.</param>
		/// <param name="_childObjectID">. Primary Key.</param>
		/// <param name="_objectMachine">. Primary Key.</param>
		/// <param name="_childObjectMachine">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(System.Int32 _cAid, System.Int32 _objectID, System.Int32 _childObjectID, System.String _objectMachine, System.String _childObjectMachine)
		{
			return Delete(null, _cAid, _objectID, _childObjectID, _objectMachine, _childObjectMachine);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_cAid">. Primary Key.</param>
		/// <param name="_objectID">. Primary Key.</param>
		/// <param name="_childObjectID">. Primary Key.</param>
		/// <param name="_objectMachine">. Primary Key.</param>
		/// <param name="_childObjectMachine">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(TransactionManager transactionManager, System.Int32 _cAid, System.Int32 _objectID, System.Int32 _childObjectID, System.String _objectMachine, System.String _childObjectMachine);		
		
		#endregion Delete Methods
		
		#region Get By Foreign Key Functions
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_VCStatus key.
		///		FK_ObjectAssociation_VCStatus Description: 
		/// </summary>
		/// <param name="_VCStatusID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public TList<ObjectAssociation> GetByVCStatusID(System.Int32 _VCStatusID)
		{
			int count = -1;
			return GetByVCStatusID(_VCStatusID, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_VCStatus key.
		///		FK_ObjectAssociation_VCStatus Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_VCStatusID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		/// <remarks></remarks>
		public TList<ObjectAssociation> GetByVCStatusID(TransactionManager transactionManager, System.Int32 _VCStatusID)
		{
			int count = -1;
			return GetByVCStatusID(transactionManager, _VCStatusID, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_VCStatus key.
		///		FK_ObjectAssociation_VCStatus Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_VCStatusID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public TList<ObjectAssociation> GetByVCStatusID(TransactionManager transactionManager, System.Int32 _VCStatusID, int start, int pageLength)
		{
			int count = -1;
			return GetByVCStatusID(transactionManager, _VCStatusID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_VCStatus key.
		///		fK_ObjectAssociation_VCStatus Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_VCStatusID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public TList<ObjectAssociation> GetByVCStatusID(System.Int32 _VCStatusID, int start, int pageLength)
		{
			int count =  -1;
			return GetByVCStatusID(null, _VCStatusID, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_VCStatus key.
		///		fK_ObjectAssociation_VCStatus Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_VCStatusID"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public TList<ObjectAssociation> GetByVCStatusID(System.Int32 _VCStatusID, int start, int pageLength,out int count)
		{
			return GetByVCStatusID(null, _VCStatusID, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_VCStatus key.
		///		FK_ObjectAssociation_VCStatus Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_VCStatusID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public abstract TList<ObjectAssociation> GetByVCStatusID(TransactionManager transactionManager, System.Int32 _VCStatusID, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_ClassAssociation key.
		///		FK_ObjectAssociation_ClassAssociation Description: 
		/// </summary>
		/// <param name="_cAid"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public TList<ObjectAssociation> GetByCAid(System.Int32 _cAid)
		{
			int count = -1;
			return GetByCAid(_cAid, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_ClassAssociation key.
		///		FK_ObjectAssociation_ClassAssociation Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_cAid"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		/// <remarks></remarks>
		public TList<ObjectAssociation> GetByCAid(TransactionManager transactionManager, System.Int32 _cAid)
		{
			int count = -1;
			return GetByCAid(transactionManager, _cAid, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_ClassAssociation key.
		///		FK_ObjectAssociation_ClassAssociation Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_cAid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public TList<ObjectAssociation> GetByCAid(TransactionManager transactionManager, System.Int32 _cAid, int start, int pageLength)
		{
			int count = -1;
			return GetByCAid(transactionManager, _cAid, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_ClassAssociation key.
		///		fK_ObjectAssociation_ClassAssociation Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_cAid"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public TList<ObjectAssociation> GetByCAid(System.Int32 _cAid, int start, int pageLength)
		{
			int count =  -1;
			return GetByCAid(null, _cAid, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_ClassAssociation key.
		///		fK_ObjectAssociation_ClassAssociation Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_cAid"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public TList<ObjectAssociation> GetByCAid(System.Int32 _cAid, int start, int pageLength,out int count)
		{
			return GetByCAid(null, _cAid, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_ClassAssociation key.
		///		FK_ObjectAssociation_ClassAssociation Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_cAid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public abstract TList<ObjectAssociation> GetByCAid(TransactionManager transactionManager, System.Int32 _cAid, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_MetaObject key.
		///		FK_ObjectAssociation_MetaObject Description: 
		/// </summary>
		/// <param name="_objectID"></param>
		/// <param name="_objectMachine"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public TList<ObjectAssociation> GetByObjectIDObjectMachine(System.Int32 _objectID, System.String _objectMachine)
		{
			int count = -1;
			return GetByObjectIDObjectMachine(_objectID, _objectMachine, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_MetaObject key.
		///		FK_ObjectAssociation_MetaObject Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_objectID"></param>
		/// <param name="_objectMachine"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		/// <remarks></remarks>
		public TList<ObjectAssociation> GetByObjectIDObjectMachine(TransactionManager transactionManager, System.Int32 _objectID, System.String _objectMachine)
		{
			int count = -1;
			return GetByObjectIDObjectMachine(transactionManager, _objectID, _objectMachine, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_MetaObject key.
		///		FK_ObjectAssociation_MetaObject Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_objectID"></param>
		/// <param name="_objectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public TList<ObjectAssociation> GetByObjectIDObjectMachine(TransactionManager transactionManager, System.Int32 _objectID, System.String _objectMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByObjectIDObjectMachine(transactionManager, _objectID, _objectMachine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_MetaObject key.
		///		fK_ObjectAssociation_MetaObject Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_objectID"></param>
		/// <param name="_objectMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public TList<ObjectAssociation> GetByObjectIDObjectMachine(System.Int32 _objectID, System.String _objectMachine, int start, int pageLength)
		{
			int count =  -1;
			return GetByObjectIDObjectMachine(null, _objectID, _objectMachine, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_MetaObject key.
		///		fK_ObjectAssociation_MetaObject Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_objectID"></param>
		/// <param name="_objectMachine"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public TList<ObjectAssociation> GetByObjectIDObjectMachine(System.Int32 _objectID, System.String _objectMachine, int start, int pageLength,out int count)
		{
			return GetByObjectIDObjectMachine(null, _objectID, _objectMachine, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_MetaObject key.
		///		FK_ObjectAssociation_MetaObject Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_objectID"></param>
		/// <param name="_objectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public abstract TList<ObjectAssociation> GetByObjectIDObjectMachine(TransactionManager transactionManager, System.Int32 _objectID, System.String _objectMachine, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_MetaObject1 key.
		///		FK_ObjectAssociation_MetaObject1 Description: 
		/// </summary>
		/// <param name="_childObjectID"></param>
		/// <param name="_childObjectMachine"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public TList<ObjectAssociation> GetByChildObjectIDChildObjectMachine(System.Int32 _childObjectID, System.String _childObjectMachine)
		{
			int count = -1;
			return GetByChildObjectIDChildObjectMachine(_childObjectID, _childObjectMachine, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_MetaObject1 key.
		///		FK_ObjectAssociation_MetaObject1 Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_childObjectID"></param>
		/// <param name="_childObjectMachine"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		/// <remarks></remarks>
		public TList<ObjectAssociation> GetByChildObjectIDChildObjectMachine(TransactionManager transactionManager, System.Int32 _childObjectID, System.String _childObjectMachine)
		{
			int count = -1;
			return GetByChildObjectIDChildObjectMachine(transactionManager, _childObjectID, _childObjectMachine, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_MetaObject1 key.
		///		FK_ObjectAssociation_MetaObject1 Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_childObjectID"></param>
		/// <param name="_childObjectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public TList<ObjectAssociation> GetByChildObjectIDChildObjectMachine(TransactionManager transactionManager, System.Int32 _childObjectID, System.String _childObjectMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByChildObjectIDChildObjectMachine(transactionManager, _childObjectID, _childObjectMachine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_MetaObject1 key.
		///		fK_ObjectAssociation_MetaObject1 Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_childObjectID"></param>
		/// <param name="_childObjectMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public TList<ObjectAssociation> GetByChildObjectIDChildObjectMachine(System.Int32 _childObjectID, System.String _childObjectMachine, int start, int pageLength)
		{
			int count =  -1;
			return GetByChildObjectIDChildObjectMachine(null, _childObjectID, _childObjectMachine, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_MetaObject1 key.
		///		fK_ObjectAssociation_MetaObject1 Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_childObjectID"></param>
		/// <param name="_childObjectMachine"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public TList<ObjectAssociation> GetByChildObjectIDChildObjectMachine(System.Int32 _childObjectID, System.String _childObjectMachine, int start, int pageLength,out int count)
		{
			return GetByChildObjectIDChildObjectMachine(null, _childObjectID, _childObjectMachine, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_MetaObject1 key.
		///		FK_ObjectAssociation_MetaObject1 Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_childObjectID"></param>
		/// <param name="_childObjectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public abstract TList<ObjectAssociation> GetByChildObjectIDChildObjectMachine(TransactionManager transactionManager, System.Int32 _childObjectID, System.String _childObjectMachine, int start, int pageLength, out int count);
		
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
		public override MetaBuilder.BusinessLogic.ObjectAssociation Get(TransactionManager transactionManager, MetaBuilder.BusinessLogic.ObjectAssociationKey key, int start, int pageLength)
		{
			return GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(transactionManager, key.CAid, key.ObjectID, key.ChildObjectID, key.ObjectMachine, key.ChildObjectMachine, start, pageLength);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key PK_ObjectRelationship index.
		/// </summary>
		/// <param name="_cAid"></param>
		/// <param name="_objectID"></param>
		/// <param name="_childObjectID"></param>
		/// <param name="_objectMachine"></param>
		/// <param name="_childObjectMachine"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ObjectAssociation"/> class.</returns>
		public MetaBuilder.BusinessLogic.ObjectAssociation GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(System.Int32 _cAid, System.Int32 _objectID, System.Int32 _childObjectID, System.String _objectMachine, System.String _childObjectMachine)
		{
			int count = -1;
			return GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(null,_cAid, _objectID, _childObjectID, _objectMachine, _childObjectMachine, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ObjectRelationship index.
		/// </summary>
		/// <param name="_cAid"></param>
		/// <param name="_objectID"></param>
		/// <param name="_childObjectID"></param>
		/// <param name="_objectMachine"></param>
		/// <param name="_childObjectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ObjectAssociation"/> class.</returns>
		public MetaBuilder.BusinessLogic.ObjectAssociation GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(System.Int32 _cAid, System.Int32 _objectID, System.Int32 _childObjectID, System.String _objectMachine, System.String _childObjectMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(null, _cAid, _objectID, _childObjectID, _objectMachine, _childObjectMachine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ObjectRelationship index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_cAid"></param>
		/// <param name="_objectID"></param>
		/// <param name="_childObjectID"></param>
		/// <param name="_objectMachine"></param>
		/// <param name="_childObjectMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ObjectAssociation"/> class.</returns>
		public MetaBuilder.BusinessLogic.ObjectAssociation GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(TransactionManager transactionManager, System.Int32 _cAid, System.Int32 _objectID, System.Int32 _childObjectID, System.String _objectMachine, System.String _childObjectMachine)
		{
			int count = -1;
			return GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(transactionManager, _cAid, _objectID, _childObjectID, _objectMachine, _childObjectMachine, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ObjectRelationship index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_cAid"></param>
		/// <param name="_objectID"></param>
		/// <param name="_childObjectID"></param>
		/// <param name="_objectMachine"></param>
		/// <param name="_childObjectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ObjectAssociation"/> class.</returns>
		public MetaBuilder.BusinessLogic.ObjectAssociation GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(TransactionManager transactionManager, System.Int32 _cAid, System.Int32 _objectID, System.Int32 _childObjectID, System.String _objectMachine, System.String _childObjectMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(transactionManager, _cAid, _objectID, _childObjectID, _objectMachine, _childObjectMachine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ObjectRelationship index.
		/// </summary>
		/// <param name="_cAid"></param>
		/// <param name="_objectID"></param>
		/// <param name="_childObjectID"></param>
		/// <param name="_objectMachine"></param>
		/// <param name="_childObjectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ObjectAssociation"/> class.</returns>
		public MetaBuilder.BusinessLogic.ObjectAssociation GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(System.Int32 _cAid, System.Int32 _objectID, System.Int32 _childObjectID, System.String _objectMachine, System.String _childObjectMachine, int start, int pageLength, out int count)
		{
			return GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(null, _cAid, _objectID, _childObjectID, _objectMachine, _childObjectMachine, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ObjectRelationship index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_cAid"></param>
		/// <param name="_objectID"></param>
		/// <param name="_childObjectID"></param>
		/// <param name="_objectMachine"></param>
		/// <param name="_childObjectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ObjectAssociation"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.ObjectAssociation GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(TransactionManager transactionManager, System.Int32 _cAid, System.Int32 _objectID, System.Int32 _childObjectID, System.String _objectMachine, System.String _childObjectMachine, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a TList&lt;ObjectAssociation&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="TList&lt;ObjectAssociation&gt;"/></returns>
		public static TList<ObjectAssociation> Fill(IDataReader reader, TList<ObjectAssociation> rows, int start, int pageLength)
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
				
				MetaBuilder.BusinessLogic.ObjectAssociation c = null;
				if (useEntityFactory)
				{
					key = new System.Text.StringBuilder("ObjectAssociation")
					.Append("|").Append((System.Int32)reader[((int)ObjectAssociationColumn.CAid - 1)])
					.Append("|").Append((System.Int32)reader[((int)ObjectAssociationColumn.ObjectID - 1)])
					.Append("|").Append((System.Int32)reader[((int)ObjectAssociationColumn.ChildObjectID - 1)])
					.Append("|").Append((System.String)reader[((int)ObjectAssociationColumn.ObjectMachine - 1)])
					.Append("|").Append((System.String)reader[((int)ObjectAssociationColumn.ChildObjectMachine - 1)]).ToString();
					c = EntityManager.LocateOrCreate<ObjectAssociation>(
					key.ToString(), // EntityTrackingKey
					"ObjectAssociation",  //Creational Type
					entityCreationFactoryType,  //Factory used to create entity
					enableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.ObjectAssociation();
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
					c.CAid = (System.Int32)reader[((int)ObjectAssociationColumn.CAid - 1)];
					c.OriginalCAid = c.CAid;
					c.ObjectID = (System.Int32)reader[((int)ObjectAssociationColumn.ObjectID - 1)];
					c.OriginalObjectID = c.ObjectID;
					c.ChildObjectID = (System.Int32)reader[((int)ObjectAssociationColumn.ChildObjectID - 1)];
					c.OriginalChildObjectID = c.ChildObjectID;
					c.Series = (System.Int32)reader[((int)ObjectAssociationColumn.Series - 1)];
					c.ObjectMachine = (System.String)reader[((int)ObjectAssociationColumn.ObjectMachine - 1)];
					c.OriginalObjectMachine = c.ObjectMachine;
					c.ChildObjectMachine = (System.String)reader[((int)ObjectAssociationColumn.ChildObjectMachine - 1)];
					c.OriginalChildObjectMachine = c.ChildObjectMachine;
					c.VCStatusID = (System.Int32)reader[((int)ObjectAssociationColumn.VCStatusID - 1)];
					c.VCMachineID = (reader.IsDBNull(((int)ObjectAssociationColumn.VCMachineID - 1)))?null:(System.String)reader[((int)ObjectAssociationColumn.VCMachineID - 1)];
					c.Machine = (reader.IsDBNull(((int)ObjectAssociationColumn.Machine - 1)))?null:(System.String)reader[((int)ObjectAssociationColumn.Machine - 1)];
					c.EntityTrackingKey = key;
					c.AcceptChanges();
					c.SuppressEntityEvents = false;
				}
				rows.Add(c);
			}
		return rows;
		}		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.ObjectAssociation"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.ObjectAssociation"/> object to refresh.</param>
		public static void RefreshEntity(IDataReader reader, MetaBuilder.BusinessLogic.ObjectAssociation entity)
		{
			if (!reader.Read()) return;
			
			entity.CAid = (System.Int32)reader[((int)ObjectAssociationColumn.CAid - 1)];
			entity.OriginalCAid = (System.Int32)reader["CAid"];
			entity.ObjectID = (System.Int32)reader[((int)ObjectAssociationColumn.ObjectID - 1)];
			entity.OriginalObjectID = (System.Int32)reader["ObjectID"];
			entity.ChildObjectID = (System.Int32)reader[((int)ObjectAssociationColumn.ChildObjectID - 1)];
			entity.OriginalChildObjectID = (System.Int32)reader["ChildObjectID"];
			entity.Series = (System.Int32)reader[((int)ObjectAssociationColumn.Series - 1)];
			entity.ObjectMachine = (System.String)reader[((int)ObjectAssociationColumn.ObjectMachine - 1)];
			entity.OriginalObjectMachine = (System.String)reader["ObjectMachine"];
			entity.ChildObjectMachine = (System.String)reader[((int)ObjectAssociationColumn.ChildObjectMachine - 1)];
			entity.OriginalChildObjectMachine = (System.String)reader["ChildObjectMachine"];
			entity.VCStatusID = (System.Int32)reader[((int)ObjectAssociationColumn.VCStatusID - 1)];
			entity.VCMachineID = (reader.IsDBNull(((int)ObjectAssociationColumn.VCMachineID - 1)))?null:(System.String)reader[((int)ObjectAssociationColumn.VCMachineID - 1)];
			entity.Machine = (reader.IsDBNull(((int)ObjectAssociationColumn.Machine - 1)))?null:(System.String)reader[((int)ObjectAssociationColumn.Machine - 1)];
			entity.AcceptChanges();
		}
		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.ObjectAssociation"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.ObjectAssociation"/> object.</param>
		public static void RefreshEntity(DataSet dataSet, MetaBuilder.BusinessLogic.ObjectAssociation entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.CAid = (System.Int32)dataRow["CAid"];
			entity.OriginalCAid = (System.Int32)dataRow["CAid"];
			entity.ObjectID = (System.Int32)dataRow["ObjectID"];
			entity.OriginalObjectID = (System.Int32)dataRow["ObjectID"];
			entity.ChildObjectID = (System.Int32)dataRow["ChildObjectID"];
			entity.OriginalChildObjectID = (System.Int32)dataRow["ChildObjectID"];
			entity.Series = (System.Int32)dataRow["Series"];
			entity.ObjectMachine = (System.String)dataRow["ObjectMachine"];
			entity.OriginalObjectMachine = (System.String)dataRow["ObjectMachine"];
			entity.ChildObjectMachine = (System.String)dataRow["ChildObjectMachine"];
			entity.OriginalChildObjectMachine = (System.String)dataRow["ChildObjectMachine"];
			entity.VCStatusID = (System.Int32)dataRow["VCStatusID"];
			entity.VCMachineID = Convert.IsDBNull(dataRow["VCMachineID"]) ? null : (System.String)dataRow["VCMachineID"];
			entity.Machine = Convert.IsDBNull(dataRow["Machine"]) ? null : (System.String)dataRow["Machine"];
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
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.ObjectAssociation"/> object to load.</param>
		/// <param name="deep">Boolean. A flag that indicates whether to recursively save all Property Collection that are descendants of this instance. If True, saves the complete object graph below this object. If False, saves this object only. </param>
		/// <param name="deepLoadType">DeepLoadType Enumeration to Include/Exclude object property collections from Load.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.ObjectAssociation Property Collection Type Array To Include or Exclude from Load</param>
		/// <param name="innerList">A collection of child types for easy access.</param>
	    /// <exception cref="ArgumentNullException">entity or childTypes is null.</exception>
	    /// <exception cref="ArgumentException">deepLoadType has invalid value.</exception>
		public override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.ObjectAssociation entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, DeepSession innerList)
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

			#region CAidSource	
			if (CanDeepLoad(entity, "ClassAssociation|CAidSource", deepLoadType, innerList) 
				&& entity.CAidSource == null)
			{
				object[] pkItems = new object[1];
				pkItems[0] = entity.CAid;
				ClassAssociation tmpEntity = EntityManager.LocateEntity<ClassAssociation>(EntityLocator.ConstructKeyFromPkItems(typeof(ClassAssociation), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.CAidSource = tmpEntity;
				else
					entity.CAidSource = DataRepository.ClassAssociationProvider.GetByCAid(transactionManager, entity.CAid);		
				
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'CAidSource' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.CAidSource != null)
				{
					innerList.SkipChildren = true;
					DataRepository.ClassAssociationProvider.DeepLoad(transactionManager, entity.CAidSource, deep, deepLoadType, childTypes, innerList);
					innerList.SkipChildren = false;
				}
					
			}
			#endregion CAidSource

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
			
			//used to hold DeepLoad method delegates and fire after all the local children have been loaded.
			Dictionary<string, KeyValuePair<Delegate, object>> deepHandles = new Dictionary<string, KeyValuePair<Delegate, object>>();
			// Deep load child collections  - Call GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine methods when available
			
			#region GraphFileAssociationCollection
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<GraphFileAssociation>|GraphFileAssociationCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'GraphFileAssociationCollection' loaded. key " + entity.EntityTrackingKey);
				#endif 

				entity.GraphFileAssociationCollection = DataRepository.GraphFileAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(transactionManager, entity.CAid, entity.ObjectID, entity.ChildObjectID, entity.ObjectMachine, entity.ChildObjectMachine);

				if (deep && entity.GraphFileAssociationCollection.Count > 0)
				{
					deepHandles.Add("GraphFileAssociationCollection",
						new KeyValuePair<Delegate, object>((DeepLoadHandle<GraphFileAssociation>) DataRepository.GraphFileAssociationProvider.DeepLoad,
						new object[] { transactionManager, entity.GraphFileAssociationCollection, deep, deepLoadType, childTypes, innerList }
					));
				}
			}		
			#endregion 
			
			
			#region GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileAssociation
			// RelationshipType.ManyToMany
			if (CanDeepLoad(entity, "List<GraphFile>|GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileAssociation", deepLoadType, innerList))
			{
				entity.GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileAssociation = DataRepository.GraphFileProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation(transactionManager, entity.CAid, entity.ObjectID, entity.ChildObjectID, entity.ObjectMachine, entity.ChildObjectMachine);			 
		
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileAssociation' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileAssociation != null)
				{
					deepHandles.Add("GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileAssociation",
						new KeyValuePair<Delegate, object>((DeepLoadHandle< GraphFile >) DataRepository.GraphFileProvider.DeepLoad,
						new object[] { transactionManager, entity.GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileAssociation, deep, deepLoadType, childTypes, innerList }
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
		/// Deep Save the entire object graph of the MetaBuilder.BusinessLogic.ObjectAssociation object with criteria based of the child 
		/// Type property array and DeepSaveType.
		/// </summary>
		/// <param name="transactionManager">The transaction manager.</param>
		/// <param name="entity">MetaBuilder.BusinessLogic.ObjectAssociation instance</param>
		/// <param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.ObjectAssociation Property Collection Type Array To Include or Exclude from Save</param>
		/// <param name="innerList">A Hashtable of child types for easy access.</param>
		public override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.ObjectAssociation entity, DeepSaveType deepSaveType, System.Type[] childTypes, DeepSession innerList)
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
			
			#region CAidSource
			if (CanDeepSave(entity, "ClassAssociation|CAidSource", deepSaveType, innerList) 
				&& entity.CAidSource != null)
			{
				DataRepository.ClassAssociationProvider.Save(transactionManager, entity.CAidSource);
				entity.CAid = entity.CAidSource.CAid;
			}
			#endregion 
			
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
			#endregion Composite Parent Properties

			// Save Root Entity through Provider
			if (!entity.IsDeleted)
				this.Save(transactionManager, entity);
			
			//used to hold DeepSave method delegates and fire after all the local children have been saved.
			Dictionary<string, KeyValuePair<Delegate, object>> deepHandles = new Dictionary<string, KeyValuePair<Delegate, object>>();

			#region GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileAssociation>
			if (CanDeepSave(entity.GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileAssociation, "List<GraphFile>|GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileAssociation", deepSaveType, innerList))
			{
				if (entity.GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileAssociation.Count > 0 || entity.GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileAssociation.DeletedItems.Count > 0)
				{
					DataRepository.GraphFileProvider.Save(transactionManager, entity.GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileAssociation); 
					deepHandles.Add("GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileAssociation",
						new KeyValuePair<Delegate, object>((DeepSaveHandle<GraphFile>) DataRepository.GraphFileProvider.DeepSave,
						new object[] { transactionManager, entity.GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileAssociation, deepSaveType, childTypes, innerList }
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
						if(child.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineSource != null)
						{
								child.CAid = child.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineSource.CAid;
								child.ObjectID = child.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineSource.ObjectID;
								child.ChildObjectID = child.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineSource.ChildObjectID;
								child.ObjectMachine = child.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineSource.ObjectMachine;
								child.ChildObjectMachine = child.CAidObjectIDChildObjectIDObjectMachineChildObjectMachineSource.ChildObjectMachine;
						}

						if(child.GraphFileIDGraphFileMachineSource != null)
						{
								child.GraphFileID = child.GraphFileIDGraphFileMachineSource.pkid;
								child.GraphFileMachine = child.GraphFileIDGraphFileMachineSource.Machine;
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
	
	#region ObjectAssociationChildEntityTypes
	
	///<summary>
	/// Enumeration used to expose the different child entity types 
	/// for child properties in <c>MetaBuilder.BusinessLogic.ObjectAssociation</c>
	///</summary>
	public enum ObjectAssociationChildEntityTypes
	{
		
		///<summary>
		/// Composite Property for <c>VCStatus</c> at VCStatusIDSource
		///</summary>
		[ChildEntityType(typeof(VCStatus))]
		VCStatus,
			
		///<summary>
		/// Composite Property for <c>ClassAssociation</c> at CAidSource
		///</summary>
		[ChildEntityType(typeof(ClassAssociation))]
		ClassAssociation,
			
		///<summary>
		/// Composite Property for <c>MetaObject</c> at ObjectIDObjectMachineSource
		///</summary>
		[ChildEntityType(typeof(MetaObject))]
		MetaObject,
	
		///<summary>
		/// Collection of <c>ObjectAssociation</c> as OneToMany for GraphFileAssociationCollection
		///</summary>
		[ChildEntityType(typeof(TList<GraphFileAssociation>))]
		GraphFileAssociationCollection,

		///<summary>
		/// Collection of <c>ObjectAssociation</c> as ManyToMany for GraphFileCollection_From_GraphFileAssociation
		///</summary>
		[ChildEntityType(typeof(TList<GraphFile>))]
		GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileAssociation,
	}
	
	#endregion ObjectAssociationChildEntityTypes
	
	#region ObjectAssociationFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;ObjectAssociationColumn&gt;"/> class
	/// that is used exclusively with a <see cref="ObjectAssociation"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class ObjectAssociationFilterBuilder : SqlFilterBuilder<ObjectAssociationColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the ObjectAssociationFilterBuilder class.
		/// </summary>
		public ObjectAssociationFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the ObjectAssociationFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public ObjectAssociationFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the ObjectAssociationFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public ObjectAssociationFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion ObjectAssociationFilterBuilder
	
	#region ObjectAssociationParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;ObjectAssociationColumn&gt;"/> class
	/// that is used exclusively with a <see cref="ObjectAssociation"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class ObjectAssociationParameterBuilder : ParameterizedSqlFilterBuilder<ObjectAssociationColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the ObjectAssociationParameterBuilder class.
		/// </summary>
		public ObjectAssociationParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the ObjectAssociationParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public ObjectAssociationParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the ObjectAssociationParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public ObjectAssociationParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion ObjectAssociationParameterBuilder
	
	#region ObjectAssociationSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;ObjectAssociationColumn&gt;"/> class
	/// that is used exclusively with a <see cref="ObjectAssociation"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class ObjectAssociationSortBuilder : SqlSortBuilder<ObjectAssociationColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the ObjectAssociationSqlSortBuilder class.
		/// </summary>
		public ObjectAssociationSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion ObjectAssociationSortBuilder
	
} // end namespace
