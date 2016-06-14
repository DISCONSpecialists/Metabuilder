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
	/// This class is the base class for any <see cref="ClassAssociationProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract partial class ClassAssociationProviderBaseCore : EntityProviderBase<MetaBuilder.BusinessLogic.ClassAssociation, MetaBuilder.BusinessLogic.ClassAssociationKey>
	{		
		#region Get from Many To Many Relationship Functions
		#region GetByClassFromAllowedArtifact
		
		/// <summary>
		///		Gets ClassAssociation objects from the datasource by Class in the
		///		AllowedArtifact table. Table ClassAssociation is related to table Class
		///		through the (M:N) relationship defined in the AllowedArtifact table.
		/// </summary>
		/// <param name="@class"></param>
		/// <returns>Returns a typed collection of ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByClassFromAllowedArtifact(System.String @class)
		{
			int count = -1;
			return GetByClassFromAllowedArtifact(null,@class, 0, int.MaxValue, out count);
			
		}
		
		/// <summary>
		///		Gets MetaBuilder.BusinessLogic.ClassAssociation objects from the datasource by Class in the
		///		AllowedArtifact table. Table ClassAssociation is related to table Class
		///		through the (M:N) relationship defined in the AllowedArtifact table.
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="@class"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByClassFromAllowedArtifact(System.String @class, int start, int pageLength)
		{
			int count = -1;
			return GetByClassFromAllowedArtifact(null, @class, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets ClassAssociation objects from the datasource by Class in the
		///		AllowedArtifact table. Table ClassAssociation is related to table Class
		///		through the (M:N) relationship defined in the AllowedArtifact table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="@class"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByClassFromAllowedArtifact(TransactionManager transactionManager, System.String @class)
		{
			int count = -1;
			return GetByClassFromAllowedArtifact(transactionManager, @class, 0, int.MaxValue, out count);
		}
		
		
		/// <summary>
		///		Gets ClassAssociation objects from the datasource by Class in the
		///		AllowedArtifact table. Table ClassAssociation is related to table Class
		///		through the (M:N) relationship defined in the AllowedArtifact table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="@class"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByClassFromAllowedArtifact(TransactionManager transactionManager, System.String @class,int start, int pageLength)
		{
			int count = -1;
			return GetByClassFromAllowedArtifact(transactionManager, @class, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets ClassAssociation objects from the datasource by Class in the
		///		AllowedArtifact table. Table ClassAssociation is related to table Class
		///		through the (M:N) relationship defined in the AllowedArtifact table.
		/// </summary>
		/// <param name="@class"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of ClassAssociation objects.</returns>
		public TList<ClassAssociation> GetByClassFromAllowedArtifact(System.String @class,int start, int pageLength, out int count)
		{
			
			return GetByClassFromAllowedArtifact(null, @class, start, pageLength, out count);
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
		/// <param name="@class"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of ClassAssociation objects.</returns>
		public abstract TList<ClassAssociation> GetByClassFromAllowedArtifact(TransactionManager transactionManager,System.String @class, int start, int pageLength, out int count);
		
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
		/// <param name="cAid">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(System.Int32 cAid)
		{
			return Delete(null, cAid);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="cAid">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(TransactionManager transactionManager, System.Int32 cAid);		
		
		#endregion Delete Methods
		
		#region Get By Foreign Key Functions
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_Class key.
		///		FK_AllowedClassRelationship_Class Description: 
		/// </summary>
		/// <param name="parentClass"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ClassAssociation> GetByParentClass(System.String parentClass)
		{
			int count = -1;
			return GetByParentClass(parentClass, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_Class key.
		///		FK_AllowedClassRelationship_Class Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="parentClass"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<ClassAssociation> GetByParentClass(TransactionManager transactionManager, System.String parentClass)
		{
			int count = -1;
			return GetByParentClass(transactionManager, parentClass, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_Class key.
		///		FK_AllowedClassRelationship_Class Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="parentClass"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ClassAssociation> GetByParentClass(TransactionManager transactionManager, System.String parentClass, int start, int pageLength)
		{
			int count = -1;
			return GetByParentClass(transactionManager, parentClass, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_Class key.
		///		fKAllowedClassRelationshipClass Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="parentClass"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ClassAssociation> GetByParentClass(System.String parentClass, int start, int pageLength)
		{
			int count =  -1;
			return GetByParentClass(null, parentClass, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_Class key.
		///		fKAllowedClassRelationshipClass Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="parentClass"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ClassAssociation> GetByParentClass(System.String parentClass, int start, int pageLength,out int count)
		{
			return GetByParentClass(null, parentClass, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_Class key.
		///		FK_AllowedClassRelationship_Class Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="parentClass"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<ClassAssociation> GetByParentClass(TransactionManager transactionManager, System.String parentClass, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_Class1 key.
		///		FK_AllowedClassRelationship_Class1 Description: 
		/// </summary>
		/// <param name="childClass"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ClassAssociation> GetByChildClass(System.String childClass)
		{
			int count = -1;
			return GetByChildClass(childClass, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_Class1 key.
		///		FK_AllowedClassRelationship_Class1 Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="childClass"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<ClassAssociation> GetByChildClass(TransactionManager transactionManager, System.String childClass)
		{
			int count = -1;
			return GetByChildClass(transactionManager, childClass, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_Class1 key.
		///		FK_AllowedClassRelationship_Class1 Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="childClass"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ClassAssociation> GetByChildClass(TransactionManager transactionManager, System.String childClass, int start, int pageLength)
		{
			int count = -1;
			return GetByChildClass(transactionManager, childClass, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_Class1 key.
		///		fKAllowedClassRelationshipClass1 Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="childClass"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ClassAssociation> GetByChildClass(System.String childClass, int start, int pageLength)
		{
			int count =  -1;
			return GetByChildClass(null, childClass, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_Class1 key.
		///		fKAllowedClassRelationshipClass1 Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="childClass"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ClassAssociation> GetByChildClass(System.String childClass, int start, int pageLength,out int count)
		{
			return GetByChildClass(null, childClass, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_Class1 key.
		///		FK_AllowedClassRelationship_Class1 Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="childClass"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<ClassAssociation> GetByChildClass(TransactionManager transactionManager, System.String childClass, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_RelationshipType key.
		///		FK_AllowedClassRelationship_RelationshipType Description: 
		/// </summary>
		/// <param name="associationTypeID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ClassAssociation> GetByAssociationTypeID(System.Int32 associationTypeID)
		{
			int count = -1;
			return GetByAssociationTypeID(associationTypeID, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_RelationshipType key.
		///		FK_AllowedClassRelationship_RelationshipType Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="associationTypeID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<ClassAssociation> GetByAssociationTypeID(TransactionManager transactionManager, System.Int32 associationTypeID)
		{
			int count = -1;
			return GetByAssociationTypeID(transactionManager, associationTypeID, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_RelationshipType key.
		///		FK_AllowedClassRelationship_RelationshipType Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="associationTypeID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ClassAssociation> GetByAssociationTypeID(TransactionManager transactionManager, System.Int32 associationTypeID, int start, int pageLength)
		{
			int count = -1;
			return GetByAssociationTypeID(transactionManager, associationTypeID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_RelationshipType key.
		///		fKAllowedClassRelationshipRelationshipType Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="associationTypeID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ClassAssociation> GetByAssociationTypeID(System.Int32 associationTypeID, int start, int pageLength)
		{
			int count =  -1;
			return GetByAssociationTypeID(null, associationTypeID, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_RelationshipType key.
		///		fKAllowedClassRelationshipRelationshipType Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="associationTypeID"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ClassAssociation> GetByAssociationTypeID(System.Int32 associationTypeID, int start, int pageLength,out int count)
		{
			return GetByAssociationTypeID(null, associationTypeID, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedClassRelationship_RelationshipType key.
		///		FK_AllowedClassRelationship_RelationshipType Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="associationTypeID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<ClassAssociation> GetByAssociationTypeID(TransactionManager transactionManager, System.Int32 associationTypeID, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ClassAssociation_Class key.
		///		FK_ClassAssociation_Class Description: 
		/// </summary>
		/// <param name="associationObjectClass"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ClassAssociation> GetByAssociationObjectClass(System.String associationObjectClass)
		{
			int count = -1;
			return GetByAssociationObjectClass(associationObjectClass, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ClassAssociation_Class key.
		///		FK_ClassAssociation_Class Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="associationObjectClass"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<ClassAssociation> GetByAssociationObjectClass(TransactionManager transactionManager, System.String associationObjectClass)
		{
			int count = -1;
			return GetByAssociationObjectClass(transactionManager, associationObjectClass, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_ClassAssociation_Class key.
		///		FK_ClassAssociation_Class Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="associationObjectClass"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ClassAssociation> GetByAssociationObjectClass(TransactionManager transactionManager, System.String associationObjectClass, int start, int pageLength)
		{
			int count = -1;
			return GetByAssociationObjectClass(transactionManager, associationObjectClass, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ClassAssociation_Class key.
		///		fKClassAssociationClass Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="associationObjectClass"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ClassAssociation> GetByAssociationObjectClass(System.String associationObjectClass, int start, int pageLength)
		{
			int count =  -1;
			return GetByAssociationObjectClass(null, associationObjectClass, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ClassAssociation_Class key.
		///		fKClassAssociationClass Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="associationObjectClass"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ClassAssociation> GetByAssociationObjectClass(System.String associationObjectClass, int start, int pageLength,out int count)
		{
			return GetByAssociationObjectClass(null, associationObjectClass, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ClassAssociation_Class key.
		///		FK_ClassAssociation_Class Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="associationObjectClass"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ClassAssociation objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<ClassAssociation> GetByAssociationObjectClass(TransactionManager transactionManager, System.String associationObjectClass, int start, int pageLength, out int count);
		
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
		/// <param name="cAid"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ClassAssociation"/> class.</returns>
		public MetaBuilder.BusinessLogic.ClassAssociation GetByCAid(System.Int32 cAid)
		{
			int count = -1;
			return GetByCAid(null,cAid, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ClassAssociation index.
		/// </summary>
		/// <param name="cAid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ClassAssociation"/> class.</returns>
		public MetaBuilder.BusinessLogic.ClassAssociation GetByCAid(System.Int32 cAid, int start, int pageLength)
		{
			int count = -1;
			return GetByCAid(null, cAid, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ClassAssociation index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="cAid"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ClassAssociation"/> class.</returns>
		public MetaBuilder.BusinessLogic.ClassAssociation GetByCAid(TransactionManager transactionManager, System.Int32 cAid)
		{
			int count = -1;
			return GetByCAid(transactionManager, cAid, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ClassAssociation index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="cAid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ClassAssociation"/> class.</returns>
		public MetaBuilder.BusinessLogic.ClassAssociation GetByCAid(TransactionManager transactionManager, System.Int32 cAid, int start, int pageLength)
		{
			int count = -1;
			return GetByCAid(transactionManager, cAid, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ClassAssociation index.
		/// </summary>
		/// <param name="cAid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ClassAssociation"/> class.</returns>
		public MetaBuilder.BusinessLogic.ClassAssociation GetByCAid(System.Int32 cAid, int start, int pageLength, out int count)
		{
			return GetByCAid(null, cAid, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ClassAssociation index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="cAid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ClassAssociation"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.ClassAssociation GetByCAid(TransactionManager transactionManager, System.Int32 cAid, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a MetaBuilder.BusinessLogic.TList&lt;ClassAssociation&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="MetaBuilder.BusinessLogic.TList&lt;ClassAssociation&gt;"/></returns>
		public static MetaBuilder.BusinessLogic.TList<ClassAssociation> Fill(IDataReader reader, MetaBuilder.BusinessLogic.TList<ClassAssociation> rows, int start, int pageLength)
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
				
				MetaBuilder.BusinessLogic.ClassAssociation c = null;
				if (DataRepository.Provider.UseEntityFactory)
				{
					key = @"ClassAssociation" 
							+ (reader.IsDBNull(reader.GetOrdinal("CAid"))?(int)0:(System.Int32)reader["CAid"]).ToString();

					c = EntityManager.LocateOrCreate<ClassAssociation>(
						key.ToString(), // EntityTrackingKey 
						"ClassAssociation",  //Creational Type
						DataRepository.Provider.EntityCreationalFactoryType,  //Factory used to create entity
						DataRepository.Provider.EnableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.ClassAssociation();
				}
				
				if (!DataRepository.Provider.EnableEntityTracking || c.EntityState == EntityState.Added)
                {
					c.SuppressEntityEvents = true;
					c.CAid = (System.Int32)reader["CAid"];
					c.ParentClass = (System.String)reader["ParentClass"];
					c.ChildClass = (System.String)reader["ChildClass"];
					c.AssociationTypeID = (System.Int32)reader["AssociationTypeID"];
					c.Caption = (reader.IsDBNull(reader.GetOrdinal("Caption")))?null:(System.String)reader["Caption"];
					c.AssociationObjectClass = (reader.IsDBNull(reader.GetOrdinal("AssociationObjectClass")))?null:(System.String)reader["AssociationObjectClass"];
					c.CopyIncluded = (System.Boolean)reader["CopyIncluded"];
					c.IsDefault = (System.Boolean)reader["IsDefault"];
					c.IsActive = (reader.IsDBNull(reader.GetOrdinal("IsActive")))?null:(System.Boolean?)reader["IsActive"];
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
			
			entity.CAid = (System.Int32)reader["CAid"];
			entity.ParentClass = (System.String)reader["ParentClass"];
			entity.ChildClass = (System.String)reader["ChildClass"];
			entity.AssociationTypeID = (System.Int32)reader["AssociationTypeID"];
			entity.Caption = (reader.IsDBNull(reader.GetOrdinal("Caption")))?null:(System.String)reader["Caption"];
			entity.AssociationObjectClass = (reader.IsDBNull(reader.GetOrdinal("AssociationObjectClass")))?null:(System.String)reader["AssociationObjectClass"];
			entity.CopyIncluded = (System.Boolean)reader["CopyIncluded"];
			entity.IsDefault = (System.Boolean)reader["IsDefault"];
			entity.IsActive = (reader.IsDBNull(reader.GetOrdinal("IsActive")))?null:(System.Boolean?)reader["IsActive"];
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
			entity.Caption = (Convert.IsDBNull(dataRow["Caption"]))?null:(System.String)dataRow["Caption"];
			entity.AssociationObjectClass = (Convert.IsDBNull(dataRow["AssociationObjectClass"]))?null:(System.String)dataRow["AssociationObjectClass"];
			entity.CopyIncluded = (System.Boolean)dataRow["CopyIncluded"];
			entity.IsDefault = (System.Boolean)dataRow["IsDefault"];
			entity.IsActive = (Convert.IsDBNull(dataRow["IsActive"]))?null:(System.Boolean?)dataRow["IsActive"];
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
		internal override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.ClassAssociation entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, ChildEntityTypesList innerList)
		{
			if(entity == null)
				return;

			#region ParentClassSource	
			if (CanDeepLoad(entity, "Class", "ParentClassSource", deepLoadType, innerList) 
				&& entity.ParentClassSource == null)
			{
				object[] pkItems = new object[1];
				pkItems[0] = entity.ParentClass;
				Class tmpEntity = EntityManager.LocateEntity<Class>(EntityLocator.ConstructKeyFromPkItems(typeof(Class), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.ParentClassSource = tmpEntity;
				else
					entity.ParentClassSource = DataRepository.ClassProvider.GetByName(entity.ParentClass);
			
				if (deep && entity.ParentClassSource != null)
				{
					DataRepository.ClassProvider.DeepLoad(transactionManager, entity.ParentClassSource, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion ParentClassSource

			#region ChildClassSource	
			if (CanDeepLoad(entity, "Class", "ChildClassSource", deepLoadType, innerList) 
				&& entity.ChildClassSource == null)
			{
				object[] pkItems = new object[1];
				pkItems[0] = entity.ChildClass;
				Class tmpEntity = EntityManager.LocateEntity<Class>(EntityLocator.ConstructKeyFromPkItems(typeof(Class), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.ChildClassSource = tmpEntity;
				else
					entity.ChildClassSource = DataRepository.ClassProvider.GetByName(entity.ChildClass);
			
				if (deep && entity.ChildClassSource != null)
				{
					DataRepository.ClassProvider.DeepLoad(transactionManager, entity.ChildClassSource, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion ChildClassSource

			#region AssociationTypeIDSource	
			if (CanDeepLoad(entity, "AssociationType", "AssociationTypeIDSource", deepLoadType, innerList) 
				&& entity.AssociationTypeIDSource == null)
			{
				object[] pkItems = new object[1];
				pkItems[0] = entity.AssociationTypeID;
				AssociationType tmpEntity = EntityManager.LocateEntity<AssociationType>(EntityLocator.ConstructKeyFromPkItems(typeof(AssociationType), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.AssociationTypeIDSource = tmpEntity;
				else
					entity.AssociationTypeIDSource = DataRepository.AssociationTypeProvider.GetByPkid(entity.AssociationTypeID);
			
				if (deep && entity.AssociationTypeIDSource != null)
				{
					DataRepository.AssociationTypeProvider.DeepLoad(transactionManager, entity.AssociationTypeIDSource, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion AssociationTypeIDSource

			#region AssociationObjectClassSource	
			if (CanDeepLoad(entity, "Class", "AssociationObjectClassSource", deepLoadType, innerList) 
				&& entity.AssociationObjectClassSource == null)
			{
				object[] pkItems = new object[1];
				pkItems[0] = (entity.AssociationObjectClass ?? string.Empty);
				Class tmpEntity = EntityManager.LocateEntity<Class>(EntityLocator.ConstructKeyFromPkItems(typeof(Class), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.AssociationObjectClassSource = tmpEntity;
				else
					entity.AssociationObjectClassSource = DataRepository.ClassProvider.GetByName((entity.AssociationObjectClass ?? string.Empty));
			
				if (deep && entity.AssociationObjectClassSource != null)
				{
					DataRepository.ClassProvider.DeepLoad(transactionManager, entity.AssociationObjectClassSource, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion AssociationObjectClassSource
			
			// Load Entity through Provider
			// Deep load child collections  - Call GetByCAid methods when available
			
			#region ObjectAssociationCollection
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<ObjectAssociation>", "ObjectAssociationCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'ObjectAssociationCollection' loaded.");
				#endif 

				entity.ObjectAssociationCollection = DataRepository.ObjectAssociationProvider.GetByCAid(transactionManager, entity.CAid);

				if (deep && entity.ObjectAssociationCollection.Count > 0)
				{
					DataRepository.ObjectAssociationProvider.DeepLoad(transactionManager, entity.ObjectAssociationCollection, deep, deepLoadType, childTypes, innerList);
				}
			}		
			#endregion 
			
			#region AllowedArtifactCollection
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<AllowedArtifact>", "AllowedArtifactCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'AllowedArtifactCollection' loaded.");
				#endif 

				entity.AllowedArtifactCollection = DataRepository.AllowedArtifactProvider.GetByCAid(transactionManager, entity.CAid);

				if (deep && entity.AllowedArtifactCollection.Count > 0)
				{
					DataRepository.AllowedArtifactProvider.DeepLoad(transactionManager, entity.AllowedArtifactCollection, deep, deepLoadType, childTypes, innerList);
				}
			}		
			#endregion 
			
			#region ClassCollection_From_AllowedArtifact
			// RelationshipType.ManyToMany
			if (CanDeepLoad(entity, "List<Class>", "ClassCollection_From_AllowedArtifact", deepLoadType, innerList))
			{
				entity.ClassCollection_From_AllowedArtifact = DataRepository.ClassProvider.GetByCAidFromAllowedArtifact(transactionManager, entity.CAid);			 
		
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'ClassCollection_From_AllowedArtifact' loaded.");
				#endif 

				if (deep && entity.ClassCollection_From_AllowedArtifact.Count > 0)
				{
					DataRepository.ClassProvider.DeepLoad(transactionManager, entity.ClassCollection_From_AllowedArtifact, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion
			
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
		internal override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.ClassAssociation entity, DeepSaveType deepSaveType, System.Type[] childTypes, ChildEntityTypesList innerList)
		{	
			if (entity == null)
				return false;
							
			#region Composite Parent Properties
			//Save Source Composite Properties, however, don't call deep save on them.  
			//So they only get saved a single level deep.
			
			#region ParentClassSource
			if (CanDeepSave(entity, "Class", "ParentClassSource", deepSaveType, innerList) 
				&& entity.ParentClassSource != null)
			{
				DataRepository.ClassProvider.Save(transactionManager, entity.ParentClassSource);
				entity.ParentClass = entity.ParentClassSource.Name;
			}
			#endregion 
			
			#region ChildClassSource
			if (CanDeepSave(entity, "Class", "ChildClassSource", deepSaveType, innerList) 
				&& entity.ChildClassSource != null)
			{
				DataRepository.ClassProvider.Save(transactionManager, entity.ChildClassSource);
				entity.ChildClass = entity.ChildClassSource.Name;
			}
			#endregion 
			
			#region AssociationTypeIDSource
			if (CanDeepSave(entity, "AssociationType", "AssociationTypeIDSource", deepSaveType, innerList) 
				&& entity.AssociationTypeIDSource != null)
			{
				DataRepository.AssociationTypeProvider.Save(transactionManager, entity.AssociationTypeIDSource);
				entity.AssociationTypeID = entity.AssociationTypeIDSource.Pkid;
			}
			#endregion 
			
			#region AssociationObjectClassSource
			if (CanDeepSave(entity, "Class", "AssociationObjectClassSource", deepSaveType, innerList) 
				&& entity.AssociationObjectClassSource != null)
			{
				DataRepository.ClassProvider.Save(transactionManager, entity.AssociationObjectClassSource);
				entity.AssociationObjectClass = entity.AssociationObjectClassSource.Name;
			}
			#endregion 
			#endregion Composite Parent Properties

			// Save Root Entity through Provider
			this.Save(transactionManager, entity);
			
			






			#region ClassCollection_From_AllowedArtifact>
			if (CanDeepSave(entity, "List<Class>", "ClassCollection_From_AllowedArtifact", deepSaveType, innerList))
			{
				if (entity.ClassCollection_From_AllowedArtifact.Count > 0 || entity.ClassCollection_From_AllowedArtifact.DeletedItems.Count > 0)
					DataRepository.ClassProvider.DeepSave(transactionManager, entity.ClassCollection_From_AllowedArtifact, deepSaveType, childTypes, innerList); 
			}
			#endregion 

			#region List<ObjectAssociation>
				if (CanDeepSave(entity, "List<ObjectAssociation>", "ObjectAssociationCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(ObjectAssociation child in entity.ObjectAssociationCollection)
					{
						child.CAid = entity.CAid;
					}
				
				if (entity.ObjectAssociationCollection.Count > 0 || entity.ObjectAssociationCollection.DeletedItems.Count > 0)
					DataRepository.ObjectAssociationProvider.DeepSave(transactionManager, entity.ObjectAssociationCollection, deepSaveType, childTypes, innerList);
				} 
			#endregion 
				

			#region List<AllowedArtifact>
				if (CanDeepSave(entity, "List<AllowedArtifact>", "AllowedArtifactCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(AllowedArtifact child in entity.AllowedArtifactCollection)
					{
						child.CAid = entity.CAid;
					}
				
				if (entity.AllowedArtifactCollection.Count > 0 || entity.AllowedArtifactCollection.DeletedItems.Count > 0)
					DataRepository.AllowedArtifactProvider.DeepSave(transactionManager, entity.AllowedArtifactCollection, deepSaveType, childTypes, innerList);
				} 
			#endregion 
				




						
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
		ClassCollection_From_AllowedArtifact,
	}
	
	#endregion ClassAssociationChildEntityTypes
	
	#region ClassAssociationFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
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
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
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
} // end namespace
