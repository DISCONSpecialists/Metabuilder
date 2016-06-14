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
using NUnit.Framework;

namespace OpenLicense.LicenseFile.Constraints.Tests
{
	/// <summary>
	/// 
	/// </summary>
	[TestFixture]
	public class VersionConstraintTest
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
		public void TestVersionConstraint( )
		{
			VersionConstraint version = new VersionConstraint( );

			version.Minimum = new Version( 0, 90 );
			version.Maximum = new Version( 0, 98 );

			//TODO: Validate required a license to be checked.  figure out how to handle this.
			//Assert.IsTrue( version.Validate( ), "VersionConstraintTest: Expected true but was false. " );
		}


		#endregion Methods 

	}
}
