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
	/// The <c>User</c> object inherits from the <see cref="AbstractLicenseData"/>.  The
	/// <c>User</c> is the information about the owner of the license.  It
	/// contains the name, email and organization of the individual or company who purchased
	/// the license.
	/// </summary>
	/// <seealso cref="OpenLicenseFile">OpenLicenseFile</seealso>
	/// <seealso cref="AbstractLicenseData">AbstractLicenseData</seealso>
	public class User : AbstractLicenseData
	{

		#region Fields (3) 

		private string		email			= String.Empty;
		private string		name			= String.Empty;
		private string		organization	= String.Empty;

		#endregion Fields 

		#region Constructors (4) 

		/// <summary>
		/// This initializes a <c>User</c> object with the passed in values.
		/// </summary>
		/// <param name="name">
		/// The name of the <c>User</c>.
		/// </param>
		/// <param name="email">
		/// The email address of the <c>User</c>.
		/// </param>
		/// <param name="organization">
		/// The organization the <c>User</c> belongs to.
		/// </param>
		public User( string name, string email, string organization )
		{
			this.Name			= name;
			this.Email			= email;
			this.Organization	= organization;
			
			this.isDirty		= false;
		}

		/// <summary>
		/// This initializes a <c>User</c> object with the passed in values.
		/// </summary>
		/// <param name="name">
		/// The name of the <c>User</c>.
		/// </param>
		/// <param name="email">
		/// The email address of the <c>User</c>.
		/// </param>
		public User( string name, string email )	: this( name, email, String.Empty ) { }

		/// <summary>
		/// This initializes an empty <c>User</c> object.
		/// </summary>
		public User( )	: this( String.Empty, String.Empty, String.Empty ) { }

		/// <summary>
		/// This initializes a <c>User</c> object with the passed in values.
		/// </summary>
		/// <param name="name">
		/// The name of the <c>User</c>.
		/// </param>
		public User( String name )	: this( name, String.Empty, String.Empty ) { }

		#endregion Constructors 

		#region Methods (3) 


		// Public Methods (3) 

		/// <summary>
		/// This is a static method that creates an <c>User</c> from the passed in XML
		/// <see cref="System.String">String</see>.
		/// </summary>
		/// <param>
		/// The <see cref="System.String">String</see> representing the Xml data.
		/// </param>
		/// <returns>
		/// The <c>User</c> created from parsing this <see cref="System.String">String</see>.
		/// </returns>
		public static User FromXml( String xmlData )
		{
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml( xmlData );

			return FromXml( xmlDoc.SelectSingleNode("/User") );
		}

		/// <summary>
		/// This is a static method that creates an <c>User</c> from a <see cref="XmlNode">XmlNode</see>.
		/// </summary>
		/// <param>
		/// A <see cref="XmlNode">XmlNode</see> representing the <c>User</c>.
		/// </param>
		/// <returns>
		/// The <c>User</c> created from this <see cref="XmlNode">XmlNode</see>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// Thrown if the license data is null.
		/// </exception>
		public static User FromXml( XmlNode itemsNode )
		{
			if( itemsNode == null )
				throw new ArgumentNullException( "The license data is null." );

			string name			= String.Empty;
			string org			= String.Empty;
			string email		= String.Empty;

			XmlNode nameTextNode			= itemsNode.SelectSingleNode( "Name/text()" );
			XmlNode emailTextNode			= itemsNode.SelectSingleNode( "Email/text()" );
			XmlNode organizationTextNode	= itemsNode.SelectSingleNode( "Organization/text()" );

			if( nameTextNode != null )
				name = nameTextNode.Value;

			if( emailTextNode != null )
				email = emailTextNode.Value;

			if( organizationTextNode != null )
				org = organizationTextNode.Value;

			return new User( name, email, org );
		}

		/// <summary>
		/// This creates a <see cref="System.String">String</see> representing the
		/// XML form for this <c>User</c>.
		/// </summary>
		/// <returns>
		/// The <see cref="System.String">String</see> representing this <c>User</c> in it's XML form.
		/// </returns>
		public override string ToXmlString( )
		{
			StringBuilder	xmlString	= new StringBuilder( );
			XmlTextWriter	xmlWriter	= new XmlTextWriter( new StringWriter( xmlString ) );

			xmlWriter.Formatting = Formatting.Indented;
			xmlWriter.IndentChar = '\t';

			xmlWriter.WriteStartElement( "User" );
				xmlWriter.WriteElementString( "Name", 			this.Name );
				xmlWriter.WriteElementString( "Email", 			this.Email );
				xmlWriter.WriteElementString( "Organization",	this.Organization );
			xmlWriter.WriteEndElement( ); // User
			xmlWriter.Close( );

			return xmlString.ToString( );
		}


		#endregion Methods 


#region Properties
		/// <summary>
		/// Gets or Sets the name of the <c>User</c> who owns this license.
		/// </summary>
		/// <param>
		/// Sets the name of the <c>User</c> who owns this license.
		/// </param>
		/// <returns>
		/// Gets the name of the <c>User</c> who owns this license.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Data"),
		DefaultValue(""),
		Description( "Gets or Sets the name of the User who owns this license." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if( this.name != value )
				{
					this.name		= value;
					this.isDirty	= true;
				}
			}
		}

		/// <summary>
		/// Gets or Sets the email address of the <c>User</c> who owns this license.
		/// </summary>
		/// <param>
		/// Sets the email address of the <c>User</c> who owns this license.
		/// </param>
		/// <returns>
		/// Gets the email address of the <c>User</c> who owns this license.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Data"),
		DefaultValue(""),
		Description( "Gets or Sets the email address of the User who owns this license." ),
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
		/// Gets or Sets the organization/company of the <c>User</c> who owns purchased this license.
		/// </summary>
		/// <param>
		/// Gets the organization/company of the <c>User</c> who owns purchased this license.
		/// </param>
		/// <returns>
		/// Sets the organization/company of the <c>User</c> who owns purchased this license.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Data"),
		DefaultValue(""),
		Description( "Gets or Sets the organization/company of the <c>User</c> who owns purchased this license." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public string Organization
		{
			get
			{
				return this.organization;
			}
			set
			{
				if( this.organization != value )
				{
					this.organization	= value;
					this.isDirty		= true;
				}
			}
		}
#endregion
	}
}
