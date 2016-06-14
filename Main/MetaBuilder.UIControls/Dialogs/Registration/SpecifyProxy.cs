using System;
using System.Net;
using System.Windows.Forms;

namespace MetaBuilder.UIControls.Dialogs.Registration
{
    public partial class SpecifyProxy : Form
    {

        #region Fields (4)

        private string password;
        private string proxyPort;
        private string proxyURL;
        private string username;

        #endregion Fields

        #region Constructors (1)

        public SpecifyProxy()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties (4)

        public string Password
        {
            get { return password; }
        }

        public string ProxyPort
        {
            get { return proxyPort; }
        }

        public string ProxyURL
        {
            get { return proxyURL; }
        }

        public string Username
        {
            get { return username; }
        }

        #endregion Properties

        #region Methods (6)


        // Private Methods (6) 

        private void btnOK_Click(object sender, EventArgs e)
        {
            ValidateFormAndSetFields();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            bool validated = ValidateFormAndSetFields();
            if (validated)
            {
                if (cbUseProxy.Checked)
                {
                    SetProxyInfo();
                }
                HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create("http://www.metabuilder.co.za");
                httpReq.AllowAutoRedirect = false;
                try
                {
                    HttpWebResponse httpRes = (HttpWebResponse)httpReq.GetResponse();

                    switch (httpRes.StatusCode)
                    {
                        case HttpStatusCode.Found:
                            MessageBox.Show(this,"Test Succeeded. Click \"OK\" to continue with registration.", "Specify Proxy", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            DialogResult = DialogResult.OK;
                            Close();
                            break;
                        case HttpStatusCode.OK:
                            MessageBox.Show(this,"Test Succeeded. Click \"OK\" to continue with registration.", "Specify Proxy", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            DialogResult = DialogResult.OK;
                            Close();
                            break;
                        case HttpStatusCode.ProxyAuthenticationRequired:
                            MessageBox.Show(this,"Test Failed - Proxy Authentication Required.", "Specify Proxy", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            ReturnProxy = null;
                            break;
                        case HttpStatusCode.UseProxy:
                            MessageBox.Show(this,"Test Failed - Proxy Server Required.", "Specify Proxy", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            ReturnProxy = null;
                            break;
                    }
                    httpRes.Close();
                }
                catch (WebException webEx)
                {
                    MessageBox.Show(this,webEx.Message.ToString(), "Specify Proxy - Cannot Connect", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    ReturnProxy = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this,"Test Failed - Forbidden.", "Specify Proxy - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ReturnProxy = null;
                }
                // Close the response.
            }
        }

        private void cbSecureProxy_CheckedChanged(object sender, EventArgs e)
        {
            txtConfirm.Enabled = cbSecureProxy.Checked;
            txtPassword.Enabled = cbSecureProxy.Checked;
            txtUsername.Enabled = cbSecureProxy.Checked;
        }

        private void cbUseProxy_CheckedChanged(object sender, EventArgs e)
        {
            grpUseProxy.Enabled = cbUseProxy.Checked;
        }

        public WebProxy ReturnProxy;

        void SetProxyInfo()
        {
            WebProxy p;
            if (cbSecureProxy.Checked)
            {
                p = new WebProxy(ProxyURL + ":" + ProxyPort, true, null, new NetworkCredential(username, password));
            }
            else
            {
                p = new WebProxy(ProxyURL + ":" + ProxyPort);
            }
            ReturnProxy = p;
            GlobalProxySelection.Select = p;
            WebRequest.DefaultWebProxy = p;
        }

        private bool ValidateFormAndSetFields()
        {
            errorProvider1.Clear();
            if (cbUseProxy.Checked)
            {
                if (txtProxyURL.Text.Length == 0)
                {
                    errorProvider1.SetError(txtProxyURL, "Proxy not specified");
                    return false;
                }
                else
                {
                    proxyURL = txtProxyURL.Text;
                }
                if (txtPort.Text.Length == 0)
                {
                    errorProvider1.SetError(txtPort, "Port not specified");
                    return false;
                }

                int port = 0;
                bool parsed = int.TryParse(txtPort.Text, out port);
                if (!parsed)
                {
                    errorProvider1.SetError(txtPort, "Invalid Port");
                    return false;
                }
                else
                {
                    proxyPort = txtPort.Text;
                }
                if (cbSecureProxy.Checked)
                {
                    if (txtUsername.Text.Length == 0)
                    {
                        errorProvider1.SetError(txtUsername, "Please specify a username");
                        return false;
                    }
                    else
                    {
                        username = txtUsername.Text;
                    }

                    if (txtPassword.Text != txtConfirm.Text)
                    {
                        errorProvider1.SetError(txtConfirm, "Passwords do not match");
                        return false;
                    }
                    else
                    {
                        password = txtPassword.Text;
                    }
                }
            }
            return true;
        }

        #endregion Methods

    }
}