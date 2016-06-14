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

namespace OpenLicense.LicenseFile
{
	/// <summary>
	/// <p><c>ILicenseData</c> is an interface class which all licensing
	/// data, other then Constraints) must inherit.  The <c>ILicenseData</c>
	/// defines the necessary items for any license data to be used by
	/// the <see cref="OpenLicenseFile">OpenLicenseFile</see>.</p>
	/// </summary>
	/// <seealso cref="OpenLicenseFile">OpenLicenseFile</seealso>
	/// <seealso cref="System.String">String</seealso>
	/// <seealso cref="System.Xml.XmlDocument">XmlDocument</seealso>
	/// <seealso cref="System.Xml.XmlNode">XmlNode</seealso>
	public interface ILicenseData
	{
		/// <summary>
		/// Converts this instance of License Data to a <see cref="System.String">String</see>
		/// representing the Xml of the specific License Data object.
		/// </summary>
		/// <return>
		/// The <c>String</c> representing this License Data.
		/// </return>
		string ToXmlString( );

#region Properties
		/// <summary>
		/// Gets if the license data has changed since the last save.
		/// </summary>
		/// <returns>
		/// Gets if the license data has changed since the last save.
		/// </returns>
		bool IsDirty
		{
			get;
		}

		///	<summary>
		/// Resets the IsDirty flag to know this item has been saved.
		/// </summary>
		void Saved( );
#endregion
	}
} /* NameSpace OpenLicense */

