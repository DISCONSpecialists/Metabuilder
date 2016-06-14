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
	/// This class is the base class for any <see cref="METAViewLocationRetrievalProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAViewLocationRetrievalProviderBaseCore : EntityViewProviderBase<METAViewLocationRetrieval>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAViewLocationRetrieval&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAViewLocationRetrieval&gt;"/></returns>
		protected static VList&lt;METAViewLocationRetrieval&gt; Fill(DataSet dataSet, VList<METAViewLocationRetrieval> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAViewLocationRetrieval>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAViewLocationRetrieval&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAViewLocationRetrieval>"/></returns>
		protected static VList&lt;METAViewLocationRetrieval&gt; Fill(DataTable dataTable, VList<METAViewLocationRetrieval> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAViewLocationRetrieval c = new METAViewLocationRetrieval();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeID = (Convert.IsDBNull(row["WorkspaceTypeID"]))?(int)0:(System.Int32?)row["WorkspaceTypeID"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.Pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.Name = (Convert.IsDBNull(row["Name"]))?string.Empty:(System.String)row["Name"];
					c.LocationType = (Convert.IsDBNull(row["LocationType"]))?string.Empty:(System.String)row["LocationType"];
					c.CustomField1 = (Convert.IsDBNull(row["CustomField1"]))?string.Empty:(System.String)row["CustomField1"];
					c.CustomField2 = (Convert.IsDBNull(row["CustomField2"]))?string.Empty:(System.String)row["CustomField2"];
					c.CustomField3 = (Convert.IsDBNull(row["CustomField3"]))?string.Empty:(System.String)row["CustomField3"];
					c.Address = (Convert.IsDBNull(row["Address"]))?string.Empty:(System.String)row["Address"];
					c.Telephone = (Convert.IsDBNull(row["Telephone"]))?string.Empty:(System.String)row["Telephone"];
					c.Fax = (Convert.IsDBNull(row["Fax"]))?string.Empty:(System.String)row["Fax"];
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
		/// Fill an <see cref="VList&lt;METAViewLocationRetrieval&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAViewLocationRetrieval&gt;"/></returns>
		protected VList<METAViewLocationRetrieval> Fill(IDataReader reader, VList<METAViewLocationRetrieval> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAViewLocationRetrieval entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAViewLocationRetrieval>("METAViewLocationRetrieval",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAViewLocationRetrieval();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAViewLocationRetrievalColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewLocationRetrievalColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewLocationRetrievalColumn.WorkspaceTypeID)];
					//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
					entity.VCStatusID = (System.Int32)reader[((int)METAViewLocationRetrievalColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.Pkid = (System.Int32)reader[((int)METAViewLocationRetrievalColumn.Pkid)];
					//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAViewLocationRetrievalColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAViewLocationRetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewLocationRetrievalColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Name = (reader.IsDBNull(((int)METAViewLocationRetrievalColumn.Name)))?null:(System.String)reader[((int)METAViewLocationRetrievalColumn.Name)];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.LocationType = (reader.IsDBNull(((int)METAViewLocationRetrievalColumn.LocationType)))?null:(System.String)reader[((int)METAViewLocationRetrievalColumn.LocationType)];
					//entity.LocationType = (Convert.IsDBNull(reader["LocationType"]))?string.Empty:(System.String)reader["LocationType"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAViewLocationRetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAViewLocationRetrievalColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAViewLocationRetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAViewLocationRetrievalColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAViewLocationRetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAViewLocationRetrievalColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.Address = (reader.IsDBNull(((int)METAViewLocationRetrievalColumn.Address)))?null:(System.String)reader[((int)METAViewLocationRetrievalColumn.Address)];
					//entity.Address = (Convert.IsDBNull(reader["Address"]))?string.Empty:(System.String)reader["Address"];
					entity.Telephone = (reader.IsDBNull(((int)METAViewLocationRetrievalColumn.Telephone)))?null:(System.String)reader[((int)METAViewLocationRetrievalColumn.Telephone)];
					//entity.Telephone = (Convert.IsDBNull(reader["Telephone"]))?string.Empty:(System.String)reader["Telephone"];
					entity.Fax = (reader.IsDBNull(((int)METAViewLocationRetrievalColumn.Fax)))?null:(System.String)reader[((int)METAViewLocationRetrievalColumn.Fax)];
					//entity.Fax = (Convert.IsDBNull(reader["Fax"]))?string.Empty:(System.String)reader["Fax"];
					entity.GapType = (reader.IsDBNull(((int)METAViewLocationRetrievalColumn.GapType)))?null:(System.String)reader[((int)METAViewLocationRetrievalColumn.GapType)];
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
		/// Refreshes the <see cref="METAViewLocationRetrieval"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewLocationRetrieval"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAViewLocationRetrieval entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAViewLocationRetrievalColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewLocationRetrievalColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewLocationRetrievalColumn.WorkspaceTypeID)];
			//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
			entity.VCStatusID = (System.Int32)reader[((int)METAViewLocationRetrievalColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.Pkid = (System.Int32)reader[((int)METAViewLocationRetrievalColumn.Pkid)];
			//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAViewLocationRetrievalColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAViewLocationRetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewLocationRetrievalColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Name = (reader.IsDBNull(((int)METAViewLocationRetrievalColumn.Name)))?null:(System.String)reader[((int)METAViewLocationRetrievalColumn.Name)];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.LocationType = (reader.IsDBNull(((int)METAViewLocationRetrievalColumn.LocationType)))?null:(System.String)reader[((int)METAViewLocationRetrievalColumn.LocationType)];
			//entity.LocationType = (Convert.IsDBNull(reader["LocationType"]))?string.Empty:(System.String)reader["LocationType"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAViewLocationRetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAViewLocationRetrievalColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAViewLocationRetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAViewLocationRetrievalColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAViewLocationRetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAViewLocationRetrievalColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.Address = (reader.IsDBNull(((int)METAViewLocationRetrievalColumn.Address)))?null:(System.String)reader[((int)METAViewLocationRetrievalColumn.Address)];
			//entity.Address = (Convert.IsDBNull(reader["Address"]))?string.Empty:(System.String)reader["Address"];
			entity.Telephone = (reader.IsDBNull(((int)METAViewLocationRetrievalColumn.Telephone)))?null:(System.String)reader[((int)METAViewLocationRetrievalColumn.Telephone)];
			//entity.Telephone = (Convert.IsDBNull(reader["Telephone"]))?string.Empty:(System.String)reader["Telephone"];
			entity.Fax = (reader.IsDBNull(((int)METAViewLocationRetrievalColumn.Fax)))?null:(System.String)reader[((int)METAViewLocationRetrievalColumn.Fax)];
			//entity.Fax = (Convert.IsDBNull(reader["Fax"]))?string.Empty:(System.String)reader["Fax"];
			entity.GapType = (reader.IsDBNull(((int)METAViewLocationRetrievalColumn.GapType)))?null:(System.String)reader[((int)METAViewLocationRetrievalColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAViewLocationRetrieval"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewLocationRetrieval"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAViewLocationRetrieval entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeID = (Convert.IsDBNull(dataRow["WorkspaceTypeID"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeID"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.Pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.Name = (Convert.IsDBNull(dataRow["Name"]))?string.Empty:(System.String)dataRow["Name"];
			entity.LocationType = (Convert.IsDBNull(dataRow["LocationType"]))?string.Empty:(System.String)dataRow["LocationType"];
			entity.CustomField1 = (Convert.IsDBNull(dataRow["CustomField1"]))?string.Empty:(System.String)dataRow["CustomField1"];
			entity.CustomField2 = (Convert.IsDBNull(dataRow["CustomField2"]))?string.Empty:(System.String)dataRow["CustomField2"];
			entity.CustomField3 = (Convert.IsDBNull(dataRow["CustomField3"]))?string.Empty:(System.String)dataRow["CustomField3"];
			entity.Address = (Convert.IsDBNull(dataRow["Address"]))?string.Empty:(System.String)dataRow["Address"];
			entity.Telephone = (Convert.IsDBNull(dataRow["Telephone"]))?string.Empty:(System.String)dataRow["Telephone"];
			entity.Fax = (Convert.IsDBNull(dataRow["Fax"]))?string.Empty:(System.String)dataRow["Fax"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAViewLocationRetrievalFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewLocationRetrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewLocationRetrievalFilterBuilder : SqlFilterBuilder<METAViewLocationRetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewLocationRetrievalFilterBuilder class.
		/// </summary>
		public METAViewLocationRetrievalFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewLocationRetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewLocationRetrievalFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewLocationRetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewLocationRetrievalFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewLocationRetrievalFilterBuilder

	#region METAViewLocationRetrievalParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewLocationRetrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewLocationRetrievalParameterBuilder : ParameterizedSqlFilterBuilder<METAViewLocationRetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewLocationRetrievalParameterBuilder class.
		/// </summary>
		public METAViewLocationRetrievalParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewLocationRetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewLocationRetrievalParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewLocationRetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewLocationRetrievalParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewLocationRetrievalParameterBuilder
	
	#region METAViewLocationRetrievalSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewLocationRetrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAViewLocationRetrievalSortBuilder : SqlSortBuilder<METAViewLocationRetrievalColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewLocationRetrievalSqlSortBuilder class.
		/// </summary>
		public METAViewLocationRetrievalSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAViewLocationRetrievalSortBuilder

} // end namespace
