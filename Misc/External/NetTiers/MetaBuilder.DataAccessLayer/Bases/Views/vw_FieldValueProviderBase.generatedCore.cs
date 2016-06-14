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
	/// This class is the base class for any <see cref="vw_FieldValueProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class vw_FieldValueProviderBaseCore : EntityViewProviderBase<vw_FieldValue>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;vw_FieldValue&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;vw_FieldValue&gt;"/></returns>
		protected static VList&lt;vw_FieldValue&gt; Fill(DataSet dataSet, VList<vw_FieldValue> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<vw_FieldValue>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;vw_FieldValue&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<vw_FieldValue>"/></returns>
		protected static VList&lt;vw_FieldValue&gt; Fill(DataTable dataTable, VList<vw_FieldValue> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					vw_FieldValue c = new vw_FieldValue();
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
		/// Fill an <see cref="VList&lt;vw_FieldValue&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;vw_FieldValue&gt;"/></returns>
		protected VList<vw_FieldValue> Fill(IDataReader reader, VList<vw_FieldValue> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					vw_FieldValue entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<vw_FieldValue>("vw_FieldValue",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new vw_FieldValue();
					}
					
					entity.SuppressEntityEvents = true;

					entity.Field = (System.String)reader[((int)vw_FieldValueColumn.Field)];
					//entity.Field = (Convert.IsDBNull(reader["Field"]))?string.Empty:(System.String)reader["Field"];
					entity.ValueString = (reader.IsDBNull(((int)vw_FieldValueColumn.ValueString)))?null:(System.String)reader[((int)vw_FieldValueColumn.ValueString)];
					//entity.ValueString = (Convert.IsDBNull(reader["ValueString"]))?string.Empty:(System.String)reader["ValueString"];
					entity.ValueInt = (reader.IsDBNull(((int)vw_FieldValueColumn.ValueInt)))?null:(System.Int32?)reader[((int)vw_FieldValueColumn.ValueInt)];
					//entity.ValueInt = (Convert.IsDBNull(reader["ValueInt"]))?(int)0:(System.Int32?)reader["ValueInt"];
					entity.ValueDouble = (reader.IsDBNull(((int)vw_FieldValueColumn.ValueDouble)))?null:(System.Decimal?)reader[((int)vw_FieldValueColumn.ValueDouble)];
					//entity.ValueDouble = (Convert.IsDBNull(reader["ValueDouble"]))?0.0m:(System.Decimal?)reader["ValueDouble"];
					entity.ValueObjectID = (reader.IsDBNull(((int)vw_FieldValueColumn.ValueObjectID)))?null:(System.Int32?)reader[((int)vw_FieldValueColumn.ValueObjectID)];
					//entity.ValueObjectID = (Convert.IsDBNull(reader["ValueObjectID"]))?(int)0:(System.Int32?)reader["ValueObjectID"];
					entity.ValueDate = (reader.IsDBNull(((int)vw_FieldValueColumn.ValueDate)))?null:(System.DateTime?)reader[((int)vw_FieldValueColumn.ValueDate)];
					//entity.ValueDate = (Convert.IsDBNull(reader["ValueDate"]))?DateTime.MinValue:(System.DateTime?)reader["ValueDate"];
					entity.ValueBoolean = (reader.IsDBNull(((int)vw_FieldValueColumn.ValueBoolean)))?null:(System.Boolean?)reader[((int)vw_FieldValueColumn.ValueBoolean)];
					//entity.ValueBoolean = (Convert.IsDBNull(reader["ValueBoolean"]))?false:(System.Boolean?)reader["ValueBoolean"];
					entity.AcceptChanges();
					entity.SuppressEntityEvents = false;
					
					rows.Add(entity);
					pageLength -= 1;
				}
				recordnum += 1;
			}
			return rows;
		}
		
		
		/// <summary>
		/// Refreshes the <see cref="vw_FieldValue"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="vw_FieldValue"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, vw_FieldValue entity)
		{
			reader.Read();
			entity.Field = (System.String)reader[((int)vw_FieldValueColumn.Field)];
			//entity.Field = (Convert.IsDBNull(reader["Field"]))?string.Empty:(System.String)reader["Field"];
			entity.ValueString = (reader.IsDBNull(((int)vw_FieldValueColumn.ValueString)))?null:(System.String)reader[((int)vw_FieldValueColumn.ValueString)];
			//entity.ValueString = (Convert.IsDBNull(reader["ValueString"]))?string.Empty:(System.String)reader["ValueString"];
			entity.ValueInt = (reader.IsDBNull(((int)vw_FieldValueColumn.ValueInt)))?null:(System.Int32?)reader[((int)vw_FieldValueColumn.ValueInt)];
			//entity.ValueInt = (Convert.IsDBNull(reader["ValueInt"]))?(int)0:(System.Int32?)reader["ValueInt"];
			entity.ValueDouble = (reader.IsDBNull(((int)vw_FieldValueColumn.ValueDouble)))?null:(System.Decimal?)reader[((int)vw_FieldValueColumn.ValueDouble)];
			//entity.ValueDouble = (Convert.IsDBNull(reader["ValueDouble"]))?0.0m:(System.Decimal?)reader["ValueDouble"];
			entity.ValueObjectID = (reader.IsDBNull(((int)vw_FieldValueColumn.ValueObjectID)))?null:(System.Int32?)reader[((int)vw_FieldValueColumn.ValueObjectID)];
			//entity.ValueObjectID = (Convert.IsDBNull(reader["ValueObjectID"]))?(int)0:(System.Int32?)reader["ValueObjectID"];
			entity.ValueDate = (reader.IsDBNull(((int)vw_FieldValueColumn.ValueDate)))?null:(System.DateTime?)reader[((int)vw_FieldValueColumn.ValueDate)];
			//entity.ValueDate = (Convert.IsDBNull(reader["ValueDate"]))?DateTime.MinValue:(System.DateTime?)reader["ValueDate"];
			entity.ValueBoolean = (reader.IsDBNull(((int)vw_FieldValueColumn.ValueBoolean)))?null:(System.Boolean?)reader[((int)vw_FieldValueColumn.ValueBoolean)];
			//entity.ValueBoolean = (Convert.IsDBNull(reader["ValueBoolean"]))?false:(System.Boolean?)reader["ValueBoolean"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="vw_FieldValue"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="vw_FieldValue"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, vw_FieldValue entity)
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

	#region vw_FieldValueFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="vw_FieldValue"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class vw_FieldValueFilterBuilder : SqlFilterBuilder<vw_FieldValueColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the vw_FieldValueFilterBuilder class.
		/// </summary>
		public vw_FieldValueFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the vw_FieldValueFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public vw_FieldValueFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the vw_FieldValueFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public vw_FieldValueFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion vw_FieldValueFilterBuilder

	#region vw_FieldValueParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="vw_FieldValue"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class vw_FieldValueParameterBuilder : ParameterizedSqlFilterBuilder<vw_FieldValueColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the vw_FieldValueParameterBuilder class.
		/// </summary>
		public vw_FieldValueParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the vw_FieldValueParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public vw_FieldValueParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the vw_FieldValueParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public vw_FieldValueParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion vw_FieldValueParameterBuilder
	
	#region vw_FieldValueSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="vw_FieldValue"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class vw_FieldValueSortBuilder : SqlSortBuilder<vw_FieldValueColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the vw_FieldValueSqlSortBuilder class.
		/// </summary>
		public vw_FieldValueSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion vw_FieldValueSortBuilder

} // end namespace
