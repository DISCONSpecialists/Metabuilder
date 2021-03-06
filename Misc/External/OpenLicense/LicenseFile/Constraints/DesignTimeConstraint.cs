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

namespace OpenLicense.LicenseFile.Constraints
{
	/// <summary>
	/// <p>This <see cref='DesigntimeConstraint'/> constrains the user to only running
	/// the license in a design time environment.  If it is not in a Design Time
	/// environment then this constraint will fail.</p>
	/// </summary>
	/// <seealso cref="AbstractConstraint">AbstractConstraint</seealso>
	/// <seealso cref="IConstraint">IConstraint</seealso>
	/// <seealso cref="OpenLicenseFile">OpenLicenseFile</seealso>
	public class DesigntimeConstraint : AbstractConstraint
	{

		#region Constructors (2) 

		/// <summary>
		/// This is the constructor for the <c>DesigntimeConstraint</c>.  The constructor
		/// is used to create the object with a valid license to attach it to.
		/// </summary>
		public DesigntimeConstraint( ) : this( null ) { }

		/// <summary>
		/// This is the constructor for the <c>DesigntimeConstraint</c>.  The constructor
		/// is used to create the object and assign it to the proper license.
		/// </summary>
		/// <param name="license">
		/// The <see cref="OpenLicenseFile">OpenLicenseFile</see> this constraint
		/// belongs to.
		/// </param>
		public DesigntimeConstraint( OpenLicenseFile license )
		{
			base.License		= license;
			base.Name			= "Design Time Constraint";
			base.Description	=  "This DesigntimeConstraint constrains the user to only running ";
			base.Description	+= "the license in a design time environment.  If it is not in a ";
			base.Description	+= "Design Time environment then this constraint will fail.";
		}

		#endregion Constructors 

		#region Methods (4) 


		// Public Methods (4) 

		/// <summary>
		/// This is responsible for parsing a <see cref="System.String">String</see>
		/// to form the <c>DesigntimeConstraint</c>.
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
		/// This creates a <c>DesigntimeConstraint</c> from an <see cref="System.Xml.XmlNode">XmlNode</see>.
		/// </summary>
		/// <param name="itemsNode">
		/// A <see cref="XmlNode">XmlNode</see> representing the <c>DesigntimeConstraint</c>.
		/// </param>
		/// <exception cref="ArgumentException">
		/// Thrown if the <see cref="XmlNode">XmlNode</see> is null.
		/// </exception>
		public override void FromXml( XmlNode itemsNode )
		{
			if( itemsNode == null )
				throw new ArgumentNullException( "The license data is null." );

			XmlNode nameTextNode		= itemsNode.SelectSingleNode( "Name/text()" );
			XmlNode descriptionTextNode	= itemsNode.SelectSingleNode( "Description/text()" );

			if( nameTextNode != null )
				this.Name		= nameTextNode.Value;

			if( descriptionTextNode != null )
				Description	= descriptionTextNode.Value;
		}

		/// <summary>
		/// Converts this <c>DesigntimeConstraint</c> to an Xml <c>String</c>.
		/// </summary>
		/// <returns>
		/// A <c>String</c> representing the DesigntimeConstraint as Xml data.
		/// </returns>
		public override string ToXmlString( )
		{
			StringBuilder xmlString = new StringBuilder( );

			XmlTextWriter xmlWriter = new XmlTextWriter( new StringWriter( xmlString ) );
			xmlWriter.Formatting = Formatting.Indented;
			xmlWriter.IndentChar = '\t';
			xmlWriter.WriteStartElement( "Constraint" );
				xmlWriter.WriteElementString( "Name", 			this.Name );
				xmlWriter.WriteElementString( "Type", 			this.GetType( ).ToString( ) );
				xmlWriter.WriteElementString( "Description",	this.Description );
			xmlWriter.WriteEndElement( ); // constraint

			xmlWriter.Close( );

			return xmlString.ToString( );
		}

		/// <summary>
		/// <p>This verifies the license meets its desired validation criteria.  This includes
		/// validating that the license is being used in the context of Design Time. If it is
		/// not then the license validation will return false and the failure reason will be
		/// set.</p>
		/// </summary>
		/// <returns>
		/// <c>True</c> if the license meets the validation criteria.  Otherwise
		/// <c>False</c>.
		/// </returns>
		/// <remarks>
		/// When a failure occurs the FailureReason will be set to: "The license may only be used
		/// in a Design Time environment.  Runtime licensing is not supported."
		/// </remarks>
		public override bool Validate( )
		{
			if( base.License != null )
				base.License.FailureReason = String.Empty;

			if( LicenseManager.CurrentContext.UsageMode != LicenseUsageMode.Designtime )
			{
				if( base.License != null )
					base.License.FailureReason = "The license may only be used in a Design Time environment.  Runtime licensing is not supported.";

				return false;
			}
			else
			{
				return true;
			}
		}


		#endregion Methods 

	}
} /* NameSpace OpenLicense */
