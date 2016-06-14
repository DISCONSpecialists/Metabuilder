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
	/// This class is the base class for any <see cref="METAViewSystemComponentRetrievalProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAViewSystemComponentRetrievalProviderBaseCore : EntityViewProviderBase<METAViewSystemComponentRetrieval>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAViewSystemComponentRetrieval&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAViewSystemComponentRetrieval&gt;"/></returns>
		protected static VList&lt;METAViewSystemComponentRetrieval&gt; Fill(DataSet dataSet, VList<METAViewSystemComponentRetrieval> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAViewSystemComponentRetrieval>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAViewSystemComponentRetrieval&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAViewSystemComponentRetrieval>"/></returns>
		protected static VList&lt;METAViewSystemComponentRetrieval&gt; Fill(DataTable dataTable, VList<METAViewSystemComponentRetrieval> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAViewSystemComponentRetrieval c = new METAViewSystemComponentRetrieval();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeID = (Convert.IsDBNull(row["WorkspaceTypeID"]))?(int)0:(System.Int32?)row["WorkspaceTypeID"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.Pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
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
					c.IsDNS = (Convert.IsDBNull(row["isDNS"]))?string.Empty:(System.String)row["isDNS"];
					c.IsDHCP = (Convert.IsDBNull(row["isDHCP"]))?string.Empty:(System.String)row["isDHCP"];
					c.Capacity = (Convert.IsDBNull(row["Capacity"]))?string.Empty:(System.String)row["Capacity"];
					c.MemTotal = (Convert.IsDBNull(row["Mem_Total"]))?string.Empty:(System.String)row["Mem_Total"];
					c.CPUType = (Convert.IsDBNull(row["CPU_Type"]))?string.Empty:(System.String)row["CPU_Type"];
					c.CPUSpeed = (Convert.IsDBNull(row["CPU_Speed"]))?string.Empty:(System.String)row["CPU_Speed"];
					c.Monitor = (Convert.IsDBNull(row["Monitor"]))?string.Empty:(System.String)row["Monitor"];
					c.VideoCard = (Convert.IsDBNull(row["Video_Card"]))?string.Empty:(System.String)row["Video_Card"];
					c.NumberOfDisks = (Convert.IsDBNull(row["Number_Of_Disks"]))?string.Empty:(System.String)row["Number_Of_Disks"];
					c.DatePurchased = (Convert.IsDBNull(row["DatePurchased"]))?string.Empty:(System.String)row["DatePurchased"];
					c.UnderWarranty = (Convert.IsDBNull(row["UnderWarranty"]))?string.Empty:(System.String)row["UnderWarranty"];
					c.Domain = (Convert.IsDBNull(row["Domain"]))?string.Empty:(System.String)row["Domain"];
					c.CustomField1 = (Convert.IsDBNull(row["CustomField1"]))?string.Empty:(System.String)row["CustomField1"];
					c.CustomField2 = (Convert.IsDBNull(row["CustomField2"]))?string.Empty:(System.String)row["CustomField2"];
					c.CustomField3 = (Convert.IsDBNull(row["CustomField3"]))?string.Empty:(System.String)row["CustomField3"];
					c.ServerType = (Convert.IsDBNull(row["ServerType"]))?string.Empty:(System.String)row["ServerType"];
					c.GapType = (Convert.IsDBNull(row["GapType"]))?string.Empty:(System.String)row["GapType"];
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
		/// Fill an <see cref="VList&lt;METAViewSystemComponentRetrieval&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAViewSystemComponentRetrieval&gt;"/></returns>
		protected VList<METAViewSystemComponentRetrieval> Fill(IDataReader reader, VList<METAViewSystemComponentRetrieval> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAViewSystemComponentRetrieval entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAViewSystemComponentRetrieval>("METAViewSystemComponentRetrieval",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAViewSystemComponentRetrieval();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAViewSystemComponentRetrievalColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewSystemComponentRetrievalColumn.WorkspaceTypeID)];
					//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
					entity.VCStatusID = (System.Int32)reader[((int)METAViewSystemComponentRetrievalColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.Pkid = (System.Int32)reader[((int)METAViewSystemComponentRetrievalColumn.Pkid)];
					//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAViewSystemComponentRetrievalColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Type = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.Type)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.Type)];
					//entity.Type = (Convert.IsDBNull(reader["Type"]))?string.Empty:(System.String)reader["Type"];
					entity.Name = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.Name)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.Name)];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.Description = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.Description)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.Description)];
					//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
					entity.SeverityRating = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.SeverityRating)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.SeverityRating)];
					//entity.SeverityRating = (Convert.IsDBNull(reader["SeverityRating"]))?string.Empty:(System.String)reader["SeverityRating"];
					entity.Configuration = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.Configuration)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.Configuration)];
					//entity.Configuration = (Convert.IsDBNull(reader["Configuration"]))?string.Empty:(System.String)reader["Configuration"];
					entity.MACAddress = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.MACAddress)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.MACAddress)];
					//entity.MACAddress = (Convert.IsDBNull(reader["MACAddress"]))?string.Empty:(System.String)reader["MACAddress"];
					entity.StaticIP = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.StaticIP)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.StaticIP)];
					//entity.StaticIP = (Convert.IsDBNull(reader["StaticIP"]))?string.Empty:(System.String)reader["StaticIP"];
					entity.NetworkAddress3 = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.NetworkAddress3)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.NetworkAddress3)];
					//entity.NetworkAddress3 = (Convert.IsDBNull(reader["NetworkAddress3"]))?string.Empty:(System.String)reader["NetworkAddress3"];
					entity.NetworkAddress4 = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.NetworkAddress4)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.NetworkAddress4)];
					//entity.NetworkAddress4 = (Convert.IsDBNull(reader["NetworkAddress4"]))?string.Empty:(System.String)reader["NetworkAddress4"];
					entity.NetworkAddress5 = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.NetworkAddress5)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.NetworkAddress5)];
					//entity.NetworkAddress5 = (Convert.IsDBNull(reader["NetworkAddress5"]))?string.Empty:(System.String)reader["NetworkAddress5"];
					entity.Make = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.Make)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.Make)];
					//entity.Make = (Convert.IsDBNull(reader["Make"]))?string.Empty:(System.String)reader["Make"];
					entity.Model = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.Model)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.Model)];
					//entity.Model = (Convert.IsDBNull(reader["Model"]))?string.Empty:(System.String)reader["Model"];
					entity.SerialNumber = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.SerialNumber)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.SerialNumber)];
					//entity.SerialNumber = (Convert.IsDBNull(reader["SerialNumber"]))?string.Empty:(System.String)reader["SerialNumber"];
					entity.AssetNumber = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.AssetNumber)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.AssetNumber)];
					//entity.AssetNumber = (Convert.IsDBNull(reader["AssetNumber"]))?string.Empty:(System.String)reader["AssetNumber"];
					entity.IsDNS = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.IsDNS)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.IsDNS)];
					//entity.IsDNS = (Convert.IsDBNull(reader["isDNS"]))?string.Empty:(System.String)reader["isDNS"];
					entity.IsDHCP = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.IsDHCP)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.IsDHCP)];
					//entity.IsDHCP = (Convert.IsDBNull(reader["isDHCP"]))?string.Empty:(System.String)reader["isDHCP"];
					entity.Capacity = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.Capacity)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.Capacity)];
					//entity.Capacity = (Convert.IsDBNull(reader["Capacity"]))?string.Empty:(System.String)reader["Capacity"];
					entity.MemTotal = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.MemTotal)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.MemTotal)];
					//entity.MemTotal = (Convert.IsDBNull(reader["Mem_Total"]))?string.Empty:(System.String)reader["Mem_Total"];
					entity.CPUType = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.CPUType)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.CPUType)];
					//entity.CPUType = (Convert.IsDBNull(reader["CPU_Type"]))?string.Empty:(System.String)reader["CPU_Type"];
					entity.CPUSpeed = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.CPUSpeed)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.CPUSpeed)];
					//entity.CPUSpeed = (Convert.IsDBNull(reader["CPU_Speed"]))?string.Empty:(System.String)reader["CPU_Speed"];
					entity.Monitor = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.Monitor)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.Monitor)];
					//entity.Monitor = (Convert.IsDBNull(reader["Monitor"]))?string.Empty:(System.String)reader["Monitor"];
					entity.VideoCard = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.VideoCard)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.VideoCard)];
					//entity.VideoCard = (Convert.IsDBNull(reader["Video_Card"]))?string.Empty:(System.String)reader["Video_Card"];
					entity.NumberOfDisks = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.NumberOfDisks)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.NumberOfDisks)];
					//entity.NumberOfDisks = (Convert.IsDBNull(reader["Number_Of_Disks"]))?string.Empty:(System.String)reader["Number_Of_Disks"];
					entity.DatePurchased = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.DatePurchased)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.DatePurchased)];
					//entity.DatePurchased = (Convert.IsDBNull(reader["DatePurchased"]))?string.Empty:(System.String)reader["DatePurchased"];
					entity.UnderWarranty = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.UnderWarranty)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.UnderWarranty)];
					//entity.UnderWarranty = (Convert.IsDBNull(reader["UnderWarranty"]))?string.Empty:(System.String)reader["UnderWarranty"];
					entity.Domain = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.Domain)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.Domain)];
					//entity.Domain = (Convert.IsDBNull(reader["Domain"]))?string.Empty:(System.String)reader["Domain"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.ServerType = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.ServerType)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.ServerType)];
					//entity.ServerType = (Convert.IsDBNull(reader["ServerType"]))?string.Empty:(System.String)reader["ServerType"];
					entity.GapType = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.GapType)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.GapType)];
					//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
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
		/// Refreshes the <see cref="METAViewSystemComponentRetrieval"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewSystemComponentRetrieval"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAViewSystemComponentRetrieval entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAViewSystemComponentRetrievalColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewSystemComponentRetrievalColumn.WorkspaceTypeID)];
			//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
			entity.VCStatusID = (System.Int32)reader[((int)METAViewSystemComponentRetrievalColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.Pkid = (System.Int32)reader[((int)METAViewSystemComponentRetrievalColumn.Pkid)];
			//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAViewSystemComponentRetrievalColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Type = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.Type)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.Type)];
			//entity.Type = (Convert.IsDBNull(reader["Type"]))?string.Empty:(System.String)reader["Type"];
			entity.Name = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.Name)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.Name)];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.Description = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.Description)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.Description)];
			//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
			entity.SeverityRating = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.SeverityRating)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.SeverityRating)];
			//entity.SeverityRating = (Convert.IsDBNull(reader["SeverityRating"]))?string.Empty:(System.String)reader["SeverityRating"];
			entity.Configuration = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.Configuration)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.Configuration)];
			//entity.Configuration = (Convert.IsDBNull(reader["Configuration"]))?string.Empty:(System.String)reader["Configuration"];
			entity.MACAddress = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.MACAddress)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.MACAddress)];
			//entity.MACAddress = (Convert.IsDBNull(reader["MACAddress"]))?string.Empty:(System.String)reader["MACAddress"];
			entity.StaticIP = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.StaticIP)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.StaticIP)];
			//entity.StaticIP = (Convert.IsDBNull(reader["StaticIP"]))?string.Empty:(System.String)reader["StaticIP"];
			entity.NetworkAddress3 = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.NetworkAddress3)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.NetworkAddress3)];
			//entity.NetworkAddress3 = (Convert.IsDBNull(reader["NetworkAddress3"]))?string.Empty:(System.String)reader["NetworkAddress3"];
			entity.NetworkAddress4 = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.NetworkAddress4)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.NetworkAddress4)];
			//entity.NetworkAddress4 = (Convert.IsDBNull(reader["NetworkAddress4"]))?string.Empty:(System.String)reader["NetworkAddress4"];
			entity.NetworkAddress5 = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.NetworkAddress5)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.NetworkAddress5)];
			//entity.NetworkAddress5 = (Convert.IsDBNull(reader["NetworkAddress5"]))?string.Empty:(System.String)reader["NetworkAddress5"];
			entity.Make = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.Make)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.Make)];
			//entity.Make = (Convert.IsDBNull(reader["Make"]))?string.Empty:(System.String)reader["Make"];
			entity.Model = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.Model)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.Model)];
			//entity.Model = (Convert.IsDBNull(reader["Model"]))?string.Empty:(System.String)reader["Model"];
			entity.SerialNumber = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.SerialNumber)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.SerialNumber)];
			//entity.SerialNumber = (Convert.IsDBNull(reader["SerialNumber"]))?string.Empty:(System.String)reader["SerialNumber"];
			entity.AssetNumber = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.AssetNumber)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.AssetNumber)];
			//entity.AssetNumber = (Convert.IsDBNull(reader["AssetNumber"]))?string.Empty:(System.String)reader["AssetNumber"];
			entity.IsDNS = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.IsDNS)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.IsDNS)];
			//entity.IsDNS = (Convert.IsDBNull(reader["isDNS"]))?string.Empty:(System.String)reader["isDNS"];
			entity.IsDHCP = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.IsDHCP)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.IsDHCP)];
			//entity.IsDHCP = (Convert.IsDBNull(reader["isDHCP"]))?string.Empty:(System.String)reader["isDHCP"];
			entity.Capacity = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.Capacity)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.Capacity)];
			//entity.Capacity = (Convert.IsDBNull(reader["Capacity"]))?string.Empty:(System.String)reader["Capacity"];
			entity.MemTotal = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.MemTotal)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.MemTotal)];
			//entity.MemTotal = (Convert.IsDBNull(reader["Mem_Total"]))?string.Empty:(System.String)reader["Mem_Total"];
			entity.CPUType = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.CPUType)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.CPUType)];
			//entity.CPUType = (Convert.IsDBNull(reader["CPU_Type"]))?string.Empty:(System.String)reader["CPU_Type"];
			entity.CPUSpeed = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.CPUSpeed)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.CPUSpeed)];
			//entity.CPUSpeed = (Convert.IsDBNull(reader["CPU_Speed"]))?string.Empty:(System.String)reader["CPU_Speed"];
			entity.Monitor = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.Monitor)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.Monitor)];
			//entity.Monitor = (Convert.IsDBNull(reader["Monitor"]))?string.Empty:(System.String)reader["Monitor"];
			entity.VideoCard = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.VideoCard)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.VideoCard)];
			//entity.VideoCard = (Convert.IsDBNull(reader["Video_Card"]))?string.Empty:(System.String)reader["Video_Card"];
			entity.NumberOfDisks = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.NumberOfDisks)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.NumberOfDisks)];
			//entity.NumberOfDisks = (Convert.IsDBNull(reader["Number_Of_Disks"]))?string.Empty:(System.String)reader["Number_Of_Disks"];
			entity.DatePurchased = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.DatePurchased)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.DatePurchased)];
			//entity.DatePurchased = (Convert.IsDBNull(reader["DatePurchased"]))?string.Empty:(System.String)reader["DatePurchased"];
			entity.UnderWarranty = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.UnderWarranty)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.UnderWarranty)];
			//entity.UnderWarranty = (Convert.IsDBNull(reader["UnderWarranty"]))?string.Empty:(System.String)reader["UnderWarranty"];
			entity.Domain = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.Domain)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.Domain)];
			//entity.Domain = (Convert.IsDBNull(reader["Domain"]))?string.Empty:(System.String)reader["Domain"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.ServerType = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.ServerType)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.ServerType)];
			//entity.ServerType = (Convert.IsDBNull(reader["ServerType"]))?string.Empty:(System.String)reader["ServerType"];
			entity.GapType = (reader.IsDBNull(((int)METAViewSystemComponentRetrievalColumn.GapType)))?null:(System.String)reader[((int)METAViewSystemComponentRetrievalColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAViewSystemComponentRetrieval"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewSystemComponentRetrieval"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAViewSystemComponentRetrieval entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeID = (Convert.IsDBNull(dataRow["WorkspaceTypeID"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeID"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.Pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
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
			entity.IsDNS = (Convert.IsDBNull(dataRow["isDNS"]))?string.Empty:(System.String)dataRow["isDNS"];
			entity.IsDHCP = (Convert.IsDBNull(dataRow["isDHCP"]))?string.Empty:(System.String)dataRow["isDHCP"];
			entity.Capacity = (Convert.IsDBNull(dataRow["Capacity"]))?string.Empty:(System.String)dataRow["Capacity"];
			entity.MemTotal = (Convert.IsDBNull(dataRow["Mem_Total"]))?string.Empty:(System.String)dataRow["Mem_Total"];
			entity.CPUType = (Convert.IsDBNull(dataRow["CPU_Type"]))?string.Empty:(System.String)dataRow["CPU_Type"];
			entity.CPUSpeed = (Convert.IsDBNull(dataRow["CPU_Speed"]))?string.Empty:(System.String)dataRow["CPU_Speed"];
			entity.Monitor = (Convert.IsDBNull(dataRow["Monitor"]))?string.Empty:(System.String)dataRow["Monitor"];
			entity.VideoCard = (Convert.IsDBNull(dataRow["Video_Card"]))?string.Empty:(System.String)dataRow["Video_Card"];
			entity.NumberOfDisks = (Convert.IsDBNull(dataRow["Number_Of_Disks"]))?string.Empty:(System.String)dataRow["Number_Of_Disks"];
			entity.DatePurchased = (Convert.IsDBNull(dataRow["DatePurchased"]))?string.Empty:(System.String)dataRow["DatePurchased"];
			entity.UnderWarranty = (Convert.IsDBNull(dataRow["UnderWarranty"]))?string.Empty:(System.String)dataRow["UnderWarranty"];
			entity.Domain = (Convert.IsDBNull(dataRow["Domain"]))?string.Empty:(System.String)dataRow["Domain"];
			entity.CustomField1 = (Convert.IsDBNull(dataRow["CustomField1"]))?string.Empty:(System.String)dataRow["CustomField1"];
			entity.CustomField2 = (Convert.IsDBNull(dataRow["CustomField2"]))?string.Empty:(System.String)dataRow["CustomField2"];
			entity.CustomField3 = (Convert.IsDBNull(dataRow["CustomField3"]))?string.Empty:(System.String)dataRow["CustomField3"];
			entity.ServerType = (Convert.IsDBNull(dataRow["ServerType"]))?string.Empty:(System.String)dataRow["ServerType"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAViewSystemComponentRetrievalFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewSystemComponentRetrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewSystemComponentRetrievalFilterBuilder : SqlFilterBuilder<METAViewSystemComponentRetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewSystemComponentRetrievalFilterBuilder class.
		/// </summary>
		public METAViewSystemComponentRetrievalFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewSystemComponentRetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewSystemComponentRetrievalFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewSystemComponentRetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewSystemComponentRetrievalFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewSystemComponentRetrievalFilterBuilder

	#region METAViewSystemComponentRetrievalParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewSystemComponentRetrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewSystemComponentRetrievalParameterBuilder : ParameterizedSqlFilterBuilder<METAViewSystemComponentRetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewSystemComponentRetrievalParameterBuilder class.
		/// </summary>
		public METAViewSystemComponentRetrievalParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewSystemComponentRetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewSystemComponentRetrievalParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewSystemComponentRetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewSystemComponentRetrievalParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewSystemComponentRetrievalParameterBuilder
	
	#region METAViewSystemComponentRetrievalSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewSystemComponentRetrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAViewSystemComponentRetrievalSortBuilder : SqlSortBuilder<METAViewSystemComponentRetrievalColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewSystemComponentRetrievalSqlSortBuilder class.
		/// </summary>
		public METAViewSystemComponentRetrievalSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAViewSystemComponentRetrievalSortBuilder

} // end namespace
