using System;
using System.ComponentModel;
using System.Windows.Forms;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.DataAccessLayer.OldCode.Diagramming;
using MetaBuilder.Meta;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.MetaControls
{


    public partial class ObjectProperties : UserControl
    {

        #region Fields (1)

        private MetaBase myMetaObject;

        #endregion Fields

        #region Constructors (1)

        private string Provider { get { return Server ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider; } }
        private bool Server = false;
        public ObjectProperties(bool server)
        {
            Server = server;
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties (1)

        [Browsable(false)]
        public MetaBase MyMetaObject
        {
            get { return myMetaObject; }
            set
            {
                myMetaObject = value;
                if (value != null)
                {
                    BindInterface();
                    btnViewInContext.Visible = (myMetaObject.pkid > 0 && myMetaObject.MachineName != null);
                }
            }
        }

        #endregion Properties

        #region Delegates and Events (2)

        // Events (2) 

        [Browsable(false)]
        public event EventHandler OpenDiagram;

        [Browsable(false)]
        public event ViewInContextEventHandler ViewInContext;

        #endregion Delegates and Events

        #region Methods (6)

        // Protected Methods (2) 

        protected void OnOpenDiagram()
        {
            if (OpenDiagram != null)
            {
                if (listDiagrams.SelectedItem != null)
                {
                    CustomListItem clitem = listDiagrams.SelectedItem as CustomListItem;
                    if (clitem != null)
                    {
                        GraphFileKey fileKey = new GraphFileKey(clitem.Tag as GraphFile);
                        OpenDiagram(fileKey, EventArgs.Empty);
                    }
                }
            }
        }

        protected void OnViewInContext(MetaBase mbase)
        {
            if (ViewInContext != null)
                ViewInContext(mbase);
        }

        // Private Methods (4) 

        private void BindInterface()
        {
            TList<GraphFileObject> graphFileObjects = DataRepository.Connections[Provider].Provider.GraphFileObjectProvider.GetByMetaObjectIDMachineID(myMetaObject.pkid, myMetaObject.MachineName);
            int counter = 0;
            TempFileGraphAdapter tfga = new TempFileGraphAdapter();
            listDiagrams.Items.Clear();
            foreach (GraphFileObject gfo in graphFileObjects)
            {
                GraphFile quickDetails = tfga.GetQuickFileDetails(gfo.GraphFileID, gfo.GraphFileMachine, Server);
                if (!VCStatusTool.IsObsoleteOrMarkedForDelete(quickDetails))
                {
                    CustomListItem clitem = new CustomListItem();
                    clitem.Caption = strings.GetFileNameOnly(quickDetails.Name);
                    clitem.Tag = quickDetails;
                    if (quickDetails.IsActive)
                    {
                        counter++;
                        listDiagrams.Items.Add(clitem);
                    }
                }
            }
            if (listDiagrams.Items.Count > 0)
            {
                listDiagrams.SelectedIndex = 0;
                btnOpenSelectedDiagrams.Enabled = true;
            }
            else
            {
                btnOpenSelectedDiagrams.Enabled = false;
            }
            listDiagrams.DisplayMember = "Caption";
            lblDiagrams.Text = counter.ToString();

            TList<Artifact> asArtefact = DataRepository.Connections[Provider].Provider.ArtifactProvider.GetByArtifactObjectIDArtefactMachine(myMetaObject.pkid, myMetaObject.MachineName);
            lblAsArtefact.Text = asArtefact.Count.ToString();

            TList<ObjectAssociation> associationsTo = DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.GetByChildObjectIDChildObjectMachine(myMetaObject.pkid, myMetaObject.MachineName);
            TList<ObjectAssociation> associationsFrom = DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(myMetaObject.pkid, myMetaObject.MachineName);
            lblAsParent.Text = associationsFrom.Count.ToString();
            lblAsChild.Text = associationsTo.Count.ToString();
            lblClass.Text = myMetaObject._ClassName;
            lblPKID.Text = myMetaObject.pkid.ToString();
            lblMachine.Text = myMetaObject.MachineName;
            lblWorkspace.Text = myMetaObject.WorkspaceName;
            lblStatus.Text = myMetaObject.state.ToString();
            metaPropertyGrid1.SelectedObject = myMetaObject;
            metaPropertyGrid1.Enabled = false;

            //tabOverview.SelectedTab = tabPageProperties;
        }

        private void btnOpenSelectedDiagrams_Click(object sender, EventArgs e)
        {
            OnOpenDiagram();
        }

        private void btnViewInContext_Click(object sender, EventArgs e)
        {
            if (myMetaObject.pkid > 0 && myMetaObject.MachineName != null)
            {
                try
                {
                    OnViewInContext(myMetaObject);
                }
                catch
                {
                }
            }
        }

        private void grpGraphFiles_Enter(object sender, EventArgs e)
        {

        }

        #endregion Methods

    }
}
