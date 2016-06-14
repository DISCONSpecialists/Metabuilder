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
	/// This class is the base class for any <see cref="METAViewAttributeRetrievalProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAViewAttributeRetrievalProviderBaseCore : EntityViewProviderBase<METAViewAttributeRetrieval>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAViewAttributeRetrieval&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAViewAttributeRetrieval&gt;"/></returns>
		protected static VList&lt;METAViewAttributeRetrieval&gt; Fill(DataSet dataSet, VList<METAViewAttributeRetrieval> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAViewAttributeRetrieval>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAViewAttributeRetrieval&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAViewAttributeRetrieval>"/></returns>
		protected static VList&lt;METAViewAttributeRetrieval&gt; Fill(DataTable dataTable, VList<METAViewAttributeRetrieval> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAViewAttributeRetrieval c = new METAViewAttributeRetrieval();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeID = (Convert.IsDBNull(row["WorkspaceTypeID"]))?(int)0:(System.Int32?)row["WorkspaceTypeID"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.Pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.Name = (Convert.IsDBNull(row["Name"]))?string.Empty:(System.String)row["Name"];
					c.CustomField1 = (Convert.IsDBNull(row["CustomField1"]))?string.Empty:(System.String)row["CustomField1"];
					c.CustomField2 = (Convert.IsDBNull(row["CustomField2"]))?string.Empty:(System.String)row["CustomField2"];
					c.CustomField3 = (Convert.IsDBNull(row["CustomField3"]))?string.Empty:(System.String)row["CustomField3"];
					c.DomainType = (Convert.IsDBNull(row["DomainType"]))?string.Empty:(System.String)row["DomainType"];
					c.DomainDef = (Convert.IsDBNull(row["DomainDef"]))?string.Empty:(System.String)row["DomainDef"];
					c.Length = (Convert.IsDBNull(row["Length"]))?(int)0:(System.Int32?)row["Length"];
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
		/// Fill an <see cref="VList&lt;METAViewAttributeRetrieval&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAViewAttributeRetrieval&gt;"/></returns>
		protected VList<METAViewAttributeRetrieval> Fill(IDataReader reader, VList<METAViewAttributeRetrieval> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAViewAttributeRetrieval entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAViewAttributeRetrieval>("METAViewAttributeRetrieval",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAViewAttributeRetrieval();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAViewAttributeRetrievalColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewAttributeRetrievalColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewAttributeRetrievalColumn.WorkspaceTypeID)];
					//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
					entity.VCStatusID = (System.Int32)reader[((int)METAViewAttributeRetrievalColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.Pkid = (System.Int32)reader[((int)METAViewAttributeRetrievalColumn.Pkid)];
					//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAViewAttributeRetrievalColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAViewAttributeRetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewAttributeRetrievalColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Name = (reader.IsDBNull(((int)METAViewAttributeRetrievalColumn.Name)))?null:(System.String)reader[((int)METAViewAttributeRetrievalColumn.Name)];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAViewAttributeRetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAViewAttributeRetrievalColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAViewAttributeRetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAViewAttributeRetrievalColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAViewAttributeRetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAViewAttributeRetrievalColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.DomainType = (reader.IsDBNull(((int)METAViewAttributeRetrievalColumn.DomainType)))?null:(System.String)reader[((int)METAViewAttributeRetrievalColumn.DomainType)];
					//entity.DomainType = (Convert.IsDBNull(reader["DomainType"]))?string.Empty:(System.String)reader["DomainType"];
					entity.DomainDef = (reader.IsDBNull(((int)METAViewAttributeRetrievalColumn.DomainDef)))?null:(System.String)reader[((int)METAViewAttributeRetrievalColumn.DomainDef)];
					//entity.DomainDef = (Convert.IsDBNull(reader["DomainDef"]))?string.Empty:(System.String)reader["DomainDef"];
					entity.Length = (reader.IsDBNull(((int)METAViewAttributeRetrievalColumn.Length)))?null:(System.Int32?)reader[((int)METAViewAttributeRetrievalColumn.Length)];
					//entity.Length = (Convert.IsDBNull(reader["Length"]))?(int)0:(System.Int32?)reader["Length"];
					entity.GapType = (reader.IsDBNull(((int)METAViewAttributeRetrievalColumn.GapType)))?null:(System.String)reader[((int)METAViewAttributeRetrievalColumn.GapType)];
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
		/// Refreshes the <see cref="METAViewAttributeRetrieval"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewAttributeRetrieval"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAViewAttributeRetrieval entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAViewAttributeRetrievalColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewAttributeRetrievalColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewAttributeRetrievalColumn.WorkspaceTypeID)];
			//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
			entity.VCStatusID = (System.Int32)reader[((int)METAViewAttributeRetrievalColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.Pkid = (System.Int32)reader[((int)METAViewAttributeRetrievalColumn.Pkid)];
			//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAViewAttributeRetrievalColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAViewAttributeRetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewAttributeRetrievalColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Name = (reader.IsDBNull(((int)METAViewAttributeRetrievalColumn.Name)))?null:(System.String)reader[((int)METAViewAttributeRetrievalColumn.Name)];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAViewAttributeRetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAViewAttributeRetrievalColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAViewAttributeRetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAViewAttributeRetrievalColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAViewAttributeRetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAViewAttributeRetrievalColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.DomainType = (reader.IsDBNull(((int)METAViewAttributeRetrievalColumn.DomainType)))?null:(System.String)reader[((int)METAViewAttributeRetrievalColumn.DomainType)];
			//entity.DomainType = (Convert.IsDBNull(reader["DomainType"]))?string.Empty:(System.String)reader["DomainType"];
			entity.DomainDef = (reader.IsDBNull(((int)METAViewAttributeRetrievalColumn.DomainDef)))?null:(System.String)reader[((int)METAViewAttributeRetrievalColumn.DomainDef)];
			//entity.DomainDef = (Convert.IsDBNull(reader["DomainDef"]))?string.Empty:(System.String)reader["DomainDef"];
			entity.Length = (reader.IsDBNull(((int)METAViewAttributeRetrievalColumn.Length)))?null:(System.Int32?)reader[((int)METAViewAttributeRetrievalColumn.Length)];
			//entity.Length = (Convert.IsDBNull(reader["Length"]))?(int)0:(System.Int32?)reader["Length"];
			entity.GapType = (reader.IsDBNull(((int)METAViewAttributeRetrievalColumn.GapType)))?null:(System.String)reader[((int)METAViewAttributeRetrievalColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAViewAttributeRetrieval"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewAttributeRetrieval"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAViewAttributeRetrieval entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeID = (Convert.IsDBNull(dataRow["WorkspaceTypeID"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeID"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.Pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.Name = (Convert.IsDBNull(dataRow["Name"]))?string.Empty:(System.String)dataRow["Name"];
			entity.CustomField1 = (Convert.IsDBNull(dataRow["CustomField1"]))?string.Empty:(System.String)dataRow["CustomField1"];
			entity.CustomField2 = (Convert.IsDBNull(dataRow["CustomField2"]))?string.Empty:(System.String)dataRow["CustomField2"];
			entity.CustomField3 = (Convert.IsDBNull(dataRow["CustomField3"]))?string.Empty:(System.String)dataRow["CustomField3"];
			entity.DomainType = (Convert.IsDBNull(dataRow["DomainType"]))?string.Empty:(System.String)dataRow["DomainType"];
			entity.DomainDef = (Convert.IsDBNull(dataRow["DomainDef"]))?string.Empty:(System.String)dataRow["DomainDef"];
			entity.Length = (Convert.IsDBNull(dataRow["Length"]))?(int)0:(System.Int32?)dataRow["Length"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAViewAttributeRetrievalFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewAttributeRetrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewAttributeRetrievalFilterBuilder : SqlFilterBuilder<METAViewAttributeRetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewAttributeRetrievalFilterBuilder class.
		/// </summary>
		public METAViewAttributeRetrievalFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewAttributeRetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewAttributeRetrievalFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewAttributeRetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewAttributeRetrievalFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewAttributeRetrievalFilterBuilder

	#region METAViewAttributeRetrievalParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewAttributeRetrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewAttributeRetrievalParameterBuilder : ParameterizedSqlFilterBuilder<METAViewAttributeRetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewAttributeRetrievalParameterBuilder class.
		/// </summary>
		public METAViewAttributeRetrievalParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewAttributeRetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewAttributeRetrievalParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewAttributeRetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewAttributeRetrievalParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewAttributeRetrievalParameterBuilder
	
	#region METAViewAttributeRetrievalSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewAttributeRetrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAViewAttributeRetrievalSortBuilder : SqlSortBuilder<METAViewAttributeRetrievalColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewAttributeRetrievalSqlSortBuilder class.
		/// </summary>
		public METAViewAttributeRetrievalSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAViewAttributeRetrievalSortBuilder

} // end namespace
