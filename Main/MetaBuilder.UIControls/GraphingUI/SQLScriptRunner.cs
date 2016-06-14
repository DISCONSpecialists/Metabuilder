using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace MetaBuilder.UIControls.GraphingUI
{
    public class SQLScriptRunner
    {

        #region Methods (1)

        // Public Methods (1) 
        public static void CheckForUpdates()
        {
            DirectoryInfo dirinfo = new DirectoryInfo(Core.Variables.Instance.MetaAssemblyPath);
            FileInfo[] finfos = dirinfo.GetFiles("*.sql");

            foreach (FileInfo finfo in finfos)
            {
                StringBuilder sbuilder = new StringBuilder();
                // Read the file into memory
                TextReader reader = new StreamReader(finfo.FullName);

                while (reader.Peek() != -1)
                {
                    sbuilder.Append(reader.ReadLine()).Append(Environment.NewLine);
                }
                reader.Close();

                // Run the SQL Query
                SqlCommand cmd = new SqlCommand();
                SqlConnection connection = new SqlConnection(Core.Variables.Instance.ConnectionString);
                connection.Open();
                cmd.CommandText = sbuilder.ToString();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                Core.Log.WriteLog("Excecuting SQLScriptRunner update on " + connection.ConnectionString + Environment.NewLine + cmd.CommandText);
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }

            for (int i = 0; i < finfos.Length; i++)
            {
                finfos[i].Delete();
            }
        }

        #endregion Methods

    }
}
