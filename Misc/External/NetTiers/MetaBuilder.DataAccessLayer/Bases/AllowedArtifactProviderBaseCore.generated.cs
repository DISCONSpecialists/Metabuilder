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
	/// This class is the base class for any <see cref="AllowedArtifactProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract partial class AllowedArtifactProviderBaseCore : EntityProviderBase<MetaBuilder.BusinessLogic.AllowedArtifact, MetaBuilder.BusinessLogic.AllowedArtifactKey>
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
		public override bool Delete(TransactionManager transactionManager, MetaBuilder.BusinessLogic.AllowedArtifactKey key)
		{
			return Delete(transactionManager, key.CAid, key.Class);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="cAid">. Primary Key.</param>
		/// <param name="@class">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(System.Int32 cAid, System.String @class)
		{
			return Delete(null, cAid, @class);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="cAid">. Primary Key.</param>
		/// <param name="@class">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(TransactionManager transactionManager, System.Int32 cAid, System.String @class);		
		
		#endregion Delete Methods
		
		#region Get By Foreign Key Functions
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedArtifact_Class key.
		///		FK_AllowedArtifact_Class Description: 
		/// </summary>
		/// <param name="@class"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.AllowedArtifact objects.</returns>
		public MetaBuilder.BusinessLogic.TList<AllowedArtifact> GetByClass(System.String @class)
		{
			int count = -1;
			return GetByClass(@class, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedArtifact_Class key.
		///		FK_AllowedArtifact_Class Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="@class"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.AllowedArtifact objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<AllowedArtifact> GetByClass(TransactionManager transactionManager, System.String @class)
		{
			int count = -1;
			return GetByClass(transactionManager, @class, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedArtifact_Class key.
		///		FK_AllowedArtifact_Class Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="@class"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.AllowedArtifact objects.</returns>
		public MetaBuilder.BusinessLogic.TList<AllowedArtifact> GetByClass(TransactionManager transactionManager, System.String @class, int start, int pageLength)
		{
			int count = -1;
			return GetByClass(transactionManager, @class, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedArtifact_Class key.
		///		fKAllowedArtifactClass Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="@class"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.AllowedArtifact objects.</returns>
		public MetaBuilder.BusinessLogic.TList<AllowedArtifact> GetByClass(System.String @class, int start, int pageLength)
		{
			int count =  -1;
			return GetByClass(null, @class, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedArtifact_Class key.
		///		fKAllowedArtifactClass Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="@class"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.AllowedArtifact objects.</returns>
		public MetaBuilder.BusinessLogic.TList<AllowedArtifact> GetByClass(System.String @class, int start, int pageLength,out int count)
		{
			return GetByClass(null, @class, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedArtifact_Class key.
		///		FK_AllowedArtifact_Class Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="@class"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.AllowedArtifact objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<AllowedArtifact> GetByClass(TransactionManager transactionManager, System.String @class, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedArtifact_ClassAssociation key.
		///		FK_AllowedArtifact_ClassAssociation Description: 
		/// </summary>
		/// <param name="cAid"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.AllowedArtifact objects.</returns>
		public MetaBuilder.BusinessLogic.TList<AllowedArtifact> GetByCAid(System.Int32 cAid)
		{
			int count = -1;
			return GetByCAid(cAid, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedArtifact_ClassAssociation key.
		///		FK_AllowedArtifact_ClassAssociation Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="cAid"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.AllowedArtifact objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<AllowedArtifact> GetByCAid(TransactionManager transactionManager, System.Int32 cAid)
		{
			int count = -1;
			return GetByCAid(transactionManager, cAid, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedArtifact_ClassAssociation key.
		///		FK_AllowedArtifact_ClassAssociation Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="cAid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.AllowedArtifact objects.</returns>
		public MetaBuilder.BusinessLogic.TList<AllowedArtifact> GetByCAid(TransactionManager transactionManager, System.Int32 cAid, int start, int pageLength)
		{
			int count = -1;
			return GetByCAid(transactionManager, cAid, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedArtifact_ClassAssociation key.
		///		fKAllowedArtifactClassAssociation Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="cAid"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.AllowedArtifact objects.</returns>
		public MetaBuilder.BusinessLogic.TList<AllowedArtifact> GetByCAid(System.Int32 cAid, int start, int pageLength)
		{
			int count =  -1;
			return GetByCAid(null, cAid, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedArtifact_ClassAssociation key.
		///		fKAllowedArtifactClassAssociation Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="cAid"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.AllowedArtifact objects.</returns>
		public MetaBuilder.BusinessLogic.TList<AllowedArtifact> GetByCAid(System.Int32 cAid, int start, int pageLength,out int count)
		{
			return GetByCAid(null, cAid, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_AllowedArtifact_ClassAssociation key.
		///		FK_AllowedArtifact_ClassAssociation Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="cAid"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.AllowedArtifact objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<AllowedArtifact> GetByCAid(TransactionManager transactionManager, System.Int32 cAid, int start, int pageLength, out int count);
		
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
		public override MetaBuilder.BusinessLogic.AllowedArtifact Get(TransactionManager transactionManager, MetaBuilder.BusinessLogic.AllowedArtifactKey key, int start, int pageLength)
		{
			return GetByCAidClass(transactionManager, key.CAid, key.Class, start, pageLength);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key PK_AllowedArtifact index.
		/// </summary>
		/// <param name="cAid"></param>
		/// <param name="@class"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.AllowedArtifact"/> class.</returns>
		public MetaBuilder.BusinessLogic.AllowedArtifact GetByCAidClass(System.Int32 cAid, System.String @class)
		{
			int count = -1;
			return GetByCAidClass(null,cAid, @class, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_AllowedArtifact index.
		/// </summary>
		/// <param name="cAid"></param>
		/// <param name="@class"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.AllowedArtifact"/> class.</returns>
		public MetaBuilder.BusinessLogic.AllowedArtifact GetByCAidClass(System.Int32 cAid, System.String @class, int start, int pageLength)
		{
			int count = -1;
			return GetByCAidClass(null, cAid, @class, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_AllowedArtifact index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="cAid"></param>
		/// <param name="@class"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.AllowedArtifact"/> class.</returns>
		public MetaBuilder.BusinessLogic.AllowedArtifact GetByCAidClass(TransactionManager transactionManager, System.Int32 cAid, System.String @class)
		{
			int count = -1;
			return GetByCAidClass(transactionManager, cAid, @class, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_AllowedArtifact index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="cAid"></param>
		/// <param name="@class"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.AllowedArtifact"/> class.</returns>
		public MetaBuilder.BusinessLogic.AllowedArtifact GetByCAidClass(TransactionManager transactionManager, System.Int32 cAid, System.String @class, int start, int pageLength)
		{
			int count = -1;
			return GetByCAidClass(transactionManager, cAid, @class, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_AllowedArtifact index.
		/// </summary>
		/// <param name="cAid"></param>
		/// <param name="@class"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.AllowedArtifact"/> class.</returns>
		public MetaBuilder.BusinessLogic.AllowedArtifact GetByCAidClass(System.Int32 cAid, System.String @class, int start, int pageLength, out int count)
		{
			return GetByCAidClass(null, cAid, @class, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_AllowedArtifact index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="cAid"></param>
		/// <param name="@class"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.AllowedArtifact"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.AllowedArtifact GetByCAidClass(TransactionManager transactionManager, System.Int32 cAid, System.String @class, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a MetaBuilder.BusinessLogic.TList&lt;AllowedArtifact&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="MetaBuilder.BusinessLogic.TList&lt;AllowedArtifact&gt;"/></returns>
		public static MetaBuilder.BusinessLogic.TList<AllowedArtifact> Fill(IDataReader reader, MetaBuilder.BusinessLogic.TList<AllowedArtifact> rows, int start, int pageLength)
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
				
				MetaBuilder.BusinessLogic.AllowedArtifact c = null;
				if (DataRepository.Provider.UseEntityFactory)
				{
					key = @"AllowedArtifact" 
							+ (reader.IsDBNull(reader.GetOrdinal("CAid"))?(int)0:(System.Int32)reader["CAid"]).ToString()
							+ (reader.IsDBNull(reader.GetOrdinal("Class"))?string.Empty:(System.String)reader["Class"]).ToString();

					c = EntityManager.LocateOrCreate<AllowedArtifact>(
						key.ToString(), // EntityTrackingKey 
						"AllowedArtifact",  //Creational Type
						DataRepository.Provider.EntityCreationalFactoryType,  //Factory used to create entity
						DataRepository.Provider.EnableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.AllowedArtifact();
				}
				
				if (!DataRepository.Provider.EnableEntityTracking || c.EntityState == EntityState.Added)
                {
					c.SuppressEntityEvents = true;
					c.CAid = (System.Int32)reader["CAid"];
					c.OriginalCAid = c.CAid; //(reader.IsDBNull(reader.GetOrdinal("CAid")))?(int)0:(System.Int32)reader["CAid"];
					c.Class = (System.String)reader["Class"];
					c.OriginalClass = c.Class; //(reader.IsDBNull(reader.GetOrdinal("Class")))?string.Empty:(System.String)reader["Class"];
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
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.AllowedArtifact"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.AllowedArtifact"/> object to refresh.</param>
		public static void RefreshEntity(IDataReader reader, MetaBuilder.BusinessLogic.AllowedArtifact entity)
		{
			if (!reader.Read()) return;
			
			entity.CAid = (System.Int32)reader["CAid"];
			entity.OriginalCAid = (System.Int32)reader["CAid"];
			entity.Class = (System.String)reader["Class"];
			entity.OriginalClass = (System.String)reader["Class"];
			entity.IsActive = (reader.IsDBNull(reader.GetOrdinal("IsActive")))?null:(System.Boolean?)reader["IsActive"];
			entity.AcceptChanges();
		}
		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.AllowedArtifact"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.AllowedArtifact"/> object.</param>
		public static void RefreshEntity(DataSet dataSet, MetaBuilder.BusinessLogic.AllowedArtifact entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.CAid = (System.Int32)dataRow["CAid"];
			entity.OriginalCAid = (System.Int32)dataRow["CAid"];
			entity.Class = (System.String)dataRow["Class"];
			entity.OriginalClass = (System.String)dataRow["Class"];
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
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.AllowedArtifact"/> object to load.</param>
		/// <param name="deep">Boolean. A flag that indicates whether to recursively save all Property Collection that are descendants of this instance. If True, saves the complete object graph below this object. If False, saves this object only. </param>
		/// <param name="deepLoadType">DeepLoadType Enumeration to Include/Exclude object property collections from Load.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.AllowedArtifact Property Collection Type Array To Include or Exclude from Load</param>
		/// <param name="innerList">A collection of child types for easy access.</param>
	    /// <exception cref="ArgumentNullException">entity or childTypes is null.</exception>
	    /// <exception cref="ArgumentException">deepLoadType has invalid value.</exception>
		internal override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.AllowedArtifact entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, ChildEntityTypesList innerList)
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
			
			// Load Entity through Provider
		}
		
		#endregion 
		
		#region DeepSave Methods

		/// <summary>
		/// Deep Save the entire object graph of the MetaBuilder.BusinessLogic.AllowedArtifact object with criteria based of the child 
		/// Type property array and DeepSaveType.
		/// </summary>
		/// <param name="transactionManager">The transaction manager.</param>
		/// <param name="entity">MetaBuilder.BusinessLogic.AllowedArtifact instance</param>
		/// <param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.AllowedArtifact Property Collection Type Array To Include or Exclude from Save</param>
		/// <param name="innerList">A Hashtable of child types for easy access.</param>
		internal override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.AllowedArtifact entity, DeepSaveType deepSaveType, System.Type[] childTypes, ChildEntityTypesList innerList)
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
			
			#region CAidSource
			if (CanDeepSave(entity, "ClassAssociation", "CAidSource", deepSaveType, innerList) 
				&& entity.CAidSource != null)
			{
				DataRepository.ClassAssociationProvider.Save(transactionManager, entity.CAidSource);
				entity.CAid = entity.CAidSource.CAid;
			}
			#endregion 
			#endregion Composite Parent Properties

			// Save Root Entity through Provider
			this.Save(transactionManager, entity);
			
			
						
			return true;
		}
		#endregion
	} // end class
	
	#region AllowedArtifactChildEntityTypes
	
	///<summary>
	/// Enumeration used to expose the different child entity types 
	/// for child properties in <c>MetaBuilder.BusinessLogic.AllowedArtifact</c>
	///</summary>
	public enum AllowedArtifactChildEntityTypes
	{
		
		///<summary>
		/// Composite Property for <c>Class</c> at ClassSource
		///</summary>
		[ChildEntityType(typeof(Class))]
		Class,
			
		///<summary>
		/// Composite Property for <c>ClassAssociation</c> at CAidSource
		///</summary>
		[ChildEntityType(typeof(ClassAssociation))]
		ClassAssociation,
		}
	
	#endregion AllowedArtifactChildEntityTypes
	
	#region AllowedArtifactFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="AllowedArtifact"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class AllowedArtifactFilterBuilder : SqlFilterBuilder<AllowedArtifactColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the AllowedArtifactFilterBuilder class.
		/// </summary>
		public AllowedArtifactFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the AllowedArtifactFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public AllowedArtifactFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the AllowedArtifactFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public AllowedArtifactFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion AllowedArtifactFilterBuilder
	
	#region AllowedArtifactParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="AllowedArtifact"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class AllowedArtifactParameterBuilder : ParameterizedSqlFilterBuilder<AllowedArtifactColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the AllowedArtifactParameterBuilder class.
		/// </summary>
		public AllowedArtifactParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the AllowedArtifactParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public AllowedArtifactParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the AllowedArtifactParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public AllowedArtifactParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion AllowedArtifactParameterBuilder
} // end namespace
