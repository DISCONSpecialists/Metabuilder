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
	public class UsageConstraintTest
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
		public void TestUsageConstraint( )
		{
			UsageConstraint usage = new UsageConstraint( );

			usage.MaxDays		= 1;
			usage.MaxHitCount	= 2;
			usage.MaxUsageCount	= 2;

			Assert.AreEqual( 1,	usage.MaxDays );
			Assert.AreEqual( 2,	usage.MaxHitCount );
			Assert.AreEqual( 2,	usage.MaxUsageCount );

			Assert.IsTrue( usage.Validate( ) );

			//usage.IncrementDays( );
			//Assert.AreEqual( 1,	usage.CurrentDays );

			//usage.IncrementHits( ); //Its already 1 so this will make it 2
			//Assert.AreEqual( 2,	usage.CurrentHitCount );

			//usage.IncrementUsage( ); //Its already 1 so this will make it 2
			//Assert.AreEqual( 2,	usage.CurrentUsageCount );

			//usage.IncrementUsage( );
			//Assert.IsFalse( usage.Validate( ) );
		}


		#endregion Methods 

	}
}
