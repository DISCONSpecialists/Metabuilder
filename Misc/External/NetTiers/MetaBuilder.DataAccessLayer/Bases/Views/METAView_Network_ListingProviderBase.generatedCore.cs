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
	/// This class is the base class for any <see cref="METAView_Network_ListingProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAView_Network_ListingProviderBaseCore : EntityViewProviderBase<METAView_Network_Listing>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAView_Network_Listing&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAView_Network_Listing&gt;"/></returns>
		protected static VList&lt;METAView_Network_Listing&gt; Fill(DataSet dataSet, VList<METAView_Network_Listing> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAView_Network_Listing>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAView_Network_Listing&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAView_Network_Listing>"/></returns>
		protected static VList&lt;METAView_Network_Listing&gt; Fill(DataTable dataTable, VList<METAView_Network_Listing> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAView_Network_Listing c = new METAView_Network_Listing();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeId = (Convert.IsDBNull(row["WorkspaceTypeId"]))?(int)0:(System.Int32?)row["WorkspaceTypeId"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.Name = (Convert.IsDBNull(row["Name"]))?string.Empty:(System.String)row["Name"];
					c.NetworkType = (Convert.IsDBNull(row["NetworkType"]))?string.Empty:(System.String)row["NetworkType"];
					c.DataStorageNetworkType = (Convert.IsDBNull(row["DataStorageNetworkType"]))?string.Empty:(System.String)row["DataStorageNetworkType"];
					c.ConnectionType = (Convert.IsDBNull(row["ConnectionType"]))?string.Empty:(System.String)row["ConnectionType"];
					c.ConnectionSpeed = (Convert.IsDBNull(row["ConnectionSpeed"]))?string.Empty:(System.String)row["ConnectionSpeed"];
					c.ConnectionSize = (Convert.IsDBNull(row["ConnectionSize"]))?string.Empty:(System.String)row["ConnectionSize"];
					c.Description = (Convert.IsDBNull(row["Description"]))?string.Empty:(System.String)row["Description"];
					c.Abbreviation = (Convert.IsDBNull(row["Abbreviation"]))?string.Empty:(System.String)row["Abbreviation"];
					c.IsBusinessExternal = (Convert.IsDBNull(row["IsBusinessExternal"]))?string.Empty:(System.String)row["IsBusinessExternal"];
					c.DataSourceID = (Convert.IsDBNull(row["DataSourceID"]))?string.Empty:(System.String)row["DataSourceID"];
					c.DataSourceName = (Convert.IsDBNull(row["DataSourceName"]))?string.Empty:(System.String)row["DataSourceName"];
					c.GeneralRemarks = (Convert.IsDBNull(row["GeneralRemarks"]))?string.Empty:(System.String)row["GeneralRemarks"];
					c.GapType = (Convert.IsDBNull(row["GapType"]))?string.Empty:(System.String)row["GapType"];
					c.PrimaryOrBackupType = (Convert.IsDBNull(row["PrimaryOrBackupType"]))?string.Empty:(System.String)row["PrimaryOrBackupType"];
					c.NetworkRange = (Convert.IsDBNull(row["NetworkRange"]))?string.Empty:(System.String)row["NetworkRange"];
					c.ArchitectureStatus = (Convert.IsDBNull(row["ArchitectureStatus"]))?string.Empty:(System.String)row["ArchitectureStatus"];
					c.ArchitectureStatusDate = (Convert.IsDBNull(row["ArchitectureStatusDate"]))?string.Empty:(System.String)row["ArchitectureStatusDate"];
					c.DesignRationale = (Convert.IsDBNull(row["DesignRationale"]))?string.Empty:(System.String)row["DesignRationale"];
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
		/// Fill an <see cref="VList&lt;METAView_Network_Listing&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAView_Network_Listing&gt;"/></returns>
		protected VList<METAView_Network_Listing> Fill(IDataReader reader, VList<METAView_Network_Listing> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAView_Network_Listing entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAView_Network_Listing>("METAView_Network_Listing",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAView_Network_Listing();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAView_Network_ListingColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_Network_ListingColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_Network_ListingColumn.WorkspaceTypeId)];
					//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
					entity.VCStatusID = (System.Int32)reader[((int)METAView_Network_ListingColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.pkid = (System.Int32)reader[((int)METAView_Network_ListingColumn.pkid)];
					//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAView_Network_ListingColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAView_Network_ListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Name = (reader.IsDBNull(((int)METAView_Network_ListingColumn.Name)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.Name)];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.NetworkType = (reader.IsDBNull(((int)METAView_Network_ListingColumn.NetworkType)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.NetworkType)];
					//entity.NetworkType = (Convert.IsDBNull(reader["NetworkType"]))?string.Empty:(System.String)reader["NetworkType"];
					entity.DataStorageNetworkType = (reader.IsDBNull(((int)METAView_Network_ListingColumn.DataStorageNetworkType)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.DataStorageNetworkType)];
					//entity.DataStorageNetworkType = (Convert.IsDBNull(reader["DataStorageNetworkType"]))?string.Empty:(System.String)reader["DataStorageNetworkType"];
					entity.ConnectionType = (reader.IsDBNull(((int)METAView_Network_ListingColumn.ConnectionType)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.ConnectionType)];
					//entity.ConnectionType = (Convert.IsDBNull(reader["ConnectionType"]))?string.Empty:(System.String)reader["ConnectionType"];
					entity.ConnectionSpeed = (reader.IsDBNull(((int)METAView_Network_ListingColumn.ConnectionSpeed)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.ConnectionSpeed)];
					//entity.ConnectionSpeed = (Convert.IsDBNull(reader["ConnectionSpeed"]))?string.Empty:(System.String)reader["ConnectionSpeed"];
					entity.ConnectionSize = (reader.IsDBNull(((int)METAView_Network_ListingColumn.ConnectionSize)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.ConnectionSize)];
					//entity.ConnectionSize = (Convert.IsDBNull(reader["ConnectionSize"]))?string.Empty:(System.String)reader["ConnectionSize"];
					entity.Description = (reader.IsDBNull(((int)METAView_Network_ListingColumn.Description)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.Description)];
					//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
					entity.Abbreviation = (reader.IsDBNull(((int)METAView_Network_ListingColumn.Abbreviation)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.Abbreviation)];
					//entity.Abbreviation = (Convert.IsDBNull(reader["Abbreviation"]))?string.Empty:(System.String)reader["Abbreviation"];
					entity.IsBusinessExternal = (reader.IsDBNull(((int)METAView_Network_ListingColumn.IsBusinessExternal)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.IsBusinessExternal)];
					//entity.IsBusinessExternal = (Convert.IsDBNull(reader["IsBusinessExternal"]))?string.Empty:(System.String)reader["IsBusinessExternal"];
					entity.DataSourceID = (reader.IsDBNull(((int)METAView_Network_ListingColumn.DataSourceID)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.DataSourceID)];
					//entity.DataSourceID = (Convert.IsDBNull(reader["DataSourceID"]))?string.Empty:(System.String)reader["DataSourceID"];
					entity.DataSourceName = (reader.IsDBNull(((int)METAView_Network_ListingColumn.DataSourceName)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.DataSourceName)];
					//entity.DataSourceName = (Convert.IsDBNull(reader["DataSourceName"]))?string.Empty:(System.String)reader["DataSourceName"];
					entity.GeneralRemarks = (reader.IsDBNull(((int)METAView_Network_ListingColumn.GeneralRemarks)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.GeneralRemarks)];
					//entity.GeneralRemarks = (Convert.IsDBNull(reader["GeneralRemarks"]))?string.Empty:(System.String)reader["GeneralRemarks"];
					entity.GapType = (reader.IsDBNull(((int)METAView_Network_ListingColumn.GapType)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.GapType)];
					//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
					entity.PrimaryOrBackupType = (reader.IsDBNull(((int)METAView_Network_ListingColumn.PrimaryOrBackupType)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.PrimaryOrBackupType)];
					//entity.PrimaryOrBackupType = (Convert.IsDBNull(reader["PrimaryOrBackupType"]))?string.Empty:(System.String)reader["PrimaryOrBackupType"];
					entity.NetworkRange = (reader.IsDBNull(((int)METAView_Network_ListingColumn.NetworkRange)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.NetworkRange)];
					//entity.NetworkRange = (Convert.IsDBNull(reader["NetworkRange"]))?string.Empty:(System.String)reader["NetworkRange"];
					entity.ArchitectureStatus = (reader.IsDBNull(((int)METAView_Network_ListingColumn.ArchitectureStatus)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.ArchitectureStatus)];
					//entity.ArchitectureStatus = (Convert.IsDBNull(reader["ArchitectureStatus"]))?string.Empty:(System.String)reader["ArchitectureStatus"];
					entity.ArchitectureStatusDate = (reader.IsDBNull(((int)METAView_Network_ListingColumn.ArchitectureStatusDate)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.ArchitectureStatusDate)];
					//entity.ArchitectureStatusDate = (Convert.IsDBNull(reader["ArchitectureStatusDate"]))?string.Empty:(System.String)reader["ArchitectureStatusDate"];
					entity.DesignRationale = (reader.IsDBNull(((int)METAView_Network_ListingColumn.DesignRationale)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.DesignRationale)];
					//entity.DesignRationale = (Convert.IsDBNull(reader["DesignRationale"]))?string.Empty:(System.String)reader["DesignRationale"];
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
		/// Refreshes the <see cref="METAView_Network_Listing"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_Network_Listing"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAView_Network_Listing entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAView_Network_ListingColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_Network_ListingColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_Network_ListingColumn.WorkspaceTypeId)];
			//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
			entity.VCStatusID = (System.Int32)reader[((int)METAView_Network_ListingColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.pkid = (System.Int32)reader[((int)METAView_Network_ListingColumn.pkid)];
			//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAView_Network_ListingColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAView_Network_ListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Name = (reader.IsDBNull(((int)METAView_Network_ListingColumn.Name)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.Name)];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.NetworkType = (reader.IsDBNull(((int)METAView_Network_ListingColumn.NetworkType)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.NetworkType)];
			//entity.NetworkType = (Convert.IsDBNull(reader["NetworkType"]))?string.Empty:(System.String)reader["NetworkType"];
			entity.DataStorageNetworkType = (reader.IsDBNull(((int)METAView_Network_ListingColumn.DataStorageNetworkType)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.DataStorageNetworkType)];
			//entity.DataStorageNetworkType = (Convert.IsDBNull(reader["DataStorageNetworkType"]))?string.Empty:(System.String)reader["DataStorageNetworkType"];
			entity.ConnectionType = (reader.IsDBNull(((int)METAView_Network_ListingColumn.ConnectionType)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.ConnectionType)];
			//entity.ConnectionType = (Convert.IsDBNull(reader["ConnectionType"]))?string.Empty:(System.String)reader["ConnectionType"];
			entity.ConnectionSpeed = (reader.IsDBNull(((int)METAView_Network_ListingColumn.ConnectionSpeed)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.ConnectionSpeed)];
			//entity.ConnectionSpeed = (Convert.IsDBNull(reader["ConnectionSpeed"]))?string.Empty:(System.String)reader["ConnectionSpeed"];
			entity.ConnectionSize = (reader.IsDBNull(((int)METAView_Network_ListingColumn.ConnectionSize)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.ConnectionSize)];
			//entity.ConnectionSize = (Convert.IsDBNull(reader["ConnectionSize"]))?string.Empty:(System.String)reader["ConnectionSize"];
			entity.Description = (reader.IsDBNull(((int)METAView_Network_ListingColumn.Description)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.Description)];
			//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
			entity.Abbreviation = (reader.IsDBNull(((int)METAView_Network_ListingColumn.Abbreviation)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.Abbreviation)];
			//entity.Abbreviation = (Convert.IsDBNull(reader["Abbreviation"]))?string.Empty:(System.String)reader["Abbreviation"];
			entity.IsBusinessExternal = (reader.IsDBNull(((int)METAView_Network_ListingColumn.IsBusinessExternal)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.IsBusinessExternal)];
			//entity.IsBusinessExternal = (Convert.IsDBNull(reader["IsBusinessExternal"]))?string.Empty:(System.String)reader["IsBusinessExternal"];
			entity.DataSourceID = (reader.IsDBNull(((int)METAView_Network_ListingColumn.DataSourceID)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.DataSourceID)];
			//entity.DataSourceID = (Convert.IsDBNull(reader["DataSourceID"]))?string.Empty:(System.String)reader["DataSourceID"];
			entity.DataSourceName = (reader.IsDBNull(((int)METAView_Network_ListingColumn.DataSourceName)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.DataSourceName)];
			//entity.DataSourceName = (Convert.IsDBNull(reader["DataSourceName"]))?string.Empty:(System.String)reader["DataSourceName"];
			entity.GeneralRemarks = (reader.IsDBNull(((int)METAView_Network_ListingColumn.GeneralRemarks)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.GeneralRemarks)];
			//entity.GeneralRemarks = (Convert.IsDBNull(reader["GeneralRemarks"]))?string.Empty:(System.String)reader["GeneralRemarks"];
			entity.GapType = (reader.IsDBNull(((int)METAView_Network_ListingColumn.GapType)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			entity.PrimaryOrBackupType = (reader.IsDBNull(((int)METAView_Network_ListingColumn.PrimaryOrBackupType)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.PrimaryOrBackupType)];
			//entity.PrimaryOrBackupType = (Convert.IsDBNull(reader["PrimaryOrBackupType"]))?string.Empty:(System.String)reader["PrimaryOrBackupType"];
			entity.NetworkRange = (reader.IsDBNull(((int)METAView_Network_ListingColumn.NetworkRange)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.NetworkRange)];
			//entity.NetworkRange = (Convert.IsDBNull(reader["NetworkRange"]))?string.Empty:(System.String)reader["NetworkRange"];
			entity.ArchitectureStatus = (reader.IsDBNull(((int)METAView_Network_ListingColumn.ArchitectureStatus)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.ArchitectureStatus)];
			//entity.ArchitectureStatus = (Convert.IsDBNull(reader["ArchitectureStatus"]))?string.Empty:(System.String)reader["ArchitectureStatus"];
			entity.ArchitectureStatusDate = (reader.IsDBNull(((int)METAView_Network_ListingColumn.ArchitectureStatusDate)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.ArchitectureStatusDate)];
			//entity.ArchitectureStatusDate = (Convert.IsDBNull(reader["ArchitectureStatusDate"]))?string.Empty:(System.String)reader["ArchitectureStatusDate"];
			entity.DesignRationale = (reader.IsDBNull(((int)METAView_Network_ListingColumn.DesignRationale)))?null:(System.String)reader[((int)METAView_Network_ListingColumn.DesignRationale)];
			//entity.DesignRationale = (Convert.IsDBNull(reader["DesignRationale"]))?string.Empty:(System.String)reader["DesignRationale"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAView_Network_Listing"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_Network_Listing"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAView_Network_Listing entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeId = (Convert.IsDBNull(dataRow["WorkspaceTypeId"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeId"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.Name = (Convert.IsDBNull(dataRow["Name"]))?string.Empty:(System.String)dataRow["Name"];
			entity.NetworkType = (Convert.IsDBNull(dataRow["NetworkType"]))?string.Empty:(System.String)dataRow["NetworkType"];
			entity.DataStorageNetworkType = (Convert.IsDBNull(dataRow["DataStorageNetworkType"]))?string.Empty:(System.String)dataRow["DataStorageNetworkType"];
			entity.ConnectionType = (Convert.IsDBNull(dataRow["ConnectionType"]))?string.Empty:(System.String)dataRow["ConnectionType"];
			entity.ConnectionSpeed = (Convert.IsDBNull(dataRow["ConnectionSpeed"]))?string.Empty:(System.String)dataRow["ConnectionSpeed"];
			entity.ConnectionSize = (Convert.IsDBNull(dataRow["ConnectionSize"]))?string.Empty:(System.String)dataRow["ConnectionSize"];
			entity.Description = (Convert.IsDBNull(dataRow["Description"]))?string.Empty:(System.String)dataRow["Description"];
			entity.Abbreviation = (Convert.IsDBNull(dataRow["Abbreviation"]))?string.Empty:(System.String)dataRow["Abbreviation"];
			entity.IsBusinessExternal = (Convert.IsDBNull(dataRow["IsBusinessExternal"]))?string.Empty:(System.String)dataRow["IsBusinessExternal"];
			entity.DataSourceID = (Convert.IsDBNull(dataRow["DataSourceID"]))?string.Empty:(System.String)dataRow["DataSourceID"];
			entity.DataSourceName = (Convert.IsDBNull(dataRow["DataSourceName"]))?string.Empty:(System.String)dataRow["DataSourceName"];
			entity.GeneralRemarks = (Convert.IsDBNull(dataRow["GeneralRemarks"]))?string.Empty:(System.String)dataRow["GeneralRemarks"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.PrimaryOrBackupType = (Convert.IsDBNull(dataRow["PrimaryOrBackupType"]))?string.Empty:(System.String)dataRow["PrimaryOrBackupType"];
			entity.NetworkRange = (Convert.IsDBNull(dataRow["NetworkRange"]))?string.Empty:(System.String)dataRow["NetworkRange"];
			entity.ArchitectureStatus = (Convert.IsDBNull(dataRow["ArchitectureStatus"]))?string.Empty:(System.String)dataRow["ArchitectureStatus"];
			entity.ArchitectureStatusDate = (Convert.IsDBNull(dataRow["ArchitectureStatusDate"]))?string.Empty:(System.String)dataRow["ArchitectureStatusDate"];
			entity.DesignRationale = (Convert.IsDBNull(dataRow["DesignRationale"]))?string.Empty:(System.String)dataRow["DesignRationale"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAView_Network_ListingFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_Network_Listing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_Network_ListingFilterBuilder : SqlFilterBuilder<METAView_Network_ListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_Network_ListingFilterBuilder class.
		/// </summary>
		public METAView_Network_ListingFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_Network_ListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_Network_ListingFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_Network_ListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_Network_ListingFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_Network_ListingFilterBuilder

	#region METAView_Network_ListingParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_Network_Listing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_Network_ListingParameterBuilder : ParameterizedSqlFilterBuilder<METAView_Network_ListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_Network_ListingParameterBuilder class.
		/// </summary>
		public METAView_Network_ListingParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_Network_ListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_Network_ListingParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_Network_ListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_Network_ListingParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_Network_ListingParameterBuilder
	
	#region METAView_Network_ListingSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_Network_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Network_ListingSortBuilder : SqlSortBuilder<METAView_Network_ListingColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_Network_ListingSqlSortBuilder class.
		/// </summary>
		public METAView_Network_ListingSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAView_Network_ListingSortBuilder

} // end namespace
