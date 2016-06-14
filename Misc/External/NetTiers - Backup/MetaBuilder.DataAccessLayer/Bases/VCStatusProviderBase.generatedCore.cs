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
	/// This class is the base class for any <see cref="VCStatusProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract partial class VCStatusProviderBaseCore : EntityProviderBase<MetaBuilder.BusinessLogic.VCStatus, MetaBuilder.BusinessLogic.VCStatusKey>
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
		public override bool Delete(TransactionManager transactionManager, MetaBuilder.BusinessLogic.VCStatusKey key)
		{
			return Delete(transactionManager, key.pkid);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="_pkid">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(System.Int32 _pkid)
		{
			return Delete(null, _pkid);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_pkid">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(TransactionManager transactionManager, System.Int32 _pkid);		
		
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
		public override MetaBuilder.BusinessLogic.VCStatus Get(TransactionManager transactionManager, MetaBuilder.BusinessLogic.VCStatusKey key, int start, int pageLength)
		{
			return GetBypkid(transactionManager, key.pkid, start, pageLength);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key PK_GraphFileStatus index.
		/// </summary>
		/// <param name="_pkid"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.VCStatus"/> class.</returns>
		public MetaBuilder.BusinessLogic.VCStatus GetBypkid(System.Int32 _pkid)
		{
			int count = -1;
			return GetBypkid(null,_pkid, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_GraphFileStatus index.
		/// </summary>
		/// <param name="_pkid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.VCStatus"/> class.</returns>
		public MetaBuilder.BusinessLogic.VCStatus GetBypkid(System.Int32 _pkid, int start, int pageLength)
		{
			int count = -1;
			return GetBypkid(null, _pkid, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_GraphFileStatus index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_pkid"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.VCStatus"/> class.</returns>
		public MetaBuilder.BusinessLogic.VCStatus GetBypkid(TransactionManager transactionManager, System.Int32 _pkid)
		{
			int count = -1;
			return GetBypkid(transactionManager, _pkid, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_GraphFileStatus index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_pkid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.VCStatus"/> class.</returns>
		public MetaBuilder.BusinessLogic.VCStatus GetBypkid(TransactionManager transactionManager, System.Int32 _pkid, int start, int pageLength)
		{
			int count = -1;
			return GetBypkid(transactionManager, _pkid, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_GraphFileStatus index.
		/// </summary>
		/// <param name="_pkid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.VCStatus"/> class.</returns>
		public MetaBuilder.BusinessLogic.VCStatus GetBypkid(System.Int32 _pkid, int start, int pageLength, out int count)
		{
			return GetBypkid(null, _pkid, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_GraphFileStatus index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_pkid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.VCStatus"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.VCStatus GetBypkid(TransactionManager transactionManager, System.Int32 _pkid, int start, int pageLength, out int count);
						
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key IX_GraphFileStatus index.
		/// </summary>
		/// <param name="_name"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.VCStatus"/> class.</returns>
		public MetaBuilder.BusinessLogic.VCStatus GetByName(System.String _name)
		{
			int count = -1;
			return GetByName(null,_name, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the IX_GraphFileStatus index.
		/// </summary>
		/// <param name="_name"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.VCStatus"/> class.</returns>
		public MetaBuilder.BusinessLogic.VCStatus GetByName(System.String _name, int start, int pageLength)
		{
			int count = -1;
			return GetByName(null, _name, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the IX_GraphFileStatus index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_name"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.VCStatus"/> class.</returns>
		public MetaBuilder.BusinessLogic.VCStatus GetByName(TransactionManager transactionManager, System.String _name)
		{
			int count = -1;
			return GetByName(transactionManager, _name, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the IX_GraphFileStatus index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_name"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.VCStatus"/> class.</returns>
		public MetaBuilder.BusinessLogic.VCStatus GetByName(TransactionManager transactionManager, System.String _name, int start, int pageLength)
		{
			int count = -1;
			return GetByName(transactionManager, _name, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the IX_GraphFileStatus index.
		/// </summary>
		/// <param name="_name"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.VCStatus"/> class.</returns>
		public MetaBuilder.BusinessLogic.VCStatus GetByName(System.String _name, int start, int pageLength, out int count)
		{
			return GetByName(null, _name, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the IX_GraphFileStatus index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_name"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.VCStatus"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.VCStatus GetByName(TransactionManager transactionManager, System.String _name, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a TList&lt;VCStatus&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="TList&lt;VCStatus&gt;"/></returns>
		public static TList<VCStatus> Fill(IDataReader reader, TList<VCStatus> rows, int start, int pageLength)
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
				
				MetaBuilder.BusinessLogic.VCStatus c = null;
				if (useEntityFactory)
				{
					key = new System.Text.StringBuilder("VCStatus")
					.Append("|").Append((System.Int32)reader[((int)VCStatusColumn.pkid - 1)]).ToString();
					c = EntityManager.LocateOrCreate<VCStatus>(
					key.ToString(), // EntityTrackingKey
					"VCStatus",  //Creational Type
					entityCreationFactoryType,  //Factory used to create entity
					enableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.VCStatus();
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
					c.pkid = (System.Int32)reader[((int)VCStatusColumn.pkid - 1)];
					c.Originalpkid = c.pkid;
					c.Name = (System.String)reader[((int)VCStatusColumn.Name - 1)];
					c.EntityTrackingKey = key;
					c.AcceptChanges();
					c.SuppressEntityEvents = false;
				}
				rows.Add(c);
			}
		return rows;
		}		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.VCStatus"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.VCStatus"/> object to refresh.</param>
		public static void RefreshEntity(IDataReader reader, MetaBuilder.BusinessLogic.VCStatus entity)
		{
			if (!reader.Read()) return;
			
			entity.pkid = (System.Int32)reader[((int)VCStatusColumn.pkid - 1)];
			entity.Originalpkid = (System.Int32)reader["pkid"];
			entity.Name = (System.String)reader[((int)VCStatusColumn.Name - 1)];
			entity.AcceptChanges();
		}
		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.VCStatus"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.VCStatus"/> object.</param>
		public static void RefreshEntity(DataSet dataSet, MetaBuilder.BusinessLogic.VCStatus entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.pkid = (System.Int32)dataRow["pkid"];
			entity.Originalpkid = (System.Int32)dataRow["pkid"];
			entity.Name = (System.String)dataRow["Name"];
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
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.VCStatus"/> object to load.</param>
		/// <param name="deep">Boolean. A flag that indicates whether to recursively save all Property Collection that are descendants of this instance. If True, saves the complete object graph below this object. If False, saves this object only. </param>
		/// <param name="deepLoadType">DeepLoadType Enumeration to Include/Exclude object property collections from Load.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.VCStatus Property Collection Type Array To Include or Exclude from Load</param>
		/// <param name="innerList">A collection of child types for easy access.</param>
	    /// <exception cref="ArgumentNullException">entity or childTypes is null.</exception>
	    /// <exception cref="ArgumentException">deepLoadType has invalid value.</exception>
		public override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.VCStatus entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, DeepSession innerList)
		{
			if(entity == null)
				return;
			
			//used to hold DeepLoad method delegates and fire after all the local children have been loaded.
			Dictionary<string, KeyValuePair<Delegate, object>> deepHandles = new Dictionary<string, KeyValuePair<Delegate, object>>();
			// Deep load child collections  - Call GetBypkid methods when available
			
			#region GraphFileCollection
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<GraphFile>|GraphFileCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'GraphFileCollection' loaded. key " + entity.EntityTrackingKey);
				#endif 

				entity.GraphFileCollection = DataRepository.GraphFileProvider.GetByVCStatusID(transactionManager, entity.pkid);

				if (deep && entity.GraphFileCollection.Count > 0)
				{
					deepHandles.Add("GraphFileCollection",
						new KeyValuePair<Delegate, object>((DeepLoadHandle<GraphFile>) DataRepository.GraphFileProvider.DeepLoad,
						new object[] { transactionManager, entity.GraphFileCollection, deep, deepLoadType, childTypes, innerList }
					));
				}
			}		
			#endregion 
			
			
			#region ObjectAssociationCollection
			//Relationship Type One : Many
			if (CanDeepLoad(entity, "List<ObjectAssociation>|ObjectAssociationCollection", deepLoadType, innerList)) 
			{
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'ObjectAssociationCollection' loaded. key " + entity.EntityTrackingKey);
				#endif 

				entity.ObjectAssociationCollection = DataRepository.ObjectAssociationProvider.GetByVCStatusID(transactionManager, entity.pkid);

				if (deep && entity.ObjectAssociationCollection.Count > 0)
				{
					deepHandles.Add("ObjectAssociationCollection",
						new KeyValuePair<Delegate, object>((DeepLoadHandle<ObjectAssociation>) DataRepository.ObjectAssociationProvider.DeepLoad,
						new object[] { transactionManager, entity.ObjectAssociationCollection, deep, deepLoadType, childTypes, innerList }
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

				entity.MetaObjectCollection = DataRepository.MetaObjectProvider.GetByVCStatusID(transactionManager, entity.pkid);

				if (deep && entity.MetaObjectCollection.Count > 0)
				{
					deepHandles.Add("MetaObjectCollection",
						new KeyValuePair<Delegate, object>((DeepLoadHandle<MetaObject>) DataRepository.MetaObjectProvider.DeepLoad,
						new object[] { transactionManager, entity.MetaObjectCollection, deep, deepLoadType, childTypes, innerList }
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
		/// Deep Save the entire object graph of the MetaBuilder.BusinessLogic.VCStatus object with criteria based of the child 
		/// Type property array and DeepSaveType.
		/// </summary>
		/// <param name="transactionManager">The transaction manager.</param>
		/// <param name="entity">MetaBuilder.BusinessLogic.VCStatus instance</param>
		/// <param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.VCStatus Property Collection Type Array To Include or Exclude from Save</param>
		/// <param name="innerList">A Hashtable of child types for easy access.</param>
		public override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.VCStatus entity, DeepSaveType deepSaveType, System.Type[] childTypes, DeepSession innerList)
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
	
			#region List<GraphFile>
				if (CanDeepSave(entity.GraphFileCollection, "List<GraphFile>|GraphFileCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(GraphFile child in entity.GraphFileCollection)
					{
						if(child.VCStatusIDSource != null)
						{
							child.VCStatusID = child.VCStatusIDSource.pkid;
						}
						else
						{
							child.VCStatusID = entity.pkid;
						}

					}

					if (entity.GraphFileCollection.Count > 0 || entity.GraphFileCollection.DeletedItems.Count > 0)
					{
						//DataRepository.GraphFileProvider.Save(transactionManager, entity.GraphFileCollection);
						
						deepHandles.Add("GraphFileCollection",
						new KeyValuePair<Delegate, object>((DeepSaveHandle< GraphFile >) DataRepository.GraphFileProvider.DeepSave,
							new object[] { transactionManager, entity.GraphFileCollection, deepSaveType, childTypes, innerList }
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
				
	
			#region List<MetaObject>
				if (CanDeepSave(entity.MetaObjectCollection, "List<MetaObject>|MetaObjectCollection", deepSaveType, innerList)) 
				{	
					// update each child parent id with the real parent id (mostly used on insert)
					foreach(MetaObject child in entity.MetaObjectCollection)
					{
						if(child.VCStatusIDSource != null)
						{
							child.VCStatusID = child.VCStatusIDSource.pkid;
						}
						else
						{
							child.VCStatusID = entity.pkid;
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
	
	#region VCStatusChildEntityTypes
	
	///<summary>
	/// Enumeration used to expose the different child entity types 
	/// for child properties in <c>MetaBuilder.BusinessLogic.VCStatus</c>
	///</summary>
	public enum VCStatusChildEntityTypes
	{

		///<summary>
		/// Collection of <c>VCStatus</c> as OneToMany for GraphFileCollection
		///</summary>
		[ChildEntityType(typeof(TList<GraphFile>))]
		GraphFileCollection,

		///<summary>
		/// Collection of <c>VCStatus</c> as OneToMany for ObjectAssociationCollection
		///</summary>
		[ChildEntityType(typeof(TList<ObjectAssociation>))]
		ObjectAssociationCollection,

		///<summary>
		/// Collection of <c>VCStatus</c> as OneToMany for MetaObjectCollection
		///</summary>
		[ChildEntityType(typeof(TList<MetaObject>))]
		MetaObjectCollection,
	}
	
	#endregion VCStatusChildEntityTypes
	
	#region VCStatusFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;VCStatusColumn&gt;"/> class
	/// that is used exclusively with a <see cref="VCStatus"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class VCStatusFilterBuilder : SqlFilterBuilder<VCStatusColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the VCStatusFilterBuilder class.
		/// </summary>
		public VCStatusFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the VCStatusFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public VCStatusFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the VCStatusFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public VCStatusFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion VCStatusFilterBuilder
	
	#region VCStatusParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;VCStatusColumn&gt;"/> class
	/// that is used exclusively with a <see cref="VCStatus"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class VCStatusParameterBuilder : ParameterizedSqlFilterBuilder<VCStatusColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the VCStatusParameterBuilder class.
		/// </summary>
		public VCStatusParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the VCStatusParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public VCStatusParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the VCStatusParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public VCStatusParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion VCStatusParameterBuilder
	
	#region VCStatusSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;VCStatusColumn&gt;"/> class
	/// that is used exclusively with a <see cref="VCStatus"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class VCStatusSortBuilder : SqlSortBuilder<VCStatusColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the VCStatusSqlSortBuilder class.
		/// </summary>
		public VCStatusSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion VCStatusSortBuilder
	
} // end namespace
