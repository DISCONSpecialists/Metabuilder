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
	/// This class is the base class for any <see cref="METAViewResourceMappingValueRetrievalProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAViewResourceMappingValueRetrievalProviderBaseCore : EntityViewProviderBase<METAViewResourceMappingValueRetrieval>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAViewResourceMappingValueRetrieval&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAViewResourceMappingValueRetrieval&gt;"/></returns>
		protected static VList&lt;METAViewResourceMappingValueRetrieval&gt; Fill(DataSet dataSet, VList<METAViewResourceMappingValueRetrieval> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAViewResourceMappingValueRetrieval>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAViewResourceMappingValueRetrieval&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAViewResourceMappingValueRetrieval>"/></returns>
		protected static VList&lt;METAViewResourceMappingValueRetrieval&gt; Fill(DataTable dataTable, VList<METAViewResourceMappingValueRetrieval> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAViewResourceMappingValueRetrieval c = new METAViewResourceMappingValueRetrieval();
					c.WorkspaceID = (Convert.IsDBNull(row["WorkspaceID"]))?(int)0:(System.Int32)row["WorkspaceID"];
					c.Pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Value = (Convert.IsDBNull(row["Value"]))?string.Empty:(System.String)row["Value"];
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
		/// Fill an <see cref="VList&lt;METAViewResourceMappingValueRetrieval&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAViewResourceMappingValueRetrieval&gt;"/></returns>
		protected VList<METAViewResourceMappingValueRetrieval> Fill(IDataReader reader, VList<METAViewResourceMappingValueRetrieval> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAViewResourceMappingValueRetrieval entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAViewResourceMappingValueRetrieval>("METAViewResourceMappingValueRetrieval",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAViewResourceMappingValueRetrieval();
					}
					entity.WorkspaceID = (System.Int32)reader["WorkspaceID"];
					//entity.WorkspaceID = (Convert.IsDBNull(reader["WorkspaceID"]))?(int)0:(System.Int32)reader["WorkspaceID"];
					entity.Pkid = (System.Int32)reader["pkid"];
					//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Value = (reader.IsDBNull(reader.GetOrdinal("Value")))?null:(System.String)reader["Value"];
					//entity.Value = (Convert.IsDBNull(reader["Value"]))?string.Empty:(System.String)reader["Value"];
					entity.AcceptChanges();
					rows.Add(entity);
					pageLength -= 1;
				}
				recordnum += 1;
			}
			return rows;
		}
		
		
		/// <summary>
		/// Refreshes the <see cref="METAViewResourceMappingValueRetrieval"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewResourceMappingValueRetrieval"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAViewResourceMappingValueRetrieval entity)
		{
			reader.Read();
			entity.WorkspaceID = (System.Int32)reader["WorkspaceID"];
			//entity.WorkspaceID = (Convert.IsDBNull(reader["WorkspaceID"]))?(int)0:(System.Int32)reader["WorkspaceID"];
			entity.Pkid = (System.Int32)reader["pkid"];
			//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Value = (reader.IsDBNull(reader.GetOrdinal("Value")))?null:(System.String)reader["Value"];
			//entity.Value = (Convert.IsDBNull(reader["Value"]))?string.Empty:(System.String)reader["Value"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAViewResourceMappingValueRetrieval"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewResourceMappingValueRetrieval"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAViewResourceMappingValueRetrieval entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceID = (Convert.IsDBNull(dataRow["WorkspaceID"]))?(int)0:(System.Int32)dataRow["WorkspaceID"];
			entity.Pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Value = (Convert.IsDBNull(dataRow["Value"]))?string.Empty:(System.String)dataRow["Value"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAViewResourceMappingValueRetrievalFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewResourceMappingValueRetrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewResourceMappingValueRetrievalFilterBuilder : SqlFilterBuilder<METAViewResourceMappingValueRetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewResourceMappingValueRetrievalFilterBuilder class.
		/// </summary>
		public METAViewResourceMappingValueRetrievalFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewResourceMappingValueRetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewResourceMappingValueRetrievalFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewResourceMappingValueRetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewResourceMappingValueRetrievalFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewResourceMappingValueRetrievalFilterBuilder

	#region METAViewResourceMappingValueRetrievalParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewResourceMappingValueRetrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewResourceMappingValueRetrievalParameterBuilder : ParameterizedSqlFilterBuilder<METAViewResourceMappingValueRetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewResourceMappingValueRetrievalParameterBuilder class.
		/// </summary>
		public METAViewResourceMappingValueRetrievalParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewResourceMappingValueRetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewResourceMappingValueRetrievalParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewResourceMappingValueRetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewResourceMappingValueRetrievalParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewResourceMappingValueRetrievalParameterBuilder
} // end namespace
