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
	/// This class is the base class for any <see cref="ObjectFieldValueProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract partial class ObjectFieldValueProviderBaseCore : EntityProviderBase<MetaBuilder.BusinessLogic.ObjectFieldValue, MetaBuilder.BusinessLogic.ObjectFieldValueKey>
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
		public override bool Delete(TransactionManager transactionManager, MetaBuilder.BusinessLogic.ObjectFieldValueKey key)
		{
			return Delete(transactionManager, key.ObjectID, key.FieldID, key.MachineID);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="_objectID">. Primary Key.</param>
		/// <param name="_fieldID">. Primary Key.</param>
		/// <param name="_machineID">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(System.Int32 _objectID, System.Int32 _fieldID, System.String _machineID)
		{
			return Delete(null, _objectID, _fieldID, _machineID);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_objectID">. Primary Key.</param>
		/// <param name="_fieldID">. Primary Key.</param>
		/// <param name="_machineID">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(TransactionManager transactionManager, System.Int32 _objectID, System.Int32 _fieldID, System.String _machineID);		
		
		#endregion Delete Methods
		
		#region Get By Foreign Key Functions
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectFieldValue_Field key.
		///		FK_ObjectFieldValue_Field Description: 
		/// </summary>
		/// <param name="_fieldID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectFieldValue objects.</returns>
		public TList<ObjectFieldValue> GetByFieldID(System.Int32 _fieldID)
		{
			int count = -1;
			return GetByFieldID(_fieldID, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectFieldValue_Field key.
		///		FK_ObjectFieldValue_Field Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_fieldID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectFieldValue objects.</returns>
		/// <remarks></remarks>
		public TList<ObjectFieldValue> GetByFieldID(TransactionManager transactionManager, System.Int32 _fieldID)
		{
			int count = -1;
			return GetByFieldID(transactionManager, _fieldID, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectFieldValue_Field key.
		///		FK_ObjectFieldValue_Field Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_fieldID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectFieldValue objects.</returns>
		public TList<ObjectFieldValue> GetByFieldID(TransactionManager transactionManager, System.Int32 _fieldID, int start, int pageLength)
		{
			int count = -1;
			return GetByFieldID(transactionManager, _fieldID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectFieldValue_Field key.
		///		fK_ObjectFieldValue_Field Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_fieldID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectFieldValue objects.</returns>
		public TList<ObjectFieldValue> GetByFieldID(System.Int32 _fieldID, int start, int pageLength)
		{
			int count =  -1;
			return GetByFieldID(null, _fieldID, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectFieldValue_Field key.
		///		fK_ObjectFieldValue_Field Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_fieldID"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectFieldValue objects.</returns>
		public TList<ObjectFieldValue> GetByFieldID(System.Int32 _fieldID, int start, int pageLength,out int count)
		{
			return GetByFieldID(null, _fieldID, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectFieldValue_Field key.
		///		FK_ObjectFieldValue_Field Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_fieldID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectFieldValue objects.</returns>
		public abstract TList<ObjectFieldValue> GetByFieldID(TransactionManager transactionManager, System.Int32 _fieldID, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectFieldValue_MetaObject key.
		///		FK_ObjectFieldValue_MetaObject Description: 
		/// </summary>
		/// <param name="_objectID"></param>
		/// <param name="_machineID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectFieldValue objects.</returns>
		public TList<ObjectFieldValue> GetByObjectIDMachineID(System.Int32 _objectID, System.String _machineID)
		{
			int count = -1;
			return GetByObjectIDMachineID(_objectID, _machineID, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectFieldValue_MetaObject key.
		///		FK_ObjectFieldValue_MetaObject Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_objectID"></param>
		/// <param name="_machineID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectFieldValue objects.</returns>
		/// <remarks></remarks>
		public TList<ObjectFieldValue> GetByObjectIDMachineID(TransactionManager transactionManager, System.Int32 _objectID, System.String _machineID)
		{
			int count = -1;
			return GetByObjectIDMachineID(transactionManager, _objectID, _machineID, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectFieldValue_MetaObject key.
		///		FK_ObjectFieldValue_MetaObject Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_objectID"></param>
		/// <param name="_machineID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectFieldValue objects.</returns>
		public TList<ObjectFieldValue> GetByObjectIDMachineID(TransactionManager transactionManager, System.Int32 _objectID, System.String _machineID, int start, int pageLength)
		{
			int count = -1;
			return GetByObjectIDMachineID(transactionManager, _objectID, _machineID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectFieldValue_MetaObject key.
		///		fK_ObjectFieldValue_MetaObject Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_objectID"></param>
		/// <param name="_machineID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectFieldValue objects.</returns>
		public TList<ObjectFieldValue> GetByObjectIDMachineID(System.Int32 _objectID, System.String _machineID, int start, int pageLength)
		{
			int count =  -1;
			return GetByObjectIDMachineID(null, _objectID, _machineID, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectFieldValue_MetaObject key.
		///		fK_ObjectFieldValue_MetaObject Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="_objectID"></param>
		/// <param name="_machineID"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectFieldValue objects.</returns>
		public TList<ObjectFieldValue> GetByObjectIDMachineID(System.Int32 _objectID, System.String _machineID, int start, int pageLength,out int count)
		{
			return GetByObjectIDMachineID(null, _objectID, _machineID, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectFieldValue_MetaObject key.
		///		FK_ObjectFieldValue_MetaObject Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_objectID"></param>
		/// <param name="_machineID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectFieldValue objects.</returns>
		public abstract TList<ObjectFieldValue> GetByObjectIDMachineID(TransactionManager transactionManager, System.Int32 _objectID, System.String _machineID, int start, int pageLength, out int count);
		
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
		public override MetaBuilder.BusinessLogic.ObjectFieldValue Get(TransactionManager transactionManager, MetaBuilder.BusinessLogic.ObjectFieldValueKey key, int start, int pageLength)
		{
			return GetByObjectIDFieldIDMachineID(transactionManager, key.ObjectID, key.FieldID, key.MachineID, start, pageLength);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key PK_ObjectFieldValue index.
		/// </summary>
		/// <param name="_objectID"></param>
		/// <param name="_fieldID"></param>
		/// <param name="_machineID"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ObjectFieldValue"/> class.</returns>
		public MetaBuilder.BusinessLogic.ObjectFieldValue GetByObjectIDFieldIDMachineID(System.Int32 _objectID, System.Int32 _fieldID, System.String _machineID)
		{
			int count = -1;
			return GetByObjectIDFieldIDMachineID(null,_objectID, _fieldID, _machineID, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ObjectFieldValue index.
		/// </summary>
		/// <param name="_objectID"></param>
		/// <param name="_fieldID"></param>
		/// <param name="_machineID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ObjectFieldValue"/> class.</returns>
		public MetaBuilder.BusinessLogic.ObjectFieldValue GetByObjectIDFieldIDMachineID(System.Int32 _objectID, System.Int32 _fieldID, System.String _machineID, int start, int pageLength)
		{
			int count = -1;
			return GetByObjectIDFieldIDMachineID(null, _objectID, _fieldID, _machineID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ObjectFieldValue index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_objectID"></param>
		/// <param name="_fieldID"></param>
		/// <param name="_machineID"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ObjectFieldValue"/> class.</returns>
		public MetaBuilder.BusinessLogic.ObjectFieldValue GetByObjectIDFieldIDMachineID(TransactionManager transactionManager, System.Int32 _objectID, System.Int32 _fieldID, System.String _machineID)
		{
			int count = -1;
			return GetByObjectIDFieldIDMachineID(transactionManager, _objectID, _fieldID, _machineID, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ObjectFieldValue index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_objectID"></param>
		/// <param name="_fieldID"></param>
		/// <param name="_machineID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ObjectFieldValue"/> class.</returns>
		public MetaBuilder.BusinessLogic.ObjectFieldValue GetByObjectIDFieldIDMachineID(TransactionManager transactionManager, System.Int32 _objectID, System.Int32 _fieldID, System.String _machineID, int start, int pageLength)
		{
			int count = -1;
			return GetByObjectIDFieldIDMachineID(transactionManager, _objectID, _fieldID, _machineID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ObjectFieldValue index.
		/// </summary>
		/// <param name="_objectID"></param>
		/// <param name="_fieldID"></param>
		/// <param name="_machineID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ObjectFieldValue"/> class.</returns>
		public MetaBuilder.BusinessLogic.ObjectFieldValue GetByObjectIDFieldIDMachineID(System.Int32 _objectID, System.Int32 _fieldID, System.String _machineID, int start, int pageLength, out int count)
		{
			return GetByObjectIDFieldIDMachineID(null, _objectID, _fieldID, _machineID, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ObjectFieldValue index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_objectID"></param>
		/// <param name="_fieldID"></param>
		/// <param name="_machineID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ObjectFieldValue"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.ObjectFieldValue GetByObjectIDFieldIDMachineID(TransactionManager transactionManager, System.Int32 _objectID, System.Int32 _fieldID, System.String _machineID, int start, int pageLength, out int count);
						
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key IX_ObjectFieldValue index.
		/// </summary>
		/// <param name="_valueString"></param>
		/// <returns>Returns an instance of the <see cref="TList&lt;ObjectFieldValue&gt;"/> class.</returns>
		public TList<ObjectFieldValue> GetByValueString(System.String _valueString)
		{
			int count = -1;
			return GetByValueString(null,_valueString, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the IX_ObjectFieldValue index.
		/// </summary>
		/// <param name="_valueString"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="TList&lt;ObjectFieldValue&gt;"/> class.</returns>
		public TList<ObjectFieldValue> GetByValueString(System.String _valueString, int start, int pageLength)
		{
			int count = -1;
			return GetByValueString(null, _valueString, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the IX_ObjectFieldValue index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_valueString"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="TList&lt;ObjectFieldValue&gt;"/> class.</returns>
		public TList<ObjectFieldValue> GetByValueString(TransactionManager transactionManager, System.String _valueString)
		{
			int count = -1;
			return GetByValueString(transactionManager, _valueString, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the IX_ObjectFieldValue index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_valueString"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="TList&lt;ObjectFieldValue&gt;"/> class.</returns>
		public TList<ObjectFieldValue> GetByValueString(TransactionManager transactionManager, System.String _valueString, int start, int pageLength)
		{
			int count = -1;
			return GetByValueString(transactionManager, _valueString, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the IX_ObjectFieldValue index.
		/// </summary>
		/// <param name="_valueString"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="TList&lt;ObjectFieldValue&gt;"/> class.</returns>
		public TList<ObjectFieldValue> GetByValueString(System.String _valueString, int start, int pageLength, out int count)
		{
			return GetByValueString(null, _valueString, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the IX_ObjectFieldValue index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_valueString"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="TList&lt;ObjectFieldValue&gt;"/> class.</returns>
		public abstract TList<ObjectFieldValue> GetByValueString(TransactionManager transactionManager, System.String _valueString, int start, int pageLength, out int count);
						
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key IX_ObjectFieldValue_1 index.
		/// </summary>
		/// <param name="_valueInt"></param>
		/// <returns>Returns an instance of the <see cref="TList&lt;ObjectFieldValue&gt;"/> class.</returns>
		public TList<ObjectFieldValue> GetByValueInt(System.Int32? _valueInt)
		{
			int count = -1;
			return GetByValueInt(null,_valueInt, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the IX_ObjectFieldValue_1 index.
		/// </summary>
		/// <param name="_valueInt"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="TList&lt;ObjectFieldValue&gt;"/> class.</returns>
		public TList<ObjectFieldValue> GetByValueInt(System.Int32? _valueInt, int start, int pageLength)
		{
			int count = -1;
			return GetByValueInt(null, _valueInt, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the IX_ObjectFieldValue_1 index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_valueInt"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="TList&lt;ObjectFieldValue&gt;"/> class.</returns>
		public TList<ObjectFieldValue> GetByValueInt(TransactionManager transactionManager, System.Int32? _valueInt)
		{
			int count = -1;
			return GetByValueInt(transactionManager, _valueInt, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the IX_ObjectFieldValue_1 index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_valueInt"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="TList&lt;ObjectFieldValue&gt;"/> class.</returns>
		public TList<ObjectFieldValue> GetByValueInt(TransactionManager transactionManager, System.Int32? _valueInt, int start, int pageLength)
		{
			int count = -1;
			return GetByValueInt(transactionManager, _valueInt, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the IX_ObjectFieldValue_1 index.
		/// </summary>
		/// <param name="_valueInt"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="TList&lt;ObjectFieldValue&gt;"/> class.</returns>
		public TList<ObjectFieldValue> GetByValueInt(System.Int32? _valueInt, int start, int pageLength, out int count)
		{
			return GetByValueInt(null, _valueInt, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the IX_ObjectFieldValue_1 index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="_valueInt"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="TList&lt;ObjectFieldValue&gt;"/> class.</returns>
		public abstract TList<ObjectFieldValue> GetByValueInt(TransactionManager transactionManager, System.Int32? _valueInt, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a TList&lt;ObjectFieldValue&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="TList&lt;ObjectFieldValue&gt;"/></returns>
		public static TList<ObjectFieldValue> Fill(IDataReader reader, TList<ObjectFieldValue> rows, int start, int pageLength)
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
				
				MetaBuilder.BusinessLogic.ObjectFieldValue c = null;
				if (useEntityFactory)
				{
					key = new System.Text.StringBuilder("ObjectFieldValue")
					.Append("|").Append((System.Int32)reader[((int)ObjectFieldValueColumn.ObjectID - 1)])
					.Append("|").Append((System.Int32)reader[((int)ObjectFieldValueColumn.FieldID - 1)])
					.Append("|").Append((System.String)reader[((int)ObjectFieldValueColumn.MachineID - 1)]).ToString();
					c = EntityManager.LocateOrCreate<ObjectFieldValue>(
					key.ToString(), // EntityTrackingKey
					"ObjectFieldValue",  //Creational Type
					entityCreationFactoryType,  //Factory used to create entity
					enableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.ObjectFieldValue();
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
					c.ObjectID = (System.Int32)reader[((int)ObjectFieldValueColumn.ObjectID - 1)];
					c.OriginalObjectID = c.ObjectID;
					c.FieldID = (System.Int32)reader[((int)ObjectFieldValueColumn.FieldID - 1)];
					c.OriginalFieldID = c.FieldID;
					c.ValueString = (reader.IsDBNull(((int)ObjectFieldValueColumn.ValueString - 1)))?null:(System.String)reader[((int)ObjectFieldValueColumn.ValueString - 1)];
					c.ValueInt = (reader.IsDBNull(((int)ObjectFieldValueColumn.ValueInt - 1)))?null:(System.Int32?)reader[((int)ObjectFieldValueColumn.ValueInt - 1)];
					c.ValueDouble = (reader.IsDBNull(((int)ObjectFieldValueColumn.ValueDouble - 1)))?null:(System.Decimal?)reader[((int)ObjectFieldValueColumn.ValueDouble - 1)];
					c.ValueObjectID = (reader.IsDBNull(((int)ObjectFieldValueColumn.ValueObjectID - 1)))?null:(System.Int32?)reader[((int)ObjectFieldValueColumn.ValueObjectID - 1)];
					c.ValueDate = (reader.IsDBNull(((int)ObjectFieldValueColumn.ValueDate - 1)))?null:(System.DateTime?)reader[((int)ObjectFieldValueColumn.ValueDate - 1)];
					c.ValueBoolean = (reader.IsDBNull(((int)ObjectFieldValueColumn.ValueBoolean - 1)))?null:(System.Boolean?)reader[((int)ObjectFieldValueColumn.ValueBoolean - 1)];
					c.ValueLongText = (reader.IsDBNull(((int)ObjectFieldValueColumn.ValueLongText - 1)))?null:(System.String)reader[((int)ObjectFieldValueColumn.ValueLongText - 1)];
					c.ValueRTF = (reader.IsDBNull(((int)ObjectFieldValueColumn.ValueRTF - 1)))?null:(System.String)reader[((int)ObjectFieldValueColumn.ValueRTF - 1)];
					c.MachineID = (System.String)reader[((int)ObjectFieldValueColumn.MachineID - 1)];
					c.OriginalMachineID = c.MachineID;
					c.EntityTrackingKey = key;
					c.AcceptChanges();
					c.SuppressEntityEvents = false;
				}
				rows.Add(c);
			}
		return rows;
		}		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.ObjectFieldValue"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.ObjectFieldValue"/> object to refresh.</param>
		public static void RefreshEntity(IDataReader reader, MetaBuilder.BusinessLogic.ObjectFieldValue entity)
		{
			if (!reader.Read()) return;
			
			entity.ObjectID = (System.Int32)reader[((int)ObjectFieldValueColumn.ObjectID - 1)];
			entity.OriginalObjectID = (System.Int32)reader["ObjectID"];
			entity.FieldID = (System.Int32)reader[((int)ObjectFieldValueColumn.FieldID - 1)];
			entity.OriginalFieldID = (System.Int32)reader["FieldID"];
			entity.ValueString = (reader.IsDBNull(((int)ObjectFieldValueColumn.ValueString - 1)))?null:(System.String)reader[((int)ObjectFieldValueColumn.ValueString - 1)];
			entity.ValueInt = (reader.IsDBNull(((int)ObjectFieldValueColumn.ValueInt - 1)))?null:(System.Int32?)reader[((int)ObjectFieldValueColumn.ValueInt - 1)];
			entity.ValueDouble = (reader.IsDBNull(((int)ObjectFieldValueColumn.ValueDouble - 1)))?null:(System.Decimal?)reader[((int)ObjectFieldValueColumn.ValueDouble - 1)];
			entity.ValueObjectID = (reader.IsDBNull(((int)ObjectFieldValueColumn.ValueObjectID - 1)))?null:(System.Int32?)reader[((int)ObjectFieldValueColumn.ValueObjectID - 1)];
			entity.ValueDate = (reader.IsDBNull(((int)ObjectFieldValueColumn.ValueDate - 1)))?null:(System.DateTime?)reader[((int)ObjectFieldValueColumn.ValueDate - 1)];
			entity.ValueBoolean = (reader.IsDBNull(((int)ObjectFieldValueColumn.ValueBoolean - 1)))?null:(System.Boolean?)reader[((int)ObjectFieldValueColumn.ValueBoolean - 1)];
			entity.ValueLongText = (reader.IsDBNull(((int)ObjectFieldValueColumn.ValueLongText - 1)))?null:(System.String)reader[((int)ObjectFieldValueColumn.ValueLongText - 1)];
			entity.ValueRTF = (reader.IsDBNull(((int)ObjectFieldValueColumn.ValueRTF - 1)))?null:(System.String)reader[((int)ObjectFieldValueColumn.ValueRTF - 1)];
			entity.MachineID = (System.String)reader[((int)ObjectFieldValueColumn.MachineID - 1)];
			entity.OriginalMachineID = (System.String)reader["MachineID"];
			entity.AcceptChanges();
		}
		
		/// <summary>
		/// Refreshes the <see cref="MetaBuilder.BusinessLogic.ObjectFieldValue"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.ObjectFieldValue"/> object.</param>
		public static void RefreshEntity(DataSet dataSet, MetaBuilder.BusinessLogic.ObjectFieldValue entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.ObjectID = (System.Int32)dataRow["ObjectID"];
			entity.OriginalObjectID = (System.Int32)dataRow["ObjectID"];
			entity.FieldID = (System.Int32)dataRow["FieldID"];
			entity.OriginalFieldID = (System.Int32)dataRow["FieldID"];
			entity.ValueString = Convert.IsDBNull(dataRow["ValueString"]) ? null : (System.String)dataRow["ValueString"];
			entity.ValueInt = Convert.IsDBNull(dataRow["ValueInt"]) ? null : (System.Int32?)dataRow["ValueInt"];
			entity.ValueDouble = Convert.IsDBNull(dataRow["ValueDouble"]) ? null : (System.Decimal?)dataRow["ValueDouble"];
			entity.ValueObjectID = Convert.IsDBNull(dataRow["ValueObjectID"]) ? null : (System.Int32?)dataRow["ValueObjectID"];
			entity.ValueDate = Convert.IsDBNull(dataRow["ValueDate"]) ? null : (System.DateTime?)dataRow["ValueDate"];
			entity.ValueBoolean = Convert.IsDBNull(dataRow["ValueBoolean"]) ? null : (System.Boolean?)dataRow["ValueBoolean"];
			entity.ValueLongText = Convert.IsDBNull(dataRow["ValueLongText"]) ? null : (System.String)dataRow["ValueLongText"];
			entity.ValueRTF = Convert.IsDBNull(dataRow["ValueRTF"]) ? null : (System.String)dataRow["ValueRTF"];
			entity.MachineID = (System.String)dataRow["MachineID"];
			entity.OriginalMachineID = (System.String)dataRow["MachineID"];
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
		/// <param name="entity">The <see cref="MetaBuilder.BusinessLogic.ObjectFieldValue"/> object to load.</param>
		/// <param name="deep">Boolean. A flag that indicates whether to recursively save all Property Collection that are descendants of this instance. If True, saves the complete object graph below this object. If False, saves this object only. </param>
		/// <param name="deepLoadType">DeepLoadType Enumeration to Include/Exclude object property collections from Load.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.ObjectFieldValue Property Collection Type Array To Include or Exclude from Load</param>
		/// <param name="innerList">A collection of child types for easy access.</param>
	    /// <exception cref="ArgumentNullException">entity or childTypes is null.</exception>
	    /// <exception cref="ArgumentException">deepLoadType has invalid value.</exception>
		public override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.ObjectFieldValue entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, DeepSession innerList)
		{
			if(entity == null)
				return;

			#region FieldIDSource	
			if (CanDeepLoad(entity, "Field|FieldIDSource", deepLoadType, innerList) 
				&& entity.FieldIDSource == null)
			{
				object[] pkItems = new object[1];
				pkItems[0] = entity.FieldID;
				Field tmpEntity = EntityManager.LocateEntity<Field>(EntityLocator.ConstructKeyFromPkItems(typeof(Field), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.FieldIDSource = tmpEntity;
				else
					entity.FieldIDSource = DataRepository.FieldProvider.GetBypkid(transactionManager, entity.FieldID);		
				
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'FieldIDSource' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.FieldIDSource != null)
				{
					innerList.SkipChildren = true;
					DataRepository.FieldProvider.DeepLoad(transactionManager, entity.FieldIDSource, deep, deepLoadType, childTypes, innerList);
					innerList.SkipChildren = false;
				}
					
			}
			#endregion FieldIDSource

			#region ObjectIDMachineIDSource	
			if (CanDeepLoad(entity, "MetaObject|ObjectIDMachineIDSource", deepLoadType, innerList) 
				&& entity.ObjectIDMachineIDSource == null)
			{
				object[] pkItems = new object[2];
				pkItems[0] = entity.ObjectID;
				pkItems[1] = entity.MachineID;
				MetaObject tmpEntity = EntityManager.LocateEntity<MetaObject>(EntityLocator.ConstructKeyFromPkItems(typeof(MetaObject), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.ObjectIDMachineIDSource = tmpEntity;
				else
					entity.ObjectIDMachineIDSource = DataRepository.MetaObjectProvider.GetBypkidMachine(transactionManager, entity.ObjectID, entity.MachineID);		
				
				#if NETTIERS_DEBUG
				System.Diagnostics.Debug.WriteLine("- property 'ObjectIDMachineIDSource' loaded. key " + entity.EntityTrackingKey);
				#endif 
				
				if (deep && entity.ObjectIDMachineIDSource != null)
				{
					innerList.SkipChildren = true;
					DataRepository.MetaObjectProvider.DeepLoad(transactionManager, entity.ObjectIDMachineIDSource, deep, deepLoadType, childTypes, innerList);
					innerList.SkipChildren = false;
				}
					
			}
			#endregion ObjectIDMachineIDSource
			
			//used to hold DeepLoad method delegates and fire after all the local children have been loaded.
			Dictionary<string, KeyValuePair<Delegate, object>> deepHandles = new Dictionary<string, KeyValuePair<Delegate, object>>();
			
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
		/// Deep Save the entire object graph of the MetaBuilder.BusinessLogic.ObjectFieldValue object with criteria based of the child 
		/// Type property array and DeepSaveType.
		/// </summary>
		/// <param name="transactionManager">The transaction manager.</param>
		/// <param name="entity">MetaBuilder.BusinessLogic.ObjectFieldValue instance</param>
		/// <param name="deepSaveType">DeepSaveType Enumeration to Include/Exclude object property collections from Save.</param>
		/// <param name="childTypes">MetaBuilder.BusinessLogic.ObjectFieldValue Property Collection Type Array To Include or Exclude from Save</param>
		/// <param name="innerList">A Hashtable of child types for easy access.</param>
		public override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.ObjectFieldValue entity, DeepSaveType deepSaveType, System.Type[] childTypes, DeepSession innerList)
		{	
			if (entity == null)
				return false;
							
			#region Composite Parent Properties
			//Save Source Composite Properties, however, don't call deep save on them.  
			//So they only get saved a single level deep.
			
			#region FieldIDSource
			if (CanDeepSave(entity, "Field|FieldIDSource", deepSaveType, innerList) 
				&& entity.FieldIDSource != null)
			{
				DataRepository.FieldProvider.Save(transactionManager, entity.FieldIDSource);
				entity.FieldID = entity.FieldIDSource.pkid;
			}
			#endregion 
			
			#region ObjectIDMachineIDSource
			if (CanDeepSave(entity, "MetaObject|ObjectIDMachineIDSource", deepSaveType, innerList) 
				&& entity.ObjectIDMachineIDSource != null)
			{
				DataRepository.MetaObjectProvider.Save(transactionManager, entity.ObjectIDMachineIDSource);
				entity.ObjectID = entity.ObjectIDMachineIDSource.pkid;
				entity.MachineID = entity.ObjectIDMachineIDSource.Machine;
			}
			#endregion 
			#endregion Composite Parent Properties

			// Save Root Entity through Provider
			if (!entity.IsDeleted)
				this.Save(transactionManager, entity);
			
			//used to hold DeepSave method delegates and fire after all the local children have been saved.
			Dictionary<string, KeyValuePair<Delegate, object>> deepHandles = new Dictionary<string, KeyValuePair<Delegate, object>>();
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
	
	#region ObjectFieldValueChildEntityTypes
	
	///<summary>
	/// Enumeration used to expose the different child entity types 
	/// for child properties in <c>MetaBuilder.BusinessLogic.ObjectFieldValue</c>
	///</summary>
	public enum ObjectFieldValueChildEntityTypes
	{
		
		///<summary>
		/// Composite Property for <c>Field</c> at FieldIDSource
		///</summary>
		[ChildEntityType(typeof(Field))]
		Field,
			
		///<summary>
		/// Composite Property for <c>MetaObject</c> at ObjectIDMachineIDSource
		///</summary>
		[ChildEntityType(typeof(MetaObject))]
		MetaObject,
		}
	
	#endregion ObjectFieldValueChildEntityTypes
	
	#region ObjectFieldValueFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;ObjectFieldValueColumn&gt;"/> class
	/// that is used exclusively with a <see cref="ObjectFieldValue"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class ObjectFieldValueFilterBuilder : SqlFilterBuilder<ObjectFieldValueColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the ObjectFieldValueFilterBuilder class.
		/// </summary>
		public ObjectFieldValueFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the ObjectFieldValueFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public ObjectFieldValueFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the ObjectFieldValueFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public ObjectFieldValueFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion ObjectFieldValueFilterBuilder
	
	#region ObjectFieldValueParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;ObjectFieldValueColumn&gt;"/> class
	/// that is used exclusively with a <see cref="ObjectFieldValue"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class ObjectFieldValueParameterBuilder : ParameterizedSqlFilterBuilder<ObjectFieldValueColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the ObjectFieldValueParameterBuilder class.
		/// </summary>
		public ObjectFieldValueParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the ObjectFieldValueParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public ObjectFieldValueParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the ObjectFieldValueParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public ObjectFieldValueParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion ObjectFieldValueParameterBuilder
	
	#region ObjectFieldValueSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;ObjectFieldValueColumn&gt;"/> class
	/// that is used exclusively with a <see cref="ObjectFieldValue"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class ObjectFieldValueSortBuilder : SqlSortBuilder<ObjectFieldValueColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the ObjectFieldValueSqlSortBuilder class.
		/// </summary>
		public ObjectFieldValueSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion ObjectFieldValueSortBuilder
	
} // end namespace
