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
		/// <param name="objectID">. Primary Key.</param>
		/// <param name="fieldID">. Primary Key.</param>
		/// <param name="machineID">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public bool Delete(System.Int32 objectID, System.Int32 fieldID, System.String machineID)
		{
			return Delete(null, objectID, fieldID, machineID);
		}
		
		/// <summary>
		/// 	Deletes a row from the DataSource.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="objectID">. Primary Key.</param>
		/// <param name="fieldID">. Primary Key.</param>
		/// <param name="machineID">. Primary Key.</param>
		/// <remarks>Deletes based on primary key(s).</remarks>
		/// <returns>Returns true if operation suceeded.</returns>
		public abstract bool Delete(TransactionManager transactionManager, System.Int32 objectID, System.Int32 fieldID, System.String machineID);		
		
		#endregion Delete Methods
		
		#region Get By Foreign Key Functions
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectFieldValue_Field key.
		///		FK_ObjectFieldValue_Field Description: 
		/// </summary>
		/// <param name="fieldID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectFieldValue objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectFieldValue> GetByFieldID(System.Int32 fieldID)
		{
			int count = -1;
			return GetByFieldID(fieldID, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectFieldValue_Field key.
		///		FK_ObjectFieldValue_Field Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="fieldID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectFieldValue objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<ObjectFieldValue> GetByFieldID(TransactionManager transactionManager, System.Int32 fieldID)
		{
			int count = -1;
			return GetByFieldID(transactionManager, fieldID, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectFieldValue_Field key.
		///		FK_ObjectFieldValue_Field Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="fieldID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectFieldValue objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectFieldValue> GetByFieldID(TransactionManager transactionManager, System.Int32 fieldID, int start, int pageLength)
		{
			int count = -1;
			return GetByFieldID(transactionManager, fieldID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectFieldValue_Field key.
		///		fKObjectFieldValueField Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="fieldID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectFieldValue objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectFieldValue> GetByFieldID(System.Int32 fieldID, int start, int pageLength)
		{
			int count =  -1;
			return GetByFieldID(null, fieldID, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectFieldValue_Field key.
		///		fKObjectFieldValueField Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="fieldID"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectFieldValue objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectFieldValue> GetByFieldID(System.Int32 fieldID, int start, int pageLength,out int count)
		{
			return GetByFieldID(null, fieldID, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectFieldValue_Field key.
		///		FK_ObjectFieldValue_Field Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="fieldID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectFieldValue objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<ObjectFieldValue> GetByFieldID(TransactionManager transactionManager, System.Int32 fieldID, int start, int pageLength, out int count);
		
	
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectFieldValue_MetaObject key.
		///		FK_ObjectFieldValue_MetaObject Description: 
		/// </summary>
		/// <param name="objectID"></param>
		/// <param name="machineID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectFieldValue objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectFieldValue> GetByObjectIDMachineID(System.Int32 objectID, System.String machineID)
		{
			int count = -1;
			return GetByObjectIDMachineID(objectID, machineID, 0,int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectFieldValue_MetaObject key.
		///		FK_ObjectFieldValue_MetaObject Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="objectID"></param>
		/// <param name="machineID"></param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectFieldValue objects.</returns>
		/// <remarks></remarks>
		public MetaBuilder.BusinessLogic.TList<ObjectFieldValue> GetByObjectIDMachineID(TransactionManager transactionManager, System.Int32 objectID, System.String machineID)
		{
			int count = -1;
			return GetByObjectIDMachineID(transactionManager, objectID, machineID, 0, int.MaxValue, out count);
		}
		
			/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectFieldValue_MetaObject key.
		///		FK_ObjectFieldValue_MetaObject Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="objectID"></param>
		/// <param name="machineID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		///  <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectFieldValue objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectFieldValue> GetByObjectIDMachineID(TransactionManager transactionManager, System.Int32 objectID, System.String machineID, int start, int pageLength)
		{
			int count = -1;
			return GetByObjectIDMachineID(transactionManager, objectID, machineID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectFieldValue_MetaObject key.
		///		fKObjectFieldValueMetaObject Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="objectID"></param>
		/// <param name="machineID"></param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectFieldValue objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectFieldValue> GetByObjectIDMachineID(System.Int32 objectID, System.String machineID, int start, int pageLength)
		{
			int count =  -1;
			return GetByObjectIDMachineID(null, objectID, machineID, start, pageLength,out count);	
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectFieldValue_MetaObject key.
		///		fKObjectFieldValueMetaObject Description: 
		/// </summary>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="objectID"></param>
		/// <param name="machineID"></param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectFieldValue objects.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectFieldValue> GetByObjectIDMachineID(System.Int32 objectID, System.String machineID, int start, int pageLength,out int count)
		{
			return GetByObjectIDMachineID(null, objectID, machineID, start, pageLength, out count);	
		}
						
		/// <summary>
		/// 	Gets rows from the datasource based on the FK_ObjectFieldValue_MetaObject key.
		///		FK_ObjectFieldValue_MetaObject Description: 
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="objectID"></param>
		/// <param name="machineID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns a typed collection of MetaBuilder.BusinessLogic.ObjectFieldValue objects.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<ObjectFieldValue> GetByObjectIDMachineID(TransactionManager transactionManager, System.Int32 objectID, System.String machineID, int start, int pageLength, out int count);
		
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
		/// <param name="objectID"></param>
		/// <param name="fieldID"></param>
		/// <param name="machineID"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ObjectFieldValue"/> class.</returns>
		public MetaBuilder.BusinessLogic.ObjectFieldValue GetByObjectIDFieldIDMachineID(System.Int32 objectID, System.Int32 fieldID, System.String machineID)
		{
			int count = -1;
			return GetByObjectIDFieldIDMachineID(null,objectID, fieldID, machineID, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ObjectFieldValue index.
		/// </summary>
		/// <param name="objectID"></param>
		/// <param name="fieldID"></param>
		/// <param name="machineID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ObjectFieldValue"/> class.</returns>
		public MetaBuilder.BusinessLogic.ObjectFieldValue GetByObjectIDFieldIDMachineID(System.Int32 objectID, System.Int32 fieldID, System.String machineID, int start, int pageLength)
		{
			int count = -1;
			return GetByObjectIDFieldIDMachineID(null, objectID, fieldID, machineID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ObjectFieldValue index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="objectID"></param>
		/// <param name="fieldID"></param>
		/// <param name="machineID"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ObjectFieldValue"/> class.</returns>
		public MetaBuilder.BusinessLogic.ObjectFieldValue GetByObjectIDFieldIDMachineID(TransactionManager transactionManager, System.Int32 objectID, System.Int32 fieldID, System.String machineID)
		{
			int count = -1;
			return GetByObjectIDFieldIDMachineID(transactionManager, objectID, fieldID, machineID, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ObjectFieldValue index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="objectID"></param>
		/// <param name="fieldID"></param>
		/// <param name="machineID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ObjectFieldValue"/> class.</returns>
		public MetaBuilder.BusinessLogic.ObjectFieldValue GetByObjectIDFieldIDMachineID(TransactionManager transactionManager, System.Int32 objectID, System.Int32 fieldID, System.String machineID, int start, int pageLength)
		{
			int count = -1;
			return GetByObjectIDFieldIDMachineID(transactionManager, objectID, fieldID, machineID, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ObjectFieldValue index.
		/// </summary>
		/// <param name="objectID"></param>
		/// <param name="fieldID"></param>
		/// <param name="machineID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ObjectFieldValue"/> class.</returns>
		public MetaBuilder.BusinessLogic.ObjectFieldValue GetByObjectIDFieldIDMachineID(System.Int32 objectID, System.Int32 fieldID, System.String machineID, int start, int pageLength, out int count)
		{
			return GetByObjectIDFieldIDMachineID(null, objectID, fieldID, machineID, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the PK_ObjectFieldValue index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="objectID"></param>
		/// <param name="fieldID"></param>
		/// <param name="machineID"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.ObjectFieldValue"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.ObjectFieldValue GetByObjectIDFieldIDMachineID(TransactionManager transactionManager, System.Int32 objectID, System.Int32 fieldID, System.String machineID, int start, int pageLength, out int count);
						
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key IX_ObjectFieldValue index.
		/// </summary>
		/// <param name="valueString"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.TList&lt;ObjectFieldValue&gt;"/> class.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectFieldValue> GetByValueString(System.String valueString)
		{
			int count = -1;
			return GetByValueString(null,valueString, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the IX_ObjectFieldValue index.
		/// </summary>
		/// <param name="valueString"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.TList&lt;ObjectFieldValue&gt;"/> class.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectFieldValue> GetByValueString(System.String valueString, int start, int pageLength)
		{
			int count = -1;
			return GetByValueString(null, valueString, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the IX_ObjectFieldValue index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="valueString"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.TList&lt;ObjectFieldValue&gt;"/> class.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectFieldValue> GetByValueString(TransactionManager transactionManager, System.String valueString)
		{
			int count = -1;
			return GetByValueString(transactionManager, valueString, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the IX_ObjectFieldValue index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="valueString"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.TList&lt;ObjectFieldValue&gt;"/> class.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectFieldValue> GetByValueString(TransactionManager transactionManager, System.String valueString, int start, int pageLength)
		{
			int count = -1;
			return GetByValueString(transactionManager, valueString, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the IX_ObjectFieldValue index.
		/// </summary>
		/// <param name="valueString"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.TList&lt;ObjectFieldValue&gt;"/> class.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectFieldValue> GetByValueString(System.String valueString, int start, int pageLength, out int count)
		{
			return GetByValueString(null, valueString, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the IX_ObjectFieldValue index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="valueString"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.TList&lt;ObjectFieldValue&gt;"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<ObjectFieldValue> GetByValueString(TransactionManager transactionManager, System.String valueString, int start, int pageLength, out int count);
						
		/// <summary>
		/// 	Gets rows from the datasource based on the primary key IX_ObjectFieldValue_1 index.
		/// </summary>
		/// <param name="valueInt"></param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.TList&lt;ObjectFieldValue&gt;"/> class.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectFieldValue> GetByValueInt(System.Int32? valueInt)
		{
			int count = -1;
			return GetByValueInt(null,valueInt, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the IX_ObjectFieldValue_1 index.
		/// </summary>
		/// <param name="valueInt"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.TList&lt;ObjectFieldValue&gt;"/> class.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectFieldValue> GetByValueInt(System.Int32? valueInt, int start, int pageLength)
		{
			int count = -1;
			return GetByValueInt(null, valueInt, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the IX_ObjectFieldValue_1 index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="valueInt"></param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.TList&lt;ObjectFieldValue&gt;"/> class.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectFieldValue> GetByValueInt(TransactionManager transactionManager, System.Int32? valueInt)
		{
			int count = -1;
			return GetByValueInt(transactionManager, valueInt, 0, int.MaxValue, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the IX_ObjectFieldValue_1 index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="valueInt"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.TList&lt;ObjectFieldValue&gt;"/> class.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectFieldValue> GetByValueInt(TransactionManager transactionManager, System.Int32? valueInt, int start, int pageLength)
		{
			int count = -1;
			return GetByValueInt(transactionManager, valueInt, start, pageLength, out count);
		}
		
		/// <summary>
		/// 	Gets rows from the datasource based on the IX_ObjectFieldValue_1 index.
		/// </summary>
		/// <param name="valueInt"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">out parameter to get total records for query</param>
		/// <remarks></remarks>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.TList&lt;ObjectFieldValue&gt;"/> class.</returns>
		public MetaBuilder.BusinessLogic.TList<ObjectFieldValue> GetByValueInt(System.Int32? valueInt, int start, int pageLength, out int count)
		{
			return GetByValueInt(null, valueInt, start, pageLength, out count);
		}
		
				
		/// <summary>
		/// 	Gets rows from the datasource based on the IX_ObjectFieldValue_1 index.
		/// </summary>
		/// <param name="transactionManager"><see cref="TransactionManager"/> object</param>
		/// <param name="valueInt"></param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">Number of rows to return.</param>
		/// <param name="count">The total number of records.</param>
		/// <returns>Returns an instance of the <see cref="MetaBuilder.BusinessLogic.TList&lt;ObjectFieldValue&gt;"/> class.</returns>
		public abstract MetaBuilder.BusinessLogic.TList<ObjectFieldValue> GetByValueInt(TransactionManager transactionManager, System.Int32? valueInt, int start, int pageLength, out int count);
						
		#endregion "Get By Index Functions"
	
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions	
		
		/// <summary>
		/// Fill a MetaBuilder.BusinessLogic.TList&lt;ObjectFieldValue&gt; From a DataReader.
		/// </summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Row number at which to start reading, the first row is 0.</param>
		/// <param name="pageLength">number of rows.</param>
		/// <returns>a <see cref="MetaBuilder.BusinessLogic.TList&lt;ObjectFieldValue&gt;"/></returns>
		public static MetaBuilder.BusinessLogic.TList<ObjectFieldValue> Fill(IDataReader reader, MetaBuilder.BusinessLogic.TList<ObjectFieldValue> rows, int start, int pageLength)
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
				
				MetaBuilder.BusinessLogic.ObjectFieldValue c = null;
				if (DataRepository.Provider.UseEntityFactory)
				{
					key = @"ObjectFieldValue" 
							+ (reader.IsDBNull(reader.GetOrdinal("ObjectID"))?(int)0:(System.Int32)reader["ObjectID"]).ToString()
							+ (reader.IsDBNull(reader.GetOrdinal("FieldID"))?(int)0:(System.Int32)reader["FieldID"]).ToString()
							+ (reader.IsDBNull(reader.GetOrdinal("MachineID"))?string.Empty:(System.String)reader["MachineID"]).ToString();

					c = EntityManager.LocateOrCreate<ObjectFieldValue>(
						key.ToString(), // EntityTrackingKey 
						"ObjectFieldValue",  //Creational Type
						DataRepository.Provider.EntityCreationalFactoryType,  //Factory used to create entity
						DataRepository.Provider.EnableEntityTracking); // Track this entity?
				}
				else
				{
					c = new MetaBuilder.BusinessLogic.ObjectFieldValue();
				}
				
				if (!DataRepository.Provider.EnableEntityTracking || c.EntityState == EntityState.Added)
                {
					c.SuppressEntityEvents = true;
					c.ObjectID = (System.Int32)reader["ObjectID"];
					c.OriginalObjectID = c.ObjectID; //(reader.IsDBNull(reader.GetOrdinal("ObjectID")))?(int)0:(System.Int32)reader["ObjectID"];
					c.FieldID = (System.Int32)reader["FieldID"];
					c.OriginalFieldID = c.FieldID; //(reader.IsDBNull(reader.GetOrdinal("FieldID")))?(int)0:(System.Int32)reader["FieldID"];
					c.ValueString = (reader.IsDBNull(reader.GetOrdinal("ValueString")))?null:(System.String)reader["ValueString"];
					c.ValueInt = (reader.IsDBNull(reader.GetOrdinal("ValueInt")))?null:(System.Int32?)reader["ValueInt"];
					c.ValueDouble = (reader.IsDBNull(reader.GetOrdinal("ValueDouble")))?null:(System.Decimal?)reader["ValueDouble"];
					c.ValueObjectID = (reader.IsDBNull(reader.GetOrdinal("ValueObjectID")))?null:(System.Int32?)reader["ValueObjectID"];
					c.ValueDate = (reader.IsDBNull(reader.GetOrdinal("ValueDate")))?null:(System.DateTime?)reader["ValueDate"];
					c.ValueBoolean = (reader.IsDBNull(reader.GetOrdinal("ValueBoolean")))?null:(System.Boolean?)reader["ValueBoolean"];
					c.ValueLongText = (reader.IsDBNull(reader.GetOrdinal("ValueLongText")))?null:(System.String)reader["ValueLongText"];
					c.ValueRTF = (reader.IsDBNull(reader.GetOrdinal("ValueRTF")))?null:(System.String)reader["ValueRTF"];
					c.MachineID = (System.String)reader["MachineID"];
					c.OriginalMachineID = c.MachineID; //(reader.IsDBNull(reader.GetOrdinal("MachineID")))?string.Empty:(System.String)reader["MachineID"];
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
			
			entity.ObjectID = (System.Int32)reader["ObjectID"];
			entity.OriginalObjectID = (System.Int32)reader["ObjectID"];
			entity.FieldID = (System.Int32)reader["FieldID"];
			entity.OriginalFieldID = (System.Int32)reader["FieldID"];
			entity.ValueString = (reader.IsDBNull(reader.GetOrdinal("ValueString")))?null:(System.String)reader["ValueString"];
			entity.ValueInt = (reader.IsDBNull(reader.GetOrdinal("ValueInt")))?null:(System.Int32?)reader["ValueInt"];
			entity.ValueDouble = (reader.IsDBNull(reader.GetOrdinal("ValueDouble")))?null:(System.Decimal?)reader["ValueDouble"];
			entity.ValueObjectID = (reader.IsDBNull(reader.GetOrdinal("ValueObjectID")))?null:(System.Int32?)reader["ValueObjectID"];
			entity.ValueDate = (reader.IsDBNull(reader.GetOrdinal("ValueDate")))?null:(System.DateTime?)reader["ValueDate"];
			entity.ValueBoolean = (reader.IsDBNull(reader.GetOrdinal("ValueBoolean")))?null:(System.Boolean?)reader["ValueBoolean"];
			entity.ValueLongText = (reader.IsDBNull(reader.GetOrdinal("ValueLongText")))?null:(System.String)reader["ValueLongText"];
			entity.ValueRTF = (reader.IsDBNull(reader.GetOrdinal("ValueRTF")))?null:(System.String)reader["ValueRTF"];
			entity.MachineID = (System.String)reader["MachineID"];
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
			entity.ValueString = (Convert.IsDBNull(dataRow["ValueString"]))?null:(System.String)dataRow["ValueString"];
			entity.ValueInt = (Convert.IsDBNull(dataRow["ValueInt"]))?null:(System.Int32?)dataRow["ValueInt"];
			entity.ValueDouble = (Convert.IsDBNull(dataRow["ValueDouble"]))?null:(System.Decimal?)dataRow["ValueDouble"];
			entity.ValueObjectID = (Convert.IsDBNull(dataRow["ValueObjectID"]))?null:(System.Int32?)dataRow["ValueObjectID"];
			entity.ValueDate = (Convert.IsDBNull(dataRow["ValueDate"]))?null:(System.DateTime?)dataRow["ValueDate"];
			entity.ValueBoolean = (Convert.IsDBNull(dataRow["ValueBoolean"]))?null:(System.Boolean?)dataRow["ValueBoolean"];
			entity.ValueLongText = (Convert.IsDBNull(dataRow["ValueLongText"]))?null:(System.String)dataRow["ValueLongText"];
			entity.ValueRTF = (Convert.IsDBNull(dataRow["ValueRTF"]))?null:(System.String)dataRow["ValueRTF"];
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
		internal override void DeepLoad(TransactionManager transactionManager, MetaBuilder.BusinessLogic.ObjectFieldValue entity, bool deep, DeepLoadType deepLoadType, System.Type[] childTypes, ChildEntityTypesList innerList)
		{
			if(entity == null)
				return;

			#region FieldIDSource	
			if (CanDeepLoad(entity, "Field", "FieldIDSource", deepLoadType, innerList) 
				&& entity.FieldIDSource == null)
			{
				object[] pkItems = new object[1];
				pkItems[0] = entity.FieldID;
				Field tmpEntity = EntityManager.LocateEntity<Field>(EntityLocator.ConstructKeyFromPkItems(typeof(Field), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.FieldIDSource = tmpEntity;
				else
					entity.FieldIDSource = DataRepository.FieldProvider.GetByPkid(entity.FieldID);
			
				if (deep && entity.FieldIDSource != null)
				{
					DataRepository.FieldProvider.DeepLoad(transactionManager, entity.FieldIDSource, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion FieldIDSource

			#region ObjectIDMachineIDSource	
			if (CanDeepLoad(entity, "MetaObject", "ObjectIDMachineIDSource", deepLoadType, innerList) 
				&& entity.ObjectIDMachineIDSource == null)
			{
				object[] pkItems = new object[2];
				pkItems[0] = entity.ObjectID;
				pkItems[1] = entity.MachineID;
				MetaObject tmpEntity = EntityManager.LocateEntity<MetaObject>(EntityLocator.ConstructKeyFromPkItems(typeof(MetaObject), pkItems), DataRepository.Provider.EnableEntityTracking);
				if (tmpEntity != null)
					entity.ObjectIDMachineIDSource = tmpEntity;
				else
					entity.ObjectIDMachineIDSource = DataRepository.MetaObjectProvider.GetByPkidMachine(entity.ObjectID, entity.MachineID);
			
				if (deep && entity.ObjectIDMachineIDSource != null)
				{
					DataRepository.MetaObjectProvider.DeepLoad(transactionManager, entity.ObjectIDMachineIDSource, deep, deepLoadType, childTypes, innerList);
				}
			}
			#endregion ObjectIDMachineIDSource
			
			// Load Entity through Provider
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
		internal override bool DeepSave(TransactionManager transactionManager, MetaBuilder.BusinessLogic.ObjectFieldValue entity, DeepSaveType deepSaveType, System.Type[] childTypes, ChildEntityTypesList innerList)
		{	
			if (entity == null)
				return false;
							
			#region Composite Parent Properties
			//Save Source Composite Properties, however, don't call deep save on them.  
			//So they only get saved a single level deep.
			
			#region FieldIDSource
			if (CanDeepSave(entity, "Field", "FieldIDSource", deepSaveType, innerList) 
				&& entity.FieldIDSource != null)
			{
				DataRepository.FieldProvider.Save(transactionManager, entity.FieldIDSource);
				entity.FieldID = entity.FieldIDSource.Pkid;
			}
			#endregion 
			
			#region ObjectIDMachineIDSource
			if (CanDeepSave(entity, "MetaObject", "ObjectIDMachineIDSource", deepSaveType, innerList) 
				&& entity.ObjectIDMachineIDSource != null)
			{
				DataRepository.MetaObjectProvider.Save(transactionManager, entity.ObjectIDMachineIDSource);
				entity.ObjectID = entity.ObjectIDMachineIDSource.Pkid;
				entity.MachineID = entity.ObjectIDMachineIDSource.Machine;
			}
			#endregion 
			#endregion Composite Parent Properties

			// Save Root Entity through Provider
			this.Save(transactionManager, entity);
			
			
						
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
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
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
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
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
} // end namespace
