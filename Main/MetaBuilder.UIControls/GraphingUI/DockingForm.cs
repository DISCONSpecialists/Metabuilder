using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using MetaBuilder.BusinessFacade.Exports;
using MetaBuilder.BusinessFacade.Imports;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.Core.Storage;
using MetaBuilder.Docking;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Nodes;
using MetaBuilder.Meta;
using MetaBuilder.UIControls.Common;
using MetaBuilder.UIControls.Dialogs;
using MetaBuilder.UIControls.Dialogs.DatabaseManagement;
using MetaBuilder.UIControls.Dialogs.RelationshipManager;
using MetaBuilder.UIControls.GraphingUI.ActionMenu;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Northwoods.Go;
using OpenLicense;
using OpenLicense.LicenseFile;
using OpenLicense.LicenseFile.Constraints;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
using MetaBuilder.MetaControls;
using MetaBuilder.Graphing.Shapes.Binding.Intellisense;
using System.Collections.ObjectModel;
using MetaBuilder.Graphing.Controllers;

namespace MetaBuilder.UIControls.GraphingUI
{
    public delegate void DelegateOpenFile(String s, bool refresh);
    //   [GuidAttribute("2de915e1-df71-3443-9f4d-32259c92ced2")]
    [LicenseProvider(typeof(OpenLicenseProvider))]
    public partial class DockingForm : Form, IMRUClient
    {

        //private bool isViewer;
        //public bool IsViewer
        //{
        //    get { return isViewer; }
        //    set { isViewer = value; }
        //}

        public DockingForm()
        {
            delegateOpenFile = new DelegateOpenFile(this.OpenFileInApplicableWindow);

            /*Tools.DocHandlers.ImportExport.OptionsForm oform = new MetaBuilder.UIControls.GraphingUI.Tools.DocHandlers.ImportExport.OptionsForm();
            oform.ShowDialog(this);*/
            /*
            Dialogs.DatabaseManagement.ManageArtefacts ma = new ManageArtefacts();
            ma.ShowDialog(this);
            Application.Exit();
            */
            DockFormController = new DockingFormController(this);

            if (AppInterop.CheckPrevInstance())
            {
                //if (FilesToOpenOnStartup != null)
                //    Core.Log.WriteLog("Previous instance found, exiting. Lost args (" + FilesToOpenOnStartup.ToString() + ")");
                Application.Exit();
                this.Close();
            }
            else
            {
                License license;
                LicenseManager.IsValid(GetType(), this, out license);
                if (license == null)
                {
                    throw new ApplicationException("A suitable license file couldn't be found");
                }
                else if (((OpenLicenseFile)license).FailureReason != String.Empty)
                {
                    throw new ApplicationException(((OpenLicenseFile)license).FailureReason);
                }

                OpenLicenseFile ofl = (OpenLicenseFile)license;
                CoreInjector coreInjector = new CoreInjector();
                coreInjector.InjectCoreVariables();

                List<IConstraint> constraints = ofl.Constraints;
                bool demoFound = false;
                foreach (IConstraint constraint in constraints)
                {
                    if (constraint is DemoConstraint)
                    {
                        //Text = "MetaBuilder - Trial";
                        Variables.Instance.IsDemo = true;
                        DemoConstraint demoConstraint = constraint as DemoConstraint;
                        DateTime date1 = DateTime.Now;
                        TimeSpan ts;
                        ts = demoConstraint.EndDate.Subtract(date1);
                        Variables.Instance.DemoDaysLeft = ts.Days;

                        //Text += ": " + ts.Days + "/" + demoConstraint.Duration + " remaining";
                        demoFound = true;

                        //if (ts.Days <= 0)
                        //{
                        //    MessageBox.Show(this,"The trial period for metabuilder has expired." + Environment.NewLine + "Please contact DISCON Specialists to buy a desktop licence in order to continue use of our product.", "Metabuilder Trial", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        //    Application.Exit();
                        //}

                    }
                }
                if (!demoFound)
                    Variables.Instance.IsDesktopEdition = true;
            }
            //                if (!demoFound)
            //                    Variables.Instance.IsDesktopEdition = true;

            //                // COMMENTED: WHY? 
            //                // CloseAllContents();
            //                // END;
            //                dockPanel1 = new DockPanel();
            //                InitializeComponent();
            //                m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
            myDockingForm = DockFormController.DockingForm;
            //                navigatorWindow = m_panningWindow;
            ShallowCopies = DockFormController.ShallowCopies;

            //#if DEBUG
            //                Text = "MetaBuilder (DEBUG!)";
            //                BackColor = Color.Khaki;

            //                //MainMenuStrip.BackColor = Color.Khaki;
            //#endif
            //                this.dockPanel1.DockBackColor = Color.FromKnownColor(KnownColor.AppWorkspace);

            //                Thread t = new Thread(new ThreadStart(LoadFontsInBackground));
            //                t.IsBackground = true;
            //                t.Start();
            //            }
        }
        public DockingForm(string type)
        {
            if (type == "Viewer")
            {
                delegateOpenFile = new DelegateOpenFile(this.OpenFileInApplicableWindow);

                Core.Variables.Instance.IsViewer = true;
                DockFormController = new DockingFormController(this);
                myDockingForm = this;
                //ShowMetaObjectProperties(null);
                Text += " Diagram Viewer";
                //Core.Variables.Instance.IsViewer = true;
                return;
            }
            Application.Exit();
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);
            if (FilesToOpenOnStartup != null && FilesToOpenOnStartup.Length > 0)
            {
                OpenMultipleFiles(FilesToOpenOnStartup);
                FilesToOpenOnStartup = null;
            }
        }

        #region Fields

        private Collection<GoObject> shallowGoObjects;
        public bool ForceCloseCancel = false;
        //private bool isSaving;
        public DelegateOpenFile delegateOpenFile;
        public string[] FilesToOpenOnStartup;
        private License license = null;
        // used to hold instances of shallow copy for other diagrams
        [NonSerialized]
        private Collection<IShallowCopyable> shallowCopies;
        private TreeLoader m_treeLoader = new TreeLoader();
        private static DockingForm myDockingForm;
        //public static PanningDocker navigatorWindow;
        public DeserializeDockContent m_deserializeDockContent;
        public void SetLoadingPositionForHPUM(int x, int y, bool fp)
        {
            forcePosition = fp;
            loadingPosX = x;
            loadingPosY = y;
        }
        public GraphViewContainer SetcurrentGraphViewContainerForHPUM
        {
            set
            {
                loaded = 0;
                iterated = 0;
                currentGraphViewContainerForHPUM = value;
                if (value == null)
                {
                    DockingForm.DockForm.tImportOptions = DockingForm.TextImportOptions.Nothing;
                    loadingPosX = 0;
                    loadingPosY = 0;
                }
                else
                {
                    DockingForm.DockForm.tImportOptions = DockingForm.TextImportOptions.Details;
                    loadingPosX = 200;
                    loadingPosY = 200;
                }
            }
        }
        private GraphViewContainer currentGraphViewContainerForHPUM = null;
        private DockingFormController dockFormController;

        #endregion Fields

        #region Properties

        public DockingFormController DockFormController
        {
            get
            {
                //if (dockFormController == null)
                //    dockFormController = new DockingFormController(null);
                return dockFormController;
            }
            set { dockFormController = value; }
        }

        public string CurrentWorkspace
        {
            get { return toolStripDropDownSelectWorkspace.Text; }
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

        public Collection<GoObject> ShallowGoObjects
        {
            get { return shallowGoObjects; }
            set { shallowGoObjects = value; }
        }

        public static DockingForm DockForm
        {
            get { return myDockingForm; }
        }

        //public static PanningDocker NavigatorWindow
        //{
        //    get { return navigatorWindow; }
        //}

        public bool PWRModeEnabled
        {
            get { return toolStripPWR.Visible; }
            set { toolStripPWR.Visible = value; }
        }

        #endregion Properties

        #region Methods

        // Public Methods

        public string GetWorkspaceTypeName(int wsTypeID)
        {
            string wsType = "";
            if (wsTypeID == 3)
            {
                wsType = " [Server Workspace]";
            }
            else
            {
                wsType = " [Client Workspace]";
            }
            return wsType;
        }

        //public NormalDiagram OpenGraphFileFromDatabase(GraphFile file, bool SkipRefresh)
        //{
        //    if (DockingForm.DockForm.OpenedFiles.ContainsKey(file.Name.ToLower()))
        //    {
        //        return null;
        //    }

        //    GraphViewContainer gvContainer = new GraphViewContainer(FileTypeList.Diagram);
        //    NormalDiagram diagram = gvContainer.LoadGraphFile(file);
        //    if (!DockingForm.DockForm.OpenedFiles.ContainsKey(diagram.Name.ToLower()))
        //    {
        //        DockingForm.DockForm.OpenedFiles.Add(diagram.Name.ToLower(), gvContainer.ContainerID);
        //    }
        //    gvContainer.Show(dockPanel1);
        //    gvContainer.SetTabText();
        //    if (gvContainer.ReadOnly)
        //        SkipRefresh = false;

        //    gvContainer.FinaliseDocumentAfterLoading(SkipRefresh);
        //    BringToFront();
        //    return diagram;
        //}
        public NormalDiagram OpenGraphFileFromDatabase(GraphFile file, bool SkipRefresh, bool fromServer)
        {
            //if (fromServer) //there will never be open documents when opening from server, so clear them all in case
            //{
            //    DockingForm.DockForm.OpenedFiles.Clear();
            //}
            //else
            //{
            //if (DockingForm.DockForm.OpenedFiles.ContainsKey(file.Name.ToLower()))
            //{
            //    return null;
            //}
            //}

            GraphViewContainer gvContainer = new GraphViewContainer(FileTypeList.Diagram);
            NormalDiagram diagram = null;
            diagram = gvContainer.LoadGraphFile(file, fromServer);
            if (!DockingForm.DockForm.OpenedFiles.ContainsKey(diagram.Name.ToLower()))
            {
                DockingForm.DockForm.OpenedFiles.Add(diagram.Name.ToLower(), gvContainer.ContainerID);
            }
            gvContainer.Show(dockPanel1);
            gvContainer.SetTabText();
            if (gvContainer.ReadOnly)
                SkipRefresh = false;

            if (!fromServer)
            {
                gvContainer.FinaliseDocumentAfterLoading(SkipRefresh);
            }
            else
            {
                SetToolstripWorkspaceToDiagram(diagram.VersionManager.CurrentVersion);
            }

            BringToFront();
            return diagram;
        }

        public void ResetStatus()
        {
            try
            {
                if (progressBar1.Maximum != 0)
                    ProgressUpdate(0);
                //progressBar1.Value = 0;
                if (statusLabel.Text == "Read Only - Cannot Save")
                {
                    //pause updates for a second so you can see it!!
                    Thread.Sleep(1000);
                }
                UpdateStatusLabel("Ready");
                //statusLabel.Text = "Ready";
                //UpdatePanWindow();
                //UpdateWindowList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            try
            {
                if (GetCurrentGraphView() != null)
                    GetCurrentGraphView().EndUpdate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //Update();
        }

        public void SetToolstripToGlobalVarWorkspace(string spaceType)
        {
            toolStripStatusCurrentWorkspace.Text = string.IsNullOrEmpty(spaceType) ? "Current Workspace: " : spaceType + " Workspace: ";

            string def = "";
            if (Variables.Instance.DefaultWorkspace.Length > 0)
            {
                if (Variables.Instance.CurrentWorkspaceName == Variables.Instance.DefaultWorkspace && Variables.Instance.CurrentWorkspaceTypeId == Variables.Instance.DefaultWorkspaceID)
                {
                    def = " <Default>";
                }
            }

            int wsTypeID = Variables.Instance.CurrentWorkspaceTypeId;
            string wsType = GetWorkspaceTypeName(wsTypeID);
            toolStripDropDownSelectWorkspace.Text = Variables.Instance.CurrentWorkspaceName + wsType + def;

            toolStripDropDownSelectWorkspace.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            if (wsType.Contains(Core.Variables.Instance.ServerProvider))
                toolStripDropDownSelectWorkspace.Image = imageListWorkspaces.Images[0];
            else
                toolStripDropDownSelectWorkspace.Image = imageListWorkspaces.Images[1];
        }

        public void SetWorkspaceName(string wsName, string wsType, bool isDiagram)
        {
            string def = "";
            if (Variables.Instance.DefaultWorkspace.Length > 0)
            {
                if (wsName == Variables.Instance.DefaultWorkspace)//int.Parse(wsType) == Variables.Instance.DefaultWorkspaceID
                {
                    def = " <Default>";
                }
            }

            if (isDiagram)
            {
                toolStripStatusCurrentWorkspace.Text = "Diagram Workspace: ";
            }
            else
                toolStripStatusCurrentWorkspace.Text = "Current Workspace: ";
            toolStripDropDownSelectWorkspace.Text = wsName + wsType + def;

            toolStripDropDownSelectWorkspace.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            if (wsType == "3")
                toolStripDropDownSelectWorkspace.Image = imageListWorkspaces.Images[0];
            else
                toolStripDropDownSelectWorkspace.Image = imageListWorkspaces.Images[1];
        }

        //TODO: Implement! MenuItems vs AppState
        public void ToggleMenuItemsForCurrentAppState()
        {
            GraphView currentGraphView = GetCurrentGraphView();

            bool noDocumentsOpen = currentGraphView == null;
            //menuItemViewPropertiesWindowDiagramming.Checked = (m_propertyGridWindow.DockState != DockState.Hidden && m_propertyGridWindow.DockState != DockState.Unknown);
            menuItemViewPropertiesMeta.Checked = (m_metaPropsWindow.DockState != DockState.Hidden && m_metaPropsWindow.DockState != DockState.Unknown);
            menuItemViewObjectExplorer.Checked = (m_metaObjectExplorer.DockState != DockState.Hidden && m_metaObjectExplorer.DockState != DockState.Unknown);
            menuItemViewStencils.Checked = (m_paletteDocker.DockState != DockState.Hidden && m_paletteDocker.DockState != DockState.Unknown);
            //menuItemViewPanAndZoom.Checked = (m_panningWindow.DockState != DockState.Hidden && m_panningWindow.DockState != DockState.Unknown);
            menuItemViewTaskList.Checked = (m_taskDocker.DockState != DockState.Hidden && m_taskDocker.DockState != DockState.Unknown);

            bool hasDocuments = dockPanel1.DocumentsCount > 0;

            //MOVED LFD
            //menuItemToolsLoadFromDatabase.Visible = hasDocuments;
            menuItemToolsLoadFromDBRefreshObjects.Visible = hasDocuments;
            menuItemToolsLoadFromDBChangedObjects.Visible = hasDocuments;

            if (GetCurrentGraphViewContainer() != null)
                menuItemToolsLoadFromDatabase.Visible = !GetCurrentGraphViewContainer().ReadOnly;

            menuItemWindows.Visible = hasDocuments;
            //menuItemEdit.Visible = hasDocuments;
            menuItemViewPanAndZoom.Enabled = hasDocuments;
            //m_panningWindow.goOverview1.Visible = hasDocuments;
            if (!hasDocuments)
            {
                //GetTaskDocker().ClearTasks();
                if (!this.Disposing)
                {
                    //m_propertyGridWindow.SelectedObject = null;
                    if (!(m_metaObjectExplorer.IsActivated))
                        m_metaPropsWindow.SelectedObject = null;
                }
                ValidationResultForm.Hide();
            }

            menuItemFileOpen.Enabled = stripButtonOpen.Enabled = !clearing;
            menuItemFileNewDiagram.Enabled = stripButtonNewDiagram.Enabled = !clearing;
            menuItemFileNew.Enabled = !clearing && !Core.Variables.Instance.IsViewer;

            menuItemDatabaseObjectManager.Enabled = !opening && !hasDocuments && !clearing;// && !CurrentlySaving.Count > 0;
            menuItemFileWorkspaces.Enabled = !opening && !hasDocuments && !clearing;// noDocumentsOpen;
            menuItemDatabaseServer.Enabled = !opening && !hasDocuments && !clearing; //notSaving
            toolStripDropDownSelectWorkspace.Enabled = !opening && !hasDocuments && !clearing;

            menuItemDatabaseServer.Visible = Variables.Instance.IsServer;
            //            menuItemToolsSynchronise.Visible = false;
            //#if DEBUG
            //            menuItemToolsSynchronise.Visible = true;
            //#endif

            menuItemViewObjectExplorer.Visible = true && !Core.Variables.Instance.IsViewer; //Variables.Instance.ShowDeveloperItems;
            menuItemToolsMode.Visible = Variables.Instance.IsDeveloperEdition;
            menuItemDBDictionary.Visible = Variables.Instance.ShowDeveloperItems;
            menuItemFileNewSymbol.Visible = Variables.Instance.ShowDeveloperItems;
            stripButtonNewSymbol.Visible = Variables.Instance.ShowDeveloperItems;
            menuItemDatabaseImporteHPUM.Visible = Variables.Instance.ShowDeveloperItems;
            menuItemDatabaseImportObjects.Visible = Variables.Instance.ShowDeveloperItems;

            stripButtonSaveAll.Visible = DiagramCount > 1 && !Core.Variables.Instance.IsViewer;

            if (Core.Variables.Instance.IsViewer)
            {
                menuItemHelp.Visible = menuItemDB.Visible = menuItemReports.Visible = menuItemToolsOptions.Visible = menuItemViewStencils.Visible = menuItemViewTaskList.Visible = menuItemDatabaseServer.Visible = menuItemTools.Visible = menuItemFileNewDiagram.Visible = stripButtonNewDiagram.Visible = false;
            }
        }

        public int DiagramCount
        {
            get
            {
                int count = 0;
                foreach (IDockContent content in dockPanel1.Documents)
                    if (content is GraphViewContainer)
                        count++;

                return count;
            }
        }

        //public void UpdatePanWindow()
        //{
        //    try
        //    {
        //        if (m_panningWindow != null)
        //        {
        //            m_panningWindow.goOverview1.Document = new GoDocument();
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}

        //public void UpdateWindowList()
        //{
        //DockingForm_MdiChildActivate(this, EventArgs.Empty);
        //}

        // Protected Methods 

        protected override void WndProc(ref Message m)
        {
            if (m.HWnd == Handle && m.Msg == 0x004A)
            {
                AppInterop.COPYDATA cd = (AppInterop.COPYDATA)Marshal.PtrToStructure(m.LParam, typeof(AppInterop.COPYDATA));
                string data = Marshal.PtrToStringUni(cd.lpData, Convert.ToInt32(cd.cbData) / 2);
                string[] args = data.Split(AppInterop.SplitChar);

                if (args.Length > 0)
                {
                    for (int i = 1; i < args.Length; i++)
                    {
                        if (args[i].Length > 0)
                        {
                            OpenFileInApplicableWindow(args[i], false);
                        }
                    }
                }
            }
            base.WndProc(ref m);
        }

        // Private Methods
        public bool IsActiveWindow
        {
            get
            {
                return (DockingFormController.GetCurWindowTitle().ToLower().Contains("metabuilder"));
            }
        }
        private bool boundCHEATeventToPropertyGridChanged;
        public void UpdateMenuItems()
        {
            if (!boundCHEATeventToPropertyGridChanged)
            {
                this.m_metaPropsWindow.metaPropertyGrid1.PropertyValueChanged += new PropertyValueChangedEventHandler(metaPropertyGrid1_PropertyValueChanged);
                boundCHEATeventToPropertyGridChanged = true;
            }
            if (this.WindowState != FormWindowState.Minimized)
            {
                if (IsActiveWindow)
                {
                    try
                    {
                        ToggleMenuItemsForCurrentAppState();
                        if (ActiveMdiChild != null)
                        {
                            GraphViewContainer gvContainer = ActiveMdiChild as GraphViewContainer;
                            if (gvContainer != null)
                            {
                                gvContainer.ToggleMenuItemsForCurrentAppState();
                                //if (!gvContainer.View.IsEditing)
                                //{
                                //    Core.Variables.spelling.Dispose();
                                //    Core.Variables.dictionary.Dispose();
                                //}
                                if (gvContainer.IsClosing)
                                {
                                    SetToolstripToGlobalVarWorkspace(null);
                                }
                            }
                            else
                            {
                                SetToolstripToGlobalVarWorkspace(null);
                            }
                        }
                        else
                        {
                            SetToolstripToGlobalVarWorkspace(null);
                        }

                    }
                    catch
                    {
                        SetToolstripToGlobalVarWorkspace(null);
                    }
                }
            }
        }

        private void metaPropertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (GetCurrentGraphViewContainer() != null)
                GetCurrentGraphViewContainer().CustomModified = true;
        }

        private ToolStripStatusLabel memoryLabel;

        /// <summary>
        /// Toggle Items based on the application state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Idle(object sender, EventArgs e)
        {
            UpdateMenuItems();
#if DEBUG
            if (memoryLabel == null)
            {
                memoryLabel = new ToolStripStatusLabel();
                memoryLabel.AutoSize = true;
                memoryLabel.BackColor = Color.DarkRed;
                memoryLabel.ForeColor = Color.White;
                memoryLabel.Alignment = ToolStripItemAlignment.Right;
                statusStrip1.Items.Add(memoryLabel);
            }
            memoryLabel.Text = GC.GetTotalMemory(true).ToString() + " bytes in use";
#endif
        }

        private void ExportAssociations()
        {
            TempMappingExporter tmpExporter = new TempMappingExporter();

            if (MessageBox.Show(this, "Do you want to export from a specific workspace?", "Workspace Specific Export", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                Dialogs.ChooseWorkspaceForAction cwa = new ChooseWorkspaceForAction();
                cwa.ShowDialog(this);
                if (cwa.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    tmpExporter.Workspace = cwa.SelectedWorkspace;
                }
            }
            tmpExporter.ShowDialog(this);
        }

        [STAThread]
        private void ExportHierarchicalData(HierarchicalToText exporter, bool Numbered, bool Tabbed, string customFileName)
        {
            QueryForTextImport query = new QueryForTextImport();
            query.IsExport = true;
            DialogResult res = query.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                // query.MyField;
                ObjectFinder ofinder = new ObjectFinder(false);
                ofinder.IncludeStatusCombo = true;
                ofinder.ExcludeStatuses.Add(VCStatusList.Obsolete);
                ofinder.ExcludeStatuses.Add(VCStatusList.MarkedForDelete);
                ofinder.LimitToClass = query.MyClass;
                ofinder.AllowMultipleSelection = false;

                DialogResult result = ofinder.ShowDialog(this);
                if (result == DialogResult.OK && ofinder.SelectedObjectsList.Count > 0)
                {
                    SplashScreen.PleaseWait.ShowPleaseWaitForm();
                    SplashScreen.PleaseWait.SetStatus("Exporting");
                    try
                    {
                        exporter.MyClass = query.MyClass;
                        exporter.FieldToExport = query.MyField;
                        exporter.ObjectID = ofinder.SelectedObjectsList[0].pkid;
                        exporter.MachineName = ofinder.SelectedObjectsList[0].MachineName;
                        exporter.Export("Hierarchical Export - " + query.MyClass + " TimeStamp - " + strings.GetDateStampString(), Numbered, Tabbed, 0, "");
                        SplashScreen.PleaseWait.CloseForm();
                        MessageBox.Show(this, "Export Completed", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteLog("ExportHierarchicalData - DockingForm" + Environment.NewLine + ex.ToString());
                        if (Core.Variables.Instance.VerboseLogging == true)
                        {
                            MessageBox.Show(this, "A problem has occurred and has been logged. Please contact technical support for assistance", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show(this, "A problem has occurred, please contact technical support for assistance", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
        [STAThread]
        private void ExportHierarchicalToTextBoth()
        {
            HierarchicalToText hierarchicalToText = new HierarchicalToText();
            ExportHierarchicalData(hierarchicalToText, true, true, "");
        }
        [STAThread]
        private void ExportToTextNumbered()
        {
            HierarchicalToText hierarchicalToText = new HierarchicalToText();
            ExportHierarchicalData(hierarchicalToText, true, false, "");
        }
        [STAThread]
        private void ExportToTextTabbed()
        {
            HierarchicalToText hierarchicalToText = new HierarchicalToText();
            ExportHierarchicalData(hierarchicalToText, false, true, "");
        }

        private void ExportObjectContext()
        {
            ExportMultipleObjectsContext exportSetup = new ExportMultipleObjectsContext(false);
            exportSetup.ShowDialog(this);
        }
        [STAThread]
        private void ExportHierarchicalToWord()
        {
            HierarchicalToWord hierarchicalToWord = new HierarchicalToWord();
            ExportHierarchicalData(hierarchicalToWord, true, false, "");
        }
        [STAThread]
        private void ExportHierarchicalToWordBoth()
        {
            HierarchicalToWord hierarchicalToWord = new HierarchicalToWord();
            ExportHierarchicalData(hierarchicalToWord, true, true, "");
        }
        [STAThread]
        private void ExportHierarchicalToWordTabbed()
        {
            HierarchicalToWord hierarchicalToWord = new HierarchicalToWord();
            ExportHierarchicalData(hierarchicalToWord, false, true, "");
        }

        /// <summary>
        /// Initialise Recent files menu manager
        /// </summary>
        //private void InitMRUManager()
        //{
        //    mruManager = new MRUManager();
        //    mruManager.Initialize(
        //        this, // owner form
        //        menuItemFileRecent, // Recent Files menu item
        //        "Software\\DISCON Specialists\\MetaBuilder"); // Registry path to keep MRU list
        //}

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

        /// <summary>
        /// Setup panning window to point to the correct GVContainer
        /// </summary>
        //private void MakePanningWindowObserveActiveMDI()
        //{
        //    if (ActiveMdiChild is GraphViewContainer)
        //    {
        //        //m_panningWindow.goOverview1.Observed = ((GraphViewContainer)ActiveMdiChild).View;
        //    }
        //}

        private void menuItemDatabaseDiagramManager_Click(object sender, EventArgs e)
        {
            DiagramManager dgmManager = new DiagramManager(false, null);
            dgmManager.ShowDialog(this);
        }

        private void menuItemDatabaseExportAssociations_Click(object sender, EventArgs e)
        {
            ThreadStart ts = new ThreadStart(ExportAssociations);
            Thread t = new Thread(ts);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void menuItemDatabaseExportHierarchyBoth_Click(object sender, EventArgs e)
        {
            ExportHierarchicalToTextBoth();
            //ThreadStart ts = new ThreadStart();
            //Thread t = new Thread(ts);
            //t.SetApartmentState(ApartmentState.STA);
            //t.Start();
        }

        private void menuItemDatabaseExportHierarchyNumbered_Click(object sender, EventArgs e)
        {
            ExportToTextNumbered();
            //ThreadStart ts = new ThreadStart();
            //Thread t = new Thread(ts);
            //t.SetApartmentState(ApartmentState.STA);
            //t.Start();
        }

        private void menuItemDatabaseExportHierarchyTabbed_Click(object sender, EventArgs e)
        {
            ExportToTextTabbed();
            //ThreadStart ts = new ThreadStart();
            //Thread t = new Thread(ts);
            //t.SetApartmentState(ApartmentState.STA);
            //t.Start();
        }

        private void menuItemDatabaseExportHierarchyWordBoth_Click(object sender, EventArgs e)
        {
            ThreadStart ts = new ThreadStart(ExportHierarchicalToWordBoth);
            Thread t = new Thread(ts);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void menuItemDatabaseExportHierarchyWordNumbered_Click(object sender, EventArgs e)
        {
            ThreadStart ts = new ThreadStart(ExportHierarchicalToWord);
            Thread t = new Thread(ts);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void menuItemDatabaseExportHierarchyWordTabbed_Click(object sender, EventArgs e)
        {
            ThreadStart ts = new ThreadStart(ExportHierarchicalToWordTabbed);
            Thread t = new Thread(ts);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void menuItemDatabaseExportObjectContext_Click(object sender, EventArgs e)
        {
            ThreadStart ts = new ThreadStart(ExportObjectContext);
            Thread t = new Thread(ts);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void menuItemDatabaseExportObjectFlow_Click(object sender, EventArgs e)
        {
            Tools.ObjectFlowExport.ObjectFlowExportInterface win = new MetaBuilder.UIControls.GraphingUI.Tools.ObjectFlowExport.ObjectFlowExportInterface();
            win.Show();
        }

        private void menuItemDatabaseObjectManager_Click_1(object sender, EventArgs e)
        {
            if (clearing)
                return;
            DatabaseManagement databaseManagementForm = new DatabaseManagement();
            databaseManagementForm.ShowDialog(this);
            CacheManager cacheManager = CacheFactory.GetCacheManager();
            cacheManager.Flush();
            loadWorkspacesButDontSelectOne();
            MetaObjectExplorer.Reset();
        }
        public void loadWorkspacesButDontSelectOne()
        {
            if (clearing)
                return;

            toolStripDropDownSelectWorkspace.DropDownItems.Clear();
            int x = 0;

            if (Core.Variables.Instance.WorkspaceHashtable == null)
                Core.Variables.Instance.WorkspaceHashtable = new System.Collections.Hashtable();

            foreach (Workspace ws in d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.WorkspaceProvider.GetAll())
            {
                //add each worksapce to variables hashtable
                if (!Core.Variables.Instance.WorkspaceHashtable.ContainsKey(ws.Name + "#" + ws.WorkspaceTypeId))
                {
                    Core.Variables.Instance.WorkspaceHashtable.Add(ws.Name + "#" + ws.WorkspaceTypeId, null);
                }
                //Add dropdown to hashtable

                //ToolStripDropDownItem i;// = new ToolStripDropDownItem();
                //i.Text = ws.Name;
                //if (ws.WorkspaceTypeId == 3)
                //    i.Image = imageListWorkspaces.Images[0];
                //else
                //    i.Image = imageListWorkspaces.Images[1];
                //i.Tag = ws;
                string def = "";
                if (Variables.Instance.DefaultWorkspace.Length > 0)
                {
                    if (ws.Name == Variables.Instance.DefaultWorkspace && ws.WorkspaceTypeId == Variables.Instance.DefaultWorkspaceID)
                    {
                        def = " <Default>";
                    }
                }
                toolStripDropDownSelectWorkspace.DropDownItems.Add(@ws.Name + def);
                toolStripDropDownSelectWorkspace.DropDownItems[x].Tag = ws;

                //if (ws.WorkspaceTypeId == 3)
                //    toolStripDropDownSelectWorkspace.ImageIndex = 0;
                //else
                //    toolStripDropDownSelectWorkspace.ImageIndex = 1;

                //if (ws.WorkspaceTypeId == 3)
                toolStripDropDownSelectWorkspace.DropDownItems[x].Image = (ws.WorkspaceTypeId == 3) ? imageListWorkspaces.Images[0] : imageListWorkspaces.Images[1];
                //else
                //toolStripDropDownSelectWorkspace.DropDownItems[x].Image = imageListWorkspaces.Images[1];
                x++;
            }
        }

        //private void menuItemFileWorkspaces_Click(object sender, EventArgs e)
        //{
        //    ChooseWorkspace cworkspace = new ChooseWorkspace();
        //    cworkspace.BringToFront();
        //    cworkspace.StartPosition = FormStartPosition.CenterScreen;
        //    cworkspace.ShowDialog(this);
        //    /*
        //    Dialogs.RelationshipManager.RelmanContainer relContainer = new MetaBuilder.UIControls.Dialogs.RelationshipManager.RelmanContainer();
        //    relContainer.ShowDialog(this);*/
        //}

        public bool clearing = false;
        private Common.Reports rptForm = null;
        private void menuItemReports_Click(object sender, EventArgs e)
        {
            if (rptForm == null || rptForm.IsDisposed)
            {
                rptForm = new Reports();
                rptForm.Show(this);
                rptForm.PrepareForm();
                rptForm.ShowInTaskbar = true;
            }
            rptForm.BringToFront();
        }

        private void menuItemToolsDocumentTree_Click(object sender, EventArgs e)
        {
            GetTreeLoader().Show(dockPanel1, DockState.DockLeft);
            //m_paletteDocker.Show(dockPanel1, DockState.DockLeft);
        }

        private void menuItemToolsPermissions_Click(object sender, EventArgs e)
        {
            PromptForPassword pfp = new PromptForPassword();
            pfp.HashedPassword = "cwRkEyzlTZ1bEquScqWGTA==";
            DialogResult result = pfp.ShowDialog(this);
            if (result == DialogResult.OK && pfp.Authenticated)
            {
                ServerWorkspacePermission swp = new ServerWorkspacePermission();
                swp.ShowDialog(this);
            }
            return;
            //UserPermissions upm = new UserPermissions();
            //upm.ShowDialog(this);
        }

        private void menuItemToolsSynchronise_Click(object sender, EventArgs e)
        {
            if (clearing)
                return;

            int serverDiagramSavingCount = 0;
            foreach (DiagramSaver s in CurrentlySaving)
                //if (s.Diagram.VersionManager.CurrentVersion.WorkspaceTypeId == 3)
                serverDiagramSavingCount += 1;
            if (serverDiagramSavingCount > 0)
            {
                MessageBox.Show(this, "There " + (serverDiagramSavingCount > 1 ? "are" : "is") + " currently " + serverDiagramSavingCount + " diagram" + (serverDiagramSavingCount > 1 ? "s" : "") + " being saved." + Environment.NewLine + "Please wait for saving to complete and try again.", "Saving in progress.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Core.Networking.Pinger.Ping(Core.Variables.Instance.ServerConnectionString))
            {
                Synchroniser sync = new Synchroniser();
                sync.ShowDialog(this);
                m_metaObjectExplorer.btnReset_Click(sender, e);
                loadWorkspacesButDontSelectOne();
            }
            else
            {
                MessageBox.Show(this, "Unable to connect to your current synchronisation server.", "Ping", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void menuItemToolsSynchronisationManager_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
#if DEBUG
            if (e.Button == MouseButtons.Left)
                return;

            if (clearing)
                return;

            if (GetCurrentGraphView() == null)
            {
                int serverDiagramSavingCount = 0;
                foreach (DiagramSaver s in CurrentlySaving)
                    //if (s.Diagram.VersionManager.CurrentVersion.WorkspaceTypeId == 3)
                    serverDiagramSavingCount += 1;
                if (serverDiagramSavingCount > 0)
                {
                    MessageBox.Show(this, "There " + (serverDiagramSavingCount > 1 ? "are" : "is") + " currently " + serverDiagramSavingCount + " diagram" + (serverDiagramSavingCount > 1 ? "s" : "") + " being saved." + Environment.NewLine + "Please wait for saving to complete and try again.", "Saving in progress.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    if (Core.Networking.Pinger.Ping(Core.Variables.Instance.ServerConnectionString))
                    {
                        CacheManager cacheManager = CacheFactory.GetCacheManager();
                        cacheManager.Flush();

                        MetaBuilder.UIControls.GraphingUI.Tools.SynchronisationManager.SynchronisationManager man = new MetaBuilder.UIControls.GraphingUI.Tools.SynchronisationManager.SynchronisationManager();
                        man.ShowDialog();

                        m_metaObjectExplorer.btnReset_Click(sender, e);

                        //refreshes workspaces which were found after synchronising
                        loadWorkspacesButDontSelectOne();
                    }
                    else
                    {
                        MessageBox.Show(this, "Unable to connect to your current synchronisation server.", "Ping", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Cannot establish a connection to the server");
                    LogEntry logEntry = new LogEntry();
                    logEntry.Title = "Database Connection Failed";
                    logEntry.Message = ex.ToString();
                    Logger.Write(logEntry);
                }
            }
            else
            {
                MessageBox.Show(this, "Please save and close all diagrams before synchronisation", "Cannot Synchronise", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
#endif
        }

        private void menuItemToolsSynchronisationManager_Click(object sender, EventArgs e)
        {
            if (clearing)
                return;

            if (GetCurrentGraphView() == null)
            {
                int serverDiagramSavingCount = 0;
                foreach (DiagramSaver s in CurrentlySaving)
                    //if (s.Diagram.VersionManager.CurrentVersion.WorkspaceTypeId == 3)
                    serverDiagramSavingCount += 1;
                if (serverDiagramSavingCount > 0)
                {
                    MessageBox.Show(this, "There " + (serverDiagramSavingCount > 1 ? "are" : "is") + " currently " + serverDiagramSavingCount + " diagram" + (serverDiagramSavingCount > 1 ? "s" : "") + " being saved." + Environment.NewLine + "Please wait for saving to complete and try again.", "Saving in progress.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    if (Core.Networking.Pinger.Ping(Core.Variables.Instance.ServerConnectionString))
                    {
                        CacheManager cacheManager = CacheFactory.GetCacheManager();
                        cacheManager.Flush();

                        ObjectManager oman = new ObjectManager();
                        oman.ShowDialog(this);
                        m_metaObjectExplorer.btnReset_Click(sender, e);

                        //refreshes workspaces which were found after synchronising
                        loadWorkspacesButDontSelectOne();
                    }
                    else
                    {
                        MessageBox.Show(this, "Unable to connect to your current synchronisation server.", "Ping", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Cannot establish a connection to the server");
                    LogEntry logEntry = new LogEntry();
                    logEntry.Title = "Database Connection Failed";
                    logEntry.Message = ex.ToString();
                    Logger.Write(logEntry);
                }
            }
            else
            {
                MessageBox.Show(this, "Please save and close all diagrams before synchronisation", "Cannot Synchronise", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void mnuItemDatabaseSWOTAnalysis_Click(object sender, EventArgs e)
        {
            SWOT.SWOT sw = new SWOT.SWOT();
            sw.ShowDialog(this);
        }
        private void mnuItemDatabaseResourceGapAnalysis_Click(object sender, EventArgs e)
        {
            Tools.GapAnalysis.ResourceGapAnalysis a = new Tools.GapAnalysis.ResourceGapAnalysis();
            a.ShowDialog(this);
        }

        private void processCommandLine(string CommandLine)
        {
            MessageBox.Show(this, "Starting " + CommandLine);
        }

        private void saveToDatabaseDisabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveToDatabaseEnabledToolStripMenuItem.Checked = false;
            saveToDatabaseDisabledToolStripMenuItem.Checked = true;
            Variables.Instance.SaveToDatabaseEnabled = false;
            MetaSettings s = new MetaSettings();
            s.PutSetting(MetaSettings.SAVETODATABASEENABLED, false);
            toolstripSaveMode.Image = imageList1.Images[1];
            toolstripSaveMode.Text = "Save to Database Disabled";
        }

        private void saveToDatabaseEnabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveToDatabaseDisabledToolStripMenuItem.Checked = false;
            saveToDatabaseEnabledToolStripMenuItem.Checked = true;
            Variables.Instance.SaveToDatabaseEnabled = true;
            MetaSettings s = new MetaSettings();
            s.PutSetting(MetaSettings.SAVETODATABASEENABLED, true);
            toolstripSaveMode.Image = imageList1.Images[0];
            toolstripSaveMode.Text = "Save to Database Enabled";
        }

        private void testAssocsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AssociationManager assocMan = new AssociationManager(false);
            assocMan.ShowDialog(this);
        }

        private void tmpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* TEMPLATE: Open all files with ADD in the name from the database
            TList<GraphFile> list = new TList<GraphFile>();
            list = DataRepository.GraphFileProvider.GetAll();
            foreach (GraphFile file in list)
            {
                if (file.Name.IndexOf("ADD") > -1)
                    OpenGraphFileFromDatabase(file);
            }
            */
        }

        #endregion Methods

        #region Docking and Documents

        private IDockContent FindDocument(string text)
        {
            if (dockPanel1.DocumentStyle == DocumentStyle.SystemMdi)
            {
                foreach (Form form in MdiChildren)
                    if (form.Text == text)
                        return form as IDockContent;
                return null;
            }
            else
            {
                foreach (IDockContent content in dockPanel1.Documents)
                    if (content.DockHandler.TabText == text)
                        return content;
                return null;
            }
        }

        protected void NewDocument(FileTypeList fileType)
        {
            GraphViewContainer gvContainer = new GraphViewContainer(fileType);
            gvContainer.Show(dockPanel1);
            if (dockPanel1.DocumentStyle == DocumentStyle.SystemMdi)
            {
                gvContainer.MdiParent = this;
                gvContainer.Show();
            }
            else
                gvContainer.Show(dockPanel1);

            SetToolstripToGlobalVarWorkspace("Diagram");
        }

        #endregion

        #region Form Events

        private void DockingForm_Load(object sender, EventArgs e)
        {
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            bool loaded = false;
            if (File.Exists(configFile))
            {
                try
                {
                    dockPanel1.LoadFromXml(configFile, m_deserializeDockContent);
                    loaded = true;
                }
                catch
                {
                    loaded = false;
                }
            }

            if (Core.Variables.Instance.IsViewer)
            {
                Application.Idle += new EventHandler(Application_Idle);
                delegateOpenFile = new DelegateOpenFile(this.OpenFileInApplicableWindow);
                toolStripDropDownSelectWorkspace.Visible = toolStripStatusCurrentWorkspace.Visible = false;
                ToggleMenuItemsForCurrentAppState();

                if (loaded == false)
                {
                    m_metaPropsWindow.Show(dockPanel1, DockState.DockRight);
                }

                //if (FilesToOpenOnStartup.Length > 0)
                //{
                //    OpenMultipleFiles(FilesToOpenOnStartup);
                //    FilesToOpenOnStartup = null;
                //}

                return;
            }

            if (loaded == false) //the config is included in the installation. Thus the only way this happens is if that file cannot be loaded
            {
                // Load windows at default locations
                m_paletteDocker.Show(dockPanel1, DockState.DockLeft);
                m_metaObjectExplorer.Show(dockPanel1, DockState.DockLeftAutoHide);

                m_metaPropsWindow.Show(dockPanel1, DockState.DockRight);
                m_taskDocker.Show(dockPanel1, DockState.DockRightAutoHide);
            }

#if DEBUG
            m_propertyGridWindow.Show(dockPanel1, DockState.DockRight);
#endif

            menuItemViewTaskList.Checked = (m_taskDocker.DockPanel != null);
            //menuItemViewPanAndZoom.Checked = (m_panningWindow.DockPanel != null);
            menuItemViewStencils.Checked = (m_paletteDocker.DockPanel != null);
            //menuItemViewPropertiesWindowDiagramming.Checked = (m_propertyGridWindow.DockPanel != null);
            menuItemViewPropertiesMeta.Checked = (m_metaPropsWindow.DockPanel != null);
            menuItemViewObjectExplorer.Checked = (m_metaObjectExplorer.DockPanel != null);
            saveToDatabaseEnabledToolStripMenuItem.Checked = Variables.Instance.SaveToDatabaseEnabled;
            saveToDatabaseDisabledToolStripMenuItem.Checked = !Variables.Instance.SaveToDatabaseEnabled;

            if (imageList1.Images.Count > 1)
            {
                toolstripSaveMode.Image = (Variables.Instance.SaveToDatabaseEnabled) ? imageList1.Images[0] : imageList1.Images[1];
                toolstripSaveMode.Text = (Variables.Instance.SaveToDatabaseEnabled) ? "Save to Database Enabled" : "Save to Database Disabled";
            }

            buildShapeCacheThread();

            LoadWorkspaces();

            OpenedFiles = DockFormController.OpenedFiles;
            Application.Idle += new EventHandler(Application_Idle);

            ToggleMenuItemsForCurrentAppState();

            SQLScriptRunner.CheckForUpdates();

            //if (FilesToOpenOnStartup.Length > 0)
            //{
            //    OpenMultipleFiles(FilesToOpenOnStartup);
            //    FilesToOpenOnStartup = null;
            //}

            //if (Core.Variables.Instance.IsDeveloperEdition)
            //    ConstructHelp();

            //setVariablesForServerVersion();
            //this.BringToFront();
            progressBar1.MouseEnter += new EventHandler(progressBar1_MouseEnter);
            statusLabel.MouseEnter += new EventHandler(progressBar1_MouseEnter);
        }

        private void progressBar1_MouseEnter(object sender, EventArgs e)
        {
            if (progressBar1.Tag != null)
            {
                if (progressBar1.Tag is DiagramSaver && CurrentlySaving.Count > 0)
                {
                    if ((progressBar1.Tag as DiagramSaver).IsBusy)
                        DockingForm.DockForm.DisplayTip((progressBar1.Tag as DiagramSaver).FileName, "Currently saving - " + (progressBar1.Tag as DiagramSaver).Amount + "%");
                }
                else
                {
                    progressBar1.Tag.ToString();
                }
            }
        }

        //private void ConstructHelp()
        //{
        //    HelpProvider1.HelpNamespace = Application.StartupPath + "\\Metabuilder.chm";

        //    HelpProvider1.CanExtend(this); 
        //    HelpProvider1.SetHelpNavigator(this, HelpNavigator.TableOfContents);
        //    HelpProvider1.SetHelpString(this, "The main for which hosts all of the functions and processes requried to use metabuilder");
        //    HelpProvider1.SetHelpKeyword(this, "MetaBuilder");
        //    HelpProvider1.SetShowHelp(this, true);

        //    HelpProvider1.CanExtend(m_paletteDocker); 
        //    HelpProvider1.SetHelpNavigator(m_paletteDocker, HelpNavigator.TableOfContents);
        //    HelpProvider1.SetHelpString(m_paletteDocker, "This container allows you to open stencils which contain shapes that can be dragged onto the canvas if one is open");
        //    HelpProvider1.SetHelpKeyword(m_paletteDocker, "Stencil");
        //    HelpProvider1.SetShowHelp(m_paletteDocker, true);

        //}

        private void LoadWorkspaces()
        {
            //Load Workspace
            loadWorkspacesButDontSelectOne();
            //statusStrip1.ImageList = imageListWorkspaces;

            toolStripStatusCurrentWorkspace.Text = ""; //"[Select Workspace]";

            //Default selection
            //TODO : On chooseworkspace-sync
            if (Variables.Instance.DefaultWorkspace.Length > 0 && Core.Variables.Instance.DefaultWorkspaceID > 0)
            {
                //Get workspace
                Workspace ws = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.WorkspaceProvider.GetByNameWorkspaceTypeId(Core.Variables.Instance.DefaultWorkspace, Core.Variables.Instance.DefaultWorkspaceID);
                if (ws != null)
                {
                    Variables.Instance.CurrentWorkspaceName = Variables.Instance.DefaultWorkspace;
                    Variables.Instance.CurrentWorkspaceTypeId = Variables.Instance.DefaultWorkspaceID;
                    SetToolstripToGlobalVarWorkspace(null);
                    //toolStripDropDownSelectWorkspace.Text = Variables.Instance.CurrentWorkspaceName;
                }
                else
                {
                    //Select sandbox
                    Variables.Instance.CurrentWorkspaceName = "Sandbox";
                    Variables.Instance.CurrentWorkspaceTypeId = 1;
                    Variables.Instance.DefaultWorkspace = Variables.Instance.CurrentWorkspaceName;
                    Variables.Instance.DefaultWorkspaceID = Variables.Instance.CurrentWorkspaceTypeId;
                    SetToolstripToGlobalVarWorkspace(null);
                    //toolStripDropDownSelectWorkspace.Text = Variables.Instance.CurrentWorkspaceName;
                    //ChooseWorkspace cw = new ChooseWorkspace();
                    //cw.ShowDialog(this);
                }
            }
            else
            {
                //Select sandbox
                Variables.Instance.CurrentWorkspaceName = "Sandbox";
                Variables.Instance.CurrentWorkspaceTypeId = 1;
                Variables.Instance.DefaultWorkspace = Variables.Instance.CurrentWorkspaceName;
                Variables.Instance.DefaultWorkspaceID = Variables.Instance.CurrentWorkspaceTypeId;
                SetToolstripToGlobalVarWorkspace(null);
                //toolStripDropDownSelectWorkspace.Text = Variables.Instance.CurrentWorkspaceName;
                //ChooseWorkspace cw = new ChooseWorkspace();
                //cw.ShowDialog(this);
            }
        }

        private void toolStripDropDownSelectWorkspace_DropDownItemClicked(object sender, System.Windows.Forms.ToolStripItemClickedEventArgs e)
        {
            e.ClickedItem.ToString();

            Workspace ws = e.ClickedItem.Tag as Workspace; toolStripDropDownSelectWorkspace.Text = ws.Name;
            //AFTER SELECTING A WORKSPACE FROM DROPDOWN
            Variables.Instance.CurrentWorkspaceTypeId = ws.WorkspaceTypeId;
            Variables.Instance.CurrentWorkspaceName = ws.Name;
            DockingForm.DockForm.SetToolstripToGlobalVarWorkspace(null);
            toolStripDropDownSelectWorkspace.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            if (ws.WorkspaceTypeId == 3)
                toolStripDropDownSelectWorkspace.Image = imageListWorkspaces.Images[0];
            else
                toolStripDropDownSelectWorkspace.Image = imageListWorkspaces.Images[1];
        }

        //private void setVariablesForServerVersion()
        //{
        //    if (Variables.Instance.IsServer)
        //    {
        //        Variables.Instance.ValidateVersionControl = true;
        //    }
        //}

        private void dockedForm_DockStateChanged(object sender, EventArgs e)
        {
            DockContent content = sender as DockContent;
            if (content.DockState == DockState.Hidden)
            {
            }
            ToggleMenuItemsForCurrentAppState();
        }

        private Collection<DiagramSaver> currentlySaving;
        public Collection<DiagramSaver> CurrentlySaving
        {
            get
            {
                if (currentlySaving == null)
                    currentlySaving = new Collection<DiagramSaver>();
                return currentlySaving;
            }
            set
            {
                currentlySaving = value;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (CurrentlySaving.Count > 0)
            {
                if (MessageBox.Show(this, "There " + (CurrentlySaving.Count > 1 ? "are" : "is") + " currently " + CurrentlySaving.Count + " diagram" + (CurrentlySaving.Count > 1 ? "s" : "") + " being saved. If you close Metabuilder now data will be lost." + Environment.NewLine + "Are you sure you want to close?", "Save in progress", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    e.Cancel = true;
                    //return;
                }
                else
                {
                    ForceCloseCancel = true;
                    //return;
                }
            }
            else
            {
                CloseAllDocuments();
                if (dockPanel1.DocumentsCount > 0)
                {
                    e.Cancel = true;
                    base.OnClosing(e);
                    return;
                }
                if (CurrentlySaving.Count > 0)
                {
                    if (MessageBox.Show(this, "There " + (CurrentlySaving.Count > 1 ? "are" : "is") + " currently " + CurrentlySaving.Count + " diagram" + (CurrentlySaving.Count > 1 ? "s" : "") + " being saved. If you close Metabuilder now data will be lost." + Environment.NewLine + "Are you sure you want to close?", "Save in progress", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        e.Cancel = true;
                        //return;
                    }
                    else
                    {
                        ForceCloseCancel = true;
                        //return;
                    }
                }
            }
#if DEBUG
            if (!e.Cancel)
            {
                //set Close to true in process
                //MetaBuilder.Saver.Program.Parameter("CLOSE");
            }
#endif
            base.OnClosing(e);
        }

        public void CloseAllDocuments()
        {
            if (dockPanel1 != null)
            {
                if (dockPanel1.DocumentStyle == DocumentStyle.SystemMdi)
                {
                    for (int i = 0; i < MdiChildren.Length; i++)
                    {
                        MdiChildren[i].Close();
                    }
                }
                else
                {
                    Collection<IDockContent> documentcontent = new Collection<IDockContent>();
                    foreach (IDockContent content in dockPanel1.Documents)
                        documentcontent.Add(content);

                    for (int i = 0; i < documentcontent.Count; i++)
                    {
                        documentcontent[i].DockHandler.Close();
                    }

                    documentcontent = null;
                }
            }
        }

        public IDockContent GetContentAt(int index)
        {
            List<IDockContent> documentcontent = new List<IDockContent>();
            int counter = 0;
            foreach (IDockContent content in dockPanel1.Documents)
            {
                if (counter == index)
                {
                    return content;
                }
            }
            return null;
        }

        public TreeLoader GetTreeLoader()
        {
            return m_treeLoader;
        }

        public TaskDocker GetTaskDocker()
        {
            return m_taskDocker;
        }

        public void CloseValidationWindow(GoDocument doc)
        {
            Collection<Tools.DataModel.UI.ValidationResultForm> vForms = DockForm.GetValidationDockers();
            Collection<Form> toClose = new Collection<Form>();
            if (vForms.Count > 0)
            {
                foreach (Tools.DataModel.UI.ValidationResultForm vform in vForms)
                {
                    if (vform.MyEngine != null)
                        if (vform.MyEngine.Diagram == doc)
                        {
                            toClose.Add(vform);
                        }
                }

                for (int i = 0; i < toClose.Count; i++)
                {
                    toClose[i].Close();
                }
            }

        }

        public Collection<Tools.DataModel.UI.ValidationResultForm> GetValidationDockers()
        {
            Collection<Tools.DataModel.UI.ValidationResultForm> retval = new Collection<MetaBuilder.UIControls.GraphingUI.Tools.DataModel.UI.ValidationResultForm>();
            foreach (DockPane p in this.dockPanel1.Panes)
            {
                if (p.Appearance == DockPane.AppearanceStyle.ToolWindow)
                {
                    foreach (DockContent content in p.Contents)
                    {
                        if (content is Tools.DataModel.UI.ValidationResultForm)
                        {
                            retval.Add(content as Tools.DataModel.UI.ValidationResultForm);
                        }
                    }
                }
                //Console.WriteLine(p.ToString());

            }
            return retval;
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(PaletteDocker).ToString())
            {
                m_paletteDocker.Text = "Stencils";
                return m_paletteDocker;
            }
            //else if (persistString == typeof(PropertyGridDocker).ToString())
            //return m_propertyGridWindow;
            else if (persistString == typeof(MetaPropertyGridDocker).ToString())
                return m_metaPropsWindow;
            else if (persistString == typeof(TaskDocker).ToString())
                return m_taskDocker;
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

        private void CloseAllContents()
        {
            // we don't want to create another instance of tool window, set DockPanel to null
            m_paletteDocker.DockPanel = null;
            m_metaPropsWindow.DockPanel = null;
            //m_propertyGridWindow.DockPanel = null;
            m_metaObjectExplorer.DockPanel = null;
            //m_panningWindow.DockPanel = null;
            m_taskDocker.DockPanel = null;
            m_treeLoader.DockPanel = null;
            // Close all other document windows
            // CancelEventArgs args = new CancelEventArgs();
            CloseAllDocuments();
        }

        private AutoResetEvent autoWait = new AutoResetEvent(false);
        public void SaveGraphViewContainerInnards(GraphViewContainer graphViewContainerBeingSaved, bool RequireWait)
        {
            graphViewContainerBeingSaved.Focus();
            // redelik seker hierdie een moet bly
            autoWait = new AutoResetEvent(false);
            graphViewContainerBeingSaved.IsSavingFromMDIParent = true;
            graphViewContainerBeingSaved.SaveComplete += new EventHandler(graphViewContainerBeingSaved_SaveComplete);

            NormalDiagram ndiagram = graphViewContainerBeingSaved.ViewController.GetDiagram();
            if (ndiagram != null && (!graphViewContainerBeingSaved.IsSavingFromItself))
            {
                if (ndiagram.Name != null)
                {
                    if (ndiagram.IsModified)
                    {
                        DialogResult res = MessageBox.Show(this, "The " + ndiagram.FileType.ToString() + " has changed. Do you want to save changes?", "Save changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                        if (res == DialogResult.Yes)
                            graphViewContainerBeingSaved.StartSaveProcess(false);
                        else
                        {
                            //return;
                        }
                    }
                }
                else
                {
                    graphViewContainerBeingSaved.StartSaveProcess(false);
                }
            }
            else
            {
                graphViewContainerBeingSaved.StartSaveProcess(false);
            }
            //m_metaObjectExplorer.Reset();
        }

        private void graphViewContainerBeingSaved_SaveComplete(object sender, EventArgs e)
        {
            Form f = sender as Form;
            ResetStatus();
            if (f.DialogResult != DialogResult.Cancel)
                f.Close();// autoWait.Set();

            Update();
            (sender as GraphViewContainer).SaveComplete -= graphViewContainerBeingSaved_SaveComplete;
        }

        public void CloseAllButThisOne(object sender, EventArgs e)
        {
            if (dockPanel1 != null)
            {
                if (dockPanel1.DocumentStyle == DocumentStyle.SystemMdi)
                {
                    for (int i = 0; i < MdiChildren.Length; i++)
                    {
                        if (MdiChildren[i] != GetCurrentGraphViewContainer())
                            MdiChildren[i].Close();
                    }
                }
                else
                {
                    Collection<IDockContent> documentcontent = new Collection<IDockContent>();
                    foreach (IDockContent content in dockPanel1.Documents)
                        documentcontent.Add(content);

                    for (int i = 0; i < documentcontent.Count; i++)
                    {
                        if (documentcontent[i] != GetCurrentGraphViewContainer())
                            documentcontent[i].DockHandler.Close();
                    }

                    documentcontent = null;
                }
            }
        }

        private void DockingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (progressBar1.Tag is DiagramSaver)
            //{
            //    if ((progressBar1.Tag as DiagramSaver).IsBusy)
            //    {
            //        //        MessageBox.Show("Metabuilder cannot be closed while a save operation is in progress", "Save in progress", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //        //        e.Cancel = true;
            //        //        return;
            //    }
            //}
            //if (!IsViewer)
            //{
            try
            {
#if DEBUG
                return;
#endif
                string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
                dockPanel1.SaveAsXml(configFile);
            }
            catch
            {
            }
            //}
        }

        private void DockingForm_MdiChildActivate(object sender, EventArgs e)
        {
            #region Workaround for the disappearance of automerging toolstrips in VS.NET 2005 and .NET v2

            ToolStripManager.RevertMerge(toolStripMain);
            if (ActiveMdiChild != null && ActiveMdiChild is GraphViewContainer)
            {
                ToolStripManager.Merge(((GraphViewContainer)ActiveMdiChild).toolStripMain, toolStripMain);
                //MakePanningWindowObserveActiveMDI();
            }

            #endregion

            if (!Core.Variables.Instance.IsViewer)
            {
                ShowMetaObjectProperties(null);
                if (GetCurrentGraphView() != null)
                {
                    //Log.WriteLog("DockingForm_MdiChildActivate::" + GetCurrentGraphView().Doc.Name);
                    try
                    {
                        foreach (DiagramSaver saver in CurrentlySaving)
                        {
                            if (saver.FileName == GetCurrentGraphView().Doc.Name)
                            {
                                UpdateStatusLabel(saver.Message);
                                ProgressUpdate(saver.Amount);
                                progressBar1.Tag = saver;
                                break;
                            }
                            else
                            {
                                UpdateStatusLabel(saver.Message);
                                ProgressUpdate(saver.Amount);
                                progressBar1.Tag = saver;
                            }
                        }
                    }
                    catch
                    {
                        //this is where cross thread calls when a saver is removed during iteration after is completion come to die
                    }
                    FindAndReplaceText.Reset();
                    DockForm.GetTaskDocker().BindToList(GetCurrentGraphViewContainer().ContainerID.ToString());
                    GetCurrentGraphViewContainer().SetupDiagramType();
                    GetCurrentGraphViewContainer().CustomModified = GetCurrentGraphViewContainer().CustomModified; //so changed and unchanged indicators reset
                    bool foundOne = false;
                    Collection<Tools.DataModel.UI.ValidationResultForm> vForms = DockForm.GetValidationDockers();
                    if (vForms.Count > 0)
                    {
                        foreach (Tools.DataModel.UI.ValidationResultForm vform in vForms)
                        {
                            if (vform.MyEngine.Diagram == GetCurrentGraphView().Doc)
                            {
                                foundOne = true;
                                vform.Activate();
                            }
                        }
                    }
                    if (!foundOne)
                    {
                        foreach (Tools.DataModel.UI.ValidationResultForm vform in vForms)
                        {
                            vform.Hide();
                        }
                    }
                }
                else
                {
                    //if (sender != null)
                    //    Log.WriteLog("DockingForm_MdiChildActivate::sender::" + sender.ToString());

                    DockForm.GetTaskDocker().ClearTasks();
                    FindAndReplaceText.Hide();
                }
            }
            // SetWorkspaceName(Core.Variables.Instance.CurrentWorkspaceName, GetWorkspaceTypeName(Core.Variables.Instance.CurrentWorkspaceTypeId));  
            Update();
        }

        #endregion

        #region File MenuItems

        private void menuItemFileNewDiagram_Click(object sender, EventArgs e)
        {
            NewDiagram();
        }

        public void SaveAll()
        {
            foreach (IDockContent content in dockPanel1.Documents)
                if (content is GraphViewContainer)
                    (content as GraphViewContainer).StartSaveProcess(false);
        }
        private void stripButtonSaveAll_Click(object sender, EventArgs e)
        {
            SaveAll();
        }

        public GraphViewContainer NewDiagram()
        {
            if (clearing)
                return null;
            //GetTaskDocker().ClearTasks();
            GraphViewContainer gvContainer = new GraphViewContainer(FileTypeList.Diagram);
            gvContainer.Show(dockPanel1);
            UpdateMenuItems();

            gvContainer.View.Document.FinishTransaction("New Document");

            gvContainer.View.Document.FinishTransaction("Making sure");

            gvContainer.View.Document.FinishTransaction("Really sure");

            gvContainer.View.Document.UndoManager.Clear();
            return gvContainer;
        }

        private void menuItemFileNewMindMapDiagram_Click(object sender, EventArgs e)
        {
            Dialogs.MindMap.MindmapForm mmForm = new MetaBuilder.UIControls.Dialogs.MindMap.MindmapForm();
            mmForm.ShowDialog(this);
        }

        private void menuItemFileNewSymbol_Click(object sender, EventArgs e)
        {
            GraphViewContainer gvContainer = new GraphViewContainer(FileTypeList.Symbol);
            gvContainer.Show(dockPanel1);
        }

        private void menuItemFileNewArrowhead_Click(object sender, EventArgs e)
        {
            GraphViewContainer gvContainer = new GraphViewContainer(FileTypeList.ArrowHead);
            gvContainer.Show(dockPanel1);
        }

        private void menuItemFileOpen_Click(object sender, EventArgs e)
        {
            if (clearing)
                return;
            string filter = "";

            FileDialogSpecification specs = FilePathManager.Instance.GetSpecification(FileTypeList.Diagram);
            filter = "Diagrams (*." + specs.Extension + ")|*." + specs.Extension + ";*.dgm";
            filter += "|Diagram without Object Refresh (*." + specs.Extension + ")|*." + specs.Extension;

            specs = FilePathManager.Instance.GetSpecification(FileTypeList.MindMap);
            filter += "|Meta Map (*." + specs.Extension + ")|*." + specs.Extension;

            specs = FilePathManager.Instance.GetSpecification(FileTypeList.Symbol);
            filter += "|Symbols (*." + specs.Extension + ")|*." + specs.Extension;

            specs = FilePathManager.Instance.GetSpecification(FileTypeList.SymbolStore);
            filter += "|SymbolStore (*." + specs.Extension + ")|*." + specs.Extension;

            specs = FilePathManager.Instance.GetSpecification(FileTypeList.SavedContextView);
            filter += "|Saved Context View (*." + specs.Extension + ")|*." + specs.Extension;

            OpenFileDialog ofdialog = new OpenFileDialog();
            ofdialog.SupportMultiDottedExtensions = true;
            ofdialog.Filter = filter;
            ofdialog.InitialDirectory = Core.Variables.Instance.DiagramPath;//.InitialDirectory = FilePathManager.Instance.GetLastUsedPath();
            ofdialog.Multiselect = true;

            DialogResult res = ofdialog.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                for (int i = 0; i < ofdialog.FileNames.Length; i++)
                {
                    //int filterindex = ofdialog.FilterIndex;
                    //bool skiprefresh = ofdialog.FilterIndex == 2 ? true : false;
                    //if (filterindex == 2)
                    //skiprefresh = true;

                    OpenFileInApplicableWindow(ofdialog.FileNames[i], ofdialog.FilterIndex == 2 ? true : false);
                }
            }
        }

        private void menuItemFileOpenFolder_Click(object sender, EventArgs e)
        {
            if (clearing)
                return;

            FolderBrowserDialog ofdialog = new FolderBrowserDialog();
            //ofdialog.RootFolder = Core.Variables.Instance.DiagramPath;//.InitialDirectory = FilePathManager.Instance.GetLastUsedPath();
            ofdialog.ShowNewFolderButton = false;
            DialogResult res = ofdialog.ShowDialog(this);
            if (res == DialogResult.OK)
                foreach (string f in Directory.GetFiles(ofdialog.SelectedPath))
                    OpenFileInApplicableWindow(f, false);
        }

        protected void menuItemExit_Click(object sender, EventArgs a)
        {
            Close();
            Application.Exit();
        }

        #endregion

        #region View MenuItems

        private void menuItemViewDocumentIcon_Click(object sender, EventArgs e)
        {
            // UITGEHAAL VIR BROES dockPanel1.ShowDocumentIcon = menuItemViewDocumentIcon.Checked = !menuItemViewDocumentIcon.Checked;
        }

        private void menuItemViewStencils_Click(object sender, EventArgs e)
        {
            bool Shown = !menuItemViewStencils.Checked;
            menuItemViewStencils.Checked = Shown;
            if (Shown)
            {
                m_paletteDocker.Show(dockPanel1, DockState.DockLeft);
            }
            else
                m_paletteDocker.DockPanel = null;
        }

        private void menuItemViewObjectExplorer_Click(object sender, EventArgs e)
        {
            bool Shown = !menuItemViewObjectExplorer.Checked;
            menuItemViewObjectExplorer.Checked = Shown;
            if (Shown)
                m_metaObjectExplorer.Show(dockPanel1, DockState.DockLeft);
            else
                m_metaObjectExplorer.DockPanel = null;
        }

        private void menuItemViewPropertiesMeta_Click(object sender, EventArgs e)
        {
            bool Shown = !menuItemViewPropertiesMeta.Checked;
            menuItemViewPropertiesMeta.Checked = Shown;
            if (Shown)
            {
                m_metaPropsWindow.Show(dockPanel1, DockState.DockRight);
            }
            else
                m_metaPropsWindow.DockPanel = null;
        }

        //private void menuItemViewPropertiesWindowDiagramming_Click(object sender, EventArgs e)
        //{
        //bool Shown = !menuItemViewPropertiesWindowDiagramming.Checked;
        //menuItemViewPropertiesWindowDiagramming.Checked = Shown;
        //if (Shown)
        //{
        //    m_propertyGridWindow.Show(dockPanel1, DockState.DockRight);
        //}
        //else
        //    m_propertyGridWindow.DockPanel = null;
        //}

        private void menuItemViewTaskList_Click(object sender, EventArgs e)
        {
            bool Shown = !menuItemViewTaskList.Checked;
            menuItemViewTaskList.Checked = Shown;
            if (Shown)
            {
                m_taskDocker.Show(dockPanel1, DockState.DockRight);
            }
            else
                m_taskDocker.DockPanel = null;
        }

        private void menuItemViewFullScreen_Click(object sender, EventArgs e)
        {
            // UITGEHAAL VIR BROES 
            /*
            bool IsFullScreen = !menuItemViewFullScreen.Checked;
            menuItemViewFullScreen.Checked = IsFullScreen;
            if (IsFullScreen)
            {
                this.BackColor = System.Drawing.Color.Black;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            }
            else
            {
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Control);
                this.FormBorderStyle = FormBorderStyle.Sizable;
            }*/
        }

        private void menuItemViewPanAndZoom_Click(object sender, EventArgs e)
        {
            bool Shown = !menuItemViewPanAndZoom.Checked;
            menuItemViewPanAndZoom.Checked = Shown;
            if (Shown)
            {
                //m_panningWindow.Show(dockPanel1, DockState.Float);
                //MakePanningWindowObserveActiveMDI();
            }
            //else
            //    m_panningWindow.DockPanel = null;
        }

        private void m_panningWindow_VisibleChanged(object sender, EventArgs e)
        {
            //menuItemViewPanAndZoom.Checked = m_panningWindow.Visible;
        }

        #endregion

        #region Tools MenuItems

        private void menuItemDatabaseImportExcelTemplatesImport_Click(object sender, EventArgs e)
        {
            OnStartExternalProcess("ImportExcelTemplate", e);
            Loader.FlushDataViews();
        }

        private void menuItemDatabaseImportExcelTemplatesGenerate_Click(object sender, EventArgs e)
        {
            string _class = GetAClass();
            if (_class.Length == 0)
                return;

            ExcelTemplate exctemplate = new ExcelTemplate();
            DialogResult res = MessageBox.Show(this, "Include Existing Data?", "Include Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            bool ok;
            if (res == DialogResult.Yes)
            {
                ok = exctemplate.ExportTemplate(_class, true);
            }
            else
            {
                ok = exctemplate.ExportTemplate(_class, false);
            }
            if (!ok) return;
            string filename = exctemplate.FileName;
            DialogResult resExplore = MessageBox.Show(this, "The file has been exported. Open the file?", "Export File", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (resExplore == DialogResult.Yes)
            {
                Launcher.OpenFile(filename);
            }
        }

        private void menuItemToolsOptions_Click(object sender, EventArgs e)
        {
            bool x = Variables.Instance.AutoSaveEnabled;
            Preferences pref = new Preferences();
            pref.ShowDialog(this);

            //TODO : rebuild cache because variables has been reset
            if (pref.DialogResult != DialogResult.Cancel)
            {
                buildShapeCacheThread();

                //setup autosave on open documents if the setting changed
                if (x != Variables.Instance.AutoSaveEnabled)
                    foreach (IDockContent content in dockPanel1.Documents)
                        if (content is GraphViewContainer)
                            (content as GraphViewContainer).SetupAutosave();
            }
        }

        private void removeHighlightsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveHighlights();
        }

        private void menuItemToolsModeDeveloper_CheckedChanged(object sender, System.EventArgs e)
        {
            if (menuItemToolsModeDeveloper.Checked)
                menuItemToolsModeClient.CheckState = CheckState.Unchecked;
            else
                menuItemToolsModeClient.CheckState = CheckState.Checked;
            Variables.Instance.ShowDeveloperItems = menuItemToolsModeDeveloper.Checked;
            UpdateMenuItems();
            m_paletteDocker.UpdateItems();
        }

        private void menuItemToolsModeClient_CheckedChanged(object sender, System.EventArgs e)
        {
            if (menuItemToolsModeClient.Checked)
                menuItemToolsModeDeveloper.CheckState = CheckState.Unchecked;
            else
                menuItemToolsModeDeveloper.CheckState = CheckState.Checked;
            Variables.Instance.ShowDeveloperItems = menuItemToolsModeDeveloper.Checked;
            UpdateMenuItems();
            m_paletteDocker.UpdateItems();
        }

        #endregion

        #region Help MenuItems

        private void menuItemHelpAbout_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.SizeGripStyle = SizeGripStyle.Hide;
            about.FormBorderStyle = FormBorderStyle.FixedDialog;
            about.ShowDialog(this);
            //MessageBox.Show(this,"Help documentation will be available soon. In the meantime, please refer to the QuickStart document.");
        }
        private void menuItemHelpShortcuts_Click(object sender, System.EventArgs e)
        {
            Hyperlink lnk = new Hyperlink();
            lnk.Arguments = Application.StartupPath + "\\" + "MetaBuilder Shortcut List.pdf";
            GeneralUtil.LaunchHyperlink(lnk);
        }
        private void menuItemHelpQuick_Click(object sender, System.EventArgs e)
        {
            Hyperlink lnk = new Hyperlink();
            lnk.Arguments = Application.StartupPath + "\\" + "Metabuilder Quick Guide.pdf";
            GeneralUtil.LaunchHyperlink(lnk);
        }

        #endregion

        #region Database MenuItems

        public void menuItemToolsLoadFromDBRefreshObjects_Click(object sender, EventArgs e)
        {
            if (GetCurrentGraphViewContainer() != null)
                GetCurrentGraphViewContainer().menuItemToolsLoadFromDBRefreshObjects_Click(sender, e);
        }
        public void menuItemToolsLoadFromDatabaseSelectObjects_Click(object sender, EventArgs e)
        {
            if (GetCurrentGraphViewContainer() == null)
                NewDiagram();
            GetCurrentGraphViewContainer().menuItemToolsLoadFromDatabaseSelectObjects_Click(sender, e);
        }
        public void menuItemToolsLoadFromDatabaseSelectDiagram_Click(object sender, EventArgs e)
        {
            if (GetCurrentGraphViewContainer() != null)
                GetCurrentGraphViewContainer().menuItemToolsLoadFromDatabaseSelectDiagram_Click(sender, e);
        }
        public void menuItemToolsLoadFromDBChangedObjectsAddIndicators_Click(object sender, EventArgs e)
        {
            if (GetCurrentGraphViewContainer() != null)
                GetCurrentGraphViewContainer().menuItemToolsLoadFromDBChangedObjectsAddIndicators_Click(sender, e);
        }
        public void menuItemToolsLoadFromDBChangedObjectsRemoveIndicators_Click(object sender, EventArgs e)
        {
            if (GetCurrentGraphViewContainer() != null)
                GetCurrentGraphViewContainer().menuItemToolsLoadFromDBChangedObjectsRemoveIndicators_Click(sender, e);
        }
        public void menuItemToolsLoadFromDatabaseArtefactObject_Click(object sender, EventArgs e)
        {
            if (GetCurrentGraphViewContainer() != null)
                GetCurrentGraphViewContainer().menuItemToolsLoadFromDatabaseArtefactObject_Click(sender, e);
        }
        public void menuItemToolsRefreshWholePrompt_Click(object sender, EventArgs e)
        {
            if (GetCurrentGraphViewContainer() != null)
                GetCurrentGraphViewContainer().menuItemToolsRefreshWholePrompt_Click(sender, e);
        }
        public void menuItemToolsRefreshWholeAuto_Click(object sender, EventArgs e)
        {
            if (GetCurrentGraphViewContainer() != null)
                GetCurrentGraphViewContainer().menuItemToolsRefreshWholeAuto_Click(sender, e);
        }
        public void menuItemToolsRefreshSelectionPrompt_Click(object sender, EventArgs e)
        {
            if (GetCurrentGraphViewContainer() != null)
                GetCurrentGraphViewContainer().menuItemToolsRefreshSelectionPrompt_Click(sender, e);
        }
        public void menuItemToolsRefreshSelectionAuto_Click(object sender, EventArgs e)
        {
            if (GetCurrentGraphViewContainer() != null)
                GetCurrentGraphViewContainer().menuItemToolsRefreshSelectionAuto_Click(sender, e);
        }

        private void menuItemDBMapper_Click(object sender, EventArgs e)
        {
            Dialogs.RelationshipManager.RelmanContainer rlc = new RelmanContainer();
            rlc.ShowDialog(this);
            //relationshipManager.ShowDialog(this);
            //Application.Run(relationshipManager);
            // OnStartExternalProcess("Mapper", e);
        }
        private void menuItemDBDictionary_Click(object sender, EventArgs e)
        {
            OnStartExternalProcess("Dictionary", e);
        }

        #endregion

        private bool dontShowPane;
        public bool DontShowPane
        {
            get { return dontShowPane; }
            set { dontShowPane = value; }
        }

        public void ShowMetaObjectProperties(MetaBase m)
        {
            if (m == null)
            {

                m_metaPropsWindow.SelectedObject = null;
                m_metaPropsWindow.Invalidate();
                return;
            }
            else
            {
                if (m_metaPropsWindow.SelectedObject != m)
                {
                    m_metaPropsWindow.SelectedObject = m;
                    if (m != null)
                        m_metaPropsWindow.Enabled = VCStatusTool.UserHasControl(m);
                }

                Collection<MetaBase> SelectedMetaBases = new Collection<MetaBase>();
                if (GetCurrentGraphViewContainer() != null)
                {
                    bool cancel = false;
                    foreach (GoObject o in GetCurrentGraphViewContainer().View.Selection)
                    {
                        if (o is IMetaNode)
                        {
                            continue;
                        }
                        else
                        {
                            cancel = true;
                            break;
                        }
                    }

                    if (cancel)
                        return;

                    foreach (IMetaNode iNode in GetCurrentGraphViewContainer().ViewController.GetIMetaNodes(GetCurrentGraphViewContainer().View.Selection))
                    {
                        SelectedMetaBases.Add(iNode.MetaObject);
                    }
                    if (SelectedMetaBases.Count > 1)
                        m_metaPropsWindow.SelectedMetaBases = SelectedMetaBases;
                    else
                        m_metaPropsWindow.SelectedMetaBases = null;
                }
            }
            if (DontShowPane)
                return;

            if (!IsAutoHide(m_metaPropsWindow))
            {
                m_metaPropsWindow.BringToFront();
                m_metaPropsWindow.Activate();
                //select first property
            }
        }
        public void ShowMetaObjectProperties(MetaBase m, bool enabled)
        {
            if (m == null)
            {
                m_metaPropsWindow.SelectedObject = null;
                m_metaPropsWindow.Invalidate();
                return;
            }
            else
            {
                if (m_metaPropsWindow.SelectedObject != m)
                {
                    m_metaPropsWindow.SelectedObject = m;
                    if (m != null)
                        m_metaPropsWindow.Enabled = enabled;
                }

                if (enabled)
                {
                    Collection<MetaBase> SelectedMetaBases = new Collection<MetaBase>();
                    if (GetCurrentGraphViewContainer() != null)
                    {
                        bool cancel = false;
                        foreach (GoObject o in GetCurrentGraphViewContainer().View.Selection)
                        {
                            if (o is IMetaNode)
                            {
                                continue;
                            }
                            else
                            {
                                cancel = true;
                                break;
                            }
                        }

                        if (cancel)
                            return;

                        foreach (IMetaNode iNode in GetCurrentGraphViewContainer().ViewController.GetIMetaNodes(GetCurrentGraphViewContainer().View.Selection))
                        {
                            SelectedMetaBases.Add(iNode.MetaObject);
                        }

                        if (SelectedMetaBases.Count > 1)
                            m_metaPropsWindow.SelectedMetaBases = SelectedMetaBases;
                        else
                            m_metaPropsWindow.SelectedMetaBases = null;
                    }
                }
            }
            if (!IsAutoHide(m_metaPropsWindow))
            {
                m_metaPropsWindow.BringToFront();
                m_metaPropsWindow.Activate();
            }
        }
        public void ShowMetaObjectProperties(object m)
        {
            if (m == null)
            {
                m_metaPropsWindow.SelectedObject = null;
                m_metaPropsWindow.Invalidate();
                return;
            }
            else
            {
                if (m_metaPropsWindow.SelectedObject != m)
                {
                    m_metaPropsWindow.SelectedObject = m;
                }
            }
            if (!IsAutoHide(m_metaPropsWindow))
            {
                m_metaPropsWindow.BringToFront();
                m_metaPropsWindow.Activate();
                //select first property

            }
        }

        public void ShowDiagramProperties()
        {
            if (GetCurrentGraphViewContainer() != null && GetCurrentGraphViewContainer().ReadOnly == false)
            {
                DocumentInfo info = (GetCurrentGraphViewContainer().View.Document as NormalDiagram).DocumentFrame.GetDocumentInfo();
                info.group = (GetCurrentGraphViewContainer().View.Document as NormalDiagram).DocumentFrame;
                m_metaPropsWindow.SelectedObject = info;
                m_metaPropsWindow.Enabled = true;

                if (!IsAutoHide(m_metaPropsWindow))
                {
                    m_metaPropsWindow.BringToFront();
                    //m_metaPropsWindow.Activate();
                }
            }
        }

        public void ShowTasks()
        {
            if (!IsAutoHide(m_taskDocker))
            {
                m_taskDocker.BringToFront();
                m_taskDocker.Activate();
            }
        }

        private bool IsAutoHide(DockContent content)
        {
            return content.DockState == DockState.DockBottomAutoHide || content.DockState == DockState.DockLeftAutoHide || content.DockState == DockState.DockRightAutoHide || content.DockState == DockState.DockTopAutoHide || content.DockState == DockState.Hidden;
        }

        #region ToDo - Stuff that doesnt work properly

        public void ApplyNewSettings()
        {
            foreach (Form form in MdiChildren)
                if (form is GraphViewContainer)
                    ((GraphViewContainer)form).ViewController.SetupGrid();
        }

        #endregion

        #region File Handling

        /// <summary>
        /// Iterates collection/array and opens the files in the correct windows
        /// </summary>
        /// <param name="filenames"></param>
        public void OpenMultipleFiles(string[] filenames)
        {
            for (int i = 0; i < filenames.Length; i++)
            {
                if (filenames[i].Contains("."))
                {
                    OpenFileInApplicableWindow(filenames[i], false);
                }
                else if (filenames[i].ToLower().Contains("dictionary"))
                {
                    //Skip these arguments
                }
                else
                {
                    //diagram type?!@?!
                    CreateDiagram(filenames[i]);
                }
            }
        }

        /// <summary>
        /// Examines a filename and based on that, opens it in the correct window
        /// </summary>
        /// <param name="filename"></param>
        public void OpenFileInApplicableWindow(string filename, bool skiprefresh)
        {
            if (clearing)
                return;
            if (!(System.IO.File.Exists(filename)))
            {
                MessageBox.Show(this, "The file you have selected (" + Environment.NewLine + filename + Environment.NewLine + ") has moved or no longer exists.", "Unable to find this file.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //if (MessageBox.Show(this,"The file you have selected no longer exists" + Environment.NewLine + "Would you like to attempt to load the latest version of it from the database?", MessageBoxButtons.YesNo, MessageBoxIcon.Question).DialogResult == DialogResult.Yes)
                //{

                //}
                //else
                DockFormController.mruManager.Remove(filename);
                return;
            }

            string extension = strings.GetFileExtension(filename);
            // Use diagram as default;
            FileDialogSpecification applicableSpec = FilePathManager.Instance.GetSpecification(FileTypeList.Other);
            Collection<FileDialogSpecification> specs = FilePathManager.Instance.GetList();
            foreach (FileDialogSpecification spec in specs)
            {
                if (spec.Extension == extension)
                {
                    applicableSpec = spec;
                    break;
                }
            }
            if (Variables.Instance.UserDebug)
                Log.WriteLog("USER DEBUG : " + Environment.NewLine + "OpenFileInApplicableWindow - " + filename + Environment.NewLine + applicableSpec.FileType.ToString());
            switch (applicableSpec.FileType)
            {
                case FileTypeList.MindMap:
                    try
                    {
                        Dialogs.MindMap.MindmapForm mmForm = new MetaBuilder.UIControls.Dialogs.MindMap.MindmapForm(filename);
                        mmForm.ShowDialog(this);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "A problem has occurred while loading the Meta Map. Please contact technical support for assistance.", "Meta Map cannot be loaded", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Log.WriteLog("Openfileinapplicablewindow : Meta Map " + Environment.NewLine + ex.ToString());
                    }
                    break;
                case FileTypeList.SavedContextView:
                    bool contextFound = false;
                    foreach (DockContent dcont in dockPanel1.Documents)
                    {
                        if (dcont is LiteGraphViewContainer)
                        {
                            LiteGraphViewContainer lcontainer = dcont as LiteGraphViewContainer;
                            if (lcontainer.TabText.ToLower() == strings.GetFileNameWithoutExtension(filename).ToLower())
                            {
                                lcontainer.Focus();
                                contextFound = true;
                                break;
                            }
                        }
                    }
                    if (!contextFound)
                    {
                        LiteGraphViewContainer gvContainercv = new LiteGraphViewContainer();
                        gvContainercv.LoadView(filename);
                        gvContainercv.Show(dockPanel1);
                    }
                    break;
                case FileTypeList.SymbolStore:
                    m_paletteDocker.LoadDockedPaletteForFile(applicableSpec, filename);
                    break;
                case FileTypeList.Diagram:
                    opening = true;
                    bool found = false;
                    GraphViewContainer gvContainer = null;
                    foreach (DockContent dcont in dockPanel1.Documents)
                    {
                        gvContainer = dcont as GraphViewContainer;
                        if (gvContainer == null)
                            continue;
                        if (gvContainer.View.Document.Name.ToLower().Trim() == filename.Replace("Autosave ", "").ToLower().Trim())
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        gvContainer = new GraphViewContainer(applicableSpec.FileType);
                        gvContainer.SkipRefresh = skiprefresh;
                        //gvContainer.View.BeginUpdate();
                        //if (Variables.Instance.UserDebug)
                        //    Log.WriteLog("USER DEBUG : " + Environment.NewLine + "calling gvContainer.LoadFile");
                        gvContainer.LoadFile(filename);
                        DockFormController.mruManager.Add(filename);

                        //gvContainer.View.EndUpdate();
                        //gvContainer.Show(dockPanel1);
                        //gvContainer.FinaliseDocumentAfterLoading(skiprefresh);

                        //if (!OpenedFiles.ContainsKey((gvContainer.ViewController.MyView.Document as NormalDiagram).Name.ToLower()))
                        //{
                        //    OpenedFiles.Add((gvContainer.ViewController.MyView.Document as NormalDiagram).Name.ToLower(), gvContainer.ContainerID);
                        //}
                    }
                    else
                    {
                        DisplayTip("File is already open", "Open file", ToolTipIcon.Info);
                        gvContainer.Focus();
                    }
                    break;
                default:
                    Log.WriteLog(filename + " cannot be opened. Extension [" + extension + "] not supported");
                    DisplayTip(filename + Environment.NewLine + "Extension [" + extension + "] not supported.", "Unable to Open File", ToolTipIcon.Warning);
                    break;
            }
            //Update();
            //Activate();
            //Focus();
            //BringToFront();
        }
        public bool opening = false;
        public DockPanel DiagramDockPanel
        {
            get { return dockPanel1; }
        }

        public void SetToolstripWorkspaceToDiagram(DocumentVersion version)
        {
            SetWorkspaceName(version.WorkspaceName, GetWorkspaceTypeName(version.WorkspaceTypeId), true);
        }
        public void RemoveLockedFile(string filename)
        {
            openedFiles.Remove(filename);
        }

        private Dictionary<string, Guid> openedFiles;
        public Dictionary<string, Guid> OpenedFiles
        {
            get
            {
                if (openedFiles == null)
                    openedFiles = new Dictionary<string, Guid>();
                return openedFiles;
            }
            set { openedFiles = value; }
        }

        #endregion

        #region Diagram Type Handling

        public void CreateDiagram(string type)
        {
            NewDiagram();
            GraphViewContainer gvContainer = GetCurrentGraphViewContainer();
            if (gvContainer == null)
                return;
            NormalDiagram gvContainerDiagram = (gvContainer.View.Document as NormalDiagram);
            gvContainerDiagram.DiagramType = type;
            gvContainer.SetupDiagramType();
        }

        #endregion

        #region External Tools

        private void mnuDatabaseDictionary_Click(object sender, EventArgs e)
        {
            OnStartExternalProcess("Dictionary", e);
        }

        [NonSerialized]
        public EventHandler StartExternalProcess;
        public virtual void OnStartExternalProcess(object sender, EventArgs e)
        {
            if (StartExternalProcess != null)
            {
                StartExternalProcess(sender, e);
            }
        }

        #endregion

        #region Helpers - easy access to graphviews and stencils

        public GraphView GetCurrentGraphView()
        {
            if (GetCurrentGraphViewContainer() != null)
                return GetCurrentGraphViewContainer().View;
            return null;
        }

        public GraphViewContainer GetCurrentGraphViewContainer()
        {
            if (ActiveMdiChild != null && ActiveMdiChild is GraphViewContainer)
                return ((GraphViewContainer)ActiveMdiChild);
            return null;
        }

        public GraphPalette GetCurrentSymbolStore()
        {
            return m_paletteDocker.GetCurrentPalette();
        }

        public DockPanel GetPaletteContainer()
        {
            return m_paletteDocker.dockPanel1;
        }

        #endregion

        #region Imports & Exports

        private void menuItemDatabaseImportModulesFromTextAndExcelWithMappingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // SAPPI Specific
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select the indented SD File";
            ofd.Filter = "Text Files|*.txt";
            ofd.Multiselect = false;
            DialogResult resultText = ofd.ShowDialog(this);
            if (resultText == DialogResult.OK)
            {
                string textFile = ofd.FileName;
                ofd.Title = "Select the Excel File";
                ofd.Filter = "Excel Files|*.xls";
                ofd.Multiselect = false;
                DialogResult excelResult = ofd.ShowDialog(this);
                if (excelResult == DialogResult.OK)
                {
                    string excelFile = ofd.FileName;
                    AppComponentModuleImportMapper appComponentMapper = new AppComponentModuleImportMapper();
                    appComponentMapper.ExcelFile = excelFile;
                    appComponentMapper.TextFile = textFile;
                    appComponentMapper.Finished += new EventHandler(appComponentMapper_Finished);
                    Thread t = new Thread(new ThreadStart(appComponentMapper.Import));
                    t.IsBackground = true;
                    t.TrySetApartmentState(ApartmentState.STA);
                    t.Start();
                    //MessageBox.Show(this,"The Import is currently taking place. Please allow a few minutes for this operation to complete. Do not close the application. You will be notified when the operation is complete.", "Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void TextFile_Report(object sender, EventArgs e)
        {
#if DEBUG
            Console.WriteLine(sender.ToString());
#endif
            DisplayTip(sender.ToString(), "Textual Structure Import");
        }
        private void Import_Finished(object sender, EventArgs e)
        {
            TextFile textImporter = sender as TextFile;
            if (textImporter.errorOccurred)
            {
                //string file = Variables.Instance.ExportsPath + "\\HierarchicalImport_Report.txt";
                //TextWriter twriter = new StreamWriter(file);
                //twriter.WriteLine(textImporter.sReport.ToString());
                //twriter.Close();
                DialogResult resExplore;
                if (textImporter.error.Length == 0)
                {
                    resExplore = MessageBox.Show("The importer encountered errors which have been logged in the trace.log file that can be found in your metabuilder folder.", "Error Report", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //if (resExplore == DialogResult.Yes)
                    //{
                    //    Launcher.OpenFile(file);
                    //}
                }
                else
                {
                    if (textImporter.error == "Found Lines Ending With Tabs")
                    {
                        if (MessageBox.Show("The importer encountered errors. Some of the lines which were to be imported have whitespace(ie : Tab(s), Space(s)) at the end of each line which is not allowed." + Environment.NewLine + "Using this regular expression notation ([ \f\t\v]+$) you can do a find and replace all of these lines with an advanced text editor like SublimeText in order to easily remove these problem lines.", "Error Report", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                        {
                        }
                    }
                    else
                    {
                        Log.WriteLog(textImporter.error);
                    }
                }
            }
            else
            {
                if (textImporter.FirstObject == null)
                {
                    MessageBox.Show("The import has reached the end of the file but was unable to import any objects. Please make sure the file is in the correct format.", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    GraphNode n = GetShape(textImporter.FirstObject.Class);
                    if (n != null)
                    {
                        n.MetaObject = textImporter.FirstObject;
                        n.HookupEvents();
                        n.BindToMetaObjectProperties();
                        ShallowGoObjects = new Collection<GoObject>();
                        ShallowGoObjects.Add(n);
                        MessageBox.Show("The import has finished successfully. The root node has been added to the clibboard and can be pasted on any diagram.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("The import has finished successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void appComponentMapper_Finished(object sender, EventArgs e)
        {
            AppComponentModuleImportMapper appComponentMapper = sender as AppComponentModuleImportMapper;
            if (appComponentMapper.sReport.Length > 0)
            {
                string file = Variables.Instance.ExportsPath + "\\AppComponent_Mapping_Report.txt";
                TextWriter twriter = new StreamWriter(file);
                twriter.WriteLine(appComponentMapper.sReport.ToString());
                twriter.Close();
                DialogResult resExplore = MessageBox.Show("The Importer encountered errors. Open " + strings.GetFileNameOnly(file) + " for details?", "Error Report", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (resExplore == DialogResult.Yes)
                {
                    Launcher.OpenFile(file);
                }
            }
            else
            {
                MessageBox.Show(this, "The import was finished successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void menuItemDatabaseImportHierarchicalData_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select the indented Text File";
            ofd.Filter = "Text Files|*.txt";
            ofd.Multiselect = false;
            DialogResult resultText = ofd.ShowDialog(this);
            if (resultText == DialogResult.OK)
            {
                string textFile = ofd.FileName;
                QueryForTextImport query = new QueryForTextImport();
                DialogResult res = query.ShowDialog(this);
                if (res == DialogResult.OK)
                {
                    TextFile txtFileImporter = new TextFile();
                    txtFileImporter.Specification = new TextImportSpecification();
                    txtFileImporter.Specification.ClassName = query.MyClass;
                    txtFileImporter.Specification.DefaultField = query.MyField;
                    txtFileImporter.Specification.SourceFile = textFile;
                    Thread t = new Thread(new ThreadStart(txtFileImporter.Import));
                    t.IsBackground = true;
                    t.TrySetApartmentState(ApartmentState.STA);
                    txtFileImporter.Finished += new EventHandler(Import_Finished);
                    txtFileImporter.Report += new EventHandler(TextFile_Report);
                    t.Start();
                    //txtFileImporter.Import();
                    //MessageBox.Show(this,"The Import is currently taking place. Please allow a few minutes for this operation to complete. Do not close the application. You will be notified when the operation is complete.", "Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            Loader.FlushDataViews();
        }

        #endregion

        #region HighlightObjects

        public void HighlightConnectedObjects(MetaBuilder.Graphing.Controllers.NodeObjectContextArgs args)
        {
            List<Dialogs.HighlightedObject> highlightedObjects = new List<HighlightedObject>();
            Dictionary<string, MetaObjectKey> connectedObjects = GetConnectedObjects(args.MyNode.MetaObject);
            //int totalCount = 0;
            foreach (Form form in MdiChildren)
            {
                if (form is GraphViewContainer)
                {
                    GraphViewContainer gvc = form as GraphViewContainer;
                    //if (gvc.View.Document.Name != args.OriginalDocumentName)
                    {
                        Collection<Dialogs.HighlightedObject> objectsOnThisDocument = gvc.DoHighlights(connectedObjects);
                        foreach (Dialogs.HighlightedObject hlo in objectsOnThisDocument)
                        {
                            bool found = false;
                            foreach (Dialogs.HighlightedObject hloAlreadyAdded in highlightedObjects)
                            {
                                if (hloAlreadyAdded.MetaObject.pkid == hlo.MetaObject.pkid && hloAlreadyAdded.DocumentName == hlo.DocumentName)
                                {
                                    found = true;
                                }
                            }
                            if (!found)
                                highlightedObjects.Add(hlo);
                        }
                        //totalCount += objectsOnThisDocument.Count;
                    }
                }
            }
            Dialogs.MoveToNextHighlight highlightNav = new MoveToNextHighlight();
            highlightNav.Highlights = highlightedObjects;
            highlightNav.TopMost = true;
            highlightNav.Show();

            //MessageBox.Show(this,totalCount.ToString() + " related objects were highlighted");
        }
        private Collection<IMetaNode> AlreadyHighlighted = null;
        public void HighlightShallowCopies(MetaBuilder.Graphing.Controllers.NodeObjectContextArgs args)
        {
            AlreadyHighlighted = new Collection<IMetaNode>();
            MetaBase mBase = args.MyNode.MetaObject;
            if (mBase.pkid <= 0)
                return;
            List<HighlightedObject> highlightedObjects = new List<HighlightedObject>();
            foreach (Form form in MdiChildren)
            {
                if (form is GraphViewContainer)
                {
                    GraphViewContainer gvc = form as GraphViewContainer;
                    foreach (GoObject gObj in gvc.View.Document)
                    {
                        if (!(gObj is IMetaNode))
                            continue;

                        IMetaNode imn = gObj as IMetaNode;
                        //if (imn.MetaObject.pkid <= 0)
                        //    continue;

                        if (imn.MetaObject.pkid == mBase.pkid && imn.MetaObject.MachineName == mBase.MachineName)
                        {
                            if (!(AlreadyHighlighted.Contains(imn)))
                            {
                                gvc.ViewController.HighlightNode(imn as GoObject, true);
                                highlightedObjects.Add(new HighlightedObject(imn.MetaObject, gObj, gvc.View.Document.Name));
                                AlreadyHighlighted.Add(imn);
                            }
                        }

                        //nodes in ILinkedContainers
                        if (imn is IGoCollection)
                        {
                            highlightedObjects.AddRange(highlightShallowCopiesInCollection(gvc, imn as IGoCollection, mBase));
                        }
                        else if (imn is MetaBuilder.Graphing.Shapes.Nodes.Containers.ILinkedContainer)
                        {
                            Log.WriteLog("DockingForm::HighlightShallowCopies " + imn.ToString() + " is not IGoCollection");
                        }

                        ////nodes in collapsible nodes
                        //if (gObj is MetaBuilder.Graphing.Shapes.CollapsibleNode)
                        //{

                        //}
                    }
                }
            }

            if (highlightedObjects.Count > 1)
            {
                foreach (IMetaNode node in AlreadyHighlighted)
                {
                    (node as GoObject).Shadowed = true;
                }
                MoveToNextHighlight highlightNav = new MoveToNextHighlight();
                highlightNav.Highlights = highlightedObjects;
                highlightNav.TopMost = true;
                highlightNav.Show();
            }
            else
            {
                RemoveHighlights();
            }
        }
        private List<HighlightedObject> highlightShallowCopiesInCollection(GraphViewContainer gvc, IGoCollection col, MetaBase mBase)
        {
            List<HighlightedObject> ret = new List<HighlightedObject>();

            foreach (GoObject obj in gvc.ViewController.GetNestedObjects(col))
            {
                if (obj is IMetaNode)
                {
                    IMetaNode imn = obj as IMetaNode;
                    if (imn.MetaObject != null)
                    {
                        if (imn.MetaObject.pkid == mBase.pkid && imn.MetaObject.MachineName == mBase.MachineName)
                        {
                            if (!(AlreadyHighlighted.Contains(imn)))
                            {
                                gvc.ViewController.HighlightNode(imn as GoObject, true);
                                ret.Add(new HighlightedObject(imn.MetaObject, imn as GoObject, gvc.View.Document.Name));
                                AlreadyHighlighted.Add(imn);
                            }
                        }
                    }
                }

                if (obj is IGoCollection)
                    ret.AddRange(highlightShallowCopiesInCollection(gvc, obj as IGoCollection, mBase));
            }

            return ret;
        }
        public void NavigateToHighlightedObject(Dialogs.HighlightedObject hlo)
        {
            foreach (Form form in MdiChildren)
            {
                if (form is GraphViewContainer)
                {
                    GraphViewContainer gvc = form as GraphViewContainer;
                    if (gvc.View.Document.Name == hlo.DocumentName)
                    {
                        gvc.View.ScrollRectangleToVisible(hlo.GoObj.Bounds);
                        gvc.Activate();
                    }
                }
            }
        }
        public void RemoveHighlights()
        {
            foreach (Form form in MdiChildren)
            {
                if (form is GraphViewContainer)
                {
                    GraphViewContainer gvc = form as GraphViewContainer;
                    gvc.ViewController.RemoveHighlights();
                }
            }
        }
        private Dictionary<string, MetaObjectKey> GetConnectedObjects(MetaBase mbase)
        {
            Dictionary<string, MetaObjectKey> connectedObjects = new Dictionary<string, MetaObjectKey>();
            if (mbase != null) //swimlanes are now imetanodes and can be null
            {
                b.TList<b.ObjectAssociation> associationsAsChild = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByChildObjectIDChildObjectMachine(mbase.pkid, mbase.MachineName);
                b.TList<b.ObjectAssociation> associationsAsParent = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(mbase.pkid, mbase.MachineName);
                foreach (b.ObjectAssociation oa in associationsAsChild)
                {
                    string key = oa.pkid.ToString() + "_" + oa.ObjectMachine;
                    if (!connectedObjects.ContainsKey(key))
                    {
                        MetaObjectKey moKey = new MetaObjectKey(oa.ObjectID, oa.ObjectMachine);
                        connectedObjects.Add(key, moKey);
                    }
                }

                foreach (b.ObjectAssociation oa in associationsAsParent)
                {
                    string key = oa.ChildObjectID.ToString() + "_" + oa.ChildObjectMachine;
                    if (!connectedObjects.ContainsKey(key))
                    {
                        MetaObjectKey moKey = new MetaObjectKey(oa.ChildObjectID, oa.ChildObjectMachine);
                        connectedObjects.Add(key, moKey);
                    }
                }
            }
            return connectedObjects;
        }

        #endregion

        #region Shape Cache

        public static FileDialogSpecification getFileSpec(string fileName)
        {
            string extension = strings.GetFileExtension(fileName);
            // Use diagram as default;
            FileDialogSpecification applicableSpec = FilePathManager.Instance.GetSpecification(FileTypeList.Diagram);
            Collection<FileDialogSpecification> specs = FilePathManager.Instance.GetList();
            foreach (FileDialogSpecification spec in specs)
            {
                if (spec.Extension == extension)
                {
                    // Gotcha!
                    applicableSpec = spec;
                    break;
                }
            }
            return applicableSpec;
        }
        //Call this to run on new thread
        public void buildShapeCacheThread()
        {
            Thread t = new Thread(buildShapeCache);
            t.IsBackground = true;
            t.Start();
        }
        //Call this to run on UI Thread
        private void buildShapeCache()
        {
            //TODO : Make specific to stencil files
            int max = Directory.GetFiles(Variables.Instance.StencilPath).Length;
            if (max == 0)
            {
                //MessageBox.Show(this, "Path" + Environment.NewLine + Variables.Instance.StencilPath + Environment.NewLine + Environment.NewLine + "Found no stencils to cache in this directory", "Some functions may not work as expected.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //set max
            UpdateTotal(max);
            int progress = 0;
            Dictionary<string, object> shapeCache = new Dictionary<string, object>();
            foreach (string file in Directory.GetFiles(Variables.Instance.StencilPath))
            {
#if DEBUG
                //continue;
#endif

                UpdateStatusLabel("Building Shape Cache");
                UpdateTotal(max);
                ProgressUpdate(progress += 1);
                //createspecification
                FileDialogSpecification fileSpec = getFileSpec(file);
                if (fileSpec.FileType != FileTypeList.SymbolStore)
                    continue;
                PaletteContainer plc = new PaletteContainer();
                try
                {
                    Graphing.Persistence.XmlPersistor xpersist = new Graphing.Persistence.XmlPersistor();
                    plc.Palette.Document = xpersist.DepersistCollection(file, Graphing.Persistence.FileType.Stencil) as BaseDocument;
                }
                catch
                {
                    //plc.Palette.Document = StorageManipulator.FileSystemManipulator.LoadDocument(file, fileSpec);
                }

                if (plc.Palette.Document == null)
                    continue;

                GoLayerCollectionObjectEnumerator stencilEnumerator = plc.Palette.Document.GetEnumerator();
                while (stencilEnumerator.MoveNext())
                //foreach (object o in plc.Palette.Document)
                {
                    object o = stencilEnumerator.Current;

                    if (!(o is GraphNode))
                        continue;

                    GraphNode node = o as GraphNode;
                    if (node == null)
                        continue;

                    if (node.BindingInfo == null)
                        continue;
                    //plc.Palette.Document.Remove(node);
                    if (node.BindingInfo.BindingClass == "DataView")
                    {
                        BoundLabel boundLabel = node.Label as BoundLabel;
                        string type = "";
                        if (boundLabel != null)
                        {
                            switch (boundLabel.Name)
                            {
                                case "lblDataViewTypexx":
                                    type = "Logical";
                                    break;
                                case "lblDataViewType":
                                    type = "Physical";
                                    break;
                            }
                        }

                        if (type != "")
                        {
                            if (!shapeCache.ContainsKey(node.BindingInfo.BindingClass + "_" + type))
                                shapeCache.Add(node.BindingInfo.BindingClass + "_" + type, node.Copy());
                        }
                        else if (!shapeCache.ContainsKey(node.BindingInfo.BindingClass))
                        {
                            shapeCache.Add(node.BindingInfo.BindingClass, node.Copy());
                        }
                    }
                    else
                        if (!shapeCache.ContainsKey(node.BindingInfo.BindingClass))
                            shapeCache.Add(node.BindingInfo.BindingClass, node.Copy());
                }
                plc.Close();
                if (!plc.IsDisposed)
                    plc.Dispose();
            }
            Variables.Instance.ShapeCache = shapeCache;
            UpdateTotal(0);
            UpdateStatusLabel("Ready");
        }

        #endregion

        #region eHPUM

        private void menuItemDatabaseeHPUM_Click(object sender, EventArgs e)
        {
            Tools.eHPUM.eHPUM win = new Tools.eHPUM.eHPUM();
            win.ShowDialog(this);
            Dictionary<int, string> keysToLoad = win.GetKeys();
            win.Dispose();

            //if keys
            if (keysToLoad != null)
            {
                bool isAutoSave = Variables.Instance.AutoSaveEnabled;
                Variables.Instance.AutoSaveEnabled = false;

                availableNodes = new Dictionary<string, GraphNode>();
                string diagramName = keysToLoad[0];
                NewDocument(FileTypeList.Diagram);
                GetCurrentGraphView().Text = "*" + diagramName;
                //remove the first one because it is the diagram
                keysToLoad.Remove(0);

                loadingPosX = 0;
                loadingPosY = 0;

                ThreadedConstruction(keysToLoad);

                Variables.Instance.AutoSaveEnabled = isAutoSave;
                //ProgressUpdate(0);
            }
            ProgressUpdate(0);
        }

        //private Tools.MetaComparer mcomparer;
        private Collection<ObjectAssociation> associationsToJoin;
        private Dictionary<string, GraphNode> availableNodes;
        private int loadingPosX;
        private int loadingPosY;
        private int loaded;
        private int iterated;
        private bool forcePosition = false;
        public bool ThreadingConstruction
        {
            get
            {
                if (statusLabel.Text == "Building shape cache" || statusLabel.Text.Contains("Compare") || statusLabel.Text.Contains("Update") || statusLabel.Text.Contains("Diff") || statusLabel.Text.Contains("Retrieving") || statusLabel.Text.Contains("Binding") || statusLabel.Text.Contains("Checking") || statusLabel.Text.Contains("Refresh") || statusLabel.Text.Contains("Complex") || statusLabel.Text.Contains("Version Control") || statusLabel.Text.Contains("Depersist") || statusLabel.Text.Contains("?") || statusLabel.Text.Contains("Autosave"))
                    return false;
                return statusLabel.Text != "Ready";
            }
        }
        public bool ThreadedConstructionLayout = false;

        public void ThreadedConstruction(ICollection<KeyValuePair<int, string>> keysToLoad)
        {
            //mcomparer = new MetaBuilder.UIControls.GraphingUI.Tools.MetaComparer();
            currentGraphViewContainerForHPUM = GetCurrentGraphViewContainer();
            associationsToJoin = new Collection<ObjectAssociation>();
            Thread eThread = new Thread(delegate()
                                            {
                                                currentGraphViewContainerForHPUM.DisableMetaPropertiesOnSelection = true;

                                                loadingPosX = 0;
                                                loadingPosY = 0;
                                                iterated = 0;

                                                int key = 1;
                                                UpdateTotal(keysToLoad.Count);
                                                UpdateStatusLabel("Noding");// " + key + "/" + keysToLoad.Count.ToString());
                                                foreach (KeyValuePair<int, string> keyValuePair in keysToLoad)
                                                {
                                                    ProgressUpdate(key);

                                                    string _class = keyValuePair.Value;
                                                    string machine = keyValuePair.Value;
                                                    int i = 0;
                                                    foreach (string s in keyValuePair.Value.Split(Convert.ToChar(":")))
                                                    {
                                                        switch (i)
                                                        {
                                                            case 0:
                                                                machine = s;
                                                                break;
                                                            case 1:
                                                                _class = s;
                                                                break;
                                                        }
                                                        i++;
                                                    }

                                                    int id = keyValuePair.Key;

                                                    string shapeClass = _class;
                                                    if (_class.Contains("_"))
                                                    {
                                                        _class = _class.Substring(0, _class.IndexOf("_"));
                                                    }

                                                    MetaBase mbObj = Loader.GetByID(_class, id, machine);
                                                    if (mbObj == null)
                                                        continue;
                                                    mbObj.tag = shapeClass;

                                                    if (mbObj.Class != "DataField" && mbObj.Class != "DataAttribute" && mbObj.Class != "DataColumn" && mbObj.Class != "Attribute" && mbObj.Class != "FlowDescription" && mbObj.Class != "Selector Attrribute")
                                                        addNodeToGraphView(mbObj);

                                                    //add associations by this as parent
                                                    foreach (b.ObjectAssociation objectAssociation in d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(mbObj.pkid, mbObj.MachineName))
                                                    {
                                                        if (!associationsToJoin.Contains(objectAssociation))
                                                            associationsToJoin.Add(objectAssociation);
                                                    }
                                                    mbObj = null;
                                                    key += 1;
                                                }

                                                //connect all objects added on diagram
                                                if (associationsToJoin.Count <= 0)
                                                {
                                                    ResetStatus();
                                                    return;
                                                }
                                                addLinksToGraphView(associationsToJoin);

                                                currentGraphViewContainerForHPUM.DisableMetaPropertiesOnSelection = false;
                                            });
            eThread.IsBackground = true;
            eThread.Start();
        }

        public bool RefreshAddedHPUMNodes = false;
        public void addNodeToGraphView(MetaBase mBase)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<MetaBase>(addNodeToGraphView), new object[] { mBase });
            }
            else
            {
                if (currentGraphViewContainerForHPUM == null)
                    return;

                if (!forcePosition)
                {
                    if (loaded % 100 == 0) //every 100 Move Loaded Column
                    {
                        iterated += 1;

                        loadingPosY = 100;
                        loadingPosX = 100 + (350 * iterated);
                    }
                }
                IMetaNode imNodeToRefresh = null;
                if (mBase.tag.ToString() == "Rationale")
                {
                    MetaBuilder.Graphing.Controllers.ShapeDesignController sdc = new MetaBuilder.Graphing.Controllers.ShapeDesignController(currentGraphViewContainerForHPUM.View);
                    Rationale rat = sdc.AddRationale(mBase);
                    rat.Position = new PointF(loadingPosX, loadingPosY);
                    imNodeToRefresh = rat;
                }
                else
                {
                    GraphNode node = null;
                    //Get Shape!
                    if (availableNodes == null)
                        availableNodes = new Dictionary<string, GraphNode>();

                    if (availableNodes.ContainsKey(mBase.tag.ToString()))
                    {
                        node = (GraphNode)availableNodes[mBase.tag.ToString()].Copy();
                    }
                    else
                    {
                        try
                        {
                            //#if !DEBUG
                            node = (GraphNode)GetShape(mBase.tag.ToString()).Copy();
                            //#endif
                            availableNodes.Add(mBase.tag.ToString(), (GraphNode)node.Copy());
                        }
                        catch
                        {
                            //MessageBox.Show(this,"You need to select a stencil that contains a shape for this class.", mBase.Class + " class needs a shape");

                            //open stencil?
                            if (Tools.LoadFromDatabase.OpenStencilForClass(mBase.tag.ToString()) == DialogResult.OK)
                            {
                                try
                                {
                                    GraphNode Xnode = (GraphNode)Tools.LoadFromDatabase.GetShape(mBase.tag.ToString());
                                    if (Xnode != null)
                                    {
                                        //Variables.Instance.ShapeCache.Add(mBase.tag.ToString(), Xnode.Copy());
                                        node = (GraphNode)Xnode.Copy();
                                        availableNodes.Add(mBase.tag.ToString(), (GraphNode)node.Copy());
                                    }
                                    else
                                    {
                                        DisplayTip(mBase.tag + " was not loaded because a shape could not be found on the selected stencil", "Missing Shape");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log.WriteLog(ex.ToString());
                                    DisplayTip(mBase.tag + " was not loaded because an error occurred while searching the shapes within the selected stencil", "Missing Shape (Error)");
                                }
                            }
                        }
                    }

                    if (node == null)
                    {
#if DEBUG
                        try
                        {
                            BaseGraphNode n = new BaseGraphNode(mBase);
                            n.Construct(mBase.Class);
                            node = n as GraphNode;
                            if (node != null)
                                goto skipReturnNullNodeForBaseNode;
                        }
                        catch
                        {
                            return;
                        }
#endif
                        return;
                    }
                skipReturnNullNodeForBaseNode:
                    node.MetaObject = mBase;
                    node.HookupEvents();
                    node.BindToMetaObjectProperties();
                    node.Position = new PointF(loadingPosX, loadingPosY);
                    currentGraphViewContainerForHPUM.View.Document.Add(node);

                    if (mBase.pkid > 0)
                    {
                        node.Shadowed = true;
                        foreach (IMetaNode iNode in currentGraphViewContainerForHPUM.ViewController.GetIMetaNodesBoundToMetaObject(mBase))
                            (iNode as GoObject).Shadowed = true;

                        if (tImportOptions == TextImportOptions.Details)
                        {
                            foreach (ObjectAssociation objNodeAssociationRationale in d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(mBase.pkid, mBase.MachineName))
                            {
                                //add rationales
                                MetaObject ratNodeObject = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetBypkidMachine(objNodeAssociationRationale.ChildObjectID, objNodeAssociationRationale.ChildObjectMachine);
                                if (ratNodeObject != null && ratNodeObject.Class == "Rationale")
                                {
                                    ShapeDesignController shapeProtoTypingTool = new ShapeDesignController(GetCurrentGraphViewContainer().View);
                                    Rationale rat = shapeProtoTypingTool.AddAndParentRationale(node, Loader.GetFromProvider(ratNodeObject.pkid, ratNodeObject.Machine, ratNodeObject.Class, false));
                                }
                            }
                        }
                    }

                    if (tImportOptions == TextImportOptions.Details)
                    {
                        if (mBase.Class == "DataView" || mBase.Class == "Entity" || mBase.Class == "DataEntity" || mBase.Class == "DataTable" || mBase.Class == "PhysicalInformationArtefact" || mBase.Class == "LogicalInformationArtefact" || mBase.Class == "DataDomain")
                        {
                            ////load child objects
                            //GoCollection col = new GoCollection();
                            ////currentGraphViewContainerForHPUM.View.Selection.Add(node);
                            //col.Add(node);
                            //MetaBuilder.UIControls.GraphingUI.Tools.MetaComparer mcompare = new MetaBuilder.UIControls.GraphingUI.Tools.MetaComparer();
                            ////if (mcomparer == null)
                            //    //mcomparer = new MetaBuilder.UIControls.GraphingUI.Tools.MetaComparer();
                            //mcompare.MyView = currentGraphViewContainerForHPUM.View;
                            ////mcomparer.CompareSelection(currentGraphViewContainerForHPUM.View.Selection, true);
                            //mcompare.RefreshSelection(col);
                            ////currentGraphViewContainerForHPUM.View.Selection.Clear();

                            foreach (ObjectAssociation objAssociation in d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(mBase.pkid, mBase.MachineName))
                            {
                                if (objAssociation.Machine != null)
                                    continue;
                                MetaObject obj = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetBypkidMachine(objAssociation.ChildObjectID, objAssociation.ChildObjectMachine);
                                if (obj.Class == "DataField" || obj.Class == "DataValue" || obj.Class == "DataAttribute" || obj.Class == "DataColumn" || obj.Class == "Attribute")
                                {
                                    if (node is CollapsibleNode)
                                    {
                                        CollapsingRecordNodeItem item = null;
                                        if (d.DataRepository.ClassAssociationProvider.GetByCAid(objAssociation.CAid).Caption.Contains("Descrip"))
                                        {
                                            item = (node as CollapsibleNode).RepeaterSections[1].AddChildItem(obj.Class, "Name");
                                            item.MetaObject = Loader.GetFromProvider(obj.pkid, obj.Machine, obj.Class, false);
                                            item.BindToMetaObjectProperties();
                                            item.Editable = true;
                                            item.HookupEvents();
                                            //add rationales
                                        }
                                        else
                                        {
                                            item = (node as CollapsibleNode).RepeaterSections[0].AddChildItem(obj.Class, "Name");
                                            item.MetaObject = Loader.GetFromProvider(obj.pkid, obj.Machine, obj.Class, false);
                                            item.BindToMetaObjectProperties();
                                            item.Editable = true;
                                            item.HookupEvents();
                                        }
                                        if (item != null)
                                        {
                                            foreach (ObjectAssociation objAssociationRationale in d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(obj.pkid, obj.Machine))
                                            {
                                                //add rationales
                                                MetaObject ratObject = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetBypkidMachine(objAssociationRationale.ChildObjectID, objAssociationRationale.ChildObjectMachine);
                                                if (ratObject != null && ratObject.Class == "Rationale")
                                                {
                                                    ShapeDesignController shapeProtoTypingTool = new ShapeDesignController(GetCurrentGraphViewContainer().View);
                                                    Rationale rat = shapeProtoTypingTool.AddAndParentRationale(item, Loader.GetFromProvider(ratObject.pkid, ratObject.Machine, ratObject.Class, false));
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        node.ToString();
                                    }
                                }
                            }
                            //go through each metaobject child and get associations to add
                            if (node is CollapsibleNode)
                            {
                                if ((node as CollapsibleNode).RepeaterSections.Count > 0)
                                {
                                    foreach (RepeaterSection section in (node as CollapsibleNode).RepeaterSections)
                                    {
                                        foreach (GoObject obj in section)
                                        {
                                            if (obj is CollapsingRecordNodeItem)
                                            {
                                                CollapsingRecordNodeItem item = obj as CollapsingRecordNodeItem;
                                                if (!item.IsHeader && item.MetaObject != null)
                                                {
                                                    foreach (b.ObjectAssociation objectAssociation in d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(item.MetaObject.pkid, item.MetaObject.MachineName))
                                                    {
                                                        //do not link association if the child is the parent object
                                                        if (objectAssociation.ChildObjectID != mBase.pkid && objectAssociation.ChildObjectMachine != mBase.MachineName)
                                                            if (associationsToJoin != null)
                                                                if (!associationsToJoin.Contains(objectAssociation))
                                                                    associationsToJoin.Add(objectAssociation);
                                                    }
                                                }
                                            }
                                            else if (obj is ExpandableLabelList)
                                            {
                                                (obj as ExpandableLabelList).Expand();
                                            }
                                        }
                                        if (section is ExpandableLabelList)
                                            (section as ExpandableLabelList).Expand();
                                    }
                                }
                            }
                        }
                    }
                    #region Refreshing NEEDS TO MOVE TO REFRESH COLLECTION INSTEAD OF SINGLE ITEMS
                    imNodeToRefresh = node;
                    if (RefreshAddedHPUMNodes && imNodeToRefresh is GoObject)
                    {
                        GoCollection col = new GoCollection();
                        col.Add(imNodeToRefresh as GoObject);

                        MetaBuilder.UIControls.GraphingUI.Tools.MetaComparerWorker mcomparerWorker = new MetaBuilder.UIControls.GraphingUI.Tools.MetaComparerWorker(col, true, currentGraphViewContainerForHPUM.View);
                        mcomparerWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(mcomparerWorker_RunWorkerCompleted);
                        mcomparerWorker.RunWorkerAsync(col);

                        //if (mcomparer == null)
                        //    mcomparer = new MetaBuilder.UIControls.GraphingUI.Tools.MetaComparerWorker();
                        //mcomparer.MyView = currentGraphViewContainerForHPUM.View;
                        //mcomparer.RefreshSelection(col);
                    }
                    #endregion
                }
                //}
                loadingPosX += 1;
                loadingPosY += 1;
                loaded += 1;
            }
        }

        private void mcomparerWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ResetStatus();
        }
        private void addLinksToGraphView(Collection<b.ObjectAssociation> associations)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<Collection<b.ObjectAssociation>>(addLinksToGraphView), new object[] { associations });
            }
            else
            {
                loadingPosX = 0;
                loadingPosY = 0;
                loaded = 0;
                iterated = 0;
                UpdateTotal(associations.Count);
                int key = 1;
                if (currentGraphViewContainerForHPUM == null)
                    return;
                Dictionary<string, IMetaNode> nodesAlreadyInDocument = Tools.LoadFromDatabase.GetNodesInCollection(currentGraphViewContainerForHPUM.View.Document);
                foreach (ObjectAssociation objectAssociation in associations)
                {
                    ProgressUpdate(key);
                    UpdateStatusLabel("Linking " + key + "/" + associations.Count);
                    Application.DoEvents();
                    if (!nodesAlreadyInDocument.ContainsKey(objectAssociation.ObjectID + objectAssociation.ObjectMachine) ||
                        !nodesAlreadyInDocument.ContainsKey(objectAssociation.ChildObjectID + objectAssociation.ChildObjectMachine))
                        continue;

                    //How do i know which node is the parent?
                    IMetaNode pNode = nodesAlreadyInDocument[objectAssociation.ObjectID + objectAssociation.ObjectMachine];
                    IMetaNode cNode = nodesAlreadyInDocument[objectAssociation.ChildObjectID + objectAssociation.ChildObjectMachine];

                    //TODO
                    //do not add links between collapsible nodes and record items if they are attribute mappings
                    QLink l = QLink.CreateLink(pNode as GoNode, cNode as GoNode, 0, objectAssociation.CAid);
                    if (l != null)
                    {
                        currentGraphViewContainerForHPUM.View.Document.Add(l);

                        //get artefacts for this link
                        foreach (Artifact artifact in d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ArtifactProvider.GetByObjectIDObjectMachine(objectAssociation.ObjectID, objectAssociation.ObjectMachine))
                        {
                            if (artifact.ChildObjectID == objectAssociation.ChildObjectID && artifact.ChildObjectMachine == objectAssociation.ChildObjectMachine)
                            {
                                MetaObject mbObj = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetBypkidMachine(artifact.ArtifactObjectID, artifact.ArtefactMachine);
                                if (mbObj != null)
                                {
                                    switch (mbObj.Class)
                                    {
                                        case "Rationale":
                                            //add rationale
                                            currentGraphViewContainerForHPUM.View.StartTransaction();
                                            Rationale rationale = new Rationale();
                                            rationale.MetaObject = Loader.GetByID(mbObj.Class, mbObj.pkid, mbObj.Machine);
                                            rationale.Location = new PointF(l.MidLabel.Location.X + 25, l.MidLabel.Location.Y - 25);
                                            rationale.Editable = true;
                                            rationale.Anchor = l.MidLabel;
                                            rationale.BindToMetaObjectProperties();
                                            rationale.HookupEvents();
                                            currentGraphViewContainerForHPUM.View.Document.Add(rationale);
                                            currentGraphViewContainerForHPUM.View.FinishTransaction("Add rationale AUTO");
                                            break;
                                        default:
                                            //add artifact
                                            currentGraphViewContainerForHPUM.View.StartTransaction();
                                            ArtefactNode n = new ArtefactNode();
                                            n.BindingInfo = new BindingInfo();
                                            n.BindingInfo.BindingClass = mbObj.Class;
                                            n.MetaObject = Loader.GetByID(mbObj.Class, mbObj.pkid, mbObj.Machine);
                                            n.Location = new PointF(l.MidLabel.Location.X + 25, l.MidLabel.Location.Y - 25);
                                            n.Editable = true;
                                            n.Label.Editable = false;
                                            n.BindToMetaObjectProperties();
                                            n.HookupEvents();

                                            currentGraphViewContainerForHPUM.View.Document.Add(n);
                                            GoGroup grp = l.MidLabel as GoGroup;
                                            foreach (GoObject o in grp)
                                            {
                                                if (o is FishLinkPort)
                                                {
                                                    // now link 'em
                                                    FishLink fishlink = new FishLink();
                                                    fishlink.FromPort = n.Port;
                                                    FishLinkPort flp = o as FishLinkPort;
                                                    //flp.IsValidFrom = false;  // can only draw links to this port, not from it
                                                    //flp.ToSpot = NoSpot;
                                                    fishlink.ToPort = flp;
                                                    currentGraphViewContainerForHPUM.View.Document.Add(fishlink);
                                                }
                                            }

                                            currentGraphViewContainerForHPUM.View.FinishTransaction("Add artefact AUTO");
                                            break;
                                    }
                                }
                            }
                        }
                    }

                    key += 1;
                }
                nodesAlreadyInDocument = null;

                if (ThreadedConstructionLayout)
                {
                    DockingForm.DockForm.UpdateStatusLabel("Basic Tree Layout");
                    TreeLayoutDialog tld = new TreeLayoutDialog();
                    tld.Auto = true;
                    tld.TreeLayout(currentGraphViewContainerForHPUM.View);
                    tld.Dispose();
                    ThreadedConstructionLayout = false;
                }

                //reset status
                ProgressUpdate(0);
            }
        }

        public GraphNode GetShape(string cls)
        {
            if (Variables.Instance.ShapeCache.Count > 0)
            {
                GraphNode newNodeObject = (GraphNode)Variables.Instance.ReturnShape(cls);
                if (newNodeObject != null)
                    return (GraphNode)newNodeObject.Copy();
            }
            return null;
        }

        #endregion

        #region RMBTI

        public enum TextImportOptions { Nothing = 0, Normal = 1, Details = 2 }
        public TextImportOptions tImportOptions = TextImportOptions.Nothing;
        private void menuItemDatabaseRMBTI_Click(object sender, EventArgs e)
        {
            availableNodes = new Dictionary<string, GraphNode>();
            loadingPosX = 100;
            loadingPosY = 100;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text Files (*.txt)|*.txt";
            ofd.Title = "Select a text file to begin the process";
            ofd.AutoUpgradeEnabled = true;
            ofd.Multiselect = false;

            if (GetCurrentGraphView() == null)
            {
                NewDocument(FileTypeList.Diagram);
            }

            if (ofd.ShowDialog(this) == DialogResult.OK && ofd.FileName.Length > 0)
            {
                int number = 0;
                System.IO.StreamReader file = new System.IO.StreamReader(ofd.FileName);

                //123456;DEON-834:Function
                Dictionary<int, string> keys = new Dictionary<int, string>();
                tImportOptions = TextImportOptions.Nothing;
                string line = "";
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Length == 0)
                        continue;
                    if (!line.Contains(";"))
                    {
                        switch (line.ToLower().Trim())
                        {
                            case "normal":
                                tImportOptions = TextImportOptions.Normal;
                                break;
                            case "details":
                                tImportOptions = TextImportOptions.Details;
                                break;
                        }
                        continue;
                    }
                    int pkid = 0;
                    string mc = "";
                    number++;
                    int x = 0;
                    foreach (string s in line.Split(';'))
                    {
                        if (x == 0)
                            pkid = int.Parse(s);
                        else if (x == 1)
                            mc = s;
                        else
                            break;
                        x++;
                    }
                    if (pkid > 0 && mc.Length > 0)
                    {
                        x = 0;
                        string machine = "";
                        string mclass = "";
                        foreach (string s in mc.Split(':'))
                        {
                            if (x == 0)
                                machine = s;
                            else if (x == 1)
                                mclass = s;
                            else
                                break;
                            x = 1;
                        }
                        if (VCStatusTool.UserHasControl(Loader.GetByID(mclass, pkid, machine)))
                            keys.Add(pkid, mc);
                    }
                }
                file.Close();
                ThreadedConstruction(keys);
            }
        }

        #endregion

        #region Invoked

        private readonly ToolTip dockingFormTip = new ToolTip();

        private delegate void DisplayTipDelegate(string message, string title);
        private delegate void DisplayTipIconDelegate(string message, string title, ToolTipIcon icon);
        //provides a simple shortlived tip at the bottom right of the form
        public void DisplayTip(string message, string title)
        {
            if (InvokeRequired)
                BeginInvoke(new DisplayTipDelegate(DisplayTip), new object[] { message, title });
            else
            {
                dockingFormTip.ToolTipIcon = ToolTipIcon.Info;
                dockingFormTip.ToolTipTitle = title;
                dockingFormTip.InitialDelay = 0;
                dockingFormTip.IsBalloon = false;
                dockingFormTip.UseAnimation = false;
                dockingFormTip.UseFading = false;
                dockingFormTip.Show(message, this, Right - 200, Bottom - 100, 5000);
            }
        }
        public void DisplayTip(string message, string title, ToolTipIcon icon)
        {
            if (InvokeRequired)
                BeginInvoke(new DisplayTipIconDelegate(DisplayTip), new object[] { message, title, icon });
            else
            {
                dockingFormTip.ToolTipIcon = icon;
                dockingFormTip.ToolTipTitle = title;
                dockingFormTip.InitialDelay = 0;
                dockingFormTip.IsBalloon = false;
                dockingFormTip.UseAnimation = false;
                dockingFormTip.UseFading = false;
                dockingFormTip.Show(message, this, Right - 200, Bottom - 100, 5000);
            }
        }

        public void UpdateTotal(int max)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<int>(UpdateTotal), new object[] { max });
            }
            else
            {
                progressBar1.Maximum = max;
                if (max == 0)
                {
                    ResetStatus();
                }
            }
        }
        public void ProgressUpdate(int progress)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<int>(ProgressUpdate), new object[] { progress });
            }
            else
            {
                if (progress == 0)
                    UpdateTotal(0);

                if (progressBar1.Maximum > 0)
                {
                    if (progressBar1.Maximum < progress)
                    {
                        progressBar1.Maximum = progress + 10;
                        progressBar1.Value = progress;
                    }
                    else
                        progressBar1.Value = progress;
                }
            }
        }
        public void UpdateStatusLabel(string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(UpdateStatusLabel), new object[] { message });
            }
            else
            {
                statusLabel.Text = message;
                if (message == "Read Only File")
                {
                    string file = "";
                    if ((progressBar1.Tag is DiagramSaver))
                        file = Environment.NewLine + (progressBar1.Tag as DiagramSaver).FileName;
                    DisplayTip("The file is read only.", "Unable to save file." + file, ToolTipIcon.Error);
                }
            }
        }

        private void statusLabel_Click(object sender, System.EventArgs e)
        {
#if DEBUG
            GetCurrentGraphViewContainer().CustomModified = false;
            statusLabel.BackColor = SystemColors.Control;
#endif
        }

        #endregion

        public void CheckIfExistsOnDiagramsThread(IMetaNode parentShape)
        {
            CheckIfExistsOnDiagrams(parentShape);
            //Thread thread = new Thread(CheckIfExistsOnDiagrams);
            //thread.Start(parentShape);
        }
        //Call this to run on UI Thread
        public void CheckIfExistsOnDiagrams(IMetaNode parentShape)
        {
            if (InvokeRequired)
                BeginInvoke(new Action<IMetaNode>(CheckIfExistsOnDiagrams), new object[] { parentShape });
            else
            {
                //DisplayTip("Checking for shallow copies of this object across diagrams", "Background Shallow Check");
                if (Core.Variables.Instance.IsViewer || (GetCurrentGraphViewContainer() != null && GetCurrentGraphViewContainer().ReadOnly))
                    return;

                if (parentShape.MetaObject == null)
                    return;
                int count = 0;

                VCStatusList currentParentState = parentShape.MetaObject.State; //check if parent is readonly
                if (currentParentState != VCStatusList.CheckedIn || currentParentState != VCStatusList.CheckedOutRead || currentParentState != VCStatusList.Locked || currentParentState != VCStatusList.Obsolete)
                {
                    foreach (DockContent c in dockPanel1.Documents)
                    {
                        if (c is GraphViewContainer)
                        {
                            GraphViewContainer container = c as GraphViewContainer;
                            foreach (Object o in container.View.Document)
                            {
                                if (o is IMetaNode)
                                {
                                    IMetaNode node = null;
                                    node = o as IMetaNode;
                                    //VCStatusList currentState = node.MetaObject.State; ////check if this node is readonly
                                    //if (currentState != VCStatusList.CheckedIn || currentState != VCStatusList.CheckedOutRead || currentState != VCStatusList.Locked || currentState != VCStatusList.Obsolete)
                                    //{
                                    //10 January 2013 only check objects with a pkid > 0 (0-pkid objects cant be shallow copies)
                                    if (node != null && node.MetaObject != null && node.MetaObject.pkid > 0 && node != parentShape) //skip the sender of this object
                                    {
                                        if (parentShape is CollapsingRecordNodeItem)
                                        {
                                            if (node is CollapsibleNode) //Only check collapsible nodes (inherit graphnode)
                                            {
                                                if ((node as CollapsibleNode).RepeaterSections.Count > 0)
                                                {
                                                    foreach (RepeaterSection section in (node as CollapsibleNode).RepeaterSections)
                                                    {
                                                        foreach (GoObject obj in section)
                                                        {
                                                            if (obj is CollapsingRecordNodeItem)
                                                            {
                                                                CollapsingRecordNodeItem item = obj as CollapsingRecordNodeItem;
                                                                if (!item.IsHeader && item.MetaObject != null && item != parentShape) //skip the sender of this object when it is a collapsible node
                                                                {
                                                                    if (item.MetaObject.pkid == parentShape.MetaObject.pkid && item.MetaObject.MachineName == parentShape.MetaObject.MachineName)
                                                                    {
                                                                        item.MetaObject = parentShape.MetaObject;
                                                                        item.HookupEvents();
                                                                        //item.FireMetaObjectChanged(item, new EventArgs());
                                                                        item.BindToMetaObjectProperties();
                                                                        item.Shadowed = true;
                                                                        (parentShape as CollapsingRecordNodeItem).Shadowed = true;
                                                                        count += 1;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else //ALL OTHER IMETANODES + artefacts
                                        {
                                            if (node.MetaObject.pkid == parentShape.MetaObject.pkid && node.MetaObject.MachineName == parentShape.MetaObject.MachineName)
                                            {
                                                node.MetaObject = parentShape.MetaObject;
                                                node.HookupEvents();
                                                node.BindToMetaObjectProperties();

                                                if (node is GraphNode)
                                                    (node as GraphNode).Shadowed = true;
                                                if (parentShape is GraphNode)
                                                    (parentShape as GraphNode).Shadowed = true;
                                                //10 January 2013
                                                if (parentShape is GraphNode && node is GraphNode)
                                                    updateNodeFormatting(parentShape as GraphNode, node as GraphNode);
                                                count += 1;
                                            }

                                            //10 January 2013 Support for ILinkedContainers
                                            if (node is MetaBuilder.Graphing.Shapes.Nodes.Containers.ILinkedContainer)
                                                count += checkIfExistsOnDiagramsContainerChecker(parentShape, node as IGoCollection);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    m_metaObjectExplorer.UpdateObjects(parentShape.MetaObject);
                }

                if (count <= 0)
                    return;

                if (parentShape != null && parentShape.MetaObject != null)
                {
                    string name = parentShape.MetaObject.ToString() == null ? parentShape.MetaObject.Class + "(" + parentShape.MetaObject.pkid + ":" + parentShape.MetaObject.MachineName + ")" : parentShape.MetaObject.ToString();
                    if (count == 1)
                    {
                        DisplayTip(count + " shallow copy of " + name + " was found across all open diagrams", "Object Modified");
                    }
                    else
                    {
                        DisplayTip(count + " shallow copies of " + name + " were found across all open diagrams", "Objects Modified");
                    }
                }
            }
        }
        public void CheckIfExistsOnDiagramsNoTip(object state)
        {
            IMetaNode parentShape = state as IMetaNode;
            if (InvokeRequired)
                BeginInvoke(new Action<IMetaNode>(CheckIfExistsOnDiagrams), new object[] { parentShape });
            else
            {
                //DisplayTip("Checking for shallow copies of this object across diagrams", "Background Shallow Check");
                if (Core.Variables.Instance.IsViewer || GetCurrentGraphViewContainer().ReadOnly)
                    return;

                if (parentShape.MetaObject == null)
                    return;
                VCStatusList currentParentState = parentShape.MetaObject.State; //check if parent is readonly
                if (currentParentState != VCStatusList.CheckedIn || currentParentState != VCStatusList.CheckedOutRead || currentParentState != VCStatusList.Locked || currentParentState != VCStatusList.Obsolete)
                {
                    foreach (DockContent c in dockPanel1.Documents)
                    {
                        if (c is GraphViewContainer)
                        {
                            GraphViewContainer container = c as GraphViewContainer;
                            foreach (Object o in container.View.Document)
                            {
                                if (o is IMetaNode)
                                {
                                    IMetaNode node = null;
                                    node = o as IMetaNode;
                                    if (node != null && node.MetaObject != null && node.MetaObject.pkid > 0 && node != parentShape) //skip the sender of this object
                                    {
                                        if (parentShape is CollapsingRecordNodeItem)
                                        {
                                            if (node is CollapsibleNode) //Only check collapsible nodes (inherit graphnode)
                                            {
                                                if ((node as CollapsibleNode).RepeaterSections.Count > 0)
                                                {
                                                    foreach (RepeaterSection section in (node as CollapsibleNode).RepeaterSections)
                                                    {
                                                        foreach (GoObject obj in section)
                                                        {
                                                            if (obj is CollapsingRecordNodeItem)
                                                            {
                                                                CollapsingRecordNodeItem item = obj as CollapsingRecordNodeItem;
                                                                if (!item.IsHeader && item.MetaObject != null && item != parentShape) //skip the sender of this object when it is a collapsible node
                                                                {
                                                                    if (item.MetaObject.pkid == parentShape.MetaObject.pkid && item.MetaObject.MachineName == parentShape.MetaObject.MachineName)
                                                                    {
                                                                        item.MetaObject = parentShape.MetaObject;
                                                                        item.BindToMetaObjectProperties();
                                                                        item.Shadowed = true;
                                                                        (parentShape as CollapsingRecordNodeItem).Shadowed = true;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else //ALL OTHER IMETANODES + artefacts
                                        {
                                            if (node.MetaObject.pkid == parentShape.MetaObject.pkid && node.MetaObject.MachineName == parentShape.MetaObject.MachineName)
                                            {
                                                node.MetaObject = parentShape.MetaObject;
                                                //node.FireMetaObjectChanged(node, new EventArgs());
                                                node.BindToMetaObjectProperties();

                                                if (node is GraphNode)
                                                    (node as GraphNode).Shadowed = true;
                                                if (parentShape is GraphNode)
                                                    (parentShape as GraphNode).Shadowed = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public string GetAClass()
        {
            ChooseClass chooseClass = new ChooseClass("Select a Class", "Select a class for this shape");
            chooseClass.StartPosition = FormStartPosition.CenterScreen;
            DialogResult res = chooseClass.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                return chooseClass.SelectedClass;
            }
            return "";
        }

        //10 January 2013
        private int checkIfExistsOnDiagramsContainerChecker(IMetaNode parentShape, IGoCollection cont)
        {
            int count = 0;
            //GoCollection col = new GoCollection();
            //if (cont is MetaBuilder.Graphing.Shapes.Nodes.Containers.SubgraphNode)
            //    col = (cont as MetaBuilder.Graphing.Shapes.Nodes.Containers.SubgraphNode).;
            //else if (cont is MetaBuilder.Graphing.Shapes.Nodes.Containers.ValueChain)
            //    col = (cont as MetaBuilder.Graphing.Shapes.Nodes.Containers.ValueChain);
            //else if (cont is MetaBuilder.Graphing.Shapes.Nodes.Containers.MappingCell)
            //    col = (cont as MetaBuilder.Graphing.Shapes.Nodes.Containers.MappingCell);

            //if ((cont as IMetaNode).MetaObject.pkid == parentShape.MetaObject.pkid && (cont as IMetaNode).MetaObject.MachineName == parentShape.MetaObject.MachineName)
            //{
            //    (cont as IMetaNode).MetaObject = parentShape.MetaObject;
            //    count += 1;
            //}

            foreach (KeyValuePair<string, IMetaNode> childNodeKvp in Tools.LoadFromDatabase.GetNodesInCollection(cont))
            {
                IMetaNode childNode = childNodeKvp.Value;
                if (childNode == parentShape)
                    continue;
                if (childNode.MetaObject.pkid == parentShape.MetaObject.pkid && childNode.MetaObject.MachineName == parentShape.MetaObject.MachineName)
                {
                    childNode.MetaObject = parentShape.MetaObject;
                    //childNode.FireMetaObjectChanged(cont, new EventArgs());
                    childNode.HookupEvents();
                    childNode.BindToMetaObjectProperties();
                    //parentShape.FireMetaObjectChanged(parentShape, new EventArgs());

                    if (childNode is GraphNode)
                        (childNode as GraphNode).Shadowed = true;
                    if (parentShape is GraphNode)
                        (parentShape as GraphNode).Shadowed = true;

                    if (parentShape is GraphNode && childNode is GraphNode)
                        updateNodeFormatting(parentShape as GraphNode, childNode as GraphNode);
                    count += 1;
                }
            }

            foreach (GoObject o in cont)
                if (o is IGoCollection)
                    count += checkIfExistsOnDiagramsContainerChecker(parentShape, o as IGoCollection);

            return count;
        }

        //TODO : thread this
        public void updateNodeFormattingForAllNodes(GraphNode node)
        {
            if (!Core.Variables.Instance.UseShallowCopyColor) //prevent unneccesary cycles
                return;

            Collection<GraphViewContainer> documentcontent = new Collection<GraphViewContainer>();
            foreach (IDockContent content in dockPanel1.Documents)
                if (content is GraphViewContainer)
                    documentcontent.Add(content as GraphViewContainer);

            for (int i = 0; i < documentcontent.Count; i++)
            {
                foreach (IMetaNode imNode in documentcontent[i].ViewController.GetIMetaNodesBoundToMetaObject(node.MetaObject))
                {
                    if (imNode == node)
                        continue;
                    if (imNode is GraphNode)
                        updateNodeFormatting(node, imNode as GraphNode);
                }
            }

            documentcontent = null;
        }

        //10 January 2013
        public void updateNodeFormatting(GraphNode parent, GraphNode child)
        {
            if (!Core.Variables.Instance.UseShallowCopyColor)
                return;

            GoCollection pCol = new GoCollection();
            pCol.Add(parent);
            MetaBuilder.Graphing.Formatting.FormattingManipulator forman = new MetaBuilder.Graphing.Formatting.FormattingManipulator(pCol);
            GoCollection cCol = new GoCollection();
            cCol.Add(child);
            forman.ApplyToSelection(cCol);
        }

        public Tools.Explorer.MetaObjectExplorer MetaObjectExplorer
        {
            get { return m_metaObjectExplorer; }
            set { m_metaObjectExplorer = value; }
        }

        private Tools.DataModel.UI.ValidationResultForm m_vResultForm;
        public Tools.DataModel.UI.ValidationResultForm ValidationResultForm
        {
            get
            {
                if (m_vResultForm == null)
                {
                    m_vResultForm = new MetaBuilder.UIControls.GraphingUI.Tools.DataModel.UI.ValidationResultForm();
                }
                else if (m_vResultForm.IsDisposed)
                {
                    m_vResultForm = new MetaBuilder.UIControls.GraphingUI.Tools.DataModel.UI.ValidationResultForm();
                    m_vResultForm.MyEngine = ValidationResultForm.MyEngine;
                }
                return m_vResultForm;
            }
            set { m_vResultForm = value; }
        }

        public ImageList ImageListWorkspaces
        {
            get
            {
                return imageListWorkspaces;
            }
        }

        public void ShallowCopyCurrentDiagram()
        {
            if (GetCurrentGraphViewContainer() != null)
            {
                GoDocument doc = (GetCurrentGraphViewContainer().View.Document as NormalDiagram).CopyAsShallow();
                NewDiagram();
                (GetCurrentGraphViewContainer().View.Document as NormalDiagram).SizeFrameLayer((doc as NormalDiagram).DocumentFrame.Frame.Bounds);
                DocumentInfo info = (doc as NormalDiagram).DocumentFrame.GetDocumentInfo();
                info.FileName = "";
                (GetCurrentGraphViewContainer().View.Document as NormalDiagram).DocumentFrame.Update(info);

                Collection<GoObject> objects = new Collection<GoObject>();
                foreach (GoObject o in doc)
                {
                    if (o is FrameLayerGroup)
                        continue;
                    objects.Add(o);
                }

                //GetCurrentGraphViewContainer().View.BeginUpdate();
                //GetCurrentGraphViewContainer().View.SuspendLayout();

                //resize the document to the frame
                GetCurrentGraphViewContainer().ViewController.SwitchFrame(GetCurrentGraphViewContainer().View.Document as NormalDiagram); //select
                GetCurrentGraphViewContainer().ViewController.SwitchFrame(GetCurrentGraphViewContainer().View.Document as NormalDiagram); //deselect

                foreach (GoObject o in objects)
                {
                    o.Remove(); //from parent doc (the shallow document in memory)
                    GetCurrentGraphViewContainer().View.Document.Add(o);
                }
                //GetCurrentGraphViewContainer().View.Selection.Clear();
                //foreach (GoObject o in GetCurrentGraphViewContainer().View.Document)
                //{
                //    GetCurrentGraphViewContainer().View.Selection.Add(o);
                //}
                ////Move the objects because they are out of position in memory for some reason
                //GetCurrentGraphViewContainer().View.MoveSelection(null, new SizeF(60, 60), true);
                //GetCurrentGraphViewContainer().View.Selection.Clear();

                doc = null;

                GetCurrentGraphViewContainer().View.EndUpdate();
                GetCurrentGraphViewContainer().View.ResumeLayout();
            }
        }

        private FindAndReplaceText frText;
        public FindAndReplaceText FindAndReplaceText
        {
            get
            {
                if (frText == null || frText.IsDisposed)
                    frText = new FindAndReplaceText();
                return frText;
            }
        }

        private Syncfusion.Windows.Forms.ColorUIControl cuicontrol;
        private object colorBTN;
        private void SetupColourPicker()
        {
            if (cuicontrol != null)
                return;

            cuicontrol = new Syncfusion.Windows.Forms.ColorUIControl();
            cuicontrol.ColorGroups = Syncfusion.Windows.Forms.ColorUIGroups.All;
            cuicontrol.BorderStyle = BorderStyle.None;
            cuicontrol.StandardTabName = "Standard";
            cuicontrol.SelectedColorGroup = Syncfusion.Windows.Forms.ColorUISelectedGroup.StandardColors;
            cuicontrol.ColorSelected += new EventHandler(cd_ColorSelected);
            cuicontrol.Dock = DockStyle.Fill;
            cuicontrol.Show();
            cuicontrol.Visible = true;
            cuicontrol.Enabled = true;
            cuicontrol.Width = 250;
            cuicontrol.Height = 500;
        }

        public void ShowColorDialog(object sender)
        {
            SetupColourPicker();

            colorBTN = sender;
            if (colorBTN is Control)
                cuicontrol.SelectedColor = (colorBTN as Control).BackColor;
            else if (colorBTN is ToolStripButton)
                cuicontrol.SelectedColor = (colorBTN as ToolStripButton).BackColor;
            else
            {
                Log.WriteLog("GraphViewContainer::ShowColorDialog::Unknown Type - " + colorBTN.GetType().ToString());
                return;
            }

            Syncfusion.Windows.Forms.PopupControlContainer pcc = new Syncfusion.Windows.Forms.PopupControlContainer();
            pcc.AutoSize = true;
            pcc.Controls.Add(cuicontrol);
            //this.Controls.Add(pcc);
            pcc.ShowPopup(new Point(MousePosition.X, MousePosition.Y));
        }
        public void HideColorDialog()
        {
            colorBTN = null;
            Syncfusion.Windows.Forms.PopupControlContainer pcc = cuicontrol.Parent as Syncfusion.Windows.Forms.PopupControlContainer;
            pcc.HidePopup(Syncfusion.Windows.Forms.PopupCloseType.Done);
            //this.Controls.Remove(pcc);
        }
        private void cd_ColorSelected(object sender, EventArgs e)
        {
            if (colorBTN is Control)
                (colorBTN as Control).BackColor = cuicontrol.SelectedColor;
            else if (colorBTN is ToolStripButton)
                (colorBTN as ToolStripButton).BackColor = cuicontrol.SelectedColor;
            else
            {
                Log.WriteLog("GraphViewContainer::cd_ColorSelected::Unknown Type - " + colorBTN.GetType().ToString());
            }

            HideColorDialog();
        }

    }
}