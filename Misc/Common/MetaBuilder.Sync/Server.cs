using System;
using System.ComponentModel;
using OpenLicense;
using OpenLicense.LicenseFile;

namespace MetaBuilder.Sync
{


    [LicenseProvider(typeof(OpenLicenseProvider))]
    public class DevLicensedComponent
    {

		#region Constructors (1) 

        public DevLicensedComponent()
        {
           
            License license;
            LicenseManager.IsValid(this.GetType(), this, out license);
            if (license == null)
            {
                throw new ApplicationException("A suitable license file couldn't be found");
            }
            else if (((OpenLicenseFile)license).FailureReason != String.Empty)
            {
                throw new ApplicationException(((OpenLicenseFile)license).FailureReason);
            }
        }

		#endregion Constructors 

    }

   
}
