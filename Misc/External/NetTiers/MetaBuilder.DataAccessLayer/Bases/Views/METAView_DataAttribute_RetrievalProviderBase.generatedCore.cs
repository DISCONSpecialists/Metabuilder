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
	/// This class is the base class for any <see cref="METAView_DataAttribute_RetrievalProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAView_DataAttribute_RetrievalProviderBaseCore : EntityViewProviderBase<METAView_DataAttribute_Retrieval>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAView_DataAttribute_Retrieval&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAView_DataAttribute_Retrieval&gt;"/></returns>
		protected static VList&lt;METAView_DataAttribute_Retrieval&gt; Fill(DataSet dataSet, VList<METAView_DataAttribute_Retrieval> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAView_DataAttribute_Retrieval>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAView_DataAttribute_Retrieval&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAView_DataAttribute_Retrieval>"/></returns>
		protected static VList&lt;METAView_DataAttribute_Retrieval&gt; Fill(DataTable dataTable, VList<METAView_DataAttribute_Retrieval> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAView_DataAttribute_Retrieval c = new METAView_DataAttribute_Retrieval();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeId = (Convert.IsDBNull(row["WorkspaceTypeId"]))?(int)0:(System.Int32?)row["WorkspaceTypeId"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.Name = (Convert.IsDBNull(row["Name"]))?string.Empty:(System.String)row["Name"];
					c.Abbreviation = (Convert.IsDBNull(row["Abbreviation"]))?string.Empty:(System.String)row["Abbreviation"];
					c.Description = (Convert.IsDBNull(row["Description"]))?string.Empty:(System.String)row["Description"];
					c.IsBusinessExternal = (Convert.IsDBNull(row["IsBusinessExternal"]))?string.Empty:(System.String)row["IsBusinessExternal"];
					c.ArchitectureStatus = (Convert.IsDBNull(row["ArchitectureStatus"]))?string.Empty:(System.String)row["ArchitectureStatus"];
					c.ArchitectureStatusDate = (Convert.IsDBNull(row["ArchitectureStatusDate"]))?string.Empty:(System.String)row["ArchitectureStatusDate"];
					c.DesignRationale = (Convert.IsDBNull(row["DesignRationale"]))?string.Empty:(System.String)row["DesignRationale"];
					c.GeneralRemarks = (Convert.IsDBNull(row["GeneralRemarks"]))?string.Empty:(System.String)row["GeneralRemarks"];
					c.GapType = (Convert.IsDBNull(row["GapType"]))?string.Empty:(System.String)row["GapType"];
					c.DataSourceID = (Convert.IsDBNull(row["DataSourceID"]))?string.Empty:(System.String)row["DataSourceID"];
					c.DataSourceName = (Convert.IsDBNull(row["DataSourceName"]))?string.Empty:(System.String)row["DataSourceName"];
					c.DataType = (Convert.IsDBNull(row["DataType"]))?string.Empty:(System.String)row["DataType"];
					c.DataLength = (Convert.IsDBNull(row["DataLength"]))?string.Empty:(System.String)row["DataLength"];
					c.ContentType = (Convert.IsDBNull(row["ContentType"]))?string.Empty:(System.String)row["ContentType"];
					c.Value = (Convert.IsDBNull(row["Value"]))?string.Empty:(System.String)row["Value"];
					c.Length = (Convert.IsDBNull(row["Length"]))?string.Empty:(System.String)row["Length"];
					c.IsFactOrMeasure = (Convert.IsDBNull(row["IsFactOrMeasure"]))?string.Empty:(System.String)row["IsFactOrMeasure"];
					c.IsDerived = (Convert.IsDBNull(row["IsDerived"]))?string.Empty:(System.String)row["IsDerived"];
					c.DomainAttributeType = (Convert.IsDBNull(row["DomainAttributeType"]))?string.Empty:(System.String)row["DomainAttributeType"];
					c.DomainDef = (Convert.IsDBNull(row["DomainDef"]))?string.Empty:(System.String)row["DomainDef"];
					c.AttributeDescription = (Convert.IsDBNull(row["AttributeDescription"]))?string.Empty:(System.String)row["AttributeDescription"];
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
		/// Fill an <see cref="VList&lt;METAView_DataAttribute_Retrieval&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAView_DataAttribute_Retrieval&gt;"/></returns>
		protected VList<METAView_DataAttribute_Retrieval> Fill(IDataReader reader, VList<METAView_DataAttribute_Retrieval> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAView_DataAttribute_Retrieval entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAView_DataAttribute_Retrieval>("METAView_DataAttribute_Retrieval",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAView_DataAttribute_Retrieval();
					}
					
					entity.SuppressEntityEvents = true;

					entity.WorkspaceName = (System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.WorkspaceName)];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_DataAttribute_RetrievalColumn.WorkspaceTypeId)];
					//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
					entity.VCStatusID = (System.Int32)reader[((int)METAView_DataAttribute_RetrievalColumn.VCStatusID)];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.pkid = (System.Int32)reader[((int)METAView_DataAttribute_RetrievalColumn.pkid)];
					//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.Machine)];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.VCMachineID)];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Name = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.Name)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.Name)];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.Abbreviation = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.Abbreviation)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.Abbreviation)];
					//entity.Abbreviation = (Convert.IsDBNull(reader["Abbreviation"]))?string.Empty:(System.String)reader["Abbreviation"];
					entity.Description = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.Description)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.Description)];
					//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
					entity.IsBusinessExternal = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.IsBusinessExternal)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.IsBusinessExternal)];
					//entity.IsBusinessExternal = (Convert.IsDBNull(reader["IsBusinessExternal"]))?string.Empty:(System.String)reader["IsBusinessExternal"];
					entity.ArchitectureStatus = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.ArchitectureStatus)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.ArchitectureStatus)];
					//entity.ArchitectureStatus = (Convert.IsDBNull(reader["ArchitectureStatus"]))?string.Empty:(System.String)reader["ArchitectureStatus"];
					entity.ArchitectureStatusDate = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.ArchitectureStatusDate)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.ArchitectureStatusDate)];
					//entity.ArchitectureStatusDate = (Convert.IsDBNull(reader["ArchitectureStatusDate"]))?string.Empty:(System.String)reader["ArchitectureStatusDate"];
					entity.DesignRationale = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.DesignRationale)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.DesignRationale)];
					//entity.DesignRationale = (Convert.IsDBNull(reader["DesignRationale"]))?string.Empty:(System.String)reader["DesignRationale"];
					entity.GeneralRemarks = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.GeneralRemarks)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.GeneralRemarks)];
					//entity.GeneralRemarks = (Convert.IsDBNull(reader["GeneralRemarks"]))?string.Empty:(System.String)reader["GeneralRemarks"];
					entity.GapType = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.GapType)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.GapType)];
					//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
					entity.DataSourceID = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.DataSourceID)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.DataSourceID)];
					//entity.DataSourceID = (Convert.IsDBNull(reader["DataSourceID"]))?string.Empty:(System.String)reader["DataSourceID"];
					entity.DataSourceName = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.DataSourceName)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.DataSourceName)];
					//entity.DataSourceName = (Convert.IsDBNull(reader["DataSourceName"]))?string.Empty:(System.String)reader["DataSourceName"];
					entity.DataType = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.DataType)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.DataType)];
					//entity.DataType = (Convert.IsDBNull(reader["DataType"]))?string.Empty:(System.String)reader["DataType"];
					entity.DataLength = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.DataLength)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.DataLength)];
					//entity.DataLength = (Convert.IsDBNull(reader["DataLength"]))?string.Empty:(System.String)reader["DataLength"];
					entity.ContentType = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.ContentType)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.ContentType)];
					//entity.ContentType = (Convert.IsDBNull(reader["ContentType"]))?string.Empty:(System.String)reader["ContentType"];
					entity.Value = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.Value)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.Value)];
					//entity.Value = (Convert.IsDBNull(reader["Value"]))?string.Empty:(System.String)reader["Value"];
					entity.Length = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.Length)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.Length)];
					//entity.Length = (Convert.IsDBNull(reader["Length"]))?string.Empty:(System.String)reader["Length"];
					entity.IsFactOrMeasure = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.IsFactOrMeasure)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.IsFactOrMeasure)];
					//entity.IsFactOrMeasure = (Convert.IsDBNull(reader["IsFactOrMeasure"]))?string.Empty:(System.String)reader["IsFactOrMeasure"];
					entity.IsDerived = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.IsDerived)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.IsDerived)];
					//entity.IsDerived = (Convert.IsDBNull(reader["IsDerived"]))?string.Empty:(System.String)reader["IsDerived"];
					entity.DomainAttributeType = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.DomainAttributeType)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.DomainAttributeType)];
					//entity.DomainAttributeType = (Convert.IsDBNull(reader["DomainAttributeType"]))?string.Empty:(System.String)reader["DomainAttributeType"];
					entity.DomainDef = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.DomainDef)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.DomainDef)];
					//entity.DomainDef = (Convert.IsDBNull(reader["DomainDef"]))?string.Empty:(System.String)reader["DomainDef"];
					entity.AttributeDescription = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.AttributeDescription)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.AttributeDescription)];
					//entity.AttributeDescription = (Convert.IsDBNull(reader["AttributeDescription"]))?string.Empty:(System.String)reader["AttributeDescription"];
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
		/// Refreshes the <see cref="METAView_DataAttribute_Retrieval"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_DataAttribute_Retrieval"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAView_DataAttribute_Retrieval entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.WorkspaceName)];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeId = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.WorkspaceTypeId)))?null:(System.Int32?)reader[((int)METAView_DataAttribute_RetrievalColumn.WorkspaceTypeId)];
			//entity.WorkspaceTypeId = (Convert.IsDBNull(reader["WorkspaceTypeId"]))?(int)0:(System.Int32?)reader["WorkspaceTypeId"];
			entity.VCStatusID = (System.Int32)reader[((int)METAView_DataAttribute_RetrievalColumn.VCStatusID)];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.pkid = (System.Int32)reader[((int)METAView_DataAttribute_RetrievalColumn.pkid)];
			//entity.pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.Machine)];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.VCMachineID)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.VCMachineID)];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Name = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.Name)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.Name)];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.Abbreviation = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.Abbreviation)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.Abbreviation)];
			//entity.Abbreviation = (Convert.IsDBNull(reader["Abbreviation"]))?string.Empty:(System.String)reader["Abbreviation"];
			entity.Description = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.Description)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.Description)];
			//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
			entity.IsBusinessExternal = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.IsBusinessExternal)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.IsBusinessExternal)];
			//entity.IsBusinessExternal = (Convert.IsDBNull(reader["IsBusinessExternal"]))?string.Empty:(System.String)reader["IsBusinessExternal"];
			entity.ArchitectureStatus = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.ArchitectureStatus)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.ArchitectureStatus)];
			//entity.ArchitectureStatus = (Convert.IsDBNull(reader["ArchitectureStatus"]))?string.Empty:(System.String)reader["ArchitectureStatus"];
			entity.ArchitectureStatusDate = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.ArchitectureStatusDate)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.ArchitectureStatusDate)];
			//entity.ArchitectureStatusDate = (Convert.IsDBNull(reader["ArchitectureStatusDate"]))?string.Empty:(System.String)reader["ArchitectureStatusDate"];
			entity.DesignRationale = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.DesignRationale)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.DesignRationale)];
			//entity.DesignRationale = (Convert.IsDBNull(reader["DesignRationale"]))?string.Empty:(System.String)reader["DesignRationale"];
			entity.GeneralRemarks = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.GeneralRemarks)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.GeneralRemarks)];
			//entity.GeneralRemarks = (Convert.IsDBNull(reader["GeneralRemarks"]))?string.Empty:(System.String)reader["GeneralRemarks"];
			entity.GapType = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.GapType)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.GapType)];
			//entity.GapType = (Convert.IsDBNull(reader["GapType"]))?string.Empty:(System.String)reader["GapType"];
			entity.DataSourceID = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.DataSourceID)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.DataSourceID)];
			//entity.DataSourceID = (Convert.IsDBNull(reader["DataSourceID"]))?string.Empty:(System.String)reader["DataSourceID"];
			entity.DataSourceName = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.DataSourceName)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.DataSourceName)];
			//entity.DataSourceName = (Convert.IsDBNull(reader["DataSourceName"]))?string.Empty:(System.String)reader["DataSourceName"];
			entity.DataType = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.DataType)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.DataType)];
			//entity.DataType = (Convert.IsDBNull(reader["DataType"]))?string.Empty:(System.String)reader["DataType"];
			entity.DataLength = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.DataLength)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.DataLength)];
			//entity.DataLength = (Convert.IsDBNull(reader["DataLength"]))?string.Empty:(System.String)reader["DataLength"];
			entity.ContentType = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.ContentType)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.ContentType)];
			//entity.ContentType = (Convert.IsDBNull(reader["ContentType"]))?string.Empty:(System.String)reader["ContentType"];
			entity.Value = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.Value)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.Value)];
			//entity.Value = (Convert.IsDBNull(reader["Value"]))?string.Empty:(System.String)reader["Value"];
			entity.Length = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.Length)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.Length)];
			//entity.Length = (Convert.IsDBNull(reader["Length"]))?string.Empty:(System.String)reader["Length"];
			entity.IsFactOrMeasure = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.IsFactOrMeasure)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.IsFactOrMeasure)];
			//entity.IsFactOrMeasure = (Convert.IsDBNull(reader["IsFactOrMeasure"]))?string.Empty:(System.String)reader["IsFactOrMeasure"];
			entity.IsDerived = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.IsDerived)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.IsDerived)];
			//entity.IsDerived = (Convert.IsDBNull(reader["IsDerived"]))?string.Empty:(System.String)reader["IsDerived"];
			entity.DomainAttributeType = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.DomainAttributeType)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.DomainAttributeType)];
			//entity.DomainAttributeType = (Convert.IsDBNull(reader["DomainAttributeType"]))?string.Empty:(System.String)reader["DomainAttributeType"];
			entity.DomainDef = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.DomainDef)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.DomainDef)];
			//entity.DomainDef = (Convert.IsDBNull(reader["DomainDef"]))?string.Empty:(System.String)reader["DomainDef"];
			entity.AttributeDescription = (reader.IsDBNull(((int)METAView_DataAttribute_RetrievalColumn.AttributeDescription)))?null:(System.String)reader[((int)METAView_DataAttribute_RetrievalColumn.AttributeDescription)];
			//entity.AttributeDescription = (Convert.IsDBNull(reader["AttributeDescription"]))?string.Empty:(System.String)reader["AttributeDescription"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAView_DataAttribute_Retrieval"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAView_DataAttribute_Retrieval"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAView_DataAttribute_Retrieval entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeId = (Convert.IsDBNull(dataRow["WorkspaceTypeId"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeId"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.Name = (Convert.IsDBNull(dataRow["Name"]))?string.Empty:(System.String)dataRow["Name"];
			entity.Abbreviation = (Convert.IsDBNull(dataRow["Abbreviation"]))?string.Empty:(System.String)dataRow["Abbreviation"];
			entity.Description = (Convert.IsDBNull(dataRow["Description"]))?string.Empty:(System.String)dataRow["Description"];
			entity.IsBusinessExternal = (Convert.IsDBNull(dataRow["IsBusinessExternal"]))?string.Empty:(System.String)dataRow["IsBusinessExternal"];
			entity.ArchitectureStatus = (Convert.IsDBNull(dataRow["ArchitectureStatus"]))?string.Empty:(System.String)dataRow["ArchitectureStatus"];
			entity.ArchitectureStatusDate = (Convert.IsDBNull(dataRow["ArchitectureStatusDate"]))?string.Empty:(System.String)dataRow["ArchitectureStatusDate"];
			entity.DesignRationale = (Convert.IsDBNull(dataRow["DesignRationale"]))?string.Empty:(System.String)dataRow["DesignRationale"];
			entity.GeneralRemarks = (Convert.IsDBNull(dataRow["GeneralRemarks"]))?string.Empty:(System.String)dataRow["GeneralRemarks"];
			entity.GapType = (Convert.IsDBNull(dataRow["GapType"]))?string.Empty:(System.String)dataRow["GapType"];
			entity.DataSourceID = (Convert.IsDBNull(dataRow["DataSourceID"]))?string.Empty:(System.String)dataRow["DataSourceID"];
			entity.DataSourceName = (Convert.IsDBNull(dataRow["DataSourceName"]))?string.Empty:(System.String)dataRow["DataSourceName"];
			entity.DataType = (Convert.IsDBNull(dataRow["DataType"]))?string.Empty:(System.String)dataRow["DataType"];
			entity.DataLength = (Convert.IsDBNull(dataRow["DataLength"]))?string.Empty:(System.String)dataRow["DataLength"];
			entity.ContentType = (Convert.IsDBNull(dataRow["ContentType"]))?string.Empty:(System.String)dataRow["ContentType"];
			entity.Value = (Convert.IsDBNull(dataRow["Value"]))?string.Empty:(System.String)dataRow["Value"];
			entity.Length = (Convert.IsDBNull(dataRow["Length"]))?string.Empty:(System.String)dataRow["Length"];
			entity.IsFactOrMeasure = (Convert.IsDBNull(dataRow["IsFactOrMeasure"]))?string.Empty:(System.String)dataRow["IsFactOrMeasure"];
			entity.IsDerived = (Convert.IsDBNull(dataRow["IsDerived"]))?string.Empty:(System.String)dataRow["IsDerived"];
			entity.DomainAttributeType = (Convert.IsDBNull(dataRow["DomainAttributeType"]))?string.Empty:(System.String)dataRow["DomainAttributeType"];
			entity.DomainDef = (Convert.IsDBNull(dataRow["DomainDef"]))?string.Empty:(System.String)dataRow["DomainDef"];
			entity.AttributeDescription = (Convert.IsDBNull(dataRow["AttributeDescription"]))?string.Empty:(System.String)dataRow["AttributeDescription"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAView_DataAttribute_RetrievalFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_DataAttribute_Retrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_DataAttribute_RetrievalFilterBuilder : SqlFilterBuilder<METAView_DataAttribute_RetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_DataAttribute_RetrievalFilterBuilder class.
		/// </summary>
		public METAView_DataAttribute_RetrievalFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_DataAttribute_RetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_DataAttribute_RetrievalFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_DataAttribute_RetrievalFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_DataAttribute_RetrievalFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_DataAttribute_RetrievalFilterBuilder

	#region METAView_DataAttribute_RetrievalParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_DataAttribute_Retrieval"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAView_DataAttribute_RetrievalParameterBuilder : ParameterizedSqlFilterBuilder<METAView_DataAttribute_RetrievalColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_DataAttribute_RetrievalParameterBuilder class.
		/// </summary>
		public METAView_DataAttribute_RetrievalParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAView_DataAttribute_RetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAView_DataAttribute_RetrievalParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAView_DataAttribute_RetrievalParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAView_DataAttribute_RetrievalParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAView_DataAttribute_RetrievalParameterBuilder
	
	#region METAView_DataAttribute_RetrievalSortBuilder
    
    /// <summary>
    /// A strongly-typed instance of the <see cref="SqlSortBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAView_DataAttribute_Retrieval"/> object.
    /// </summary>
    [CLSCompliant(true)]
    public class METAView_DataAttribute_RetrievalSortBuilder : SqlSortBuilder<METAView_DataAttribute_RetrievalColumn>
    {
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAView_DataAttribute_RetrievalSqlSortBuilder class.
		/// </summary>
		public METAView_DataAttribute_RetrievalSortBuilder() : base() { }

		#endregion Constructors

    }    
    #endregion METAView_DataAttribute_RetrievalSortBuilder

} // end namespace
