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

using System.Drawing;
using System.Xml;

namespace OpenLicense.LicenseFile
{
	/// <summary>
	/// <p><c>IConstraint</c> is an interface class which all Constraints must inherit.  The
	/// <c>IConstraint</c> defines the necessary items for a Constraint to be used by the
	/// <see cref="OpenLicenseProvider">OpenLicenseProvider</see>.  The provider then uses
	/// the Constraint's Validate( ) function to enforce the Constraint.</p>
	/// </summary>
	/// <seealso cref="OpenLicenseFile">OpenLicenseFile</seealso>
	/// <seealso cref="OpenLicenseProvider">OpenLicenseProvider</seealso>
	/// <seealso cref="System.String">String</seealso>
	/// <seealso cref="System.Xml.XmlDocument">XmlDocument</seealso>
	/// <seealso cref="System.Xml.XmlNode">XmlNode</seealso>
	public interface IConstraint
	{
		/// <summary>
		/// Defines the validation method of this Constraint.  This is the method the
		/// Constraint uses to accept or reject a license request.
		/// </summary>
		bool Validate( );

		/// <summary>
		/// This is used to create a Constraint from a <see cref="System.String">String</see>
		/// representing the Xml data of a Constraints node in the <see cref="OpenLicenseFile"/>.
		/// </summary>
		/// <param name="xmlData">
		/// A <c>String</c> representing the XML data for this Constraint.
		/// </param>
		void FromXml( string xmlData );

		/// <summary>
		/// This loads the XML data for a Constraint from an <see cref="System.Xml.XmlNode">XmlNode</see>.
		/// The <c>XmlNode</c> is the piece of the <see cref="System.Xml.XmlDocument">XmlDocument</see>
		/// that is contained within the Constraint block of the <c>XmlDocument</c>.
		/// </summary>
		/// <param name="xmlData">
		/// A <c>XmlNode</c> representing the Constraint of the <c>XmlDocument</c>.
		/// </param>
		void FromXml( XmlNode xmlData );

		/// <summary>
		/// Destroys this instance of the Constraint.
		/// </summary>
		void Dispose( );

		/// <summary>
		/// Converts this instance of the Constraint to a <see cref="System.String">String</see>
		/// representing the Xml of the Constraint object.
		/// </summary>
		/// <return>
		/// The <c>String</c> representing this Constraint.
		/// </return>
		string ToXmlString( );

#region Properties
		/// <summary>
		/// Gets or Sets the <see cref="OpenLicenseFile">OpenLicenseFile</see> this Constraint belongs to.
		/// </summary>
		/// <param>
		///	Sets the <c>OpenLicenseFile</c> this Constraint belongs to.
		/// </param>
		/// <returns>
		///	Gets the <c>OpenLicenseFile</c> this Constraint belongs to.
		/// </returns>
		OpenLicenseFile License
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or Sets the name of this Constraint.
		/// </summary>
		/// <param>
		///	Sets the name of this Constraint.
		/// </param>
		/// <returns>
		///	Gets the name of this Constraint.
		/// </returns>
		string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or Sets the description of this Constraint.
		/// </summary>
		/// <param>
		///	Sets the description of this Constraint.
		/// </param>
		/// <returns>
		///	Gets the description of this Constraint.
		/// </returns>
		string Description
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or Sets the icon for this Constraint.
		/// </summary>
		/// <param>
		///	Sets the icon for this Constraint.
		/// </param>
		/// <returns>
		///	Gets the icon for this Constraint.
		/// </returns>
		Icon Icon
		{
			get;
			set;
		}

		/// <summary>
		/// Gets if this Constraint has changed since the last save.
		/// </summary>
		/// <returns>
		/// Gets if this Constraint has changed since the last save.
		/// </returns>
		bool IsDirty
		{
			get;
		}
#endregion

		///	<summary>
		/// Resets the is dirty flag.
		/// </summary>
		void Saved( );
	}
} /* NameSpace OpenLicense */

