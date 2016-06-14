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
		/// <param name="_fieldID"></param>
		/// <returns>Returns a typed collection of MetaObject objects.</returns>
		public TList<MetaObject> GetByFieldIDFromObjectFieldValue(System.Int32 _fieldID)
		{
			int count = -1;
			return GetByFieldIDFromObjectFieldValue(null,_fieldID, 0, int.MaxValue, out count);
			
		}
		
		/// <summary>
		///		Gets MetaBuilder.BusinessLogic.MetaObject objects from the datasource by FieldID in the
		///		ObjectFieldValue table. Table MetaObject is related to table Field
		///		through the (M:N) relationship defined in the ObjectFieldValue table.
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_fieldID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of MetaObject objects.</returns>
		public TList<MetaObject> GetByFieldIDFromObjectFieldValue(System.Int32 _fieldID, int start, int pageLength)
		{
			int count = -1;
			return GetByFieldIDFromObjectFieldValue(null, _fieldID, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets MetaObject objects from the datasource by FieldID in the
		///		ObjectFieldValue table. Table MetaObject is related to table Field
		///		through the (M:N) relationship defined in the ObjectFieldValue table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_fieldID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaObject objects.</returns>
		public TList<MetaObject> GetByFieldIDFromObjectFieldValue(TransactionManager transactionManager, System.Int32 _fieldID)
		{
			int count = -1;
			return GetByFieldIDFromObjectFieldValue(transactionManager, _fieldID, 0, int.MaxValue, out count);
		}
		
		
		/// <summary>
		///		Gets MetaObject objects from the datasource by FieldID in the
		///		ObjectFieldValue table. Table MetaObject is related to table Field
		///		through the (M:N) relationship defined in the ObjectFieldValue table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_fieldID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaObject objects.</returns>
		public TList<MetaObject> GetByFieldIDFromObjectFieldValue(TransactionManager transactionManager, System.Int32 _fieldID,int start, int pageLength)
		{
			int count = -1;
			return GetByFieldIDFromObjectFieldValue(transactionManager, _fieldID, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets MetaObject objects from the datasource by FieldID in the
		///		ObjectFieldValue table. Table MetaObject is related to table Field
		///		through the (M:N) relationship defined in the ObjectFieldValue table.
		/// </summary>
		/// <param name="_fieldID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaObject objects.</returns>
		public TList<MetaObject> GetByFieldIDFromObjectFieldValue(System.Int32 _fieldID,int start, int pageLength, out int count)
		{
			
			return GetByFieldIDFromObjectFieldValue(null, _fieldID, start, pageLength, out count);
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
		/// <param name="_fieldID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of MetaObject objects.</returns>
		public abstract TList<MetaObject> GetByFieldIDFromObjectFieldValue(TransactionManager transactionManager,System.Int32 _fieldID, int start, int pageLength, out int count);
		
		#endregion GetByFieldIDFromObjectFieldValue
		
		#region GetByCAidFromObjectAssociation
		
		/// <summary>
		///		Gets MetaObject objects from the datasource by CAid in the
		///		ObjectAssociation table. Table MetaObject is related to table ClassAssociation
		///		through the (M:N) relationship defined in the ObjectAssociation table.
		/// </summary>
		/// <param name="_cAid"></param>
		/// <returns>Returns a typed collection of MetaObject objects.</returns>
		public TList<MetaObject> GetByCAidFromObjectAssociation(System.Int32 _cAid)
		{
			int count = -1;
			return GetByCAidFromObjectAssociation(null,_cAid, 0, int.MaxValue, out count);
			
		}
		
		/// <summary>
		///		Gets MetaBuilder.BusinessLogic.MetaObject objects from the datasource by CAid in the
		///		ObjectAssociation table. Table MetaObject is related to table ClassAssociation
		///		through the (M:N) relationship defined in the ObjectAssociation table.
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_cAid"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of MetaObject objects.</returns>
		public TList<MetaObject> GetByCAidFromObjectAssociation(System.Int32 _cAid, int start, int pageLength)
		{
			int count = -1;
			return GetByCAidFromObjectAssociation(null, _cAid, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets MetaObject objects from the datasource by CAid in the
		///		ObjectAssociation table. Table MetaObject is related to table ClassAssociation
		///		through the (M:N) relationship defined in the ObjectAssociation table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_cAid"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaObject objects.</returns>
		public TList<MetaObject> GetByCAidFromObjectAssociation(TransactionManager transactionManager, System.Int32 _cAid)
		{
			int count = -1;
			return GetByCAidFromObjectAssociation(transactionManager, _cAid, 0, int.MaxValue, out count);
		}
		
		
		/// <summary>
		///		Gets MetaObject objects from the datasource by CAid in the
		///		ObjectAssociation table. Table MetaObject is related to table ClassAssociation
		///		through the (M:N) relationship defined in the ObjectAssociation table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_cAid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaObject objects.</returns>
		public TList<MetaObject> GetByCAidFromObjectAssociation(TransactionManager transactionManager, System.Int32 _cAid,int start, int pageLength)
		{
			int count = -1;
			return GetByCAidFromObjectAssociation(transactionManager, _cAid, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets MetaObject objects from the datasource by CAid in the
		///		ObjectAssociation table. Table MetaObject is related to table ClassAssociation
		///		through the (M:N) relationship defined in the ObjectAssociation table.
		/// </summary>
		/// <param name="_cAid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaObject objects.</returns>
		public TList<MetaObject> GetByCAidFromObjectAssociation(System.Int32 _cAid,int start, int pageLength, out int count)
		{
			
			return GetByCAidFromObjectAssociation(null, _cAid, start, pageLength, out count);
		}


		/// <summary>
		///		Gets MetaObject objects from the datasource by CAid in the
		///		ObjectAssociation table. Table MetaObject is related to table ClassAssociation
		///		through the (M:N) relationship defined in the ObjectAssociation table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <param name="_cAid"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of MetaObject objects.</returns>
		public abstract TList<MetaObject> GetByCAidFromObjectAssociation(TransactionManager transactionManager,System.Int32 _cAid, int start, int pageLength, out int count);
		
		#endregion GetByCAidFromObjectAssociation
		
		#region GetByChildObjectIDChildObjectMachineFromObjectAssociation
		
		/// <summary>
		///		Gets MetaObject objects from the datasource by ChildObjectID in the
		///		ObjectAssociation table. Table MetaObject is related to table MetaObject
		///		through the (M:N) relationship defined in the ObjectAssociation table.
		/// </summary>
		/// <param name="_childObjectID"></param>
		/// <param name="_childObjectMachine"></param>
		/// <returns>Returns a typed collection of MetaObject objects.</returns>
		public TList<MetaObject> GetByChildObjectIDChildObjectMachineFromObjectAssociation(System.Int32 _childObjectID, System.String _childObjectMachine)
		{
			int count = -1;
			return GetByChildObjectIDChildObjectMachineFromObjectAssociation(null,_childObjectID, _childObjectMachine, 0, int.MaxValue, out count);
			
		}
		
		/// <summary>
		///		Gets MetaBuilder.BusinessLogic.MetaObject objects from the datasource by ChildObjectID in the
		///		ObjectAssociation table. Table MetaObject is related to table MetaObject
		///		through the (M:N) relationship defined in the ObjectAssociation table.
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_childObjectID"></param>
		/// <param name="_childObjectMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of MetaObject objects.</returns>
		public TList<MetaObject> GetByChildObjectIDChildObjectMachineFromObjectAssociation(System.Int32 _childObjectID, System.String _childObjectMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByChildObjectIDChildObjectMachineFromObjectAssociation(null, _childObjectID, _childObjectMachine, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets MetaObject objects from the datasource by ChildObjectID in the
		///		ObjectAssociation table. Table MetaObject is related to table MetaObject
		///		through the (M:N) relationship defined in the ObjectAssociation table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_childObjectID"></param>
		/// <param name="_childObjectMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaObject objects.</returns>
		public TList<MetaObject> GetByChildObjectIDChildObjectMachineFromObjectAssociation(TransactionManager transactionManager, System.Int32 _childObjectID, System.String _childObjectMachine)
		{
			int count = -1;
			return GetByChildObjectIDChildObjectMachineFromObjectAssociation(transactionManager, _childObjectID, _childObjectMachine, 0, int.MaxValue, out count);
		}
		
		
		/// <summary>
		///		Gets MetaObject objects from the datasource by ChildObjectID in the
		///		ObjectAssociation table. Table MetaObject is related to table MetaObject
		///		through the (M:N) relationship defined in the ObjectAssociation table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_childObjectID"></param>
		/// <param name="_childObjectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaObject objects.</returns>
		public TList<MetaObject> GetByChildObjectIDChildObjectMachineFromObjectAssociation(TransactionManager transactionManager, System.Int32 _childObjectID, System.String _childObjectMachine,int start, int pageLength)
		{
			int count = -1;
			return GetByChildObjectIDChildObjectMachineFromObjectAssociation(transactionManager, _childObjectID, _childObjectMachine, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets MetaObject objects from the datasource by ChildObjectID in the
		///		ObjectAssociation table. Table MetaObject is related to table MetaObject
		///		through the (M:N) relationship defined in the ObjectAssociation table.
		/// </summary>
		/// <param name="_childObjectID"></param>
		/// <param name="_childObjectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaObject objects.</returns>
		public TList<MetaObject> GetByChildObjectIDChildObjectMachineFromObjectAssociation(System.Int32 _childObjectID, System.String _childObjectMachine,int start, int pageLength, out int count)
		{
			
			return GetByChildObjectIDChildObjectMachineFromObjectAssociation(null, _childObjectID, _childObjectMachine, start, pageLength, out count);
		}


		/// <summary>
		///		Gets MetaObject objects from the datasource by ChildObjectID in the
		///		ObjectAssociation table. Table MetaObject is related to table MetaObject
		///		through the (M:N) relationship defined in the ObjectAssociation table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <param name="_childObjectID"></param>
		/// <param name="_childObjectMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of MetaObject objects.</returns>
		public abstract TList<MetaObject> GetByChildObjectIDChildObjectMachineFromObjectAssociation(TransactionManager transactionManager,System.Int32 _childObjectID, System.String _childObjectMachine, int start, int pageLength, out int count);
		
		#endregion GetByChildObjectIDChildObjectMachineFromObjectAssociation
		
		#region GetByObjectIDObjectMachineFromObjectAssociation
		
		/// <summary>
		///		Gets MetaObject objects from the datasource by ObjectID in the
		///		ObjectAssociation table. Table MetaObject is related to table MetaObject
		///		through the (M:N) relationship defined in the ObjectAssociation table.
		/// </summary>
		/// <param name="_objectID"></param>
		/// <param name="_objectMachine"></param>
		/// <returns>Returns a typed collection of MetaObject objects.</returns>
		public TList<MetaObject> GetByObjectIDObjectMachineFromObjectAssociation(System.Int32 _objectID, System.String _objectMachine)
		{
			int count = -1;
			return GetByObjectIDObjectMachineFromObjectAssociation(null,_objectID, _objectMachine, 0, int.MaxValue, out count);
			
		}
		
		/// <summary>
		///		Gets MetaBuilder.BusinessLogic.MetaObject objects from the datasource by ObjectID in the
		///		ObjectAssociation table. Table MetaObject is related to table MetaObject
		///		through the (M:N) relationship defined in the ObjectAssociation table.
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_objectID"></param>
		/// <param name="_objectMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of MetaObject objects.</returns>
		public TList<MetaObject> GetByObjectIDObjectMachineFromObjectAssociation(System.Int32 _objectID, System.String _objectMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByObjectIDObjectMachineFromObjectAssociation(null, _objectID, _objectMachine, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets MetaObject objects from the datasource by ObjectID in the
		///		ObjectAssociation table. Table MetaObject is related to table MetaObject
		///		through the (M:N) relationship defined in the ObjectAssociation table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_objectID"></param>
		/// <param name="_objectMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaObject objects.</returns>
		public TList<MetaObject> GetByObjectIDObjectMachineFromObjectAssociation(TransactionManager transactionManager, System.Int32 _objectID, System.String _objectMachine)
		{
			int count = -1;
			return GetByObjectIDObjectMachineFromObjectAssociation(transactionManager, _objectID, _objectMachine, 0, int.MaxValue, out count);
		}
		
		
		/// <summary>
		///		Gets MetaObject objects from the datasource by ObjectID in the
		///		ObjectAssociation table. Table MetaObject is related to table MetaObject
		///		through the (M:N) relationship defined in the ObjectAssociation table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_objectID"></param>
		/// <param name="_objectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaObject objects.</returns>
		public TList<MetaObject> GetByObjectIDObjectMachineFromObjectAssociation(TransactionManager transactionManager, System.Int32 _objectID, System.String _objectMachine,int start, int pageLength)
		{
			int count = -1;
			return GetByObjectIDObjectMachineFromObjectAssociation(transactionManager, _objectID, _objectMachine, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets MetaObject objects from the datasource by ObjectID in the
		///		ObjectAssociation table. Table MetaObject is related to table MetaObject
		///		through the (M:N) relationship defined in the ObjectAssociation table.
		/// </summary>
		/// <param name="_objectID"></param>
		/// <param name="_objectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaObject objects.</returns>
		public TList<MetaObject> GetByObjectIDObjectMachineFromObjectAssociation(System.Int32 _objectID, System.String _objectMachine,int start, int pageLength, out int count)
		{
			
			return GetByObjectIDObjectMachineFromObjectAssociation(null, _objectID, _objectMachine, start, pageLength, out count);
		}


		/// <summary>
		///		Gets MetaObject objects from the datasource by ObjectID in the
		///		ObjectAssociation table. Table MetaObject is related to table MetaObject
		///		through the (M:N) relationship defined in the ObjectAssociation table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <param name="_objectID"></param>
		/// <param name="_objectMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of MetaObject objects.</returns>
		public abstract TList<MetaObject> GetByObjectIDObjectMachineFromObjectAssociation(TransactionManager transactionManager,System.Int32 _objectID, System.String _objectMachine, int start, int pageLength, out int count);
		
		#endregion GetByObjectIDObjectMachineFromObjectAssociation
		
		#region GetByGraphFileIDGraphFileMachineFromGraphFileObject
		
		/// <summary>
		///		Gets MetaObject objects from the datasource by GraphFileID in the
		///		GraphFileObject table. Table MetaObject is related to table GraphFile
		///		through the (M:N) relationship defined in the GraphFileObject table.
		/// </summary>
		/// <param name="_graphFileID"></param>
		/// <param name="_graphFileMachine"></param>
		/// <returns>Returns a typed collection of MetaObject objects.</returns>
		public TList<MetaObject> GetByGraphFileIDGraphFileMachineFromGraphFileObject(System.Int32 _graphFileID, System.String _graphFileMachine)
		{
			int count = -1;
			return GetByGraphFileIDGraphFileMachineFromGraphFileObject(null,_graphFileID, _graphFileMachine, 0, int.MaxValue, out count);
			
		}
		
		/// <summary>
		///		Gets MetaBuilder.BusinessLogic.MetaObject objects from the datasource by GraphFileID in the
		///		GraphFileObject table. Table MetaObject is related to table GraphFile
		///		through the (M:N) relationship defined in the GraphFileObject table.
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_graphFileID"></param>
		/// <param name="_graphFileMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of MetaObject objects.</returns>
		public TList<MetaObject> GetByGraphFileIDGraphFileMachineFromGraphFileObject(System.Int32 _graphFileID, System.String _graphFileMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByGraphFileIDGraphFileMachineFromGraphFileObject(null, _graphFileID, _graphFileMachine, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets MetaObject objects from the datasource by GraphFileID in the
		///		GraphFileObject table. Table MetaObject is related to table GraphFile
		///		through the (M:N) relationship defined in the GraphFileObject table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_graphFileID"></param>
		/// <param name="_graphFileMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaObject objects.</returns>
		public TList<MetaObject> GetByGraphFileIDGraphFileMachineFromGraphFileObject(TransactionManager transactionManager, System.Int32 _graphFileID, System.String _graphFileMachine)
		{
			int count = -1;
			return GetByGraphFileIDGraphFileMachineFromGraphFileObject(transactionManager, _graphFileID, _graphFileMachine, 0, int.MaxValue, out count);
		}
		
		
		/// <summary>
		///		Gets MetaObject objects from the datasource by GraphFileID in the
		///		GraphFileObject table. Table MetaObject is related to table GraphFile
		///		through the (M:N) relationship defined in the GraphFileObject table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_graphFileID"></param>
		/// <param name="_graphFileMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaObject objects.</returns>
		public TList<MetaObject> GetByGraphFileIDGraphFileMachineFromGraphFileObject(TransactionManager transactionManager, System.Int32 _graphFileID, System.String _graphFileMachine,int start, int pageLength)
		{
			int count = -1;
			return GetByGraphFileIDGraphFileMachineFromGraphFileObject(transactionManager, _graphFileID, _graphFileMachine, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets MetaObject objects from the datasource by GraphFileID in the
		///		GraphFileObject table. Table MetaObject is related to table GraphFile
		///		through the (M:N) relationship defined in the GraphFileObject table.
		/// </summary>
		/// <param name="_graphFileID"></param>
		/// <param name="_graphFileMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaObject objects.</returns>
		public TList<MetaObject> GetByGraphFileIDGraphFileMachineFromGraphFileObject(System.Int32 _graphFileID, System.String _graphFileMachine,int start, int pageLength, out int count)
		{
			
			return GetByGraphFileIDGraphFileMachineFromGraphFileObject(null, _graphFileID, _graphFileMachine, start, pageLength, out count);
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
		/// <param name="_graphFileID"></param>
		/// <param name="_graphFileMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of MetaObject objects.</returns>
		public abstract TList<MetaObject> GetByGraphFileIDGraphFileMachineFromGraphFileObject(TransactionManager transactionManager,System.Int32 _graphFileID, System.String _graphFileMachine, int start, int pageLength, out int count);
		
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
		/// 	Gets rows from the datasource based on the FK_Object_Class key.
		///		FK_Object_Class Description: 
		/// </summary>
		/// <param name="_class"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public TList<MetaObject> GetByClass(System.String _class)
		{
			int count = -1;
			return GetByClass(_class, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Object_Class key.
		///		FK_Object_Class Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_class"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		/// <remarks></remarks>
		public TList<MetaObject> GetByClass(TransactionManager transactionManager, System.String _class)
		{
			int count = -1;
			return GetByClass(transactionManager, _class, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_Object_Class key.
		///		FK_Object_Class Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_class"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public TList<MetaObject> GetByClass(TransactionManager transactionManager, System.String _class, int start, int pageLength)
		{
			int count = -1;
			return GetByClass(transactionManager, _class, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Object_Class key.
		///		fK_Object_Class Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_class"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public TList<MetaObject> GetByClass(System.String _class, int start, int pageLength)
		{
			int count =  -1;
			return GetByClass(null, _class, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Object_Class key.
		///		fK_Object_Class Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_class"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public TList<MetaObject> GetByClass(System.String _class, int start, int pageLength,out int count)
		{
			return GetByClass(null, _class, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Object_Class key.
		///		FK_Object_Class Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_class"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public abstract TList<MetaObject> GetByClass(TransactionManager transactionManager, System.String _class, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Object_User key.
		///		FK_Object_User Description: 
		/// </summary>
		/// <param name="_userID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public TList<MetaObject> GetByUserID(System.Int32 _userID)
		{
			int count = -1;
			return GetByUserID(_userID, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Object_User key.
		///		FK_Object_User Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_userID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		/// <remarks></remarks>
		public TList<MetaObject> GetByUserID(TransactionManager transactionManager, System.Int32 _userID)
		{
			int count = -1;
			return GetByUserID(transactionManager, _userID, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_Object_User key.
		///		FK_Object_User Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_userID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public TList<MetaObject> GetByUserID(TransactionManager transactionManager, System.Int32 _userID, int start, int pageLength)
		{
			int count = -1;
			return GetByUserID(transactionManager, _userID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Object_User key.
		///		fK_Object_User Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_userID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public TList<MetaObject> GetByUserID(System.Int32 _userID, int start, int pageLength)
		{
			int count =  -1;
			return GetByUserID(null, _userID, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Object_User key.
		///		fK_Object_User Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_userID"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public TList<MetaObject> GetByUserID(System.Int32 _userID, int start, int pageLength,out int count)
		{
			return GetByUserID(null, _userID, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Object_User key.
		///		FK_Object_User Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_userID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public abstract TList<MetaObject> GetByUserID(TransactionManager transactionManager, System.Int32 _userID, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_MetaObject_VCStatus key.
		///		FK_MetaObject_VCStatus Description: 
		/// </summary>
		/// <param name="_VCStatusID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public TList<MetaObject> GetByVCStatusID(System.Int32 _VCStatusID)
		{
			int count = -1;
			return GetByVCStatusID(_VCStatusID, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_MetaObject_VCStatus key.
		///		FK_MetaObject_VCStatus Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_VCStatusID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		/// <remarks></remarks>
		public TList<MetaObject> GetByVCStatusID(TransactionManager transactionManager, System.Int32 _VCStatusID)
		{
			int count = -1;
			return GetByVCStatusID(transactionManager, _VCStatusID, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_MetaObject_VCStatus key.
		///		FK_MetaObject_VCStatus Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_VCStatusID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public TList<MetaObject> GetByVCStatusID(TransactionManager transactionManager, System.Int32 _VCStatusID, int start, int pageLength)
		{
			int count = -1;
			return GetByVCStatusID(transactionManager, _VCStatusID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_MetaObject_VCStatus key.
		///		fK_MetaObject_VCStatus Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_VCStatusID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public TList<MetaObject> GetByVCStatusID(System.Int32 _VCStatusID, int start, int pageLength)
		{
			int count =  -1;
			return GetByVCStatusID(null, _VCStatusID, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_MetaObject_VCStatus key.
		///		fK_MetaObject_VCStatus Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_VCStatusID"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public TList<MetaObject> GetByVCStatusID(System.Int32 _VCStatusID, int start, int pageLength,out int count)
		{
			return GetByVCStatusID(null, _VCStatusID, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_MetaObject_VCStatus key.
		///		FK_MetaObject_VCStatus Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_VCStatusID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public abstract TList<MetaObject> GetByVCStatusID(TransactionManager transactionManager, System.Int32 _VCStatusID, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_MetaObject_Workspace key.
		///		FK_MetaObject_Workspace Description: 
		/// </summary>
		/// <param name="_workspaceName"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public TList<MetaObject> GetByWorkspaceNameWorkspaceTypeId(System.String _workspaceName, System.Int32? _WorkspaceTypeId)
		{
			int count = -1;
			return GetByWorkspaceNameWorkspaceTypeId(_workspaceName, _WorkspaceTypeId, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_MetaObject_Workspace key.
		///		FK_MetaObject_Workspace Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_workspaceName"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		/// <remarks></remarks>
		public TList<MetaObject> GetByWorkspaceNameWorkspaceTypeId(TransactionManager transactionManager, System.String _workspaceName, System.Int32? _WorkspaceTypeId)
		{
			int count = -1;
			return GetByWorkspaceNameWorkspaceTypeId(transactionManager, _workspaceName, _WorkspaceTypeId, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_MetaObject_Workspace key.
		///		FK_MetaObject_Workspace Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_workspaceName"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public TList<MetaObject> GetByWorkspaceNameWorkspaceTypeId(TransactionManager transactionManager, System.String _workspaceName, System.Int32? _WorkspaceTypeId, int start, int pageLength)
		{
			int count = -1;
			return GetByWorkspaceNameWorkspaceTypeId(transactionManager, _workspaceName, _WorkspaceTypeId, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_MetaObject_Workspace key.
		///		fK_MetaObject_Workspace Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_workspaceName"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public TList<MetaObject> GetByWorkspaceNameWorkspaceTypeId(System.String _workspaceName, System.Int32? _WorkspaceTypeId, int start, int pageLength)
		{
			int count =  -1;
			return GetByWorkspaceNameWorkspaceTypeId(null, _workspaceName, _WorkspaceTypeId, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_MetaObject_Workspace key.
		///		fK_MetaObject_Workspace Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_workspaceName"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public TList<MetaObject> GetByWorkspaceNameWorkspaceTypeId(System.String _workspaceName, System.Int32? _WorkspaceTypeId, int start, int pageLength,out int count)
		{
			return GetByWorkspaceNameWorkspaceTypeId(null, _workspaceName, _WorkspaceTypeId, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_MetaObject_Workspace key.
		///		FK_MetaObject_Workspace Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_workspaceName"></param>
		/// <param name="_WorkspaceTypeId"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.MetaObject objects.</returns>
		public abstract TList<MetaObject> GetByWorkspaceNameWorkspaceTypeId(TransactionManager transactionManager, System.String _workspaceName, System.Int32? _WorkspaceTypeId, int start, int pageLength, out int count);
		
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
			return GetBypkidMachine(transactionManager, key.pkid, key.Machine, start, pageLength);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key PK_MetaObject index.
		/// </summary>
		/// <param name="_pkid"></param>
		/// <param name="_machine"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.MetaObject"/> class.</returns>
		public MetaBuilder.BusinessLogic.MetaObject GetBypkidMachine(System.Int32 _pkid, System.String _machine)
		{
			int count = -1;
			return GetBypkidMachine(null,_pkid, _machine, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_MetaObject index.
		/// </summary>
		/// <param name="_pkid"></param>
		/// <param name="_machine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.MetaObject"/> class.</returns>
		public MetaBuilder.BusinessLogic.MetaObject GetBypkidMachine(System.Int32 _pkid, System.String _machine, int start, int pageLength)
		{
			int count = -1;
			return GetBypkidMachine(null, _pkid, _machine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_MetaObject index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_pkid"></param>
		/// <param name="_machine"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.MetaObject"/> class.</returns>
		public MetaBuilder.BusinessLogic.MetaObject GetBypkidMachine(TransactionManager transactionManager, System.Int32 _pkid, System.String _machine)
		{
			int count = -1;
			return GetBypkidMachine(transactionManager, _pkid, _machine, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_MetaObject index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_pkid"></param>
		/// <param name="_machine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.MetaObject"/> class.</returns>
		public MetaBuilder.BusinessLogic.MetaObject GetBypkidMachine(TransactionManager transactionManager, System.Int32 _pkid, System.String _machine, int start, int pageLength)
		{
			int count = -1;
			return GetBypkidMachine(transactionManager, _pkid, _machine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_MetaObject index.
		/// </summary>
		/// <param name="_pkid"></param>
		/// <param name="_machine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.MetaObject"/> class.</returns>
		public MetaBuilder.BusinessLogic.MetaObject GetBypkidMachine(System.Int32 _pkid, System.String _machine, int start, int pageLength, out int count)
		{
			return GetBypkidMachine(null, _pkid, _machine, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_MetaObject index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_pkid"></param>
		/// <param name="_machine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.MetaObject"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.MetaObject GetBypkidMachine(TransactionManager transactionManager, System.Int32 _pkid, System.String _machine, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a TList&lt;MetaObject&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="TList&lt;MetaObject&gt;"/></returns>
		public static TList<MetaObject> Fill(IDataReader reader, TList<MetaObject> rows, int start, int pageLength)
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
				
				MetaBuilder.BusinessLogic.MetaObject c = null;
				if (useEntityFactory)
				{
					key = new System.Text.StringBuilder("MetaObject")
					.Append("|").Append((System.Int32)reader[((int)MetaObjectColumn.pkid - 1)])
					.Append("|").Append((System.String)reader[((int)MetaObjectColumn.Machine - 1)]).ToString();
					c = EntityManager.LocateOrCreate<MetaObject>(
					key.ToString(), // EntityTrackingKey
					"MetaObject",  //Creational Type
					entityCreationFactoryType,  //Factory used to create entity
					enableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.MetaObject();
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
					c.pkid = (System.Int32)reader[((int)MetaObjectColumn.pkid - 1)];
					c.Class = (System.String)reader[((int)MetaObjectColumn.Class - 1)];
					c.WorkspaceName = (System.String)reader[((int)MetaObjectColumn.WorkspaceName - 1)];
					c.UserID = (System.Int32)reader[((int)MetaObjectColumn.UserID - 1)];
					c.Machine = (System.String)reader[((int)MetaObjectColumn.Machine - 1)];
					c.OriginalMachine = c.Machine;
					c.VCStatusID = (System.Int32)reader[((int)MetaObjectColumn.VCStatusID - 1)];
					c.VCMachineID = (reader.IsDBNull(((int)MetaObjectColumn.VCMachineID - 1)))?null:(System.String)reader[((int)MetaObjectColumn.VCMachineID - 1)];
					c.WorkspaceTypeId = (reader.IsDBNull(((int)MetaObjectColumn.WorkspaceTypeId - 1)))?null:(System.Int32?)reader[((int)MetaObjectColumn.WorkspaceTypeId - 1)];
					c.DateCreated = (reader.IsDBNull(((int)MetaObjectColumn.DateCreated - 1)))?null:(System.DateTime?)reader[((int)MetaObjectColumn.DateCreated - 1)];
					c.LastModified = (reader.IsDBNull(((int)MetaObjectColumn.LastModified - 1)))?null:(System.DateTime?)reader[((int)MetaObjectColumn.LastModified - 1)];
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
			
			entity.pkid = (System.Int32)reader[((int)MetaObjectColumn.pkid - 1)];
			entity.Class = (System.String)reader[((int)MetaObjectColumn.Class - 1)];
			entity.WorkspaceName = (System.String)reader[((int)MetaObjectColumn.WorkspaceName - 1)];
			entity.UserID = (System.Int32)reader[((int)MetaObjectColumn.UserID - 1)];
			entity.Machine = (System.String)reader[((int)MetaObjectColumn.Machine - 1)];
			entity.OriginalMachine = (System.String)reader["Machine"];
			entity.VCStatusID = (System.Int32)reader[((int)MetaObjectColumn.VCStatusID - 1)];
			entity.VCMachineID = (reader.IsDBNull(((int)MetaObjectColumn.VCMachineID - 1)))?null:(System.String)reader[((int)MetaObjectColumn.VCMachineID - 1)];
			entity.WorkspaceTypeId = (reader.IsDBNull(((int)MetaObjectColumn.WorkspaceTypeId - 1)))?null:(System.Int32?)reader[((int)MetaObjectColumn.WorkspaceTypeId - 1)];
			entity.DateCreated = (reader.IsDBNull(((int)MetaObjectColumn.DateCreated - 1)))?null:(System.DateTime?)reader[((int)MetaObjectColumn.DateCreated - 1)];
			entity.LastModified = (reader.IsDBNull(((int)MetaObjectColumn.LastModified - 1)))?null:(System.DateTime?)reader[((int)MetaObjectColumn.LastModified - 1)];
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
			
			entity.pkid = (System.Int32)dataRow["pkid"];
			entity.Class = (System.String)dataRow["Class"];
			entity.WorkspaceName = (System.String)dataRow["WorkspaceName"];
			entity.UserID = (System.Int32)dataRow["UserID"];
			entity.Machine = (System.String)dataRow["Machine"];
			entity.OriginalMachine = (System.String)dataRow["Machine"];
			entity.VCStatusID = (System.Int32)dataRow["VCStatusID"];
			entity.VCMachineID = Convert.IsDBNull(dataRow["VCMachineID"]) ? null : (System.String)dataRow["VCMachineID"];
			entity.WorkspaceTypeId = Convert.IsDBNull(dataRow["WorkspaceTypeId"]) ? null : (System.Int32?)dataRow["WorkspaceTypeId"];
			entity.DateCreated = Convert.IsDBNull(dataRow["DateCreated"]) ? null : (System.DateTime?)dataRow["DateCreated"];
			entity.LastModified = Convert.IsDBNull(dataRow["LastModified"]) ? null : (System.DateTime?)dataRow["LastModified"];
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
		public override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.MetaObject entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, DeepSession innerList)
		{
			if(entity == null)
				return;

			#region ClassSource	
			if (CanDeepLoad(entity, "Class|ClassSource", deepLoadType, innerList) 
				&& entity.ClassSource == null)
			{
				object[] pkItems = new object[1];
				pkItems[0] = entity.Class;
				Class tmpEntity = EntityManager.LocateEntity<Class>(EntityLocator.ConstructKeyFromPkItems(typeof(Class), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.ClassSource = tmpEntity;
				else
					entity.ClassSource = DataRepository.ClassProvider.GetByName(transactionManager, entity.Class);		
				
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'ClassSource' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.ClassSource != null)
				{
					innerList.SkipChildren = true;
					DataRepository.ClassProvider.DeepLoad(transactionManager, entity.ClassSource, deep, deepLoadType, childTypes, innerList);
					innerList.SkipChildren = false;
				}
					
			}
			#endregion ClassSource

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

			#region WorkspaceNameWorkspaceTypeIdSource	
			if (CanDeepLoad(entity, "Workspace|WorkspaceNameWorkspaceTypeIdSource", deepLoadType, innerList) 
				&& entity.WorkspaceNameWorkspaceTypeIdSource == null)
			{
				object[] pkItems = new object[2];
				pkItems[0] = entity.WorkspaceName;
				pkItems[1] = (entity.WorkspaceTypeId ?? (int)0);
				Workspace tmpEntity = EntityManager.LocateEntity<Workspace>(EntityLocator.ConstructKeyFromPkItems(typeof(Workspace), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.WorkspaceNameWorkspaceTypeIdSource = tmpEntity;
				else
					entity.WorkspaceNameWorkspaceTypeIdSource = DataRepository.WorkspaceProvider.GetByNameWorkspaceTypeId(transactionManager, entity.WorkspaceName, (entity.WorkspaceTypeId ?? (int)0));		
				
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
			
			#region ObjectAssociationCollectionGetByChildObjectIDChildObjectMachine
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<ObjectAssociation>|ObjectAssociationCollectionGetByChildObjectIDChildObjectMachine", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'ObjectAssociationCollectionGetByChildObjectIDChildObjectMachine' loaded. key " + entity.EntityTrackingKey);
				#endif 

				entity.ObjectAssociationCollectionGetByChildObjectIDChildObjectMachine = DataRepository.ObjectAssociationProvider.GetByChildObjectIDChildObjectMachine(transactionManager, entity.pkid, entity.Machine);

				if (deep && entity.ObjectAssociationCollectionGetByChildObjectIDChildObjectMachine.Count > 0)
				{
					deepHandles.Add("ObjectAssociationCollectionGetByChildObjectIDChildObjectMachine",
						new KeyValuePair<Delegate, object>((DeepLoadHandle<ObjectAssociation>) DataRepository.ObjectAssociationProvider.DeepLoad,
						new object[] { transactionManager, entity.ObjectAssociationCollectionGetByChildObjectIDChildObjectMachine, deep, deepLoadType, childTypes, innerList }
					));
				}
			}		
			#endregion 
			
			
			#region GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileObject
			// RelationshipType.ManyToMany
			if (CanDeepLoad(entity, "List<GraphFile>|GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileObject", deepLoadType, innerList))
			{
				entity.GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileObject = DataRepository.GraphFileProvider.GetByMetaObjectIDMachineIDFromGraphFileObject(transactionManager, entity.pkid, entity.Machine);			 
		
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileObject' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileObject != null)
				{
					deepHandles.Add("GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileObject",
						new KeyValuePair<Delegate, object>((DeepLoadHandle< GraphFile >) DataRepository.GraphFileProvider.DeepLoad,
						new object[] { transactionManager, entity.GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileObject, deep, deepLoadType, childTypes, innerList }
					));
				}
			}
			#endregion
			
			
			
			#region ObjectAssociationCollectionGetByObjectIDObjectMachine
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<ObjectAssociation>|ObjectAssociationCollectionGetByObjectIDObjectMachine", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'ObjectAssociationCollectionGetByObjectIDObjectMachine' loaded. key " + entity.EntityTrackingKey);
				#endif 

				entity.ObjectAssociationCollectionGetByObjectIDObjectMachine = DataRepository.ObjectAssociationProvider.GetByObjectIDObjectMachine(transactionManager, entity.pkid, entity.Machine);

				if (deep && entity.ObjectAssociationCollectionGetByObjectIDObjectMachine.Count > 0)
				{
					deepHandles.Add("ObjectAssociationCollectionGetByObjectIDObjectMachine",
						new KeyValuePair<Delegate, object>((DeepLoadHandle<ObjectAssociation>) DataRepository.ObjectAssociationProvider.DeepLoad,
						new object[] { transactionManager, entity.ObjectAssociationCollectionGetByObjectIDObjectMachine, deep, deepLoadType, childTypes, innerList }
					));
				}
			}		
			#endregion 
			
			
			#region ArtifactCollectionGetByArtifactObjectIDArtefactMachine
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<Artifact>|ArtifactCollectionGetByArtifactObjectIDArtefactMachine", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'ArtifactCollectionGetByArtifactObjectIDArtefactMachine' loaded. key " + entity.EntityTrackingKey);
				#endif 

				entity.ArtifactCollectionGetByArtifactObjectIDArtefactMachine = DataRepository.ArtifactProvider.GetByArtifactObjectIDArtefactMachine(transactionManager, entity.pkid, entity.Machine);

				if (deep && entity.ArtifactCollectionGetByArtifactObjectIDArtefactMachine.Count > 0)
				{
					deepHandles.Add("ArtifactCollectionGetByArtifactObjectIDArtefactMachine",
						new KeyValuePair<Delegate, object>((DeepLoadHandle<Artifact>) DataRepository.ArtifactProvider.DeepLoad,
						new object[] { transactionManager, entity.ArtifactCollectionGetByArtifactObjectIDArtefactMachine, deep, deepLoadType, childTypes, innerList }
					));
				}
			}		
			#endregion 
			
			
			#region ArtifactCollectionGetByObjectIDObjectMachine
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<Artifact>|ArtifactCollectionGetByObjectIDObjectMachine", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'ArtifactCollectionGetByObjectIDObjectMachine' loaded. key " + entity.EntityTrackingKey);
				#endif 

				entity.ArtifactCollectionGetByObjectIDObjectMachine = DataRepository.ArtifactProvider.GetByObjectIDObjectMachine(transactionManager, entity.pkid, entity.Machine);

				if (deep && entity.ArtifactCollectionGetByObjectIDObjectMachine.Count > 0)
				{
					deepHandles.Add("ArtifactCollectionGetByObjectIDObjectMachine",
						new KeyValuePair<Delegate, object>((DeepLoadHandle<Artifact>) DataRepository.ArtifactProvider.DeepLoad,
						new object[] { transactionManager, entity.ArtifactCollectionGetByObjectIDObjectMachine, deep, deepLoadType, childTypes, innerList }
					));
				}
			}		
			#endregion 
			
			
			#region ObjectFieldValueCollection
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<ObjectFieldValue>|ObjectFieldValueCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'ObjectFieldValueCollection' loaded. key " + entity.EntityTrackingKey);
				#endif 

				entity.ObjectFieldValueCollection = DataRepository.ObjectFieldValueProvider.GetByObjectIDMachineID(transactionManager, entity.pkid, entity.Machine);

				if (deep && entity.ObjectFieldValueCollection.Count > 0)
				{
					deepHandles.Add("ObjectFieldValueCollection",
						new KeyValuePair<Delegate, object>((DeepLoadHandle<ObjectFieldValue>) DataRepository.ObjectFieldValueProvider.DeepLoad,
						new object[] { transactionManager, entity.ObjectFieldValueCollection, deep, deepLoadType, childTypes, innerList }
					));
				}
			}		
			#endregion 
			
			
			#region CAidClassAssociationCollection_From_ObjectAssociationChildObjectIDChildObjectMachineFromObjectAssociation
			// RelationshipType.ManyToMany
			if (CanDeepLoad(entity, "List<ClassAssociation>|CAidClassAssociationCollection_From_ObjectAssociationChildObjectIDChildObjectMachineFromObjectAssociation", deepLoadType, innerList))
			{
				entity.CAidClassAssociationCollection_From_ObjectAssociationChildObjectIDChildObjectMachineFromObjectAssociation = DataRepository.ClassAssociationProvider.GetByChildObjectIDChildObjectMachineFromObjectAssociation(transactionManager, entity.pkid, entity.Machine);			 
		
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'CAidClassAssociationCollection_From_ObjectAssociationChildObjectIDChildObjectMachineFromObjectAssociation' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.CAidClassAssociationCollection_From_ObjectAssociationChildObjectIDChildObjectMachineFromObjectAssociation != null)
				{
					deepHandles.Add("CAidClassAssociationCollection_From_ObjectAssociationChildObjectIDChildObjectMachineFromObjectAssociation",
						new KeyValuePair<Delegate, object>((DeepLoadHandle< ClassAssociation >) DataRepository.ClassAssociationProvider.DeepLoad,
						new object[] { transactionManager, entity.CAidClassAssociationCollection_From_ObjectAssociationChildObjectIDChildObjectMachineFromObjectAssociation, deep, deepLoadType, childTypes, innerList }
					));
				}
			}
			#endregion
			
			
			
			#region CAidClassAssociationCollection_From_ObjectAssociationObjectIDObjectMachineFromObjectAssociation
			// RelationshipType.ManyToMany
			if (CanDeepLoad(entity, "List<ClassAssociation>|CAidClassAssociationCollection_From_ObjectAssociationObjectIDObjectMachineFromObjectAssociation", deepLoadType, innerList))
			{
				entity.CAidClassAssociationCollection_From_ObjectAssociationObjectIDObjectMachineFromObjectAssociation = DataRepository.ClassAssociationProvider.GetByObjectIDObjectMachineFromObjectAssociation(transactionManager, entity.pkid, entity.Machine);			 
		
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'CAidClassAssociationCollection_From_ObjectAssociationObjectIDObjectMachineFromObjectAssociation' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.CAidClassAssociationCollection_From_ObjectAssociationObjectIDObjectMachineFromObjectAssociation != null)
				{
					deepHandles.Add("CAidClassAssociationCollection_From_ObjectAssociationObjectIDObjectMachineFromObjectAssociation",
						new KeyValuePair<Delegate, object>((DeepLoadHandle< ClassAssociation >) DataRepository.ClassAssociationProvider.DeepLoad,
						new object[] { transactionManager, entity.CAidClassAssociationCollection_From_ObjectAssociationObjectIDObjectMachineFromObjectAssociation, deep, deepLoadType, childTypes, innerList }
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

				entity.GraphFileObjectCollection = DataRepository.GraphFileObjectProvider.GetByMetaObjectIDMachineID(transactionManager, entity.pkid, entity.Machine);

				if (deep && entity.GraphFileObjectCollection.Count > 0)
				{
					deepHandles.Add("GraphFileObjectCollection",
						new KeyValuePair<Delegate, object>((DeepLoadHandle<GraphFileObject>) DataRepository.GraphFileObjectProvider.DeepLoad,
						new object[] { transactionManager, entity.GraphFileObjectCollection, deep, deepLoadType, childTypes, innerList }
					));
				}
			}		
			#endregion 
			
			
			#region ObjectIDObjectMachineMetaObjectCollection_From_ObjectAssociation
			// RelationshipType.ManyToMany
			if (CanDeepLoad(entity, "List<MetaObject>|ObjectIDObjectMachineMetaObjectCollection_From_ObjectAssociation", deepLoadType, innerList))
			{
				entity.ObjectIDObjectMachineMetaObjectCollection_From_ObjectAssociation = DataRepository.MetaObjectProvider.GetByChildObjectIDChildObjectMachineFromObjectAssociation(transactionManager, entity.pkid, entity.Machine);			 
		
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'ObjectIDObjectMachineMetaObjectCollection_From_ObjectAssociation' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.ObjectIDObjectMachineMetaObjectCollection_From_ObjectAssociation != null)
				{
					deepHandles.Add("ObjectIDObjectMachineMetaObjectCollection_From_ObjectAssociation",
						new KeyValuePair<Delegate, object>((DeepLoadHandle< MetaObject >) DataRepository.MetaObjectProvider.DeepLoad,
						new object[] { transactionManager, entity.ObjectIDObjectMachineMetaObjectCollection_From_ObjectAssociation, deep, deepLoadType, childTypes, innerList }
					));
				}
			}
			#endregion
			
			
			
			#region ArtifactCollectionGetByChildObjectIDChildObjectMachine
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<Artifact>|ArtifactCollectionGetByChildObjectIDChildObjectMachine", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'ArtifactCollectionGetByChildObjectIDChildObjectMachine' loaded. key " + entity.EntityTrackingKey);
				#endif 

				entity.ArtifactCollectionGetByChildObjectIDChildObjectMachine = DataRepository.ArtifactProvider.GetByChildObjectIDChildObjectMachine(transactionManager, entity.pkid, entity.Machine);

				if (deep && entity.ArtifactCollectionGetByChildObjectIDChildObjectMachine.Count > 0)
				{
					deepHandles.Add("ArtifactCollectionGetByChildObjectIDChildObjectMachine",
						new KeyValuePair<Delegate, object>((DeepLoadHandle<Artifact>) DataRepository.ArtifactProvider.DeepLoad,
						new object[] { transactionManager, entity.ArtifactCollectionGetByChildObjectIDChildObjectMachine, deep, deepLoadType, childTypes, innerList }
					));
				}
			}		
			#endregion 
			
			
			#region ChildObjectIDChildObjectMachineMetaObjectCollection_From_ObjectAssociation
			// RelationshipType.ManyToMany
			if (CanDeepLoad(entity, "List<MetaObject>|ChildObjectIDChildObjectMachineMetaObjectCollection_From_ObjectAssociation", deepLoadType, innerList))
			{
				entity.ChildObjectIDChildObjectMachineMetaObjectCollection_From_ObjectAssociation = DataRepository.MetaObjectProvider.GetByObjectIDObjectMachineFromObjectAssociation(transactionManager, entity.pkid, entity.Machine);			 
		
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'ChildObjectIDChildObjectMachineMetaObjectCollection_From_ObjectAssociation' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.ChildObjectIDChildObjectMachineMetaObjectCollection_From_ObjectAssociation != null)
				{
					deepHandles.Add("ChildObjectIDChildObjectMachineMetaObjectCollection_From_ObjectAssociation",
						new KeyValuePair<Delegate, object>((DeepLoadHandle< MetaObject >) DataRepository.MetaObjectProvider.DeepLoad,
						new object[] { transactionManager, entity.ChildObjectIDChildObjectMachineMetaObjectCollection_From_ObjectAssociation, deep, deepLoadType, childTypes, innerList }
					));
				}
			}
			#endregion
			
			
			
			#region FieldIDFieldCollection_From_ObjectFieldValue
			// RelationshipType.ManyToMany
			if (CanDeepLoad(entity, "List<Field>|FieldIDFieldCollection_From_ObjectFieldValue", deepLoadType, innerList))
			{
				entity.FieldIDFieldCollection_From_ObjectFieldValue = DataRepository.FieldProvider.GetByObjectIDMachineIDFromObjectFieldValue(transactionManager, entity.pkid, entity.Machine);			 
		
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'FieldIDFieldCollection_From_ObjectFieldValue' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.FieldIDFieldCollection_From_ObjectFieldValue != null)
				{
					deepHandles.Add("FieldIDFieldCollection_From_ObjectFieldValue",
						new KeyValuePair<Delegate, object>((DeepLoadHandle< Field >) DataRepository.FieldProvider.DeepLoad,
						new object[] { transactionManager, entity.FieldIDFieldCollection_From_ObjectFieldValue, deep, deepLoadType, childTypes, innerList }
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
		/// Deep Save the entire object graph of the MetaBuilder.BusinessLogic.MetaObject object with criteria based of the child 
		/// Type property array and DeepSaveType.
		/// </summary>
		/// <param name="transactionManager">The transaction manager.</param>
		/// <param name="entity">MetaBuilder.BusinessLogic.MetaObject instance</param>
		/// <param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.MetaObject Property Collection Type Array To Include or Exclude from Save</param>
		/// <param name="innerList">A Hashtable of child types for easy access.</param>
		public override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.MetaObject entity, DeepSaveType deepSaveType, System.Type[] childTypes, DeepSession innerList)
		{	
			if (entity == null)
				return false;
							
			#region Composite Parent Properties
			//Save Source Composite Properties, however, don't call deep save on them.  
			//So they only get saved a single level deep.
			
			#region ClassSource
			if (CanDeepSave(entity, "Class|ClassSource", deepSaveType, innerList) 
				&& entity.ClassSource != null)
			{
				DataRepository.ClassProvider.Save(transactionManager, entity.ClassSource);
				entity.Class = entity.ClassSource.Name;
			}
			#endregion 
			
			#region UserIDSource
			if (CanDeepSave(entity, "User|UserIDSource", deepSaveType, innerList) 
				&& entity.UserIDSource != null)
			{
				DataRepository.UserProvider.Save(transactionManager, entity.UserIDSource);
				entity.UserID = entity.UserIDSource.pkid;
			}
			#endregion 
			
			#region VCStatusIDSource
			if (CanDeepSave(entity, "VCStatus|VCStatusIDSource", deepSaveType, innerList) 
				&& entity.VCStatusIDSource != null)
			{
				DataRepository.VCStatusProvider.Save(transactionManager, entity.VCStatusIDSource);
				entity.VCStatusID = entity.VCStatusIDSource.pkid;
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

			#region GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileObject>
			if (CanDeepSave(entity.GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileObject, "List<GraphFile>|GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileObject", deepSaveType, innerList))
			{
				if (entity.GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileObject.Count > 0 || entity.GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileObject.DeletedItems.Count > 0)
				{
					DataRepository.GraphFileProvider.Save(transactionManager, entity.GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileObject); 
					deepHandles.Add("GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileObject",
						new KeyValuePair<Delegate, object>((DeepSaveHandle<GraphFile>) DataRepository.GraphFileProvider.DeepSave,
						new object[] { transactionManager, entity.GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileObject, deepSaveType, childTypes, innerList }
					));
				}
			}
			#endregion 

			#region CAidClassAssociationCollection_From_ObjectAssociationChildObjectIDChildObjectMachineFromObjectAssociation>
			if (CanDeepSave(entity.CAidClassAssociationCollection_From_ObjectAssociationChildObjectIDChildObjectMachineFromObjectAssociation, "List<ClassAssociation>|CAidClassAssociationCollection_From_ObjectAssociationChildObjectIDChildObjectMachineFromObjectAssociation", deepSaveType, innerList))
			{
				if (entity.CAidClassAssociationCollection_From_ObjectAssociationChildObjectIDChildObjectMachineFromObjectAssociation.Count > 0 || entity.CAidClassAssociationCollection_From_ObjectAssociationChildObjectIDChildObjectMachineFromObjectAssociation.DeletedItems.Count > 0)
				{
					DataRepository.ClassAssociationProvider.Save(transactionManager, entity.CAidClassAssociationCollection_From_ObjectAssociationChildObjectIDChildObjectMachineFromObjectAssociation); 
					deepHandles.Add("CAidClassAssociationCollection_From_ObjectAssociationChildObjectIDChildObjectMachineFromObjectAssociation",
						new KeyValuePair<Delegate, object>((DeepSaveHandle<ClassAssociation>) DataRepository.ClassAssociationProvider.DeepSave,
						new object[] { transactionManager, entity.CAidClassAssociationCollection_From_ObjectAssociationChildObjectIDChildObjectMachineFromObjectAssociation, deepSaveType, childTypes, innerList }
					));
				}
			}
			#endregion 

			#region CAidClassAssociationCollection_From_ObjectAssociationObjectIDObjectMachineFromObjectAssociation>
			if (CanDeepSave(entity.CAidClassAssociationCollection_From_ObjectAssociationObjectIDObjectMachineFromObjectAssociation, "List<ClassAssociation>|CAidClassAssociationCollection_From_ObjectAssociationObjectIDObjectMachineFromObjectAssociation", deepSaveType, innerList))
			{
				if (entity.CAidClassAssociationCollection_From_ObjectAssociationObjectIDObjectMachineFromObjectAssociation.Count > 0 || entity.CAidClassAssociationCollection_From_ObjectAssociationObjectIDObjectMachineFromObjectAssociation.DeletedItems.Count > 0)
				{
					DataRepository.ClassAssociationProvider.Save(transactionManager, entity.CAidClassAssociationCollection_From_ObjectAssociationObjectIDObjectMachineFromObjectAssociation); 
					deepHandles.Add("CAidClassAssociationCollection_From_ObjectAssociationObjectIDObjectMachineFromObjectAssociation",
						new KeyValuePair<Delegate, object>((DeepSaveHandle<ClassAssociation>) DataRepository.ClassAssociationProvider.DeepSave,
						new object[] { transactionManager, entity.CAidClassAssociationCollection_From_ObjectAssociationObjectIDObjectMachineFromObjectAssociation, deepSaveType, childTypes, innerList }
					));
				}
			}
			#endregion 

			#region ObjectIDObjectMachineMetaObjectCollection_From_ObjectAssociation>
			if (CanDeepSave(entity.ObjectIDObjectMachineMetaObjectCollection_From_ObjectAssociation, "List<MetaObject>|ObjectIDObjectMachineMetaObjectCollection_From_ObjectAssociation", deepSaveType, innerList))
			{
				if (entity.ObjectIDObjectMachineMetaObjectCollection_From_ObjectAssociation.Count > 0 || entity.ObjectIDObjectMachineMetaObjectCollection_From_ObjectAssociation.DeletedItems.Count > 0)
				{
					DataRepository.MetaObjectProvider.Save(transactionManager, entity.ObjectIDObjectMachineMetaObjectCollection_From_ObjectAssociation); 
					deepHandles.Add("ObjectIDObjectMachineMetaObjectCollection_From_ObjectAssociation",
						new KeyValuePair<Delegate, object>((DeepSaveHandle<MetaObject>) DataRepository.MetaObjectProvider.DeepSave,
						new object[] { transactionManager, entity.ObjectIDObjectMachineMetaObjectCollection_From_ObjectAssociation, deepSaveType, childTypes, innerList }
					));
				}
			}
			#endregion 

			#region ChildObjectIDChildObjectMachineMetaObjectCollection_From_ObjectAssociation>
			if (CanDeepSave(entity.ChildObjectIDChildObjectMachineMetaObjectCollection_From_ObjectAssociation, "List<MetaObject>|ChildObjectIDChildObjectMachineMetaObjectCollection_From_ObjectAssociation", deepSaveType, innerList))
			{
				if (entity.ChildObjectIDChildObjectMachineMetaObjectCollection_From_ObjectAssociation.Count > 0 || entity.ChildObjectIDChildObjectMachineMetaObjectCollection_From_ObjectAssociation.DeletedItems.Count > 0)
				{
					DataRepository.MetaObjectProvider.Save(transactionManager, entity.ChildObjectIDChildObjectMachineMetaObjectCollection_From_ObjectAssociation); 
					deepHandles.Add("ChildObjectIDChildObjectMachineMetaObjectCollection_From_ObjectAssociation",
						new KeyValuePair<Delegate, object>((DeepSaveHandle<MetaObject>) DataRepository.MetaObjectProvider.DeepSave,
						new object[] { transactionManager, entity.ChildObjectIDChildObjectMachineMetaObjectCollection_From_ObjectAssociation, deepSaveType, childTypes, innerList }
					));
				}
			}
			#endregion 

			#region FieldIDFieldCollection_From_ObjectFieldValue>
			if (CanDeepSave(entity.FieldIDFieldCollection_From_ObjectFieldValue, "List<Field>|FieldIDFieldCollection_From_ObjectFieldValue", deepSaveType, innerList))
			{
				if (entity.FieldIDFieldCollection_From_ObjectFieldValue.Count > 0 || entity.FieldIDFieldCollection_From_ObjectFieldValue.DeletedItems.Count > 0)
				{
					DataRepository.FieldProvider.Save(transactionManager, entity.FieldIDFieldCollection_From_ObjectFieldValue); 
					deepHandles.Add("FieldIDFieldCollection_From_ObjectFieldValue",
						new KeyValuePair<Delegate, object>((DeepSaveHandle<Field>) DataRepository.FieldProvider.DeepSave,
						new object[] { transactionManager, entity.FieldIDFieldCollection_From_ObjectFieldValue, deepSaveType, childTypes, innerList }
					));
				}
			}
			#endregion 
	
			#region List<ObjectAssociation>
				if (CanDeepSave(entity.ObjectAssociationCollectionGetByChildObjectIDChildObjectMachine, "List<ObjectAssociation>|ObjectAssociationCollectionGetByChildObjectIDChildObjectMachine", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(ObjectAssociation child in entity.ObjectAssociationCollectionGetByChildObjectIDChildObjectMachine)
					{
						if(child.ChildObjectIDChildObjectMachineSource != null)
						{
								child.ChildObjectID = child.ChildObjectIDChildObjectMachineSource.pkid;
								child.ChildObjectMachine = child.ChildObjectIDChildObjectMachineSource.Machine;
						}

						if(child.CAidSource != null)
						{
								child.CAid = child.CAidSource.CAid;
						}

						if(child.ObjectIDObjectMachineSource != null)
						{
								child.ObjectID = child.ObjectIDObjectMachineSource.pkid;
								child.ObjectMachine = child.ObjectIDObjectMachineSource.Machine;
						}

					}

					if (entity.ObjectAssociationCollectionGetByChildObjectIDChildObjectMachine.Count > 0 || entity.ObjectAssociationCollectionGetByChildObjectIDChildObjectMachine.DeletedItems.Count > 0)
					{
						//DataRepository.ObjectAssociationProvider.Save(transactionManager, entity.ObjectAssociationCollectionGetByChildObjectIDChildObjectMachine);
						
						deepHandles.Add("ObjectAssociationCollectionGetByChildObjectIDChildObjectMachine",
						new KeyValuePair<Delegate, object>((DeepSaveHandle< ObjectAssociation >) DataRepository.ObjectAssociationProvider.DeepSave,
							new object[] { transactionManager, entity.ObjectAssociationCollectionGetByChildObjectIDChildObjectMachine, deepSaveType, childTypes, innerList }
						));
					}
				} 
			#endregion 
				
	
			#region List<ObjectAssociation>
				if (CanDeepSave(entity.ObjectAssociationCollectionGetByObjectIDObjectMachine, "List<ObjectAssociation>|ObjectAssociationCollectionGetByObjectIDObjectMachine", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(ObjectAssociation child in entity.ObjectAssociationCollectionGetByObjectIDObjectMachine)
					{
						if(child.ChildObjectIDChildObjectMachineSource != null)
						{
								child.ChildObjectID = child.ChildObjectIDChildObjectMachineSource.pkid;
								child.ChildObjectMachine = child.ChildObjectIDChildObjectMachineSource.Machine;
						}

						if(child.CAidSource != null)
						{
								child.CAid = child.CAidSource.CAid;
						}

						if(child.ObjectIDObjectMachineSource != null)
						{
								child.ObjectID = child.ObjectIDObjectMachineSource.pkid;
								child.ObjectMachine = child.ObjectIDObjectMachineSource.Machine;
						}

					}

					if (entity.ObjectAssociationCollectionGetByObjectIDObjectMachine.Count > 0 || entity.ObjectAssociationCollectionGetByObjectIDObjectMachine.DeletedItems.Count > 0)
					{
						//DataRepository.ObjectAssociationProvider.Save(transactionManager, entity.ObjectAssociationCollectionGetByObjectIDObjectMachine);
						
						deepHandles.Add("ObjectAssociationCollectionGetByObjectIDObjectMachine",
						new KeyValuePair<Delegate, object>((DeepSaveHandle< ObjectAssociation >) DataRepository.ObjectAssociationProvider.DeepSave,
							new object[] { transactionManager, entity.ObjectAssociationCollectionGetByObjectIDObjectMachine, deepSaveType, childTypes, innerList }
						));
					}
				} 
			#endregion 
				
	
			#region List<Artifact>
				if (CanDeepSave(entity.ArtifactCollectionGetByArtifactObjectIDArtefactMachine, "List<Artifact>|ArtifactCollectionGetByArtifactObjectIDArtefactMachine", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(Artifact child in entity.ArtifactCollectionGetByArtifactObjectIDArtefactMachine)
					{
						if(child.ArtifactObjectIDArtefactMachineSource != null)
						{
							child.ArtifactObjectID = child.ArtifactObjectIDArtefactMachineSource.pkid;
							child.ArtefactMachine = child.ArtifactObjectIDArtefactMachineSource.Machine;
						}
						else
						{
							child.ArtifactObjectID = entity.pkid;
							child.ArtefactMachine = entity.Machine;
						}

					}

					if (entity.ArtifactCollectionGetByArtifactObjectIDArtefactMachine.Count > 0 || entity.ArtifactCollectionGetByArtifactObjectIDArtefactMachine.DeletedItems.Count > 0)
					{
						//DataRepository.ArtifactProvider.Save(transactionManager, entity.ArtifactCollectionGetByArtifactObjectIDArtefactMachine);
						
						deepHandles.Add("ArtifactCollectionGetByArtifactObjectIDArtefactMachine",
						new KeyValuePair<Delegate, object>((DeepSaveHandle< Artifact >) DataRepository.ArtifactProvider.DeepSave,
							new object[] { transactionManager, entity.ArtifactCollectionGetByArtifactObjectIDArtefactMachine, deepSaveType, childTypes, innerList }
						));
					}
				} 
			#endregion 
				
	
			#region List<Artifact>
				if (CanDeepSave(entity.ArtifactCollectionGetByObjectIDObjectMachine, "List<Artifact>|ArtifactCollectionGetByObjectIDObjectMachine", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(Artifact child in entity.ArtifactCollectionGetByObjectIDObjectMachine)
					{
						if(child.ObjectIDObjectMachineSource != null)
						{
							child.ObjectID = child.ObjectIDObjectMachineSource.pkid;
							child.ObjectMachine = child.ObjectIDObjectMachineSource.Machine;
						}
						else
						{
							child.ObjectID = entity.pkid;
							child.ObjectMachine = entity.Machine;
						}

					}

					if (entity.ArtifactCollectionGetByObjectIDObjectMachine.Count > 0 || entity.ArtifactCollectionGetByObjectIDObjectMachine.DeletedItems.Count > 0)
					{
						//DataRepository.ArtifactProvider.Save(transactionManager, entity.ArtifactCollectionGetByObjectIDObjectMachine);
						
						deepHandles.Add("ArtifactCollectionGetByObjectIDObjectMachine",
						new KeyValuePair<Delegate, object>((DeepSaveHandle< Artifact >) DataRepository.ArtifactProvider.DeepSave,
							new object[] { transactionManager, entity.ArtifactCollectionGetByObjectIDObjectMachine, deepSaveType, childTypes, innerList }
						));
					}
				} 
			#endregion 
				
	
			#region List<ObjectFieldValue>
				if (CanDeepSave(entity.ObjectFieldValueCollection, "List<ObjectFieldValue>|ObjectFieldValueCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(ObjectFieldValue child in entity.ObjectFieldValueCollection)
					{
						if(child.ObjectIDMachineIDSource != null)
						{
								child.ObjectID = child.ObjectIDMachineIDSource.pkid;
								child.MachineID = child.ObjectIDMachineIDSource.Machine;
						}

						if(child.FieldIDSource != null)
						{
								child.FieldID = child.FieldIDSource.pkid;
						}

					}

					if (entity.ObjectFieldValueCollection.Count > 0 || entity.ObjectFieldValueCollection.DeletedItems.Count > 0)
					{
						//DataRepository.ObjectFieldValueProvider.Save(transactionManager, entity.ObjectFieldValueCollection);
						
						deepHandles.Add("ObjectFieldValueCollection",
						new KeyValuePair<Delegate, object>((DeepSaveHandle< ObjectFieldValue >) DataRepository.ObjectFieldValueProvider.DeepSave,
							new object[] { transactionManager, entity.ObjectFieldValueCollection, deepSaveType, childTypes, innerList }
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
						if(child.MetaObjectIDMachineIDSource != null)
						{
								child.MetaObjectID = child.MetaObjectIDMachineIDSource.pkid;
								child.MachineID = child.MetaObjectIDMachineIDSource.Machine;
						}

						if(child.GraphFileIDGraphFileMachineSource != null)
						{
								child.GraphFileID = child.GraphFileIDGraphFileMachineSource.pkid;
								child.GraphFileMachine = child.GraphFileIDGraphFileMachineSource.Machine;
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
				
	
			#region List<Artifact>
				if (CanDeepSave(entity.ArtifactCollectionGetByChildObjectIDChildObjectMachine, "List<Artifact>|ArtifactCollectionGetByChildObjectIDChildObjectMachine", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(Artifact child in entity.ArtifactCollectionGetByChildObjectIDChildObjectMachine)
					{
						if(child.ChildObjectIDChildObjectMachineSource != null)
						{
							child.ChildObjectID = child.ChildObjectIDChildObjectMachineSource.pkid;
							child.ChildObjectMachine = child.ChildObjectIDChildObjectMachineSource.Machine;
						}
						else
						{
							child.ChildObjectID = entity.pkid;
							child.ChildObjectMachine = entity.Machine;
						}

					}

					if (entity.ArtifactCollectionGetByChildObjectIDChildObjectMachine.Count > 0 || entity.ArtifactCollectionGetByChildObjectIDChildObjectMachine.DeletedItems.Count > 0)
					{
						//DataRepository.ArtifactProvider.Save(transactionManager, entity.ArtifactCollectionGetByChildObjectIDChildObjectMachine);
						
						deepHandles.Add("ArtifactCollectionGetByChildObjectIDChildObjectMachine",
						new KeyValuePair<Delegate, object>((DeepSaveHandle< Artifact >) DataRepository.ArtifactProvider.DeepSave,
							new object[] { transactionManager, entity.ArtifactCollectionGetByChildObjectIDChildObjectMachine, deepSaveType, childTypes, innerList }
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
		/// Composite Property for <c>Workspace</c> at WorkspaceNameWorkspaceTypeIdSource
		///</summary>
		[ChildEntityType(typeof(Workspace))]
		Workspace,
	
		///<summary>
		/// Collection of <c>MetaObject</c> as OneToMany for ObjectAssociationCollection
		///</summary>
		[ChildEntityType(typeof(TList<ObjectAssociation>))]
		ObjectAssociationCollectionGetByChildObjectIDChildObjectMachine,

		///<summary>
		/// Collection of <c>MetaObject</c> as ManyToMany for GraphFileCollection_From_GraphFileObject
		///</summary>
		[ChildEntityType(typeof(TList<GraphFile>))]
		GraphFileIDGraphFileMachineGraphFileCollection_From_GraphFileObject,

		///<summary>
		/// Collection of <c>MetaObject</c> as OneToMany for ObjectAssociationCollection
		///</summary>
		[ChildEntityType(typeof(TList<ObjectAssociation>))]
		ObjectAssociationCollectionGetByObjectIDObjectMachine,

		///<summary>
		/// Collection of <c>MetaObject</c> as OneToMany for ArtifactCollection
		///</summary>
		[ChildEntityType(typeof(TList<Artifact>))]
		ArtifactCollectionGetByArtifactObjectIDArtefactMachine,

		///<summary>
		/// Collection of <c>MetaObject</c> as OneToMany for ArtifactCollection
		///</summary>
		[ChildEntityType(typeof(TList<Artifact>))]
		ArtifactCollectionGetByObjectIDObjectMachine,

		///<summary>
		/// Collection of <c>MetaObject</c> as OneToMany for ObjectFieldValueCollection
		///</summary>
		[ChildEntityType(typeof(TList<ObjectFieldValue>))]
		ObjectFieldValueCollection,

		///<summary>
		/// Collection of <c>MetaObject</c> as ManyToMany for ClassAssociationCollection_From_ObjectAssociation
		///</summary>
		[ChildEntityType(typeof(TList<ClassAssociation>))]
		CAidClassAssociationCollection_From_ObjectAssociationChildObjectIDChildObjectMachineFromObjectAssociation,

		///<summary>
		/// Collection of <c>MetaObject</c> as ManyToMany for ClassAssociationCollection_From_ObjectAssociation
		///</summary>
		[ChildEntityType(typeof(TList<ClassAssociation>))]
		CAidClassAssociationCollection_From_ObjectAssociationObjectIDObjectMachineFromObjectAssociation,

		///<summary>
		/// Collection of <c>MetaObject</c> as OneToMany for GraphFileObjectCollection
		///</summary>
		[ChildEntityType(typeof(TList<GraphFileObject>))]
		GraphFileObjectCollection,

		///<summary>
		/// Collection of <c>MetaObject</c> as ManyToMany for MetaObjectCollection_From_ObjectAssociation
		///</summary>
		[ChildEntityType(typeof(TList<MetaObject>))]
		ObjectIDObjectMachineMetaObjectCollection_From_ObjectAssociation,

		///<summary>
		/// Collection of <c>MetaObject</c> as OneToMany for ArtifactCollection
		///</summary>
		[ChildEntityType(typeof(TList<Artifact>))]
		ArtifactCollectionGetByChildObjectIDChildObjectMachine,

		///<summary>
		/// Collection of <c>MetaObject</c> as ManyToMany for MetaObjectCollection_From_ObjectAssociation
		///</summary>
		[ChildEntityType(typeof(TList<MetaObject>))]
		ChildObjectIDChildObjectMachineMetaObjectCollection_From_ObjectAssociation,

		///<summary>
		/// Collection of <c>MetaObject</c> as ManyToMany for FieldCollection_From_ObjectFieldValue
		///</summary>
		[ChildEntityType(typeof(TList<Field>))]
		FieldIDFieldCollection_From_ObjectFieldValue,
	}
	
	#endregion MetaObjectChildEntityTypes
	
	#region MetaObjectFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;MetaObjectColumn&gt;"/> class
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
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;MetaObjectColumn&gt;"/> class
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
	
	#region MetaObjectSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;MetaObjectColumn&gt;"/> class
	/// that is used exclusively with a <see cref="MetaObject"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class MetaObjectSortBuilder : SqlSortBuilder<MetaObjectColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the MetaObjectSqlSortBuilder class.
		/// </summary>
		public MetaObjectSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion MetaObjectSortBuilder
	
} // end namespace
