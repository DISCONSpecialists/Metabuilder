using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using OpenLicense;
using System.Data.SqlClient;
using System.Data;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class Service : System.Web.Services.WebService
{
    public Service () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string GetLicense(string type, string name, string email, string company, string regkey,string Username, string UserdomainName, SerializableDictionary<string,string> customValues)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "GetKey";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["RegConn"].ConnectionString);
        cmd.Parameters.Add(new SqlParameter("@RegKey", SqlDbType.VarChar, 50));
        cmd.Parameters["@RegKey"].Direction = ParameterDirection.InputOutput;

        cmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.VarChar, 100));
        cmd.Parameters.Add(new SqlParameter("@UserDomain", SqlDbType.VarChar, 100));
        cmd.Parameters.Add(new SqlParameter("@TypeName", SqlDbType.VarChar, 200));
        cmd.Parameters.Add(new SqlParameter("@RetVal", SqlDbType.VarChar, 100));
        cmd.Parameters["@Retval"].Direction = ParameterDirection.Output;

        cmd.Parameters["@RegKey"].Value = regkey;
        cmd.Parameters["@Username"].Value = Username;
        cmd.Parameters["@UserDomain"].Value = UserdomainName;
        cmd.Parameters["@TypeName"].Value = type;

        cmd.Parameters.Add(new SqlParameter("@LimitDays", SqlDbType.Int));
        cmd.Parameters["@LimitDays"].Direction = ParameterDirection.Output;

        cmd.Parameters.Add(new SqlParameter("@LimitUntilDate", SqlDbType.DateTime));
        cmd.Parameters["@LimitUntilDate"].Direction = ParameterDirection.Output;

        cmd.Parameters.Add(new SqlParameter("@LimitStartHour", SqlDbType.Int));
        cmd.Parameters["@LimitStartHour"].Direction = ParameterDirection.Output;

        cmd.Parameters.Add(new SqlParameter("@LimitEndHour", SqlDbType.Int));
        cmd.Parameters["@LimitEndHour"].Direction = ParameterDirection.Output;


        cmd.Parameters.Add(new SqlParameter("@LimitVersionMinimumMajor", SqlDbType.Int));
        cmd.Parameters["@LimitVersionMinimumMajor"].Direction = ParameterDirection.Output;

        cmd.Parameters.Add(new SqlParameter("@LimitVersionMinimumMinor", SqlDbType.Int));
        cmd.Parameters["@LimitVersionMinimumMinor"].Direction = ParameterDirection.Output;

        cmd.Parameters.Add(new SqlParameter("@LimitVersionMaximumMajor", SqlDbType.Int));
        cmd.Parameters["@LimitVersionMaximumMajor"].Direction = ParameterDirection.Output;

        cmd.Parameters.Add(new SqlParameter("@LimitVersionMaximumMinor", SqlDbType.Int));
        cmd.Parameters["@LimitVersionMaximumMinor"].Direction = ParameterDirection.Output;


        if (cmd.Connection.State != ConnectionState.Open)
        {
            cmd.Connection.Open();
        }

        cmd.ExecuteNonQuery();
        string key = cmd.Parameters["@Retval"].Value.ToString();

        if (key.Length == 0)
            return "";
        else
        {

            OpenLicense.LicenseFile.OpenLicenseFile licenseFile = new OpenLicense.LicenseFile.OpenLicenseFile(Type.GetType(type), string.Empty);
            licenseFile.CustomData.AddCustomData("RegKey", regkey);


            if (cmd.Parameters["@LimitDays"].Value.ToString().Length > 0)
            {
                int LimitDays = int.Parse(cmd.Parameters["@LimitDays"].Value.ToString());
                OpenLicense.LicenseFile.Constraints.DemoConstraint demoConstraint = new OpenLicense.LicenseFile.Constraints.DemoConstraint();
                demoConstraint.Name = "Day Limitation Constraint";
                demoConstraint.Duration = LimitDays;
                licenseFile.Constraints.Add(demoConstraint);
            }

            if (cmd.Parameters["@LimitUntilDate"].Value.ToString().Length > 0)
            {
                DateTime limitUntil = DateTime.Parse(cmd.Parameters["@LimitUntilDate"].Value.ToString());
                OpenLicense.LicenseFile.Constraints.DemoConstraint demoConstraint2 = new OpenLicense.LicenseFile.Constraints.DemoConstraint();
                demoConstraint2.Name = "Date limitation constraint";
                demoConstraint2.StartDate = DateTime.Now;
                demoConstraint2.EndDate = limitUntil;
                licenseFile.Constraints.Add(demoConstraint2);
            }

            if (cmd.Parameters["@LimitStartHour"].Value.ToString().Length > 0 && cmd.Parameters["@LimitEndHour"].Value.ToString().Length > 0)
            {
                int LimitStartHour = int.Parse(cmd.Parameters["@LimitStartHour"].Value.ToString());
                int LimitEndHour = int.Parse(cmd.Parameters["@LimitEndHour"].Value.ToString());

                OpenLicense.LicenseFile.Constraints.DayTimeConstraint dayTimeConstraint = new OpenLicense.LicenseFile.Constraints.DayTimeConstraint();
                OpenLicense.LicenseFile.Constraints.TimeRange trange = new OpenLicense.LicenseFile.Constraints.TimeRange();
                trange.Start = new OpenLicense.LicenseFile.Constraints.Time(LimitStartHour);
                trange.End = new OpenLicense.LicenseFile.Constraints.Time(LimitEndHour);
                dayTimeConstraint.Items.Add(trange);
                licenseFile.Constraints.Add(dayTimeConstraint);
            }

            if (cmd.Parameters["@LimitVersionMinimumMajor"].Value.ToString().Length > 0
                &&
                cmd.Parameters["@LimitVersionMinimumMinor"].Value.ToString().Length > 0
&&
                cmd.Parameters["@LimitVersionMaximumMajor"].Value.ToString().Length > 0
&&
                cmd.Parameters["@LimitVersionMaximumMinor"].Value.ToString().Length > 0)
            {
                int minMaj = int.Parse(cmd.Parameters["@LimitVersionMinimumMajor"].Value.ToString());
                int minMin = int.Parse(cmd.Parameters["@LimitVersionMinimumMinor"].Value.ToString());
                int maxMaj = int.Parse(cmd.Parameters["@LimitVersionMinimumMinor"].Value.ToString());
                int maxMin = int.Parse(cmd.Parameters["@LimitVersionMinimumMinor"].Value.ToString());

                OpenLicense.LicenseFile.Constraints.VersionConstraint vConstraint = new OpenLicense.LicenseFile.Constraints.VersionConstraint();
                vConstraint.Maximum = new Version(maxMaj, maxMin);
                vConstraint.Minimum = new Version(minMaj, minMin);
                licenseFile.Constraints.Add(vConstraint);
            }




            // Save the info (first only username & domain), then customfields
            SqlCommand cmdInserter = new SqlCommand("SetKeyFields", cmd.Connection);
            cmdInserter.CommandType = CommandType.StoredProcedure;

            cmdInserter.Parameters.Add(new SqlParameter("@RegKey", SqlDbType.VarChar, 100));
            cmdInserter.Parameters["@RegKey"].Value = key;
            cmdInserter.Parameters.Add(new SqlParameter("@Username", SqlDbType.VarChar, 100));
            cmdInserter.Parameters.Add(new SqlParameter("@UserDomain", SqlDbType.VarChar, 100));
            cmdInserter.Parameters.Add(new SqlParameter("@TypeName", SqlDbType.VarChar, 200));
            cmdInserter.Parameters["@Username"].Value = Username;
            cmdInserter.Parameters["@UserDomain"].Value = UserdomainName;
            cmdInserter.Parameters["@TypeName"].Value = type;

            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }

            cmdInserter.ExecuteNonQuery();


            SqlCommand cmdInserterCustom = new SqlCommand("SetKeyCustomFields", cmd.Connection);
            cmdInserterCustom.CommandType = CommandType.StoredProcedure;
            cmdInserterCustom.Parameters.Add(new SqlParameter("@RegKey", SqlDbType.VarChar, 100));
            cmdInserterCustom.Parameters["@RegKey"].Value = key;
            cmdInserterCustom.Parameters.Add(new SqlParameter("@FieldName", SqlDbType.VarChar, 50));
            cmdInserterCustom.Parameters.Add(new SqlParameter("@FieldValue", SqlDbType.VarChar, 100));
            cmdInserterCustom.Parameters.Add(new SqlParameter("@TypeName", SqlDbType.VarChar, 200));
            cmdInserterCustom.Parameters["@TypeName"].Value = type;


            cmdInserterCustom.Parameters["@FieldName"].Value = "Email";
            cmdInserterCustom.Parameters["@FieldValue"].Value = email;
            cmdInserterCustom.ExecuteNonQuery();

            cmdInserterCustom.Parameters["@FieldName"].Value = "Organisation";
            cmdInserterCustom.Parameters["@FieldValue"].Value = company;

            cmdInserterCustom.Parameters["@FieldName"].Value = "Name";
            cmdInserterCustom.Parameters["@FieldValue"].Value = name;

            cmdInserterCustom.ExecuteNonQuery();
    
            foreach (string sKey in customValues.Keys)
            {
                if (customValues[sKey] != null)
                {
                    cmdInserterCustom.Parameters["@FieldName"].Value = sKey;
                    cmdInserterCustom.Parameters["@FieldValue"].Value = customValues[sKey];
                    cmdInserterCustom.ExecuteNonQuery();
                }
            }


            //licenseFile.SetupEncryptionKey("moklet");

            licenseFile.Constraints.Add(new OpenLicense.LicenseFile.Constraints.RuntimeConstraint());
            OpenLicense.LicenseFile.Constraints.MachineSpecConstraint machConstraint = new OpenLicense.LicenseFile.Constraints.MachineSpecConstraint();
            machConstraint.UserName = Username;
            machConstraint.WindowsDomainName = UserdomainName;
            licenseFile.Constraints.Add(machConstraint);
            licenseFile.Issuer = new OpenLicense.LicenseFile.Issuer();
            licenseFile.Issuer.Email = "deonf@discon.co.za";
            licenseFile.Issuer.FullName = "Deon Fourie";
            licenseFile.Issuer.Url = "http://www.metabuilder.co.za";
            licenseFile.UseEncryption = false;
            licenseFile.User = new OpenLicense.LicenseFile.User();
            licenseFile.User.Email = email;
            licenseFile.User.Name = name;
            licenseFile.User.Organization = company;
            licenseFile.Product.IsLicensed = true;
            return licenseFile.ToXmlString();
        }
        return "";
    }
    
}
