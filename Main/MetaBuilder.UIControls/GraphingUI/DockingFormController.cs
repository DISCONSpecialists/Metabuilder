using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using OpenLicense;
using OpenLicense.LicenseFile;
using OpenLicense.LicenseFile.Constraints;
using MetaBuilder.Core;
using MetaBuilder.Docking;
using System.Drawing;
using System.Drawing.Text;
using System.ComponentModel;
using System.Windows.Forms;
using MetaBuilder.Graphing.Shapes.Nodes;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.UIControls.GraphingUI.ActionMenu;
using System.Threading;
using MetaBuilder.BusinessLogic;
using MetaBuilder.MetaControls;
using System.Collections.ObjectModel;

namespace MetaBuilder.UIControls.GraphingUI
{
    public class DockingFormController
    {
        public MRUManager mruManager;
        //public DelegateOpenFile delegateOpenFile;
        public string[] FilesToOpenOnStartup;
        //private License license = null;
        // used to hold instances of shallow copy for other diagrams
        [NonSerialized]
        private Collection<IShallowCopyable> shallowCopies;
        private DockingForm dockingForm;
        public DockingForm DockingForm
        {
            get { return dockingForm; }
            set { dockingForm = value; }
        }
        private Dictionary<string, Guid> openedFiles;
        public Dictionary<string, Guid> OpenedFiles
        {
            get { return openedFiles; }
            set { openedFiles = value; }
        }
        public string CurrentWorkspace
        {
            get { return DockingForm.toolStripDropDownSelectWorkspace.Text; }
        }
        public bool HasOpenDocuments
        {
            get
            {
                GraphView currentGraphView = GetCurrentGraphView();
                return currentGraphView != null;
            }
        }
        public Collection<IShallowCopyable> ShallowCopies
        {
            get { return shallowCopies; }
            set { shallowCopies = value; }
        }

        public DockingFormController(DockingForm dockingForm)
        {
            DockingForm = dockingForm;
            constructDockingForm();
        }
        private void constructDockingForm()
        {
            if (AppInterop.CheckPrevInstance())
            {
                Application.Exit();
                DockingForm.Close();
            }
            else
            {
                // COMMENTED: WHY? 
                // CloseAllContents();
                // END;
                DockingForm.dockPanel1 = new DockPanel();
                DockingForm.InitializeComponent();
                DockingForm.m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
                InitMRUManager();
                //myDockingForm = this;
                //DockingForm.navigatorWindow = DockingForm.m_panningWindow;
                //delegateOpenFile = new DelegateOpenFile(DockingForm.OpenFileInApplicableWindow);
                ShallowCopies = new Collection<IShallowCopyable>();
                Application.Idle += new EventHandler(Application_Idle);
#if DEBUG
                DockingForm.Text = "MetaBuilder (DEBUG!)";
                DockingForm.BackColor = Color.Khaki;

                //MainMenuStrip.BackColor = Color.Khaki;
#endif
                DockingForm.dockPanel1.DockBackColor = Color.FromKnownColor(KnownColor.AppWorkspace);
                Thread t = new Thread(new ThreadStart(LoadFontsInBackground));
                t.IsBackground = true;
                t.Start();

            }
        }
        private void InitMRUManager()
        {
            mruManager = new MRUManager();
            mruManager.Initialize(
                DockingForm, // owner form
                DockingForm.menuItemFileRecent, // Recent Files menu item
                "Software\\DISCON Specialists\\MetaBuilder"); // Registry path to keep MRU list
        }
        private void Application_Idle(object sender, EventArgs e)
        {

        }

        private void LoadFontsInBackground()
        {
            InstalledFontCollection InstalledFonts = new InstalledFontCollection();
            List<Font> fonts = new List<Font>();
            try
            {
                foreach (FontFamily family in InstalledFonts.Families)
                {
                    try
                    {
                        if (family.IsStyleAvailable(FontStyle.Regular))
                            fonts.Add(new Font(family, 12));
                    }
                    catch
                    {
                        // We end up here if the font could not be created
                        // with the default style.
                    }
                }
            }
            catch
            {
            }
        }
        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(PaletteDocker).ToString())
            {
                DockingForm.m_paletteDocker.Text = "Stencils";
                return DockingForm.m_paletteDocker;
            }
            //else if (persistString == typeof(PropertyGridDocker).ToString())
            //    return DockingForm.m_propertyGridWindow;
            else if (persistString == typeof(MetaPropertyGridDocker).ToString())
                return DockingForm.m_metaPropsWindow;
            else if (persistString == typeof(Tools.Explorer.MetaObjectExplorer).ToString())
                return DockingForm.m_metaObjectExplorer;
            else if (persistString == typeof(TaskDocker).ToString())
                return DockingForm.m_taskDocker;
            else
            {
                string[] parsedStrings = persistString.Split(new char[] { ',' });
                if (parsedStrings.Length != 3)
                    return null;
                if (parsedStrings[0] != typeof(GraphViewContainer).ToString())
                    return null;
                GraphViewContainer gvContainer = new GraphViewContainer(FileTypeList.Diagram);
                if (parsedStrings[1] != string.Empty)
                    gvContainer.Text = parsedStrings[1];
                if (parsedStrings[2] != string.Empty)
                    gvContainer.Text = parsedStrings[1];
                return gvContainer;
            }
        }

        public GraphView GetCurrentGraphView()
        {
            if (DockingForm.ActiveMdiChild != null)
                if (DockingForm.ActiveMdiChild is GraphViewContainer)
                    return (DockingForm.ActiveMdiChild as GraphViewContainer).View;
            return null;
        }

        #region External
        [DllImport("user32", SetLastError = true)]
        public static extern int GetForegroundWindow();
        [DllImport("user32", SetLastError = true)]
        public static extern int GetWindowThreadProcessId(int hwnd, ref int lProcessId);
        public static string GetCurWindowTitle()
        {
            int hwnd = GetForegroundWindow();
            int procid = 0;
            int threadid = GetWindowThreadProcessId(hwnd, ref procid);
            return Process.GetProcessById(procid).MainWindowTitle;
            //return procid;
        }
        #endregion

    }
}