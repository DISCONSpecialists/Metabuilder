using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.Core;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Security.Principal;
using OpenLicense;
using OpenLicense.LicenseFile;
using System.Xml;
using System.Threading;
using MetaBuilder.UIControls;
using MetaBuilder.BusinessLogic;
using System.Diagnostics;
using System.Reflection;

namespace MetaBuilder.WinUI
{
    public class StartupTest
    {
        public StartupTest()
        {
            failures = new List<LoadFailure>();
        }

        private List<LoadFailure> failures;
        private bool hasSettingsFile = false;
        public bool HasErrors
        {
            get { return failures.Count > 0; }
        }
        private const string regStrKey = "LintPTRDstr";

        private bool requiresUpdate = false;
        public bool RequiresUpdate
        {
            get { return requiresUpdate; }
            set { requiresUpdate = value; }
        }
        public string requiresUpdateVersion = "";
        #region Tests

        private bool FirstRun = false;
        public void TestPermission()
        {
            //WindowsIdentity identity = WindowsIdentity.GetCurrent();
            //WindowsPrincipal principal = new WindowsPrincipal(identity);
            //bool isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator) || principal.IsInRole(WindowsBuiltInRole.PowerUser);
            //if (!isAdmin)
            //{
            //    LoadFailure UserPerm = new LoadFailure("User Permissions", "Metabuilder requires administrative permissions. Right click on your shortcut and choose 'Run as Administrator' or contact technical support");
            //    failures.Add(UserPerm);
            //}
            //permission to FOLDER
            if (CheckDirectoryAccess(Application.StartupPath) == false)
            {
                LoadFailure FolderPerm = new LoadFailure("Folder Permissions", "Metabuilder requires permissions to read and write to its root folder and the folders within it. Right click on your shortcut and choose 'Run as Administrator' or contact technical support");
                failures.Add(FolderPerm);
            }
        }
        public void TestFolders()
        {
            if (hasSettingsFile)
            {
                TestForInvalidFolder("Diagrams", Variables.Instance.DiagramPath);
                TestForInvalidFolder("Exports", Variables.Instance.ExportsPath);
                TestForInvalidFolder("SymbolStores", Variables.Instance.StencilPath);

                TestForInvalidFolder("Symbols", Core.Variables.Instance.SymbolPath);
                TestForInvalidFolder("SourceCode", Variables.Instance.SourceCodePath);
            }
        }
        private void TestForInvalidFolder(string name, string path)
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                catch (Exception ex)
                {
                    Core.Log.WriteLog("TestForInvalidFolder::" + ex.ToString());
                }
                if (!Directory.Exists(path))
                    failures.Add(new LoadFailure(name, "Cannot find the path: " + Environment.NewLine + "\t" + path));
            }
        }
        public void TestForSettingsFile()
        {
            // Instead of testing it - we're now creating one with defaults
            if (File.Exists(Application.StartupPath + "\\" + Core.FilterVariables.filterName))
            {
                hasSettingsFile = true;
            }
            else
            {
                FirstRun = true;
                MetaSettings s = new MetaSettings();
                //If no file, create it AND THEN SET DEFAULTS INCLUDING CONNECTION STRING FROM .config
                s.PutSetting(MetaSettings.GEN_DBCONNLOCAL, System.Configuration.ConfigurationManager.ConnectionStrings["netTiersConnectionString"].ConnectionString);
                s.PutSetting(MetaSettings.GEN_DBCONNSYNC, System.Configuration.ConfigurationManager.ConnectionStrings["netTiersConnectionString"].ConnectionString);
                s.PutSetting(MetaSettings.GEN_FULLNAME, Environment.UserName);
                s.PutSetting(MetaSettings.GEN_COMPANY, "Company Name");
                s.PutSetting(MetaSettings.VIEW_GRIDCELLSIZE, decimal.Parse("20", System.Globalization.CultureInfo.InvariantCulture));
                s.PutSetting(MetaSettings.VIEW_SHOWGRID, false);
                s.PutSetting(MetaSettings.VIEW_SNAPTOGRID, true);
                s.PutSetting(MetaSettings.PRINT_ARTEFACTLINES, false);
                s.PutSetting(MetaSettings.PRINT_COMMENTS, false);
                s.PutSetting(MetaSettings.PRINT_VCINDICATORS, false);
                s.PutSetting(MetaSettings.VIEW_VALIDATE_ON_OPEN, false);
                s.PutSetting(MetaSettings.AUTOSAVEENABLED, false);
                s.PutSetting(MetaSettings.AUTOSAVEINTERVAL, 5);
                s.PutSetting(MetaSettings.SAVETODATABASEENABLED, true);
                s.PutSetting(MetaSettings.CACHE_NUMBEROFOBJECTROWS, 250);
                s.PutSetting(MetaSettings.WARNDIAGRAMDIFFWORKSPACE, true);
                s.PutSetting(MetaSettings.VIEW_COMPARE_MO_WORKSPACES, false);
                s.PutSetting(MetaSettings.SPELLCHECK, false);
                s.PutSetting(MetaSettings.CUSTOMSUGGESTION, false);
                s.PutSetting(MetaSettings.VERBOSE_LOGGING, true);
                s.PutSetting(MetaSettings.DEFAULT_WORKSPACE, "");
                s.PutSetting(MetaSettings.DEFAULT_WORKSPACE_ID, 0);
                s.PutSetting(MetaSettings.USE_QUICKPANEL, true);
                s.PutSetting(MetaSettings.USE_SHALLOWCOPYCOLOR, true);
                s.PutSetting(MetaSettings.DEFAULT_FROM_PORT, "Bottom");
                s.PutSetting(MetaSettings.DEFAULT_TO_PORT, "Left");
                s.PutSetting(MetaSettings.VIEW_CHECK_DUPLICATES, true);
                s.PutSetting(MetaSettings.VIEW_CHECK_DUPLICATES_ACROSSWORKSPACES, true);
                s.PutSetting(MetaSettings.VIEW_CHECK_DUPLICATES_ALERT, false);
                hasSettingsFile = true;
            }
#if DEBUG
            FirstRun = true;
#endif
        }
        public void TestConnection()
        {
            if (hasSettingsFile)
            {
                Variables.Instance.CheckForUpdates = true;
                SqlConnection conn = new SqlConnection();
                try
                {
                    conn.ConnectionString = Variables.Instance.ConnectionString;
                    //conn.ConnectionString = "XX"; //FORCE FAIL
                    conn.Open();
                }
                catch
                {
                    MetaBuilder.MetaControls.SQLConnectionStringBuilder builder = new MetaBuilder.MetaControls.SQLConnectionStringBuilder();
                    builder.SQLConnectionString.ConnectionString = Variables.Instance.ConnectionString;
                    builder.Bind();

                    builder.StartPosition = FormStartPosition.Manual;
                    builder.Location = new System.Drawing.Point(SplashScreen.SplashScreen.SplashForm.Right + 1, SplashScreen.SplashScreen.SplashForm.Bottom + 1);

                    builder.TopMost = true;
                    builder.BringToFront();
                    builder.ShowDialog();

                    if (builder.DialogResult != DialogResult.OK)
                    {
                        LoadFailure DB = new LoadFailure("Database Connection", "Could not connect to the database");
                        failures.Add(DB);
                        return;
                    }
                    else
                    {
                        Variables.Instance.ConnectionString = builder.SQLConnectionString.ConnectionString;
                        MetaSettings s = new MetaSettings();
                        s.PutSetting(MetaSettings.GEN_DBCONNLOCAL, builder.SQLConnectionString.ConnectionString);
                        CoreInjector inject = new CoreInjector();
                        inject.InjectConnections();
                        conn.ConnectionString = Variables.Instance.ConnectionString;
                        conn.Open();
                    }
                }

                if (FirstRun)
                    updateUURI();

                if (Variables.Instance.CheckForUpdates)
                    CheckMetabuilderVersion();

                try
                {
                    TestDatabaseVersion(conn);
                }
                catch (Exception ex)
                {
                    Log.WriteLog(ex.ToString());
                    LoadFailure DB = new LoadFailure("Database Update", "A database update was unsuccessful. Please contact technical support.");
                    failures.Add(DB);
                }

                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }

        public bool TestServerConnection()
        {
            if (hasSettingsFile)
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = Variables.Instance.ServerConnectionString;
                try
                {
                    if (!(Core.Networking.Pinger.Ping(Variables.Instance.ServerConnectionString)))
                    {
                        return false;
                    }
                    conn.Open();

                    SqlDataAdapter dbCheckAdapter = new SqlDataAdapter("SELECT * FROM DatabasePatches", conn);
                    System.Data.DataSet dbCheckSet = new System.Data.DataSet();
                    bool created = false;
                    try
                    {
                        dbCheckAdapter.Fill(dbCheckSet);
                    }
                    catch
                    {
                        //create the table
                        string dbTableCreateString = "";
                        dbTableCreateString += "CREATE TABLE [dbo].[DatabasePatches](";
                        dbTableCreateString += "    [PatchIdentifier] uniqueidentifier NOT NULL,";
                        dbTableCreateString += "    [Description] [varchar](250) NULL,";
                        dbTableCreateString += ") ON [PRIMARY]";
                        SqlCommand dbCommand = new SqlCommand(dbTableCreateString, conn);
                        dbCommand.CommandType = System.Data.CommandType.Text;
                        dbCommand.ExecuteNonQuery();
                        created = true;
                    }
                    if (created)
                    {
                        dbCheckAdapter.Fill(dbCheckSet);
                    }
                    DatabaseVersioning ver = new DatabaseVersioning(conn, true);
                    ver.CheckVersions(dbCheckSet.Tables[0]);
                    dbCheckAdapter.Dispose();
                    dbCheckSet.Dispose();
                    ver.Dispose();

                    conn.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();

                    Log.WriteLog("Server Connection And Versioning" + Environment.NewLine + ex.ToString());
                    return false;
                }
            }
            return false;
        }
        private void TestDatabaseVersion(SqlConnection conn)
        {
            if (conn.State != System.Data.ConnectionState.Open)
            {
                failures.Add(new LoadFailure("Database Version", "Connection was not opened and database version cannot be verified"));
                return;
            }
            //check if version table exists (try selecting all from it into dataset)
            SqlDataAdapter dbCheckAdapter = new SqlDataAdapter("SELECT * FROM DatabasePatches", conn);
            System.Data.DataSet dbCheckSet = new System.Data.DataSet();
            bool created = false;
            try
            {
                dbCheckAdapter.Fill(dbCheckSet);
            }
            catch
            {
                //create the table
                string dbTableCreateString = "";
                dbTableCreateString += "CREATE TABLE [dbo].[DatabasePatches](";
                dbTableCreateString += "    [PatchIdentifier] uniqueidentifier NOT NULL,";
                dbTableCreateString += "    [Description] [varchar](250) NULL,";
                dbTableCreateString += ") ON [PRIMARY]";
                SqlCommand dbCommand = new SqlCommand(dbTableCreateString, conn);
                dbCommand.CommandType = System.Data.CommandType.Text;
                dbCommand.ExecuteNonQuery();
                created = true;
            }
            //check versions
            if (created)
            {
                dbCheckAdapter.Fill(dbCheckSet);
            }
            DatabaseVersioning ver = new DatabaseVersioning(conn, false);
            ver.CheckVersions(dbCheckSet.Tables[0]);
            dbCheckAdapter.Dispose();
            dbCheckSet.Dispose();
            ver.Dispose();

        }
        public void TestMetaModel()
        {
            if (!File.Exists(Variables.Instance.MetaAssembly) || !File.Exists(Variables.Instance.MetaSortAssembly))
            {
                try
                {
                    MetaBuilder.Sync.DevLicensedComponent devLic = new MetaBuilder.Sync.DevLicensedComponent();
                    //int.Parse("jasdlf"); //force fail
                }
                catch
                {
                    if (!File.Exists(Variables.Instance.MetaAssembly))
                        failures.Add(new LoadFailure("MetaModel", "Cannot find main assembly. A Developer licence is required to build one."));

                    if (!File.Exists(Variables.Instance.MetaSortAssembly))
                        failures.Add(new LoadFailure("MetaModel", "Cannot find sort assembly. A Developer licence is required to build one."));
                    return;
                }

                MessageBox.Show("You can rebuild it in the next window.", "Metamodel Assembly Missing", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Dictionary.ClassEditor editor = new MetaBuilder.WinUI.Dictionary.ClassEditor();
                editor.ShowDialog();

                if (!File.Exists(Variables.Instance.MetaAssembly))
                    failures.Add(new LoadFailure("MetaModel", "Cannot find main assembly"));

                if (!File.Exists(Variables.Instance.MetaSortAssembly))
                    failures.Add(new LoadFailure("MetaModel", "Cannot find sort assembly"));
            }
        }

        public void BuildEnums()
        {
            return;
            TList<DomainDefinition> definitions = DataAccessLayer.DataRepository.Domains(Core.Variables.Instance.ClientProvider);
            MetaBuilder.Meta.Builder.Compiler compiler = new MetaBuilder.Meta.Builder.Compiler(0, definitions.Count);

            foreach (DomainDefinition def in definitions)
            {
                //if (def.IsActive == false)
                //    continue;
                compiler.CreateEnum(Variables.MetaNameSpace, def.Name);
                TList<DomainDefinitionPossibleValue> values = DataAccessLayer.DataRepository.Provider.DomainDefinitionPossibleValueProvider.GetByDomainDefinitionID(def.pkid);
                values.Sort("Series");
                foreach (DomainDefinitionPossibleValue val in values)
                {
                    if (val.IsActive == false)
                        continue;
                    compiler.AddEnumValue(val.Description, val.PossibleValue, val.Series);
                }
                values = null;
                compiler.GenerateEnumCode();
            }
            try
            {
                compiler.CompileEnumCode();
                if (compiler.results != null && compiler.results.Errors.Count > 0)
                    Log.WriteLog(compiler.results.ToString());
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString());
            }
        }

        public void CacheAssociationHive()
        {
            AssociationManager assocmanager = AssociationManager.Instance;
            Log.WriteLog(MetaBuilder.BusinessFacade.MetaHelper.Singletons.GetAssociationHelper().AllowedAssociations.Allowed.Count + " active allowed associations and some with artifacts initialized");
        }

        public void updateUURI()
        {
            Log.WriteLog("Default URI");
            try
            {
                foreach (BusinessLogic.UURI uri in DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.UURIProvider.GetAll())
                {
                    uri.FileName = Application.StartupPath + "\\Metadata\\Images\\" + uri.FileName.Substring(uri.FileName.LastIndexOf("\\"));
                    uri.EntityState = MetaBuilder.BusinessLogic.EntityState.Changed;
#if !DEBUG
                    DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.UURIProvider.Save(uri);
#endif
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString());
            }
        }

        public void CheckMetabuilderVersion()
        {
            Thread thread = new Thread(new ThreadStart(CheckMetabuilderVersionThread));
            thread.Name = "MBVCt";
            thread.Start();
        }
        public void CheckMetabuilderVersionThread()
        {
            try
            {
                if (!Core.Networking.Pinger.PingAddress("http://www.metabuilder.co.za"))
                    return;
                string currentVersion = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).FileVersion.ToString();
                using (MetaBuilder.WinUI.Metabuilder.co.za.Licencing.MetabuilderLicencing lic = new MetaBuilder.WinUI.Metabuilder.co.za.Licencing.MetabuilderLicencing())
                {
                    requiresUpdateVersion = lic.CheckVersion(currentVersion);
                }
                //if (requiresUpdateVersion.Contains(currentVersion))
                if (CompareVersions(currentVersion, requiresUpdateVersion.ToLower().Replace(".exe", "").Replace("metabuilder-", "")) >= 0)
                    requiresUpdate = false;
                else
                    requiresUpdate = true;
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString());
            }
        }
        /// <summary>
        /// Compare versions of form "1,2,3,4" or "1.2.3.4". Throws FormatException
        /// in case of invalid version.
        /// </summary>
        /// <param name="strA">the first version</param>
        /// <param name="strB">the second version</param>
        /// <returns>less than zero if strA is less than strB, equal to zero if
        /// strA equals strB, and greater than zero if strA is greater than strB</returns>
        public static int CompareVersions(String strA, String strB)
        {
            Log.WriteLog("StartupTest::CompareVersions::CURRENT [" + strA + "] to REMOTE [" + strB + "]");
            if (strA == strB)
                return 0;
            Version vA = new Version(strA.Replace(",", "."));
            Version vB = new Version(strB.Replace(",", "."));
            return vA.CompareTo(vB);
        }

        public void ValidateRegistration()
        {
            Thread t = new Thread(new ThreadStart(ValidateRegistrationThread));
            t.Name = "MBRVt";
            t.Start();
        }
        public void ValidateRegistrationThread()
        {
            bool canConnect = false;

            #region Desktop
            try
            {
                //Check reg ie. docking form .lic file exists... //DOES NOT CAUSE METABUILDER TO NOT OPEN
                if (File.Exists(Application.StartupPath + "\\MetaBuilder.UIControls.GraphingUI.DockingForm.lic"))
                {
                    //Get xml
                    StreamReader sr = new StreamReader(Application.StartupPath + "\\MetaBuilder.UIControls.GraphingUI.DockingForm.lic");
                    string xml = sr.ReadToEnd();
                    sr.Close();
                    //open licence
                    OpenLicenseFile file = new OpenLicenseFile();
                    file.LoadFromXml(xml);
                    string regKey = file.CustomData.Items["regkey"].ToString();
                    //it was just used...
                    file.ModificationDate = DateTime.Now;
                    file.SaveFile(Application.StartupPath + "\\MetaBuilder.UIControls.GraphingUI.DockingForm.lic");
                    //check/log  online
                    if (Core.Networking.Pinger.PingAddress("http://www.metabuilder.co.za/default.aspx"))//MessageBox.Show(this,"Can contact metabuilder licencing service", "Licencing");
                    {
                        canConnect = true;
                        //use key to check if you have a dockingform licence?
                        MetaBuilder.WinUI.Metabuilder.co.za.Licencing.MetabuilderLicencing lic = new MetaBuilder.WinUI.Metabuilder.co.za.Licencing.MetabuilderLicencing();
                        string ret = lic.LicenceUsage("MetaBuilder.UIControls.GraphingUI.DockingForm", regKey, Environment.UserName, Environment.UserDomainName);
                        Log.WriteLog(ret.ToString());
                    }
                }
                else
                {
                    //no problem the regform will show and 'create' a licence after the startup tests have completed.
                    Log.WriteLog("StartupTest::ValidateRegistration::DockingForm.lic not found");
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString());
            }
            #endregion

            #region Server
            try
            {
                //Check reg ie. docking form .lic file exists... //DOES NOT CAUSE METABUILDER TO NOT OPEN
                if (File.Exists(Application.StartupPath + "\\MetaBuilder.UIControls.GraphingUI.ObjectManager.lic"))
                {
                    //Get xml
                    StreamReader sr = new StreamReader(Application.StartupPath + "\\MetaBuilder.UIControls.GraphingUI.ObjectManager.lic");
                    string xml = sr.ReadToEnd();
                    sr.Close();
                    //open licence
                    OpenLicenseFile file = new OpenLicenseFile();
                    file.LoadFromXml(xml);
                    string regKey = file.CustomData.Items["regkey"].ToString();
                    //it was just used...
                    file.ModificationDate = DateTime.Now;
                    file.SaveFile(Application.StartupPath + "\\MetaBuilder.UIControls.GraphingUI.ObjectManager.lic");
                    //check/log  online
                    //use key to check if you have a dockingform licence?
                    if (canConnect)
                    {
                        MetaBuilder.WinUI.Metabuilder.co.za.Licencing.MetabuilderLicencing lic = new MetaBuilder.WinUI.Metabuilder.co.za.Licencing.MetabuilderLicencing();
                        string ret = lic.LicenceUsage("MetaBuilder.UIControls.GraphingUI.ObjectManager", regKey, Environment.UserName, Environment.UserDomainName);
                        Log.WriteLog(ret.ToString());
                    }
                }
                else
                {
                    //no problem the regform will show and 'create' a licence after the startup tests have completed.
                    Log.WriteLog("StartupTest::ValidateRegistration::ObjectManager.lic not found");
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString());
            }
            #endregion

            #region Development
            try
            {
                //Check reg ie. docking form .lic file exists... //DOES NOT CAUSE METABUILDER TO NOT OPEN
                if (File.Exists(Application.StartupPath + "\\MetaBuilder.Sync.DevLicensedComponent.lic"))
                {
                    //Get xml
                    StreamReader sr = new StreamReader(Application.StartupPath + "\\MetaBuilder.Sync.DevLicensedComponent.lic");
                    string xml = sr.ReadToEnd();
                    sr.Close();
                    //open licence
                    OpenLicenseFile file = new OpenLicenseFile();
                    file.LoadFromXml(xml);
                    string regKey = file.CustomData.Items["regkey"].ToString();
                    //it was just used...
                    file.ModificationDate = DateTime.Now;
                    file.SaveFile(Application.StartupPath + "\\MetaBuilder.Sync.DevLicensedComponent.lic");
                    //check/log  online
                    //use key to check if you have a dockingform licence?
                    if (canConnect)
                    {
                        MetaBuilder.WinUI.Metabuilder.co.za.Licencing.MetabuilderLicencing lic = new MetaBuilder.WinUI.Metabuilder.co.za.Licencing.MetabuilderLicencing();
                        string ret = lic.LicenceUsage("MetaBuilder.Sync.DevLicensedComponent", regKey, Environment.UserName, Environment.UserDomainName);
                        Log.WriteLog(ret.ToString());
                    }
                }
                else
                {
                    //no problem the regform will show and 'create' a licence after the startup tests have completed.
                    Log.WriteLog("StartupTest::ValidateRegistration::DevComponent.lic not found");
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString());
            }
            #endregion

            Core.Variables.Instance.ValidatedRegistration = true;

            //skip Registry licence check
            return;
            #region Registry
            //Licence file must always exist at this point
            if (!File.Exists(Application.StartupPath + "\\MetaBuilder.UIControls.GraphingUI.DockingForm.lic"))
            {
                //this is not a failure could be first run
                //failures.Add(new LoadFailure("Registration Validation", "Your registration was unable to be validated because a suitable licence cannot be found"));
                return;
            }
            //MetaBuilder.UIControls.GraphingUI.DockingForm.lic MAIN METABUILDER INTERFACE LICENCE FILE
            DateTime fileCreationDate = File.GetCreationTime(Application.StartupPath + "\\MetaBuilder.UIControls.GraphingUI.DockingForm.lic");
            //string regTime = "";
            DateTime? registryCreationDate = null;
            try
            {
                //regTime = Settings.SystemWide.GetString(regStrKey);
                if (Settings.SystemWide.GetString(regStrKey).Length > 0)
                    registryCreationDate = DateTime.Parse(Settings.SystemWide.GetString(regStrKey));
            }
            catch
            {
                registryCreationDate = null;
            }
            //if there is no date in the registry but there is a file, write that date.
            if (registryCreationDate == null)
            {
                Settings.SystemWide.SetString(regStrKey, fileCreationDate.ToString());
                registryCreationDate = DateTime.Parse(Settings.SystemWide.GetString(regStrKey));
            }
            //if a creation date of the dockingform.lic file is found in the registry check it against this dockingform.lic files date.
            if (fileCreationDate.ToLongDateString() != registryCreationDate.Value.ToLongDateString() && fileCreationDate.ToShortTimeString() != registryCreationDate.Value.ToShortTimeString())
            {
                //if no match occurs but there was a date in the registry then the licence file has been manually circumvented which this check prevents against.
                //DC this biatch
                failures.Add(new LoadFailure("Registration Validation", "Your registration was unable to be validated"));
            }
            #endregion

        }

        #endregion

        private const string TEMP_FILE = "\\tempFile.tmp";
        /// <summary>
        /// Checks the ability to create and write to a file in the supplied directory.
        /// </summary>
        /// <param name="directory">String representing the directory path to check.</param>
        /// <returns>True if successful; otherwise false.</returns>
        private static bool CheckDirectoryAccess(string directory)
        {
            Log.WriteLog("Checking directory permission");
            bool success = false;
            string fullPath = directory + TEMP_FILE;

            if (Directory.Exists(directory))
            {
                try
                {
                    using (FileStream fs = new FileStream(fullPath, FileMode.CreateNew, FileAccess.Write))
                    {
                        fs.WriteByte(0xff);
                    }

                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                        success = true;
                    }
                }
                catch (Exception)
                {
                    success = false;
                }
            }
            return success;
        }

        public void DisplayBox()
        {
            if (failures.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Errors occurred during startup:" + Environment.NewLine + Environment.NewLine);
                for (int i = 0; i < failures.Count; i++)
                {
                    sb.Append("\t" + failures[i].ToString() + Environment.NewLine);
                }

                MessageBox.Show(sb.ToString(), "Startup Failures", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                LogEntry logEntry = new LogEntry();
                logEntry.Message = sb.ToString();
                Logger.Write(logEntry);
            }
        }

    }

    public class LoadFailure
    {
        public LoadFailure(string name, string message)
        {
            FailureType = name;
            ErrorMessage = message;
        }
        private string failureType;
        public string FailureType
        {
            get { return failureType; }
            set { failureType = value; }
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }

        public override string ToString()
        {
            return errorMessage;
        }

    }
}