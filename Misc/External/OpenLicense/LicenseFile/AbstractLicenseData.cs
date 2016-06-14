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

using System.ComponentModel;

namespace OpenLicense.LicenseFile
{
	/// <summary>
	/// <p><c>AbstractLicenseData</c> is an abstract class which all licensing
	/// data, other then Constraints) must inherit.  The <c>AbstractLicenseData</c>
	/// defines the necessary items for any license data to be used by
	/// the <see cref="OpenLicenseFile">OpenLicenseFile</see>.</p>
	/// </summary>
	/// <seealso cref="ILicenseData">ILicenseData</seealso>
	/// <seealso cref="OpenLicenseFile">OpenLicenseFile</seealso>
	/// <seealso cref="System.String">String</seealso>
	/// <seealso cref="System.Xml.XmlDocument">XmlDocument</seealso>
	/// <seealso cref="System.Xml.XmlNode">XmlNode</seealso>
	public abstract class AbstractLicenseData : ILicenseData
	{

		#region Fields (1) 

		/// <summary>
		/// Used to denote if this object has changed and hasn't yet been saved.
		/// </summary>
		protected bool	isDirty = false;

		#endregion Fields 

		#region Properties (1) 

		/// <summary>
		/// Gets if this object has changed since the last save.
		/// </summary>
		/// <returns>
		/// Gets if this object has changed since the last save.
		/// </returns>
		[
		Bindable(false),
		Browsable(false),
		Category("Data"),
		DefaultValue(false),
		Description( "Gets if this object has changed since the last save." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public bool IsDirty
		{
			get
			{
				return this.isDirty;
			}
		}

		#endregion Properties 

		#region Methods (2) 


		// Public Methods (2) 

		///	<summary>
		/// Resets the is dirty flag to know this item has been saved.
		/// </summary>
		public void Saved( )
		{
			this.isDirty = false;
		}

		/// <summary>
		/// Converts this instance of License Data to a
		/// <see cref="System.String">String</see> representing the XML
		/// of the specific License Data object.
		/// </summary>
		/// <return>
		/// The <c>String</c> representing this License Data.
		/// </return>
		public abstract string ToXmlString( );


		#endregion Methods 

	}
} /* NameSpace OpenLicense */

