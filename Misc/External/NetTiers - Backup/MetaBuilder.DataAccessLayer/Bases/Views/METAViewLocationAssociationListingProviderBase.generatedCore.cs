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
	/// This class is the base class for any <see cref="METAViewLocationAssociationListingProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAViewLocationAssociationListingProviderBaseCore : EntityViewProviderBase<METAViewLocationAssociationListing>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAViewLocationAssociationListing&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAViewLocationAssociationListing&gt;"/></returns>
		protected static VList&lt;METAViewLocationAssociationListing&gt; Fill(DataSet dataSet, VList<METAViewLocationAssociationListing> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAViewLocationAssociationListing>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAViewLocationAssociationListing&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAViewLocationAssociationListing>"/></returns>
		protected static VList&lt;METAViewLocationAssociationListing&gt; Fill(DataTable dataTable, VList<METAViewLocationAssociationListing> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAViewLocationAssociationListing c = new METAViewLocationAssociationListing();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeID = (Convert.IsDBNull(row["WorkspaceTypeID"]))?(int)0:(System.Int32?)row["WorkspaceTypeID"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.Pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.Distance = (Convert.IsDBNull(row["Distance"]))?string.Empty:(System.String)row["Distance"];
					c.TimeIndicator = (Convert.IsDBNull(row["TimeIndicator"]))?string.Empty:(System.String)row["TimeIndicator"];
					c.AssociationType = (Convert.IsDBNull(row["AssociationType"]))?string.Empty:(System.String)row["AssociationType"];
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
		/// Fill an <see cref="VList&lt;METAViewLocationAssociationListing&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAViewLocationAssociationListing&gt;"/></returns>
		protected VList<METAViewLocationAssociationListing> Fill(IDataReader reader, VList<METAViewLocationAssociationListing> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAViewLocationAssociationListing entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAViewLocationAssociationListing>("METAViewLocationAssociationListing",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAViewLocationAssociationListing();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAViewLocationAssociationListingColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewLocationAssociationListingColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewLocationAssociationListingColumn.WorkspaceTypeID)];
					//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
					entity.VCStatusID = (System.Int32)reader[((int)METAViewLocationAssociationListingColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.Pkid = (System.Int32)reader[((int)METAViewLocationAssociationListingColumn.Pkid)];
					//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAViewLocationAssociationListingColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAViewLocationAssociationListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewLocationAssociationListingColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Distance = (reader.IsDBNull(((int)METAViewLocationAssociationListingColumn.Distance)))?null:(System.String)reader[((int)METAViewLocationAssociationListingColumn.Distance)];
					//entity.Distance = (Convert.IsDBNull(reader["Distance"]))?string.Empty:(System.String)reader["Distance"];
					entity.TimeIndicator = (reader.IsDBNull(((int)METAViewLocationAssociationListingColumn.TimeIndicator)))?null:(System.String)reader[((int)METAViewLocationAssociationListingColumn.TimeIndicator)];
					//entity.TimeIndicator = (Convert.IsDBNull(reader["TimeIndicator"]))?string.Empty:(System.String)reader["TimeIndicator"];
					entity.AssociationType = (reader.IsDBNull(((int)METAViewLocationAssociationListingColumn.AssociationType)))?null:(System.String)reader[((int)METAViewLocationAssociationListingColumn.AssociationType)];
					//entity.AssociationType = (Convert.IsDBNull(reader["AssociationType"]))?string.Empty:(System.String)reader["AssociationType"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAViewLocationAssociationListingColumn.CustomField1)))?null:(System.String)reader[((int)METAViewLocationAssociationListingColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAViewLocationAssociationListingColumn.CustomField2)))?null:(System.String)reader[((int)METAViewLocationAssociationListingColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAViewLocationAssociationListingColumn.CustomField3)))?null:(System.String)reader[((int)METAViewLocationAssociationListingColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.GapType = (reader.IsDBNull(((int)METAViewLocationAssociationListingColumn.GapType)))?null:(System.String)reader[((int)METAViewLocationAssociationListingColumn.GapType)];
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
		/// Refreshes the <see cref="METAViewLocationAssociationListing"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewLocationAssociationListing"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAViewLocationAssociationListing entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAViewLocationAssociationListingColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewLocationAssociationListingColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewLocationAssociationListingColumn.WorkspaceTypeID)];
			//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
			entity.VCStatusID = (System.Int32)reader[((int)METAViewLocationAssociationListingColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.Pkid = (System.Int32)reader[((int)METAViewLocationAssociationListingColumn.Pkid)];
			//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAViewLocationAssociationListingColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAViewLocationAssociationListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewLocationAssociationListingColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Distance = (reader.IsDBNull(((int)METAViewLocationAssociationListingColumn.Distance)))?null:(System.String)reader[((int)METAViewLocationAssociationListingColumn.Distance)];
			//entity.Distance = (Convert.IsDBNull(reader["Distance"]))?string.Empty:(System.String)reader["Distance"];
			entity.TimeIndicator = (reader.IsDBNull(((int)METAViewLocationAssociationListingColumn.TimeIndicator)))?null:(System.String)reader[((int)METAViewLocationAssociationListingColumn.TimeIndicator)];
			//entity.TimeIndicator = (Convert.IsDBNull(reader["TimeIndicator"]))?string.Empty:(System.String)reader["TimeIndicator"];
			entity.AssociationType = (reader.IsDBNull(((int)METAViewLocationAssociationListingColumn.AssociationType)))?null:(System.String)reader[((int)METAViewLocationAssociationListingColumn.AssociationType)];
			//entity.AssociationType = (Convert.IsDBNull(reader["AssociationType"]))?string.Empty:(System.String)reader["AssociationType"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAViewLocationAssociationListingColumn.CustomField1)))?null:(System.String)reader[((int)METAViewLocationAssociationListingColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAViewLocationAssociationListingColumn.CustomField2)))?null:(System.String)reader[((int)METAViewLocationAssociationListingColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAViewLocationAssociationListingColumn.CustomField3)))?null:(System.String)reader[((int)METAViewLocationAssociationListingColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.GapType = (reader.IsDBNull(((int)METAViewLocationAssociationListingColumn.GapType)))?null:(System.String)reader[((int)METAViewLocationAssociationListingColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAViewLocationAssociationListing"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewLocationAssociationListing"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAViewLocationAssociationListing entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeID = (Convert.IsDBNull(dataRow["WorkspaceTypeID"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeID"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.Pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.Distance = (Convert.IsDBNull(dataRow["Distance"]))?string.Empty:(System.String)dataRow["Distance"];
			entity.TimeIndicator = (Convert.IsDBNull(dataRow["TimeIndicator"]))?string.Empty:(System.String)dataRow["TimeIndicator"];
			entity.AssociationType = (Convert.IsDBNull(dataRow["AssociationType"]))?string.Empty:(System.String)dataRow["AssociationType"];
			entity.CustomField1 = (Convert.IsDBNull(dataRow["CustomField1"]))?string.Empty:(System.String)dataRow["CustomField1"];
			entity.CustomField2 = (Convert.IsDBNull(dataRow["CustomField2"]))?string.Empty:(System.String)dataRow["CustomField2"];
			entity.CustomField3 = (Convert.IsDBNull(dataRow["CustomField3"]))?string.Empty:(System.String)dataRow["CustomField3"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAViewLocationAssociationListingFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewLocationAssociationListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewLocationAssociationListingFilterBuilder : SqlFilterBuilder<METAViewLocationAssociationListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewLocationAssociationListingFilterBuilder class.
		/// </summary>
		public METAViewLocationAssociationListingFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewLocationAssociationListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewLocationAssociationListingFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewLocationAssociationListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewLocationAssociationListingFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewLocationAssociationListingFilterBuilder

	#region METAViewLocationAssociationListingParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewLocationAssociationListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewLocationAssociationListingParameterBuilder : ParameterizedSqlFilterBuilder<METAViewLocationAssociationListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewLocationAssociationListingParameterBuilder class.
		/// </summary>
		public METAViewLocationAssociationListingParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewLocationAssociationListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewLocationAssociationListingParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewLocationAssociationListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewLocationAssociationListingParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewLocationAssociationListingParameterBuilder
	
	#region METAViewLocationAssociationListingSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewLocationAssociationListing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAViewLocationAssociationListingSortBuilder : SqlSortBuilder<METAViewLocationAssociationListingColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewLocationAssociationListingSqlSortBuilder class.
		/// </summary>
		public METAViewLocationAssociationListingSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAViewLocationAssociationListingSortBuilder

} // end namespace
