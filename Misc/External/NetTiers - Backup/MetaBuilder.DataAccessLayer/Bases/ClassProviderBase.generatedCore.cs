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
		/// <param name="_cAid"></param>
		/// <returns>Returns a typed collection of Class objects.</returns>
		public TList<Class> GetByCAidFromAllowedArtifact(System.Int32 _cAid)
		{
			int count = -1;
			return GetByCAidFromAllowedArtifact(null,_cAid, 0, int.MaxValue, out count);
			
		}
		
		/// <summary>
		///		Gets MetaBuilder.BusinessLogic.Class objects from the datasource by CAid in the
		///		AllowedArtifact table. Table Class is related to table ClassAssociation
		///		through the (M:N) relationship defined in the AllowedArtifact table.
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_cAid"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of Class objects.</returns>
		public TList<Class> GetByCAidFromAllowedArtifact(System.Int32 _cAid, int start, int pageLength)
		{
			int count = -1;
			return GetByCAidFromAllowedArtifact(null, _cAid, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets Class objects from the datasource by CAid in the
		///		AllowedArtifact table. Table Class is related to table ClassAssociation
		///		through the (M:N) relationship defined in the AllowedArtifact table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_cAid"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of Class objects.</returns>
		public TList<Class> GetByCAidFromAllowedArtifact(TransactionManager transactionManager, System.Int32 _cAid)
		{
			int count = -1;
			return GetByCAidFromAllowedArtifact(transactionManager, _cAid, 0, int.MaxValue, out count);
		}
		
		
		/// <summary>
		///		Gets Class objects from the datasource by CAid in the
		///		AllowedArtifact table. Table Class is related to table ClassAssociation
		///		through the (M:N) relationship defined in the AllowedArtifact table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_cAid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of Class objects.</returns>
		public TList<Class> GetByCAidFromAllowedArtifact(TransactionManager transactionManager, System.Int32 _cAid,int start, int pageLength)
		{
			int count = -1;
			return GetByCAidFromAllowedArtifact(transactionManager, _cAid, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets Class objects from the datasource by CAid in the
		///		AllowedArtifact table. Table Class is related to table ClassAssociation
		///		through the (M:N) relationship defined in the AllowedArtifact table.
		/// </summary>
		/// <param name="_cAid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of Class objects.</returns>
		public TList<Class> GetByCAidFromAllowedArtifact(System.Int32 _cAid,int start, int pageLength, out int count)
		{
			
			return GetByCAidFromAllowedArtifact(null, _cAid, start, pageLength, out count);
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
		/// <param name="_cAid"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of Class objects.</returns>
		public abstract TList<Class> GetByCAidFromAllowedArtifact(TransactionManager transactionManager,System.Int32 _cAid, int start, int pageLength, out int count);
		
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
		/// <param name="_name">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(System.String _name)
		{
			return Delete(null, _name);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_name">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(TransactionManager transactionManager, System.String _name);		
		
		#endregion Delete Methods
		
		#region Get By Foreign Key Functions
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Class_ClassType key.
		///		FK_Class_ClassType Description: 
		/// </summary>
		/// <param name="_classType"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Class objects.</returns>
		public TList<Class> GetByClassType(System.String _classType)
		{
			int count = -1;
			return GetByClassType(_classType, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Class_ClassType key.
		///		FK_Class_ClassType Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_classType"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Class objects.</returns>
		/// <remarks></remarks>
		public TList<Class> GetByClassType(TransactionManager transactionManager, System.String _classType)
		{
			int count = -1;
			return GetByClassType(transactionManager, _classType, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_Class_ClassType key.
		///		FK_Class_ClassType Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_classType"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Class objects.</returns>
		public TList<Class> GetByClassType(TransactionManager transactionManager, System.String _classType, int start, int pageLength)
		{
			int count = -1;
			return GetByClassType(transactionManager, _classType, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Class_ClassType key.
		///		fK_Class_ClassType Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_classType"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Class objects.</returns>
		public TList<Class> GetByClassType(System.String _classType, int start, int pageLength)
		{
			int count =  -1;
			return GetByClassType(null, _classType, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Class_ClassType key.
		///		fK_Class_ClassType Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_classType"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Class objects.</returns>
		public TList<Class> GetByClassType(System.String _classType, int start, int pageLength,out int count)
		{
			return GetByClassType(null, _classType, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Class_ClassType key.
		///		FK_Class_ClassType Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_classType"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Class objects.</returns>
		public abstract TList<Class> GetByClassType(TransactionManager transactionManager, System.String _classType, int start, int pageLength, out int count);
		
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
		/// <param name="_name"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Class"/> class.</returns>
		public MetaBuilder.BusinessLogic.Class GetByName(System.String _name)
		{
			int count = -1;
			return GetByName(null,_name, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Class index.
		/// </summary>
		/// <param name="_name"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Class"/> class.</returns>
		public MetaBuilder.BusinessLogic.Class GetByName(System.String _name, int start, int pageLength)
		{
			int count = -1;
			return GetByName(null, _name, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Class index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_name"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Class"/> class.</returns>
		public MetaBuilder.BusinessLogic.Class GetByName(TransactionManager transactionManager, System.String _name)
		{
			int count = -1;
			return GetByName(transactionManager, _name, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Class index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_name"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Class"/> class.</returns>
		public MetaBuilder.BusinessLogic.Class GetByName(TransactionManager transactionManager, System.String _name, int start, int pageLength)
		{
			int count = -1;
			return GetByName(transactionManager, _name, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Class index.
		/// </summary>
		/// <param name="_name"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Class"/> class.</returns>
		public MetaBuilder.BusinessLogic.Class GetByName(System.String _name, int start, int pageLength, out int count)
		{
			return GetByName(null, _name, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Class index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_name"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Class"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.Class GetByName(TransactionManager transactionManager, System.String _name, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a TList&lt;Class&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="TList&lt;Class&gt;"/></returns>
		public static TList<Class> Fill(IDataReader reader, TList<Class> rows, int start, int pageLength)
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
				
				MetaBuilder.BusinessLogic.Class c = null;
				if (useEntityFactory)
				{
					key = new System.Text.StringBuilder("Class")
					.Append("|").Append((System.String)reader[((int)ClassColumn.Name - 1)]).ToString();
					c = EntityManager.LocateOrCreate<Class>(
					key.ToString(), // EntityTrackingKey
					"Class",  //Creational Type
					entityCreationFactoryType,  //Factory used to create entity
					enableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.Class();
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
					c.Name = (System.String)reader[((int)ClassColumn.Name - 1)];
					c.OriginalName = c.Name;
					c.DescriptionCode = (reader.IsDBNull(((int)ClassColumn.DescriptionCode - 1)))?null:(System.String)reader[((int)ClassColumn.DescriptionCode - 1)];
					c.Category = (reader.IsDBNull(((int)ClassColumn.Category - 1)))?null:(System.String)reader[((int)ClassColumn.Category - 1)];
					c.ClassType = (reader.IsDBNull(((int)ClassColumn.ClassType - 1)))?null:(System.String)reader[((int)ClassColumn.ClassType - 1)];
					c.IsActive = (reader.IsDBNull(((int)ClassColumn.IsActive - 1)))?null:(System.Boolean?)reader[((int)ClassColumn.IsActive - 1)];
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
			
			entity.Name = (System.String)reader[((int)ClassColumn.Name - 1)];
			entity.OriginalName = (System.String)reader["Name"];
			entity.DescriptionCode = (reader.IsDBNull(((int)ClassColumn.DescriptionCode - 1)))?null:(System.String)reader[((int)ClassColumn.DescriptionCode - 1)];
			entity.Category = (reader.IsDBNull(((int)ClassColumn.Category - 1)))?null:(System.String)reader[((int)ClassColumn.Category - 1)];
			entity.ClassType = (reader.IsDBNull(((int)ClassColumn.ClassType - 1)))?null:(System.String)reader[((int)ClassColumn.ClassType - 1)];
			entity.IsActive = (reader.IsDBNull(((int)ClassColumn.IsActive - 1)))?null:(System.Boolean?)reader[((int)ClassColumn.IsActive - 1)];
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
			entity.DescriptionCode = Convert.IsDBNull(dataRow["DescriptionCode"]) ? null : (System.String)dataRow["DescriptionCode"];
			entity.Category = Convert.IsDBNull(dataRow["Category"]) ? null : (System.String)dataRow["Category"];
			entity.ClassType = Convert.IsDBNull(dataRow["ClassType"]) ? null : (System.String)dataRow["ClassType"];
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
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.Class"/> object to load.</param>
		/// <param name="deep">Boolean. A flag that indicates whether to recursively save all Property Collection that are descendants of this instance. If True, saves the complete object graph below this object. If False, saves this object only. </param>
		/// <param name="deepLoadType">DeepLoadType Enumeration to Include/Exclude object property collections from Load.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.Class Property Collection Type Array To Include or Exclude from Load</param>
		/// <param name="innerList">A collection of child types for easy access.</param>
	    /// <exception cref="ArgumentNullException">entity or childTypes is null.</exception>
	    /// <exception cref="ArgumentException">deepLoadType has invalid value.</exception>
		public override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.Class entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, DeepSession innerList)
		{
			if(entity == null)
				return;

			#region ClassTypeSource	
			if (CanDeepLoad(entity, "ClassType|ClassTypeSource", deepLoadType, innerList) 
				&& entity.ClassTypeSource == null)
			{
				object[] pkItems = new object[1];
				pkItems[0] = (entity.ClassType ?? string.Empty);
				ClassType tmpEntity = EntityManager.LocateEntity<ClassType>(EntityLocator.ConstructKeyFromPkItems(typeof(ClassType), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.ClassTypeSource = tmpEntity;
				else
					entity.ClassTypeSource = DataRepository.ClassTypeProvider.GetByClassType(transactionManager, (entity.ClassType ?? string.Empty));		
				
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'ClassTypeSource' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.ClassTypeSource != null)
				{
					innerList.SkipChildren = true;
					DataRepository.ClassTypeProvider.DeepLoad(transactionManager, entity.ClassTypeSource, deep, deepLoadType, childTypes, innerList);
					innerList.SkipChildren = false;
				}
					
			}
			#endregion ClassTypeSource
			
			//used to hold DeepLoad method delegates and fire after all the local children have been loaded.
			Dictionary<string, KeyValuePair<Delegate, object>> deepHandles = new Dictionary<string, KeyValuePair<Delegate, object>>();
			// Deep load child collections  - Call GetByName methods when available
			
			#region FieldCollection
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<Field>|FieldCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'FieldCollection' loaded. key " + entity.EntityTrackingKey);
				#endif 

				entity.FieldCollection = DataRepository.FieldProvider.GetByClass(transactionManager, entity.Name);

				if (deep && entity.FieldCollection.Count > 0)
				{
					deepHandles.Add("FieldCollection",
						new KeyValuePair<Delegate, object>((DeepLoadHandle<Field>) DataRepository.FieldProvider.DeepLoad,
						new object[] { transactionManager, entity.FieldCollection, deep, deepLoadType, childTypes, innerList }
					));
				}
			}		
			#endregion 
			
			
			#region ClassAssociationCollectionGetByParentClass
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<ClassAssociation>|ClassAssociationCollectionGetByParentClass", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'ClassAssociationCollectionGetByParentClass' loaded. key " + entity.EntityTrackingKey);
				#endif 

				entity.ClassAssociationCollectionGetByParentClass = DataRepository.ClassAssociationProvider.GetByParentClass(transactionManager, entity.Name);

				if (deep && entity.ClassAssociationCollectionGetByParentClass.Count > 0)
				{
					deepHandles.Add("ClassAssociationCollectionGetByParentClass",
						new KeyValuePair<Delegate, object>((DeepLoadHandle<ClassAssociation>) DataRepository.ClassAssociationProvider.DeepLoad,
						new object[] { transactionManager, entity.ClassAssociationCollectionGetByParentClass, deep, deepLoadType, childTypes, innerList }
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

				entity.AllowedArtifactCollection = DataRepository.AllowedArtifactProvider.GetByClass(transactionManager, entity.Name);

				if (deep && entity.AllowedArtifactCollection.Count > 0)
				{
					deepHandles.Add("AllowedArtifactCollection",
						new KeyValuePair<Delegate, object>((DeepLoadHandle<AllowedArtifact>) DataRepository.AllowedArtifactProvider.DeepLoad,
						new object[] { transactionManager, entity.AllowedArtifactCollection, deep, deepLoadType, childTypes, innerList }
					));
				}
			}		
			#endregion 
			
			
			#region CAidClassAssociationCollection_From_AllowedArtifact
			// RelationshipType.ManyToMany
			if (CanDeepLoad(entity, "List<ClassAssociation>|CAidClassAssociationCollection_From_AllowedArtifact", deepLoadType, innerList))
			{
				entity.CAidClassAssociationCollection_From_AllowedArtifact = DataRepository.ClassAssociationProvider.GetByClassFromAllowedArtifact(transactionManager, entity.Name);			 
		
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'CAidClassAssociationCollection_From_AllowedArtifact' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.CAidClassAssociationCollection_From_AllowedArtifact != null)
				{
					deepHandles.Add("CAidClassAssociationCollection_From_AllowedArtifact",
						new KeyValuePair<Delegate, object>((DeepLoadHandle< ClassAssociation >) DataRepository.ClassAssociationProvider.DeepLoad,
						new object[] { transactionManager, entity.CAidClassAssociationCollection_From_AllowedArtifact, deep, deepLoadType, childTypes, innerList }
					));
				}
			}
			#endregion
			
			
			
			#region MetaObjectCollection
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<MetaObject>|MetaObjectCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'MetaObjectCollection' loaded. key " + entity.EntityTrackingKey);
				#endif 

				entity.MetaObjectCollection = DataRepository.MetaObjectProvider.GetByClass(transactionManager, entity.Name);

				if (deep && entity.MetaObjectCollection.Count > 0)
				{
					deepHandles.Add("MetaObjectCollection",
						new KeyValuePair<Delegate, object>((DeepLoadHandle<MetaObject>) DataRepository.MetaObjectProvider.DeepLoad,
						new object[] { transactionManager, entity.MetaObjectCollection, deep, deepLoadType, childTypes, innerList }
					));
				}
			}		
			#endregion 
			
			
			#region ClassAssociationCollectionGetByChildClass
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<ClassAssociation>|ClassAssociationCollectionGetByChildClass", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'ClassAssociationCollectionGetByChildClass' loaded. key " + entity.EntityTrackingKey);
				#endif 

				entity.ClassAssociationCollectionGetByChildClass = DataRepository.ClassAssociationProvider.GetByChildClass(transactionManager, entity.Name);

				if (deep && entity.ClassAssociationCollectionGetByChildClass.Count > 0)
				{
					deepHandles.Add("ClassAssociationCollectionGetByChildClass",
						new KeyValuePair<Delegate, object>((DeepLoadHandle<ClassAssociation>) DataRepository.ClassAssociationProvider.DeepLoad,
						new object[] { transactionManager, entity.ClassAssociationCollectionGetByChildClass, deep, deepLoadType, childTypes, innerList }
					));
				}
			}		
			#endregion 
			
			
			#region ClassAssociationCollectionGetByAssociationObjectClass
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<ClassAssociation>|ClassAssociationCollectionGetByAssociationObjectClass", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'ClassAssociationCollectionGetByAssociationObjectClass' loaded. key " + entity.EntityTrackingKey);
				#endif 

				entity.ClassAssociationCollectionGetByAssociationObjectClass = DataRepository.ClassAssociationProvider.GetByAssociationObjectClass(transactionManager, entity.Name);

				if (deep && entity.ClassAssociationCollectionGetByAssociationObjectClass.Count > 0)
				{
					deepHandles.Add("ClassAssociationCollectionGetByAssociationObjectClass",
						new KeyValuePair<Delegate, object>((DeepLoadHandle<ClassAssociation>) DataRepository.ClassAssociationProvider.DeepLoad,
						new object[] { transactionManager, entity.ClassAssociationCollectionGetByAssociationObjectClass, deep, deepLoadType, childTypes, innerList }
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
		/// Deep Save the entire object graph of the MetaBuilder.BusinessLogic.Class object with criteria based of the child 
		/// Type property array and DeepSaveType.
		/// </summary>
		/// <param name="transactionManager">The transaction manager.</param>
		/// <param name="entity">MetaBuilder.BusinessLogic.Class instance</param>
		/// <param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.Class Property Collection Type Array To Include or Exclude from Save</param>
		/// <param name="innerList">A Hashtable of child types for easy access.</param>
		public override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.Class entity, DeepSaveType deepSaveType, System.Type[] childTypes, DeepSession innerList)
		{	
			if (entity == null)
				return false;
							
			#region Composite Parent Properties
			//Save Source Composite Properties, however, don't call deep save on them.  
			//So they only get saved a single level deep.
			
			#region ClassTypeSource
			if (CanDeepSave(entity, "ClassType|ClassTypeSource", deepSaveType, innerList) 
				&& entity.ClassTypeSource != null)
			{
				DataRepository.ClassTypeProvider.Save(transactionManager, entity.ClassTypeSource);
				entity.ClassType = entity.ClassTypeSource.ClassType;
			}
			#endregion 
			#endregion Composite Parent Properties

			// Save Root Entity through Provider
			if (!entity.IsDeleted)
				this.Save(transactionManager, entity);
			
			//used to hold DeepSave method delegates and fire after all the local children have been saved.
			Dictionary<string, KeyValuePair<Delegate, object>> deepHandles = new Dictionary<string, KeyValuePair<Delegate, object>>();

			#region CAidClassAssociationCollection_From_AllowedArtifact>
			if (CanDeepSave(entity.CAidClassAssociationCollection_From_AllowedArtifact, "List<ClassAssociation>|CAidClassAssociationCollection_From_AllowedArtifact", deepSaveType, innerList))
			{
				if (entity.CAidClassAssociationCollection_From_AllowedArtifact.Count > 0 || entity.CAidClassAssociationCollection_From_AllowedArtifact.DeletedItems.Count > 0)
				{
					DataRepository.ClassAssociationProvider.Save(transactionManager, entity.CAidClassAssociationCollection_From_AllowedArtifact); 
					deepHandles.Add("CAidClassAssociationCollection_From_AllowedArtifact",
						new KeyValuePair<Delegate, object>((DeepSaveHandle<ClassAssociation>) DataRepository.ClassAssociationProvider.DeepSave,
						new object[] { transactionManager, entity.CAidClassAssociationCollection_From_AllowedArtifact, deepSaveType, childTypes, innerList }
					));
				}
			}
			#endregion 
	
			#region List<Field>
				if (CanDeepSave(entity.FieldCollection, "List<Field>|FieldCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(Field child in entity.FieldCollection)
					{
						if(child.ClassSource != null)
						{
							child.Class = child.ClassSource.Name;
						}
						else
						{
							child.Class = entity.Name;
						}

					}

					if (entity.FieldCollection.Count > 0 || entity.FieldCollection.DeletedItems.Count > 0)
					{
						//DataRepository.FieldProvider.Save(transactionManager, entity.FieldCollection);
						
						deepHandles.Add("FieldCollection",
						new KeyValuePair<Delegate, object>((DeepSaveHandle< Field >) DataRepository.FieldProvider.DeepSave,
							new object[] { transactionManager, entity.FieldCollection, deepSaveType, childTypes, innerList }
						));
					}
				} 
			#endregion 
				
	
			#region List<ClassAssociation>
				if (CanDeepSave(entity.ClassAssociationCollectionGetByParentClass, "List<ClassAssociation>|ClassAssociationCollectionGetByParentClass", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(ClassAssociation child in entity.ClassAssociationCollectionGetByParentClass)
					{
						if(child.ParentClassSource != null)
						{
							child.ParentClass = child.ParentClassSource.Name;
						}
						else
						{
							child.ParentClass = entity.Name;
						}

					}

					if (entity.ClassAssociationCollectionGetByParentClass.Count > 0 || entity.ClassAssociationCollectionGetByParentClass.DeletedItems.Count > 0)
					{
						//DataRepository.ClassAssociationProvider.Save(transactionManager, entity.ClassAssociationCollectionGetByParentClass);
						
						deepHandles.Add("ClassAssociationCollectionGetByParentClass",
						new KeyValuePair<Delegate, object>((DeepSaveHandle< ClassAssociation >) DataRepository.ClassAssociationProvider.DeepSave,
							new object[] { transactionManager, entity.ClassAssociationCollectionGetByParentClass, deepSaveType, childTypes, innerList }
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
						if(child.ClassSource != null)
						{
								child.Class = child.ClassSource.Name;
						}

						if(child.CAidSource != null)
						{
								child.CAid = child.CAidSource.CAid;
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
				
	
			#region List<MetaObject>
				if (CanDeepSave(entity.MetaObjectCollection, "List<MetaObject>|MetaObjectCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(MetaObject child in entity.MetaObjectCollection)
					{
						if(child.ClassSource != null)
						{
							child.Class = child.ClassSource.Name;
						}
						else
						{
							child.Class = entity.Name;
						}

					}

					if (entity.MetaObjectCollection.Count > 0 || entity.MetaObjectCollection.DeletedItems.Count > 0)
					{
						//DataRepository.MetaObjectProvider.Save(transactionManager, entity.MetaObjectCollection);
						
						deepHandles.Add("MetaObjectCollection",
						new KeyValuePair<Delegate, object>((DeepSaveHandle< MetaObject >) DataRepository.MetaObjectProvider.DeepSave,
							new object[] { transactionManager, entity.MetaObjectCollection, deepSaveType, childTypes, innerList }
						));
					}
				} 
			#endregion 
				
	
			#region List<ClassAssociation>
				if (CanDeepSave(entity.ClassAssociationCollectionGetByChildClass, "List<ClassAssociation>|ClassAssociationCollectionGetByChildClass", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(ClassAssociation child in entity.ClassAssociationCollectionGetByChildClass)
					{
						if(child.ChildClassSource != null)
						{
							child.ChildClass = child.ChildClassSource.Name;
						}
						else
						{
							child.ChildClass = entity.Name;
						}

					}

					if (entity.ClassAssociationCollectionGetByChildClass.Count > 0 || entity.ClassAssociationCollectionGetByChildClass.DeletedItems.Count > 0)
					{
						//DataRepository.ClassAssociationProvider.Save(transactionManager, entity.ClassAssociationCollectionGetByChildClass);
						
						deepHandles.Add("ClassAssociationCollectionGetByChildClass",
						new KeyValuePair<Delegate, object>((DeepSaveHandle< ClassAssociation >) DataRepository.ClassAssociationProvider.DeepSave,
							new object[] { transactionManager, entity.ClassAssociationCollectionGetByChildClass, deepSaveType, childTypes, innerList }
						));
					}
				} 
			#endregion 
				
	
			#region List<ClassAssociation>
				if (CanDeepSave(entity.ClassAssociationCollectionGetByAssociationObjectClass, "List<ClassAssociation>|ClassAssociationCollectionGetByAssociationObjectClass", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(ClassAssociation child in entity.ClassAssociationCollectionGetByAssociationObjectClass)
					{
						if(child.AssociationObjectClassSource != null)
						{
							child.AssociationObjectClass = child.AssociationObjectClassSource.Name;
						}
						else
						{
							child.AssociationObjectClass = entity.Name;
						}

					}

					if (entity.ClassAssociationCollectionGetByAssociationObjectClass.Count > 0 || entity.ClassAssociationCollectionGetByAssociationObjectClass.DeletedItems.Count > 0)
					{
						//DataRepository.ClassAssociationProvider.Save(transactionManager, entity.ClassAssociationCollectionGetByAssociationObjectClass);
						
						deepHandles.Add("ClassAssociationCollectionGetByAssociationObjectClass",
						new KeyValuePair<Delegate, object>((DeepSaveHandle< ClassAssociation >) DataRepository.ClassAssociationProvider.DeepSave,
							new object[] { transactionManager, entity.ClassAssociationCollectionGetByAssociationObjectClass, deepSaveType, childTypes, innerList }
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
		ClassAssociationCollectionGetByParentClass,

		///<summary>
		/// Collection of <c>Class</c> as OneToMany for AllowedArtifactCollection
		///</summary>
		[ChildEntityType(typeof(TList<AllowedArtifact>))]
		AllowedArtifactCollection,

		///<summary>
		/// Collection of <c>Class</c> as ManyToMany for ClassAssociationCollection_From_AllowedArtifact
		///</summary>
		[ChildEntityType(typeof(TList<ClassAssociation>))]
		CAidClassAssociationCollection_From_AllowedArtifact,

		///<summary>
		/// Collection of <c>Class</c> as OneToMany for MetaObjectCollection
		///</summary>
		[ChildEntityType(typeof(TList<MetaObject>))]
		MetaObjectCollection,

		///<summary>
		/// Collection of <c>Class</c> as OneToMany for ClassAssociationCollection
		///</summary>
		[ChildEntityType(typeof(TList<ClassAssociation>))]
		ClassAssociationCollectionGetByChildClass,

		///<summary>
		/// Collection of <c>Class</c> as OneToMany for ClassAssociationCollection
		///</summary>
		[ChildEntityType(typeof(TList<ClassAssociation>))]
		ClassAssociationCollectionGetByAssociationObjectClass,
	}
	
	#endregion ClassChildEntityTypes
	
	#region ClassFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;ClassColumn&gt;"/> class
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
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;ClassColumn&gt;"/> class
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
	
	#region ClassSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;ClassColumn&gt;"/> class
	/// that is used exclusively with a <see cref="Class"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class ClassSortBuilder : SqlSortBuilder<ClassColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the ClassSqlSortBuilder class.
		/// </summary>
		public ClassSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion ClassSortBuilder
	
} // end namespace
