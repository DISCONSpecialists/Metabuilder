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
	/// This class is the base class for any <see cref="METAView_SystemComponent_RetrievalProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAView_SystemComponent_RetrievalProviderBaseCore : EntityViewProviderBase<METAView_SystemComponent_Retrieval>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAView_SystemComponent_Retrieval&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAView_SystemComponent_Retrieval&gt;"/></returns>
		protected static VList&lt;METAView_SystemComponent_Retrieval&gt; Fill(DataSet dataSet, VList<METAView_SystemComponent_Retrieval> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAView_SystemComponent_Retrieval>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAView_SystemComponent_Retrieval&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAView_SystemComponent_Retrieval>"/></returns>
		protected static VList&lt;METAView_SystemComponent_Retrieval&gt; Fill(DataTable dataTable, VList<METAView_SystemComponent_Retrieval> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAView_SystemComponent_Retrieval c = new METAView_SystemComponent_Retrieval();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeId = (Convert.IsDBNull(row["WorkspaceTypeId"]))?(int)0:(System.Int32?)row["WorkspaceTypeId"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.Type = (Convert.IsDBNull(row["Type"]))?string.Empty:(System.String)row["Type"];
					c.Name = (Convert.IsDBNull(row["Name"]))?string.Empty:(System.String)row["Name"];
					c.Description = (Convert.IsDBNull(row["Description"]))?string.Empty:(System.String)row["Description"];
					c.SeverityRating = (Convert.IsDBNull(row["SeverityRating"]))?string.Empty:(System.String)row["SeverityRating"];
					c.Configuration = (Convert.IsDBNull(row["Configuration"]))?string.Empty:(System.String)row["Configuration"];
					c.MACAddress = (Convert.IsDBNull(row["MACAddress"]))?string.Empty:(System.String)row["MACAddress"];
					c.StaticIP = (Convert.IsDBNull(row["StaticIP"]))?string.Empty:(System.String)row["StaticIP"];
					c.NetworkAddress3 = (Convert.IsDBNull(row["NetworkAddress3"]))?string.Empty:(System.String)row["NetworkAddress3"];
					c.NetworkAddress4 = (Convert.IsDBNull(row["NetworkAddress4"]))?string.Empty:(System.String)row["NetworkAddress4"];
					c.NetworkAddress5 = (Convert.IsDBNull(row["NetworkAddress5"]))?string.Empty:(System.String)row["NetworkAddress5"];
					c.Make = (Convert.IsDBNull(row["Make"]))?string.Empty:(System.String)row["Make"];
					c.Model = (Convert.IsDBNull(row["Model"]))?string.Empty:(System.String)row["Model"];
					c.SerialNumber = (Convert.IsDBNull(row["SerialNumber"]))?string.Empty:(System.String)row["SerialNumber"];
					c.AssetNumber = (Convert.IsDBNull(row["AssetNumber"]))?string.Empty:(System.String)row["AssetNumber"];
					c.isDNS = (Convert.IsDBNull(row["isDNS"]))?string.Empty:(System.String)row["isDNS"];
					c.isDHCP = (Convert.IsDBNull(row["isDHCP"]))?string.Empty:(System.String)row["isDHCP"];
					c.Capacity = (Convert.IsDBNull(row["Capacity"]))?string.Empty:(System.String)row["Capacity"];
					c.Mem_Total = (Convert.IsDBNull(row["Mem_Total"]))?string.Empty:(System.String)row["Mem_Total"];
					c.CPU_Type = (Convert.IsDBNull(row["CPU_Type"]))?string.Empty:(System.String)row["CPU_Type"];
					c.CPU_Speed = (Convert.IsDBNull(row["CPU_Speed"]))?string.Empty:(System.String)row["CPU_Speed"];
					c.Monitor = (Convert.IsDBNull(row["Monitor"]))?string.Empty:(System.String)row["Monitor"];
					c.Video_Card = (Convert.IsDBNull(row["Video_Card"]))?string.Empty:(System.String)row["Video_Card"];
					c.Number_Of_Disks = (Convert.IsDBNull(row["Number_Of_Disks"]))?string.Empty:(System.String)row["Number_Of_Disks"];
					c.DatePurchased = (Convert.IsDBNull(row["DatePurchased"]))?string.Empty:(System.String)row["DatePurchased"];
					c.UnderWarranty = (Convert.IsDBNull(row["UnderWarranty"]))?string.Empty:(System.String)row["UnderWarranty"];
					c.Domain = (Convert.IsDBNull(row["Domain"]))?string.Empty:(System.String)row["Domain"];
					c.CustomField1 = (Convert.IsDBNull(row["CustomField1"]))?string.Empty:(System.String)row["CustomField1"];
					c.CustomField2 = (Convert.IsDBNull(row["CustomField2"]))?string.Empty:(System.String)row["CustomField2"];
					c.CustomField3 = (Convert.IsDBNull(row["CustomField3"]))?string.Empty:(System.String)row["CustomField3"];
					c.ServerType = (Convert.IsDBNull(row["ServerType"]))?string.Empty:(System.String)row["ServerType"];
					c.GapType = (Convert.IsDBNull(row["GapType"]))?string.Empty:(System.String)row["GapType"];
					c.AbbreviatedName = (Convert.IsDBNull(row["AbbreviatedName"]))?string.Empty:(System.String)row["AbbreviatedName"];
					c.Abbreviation = (Convert.IsDBNull(row["Abbreviation"]))?string.Empty:(System.String)row["Abbreviation"];
					c.IsBusinessExternal = (Convert.IsDBNull(row["IsBusinessExternal"]))?string.Empty:(System.String)row["IsBusinessExternal"];
					c.StandardisationStatus = (Convert.IsDBNull(row["StandardisationStatus"]))?string.Empty:(System.String)row["StandardisationStatus"];
					c.StandardisationStatusDate = (Convert.IsDBNull(row["StandardisationStatusDate"]))?string.Empty:(System.String)row["StandardisationStatusDate"];
					c.DataSourceID = (Convert.IsDBNull(row["DataSourceID"]))?string.Empty:(System.String)row["DataSourceID"];
					c.DataSourceName = (Convert.IsDBNull(row["DataSourceName"]))?string.Empty:(System.String)row["DataSourceName"];
					c.GeneralRemarks = (Convert.IsDBNull(row["GeneralRemarks"]))?string.Empty:(System.String)row["GeneralRemarks"];
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
		/// Fill an <see cref="VList&lt;METAView_SystemComponent_Retrieval&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAView_SystemComponent_Retrieval&gt;"/></returns>
		protected VList<METAView_SystemComponent_Retrieval> Fill(IDataReader reader, VList<METAView_SystemComponent_Retrieval> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAView_SystemComponent_Retrieval entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAView_SystemComponent_Retrieval>("METAView_SystemComponent_Retrieval",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAView_SystemComponent_Retrieval();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_SystemComponent_RetrievalColumn.WorkspaceTypeId)];
					//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
					entity.VCStatusID = (System.Int32)reader[((int)METAView_SystemComponent_RetrievalColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.pkid = (System.Int32)reader[((int)METAView_SystemComponent_RetrievalColumn.pkid)];
					//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Type = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.Type)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.Type)];
					//entity.Type = (Convert.IsDBNull(reader["Type"]))?string.Empty:(System.String)reader["Type"];
					entity.Name = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.Name)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.Name)];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.Description = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.Description)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.Description)];
					//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
					entity.SeverityRating = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.SeverityRating)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.SeverityRating)];
					//entity.SeverityRating = (Convert.IsDBNull(reader["SeverityRating"]))?string.Empty:(System.String)reader["SeverityRating"];
					entity.Configuration = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.Configuration)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.Configuration)];
					//entity.Configuration = (Convert.IsDBNull(reader["Configuration"]))?string.Empty:(System.String)reader["Configuration"];
					entity.MACAddress = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.MACAddress)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.MACAddress)];
					//entity.MACAddress = (Convert.IsDBNull(reader["MACAddress"]))?string.Empty:(System.String)reader["MACAddress"];
					entity.StaticIP = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.StaticIP)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.StaticIP)];
					//entity.StaticIP = (Convert.IsDBNull(reader["StaticIP"]))?string.Empty:(System.String)reader["StaticIP"];
					entity.NetworkAddress3 = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.NetworkAddress3)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.NetworkAddress3)];
					//entity.NetworkAddress3 = (Convert.IsDBNull(reader["NetworkAddress3"]))?string.Empty:(System.String)reader["NetworkAddress3"];
					entity.NetworkAddress4 = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.NetworkAddress4)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.NetworkAddress4)];
					//entity.NetworkAddress4 = (Convert.IsDBNull(reader["NetworkAddress4"]))?string.Empty:(System.String)reader["NetworkAddress4"];
					entity.NetworkAddress5 = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.NetworkAddress5)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.NetworkAddress5)];
					//entity.NetworkAddress5 = (Convert.IsDBNull(reader["NetworkAddress5"]))?string.Empty:(System.String)reader["NetworkAddress5"];
					entity.Make = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.Make)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.Make)];
					//entity.Make = (Convert.IsDBNull(reader["Make"]))?string.Empty:(System.String)reader["Make"];
					entity.Model = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.Model)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.Model)];
					//entity.Model = (Convert.IsDBNull(reader["Model"]))?string.Empty:(System.String)reader["Model"];
					entity.SerialNumber = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.SerialNumber)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.SerialNumber)];
					//entity.SerialNumber = (Convert.IsDBNull(reader["SerialNumber"]))?string.Empty:(System.String)reader["SerialNumber"];
					entity.AssetNumber = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.AssetNumber)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.AssetNumber)];
					//entity.AssetNumber = (Convert.IsDBNull(reader["AssetNumber"]))?string.Empty:(System.String)reader["AssetNumber"];
					entity.isDNS = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.isDNS)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.isDNS)];
					//entity.isDNS = (Convert.IsDBNull(reader["isDNS"]))?string.Empty:(System.String)reader["isDNS"];
					entity.isDHCP = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.isDHCP)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.isDHCP)];
					//entity.isDHCP = (Convert.IsDBNull(reader["isDHCP"]))?string.Empty:(System.String)reader["isDHCP"];
					entity.Capacity = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.Capacity)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.Capacity)];
					//entity.Capacity = (Convert.IsDBNull(reader["Capacity"]))?string.Empty:(System.String)reader["Capacity"];
					entity.Mem_Total = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.Mem_Total)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.Mem_Total)];
					//entity.Mem_Total = (Convert.IsDBNull(reader["Mem_Total"]))?string.Empty:(System.String)reader["Mem_Total"];
					entity.CPU_Type = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.CPU_Type)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.CPU_Type)];
					//entity.CPU_Type = (Convert.IsDBNull(reader["CPU_Type"]))?string.Empty:(System.String)reader["CPU_Type"];
					entity.CPU_Speed = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.CPU_Speed)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.CPU_Speed)];
					//entity.CPU_Speed = (Convert.IsDBNull(reader["CPU_Speed"]))?string.Empty:(System.String)reader["CPU_Speed"];
					entity.Monitor = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.Monitor)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.Monitor)];
					//entity.Monitor = (Convert.IsDBNull(reader["Monitor"]))?string.Empty:(System.String)reader["Monitor"];
					entity.Video_Card = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.Video_Card)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.Video_Card)];
					//entity.Video_Card = (Convert.IsDBNull(reader["Video_Card"]))?string.Empty:(System.String)reader["Video_Card"];
					entity.Number_Of_Disks = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.Number_Of_Disks)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.Number_Of_Disks)];
					//entity.Number_Of_Disks = (Convert.IsDBNull(reader["Number_Of_Disks"]))?string.Empty:(System.String)reader["Number_Of_Disks"];
					entity.DatePurchased = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.DatePurchased)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.DatePurchased)];
					//entity.DatePurchased = (Convert.IsDBNull(reader["DatePurchased"]))?string.Empty:(System.String)reader["DatePurchased"];
					entity.UnderWarranty = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.UnderWarranty)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.UnderWarranty)];
					//entity.UnderWarranty = (Convert.IsDBNull(reader["UnderWarranty"]))?string.Empty:(System.String)reader["UnderWarranty"];
					entity.Domain = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.Domain)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.Domain)];
					//entity.Domain = (Convert.IsDBNull(reader["Domain"]))?string.Empty:(System.String)reader["Domain"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.ServerType = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.ServerType)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.ServerType)];
					//entity.ServerType = (Convert.IsDBNull(reader["ServerType"]))?string.Empty:(System.String)reader["ServerType"];
					entity.GapType = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.GapType)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.GapType)];
					//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
					entity.AbbreviatedName = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.AbbreviatedName)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.AbbreviatedName)];
					//entity.AbbreviatedName = (Convert.IsDBNull(reader["AbbreviatedName"]))?string.Empty:(System.String)reader["AbbreviatedName"];
					entity.Abbreviation = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.Abbreviation)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.Abbreviation)];
					//entity.Abbreviation = (Convert.IsDBNull(reader["Abbreviation"]))?string.Empty:(System.String)reader["Abbreviation"];
					entity.IsBusinessExternal = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.IsBusinessExternal)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.IsBusinessExternal)];
					//entity.IsBusinessExternal = (Convert.IsDBNull(reader["IsBusinessExternal"]))?string.Empty:(System.String)reader["IsBusinessExternal"];
					entity.StandardisationStatus = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.StandardisationStatus)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.StandardisationStatus)];
					//entity.StandardisationStatus = (Convert.IsDBNull(reader["StandardisationStatus"]))?string.Empty:(System.String)reader["StandardisationStatus"];
					entity.StandardisationStatusDate = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.StandardisationStatusDate)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.StandardisationStatusDate)];
					//entity.StandardisationStatusDate = (Convert.IsDBNull(reader["StandardisationStatusDate"]))?string.Empty:(System.String)reader["StandardisationStatusDate"];
					entity.DataSourceID = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.DataSourceID)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.DataSourceID)];
					//entity.DataSourceID = (Convert.IsDBNull(reader["DataSourceID"]))?string.Empty:(System.String)reader["DataSourceID"];
					entity.DataSourceName = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.DataSourceName)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.DataSourceName)];
					//entity.DataSourceName = (Convert.IsDBNull(reader["DataSourceName"]))?string.Empty:(System.String)reader["DataSourceName"];
					entity.GeneralRemarks = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.GeneralRemarks)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.GeneralRemarks)];
					//entity.GeneralRemarks = (Convert.IsDBNull(reader["GeneralRemarks"]))?string.Empty:(System.String)reader["GeneralRemarks"];
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
		/// Refreshes the <see cref="METAView_SystemComponent_Retrieval"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_SystemComponent_Retrieval"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAView_SystemComponent_Retrieval entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_SystemComponent_RetrievalColumn.WorkspaceTypeId)];
			//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
			entity.VCStatusID = (System.Int32)reader[((int)METAView_SystemComponent_RetrievalColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.pkid = (System.Int32)reader[((int)METAView_SystemComponent_RetrievalColumn.pkid)];
			//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Type = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.Type)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.Type)];
			//entity.Type = (Convert.IsDBNull(reader["Type"]))?string.Empty:(System.String)reader["Type"];
			entity.Name = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.Name)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.Name)];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.Description = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.Description)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.Description)];
			//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
			entity.SeverityRating = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.SeverityRating)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.SeverityRating)];
			//entity.SeverityRating = (Convert.IsDBNull(reader["SeverityRating"]))?string.Empty:(System.String)reader["SeverityRating"];
			entity.Configuration = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.Configuration)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.Configuration)];
			//entity.Configuration = (Convert.IsDBNull(reader["Configuration"]))?string.Empty:(System.String)reader["Configuration"];
			entity.MACAddress = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.MACAddress)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.MACAddress)];
			//entity.MACAddress = (Convert.IsDBNull(reader["MACAddress"]))?string.Empty:(System.String)reader["MACAddress"];
			entity.StaticIP = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.StaticIP)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.StaticIP)];
			//entity.StaticIP = (Convert.IsDBNull(reader["StaticIP"]))?string.Empty:(System.String)reader["StaticIP"];
			entity.NetworkAddress3 = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.NetworkAddress3)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.NetworkAddress3)];
			//entity.NetworkAddress3 = (Convert.IsDBNull(reader["NetworkAddress3"]))?string.Empty:(System.String)reader["NetworkAddress3"];
			entity.NetworkAddress4 = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.NetworkAddress4)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.NetworkAddress4)];
			//entity.NetworkAddress4 = (Convert.IsDBNull(reader["NetworkAddress4"]))?string.Empty:(System.String)reader["NetworkAddress4"];
			entity.NetworkAddress5 = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.NetworkAddress5)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.NetworkAddress5)];
			//entity.NetworkAddress5 = (Convert.IsDBNull(reader["NetworkAddress5"]))?string.Empty:(System.String)reader["NetworkAddress5"];
			entity.Make = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.Make)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.Make)];
			//entity.Make = (Convert.IsDBNull(reader["Make"]))?string.Empty:(System.String)reader["Make"];
			entity.Model = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.Model)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.Model)];
			//entity.Model = (Convert.IsDBNull(reader["Model"]))?string.Empty:(System.String)reader["Model"];
			entity.SerialNumber = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.SerialNumber)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.SerialNumber)];
			//entity.SerialNumber = (Convert.IsDBNull(reader["SerialNumber"]))?string.Empty:(System.String)reader["SerialNumber"];
			entity.AssetNumber = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.AssetNumber)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.AssetNumber)];
			//entity.AssetNumber = (Convert.IsDBNull(reader["AssetNumber"]))?string.Empty:(System.String)reader["AssetNumber"];
			entity.isDNS = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.isDNS)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.isDNS)];
			//entity.isDNS = (Convert.IsDBNull(reader["isDNS"]))?string.Empty:(System.String)reader["isDNS"];
			entity.isDHCP = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.isDHCP)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.isDHCP)];
			//entity.isDHCP = (Convert.IsDBNull(reader["isDHCP"]))?string.Empty:(System.String)reader["isDHCP"];
			entity.Capacity = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.Capacity)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.Capacity)];
			//entity.Capacity = (Convert.IsDBNull(reader["Capacity"]))?string.Empty:(System.String)reader["Capacity"];
			entity.Mem_Total = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.Mem_Total)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.Mem_Total)];
			//entity.Mem_Total = (Convert.IsDBNull(reader["Mem_Total"]))?string.Empty:(System.String)reader["Mem_Total"];
			entity.CPU_Type = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.CPU_Type)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.CPU_Type)];
			//entity.CPU_Type = (Convert.IsDBNull(reader["CPU_Type"]))?string.Empty:(System.String)reader["CPU_Type"];
			entity.CPU_Speed = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.CPU_Speed)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.CPU_Speed)];
			//entity.CPU_Speed = (Convert.IsDBNull(reader["CPU_Speed"]))?string.Empty:(System.String)reader["CPU_Speed"];
			entity.Monitor = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.Monitor)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.Monitor)];
			//entity.Monitor = (Convert.IsDBNull(reader["Monitor"]))?string.Empty:(System.String)reader["Monitor"];
			entity.Video_Card = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.Video_Card)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.Video_Card)];
			//entity.Video_Card = (Convert.IsDBNull(reader["Video_Card"]))?string.Empty:(System.String)reader["Video_Card"];
			entity.Number_Of_Disks = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.Number_Of_Disks)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.Number_Of_Disks)];
			//entity.Number_Of_Disks = (Convert.IsDBNull(reader["Number_Of_Disks"]))?string.Empty:(System.String)reader["Number_Of_Disks"];
			entity.DatePurchased = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.DatePurchased)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.DatePurchased)];
			//entity.DatePurchased = (Convert.IsDBNull(reader["DatePurchased"]))?string.Empty:(System.String)reader["DatePurchased"];
			entity.UnderWarranty = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.UnderWarranty)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.UnderWarranty)];
			//entity.UnderWarranty = (Convert.IsDBNull(reader["UnderWarranty"]))?string.Empty:(System.String)reader["UnderWarranty"];
			entity.Domain = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.Domain)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.Domain)];
			//entity.Domain = (Convert.IsDBNull(reader["Domain"]))?string.Empty:(System.String)reader["Domain"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.ServerType = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.ServerType)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.ServerType)];
			//entity.ServerType = (Convert.IsDBNull(reader["ServerType"]))?string.Empty:(System.String)reader["ServerType"];
			entity.GapType = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.GapType)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			entity.AbbreviatedName = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.AbbreviatedName)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.AbbreviatedName)];
			//entity.AbbreviatedName = (Convert.IsDBNull(reader["AbbreviatedName"]))?string.Empty:(System.String)reader["AbbreviatedName"];
			entity.Abbreviation = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.Abbreviation)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.Abbreviation)];
			//entity.Abbreviation = (Convert.IsDBNull(reader["Abbreviation"]))?string.Empty:(System.String)reader["Abbreviation"];
			entity.IsBusinessExternal = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.IsBusinessExternal)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.IsBusinessExternal)];
			//entity.IsBusinessExternal = (Convert.IsDBNull(reader["IsBusinessExternal"]))?string.Empty:(System.String)reader["IsBusinessExternal"];
			entity.StandardisationStatus = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.StandardisationStatus)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.StandardisationStatus)];
			//entity.StandardisationStatus = (Convert.IsDBNull(reader["StandardisationStatus"]))?string.Empty:(System.String)reader["StandardisationStatus"];
			entity.StandardisationStatusDate = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.StandardisationStatusDate)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.StandardisationStatusDate)];
			//entity.StandardisationStatusDate = (Convert.IsDBNull(reader["StandardisationStatusDate"]))?string.Empty:(System.String)reader["StandardisationStatusDate"];
			entity.DataSourceID = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.DataSourceID)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.DataSourceID)];
			//entity.DataSourceID = (Convert.IsDBNull(reader["DataSourceID"]))?string.Empty:(System.String)reader["DataSourceID"];
			entity.DataSourceName = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.DataSourceName)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.DataSourceName)];
			//entity.DataSourceName = (Convert.IsDBNull(reader["DataSourceName"]))?string.Empty:(System.String)reader["DataSourceName"];
			entity.GeneralRemarks = (reader.IsDBNull(((int)METAView_SystemComponent_RetrievalColumn.GeneralRemarks)))?null:(System.String)reader[((int)METAView_SystemComponent_RetrievalColumn.GeneralRemarks)];
			//entity.GeneralRemarks = (Convert.IsDBNull(reader["GeneralRemarks"]))?string.Empty:(System.String)reader["GeneralRemarks"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAView_SystemComponent_Retrieval"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_SystemComponent_Retrieval"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAView_SystemComponent_Retrieval entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeId = (Convert.IsDBNull(dataRow["WorkspaceTypeId"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeId"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.Type = (Convert.IsDBNull(dataRow["Type"]))?string.Empty:(System.String)dataRow["Type"];
			entity.Name = (Convert.IsDBNull(dataRow["Name"]))?string.Empty:(System.String)dataRow["Name"];
			entity.Description = (Convert.IsDBNull(dataRow["Description"]))?string.Empty:(System.String)dataRow["Description"];
			entity.SeverityRating = (Convert.IsDBNull(dataRow["SeverityRating"]))?string.Empty:(System.String)dataRow["SeverityRating"];
			entity.Configuration = (Convert.IsDBNull(dataRow["Configuration"]))?string.Empty:(System.String)dataRow["Configuration"];
			entity.MACAddress = (Convert.IsDBNull(dataRow["MACAddress"]))?string.Empty:(System.String)dataRow["MACAddress"];
			entity.StaticIP = (Convert.IsDBNull(dataRow["StaticIP"]))?string.Empty:(System.String)dataRow["StaticIP"];
			entity.NetworkAddress3 = (Convert.IsDBNull(dataRow["NetworkAddress3"]))?string.Empty:(System.String)dataRow["NetworkAddress3"];
			entity.NetworkAddress4 = (Convert.IsDBNull(dataRow["NetworkAddress4"]))?string.Empty:(System.String)dataRow["NetworkAddress4"];
			entity.NetworkAddress5 = (Convert.IsDBNull(dataRow["NetworkAddress5"]))?string.Empty:(System.String)dataRow["NetworkAddress5"];
			entity.Make = (Convert.IsDBNull(dataRow["Make"]))?string.Empty:(System.String)dataRow["Make"];
			entity.Model = (Convert.IsDBNull(dataRow["Model"]))?string.Empty:(System.String)dataRow["Model"];
			entity.SerialNumber = (Convert.IsDBNull(dataRow["SerialNumber"]))?string.Empty:(System.String)dataRow["SerialNumber"];
			entity.AssetNumber = (Convert.IsDBNull(dataRow["AssetNumber"]))?string.Empty:(System.String)dataRow["AssetNumber"];
			entity.isDNS = (Convert.IsDBNull(dataRow["isDNS"]))?string.Empty:(System.String)dataRow["isDNS"];
			entity.isDHCP = (Convert.IsDBNull(dataRow["isDHCP"]))?string.Empty:(System.String)dataRow["isDHCP"];
			entity.Capacity = (Convert.IsDBNull(dataRow["Capacity"]))?string.Empty:(System.String)dataRow["Capacity"];
			entity.Mem_Total = (Convert.IsDBNull(dataRow["Mem_Total"]))?string.Empty:(System.String)dataRow["Mem_Total"];
			entity.CPU_Type = (Convert.IsDBNull(dataRow["CPU_Type"]))?string.Empty:(System.String)dataRow["CPU_Type"];
			entity.CPU_Speed = (Convert.IsDBNull(dataRow["CPU_Speed"]))?string.Empty:(System.String)dataRow["CPU_Speed"];
			entity.Monitor = (Convert.IsDBNull(dataRow["Monitor"]))?string.Empty:(System.String)dataRow["Monitor"];
			entity.Video_Card = (Convert.IsDBNull(dataRow["Video_Card"]))?string.Empty:(System.String)dataRow["Video_Card"];
			entity.Number_Of_Disks = (Convert.IsDBNull(dataRow["Number_Of_Disks"]))?string.Empty:(System.String)dataRow["Number_Of_Disks"];
			entity.DatePurchased = (Convert.IsDBNull(dataRow["DatePurchased"]))?string.Empty:(System.String)dataRow["DatePurchased"];
			entity.UnderWarranty = (Convert.IsDBNull(dataRow["UnderWarranty"]))?string.Empty:(System.String)dataRow["UnderWarranty"];
			entity.Domain = (Convert.IsDBNull(dataRow["Domain"]))?string.Empty:(System.String)dataRow["Domain"];
			entity.CustomField1 = (Convert.IsDBNull(dataRow["CustomField1"]))?string.Empty:(System.String)dataRow["CustomField1"];
			entity.CustomField2 = (Convert.IsDBNull(dataRow["CustomField2"]))?string.Empty:(System.String)dataRow["CustomField2"];
			entity.CustomField3 = (Convert.IsDBNull(dataRow["CustomField3"]))?string.Empty:(System.String)dataRow["CustomField3"];
			entity.ServerType = (Convert.IsDBNull(dataRow["ServerType"]))?string.Empty:(System.String)dataRow["ServerType"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.AbbreviatedName = (Convert.IsDBNull(dataRow["AbbreviatedName"]))?string.Empty:(System.String)dataRow["AbbreviatedName"];
			entity.Abbreviation = (Convert.IsDBNull(dataRow["Abbreviation"]))?string.Empty:(System.String)dataRow["Abbreviation"];
			entity.IsBusinessExternal = (Convert.IsDBNull(dataRow["IsBusinessExternal"]))?string.Empty:(System.String)dataRow["IsBusinessExternal"];
			entity.StandardisationStatus = (Convert.IsDBNull(dataRow["StandardisationStatus"]))?string.Empty:(System.String)dataRow["StandardisationStatus"];
			entity.StandardisationStatusDate = (Convert.IsDBNull(dataRow["StandardisationStatusDate"]))?string.Empty:(System.String)dataRow["StandardisationStatusDate"];
			entity.DataSourceID = (Convert.IsDBNull(dataRow["DataSourceID"]))?string.Empty:(System.String)dataRow["DataSourceID"];
			entity.DataSourceName = (Convert.IsDBNull(dataRow["DataSourceName"]))?string.Empty:(System.String)dataRow["DataSourceName"];
			entity.GeneralRemarks = (Convert.IsDBNull(dataRow["GeneralRemarks"]))?string.Empty:(System.String)dataRow["GeneralRemarks"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAView_SystemComponent_RetrievalFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_SystemComponent_Retrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_SystemComponent_RetrievalFilterBuilder : SqlFilterBuilder<METAView_SystemComponent_RetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_SystemComponent_RetrievalFilterBuilder class.
		/// </summary>
		public METAView_SystemComponent_RetrievalFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_SystemComponent_RetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_SystemComponent_RetrievalFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_SystemComponent_RetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_SystemComponent_RetrievalFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_SystemComponent_RetrievalFilterBuilder

	#region METAView_SystemComponent_RetrievalParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_SystemComponent_Retrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_SystemComponent_RetrievalParameterBuilder : ParameterizedSqlFilterBuilder<METAView_SystemComponent_RetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_SystemComponent_RetrievalParameterBuilder class.
		/// </summary>
		public METAView_SystemComponent_RetrievalParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_SystemComponent_RetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_SystemComponent_RetrievalParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_SystemComponent_RetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_SystemComponent_RetrievalParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_SystemComponent_RetrievalParameterBuilder
	
	#region METAView_SystemComponent_RetrievalSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_SystemComponent_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_SystemComponent_RetrievalSortBuilder : SqlSortBuilder<METAView_SystemComponent_RetrievalColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_SystemComponent_RetrievalSqlSortBuilder class.
		/// </summary>
		public METAView_SystemComponent_RetrievalSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAView_SystemComponent_RetrievalSortBuilder

} // end namespace
