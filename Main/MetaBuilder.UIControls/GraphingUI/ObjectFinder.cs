using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Meta;
using MetaBuilder.MetaControls;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.UIControls.GraphingUI
{
    public partial class ObjectFinder : Form
    {

        //#region Fields (1)

        //private Dictionary<MetaObjectKey, MetaBase> listedObjects;

        //#endregion Fields

        #region Constructors (1)

        Timer t = new Timer();
        private bool Server = false;
        public string Provider { get { return Server ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider; } }
        public ObjectFinder(bool server)
        {
            Server = server;
            InitializeComponent();
            AllowMultipleSelection = true;
            LimitToClass = null;
            t.Interval = 100;
            t.Tick += new EventHandler(t_Tick);
            t.Enabled = true;
            t.Start();
        }

        private void t_Tick(object sender, EventArgs e)
        {
            btnOK.Enabled = (SelectedObjects != null && SelectedObjects.Count != 0);
            if (btnOK.Enabled)
                lblCount.Text = SelectedObjects.Count + " Selected Object" + (SelectedObjects.Count > 1 ? "s" : "");
            lblCount.Visible = btnOK.Enabled;
        }

        #endregion Constructors

        #region Properties (6)

        private bool lfdb = false;
        public bool LFDB
        {
            get { return lfdb; }
            set
            {
                lfdb = value;
                if (lfdb)
                    chkLayout.Visible = true;
                objectFinderControl1.LFDB = lfdb;
            }
        }

        public bool AutoLayout { get { return chkLayout.Checked; } }

        public bool AllowMultipleSelection
        {
            get { return objectFinderControl1.AllowMultipleSelection; }
            set { objectFinderControl1.AllowMultipleSelection = value; }
        }

        public bool ArtefactsOnly
        {
            get { return objectFinderControl1.ArtefactsOnly; }
            set { objectFinderControl1.ArtefactsOnly = value; }
        }

        public List<VCStatusList> ExcludeStatuses
        {
            get { return objectFinderControl1.ExcludeStatuses; }
            set { objectFinderControl1.ExcludeStatuses = value; }
        }

        public bool IncludeStatusCombo
        {
            get { return objectFinderControl1.IncludeStatusCombo; }
            set { objectFinderControl1.IncludeStatusCombo = value; }
        }

        public string LimitToClass
        {
            get { return objectFinderControl1.LimitToClass; }
            set { objectFinderControl1.LimitToClass = value; }
        }

        public Dictionary<MetaObjectKey, MetaBase> SelectedObjects
        {
            get { return objectFinderControl1.SelectedObjects; }
        }

        #endregion Properties

        #region Methods (3)

        // Private Methods (3) 

        private void ObjectFinder_Load(object sender, EventArgs e)
        {
            objectFinderControl1.DoInitialisation();
            objectFinderControl1.ViewInContext += new ViewInContextEventHandler(objectFinderControl1_ViewInContext);
            objectFinderControl1.OpenDiagram += new EventHandler(objectFinderControl1_OpenDiagram);
        }
        private void ObjectFinder_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            objectFinderControl1.CancelThread();
        }
        void objectFinderControl1_OpenDiagram(object sender, EventArgs e)
        {
            GraphFileKey key = sender as GraphFileKey;
            GraphFile file = DataRepository.Connections[Provider].Provider.GraphFileProvider.Get(key);
            DockingForm.DockForm.OpenGraphFileFromDatabase(file, false, false);
        }

        void objectFinderControl1_ViewInContext(MetaBase mbase)
        {
            try
            {
                LiteGraphViewContainer contexter = new LiteGraphViewContainer();
                contexter.UseServer = Server;
                contexter.Setup(mbase);
                contexter.Size = new System.Drawing.Size(750, 750);
                Core.Log.WriteLog("Context container created, show is next");
                contexter.Show();
            }
            catch
            {
            }
        }

        #endregion Methods

        #region Form Stuff

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public List<MetaBase> SelectedObjectsList
        {
            get
            {
                List<MetaBase> retval = new List<MetaBase>();
                foreach (KeyValuePair<MetaObjectKey, MetaBase> kvp in SelectedObjects)
                {
                    retval.Add(kvp.Value);
                }
                return retval;
            }
        }

        #endregion
    }
}