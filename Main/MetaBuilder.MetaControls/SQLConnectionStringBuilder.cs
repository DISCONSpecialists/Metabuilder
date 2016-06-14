using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;

namespace MetaBuilder.MetaControls
{
    public partial class SQLConnectionStringBuilder : Form
    {
        //Dictionary<string, string> connections = new Dictionary<string, string>();

        public SQLConnectionStringBuilder()
        {
            InitializeComponent();

            try
            {
                System.Data.DataTable dt = System.Data.Sql.SqlDataSourceEnumerator.Instance.GetDataSources();
                //[0]	{ServerName}	object {System.Data.DataColumn}
                //[1]	{InstanceName}	object {System.Data.DataColumn}
                //[2]	{IsClustered}	object {System.Data.DataColumn}
                //[3]	{Version}	object {System.Data.DataColumn}

                foreach (System.Data.DataRow row in dt.Rows)
                {
                    textBoxServer.Items.Add(row[0].ToString() + "\\" + row[1].ToString());
                    //if (row[0].ToString() == Environment.MachineName) //local only
                    //{
                    //check can connect

                    //search databases (SELECT * FROM sys.databases)

                    //}
                }
                //dt.ToString();
            }
            catch
            {

            }
        }

        private void textBoxServer_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            SQLConnectionString.PersistSecurityInfo = false;
            SQLConnectionString.MultipleActiveResultSets = true;

            SQLConnectionString.DataSource = textBoxServer.Text;
            SQLConnectionString.InitialCatalog = textBoxDatabase.Text;
            SQLConnectionString.IntegratedSecurity = !checkBox2.Checked;
            if (SQLConnectionString.IntegratedSecurity)
            {
                SQLConnectionString.UserID = textBoxUsername.Text;
                SQLConnectionString.Password = textBoxPassword.Text;
            }

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = SQLConnectionString.ConnectionString;
            try
            {
                conn.Open();
                textBoxDatabase.Items.Clear();
                SqlCommand cmd = new SqlCommand("SELECT Name FROM sys.databases WHERE database_id > 4", conn);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        textBoxDatabase.Items.Add((string)reader["Name"].ToString());
                    }
                }

                conn.Close();
            }
            catch
            {
                //user details invalid
                textBoxServer.Text = "";
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = checkBox2.Checked;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            SQLConnectionString = null;
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            SQLConnectionString.PersistSecurityInfo = false;
            SQLConnectionString.MultipleActiveResultSets = true;

            SQLConnectionString.DataSource = textBoxServer.Text;
            SQLConnectionString.InitialCatalog = textBoxDatabase.Text;
            SQLConnectionString.IntegratedSecurity = !checkBox2.Checked;
            if (SQLConnectionString.IntegratedSecurity)
            {
                SQLConnectionString.UserID = textBoxUsername.Text;
                SQLConnectionString.Password = textBoxPassword.Text;
            }

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = SQLConnectionString.ConnectionString;
            try
            {
                conn.Open();
                conn.Close();
                //SQLConnectionString.ConnectTimeout = 600;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (SqlException sqlex)
            {
                MessageBox.Show(this, sqlex.Message.ToString(), "Unable to connect");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Connection Error");
            }
        }

        private SqlConnectionStringBuilder sqlConnectionString;
        public SqlConnectionStringBuilder SQLConnectionString
        {
            get
            {
                if (sqlConnectionString == null)
                    sqlConnectionString = new SqlConnectionStringBuilder();
                return sqlConnectionString;
            }
            set
            {
                if (sqlConnectionString == null)
                    sqlConnectionString = new SqlConnectionStringBuilder();
                sqlConnectionString = value;
            }
        }

        public void Bind()
        {
            textBoxServer.Text = SQLConnectionString.DataSource;
            checkBox2.Checked = !SQLConnectionString.IntegratedSecurity;
            textBoxUsername.Text = SQLConnectionString.UserID;
            textBoxPassword.Text = SQLConnectionString.Password;
            textBoxDatabase.Text = SQLConnectionString.InitialCatalog;
        }
    }
}