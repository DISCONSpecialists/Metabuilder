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
	/// This class is the base class for any <see cref="METAViewRationaleRetrievalProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAViewRationaleRetrievalProviderBaseCore : EntityViewProviderBase<METAViewRationaleRetrieval>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAViewRationaleRetrieval&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAViewRationaleRetrieval&gt;"/></returns>
		protected static VList&lt;METAViewRationaleRetrieval&gt; Fill(DataSet dataSet, VList<METAViewRationaleRetrieval> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAViewRationaleRetrieval>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAViewRationaleRetrieval&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAViewRationaleRetrieval>"/></returns>
		protected static VList&lt;METAViewRationaleRetrieval&gt; Fill(DataTable dataTable, VList<METAViewRationaleRetrieval> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAViewRationaleRetrieval c = new METAViewRationaleRetrieval();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeID = (Convert.IsDBNull(row["WorkspaceTypeID"]))?(int)0:(System.Int32?)row["WorkspaceTypeID"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.Pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.UniqueRef = (Convert.IsDBNull(row["UniqueRef"]))?string.Empty:(System.String)row["UniqueRef"];
					c.RationaleType = (Convert.IsDBNull(row["RationaleType"]))?string.Empty:(System.String)row["RationaleType"];
					c.Value = (Convert.IsDBNull(row["Value"]))?string.Empty:(System.String)row["Value"];
					c.AuthorName = (Convert.IsDBNull(row["AuthorName"]))?string.Empty:(System.String)row["AuthorName"];
					c.EffectiveDate = (Convert.IsDBNull(row["EffectiveDate"]))?string.Empty:(System.String)row["EffectiveDate"];
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
		/// Fill an <see cref="VList&lt;METAViewRationaleRetrieval&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAViewRationaleRetrieval&gt;"/></returns>
		protected VList<METAViewRationaleRetrieval> Fill(IDataReader reader, VList<METAViewRationaleRetrieval> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAViewRationaleRetrieval entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAViewRationaleRetrieval>("METAViewRationaleRetrieval",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAViewRationaleRetrieval();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAViewRationaleRetrievalColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewRationaleRetrievalColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewRationaleRetrievalColumn.WorkspaceTypeID)];
					//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
					entity.VCStatusID = (System.Int32)reader[((int)METAViewRationaleRetrievalColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.Pkid = (System.Int32)reader[((int)METAViewRationaleRetrievalColumn.Pkid)];
					//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAViewRationaleRetrievalColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAViewRationaleRetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewRationaleRetrievalColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.UniqueRef = (reader.IsDBNull(((int)METAViewRationaleRetrievalColumn.UniqueRef)))?null:(System.String)reader[((int)METAViewRationaleRetrievalColumn.UniqueRef)];
					//entity.UniqueRef = (Convert.IsDBNull(reader["UniqueRef"]))?string.Empty:(System.String)reader["UniqueRef"];
					entity.RationaleType = (reader.IsDBNull(((int)METAViewRationaleRetrievalColumn.RationaleType)))?null:(System.String)reader[((int)METAViewRationaleRetrievalColumn.RationaleType)];
					//entity.RationaleType = (Convert.IsDBNull(reader["RationaleType"]))?string.Empty:(System.String)reader["RationaleType"];
					entity.Value = (reader.IsDBNull(((int)METAViewRationaleRetrievalColumn.Value)))?null:(System.String)reader[((int)METAViewRationaleRetrievalColumn.Value)];
					//entity.Value = (Convert.IsDBNull(reader["Value"]))?string.Empty:(System.String)reader["Value"];
					entity.AuthorName = (reader.IsDBNull(((int)METAViewRationaleRetrievalColumn.AuthorName)))?null:(System.String)reader[((int)METAViewRationaleRetrievalColumn.AuthorName)];
					//entity.AuthorName = (Convert.IsDBNull(reader["AuthorName"]))?string.Empty:(System.String)reader["AuthorName"];
					entity.EffectiveDate = (reader.IsDBNull(((int)METAViewRationaleRetrievalColumn.EffectiveDate)))?null:(System.String)reader[((int)METAViewRationaleRetrievalColumn.EffectiveDate)];
					//entity.EffectiveDate = (Convert.IsDBNull(reader["EffectiveDate"]))?string.Empty:(System.String)reader["EffectiveDate"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAViewRationaleRetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAViewRationaleRetrievalColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAViewRationaleRetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAViewRationaleRetrievalColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAViewRationaleRetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAViewRationaleRetrievalColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.GapType = (reader.IsDBNull(((int)METAViewRationaleRetrievalColumn.GapType)))?null:(System.String)reader[((int)METAViewRationaleRetrievalColumn.GapType)];
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
		/// Refreshes the <see cref="METAViewRationaleRetrieval"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewRationaleRetrieval"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAViewRationaleRetrieval entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAViewRationaleRetrievalColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewRationaleRetrievalColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewRationaleRetrievalColumn.WorkspaceTypeID)];
			//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
			entity.VCStatusID = (System.Int32)reader[((int)METAViewRationaleRetrievalColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.Pkid = (System.Int32)reader[((int)METAViewRationaleRetrievalColumn.Pkid)];
			//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAViewRationaleRetrievalColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAViewRationaleRetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewRationaleRetrievalColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.UniqueRef = (reader.IsDBNull(((int)METAViewRationaleRetrievalColumn.UniqueRef)))?null:(System.String)reader[((int)METAViewRationaleRetrievalColumn.UniqueRef)];
			//entity.UniqueRef = (Convert.IsDBNull(reader["UniqueRef"]))?string.Empty:(System.String)reader["UniqueRef"];
			entity.RationaleType = (reader.IsDBNull(((int)METAViewRationaleRetrievalColumn.RationaleType)))?null:(System.String)reader[((int)METAViewRationaleRetrievalColumn.RationaleType)];
			//entity.RationaleType = (Convert.IsDBNull(reader["RationaleType"]))?string.Empty:(System.String)reader["RationaleType"];
			entity.Value = (reader.IsDBNull(((int)METAViewRationaleRetrievalColumn.Value)))?null:(System.String)reader[((int)METAViewRationaleRetrievalColumn.Value)];
			//entity.Value = (Convert.IsDBNull(reader["Value"]))?string.Empty:(System.String)reader["Value"];
			entity.AuthorName = (reader.IsDBNull(((int)METAViewRationaleRetrievalColumn.AuthorName)))?null:(System.String)reader[((int)METAViewRationaleRetrievalColumn.AuthorName)];
			//entity.AuthorName = (Convert.IsDBNull(reader["AuthorName"]))?string.Empty:(System.String)reader["AuthorName"];
			entity.EffectiveDate = (reader.IsDBNull(((int)METAViewRationaleRetrievalColumn.EffectiveDate)))?null:(System.String)reader[((int)METAViewRationaleRetrievalColumn.EffectiveDate)];
			//entity.EffectiveDate = (Convert.IsDBNull(reader["EffectiveDate"]))?string.Empty:(System.String)reader["EffectiveDate"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAViewRationaleRetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAViewRationaleRetrievalColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAViewRationaleRetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAViewRationaleRetrievalColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAViewRationaleRetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAViewRationaleRetrievalColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.GapType = (reader.IsDBNull(((int)METAViewRationaleRetrievalColumn.GapType)))?null:(System.String)reader[((int)METAViewRationaleRetrievalColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAViewRationaleRetrieval"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewRationaleRetrieval"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAViewRationaleRetrieval entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeID = (Convert.IsDBNull(dataRow["WorkspaceTypeID"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeID"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.Pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.UniqueRef = (Convert.IsDBNull(dataRow["UniqueRef"]))?string.Empty:(System.String)dataRow["UniqueRef"];
			entity.RationaleType = (Convert.IsDBNull(dataRow["RationaleType"]))?string.Empty:(System.String)dataRow["RationaleType"];
			entity.Value = (Convert.IsDBNull(dataRow["Value"]))?string.Empty:(System.String)dataRow["Value"];
			entity.AuthorName = (Convert.IsDBNull(dataRow["AuthorName"]))?string.Empty:(System.String)dataRow["AuthorName"];
			entity.EffectiveDate = (Convert.IsDBNull(dataRow["EffectiveDate"]))?string.Empty:(System.String)dataRow["EffectiveDate"];
			entity.CustomField1 = (Convert.IsDBNull(dataRow["CustomField1"]))?string.Empty:(System.String)dataRow["CustomField1"];
			entity.CustomField2 = (Convert.IsDBNull(dataRow["CustomField2"]))?string.Empty:(System.String)dataRow["CustomField2"];
			entity.CustomField3 = (Convert.IsDBNull(dataRow["CustomField3"]))?string.Empty:(System.String)dataRow["CustomField3"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAViewRationaleRetrievalFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewRationaleRetrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewRationaleRetrievalFilterBuilder : SqlFilterBuilder<METAViewRationaleRetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewRationaleRetrievalFilterBuilder class.
		/// </summary>
		public METAViewRationaleRetrievalFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewRationaleRetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewRationaleRetrievalFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewRationaleRetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewRationaleRetrievalFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewRationaleRetrievalFilterBuilder

	#region METAViewRationaleRetrievalParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewRationaleRetrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewRationaleRetrievalParameterBuilder : ParameterizedSqlFilterBuilder<METAViewRationaleRetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewRationaleRetrievalParameterBuilder class.
		/// </summary>
		public METAViewRationaleRetrievalParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewRationaleRetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewRationaleRetrievalParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewRationaleRetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewRationaleRetrievalParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewRationaleRetrievalParameterBuilder
	
	#region METAViewRationaleRetrievalSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewRationaleRetrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAViewRationaleRetrievalSortBuilder : SqlSortBuilder<METAViewRationaleRetrievalColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewRationaleRetrievalSqlSortBuilder class.
		/// </summary>
		public METAViewRationaleRetrievalSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAViewRationaleRetrievalSortBuilder

} // end namespace
