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
using System.Drawing;
using System.Xml;

namespace OpenLicense.LicenseFile
{
	/// <summary>
	/// <p><c>AbstractConstraint</c> is an abstract class which all licensing
	/// constraints are built from.  The <c>AbstractConstraint</c> defines
	/// the necessary items for a Constraint to be used by
	/// the <see cref="OpenLicenseProvider">OpenLicenseProvider</see>.  The
	/// provider then uses the constraints <c>Validate</c> function to
	/// enforce the Constraint.</p>
	/// </summary>
	/// <seealso cref="IConstraint">IConstraint</seealso>
	/// <seealso cref="OpenLicenseFile">OpenLicenseFile</seealso>
	/// <seealso cref="OpenLicenseProvider">OpenLicenseProvider</seealso>
	/// <seealso cref="System.String">String</seealso>
	/// <seealso cref="System.Xml.XmlDocument">XmlDocument</seealso>
	/// <seealso cref="System.Xml.XmlNode">XmlNode</seealso>
	public abstract class AbstractConstraint : AbstractLicenseData, IConstraint
	{

		#region Fields (4) 

		private string				description		= String.Empty;
		private Icon				icon			= null;
		private	OpenLicenseFile		license			= null;
		private	string				name			= String.Empty;

		#endregion Fields 

		#region Methods (4) 


		// Public Methods (4) 

		/// <summary>
		/// Destroys this instance of the Constraint.
		/// </summary>
		public void Dispose( )
		{
		}

		/// <summary>
		/// This is used to create the Constraint from a
		/// <see cref="System.String">String</see> representing the Xml data
		/// of a constraint node.
		/// </summary>
		/// <param name="xmlData">
		/// A <c>String</c> representing the XML data for this Constraint.
		/// </param>
		public abstract void FromXml( string xmlData );

		/// <summary>
		/// This loads the XML data for the Constraint from an
		/// <see cref="System.Xml.XmlNode">XmlNode</see>.  The <c>XmlNode</c>
		/// is the piece of the <see cref="System.Xml.XmlDocument">XmlDocument</see>
		/// that is contained within the constraint block of the
		/// <c>XmlDocument</c>.
		/// </summary>
		/// <param name="xmlData">
		/// A <c>XmlNode</c> representing the constraint
		/// of the <c>XmlDocument</c>.
		/// </param>
		public abstract void FromXml( XmlNode xmlData );

		/// <summary>
		/// Defines the validation method of this Constraint.  This
		/// is the method the Constraint uses to accept or reject a
		/// license request.
		/// </summary>
		public abstract bool Validate( );


		#endregion Methods 


#region Properties
		/// <summary>
		/// Gets or Sets <see cref="OpenLicenseFile">OpenLicenseFile</see> this constraint belongs to.
		/// </summary>
		/// <param>
		///	Sets the <c>OpenLicenseFile</c> this constraint belongs to.
		/// </param>
		/// <returns>
		///	Gets the <c>OpenLicenseFile</c> this constraint belongs to.
		/// </returns>
		[
		Bindable(false),
		Browsable(false),
		DefaultValue(0),
		Description( "Gets or Sets OpenLicenseFile this constraint belongs to." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public OpenLicenseFile License
		{
			get
			{
				return this.license;
			}
			set
			{
				this.license = value;
			}
		}
		
		/// <summary>
		/// Gets or Sets the name of this constraint.
		/// </summary>
		/// <param>
		///	Sets the name of this constraint.
		/// </param>
		/// <returns>
		///	Gets the name of this constraint.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Constraints"),
		DefaultValue(""),
		Description( "Gets or Sets the name of this constraint." ),
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
				this.name = value;
			}
		}

		/// <summary>
		/// Gets or Sets the description of this constraint.
		/// </summary>
		/// <param>
		///	Sets the description of this constraint.
		/// </param>
		/// <returns>
		///	Gets the description of this constraint.
		/// </returns>
		[
		Bindable(false),
		Browsable(false),
		Category("Constraints"),
		DefaultValue(""),
		Description( "Gets or Sets the description of this constraint." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
		ReadOnly(true)
		]
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		/// <summary>
		/// Gets or Sets the icon for this constraint.
		/// </summary>
		/// <param>
		///	Sets the icon for this constraint.
		/// </param>
		/// <returns>
		///	Gets the icon for this constraint.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Constraints"),
		DefaultValue(null),
		Description( "Gets or Sets the icon for this constraint." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public Icon Icon
		{
			get
			{
				return this.icon;
			}
			set
			{
				this.icon = value;
			}
		}
#endregion //Properties

	}
} /* NameSpace OpenLicense */
