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
	/// This class is the base class for any <see cref="METAViewModelRealListingProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAViewModelRealListingProviderBaseCore : EntityViewProviderBase<METAViewModelRealListing>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAViewModelRealListing&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAViewModelRealListing&gt;"/></returns>
		protected static VList&lt;METAViewModelRealListing&gt; Fill(DataSet dataSet, VList<METAViewModelRealListing> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAViewModelRealListing>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAViewModelRealListing&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAViewModelRealListing>"/></returns>
		protected static VList&lt;METAViewModelRealListing&gt; Fill(DataTable dataTable, VList<METAViewModelRealListing> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAViewModelRealListing c = new METAViewModelRealListing();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeID = (Convert.IsDBNull(row["WorkspaceTypeID"]))?(int)0:(System.Int32?)row["WorkspaceTypeID"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.Pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
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
		/// Fill an <see cref="VList&lt;METAViewModelRealListing&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAViewModelRealListing&gt;"/></returns>
		protected VList<METAViewModelRealListing> Fill(IDataReader reader, VList<METAViewModelRealListing> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAViewModelRealListing entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAViewModelRealListing>("METAViewModelRealListing",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAViewModelRealListing();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAViewModelRealListingColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewModelRealListingColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewModelRealListingColumn.WorkspaceTypeID)];
					//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
					entity.VCStatusID = (System.Int32)reader[((int)METAViewModelRealListingColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.Pkid = (System.Int32)reader[((int)METAViewModelRealListingColumn.Pkid)];
					//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAViewModelRealListingColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAViewModelRealListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewModelRealListingColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
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
		/// Refreshes the <see cref="METAViewModelRealListing"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewModelRealListing"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAViewModelRealListing entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAViewModelRealListingColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewModelRealListingColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewModelRealListingColumn.WorkspaceTypeID)];
			//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
			entity.VCStatusID = (System.Int32)reader[((int)METAViewModelRealListingColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.Pkid = (System.Int32)reader[((int)METAViewModelRealListingColumn.Pkid)];
			//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAViewModelRealListingColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAViewModelRealListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewModelRealListingColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAViewModelRealListing"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewModelRealListing"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAViewModelRealListing entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeID = (Convert.IsDBNull(dataRow["WorkspaceTypeID"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeID"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.Pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAViewModelRealListingFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewModelRealListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewModelRealListingFilterBuilder : SqlFilterBuilder<METAViewModelRealListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewModelRealListingFilterBuilder class.
		/// </summary>
		public METAViewModelRealListingFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewModelRealListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewModelRealListingFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewModelRealListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewModelRealListingFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewModelRealListingFilterBuilder

	#region METAViewModelRealListingParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewModelRealListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewModelRealListingParameterBuilder : ParameterizedSqlFilterBuilder<METAViewModelRealListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewModelRealListingParameterBuilder class.
		/// </summary>
		public METAViewModelRealListingParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewModelRealListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewModelRealListingParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewModelRealListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewModelRealListingParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewModelRealListingParameterBuilder
	
	#region METAViewModelRealListingSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewModelRealListing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAViewModelRealListingSortBuilder : SqlSortBuilder<METAViewModelRealListingColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewModelRealListingSqlSortBuilder class.
		/// </summary>
		public METAViewModelRealListingSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAViewModelRealListingSortBuilder

} // end namespace
