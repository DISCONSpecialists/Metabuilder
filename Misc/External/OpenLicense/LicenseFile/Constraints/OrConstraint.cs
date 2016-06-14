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
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace OpenLicense.LicenseFile.Constraints
{
	/// <summary>
	/// <p>This <see cref='OrConstraint'/> contains a collection of constraints that 
	/// will be grouped together as a bitwise OR operation.  It is responsible for 
	/// validating the containing <c>IConstraints</c> and will be valid as long as 
	/// one of the constraints contained is valid.  The purpose of this is to allow 
	/// multiple constraints to be added and create a run as long as one is valid 
	/// scheme.</p>
	/// </summary>
	/// <seealso cref="AbstractConstraint">AbstractConstraint</seealso>
	/// <seealso cref="IConstraint">IConstraint</seealso>
	/// <seealso cref="OpenLicenseFile">OpenLicenseFile</seealso>
	public class OrConstraint : AbstractContainerConstraint
	{

		#region Constructors (2) 

		/// <summary>
		/// This is the constructor for the <c>OrConstraint</c>.  The constructor
		/// is used to create the object without a valid license attached it to.
		/// </summary>
		public OrConstraint( ) : this( null ) { }

		/// <summary>
		/// This is the constructor for the <c>OrConstraint</c>.  The constructor
		/// is used to create the object and assign it to the proper license.
		/// </summary>
		/// <param name="license">
		/// The <see cref="OpenLicenseFile">OpenLicenseFile</see> this constraint
		/// belongs to.
		/// </param>
		public OrConstraint( OpenLicenseFile license )
		{
			base.License		= license;
			base.Name			= "Or Constraint";
			base.Description	=  "This OrConstraint contains a collection of constraints that will ";
			base.Description	+= "be grouped together as a bitwise OR operation.  It is responsible ";
			base.Description	+= "for validating the containing IConstraints and will be valid as ";
			base.Description	+= "long as one of the constraints contained is valid.  The purpose of ";
			base.Description	+= "this is to allow multiple constraints to be added and create a run ";
			base.Description	+= "as long as one is valid scheme.";
		}

		#endregion Constructors 

		#region Methods (5) 


		// Public Methods (4) 

		/// <summary>
		/// This is responsible for parsing a <see cref="System.String">String</see>
		/// to form the <c>OrConstriant</c>.
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
		/// This creates an <c>OrConstraint</c> from an <see cref="System.Xml.XmlNode">XmlNode</see>.
		/// </summary>
		/// <param name="itemsNode">
		/// A <see cref="XmlNode">XmlNode</see> representing the <c>OrConstraint</c>.
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

			if( nameTextNode != null )
				Name 		= nameTextNode.Value;

			if( descriptionTextNode != null )
				Description	= descriptionTextNode.Value;

			parseConstraintsFields( itemsNode );
		}

		/// <summary>
		/// Converts this <c>OrConstraint</c> to an Xml <c>String</c>.
		/// </summary>
		/// <returns>
		/// A <c>String</c> representing the IConstraint as Xml data.
		/// </returns>
		public override string ToXmlString( )
		{
			StringBuilder xmlString = new StringBuilder( );

			XmlTextWriter xmlWriter = new XmlTextWriter( new StringWriter( xmlString ) );
			xmlWriter.Formatting = Formatting.Indented;

			if( this.Items != null && this.Items.Count > 0 )
			{
				xmlWriter.WriteStartElement( "Constraint" );
				xmlWriter.WriteElementString( "Name", 				this.Name );
				xmlWriter.WriteElementString( "Type", 				this.GetType( ).ToString( ) );
				xmlWriter.WriteElementString( "Description",		this.Description );
				for( int i = 0; i < this.Items.Count; i++ )
				{
					xmlWriter.WriteRaw( ( (IConstraint)Items[i] ).ToXmlString( ) );
				}
				xmlWriter.WriteEndElement( ); // constraints
			}

			xmlWriter.Close( );

			return xmlString.ToString( );
		}

		/// <summary>
		/// This verifies the license meets its desired validation criteria.  This test will 
		/// pass as long as one of the constraints contained within it's collection is valid.
		/// If all of them are invalid, then the validate will fail and the license failure 
		/// reason will be set to the last constraint which failed while being validated.
		/// </summary>
		/// <returns>
		/// <c>True</c> if the license meets the validation criteria.  Otherwise
		/// <c>False</c>.
		/// </returns>
		/// <remarks>
		/// When a failure occurs the FailureReason will be set to the details of the
		/// failure for the last constraint which failed.
		/// </remarks>
		public override bool Validate( )
		{
			for( int i = 0; i < this.Items.Count; i++ )
			{
				if( ((IConstraint)this.Items[i]).Validate( ) )
				{
					if( base.License != null )
						base.License.FailureReason = String.Empty;

					return true;
				}
			}

			return false;
		}



		// Private Methods (1) 

		/// <summary>
		/// Parse the XML content of the constraints group/fields section of the license.
		/// </summary>
		/// <param name="itemsNode">
		/// A <see cref="System.Xml.XmlNode">XmlNode</see> representing the
		/// Constraints List (System.Collections.Generic.List) section of the
		/// license.
		/// </param>
		private void parseConstraintsFields( XmlNode itemsNode )
		{
			// Check if custom fields are defined
			if( itemsNode == null )
				return;

			// If they are then process all of them
			XmlNodeList constraints = itemsNode.ChildNodes;

			for( int i = 0; i < constraints.Count; i++ )
			{
				XmlNode constraint		= constraints[i];
				XmlNode typeTextNode	= constraint.SelectSingleNode( "Type/text()" );

				if( typeTextNode != null )
				{
					Type constraintType = Type.GetType( (String)typeTextNode.Value , false, true );
					ConstructorInfo cInfo = constraintType.GetConstructor( new Type[] {typeof( OpenLicenseFile )} );
					IConstraint c = (IConstraint)cInfo.Invoke( new Object[] { this.License } );
					c.FromXml( constraint );
					this.Items.Add( c );
				}
			}
		}


		#endregion Methods 

	}
} /* NameSpace OpenLicense */
