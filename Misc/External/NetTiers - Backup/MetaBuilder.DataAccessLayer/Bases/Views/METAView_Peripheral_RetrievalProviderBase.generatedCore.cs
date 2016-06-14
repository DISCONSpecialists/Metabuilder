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
	/// This class is the base class for any <see cref="METAView_Peripheral_RetrievalProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAView_Peripheral_RetrievalProviderBaseCore : EntityViewProviderBase<METAView_Peripheral_Retrieval>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAView_Peripheral_Retrieval&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAView_Peripheral_Retrieval&gt;"/></returns>
		protected static VList&lt;METAView_Peripheral_Retrieval&gt; Fill(DataSet dataSet, VList<METAView_Peripheral_Retrieval> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAView_Peripheral_Retrieval>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAView_Peripheral_Retrieval&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAView_Peripheral_Retrieval>"/></returns>
		protected static VList&lt;METAView_Peripheral_Retrieval&gt; Fill(DataTable dataTable, VList<METAView_Peripheral_Retrieval> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAView_Peripheral_Retrieval c = new METAView_Peripheral_Retrieval();
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
					c.NetworkAddress1 = (Convert.IsDBNull(row["NetworkAddress1"]))?string.Empty:(System.String)row["NetworkAddress1"];
					c.NetworkAddress2 = (Convert.IsDBNull(row["NetworkAddress2"]))?string.Empty:(System.String)row["NetworkAddress2"];
					c.NetworkAddress3 = (Convert.IsDBNull(row["NetworkAddress3"]))?string.Empty:(System.String)row["NetworkAddress3"];
					c.NetworkAddress4 = (Convert.IsDBNull(row["NetworkAddress4"]))?string.Empty:(System.String)row["NetworkAddress4"];
					c.NetworkAddress5 = (Convert.IsDBNull(row["NetworkAddress5"]))?string.Empty:(System.String)row["NetworkAddress5"];
					c.Make = (Convert.IsDBNull(row["Make"]))?string.Empty:(System.String)row["Make"];
					c.Model = (Convert.IsDBNull(row["Model"]))?string.Empty:(System.String)row["Model"];
					c.SerialNumber = (Convert.IsDBNull(row["SerialNumber"]))?string.Empty:(System.String)row["SerialNumber"];
					c.AssetNumber = (Convert.IsDBNull(row["AssetNumber"]))?string.Empty:(System.String)row["AssetNumber"];
					c.Copy_PPM = (Convert.IsDBNull(row["Copy_PPM"]))?string.Empty:(System.String)row["Copy_PPM"];
					c.Print_PPM = (Convert.IsDBNull(row["Print_PPM"]))?string.Empty:(System.String)row["Print_PPM"];
					c.isColor = (Convert.IsDBNull(row["isColor"]))?string.Empty:(System.String)row["isColor"];
					c.isManaged = (Convert.IsDBNull(row["isManaged"]))?string.Empty:(System.String)row["isManaged"];
					c.isNetwork = (Convert.IsDBNull(row["isNetwork"]))?string.Empty:(System.String)row["isNetwork"];
					c.DatePurchased = (Convert.IsDBNull(row["DatePurchased"]))?string.Empty:(System.String)row["DatePurchased"];
					c.UnderWarranty = (Convert.IsDBNull(row["UnderWarranty"]))?string.Empty:(System.String)row["UnderWarranty"];
					c.Contract = (Convert.IsDBNull(row["Contract"]))?string.Empty:(System.String)row["Contract"];
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
		/// Fill an <see cref="VList&lt;METAView_Peripheral_Retrieval&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAView_Peripheral_Retrieval&gt;"/></returns>
		protected VList<METAView_Peripheral_Retrieval> Fill(IDataReader reader, VList<METAView_Peripheral_Retrieval> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAView_Peripheral_Retrieval entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAView_Peripheral_Retrieval>("METAView_Peripheral_Retrieval",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAView_Peripheral_Retrieval();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAView_Peripheral_RetrievalColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_Peripheral_RetrievalColumn.WorkspaceTypeId)];
					//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
					entity.VCStatusID = (System.Int32)reader[((int)METAView_Peripheral_RetrievalColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.pkid = (System.Int32)reader[((int)METAView_Peripheral_RetrievalColumn.pkid)];
					//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAView_Peripheral_RetrievalColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Type = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.Type)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.Type)];
					//entity.Type = (Convert.IsDBNull(reader["Type"]))?string.Empty:(System.String)reader["Type"];
					entity.Name = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.Name)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.Name)];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.Description = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.Description)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.Description)];
					//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
					entity.SeverityRating = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.SeverityRating)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.SeverityRating)];
					//entity.SeverityRating = (Convert.IsDBNull(reader["SeverityRating"]))?string.Empty:(System.String)reader["SeverityRating"];
					entity.Configuration = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.Configuration)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.Configuration)];
					//entity.Configuration = (Convert.IsDBNull(reader["Configuration"]))?string.Empty:(System.String)reader["Configuration"];
					entity.NetworkAddress1 = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.NetworkAddress1)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.NetworkAddress1)];
					//entity.NetworkAddress1 = (Convert.IsDBNull(reader["NetworkAddress1"]))?string.Empty:(System.String)reader["NetworkAddress1"];
					entity.NetworkAddress2 = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.NetworkAddress2)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.NetworkAddress2)];
					//entity.NetworkAddress2 = (Convert.IsDBNull(reader["NetworkAddress2"]))?string.Empty:(System.String)reader["NetworkAddress2"];
					entity.NetworkAddress3 = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.NetworkAddress3)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.NetworkAddress3)];
					//entity.NetworkAddress3 = (Convert.IsDBNull(reader["NetworkAddress3"]))?string.Empty:(System.String)reader["NetworkAddress3"];
					entity.NetworkAddress4 = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.NetworkAddress4)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.NetworkAddress4)];
					//entity.NetworkAddress4 = (Convert.IsDBNull(reader["NetworkAddress4"]))?string.Empty:(System.String)reader["NetworkAddress4"];
					entity.NetworkAddress5 = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.NetworkAddress5)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.NetworkAddress5)];
					//entity.NetworkAddress5 = (Convert.IsDBNull(reader["NetworkAddress5"]))?string.Empty:(System.String)reader["NetworkAddress5"];
					entity.Make = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.Make)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.Make)];
					//entity.Make = (Convert.IsDBNull(reader["Make"]))?string.Empty:(System.String)reader["Make"];
					entity.Model = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.Model)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.Model)];
					//entity.Model = (Convert.IsDBNull(reader["Model"]))?string.Empty:(System.String)reader["Model"];
					entity.SerialNumber = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.SerialNumber)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.SerialNumber)];
					//entity.SerialNumber = (Convert.IsDBNull(reader["SerialNumber"]))?string.Empty:(System.String)reader["SerialNumber"];
					entity.AssetNumber = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.AssetNumber)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.AssetNumber)];
					//entity.AssetNumber = (Convert.IsDBNull(reader["AssetNumber"]))?string.Empty:(System.String)reader["AssetNumber"];
					entity.Copy_PPM = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.Copy_PPM)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.Copy_PPM)];
					//entity.Copy_PPM = (Convert.IsDBNull(reader["Copy_PPM"]))?string.Empty:(System.String)reader["Copy_PPM"];
					entity.Print_PPM = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.Print_PPM)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.Print_PPM)];
					//entity.Print_PPM = (Convert.IsDBNull(reader["Print_PPM"]))?string.Empty:(System.String)reader["Print_PPM"];
					entity.isColor = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.isColor)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.isColor)];
					//entity.isColor = (Convert.IsDBNull(reader["isColor"]))?string.Empty:(System.String)reader["isColor"];
					entity.isManaged = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.isManaged)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.isManaged)];
					//entity.isManaged = (Convert.IsDBNull(reader["isManaged"]))?string.Empty:(System.String)reader["isManaged"];
					entity.isNetwork = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.isNetwork)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.isNetwork)];
					//entity.isNetwork = (Convert.IsDBNull(reader["isNetwork"]))?string.Empty:(System.String)reader["isNetwork"];
					entity.DatePurchased = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.DatePurchased)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.DatePurchased)];
					//entity.DatePurchased = (Convert.IsDBNull(reader["DatePurchased"]))?string.Empty:(System.String)reader["DatePurchased"];
					entity.UnderWarranty = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.UnderWarranty)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.UnderWarranty)];
					//entity.UnderWarranty = (Convert.IsDBNull(reader["UnderWarranty"]))?string.Empty:(System.String)reader["UnderWarranty"];
					entity.Contract = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.Contract)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.Contract)];
					//entity.Contract = (Convert.IsDBNull(reader["Contract"]))?string.Empty:(System.String)reader["Contract"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.GapType = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.GapType)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.GapType)];
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
		/// Refreshes the <see cref="METAView_Peripheral_Retrieval"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_Peripheral_Retrieval"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAView_Peripheral_Retrieval entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAView_Peripheral_RetrievalColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_Peripheral_RetrievalColumn.WorkspaceTypeId)];
			//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
			entity.VCStatusID = (System.Int32)reader[((int)METAView_Peripheral_RetrievalColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.pkid = (System.Int32)reader[((int)METAView_Peripheral_RetrievalColumn.pkid)];
			//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAView_Peripheral_RetrievalColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Type = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.Type)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.Type)];
			//entity.Type = (Convert.IsDBNull(reader["Type"]))?string.Empty:(System.String)reader["Type"];
			entity.Name = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.Name)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.Name)];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.Description = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.Description)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.Description)];
			//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
			entity.SeverityRating = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.SeverityRating)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.SeverityRating)];
			//entity.SeverityRating = (Convert.IsDBNull(reader["SeverityRating"]))?string.Empty:(System.String)reader["SeverityRating"];
			entity.Configuration = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.Configuration)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.Configuration)];
			//entity.Configuration = (Convert.IsDBNull(reader["Configuration"]))?string.Empty:(System.String)reader["Configuration"];
			entity.NetworkAddress1 = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.NetworkAddress1)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.NetworkAddress1)];
			//entity.NetworkAddress1 = (Convert.IsDBNull(reader["NetworkAddress1"]))?string.Empty:(System.String)reader["NetworkAddress1"];
			entity.NetworkAddress2 = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.NetworkAddress2)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.NetworkAddress2)];
			//entity.NetworkAddress2 = (Convert.IsDBNull(reader["NetworkAddress2"]))?string.Empty:(System.String)reader["NetworkAddress2"];
			entity.NetworkAddress3 = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.NetworkAddress3)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.NetworkAddress3)];
			//entity.NetworkAddress3 = (Convert.IsDBNull(reader["NetworkAddress3"]))?string.Empty:(System.String)reader["NetworkAddress3"];
			entity.NetworkAddress4 = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.NetworkAddress4)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.NetworkAddress4)];
			//entity.NetworkAddress4 = (Convert.IsDBNull(reader["NetworkAddress4"]))?string.Empty:(System.String)reader["NetworkAddress4"];
			entity.NetworkAddress5 = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.NetworkAddress5)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.NetworkAddress5)];
			//entity.NetworkAddress5 = (Convert.IsDBNull(reader["NetworkAddress5"]))?string.Empty:(System.String)reader["NetworkAddress5"];
			entity.Make = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.Make)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.Make)];
			//entity.Make = (Convert.IsDBNull(reader["Make"]))?string.Empty:(System.String)reader["Make"];
			entity.Model = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.Model)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.Model)];
			//entity.Model = (Convert.IsDBNull(reader["Model"]))?string.Empty:(System.String)reader["Model"];
			entity.SerialNumber = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.SerialNumber)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.SerialNumber)];
			//entity.SerialNumber = (Convert.IsDBNull(reader["SerialNumber"]))?string.Empty:(System.String)reader["SerialNumber"];
			entity.AssetNumber = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.AssetNumber)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.AssetNumber)];
			//entity.AssetNumber = (Convert.IsDBNull(reader["AssetNumber"]))?string.Empty:(System.String)reader["AssetNumber"];
			entity.Copy_PPM = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.Copy_PPM)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.Copy_PPM)];
			//entity.Copy_PPM = (Convert.IsDBNull(reader["Copy_PPM"]))?string.Empty:(System.String)reader["Copy_PPM"];
			entity.Print_PPM = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.Print_PPM)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.Print_PPM)];
			//entity.Print_PPM = (Convert.IsDBNull(reader["Print_PPM"]))?string.Empty:(System.String)reader["Print_PPM"];
			entity.isColor = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.isColor)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.isColor)];
			//entity.isColor = (Convert.IsDBNull(reader["isColor"]))?string.Empty:(System.String)reader["isColor"];
			entity.isManaged = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.isManaged)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.isManaged)];
			//entity.isManaged = (Convert.IsDBNull(reader["isManaged"]))?string.Empty:(System.String)reader["isManaged"];
			entity.isNetwork = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.isNetwork)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.isNetwork)];
			//entity.isNetwork = (Convert.IsDBNull(reader["isNetwork"]))?string.Empty:(System.String)reader["isNetwork"];
			entity.DatePurchased = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.DatePurchased)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.DatePurchased)];
			//entity.DatePurchased = (Convert.IsDBNull(reader["DatePurchased"]))?string.Empty:(System.String)reader["DatePurchased"];
			entity.UnderWarranty = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.UnderWarranty)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.UnderWarranty)];
			//entity.UnderWarranty = (Convert.IsDBNull(reader["UnderWarranty"]))?string.Empty:(System.String)reader["UnderWarranty"];
			entity.Contract = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.Contract)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.Contract)];
			//entity.Contract = (Convert.IsDBNull(reader["Contract"]))?string.Empty:(System.String)reader["Contract"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.GapType = (reader.IsDBNull(((int)METAView_Peripheral_RetrievalColumn.GapType)))?null:(System.String)reader[((int)METAView_Peripheral_RetrievalColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAView_Peripheral_Retrieval"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_Peripheral_Retrieval"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAView_Peripheral_Retrieval entity)
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
			entity.NetworkAddress1 = (Convert.IsDBNull(dataRow["NetworkAddress1"]))?string.Empty:(System.String)dataRow["NetworkAddress1"];
			entity.NetworkAddress2 = (Convert.IsDBNull(dataRow["NetworkAddress2"]))?string.Empty:(System.String)dataRow["NetworkAddress2"];
			entity.NetworkAddress3 = (Convert.IsDBNull(dataRow["NetworkAddress3"]))?string.Empty:(System.String)dataRow["NetworkAddress3"];
			entity.NetworkAddress4 = (Convert.IsDBNull(dataRow["NetworkAddress4"]))?string.Empty:(System.String)dataRow["NetworkAddress4"];
			entity.NetworkAddress5 = (Convert.IsDBNull(dataRow["NetworkAddress5"]))?string.Empty:(System.String)dataRow["NetworkAddress5"];
			entity.Make = (Convert.IsDBNull(dataRow["Make"]))?string.Empty:(System.String)dataRow["Make"];
			entity.Model = (Convert.IsDBNull(dataRow["Model"]))?string.Empty:(System.String)dataRow["Model"];
			entity.SerialNumber = (Convert.IsDBNull(dataRow["SerialNumber"]))?string.Empty:(System.String)dataRow["SerialNumber"];
			entity.AssetNumber = (Convert.IsDBNull(dataRow["AssetNumber"]))?string.Empty:(System.String)dataRow["AssetNumber"];
			entity.Copy_PPM = (Convert.IsDBNull(dataRow["Copy_PPM"]))?string.Empty:(System.String)dataRow["Copy_PPM"];
			entity.Print_PPM = (Convert.IsDBNull(dataRow["Print_PPM"]))?string.Empty:(System.String)dataRow["Print_PPM"];
			entity.isColor = (Convert.IsDBNull(dataRow["isColor"]))?string.Empty:(System.String)dataRow["isColor"];
			entity.isManaged = (Convert.IsDBNull(dataRow["isManaged"]))?string.Empty:(System.String)dataRow["isManaged"];
			entity.isNetwork = (Convert.IsDBNull(dataRow["isNetwork"]))?string.Empty:(System.String)dataRow["isNetwork"];
			entity.DatePurchased = (Convert.IsDBNull(dataRow["DatePurchased"]))?string.Empty:(System.String)dataRow["DatePurchased"];
			entity.UnderWarranty = (Convert.IsDBNull(dataRow["UnderWarranty"]))?string.Empty:(System.String)dataRow["UnderWarranty"];
			entity.Contract = (Convert.IsDBNull(dataRow["Contract"]))?string.Empty:(System.String)dataRow["Contract"];
			entity.CustomField1 = (Convert.IsDBNull(dataRow["CustomField1"]))?string.Empty:(System.String)dataRow["CustomField1"];
			entity.CustomField2 = (Convert.IsDBNull(dataRow["CustomField2"]))?string.Empty:(System.String)dataRow["CustomField2"];
			entity.CustomField3 = (Convert.IsDBNull(dataRow["CustomField3"]))?string.Empty:(System.String)dataRow["CustomField3"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAView_Peripheral_RetrievalFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_Peripheral_Retrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_Peripheral_RetrievalFilterBuilder : SqlFilterBuilder<METAView_Peripheral_RetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_Peripheral_RetrievalFilterBuilder class.
		/// </summary>
		public METAView_Peripheral_RetrievalFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_Peripheral_RetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_Peripheral_RetrievalFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_Peripheral_RetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_Peripheral_RetrievalFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_Peripheral_RetrievalFilterBuilder

	#region METAView_Peripheral_RetrievalParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_Peripheral_Retrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_Peripheral_RetrievalParameterBuilder : ParameterizedSqlFilterBuilder<METAView_Peripheral_RetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_Peripheral_RetrievalParameterBuilder class.
		/// </summary>
		public METAView_Peripheral_RetrievalParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_Peripheral_RetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_Peripheral_RetrievalParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_Peripheral_RetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_Peripheral_RetrievalParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_Peripheral_RetrievalParameterBuilder
	
	#region METAView_Peripheral_RetrievalSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_Peripheral_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Peripheral_RetrievalSortBuilder : SqlSortBuilder<METAView_Peripheral_RetrievalColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_Peripheral_RetrievalSqlSortBuilder class.
		/// </summary>
		public METAView_Peripheral_RetrievalSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAView_Peripheral_RetrievalSortBuilder

} // end namespace
