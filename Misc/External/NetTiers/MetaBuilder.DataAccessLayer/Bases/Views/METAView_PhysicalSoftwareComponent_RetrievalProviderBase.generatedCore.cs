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
	/// This class is the base class for any <see cref="METAView_PhysicalSoftwareComponent_RetrievalProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAView_PhysicalSoftwareComponent_RetrievalProviderBaseCore : EntityViewProviderBase<METAView_PhysicalSoftwareComponent_Retrieval>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAView_PhysicalSoftwareComponent_Retrieval&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAView_PhysicalSoftwareComponent_Retrieval&gt;"/></returns>
		protected static VList&lt;METAView_PhysicalSoftwareComponent_Retrieval&gt; Fill(DataSet dataSet, VList<METAView_PhysicalSoftwareComponent_Retrieval> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAView_PhysicalSoftwareComponent_Retrieval>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAView_PhysicalSoftwareComponent_Retrieval&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAView_PhysicalSoftwareComponent_Retrieval>"/></returns>
		protected static VList&lt;METAView_PhysicalSoftwareComponent_Retrieval&gt; Fill(DataTable dataTable, VList<METAView_PhysicalSoftwareComponent_Retrieval> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAView_PhysicalSoftwareComponent_Retrieval c = new METAView_PhysicalSoftwareComponent_Retrieval();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeId = (Convert.IsDBNull(row["WorkspaceTypeId"]))?(int)0:(System.Int32?)row["WorkspaceTypeId"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.InternalName = (Convert.IsDBNull(row["InternalName"]))?string.Empty:(System.String)row["InternalName"];
					c.ConfigurationID = (Convert.IsDBNull(row["ConfigurationID"]))?string.Empty:(System.String)row["ConfigurationID"];
					c.SoftwareType = (Convert.IsDBNull(row["SoftwareType"]))?string.Empty:(System.String)row["SoftwareType"];
					c.SoftwareLevel = (Convert.IsDBNull(row["SoftwareLevel"]))?string.Empty:(System.String)row["SoftwareLevel"];
					c.IsBespoke = (Convert.IsDBNull(row["IsBespoke"]))?string.Empty:(System.String)row["IsBespoke"];
					c.UserInterfaceType = (Convert.IsDBNull(row["UserInterfaceType"]))?string.Empty:(System.String)row["UserInterfaceType"];
					c.NumberofUsers = (Convert.IsDBNull(row["NumberofUsers"]))?string.Empty:(System.String)row["NumberofUsers"];
					c.SeverityRating = (Convert.IsDBNull(row["SeverityRating"]))?string.Empty:(System.String)row["SeverityRating"];
					c.Edition = (Convert.IsDBNull(row["Edition"]))?string.Empty:(System.String)row["Edition"];
					c.Release = (Convert.IsDBNull(row["Release"]))?string.Empty:(System.String)row["Release"];
					c.ServicePackID = (Convert.IsDBNull(row["ServicePackID"]))?string.Empty:(System.String)row["ServicePackID"];
					c.VersionNumber = (Convert.IsDBNull(row["VersionNumber"]))?string.Empty:(System.String)row["VersionNumber"];
					c.ID = (Convert.IsDBNull(row["ID"]))?string.Empty:(System.String)row["ID"];
					c.PublisherName = (Convert.IsDBNull(row["PublisherName"]))?string.Empty:(System.String)row["PublisherName"];
					c.Language = (Convert.IsDBNull(row["Language"]))?string.Empty:(System.String)row["Language"];
					c.DateCreated = (Convert.IsDBNull(row["DateCreated"]))?string.Empty:(System.String)row["DateCreated"];
					c.DatePurchased = (Convert.IsDBNull(row["DatePurchased"]))?string.Empty:(System.String)row["DatePurchased"];
					c.IsDNS = (Convert.IsDBNull(row["IsDNS"]))?string.Empty:(System.String)row["IsDNS"];
					c.IsDHCP = (Convert.IsDBNull(row["IsDHCP"]))?string.Empty:(System.String)row["IsDHCP"];
					c.isLicensed = (Convert.IsDBNull(row["isLicensed"]))?string.Empty:(System.String)row["isLicensed"];
					c.LicenseNumber = (Convert.IsDBNull(row["LicenseNumber"]))?string.Empty:(System.String)row["LicenseNumber"];
					c.HasCopyright = (Convert.IsDBNull(row["HasCopyright"]))?string.Empty:(System.String)row["HasCopyright"];
					c.Configuration = (Convert.IsDBNull(row["Configuration"]))?string.Empty:(System.String)row["Configuration"];
					c.Type = (Convert.IsDBNull(row["Type"]))?string.Empty:(System.String)row["Type"];
					c.UserInterface = (Convert.IsDBNull(row["UserInterface"]))?string.Empty:(System.String)row["UserInterface"];
					c.ServicePack = (Convert.IsDBNull(row["ServicePack"]))?string.Empty:(System.String)row["ServicePack"];
					c.Version = (Convert.IsDBNull(row["Version"]))?string.Empty:(System.String)row["Version"];
					c.Publisher = (Convert.IsDBNull(row["Publisher"]))?string.Empty:(System.String)row["Publisher"];
					c.Copyright = (Convert.IsDBNull(row["Copyright"]))?string.Empty:(System.String)row["Copyright"];
					c.Name = (Convert.IsDBNull(row["Name"]))?string.Empty:(System.String)row["Name"];
					c.Description = (Convert.IsDBNull(row["Description"]))?string.Empty:(System.String)row["Description"];
					c.Abbreviation = (Convert.IsDBNull(row["Abbreviation"]))?string.Empty:(System.String)row["Abbreviation"];
					c.IsBusinessExternal = (Convert.IsDBNull(row["IsBusinessExternal"]))?string.Empty:(System.String)row["IsBusinessExternal"];
					c.StandardisationStatus = (Convert.IsDBNull(row["StandardisationStatus"]))?string.Empty:(System.String)row["StandardisationStatus"];
					c.StandardisationStatusDate = (Convert.IsDBNull(row["StandardisationStatusDate"]))?string.Empty:(System.String)row["StandardisationStatusDate"];
					c.DataSourceID = (Convert.IsDBNull(row["DataSourceID"]))?string.Empty:(System.String)row["DataSourceID"];
					c.DataSourceName = (Convert.IsDBNull(row["DataSourceName"]))?string.Empty:(System.String)row["DataSourceName"];
					c.GeneralRemarks = (Convert.IsDBNull(row["GeneralRemarks"]))?string.Empty:(System.String)row["GeneralRemarks"];
					c.GapType = (Convert.IsDBNull(row["GapType"]))?string.Empty:(System.String)row["GapType"];
					c.CustomField1 = (Convert.IsDBNull(row["CustomField1"]))?string.Empty:(System.String)row["CustomField1"];
					c.CustomField2 = (Convert.IsDBNull(row["CustomField2"]))?string.Empty:(System.String)row["CustomField2"];
					c.CustomField3 = (Convert.IsDBNull(row["CustomField3"]))?string.Empty:(System.String)row["CustomField3"];
					c.ArchitectureStatus = (Convert.IsDBNull(row["ArchitectureStatus"]))?string.Empty:(System.String)row["ArchitectureStatus"];
					c.ArchitectureStatusDate = (Convert.IsDBNull(row["ArchitectureStatusDate"]))?string.Empty:(System.String)row["ArchitectureStatusDate"];
					c.DesignRationale = (Convert.IsDBNull(row["DesignRationale"]))?string.Empty:(System.String)row["DesignRationale"];
					c.IsLicenced = (Convert.IsDBNull(row["IsLicenced"]))?string.Empty:(System.String)row["IsLicenced"];
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
		/// Fill an <see cref="VList&lt;METAView_PhysicalSoftwareComponent_Retrieval&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAView_PhysicalSoftwareComponent_Retrieval&gt;"/></returns>
		protected VList<METAView_PhysicalSoftwareComponent_Retrieval> Fill(IDataReader reader, VList<METAView_PhysicalSoftwareComponent_Retrieval> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAView_PhysicalSoftwareComponent_Retrieval entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAView_PhysicalSoftwareComponent_Retrieval>("METAView_PhysicalSoftwareComponent_Retrieval",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAView_PhysicalSoftwareComponent_Retrieval();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.WorkspaceTypeId)];
					//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
					entity.VCStatusID = (System.Int32)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.pkid = (System.Int32)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.pkid)];
					//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.InternalName = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.InternalName)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.InternalName)];
					//entity.InternalName = (Convert.IsDBNull(reader["InternalName"]))?string.Empty:(System.String)reader["InternalName"];
					entity.ConfigurationID = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.ConfigurationID)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.ConfigurationID)];
					//entity.ConfigurationID = (Convert.IsDBNull(reader["ConfigurationID"]))?string.Empty:(System.String)reader["ConfigurationID"];
					entity.SoftwareType = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.SoftwareType)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.SoftwareType)];
					//entity.SoftwareType = (Convert.IsDBNull(reader["SoftwareType"]))?string.Empty:(System.String)reader["SoftwareType"];
					entity.SoftwareLevel = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.SoftwareLevel)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.SoftwareLevel)];
					//entity.SoftwareLevel = (Convert.IsDBNull(reader["SoftwareLevel"]))?string.Empty:(System.String)reader["SoftwareLevel"];
					entity.IsBespoke = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.IsBespoke)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.IsBespoke)];
					//entity.IsBespoke = (Convert.IsDBNull(reader["IsBespoke"]))?string.Empty:(System.String)reader["IsBespoke"];
					entity.UserInterfaceType = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.UserInterfaceType)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.UserInterfaceType)];
					//entity.UserInterfaceType = (Convert.IsDBNull(reader["UserInterfaceType"]))?string.Empty:(System.String)reader["UserInterfaceType"];
					entity.NumberofUsers = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.NumberofUsers)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.NumberofUsers)];
					//entity.NumberofUsers = (Convert.IsDBNull(reader["NumberofUsers"]))?string.Empty:(System.String)reader["NumberofUsers"];
					entity.SeverityRating = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.SeverityRating)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.SeverityRating)];
					//entity.SeverityRating = (Convert.IsDBNull(reader["SeverityRating"]))?string.Empty:(System.String)reader["SeverityRating"];
					entity.Edition = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Edition)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Edition)];
					//entity.Edition = (Convert.IsDBNull(reader["Edition"]))?string.Empty:(System.String)reader["Edition"];
					entity.Release = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Release)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Release)];
					//entity.Release = (Convert.IsDBNull(reader["Release"]))?string.Empty:(System.String)reader["Release"];
					entity.ServicePackID = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.ServicePackID)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.ServicePackID)];
					//entity.ServicePackID = (Convert.IsDBNull(reader["ServicePackID"]))?string.Empty:(System.String)reader["ServicePackID"];
					entity.VersionNumber = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.VersionNumber)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.VersionNumber)];
					//entity.VersionNumber = (Convert.IsDBNull(reader["VersionNumber"]))?string.Empty:(System.String)reader["VersionNumber"];
					entity.ID = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.ID)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.ID)];
					//entity.ID = (Convert.IsDBNull(reader["ID"]))?string.Empty:(System.String)reader["ID"];
					entity.PublisherName = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.PublisherName)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.PublisherName)];
					//entity.PublisherName = (Convert.IsDBNull(reader["PublisherName"]))?string.Empty:(System.String)reader["PublisherName"];
					entity.Language = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Language)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Language)];
					//entity.Language = (Convert.IsDBNull(reader["Language"]))?string.Empty:(System.String)reader["Language"];
					entity.DateCreated = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.DateCreated)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.DateCreated)];
					//entity.DateCreated = (Convert.IsDBNull(reader["DateCreated"]))?string.Empty:(System.String)reader["DateCreated"];
					entity.DatePurchased = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.DatePurchased)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.DatePurchased)];
					//entity.DatePurchased = (Convert.IsDBNull(reader["DatePurchased"]))?string.Empty:(System.String)reader["DatePurchased"];
					entity.IsDNS = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.IsDNS)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.IsDNS)];
					//entity.IsDNS = (Convert.IsDBNull(reader["IsDNS"]))?string.Empty:(System.String)reader["IsDNS"];
					entity.IsDHCP = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.IsDHCP)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.IsDHCP)];
					//entity.IsDHCP = (Convert.IsDBNull(reader["IsDHCP"]))?string.Empty:(System.String)reader["IsDHCP"];
					entity.isLicensed = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.isLicensed)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.isLicensed)];
					//entity.isLicensed = (Convert.IsDBNull(reader["isLicensed"]))?string.Empty:(System.String)reader["isLicensed"];
					entity.LicenseNumber = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.LicenseNumber)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.LicenseNumber)];
					//entity.LicenseNumber = (Convert.IsDBNull(reader["LicenseNumber"]))?string.Empty:(System.String)reader["LicenseNumber"];
					entity.HasCopyright = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.HasCopyright)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.HasCopyright)];
					//entity.HasCopyright = (Convert.IsDBNull(reader["HasCopyright"]))?string.Empty:(System.String)reader["HasCopyright"];
					entity.Configuration = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Configuration)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Configuration)];
					//entity.Configuration = (Convert.IsDBNull(reader["Configuration"]))?string.Empty:(System.String)reader["Configuration"];
					entity.Type = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Type)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Type)];
					//entity.Type = (Convert.IsDBNull(reader["Type"]))?string.Empty:(System.String)reader["Type"];
					entity.UserInterface = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.UserInterface)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.UserInterface)];
					//entity.UserInterface = (Convert.IsDBNull(reader["UserInterface"]))?string.Empty:(System.String)reader["UserInterface"];
					entity.ServicePack = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.ServicePack)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.ServicePack)];
					//entity.ServicePack = (Convert.IsDBNull(reader["ServicePack"]))?string.Empty:(System.String)reader["ServicePack"];
					entity.Version = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Version)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Version)];
					//entity.Version = (Convert.IsDBNull(reader["Version"]))?string.Empty:(System.String)reader["Version"];
					entity.Publisher = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Publisher)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Publisher)];
					//entity.Publisher = (Convert.IsDBNull(reader["Publisher"]))?string.Empty:(System.String)reader["Publisher"];
					entity.Copyright = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Copyright)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Copyright)];
					//entity.Copyright = (Convert.IsDBNull(reader["Copyright"]))?string.Empty:(System.String)reader["Copyright"];
					entity.Name = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Name)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Name)];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.Description = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Description)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Description)];
					//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
					entity.Abbreviation = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Abbreviation)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Abbreviation)];
					//entity.Abbreviation = (Convert.IsDBNull(reader["Abbreviation"]))?string.Empty:(System.String)reader["Abbreviation"];
					entity.IsBusinessExternal = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.IsBusinessExternal)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.IsBusinessExternal)];
					//entity.IsBusinessExternal = (Convert.IsDBNull(reader["IsBusinessExternal"]))?string.Empty:(System.String)reader["IsBusinessExternal"];
					entity.StandardisationStatus = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.StandardisationStatus)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.StandardisationStatus)];
					//entity.StandardisationStatus = (Convert.IsDBNull(reader["StandardisationStatus"]))?string.Empty:(System.String)reader["StandardisationStatus"];
					entity.StandardisationStatusDate = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.StandardisationStatusDate)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.StandardisationStatusDate)];
					//entity.StandardisationStatusDate = (Convert.IsDBNull(reader["StandardisationStatusDate"]))?string.Empty:(System.String)reader["StandardisationStatusDate"];
					entity.DataSourceID = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.DataSourceID)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.DataSourceID)];
					//entity.DataSourceID = (Convert.IsDBNull(reader["DataSourceID"]))?string.Empty:(System.String)reader["DataSourceID"];
					entity.DataSourceName = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.DataSourceName)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.DataSourceName)];
					//entity.DataSourceName = (Convert.IsDBNull(reader["DataSourceName"]))?string.Empty:(System.String)reader["DataSourceName"];
					entity.GeneralRemarks = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.GeneralRemarks)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.GeneralRemarks)];
					//entity.GeneralRemarks = (Convert.IsDBNull(reader["GeneralRemarks"]))?string.Empty:(System.String)reader["GeneralRemarks"];
					entity.GapType = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.GapType)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.GapType)];
					//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.ArchitectureStatus = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.ArchitectureStatus)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.ArchitectureStatus)];
					//entity.ArchitectureStatus = (Convert.IsDBNull(reader["ArchitectureStatus"]))?string.Empty:(System.String)reader["ArchitectureStatus"];
					entity.ArchitectureStatusDate = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.ArchitectureStatusDate)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.ArchitectureStatusDate)];
					//entity.ArchitectureStatusDate = (Convert.IsDBNull(reader["ArchitectureStatusDate"]))?string.Empty:(System.String)reader["ArchitectureStatusDate"];
					entity.DesignRationale = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.DesignRationale)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.DesignRationale)];
					//entity.DesignRationale = (Convert.IsDBNull(reader["DesignRationale"]))?string.Empty:(System.String)reader["DesignRationale"];
					entity.IsLicenced = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.IsLicenced)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.IsLicenced)];
					//entity.IsLicenced = (Convert.IsDBNull(reader["IsLicenced"]))?string.Empty:(System.String)reader["IsLicenced"];
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
		/// Refreshes the <see cref="METAView_PhysicalSoftwareComponent_Retrieval"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_PhysicalSoftwareComponent_Retrieval"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAView_PhysicalSoftwareComponent_Retrieval entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.WorkspaceTypeId)];
			//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
			entity.VCStatusID = (System.Int32)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.pkid = (System.Int32)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.pkid)];
			//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.InternalName = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.InternalName)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.InternalName)];
			//entity.InternalName = (Convert.IsDBNull(reader["InternalName"]))?string.Empty:(System.String)reader["InternalName"];
			entity.ConfigurationID = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.ConfigurationID)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.ConfigurationID)];
			//entity.ConfigurationID = (Convert.IsDBNull(reader["ConfigurationID"]))?string.Empty:(System.String)reader["ConfigurationID"];
			entity.SoftwareType = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.SoftwareType)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.SoftwareType)];
			//entity.SoftwareType = (Convert.IsDBNull(reader["SoftwareType"]))?string.Empty:(System.String)reader["SoftwareType"];
			entity.SoftwareLevel = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.SoftwareLevel)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.SoftwareLevel)];
			//entity.SoftwareLevel = (Convert.IsDBNull(reader["SoftwareLevel"]))?string.Empty:(System.String)reader["SoftwareLevel"];
			entity.IsBespoke = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.IsBespoke)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.IsBespoke)];
			//entity.IsBespoke = (Convert.IsDBNull(reader["IsBespoke"]))?string.Empty:(System.String)reader["IsBespoke"];
			entity.UserInterfaceType = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.UserInterfaceType)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.UserInterfaceType)];
			//entity.UserInterfaceType = (Convert.IsDBNull(reader["UserInterfaceType"]))?string.Empty:(System.String)reader["UserInterfaceType"];
			entity.NumberofUsers = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.NumberofUsers)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.NumberofUsers)];
			//entity.NumberofUsers = (Convert.IsDBNull(reader["NumberofUsers"]))?string.Empty:(System.String)reader["NumberofUsers"];
			entity.SeverityRating = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.SeverityRating)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.SeverityRating)];
			//entity.SeverityRating = (Convert.IsDBNull(reader["SeverityRating"]))?string.Empty:(System.String)reader["SeverityRating"];
			entity.Edition = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Edition)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Edition)];
			//entity.Edition = (Convert.IsDBNull(reader["Edition"]))?string.Empty:(System.String)reader["Edition"];
			entity.Release = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Release)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Release)];
			//entity.Release = (Convert.IsDBNull(reader["Release"]))?string.Empty:(System.String)reader["Release"];
			entity.ServicePackID = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.ServicePackID)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.ServicePackID)];
			//entity.ServicePackID = (Convert.IsDBNull(reader["ServicePackID"]))?string.Empty:(System.String)reader["ServicePackID"];
			entity.VersionNumber = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.VersionNumber)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.VersionNumber)];
			//entity.VersionNumber = (Convert.IsDBNull(reader["VersionNumber"]))?string.Empty:(System.String)reader["VersionNumber"];
			entity.ID = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.ID)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.ID)];
			//entity.ID = (Convert.IsDBNull(reader["ID"]))?string.Empty:(System.String)reader["ID"];
			entity.PublisherName = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.PublisherName)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.PublisherName)];
			//entity.PublisherName = (Convert.IsDBNull(reader["PublisherName"]))?string.Empty:(System.String)reader["PublisherName"];
			entity.Language = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Language)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Language)];
			//entity.Language = (Convert.IsDBNull(reader["Language"]))?string.Empty:(System.String)reader["Language"];
			entity.DateCreated = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.DateCreated)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.DateCreated)];
			//entity.DateCreated = (Convert.IsDBNull(reader["DateCreated"]))?string.Empty:(System.String)reader["DateCreated"];
			entity.DatePurchased = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.DatePurchased)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.DatePurchased)];
			//entity.DatePurchased = (Convert.IsDBNull(reader["DatePurchased"]))?string.Empty:(System.String)reader["DatePurchased"];
			entity.IsDNS = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.IsDNS)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.IsDNS)];
			//entity.IsDNS = (Convert.IsDBNull(reader["IsDNS"]))?string.Empty:(System.String)reader["IsDNS"];
			entity.IsDHCP = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.IsDHCP)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.IsDHCP)];
			//entity.IsDHCP = (Convert.IsDBNull(reader["IsDHCP"]))?string.Empty:(System.String)reader["IsDHCP"];
			entity.isLicensed = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.isLicensed)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.isLicensed)];
			//entity.isLicensed = (Convert.IsDBNull(reader["isLicensed"]))?string.Empty:(System.String)reader["isLicensed"];
			entity.LicenseNumber = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.LicenseNumber)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.LicenseNumber)];
			//entity.LicenseNumber = (Convert.IsDBNull(reader["LicenseNumber"]))?string.Empty:(System.String)reader["LicenseNumber"];
			entity.HasCopyright = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.HasCopyright)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.HasCopyright)];
			//entity.HasCopyright = (Convert.IsDBNull(reader["HasCopyright"]))?string.Empty:(System.String)reader["HasCopyright"];
			entity.Configuration = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Configuration)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Configuration)];
			//entity.Configuration = (Convert.IsDBNull(reader["Configuration"]))?string.Empty:(System.String)reader["Configuration"];
			entity.Type = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Type)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Type)];
			//entity.Type = (Convert.IsDBNull(reader["Type"]))?string.Empty:(System.String)reader["Type"];
			entity.UserInterface = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.UserInterface)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.UserInterface)];
			//entity.UserInterface = (Convert.IsDBNull(reader["UserInterface"]))?string.Empty:(System.String)reader["UserInterface"];
			entity.ServicePack = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.ServicePack)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.ServicePack)];
			//entity.ServicePack = (Convert.IsDBNull(reader["ServicePack"]))?string.Empty:(System.String)reader["ServicePack"];
			entity.Version = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Version)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Version)];
			//entity.Version = (Convert.IsDBNull(reader["Version"]))?string.Empty:(System.String)reader["Version"];
			entity.Publisher = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Publisher)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Publisher)];
			//entity.Publisher = (Convert.IsDBNull(reader["Publisher"]))?string.Empty:(System.String)reader["Publisher"];
			entity.Copyright = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Copyright)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Copyright)];
			//entity.Copyright = (Convert.IsDBNull(reader["Copyright"]))?string.Empty:(System.String)reader["Copyright"];
			entity.Name = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Name)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Name)];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.Description = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Description)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Description)];
			//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
			entity.Abbreviation = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Abbreviation)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.Abbreviation)];
			//entity.Abbreviation = (Convert.IsDBNull(reader["Abbreviation"]))?string.Empty:(System.String)reader["Abbreviation"];
			entity.IsBusinessExternal = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.IsBusinessExternal)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.IsBusinessExternal)];
			//entity.IsBusinessExternal = (Convert.IsDBNull(reader["IsBusinessExternal"]))?string.Empty:(System.String)reader["IsBusinessExternal"];
			entity.StandardisationStatus = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.StandardisationStatus)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.StandardisationStatus)];
			//entity.StandardisationStatus = (Convert.IsDBNull(reader["StandardisationStatus"]))?string.Empty:(System.String)reader["StandardisationStatus"];
			entity.StandardisationStatusDate = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.StandardisationStatusDate)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.StandardisationStatusDate)];
			//entity.StandardisationStatusDate = (Convert.IsDBNull(reader["StandardisationStatusDate"]))?string.Empty:(System.String)reader["StandardisationStatusDate"];
			entity.DataSourceID = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.DataSourceID)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.DataSourceID)];
			//entity.DataSourceID = (Convert.IsDBNull(reader["DataSourceID"]))?string.Empty:(System.String)reader["DataSourceID"];
			entity.DataSourceName = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.DataSourceName)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.DataSourceName)];
			//entity.DataSourceName = (Convert.IsDBNull(reader["DataSourceName"]))?string.Empty:(System.String)reader["DataSourceName"];
			entity.GeneralRemarks = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.GeneralRemarks)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.GeneralRemarks)];
			//entity.GeneralRemarks = (Convert.IsDBNull(reader["GeneralRemarks"]))?string.Empty:(System.String)reader["GeneralRemarks"];
			entity.GapType = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.GapType)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.ArchitectureStatus = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.ArchitectureStatus)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.ArchitectureStatus)];
			//entity.ArchitectureStatus = (Convert.IsDBNull(reader["ArchitectureStatus"]))?string.Empty:(System.String)reader["ArchitectureStatus"];
			entity.ArchitectureStatusDate = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.ArchitectureStatusDate)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.ArchitectureStatusDate)];
			//entity.ArchitectureStatusDate = (Convert.IsDBNull(reader["ArchitectureStatusDate"]))?string.Empty:(System.String)reader["ArchitectureStatusDate"];
			entity.DesignRationale = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.DesignRationale)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.DesignRationale)];
			//entity.DesignRationale = (Convert.IsDBNull(reader["DesignRationale"]))?string.Empty:(System.String)reader["DesignRationale"];
			entity.IsLicenced = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.IsLicenced)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_RetrievalColumn.IsLicenced)];
			//entity.IsLicenced = (Convert.IsDBNull(reader["IsLicenced"]))?string.Empty:(System.String)reader["IsLicenced"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAView_PhysicalSoftwareComponent_Retrieval"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_PhysicalSoftwareComponent_Retrieval"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAView_PhysicalSoftwareComponent_Retrieval entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeId = (Convert.IsDBNull(dataRow["WorkspaceTypeId"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeId"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.InternalName = (Convert.IsDBNull(dataRow["InternalName"]))?string.Empty:(System.String)dataRow["InternalName"];
			entity.ConfigurationID = (Convert.IsDBNull(dataRow["ConfigurationID"]))?string.Empty:(System.String)dataRow["ConfigurationID"];
			entity.SoftwareType = (Convert.IsDBNull(dataRow["SoftwareType"]))?string.Empty:(System.String)dataRow["SoftwareType"];
			entity.SoftwareLevel = (Convert.IsDBNull(dataRow["SoftwareLevel"]))?string.Empty:(System.String)dataRow["SoftwareLevel"];
			entity.IsBespoke = (Convert.IsDBNull(dataRow["IsBespoke"]))?string.Empty:(System.String)dataRow["IsBespoke"];
			entity.UserInterfaceType = (Convert.IsDBNull(dataRow["UserInterfaceType"]))?string.Empty:(System.String)dataRow["UserInterfaceType"];
			entity.NumberofUsers = (Convert.IsDBNull(dataRow["NumberofUsers"]))?string.Empty:(System.String)dataRow["NumberofUsers"];
			entity.SeverityRating = (Convert.IsDBNull(dataRow["SeverityRating"]))?string.Empty:(System.String)dataRow["SeverityRating"];
			entity.Edition = (Convert.IsDBNull(dataRow["Edition"]))?string.Empty:(System.String)dataRow["Edition"];
			entity.Release = (Convert.IsDBNull(dataRow["Release"]))?string.Empty:(System.String)dataRow["Release"];
			entity.ServicePackID = (Convert.IsDBNull(dataRow["ServicePackID"]))?string.Empty:(System.String)dataRow["ServicePackID"];
			entity.VersionNumber = (Convert.IsDBNull(dataRow["VersionNumber"]))?string.Empty:(System.String)dataRow["VersionNumber"];
			entity.ID = (Convert.IsDBNull(dataRow["ID"]))?string.Empty:(System.String)dataRow["ID"];
			entity.PublisherName = (Convert.IsDBNull(dataRow["PublisherName"]))?string.Empty:(System.String)dataRow["PublisherName"];
			entity.Language = (Convert.IsDBNull(dataRow["Language"]))?string.Empty:(System.String)dataRow["Language"];
			entity.DateCreated = (Convert.IsDBNull(dataRow["DateCreated"]))?string.Empty:(System.String)dataRow["DateCreated"];
			entity.DatePurchased = (Convert.IsDBNull(dataRow["DatePurchased"]))?string.Empty:(System.String)dataRow["DatePurchased"];
			entity.IsDNS = (Convert.IsDBNull(dataRow["IsDNS"]))?string.Empty:(System.String)dataRow["IsDNS"];
			entity.IsDHCP = (Convert.IsDBNull(dataRow["IsDHCP"]))?string.Empty:(System.String)dataRow["IsDHCP"];
			entity.isLicensed = (Convert.IsDBNull(dataRow["isLicensed"]))?string.Empty:(System.String)dataRow["isLicensed"];
			entity.LicenseNumber = (Convert.IsDBNull(dataRow["LicenseNumber"]))?string.Empty:(System.String)dataRow["LicenseNumber"];
			entity.HasCopyright = (Convert.IsDBNull(dataRow["HasCopyright"]))?string.Empty:(System.String)dataRow["HasCopyright"];
			entity.Configuration = (Convert.IsDBNull(dataRow["Configuration"]))?string.Empty:(System.String)dataRow["Configuration"];
			entity.Type = (Convert.IsDBNull(dataRow["Type"]))?string.Empty:(System.String)dataRow["Type"];
			entity.UserInterface = (Convert.IsDBNull(dataRow["UserInterface"]))?string.Empty:(System.String)dataRow["UserInterface"];
			entity.ServicePack = (Convert.IsDBNull(dataRow["ServicePack"]))?string.Empty:(System.String)dataRow["ServicePack"];
			entity.Version = (Convert.IsDBNull(dataRow["Version"]))?string.Empty:(System.String)dataRow["Version"];
			entity.Publisher = (Convert.IsDBNull(dataRow["Publisher"]))?string.Empty:(System.String)dataRow["Publisher"];
			entity.Copyright = (Convert.IsDBNull(dataRow["Copyright"]))?string.Empty:(System.String)dataRow["Copyright"];
			entity.Name = (Convert.IsDBNull(dataRow["Name"]))?string.Empty:(System.String)dataRow["Name"];
			entity.Description = (Convert.IsDBNull(dataRow["Description"]))?string.Empty:(System.String)dataRow["Description"];
			entity.Abbreviation = (Convert.IsDBNull(dataRow["Abbreviation"]))?string.Empty:(System.String)dataRow["Abbreviation"];
			entity.IsBusinessExternal = (Convert.IsDBNull(dataRow["IsBusinessExternal"]))?string.Empty:(System.String)dataRow["IsBusinessExternal"];
			entity.StandardisationStatus = (Convert.IsDBNull(dataRow["StandardisationStatus"]))?string.Empty:(System.String)dataRow["StandardisationStatus"];
			entity.StandardisationStatusDate = (Convert.IsDBNull(dataRow["StandardisationStatusDate"]))?string.Empty:(System.String)dataRow["StandardisationStatusDate"];
			entity.DataSourceID = (Convert.IsDBNull(dataRow["DataSourceID"]))?string.Empty:(System.String)dataRow["DataSourceID"];
			entity.DataSourceName = (Convert.IsDBNull(dataRow["DataSourceName"]))?string.Empty:(System.String)dataRow["DataSourceName"];
			entity.GeneralRemarks = (Convert.IsDBNull(dataRow["GeneralRemarks"]))?string.Empty:(System.String)dataRow["GeneralRemarks"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.CustomField1 = (Convert.IsDBNull(dataRow["CustomField1"]))?string.Empty:(System.String)dataRow["CustomField1"];
			entity.CustomField2 = (Convert.IsDBNull(dataRow["CustomField2"]))?string.Empty:(System.String)dataRow["CustomField2"];
			entity.CustomField3 = (Convert.IsDBNull(dataRow["CustomField3"]))?string.Empty:(System.String)dataRow["CustomField3"];
			entity.ArchitectureStatus = (Convert.IsDBNull(dataRow["ArchitectureStatus"]))?string.Empty:(System.String)dataRow["ArchitectureStatus"];
			entity.ArchitectureStatusDate = (Convert.IsDBNull(dataRow["ArchitectureStatusDate"]))?string.Empty:(System.String)dataRow["ArchitectureStatusDate"];
			entity.DesignRationale = (Convert.IsDBNull(dataRow["DesignRationale"]))?string.Empty:(System.String)dataRow["DesignRationale"];
			entity.IsLicenced = (Convert.IsDBNull(dataRow["IsLicenced"]))?string.Empty:(System.String)dataRow["IsLicenced"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAView_PhysicalSoftwareComponent_RetrievalFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_PhysicalSoftwareComponent_Retrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_PhysicalSoftwareComponent_RetrievalFilterBuilder : SqlFilterBuilder<METAView_PhysicalSoftwareComponent_RetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_PhysicalSoftwareComponent_RetrievalFilterBuilder class.
		/// </summary>
		public METAView_PhysicalSoftwareComponent_RetrievalFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_PhysicalSoftwareComponent_RetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_PhysicalSoftwareComponent_RetrievalFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_PhysicalSoftwareComponent_RetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_PhysicalSoftwareComponent_RetrievalFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_PhysicalSoftwareComponent_RetrievalFilterBuilder

	#region METAView_PhysicalSoftwareComponent_RetrievalParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_PhysicalSoftwareComponent_Retrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_PhysicalSoftwareComponent_RetrievalParameterBuilder : ParameterizedSqlFilterBuilder<METAView_PhysicalSoftwareComponent_RetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_PhysicalSoftwareComponent_RetrievalParameterBuilder class.
		/// </summary>
		public METAView_PhysicalSoftwareComponent_RetrievalParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_PhysicalSoftwareComponent_RetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_PhysicalSoftwareComponent_RetrievalParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_PhysicalSoftwareComponent_RetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_PhysicalSoftwareComponent_RetrievalParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_PhysicalSoftwareComponent_RetrievalParameterBuilder
	
	#region METAView_PhysicalSoftwareComponent_RetrievalSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_PhysicalSoftwareComponent_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_PhysicalSoftwareComponent_RetrievalSortBuilder : SqlSortBuilder<METAView_PhysicalSoftwareComponent_RetrievalColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_PhysicalSoftwareComponent_RetrievalSqlSortBuilder class.
		/// </summary>
		public METAView_PhysicalSoftwareComponent_RetrievalSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAView_PhysicalSoftwareComponent_RetrievalSortBuilder

} // end namespace
