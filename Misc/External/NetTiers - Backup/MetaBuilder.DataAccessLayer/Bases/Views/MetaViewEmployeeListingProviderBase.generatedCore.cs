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
	/// This class is the base class for any <see cref="METAViewEmployeeListingProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAViewEmployeeListingProviderBaseCore : EntityViewProviderBase<METAViewEmployeeListing>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAViewEmployeeListing&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAViewEmployeeListing&gt;"/></returns>
		protected static VList&lt;METAViewEmployeeListing&gt; Fill(DataSet dataSet, VList<METAViewEmployeeListing> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAViewEmployeeListing>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAViewEmployeeListing&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAViewEmployeeListing>"/></returns>
		protected static VList&lt;METAViewEmployeeListing&gt; Fill(DataTable dataTable, VList<METAViewEmployeeListing> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAViewEmployeeListing c = new METAViewEmployeeListing();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeID = (Convert.IsDBNull(row["WorkspaceTypeID"]))?(int)0:(System.Int32?)row["WorkspaceTypeID"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.Pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
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
		/// Fill an <see cref="VList&lt;METAViewEmployeeListing&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAViewEmployeeListing&gt;"/></returns>
		protected VList<METAViewEmployeeListing> Fill(IDataReader reader, VList<METAViewEmployeeListing> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAViewEmployeeListing entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAViewEmployeeListing>("METAViewEmployeeListing",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAViewEmployeeListing();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAViewEmployeeListingColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewEmployeeListingColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewEmployeeListingColumn.WorkspaceTypeID)];
					//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
					entity.VCStatusID = (System.Int32)reader[((int)METAViewEmployeeListingColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.Pkid = (System.Int32)reader[((int)METAViewEmployeeListingColumn.Pkid)];
					//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAViewEmployeeListingColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAViewEmployeeListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewEmployeeListingColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Name = (reader.IsDBNull(((int)METAViewEmployeeListingColumn.Name)))?null:(System.String)reader[((int)METAViewEmployeeListingColumn.Name)];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.Surname = (reader.IsDBNull(((int)METAViewEmployeeListingColumn.Surname)))?null:(System.String)reader[((int)METAViewEmployeeListingColumn.Surname)];
					//entity.Surname = (Convert.IsDBNull(reader["Surname"]))?string.Empty:(System.String)reader["Surname"];
					entity.Title = (reader.IsDBNull(((int)METAViewEmployeeListingColumn.Title)))?null:(System.String)reader[((int)METAViewEmployeeListingColumn.Title)];
					//entity.Title = (Convert.IsDBNull(reader["Title"]))?string.Empty:(System.String)reader["Title"];
					entity.EmployeeNumber = (reader.IsDBNull(((int)METAViewEmployeeListingColumn.EmployeeNumber)))?null:(System.String)reader[((int)METAViewEmployeeListingColumn.EmployeeNumber)];
					//entity.EmployeeNumber = (Convert.IsDBNull(reader["EmployeeNumber"]))?string.Empty:(System.String)reader["EmployeeNumber"];
					entity.IDNumber = (reader.IsDBNull(((int)METAViewEmployeeListingColumn.IDNumber)))?null:(System.String)reader[((int)METAViewEmployeeListingColumn.IDNumber)];
					//entity.IDNumber = (Convert.IsDBNull(reader["IDNumber"]))?string.Empty:(System.String)reader["IDNumber"];
					entity.Email = (reader.IsDBNull(((int)METAViewEmployeeListingColumn.Email)))?null:(System.String)reader[((int)METAViewEmployeeListingColumn.Email)];
					//entity.Email = (Convert.IsDBNull(reader["Email"]))?string.Empty:(System.String)reader["Email"];
					entity.Telephone = (reader.IsDBNull(((int)METAViewEmployeeListingColumn.Telephone)))?null:(System.String)reader[((int)METAViewEmployeeListingColumn.Telephone)];
					//entity.Telephone = (Convert.IsDBNull(reader["Telephone"]))?string.Empty:(System.String)reader["Telephone"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAViewEmployeeListingColumn.CustomField1)))?null:(System.String)reader[((int)METAViewEmployeeListingColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAViewEmployeeListingColumn.CustomField2)))?null:(System.String)reader[((int)METAViewEmployeeListingColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAViewEmployeeListingColumn.CustomField3)))?null:(System.String)reader[((int)METAViewEmployeeListingColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.Mobile = (reader.IsDBNull(((int)METAViewEmployeeListingColumn.Mobile)))?null:(System.String)reader[((int)METAViewEmployeeListingColumn.Mobile)];
					//entity.Mobile = (Convert.IsDBNull(reader["Mobile"]))?string.Empty:(System.String)reader["Mobile"];
					entity.Fax = (reader.IsDBNull(((int)METAViewEmployeeListingColumn.Fax)))?null:(System.String)reader[((int)METAViewEmployeeListingColumn.Fax)];
					//entity.Fax = (Convert.IsDBNull(reader["Fax"]))?string.Empty:(System.String)reader["Fax"];
					entity.GapType = (reader.IsDBNull(((int)METAViewEmployeeListingColumn.GapType)))?null:(System.String)reader[((int)METAViewEmployeeListingColumn.GapType)];
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
		/// Refreshes the <see cref="METAViewEmployeeListing"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewEmployeeListing"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAViewEmployeeListing entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAViewEmployeeListingColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewEmployeeListingColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewEmployeeListingColumn.WorkspaceTypeID)];
			//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
			entity.VCStatusID = (System.Int32)reader[((int)METAViewEmployeeListingColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.Pkid = (System.Int32)reader[((int)METAViewEmployeeListingColumn.Pkid)];
			//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAViewEmployeeListingColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAViewEmployeeListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewEmployeeListingColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Name = (reader.IsDBNull(((int)METAViewEmployeeListingColumn.Name)))?null:(System.String)reader[((int)METAViewEmployeeListingColumn.Name)];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.Surname = (reader.IsDBNull(((int)METAViewEmployeeListingColumn.Surname)))?null:(System.String)reader[((int)METAViewEmployeeListingColumn.Surname)];
			//entity.Surname = (Convert.IsDBNull(reader["Surname"]))?string.Empty:(System.String)reader["Surname"];
			entity.Title = (reader.IsDBNull(((int)METAViewEmployeeListingColumn.Title)))?null:(System.String)reader[((int)METAViewEmployeeListingColumn.Title)];
			//entity.Title = (Convert.IsDBNull(reader["Title"]))?string.Empty:(System.String)reader["Title"];
			entity.EmployeeNumber = (reader.IsDBNull(((int)METAViewEmployeeListingColumn.EmployeeNumber)))?null:(System.String)reader[((int)METAViewEmployeeListingColumn.EmployeeNumber)];
			//entity.EmployeeNumber = (Convert.IsDBNull(reader["EmployeeNumber"]))?string.Empty:(System.String)reader["EmployeeNumber"];
			entity.IDNumber = (reader.IsDBNull(((int)METAViewEmployeeListingColumn.IDNumber)))?null:(System.String)reader[((int)METAViewEmployeeListingColumn.IDNumber)];
			//entity.IDNumber = (Convert.IsDBNull(reader["IDNumber"]))?string.Empty:(System.String)reader["IDNumber"];
			entity.Email = (reader.IsDBNull(((int)METAViewEmployeeListingColumn.Email)))?null:(System.String)reader[((int)METAViewEmployeeListingColumn.Email)];
			//entity.Email = (Convert.IsDBNull(reader["Email"]))?string.Empty:(System.String)reader["Email"];
			entity.Telephone = (reader.IsDBNull(((int)METAViewEmployeeListingColumn.Telephone)))?null:(System.String)reader[((int)METAViewEmployeeListingColumn.Telephone)];
			//entity.Telephone = (Convert.IsDBNull(reader["Telephone"]))?string.Empty:(System.String)reader["Telephone"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAViewEmployeeListingColumn.CustomField1)))?null:(System.String)reader[((int)METAViewEmployeeListingColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAViewEmployeeListingColumn.CustomField2)))?null:(System.String)reader[((int)METAViewEmployeeListingColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAViewEmployeeListingColumn.CustomField3)))?null:(System.String)reader[((int)METAViewEmployeeListingColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.Mobile = (reader.IsDBNull(((int)METAViewEmployeeListingColumn.Mobile)))?null:(System.String)reader[((int)METAViewEmployeeListingColumn.Mobile)];
			//entity.Mobile = (Convert.IsDBNull(reader["Mobile"]))?string.Empty:(System.String)reader["Mobile"];
			entity.Fax = (reader.IsDBNull(((int)METAViewEmployeeListingColumn.Fax)))?null:(System.String)reader[((int)METAViewEmployeeListingColumn.Fax)];
			//entity.Fax = (Convert.IsDBNull(reader["Fax"]))?string.Empty:(System.String)reader["Fax"];
			entity.GapType = (reader.IsDBNull(((int)METAViewEmployeeListingColumn.GapType)))?null:(System.String)reader[((int)METAViewEmployeeListingColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAViewEmployeeListing"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewEmployeeListing"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAViewEmployeeListing entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeID = (Convert.IsDBNull(dataRow["WorkspaceTypeID"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeID"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.Pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
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

	#region METAViewEmployeeListingFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewEmployeeListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewEmployeeListingFilterBuilder : SqlFilterBuilder<METAViewEmployeeListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewEmployeeListingFilterBuilder class.
		/// </summary>
		public METAViewEmployeeListingFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewEmployeeListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewEmployeeListingFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewEmployeeListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewEmployeeListingFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewEmployeeListingFilterBuilder

	#region METAViewEmployeeListingParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewEmployeeListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewEmployeeListingParameterBuilder : ParameterizedSqlFilterBuilder<METAViewEmployeeListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewEmployeeListingParameterBuilder class.
		/// </summary>
		public METAViewEmployeeListingParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewEmployeeListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewEmployeeListingParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewEmployeeListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewEmployeeListingParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewEmployeeListingParameterBuilder
	
	#region METAViewEmployeeListingSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewEmployeeListing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAViewEmployeeListingSortBuilder : SqlSortBuilder<METAViewEmployeeListingColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewEmployeeListingSqlSortBuilder class.
		/// </summary>
		public METAViewEmployeeListingSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAViewEmployeeListingSortBuilder

} // end namespace
