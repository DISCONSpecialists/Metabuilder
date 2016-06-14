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
	/// This class is the base class for any <see cref="METAView_Function_RetrievalProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAView_Function_RetrievalProviderBaseCore : EntityViewProviderBase<METAView_Function_Retrieval>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAView_Function_Retrieval&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAView_Function_Retrieval&gt;"/></returns>
		protected static VList&lt;METAView_Function_Retrieval&gt; Fill(DataSet dataSet, VList<METAView_Function_Retrieval> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAView_Function_Retrieval>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAView_Function_Retrieval&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAView_Function_Retrieval>"/></returns>
		protected static VList&lt;METAView_Function_Retrieval&gt; Fill(DataTable dataTable, VList<METAView_Function_Retrieval> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAView_Function_Retrieval c = new METAView_Function_Retrieval();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeId = (Convert.IsDBNull(row["WorkspaceTypeId"]))?(int)0:(System.Int32?)row["WorkspaceTypeId"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.Name = (Convert.IsDBNull(row["Name"]))?string.Empty:(System.String)row["Name"];
					c.ContextualIndicator = (Convert.IsDBNull(row["ContextualIndicator"]))?string.Empty:(System.String)row["ContextualIndicator"];
					c.FunctionCriticality = (Convert.IsDBNull(row["FunctionCriticality"]))?string.Empty:(System.String)row["FunctionCriticality"];
					c.ManagementAbility = (Convert.IsDBNull(row["ManagementAbility"]))?string.Empty:(System.String)row["ManagementAbility"];
					c.InfoAvailability = (Convert.IsDBNull(row["InfoAvailability"]))?string.Empty:(System.String)row["InfoAvailability"];
					c.LegalAspects = (Convert.IsDBNull(row["LegalAspects"]))?string.Empty:(System.String)row["LegalAspects"];
					c.Technology = (Convert.IsDBNull(row["Technology"]))?string.Empty:(System.String)row["Technology"];
					c.Budget = (Convert.IsDBNull(row["Budget"]))?string.Empty:(System.String)row["Budget"];
					c.Energy = (Convert.IsDBNull(row["Energy"]))?string.Empty:(System.String)row["Energy"];
					c.RawMaterial = (Convert.IsDBNull(row["RawMaterial"]))?string.Empty:(System.String)row["RawMaterial"];
					c.SkillAvailability = (Convert.IsDBNull(row["SkillAvailability"]))?string.Empty:(System.String)row["SkillAvailability"];
					c.Efficiency = (Convert.IsDBNull(row["Efficiency"]))?string.Empty:(System.String)row["Efficiency"];
					c.Effectiveness = (Convert.IsDBNull(row["Effectiveness"]))?string.Empty:(System.String)row["Effectiveness"];
					c.Manpower = (Convert.IsDBNull(row["Manpower"]))?string.Empty:(System.String)row["Manpower"];
					c.CustomField1 = (Convert.IsDBNull(row["CustomField1"]))?string.Empty:(System.String)row["CustomField1"];
					c.CustomField2 = (Convert.IsDBNull(row["CustomField2"]))?string.Empty:(System.String)row["CustomField2"];
					c.CustomField3 = (Convert.IsDBNull(row["CustomField3"]))?string.Empty:(System.String)row["CustomField3"];
					c.EnvironmentInd = (Convert.IsDBNull(row["EnvironmentInd"]))?string.Empty:(System.String)row["EnvironmentInd"];
					c.GovernanceMech = (Convert.IsDBNull(row["GovernanceMech"]))?string.Empty:(System.String)row["GovernanceMech"];
					c.CapitalAvailability = (Convert.IsDBNull(row["CapitalAvailability"]))?string.Empty:(System.String)row["CapitalAvailability"];
					c.Competencies = (Convert.IsDBNull(row["Competencies"]))?string.Empty:(System.String)row["Competencies"];
					c.Ethics = (Convert.IsDBNull(row["Ethics"]))?string.Empty:(System.String)row["Ethics"];
					c.Facilities = (Convert.IsDBNull(row["Facilities"]))?string.Empty:(System.String)row["Facilities"];
					c.ServicesUSage = (Convert.IsDBNull(row["ServicesUSage"]))?string.Empty:(System.String)row["ServicesUSage"];
					c.Equipment = (Convert.IsDBNull(row["Equipment"]))?string.Empty:(System.String)row["Equipment"];
					c.TimeIndicator = (Convert.IsDBNull(row["TimeIndicator"]))?string.Empty:(System.String)row["TimeIndicator"];
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
					c.FunctionCategory = (Convert.IsDBNull(row["FunctionCategory"]))?string.Empty:(System.String)row["FunctionCategory"];
					c.OverallEfficiencyRating = (Convert.IsDBNull(row["OverallEfficiencyRating"]))?string.Empty:(System.String)row["OverallEfficiencyRating"];
					c.OverallEffectivenessRating = (Convert.IsDBNull(row["OverallEffectivenessRating"]))?string.Empty:(System.String)row["OverallEffectivenessRating"];
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
		/// Fill an <see cref="VList&lt;METAView_Function_Retrieval&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAView_Function_Retrieval&gt;"/></returns>
		protected VList<METAView_Function_Retrieval> Fill(IDataReader reader, VList<METAView_Function_Retrieval> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAView_Function_Retrieval entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAView_Function_Retrieval>("METAView_Function_Retrieval",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAView_Function_Retrieval();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAView_Function_RetrievalColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_Function_RetrievalColumn.WorkspaceTypeId)];
					//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
					entity.VCStatusID = (System.Int32)reader[((int)METAView_Function_RetrievalColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.pkid = (System.Int32)reader[((int)METAView_Function_RetrievalColumn.pkid)];
					//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAView_Function_RetrievalColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Name = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.Name)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.Name)];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.ContextualIndicator = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.ContextualIndicator)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.ContextualIndicator)];
					//entity.ContextualIndicator = (Convert.IsDBNull(reader["ContextualIndicator"]))?string.Empty:(System.String)reader["ContextualIndicator"];
					entity.FunctionCriticality = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.FunctionCriticality)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.FunctionCriticality)];
					//entity.FunctionCriticality = (Convert.IsDBNull(reader["FunctionCriticality"]))?string.Empty:(System.String)reader["FunctionCriticality"];
					entity.ManagementAbility = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.ManagementAbility)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.ManagementAbility)];
					//entity.ManagementAbility = (Convert.IsDBNull(reader["ManagementAbility"]))?string.Empty:(System.String)reader["ManagementAbility"];
					entity.InfoAvailability = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.InfoAvailability)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.InfoAvailability)];
					//entity.InfoAvailability = (Convert.IsDBNull(reader["InfoAvailability"]))?string.Empty:(System.String)reader["InfoAvailability"];
					entity.LegalAspects = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.LegalAspects)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.LegalAspects)];
					//entity.LegalAspects = (Convert.IsDBNull(reader["LegalAspects"]))?string.Empty:(System.String)reader["LegalAspects"];
					entity.Technology = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.Technology)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.Technology)];
					//entity.Technology = (Convert.IsDBNull(reader["Technology"]))?string.Empty:(System.String)reader["Technology"];
					entity.Budget = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.Budget)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.Budget)];
					//entity.Budget = (Convert.IsDBNull(reader["Budget"]))?string.Empty:(System.String)reader["Budget"];
					entity.Energy = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.Energy)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.Energy)];
					//entity.Energy = (Convert.IsDBNull(reader["Energy"]))?string.Empty:(System.String)reader["Energy"];
					entity.RawMaterial = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.RawMaterial)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.RawMaterial)];
					//entity.RawMaterial = (Convert.IsDBNull(reader["RawMaterial"]))?string.Empty:(System.String)reader["RawMaterial"];
					entity.SkillAvailability = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.SkillAvailability)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.SkillAvailability)];
					//entity.SkillAvailability = (Convert.IsDBNull(reader["SkillAvailability"]))?string.Empty:(System.String)reader["SkillAvailability"];
					entity.Efficiency = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.Efficiency)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.Efficiency)];
					//entity.Efficiency = (Convert.IsDBNull(reader["Efficiency"]))?string.Empty:(System.String)reader["Efficiency"];
					entity.Effectiveness = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.Effectiveness)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.Effectiveness)];
					//entity.Effectiveness = (Convert.IsDBNull(reader["Effectiveness"]))?string.Empty:(System.String)reader["Effectiveness"];
					entity.Manpower = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.Manpower)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.Manpower)];
					//entity.Manpower = (Convert.IsDBNull(reader["Manpower"]))?string.Empty:(System.String)reader["Manpower"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.EnvironmentInd = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.EnvironmentInd)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.EnvironmentInd)];
					//entity.EnvironmentInd = (Convert.IsDBNull(reader["EnvironmentInd"]))?string.Empty:(System.String)reader["EnvironmentInd"];
					entity.GovernanceMech = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.GovernanceMech)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.GovernanceMech)];
					//entity.GovernanceMech = (Convert.IsDBNull(reader["GovernanceMech"]))?string.Empty:(System.String)reader["GovernanceMech"];
					entity.CapitalAvailability = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.CapitalAvailability)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.CapitalAvailability)];
					//entity.CapitalAvailability = (Convert.IsDBNull(reader["CapitalAvailability"]))?string.Empty:(System.String)reader["CapitalAvailability"];
					entity.Competencies = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.Competencies)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.Competencies)];
					//entity.Competencies = (Convert.IsDBNull(reader["Competencies"]))?string.Empty:(System.String)reader["Competencies"];
					entity.Ethics = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.Ethics)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.Ethics)];
					//entity.Ethics = (Convert.IsDBNull(reader["Ethics"]))?string.Empty:(System.String)reader["Ethics"];
					entity.Facilities = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.Facilities)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.Facilities)];
					//entity.Facilities = (Convert.IsDBNull(reader["Facilities"]))?string.Empty:(System.String)reader["Facilities"];
					entity.ServicesUSage = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.ServicesUSage)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.ServicesUSage)];
					//entity.ServicesUSage = (Convert.IsDBNull(reader["ServicesUSage"]))?string.Empty:(System.String)reader["ServicesUSage"];
					entity.Equipment = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.Equipment)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.Equipment)];
					//entity.Equipment = (Convert.IsDBNull(reader["Equipment"]))?string.Empty:(System.String)reader["Equipment"];
					entity.TimeIndicator = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.TimeIndicator)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.TimeIndicator)];
					//entity.TimeIndicator = (Convert.IsDBNull(reader["TimeIndicator"]))?string.Empty:(System.String)reader["TimeIndicator"];
					entity.GapType = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.GapType)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.GapType)];
					//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
					entity.Description = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.Description)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.Description)];
					//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
					entity.Abbreviation = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.Abbreviation)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.Abbreviation)];
					//entity.Abbreviation = (Convert.IsDBNull(reader["Abbreviation"]))?string.Empty:(System.String)reader["Abbreviation"];
					entity.IsBusinessExternal = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.IsBusinessExternal)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.IsBusinessExternal)];
					//entity.IsBusinessExternal = (Convert.IsDBNull(reader["IsBusinessExternal"]))?string.Empty:(System.String)reader["IsBusinessExternal"];
					entity.StandardisationStatus = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.StandardisationStatus)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.StandardisationStatus)];
					//entity.StandardisationStatus = (Convert.IsDBNull(reader["StandardisationStatus"]))?string.Empty:(System.String)reader["StandardisationStatus"];
					entity.StandardisationStatusDate = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.StandardisationStatusDate)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.StandardisationStatusDate)];
					//entity.StandardisationStatusDate = (Convert.IsDBNull(reader["StandardisationStatusDate"]))?string.Empty:(System.String)reader["StandardisationStatusDate"];
					entity.DataSourceID = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.DataSourceID)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.DataSourceID)];
					//entity.DataSourceID = (Convert.IsDBNull(reader["DataSourceID"]))?string.Empty:(System.String)reader["DataSourceID"];
					entity.DataSourceName = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.DataSourceName)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.DataSourceName)];
					//entity.DataSourceName = (Convert.IsDBNull(reader["DataSourceName"]))?string.Empty:(System.String)reader["DataSourceName"];
					entity.GeneralRemarks = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.GeneralRemarks)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.GeneralRemarks)];
					//entity.GeneralRemarks = (Convert.IsDBNull(reader["GeneralRemarks"]))?string.Empty:(System.String)reader["GeneralRemarks"];
					entity.ArchitectureStatus = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.ArchitectureStatus)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.ArchitectureStatus)];
					//entity.ArchitectureStatus = (Convert.IsDBNull(reader["ArchitectureStatus"]))?string.Empty:(System.String)reader["ArchitectureStatus"];
					entity.ArchitectureStatusDate = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.ArchitectureStatusDate)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.ArchitectureStatusDate)];
					//entity.ArchitectureStatusDate = (Convert.IsDBNull(reader["ArchitectureStatusDate"]))?string.Empty:(System.String)reader["ArchitectureStatusDate"];
					entity.DesignRationale = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.DesignRationale)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.DesignRationale)];
					//entity.DesignRationale = (Convert.IsDBNull(reader["DesignRationale"]))?string.Empty:(System.String)reader["DesignRationale"];
					entity.FunctionCategory = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.FunctionCategory)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.FunctionCategory)];
					//entity.FunctionCategory = (Convert.IsDBNull(reader["FunctionCategory"]))?string.Empty:(System.String)reader["FunctionCategory"];
					entity.OverallEfficiencyRating = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.OverallEfficiencyRating)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.OverallEfficiencyRating)];
					//entity.OverallEfficiencyRating = (Convert.IsDBNull(reader["OverallEfficiencyRating"]))?string.Empty:(System.String)reader["OverallEfficiencyRating"];
					entity.OverallEffectivenessRating = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.OverallEffectivenessRating)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.OverallEffectivenessRating)];
					//entity.OverallEffectivenessRating = (Convert.IsDBNull(reader["OverallEffectivenessRating"]))?string.Empty:(System.String)reader["OverallEffectivenessRating"];
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
		/// Refreshes the <see cref="METAView_Function_Retrieval"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_Function_Retrieval"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAView_Function_Retrieval entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAView_Function_RetrievalColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_Function_RetrievalColumn.WorkspaceTypeId)];
			//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
			entity.VCStatusID = (System.Int32)reader[((int)METAView_Function_RetrievalColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.pkid = (System.Int32)reader[((int)METAView_Function_RetrievalColumn.pkid)];
			//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAView_Function_RetrievalColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Name = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.Name)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.Name)];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.ContextualIndicator = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.ContextualIndicator)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.ContextualIndicator)];
			//entity.ContextualIndicator = (Convert.IsDBNull(reader["ContextualIndicator"]))?string.Empty:(System.String)reader["ContextualIndicator"];
			entity.FunctionCriticality = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.FunctionCriticality)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.FunctionCriticality)];
			//entity.FunctionCriticality = (Convert.IsDBNull(reader["FunctionCriticality"]))?string.Empty:(System.String)reader["FunctionCriticality"];
			entity.ManagementAbility = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.ManagementAbility)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.ManagementAbility)];
			//entity.ManagementAbility = (Convert.IsDBNull(reader["ManagementAbility"]))?string.Empty:(System.String)reader["ManagementAbility"];
			entity.InfoAvailability = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.InfoAvailability)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.InfoAvailability)];
			//entity.InfoAvailability = (Convert.IsDBNull(reader["InfoAvailability"]))?string.Empty:(System.String)reader["InfoAvailability"];
			entity.LegalAspects = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.LegalAspects)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.LegalAspects)];
			//entity.LegalAspects = (Convert.IsDBNull(reader["LegalAspects"]))?string.Empty:(System.String)reader["LegalAspects"];
			entity.Technology = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.Technology)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.Technology)];
			//entity.Technology = (Convert.IsDBNull(reader["Technology"]))?string.Empty:(System.String)reader["Technology"];
			entity.Budget = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.Budget)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.Budget)];
			//entity.Budget = (Convert.IsDBNull(reader["Budget"]))?string.Empty:(System.String)reader["Budget"];
			entity.Energy = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.Energy)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.Energy)];
			//entity.Energy = (Convert.IsDBNull(reader["Energy"]))?string.Empty:(System.String)reader["Energy"];
			entity.RawMaterial = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.RawMaterial)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.RawMaterial)];
			//entity.RawMaterial = (Convert.IsDBNull(reader["RawMaterial"]))?string.Empty:(System.String)reader["RawMaterial"];
			entity.SkillAvailability = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.SkillAvailability)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.SkillAvailability)];
			//entity.SkillAvailability = (Convert.IsDBNull(reader["SkillAvailability"]))?string.Empty:(System.String)reader["SkillAvailability"];
			entity.Efficiency = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.Efficiency)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.Efficiency)];
			//entity.Efficiency = (Convert.IsDBNull(reader["Efficiency"]))?string.Empty:(System.String)reader["Efficiency"];
			entity.Effectiveness = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.Effectiveness)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.Effectiveness)];
			//entity.Effectiveness = (Convert.IsDBNull(reader["Effectiveness"]))?string.Empty:(System.String)reader["Effectiveness"];
			entity.Manpower = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.Manpower)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.Manpower)];
			//entity.Manpower = (Convert.IsDBNull(reader["Manpower"]))?string.Empty:(System.String)reader["Manpower"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.EnvironmentInd = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.EnvironmentInd)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.EnvironmentInd)];
			//entity.EnvironmentInd = (Convert.IsDBNull(reader["EnvironmentInd"]))?string.Empty:(System.String)reader["EnvironmentInd"];
			entity.GovernanceMech = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.GovernanceMech)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.GovernanceMech)];
			//entity.GovernanceMech = (Convert.IsDBNull(reader["GovernanceMech"]))?string.Empty:(System.String)reader["GovernanceMech"];
			entity.CapitalAvailability = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.CapitalAvailability)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.CapitalAvailability)];
			//entity.CapitalAvailability = (Convert.IsDBNull(reader["CapitalAvailability"]))?string.Empty:(System.String)reader["CapitalAvailability"];
			entity.Competencies = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.Competencies)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.Competencies)];
			//entity.Competencies = (Convert.IsDBNull(reader["Competencies"]))?string.Empty:(System.String)reader["Competencies"];
			entity.Ethics = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.Ethics)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.Ethics)];
			//entity.Ethics = (Convert.IsDBNull(reader["Ethics"]))?string.Empty:(System.String)reader["Ethics"];
			entity.Facilities = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.Facilities)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.Facilities)];
			//entity.Facilities = (Convert.IsDBNull(reader["Facilities"]))?string.Empty:(System.String)reader["Facilities"];
			entity.ServicesUSage = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.ServicesUSage)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.ServicesUSage)];
			//entity.ServicesUSage = (Convert.IsDBNull(reader["ServicesUSage"]))?string.Empty:(System.String)reader["ServicesUSage"];
			entity.Equipment = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.Equipment)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.Equipment)];
			//entity.Equipment = (Convert.IsDBNull(reader["Equipment"]))?string.Empty:(System.String)reader["Equipment"];
			entity.TimeIndicator = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.TimeIndicator)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.TimeIndicator)];
			//entity.TimeIndicator = (Convert.IsDBNull(reader["TimeIndicator"]))?string.Empty:(System.String)reader["TimeIndicator"];
			entity.GapType = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.GapType)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			entity.Description = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.Description)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.Description)];
			//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
			entity.Abbreviation = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.Abbreviation)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.Abbreviation)];
			//entity.Abbreviation = (Convert.IsDBNull(reader["Abbreviation"]))?string.Empty:(System.String)reader["Abbreviation"];
			entity.IsBusinessExternal = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.IsBusinessExternal)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.IsBusinessExternal)];
			//entity.IsBusinessExternal = (Convert.IsDBNull(reader["IsBusinessExternal"]))?string.Empty:(System.String)reader["IsBusinessExternal"];
			entity.StandardisationStatus = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.StandardisationStatus)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.StandardisationStatus)];
			//entity.StandardisationStatus = (Convert.IsDBNull(reader["StandardisationStatus"]))?string.Empty:(System.String)reader["StandardisationStatus"];
			entity.StandardisationStatusDate = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.StandardisationStatusDate)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.StandardisationStatusDate)];
			//entity.StandardisationStatusDate = (Convert.IsDBNull(reader["StandardisationStatusDate"]))?string.Empty:(System.String)reader["StandardisationStatusDate"];
			entity.DataSourceID = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.DataSourceID)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.DataSourceID)];
			//entity.DataSourceID = (Convert.IsDBNull(reader["DataSourceID"]))?string.Empty:(System.String)reader["DataSourceID"];
			entity.DataSourceName = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.DataSourceName)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.DataSourceName)];
			//entity.DataSourceName = (Convert.IsDBNull(reader["DataSourceName"]))?string.Empty:(System.String)reader["DataSourceName"];
			entity.GeneralRemarks = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.GeneralRemarks)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.GeneralRemarks)];
			//entity.GeneralRemarks = (Convert.IsDBNull(reader["GeneralRemarks"]))?string.Empty:(System.String)reader["GeneralRemarks"];
			entity.ArchitectureStatus = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.ArchitectureStatus)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.ArchitectureStatus)];
			//entity.ArchitectureStatus = (Convert.IsDBNull(reader["ArchitectureStatus"]))?string.Empty:(System.String)reader["ArchitectureStatus"];
			entity.ArchitectureStatusDate = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.ArchitectureStatusDate)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.ArchitectureStatusDate)];
			//entity.ArchitectureStatusDate = (Convert.IsDBNull(reader["ArchitectureStatusDate"]))?string.Empty:(System.String)reader["ArchitectureStatusDate"];
			entity.DesignRationale = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.DesignRationale)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.DesignRationale)];
			//entity.DesignRationale = (Convert.IsDBNull(reader["DesignRationale"]))?string.Empty:(System.String)reader["DesignRationale"];
			entity.FunctionCategory = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.FunctionCategory)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.FunctionCategory)];
			//entity.FunctionCategory = (Convert.IsDBNull(reader["FunctionCategory"]))?string.Empty:(System.String)reader["FunctionCategory"];
			entity.OverallEfficiencyRating = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.OverallEfficiencyRating)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.OverallEfficiencyRating)];
			//entity.OverallEfficiencyRating = (Convert.IsDBNull(reader["OverallEfficiencyRating"]))?string.Empty:(System.String)reader["OverallEfficiencyRating"];
			entity.OverallEffectivenessRating = (reader.IsDBNull(((int)METAView_Function_RetrievalColumn.OverallEffectivenessRating)))?null:(System.String)reader[((int)METAView_Function_RetrievalColumn.OverallEffectivenessRating)];
			//entity.OverallEffectivenessRating = (Convert.IsDBNull(reader["OverallEffectivenessRating"]))?string.Empty:(System.String)reader["OverallEffectivenessRating"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAView_Function_Retrieval"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_Function_Retrieval"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAView_Function_Retrieval entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeId = (Convert.IsDBNull(dataRow["WorkspaceTypeId"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeId"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.Name = (Convert.IsDBNull(dataRow["Name"]))?string.Empty:(System.String)dataRow["Name"];
			entity.ContextualIndicator = (Convert.IsDBNull(dataRow["ContextualIndicator"]))?string.Empty:(System.String)dataRow["ContextualIndicator"];
			entity.FunctionCriticality = (Convert.IsDBNull(dataRow["FunctionCriticality"]))?string.Empty:(System.String)dataRow["FunctionCriticality"];
			entity.ManagementAbility = (Convert.IsDBNull(dataRow["ManagementAbility"]))?string.Empty:(System.String)dataRow["ManagementAbility"];
			entity.InfoAvailability = (Convert.IsDBNull(dataRow["InfoAvailability"]))?string.Empty:(System.String)dataRow["InfoAvailability"];
			entity.LegalAspects = (Convert.IsDBNull(dataRow["LegalAspects"]))?string.Empty:(System.String)dataRow["LegalAspects"];
			entity.Technology = (Convert.IsDBNull(dataRow["Technology"]))?string.Empty:(System.String)dataRow["Technology"];
			entity.Budget = (Convert.IsDBNull(dataRow["Budget"]))?string.Empty:(System.String)dataRow["Budget"];
			entity.Energy = (Convert.IsDBNull(dataRow["Energy"]))?string.Empty:(System.String)dataRow["Energy"];
			entity.RawMaterial = (Convert.IsDBNull(dataRow["RawMaterial"]))?string.Empty:(System.String)dataRow["RawMaterial"];
			entity.SkillAvailability = (Convert.IsDBNull(dataRow["SkillAvailability"]))?string.Empty:(System.String)dataRow["SkillAvailability"];
			entity.Efficiency = (Convert.IsDBNull(dataRow["Efficiency"]))?string.Empty:(System.String)dataRow["Efficiency"];
			entity.Effectiveness = (Convert.IsDBNull(dataRow["Effectiveness"]))?string.Empty:(System.String)dataRow["Effectiveness"];
			entity.Manpower = (Convert.IsDBNull(dataRow["Manpower"]))?string.Empty:(System.String)dataRow["Manpower"];
			entity.CustomField1 = (Convert.IsDBNull(dataRow["CustomField1"]))?string.Empty:(System.String)dataRow["CustomField1"];
			entity.CustomField2 = (Convert.IsDBNull(dataRow["CustomField2"]))?string.Empty:(System.String)dataRow["CustomField2"];
			entity.CustomField3 = (Convert.IsDBNull(dataRow["CustomField3"]))?string.Empty:(System.String)dataRow["CustomField3"];
			entity.EnvironmentInd = (Convert.IsDBNull(dataRow["EnvironmentInd"]))?string.Empty:(System.String)dataRow["EnvironmentInd"];
			entity.GovernanceMech = (Convert.IsDBNull(dataRow["GovernanceMech"]))?string.Empty:(System.String)dataRow["GovernanceMech"];
			entity.CapitalAvailability = (Convert.IsDBNull(dataRow["CapitalAvailability"]))?string.Empty:(System.String)dataRow["CapitalAvailability"];
			entity.Competencies = (Convert.IsDBNull(dataRow["Competencies"]))?string.Empty:(System.String)dataRow["Competencies"];
			entity.Ethics = (Convert.IsDBNull(dataRow["Ethics"]))?string.Empty:(System.String)dataRow["Ethics"];
			entity.Facilities = (Convert.IsDBNull(dataRow["Facilities"]))?string.Empty:(System.String)dataRow["Facilities"];
			entity.ServicesUSage = (Convert.IsDBNull(dataRow["ServicesUSage"]))?string.Empty:(System.String)dataRow["ServicesUSage"];
			entity.Equipment = (Convert.IsDBNull(dataRow["Equipment"]))?string.Empty:(System.String)dataRow["Equipment"];
			entity.TimeIndicator = (Convert.IsDBNull(dataRow["TimeIndicator"]))?string.Empty:(System.String)dataRow["TimeIndicator"];
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
			entity.FunctionCategory = (Convert.IsDBNull(dataRow["FunctionCategory"]))?string.Empty:(System.String)dataRow["FunctionCategory"];
			entity.OverallEfficiencyRating = (Convert.IsDBNull(dataRow["OverallEfficiencyRating"]))?string.Empty:(System.String)dataRow["OverallEfficiencyRating"];
			entity.OverallEffectivenessRating = (Convert.IsDBNull(dataRow["OverallEffectivenessRating"]))?string.Empty:(System.String)dataRow["OverallEffectivenessRating"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAView_Function_RetrievalFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_Function_Retrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_Function_RetrievalFilterBuilder : SqlFilterBuilder<METAView_Function_RetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_Function_RetrievalFilterBuilder class.
		/// </summary>
		public METAView_Function_RetrievalFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_Function_RetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_Function_RetrievalFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_Function_RetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_Function_RetrievalFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_Function_RetrievalFilterBuilder

	#region METAView_Function_RetrievalParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_Function_Retrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_Function_RetrievalParameterBuilder : ParameterizedSqlFilterBuilder<METAView_Function_RetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_Function_RetrievalParameterBuilder class.
		/// </summary>
		public METAView_Function_RetrievalParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_Function_RetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_Function_RetrievalParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_Function_RetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_Function_RetrievalParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_Function_RetrievalParameterBuilder
	
	#region METAView_Function_RetrievalSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_Function_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Function_RetrievalSortBuilder : SqlSortBuilder<METAView_Function_RetrievalColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_Function_RetrievalSqlSortBuilder class.
		/// </summary>
		public METAView_Function_RetrievalSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAView_Function_RetrievalSortBuilder

} // end namespace
