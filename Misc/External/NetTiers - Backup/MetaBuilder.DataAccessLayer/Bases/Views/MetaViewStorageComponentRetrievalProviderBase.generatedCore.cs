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
	/// This class is the base class for any <see cref="METAViewStorageComponentRetrievalProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAViewStorageComponentRetrievalProviderBaseCore : EntityViewProviderBase<METAViewStorageComponentRetrieval>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAViewStorageComponentRetrieval&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAViewStorageComponentRetrieval&gt;"/></returns>
		protected static VList&lt;METAViewStorageComponentRetrieval&gt; Fill(DataSet dataSet, VList<METAViewStorageComponentRetrieval> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAViewStorageComponentRetrieval>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAViewStorageComponentRetrieval&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAViewStorageComponentRetrieval>"/></returns>
		protected static VList&lt;METAViewStorageComponentRetrieval&gt; Fill(DataTable dataTable, VList<METAViewStorageComponentRetrieval> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAViewStorageComponentRetrieval c = new METAViewStorageComponentRetrieval();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeID = (Convert.IsDBNull(row["WorkspaceTypeID"]))?(int)0:(System.Int32?)row["WorkspaceTypeID"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.Pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.Type = (Convert.IsDBNull(row["Type"]))?string.Empty:(System.String)row["Type"];
					c.Name = (Convert.IsDBNull(row["Name"]))?string.Empty:(System.String)row["Name"];
					c.Description = (Convert.IsDBNull(row["Description"]))?string.Empty:(System.String)row["Description"];
					c.SeverityIndicator = (Convert.IsDBNull(row["SeverityIndicator"]))?string.Empty:(System.String)row["SeverityIndicator"];
					c.Configuration = (Convert.IsDBNull(row["Configuration"]))?string.Empty:(System.String)row["Configuration"];
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
					c.NumberOfDisks = (Convert.IsDBNull(row["Number_of_Disks"]))?string.Empty:(System.String)row["Number_of_Disks"];
					c.DatePurchased = (Convert.IsDBNull(row["DatePurchased"]))?string.Empty:(System.String)row["DatePurchased"];
					c.UnderWarranty = (Convert.IsDBNull(row["UnderWarranty"]))?string.Empty:(System.String)row["UnderWarranty"];
					c.FileSystem = (Convert.IsDBNull(row["FileSystem"]))?string.Empty:(System.String)row["FileSystem"];
					c.CustomField1 = (Convert.IsDBNull(row["CustomField1"]))?string.Empty:(System.String)row["CustomField1"];
					c.CustomField2 = (Convert.IsDBNull(row["CustomField2"]))?string.Empty:(System.String)row["CustomField2"];
					c.CustomField3 = (Convert.IsDBNull(row["CustomField3"]))?string.Empty:(System.String)row["CustomField3"];
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
		/// Fill an <see cref="VList&lt;METAViewStorageComponentRetrieval&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAViewStorageComponentRetrieval&gt;"/></returns>
		protected VList<METAViewStorageComponentRetrieval> Fill(IDataReader reader, VList<METAViewStorageComponentRetrieval> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAViewStorageComponentRetrieval entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAViewStorageComponentRetrieval>("METAViewStorageComponentRetrieval",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAViewStorageComponentRetrieval();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAViewStorageComponentRetrievalColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewStorageComponentRetrievalColumn.WorkspaceTypeID)];
					//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
					entity.VCStatusID = (System.Int32)reader[((int)METAViewStorageComponentRetrievalColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.Pkid = (System.Int32)reader[((int)METAViewStorageComponentRetrievalColumn.Pkid)];
					//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAViewStorageComponentRetrievalColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Type = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.Type)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.Type)];
					//entity.Type = (Convert.IsDBNull(reader["Type"]))?string.Empty:(System.String)reader["Type"];
					entity.Name = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.Name)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.Name)];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.Description = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.Description)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.Description)];
					//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
					entity.SeverityIndicator = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.SeverityIndicator)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.SeverityIndicator)];
					//entity.SeverityIndicator = (Convert.IsDBNull(reader["SeverityIndicator"]))?string.Empty:(System.String)reader["SeverityIndicator"];
					entity.Configuration = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.Configuration)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.Configuration)];
					//entity.Configuration = (Convert.IsDBNull(reader["Configuration"]))?string.Empty:(System.String)reader["Configuration"];
					entity.NetworkAddress1 = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.NetworkAddress1)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.NetworkAddress1)];
					//entity.NetworkAddress1 = (Convert.IsDBNull(reader["NetworkAddress1"]))?string.Empty:(System.String)reader["NetworkAddress1"];
					entity.NetworkAddress2 = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.NetworkAddress2)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.NetworkAddress2)];
					//entity.NetworkAddress2 = (Convert.IsDBNull(reader["NetworkAddress2"]))?string.Empty:(System.String)reader["NetworkAddress2"];
					entity.NetworkAddress3 = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.NetworkAddress3)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.NetworkAddress3)];
					//entity.NetworkAddress3 = (Convert.IsDBNull(reader["NetworkAddress3"]))?string.Empty:(System.String)reader["NetworkAddress3"];
					entity.NetworkAddress4 = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.NetworkAddress4)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.NetworkAddress4)];
					//entity.NetworkAddress4 = (Convert.IsDBNull(reader["NetworkAddress4"]))?string.Empty:(System.String)reader["NetworkAddress4"];
					entity.NetworkAddress5 = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.NetworkAddress5)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.NetworkAddress5)];
					//entity.NetworkAddress5 = (Convert.IsDBNull(reader["NetworkAddress5"]))?string.Empty:(System.String)reader["NetworkAddress5"];
					entity.Make = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.Make)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.Make)];
					//entity.Make = (Convert.IsDBNull(reader["Make"]))?string.Empty:(System.String)reader["Make"];
					entity.Model = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.Model)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.Model)];
					//entity.Model = (Convert.IsDBNull(reader["Model"]))?string.Empty:(System.String)reader["Model"];
					entity.SerialNumber = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.SerialNumber)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.SerialNumber)];
					//entity.SerialNumber = (Convert.IsDBNull(reader["SerialNumber"]))?string.Empty:(System.String)reader["SerialNumber"];
					entity.AssetNumber = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.AssetNumber)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.AssetNumber)];
					//entity.AssetNumber = (Convert.IsDBNull(reader["AssetNumber"]))?string.Empty:(System.String)reader["AssetNumber"];
					entity.Capacity = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.Capacity)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.Capacity)];
					//entity.Capacity = (Convert.IsDBNull(reader["Capacity"]))?string.Empty:(System.String)reader["Capacity"];
					entity.NumberOfDisks = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.NumberOfDisks)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.NumberOfDisks)];
					//entity.NumberOfDisks = (Convert.IsDBNull(reader["Number_of_Disks"]))?string.Empty:(System.String)reader["Number_of_Disks"];
					entity.DatePurchased = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.DatePurchased)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.DatePurchased)];
					//entity.DatePurchased = (Convert.IsDBNull(reader["DatePurchased"]))?string.Empty:(System.String)reader["DatePurchased"];
					entity.UnderWarranty = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.UnderWarranty)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.UnderWarranty)];
					//entity.UnderWarranty = (Convert.IsDBNull(reader["UnderWarranty"]))?string.Empty:(System.String)reader["UnderWarranty"];
					entity.FileSystem = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.FileSystem)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.FileSystem)];
					//entity.FileSystem = (Convert.IsDBNull(reader["FileSystem"]))?string.Empty:(System.String)reader["FileSystem"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.GapType = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.GapType)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.GapType)];
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
		/// Refreshes the <see cref="METAViewStorageComponentRetrieval"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewStorageComponentRetrieval"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAViewStorageComponentRetrieval entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAViewStorageComponentRetrievalColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewStorageComponentRetrievalColumn.WorkspaceTypeID)];
			//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
			entity.VCStatusID = (System.Int32)reader[((int)METAViewStorageComponentRetrievalColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.Pkid = (System.Int32)reader[((int)METAViewStorageComponentRetrievalColumn.Pkid)];
			//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAViewStorageComponentRetrievalColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Type = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.Type)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.Type)];
			//entity.Type = (Convert.IsDBNull(reader["Type"]))?string.Empty:(System.String)reader["Type"];
			entity.Name = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.Name)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.Name)];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.Description = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.Description)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.Description)];
			//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
			entity.SeverityIndicator = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.SeverityIndicator)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.SeverityIndicator)];
			//entity.SeverityIndicator = (Convert.IsDBNull(reader["SeverityIndicator"]))?string.Empty:(System.String)reader["SeverityIndicator"];
			entity.Configuration = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.Configuration)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.Configuration)];
			//entity.Configuration = (Convert.IsDBNull(reader["Configuration"]))?string.Empty:(System.String)reader["Configuration"];
			entity.NetworkAddress1 = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.NetworkAddress1)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.NetworkAddress1)];
			//entity.NetworkAddress1 = (Convert.IsDBNull(reader["NetworkAddress1"]))?string.Empty:(System.String)reader["NetworkAddress1"];
			entity.NetworkAddress2 = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.NetworkAddress2)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.NetworkAddress2)];
			//entity.NetworkAddress2 = (Convert.IsDBNull(reader["NetworkAddress2"]))?string.Empty:(System.String)reader["NetworkAddress2"];
			entity.NetworkAddress3 = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.NetworkAddress3)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.NetworkAddress3)];
			//entity.NetworkAddress3 = (Convert.IsDBNull(reader["NetworkAddress3"]))?string.Empty:(System.String)reader["NetworkAddress3"];
			entity.NetworkAddress4 = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.NetworkAddress4)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.NetworkAddress4)];
			//entity.NetworkAddress4 = (Convert.IsDBNull(reader["NetworkAddress4"]))?string.Empty:(System.String)reader["NetworkAddress4"];
			entity.NetworkAddress5 = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.NetworkAddress5)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.NetworkAddress5)];
			//entity.NetworkAddress5 = (Convert.IsDBNull(reader["NetworkAddress5"]))?string.Empty:(System.String)reader["NetworkAddress5"];
			entity.Make = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.Make)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.Make)];
			//entity.Make = (Convert.IsDBNull(reader["Make"]))?string.Empty:(System.String)reader["Make"];
			entity.Model = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.Model)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.Model)];
			//entity.Model = (Convert.IsDBNull(reader["Model"]))?string.Empty:(System.String)reader["Model"];
			entity.SerialNumber = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.SerialNumber)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.SerialNumber)];
			//entity.SerialNumber = (Convert.IsDBNull(reader["SerialNumber"]))?string.Empty:(System.String)reader["SerialNumber"];
			entity.AssetNumber = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.AssetNumber)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.AssetNumber)];
			//entity.AssetNumber = (Convert.IsDBNull(reader["AssetNumber"]))?string.Empty:(System.String)reader["AssetNumber"];
			entity.Capacity = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.Capacity)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.Capacity)];
			//entity.Capacity = (Convert.IsDBNull(reader["Capacity"]))?string.Empty:(System.String)reader["Capacity"];
			entity.NumberOfDisks = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.NumberOfDisks)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.NumberOfDisks)];
			//entity.NumberOfDisks = (Convert.IsDBNull(reader["Number_of_Disks"]))?string.Empty:(System.String)reader["Number_of_Disks"];
			entity.DatePurchased = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.DatePurchased)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.DatePurchased)];
			//entity.DatePurchased = (Convert.IsDBNull(reader["DatePurchased"]))?string.Empty:(System.String)reader["DatePurchased"];
			entity.UnderWarranty = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.UnderWarranty)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.UnderWarranty)];
			//entity.UnderWarranty = (Convert.IsDBNull(reader["UnderWarranty"]))?string.Empty:(System.String)reader["UnderWarranty"];
			entity.FileSystem = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.FileSystem)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.FileSystem)];
			//entity.FileSystem = (Convert.IsDBNull(reader["FileSystem"]))?string.Empty:(System.String)reader["FileSystem"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.GapType = (reader.IsDBNull(((int)METAViewStorageComponentRetrievalColumn.GapType)))?null:(System.String)reader[((int)METAViewStorageComponentRetrievalColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAViewStorageComponentRetrieval"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewStorageComponentRetrieval"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAViewStorageComponentRetrieval entity)
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
			entity.SeverityIndicator = (Convert.IsDBNull(dataRow["SeverityIndicator"]))?string.Empty:(System.String)dataRow["SeverityIndicator"];
			entity.Configuration = (Convert.IsDBNull(dataRow["Configuration"]))?string.Empty:(System.String)dataRow["Configuration"];
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
			entity.NumberOfDisks = (Convert.IsDBNull(dataRow["Number_of_Disks"]))?string.Empty:(System.String)dataRow["Number_of_Disks"];
			entity.DatePurchased = (Convert.IsDBNull(dataRow["DatePurchased"]))?string.Empty:(System.String)dataRow["DatePurchased"];
			entity.UnderWarranty = (Convert.IsDBNull(dataRow["UnderWarranty"]))?string.Empty:(System.String)dataRow["UnderWarranty"];
			entity.FileSystem = (Convert.IsDBNull(dataRow["FileSystem"]))?string.Empty:(System.String)dataRow["FileSystem"];
			entity.CustomField1 = (Convert.IsDBNull(dataRow["CustomField1"]))?string.Empty:(System.String)dataRow["CustomField1"];
			entity.CustomField2 = (Convert.IsDBNull(dataRow["CustomField2"]))?string.Empty:(System.String)dataRow["CustomField2"];
			entity.CustomField3 = (Convert.IsDBNull(dataRow["CustomField3"]))?string.Empty:(System.String)dataRow["CustomField3"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAViewStorageComponentRetrievalFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewStorageComponentRetrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewStorageComponentRetrievalFilterBuilder : SqlFilterBuilder<METAViewStorageComponentRetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewStorageComponentRetrievalFilterBuilder class.
		/// </summary>
		public METAViewStorageComponentRetrievalFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewStorageComponentRetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewStorageComponentRetrievalFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewStorageComponentRetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewStorageComponentRetrievalFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewStorageComponentRetrievalFilterBuilder

	#region METAViewStorageComponentRetrievalParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewStorageComponentRetrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewStorageComponentRetrievalParameterBuilder : ParameterizedSqlFilterBuilder<METAViewStorageComponentRetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewStorageComponentRetrievalParameterBuilder class.
		/// </summary>
		public METAViewStorageComponentRetrievalParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewStorageComponentRetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewStorageComponentRetrievalParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewStorageComponentRetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewStorageComponentRetrievalParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewStorageComponentRetrievalParameterBuilder
	
	#region METAViewStorageComponentRetrievalSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewStorageComponentRetrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAViewStorageComponentRetrievalSortBuilder : SqlSortBuilder<METAViewStorageComponentRetrievalColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewStorageComponentRetrievalSqlSortBuilder class.
		/// </summary>
		public METAViewStorageComponentRetrievalSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAViewStorageComponentRetrievalSortBuilder

} // end namespace
