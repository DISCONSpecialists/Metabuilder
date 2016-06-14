using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;

namespace MetaBuilder.Core.Networking
{
    public static class Pinger
    {
        public static bool Ping(string connectionString)
        {
            //server=.;Integrated Security=true;database=MetabuilderServer;Connection Reset=FALSE;Pooling=false;Connect Timeout=45;
            string server = "";
            foreach (string s in connectionString.Split(';'))
            {
                if (s.ToLower().Contains("server=") || s.ToLower().Contains("data source="))
                {
                    server = s.Substring(s.IndexOf("=") + 1);
                }
            }
            Log.WriteLog("Ping:Raw Server : " + server);
            if (server.Length == 0)
                return false;
            else
            {
                if (server == "." || server.Contains(".\\")) //local
                {
                    server = Environment.MachineName;
                }
                else if (server.Contains("\\")) //remote
                {
                    server = server.Substring(0, server.IndexOf("\\"));
                }
            }

            bool result = false;
            Log.WriteLog("Pinging Server " + server);

            Ping p = new Ping();
            try
            {
                //System.Windows.Forms.MessageBox.Show(this,"Pinging Server " + server, "PING");
                PingReply reply = p.Send(server, 2000);
                if (reply.Status == IPStatus.Success)
                    return true;
            }
            catch (Exception ex)
            {
                Log.WriteLog("Pinging Server " + server + " Failed " + ex.ToString());
            }
            return result;
        }

        public static bool PingAddress(string address)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(address);
                request.Proxy = WebRequest.DefaultWebProxy;
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                request.Timeout = 5000;
                request.AllowAutoRedirect = false; // find out if this site is up and don't follow a redirector
                request.Method = "HEAD";
                //#if DEBUG
                //                return false;
                //#endif
                WebResponse response = request.GetResponse();
                return true;
            }
            catch (WebException webEx)
            {
                //Proxy server requires authentication
                Log.WriteLog(webEx.ToString());
            }
            catch (Exception Ex)
            {
                Log.WriteLog(Ex.ToString());
            }
            Log.WriteLog("External unsuccesful");
            return false;
        }
        public static bool PingIP(string IP)
        {
            return true;
        }
    }
}
