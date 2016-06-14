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
	/// This class is the base class for any <see cref="METAView_FunctionalDependency_ListingProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAView_FunctionalDependency_ListingProviderBaseCore : EntityViewProviderBase<METAView_FunctionalDependency_Listing>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAView_FunctionalDependency_Listing&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAView_FunctionalDependency_Listing&gt;"/></returns>
		protected static VList&lt;METAView_FunctionalDependency_Listing&gt; Fill(DataSet dataSet, VList<METAView_FunctionalDependency_Listing> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAView_FunctionalDependency_Listing>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAView_FunctionalDependency_Listing&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAView_FunctionalDependency_Listing>"/></returns>
		protected static VList&lt;METAView_FunctionalDependency_Listing&gt; Fill(DataTable dataTable, VList<METAView_FunctionalDependency_Listing> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAView_FunctionalDependency_Listing c = new METAView_FunctionalDependency_Listing();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeId = (Convert.IsDBNull(row["WorkspaceTypeId"]))?(int)0:(System.Int32?)row["WorkspaceTypeId"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.InferenceRule = (Convert.IsDBNull(row["InferenceRule"]))?string.Empty:(System.String)row["InferenceRule"];
					c.Condition = (Convert.IsDBNull(row["Condition"]))?string.Empty:(System.String)row["Condition"];
					c.Description = (Convert.IsDBNull(row["Description"]))?string.Empty:(System.String)row["Description"];
					c.CohesionWeight = (Convert.IsDBNull(row["CohesionWeight"]))?string.Empty:(System.String)row["CohesionWeight"];
					c.CustomField1 = (Convert.IsDBNull(row["CustomField1"]))?string.Empty:(System.String)row["CustomField1"];
					c.CustomField2 = (Convert.IsDBNull(row["CustomField2"]))?string.Empty:(System.String)row["CustomField2"];
					c.CustomField3 = (Convert.IsDBNull(row["CustomField3"]))?string.Empty:(System.String)row["CustomField3"];
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
		/// Fill an <see cref="VList&lt;METAView_FunctionalDependency_Listing&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAView_FunctionalDependency_Listing&gt;"/></returns>
		protected VList<METAView_FunctionalDependency_Listing> Fill(IDataReader reader, VList<METAView_FunctionalDependency_Listing> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAView_FunctionalDependency_Listing entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAView_FunctionalDependency_Listing>("METAView_FunctionalDependency_Listing",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAView_FunctionalDependency_Listing();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAView_FunctionalDependency_ListingColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_FunctionalDependency_ListingColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_FunctionalDependency_ListingColumn.WorkspaceTypeId)];
					//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
					entity.VCStatusID = (System.Int32)reader[((int)METAView_FunctionalDependency_ListingColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.pkid = (System.Int32)reader[((int)METAView_FunctionalDependency_ListingColumn.pkid)];
					//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAView_FunctionalDependency_ListingColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAView_FunctionalDependency_ListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_FunctionalDependency_ListingColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.InferenceRule = (reader.IsDBNull(((int)METAView_FunctionalDependency_ListingColumn.InferenceRule)))?null:(System.String)reader[((int)METAView_FunctionalDependency_ListingColumn.InferenceRule)];
					//entity.InferenceRule = (Convert.IsDBNull(reader["InferenceRule"]))?string.Empty:(System.String)reader["InferenceRule"];
					entity.Condition = (reader.IsDBNull(((int)METAView_FunctionalDependency_ListingColumn.Condition)))?null:(System.String)reader[((int)METAView_FunctionalDependency_ListingColumn.Condition)];
					//entity.Condition = (Convert.IsDBNull(reader["Condition"]))?string.Empty:(System.String)reader["Condition"];
					entity.Description = (reader.IsDBNull(((int)METAView_FunctionalDependency_ListingColumn.Description)))?null:(System.String)reader[((int)METAView_FunctionalDependency_ListingColumn.Description)];
					//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
					entity.CohesionWeight = (reader.IsDBNull(((int)METAView_FunctionalDependency_ListingColumn.CohesionWeight)))?null:(System.String)reader[((int)METAView_FunctionalDependency_ListingColumn.CohesionWeight)];
					//entity.CohesionWeight = (Convert.IsDBNull(reader["CohesionWeight"]))?string.Empty:(System.String)reader["CohesionWeight"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAView_FunctionalDependency_ListingColumn.CustomField1)))?null:(System.String)reader[((int)METAView_FunctionalDependency_ListingColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAView_FunctionalDependency_ListingColumn.CustomField2)))?null:(System.String)reader[((int)METAView_FunctionalDependency_ListingColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAView_FunctionalDependency_ListingColumn.CustomField3)))?null:(System.String)reader[((int)METAView_FunctionalDependency_ListingColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.GapType = (reader.IsDBNull(((int)METAView_FunctionalDependency_ListingColumn.GapType)))?null:(System.String)reader[((int)METAView_FunctionalDependency_ListingColumn.GapType)];
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
		/// Refreshes the <see cref="METAView_FunctionalDependency_Listing"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_FunctionalDependency_Listing"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAView_FunctionalDependency_Listing entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAView_FunctionalDependency_ListingColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_FunctionalDependency_ListingColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_FunctionalDependency_ListingColumn.WorkspaceTypeId)];
			//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
			entity.VCStatusID = (System.Int32)reader[((int)METAView_FunctionalDependency_ListingColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.pkid = (System.Int32)reader[((int)METAView_FunctionalDependency_ListingColumn.pkid)];
			//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAView_FunctionalDependency_ListingColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAView_FunctionalDependency_ListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_FunctionalDependency_ListingColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.InferenceRule = (reader.IsDBNull(((int)METAView_FunctionalDependency_ListingColumn.InferenceRule)))?null:(System.String)reader[((int)METAView_FunctionalDependency_ListingColumn.InferenceRule)];
			//entity.InferenceRule = (Convert.IsDBNull(reader["InferenceRule"]))?string.Empty:(System.String)reader["InferenceRule"];
			entity.Condition = (reader.IsDBNull(((int)METAView_FunctionalDependency_ListingColumn.Condition)))?null:(System.String)reader[((int)METAView_FunctionalDependency_ListingColumn.Condition)];
			//entity.Condition = (Convert.IsDBNull(reader["Condition"]))?string.Empty:(System.String)reader["Condition"];
			entity.Description = (reader.IsDBNull(((int)METAView_FunctionalDependency_ListingColumn.Description)))?null:(System.String)reader[((int)METAView_FunctionalDependency_ListingColumn.Description)];
			//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
			entity.CohesionWeight = (reader.IsDBNull(((int)METAView_FunctionalDependency_ListingColumn.CohesionWeight)))?null:(System.String)reader[((int)METAView_FunctionalDependency_ListingColumn.CohesionWeight)];
			//entity.CohesionWeight = (Convert.IsDBNull(reader["CohesionWeight"]))?string.Empty:(System.String)reader["CohesionWeight"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAView_FunctionalDependency_ListingColumn.CustomField1)))?null:(System.String)reader[((int)METAView_FunctionalDependency_ListingColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAView_FunctionalDependency_ListingColumn.CustomField2)))?null:(System.String)reader[((int)METAView_FunctionalDependency_ListingColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAView_FunctionalDependency_ListingColumn.CustomField3)))?null:(System.String)reader[((int)METAView_FunctionalDependency_ListingColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.GapType = (reader.IsDBNull(((int)METAView_FunctionalDependency_ListingColumn.GapType)))?null:(System.String)reader[((int)METAView_FunctionalDependency_ListingColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAView_FunctionalDependency_Listing"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_FunctionalDependency_Listing"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAView_FunctionalDependency_Listing entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeId = (Convert.IsDBNull(dataRow["WorkspaceTypeId"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeId"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.InferenceRule = (Convert.IsDBNull(dataRow["InferenceRule"]))?string.Empty:(System.String)dataRow["InferenceRule"];
			entity.Condition = (Convert.IsDBNull(dataRow["Condition"]))?string.Empty:(System.String)dataRow["Condition"];
			entity.Description = (Convert.IsDBNull(dataRow["Description"]))?string.Empty:(System.String)dataRow["Description"];
			entity.CohesionWeight = (Convert.IsDBNull(dataRow["CohesionWeight"]))?string.Empty:(System.String)dataRow["CohesionWeight"];
			entity.CustomField1 = (Convert.IsDBNull(dataRow["CustomField1"]))?string.Empty:(System.String)dataRow["CustomField1"];
			entity.CustomField2 = (Convert.IsDBNull(dataRow["CustomField2"]))?string.Empty:(System.String)dataRow["CustomField2"];
			entity.CustomField3 = (Convert.IsDBNull(dataRow["CustomField3"]))?string.Empty:(System.String)dataRow["CustomField3"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAView_FunctionalDependency_ListingFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_FunctionalDependency_Listing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_FunctionalDependency_ListingFilterBuilder : SqlFilterBuilder<METAView_FunctionalDependency_ListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_FunctionalDependency_ListingFilterBuilder class.
		/// </summary>
		public METAView_FunctionalDependency_ListingFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_FunctionalDependency_ListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_FunctionalDependency_ListingFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_FunctionalDependency_ListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_FunctionalDependency_ListingFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_FunctionalDependency_ListingFilterBuilder

	#region METAView_FunctionalDependency_ListingParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_FunctionalDependency_Listing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_FunctionalDependency_ListingParameterBuilder : ParameterizedSqlFilterBuilder<METAView_FunctionalDependency_ListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_FunctionalDependency_ListingParameterBuilder class.
		/// </summary>
		public METAView_FunctionalDependency_ListingParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_FunctionalDependency_ListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_FunctionalDependency_ListingParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_FunctionalDependency_ListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_FunctionalDependency_ListingParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_FunctionalDependency_ListingParameterBuilder
	
	#region METAView_FunctionalDependency_ListingSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_FunctionalDependency_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_FunctionalDependency_ListingSortBuilder : SqlSortBuilder<METAView_FunctionalDependency_ListingColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_FunctionalDependency_ListingSqlSortBuilder class.
		/// </summary>
		public METAView_FunctionalDependency_ListingSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAView_FunctionalDependency_ListingSortBuilder

} // end namespace
