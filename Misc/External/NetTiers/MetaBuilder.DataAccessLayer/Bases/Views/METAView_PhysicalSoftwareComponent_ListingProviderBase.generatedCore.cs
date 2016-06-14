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
	/// This class is the base class for any <see cref="METAView_PhysicalSoftwareComponent_ListingProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAView_PhysicalSoftwareComponent_ListingProviderBaseCore : EntityViewProviderBase<METAView_PhysicalSoftwareComponent_Listing>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAView_PhysicalSoftwareComponent_Listing&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAView_PhysicalSoftwareComponent_Listing&gt;"/></returns>
		protected static VList&lt;METAView_PhysicalSoftwareComponent_Listing&gt; Fill(DataSet dataSet, VList<METAView_PhysicalSoftwareComponent_Listing> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAView_PhysicalSoftwareComponent_Listing>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAView_PhysicalSoftwareComponent_Listing&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAView_PhysicalSoftwareComponent_Listing>"/></returns>
		protected static VList&lt;METAView_PhysicalSoftwareComponent_Listing&gt; Fill(DataTable dataTable, VList<METAView_PhysicalSoftwareComponent_Listing> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAView_PhysicalSoftwareComponent_Listing c = new METAView_PhysicalSoftwareComponent_Listing();
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
					c.LicenseNumber = (Convert.IsDBNull(row["LicenseNumber"]))?string.Empty:(System.String)row["LicenseNumber"];
					c.HasCopyright = (Convert.IsDBNull(row["HasCopyright"]))?string.Empty:(System.String)row["HasCopyright"];
					c.Name = (Convert.IsDBNull(row["Name"]))?string.Empty:(System.String)row["Name"];
					c.Description = (Convert.IsDBNull(row["Description"]))?string.Empty:(System.String)row["Description"];
					c.Abbreviation = (Convert.IsDBNull(row["Abbreviation"]))?string.Empty:(System.String)row["Abbreviation"];
					c.IsBusinessExternal = (Convert.IsDBNull(row["IsBusinessExternal"]))?string.Empty:(System.String)row["IsBusinessExternal"];
					c.DataSourceID = (Convert.IsDBNull(row["DataSourceID"]))?string.Empty:(System.String)row["DataSourceID"];
					c.DataSourceName = (Convert.IsDBNull(row["DataSourceName"]))?string.Empty:(System.String)row["DataSourceName"];
					c.GeneralRemarks = (Convert.IsDBNull(row["GeneralRemarks"]))?string.Empty:(System.String)row["GeneralRemarks"];
					c.GapType = (Convert.IsDBNull(row["GapType"]))?string.Empty:(System.String)row["GapType"];
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
		/// Fill an <see cref="VList&lt;METAView_PhysicalSoftwareComponent_Listing&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAView_PhysicalSoftwareComponent_Listing&gt;"/></returns>
		protected VList<METAView_PhysicalSoftwareComponent_Listing> Fill(IDataReader reader, VList<METAView_PhysicalSoftwareComponent_Listing> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAView_PhysicalSoftwareComponent_Listing entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAView_PhysicalSoftwareComponent_Listing>("METAView_PhysicalSoftwareComponent_Listing",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAView_PhysicalSoftwareComponent_Listing();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.WorkspaceTypeId)];
					//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
					entity.VCStatusID = (System.Int32)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.pkid = (System.Int32)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.pkid)];
					//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.InternalName = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.InternalName)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.InternalName)];
					//entity.InternalName = (Convert.IsDBNull(reader["InternalName"]))?string.Empty:(System.String)reader["InternalName"];
					entity.ConfigurationID = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.ConfigurationID)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.ConfigurationID)];
					//entity.ConfigurationID = (Convert.IsDBNull(reader["ConfigurationID"]))?string.Empty:(System.String)reader["ConfigurationID"];
					entity.SoftwareType = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.SoftwareType)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.SoftwareType)];
					//entity.SoftwareType = (Convert.IsDBNull(reader["SoftwareType"]))?string.Empty:(System.String)reader["SoftwareType"];
					entity.SoftwareLevel = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.SoftwareLevel)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.SoftwareLevel)];
					//entity.SoftwareLevel = (Convert.IsDBNull(reader["SoftwareLevel"]))?string.Empty:(System.String)reader["SoftwareLevel"];
					entity.IsBespoke = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.IsBespoke)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.IsBespoke)];
					//entity.IsBespoke = (Convert.IsDBNull(reader["IsBespoke"]))?string.Empty:(System.String)reader["IsBespoke"];
					entity.UserInterfaceType = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.UserInterfaceType)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.UserInterfaceType)];
					//entity.UserInterfaceType = (Convert.IsDBNull(reader["UserInterfaceType"]))?string.Empty:(System.String)reader["UserInterfaceType"];
					entity.NumberofUsers = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.NumberofUsers)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.NumberofUsers)];
					//entity.NumberofUsers = (Convert.IsDBNull(reader["NumberofUsers"]))?string.Empty:(System.String)reader["NumberofUsers"];
					entity.SeverityRating = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.SeverityRating)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.SeverityRating)];
					//entity.SeverityRating = (Convert.IsDBNull(reader["SeverityRating"]))?string.Empty:(System.String)reader["SeverityRating"];
					entity.Edition = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.Edition)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.Edition)];
					//entity.Edition = (Convert.IsDBNull(reader["Edition"]))?string.Empty:(System.String)reader["Edition"];
					entity.Release = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.Release)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.Release)];
					//entity.Release = (Convert.IsDBNull(reader["Release"]))?string.Empty:(System.String)reader["Release"];
					entity.ServicePackID = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.ServicePackID)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.ServicePackID)];
					//entity.ServicePackID = (Convert.IsDBNull(reader["ServicePackID"]))?string.Empty:(System.String)reader["ServicePackID"];
					entity.VersionNumber = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.VersionNumber)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.VersionNumber)];
					//entity.VersionNumber = (Convert.IsDBNull(reader["VersionNumber"]))?string.Empty:(System.String)reader["VersionNumber"];
					entity.ID = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.ID)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.ID)];
					//entity.ID = (Convert.IsDBNull(reader["ID"]))?string.Empty:(System.String)reader["ID"];
					entity.PublisherName = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.PublisherName)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.PublisherName)];
					//entity.PublisherName = (Convert.IsDBNull(reader["PublisherName"]))?string.Empty:(System.String)reader["PublisherName"];
					entity.Language = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.Language)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.Language)];
					//entity.Language = (Convert.IsDBNull(reader["Language"]))?string.Empty:(System.String)reader["Language"];
					entity.DateCreated = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.DateCreated)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.DateCreated)];
					//entity.DateCreated = (Convert.IsDBNull(reader["DateCreated"]))?string.Empty:(System.String)reader["DateCreated"];
					entity.DatePurchased = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.DatePurchased)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.DatePurchased)];
					//entity.DatePurchased = (Convert.IsDBNull(reader["DatePurchased"]))?string.Empty:(System.String)reader["DatePurchased"];
					entity.LicenseNumber = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.LicenseNumber)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.LicenseNumber)];
					//entity.LicenseNumber = (Convert.IsDBNull(reader["LicenseNumber"]))?string.Empty:(System.String)reader["LicenseNumber"];
					entity.HasCopyright = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.HasCopyright)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.HasCopyright)];
					//entity.HasCopyright = (Convert.IsDBNull(reader["HasCopyright"]))?string.Empty:(System.String)reader["HasCopyright"];
					entity.Name = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.Name)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.Name)];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.Description = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.Description)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.Description)];
					//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
					entity.Abbreviation = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.Abbreviation)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.Abbreviation)];
					//entity.Abbreviation = (Convert.IsDBNull(reader["Abbreviation"]))?string.Empty:(System.String)reader["Abbreviation"];
					entity.IsBusinessExternal = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.IsBusinessExternal)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.IsBusinessExternal)];
					//entity.IsBusinessExternal = (Convert.IsDBNull(reader["IsBusinessExternal"]))?string.Empty:(System.String)reader["IsBusinessExternal"];
					entity.DataSourceID = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.DataSourceID)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.DataSourceID)];
					//entity.DataSourceID = (Convert.IsDBNull(reader["DataSourceID"]))?string.Empty:(System.String)reader["DataSourceID"];
					entity.DataSourceName = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.DataSourceName)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.DataSourceName)];
					//entity.DataSourceName = (Convert.IsDBNull(reader["DataSourceName"]))?string.Empty:(System.String)reader["DataSourceName"];
					entity.GeneralRemarks = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.GeneralRemarks)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.GeneralRemarks)];
					//entity.GeneralRemarks = (Convert.IsDBNull(reader["GeneralRemarks"]))?string.Empty:(System.String)reader["GeneralRemarks"];
					entity.GapType = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.GapType)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.GapType)];
					//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
					entity.ArchitectureStatus = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.ArchitectureStatus)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.ArchitectureStatus)];
					//entity.ArchitectureStatus = (Convert.IsDBNull(reader["ArchitectureStatus"]))?string.Empty:(System.String)reader["ArchitectureStatus"];
					entity.ArchitectureStatusDate = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.ArchitectureStatusDate)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.ArchitectureStatusDate)];
					//entity.ArchitectureStatusDate = (Convert.IsDBNull(reader["ArchitectureStatusDate"]))?string.Empty:(System.String)reader["ArchitectureStatusDate"];
					entity.DesignRationale = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.DesignRationale)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.DesignRationale)];
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
		/// Refreshes the <see cref="METAView_PhysicalSoftwareComponent_Listing"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_PhysicalSoftwareComponent_Listing"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAView_PhysicalSoftwareComponent_Listing entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.WorkspaceTypeId)];
			//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
			entity.VCStatusID = (System.Int32)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.pkid = (System.Int32)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.pkid)];
			//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.InternalName = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.InternalName)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.InternalName)];
			//entity.InternalName = (Convert.IsDBNull(reader["InternalName"]))?string.Empty:(System.String)reader["InternalName"];
			entity.ConfigurationID = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.ConfigurationID)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.ConfigurationID)];
			//entity.ConfigurationID = (Convert.IsDBNull(reader["ConfigurationID"]))?string.Empty:(System.String)reader["ConfigurationID"];
			entity.SoftwareType = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.SoftwareType)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.SoftwareType)];
			//entity.SoftwareType = (Convert.IsDBNull(reader["SoftwareType"]))?string.Empty:(System.String)reader["SoftwareType"];
			entity.SoftwareLevel = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.SoftwareLevel)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.SoftwareLevel)];
			//entity.SoftwareLevel = (Convert.IsDBNull(reader["SoftwareLevel"]))?string.Empty:(System.String)reader["SoftwareLevel"];
			entity.IsBespoke = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.IsBespoke)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.IsBespoke)];
			//entity.IsBespoke = (Convert.IsDBNull(reader["IsBespoke"]))?string.Empty:(System.String)reader["IsBespoke"];
			entity.UserInterfaceType = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.UserInterfaceType)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.UserInterfaceType)];
			//entity.UserInterfaceType = (Convert.IsDBNull(reader["UserInterfaceType"]))?string.Empty:(System.String)reader["UserInterfaceType"];
			entity.NumberofUsers = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.NumberofUsers)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.NumberofUsers)];
			//entity.NumberofUsers = (Convert.IsDBNull(reader["NumberofUsers"]))?string.Empty:(System.String)reader["NumberofUsers"];
			entity.SeverityRating = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.SeverityRating)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.SeverityRating)];
			//entity.SeverityRating = (Convert.IsDBNull(reader["SeverityRating"]))?string.Empty:(System.String)reader["SeverityRating"];
			entity.Edition = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.Edition)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.Edition)];
			//entity.Edition = (Convert.IsDBNull(reader["Edition"]))?string.Empty:(System.String)reader["Edition"];
			entity.Release = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.Release)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.Release)];
			//entity.Release = (Convert.IsDBNull(reader["Release"]))?string.Empty:(System.String)reader["Release"];
			entity.ServicePackID = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.ServicePackID)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.ServicePackID)];
			//entity.ServicePackID = (Convert.IsDBNull(reader["ServicePackID"]))?string.Empty:(System.String)reader["ServicePackID"];
			entity.VersionNumber = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.VersionNumber)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.VersionNumber)];
			//entity.VersionNumber = (Convert.IsDBNull(reader["VersionNumber"]))?string.Empty:(System.String)reader["VersionNumber"];
			entity.ID = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.ID)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.ID)];
			//entity.ID = (Convert.IsDBNull(reader["ID"]))?string.Empty:(System.String)reader["ID"];
			entity.PublisherName = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.PublisherName)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.PublisherName)];
			//entity.PublisherName = (Convert.IsDBNull(reader["PublisherName"]))?string.Empty:(System.String)reader["PublisherName"];
			entity.Language = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.Language)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.Language)];
			//entity.Language = (Convert.IsDBNull(reader["Language"]))?string.Empty:(System.String)reader["Language"];
			entity.DateCreated = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.DateCreated)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.DateCreated)];
			//entity.DateCreated = (Convert.IsDBNull(reader["DateCreated"]))?string.Empty:(System.String)reader["DateCreated"];
			entity.DatePurchased = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.DatePurchased)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.DatePurchased)];
			//entity.DatePurchased = (Convert.IsDBNull(reader["DatePurchased"]))?string.Empty:(System.String)reader["DatePurchased"];
			entity.LicenseNumber = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.LicenseNumber)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.LicenseNumber)];
			//entity.LicenseNumber = (Convert.IsDBNull(reader["LicenseNumber"]))?string.Empty:(System.String)reader["LicenseNumber"];
			entity.HasCopyright = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.HasCopyright)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.HasCopyright)];
			//entity.HasCopyright = (Convert.IsDBNull(reader["HasCopyright"]))?string.Empty:(System.String)reader["HasCopyright"];
			entity.Name = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.Name)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.Name)];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.Description = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.Description)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.Description)];
			//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
			entity.Abbreviation = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.Abbreviation)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.Abbreviation)];
			//entity.Abbreviation = (Convert.IsDBNull(reader["Abbreviation"]))?string.Empty:(System.String)reader["Abbreviation"];
			entity.IsBusinessExternal = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.IsBusinessExternal)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.IsBusinessExternal)];
			//entity.IsBusinessExternal = (Convert.IsDBNull(reader["IsBusinessExternal"]))?string.Empty:(System.String)reader["IsBusinessExternal"];
			entity.DataSourceID = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.DataSourceID)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.DataSourceID)];
			//entity.DataSourceID = (Convert.IsDBNull(reader["DataSourceID"]))?string.Empty:(System.String)reader["DataSourceID"];
			entity.DataSourceName = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.DataSourceName)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.DataSourceName)];
			//entity.DataSourceName = (Convert.IsDBNull(reader["DataSourceName"]))?string.Empty:(System.String)reader["DataSourceName"];
			entity.GeneralRemarks = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.GeneralRemarks)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.GeneralRemarks)];
			//entity.GeneralRemarks = (Convert.IsDBNull(reader["GeneralRemarks"]))?string.Empty:(System.String)reader["GeneralRemarks"];
			entity.GapType = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.GapType)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			entity.ArchitectureStatus = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.ArchitectureStatus)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.ArchitectureStatus)];
			//entity.ArchitectureStatus = (Convert.IsDBNull(reader["ArchitectureStatus"]))?string.Empty:(System.String)reader["ArchitectureStatus"];
			entity.ArchitectureStatusDate = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.ArchitectureStatusDate)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.ArchitectureStatusDate)];
			//entity.ArchitectureStatusDate = (Convert.IsDBNull(reader["ArchitectureStatusDate"]))?string.Empty:(System.String)reader["ArchitectureStatusDate"];
			entity.DesignRationale = (reader.IsDBNull(((int)METAView_PhysicalSoftwareComponent_ListingColumn.DesignRationale)))?null:(System.String)reader[((int)METAView_PhysicalSoftwareComponent_ListingColumn.DesignRationale)];
			//entity.DesignRationale = (Convert.IsDBNull(reader["DesignRationale"]))?string.Empty:(System.String)reader["DesignRationale"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAView_PhysicalSoftwareComponent_Listing"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_PhysicalSoftwareComponent_Listing"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAView_PhysicalSoftwareComponent_Listing entity)
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
			entity.LicenseNumber = (Convert.IsDBNull(dataRow["LicenseNumber"]))?string.Empty:(System.String)dataRow["LicenseNumber"];
			entity.HasCopyright = (Convert.IsDBNull(dataRow["HasCopyright"]))?string.Empty:(System.String)dataRow["HasCopyright"];
			entity.Name = (Convert.IsDBNull(dataRow["Name"]))?string.Empty:(System.String)dataRow["Name"];
			entity.Description = (Convert.IsDBNull(dataRow["Description"]))?string.Empty:(System.String)dataRow["Description"];
			entity.Abbreviation = (Convert.IsDBNull(dataRow["Abbreviation"]))?string.Empty:(System.String)dataRow["Abbreviation"];
			entity.IsBusinessExternal = (Convert.IsDBNull(dataRow["IsBusinessExternal"]))?string.Empty:(System.String)dataRow["IsBusinessExternal"];
			entity.DataSourceID = (Convert.IsDBNull(dataRow["DataSourceID"]))?string.Empty:(System.String)dataRow["DataSourceID"];
			entity.DataSourceName = (Convert.IsDBNull(dataRow["DataSourceName"]))?string.Empty:(System.String)dataRow["DataSourceName"];
			entity.GeneralRemarks = (Convert.IsDBNull(dataRow["GeneralRemarks"]))?string.Empty:(System.String)dataRow["GeneralRemarks"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.ArchitectureStatus = (Convert.IsDBNull(dataRow["ArchitectureStatus"]))?string.Empty:(System.String)dataRow["ArchitectureStatus"];
			entity.ArchitectureStatusDate = (Convert.IsDBNull(dataRow["ArchitectureStatusDate"]))?string.Empty:(System.String)dataRow["ArchitectureStatusDate"];
			entity.DesignRationale = (Convert.IsDBNull(dataRow["DesignRationale"]))?string.Empty:(System.String)dataRow["DesignRationale"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAView_PhysicalSoftwareComponent_ListingFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_PhysicalSoftwareComponent_Listing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_PhysicalSoftwareComponent_ListingFilterBuilder : SqlFilterBuilder<METAView_PhysicalSoftwareComponent_ListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_PhysicalSoftwareComponent_ListingFilterBuilder class.
		/// </summary>
		public METAView_PhysicalSoftwareComponent_ListingFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_PhysicalSoftwareComponent_ListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_PhysicalSoftwareComponent_ListingFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_PhysicalSoftwareComponent_ListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_PhysicalSoftwareComponent_ListingFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_PhysicalSoftwareComponent_ListingFilterBuilder

	#region METAView_PhysicalSoftwareComponent_ListingParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_PhysicalSoftwareComponent_Listing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_PhysicalSoftwareComponent_ListingParameterBuilder : ParameterizedSqlFilterBuilder<METAView_PhysicalSoftwareComponent_ListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_PhysicalSoftwareComponent_ListingParameterBuilder class.
		/// </summary>
		public METAView_PhysicalSoftwareComponent_ListingParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_PhysicalSoftwareComponent_ListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_PhysicalSoftwareComponent_ListingParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_PhysicalSoftwareComponent_ListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_PhysicalSoftwareComponent_ListingParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_PhysicalSoftwareComponent_ListingParameterBuilder
	
	#region METAView_PhysicalSoftwareComponent_ListingSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_PhysicalSoftwareComponent_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_PhysicalSoftwareComponent_ListingSortBuilder : SqlSortBuilder<METAView_PhysicalSoftwareComponent_ListingColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_PhysicalSoftwareComponent_ListingSqlSortBuilder class.
		/// </summary>
		public METAView_PhysicalSoftwareComponent_ListingSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAView_PhysicalSoftwareComponent_ListingSortBuilder

} // end namespace
