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
	/// This class is the base class for any <see cref="METAView_DataColumn_RetrievalProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAView_DataColumn_RetrievalProviderBaseCore : EntityViewProviderBase<METAView_DataColumn_Retrieval>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAView_DataColumn_Retrieval&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAView_DataColumn_Retrieval&gt;"/></returns>
		protected static VList&lt;METAView_DataColumn_Retrieval&gt; Fill(DataSet dataSet, VList<METAView_DataColumn_Retrieval> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAView_DataColumn_Retrieval>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAView_DataColumn_Retrieval&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAView_DataColumn_Retrieval>"/></returns>
		protected static VList&lt;METAView_DataColumn_Retrieval&gt; Fill(DataTable dataTable, VList<METAView_DataColumn_Retrieval> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAView_DataColumn_Retrieval c = new METAView_DataColumn_Retrieval();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeId = (Convert.IsDBNull(row["WorkspaceTypeId"]))?(int)0:(System.Int32?)row["WorkspaceTypeId"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.Name = (Convert.IsDBNull(row["Name"]))?string.Empty:(System.String)row["Name"];
					c.DomainDef = (Convert.IsDBNull(row["DomainDef"]))?string.Empty:(System.String)row["DomainDef"];
					c.CustomField1 = (Convert.IsDBNull(row["CustomField1"]))?string.Empty:(System.String)row["CustomField1"];
					c.CustomField2 = (Convert.IsDBNull(row["CustomField2"]))?string.Empty:(System.String)row["CustomField2"];
					c.CustomField3 = (Convert.IsDBNull(row["CustomField3"]))?string.Empty:(System.String)row["CustomField3"];
					c.ColumnType = (Convert.IsDBNull(row["ColumnType"]))?string.Empty:(System.String)row["ColumnType"];
					c.ColumnLength = (Convert.IsDBNull(row["ColumnLength"]))?(int)0:(System.Int32?)row["ColumnLength"];
					c.GapType = (Convert.IsDBNull(row["GapType"]))?string.Empty:(System.String)row["GapType"];
					c.DataType = (Convert.IsDBNull(row["DataType"]))?string.Empty:(System.String)row["DataType"];
					c.Description = (Convert.IsDBNull(row["Description"]))?string.Empty:(System.String)row["Description"];
					c.Abbreviation = (Convert.IsDBNull(row["Abbreviation"]))?string.Empty:(System.String)row["Abbreviation"];
					c.IsBusinessExternal = (Convert.IsDBNull(row["IsBusinessExternal"]))?string.Empty:(System.String)row["IsBusinessExternal"];
					c.StandardisationStatus = (Convert.IsDBNull(row["StandardisationStatus"]))?string.Empty:(System.String)row["StandardisationStatus"];
					c.StandardisationStatusDate = (Convert.IsDBNull(row["StandardisationStatusDate"]))?string.Empty:(System.String)row["StandardisationStatusDate"];
					c.DataSourceID = (Convert.IsDBNull(row["DataSourceID"]))?string.Empty:(System.String)row["DataSourceID"];
					c.DataSourceName = (Convert.IsDBNull(row["DataSourceName"]))?string.Empty:(System.String)row["DataSourceName"];
					c.GeneralRemarks = (Convert.IsDBNull(row["GeneralRemarks"]))?string.Empty:(System.String)row["GeneralRemarks"];
					c.NumberofValues = (Convert.IsDBNull(row["NumberofValues"]))?string.Empty:(System.String)row["NumberofValues"];
					c.NullValuePercentage = (Convert.IsDBNull(row["NullValuePercentage"]))?string.Empty:(System.String)row["NullValuePercentage"];
					c.ConsistencyPercentage = (Convert.IsDBNull(row["ConsistencyPercentage"]))?string.Empty:(System.String)row["ConsistencyPercentage"];
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
		/// Fill an <see cref="VList&lt;METAView_DataColumn_Retrieval&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAView_DataColumn_Retrieval&gt;"/></returns>
		protected VList<METAView_DataColumn_Retrieval> Fill(IDataReader reader, VList<METAView_DataColumn_Retrieval> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAView_DataColumn_Retrieval entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAView_DataColumn_Retrieval>("METAView_DataColumn_Retrieval",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAView_DataColumn_Retrieval();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAView_DataColumn_RetrievalColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_DataColumn_RetrievalColumn.WorkspaceTypeId)];
					//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
					entity.VCStatusID = (System.Int32)reader[((int)METAView_DataColumn_RetrievalColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.pkid = (System.Int32)reader[((int)METAView_DataColumn_RetrievalColumn.pkid)];
					//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAView_DataColumn_RetrievalColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Name = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.Name)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.Name)];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.DomainDef = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.DomainDef)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.DomainDef)];
					//entity.DomainDef = (Convert.IsDBNull(reader["DomainDef"]))?string.Empty:(System.String)reader["DomainDef"];
					entity.CustomField1 = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.CustomField1)];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.CustomField2)];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.CustomField3)];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.ColumnType = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.ColumnType)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.ColumnType)];
					//entity.ColumnType = (Convert.IsDBNull(reader["ColumnType"]))?string.Empty:(System.String)reader["ColumnType"];
					entity.ColumnLength = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.ColumnLength)))?null:(System.Int32?)reader[((int)METAView_DataColumn_RetrievalColumn.ColumnLength)];
					//entity.ColumnLength = (Convert.IsDBNull(reader["ColumnLength"]))?(int)0:(System.Int32?)reader["ColumnLength"];
					entity.GapType = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.GapType)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.GapType)];
					//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
					entity.DataType = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.DataType)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.DataType)];
					//entity.DataType = (Convert.IsDBNull(reader["DataType"]))?string.Empty:(System.String)reader["DataType"];
					entity.Description = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.Description)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.Description)];
					//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
					entity.Abbreviation = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.Abbreviation)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.Abbreviation)];
					//entity.Abbreviation = (Convert.IsDBNull(reader["Abbreviation"]))?string.Empty:(System.String)reader["Abbreviation"];
					entity.IsBusinessExternal = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.IsBusinessExternal)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.IsBusinessExternal)];
					//entity.IsBusinessExternal = (Convert.IsDBNull(reader["IsBusinessExternal"]))?string.Empty:(System.String)reader["IsBusinessExternal"];
					entity.StandardisationStatus = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.StandardisationStatus)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.StandardisationStatus)];
					//entity.StandardisationStatus = (Convert.IsDBNull(reader["StandardisationStatus"]))?string.Empty:(System.String)reader["StandardisationStatus"];
					entity.StandardisationStatusDate = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.StandardisationStatusDate)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.StandardisationStatusDate)];
					//entity.StandardisationStatusDate = (Convert.IsDBNull(reader["StandardisationStatusDate"]))?string.Empty:(System.String)reader["StandardisationStatusDate"];
					entity.DataSourceID = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.DataSourceID)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.DataSourceID)];
					//entity.DataSourceID = (Convert.IsDBNull(reader["DataSourceID"]))?string.Empty:(System.String)reader["DataSourceID"];
					entity.DataSourceName = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.DataSourceName)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.DataSourceName)];
					//entity.DataSourceName = (Convert.IsDBNull(reader["DataSourceName"]))?string.Empty:(System.String)reader["DataSourceName"];
					entity.GeneralRemarks = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.GeneralRemarks)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.GeneralRemarks)];
					//entity.GeneralRemarks = (Convert.IsDBNull(reader["GeneralRemarks"]))?string.Empty:(System.String)reader["GeneralRemarks"];
					entity.NumberofValues = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.NumberofValues)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.NumberofValues)];
					//entity.NumberofValues = (Convert.IsDBNull(reader["NumberofValues"]))?string.Empty:(System.String)reader["NumberofValues"];
					entity.NullValuePercentage = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.NullValuePercentage)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.NullValuePercentage)];
					//entity.NullValuePercentage = (Convert.IsDBNull(reader["NullValuePercentage"]))?string.Empty:(System.String)reader["NullValuePercentage"];
					entity.ConsistencyPercentage = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.ConsistencyPercentage)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.ConsistencyPercentage)];
					//entity.ConsistencyPercentage = (Convert.IsDBNull(reader["ConsistencyPercentage"]))?string.Empty:(System.String)reader["ConsistencyPercentage"];
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
		/// Refreshes the <see cref="METAView_DataColumn_Retrieval"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_DataColumn_Retrieval"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAView_DataColumn_Retrieval entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAView_DataColumn_RetrievalColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_DataColumn_RetrievalColumn.WorkspaceTypeId)];
			//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
			entity.VCStatusID = (System.Int32)reader[((int)METAView_DataColumn_RetrievalColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.pkid = (System.Int32)reader[((int)METAView_DataColumn_RetrievalColumn.pkid)];
			//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAView_DataColumn_RetrievalColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Name = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.Name)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.Name)];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.DomainDef = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.DomainDef)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.DomainDef)];
			//entity.DomainDef = (Convert.IsDBNull(reader["DomainDef"]))?string.Empty:(System.String)reader["DomainDef"];
			entity.CustomField1 = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.CustomField1)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.CustomField1)];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.CustomField2)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.CustomField2)];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.CustomField3)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.CustomField3)];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			entity.ColumnType = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.ColumnType)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.ColumnType)];
			//entity.ColumnType = (Convert.IsDBNull(reader["ColumnType"]))?string.Empty:(System.String)reader["ColumnType"];
			entity.ColumnLength = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.ColumnLength)))?null:(System.Int32?)reader[((int)METAView_DataColumn_RetrievalColumn.ColumnLength)];
			//entity.ColumnLength = (Convert.IsDBNull(reader["ColumnLength"]))?(int)0:(System.Int32?)reader["ColumnLength"];
			entity.GapType = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.GapType)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			entity.DataType = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.DataType)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.DataType)];
			//entity.DataType = (Convert.IsDBNull(reader["DataType"]))?string.Empty:(System.String)reader["DataType"];
			entity.Description = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.Description)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.Description)];
			//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
			entity.Abbreviation = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.Abbreviation)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.Abbreviation)];
			//entity.Abbreviation = (Convert.IsDBNull(reader["Abbreviation"]))?string.Empty:(System.String)reader["Abbreviation"];
			entity.IsBusinessExternal = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.IsBusinessExternal)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.IsBusinessExternal)];
			//entity.IsBusinessExternal = (Convert.IsDBNull(reader["IsBusinessExternal"]))?string.Empty:(System.String)reader["IsBusinessExternal"];
			entity.StandardisationStatus = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.StandardisationStatus)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.StandardisationStatus)];
			//entity.StandardisationStatus = (Convert.IsDBNull(reader["StandardisationStatus"]))?string.Empty:(System.String)reader["StandardisationStatus"];
			entity.StandardisationStatusDate = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.StandardisationStatusDate)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.StandardisationStatusDate)];
			//entity.StandardisationStatusDate = (Convert.IsDBNull(reader["StandardisationStatusDate"]))?string.Empty:(System.String)reader["StandardisationStatusDate"];
			entity.DataSourceID = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.DataSourceID)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.DataSourceID)];
			//entity.DataSourceID = (Convert.IsDBNull(reader["DataSourceID"]))?string.Empty:(System.String)reader["DataSourceID"];
			entity.DataSourceName = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.DataSourceName)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.DataSourceName)];
			//entity.DataSourceName = (Convert.IsDBNull(reader["DataSourceName"]))?string.Empty:(System.String)reader["DataSourceName"];
			entity.GeneralRemarks = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.GeneralRemarks)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.GeneralRemarks)];
			//entity.GeneralRemarks = (Convert.IsDBNull(reader["GeneralRemarks"]))?string.Empty:(System.String)reader["GeneralRemarks"];
			entity.NumberofValues = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.NumberofValues)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.NumberofValues)];
			//entity.NumberofValues = (Convert.IsDBNull(reader["NumberofValues"]))?string.Empty:(System.String)reader["NumberofValues"];
			entity.NullValuePercentage = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.NullValuePercentage)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.NullValuePercentage)];
			//entity.NullValuePercentage = (Convert.IsDBNull(reader["NullValuePercentage"]))?string.Empty:(System.String)reader["NullValuePercentage"];
			entity.ConsistencyPercentage = (reader.IsDBNull(((int)METAView_DataColumn_RetrievalColumn.ConsistencyPercentage)))?null:(System.String)reader[((int)METAView_DataColumn_RetrievalColumn.ConsistencyPercentage)];
			//entity.ConsistencyPercentage = (Convert.IsDBNull(reader["ConsistencyPercentage"]))?string.Empty:(System.String)reader["ConsistencyPercentage"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAView_DataColumn_Retrieval"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_DataColumn_Retrieval"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAView_DataColumn_Retrieval entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeId = (Convert.IsDBNull(dataRow["WorkspaceTypeId"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeId"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.Name = (Convert.IsDBNull(dataRow["Name"]))?string.Empty:(System.String)dataRow["Name"];
			entity.DomainDef = (Convert.IsDBNull(dataRow["DomainDef"]))?string.Empty:(System.String)dataRow["DomainDef"];
			entity.CustomField1 = (Convert.IsDBNull(dataRow["CustomField1"]))?string.Empty:(System.String)dataRow["CustomField1"];
			entity.CustomField2 = (Convert.IsDBNull(dataRow["CustomField2"]))?string.Empty:(System.String)dataRow["CustomField2"];
			entity.CustomField3 = (Convert.IsDBNull(dataRow["CustomField3"]))?string.Empty:(System.String)dataRow["CustomField3"];
			entity.ColumnType = (Convert.IsDBNull(dataRow["ColumnType"]))?string.Empty:(System.String)dataRow["ColumnType"];
			entity.ColumnLength = (Convert.IsDBNull(dataRow["ColumnLength"]))?(int)0:(System.Int32?)dataRow["ColumnLength"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.DataType = (Convert.IsDBNull(dataRow["DataType"]))?string.Empty:(System.String)dataRow["DataType"];
			entity.Description = (Convert.IsDBNull(dataRow["Description"]))?string.Empty:(System.String)dataRow["Description"];
			entity.Abbreviation = (Convert.IsDBNull(dataRow["Abbreviation"]))?string.Empty:(System.String)dataRow["Abbreviation"];
			entity.IsBusinessExternal = (Convert.IsDBNull(dataRow["IsBusinessExternal"]))?string.Empty:(System.String)dataRow["IsBusinessExternal"];
			entity.StandardisationStatus = (Convert.IsDBNull(dataRow["StandardisationStatus"]))?string.Empty:(System.String)dataRow["StandardisationStatus"];
			entity.StandardisationStatusDate = (Convert.IsDBNull(dataRow["StandardisationStatusDate"]))?string.Empty:(System.String)dataRow["StandardisationStatusDate"];
			entity.DataSourceID = (Convert.IsDBNull(dataRow["DataSourceID"]))?string.Empty:(System.String)dataRow["DataSourceID"];
			entity.DataSourceName = (Convert.IsDBNull(dataRow["DataSourceName"]))?string.Empty:(System.String)dataRow["DataSourceName"];
			entity.GeneralRemarks = (Convert.IsDBNull(dataRow["GeneralRemarks"]))?string.Empty:(System.String)dataRow["GeneralRemarks"];
			entity.NumberofValues = (Convert.IsDBNull(dataRow["NumberofValues"]))?string.Empty:(System.String)dataRow["NumberofValues"];
			entity.NullValuePercentage = (Convert.IsDBNull(dataRow["NullValuePercentage"]))?string.Empty:(System.String)dataRow["NullValuePercentage"];
			entity.ConsistencyPercentage = (Convert.IsDBNull(dataRow["ConsistencyPercentage"]))?string.Empty:(System.String)dataRow["ConsistencyPercentage"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAView_DataColumn_RetrievalFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_DataColumn_Retrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_DataColumn_RetrievalFilterBuilder : SqlFilterBuilder<METAView_DataColumn_RetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_DataColumn_RetrievalFilterBuilder class.
		/// </summary>
		public METAView_DataColumn_RetrievalFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_DataColumn_RetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_DataColumn_RetrievalFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_DataColumn_RetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_DataColumn_RetrievalFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_DataColumn_RetrievalFilterBuilder

	#region METAView_DataColumn_RetrievalParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_DataColumn_Retrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_DataColumn_RetrievalParameterBuilder : ParameterizedSqlFilterBuilder<METAView_DataColumn_RetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_DataColumn_RetrievalParameterBuilder class.
		/// </summary>
		public METAView_DataColumn_RetrievalParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_DataColumn_RetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_DataColumn_RetrievalParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_DataColumn_RetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_DataColumn_RetrievalParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_DataColumn_RetrievalParameterBuilder
	
	#region METAView_DataColumn_RetrievalSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_DataColumn_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataColumn_RetrievalSortBuilder : SqlSortBuilder<METAView_DataColumn_RetrievalColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_DataColumn_RetrievalSqlSortBuilder class.
		/// </summary>
		public METAView_DataColumn_RetrievalSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAView_DataColumn_RetrievalSortBuilder

} // end namespace
