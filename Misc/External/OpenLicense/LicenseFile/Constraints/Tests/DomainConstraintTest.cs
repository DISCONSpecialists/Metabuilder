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

namespace OpenLicense.LicenseFile.Constraints.Tests
{
	/// <summary>
	/// 
	/// </summary>
	[TestFixture]
	public class DomainConstraintTest
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
		public void TestDomainConstraint( )
		{
			DomainConstraint domain = new DomainConstraint( );
			string[] arrayOfStrings = new string[3];

			arrayOfStrings[0] = "asp.net";
			arrayOfStrings[1] = "spextreme.com";
			arrayOfStrings[2] = "microsoft.com";

			Assert.IsFalse( domain.Validate( ), "Expected false for a null value as domain but was true." );
			
			domain.Domains = arrayOfStrings;
			
			domain.CurrentDomain = "http://www.asp.net/";
			Assert.IsFalse( domain.Validate( ), "Expected false for www. on the start of the domain." );

			domain.CurrentDomain = "ASP.NET";
			Assert.IsTrue( domain.Validate( ), "Expected true since name is in list but was false." );

			domain.CurrentDomain	= "NotValid.com";
			Assert.IsFalse( domain.Validate( ), "Expected false since name is not in list but was true." );
		}


		#endregion Methods 

	}
}
