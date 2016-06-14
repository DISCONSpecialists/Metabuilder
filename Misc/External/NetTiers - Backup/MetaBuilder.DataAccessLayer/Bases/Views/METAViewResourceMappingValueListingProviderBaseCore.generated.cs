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
	/// This class is the base class for any <see cref="METAViewResourceMappingValueListingProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAViewResourceMappingValueListingProviderBaseCore : EntityViewProviderBase<METAViewResourceMappingValueListing>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAViewResourceMappingValueListing&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAViewResourceMappingValueListing&gt;"/></returns>
		protected static VList&lt;METAViewResourceMappingValueListing&gt; Fill(DataSet dataSet, VList<METAViewResourceMappingValueListing> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAViewResourceMappingValueListing>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAViewResourceMappingValueListing&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAViewResourceMappingValueListing>"/></returns>
		protected static VList&lt;METAViewResourceMappingValueListing&gt; Fill(DataTable dataTable, VList<METAViewResourceMappingValueListing> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAViewResourceMappingValueListing c = new METAViewResourceMappingValueListing();
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
		/// Fill an <see cref="VList&lt;METAViewResourceMappingValueListing&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAViewResourceMappingValueListing&gt;"/></returns>
		protected VList<METAViewResourceMappingValueListing> Fill(IDataReader reader, VList<METAViewResourceMappingValueListing> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAViewResourceMappingValueListing entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAViewResourceMappingValueListing>("METAViewResourceMappingValueListing",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAViewResourceMappingValueListing();
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
		/// Refreshes the <see cref="METAViewResourceMappingValueListing"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewResourceMappingValueListing"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAViewResourceMappingValueListing entity)
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
		/// Refreshes the <see cref="METAViewResourceMappingValueListing"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewResourceMappingValueListing"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAViewResourceMappingValueListing entity)
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

	#region METAViewResourceMappingValueListingFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewResourceMappingValueListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewResourceMappingValueListingFilterBuilder : SqlFilterBuilder<METAViewResourceMappingValueListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewResourceMappingValueListingFilterBuilder class.
		/// </summary>
		public METAViewResourceMappingValueListingFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewResourceMappingValueListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewResourceMappingValueListingFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewResourceMappingValueListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewResourceMappingValueListingFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewResourceMappingValueListingFilterBuilder

	#region METAViewResourceMappingValueListingParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewResourceMappingValueListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewResourceMappingValueListingParameterBuilder : ParameterizedSqlFilterBuilder<METAViewResourceMappingValueListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewResourceMappingValueListingParameterBuilder class.
		/// </summary>
		public METAViewResourceMappingValueListingParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewResourceMappingValueListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewResourceMappingValueListingParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewResourceMappingValueListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewResourceMappingValueListingParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewResourceMappingValueListingParameterBuilder
} // end namespace
