using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;

namespace MetaBuilder.Core
{
    public sealed class Network
    {
        private Network()
        {
        }

        public static string GetMACAddress()
        {
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return sMacAddress;
        }

        internal class ProxyInfo
        {
            private bool autoDetect;
            private string autoConfigUrl;
            private string proxy;
            private string proxyBypass;

            public bool AutoDetect
            {
                get
                {
                    return this.autoDetect;
                }
            }

            public string AutoConfigUrl
            {
                get
                {
                    return this.autoConfigUrl;
                }
            }

            public string Proxy
            {
                get
                {
                    return this.proxy;
                }
            }

            public string ProxyBypass
            {
                get
                {
                    return this.proxyBypass;
                }
            }

            internal ProxyInfo(bool autoDetect, string autoConfigUrl, string proxy, string proxyBypass)
            {
                this.autoDetect = autoDetect;
                this.autoConfigUrl = autoConfigUrl;
                this.proxy = proxy;
                this.proxyBypass = proxyBypass;
            }
        }

        internal static bool GetProxyDetails(out ProxyInfo proxyInfo)
        {
            NativeStructs.WINHTTP_CURRENT_USER_IE_PROXY_CONFIG proxyConfig = new NativeStructs.WINHTTP_CURRENT_USER_IE_PROXY_CONFIG();
            bool result = SafeNativeMethods.WinHttpGetIEProxyConfigForCurrentUser(ref proxyConfig);

            bool autoDetect;
            string autoConfigUrl;
            string proxy;
            string proxyBypass;

            if (result)
            {
                autoDetect = proxyConfig.fAutoDetect;
                autoConfigUrl = Marshal.PtrToStringUni(proxyConfig.lpszAutoConfigUrl);
                proxy = Marshal.PtrToStringUni(proxyConfig.lpszProxy);
                proxyBypass = Marshal.PtrToStringUni(proxyConfig.lpszProxyBypass);

                if (proxyConfig.lpszAutoConfigUrl != IntPtr.Zero)
                {
                    SafeNativeMethods.GlobalFree(proxyConfig.lpszAutoConfigUrl);
                    proxyConfig.lpszAutoConfigUrl = IntPtr.Zero;
                }

                if (proxyConfig.lpszProxy != IntPtr.Zero)
                {
                    SafeNativeMethods.GlobalFree(proxyConfig.lpszProxy);
                    proxyConfig.lpszProxy = IntPtr.Zero;
                }

                if (proxyConfig.lpszProxyBypass != IntPtr.Zero)
                {
                    SafeNativeMethods.GlobalFree(proxyConfig.lpszProxyBypass);
                    proxyConfig.lpszProxyBypass = IntPtr.Zero;
                }
            }
            else
            {
                autoDetect = false;
                autoConfigUrl = null;
                proxy = null;
                proxyBypass = null;
            }

            proxyInfo = new ProxyInfo(autoDetect, autoConfigUrl, proxy, proxyBypass);
            return result;
        }

        /// <summary>
        /// Returns a list of proxies that should be usable to get access to Internet URI's.
        /// </summary>
        /// <returns>An array of WebProxy instances. A null value at any array location indicates to use no proxy.</returns>
        public static WebProxy[] GetProxyList()
        {
            List<WebProxy> proxies = new List<WebProxy>();

            // Get the IE-supplied proxy settings
            ProxyInfo info;
            bool result = GetProxyDetails(out info);

            if (result)
            {
                WebProxy proxy;

                try
                {
                    proxy = new WebProxy(info.Proxy);
                }

                catch
                {
                    proxy = null;
                }

                if (proxy != null)
                {
                    // We will add the proxy twice. First, with the default credentials for the logged-in user. Second, with no credentials.
                    WebProxy proxyWithCreds = new WebProxy(info.Proxy);
                    proxyWithCreds.Credentials = CredentialCache.DefaultCredentials;
                    proxies.Add(proxyWithCreds);
                    proxies.Add(proxy);
                }
            }

            // No proxy is the lowest priority
            proxies.Add(null);

            return proxies.ToArray();
        }
    }
}
