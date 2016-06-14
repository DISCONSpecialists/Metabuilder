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
		/// <param name="graphFileID"></param>
		/// <param name="graphFileMachine"></param>
		/// <returns>Returns a typed collection of ObjectAssociation objects.</returns>
		public TList<ObjectAssociation> GetByGraphFileIDGraphFileMachineFromGraphFileAssociation(System.Int32 graphFileID, System.String graphFileMachine)
		{
			int count = -1;
			return GetByGraphFileIDGraphFileMachineFromGraphFileAssociation(null,graphFileID, graphFileMachine, 0, int.MaxValue, out count);
			
		}
		
		/// <summary>
		///		Gets MetaBuilder.BusinessLogic.ObjectAssociation objects from the datasource by GraphFileID in the
		///		GraphFileAssociation table. Table ObjectAssociation is related to table GraphFile
		///		through the (M:N) relationship defined in the GraphFileAssociation table.
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="graphFileID"></param>
		/// <param name="graphFileMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of ObjectAssociation objects.</returns>
		public TList<ObjectAssociation> GetByGraphFileIDGraphFileMachineFromGraphFileAssociation(System.Int32 graphFileID, System.String graphFileMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByGraphFileIDGraphFileMachineFromGraphFileAssociation(null, graphFileID, graphFileMachine, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets ObjectAssociation objects from the datasource by GraphFileID in the
		///		GraphFileAssociation table. Table ObjectAssociation is related to table GraphFile
		///		through the (M:N) relationship defined in the GraphFileAssociation table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="graphFileID"></param>
		/// <param name="graphFileMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of ObjectAssociation objects.</returns>
		public TList<ObjectAssociation> GetByGraphFileIDGraphFileMachineFromGraphFileAssociation(TransactionManager transactionManager, System.Int32 graphFileID, System.String graphFileMachine)
		{
			int count = -1;
			return GetByGraphFileIDGraphFileMachineFromGraphFileAssociation(transactionManager, graphFileID, graphFileMachine, 0, int.MaxValue, out count);
		}
		
		
		/// <summary>
		///		Gets ObjectAssociation objects from the datasource by GraphFileID in the
		///		GraphFileAssociation table. Table ObjectAssociation is related to table GraphFile
		///		through the (M:N) relationship defined in the GraphFileAssociation table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="graphFileID"></param>
		/// <param name="graphFileMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of ObjectAssociation objects.</returns>
		public TList<ObjectAssociation> GetByGraphFileIDGraphFileMachineFromGraphFileAssociation(TransactionManager transactionManager, System.Int32 graphFileID, System.String graphFileMachine,int start, int pageLength)
		{
			int count = -1;
			return GetByGraphFileIDGraphFileMachineFromGraphFileAssociation(transactionManager, graphFileID, graphFileMachine, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets ObjectAssociation objects from the datasource by GraphFileID in the
		///		GraphFileAssociation table. Table ObjectAssociation is related to table GraphFile
		///		through the (M:N) relationship defined in the GraphFileAssociation table.
		/// </summary>
		/// <param name="graphFileID"></param>
		/// <param name="graphFileMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of ObjectAssociation objects.</returns>
		public TList<ObjectAssociation> GetByGraphFileIDGraphFileMachineFromGraphFileAssociation(System.Int32 graphFileID, System.String graphFileMachine,int start, int pageLength, out int count)
		{
			
			return GetByGraphFileIDGraphFileMachineFromGraphFileAssociation(null, graphFileID, graphFileMachine, start, pageLength, out count);
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
		/// <param name="graphFileID"></param>
		/// <param name="graphFileMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of ObjectAssociation objects.</returns>
		public abstract TList<ObjectAssociation> GetByGraphFileIDGraphFileMachineFromGraphFileAssociation(TransactionManager transactionManager,System.Int32 graphFileID, System.String graphFileMachine, int start, int pageLength, out int count);
		
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
		/// <param name="cAid">. Primary Key.</param>
		/// <param name="objectID">. Primary Key.</param>
		/// <param name="childObjectID">. Primary Key.</param>
		/// <param name="objectMachine">. Primary Key.</param>
		/// <param name="childObjectMachine">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(System.Int32 cAid, System.Int32 objectID, System.Int32 childObjectID, System.String objectMachine, System.String childObjectMachine)
		{
			return Delete(null, cAid, objectID, childObjectID, objectMachine, childObjectMachine);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="cAid">. Primary Key.</param>
		/// <param name="objectID">. Primary Key.</param>
		/// <param name="childObjectID">. Primary Key.</param>
		/// <param name="objectMachine">. Primary Key.</param>
		/// <param name="childObjectMachine">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(TransactionManager transactionManager, System.Int32 cAid, System.Int32 objectID, System.Int32 childObjectID, System.String objectMachine, System.String childObjectMachine);		
		
		#endregion Delete Methods
		
		#region Get By Foreign Key Functions
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_VCStatus key.
		///		FK_ObjectAssociation_VCStatus Description: 
		/// </summary>
		/// <param name="vCStatusID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectAssociation> GetByVCStatusID(System.Int32 vCStatusID)
		{
			int count = -1;
			return GetByVCStatusID(vCStatusID, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_VCStatus key.
		///		FK_ObjectAssociation_VCStatus Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="vCStatusID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<ObjectAssociation> GetByVCStatusID(TransactionManager transactionManager, System.Int32 vCStatusID)
		{
			int count = -1;
			return GetByVCStatusID(transactionManager, vCStatusID, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_VCStatus key.
		///		FK_ObjectAssociation_VCStatus Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="vCStatusID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectAssociation> GetByVCStatusID(TransactionManager transactionManager, System.Int32 vCStatusID, int start, int pageLength)
		{
			int count = -1;
			return GetByVCStatusID(transactionManager, vCStatusID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_VCStatus key.
		///		fKObjectAssociationVCStatus Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="vCStatusID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectAssociation> GetByVCStatusID(System.Int32 vCStatusID, int start, int pageLength)
		{
			int count =  -1;
			return GetByVCStatusID(null, vCStatusID, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_VCStatus key.
		///		fKObjectAssociationVCStatus Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="vCStatusID"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectAssociation> GetByVCStatusID(System.Int32 vCStatusID, int start, int pageLength,out int count)
		{
			return GetByVCStatusID(null, vCStatusID, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_VCStatus key.
		///		FK_ObjectAssociation_VCStatus Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="vCStatusID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<ObjectAssociation> GetByVCStatusID(TransactionManager transactionManager, System.Int32 vCStatusID, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_ClassAssociation key.
		///		FK_ObjectAssociation_ClassAssociation Description: 
		/// </summary>
		/// <param name="cAid"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectAssociation> GetByCAid(System.Int32 cAid)
		{
			int count = -1;
			return GetByCAid(cAid, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_ClassAssociation key.
		///		FK_ObjectAssociation_ClassAssociation Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="cAid"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<ObjectAssociation> GetByCAid(TransactionManager transactionManager, System.Int32 cAid)
		{
			int count = -1;
			return GetByCAid(transactionManager, cAid, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_ClassAssociation key.
		///		FK_ObjectAssociation_ClassAssociation Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="cAid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectAssociation> GetByCAid(TransactionManager transactionManager, System.Int32 cAid, int start, int pageLength)
		{
			int count = -1;
			return GetByCAid(transactionManager, cAid, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_ClassAssociation key.
		///		fKObjectAssociationClassAssociation Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="cAid"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectAssociation> GetByCAid(System.Int32 cAid, int start, int pageLength)
		{
			int count =  -1;
			return GetByCAid(null, cAid, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_ClassAssociation key.
		///		fKObjectAssociationClassAssociation Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="cAid"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectAssociation> GetByCAid(System.Int32 cAid, int start, int pageLength,out int count)
		{
			return GetByCAid(null, cAid, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_ClassAssociation key.
		///		FK_ObjectAssociation_ClassAssociation Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="cAid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<ObjectAssociation> GetByCAid(TransactionManager transactionManager, System.Int32 cAid, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_MetaObject key.
		///		FK_ObjectAssociation_MetaObject Description: 
		/// </summary>
		/// <param name="objectID"></param>
		/// <param name="objectMachine"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectAssociation> GetByObjectIDObjectMachine(System.Int32 objectID, System.String objectMachine)
		{
			int count = -1;
			return GetByObjectIDObjectMachine(objectID, objectMachine, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_MetaObject key.
		///		FK_ObjectAssociation_MetaObject Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="objectID"></param>
		/// <param name="objectMachine"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<ObjectAssociation> GetByObjectIDObjectMachine(TransactionManager transactionManager, System.Int32 objectID, System.String objectMachine)
		{
			int count = -1;
			return GetByObjectIDObjectMachine(transactionManager, objectID, objectMachine, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_MetaObject key.
		///		FK_ObjectAssociation_MetaObject Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="objectID"></param>
		/// <param name="objectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectAssociation> GetByObjectIDObjectMachine(TransactionManager transactionManager, System.Int32 objectID, System.String objectMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByObjectIDObjectMachine(transactionManager, objectID, objectMachine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_MetaObject key.
		///		fKObjectAssociationMetaObject Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="objectID"></param>
		/// <param name="objectMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectAssociation> GetByObjectIDObjectMachine(System.Int32 objectID, System.String objectMachine, int start, int pageLength)
		{
			int count =  -1;
			return GetByObjectIDObjectMachine(null, objectID, objectMachine, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_MetaObject key.
		///		fKObjectAssociationMetaObject Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="objectID"></param>
		/// <param name="objectMachine"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectAssociation> GetByObjectIDObjectMachine(System.Int32 objectID, System.String objectMachine, int start, int pageLength,out int count)
		{
			return GetByObjectIDObjectMachine(null, objectID, objectMachine, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_MetaObject key.
		///		FK_ObjectAssociation_MetaObject Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="objectID"></param>
		/// <param name="objectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<ObjectAssociation> GetByObjectIDObjectMachine(TransactionManager transactionManager, System.Int32 objectID, System.String objectMachine, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_MetaObject1 key.
		///		FK_ObjectAssociation_MetaObject1 Description: 
		/// </summary>
		/// <param name="childObjectID"></param>
		/// <param name="childObjectMachine"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectAssociation> GetByChildObjectIDChildObjectMachine(System.Int32 childObjectID, System.String childObjectMachine)
		{
			int count = -1;
			return GetByChildObjectIDChildObjectMachine(childObjectID, childObjectMachine, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_MetaObject1 key.
		///		FK_ObjectAssociation_MetaObject1 Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="childObjectID"></param>
		/// <param name="childObjectMachine"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<ObjectAssociation> GetByChildObjectIDChildObjectMachine(TransactionManager transactionManager, System.Int32 childObjectID, System.String childObjectMachine)
		{
			int count = -1;
			return GetByChildObjectIDChildObjectMachine(transactionManager, childObjectID, childObjectMachine, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_MetaObject1 key.
		///		FK_ObjectAssociation_MetaObject1 Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="childObjectID"></param>
		/// <param name="childObjectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectAssociation> GetByChildObjectIDChildObjectMachine(TransactionManager transactionManager, System.Int32 childObjectID, System.String childObjectMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByChildObjectIDChildObjectMachine(transactionManager, childObjectID, childObjectMachine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_MetaObject1 key.
		///		fKObjectAssociationMetaObject1 Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="childObjectID"></param>
		/// <param name="childObjectMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectAssociation> GetByChildObjectIDChildObjectMachine(System.Int32 childObjectID, System.String childObjectMachine, int start, int pageLength)
		{
			int count =  -1;
			return GetByChildObjectIDChildObjectMachine(null, childObjectID, childObjectMachine, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_MetaObject1 key.
		///		fKObjectAssociationMetaObject1 Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="childObjectID"></param>
		/// <param name="childObjectMachine"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectAssociation> GetByChildObjectIDChildObjectMachine(System.Int32 childObjectID, System.String childObjectMachine, int start, int pageLength,out int count)
		{
			return GetByChildObjectIDChildObjectMachine(null, childObjectID, childObjectMachine, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectAssociation_MetaObject1 key.
		///		FK_ObjectAssociation_MetaObject1 Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="childObjectID"></param>
		/// <param name="childObjectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectAssociation objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<ObjectAssociation> GetByChildObjectIDChildObjectMachine(TransactionManager transactionManager, System.Int32 childObjectID, System.String childObjectMachine, int start, int pageLength, out int count);
		
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
		/// <param name="cAid"></param>
		/// <param name="objectID"></param>
		/// <param name="childObjectID"></param>
		/// <param name="objectMachine"></param>
		/// <param name="childObjectMachine"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ObjectAssociation"/> class.</returns>
		public MetaBuilder.BusinessLogic.ObjectAssociation GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(System.Int32 cAid, System.Int32 objectID, System.Int32 childObjectID, System.String objectMachine, System.String childObjectMachine)
		{
			int count = -1;
			return GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(null,cAid, objectID, childObjectID, objectMachine, childObjectMachine, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ObjectRelationship index.
		/// </summary>
		/// <param name="cAid"></param>
		/// <param name="objectID"></param>
		/// <param name="childObjectID"></param>
		/// <param name="objectMachine"></param>
		/// <param name="childObjectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ObjectAssociation"/> class.</returns>
		public MetaBuilder.BusinessLogic.ObjectAssociation GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(System.Int32 cAid, System.Int32 objectID, System.Int32 childObjectID, System.String objectMachine, System.String childObjectMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(null, cAid, objectID, childObjectID, objectMachine, childObjectMachine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ObjectRelationship index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="cAid"></param>
		/// <param name="objectID"></param>
		/// <param name="childObjectID"></param>
		/// <param name="objectMachine"></param>
		/// <param name="childObjectMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ObjectAssociation"/> class.</returns>
		public MetaBuilder.BusinessLogic.ObjectAssociation GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(TransactionManager transactionManager, System.Int32 cAid, System.Int32 objectID, System.Int32 childObjectID, System.String objectMachine, System.String childObjectMachine)
		{
			int count = -1;
			return GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(transactionManager, cAid, objectID, childObjectID, objectMachine, childObjectMachine, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ObjectRelationship index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="cAid"></param>
		/// <param name="objectID"></param>
		/// <param name="childObjectID"></param>
		/// <param name="objectMachine"></param>
		/// <param name="childObjectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ObjectAssociation"/> class.</returns>
		public MetaBuilder.BusinessLogic.ObjectAssociation GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(TransactionManager transactionManager, System.Int32 cAid, System.Int32 objectID, System.Int32 childObjectID, System.String objectMachine, System.String childObjectMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(transactionManager, cAid, objectID, childObjectID, objectMachine, childObjectMachine, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ObjectRelationship index.
		/// </summary>
		/// <param name="cAid"></param>
		/// <param name="objectID"></param>
		/// <param name="childObjectID"></param>
		/// <param name="objectMachine"></param>
		/// <param name="childObjectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ObjectAssociation"/> class.</returns>
		public MetaBuilder.BusinessLogic.ObjectAssociation GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(System.Int32 cAid, System.Int32 objectID, System.Int32 childObjectID, System.String objectMachine, System.String childObjectMachine, int start, int pageLength, out int count)
		{
			return GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(null, cAid, objectID, childObjectID, objectMachine, childObjectMachine, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ObjectRelationship index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="cAid"></param>
		/// <param name="objectID"></param>
		/// <param name="childObjectID"></param>
		/// <param name="objectMachine"></param>
		/// <param name="childObjectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ObjectAssociation"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.ObjectAssociation GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(TransactionManager transactionManager, System.Int32 cAid, System.Int32 objectID, System.Int32 childObjectID, System.String objectMachine, System.String childObjectMachine, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a MetaBuilder.BusinessLogic.TList&lt;ObjectAssociation&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="MetaBuilder.BusinessLogic.TList&lt;ObjectAssociation&gt;"/></returns>
		public static MetaBuilder.BusinessLogic.TList<ObjectAssociation> Fill(IDataReader reader, MetaBuilder.BusinessLogic.TList<ObjectAssociation> rows, int start, int pageLength)
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
				
				MetaBuilder.BusinessLogic.ObjectAssociation c = null;
				if (DataRepository.Provider.UseEntityFactory)
				{
					key = @"ObjectAssociation" 
							+ (reader.IsDBNull(reader.GetOrdinal("CAid"))?(int)0:(System.Int32)reader["CAid"]).ToString()
							+ (reader.IsDBNull(reader.GetOrdinal("ObjectID"))?(int)0:(System.Int32)reader["ObjectID"]).ToString()
							+ (reader.IsDBNull(reader.GetOrdinal("ChildObjectID"))?(int)0:(System.Int32)reader["ChildObjectID"]).ToString()
							+ (reader.IsDBNull(reader.GetOrdinal("ObjectMachine"))?string.Empty:(System.String)reader["ObjectMachine"]).ToString()
							+ (reader.IsDBNull(reader.GetOrdinal("ChildObjectMachine"))?string.Empty:(System.String)reader["ChildObjectMachine"]).ToString();

					c = EntityManager.LocateOrCreate<ObjectAssociation>(
						key.ToString(), // EntityTrackingKey 
						"ObjectAssociation",  //Creational Type
						DataRepository.Provider.EntityCreationalFactoryType,  //Factory used to create entity
						DataRepository.Provider.EnableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.ObjectAssociation();
				}
				
				if (!DataRepository.Provider.EnableEntityTracking || c.EntityState == EntityState.Added)
                {
					c.SuppressEntityEvents = true;
					c.CAid = (System.Int32)reader["CAid"];
					c.OriginalCAid = c.CAid; //(reader.IsDBNull(reader.GetOrdinal("CAid")))?(int)0:(System.Int32)reader["CAid"];
					c.ObjectID = (System.Int32)reader["ObjectID"];
					c.OriginalObjectID = c.ObjectID; //(reader.IsDBNull(reader.GetOrdinal("ObjectID")))?(int)0:(System.Int32)reader["ObjectID"];
					c.ChildObjectID = (System.Int32)reader["ChildObjectID"];
					c.OriginalChildObjectID = c.ChildObjectID; //(reader.IsDBNull(reader.GetOrdinal("ChildObjectID")))?(int)0:(System.Int32)reader["ChildObjectID"];
					c.Series = (System.Int32)reader["Series"];
					c.ObjectMachine = (System.String)reader["ObjectMachine"];
					c.OriginalObjectMachine = c.ObjectMachine; //(reader.IsDBNull(reader.GetOrdinal("ObjectMachine")))?string.Empty:(System.String)reader["ObjectMachine"];
					c.ChildObjectMachine = (System.String)reader["ChildObjectMachine"];
					c.OriginalChildObjectMachine = c.ChildObjectMachine; //(reader.IsDBNull(reader.GetOrdinal("ChildObjectMachine")))?string.Empty:(System.String)reader["ChildObjectMachine"];
					c.VCStatusID = (System.Int32)reader["VCStatusID"];
					c.VCMachineID = (reader.IsDBNull(reader.GetOrdinal("VCMachineID")))?null:(System.String)reader["VCMachineID"];
					c.Machine = (reader.IsDBNull(reader.GetOrdinal("Machine")))?null:(System.String)reader["Machine"];
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
			
			entity.CAid = (System.Int32)reader["CAid"];
			entity.OriginalCAid = (System.Int32)reader["CAid"];
			entity.ObjectID = (System.Int32)reader["ObjectID"];
			entity.OriginalObjectID = (System.Int32)reader["ObjectID"];
			entity.ChildObjectID = (System.Int32)reader["ChildObjectID"];
			entity.OriginalChildObjectID = (System.Int32)reader["ChildObjectID"];
			entity.Series = (System.Int32)reader["Series"];
			entity.ObjectMachine = (System.String)reader["ObjectMachine"];
			entity.OriginalObjectMachine = (System.String)reader["ObjectMachine"];
			entity.ChildObjectMachine = (System.String)reader["ChildObjectMachine"];
			entity.OriginalChildObjectMachine = (System.String)reader["ChildObjectMachine"];
			entity.VCStatusID = (System.Int32)reader["VCStatusID"];
			entity.VCMachineID = (reader.IsDBNull(reader.GetOrdinal("VCMachineID")))?null:(System.String)reader["VCMachineID"];
			entity.Machine = (reader.IsDBNull(reader.GetOrdinal("Machine")))?null:(System.String)reader["Machine"];
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
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?null:(System.String)dataRow["VCMachineID"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?null:(System.String)dataRow["Machine"];
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
		internal override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.ObjectAssociation entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, ChildEntityTypesList innerList)
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

			#region CAidSource	
			if (CanDeepLoad(entity, "ClassAssociation", "CAidSource", deepLoadType, innerList) 
				&& entity.CAidSource == null)
			{
				object[] pkItems = new object[1];
				pkItems[0] = entity.CAid;
				ClassAssociation tmpEntity = EntityManager.LocateEntity<ClassAssociation>(EntityLocator.ConstructKeyFromPkItems(typeof(ClassAssociation), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.CAidSource = tmpEntity;
				else
					entity.CAidSource = DataRepository.ClassAssociationProvider.GetByCAid(entity.CAid);
			
				if (deep && entity.CAidSource != null)
				{
					DataRepository.ClassAssociationProvider.DeepLoad(transactionManager, entity.CAidSource, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion CAidSource

			#region ObjectIDObjectMachineSource	
			if (CanDeepLoad(entity, "MetaObject", "ObjectIDObjectMachineSource", deepLoadType, innerList) 
				&& entity.ObjectIDObjectMachineSource == null)
			{
				object[] pkItems = new object[2];
				pkItems[0] = entity.ObjectID;
				pkItems[1] = entity.ObjectMachine;
				MetaObject tmpEntity = EntityManager.LocateEntity<MetaObject>(EntityLocator.ConstructKeyFromPkItems(typeof(MetaObject), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.ObjectIDObjectMachineSource = tmpEntity;
				else
					entity.ObjectIDObjectMachineSource = DataRepository.MetaObjectProvider.GetByPkidMachine(entity.ObjectID, entity.ObjectMachine);
			
				if (deep && entity.ObjectIDObjectMachineSource != null)
				{
					DataRepository.MetaObjectProvider.DeepLoad(transactionManager, entity.ObjectIDObjectMachineSource, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion ObjectIDObjectMachineSource

			#region ChildObjectIDChildObjectMachineSource	
			if (CanDeepLoad(entity, "MetaObject", "ChildObjectIDChildObjectMachineSource", deepLoadType, innerList) 
				&& entity.ChildObjectIDChildObjectMachineSource == null)
			{
				object[] pkItems = new object[2];
				pkItems[0] = entity.ChildObjectID;
				pkItems[1] = entity.ChildObjectMachine;
				MetaObject tmpEntity = EntityManager.LocateEntity<MetaObject>(EntityLocator.ConstructKeyFromPkItems(typeof(MetaObject), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.ChildObjectIDChildObjectMachineSource = tmpEntity;
				else
					entity.ChildObjectIDChildObjectMachineSource = DataRepository.MetaObjectProvider.GetByPkidMachine(entity.ChildObjectID, entity.ChildObjectMachine);
			
				if (deep && entity.ChildObjectIDChildObjectMachineSource != null)
				{
					DataRepository.MetaObjectProvider.DeepLoad(transactionManager, entity.ChildObjectIDChildObjectMachineSource, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion ChildObjectIDChildObjectMachineSource
			
			// Load Entity through Provider
			// Deep load child collections  - Call GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine methods when available
			
			#region GraphFileCollection_From_GraphFileAssociation
			// RelationshipType.ManyToMany
			if (CanDeepLoad(entity, "List<GraphFile>", "GraphFileCollection_From_GraphFileAssociation", deepLoadType, innerList))
			{
				entity.GraphFileCollection_From_GraphFileAssociation = DataRepository.GraphFileProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachineFromGraphFileAssociation(transactionManager, entity.CAid, entity.ObjectID, entity.ChildObjectID, entity.ObjectMachine, entity.ChildObjectMachine);			 
		
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'GraphFileCollection_From_GraphFileAssociation' loaded.");
				#endif 

				if (deep && entity.GraphFileCollection_From_GraphFileAssociation.Count > 0)
				{
					DataRepository.GraphFileProvider.DeepLoad(transactionManager, entity.GraphFileCollection_From_GraphFileAssociation, deep, deepLoadType, childTypes, innerList);
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

				entity.GraphFileAssociationCollection = DataRepository.GraphFileAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(transactionManager, entity.CAid, entity.ObjectID, entity.ChildObjectID, entity.ObjectMachine, entity.ChildObjectMachine);

				if (deep && entity.GraphFileAssociationCollection.Count > 0)
				{
					DataRepository.GraphFileAssociationProvider.DeepLoad(transactionManager, entity.GraphFileAssociationCollection, deep, deepLoadType, childTypes, innerList);
				}
			}		
			#endregion 
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
		internal override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.ObjectAssociation entity, DeepSaveType deepSaveType, System.Type[] childTypes, ChildEntityTypesList innerList)
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
			
			#region CAidSource
			if (CanDeepSave(entity, "ClassAssociation", "CAidSource", deepSaveType, innerList) 
				&& entity.CAidSource != null)
			{
				DataRepository.ClassAssociationProvider.Save(transactionManager, entity.CAidSource);
				entity.CAid = entity.CAidSource.CAid;
			}
			#endregion 
			
			#region ObjectIDObjectMachineSource
			if (CanDeepSave(entity, "MetaObject", "ObjectIDObjectMachineSource", deepSaveType, innerList) 
				&& entity.ObjectIDObjectMachineSource != null)
			{
				DataRepository.MetaObjectProvider.Save(transactionManager, entity.ObjectIDObjectMachineSource);
				entity.ObjectID = entity.ObjectIDObjectMachineSource.Pkid;
				entity.ObjectMachine = entity.ObjectIDObjectMachineSource.Machine;
			}
			#endregion 
			
			#region ChildObjectIDChildObjectMachineSource
			if (CanDeepSave(entity, "MetaObject", "ChildObjectIDChildObjectMachineSource", deepSaveType, innerList) 
				&& entity.ChildObjectIDChildObjectMachineSource != null)
			{
				DataRepository.MetaObjectProvider.Save(transactionManager, entity.ChildObjectIDChildObjectMachineSource);
				entity.ChildObjectID = entity.ChildObjectIDChildObjectMachineSource.Pkid;
				entity.ChildObjectMachine = entity.ChildObjectIDChildObjectMachineSource.Machine;
			}
			#endregion 
			#endregion Composite Parent Properties

			// Save Root Entity through Provider
			this.Save(transactionManager, entity);
			
			



			#region GraphFileCollection_From_GraphFileAssociation>
			if (CanDeepSave(entity, "List<GraphFile>", "GraphFileCollection_From_GraphFileAssociation", deepSaveType, innerList))
			{
				if (entity.GraphFileCollection_From_GraphFileAssociation.Count > 0 || entity.GraphFileCollection_From_GraphFileAssociation.DeletedItems.Count > 0)
					DataRepository.GraphFileProvider.DeepSave(transactionManager, entity.GraphFileCollection_From_GraphFileAssociation, deepSaveType, childTypes, innerList); 
			}
			#endregion 



			#region List<GraphFileAssociation>
				if (CanDeepSave(entity, "List<GraphFileAssociation>", "GraphFileAssociationCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(GraphFileAssociation child in entity.GraphFileAssociationCollection)
					{
						child.CAid = entity.CAid;
						child.ObjectID = entity.ObjectID;
						child.ChildObjectID = entity.ChildObjectID;
						child.ObjectMachine = entity.ObjectMachine;
						child.ChildObjectMachine = entity.ChildObjectMachine;
					}
				
				if (entity.GraphFileAssociationCollection.Count > 0 || entity.GraphFileAssociationCollection.DeletedItems.Count > 0)
					DataRepository.GraphFileAssociationProvider.DeepSave(transactionManager, entity.GraphFileAssociationCollection, deepSaveType, childTypes, innerList);
				} 
			#endregion 
				


						
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
		/// Collection of <c>ObjectAssociation</c> as ManyToMany for GraphFileCollection_From_GraphFileAssociation
		///</summary>
		[ChildEntityType(typeof(TList<GraphFile>))]
		GraphFileCollection_From_GraphFileAssociation,

		///<summary>
		/// Collection of <c>ObjectAssociation</c> as OneToMany for GraphFileAssociationCollection
		///</summary>
		[ChildEntityType(typeof(TList<GraphFileAssociation>))]
		GraphFileAssociationCollection,
	}
	
	#endregion ObjectAssociationChildEntityTypes
	
	#region ObjectAssociationFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
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
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
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
} // end namespace
