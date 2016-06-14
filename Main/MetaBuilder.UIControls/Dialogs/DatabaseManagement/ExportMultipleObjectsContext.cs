using System;
using System.Windows.Forms;
using MetaBuilder.BusinessFacade.Exports;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Meta;
using MetaBuilder.MetaControls;
using MetaBuilder.UIControls.GraphingUI;

namespace MetaBuilder.UIControls.Dialogs.DatabaseManagement
{
    public partial class ExportMultipleObjectsContext : Form
    {

        #region Constructors (1)
        private bool Server = false;
        public ExportMultipleObjectsContext(bool server)
        {
            Server = server;
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods (4)

        // Private Methods (4) 

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportContext contextExporter = new ExportContext();
            foreach (MetaBase mbase in this.objectFinderControl1.SelectedObjectsList)
                contextExporter.ObjectsToExport.Add(mbase);
            contextExporter.Export();
        }

        private void ManageMarkedForDeleteObjects_Load(object sender, EventArgs e)
        {
            objectFinderControl1.ExcludeStatuses.Add(VCStatusList.Obsolete);
            objectFinderControl1.ExcludeStatuses.Add(VCStatusList.MarkedForDelete);
            objectFinderControl1.AllowMultipleSelection = true;
            objectFinderControl1.IncludeStatusCombo = true;
            objectFinderControl1.DoInitialisation();
            objectFinderControl1.ViewInContext += new ViewInContextEventHandler(objectFinderControl1_ViewInContext);
            objectFinderControl1.OpenDiagram += new EventHandler(objectFinderControl1_OpenDiagram);
        }

        private void objectFinderControl1_OpenDiagram(object sender, EventArgs e)
        {
            GraphFileKey key = sender as GraphFileKey;
            GraphFile file = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileProvider.Get(key);
            DockingForm.DockForm.OpenGraphFileFromDatabase(file, false, false);
        }

        private void objectFinderControl1_ViewInContext(MetaBase mbase)
        {
            LiteGraphViewContainer contexter = new LiteGraphViewContainer();
            contexter.Setup(mbase);
            contexter.ShowDialog(this);
        }

        #endregion Methods

    }
}