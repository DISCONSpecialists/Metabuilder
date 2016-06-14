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

namespace OpenLicense
{
	/// <summary>
	/// This is an assembly attribute to be used to defined the product and 
	/// company information.  This information will be used when an exception
	/// is thrown because a valid license couldn't be found.  It may also be 
	/// used by a developer to access their information to display in a 
	/// product's about page. 
	/// </summary>
	/// <example>
	/// c#
	/// <code>
	/// &#91;assembly: OpenLicense.AssemblyOpenLicenseHelper("Open License", "SP extreme")&#93;
	/// </code>
	/// vb#
	/// <code>
	/// &lt;assembly: OpenLicense.AssemblyOpenLicenseHelper("Open License", "SP extreme")&gt;
	/// </code>
	/// </example>
	[
	AttributeUsage( AttributeTargets.Assembly )
	]
	public class AssemblyOpenLicenseHelperAttribute : Attribute
	{

		#region Fields (5) 

		private string	company		= String.Empty;
		private string	email		= String.Empty;
		private string	phone		= String.Empty;
		private string	product		= String.Empty;
		private string	url			= String.Empty;

		#endregion Fields 

		#region Constructors (6) 

		/// <summary>
		/// The constructor for an <c>AssemblyOpenLicenseHelperAttribute</c>.
		/// </summary>
		/// <param name="product">
		/// The name of the product this licensing scheme is for.
		/// </param>
		/// <param name="company">
		/// The name of the company who developed this product.
		/// </param>
		/// <param name="url">
		/// The URL of the company.
		/// </param>
		/// <param name="email">
		/// An email address to contact the company. Generally this is a support email address.
		/// </param>
		/// <param name="phone">
		/// A phone number to contact the company. Generally this is a support phone number.
		/// </param>
		public AssemblyOpenLicenseHelperAttribute( string product, string company, string url, string email, 
		                                           string phone )
		{
			this.Product	= product;
			this.Company	= company;
			this.Url		= url;
			this.Email		= email;
			this.Phone		= phone;
		}

		/// <summary>
		/// The constructor for an <c>AssemblyOpenLicenseHelperAttribute</c>.
		/// </summary>
		/// <param name="product">
		/// The name of the product this licensing scheme is for.
		/// </param>
		/// <param name="company">
		/// The name of the company who developed this product.
		/// </param>
		/// <param name="url">
		/// The URL of the company.
		/// </param>
		/// <param name="email">
		/// An email address to contact the company. Generally this is a support email address.
		/// </param>
		public AssemblyOpenLicenseHelperAttribute( string product, string company, string url, string email ) : 
				this( product, company, url, email, String.Empty ) { }

		/// <summary>
		/// The constructor for an <c>AssemblyOpenLicenseHelperAttribute</c>.
		/// </summary>
		/// <param name="product">
		/// The name of the product this licensing scheme is for.
		/// </param>
		/// <param name="company">
		/// The name of the company who developed this product.
		/// </param>
		/// <param name="url">
		/// The URL of the company.
		/// </param>
		public AssemblyOpenLicenseHelperAttribute( string product, string company, string url ) : 
				this( product, company, url, String.Empty, String.Empty ) { }

		/// <summary>
		/// The constructor for an <c>AssemblyOpenLicenseHelperAttribute</c>.
		/// </summary>
		/// <param name="product">
		/// The name of the product this licensing scheme is for.
		/// </param>
		/// <param name="company">
		/// The name of the company who developed this product.
		/// </param>
		public AssemblyOpenLicenseHelperAttribute( string product, string company ) : 
				this( product, company, String.Empty, String.Empty, String.Empty ) { }

		/// <summary>
		/// The constructor for an empty <c>AssemblyOpenLicenseHelperAttribute</c>.
		/// </summary>
		public AssemblyOpenLicenseHelperAttribute( ) : 
				this( "", String.Empty, String.Empty, String.Empty, String.Empty ) { }

		/// <summary>
		/// The constructor for an <c>AssemblyOpenLicenseHelperAttribute</c>.
		/// </summary>
		/// <param name="product">
		/// The name of the product this licensing scheme is for.
		/// </param>
		public AssemblyOpenLicenseHelperAttribute( string product ) : 
				this( product, String.Empty, String.Empty, String.Empty, String.Empty ) { }

		#endregion Constructors 


#region Properties
		/// <summary>
		/// Gets or Sets the name of the product this licensing scheme is for.
		/// </summary>
		/// <param>
		/// Sets the name of the product this licensing scheme is for.
		/// </param>
		/// <returns>
		/// Gets the name of the product this licensing scheme is for.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Data"),
		DefaultValue(""),
		Description( "Gets or Sets the name of the product this licensing scheme is for." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public string Product
		{
			get
			{
				return this.product;
			}
			set
			{
				this.product = value;
			}
		}

		/// <summary>
		/// Gets or Sets the name of the company who developed this product.
		/// </summary>
		/// <param>
		/// Sets the name of the company who developed this product.
		/// </param>
		/// <returns>
		/// Gets the name of the company who developed this product.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Data"),
		DefaultValue(""),
		Description( "Gets or Sets the name of the company who developed this product." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public string Company
		{
			get
			{
				return this.company;
			}
			set
			{
				this.company = value;
			}
		}

		/// <summary>
		/// Gets or Sets the URL of the company's web address who developed this product.
		/// </summary>
		/// <param>
		/// Sets the URL of the company's web address who developed this product.
		/// </param>
		/// <returns>
		/// Gets the URL of the company's web address who developed this product.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Data"),
		DefaultValue(""),
		Description( "Gets or Sets the URL of the company's web address who developed this product." ),
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
				this.url = value;
			}
		}

		/// <summary>
		/// Gets or Sets an email address to contact the company. Generally this is a support email address.
		/// </summary>
		/// <param>
		/// Gets an email address to contact the company.
		/// </param>
		/// <returns>
		/// Gets an email address to contact the company.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Data"),
		DefaultValue(""),
		Description( "Gets or Sets an email address to contact the company." ),
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
				this.email = value;
			}
		}

		/// <summary>
		/// Gets or Sets phone number to contact the company. Generally this is a support phone number.
		/// </summary>
		/// <param>
		/// Sets phone number to contact the company. 
		/// </param>
		/// <returns>
		/// Gets phone number to contact the company.
		/// </returns>
		[
		Bindable(false),
		Browsable(true),
		Category("Data"),
		DefaultValue(""),
		Description( "Gets or Sets phone number to contact the company." ),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public string Phone
		{
			get
			{
				return this.phone;
			}
			set
			{
				this.phone = value;
			}
		}
#endregion
	}
} /* NameSpace OpenLicense */

