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
	/// This class is the base class for any <see cref="METAViewSystemComponentListingProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAViewSystemComponentListingProviderBaseCore : EntityViewProviderBase<METAViewSystemComponentListing>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAViewSystemComponentListing&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAViewSystemComponentListing&gt;"/></returns>
		protected static VList&lt;METAViewSystemComponentListing&gt; Fill(DataSet dataSet, VList<METAViewSystemComponentListing> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAViewSystemComponentListing>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAViewSystemComponentListing&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAViewSystemComponentListing>"/></returns>
		protected static VList&lt;METAViewSystemComponentListing&gt; Fill(DataTable dataTable, VList<METAViewSystemComponentListing> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAViewSystemComponentListing c = new METAViewSystemComponentListing();
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
		/// Fill an <see cref="VList&lt;METAViewSystemComponentListing&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAViewSystemComponentListing&gt;"/></returns>
		protected VList<METAViewSystemComponentListing> Fill(IDataReader reader, VList<METAViewSystemComponentListing> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAViewSystemComponentListing entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAViewSystemComponentListing>("METAViewSystemComponentListing",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAViewSystemComponentListing();
					}
					entity.WorkspaceName = (System.String)reader["WorkspaceName"];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeID = (reader.IsDBNull(reader.GetOrdinal("WorkspaceTypeID")))?null:(System.Int32?)reader["WorkspaceTypeID"];
					//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
					entity.VCStatusID = (System.Int32)reader["VCStatusID"];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.Pkid = (System.Int32)reader["pkid"];
					//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader["Machine"];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(reader.GetOrdinal("VCMachineID")))?null:(System.String)reader["VCMachineID"];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Type = (reader.IsDBNull(reader.GetOrdinal("Type")))?null:(System.String)reader["Type"];
					//entity.Type = (Convert.IsDBNull(reader["Type"]))?string.Empty:(System.String)reader["Type"];
					entity.Name = (reader.IsDBNull(reader.GetOrdinal("Name")))?null:(System.String)reader["Name"];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.Description = (reader.IsDBNull(reader.GetOrdinal("Description")))?null:(System.String)reader["Description"];
					//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
					entity.SeverityRating = (reader.IsDBNull(reader.GetOrdinal("SeverityRating")))?null:(System.String)reader["SeverityRating"];
					//entity.SeverityRating = (Convert.IsDBNull(reader["SeverityRating"]))?string.Empty:(System.String)reader["SeverityRating"];
					entity.Configuration = (reader.IsDBNull(reader.GetOrdinal("Configuration")))?null:(System.String)reader["Configuration"];
					//entity.Configuration = (Convert.IsDBNull(reader["Configuration"]))?string.Empty:(System.String)reader["Configuration"];
					entity.MACAddress = (reader.IsDBNull(reader.GetOrdinal("MACAddress")))?null:(System.String)reader["MACAddress"];
					//entity.MACAddress = (Convert.IsDBNull(reader["MACAddress"]))?string.Empty:(System.String)reader["MACAddress"];
					entity.StaticIP = (reader.IsDBNull(reader.GetOrdinal("StaticIP")))?null:(System.String)reader["StaticIP"];
					//entity.StaticIP = (Convert.IsDBNull(reader["StaticIP"]))?string.Empty:(System.String)reader["StaticIP"];
					entity.NetworkAddress3 = (reader.IsDBNull(reader.GetOrdinal("NetworkAddress3")))?null:(System.String)reader["NetworkAddress3"];
					//entity.NetworkAddress3 = (Convert.IsDBNull(reader["NetworkAddress3"]))?string.Empty:(System.String)reader["NetworkAddress3"];
					entity.NetworkAddress4 = (reader.IsDBNull(reader.GetOrdinal("NetworkAddress4")))?null:(System.String)reader["NetworkAddress4"];
					//entity.NetworkAddress4 = (Convert.IsDBNull(reader["NetworkAddress4"]))?string.Empty:(System.String)reader["NetworkAddress4"];
					entity.NetworkAddress5 = (reader.IsDBNull(reader.GetOrdinal("NetworkAddress5")))?null:(System.String)reader["NetworkAddress5"];
					//entity.NetworkAddress5 = (Convert.IsDBNull(reader["NetworkAddress5"]))?string.Empty:(System.String)reader["NetworkAddress5"];
					entity.Make = (reader.IsDBNull(reader.GetOrdinal("Make")))?null:(System.String)reader["Make"];
					//entity.Make = (Convert.IsDBNull(reader["Make"]))?string.Empty:(System.String)reader["Make"];
					entity.Model = (reader.IsDBNull(reader.GetOrdinal("Model")))?null:(System.String)reader["Model"];
					//entity.Model = (Convert.IsDBNull(reader["Model"]))?string.Empty:(System.String)reader["Model"];
					entity.SerialNumber = (reader.IsDBNull(reader.GetOrdinal("SerialNumber")))?null:(System.String)reader["SerialNumber"];
					//entity.SerialNumber = (Convert.IsDBNull(reader["SerialNumber"]))?string.Empty:(System.String)reader["SerialNumber"];
					entity.AssetNumber = (reader.IsDBNull(reader.GetOrdinal("AssetNumber")))?null:(System.String)reader["AssetNumber"];
					//entity.AssetNumber = (Convert.IsDBNull(reader["AssetNumber"]))?string.Empty:(System.String)reader["AssetNumber"];
					entity.IsDNS = (reader.IsDBNull(reader.GetOrdinal("isDNS")))?null:(System.String)reader["isDNS"];
					//entity.IsDNS = (Convert.IsDBNull(reader["isDNS"]))?string.Empty:(System.String)reader["isDNS"];
					entity.IsDHCP = (reader.IsDBNull(reader.GetOrdinal("isDHCP")))?null:(System.String)reader["isDHCP"];
					//entity.IsDHCP = (Convert.IsDBNull(reader["isDHCP"]))?string.Empty:(System.String)reader["isDHCP"];
					entity.Capacity = (reader.IsDBNull(reader.GetOrdinal("Capacity")))?null:(System.String)reader["Capacity"];
					//entity.Capacity = (Convert.IsDBNull(reader["Capacity"]))?string.Empty:(System.String)reader["Capacity"];
					entity.MemTotal = (reader.IsDBNull(reader.GetOrdinal("Mem_Total")))?null:(System.String)reader["Mem_Total"];
					//entity.MemTotal = (Convert.IsDBNull(reader["Mem_Total"]))?string.Empty:(System.String)reader["Mem_Total"];
					entity.CPUType = (reader.IsDBNull(reader.GetOrdinal("CPU_Type")))?null:(System.String)reader["CPU_Type"];
					//entity.CPUType = (Convert.IsDBNull(reader["CPU_Type"]))?string.Empty:(System.String)reader["CPU_Type"];
					entity.CPUSpeed = (reader.IsDBNull(reader.GetOrdinal("CPU_Speed")))?null:(System.String)reader["CPU_Speed"];
					//entity.CPUSpeed = (Convert.IsDBNull(reader["CPU_Speed"]))?string.Empty:(System.String)reader["CPU_Speed"];
					entity.Monitor = (reader.IsDBNull(reader.GetOrdinal("Monitor")))?null:(System.String)reader["Monitor"];
					//entity.Monitor = (Convert.IsDBNull(reader["Monitor"]))?string.Empty:(System.String)reader["Monitor"];
					entity.VideoCard = (reader.IsDBNull(reader.GetOrdinal("Video_Card")))?null:(System.String)reader["Video_Card"];
					//entity.VideoCard = (Convert.IsDBNull(reader["Video_Card"]))?string.Empty:(System.String)reader["Video_Card"];
					entity.NumberOfDisks = (reader.IsDBNull(reader.GetOrdinal("Number_Of_Disks")))?null:(System.String)reader["Number_Of_Disks"];
					//entity.NumberOfDisks = (Convert.IsDBNull(reader["Number_Of_Disks"]))?string.Empty:(System.String)reader["Number_Of_Disks"];
					entity.DatePurchased = (reader.IsDBNull(reader.GetOrdinal("DatePurchased")))?null:(System.String)reader["DatePurchased"];
					//entity.DatePurchased = (Convert.IsDBNull(reader["DatePurchased"]))?string.Empty:(System.String)reader["DatePurchased"];
					entity.UnderWarranty = (reader.IsDBNull(reader.GetOrdinal("UnderWarranty")))?null:(System.String)reader["UnderWarranty"];
					//entity.UnderWarranty = (Convert.IsDBNull(reader["UnderWarranty"]))?string.Empty:(System.String)reader["UnderWarranty"];
					entity.Domain = (reader.IsDBNull(reader.GetOrdinal("Domain")))?null:(System.String)reader["Domain"];
					//entity.Domain = (Convert.IsDBNull(reader["Domain"]))?string.Empty:(System.String)reader["Domain"];
					entity.CustomField1 = (reader.IsDBNull(reader.GetOrdinal("CustomField1")))?null:(System.String)reader["CustomField1"];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(reader.GetOrdinal("CustomField2")))?null:(System.String)reader["CustomField2"];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(reader.GetOrdinal("CustomField3")))?null:(System.String)reader["CustomField3"];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.AcceptChanges();
					rows.Add(entity);
					pageLength -= 1;
				}
				recordnum += 1;
			}
			return rows;
		}
		
		
		/// <summary>
		/// Refreshes the <see cref="METAViewSystemComponentListing"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewSystemComponentListing"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAViewSystemComponentListing entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader["WorkspaceName"];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeID = (reader.IsDBNull(reader.GetOrdinal("WorkspaceTypeID")))?null:(System.Int32?)reader["WorkspaceTypeID"];
			//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
			entity.VCStatusID = (System.Int32)reader["VCStatusID"];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.Pkid = (System.Int32)reader["pkid"];
			//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader["Machine"];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(reader.GetOrdinal("VCMachineID")))?null:(System.String)reader["VCMachineID"];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Type = (reader.IsDBNull(reader.GetOrdinal("Type")))?null:(System.String)reader["Type"];
			//entity.Type = (Convert.IsDBNull(reader["Type"]))?string.Empty:(System.String)reader["Type"];
			entity.Name = (reader.IsDBNull(reader.GetOrdinal("Name")))?null:(System.String)reader["Name"];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.Description = (reader.IsDBNull(reader.GetOrdinal("Description")))?null:(System.String)reader["Description"];
			//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
			entity.SeverityRating = (reader.IsDBNull(reader.GetOrdinal("SeverityRating")))?null:(System.String)reader["SeverityRating"];
			//entity.SeverityRating = (Convert.IsDBNull(reader["SeverityRating"]))?string.Empty:(System.String)reader["SeverityRating"];
			entity.Configuration = (reader.IsDBNull(reader.GetOrdinal("Configuration")))?null:(System.String)reader["Configuration"];
			//entity.Configuration = (Convert.IsDBNull(reader["Configuration"]))?string.Empty:(System.String)reader["Configuration"];
			entity.MACAddress = (reader.IsDBNull(reader.GetOrdinal("MACAddress")))?null:(System.String)reader["MACAddress"];
			//entity.MACAddress = (Convert.IsDBNull(reader["MACAddress"]))?string.Empty:(System.String)reader["MACAddress"];
			entity.StaticIP = (reader.IsDBNull(reader.GetOrdinal("StaticIP")))?null:(System.String)reader["StaticIP"];
			//entity.StaticIP = (Convert.IsDBNull(reader["StaticIP"]))?string.Empty:(System.String)reader["StaticIP"];
			entity.NetworkAddress3 = (reader.IsDBNull(reader.GetOrdinal("NetworkAddress3")))?null:(System.String)reader["NetworkAddress3"];
			//entity.NetworkAddress3 = (Convert.IsDBNull(reader["NetworkAddress3"]))?string.Empty:(System.String)reader["NetworkAddress3"];
			entity.NetworkAddress4 = (reader.IsDBNull(reader.GetOrdinal("NetworkAddress4")))?null:(System.String)reader["NetworkAddress4"];
			//entity.NetworkAddress4 = (Convert.IsDBNull(reader["NetworkAddress4"]))?string.Empty:(System.String)reader["NetworkAddress4"];
			entity.NetworkAddress5 = (reader.IsDBNull(reader.GetOrdinal("NetworkAddress5")))?null:(System.String)reader["NetworkAddress5"];
			//entity.NetworkAddress5 = (Convert.IsDBNull(reader["NetworkAddress5"]))?string.Empty:(System.String)reader["NetworkAddress5"];
			entity.Make = (reader.IsDBNull(reader.GetOrdinal("Make")))?null:(System.String)reader["Make"];
			//entity.Make = (Convert.IsDBNull(reader["Make"]))?string.Empty:(System.String)reader["Make"];
			entity.Model = (reader.IsDBNull(reader.GetOrdinal("Model")))?null:(System.String)reader["Model"];
			//entity.Model = (Convert.IsDBNull(reader["Model"]))?string.Empty:(System.String)reader["Model"];
			entity.SerialNumber = (reader.IsDBNull(reader.GetOrdinal("SerialNumber")))?null:(System.String)reader["SerialNumber"];
			//entity.SerialNumber = (Convert.IsDBNull(reader["SerialNumber"]))?string.Empty:(System.String)reader["SerialNumber"];
			entity.AssetNumber = (reader.IsDBNull(reader.GetOrdinal("AssetNumber")))?null:(System.String)reader["AssetNumber"];
			//entity.AssetNumber = (Convert.IsDBNull(reader["AssetNumber"]))?string.Empty:(System.String)reader["AssetNumber"];
			entity.IsDNS = (reader.IsDBNull(reader.GetOrdinal("isDNS")))?null:(System.String)reader["isDNS"];
			//entity.IsDNS = (Convert.IsDBNull(reader["isDNS"]))?string.Empty:(System.String)reader["isDNS"];
			entity.IsDHCP = (reader.IsDBNull(reader.GetOrdinal("isDHCP")))?null:(System.String)reader["isDHCP"];
			//entity.IsDHCP = (Convert.IsDBNull(reader["isDHCP"]))?string.Empty:(System.String)reader["isDHCP"];
			entity.Capacity = (reader.IsDBNull(reader.GetOrdinal("Capacity")))?null:(System.String)reader["Capacity"];
			//entity.Capacity = (Convert.IsDBNull(reader["Capacity"]))?string.Empty:(System.String)reader["Capacity"];
			entity.MemTotal = (reader.IsDBNull(reader.GetOrdinal("Mem_Total")))?null:(System.String)reader["Mem_Total"];
			//entity.MemTotal = (Convert.IsDBNull(reader["Mem_Total"]))?string.Empty:(System.String)reader["Mem_Total"];
			entity.CPUType = (reader.IsDBNull(reader.GetOrdinal("CPU_Type")))?null:(System.String)reader["CPU_Type"];
			//entity.CPUType = (Convert.IsDBNull(reader["CPU_Type"]))?string.Empty:(System.String)reader["CPU_Type"];
			entity.CPUSpeed = (reader.IsDBNull(reader.GetOrdinal("CPU_Speed")))?null:(System.String)reader["CPU_Speed"];
			//entity.CPUSpeed = (Convert.IsDBNull(reader["CPU_Speed"]))?string.Empty:(System.String)reader["CPU_Speed"];
			entity.Monitor = (reader.IsDBNull(reader.GetOrdinal("Monitor")))?null:(System.String)reader["Monitor"];
			//entity.Monitor = (Convert.IsDBNull(reader["Monitor"]))?string.Empty:(System.String)reader["Monitor"];
			entity.VideoCard = (reader.IsDBNull(reader.GetOrdinal("Video_Card")))?null:(System.String)reader["Video_Card"];
			//entity.VideoCard = (Convert.IsDBNull(reader["Video_Card"]))?string.Empty:(System.String)reader["Video_Card"];
			entity.NumberOfDisks = (reader.IsDBNull(reader.GetOrdinal("Number_Of_Disks")))?null:(System.String)reader["Number_Of_Disks"];
			//entity.NumberOfDisks = (Convert.IsDBNull(reader["Number_Of_Disks"]))?string.Empty:(System.String)reader["Number_Of_Disks"];
			entity.DatePurchased = (reader.IsDBNull(reader.GetOrdinal("DatePurchased")))?null:(System.String)reader["DatePurchased"];
			//entity.DatePurchased = (Convert.IsDBNull(reader["DatePurchased"]))?string.Empty:(System.String)reader["DatePurchased"];
			entity.UnderWarranty = (reader.IsDBNull(reader.GetOrdinal("UnderWarranty")))?null:(System.String)reader["UnderWarranty"];
			//entity.UnderWarranty = (Convert.IsDBNull(reader["UnderWarranty"]))?string.Empty:(System.String)reader["UnderWarranty"];
			entity.Domain = (reader.IsDBNull(reader.GetOrdinal("Domain")))?null:(System.String)reader["Domain"];
			//entity.Domain = (Convert.IsDBNull(reader["Domain"]))?string.Empty:(System.String)reader["Domain"];
			entity.CustomField1 = (reader.IsDBNull(reader.GetOrdinal("CustomField1")))?null:(System.String)reader["CustomField1"];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(reader.GetOrdinal("CustomField2")))?null:(System.String)reader["CustomField2"];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(reader.GetOrdinal("CustomField3")))?null:(System.String)reader["CustomField3"];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAViewSystemComponentListing"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewSystemComponentListing"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAViewSystemComponentListing entity)
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
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAViewSystemComponentListingFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewSystemComponentListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewSystemComponentListingFilterBuilder : SqlFilterBuilder<METAViewSystemComponentListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewSystemComponentListingFilterBuilder class.
		/// </summary>
		public METAViewSystemComponentListingFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewSystemComponentListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewSystemComponentListingFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewSystemComponentListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewSystemComponentListingFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewSystemComponentListingFilterBuilder

	#region METAViewSystemComponentListingParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewSystemComponentListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewSystemComponentListingParameterBuilder : ParameterizedSqlFilterBuilder<METAViewSystemComponentListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewSystemComponentListingParameterBuilder class.
		/// </summary>
		public METAViewSystemComponentListingParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewSystemComponentListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewSystemComponentListingParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewSystemComponentListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewSystemComponentListingParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewSystemComponentListingParameterBuilder
} // end namespace
