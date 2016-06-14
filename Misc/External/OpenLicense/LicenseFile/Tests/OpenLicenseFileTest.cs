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
	///<summary>
	/// 
	///</summary>
	[TestFixture]
	public class OpenLicenseFileTest
	{

		#region Fields (1) 

		///<summary>
		/// 
		///</summary>
		public OpenLicenseFile file = new OpenLicenseFile( Type.GetType( "OpenLicense.LicenseFile.Tests.OpenLicenseFileTest" ), "" );

		#endregion Fields 

		#region Methods (1) 


		// Public Methods (1) 

		///<summary>
		/// 
		///</summary>
		[Test]
		public void TestOpenLicenseFileParseString( )
		{
			file.LoadFromXml( "<?xml version=\"1.0\" encoding=\"utf-16\"?><License Version=\"1.0\"><ValidationKey>WVLsZqy+5j7+bMqE1eHNn60PwCdp1cV6MJvYJee0PGVqs//RHQj4HPrVT+aR3IKgEc3MjoZyXd/c54dn7MUyzg==</ValidationKey><CreationDate>Tuesday, January 10, 2006 7:46:00 PM</CreationDate><FirstUseDate>Tuesday, January 10, 2006 7:48:54 PM</FirstUseDate><ModificationDate>Wednesday, January 11, 2006 6:10:12 PM</ModificationDate><Statistics><DateTimeLastAccessed>1/1/0001 12:00:00 AM</DateTimeLastAccessed><AccessCount>5</AccessCount><HitCount>5</HitCount><DaysCount>1</DaysCount><UsageCount>5</UsageCount></Statistics><Issuer><FullName>SP extreme</FullName><Email>test@email.com</Email><Url>http://www.spextreme.com/</Url></Issuer><Product><Assembly>LicenseLabel, Version=0.93.1805.40767, Culture=neutral, PublicKeyToken=null</Assembly><IsInGac>False</IsInGac><FilePath></FilePath><ShortName>LicenseLabel</ShortName><FullName>LicenseLabel, Version=0.93.1805.40767, Culture=neutral, PublicKeyToken=null</FullName><Version>0.93.1805.40767</Version><Developer></Developer><Description></Description><IsLicensed>False</IsLicensed></Product><Constraints><Constraint><Name>Beta Constraint</Name><Type>OpenLicense.LicenseFile.Constraints.BetaConstraint</Type><Description>The BetaConstraint constrains the user to a given time period.  It supports an end date that the license will expire. It also has the ability to show the user a link to download an update to the beta once it expires.</Description><UpdateUrl /><EndDate /></Constraint><Constraint><Name>Or Constraint</Name><Type>OpenLicense.LicenseFile.Constraints.OrConstraint</Type><Description>This OrConstraint contains a collection of constraints that will be grouped together as a Bitwise OR operation.  It is responsible for validating the containing IConstraints and will be valid as long as one of the constraints contained is valid.  The purpose of this is to allow multiple constraints to be added and create a run as long as one is valid scheme.</Description><Constraint><Name>Design Time Constraint</Name><Type>OpenLicense.LicenseFile.Constraints.DesigntimeConstraint</Type><Description>This DesigntimeConstraint constrains the user to only running the license in a design time environment.  If it is not in a Design Time environment then this constraint will fail.</Description></Constraint><Constraint><Name>Runtime Constraint</Name><Type>OpenLicense.LicenseFile.Constraints.RuntimeConstraint</Type><Description>This RuntimeConstraint constrains the user to only running the license in a runtime environment.  If it is not in a Runtime environment then this constraint will fail.</Description></Constraint></Constraint></Constraints><User><Name>Test Person</Name><Email>test2@email.com</Email><Organization>Test Org</Organization></User></License>" );
			
			Assert.IsNotNull( file.Constraints );
			Assert.AreEqual( "Beta Constraint", file.Constraints[0].Name );
			Assert.AreEqual( "", file.FailureReason );
			Assert.IsFalse( file.IsDirty );
			Assert.IsFalse( file.IsReadOnly );
			Assert.AreEqual( Type.GetType( "OpenLicense.LicenseFile.Tests.OpenLicenseFileTest", false ), file.Type );
			Assert.AreEqual( "LicenseLabel", file.Product.ShortName );
			
			Assert.AreEqual( "SP extreme", file.Issuer.FullName );
			Assert.AreEqual( "test@email.com", file.Issuer.Email );
			Assert.AreEqual( "http://www.spextreme.com/", file.Issuer.Url );
			Assert.IsFalse( file.Issuer.IsDirty );
			
			Assert.IsNull( file.CustomData );
			
			Assert.AreEqual( 1, file.CreationDate.Month );
			Assert.AreEqual( 10, file.CreationDate.Day );
			Assert.AreEqual( 2006, file.CreationDate.Year );
			Assert.AreEqual( 1, file.FirstUsesDate.Month );
			Assert.AreEqual( 10, file.FirstUsesDate.Day );
			Assert.AreEqual( 2006, file.FirstUsesDate.Year );
			Assert.AreEqual( 1, file.ModificationDate.Month );
			Assert.AreEqual( 11, file.ModificationDate.Day );
			Assert.AreEqual( 2006, file.ModificationDate.Year );
			
			Assert.AreEqual( 5, file.Statistics.AccessCount );
			Assert.AreEqual( 1, file.Statistics.DateTimeLastAccessed.Month );
			Assert.AreEqual( 1, file.Statistics.DaysCount );
			Assert.AreEqual( 5, file.Statistics.HitCount );
			Assert.IsFalse( file.Statistics.IsDirty );
			Assert.AreEqual( 5, file.Statistics.UsageCount );
			
			Assert.AreEqual( "Test Person", file.User.Name );
			Assert.AreEqual( "test2@email.com", file.User.Email );
			Assert.AreEqual( "Test Org", file.User.Organization );
			Assert.IsFalse( file.User.IsDirty );
			
//			TODO: finish test for open license
//			TODO: refactor any collections for generics
		}


		#endregion Methods 
		/*
		<?xml version="1.0" encoding="utf-16"?>
		<License Version="1.0">
				<ValidationKey>WVLsZqy+5j7+bMqE1eHNn60PwCdp1cV6MJvYJee0PGVqs//RHQj4HPrVT+aR3IKgEc3MjoZyXd/c54dn7MUyzg==</ValidationKey>
				<CreationDate>Tuesday, January 10, 2006 7:46:00 PM</CreationDate>
				<FirstUseDate>Tuesday, January 10, 2006 7:48:54 PM</FirstUseDate>
				<ModificationDate>Wednesday, January 11, 2006 6:10:12 PM</ModificationDate><Statistics>
				<DateTimeLastAccessed>1/1/0001 12:00:00 AM</DateTimeLastAccessed>
				<AccessCount>5</AccessCount>
				<HitCount>5</HitCount>
				<DaysCount>1</DaysCount>
				<UsageCount>5</UsageCount>
		</Statistics><Issuer>
				<FullName>SP extreme</FullName>
				<Email>test@email.com</Email>
				<Url>http://www.spextreme.com/</Url>
		</Issuer><Product>
				<Assembly>LicenseLabel, Version=0.93.1805.40767, Culture=neutral, PublicKeyToken=null</Assembly>
				<IsInGac>False</IsInGac>
				<FilePath>
				</FilePath>
				<ShortName>LicenseLabel</ShortName>
				<FullName>LicenseLabel, Version=0.93.1805.40767, Culture=neutral, PublicKeyToken=null</FullName>
				<Version>0.93.1805.40767</Version>
				<Developer>
				</Developer>
				<Description>
				</Description>
				<IsLicensed>False</IsLicensed>
		</Product>
		<Constraints>
			<Constraint>
				<Name>Beta Constraint</Name>
				<Type>OpenLicense.LicenseFile.Constraints.BetaConstraint</Type>
				<Description>The BetaConstraint constrains the user to a given time period.  It supports an end date that the license will expire. It also has the ability to show the user a link to download an update to the beta once it expires.</Description>
				<UpdateUrl />
				<EndDate />
			</Constraint>
			<Constraint>
				<Name>Or Constraint</Name>
				<Type>OpenLicense.LicenseFile.Constraints.OrConstraint</Type>
				<Description>This OrConstraint contains a collection of constraints that will be grouped together as a Bitwise OR operation.  It is responsible for validating the containing IConstraints and will be valid as long as one of the constraints contained is valid.  The purpose of this is to allow multiple constraints to be added and create a run as long as one is valid scheme.</Description>
				<Constraint>
					<Name>Design Time Constraint</Name>
					<Type>OpenLicense.LicenseFile.Constraints.DesigntimeConstraint</Type>
					<Description>This DesigntimeConstraint constrains the user to only running the license in a design time environment.  If it is not in a Design Time environment then this constraint will fail.</Description>
				</Constraint>
				<Constraint>
					<Name>OpenLicense.LicenseFile.Constraints Constraint</Name>
					<Type>OpenLicense.RuntimeConstraint</Type>
					<Description>This RuntimeConstraint constrains the user to only running the license in a runtime environment.  If it is not in a Runtime environment then this constraint will fail.</Description>
				</Constraint>
			</Constraint>
		</Constraints>
		<User>
				<Name>Test Person</Name>
				<Email>test2@email.com</Email>
				<Organization>Test Org</Organization>
		</User>
		</License>
		*/

	}
}
