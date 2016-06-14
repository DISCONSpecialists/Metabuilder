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
using System.IO;
using System.Text;
using System.Xml;

namespace OpenLicense.LicenseFile
{
	/// <summary>
	/// The <c>Issuer</c> object inherits from the <see cref="AbstractLicenseData"/>.  The
	/// <c>Issuer</c> is used as the person who released the license.  The issue contains
	/// the person's name, email address and URL.
	/// </summary>
	/// <seealso cref="OpenLicenseFile">OpenLicenseFile</seealso>
	/// <seealso cref="AbstractLicenseData">AbstractLicenseData</seealso>
	public class Issuer : AbstractLicenseData
	{

		#region Fields (3) 

		private string		email		= String.Empty;
		private string		fullName	= String.Empty;
		private string		url			= String.Empty;

		#endregion Fields 

		#region Constructors (4) 

		/// <summary>
		/// This initializes an <c>Issuer</c> with a given name, email and URL.
		/// </summary>
		/// <param name="name">
		/// The name of the person or company issuing the license.
		/// </param>
		/// <param name="email">
		/// The email of the person or company issuing the license. Generally this is a support email address.
		/// </param>
		/// <param name="url">
		/// The url of the person or company issuing the license. Generally this is the company's web address.
		/// </param>
		public Issuer( string name, string email, string url )
		{
			this.fullName	= name;
			this.email		= email;
			this.url		= url;
		}

		/// <summary>
		/// This initializes an <c>Issuer</c> with a given name and email.
		/// </summary>
		/// <param name="name">
		/// The name of the person or company issuing the license.
		/// </param>
		/// <param name="email">
		/// The email of the person or company issuing the license. Generally this is a support email address.
		/// </param>
		public Issuer( string name, string email )	: this( name, email, String.Empty ) { }

		/// <summary>
		/// This initializes an empty <c>Issuer</c>.
		/// </summary>
		public Issuer( )							: this( String.Empty, String.Empty, String.Empty ) { }

		/// <summary>
		/// This initializes an <c>Issuer</c> with a given name.
		/// </summary>
		/// <param name="name">
		/// The name of the person or company issuing the license.
		/// </param>
		public Issuer( string name )				: this( name, String.Empty, String.Empty ) { }

		#endregion Constructors 

		#region Properties (3) 

		/// <summary>
		/// Gets or Sets the email address of the <c>Issuer</c> for this license.
		/// </summary>
		/// <param>
		///	Gets the email address of the <c>Issuer</c> for this license.
		/// </param>
		/// <returns>
		/// Sets the email address of the <c>Issuer</c> for this license.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Data"),
		DefaultValue(""),
		Description( "Gets or Sets the email address of the Issuer for this license." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public string Email
		{
			get
			{
				return this.email;
			}
			set
			{
				if( this.email != value )
				{
					this.email		= value;
					this.isDirty	= true;
				}
			}
		}

		/// <summary>
		/// Gets or Sets the name of the <c>Issuer</c> of this license.
		/// </summary>
		/// <param>
		///	Gets the name of the <c>Issuer</c> of this license.
		/// </param>
		/// <returns>
		/// Sets the name of the <c>Issuer</c> of this license.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Data"),
		DefaultValue(""),
		Description( "Gets or Sets the name of the Issuer of this license." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public string FullName
		{
			get
			{
				return this.fullName;
			}
			set
			{
				if( this.fullName != value )
				{
					this.fullName	= value;
					this.isDirty	= true;
				}
			}
		}

		/// <summary>
		/// Gets or Sets the URL of the <c>Issuer</c> for this license.
		/// </summary>
		/// <param>
		///	Sets the URL of the <c>Issuer</c> for this license.
		/// </param>
		/// <returns>
		/// Gets the URL of the <c>Issuer</c> for this license.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Data"),
		DefaultValue(""),
		Description( "Gets or Sets the URL of the Issuer for this license." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public string Url
		{
			get
			{
				return this.url;
			}
			set
			{
				if( this.url != value )
				{
				   	this.url		= value;
					this.isDirty	= true;
				}
			}
		}

		#endregion Properties 

		#region Methods (3) 


		// Public Methods (3) 

		/// <summary>
		/// This is a static method that creates an <c>Issuer</c> from the passed in XML
		/// <see cref="System.String">String</see>.
		/// </summary>
		/// <param>
		/// The <see cref="System.String">String</see> representing the Xml data.
		/// </param>
		/// <returns>
		/// The <c>Issuer</c> created from parsing this <see cref="System.String">String</see>.
		/// </returns>
		public static Issuer FromXml( string xmlData )
		{
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml( xmlData );

			return FromXml( xmlDoc.SelectSingleNode("/Issuer") );
		}

		/// <summary>
		/// This is a static method that creates an <c>Issuer</c> from a <see cref="XmlNode">XmlNode</see>.
		/// </summary>
		/// <param>
		/// A <see cref="XmlNode">XmlNode</see> representing the <c>Issuer</c>.
		/// </param>
		/// <returns>
		/// The <c>Issuer</c> created from this <see cref="XmlNode">XmlNode</see>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// Thrown if the license data is null.
		/// </exception>
		public static Issuer FromXml( XmlNode itemsNode )
		{
			if( itemsNode == null )
				throw new ArgumentNullException( "The license data is null." );

			string fullName	= String.Empty;
			string url		= String.Empty;
			string email	= String.Empty;

			XmlNode nameTextNode	= itemsNode.SelectSingleNode( "FullName/text()" );
			XmlNode emailTextNode	= itemsNode.SelectSingleNode( "Email/text()" );
			XmlNode urlTextNode		= itemsNode.SelectSingleNode( "Url/text()" );

			if( nameTextNode != null )
				fullName = nameTextNode.Value;

			if( emailTextNode != null )
				email = emailTextNode.Value;

			if( urlTextNode != null )
				url = urlTextNode.Value;

			return new Issuer( fullName, email, url );
		}

		/// <summary>
		/// This creates a <see cref="System.String">String</see> representing the
		/// XML form for this <c>Issuer</c>.
		/// </summary>
		/// <returns>
		/// The <see cref="System.String">String</see> representing this <c>Issuer</c> in it's XML form.
		/// </returns>
		public override string ToXmlString( )
		{
			StringBuilder	xmlString	= new StringBuilder( );
			XmlTextWriter	xmlWriter	= new XmlTextWriter( new StringWriter( xmlString ) );

			xmlWriter.Formatting = Formatting.Indented;
			xmlWriter.IndentChar = '\t';

			xmlWriter.WriteStartElement( "Issuer" );
				xmlWriter.WriteElementString( "FullName", 	this.FullName );
				xmlWriter.WriteElementString( "Email", 		this.Email );
				xmlWriter.WriteElementString( "Url",		this.Url );
			xmlWriter.WriteEndElement( ); // Issuer
			xmlWriter.Close( );

			return xmlString.ToString( );
		}


		#endregion Methods 

	}
}
