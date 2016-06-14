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
	/// This class is the base class for any <see cref="ClassProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract partial class ClassProviderBaseCore : EntityProviderBase<MetaBuilder.BusinessLogic.Class, MetaBuilder.BusinessLogic.ClassKey>
	{		
		#region Get from Many To Many Relationship Functions
		#region GetByCAidFromAllowedArtifact
		
		/// <summary>
		///		Gets Class objects from the datasource by CAid in the
		///		AllowedArtifact table. Table Class is related to table ClassAssociation
		///		through the (M:N) relationship defined in the AllowedArtifact table.
		/// </summary>
		/// <param name="cAid"></param>
		/// <returns>Returns a typed collection of Class objects.</returns>
		public TList<Class> GetByCAidFromAllowedArtifact(System.Int32 cAid)
		{
			int count = -1;
			return GetByCAidFromAllowedArtifact(null,cAid, 0, int.MaxValue, out count);
			
		}
		
		/// <summary>
		///		Gets MetaBuilder.BusinessLogic.Class objects from the datasource by CAid in the
		///		AllowedArtifact table. Table Class is related to table ClassAssociation
		///		through the (M:N) relationship defined in the AllowedArtifact table.
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="cAid"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of Class objects.</returns>
		public TList<Class> GetByCAidFromAllowedArtifact(System.Int32 cAid, int start, int pageLength)
		{
			int count = -1;
			return GetByCAidFromAllowedArtifact(null, cAid, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets Class objects from the datasource by CAid in the
		///		AllowedArtifact table. Table Class is related to table ClassAssociation
		///		through the (M:N) relationship defined in the AllowedArtifact table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="cAid"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of Class objects.</returns>
		public TList<Class> GetByCAidFromAllowedArtifact(TransactionManager transactionManager, System.Int32 cAid)
		{
			int count = -1;
			return GetByCAidFromAllowedArtifact(transactionManager, cAid, 0, int.MaxValue, out count);
		}
		
		
		/// <summary>
		///		Gets Class objects from the datasource by CAid in the
		///		AllowedArtifact table. Table Class is related to table ClassAssociation
		///		through the (M:N) relationship defined in the AllowedArtifact table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="cAid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of Class objects.</returns>
		public TList<Class> GetByCAidFromAllowedArtifact(TransactionManager transactionManager, System.Int32 cAid,int start, int pageLength)
		{
			int count = -1;
			return GetByCAidFromAllowedArtifact(transactionManager, cAid, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets Class objects from the datasource by CAid in the
		///		AllowedArtifact table. Table Class is related to table ClassAssociation
		///		through the (M:N) relationship defined in the AllowedArtifact table.
		/// </summary>
		/// <param name="cAid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of Class objects.</returns>
		public TList<Class> GetByCAidFromAllowedArtifact(System.Int32 cAid,int start, int pageLength, out int count)
		{
			
			return GetByCAidFromAllowedArtifact(null, cAid, start, pageLength, out count);
		}


		/// <summary>
		///		Gets Class objects from the datasource by CAid in the
		///		AllowedArtifact table. Table Class is related to table ClassAssociation
		///		through the (M:N) relationship defined in the AllowedArtifact table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <param name="cAid"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of Class objects.</returns>
		public abstract TList<Class> GetByCAidFromAllowedArtifact(TransactionManager transactionManager,System.Int32 cAid, int start, int pageLength, out int count);
		
		#endregion GetByCAidFromAllowedArtifact
		
		#endregion	
		
		#region Delete Methods

		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager">A <see cref="TransactionManager"/> object.</param>
		/// <param name="key">The unique identifier of the row to delete.</param>
		/// <returns>Returns true if operation suceeded.</returns>
		public override bool Delete(TransactionManager transactionManager, MetaBuilder.BusinessLogic.ClassKey key)
		{
			return Delete(transactionManager, key.Name);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="name">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(System.String name)
		{
			return Delete(null, name);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="name">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(TransactionManager transactionManager, System.String name);		
		
		#endregion Delete Methods
		
		#region Get By Foreign Key Functions
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Class_ClassType key.
		///		FK_Class_ClassType Description: 
		/// </summary>
		/// <param name="classType"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Class objects.</returns>
		public MetaBuilder.BusinessLogic.TList<Class> GetByClassType(System.String classType)
		{
			int count = -1;
			return GetByClassType(classType, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Class_ClassType key.
		///		FK_Class_ClassType Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="classType"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Class objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<Class> GetByClassType(TransactionManager transactionManager, System.String classType)
		{
			int count = -1;
			return GetByClassType(transactionManager, classType, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_Class_ClassType key.
		///		FK_Class_ClassType Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="classType"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Class objects.</returns>
		public MetaBuilder.BusinessLogic.TList<Class> GetByClassType(TransactionManager transactionManager, System.String classType, int start, int pageLength)
		{
			int count = -1;
			return GetByClassType(transactionManager, classType, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Class_ClassType key.
		///		fKClassClassType Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="classType"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Class objects.</returns>
		public MetaBuilder.BusinessLogic.TList<Class> GetByClassType(System.String classType, int start, int pageLength)
		{
			int count =  -1;
			return GetByClassType(null, classType, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Class_ClassType key.
		///		fKClassClassType Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="classType"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Class objects.</returns>
		public MetaBuilder.BusinessLogic.TList<Class> GetByClassType(System.String classType, int start, int pageLength,out int count)
		{
			return GetByClassType(null, classType, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Class_ClassType key.
		///		FK_Class_ClassType Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="classType"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Class objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<Class> GetByClassType(TransactionManager transactionManager, System.String classType, int start, int pageLength, out int count);
		
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
		public override MetaBuilder.BusinessLogic.Class Get(TransactionManager transactionManager, MetaBuilder.BusinessLogic.ClassKey key, int start, int pageLength)
		{
			return GetByName(transactionManager, key.Name, start, pageLength);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key PK_Class index.
		/// </summary>
		/// <param name="name"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Class"/> class.</returns>
		public MetaBuilder.BusinessLogic.Class GetByName(System.String name)
		{
			int count = -1;
			return GetByName(null,name, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Class index.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Class"/> class.</returns>
		public MetaBuilder.BusinessLogic.Class GetByName(System.String name, int start, int pageLength)
		{
			int count = -1;
			return GetByName(null, name, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Class index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="name"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Class"/> class.</returns>
		public MetaBuilder.BusinessLogic.Class GetByName(TransactionManager transactionManager, System.String name)
		{
			int count = -1;
			return GetByName(transactionManager, name, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Class index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="name"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Class"/> class.</returns>
		public MetaBuilder.BusinessLogic.Class GetByName(TransactionManager transactionManager, System.String name, int start, int pageLength)
		{
			int count = -1;
			return GetByName(transactionManager, name, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Class index.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Class"/> class.</returns>
		public MetaBuilder.BusinessLogic.Class GetByName(System.String name, int start, int pageLength, out int count)
		{
			return GetByName(null, name, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Class index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="name"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Class"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.Class GetByName(TransactionManager transactionManager, System.String name, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a MetaBuilder.BusinessLogic.TList&lt;Class&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="MetaBuilder.BusinessLogic.TList&lt;Class&gt;"/></returns>
		public static MetaBuilder.BusinessLogic.TList<Class> Fill(IDataReader reader, MetaBuilder.BusinessLogic.TList<Class> rows, int start, int pageLength)
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
				
				MetaBuilder.BusinessLogic.Class c = null;
				if (DataRepository.Provider.UseEntityFactory)
				{
					key = @"Class" 
							+ (reader.IsDBNull(reader.GetOrdinal("Name"))?string.Empty:(System.String)reader["Name"]).ToString();

					c = EntityManager.LocateOrCreate<Class>(
						key.ToString(), // EntityTrackingKey 
						"Class",  //Creational Type
						DataRepository.Provider.EntityCreationalFactoryType,  //Factory used to create entity
						DataRepository.Provider.EnableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.Class();
				}
				
				if (!DataRepository.Provider.EnableEntityTracking || c.EntityState == EntityState.Added)
                {
					c.SuppressEntityEvents = true;
					c.Name = (System.String)reader["Name"];
					c.OriginalName = c.Name; //(reader.IsDBNull(reader.GetOrdinal("Name")))?string.Empty:(System.String)reader["Name"];
					c.DescriptionCode = (reader.IsDBNull(reader.GetOrdinal("DescriptionCode")))?null:(System.String)reader["DescriptionCode"];
					c.Category = (reader.IsDBNull(reader.GetOrdinal("Category")))?null:(System.String)reader["Category"];
					c.ClassType = (reader.IsDBNull(reader.GetOrdinal("ClassType")))?null:(System.String)reader["ClassType"];
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
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.Class"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.Class"/> object to refresh.</param>
		public static void RefreshEntity(IDataReader reader, MetaBuilder.BusinessLogic.Class entity)
		{
			if (!reader.Read()) return;
			
			entity.Name = (System.String)reader["Name"];
			entity.OriginalName = (System.String)reader["Name"];
			entity.DescriptionCode = (reader.IsDBNull(reader.GetOrdinal("DescriptionCode")))?null:(System.String)reader["DescriptionCode"];
			entity.Category = (reader.IsDBNull(reader.GetOrdinal("Category")))?null:(System.String)reader["Category"];
			entity.ClassType = (reader.IsDBNull(reader.GetOrdinal("ClassType")))?null:(System.String)reader["ClassType"];
			entity.IsActive = (reader.IsDBNull(reader.GetOrdinal("IsActive")))?null:(System.Boolean?)reader["IsActive"];
			entity.AcceptChanges();
		}
		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.Class"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.Class"/> object.</param>
		public static void RefreshEntity(DataSet dataSet, MetaBuilder.BusinessLogic.Class entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.Name = (System.String)dataRow["Name"];
			entity.OriginalName = (System.String)dataRow["Name"];
			entity.DescriptionCode = (Convert.IsDBNull(dataRow["DescriptionCode"]))?null:(System.String)dataRow["DescriptionCode"];
			entity.Category = (Convert.IsDBNull(dataRow["Category"]))?null:(System.String)dataRow["Category"];
			entity.ClassType = (Convert.IsDBNull(dataRow["ClassType"]))?null:(System.String)dataRow["ClassType"];
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
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.Class"/> object to load.</param>
		/// <param name="deep">Boolean. A flag that indicates whether to recursively save all Property Collection that are descendants of this instance. If True, saves the complete object graph below this object. If False, saves this object only. </param>
		/// <param name="deepLoadType">DeepLoadType Enumeration to Include/Exclude object property collections from Load.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.Class Property Collection Type Array To Include or Exclude from Load</param>
		/// <param name="innerList">A collection of child types for easy access.</param>
	    /// <exception cref="ArgumentNullException">entity or childTypes is null.</exception>
	    /// <exception cref="ArgumentException">deepLoadType has invalid value.</exception>
		internal override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.Class entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, ChildEntityTypesList innerList)
		{
			if(entity == null)
				return;

			#region ClassTypeSource	
			if (CanDeepLoad(entity, "ClassType", "ClassTypeSource", deepLoadType, innerList) 
				&& entity.ClassTypeSource == null)
			{
				object[] pkItems = new object[1];
				pkItems[0] = (entity.ClassType ?? string.Empty);
				ClassType tmpEntity = EntityManager.LocateEntity<ClassType>(EntityLocator.ConstructKeyFromPkItems(typeof(ClassType), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.ClassTypeSource = tmpEntity;
				else
					entity.ClassTypeSource = DataRepository.ClassTypeProvider.GetByClassType((entity.ClassType ?? string.Empty));
			
				if (deep && entity.ClassTypeSource != null)
				{
					DataRepository.ClassTypeProvider.DeepLoad(transactionManager, entity.ClassTypeSource, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion ClassTypeSource
			
			// Load Entity through Provider
			// Deep load child collections  - Call GetByName methods when available
			
			#region FieldCollection
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<Field>", "FieldCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'FieldCollection' loaded.");
				#endif 

				entity.FieldCollection = DataRepository.FieldProvider.GetByClass(transactionManager, entity.Name);

				if (deep && entity.FieldCollection.Count > 0)
				{
					DataRepository.FieldProvider.DeepLoad(transactionManager, entity.FieldCollection, deep, deepLoadType, childTypes, innerList);
				}
			}		
			#endregion 
			
			#region ClassAssociationCollectionByParentClass
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<ClassAssociation>", "ClassAssociationCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'ClassAssociationCollectionByParentClass' loaded.");
				#endif 

				entity.ClassAssociationCollectionByParentClass = DataRepository.ClassAssociationProvider.GetByParentClass(transactionManager, entity.Name);

				if (deep && entity.ClassAssociationCollectionByParentClass.Count > 0)
				{
					DataRepository.ClassAssociationProvider.DeepLoad(transactionManager, entity.ClassAssociationCollectionByParentClass, deep, deepLoadType, childTypes, innerList);
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

				entity.AllowedArtifactCollection = DataRepository.AllowedArtifactProvider.GetByClass(transactionManager, entity.Name);

				if (deep && entity.AllowedArtifactCollection.Count > 0)
				{
					DataRepository.AllowedArtifactProvider.DeepLoad(transactionManager, entity.AllowedArtifactCollection, deep, deepLoadType, childTypes, innerList);
				}
			}		
			#endregion 
			
			#region ClassAssociationCollectionByChildClass
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<ClassAssociation>", "ClassAssociationCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'ClassAssociationCollectionByChildClass' loaded.");
				#endif 

				entity.ClassAssociationCollectionByChildClass = DataRepository.ClassAssociationProvider.GetByChildClass(transactionManager, entity.Name);

				if (deep && entity.ClassAssociationCollectionByChildClass.Count > 0)
				{
					DataRepository.ClassAssociationProvider.DeepLoad(transactionManager, entity.ClassAssociationCollectionByChildClass, deep, deepLoadType, childTypes, innerList);
				}
			}		
			#endregion 
			
			#region ClassAssociationCollectionByAssociationObjectClass
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<ClassAssociation>", "ClassAssociationCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'ClassAssociationCollectionByAssociationObjectClass' loaded.");
				#endif 

				entity.ClassAssociationCollectionByAssociationObjectClass = DataRepository.ClassAssociationProvider.GetByAssociationObjectClass(transactionManager, entity.Name);

				if (deep && entity.ClassAssociationCollectionByAssociationObjectClass.Count > 0)
				{
					DataRepository.ClassAssociationProvider.DeepLoad(transactionManager, entity.ClassAssociationCollectionByAssociationObjectClass, deep, deepLoadType, childTypes, innerList);
				}
			}		
			#endregion 
			
			#region MetaObjectCollection
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<MetaObject>", "MetaObjectCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'MetaObjectCollection' loaded.");
				#endif 

				entity.MetaObjectCollection = DataRepository.MetaObjectProvider.GetByClass(transactionManager, entity.Name);

				if (deep && entity.MetaObjectCollection.Count > 0)
				{
					DataRepository.MetaObjectProvider.DeepLoad(transactionManager, entity.MetaObjectCollection, deep, deepLoadType, childTypes, innerList);
				}
			}		
			#endregion 
			
			#region ClassAssociationCollection_From_AllowedArtifact
			// RelationshipType.ManyToMany
			if (CanDeepLoad(entity, "List<ClassAssociation>", "ClassAssociationCollection_From_AllowedArtifact", deepLoadType, innerList))
			{
				entity.ClassAssociationCollection_From_AllowedArtifact = DataRepository.ClassAssociationProvider.GetByClassFromAllowedArtifact(transactionManager, entity.Name);			 
		
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'ClassAssociationCollection_From_AllowedArtifact' loaded.");
				#endif 

				if (deep && entity.ClassAssociationCollection_From_AllowedArtifact.Count > 0)
				{
					DataRepository.ClassAssociationProvider.DeepLoad(transactionManager, entity.ClassAssociationCollection_From_AllowedArtifact, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion
			
		}
		
		#endregion 
		
		#region DeepSave Methods

		/// <summary>
		/// Deep Save the entire object graph of the MetaBuilder.BusinessLogic.Class object with criteria based of the child 
		/// Type property array and DeepSaveType.
		/// </summary>
		/// <param name="transactionManager">The transaction manager.</param>
		/// <param name="entity">MetaBuilder.BusinessLogic.Class instance</param>
		/// <param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.Class Property Collection Type Array To Include or Exclude from Save</param>
		/// <param name="innerList">A Hashtable of child types for easy access.</param>
		internal override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.Class entity, DeepSaveType deepSaveType, System.Type[] childTypes, ChildEntityTypesList innerList)
		{	
			if (entity == null)
				return false;
							
			#region Composite Parent Properties
			//Save Source Composite Properties, however, don't call deep save on them.  
			//So they only get saved a single level deep.
			
			#region ClassTypeSource
			if (CanDeepSave(entity, "ClassType", "ClassTypeSource", deepSaveType, innerList) 
				&& entity.ClassTypeSource != null)
			{
				DataRepository.ClassTypeProvider.Save(transactionManager, entity.ClassTypeSource);
				entity.ClassType = entity.ClassTypeSource.ClassType;
			}
			#endregion 
			#endregion Composite Parent Properties

			// Save Root Entity through Provider
			this.Save(transactionManager, entity);
			
			














			#region ClassAssociationCollection_From_AllowedArtifact>
			if (CanDeepSave(entity, "List<ClassAssociation>", "ClassAssociationCollection_From_AllowedArtifact", deepSaveType, innerList))
			{
				if (entity.ClassAssociationCollection_From_AllowedArtifact.Count > 0 || entity.ClassAssociationCollection_From_AllowedArtifact.DeletedItems.Count > 0)
					DataRepository.ClassAssociationProvider.DeepSave(transactionManager, entity.ClassAssociationCollection_From_AllowedArtifact, deepSaveType, childTypes, innerList); 
			}
			#endregion 

			#region List<Field>
				if (CanDeepSave(entity, "List<Field>", "FieldCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(Field child in entity.FieldCollection)
					{
						child.Class = entity.Name;
					}
				
				if (entity.FieldCollection.Count > 0 || entity.FieldCollection.DeletedItems.Count > 0)
					DataRepository.FieldProvider.DeepSave(transactionManager, entity.FieldCollection, deepSaveType, childTypes, innerList);
				} 
			#endregion 
				

			#region List<ClassAssociation>
				if (CanDeepSave(entity, "List<ClassAssociation>", "ClassAssociationCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(ClassAssociation child in entity.ClassAssociationCollectionByParentClass)
					{
						child.ParentClass = entity.Name;
					}
				
				if (entity.ClassAssociationCollectionByParentClass.Count > 0 || entity.ClassAssociationCollectionByParentClass.DeletedItems.Count > 0)
					DataRepository.ClassAssociationProvider.DeepSave(transactionManager, entity.ClassAssociationCollectionByParentClass, deepSaveType, childTypes, innerList);
				} 
			#endregion 
				

			#region List<AllowedArtifact>
				if (CanDeepSave(entity, "List<AllowedArtifact>", "AllowedArtifactCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(AllowedArtifact child in entity.AllowedArtifactCollection)
					{
						child.Class = entity.Name;
					}
				
				if (entity.AllowedArtifactCollection.Count > 0 || entity.AllowedArtifactCollection.DeletedItems.Count > 0)
					DataRepository.AllowedArtifactProvider.DeepSave(transactionManager, entity.AllowedArtifactCollection, deepSaveType, childTypes, innerList);
				} 
			#endregion 
				

			#region List<ClassAssociation>
				if (CanDeepSave(entity, "List<ClassAssociation>", "ClassAssociationCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(ClassAssociation child in entity.ClassAssociationCollectionByChildClass)
					{
						child.ChildClass = entity.Name;
					}
				
				if (entity.ClassAssociationCollectionByChildClass.Count > 0 || entity.ClassAssociationCollectionByChildClass.DeletedItems.Count > 0)
					DataRepository.ClassAssociationProvider.DeepSave(transactionManager, entity.ClassAssociationCollectionByChildClass, deepSaveType, childTypes, innerList);
				} 
			#endregion 
				

			#region List<ClassAssociation>
				if (CanDeepSave(entity, "List<ClassAssociation>", "ClassAssociationCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(ClassAssociation child in entity.ClassAssociationCollectionByAssociationObjectClass)
					{
						child.AssociationObjectClass = entity.Name;
					}
				
				if (entity.ClassAssociationCollectionByAssociationObjectClass.Count > 0 || entity.ClassAssociationCollectionByAssociationObjectClass.DeletedItems.Count > 0)
					DataRepository.ClassAssociationProvider.DeepSave(transactionManager, entity.ClassAssociationCollectionByAssociationObjectClass, deepSaveType, childTypes, innerList);
				} 
			#endregion 
				

			#region List<MetaObject>
				if (CanDeepSave(entity, "List<MetaObject>", "MetaObjectCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(MetaObject child in entity.MetaObjectCollection)
					{
						child.Class = entity.Name;
					}
				
				if (entity.MetaObjectCollection.Count > 0 || entity.MetaObjectCollection.DeletedItems.Count > 0)
					DataRepository.MetaObjectProvider.DeepSave(transactionManager, entity.MetaObjectCollection, deepSaveType, childTypes, innerList);
				} 
			#endregion 
				








						
			return true;
		}
		#endregion
	} // end class
	
	#region ClassChildEntityTypes
	
	///<summary>
	/// Enumeration used to expose the different child entity types 
	/// for child properties in <c>MetaBuilder.BusinessLogic.Class</c>
	///</summary>
	public enum ClassChildEntityTypes
	{
		
		///<summary>
		/// Composite Property for <c>ClassType</c> at ClassTypeSource
		///</summary>
		[ChildEntityType(typeof(ClassType))]
		ClassType,
	
		///<summary>
		/// Collection of <c>Class</c> as OneToMany for FieldCollection
		///</summary>
		[ChildEntityType(typeof(TList<Field>))]
		FieldCollection,

		///<summary>
		/// Collection of <c>Class</c> as OneToMany for ClassAssociationCollection
		///</summary>
		[ChildEntityType(typeof(TList<ClassAssociation>))]
		ClassAssociationCollectionByParentClass,

		///<summary>
		/// Collection of <c>Class</c> as OneToMany for AllowedArtifactCollection
		///</summary>
		[ChildEntityType(typeof(TList<AllowedArtifact>))]
		AllowedArtifactCollection,

		///<summary>
		/// Collection of <c>Class</c> as OneToMany for ClassAssociationCollection
		///</summary>
		[ChildEntityType(typeof(TList<ClassAssociation>))]
		ClassAssociationCollectionByChildClass,

		///<summary>
		/// Collection of <c>Class</c> as OneToMany for ClassAssociationCollection
		///</summary>
		[ChildEntityType(typeof(TList<ClassAssociation>))]
		ClassAssociationCollectionByAssociationObjectClass,

		///<summary>
		/// Collection of <c>Class</c> as OneToMany for MetaObjectCollection
		///</summary>
		[ChildEntityType(typeof(TList<MetaObject>))]
		MetaObjectCollection,

		///<summary>
		/// Collection of <c>Class</c> as ManyToMany for ClassAssociationCollection_From_AllowedArtifact
		///</summary>
		[ChildEntityType(typeof(TList<ClassAssociation>))]
		ClassAssociationCollection_From_AllowedArtifact,
	}
	
	#endregion ClassChildEntityTypes
	
	#region ClassFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="Class"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class ClassFilterBuilder : SqlFilterBuilder<ClassColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the ClassFilterBuilder class.
		/// </summary>
		public ClassFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the ClassFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public ClassFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the ClassFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public ClassFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion ClassFilterBuilder
	
	#region ClassParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="Class"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class ClassParameterBuilder : ParameterizedSqlFilterBuilder<ClassColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the ClassParameterBuilder class.
		/// </summary>
		public ClassParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the ClassParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public ClassParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the ClassParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public ClassParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion ClassParameterBuilder
} // end namespace
