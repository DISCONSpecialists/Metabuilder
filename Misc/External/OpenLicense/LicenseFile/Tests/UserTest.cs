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

namespace OpenLicense.LicenseFile.Tests
{
	/// <summary>
	/// 
	/// </summary>
	[TestFixture]
	public class UserTest
	{

		#region Methods (1) 


		// Public Methods (1) 

		/// <summary>
		/// 
		/// </summary>
		[Test]
		public void TestUserProperties( )
		{
			User user = new User( "name", "email", "org" );

			Assert.AreEqual( "name",	user.Name );
			Assert.AreEqual( "email",	user.Email );
			Assert.AreEqual( "org",		user.Organization );
			
			Assert.IsFalse( user.IsDirty );
			
			user.Name = "name2";
			Assert.AreEqual( "name2",	user.Name );

			user.Email = "email2";
			Assert.AreEqual( "email2",	user.Email );

			user.Organization = "org2";
			Assert.AreEqual( "org2",	user.Organization );
			
			Assert.IsTrue( user.IsDirty );
		}


		#endregion Methods 

	}
}
