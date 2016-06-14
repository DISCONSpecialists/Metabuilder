using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Sync;
using MetaBuilder.UIControls.GraphingUI;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
using System.IO;

namespace MetaBuilder.UIControls
{
    public class CoreInjector
    {

        #region Fields (1)

        private MetaSettings s;

        #endregion Fields

        #region Constructors (1)

        public CoreInjector()
        {
            s = new MetaSettings();
        }

        #endregion Constructors

        #region Methods (2)


        // Public Methods (2) 

        public void InjectConnections()
        {
            DataRepository.Connections.Clear();
            DataRepository.AddConnection(Core.Variables.Instance.ClientProvider, Variables.Instance.ConnectionString);
            DataRepository.AddConnection(Core.Variables.Instance.ServerProvider, Variables.Instance.ServerConnectionString);
        }

        public void Inject(string userdomainidentifier, bool updateWorkspace)
        {
            Log.WriteLog("V " + FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetEntryAssembly().Location).FileVersion.ToString() + "::Injecting");

            Variables.Instance.UserDomainIdentifier = userdomainidentifier;
            int userid = 0;
            TList<User> users = new TList<User>();
            try
            {
                users = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.UserProvider.GetAll();
            }
            catch (Exception x)
            {
                MessageBox.Show("Cannot establish a database connection. Check your settings and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogEntry logEntry = new LogEntry();
                logEntry.Title = "Database Connection Failed";
                logEntry.Message = x.ToString();
                Logger.Write(logEntry);
                Application.Exit();
                Process.GetCurrentProcess().Kill();
            }
            for (int i = 0; i < users.Count; i++)
            {
                if (userid > 0)
                    break;

                User user = users[i];
                if (Variables.Instance.UserDomainIdentifier.ToLower().Trim() == user.Name.ToLower().Trim())
                {
                    userid = user.pkid;
                }
                // HACK: temp procedure that updates user names on previous databases
                if (user.Name == "Admin")
                {
                    user.Name = Variables.Instance.UserDomainIdentifier;
                    DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.UserProvider.Save(user);
                    userid = user.pkid;
                }
            }

            if (userid == 0)
            {

                // never run the app before, add user to database
                try
                {
                    User user = new User();
                    user.Name = Variables.Instance.UserDomainIdentifier;
                    user.Password = "";
                    user.CreateDate = DateTime.Now;
                    DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.UserProvider.Save(user);

                    userid = user.pkid;
                    Log.WriteLog(user.Name + " - ID: " + userid, "User Added", TraceEventType.Information);
                }
                catch (Exception ex)
                {
                    userid = 0;
                    Log.WriteLog(ex.ToString(), userdomainidentifier + " cannot be found nor added to the User table", TraceEventType.Error);
                    //MessageBox.Show(this,"The user of this computer (" + userdomainidentifier + ") can either not be found in the metabuilder database or is unable to be added to it." + Environment.NewLine + "This error has been logged, please contact your system administrator." + Environment.NewLine + "The application will now exit", "User Identification Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //Application.Exit();
                }

            }

            Log.WriteLog("ID: " + userid.ToString(), "User ID Set In CoreInjector (local) with Identifier:(" + userdomainidentifier + ")", TraceEventType.Information);
            WorkspaceKey key = new WorkspaceKey();
            Hashtable hashWorkspaces = new Hashtable();
            if (updateWorkspace)
            {
                TList<Workspace> workspaces = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.WorkspaceProvider.GetAll();
                foreach (Workspace ws in workspaces)
                {
                    if (ws.Name == "Sandbox")
                    {
                        key.Name = ws.Name;
                        key.WorkspaceTypeId = ws.WorkspaceTypeId;
                    }
                    hashWorkspaces.Add(ws.Name + "#" + ws.WorkspaceTypeId.ToString(), null);
                }
                if (key.Name == null)
                {
//#if !DEBUG
//                    MessageBox.Show("Cannot find Sandbox workspace... Auto-creating", "Sandbox", MessageBoxButtons.OK, MessageBoxIcon.Information);
//#endif
                    Workspace wsAuto = new Workspace();
                    wsAuto.Name = "Sandbox";
                    wsAuto.WorkspaceTypeId = 1;
                    DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.WorkspaceProvider.Save(wsAuto);
                    key.Name = wsAuto.Name;
                    key.WorkspaceTypeId = wsAuto.WorkspaceTypeId;
                }
            }
            Variables.RefreshType rtype = (Variables.RefreshType)Enum.Parse(typeof(Variables.RefreshType), s.GetSetting(MetaSettings.VIEW_COMPAREANDREFRESH_TYPE, "Prompt"), true);

            if (updateWorkspace)
                Variables.Instance = new Variables(userid, key.WorkspaceTypeId, key.Name, hashWorkspaces, rtype);
            else
                Variables.Instance = new Variables(userid, Variables.Instance.CurrentWorkspaceTypeId, Variables.Instance.CurrentWorkspaceName, Variables.Instance.WorkspaceHashtable, rtype);

            Variables.Instance.UserDomainIdentifier = Environment.UserDomainName + "\\" + Environment.UserName;

            try
            {
                ObjectManager oman = new ObjectManager();
                Variables.Instance.IsServer = true;
                //Log.WriteLog("Licencing : Server success");
            }
            catch (Exception x)
            {
                Variables.Instance.IsServer = File.Exists(Application.StartupPath + "\\MetaBuilder.UIControls.GraphingUI.ObjectManager.lic");
                Log.WriteLog(x.ToString());
                //throw (x);
            }
            try
            {
                DevLicensedComponent devComp = new DevLicensedComponent();
                Variables.Instance.IsDeveloperEdition = true;
                //Log.WriteLog("Licencing : Developer success");
            }
            catch (Exception x)
            {
                Log.WriteLog(x.ToString());
                //throw (x);
            }
        }

        /// <summary>
        /// Creates the global variables used throughout the application using dependency injection
        /// </summary>
        public void InjectCoreVariables()
        {
            Inject(Environment.UserDomainName + "\\" + Environment.UserName, true);
        }
        public void InjectCoreVariables(bool updateWorkspace)
        {
            Inject(Environment.UserDomainName + "\\" + Environment.UserName, updateWorkspace);
        }

        #endregion Methods

    }
}