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
	/// This class is the base class for any <see cref="METAViewProcessListingProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAViewProcessListingProviderBaseCore : EntityViewProviderBase<METAViewProcessListing>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAViewProcessListing&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAViewProcessListing&gt;"/></returns>
		protected static VList&lt;METAViewProcessListing&gt; Fill(DataSet dataSet, VList<METAViewProcessListing> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAViewProcessListing>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAViewProcessListing&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAViewProcessListing>"/></returns>
		protected static VList&lt;METAViewProcessListing&gt; Fill(DataTable dataTable, VList<METAViewProcessListing> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAViewProcessListing c = new METAViewProcessListing();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeID = (Convert.IsDBNull(row["WorkspaceTypeID"]))?(int)0:(System.Int32?)row["WorkspaceTypeID"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.Pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.Name = (Convert.IsDBNull(row["Name"]))?string.Empty:(System.String)row["Name"];
					c.ExecutionIndicator = (Convert.IsDBNull(row["ExecutionIndicator"]))?string.Empty:(System.String)row["ExecutionIndicator"];
					c.ContextualIndicator = (Convert.IsDBNull(row["ContextualIndicator"]))?string.Empty:(System.String)row["ContextualIndicator"];
					c.SequenceNumber = (Convert.IsDBNull(row["SequenceNumber"]))?string.Empty:(System.String)row["SequenceNumber"];
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
		/// Fill an <see cref="VList&lt;METAViewProcessListing&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAViewProcessListing&gt;"/></returns>
		protected VList<METAViewProcessListing> Fill(IDataReader reader, VList<METAViewProcessListing> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAViewProcessListing entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAViewProcessListing>("METAViewProcessListing",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAViewProcessListing();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAViewProcessListingColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewProcessListingColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewProcessListingColumn.WorkspaceTypeID)];
					//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
					entity.VCStatusID = (System.Int32)reader[((int)METAViewProcessListingColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.Pkid = (System.Int32)reader[((int)METAViewProcessListingColumn.Pkid)];
					//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAViewProcessListingColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAViewProcessListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewProcessListingColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Name = (reader.IsDBNull(((int)METAViewProcessListingColumn.Name)))?null:(System.String)reader[((int)METAViewProcessListingColumn.Name)];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.ExecutionIndicator = (reader.IsDBNull(((int)METAViewProcessListingColumn.ExecutionIndicator)))?null:(System.String)reader[((int)METAViewProcessListingColumn.ExecutionIndicator)];
					//entity.ExecutionIndicator = (Convert.IsDBNull(reader["ExecutionIndicator"]))?string.Empty:(System.String)reader["ExecutionIndicator"];
					entity.ContextualIndicator = (reader.IsDBNull(((int)METAViewProcessListingColumn.ContextualIndicator)))?null:(System.String)reader[((int)METAViewProcessListingColumn.ContextualIndicator)];
					//entity.ContextualIndicator = (Convert.IsDBNull(reader["ContextualIndicator"]))?string.Empty:(System.String)reader["ContextualIndicator"];
					entity.SequenceNumber = (reader.IsDBNull(((int)METAViewProcessListingColumn.SequenceNumber)))?null:(System.String)reader[((int)METAViewProcessListingColumn.SequenceNumber)];
					//entity.SequenceNumber = (Convert.IsDBNull(reader["SequenceNumber"]))?string.Empty:(System.String)reader["SequenceNumber"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAViewProcessListingColumn.CustomField1)))?null:(System.String)reader[((int)METAViewProcessListingColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAViewProcessListingColumn.CustomField2)))?null:(System.String)reader[((int)METAViewProcessListingColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAViewProcessListingColumn.CustomField3)))?null:(System.String)reader[((int)METAViewProcessListingColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.GapType = (reader.IsDBNull(((int)METAViewProcessListingColumn.GapType)))?null:(System.String)reader[((int)METAViewProcessListingColumn.GapType)];
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
		/// Refreshes the <see cref="METAViewProcessListing"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewProcessListing"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAViewProcessListing entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAViewProcessListingColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewProcessListingColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewProcessListingColumn.WorkspaceTypeID)];
			//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
			entity.VCStatusID = (System.Int32)reader[((int)METAViewProcessListingColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.Pkid = (System.Int32)reader[((int)METAViewProcessListingColumn.Pkid)];
			//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAViewProcessListingColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAViewProcessListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewProcessListingColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Name = (reader.IsDBNull(((int)METAViewProcessListingColumn.Name)))?null:(System.String)reader[((int)METAViewProcessListingColumn.Name)];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.ExecutionIndicator = (reader.IsDBNull(((int)METAViewProcessListingColumn.ExecutionIndicator)))?null:(System.String)reader[((int)METAViewProcessListingColumn.ExecutionIndicator)];
			//entity.ExecutionIndicator = (Convert.IsDBNull(reader["ExecutionIndicator"]))?string.Empty:(System.String)reader["ExecutionIndicator"];
			entity.ContextualIndicator = (reader.IsDBNull(((int)METAViewProcessListingColumn.ContextualIndicator)))?null:(System.String)reader[((int)METAViewProcessListingColumn.ContextualIndicator)];
			//entity.ContextualIndicator = (Convert.IsDBNull(reader["ContextualIndicator"]))?string.Empty:(System.String)reader["ContextualIndicator"];
			entity.SequenceNumber = (reader.IsDBNull(((int)METAViewProcessListingColumn.SequenceNumber)))?null:(System.String)reader[((int)METAViewProcessListingColumn.SequenceNumber)];
			//entity.SequenceNumber = (Convert.IsDBNull(reader["SequenceNumber"]))?string.Empty:(System.String)reader["SequenceNumber"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAViewProcessListingColumn.CustomField1)))?null:(System.String)reader[((int)METAViewProcessListingColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAViewProcessListingColumn.CustomField2)))?null:(System.String)reader[((int)METAViewProcessListingColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAViewProcessListingColumn.CustomField3)))?null:(System.String)reader[((int)METAViewProcessListingColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.GapType = (reader.IsDBNull(((int)METAViewProcessListingColumn.GapType)))?null:(System.String)reader[((int)METAViewProcessListingColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAViewProcessListing"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewProcessListing"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAViewProcessListing entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeID = (Convert.IsDBNull(dataRow["WorkspaceTypeID"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeID"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.Pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.Name = (Convert.IsDBNull(dataRow["Name"]))?string.Empty:(System.String)dataRow["Name"];
			entity.ExecutionIndicator = (Convert.IsDBNull(dataRow["ExecutionIndicator"]))?string.Empty:(System.String)dataRow["ExecutionIndicator"];
			entity.ContextualIndicator = (Convert.IsDBNull(dataRow["ContextualIndicator"]))?string.Empty:(System.String)dataRow["ContextualIndicator"];
			entity.SequenceNumber = (Convert.IsDBNull(dataRow["SequenceNumber"]))?string.Empty:(System.String)dataRow["SequenceNumber"];
			entity.CustomField1 = (Convert.IsDBNull(dataRow["CustomField1"]))?string.Empty:(System.String)dataRow["CustomField1"];
			entity.CustomField2 = (Convert.IsDBNull(dataRow["CustomField2"]))?string.Empty:(System.String)dataRow["CustomField2"];
			entity.CustomField3 = (Convert.IsDBNull(dataRow["CustomField3"]))?string.Empty:(System.String)dataRow["CustomField3"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAViewProcessListingFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewProcessListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewProcessListingFilterBuilder : SqlFilterBuilder<METAViewProcessListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewProcessListingFilterBuilder class.
		/// </summary>
		public METAViewProcessListingFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewProcessListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewProcessListingFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewProcessListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewProcessListingFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewProcessListingFilterBuilder

	#region METAViewProcessListingParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewProcessListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewProcessListingParameterBuilder : ParameterizedSqlFilterBuilder<METAViewProcessListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewProcessListingParameterBuilder class.
		/// </summary>
		public METAViewProcessListingParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewProcessListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewProcessListingParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewProcessListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewProcessListingParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewProcessListingParameterBuilder
	
	#region METAViewProcessListingSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewProcessListing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAViewProcessListingSortBuilder : SqlSortBuilder<METAViewProcessListingColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewProcessListingSqlSortBuilder class.
		/// </summary>
		public METAViewProcessListingSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAViewProcessListingSortBuilder

} // end namespace
