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

using NUnit.Framework;

namespace OpenLicense.LicenseFile.Tests.Tests
{
	/// <summary>
	/// 
	/// </summary>
	[TestFixture]
	public class IssuerTest
	{

		#region Methods (3) 


		// Public Methods (3) 

		/// <summary>
		/// 
		/// </summary>
		[TestFixtureTearDown]
		public void Dispose()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		[TestFixtureSetUp]
		public void Init()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		[Test]
		public void TestIssuerProperties( )
		{
			Issuer issuer = new Issuer( "Test Company",
		        	                    "test@testcocmpany.com",
		    	                        "http://www.testcompany.com/"  );

			Assert.AreEqual( "Test Company",					issuer.FullName );
			Assert.AreEqual( "test@testcocmpany.com",			issuer.Email );
			Assert.AreEqual( "http://www.testcompany.com/",		issuer.Url );

			issuer.FullName	= "Test Company 2";
			Assert.AreEqual( "Test Company 2",					issuer.FullName );

			issuer.Email	= "test2@testcocmpany.com";
			Assert.AreEqual( "test2@testcocmpany.com",			issuer.Email );

			issuer.Url		= "http://www.testcompany2.com/";
			Assert.AreEqual( "http://www.testcompany2.com/",	issuer.Url );
		}


		#endregion Methods 

	}
}
