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

namespace OpenLicense.LicenseFile.Tests
{
	/// <summary>
	///
	/// </summary>
	[TestFixture]
	public class StatisticsTest
	{

		#region Methods (1) 


		// Public Methods (1) 

		/// <summary>
		///
		/// </summary>
		[Test]
		public void TestStatisticsProperties( )
		{
			DateTime rightNow = DateTime.Now;

			Statistics stats = new Statistics( 0, 0, 0, 0, rightNow );

			Assert.IsFalse( stats.IsDirty );

			Assert.AreEqual( 0, stats.AccessCount );
			Assert.AreEqual( 0, stats.DaysCount );
			Assert.AreEqual( 0, stats.HitCount );
			Assert.AreEqual( 0, stats.UsageCount );
			Assert.AreEqual( rightNow,	stats.DateTimeLastAccessed );

			stats.IncrementAccessCount( );
			Assert.AreEqual( 1, stats.AccessCount );

			//Days shouldn't increase since it was already created today.
			stats.IncrementDaysUsed( );
			Assert.AreEqual( 0, stats.DaysCount );

			stats.IncrementHitCount( );
			Assert.AreEqual( 1, stats.HitCount );

			stats.IncrementUsageUsed( );
			Assert.AreEqual( 1, stats.UsageCount );

			Assert.IsTrue( stats.IsDirty );
		}


		#endregion Methods 

	}
}
