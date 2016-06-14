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
	/// This class is the base class for any <see cref="METAViewPeripheralRetrievalProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAViewPeripheralRetrievalProviderBaseCore : EntityViewProviderBase<METAViewPeripheralRetrieval>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAViewPeripheralRetrieval&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAViewPeripheralRetrieval&gt;"/></returns>
		protected static VList&lt;METAViewPeripheralRetrieval&gt; Fill(DataSet dataSet, VList<METAViewPeripheralRetrieval> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAViewPeripheralRetrieval>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAViewPeripheralRetrieval&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAViewPeripheralRetrieval>"/></returns>
		protected static VList&lt;METAViewPeripheralRetrieval&gt; Fill(DataTable dataTable, VList<METAViewPeripheralRetrieval> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAViewPeripheralRetrieval c = new METAViewPeripheralRetrieval();
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
					c.NetworkAddress1 = (Convert.IsDBNull(row["NetworkAddress1"]))?string.Empty:(System.String)row["NetworkAddress1"];
					c.NetworkAddress2 = (Convert.IsDBNull(row["NetworkAddress2"]))?string.Empty:(System.String)row["NetworkAddress2"];
					c.NetworkAddress3 = (Convert.IsDBNull(row["NetworkAddress3"]))?string.Empty:(System.String)row["NetworkAddress3"];
					c.NetworkAddress4 = (Convert.IsDBNull(row["NetworkAddress4"]))?string.Empty:(System.String)row["NetworkAddress4"];
					c.NetworkAddress5 = (Convert.IsDBNull(row["NetworkAddress5"]))?string.Empty:(System.String)row["NetworkAddress5"];
					c.Make = (Convert.IsDBNull(row["Make"]))?string.Empty:(System.String)row["Make"];
					c.Model = (Convert.IsDBNull(row["Model"]))?string.Empty:(System.String)row["Model"];
					c.SerialNumber = (Convert.IsDBNull(row["SerialNumber"]))?string.Empty:(System.String)row["SerialNumber"];
					c.AssetNumber = (Convert.IsDBNull(row["AssetNumber"]))?string.Empty:(System.String)row["AssetNumber"];
					c.CopyPPM = (Convert.IsDBNull(row["Copy_PPM"]))?string.Empty:(System.String)row["Copy_PPM"];
					c.PrintPPM = (Convert.IsDBNull(row["Print_PPM"]))?string.Empty:(System.String)row["Print_PPM"];
					c.IsColor = (Convert.IsDBNull(row["isColor"]))?string.Empty:(System.String)row["isColor"];
					c.IsManaged = (Convert.IsDBNull(row["isManaged"]))?string.Empty:(System.String)row["isManaged"];
					c.IsNetwork = (Convert.IsDBNull(row["isNetwork"]))?string.Empty:(System.String)row["isNetwork"];
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
		/// Fill an <see cref="VList&lt;METAViewPeripheralRetrieval&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAViewPeripheralRetrieval&gt;"/></returns>
		protected VList<METAViewPeripheralRetrieval> Fill(IDataReader reader, VList<METAViewPeripheralRetrieval> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAViewPeripheralRetrieval entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAViewPeripheralRetrieval>("METAViewPeripheralRetrieval",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAViewPeripheralRetrieval();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAViewPeripheralRetrievalColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewPeripheralRetrievalColumn.WorkspaceTypeID)];
					//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
					entity.VCStatusID = (System.Int32)reader[((int)METAViewPeripheralRetrievalColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.Pkid = (System.Int32)reader[((int)METAViewPeripheralRetrievalColumn.Pkid)];
					//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAViewPeripheralRetrievalColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Type = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.Type)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.Type)];
					//entity.Type = (Convert.IsDBNull(reader["Type"]))?string.Empty:(System.String)reader["Type"];
					entity.Name = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.Name)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.Name)];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.Description = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.Description)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.Description)];
					//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
					entity.SeverityRating = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.SeverityRating)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.SeverityRating)];
					//entity.SeverityRating = (Convert.IsDBNull(reader["SeverityRating"]))?string.Empty:(System.String)reader["SeverityRating"];
					entity.Configuration = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.Configuration)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.Configuration)];
					//entity.Configuration = (Convert.IsDBNull(reader["Configuration"]))?string.Empty:(System.String)reader["Configuration"];
					entity.NetworkAddress1 = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.NetworkAddress1)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.NetworkAddress1)];
					//entity.NetworkAddress1 = (Convert.IsDBNull(reader["NetworkAddress1"]))?string.Empty:(System.String)reader["NetworkAddress1"];
					entity.NetworkAddress2 = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.NetworkAddress2)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.NetworkAddress2)];
					//entity.NetworkAddress2 = (Convert.IsDBNull(reader["NetworkAddress2"]))?string.Empty:(System.String)reader["NetworkAddress2"];
					entity.NetworkAddress3 = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.NetworkAddress3)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.NetworkAddress3)];
					//entity.NetworkAddress3 = (Convert.IsDBNull(reader["NetworkAddress3"]))?string.Empty:(System.String)reader["NetworkAddress3"];
					entity.NetworkAddress4 = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.NetworkAddress4)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.NetworkAddress4)];
					//entity.NetworkAddress4 = (Convert.IsDBNull(reader["NetworkAddress4"]))?string.Empty:(System.String)reader["NetworkAddress4"];
					entity.NetworkAddress5 = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.NetworkAddress5)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.NetworkAddress5)];
					//entity.NetworkAddress5 = (Convert.IsDBNull(reader["NetworkAddress5"]))?string.Empty:(System.String)reader["NetworkAddress5"];
					entity.Make = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.Make)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.Make)];
					//entity.Make = (Convert.IsDBNull(reader["Make"]))?string.Empty:(System.String)reader["Make"];
					entity.Model = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.Model)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.Model)];
					//entity.Model = (Convert.IsDBNull(reader["Model"]))?string.Empty:(System.String)reader["Model"];
					entity.SerialNumber = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.SerialNumber)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.SerialNumber)];
					//entity.SerialNumber = (Convert.IsDBNull(reader["SerialNumber"]))?string.Empty:(System.String)reader["SerialNumber"];
					entity.AssetNumber = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.AssetNumber)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.AssetNumber)];
					//entity.AssetNumber = (Convert.IsDBNull(reader["AssetNumber"]))?string.Empty:(System.String)reader["AssetNumber"];
					entity.CopyPPM = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.CopyPPM)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.CopyPPM)];
					//entity.CopyPPM = (Convert.IsDBNull(reader["Copy_PPM"]))?string.Empty:(System.String)reader["Copy_PPM"];
					entity.PrintPPM = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.PrintPPM)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.PrintPPM)];
					//entity.PrintPPM = (Convert.IsDBNull(reader["Print_PPM"]))?string.Empty:(System.String)reader["Print_PPM"];
					entity.IsColor = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.IsColor)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.IsColor)];
					//entity.IsColor = (Convert.IsDBNull(reader["isColor"]))?string.Empty:(System.String)reader["isColor"];
					entity.IsManaged = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.IsManaged)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.IsManaged)];
					//entity.IsManaged = (Convert.IsDBNull(reader["isManaged"]))?string.Empty:(System.String)reader["isManaged"];
					entity.IsNetwork = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.IsNetwork)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.IsNetwork)];
					//entity.IsNetwork = (Convert.IsDBNull(reader["isNetwork"]))?string.Empty:(System.String)reader["isNetwork"];
					entity.DatePurchased = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.DatePurchased)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.DatePurchased)];
					//entity.DatePurchased = (Convert.IsDBNull(reader["DatePurchased"]))?string.Empty:(System.String)reader["DatePurchased"];
					entity.UnderWarranty = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.UnderWarranty)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.UnderWarranty)];
					//entity.UnderWarranty = (Convert.IsDBNull(reader["UnderWarranty"]))?string.Empty:(System.String)reader["UnderWarranty"];
					entity.Contract = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.Contract)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.Contract)];
					//entity.Contract = (Convert.IsDBNull(reader["Contract"]))?string.Empty:(System.String)reader["Contract"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.GapType = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.GapType)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.GapType)];
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
		/// Refreshes the <see cref="METAViewPeripheralRetrieval"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewPeripheralRetrieval"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAViewPeripheralRetrieval entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAViewPeripheralRetrievalColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewPeripheralRetrievalColumn.WorkspaceTypeID)];
			//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
			entity.VCStatusID = (System.Int32)reader[((int)METAViewPeripheralRetrievalColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.Pkid = (System.Int32)reader[((int)METAViewPeripheralRetrievalColumn.Pkid)];
			//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAViewPeripheralRetrievalColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Type = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.Type)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.Type)];
			//entity.Type = (Convert.IsDBNull(reader["Type"]))?string.Empty:(System.String)reader["Type"];
			entity.Name = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.Name)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.Name)];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.Description = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.Description)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.Description)];
			//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
			entity.SeverityRating = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.SeverityRating)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.SeverityRating)];
			//entity.SeverityRating = (Convert.IsDBNull(reader["SeverityRating"]))?string.Empty:(System.String)reader["SeverityRating"];
			entity.Configuration = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.Configuration)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.Configuration)];
			//entity.Configuration = (Convert.IsDBNull(reader["Configuration"]))?string.Empty:(System.String)reader["Configuration"];
			entity.NetworkAddress1 = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.NetworkAddress1)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.NetworkAddress1)];
			//entity.NetworkAddress1 = (Convert.IsDBNull(reader["NetworkAddress1"]))?string.Empty:(System.String)reader["NetworkAddress1"];
			entity.NetworkAddress2 = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.NetworkAddress2)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.NetworkAddress2)];
			//entity.NetworkAddress2 = (Convert.IsDBNull(reader["NetworkAddress2"]))?string.Empty:(System.String)reader["NetworkAddress2"];
			entity.NetworkAddress3 = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.NetworkAddress3)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.NetworkAddress3)];
			//entity.NetworkAddress3 = (Convert.IsDBNull(reader["NetworkAddress3"]))?string.Empty:(System.String)reader["NetworkAddress3"];
			entity.NetworkAddress4 = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.NetworkAddress4)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.NetworkAddress4)];
			//entity.NetworkAddress4 = (Convert.IsDBNull(reader["NetworkAddress4"]))?string.Empty:(System.String)reader["NetworkAddress4"];
			entity.NetworkAddress5 = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.NetworkAddress5)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.NetworkAddress5)];
			//entity.NetworkAddress5 = (Convert.IsDBNull(reader["NetworkAddress5"]))?string.Empty:(System.String)reader["NetworkAddress5"];
			entity.Make = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.Make)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.Make)];
			//entity.Make = (Convert.IsDBNull(reader["Make"]))?string.Empty:(System.String)reader["Make"];
			entity.Model = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.Model)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.Model)];
			//entity.Model = (Convert.IsDBNull(reader["Model"]))?string.Empty:(System.String)reader["Model"];
			entity.SerialNumber = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.SerialNumber)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.SerialNumber)];
			//entity.SerialNumber = (Convert.IsDBNull(reader["SerialNumber"]))?string.Empty:(System.String)reader["SerialNumber"];
			entity.AssetNumber = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.AssetNumber)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.AssetNumber)];
			//entity.AssetNumber = (Convert.IsDBNull(reader["AssetNumber"]))?string.Empty:(System.String)reader["AssetNumber"];
			entity.CopyPPM = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.CopyPPM)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.CopyPPM)];
			//entity.CopyPPM = (Convert.IsDBNull(reader["Copy_PPM"]))?string.Empty:(System.String)reader["Copy_PPM"];
			entity.PrintPPM = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.PrintPPM)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.PrintPPM)];
			//entity.PrintPPM = (Convert.IsDBNull(reader["Print_PPM"]))?string.Empty:(System.String)reader["Print_PPM"];
			entity.IsColor = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.IsColor)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.IsColor)];
			//entity.IsColor = (Convert.IsDBNull(reader["isColor"]))?string.Empty:(System.String)reader["isColor"];
			entity.IsManaged = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.IsManaged)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.IsManaged)];
			//entity.IsManaged = (Convert.IsDBNull(reader["isManaged"]))?string.Empty:(System.String)reader["isManaged"];
			entity.IsNetwork = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.IsNetwork)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.IsNetwork)];
			//entity.IsNetwork = (Convert.IsDBNull(reader["isNetwork"]))?string.Empty:(System.String)reader["isNetwork"];
			entity.DatePurchased = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.DatePurchased)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.DatePurchased)];
			//entity.DatePurchased = (Convert.IsDBNull(reader["DatePurchased"]))?string.Empty:(System.String)reader["DatePurchased"];
			entity.UnderWarranty = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.UnderWarranty)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.UnderWarranty)];
			//entity.UnderWarranty = (Convert.IsDBNull(reader["UnderWarranty"]))?string.Empty:(System.String)reader["UnderWarranty"];
			entity.Contract = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.Contract)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.Contract)];
			//entity.Contract = (Convert.IsDBNull(reader["Contract"]))?string.Empty:(System.String)reader["Contract"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.GapType = (reader.IsDBNull(((int)METAViewPeripheralRetrievalColumn.GapType)))?null:(System.String)reader[((int)METAViewPeripheralRetrievalColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAViewPeripheralRetrieval"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewPeripheralRetrieval"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAViewPeripheralRetrieval entity)
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
			entity.NetworkAddress1 = (Convert.IsDBNull(dataRow["NetworkAddress1"]))?string.Empty:(System.String)dataRow["NetworkAddress1"];
			entity.NetworkAddress2 = (Convert.IsDBNull(dataRow["NetworkAddress2"]))?string.Empty:(System.String)dataRow["NetworkAddress2"];
			entity.NetworkAddress3 = (Convert.IsDBNull(dataRow["NetworkAddress3"]))?string.Empty:(System.String)dataRow["NetworkAddress3"];
			entity.NetworkAddress4 = (Convert.IsDBNull(dataRow["NetworkAddress4"]))?string.Empty:(System.String)dataRow["NetworkAddress4"];
			entity.NetworkAddress5 = (Convert.IsDBNull(dataRow["NetworkAddress5"]))?string.Empty:(System.String)dataRow["NetworkAddress5"];
			entity.Make = (Convert.IsDBNull(dataRow["Make"]))?string.Empty:(System.String)dataRow["Make"];
			entity.Model = (Convert.IsDBNull(dataRow["Model"]))?string.Empty:(System.String)dataRow["Model"];
			entity.SerialNumber = (Convert.IsDBNull(dataRow["SerialNumber"]))?string.Empty:(System.String)dataRow["SerialNumber"];
			entity.AssetNumber = (Convert.IsDBNull(dataRow["AssetNumber"]))?string.Empty:(System.String)dataRow["AssetNumber"];
			entity.CopyPPM = (Convert.IsDBNull(dataRow["Copy_PPM"]))?string.Empty:(System.String)dataRow["Copy_PPM"];
			entity.PrintPPM = (Convert.IsDBNull(dataRow["Print_PPM"]))?string.Empty:(System.String)dataRow["Print_PPM"];
			entity.IsColor = (Convert.IsDBNull(dataRow["isColor"]))?string.Empty:(System.String)dataRow["isColor"];
			entity.IsManaged = (Convert.IsDBNull(dataRow["isManaged"]))?string.Empty:(System.String)dataRow["isManaged"];
			entity.IsNetwork = (Convert.IsDBNull(dataRow["isNetwork"]))?string.Empty:(System.String)dataRow["isNetwork"];
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

	#region METAViewPeripheralRetrievalFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewPeripheralRetrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewPeripheralRetrievalFilterBuilder : SqlFilterBuilder<METAViewPeripheralRetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewPeripheralRetrievalFilterBuilder class.
		/// </summary>
		public METAViewPeripheralRetrievalFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewPeripheralRetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewPeripheralRetrievalFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewPeripheralRetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewPeripheralRetrievalFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewPeripheralRetrievalFilterBuilder

	#region METAViewPeripheralRetrievalParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewPeripheralRetrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewPeripheralRetrievalParameterBuilder : ParameterizedSqlFilterBuilder<METAViewPeripheralRetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewPeripheralRetrievalParameterBuilder class.
		/// </summary>
		public METAViewPeripheralRetrievalParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewPeripheralRetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewPeripheralRetrievalParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewPeripheralRetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewPeripheralRetrievalParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewPeripheralRetrievalParameterBuilder
	
	#region METAViewPeripheralRetrievalSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewPeripheralRetrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAViewPeripheralRetrievalSortBuilder : SqlSortBuilder<METAViewPeripheralRetrievalColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewPeripheralRetrievalSqlSortBuilder class.
		/// </summary>
		public METAViewPeripheralRetrievalSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAViewPeripheralRetrievalSortBuilder

} // end namespace
