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
	/// This class is the base class for any <see cref="METAViewImplicationListingProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAViewImplicationListingProviderBaseCore : EntityViewProviderBase<METAViewImplicationListing>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAViewImplicationListing&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAViewImplicationListing&gt;"/></returns>
		protected static VList&lt;METAViewImplicationListing&gt; Fill(DataSet dataSet, VList<METAViewImplicationListing> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAViewImplicationListing>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAViewImplicationListing&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAViewImplicationListing>"/></returns>
		protected static VList&lt;METAViewImplicationListing&gt; Fill(DataTable dataTable, VList<METAViewImplicationListing> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAViewImplicationListing c = new METAViewImplicationListing();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeID = (Convert.IsDBNull(row["WorkspaceTypeID"]))?(int)0:(System.Int32?)row["WorkspaceTypeID"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.Pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.Name = (Convert.IsDBNull(row["Name"]))?string.Empty:(System.String)row["Name"];
					c.ExtInfIndicator = (Convert.IsDBNull(row["Ext_Inf_Indicator"]))?(int)0:(System.Int32?)row["Ext_Inf_Indicator"];
					c.IntCapabilityIndicator = (Convert.IsDBNull(row["Int_Capability_Indicator"]))?(int)0:(System.Int32?)row["Int_Capability_Indicator"];
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
		/// Fill an <see cref="VList&lt;METAViewImplicationListing&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAViewImplicationListing&gt;"/></returns>
		protected VList<METAViewImplicationListing> Fill(IDataReader reader, VList<METAViewImplicationListing> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAViewImplicationListing entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAViewImplicationListing>("METAViewImplicationListing",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAViewImplicationListing();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAViewImplicationListingColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewImplicationListingColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewImplicationListingColumn.WorkspaceTypeID)];
					//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
					entity.VCStatusID = (System.Int32)reader[((int)METAViewImplicationListingColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.Pkid = (System.Int32)reader[((int)METAViewImplicationListingColumn.Pkid)];
					//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAViewImplicationListingColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAViewImplicationListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewImplicationListingColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Name = (reader.IsDBNull(((int)METAViewImplicationListingColumn.Name)))?null:(System.String)reader[((int)METAViewImplicationListingColumn.Name)];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.ExtInfIndicator = (reader.IsDBNull(((int)METAViewImplicationListingColumn.ExtInfIndicator)))?null:(System.Int32?)reader[((int)METAViewImplicationListingColumn.ExtInfIndicator)];
					//entity.ExtInfIndicator = (Convert.IsDBNull(reader["Ext_Inf_Indicator"]))?(int)0:(System.Int32?)reader["Ext_Inf_Indicator"];
					entity.IntCapabilityIndicator = (reader.IsDBNull(((int)METAViewImplicationListingColumn.IntCapabilityIndicator)))?null:(System.Int32?)reader[((int)METAViewImplicationListingColumn.IntCapabilityIndicator)];
					//entity.IntCapabilityIndicator = (Convert.IsDBNull(reader["Int_Capability_Indicator"]))?(int)0:(System.Int32?)reader["Int_Capability_Indicator"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAViewImplicationListingColumn.CustomField1)))?null:(System.String)reader[((int)METAViewImplicationListingColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAViewImplicationListingColumn.CustomField2)))?null:(System.String)reader[((int)METAViewImplicationListingColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAViewImplicationListingColumn.CustomField3)))?null:(System.String)reader[((int)METAViewImplicationListingColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.GapType = (reader.IsDBNull(((int)METAViewImplicationListingColumn.GapType)))?null:(System.String)reader[((int)METAViewImplicationListingColumn.GapType)];
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
		/// Refreshes the <see cref="METAViewImplicationListing"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewImplicationListing"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAViewImplicationListing entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAViewImplicationListingColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewImplicationListingColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewImplicationListingColumn.WorkspaceTypeID)];
			//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
			entity.VCStatusID = (System.Int32)reader[((int)METAViewImplicationListingColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.Pkid = (System.Int32)reader[((int)METAViewImplicationListingColumn.Pkid)];
			//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAViewImplicationListingColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAViewImplicationListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewImplicationListingColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Name = (reader.IsDBNull(((int)METAViewImplicationListingColumn.Name)))?null:(System.String)reader[((int)METAViewImplicationListingColumn.Name)];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.ExtInfIndicator = (reader.IsDBNull(((int)METAViewImplicationListingColumn.ExtInfIndicator)))?null:(System.Int32?)reader[((int)METAViewImplicationListingColumn.ExtInfIndicator)];
			//entity.ExtInfIndicator = (Convert.IsDBNull(reader["Ext_Inf_Indicator"]))?(int)0:(System.Int32?)reader["Ext_Inf_Indicator"];
			entity.IntCapabilityIndicator = (reader.IsDBNull(((int)METAViewImplicationListingColumn.IntCapabilityIndicator)))?null:(System.Int32?)reader[((int)METAViewImplicationListingColumn.IntCapabilityIndicator)];
			//entity.IntCapabilityIndicator = (Convert.IsDBNull(reader["Int_Capability_Indicator"]))?(int)0:(System.Int32?)reader["Int_Capability_Indicator"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAViewImplicationListingColumn.CustomField1)))?null:(System.String)reader[((int)METAViewImplicationListingColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAViewImplicationListingColumn.CustomField2)))?null:(System.String)reader[((int)METAViewImplicationListingColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAViewImplicationListingColumn.CustomField3)))?null:(System.String)reader[((int)METAViewImplicationListingColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.GapType = (reader.IsDBNull(((int)METAViewImplicationListingColumn.GapType)))?null:(System.String)reader[((int)METAViewImplicationListingColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAViewImplicationListing"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewImplicationListing"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAViewImplicationListing entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeID = (Convert.IsDBNull(dataRow["WorkspaceTypeID"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeID"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.Pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.Name = (Convert.IsDBNull(dataRow["Name"]))?string.Empty:(System.String)dataRow["Name"];
			entity.ExtInfIndicator = (Convert.IsDBNull(dataRow["Ext_Inf_Indicator"]))?(int)0:(System.Int32?)dataRow["Ext_Inf_Indicator"];
			entity.IntCapabilityIndicator = (Convert.IsDBNull(dataRow["Int_Capability_Indicator"]))?(int)0:(System.Int32?)dataRow["Int_Capability_Indicator"];
			entity.CustomField1 = (Convert.IsDBNull(dataRow["CustomField1"]))?string.Empty:(System.String)dataRow["CustomField1"];
			entity.CustomField2 = (Convert.IsDBNull(dataRow["CustomField2"]))?string.Empty:(System.String)dataRow["CustomField2"];
			entity.CustomField3 = (Convert.IsDBNull(dataRow["CustomField3"]))?string.Empty:(System.String)dataRow["CustomField3"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAViewImplicationListingFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewImplicationListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewImplicationListingFilterBuilder : SqlFilterBuilder<METAViewImplicationListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewImplicationListingFilterBuilder class.
		/// </summary>
		public METAViewImplicationListingFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewImplicationListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewImplicationListingFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewImplicationListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewImplicationListingFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewImplicationListingFilterBuilder

	#region METAViewImplicationListingParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewImplicationListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewImplicationListingParameterBuilder : ParameterizedSqlFilterBuilder<METAViewImplicationListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewImplicationListingParameterBuilder class.
		/// </summary>
		public METAViewImplicationListingParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewImplicationListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewImplicationListingParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewImplicationListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewImplicationListingParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewImplicationListingParameterBuilder
	
	#region METAViewImplicationListingSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewImplicationListing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAViewImplicationListingSortBuilder : SqlSortBuilder<METAViewImplicationListingColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewImplicationListingSqlSortBuilder class.
		/// </summary>
		public METAViewImplicationListingSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAViewImplicationListingSortBuilder

} // end namespace
