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
	/// This class is the base class for any <see cref="METAViewScenarioRetrievalProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAViewScenarioRetrievalProviderBaseCore : EntityViewProviderBase<METAViewScenarioRetrieval>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAViewScenarioRetrieval&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAViewScenarioRetrieval&gt;"/></returns>
		protected static VList&lt;METAViewScenarioRetrieval&gt; Fill(DataSet dataSet, VList<METAViewScenarioRetrieval> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAViewScenarioRetrieval>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAViewScenarioRetrieval&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAViewScenarioRetrieval>"/></returns>
		protected static VList&lt;METAViewScenarioRetrieval&gt; Fill(DataTable dataTable, VList<METAViewScenarioRetrieval> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAViewScenarioRetrieval c = new METAViewScenarioRetrieval();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeID = (Convert.IsDBNull(row["WorkspaceTypeID"]))?(int)0:(System.Int32?)row["WorkspaceTypeID"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.Pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.Name = (Convert.IsDBNull(row["Name"]))?string.Empty:(System.String)row["Name"];
					c.AccProbOfReal = (Convert.IsDBNull(row["Acc_Prob_of_Real"]))?string.Empty:(System.String)row["Acc_Prob_of_Real"];
					c.StartDate = (Convert.IsDBNull(row["Start_Date"]))?string.Empty:(System.String)row["Start_Date"];
					c.EndDate = (Convert.IsDBNull(row["End_Date"]))?string.Empty:(System.String)row["End_Date"];
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
		/// Fill an <see cref="VList&lt;METAViewScenarioRetrieval&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAViewScenarioRetrieval&gt;"/></returns>
		protected VList<METAViewScenarioRetrieval> Fill(IDataReader reader, VList<METAViewScenarioRetrieval> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAViewScenarioRetrieval entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAViewScenarioRetrieval>("METAViewScenarioRetrieval",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAViewScenarioRetrieval();
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
					entity.AccProbOfReal = (reader.IsDBNull(reader.GetOrdinal("Acc_Prob_of_Real")))?null:(System.String)reader["Acc_Prob_of_Real"];
					//entity.AccProbOfReal = (Convert.IsDBNull(reader["Acc_Prob_of_Real"]))?string.Empty:(System.String)reader["Acc_Prob_of_Real"];
					entity.StartDate = (reader.IsDBNull(reader.GetOrdinal("Start_Date")))?null:(System.String)reader["Start_Date"];
					//entity.StartDate = (Convert.IsDBNull(reader["Start_Date"]))?string.Empty:(System.String)reader["Start_Date"];
					entity.EndDate = (reader.IsDBNull(reader.GetOrdinal("End_Date")))?null:(System.String)reader["End_Date"];
					//entity.EndDate = (Convert.IsDBNull(reader["End_Date"]))?string.Empty:(System.String)reader["End_Date"];
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
		/// Refreshes the <see cref="METAViewScenarioRetrieval"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewScenarioRetrieval"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAViewScenarioRetrieval entity)
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
			entity.AccProbOfReal = (reader.IsDBNull(reader.GetOrdinal("Acc_Prob_of_Real")))?null:(System.String)reader["Acc_Prob_of_Real"];
			//entity.AccProbOfReal = (Convert.IsDBNull(reader["Acc_Prob_of_Real"]))?string.Empty:(System.String)reader["Acc_Prob_of_Real"];
			entity.StartDate = (reader.IsDBNull(reader.GetOrdinal("Start_Date")))?null:(System.String)reader["Start_Date"];
			//entity.StartDate = (Convert.IsDBNull(reader["Start_Date"]))?string.Empty:(System.String)reader["Start_Date"];
			entity.EndDate = (reader.IsDBNull(reader.GetOrdinal("End_Date")))?null:(System.String)reader["End_Date"];
			//entity.EndDate = (Convert.IsDBNull(reader["End_Date"]))?string.Empty:(System.String)reader["End_Date"];
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
		/// Refreshes the <see cref="METAViewScenarioRetrieval"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewScenarioRetrieval"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAViewScenarioRetrieval entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeID = (Convert.IsDBNull(dataRow["WorkspaceTypeID"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeID"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.Pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.Name = (Convert.IsDBNull(dataRow["Name"]))?string.Empty:(System.String)dataRow["Name"];
			entity.AccProbOfReal = (Convert.IsDBNull(dataRow["Acc_Prob_of_Real"]))?string.Empty:(System.String)dataRow["Acc_Prob_of_Real"];
			entity.StartDate = (Convert.IsDBNull(dataRow["Start_Date"]))?string.Empty:(System.String)dataRow["Start_Date"];
			entity.EndDate = (Convert.IsDBNull(dataRow["End_Date"]))?string.Empty:(System.String)dataRow["End_Date"];
			entity.CustomField1 = (Convert.IsDBNull(dataRow["CustomField1"]))?string.Empty:(System.String)dataRow["CustomField1"];
			entity.CustomField2 = (Convert.IsDBNull(dataRow["CustomField2"]))?string.Empty:(System.String)dataRow["CustomField2"];
			entity.CustomField3 = (Convert.IsDBNull(dataRow["CustomField3"]))?string.Empty:(System.String)dataRow["CustomField3"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAViewScenarioRetrievalFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewScenarioRetrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewScenarioRetrievalFilterBuilder : SqlFilterBuilder<METAViewScenarioRetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewScenarioRetrievalFilterBuilder class.
		/// </summary>
		public METAViewScenarioRetrievalFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewScenarioRetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewScenarioRetrievalFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewScenarioRetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewScenarioRetrievalFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewScenarioRetrievalFilterBuilder

	#region METAViewScenarioRetrievalParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewScenarioRetrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewScenarioRetrievalParameterBuilder : ParameterizedSqlFilterBuilder<METAViewScenarioRetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewScenarioRetrievalParameterBuilder class.
		/// </summary>
		public METAViewScenarioRetrievalParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewScenarioRetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewScenarioRetrievalParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewScenarioRetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewScenarioRetrievalParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewScenarioRetrievalParameterBuilder
} // end namespace
