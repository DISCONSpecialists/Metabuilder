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
	/// This class is the base class for any <see cref="METAViewConnectionTypeListingProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAViewConnectionTypeListingProviderBaseCore : EntityViewProviderBase<METAViewConnectionTypeListing>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAViewConnectionTypeListing&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAViewConnectionTypeListing&gt;"/></returns>
		protected static VList&lt;METAViewConnectionTypeListing&gt; Fill(DataSet dataSet, VList<METAViewConnectionTypeListing> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAViewConnectionTypeListing>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAViewConnectionTypeListing&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAViewConnectionTypeListing>"/></returns>
		protected static VList&lt;METAViewConnectionTypeListing&gt; Fill(DataTable dataTable, VList<METAViewConnectionTypeListing> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAViewConnectionTypeListing c = new METAViewConnectionTypeListing();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeID = (Convert.IsDBNull(row["WorkspaceTypeID"]))?(int)0:(System.Int32?)row["WorkspaceTypeID"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.Pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.Value = (Convert.IsDBNull(row["Value"]))?string.Empty:(System.String)row["Value"];
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
		/// Fill an <see cref="VList&lt;METAViewConnectionTypeListing&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAViewConnectionTypeListing&gt;"/></returns>
		protected VList<METAViewConnectionTypeListing> Fill(IDataReader reader, VList<METAViewConnectionTypeListing> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAViewConnectionTypeListing entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAViewConnectionTypeListing>("METAViewConnectionTypeListing",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAViewConnectionTypeListing();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAViewConnectionTypeListingColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewConnectionTypeListingColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewConnectionTypeListingColumn.WorkspaceTypeID)];
					//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
					entity.VCStatusID = (System.Int32)reader[((int)METAViewConnectionTypeListingColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.Pkid = (System.Int32)reader[((int)METAViewConnectionTypeListingColumn.Pkid)];
					//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAViewConnectionTypeListingColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAViewConnectionTypeListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewConnectionTypeListingColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Value = (reader.IsDBNull(((int)METAViewConnectionTypeListingColumn.Value)))?null:(System.String)reader[((int)METAViewConnectionTypeListingColumn.Value)];
					//entity.Value = (Convert.IsDBNull(reader["Value"]))?string.Empty:(System.String)reader["Value"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAViewConnectionTypeListingColumn.CustomField1)))?null:(System.String)reader[((int)METAViewConnectionTypeListingColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAViewConnectionTypeListingColumn.CustomField2)))?null:(System.String)reader[((int)METAViewConnectionTypeListingColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAViewConnectionTypeListingColumn.CustomField3)))?null:(System.String)reader[((int)METAViewConnectionTypeListingColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.GapType = (reader.IsDBNull(((int)METAViewConnectionTypeListingColumn.GapType)))?null:(System.String)reader[((int)METAViewConnectionTypeListingColumn.GapType)];
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
		/// Refreshes the <see cref="METAViewConnectionTypeListing"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewConnectionTypeListing"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAViewConnectionTypeListing entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAViewConnectionTypeListingColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewConnectionTypeListingColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewConnectionTypeListingColumn.WorkspaceTypeID)];
			//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
			entity.VCStatusID = (System.Int32)reader[((int)METAViewConnectionTypeListingColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.Pkid = (System.Int32)reader[((int)METAViewConnectionTypeListingColumn.Pkid)];
			//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAViewConnectionTypeListingColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAViewConnectionTypeListingColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewConnectionTypeListingColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Value = (reader.IsDBNull(((int)METAViewConnectionTypeListingColumn.Value)))?null:(System.String)reader[((int)METAViewConnectionTypeListingColumn.Value)];
			//entity.Value = (Convert.IsDBNull(reader["Value"]))?string.Empty:(System.String)reader["Value"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAViewConnectionTypeListingColumn.CustomField1)))?null:(System.String)reader[((int)METAViewConnectionTypeListingColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAViewConnectionTypeListingColumn.CustomField2)))?null:(System.String)reader[((int)METAViewConnectionTypeListingColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAViewConnectionTypeListingColumn.CustomField3)))?null:(System.String)reader[((int)METAViewConnectionTypeListingColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.GapType = (reader.IsDBNull(((int)METAViewConnectionTypeListingColumn.GapType)))?null:(System.String)reader[((int)METAViewConnectionTypeListingColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAViewConnectionTypeListing"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewConnectionTypeListing"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAViewConnectionTypeListing entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeID = (Convert.IsDBNull(dataRow["WorkspaceTypeID"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeID"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.Pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.Value = (Convert.IsDBNull(dataRow["Value"]))?string.Empty:(System.String)dataRow["Value"];
			entity.CustomField1 = (Convert.IsDBNull(dataRow["CustomField1"]))?string.Empty:(System.String)dataRow["CustomField1"];
			entity.CustomField2 = (Convert.IsDBNull(dataRow["CustomField2"]))?string.Empty:(System.String)dataRow["CustomField2"];
			entity.CustomField3 = (Convert.IsDBNull(dataRow["CustomField3"]))?string.Empty:(System.String)dataRow["CustomField3"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAViewConnectionTypeListingFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewConnectionTypeListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewConnectionTypeListingFilterBuilder : SqlFilterBuilder<METAViewConnectionTypeListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewConnectionTypeListingFilterBuilder class.
		/// </summary>
		public METAViewConnectionTypeListingFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewConnectionTypeListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewConnectionTypeListingFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewConnectionTypeListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewConnectionTypeListingFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewConnectionTypeListingFilterBuilder

	#region METAViewConnectionTypeListingParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewConnectionTypeListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewConnectionTypeListingParameterBuilder : ParameterizedSqlFilterBuilder<METAViewConnectionTypeListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewConnectionTypeListingParameterBuilder class.
		/// </summary>
		public METAViewConnectionTypeListingParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewConnectionTypeListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewConnectionTypeListingParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewConnectionTypeListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewConnectionTypeListingParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewConnectionTypeListingParameterBuilder
	
	#region METAViewConnectionTypeListingSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewConnectionTypeListing"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAViewConnectionTypeListingSortBuilder : SqlSortBuilder<METAViewConnectionTypeListingColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewConnectionTypeListingSqlSortBuilder class.
		/// </summary>
		public METAViewConnectionTypeListingSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAViewConnectionTypeListingSortBuilder

} // end namespace
