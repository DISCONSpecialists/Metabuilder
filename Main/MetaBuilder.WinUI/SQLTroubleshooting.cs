using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Globalization;

namespace MetaBuilder.WinUI
{

    public class SqlTroubleshooting : Form
    {
        private MetaControls.MetaButton buttonClose;
        private MetaControls.MetaButton buttonInstall;
        private MetaControls.MetaButton buttonConnect;
        private TextBox txtInstance;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtDatabase;
        private Label label4;
        private TextBox txtPassword;
        private Label label5;
        private TextBox txtUsername;
        private CheckBox checkBoxAuth;
        private Panel panelAuth;

        private void InitializeComponent()
        {
            this.buttonClose = new MetaControls.MetaButton();
            this.buttonInstall = new MetaControls.MetaButton();
            this.buttonConnect = new MetaControls.MetaButton();
            this.txtInstance = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.checkBoxAuth = new System.Windows.Forms.CheckBox();
            this.panelAuth = new System.Windows.Forms.Panel();
            this.panelAuth.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(207, 167);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "Close";
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonInstall
            // 
            this.buttonInstall.Location = new System.Drawing.Point(13, 167);
            this.buttonInstall.Name = "buttonInstall";
            this.buttonInstall.Size = new System.Drawing.Size(75, 23);
            this.buttonInstall.TabIndex = 1;
            this.buttonInstall.Text = "Install";
            this.buttonInstall.Visible = false;
            this.buttonInstall.Click += new System.EventHandler(this.buttonInstall_Click);
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(124, 167);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 2;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // txtInstance
            // 
            this.txtInstance.Location = new System.Drawing.Point(92, 34);
            this.txtInstance.Name = "txtInstance";
            this.txtInstance.Size = new System.Drawing.Size(188, 20);
            this.txtInstance.TabIndex = 3;
            this.txtInstance.TextChanged += new System.EventHandler(this.txtChanged);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(292, 32);
            this.label1.TabIndex = 4;
            this.label1.Text = "Please enter the details of a SQL Server and click connect.\r\nIf the connection su" +
                "cceeds startup will continue.";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(4, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Instance";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(4, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Name";
            // 
            // txtDatabase
            // 
            this.txtDatabase.Location = new System.Drawing.Point(92, 60);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(188, 20);
            this.txtDatabase.TabIndex = 6;
            this.txtDatabase.TextChanged += new System.EventHandler(this.txtChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(91, 33);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(188, 20);
            this.txtPassword.TabIndex = 10;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(3, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Username";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(91, 7);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(188, 20);
            this.txtUsername.TabIndex = 8;
            this.txtUsername.Text = "sa";
            this.txtUsername.TextChanged += new System.EventHandler(this.txtChanged);
            // 
            // checkBoxAuth
            // 
            this.checkBoxAuth.AutoSize = true;
            this.checkBoxAuth.Location = new System.Drawing.Point(92, 86);
            this.checkBoxAuth.Name = "checkBoxAuth";
            this.checkBoxAuth.Size = new System.Drawing.Size(163, 17);
            this.checkBoxAuth.TabIndex = 12;
            this.checkBoxAuth.Text = "Use Windows Authentication";
            this.checkBoxAuth.UseVisualStyleBackColor = true;
            this.checkBoxAuth.CheckedChanged += new System.EventHandler(this.checkBoxAuth_CheckedChanged);
            // 
            // panelAuth
            // 
            this.panelAuth.Controls.Add(this.label5);
            this.panelAuth.Controls.Add(this.txtUsername);
            this.panelAuth.Controls.Add(this.label4);
            this.panelAuth.Controls.Add(this.txtPassword);
            this.panelAuth.Location = new System.Drawing.Point(3, 104);
            this.panelAuth.Name = "panelAuth";
            this.panelAuth.Size = new System.Drawing.Size(289, 57);
            this.panelAuth.TabIndex = 13;
            this.panelAuth.Visible = false;
            // 
            // SQLTroubleShooting
            // 
            this.ClientSize = new System.Drawing.Size(292, 196);
            this.Controls.Add(this.panelAuth);
            this.Controls.Add(this.checkBoxAuth);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDatabase);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtInstance);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.buttonInstall);
            this.Controls.Add(this.buttonClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SQLTroubleShooting";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SQL TroubleShooting";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SqlTroubleshooting_FormClosing);
            this.panelAuth.ResumeLayout(false);
            this.panelAuth.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private bool success;
        public bool Success { get { return success; } set { success = value; } }
        private string connectionString;
        public string ConnectionString { get { return connectionString; } set { connectionString = value; } }
        private SqlConnection connection;

        public SqlTroubleshooting()
        {
            InitializeComponent();

            Success = false;
            ConnectionString = Core.Variables.Instance.ConnectionString;
            //Server=.\SQLExpress;Initial Catalog=METABUILDER;Integrated Security=true;
            foreach (string x in connectionString.Split(';'))
            {
                int i = 0;
                foreach (string y in x.Split('='))
                {
                    if (i == 1)
                    {
                        if (x.ToLower(CultureInfo.InvariantCulture).Contains(Core.Variables.Instance.ServerProvider))
                            txtInstance.Text = y;
                        else if (x.ToLower(CultureInfo.InvariantCulture).Contains("catalog") || x.ToLower(CultureInfo.InvariantCulture).Contains("database"))
                            txtDatabase.Text = y;
                        else if (x.ToLower(CultureInfo.InvariantCulture).Contains("integrated security"))
                            checkBoxAuth.Checked = bool.Parse(y);
                        else if (x.ToLower(CultureInfo.InvariantCulture).Contains("user"))
                            txtUsername.Text = y;
                        else if (x.ToLower(CultureInfo.InvariantCulture).Contains("pass"))
                            txtPassword.Text = y;
                    }

                    i += 1;
                }
            }
        }
        private void SqlTroubleshooting_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Success)
            {
                if (MessageBox.Show(this, "Are you sure you would like to proceed?", "A connection has not successfully been established", MessageBoxButtons.YesNo) == DialogResult.No)
                    e.Cancel = true;
            }
        }

        private void installInstance()
        {
            string commands = "/qb username=\"CustomerUsername\" companyname=\"OurCompany\" addlocal=ALL  disablenetworkprotocols=\"0\" instancename=\"" + txtInstance.Text + "\"";
            if (checkBoxAuth.Checked == false)
            {
                //SQL Authentication
                commands += " SECURITYMODE=\"SQL\" SAPWD=\"" + txtPassword.Text + "\"";
            }
            //C:\SQLSERVER2005\SQLEXPR32.EXE + commands
            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = Application.StartupPath + "\\Data\\SQLEEXPR.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = commands;

            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch
            {
                // Log error.
            }

            //create db
            createDatabase();
        }
        private void createDatabase()
        {
            //string dbFilename = Application.StartupPath + "\\Data\\txtDatabase.text + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString()";

            createModel();
        }
        private void createModel()
        {
            Success = true;
            Close();
        }

        private void txtChanged(object sender, EventArgs e)
        {
            Success = false;
        }

        private void buildConnectionString()
        {
            SqlConnectionStringBuilder sqlConnectionBuilder = new SqlConnectionStringBuilder();
            sqlConnectionBuilder.DataSource = txtInstance.Text;
            sqlConnectionBuilder.InitialCatalog = txtDatabase.Text;
            sqlConnectionBuilder.IntegratedSecurity = checkBoxAuth.Checked;
            if (!checkBoxAuth.Checked)
            {
                sqlConnectionBuilder.UserID = txtUsername.Text;
                sqlConnectionBuilder.Password = txtPassword.Text;
            }

            ConnectionString = sqlConnectionBuilder.ConnectionString;
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            buildConnectionString();
            try
            {
                connection = new SqlConnection(ConnectionString);
                //try connect instance
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    //try select from metabuilder [class table]
                    try
                    {
                        SqlCommand cmd = new SqlCommand("select top 1 * from [class] where name ='function'", connection);
                        cmd.ExecuteNonQuery();
                        Success = true;
                        Close();
                    }
                    catch
                    {
                        this.Cursor = Cursors.Default;
                        //MessageBox.Show(this,"A connection was established but it seems the database is missing." + Environment.NewLine + "One will now be created", "Database missing");
                        this.Cursor = Cursors.WaitCursor;
                        //create db
                        connection.Close();
                        createDatabase();
                    }
                }
                else
                {
                    connection = null;
                    Success = false;
                }
            }
            catch
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(this, "A connection was not established ." + Environment.NewLine + "Click install to create a new instance", "Database Connection");
                connection = null;
                Success = false;
            }
            this.Cursor = Cursors.Default;
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();

            if (Success)
            {
                Core.Variables.Instance.ConnectionString = ConnectionString;
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void buttonInstall_Click(object sender, EventArgs e)
        {
            installInstance();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void checkBoxAuth_CheckedChanged(object sender, EventArgs e)
        {
            panelAuth.Visible = !checkBoxAuth.Checked;
        }

    }
}