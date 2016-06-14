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
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml;

namespace OpenLicense.LicenseFile.Constraints
{
	/// <summary>
	/// <p>The <see cref="DayTimeConstraint">DayTimeConstraint</see> constrains the user
	/// to only using this license during a period of time within a day.  I.E. A user may
	/// use the license between midnight and 7am and 5pm and midnight - Non business hours.
	/// </p>
	/// </summary>
	/// <seealso cref="AbstractConstraint">AbstractConstraint</seealso>
	/// <seealso cref="IConstraint">IConstraint</seealso>
	/// <seealso cref="OpenLicenseFile">OpenLicenseFile</seealso>
	public class DayTimeConstraint : AbstractConstraint
	{

		#region Fields (1) 

		/// <summary>
		/// The time range values collection.
		/// </summary>
		private TimeRangeCollection	timeRange	= new TimeRangeCollection( );

		#endregion Fields 

		#region Constructors (2) 

		/// <summary>
		/// This is the constructor for the <c>DayTimeConstraint</c>.  The constructor
		/// is used to create the object with a valid license to attach it to.
		/// </summary>
		public DayTimeConstraint( ) : this( null ) { }

		/// <summary>
		/// This is the constructor for the <c>DayTimeConstraint</c>.  The constructor
		/// is used to create the object and assign it to the proper license.
		/// </summary>
		/// <param name="license">
		/// The <see cref="OpenLicenseFile">OpenLicenseFile</see> this constraint
		/// belongs to.
		/// </param>
		public DayTimeConstraint( OpenLicenseFile license )
		{
			base.License		= license;
			base.Name			= "Day Time Constraint";
			base.Description	=  "The DayTimeConstraint constrains the user ";
			base.Description	+= "to only using this license during a period of time within a day.  I.E. A user may ";
			base.Description	+= "use the license between midnight and 7am and 5pm and midnight - Non business hours.";
		}

		#endregion Constructors 

		#region Methods (4) 


		// Public Methods (4) 

		/// <summary>
		/// This is responsible for parsing a <see cref="System.String">String</see>
		/// to form the <c>DayTimeConstriant</c>.
		/// </summary>
		/// <param name="xmlData">
		/// The Xml data in the form of a <c>String</c>.
		/// </param>
		public override void FromXml( string xmlData )
		{
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml( xmlData );

			FromXml( xmlDoc.SelectSingleNode("/Constraint") );
		}

		/// <summary>
		/// This creates a <c>DayTimeConstraint</c> from an <see cref="System.Xml.XmlNode">XmlNode</see>.
		/// </summary>
		/// <param name="itemsNode">
		/// A <see cref="XmlNode">XmlNode</see> representing the <c>DayTimeConstraint</c>.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Thrown if the <see cref="XmlNode">XmlNode</see> is null.
		/// </exception>
		public override void FromXml( XmlNode itemsNode )
		{
			if( itemsNode == null )
				throw new ArgumentNullException( "The license data is null." );

			XmlNode		nameTextNode		= itemsNode.SelectSingleNode( "Name/text()" );
			XmlNode		descriptionTextNode	= itemsNode.SelectSingleNode( "Description/text()" );
			XmlNodeList	rangeListNode		= itemsNode.SelectNodes( "Range" );

			if( nameTextNode != null )
				Name 		= nameTextNode.Value;

			if( descriptionTextNode != null )
				Description	= descriptionTextNode.Value;

			if( this.timeRange == null && rangeListNode.Count > 0 )
					this.timeRange = new TimeRangeCollection( );

			for( int i = 0; i < rangeListNode.Count; i++ )
			{
				XmlAttributeCollection attrColection = ((XmlNode)rangeListNode[i]).Attributes;

				XmlAttribute startTimeNode		= (XmlAttribute)attrColection.GetNamedItem( "StartTime" );
				XmlAttribute endTimeNode		= (XmlAttribute)attrColection.GetNamedItem( "EndTime" );

				Time start	= null;
				Time end	= null;

				if( startTimeNode != null )
					start	= new Time( Convert.ToInt32( startTimeNode.Value ) );

				if( endTimeNode != null )
					end	= new Time( Convert.ToInt32( endTimeNode.Value ) );

				this.timeRange.Add( new TimeRange( start, end ) );
			}
		}

		/// <summary>
		/// Converts this <c>DayTimeConstraint</c> to an Xml <c>String</c>.
		/// </summary>
		/// <returns>
		/// A <c>String</c> representing the DayTimeConstraint as Xml data.
		/// </returns>
		public override string ToXmlString( )
		{
			StringBuilder xmlString = new StringBuilder( );

			XmlTextWriter xmlWriter = new XmlTextWriter( new StringWriter( xmlString ) );
			xmlWriter.Formatting = Formatting.Indented;

			xmlWriter.WriteStartElement( "Constraint" );
				xmlWriter.WriteElementString( "Name", 				this.Name );
				xmlWriter.WriteElementString( "Type", 				this.GetType( ).ToString( ) );
				xmlWriter.WriteElementString( "Description",		this.Description );
				for( int i = 0; i < this.timeRange.Count; i++ )
			    {
					TimeRange r = this.timeRange[i];

					xmlWriter.WriteStartElement( "Range", "" );
					xmlWriter.WriteAttributeString( "StartTime",	"", r.Start.MilitaryTime.ToString( ) );
					xmlWriter.WriteAttributeString( "EndTime", 		"", r.End.MilitaryTime.ToString( ) );
					xmlWriter.WriteEndElement( );
				}
			xmlWriter.WriteEndElement( ); // Constraint

			xmlWriter.Close( );

			return xmlString.ToString( );
		}

		/// <summary>
		/// This verifies the license is allowed to be used during the current time range.
		/// </summary>
		/// <returns>
		/// <c>True</c> if the license meets the validation criteria.  Otherwise
		/// <c>False</c>.
		/// </returns>
		public override bool Validate( )
		{
			Time currentMoment = Time.Now();

			//Loop through each entry
			for( int i = 0; i < this.timeRange.Count; i++ )
		    {
				if( ((TimeRange)this.timeRange[i]).Start.MilitaryTime <= currentMoment.MilitaryTime &&
				    ((TimeRange)this.timeRange[i]).End.MilitaryTime >= currentMoment.MilitaryTime )
				{
					return true;
				}
		    }
			return false;
		}


		#endregion Methods 


#region Properties
		/// <summary>
		/// Gets or Sets the time range to be used for this constraint.
		/// </summary>
		/// <param>
		///	Sets the time range to be used for this constraint.
		/// </param>
		/// <returns>
		/// Gets the time range to be used for this constraint.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Constraints"),
		DefaultValue(null),
		Description( "Gets or Sets the time range to be used for this constraint." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public TimeRangeCollection Items
		{
			get
			{
				return this.timeRange;
			}
			set
			{
				this.timeRange	= value;
				this.isDirty	= true;
			}
		}
#endregion
	}


	/// <summary>
	/// <p>The Time object is a rough implementation of military time to be used as the
	/// time range.  This will eventually change but for now it serves the purpose of
	/// defining a time range using 0 to 2400.  This class also has the ability to convert
	/// the current DateTime.Now to a Time object value.</p>
	/// </summary>
	public class Time
	{

		#region Fields (1) 

		private int		timeValue			= -1;

		#endregion Fields 

		#region Constructors (2) 

		/// <summary>
		/// This will create a Time object with no time value being set
		/// </summary>
		public Time( )							: this( -1 ) { }

		/// <summary>
		/// This will create a time value with the passed in time value.
		/// </summary>
		public Time( int timeValue )
		{
			this.timeValue = timeValue;
		}

		#endregion Constructors 

		#region Properties (1) 

		/// <summary>
		/// Gets or Sets the current Military time value.  Military time is in the range of 0 to 2400.
		/// </summary>
		/// <param>
		///	Sets the current Military time value.
		/// </param>
		/// <returns>
		///	Gets the current Military time value.
		/// </returns>
		/// <exception cref="System.ApplicationException">
		/// This will throw an exception if the time value is out of range (0 to 2400).
		/// </exception>
		[
		Bindable(false),
		Browsable(true),
		Category("Time"),
		DefaultValue(0),
		Description( "Gets or Sets the current Military time value.  Military time is in the range of 0 to 2400." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public int MilitaryTime
		{
			get
			{
				return this.timeValue;
			}
			set
			{
				if( value < 0 || value > 2400 )
				{
					throw new ApplicationException( "The time must be in valid military time standard ( 0 to 2400)." );
				}
				this.timeValue = value;
			}
		}

		#endregion Properties 

		#region Methods (1) 


		// Public Methods (1) 

 //Military time value 0000 - 2400
		/// <summary>
		/// A static method to return the current Time.  This uses the DateTime.Now call
		/// and then converts it to a Time object.
		/// </summary>
		public static Time Now( )
		{
			DateTime d = DateTime.Now;

			int timeValue = d.Hour * 100;
			timeValue = timeValue + d.Minute;

			return new Time( timeValue );
		}


		#endregion Methods 

	}


	/// <summary>
	/// This is a range of Time values to provide a start/end time.
	/// </summary>
	public class TimeRange
	{

		#region Fields (2) 

		private Time end	= null;
		private Time start	= null;

		#endregion Fields 

		#region Constructors (3) 

		/// <summary>
		/// Initialized a Time Range with a the given start and stop time.
		/// </summary>
		public TimeRange( Time start, Time end )
		{
			this.start	= start;
			this.end	= end;
		}

		/// <summary>
		/// Initialized a Time Range with a start time of 0 and an end time of 2400.
		/// </summary>
		public TimeRange(  ) 			: this( new Time( 0 ), new Time( 2400 ) ) { }

		/// <summary>
		/// Initialized a Time Range with a given start time and an end time of 2400.
		/// </summary>
		public TimeRange( Time start )	: this( start, new Time( 2400 ) ) { }

		#endregion Constructors 

		#region Properties (2) 

		/// <summary>
		/// Gets or Sets the End Time value of this range.
		/// </summary>
		/// <param>
		///	Sets the End Time value of this range.
		/// </param>
		/// <returns>
		///	Gets the End Time value of this range.
		/// </returns>
		/// <exception cref="System.ApplicationException">
		/// This will throw an exception if the end time is less then or equal to the start time.
		/// </exception>
		[
		Bindable(false),
		Browsable(true),
		Category("Time"),
		DefaultValue(null),
		Description( "Gets or Sets the End Time value of this range." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public Time End
		{
			get
			{
				return this.end;
			}
			set
			{

				if( start.MilitaryTime > -1 && value.MilitaryTime > start.MilitaryTime )
				{
					this.end	= value;
				}
				else
				{
					throw new ApplicationException( "The End value may not be less then or equal to the Start value." );
				}
			}
		}

		/// <summary>
		/// Gets or Sets the Start Time value of this range.
		/// </summary>
		/// <param>
		///	Sets the Start Time value of this range.
		/// </param>
		/// <returns>
		///	Gets the Start Time value of this range.
		/// </returns>
		/// <exception cref="System.ApplicationException">
		/// This will throw an exception if the start time is greater then or equal to the end time.
		/// </exception>
		[
		Bindable(false),
		Browsable(true),
		Category("Time"),
		DefaultValue(null),
		Description( "Gets or Sets the Start Time value of this range." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public Time Start
		{
			get
			{
				return this.start;
			}
			set
			{
				if( end.MilitaryTime > -1 && value.MilitaryTime < end.MilitaryTime )
				{
					this.start	= value;
				}
				else
				{
					throw new ApplicationException( "The Start value may not be greater then or equal to the End value." );
				}
			}
		}

		#endregion Properties 

	}

	/// <summary>
	///   A collection that stores <see cref='TimeRange'/> objects.
	/// </summary>
	[Serializable()]
	public class TimeRangeCollection : CollectionBase {

		#region Constructors (3) 

		/// <summary>
		///   Initializes a new instance of <see cref='TimeRangeCollection'/> based on another <see cref='TimeRangeCollection'/>.
		/// </summary>
		/// <param name='val'>
		///   A <see cref='TimeRangeCollection'/> from which the contents are copied
		/// </param>
		public TimeRangeCollection(TimeRangeCollection val)
		{
			this.AddRange(val);
		}

		/// <summary>
		///   Initializes a new instance of <see cref='TimeRangeCollection'/> containing any array of <see cref='TimeRange'/> objects.
		/// </summary>
		/// <param name='val'>
		///       A array of <see cref='TimeRange'/> objects with which to initialize the collection
		/// </param>
		public TimeRangeCollection(TimeRange[] val)
		{
			this.AddRange(val);
		}

		/// <summary>
		/// Initializes a new instance of <see cref='TimeRangeCollection'/>.
		/// </summary>
		public TimeRangeCollection()
		{
		}

		#endregion Constructors 

		#region Properties (1) 

		/// <summary>
		///   Represents the entry at the specified index of the <see cref='TimeRange'/>.
		/// </summary>
		/// <param name='index'>The zero-based index of the entry to locate in the collection.</param>
		/// <value>The entry at the specified index of the collection.</value>
		/// <exception cref='ArgumentOutOfRangeException'><paramref name='index'/> is outside the valid range of indexes for the collection.</exception>
		public TimeRange this[int index] {
			get {
				return ((TimeRange)(List[index]));
			}
			set {
				List[index] = value;
			}
		}

		#endregion Properties 

		#region Methods (9) 


		// Public Methods (9) 

		/// <summary>
		///   Adds a <see cref='TimeRange'/> with the specified value to the
		///   <see cref='TimeRangeCollection'/>.
		/// </summary>
		/// <param name='val'>The <see cref='TimeRange'/> to add.</param>
		/// <returns>The index at which the new element was inserted.</returns>
		public int Add(TimeRange val)
		{
			return List.Add(val);
		}

		/// <summary>
		///   Copies the elements of an array to the end of the <see cref='TimeRangeCollection'/>.
		/// </summary>
		/// <param name='val'>
		///    An array of type <see cref='TimeRange'/> containing the objects to add to the collection.
		/// </param>
		/// <seealso cref='TimeRangeCollection.Add'/>
		public void AddRange(TimeRange[] val)
		{
			for (int i = 0; i < val.Length; i++) {
				this.Add(val[i]);
			}
		}

		/// <summary>
		///   Adds the contents of another <see cref='TimeRangeCollection'/> to the end of the collection.
		/// </summary>
		/// <param name='val'>
		///    A <see cref='TimeRangeCollection'/> containing the objects to add to the collection.
		/// </param>
		/// <seealso cref='TimeRangeCollection.Add'/>
		public void AddRange(TimeRangeCollection val)
		{
			for (int i = 0; i < val.Count; i++)
			{
				this.Add(val[i]);
			}
		}

		/// <summary>
		///   Gets a value indicating whether the
		///    <see cref='TimeRangeCollection'/> contains the specified <see cref='TimeRange'/>.
		/// </summary>
		/// <param name='val'>The <see cref='TimeRange'/> to locate.</param>
		/// <returns>
		/// <see langword='true'/> if the <see cref='TimeRange'/> is contained in the collection;
		///   otherwise, <see langword='false'/>.
		/// </returns>
		/// <seealso cref='TimeRangeCollection.IndexOf'/>
		public bool Contains(TimeRange val)
		{
			return List.Contains(val);
		}

		/// <summary>
		///   Copies the <see cref='TimeRangeCollection'/> values to a one-dimensional <see cref='Array'/> instance at the
		///    specified index.
		/// </summary>
		/// <param name='array'>The one-dimensional <see cref='Array'/> that is the destination of the values copied from <see cref='TimeRangeCollection'/>.</param>
		/// <param name='index'>The index in <paramref name='array'/> where copying begins.</param>
		/// <exception cref='ArgumentException'>
		///   <para><paramref name='array'/> is multidimensional.</para>
		///   <para>-or-</para>
		///   <para>The number of elements in the <see cref='TimeRangeCollection'/> is greater than
		///         the available space between <paramref name='arrayIndex'/> and the end of
		///         <paramref name='array'/>.</para>
		/// </exception>
		/// <exception cref='ArgumentNullException'><paramref name='array'/> is <see langword='null'/>. </exception>
		/// <exception cref='ArgumentOutOfRangeException'><paramref name='arrayIndex'/> is less than <paramref name='array'/>'s lowbound. </exception>
		/// <seealso cref='Array'/>
		public void CopyTo(TimeRange[] array, int index)
		{
			List.CopyTo(array, index);
		}

		/// <summary>
		///  Returns an enumerator that can iterate through the <see cref='TimeRangeCollection'/>.
		/// </summary>
		/// <seealso cref='IEnumerator'/>
		public new TimeRangeEnumerator GetEnumerator()
		{
			return new TimeRangeEnumerator(this);
		}

		/// <summary>
		///    Returns the index of a <see cref='TimeRange'/> in
		///       the <see cref='TimeRangeCollection'/>.
		/// </summary>
		/// <param name='val'>The <see cref='TimeRange'/> to locate.</param>
		/// <returns>
		///   The index of the <see cref='TimeRange'/> of <paramref name='val'/> in the
		///   <see cref='TimeRangeCollection'/>, if found; otherwise, -1.
		/// </returns>
		/// <seealso cref='TimeRangeCollection.Contains'/>
		public int IndexOf(TimeRange val)
		{
			return List.IndexOf(val);
		}

		/// <summary>
		///   Inserts a <see cref='TimeRange'/> into the <see cref='TimeRangeCollection'/> at the specified index.
		/// </summary>
		/// <param name='index'>The zero-based index where <paramref name='val'/> should be inserted.</param>
		/// <param name='val'>The <see cref='TimeRange'/> to insert.</param>
		/// <seealso cref='TimeRangeCollection.Add'/>
		public void Insert(int index, TimeRange val)
		{
			List.Insert(index, val);
		}

		/// <summary>
		///   Removes a specific <see cref='TimeRange'/> from the <see cref='TimeRangeCollection'/>.
		/// </summary>
		/// <param name='val'>The <see cref='TimeRange'/> to remove from the <see cref='TimeRangeCollection'/>.</param>
		/// <exception cref='ArgumentException'><paramref name='val'/> is not found in the Collection.</exception>
		public void Remove(TimeRange val)
		{
			List.Remove(val);
		}


		#endregion Methods 

		#region Nested Classes (1) 


		/// <summary>
		///   Enumerator that can iterate through a TimeRangeCollection.
		/// </summary>
		/// <seealso cref='IEnumerator'/>
		/// <seealso cref='TimeRangeCollection'/>
		/// <seealso cref='TimeRange'/>
		public class TimeRangeEnumerator : IEnumerator
		{

		#region Fields (2) 

			IEnumerator baseEnumerator;
			IEnumerable temp;

		#endregion Fields 

		#region Constructors (1) 

			/// <summary>
			///   Initializes a new instance of <see cref='TimeRangeEnumerator'/>.
			/// </summary>
			public TimeRangeEnumerator(TimeRangeCollection mappings)
			{
				this.temp = ((IEnumerable)(mappings));
				this.baseEnumerator = temp.GetEnumerator();
			}

		#endregion Constructors 

		#region Properties (2) 

			/// <summary>
			///   Gets the current <see cref='TimeRange'/> in the <seealso cref='TimeRangeCollection'/>.
			/// </summary>
			public TimeRange Current {
				get {
					return ((TimeRange)(baseEnumerator.Current));
				}
			}

			object IEnumerator.Current {
				get {
					return baseEnumerator.Current;
				}
			}

		#endregion Properties 

		#region Methods (2) 


		// Public Methods (2) 

			/// <summary>
			///   Advances the enumerator to the next <see cref='TimeRange'/> of the <see cref='TimeRangeCollection'/>.
			/// </summary>
			public bool MoveNext()
			{
				return baseEnumerator.MoveNext();
			}

			/// <summary>
			///   Sets the enumerator to its initial position, which is before the first element in the <see cref='TimeRangeCollection'/>.
			/// </summary>
			public void Reset()
			{
				baseEnumerator.Reset();
			}


		#endregion Methods 

		}
		#endregion Nested Classes 

	}
} /* NameSpace OpenLicense */
