//=====================================================================
// This file is part of the Desaware Licensing System samples.
//
// Copyright ©2003-2004 Desaware Inc. All rights reserved.
//
//THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//PARTICULAR PURPOSE.
//=====================================================================
// Desaware Licensing System CustomData sample project, RegForm.cs file.
// Copyright ©2003-2004 by Desaware Inc. All Rights Reserved
// www.desaware.com
using System;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Text;
using System.Windows.Forms;
using MetaBuilder.Core;
using MetaBuilder.Sync;
using MetaBuilder.UIControls.Dialogs.Registration;
using MetaBuilder.UIControls.GraphingUI;
using MetaBuilder.UIControls.RegistrationWS;
using OpenLicense;
using OpenLicense.LicenseFile;
using OpenLicense.LicenseFile.Constraints;
using MetaBuilder.SplashScreen;

namespace MetaBuilder.UIControls
{
    /// <summary>
    /// Summary description for RegForm.
    /// </summary>
    public class RegForm : Form
    {

        #region Fields (34)

        internal MetaControls.MetaButton btnDemo;
        internal MetaControls.MetaButton cmdCancel;
        internal MetaControls.MetaButton cmdRegister;
        private IContainer components;
        private ErrorProvider errorProvider1;
        private ErrorProvider errorProvider2;
        private ErrorProvider errorProvider3;
        private ErrorProvider errorProvider4;
        private ErrorProvider errorProvider5;
        private ErrorProvider errorProvider6;
        private Label label1;
        internal Label lblAddress;
        internal Label lblCity;
        internal Label lblCompany;
        internal Label lblCountry;
        internal Label lblEmail;
        internal Label lblFirstName;
        internal Label lblLastName;
        internal Label lbllicensekey;
        internal Label lblPhone;
        internal Label lblState;
        internal Label lblZipCode;
        private bool regSuccess;
        internal TextBox txtAddress;
        internal TextBox txtCity;
        internal TextBox txtCompany;
        internal TextBox txtCountry;
        internal TextBox txtEmail;
        internal TextBox txtFirstName;
        private TextBox txtKey;
        internal TextBox txtLastName;
        internal TextBox txtPhone;
        internal TextBox txtState;
        internal TextBox txtZipCode;

        #endregion Fields

        #region Constructors (1)

        public RegForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods (15)

        // Protected Methods (1) 

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        // Private Methods (14) 

        private bool AttemptRegistration(string typeName, Service svc, string regKey, SerializableDictionary<string, string> customValues, IWebProxy proxy)
        {
            svc.Proxy = proxy;
            try
            {
                string licenseText = svc.GetLicense(typeName, txtFirstName.Text + " " + txtLastName.Text, txtEmail.Text, txtCompany.Text, txtKey.Text, Environment.UserName, Environment.UserDomainName, customValues);

                if (licenseText.Length > 15)
                {
                    RemoveLicenseFile(typeName);

                    try
                    {
                        OpenLicenseFile file = OpenLicenseFile.LoadFromString(licenseText, Type.GetType(typeName));
                        file.SaveFile(Application.StartupPath + "\\" + typeName + ".lic");
                        PleaseWait.SetStatus("Registration Updated...");
                        return true;
                    }
                    catch (Exception ex) //for any other inkale
                    {
                        Log.WriteLog("AttemptRegistration::" + ex.ToString());
                        return false;
                    }
                }
                else //some people can be inkale
                {
                    Log.WriteLog("AttemptRegistration::" + licenseText.ToString());
                    MessageBox.Show(this, "The key you have entered is either already used or no longer valid. Please contact technical support.", "Invalid Key", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (InvalidOperationException ipEX) //for when the server is inkale
            {
                Log.WriteLog("AttemptRegistration::" + ipEX.ToString());
                MessageBox.Show(this, "Please try again later.", "Server cannot be contacted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) //for any other inkale
            {
                Log.WriteLog("AttemptRegistration::" + ex.ToString());
                MessageBox.Show(this, "An error has occurred and has been logged. Please contact technical support.", "Undefined error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return false;
        }

        private bool AttemptTrialRegistration(Service svc, SerializableDictionary<string, string> customValues, IWebProxy proxy)
        {
            //Insert new trial licence type with key of MACADDRESS
            Network.GetMACAddress().ToString();
            return true;
        }

        private void btnDemo_Click(object sender, EventArgs e)
        {
            DoTrialRegister();
        }

        private void btnOfflineRegistration_Click(object sender, EventArgs e)
        {
            bool errs;
            SerializableDictionary<string, string> reginfo = GetRegInfo(false, out errs);
            string xmlFile = System.Environment.SpecialFolder.Desktop + "\\MetaBuilder Registration File.key";

            CreateXMLFile(xmlFile, txtKey.Text, reginfo);

            StringBuilder sb = new StringBuilder();
            sb.Append("The following file has been created on your desktop: " + Environment.NewLine);
            sb.Append(xmlFile + Environment.NewLine);
            sb.Append("Please email this file to MetaBuilder support. We should respond within 3 to 5 working days" + Environment.NewLine);
            sb.Append("You may now continue running this program in trial mode unless your trial has expired.");
            MessageBox.Show(this, sb.ToString(), "File Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnDemo_Click(sender, e);
            cmdCancel_Click(sender, e);
        }

        private WebProxy SpecifyProxyResult;

        private void btnProxy_Click(object sender, EventArgs e)
        {
            SpecifyProxyResult = null;
            SpecifyProxy specProxy = new SpecifyProxy();
            if (specProxy.ShowDialog(this) == DialogResult.OK)
            {
                SpecifyProxyResult = specProxy.ReturnProxy;
                cmdRegister_Click(sender, e);
            }
        }

        private void ClearIsolatedStorage()
        {
            try
            {
                IsolatedStorageFile isoFile = IsolatedStorageFile.GetStore(IsolatedStorageScope.User |
                                                                           IsolatedStorageScope.Domain |
                                                                           IsolatedStorageScope.Assembly, null, null);
                isoFile.Remove();
            }
            catch
            {
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            if (!regSuccess)
            {
                DialogResult = DialogResult.Cancel;
            }
            else
            {
                DialogResult = DialogResult.OK;
            }
            Close();
        }

        //private ClientLicense m_ClientLicense;
        /*public RegForm()
        {
            InitializeComponent();
        }*/
        //** Event handler for both the Register and RegisterDemo buttons
        private void cmdRegister_Click(object sender, EventArgs e)
        {
            try
            {
                DoRegister();
            }
            catch
            {
                try
                {
                    PleaseWait.CloseForm();
                }
                catch
                {
                }
                StringBuilder sbError = new StringBuilder();
                sbError.Append("Registration Failed - cannot establish connection.").Append(Environment.NewLine);
                sbError.Append("Please ensure that you have internet access. ").Append(Environment.NewLine);
                //sbError.Append("If you are behind a proxy, click on the \"Offline Registration\" button.").Append(Environment.NewLine).Append(Environment.NewLine);
                //sbError.Append("If you are behind a proxy, click on the \"Setup Proxy\" button.").Append(Environment.NewLine).Append(Environment.NewLine);
                //sbError.Append("Click OK to continue");
                sbError.Append("If you are behind a proxy, click \"Yes\" to specify a proxy and restart the process or click \"No\" to ammend the information and retry.");
                if (MessageBox.Show(this, sbError.ToString(), "Connection Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    btnProxy_Click(sender, e);
                }
            }
        }

        private void CreateXMLFile(string filename, string keyText, SerializableDictionary<string, string> registrationInfo)
        {

        }

        private void DoRegister()
        {
            bool errs;
            SerializableDictionary<string, string> reginfo = GetRegInfo(false, out errs);
            if (errs)
                return;
            errorProvider1.Clear();
            errorProvider2.Clear();
            errorProvider3.Clear();
            errorProvider4.Clear();
            errorProvider5.Clear();
            errorProvider6.Clear();

            /*
            CPUId cpu = new CPUId();
            MotherboardID mbid = new MotherboardID();
            string cp = cpu.GetProcessorID();
            if (cp.Length > 255)
                reginfo.Add("CPUId", cp.Substring(0, 255));
            else
                reginfo.Add("CPUId", cp);

            string mb = mbid.GetMotherboardID();
            if (mb.Length>255)
                reginfo.Add("MotherboardID", mb.Substring(0,255));
            else
                reginfo.Add("MotherboardID", mb);
            */

            Service svcServer = new Service();
            WebProxy[] proxies = Core.Network.GetProxyList();
            bool ServerRegistered = false;
            bool ClientRegistered = false;
            bool DeveloperEditionRegistered = false;
            bool success = false;
            PleaseWait.ShowPleaseWaitForm();
            PleaseWait.SetStatus("Contacting Server...");

            if (SpecifyProxyResult != null)
            {
                try
                {
                    ClientRegistered = AttemptRegistration(typeof(DockingForm).FullName, svcServer, txtKey.Text, reginfo, SpecifyProxyResult);
                    ServerRegistered = AttemptRegistration(typeof(ObjectManager).FullName, svcServer, txtKey.Text, reginfo, SpecifyProxyResult);
                    DeveloperEditionRegistered = AttemptRegistration(typeof(DevLicensedComponent).FullName, svcServer, txtKey.Text, reginfo, SpecifyProxyResult);

                    success = ServerRegistered || ClientRegistered || DeveloperEditionRegistered;
                }
                catch
                {

                }
            }
            else
            {
                try //Register using default proxy and credentials
                {
                    IWebProxy pro = WebRequest.DefaultWebProxy;
                    pro.Credentials = CredentialCache.DefaultCredentials;

                    ClientRegistered = AttemptRegistration(typeof(DockingForm).FullName, svcServer, txtKey.Text, reginfo, pro);
                    ServerRegistered = AttemptRegistration(typeof(ObjectManager).FullName, svcServer, txtKey.Text, reginfo, pro);
                    DeveloperEditionRegistered = AttemptRegistration(typeof(DevLicensedComponent).FullName, svcServer, txtKey.Text, reginfo, pro);

                    success = ServerRegistered || ClientRegistered || DeveloperEditionRegistered;
                }
                catch
                {
                    foreach (WebProxy proxy in proxies)
                    {
                        try //Register for each proxy that you find (none of these have credentials so this can NEVER work if authentication is required)
                        {
                            ClientRegistered = AttemptRegistration(typeof(DockingForm).FullName, svcServer, txtKey.Text, reginfo, proxy);
                            ServerRegistered = AttemptRegistration(typeof(ObjectManager).FullName, svcServer, txtKey.Text, reginfo, proxy);
                            DeveloperEditionRegistered = AttemptRegistration(typeof(DevLicensedComponent).FullName, svcServer, txtKey.Text, reginfo, proxy);

                            success = ServerRegistered || ClientRegistered || DeveloperEditionRegistered;
                        }
                        catch
                        {
                        }
                    }
                }
            }

            if (!success) //Cause an error here to throw exception in cmdRegister_Click if behind proxy and not connected to start specify proxy and restart this process when specify proxy succeeds
            {
                ClientRegistered = AttemptRegistration(typeof(DockingForm).FullName, svcServer, txtKey.Text, reginfo, null);
                ServerRegistered = AttemptRegistration(typeof(ObjectManager).FullName, svcServer, txtKey.Text, reginfo, null);
                DeveloperEditionRegistered = AttemptRegistration(typeof(DevLicensedComponent).FullName, svcServer, txtKey.Text, reginfo, null);
            }

            StringBuilder sbReg = new StringBuilder();
            if (ClientRegistered)
                sbReg.Append("Desktop Client" + Environment.NewLine);
            if (ServerRegistered)
                sbReg.Append("Server Edition" + Environment.NewLine);
            if (DeveloperEditionRegistered)
                sbReg.Append("Developer Edition" + Environment.NewLine);

            if (DeveloperEditionRegistered || ServerRegistered || ClientRegistered)
            {
                PleaseWait.SetStatus("Thank you for your registration. The following component(s) have been registered:" + Environment.NewLine + sbReg.ToString());
                PleaseWait.PleaseWaitForm.RequireClickToClose = true;
                PleaseWait.PleaseWaitForm.Hide();
                DialogResult = PleaseWait.PleaseWaitForm.ShowDialog(this);
                Variables.Instance.IsServer = ServerRegistered;
                Variables.Instance.IsDeveloperEdition = DeveloperEditionRegistered;
                ClearIsolatedStorage();
                regSuccess = true;
                PleaseWait.CloseForm();
                this.DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                PleaseWait.SetStatus("Registration Failed");
                PleaseWait.PleaseWaitForm.RequireClickToClose = true;
            }
        }

        private void DoTrialRegister()
        {
            bool errs;
            SerializableDictionary<string, string> reginfo = GetRegInfo(true, out errs);
            if (errs)
                return;
            errorProvider1.Clear();
            errorProvider2.Clear();
            errorProvider3.Clear();
            errorProvider4.Clear();
            errorProvider5.Clear();
            errorProvider6.Clear();

            Service svcServer = new Service();
            WebProxy[] proxies = Core.Network.GetProxyList();
            bool success = false;
            PleaseWait.ShowPleaseWaitForm();
            PleaseWait.SetStatus("Contacting Server...");
            foreach (WebProxy proxy in proxies)
            {
                try
                {
                    success = AttemptTrialRegistration(svcServer, reginfo, proxy);
                }
                catch
                {

                }
            }


            if (success)
            {
                OpenLicenseFile lfile = new OpenLicenseFile(typeof(DockingForm), "");
                DemoConstraint demoConstraint = new DemoConstraint();
                demoConstraint.Duration = 14;
                demoConstraint.StartDate = DateTime.Now;
                demoConstraint.EndDate = DateTime.Now.AddDays(14);
                lfile.Constraints.Add(demoConstraint);
                lfile.SaveFile(Application.StartupPath + "\\" + typeof(DockingForm).FullName + ".lic");
                regSuccess = true;

                PleaseWait.SetStatus("Thank you for your registration. The following component(s) have been registered:" + Environment.NewLine + "Metabuilder " + demoConstraint.Duration.ToString() + "Day Trial");
                PleaseWait.PleaseWaitForm.RequireClickToClose = true;
                PleaseWait.PleaseWaitForm.Hide();
                DialogResult = PleaseWait.PleaseWaitForm.ShowDialog(this);
                Variables.Instance.IsServer = false;
                Variables.Instance.IsDeveloperEdition = false;
                ClearIsolatedStorage();
                regSuccess = true;
                PleaseWait.CloseForm();
                this.DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                PleaseWait.SetStatus("Registration Failed");
                PleaseWait.PleaseWaitForm.RequireClickToClose = true;
            }

            //this.DialogResult = DialogResult.OK;
            //Close();
        }

        private SerializableDictionary<string, string> GetRegInfo(bool istrial, out bool errs)
        {
            SerializableDictionary<string, string> reginfo = new SerializableDictionary<string, string>();
            //string errmsg = "";
            errs = false;
            if (txtLastName.Text.Length > 0)
                reginfo.Add("LastName", txtLastName.Text);
            else
            {
                errorProvider1.SetError(txtLastName, "Required Field");
                errs = true;
            }
            if (txtFirstName.Text.Length > 0)
                reginfo.Add("FirstName", txtFirstName.Text);
            else
            {
                errorProvider2.SetError(txtFirstName, "Required Field");
                errs = true;
            }
            if (txtCompany.Text.Length > 0)
                reginfo.Add("Company", txtCompany.Text);

            if (txtAddress.Text.Length > 0)
                reginfo.Add("Address", txtAddress.Text);

            if (txtCity.Text.Length > 0)
                reginfo.Add("City", txtCity.Text);
            if (txtState.Text.Length > 0)
                reginfo.Add("State", txtState.Text);
            if (txtZipCode.Text.Length > 0)
                reginfo.Add("Zip", txtZipCode.Text);
            if (txtCountry.Text.Length > 0)
                reginfo.Add("Country", txtCountry.Text);
            if (txtPhone.Text.Length > 0)
                reginfo.Add("Phone", txtPhone.Text);
            if (txtEmail.Text.Length > 0)
                reginfo.Add("Email", txtEmail.Text);
            else
            {
                errorProvider5.SetError(txtEmail, "Required Field");
                errs = true;
            }

            if (!istrial)
            {
                if (txtKey.Text.Length == 0)
                {
                    errorProvider6.SetError(txtKey, "Required Field");
                    errs = true;
                }
            }

            try
            {
                if (Environment.UserDomainName.Length > 255)
                    reginfo.Add("UserDomainName", Environment.UserDomainName.Substring(0, 100));
                else
                    reginfo.Add("UserDomainName", Environment.UserDomainName);
            }
            catch
            {
            }
            if (Environment.UserName.Length > 255)
                reginfo.Add("Username", Environment.UserName.Substring(0, 255));
            else
                reginfo.Add("Username", Environment.UserName);
            if (Environment.MachineName.Length > 255)
                reginfo.Add("MachineName", Environment.MachineName.Substring(0, 255));
            else
                reginfo.Add("MachineName", Environment.MachineName);
            if (Environment.OSVersion.ToString().Length > 255)
                reginfo.Add("OSVersion", Environment.OSVersion.ToString().Substring(0, 255));
            else
                reginfo.Add("OSVersion", Environment.OSVersion.ToString());
            return reginfo;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lblEmail_Click(object sender, EventArgs e)
        {

        }

        private void RegForm_Load(object sender, EventArgs e)
        {
            btnDemo.Visible = (!(File.Exists(Application.StartupPath + "\\" + typeof(DockingForm).FullName + ".lic")));
            cmdRegister.Text = "Register Application";
            cmdRegister.Enabled = true;
        }

        private void RemoveLicenseFile(string typeName)
        {
            // delete any license files
            try
            {
                File.Delete(Application.StartupPath + "\\" + typeName + ".lic");
            }
            catch
            {
            }
        }

        #endregion Methods

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lbllicensekey = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.lblPhone = new System.Windows.Forms.Label();
            this.txtCountry = new System.Windows.Forms.TextBox();
            this.lblCountry = new System.Windows.Forms.Label();
            this.txtCompany = new System.Windows.Forms.TextBox();
            this.lblCompany = new System.Windows.Forms.Label();
            this.txtZipCode = new System.Windows.Forms.TextBox();
            this.lblZipCode = new System.Windows.Forms.Label();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.lblLastName = new System.Windows.Forms.Label();
            this.txtState = new System.Windows.Forms.TextBox();
            this.lblState = new System.Windows.Forms.Label();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.lblCity = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.cmdCancel = new MetaControls.MetaButton();
            this.cmdRegister = new MetaControls.MetaButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.btnDemo = new MetaControls.MetaButton();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProvider2 = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProvider3 = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProvider4 = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProvider5 = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProvider6 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider6)).BeginInit();
            this.SuspendLayout();
            // 
            // lbllicensekey
            // 
            this.lbllicensekey.Location = new System.Drawing.Point(8, 288);
            this.lbllicensekey.Name = "lbllicensekey";
            this.lbllicensekey.Size = new System.Drawing.Size(340, 16);
            this.lbllicensekey.TabIndex = 41;
            this.lbllicensekey.Text = "Enter the license key:";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(132, 256);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(205, 20);
            this.txtEmail.TabIndex = 11;
            // 
            // lblEmail
            // 
            this.lblEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.Location = new System.Drawing.Point(8, 260);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(120, 16);
            this.lblEmail.TabIndex = 39;
            this.lblEmail.Text = "* Email";
            this.lblEmail.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblEmail.Click += new System.EventHandler(this.lblEmail_Click);
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(132, 228);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(205, 20);
            this.txtPhone.TabIndex = 10;
            // 
            // lblPhone
            // 
            this.lblPhone.Location = new System.Drawing.Point(8, 232);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(120, 16);
            this.lblPhone.TabIndex = 37;
            this.lblPhone.Text = "Phone";
            this.lblPhone.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtCountry
            // 
            this.txtCountry.Location = new System.Drawing.Point(132, 200);
            this.txtCountry.Name = "txtCountry";
            this.txtCountry.Size = new System.Drawing.Size(205, 20);
            this.txtCountry.TabIndex = 8;
            // 
            // lblCountry
            // 
            this.lblCountry.Location = new System.Drawing.Point(8, 204);
            this.lblCountry.Name = "lblCountry";
            this.lblCountry.Size = new System.Drawing.Size(120, 16);
            this.lblCountry.TabIndex = 35;
            this.lblCountry.Text = "Country";
            this.lblCountry.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtCompany
            // 
            this.txtCompany.Location = new System.Drawing.Point(132, 60);
            this.txtCompany.Name = "txtCompany";
            this.txtCompany.Size = new System.Drawing.Size(205, 20);
            this.txtCompany.TabIndex = 3;
            // 
            // lblCompany
            // 
            this.lblCompany.Location = new System.Drawing.Point(29, 64);
            this.lblCompany.Name = "lblCompany";
            this.lblCompany.Size = new System.Drawing.Size(99, 16);
            this.lblCompany.TabIndex = 25;
            this.lblCompany.Text = "Company";
            this.lblCompany.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtZipCode
            // 
            this.txtZipCode.Location = new System.Drawing.Point(132, 172);
            this.txtZipCode.Name = "txtZipCode";
            this.txtZipCode.Size = new System.Drawing.Size(205, 20);
            this.txtZipCode.TabIndex = 7;
            // 
            // lblZipCode
            // 
            this.lblZipCode.Location = new System.Drawing.Point(8, 176);
            this.lblZipCode.Name = "lblZipCode";
            this.lblZipCode.Size = new System.Drawing.Size(120, 16);
            this.lblZipCode.TabIndex = 33;
            this.lblZipCode.Text = "Zip Code";
            this.lblZipCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(132, 32);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(205, 20);
            this.txtLastName.TabIndex = 2;
            // 
            // lblLastName
            // 
            this.lblLastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastName.Location = new System.Drawing.Point(6, 35);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(120, 16);
            this.lblLastName.TabIndex = 21;
            this.lblLastName.Text = "* Last Name";
            this.lblLastName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtState
            // 
            this.txtState.Location = new System.Drawing.Point(132, 144);
            this.txtState.Name = "txtState";
            this.txtState.Size = new System.Drawing.Size(205, 20);
            this.txtState.TabIndex = 6;
            // 
            // lblState
            // 
            this.lblState.Location = new System.Drawing.Point(8, 148);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(120, 16);
            this.lblState.TabIndex = 31;
            this.lblState.Text = "State";
            this.lblState.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtCity
            // 
            this.txtCity.Location = new System.Drawing.Point(132, 116);
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(205, 20);
            this.txtCity.TabIndex = 5;
            // 
            // lblCity
            // 
            this.lblCity.Location = new System.Drawing.Point(8, 120);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(120, 16);
            this.lblCity.TabIndex = 29;
            this.lblCity.Text = "City";
            this.lblCity.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(132, 88);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(205, 20);
            this.txtAddress.TabIndex = 4;
            // 
            // lblAddress
            // 
            this.lblAddress.Location = new System.Drawing.Point(8, 92);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(120, 16);
            this.lblAddress.TabIndex = 27;
            this.lblAddress.Text = "Address";
            this.lblAddress.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(132, 6);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(205, 20);
            this.txtFirstName.TabIndex = 1;
            // 
            // lblFirstName
            // 
            this.lblFirstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFirstName.Location = new System.Drawing.Point(6, 9);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(120, 16);
            this.lblFirstName.TabIndex = 23;
            this.lblFirstName.Text = "* First Name";
            this.lblFirstName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cmdCancel
            // 
            this.cmdCancel.Location = new System.Drawing.Point(266, 332);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(71, 23);
            this.cmdCancel.TabIndex = 15;
            this.cmdCancel.Text = "Close";
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdRegister
            // 
            this.cmdRegister.Location = new System.Drawing.Point(12, 332);
            this.cmdRegister.Name = "cmdRegister";
            this.cmdRegister.Size = new System.Drawing.Size(116, 23);
            this.cmdRegister.TabIndex = 13;
            this.cmdRegister.Text = "Register";
            this.cmdRegister.Click += new System.EventHandler(this.cmdRegister_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Brown;
            this.label1.Location = new System.Drawing.Point(6, 360);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 13);
            this.label1.TabIndex = 46;
            this.label1.Text = "* Internet Connection Required";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // txtKey
            // 
            this.txtKey.Location = new System.Drawing.Point(12, 307);
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(325, 20);
            this.txtKey.TabIndex = 12;
            // 
            // btnDemo
            // 
            this.btnDemo.Location = new System.Drawing.Point(132, 332);
            this.btnDemo.Name = "btnDemo";
            this.btnDemo.Size = new System.Drawing.Size(128, 23);
            this.btnDemo.TabIndex = 14;
            this.btnDemo.Text = "Run in Trial Mode";
            this.btnDemo.Click += new System.EventHandler(this.btnDemo_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // errorProvider2
            // 
            this.errorProvider2.ContainerControl = this;
            // 
            // errorProvider3
            // 
            this.errorProvider3.ContainerControl = this;
            // 
            // errorProvider4
            // 
            this.errorProvider4.ContainerControl = this;
            // 
            // errorProvider5
            // 
            this.errorProvider5.ContainerControl = this;
            // 
            // errorProvider6
            // 
            this.errorProvider6.ContainerControl = this;
            // 
            // RegForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(351, 376);
            this.Controls.Add(this.txtKey);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.btnDemo);
            this.Controls.Add(this.cmdRegister);
            this.Controls.Add(this.lbllicensekey);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.lblPhone);
            this.Controls.Add(this.txtCountry);
            this.Controls.Add(this.lblCountry);
            this.Controls.Add(this.txtCompany);
            this.Controls.Add(this.lblCompany);
            this.Controls.Add(this.txtZipCode);
            this.Controls.Add(this.lblZipCode);
            this.Controls.Add(this.txtLastName);
            this.Controls.Add(this.lblLastName);
            this.Controls.Add(this.txtState);
            this.Controls.Add(this.lblState);
            this.Controls.Add(this.txtCity);
            this.Controls.Add(this.lblCity);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.lblAddress);
            this.Controls.Add(this.txtFirstName);
            this.Controls.Add(this.lblFirstName);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RegForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Registration Form";
            this.Load += new System.EventHandler(this.RegForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider6)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
    }
}