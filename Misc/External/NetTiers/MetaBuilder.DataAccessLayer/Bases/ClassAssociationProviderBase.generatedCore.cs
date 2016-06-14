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
	/// This class is the base class for any <see cref="ClassAssociationProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract partial class ClassAssociationProviderBaseCore : EntityProviderBase<MetaBuilder.BusinessLogic.ClassAssociation, MetaBuilder.BusinessLogic.ClassAssociationKey>
	{		
		#region Get from Many To Many Relationship Functions
		#region GetByObjectIDObjectMachineFromObjectAssociation
		
		/// <summary>
		///		Gets ClassAssociation objects from the datasource by ObjectID in the
		///		ObjectAssociation table. Table ClassAssociation is related to table MetaObject
		///		through the (M:N) relationship defined in the ObjectAssociation table.
		/// </summary>
		/// <param name="_objectID"></param>
		/// <param name="_objectMachine"></param>
		/// <returns>Returns a typed collection of ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByObjectIDObjectMachineFromObjectAssociation(System.Int32 _objectID, System.String _objectMachine)
		{
			int count = -1;
			return GetByObjectIDObjectMachineFromObjectAssociation(null,_objectID, _objectMachine, 0, int.MaxValue, out count);
			
		}
		
		/// <summary>
		///		Gets MetaBuilder.BusinessLogic.ClassAssociation objects from the datasource by ObjectID in the
		///		ObjectAssociation table. Table ClassAssociation is related to table MetaObject
		///		through the (M:N) relationship defined in the ObjectAssociation table.
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_objectID"></param>
		/// <param name="_objectMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByObjectIDObjectMachineFromObjectAssociation(System.Int32 _objectID, System.String _objectMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByObjectIDObjectMachineFromObjectAssociation(null, _objectID, _objectMachine, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets ClassAssociation objects from the datasource by ObjectID in the
		///		ObjectAssociation table. Table ClassAssociation is related to table MetaObject
		///		through the (M:N) relationship defined in the ObjectAssociation table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_objectID"></param>
		/// <param name="_objectMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByObjectIDObjectMachineFromObjectAssociation(TransactionManager transactionManager, System.Int32 _objectID, System.String _objectMachine)
		{
			int count = -1;
			return GetByObjectIDObjectMachineFromObjectAssociation(transactionManager, _objectID, _objectMachine, 0, int.MaxValue, out count);
		}
		
		
		/// <summary>
		///		Gets ClassAssociation objects from the datasource by ObjectID in the
		///		ObjectAssociation table. Table ClassAssociation is related to table MetaObject
		///		through the (M:N) relationship defined in the ObjectAssociation table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_objectID"></param>
		/// <param name="_objectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByObjectIDObjectMachineFromObjectAssociation(TransactionManager transactionManager, System.Int32 _objectID, System.String _objectMachine,int start, int pageLength)
		{
			int count = -1;
			return GetByObjectIDObjectMachineFromObjectAssociation(transactionManager, _objectID, _objectMachine, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets ClassAssociation objects from the datasource by ObjectID in the
		///		ObjectAssociation table. Table ClassAssociation is related to table MetaObject
		///		through the (M:N) relationship defined in the ObjectAssociation table.
		/// </summary>
		/// <param name="_objectID"></param>
		/// <param name="_objectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByObjectIDObjectMachineFromObjectAssociation(System.Int32 _objectID, System.String _objectMachine,int start, int pageLength, out int count)
		{
			
			return GetByObjectIDObjectMachineFromObjectAssociation(null, _objectID, _objectMachine, start, pageLength, out count);
		}


		/// <summary>
		///		Gets ClassAssociation objects from the datasource by ObjectID in the
		///		ObjectAssociation table. Table ClassAssociation is related to table MetaObject
		///		through the (M:N) relationship defined in the ObjectAssociation table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <param name="_objectID"></param>
		/// <param name="_objectMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of ClassAssociation objects.</returns>
		public abstract TList<ClassAssociation> GetByObjectIDObjectMachineFromObjectAssociation(TransactionManager transactionManager,System.Int32 _objectID, System.String _objectMachine, int start, int pageLength, out int count);
		
		#endregion GetByObjectIDObjectMachineFromObjectAssociation
		
		#region GetByChildObjectIDChildObjectMachineFromObjectAssociation
		
		/// <summary>
		///		Gets ClassAssociation objects from the datasource by ChildObjectID in the
		///		ObjectAssociation table. Table ClassAssociation is related to table MetaObject
		///		through the (M:N) relationship defined in the ObjectAssociation table.
		/// </summary>
		/// <param name="_childObjectID"></param>
		/// <param name="_childObjectMachine"></param>
		/// <returns>Returns a typed collection of ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByChildObjectIDChildObjectMachineFromObjectAssociation(System.Int32 _childObjectID, System.String _childObjectMachine)
		{
			int count = -1;
			return GetByChildObjectIDChildObjectMachineFromObjectAssociation(null,_childObjectID, _childObjectMachine, 0, int.MaxValue, out count);
			
		}
		
		/// <summary>
		///		Gets MetaBuilder.BusinessLogic.ClassAssociation objects from the datasource by ChildObjectID in the
		///		ObjectAssociation table. Table ClassAssociation is related to table MetaObject
		///		through the (M:N) relationship defined in the ObjectAssociation table.
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_childObjectID"></param>
		/// <param name="_childObjectMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByChildObjectIDChildObjectMachineFromObjectAssociation(System.Int32 _childObjectID, System.String _childObjectMachine, int start, int pageLength)
		{
			int count = -1;
			return GetByChildObjectIDChildObjectMachineFromObjectAssociation(null, _childObjectID, _childObjectMachine, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets ClassAssociation objects from the datasource by ChildObjectID in the
		///		ObjectAssociation table. Table ClassAssociation is related to table MetaObject
		///		through the (M:N) relationship defined in the ObjectAssociation table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_childObjectID"></param>
		/// <param name="_childObjectMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByChildObjectIDChildObjectMachineFromObjectAssociation(TransactionManager transactionManager, System.Int32 _childObjectID, System.String _childObjectMachine)
		{
			int count = -1;
			return GetByChildObjectIDChildObjectMachineFromObjectAssociation(transactionManager, _childObjectID, _childObjectMachine, 0, int.MaxValue, out count);
		}
		
		
		/// <summary>
		///		Gets ClassAssociation objects from the datasource by ChildObjectID in the
		///		ObjectAssociation table. Table ClassAssociation is related to table MetaObject
		///		through the (M:N) relationship defined in the ObjectAssociation table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_childObjectID"></param>
		/// <param name="_childObjectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByChildObjectIDChildObjectMachineFromObjectAssociation(TransactionManager transactionManager, System.Int32 _childObjectID, System.String _childObjectMachine,int start, int pageLength)
		{
			int count = -1;
			return GetByChildObjectIDChildObjectMachineFromObjectAssociation(transactionManager, _childObjectID, _childObjectMachine, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets ClassAssociation objects from the datasource by ChildObjectID in the
		///		ObjectAssociation table. Table ClassAssociation is related to table MetaObject
		///		through the (M:N) relationship defined in the ObjectAssociation table.
		/// </summary>
		/// <param name="_childObjectID"></param>
		/// <param name="_childObjectMachine"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByChildObjectIDChildObjectMachineFromObjectAssociation(System.Int32 _childObjectID, System.String _childObjectMachine,int start, int pageLength, out int count)
		{
			
			return GetByChildObjectIDChildObjectMachineFromObjectAssociation(null, _childObjectID, _childObjectMachine, start, pageLength, out count);
		}


		/// <summary>
		///		Gets ClassAssociation objects from the datasource by ChildObjectID in the
		///		ObjectAssociation table. Table ClassAssociation is related to table MetaObject
		///		through the (M:N) relationship defined in the ObjectAssociation table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <param name="_childObjectID"></param>
		/// <param name="_childObjectMachine"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of ClassAssociation objects.</returns>
		public abstract TList<ClassAssociation> GetByChildObjectIDChildObjectMachineFromObjectAssociation(TransactionManager transactionManager,System.Int32 _childObjectID, System.String _childObjectMachine, int start, int pageLength, out int count);
		
		#endregion GetByChildObjectIDChildObjectMachineFromObjectAssociation
		
		#region GetByClassFromAllowedArtifact
		
		/// <summary>
		///		Gets ClassAssociation objects from the datasource by Class in the
		///		AllowedArtifact table. Table ClassAssociation is related to table Class
		///		through the (M:N) relationship defined in the AllowedArtifact table.
		/// </summary>
		/// <param name="_class"></param>
		/// <returns>Returns a typed collection of ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByClassFromAllowedArtifact(System.String _class)
		{
			int count = -1;
			return GetByClassFromAllowedArtifact(null,_class, 0, int.MaxValue, out count);
			
		}
		
		/// <summary>
		///		Gets MetaBuilder.BusinessLogic.ClassAssociation objects from the datasource by Class in the
		///		AllowedArtifact table. Table ClassAssociation is related to table Class
		///		through the (M:N) relationship defined in the AllowedArtifact table.
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_class"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByClassFromAllowedArtifact(System.String _class, int start, int pageLength)
		{
			int count = -1;
			return GetByClassFromAllowedArtifact(null, _class, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets ClassAssociation objects from the datasource by Class in the
		///		AllowedArtifact table. Table ClassAssociation is related to table Class
		///		through the (M:N) relationship defined in the AllowedArtifact table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_class"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByClassFromAllowedArtifact(TransactionManager transactionManager, System.String _class)
		{
			int count = -1;
			return GetByClassFromAllowedArtifact(transactionManager, _class, 0, int.MaxValue, out count);
		}
		
		
		/// <summary>
		///		Gets ClassAssociation objects from the datasource by Class in the
		///		AllowedArtifact table. Table ClassAssociation is related to table Class
		///		through the (M:N) relationship defined in the AllowedArtifact table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_class"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByClassFromAllowedArtifact(TransactionManager transactionManager, System.String _class,int start, int pageLength)
		{
			int count = -1;
			return GetByClassFromAllowedArtifact(transactionManager, _class, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets ClassAssociation objects from the datasource by Class in the
		///		AllowedArtifact table. Table ClassAssociation is related to table Class
		///		through the (M:N) relationship defined in the AllowedArtifact table.
		/// </summary>
		/// <param name="_class"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByClassFromAllowedArtifact(System.String _class,int start, int pageLength, out int count)
		{
			
			return GetByClassFromAllowedArtifact(null, _class, start, pageLength, out count);
		}


		/// <summary>
		///		Gets ClassAssociation objects from the datasource by Class in the
		///		AllowedArtifact table. Table ClassAssociation is related to table Class
		///		through the (M:N) relationship defined in the AllowedArtifact table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <param name="_class"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of ClassAssociation objects.</returns>
		public abstract TList<ClassAssociation> GetByClassFromAllowedArtifact(TransactionManager transactionManager,System.String _class, int start, int pageLength, out int count);
		
		#endregion GetByClassFromAllowedArtifact
		
		#endregion	
		
		#region Delete Methods

		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager">A <see cref="TransactionManager"/> object.</param>
		/// <param name="key">The unique identifier of the row to delete.</param>
		/// <returns>Returns true if operation suceeded.</returns>
		public override bool Delete(TransactionManager transactionManager, MetaBuilder.BusinessLogic.ClassAssociationKey key)
		{
			return Delete(transactionManager, key.CAid);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="_cAid">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(System.Int32 _cAid)
		{
			return Delete(null, _cAid);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_cAid">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(TransactionManager transactionManager, System.Int32 _cAid);		
		
		#endregion Delete Methods
		
		#region Get By Foreign Key Functions
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_Class key.
		///		FK_AllowedClassRelationship_Class Description: 
		/// </summary>
		/// <param name="_parentClass"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByParentClass(System.String _parentClass)
		{
			int count = -1;
			return GetByParentClass(_parentClass, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_Class key.
		///		FK_AllowedClassRelationship_Class Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_parentClass"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		/// <remarks></remarks>
		public TList<ClassAssociation> GetByParentClass(TransactionManager transactionManager, System.String _parentClass)
		{
			int count = -1;
			return GetByParentClass(transactionManager, _parentClass, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_Class key.
		///		FK_AllowedClassRelationship_Class Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_parentClass"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByParentClass(TransactionManager transactionManager, System.String _parentClass, int start, int pageLength)
		{
			int count = -1;
			return GetByParentClass(transactionManager, _parentClass, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_Class key.
		///		fK_AllowedClassRelationship_Class Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_parentClass"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByParentClass(System.String _parentClass, int start, int pageLength)
		{
			int count =  -1;
			return GetByParentClass(null, _parentClass, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_Class key.
		///		fK_AllowedClassRelationship_Class Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_parentClass"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByParentClass(System.String _parentClass, int start, int pageLength,out int count)
		{
			return GetByParentClass(null, _parentClass, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_Class key.
		///		FK_AllowedClassRelationship_Class Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_parentClass"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public abstract TList<ClassAssociation> GetByParentClass(TransactionManager transactionManager, System.String _parentClass, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_Class1 key.
		///		FK_AllowedClassRelationship_Class1 Description: 
		/// </summary>
		/// <param name="_childClass"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByChildClass(System.String _childClass)
		{
			int count = -1;
			return GetByChildClass(_childClass, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_Class1 key.
		///		FK_AllowedClassRelationship_Class1 Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_childClass"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		/// <remarks></remarks>
		public TList<ClassAssociation> GetByChildClass(TransactionManager transactionManager, System.String _childClass)
		{
			int count = -1;
			return GetByChildClass(transactionManager, _childClass, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_Class1 key.
		///		FK_AllowedClassRelationship_Class1 Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_childClass"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByChildClass(TransactionManager transactionManager, System.String _childClass, int start, int pageLength)
		{
			int count = -1;
			return GetByChildClass(transactionManager, _childClass, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_Class1 key.
		///		fK_AllowedClassRelationship_Class1 Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_childClass"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByChildClass(System.String _childClass, int start, int pageLength)
		{
			int count =  -1;
			return GetByChildClass(null, _childClass, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_Class1 key.
		///		fK_AllowedClassRelationship_Class1 Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_childClass"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByChildClass(System.String _childClass, int start, int pageLength,out int count)
		{
			return GetByChildClass(null, _childClass, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_Class1 key.
		///		FK_AllowedClassRelationship_Class1 Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_childClass"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public abstract TList<ClassAssociation> GetByChildClass(TransactionManager transactionManager, System.String _childClass, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_RelationshipType key.
		///		FK_AllowedClassRelationship_RelationshipType Description: 
		/// </summary>
		/// <param name="_associationTypeID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByAssociationTypeID(System.Int32 _associationTypeID)
		{
			int count = -1;
			return GetByAssociationTypeID(_associationTypeID, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_RelationshipType key.
		///		FK_AllowedClassRelationship_RelationshipType Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_associationTypeID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		/// <remarks></remarks>
		public TList<ClassAssociation> GetByAssociationTypeID(TransactionManager transactionManager, System.Int32 _associationTypeID)
		{
			int count = -1;
			return GetByAssociationTypeID(transactionManager, _associationTypeID, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_RelationshipType key.
		///		FK_AllowedClassRelationship_RelationshipType Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_associationTypeID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByAssociationTypeID(TransactionManager transactionManager, System.Int32 _associationTypeID, int start, int pageLength)
		{
			int count = -1;
			return GetByAssociationTypeID(transactionManager, _associationTypeID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_RelationshipType key.
		///		fK_AllowedClassRelationship_RelationshipType Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_associationTypeID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByAssociationTypeID(System.Int32 _associationTypeID, int start, int pageLength)
		{
			int count =  -1;
			return GetByAssociationTypeID(null, _associationTypeID, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_RelationshipType key.
		///		fK_AllowedClassRelationship_RelationshipType Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_associationTypeID"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByAssociationTypeID(System.Int32 _associationTypeID, int start, int pageLength,out int count)
		{
			return GetByAssociationTypeID(null, _associationTypeID, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_RelationshipType key.
		///		FK_AllowedClassRelationship_RelationshipType Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_associationTypeID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public abstract TList<ClassAssociation> GetByAssociationTypeID(TransactionManager transactionManager, System.Int32 _associationTypeID, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ClassAssociation_Class key.
		///		FK_ClassAssociation_Class Description: 
		/// </summary>
		/// <param name="_associationObjectClass"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByAssociationObjectClass(System.String _associationObjectClass)
		{
			int count = -1;
			return GetByAssociationObjectClass(_associationObjectClass, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ClassAssociation_Class key.
		///		FK_ClassAssociation_Class Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_associationObjectClass"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		/// <remarks></remarks>
		public TList<ClassAssociation> GetByAssociationObjectClass(TransactionManager transactionManager, System.String _associationObjectClass)
		{
			int count = -1;
			return GetByAssociationObjectClass(transactionManager, _associationObjectClass, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_ClassAssociation_Class key.
		///		FK_ClassAssociation_Class Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_associationObjectClass"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByAssociationObjectClass(TransactionManager transactionManager, System.String _associationObjectClass, int start, int pageLength)
		{
			int count = -1;
			return GetByAssociationObjectClass(transactionManager, _associationObjectClass, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ClassAssociation_Class key.
		///		fK_ClassAssociation_Class Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_associationObjectClass"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByAssociationObjectClass(System.String _associationObjectClass, int start, int pageLength)
		{
			int count =  -1;
			return GetByAssociationObjectClass(null, _associationObjectClass, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ClassAssociation_Class key.
		///		fK_ClassAssociation_Class Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_associationObjectClass"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByAssociationObjectClass(System.String _associationObjectClass, int start, int pageLength,out int count)
		{
			return GetByAssociationObjectClass(null, _associationObjectClass, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ClassAssociation_Class key.
		///		FK_ClassAssociation_Class Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_associationObjectClass"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public abstract TList<ClassAssociation> GetByAssociationObjectClass(TransactionManager transactionManager, System.String _associationObjectClass, int start, int pageLength, out int count);
		
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
		public override MetaBuilder.BusinessLogic.ClassAssociation Get(TransactionManager transactionManager, MetaBuilder.BusinessLogic.ClassAssociationKey key, int start, int pageLength)
		{
			return GetByCAid(transactionManager, key.CAid, start, pageLength);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key PK_ClassAssociation index.
		/// </summary>
		/// <param name="_cAid"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ClassAssociation"/> class.</returns>
		public MetaBuilder.BusinessLogic.ClassAssociation GetByCAid(System.Int32 _cAid)
		{
			int count = -1;
			return GetByCAid(null,_cAid, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ClassAssociation index.
		/// </summary>
		/// <param name="_cAid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ClassAssociation"/> class.</returns>
		public MetaBuilder.BusinessLogic.ClassAssociation GetByCAid(System.Int32 _cAid, int start, int pageLength)
		{
			int count = -1;
			return GetByCAid(null, _cAid, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ClassAssociation index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_cAid"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ClassAssociation"/> class.</returns>
		public MetaBuilder.BusinessLogic.ClassAssociation GetByCAid(TransactionManager transactionManager, System.Int32 _cAid)
		{
			int count = -1;
			return GetByCAid(transactionManager, _cAid, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ClassAssociation index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_cAid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ClassAssociation"/> class.</returns>
		public MetaBuilder.BusinessLogic.ClassAssociation GetByCAid(TransactionManager transactionManager, System.Int32 _cAid, int start, int pageLength)
		{
			int count = -1;
			return GetByCAid(transactionManager, _cAid, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ClassAssociation index.
		/// </summary>
		/// <param name="_cAid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ClassAssociation"/> class.</returns>
		public MetaBuilder.BusinessLogic.ClassAssociation GetByCAid(System.Int32 _cAid, int start, int pageLength, out int count)
		{
			return GetByCAid(null, _cAid, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ClassAssociation index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_cAid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ClassAssociation"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.ClassAssociation GetByCAid(TransactionManager transactionManager, System.Int32 _cAid, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a TList&lt;ClassAssociation&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="TList&lt;ClassAssociation&gt;"/></returns>
		public static TList<ClassAssociation> Fill(IDataReader reader, TList<ClassAssociation> rows, int start, int pageLength)
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
				
				MetaBuilder.BusinessLogic.ClassAssociation c = null;
				if (useEntityFactory)
				{
					key = new System.Text.StringBuilder("ClassAssociation")
					.Append("|").Append((System.Int32)reader[((int)ClassAssociationColumn.CAid - 1)]).ToString();
					c = EntityManager.LocateOrCreate<ClassAssociation>(
					key.ToString(), // EntityTrackingKey
					"ClassAssociation",  //Creational Type
					entityCreationFactoryType,  //Factory used to create entity
					enableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.ClassAssociation();
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
					c.CAid = (System.Int32)reader[((int)ClassAssociationColumn.CAid - 1)];
					c.ParentClass = (System.String)reader[((int)ClassAssociationColumn.ParentClass - 1)];
					c.ChildClass = (System.String)reader[((int)ClassAssociationColumn.ChildClass - 1)];
					c.AssociationTypeID = (System.Int32)reader[((int)ClassAssociationColumn.AssociationTypeID - 1)];
					c.Caption = (reader.IsDBNull(((int)ClassAssociationColumn.Caption - 1)))?null:(System.String)reader[((int)ClassAssociationColumn.Caption - 1)];
					c.AssociationObjectClass = (reader.IsDBNull(((int)ClassAssociationColumn.AssociationObjectClass - 1)))?null:(System.String)reader[((int)ClassAssociationColumn.AssociationObjectClass - 1)];
					c.CopyIncluded = (System.Boolean)reader[((int)ClassAssociationColumn.CopyIncluded - 1)];
					c.IsDefault = (System.Boolean)reader[((int)ClassAssociationColumn.IsDefault - 1)];
					c.IsActive = (reader.IsDBNull(((int)ClassAssociationColumn.IsActive - 1)))?null:(System.Boolean?)reader[((int)ClassAssociationColumn.IsActive - 1)];
					c.EntityTrackingKey = key;
					c.AcceptChanges();
					c.SuppressEntityEvents = false;
				}
				rows.Add(c);
			}
		return rows;
		}		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.ClassAssociation"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.ClassAssociation"/> object to refresh.</param>
		public static void RefreshEntity(IDataReader reader, MetaBuilder.BusinessLogic.ClassAssociation entity)
		{
			if (!reader.Read()) return;
			
			entity.CAid = (System.Int32)reader[((int)ClassAssociationColumn.CAid - 1)];
			entity.ParentClass = (System.String)reader[((int)ClassAssociationColumn.ParentClass - 1)];
			entity.ChildClass = (System.String)reader[((int)ClassAssociationColumn.ChildClass - 1)];
			entity.AssociationTypeID = (System.Int32)reader[((int)ClassAssociationColumn.AssociationTypeID - 1)];
			entity.Caption = (reader.IsDBNull(((int)ClassAssociationColumn.Caption - 1)))?null:(System.String)reader[((int)ClassAssociationColumn.Caption - 1)];
			entity.AssociationObjectClass = (reader.IsDBNull(((int)ClassAssociationColumn.AssociationObjectClass - 1)))?null:(System.String)reader[((int)ClassAssociationColumn.AssociationObjectClass - 1)];
			entity.CopyIncluded = (System.Boolean)reader[((int)ClassAssociationColumn.CopyIncluded - 1)];
			entity.IsDefault = (System.Boolean)reader[((int)ClassAssociationColumn.IsDefault - 1)];
			entity.IsActive = (reader.IsDBNull(((int)ClassAssociationColumn.IsActive - 1)))?null:(System.Boolean?)reader[((int)ClassAssociationColumn.IsActive - 1)];
			entity.AcceptChanges();
		}
		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.ClassAssociation"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.ClassAssociation"/> object.</param>
		public static void RefreshEntity(DataSet dataSet, MetaBuilder.BusinessLogic.ClassAssociation entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.CAid = (System.Int32)dataRow["CAid"];
			entity.ParentClass = (System.String)dataRow["ParentClass"];
			entity.ChildClass = (System.String)dataRow["ChildClass"];
			entity.AssociationTypeID = (System.Int32)dataRow["AssociationTypeID"];
			entity.Caption = Convert.IsDBNull(dataRow["Caption"]) ? null : (System.String)dataRow["Caption"];
			entity.AssociationObjectClass = Convert.IsDBNull(dataRow["AssociationObjectClass"]) ? null : (System.String)dataRow["AssociationObjectClass"];
			entity.CopyIncluded = (System.Boolean)dataRow["CopyIncluded"];
			entity.IsDefault = (System.Boolean)dataRow["IsDefault"];
			entity.IsActive = Convert.IsDBNull(dataRow["IsActive"]) ? null : (System.Boolean?)dataRow["IsActive"];
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
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.ClassAssociation"/> object to load.</param>
		/// <param name="deep">Boolean. A flag that indicates whether to recursively save all Property Collection that are descendants of this instance. If True, saves the complete object graph below this object. If False, saves this object only. </param>
		/// <param name="deepLoadType">DeepLoadType Enumeration to Include/Exclude object property collections from Load.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.ClassAssociation Property Collection Type Array To Include or Exclude from Load</param>
		/// <param name="innerList">A collection of child types for easy access.</param>
	    /// <exception cref="ArgumentNullException">entity or childTypes is null.</exception>
	    /// <exception cref="ArgumentException">deepLoadType has invalid value.</exception>
		public override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.ClassAssociation entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, DeepSession innerList)
		{
			if(entity == null)
				return;

			#region ParentClassSource	
			if (CanDeepLoad(entity, "Class|ParentClassSource", deepLoadType, innerList) 
				&& entity.ParentClassSource == null)
			{
				object[] pkItems = new object[1];
				pkItems[0] = entity.ParentClass;
				Class tmpEntity = EntityManager.LocateEntity<Class>(EntityLocator.ConstructKeyFromPkItems(typeof(Class), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.ParentClassSource = tmpEntity;
				else
					entity.ParentClassSource = DataRepository.ClassProvider.GetByName(transactionManager, entity.ParentClass);		
				
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'ParentClassSource' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.ParentClassSource != null)
				{
					innerList.SkipChildren = true;
					DataRepository.ClassProvider.DeepLoad(transactionManager, entity.ParentClassSource, deep, deepLoadType, childTypes, innerList);
					innerList.SkipChildren = false;
				}
					
			}
			#endregion ParentClassSource

			#region ChildClassSource	
			if (CanDeepLoad(entity, "Class|ChildClassSource", deepLoadType, innerList) 
				&& entity.ChildClassSource == null)
			{
				object[] pkItems = new object[1];
				pkItems[0] = entity.ChildClass;
				Class tmpEntity = EntityManager.LocateEntity<Class>(EntityLocator.ConstructKeyFromPkItems(typeof(Class), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.ChildClassSource = tmpEntity;
				else
					entity.ChildClassSource = DataRepository.ClassProvider.GetByName(transactionManager, entity.ChildClass);		
				
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'ChildClassSource' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.ChildClassSource != null)
				{
					innerList.SkipChildren = true;
					DataRepository.ClassProvider.DeepLoad(transactionManager, entity.ChildClassSource, deep, deepLoadType, childTypes, innerList);
					innerList.SkipChildren = false;
				}
					
			}
			#endregion ChildClassSource

			#region AssociationTypeIDSource	
			if (CanDeepLoad(entity, "AssociationType|AssociationTypeIDSource", deepLoadType, innerList) 
				&& entity.AssociationTypeIDSource == null)
			{
				object[] pkItems = new object[1];
				pkItems[0] = entity.AssociationTypeID;
				AssociationType tmpEntity = EntityManager.LocateEntity<AssociationType>(EntityLocator.ConstructKeyFromPkItems(typeof(AssociationType), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.AssociationTypeIDSource = tmpEntity;
				else
					entity.AssociationTypeIDSource = DataRepository.AssociationTypeProvider.GetBypkid(transactionManager, entity.AssociationTypeID);		
				
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'AssociationTypeIDSource' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.AssociationTypeIDSource != null)
				{
					innerList.SkipChildren = true;
					DataRepository.AssociationTypeProvider.DeepLoad(transactionManager, entity.AssociationTypeIDSource, deep, deepLoadType, childTypes, innerList);
					innerList.SkipChildren = false;
				}
					
			}
			#endregion AssociationTypeIDSource

			#region AssociationObjectClassSource	
			if (CanDeepLoad(entity, "Class|AssociationObjectClassSource", deepLoadType, innerList) 
				&& entity.AssociationObjectClassSource == null)
			{
				object[] pkItems = new object[1];
				pkItems[0] = (entity.AssociationObjectClass ?? string.Empty);
				Class tmpEntity = EntityManager.LocateEntity<Class>(EntityLocator.ConstructKeyFromPkItems(typeof(Class), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.AssociationObjectClassSource = tmpEntity;
				else
					entity.AssociationObjectClassSource = DataRepository.ClassProvider.GetByName(transactionManager, (entity.AssociationObjectClass ?? string.Empty));		
				
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'AssociationObjectClassSource' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.AssociationObjectClassSource != null)
				{
					innerList.SkipChildren = true;
					DataRepository.ClassProvider.DeepLoad(transactionManager, entity.AssociationObjectClassSource, deep, deepLoadType, childTypes, innerList);
					innerList.SkipChildren = false;
				}
					
			}
			#endregion AssociationObjectClassSource
			
			//used to hold DeepLoad method delegates and fire after all the local children have been loaded.
			Dictionary<string, KeyValuePair<Delegate, object>> deepHandles = new Dictionary<string, KeyValuePair<Delegate, object>>();
			// Deep load child collections  - Call GetByCAid methods when available
			
			#region ObjectAssociationCollection
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<ObjectAssociation>|ObjectAssociationCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'ObjectAssociationCollection' loaded. key " + entity.EntityTrackingKey);
				#endif 

				entity.ObjectAssociationCollection = DataRepository.ObjectAssociationProvider.GetByCAid(transactionManager, entity.CAid);

				if (deep && entity.ObjectAssociationCollection.Count > 0)
				{
					deepHandles.Add("ObjectAssociationCollection",
						new KeyValuePair<Delegate, object>((DeepLoadHandle<ObjectAssociation>) DataRepository.ObjectAssociationProvider.DeepLoad,
						new object[] { transactionManager, entity.ObjectAssociationCollection, deep, deepLoadType, childTypes, innerList }
					));
				}
			}		
			#endregion 
			
			
			#region AllowedArtifactCollection
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<AllowedArtifact>|AllowedArtifactCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'AllowedArtifactCollection' loaded. key " + entity.EntityTrackingKey);
				#endif 

				entity.AllowedArtifactCollection = DataRepository.AllowedArtifactProvider.GetByCAid(transactionManager, entity.CAid);

				if (deep && entity.AllowedArtifactCollection.Count > 0)
				{
					deepHandles.Add("AllowedArtifactCollection",
						new KeyValuePair<Delegate, object>((DeepLoadHandle<AllowedArtifact>) DataRepository.AllowedArtifactProvider.DeepLoad,
						new object[] { transactionManager, entity.AllowedArtifactCollection, deep, deepLoadType, childTypes, innerList }
					));
				}
			}		
			#endregion 
			
			
			#region ClassClassCollection_From_AllowedArtifact
			// RelationshipType.ManyToMany
			if (CanDeepLoad(entity, "List<Class>|ClassClassCollection_From_AllowedArtifact", deepLoadType, innerList))
			{
				entity.ClassClassCollection_From_AllowedArtifact = DataRepository.ClassProvider.GetByCAidFromAllowedArtifact(transactionManager, entity.CAid);			 
		
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'ClassClassCollection_From_AllowedArtifact' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.ClassClassCollection_From_AllowedArtifact != null)
				{
					deepHandles.Add("ClassClassCollection_From_AllowedArtifact",
						new KeyValuePair<Delegate, object>((DeepLoadHandle< Class >) DataRepository.ClassProvider.DeepLoad,
						new object[] { transactionManager, entity.ClassClassCollection_From_AllowedArtifact, deep, deepLoadType, childTypes, innerList }
					));
				}
			}
			#endregion
			
			
			
			#region ObjectIDObjectMachineMetaObjectCollection_From_ObjectAssociation
			// RelationshipType.ManyToMany
			if (CanDeepLoad(entity, "List<MetaObject>|ObjectIDObjectMachineMetaObjectCollection_From_ObjectAssociation", deepLoadType, innerList))
			{
				entity.ObjectIDObjectMachineMetaObjectCollection_From_ObjectAssociation = DataRepository.MetaObjectProvider.GetByCAidFromObjectAssociation(transactionManager, entity.CAid);			 
		
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
			
			
			
			#region ChildObjectIDChildObjectMachineMetaObjectCollection_From_ObjectAssociation
			// RelationshipType.ManyToMany
			if (CanDeepLoad(entity, "List<MetaObject>|ChildObjectIDChildObjectMachineMetaObjectCollection_From_ObjectAssociation", deepLoadType, innerList))
			{
				entity.ChildObjectIDChildObjectMachineMetaObjectCollection_From_ObjectAssociation = DataRepository.MetaObjectProvider.GetByCAidFromObjectAssociation(transactionManager, entity.CAid);			 
		
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
		/// Deep Save the entire object graph of the MetaBuilder.BusinessLogic.ClassAssociation object with criteria based of the child 
		/// Type property array and DeepSaveType.
		/// </summary>
		/// <param name="transactionManager">The transaction manager.</param>
		/// <param name="entity">MetaBuilder.BusinessLogic.ClassAssociation instance</param>
		/// <param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.ClassAssociation Property Collection Type Array To Include or Exclude from Save</param>
		/// <param name="innerList">A Hashtable of child types for easy access.</param>
		public override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.ClassAssociation entity, DeepSaveType deepSaveType, System.Type[] childTypes, DeepSession innerList)
		{	
			if (entity == null)
				return false;
							
			#region Composite Parent Properties
			//Save Source Composite Properties, however, don't call deep save on them.  
			//So they only get saved a single level deep.
			
			#region ParentClassSource
			if (CanDeepSave(entity, "Class|ParentClassSource", deepSaveType, innerList) 
				&& entity.ParentClassSource != null)
			{
				DataRepository.ClassProvider.Save(transactionManager, entity.ParentClassSource);
				entity.ParentClass = entity.ParentClassSource.Name;
			}
			#endregion 
			
			#region ChildClassSource
			if (CanDeepSave(entity, "Class|ChildClassSource", deepSaveType, innerList) 
				&& entity.ChildClassSource != null)
			{
				DataRepository.ClassProvider.Save(transactionManager, entity.ChildClassSource);
				entity.ChildClass = entity.ChildClassSource.Name;
			}
			#endregion 
			
			#region AssociationTypeIDSource
			if (CanDeepSave(entity, "AssociationType|AssociationTypeIDSource", deepSaveType, innerList) 
				&& entity.AssociationTypeIDSource != null)
			{
				DataRepository.AssociationTypeProvider.Save(transactionManager, entity.AssociationTypeIDSource);
				entity.AssociationTypeID = entity.AssociationTypeIDSource.pkid;
			}
			#endregion 
			
			#region AssociationObjectClassSource
			if (CanDeepSave(entity, "Class|AssociationObjectClassSource", deepSaveType, innerList) 
				&& entity.AssociationObjectClassSource != null)
			{
				DataRepository.ClassProvider.Save(transactionManager, entity.AssociationObjectClassSource);
				entity.AssociationObjectClass = entity.AssociationObjectClassSource.Name;
			}
			#endregion 
			#endregion Composite Parent Properties

			// Save Root Entity through Provider
			if (!entity.IsDeleted)
				this.Save(transactionManager, entity);
			
			//used to hold DeepSave method delegates and fire after all the local children have been saved.
			Dictionary<string, KeyValuePair<Delegate, object>> deepHandles = new Dictionary<string, KeyValuePair<Delegate, object>>();

			#region ClassClassCollection_From_AllowedArtifact>
			if (CanDeepSave(entity.ClassClassCollection_From_AllowedArtifact, "List<Class>|ClassClassCollection_From_AllowedArtifact", deepSaveType, innerList))
			{
				if (entity.ClassClassCollection_From_AllowedArtifact.Count > 0 || entity.ClassClassCollection_From_AllowedArtifact.DeletedItems.Count > 0)
				{
					DataRepository.ClassProvider.Save(transactionManager, entity.ClassClassCollection_From_AllowedArtifact); 
					deepHandles.Add("ClassClassCollection_From_AllowedArtifact",
						new KeyValuePair<Delegate, object>((DeepSaveHandle<Class>) DataRepository.ClassProvider.DeepSave,
						new object[] { transactionManager, entity.ClassClassCollection_From_AllowedArtifact, deepSaveType, childTypes, innerList }
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
	
			#region List<ObjectAssociation>
				if (CanDeepSave(entity.ObjectAssociationCollection, "List<ObjectAssociation>|ObjectAssociationCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(ObjectAssociation child in entity.ObjectAssociationCollection)
					{
						if(child.CAidSource != null)
						{
								child.CAid = child.CAidSource.CAid;
						}

						if(child.ObjectIDObjectMachineSource != null)
						{
								child.ObjectID = child.ObjectIDObjectMachineSource.pkid;
								child.ObjectMachine = child.ObjectIDObjectMachineSource.Machine;
						}

						if(child.ChildObjectIDChildObjectMachineSource != null)
						{
								child.ChildObjectID = child.ChildObjectIDChildObjectMachineSource.pkid;
								child.ChildObjectMachine = child.ChildObjectIDChildObjectMachineSource.Machine;
						}

					}

					if (entity.ObjectAssociationCollection.Count > 0 || entity.ObjectAssociationCollection.DeletedItems.Count > 0)
					{
						//DataRepository.ObjectAssociationProvider.Save(transactionManager, entity.ObjectAssociationCollection);
						
						deepHandles.Add("ObjectAssociationCollection",
						new KeyValuePair<Delegate, object>((DeepSaveHandle< ObjectAssociation >) DataRepository.ObjectAssociationProvider.DeepSave,
							new object[] { transactionManager, entity.ObjectAssociationCollection, deepSaveType, childTypes, innerList }
						));
					}
				} 
			#endregion 
				
	
			#region List<AllowedArtifact>
				if (CanDeepSave(entity.AllowedArtifactCollection, "List<AllowedArtifact>|AllowedArtifactCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(AllowedArtifact child in entity.AllowedArtifactCollection)
					{
						if(child.CAidSource != null)
						{
								child.CAid = child.CAidSource.CAid;
						}

						if(child.ClassSource != null)
						{
								child.Class = child.ClassSource.Name;
						}

					}

					if (entity.AllowedArtifactCollection.Count > 0 || entity.AllowedArtifactCollection.DeletedItems.Count > 0)
					{
						//DataRepository.AllowedArtifactProvider.Save(transactionManager, entity.AllowedArtifactCollection);
						
						deepHandles.Add("AllowedArtifactCollection",
						new KeyValuePair<Delegate, object>((DeepSaveHandle< AllowedArtifact >) DataRepository.AllowedArtifactProvider.DeepSave,
							new object[] { transactionManager, entity.AllowedArtifactCollection, deepSaveType, childTypes, innerList }
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
	
	#region ClassAssociationChildEntityTypes
	
	///<summary>
	/// Enumeration used to expose the different child entity types 
	/// for child properties in <c>MetaBuilder.BusinessLogic.ClassAssociation</c>
	///</summary>
	public enum ClassAssociationChildEntityTypes
	{
		
		///<summary>
		/// Composite Property for <c>Class</c> at ParentClassSource
		///</summary>
		[ChildEntityType(typeof(Class))]
		Class,
			
		///<summary>
		/// Composite Property for <c>AssociationType</c> at AssociationTypeIDSource
		///</summary>
		[ChildEntityType(typeof(AssociationType))]
		AssociationType,
	
		///<summary>
		/// Collection of <c>ClassAssociation</c> as OneToMany for ObjectAssociationCollection
		///</summary>
		[ChildEntityType(typeof(TList<ObjectAssociation>))]
		ObjectAssociationCollection,

		///<summary>
		/// Collection of <c>ClassAssociation</c> as OneToMany for AllowedArtifactCollection
		///</summary>
		[ChildEntityType(typeof(TList<AllowedArtifact>))]
		AllowedArtifactCollection,

		///<summary>
		/// Collection of <c>ClassAssociation</c> as ManyToMany for ClassCollection_From_AllowedArtifact
		///</summary>
		[ChildEntityType(typeof(TList<Class>))]
		ClassClassCollection_From_AllowedArtifact,

		///<summary>
		/// Collection of <c>ClassAssociation</c> as ManyToMany for MetaObjectCollection_From_ObjectAssociation
		///</summary>
		[ChildEntityType(typeof(TList<MetaObject>))]
		ObjectIDObjectMachineMetaObjectCollection_From_ObjectAssociation,

		///<summary>
		/// Collection of <c>ClassAssociation</c> as ManyToMany for MetaObjectCollection_From_ObjectAssociation
		///</summary>
		[ChildEntityType(typeof(TList<MetaObject>))]
		ChildObjectIDChildObjectMachineMetaObjectCollection_From_ObjectAssociation,
	}
	
	#endregion ClassAssociationChildEntityTypes
	
	#region ClassAssociationFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;ClassAssociationColumn&gt;"/> class
	/// that is used exclusively with a <see cref="ClassAssociation"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class ClassAssociationFilterBuilder : SqlFilterBuilder<ClassAssociationColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the ClassAssociationFilterBuilder class.
		/// </summary>
		public ClassAssociationFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the ClassAssociationFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public ClassAssociationFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the ClassAssociationFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public ClassAssociationFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion ClassAssociationFilterBuilder
	
	#region ClassAssociationParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;ClassAssociationColumn&gt;"/> class
	/// that is used exclusively with a <see cref="ClassAssociation"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class ClassAssociationParameterBuilder : ParameterizedSqlFilterBuilder<ClassAssociationColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the ClassAssociationParameterBuilder class.
		/// </summary>
		public ClassAssociationParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the ClassAssociationParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public ClassAssociationParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the ClassAssociationParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public ClassAssociationParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion ClassAssociationParameterBuilder
	
	#region ClassAssociationSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;ClassAssociationColumn&gt;"/> class
	/// that is used exclusively with a <see cref="ClassAssociation"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class ClassAssociationSortBuilder : SqlSortBuilder<ClassAssociationColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the ClassAssociationSqlSortBuilder class.
		/// </summary>
		public ClassAssociationSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion ClassAssociationSortBuilder
	
} // end namespace
