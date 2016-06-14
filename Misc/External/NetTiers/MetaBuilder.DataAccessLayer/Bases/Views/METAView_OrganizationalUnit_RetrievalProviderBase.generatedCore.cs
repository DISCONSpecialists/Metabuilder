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
	/// This class is the base class for any <see cref="METAView_OrganizationalUnit_RetrievalProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAView_OrganizationalUnit_RetrievalProviderBaseCore : EntityViewProviderBase<METAView_OrganizationalUnit_Retrieval>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAView_OrganizationalUnit_Retrieval&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAView_OrganizationalUnit_Retrieval&gt;"/></returns>
		protected static VList&lt;METAView_OrganizationalUnit_Retrieval&gt; Fill(DataSet dataSet, VList<METAView_OrganizationalUnit_Retrieval> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAView_OrganizationalUnit_Retrieval>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAView_OrganizationalUnit_Retrieval&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAView_OrganizationalUnit_Retrieval>"/></returns>
		protected static VList&lt;METAView_OrganizationalUnit_Retrieval&gt; Fill(DataTable dataTable, VList<METAView_OrganizationalUnit_Retrieval> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAView_OrganizationalUnit_Retrieval c = new METAView_OrganizationalUnit_Retrieval();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeId = (Convert.IsDBNull(row["WorkspaceTypeId"]))?(int)0:(System.Int32?)row["WorkspaceTypeId"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.Name = (Convert.IsDBNull(row["Name"]))?string.Empty:(System.String)row["Name"];
					c.Type = (Convert.IsDBNull(row["Type"]))?string.Empty:(System.String)row["Type"];
					c.CustomField1 = (Convert.IsDBNull(row["CustomField1"]))?string.Empty:(System.String)row["CustomField1"];
					c.CustomField2 = (Convert.IsDBNull(row["CustomField2"]))?string.Empty:(System.String)row["CustomField2"];
					c.CustomField3 = (Convert.IsDBNull(row["CustomField3"]))?string.Empty:(System.String)row["CustomField3"];
					c.Telephone = (Convert.IsDBNull(row["Telephone"]))?string.Empty:(System.String)row["Telephone"];
					c.Fax = (Convert.IsDBNull(row["Fax"]))?string.Empty:(System.String)row["Fax"];
					c.Email = (Convert.IsDBNull(row["Email"]))?string.Empty:(System.String)row["Email"];
					c.GapType = (Convert.IsDBNull(row["GapType"]))?string.Empty:(System.String)row["GapType"];
					c.Description = (Convert.IsDBNull(row["Description"]))?string.Empty:(System.String)row["Description"];
					c.Abbreviation = (Convert.IsDBNull(row["Abbreviation"]))?string.Empty:(System.String)row["Abbreviation"];
					c.IsBusinessExternal = (Convert.IsDBNull(row["IsBusinessExternal"]))?string.Empty:(System.String)row["IsBusinessExternal"];
					c.StandardisationStatus = (Convert.IsDBNull(row["StandardisationStatus"]))?string.Empty:(System.String)row["StandardisationStatus"];
					c.StandardisationStatusDate = (Convert.IsDBNull(row["StandardisationStatusDate"]))?string.Empty:(System.String)row["StandardisationStatusDate"];
					c.DataSourceID = (Convert.IsDBNull(row["DataSourceID"]))?string.Empty:(System.String)row["DataSourceID"];
					c.DataSourceName = (Convert.IsDBNull(row["DataSourceName"]))?string.Empty:(System.String)row["DataSourceName"];
					c.GeneralRemarks = (Convert.IsDBNull(row["GeneralRemarks"]))?string.Empty:(System.String)row["GeneralRemarks"];
					c.ArchitectureStatus = (Convert.IsDBNull(row["ArchitectureStatus"]))?string.Empty:(System.String)row["ArchitectureStatus"];
					c.ArchitectureStatusDate = (Convert.IsDBNull(row["ArchitectureStatusDate"]))?string.Empty:(System.String)row["ArchitectureStatusDate"];
					c.DesignRationale = (Convert.IsDBNull(row["DesignRationale"]))?string.Empty:(System.String)row["DesignRationale"];
					c.OrganizationalUnitType = (Convert.IsDBNull(row["OrganizationalUnitType"]))?string.Empty:(System.String)row["OrganizationalUnitType"];
					c.TelephoneNumber = (Convert.IsDBNull(row["TelephoneNumber"]))?string.Empty:(System.String)row["TelephoneNumber"];
					c.FaxNumber = (Convert.IsDBNull(row["FaxNumber"]))?string.Empty:(System.String)row["FaxNumber"];
					c.EmailAddress = (Convert.IsDBNull(row["EmailAddress"]))?string.Empty:(System.String)row["EmailAddress"];
					c.Headcount = (Convert.IsDBNull(row["Headcount"]))?string.Empty:(System.String)row["Headcount"];
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
		/// Fill an <see cref="VList&lt;METAView_OrganizationalUnit_Retrieval&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAView_OrganizationalUnit_Retrieval&gt;"/></returns>
		protected VList<METAView_OrganizationalUnit_Retrieval> Fill(IDataReader reader, VList<METAView_OrganizationalUnit_Retrieval> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAView_OrganizationalUnit_Retrieval entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAView_OrganizationalUnit_Retrieval>("METAView_OrganizationalUnit_Retrieval",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAView_OrganizationalUnit_Retrieval();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.WorkspaceTypeId)];
					//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
					entity.VCStatusID = (System.Int32)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.pkid = (System.Int32)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.pkid)];
					//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Name = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.Name)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.Name)];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.Type = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.Type)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.Type)];
					//entity.Type = (Convert.IsDBNull(reader["Type"]))?string.Empty:(System.String)reader["Type"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.Telephone = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.Telephone)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.Telephone)];
					//entity.Telephone = (Convert.IsDBNull(reader["Telephone"]))?string.Empty:(System.String)reader["Telephone"];
					entity.Fax = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.Fax)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.Fax)];
					//entity.Fax = (Convert.IsDBNull(reader["Fax"]))?string.Empty:(System.String)reader["Fax"];
					entity.Email = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.Email)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.Email)];
					//entity.Email = (Convert.IsDBNull(reader["Email"]))?string.Empty:(System.String)reader["Email"];
					entity.GapType = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.GapType)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.GapType)];
					//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
					entity.Description = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.Description)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.Description)];
					//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
					entity.Abbreviation = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.Abbreviation)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.Abbreviation)];
					//entity.Abbreviation = (Convert.IsDBNull(reader["Abbreviation"]))?string.Empty:(System.String)reader["Abbreviation"];
					entity.IsBusinessExternal = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.IsBusinessExternal)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.IsBusinessExternal)];
					//entity.IsBusinessExternal = (Convert.IsDBNull(reader["IsBusinessExternal"]))?string.Empty:(System.String)reader["IsBusinessExternal"];
					entity.StandardisationStatus = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.StandardisationStatus)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.StandardisationStatus)];
					//entity.StandardisationStatus = (Convert.IsDBNull(reader["StandardisationStatus"]))?string.Empty:(System.String)reader["StandardisationStatus"];
					entity.StandardisationStatusDate = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.StandardisationStatusDate)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.StandardisationStatusDate)];
					//entity.StandardisationStatusDate = (Convert.IsDBNull(reader["StandardisationStatusDate"]))?string.Empty:(System.String)reader["StandardisationStatusDate"];
					entity.DataSourceID = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.DataSourceID)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.DataSourceID)];
					//entity.DataSourceID = (Convert.IsDBNull(reader["DataSourceID"]))?string.Empty:(System.String)reader["DataSourceID"];
					entity.DataSourceName = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.DataSourceName)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.DataSourceName)];
					//entity.DataSourceName = (Convert.IsDBNull(reader["DataSourceName"]))?string.Empty:(System.String)reader["DataSourceName"];
					entity.GeneralRemarks = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.GeneralRemarks)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.GeneralRemarks)];
					//entity.GeneralRemarks = (Convert.IsDBNull(reader["GeneralRemarks"]))?string.Empty:(System.String)reader["GeneralRemarks"];
					entity.ArchitectureStatus = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.ArchitectureStatus)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.ArchitectureStatus)];
					//entity.ArchitectureStatus = (Convert.IsDBNull(reader["ArchitectureStatus"]))?string.Empty:(System.String)reader["ArchitectureStatus"];
					entity.ArchitectureStatusDate = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.ArchitectureStatusDate)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.ArchitectureStatusDate)];
					//entity.ArchitectureStatusDate = (Convert.IsDBNull(reader["ArchitectureStatusDate"]))?string.Empty:(System.String)reader["ArchitectureStatusDate"];
					entity.DesignRationale = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.DesignRationale)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.DesignRationale)];
					//entity.DesignRationale = (Convert.IsDBNull(reader["DesignRationale"]))?string.Empty:(System.String)reader["DesignRationale"];
					entity.OrganizationalUnitType = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.OrganizationalUnitType)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.OrganizationalUnitType)];
					//entity.OrganizationalUnitType = (Convert.IsDBNull(reader["OrganizationalUnitType"]))?string.Empty:(System.String)reader["OrganizationalUnitType"];
					entity.TelephoneNumber = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.TelephoneNumber)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.TelephoneNumber)];
					//entity.TelephoneNumber = (Convert.IsDBNull(reader["TelephoneNumber"]))?string.Empty:(System.String)reader["TelephoneNumber"];
					entity.FaxNumber = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.FaxNumber)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.FaxNumber)];
					//entity.FaxNumber = (Convert.IsDBNull(reader["FaxNumber"]))?string.Empty:(System.String)reader["FaxNumber"];
					entity.EmailAddress = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.EmailAddress)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.EmailAddress)];
					//entity.EmailAddress = (Convert.IsDBNull(reader["EmailAddress"]))?string.Empty:(System.String)reader["EmailAddress"];
					entity.Headcount = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.Headcount)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.Headcount)];
					//entity.Headcount = (Convert.IsDBNull(reader["Headcount"]))?string.Empty:(System.String)reader["Headcount"];
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
		/// Refreshes the <see cref="METAView_OrganizationalUnit_Retrieval"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_OrganizationalUnit_Retrieval"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAView_OrganizationalUnit_Retrieval entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.WorkspaceTypeId)];
			//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
			entity.VCStatusID = (System.Int32)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.pkid = (System.Int32)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.pkid)];
			//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Name = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.Name)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.Name)];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.Type = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.Type)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.Type)];
			//entity.Type = (Convert.IsDBNull(reader["Type"]))?string.Empty:(System.String)reader["Type"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.Telephone = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.Telephone)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.Telephone)];
			//entity.Telephone = (Convert.IsDBNull(reader["Telephone"]))?string.Empty:(System.String)reader["Telephone"];
			entity.Fax = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.Fax)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.Fax)];
			//entity.Fax = (Convert.IsDBNull(reader["Fax"]))?string.Empty:(System.String)reader["Fax"];
			entity.Email = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.Email)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.Email)];
			//entity.Email = (Convert.IsDBNull(reader["Email"]))?string.Empty:(System.String)reader["Email"];
			entity.GapType = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.GapType)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			entity.Description = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.Description)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.Description)];
			//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
			entity.Abbreviation = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.Abbreviation)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.Abbreviation)];
			//entity.Abbreviation = (Convert.IsDBNull(reader["Abbreviation"]))?string.Empty:(System.String)reader["Abbreviation"];
			entity.IsBusinessExternal = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.IsBusinessExternal)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.IsBusinessExternal)];
			//entity.IsBusinessExternal = (Convert.IsDBNull(reader["IsBusinessExternal"]))?string.Empty:(System.String)reader["IsBusinessExternal"];
			entity.StandardisationStatus = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.StandardisationStatus)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.StandardisationStatus)];
			//entity.StandardisationStatus = (Convert.IsDBNull(reader["StandardisationStatus"]))?string.Empty:(System.String)reader["StandardisationStatus"];
			entity.StandardisationStatusDate = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.StandardisationStatusDate)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.StandardisationStatusDate)];
			//entity.StandardisationStatusDate = (Convert.IsDBNull(reader["StandardisationStatusDate"]))?string.Empty:(System.String)reader["StandardisationStatusDate"];
			entity.DataSourceID = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.DataSourceID)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.DataSourceID)];
			//entity.DataSourceID = (Convert.IsDBNull(reader["DataSourceID"]))?string.Empty:(System.String)reader["DataSourceID"];
			entity.DataSourceName = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.DataSourceName)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.DataSourceName)];
			//entity.DataSourceName = (Convert.IsDBNull(reader["DataSourceName"]))?string.Empty:(System.String)reader["DataSourceName"];
			entity.GeneralRemarks = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.GeneralRemarks)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.GeneralRemarks)];
			//entity.GeneralRemarks = (Convert.IsDBNull(reader["GeneralRemarks"]))?string.Empty:(System.String)reader["GeneralRemarks"];
			entity.ArchitectureStatus = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.ArchitectureStatus)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.ArchitectureStatus)];
			//entity.ArchitectureStatus = (Convert.IsDBNull(reader["ArchitectureStatus"]))?string.Empty:(System.String)reader["ArchitectureStatus"];
			entity.ArchitectureStatusDate = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.ArchitectureStatusDate)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.ArchitectureStatusDate)];
			//entity.ArchitectureStatusDate = (Convert.IsDBNull(reader["ArchitectureStatusDate"]))?string.Empty:(System.String)reader["ArchitectureStatusDate"];
			entity.DesignRationale = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.DesignRationale)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.DesignRationale)];
			//entity.DesignRationale = (Convert.IsDBNull(reader["DesignRationale"]))?string.Empty:(System.String)reader["DesignRationale"];
			entity.OrganizationalUnitType = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.OrganizationalUnitType)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.OrganizationalUnitType)];
			//entity.OrganizationalUnitType = (Convert.IsDBNull(reader["OrganizationalUnitType"]))?string.Empty:(System.String)reader["OrganizationalUnitType"];
			entity.TelephoneNumber = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.TelephoneNumber)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.TelephoneNumber)];
			//entity.TelephoneNumber = (Convert.IsDBNull(reader["TelephoneNumber"]))?string.Empty:(System.String)reader["TelephoneNumber"];
			entity.FaxNumber = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.FaxNumber)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.FaxNumber)];
			//entity.FaxNumber = (Convert.IsDBNull(reader["FaxNumber"]))?string.Empty:(System.String)reader["FaxNumber"];
			entity.EmailAddress = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.EmailAddress)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.EmailAddress)];
			//entity.EmailAddress = (Convert.IsDBNull(reader["EmailAddress"]))?string.Empty:(System.String)reader["EmailAddress"];
			entity.Headcount = (reader.IsDBNull(((int)METAView_OrganizationalUnit_RetrievalColumn.Headcount)))?null:(System.String)reader[((int)METAView_OrganizationalUnit_RetrievalColumn.Headcount)];
			//entity.Headcount = (Convert.IsDBNull(reader["Headcount"]))?string.Empty:(System.String)reader["Headcount"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAView_OrganizationalUnit_Retrieval"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_OrganizationalUnit_Retrieval"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAView_OrganizationalUnit_Retrieval entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeId = (Convert.IsDBNull(dataRow["WorkspaceTypeId"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeId"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.Name = (Convert.IsDBNull(dataRow["Name"]))?string.Empty:(System.String)dataRow["Name"];
			entity.Type = (Convert.IsDBNull(dataRow["Type"]))?string.Empty:(System.String)dataRow["Type"];
			entity.CustomField1 = (Convert.IsDBNull(dataRow["CustomField1"]))?string.Empty:(System.String)dataRow["CustomField1"];
			entity.CustomField2 = (Convert.IsDBNull(dataRow["CustomField2"]))?string.Empty:(System.String)dataRow["CustomField2"];
			entity.CustomField3 = (Convert.IsDBNull(dataRow["CustomField3"]))?string.Empty:(System.String)dataRow["CustomField3"];
			entity.Telephone = (Convert.IsDBNull(dataRow["Telephone"]))?string.Empty:(System.String)dataRow["Telephone"];
			entity.Fax = (Convert.IsDBNull(dataRow["Fax"]))?string.Empty:(System.String)dataRow["Fax"];
			entity.Email = (Convert.IsDBNull(dataRow["Email"]))?string.Empty:(System.String)dataRow["Email"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.Description = (Convert.IsDBNull(dataRow["Description"]))?string.Empty:(System.String)dataRow["Description"];
			entity.Abbreviation = (Convert.IsDBNull(dataRow["Abbreviation"]))?string.Empty:(System.String)dataRow["Abbreviation"];
			entity.IsBusinessExternal = (Convert.IsDBNull(dataRow["IsBusinessExternal"]))?string.Empty:(System.String)dataRow["IsBusinessExternal"];
			entity.StandardisationStatus = (Convert.IsDBNull(dataRow["StandardisationStatus"]))?string.Empty:(System.String)dataRow["StandardisationStatus"];
			entity.StandardisationStatusDate = (Convert.IsDBNull(dataRow["StandardisationStatusDate"]))?string.Empty:(System.String)dataRow["StandardisationStatusDate"];
			entity.DataSourceID = (Convert.IsDBNull(dataRow["DataSourceID"]))?string.Empty:(System.String)dataRow["DataSourceID"];
			entity.DataSourceName = (Convert.IsDBNull(dataRow["DataSourceName"]))?string.Empty:(System.String)dataRow["DataSourceName"];
			entity.GeneralRemarks = (Convert.IsDBNull(dataRow["GeneralRemarks"]))?string.Empty:(System.String)dataRow["GeneralRemarks"];
			entity.ArchitectureStatus = (Convert.IsDBNull(dataRow["ArchitectureStatus"]))?string.Empty:(System.String)dataRow["ArchitectureStatus"];
			entity.ArchitectureStatusDate = (Convert.IsDBNull(dataRow["ArchitectureStatusDate"]))?string.Empty:(System.String)dataRow["ArchitectureStatusDate"];
			entity.DesignRationale = (Convert.IsDBNull(dataRow["DesignRationale"]))?string.Empty:(System.String)dataRow["DesignRationale"];
			entity.OrganizationalUnitType = (Convert.IsDBNull(dataRow["OrganizationalUnitType"]))?string.Empty:(System.String)dataRow["OrganizationalUnitType"];
			entity.TelephoneNumber = (Convert.IsDBNull(dataRow["TelephoneNumber"]))?string.Empty:(System.String)dataRow["TelephoneNumber"];
			entity.FaxNumber = (Convert.IsDBNull(dataRow["FaxNumber"]))?string.Empty:(System.String)dataRow["FaxNumber"];
			entity.EmailAddress = (Convert.IsDBNull(dataRow["EmailAddress"]))?string.Empty:(System.String)dataRow["EmailAddress"];
			entity.Headcount = (Convert.IsDBNull(dataRow["Headcount"]))?string.Empty:(System.String)dataRow["Headcount"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAView_OrganizationalUnit_RetrievalFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_OrganizationalUnit_Retrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_OrganizationalUnit_RetrievalFilterBuilder : SqlFilterBuilder<METAView_OrganizationalUnit_RetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_OrganizationalUnit_RetrievalFilterBuilder class.
		/// </summary>
		public METAView_OrganizationalUnit_RetrievalFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_OrganizationalUnit_RetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_OrganizationalUnit_RetrievalFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_OrganizationalUnit_RetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_OrganizationalUnit_RetrievalFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_OrganizationalUnit_RetrievalFilterBuilder

	#region METAView_OrganizationalUnit_RetrievalParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_OrganizationalUnit_Retrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_OrganizationalUnit_RetrievalParameterBuilder : ParameterizedSqlFilterBuilder<METAView_OrganizationalUnit_RetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_OrganizationalUnit_RetrievalParameterBuilder class.
		/// </summary>
		public METAView_OrganizationalUnit_RetrievalParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_OrganizationalUnit_RetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_OrganizationalUnit_RetrievalParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_OrganizationalUnit_RetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_OrganizationalUnit_RetrievalParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_OrganizationalUnit_RetrievalParameterBuilder
	
	#region METAView_OrganizationalUnit_RetrievalSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_OrganizationalUnit_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_OrganizationalUnit_RetrievalSortBuilder : SqlSortBuilder<METAView_OrganizationalUnit_RetrievalColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_OrganizationalUnit_RetrievalSqlSortBuilder class.
		/// </summary>
		public METAView_OrganizationalUnit_RetrievalSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAView_OrganizationalUnit_RetrievalSortBuilder

} // end namespace
