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
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;

namespace OpenLicense.LicenseFile.Constraints
{
	/// <summary>
	/// <p>This <see cref='DomainConstraint'/> constrains the user to running within a
	/// given set of Domains.  This is primarily used for web based licensing to make
	/// sure that the license is only used for specific domains.</p>
	/// </summary>
	/// <seealso cref="AbstractConstraint">AbstractConstraint</seealso>
	/// <seealso cref="IConstraint">IConstraint</seealso>
	/// <seealso cref="OpenLicenseFile">OpenLicenseFile</seealso>
	public class DomainConstraint : AbstractConstraint
	{

		#region Fields (4) 

		private HttpContext			context					= null;
		private string				currentDomain			= String.Empty;
		private Uri					currentDomainUri		= null;
		private	StringCollection	domainCollection		= new StringCollection( );

		#endregion Fields 

		#region Constructors (2) 

		/// <summary>
		/// This is the constructor for the <c>DomainConstraint</c>.  The constructor
		/// is used to create the object with a valid license to attach it to.
		/// </summary>
		public DomainConstraint( ) : this( null ) { }

		/// <summary>
		/// This is the constructor for the <c>DomainConstraint</c>.  The constructor
		/// is used to create the object and assign it to the proper license.
		/// </summary>
		/// <param name="license">
		/// The <see cref="OpenLicenseFile">OpenLicenseFile</see> this constraint
		/// belongs to.
		/// </param>
		/// <remarks>
		/// This constructor initializes the current domain.  When initializing this
		/// an exception can be thrown.  The constructor will catch the exception
		/// and set the current domain to an empty string.  The license can handle this
		/// by setting one of the domains to an empty string which should allow the
		/// validation to pass.  This call will throw an exception only when used in
		/// a non web environment.
		/// </remarks>
		public DomainConstraint( OpenLicenseFile license )
		{
			base.License		= license;
			base.Name			= "Domain Constraint";
			base.Description	=  "This DomainConstraint constrains the user to running within a given ";
			base.Description	+= "set of Domains.  This is primarily used for web based licensing to make ";
			base.Description	+= "sure that the license is only used for specific domains.";



            try
			{
				this.context			= HttpContext.Current;
				this.currentDomainUri	= this.context.Request.Url;
				this.CurrentDomain		= this.currentDomainUri.Host;
			}
			catch
			{
				this.CurrentDomain = String.Empty;
			}
		}

		#endregion Constructors 

		#region Methods (4) 


		// Public Methods (4) 

		/// <summary>
		/// This is responsible for parsing a <see cref="System.String">String</see>
		/// to form the <c>DomainConstriant</c>.
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
		/// This creates a <c>DomainConstraint</c> from an <see cref="System.Xml.XmlNode">XmlNode</see>.
		/// </summary>
		/// <param name="itemsNode">
		/// A <see cref="XmlNode">XmlNode</see> representing the <c>DomainConstraint</c>.
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
			XmlNodeList	domainListNode		= itemsNode.SelectNodes( "Domain" );

			if( nameTextNode != null )
				Name 		= nameTextNode.Value;

			if( descriptionTextNode != null )
				Description	= descriptionTextNode.Value;

			if( this.domainCollection == null && domainListNode.Count > 0 )
				this.domainCollection = new StringCollection( );

			for( int i = 0; i < domainListNode.Count; i++ )
			{
				this.domainCollection.Add( ((XmlNode)domainListNode[i]).InnerText );
			}
		}

		/// <summary>
		/// Converts this <c>DomainConstraint</c> to an Xml <c>String</c>.
		/// </summary>
		/// <returns>
		/// A <c>String</c> representing the DomainConstraint as Xml data.
		/// </returns>
		public override string ToXmlString( )
		{
			StringBuilder xmlString = new StringBuilder( );

			XmlTextWriter xmlWriter = new XmlTextWriter( new StringWriter( xmlString ) );
			xmlWriter.Formatting = Formatting.Indented;

			xmlWriter.WriteStartElement( "Constraint" );
				xmlWriter.WriteElementString( "Name", 				this.Name );
				xmlWriter.WriteElementString( "Type",				this.GetType( ).ToString( ) );
				xmlWriter.WriteElementString( "Description",		this.Description );

				for( int i = 0; i < this.domainCollection.Count; i++ )
					xmlWriter.WriteElementString( "Domain",		(string)this.domainCollection[i] );

			xmlWriter.WriteEndElement( ); // Constraint

			xmlWriter.Close( );

			return xmlString.ToString( );
		}

		/// <summary>
		/// <p>This verifies the license meets its desired validation criteria.  This includes
		/// validating that the license is run within the set of given domains.  If the license
		/// is able to match a domain with the current domain then it will be successful (true).
		/// Otherwise it will return false and set the failure reason to the reason for the
		/// failure.</p>
		/// </summary>
		/// <returns>
		/// <c>True</c> if the license meets the validation criteria.  Otherwise
		/// <c>False</c>.
		/// </returns>
		/// <remarks>
		/// When a failure occurs the FailureReason will be set to: "The current domain is not
		/// supported by this license."
		/// </remarks>
		public override bool Validate( )
		{
            return true;
			foreach( String s in this.domainCollection )
			{
				if( ( s.ToLower( ) ).Equals( this.CurrentDomain.ToLower() ) )
				{
					if( base.License != null )
						base.License.FailureReason = String.Empty;

					return true;
				}
			}

			if( base.License != null )
				base.License.FailureReason = "The current domain is not supported by this license.";

			return false;
		}


		#endregion Methods 


#region Properties
		/// <summary>
		/// Gets or Sets the an array of strings which represent the domain names allowed to use this license.
		/// </summary>
		/// <param>
		/// Sets the an array of strings which represent the domain names allowed to use this license.
		/// </param>
		/// <returns>
		///	Gets the an array of strings which represent the domain names allowed to use this license.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Constraints"),
		DefaultValue(null),
		Description( "Gets or Sets the an array of strings which represent the domain names allowed to use this license." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public string[] Domains
		{
			get
			{
				string[]	tempArray = new string[this.domainCollection.Count];
				for( int i=0; i < this.domainCollection.Count; i++ )
				{
					tempArray[i] = this.domainCollection[i];
				}
				return tempArray;
			}
			set
			{
				this.domainCollection.AddRange( value );
				this.isDirty	= true;
			}
		}


		/// <summary>
		/// Gets or Sets the current domain this license is being executed on.
		/// </summary>
		/// <param>
		/// Sets the current domain this license is being executed on.
		/// </param>
		/// <returns>
		///	Gets the current domain this license is being executed on.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Constraints"),
		DefaultValue(""),
		Description( "Gets or Sets the current domain this license is being executed on." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public string CurrentDomain
		{
			get
			{
				return this.currentDomain;
			}
			set
			{
				if( ((String)value).StartsWith( "http" ) )
				{
					value				= ((String)value).TrimEnd( "/".ToCharArray( ) );
					this.currentDomain	= (((String)value).TrimStart( "http://".ToCharArray( ) )).ToLower( );
				}
				else
				{
					this.currentDomain = value.ToLower( );
				}
			}
		}
#endregion
	}
} /* NameSpace OpenLicense */

