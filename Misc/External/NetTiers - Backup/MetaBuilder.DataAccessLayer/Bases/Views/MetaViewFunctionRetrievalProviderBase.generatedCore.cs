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
	/// This class is the base class for any <see cref="METAViewFunctionRetrievalProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAViewFunctionRetrievalProviderBaseCore : EntityViewProviderBase<METAViewFunctionRetrieval>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAViewFunctionRetrieval&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAViewFunctionRetrieval&gt;"/></returns>
		protected static VList&lt;METAViewFunctionRetrieval&gt; Fill(DataSet dataSet, VList<METAViewFunctionRetrieval> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAViewFunctionRetrieval>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAViewFunctionRetrieval&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAViewFunctionRetrieval>"/></returns>
		protected static VList&lt;METAViewFunctionRetrieval&gt; Fill(DataTable dataTable, VList<METAViewFunctionRetrieval> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAViewFunctionRetrieval c = new METAViewFunctionRetrieval();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeID = (Convert.IsDBNull(row["WorkspaceTypeID"]))?(int)0:(System.Int32?)row["WorkspaceTypeID"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.Pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.Name = (Convert.IsDBNull(row["Name"]))?string.Empty:(System.String)row["Name"];
					c.ContextualIndicator = (Convert.IsDBNull(row["ContextualIndicator"]))?string.Empty:(System.String)row["ContextualIndicator"];
					c.FunctionCriticality = (Convert.IsDBNull(row["FunctionCriticality"]))?(int)0:(System.Int32?)row["FunctionCriticality"];
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
					c.ServicesUsage = (Convert.IsDBNull(row["ServicesUsage"]))?string.Empty:(System.String)row["ServicesUsage"];
					c.Equipment = (Convert.IsDBNull(row["Equipment"]))?string.Empty:(System.String)row["Equipment"];
					c.TimeIndicator = (Convert.IsDBNull(row["TimeIndicator"]))?string.Empty:(System.String)row["TimeIndicator"];
					c.Effeciency = (Convert.IsDBNull(row["Effeciency"]))?string.Empty:(System.String)row["Effeciency"];
					c.Effectiviness = (Convert.IsDBNull(row["Effectiviness"]))?string.Empty:(System.String)row["Effectiviness"];
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
		/// Fill an <see cref="VList&lt;METAViewFunctionRetrieval&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAViewFunctionRetrieval&gt;"/></returns>
		protected VList<METAViewFunctionRetrieval> Fill(IDataReader reader, VList<METAViewFunctionRetrieval> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAViewFunctionRetrieval entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAViewFunctionRetrieval>("METAViewFunctionRetrieval",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAViewFunctionRetrieval();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAViewFunctionRetrievalColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewFunctionRetrievalColumn.WorkspaceTypeID)];
					//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
					entity.VCStatusID = (System.Int32)reader[((int)METAViewFunctionRetrievalColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.Pkid = (System.Int32)reader[((int)METAViewFunctionRetrievalColumn.Pkid)];
					//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAViewFunctionRetrievalColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Name = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.Name)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.Name)];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.ContextualIndicator = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.ContextualIndicator)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.ContextualIndicator)];
					//entity.ContextualIndicator = (Convert.IsDBNull(reader["ContextualIndicator"]))?string.Empty:(System.String)reader["ContextualIndicator"];
					entity.FunctionCriticality = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.FunctionCriticality)))?null:(System.Int32?)reader[((int)METAViewFunctionRetrievalColumn.FunctionCriticality)];
					//entity.FunctionCriticality = (Convert.IsDBNull(reader["FunctionCriticality"]))?(int)0:(System.Int32?)reader["FunctionCriticality"];
					entity.ManagementAbility = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.ManagementAbility)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.ManagementAbility)];
					//entity.ManagementAbility = (Convert.IsDBNull(reader["ManagementAbility"]))?string.Empty:(System.String)reader["ManagementAbility"];
					entity.InfoAvailability = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.InfoAvailability)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.InfoAvailability)];
					//entity.InfoAvailability = (Convert.IsDBNull(reader["InfoAvailability"]))?string.Empty:(System.String)reader["InfoAvailability"];
					entity.LegalAspects = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.LegalAspects)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.LegalAspects)];
					//entity.LegalAspects = (Convert.IsDBNull(reader["LegalAspects"]))?string.Empty:(System.String)reader["LegalAspects"];
					entity.Technology = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.Technology)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.Technology)];
					//entity.Technology = (Convert.IsDBNull(reader["Technology"]))?string.Empty:(System.String)reader["Technology"];
					entity.Budget = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.Budget)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.Budget)];
					//entity.Budget = (Convert.IsDBNull(reader["Budget"]))?string.Empty:(System.String)reader["Budget"];
					entity.Energy = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.Energy)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.Energy)];
					//entity.Energy = (Convert.IsDBNull(reader["Energy"]))?string.Empty:(System.String)reader["Energy"];
					entity.RawMaterial = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.RawMaterial)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.RawMaterial)];
					//entity.RawMaterial = (Convert.IsDBNull(reader["RawMaterial"]))?string.Empty:(System.String)reader["RawMaterial"];
					entity.SkillAvailability = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.SkillAvailability)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.SkillAvailability)];
					//entity.SkillAvailability = (Convert.IsDBNull(reader["SkillAvailability"]))?string.Empty:(System.String)reader["SkillAvailability"];
					entity.Efficiency = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.Efficiency)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.Efficiency)];
					//entity.Efficiency = (Convert.IsDBNull(reader["Efficiency"]))?string.Empty:(System.String)reader["Efficiency"];
					entity.Effectiveness = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.Effectiveness)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.Effectiveness)];
					//entity.Effectiveness = (Convert.IsDBNull(reader["Effectiveness"]))?string.Empty:(System.String)reader["Effectiveness"];
					entity.Manpower = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.Manpower)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.Manpower)];
					//entity.Manpower = (Convert.IsDBNull(reader["Manpower"]))?string.Empty:(System.String)reader["Manpower"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.EnvironmentInd = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.EnvironmentInd)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.EnvironmentInd)];
					//entity.EnvironmentInd = (Convert.IsDBNull(reader["EnvironmentInd"]))?string.Empty:(System.String)reader["EnvironmentInd"];
					entity.GovernanceMech = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.GovernanceMech)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.GovernanceMech)];
					//entity.GovernanceMech = (Convert.IsDBNull(reader["GovernanceMech"]))?string.Empty:(System.String)reader["GovernanceMech"];
					entity.CapitalAvailability = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.CapitalAvailability)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.CapitalAvailability)];
					//entity.CapitalAvailability = (Convert.IsDBNull(reader["CapitalAvailability"]))?string.Empty:(System.String)reader["CapitalAvailability"];
					entity.Competencies = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.Competencies)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.Competencies)];
					//entity.Competencies = (Convert.IsDBNull(reader["Competencies"]))?string.Empty:(System.String)reader["Competencies"];
					entity.Ethics = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.Ethics)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.Ethics)];
					//entity.Ethics = (Convert.IsDBNull(reader["Ethics"]))?string.Empty:(System.String)reader["Ethics"];
					entity.Facilities = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.Facilities)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.Facilities)];
					//entity.Facilities = (Convert.IsDBNull(reader["Facilities"]))?string.Empty:(System.String)reader["Facilities"];
					entity.ServicesUsage = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.ServicesUsage)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.ServicesUsage)];
					//entity.ServicesUsage = (Convert.IsDBNull(reader["ServicesUsage"]))?string.Empty:(System.String)reader["ServicesUsage"];
					entity.Equipment = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.Equipment)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.Equipment)];
					//entity.Equipment = (Convert.IsDBNull(reader["Equipment"]))?string.Empty:(System.String)reader["Equipment"];
					entity.TimeIndicator = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.TimeIndicator)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.TimeIndicator)];
					//entity.TimeIndicator = (Convert.IsDBNull(reader["TimeIndicator"]))?string.Empty:(System.String)reader["TimeIndicator"];
					entity.Effeciency = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.Effeciency)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.Effeciency)];
					//entity.Effeciency = (Convert.IsDBNull(reader["Effeciency"]))?string.Empty:(System.String)reader["Effeciency"];
					entity.Effectiviness = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.Effectiviness)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.Effectiviness)];
					//entity.Effectiviness = (Convert.IsDBNull(reader["Effectiviness"]))?string.Empty:(System.String)reader["Effectiviness"];
					entity.GapType = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.GapType)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.GapType)];
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
		/// Refreshes the <see cref="METAViewFunctionRetrieval"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewFunctionRetrieval"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAViewFunctionRetrieval entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAViewFunctionRetrievalColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewFunctionRetrievalColumn.WorkspaceTypeID)];
			//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
			entity.VCStatusID = (System.Int32)reader[((int)METAViewFunctionRetrievalColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.Pkid = (System.Int32)reader[((int)METAViewFunctionRetrievalColumn.Pkid)];
			//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAViewFunctionRetrievalColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Name = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.Name)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.Name)];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.ContextualIndicator = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.ContextualIndicator)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.ContextualIndicator)];
			//entity.ContextualIndicator = (Convert.IsDBNull(reader["ContextualIndicator"]))?string.Empty:(System.String)reader["ContextualIndicator"];
			entity.FunctionCriticality = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.FunctionCriticality)))?null:(System.Int32?)reader[((int)METAViewFunctionRetrievalColumn.FunctionCriticality)];
			//entity.FunctionCriticality = (Convert.IsDBNull(reader["FunctionCriticality"]))?(int)0:(System.Int32?)reader["FunctionCriticality"];
			entity.ManagementAbility = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.ManagementAbility)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.ManagementAbility)];
			//entity.ManagementAbility = (Convert.IsDBNull(reader["ManagementAbility"]))?string.Empty:(System.String)reader["ManagementAbility"];
			entity.InfoAvailability = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.InfoAvailability)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.InfoAvailability)];
			//entity.InfoAvailability = (Convert.IsDBNull(reader["InfoAvailability"]))?string.Empty:(System.String)reader["InfoAvailability"];
			entity.LegalAspects = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.LegalAspects)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.LegalAspects)];
			//entity.LegalAspects = (Convert.IsDBNull(reader["LegalAspects"]))?string.Empty:(System.String)reader["LegalAspects"];
			entity.Technology = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.Technology)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.Technology)];
			//entity.Technology = (Convert.IsDBNull(reader["Technology"]))?string.Empty:(System.String)reader["Technology"];
			entity.Budget = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.Budget)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.Budget)];
			//entity.Budget = (Convert.IsDBNull(reader["Budget"]))?string.Empty:(System.String)reader["Budget"];
			entity.Energy = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.Energy)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.Energy)];
			//entity.Energy = (Convert.IsDBNull(reader["Energy"]))?string.Empty:(System.String)reader["Energy"];
			entity.RawMaterial = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.RawMaterial)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.RawMaterial)];
			//entity.RawMaterial = (Convert.IsDBNull(reader["RawMaterial"]))?string.Empty:(System.String)reader["RawMaterial"];
			entity.SkillAvailability = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.SkillAvailability)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.SkillAvailability)];
			//entity.SkillAvailability = (Convert.IsDBNull(reader["SkillAvailability"]))?string.Empty:(System.String)reader["SkillAvailability"];
			entity.Efficiency = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.Efficiency)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.Efficiency)];
			//entity.Efficiency = (Convert.IsDBNull(reader["Efficiency"]))?string.Empty:(System.String)reader["Efficiency"];
			entity.Effectiveness = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.Effectiveness)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.Effectiveness)];
			//entity.Effectiveness = (Convert.IsDBNull(reader["Effectiveness"]))?string.Empty:(System.String)reader["Effectiveness"];
			entity.Manpower = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.Manpower)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.Manpower)];
			//entity.Manpower = (Convert.IsDBNull(reader["Manpower"]))?string.Empty:(System.String)reader["Manpower"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.EnvironmentInd = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.EnvironmentInd)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.EnvironmentInd)];
			//entity.EnvironmentInd = (Convert.IsDBNull(reader["EnvironmentInd"]))?string.Empty:(System.String)reader["EnvironmentInd"];
			entity.GovernanceMech = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.GovernanceMech)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.GovernanceMech)];
			//entity.GovernanceMech = (Convert.IsDBNull(reader["GovernanceMech"]))?string.Empty:(System.String)reader["GovernanceMech"];
			entity.CapitalAvailability = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.CapitalAvailability)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.CapitalAvailability)];
			//entity.CapitalAvailability = (Convert.IsDBNull(reader["CapitalAvailability"]))?string.Empty:(System.String)reader["CapitalAvailability"];
			entity.Competencies = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.Competencies)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.Competencies)];
			//entity.Competencies = (Convert.IsDBNull(reader["Competencies"]))?string.Empty:(System.String)reader["Competencies"];
			entity.Ethics = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.Ethics)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.Ethics)];
			//entity.Ethics = (Convert.IsDBNull(reader["Ethics"]))?string.Empty:(System.String)reader["Ethics"];
			entity.Facilities = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.Facilities)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.Facilities)];
			//entity.Facilities = (Convert.IsDBNull(reader["Facilities"]))?string.Empty:(System.String)reader["Facilities"];
			entity.ServicesUsage = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.ServicesUsage)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.ServicesUsage)];
			//entity.ServicesUsage = (Convert.IsDBNull(reader["ServicesUsage"]))?string.Empty:(System.String)reader["ServicesUsage"];
			entity.Equipment = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.Equipment)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.Equipment)];
			//entity.Equipment = (Convert.IsDBNull(reader["Equipment"]))?string.Empty:(System.String)reader["Equipment"];
			entity.TimeIndicator = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.TimeIndicator)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.TimeIndicator)];
			//entity.TimeIndicator = (Convert.IsDBNull(reader["TimeIndicator"]))?string.Empty:(System.String)reader["TimeIndicator"];
			entity.Effeciency = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.Effeciency)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.Effeciency)];
			//entity.Effeciency = (Convert.IsDBNull(reader["Effeciency"]))?string.Empty:(System.String)reader["Effeciency"];
			entity.Effectiviness = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.Effectiviness)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.Effectiviness)];
			//entity.Effectiviness = (Convert.IsDBNull(reader["Effectiviness"]))?string.Empty:(System.String)reader["Effectiviness"];
			entity.GapType = (reader.IsDBNull(((int)METAViewFunctionRetrievalColumn.GapType)))?null:(System.String)reader[((int)METAViewFunctionRetrievalColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAViewFunctionRetrieval"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewFunctionRetrieval"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAViewFunctionRetrieval entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeID = (Convert.IsDBNull(dataRow["WorkspaceTypeID"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeID"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.Pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.Name = (Convert.IsDBNull(dataRow["Name"]))?string.Empty:(System.String)dataRow["Name"];
			entity.ContextualIndicator = (Convert.IsDBNull(dataRow["ContextualIndicator"]))?string.Empty:(System.String)dataRow["ContextualIndicator"];
			entity.FunctionCriticality = (Convert.IsDBNull(dataRow["FunctionCriticality"]))?(int)0:(System.Int32?)dataRow["FunctionCriticality"];
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
			entity.ServicesUsage = (Convert.IsDBNull(dataRow["ServicesUsage"]))?string.Empty:(System.String)dataRow["ServicesUsage"];
			entity.Equipment = (Convert.IsDBNull(dataRow["Equipment"]))?string.Empty:(System.String)dataRow["Equipment"];
			entity.TimeIndicator = (Convert.IsDBNull(dataRow["TimeIndicator"]))?string.Empty:(System.String)dataRow["TimeIndicator"];
			entity.Effeciency = (Convert.IsDBNull(dataRow["Effeciency"]))?string.Empty:(System.String)dataRow["Effeciency"];
			entity.Effectiviness = (Convert.IsDBNull(dataRow["Effectiviness"]))?string.Empty:(System.String)dataRow["Effectiviness"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAViewFunctionRetrievalFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewFunctionRetrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewFunctionRetrievalFilterBuilder : SqlFilterBuilder<METAViewFunctionRetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewFunctionRetrievalFilterBuilder class.
		/// </summary>
		public METAViewFunctionRetrievalFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewFunctionRetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewFunctionRetrievalFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewFunctionRetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewFunctionRetrievalFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewFunctionRetrievalFilterBuilder

	#region METAViewFunctionRetrievalParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewFunctionRetrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewFunctionRetrievalParameterBuilder : ParameterizedSqlFilterBuilder<METAViewFunctionRetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewFunctionRetrievalParameterBuilder class.
		/// </summary>
		public METAViewFunctionRetrievalParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewFunctionRetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewFunctionRetrievalParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewFunctionRetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewFunctionRetrievalParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewFunctionRetrievalParameterBuilder
	
	#region METAViewFunctionRetrievalSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewFunctionRetrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAViewFunctionRetrievalSortBuilder : SqlSortBuilder<METAViewFunctionRetrievalColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewFunctionRetrievalSqlSortBuilder class.
		/// </summary>
		public METAViewFunctionRetrievalSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAViewFunctionRetrievalSortBuilder

} // end namespace
