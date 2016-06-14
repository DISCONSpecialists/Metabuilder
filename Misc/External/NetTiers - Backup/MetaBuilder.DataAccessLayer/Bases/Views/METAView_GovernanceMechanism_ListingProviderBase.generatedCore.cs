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
	/// This class is the base class for any <see cref="METAView_GovernanceMechanism_ListingProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAView_GovernanceMechanism_ListingProviderBaseCore : EntityViewProviderBase<METAView_GovernanceMechanism_Listing>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAView_GovernanceMechanism_Listing&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAView_GovernanceMechanism_Listing&gt;"/></returns>
		protected static VList&lt;METAView_GovernanceMechanism_Listing&gt; Fill(DataSet dataSet, VList<METAView_GovernanceMechanism_Listing> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAView_GovernanceMechanism_Listing>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAView_GovernanceMechanism_Listing&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAView_GovernanceMechanism_Listing>"/></returns>
		protected static VList&lt;METAView_GovernanceMechanism_Listing&gt; Fill(DataTable dataTable, VList<METAView_GovernanceMechanism_Listing> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAView_GovernanceMechanism_Listing c = new METAView_GovernanceMechanism_Listing();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeId = (Convert.IsDBNull(row["WorkspaceTypeId"]))?(int)0:(System.Int32?)row["WorkspaceTypeId"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
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
		/// Fill an <see cref="VList&lt;METAView_GovernanceMechanism_Listing&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAView_GovernanceMechanism_Listing&gt;"/></returns>
		protected VList<METAView_GovernanceMechanism_Listing> Fill(IDataReader reader, VList<METAView_GovernanceMechanism_Listing> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAView_GovernanceMechanism_Listing entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAView_GovernanceMechanism_Listing>("METAView_GovernanceMechanism_Listing",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAView_GovernanceMechanism_Listing();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAView_GovernanceMechanism_ListingColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_GovernanceMechanism_ListingColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_GovernanceMechanism_ListingColumn.WorkspaceTypeId)];
					//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
					entity.VCStatusID = (System.Int32)reader[((int)METAView_GovernanceMechanism_ListingColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.pkid = (System.Int32)reader[((int)METAView_GovernanceMechanism_ListingColumn.pkid)];
					//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAView_GovernanceMechanism_ListingColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAView_GovernanceMechanism_ListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_GovernanceMechanism_ListingColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.EnvironmentInd = (reader.IsDBNull(((int)METAView_GovernanceMechanism_ListingColumn.EnvironmentInd)))?null:(System.String)reader[((int)METAView_GovernanceMechanism_ListingColumn.EnvironmentInd)];
					//entity.EnvironmentInd = (Convert.IsDBNull(reader["EnvironmentInd"]))?string.Empty:(System.String)reader["EnvironmentInd"];
					entity.GovernanceMechType = (reader.IsDBNull(((int)METAView_GovernanceMechanism_ListingColumn.GovernanceMechType)))?null:(System.String)reader[((int)METAView_GovernanceMechanism_ListingColumn.GovernanceMechType)];
					//entity.GovernanceMechType = (Convert.IsDBNull(reader["GovernanceMechType"]))?string.Empty:(System.String)reader["GovernanceMechType"];
					entity.UniqueRef = (reader.IsDBNull(((int)METAView_GovernanceMechanism_ListingColumn.UniqueRef)))?null:(System.String)reader[((int)METAView_GovernanceMechanism_ListingColumn.UniqueRef)];
					//entity.UniqueRef = (Convert.IsDBNull(reader["UniqueRef"]))?string.Empty:(System.String)reader["UniqueRef"];
					entity.ValidityPeriod = (reader.IsDBNull(((int)METAView_GovernanceMechanism_ListingColumn.ValidityPeriod)))?null:(System.String)reader[((int)METAView_GovernanceMechanism_ListingColumn.ValidityPeriod)];
					//entity.ValidityPeriod = (Convert.IsDBNull(reader["ValidityPeriod"]))?string.Empty:(System.String)reader["ValidityPeriod"];
					entity.Description = (reader.IsDBNull(((int)METAView_GovernanceMechanism_ListingColumn.Description)))?null:(System.String)reader[((int)METAView_GovernanceMechanism_ListingColumn.Description)];
					//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
					entity.GapType = (reader.IsDBNull(((int)METAView_GovernanceMechanism_ListingColumn.GapType)))?null:(System.String)reader[((int)METAView_GovernanceMechanism_ListingColumn.GapType)];
					//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAView_GovernanceMechanism_ListingColumn.CustomField1)))?null:(System.String)reader[((int)METAView_GovernanceMechanism_ListingColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAView_GovernanceMechanism_ListingColumn.CustomField2)))?null:(System.String)reader[((int)METAView_GovernanceMechanism_ListingColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAView_GovernanceMechanism_ListingColumn.CustomField3)))?null:(System.String)reader[((int)METAView_GovernanceMechanism_ListingColumn.CustomField3)];
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
		/// Refreshes the <see cref="METAView_GovernanceMechanism_Listing"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_GovernanceMechanism_Listing"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAView_GovernanceMechanism_Listing entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAView_GovernanceMechanism_ListingColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_GovernanceMechanism_ListingColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_GovernanceMechanism_ListingColumn.WorkspaceTypeId)];
			//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
			entity.VCStatusID = (System.Int32)reader[((int)METAView_GovernanceMechanism_ListingColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.pkid = (System.Int32)reader[((int)METAView_GovernanceMechanism_ListingColumn.pkid)];
			//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAView_GovernanceMechanism_ListingColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAView_GovernanceMechanism_ListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_GovernanceMechanism_ListingColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.EnvironmentInd = (reader.IsDBNull(((int)METAView_GovernanceMechanism_ListingColumn.EnvironmentInd)))?null:(System.String)reader[((int)METAView_GovernanceMechanism_ListingColumn.EnvironmentInd)];
			//entity.EnvironmentInd = (Convert.IsDBNull(reader["EnvironmentInd"]))?string.Empty:(System.String)reader["EnvironmentInd"];
			entity.GovernanceMechType = (reader.IsDBNull(((int)METAView_GovernanceMechanism_ListingColumn.GovernanceMechType)))?null:(System.String)reader[((int)METAView_GovernanceMechanism_ListingColumn.GovernanceMechType)];
			//entity.GovernanceMechType = (Convert.IsDBNull(reader["GovernanceMechType"]))?string.Empty:(System.String)reader["GovernanceMechType"];
			entity.UniqueRef = (reader.IsDBNull(((int)METAView_GovernanceMechanism_ListingColumn.UniqueRef)))?null:(System.String)reader[((int)METAView_GovernanceMechanism_ListingColumn.UniqueRef)];
			//entity.UniqueRef = (Convert.IsDBNull(reader["UniqueRef"]))?string.Empty:(System.String)reader["UniqueRef"];
			entity.ValidityPeriod = (reader.IsDBNull(((int)METAView_GovernanceMechanism_ListingColumn.ValidityPeriod)))?null:(System.String)reader[((int)METAView_GovernanceMechanism_ListingColumn.ValidityPeriod)];
			//entity.ValidityPeriod = (Convert.IsDBNull(reader["ValidityPeriod"]))?string.Empty:(System.String)reader["ValidityPeriod"];
			entity.Description = (reader.IsDBNull(((int)METAView_GovernanceMechanism_ListingColumn.Description)))?null:(System.String)reader[((int)METAView_GovernanceMechanism_ListingColumn.Description)];
			//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
			entity.GapType = (reader.IsDBNull(((int)METAView_GovernanceMechanism_ListingColumn.GapType)))?null:(System.String)reader[((int)METAView_GovernanceMechanism_ListingColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAView_GovernanceMechanism_ListingColumn.CustomField1)))?null:(System.String)reader[((int)METAView_GovernanceMechanism_ListingColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAView_GovernanceMechanism_ListingColumn.CustomField2)))?null:(System.String)reader[((int)METAView_GovernanceMechanism_ListingColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAView_GovernanceMechanism_ListingColumn.CustomField3)))?null:(System.String)reader[((int)METAView_GovernanceMechanism_ListingColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAView_GovernanceMechanism_Listing"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_GovernanceMechanism_Listing"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAView_GovernanceMechanism_Listing entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeId = (Convert.IsDBNull(dataRow["WorkspaceTypeId"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeId"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
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

	#region METAView_GovernanceMechanism_ListingFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_GovernanceMechanism_Listing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_GovernanceMechanism_ListingFilterBuilder : SqlFilterBuilder<METAView_GovernanceMechanism_ListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_GovernanceMechanism_ListingFilterBuilder class.
		/// </summary>
		public METAView_GovernanceMechanism_ListingFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_GovernanceMechanism_ListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_GovernanceMechanism_ListingFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_GovernanceMechanism_ListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_GovernanceMechanism_ListingFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_GovernanceMechanism_ListingFilterBuilder

	#region METAView_GovernanceMechanism_ListingParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_GovernanceMechanism_Listing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_GovernanceMechanism_ListingParameterBuilder : ParameterizedSqlFilterBuilder<METAView_GovernanceMechanism_ListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_GovernanceMechanism_ListingParameterBuilder class.
		/// </summary>
		public METAView_GovernanceMechanism_ListingParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_GovernanceMechanism_ListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_GovernanceMechanism_ListingParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_GovernanceMechanism_ListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_GovernanceMechanism_ListingParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_GovernanceMechanism_ListingParameterBuilder
	
	#region METAView_GovernanceMechanism_ListingSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_GovernanceMechanism_Listing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_GovernanceMechanism_ListingSortBuilder : SqlSortBuilder<METAView_GovernanceMechanism_ListingColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_GovernanceMechanism_ListingSqlSortBuilder class.
		/// </summary>
		public METAView_GovernanceMechanism_ListingSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAView_GovernanceMechanism_ListingSortBuilder

} // end namespace
