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
	/// This class is the base class for any <see cref="METAView_Person_RetrievalProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAView_Person_RetrievalProviderBaseCore : EntityViewProviderBase<METAView_Person_Retrieval>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAView_Person_Retrieval&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAView_Person_Retrieval&gt;"/></returns>
		protected static VList&lt;METAView_Person_Retrieval&gt; Fill(DataSet dataSet, VList<METAView_Person_Retrieval> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAView_Person_Retrieval>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAView_Person_Retrieval&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAView_Person_Retrieval>"/></returns>
		protected static VList&lt;METAView_Person_Retrieval&gt; Fill(DataTable dataTable, VList<METAView_Person_Retrieval> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAView_Person_Retrieval c = new METAView_Person_Retrieval();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeId = (Convert.IsDBNull(row["WorkspaceTypeId"]))?(int)0:(System.Int32?)row["WorkspaceTypeId"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.Name = (Convert.IsDBNull(row["Name"]))?string.Empty:(System.String)row["Name"];
					c.Abbreviation = (Convert.IsDBNull(row["Abbreviation"]))?string.Empty:(System.String)row["Abbreviation"];
					c.Description = (Convert.IsDBNull(row["Description"]))?string.Empty:(System.String)row["Description"];
					c.IsBusinessExternal = (Convert.IsDBNull(row["IsBusinessExternal"]))?string.Empty:(System.String)row["IsBusinessExternal"];
					c.ArchitectureStatus = (Convert.IsDBNull(row["ArchitectureStatus"]))?string.Empty:(System.String)row["ArchitectureStatus"];
					c.ArchitectureStatusDate = (Convert.IsDBNull(row["ArchitectureStatusDate"]))?string.Empty:(System.String)row["ArchitectureStatusDate"];
					c.DesignRationale = (Convert.IsDBNull(row["DesignRationale"]))?string.Empty:(System.String)row["DesignRationale"];
					c.GeneralRemarks = (Convert.IsDBNull(row["GeneralRemarks"]))?string.Empty:(System.String)row["GeneralRemarks"];
					c.GapType = (Convert.IsDBNull(row["GapType"]))?string.Empty:(System.String)row["GapType"];
					c.DataSourceID = (Convert.IsDBNull(row["DataSourceID"]))?string.Empty:(System.String)row["DataSourceID"];
					c.DataSourceName = (Convert.IsDBNull(row["DataSourceName"]))?string.Empty:(System.String)row["DataSourceName"];
					c.PersonType = (Convert.IsDBNull(row["PersonType"]))?string.Empty:(System.String)row["PersonType"];
					c.Surname = (Convert.IsDBNull(row["Surname"]))?string.Empty:(System.String)row["Surname"];
					c.OtherInitials = (Convert.IsDBNull(row["OtherInitials"]))?string.Empty:(System.String)row["OtherInitials"];
					c.Title = (Convert.IsDBNull(row["Title"]))?string.Empty:(System.String)row["Title"];
					c.EmployeeNumber = (Convert.IsDBNull(row["EmployeeNumber"]))?string.Empty:(System.String)row["EmployeeNumber"];
					c.IDOrPassportNumber = (Convert.IsDBNull(row["IDOrPassportNumber"]))?string.Empty:(System.String)row["IDOrPassportNumber"];
					c.IDNumber = (Convert.IsDBNull(row["IDNumber"]))?string.Empty:(System.String)row["IDNumber"];
					c.TelephoneNumber = (Convert.IsDBNull(row["TelephoneNumber"]))?string.Empty:(System.String)row["TelephoneNumber"];
					c.Telephone = (Convert.IsDBNull(row["Telephone"]))?string.Empty:(System.String)row["Telephone"];
					c.FaxNumber = (Convert.IsDBNull(row["FaxNumber"]))?string.Empty:(System.String)row["FaxNumber"];
					c.EmailAddress = (Convert.IsDBNull(row["EmailAddress"]))?string.Empty:(System.String)row["EmailAddress"];
					c.Email = (Convert.IsDBNull(row["Email"]))?string.Empty:(System.String)row["Email"];
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
		/// Fill an <see cref="VList&lt;METAView_Person_Retrieval&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAView_Person_Retrieval&gt;"/></returns>
		protected VList<METAView_Person_Retrieval> Fill(IDataReader reader, VList<METAView_Person_Retrieval> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAView_Person_Retrieval entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAView_Person_Retrieval>("METAView_Person_Retrieval",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAView_Person_Retrieval();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAView_Person_RetrievalColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_Person_RetrievalColumn.WorkspaceTypeId)];
					//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
					entity.VCStatusID = (System.Int32)reader[((int)METAView_Person_RetrievalColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.pkid = (System.Int32)reader[((int)METAView_Person_RetrievalColumn.pkid)];
					//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAView_Person_RetrievalColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Name = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.Name)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.Name)];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.Abbreviation = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.Abbreviation)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.Abbreviation)];
					//entity.Abbreviation = (Convert.IsDBNull(reader["Abbreviation"]))?string.Empty:(System.String)reader["Abbreviation"];
					entity.Description = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.Description)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.Description)];
					//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
					entity.IsBusinessExternal = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.IsBusinessExternal)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.IsBusinessExternal)];
					//entity.IsBusinessExternal = (Convert.IsDBNull(reader["IsBusinessExternal"]))?string.Empty:(System.String)reader["IsBusinessExternal"];
					entity.ArchitectureStatus = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.ArchitectureStatus)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.ArchitectureStatus)];
					//entity.ArchitectureStatus = (Convert.IsDBNull(reader["ArchitectureStatus"]))?string.Empty:(System.String)reader["ArchitectureStatus"];
					entity.ArchitectureStatusDate = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.ArchitectureStatusDate)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.ArchitectureStatusDate)];
					//entity.ArchitectureStatusDate = (Convert.IsDBNull(reader["ArchitectureStatusDate"]))?string.Empty:(System.String)reader["ArchitectureStatusDate"];
					entity.DesignRationale = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.DesignRationale)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.DesignRationale)];
					//entity.DesignRationale = (Convert.IsDBNull(reader["DesignRationale"]))?string.Empty:(System.String)reader["DesignRationale"];
					entity.GeneralRemarks = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.GeneralRemarks)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.GeneralRemarks)];
					//entity.GeneralRemarks = (Convert.IsDBNull(reader["GeneralRemarks"]))?string.Empty:(System.String)reader["GeneralRemarks"];
					entity.GapType = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.GapType)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.GapType)];
					//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
					entity.DataSourceID = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.DataSourceID)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.DataSourceID)];
					//entity.DataSourceID = (Convert.IsDBNull(reader["DataSourceID"]))?string.Empty:(System.String)reader["DataSourceID"];
					entity.DataSourceName = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.DataSourceName)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.DataSourceName)];
					//entity.DataSourceName = (Convert.IsDBNull(reader["DataSourceName"]))?string.Empty:(System.String)reader["DataSourceName"];
					entity.PersonType = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.PersonType)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.PersonType)];
					//entity.PersonType = (Convert.IsDBNull(reader["PersonType"]))?string.Empty:(System.String)reader["PersonType"];
					entity.Surname = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.Surname)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.Surname)];
					//entity.Surname = (Convert.IsDBNull(reader["Surname"]))?string.Empty:(System.String)reader["Surname"];
					entity.OtherInitials = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.OtherInitials)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.OtherInitials)];
					//entity.OtherInitials = (Convert.IsDBNull(reader["OtherInitials"]))?string.Empty:(System.String)reader["OtherInitials"];
					entity.Title = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.Title)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.Title)];
					//entity.Title = (Convert.IsDBNull(reader["Title"]))?string.Empty:(System.String)reader["Title"];
					entity.EmployeeNumber = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.EmployeeNumber)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.EmployeeNumber)];
					//entity.EmployeeNumber = (Convert.IsDBNull(reader["EmployeeNumber"]))?string.Empty:(System.String)reader["EmployeeNumber"];
					entity.IDOrPassportNumber = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.IDOrPassportNumber)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.IDOrPassportNumber)];
					//entity.IDOrPassportNumber = (Convert.IsDBNull(reader["IDOrPassportNumber"]))?string.Empty:(System.String)reader["IDOrPassportNumber"];
					entity.IDNumber = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.IDNumber)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.IDNumber)];
					//entity.IDNumber = (Convert.IsDBNull(reader["IDNumber"]))?string.Empty:(System.String)reader["IDNumber"];
					entity.TelephoneNumber = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.TelephoneNumber)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.TelephoneNumber)];
					//entity.TelephoneNumber = (Convert.IsDBNull(reader["TelephoneNumber"]))?string.Empty:(System.String)reader["TelephoneNumber"];
					entity.Telephone = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.Telephone)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.Telephone)];
					//entity.Telephone = (Convert.IsDBNull(reader["Telephone"]))?string.Empty:(System.String)reader["Telephone"];
					entity.FaxNumber = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.FaxNumber)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.FaxNumber)];
					//entity.FaxNumber = (Convert.IsDBNull(reader["FaxNumber"]))?string.Empty:(System.String)reader["FaxNumber"];
					entity.EmailAddress = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.EmailAddress)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.EmailAddress)];
					//entity.EmailAddress = (Convert.IsDBNull(reader["EmailAddress"]))?string.Empty:(System.String)reader["EmailAddress"];
					entity.Email = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.Email)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.Email)];
					//entity.Email = (Convert.IsDBNull(reader["Email"]))?string.Empty:(System.String)reader["Email"];
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
		/// Refreshes the <see cref="METAView_Person_Retrieval"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_Person_Retrieval"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAView_Person_Retrieval entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAView_Person_RetrievalColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_Person_RetrievalColumn.WorkspaceTypeId)];
			//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
			entity.VCStatusID = (System.Int32)reader[((int)METAView_Person_RetrievalColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.pkid = (System.Int32)reader[((int)METAView_Person_RetrievalColumn.pkid)];
			//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAView_Person_RetrievalColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Name = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.Name)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.Name)];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.Abbreviation = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.Abbreviation)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.Abbreviation)];
			//entity.Abbreviation = (Convert.IsDBNull(reader["Abbreviation"]))?string.Empty:(System.String)reader["Abbreviation"];
			entity.Description = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.Description)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.Description)];
			//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
			entity.IsBusinessExternal = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.IsBusinessExternal)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.IsBusinessExternal)];
			//entity.IsBusinessExternal = (Convert.IsDBNull(reader["IsBusinessExternal"]))?string.Empty:(System.String)reader["IsBusinessExternal"];
			entity.ArchitectureStatus = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.ArchitectureStatus)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.ArchitectureStatus)];
			//entity.ArchitectureStatus = (Convert.IsDBNull(reader["ArchitectureStatus"]))?string.Empty:(System.String)reader["ArchitectureStatus"];
			entity.ArchitectureStatusDate = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.ArchitectureStatusDate)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.ArchitectureStatusDate)];
			//entity.ArchitectureStatusDate = (Convert.IsDBNull(reader["ArchitectureStatusDate"]))?string.Empty:(System.String)reader["ArchitectureStatusDate"];
			entity.DesignRationale = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.DesignRationale)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.DesignRationale)];
			//entity.DesignRationale = (Convert.IsDBNull(reader["DesignRationale"]))?string.Empty:(System.String)reader["DesignRationale"];
			entity.GeneralRemarks = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.GeneralRemarks)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.GeneralRemarks)];
			//entity.GeneralRemarks = (Convert.IsDBNull(reader["GeneralRemarks"]))?string.Empty:(System.String)reader["GeneralRemarks"];
			entity.GapType = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.GapType)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			entity.DataSourceID = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.DataSourceID)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.DataSourceID)];
			//entity.DataSourceID = (Convert.IsDBNull(reader["DataSourceID"]))?string.Empty:(System.String)reader["DataSourceID"];
			entity.DataSourceName = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.DataSourceName)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.DataSourceName)];
			//entity.DataSourceName = (Convert.IsDBNull(reader["DataSourceName"]))?string.Empty:(System.String)reader["DataSourceName"];
			entity.PersonType = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.PersonType)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.PersonType)];
			//entity.PersonType = (Convert.IsDBNull(reader["PersonType"]))?string.Empty:(System.String)reader["PersonType"];
			entity.Surname = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.Surname)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.Surname)];
			//entity.Surname = (Convert.IsDBNull(reader["Surname"]))?string.Empty:(System.String)reader["Surname"];
			entity.OtherInitials = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.OtherInitials)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.OtherInitials)];
			//entity.OtherInitials = (Convert.IsDBNull(reader["OtherInitials"]))?string.Empty:(System.String)reader["OtherInitials"];
			entity.Title = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.Title)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.Title)];
			//entity.Title = (Convert.IsDBNull(reader["Title"]))?string.Empty:(System.String)reader["Title"];
			entity.EmployeeNumber = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.EmployeeNumber)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.EmployeeNumber)];
			//entity.EmployeeNumber = (Convert.IsDBNull(reader["EmployeeNumber"]))?string.Empty:(System.String)reader["EmployeeNumber"];
			entity.IDOrPassportNumber = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.IDOrPassportNumber)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.IDOrPassportNumber)];
			//entity.IDOrPassportNumber = (Convert.IsDBNull(reader["IDOrPassportNumber"]))?string.Empty:(System.String)reader["IDOrPassportNumber"];
			entity.IDNumber = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.IDNumber)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.IDNumber)];
			//entity.IDNumber = (Convert.IsDBNull(reader["IDNumber"]))?string.Empty:(System.String)reader["IDNumber"];
			entity.TelephoneNumber = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.TelephoneNumber)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.TelephoneNumber)];
			//entity.TelephoneNumber = (Convert.IsDBNull(reader["TelephoneNumber"]))?string.Empty:(System.String)reader["TelephoneNumber"];
			entity.Telephone = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.Telephone)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.Telephone)];
			//entity.Telephone = (Convert.IsDBNull(reader["Telephone"]))?string.Empty:(System.String)reader["Telephone"];
			entity.FaxNumber = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.FaxNumber)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.FaxNumber)];
			//entity.FaxNumber = (Convert.IsDBNull(reader["FaxNumber"]))?string.Empty:(System.String)reader["FaxNumber"];
			entity.EmailAddress = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.EmailAddress)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.EmailAddress)];
			//entity.EmailAddress = (Convert.IsDBNull(reader["EmailAddress"]))?string.Empty:(System.String)reader["EmailAddress"];
			entity.Email = (reader.IsDBNull(((int)METAView_Person_RetrievalColumn.Email)))?null:(System.String)reader[((int)METAView_Person_RetrievalColumn.Email)];
			//entity.Email = (Convert.IsDBNull(reader["Email"]))?string.Empty:(System.String)reader["Email"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAView_Person_Retrieval"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_Person_Retrieval"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAView_Person_Retrieval entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeId = (Convert.IsDBNull(dataRow["WorkspaceTypeId"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeId"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.Name = (Convert.IsDBNull(dataRow["Name"]))?string.Empty:(System.String)dataRow["Name"];
			entity.Abbreviation = (Convert.IsDBNull(dataRow["Abbreviation"]))?string.Empty:(System.String)dataRow["Abbreviation"];
			entity.Description = (Convert.IsDBNull(dataRow["Description"]))?string.Empty:(System.String)dataRow["Description"];
			entity.IsBusinessExternal = (Convert.IsDBNull(dataRow["IsBusinessExternal"]))?string.Empty:(System.String)dataRow["IsBusinessExternal"];
			entity.ArchitectureStatus = (Convert.IsDBNull(dataRow["ArchitectureStatus"]))?string.Empty:(System.String)dataRow["ArchitectureStatus"];
			entity.ArchitectureStatusDate = (Convert.IsDBNull(dataRow["ArchitectureStatusDate"]))?string.Empty:(System.String)dataRow["ArchitectureStatusDate"];
			entity.DesignRationale = (Convert.IsDBNull(dataRow["DesignRationale"]))?string.Empty:(System.String)dataRow["DesignRationale"];
			entity.GeneralRemarks = (Convert.IsDBNull(dataRow["GeneralRemarks"]))?string.Empty:(System.String)dataRow["GeneralRemarks"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.DataSourceID = (Convert.IsDBNull(dataRow["DataSourceID"]))?string.Empty:(System.String)dataRow["DataSourceID"];
			entity.DataSourceName = (Convert.IsDBNull(dataRow["DataSourceName"]))?string.Empty:(System.String)dataRow["DataSourceName"];
			entity.PersonType = (Convert.IsDBNull(dataRow["PersonType"]))?string.Empty:(System.String)dataRow["PersonType"];
			entity.Surname = (Convert.IsDBNull(dataRow["Surname"]))?string.Empty:(System.String)dataRow["Surname"];
			entity.OtherInitials = (Convert.IsDBNull(dataRow["OtherInitials"]))?string.Empty:(System.String)dataRow["OtherInitials"];
			entity.Title = (Convert.IsDBNull(dataRow["Title"]))?string.Empty:(System.String)dataRow["Title"];
			entity.EmployeeNumber = (Convert.IsDBNull(dataRow["EmployeeNumber"]))?string.Empty:(System.String)dataRow["EmployeeNumber"];
			entity.IDOrPassportNumber = (Convert.IsDBNull(dataRow["IDOrPassportNumber"]))?string.Empty:(System.String)dataRow["IDOrPassportNumber"];
			entity.IDNumber = (Convert.IsDBNull(dataRow["IDNumber"]))?string.Empty:(System.String)dataRow["IDNumber"];
			entity.TelephoneNumber = (Convert.IsDBNull(dataRow["TelephoneNumber"]))?string.Empty:(System.String)dataRow["TelephoneNumber"];
			entity.Telephone = (Convert.IsDBNull(dataRow["Telephone"]))?string.Empty:(System.String)dataRow["Telephone"];
			entity.FaxNumber = (Convert.IsDBNull(dataRow["FaxNumber"]))?string.Empty:(System.String)dataRow["FaxNumber"];
			entity.EmailAddress = (Convert.IsDBNull(dataRow["EmailAddress"]))?string.Empty:(System.String)dataRow["EmailAddress"];
			entity.Email = (Convert.IsDBNull(dataRow["Email"]))?string.Empty:(System.String)dataRow["Email"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAView_Person_RetrievalFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_Person_Retrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_Person_RetrievalFilterBuilder : SqlFilterBuilder<METAView_Person_RetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_Person_RetrievalFilterBuilder class.
		/// </summary>
		public METAView_Person_RetrievalFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_Person_RetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_Person_RetrievalFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_Person_RetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_Person_RetrievalFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_Person_RetrievalFilterBuilder

	#region METAView_Person_RetrievalParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_Person_Retrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_Person_RetrievalParameterBuilder : ParameterizedSqlFilterBuilder<METAView_Person_RetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_Person_RetrievalParameterBuilder class.
		/// </summary>
		public METAView_Person_RetrievalParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_Person_RetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_Person_RetrievalParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_Person_RetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_Person_RetrievalParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_Person_RetrievalParameterBuilder
	
	#region METAView_Person_RetrievalSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_Person_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Person_RetrievalSortBuilder : SqlSortBuilder<METAView_Person_RetrievalColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_Person_RetrievalSqlSortBuilder class.
		/// </summary>
		public METAView_Person_RetrievalSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAView_Person_RetrievalSortBuilder

} // end namespace
