using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace MetaBuilder.Core
{
    /// <summary>
    /// Provides methods and properties related to scanning and printing.
    /// </summary>
    /// <remarks>
    /// Originally adapted from http://www.codeproject.com/dotnet/wiascriptingdotnet.asp
    /// </remarks>
    public sealed class ScanningAndPrinting
    {
        private ScanningAndPrinting()
        {
        }

        private const string wiaProxy32ExeName = "WiaProxy32.exe";

        private static int CallWiaProxy32(string args, bool spinEvents)
        {
            string ourPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            string proxyPath = Path.Combine(ourPath, wiaProxy32ExeName);
            ProcessStartInfo psi = new ProcessStartInfo(proxyPath, args);

            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;

            int exitCode = -1;

            try
            {
                Process process = Process.Start(psi);

                // Can't just use process.WaitForExit() because then the Paint.NET UI
                // will not repaint and it'll look weird because of that.
                while (!process.HasExited)
                {
                    if (spinEvents)
                    {
                        Application.DoEvents();
                    }

                    Thread.Sleep(10);
                }

                exitCode = process.ExitCode;
                process.Dispose();
            }

            catch
            {
            }

            return exitCode;
        }

        /// <summary>
        /// Gets whether or not the scanning and printing features are available without
        /// taking into account whether a scanner or printer are actually connected.
        /// </summary>
        public static bool IsComponentAvailable
        {
            get
            {
                return 1 == CallWiaProxy32("IsComponentAvailable 1", false);
            }
        }

        /// <summary>
        /// Gets whether printing is possible. This does not take into account whether a printer
        /// is actually connected or available, just that it is possible to print (it is possible
        /// that the printing UI has a facility for adding or loading a new printer).
        /// </summary>
        public static bool CanPrint
        {
            get
            {
                return 1 == CallWiaProxy32("CanPrint 1", false);
            }
        }

        /// <summary>
        /// Gets whether scanning is possible. The user must have a scanner connect for this to return true.
        /// </summary>
        /// <remarks>
        /// This also covers image acquisition from, say, a camera.
        /// </remarks>
        public static bool CanScan
        {
            get
            {
                return 1 == CallWiaProxy32("CanScan 1", false);
            }
        }

        /// <summary>
        /// Presents a user interface for printing the given image.
        /// </summary>
        /// <param name="owner">The parent/owner control for the UI that will be presented for printing.</param>
        /// <param name="fileName">The name of a file containing a bitmap (.BMP) to print.</param>
        public static void Print(Control owner, string fileName)
        {
            if (!CanPrint)
            {
                throw new InvalidOperationException("Printing is not available");
            }

            // Disable the entire UI, otherwise it's possible to close the app while the
            // print wizard is active! And then it crashes.
            Form ownedForm = owner.FindForm();
            bool[] ownedFormsEnabled = null;

            if (ownedForm != null)
            {
                ownedFormsEnabled = new bool[ownedForm.OwnedForms.Length];

                for (int i = 0; i < ownedForm.OwnedForms.Length; ++i)
                {
                    ownedFormsEnabled[i] = ownedForm.OwnedForms[i].Enabled;
                    ownedForm.OwnedForms[i].Enabled = false;
                }

                owner.FindForm().Enabled = false;
            } 
            
            CallWiaProxy32("Print \"" + fileName + "\"", true);

            if (ownedForm != null)
            {
                for (int i = 0; i < ownedForm.OwnedForms.Length; ++i)
                {
                    ownedForm.OwnedForms[i].Enabled = ownedFormsEnabled[i];
                }

                owner.FindForm().Enabled = true;
            }

            owner.FindForm().Activate();
        }

        /// <summary>
        /// Presents a user interface for scanning.
        /// </summary>
        /// <param name="fileName">
        /// The filename of where to stored the scanned/acquired image. Only valid if the return value is ScanResult.Success.
        /// </param>
        /// <returns>The result of the scanning operation.</returns>
    }
}
