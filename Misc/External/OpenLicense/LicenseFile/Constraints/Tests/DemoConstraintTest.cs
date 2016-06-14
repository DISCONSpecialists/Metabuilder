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
	public class DemoConstraintTest
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
		public void TestDemoConstraint( )
		{
			DemoConstraint demo = new DemoConstraint( );

			int day		= DateTime.Now.Day	- 4;
			int month	= DateTime.Now.Month;

			if( day <= 0 )
			{
				day		= 1;
				month	= month - 1; //this will fail if January 1 to 4
			}

			demo.Condition		= "conditions";
			demo.Duration		= 10;
			demo.StartDate		= new DateTime( DateTime.Now.Year, month, day );
			demo.EndDate		= new DateTime( DateTime.Now.Year, DateTime.Now.Month, ( DateTime.Now.Day + 2 ) );
			demo.InfoURL		= "info url";
			demo.PurchaseURL	= "purchase url";

			Assert.AreEqual( "conditions",		demo.Condition );
			Assert.AreEqual( "info url",		demo.InfoURL );
			Assert.AreEqual( "purchase url",	demo.PurchaseURL );

			Assert.IsTrue( demo.Validate( ), "Start: " + demo.StartDate.ToString( ) +
			              					 " | End: " + demo.EndDate.ToString( ) +
			              					 " | Duration: " + demo.Duration.ToString( ) +
			              					 " | Time Used: " + DateTime.Now );

			demo.StartDate		= DateTime.Now;
			Assert.IsTrue( demo.Validate( ), "Start was " + demo.StartDate.ToString( ) + " expected " + DateTime.Now );

			demo.StartDate		= DateTime.Now.AddDays( 2 );
			Assert.IsFalse( demo.Validate( ), "Start was " + demo.StartDate.ToString( ) + " expected " + DateTime.Now );

			demo.StartDate		= DateTime.Now;
			demo.EndDate		= DateTime.Now;
			Assert.IsTrue( demo.Validate( ), "End was " + demo.StartDate.ToString( ) + " expected " + DateTime.Now );

			demo.EndDate		= new DateTime( DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1 );
			Assert.IsFalse( demo.Validate( ), "End was " + demo.StartDate.ToString( ) + " expected " + DateTime.Now );
		}


		#endregion Methods 

	}
}
