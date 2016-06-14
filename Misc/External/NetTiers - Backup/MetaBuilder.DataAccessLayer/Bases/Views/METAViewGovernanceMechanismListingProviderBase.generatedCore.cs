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
	/// This class is the base class for any <see cref="METAViewGovernanceMechanismListingProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAViewGovernanceMechanismListingProviderBaseCore : EntityViewProviderBase<METAViewGovernanceMechanismListing>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAViewGovernanceMechanismListing&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAViewGovernanceMechanismListing&gt;"/></returns>
		protected static VList&lt;METAViewGovernanceMechanismListing&gt; Fill(DataSet dataSet, VList<METAViewGovernanceMechanismListing> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAViewGovernanceMechanismListing>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAViewGovernanceMechanismListing&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAViewGovernanceMechanismListing>"/></returns>
		protected static VList&lt;METAViewGovernanceMechanismListing&gt; Fill(DataTable dataTable, VList<METAViewGovernanceMechanismListing> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAViewGovernanceMechanismListing c = new METAViewGovernanceMechanismListing();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeID = (Convert.IsDBNull(row["WorkspaceTypeID"]))?(int)0:(System.Int32?)row["WorkspaceTypeID"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.Pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.EnvironmentInd = (Convert.IsDBNull(row["EnvironmentInd"]))?string.Empty:(System.String)row["EnvironmentInd"];
					c.GovernanceMechType = (Convert.IsDBNull(row["GovernanceMechType"]))?string.Empty:(System.String)row["GovernanceMechType"];
					c.UniqueRef = (Convert.IsDBNull(row["UniqueRef"]))?string.Empty:(System.String)row["UniqueRef"];
					c.ValidityPeriod = (Convert.IsDBNull(row["ValidityPeriod"]))?string.Empty:(System.String)row["ValidityPeriod"];
					c.Description = (Convert.IsDBNull(row["Description"]))?string.Empty:(System.String)row["Description"];
					c.GapType = (Convert.IsDBNull(row["GapType"]))?string.Empty:(System.String)row["GapType"];
					c.CustomField1 = (Convert.IsDBNull(row["CustomField1"]))?string.Empty:(System.String)row["CustomField1"];
					c.CustomField2 = (Convert.IsDBNull(row["CustomField2"]))?string.Empty:(System.String)row["CustomField2"];
					c.CustomField3 = (Convert.IsDBNull(row["CustomField3"]))?string.Empty:(System.String)row["CustomField3"];
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
		/// Fill an <see cref="VList&lt;METAViewGovernanceMechanismListing&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAViewGovernanceMechanismListing&gt;"/></returns>
		protected VList<METAViewGovernanceMechanismListing> Fill(IDataReader reader, VList<METAViewGovernanceMechanismListing> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAViewGovernanceMechanismListing entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAViewGovernanceMechanismListing>("METAViewGovernanceMechanismListing",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAViewGovernanceMechanismListing();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAViewGovernanceMechanismListingColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewGovernanceMechanismListingColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewGovernanceMechanismListingColumn.WorkspaceTypeID)];
					//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
					entity.VCStatusID = (System.Int32)reader[((int)METAViewGovernanceMechanismListingColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.Pkid = (System.Int32)reader[((int)METAViewGovernanceMechanismListingColumn.Pkid)];
					//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAViewGovernanceMechanismListingColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAViewGovernanceMechanismListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewGovernanceMechanismListingColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.EnvironmentInd = (reader.IsDBNull(((int)METAViewGovernanceMechanismListingColumn.EnvironmentInd)))?null:(System.String)reader[((int)METAViewGovernanceMechanismListingColumn.EnvironmentInd)];
					//entity.EnvironmentInd = (Convert.IsDBNull(reader["EnvironmentInd"]))?string.Empty:(System.String)reader["EnvironmentInd"];
					entity.GovernanceMechType = (reader.IsDBNull(((int)METAViewGovernanceMechanismListingColumn.GovernanceMechType)))?null:(System.String)reader[((int)METAViewGovernanceMechanismListingColumn.GovernanceMechType)];
					//entity.GovernanceMechType = (Convert.IsDBNull(reader["GovernanceMechType"]))?string.Empty:(System.String)reader["GovernanceMechType"];
					entity.UniqueRef = (reader.IsDBNull(((int)METAViewGovernanceMechanismListingColumn.UniqueRef)))?null:(System.String)reader[((int)METAViewGovernanceMechanismListingColumn.UniqueRef)];
					//entity.UniqueRef = (Convert.IsDBNull(reader["UniqueRef"]))?string.Empty:(System.String)reader["UniqueRef"];
					entity.ValidityPeriod = (reader.IsDBNull(((int)METAViewGovernanceMechanismListingColumn.ValidityPeriod)))?null:(System.String)reader[((int)METAViewGovernanceMechanismListingColumn.ValidityPeriod)];
					//entity.ValidityPeriod = (Convert.IsDBNull(reader["ValidityPeriod"]))?string.Empty:(System.String)reader["ValidityPeriod"];
					entity.Description = (reader.IsDBNull(((int)METAViewGovernanceMechanismListingColumn.Description)))?null:(System.String)reader[((int)METAViewGovernanceMechanismListingColumn.Description)];
					//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
					entity.GapType = (reader.IsDBNull(((int)METAViewGovernanceMechanismListingColumn.GapType)))?null:(System.String)reader[((int)METAViewGovernanceMechanismListingColumn.GapType)];
					//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAViewGovernanceMechanismListingColumn.CustomField1)))?null:(System.String)reader[((int)METAViewGovernanceMechanismListingColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAViewGovernanceMechanismListingColumn.CustomField2)))?null:(System.String)reader[((int)METAViewGovernanceMechanismListingColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAViewGovernanceMechanismListingColumn.CustomField3)))?null:(System.String)reader[((int)METAViewGovernanceMechanismListingColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
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
		/// Refreshes the <see cref="METAViewGovernanceMechanismListing"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewGovernanceMechanismListing"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAViewGovernanceMechanismListing entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAViewGovernanceMechanismListingColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewGovernanceMechanismListingColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewGovernanceMechanismListingColumn.WorkspaceTypeID)];
			//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
			entity.VCStatusID = (System.Int32)reader[((int)METAViewGovernanceMechanismListingColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.Pkid = (System.Int32)reader[((int)METAViewGovernanceMechanismListingColumn.Pkid)];
			//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAViewGovernanceMechanismListingColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAViewGovernanceMechanismListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewGovernanceMechanismListingColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.EnvironmentInd = (reader.IsDBNull(((int)METAViewGovernanceMechanismListingColumn.EnvironmentInd)))?null:(System.String)reader[((int)METAViewGovernanceMechanismListingColumn.EnvironmentInd)];
			//entity.EnvironmentInd = (Convert.IsDBNull(reader["EnvironmentInd"]))?string.Empty:(System.String)reader["EnvironmentInd"];
			entity.GovernanceMechType = (reader.IsDBNull(((int)METAViewGovernanceMechanismListingColumn.GovernanceMechType)))?null:(System.String)reader[((int)METAViewGovernanceMechanismListingColumn.GovernanceMechType)];
			//entity.GovernanceMechType = (Convert.IsDBNull(reader["GovernanceMechType"]))?string.Empty:(System.String)reader["GovernanceMechType"];
			entity.UniqueRef = (reader.IsDBNull(((int)METAViewGovernanceMechanismListingColumn.UniqueRef)))?null:(System.String)reader[((int)METAViewGovernanceMechanismListingColumn.UniqueRef)];
			//entity.UniqueRef = (Convert.IsDBNull(reader["UniqueRef"]))?string.Empty:(System.String)reader["UniqueRef"];
			entity.ValidityPeriod = (reader.IsDBNull(((int)METAViewGovernanceMechanismListingColumn.ValidityPeriod)))?null:(System.String)reader[((int)METAViewGovernanceMechanismListingColumn.ValidityPeriod)];
			//entity.ValidityPeriod = (Convert.IsDBNull(reader["ValidityPeriod"]))?string.Empty:(System.String)reader["ValidityPeriod"];
			entity.Description = (reader.IsDBNull(((int)METAViewGovernanceMechanismListingColumn.Description)))?null:(System.String)reader[((int)METAViewGovernanceMechanismListingColumn.Description)];
			//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
			entity.GapType = (reader.IsDBNull(((int)METAViewGovernanceMechanismListingColumn.GapType)))?null:(System.String)reader[((int)METAViewGovernanceMechanismListingColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAViewGovernanceMechanismListingColumn.CustomField1)))?null:(System.String)reader[((int)METAViewGovernanceMechanismListingColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAViewGovernanceMechanismListingColumn.CustomField2)))?null:(System.String)reader[((int)METAViewGovernanceMechanismListingColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAViewGovernanceMechanismListingColumn.CustomField3)))?null:(System.String)reader[((int)METAViewGovernanceMechanismListingColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAViewGovernanceMechanismListing"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewGovernanceMechanismListing"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAViewGovernanceMechanismListing entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeID = (Convert.IsDBNull(dataRow["WorkspaceTypeID"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeID"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.Pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.EnvironmentInd = (Convert.IsDBNull(dataRow["EnvironmentInd"]))?string.Empty:(System.String)dataRow["EnvironmentInd"];
			entity.GovernanceMechType = (Convert.IsDBNull(dataRow["GovernanceMechType"]))?string.Empty:(System.String)dataRow["GovernanceMechType"];
			entity.UniqueRef = (Convert.IsDBNull(dataRow["UniqueRef"]))?string.Empty:(System.String)dataRow["UniqueRef"];
			entity.ValidityPeriod = (Convert.IsDBNull(dataRow["ValidityPeriod"]))?string.Empty:(System.String)dataRow["ValidityPeriod"];
			entity.Description = (Convert.IsDBNull(dataRow["Description"]))?string.Empty:(System.String)dataRow["Description"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.CustomField1 = (Convert.IsDBNull(dataRow["CustomField1"]))?string.Empty:(System.String)dataRow["CustomField1"];
			entity.CustomField2 = (Convert.IsDBNull(dataRow["CustomField2"]))?string.Empty:(System.String)dataRow["CustomField2"];
			entity.CustomField3 = (Convert.IsDBNull(dataRow["CustomField3"]))?string.Empty:(System.String)dataRow["CustomField3"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAViewGovernanceMechanismListingFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewGovernanceMechanismListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewGovernanceMechanismListingFilterBuilder : SqlFilterBuilder<METAViewGovernanceMechanismListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewGovernanceMechanismListingFilterBuilder class.
		/// </summary>
		public METAViewGovernanceMechanismListingFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewGovernanceMechanismListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewGovernanceMechanismListingFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewGovernanceMechanismListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewGovernanceMechanismListingFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewGovernanceMechanismListingFilterBuilder

	#region METAViewGovernanceMechanismListingParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewGovernanceMechanismListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewGovernanceMechanismListingParameterBuilder : ParameterizedSqlFilterBuilder<METAViewGovernanceMechanismListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewGovernanceMechanismListingParameterBuilder class.
		/// </summary>
		public METAViewGovernanceMechanismListingParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewGovernanceMechanismListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewGovernanceMechanismListingParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewGovernanceMechanismListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewGovernanceMechanismListingParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewGovernanceMechanismListingParameterBuilder
	
	#region METAViewGovernanceMechanismListingSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewGovernanceMechanismListing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAViewGovernanceMechanismListingSortBuilder : SqlSortBuilder<METAViewGovernanceMechanismListingColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewGovernanceMechanismListingSqlSortBuilder class.
		/// </summary>
		public METAViewGovernanceMechanismListingSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAViewGovernanceMechanismListingSortBuilder

} // end namespace
