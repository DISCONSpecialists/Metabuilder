using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MetaBuilder.UIControls.GraphingUI;
using MetaBuilder.Core;

namespace MetaBuilder.Viewer
{
    static class Program
    {
        private static bool mb_layout = false;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                bool isFirst = false;
                try
                {
                    isFirst = SingletonController.IamFirst(new SingletonController.ReceiveDelegate(myReceive));
                }
                catch
                {
                    isFirst = false;
                }
                if (isFirst)
                {
                    //load settings
                    MetaSettings s = new MetaSettings();
                    myDockForm = new DockingForm("Viewer");
                    myDockForm.FilesToOpenOnStartup = args;
                    myDockForm.Layout += new LayoutEventHandler(form_Layout);
                    Application.Run(myDockForm);
                }
                else
                {
                    // send command line args to running app, then terminate
                    //MessageBox.Show("Send " + args.ToString());
                    SingletonController.Send(args);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Metabuilder Viewer Load", ex.ToString() + Environment.NewLine + ex.StackTrace, System.Diagnostics.EventLogEntryType.Error);
            }
        }
        static void form_Layout(object sender, LayoutEventArgs e)
        {
            if (mb_layout == false)
            {
                mb_layout = true;
            }
        }
        static DockingForm myDockForm;
        private static void myReceive(string[] args)
        {
            while (!mb_layout)
            {

            }
            //if (form != null)
            //    form.OpenMultipleFiles(args);
            for (int i = 0; i < args.Length; i++)
            {
                try
                {
                    //MessageBox.Show("Recieve " + args[i].ToString());
                    //DockingForm.DockForm
                    myDockForm.Invoke(myDockForm.delegateOpenFile, new object[] { args[i], false });
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.ToString());
                    //System.Diagnostics.EventLog.WriteEntry("Metabuilder Viewer Recieve", x.ToString() + Environment.NewLine + x.StackTrace, System.Diagnostics.EventLogEntryType.Error);
                }
            }
        }
    }
}