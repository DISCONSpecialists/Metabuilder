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
	/// This class is the base class for any <see cref="METAViewNetworkComponentListingProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAViewNetworkComponentListingProviderBaseCore : EntityViewProviderBase<METAViewNetworkComponentListing>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAViewNetworkComponentListing&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAViewNetworkComponentListing&gt;"/></returns>
		protected static VList&lt;METAViewNetworkComponentListing&gt; Fill(DataSet dataSet, VList<METAViewNetworkComponentListing> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAViewNetworkComponentListing>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAViewNetworkComponentListing&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAViewNetworkComponentListing>"/></returns>
		protected static VList&lt;METAViewNetworkComponentListing&gt; Fill(DataTable dataTable, VList<METAViewNetworkComponentListing> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAViewNetworkComponentListing c = new METAViewNetworkComponentListing();
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
					c.MacAddress = (Convert.IsDBNull(row["MacAddress"]))?string.Empty:(System.String)row["MacAddress"];
					c.NetworkAddress2 = (Convert.IsDBNull(row["NetworkAddress2"]))?string.Empty:(System.String)row["NetworkAddress2"];
					c.NetworkAddress3 = (Convert.IsDBNull(row["NetworkAddress3"]))?string.Empty:(System.String)row["NetworkAddress3"];
					c.NetworkAddress4 = (Convert.IsDBNull(row["NetworkAddress4"]))?string.Empty:(System.String)row["NetworkAddress4"];
					c.NetworkAddress5 = (Convert.IsDBNull(row["NetworkAddress5"]))?string.Empty:(System.String)row["NetworkAddress5"];
					c.Make = (Convert.IsDBNull(row["Make"]))?string.Empty:(System.String)row["Make"];
					c.Model = (Convert.IsDBNull(row["Model"]))?string.Empty:(System.String)row["Model"];
					c.SerialNumber = (Convert.IsDBNull(row["SerialNumber"]))?string.Empty:(System.String)row["SerialNumber"];
					c.AssetNumber = (Convert.IsDBNull(row["AssetNumber"]))?string.Empty:(System.String)row["AssetNumber"];
					c.ConnectionSpeed = (Convert.IsDBNull(row["ConnectionSpeed"]))?string.Empty:(System.String)row["ConnectionSpeed"];
					c.NumberOfPorts = (Convert.IsDBNull(row["Number_of_Ports"]))?string.Empty:(System.String)row["Number_of_Ports"];
					c.NumberOfPortsAvailable = (Convert.IsDBNull(row["Number_of_Ports_Available"]))?string.Empty:(System.String)row["Number_of_Ports_Available"];
					c.Range = (Convert.IsDBNull(row["Range"]))?string.Empty:(System.String)row["Range"];
					c.IsDNS = (Convert.IsDBNull(row["isDNS"]))?string.Empty:(System.String)row["isDNS"];
					c.IsDHCP = (Convert.IsDBNull(row["isDHCP"]))?string.Empty:(System.String)row["isDHCP"];
					c.IsManaged = (Convert.IsDBNull(row["isManaged"]))?string.Empty:(System.String)row["isManaged"];
					c.MemTotal = (Convert.IsDBNull(row["Mem_Total"]))?string.Empty:(System.String)row["Mem_Total"];
					c.DatePurchased = (Convert.IsDBNull(row["DatePurchased"]))?string.Empty:(System.String)row["DatePurchased"];
					c.UnderWarranty = (Convert.IsDBNull(row["UnderWarranty"]))?string.Empty:(System.String)row["UnderWarranty"];
					c.CustomField1 = (Convert.IsDBNull(row["CustomField1"]))?string.Empty:(System.String)row["CustomField1"];
					c.CustomField2 = (Convert.IsDBNull(row["CustomField2"]))?string.Empty:(System.String)row["CustomField2"];
					c.CustomField3 = (Convert.IsDBNull(row["CustomField3"]))?string.Empty:(System.String)row["CustomField3"];
					c.NetworkType = (Convert.IsDBNull(row["NetworkType"]))?string.Empty:(System.String)row["NetworkType"];
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
		/// Fill an <see cref="VList&lt;METAViewNetworkComponentListing&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAViewNetworkComponentListing&gt;"/></returns>
		protected VList<METAViewNetworkComponentListing> Fill(IDataReader reader, VList<METAViewNetworkComponentListing> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAViewNetworkComponentListing entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAViewNetworkComponentListing>("METAViewNetworkComponentListing",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAViewNetworkComponentListing();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAViewNetworkComponentListingColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewNetworkComponentListingColumn.WorkspaceTypeID)];
					//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
					entity.VCStatusID = (System.Int32)reader[((int)METAViewNetworkComponentListingColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.Pkid = (System.Int32)reader[((int)METAViewNetworkComponentListingColumn.Pkid)];
					//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAViewNetworkComponentListingColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Type = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.Type)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.Type)];
					//entity.Type = (Convert.IsDBNull(reader["Type"]))?string.Empty:(System.String)reader["Type"];
					entity.Name = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.Name)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.Name)];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.Description = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.Description)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.Description)];
					//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
					entity.SeverityRating = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.SeverityRating)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.SeverityRating)];
					//entity.SeverityRating = (Convert.IsDBNull(reader["SeverityRating"]))?string.Empty:(System.String)reader["SeverityRating"];
					entity.Configuration = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.Configuration)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.Configuration)];
					//entity.Configuration = (Convert.IsDBNull(reader["Configuration"]))?string.Empty:(System.String)reader["Configuration"];
					entity.MacAddress = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.MacAddress)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.MacAddress)];
					//entity.MacAddress = (Convert.IsDBNull(reader["MacAddress"]))?string.Empty:(System.String)reader["MacAddress"];
					entity.NetworkAddress2 = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.NetworkAddress2)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.NetworkAddress2)];
					//entity.NetworkAddress2 = (Convert.IsDBNull(reader["NetworkAddress2"]))?string.Empty:(System.String)reader["NetworkAddress2"];
					entity.NetworkAddress3 = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.NetworkAddress3)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.NetworkAddress3)];
					//entity.NetworkAddress3 = (Convert.IsDBNull(reader["NetworkAddress3"]))?string.Empty:(System.String)reader["NetworkAddress3"];
					entity.NetworkAddress4 = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.NetworkAddress4)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.NetworkAddress4)];
					//entity.NetworkAddress4 = (Convert.IsDBNull(reader["NetworkAddress4"]))?string.Empty:(System.String)reader["NetworkAddress4"];
					entity.NetworkAddress5 = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.NetworkAddress5)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.NetworkAddress5)];
					//entity.NetworkAddress5 = (Convert.IsDBNull(reader["NetworkAddress5"]))?string.Empty:(System.String)reader["NetworkAddress5"];
					entity.Make = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.Make)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.Make)];
					//entity.Make = (Convert.IsDBNull(reader["Make"]))?string.Empty:(System.String)reader["Make"];
					entity.Model = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.Model)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.Model)];
					//entity.Model = (Convert.IsDBNull(reader["Model"]))?string.Empty:(System.String)reader["Model"];
					entity.SerialNumber = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.SerialNumber)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.SerialNumber)];
					//entity.SerialNumber = (Convert.IsDBNull(reader["SerialNumber"]))?string.Empty:(System.String)reader["SerialNumber"];
					entity.AssetNumber = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.AssetNumber)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.AssetNumber)];
					//entity.AssetNumber = (Convert.IsDBNull(reader["AssetNumber"]))?string.Empty:(System.String)reader["AssetNumber"];
					entity.ConnectionSpeed = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.ConnectionSpeed)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.ConnectionSpeed)];
					//entity.ConnectionSpeed = (Convert.IsDBNull(reader["ConnectionSpeed"]))?string.Empty:(System.String)reader["ConnectionSpeed"];
					entity.NumberOfPorts = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.NumberOfPorts)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.NumberOfPorts)];
					//entity.NumberOfPorts = (Convert.IsDBNull(reader["Number_of_Ports"]))?string.Empty:(System.String)reader["Number_of_Ports"];
					entity.NumberOfPortsAvailable = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.NumberOfPortsAvailable)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.NumberOfPortsAvailable)];
					//entity.NumberOfPortsAvailable = (Convert.IsDBNull(reader["Number_of_Ports_Available"]))?string.Empty:(System.String)reader["Number_of_Ports_Available"];
					entity.Range = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.Range)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.Range)];
					//entity.Range = (Convert.IsDBNull(reader["Range"]))?string.Empty:(System.String)reader["Range"];
					entity.IsDNS = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.IsDNS)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.IsDNS)];
					//entity.IsDNS = (Convert.IsDBNull(reader["isDNS"]))?string.Empty:(System.String)reader["isDNS"];
					entity.IsDHCP = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.IsDHCP)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.IsDHCP)];
					//entity.IsDHCP = (Convert.IsDBNull(reader["isDHCP"]))?string.Empty:(System.String)reader["isDHCP"];
					entity.IsManaged = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.IsManaged)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.IsManaged)];
					//entity.IsManaged = (Convert.IsDBNull(reader["isManaged"]))?string.Empty:(System.String)reader["isManaged"];
					entity.MemTotal = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.MemTotal)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.MemTotal)];
					//entity.MemTotal = (Convert.IsDBNull(reader["Mem_Total"]))?string.Empty:(System.String)reader["Mem_Total"];
					entity.DatePurchased = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.DatePurchased)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.DatePurchased)];
					//entity.DatePurchased = (Convert.IsDBNull(reader["DatePurchased"]))?string.Empty:(System.String)reader["DatePurchased"];
					entity.UnderWarranty = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.UnderWarranty)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.UnderWarranty)];
					//entity.UnderWarranty = (Convert.IsDBNull(reader["UnderWarranty"]))?string.Empty:(System.String)reader["UnderWarranty"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.CustomField1)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.CustomField2)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.CustomField3)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.NetworkType = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.NetworkType)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.NetworkType)];
					//entity.NetworkType = (Convert.IsDBNull(reader["NetworkType"]))?string.Empty:(System.String)reader["NetworkType"];
					entity.GapType = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.GapType)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.GapType)];
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
		/// Refreshes the <see cref="METAViewNetworkComponentListing"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewNetworkComponentListing"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAViewNetworkComponentListing entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAViewNetworkComponentListingColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewNetworkComponentListingColumn.WorkspaceTypeID)];
			//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
			entity.VCStatusID = (System.Int32)reader[((int)METAViewNetworkComponentListingColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.Pkid = (System.Int32)reader[((int)METAViewNetworkComponentListingColumn.Pkid)];
			//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAViewNetworkComponentListingColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Type = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.Type)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.Type)];
			//entity.Type = (Convert.IsDBNull(reader["Type"]))?string.Empty:(System.String)reader["Type"];
			entity.Name = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.Name)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.Name)];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.Description = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.Description)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.Description)];
			//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
			entity.SeverityRating = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.SeverityRating)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.SeverityRating)];
			//entity.SeverityRating = (Convert.IsDBNull(reader["SeverityRating"]))?string.Empty:(System.String)reader["SeverityRating"];
			entity.Configuration = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.Configuration)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.Configuration)];
			//entity.Configuration = (Convert.IsDBNull(reader["Configuration"]))?string.Empty:(System.String)reader["Configuration"];
			entity.MacAddress = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.MacAddress)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.MacAddress)];
			//entity.MacAddress = (Convert.IsDBNull(reader["MacAddress"]))?string.Empty:(System.String)reader["MacAddress"];
			entity.NetworkAddress2 = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.NetworkAddress2)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.NetworkAddress2)];
			//entity.NetworkAddress2 = (Convert.IsDBNull(reader["NetworkAddress2"]))?string.Empty:(System.String)reader["NetworkAddress2"];
			entity.NetworkAddress3 = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.NetworkAddress3)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.NetworkAddress3)];
			//entity.NetworkAddress3 = (Convert.IsDBNull(reader["NetworkAddress3"]))?string.Empty:(System.String)reader["NetworkAddress3"];
			entity.NetworkAddress4 = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.NetworkAddress4)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.NetworkAddress4)];
			//entity.NetworkAddress4 = (Convert.IsDBNull(reader["NetworkAddress4"]))?string.Empty:(System.String)reader["NetworkAddress4"];
			entity.NetworkAddress5 = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.NetworkAddress5)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.NetworkAddress5)];
			//entity.NetworkAddress5 = (Convert.IsDBNull(reader["NetworkAddress5"]))?string.Empty:(System.String)reader["NetworkAddress5"];
			entity.Make = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.Make)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.Make)];
			//entity.Make = (Convert.IsDBNull(reader["Make"]))?string.Empty:(System.String)reader["Make"];
			entity.Model = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.Model)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.Model)];
			//entity.Model = (Convert.IsDBNull(reader["Model"]))?string.Empty:(System.String)reader["Model"];
			entity.SerialNumber = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.SerialNumber)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.SerialNumber)];
			//entity.SerialNumber = (Convert.IsDBNull(reader["SerialNumber"]))?string.Empty:(System.String)reader["SerialNumber"];
			entity.AssetNumber = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.AssetNumber)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.AssetNumber)];
			//entity.AssetNumber = (Convert.IsDBNull(reader["AssetNumber"]))?string.Empty:(System.String)reader["AssetNumber"];
			entity.ConnectionSpeed = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.ConnectionSpeed)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.ConnectionSpeed)];
			//entity.ConnectionSpeed = (Convert.IsDBNull(reader["ConnectionSpeed"]))?string.Empty:(System.String)reader["ConnectionSpeed"];
			entity.NumberOfPorts = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.NumberOfPorts)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.NumberOfPorts)];
			//entity.NumberOfPorts = (Convert.IsDBNull(reader["Number_of_Ports"]))?string.Empty:(System.String)reader["Number_of_Ports"];
			entity.NumberOfPortsAvailable = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.NumberOfPortsAvailable)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.NumberOfPortsAvailable)];
			//entity.NumberOfPortsAvailable = (Convert.IsDBNull(reader["Number_of_Ports_Available"]))?string.Empty:(System.String)reader["Number_of_Ports_Available"];
			entity.Range = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.Range)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.Range)];
			//entity.Range = (Convert.IsDBNull(reader["Range"]))?string.Empty:(System.String)reader["Range"];
			entity.IsDNS = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.IsDNS)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.IsDNS)];
			//entity.IsDNS = (Convert.IsDBNull(reader["isDNS"]))?string.Empty:(System.String)reader["isDNS"];
			entity.IsDHCP = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.IsDHCP)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.IsDHCP)];
			//entity.IsDHCP = (Convert.IsDBNull(reader["isDHCP"]))?string.Empty:(System.String)reader["isDHCP"];
			entity.IsManaged = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.IsManaged)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.IsManaged)];
			//entity.IsManaged = (Convert.IsDBNull(reader["isManaged"]))?string.Empty:(System.String)reader["isManaged"];
			entity.MemTotal = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.MemTotal)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.MemTotal)];
			//entity.MemTotal = (Convert.IsDBNull(reader["Mem_Total"]))?string.Empty:(System.String)reader["Mem_Total"];
			entity.DatePurchased = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.DatePurchased)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.DatePurchased)];
			//entity.DatePurchased = (Convert.IsDBNull(reader["DatePurchased"]))?string.Empty:(System.String)reader["DatePurchased"];
			entity.UnderWarranty = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.UnderWarranty)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.UnderWarranty)];
			//entity.UnderWarranty = (Convert.IsDBNull(reader["UnderWarranty"]))?string.Empty:(System.String)reader["UnderWarranty"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.CustomField1)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.CustomField2)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.CustomField3)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.NetworkType = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.NetworkType)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.NetworkType)];
			//entity.NetworkType = (Convert.IsDBNull(reader["NetworkType"]))?string.Empty:(System.String)reader["NetworkType"];
			entity.GapType = (reader.IsDBNull(((int)METAViewNetworkComponentListingColumn.GapType)))?null:(System.String)reader[((int)METAViewNetworkComponentListingColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAViewNetworkComponentListing"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewNetworkComponentListing"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAViewNetworkComponentListing entity)
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
			entity.MacAddress = (Convert.IsDBNull(dataRow["MacAddress"]))?string.Empty:(System.String)dataRow["MacAddress"];
			entity.NetworkAddress2 = (Convert.IsDBNull(dataRow["NetworkAddress2"]))?string.Empty:(System.String)dataRow["NetworkAddress2"];
			entity.NetworkAddress3 = (Convert.IsDBNull(dataRow["NetworkAddress3"]))?string.Empty:(System.String)dataRow["NetworkAddress3"];
			entity.NetworkAddress4 = (Convert.IsDBNull(dataRow["NetworkAddress4"]))?string.Empty:(System.String)dataRow["NetworkAddress4"];
			entity.NetworkAddress5 = (Convert.IsDBNull(dataRow["NetworkAddress5"]))?string.Empty:(System.String)dataRow["NetworkAddress5"];
			entity.Make = (Convert.IsDBNull(dataRow["Make"]))?string.Empty:(System.String)dataRow["Make"];
			entity.Model = (Convert.IsDBNull(dataRow["Model"]))?string.Empty:(System.String)dataRow["Model"];
			entity.SerialNumber = (Convert.IsDBNull(dataRow["SerialNumber"]))?string.Empty:(System.String)dataRow["SerialNumber"];
			entity.AssetNumber = (Convert.IsDBNull(dataRow["AssetNumber"]))?string.Empty:(System.String)dataRow["AssetNumber"];
			entity.ConnectionSpeed = (Convert.IsDBNull(dataRow["ConnectionSpeed"]))?string.Empty:(System.String)dataRow["ConnectionSpeed"];
			entity.NumberOfPorts = (Convert.IsDBNull(dataRow["Number_of_Ports"]))?string.Empty:(System.String)dataRow["Number_of_Ports"];
			entity.NumberOfPortsAvailable = (Convert.IsDBNull(dataRow["Number_of_Ports_Available"]))?string.Empty:(System.String)dataRow["Number_of_Ports_Available"];
			entity.Range = (Convert.IsDBNull(dataRow["Range"]))?string.Empty:(System.String)dataRow["Range"];
			entity.IsDNS = (Convert.IsDBNull(dataRow["isDNS"]))?string.Empty:(System.String)dataRow["isDNS"];
			entity.IsDHCP = (Convert.IsDBNull(dataRow["isDHCP"]))?string.Empty:(System.String)dataRow["isDHCP"];
			entity.IsManaged = (Convert.IsDBNull(dataRow["isManaged"]))?string.Empty:(System.String)dataRow["isManaged"];
			entity.MemTotal = (Convert.IsDBNull(dataRow["Mem_Total"]))?string.Empty:(System.String)dataRow["Mem_Total"];
			entity.DatePurchased = (Convert.IsDBNull(dataRow["DatePurchased"]))?string.Empty:(System.String)dataRow["DatePurchased"];
			entity.UnderWarranty = (Convert.IsDBNull(dataRow["UnderWarranty"]))?string.Empty:(System.String)dataRow["UnderWarranty"];
			entity.CustomField1 = (Convert.IsDBNull(dataRow["CustomField1"]))?string.Empty:(System.String)dataRow["CustomField1"];
			entity.CustomField2 = (Convert.IsDBNull(dataRow["CustomField2"]))?string.Empty:(System.String)dataRow["CustomField2"];
			entity.CustomField3 = (Convert.IsDBNull(dataRow["CustomField3"]))?string.Empty:(System.String)dataRow["CustomField3"];
			entity.NetworkType = (Convert.IsDBNull(dataRow["NetworkType"]))?string.Empty:(System.String)dataRow["NetworkType"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAViewNetworkComponentListingFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewNetworkComponentListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewNetworkComponentListingFilterBuilder : SqlFilterBuilder<METAViewNetworkComponentListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewNetworkComponentListingFilterBuilder class.
		/// </summary>
		public METAViewNetworkComponentListingFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewNetworkComponentListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewNetworkComponentListingFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewNetworkComponentListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewNetworkComponentListingFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewNetworkComponentListingFilterBuilder

	#region METAViewNetworkComponentListingParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewNetworkComponentListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewNetworkComponentListingParameterBuilder : ParameterizedSqlFilterBuilder<METAViewNetworkComponentListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewNetworkComponentListingParameterBuilder class.
		/// </summary>
		public METAViewNetworkComponentListingParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewNetworkComponentListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewNetworkComponentListingParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewNetworkComponentListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewNetworkComponentListingParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewNetworkComponentListingParameterBuilder
	
	#region METAViewNetworkComponentListingSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewNetworkComponentListing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAViewNetworkComponentListingSortBuilder : SqlSortBuilder<METAViewNetworkComponentListingColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewNetworkComponentListingSqlSortBuilder class.
		/// </summary>
		public METAViewNetworkComponentListingSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAViewNetworkComponentListingSortBuilder

} // end namespace
