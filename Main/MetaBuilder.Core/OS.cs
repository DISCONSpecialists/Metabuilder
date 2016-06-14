using System;

namespace MetaBuilder.Core
{
    public sealed class OS
    {
        private OS()
        {
        }

        public static Version Windows2000
        {
            get
            {
                return new Version(5, 0);
            }
        }

        public static Version WindowsXP
        {
            get
            {
                return new Version(5, 1);
            }
        }

        public static Version WindowsServer2003
        {
            get
            {
                return new Version(5, 2);
            }
        }

        public static Version WindowsVista
        {
            get
            {
                return new Version(6, 0);
            }
        }

        public static string Revision
        {
            get
            {
                NativeStructs.OSVERSIONINFOEX osviex = new NativeStructs.OSVERSIONINFOEX();
                osviex.dwOSVersionInfoSize = (uint)NativeStructs.OSVERSIONINFOEX.SizeOf;
                bool result = SafeNativeMethods.GetVersionEx(ref osviex);

                if (result)
                {
                    return osviex.szCSDVersion;
                }
                else
                {
                    return "Unknown";
                }
            }
        }

        public static OSType Type
        {
            get
            {
                NativeStructs.OSVERSIONINFOEX osviex = new NativeStructs.OSVERSIONINFOEX();
                osviex.dwOSVersionInfoSize = (uint)NativeStructs.OSVERSIONINFOEX.SizeOf;
                bool result = SafeNativeMethods.GetVersionEx(ref osviex);
                OSType type;

                if (result)
                {
                    type = (OSType)osviex.wProductType;
                }
                else
                {
                    type = OSType.Unknown;
                }

                return type;
            }
        }
    }
}
