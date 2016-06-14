﻿#region Using directives

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
	/// This class is the base class for any <see cref="METAView_SubsetIndicator_ListingProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAView_SubsetIndicator_ListingProviderBaseCore : EntityViewProviderBase<METAView_SubsetIndicator_Listing>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAView_SubsetIndicator_Listing&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAView_SubsetIndicator_Listing&gt;"/></returns>
		protected static VList&lt;METAView_SubsetIndicator_Listing&gt; Fill(DataSet dataSet, VList<METAView_SubsetIndicator_Listing> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAView_SubsetIndicator_Listing>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAView_SubsetIndicator_Listing&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAView_SubsetIndicator_Listing>"/></returns>
		protected static VList&lt;METAView_SubsetIndicator_Listing&gt; Fill(DataTable dataTable, VList<METAView_SubsetIndicator_Listing> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAView_SubsetIndicator_Listing c = new METAView_SubsetIndicator_Listing();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeId = (Convert.IsDBNull(row["WorkspaceTypeId"]))?(int)0:(System.Int32?)row["WorkspaceTypeId"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.SubsetIndicatorType = (Convert.IsDBNull(row["SubsetIndicatorType"]))?string.Empty:(System.String)row["SubsetIndicatorType"];
					c.IsCompleteSet = (Convert.IsDBNull(row["IsCompleteSet"]))?string.Empty:(System.String)row["IsCompleteSet"];
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
		/// Fill an <see cref="VList&lt;METAView_SubsetIndicator_Listing&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAView_SubsetIndicator_Listing&gt;"/></returns>
		protected VList<METAView_SubsetIndicator_Listing> Fill(IDataReader reader, VList<METAView_SubsetIndicator_Listing> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAView_SubsetIndicator_Listing entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAView_SubsetIndicator_Listing>("METAView_SubsetIndicator_Listing",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAView_SubsetIndicator_Listing();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAView_SubsetIndicator_ListingColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_SubsetIndicator_ListingColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_SubsetIndicator_ListingColumn.WorkspaceTypeId)];
					//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
					entity.VCStatusID = (System.Int32)reader[((int)METAView_SubsetIndicator_ListingColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.pkid = (System.Int32)reader[((int)METAView_SubsetIndicator_ListingColumn.pkid)];
					//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAView_SubsetIndicator_ListingColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAView_SubsetIndicator_ListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_SubsetIndicator_ListingColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.SubsetIndicatorType = (reader.IsDBNull(((int)METAView_SubsetIndicator_ListingColumn.SubsetIndicatorType)))?null:(System.String)reader[((int)METAView_SubsetIndicator_ListingColumn.SubsetIndicatorType)];
					//entity.SubsetIndicatorType = (Convert.IsDBNull(reader["SubsetIndicatorType"]))?string.Empty:(System.String)reader["SubsetIndicatorType"];
					entity.IsCompleteSet = (reader.IsDBNull(((int)METAView_SubsetIndicator_ListingColumn.IsCompleteSet)))?null:(System.String)reader[((int)METAView_SubsetIndicator_ListingColumn.IsCompleteSet)];
					//entity.IsCompleteSet = (Convert.IsDBNull(reader["IsCompleteSet"]))?string.Empty:(System.String)reader["IsCompleteSet"];
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
		/// Refreshes the <see cref="METAView_SubsetIndicator_Listing"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_SubsetIndicator_Listing"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAView_SubsetIndicator_Listing entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAView_SubsetIndicator_ListingColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_SubsetIndicator_ListingColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_SubsetIndicator_ListingColumn.WorkspaceTypeId)];
			//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
			entity.VCStatusID = (System.Int32)reader[((int)METAView_SubsetIndicator_ListingColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.pkid = (System.Int32)reader[((int)METAView_SubsetIndicator_ListingColumn.pkid)];
			//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAView_SubsetIndicator_ListingColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAView_SubsetIndicator_ListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_SubsetIndicator_ListingColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.SubsetIndicatorType = (reader.IsDBNull(((int)METAView_SubsetIndicator_ListingColumn.SubsetIndicatorType)))?null:(System.String)reader[((int)METAView_SubsetIndicator_ListingColumn.SubsetIndicatorType)];
			//entity.SubsetIndicatorType = (Convert.IsDBNull(reader["SubsetIndicatorType"]))?string.Empty:(System.String)reader["SubsetIndicatorType"];
			entity.IsCompleteSet = (reader.IsDBNull(((int)METAView_SubsetIndicator_ListingColumn.IsCompleteSet)))?null:(System.String)reader[((int)METAView_SubsetIndicator_ListingColumn.IsCompleteSet)];
			//entity.IsCompleteSet = (Convert.IsDBNull(reader["IsCompleteSet"]))?string.Empty:(System.String)reader["IsCompleteSet"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAView_SubsetIndicator_Listing"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_SubsetIndicator_Listing"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAView_SubsetIndicator_Listing entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeId = (Convert.IsDBNull(dataRow["WorkspaceTypeId"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeId"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.SubsetIndicatorType = (Convert.IsDBNull(dataRow["SubsetIndicatorType"]))?string.Empty:(System.String)dataRow["SubsetIndicatorType"];
			entity.IsCompleteSet = (Convert.IsDBNull(dataRow["IsCompleteSet"]))?string.Empty:(System.String)dataRow["IsCompleteSet"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAView_SubsetIndicator_ListingFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_SubsetIndicator_Listing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_SubsetIndicator_ListingFilterBuilder : SqlFilterBuilder<METAView_SubsetIndicator_ListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_SubsetIndicator_ListingFilterBuilder class.
		/// </summary>
		public METAView_SubsetIndicator_ListingFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_SubsetIndicator_ListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_SubsetIndicator_ListingFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_SubsetIndicator_ListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_SubsetIndicator_ListingFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_SubsetIndicator_ListingFilterBuilder

	#region METAView_SubsetIndicator_ListingParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_SubsetIndicator_Listing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_SubsetIndicator_ListingParameterBuilder : ParameterizedSqlFilterBuilder<METAView_SubsetIndicator_ListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_SubsetIndicator_ListingParameterBuilder class.
		/// </summary>
		public METAView_SubsetIndicator_ListingParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_SubsetIndicator_ListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_SubsetIndicator_ListingParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_SubsetIndicator_ListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_SubsetIndicator_ListingParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_SubsetIndicator_ListingParameterBuilder
	
	#region METAView_SubsetIndicator_ListingSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_SubsetIndicator_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_SubsetIndicator_ListingSortBuilder : SqlSortBuilder<METAView_SubsetIndicator_ListingColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_SubsetIndicator_ListingSqlSortBuilder class.
		/// </summary>
		public METAView_SubsetIndicator_ListingSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAView_SubsetIndicator_ListingSortBuilder

} // end namespace
