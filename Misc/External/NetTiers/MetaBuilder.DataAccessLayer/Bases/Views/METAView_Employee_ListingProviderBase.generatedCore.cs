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
	/// This class is the base class for any <see cref="METAView_Employee_ListingProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAView_Employee_ListingProviderBaseCore : EntityViewProviderBase<METAView_Employee_Listing>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAView_Employee_Listing&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAView_Employee_Listing&gt;"/></returns>
		protected static VList&lt;METAView_Employee_Listing&gt; Fill(DataSet dataSet, VList<METAView_Employee_Listing> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAView_Employee_Listing>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAView_Employee_Listing&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAView_Employee_Listing>"/></returns>
		protected static VList&lt;METAView_Employee_Listing&gt; Fill(DataTable dataTable, VList<METAView_Employee_Listing> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAView_Employee_Listing c = new METAView_Employee_Listing();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeId = (Convert.IsDBNull(row["WorkspaceTypeId"]))?(int)0:(System.Int32?)row["WorkspaceTypeId"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.Name = (Convert.IsDBNull(row["Name"]))?string.Empty:(System.String)row["Name"];
					c.Surname = (Convert.IsDBNull(row["Surname"]))?string.Empty:(System.String)row["Surname"];
					c.Title = (Convert.IsDBNull(row["Title"]))?string.Empty:(System.String)row["Title"];
					c.EmployeeNumber = (Convert.IsDBNull(row["EmployeeNumber"]))?string.Empty:(System.String)row["EmployeeNumber"];
					c.IDNumber = (Convert.IsDBNull(row["IDNumber"]))?string.Empty:(System.String)row["IDNumber"];
					c.Email = (Convert.IsDBNull(row["Email"]))?string.Empty:(System.String)row["Email"];
					c.Telephone = (Convert.IsDBNull(row["Telephone"]))?string.Empty:(System.String)row["Telephone"];
					c.GapType = (Convert.IsDBNull(row["GapType"]))?string.Empty:(System.String)row["GapType"];
					c.Description = (Convert.IsDBNull(row["Description"]))?string.Empty:(System.String)row["Description"];
					c.Abbreviation = (Convert.IsDBNull(row["Abbreviation"]))?string.Empty:(System.String)row["Abbreviation"];
					c.IsBusinessExternal = (Convert.IsDBNull(row["IsBusinessExternal"]))?string.Empty:(System.String)row["IsBusinessExternal"];
					c.DataSourceID = (Convert.IsDBNull(row["DataSourceID"]))?string.Empty:(System.String)row["DataSourceID"];
					c.DataSourceName = (Convert.IsDBNull(row["DataSourceName"]))?string.Empty:(System.String)row["DataSourceName"];
					c.GeneralRemarks = (Convert.IsDBNull(row["GeneralRemarks"]))?string.Empty:(System.String)row["GeneralRemarks"];
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
		/// Fill an <see cref="VList&lt;METAView_Employee_Listing&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAView_Employee_Listing&gt;"/></returns>
		protected VList<METAView_Employee_Listing> Fill(IDataReader reader, VList<METAView_Employee_Listing> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAView_Employee_Listing entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAView_Employee_Listing>("METAView_Employee_Listing",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAView_Employee_Listing();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAView_Employee_ListingColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_Employee_ListingColumn.WorkspaceTypeId)];
					//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
					entity.VCStatusID = (System.Int32)reader[((int)METAView_Employee_ListingColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.pkid = (System.Int32)reader[((int)METAView_Employee_ListingColumn.pkid)];
					//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAView_Employee_ListingColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Name = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.Name)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.Name)];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.Surname = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.Surname)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.Surname)];
					//entity.Surname = (Convert.IsDBNull(reader["Surname"]))?string.Empty:(System.String)reader["Surname"];
					entity.Title = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.Title)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.Title)];
					//entity.Title = (Convert.IsDBNull(reader["Title"]))?string.Empty:(System.String)reader["Title"];
					entity.EmployeeNumber = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.EmployeeNumber)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.EmployeeNumber)];
					//entity.EmployeeNumber = (Convert.IsDBNull(reader["EmployeeNumber"]))?string.Empty:(System.String)reader["EmployeeNumber"];
					entity.IDNumber = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.IDNumber)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.IDNumber)];
					//entity.IDNumber = (Convert.IsDBNull(reader["IDNumber"]))?string.Empty:(System.String)reader["IDNumber"];
					entity.Email = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.Email)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.Email)];
					//entity.Email = (Convert.IsDBNull(reader["Email"]))?string.Empty:(System.String)reader["Email"];
					entity.Telephone = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.Telephone)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.Telephone)];
					//entity.Telephone = (Convert.IsDBNull(reader["Telephone"]))?string.Empty:(System.String)reader["Telephone"];
					entity.GapType = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.GapType)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.GapType)];
					//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
					entity.Description = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.Description)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.Description)];
					//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
					entity.Abbreviation = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.Abbreviation)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.Abbreviation)];
					//entity.Abbreviation = (Convert.IsDBNull(reader["Abbreviation"]))?string.Empty:(System.String)reader["Abbreviation"];
					entity.IsBusinessExternal = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.IsBusinessExternal)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.IsBusinessExternal)];
					//entity.IsBusinessExternal = (Convert.IsDBNull(reader["IsBusinessExternal"]))?string.Empty:(System.String)reader["IsBusinessExternal"];
					entity.DataSourceID = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.DataSourceID)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.DataSourceID)];
					//entity.DataSourceID = (Convert.IsDBNull(reader["DataSourceID"]))?string.Empty:(System.String)reader["DataSourceID"];
					entity.DataSourceName = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.DataSourceName)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.DataSourceName)];
					//entity.DataSourceName = (Convert.IsDBNull(reader["DataSourceName"]))?string.Empty:(System.String)reader["DataSourceName"];
					entity.GeneralRemarks = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.GeneralRemarks)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.GeneralRemarks)];
					//entity.GeneralRemarks = (Convert.IsDBNull(reader["GeneralRemarks"]))?string.Empty:(System.String)reader["GeneralRemarks"];
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
		/// Refreshes the <see cref="METAView_Employee_Listing"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_Employee_Listing"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAView_Employee_Listing entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAView_Employee_ListingColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_Employee_ListingColumn.WorkspaceTypeId)];
			//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
			entity.VCStatusID = (System.Int32)reader[((int)METAView_Employee_ListingColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.pkid = (System.Int32)reader[((int)METAView_Employee_ListingColumn.pkid)];
			//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAView_Employee_ListingColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Name = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.Name)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.Name)];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.Surname = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.Surname)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.Surname)];
			//entity.Surname = (Convert.IsDBNull(reader["Surname"]))?string.Empty:(System.String)reader["Surname"];
			entity.Title = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.Title)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.Title)];
			//entity.Title = (Convert.IsDBNull(reader["Title"]))?string.Empty:(System.String)reader["Title"];
			entity.EmployeeNumber = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.EmployeeNumber)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.EmployeeNumber)];
			//entity.EmployeeNumber = (Convert.IsDBNull(reader["EmployeeNumber"]))?string.Empty:(System.String)reader["EmployeeNumber"];
			entity.IDNumber = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.IDNumber)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.IDNumber)];
			//entity.IDNumber = (Convert.IsDBNull(reader["IDNumber"]))?string.Empty:(System.String)reader["IDNumber"];
			entity.Email = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.Email)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.Email)];
			//entity.Email = (Convert.IsDBNull(reader["Email"]))?string.Empty:(System.String)reader["Email"];
			entity.Telephone = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.Telephone)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.Telephone)];
			//entity.Telephone = (Convert.IsDBNull(reader["Telephone"]))?string.Empty:(System.String)reader["Telephone"];
			entity.GapType = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.GapType)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			entity.Description = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.Description)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.Description)];
			//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
			entity.Abbreviation = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.Abbreviation)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.Abbreviation)];
			//entity.Abbreviation = (Convert.IsDBNull(reader["Abbreviation"]))?string.Empty:(System.String)reader["Abbreviation"];
			entity.IsBusinessExternal = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.IsBusinessExternal)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.IsBusinessExternal)];
			//entity.IsBusinessExternal = (Convert.IsDBNull(reader["IsBusinessExternal"]))?string.Empty:(System.String)reader["IsBusinessExternal"];
			entity.DataSourceID = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.DataSourceID)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.DataSourceID)];
			//entity.DataSourceID = (Convert.IsDBNull(reader["DataSourceID"]))?string.Empty:(System.String)reader["DataSourceID"];
			entity.DataSourceName = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.DataSourceName)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.DataSourceName)];
			//entity.DataSourceName = (Convert.IsDBNull(reader["DataSourceName"]))?string.Empty:(System.String)reader["DataSourceName"];
			entity.GeneralRemarks = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.GeneralRemarks)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.GeneralRemarks)];
			//entity.GeneralRemarks = (Convert.IsDBNull(reader["GeneralRemarks"]))?string.Empty:(System.String)reader["GeneralRemarks"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAView_Employee_Listing"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_Employee_Listing"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAView_Employee_Listing entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeId = (Convert.IsDBNull(dataRow["WorkspaceTypeId"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeId"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.Name = (Convert.IsDBNull(dataRow["Name"]))?string.Empty:(System.String)dataRow["Name"];
			entity.Surname = (Convert.IsDBNull(dataRow["Surname"]))?string.Empty:(System.String)dataRow["Surname"];
			entity.Title = (Convert.IsDBNull(dataRow["Title"]))?string.Empty:(System.String)dataRow["Title"];
			entity.EmployeeNumber = (Convert.IsDBNull(dataRow["EmployeeNumber"]))?string.Empty:(System.String)dataRow["EmployeeNumber"];
			entity.IDNumber = (Convert.IsDBNull(dataRow["IDNumber"]))?string.Empty:(System.String)dataRow["IDNumber"];
			entity.Email = (Convert.IsDBNull(dataRow["Email"]))?string.Empty:(System.String)dataRow["Email"];
			entity.Telephone = (Convert.IsDBNull(dataRow["Telephone"]))?string.Empty:(System.String)dataRow["Telephone"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.Description = (Convert.IsDBNull(dataRow["Description"]))?string.Empty:(System.String)dataRow["Description"];
			entity.Abbreviation = (Convert.IsDBNull(dataRow["Abbreviation"]))?string.Empty:(System.String)dataRow["Abbreviation"];
			entity.IsBusinessExternal = (Convert.IsDBNull(dataRow["IsBusinessExternal"]))?string.Empty:(System.String)dataRow["IsBusinessExternal"];
			entity.DataSourceID = (Convert.IsDBNull(dataRow["DataSourceID"]))?string.Empty:(System.String)dataRow["DataSourceID"];
			entity.DataSourceName = (Convert.IsDBNull(dataRow["DataSourceName"]))?string.Empty:(System.String)dataRow["DataSourceName"];
			entity.GeneralRemarks = (Convert.IsDBNull(dataRow["GeneralRemarks"]))?string.Empty:(System.String)dataRow["GeneralRemarks"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAView_Employee_ListingFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_Employee_Listing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_Employee_ListingFilterBuilder : SqlFilterBuilder<METAView_Employee_ListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_Employee_ListingFilterBuilder class.
		/// </summary>
		public METAView_Employee_ListingFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_Employee_ListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_Employee_ListingFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_Employee_ListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_Employee_ListingFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_Employee_ListingFilterBuilder

	#region METAView_Employee_ListingParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_Employee_Listing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_Employee_ListingParameterBuilder : ParameterizedSqlFilterBuilder<METAView_Employee_ListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_Employee_ListingParameterBuilder class.
		/// </summary>
		public METAView_Employee_ListingParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_Employee_ListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_Employee_ListingParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_Employee_ListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_Employee_ListingParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_Employee_ListingParameterBuilder
	
	#region METAView_Employee_ListingSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_Employee_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_Employee_ListingSortBuilder : SqlSortBuilder<METAView_Employee_ListingColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_Employee_ListingSqlSortBuilder class.
		/// </summary>
		public METAView_Employee_ListingSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAView_Employee_ListingSortBuilder

} // end namespace
