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
	/// This class is the base class for any <see cref="METAView_StorageComponent_ListingProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAView_StorageComponent_ListingProviderBaseCore : EntityViewProviderBase<METAView_StorageComponent_Listing>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAView_StorageComponent_Listing&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAView_StorageComponent_Listing&gt;"/></returns>
		protected static VList&lt;METAView_StorageComponent_Listing&gt; Fill(DataSet dataSet, VList<METAView_StorageComponent_Listing> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAView_StorageComponent_Listing>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAView_StorageComponent_Listing&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAView_StorageComponent_Listing>"/></returns>
		protected static VList&lt;METAView_StorageComponent_Listing&gt; Fill(DataTable dataTable, VList<METAView_StorageComponent_Listing> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAView_StorageComponent_Listing c = new METAView_StorageComponent_Listing();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeId = (Convert.IsDBNull(row["WorkspaceTypeId"]))?(int)0:(System.Int32?)row["WorkspaceTypeId"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.Name = (Convert.IsDBNull(row["Name"]))?string.Empty:(System.String)row["Name"];
					c.Description = (Convert.IsDBNull(row["Description"]))?string.Empty:(System.String)row["Description"];
					c.NetworkAddress1 = (Convert.IsDBNull(row["NetworkAddress1"]))?string.Empty:(System.String)row["NetworkAddress1"];
					c.NetworkAddress2 = (Convert.IsDBNull(row["NetworkAddress2"]))?string.Empty:(System.String)row["NetworkAddress2"];
					c.NetworkAddress3 = (Convert.IsDBNull(row["NetworkAddress3"]))?string.Empty:(System.String)row["NetworkAddress3"];
					c.NetworkAddress4 = (Convert.IsDBNull(row["NetworkAddress4"]))?string.Empty:(System.String)row["NetworkAddress4"];
					c.NetworkAddress5 = (Convert.IsDBNull(row["NetworkAddress5"]))?string.Empty:(System.String)row["NetworkAddress5"];
					c.Make = (Convert.IsDBNull(row["Make"]))?string.Empty:(System.String)row["Make"];
					c.Model = (Convert.IsDBNull(row["Model"]))?string.Empty:(System.String)row["Model"];
					c.SerialNumber = (Convert.IsDBNull(row["SerialNumber"]))?string.Empty:(System.String)row["SerialNumber"];
					c.AssetNumber = (Convert.IsDBNull(row["AssetNumber"]))?string.Empty:(System.String)row["AssetNumber"];
					c.Capacity = (Convert.IsDBNull(row["Capacity"]))?string.Empty:(System.String)row["Capacity"];
					c.DatePurchased = (Convert.IsDBNull(row["DatePurchased"]))?string.Empty:(System.String)row["DatePurchased"];
					c.UnderWarranty = (Convert.IsDBNull(row["UnderWarranty"]))?string.Empty:(System.String)row["UnderWarranty"];
					c.FileSystem = (Convert.IsDBNull(row["FileSystem"]))?string.Empty:(System.String)row["FileSystem"];
					c.GapType = (Convert.IsDBNull(row["GapType"]))?string.Empty:(System.String)row["GapType"];
					c.StorageComponentType = (Convert.IsDBNull(row["StorageComponentType"]))?string.Empty:(System.String)row["StorageComponentType"];
					c.IsPrimaryOrBackup = (Convert.IsDBNull(row["IsPrimaryOrBackup"]))?string.Empty:(System.String)row["IsPrimaryOrBackup"];
					c.SeverityRating = (Convert.IsDBNull(row["SeverityRating"]))?string.Empty:(System.String)row["SeverityRating"];
					c.ConfigurationID = (Convert.IsDBNull(row["ConfigurationID"]))?string.Empty:(System.String)row["ConfigurationID"];
					c.ModelNumber = (Convert.IsDBNull(row["ModelNumber"]))?string.Empty:(System.String)row["ModelNumber"];
					c.isManaged = (Convert.IsDBNull(row["isManaged"]))?string.Empty:(System.String)row["isManaged"];
					c.ContractNumber = (Convert.IsDBNull(row["ContractNumber"]))?string.Empty:(System.String)row["ContractNumber"];
					c.NumberofDisks = (Convert.IsDBNull(row["NumberofDisks"]))?string.Empty:(System.String)row["NumberofDisks"];
					c.Abbreviation = (Convert.IsDBNull(row["Abbreviation"]))?string.Empty:(System.String)row["Abbreviation"];
					c.IsBusinessExternal = (Convert.IsDBNull(row["IsBusinessExternal"]))?string.Empty:(System.String)row["IsBusinessExternal"];
					c.DataSourceID = (Convert.IsDBNull(row["DataSourceID"]))?string.Empty:(System.String)row["DataSourceID"];
					c.DataSourceName = (Convert.IsDBNull(row["DataSourceName"]))?string.Empty:(System.String)row["DataSourceName"];
					c.GeneralRemarks = (Convert.IsDBNull(row["GeneralRemarks"]))?string.Empty:(System.String)row["GeneralRemarks"];
					c.PrimaryOrBackupType = (Convert.IsDBNull(row["PrimaryOrBackupType"]))?string.Empty:(System.String)row["PrimaryOrBackupType"];
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
		/// Fill an <see cref="VList&lt;METAView_StorageComponent_Listing&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAView_StorageComponent_Listing&gt;"/></returns>
		protected VList<METAView_StorageComponent_Listing> Fill(IDataReader reader, VList<METAView_StorageComponent_Listing> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAView_StorageComponent_Listing entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAView_StorageComponent_Listing>("METAView_StorageComponent_Listing",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAView_StorageComponent_Listing();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAView_StorageComponent_ListingColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_StorageComponent_ListingColumn.WorkspaceTypeId)];
					//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
					entity.VCStatusID = (System.Int32)reader[((int)METAView_StorageComponent_ListingColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.pkid = (System.Int32)reader[((int)METAView_StorageComponent_ListingColumn.pkid)];
					//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAView_StorageComponent_ListingColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Name = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.Name)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.Name)];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.Description = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.Description)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.Description)];
					//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
					entity.NetworkAddress1 = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.NetworkAddress1)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.NetworkAddress1)];
					//entity.NetworkAddress1 = (Convert.IsDBNull(reader["NetworkAddress1"]))?string.Empty:(System.String)reader["NetworkAddress1"];
					entity.NetworkAddress2 = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.NetworkAddress2)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.NetworkAddress2)];
					//entity.NetworkAddress2 = (Convert.IsDBNull(reader["NetworkAddress2"]))?string.Empty:(System.String)reader["NetworkAddress2"];
					entity.NetworkAddress3 = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.NetworkAddress3)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.NetworkAddress3)];
					//entity.NetworkAddress3 = (Convert.IsDBNull(reader["NetworkAddress3"]))?string.Empty:(System.String)reader["NetworkAddress3"];
					entity.NetworkAddress4 = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.NetworkAddress4)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.NetworkAddress4)];
					//entity.NetworkAddress4 = (Convert.IsDBNull(reader["NetworkAddress4"]))?string.Empty:(System.String)reader["NetworkAddress4"];
					entity.NetworkAddress5 = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.NetworkAddress5)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.NetworkAddress5)];
					//entity.NetworkAddress5 = (Convert.IsDBNull(reader["NetworkAddress5"]))?string.Empty:(System.String)reader["NetworkAddress5"];
					entity.Make = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.Make)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.Make)];
					//entity.Make = (Convert.IsDBNull(reader["Make"]))?string.Empty:(System.String)reader["Make"];
					entity.Model = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.Model)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.Model)];
					//entity.Model = (Convert.IsDBNull(reader["Model"]))?string.Empty:(System.String)reader["Model"];
					entity.SerialNumber = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.SerialNumber)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.SerialNumber)];
					//entity.SerialNumber = (Convert.IsDBNull(reader["SerialNumber"]))?string.Empty:(System.String)reader["SerialNumber"];
					entity.AssetNumber = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.AssetNumber)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.AssetNumber)];
					//entity.AssetNumber = (Convert.IsDBNull(reader["AssetNumber"]))?string.Empty:(System.String)reader["AssetNumber"];
					entity.Capacity = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.Capacity)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.Capacity)];
					//entity.Capacity = (Convert.IsDBNull(reader["Capacity"]))?string.Empty:(System.String)reader["Capacity"];
					entity.DatePurchased = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.DatePurchased)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.DatePurchased)];
					//entity.DatePurchased = (Convert.IsDBNull(reader["DatePurchased"]))?string.Empty:(System.String)reader["DatePurchased"];
					entity.UnderWarranty = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.UnderWarranty)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.UnderWarranty)];
					//entity.UnderWarranty = (Convert.IsDBNull(reader["UnderWarranty"]))?string.Empty:(System.String)reader["UnderWarranty"];
					entity.FileSystem = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.FileSystem)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.FileSystem)];
					//entity.FileSystem = (Convert.IsDBNull(reader["FileSystem"]))?string.Empty:(System.String)reader["FileSystem"];
					entity.GapType = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.GapType)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.GapType)];
					//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
					entity.StorageComponentType = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.StorageComponentType)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.StorageComponentType)];
					//entity.StorageComponentType = (Convert.IsDBNull(reader["StorageComponentType"]))?string.Empty:(System.String)reader["StorageComponentType"];
					entity.IsPrimaryOrBackup = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.IsPrimaryOrBackup)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.IsPrimaryOrBackup)];
					//entity.IsPrimaryOrBackup = (Convert.IsDBNull(reader["IsPrimaryOrBackup"]))?string.Empty:(System.String)reader["IsPrimaryOrBackup"];
					entity.SeverityRating = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.SeverityRating)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.SeverityRating)];
					//entity.SeverityRating = (Convert.IsDBNull(reader["SeverityRating"]))?string.Empty:(System.String)reader["SeverityRating"];
					entity.ConfigurationID = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.ConfigurationID)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.ConfigurationID)];
					//entity.ConfigurationID = (Convert.IsDBNull(reader["ConfigurationID"]))?string.Empty:(System.String)reader["ConfigurationID"];
					entity.ModelNumber = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.ModelNumber)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.ModelNumber)];
					//entity.ModelNumber = (Convert.IsDBNull(reader["ModelNumber"]))?string.Empty:(System.String)reader["ModelNumber"];
					entity.isManaged = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.isManaged)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.isManaged)];
					//entity.isManaged = (Convert.IsDBNull(reader["isManaged"]))?string.Empty:(System.String)reader["isManaged"];
					entity.ContractNumber = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.ContractNumber)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.ContractNumber)];
					//entity.ContractNumber = (Convert.IsDBNull(reader["ContractNumber"]))?string.Empty:(System.String)reader["ContractNumber"];
					entity.NumberofDisks = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.NumberofDisks)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.NumberofDisks)];
					//entity.NumberofDisks = (Convert.IsDBNull(reader["NumberofDisks"]))?string.Empty:(System.String)reader["NumberofDisks"];
					entity.Abbreviation = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.Abbreviation)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.Abbreviation)];
					//entity.Abbreviation = (Convert.IsDBNull(reader["Abbreviation"]))?string.Empty:(System.String)reader["Abbreviation"];
					entity.IsBusinessExternal = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.IsBusinessExternal)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.IsBusinessExternal)];
					//entity.IsBusinessExternal = (Convert.IsDBNull(reader["IsBusinessExternal"]))?string.Empty:(System.String)reader["IsBusinessExternal"];
					entity.DataSourceID = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.DataSourceID)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.DataSourceID)];
					//entity.DataSourceID = (Convert.IsDBNull(reader["DataSourceID"]))?string.Empty:(System.String)reader["DataSourceID"];
					entity.DataSourceName = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.DataSourceName)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.DataSourceName)];
					//entity.DataSourceName = (Convert.IsDBNull(reader["DataSourceName"]))?string.Empty:(System.String)reader["DataSourceName"];
					entity.GeneralRemarks = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.GeneralRemarks)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.GeneralRemarks)];
					//entity.GeneralRemarks = (Convert.IsDBNull(reader["GeneralRemarks"]))?string.Empty:(System.String)reader["GeneralRemarks"];
					entity.PrimaryOrBackupType = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.PrimaryOrBackupType)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.PrimaryOrBackupType)];
					//entity.PrimaryOrBackupType = (Convert.IsDBNull(reader["PrimaryOrBackupType"]))?string.Empty:(System.String)reader["PrimaryOrBackupType"];
					entity.ArchitectureStatus = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.ArchitectureStatus)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.ArchitectureStatus)];
					//entity.ArchitectureStatus = (Convert.IsDBNull(reader["ArchitectureStatus"]))?string.Empty:(System.String)reader["ArchitectureStatus"];
					entity.ArchitectureStatusDate = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.ArchitectureStatusDate)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.ArchitectureStatusDate)];
					//entity.ArchitectureStatusDate = (Convert.IsDBNull(reader["ArchitectureStatusDate"]))?string.Empty:(System.String)reader["ArchitectureStatusDate"];
					entity.DesignRationale = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.DesignRationale)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.DesignRationale)];
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
		/// Refreshes the <see cref="METAView_StorageComponent_Listing"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_StorageComponent_Listing"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAView_StorageComponent_Listing entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAView_StorageComponent_ListingColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_StorageComponent_ListingColumn.WorkspaceTypeId)];
			//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
			entity.VCStatusID = (System.Int32)reader[((int)METAView_StorageComponent_ListingColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.pkid = (System.Int32)reader[((int)METAView_StorageComponent_ListingColumn.pkid)];
			//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAView_StorageComponent_ListingColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Name = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.Name)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.Name)];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.Description = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.Description)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.Description)];
			//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
			entity.NetworkAddress1 = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.NetworkAddress1)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.NetworkAddress1)];
			//entity.NetworkAddress1 = (Convert.IsDBNull(reader["NetworkAddress1"]))?string.Empty:(System.String)reader["NetworkAddress1"];
			entity.NetworkAddress2 = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.NetworkAddress2)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.NetworkAddress2)];
			//entity.NetworkAddress2 = (Convert.IsDBNull(reader["NetworkAddress2"]))?string.Empty:(System.String)reader["NetworkAddress2"];
			entity.NetworkAddress3 = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.NetworkAddress3)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.NetworkAddress3)];
			//entity.NetworkAddress3 = (Convert.IsDBNull(reader["NetworkAddress3"]))?string.Empty:(System.String)reader["NetworkAddress3"];
			entity.NetworkAddress4 = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.NetworkAddress4)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.NetworkAddress4)];
			//entity.NetworkAddress4 = (Convert.IsDBNull(reader["NetworkAddress4"]))?string.Empty:(System.String)reader["NetworkAddress4"];
			entity.NetworkAddress5 = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.NetworkAddress5)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.NetworkAddress5)];
			//entity.NetworkAddress5 = (Convert.IsDBNull(reader["NetworkAddress5"]))?string.Empty:(System.String)reader["NetworkAddress5"];
			entity.Make = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.Make)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.Make)];
			//entity.Make = (Convert.IsDBNull(reader["Make"]))?string.Empty:(System.String)reader["Make"];
			entity.Model = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.Model)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.Model)];
			//entity.Model = (Convert.IsDBNull(reader["Model"]))?string.Empty:(System.String)reader["Model"];
			entity.SerialNumber = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.SerialNumber)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.SerialNumber)];
			//entity.SerialNumber = (Convert.IsDBNull(reader["SerialNumber"]))?string.Empty:(System.String)reader["SerialNumber"];
			entity.AssetNumber = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.AssetNumber)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.AssetNumber)];
			//entity.AssetNumber = (Convert.IsDBNull(reader["AssetNumber"]))?string.Empty:(System.String)reader["AssetNumber"];
			entity.Capacity = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.Capacity)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.Capacity)];
			//entity.Capacity = (Convert.IsDBNull(reader["Capacity"]))?string.Empty:(System.String)reader["Capacity"];
			entity.DatePurchased = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.DatePurchased)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.DatePurchased)];
			//entity.DatePurchased = (Convert.IsDBNull(reader["DatePurchased"]))?string.Empty:(System.String)reader["DatePurchased"];
			entity.UnderWarranty = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.UnderWarranty)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.UnderWarranty)];
			//entity.UnderWarranty = (Convert.IsDBNull(reader["UnderWarranty"]))?string.Empty:(System.String)reader["UnderWarranty"];
			entity.FileSystem = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.FileSystem)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.FileSystem)];
			//entity.FileSystem = (Convert.IsDBNull(reader["FileSystem"]))?string.Empty:(System.String)reader["FileSystem"];
			entity.GapType = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.GapType)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			entity.StorageComponentType = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.StorageComponentType)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.StorageComponentType)];
			//entity.StorageComponentType = (Convert.IsDBNull(reader["StorageComponentType"]))?string.Empty:(System.String)reader["StorageComponentType"];
			entity.IsPrimaryOrBackup = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.IsPrimaryOrBackup)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.IsPrimaryOrBackup)];
			//entity.IsPrimaryOrBackup = (Convert.IsDBNull(reader["IsPrimaryOrBackup"]))?string.Empty:(System.String)reader["IsPrimaryOrBackup"];
			entity.SeverityRating = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.SeverityRating)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.SeverityRating)];
			//entity.SeverityRating = (Convert.IsDBNull(reader["SeverityRating"]))?string.Empty:(System.String)reader["SeverityRating"];
			entity.ConfigurationID = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.ConfigurationID)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.ConfigurationID)];
			//entity.ConfigurationID = (Convert.IsDBNull(reader["ConfigurationID"]))?string.Empty:(System.String)reader["ConfigurationID"];
			entity.ModelNumber = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.ModelNumber)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.ModelNumber)];
			//entity.ModelNumber = (Convert.IsDBNull(reader["ModelNumber"]))?string.Empty:(System.String)reader["ModelNumber"];
			entity.isManaged = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.isManaged)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.isManaged)];
			//entity.isManaged = (Convert.IsDBNull(reader["isManaged"]))?string.Empty:(System.String)reader["isManaged"];
			entity.ContractNumber = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.ContractNumber)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.ContractNumber)];
			//entity.ContractNumber = (Convert.IsDBNull(reader["ContractNumber"]))?string.Empty:(System.String)reader["ContractNumber"];
			entity.NumberofDisks = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.NumberofDisks)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.NumberofDisks)];
			//entity.NumberofDisks = (Convert.IsDBNull(reader["NumberofDisks"]))?string.Empty:(System.String)reader["NumberofDisks"];
			entity.Abbreviation = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.Abbreviation)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.Abbreviation)];
			//entity.Abbreviation = (Convert.IsDBNull(reader["Abbreviation"]))?string.Empty:(System.String)reader["Abbreviation"];
			entity.IsBusinessExternal = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.IsBusinessExternal)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.IsBusinessExternal)];
			//entity.IsBusinessExternal = (Convert.IsDBNull(reader["IsBusinessExternal"]))?string.Empty:(System.String)reader["IsBusinessExternal"];
			entity.DataSourceID = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.DataSourceID)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.DataSourceID)];
			//entity.DataSourceID = (Convert.IsDBNull(reader["DataSourceID"]))?string.Empty:(System.String)reader["DataSourceID"];
			entity.DataSourceName = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.DataSourceName)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.DataSourceName)];
			//entity.DataSourceName = (Convert.IsDBNull(reader["DataSourceName"]))?string.Empty:(System.String)reader["DataSourceName"];
			entity.GeneralRemarks = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.GeneralRemarks)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.GeneralRemarks)];
			//entity.GeneralRemarks = (Convert.IsDBNull(reader["GeneralRemarks"]))?string.Empty:(System.String)reader["GeneralRemarks"];
			entity.PrimaryOrBackupType = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.PrimaryOrBackupType)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.PrimaryOrBackupType)];
			//entity.PrimaryOrBackupType = (Convert.IsDBNull(reader["PrimaryOrBackupType"]))?string.Empty:(System.String)reader["PrimaryOrBackupType"];
			entity.ArchitectureStatus = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.ArchitectureStatus)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.ArchitectureStatus)];
			//entity.ArchitectureStatus = (Convert.IsDBNull(reader["ArchitectureStatus"]))?string.Empty:(System.String)reader["ArchitectureStatus"];
			entity.ArchitectureStatusDate = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.ArchitectureStatusDate)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.ArchitectureStatusDate)];
			//entity.ArchitectureStatusDate = (Convert.IsDBNull(reader["ArchitectureStatusDate"]))?string.Empty:(System.String)reader["ArchitectureStatusDate"];
			entity.DesignRationale = (reader.IsDBNull(((int)METAView_StorageComponent_ListingColumn.DesignRationale)))?null:(System.String)reader[((int)METAView_StorageComponent_ListingColumn.DesignRationale)];
			//entity.DesignRationale = (Convert.IsDBNull(reader["DesignRationale"]))?string.Empty:(System.String)reader["DesignRationale"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAView_StorageComponent_Listing"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_StorageComponent_Listing"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAView_StorageComponent_Listing entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeId = (Convert.IsDBNull(dataRow["WorkspaceTypeId"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeId"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.Name = (Convert.IsDBNull(dataRow["Name"]))?string.Empty:(System.String)dataRow["Name"];
			entity.Description = (Convert.IsDBNull(dataRow["Description"]))?string.Empty:(System.String)dataRow["Description"];
			entity.NetworkAddress1 = (Convert.IsDBNull(dataRow["NetworkAddress1"]))?string.Empty:(System.String)dataRow["NetworkAddress1"];
			entity.NetworkAddress2 = (Convert.IsDBNull(dataRow["NetworkAddress2"]))?string.Empty:(System.String)dataRow["NetworkAddress2"];
			entity.NetworkAddress3 = (Convert.IsDBNull(dataRow["NetworkAddress3"]))?string.Empty:(System.String)dataRow["NetworkAddress3"];
			entity.NetworkAddress4 = (Convert.IsDBNull(dataRow["NetworkAddress4"]))?string.Empty:(System.String)dataRow["NetworkAddress4"];
			entity.NetworkAddress5 = (Convert.IsDBNull(dataRow["NetworkAddress5"]))?string.Empty:(System.String)dataRow["NetworkAddress5"];
			entity.Make = (Convert.IsDBNull(dataRow["Make"]))?string.Empty:(System.String)dataRow["Make"];
			entity.Model = (Convert.IsDBNull(dataRow["Model"]))?string.Empty:(System.String)dataRow["Model"];
			entity.SerialNumber = (Convert.IsDBNull(dataRow["SerialNumber"]))?string.Empty:(System.String)dataRow["SerialNumber"];
			entity.AssetNumber = (Convert.IsDBNull(dataRow["AssetNumber"]))?string.Empty:(System.String)dataRow["AssetNumber"];
			entity.Capacity = (Convert.IsDBNull(dataRow["Capacity"]))?string.Empty:(System.String)dataRow["Capacity"];
			entity.DatePurchased = (Convert.IsDBNull(dataRow["DatePurchased"]))?string.Empty:(System.String)dataRow["DatePurchased"];
			entity.UnderWarranty = (Convert.IsDBNull(dataRow["UnderWarranty"]))?string.Empty:(System.String)dataRow["UnderWarranty"];
			entity.FileSystem = (Convert.IsDBNull(dataRow["FileSystem"]))?string.Empty:(System.String)dataRow["FileSystem"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.StorageComponentType = (Convert.IsDBNull(dataRow["StorageComponentType"]))?string.Empty:(System.String)dataRow["StorageComponentType"];
			entity.IsPrimaryOrBackup = (Convert.IsDBNull(dataRow["IsPrimaryOrBackup"]))?string.Empty:(System.String)dataRow["IsPrimaryOrBackup"];
			entity.SeverityRating = (Convert.IsDBNull(dataRow["SeverityRating"]))?string.Empty:(System.String)dataRow["SeverityRating"];
			entity.ConfigurationID = (Convert.IsDBNull(dataRow["ConfigurationID"]))?string.Empty:(System.String)dataRow["ConfigurationID"];
			entity.ModelNumber = (Convert.IsDBNull(dataRow["ModelNumber"]))?string.Empty:(System.String)dataRow["ModelNumber"];
			entity.isManaged = (Convert.IsDBNull(dataRow["isManaged"]))?string.Empty:(System.String)dataRow["isManaged"];
			entity.ContractNumber = (Convert.IsDBNull(dataRow["ContractNumber"]))?string.Empty:(System.String)dataRow["ContractNumber"];
			entity.NumberofDisks = (Convert.IsDBNull(dataRow["NumberofDisks"]))?string.Empty:(System.String)dataRow["NumberofDisks"];
			entity.Abbreviation = (Convert.IsDBNull(dataRow["Abbreviation"]))?string.Empty:(System.String)dataRow["Abbreviation"];
			entity.IsBusinessExternal = (Convert.IsDBNull(dataRow["IsBusinessExternal"]))?string.Empty:(System.String)dataRow["IsBusinessExternal"];
			entity.DataSourceID = (Convert.IsDBNull(dataRow["DataSourceID"]))?string.Empty:(System.String)dataRow["DataSourceID"];
			entity.DataSourceName = (Convert.IsDBNull(dataRow["DataSourceName"]))?string.Empty:(System.String)dataRow["DataSourceName"];
			entity.GeneralRemarks = (Convert.IsDBNull(dataRow["GeneralRemarks"]))?string.Empty:(System.String)dataRow["GeneralRemarks"];
			entity.PrimaryOrBackupType = (Convert.IsDBNull(dataRow["PrimaryOrBackupType"]))?string.Empty:(System.String)dataRow["PrimaryOrBackupType"];
			entity.ArchitectureStatus = (Convert.IsDBNull(dataRow["ArchitectureStatus"]))?string.Empty:(System.String)dataRow["ArchitectureStatus"];
			entity.ArchitectureStatusDate = (Convert.IsDBNull(dataRow["ArchitectureStatusDate"]))?string.Empty:(System.String)dataRow["ArchitectureStatusDate"];
			entity.DesignRationale = (Convert.IsDBNull(dataRow["DesignRationale"]))?string.Empty:(System.String)dataRow["DesignRationale"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAView_StorageComponent_ListingFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_StorageComponent_Listing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_StorageComponent_ListingFilterBuilder : SqlFilterBuilder<METAView_StorageComponent_ListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_StorageComponent_ListingFilterBuilder class.
		/// </summary>
		public METAView_StorageComponent_ListingFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_StorageComponent_ListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_StorageComponent_ListingFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_StorageComponent_ListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_StorageComponent_ListingFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_StorageComponent_ListingFilterBuilder

	#region METAView_StorageComponent_ListingParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_StorageComponent_Listing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_StorageComponent_ListingParameterBuilder : ParameterizedSqlFilterBuilder<METAView_StorageComponent_ListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_StorageComponent_ListingParameterBuilder class.
		/// </summary>
		public METAView_StorageComponent_ListingParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_StorageComponent_ListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_StorageComponent_ListingParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_StorageComponent_ListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_StorageComponent_ListingParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_StorageComponent_ListingParameterBuilder
	
	#region METAView_StorageComponent_ListingSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_StorageComponent_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_StorageComponent_ListingSortBuilder : SqlSortBuilder<METAView_StorageComponent_ListingColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_StorageComponent_ListingSqlSortBuilder class.
		/// </summary>
		public METAView_StorageComponent_ListingSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAView_StorageComponent_ListingSortBuilder

} // end namespace
