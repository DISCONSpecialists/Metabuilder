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
	/// This class is the base class for any <see cref="METAViewMutuallyExclusiveIndicatorListingProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAViewMutuallyExclusiveIndicatorListingProviderBaseCore : EntityViewProviderBase<METAViewMutuallyExclusiveIndicatorListing>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAViewMutuallyExclusiveIndicatorListing&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAViewMutuallyExclusiveIndicatorListing&gt;"/></returns>
		protected static VList&lt;METAViewMutuallyExclusiveIndicatorListing&gt; Fill(DataSet dataSet, VList<METAViewMutuallyExclusiveIndicatorListing> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAViewMutuallyExclusiveIndicatorListing>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAViewMutuallyExclusiveIndicatorListing&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAViewMutuallyExclusiveIndicatorListing>"/></returns>
		protected static VList&lt;METAViewMutuallyExclusiveIndicatorListing&gt; Fill(DataTable dataTable, VList<METAViewMutuallyExclusiveIndicatorListing> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAViewMutuallyExclusiveIndicatorListing c = new METAViewMutuallyExclusiveIndicatorListing();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeID = (Convert.IsDBNull(row["WorkspaceTypeID"]))?(int)0:(System.Int32?)row["WorkspaceTypeID"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.Pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.Name = (Convert.IsDBNull(row["Name"]))?string.Empty:(System.String)row["Name"];
					c.CustomField1 = (Convert.IsDBNull(row["CustomField1"]))?string.Empty:(System.String)row["CustomField1"];
					c.CustomField2 = (Convert.IsDBNull(row["CustomField2"]))?string.Empty:(System.String)row["CustomField2"];
					c.CustomField3 = (Convert.IsDBNull(row["CustomField3"]))?string.Empty:(System.String)row["CustomField3"];
					c.SelectorType = (Convert.IsDBNull(row["SelectorType"]))?string.Empty:(System.String)row["SelectorType"];
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
		/// Fill an <see cref="VList&lt;METAViewMutuallyExclusiveIndicatorListing&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAViewMutuallyExclusiveIndicatorListing&gt;"/></returns>
		protected VList<METAViewMutuallyExclusiveIndicatorListing> Fill(IDataReader reader, VList<METAViewMutuallyExclusiveIndicatorListing> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAViewMutuallyExclusiveIndicatorListing entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAViewMutuallyExclusiveIndicatorListing>("METAViewMutuallyExclusiveIndicatorListing",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAViewMutuallyExclusiveIndicatorListing();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAViewMutuallyExclusiveIndicatorListingColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewMutuallyExclusiveIndicatorListingColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewMutuallyExclusiveIndicatorListingColumn.WorkspaceTypeID)];
					//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
					entity.VCStatusID = (System.Int32)reader[((int)METAViewMutuallyExclusiveIndicatorListingColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.Pkid = (System.Int32)reader[((int)METAViewMutuallyExclusiveIndicatorListingColumn.Pkid)];
					//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAViewMutuallyExclusiveIndicatorListingColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAViewMutuallyExclusiveIndicatorListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewMutuallyExclusiveIndicatorListingColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Name = (reader.IsDBNull(((int)METAViewMutuallyExclusiveIndicatorListingColumn.Name)))?null:(System.String)reader[((int)METAViewMutuallyExclusiveIndicatorListingColumn.Name)];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAViewMutuallyExclusiveIndicatorListingColumn.CustomField1)))?null:(System.String)reader[((int)METAViewMutuallyExclusiveIndicatorListingColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAViewMutuallyExclusiveIndicatorListingColumn.CustomField2)))?null:(System.String)reader[((int)METAViewMutuallyExclusiveIndicatorListingColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAViewMutuallyExclusiveIndicatorListingColumn.CustomField3)))?null:(System.String)reader[((int)METAViewMutuallyExclusiveIndicatorListingColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.SelectorType = (reader.IsDBNull(((int)METAViewMutuallyExclusiveIndicatorListingColumn.SelectorType)))?null:(System.String)reader[((int)METAViewMutuallyExclusiveIndicatorListingColumn.SelectorType)];
					//entity.SelectorType = (Convert.IsDBNull(reader["SelectorType"]))?string.Empty:(System.String)reader["SelectorType"];
					entity.GapType = (reader.IsDBNull(((int)METAViewMutuallyExclusiveIndicatorListingColumn.GapType)))?null:(System.String)reader[((int)METAViewMutuallyExclusiveIndicatorListingColumn.GapType)];
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
		/// Refreshes the <see cref="METAViewMutuallyExclusiveIndicatorListing"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewMutuallyExclusiveIndicatorListing"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAViewMutuallyExclusiveIndicatorListing entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAViewMutuallyExclusiveIndicatorListingColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewMutuallyExclusiveIndicatorListingColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewMutuallyExclusiveIndicatorListingColumn.WorkspaceTypeID)];
			//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
			entity.VCStatusID = (System.Int32)reader[((int)METAViewMutuallyExclusiveIndicatorListingColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.Pkid = (System.Int32)reader[((int)METAViewMutuallyExclusiveIndicatorListingColumn.Pkid)];
			//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAViewMutuallyExclusiveIndicatorListingColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAViewMutuallyExclusiveIndicatorListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewMutuallyExclusiveIndicatorListingColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Name = (reader.IsDBNull(((int)METAViewMutuallyExclusiveIndicatorListingColumn.Name)))?null:(System.String)reader[((int)METAViewMutuallyExclusiveIndicatorListingColumn.Name)];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAViewMutuallyExclusiveIndicatorListingColumn.CustomField1)))?null:(System.String)reader[((int)METAViewMutuallyExclusiveIndicatorListingColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAViewMutuallyExclusiveIndicatorListingColumn.CustomField2)))?null:(System.String)reader[((int)METAViewMutuallyExclusiveIndicatorListingColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAViewMutuallyExclusiveIndicatorListingColumn.CustomField3)))?null:(System.String)reader[((int)METAViewMutuallyExclusiveIndicatorListingColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.SelectorType = (reader.IsDBNull(((int)METAViewMutuallyExclusiveIndicatorListingColumn.SelectorType)))?null:(System.String)reader[((int)METAViewMutuallyExclusiveIndicatorListingColumn.SelectorType)];
			//entity.SelectorType = (Convert.IsDBNull(reader["SelectorType"]))?string.Empty:(System.String)reader["SelectorType"];
			entity.GapType = (reader.IsDBNull(((int)METAViewMutuallyExclusiveIndicatorListingColumn.GapType)))?null:(System.String)reader[((int)METAViewMutuallyExclusiveIndicatorListingColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAViewMutuallyExclusiveIndicatorListing"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewMutuallyExclusiveIndicatorListing"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAViewMutuallyExclusiveIndicatorListing entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeID = (Convert.IsDBNull(dataRow["WorkspaceTypeID"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeID"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.Pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.Name = (Convert.IsDBNull(dataRow["Name"]))?string.Empty:(System.String)dataRow["Name"];
			entity.CustomField1 = (Convert.IsDBNull(dataRow["CustomField1"]))?string.Empty:(System.String)dataRow["CustomField1"];
			entity.CustomField2 = (Convert.IsDBNull(dataRow["CustomField2"]))?string.Empty:(System.String)dataRow["CustomField2"];
			entity.CustomField3 = (Convert.IsDBNull(dataRow["CustomField3"]))?string.Empty:(System.String)dataRow["CustomField3"];
			entity.SelectorType = (Convert.IsDBNull(dataRow["SelectorType"]))?string.Empty:(System.String)dataRow["SelectorType"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAViewMutuallyExclusiveIndicatorListingFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewMutuallyExclusiveIndicatorListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewMutuallyExclusiveIndicatorListingFilterBuilder : SqlFilterBuilder<METAViewMutuallyExclusiveIndicatorListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewMutuallyExclusiveIndicatorListingFilterBuilder class.
		/// </summary>
		public METAViewMutuallyExclusiveIndicatorListingFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewMutuallyExclusiveIndicatorListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewMutuallyExclusiveIndicatorListingFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewMutuallyExclusiveIndicatorListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewMutuallyExclusiveIndicatorListingFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewMutuallyExclusiveIndicatorListingFilterBuilder

	#region METAViewMutuallyExclusiveIndicatorListingParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewMutuallyExclusiveIndicatorListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewMutuallyExclusiveIndicatorListingParameterBuilder : ParameterizedSqlFilterBuilder<METAViewMutuallyExclusiveIndicatorListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewMutuallyExclusiveIndicatorListingParameterBuilder class.
		/// </summary>
		public METAViewMutuallyExclusiveIndicatorListingParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewMutuallyExclusiveIndicatorListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewMutuallyExclusiveIndicatorListingParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewMutuallyExclusiveIndicatorListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewMutuallyExclusiveIndicatorListingParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewMutuallyExclusiveIndicatorListingParameterBuilder
	
	#region METAViewMutuallyExclusiveIndicatorListingSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewMutuallyExclusiveIndicatorListing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAViewMutuallyExclusiveIndicatorListingSortBuilder : SqlSortBuilder<METAViewMutuallyExclusiveIndicatorListingColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewMutuallyExclusiveIndicatorListingSqlSortBuilder class.
		/// </summary>
		public METAViewMutuallyExclusiveIndicatorListingSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAViewMutuallyExclusiveIndicatorListingSortBuilder

} // end namespace
