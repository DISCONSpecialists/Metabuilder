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
	/// This class is the base class for any <see cref="METAViewConditionalDescriptionListingProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAViewConditionalDescriptionListingProviderBaseCore : EntityViewProviderBase<METAViewConditionalDescriptionListing>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAViewConditionalDescriptionListing&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAViewConditionalDescriptionListing&gt;"/></returns>
		protected static VList&lt;METAViewConditionalDescriptionListing&gt; Fill(DataSet dataSet, VList<METAViewConditionalDescriptionListing> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAViewConditionalDescriptionListing>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAViewConditionalDescriptionListing&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAViewConditionalDescriptionListing>"/></returns>
		protected static VList&lt;METAViewConditionalDescriptionListing&gt; Fill(DataTable dataTable, VList<METAViewConditionalDescriptionListing> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAViewConditionalDescriptionListing c = new METAViewConditionalDescriptionListing();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeID = (Convert.IsDBNull(row["WorkspaceTypeID"]))?(int)0:(System.Int32?)row["WorkspaceTypeID"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.Pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.Value = (Convert.IsDBNull(row["Value"]))?string.Empty:(System.String)row["Value"];
					c.Sequence = (Convert.IsDBNull(row["Sequence"]))?(int)0:(System.Int32?)row["Sequence"];
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
		/// Fill an <see cref="VList&lt;METAViewConditionalDescriptionListing&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAViewConditionalDescriptionListing&gt;"/></returns>
		protected VList<METAViewConditionalDescriptionListing> Fill(IDataReader reader, VList<METAViewConditionalDescriptionListing> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAViewConditionalDescriptionListing entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAViewConditionalDescriptionListing>("METAViewConditionalDescriptionListing",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAViewConditionalDescriptionListing();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAViewConditionalDescriptionListingColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewConditionalDescriptionListingColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewConditionalDescriptionListingColumn.WorkspaceTypeID)];
					//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
					entity.VCStatusID = (System.Int32)reader[((int)METAViewConditionalDescriptionListingColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.Pkid = (System.Int32)reader[((int)METAViewConditionalDescriptionListingColumn.Pkid)];
					//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAViewConditionalDescriptionListingColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAViewConditionalDescriptionListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewConditionalDescriptionListingColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Value = (reader.IsDBNull(((int)METAViewConditionalDescriptionListingColumn.Value)))?null:(System.String)reader[((int)METAViewConditionalDescriptionListingColumn.Value)];
					//entity.Value = (Convert.IsDBNull(reader["Value"]))?string.Empty:(System.String)reader["Value"];
					entity.Sequence = (reader.IsDBNull(((int)METAViewConditionalDescriptionListingColumn.Sequence)))?null:(System.Int32?)reader[((int)METAViewConditionalDescriptionListingColumn.Sequence)];
					//entity.Sequence = (Convert.IsDBNull(reader["Sequence"]))?(int)0:(System.Int32?)reader["Sequence"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAViewConditionalDescriptionListingColumn.CustomField1)))?null:(System.String)reader[((int)METAViewConditionalDescriptionListingColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAViewConditionalDescriptionListingColumn.CustomField2)))?null:(System.String)reader[((int)METAViewConditionalDescriptionListingColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAViewConditionalDescriptionListingColumn.CustomField3)))?null:(System.String)reader[((int)METAViewConditionalDescriptionListingColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.GapType = (reader.IsDBNull(((int)METAViewConditionalDescriptionListingColumn.GapType)))?null:(System.String)reader[((int)METAViewConditionalDescriptionListingColumn.GapType)];
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
		/// Refreshes the <see cref="METAViewConditionalDescriptionListing"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewConditionalDescriptionListing"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAViewConditionalDescriptionListing entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAViewConditionalDescriptionListingColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewConditionalDescriptionListingColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewConditionalDescriptionListingColumn.WorkspaceTypeID)];
			//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
			entity.VCStatusID = (System.Int32)reader[((int)METAViewConditionalDescriptionListingColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.Pkid = (System.Int32)reader[((int)METAViewConditionalDescriptionListingColumn.Pkid)];
			//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAViewConditionalDescriptionListingColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAViewConditionalDescriptionListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewConditionalDescriptionListingColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Value = (reader.IsDBNull(((int)METAViewConditionalDescriptionListingColumn.Value)))?null:(System.String)reader[((int)METAViewConditionalDescriptionListingColumn.Value)];
			//entity.Value = (Convert.IsDBNull(reader["Value"]))?string.Empty:(System.String)reader["Value"];
			entity.Sequence = (reader.IsDBNull(((int)METAViewConditionalDescriptionListingColumn.Sequence)))?null:(System.Int32?)reader[((int)METAViewConditionalDescriptionListingColumn.Sequence)];
			//entity.Sequence = (Convert.IsDBNull(reader["Sequence"]))?(int)0:(System.Int32?)reader["Sequence"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAViewConditionalDescriptionListingColumn.CustomField1)))?null:(System.String)reader[((int)METAViewConditionalDescriptionListingColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAViewConditionalDescriptionListingColumn.CustomField2)))?null:(System.String)reader[((int)METAViewConditionalDescriptionListingColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAViewConditionalDescriptionListingColumn.CustomField3)))?null:(System.String)reader[((int)METAViewConditionalDescriptionListingColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.GapType = (reader.IsDBNull(((int)METAViewConditionalDescriptionListingColumn.GapType)))?null:(System.String)reader[((int)METAViewConditionalDescriptionListingColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAViewConditionalDescriptionListing"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewConditionalDescriptionListing"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAViewConditionalDescriptionListing entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeID = (Convert.IsDBNull(dataRow["WorkspaceTypeID"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeID"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.Pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.Value = (Convert.IsDBNull(dataRow["Value"]))?string.Empty:(System.String)dataRow["Value"];
			entity.Sequence = (Convert.IsDBNull(dataRow["Sequence"]))?(int)0:(System.Int32?)dataRow["Sequence"];
			entity.CustomField1 = (Convert.IsDBNull(dataRow["CustomField1"]))?string.Empty:(System.String)dataRow["CustomField1"];
			entity.CustomField2 = (Convert.IsDBNull(dataRow["CustomField2"]))?string.Empty:(System.String)dataRow["CustomField2"];
			entity.CustomField3 = (Convert.IsDBNull(dataRow["CustomField3"]))?string.Empty:(System.String)dataRow["CustomField3"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAViewConditionalDescriptionListingFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewConditionalDescriptionListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewConditionalDescriptionListingFilterBuilder : SqlFilterBuilder<METAViewConditionalDescriptionListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewConditionalDescriptionListingFilterBuilder class.
		/// </summary>
		public METAViewConditionalDescriptionListingFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewConditionalDescriptionListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewConditionalDescriptionListingFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewConditionalDescriptionListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewConditionalDescriptionListingFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewConditionalDescriptionListingFilterBuilder

	#region METAViewConditionalDescriptionListingParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewConditionalDescriptionListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewConditionalDescriptionListingParameterBuilder : ParameterizedSqlFilterBuilder<METAViewConditionalDescriptionListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewConditionalDescriptionListingParameterBuilder class.
		/// </summary>
		public METAViewConditionalDescriptionListingParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewConditionalDescriptionListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewConditionalDescriptionListingParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewConditionalDescriptionListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewConditionalDescriptionListingParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewConditionalDescriptionListingParameterBuilder
	
	#region METAViewConditionalDescriptionListingSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewConditionalDescriptionListing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAViewConditionalDescriptionListingSortBuilder : SqlSortBuilder<METAViewConditionalDescriptionListingColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewConditionalDescriptionListingSqlSortBuilder class.
		/// </summary>
		public METAViewConditionalDescriptionListingSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAViewConditionalDescriptionListingSortBuilder

} // end namespace
