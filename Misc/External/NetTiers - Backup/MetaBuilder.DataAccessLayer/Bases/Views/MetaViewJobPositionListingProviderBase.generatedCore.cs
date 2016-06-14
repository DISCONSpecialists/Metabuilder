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
	/// This class is the base class for any <see cref="METAViewJobPositionListingProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAViewJobPositionListingProviderBaseCore : EntityViewProviderBase<METAViewJobPositionListing>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAViewJobPositionListing&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAViewJobPositionListing&gt;"/></returns>
		protected static VList&lt;METAViewJobPositionListing&gt; Fill(DataSet dataSet, VList<METAViewJobPositionListing> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAViewJobPositionListing>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAViewJobPositionListing&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAViewJobPositionListing>"/></returns>
		protected static VList&lt;METAViewJobPositionListing&gt; Fill(DataTable dataTable, VList<METAViewJobPositionListing> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAViewJobPositionListing c = new METAViewJobPositionListing();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeID = (Convert.IsDBNull(row["WorkspaceTypeID"]))?(int)0:(System.Int32?)row["WorkspaceTypeID"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.Pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.Name = (Convert.IsDBNull(row["Name"]))?string.Empty:(System.String)row["Name"];
					c.TotalRequired = (Convert.IsDBNull(row["TotalRequired"]))?(int)0:(System.Int32?)row["TotalRequired"];
					c.TotalOccupied = (Convert.IsDBNull(row["TotalOccupied"]))?(int)0:(System.Int32?)row["TotalOccupied"];
					c.TotalAvailable = (Convert.IsDBNull(row["TotalAvailable"]))?(int)0:(System.Int32?)row["TotalAvailable"];
					c.TimeStamp = (Convert.IsDBNull(row["TimeStamp"]))?DateTime.MinValue:(System.DateTime?)row["TimeStamp"];
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
		/// Fill an <see cref="VList&lt;METAViewJobPositionListing&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAViewJobPositionListing&gt;"/></returns>
		protected VList<METAViewJobPositionListing> Fill(IDataReader reader, VList<METAViewJobPositionListing> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAViewJobPositionListing entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAViewJobPositionListing>("METAViewJobPositionListing",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAViewJobPositionListing();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAViewJobPositionListingColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewJobPositionListingColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewJobPositionListingColumn.WorkspaceTypeID)];
					//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
					entity.VCStatusID = (System.Int32)reader[((int)METAViewJobPositionListingColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.Pkid = (System.Int32)reader[((int)METAViewJobPositionListingColumn.Pkid)];
					//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAViewJobPositionListingColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAViewJobPositionListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewJobPositionListingColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Name = (reader.IsDBNull(((int)METAViewJobPositionListingColumn.Name)))?null:(System.String)reader[((int)METAViewJobPositionListingColumn.Name)];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.TotalRequired = (reader.IsDBNull(((int)METAViewJobPositionListingColumn.TotalRequired)))?null:(System.Int32?)reader[((int)METAViewJobPositionListingColumn.TotalRequired)];
					//entity.TotalRequired = (Convert.IsDBNull(reader["TotalRequired"]))?(int)0:(System.Int32?)reader["TotalRequired"];
					entity.TotalOccupied = (reader.IsDBNull(((int)METAViewJobPositionListingColumn.TotalOccupied)))?null:(System.Int32?)reader[((int)METAViewJobPositionListingColumn.TotalOccupied)];
					//entity.TotalOccupied = (Convert.IsDBNull(reader["TotalOccupied"]))?(int)0:(System.Int32?)reader["TotalOccupied"];
					entity.TotalAvailable = (reader.IsDBNull(((int)METAViewJobPositionListingColumn.TotalAvailable)))?null:(System.Int32?)reader[((int)METAViewJobPositionListingColumn.TotalAvailable)];
					//entity.TotalAvailable = (Convert.IsDBNull(reader["TotalAvailable"]))?(int)0:(System.Int32?)reader["TotalAvailable"];
					entity.TimeStamp = (reader.IsDBNull(((int)METAViewJobPositionListingColumn.TimeStamp)))?null:(System.DateTime?)reader[((int)METAViewJobPositionListingColumn.TimeStamp)];
					//entity.TimeStamp = (Convert.IsDBNull(reader["TimeStamp"]))?DateTime.MinValue:(System.DateTime?)reader["TimeStamp"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAViewJobPositionListingColumn.CustomField1)))?null:(System.String)reader[((int)METAViewJobPositionListingColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAViewJobPositionListingColumn.CustomField2)))?null:(System.String)reader[((int)METAViewJobPositionListingColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAViewJobPositionListingColumn.CustomField3)))?null:(System.String)reader[((int)METAViewJobPositionListingColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.GapType = (reader.IsDBNull(((int)METAViewJobPositionListingColumn.GapType)))?null:(System.String)reader[((int)METAViewJobPositionListingColumn.GapType)];
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
		/// Refreshes the <see cref="METAViewJobPositionListing"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewJobPositionListing"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAViewJobPositionListing entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAViewJobPositionListingColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewJobPositionListingColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewJobPositionListingColumn.WorkspaceTypeID)];
			//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
			entity.VCStatusID = (System.Int32)reader[((int)METAViewJobPositionListingColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.Pkid = (System.Int32)reader[((int)METAViewJobPositionListingColumn.Pkid)];
			//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAViewJobPositionListingColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAViewJobPositionListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewJobPositionListingColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Name = (reader.IsDBNull(((int)METAViewJobPositionListingColumn.Name)))?null:(System.String)reader[((int)METAViewJobPositionListingColumn.Name)];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.TotalRequired = (reader.IsDBNull(((int)METAViewJobPositionListingColumn.TotalRequired)))?null:(System.Int32?)reader[((int)METAViewJobPositionListingColumn.TotalRequired)];
			//entity.TotalRequired = (Convert.IsDBNull(reader["TotalRequired"]))?(int)0:(System.Int32?)reader["TotalRequired"];
			entity.TotalOccupied = (reader.IsDBNull(((int)METAViewJobPositionListingColumn.TotalOccupied)))?null:(System.Int32?)reader[((int)METAViewJobPositionListingColumn.TotalOccupied)];
			//entity.TotalOccupied = (Convert.IsDBNull(reader["TotalOccupied"]))?(int)0:(System.Int32?)reader["TotalOccupied"];
			entity.TotalAvailable = (reader.IsDBNull(((int)METAViewJobPositionListingColumn.TotalAvailable)))?null:(System.Int32?)reader[((int)METAViewJobPositionListingColumn.TotalAvailable)];
			//entity.TotalAvailable = (Convert.IsDBNull(reader["TotalAvailable"]))?(int)0:(System.Int32?)reader["TotalAvailable"];
			entity.TimeStamp = (reader.IsDBNull(((int)METAViewJobPositionListingColumn.TimeStamp)))?null:(System.DateTime?)reader[((int)METAViewJobPositionListingColumn.TimeStamp)];
			//entity.TimeStamp = (Convert.IsDBNull(reader["TimeStamp"]))?DateTime.MinValue:(System.DateTime?)reader["TimeStamp"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAViewJobPositionListingColumn.CustomField1)))?null:(System.String)reader[((int)METAViewJobPositionListingColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAViewJobPositionListingColumn.CustomField2)))?null:(System.String)reader[((int)METAViewJobPositionListingColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAViewJobPositionListingColumn.CustomField3)))?null:(System.String)reader[((int)METAViewJobPositionListingColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.GapType = (reader.IsDBNull(((int)METAViewJobPositionListingColumn.GapType)))?null:(System.String)reader[((int)METAViewJobPositionListingColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAViewJobPositionListing"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewJobPositionListing"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAViewJobPositionListing entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeID = (Convert.IsDBNull(dataRow["WorkspaceTypeID"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeID"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.Pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.Name = (Convert.IsDBNull(dataRow["Name"]))?string.Empty:(System.String)dataRow["Name"];
			entity.TotalRequired = (Convert.IsDBNull(dataRow["TotalRequired"]))?(int)0:(System.Int32?)dataRow["TotalRequired"];
			entity.TotalOccupied = (Convert.IsDBNull(dataRow["TotalOccupied"]))?(int)0:(System.Int32?)dataRow["TotalOccupied"];
			entity.TotalAvailable = (Convert.IsDBNull(dataRow["TotalAvailable"]))?(int)0:(System.Int32?)dataRow["TotalAvailable"];
			entity.TimeStamp = (Convert.IsDBNull(dataRow["TimeStamp"]))?DateTime.MinValue:(System.DateTime?)dataRow["TimeStamp"];
			entity.CustomField1 = (Convert.IsDBNull(dataRow["CustomField1"]))?string.Empty:(System.String)dataRow["CustomField1"];
			entity.CustomField2 = (Convert.IsDBNull(dataRow["CustomField2"]))?string.Empty:(System.String)dataRow["CustomField2"];
			entity.CustomField3 = (Convert.IsDBNull(dataRow["CustomField3"]))?string.Empty:(System.String)dataRow["CustomField3"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAViewJobPositionListingFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewJobPositionListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewJobPositionListingFilterBuilder : SqlFilterBuilder<METAViewJobPositionListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewJobPositionListingFilterBuilder class.
		/// </summary>
		public METAViewJobPositionListingFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewJobPositionListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewJobPositionListingFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewJobPositionListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewJobPositionListingFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewJobPositionListingFilterBuilder

	#region METAViewJobPositionListingParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewJobPositionListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewJobPositionListingParameterBuilder : ParameterizedSqlFilterBuilder<METAViewJobPositionListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewJobPositionListingParameterBuilder class.
		/// </summary>
		public METAViewJobPositionListingParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewJobPositionListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewJobPositionListingParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewJobPositionListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewJobPositionListingParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewJobPositionListingParameterBuilder
	
	#region METAViewJobPositionListingSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewJobPositionListing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAViewJobPositionListingSortBuilder : SqlSortBuilder<METAViewJobPositionListingColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewJobPositionListingSqlSortBuilder class.
		/// </summary>
		public METAViewJobPositionListingSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAViewJobPositionListingSortBuilder

} // end namespace
