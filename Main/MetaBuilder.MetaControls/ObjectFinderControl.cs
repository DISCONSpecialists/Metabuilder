using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.DataAccessLayer.OldCode.Diagramming;
using MetaBuilder.Meta;
using TraceTool;
using MetaBuilder.SplashScreen;
using System.Threading;

namespace MetaBuilder.MetaControls
{

    public partial class ObjectFinderControl : UserControl
    {

        private bool orphans;
        public bool Orphans
        {
            get { return orphans; }
            set { orphans = value; }
        }

        private bool rso;
        public bool RSO
        {
            get { return rso; }
            set { rso = value; }
        }

        public bool Server = false;
        private string Provider
        {
            get
            {
                return Server == true ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider;
            }
        }

        #region Fields (9)

        private bool allowSaveAndLoad;
        private bool artefactsOnly;
        const string CONST_NO_DIAGRAMS = "None (Orphaned Objects)";
        [Browsable(false)]
        private List<VCStatusList> excludeStatuses;
        private bool excludeVCItems;
        private bool includeStatusCombo;
        private string limitToClass;
        private int limitToStatus;
        private List<string> validArtefacts;

        public TList<Workspace> ServerWorkspacesUserHasWithAdminPermission;

        #endregion Fields

        #region Constructors (1)

        public ObjectFinderControl(bool server, TList<Workspace> serverAdminWorkspaces)
        {
            InitializeComponent();
            Loader.FlushDataViews();
            objectList1.AllowMultipleSelection = true;
            objectList1.ViewInContext += new ViewInContextEventHandler(objectList1_ViewInContext);
            objectList1.OpenDiagram += new EventHandler(objectList1_OpenDiagram);

            LimitToStatus = -1;
            LimitToClass = null;
            AllowSaveAndLoad = true;
            ExcludeStatuses = new List<VCStatusList>();

            Server = server;
            ServerWorkspacesUserHasWithAdminPermission = serverAdminWorkspaces;

            cbDomainValue.Enabled = false;
            comboDomainValue.Enabled = false;
        }

        #endregion Constructors

        #region Properties (7)

        private bool lfdb = false;
        public bool LFDB
        {
            get { return lfdb; }
            set
            {
                lfdb = value;
                objectList1.LFDB = lfdb;
            }
        }

        bool serverMFD = false;
        public bool ServerMFD
        {
            set
            {
                if (Server)
                {
                    serverMFD = value;
                }
            }
        }

        public bool AllowSaveAndLoad
        {
            get { return allowSaveAndLoad; }
            set { allowSaveAndLoad = value; }
        }

        public bool ArtefactsOnly
        {
            get { return artefactsOnly; }
            set { artefactsOnly = value; }
        }

        [Browsable(false)]
        public List<VCStatusList> ExcludeStatuses
        {
            get
            {
                EnsureExcludeListNotNull();
                return excludeStatuses;
            }
            set
            {
                EnsureExcludeListNotNull();
                excludeStatuses = value;
            }
        }

        public bool ExcludeVCItems
        {
            get { return excludeVCItems; }
            set { excludeVCItems = value; }
        }

        public bool IncludeStatusCombo
        {
            get { return includeStatusCombo; }
            set { includeStatusCombo = value; }
        }

        public string LimitToClass
        {
            get { return limitToClass; }
            set { limitToClass = value; }
        }

        public int LimitToStatus
        {
            get { return limitToStatus; }
            set { limitToStatus = value; }
        }

        #endregion Properties

        #region Delegates and Events (2)


        // Events (2) 

        public event EventHandler OpenDiagram;

        public event ViewInContextEventHandler ViewInContext;


        #endregion Delegates and Events

        #region Methods (13)

        // Public Methods (2) 

        public void DoInitialisation()
        {
            objectList1.ListedObjectsDictionary = new Dictionary<MetaObjectKey, MetaBase>();
            objectList1.SelectedObjectsDictionary = new Dictionary<MetaObjectKey, MetaBase>();
            objectList1.UpdateSearchResult();
            objectList1.UpdateSelection();

            ExcludeStatuses.Add(VCStatusList.Deleted);
            ExcludeStatuses.Add(VCStatusList.PartiallyCheckedIn);
            ExcludeStatuses.Add(VCStatusList.PCI_Revoked);
            ExcludeStatuses.Add(VCStatusList.Skipped);
            ExcludeStatuses.Add(VCStatusList.Obsolete);

            InitDropdowns();
        }

        public void RemoveRow(MetaObjectKey key)
        {
            objectList1.RemoveItem(key);
            objectList1.SelectedObjectsDictionary.Remove(key);
        }

        // Protected Methods (2) 

        protected void OnOpenDiagram(object sender)
        {
            if (OpenDiagram != null)
            {
                OpenDiagram(sender, EventArgs.Empty);
            }
        }

        protected void OnViewInContext(MetaBase mbase)
        {
            if (ViewInContext != null)
                ViewInContext(mbase);
        }

        // Private Methods (9) 

        private void cbStatus_Click(object sender, EventArgs e)
        {
            comboStatus.Enabled = cbStatus.Checked;
        }

        private void EnsureExcludeListNotNull()
        {
            if (excludeStatuses == null)
                excludeStatuses = new List<VCStatusList>();
        }

        private void LoadSelection(SerializableObjectSearchResult result)
        {
            Loader.FlushDataViews();
            if (cbClearResults.Checked)
            {
                objectList1.SelectedObjectsDictionary = new Dictionary<MetaObjectKey, MetaBase>();
                objectList1.ListedObjectsDictionary = new Dictionary<MetaObjectKey, MetaBase>();
            }
            if (objectList1.ListedObjectsDictionary == null)
                objectList1.ListedObjectsDictionary = new Dictionary<MetaObjectKey, MetaBase>();
            if (objectList1.SelectedObjectsDictionary == null)
                objectList1.SelectedObjectsDictionary = new Dictionary<MetaObjectKey, MetaBase>();
            foreach (SerializableObjectSearchResult.ObjectIDentifier identifier in result.ObjectIdentifiers)
            {
                MetaBase mb = Loader.GetByID(identifier.ObjectClass, identifier.Key.pkid, identifier.Key.Machine);
                if (mb.IsInDatabase(Core.Variables.Instance.ClientProvider))
                {
                    if (mb.ToString() != null)
                    {
                        objectList1.AddListedObjectDictionaryItemSafely(identifier.Key, mb);
                        objectList1.AddSelectedObjectDictionaryItemSafely(identifier.Key, mb);
                    }
                }
            }
            objectList1.UpdateSelection();
            objectList1.BindList();
            objectList1.UpdateSearchResult();
        }

        private void ObjectFinder_Load(object sender, EventArgs e)
        {

        }

        private void ObjectFinderControl_Load(object sender, EventArgs e)
        {
            cbStatus.Visible = includeStatusCombo;
            comboStatus.Visible = includeStatusCombo;

            int heightAdjuster = comboStatus.Height;
            if (!includeStatusCombo)
            {
                groupBox1.Height = groupBox1.Height - heightAdjuster;
                panel1.Height = panel1.Height - heightAdjuster;
            }
        }

        void objectList1_OpenDiagram(object sender, EventArgs e)
        {
            OnOpenDiagram(sender);
        }

        void objectList1_ViewInContext(MetaBase mbase)
        {
            OnViewInContext(mbase);
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.InitialDirectory = Variables.Instance.ExportsPath;
            string extension = "mbselxml";
            openDialog.Filter = "Saved Object Selection (*." + extension + ") | *." + extension;
            openDialog.Multiselect = false;
            DialogResult res = openDialog.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                SerializableObjectSearchResult sResult = new SerializableObjectSearchResult();
                sResult.ObjectIdentifiers = new List<SerializableObjectSearchResult.ObjectIDentifier>();
                sResult.Open(openDialog.FileName);
                LoadSelection(sResult);
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            SerializableObjectSearchResult sResult = new SerializableObjectSearchResult();
            sResult.ObjectIdentifiers = new List<SerializableObjectSearchResult.ObjectIDentifier>();
            foreach (KeyValuePair<MetaObjectKey, MetaBase> kvp in objectList1.SelectedObjectsDictionary)
            {
                SerializableObjectSearchResult.ObjectIDentifier identifier = new SerializableObjectSearchResult.ObjectIDentifier();
                identifier.Key = kvp.Key;
                identifier.ObjectClass = kvp.Value._ClassName;
                identifier.StringValue = kvp.Value.ToString();
                sResult.ObjectIdentifiers.Add(identifier);
            }
            SaveFileDialog sfileDialog = new SaveFileDialog();
            sfileDialog.InitialDirectory = Variables.Instance.ExportsPath;
            string extension = "mbselxml";
            sfileDialog.Filter = "Saved Object Selection (*." + extension + ") | *." + extension;
            DialogResult res = sfileDialog.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                sResult.Save(sfileDialog.FileName);
            }
        }

        #endregion Methods

        #region Transitive Return Values
        public bool AllowMultipleSelection
        {
            get { return objectList1.AllowMultipleSelection; }
            set { objectList1.AllowMultipleSelection = value; }
        }
        public Dictionary<MetaObjectKey, MetaBase> SelectedObjects
        {
            get { return objectList1.SelectedObjectsDictionary; }
        }
        public List<MetaBase> SelectedObjectsList
        {
            get
            {
                return objectList1.SelectedObjectsList;
            }
        }
        #endregion

        #region Form Stuff
        private void InitDropdowns()
        {
            comboDomain.Enabled = false;
            comboDomain.Visible = true;
            comboDomain.BringToFront();

            if (Server)
                btnFind.Text += " (Server)";

            comboStatus.DisplayMember = "Caption";
            comboWorkspace.DisplayMember = "Caption";

            if (!Server)
            {
                TList<Workspace> workspaces = DataRepository.Connections[Provider].Provider.WorkspaceProvider.GetAll();
                foreach (Workspace ws in workspaces)
                {
                    DropdownFriendlyWorkspace ddfWS = new DropdownFriendlyWorkspace();
                    ddfWS.WorkspaceTypeId = ws.WorkspaceTypeId;
                    ddfWS.Name = ws.Name;
                    CustomListItem cli = new CustomListItem();
                    cli.Caption = ddfWS.ToString();
                    cli.Tag = ddfWS;
                    comboWorkspace.Items.Add(cli);
                }
            }
            else
            {
                if (ServerWorkspacesUserHasWithAdminPermission != null) //always null when rso
                {
                    foreach (Workspace wsServer in ServerWorkspacesUserHasWithAdminPermission)
                    {
                        DropdownFriendlyWorkspace ddfWS = new DropdownFriendlyWorkspace();
                        ddfWS.WorkspaceTypeId = wsServer.WorkspaceTypeId;
                        ddfWS.Name = wsServer.Name;
                        CustomListItem cli = new CustomListItem();
                        cli.Caption = ddfWS.ToString();
                        cli.Tag = ddfWS;
                        comboWorkspace.Items.Add(cli);
                    }
                }
            }

            TList<Class> classes = DataRepository.Classes(Provider);// Connections[Provider].Provider.ClassProvider.GetAll();
            validArtefacts = new List<string>();

            if (ArtefactsOnly)
            {
                TList<AllowedArtifact> allowedArtefacts = DataRepository.Connections[Provider].Provider.AllowedArtifactProvider.GetAll();
                foreach (AllowedArtifact allowedArt in allowedArtefacts)
                {
                    validArtefacts.Add(allowedArt.Class);
                }
            }
            if (LimitToClass != null && !(LimitToClass.Contains("|"))) //Class is inactive but the allowed artefact is not?
                classes.Filter = "IsActive = 'True'";
            foreach (Class cs in classes)
            {
                if (LimitToClass == null)
                {
                    if (ArtefactsOnly)
                    {
                        if (validArtefacts.Contains(cs.Name) && cs.IsActive == true) //Active only
                            comboClass.Items.Add(cs);
                    }
                    else
                    {
                        if (cs.IsActive == true)
                            comboClass.Items.Add(cs);
                    }
                }
                else if (LimitToClass == cs.Name && cs.IsActive == true) //Active only
                {
                    comboClass.Items.Add(cs);
                    comboClass.Text = cs.Name;
                    cbClass.Checked = true;
                    cbClass.Enabled = false;
                }
                else if (LimitToClass.Contains(cs.Name))//Class is inactive but the allowed artefact is not?
                {
                    comboClass.Items.Add(cs);
                    comboClass.Text = cs.Name;
                    cbClass.Checked = true;
                    cbClass.Enabled = false;
                    comboClass.Enabled = true;
                }
            }

            btnSelectAll.Enabled = AllowMultipleSelection;
            if (LimitToStatus == -1)
            {
                foreach (VCStatusList status in Enum.GetValues(typeof(VCStatusList)))
                {
                    if (!ExcludeStatuses.Contains(status))
                    {
                        CustomListItem clitemStatus = new CustomListItem();
                        clitemStatus.Tag = status;
                        clitemStatus.Caption = ((VCStatusList)status).ToString();
                        comboStatus.Items.Add(clitemStatus);
                    }
                }
                cbStatus.Enabled = true;
                comboStatus.Enabled = false;
            }
            else
            {
                CustomListItem clitemStatusLimited = new CustomListItem();
                clitemStatusLimited.Tag = limitToStatus;
                clitemStatusLimited.Caption = ((VCStatusList)limitToStatus).ToString();
                comboStatus.Items.Add(clitemStatusLimited);
                comboStatus.SelectedIndex = 0;
                cbStatus.Checked = true;
                cbStatus.Enabled = false;
                comboStatus.Enabled = false;
            }

            if (RSO)
            {
                cbWorkspace.Enabled = cbStatus.Enabled = false;
                ExcludeStatuses = new List<VCStatusList>();
                ExcludeStatuses.Add(VCStatusList.MarkedForDelete);
                ExcludeStatuses.Add(VCStatusList.None);
            }

            UpdateFileList();
            SelectFirstItems();

            if (!Core.Variables.Instance.IsServer)
            {
                cbStatus.Visible = false;
                comboStatus.Visible = false;
            }
        }

        private void SelectFirstItems()
        {
            /*
                        if (comboWorkspace.Items.Count > 0 && comboWorkspace.SelectedIndex == -1)
                            comboWorkspace.SelectedIndex = 0;

                        if (comboClass.Items.Count > 0 && comboClass.SelectedIndex == -1)
                            comboClass.SelectedIndex = 0;

                        if (comboDiagram.Items.Count > 0 && comboDiagram.SelectedIndex ==-1)
                            comboDiagram.SelectedIndex = 0;*/
        }
        private void UpdateFileList()
        {
            comboDiagram.Items.Clear();

            TList<GraphFile> files;
            TempFileGraphAdapter tgfa = new TempFileGraphAdapter();
            if (cbWorkspace.Checked)
            {
                CustomListItem cli = comboWorkspace.SelectedItem as CustomListItem;

                if (cli != null)
                {
                    DropdownFriendlyWorkspace ddfws = cli.Tag as DropdownFriendlyWorkspace;
                    files = tgfa.GetFilesByWorkspaceTypeIdWorkspaceName(ddfws.WorkspaceTypeId, ddfws.Name, (int)FileTypeList.Diagram, Server);
                }
                else
                {
                    files = tgfa.GetFilesByTypeID((int)FileTypeList.Diagram, Server);
                }
            }
            else
            {
                files = tgfa.GetFilesByTypeID((int)FileTypeList.Diagram, Server);
            }

            GraphFile fileNone = new GraphFile();
            fileNone.Name = CONST_NO_DIAGRAMS;
            comboDiagram.Items.Add(fileNone);
            if (!Orphans)
            {
                foreach (GraphFile file in files)
                {
                    try
                    {
                        // temporarily change the file name for display purposes
                        if (file.IsActive)
                        {
                            GraphFile addFile = file;
                            addFile.Name = strings.GetFileNameOnly(file.Name);
                            comboDiagram.Items.Add(addFile);
                        }
                    }
                    catch (Exception x)
                    {
                        // Console.WriteLine(x.ToString());
                    }
                }
            }
            else
            {
                cbDiagram.Enabled = false;
                comboDiagram.Enabled = false;
                cbDiagram.Checked = true;
                comboDiagram.SelectedIndex = 0;

                cbStatus.Enabled = false;
                comboStatus.Enabled = false;
            }

            //if (serverMFD)
            //{
            //    comboDiagram.SelectedIndex = 0;
            //    comboDiagram.Enabled = false;
            //}
        }

        private void comboWorkspace_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFileList();

        }
        private void comboClass_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            cbDomainValue.Checked = false;
            comboDomain.Enabled = false;
            comboDomain.Items.Clear();
            comboDomainValue.Enabled = false;
            comboDomainValue.Tag = null;

            TList<DomainDefinitionPossibleValue> values = new TList<DomainDefinitionPossibleValue>();
            DomainDefinitionPossibleValue x = new DomainDefinitionPossibleValue();
            x.PossibleValue = "None";
            values.Add(x);
            cbDomainValue.Text = "Not Defined";
            cbDomainValue.Enabled = false;
            comboDomainValue.Enabled = false;
            //get the domain value object for this class
            if (comboClass.SelectedItem != null)
            {
                TList<Field> fields = DataRepository.ClientFieldsByClass((comboClass.SelectedItem as Class).Name.ToString());
                fields.Sort("SortOrder");
                foreach (Field f in fields)
                {
                    try
                    {
                        if (f.DataType != "IsBusinessExternal" && f.DataType != "GapType" && f.IsActive == true) //f.datatype.contains("Type")
                        {
                            if (int.Parse(DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ExecuteScalar(CommandType.Text, "SELECT COUNT(Name) FROM DomainDefinition WHERE Name = '" + f.DataType + "'").ToString()) == 0)
                                continue;
                            DomainDefinition def = DataRepository.DomainDefinitionProvider.Find("Name = '" + f.DataType + "'")[0];
                            comboDomain.Items.Add(def);
                            comboDomain.DisplayMember = "Name";

                            if (def.IsActive == true)
                            {
                                cbDomainValue.Text = f.DataType;
                                comboDomainValue.Tag = f;
                                cbDomainValue.Enabled = true;

                                foreach (DomainDefinitionPossibleValue val in DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.DomainDefinitionPossibleValueProvider.GetByDomainDefinitionID(def.pkid))
                                {
                                    if (val.IsActive == true)
                                        values.Add(val);
                                }
                            }
                            if (!Core.Variables.Instance.IsDeveloperEdition)
                                break;
                        }
                    }
                    catch
                    {

                    }
                }

                comboDomainValue.DataSource = values;
                comboDomainValue.SelectedItem = null;

                comboDomainValue.DropDownStyle = ComboBoxStyle.DropDown;
                comboDomainValue.AutoCompleteMode = AutoCompleteMode.Suggest;
            }
            else
            {
                cbDomainValue.Text = "Class Type";
            }
        }

        private void comboDomain_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //throw new System.Exception("The method or operation is not implemented.");
            DomainDefinition def = comboDomain.SelectedItem as DomainDefinition;
            if (def != null)
            {
                TList<DomainDefinitionPossibleValue> values = new TList<DomainDefinitionPossibleValue>();
                foreach (DomainDefinitionPossibleValue val in DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.DomainDefinitionPossibleValueProvider.GetByDomainDefinitionID(def.pkid))
                {
                    if (val.IsActive == true)
                        values.Add(val);
                }

                comboDomainValue.DataSource = values;
                comboDomainValue.SelectedItem = null;

                comboDomainValue.DropDownStyle = ComboBoxStyle.DropDown;
                comboDomainValue.AutoCompleteMode = AutoCompleteMode.Suggest;
            }
        }

        private void cbWorkspace_CheckedChanged(object sender, EventArgs e)
        {
            comboWorkspace.Enabled = cbWorkspace.Checked;
            UpdateFileList();
            comboWorkspace.SelectedItem = null;
        }
        private void cbDiagram_CheckedChanged(object sender, EventArgs e)
        {
            comboDiagram.Enabled = cbDiagram.Checked;
            comboDiagram.SelectedItem = null;
        }
        private void cbClass_CheckedChanged(object sender, EventArgs e)
        {
            if (cbClass.Checked == false)
            {
                cbDomainValue.Enabled = false;
                cbDomainValue.Checked = false;
                comboDomainValue.Enabled = false;
                cbDomainValue.Text = "Class Type";
                comboDomainValue.SelectedItem = null;
            }
            comboClass.Enabled = cbClass.Checked;
            comboClass.SelectedItem = null;
        }
        private void cbDomainValue_CheckedChanged(object sender, EventArgs e)
        {
            comboDomainValue.Enabled = cbDomainValue.Checked;
            comboDomainValue.SelectedItem = null;

            comboDomain.Enabled = cbDomainValue.Checked;
            comboDomain.SelectedItem = null;

            if (comboDomain.Items.Count > 0)
                comboDomain.SelectedIndex = 0;
        }

        public void Confirm()
        {
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            FindThread();
        }

        private void Find()
        {
            //PleaseWait.ShowPleaseWaitForm();
            //PleaseWait.SetStatus("Fetching data...");
            //Cursor = Cursors.WaitCursor;
            FillData();
            //objectList1.BindList();
            //Cursor = Cursors.Default;
            //PleaseWait.CloseForm();
        }

        //InThreadVariables
        bool filterClass = false;
        bool filterWorkspace = false;
        bool filterDiagram = false;
        bool filterExcludeStatuses = false;
        bool filterStatus = false;
        bool filterStringValue = false;
        bool clearResults = false;
        //bool classDomain = false;

        string str_value = "";
        string str_class = "";
        string str_domain = "";
        GraphFile file = null;
        VCStatusList filterForStatus = VCStatusList.Deleted;
        DropdownFriendlyWorkspace ddfWS = null;

        private void FindThread()
        {
            cancelThread = false;
            buttonEnabled(false);

            #region filter Variables

            filterClass = cbClass.Checked && comboClass.Text.Length > 0;
            if (filterClass)
            {
                str_class = comboClass.Text;
                if (cbDomainValue.Checked)
                {
                    if (comboDomainValue.SelectedIndex > 0)
                    {
                        if (comboDomainValue.Tag is Field)
                        {
                            //Field f = (comboDomainValue.Tag as Field);
                            DomainDefinitionPossibleValue v = (comboDomainValue.SelectedItem as DomainDefinitionPossibleValue);
                            str_domain = v.PossibleValue;
                        }
                    }
                    else
                        str_domain = comboDomainValue.Text;
                }
            }
            else
            {
                str_class = "";
                str_domain = "";
            }
            filterWorkspace = cbWorkspace.Checked && comboWorkspace.Text.Length > 0;
            if (filterWorkspace)
            {
                CustomListItem cli = comboWorkspace.SelectedItem as CustomListItem;
                if (cli != null)
                {
                    ddfWS = cli.Tag as DropdownFriendlyWorkspace;
                }
            }
            else
            {
                ddfWS = null;
            }
            filterDiagram = cbDiagram.Checked && comboDiagram.Text.Length > 0;
            if (filterDiagram)
                file = comboDiagram.SelectedItem as GraphFile;
            else
                file = null;
            filterExcludeStatuses = ExcludeStatuses.Count > 0;
            filterStatus = cbStatus.Checked && comboStatus.Text.Length > 0;
            if (filterStatus)
            {
                CustomListItem clitem = comboStatus.SelectedItem as CustomListItem;
                filterForStatus = (VCStatusList)clitem.Tag;
            }
            else
                filterForStatus = VCStatusList.Deleted;
            filterStringValue = txtSearchValue.Text.Length > 0;
            if (filterStringValue)
                str_value = txtSearchValue.Text;
            else
                str_value = "";
            clearResults = cbClearResults.Checked;

            #endregion

            Thread t = new Thread(new ThreadStart(Find));
            t.Start();
        }

        bool cancelThread = false;
        public void CancelThread()
        {
            cancelThread = true;
            Thread.Sleep(50);
            buttonEnabled(true);
            Report(0, 0, "");
        }
        delegate void buttonEnabledDelegate(bool enabled);
        private void buttonEnabled(bool enabled)
        {
            if (InvokeRequired)
                BeginInvoke(new buttonEnabledDelegate(buttonEnabled), new object[] { enabled });
            else
                btnFind.Enabled = enabled;
        }
        delegate void ReportDelegate(int total, int progress, string message);
        private void Report(int total, int progress, string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ReportDelegate(Report), new object[] { total, progress, message });
            }
            else
            {
                progressBarFind.Maximum = total;
                progressBarFind.Value = progress;
                labelProgress.Text = message;
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            objectList1.SelectAll();
        }

        private void btnSelectNone_Click(object sender, EventArgs e)
        {
            objectList1.SelectNone();
        }

        #endregion

        #region Data Events

        private void FillData()
        {
            if (clearResults)
                objectList1.ListedObjectsDictionary = new Dictionary<MetaObjectKey, MetaBase>();

            if (objectList1.SelectedObjectsDictionary == null)
                objectList1.SelectedObjectsDictionary = new Dictionary<MetaObjectKey, MetaBase>();

            Report(0, 0, "Filtering");

            #region Data Retrieval & Filter  - this might be speeded up by using stored procs

            TList<MetaObject> objects;
            List<MetaObject> objectsToRemove = new List<MetaObject>();

            List<string> filters = new List<string>();
            //if (filterStringValue)
            //{
            //    filters.Add(" and cast(pkid as varchar(20)) + '|' + Machine in (select cast(ObjectID as varchar(20)) + '|' + MachineID from ObjectFieldValue where ValueString like '%" + strings.replaceApostrophe(str_value) + "%')");
            //}

            if (filterClass)
            {
                filters.Add(" AND CLASS='" + str_class + "' ");
                if (str_domain.Length > 0)
                    filters.Add(" and cast(pkid as varchar(20)) + '|' + Machine in (select cast(ObjectID as varchar(20)) + '|' + MachineID from ObjectFieldValue where ValueString = '" + str_domain + "')");
            }
            if (filterWorkspace && ddfWS != null && !RSO)
            {
                filters.Add(" AND WorkspaceName = '" + ddfWS.Name + "' and WorkspaceTypeId=" + ddfWS.WorkspaceTypeId.ToString());
            }
            else if (Server)
            {
                if (ServerWorkspacesUserHasWithAdminPermission != null || ServerWorkspacesUserHasWithAdminPermission.Count != 0)
                    if (ServerWorkspacesUserHasWithAdminPermission.Count == 1)
                    {
                        filters.Add(" AND WorkspaceName = '" + ServerWorkspacesUserHasWithAdminPermission[0].Name + "' and WorkspaceTypeId=" + ServerWorkspacesUserHasWithAdminPermission[0].WorkspaceTypeId.ToString());
                    }
                    else
                    {
                        filters.Add(" AND ( ");
                        int wsItem = 0;
                        foreach (Workspace ws in ServerWorkspacesUserHasWithAdminPermission)
                        {
                            string serverWsFilterString = " ( WorkspaceName = '" + ws.Name + "' and WorkspaceTypeId=" + ws.WorkspaceTypeId.ToString() + " )";
                            if (wsItem != ServerWorkspacesUserHasWithAdminPermission.Count - 1)
                            {
                                //is last item and must not have or
                                serverWsFilterString += " OR ";
                            }
                            wsItem++;
                            filters.Add(serverWsFilterString);
                        }
                        filters.Add(" ) ");
                    }
            }
            else if (RSO)
            {
                filters.Add(" AND WorkspaceTypeId = 3 ");
            }
            if (filterDiagram)
            {
                if (file != null)
                {
                    if (file.Name == CONST_NO_DIAGRAMS)
                    {
                        // gets objects that dont exist on ACTIVE diagrams
                        filters.Add("and cast(pkid as varchar(20)) + '|' + Machine NOT in (SELECT     CAST(dbo.GraphFileObject.MetaObjectID AS varchar(20)) + '|' + dbo.GraphFileObject.MachineID AS Expr1 FROM dbo.GraphFile INNER JOIN dbo.GraphFileObject ON dbo.GraphFile.pkid = dbo.GraphFileObject.GraphFileID AND dbo.GraphFile.Machine = dbo.GraphFileObject.GraphFileMachine WHERE     (dbo.GraphFile.IsActive = 'True')) ");
                    }
                    else
                    {
                        filters.Add(" and cast(pkid as varchar(20)) + '|' + Machine in (select cast(metaObjectID as varchar(20)) + '|' + MachineID from GraphFileObject where GraphFileID=" + file.pkid.ToString() + " and GraphFileMachine='" + file.Machine + "')");
                    }
                }
            }

            if (filterExcludeStatuses)
            {
                foreach (VCStatusList excludeStatusItem in ExcludeStatuses)
                {
                    filters.Add(" AND NOT VCStatusID=" + ((int)excludeStatusItem).ToString());
                }
            }

            if (filterStatus)
            {
                filters.Add(" AND VCStatusID=" + ((int)filterForStatus).ToString());
            }
            //This is removed because it filters out checkedoutMFD Items that may need restoration
            //if (ExcludeVCItems)
            //{
            //filters.Add(" AND VCMachineID is null");
            //}

            filters.Add(" AND CLASS IN (SELECT NAME FROM CLASS WHERE ISACTIVE = 'True')");

            #endregion

            ObjectHelper objHelper = new ObjectHelper(Server);
#if DEBUG
            TTrace.Debug.Send("Fetch MetaObject table data START");
#endif
            //select * from metaobject where filters
            DataView dvSearchResult = null;
            try
            {
                dvSearchResult = objHelper.GetObjectsFiltered(filters, Server);
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog(ex.ToString());
            }
#if DEBUG
            TTrace.Debug.Send("Fetch MetaObject table data END");
            TTrace.Debug.Send("Instantiate MetaObjects START");
            TTrace.Debug.Indent("Starting");
#endif
            int counter = 0;
            Dictionary<string, List<MetaKey>> itemsToRetrieve = new Dictionary<string, List<MetaKey>>();
            Report(dvSearchResult.Count, counter, "Searching");
            if (dvSearchResult != null)
            {
                foreach (DataRowView drvObject in dvSearchResult)
                {
                    counter += 1;
                    Report(dvSearchResult.Count, counter, "Searching");
                    if (cancelThread)
                        return;
                    string classname = drvObject["Class"].ToString();

                    List<MetaKey> keys = new List<MetaKey>();
                    if (!itemsToRetrieve.ContainsKey(classname))
                    {
#if DEBUG
                        TTrace.Debug.Send(classname);
#endif
                        itemsToRetrieve.Add(classname, keys);
                    }
                    else
                    {
                        keys = itemsToRetrieve[classname];
                    }
                    MetaKey mkey = new MetaKey();
                    mkey.PKID = int.Parse(drvObject["pkid"].ToString());
                    mkey.Machine = drvObject["Machine"].ToString();
                    keys.Add(mkey);
                }
            }
#if DEBUG
            TTrace.Debug.UnIndent();
            TTrace.Debug.Send("Instantiate MetaObjects END");
            TTrace.Debug.Send("Load MB Object Values START");
#endif
            List<MetaBase> loaded = Loader.GetFromProvider(itemsToRetrieve, Server);
#if DEBUG
            TTrace.Debug.Indent("Count:" + loaded.Count.ToString());
#endif
            counter = 0;
            Report(loaded.Count + 2, counter, "Loading");
            foreach (MetaBase mbase in loaded)
            {
                if (cancelThread)
                    return;

                counter += 1;
                if (filterStringValue)
                    if (!(mbase.ToString().ToLower().Contains(str_value.ToLower())))
                        continue;

                Report(loaded.Count + 2, counter, "Loading");
                if (RSO)
                {
                    MetaObject serverObject = null;
                    try
                    {
                        //Report(loaded.Count + 2, counter, "Loading [Server]");
                        serverObject = DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.MetaObjectProvider.GetBypkidMachine(mbase.pkid, mbase.MachineName);
                    }
                    catch
                    {
                    }
                    //skip when object exists
                    if (serverObject != null)
                    {
                        continue;
                    }
                }

                if (cancelThread)
                    return;
                if (ArtefactsOnly)
                {
                    if (validArtefacts.Contains(mbase._ClassName))
                    {
                        objectList1.AddListedObjectDictionaryItemSafely(new MetaObjectKey(mbase.pkid, mbase.MachineName), mbase);
                    }
                }
                else
                {
                    objectList1.AddListedObjectDictionaryItemSafely(new MetaObjectKey(mbase.pkid, mbase.MachineName), mbase);
                }
            }
#if DEBUG
            TTrace.Debug.Send("Load MB Object Values END");
            TTrace.Debug.Send("Refresh Grid START");
#endif

            Report(0, 0, "Compiling");
            //if (objectList1.ListedObjectsDictionary.Count == 0)
            objectList1.USR();
#if DEBUG
            TTrace.Debug.Send("Refresh Grid END");
#endif

            Report(0, 0, "");
            buttonEnabled(true);
        }
        #endregion

        private void txtSearchValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                FindThread();
        }
    }
}