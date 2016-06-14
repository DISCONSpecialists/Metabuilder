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
	/// This class is the base class for any <see cref="METAView_DataTable_RetrievalProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAView_DataTable_RetrievalProviderBaseCore : EntityViewProviderBase<METAView_DataTable_Retrieval>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAView_DataTable_Retrieval&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAView_DataTable_Retrieval&gt;"/></returns>
		protected static VList&lt;METAView_DataTable_Retrieval&gt; Fill(DataSet dataSet, VList<METAView_DataTable_Retrieval> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAView_DataTable_Retrieval>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAView_DataTable_Retrieval&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAView_DataTable_Retrieval>"/></returns>
		protected static VList&lt;METAView_DataTable_Retrieval&gt; Fill(DataTable dataTable, VList<METAView_DataTable_Retrieval> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAView_DataTable_Retrieval c = new METAView_DataTable_Retrieval();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeId = (Convert.IsDBNull(row["WorkspaceTypeId"]))?(int)0:(System.Int32?)row["WorkspaceTypeId"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.Name = (Convert.IsDBNull(row["Name"]))?string.Empty:(System.String)row["Name"];
					c.InitialPopulation = (Convert.IsDBNull(row["InitialPopulation"]))?(int)0:(System.Int32?)row["InitialPopulation"];
					c.GrowthRateOverTime = (Convert.IsDBNull(row["GrowthRateOverTime"]))?string.Empty:(System.String)row["GrowthRateOverTime"];
					c.RecordSize = (Convert.IsDBNull(row["RecordSize"]))?string.Empty:(System.String)row["RecordSize"];
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
		/// Fill an <see cref="VList&lt;METAView_DataTable_Retrieval&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAView_DataTable_Retrieval&gt;"/></returns>
		protected VList<METAView_DataTable_Retrieval> Fill(IDataReader reader, VList<METAView_DataTable_Retrieval> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAView_DataTable_Retrieval entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAView_DataTable_Retrieval>("METAView_DataTable_Retrieval",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAView_DataTable_Retrieval();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAView_DataTable_RetrievalColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_DataTable_RetrievalColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_DataTable_RetrievalColumn.WorkspaceTypeId)];
					//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
					entity.VCStatusID = (System.Int32)reader[((int)METAView_DataTable_RetrievalColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.pkid = (System.Int32)reader[((int)METAView_DataTable_RetrievalColumn.pkid)];
					//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAView_DataTable_RetrievalColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAView_DataTable_RetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_DataTable_RetrievalColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Name = (reader.IsDBNull(((int)METAView_DataTable_RetrievalColumn.Name)))?null:(System.String)reader[((int)METAView_DataTable_RetrievalColumn.Name)];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.InitialPopulation = (reader.IsDBNull(((int)METAView_DataTable_RetrievalColumn.InitialPopulation)))?null:(System.Int32?)reader[((int)METAView_DataTable_RetrievalColumn.InitialPopulation)];
					//entity.InitialPopulation = (Convert.IsDBNull(reader["InitialPopulation"]))?(int)0:(System.Int32?)reader["InitialPopulation"];
					entity.GrowthRateOverTime = (reader.IsDBNull(((int)METAView_DataTable_RetrievalColumn.GrowthRateOverTime)))?null:(System.String)reader[((int)METAView_DataTable_RetrievalColumn.GrowthRateOverTime)];
					//entity.GrowthRateOverTime = (Convert.IsDBNull(reader["GrowthRateOverTime"]))?string.Empty:(System.String)reader["GrowthRateOverTime"];
					entity.RecordSize = (reader.IsDBNull(((int)METAView_DataTable_RetrievalColumn.RecordSize)))?null:(System.String)reader[((int)METAView_DataTable_RetrievalColumn.RecordSize)];
					//entity.RecordSize = (Convert.IsDBNull(reader["RecordSize"]))?string.Empty:(System.String)reader["RecordSize"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAView_DataTable_RetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAView_DataTable_RetrievalColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAView_DataTable_RetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAView_DataTable_RetrievalColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAView_DataTable_RetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAView_DataTable_RetrievalColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.GapType = (reader.IsDBNull(((int)METAView_DataTable_RetrievalColumn.GapType)))?null:(System.String)reader[((int)METAView_DataTable_RetrievalColumn.GapType)];
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
		/// Refreshes the <see cref="METAView_DataTable_Retrieval"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_DataTable_Retrieval"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAView_DataTable_Retrieval entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAView_DataTable_RetrievalColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_DataTable_RetrievalColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_DataTable_RetrievalColumn.WorkspaceTypeId)];
			//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
			entity.VCStatusID = (System.Int32)reader[((int)METAView_DataTable_RetrievalColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.pkid = (System.Int32)reader[((int)METAView_DataTable_RetrievalColumn.pkid)];
			//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAView_DataTable_RetrievalColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAView_DataTable_RetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_DataTable_RetrievalColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Name = (reader.IsDBNull(((int)METAView_DataTable_RetrievalColumn.Name)))?null:(System.String)reader[((int)METAView_DataTable_RetrievalColumn.Name)];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.InitialPopulation = (reader.IsDBNull(((int)METAView_DataTable_RetrievalColumn.InitialPopulation)))?null:(System.Int32?)reader[((int)METAView_DataTable_RetrievalColumn.InitialPopulation)];
			//entity.InitialPopulation = (Convert.IsDBNull(reader["InitialPopulation"]))?(int)0:(System.Int32?)reader["InitialPopulation"];
			entity.GrowthRateOverTime = (reader.IsDBNull(((int)METAView_DataTable_RetrievalColumn.GrowthRateOverTime)))?null:(System.String)reader[((int)METAView_DataTable_RetrievalColumn.GrowthRateOverTime)];
			//entity.GrowthRateOverTime = (Convert.IsDBNull(reader["GrowthRateOverTime"]))?string.Empty:(System.String)reader["GrowthRateOverTime"];
			entity.RecordSize = (reader.IsDBNull(((int)METAView_DataTable_RetrievalColumn.RecordSize)))?null:(System.String)reader[((int)METAView_DataTable_RetrievalColumn.RecordSize)];
			//entity.RecordSize = (Convert.IsDBNull(reader["RecordSize"]))?string.Empty:(System.String)reader["RecordSize"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAView_DataTable_RetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAView_DataTable_RetrievalColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAView_DataTable_RetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAView_DataTable_RetrievalColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAView_DataTable_RetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAView_DataTable_RetrievalColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.GapType = (reader.IsDBNull(((int)METAView_DataTable_RetrievalColumn.GapType)))?null:(System.String)reader[((int)METAView_DataTable_RetrievalColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAView_DataTable_Retrieval"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_DataTable_Retrieval"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAView_DataTable_Retrieval entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeId = (Convert.IsDBNull(dataRow["WorkspaceTypeId"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeId"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.Name = (Convert.IsDBNull(dataRow["Name"]))?string.Empty:(System.String)dataRow["Name"];
			entity.InitialPopulation = (Convert.IsDBNull(dataRow["InitialPopulation"]))?(int)0:(System.Int32?)dataRow["InitialPopulation"];
			entity.GrowthRateOverTime = (Convert.IsDBNull(dataRow["GrowthRateOverTime"]))?string.Empty:(System.String)dataRow["GrowthRateOverTime"];
			entity.RecordSize = (Convert.IsDBNull(dataRow["RecordSize"]))?string.Empty:(System.String)dataRow["RecordSize"];
			entity.CustomField1 = (Convert.IsDBNull(dataRow["CustomField1"]))?string.Empty:(System.String)dataRow["CustomField1"];
			entity.CustomField2 = (Convert.IsDBNull(dataRow["CustomField2"]))?string.Empty:(System.String)dataRow["CustomField2"];
			entity.CustomField3 = (Convert.IsDBNull(dataRow["CustomField3"]))?string.Empty:(System.String)dataRow["CustomField3"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAView_DataTable_RetrievalFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_DataTable_Retrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_DataTable_RetrievalFilterBuilder : SqlFilterBuilder<METAView_DataTable_RetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_DataTable_RetrievalFilterBuilder class.
		/// </summary>
		public METAView_DataTable_RetrievalFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_DataTable_RetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_DataTable_RetrievalFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_DataTable_RetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_DataTable_RetrievalFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_DataTable_RetrievalFilterBuilder

	#region METAView_DataTable_RetrievalParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_DataTable_Retrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_DataTable_RetrievalParameterBuilder : ParameterizedSqlFilterBuilder<METAView_DataTable_RetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_DataTable_RetrievalParameterBuilder class.
		/// </summary>
		public METAView_DataTable_RetrievalParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_DataTable_RetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_DataTable_RetrievalParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_DataTable_RetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_DataTable_RetrievalParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_DataTable_RetrievalParameterBuilder
	
	#region METAView_DataTable_RetrievalSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_DataTable_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataTable_RetrievalSortBuilder : SqlSortBuilder<METAView_DataTable_RetrievalColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_DataTable_RetrievalSqlSortBuilder class.
		/// </summary>
		public METAView_DataTable_RetrievalSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAView_DataTable_RetrievalSortBuilder

} // end namespace
