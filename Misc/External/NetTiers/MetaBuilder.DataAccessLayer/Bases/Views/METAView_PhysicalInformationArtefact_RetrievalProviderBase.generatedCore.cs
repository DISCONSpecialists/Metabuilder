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
	/// This class is the base class for any <see cref="METAView_PhysicalInformationArtefact_RetrievalProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAView_PhysicalInformationArtefact_RetrievalProviderBaseCore : EntityViewProviderBase<METAView_PhysicalInformationArtefact_Retrieval>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAView_PhysicalInformationArtefact_Retrieval&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAView_PhysicalInformationArtefact_Retrieval&gt;"/></returns>
		protected static VList&lt;METAView_PhysicalInformationArtefact_Retrieval&gt; Fill(DataSet dataSet, VList<METAView_PhysicalInformationArtefact_Retrieval> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAView_PhysicalInformationArtefact_Retrieval>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAView_PhysicalInformationArtefact_Retrieval&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAView_PhysicalInformationArtefact_Retrieval>"/></returns>
		protected static VList&lt;METAView_PhysicalInformationArtefact_Retrieval&gt; Fill(DataTable dataTable, VList<METAView_PhysicalInformationArtefact_Retrieval> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAView_PhysicalInformationArtefact_Retrieval c = new METAView_PhysicalInformationArtefact_Retrieval();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeId = (Convert.IsDBNull(row["WorkspaceTypeId"]))?(int)0:(System.Int32?)row["WorkspaceTypeId"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.PhysicalInformationArtefactType = (Convert.IsDBNull(row["PhysicalInformationArtefactType"]))?string.Empty:(System.String)row["PhysicalInformationArtefactType"];
					c.FormatStandard = (Convert.IsDBNull(row["FormatStandard"]))?string.Empty:(System.String)row["FormatStandard"];
					c.ContentType = (Convert.IsDBNull(row["ContentType"]))?string.Empty:(System.String)row["ContentType"];
					c.SecurityClassification = (Convert.IsDBNull(row["SecurityClassification"]))?string.Empty:(System.String)row["SecurityClassification"];
					c.IsRecord = (Convert.IsDBNull(row["IsRecord"]))?string.Empty:(System.String)row["IsRecord"];
					c.Name = (Convert.IsDBNull(row["Name"]))?string.Empty:(System.String)row["Name"];
					c.Description = (Convert.IsDBNull(row["Description"]))?string.Empty:(System.String)row["Description"];
					c.Abbreviation = (Convert.IsDBNull(row["Abbreviation"]))?string.Empty:(System.String)row["Abbreviation"];
					c.IsBusinessExternal = (Convert.IsDBNull(row["IsBusinessExternal"]))?string.Empty:(System.String)row["IsBusinessExternal"];
					c.StandardisationStatus = (Convert.IsDBNull(row["StandardisationStatus"]))?string.Empty:(System.String)row["StandardisationStatus"];
					c.StandardisationStatusDate = (Convert.IsDBNull(row["StandardisationStatusDate"]))?string.Empty:(System.String)row["StandardisationStatusDate"];
					c.DesignRationale = (Convert.IsDBNull(row["DesignRationale"]))?string.Empty:(System.String)row["DesignRationale"];
					c.GeneralRemarks = (Convert.IsDBNull(row["GeneralRemarks"]))?string.Empty:(System.String)row["GeneralRemarks"];
					c.GapType = (Convert.IsDBNull(row["GapType"]))?string.Empty:(System.String)row["GapType"];
					c.DataSourceID = (Convert.IsDBNull(row["DataSourceID"]))?string.Empty:(System.String)row["DataSourceID"];
					c.DataSourceName = (Convert.IsDBNull(row["DataSourceName"]))?string.Empty:(System.String)row["DataSourceName"];
					c.ArchitectureStatus = (Convert.IsDBNull(row["ArchitectureStatus"]))?string.Empty:(System.String)row["ArchitectureStatus"];
					c.ArchitectureStatusDate = (Convert.IsDBNull(row["ArchitectureStatusDate"]))?string.Empty:(System.String)row["ArchitectureStatusDate"];
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
		/// Fill an <see cref="VList&lt;METAView_PhysicalInformationArtefact_Retrieval&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAView_PhysicalInformationArtefact_Retrieval&gt;"/></returns>
		protected VList<METAView_PhysicalInformationArtefact_Retrieval> Fill(IDataReader reader, VList<METAView_PhysicalInformationArtefact_Retrieval> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAView_PhysicalInformationArtefact_Retrieval entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAView_PhysicalInformationArtefact_Retrieval>("METAView_PhysicalInformationArtefact_Retrieval",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAView_PhysicalInformationArtefact_Retrieval();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.WorkspaceTypeId)];
					//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
					entity.VCStatusID = (System.Int32)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.pkid = (System.Int32)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.pkid)];
					//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.PhysicalInformationArtefactType = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.PhysicalInformationArtefactType)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.PhysicalInformationArtefactType)];
					//entity.PhysicalInformationArtefactType = (Convert.IsDBNull(reader["PhysicalInformationArtefactType"]))?string.Empty:(System.String)reader["PhysicalInformationArtefactType"];
					entity.FormatStandard = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.FormatStandard)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.FormatStandard)];
					//entity.FormatStandard = (Convert.IsDBNull(reader["FormatStandard"]))?string.Empty:(System.String)reader["FormatStandard"];
					entity.ContentType = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.ContentType)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.ContentType)];
					//entity.ContentType = (Convert.IsDBNull(reader["ContentType"]))?string.Empty:(System.String)reader["ContentType"];
					entity.SecurityClassification = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.SecurityClassification)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.SecurityClassification)];
					//entity.SecurityClassification = (Convert.IsDBNull(reader["SecurityClassification"]))?string.Empty:(System.String)reader["SecurityClassification"];
					entity.IsRecord = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.IsRecord)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.IsRecord)];
					//entity.IsRecord = (Convert.IsDBNull(reader["IsRecord"]))?string.Empty:(System.String)reader["IsRecord"];
					entity.Name = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.Name)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.Name)];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.Description = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.Description)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.Description)];
					//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
					entity.Abbreviation = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.Abbreviation)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.Abbreviation)];
					//entity.Abbreviation = (Convert.IsDBNull(reader["Abbreviation"]))?string.Empty:(System.String)reader["Abbreviation"];
					entity.IsBusinessExternal = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.IsBusinessExternal)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.IsBusinessExternal)];
					//entity.IsBusinessExternal = (Convert.IsDBNull(reader["IsBusinessExternal"]))?string.Empty:(System.String)reader["IsBusinessExternal"];
					entity.StandardisationStatus = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.StandardisationStatus)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.StandardisationStatus)];
					//entity.StandardisationStatus = (Convert.IsDBNull(reader["StandardisationStatus"]))?string.Empty:(System.String)reader["StandardisationStatus"];
					entity.StandardisationStatusDate = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.StandardisationStatusDate)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.StandardisationStatusDate)];
					//entity.StandardisationStatusDate = (Convert.IsDBNull(reader["StandardisationStatusDate"]))?string.Empty:(System.String)reader["StandardisationStatusDate"];
					entity.DesignRationale = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.DesignRationale)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.DesignRationale)];
					//entity.DesignRationale = (Convert.IsDBNull(reader["DesignRationale"]))?string.Empty:(System.String)reader["DesignRationale"];
					entity.GeneralRemarks = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.GeneralRemarks)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.GeneralRemarks)];
					//entity.GeneralRemarks = (Convert.IsDBNull(reader["GeneralRemarks"]))?string.Empty:(System.String)reader["GeneralRemarks"];
					entity.GapType = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.GapType)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.GapType)];
					//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
					entity.DataSourceID = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.DataSourceID)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.DataSourceID)];
					//entity.DataSourceID = (Convert.IsDBNull(reader["DataSourceID"]))?string.Empty:(System.String)reader["DataSourceID"];
					entity.DataSourceName = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.DataSourceName)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.DataSourceName)];
					//entity.DataSourceName = (Convert.IsDBNull(reader["DataSourceName"]))?string.Empty:(System.String)reader["DataSourceName"];
					entity.ArchitectureStatus = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.ArchitectureStatus)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.ArchitectureStatus)];
					//entity.ArchitectureStatus = (Convert.IsDBNull(reader["ArchitectureStatus"]))?string.Empty:(System.String)reader["ArchitectureStatus"];
					entity.ArchitectureStatusDate = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.ArchitectureStatusDate)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.ArchitectureStatusDate)];
					//entity.ArchitectureStatusDate = (Convert.IsDBNull(reader["ArchitectureStatusDate"]))?string.Empty:(System.String)reader["ArchitectureStatusDate"];
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
		/// Refreshes the <see cref="METAView_PhysicalInformationArtefact_Retrieval"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_PhysicalInformationArtefact_Retrieval"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAView_PhysicalInformationArtefact_Retrieval entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.WorkspaceTypeId)];
			//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
			entity.VCStatusID = (System.Int32)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.pkid = (System.Int32)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.pkid)];
			//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.PhysicalInformationArtefactType = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.PhysicalInformationArtefactType)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.PhysicalInformationArtefactType)];
			//entity.PhysicalInformationArtefactType = (Convert.IsDBNull(reader["PhysicalInformationArtefactType"]))?string.Empty:(System.String)reader["PhysicalInformationArtefactType"];
			entity.FormatStandard = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.FormatStandard)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.FormatStandard)];
			//entity.FormatStandard = (Convert.IsDBNull(reader["FormatStandard"]))?string.Empty:(System.String)reader["FormatStandard"];
			entity.ContentType = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.ContentType)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.ContentType)];
			//entity.ContentType = (Convert.IsDBNull(reader["ContentType"]))?string.Empty:(System.String)reader["ContentType"];
			entity.SecurityClassification = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.SecurityClassification)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.SecurityClassification)];
			//entity.SecurityClassification = (Convert.IsDBNull(reader["SecurityClassification"]))?string.Empty:(System.String)reader["SecurityClassification"];
			entity.IsRecord = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.IsRecord)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.IsRecord)];
			//entity.IsRecord = (Convert.IsDBNull(reader["IsRecord"]))?string.Empty:(System.String)reader["IsRecord"];
			entity.Name = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.Name)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.Name)];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.Description = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.Description)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.Description)];
			//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
			entity.Abbreviation = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.Abbreviation)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.Abbreviation)];
			//entity.Abbreviation = (Convert.IsDBNull(reader["Abbreviation"]))?string.Empty:(System.String)reader["Abbreviation"];
			entity.IsBusinessExternal = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.IsBusinessExternal)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.IsBusinessExternal)];
			//entity.IsBusinessExternal = (Convert.IsDBNull(reader["IsBusinessExternal"]))?string.Empty:(System.String)reader["IsBusinessExternal"];
			entity.StandardisationStatus = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.StandardisationStatus)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.StandardisationStatus)];
			//entity.StandardisationStatus = (Convert.IsDBNull(reader["StandardisationStatus"]))?string.Empty:(System.String)reader["StandardisationStatus"];
			entity.StandardisationStatusDate = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.StandardisationStatusDate)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.StandardisationStatusDate)];
			//entity.StandardisationStatusDate = (Convert.IsDBNull(reader["StandardisationStatusDate"]))?string.Empty:(System.String)reader["StandardisationStatusDate"];
			entity.DesignRationale = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.DesignRationale)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.DesignRationale)];
			//entity.DesignRationale = (Convert.IsDBNull(reader["DesignRationale"]))?string.Empty:(System.String)reader["DesignRationale"];
			entity.GeneralRemarks = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.GeneralRemarks)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.GeneralRemarks)];
			//entity.GeneralRemarks = (Convert.IsDBNull(reader["GeneralRemarks"]))?string.Empty:(System.String)reader["GeneralRemarks"];
			entity.GapType = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.GapType)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			entity.DataSourceID = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.DataSourceID)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.DataSourceID)];
			//entity.DataSourceID = (Convert.IsDBNull(reader["DataSourceID"]))?string.Empty:(System.String)reader["DataSourceID"];
			entity.DataSourceName = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.DataSourceName)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.DataSourceName)];
			//entity.DataSourceName = (Convert.IsDBNull(reader["DataSourceName"]))?string.Empty:(System.String)reader["DataSourceName"];
			entity.ArchitectureStatus = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.ArchitectureStatus)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.ArchitectureStatus)];
			//entity.ArchitectureStatus = (Convert.IsDBNull(reader["ArchitectureStatus"]))?string.Empty:(System.String)reader["ArchitectureStatus"];
			entity.ArchitectureStatusDate = (reader.IsDBNull(((int)METAView_PhysicalInformationArtefact_RetrievalColumn.ArchitectureStatusDate)))?null:(System.String)reader[((int)METAView_PhysicalInformationArtefact_RetrievalColumn.ArchitectureStatusDate)];
			//entity.ArchitectureStatusDate = (Convert.IsDBNull(reader["ArchitectureStatusDate"]))?string.Empty:(System.String)reader["ArchitectureStatusDate"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAView_PhysicalInformationArtefact_Retrieval"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_PhysicalInformationArtefact_Retrieval"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAView_PhysicalInformationArtefact_Retrieval entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeId = (Convert.IsDBNull(dataRow["WorkspaceTypeId"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeId"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.PhysicalInformationArtefactType = (Convert.IsDBNull(dataRow["PhysicalInformationArtefactType"]))?string.Empty:(System.String)dataRow["PhysicalInformationArtefactType"];
			entity.FormatStandard = (Convert.IsDBNull(dataRow["FormatStandard"]))?string.Empty:(System.String)dataRow["FormatStandard"];
			entity.ContentType = (Convert.IsDBNull(dataRow["ContentType"]))?string.Empty:(System.String)dataRow["ContentType"];
			entity.SecurityClassification = (Convert.IsDBNull(dataRow["SecurityClassification"]))?string.Empty:(System.String)dataRow["SecurityClassification"];
			entity.IsRecord = (Convert.IsDBNull(dataRow["IsRecord"]))?string.Empty:(System.String)dataRow["IsRecord"];
			entity.Name = (Convert.IsDBNull(dataRow["Name"]))?string.Empty:(System.String)dataRow["Name"];
			entity.Description = (Convert.IsDBNull(dataRow["Description"]))?string.Empty:(System.String)dataRow["Description"];
			entity.Abbreviation = (Convert.IsDBNull(dataRow["Abbreviation"]))?string.Empty:(System.String)dataRow["Abbreviation"];
			entity.IsBusinessExternal = (Convert.IsDBNull(dataRow["IsBusinessExternal"]))?string.Empty:(System.String)dataRow["IsBusinessExternal"];
			entity.StandardisationStatus = (Convert.IsDBNull(dataRow["StandardisationStatus"]))?string.Empty:(System.String)dataRow["StandardisationStatus"];
			entity.StandardisationStatusDate = (Convert.IsDBNull(dataRow["StandardisationStatusDate"]))?string.Empty:(System.String)dataRow["StandardisationStatusDate"];
			entity.DesignRationale = (Convert.IsDBNull(dataRow["DesignRationale"]))?string.Empty:(System.String)dataRow["DesignRationale"];
			entity.GeneralRemarks = (Convert.IsDBNull(dataRow["GeneralRemarks"]))?string.Empty:(System.String)dataRow["GeneralRemarks"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.DataSourceID = (Convert.IsDBNull(dataRow["DataSourceID"]))?string.Empty:(System.String)dataRow["DataSourceID"];
			entity.DataSourceName = (Convert.IsDBNull(dataRow["DataSourceName"]))?string.Empty:(System.String)dataRow["DataSourceName"];
			entity.ArchitectureStatus = (Convert.IsDBNull(dataRow["ArchitectureStatus"]))?string.Empty:(System.String)dataRow["ArchitectureStatus"];
			entity.ArchitectureStatusDate = (Convert.IsDBNull(dataRow["ArchitectureStatusDate"]))?string.Empty:(System.String)dataRow["ArchitectureStatusDate"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAView_PhysicalInformationArtefact_RetrievalFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_PhysicalInformationArtefact_Retrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_PhysicalInformationArtefact_RetrievalFilterBuilder : SqlFilterBuilder<METAView_PhysicalInformationArtefact_RetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_PhysicalInformationArtefact_RetrievalFilterBuilder class.
		/// </summary>
		public METAView_PhysicalInformationArtefact_RetrievalFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_PhysicalInformationArtefact_RetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_PhysicalInformationArtefact_RetrievalFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_PhysicalInformationArtefact_RetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_PhysicalInformationArtefact_RetrievalFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_PhysicalInformationArtefact_RetrievalFilterBuilder

	#region METAView_PhysicalInformationArtefact_RetrievalParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_PhysicalInformationArtefact_Retrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_PhysicalInformationArtefact_RetrievalParameterBuilder : ParameterizedSqlFilterBuilder<METAView_PhysicalInformationArtefact_RetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_PhysicalInformationArtefact_RetrievalParameterBuilder class.
		/// </summary>
		public METAView_PhysicalInformationArtefact_RetrievalParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_PhysicalInformationArtefact_RetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_PhysicalInformationArtefact_RetrievalParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_PhysicalInformationArtefact_RetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_PhysicalInformationArtefact_RetrievalParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_PhysicalInformationArtefact_RetrievalParameterBuilder
	
	#region METAView_PhysicalInformationArtefact_RetrievalSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_PhysicalInformationArtefact_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_PhysicalInformationArtefact_RetrievalSortBuilder : SqlSortBuilder<METAView_PhysicalInformationArtefact_RetrievalColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_PhysicalInformationArtefact_RetrievalSqlSortBuilder class.
		/// </summary>
		public METAView_PhysicalInformationArtefact_RetrievalSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAView_PhysicalInformationArtefact_RetrievalSortBuilder

} // end namespace
