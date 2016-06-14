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
	/// This class is the base class for any <see cref="METAView_Risk_ListingProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAView_Risk_ListingProviderBaseCore : EntityViewProviderBase<METAView_Risk_Listing>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAView_Risk_Listing&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAView_Risk_Listing&gt;"/></returns>
		protected static VList&lt;METAView_Risk_Listing&gt; Fill(DataSet dataSet, VList<METAView_Risk_Listing> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAView_Risk_Listing>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAView_Risk_Listing&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAView_Risk_Listing>"/></returns>
		protected static VList&lt;METAView_Risk_Listing&gt; Fill(DataTable dataTable, VList<METAView_Risk_Listing> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAView_Risk_Listing c = new METAView_Risk_Listing();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeId = (Convert.IsDBNull(row["WorkspaceTypeId"]))?(int)0:(System.Int32?)row["WorkspaceTypeId"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.Name = (Convert.IsDBNull(row["Name"]))?string.Empty:(System.String)row["Name"];
					c.Abbreviation = (Convert.IsDBNull(row["Abbreviation"]))?string.Empty:(System.String)row["Abbreviation"];
					c.Description = (Convert.IsDBNull(row["Description"]))?string.Empty:(System.String)row["Description"];
					c.IsBusinessExternal = (Convert.IsDBNull(row["IsBusinessExternal"]))?string.Empty:(System.String)row["IsBusinessExternal"];
					c.ArchitectureStatus = (Convert.IsDBNull(row["ArchitectureStatus"]))?string.Empty:(System.String)row["ArchitectureStatus"];
					c.ArchitectureStatusDate = (Convert.IsDBNull(row["ArchitectureStatusDate"]))?string.Empty:(System.String)row["ArchitectureStatusDate"];
					c.DesignRationale = (Convert.IsDBNull(row["DesignRationale"]))?string.Empty:(System.String)row["DesignRationale"];
					c.GeneralRemarks = (Convert.IsDBNull(row["GeneralRemarks"]))?string.Empty:(System.String)row["GeneralRemarks"];
					c.GapType = (Convert.IsDBNull(row["GapType"]))?string.Empty:(System.String)row["GapType"];
					c.DataSourceID = (Convert.IsDBNull(row["DataSourceID"]))?string.Empty:(System.String)row["DataSourceID"];
					c.DataSourceName = (Convert.IsDBNull(row["DataSourceName"]))?string.Empty:(System.String)row["DataSourceName"];
					c.RiskStatus = (Convert.IsDBNull(row["RiskStatus"]))?string.Empty:(System.String)row["RiskStatus"];
					c.StatusDate = (Convert.IsDBNull(row["StatusDate"]))?string.Empty:(System.String)row["StatusDate"];
					c.RiskRating = (Convert.IsDBNull(row["RiskRating"]))?string.Empty:(System.String)row["RiskRating"];
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
		/// Fill an <see cref="VList&lt;METAView_Risk_Listing&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAView_Risk_Listing&gt;"/></returns>
		protected VList<METAView_Risk_Listing> Fill(IDataReader reader, VList<METAView_Risk_Listing> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAView_Risk_Listing entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAView_Risk_Listing>("METAView_Risk_Listing",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAView_Risk_Listing();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAView_Risk_ListingColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_Risk_ListingColumn.WorkspaceTypeId)];
					//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
					entity.VCStatusID = (System.Int32)reader[((int)METAView_Risk_ListingColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.pkid = (System.Int32)reader[((int)METAView_Risk_ListingColumn.pkid)];
					//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAView_Risk_ListingColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_Risk_ListingColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Name = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.Name)))?null:(System.String)reader[((int)METAView_Risk_ListingColumn.Name)];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.Abbreviation = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.Abbreviation)))?null:(System.String)reader[((int)METAView_Risk_ListingColumn.Abbreviation)];
					//entity.Abbreviation = (Convert.IsDBNull(reader["Abbreviation"]))?string.Empty:(System.String)reader["Abbreviation"];
					entity.Description = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.Description)))?null:(System.String)reader[((int)METAView_Risk_ListingColumn.Description)];
					//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
					entity.IsBusinessExternal = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.IsBusinessExternal)))?null:(System.String)reader[((int)METAView_Risk_ListingColumn.IsBusinessExternal)];
					//entity.IsBusinessExternal = (Convert.IsDBNull(reader["IsBusinessExternal"]))?string.Empty:(System.String)reader["IsBusinessExternal"];
					entity.ArchitectureStatus = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.ArchitectureStatus)))?null:(System.String)reader[((int)METAView_Risk_ListingColumn.ArchitectureStatus)];
					//entity.ArchitectureStatus = (Convert.IsDBNull(reader["ArchitectureStatus"]))?string.Empty:(System.String)reader["ArchitectureStatus"];
					entity.ArchitectureStatusDate = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.ArchitectureStatusDate)))?null:(System.String)reader[((int)METAView_Risk_ListingColumn.ArchitectureStatusDate)];
					//entity.ArchitectureStatusDate = (Convert.IsDBNull(reader["ArchitectureStatusDate"]))?string.Empty:(System.String)reader["ArchitectureStatusDate"];
					entity.DesignRationale = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.DesignRationale)))?null:(System.String)reader[((int)METAView_Risk_ListingColumn.DesignRationale)];
					//entity.DesignRationale = (Convert.IsDBNull(reader["DesignRationale"]))?string.Empty:(System.String)reader["DesignRationale"];
					entity.GeneralRemarks = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.GeneralRemarks)))?null:(System.String)reader[((int)METAView_Risk_ListingColumn.GeneralRemarks)];
					//entity.GeneralRemarks = (Convert.IsDBNull(reader["GeneralRemarks"]))?string.Empty:(System.String)reader["GeneralRemarks"];
					entity.GapType = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.GapType)))?null:(System.String)reader[((int)METAView_Risk_ListingColumn.GapType)];
					//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
					entity.DataSourceID = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.DataSourceID)))?null:(System.String)reader[((int)METAView_Risk_ListingColumn.DataSourceID)];
					//entity.DataSourceID = (Convert.IsDBNull(reader["DataSourceID"]))?string.Empty:(System.String)reader["DataSourceID"];
					entity.DataSourceName = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.DataSourceName)))?null:(System.String)reader[((int)METAView_Risk_ListingColumn.DataSourceName)];
					//entity.DataSourceName = (Convert.IsDBNull(reader["DataSourceName"]))?string.Empty:(System.String)reader["DataSourceName"];
					entity.RiskStatus = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.RiskStatus)))?null:(System.String)reader[((int)METAView_Risk_ListingColumn.RiskStatus)];
					//entity.RiskStatus = (Convert.IsDBNull(reader["RiskStatus"]))?string.Empty:(System.String)reader["RiskStatus"];
					entity.StatusDate = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.StatusDate)))?null:(System.String)reader[((int)METAView_Risk_ListingColumn.StatusDate)];
					//entity.StatusDate = (Convert.IsDBNull(reader["StatusDate"]))?string.Empty:(System.String)reader["StatusDate"];
					entity.RiskRating = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.RiskRating)))?null:(System.String)reader[((int)METAView_Risk_ListingColumn.RiskRating)];
					//entity.RiskRating = (Convert.IsDBNull(reader["RiskRating"]))?string.Empty:(System.String)reader["RiskRating"];
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
		/// Refreshes the <see cref="METAView_Risk_Listing"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_Risk_Listing"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAView_Risk_Listing entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAView_Risk_ListingColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_Risk_ListingColumn.WorkspaceTypeId)];
			//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
			entity.VCStatusID = (System.Int32)reader[((int)METAView_Risk_ListingColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.pkid = (System.Int32)reader[((int)METAView_Risk_ListingColumn.pkid)];
			//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAView_Risk_ListingColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_Risk_ListingColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Name = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.Name)))?null:(System.String)reader[((int)METAView_Risk_ListingColumn.Name)];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.Abbreviation = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.Abbreviation)))?null:(System.String)reader[((int)METAView_Risk_ListingColumn.Abbreviation)];
			//entity.Abbreviation = (Convert.IsDBNull(reader["Abbreviation"]))?string.Empty:(System.String)reader["Abbreviation"];
			entity.Description = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.Description)))?null:(System.String)reader[((int)METAView_Risk_ListingColumn.Description)];
			//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
			entity.IsBusinessExternal = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.IsBusinessExternal)))?null:(System.String)reader[((int)METAView_Risk_ListingColumn.IsBusinessExternal)];
			//entity.IsBusinessExternal = (Convert.IsDBNull(reader["IsBusinessExternal"]))?string.Empty:(System.String)reader["IsBusinessExternal"];
			entity.ArchitectureStatus = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.ArchitectureStatus)))?null:(System.String)reader[((int)METAView_Risk_ListingColumn.ArchitectureStatus)];
			//entity.ArchitectureStatus = (Convert.IsDBNull(reader["ArchitectureStatus"]))?string.Empty:(System.String)reader["ArchitectureStatus"];
			entity.ArchitectureStatusDate = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.ArchitectureStatusDate)))?null:(System.String)reader[((int)METAView_Risk_ListingColumn.ArchitectureStatusDate)];
			//entity.ArchitectureStatusDate = (Convert.IsDBNull(reader["ArchitectureStatusDate"]))?string.Empty:(System.String)reader["ArchitectureStatusDate"];
			entity.DesignRationale = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.DesignRationale)))?null:(System.String)reader[((int)METAView_Risk_ListingColumn.DesignRationale)];
			//entity.DesignRationale = (Convert.IsDBNull(reader["DesignRationale"]))?string.Empty:(System.String)reader["DesignRationale"];
			entity.GeneralRemarks = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.GeneralRemarks)))?null:(System.String)reader[((int)METAView_Risk_ListingColumn.GeneralRemarks)];
			//entity.GeneralRemarks = (Convert.IsDBNull(reader["GeneralRemarks"]))?string.Empty:(System.String)reader["GeneralRemarks"];
			entity.GapType = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.GapType)))?null:(System.String)reader[((int)METAView_Risk_ListingColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			entity.DataSourceID = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.DataSourceID)))?null:(System.String)reader[((int)METAView_Risk_ListingColumn.DataSourceID)];
			//entity.DataSourceID = (Convert.IsDBNull(reader["DataSourceID"]))?string.Empty:(System.String)reader["DataSourceID"];
			entity.DataSourceName = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.DataSourceName)))?null:(System.String)reader[((int)METAView_Risk_ListingColumn.DataSourceName)];
			//entity.DataSourceName = (Convert.IsDBNull(reader["DataSourceName"]))?string.Empty:(System.String)reader["DataSourceName"];
			entity.RiskStatus = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.RiskStatus)))?null:(System.String)reader[((int)METAView_Risk_ListingColumn.RiskStatus)];
			//entity.RiskStatus = (Convert.IsDBNull(reader["RiskStatus"]))?string.Empty:(System.String)reader["RiskStatus"];
			entity.StatusDate = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.StatusDate)))?null:(System.String)reader[((int)METAView_Risk_ListingColumn.StatusDate)];
			//entity.StatusDate = (Convert.IsDBNull(reader["StatusDate"]))?string.Empty:(System.String)reader["StatusDate"];
			entity.RiskRating = (reader.IsDBNull(((int)METAView_Risk_ListingColumn.RiskRating)))?null:(System.String)reader[((int)METAView_Risk_ListingColumn.RiskRating)];
			//entity.RiskRating = (Convert.IsDBNull(reader["RiskRating"]))?string.Empty:(System.String)reader["RiskRating"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAView_Risk_Listing"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_Risk_Listing"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAView_Risk_Listing entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeId = (Convert.IsDBNull(dataRow["WorkspaceTypeId"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeId"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.Name = (Convert.IsDBNull(dataRow["Name"]))?string.Empty:(System.String)dataRow["Name"];
			entity.Abbreviation = (Convert.IsDBNull(dataRow["Abbreviation"]))?string.Empty:(System.String)dataRow["Abbreviation"];
			entity.Description = (Convert.IsDBNull(dataRow["Description"]))?string.Empty:(System.String)dataRow["Description"];
			entity.IsBusinessExternal = (Convert.IsDBNull(dataRow["IsBusinessExternal"]))?string.Empty:(System.String)dataRow["IsBusinessExternal"];
			entity.ArchitectureStatus = (Convert.IsDBNull(dataRow["ArchitectureStatus"]))?string.Empty:(System.String)dataRow["ArchitectureStatus"];
			entity.ArchitectureStatusDate = (Convert.IsDBNull(dataRow["ArchitectureStatusDate"]))?string.Empty:(System.String)dataRow["ArchitectureStatusDate"];
			entity.DesignRationale = (Convert.IsDBNull(dataRow["DesignRationale"]))?string.Empty:(System.String)dataRow["DesignRationale"];
			entity.GeneralRemarks = (Convert.IsDBNull(dataRow["GeneralRemarks"]))?string.Empty:(System.String)dataRow["GeneralRemarks"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.DataSourceID = (Convert.IsDBNull(dataRow["DataSourceID"]))?string.Empty:(System.String)dataRow["DataSourceID"];
			entity.DataSourceName = (Convert.IsDBNull(dataRow["DataSourceName"]))?string.Empty:(System.String)dataRow["DataSourceName"];
			entity.RiskStatus = (Convert.IsDBNull(dataRow["RiskStatus"]))?string.Empty:(System.String)dataRow["RiskStatus"];
			entity.StatusDate = (Convert.IsDBNull(dataRow["StatusDate"]))?string.Empty:(System.String)dataRow["StatusDate"];
			entity.RiskRating = (Convert.IsDBNull(dataRow["RiskRating"]))?string.Empty:(System.String)dataRow["RiskRating"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAView_Risk_ListingFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_Risk_Listing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_Risk_ListingFilterBuilder : SqlFilterBuilder<METAView_Risk_ListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_Risk_ListingFilterBuilder class.
		/// </summary>
		public METAView_Risk_ListingFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_Risk_ListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_Risk_ListingFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_Risk_ListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_Risk_ListingFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_Risk_ListingFilterBuilder

	#region METAView_Risk_ListingParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_Risk_Listing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_Risk_ListingParameterBuilder : ParameterizedSqlFilterBuilder<METAView_Risk_ListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_Risk_ListingParameterBuilder class.
		/// </summary>
		public METAView_Risk_ListingParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_Risk_ListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_Risk_ListingParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_Risk_ListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_Risk_ListingParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_Risk_ListingParameterBuilder
	
	#region METAView_Risk_ListingSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_Risk_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Risk_ListingSortBuilder : SqlSortBuilder<METAView_Risk_ListingColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_Risk_ListingSqlSortBuilder class.
		/// </summary>
		public METAView_Risk_ListingSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAView_Risk_ListingSortBuilder

} // end namespace
