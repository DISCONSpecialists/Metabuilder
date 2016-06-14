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
	/// This class is the base class for any <see cref="METAViewFunctionalDependencyRetrievalProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAViewFunctionalDependencyRetrievalProviderBaseCore : EntityViewProviderBase<METAViewFunctionalDependencyRetrieval>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAViewFunctionalDependencyRetrieval&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAViewFunctionalDependencyRetrieval&gt;"/></returns>
		protected static VList&lt;METAViewFunctionalDependencyRetrieval&gt; Fill(DataSet dataSet, VList<METAViewFunctionalDependencyRetrieval> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAViewFunctionalDependencyRetrieval>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAViewFunctionalDependencyRetrieval&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAViewFunctionalDependencyRetrieval>"/></returns>
		protected static VList&lt;METAViewFunctionalDependencyRetrieval&gt; Fill(DataTable dataTable, VList<METAViewFunctionalDependencyRetrieval> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAViewFunctionalDependencyRetrieval c = new METAViewFunctionalDependencyRetrieval();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeID = (Convert.IsDBNull(row["WorkspaceTypeID"]))?(int)0:(System.Int32?)row["WorkspaceTypeID"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.Pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
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
		/// Fill an <see cref="VList&lt;METAViewFunctionalDependencyRetrieval&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAViewFunctionalDependencyRetrieval&gt;"/></returns>
		protected VList<METAViewFunctionalDependencyRetrieval> Fill(IDataReader reader, VList<METAViewFunctionalDependencyRetrieval> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAViewFunctionalDependencyRetrieval entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAViewFunctionalDependencyRetrieval>("METAViewFunctionalDependencyRetrieval",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAViewFunctionalDependencyRetrieval();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAViewFunctionalDependencyRetrievalColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewFunctionalDependencyRetrievalColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewFunctionalDependencyRetrievalColumn.WorkspaceTypeID)];
					//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
					entity.VCStatusID = (System.Int32)reader[((int)METAViewFunctionalDependencyRetrievalColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.Pkid = (System.Int32)reader[((int)METAViewFunctionalDependencyRetrievalColumn.Pkid)];
					//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAViewFunctionalDependencyRetrievalColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAViewFunctionalDependencyRetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewFunctionalDependencyRetrievalColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.InferenceRule = (reader.IsDBNull(((int)METAViewFunctionalDependencyRetrievalColumn.InferenceRule)))?null:(System.String)reader[((int)METAViewFunctionalDependencyRetrievalColumn.InferenceRule)];
					//entity.InferenceRule = (Convert.IsDBNull(reader["InferenceRule"]))?string.Empty:(System.String)reader["InferenceRule"];
					entity.Condition = (reader.IsDBNull(((int)METAViewFunctionalDependencyRetrievalColumn.Condition)))?null:(System.String)reader[((int)METAViewFunctionalDependencyRetrievalColumn.Condition)];
					//entity.Condition = (Convert.IsDBNull(reader["Condition"]))?string.Empty:(System.String)reader["Condition"];
					entity.Description = (reader.IsDBNull(((int)METAViewFunctionalDependencyRetrievalColumn.Description)))?null:(System.String)reader[((int)METAViewFunctionalDependencyRetrievalColumn.Description)];
					//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
					entity.CohesionWeight = (reader.IsDBNull(((int)METAViewFunctionalDependencyRetrievalColumn.CohesionWeight)))?null:(System.String)reader[((int)METAViewFunctionalDependencyRetrievalColumn.CohesionWeight)];
					//entity.CohesionWeight = (Convert.IsDBNull(reader["CohesionWeight"]))?string.Empty:(System.String)reader["CohesionWeight"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAViewFunctionalDependencyRetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAViewFunctionalDependencyRetrievalColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAViewFunctionalDependencyRetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAViewFunctionalDependencyRetrievalColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAViewFunctionalDependencyRetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAViewFunctionalDependencyRetrievalColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.GapType = (reader.IsDBNull(((int)METAViewFunctionalDependencyRetrievalColumn.GapType)))?null:(System.String)reader[((int)METAViewFunctionalDependencyRetrievalColumn.GapType)];
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
		/// Refreshes the <see cref="METAViewFunctionalDependencyRetrieval"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewFunctionalDependencyRetrieval"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAViewFunctionalDependencyRetrieval entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAViewFunctionalDependencyRetrievalColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewFunctionalDependencyRetrievalColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewFunctionalDependencyRetrievalColumn.WorkspaceTypeID)];
			//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
			entity.VCStatusID = (System.Int32)reader[((int)METAViewFunctionalDependencyRetrievalColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.Pkid = (System.Int32)reader[((int)METAViewFunctionalDependencyRetrievalColumn.Pkid)];
			//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAViewFunctionalDependencyRetrievalColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAViewFunctionalDependencyRetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewFunctionalDependencyRetrievalColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.InferenceRule = (reader.IsDBNull(((int)METAViewFunctionalDependencyRetrievalColumn.InferenceRule)))?null:(System.String)reader[((int)METAViewFunctionalDependencyRetrievalColumn.InferenceRule)];
			//entity.InferenceRule = (Convert.IsDBNull(reader["InferenceRule"]))?string.Empty:(System.String)reader["InferenceRule"];
			entity.Condition = (reader.IsDBNull(((int)METAViewFunctionalDependencyRetrievalColumn.Condition)))?null:(System.String)reader[((int)METAViewFunctionalDependencyRetrievalColumn.Condition)];
			//entity.Condition = (Convert.IsDBNull(reader["Condition"]))?string.Empty:(System.String)reader["Condition"];
			entity.Description = (reader.IsDBNull(((int)METAViewFunctionalDependencyRetrievalColumn.Description)))?null:(System.String)reader[((int)METAViewFunctionalDependencyRetrievalColumn.Description)];
			//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
			entity.CohesionWeight = (reader.IsDBNull(((int)METAViewFunctionalDependencyRetrievalColumn.CohesionWeight)))?null:(System.String)reader[((int)METAViewFunctionalDependencyRetrievalColumn.CohesionWeight)];
			//entity.CohesionWeight = (Convert.IsDBNull(reader["CohesionWeight"]))?string.Empty:(System.String)reader["CohesionWeight"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAViewFunctionalDependencyRetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAViewFunctionalDependencyRetrievalColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAViewFunctionalDependencyRetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAViewFunctionalDependencyRetrievalColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAViewFunctionalDependencyRetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAViewFunctionalDependencyRetrievalColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.GapType = (reader.IsDBNull(((int)METAViewFunctionalDependencyRetrievalColumn.GapType)))?null:(System.String)reader[((int)METAViewFunctionalDependencyRetrievalColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAViewFunctionalDependencyRetrieval"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewFunctionalDependencyRetrieval"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAViewFunctionalDependencyRetrieval entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeID = (Convert.IsDBNull(dataRow["WorkspaceTypeID"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeID"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.Pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
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

	#region METAViewFunctionalDependencyRetrievalFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewFunctionalDependencyRetrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewFunctionalDependencyRetrievalFilterBuilder : SqlFilterBuilder<METAViewFunctionalDependencyRetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewFunctionalDependencyRetrievalFilterBuilder class.
		/// </summary>
		public METAViewFunctionalDependencyRetrievalFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewFunctionalDependencyRetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewFunctionalDependencyRetrievalFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewFunctionalDependencyRetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewFunctionalDependencyRetrievalFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewFunctionalDependencyRetrievalFilterBuilder

	#region METAViewFunctionalDependencyRetrievalParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewFunctionalDependencyRetrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewFunctionalDependencyRetrievalParameterBuilder : ParameterizedSqlFilterBuilder<METAViewFunctionalDependencyRetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewFunctionalDependencyRetrievalParameterBuilder class.
		/// </summary>
		public METAViewFunctionalDependencyRetrievalParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewFunctionalDependencyRetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewFunctionalDependencyRetrievalParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewFunctionalDependencyRetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewFunctionalDependencyRetrievalParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewFunctionalDependencyRetrievalParameterBuilder
	
	#region METAViewFunctionalDependencyRetrievalSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewFunctionalDependencyRetrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAViewFunctionalDependencyRetrievalSortBuilder : SqlSortBuilder<METAViewFunctionalDependencyRetrievalColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewFunctionalDependencyRetrievalSqlSortBuilder class.
		/// </summary>
		public METAViewFunctionalDependencyRetrievalSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAViewFunctionalDependencyRetrievalSortBuilder

} // end namespace
