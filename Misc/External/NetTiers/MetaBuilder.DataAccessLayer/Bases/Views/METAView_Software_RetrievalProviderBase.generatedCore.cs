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
	/// This class is the base class for any <see cref="METAView_Software_RetrievalProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAView_Software_RetrievalProviderBaseCore : EntityViewProviderBase<METAView_Software_Retrieval>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAView_Software_Retrieval&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAView_Software_Retrieval&gt;"/></returns>
		protected static VList&lt;METAView_Software_Retrieval&gt; Fill(DataSet dataSet, VList<METAView_Software_Retrieval> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAView_Software_Retrieval>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAView_Software_Retrieval&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAView_Software_Retrieval>"/></returns>
		protected static VList&lt;METAView_Software_Retrieval&gt; Fill(DataTable dataTable, VList<METAView_Software_Retrieval> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAView_Software_Retrieval c = new METAView_Software_Retrieval();
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
					c.Copyright = (Convert.IsDBNull(row["Copyright"]))?string.Empty:(System.String)row["Copyright"];
					c.Publisher = (Convert.IsDBNull(row["Publisher"]))?string.Empty:(System.String)row["Publisher"];
					c.InternalName = (Convert.IsDBNull(row["InternalName"]))?string.Empty:(System.String)row["InternalName"];
					c.Language = (Convert.IsDBNull(row["Language"]))?string.Empty:(System.String)row["Language"];
					c.DateCreated = (Convert.IsDBNull(row["DateCreated"]))?string.Empty:(System.String)row["DateCreated"];
					c.isDNS = (Convert.IsDBNull(row["isDNS"]))?string.Empty:(System.String)row["isDNS"];
					c.isDHCP = (Convert.IsDBNull(row["isDHCP"]))?string.Empty:(System.String)row["isDHCP"];
					c.isLicensed = (Convert.IsDBNull(row["isLicensed"]))?string.Empty:(System.String)row["isLicensed"];
					c.LicenseNumber = (Convert.IsDBNull(row["LicenseNumber"]))?string.Empty:(System.String)row["LicenseNumber"];
					c.DatePurchased = (Convert.IsDBNull(row["DatePurchased"]))?string.Empty:(System.String)row["DatePurchased"];
					c.Version = (Convert.IsDBNull(row["Version"]))?string.Empty:(System.String)row["Version"];
					c.ID = (Convert.IsDBNull(row["ID"]))?string.Empty:(System.String)row["ID"];
					c.CustomField1 = (Convert.IsDBNull(row["CustomField1"]))?string.Empty:(System.String)row["CustomField1"];
					c.CustomField2 = (Convert.IsDBNull(row["CustomField2"]))?string.Empty:(System.String)row["CustomField2"];
					c.CustomField3 = (Convert.IsDBNull(row["CustomField3"]))?string.Empty:(System.String)row["CustomField3"];
					c.UserInterface = (Convert.IsDBNull(row["UserInterface"]))?string.Empty:(System.String)row["UserInterface"];
					c.GapType = (Convert.IsDBNull(row["GapType"]))?string.Empty:(System.String)row["GapType"];
					c.AbbreviatedName = (Convert.IsDBNull(row["AbbreviatedName"]))?string.Empty:(System.String)row["AbbreviatedName"];
					c.Release = (Convert.IsDBNull(row["Release"]))?string.Empty:(System.String)row["Release"];
					c.Edition = (Convert.IsDBNull(row["Edition"]))?string.Empty:(System.String)row["Edition"];
					c.ServicePack = (Convert.IsDBNull(row["ServicePack"]))?string.Empty:(System.String)row["ServicePack"];
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
		/// Fill an <see cref="VList&lt;METAView_Software_Retrieval&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAView_Software_Retrieval&gt;"/></returns>
		protected VList<METAView_Software_Retrieval> Fill(IDataReader reader, VList<METAView_Software_Retrieval> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAView_Software_Retrieval entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAView_Software_Retrieval>("METAView_Software_Retrieval",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAView_Software_Retrieval();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAView_Software_RetrievalColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_Software_RetrievalColumn.WorkspaceTypeId)];
					//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
					entity.VCStatusID = (System.Int32)reader[((int)METAView_Software_RetrievalColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.pkid = (System.Int32)reader[((int)METAView_Software_RetrievalColumn.pkid)];
					//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAView_Software_RetrievalColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Type = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.Type)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.Type)];
					//entity.Type = (Convert.IsDBNull(reader["Type"]))?string.Empty:(System.String)reader["Type"];
					entity.Name = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.Name)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.Name)];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.Description = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.Description)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.Description)];
					//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
					entity.SeverityRating = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.SeverityRating)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.SeverityRating)];
					//entity.SeverityRating = (Convert.IsDBNull(reader["SeverityRating"]))?string.Empty:(System.String)reader["SeverityRating"];
					entity.Configuration = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.Configuration)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.Configuration)];
					//entity.Configuration = (Convert.IsDBNull(reader["Configuration"]))?string.Empty:(System.String)reader["Configuration"];
					entity.Copyright = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.Copyright)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.Copyright)];
					//entity.Copyright = (Convert.IsDBNull(reader["Copyright"]))?string.Empty:(System.String)reader["Copyright"];
					entity.Publisher = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.Publisher)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.Publisher)];
					//entity.Publisher = (Convert.IsDBNull(reader["Publisher"]))?string.Empty:(System.String)reader["Publisher"];
					entity.InternalName = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.InternalName)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.InternalName)];
					//entity.InternalName = (Convert.IsDBNull(reader["InternalName"]))?string.Empty:(System.String)reader["InternalName"];
					entity.Language = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.Language)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.Language)];
					//entity.Language = (Convert.IsDBNull(reader["Language"]))?string.Empty:(System.String)reader["Language"];
					entity.DateCreated = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.DateCreated)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.DateCreated)];
					//entity.DateCreated = (Convert.IsDBNull(reader["DateCreated"]))?string.Empty:(System.String)reader["DateCreated"];
					entity.isDNS = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.isDNS)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.isDNS)];
					//entity.isDNS = (Convert.IsDBNull(reader["isDNS"]))?string.Empty:(System.String)reader["isDNS"];
					entity.isDHCP = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.isDHCP)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.isDHCP)];
					//entity.isDHCP = (Convert.IsDBNull(reader["isDHCP"]))?string.Empty:(System.String)reader["isDHCP"];
					entity.isLicensed = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.isLicensed)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.isLicensed)];
					//entity.isLicensed = (Convert.IsDBNull(reader["isLicensed"]))?string.Empty:(System.String)reader["isLicensed"];
					entity.LicenseNumber = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.LicenseNumber)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.LicenseNumber)];
					//entity.LicenseNumber = (Convert.IsDBNull(reader["LicenseNumber"]))?string.Empty:(System.String)reader["LicenseNumber"];
					entity.DatePurchased = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.DatePurchased)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.DatePurchased)];
					//entity.DatePurchased = (Convert.IsDBNull(reader["DatePurchased"]))?string.Empty:(System.String)reader["DatePurchased"];
					entity.Version = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.Version)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.Version)];
					//entity.Version = (Convert.IsDBNull(reader["Version"]))?string.Empty:(System.String)reader["Version"];
					entity.ID = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.ID)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.ID)];
					//entity.ID = (Convert.IsDBNull(reader["ID"]))?string.Empty:(System.String)reader["ID"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.UserInterface = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.UserInterface)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.UserInterface)];
					//entity.UserInterface = (Convert.IsDBNull(reader["UserInterface"]))?string.Empty:(System.String)reader["UserInterface"];
					entity.GapType = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.GapType)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.GapType)];
					//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
					entity.AbbreviatedName = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.AbbreviatedName)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.AbbreviatedName)];
					//entity.AbbreviatedName = (Convert.IsDBNull(reader["AbbreviatedName"]))?string.Empty:(System.String)reader["AbbreviatedName"];
					entity.Release = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.Release)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.Release)];
					//entity.Release = (Convert.IsDBNull(reader["Release"]))?string.Empty:(System.String)reader["Release"];
					entity.Edition = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.Edition)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.Edition)];
					//entity.Edition = (Convert.IsDBNull(reader["Edition"]))?string.Empty:(System.String)reader["Edition"];
					entity.ServicePack = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.ServicePack)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.ServicePack)];
					//entity.ServicePack = (Convert.IsDBNull(reader["ServicePack"]))?string.Empty:(System.String)reader["ServicePack"];
					entity.Abbreviation = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.Abbreviation)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.Abbreviation)];
					//entity.Abbreviation = (Convert.IsDBNull(reader["Abbreviation"]))?string.Empty:(System.String)reader["Abbreviation"];
					entity.IsBusinessExternal = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.IsBusinessExternal)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.IsBusinessExternal)];
					//entity.IsBusinessExternal = (Convert.IsDBNull(reader["IsBusinessExternal"]))?string.Empty:(System.String)reader["IsBusinessExternal"];
					entity.StandardisationStatus = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.StandardisationStatus)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.StandardisationStatus)];
					//entity.StandardisationStatus = (Convert.IsDBNull(reader["StandardisationStatus"]))?string.Empty:(System.String)reader["StandardisationStatus"];
					entity.StandardisationStatusDate = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.StandardisationStatusDate)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.StandardisationStatusDate)];
					//entity.StandardisationStatusDate = (Convert.IsDBNull(reader["StandardisationStatusDate"]))?string.Empty:(System.String)reader["StandardisationStatusDate"];
					entity.DataSourceID = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.DataSourceID)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.DataSourceID)];
					//entity.DataSourceID = (Convert.IsDBNull(reader["DataSourceID"]))?string.Empty:(System.String)reader["DataSourceID"];
					entity.DataSourceName = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.DataSourceName)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.DataSourceName)];
					//entity.DataSourceName = (Convert.IsDBNull(reader["DataSourceName"]))?string.Empty:(System.String)reader["DataSourceName"];
					entity.GeneralRemarks = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.GeneralRemarks)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.GeneralRemarks)];
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
		/// Refreshes the <see cref="METAView_Software_Retrieval"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_Software_Retrieval"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAView_Software_Retrieval entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAView_Software_RetrievalColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_Software_RetrievalColumn.WorkspaceTypeId)];
			//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
			entity.VCStatusID = (System.Int32)reader[((int)METAView_Software_RetrievalColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.pkid = (System.Int32)reader[((int)METAView_Software_RetrievalColumn.pkid)];
			//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAView_Software_RetrievalColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Type = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.Type)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.Type)];
			//entity.Type = (Convert.IsDBNull(reader["Type"]))?string.Empty:(System.String)reader["Type"];
			entity.Name = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.Name)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.Name)];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.Description = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.Description)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.Description)];
			//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
			entity.SeverityRating = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.SeverityRating)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.SeverityRating)];
			//entity.SeverityRating = (Convert.IsDBNull(reader["SeverityRating"]))?string.Empty:(System.String)reader["SeverityRating"];
			entity.Configuration = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.Configuration)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.Configuration)];
			//entity.Configuration = (Convert.IsDBNull(reader["Configuration"]))?string.Empty:(System.String)reader["Configuration"];
			entity.Copyright = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.Copyright)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.Copyright)];
			//entity.Copyright = (Convert.IsDBNull(reader["Copyright"]))?string.Empty:(System.String)reader["Copyright"];
			entity.Publisher = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.Publisher)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.Publisher)];
			//entity.Publisher = (Convert.IsDBNull(reader["Publisher"]))?string.Empty:(System.String)reader["Publisher"];
			entity.InternalName = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.InternalName)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.InternalName)];
			//entity.InternalName = (Convert.IsDBNull(reader["InternalName"]))?string.Empty:(System.String)reader["InternalName"];
			entity.Language = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.Language)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.Language)];
			//entity.Language = (Convert.IsDBNull(reader["Language"]))?string.Empty:(System.String)reader["Language"];
			entity.DateCreated = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.DateCreated)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.DateCreated)];
			//entity.DateCreated = (Convert.IsDBNull(reader["DateCreated"]))?string.Empty:(System.String)reader["DateCreated"];
			entity.isDNS = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.isDNS)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.isDNS)];
			//entity.isDNS = (Convert.IsDBNull(reader["isDNS"]))?string.Empty:(System.String)reader["isDNS"];
			entity.isDHCP = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.isDHCP)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.isDHCP)];
			//entity.isDHCP = (Convert.IsDBNull(reader["isDHCP"]))?string.Empty:(System.String)reader["isDHCP"];
			entity.isLicensed = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.isLicensed)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.isLicensed)];
			//entity.isLicensed = (Convert.IsDBNull(reader["isLicensed"]))?string.Empty:(System.String)reader["isLicensed"];
			entity.LicenseNumber = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.LicenseNumber)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.LicenseNumber)];
			//entity.LicenseNumber = (Convert.IsDBNull(reader["LicenseNumber"]))?string.Empty:(System.String)reader["LicenseNumber"];
			entity.DatePurchased = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.DatePurchased)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.DatePurchased)];
			//entity.DatePurchased = (Convert.IsDBNull(reader["DatePurchased"]))?string.Empty:(System.String)reader["DatePurchased"];
			entity.Version = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.Version)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.Version)];
			//entity.Version = (Convert.IsDBNull(reader["Version"]))?string.Empty:(System.String)reader["Version"];
			entity.ID = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.ID)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.ID)];
			//entity.ID = (Convert.IsDBNull(reader["ID"]))?string.Empty:(System.String)reader["ID"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.UserInterface = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.UserInterface)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.UserInterface)];
			//entity.UserInterface = (Convert.IsDBNull(reader["UserInterface"]))?string.Empty:(System.String)reader["UserInterface"];
			entity.GapType = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.GapType)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			entity.AbbreviatedName = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.AbbreviatedName)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.AbbreviatedName)];
			//entity.AbbreviatedName = (Convert.IsDBNull(reader["AbbreviatedName"]))?string.Empty:(System.String)reader["AbbreviatedName"];
			entity.Release = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.Release)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.Release)];
			//entity.Release = (Convert.IsDBNull(reader["Release"]))?string.Empty:(System.String)reader["Release"];
			entity.Edition = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.Edition)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.Edition)];
			//entity.Edition = (Convert.IsDBNull(reader["Edition"]))?string.Empty:(System.String)reader["Edition"];
			entity.ServicePack = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.ServicePack)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.ServicePack)];
			//entity.ServicePack = (Convert.IsDBNull(reader["ServicePack"]))?string.Empty:(System.String)reader["ServicePack"];
			entity.Abbreviation = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.Abbreviation)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.Abbreviation)];
			//entity.Abbreviation = (Convert.IsDBNull(reader["Abbreviation"]))?string.Empty:(System.String)reader["Abbreviation"];
			entity.IsBusinessExternal = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.IsBusinessExternal)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.IsBusinessExternal)];
			//entity.IsBusinessExternal = (Convert.IsDBNull(reader["IsBusinessExternal"]))?string.Empty:(System.String)reader["IsBusinessExternal"];
			entity.StandardisationStatus = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.StandardisationStatus)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.StandardisationStatus)];
			//entity.StandardisationStatus = (Convert.IsDBNull(reader["StandardisationStatus"]))?string.Empty:(System.String)reader["StandardisationStatus"];
			entity.StandardisationStatusDate = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.StandardisationStatusDate)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.StandardisationStatusDate)];
			//entity.StandardisationStatusDate = (Convert.IsDBNull(reader["StandardisationStatusDate"]))?string.Empty:(System.String)reader["StandardisationStatusDate"];
			entity.DataSourceID = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.DataSourceID)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.DataSourceID)];
			//entity.DataSourceID = (Convert.IsDBNull(reader["DataSourceID"]))?string.Empty:(System.String)reader["DataSourceID"];
			entity.DataSourceName = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.DataSourceName)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.DataSourceName)];
			//entity.DataSourceName = (Convert.IsDBNull(reader["DataSourceName"]))?string.Empty:(System.String)reader["DataSourceName"];
			entity.GeneralRemarks = (reader.IsDBNull(((int)METAView_Software_RetrievalColumn.GeneralRemarks)))?null:(System.String)reader[((int)METAView_Software_RetrievalColumn.GeneralRemarks)];
			//entity.GeneralRemarks = (Convert.IsDBNull(reader["GeneralRemarks"]))?string.Empty:(System.String)reader["GeneralRemarks"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAView_Software_Retrieval"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_Software_Retrieval"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAView_Software_Retrieval entity)
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
			entity.Copyright = (Convert.IsDBNull(dataRow["Copyright"]))?string.Empty:(System.String)dataRow["Copyright"];
			entity.Publisher = (Convert.IsDBNull(dataRow["Publisher"]))?string.Empty:(System.String)dataRow["Publisher"];
			entity.InternalName = (Convert.IsDBNull(dataRow["InternalName"]))?string.Empty:(System.String)dataRow["InternalName"];
			entity.Language = (Convert.IsDBNull(dataRow["Language"]))?string.Empty:(System.String)dataRow["Language"];
			entity.DateCreated = (Convert.IsDBNull(dataRow["DateCreated"]))?string.Empty:(System.String)dataRow["DateCreated"];
			entity.isDNS = (Convert.IsDBNull(dataRow["isDNS"]))?string.Empty:(System.String)dataRow["isDNS"];
			entity.isDHCP = (Convert.IsDBNull(dataRow["isDHCP"]))?string.Empty:(System.String)dataRow["isDHCP"];
			entity.isLicensed = (Convert.IsDBNull(dataRow["isLicensed"]))?string.Empty:(System.String)dataRow["isLicensed"];
			entity.LicenseNumber = (Convert.IsDBNull(dataRow["LicenseNumber"]))?string.Empty:(System.String)dataRow["LicenseNumber"];
			entity.DatePurchased = (Convert.IsDBNull(dataRow["DatePurchased"]))?string.Empty:(System.String)dataRow["DatePurchased"];
			entity.Version = (Convert.IsDBNull(dataRow["Version"]))?string.Empty:(System.String)dataRow["Version"];
			entity.ID = (Convert.IsDBNull(dataRow["ID"]))?string.Empty:(System.String)dataRow["ID"];
			entity.CustomField1 = (Convert.IsDBNull(dataRow["CustomField1"]))?string.Empty:(System.String)dataRow["CustomField1"];
			entity.CustomField2 = (Convert.IsDBNull(dataRow["CustomField2"]))?string.Empty:(System.String)dataRow["CustomField2"];
			entity.CustomField3 = (Convert.IsDBNull(dataRow["CustomField3"]))?string.Empty:(System.String)dataRow["CustomField3"];
			entity.UserInterface = (Convert.IsDBNull(dataRow["UserInterface"]))?string.Empty:(System.String)dataRow["UserInterface"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.AbbreviatedName = (Convert.IsDBNull(dataRow["AbbreviatedName"]))?string.Empty:(System.String)dataRow["AbbreviatedName"];
			entity.Release = (Convert.IsDBNull(dataRow["Release"]))?string.Empty:(System.String)dataRow["Release"];
			entity.Edition = (Convert.IsDBNull(dataRow["Edition"]))?string.Empty:(System.String)dataRow["Edition"];
			entity.ServicePack = (Convert.IsDBNull(dataRow["ServicePack"]))?string.Empty:(System.String)dataRow["ServicePack"];
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

	#region METAView_Software_RetrievalFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_Software_Retrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_Software_RetrievalFilterBuilder : SqlFilterBuilder<METAView_Software_RetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_Software_RetrievalFilterBuilder class.
		/// </summary>
		public METAView_Software_RetrievalFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_Software_RetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_Software_RetrievalFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_Software_RetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_Software_RetrievalFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_Software_RetrievalFilterBuilder

	#region METAView_Software_RetrievalParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_Software_Retrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_Software_RetrievalParameterBuilder : ParameterizedSqlFilterBuilder<METAView_Software_RetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_Software_RetrievalParameterBuilder class.
		/// </summary>
		public METAView_Software_RetrievalParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_Software_RetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_Software_RetrievalParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_Software_RetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_Software_RetrievalParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_Software_RetrievalParameterBuilder
	
	#region METAView_Software_RetrievalSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_Software_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Software_RetrievalSortBuilder : SqlSortBuilder<METAView_Software_RetrievalColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_Software_RetrievalSqlSortBuilder class.
		/// </summary>
		public METAView_Software_RetrievalSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAView_Software_RetrievalSortBuilder

} // end namespace
