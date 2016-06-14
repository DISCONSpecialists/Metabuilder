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
	/// This class is the base class for any <see cref="METAViewSoftwareListingProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class METAViewSoftwareListingProviderBaseCore : EntityViewProviderBase<METAViewSoftwareListing>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;METAViewSoftwareListing&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;METAViewSoftwareListing&gt;"/></returns>
		protected static VList&lt;METAViewSoftwareListing&gt; Fill(DataSet dataSet, VList<METAViewSoftwareListing> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<METAViewSoftwareListing>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;METAViewSoftwareListing&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<METAViewSoftwareListing>"/></returns>
		protected static VList&lt;METAViewSoftwareListing&gt; Fill(DataTable dataTable, VList<METAViewSoftwareListing> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					METAViewSoftwareListing c = new METAViewSoftwareListing();
					c.WorkspaceName = (Convert.IsDBNull(row["WorkspaceName"]))?string.Empty:(System.String)row["WorkspaceName"];
					c.WorkspaceTypeID = (Convert.IsDBNull(row["WorkspaceTypeID"]))?(int)0:(System.Int32?)row["WorkspaceTypeID"];
					c.VCStatusID = (Convert.IsDBNull(row["VCStatusID"]))?(int)0:(System.Int32)row["VCStatusID"];
					c.Pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
					c.VCMachineID = (Convert.IsDBNull(row["VCMachineID"]))?string.Empty:(System.String)row["VCMachineID"];
					c.Type = (Convert.IsDBNull(row["Type"]))?string.Empty:(System.String)row["Type"];
					c.Name = (Convert.IsDBNull(row["Name"]))?string.Empty:(System.String)row["Name"];
					c.Description = (Convert.IsDBNull(row["Description"]))?string.Empty:(System.String)row["Description"];
					c.SeverityRating = (Convert.IsDBNull(row["SeverityRating"]))?string.Empty:(System.String)row["SeverityRating"];
					c.Configuration = (Convert.IsDBNull(row["Configuration"]))?string.Empty:(System.String)row["Configuration"];
					c.Copyright = (Convert.IsDBNull(row["Copyright"]))?string.Empty:(System.String)row["Copyright"];
					c.Publisher = (Convert.IsDBNull(row["Publisher"]))?string.Empty:(System.String)row["Publisher"];
					c.InternalName = (Convert.IsDBNull(row["InternalName"]))?string.Empty:(System.String)row["InternalName"];
					c.Language = (Convert.IsDBNull(row["Language"]))?string.Empty:(System.String)row["Language"];
					c.DateCreated = (Convert.IsDBNull(row["DateCreated"]))?string.Empty:(System.String)row["DateCreated"];
					c.IsDNS = (Convert.IsDBNull(row["isDNS"]))?string.Empty:(System.String)row["isDNS"];
					c.IsDHCP = (Convert.IsDBNull(row["isDHCP"]))?string.Empty:(System.String)row["isDHCP"];
					c.IsLicensed = (Convert.IsDBNull(row["isLicensed"]))?string.Empty:(System.String)row["isLicensed"];
					c.LicenseNumber = (Convert.IsDBNull(row["LicenseNumber"]))?string.Empty:(System.String)row["LicenseNumber"];
					c.DatePurchased = (Convert.IsDBNull(row["DatePurchased"]))?string.Empty:(System.String)row["DatePurchased"];
					c.Version = (Convert.IsDBNull(row["Version"]))?string.Empty:(System.String)row["Version"];
					c.ID = (Convert.IsDBNull(row["ID"]))?string.Empty:(System.String)row["ID"];
					c.CustomField1 = (Convert.IsDBNull(row["CustomField1"]))?string.Empty:(System.String)row["CustomField1"];
					c.CustomField2 = (Convert.IsDBNull(row["CustomField2"]))?string.Empty:(System.String)row["CustomField2"];
					c.CustomField3 = (Convert.IsDBNull(row["CustomField3"]))?string.Empty:(System.String)row["CustomField3"];
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
		/// Fill an <see cref="VList&lt;METAViewSoftwareListing&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;METAViewSoftwareListing&gt;"/></returns>
		protected VList<METAViewSoftwareListing> Fill(IDataReader reader, VList<METAViewSoftwareListing> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					METAViewSoftwareListing entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<METAViewSoftwareListing>("METAViewSoftwareListing",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new METAViewSoftwareListing();
					}
					entity.WorkspaceName = (System.String)reader["WorkspaceName"];
					//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
					entity.WorkspaceTypeID = (reader.IsDBNull(reader.GetOrdinal("WorkspaceTypeID")))?null:(System.Int32?)reader["WorkspaceTypeID"];
					//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
					entity.VCStatusID = (System.Int32)reader["VCStatusID"];
					//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
					entity.Pkid = (System.Int32)reader["pkid"];
					//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.Machine = (System.String)reader["Machine"];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.VCMachineID = (reader.IsDBNull(reader.GetOrdinal("VCMachineID")))?null:(System.String)reader["VCMachineID"];
					//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
					entity.Type = (reader.IsDBNull(reader.GetOrdinal("Type")))?null:(System.String)reader["Type"];
					//entity.Type = (Convert.IsDBNull(reader["Type"]))?string.Empty:(System.String)reader["Type"];
					entity.Name = (reader.IsDBNull(reader.GetOrdinal("Name")))?null:(System.String)reader["Name"];
					//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
					entity.Description = (reader.IsDBNull(reader.GetOrdinal("Description")))?null:(System.String)reader["Description"];
					//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
					entity.SeverityRating = (reader.IsDBNull(reader.GetOrdinal("SeverityRating")))?null:(System.String)reader["SeverityRating"];
					//entity.SeverityRating = (Convert.IsDBNull(reader["SeverityRating"]))?string.Empty:(System.String)reader["SeverityRating"];
					entity.Configuration = (reader.IsDBNull(reader.GetOrdinal("Configuration")))?null:(System.String)reader["Configuration"];
					//entity.Configuration = (Convert.IsDBNull(reader["Configuration"]))?string.Empty:(System.String)reader["Configuration"];
					entity.Copyright = (reader.IsDBNull(reader.GetOrdinal("Copyright")))?null:(System.String)reader["Copyright"];
					//entity.Copyright = (Convert.IsDBNull(reader["Copyright"]))?string.Empty:(System.String)reader["Copyright"];
					entity.Publisher = (reader.IsDBNull(reader.GetOrdinal("Publisher")))?null:(System.String)reader["Publisher"];
					//entity.Publisher = (Convert.IsDBNull(reader["Publisher"]))?string.Empty:(System.String)reader["Publisher"];
					entity.InternalName = (reader.IsDBNull(reader.GetOrdinal("InternalName")))?null:(System.String)reader["InternalName"];
					//entity.InternalName = (Convert.IsDBNull(reader["InternalName"]))?string.Empty:(System.String)reader["InternalName"];
					entity.Language = (reader.IsDBNull(reader.GetOrdinal("Language")))?null:(System.String)reader["Language"];
					//entity.Language = (Convert.IsDBNull(reader["Language"]))?string.Empty:(System.String)reader["Language"];
					entity.DateCreated = (reader.IsDBNull(reader.GetOrdinal("DateCreated")))?null:(System.String)reader["DateCreated"];
					//entity.DateCreated = (Convert.IsDBNull(reader["DateCreated"]))?string.Empty:(System.String)reader["DateCreated"];
					entity.IsDNS = (reader.IsDBNull(reader.GetOrdinal("isDNS")))?null:(System.String)reader["isDNS"];
					//entity.IsDNS = (Convert.IsDBNull(reader["isDNS"]))?string.Empty:(System.String)reader["isDNS"];
					entity.IsDHCP = (reader.IsDBNull(reader.GetOrdinal("isDHCP")))?null:(System.String)reader["isDHCP"];
					//entity.IsDHCP = (Convert.IsDBNull(reader["isDHCP"]))?string.Empty:(System.String)reader["isDHCP"];
					entity.IsLicensed = (reader.IsDBNull(reader.GetOrdinal("isLicensed")))?null:(System.String)reader["isLicensed"];
					//entity.IsLicensed = (Convert.IsDBNull(reader["isLicensed"]))?string.Empty:(System.String)reader["isLicensed"];
					entity.LicenseNumber = (reader.IsDBNull(reader.GetOrdinal("LicenseNumber")))?null:(System.String)reader["LicenseNumber"];
					//entity.LicenseNumber = (Convert.IsDBNull(reader["LicenseNumber"]))?string.Empty:(System.String)reader["LicenseNumber"];
					entity.DatePurchased = (reader.IsDBNull(reader.GetOrdinal("DatePurchased")))?null:(System.String)reader["DatePurchased"];
					//entity.DatePurchased = (Convert.IsDBNull(reader["DatePurchased"]))?string.Empty:(System.String)reader["DatePurchased"];
					entity.Version = (reader.IsDBNull(reader.GetOrdinal("Version")))?null:(System.String)reader["Version"];
					//entity.Version = (Convert.IsDBNull(reader["Version"]))?string.Empty:(System.String)reader["Version"];
					entity.ID = (reader.IsDBNull(reader.GetOrdinal("ID")))?null:(System.String)reader["ID"];
					//entity.ID = (Convert.IsDBNull(reader["ID"]))?string.Empty:(System.String)reader["ID"];
					entity.CustomField1 = (reader.IsDBNull(reader.GetOrdinal("CustomField1")))?null:(System.String)reader["CustomField1"];
					//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
					entity.CustomField2 = (reader.IsDBNull(reader.GetOrdinal("CustomField2")))?null:(System.String)reader["CustomField2"];
					//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
					entity.CustomField3 = (reader.IsDBNull(reader.GetOrdinal("CustomField3")))?null:(System.String)reader["CustomField3"];
					//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
					entity.AcceptChanges();
					rows.Add(entity);
					pageLength -= 1;
				}
				recordnum += 1;
			}
			return rows;
		}
		
		
		/// <summary>
		/// Refreshes the <see cref="METAViewSoftwareListing"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewSoftwareListing"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, METAViewSoftwareListing entity)
		{
			reader.Read();
			entity.WorkspaceName = (System.String)reader["WorkspaceName"];
			//entity.WorkspaceName = (Convert.IsDBNull(reader["WorkspaceName"]))?string.Empty:(System.String)reader["WorkspaceName"];
			entity.WorkspaceTypeID = (reader.IsDBNull(reader.GetOrdinal("WorkspaceTypeID")))?null:(System.Int32?)reader["WorkspaceTypeID"];
			//entity.WorkspaceTypeID = (Convert.IsDBNull(reader["WorkspaceTypeID"]))?(int)0:(System.Int32?)reader["WorkspaceTypeID"];
			entity.VCStatusID = (System.Int32)reader["VCStatusID"];
			//entity.VCStatusID = (Convert.IsDBNull(reader["VCStatusID"]))?(int)0:(System.Int32)reader["VCStatusID"];
			entity.Pkid = (System.Int32)reader["pkid"];
			//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.Machine = (System.String)reader["Machine"];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			entity.VCMachineID = (reader.IsDBNull(reader.GetOrdinal("VCMachineID")))?null:(System.String)reader["VCMachineID"];
			//entity.VCMachineID = (Convert.IsDBNull(reader["VCMachineID"]))?string.Empty:(System.String)reader["VCMachineID"];
			entity.Type = (reader.IsDBNull(reader.GetOrdinal("Type")))?null:(System.String)reader["Type"];
			//entity.Type = (Convert.IsDBNull(reader["Type"]))?string.Empty:(System.String)reader["Type"];
			entity.Name = (reader.IsDBNull(reader.GetOrdinal("Name")))?null:(System.String)reader["Name"];
			//entity.Name = (Convert.IsDBNull(reader["Name"]))?string.Empty:(System.String)reader["Name"];
			entity.Description = (reader.IsDBNull(reader.GetOrdinal("Description")))?null:(System.String)reader["Description"];
			//entity.Description = (Convert.IsDBNull(reader["Description"]))?string.Empty:(System.String)reader["Description"];
			entity.SeverityRating = (reader.IsDBNull(reader.GetOrdinal("SeverityRating")))?null:(System.String)reader["SeverityRating"];
			//entity.SeverityRating = (Convert.IsDBNull(reader["SeverityRating"]))?string.Empty:(System.String)reader["SeverityRating"];
			entity.Configuration = (reader.IsDBNull(reader.GetOrdinal("Configuration")))?null:(System.String)reader["Configuration"];
			//entity.Configuration = (Convert.IsDBNull(reader["Configuration"]))?string.Empty:(System.String)reader["Configuration"];
			entity.Copyright = (reader.IsDBNull(reader.GetOrdinal("Copyright")))?null:(System.String)reader["Copyright"];
			//entity.Copyright = (Convert.IsDBNull(reader["Copyright"]))?string.Empty:(System.String)reader["Copyright"];
			entity.Publisher = (reader.IsDBNull(reader.GetOrdinal("Publisher")))?null:(System.String)reader["Publisher"];
			//entity.Publisher = (Convert.IsDBNull(reader["Publisher"]))?string.Empty:(System.String)reader["Publisher"];
			entity.InternalName = (reader.IsDBNull(reader.GetOrdinal("InternalName")))?null:(System.String)reader["InternalName"];
			//entity.InternalName = (Convert.IsDBNull(reader["InternalName"]))?string.Empty:(System.String)reader["InternalName"];
			entity.Language = (reader.IsDBNull(reader.GetOrdinal("Language")))?null:(System.String)reader["Language"];
			//entity.Language = (Convert.IsDBNull(reader["Language"]))?string.Empty:(System.String)reader["Language"];
			entity.DateCreated = (reader.IsDBNull(reader.GetOrdinal("DateCreated")))?null:(System.String)reader["DateCreated"];
			//entity.DateCreated = (Convert.IsDBNull(reader["DateCreated"]))?string.Empty:(System.String)reader["DateCreated"];
			entity.IsDNS = (reader.IsDBNull(reader.GetOrdinal("isDNS")))?null:(System.String)reader["isDNS"];
			//entity.IsDNS = (Convert.IsDBNull(reader["isDNS"]))?string.Empty:(System.String)reader["isDNS"];
			entity.IsDHCP = (reader.IsDBNull(reader.GetOrdinal("isDHCP")))?null:(System.String)reader["isDHCP"];
			//entity.IsDHCP = (Convert.IsDBNull(reader["isDHCP"]))?string.Empty:(System.String)reader["isDHCP"];
			entity.IsLicensed = (reader.IsDBNull(reader.GetOrdinal("isLicensed")))?null:(System.String)reader["isLicensed"];
			//entity.IsLicensed = (Convert.IsDBNull(reader["isLicensed"]))?string.Empty:(System.String)reader["isLicensed"];
			entity.LicenseNumber = (reader.IsDBNull(reader.GetOrdinal("LicenseNumber")))?null:(System.String)reader["LicenseNumber"];
			//entity.LicenseNumber = (Convert.IsDBNull(reader["LicenseNumber"]))?string.Empty:(System.String)reader["LicenseNumber"];
			entity.DatePurchased = (reader.IsDBNull(reader.GetOrdinal("DatePurchased")))?null:(System.String)reader["DatePurchased"];
			//entity.DatePurchased = (Convert.IsDBNull(reader["DatePurchased"]))?string.Empty:(System.String)reader["DatePurchased"];
			entity.Version = (reader.IsDBNull(reader.GetOrdinal("Version")))?null:(System.String)reader["Version"];
			//entity.Version = (Convert.IsDBNull(reader["Version"]))?string.Empty:(System.String)reader["Version"];
			entity.ID = (reader.IsDBNull(reader.GetOrdinal("ID")))?null:(System.String)reader["ID"];
			//entity.ID = (Convert.IsDBNull(reader["ID"]))?string.Empty:(System.String)reader["ID"];
			entity.CustomField1 = (reader.IsDBNull(reader.GetOrdinal("CustomField1")))?null:(System.String)reader["CustomField1"];
			//entity.CustomField1 = (Convert.IsDBNull(reader["CustomField1"]))?string.Empty:(System.String)reader["CustomField1"];
			entity.CustomField2 = (reader.IsDBNull(reader.GetOrdinal("CustomField2")))?null:(System.String)reader["CustomField2"];
			//entity.CustomField2 = (Convert.IsDBNull(reader["CustomField2"]))?string.Empty:(System.String)reader["CustomField2"];
			entity.CustomField3 = (reader.IsDBNull(reader.GetOrdinal("CustomField3")))?null:(System.String)reader["CustomField3"];
			//entity.CustomField3 = (Convert.IsDBNull(reader["CustomField3"]))?string.Empty:(System.String)reader["CustomField3"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="METAViewSoftwareListing"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="METAViewSoftwareListing"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, METAViewSoftwareListing entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.WorkspaceName = (Convert.IsDBNull(dataRow["WorkspaceName"]))?string.Empty:(System.String)dataRow["WorkspaceName"];
			entity.WorkspaceTypeID = (Convert.IsDBNull(dataRow["WorkspaceTypeID"]))?(int)0:(System.Int32?)dataRow["WorkspaceTypeID"];
			entity.VCStatusID = (Convert.IsDBNull(dataRow["VCStatusID"]))?(int)0:(System.Int32)dataRow["VCStatusID"];
			entity.Pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.VCMachineID = (Convert.IsDBNull(dataRow["VCMachineID"]))?string.Empty:(System.String)dataRow["VCMachineID"];
			entity.Type = (Convert.IsDBNull(dataRow["Type"]))?string.Empty:(System.String)dataRow["Type"];
			entity.Name = (Convert.IsDBNull(dataRow["Name"]))?string.Empty:(System.String)dataRow["Name"];
			entity.Description = (Convert.IsDBNull(dataRow["Description"]))?string.Empty:(System.String)dataRow["Description"];
			entity.SeverityRating = (Convert.IsDBNull(dataRow["SeverityRating"]))?string.Empty:(System.String)dataRow["SeverityRating"];
			entity.Configuration = (Convert.IsDBNull(dataRow["Configuration"]))?string.Empty:(System.String)dataRow["Configuration"];
			entity.Copyright = (Convert.IsDBNull(dataRow["Copyright"]))?string.Empty:(System.String)dataRow["Copyright"];
			entity.Publisher = (Convert.IsDBNull(dataRow["Publisher"]))?string.Empty:(System.String)dataRow["Publisher"];
			entity.InternalName = (Convert.IsDBNull(dataRow["InternalName"]))?string.Empty:(System.String)dataRow["InternalName"];
			entity.Language = (Convert.IsDBNull(dataRow["Language"]))?string.Empty:(System.String)dataRow["Language"];
			entity.DateCreated = (Convert.IsDBNull(dataRow["DateCreated"]))?string.Empty:(System.String)dataRow["DateCreated"];
			entity.IsDNS = (Convert.IsDBNull(dataRow["isDNS"]))?string.Empty:(System.String)dataRow["isDNS"];
			entity.IsDHCP = (Convert.IsDBNull(dataRow["isDHCP"]))?string.Empty:(System.String)dataRow["isDHCP"];
			entity.IsLicensed = (Convert.IsDBNull(dataRow["isLicensed"]))?string.Empty:(System.String)dataRow["isLicensed"];
			entity.LicenseNumber = (Convert.IsDBNull(dataRow["LicenseNumber"]))?string.Empty:(System.String)dataRow["LicenseNumber"];
			entity.DatePurchased = (Convert.IsDBNull(dataRow["DatePurchased"]))?string.Empty:(System.String)dataRow["DatePurchased"];
			entity.Version = (Convert.IsDBNull(dataRow["Version"]))?string.Empty:(System.String)dataRow["Version"];
			entity.ID = (Convert.IsDBNull(dataRow["ID"]))?string.Empty:(System.String)dataRow["ID"];
			entity.CustomField1 = (Convert.IsDBNull(dataRow["CustomField1"]))?string.Empty:(System.String)dataRow["CustomField1"];
			entity.CustomField2 = (Convert.IsDBNull(dataRow["CustomField2"]))?string.Empty:(System.String)dataRow["CustomField2"];
			entity.CustomField3 = (Convert.IsDBNull(dataRow["CustomField3"]))?string.Empty:(System.String)dataRow["CustomField3"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region METAViewSoftwareListingFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewSoftwareListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewSoftwareListingFilterBuilder : SqlFilterBuilder<METAViewSoftwareListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewSoftwareListingFilterBuilder class.
		/// </summary>
		public METAViewSoftwareListingFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewSoftwareListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewSoftwareListingFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewSoftwareListingFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewSoftwareListingFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewSoftwareListingFilterBuilder

	#region METAViewSoftwareListingParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="METAViewSoftwareListing"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class METAViewSoftwareListingParameterBuilder : ParameterizedSqlFilterBuilder<METAViewSoftwareListingColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the METAViewSoftwareListingParameterBuilder class.
		/// </summary>
		public METAViewSoftwareListingParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the METAViewSoftwareListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public METAViewSoftwareListingParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the METAViewSoftwareListingParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public METAViewSoftwareListingParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion METAViewSoftwareListingParameterBuilder
} // end namespace
