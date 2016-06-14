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

using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace OpenLicense
{
	/// <summary>
	/// A set of static functions to support different functionality throughout Open License 
	/// that is shared.
	/// </summary>
	public class Utilities
	{

		#region Fields (1) 

		private static bool				debug 				= false;

		#endregion Fields 

		#region Methods (3) 


		// Public Methods (3) 

		/// <summary>
		/// This will remove any unusable characters in strings which can not be 
		/// used as file names.
		/// </summary>
		/// <param name="str">
		/// The string to check
		/// </param>
		/// <returns>
		/// The resulting string
		/// </returns>
		public static string CheckStringForInvalidFileNameCharacters( string str )
		{
			str = str.Replace( "\\","A" );
			str = str.Replace( "/","B" );
			str = str.Replace( ":","C" );
			str = str.Replace( "*","D" );
			str = str.Replace( "?","E" );
			str = str.Replace( "<","F" );
			str = str.Replace( ">","G" );
			str = str.Replace( "|","H" );
			//str = str.Replace( Chr(34),"I" );
			
			return str;
		}

		/// <summary>
		/// This creates the Crypto Service Provider for an encryption stream.  This 
		/// also handles setting up the key for the provider and validating it is 
		/// the proper length.  If it is not the proper length it will be padded 
		/// to the correct size.
		/// </summary>
		/// <param name="key">
		/// The sting to be used as the encryption Key and IV.
		/// </param>
		/// <returns>
		/// The RijndaelManaged to be use as the Encryption Provider.
		/// </returns>
		public static RijndaelManaged CreateCryptoServiceProvider( string key )
		{
			RijndaelManaged	cryptoService	= new RijndaelManaged( );
			SHA512Managed	sha512			= new SHA512Managed(  );
			
			sha512.ComputeHash( Encoding.UTF8.GetBytes( key ) );
		
			cryptoService.Mode	= CipherMode.CBC;
			
			byte[] byteKeyArray	= new byte[24];
			byte[] byteIVArray	= new byte[16];
			
			for( int loop=0; loop<24; loop++ ) byteKeyArray[loop] = sha512.Hash[loop];
			for( int loop=0; loop<16; loop++ ) byteIVArray[loop] = sha512.Hash[loop];
			
			cryptoService.Key	= byteKeyArray;
			cryptoService.IV	= byteIVArray;
			
			return cryptoService;
		}

		/// <summary>
		/// Writes debug output to trace calls in the web environment... To be deleted
		/// and replace with the <see cref="System.Diagnostics.Debug">Debug</see> Class.
		/// </summary>
		/// <param name="str">
		/// The string to output to the screen
		/// </param>
		public static void WriteDebugOutput( string str )
		{
			if( debug == true )
			{
				if( HttpContext.Current != null )
				{
					HttpContext.Current.Response.Write( "<pre>" + str + "</pre>" );
					HttpContext.Current.Trace.Warn( str );  //So it comes out red...
				}
			}
		}


		#endregion Methods 

	}
}

#region Unit Test
#if TEST
namespace OpenLicense.UnitTest
{
	using NUnit.Framework;
	
	/// <summary>
	/// 
	/// </summary>
	[TestFixture]
	public class TestUtilities
	{

		#region Methods (1) 


		// Public Methods (1) 

		/// <summary>
		/// 
		/// </summary>
		[Test]
		public void TestToByteArray( )
		{
//			Assert.AreEqual( 49,	(Utilities.ToByteArray( "1234" ) )[0] );
//			Assert.AreEqual( 50,	(Utilities.ToByteArray( "1234" ) )[1] );
//			Assert.AreEqual( 51,	(Utilities.ToByteArray( "1234" ) )[2] );
//			Assert.AreEqual( 52,	(Utilities.ToByteArray( "1234" ) )[3] );
		}


		#endregion Methods 

	}
}
#endif
#endregion

