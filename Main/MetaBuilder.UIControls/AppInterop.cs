using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MetaBuilder.UIControls
{
    public class AppInterop
    {

		#region Fields (1) 

        public static char SplitChar = '~';

		#endregion Fields 

		#region Methods (4) 


		// Public Methods (2) 

        /// <summary>
        /// Checks for a previous instance of this app and forwards the
        /// command line to this instance if found.
        /// </summary>
        /// <returns>
        /// True if a previous instance was found.
        /// </returns>
        public static bool CheckPrevInstance()
        {
            IntPtr hWnd = GetHWndOfPrevInstance(Process.GetCurrentProcess().ProcessName);
          
            if (hWnd != IntPtr.Zero)
            {
                string[] args = Environment.GetCommandLineArgs();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < args.Length; i++)
                {
                    sb.Append(args[i]).Append(SplitChar.ToString());
                }

                SendWMData(hWnd, sb.ToString());
                System.Threading.Thread.Sleep(3000);
                return true;
            }

            return false;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, int wParam,
        IntPtr lParam);



		// Private Methods (2) 

        /// <summary>
        /// Searches for a previous instance of this app.
        /// </summary>
        /// <returns>
        /// hWnd of the main window of the previous instance
        /// or IntPtr.Zero if not found.
        /// </returns>
        private static IntPtr GetHWndOfPrevInstance(string ProcessName)
        {
            //get the current process
            Process CurrentProcess = Process.GetCurrentProcess();
            //get a collection of the currently active processes with the same name
            Process[] Ps = Process.GetProcessesByName(ProcessName);
            //if only one exists then there is no previous instance
            if (Ps.Length > 1)
            {
                foreach (Process P in Ps)
                {
                    if (P.Id != CurrentProcess.Id)//ignore this process
                    {
                        //weed out apps that have the same exe name but are started from a different filename. 
                        if (P.ProcessName == ProcessName)
                        {
                            IntPtr hWnd = IntPtr.Zero;
                            try
                            {
                                //if process does not have a MainWindowHandle then an exception will be thrown
                                //so catch and ignore the error.
                                hWnd = P.MainWindowHandle;
                            }
                            catch { }
                            //return if hWnd found.
                            if (hWnd.ToInt32() != 0) return hWnd;
                        }
                    }
                }
            }
            return IntPtr.Zero;
        }

        private static void SendWMData(IntPtr hwnd, string startupArgs)
        {
            COPYDATA cd = new COPYDATA();
            cd.dwData = 0;
            cd.cbData = (uint)(startupArgs.Length * 2);
            cd.lpData = Marshal.StringToHGlobalUni(startupArgs);
            IntPtr lpPtr = Marshal.AllocHGlobal(Marshal.SizeOf(cd));

            Marshal.StructureToPtr(cd, lpPtr, true);
            SendMessage(hwnd, 0x004A, 0, lpPtr);
            COPYDATA cd2 = (COPYDATA)Marshal.PtrToStructure(lpPtr, typeof(COPYDATA));
            string strCheck = Marshal.PtrToStringUni(cd2.lpData);
            Marshal.FreeHGlobal(lpPtr);
        }


		#endregion Methods 
        [StructLayout(LayoutKind.Sequential)]
        public struct COPYDATA
        {
            public uint dwData;
            public uint cbData;
            public IntPtr lpData;
        }

    }


}
