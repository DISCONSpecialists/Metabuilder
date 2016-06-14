using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.UIControls;
using MetaBuilder.UIControls.Dialogs;
using MetaBuilder.UIControls.Dialogs.RelationshipManager;
using MetaBuilder.UIControls.GraphingUI;
using MetaBuilder.WinUI.Dictionary;
using MetaBuilder.WinUI.Import;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using scr = MetaBuilder.SplashScreen;
using MetaBuilder.BusinessFacade.MetaHelper;
using System.Reflection;

namespace MetaBuilder.WinUI
{
    public class AppLoader
    {
        private static DockingForm f;
        //private static string[] FilesToOpenOnStartup;
        private static bool m_bLayoutCalled = false;
        private static DateTime m_dt;

        private static void myReceive(string[] args)
        {
            //System.Threading.Thread.Sleep(250);
            while (m_bLayoutCalled == false)
            {

            }
            for (int i = 0; i < args.Length; i++)
            {
                try
                {
                    string arg = args[i];
                    DockingForm.DockForm.Invoke(DockingForm.DockForm.delegateOpenFile, new object[] { arg, false });
                    //LogEntry entry = new LogEntry();
                    //entry.Message = "Called the delegate";
                    //Logger.Write(entry);
                }
                catch (Exception x)
                {
                    LogEntry entry = new LogEntry();
                    entry.Message = x.ToString();
                    Logger.Write(entry);
                }
            }
        }

        private static void RemoveEventLogTraceFromConfigFile()
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.Load(Application.StartupPath + "\\metabuilder.exe.config");
            bool removed = false;
            foreach (System.Xml.XmlNode element in doc.DocumentElement.ChildNodes)
            {
                if (element.Name != "loggingConfiguration")
                    continue;
                foreach (System.Xml.XmlNode n in element.ChildNodes)
                {
                    if (n.Name == "categorySources" || n.Name == "specialSources")
                    {
                        foreach (System.Xml.XmlNode nn in n.ChildNodes) //add
                        {
                            if (nn.Name == "add" || nn.Name == "errors")
                            {
                                foreach (System.Xml.XmlNode nnn in nn.ChildNodes) //listeners
                                {
                                    if (nnn.Name != "listeners")
                                        continue;
                                    System.Collections.Generic.List<System.Xml.XmlNode> removeNodes = new System.Collections.Generic.List<System.Xml.XmlNode>();
                                    foreach (System.Xml.XmlNode nnnn in nnn.ChildNodes) //add name contains event
                                    {
                                        if (nnnn.Name != "add")
                                            continue;
                                        if (nnnn.Attributes[0].Value.ToString().ToLower().Contains("event"))
                                            removeNodes.Add(nnnn);
                                    }
                                    foreach (System.Xml.XmlNode node in removeNodes)
                                    {
                                        nnn.RemoveChild(node);
                                        removed = true;
                                    }
                                    nn.ToString();
                                }
                            }
                        }
                    }
                }
            }
            if (removed)
                doc.Save(Application.StartupPath + "\\metabuilder.exe.config");
        }

        [STAThread]
        private static void Main(string[] args)
        {

#if DEBUG

            //args = new string[] { "C:\\Users\\Deon\\Desktop\\CDH.mdgm" };

            System.IO.DirectoryInfo appDirInfo = new System.IO.DirectoryInfo(Application.StartupPath);
            Core.FilterVariables.filters = new System.Collections.Generic.List<string>();
            Core.FilterVariables.filters.Add("settings.xml");

            foreach (System.IO.FileInfo file in appDirInfo.GetFiles())
            {
                if (!(file.Extension.Contains("xml")))
                    continue;

                if (!(file.FullName.Contains("settings")))
                    continue;

                if (!(file.FullName.Contains("-")))
                    continue;

                Core.FilterVariables.filters.Add(file.Name);
            }
            if (Core.FilterVariables.filters.Count > 1)
            {
                Filter.SelectFilter f = new MetaBuilder.WinUI.Filter.SelectFilter(Core.FilterVariables.filters);
                f.ShowDialog();
            }
#endif

            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

            // test if this is the first instance and register receiver, if so.
            bool isFirst = false;
            try
            {
                isFirst = SingletonController.IamFirst(new SingletonController.ReceiveDelegate(myReceive));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                isFirst = false;
            }
            if (isFirst)
            {
                try
                {
                    RemoveEventLogTraceFromConfigFile();
                }
                catch (Exception ex)
                {
                    Core.Log.WriteLog(ex.ToString());
                }

                bool DeleteLog = false;
                try
                {
                    if (System.IO.File.Exists(Application.StartupPath + "\\trace.log"))
                    {
                        long maxBytes = 10000000; //10mb
                        //1000bytes is 1kb;
                        //   1000kb is 1mb;
                        //     1000mb is 1gb
                        //10000000bytes is 10 mb
                        //Logger.Writer.Dispose();
                        System.IO.FileInfo fi = new System.IO.FileInfo(Application.StartupPath + "\\trace.log");
                        DeleteLog = fi.Length > maxBytes;
                    }
                }
                catch (Exception ex)
                {
                    Core.Log.WriteLog(ex.ToString());
                }

                DoStartup(args);

                try
                {
                    if (DeleteLog)
                    {
                        string path = Environment.CurrentDirectory + "\\trace.log";

                        System.Diagnostics.ProcessStartInfo start = new System.Diagnostics.ProcessStartInfo();
                        start.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                        start.FileName = "cmd.exe";
                        start.Arguments = string.Format("/c timeout 10 && del \"{0}\"", path);

                        Process.Start(start);
                    }
                }
                catch
                {
                }
            }
            else
            {
                //Thread.Sleep(250);
                // send command line args to running app, then terminate
                SingletonController.Send(args);
            }

            SingletonController.Cleanup();
        }

        private static void DoStartup(string[] args)
        {
            if (DoStartupThingies())
            {
                try
                {
#if DEBUG
                    //RegForm rform = new RegForm();
                    //DialogResult result = rform.ShowDialog();
                    //if (result != DialogResult.OK)
                    //{
                    //    Application.Exit();
                    //    return;
                    //}

                    //MetaBuilder.UIControls.Dialogs.Registration.SpecifyProxy specProxy = new MetaBuilder.UIControls.Dialogs.Registration.SpecifyProxy();
                    //if (specProxy.ShowDialog() == DialogResult.OK)
                    //{
                    //    specProxy.ReturnProxy.ToString();
                    //}

                    //Dictionary.ClassEditor dd = new Dictionary.ClassEditor();
                    //dd.ShowDialog();
                    //return;
#endif

                    try
                    {
                        if (args != null && args.Length > 0)
                        {
                            if (args[0].ToString().ToLower().Contains("dictionary"))
                            {
                                Dictionary.ClassEditor d = new Dictionary.ClassEditor(); //Exception
                                d.ShowDialog();
                            }
                            else if (args[0].ToString().Contains("Or3g")) //override for broken cant read/write file FNB clients
                            {
                                Core.Variables.Instance.IsDesktopEdition = true; //still require the file to be present
                                Core.Variables.Instance.IsServer = true; //still require the file to be present
                            }
                        }
                    }
                    catch
                    {
                        //Exception                                             
                        MessageBox.Show("Missing licence", "You require a developer licence for this startup argument.", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    f = new DockingForm();
                }
                catch (Exception except)
                {
                    string error = except.ToString();
                    LogEntry logentry = new LogEntry();
                    logentry.Message = except.ToString();
                    Logger.Write(logentry);
                    RegForm rform = new RegForm();
                    DialogResult result = rform.ShowDialog();
                    if (result != DialogResult.OK)
                    {
                        Application.Exit();
                        return;
                    }
                    else
                    {
                        //try
                        //{
                        f = new DockingForm();
                        scr.SplashScreen.CloseForm();
                        //}
                        //catch (Exception x)
                        //{
                        //    MessageBox.Show("Exceptional Exception", "Either a suitable licence could not be found or an error has occurred preventing metabuilder from completing startup and subsequently will now close.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        //    Core.Log.WriteLog(x.ToString());
                        //}
                    }
                }

                if (f == null || f.IsDisposed)
                    return;
                //FilesToOpenOnStartup = args;

                //if (f != null)
                //{
                f.FilesToOpenOnStartup = args;
                f.StartExternalProcess += new EventHandler(runner_StartProcessHere);

                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

                //try
                //{
                f.Layout += new LayoutEventHandler(f_Layout);
                //f.BringToFront();
                Application.Run(f);

                //}
                //catch (Exception x)
                //{
                //    UnhandledExceptionEventArgs uexargs = new UnhandledExceptionEventArgs(x, false);
                //    CurrentDomain_UnhandledException(f, uexargs);
                //}
                //}
            }
            else
            {
                Application.Exit();
            }
        }

        private static void ExposeCulture()
        {
            string culture = "Culture " + System.Globalization.CultureInfo.CurrentCulture.ToString() + Environment.NewLine;
            culture += "Override " + System.Globalization.CultureInfo.CurrentCulture.UseUserOverride.ToString() + Environment.NewLine;
            //culture += "Date " + System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + Environment.NewLine;
            culture += "Number " + System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToString();
            Core.Log.WriteLog(culture); ;
        }

        private static bool DoStartupThingies()
        {
            try { ExposeCulture(); }
            catch
            {
                //we dont care if this fails nor why
            }

            StartupTest sTest = new StartupTest();
            try
            {
                scr.SplashScreen.ShowSplashScreen();

                System.Threading.Thread.Sleep(200);
                scr.SplashScreen.SetStatus("Windows Identity");
                sTest.TestPermission();

                System.Threading.Thread.Sleep(100);
                scr.SplashScreen.SetStatus("Reading Settings");
                sTest.TestForSettingsFile();

                scr.SplashScreen.SetStatus("Injecting Connections");
                CoreInjector inject = new CoreInjector();
                inject.InjectConnections();

                System.Threading.Thread.Sleep(100);
                scr.SplashScreen.SetStatus("Validating Registration");
                sTest.ValidateRegistration();

                System.Threading.Thread.Sleep(100);
                scr.SplashScreen.SetStatus("Checking Folders");
                sTest.TestFolders();

                System.Threading.Thread.Sleep(100);
                scr.SplashScreen.SetStatus("Updating Metamodel");
                sTest.TestConnection();

                //DATABASE PATCHING
                if (System.IO.File.Exists(Application.StartupPath + "\\UPDATESERVERDB.UDBF"))
                {
                    if (Core.Variables.Instance.ServerConnectionString.Length > 0)
                    {
                        System.Threading.Thread.Sleep(100);
                        scr.SplashScreen.SetStatus("Updating Server Metamodel");
                        if (sTest.TestServerConnection())
                        {
                            System.Threading.Thread.Sleep(100);
                            scr.SplashScreen.SetStatus("Complete");
                        }
                        else
                        {
                            System.Threading.Thread.Sleep(100);
                            scr.SplashScreen.SetStatus("Server Metamodel update failed");
                        }
                    }
                }

                //System.Threading.Thread.Sleep(500);
                //scr.SplashScreen.SetStatus("Reading Settings");
                //sTest.ReadSettings();

                System.Threading.Thread.Sleep(100);
                scr.SplashScreen.SetStatus("Testing MetaModel");
                sTest.TestMetaModel();

#if DEBUG
                System.Threading.Thread.Sleep(100);
                scr.SplashScreen.SetStatus("Enumerating Domain Definitions");
                sTest.BuildEnums();
#endif

                System.Threading.Thread.Sleep(100);
                scr.SplashScreen.SetStatus("Caching Associations");
                sTest.CacheAssociationHive();

                //System.Threading.Thread.Sleep(500);
                //scr.SplashScreen.SetStatus("Initializing Engine");

                scr.SplashScreen.CloseForm();
                Application.DoEvents();
            }
            catch (Exception xx)
            {
                if (scr.SplashScreen.SplashForm != null)
                    scr.SplashScreen.CloseForm();
                //sTest.DisplayBox("Exception", xx.ToString());

                LogEntry logEntry = new LogEntry();
                logEntry.Title = "Startup error";
                logEntry.Message = xx.ToString();
                Logger.Write(logEntry);
                //return false;
            }
            if (sTest.HasErrors)
            {
                scr.SplashScreen.SetStatus("Startup Tests Failed");
                //System.Threading.Thread.Sleep(500);

                if (scr.SplashScreen.SplashForm != null)
                    scr.SplashScreen.CloseForm();
                sTest.DisplayBox();
                return false;
            }
            else
            {
                scr.SplashScreen.SetStatus("Startup Tests Passed");
                //System.Threading.Thread.Sleep(500);
            }

            if (sTest.RequiresUpdate)
            {
#if DEBUG
                //no need to dl updates when debugging
                return true;
#endif
                scr.SplashScreen.SetStatus("Update Available");
                Core.Log.WriteLog("Update available");
#if DEBUG
                return true;
                //show custom dialog displaying changes between current version and updated version
                //maybe wrap into UpdateDownloader?
#endif
                if (MessageBox.Show("There is an updated version of Metabuilder available. " + Environment.NewLine + "Current Version:" + FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).FileVersion.ToString() + " --> New Version: " + sTest.requiresUpdateVersion.ToLower().Replace(".exe", "").Replace("metabuilder-", "") + Environment.NewLine + "Download and install it now?", "Metabuilder Update Available", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        //download file===>sTest.requiresUpdateVersion
                        UpdateDownloader dl = new UpdateDownloader();
                        dl.DownloadFile("http://www.metabuilder.co.za/Licencing/" + sTest.requiresUpdateVersion, Application.StartupPath + "\\" + sTest.requiresUpdateVersion);
                        dl.ShowDialog();
                        if (dl.DialogResult == DialogResult.OK)
                        {
                            Process.Start(Application.StartupPath + "\\" + sTest.requiresUpdateVersion); //downloaded installation
                            //returning false fails the startuptests and application.exits
                            return false;
                        }
                        else
                        {
                            //dl cancelled
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Core.Log.WriteLog(ex.ToString());
                    }
                    return false;
                }
            }

            return true;
        }

        private static void f_Layout(object sender, LayoutEventArgs e)
        {
            if (m_bLayoutCalled == false)
            {
                m_bLayoutCalled = true;
                //MessageBox.Show("Layed out");
                m_dt = DateTime.Now;
                if (SplashScreen.SplashScreen.SplashForm != null)
                    SplashScreen.SplashScreen.SplashForm.Owner = f;
                f.Activate();
                f.TopMost = false;
                SplashScreen.SplashScreen.CloseForm();
            }
        }

        public static void runner_StartProcessHere(object sender, EventArgs e)
        {
            switch ((string)sender)
            {
                case "Dictionary":
                    ClassEditor classEditor = new ClassEditor();
                    classEditor.Show();
                    break;
                case "Mapper":
                    RelmanContainer relContainer = new RelmanContainer();
                    relContainer.ShowInTaskbar = false;
                    relContainer.ShowIcon = false;
                    relContainer.ShowDialog();
                    break;
                case "ImportExcelTemplate":
                    ExcelImportInterface excelImport = new ExcelImportInterface();
                    excelImport.Import(false);
                    break;
            }

            #region Old Code for OLD EXport & Import

            /*
            Import.VisioImport.ImportInterface VisioImporterGUI;
            switch (s)
            {
               
                case "OIDReports":
                    Reports.OIDReport oidrep = new Reports.OIDReport();
                    oidrep.Bind();
                    oidrep.ShowDialog(this);



                    break;
                case "ADDReports":
                    Reports.ADDReport addrep = new Reports.ADDReport();
                    addrep.Bind();
                    addrep.ShowDialog(this);

                    break;
                case "Export":
                    Export.ExportUI expUI = new Export.ExportUI();
                    expUI.ShowDialog(this);

                    break;
                case "ImportOIDs":
                    VisioImporterGUI = new Import.VisioImport.ImportInterface();
                    VisioImporterGUI.Show();
                    VisioImporterGUI.Import(Import.VisioImport.VisioDrawingType.OID, true);
                    break;
                case "ImportOID":
                    VisioImporterGUI = new Import.VisioImport.ImportInterface();
                    VisioImporterGUI.Show();
                    VisioImporterGUI.Import(Import.VisioImport.VisioDrawingType.OID, false);
                    break;
                case "ImportFSDs":
                    VisioImporterGUI = new Import.VisioImport.ImportInterface();
                    VisioImporterGUI.Show();
                    VisioImporterGUI.Import(Import.VisioImport.VisioDrawingType.FSD, true);
                    break;
                case "ImportFSD":
                    VisioImporterGUI = new Import.VisioImport.ImportInterface();
                    VisioImporterGUI.Show();
                    VisioImporterGUI.Import(Import.VisioImport.VisioDrawingType.FSD, false);
                    break;
                case "ImportADD":
                    VisioImporterGUI = new Import.VisioImport.ImportInterface();
                    VisioImporterGUI.Show();
                    VisioImporterGUI.Import(Import.VisioImport.VisioDrawingType.ADD, false);
                    break;
                case "ImportADDs":
                    VisioImporterGUI = new Import.VisioImport.ImportInterface();
                    VisioImporterGUI.Show();
                    VisioImporterGUI.Import(Import.VisioImport.VisioDrawingType.ADD, false);
                    break;
               
               
                default:
                    //
                    break;
            }*/

            #endregion
        }

        private static void UnhandledException(Exception ex)
        {
            LogEntry logEntry = new LogEntry();
            StringBuilder message = new StringBuilder();
            AddExceptionToLogEntry(ex, message);
            logEntry.Message = message.ToString();
            logEntry.Categories.Add("UnhandledException");
            logEntry.TimeStamp = DateTime.Now;
            logEntry.Title = "UnhandledException";
            logEntry.MachineName = Environment.MachineName;
            logEntry.AppDomainName = AppDomain.CurrentDomain.ToString();
            Logger.Write(logEntry);
            MessageBox.Show("An Exception Has Occurred", "It was logged. Metabuilder will close after you click OK.", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void AddExceptionToLogEntry(Exception ex, StringBuilder message)
        {
            message.AppendLine("Source");
            message.AppendLine(ex.Source);
            message.AppendLine();
            message.AppendLine("StackTrace");
            message.AppendLine(ex.StackTrace);
            message.AppendLine();
            message.AppendLine("Message");
            message.AppendLine(ex.Message);
            message.AppendLine();
            message.AppendLine("TargetSite");
            message.AppendLine(ex.TargetSite.Name);
            message.AppendLine();
            message.AppendLine("Module");
            message.AppendLine(ex.TargetSite.Module.Name);
            if (ex.InnerException != null)
                AddExceptionToLogEntry(ex.InnerException, message);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //AppLoader.UnhandledException(e.ExceptionObject);
            LogEntry entry = new LogEntry();
            entry.AddErrorMessage(e.ExceptionObject.ToString());
            Logger.Write(entry);
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            //AppLoader.UnhandledException(e.Exception);
            //LogEntry entry = new LogEntry();
            //entry.AddErrorMessage(e.Exception.Message);
            //entry.AddErrorMessage(e.Exception.StackTrace);
            //Logger.Write(entry);
            Core.Log.WriteLog(e.Exception.ToString());
        }
    }
}