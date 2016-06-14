using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.DataAccessLayer.OldCode.Diagramming;
using MetaBuilder.Meta;
using MetaBuilder.SplashScreen;
using System.Threading;
using ObjectAssociation = MetaBuilder.BusinessLogic.ObjectAssociation;

namespace MetaBuilder.MetaControls
{
    public partial class AssociationLocatorControl : UserControl
    {

        #region Fields (6)

        private bool allowSaveAndLoad;
        TList<ClassAssociation> classAssocs;
        const string CONST_NO_DIAGRAMS = "None (Orphaned Associations)";
        [Browsable(false)]
        private List<VCStatusList> excludeStatuses;
        private bool excludeVCItems;
        private bool includeStatusCombo;

        #endregion Fields

        private string ProviderString()
        {
            return Server ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString;
        }
        private string Provider()
        {
            return Server ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider;
        }

        #region Constructors (1)
        bool Server = false;
        public AssociationLocatorControl(bool server)
        {
            Server = server;
            InitializeComponent();
            AllowMultipleSelection = true;
            LimitToStatus = -1;// VCStatusList.None;
            excludeStatuses = new List<VCStatusList>();
            classAssocs = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetAll();
        }

        #endregion Constructors

        #region Properties (7)

        public bool AllowMultipleSelection
        {
            get { return associationList2.AllowMultipleSelection; }
            set { associationList2.AllowMultipleSelection = value; }
        }

        public bool AllowSaveAndLoad
        {
            get { return allowSaveAndLoad; }
            set { allowSaveAndLoad = value; }
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

        public int LimitToStatus
        {
            get { return associationList2.LimitToStatus; }
            set { associationList2.LimitToStatus = value; }
        }

        public Dictionary<ObjectAssociationKey, ObjectAssociationInclObj> SelectedAssociations
        {
            get { return associationList2.SelectedAssociations; }
            set { associationList2.SelectedAssociations = value; }
        }

        #endregion Properties

        #region Methods (15)


        // Public Methods (2) 

        public void DoInitialising()
        {
            associationList2.SelectedAssociations = new Dictionary<ObjectAssociationKey, ObjectAssociationInclObj>();
            associationList2.UpdateSearchResult();
            associationList2.UpdateSelection();
            InitDropdowns();
        }

        public void RemoveRow(ObjectAssociationKey key)
        {
            associationList2.RemoveItem(key);
        }

        // Private Methods (13) 

        private void AssociationLocatorControl_Load(object sender, EventArgs e)
        {
            cbStatus.Visible = includeStatusCombo;
            comboStatus.Visible = includeStatusCombo;
            int heightAdjuster = comboStatus.Height;
            if (!includeStatusCombo)
            {
                groupBox1.Height = groupBox1.Height - heightAdjuster;
                this.panelTop.Height = panelTop.Height - heightAdjuster;
            }
        }

        private void btnSelectNone_Click(object sender, EventArgs e)
        {
            associationList2.SelectNone();
        }

        private void cbStatus_CheckedChanged(object sender, EventArgs e)
        {
            comboStatus.Enabled = cbStatus.Checked;
        }

        private void cbStatus_CheckedChanged_1(object sender, EventArgs e)
        {
            comboStatus.Enabled = cbStatus.Checked;
        }

        private void cbType_CheckedChanged(object sender, EventArgs e)
        {
            comboAssociationType.Enabled = cbType.Checked;
            ResetClassAssociationDropdown();
            UpdateClassAssociationList();
        }

        private void comboAssociationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetClassAssociationDropdown();
            UpdateClassAssociationList();
        }

        private void comboClass_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void EnsureExcludeListNotNull()
        {
            if (excludeStatuses == null)
                excludeStatuses = new List<VCStatusList>();
        }

        private ClassAssociation GetClassAssociation(int CAid)
        {
            classAssocs.Filter = "CAid = " + CAid.ToString();
            return classAssocs[0];
        }

        private void LoadSelection(SerializableAssociationSearchResult result)
        {
            if (cbClearResults.Checked)
            {
                associationList2.SelectedAssociations = new Dictionary<ObjectAssociationKey, ObjectAssociationInclObj>();
                associationList2.ListedAssociations = new Dictionary<ObjectAssociationKey, ObjectAssociationInclObj>();
            }
            foreach (SerializableAssociationSearchResult.AssociationIdentifier identifier in result.AssociationIdentifiers)
            {

                if (associationList2.SelectedAssociations == null)
                    associationList2.SelectedAssociations = new Dictionary<ObjectAssociationKey, ObjectAssociationInclObj>();
                if (associationList2.ListedAssociations == null)
                    associationList2.ListedAssociations = new Dictionary<ObjectAssociationKey, ObjectAssociationInclObj>();

                MetaBase mbFrom = Loader.GetFromProvider(identifier.Key.ObjectID, identifier.Key.ObjectMachine, identifier.FromObjectClass, Server);
                MetaBase mbTO = Loader.GetFromProvider(identifier.Key.ChildObjectID, identifier.Key.ChildObjectMachine, identifier.ToObjectClass, Server);
                ObjectAssociationInclObj incl = new ObjectAssociationInclObj();
                incl.FromObject = mbFrom;
                incl.ToObject = mbTO;
                incl.ObjectAss = new ObjectAssociation();
                incl.ObjectAss.CAid = identifier.Key.CAid;
                incl.ObjectAss.ObjectID = identifier.Key.ObjectID;
                incl.ObjectAss.ObjectMachine = identifier.Key.ObjectMachine;
                incl.ObjectAss.ChildObjectID = identifier.Key.ChildObjectID;
                incl.ObjectAss.ChildObjectMachine = identifier.Key.ChildObjectMachine;

                //   if (mbFrom.ToString() != null && mbTO.ToString()!=null)
                if (!(associationList2.ListedAssociations.ContainsKey(identifier.Key)))
                    associationList2.ListedAssociations.Add(identifier.Key, incl);

                if (!(associationList2.SelectedAssociations.ContainsKey(identifier.Key)))
                    associationList2.SelectedAssociations.Add(identifier.Key, incl); ;
            }
            associationList2.UpdateSelection();
            associationList2.BindList();
            associationList2.UpdateSearchResult();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.InitialDirectory = Variables.Instance.ExportsPath;
            string extension = "mbacxml";
            openDialog.Filter = "Saved Association Selection (*." + extension + ") | *." + extension;
            openDialog.Multiselect = false;
            DialogResult res = openDialog.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                SerializableAssociationSearchResult sResult = new SerializableAssociationSearchResult();
                sResult.AssociationIdentifiers = new List<SerializableAssociationSearchResult.AssociationIdentifier>();
                sResult.Open(openDialog.FileName);
                LoadSelection(sResult);
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            SerializableAssociationSearchResult sResult = new SerializableAssociationSearchResult();
            sResult.AssociationIdentifiers = new List<SerializableAssociationSearchResult.AssociationIdentifier>();
            foreach (KeyValuePair<ObjectAssociationKey, ObjectAssociationInclObj> kvp in associationList2.SelectedAssociations)
            {
                SerializableAssociationSearchResult.AssociationIdentifier ident = new SerializableAssociationSearchResult.AssociationIdentifier();
                ident.Key = kvp.Key;
                ident.AssociationDescription = ((AssociationTypeList)GetClassAssociation(kvp.Key.CAid).AssociationTypeID).ToString();
                ident.FromObjectClass = kvp.Value.FromObject._ClassName;
                ident.ToObjectClass = kvp.Value.ToObject._ClassName;
                ident.FromObjectString = kvp.Value.FromObject.ToString();
                ident.ToObjectString = kvp.Value.ToObject.ToString();
                sResult.AssociationIdentifiers.Add(ident);
            }
            SaveFileDialog sfileDialog = new SaveFileDialog();
            sfileDialog.InitialDirectory = Variables.Instance.ExportsPath;
            string extension = "mbacxml";
            sfileDialog.Filter = "Saved Association Selection (*." + extension + ") | *." + extension;
            DialogResult res = sfileDialog.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                sResult.Save(sfileDialog.FileName);
            }
        }

        private void UpdateClassAssociationList()
        {
            if (comboAssociationType.SelectedItem != null && cbType.Checked)
            {
                CustomListItem clItem = comboAssociationType.SelectedItem as CustomListItem;

                AssociationType assocType = clItem.Tag as AssociationType;

                if (assocType != null)
                {
                    List<CustomListItem> itemsToRemove = new List<CustomListItem>();
                    for (int i = 0; i < comboClass.Items.Count; i++)
                    {
                        CustomListItem clAssoc = comboClass.Items[i] as CustomListItem;
                        ClassAssociation assoc = clAssoc.Tag as ClassAssociation;
                        if (assoc.AssociationTypeID != assocType.pkid)
                        {
                            itemsToRemove.Add(clAssoc);

                        }
                    }
                    for (int i = 0; i < itemsToRemove.Count; i++)
                    {
                        comboClass.Items.Remove(itemsToRemove[i]);
                    }
                }
                else
                {
                    // Get Orphans!

                }
            }
        }

        #endregion Methods

        #region Form Stuff

        private void InitDropdowns()
        {
            if (Server)
                Text += " (Server)";

            UpdateFileList();
            TList<AssociationType> assocTypes = DataRepository.Connections[Provider()].Provider.AssociationTypeProvider.GetAll();
            assocTypes.Sort("Name");
            foreach (AssociationType assocType in assocTypes)
            {
                CustomListItem clitem = new CustomListItem();
                clitem.Tag = assocType;
                clitem.Caption = assocType.Name;
                comboAssociationType.Items.Add(clitem);
            }

            CustomListItem clitemOrphans = new CustomListItem();
            clitemOrphans.Caption = "Orphaned";
            comboAssociationType.Items.Add(clitemOrphans);

            comboAssociationType.DisplayMember = "Caption";
            comboClass.DisplayMember = "Caption";
            comboStatus.DisplayMember = "Caption";
            ResetClassAssociationDropdown();
            comboStatus.Items.Clear();
            if (LimitToStatus == -1)
            {
                foreach (VCStatusList status in Enum.GetValues(typeof(VCStatusList)))
                {
                    if (!(ExcludeStatuses.Contains(status)))
                    {
                        CustomListItem clitemStatus = new CustomListItem();
                        clitemStatus.Tag = status;
                        clitemStatus.Caption = status.ToString();
                        comboStatus.Items.Add(clitemStatus);
                    }
                }
            }
            else
            {
                CustomListItem clitemStatusLimited = new CustomListItem();
                clitemStatusLimited.Tag = LimitToStatus;
                clitemStatusLimited.Caption = ((VCStatusList)LimitToStatus).ToString();
                comboStatus.Items.Add(clitemStatusLimited);
                cbStatus.Checked = true;
                cbStatus.Enabled = false;
                comboStatus.SelectedIndex = 0;
            }

        }
        private void UpdateFileList()
        {
            comboDiagram.Items.Clear();
            TempFileGraphAdapter tgfa = new TempFileGraphAdapter();
            TList<GraphFile> files = tgfa.GetFilesByTypeID((int)FileTypeList.Diagram, Server);

            GraphFile fileNone = new GraphFile();
            fileNone.Name = CONST_NO_DIAGRAMS;
            comboDiagram.Items.Add(fileNone);
            foreach (GraphFile file in files)
            {
                try
                {
                    // temporarily change the file name for display purposes
                    if (file.IsActive)
                    {
                        GraphFile addFile = file;
                        addFile.Name = strings.GetFileNameOnly(file.Name);// +" V" + file.MajorVersion.ToString() + "." + file.MinorVersion.ToString();
                        comboDiagram.Items.Add(addFile);
                    }
                }
                catch (Exception x)
                {
                    // Console.WriteLine(x.ToString());
                }
            }
        }
        private void cbDiagram_CheckedChanged(object sender, EventArgs e)
        {
            comboDiagram.Enabled = cbDiagram.Checked;
        }

        private void ResetClassAssociationDropdown()
        {
            TList<ClassAssociation> dropdownclassAssocs = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetAll();
            dropdownclassAssocs.Sort("ParentClass,ChildClass");

            foreach (ClassAssociation classAssoc in dropdownclassAssocs)
            {
                CustomListItem clitemAssoc = new CustomListItem();
                clitemAssoc.Caption = classAssoc.Caption;
                clitemAssoc.Tag = classAssoc;
                comboClass.Items.Add(clitemAssoc);
            }
        }

        private void cbClass_CheckedChanged(object sender, EventArgs e)
        {
            comboClass.Enabled = cbClass.Checked;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            FindThread();
        }

        private void Find()
        {
            //PleaseWait.ShowPleaseWaitForm();
            //PleaseWait.SetStatus("Fetching data...");
            FillData();
            //associationList2.BindList();
            //PleaseWait.CloseForm();
        }

        ClassAssociation classAssoc = null;
        CustomListItem clitem = null;
        bool clearRestults = false;
        bool typeAssType = false;
        List<int> caids = new List<int>();
        bool diagram = false;
        GraphFile file = null;
        string str_File = "";

        private void FindThread()
        {
            cancelThread = false;
            buttonEnabled(false);

            clearRestults = cbClearResults.Checked;
            if (cbClass.Checked && comboClass.Text.Length > 0)
            {
                CustomListItem clitem = comboClass.SelectedItem as CustomListItem;
                classAssoc = clitem.Tag as ClassAssociation;
            }
            else
            {
                classAssoc = null;
            }
            if (cbStatus.Checked && comboStatus.Text.Length > 0)
            {
                clitem = comboStatus.SelectedItem as CustomListItem;
            }
            else
            {
                clitem = null;
            }
            if (cbType.Checked && comboAssociationType.Text.Length > 0)
            {
                typeAssType = true;
                caids = new List<int>();
                foreach (CustomListItem item in comboClass.Items)
                {
                    caids.Add((item.Tag as ClassAssociation).CAid);
                }
            }
            else
                typeAssType = false;

            if (cbDiagram.Checked)
            {
                if (comboDiagram.Text.Length > 0)
                {
                    if (comboDiagram.Text == CONST_NO_DIAGRAMS)
                    {
                        str_File = comboDiagram.Text;
                    }
                    else
                    {
                        file = comboDiagram.SelectedItem as GraphFile;
                    }
                }
            }
            else
            {
                str_File = "";
                file = null;
            }

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
            {
                btnFind.Enabled = enabled;
                classAssoc = null;
                clitem = null;
            }
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
            associationList2.SelectAll();
        }

        #endregion

        #region Data Events
        private void FillData()
        {
            if (clearRestults)
                associationList2.ListedAssociations = new Dictionary<ObjectAssociationKey, ObjectAssociationInclObj>();
            if (associationList2.SelectedAssociations == null)
                associationList2.SelectedAssociations = new Dictionary<ObjectAssociationKey, ObjectAssociationInclObj>();

            Report(0, 0, "Filtering");

            #region Data Retrieval & Filter  - this might be speeded up by using stored procs
            TList<ObjectAssociation> associations;
            List<ObjectAssociation> assocsToRemove = new List<ObjectAssociation>();
            if (classAssoc != null)
            {
                associations = DataRepository.Connections[Provider()].Provider.ObjectAssociationProvider.GetByCAid(classAssoc.CAid);
            }
            else
            {
                associations = DataRepository.Connections[Provider()].Provider.ObjectAssociationProvider.GetAll();
            }
            if (cancelThread)
                return;
            //ADDED
            //if caid of association not in comboclass at this point then remove it
            if (typeAssType)
            {
                foreach (ObjectAssociation a in associations)
                {
                    if (!caids.Contains(a.CAid))
                        assocsToRemove.Add(a);
                }
            }

            if (clitem != null)
            {
                VCStatusList filterForStatus = (VCStatusList)clitem.Tag;// as ;// (b.VCStatusList)Enum.Parse(typeof(VCStatusList), comboStatus.Text);

                if (filterForStatus != VCStatusList.None)
                {
                    foreach (ObjectAssociation oa in associations)
                    {
                        if (oa.VCStatusID != (int)filterForStatus)
                        {
                            assocsToRemove.Add(oa);
                        }
                    }
                }
            }

            if (ExcludeStatuses.Count > 0)
            {
                foreach (ObjectAssociation oa in associations)
                {
                    if (ExcludeStatuses.Contains((VCStatusList)oa.VCStatusID) && !assocsToRemove.Contains(oa))
                    {
                        assocsToRemove.Add(oa);
                    }
                }
            }

            if (ExcludeVCItems)
            {
                foreach (ObjectAssociation oa in associations)
                {
                    if (oa.VCMachineID != null)
                    {
                        if (oa.VCMachineID.Length > 0)
                        {
                            assocsToRemove.Add(oa);
                        }
                    }
                }
            }

            if (str_File.Length > 0)
            {
                // get list of orphaned assocs
                // if assoc not in list, remove
                AssociationHelper assHelper = new AssociationHelper();
                List<ObjectAssociation> orphans = assHelper.GetOrphanedAssociations();
                foreach (ObjectAssociation oa in associations)
                {
                    bool found = false;
                    foreach (ObjectAssociation orphan in orphans)
                    {
                        if (orphan.CAid == oa.CAid && orphan.ObjectMachine == oa.ObjectMachine && orphan.ChildObjectID == oa.ChildObjectID && orphan.ObjectMachine == oa.ObjectMachine && orphan.ChildObjectMachine == oa.ChildObjectMachine)
                        {
                            found = true;
                        }
                    }

                    ClassAssociation retrievedClassAssoc = GetClassAssociation(oa.CAid);
                    if (retrievedClassAssoc.AssociationTypeID == 4)
                    {
                        // mappings. reverse check!
                        // 1st get the reverse CAid
                        foreach (ClassAssociation mirrorClassAssoc in classAssocs)
                        {
                            if (cancelThread)
                                return;
                            if (mirrorClassAssoc.ParentClass == retrievedClassAssoc.ChildClass && mirrorClassAssoc.ChildClass == retrievedClassAssoc.ParentClass && mirrorClassAssoc.AssociationTypeID == retrievedClassAssoc.AssociationTypeID)
                            {
                                if (cancelThread)
                                    return;
                                foreach (ObjectAssociation orphan in orphans)
                                {
                                    if (orphan.ObjectMachine == oa.ChildObjectMachine && orphan.ChildObjectID == oa.ObjectID && orphan.ObjectMachine == oa.ChildObjectMachine && orphan.ChildObjectMachine == oa.ObjectMachine)
                                    {
                                        found = false;
                                    }

                                    if (orphan.ObjectMachine == oa.ObjectMachine && orphan.ChildObjectID == oa.ChildObjectID && orphan.ObjectMachine == oa.ObjectMachine && orphan.ChildObjectMachine == oa.ChildObjectMachine)
                                    {
                                        found = false;
                                    }
                                }
                            }
                        }

                    }

                    if (!found)
                        assocsToRemove.Add(oa);

                }

            }
            else
            {
                if (file != null)
                {
                    TList<ObjectAssociation> assocsOnDiagram = DataRepository.Connections[Provider()].Provider.ObjectAssociationProvider.GetByGraphFileIDGraphFileMachineFromGraphFileAssociation(file.pkid, file.Machine);
                    RemoveAssociationsNotInTList(associations, assocsToRemove, assocsOnDiagram);
                }
            }

            for (int i = 0; i < assocsToRemove.Count; i++)
            {
                associations.Remove(assocsToRemove[i]);
            }
            assocsToRemove = null;
            #endregion

            // create instances
            int counter = 0;
            Report(associations.Count, counter, "Loading");
            foreach (ObjectAssociation association in associations)
            {
                if (cancelThread)
                    return;
                Report(associations.Count, counter, "Loading");
                counter += 1;
                ClassAssociation thisClassAssoc = GetClassAssociation(association.CAid);
                MetaBase mbaseFrom = Loader.GetFromProvider(association.ObjectID, association.ObjectMachine, thisClassAssoc.ParentClass, Server);
                MetaBase mbaseTo = Loader.GetFromProvider(association.ChildObjectID, association.ChildObjectMachine, thisClassAssoc.ChildClass, Server);
                ObjectAssociationInclObj incl = new ObjectAssociationInclObj();
                incl.ObjectAss = association;
                incl.FromObject = mbaseFrom;
                incl.ToObject = mbaseTo;

                ObjectAssociationKey key = new ObjectAssociationKey(association.CAid, association.ObjectID, association.ChildObjectID, association.ObjectMachine, association.ChildObjectMachine);

                // TODO : REMOVE THIS WHEN BUG FIXED
                //if both objects are readonly dont add the object(this is because somtimes associations are not properly checked out becuase of a bug)
                MetaObject mbfrom = DataRepository.Connections[Provider()].Provider.MetaObjectProvider.GetBypkidMachine(association.ObjectID, association.ObjectMachine);
                MetaObject mbto = DataRepository.Connections[Provider()].Provider.MetaObjectProvider.GetBypkidMachine(association.ChildObjectID, association.ChildObjectMachine);

                bool readFrom = false;
                bool readTo = false;

                if (mbfrom.VCStatusID == (int)VCStatusList.CheckedIn || mbfrom.VCStatusID == (int)VCStatusList.CheckedOutRead || mbfrom.VCStatusID == (int)VCStatusList.Locked || mbfrom.VCStatusID == (int)VCStatusList.Obsolete)
                    readFrom = true;
                if (mbto.VCStatusID == (int)VCStatusList.CheckedIn || mbto.VCStatusID == (int)VCStatusList.CheckedOutRead || mbto.VCStatusID == (int)VCStatusList.Locked || mbto.VCStatusID == (int)VCStatusList.Obsolete)
                    readTo = true;

                if (readFrom && readTo)
                {
                    //dont add it
                }
                else
                    if (!associationList2.ListedAssociations.ContainsKey(key))
                        associationList2.ListedAssociations.Add(key, incl);

            }

            Report(associations.Count, counter, "Compiling");
            //if (associationList2.ListedAssociations.Count > 0)
            associationList2.USR();

            Report(0, 0, "");
            buttonEnabled(true);
        }
        private static void RemoveAssociationsNotInTList(TList<ObjectAssociation> associations, List<ObjectAssociation> assocsToRemove, TList<ObjectAssociation> assocsOnDiagram)
        {
            foreach (ObjectAssociation oa in associations)
            {
                bool found = false;
                foreach (ObjectAssociation orphan in assocsOnDiagram)
                {
                    if (orphan.CAid == oa.CAid && orphan.ObjectMachine == oa.ObjectMachine && orphan.ChildObjectID == oa.ChildObjectID && orphan.ObjectMachine == oa.ObjectMachine && orphan.ChildObjectMachine == oa.ChildObjectMachine)
                    {
                        found = true;
                    }
                }

                if (!found)
                    assocsToRemove.Add(oa);

            }
        }
        #endregion
    }
}