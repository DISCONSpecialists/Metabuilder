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
	/// This class is the base class for any <see cref="METAViewFunctionListingProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAViewFunctionListingProviderBaseCore : EntityViewProviderBase<METAViewFunctionListing>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAViewFunctionListing&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAViewFunctionListing&gt;"/></returns>
		protected static VList&lt;METAViewFunctionListing&gt; Fill(DataSet dataSet, VList<METAViewFunctionListing> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAViewFunctionListing>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAViewFunctionListing&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAViewFunctionListing>"/></returns>
		protected static VList&lt;METAViewFunctionListing&gt; Fill(DataTable dataTable, VList<METAViewFunctionListing> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAViewFunctionListing c = new METAViewFunctionListing();
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
		/// Fill an <see cref="VList&lt;METAViewFunctionListing&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAViewFunctionListing&gt;"/></returns>
		protected VList<METAViewFunctionListing> Fill(IDataReader reader, VList<METAViewFunctionListing> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAViewFunctionListing entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAViewFunctionListing>("METAViewFunctionListing",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAViewFunctionListing();
					}
					entity.WorkspaceName = (System.String)reader["WorkspaceName"];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeID = (reader.IsDBNull(reader.GetOrdinal("WorkspaceTypeID")))?null:(System.Int32?)reader["WorkspaceTypeID"];
					//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
					entity.VCStatusID = (System.Int32)reader["VCStatusID"];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.Pkid = (System.Int32)reader["pkid"];
					//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader["Machine"];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(reader.GetOrdinal("VCMachineID")))?null:(System.String)reader["VCMachineID"];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Name = (reader.IsDBNull(reader.GetOrdinal("Name")))?null:(System.String)reader["Name"];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.ContextualIndicator = (reader.IsDBNull(reader.GetOrdinal("ContextualIndicator")))?null:(System.String)reader["ContextualIndicator"];
					//entity.ContextualIndicator = (Convert.IsDBNull(reader["ContextualIndicator"]))?string.Empty:(System.String)reader["ContextualIndicator"];
					entity.FunctionCriticality = (reader.IsDBNull(reader.GetOrdinal("FunctionCriticality")))?null:(System.Int32?)reader["FunctionCriticality"];
					//entity.FunctionCriticality = (Convert.IsDBNull(reader["FunctionCriticality"]))?(int)0:(System.Int32?)reader["FunctionCriticality"];
					entity.ManagementAbility = (reader.IsDBNull(reader.GetOrdinal("ManagementAbility")))?null:(System.String)reader["ManagementAbility"];
					//entity.ManagementAbility = (Convert.IsDBNull(reader["ManagementAbility"]))?string.Empty:(System.String)reader["ManagementAbility"];
					entity.InfoAvailability = (reader.IsDBNull(reader.GetOrdinal("InfoAvailability")))?null:(System.String)reader["InfoAvailability"];
					//entity.InfoAvailability = (Convert.IsDBNull(reader["InfoAvailability"]))?string.Empty:(System.String)reader["InfoAvailability"];
					entity.LegalAspects = (reader.IsDBNull(reader.GetOrdinal("LegalAspects")))?null:(System.String)reader["LegalAspects"];
					//entity.LegalAspects = (Convert.IsDBNull(reader["LegalAspects"]))?string.Empty:(System.String)reader["LegalAspects"];
					entity.Technology = (reader.IsDBNull(reader.GetOrdinal("Technology")))?null:(System.String)reader["Technology"];
					//entity.Technology = (Convert.IsDBNull(reader["Technology"]))?string.Empty:(System.String)reader["Technology"];
					entity.Budget = (reader.IsDBNull(reader.GetOrdinal("Budget")))?null:(System.String)reader["Budget"];
					//entity.Budget = (Convert.IsDBNull(reader["Budget"]))?string.Empty:(System.String)reader["Budget"];
					entity.Energy = (reader.IsDBNull(reader.GetOrdinal("Energy")))?null:(System.String)reader["Energy"];
					//entity.Energy = (Convert.IsDBNull(reader["Energy"]))?string.Empty:(System.String)reader["Energy"];
					entity.RawMaterial = (reader.IsDBNull(reader.GetOrdinal("RawMaterial")))?null:(System.String)reader["RawMaterial"];
					//entity.RawMaterial = (Convert.IsDBNull(reader["RawMaterial"]))?string.Empty:(System.String)reader["RawMaterial"];
					entity.SkillAvailability = (reader.IsDBNull(reader.GetOrdinal("SkillAvailability")))?null:(System.String)reader["SkillAvailability"];
					//entity.SkillAvailability = (Convert.IsDBNull(reader["SkillAvailability"]))?string.Empty:(System.String)reader["SkillAvailability"];
					entity.Efficiency = (reader.IsDBNull(reader.GetOrdinal("Efficiency")))?null:(System.String)reader["Efficiency"];
					//entity.Efficiency = (Convert.IsDBNull(reader["Efficiency"]))?string.Empty:(System.String)reader["Efficiency"];
					entity.Effectiveness = (reader.IsDBNull(reader.GetOrdinal("Effectiveness")))?null:(System.String)reader["Effectiveness"];
					//entity.Effectiveness = (Convert.IsDBNull(reader["Effectiveness"]))?string.Empty:(System.String)reader["Effectiveness"];
					entity.Manpower = (reader.IsDBNull(reader.GetOrdinal("Manpower")))?null:(System.String)reader["Manpower"];
					//entity.Manpower = (Convert.IsDBNull(reader["Manpower"]))?string.Empty:(System.String)reader["Manpower"];
					entity.CustomField1 = (reader.IsDBNull(reader.GetOrdinal("CustomField1")))?null:(System.String)reader["CustomField1"];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(reader.GetOrdinal("CustomField2")))?null:(System.String)reader["CustomField2"];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(reader.GetOrdinal("CustomField3")))?null:(System.String)reader["CustomField3"];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.AcceptChanges();
					rows.Add(entity);
					pageLength -= 1;
				}
				recordnum += 1;
			}
			return rows;
		}
		
		
		/// <summary>
		/// Refreshes the <see cref="METAViewFunctionListing"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewFunctionListing"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAViewFunctionListing entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader["WorkspaceName"];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeID = (reader.IsDBNull(reader.GetOrdinal("WorkspaceTypeID")))?null:(System.Int32?)reader["WorkspaceTypeID"];
			//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
			entity.VCStatusID = (System.Int32)reader["VCStatusID"];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.Pkid = (System.Int32)reader["pkid"];
			//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader["Machine"];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(reader.GetOrdinal("VCMachineID")))?null:(System.String)reader["VCMachineID"];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Name = (reader.IsDBNull(reader.GetOrdinal("Name")))?null:(System.String)reader["Name"];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.ContextualIndicator = (reader.IsDBNull(reader.GetOrdinal("ContextualIndicator")))?null:(System.String)reader["ContextualIndicator"];
			//entity.ContextualIndicator = (Convert.IsDBNull(reader["ContextualIndicator"]))?string.Empty:(System.String)reader["ContextualIndicator"];
			entity.FunctionCriticality = (reader.IsDBNull(reader.GetOrdinal("FunctionCriticality")))?null:(System.Int32?)reader["FunctionCriticality"];
			//entity.FunctionCriticality = (Convert.IsDBNull(reader["FunctionCriticality"]))?(int)0:(System.Int32?)reader["FunctionCriticality"];
			entity.ManagementAbility = (reader.IsDBNull(reader.GetOrdinal("ManagementAbility")))?null:(System.String)reader["ManagementAbility"];
			//entity.ManagementAbility = (Convert.IsDBNull(reader["ManagementAbility"]))?string.Empty:(System.String)reader["ManagementAbility"];
			entity.InfoAvailability = (reader.IsDBNull(reader.GetOrdinal("InfoAvailability")))?null:(System.String)reader["InfoAvailability"];
			//entity.InfoAvailability = (Convert.IsDBNull(reader["InfoAvailability"]))?string.Empty:(System.String)reader["InfoAvailability"];
			entity.LegalAspects = (reader.IsDBNull(reader.GetOrdinal("LegalAspects")))?null:(System.String)reader["LegalAspects"];
			//entity.LegalAspects = (Convert.IsDBNull(reader["LegalAspects"]))?string.Empty:(System.String)reader["LegalAspects"];
			entity.Technology = (reader.IsDBNull(reader.GetOrdinal("Technology")))?null:(System.String)reader["Technology"];
			//entity.Technology = (Convert.IsDBNull(reader["Technology"]))?string.Empty:(System.String)reader["Technology"];
			entity.Budget = (reader.IsDBNull(reader.GetOrdinal("Budget")))?null:(System.String)reader["Budget"];
			//entity.Budget = (Convert.IsDBNull(reader["Budget"]))?string.Empty:(System.String)reader["Budget"];
			entity.Energy = (reader.IsDBNull(reader.GetOrdinal("Energy")))?null:(System.String)reader["Energy"];
			//entity.Energy = (Convert.IsDBNull(reader["Energy"]))?string.Empty:(System.String)reader["Energy"];
			entity.RawMaterial = (reader.IsDBNull(reader.GetOrdinal("RawMaterial")))?null:(System.String)reader["RawMaterial"];
			//entity.RawMaterial = (Convert.IsDBNull(reader["RawMaterial"]))?string.Empty:(System.String)reader["RawMaterial"];
			entity.SkillAvailability = (reader.IsDBNull(reader.GetOrdinal("SkillAvailability")))?null:(System.String)reader["SkillAvailability"];
			//entity.SkillAvailability = (Convert.IsDBNull(reader["SkillAvailability"]))?string.Empty:(System.String)reader["SkillAvailability"];
			entity.Efficiency = (reader.IsDBNull(reader.GetOrdinal("Efficiency")))?null:(System.String)reader["Efficiency"];
			//entity.Efficiency = (Convert.IsDBNull(reader["Efficiency"]))?string.Empty:(System.String)reader["Efficiency"];
			entity.Effectiveness = (reader.IsDBNull(reader.GetOrdinal("Effectiveness")))?null:(System.String)reader["Effectiveness"];
			//entity.Effectiveness = (Convert.IsDBNull(reader["Effectiveness"]))?string.Empty:(System.String)reader["Effectiveness"];
			entity.Manpower = (reader.IsDBNull(reader.GetOrdinal("Manpower")))?null:(System.String)reader["Manpower"];
			//entity.Manpower = (Convert.IsDBNull(reader["Manpower"]))?string.Empty:(System.String)reader["Manpower"];
			entity.CustomField1 = (reader.IsDBNull(reader.GetOrdinal("CustomField1")))?null:(System.String)reader["CustomField1"];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(reader.GetOrdinal("CustomField2")))?null:(System.String)reader["CustomField2"];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(reader.GetOrdinal("CustomField3")))?null:(System.String)reader["CustomField3"];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAViewFunctionListing"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewFunctionListing"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAViewFunctionListing entity)
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
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAViewFunctionListingFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewFunctionListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewFunctionListingFilterBuilder : SqlFilterBuilder<METAViewFunctionListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewFunctionListingFilterBuilder class.
		/// </summary>
		public METAViewFunctionListingFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewFunctionListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewFunctionListingFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewFunctionListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewFunctionListingFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewFunctionListingFilterBuilder

	#region METAViewFunctionListingParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewFunctionListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewFunctionListingParameterBuilder : ParameterizedSqlFilterBuilder<METAViewFunctionListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewFunctionListingParameterBuilder class.
		/// </summary>
		public METAViewFunctionListingParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewFunctionListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewFunctionListingParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewFunctionListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewFunctionListingParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewFunctionListingParameterBuilder
} // end namespace
