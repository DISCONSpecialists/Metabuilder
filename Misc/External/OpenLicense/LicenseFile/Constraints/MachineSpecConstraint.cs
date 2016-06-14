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
using System.IO;
using System.Text;
using System.Xml;

namespace OpenLicense.LicenseFile.Constraints
{


	public class MachineSpecConstraint : AbstractConstraint
	{

		#region Fields (4) 

/*
        public class MotherboardID
        {
            string motherboardID;
            public string GetMotherboardID()
            {
                if (motherboardID != null)
                    return motherboardID;
                try
                {
                    SelectQuery query = new SelectQuery("Win32_BaseBoard");
                    ManagementObjectSearcher search = new ManagementObjectSearcher(query);
                    foreach (ManagementObject info in search.Get())
                    {
                        motherboardID = info["SerialNumber"].ToString();
                    }
                }
                catch
                {
                }
                return motherboardID;

            }
        }
        public class CPUId
        {
            string procID;
            public string GetProcessorID()
            {
                if (procID != null)
                    return procID;
                try
                {
                    SelectQuery query = new SelectQuery("Win32_processor");
                    ManagementObjectSearcher search = new ManagementObjectSearcher(query);
                    foreach (ManagementObject info in search.Get())
                    {
                        procID = info["processorId"].ToString();
                    }
                }
                catch
                {
                }
                return procID;

            }
        }
        private string currentProcessorID;
        private string currentMotherboardID;
        private string motherboardID;
        public string MBID
        {
            get { return motherboardID; }
            set { motherboardID = value; }
        }
        private string processorID;
        public string ProcessorID
        {
            get { return processorID; }
            set { processorID = value; }
        }
        */
        private string currentUsername;
        private string currentWindowsDomain;
        private string userName;
        private string windowsDomainName;

		#endregion Fields 

		#region Constructors (2) 

		/// <summary>
		/// This is the constructor for the <c>MachineSpecConstraint</c>.  The constructor
		/// is used to create the object with a valid license to attach it to.
		/// </summary>
		public MachineSpecConstraint( ) : this( null ) { }

        public MachineSpecConstraint(OpenLicenseFile license)
		{
			base.License		= license;
			base.Name			= "MachineSpecConstraint Constraint";
			base.Description	=  "This MachineSpecConstraint constrains the user to running within a given ";
			base.Description	+= " hardware configuration.";

           /* MotherboardID mbID = new MotherboardID();
            this.currentMotherboardID = mbID.GetMotherboardID();

            CPUId cpuid = new CPUId();
            this.currentProcessorID = cpuid.GetProcessorID();*/

            currentUsername = Environment.UserName;
            currentWindowsDomain = Environment.UserDomainName;
			try
			{
			}
			catch
			{
			}
		}

		#endregion Constructors 

		#region Properties (2) 

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public string WindowsDomainName
        {
            get { return windowsDomainName; }
            set { windowsDomainName = value; }
        }

		#endregion Properties 

		#region Methods (4) 


		// Public Methods (4) 

		/// <summary>
		/// This is responsible for parsing a <see cref="System.String">String</see>
		/// to form the <c>DomainConstriant</c>.
		/// </summary>
		/// <param name="xmlData">
		/// The Xml data in the form of a <c>String</c>.
		/// </param>
		public override void FromXml( string xmlData )
		{
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml( xmlData );

			FromXml( xmlDoc.SelectSingleNode("/Constraint") );
		}

		/// <summary>
		/// This creates a <c>MachineSpecConstraint</c> from an <see cref="System.Xml.XmlNode">XmlNode</see>.
		/// </summary>
		/// <param name="itemsNode">
		/// A <see cref="XmlNode">XmlNode</see> representing the <c>DomainConstraint</c>.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Thrown if the <see cref="XmlNode">XmlNode</see> is null.
		/// </exception>
		public override void FromXml( XmlNode itemsNode )
		{
			if( itemsNode == null )
				throw new ArgumentNullException( "The license data is null." );

			XmlNode		nameTextNode		= itemsNode.SelectSingleNode( "Name/text()" );
			XmlNode		descriptionTextNode	= itemsNode.SelectSingleNode( "Description/text()" );
			//XmlNodeList	machSpecNodeList		= itemsNode.SelectNodes( "MachineSpec" );

			if( nameTextNode != null )
				Name 		= nameTextNode.Value;

			if( descriptionTextNode != null )
				Description	= descriptionTextNode.Value;


            XmlNode windowsDomainNode = itemsNode.SelectSingleNode("WindowsDomain/text()");
            this.windowsDomainName = windowsDomainNode.Value;

            XmlNode usernameNode = itemsNode.SelectSingleNode("Username/text()");
            this.userName = usernameNode.Value;

            /*
            XmlNode procIDNode = itemsNode.SelectSingleNode("ProcessorID/text()");
            processorID = procIDNode.Value;

            XmlNode mbIDNode = itemsNode.SelectSingleNode("MotherboardID/text()");
            motherboardID = mbIDNode.Value;*/


		}

		/// <summary>
		/// Converts this <c>DomainConstraint</c> to an Xml <c>String</c>.
		/// </summary>
		/// <returns>
		/// A <c>String</c> representing the DomainConstraint as Xml data.
		/// </returns>
		public override string ToXmlString( )
		{
			StringBuilder xmlString = new StringBuilder( );

			XmlTextWriter xmlWriter = new XmlTextWriter( new StringWriter( xmlString ) );
			xmlWriter.Formatting = Formatting.Indented;

			xmlWriter.WriteStartElement( "Constraint" );
				xmlWriter.WriteElementString( "Name", 				this.Name );
				xmlWriter.WriteElementString( "Type",				this.GetType( ).ToString( ) );
				xmlWriter.WriteElementString( "Description",		this.Description );

                xmlWriter.WriteElementString("WindowsDomain", this.windowsDomainName);
                xmlWriter.WriteElementString("Username", this.userName);
                /*xmlWriter.WriteElementString("ProcessorID", this.processorID);
                xmlWriter.WriteElementString("MotherboardID", this.motherboardID);*/

			
			xmlWriter.WriteEndElement( ); // Constraint

			xmlWriter.Close( );

			return xmlString.ToString( );
		}

		/// <summary>
		/// <p>This verifies the license meets its desired validation criteria.  This includes
		/// validating that the license is run within the set of given domains.  If the license
		/// is able to match a domain with the current domain then it will be successful (true).
		/// Otherwise it will return false and set the failure reason to the reason for the
		/// failure.</p>
		/// </summary>
		/// <returns>
		/// <c>True</c> if the license meets the validation criteria.  Otherwise
		/// <c>False</c>.
		/// </returns>
		/// <remarks>
		/// When a failure occurs the FailureReason will be set to: "The current domain is not
		/// supported by this license."
		/// </remarks>
		public override bool Validate( )
		{
            return true;
            int failureCount = 0;
/*
            if (motherboardID != currentMotherboardID)
                failureCount++;

            if (processorID != currentProcessorID)
                failureCount ++;*/
/*
           if (userName != currentUsername)
                failureCount ++;

            if (windowsDomainName != currentWindowsDomain)
                failureCount ++;
            */
            if (failureCount > 0)
            {
                base.License.FailureReason = "The current hardware configuration is not supported by this license.";
            }
            else
                return true;
	
            /*
			if( base.License != null )
				base.License.FailureReason = "The current domain is not supported by this license.";*/

			return false;
		}


		#endregion Methods 

	}
} /* NameSpace OpenLicense */

