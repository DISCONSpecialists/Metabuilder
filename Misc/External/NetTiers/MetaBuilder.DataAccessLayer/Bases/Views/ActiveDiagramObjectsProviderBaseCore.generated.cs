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
	/// This class is the base class for any <see cref="ActiveDiagramObjectsProviderBase"/> implementation.
	/// It exposes CRUD methods as well as selecting on index, foreign keys and custom stored procedures.
	///</summary>
	public abstract class ActiveDiagramObjectsProviderBaseCore : EntityViewProviderBase<ActiveDiagramObjects>
	{
		#region Custom Methods
		
		
		#endregion

		#region Helper Functions
		
		/*
		///<summary>
		/// Fill an VList&lt;ActiveDiagramObjects&gt; From a DataSet
		///</summary>
		/// <param name="dataSet">the DataSet</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList&lt;ActiveDiagramObjects&gt;"/></returns>
		protected static VList&lt;ActiveDiagramObjects&gt; Fill(DataSet dataSet, VList<ActiveDiagramObjects> rows, int start, int pagelen)
		{
			if (dataSet.Tables.Count == 1)
			{
				return Fill(dataSet.Tables[0], rows, start, pagelen);
			}
			else
			{
				return new VList<ActiveDiagramObjects>();
			}	
		}
		
		
		///<summary>
		/// Fill an VList&lt;ActiveDiagramObjects&gt; From a DataTable
		///</summary>
		/// <param name="dataTable">the DataTable that hold the data.</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pagelen">number of row.</param>
		///<returns><see chref="VList<ActiveDiagramObjects>"/></returns>
		protected static VList&lt;ActiveDiagramObjects&gt; Fill(DataTable dataTable, VList<ActiveDiagramObjects> rows, int start, int pagelen)
		{
			int recordnum = 0;
			
			System.Collections.IEnumerator dataRows =  dataTable.Rows.GetEnumerator();
			
			while (dataRows.MoveNext() && (pagelen != 0))
			{
				if(recordnum >= start)
				{
					DataRow row = (DataRow)dataRows.Current;
				
					ActiveDiagramObjects c = new ActiveDiagramObjects();
					c.Pkid = (Convert.IsDBNull(row["pkid"]))?(int)0:(System.Int32)row["pkid"];
					c.FileName = (Convert.IsDBNull(row["File Name"]))?string.Empty:(System.String)row["File Name"];
					c.MetaObjectID = (Convert.IsDBNull(row["MetaObjectID"]))?(int)0:(System.Int32)row["MetaObjectID"];
					c.Machine = (Convert.IsDBNull(row["Machine"]))?string.Empty:(System.String)row["Machine"];
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
		/// Fill an <see cref="VList&lt;ActiveDiagramObjects&gt;"/> From a DataReader.
		///</summary>
		/// <param name="reader">Datareader</param>
		/// <param name="rows">The collection to fill</param>
		/// <param name="start">Start row</param>
		/// <param name="pageLength">number of row.</param>
		///<returns>a <see cref="VList&lt;ActiveDiagramObjects&gt;"/></returns>
		protected VList<ActiveDiagramObjects> Fill(IDataReader reader, VList<ActiveDiagramObjects> rows, int start, int pageLength)
		{
			int recordnum = 0;
			while (reader.Read() && (pageLength != 0))
			{
				if(recordnum >= start)
				{
					ActiveDiagramObjects entity = null;
					if (DataRepository.Provider.UseEntityFactory)
					{
						entity = EntityManager.CreateViewEntity<ActiveDiagramObjects>("ActiveDiagramObjects",  DataRepository.Provider.EntityCreationalFactoryType); 
					}
					else
					{
						entity = new ActiveDiagramObjects();
					}
					entity.Pkid = (System.Int32)reader["pkid"];
					//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
					entity.FileName = (reader.IsDBNull(reader.GetOrdinal("File Name")))?null:(System.String)reader["File Name"];
					//entity.FileName = (Convert.IsDBNull(reader["File Name"]))?string.Empty:(System.String)reader["File Name"];
					entity.MetaObjectID = (System.Int32)reader["MetaObjectID"];
					//entity.MetaObjectID = (Convert.IsDBNull(reader["MetaObjectID"]))?(int)0:(System.Int32)reader["MetaObjectID"];
					entity.Machine = (System.String)reader["Machine"];
					//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
					entity.AcceptChanges();
					rows.Add(entity);
					pageLength -= 1;
				}
				recordnum += 1;
			}
			return rows;
		}
		
		
		/// <summary>
		/// Refreshes the <see cref="ActiveDiagramObjects"/> object from the <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
		/// <param name="entity">The <see cref="ActiveDiagramObjects"/> object to refresh.</param>
		protected void RefreshEntity(IDataReader reader, ActiveDiagramObjects entity)
		{
			reader.Read();
			entity.Pkid = (System.Int32)reader["pkid"];
			//entity.Pkid = (Convert.IsDBNull(reader["pkid"]))?(int)0:(System.Int32)reader["pkid"];
			entity.FileName = (reader.IsDBNull(reader.GetOrdinal("File Name")))?null:(System.String)reader["File Name"];
			//entity.FileName = (Convert.IsDBNull(reader["File Name"]))?string.Empty:(System.String)reader["File Name"];
			entity.MetaObjectID = (System.Int32)reader["MetaObjectID"];
			//entity.MetaObjectID = (Convert.IsDBNull(reader["MetaObjectID"]))?(int)0:(System.Int32)reader["MetaObjectID"];
			entity.Machine = (System.String)reader["Machine"];
			//entity.Machine = (Convert.IsDBNull(reader["Machine"]))?string.Empty:(System.String)reader["Machine"];
			reader.Close();
	
			entity.AcceptChanges();
		}
		
		/*
		/// <summary>
		/// Refreshes the <see cref="ActiveDiagramObjects"/> object from the <see cref="DataSet"/>.
		/// </summary>
		/// <param name="dataSet">The <see cref="DataSet"/> to read from.</param>
		/// <param name="entity">The <see cref="ActiveDiagramObjects"/> object.</param>
		protected static void RefreshEntity(DataSet dataSet, ActiveDiagramObjects entity)
		{
			DataRow dataRow = dataSet.Tables[0].Rows[0];
			
			entity.Pkid = (Convert.IsDBNull(dataRow["pkid"]))?(int)0:(System.Int32)dataRow["pkid"];
			entity.FileName = (Convert.IsDBNull(dataRow["File Name"]))?string.Empty:(System.String)dataRow["File Name"];
			entity.MetaObjectID = (Convert.IsDBNull(dataRow["MetaObjectID"]))?(int)0:(System.Int32)dataRow["MetaObjectID"];
			entity.Machine = (Convert.IsDBNull(dataRow["Machine"]))?string.Empty:(System.String)dataRow["Machine"];
			entity.AcceptChanges();
		}
		*/
			
		#endregion Helper Functions
	}//end class

	#region ActiveDiagramObjectsFilterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="SqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="ActiveDiagramObjects"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class ActiveDiagramObjectsFilterBuilder : SqlFilterBuilder<ActiveDiagramObjectsColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the ActiveDiagramObjectsFilterBuilder class.
		/// </summary>
		public ActiveDiagramObjectsFilterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the ActiveDiagramObjectsFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public ActiveDiagramObjectsFilterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the ActiveDiagramObjectsFilterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public ActiveDiagramObjectsFilterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion ActiveDiagramObjectsFilterBuilder

	#region ActiveDiagramObjectsParameterBuilder
	
	/// <summary>
	/// A strongly-typed instance of the <see cref="ParameterizedSqlFilterBuilder&lt;EntityColumn&gt;"/> class
	/// that is used exclusively with a <see cref="ActiveDiagramObjects"/> object.
	/// </summary>
	[CLSCompliant(true)]
	public class ActiveDiagramObjectsParameterBuilder : ParameterizedSqlFilterBuilder<ActiveDiagramObjectsColumn>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the ActiveDiagramObjectsParameterBuilder class.
		/// </summary>
		public ActiveDiagramObjectsParameterBuilder() : base() { }

		/// <summary>
		/// Initializes a new instance of the ActiveDiagramObjectsParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		public ActiveDiagramObjectsParameterBuilder(bool ignoreCase) : base(ignoreCase) { }

		/// <summary>
		/// Initializes a new instance of the ActiveDiagramObjectsParameterBuilder class.
		/// </summary>
		/// <param name="ignoreCase">Specifies whether to create case-insensitive statements.</param>
		/// <param name="useAnd">Specifies whether to combine statements using AND or OR.</param>
		public ActiveDiagramObjectsParameterBuilder(bool ignoreCase, bool useAnd) : base(ignoreCase, useAnd) { }

		#endregion Constructors
	}

	#endregion ActiveDiagramObjectsParameterBuilder
} // end namespace
