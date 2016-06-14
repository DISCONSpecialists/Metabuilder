//--------------------------------------------------------------------------------
// Open License - A license manager for .NET software
// Copyright (C) 2004-2006 SP extreme (http://www.spextreme.com)
//
// This library is free software; you can redistribute it and/or modify it under 
// the terms of the GNU Lesser General Public License as published by the Free 
// Software Foundation; either version 2.1 of the License, or (at your option)  
// any later version.
//
// This library is distributed in the hope that it will be useful, but WITHOUT 
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
// FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more 
// details.
//
// You should have received a copy of the GNU Lesser General Public License 
// along with this library; if not, write to the Free Software Foundation, Inc., 
// 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//--------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace OpenLicense.LicenseFile
{
	/// <summary>
	/// The <c>Statistics</c> object inherits from the <see cref="AbstractLicenseData"/>.  The
	/// <c>Statistics</c> is the information about usage of the license for general
	/// information purposes*.
	/// </summary>
	/// <remarks>
	/// Currently it is not used for anything else.  In later version of Open License this
	/// information may be passed back to a License Server to support tracking License
	/// Statistics.
	/// </remarks>
	/// <seealso cref="OpenLicenseFile">OpenLicenseFile</seealso>
	/// <seealso cref="AbstractLicenseData">AbstractLicenseData</seealso>
	public class Statistics : AbstractLicenseData
	{

		#region Fields (5) 

		private int				accessCount			= 0;
		private DateTime		dateLastAccessed	= new DateTime( 0 );
		private int				daysCount			= 0;
		private int				hitCount			= 0;
		private int				usageCount			= 0;

		#endregion Fields 

		#region Constructors (6) 

		/// <summary>
		/// This initializes a <c>Statistics</c> object with the passed in values.
		/// </summary>
		/// <param name="access">
		/// The account count to initialize this object with.
		/// </param>
		/// <param name="days">
		/// The day count to initialize this object with.
		/// </param>
		/// <param name="usage">
		/// The usage count to initialize this object with.
		/// </param>
		/// <param name="hits">
		/// The hit count to initialize this object with.
		/// </param>
		/// <param name="dateAccessed">
		/// The DateTime value to initialize this objects last access time with.
		/// </param>
		public Statistics( int access, int days, int usage, int hits, DateTime dateAccessed )
		{
			this.daysCount				= days;
			this.hitCount				= hits;
			this.usageCount				= usage;
			this.accessCount			= access;
			this.dateLastAccessed		= dateAccessed;
		}

		/// <summary>
		/// This initializes a <c>Statistics</c> object with the passed in values.
		/// </summary>
		/// <param name="access">
		/// The account count to initialize this object with.
		/// </param>
		/// <param name="days">
		/// The day count to initialize this object with.
		/// </param>
		/// <param name="usage">
		/// The usage count to initialize this object with.
		/// </param>
		/// <param name="hits">
		/// The hit count to initialize this object with.
		/// </param>
		public Statistics( int access, int days, int usage, int hits )	: this( access, days, usage, hits, DateTime.Now ) { }

		/// <summary>
		/// This initializes a <c>Statistics</c> object with the passed in values.
		/// </summary>
		/// <param name="access">
		/// The account count to initialize this object with.
		/// </param>
		/// <param name="days">
		/// The day count to initialize this object with.
		/// </param>
		/// <param name="usage">
		/// The usage count to initialize this object with.
		/// </param>
		public Statistics( int access, int days, int usage )			: this( access, days, usage, 0, DateTime.Now ) { }

		/// <summary>
		/// This initializes a <c>Statistics</c> object with the passed in values.
		/// </summary>
		/// <param name="access">
		/// The account count to initialize this object with.
		/// </param>
		/// <param name="days">
		/// The day count to initialize this object with.
		/// </param>
		public Statistics( int access, int days )						: this( access, days, 0, 0, DateTime.Now ) { }

		/// <summary>
		/// This initializes an empty <c>Statistics</c> object.
		/// </summary>
		public Statistics( )											: this( 0, 0, 0, 0, DateTime.Now ) { }

		/// <summary>
		/// This initializes a <c>Statistics</c> object with the passed in value.
		/// </summary>
		/// <param name="access">
		/// The account count to initialize this object with.
		/// </param>
		public Statistics( int access )									: this( access, 0, 0, 0, DateTime.Now ) { }

		#endregion Constructors 

		#region Methods (9) 


		// Public Methods (9) 

		/// <summary>
		/// This is a static method that creates an <c>Statistics</c> object from the passed in XML
		/// <see cref="System.String">String</see>.
		/// </summary>
		/// <param>
		/// The <see cref="System.String">String</see> representing the Xml data.
		/// </param>
		/// <returns>
		/// The <c>Statistics</c> object created from parsing this
		/// <see cref="System.String">String</see>.
		/// </returns>
		public static Statistics FromXml( String xmlData )
		{
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml( xmlData );

			return FromXml( xmlDoc.SelectSingleNode("/Statistics") );
		}

		/// <summary>
		/// This is a static method that creates an <c>Statistics</c> object from a
		/// <see cref="XmlNode">XmlNode</see>.
		/// </summary>
		/// <param>
		/// A <see cref="XmlNode">XmlNode</see> representing the <c>Statistics</c> object.
		/// </param>
		/// <returns>
		/// The <c>Statistics</c> object created from this <see cref="XmlNode">XmlNode</see>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// Thrown if the license data is null.
		/// </exception>
		public static Statistics FromXml( XmlNode itemsNode )
		{
            return new Statistics();
			if( itemsNode == null )
				throw new ArgumentNullException( "The license data is null." );

			int			hit				= 0;
			int			usage			= 0;
			int			days			= 0;
			int			access			= 0;
			DateTime	dateAccessed	= new DateTime( 0 );

			XmlNode hitTextNode				= itemsNode.SelectSingleNode( "HitCount/text()" );
			XmlNode usageTextNode			= itemsNode.SelectSingleNode( "UsageCount/text()" );
			XmlNode daysTextNode			= itemsNode.SelectSingleNode( "DaysCount/text()" );
			XmlNode accessTextNode			= itemsNode.SelectSingleNode( "AccessCount/text()" );
			XmlNode dateAccessedTextNode	= itemsNode.SelectSingleNode( "DateTimeLastAccessed/text()" );

			if( dateAccessedTextNode != null )
			{
                try
                {
                    IFormatProvider culture = new CultureInfo( "en-ZA", true );
                    dateAccessed = MetaBuilder.Core.GlobalParser.ParseGlobalisedDateString(dateAccessedTextNode.Value);//, culture, DateTimeStyles.NoCurrentDateDefault);
                }
                catch
                {
                    dateAccessed = Convert.ToDateTime( dateAccessedTextNode.Value );
                }
			
			}
			
			if( daysTextNode != null )
				days = Convert.ToInt32( daysTextNode.Value );

			if( usageTextNode != null )
				usage = Convert.ToInt32( usageTextNode.Value );

			if( hitTextNode != null )
				hit = Convert.ToInt32( hitTextNode.Value );

			if( accessTextNode != null )
				access = Convert.ToInt32( accessTextNode.Value );

			return new Statistics( access, days, usage, hit, dateAccessed );
		}

		/// <summary>
		/// This increments the total number of accesses for this license by one.
		/// </summary>
		public void IncrementAccessCount( )
		{
			this.accessCount++;
			this.isDirty = true;
			
			UpdateLastAccessDate( );
		}

		/// <summary>
		/// This increments the total number of days this license has been used by
		/// one.  They day will only incremented when a new day has occurred.
		/// </summary>
		public void IncrementDaysUsed( )
		{
			if( this.DateTimeLastAccessed.Month	!= DateTime.Now.Month	||
			    this.DateTimeLastAccessed.Day	!= DateTime.Now.Day		||
			    this.DateTimeLastAccessed.Year	!= DateTime.Now.Year )
			{
				this.daysCount++;
				this.isDirty = true;
			
				UpdateLastAccessDate( );
			}
		}

		/// <summary>
		/// This increments the total number of hits for this license by one.
		/// </summary>
		public void IncrementHitCount( )
		{
			this.hitCount++;
			this.isDirty = true;
			
			UpdateLastAccessDate( );
		}

		/// <summary>
		/// This increments the total number of uses for this license by one.
		/// </summary>
		public void IncrementUsageUsed( )
		{
			this.usageCount++;
			this.isDirty = true;
			
			UpdateLastAccessDate( );
		}

		/// <summary>
		/// This resets the statical information that should only be set by the user.  The
		/// creator of the lilcense shouldn't effect these values.  This is primarily used 
		/// by Open License Builder.
		/// </summary>
		public void ResetUserStatistics( )
		{
			this.dateLastAccessed	= new DateTime( 0 );
			this.accessCount		= 0;
			this.daysCount			= 0;
			this.hitCount			= 0;
			this.usageCount			= 0;
		}

		/// <summary>
		/// This creates a <see cref="System.String">String</see> representing the
		/// XML form for this <c>Statistics</c> object.
		/// </summary>
		/// <returns>
		/// The <see cref="System.String">String</see> representing this <c>Statistics</c> in it's XML form.
		/// </returns>
		public override String ToXmlString( )
		{
            return "";
			StringBuilder	xmlString	= new StringBuilder( );
			XmlTextWriter	xmlWriter	= new XmlTextWriter( new StringWriter( xmlString ) );

			xmlWriter.Formatting = Formatting.Indented;
			xmlWriter.IndentChar = '\t';

			xmlWriter.WriteStartElement( "Statistics" );
            xmlWriter.WriteElementString("DateTimeLastAccessed", dateLastAccessed.ToString(new CultureInfo("en-ZA"))); //"yy/MM/dd hh:mm"));
				xmlWriter.WriteElementString( "AccessCount", 			this.AccessCount.ToString( ) );
				xmlWriter.WriteElementString( "HitCount", 				this.HitCount.ToString( ) );
				xmlWriter.WriteElementString( "DaysCount",				this.DaysCount.ToString( ) );
				xmlWriter.WriteElementString( "UsageCount",				this.UsageCount.ToString( ) );
			xmlWriter.WriteEndElement( ); // Statistics
			xmlWriter.Close( );

			return xmlString.ToString( );
		}

		/// <summary>
		/// This updates the LastAccess date.
		/// </summary>
		public void UpdateLastAccessDate( )
		{
			this.dateLastAccessed = DateTime.Now;
			this.isDirty = true;
		}


		#endregion Methods 


#region Properties
		/// <summary>
		/// Gets the number of times the license has been accessed.
		/// </summary>
		/// <returns>
		/// Gets the number of times the license has been accessed.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Data"),
		DefaultValue(0),
		Description( "Gets the number of times the license has been accessed." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		ReadOnly(true)
		]
		public int AccessCount
		{
			get
			{
				return this.accessCount;
			}
		}

		/// <summary>
		/// Gets the number of times the license has been accessed. Generally used in web service environments.
		/// </summary>
		/// <returns>
		/// Gets the number of times the license has been accessed. Generally used in web service environments
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Data"),
		DefaultValue(0),
		Description( "Gets the number of times the license has been accessed. Generally used in web service environments" ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		ReadOnly(true)
		]
		public int HitCount
		{
			get
			{
				return this.hitCount;
			}
		}

		/// <summary>
		/// Gets the number of days this license has been used.
		/// </summary>
		/// <returns>
		/// Gets the number of days this license has been used.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Data"),
		DefaultValue(0),
		Description( "Gets the number of days this license has been used." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		ReadOnly(true)
		]
		public int DaysCount
		{
			get
			{
				return this.daysCount;
			}
		}

		/// <summary>
		/// Gets the number of uses this license has had.
		/// </summary>
		/// <returns>
		/// Gets the number of uses this license has had.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Data"),
		DefaultValue(0),
		Description( "Gets the number of uses this license has had." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		ReadOnly(true)
		]
		public int UsageCount
		{
			get
			{
				return this.usageCount;
			}
		}

		/// <summary>
		/// Gets the last time this license was accessed.
		/// </summary>
		/// <returns>
		/// Gets the last time this license was accessed.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Data"),
		DefaultValue(0),
		Description( "Gets the last time this license was accessed." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		ReadOnly(true)
		]
		public DateTime DateTimeLastAccessed
		{
			get
			{
				return this.dateLastAccessed;
			}
		}
#endregion
	}
}
