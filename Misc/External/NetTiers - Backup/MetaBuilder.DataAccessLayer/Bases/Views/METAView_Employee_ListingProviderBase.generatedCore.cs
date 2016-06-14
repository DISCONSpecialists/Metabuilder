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
					c.CustomField1 = (Convert.IsDBNull(row["CustomField1"]))?string.Empty:(System.String)row["CustomField1"];
					c.CustomField2 = (Convert.IsDBNull(row["CustomField2"]))?string.Empty:(System.String)row["CustomField2"];
					c.CustomField3 = (Convert.IsDBNull(row["CustomField3"]))?string.Empty:(System.String)row["CustomField3"];
					c.Mobile = (Convert.IsDBNull(row["Mobile"]))?string.Empty:(System.String)row["Mobile"];
					c.Fax = (Convert.IsDBNull(row["Fax"]))?string.Empty:(System.String)row["Fax"];
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
					entity.CustomField1 = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.CustomField1)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.CustomField2)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.CustomField3)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.Mobile = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.Mobile)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.Mobile)];
					//entity.Mobile = (Convert.IsDBNull(reader["Mobile"]))?string.Empty:(System.String)reader["Mobile"];
					entity.Fax = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.Fax)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.Fax)];
					//entity.Fax = (Convert.IsDBNull(reader["Fax"]))?string.Empty:(System.String)reader["Fax"];
					entity.GapType = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.GapType)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.GapType)];
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
			entity.CustomField1 = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.CustomField1)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.CustomField2)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.CustomField3)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.Mobile = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.Mobile)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.Mobile)];
			//entity.Mobile = (Convert.IsDBNull(reader["Mobile"]))?string.Empty:(System.String)reader["Mobile"];
			entity.Fax = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.Fax)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.Fax)];
			//entity.Fax = (Convert.IsDBNull(reader["Fax"]))?string.Empty:(System.String)reader["Fax"];
			entity.GapType = (reader.IsDBNull(((int)METAView_Employee_ListingColumn.GapType)))?null:(System.String)reader[((int)METAView_Employee_ListingColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
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
			entity.CustomField1 = (Convert.IsDBNull(dataRow["CustomField1"]))?string.Empty:(System.String)dataRow["CustomField1"];
			entity.CustomField2 = (Convert.IsDBNull(dataRow["CustomField2"]))?string.Empty:(System.String)dataRow["CustomField2"];
			entity.CustomField3 = (Convert.IsDBNull(dataRow["CustomField3"]))?string.Empty:(System.String)dataRow["CustomField3"];
			entity.Mobile = (Convert.IsDBNull(dataRow["Mobile"]))?string.Empty:(System.String)dataRow["Mobile"];
			entity.Fax = (Convert.IsDBNull(dataRow["Fax"]))?string.Empty:(System.String)dataRow["Fax"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
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