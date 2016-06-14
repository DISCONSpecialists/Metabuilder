using System;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using MetaBuilder.ResourceManagement;

namespace MetaBuilder.UIControls.Dialogs
{
    public partial class SplashScreen : Form
    {

		#region Fields (4) 

        private Image bitmap;
        private static SplashScreen MySplashScreen = null;
        private static Thread MySplashThread = null;
        public string VersionText = null;

		#endregion Fields 

		#region Constructors (1) 

        public SplashScreen()
        {
            Version ver = Assembly.GetExecutingAssembly().GetName().Version;
            VersionText = ver.Major.ToString() + "." + ver.Minor.ToString() + " Build " + ver.Build.ToString();

            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;
            this.ShowInTaskbar = false;
            this.SendToBack();
            ////Attempted showontop fix
            //this.TopMost = true;
            //this.Focus();
            //this.BringToFront();
            //this.TopMost = false;
#if DEBUG
			VersionText = VersionText + " (debug)";
#endif
            bitmap = Reader.Logo;
            ClientSize = bitmap.Size;
            using (Font font = new Font("Sans Serif", 12))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.DrawString(VersionText, font, Brushes.White, 320, 180);
                }
            }
            BackgroundImage = bitmap;
        }

		#endregion Constructors 

		#region Methods (3) 


		// Public Methods (2) 

        //	public Method to hide the SplashScreen
        public new static void Close()
        {
            //7 January 2013 - Why is there extra code?
            if (MySplashThread == null) return;
            //if (MySplashScreen == null) return;
            //try
            //{
            //    //MySplashScreen.Close();
            //    //Invoke(new MethodInvoker(SplashScreen.Close));
            //}
            //catch (Exception)
            //{
            //}
            //MySplashThread = null;
            MySplashScreen = null;
        }

        //	public Method to show the SplashScreen
        public new static void Show()
        {
            if (MySplashThread != null)
                return;
            MySplashThread = new Thread(new ThreadStart(ShowThread));
            MySplashThread.IsBackground = true;
            MySplashThread.SetApartmentState(ApartmentState.STA);
            MySplashThread.Start();
        }

		// Private Methods (1) 

        //	internally used as a thread function - showing the form and
        //	starting the messageloop for it
        private static void ShowThread()
        {
            MySplashScreen = new SplashScreen();
            Application.Run(MySplashScreen);
        }

		#endregion Methods 

    }
}