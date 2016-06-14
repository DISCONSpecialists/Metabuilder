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
	/// This class is the base class for any <see cref="FieldProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract partial class FieldProviderBaseCore : EntityProviderBase<MetaBuilder.BusinessLogic.Field, MetaBuilder.BusinessLogic.FieldKey>
	{		
		#region Get from Many To Many Relationship Functions
		#region GetByObjectIDMachineIDFromObjectFieldValue
		
		/// <summary>
		///		Gets Field objects from the datasource by ObjectID in the
		///		ObjectFieldValue table. Table Field is related to table MetaObject
		///		through the (M:N) relationship defined in the ObjectFieldValue table.
		/// </summary>
		/// <param name="objectID"></param>
		/// <param name="machineID"></param>
		/// <returns>Returns a typed collection of Field objects.</returns>
		public TList<Field> GetByObjectIDMachineIDFromObjectFieldValue(System.Int32 objectID, System.String machineID)
		{
			int count = -1;
			return GetByObjectIDMachineIDFromObjectFieldValue(null,objectID, machineID, 0, int.MaxValue, out count);
			
		}
		
		/// <summary>
		///		Gets MetaBuilder.BusinessLogic.Field objects from the datasource by ObjectID in the
		///		ObjectFieldValue table. Table Field is related to table MetaObject
		///		through the (M:N) relationship defined in the ObjectFieldValue table.
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="objectID"></param>
		/// <param name="machineID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of Field objects.</returns>
		public TList<Field> GetByObjectIDMachineIDFromObjectFieldValue(System.Int32 objectID, System.String machineID, int start, int pageLength)
		{
			int count = -1;
			return GetByObjectIDMachineIDFromObjectFieldValue(null, objectID, machineID, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets Field objects from the datasource by ObjectID in the
		///		ObjectFieldValue table. Table Field is related to table MetaObject
		///		through the (M:N) relationship defined in the ObjectFieldValue table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="objectID"></param>
		/// <param name="machineID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of Field objects.</returns>
		public TList<Field> GetByObjectIDMachineIDFromObjectFieldValue(TransactionManager transactionManager, System.Int32 objectID, System.String machineID)
		{
			int count = -1;
			return GetByObjectIDMachineIDFromObjectFieldValue(transactionManager, objectID, machineID, 0, int.MaxValue, out count);
		}
		
		
		/// <summary>
		///		Gets Field objects from the datasource by ObjectID in the
		///		ObjectFieldValue table. Table Field is related to table MetaObject
		///		through the (M:N) relationship defined in the ObjectFieldValue table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="objectID"></param>
		/// <param name="machineID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of Field objects.</returns>
		public TList<Field> GetByObjectIDMachineIDFromObjectFieldValue(TransactionManager transactionManager, System.Int32 objectID, System.String machineID,int start, int pageLength)
		{
			int count = -1;
			return GetByObjectIDMachineIDFromObjectFieldValue(transactionManager, objectID, machineID, start, pageLength, out count);
		}
		
		/// <summary>
		///		Gets Field objects from the datasource by ObjectID in the
		///		ObjectFieldValue table. Table Field is related to table MetaObject
		///		through the (M:N) relationship defined in the ObjectFieldValue table.
		/// </summary>
		/// <param name="objectID"></param>
		/// <param name="machineID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of Field objects.</returns>
		public TList<Field> GetByObjectIDMachineIDFromObjectFieldValue(System.Int32 objectID, System.String machineID,int start, int pageLength, out int count)
		{
			
			return GetByObjectIDMachineIDFromObjectFieldValue(null, objectID, machineID, start, pageLength, out count);
		}


		/// <summary>
		///		Gets Field objects from the datasource by ObjectID in the
		///		ObjectFieldValue table. Table Field is related to table MetaObject
		///		through the (M:N) relationship defined in the ObjectFieldValue table.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <param name="objectID"></param>
		/// <param name="machineID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a TList of Field objects.</returns>
		public abstract TList<Field> GetByObjectIDMachineIDFromObjectFieldValue(TransactionManager transactionManager,System.Int32 objectID, System.String machineID, int start, int pageLength, out int count);
		
		#endregion GetByObjectIDMachineIDFromObjectFieldValue
		
		#endregion	
		
		#region Delete Methods

		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager">A <see cref="TransactionManager"/> object.</param>
		/// <param name="key">The unique identifier of the row to delete.</param>
		/// <returns>Returns true if operation suceeded.</returns>
		public override bool Delete(TransactionManager transactionManager, MetaBuilder.BusinessLogic.FieldKey key)
		{
			return Delete(transactionManager, key.Pkid);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="pkid">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(System.Int32 pkid)
		{
			return Delete(null, pkid);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="pkid">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(TransactionManager transactionManager, System.Int32 pkid);		
		
		#endregion Delete Methods
		
		#region Get By Foreign Key Functions
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Field_Class key.
		///		FK_Field_Class Description: 
		/// </summary>
		/// <param name="@class"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Field objects.</returns>
		public MetaBuilder.BusinessLogic.TList<Field> GetByClass(System.String @class)
		{
			int count = -1;
			return GetByClass(@class, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Field_Class key.
		///		FK_Field_Class Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="@class"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Field objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<Field> GetByClass(TransactionManager transactionManager, System.String @class)
		{
			int count = -1;
			return GetByClass(transactionManager, @class, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_Field_Class key.
		///		FK_Field_Class Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="@class"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Field objects.</returns>
		public MetaBuilder.BusinessLogic.TList<Field> GetByClass(TransactionManager transactionManager, System.String @class, int start, int pageLength)
		{
			int count = -1;
			return GetByClass(transactionManager, @class, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Field_Class key.
		///		fKFieldClass Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="@class"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Field objects.</returns>
		public MetaBuilder.BusinessLogic.TList<Field> GetByClass(System.String @class, int start, int pageLength)
		{
			int count =  -1;
			return GetByClass(null, @class, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Field_Class key.
		///		fKFieldClass Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="@class"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Field objects.</returns>
		public MetaBuilder.BusinessLogic.TList<Field> GetByClass(System.String @class, int start, int pageLength,out int count)
		{
			return GetByClass(null, @class, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_Field_Class key.
		///		FK_Field_Class Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="@class"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.Field objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<Field> GetByClass(TransactionManager transactionManager, System.String @class, int start, int pageLength, out int count);
		
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
		public override MetaBuilder.BusinessLogic.Field Get(TransactionManager transactionManager, MetaBuilder.BusinessLogic.FieldKey key, int start, int pageLength)
		{
			return GetByPkid(transactionManager, key.Pkid, start, pageLength);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key PK_Field index.
		/// </summary>
		/// <param name="pkid"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Field"/> class.</returns>
		public MetaBuilder.BusinessLogic.Field GetByPkid(System.Int32 pkid)
		{
			int count = -1;
			return GetByPkid(null,pkid, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Field index.
		/// </summary>
		/// <param name="pkid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Field"/> class.</returns>
		public MetaBuilder.BusinessLogic.Field GetByPkid(System.Int32 pkid, int start, int pageLength)
		{
			int count = -1;
			return GetByPkid(null, pkid, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Field index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="pkid"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Field"/> class.</returns>
		public MetaBuilder.BusinessLogic.Field GetByPkid(TransactionManager transactionManager, System.Int32 pkid)
		{
			int count = -1;
			return GetByPkid(transactionManager, pkid, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Field index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="pkid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Field"/> class.</returns>
		public MetaBuilder.BusinessLogic.Field GetByPkid(TransactionManager transactionManager, System.Int32 pkid, int start, int pageLength)
		{
			int count = -1;
			return GetByPkid(transactionManager, pkid, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Field index.
		/// </summary>
		/// <param name="pkid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Field"/> class.</returns>
		public MetaBuilder.BusinessLogic.Field GetByPkid(System.Int32 pkid, int start, int pageLength, out int count)
		{
			return GetByPkid(null, pkid, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_Field index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="pkid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.Field"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.Field GetByPkid(TransactionManager transactionManager, System.Int32 pkid, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a MetaBuilder.BusinessLogic.TList&lt;Field&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="MetaBuilder.BusinessLogic.TList&lt;Field&gt;"/></returns>
		public static MetaBuilder.BusinessLogic.TList<Field> Fill(IDataReader reader, MetaBuilder.BusinessLogic.TList<Field> rows, int start, int pageLength)
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
				
				MetaBuilder.BusinessLogic.Field c = null;
				if (DataRepository.Provider.UseEntityFactory)
				{
					key = @"Field" 
							+ (reader.IsDBNull(reader.GetOrdinal("pkid"))?(int)0:(System.Int32)reader["pkid"]).ToString();

					c = EntityManager.LocateOrCreate<Field>(
						key.ToString(), // EntityTrackingKey 
						"Field",  //Creational Type
						DataRepository.Provider.EntityCreationalFactoryType,  //Factory used to create entity
						DataRepository.Provider.EnableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.Field();
				}
				
				if (!DataRepository.Provider.EnableEntityTracking || c.EntityState == EntityState.Added)
                {
					c.SuppressEntityEvents = true;
					c.Pkid = (System.Int32)reader["pkid"];
					c.Class = (System.String)reader["Class"];
					c.Name = (System.String)reader["Name"];
					c.DataType = (System.String)reader["DataType"];
					c.Category = (System.String)reader["Category"];
					c.Description = (reader.IsDBNull(reader.GetOrdinal("Description")))?null:(System.String)reader["Description"];
					c.IsUnique = (reader.IsDBNull(reader.GetOrdinal("IsUnique")))?null:(System.Boolean?)reader["IsUnique"];
					c.SortOrder = (reader.IsDBNull(reader.GetOrdinal("SortOrder")))?null:(System.Int32?)reader["SortOrder"];
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
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.Field"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.Field"/> object to refresh.</param>
		public static void RefreshEntity(IDataReader reader, MetaBuilder.BusinessLogic.Field entity)
		{
			if (!reader.Read()) return;
			
			entity.Pkid = (System.Int32)reader["pkid"];
			entity.Class = (System.String)reader["Class"];
			entity.Name = (System.String)reader["Name"];
			entity.DataType = (System.String)reader["DataType"];
			entity.Category = (System.String)reader["Category"];
			entity.Description = (reader.IsDBNull(reader.GetOrdinal("Description")))?null:(System.String)reader["Description"];
			entity.IsUnique = (reader.IsDBNull(reader.GetOrdinal("IsUnique")))?null:(System.Boolean?)reader["IsUnique"];
			entity.SortOrder = (reader.IsDBNull(reader.GetOrdinal("SortOrder")))?null:(System.Int32?)reader["SortOrder"];
			entity.IsActive = (reader.IsDBNull(reader.GetOrdinal("IsActive")))?null:(System.Boolean?)reader["IsActive"];
			entity.AcceptChanges();
		}
		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.Field"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.Field"/> object.</param>
		public static void RefreshEntity(DataSet dataSet, MetaBuilder.BusinessLogic.Field entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.Pkid = (System.Int32)dataRow["pkid"];
			entity.Class = (System.String)dataRow["Class"];
			entity.Name = (System.String)dataRow["Name"];
			entity.DataType = (System.String)dataRow["DataType"];
			entity.Category = (System.String)dataRow["Category"];
			entity.Description = (Convert.IsDBNull(dataRow["Description"]))?null:(System.String)dataRow["Description"];
			entity.IsUnique = (Convert.IsDBNull(dataRow["IsUnique"]))?null:(System.Boolean?)dataRow["IsUnique"];
			entity.SortOrder = (Convert.IsDBNull(dataRow["SortOrder"]))?null:(System.Int32?)dataRow["SortOrder"];
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
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.Field"/> object to load.</param>
		/// <param name="deep">Boolean. A flag that indicates whether to recursively save all Property Collection that are descendants of this instance. If True, saves the complete object graph below this object. If False, saves this object only. </param>
		/// <param name="deepLoadType">DeepLoadType Enumeration to Include/Exclude object property collections from Load.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.Field Property Collection Type Array To Include or Exclude from Load</param>
		/// <param name="innerList">A collection of child types for easy access.</param>
	    /// <exception cref="ArgumentNullException">entity or childTypes is null.</exception>
	    /// <exception cref="ArgumentException">deepLoadType has invalid value.</exception>
		internal override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.Field entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, ChildEntityTypesList innerList)
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
			
			// Load Entity through Provider
			// Deep load child collections  - Call GetBypkid methods when available
			
			#region MetaObjectCollection_From_ObjectFieldValue
			// RelationshipType.ManyToMany
			if (CanDeepLoad(entity, "List<MetaObject>", "MetaObjectCollection_From_ObjectFieldValue", deepLoadType, innerList))
			{
				entity.MetaObjectCollection_From_ObjectFieldValue = DataRepository.MetaObjectProvider.GetByFieldIDFromObjectFieldValue(transactionManager, entity.Pkid);			 
		
				#if NETTIERS_DEBUG
				Debug.WriteLine("- property 'MetaObjectCollection_From_ObjectFieldValue' loaded.");
				#endif 

				if (deep && entity.MetaObjectCollection_From_ObjectFieldValue.Count > 0)
				{
					DataRepository.MetaObjectProvider.DeepLoad(transactionManager, entity.MetaObjectCollection_From_ObjectFieldValue, deep, deepLoadType, childTypes, innerList);
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

				entity.ObjectFieldValueCollection = DataRepository.ObjectFieldValueProvider.GetByFieldID(transactionManager, entity.Pkid);

				if (deep && entity.ObjectFieldValueCollection.Count > 0)
				{
					DataRepository.ObjectFieldValueProvider.DeepLoad(transactionManager, entity.ObjectFieldValueCollection, deep, deepLoadType, childTypes, innerList);
				}
			}		
			#endregion 
		}
		
		#endregion 
		
		#region DeepSave Methods

		/// <summary>
		/// Deep Save the entire object graph of the MetaBuilder.BusinessLogic.Field object with criteria based of the child 
		/// Type property array and DeepSaveType.
		/// </summary>
		/// <param name="transactionManager">The transaction manager.</param>
		/// <param name="entity">MetaBuilder.BusinessLogic.Field instance</param>
		/// <param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.Field Property Collection Type Array To Include or Exclude from Save</param>
		/// <param name="innerList">A Hashtable of child types for easy access.</param>
		internal override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.Field entity, DeepSaveType deepSaveType, System.Type[] childTypes, ChildEntityTypesList innerList)
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
			#endregion Composite Parent Properties

			// Save Root Entity through Provider
			this.Save(transactionManager, entity);
			
			



			#region MetaObjectCollection_From_ObjectFieldValue>
			if (CanDeepSave(entity, "List<MetaObject>", "MetaObjectCollection_From_ObjectFieldValue", deepSaveType, innerList))
			{
				if (entity.MetaObjectCollection_From_ObjectFieldValue.Count > 0 || entity.MetaObjectCollection_From_ObjectFieldValue.DeletedItems.Count > 0)
					DataRepository.MetaObjectProvider.DeepSave(transactionManager, entity.MetaObjectCollection_From_ObjectFieldValue, deepSaveType, childTypes, innerList); 
			}
			#endregion 



			#region List<ObjectFieldValue>
				if (CanDeepSave(entity, "List<ObjectFieldValue>", "ObjectFieldValueCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(ObjectFieldValue child in entity.ObjectFieldValueCollection)
					{
						child.FieldID = entity.Pkid;
					}
				
				if (entity.ObjectFieldValueCollection.Count > 0 || entity.ObjectFieldValueCollection.DeletedItems.Count > 0)
					DataRepository.ObjectFieldValueProvider.DeepSave(transactionManager, entity.ObjectFieldValueCollection, deepSaveType, childTypes, innerList);
				} 
			#endregion 
				


						
			return true;
		}
		#endregion
	} // end class
	
	#region FieldChildEntityTypes
	
	///<summary>
	/// Enumeration used to expose the different child entity types 
	/// for child properties in <c>MetaBuilder.BusinessLogic.Field</c>
	///</summary>
	public enum FieldChildEntityTypes
	{
		
		///<summary>
		/// Composite Property for <c>Class</c> at ClassSource
		///</summary>
		[ChildEntityType(typeof(Class))]
		Class,
	
		///<summary>
		/// Collection of <c>Field</c> as ManyToMany for MetaObjectCollection_From_ObjectFieldValue
		///</summary>
		[ChildEntityType(typeof(TList<MetaObject>))]
		MetaObjectCollection_From_ObjectFieldValue,

		///<summary>
		/// Collection of <c>Field</c> as OneToMany for ObjectFieldValueCollection
		///</summary>
		[ChildEntityType(typeof(TList<ObjectFieldValue>))]
		ObjectFieldValueCollection,
	}
	
	#endregion FieldChildEntityTypes
	
	#region FieldFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="Field"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class FieldFilterBuilder : SqlFilterBuilder<FieldColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the FieldFilterBuilder class.
		/// </summary>
		public FieldFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the FieldFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public FieldFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the FieldFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public FieldFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion FieldFilterBuilder
	
	#region FieldParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="Field"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class FieldParameterBuilder : ParameterizedSqlFilterBuilder<FieldColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the FieldParameterBuilder class.
		/// </summary>
		public FieldParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the FieldParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public FieldParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the FieldParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public FieldParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion FieldParameterBuilder
} // end namespace
