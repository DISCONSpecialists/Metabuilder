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
	/// This class is the base class for any <see cref="METAViewDataColumnListingProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAViewDataColumnListingProviderBaseCore : EntityViewProviderBase<METAViewDataColumnListing>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAViewDataColumnListing&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAViewDataColumnListing&gt;"/></returns>
		protected static VList&lt;METAViewDataColumnListing&gt; Fill(DataSet dataSet, VList<METAViewDataColumnListing> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAViewDataColumnListing>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAViewDataColumnListing&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAViewDataColumnListing>"/></returns>
		protected static VList&lt;METAViewDataColumnListing&gt; Fill(DataTable dataTable, VList<METAViewDataColumnListing> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAViewDataColumnListing c = new METAViewDataColumnListing();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeID = (Convert.IsDBNull(row["WorkspaceTypeID"]))?(int)0:(System.Int32?)row["WorkspaceTypeID"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.Pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.Name = (Convert.IsDBNull(row["Name"]))?string.Empty:(System.String)row["Name"];
					c.DomainDef = (Convert.IsDBNull(row["DomainDef"]))?string.Empty:(System.String)row["DomainDef"];
					c.CustomField1 = (Convert.IsDBNull(row["CustomField1"]))?string.Empty:(System.String)row["CustomField1"];
					c.CustomField2 = (Convert.IsDBNull(row["CustomField2"]))?string.Empty:(System.String)row["CustomField2"];
					c.CustomField3 = (Convert.IsDBNull(row["CustomField3"]))?string.Empty:(System.String)row["CustomField3"];
					c.ColumnType = (Convert.IsDBNull(row["ColumnType"]))?string.Empty:(System.String)row["ColumnType"];
					c.ColumnLength = (Convert.IsDBNull(row["ColumnLength"]))?(int)0:(System.Int32?)row["ColumnLength"];
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
		/// Fill an <see cref="VList&lt;METAViewDataColumnListing&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAViewDataColumnListing&gt;"/></returns>
		protected VList<METAViewDataColumnListing> Fill(IDataReader reader, VList<METAViewDataColumnListing> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAViewDataColumnListing entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAViewDataColumnListing>("METAViewDataColumnListing",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAViewDataColumnListing();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAViewDataColumnListingColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewDataColumnListingColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewDataColumnListingColumn.WorkspaceTypeID)];
					//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
					entity.VCStatusID = (System.Int32)reader[((int)METAViewDataColumnListingColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.Pkid = (System.Int32)reader[((int)METAViewDataColumnListingColumn.Pkid)];
					//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAViewDataColumnListingColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAViewDataColumnListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewDataColumnListingColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Name = (reader.IsDBNull(((int)METAViewDataColumnListingColumn.Name)))?null:(System.String)reader[((int)METAViewDataColumnListingColumn.Name)];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.DomainDef = (reader.IsDBNull(((int)METAViewDataColumnListingColumn.DomainDef)))?null:(System.String)reader[((int)METAViewDataColumnListingColumn.DomainDef)];
					//entity.DomainDef = (Convert.IsDBNull(reader["DomainDef"]))?string.Empty:(System.String)reader["DomainDef"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAViewDataColumnListingColumn.CustomField1)))?null:(System.String)reader[((int)METAViewDataColumnListingColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAViewDataColumnListingColumn.CustomField2)))?null:(System.String)reader[((int)METAViewDataColumnListingColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAViewDataColumnListingColumn.CustomField3)))?null:(System.String)reader[((int)METAViewDataColumnListingColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.ColumnType = (reader.IsDBNull(((int)METAViewDataColumnListingColumn.ColumnType)))?null:(System.String)reader[((int)METAViewDataColumnListingColumn.ColumnType)];
					//entity.ColumnType = (Convert.IsDBNull(reader["ColumnType"]))?string.Empty:(System.String)reader["ColumnType"];
					entity.ColumnLength = (reader.IsDBNull(((int)METAViewDataColumnListingColumn.ColumnLength)))?null:(System.Int32?)reader[((int)METAViewDataColumnListingColumn.ColumnLength)];
					//entity.ColumnLength = (Convert.IsDBNull(reader["ColumnLength"]))?(int)0:(System.Int32?)reader["ColumnLength"];
					entity.GapType = (reader.IsDBNull(((int)METAViewDataColumnListingColumn.GapType)))?null:(System.String)reader[((int)METAViewDataColumnListingColumn.GapType)];
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
		/// Refreshes the <see cref="METAViewDataColumnListing"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewDataColumnListing"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAViewDataColumnListing entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAViewDataColumnListingColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewDataColumnListingColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewDataColumnListingColumn.WorkspaceTypeID)];
			//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
			entity.VCStatusID = (System.Int32)reader[((int)METAViewDataColumnListingColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.Pkid = (System.Int32)reader[((int)METAViewDataColumnListingColumn.Pkid)];
			//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAViewDataColumnListingColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAViewDataColumnListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewDataColumnListingColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Name = (reader.IsDBNull(((int)METAViewDataColumnListingColumn.Name)))?null:(System.String)reader[((int)METAViewDataColumnListingColumn.Name)];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.DomainDef = (reader.IsDBNull(((int)METAViewDataColumnListingColumn.DomainDef)))?null:(System.String)reader[((int)METAViewDataColumnListingColumn.DomainDef)];
			//entity.DomainDef = (Convert.IsDBNull(reader["DomainDef"]))?string.Empty:(System.String)reader["DomainDef"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAViewDataColumnListingColumn.CustomField1)))?null:(System.String)reader[((int)METAViewDataColumnListingColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAViewDataColumnListingColumn.CustomField2)))?null:(System.String)reader[((int)METAViewDataColumnListingColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAViewDataColumnListingColumn.CustomField3)))?null:(System.String)reader[((int)METAViewDataColumnListingColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.ColumnType = (reader.IsDBNull(((int)METAViewDataColumnListingColumn.ColumnType)))?null:(System.String)reader[((int)METAViewDataColumnListingColumn.ColumnType)];
			//entity.ColumnType = (Convert.IsDBNull(reader["ColumnType"]))?string.Empty:(System.String)reader["ColumnType"];
			entity.ColumnLength = (reader.IsDBNull(((int)METAViewDataColumnListingColumn.ColumnLength)))?null:(System.Int32?)reader[((int)METAViewDataColumnListingColumn.ColumnLength)];
			//entity.ColumnLength = (Convert.IsDBNull(reader["ColumnLength"]))?(int)0:(System.Int32?)reader["ColumnLength"];
			entity.GapType = (reader.IsDBNull(((int)METAViewDataColumnListingColumn.GapType)))?null:(System.String)reader[((int)METAViewDataColumnListingColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAViewDataColumnListing"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewDataColumnListing"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAViewDataColumnListing entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeID = (Convert.IsDBNull(dataRow["WorkspaceTypeID"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeID"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.Pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.Name = (Convert.IsDBNull(dataRow["Name"]))?string.Empty:(System.String)dataRow["Name"];
			entity.DomainDef = (Convert.IsDBNull(dataRow["DomainDef"]))?string.Empty:(System.String)dataRow["DomainDef"];
			entity.CustomField1 = (Convert.IsDBNull(dataRow["CustomField1"]))?string.Empty:(System.String)dataRow["CustomField1"];
			entity.CustomField2 = (Convert.IsDBNull(dataRow["CustomField2"]))?string.Empty:(System.String)dataRow["CustomField2"];
			entity.CustomField3 = (Convert.IsDBNull(dataRow["CustomField3"]))?string.Empty:(System.String)dataRow["CustomField3"];
			entity.ColumnType = (Convert.IsDBNull(dataRow["ColumnType"]))?string.Empty:(System.String)dataRow["ColumnType"];
			entity.ColumnLength = (Convert.IsDBNull(dataRow["ColumnLength"]))?(int)0:(System.Int32?)dataRow["ColumnLength"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAViewDataColumnListingFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewDataColumnListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewDataColumnListingFilterBuilder : SqlFilterBuilder<METAViewDataColumnListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewDataColumnListingFilterBuilder class.
		/// </summary>
		public METAViewDataColumnListingFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewDataColumnListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewDataColumnListingFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewDataColumnListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewDataColumnListingFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewDataColumnListingFilterBuilder

	#region METAViewDataColumnListingParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewDataColumnListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewDataColumnListingParameterBuilder : ParameterizedSqlFilterBuilder<METAViewDataColumnListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewDataColumnListingParameterBuilder class.
		/// </summary>
		public METAViewDataColumnListingParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewDataColumnListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewDataColumnListingParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewDataColumnListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewDataColumnListingParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewDataColumnListingParameterBuilder
	
	#region METAViewDataColumnListingSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewDataColumnListing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAViewDataColumnListingSortBuilder : SqlSortBuilder<METAViewDataColumnListingColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewDataColumnListingSqlSortBuilder class.
		/// </summary>
		public METAViewDataColumnListingSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAViewDataColumnListingSortBuilder

} // end namespace
