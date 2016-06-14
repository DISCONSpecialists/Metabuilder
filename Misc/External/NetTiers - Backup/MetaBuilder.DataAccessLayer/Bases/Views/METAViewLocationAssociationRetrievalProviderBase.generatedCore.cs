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
	/// This class is the base class for any <see cref="METAViewLocationAssociationRetrievalProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAViewLocationAssociationRetrievalProviderBaseCore : EntityViewProviderBase<METAViewLocationAssociationRetrieval>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAViewLocationAssociationRetrieval&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAViewLocationAssociationRetrieval&gt;"/></returns>
		protected static VList&lt;METAViewLocationAssociationRetrieval&gt; Fill(DataSet dataSet, VList<METAViewLocationAssociationRetrieval> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAViewLocationAssociationRetrieval>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAViewLocationAssociationRetrieval&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAViewLocationAssociationRetrieval>"/></returns>
		protected static VList&lt;METAViewLocationAssociationRetrieval&gt; Fill(DataTable dataTable, VList<METAViewLocationAssociationRetrieval> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAViewLocationAssociationRetrieval c = new METAViewLocationAssociationRetrieval();
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
		/// Fill an <see cref="VList&lt;METAViewLocationAssociationRetrieval&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAViewLocationAssociationRetrieval&gt;"/></returns>
		protected VList<METAViewLocationAssociationRetrieval> Fill(IDataReader reader, VList<METAViewLocationAssociationRetrieval> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAViewLocationAssociationRetrieval entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAViewLocationAssociationRetrieval>("METAViewLocationAssociationRetrieval",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAViewLocationAssociationRetrieval();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAViewLocationAssociationRetrievalColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewLocationAssociationRetrievalColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewLocationAssociationRetrievalColumn.WorkspaceTypeID)];
					//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
					entity.VCStatusID = (System.Int32)reader[((int)METAViewLocationAssociationRetrievalColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.Pkid = (System.Int32)reader[((int)METAViewLocationAssociationRetrievalColumn.Pkid)];
					//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAViewLocationAssociationRetrievalColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAViewLocationAssociationRetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewLocationAssociationRetrievalColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Distance = (reader.IsDBNull(((int)METAViewLocationAssociationRetrievalColumn.Distance)))?null:(System.String)reader[((int)METAViewLocationAssociationRetrievalColumn.Distance)];
					//entity.Distance = (Convert.IsDBNull(reader["Distance"]))?string.Empty:(System.String)reader["Distance"];
					entity.TimeIndicator = (reader.IsDBNull(((int)METAViewLocationAssociationRetrievalColumn.TimeIndicator)))?null:(System.String)reader[((int)METAViewLocationAssociationRetrievalColumn.TimeIndicator)];
					//entity.TimeIndicator = (Convert.IsDBNull(reader["TimeIndicator"]))?string.Empty:(System.String)reader["TimeIndicator"];
					entity.AssociationType = (reader.IsDBNull(((int)METAViewLocationAssociationRetrievalColumn.AssociationType)))?null:(System.String)reader[((int)METAViewLocationAssociationRetrievalColumn.AssociationType)];
					//entity.AssociationType = (Convert.IsDBNull(reader["AssociationType"]))?string.Empty:(System.String)reader["AssociationType"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAViewLocationAssociationRetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAViewLocationAssociationRetrievalColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAViewLocationAssociationRetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAViewLocationAssociationRetrievalColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAViewLocationAssociationRetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAViewLocationAssociationRetrievalColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.GapType = (reader.IsDBNull(((int)METAViewLocationAssociationRetrievalColumn.GapType)))?null:(System.String)reader[((int)METAViewLocationAssociationRetrievalColumn.GapType)];
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
		/// Refreshes the <see cref="METAViewLocationAssociationRetrieval"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewLocationAssociationRetrieval"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAViewLocationAssociationRetrieval entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAViewLocationAssociationRetrievalColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeID = (reader.IsDBNull(((int)METAViewLocationAssociationRetrievalColumn.WorkspaceTypeID)))?null:(System.Int32?)reader[((int)METAViewLocationAssociationRetrievalColumn.WorkspaceTypeID)];
			//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
			entity.VCStatusID = (System.Int32)reader[((int)METAViewLocationAssociationRetrievalColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.Pkid = (System.Int32)reader[((int)METAViewLocationAssociationRetrievalColumn.Pkid)];
			//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAViewLocationAssociationRetrievalColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAViewLocationAssociationRetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAViewLocationAssociationRetrievalColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Distance = (reader.IsDBNull(((int)METAViewLocationAssociationRetrievalColumn.Distance)))?null:(System.String)reader[((int)METAViewLocationAssociationRetrievalColumn.Distance)];
			//entity.Distance = (Convert.IsDBNull(reader["Distance"]))?string.Empty:(System.String)reader["Distance"];
			entity.TimeIndicator = (reader.IsDBNull(((int)METAViewLocationAssociationRetrievalColumn.TimeIndicator)))?null:(System.String)reader[((int)METAViewLocationAssociationRetrievalColumn.TimeIndicator)];
			//entity.TimeIndicator = (Convert.IsDBNull(reader["TimeIndicator"]))?string.Empty:(System.String)reader["TimeIndicator"];
			entity.AssociationType = (reader.IsDBNull(((int)METAViewLocationAssociationRetrievalColumn.AssociationType)))?null:(System.String)reader[((int)METAViewLocationAssociationRetrievalColumn.AssociationType)];
			//entity.AssociationType = (Convert.IsDBNull(reader["AssociationType"]))?string.Empty:(System.String)reader["AssociationType"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAViewLocationAssociationRetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAViewLocationAssociationRetrievalColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAViewLocationAssociationRetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAViewLocationAssociationRetrievalColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAViewLocationAssociationRetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAViewLocationAssociationRetrievalColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.GapType = (reader.IsDBNull(((int)METAViewLocationAssociationRetrievalColumn.GapType)))?null:(System.String)reader[((int)METAViewLocationAssociationRetrievalColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAViewLocationAssociationRetrieval"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewLocationAssociationRetrieval"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAViewLocationAssociationRetrieval entity)
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

	#region METAViewLocationAssociationRetrievalFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewLocationAssociationRetrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewLocationAssociationRetrievalFilterBuilder : SqlFilterBuilder<METAViewLocationAssociationRetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewLocationAssociationRetrievalFilterBuilder class.
		/// </summary>
		public METAViewLocationAssociationRetrievalFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewLocationAssociationRetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewLocationAssociationRetrievalFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewLocationAssociationRetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewLocationAssociationRetrievalFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewLocationAssociationRetrievalFilterBuilder

	#region METAViewLocationAssociationRetrievalParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewLocationAssociationRetrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewLocationAssociationRetrievalParameterBuilder : ParameterizedSqlFilterBuilder<METAViewLocationAssociationRetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewLocationAssociationRetrievalParameterBuilder class.
		/// </summary>
		public METAViewLocationAssociationRetrievalParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewLocationAssociationRetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewLocationAssociationRetrievalParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewLocationAssociationRetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewLocationAssociationRetrievalParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewLocationAssociationRetrievalParameterBuilder
	
	#region METAViewLocationAssociationRetrievalSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewLocationAssociationRetrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAViewLocationAssociationRetrievalSortBuilder : SqlSortBuilder<METAViewLocationAssociationRetrievalColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewLocationAssociationRetrievalSqlSortBuilder class.
		/// </summary>
		public METAViewLocationAssociationRetrievalSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAViewLocationAssociationRetrievalSortBuilder

} // end namespace
