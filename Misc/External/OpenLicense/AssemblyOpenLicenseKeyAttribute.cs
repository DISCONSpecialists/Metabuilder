//--------------------------------------------------------------------------------
// Open License - A license manager for .NET software
// Copyright (C) 2004-2006.*
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
using System.Reflection;

namespace OpenLicense
{
	/// <summary>
	/// This is an assembly attribute to be used to defined the encryption key that 
	/// should be used to decrypt the license file when it is encrypted.  If the value 
	/// is defined then the license file will be decrypted using the defined key.  If 
	/// the decrypt fails then an exception will be thrown. 
	/// </summary>
	/// <example>
	/// c#
	/// <code>
	/// &#91;assembly: OpenLicense.AssemblyOpenLicenseKey("test")&#93;
	/// </code>
	/// vb#
	/// <code>
	/// &lt;assembly: OpenLicense.AssemblyOpenLicenseKey("test")&gt;
	/// </code>
	/// </example>
	[
	AttributeUsage( AttributeTargets.Assembly )
	]
	public class AssemblyOpenLicenseKeyAttribute : Attribute
	{

		#region Fields (1) 

		private string	encryptionKey		= String.Empty;

		#endregion Fields 

		#region Constructors (2) 

		/// <summary>
		/// The constructor for an empty <c>AssemblyOpenLicenseKeyAttribute</c>.
		/// </summary>
		public AssemblyOpenLicenseKeyAttribute( ) : this( "" ) {}

		/// <summary>
		/// The constructor for an <c>AssemblyOpenLicenseKeyAttribute</c> with a given encryption key.
		/// </summary>
		public AssemblyOpenLicenseKeyAttribute( string key )
		{
			this.encryptionKey = key;
		}

		#endregion Constructors 

		#region Properties (1) 

		/// <summary>
		/// Gets the Encryption Key to be used for the license.
		/// </summary>
		/// <returns>
		/// Gets the Encryption Key to be used for the license.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Data"),
		DefaultValue(""),
		Description( "Gets the Encryption Key to be used for the license." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public string EncryptionKey
		{
			get
			{
				return this.encryptionKey;	
			}
		}

		#endregion Properties 

		#region Methods (1) 


		// Public Methods (1) 

		/// <summary>
		/// This is responsible for returning the encryption key defined in 
		/// the Assembly.  If not defined then an Empty string is returned.
		/// </summary>
		/// <param name="type">
		/// The object type being licensed.
		/// </param>
		public static string GetEncryptionKeyAttribute( Type type )
		{
			string key	= String.Empty;
			
			// Look for key defined in Assembly.
			AssemblyOpenLicenseKeyAttribute attr =
				(AssemblyOpenLicenseKeyAttribute)Attribute.GetCustomAttribute(
					Assembly.GetAssembly( type ),
					Type.GetType( "OpenLicense.AssemblyOpenLicenseKeyAttribute" ) );
			
			if( attr != null )
				return attr.EncryptionKey;	
			else
				return String.Empty;
		}


		#endregion Methods 

	}
} /* NameSpace OpenLicense */

