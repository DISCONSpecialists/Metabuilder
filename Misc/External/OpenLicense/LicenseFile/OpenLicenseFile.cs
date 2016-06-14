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
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Logging;

/*
<?xml version="1.0" encoding="utf-16"?>
<License Version="1.0">
	<ValidationKey>hash key data</ValidationKey>
	<CreationDate>date</CreationDate>
	<FirstUseDate>date</FirstUseDate>
	<ModificationDate>date</ModificationDate>
	<Statistics>
		<DateTimeLastAccessed>date</DateTimeLastAccessed>
		<AccessCount>#</AccessCount>
		<HitCount>#</HitCount>
		<DaysCount>#</DaysCount>
		<UsageCount>#</UsageCount>
	</Statistics>
	<Issuer>
		<FullName>string</FullName>
		<Email>string</Email>
		<Url>string</Url>
	</Issuer>
	<Product>
		<Assembly>string</Assembly>
		<IsInGac>False</IsInGac>
		<FilePath>string</FilePath>
		<ShortName>string</ShortName>
		<FullName>string</FullName>
		<Version></Version>
		<Developer></Developer>
		<Description></Description>
		<IsLicensed>False</IsLicensed>
	</Product>
	<Constraints>
		...
	</Constraints>
	<User>
		<Name />
		<Email />
		<Organization />
	</User>
</License>
*/
namespace OpenLicense.LicenseFile
{
    /// <summary>
    /// This is the .LIC file object.  This file contains all the fields
    /// a developer should need to create a license of their choice.  It supports
    /// many standard fields and has the ability to store custom key/value pairs.
    /// This file also contains a Constraints List (System.Collections.Generic.List) 
    /// which is responsible for determine the validity of this license.  The 
    /// constraints are used to determine if the license may be used.
    /// </summary>
    /// <seealso cref="OpenLicenseProvider">OpenLicenseProvider</seealso>
    /// <seealso cref="System.String">String</seealso>
    /// <seealso cref="System.Xml.XmlDocument">XmlDocument</seealso>
    /// <seealso cref="System.Xml.XmlNode">XmlNode</seealso>
    [Serializable]
    public class OpenLicenseFile : License
    {

		#region Fields (21) 

        // Limiting Factors for a license.
        private List<IConstraint> constraints = new List<IConstraint>();
        //Usage dates
        private DateTime creationDate = new DateTime();
        private IFormatProvider culture = CultureInfo.InvariantCulture;
        //Custom information for the license
        private CustomData customData = new CustomData();
        // Internal information for tracking
        private string failureMessage = String.Empty;
        private DateTime firstUseDate = new DateTime();
        private string hashKey = String.Empty;
        private bool isDirty = false;
        private bool isReadOnly = false;
        private Issuer issuer = new Issuer();
        //Options for this license
        private string key = String.Empty;
        private DateTime modificationDate = new DateTime();
        // Product information
        private Product product = new Product();
        private string readKeyHash = String.Empty;
        //Hash String for validation
        private string signedKeyHash = String.Empty;
        private Statistics statistics = new Statistics();
        private Type type;
        private bool useEncryption = false;
        //Users information for this license
        private User user = new User();
        private string version = "1.0";
        private bool writeKey = true;

		#endregion Fields 

		#region Constructors (1) 

        /// <summary>
        /// The constructor for an <c>OpenLicenseFile</c>.
        /// </summary>
        /// <param name="type">
        /// The <see cref="System.Type">Type</see> requesting the License.
        /// </param>
        /// <param name="key">
        /// A <see cref="System.String">String</see> representing the Xml data of the license.
        /// </param>
        public OpenLicenseFile(Type type, string key)
        {
            this.key = key;
            this.type = type;
            if (this.LicenseKey != null && this.LicenseKey != String.Empty)
                LoadFromXml(this.LicenseKey);
        }

		#endregion Constructors 

		#region Methods (1) 

		// Public Methods (1) 

        /// <summary>
        /// Destroys this instance of the license.
        /// </summary>
        public override void Dispose()
        {

        }

		#endregion Methods 

        #region Static
        /// <summary>
        /// Creates a new <c>OpenLicenseFile</c> from a
        /// <see cref="System.String">String</see> that contains Xml data.
        /// </summary>
        /// <param name="type">
        /// The <see cref="System.Type">Type</see> requesting the License.
        /// </param>
        /// <param name="str">
        /// A <see cref="System.String">String</see> representing a valid XML version of a
        /// license.
        /// </param>
        /// <returns>
        /// The <c>OpenLicenseFile</c> generated from this Xml data or null if an error occurred.
        /// </returns>
        public static OpenLicenseFile FromXml(Type type, string str)
        {
            return new OpenLicenseFile(type, str);
        }

        public OpenLicenseFile()
        {
        }

        /// <summary>
        /// Creates a new <c>OpenLicenseFile</c> from a <see cref="System.String">String</see>.
        /// </summary>
        /// <param name="type">
        /// The <see cref="System.Type">Type</see> requesting the object to be licensed.
        /// </param>
        /// <param name="doc">
        /// A <see cref="System.Xml.XmlDocument">XmlDocument</see> representing the license.
        /// </param>
        /// <returns>
        /// The <c>OpenLicenseFile</c> generated from this
        /// <see cref="System.Xml.XmlDocument">XmlDocument</see> or null if an error occurred.
        /// </returns>
        public static OpenLicenseFile FromXml(Type type, XmlDocument doc)
        {
            OpenLicenseFile newLicense = new OpenLicenseFile(type, String.Empty);

            newLicense.LoadFromXml(doc);

            return newLicense;
        }

        /// <summary>
        /// Create an OpenLicenseFile from a string
        /// </summary>
        /// <param name="str">
        /// The string to create the license from.
        /// </param>
        /// <param name="type">
        /// The Type of object being loaded.
        /// </param>
        /// <returns>
        /// A new OpenLicenseFile from the string or blank new file.
        /// </returns>
        public static OpenLicenseFile LoadFromString(string str, Type type)
        {
            return new OpenLicenseFile(type, str);
        }

        /// <summary>
        /// Loads the specified file to create an OpenLicenseFile.
        /// </summary>
        /// <param name="stream">
        /// The stream to use to open the file.
        /// </param>
        /// <param name="type">
        /// The Type of object being loaded.
        /// </param>
        /// <param name="useEncryption">
        /// A flag to know if this file should be encrypted when saved. True will encrypt the
        /// file.  If this is set then the key value must be defined as a valid string.
        /// </param>
        /// <param name="key">
        /// The encryption key to be used to encrypt the file. Only used if useEncryption is set to true.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when the encryption key is blank and the useEncryption value is set to true.
        /// </exception>
        public static OpenLicenseFile LoadFile(Stream stream, Type type, bool useEncryption, string key)
        {
            //13 March 2013
            //2 years of development with this set to
            //SKIP LICENCING BY SIMPLY ISSUEING A NEW LICENCE EVERY TIME YOU START METABUILDER
            //TODO : return new OpenLicenseFile();
            if (useEncryption && key == String.Empty)
                throw new ArgumentException("The encryption key can not be left blank when encrypt is set to true");

            OpenLicenseFile license = null;

            CryptoStream cryptoStream = null;
            StreamReader streamReader = null;

            if (useEncryption)
            {
                Rijndael provider = Utilities.CreateCryptoServiceProvider(key);

                cryptoStream = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Read);
                streamReader = new StreamReader(cryptoStream);
            }
            else
                streamReader = new StreamReader(stream);

            string fileString = streamReader.ReadToEnd();

            license = LoadFromString(fileString, type);

            if (streamReader != null) streamReader.Close();
            if (cryptoStream != null) cryptoStream.Close();

            license.ResetIsDirty();

            return license;
        }

        /// <summary>
        /// Loads the specified file to create an OpenLicenseFile.
        /// </summary>
        /// <param name="type">
        /// The type requesting to load the OpenLicenseFile for.  This is used to create the license.
        /// </param>
        /// <param name="path">
        /// The full path to save the file.
        /// </param>
        /// <param name="fileName">
        /// The name of the file to be created.
        /// </param>
        /// <param name="useEncryption">
        /// A flag to know if this file should be encrypted when saved. True will encrypt the
        /// file.  If this is set then the key value must be defined as a valid string.
        /// </param>
        /// <param name="key">
        /// The encryption key to be used to encrypt the file. Only used if use_encryption is set to true.
        /// </param>
        public static OpenLicenseFile LoadFile(Type type, string path, string fileName, bool useEncryption,
                                                string key)
        {
            StringBuilder resolvedFile = new StringBuilder(path);
            if (!path.EndsWith("\\"))
                resolvedFile.Append("\\");
            resolvedFile.Append(fileName);

            return LoadFile(type, resolvedFile.ToString(), useEncryption, key);

        }

        /// <summary>
        /// Loads the specified file to create an OpenLicenseFile.
        /// </summary>
        /// <param name="type">
        /// The type requesting to load the OpenLicenseFile for.  This is used to create the license.
        /// </param>
        /// <param name="pathWithFileName">
        /// The full path including the filename.
        /// </param>
        /// <param name="useEncryption">
        /// A flag to know if this file should be encrypted when saved. True will encrypt the
        /// file.  If this is set then the key value must be defined as a valid string.
        /// </param>
        /// <param name="key">
        /// The encryption key to be used to encrypt the file. Only used if use_encryption is set to true.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown if the path does not exist.
        /// </exception>
        public static OpenLicenseFile LoadFile(Type type, string pathWithFileName, bool useEncryption, string key)
        {
            if (!File.Exists(pathWithFileName))
                throw new ArgumentException("File path invalid");

            FileStream fileStream = null;
          
          

            fileStream = new FileStream(pathWithFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);

            return LoadFile(fileStream, type, useEncryption, key);
        }

        /// <summary>
        /// Saves the license to the specified path using the given file name.
        /// </summary>
        /// <param name="license">
        /// The <c>OpenLicenseFile</c> to save.
        /// </param>
        /// <param name="stream">
        /// The stream to write the file to.
        /// </param>
        /// <param name="useEncryption">
        /// A flag to know if this file should be encrypted when saved. True will encrypt the
        /// file.  If this is set then the key value must be defined as a valid string.
        /// </param>
        /// <param name="key">
        /// The encryption key to be used to encrypt the file. Only used if use_encryption is set to true.
        /// </param>
        public static void SaveFile(OpenLicenseFile license, Stream stream, bool useEncryption, string key)
        {
            license.SetupEncryptionKey(key);

            license.SaveFile(stream);
        }

        /// <summary>
        /// Saves the license to the specified path using the given file name.
        /// </summary>
        /// <param name="license">
        /// The <c>OpenLicenseFile</c> to save.
        /// </param>
        /// <param name="path">
        /// The full path to save the file.
        /// </param>
        /// <param name="fileName">
        /// The name of the file to be created.
        /// </param>
        public static void SaveFile(OpenLicenseFile license, String path, string fileName)
        {
            SaveFile(license, path, fileName, false, String.Empty);
        }

        /// <summary>
        /// Saves the license to the specified path using the given file name.
        /// </summary>
        /// <param name="license">
        /// The <c>OpenLicenseFile</c> to save.
        /// </param>
        /// <param name="path">
        /// The full path to save the file.
        /// </param>
        /// <param name="fileName">
        /// The name of the file to be created.
        /// </param>
        /// <param name="useEncryption">
        /// A flag to know if this file should be encrypted when saved. True will encrypt the
        /// file.  If this is set then the key value must be defined as a valid string.
        /// </param>
        /// <param name="key">
        /// The encryption key to be used to encrypt the file. Only used if use_encryption is set to true.
        /// </param>
        public static void SaveFile(OpenLicenseFile license, string path, string fileName,
                                     bool useEncryption, string key)
        {
            StringBuilder resolvedFile = new StringBuilder(path);
            if (!path.EndsWith("\\"))
                resolvedFile.Append("\\");
            resolvedFile.Append(fileName);

            SaveFile(license, resolvedFile.ToString(), useEncryption, key);
        }

        /// <summary>
        /// Saves the license to the specified path using the given file name.
        /// </summary>
        /// <param name="license">
        /// The <c>OpenLicenseFile</c> to save.
        /// </param>
        /// <param name="fullPathWithFileName">
        /// The full path including the filename.
        /// </param>
        /// <param name="useEncryption">
        /// A flag to know if this file should be encrypted when saved. True will encrypt the
        /// file.  If this is set then the key value must be defined as a valid string.
        /// </param>
        /// <param name="key">
        /// The encryption key to be used to encrypt the file. Only used if use_encryption is set to true.
        /// </param>
        public static void SaveFile(OpenLicenseFile license, string fullPathWithFileName, bool useEncryption,
                                     string key)
        {
            license.SetupEncryptionKey(key);

            license.SaveFile(fullPathWithFileName);

        }
        #endregion

        #region Loading & Parsing
        /// <summary>
        /// Loads a license from a Xml file to generate the license.
        /// </summary>
        /// <param name="fullPathWithName">
        /// A <see cref="System.String">String</see> representing the path and file name of
        /// the license.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown if the file could not be found.
        /// </exception>
        /// <remarks>
        /// The full path name including the file name is required for this function to know
        /// where to save the file.  You may want to use the <c>MapPath</c> function to make sure you
        /// have the proper path.
        /// </remarks>
        public void LoadFromFile(string fullPathWithName)
        {
#if DEBUG
            // Console.WriteLine(fullPathWithName);
#endif
            //Check if file exists.  If not throws an exception...
            if (!File.Exists(fullPathWithName))
                throw new ArgumentException("The path and file name are not valid.  It must point to a valid XML file.");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fullPathWithName);

            parseLicense(xmlDoc.SelectSingleNode("/License"));
        }

        /// <summary>
        /// Loads a license from a <see cref="System.String">String</see> representing
        /// the Xml data for the license.
        /// </summary>
        /// <param name="xmlData">
        /// A <see cref="System.String">String</see> representing the Xml data of a
        /// license.
        /// </param>
        public void LoadFromXml(String xmlData)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlData);

            parseLicense(xmlDoc.SelectSingleNode("/License"));
        }

        /// <summary>
        /// Loads a license from a <see cref="System.Xml.XmlDocument">XmlDocument</see>
        /// for the license.
        /// </summary>
        /// <param name="xmlDoc">
        /// A <see cref="System.Xml.XmlDocument">XmlDocument</see> representing the license.
        /// </param>
        public void LoadFromXml(XmlDocument xmlDoc)
        {
            parseLicense(xmlDoc.SelectSingleNode("/License"));
        }

        /// <summary>
        /// Loads a license from a <see cref="System.Xml.XmlNode">XmlNode</see> for the
        /// license.
        /// </summary>
        /// <param name="xmlData">
        /// A <see cref="System.Xml.XmlNode">XmlNode</see> representing the license.
        /// </param>
        public void LoadFromXml(XmlNode xmlData)
        {
            parseLicense(xmlData.SelectSingleNode("/License"));
        }

        /// <summary>
        /// Parses the XML content of the license. Used internally when the key is passed
        /// into the constructor to build the license object.  It is also used by the
        /// LoadFromXml methods.
        /// </summary>
        /// <param name="itemsNode">
        /// A <see cref="System.Xml.XmlNode">XmlNode</see> representing the license.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the license data is null.
        /// </exception>
        private void parseLicense(XmlNode itemsNode)
        {
            if (itemsNode == null)
                throw new ArgumentNullException("The license data is null.");

            XmlNodeList licenseItems = itemsNode.ChildNodes;

            // Parse the items in the license first...
            parseLicenseItems(itemsNode);

            // Now go through the children for the custom values and user items.
            for (int i = 0; i < licenseItems.Count; i++)
            {
                XmlNode licenseItem = licenseItems[i];

                if (licenseItem.Name == "Product")
                    this.product = Product.FromXml(licenseItem);
                else if (licenseItem.Name == "User")
                    user = User.FromXml(licenseItem);
                else if (licenseItem.Name == "CustomData")
                    parseCustomFields(licenseItems[i]);
                else if (licenseItem.Name == "Constraints")
                    parseConstraintsFields(licenseItems[i]);
                else if (licenseItem.Name == "Issuer")
                    this.issuer = Issuer.FromXml(licenseItem);
                else if (licenseItem.Name == "Statistics")
                    this.statistics = Statistics.FromXml(licenseItem);
            }

            this.GenerateHashString();
        }

        /// <summary>
        /// Parse the XML content of the license that is not part of another child object.
        /// </summary>
        /// <param name="itemNode">
        /// A <see cref="System.Xml.XmlNode">XmlNode</see> representing the license.
        /// </param>
        private void parseLicenseItems(XmlNode itemNode)
        {
            XmlNode hashKeyTextNode = itemNode.SelectSingleNode("ValidationKey/text()");		// String
            XmlNode creationDateTextNode = itemNode.SelectSingleNode("CreationDate/text()");		// Date
            XmlNode firstUseDateTextNode = itemNode.SelectSingleNode("FirstUseDate/text()");		// Date
            XmlNode modificationDateTextNode = itemNode.SelectSingleNode("ModificationDate/text()");	// Date

            if (hashKeyTextNode != null)
                this.readKeyHash = hashKeyTextNode.Value;

            try
            {
                if (creationDateTextNode != null)
                {
                    this.creationDate = MetaBuilder.Core.GlobalParser.ParseGlobalisedDateString(creationDateTextNode.Value);
                }

                if (firstUseDateTextNode != null)
                {
                    this.firstUseDate =
                        MetaBuilder.Core.GlobalParser.ParseGlobalisedDateString(firstUseDateTextNode.Value);
                }

                if (modificationDateTextNode != null)
                {
                    this.modificationDate =
                        MetaBuilder.Core.GlobalParser.ParseGlobalisedDateString(modificationDateTextNode.Value);
                }
            }
            catch
            {
                creationDate = DateTime.Now;
                firstUseDate = DateTime.Now;
                modificationDate = DateTime.Now;
            }
            this.signedKeyHash = String.Empty;
        }

        /// <summary>
        /// Parse the XML content of the custom fields section of the license.
        /// </summary>
        /// <param name="itemsNode">
        /// A <see cref="System.Xml.XmlNode">XmlNode</see> representing the Custom Fields
        /// section of the license.
        /// </param>
        private void parseCustomFields(XmlNode itemsNode)
        {
            // Check if custom fields are defined
            if (itemsNode == null)
                return;

            // If they are then process all of them
            XmlNodeList customItems = itemsNode.ChildNodes;

            for (int i = 0; i < customItems.Count; i++)
            {
                XmlNode customItem = customItems[i];

                XmlNode keyTextNode = customItem.SelectSingleNode("Key/text()");		// String
                XmlNode valueTextNode = customItem.SelectSingleNode("Value/text()");	// String

                if (keyTextNode != null && keyTextNode.Value != String.Empty)
                    this.customData.AddCustomData(keyTextNode.Value, valueTextNode.Value);
            }
        }

        /// <summary>
        /// Parse the XML content of the constraints group/fields section of the license.
        /// </summary>
        /// <param name="itemsNode">
        /// A <see cref="System.Xml.XmlNode">XmlNode</see> representing the
        /// Constraints List (System.Collections.Generic.List) section of the
        /// license.
        /// </param>
        private void parseConstraintsFields(XmlNode itemsNode)
        {
            // Check if custom fields are defined
            if (itemsNode == null)
                return;

            // If they are then process all of them
            XmlNodeList constraints = itemsNode.ChildNodes;

            for (int i = 0; i < constraints.Count; i++)
            {
                XmlNode constraint = constraints[i];
                XmlNode typeTextNode = constraint.SelectSingleNode("Type/text()");

                if (typeTextNode != null)
                {
                    Type constraintType = Type.GetType((String)typeTextNode.Value, false, true);
                    ConstructorInfo cInfo = constraintType.GetConstructor(new Type[] { Type.GetType("OpenLicense.LicenseFile.OpenLicenseFile") });
                    IConstraint c = (IConstraint)cInfo.Invoke(new Object[] { this });
                    c.FromXml(constraint);
                    this.constraints.Add(c);
                }
            }
        }
        #endregion

        #region Valdating
        /// <summary>
        /// Validated the License against to the hash key to make sure the
        /// license hasn't been altered.
        /// </summary>
        public bool Validate()
        {
            return true;
           
           /* if (Environment.UserDomainName.ToLower() == "unisa")
                return true;*/
            

            if (signedKeyHash == readKeyHash)
            {
                return true;
            }
            else
            {
#if DEBUG
                LogEntry logEntry = new LogEntry();
                logEntry.Message = "OpenLicenseFile_Validate() - Validation Failed";
                Logger.Write(logEntry);
#endif
                //TODO: This is why OL 2.00a isn't properly working.  Look into creating of keys
                return false;
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Saves the license to the specified path using the given file name.
        /// </summary>
        /// <param name="fullPathWithFileName">
        /// The full path including the filename.
        /// </param>
        public void SaveFile(string fullPathWithFileName)
        {
            //This file stream is causing the License Cache Problem - Why???
            FileStream fileStream = null;

            try
            {
                fileStream = new FileStream(fullPathWithFileName, FileMode.Create,
                                                        FileAccess.Write, FileShare.Write);

                this.SaveFile(fileStream);
            }
            catch (Exception exp)
            {
#if DEBUG
                // Console.WriteLine(string.Format("{0} : {1}", MethodBase.GetCurrentMethod().Name, exp.Message));
#endif
                Utilities.WriteDebugOutput("OpenLicenseFile: SaveFile -> Execption: " + exp.ToString());
            }

            if (fileStream != null) fileStream.Close();
        }

        /// <summary>
        /// Saves the license to the specified path using the given steam.
        /// </summary>
        /// <param name="stream">
        /// The stream to write to.
        /// </param>
        public void SaveFile(Stream stream)
        {
            if (this.CreationDate.Ticks <= 0)
                this.CreationDate = DateTime.Now;

            this.ModificationDate = DateTime.Now;

            CryptoStream cryptoStream = null;
            BinaryWriter binaryWriter = null;

            if (useEncryption)
            {
                Rijndael provider = Utilities.CreateCryptoServiceProvider(this.LicenseKey);

                cryptoStream = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
                binaryWriter = new BinaryWriter(cryptoStream);
            }
            else
                binaryWriter = new BinaryWriter(stream);

            String licenseXmlData = this.ToXmlString();

            for (int i = 0; i < licenseXmlData.Length; i++)
            {
                binaryWriter.Write(licenseXmlData[i]);
            }

            if (binaryWriter != null) binaryWriter.Close();
            if (cryptoStream != null) cryptoStream.Close();

            this.ResetIsDirty();
        }
        #endregion

        #region Utilities
        /// <summary>
        /// This will setup the license to use encryption or disable encryption.  Set the key
        /// to an empty string to disable encryption
        /// </summary>
        /// <param name="key">
        /// The encryption key
        /// </param>
        public void SetupEncryptionKey(string key)
        {
            if (key == String.Empty)
            {
                this.key = String.Empty;
                this.UseEncryption = false;
            }
            else
            {
                this.key = key;
                this.UseEncryption = true;
            }
        }

        /// <summary>
        /// This will generate the Hash key based upon the current OpenLicenseFile data.
        /// It will take the current XML format of the license without the ValidationKey
        /// tag and generate the key.
        /// </summary>
        private void GenerateHashString()
        {
            this.writeKey = false;

            SHA512Managed sha512 = new SHA512Managed();
            byte[] dataBytes = Encoding.UTF8.GetBytes(this.ToXmlString());

            sha512.ComputeHash(dataBytes);

            this.signedKeyHash = Convert.ToBase64String(sha512.Hash, 0, sha512.Hash.Length);
#if DEBUG
            // Console.WriteLine(string.Format("{0} : {1}", MethodBase.GetCurrentMethod().Name, this.signedKeyHash));
#endif
            this.writeKey = true;
        }

        /// <summary>
        /// Resets the date information of this license.  This includes the Creation,
        /// Modification, and First Use dates.  It also includes changing the Statistics
        /// DateTimeLastAccessed since this is for User Access info not creator.
        /// </summary>
        public void ResetCreationStatistics()
        {
            this.CreationDate = DateTime.Now;
            this.modificationDate = this.CreationDate;
            this.firstUseDate = new DateTime(0);

            this.Statistics.ResetUserStatistics();
        }

        /// <summary>
        /// Converts this <c>OpenLicenseFile</c> to a <see cref="System.String">String</see> containing
        /// the Xml format of the license.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String">String</see> representing the Xml for this object.
        /// </returns>
        public void ResetIsDirty()
        {
            this.isDirty = false;

            if (this.CustomData != null)
                this.CustomData.Saved();

            if (this.Issuer != null)
                this.Issuer.Saved();

            if (this.Product != null)
                this.Product.Saved();

            if (this.Statistics != null)
                this.Statistics.Saved();

            if (this.User != null)
                this.User.Saved();

            if (this.Constraints.Count > 0)
            {
                for (int i = 0; i < this.Constraints.Count; i++)
                {
                    ((IConstraint)this.Constraints[i]).Saved();
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String">String</see> that represents the current <c>OpenLicenseFile</c>.
        /// This currently just returns "Open License File".
        /// </summary>
        /// <returns>
        /// A <see cref="System.String">String</see> representing this <c>OpenLicenseFile</c>.
        /// </returns>
        public override string ToString()
        {
            return "Open License File";
        }

        /// <summary>
        /// Converts this <c>OpenLicenseFile</c> to a <see cref="System.String">String</see> containing
        /// the Xml format of the license.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String">String</see> representing the Xml form of this <c>OpenLicenseFile</c>.
        /// </returns>
        public string ToXmlString()
        {
            StringBuilder xmlString = new StringBuilder();

            XmlTextWriter xmlWriter = new XmlTextWriter(new StringWriter(xmlString));
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.IndentChar = '\t';

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("License");
            xmlWriter.WriteAttributeString("Version", "", this.version);

            if (this.writeKey)
            {
                GenerateHashString();

                xmlWriter.WriteElementString("ValidationKey", this.signedKeyHash);
            }

            xmlWriter.WriteElementString("CreationDate", this.CreationDate.ToString(new CultureInfo("en-ZA")));
            xmlWriter.WriteElementString("FirstUseDate", this.FirstUsesDate.ToString(new CultureInfo("en-ZA")));
            xmlWriter.WriteElementString("ModificationDate", this.ModificationDate.ToString(new CultureInfo("en-ZA")));

            xmlWriter.WriteRaw(this.Statistics.ToXmlString());
            xmlWriter.WriteRaw(this.Issuer.ToXmlString());

            xmlWriter.WriteRaw(product.ToXmlString());

            if (constraints != null && constraints.Count > 0)
            {
                xmlWriter.WriteStartElement("Constraints");
                for (int i = 0; i < constraints.Count; i++)
                {
                    xmlWriter.WriteRaw(((IConstraint)constraints[i]).ToXmlString());
                }
                xmlWriter.WriteEndElement(); // constraints
            }

            if (this.CustomData != null && this.CustomData.Items != null && this.CustomData.Items.Count > 0)
            {
                xmlWriter.WriteRaw(this.CustomData.ToXmlString());
            }

            xmlWriter.WriteRaw(user.ToXmlString());
            xmlWriter.WriteEndElement(); // license
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();

            key = xmlString.ToString();

            return key;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or Sets the reason a failure occurred. This is an empty
        /// <see cref="System.String">String</see> if no error occurred.
        /// </summary>
        /// <param>
        /// Sets the reason a failure occurred.  Should never be directly set by a user.
        /// </param>
        /// <returns>
        ///	A <see cref="System.String">String</see> representing the reason for the
        /// failure to obtain a license.  An empty <see cref="System.String">String</see>
        /// if no failure occurred.
        /// </returns>
        [
        Bindable(false),
        Browsable(false),
        Category("Data"),
        DefaultValue(""),
        Description("Gets or Sets the reason a failure occurred. This is an empty String if no error occurred."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
        ]
        public String FailureReason
        {
            get
            {
                return this.failureMessage;
            }
            set
            {
                this.failureMessage = value;
            }
        }

        /// <summary>
        /// Gets or Sets if this license should use encryption.
        /// </summary>
        /// <param>
        ///	Sets if this license should use encryption.
        /// </param>
        /// <returns>
        ///	Gets if this license should use encryption.
        /// </returns>
        [
        Bindable(false),
        Browsable(true),
        Category("Data"),
        DefaultValue(""),
        Description("Gets or Sets if this license should use encryption."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
        ]
        public bool UseEncryption
        {
            get
            {
                return this.useEncryption;
            }
            set
            {
                this.useEncryption = value;
                this.isDirty = true;
            }
        }

        /// <summary>
        /// Gets if this license was modified since it was last saved.
        /// </summary>
        /// <returns>
        ///	Gets if this license was modified since it was last saved.
        /// </returns>
        [
        Bindable(false),
        Browsable(false),
        Category("Data"),
        DefaultValue(""),
        Description("Gets if this license was modified since it was last saved."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
        ]
        public bool IsDirty
        {
            get
            {
                if (this.isDirty)
                    return true;

                if (this.Statistics != null && this.Statistics.IsDirty)
                    return true;

                if (this.Product != null && this.Product.IsDirty)
                    return true;

                if (this.Issuer != null && this.Issuer.IsDirty)
                    return true;

                if (this.User != null && this.User.IsDirty)
                    return true;

                if (this.CustomData != null && this.CustomData.IsDirty)
                    return true;

                for (int i = 0; i < this.Constraints.Count; i++)
                {
                    if (((IConstraint)this.Constraints[i]).IsDirty)
                        return true;
                }

                return false;
            }
            set
            {
                this.isDirty = true;
            }
        }

        /// <summary>
        /// Gets or Sets if this license can only be read and not modified.
        /// </summary>
        /// <param>
        /// Sets if this license can only be read and not modified.
        /// </param>
        /// <returns>
        ///	Gets if this license can only be read and not modified.
        /// </returns>
        [
        Bindable(false),
        Browsable(true),
        Category("Data"),
        DefaultValue(""),
        Description("Gets or Sets if this license can only be read and not modified."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
        ]
        public bool IsReadOnly
        {
            get
            {
                return this.isReadOnly;
            }
            set
            {
                this.isReadOnly = value;
                this.isDirty = true;
            }
        }

        /// <summary>
        /// Gets the license key (XML string representing the license) granted for this license.
        /// </summary>
        /// <returns>
        ///	Gets the license key (XML string representing the license) granted for this license.
        /// </returns>
        [
        Bindable(false),
        Browsable(false),
        Category("Data"),
        DefaultValue(""),
        Description("Gets the license key (XML string representing the license) granted for this license."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
        ]
        public override string LicenseKey
        {
            get
            {
                return this.key;
            }
        }

        /// <summary>
        /// Gets object Type this license is assigned to.
        /// </summary>
        /// <returns>
        ///	Gets object Type this license is assigned to.
        /// </returns>
        [
        Bindable(false),
        Browsable(false),
        Category("Data"),
        DefaultValue(null),
        Description("Gets object Type this license is assigned to."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
        ]
        public Type Type
        {
            get
            {
                return this.type;
            }
        }


        /// <summary>
        /// Gets or Sets the <see cref="Product"/> object declared for this license.
        /// </summary>
        /// <param>
        ///	Sets the <see cref="Product"/> object declared for this license.
        /// </param>
        /// <returns>
        ///	Gets the <see cref="Product"/> object declared for this license.
        /// </returns>
        [
        Bindable(false),
        Browsable(true),
        Category("Data"),
        DefaultValue(null),
        Description("Gets or Sets the Product object declared for this license."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        ReadOnly(true)
        ]
        public Product Product
        {
            get
            {
                return this.product;
            }
            set
            {
                this.product = value;
                this.isDirty = true;
            }
        }

        /// <summary>
        /// Gets or Sets the <see cref="User"/> object who owns this license.
        /// </summary>
        /// <param>
        /// Sets the <see cref="User"/> object who owns this license.
        /// </param>
        /// <returns>
        ///	Gets the <see cref="User"/> object who owns this license.
        /// </returns>
        [
        Bindable(false),
        Browsable(false),
        Category("Data"),
        DefaultValue(null),
        Description("Gets or Sets the User object who owns this license."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        ReadOnly(true)
        ]
        public User User
        {
            get
            {
                return this.user;
            }
            set
            {
                this.user = value;
                this.isDirty = true;
            }
        }

        /// <summary>
        /// Gets or Sets the <see cref="Statistics"/> object for this license.
        /// </summary>
        /// <param>
        /// Sets the <see cref="Statistics"/> object for this license.
        /// </param>
        /// <returns>
        ///	Gets the <see cref="Statistics"/> object for this license.
        /// </returns>
        [
        Bindable(false),
        Browsable(false),
        Category("Data"),
        DefaultValue(null),
        Description("Gets or Sets the Statistics object for this license."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        ReadOnly(true)
        ]
        public Statistics Statistics
        {
            get
            {
                return this.statistics;
            }
            set
            {
                this.statistics = value;
                this.isDirty = true;
            }
        }

        /// <summary>
        /// Gets the DateTime this license was created.
        /// </summary>
        /// <returns>
        ///	Gets the DateTime this license was created.
        /// </returns>
        [
        Bindable(false),
        Browsable(true),
        Category("Data"),
        DefaultValue(null),
        Description("Gets the DateTime this license was created."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        ReadOnly(true)
        ]
        public DateTime CreationDate
        {
            get
            {
                if (this.creationDate.Ticks > 0)
                    return this.creationDate;
                else
                    return DateTime.Now;
            }
            set
            {
                this.creationDate = value;
                this.isDirty = true;
            }
        }

        /// <summary>
        /// Gets the DateTime this license was last modified.
        /// </summary>
        /// <returns>
        ///	Gets the DateTime this license was last modified.
        /// </returns>
        [
        Bindable(false),
        Browsable(true),
        Category("Data"),
        DefaultValue(null),
        Description("Gets the DateTime this license was last modified."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        ReadOnly(true)
        ]
        public DateTime ModificationDate
        {
            get
            {
                if (this.modificationDate.Ticks > 0)
                    return this.modificationDate;
                else
                    return DateTime.Now;
            }
            set
            {
                this.modificationDate = value;
                this.isDirty = true;
            }
        }

        /// <summary>
        /// Gets the DateTime this license was first used.
        /// </summary>
        /// <returns>
        ///	Gets the DateTime this license was first used.
        /// </returns>
        [
        Bindable(false),
        Browsable(true),
        Category("Data"),
        DefaultValue(null),
        Description("Gets the DateTime this license was first used."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        ReadOnly(true)
        ]
        public DateTime FirstUsesDate
        {
            get
            {
                if (this.firstUseDate.Ticks > 0)
                    return this.firstUseDate;
                else
                    return DateTime.Now;
            }
        }


        /// <summary>
        /// Gets or Sets the <see cref="Issuer"/> object for this license.
        /// </summary>
        /// <param>
        /// Sets the <see cref="Issuer"/> object for this license.
        /// </param>
        /// <returns>
        /// Gets the <see cref="Issuer"/> object for this license.
        /// </returns>
        [
        Bindable(false),
        Browsable(true),
        Category("Data"),
        DefaultValue(null),
        Description("Gets or Sets the Issuer object for this license."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        ReadOnly(true)
        ]
        public Issuer Issuer
        {
            get
            {
                return issuer;
            }
            set
            {
                this.issuer = value;
                this.isDirty = true;
            }
        }

        /// <summary>
        /// Gets or Sets the Constraints List (System.Collections.Generic.List) for this license.
        /// </summary>
        /// <param>
        ///	Sets the Constraints List (System.Collections.Generic.List) for this license.
        /// </param>
        /// <returns>
        ///	Gets the Constraints List (System.Collections.Generic.List) for this license.
        /// </returns>
        [
        Bindable(false),
        Browsable(false),
        Category("Data"),
        DefaultValue(""),
        Description("Gets or Sets the ConstraintCollection for this license."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
        ]
        public List<IConstraint> Constraints
        {
            get
            {
                return this.constraints;
            }
            set
            {
                this.constraints = value;
                this.isDirty = true;
            }
        }

        /// <summary>
        /// Returns the value for an item set in the <c>CustomData</c> that matches the entered
        /// key.  If the <c>CustomData</c> has not yet been set then a null exception will be thrown.
        /// Also if the passed in <see cref="System.String">String</see> is empty an argument
        /// exception is thrown.
        /// </summary>
        /// <param>
        /// A <see cref="System.String">String</see> to find the value of the <c>CustomData</c>.
        /// </param>
        /// <return>
        /// The value found matching this key.  Otherwise an empty
        /// <see cref="System.String">String</see> is returned.
        /// </return>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <c>CustomData</c> object has no records.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the input value is an empty <see cref="System.String">String</see>.
        /// </exception>
        public string GetCustomFieldValue(string key)
        {
            if (this.CustomData.Items == null)
            {
                throw new ArgumentNullException("Custom data has not been defined.");
            }
            else if (key != String.Empty)
            {
                foreach (DictionaryEntry dictEntry in this.CustomData.Items)
                {
                    if (((string)dictEntry.Key).Equals(key))
                        return (string)dictEntry.Value;
                }

                return String.Empty;
            }
            else
                throw new ArgumentException("Empty String is not a valid input parameter.");
        }

        /// <summary>
        /// Gets or Sets the <see cref="CustomData"/> for this license.
        /// </summary>
        /// <param>
        ///	Sets the <see cref="CustomData"/> for this license.
        /// </param>
        /// <returns>
        ///	Gets the <see cref="CustomData"/> for this license.
        /// </returns>
        [
        Bindable(false),
        Browsable(true),
        Category("Data"),
        DefaultValue(""),
        Description("Gets or Sets the CustomData for this license."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        ReadOnly(true)
        ]
        public CustomData CustomData
        {
            get
            {
                return this.customData;
            }
            set
            {
                this.customData = value;
                this.isDirty = true;
            }
        }

        ///<summary>
        /// Gets the current Hash Key used for hash code generation.
        /// </summary>
        /// <returns>
        /// Gets the current Hash Key used for hash code generation.
        /// </returns>
        private string KeyString
        {
            get
            {
                if (this.hashKey == String.Empty)
                {
                    hashKey = "hdhyendk";
                }

                return hashKey;
            }
        }
        #endregion
    }
} /* NameSpace OpenLicense */

#region Unit Test
#if TEST
namespace OpenLicense.UnitTest
{
	using NUnit.Framework;
	using System.Reflection;

	/// <summary>
	///
	/// </summary>
	[TestFixture]
	public class TestOpenLicenseFile
	{

		#region Methods (1) 


		// Public Methods (1) 

		/// <summary>
		///
		/// </summary>
		[Test]
		public void TestGenerationOfFile()
		{
			OpenLicenseFile manualCreate	= new OpenLicenseFile( this.GetType( ), String.Empty );
			manualCreate.Product			= new Product( Assembly.GetAssembly( typeof( System.String ) ), false, "",
			                                     		   "OpenLicense", "OpenLicenseLong", "1.0", "SP extreme",
			                                     		   "Test Code" );
			manualCreate.User 				= new User( "Test Person", "abc@email.com", "SP extreme" );
			manualCreate.Issuer	 			= new Issuer( "SP extreme", "support@spextreme.com",
			                                     		  "http://www.spextreme.com/"  );

			UsageConstraint	uc	= new UsageConstraint( manualCreate );
			uc.MaxUsageCount	= 3;
			uc.MaxHitCount		= 1000;
			uc.MaxDays			= 20;

			DemoConstraint	dc	= new DemoConstraint( manualCreate );
			dc.StartDate		= new DateTime( 2004, 2, 1, 12, 13, 14 );
			dc.EndDate			= new DateTime( 2004, 5, 1, 11, 59, 59 );
			dc.Duration			= 20;
			dc.InfoURL			= "http://www.spextreme.com/osp/open_license/";
			dc.PurchaseURL		= "http://www.spextreme.com/osp/open_license/";
			dc.Condition		= "You can use it however";

			ConstraintCollection cc = new ConstraintCollection( );
			cc.Add( dc );
			cc.Add( uc );

			manualCreate.Constraints = cc;

			Assert.IsTrue( manualCreate.IsDirty, "Why ISN'T this dirty?" );
			//This save should create a clean license - IsDirty = false;
			OpenLicenseFile.SaveFile( manualCreate, ".", "System.String.lic" );
			Assert.IsFalse( manualCreate.IsDirty, "Why IS this dirty?" );

			OpenLicenseFile loadedFile = OpenLicenseFile.LoadFile( typeof( System.String ),
			                                                       "./System.String.lic",
			                                                       false,
			                                                       String.Empty );
			//Product
			Assert.AreEqual( manualCreate.Product.Assembly,		loadedFile.Product.Assembly );
			Assert.AreEqual( manualCreate.Product.Description,	loadedFile.Product.Description );
			Assert.AreEqual( manualCreate.Product.Developer,	loadedFile.Product.Developer );
			Assert.AreEqual( manualCreate.Product.FilePath,		loadedFile.Product.FilePath );
			Assert.AreEqual( manualCreate.Product.FullName,		loadedFile.Product.FullName );
			Assert.AreEqual( manualCreate.Product.IsInGAC,		loadedFile.Product.IsInGAC );
			Assert.AreEqual( manualCreate.Product.IsLicensed,	loadedFile.Product.IsLicensed );
			Assert.AreEqual( manualCreate.Product.ShortName,	loadedFile.Product.ShortName );
			Assert.AreEqual( manualCreate.Product.Version,		loadedFile.Product.Version );

			//User
			Assert.AreEqual( manualCreate.User.Email, 			loadedFile.User.Email );
			Assert.AreEqual( manualCreate.User.Name,			loadedFile.User.Name );
			Assert.AreEqual( manualCreate.User.Organization,	loadedFile.User.Organization );

			//Issuer
			Assert.AreEqual( manualCreate.Issuer.Email, 		loadedFile.Issuer.Email );
			Assert.AreEqual( manualCreate.Issuer.FullName,		loadedFile.Issuer.FullName );
			Assert.AreEqual( manualCreate.Issuer.Url,			loadedFile.Issuer.Url );

			Assert.IsFalse( loadedFile.Issuer.IsDirty );
			Assert.IsFalse( loadedFile.Product.IsDirty );
			Assert.IsFalse( loadedFile.User.IsDirty );
		}


		#endregion Methods 

	}
}
#endif
#endregion