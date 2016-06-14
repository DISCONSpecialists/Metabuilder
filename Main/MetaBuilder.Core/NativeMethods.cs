using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MetaBuilder.Core
{
    /// <summary>
    /// Summary description for NativeMethods.
    /// </summary>
    internal sealed class NativeMethods
    {
        private NativeMethods()
        {
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GlobalMemoryStatusEx(ref NativeStructs.MEMORYSTATUSEX lpBuffer);

        [DllImport("shell32.dll")]
        internal static extern void SHAddToRecentDocs(uint uFlags, IntPtr pv);

        [DllImport("kernel32.dll")]
        internal static extern void GetSystemInfo(ref NativeStructs.SYSTEM_INFO lpSystemInfo);

        [DllImport("kernel32.dll")]
        internal static extern void GetNativeSystemInfo(ref NativeStructs.SYSTEM_INFO lpSystemInfo);

        [DllImport("Wintrust.dll", PreserveSig = true)]
        internal extern static unsafe int WinVerifyTrust(
            IntPtr hWnd,
            ref Guid pgActionID,
            ref NativeStructs.WINTRUST_DATA pWinTrustData
            );

        [DllImport("User32.dll")]
        internal extern static unsafe uint SystemParametersInfo(
            uint uiAction,
            uint uiParam,
            void *pvParam,
            uint fWinIni
            );   

        internal static void ThrowOnWin32Error()
        {
            ThrowOnWin32Error(string.Empty);
        }

        internal static void ThrowOnWin32Error(string message)
        {
            int lastWin32Error = Marshal.GetLastWin32Error();
            
            if (lastWin32Error != NativeConstants.ERROR_SUCCESS)
            {
                throw new Win32Exception(lastWin32Error, message + " (" + lastWin32Error.ToString() + ")");
            }
        }
    }
}
