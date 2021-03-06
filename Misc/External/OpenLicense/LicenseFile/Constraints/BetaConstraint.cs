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

namespace OpenLicense.LicenseFile.Constraints
{
	/// <summary>
	/// <p>This <see cref='BetaConstraint'/> constrains the user
	/// to a given time period.  It supports an end date that the license will expire.
	/// It also has the ability to show the user a link to download an update to the
	/// beta once it expires.</p>
	/// </summary>
	/// <seealso cref="AbstractConstraint">AbstractConstraint</seealso>
	/// <seealso cref="IConstraint">IConstraint</seealso>
	/// <seealso cref="OpenLicenseFile">OpenLicenseFile</seealso>
	public class BetaConstraint : AbstractConstraint
	{
		private	DateTime		end					= new DateTime( );
		private String			updateUrl			= String.Empty;

		/// <summary>
		/// This is the constructor for the <c>BetaConstraint</c>.  The constructor
		/// is used to create the object with a valid license to attach it to.
		/// </summary>
		public BetaConstraint( ) : this( null ) { }

		/// <summary>
		/// This is the constructor for the <c>BetaConstraint</c>.  The constructor
		/// is used to create the object and assign it to the proper license.
		/// </summary>
		/// <param name="license">
		/// The <see cref="OpenLicenseFile">OpenLicenseFile</see> this constraint
		/// belongs to.
		/// </param>
		public BetaConstraint( OpenLicenseFile license )
		{
			base.License		= license;
			base.Name			= "Beta Constraint";
			base.Description	=  "The BetaConstraint constrains the user to a given time ";
			base.Description	+= "period.  It supports an end date that the license will ";
			base.Description	+= "expire. It also has the ability to show the user a link ";
			base.Description	+= "to download an update to the beta once it expires.";
		}

		/// <summary>
		/// <p>This verifies the license meets its desired validation criteria.  This includes
		/// validating that the license is before the defined end date.  If it is not then
		/// the license validation will return false and the failure reason will be set.</p>
		/// </summary>
		/// <returns>
		/// <c>True</c> if the license meets the validation criteria.  Otherwise
		/// <c>False</c>.
		/// </returns>
		/// <remarks>
		/// When a failure occurs the FailureReason will be set to: "The beta period has
		/// expired. You may get an update at: xxx"  The section "You may get an update
		/// at: xxx" will only be displayed if UpdateUrl has been assinged a value.
		/// </remarks>
		public override bool Validate( )
		{
			if( EndDate.Ticks > 0 && DateTime.Now > EndDate )
			{
				if( base.License != null )
				{
					StringBuilder errStr = new StringBuilder( );
					errStr.Append( "The beta period has expired." );
					if( this.UpdateURL != String.Empty )
					{
						errStr.Append( "\n" );
						errStr.Append( "You may get an update at: " );
						errStr.Append( "\n" );
						errStr.Append( this.UpdateURL );
					}

					base.License.FailureReason = errStr.ToString( );
				}

				return false;
			}

			if( base.License != null )
				base.License.FailureReason = String.Empty;

			return true;
		}

		/// <summary>
		/// This is responsible for parsing a <see cref="System.String">String</see>
		/// to form the <c>BetaConstriant</c>.
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
		/// This creates a <c>BetaConstraint</c> from an <see cref="System.Xml.XmlNode">XmlNode</see>.
		/// </summary>
		/// <param name="itemsNode">
		/// A <see cref="XmlNode">XmlNode</see> representing the <c>BetaConstraint</c>.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Thrown if the <see cref="XmlNode">XmlNode</see> is null.
		/// </exception>
		public override void FromXml( XmlNode itemsNode )
		{
			if( itemsNode == null )
				throw new ArgumentNullException( "The license data is null." );

			XmlNode nameTextNode			= itemsNode.SelectSingleNode( "Name/text()" );
			XmlNode descriptionTextNode		= itemsNode.SelectSingleNode( "Description/text()" );
			XmlNode endDateTextNode			= itemsNode.SelectSingleNode( "EndDate/text()" );
			XmlNode updateURLTextNode		= itemsNode.SelectSingleNode( "UpdateUrl/text()" );

			if( nameTextNode != null )
				Name 		= nameTextNode.Value;

			if( descriptionTextNode != null )
				Description	= descriptionTextNode.Value;

			if( endDateTextNode != null )
			{
			    EndDate = MetaBuilder.Core.GlobalParser.ParseGlobalisedDateString(endDateTextNode.Value);
				//EndDate		= Convert.ToDateTime( endDateTextNode.Value );
			}
			
			if( updateURLTextNode != null )
				UpdateURL		= updateURLTextNode.Value;
		}

		/// <summary>
		/// Converts this <c>BetaConstraint</c> to an Xml <c>String</c>.
		/// </summary>
		/// <returns>
		/// A <c>String</c> representing the BetaConstraint as Xml data.
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
				xmlWriter.WriteElementString( "UpdateUrl",			this.UpdateURL );
				if( EndDate.Ticks != 0 )
					xmlWriter.WriteElementString( "EndDate",		EndDate.ToString( ) );
				else
					xmlWriter.WriteElementString( "EndDate",		String.Empty );
			xmlWriter.WriteEndElement( ); // Constraint

			xmlWriter.Close( );

			return xmlString.ToString( );
		}

#region Properties
		/// <summary>
		/// Gets or Sets the end date/time for this <see cref="BetaConstraint">BetaConstraint</see>.
		/// </summary>
		/// <param>
		///	Sets the end date/time for this <see cref="BetaConstraint">BetaConstraint</see>.
		/// </param>
		/// <returns>
		///	Gets the end date/time for this <see cref="BetaConstraint">BetaConstraint</see>.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Constraints"),
		DefaultValue(null),
		Description( "Gets or Sets the end date/time for this BetaConstraint." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public DateTime EndDate
		{
			get
			{
				return this.end;
			}
			set
			{
				if( !this.EndDate.Equals( value ) )
				{
					this.end		= value;
					this.isDirty	= true;
				}
			}
		}

		/// <summary>
		/// Gets or Sets the URL, as a <see cref="System.String">String</see>, which
		/// points to where an update can be obtained.
		/// </summary>
		/// <param>
		///	Sets the URL, as a <see cref="System.String">String</see>, which
		/// points to where an update can be obtained.
		/// </param>
		/// <returns>
		///	Gets the URL, as a <see cref="System.String">String</see>, which
		/// points to where an update can be obtained.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Data"),
		DefaultValue(""),
		Description( "Gets or Sets the URL, as a String , which points to where an update can be obtained." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public string UpdateURL
		{
			get
			{
				return this.updateUrl;
			}
			set
			{
				if( !this.UpdateURL.Equals( value ) )
				{
					this.updateUrl	= value;
					this.isDirty	= true;
				}
			}
		}
	}
#endregion
} /* NameSpace OpenLicense */
