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
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Windows.Forms;
using OpenLicense.LicenseFile;

//For Debug

namespace OpenLicense
{
	/// <summary>
	/// This is the core piece to Open License.  It is the piece that the <c>LicenseManager</c>
	/// used to validate that a license can be granted to a specific type.  This provider is
	/// referenced by the process attempting to validate a license.
	/// </summary>
	/// <seealso cref="System.ComponentModel.LicenseProvider">LicenseProvider</seealso>
	/// <seealso cref="OpenLicenseFile">OpenLicenseFile</seealso>
	public class OpenLicenseProvider : LicenseProvider
	{

		#region Fields (3) 

		private string				absoluteFilePath	= String.Empty;
		private string				encryptionKey		= String.Empty;
		private static readonly OpenLicenseCollector licenseCollector = new OpenLicenseCollector( );

		#endregion Fields 

		#region Constructors (1) 

//TimeStamp Property
		/// <summary>
		/// This is the constructor for the <c>OpenLicenseProvider</c>.
		/// </summary>
		public OpenLicenseProvider( )
		{
			Utilities.WriteDebugOutput( "OpenLicenseProvider: OpenLicenseProvider Constructor" ); 
		}

		#endregion Constructors 

		#region Properties (2) 

		/// <summary>
		/// Gets the LicenseCollector from the current location.  Used to access the proper context
		/// for the License Collection.
		/// </summary>
		private static OpenLicenseCollector LicenseCollector
		{
			get
			{
				Utilities.WriteDebugOutput( "Get Static LicenseCollector" );
				
				OpenLicenseCollector localLC;
				
				if( HttpContext.Current == null )
				{
					return licenseCollector;
				}
				else
				{
					localLC = (OpenLicenseCollector)HttpContext.Current.Application["LicenseCollector"];
					if( localLC == null )   //Should occur only on first application usage
					{
						localLC = licenseCollector;
						HttpContext.Current.Application.Add( "LicenseCollector", localLC ); 
						//TimeStamp = DateTime.Now;
					}
					return localLC;
				}
			}
		}

 //LicenseCollector Property
		private static DateTime TimeStamp
		{
			get
			{
				object ts = HttpContext.Current.Application["LicenseCollectorTS"];
				
				if( ts == null )   //Should not occur
				{
					ts = DateTime.Now;
					HttpContext.Current.Application.Add( "LicenseCollectorTS", (DateTime)ts );
				}
				
				return (DateTime)ts;
			}
			set
			{
				object ts = HttpContext.Current.Application["LicenseCollectorTS"];
				
				if( ts == null )
				{
					HttpContext.Current.Application.Add( "LicenseCollectorTS", value );
				}
				else
				{
					HttpContext.Current.Application["LicenseCollectorTS"] = value;
				}
			}
		}

		#endregion Properties 

		#region Methods (3) 

		// Public Methods (1) 

		/// <summary>
		/// Removes the instance of <see cref="Type">Type</see> from the cache. Cache is
		/// only used in web environments.  This call should only be used if you want to force
		/// Open License to read the file from the disk.
		/// </summary>
		/// <param name="type">
		/// The <see cref="Type">Type</see> to remove from the cache.
		/// </param>
		/// <remarks>
		/// It is highly recommended that this never be used in real world environments because
		/// the cache is where all settings are kept.  Currently as of 0.941 Open License does
		/// save the license to a file.  So if the cache is reset then the license will never
		/// expire for Usage type constraints.
		/// </remarks>
		public static void ResetCashe( Type type )
		{
			Utilities.WriteDebugOutput( "OpenLicenseProvider: ResetCache : " + type.ToString( ) );
			
			LicenseCollector.RemoveLicense( type );
		}

		// Protected Methods (2) 

		/// <summary>
		/// Creates a new empty license based upon the type passed in.
		/// </summary>
		/// <param name="type">
		/// The <see cref="System.Type">Type</see> this license is for.
		/// </param>
		protected virtual OpenLicenseFile CreateEmptyLicense( Type type )
		{
			Utilities.WriteDebugOutput( "OpenLicenseProvider: CreateEmptyLicense Function" );
			return new OpenLicenseFile( type, String.Empty );
		}

		/// <summary>
		/// Creates a new license based upon the data passed in.
		/// </summary>
		/// <param name="type">
		/// The <see cref="System.Type">Type</see> this license is for.
		/// </param>
		/// <param name="key">
		/// They key to generate this license.  The key should be a string in the proper XML format
		/// of an <c>OpenLicenseFile</c>.
		/// </param>
		protected virtual OpenLicenseFile CreateLicense( Type type, string key )
		{
			Utilities.WriteDebugOutput( "OpenLicenseProvider: CreateLicense Function" );
			return new OpenLicenseFile( type, key );
		}

		#endregion Methods 

#region Obtaining A License
		/// <summary>
		/// This is responsible for obtaining the license or reporting the error if non is found.
		/// </summary>
		/// <param name="context">
		/// The context this license is for (Run Time or Design Time)
		/// </param>
		/// <param name="type">
		/// The <see cref="System.Type">Type</see> this license is for.
		/// </param>
		/// <param name="instance">
		/// The instance of an object requesting the license.
		/// </param>
		/// <param name="allowExceptions">
		/// True if an exception should be thrown. Otherwise false
		/// </param>
		/// <exception name="LicenseException">
		/// <p>The reason the validation failed.</p>
		/// </exception>
		public override License GetLicense( LicenseContext context, Type type, object instance, bool allowExceptions )
		{
			Utilities.WriteDebugOutput( "OpenLicenseProvider: GetLicense Function" );
			
			OpenLicenseFile	currentLicense		= null;
			OpenLicenseFile	alternateLicense	= null;

			string			exceptionString		= String.Empty;
			string			licFile				= type.FullName + ".lic";

			// If no context is provided, do nothing
			if( context == null )
			{
				return null;
			}
			
			this.encryptionKey = AssemblyOpenLicenseKeyAttribute.GetEncryptionKeyAttribute( type );

			// Look for the license in the cache... otherwise get it from Isolated Storage
			if( ( currentLicense = LicenseCollector.GetLicense( type ) ) == null )
			{
				currentLicense = GetIsolatedStorageLicense( licFile.ToString( ), type, encryptionKey );
			}
            
          /*  Microsoft.Practices.EnterpriseLibrary.Logging.LogEntry logEntry = new Microsoft.Practices.EnterpriseLibrary.Logging.LogEntry();
            logEntry.Message = "Loading License From : " + licFile;
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(logEntry);*/

			if( ( alternateLicense = GetLicenseFromPaths( licFile.ToString( ), type, encryptionKey ) ) == null )
			{

                
				if( ( alternateLicense = GetEmbeddedLicense( licFile.ToString( ), type, encryptionKey ) ) != null )
				{
					if( HttpContext.Current != null )
					{
						this.absoluteFilePath = HttpContext.Current.Server.MapPath( "~" + Path.DirectorySeparatorChar ) + "bin";
					}
					else
					{
						this.absoluteFilePath = Application.StartupPath;
					}
					this.absoluteFilePath += Path.DirectorySeparatorChar;
					this.absoluteFilePath += licFile.ToString( );
				}
			}
			
			//Check validation key on license file.  If not valid null the license.
			if( currentLicense != null && !currentLicense.Validate( ) )
			{
				currentLicense = null;
			}
			
			if( alternateLicense != null && !alternateLicense.Validate( ) )
			{
				alternateLicense = null;	
			}
			
			//Compare currentLicense with Alternate License
			if( currentLicense == null && alternateLicense == null )
			{
				exceptionString = BuildExceptionString( type );

				if( allowExceptions )
				{
					throw new LicenseException( type, null, exceptionString );
				}
				return null;
			}

			if( currentLicense == null && alternateLicense != null )
			{
				currentLicense = alternateLicense;
				alternateLicense = null;
			}

			if( currentLicense != null && alternateLicense != null )
			{
				//if( currentLicense.CreationDate < alternateLicense.CreationDate )
				if( currentLicense.CreationDate.CompareTo( alternateLicense.CreationDate ) <= 0 )
				{
					//Replace the current license since the alternate is new...
					currentLicense		= alternateLicense;
					alternateLicense	= null;
				}
			}
			
			//Validate date on computer hasn't changes since last use of the license.
//			if( currentLicense.Statistics.DateTimeLastAccessed.Ticks > 0 &&
//			    DateTime.Now < currentLicense.Statistics.DateTimeLastAccessed )
			/*if( ( currentLicense.Statistics.DateTimeLastAccessed.Ticks > 0 ) &&
				( currentLicense.Statistics.DateTimeLastAccessed.CompareTo(DateTime.Now ) > 0 ) )
			{
				currentLicense.FailureReason = "It appears that the date on the computer was changed " +
											   "since the last use of this license.  The licensing scheme " +
											   "does not allow this.";

				return currentLicense;
			}**/

			if( !ValidateLicense( currentLicense ) )
			{
				exceptionString = currentLicense.FailureReason;

				if( exceptionString == String.Empty )
				{
					exceptionString = BuildExceptionString( type );
				}

				if( allowExceptions )
				{
					throw new LicenseException( type, null, exceptionString );
				}

				currentLicense.FailureReason = exceptionString;
				
				currentLicense.Statistics.UpdateLastAccessDate( );
				PerformSave( type, currentLicense );

				return currentLicense;
			}

			//Ok we have a valid license now...
			//Lets update the stats and save this file...
			currentLicense.Statistics.IncrementDaysUsed( );
			currentLicense.Statistics.IncrementAccessCount( );
			currentLicense.Statistics.IncrementUsageUsed( );
			currentLicense.Statistics.IncrementHitCount( );

			PerformSave( type, currentLicense );

			return currentLicense;
		}

		/// <summary>
		/// Obtains a license from Cache if it has been stored there.  Otherwise returns null.
		/// </summary>
		/// <param name="type">
		/// The <see cref="System.Type">Type</see> this license is for.
		/// </param>
		/// <returns>
		/// A valid license if found.  Otherwise null if an error occurs or the license file could
		/// not be found.
		/// </returns>
		private OpenLicenseFile GetCachedLicense( Type type )
		{
			Utilities.WriteDebugOutput( "OpenLicenseProvider: GetCachedLicense Function" );
			
			if( HttpContext.Current != null ) //Web App
			{
				if( HttpContext.Current.Cache[type.ToString( )] != null )
				{
					return (OpenLicenseFile)HttpContext.Current.Cache[type.ToString( )];
				}
			}
			
			return null;
		}
		
		/// <summary>
		/// Obtains a license from Isolated Storage.  If the file doesn't exist then it will return
		/// null.
		/// </summary>
		/// <param name="licFile">
		/// The string name of the license file.
		/// </param>
		/// <param name="type">
		/// The <see cref="System.Type">Type</see> this license is for.
		/// </param>
		/// <param name="key">
		/// The encryption key to decrypt the license stream.  Empty Sting if not encrypted.
		/// </param>
		/// <returns>
		/// A valid license file if found.  Otherwise null if an error occurs or the license file could
		/// not be found.
		/// </returns>
		private OpenLicenseFile GetIsolatedStorageLicense( string licFile, Type type, string key )
		{
            return null;
			Utilities.WriteDebugOutput( "OpenLicenseProvider: GetIsolatedStorageLicense Function" );

			if( HttpContext.Current != null ) //Web App
			{
				return null;
			}

			IsolatedStorageFile	isoFile	= null;
			OpenLicenseFile		ol_file	= null;

			try
			{
				isoFile	= IsolatedStorageFile.GetStore( IsolatedStorageScope.User |
				                                        IsolatedStorageScope.Domain |
														IsolatedStorageScope.Assembly, null, null);
			}
			catch //( Exception exp )
			{
				return null;
			}


			IFormatter	formatter		= new BinaryFormatter();
			Stream		reader			= null;
			String		licenseString	= String.Empty;
			bool		useEnc			= false;

			if( key != String.Empty )
			{
				useEnc = true;
			}

			try
			{
				reader	= new IsolatedStorageFileStream( GenerateIsolatedFileName( licFile ), FileMode.Open, isoFile );
				ol_file	= OpenLicenseFile.LoadFile( reader, type, useEnc, key );
			}
			catch
			{
			}

			if( reader != null )
				reader.Close();

			if( ol_file != null && ol_file.LicenseKey != String.Empty )
			{
				return ol_file;
			}

			return null;
		}

		/// <summary>
		/// Obtains a license from a preset set of paths.  The paths are defined as:
		/// <list>
		/// <item>Application start path</item>
		/// <item>The Application Bin directory</item>
		/// </list>
		/// If the file doesn't exist then it will return null.
		/// </summary>
		/// <param name="licFile">
		/// The string name of the license file.
		/// </param>
		/// <param name="type">
		/// The <see cref="System.Type">Type</see> this license is for.
		/// </param>
		/// <param name="key">
		/// The encryption key to decrypt the license stream.  Empty Sting if not encrypted.
		/// </param>
		/// <returns>
		/// A valid license file if found.  Otherwise null if an error occurs or the license file could
		/// not be found.
		/// </returns>
		private OpenLicenseFile GetLicenseFromPaths( string licFile, Type type, string key )
		{
			Utilities.WriteDebugOutput( "OpenLicenseProvider: GetLicenseFromPaths Function" );

			Stream	stream	= null;
			bool	useEnc	= false;

			if( key != String.Empty )
			{
				useEnc = true;
			}

			this.absoluteFilePath	= this.GetFilePath( licFile.ToString( ) );
            /*Microsoft.Practices.EnterpriseLibrary.Logging.LogEntry logEntry = new Microsoft.Practices.EnterpriseLibrary.Logging.LogEntry();
            logEntry.Message = "Loading License From : " + absoluteFilePath;
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(logEntry);*/

			if( this.absoluteFilePath == String.Empty )
			{
				return null;
			}

			stream = new FileStream( this.absoluteFilePath, FileMode.Open, FileAccess.Read, FileShare.Read );

			if( stream == null )
			{
				return null;
			}

			return OpenLicenseFile.LoadFile( stream, type, useEnc, key );
		}

		/// <summary>
		/// Obtains a license embedded into the Assembly. It checks the running assembly
		/// to see if the license has been embedded.  If the file doesn't exist then it
		/// will return null.
		/// </summary>
		/// <param name="licFile">
		/// The string name of the license file.
		/// </param>
		/// <param name="type">
		/// The <see cref="System.Type">Type</see> this license is for.
		/// </param>
		/// <param name="key">
		/// The encryption key to decrypt the license stream.  Empty Sting if not encrypted.
		/// </param>
		/// <returns>
		/// A valid license file if found.  Otherwise null if an error occurs or the license file could
		/// not be found.
		/// </returns>
		private OpenLicenseFile GetEmbeddedLicense( string licFile, Type type, string key )
		{
			Utilities.WriteDebugOutput( "OpenLicenseProvider: GetEmbeddedLicense Function" );

			Stream	stream	= null;
			bool	useEnc	= false;


			if( key != String.Empty )
			{
				useEnc = true;
			}

			try
			{
				stream = Assembly.GetAssembly( type ).GetManifestResourceStream( licFile );
			}
			catch
			{
			}

			if( stream == null )
			{
				return null;
			}

			return OpenLicenseFile.LoadFile( stream, type, useEnc, key );
		}
#endregion

#region License Functions
		/// <summary>
		/// Validate this license against the defined <see cref="IConstraint">IConstraints</see> in the
		/// Constraints List (System.Collections.Generic.List).
		/// </summary>
		/// <param name="license">
		/// The <see cref="OpenLicenseFile"/> to validate.
		/// </param>
		/// <returns>
		/// <c>True</c> if the license is valid; Otherwise <c>False</c>.
		/// </returns>
        public bool ValidateLicense(OpenLicenseFile license)
        {
            Utilities.WriteDebugOutput("OpenLicenseProvider: ValidateLicense Function");
            /*  if (DateTime.Now < Core.GlobalParser.ParseGlobalisedDateString("31 August 2007"))
              {
                  return true;
              }*/

            if (license == null)
            {
                /*Microsoft.Practices.EnterpriseLibrary.Logging.LogEntry logEntry = new Microsoft.Practices.EnterpriseLibrary.Logging.LogEntry();
                logEntry.Message = "OpenLicenseProvider_ValidateLicense() - No License";
                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(logEntry);*/
                return false;
            }

            //If the license itself is licensed then don't bother checking the constraints
            for (int i = 0; i < license.Constraints.Count; i++)
            {
                // Need to add support for and/or of license constraints
                IConstraint temp = (IConstraint)license.Constraints[i];

                if (!temp.Validate())
                {

                    /*Microsoft.Practices.EnterpriseLibrary.Logging.LogEntry logEntry2 = new Microsoft.Practices.EnterpriseLibrary.Logging.LogEntry();
                    logEntry2.Message = "OpenLicenseProvider_ValidateLicense() - constraint: " + temp.ToString() + " - Failed";
                    Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(logEntry2);*/
                    return false;
                }
            }

            return license.Validate();
            if (license.Product.IsLicensed)
            {
                return true;
            }
            return true;
        }

		/// <summary>
		/// This is responsible for saving the license into the proper locations.
		/// </summary>
		/// <param name="type">
		/// The object <c>Type</c> being licensed.
		/// </param>
		/// <param name="license">
		/// The license to save.
		/// </param>
		private void PerformSave( Type type, OpenLicenseFile license )
		{
			Utilities.WriteDebugOutput( "OpenLicenseProvider: PerformSave function" );
			
		//	if( ( HttpContext.Current == null ) ||				//Desktop App
		//		( DateTime.Now > TimeStamp.AddMinutes( 15 ) ) )	//Web App
			if( HttpContext.Current == null )  // Desktop app
			{
		//		SaveLicenseFile( license, this.encryptionKey, false );
				SaveLicenseFile(type, license, this.encryptionKey, true );
				
		//		TimeStamp = DateTime.Now;
			}
			
			if( HttpContext.Current != null )					//Web App
			{
				//Force the save outside the bin directory since this causes the app to restart.
				this.absoluteFilePath = HttpContext.Current.Server.MapPath( "~" + Path.DirectorySeparatorChar ) + license.Type.FullName + ".lic";
				
				SaveLicenseFile(type, license, this.encryptionKey, true );
				
				LicenseCollector.AddLicense( type, license );
			}
		}
		
		/// <summary>
		/// This saves the license in the proper location.
		/// </summary>
		/// <param name="licenseFile">
		/// The license file to save.
		/// </param>
		/// <param name="key">
		/// The encryption key to use for encrypting the license.  If empty string then no encryption will be used.
		/// </param>
		/// <param name="useIsolatedStorage">
		/// Save to IsolatedStorage if true. Otherwise use path.
		/// </param>
		private void SaveLicenseFile(Type type, OpenLicenseFile licenseFile, string key, bool useIsolatedStorage )
		{
			Utilities.WriteDebugOutput( "OpenLicenseProvider: SaveLicenseFile Function" );

			bool useEnc = false;

			if( key != String.Empty )
			{
				useEnc = true;
			}
			
			if( this.absoluteFilePath == String.Empty && HttpContext.Current == null )
			{
				this.absoluteFilePath = Application.ExecutablePath + licenseFile.Type.FullName + ".lic";
			}
			else if( this.absoluteFilePath == String.Empty && HttpContext.Current != null )
			{
				this.absoluteFilePath = HttpContext.Current.Server.MapPath( "~" + Path.DirectorySeparatorChar ) + licenseFile.Type.FullName + ".lic";
			}

			if( useIsolatedStorage && ( HttpContext.Current == null ) )
			{
				try
				{
					IsolatedStorageFile	isoFile	= IsolatedStorageFile.GetStore( IsolatedStorageScope.User |
					                                                        	IsolatedStorageScope.Domain |
																				IsolatedStorageScope.Assembly, null, null);

					string licFile = GenerateIsolatedFileName( type.FullName + ".lic" );

					Stream writer = new IsolatedStorageFileStream( licFile, FileMode.Create, isoFile );
						//new IsolatedStorageFileStream( licFile, FileMode.OpenOrCreate, isoFile );
					
					OpenLicenseFile.SaveFile( licenseFile, writer, useEnc, key );

					writer.Close();
				}
				catch
				{
					if( this.absoluteFilePath != String.Empty )
					{
						OpenLicenseFile.SaveFile( licenseFile, this.absoluteFilePath, useEnc, this.encryptionKey );
					}
				}
			}
			else
			{
				if( this.absoluteFilePath != String.Empty )
				{
					OpenLicenseFile.SaveFile(licenseFile, this.absoluteFilePath, useEnc, this.encryptionKey );
				}
			}
		}
#endregion

#region Utilities
		/// <summary>
		/// Returns the hash key based upon the passed in string.
		/// </summary>
		/// <param name="fileName">
		/// The string to generate the Hash key for.
		/// </param>
		/// <returns>
		/// The generated hash key as a <see cref="System.String">String</see>.
		/// </returns>
		private string GenerateIsolatedFileName( string fileName )
		{
			Utilities.WriteDebugOutput( "OpenLicenseProvider: GenerateIsolatedFileName Function" );

			SHA1Managed	sha1		= new SHA1Managed(  );
			byte[]		dataBytes	= Encoding.UTF8.GetBytes( fileName );

			sha1.ComputeHash( dataBytes );
			
			return Utilities.CheckStringForInvalidFileNameCharacters( Convert.ToBase64String(sha1.Hash, 0, sha1.Hash.Length).Replace ('/', '~' ) );
		}

		/// <summary>
		/// The path to a license file based on the license name.
		/// </summary>
		/// <param name="licenseFile">
		/// The name of the license file to attempt to find.
		/// </param>
		/// <returns>
		/// The path appended with the file name where the license was found.  Otherwise an
		/// empty string is returned.
		/// </returns>
		private string GetFilePath( string licenseFile )
		{
			Utilities.WriteDebugOutput( "OpenLicenseProvider: GetFilePath Function" );

			ArrayList strArray = new ArrayList( );

			//TODO: Change to pull based on Assembly locations
			if( HttpContext.Current != null )
			{
				strArray.Add( HttpContext.Current.Server.MapPath( "~" + Path.DirectorySeparatorChar ) );
				strArray.Add( HttpContext.Current.Server.MapPath( "." + Path.DirectorySeparatorChar ) );
				strArray.Add( HttpContext.Current.Server.MapPath( "~" + Path.DirectorySeparatorChar ) + "bin" + Path.DirectorySeparatorChar );
			}
			else
			{
				strArray.Add( Application.StartupPath + Path.DirectorySeparatorChar );
				strArray.Add( Application.StartupPath + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar );
			
			}
			
			for( int i = 0; i < strArray.Count; i++ )
			{
				//Check current directory...
				if( File.Exists( ( (String)strArray[i] ) + licenseFile ) )
					return ( (String)strArray[i] ) + licenseFile;
			}

			return String.Empty;
		}

		/// <summary>
		/// Used to build an exception string with the Helper Attributes data
		/// if it exists.
		/// </summary>
		/// <param name="type">
		/// The object type that the exception occurred for.
		/// </param>
		private string BuildExceptionString( Type type )
		{
			Utilities.WriteDebugOutput( "OpenLicenseProvider: BuildExceptionString Function" );

			StringBuilder errorMsg		= new StringBuilder( );

			AssemblyOpenLicenseHelperAttribute attr =
				(AssemblyOpenLicenseHelperAttribute)Attribute.GetCustomAttribute(
					Assembly.GetAssembly( type ),
					Type.GetType( "OpenLicense.AssemblyOpenLicenseHelperAttribute" ) );

			errorMsg.Append( "A valid license cannot be granted for the type " );

			if( attr != null )
			{
				if( attr.Product != String.Empty )
					errorMsg.Append( attr.Product );
				else
					errorMsg.Append( Assembly.GetAssembly( type ).GetName( ).Name );

				errorMsg.Append( ".  " );
				errorMsg.Append( "Contact " );

				if( attr.Company != String.Empty )
				{
					errorMsg.Append( attr.Company );
					errorMsg.Append( " " );
				}
				else
					errorMsg.Append( "the manufacturer " );

				if( attr.Url != String.Empty )
				{
					errorMsg.Append( " (" );
					errorMsg.Append( attr.Url );
					errorMsg.Append( ") " );
				}

				if( attr.Email != String.Empty )
				{
					errorMsg.Append( "at " );
					errorMsg.Append( attr.Email );
					errorMsg.Append( " " );

					if( attr.Phone != String.Empty )
						errorMsg.Append( "or by phone at " );
				}

				if( attr.Phone != String.Empty )
				{
					if( attr.Email == String.Empty )
						errorMsg.Append( "at " );

					errorMsg.Append( attr.Phone );
					errorMsg.Append( " " );
				}

				errorMsg.Append( "for more information." );
			}
			else
			{
				errorMsg.Append( Assembly.GetAssembly( type ).GetName( ).Name );
				errorMsg.Append( ".  " );
				errorMsg.Append( "Contact the manufacturer of the component for more information." );
			}

			return errorMsg.ToString( );
		}
#endregion

#region OpenLicenseCollector
		/// <summary>
		/// This is a collection of Licenses used internally by the provider when caching
		/// license files.
		/// </summary>
		private sealed class OpenLicenseCollector
		{
			private IDictionary collectedLicenses;

			///<summary>
			/// This is the constructor for the OpenLicenseCollector.
			/// </summary>
			public OpenLicenseCollector( )
			{
				this.collectedLicenses = new HybridDictionary( );
			}

			///<summary>
			/// This adds a new license to the collector.
			/// </summary>
			/// <param name="type">
			/// The <see cref="Type">Type</see> of this object to create the license for.
			/// </param>
			/// <param name="license">
			/// The license to add.
			/// </param>
			/// <exception name="ArgumentNullException">
			/// <p>Thrown if the license or type value are null</p>
			/// </exception>
			public void AddLicense( Type type, OpenLicenseFile license )
			{
				if( type == null )
				{
					throw new ArgumentNullException( "type" );
				}

				if( license == null )
				{
					throw new ArgumentNullException( "license" );
				}

				this.collectedLicenses[type] = license;
			}

			///<summary>
			/// This gets the license for the given <see cref="Type">Type</see>.
			/// </summary>
			/// <param>
			/// The <see cref="Type">Type</see> to get the license for.
			/// </param>
			/// <exception name="ArgumentNullException">
			/// <p>Thrown if the type value are null</p>
			/// </exception>
			public OpenLicenseFile GetLicense( Type type )
			{
				if( type == null )
				{
					throw new ArgumentNullException( "type" );
				}

				if( this.collectedLicenses.Count == 0 )
				{
					return null;
				}

				return (OpenLicenseFile)collectedLicenses[type];
			}

			///<summary>
			/// This removes the license for the given <see cref="Type">Type</see>.
			/// </summary>
			/// <param>
			/// The <see cref="Type">Type</see> to remove.
			/// </param>
			/// <exception name="ArgumentNullException">
			/// <p>Thrown if the type value are null</p>
			/// </exception>
			public void RemoveLicense( Type type )
			{
				if( type == null )
				{
					throw new ArgumentNullException( "type" );
				}

				this.collectedLicenses.Remove( type );
			}
		}
#endregion
	}
} /* NameSpace OpenLicense */
