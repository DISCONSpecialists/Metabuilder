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

using System.Reflection;
using NUnit.Framework;

namespace OpenLicense.LicenseFile.Tests
{
	/// <summary>
	/// 
	/// </summary>
	[TestFixture]
	public class ProductTest
	{

		#region Methods (1) 


		// Public Methods (1) 

		/// <summary>
		/// 
		/// </summary>
		[Test]
		public void TestProductProperties()
		{
			Product p = new Product( Assembly.GetExecutingAssembly( ), false, "./", "OpenLicense.dll",
 			                         "OpenLicense.dll fullname", "1.0.0.0", "SP extreme", "A description", false );
			
			Assert.IsNotNull( p.Assembly );
			Assert.IsTrue( p.Assembly.FullName.IndexOf( "OpenLicense" ) != -1 );
			Assert.AreEqual( "A description", p.Description );
			Assert.AreEqual( "SP extreme", p.Developer );
			Assert.AreEqual( "./", p.FilePath );
			Assert.AreEqual( "OpenLicense.dll fullname", p.FullName );
			Assert.IsFalse( p.IsDirty );
			Assert.IsFalse( p.IsInGAC );
			Assert.IsFalse( p.IsLicensed );
			Assert.AreEqual( "OpenLicense.dll", p.ShortName );
			Assert.AreEqual( "1.0.0.0", p.Version );
		}


		#endregion Methods 

	}
}
