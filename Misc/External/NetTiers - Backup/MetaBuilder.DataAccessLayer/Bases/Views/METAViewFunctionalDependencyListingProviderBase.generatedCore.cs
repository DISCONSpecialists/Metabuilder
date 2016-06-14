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
	/// This class is the base class for any <see cref="METAViewFunctionalDependencyListingProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAViewFunctionalDependencyListingProviderBaseCore : EntityViewProviderBase<METAViewFunctionalDependencyListing>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAViewFunctionalDependencyListing&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAViewFunctionalDependencyListing&gt;"/></returns>
		protected static VList&lt;METAViewFunctionalDependencyListing&gt; Fill(DataSet dataSet, VList<METAViewFunctionalDependencyListing> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAViewFunctionalDependencyListing>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAViewFunctionalDependencyListing&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAViewFunctionalDependencyListing>"/></returns>
		protected static VList&lt;METAViewFunctionalDependencyListing&gt; Fill(DataTable dataTable, VList<METAViewFunctionalDependencyListing> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAViewFunctionalDependencyListing c = new METAViewFunctionalDependencyListing();
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
		/// Fill an <see cref="VList&lt;METAViewFunctionalDependencyListing&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAViewFunctionalDependencyListing&gt;"/></returns>
		protected VList<METAViewFunctionalDependencyListing> Fill(IDataReader reader, VList<METAViewFunctionalDependencyListing> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAViewFunctionalDependencyListing entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAViewFunctionalDependencyListing>("METAViewFunctionalDependencyListing",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAViewFunctionalDependencyListing();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAViewFunctionalDependencyListingColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewFunctionalDependencyListingColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewFunctionalDependencyListingColumn.WorkspaceTypeID)];
					//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
					entity.VCStatusID = (System.Int32)reader[((int)METAViewFunctionalDependencyListingColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.Pkid = (System.Int32)reader[((int)METAViewFunctionalDependencyListingColumn.Pkid)];
					//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAViewFunctionalDependencyListingColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAViewFunctionalDependencyListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewFunctionalDependencyListingColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.InferenceRule = (reader.IsDBNull(((int)METAViewFunctionalDependencyListingColumn.InferenceRule)))?null:(System.String)reader[((int)METAViewFunctionalDependencyListingColumn.InferenceRule)];
					//entity.InferenceRule = (Convert.IsDBNull(reader["InferenceRule"]))?string.Empty:(System.String)reader["InferenceRule"];
					entity.Condition = (reader.IsDBNull(((int)METAViewFunctionalDependencyListingColumn.Condition)))?null:(System.String)reader[((int)METAViewFunctionalDependencyListingColumn.Condition)];
					//entity.Condition = (Convert.IsDBNull(reader["Condition"]))?string.Empty:(System.String)reader["Condition"];
					entity.Description = (reader.IsDBNull(((int)METAViewFunctionalDependencyListingColumn.Description)))?null:(System.String)reader[((int)METAViewFunctionalDependencyListingColumn.Description)];
					//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
					entity.CohesionWeight = (reader.IsDBNull(((int)METAViewFunctionalDependencyListingColumn.CohesionWeight)))?null:(System.String)reader[((int)METAViewFunctionalDependencyListingColumn.CohesionWeight)];
					//entity.CohesionWeight = (Convert.IsDBNull(reader["CohesionWeight"]))?string.Empty:(System.String)reader["CohesionWeight"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAViewFunctionalDependencyListingColumn.CustomField1)))?null:(System.String)reader[((int)METAViewFunctionalDependencyListingColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAViewFunctionalDependencyListingColumn.CustomField2)))?null:(System.String)reader[((int)METAViewFunctionalDependencyListingColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAViewFunctionalDependencyListingColumn.CustomField3)))?null:(System.String)reader[((int)METAViewFunctionalDependencyListingColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.GapType = (reader.IsDBNull(((int)METAViewFunctionalDependencyListingColumn.GapType)))?null:(System.String)reader[((int)METAViewFunctionalDependencyListingColumn.GapType)];
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
		/// Refreshes the <see cref="METAViewFunctionalDependencyListing"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewFunctionalDependencyListing"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAViewFunctionalDependencyListing entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAViewFunctionalDependencyListingColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewFunctionalDependencyListingColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewFunctionalDependencyListingColumn.WorkspaceTypeID)];
			//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
			entity.VCStatusID = (System.Int32)reader[((int)METAViewFunctionalDependencyListingColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.Pkid = (System.Int32)reader[((int)METAViewFunctionalDependencyListingColumn.Pkid)];
			//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAViewFunctionalDependencyListingColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAViewFunctionalDependencyListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewFunctionalDependencyListingColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.InferenceRule = (reader.IsDBNull(((int)METAViewFunctionalDependencyListingColumn.InferenceRule)))?null:(System.String)reader[((int)METAViewFunctionalDependencyListingColumn.InferenceRule)];
			//entity.InferenceRule = (Convert.IsDBNull(reader["InferenceRule"]))?string.Empty:(System.String)reader["InferenceRule"];
			entity.Condition = (reader.IsDBNull(((int)METAViewFunctionalDependencyListingColumn.Condition)))?null:(System.String)reader[((int)METAViewFunctionalDependencyListingColumn.Condition)];
			//entity.Condition = (Convert.IsDBNull(reader["Condition"]))?string.Empty:(System.String)reader["Condition"];
			entity.Description = (reader.IsDBNull(((int)METAViewFunctionalDependencyListingColumn.Description)))?null:(System.String)reader[((int)METAViewFunctionalDependencyListingColumn.Description)];
			//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
			entity.CohesionWeight = (reader.IsDBNull(((int)METAViewFunctionalDependencyListingColumn.CohesionWeight)))?null:(System.String)reader[((int)METAViewFunctionalDependencyListingColumn.CohesionWeight)];
			//entity.CohesionWeight = (Convert.IsDBNull(reader["CohesionWeight"]))?string.Empty:(System.String)reader["CohesionWeight"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAViewFunctionalDependencyListingColumn.CustomField1)))?null:(System.String)reader[((int)METAViewFunctionalDependencyListingColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAViewFunctionalDependencyListingColumn.CustomField2)))?null:(System.String)reader[((int)METAViewFunctionalDependencyListingColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAViewFunctionalDependencyListingColumn.CustomField3)))?null:(System.String)reader[((int)METAViewFunctionalDependencyListingColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.GapType = (reader.IsDBNull(((int)METAViewFunctionalDependencyListingColumn.GapType)))?null:(System.String)reader[((int)METAViewFunctionalDependencyListingColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAViewFunctionalDependencyListing"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewFunctionalDependencyListing"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAViewFunctionalDependencyListing entity)
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

	#region METAViewFunctionalDependencyListingFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewFunctionalDependencyListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewFunctionalDependencyListingFilterBuilder : SqlFilterBuilder<METAViewFunctionalDependencyListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewFunctionalDependencyListingFilterBuilder class.
		/// </summary>
		public METAViewFunctionalDependencyListingFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewFunctionalDependencyListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewFunctionalDependencyListingFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewFunctionalDependencyListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewFunctionalDependencyListingFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewFunctionalDependencyListingFilterBuilder

	#region METAViewFunctionalDependencyListingParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewFunctionalDependencyListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewFunctionalDependencyListingParameterBuilder : ParameterizedSqlFilterBuilder<METAViewFunctionalDependencyListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewFunctionalDependencyListingParameterBuilder class.
		/// </summary>
		public METAViewFunctionalDependencyListingParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewFunctionalDependencyListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewFunctionalDependencyListingParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewFunctionalDependencyListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewFunctionalDependencyListingParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewFunctionalDependencyListingParameterBuilder
	
	#region METAViewFunctionalDependencyListingSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewFunctionalDependencyListing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAViewFunctionalDependencyListingSortBuilder : SqlSortBuilder<METAViewFunctionalDependencyListingColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewFunctionalDependencyListingSqlSortBuilder class.
		/// </summary>
		public METAViewFunctionalDependencyListingSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAViewFunctionalDependencyListingSortBuilder

} // end namespace
