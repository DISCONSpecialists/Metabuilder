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
	/// This class is the base class for any <see cref="METAView_Rationale_ListingProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAView_Rationale_ListingProviderBaseCore : EntityViewProviderBase<METAView_Rationale_Listing>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAView_Rationale_Listing&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAView_Rationale_Listing&gt;"/></returns>
		protected static VList&lt;METAView_Rationale_Listing&gt; Fill(DataSet dataSet, VList<METAView_Rationale_Listing> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAView_Rationale_Listing>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAView_Rationale_Listing&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAView_Rationale_Listing>"/></returns>
		protected static VList&lt;METAView_Rationale_Listing&gt; Fill(DataTable dataTable, VList<METAView_Rationale_Listing> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAView_Rationale_Listing c = new METAView_Rationale_Listing();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeId = (Convert.IsDBNull(row["WorkspaceTypeId"]))?(int)0:(System.Int32?)row["WorkspaceTypeId"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.UniqueRef = (Convert.IsDBNull(row["UniqueRef"]))?string.Empty:(System.String)row["UniqueRef"];
					c.RationaleType = (Convert.IsDBNull(row["RationaleType"]))?string.Empty:(System.String)row["RationaleType"];
					c.Value = (Convert.IsDBNull(row["Value"]))?string.Empty:(System.String)row["Value"];
					c.AuthorName = (Convert.IsDBNull(row["AuthorName"]))?string.Empty:(System.String)row["AuthorName"];
					c.EffectiveDate = (Convert.IsDBNull(row["EffectiveDate"]))?string.Empty:(System.String)row["EffectiveDate"];
					c.LongDescription = (Convert.IsDBNull(row["LongDescription"]))?string.Empty:(System.String)row["LongDescription"];
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
		/// Fill an <see cref="VList&lt;METAView_Rationale_Listing&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAView_Rationale_Listing&gt;"/></returns>
		protected VList<METAView_Rationale_Listing> Fill(IDataReader reader, VList<METAView_Rationale_Listing> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAView_Rationale_Listing entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAView_Rationale_Listing>("METAView_Rationale_Listing",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAView_Rationale_Listing();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAView_Rationale_ListingColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_Rationale_ListingColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_Rationale_ListingColumn.WorkspaceTypeId)];
					//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
					entity.VCStatusID = (System.Int32)reader[((int)METAView_Rationale_ListingColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.pkid = (System.Int32)reader[((int)METAView_Rationale_ListingColumn.pkid)];
					//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAView_Rationale_ListingColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAView_Rationale_ListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_Rationale_ListingColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.UniqueRef = (reader.IsDBNull(((int)METAView_Rationale_ListingColumn.UniqueRef)))?null:(System.String)reader[((int)METAView_Rationale_ListingColumn.UniqueRef)];
					//entity.UniqueRef = (Convert.IsDBNull(reader["UniqueRef"]))?string.Empty:(System.String)reader["UniqueRef"];
					entity.RationaleType = (reader.IsDBNull(((int)METAView_Rationale_ListingColumn.RationaleType)))?null:(System.String)reader[((int)METAView_Rationale_ListingColumn.RationaleType)];
					//entity.RationaleType = (Convert.IsDBNull(reader["RationaleType"]))?string.Empty:(System.String)reader["RationaleType"];
					entity.Value = (reader.IsDBNull(((int)METAView_Rationale_ListingColumn.Value)))?null:(System.String)reader[((int)METAView_Rationale_ListingColumn.Value)];
					//entity.Value = (Convert.IsDBNull(reader["Value"]))?string.Empty:(System.String)reader["Value"];
					entity.AuthorName = (reader.IsDBNull(((int)METAView_Rationale_ListingColumn.AuthorName)))?null:(System.String)reader[((int)METAView_Rationale_ListingColumn.AuthorName)];
					//entity.AuthorName = (Convert.IsDBNull(reader["AuthorName"]))?string.Empty:(System.String)reader["AuthorName"];
					entity.EffectiveDate = (reader.IsDBNull(((int)METAView_Rationale_ListingColumn.EffectiveDate)))?null:(System.String)reader[((int)METAView_Rationale_ListingColumn.EffectiveDate)];
					//entity.EffectiveDate = (Convert.IsDBNull(reader["EffectiveDate"]))?string.Empty:(System.String)reader["EffectiveDate"];
					entity.LongDescription = (reader.IsDBNull(((int)METAView_Rationale_ListingColumn.LongDescription)))?null:(System.String)reader[((int)METAView_Rationale_ListingColumn.LongDescription)];
					//entity.LongDescription = (Convert.IsDBNull(reader["LongDescription"]))?string.Empty:(System.String)reader["LongDescription"];
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
		/// Refreshes the <see cref="METAView_Rationale_Listing"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_Rationale_Listing"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAView_Rationale_Listing entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAView_Rationale_ListingColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_Rationale_ListingColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_Rationale_ListingColumn.WorkspaceTypeId)];
			//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
			entity.VCStatusID = (System.Int32)reader[((int)METAView_Rationale_ListingColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.pkid = (System.Int32)reader[((int)METAView_Rationale_ListingColumn.pkid)];
			//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAView_Rationale_ListingColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAView_Rationale_ListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_Rationale_ListingColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.UniqueRef = (reader.IsDBNull(((int)METAView_Rationale_ListingColumn.UniqueRef)))?null:(System.String)reader[((int)METAView_Rationale_ListingColumn.UniqueRef)];
			//entity.UniqueRef = (Convert.IsDBNull(reader["UniqueRef"]))?string.Empty:(System.String)reader["UniqueRef"];
			entity.RationaleType = (reader.IsDBNull(((int)METAView_Rationale_ListingColumn.RationaleType)))?null:(System.String)reader[((int)METAView_Rationale_ListingColumn.RationaleType)];
			//entity.RationaleType = (Convert.IsDBNull(reader["RationaleType"]))?string.Empty:(System.String)reader["RationaleType"];
			entity.Value = (reader.IsDBNull(((int)METAView_Rationale_ListingColumn.Value)))?null:(System.String)reader[((int)METAView_Rationale_ListingColumn.Value)];
			//entity.Value = (Convert.IsDBNull(reader["Value"]))?string.Empty:(System.String)reader["Value"];
			entity.AuthorName = (reader.IsDBNull(((int)METAView_Rationale_ListingColumn.AuthorName)))?null:(System.String)reader[((int)METAView_Rationale_ListingColumn.AuthorName)];
			//entity.AuthorName = (Convert.IsDBNull(reader["AuthorName"]))?string.Empty:(System.String)reader["AuthorName"];
			entity.EffectiveDate = (reader.IsDBNull(((int)METAView_Rationale_ListingColumn.EffectiveDate)))?null:(System.String)reader[((int)METAView_Rationale_ListingColumn.EffectiveDate)];
			//entity.EffectiveDate = (Convert.IsDBNull(reader["EffectiveDate"]))?string.Empty:(System.String)reader["EffectiveDate"];
			entity.LongDescription = (reader.IsDBNull(((int)METAView_Rationale_ListingColumn.LongDescription)))?null:(System.String)reader[((int)METAView_Rationale_ListingColumn.LongDescription)];
			//entity.LongDescription = (Convert.IsDBNull(reader["LongDescription"]))?string.Empty:(System.String)reader["LongDescription"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAView_Rationale_Listing"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_Rationale_Listing"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAView_Rationale_Listing entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeId = (Convert.IsDBNull(dataRow["WorkspaceTypeId"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeId"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.UniqueRef = (Convert.IsDBNull(dataRow["UniqueRef"]))?string.Empty:(System.String)dataRow["UniqueRef"];
			entity.RationaleType = (Convert.IsDBNull(dataRow["RationaleType"]))?string.Empty:(System.String)dataRow["RationaleType"];
			entity.Value = (Convert.IsDBNull(dataRow["Value"]))?string.Empty:(System.String)dataRow["Value"];
			entity.AuthorName = (Convert.IsDBNull(dataRow["AuthorName"]))?string.Empty:(System.String)dataRow["AuthorName"];
			entity.EffectiveDate = (Convert.IsDBNull(dataRow["EffectiveDate"]))?string.Empty:(System.String)dataRow["EffectiveDate"];
			entity.LongDescription = (Convert.IsDBNull(dataRow["LongDescription"]))?string.Empty:(System.String)dataRow["LongDescription"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAView_Rationale_ListingFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_Rationale_Listing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_Rationale_ListingFilterBuilder : SqlFilterBuilder<METAView_Rationale_ListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_Rationale_ListingFilterBuilder class.
		/// </summary>
		public METAView_Rationale_ListingFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_Rationale_ListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_Rationale_ListingFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_Rationale_ListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_Rationale_ListingFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_Rationale_ListingFilterBuilder

	#region METAView_Rationale_ListingParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_Rationale_Listing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_Rationale_ListingParameterBuilder : ParameterizedSqlFilterBuilder<METAView_Rationale_ListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_Rationale_ListingParameterBuilder class.
		/// </summary>
		public METAView_Rationale_ListingParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_Rationale_ListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_Rationale_ListingParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_Rationale_ListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_Rationale_ListingParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_Rationale_ListingParameterBuilder
	
	#region METAView_Rationale_ListingSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_Rationale_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Rationale_ListingSortBuilder : SqlSortBuilder<METAView_Rationale_ListingColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_Rationale_ListingSqlSortBuilder class.
		/// </summary>
		public METAView_Rationale_ListingSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAView_Rationale_ListingSortBuilder

} // end namespace
