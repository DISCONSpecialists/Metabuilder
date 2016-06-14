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
	/// This class is the base class for any <see cref="METAViewDataSchemaListingProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAViewDataSchemaListingProviderBaseCore : EntityViewProviderBase<METAViewDataSchemaListing>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAViewDataSchemaListing&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAViewDataSchemaListing&gt;"/></returns>
		protected static VList&lt;METAViewDataSchemaListing&gt; Fill(DataSet dataSet, VList<METAViewDataSchemaListing> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAViewDataSchemaListing>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAViewDataSchemaListing&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAViewDataSchemaListing>"/></returns>
		protected static VList&lt;METAViewDataSchemaListing&gt; Fill(DataTable dataTable, VList<METAViewDataSchemaListing> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAViewDataSchemaListing c = new METAViewDataSchemaListing();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeID = (Convert.IsDBNull(row["WorkspaceTypeID"]))?(int)0:(System.Int32?)row["WorkspaceTypeID"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.Pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.Name = (Convert.IsDBNull(row["Name"]))?string.Empty:(System.String)row["Name"];
					c.DataSchemaType = (Convert.IsDBNull(row["DataSchemaType"]))?string.Empty:(System.String)row["DataSchemaType"];
					c.CustomField1 = (Convert.IsDBNull(row["CustomField1"]))?string.Empty:(System.String)row["CustomField1"];
					c.CustomField2 = (Convert.IsDBNull(row["CustomField2"]))?string.Empty:(System.String)row["CustomField2"];
					c.CustomField3 = (Convert.IsDBNull(row["CustomField3"]))?string.Empty:(System.String)row["CustomField3"];
					c.ArchPriority = (Convert.IsDBNull(row["ArchPriority"]))?string.Empty:(System.String)row["ArchPriority"];
					c.DatabaseType = (Convert.IsDBNull(row["DatabaseType"]))?string.Empty:(System.String)row["DatabaseType"];
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
		/// Fill an <see cref="VList&lt;METAViewDataSchemaListing&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAViewDataSchemaListing&gt;"/></returns>
		protected VList<METAViewDataSchemaListing> Fill(IDataReader reader, VList<METAViewDataSchemaListing> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAViewDataSchemaListing entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAViewDataSchemaListing>("METAViewDataSchemaListing",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAViewDataSchemaListing();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAViewDataSchemaListingColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewDataSchemaListingColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewDataSchemaListingColumn.WorkspaceTypeID)];
					//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
					entity.VCStatusID = (System.Int32)reader[((int)METAViewDataSchemaListingColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.Pkid = (System.Int32)reader[((int)METAViewDataSchemaListingColumn.Pkid)];
					//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAViewDataSchemaListingColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAViewDataSchemaListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewDataSchemaListingColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Name = (reader.IsDBNull(((int)METAViewDataSchemaListingColumn.Name)))?null:(System.String)reader[((int)METAViewDataSchemaListingColumn.Name)];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.DataSchemaType = (reader.IsDBNull(((int)METAViewDataSchemaListingColumn.DataSchemaType)))?null:(System.String)reader[((int)METAViewDataSchemaListingColumn.DataSchemaType)];
					//entity.DataSchemaType = (Convert.IsDBNull(reader["DataSchemaType"]))?string.Empty:(System.String)reader["DataSchemaType"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAViewDataSchemaListingColumn.CustomField1)))?null:(System.String)reader[((int)METAViewDataSchemaListingColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAViewDataSchemaListingColumn.CustomField2)))?null:(System.String)reader[((int)METAViewDataSchemaListingColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAViewDataSchemaListingColumn.CustomField3)))?null:(System.String)reader[((int)METAViewDataSchemaListingColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.ArchPriority = (reader.IsDBNull(((int)METAViewDataSchemaListingColumn.ArchPriority)))?null:(System.String)reader[((int)METAViewDataSchemaListingColumn.ArchPriority)];
					//entity.ArchPriority = (Convert.IsDBNull(reader["ArchPriority"]))?string.Empty:(System.String)reader["ArchPriority"];
					entity.DatabaseType = (reader.IsDBNull(((int)METAViewDataSchemaListingColumn.DatabaseType)))?null:(System.String)reader[((int)METAViewDataSchemaListingColumn.DatabaseType)];
					//entity.DatabaseType = (Convert.IsDBNull(reader["DatabaseType"]))?string.Empty:(System.String)reader["DatabaseType"];
					entity.GapType = (reader.IsDBNull(((int)METAViewDataSchemaListingColumn.GapType)))?null:(System.String)reader[((int)METAViewDataSchemaListingColumn.GapType)];
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
		/// Refreshes the <see cref="METAViewDataSchemaListing"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewDataSchemaListing"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAViewDataSchemaListing entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAViewDataSchemaListingColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewDataSchemaListingColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewDataSchemaListingColumn.WorkspaceTypeID)];
			//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
			entity.VCStatusID = (System.Int32)reader[((int)METAViewDataSchemaListingColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.Pkid = (System.Int32)reader[((int)METAViewDataSchemaListingColumn.Pkid)];
			//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAViewDataSchemaListingColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAViewDataSchemaListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewDataSchemaListingColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Name = (reader.IsDBNull(((int)METAViewDataSchemaListingColumn.Name)))?null:(System.String)reader[((int)METAViewDataSchemaListingColumn.Name)];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.DataSchemaType = (reader.IsDBNull(((int)METAViewDataSchemaListingColumn.DataSchemaType)))?null:(System.String)reader[((int)METAViewDataSchemaListingColumn.DataSchemaType)];
			//entity.DataSchemaType = (Convert.IsDBNull(reader["DataSchemaType"]))?string.Empty:(System.String)reader["DataSchemaType"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAViewDataSchemaListingColumn.CustomField1)))?null:(System.String)reader[((int)METAViewDataSchemaListingColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAViewDataSchemaListingColumn.CustomField2)))?null:(System.String)reader[((int)METAViewDataSchemaListingColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAViewDataSchemaListingColumn.CustomField3)))?null:(System.String)reader[((int)METAViewDataSchemaListingColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.ArchPriority = (reader.IsDBNull(((int)METAViewDataSchemaListingColumn.ArchPriority)))?null:(System.String)reader[((int)METAViewDataSchemaListingColumn.ArchPriority)];
			//entity.ArchPriority = (Convert.IsDBNull(reader["ArchPriority"]))?string.Empty:(System.String)reader["ArchPriority"];
			entity.DatabaseType = (reader.IsDBNull(((int)METAViewDataSchemaListingColumn.DatabaseType)))?null:(System.String)reader[((int)METAViewDataSchemaListingColumn.DatabaseType)];
			//entity.DatabaseType = (Convert.IsDBNull(reader["DatabaseType"]))?string.Empty:(System.String)reader["DatabaseType"];
			entity.GapType = (reader.IsDBNull(((int)METAViewDataSchemaListingColumn.GapType)))?null:(System.String)reader[((int)METAViewDataSchemaListingColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAViewDataSchemaListing"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewDataSchemaListing"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAViewDataSchemaListing entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeID = (Convert.IsDBNull(dataRow["WorkspaceTypeID"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeID"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.Pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.Name = (Convert.IsDBNull(dataRow["Name"]))?string.Empty:(System.String)dataRow["Name"];
			entity.DataSchemaType = (Convert.IsDBNull(dataRow["DataSchemaType"]))?string.Empty:(System.String)dataRow["DataSchemaType"];
			entity.CustomField1 = (Convert.IsDBNull(dataRow["CustomField1"]))?string.Empty:(System.String)dataRow["CustomField1"];
			entity.CustomField2 = (Convert.IsDBNull(dataRow["CustomField2"]))?string.Empty:(System.String)dataRow["CustomField2"];
			entity.CustomField3 = (Convert.IsDBNull(dataRow["CustomField3"]))?string.Empty:(System.String)dataRow["CustomField3"];
			entity.ArchPriority = (Convert.IsDBNull(dataRow["ArchPriority"]))?string.Empty:(System.String)dataRow["ArchPriority"];
			entity.DatabaseType = (Convert.IsDBNull(dataRow["DatabaseType"]))?string.Empty:(System.String)dataRow["DatabaseType"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAViewDataSchemaListingFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewDataSchemaListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewDataSchemaListingFilterBuilder : SqlFilterBuilder<METAViewDataSchemaListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewDataSchemaListingFilterBuilder class.
		/// </summary>
		public METAViewDataSchemaListingFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewDataSchemaListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewDataSchemaListingFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewDataSchemaListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewDataSchemaListingFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewDataSchemaListingFilterBuilder

	#region METAViewDataSchemaListingParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewDataSchemaListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewDataSchemaListingParameterBuilder : ParameterizedSqlFilterBuilder<METAViewDataSchemaListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewDataSchemaListingParameterBuilder class.
		/// </summary>
		public METAViewDataSchemaListingParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewDataSchemaListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewDataSchemaListingParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewDataSchemaListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewDataSchemaListingParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewDataSchemaListingParameterBuilder
	
	#region METAViewDataSchemaListingSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewDataSchemaListing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAViewDataSchemaListingSortBuilder : SqlSortBuilder<METAViewDataSchemaListingColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewDataSchemaListingSqlSortBuilder class.
		/// </summary>
		public METAViewDataSchemaListingSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAViewDataSchemaListingSortBuilder

} // end namespace
