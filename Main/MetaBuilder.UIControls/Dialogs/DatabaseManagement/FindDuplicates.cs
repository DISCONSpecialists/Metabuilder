using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Meta;
using MetaBuilder.MetaControls;
using MetaBuilder.UIControls.GraphingUI;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace MetaBuilder.UIControls.Dialogs.DatabaseManagement
{
    public partial class FindDuplicates : Form
    {

        #region Constructors (1)

        private bool Server = false;
        public string Provider { get { return Server ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider; } }
        public TList<Workspace> ServerWorkspacesUserHasWithAdminPermission;

        public FindDuplicates(bool server, TList<Workspace> serverAdminWorkspaces)
        {
            Server = server;
            ServerWorkspacesUserHasWithAdminPermission = serverAdminWorkspaces;
            InitializeComponent();

            backgroundWorker1.WorkerReportsProgress = false;
            backgroundWorker1.WorkerSupportsCancellation = false;
            backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            t.Tick += new EventHandler(t_Tick);
            this.FormClosing += new FormClosingEventHandler(FindDuplicates_FormClosing);
            if (Server)
            {
                duplicateObjectFinderControl1.ExcludeStatuses.Add(VCStatusList.CheckedOut);
                duplicateObjectFinderControl1.ExcludeVCItems = false;
            }
            else
            {
                duplicateObjectFinderControl1.ExcludeVCItems = true;
            }
        }

        private void FindDuplicates_FormClosing(object sender, FormClosingEventArgs e)
        {
            duplicateObjectFinderControl1.CancelThread();
            //MetaBuilder.SplashScreen.PleaseWait.CloseForm();
        }

        private void t_Tick(object sender, EventArgs e)
        {
            if (progressBarFileMerging.Value < progressBarFileMerging.Maximum)
                progressBarFileMerging.PerformStep();
            else
                progressBarFileMerging.Value = progressBarFileMerging.Step * 10;
        }

        #endregion Constructors

        #region Methods (5)

        // Private Methods (5) 
        private List<DuplicateObjectSpec> specs;
        Timer t = new Timer();
        private void btnStartMergeProcess_Click(object sender, EventArgs e)
        {
            if (duplicateObjectFinderControl1.SelectedObjectsList.Count > 0)
            {
                if (duplicateObjectFinderControl1.DuplicateObjects.Count > 0)
                {
                    specs = new List<DuplicateObjectSpec>();
                    foreach (KeyValuePair<MetaBase, List<MetaBase>> kvp in duplicateObjectFinderControl1.DuplicateObjects)
                    {
                        if (duplicateObjectFinderControl1.SelectedObjectsList.Contains(kvp.Key))
                        {
                            DuplicateObjectSpec spec = new DuplicateObjectSpec();
                            spec.Matches = new List<MetaBase>();
                            spec.Matches.Add(kvp.Key);
                            foreach (MetaBase mbMatched in kvp.Value)
                            {
                                spec.Matches.Add(mbMatched);
                            }
                            specs.Add(spec);
                        }
                    }
                    if (specs.Count > 0)
                    {
                        MergeObjects merger = new MergeObjects(Server);
                        merger.DuplicateObjectSpecifications = specs;
                        merger.ViewInContext += new ViewInContextEventHandler(duplicateObjectFinderControl1_ViewInContext);
                        merger.OpenDiagram += new EventHandler(merger_OpenDiagram);
                        merger.Start();
                        DialogResult res = merger.ShowDialog(this);
                        if (res == DialogResult.OK)
                        {
                            FormBorderStyle = FormBorderStyle.None;

                            labelFileMergeProgress.Visible = true;
                            labelFileMergeProgress.Text = "Merging Diagram Files...";

                            progressBarFileMerging.Visible = true;
                            progressBarFileMerging.Minimum = 10;
                            progressBarFileMerging.Maximum = 1000;
                            progressBarFileMerging.Step = 10;
                            progressBarFileMerging.Style = ProgressBarStyle.Continuous;

                            t.Start();
                            backgroundWorker1.RunWorkerAsync();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(this,"Select at least one item to merge", "Selection", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool openAffected = false;
        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (objectmerger.FilesHaveBeenProcessed)
            {
                List<ObjectMerger.FileSpec> filesToCheck = objectmerger.filesToCheck;

                if (Server)
                {
                    StringBuilder sbMessage = new StringBuilder();
                    sbMessage.Append("PLEASE READ THE FOLLOWING INSTRUCTIONS CAREFULLY" + Environment.NewLine);
                    sbMessage.Append("The following diagrams were influenced by the Merge action").Append(Environment.NewLine);
                    sbMessage.Append(Environment.NewLine);
                    sbMessage.Append("(Server) Diagrams").Append(Environment.NewLine);
                    foreach (ObjectMerger.FileSpec s in filesToCheck)
                    {
                        sbMessage.Append(" - " + s.Name).Append(Environment.NewLine);// + " " + s.Replaced.ToString()).Append(Environment.NewLine);
                    }
                    sbMessage.Append(Environment.NewLine);
                    sbMessage.Append("The latest version(s) of these files are checked in and ready for check out" + Environment.NewLine);
                    MessageBox.Show(this,sbMessage.ToString(), "Server Diagrams Were Modified", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    StringBuilder sbMessage = new StringBuilder();
                    sbMessage.Append("PLEASE READ THE FOLLOWING INSTRUCTIONS CAREFULLY" + Environment.NewLine);
                    sbMessage.Append("The following diagrams were influenced by the Merge action").Append(Environment.NewLine);
                    sbMessage.Append(Environment.NewLine);
                    sbMessage.Append("Diagrams").Append(Environment.NewLine);
                    foreach (ObjectMerger.FileSpec s in filesToCheck)
                    {
                        sbMessage.Append(" - " + s.Name).Append(Environment.NewLine);// + " " + s.Replaced.ToString()).Append(Environment.NewLine);
                    }
                    sbMessage.Append(Environment.NewLine);
                    sbMessage.Append("The latest version(s) of these files can be found in your Diagrams folder" + Environment.NewLine + Environment.NewLine);
                    sbMessage.Append("Click yes if you would like to open these diagrams" + Environment.NewLine);
                    if (MessageBox.Show(this,sbMessage.ToString(), "Diagrams Were Modified", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        openAffected = true;
                    }
                }
            }
            FormBorderStyle = FormBorderStyle.Sizable;
            labelFileMergeProgress.Visible = false;
            progressBarFileMerging.Visible = false;
            t.Stop();
            DialogResult = DialogResult.OK;
            CacheManager cacheManager = CacheFactory.GetCacheManager();
            cacheManager.Flush();
            if (openAffected && !Server)
            {
                foreach (ObjectMerger.FileSpec openingFileSpec in objectmerger.filesToCheck)
                {
                    DockingForm.DockForm.OpenFileInApplicableWindow(Core.Variables.Instance.DiagramPath + openingFileSpec.Name, false);
                }
            }
            this.Close();
        }
        private ObjectMerger objectmerger;
        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            objectmerger = new ObjectMerger();
            objectmerger.ExecuteMerge(specs, Server);
        }

        private void duplicateObjectFinderControl1_OpenDiagram(object sender, EventArgs e)
        {
            GraphFileKey key = sender as GraphFileKey;
            MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter adapter = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();
            GraphFile file = adapter.GetQuickFileDetails(key.pkid, key.Machine, (Provider == Core.Variables.Instance.ServerProvider));
            //GraphFile file = DataRepository.Connections[Provider].Provider.GraphFileProvider.Get(key);
            DockingForm.DockForm.OpenGraphFileFromDatabase(file, false, Server);
        }

        private void duplicateObjectFinderControl1_ViewInContext(MetaBase mbase)
        {
            LiteGraphViewContainer contexter = new LiteGraphViewContainer();
            contexter.UseServer = Server;
            contexter.Setup(mbase);
            contexter.ShowDialog(this);
        }

        private void FindDuplicates_Load(object sender, EventArgs e)
        {
            duplicateObjectFinderControl1.DoInitialisation();
            duplicateObjectFinderControl1.ViewInContext += new ViewInContextEventHandler(duplicateObjectFinderControl1_ViewInContext);
            duplicateObjectFinderControl1.OpenDiagram += new EventHandler(duplicateObjectFinderControl1_OpenDiagram);
        }

        private void merger_OpenDiagram(object sender, EventArgs e)
        {
            GraphFileKey key = sender as GraphFileKey;
            MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter adapter = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();
            GraphFile file = adapter.GetQuickFileDetails(key.pkid, key.Machine, (Provider == Core.Variables.Instance.ServerProvider));
            //GraphFile file = DataRepository.Connections[Provider].Provider.GraphFileProvider.Get(key);
            DockingForm.DockForm.OpenGraphFileFromDatabase(file, false, false);
        }

        #endregion Methods

    }
}