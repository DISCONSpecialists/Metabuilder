#region Using directives

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;

#endregion

namespace MetaBuilder.DataAccessLayer.Bases
{	
	///<summary>
	/// This class is the base class for any <see cref="VwFieldValueProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class VwFieldValueProviderBaseCore : EntityViewProviderBase<VwFieldValue>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;VwFieldValue&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;VwFieldValue&gt;"/></returns>
		protected static VList&lt;VwFieldValue&gt; Fill(DataSet dataSet, VList<VwFieldValue> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<VwFieldValue>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;VwFieldValue&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<VwFieldValue>"/></returns>
		protected static VList&lt;VwFieldValue&gt; Fill(DataTable dataTable, VList<VwFieldValue> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					VwFieldValue c = new VwFieldValue();
					c.Field = (Convert.IsDBNull(row["Field"]))?string.Empty:(System.String)row["Field"];
					c.ValueString = (Convert.IsDBNull(row["ValueString"]))?string.Empty:(System.String)row["ValueString"];
					c.ValueInt = (Convert.IsDBNull(row["ValueInt"]))?(int)0:(System.Int32?)row["ValueInt"];
					c.ValueDouble = (Convert.IsDBNull(row["ValueDouble"]))?0.0m:(System.Decimal?)row["ValueDouble"];
					c.ValueObjectID = (Convert.IsDBNull(row["ValueObjectID"]))?(int)0:(System.Int32?)row["ValueObjectID"];
					c.ValueDate = (Convert.IsDBNull(row["ValueDate"]))?DateTime.MinValue:(System.DateTime?)row["ValueDate"];
					c.ValueBoolean = (Convert.IsDBNull(row["ValueBoolean"]))?false:(System.Boolean?)row["ValueBoolean"];
					c.AcceptChanges();
					rows.Add(c);
					pagelen -= 1;
				}
				recordnum += 1;
			}
			return rows;
		}
		*/	
						
		///<summary>
		/// Fill an <see cref="VList&lt;VwFieldValue&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;VwFieldValue&gt;"/></returns>
		protected VList<VwFieldValue> Fill(IDataReader reader, VList<VwFieldValue> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					VwFieldValue entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<VwFieldValue>("VwFieldValue",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new VwFieldValue();
					}
					entity.Field = (System.String)reader["Field"];
					//entity.Field = (Convert.IsDBNull(reader["Field"]))?string.Empty:(System.String)reader["Field"];
					entity.ValueString = (reader.IsDBNull(reader.GetOrdinal("ValueString")))?null:(System.String)reader["ValueString"];
					//entity.ValueString = (Convert.IsDBNull(reader["ValueString"]))?string.Empty:(System.String)reader["ValueString"];
					entity.ValueInt = (reader.IsDBNull(reader.GetOrdinal("ValueInt")))?null:(System.Int32?)reader["ValueInt"];
					//entity.ValueInt = (Convert.IsDBNull(reader["ValueInt"]))?(int)0:(System.Int32?)reader["ValueInt"];
					entity.ValueDouble = (reader.IsDBNull(reader.GetOrdinal("ValueDouble")))?null:(System.Decimal?)reader["ValueDouble"];
					//entity.ValueDouble = (Convert.IsDBNull(reader["ValueDouble"]))?0.0m:(System.Decimal?)reader["ValueDouble"];
					entity.ValueObjectID = (reader.IsDBNull(reader.GetOrdinal("ValueObjectID")))?null:(System.Int32?)reader["ValueObjectID"];
					//entity.ValueObjectID = (Convert.IsDBNull(reader["ValueObjectID"]))?(int)0:(System.Int32?)reader["ValueObjectID"];
					entity.ValueDate = (reader.IsDBNull(reader.GetOrdinal("ValueDate")))?null:(System.DateTime?)reader["ValueDate"];
					//entity.ValueDate = (Convert.IsDBNull(reader["ValueDate"]))?DateTime.MinValue:(System.DateTime?)reader["ValueDate"];
					entity.ValueBoolean = (reader.IsDBNull(reader.GetOrdinal("ValueBoolean")))?null:(System.Boolean?)reader["ValueBoolean"];
					//entity.ValueBoolean = (Convert.IsDBNull(reader["ValueBoolean"]))?false:(System.Boolean?)reader["ValueBoolean"];
					entity.AcceptChanges();
					rows.Add(entity);
					pageLength -= 1;
				}
				recordnum += 1;
			}
			return rows;
		}
		
		
		/// <summary>
		/// Refreshes the <see cref="VwFieldValue"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="VwFieldValue"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, VwFieldValue entity)
		{
			reader.Read();
			entity.Field = (System.String)reader["Field"];
			//entity.Field = (Convert.IsDBNull(reader["Field"]))?string.Empty:(System.String)reader["Field"];
			entity.ValueString = (reader.IsDBNull(reader.GetOrdinal("ValueString")))?null:(System.String)reader["ValueString"];
			//entity.ValueString = (Convert.IsDBNull(reader["ValueString"]))?string.Empty:(System.String)reader["ValueString"];
			entity.ValueInt = (reader.IsDBNull(reader.GetOrdinal("ValueInt")))?null:(System.Int32?)reader["ValueInt"];
			//entity.ValueInt = (Convert.IsDBNull(reader["ValueInt"]))?(int)0:(System.Int32?)reader["ValueInt"];
			entity.ValueDouble = (reader.IsDBNull(reader.GetOrdinal("ValueDouble")))?null:(System.Decimal?)reader["ValueDouble"];
			//entity.ValueDouble = (Convert.IsDBNull(reader["ValueDouble"]))?0.0m:(System.Decimal?)reader["ValueDouble"];
			entity.ValueObjectID = (reader.IsDBNull(reader.GetOrdinal("ValueObjectID")))?null:(System.Int32?)reader["ValueObjectID"];
			//entity.ValueObjectID = (Convert.IsDBNull(reader["ValueObjectID"]))?(int)0:(System.Int32?)reader["ValueObjectID"];
			entity.ValueDate = (reader.IsDBNull(reader.GetOrdinal("ValueDate")))?null:(System.DateTime?)reader["ValueDate"];
			//entity.ValueDate = (Convert.IsDBNull(reader["ValueDate"]))?DateTime.MinValue:(System.DateTime?)reader["ValueDate"];
			entity.ValueBoolean = (reader.IsDBNull(reader.GetOrdinal("ValueBoolean")))?null:(System.Boolean?)reader["ValueBoolean"];
			//entity.ValueBoolean = (Convert.IsDBNull(reader["ValueBoolean"]))?false:(System.Boolean?)reader["ValueBoolean"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="VwFieldValue"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="VwFieldValue"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, VwFieldValue entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.Field = (Convert.IsDBNull(dataRow["Field"]))?string.Empty:(System.String)dataRow["Field"];
			entity.ValueString = (Convert.IsDBNull(dataRow["ValueString"]))?string.Empty:(System.String)dataRow["ValueString"];
			entity.ValueInt = (Convert.IsDBNull(dataRow["ValueInt"]))?(int)0:(System.Int32?)dataRow["ValueInt"];
			entity.ValueDouble = (Convert.IsDBNull(dataRow["ValueDouble"]))?0.0m:(System.Decimal?)dataRow["ValueDouble"];
			entity.ValueObjectID = (Convert.IsDBNull(dataRow["ValueObjectID"]))?(int)0:(System.Int32?)dataRow["ValueObjectID"];
			entity.ValueDate = (Convert.IsDBNull(dataRow["ValueDate"]))?DateTime.MinValue:(System.DateTime?)dataRow["ValueDate"];
			entity.ValueBoolean = (Convert.IsDBNull(dataRow["ValueBoolean"]))?false:(System.Boolean?)dataRow["ValueBoolean"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region VwFieldValueFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="VwFieldValue"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class VwFieldValueFilterBuilder : SqlFilterBuilder<VwFieldValueColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the VwFieldValueFilterBuilder class.
		/// </summary>
		public VwFieldValueFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the VwFieldValueFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public VwFieldValueFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the VwFieldValueFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public VwFieldValueFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion VwFieldValueFilterBuilder

	#region VwFieldValueParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="VwFieldValue"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class VwFieldValueParameterBuilder : ParameterizedSqlFilterBuilder<VwFieldValueColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the VwFieldValueParameterBuilder class.
		/// </summary>
		public VwFieldValueParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the VwFieldValueParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public VwFieldValueParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the VwFieldValueParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public VwFieldValueParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion VwFieldValueParameterBuilder
} // end namespace
