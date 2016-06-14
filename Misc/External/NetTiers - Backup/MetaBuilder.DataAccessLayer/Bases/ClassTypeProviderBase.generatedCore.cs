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
	/// This class is the base class for any <see cref="ClassTypeProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract partial class ClassTypeProviderBaseCore : EntityProviderBase<MetaBuilder.BusinessLogic.ClassType, MetaBuilder.BusinessLogic.ClassTypeKey>
	{		
		#region Get from Many To Many Relationship Functions
		#endregion	
		
		#region Delete Methods

		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager">A <see cref="TransactionManager"/> object.</param>
		/// <param name="key">The unique identifier of the row to delete.</param>
		/// <returns>Returns true if operation suceeded.</returns>
		public override bool Delete(TransactionManager transactionManager, MetaBuilder.BusinessLogic.ClassTypeKey key)
		{
			return Delete(transactionManager, key.ClassType);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="_classType">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(System.String _classType)
		{
			return Delete(null, _classType);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_classType">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(TransactionManager transactionManager, System.String _classType);		
		
		#endregion Delete Methods
		
		#region Get By Foreign Key Functions
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
		public override MetaBuilder.BusinessLogic.ClassType Get(TransactionManager transactionManager, MetaBuilder.BusinessLogic.ClassTypeKey key, int start, int pageLength)
		{
			return GetByClassType(transactionManager, key.ClassType, start, pageLength);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key PK_ClassType index.
		/// </summary>
		/// <param name="_classType"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ClassType"/> class.</returns>
		public MetaBuilder.BusinessLogic.ClassType GetByClassType(System.String _classType)
		{
			int count = -1;
			return GetByClassType(null,_classType, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ClassType index.
		/// </summary>
		/// <param name="_classType"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ClassType"/> class.</returns>
		public MetaBuilder.BusinessLogic.ClassType GetByClassType(System.String _classType, int start, int pageLength)
		{
			int count = -1;
			return GetByClassType(null, _classType, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ClassType index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_classType"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ClassType"/> class.</returns>
		public MetaBuilder.BusinessLogic.ClassType GetByClassType(TransactionManager transactionManager, System.String _classType)
		{
			int count = -1;
			return GetByClassType(transactionManager, _classType, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ClassType index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_classType"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ClassType"/> class.</returns>
		public MetaBuilder.BusinessLogic.ClassType GetByClassType(TransactionManager transactionManager, System.String _classType, int start, int pageLength)
		{
			int count = -1;
			return GetByClassType(transactionManager, _classType, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ClassType index.
		/// </summary>
		/// <param name="_classType"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ClassType"/> class.</returns>
		public MetaBuilder.BusinessLogic.ClassType GetByClassType(System.String _classType, int start, int pageLength, out int count)
		{
			return GetByClassType(null, _classType, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ClassType index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_classType"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ClassType"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.ClassType GetByClassType(TransactionManager transactionManager, System.String _classType, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a TList&lt;ClassType&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="TList&lt;ClassType&gt;"/></returns>
		public static TList<ClassType> Fill(IDataReader reader, TList<ClassType> rows, int start, int pageLength)
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
				
				MetaBuilder.BusinessLogic.ClassType c = null;
				if (useEntityFactory)
				{
					key = new System.Text.StringBuilder("ClassType")
					.Append("|").Append((System.String)reader[((int)ClassTypeColumn.ClassType - 1)]).ToString();
					c = EntityManager.LocateOrCreate<ClassType>(
					key.ToString(), // EntityTrackingKey
					"ClassType",  //Creational Type
					entityCreationFactoryType,  //Factory used to create entity
					enableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.ClassType();
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
					c.ClassType = (System.String)reader[((int)ClassTypeColumn.ClassType - 1)];
					c.OriginalClassType = c.ClassType;
					c.EntityTrackingKey = key;
					c.AcceptChanges();
					c.SuppressEntityEvents = false;
				}
				rows.Add(c);
			}
		return rows;
		}		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.ClassType"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.ClassType"/> object to refresh.</param>
		public static void RefreshEntity(IDataReader reader, MetaBuilder.BusinessLogic.ClassType entity)
		{
			if (!reader.Read()) return;
			
			entity.ClassType = (System.String)reader[((int)ClassTypeColumn.ClassType - 1)];
			entity.OriginalClassType = (System.String)reader["ClassType"];
			entity.AcceptChanges();
		}
		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.ClassType"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.ClassType"/> object.</param>
		public static void RefreshEntity(DataSet dataSet, MetaBuilder.BusinessLogic.ClassType entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.ClassType = (System.String)dataRow["ClassType"];
			entity.OriginalClassType = (System.String)dataRow["ClassType"];
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
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.ClassType"/> object to load.</param>
		/// <param name="deep">Boolean. A flag that indicates whether to recursively save all Property Collection that are descendants of this instance. If True, saves the complete object graph below this object. If False, saves this object only. </param>
		/// <param name="deepLoadType">DeepLoadType Enumeration to Include/Exclude object property collections from Load.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.ClassType Property Collection Type Array To Include or Exclude from Load</param>
		/// <param name="innerList">A collection of child types for easy access.</param>
	    /// <exception cref="ArgumentNullException">entity or childTypes is null.</exception>
	    /// <exception cref="ArgumentException">deepLoadType has invalid value.</exception>
		public override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.ClassType entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, DeepSession innerList)
		{
			if(entity == null)
				return;
			
			//used to hold DeepLoad method delegates and fire after all the local children have been loaded.
			Dictionary<string, KeyValuePair<Delegate, object>> deepHandles = new Dictionary<string, KeyValuePair<Delegate, object>>();
			// Deep load child collections  - Call GetByClassType methods when available
			
			#region ClassCollection
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<Class>|ClassCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'ClassCollection' loaded. key " + entity.EntityTrackingKey);
				#endif 

				entity.ClassCollection = DataRepository.ClassProvider.GetByClassType(transactionManager, entity.ClassType);

				if (deep && entity.ClassCollection.Count > 0)
				{
					deepHandles.Add("ClassCollection",
						new KeyValuePair<Delegate, object>((DeepLoadHandle<Class>) DataRepository.ClassProvider.DeepLoad,
						new object[] { transactionManager, entity.ClassCollection, deep, deepLoadType, childTypes, innerList }
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
		/// Deep Save the entire object graph of the MetaBuilder.BusinessLogic.ClassType object with criteria based of the child 
		/// Type property array and DeepSaveType.
		/// </summary>
		/// <param name="transactionManager">The transaction manager.</param>
		/// <param name="entity">MetaBuilder.BusinessLogic.ClassType instance</param>
		/// <param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.ClassType Property Collection Type Array To Include or Exclude from Save</param>
		/// <param name="innerList">A Hashtable of child types for easy access.</param>
		public override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.ClassType entity, DeepSaveType deepSaveType, System.Type[] childTypes, DeepSession innerList)
		{	
			if (entity == null)
				return false;
							
			#region Composite Parent Properties
			//Save Source Composite Properties, however, don't call deep save on them.  
			//So they only get saved a single level deep.
			#endregion Composite Parent Properties

			// Save Root Entity through Provider
			if (!entity.IsDeleted)
				this.Save(transactionManager, entity);
			
			//used to hold DeepSave method delegates and fire after all the local children have been saved.
			Dictionary<string, KeyValuePair<Delegate, object>> deepHandles = new Dictionary<string, KeyValuePair<Delegate, object>>();
	
			#region List<Class>
				if (CanDeepSave(entity.ClassCollection, "List<Class>|ClassCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(Class child in entity.ClassCollection)
					{
						if(child.ClassTypeSource != null)
						{
							child.ClassType = child.ClassTypeSource.ClassType;
						}
						else
						{
							child.ClassType = entity.ClassType;
						}

					}

					if (entity.ClassCollection.Count > 0 || entity.ClassCollection.DeletedItems.Count > 0)
					{
						//DataRepository.ClassProvider.Save(transactionManager, entity.ClassCollection);
						
						deepHandles.Add("ClassCollection",
						new KeyValuePair<Delegate, object>((DeepSaveHandle< Class >) DataRepository.ClassProvider.DeepSave,
							new object[] { transactionManager, entity.ClassCollection, deepSaveType, childTypes, innerList }
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
	
	#region ClassTypeChildEntityTypes
	
	///<summary>
	/// Enumeration used to expose the different child entity types 
	/// for child properties in <c>MetaBuilder.BusinessLogic.ClassType</c>
	///</summary>
	public enum ClassTypeChildEntityTypes
	{

		///<summary>
		/// Collection of <c>ClassType</c> as OneToMany for ClassCollection
		///</summary>
		[ChildEntityType(typeof(TList<Class>))]
		ClassCollection,
	}
	
	#endregion ClassTypeChildEntityTypes
	
	#region ClassTypeFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;ClassTypeColumn&gt;"/> class
	/// that is used exclusively with a <see cref="ClassType"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class ClassTypeFilterBuilder : SqlFilterBuilder<ClassTypeColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the ClassTypeFilterBuilder class.
		/// </summary>
		public ClassTypeFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the ClassTypeFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public ClassTypeFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the ClassTypeFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public ClassTypeFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion ClassTypeFilterBuilder
	
	#region ClassTypeParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;ClassTypeColumn&gt;"/> class
	/// that is used exclusively with a <see cref="ClassType"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class ClassTypeParameterBuilder : ParameterizedSqlFilterBuilder<ClassTypeColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the ClassTypeParameterBuilder class.
		/// </summary>
		public ClassTypeParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the ClassTypeParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public ClassTypeParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the ClassTypeParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public ClassTypeParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion ClassTypeParameterBuilder
	
	#region ClassTypeSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;ClassTypeColumn&gt;"/> class
	/// that is used exclusively with a <see cref="ClassType"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class ClassTypeSortBuilder : SqlSortBuilder<ClassTypeColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the ClassTypeSqlSortBuilder class.
		/// </summary>
		public ClassTypeSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion ClassTypeSortBuilder
	
} // end namespace
