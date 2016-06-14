using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.DataAccessLayer.OldCode.Diagramming;
using MetaBuilder.Meta;
using MetaBuilder.SplashScreen;
using System.Threading;

namespace MetaBuilder.MetaControls
{
    public partial class DuplicateObjectFinderControl : UserControl
    {

        #region Fields (7)

        private bool allowSaveAndLoad;
        private Dictionary<MetaBase, List<MetaBase>> duplicateObjects;
        [Browsable(false)]
        private List<VCStatusList> excludeStatuses;
        private bool excludeVCItems;
        private bool includeStatusCombo;
        private string limitToClass;
        private int limitToStatus;

        public TList<Workspace> ServerWorkspacesUserHasWithAdminPermission;
        #endregion Fields

        #region Constructors (1)
        bool Server = false;
        private string Provider
        {
            get
            {
                return Server == true ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider;
            }
        }
        public DuplicateObjectFinderControl(bool server, TList<Workspace> serverAdminWorkspaces)
        {
            InitializeComponent();

            objectList1.AllowMultipleSelection = true;
            objectList1.ViewInContext += new ViewInContextEventHandler(objectList1_ViewInContext);
            objectList1.OpenDiagram += new EventHandler(objectList1_OpenDiagram);
            LimitToStatus = -1;
            LimitToClass = null;
            AllowSaveAndLoad = true;
            ExcludeStatuses = new List<VCStatusList>();
            openToolStripButton.Visible = false;
            saveToolStripButton.Visible = false;

            Server = server;
            ServerWorkspacesUserHasWithAdminPermission = serverAdminWorkspaces;
        }

        #endregion Constructors

        #region Properties (7)

        public bool AllowSaveAndLoad
        {
            get { return allowSaveAndLoad; }
            set { allowSaveAndLoad = value; }
        }

        public Dictionary<MetaBase, List<MetaBase>> DuplicateObjects
        {
            get { return duplicateObjects; }
        }

        [Browsable(false)]
        public List<VCStatusList> ExcludeStatuses
        {
            get
            {
                if (excludeStatuses == null)
                    excludeStatuses = new List<VCStatusList>();
                return excludeStatuses;
            }
            set { excludeStatuses = value; }
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
            InitDropdowns();
        }

        public void RemoveRow(MetaObjectKey key)
        {
            objectList1.RemoveItem(key);
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

        private void DuplicateObjectFinderControl_Load(object sender, EventArgs e)
        {
            cbStatus.Visible = includeStatusCombo;
            comboStatus.Visible = includeStatusCombo;

            int heightAdjuster = comboStatus.Height;
            if (!includeStatusCombo)
            {
                groupBox1.Height = groupBox1.Height - heightAdjuster;
                panel1.Height = panel1.Height - heightAdjuster;
            }
            duplicateObjects = new Dictionary<MetaBase, List<MetaBase>>();
        }

        private string returnParentStringValueForComplexChild(MetaKey key)
        {
            string provider = Server ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider;
            MetaObject mObj = null;
            //find the first parent of this object which is a complex node
            foreach (BusinessLogic.ObjectAssociation obj in DataRepository.Connections[provider].Provider.ObjectAssociationProvider.GetByChildObjectIDChildObjectMachine(key.PKID, key.Machine))
            {
                mObj = DataRepository.Connections[provider].Provider.MetaObjectProvider.GetBypkidMachine(obj.ObjectID, obj.ObjectMachine);
                if (mObj.Class == "DataTable" || mObj.Class == "DataView" || mObj.Class == "PhysicalInformationArtefact" || mObj.Class == "LogicalInformationArtefact")//mObj.Class == "Entity" ||
                    break;
                else
                    mObj = null;
            }

            if (mObj != null)
            {
                return ":" + Loader.GetFromProvider(mObj.pkid, mObj.Machine, mObj.Class, provider == Core.Variables.Instance.ServerProvider).ToString();
            }
            return "";
        }

        bool cancelThread = false;
        public void CancelThread()
        {
            cancelThread = true;
            Thread.Sleep(50);
            buttonEnabled(true);
            Report(0, 0, "");
        }
        private void FindDuplicates()
        {
            Report(0, 0, "Collecting");
            //1 retrieval of all data
            //get all objects with a (properties string)
            string connstring = Server ? Core.Variables.Instance.ServerConnectionString : Core.Variables.Instance.ConnectionString;
            SqlConnection conn = new SqlConnection(connstring);
            SqlCommand com = new SqlCommand();
            com.Connection = conn;
            com.CommandType = CommandType.Text;
            string filterString = "";
            foreach (string f in filters)
            {
                filterString += f;
            }
            com.CommandText = "select metaobject.pkid,metaobject.machine,metaobject.class,metaobject.workspacename,metaobject.workspacetypeid,metaobject.vcstatusid,metaobject.vcmachineid,objectfieldvalue.fieldid,objectfieldvalue.valuestring,field.Name from metaobject inner join objectfieldvalue on objectid = pkid and machineid = machine inner join field on objectfieldvalue.fieldid = field.pkid where (objectfieldvalue.valuestring <> '' or objectfieldvalue.valuestring <> null)  " + filterString + " ORDER BY metaobject.class,metaobject.pkid,objectfieldvalue.FieldID,objectfieldvalue.valuestring";
            com.CommandTimeout = 0;
            DataSet ds = new DataSet();
            SqlDataAdapter dap = new SqlDataAdapter();
            dap.SelectCommand = com;
            dap.Fill(ds, "Objects");
            //return ds.Tables["Objects"].DefaultView;

            Dictionary<MetaClassKey, List<FieldValue>> toBeCompared = new Dictionary<MetaClassKey, List<FieldValue>>();
            //1 load of all data
            //compile objects into pkid/machine/class/string (propertiesstring = fieldvalues for value string where !=null)
            //pkid  machine     class   ws  wsid    s   vcmachine   fieldid value
            List<MetaClassKey> keys = new List<MetaClassKey>();
            int counter = 0;
            foreach (DataRow row in ds.Tables["Objects"].DefaultView.Table.Rows)
            {
                Report(ds.Tables["Objects"].DefaultView.Table.Rows.Count, counter, "Loading");
                counter += 1;
                if (cancelThread)
                    return;
                MetaKey metaKey = new MetaKey();
                metaKey.PKID = int.Parse(row[0].ToString());
                metaKey.Machine = row[1].ToString();
                MetaClassKey MCkey = new MetaClassKey(metaKey, row[2].ToString());

                bool found = false;
                foreach (MetaClassKey key in keys)
                {
                    if (key.ObjectKey.PKID == MCkey.ObjectKey.PKID && key.ObjectKey.Machine == MCkey.ObjectKey.Machine && key.ObjectClass == MCkey.ObjectClass)
                    {
                        found = true;
                        MCkey = key;
                        break;
                    }
                }
                if (!found)
                    keys.Add(MCkey);

                int fieldid = int.Parse(row[7].ToString());
                string fieldvalue = row[8].ToString();

                //If i am a column or attribute append my parents name to the value
                if (MCkey.ObjectClass == "Attribute" || MCkey.ObjectClass == "DataColumn" || MCkey.ObjectClass == "DataAttribute" || MCkey.ObjectClass == "DataField")
                {
                    fieldvalue += returnParentStringValueForComplexChild(metaKey);
                }
                //bool foundKey = false;
                //int keyIndex = -1;
                //foreach (KeyValuePair<MetaClassKey, List<FieldValue>> keyPair in toBeCompared)
                //{
                //    keyIndex++;
                //    if (keyPair.Key.ObjectKey.PKID != MCkey.ObjectKey.PKID && keyPair.Key.ObjectKey.Machine != MCkey.ObjectKey.Machine && keyPair.Key.ObjectClass == MCkey.ObjectClass)
                //    {
                //        foundKey = true;
                //    }
                //}

                bool addDuplicateCheckField = false; //TODO setting to disable this? just set this to true
                //row[9].ToString();
                if (row[2].ToString() == "Employee" || row[2].ToString() == "Rationale")
                {
                    if (row[9].ToString() == "Name" || row[9].ToString() == "Surname" || row[9].ToString() == "UniqueRef")
                        addDuplicateCheckField = true;
                }
                else
                {
                    if (row[9].ToString() == "Name")
                        addDuplicateCheckField = true;
                }

                if (addDuplicateCheckField)
                {
                    if (!toBeCompared.ContainsKey(MCkey))
                    {
                        //keyIndex = 0;
                        toBeCompared.Add(MCkey, new List<FieldValue>());
                        toBeCompared[MCkey].Add(new FieldValue(fieldid, fieldvalue));
                    }
                    else
                    {
                        toBeCompared[MCkey].Add(new FieldValue(fieldid, fieldvalue));
                    }
                }
            }

            //compare apples with apples
            counter = 0;
            Report(0, 0, "Comparing");
            List<MetaKey> alreadyAddedDuplicates = new List<MetaKey>();
            Dictionary<MetaClassKey, List<MetaClassKey>> keysWithDuplicates = new Dictionary<MetaClassKey, List<MetaClassKey>>();
            foreach (KeyValuePair<MetaClassKey, List<FieldValue>> keyPair in toBeCompared)
            {
                Report(toBeCompared.Count, counter, "Comparing");
                counter += 1;
                if (cancelThread)
                    return;
                //skip objects which have already been added as duplicate objects
                if (alreadyAddedDuplicates.Contains(keyPair.Key.ObjectKey))
                    continue;

                List<MetaClassKey> duplicateObjectMetaKeys = new List<MetaClassKey>();
                bool foundDuplicates = false;

                foreach (KeyValuePair<MetaClassKey, List<FieldValue>> keyPairCompared in toBeCompared)
                {
                    if (keysWithDuplicates.ContainsKey(keyPairCompared.Key) || alreadyAddedDuplicates.Contains(keyPairCompared.Key.ObjectKey))
                        continue;

                    //is different key and same class
                    if (((keyPair.Key.ObjectKey.PKID.ToString() + keyPair.Key.ObjectKey.Machine) != (keyPairCompared.Key.ObjectKey.PKID.ToString() + keyPairCompared.Key.ObjectKey.Machine)) && keyPair.Key.ObjectClass == keyPairCompared.Key.ObjectClass)
                    {
                        //is value the same
                        string keyValueString = "";
                        string comparedKeyValueString = "";

                        foreach (FieldValue KeyFieldValue in keyPair.Value)
                        {
                            keyValueString += KeyFieldValue.ToString();
                        }
                        foreach (FieldValue comparedKeyFieldValue in keyPairCompared.Value)
                        {
                            comparedKeyValueString += comparedKeyFieldValue.ToString();
                        }

                        if (keyValueString.ToLower().Replace(" ", "").Replace(Environment.NewLine.ToString(), "") == comparedKeyValueString.ToLower().Replace(" ", "").Replace(Environment.NewLine.ToString(), ""))
                        {
                            foundDuplicates = true;

                            if (!duplicateObjectMetaKeys.Contains(keyPairCompared.Key))
                            {
                                duplicateObjectMetaKeys.Add(keyPairCompared.Key);
                                alreadyAddedDuplicates.Add(keyPairCompared.Key.ObjectKey);
                            }
                        }
                    }
                }

                if (foundDuplicates)
                {
                    //if (keysWithDuplicates.ContainsKey(keyPair.Key))
                    //{
                    //    //find key and append these duplicateobjectkeys
                    //    keyPair.ToString();
                    //}

                    keysWithDuplicates.Add(keyPair.Key, duplicateObjectMetaKeys);
                    //alreadyAddedDuplicates.Add(keyPair.Key.ObjectKey);
                }
            }

            //the key is the main object and the list of keys are the duplicates found for it
            //convert each MetaKey to metaBase
            counter = 0;
            foreach (KeyValuePair<MetaClassKey, List<MetaClassKey>> KeyWithItsDuplicates in keysWithDuplicates)
            {
                Report(keysWithDuplicates.Count, counter, "Compiling");
                counter += 1;
                if (cancelThread)
                    return;
                MetaBase mBase = Loader.GetFromProvider(KeyWithItsDuplicates.Key.ObjectKey.PKID, KeyWithItsDuplicates.Key.ObjectKey.Machine, KeyWithItsDuplicates.Key.ObjectClass, Server);
                List<MetaBase> duplicateMetaBaseObjects = new List<MetaBase>();
                foreach (MetaClassKey duplicateObject in KeyWithItsDuplicates.Value)
                {
                    duplicateMetaBaseObjects.Add(Loader.GetFromProvider(duplicateObject.ObjectKey.PKID, duplicateObject.ObjectKey.Machine, duplicateObject.ObjectClass, Server));
                }
                duplicateObjects.Add(mBase, duplicateMetaBaseObjects);
                objectList1.AddListedObjectDictionaryItemSafely(new MetaObjectKey(mBase.pkid, mBase.MachineName), mBase);
            }
            Report(0, 0, "Completing");
            objectList1.USR();
            buttonEnabled(true);
            Report(0, 0, "");
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
        //string str_domain = "";
        GraphFile file = null;
        VCStatusList filterForStatus = VCStatusList.Deleted;
        Workspace ws = null;

        private void FindDuplicatesThread()
        {
            cancelThread = false;
            buttonEnabled(false);

            Thread t = new Thread(new ThreadStart(FindDuplicates));
            t.Start();
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

        //private void FindDuplicates(List<MetaBase> items)
        //{
        //    List<MetaBase> itemsWithDuplicates = new List<MetaBase>();
        //    List<MetaBase> itemsMarkedAsDuplicates = new List<MetaBase>();
        //    foreach (MetaBase item in items)
        //    {
        //        MetaBase mb = item;
        //        if (!duplicateObjects.ContainsKey(mb))
        //        {
        //            duplicateObjects.Add(mb, new List<MetaBase>());
        //        }

        //        if (!itemsMarkedAsDuplicates.Contains(mb))
        //        {
        //            bool foundDuplicates = false;
        //            foreach (MetaBase compareditem in items)
        //            {
        //                MetaBase mbCompared = compareditem;
        //                bool sameString = false;
        //                bool sameClass = mb._ClassName == mbCompared._ClassName;
        //                if (!sameClass) //duplicates must be the same class
        //                    continue;
        //                //4 June 2013
        //                //if (mb._ClassName == "Attribute" && mbCompared._ClassName == "Attribute")
        //                if (mb._ClassName == "Attribute" || mb._ClassName == "DataColumn")  || MCkey.ObjectClass == "DataAttribute" || MCkey.ObjectClass == "DataField"
        //                {
        //                    TList<MetaObject> mbAttParents = DataRepository.Connections[Provider].Provider.MetaObjectProvider.GetByChildObjectIDChildObjectMachineFromObjectAssociation(mbCompared.pkid, mb.MachineName);
        //                    TList<MetaObject> mbComparedAttParents = DataRepository.Connections[Provider].Provider.MetaObjectProvider.GetByChildObjectIDChildObjectMachineFromObjectAssociation(mbCompared.pkid, mb.MachineName);
        //                    //check if the parent is the same foreach parent
        //                    foreach (MetaObject mbObjAttParent in mbAttParents)
        //                    {
        //                        if (foundDuplicates)
        //                            break;
        //                        foreach (MetaObject mbObjComparedAttParent in mbComparedAttParents)
        //                        {
        //                            if (mbObjAttParent.Class != mbObjComparedAttParent.Class) //different classes are immediately false
        //                                continue;
        //                            MetaBase mbAttParent = Loader.GetFromProvider(mbObjAttParent.pkid, mbObjAttParent.Machine, mbObjAttParent.Class, Server);
        //                            MetaBase mbComparedAttParent = Loader.GetFromProvider(mbObjComparedAttParent.pkid, mbObjComparedAttParent.Machine, mbObjComparedAttParent.Class, Server);

        //                            if ((mbAttParent.ToString() + "`" + mb.ToString()) == (mbComparedAttParent.ToString() + "~" + mbCompared.ToString()))
        //                            {
        //                                if (!duplicateObjects[mb].Contains(mbCompared))
        //                                    duplicateObjects[mb].Add(mbCompared);
        //                                foundDuplicates = true;
        //                                break;
        //                            }
        //                        }
        //                    }
        //                    //Unparented complex child objects
        //                    if (mbAttParents.Count == 0 && mbComparedAttParents.Count == 0)
        //                    {
        //                        sameString = mb.ToString() == mbCompared.ToString();
        //                    }
        //                }
        //                else
        //                {
        //                    //non complex child classes follow normal logic in that they are duplicates when having the same name
        //                    sameString = mb.ToString() == mbCompared.ToString();
        //                }

        //                bool pkidDifferent = mb.pkid != mbCompared.pkid;
        //                bool machineDifferent = mb.MachineName != mbCompared.MachineName;
        //                bool keyDifferent = pkidDifferent || machineDifferent;

        //                if (sameString && sameClass && keyDifferent)
        //                {
        //                    itemsMarkedAsDuplicates.Add(mbCompared);
        //                    if (!duplicateObjects[mb].Contains(mbCompared))
        //                    {
        //                        duplicateObjects[mb].Add(mbCompared);
        //                    }
        //                    foundDuplicates = true;
        //                }
        //            }
        //            if (foundDuplicates)
        //                itemsWithDuplicates.Add(mb);
        //        }

        //        if (duplicateObjects[mb].Count == 0)
        //        {
        //            duplicateObjects.Remove(mb);
        //        }
        //    }
        //    foreach (MetaBase mb in itemsWithDuplicates)
        //    {
        //        objectList1.AddListedObjectDictionaryItemSafely(new MetaObjectKey(mb.pkid, mb.MachineName), mb);
        //    }
        //    if (objectList1.ListedObjectsDictionary.Count == 0)
        //        objectList1.UpdateSearchResult();
        //}

        private void LoadSelection(SerializableObjectSearchResult result)
        {
            /*
            if (cbClearResults.Checked)
            {
                objectList1.SelectedObjectsDictionary = new Dictionary<MetaObjectKey, MetaBase>();
                objectList1.ListedObjectsDictionary = new Dictionary<MetaObjectKey, MetaBase>();
            }
            foreach (SerializableObjectSearchResult.ObjectIDentifier identifier in result.ObjectIdentifiers)
            {
                MetaBase mb = Loader.CreateInstance(identifier.ObjectClass);
                mb.LoadFromID(identifier.Key.Pkid,identifier.Key.Machine);
                if (mb.ToString() != null)
                {
                    objectList1.ListedObjectsDictionary.Add(identifier.Key,mb);
                    objectList1.SelectedObjectsDictionary.Add(identifier.Key, mb);
                }
            }
            objectList1.UpdateSelection();
            objectList1.BindList();
            objectList1.UpdateSearchResult();*/
        }

        private void ObjectFinder_Load(object sender, EventArgs e)
        {

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
            /*
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.InitialDirectory = Core.Variables.Instance.ExportsPath;
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
            }*/
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            /*
            SerializableObjectSearchResult sResult = new SerializableObjectSearchResult();
            sResult.ObjectIdentifiers = new List<SerializableObjectSearchResult.ObjectIDentifier>();
            foreach (KeyValuePair<MetaObjectKey,MetaBase> kvp in objectList1.SelectedObjectsDictionary)
            {
                SerializableObjectSearchResult.ObjectIDentifier identifier = new SerializableObjectSearchResult.ObjectIDentifier();
                identifier.Key = kvp.Key;
                identifier.ObjectClass = kvp.Value._ClassName;
                identifier.StringValue = kvp.Value.ToString();
                sResult.ObjectIdentifiers.Add(identifier);
            }
            SaveFileDialog sfileDialog = new SaveFileDialog();
            sfileDialog.InitialDirectory = Core.Variables.Instance.ExportsPath;
            string extension = "mbselxml";
            sfileDialog.Filter = "Saved Object Selection (*." + extension + ") | *." + extension;
            DialogResult res = sfileDialog.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                sResult.Save(sfileDialog.FileName);
            }
             * */
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
            if (Server)
                btnFind.Text += " (Server)";

            comboStatus.DisplayMember = "Caption";
            if (!Server)
            {
                TList<Workspace> workspaces = DataRepository.Connections[Provider].Provider.WorkspaceProvider.GetAll();
                foreach (Workspace ws in workspaces)
                {
                    comboWorkspace.Items.Add(ws);
                }
            }
            else
            {
                foreach (Workspace wsServer in ServerWorkspacesUserHasWithAdminPermission)
                {
                    comboWorkspace.Items.Add(wsServer);
                }
            }
            TList<Class> classes = DataRepository.Classes(Provider);//.Provider.ClassProvider.GetAll();
            foreach (Class cs in classes)
            {
                if (LimitToClass == null)
                    comboClass.Items.Add(cs);
                else if (LimitToClass == cs.Name)
                {
                    if (cs.IsActive == true)
                        comboClass.Items.Add(cs);
                    comboClass.Text = cs.Name;
                    cbClass.Checked = true;
                    cbClass.Enabled = false;
                }
            }
            if (LimitToStatus == -1)
            {
                foreach (VCStatusList status in Enum.GetValues(typeof(VCStatusList)))
                {
                    if (ExcludeStatuses != null && status != VCStatusList.Deleted && status != VCStatusList.Obsolete && status != VCStatusList.PCI_Revoked && status != VCStatusList.PartiallyCheckedIn && status != VCStatusList.Skipped && status != VCStatusList.CheckedOutRead)
                    {
                        if (!ExcludeStatuses.Contains(status))
                        {
                            CustomListItem clitemStatus = new CustomListItem();
                            clitemStatus.Tag = status;
                            clitemStatus.Caption = ((VCStatusList)status).ToString();
                            comboStatus.Items.Add(clitemStatus);
                        }
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
            if (ExcludeStatuses == null)
                ExcludeStatuses = new List<VCStatusList>();
            //always skip mfd when duplicate checking
            ExcludeStatuses.Add(VCStatusList.MarkedForDelete);

            UpdateFileList();

            if (!Core.Variables.Instance.IsServer)
            {
                cbStatus.Visible = false;
                comboStatus.Visible = false;
            }
        }

        private void UpdateFileList()
        {
            comboDiagram.Items.Clear();
            TList<GraphFile> files;
            TempFileGraphAdapter tgfa = new TempFileGraphAdapter();
            if (cbWorkspace.Checked)
            {
                Workspace ws = comboWorkspace.SelectedItem as Workspace;
                if (ws != null)
                {
                    files = tgfa.GetFilesByWorkspaceTypeIdWorkspaceName(ws.WorkspaceTypeId, ws.Name, (int)FileTypeList.Diagram, Server);
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

            foreach (GraphFile file in files)
            {
                try
                {
                    // temporarily change the file name for display purposes
                    if (file.IsActive)
                    {
                        file.Name = strings.GetFileNameOnly(file.Name) + " V" + file.MajorVersion.ToString() + "." + file.MinorVersion.ToString();

                        if (cbStatus.Checked)
                        {
                            CustomListItem clItem = comboStatus.SelectedItem as CustomListItem;
                            if (file.VCStatusID == (int)clItem.Tag)
                            {
                                comboDiagram.Items.Add(file);
                            }
                        }
                        else
                        {
                            comboDiagram.Items.Add(file);
                        }
                    }
                }
                catch
                {
                }
            }
        }

        private void comboWorkspace_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFileList();
        }

        private void cbWorkspace_CheckedChanged(object sender, EventArgs e)
        {
            comboWorkspace.Enabled = cbWorkspace.Checked;
            UpdateFileList();
        }

        private void cbDiagram_CheckedChanged(object sender, EventArgs e)
        {
            comboDiagram.Enabled = cbDiagram.Checked;
        }

        private void cbClass_CheckedChanged(object sender, EventArgs e)
        {
            comboClass.Enabled = cbClass.Checked;
        }
        public void Confirm()
        {
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            Find();
        }

        private void Find()
        {
            //PleaseWait.ShowPleaseWaitForm();
            //PleaseWait.SetStatus("Comparing Data");
            FillData();
            //objectList1.BindList();
            //PleaseWait.CloseForm();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            objectList1.SelectAll();
        }

        private void btnSelectNone_Click(object sender, EventArgs e)
        {
            objectList1.SelectNone();
        }

        private void txtSearchValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Find();
        }

        #endregion

        #region Data Events

        private List<string> filters;

        private void FillData()
        {
            #region filter Variables

            filterClass = cbClass.Checked && comboClass.Text.Length > 0;
            if (filterClass)
            {
                str_class = comboClass.Text;
                //if (cbDomainValue.Checked)
                //{
                //    if (comboDomainValue.SelectedIndex > 0)
                //    {
                //        if (comboDomainValue.Tag is Field)
                //        {
                //            //Field f = (comboDomainValue.Tag as Field);
                //            DomainDefinitionPossibleValue v = (comboDomainValue.SelectedItem as DomainDefinitionPossibleValue);
                //            str_domain = v.PossibleValue;
                //        }
                //    }
                //}
            }
            else
            {
                str_class = "";
                //str_domain = "";
            }
            filterWorkspace = cbWorkspace.Checked && comboWorkspace.Text.Length > 0;
            if (filterWorkspace)
            {
                ws = comboWorkspace.SelectedItem as Workspace;
            }
            else
            {
                ws = null;
            }
            filterDiagram = cbDiagram.Checked && comboDiagram.Text.Length > 0;
            if (filterDiagram)
                file = comboDiagram.SelectedItem as GraphFile;
            else
                file = null;
            if (ExcludeStatuses != null)
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

            if (limitToStatus != -1)
            {
                CustomListItem clitem = comboStatus.SelectedItem as CustomListItem;
                filterForStatus = (VCStatusList)clitem.Tag;
            }

            #endregion

            if (clearResults)
            {
                objectList1.ListedObjectsDictionary = new Dictionary<MetaObjectKey, MetaBase>();
                duplicateObjects = new Dictionary<MetaBase, List<MetaBase>>();
            }

            if (objectList1.SelectedObjectsDictionary == null)
                objectList1.SelectedObjectsDictionary = new Dictionary<MetaObjectKey, MetaBase>();

            #region Data Retrieval & Filter  - this might be speeded up by using stored procs
            //bool filterClass = cbClass.Checked && comboClass.Text.Length > 0;
            //bool filterWorkspace = cbWorkspace.Checked && comboWorkspace.Text.Length > 0;
            //bool filterDiagram = cbDiagram.Checked && comboDiagram.Text.Length > 0;
            //bool filterExcludeStatuses = false;
            //if (ExcludeStatuses != null)
            //    filterExcludeStatuses = ExcludeStatuses.Count > 0;
            //bool filterStatus = cbStatus.Checked && comboStatus.Text.Length > 0;
            //bool filterStringValue = !string.IsNullOrEmpty(txtSearchValue.Text);

            //TList<MetaObject> objects;
            List<MetaObject> objectsToRemove = new List<MetaObject>();
            filters = new List<string>();

            if (filterStringValue)
            {
                filters.Add(" and cast(metaobject.pkid as varchar(20)) + '|' + Machine in (select cast(ObjectID as varchar(20)) + '|' + MachineID from ObjectFieldValue where ValueString like '%" + strings.replaceApostrophe(str_value) + "%')");
            }
            if (filterClass)
                filters.Add(" AND metaobject.CLASS='" + str_class + "' ");

            if (filterWorkspace)
            {
                filters.Add(" AND WorkspaceName = '" + ws.Name + "' and WorkspaceTypeId=" + ws.WorkspaceTypeId.ToString());
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

            if (filterDiagram)
            {
                if (file != null)
                {
                    filters.Add(" and cast(metaobject.pkid as varchar(20)) + '|' + Machine in (select cast(metaObjectID as varchar(20)) + '|' + MachineID from GraphFileObject where GraphFileID=" + file.pkid.ToString() + " and GraphFileMachine='" + file.Machine + "')");
                }
            }

            if (filterExcludeStatuses)
            {
                foreach (VCStatusList excludeStatusItem in excludeStatuses)
                {
                    filters.Add(" AND NOT VCStatusID=" + ((int)excludeStatusItem).ToString());
                }
            }

            if (filterStatus)
            {
                filters.Add(" AND VCStatusID=" + ((int)filterForStatus).ToString());
            }

            if (ExcludeVCItems && !Server)
            {
                filters.Add(" AND VCMachineID IS NULL");
            }

            if (limitToStatus != -1)
            {
                //CustomListItem clitem = comboStatus.SelectedItem as CustomListItem;
                //VCStatusList filterForStatus = (VCStatusList)clitem.Tag;
                filters.Add(" AND VCStatusID=" + ((int)filterForStatus).ToString());
            }

            #endregion

            FindDuplicatesThread();
            //return;

            //ObjectHelper objHelper = new ObjectHelper(Server);
            //List<MetaBase> objectsToCheck = new List<MetaBase>();
            //DataView dvSearchResult = null;
            //try
            //{
            //    dvSearchResult = objHelper.GetObjectsFiltered(filters, Server);
            //}
            //catch (Exception ex)
            //{
            //    Core.Log.WriteLog(ex.ToString());
            //}
            //Dictionary<string, List<MetaKey>> loaderObjs = new Dictionary<string, List<MetaKey>>();
            //if (dvSearchResult != null)
            //    foreach (DataRowView drvObject in dvSearchResult)
            //    {
            //        string className = drvObject["Class"].ToString();
            //        if (!loaderObjs.ContainsKey(className))
            //        {
            //            loaderObjs.Add(className, new List<MetaKey>());
            //        }
            //        MetaKey k = new MetaKey();
            //        k.PKID = int.Parse(drvObject["pkid"].ToString());
            //        k.Machine = drvObject["Machine"].ToString();
            //        loaderObjs[className].Add(k);
            //    }
            //PleaseWait.SetStatus("Building Objects...");
            //List<MetaBase> objsLoaded = Loader.GetFromProvider(loaderObjs, Server);
            //foreach (MetaBase mb in objsLoaded)
            //{
            //    objectsToCheck.Add(mb);
            //}
            //objsLoaded = null;
            //FindDuplicates(objectsToCheck);
        }
        #endregion

        //private void FindDupicatesWithoutBeingStoned(List<MetaBase> MetaBase)
        //{
        //    //build dupe objects into array
        //}

    }

    public class PossibleDuplication
    {

        #region Fields (3)

        private List<GraphFileKey> affectedFiles;
        private MetaBase myMetaObject;
        private List<MetaBase> possibleDuplicates;

        #endregion Fields

        #region Properties (3)

        public List<GraphFileKey> AffectedFiles
        {
            get { return affectedFiles; }
            set { affectedFiles = value; }
        }

        public MetaBase MyMetaObject
        {
            get { return myMetaObject; }
            set { myMetaObject = value; }
        }

        public List<MetaBase> PossibleDuplicates
        {
            get { return possibleDuplicates; }
            set { possibleDuplicates = value; }
        }

        #endregion Properties

    }

    public class MetaClassKey
    {
        private MetaKey objectKey;
        public MetaKey ObjectKey { get { return objectKey; } set { objectKey = value; } }
        private string objectClass;
        public string ObjectClass { get { return objectClass; } set { objectClass = value; } }
        public MetaClassKey(MetaKey key, string keyClass)
        {
            ObjectKey = key;
            ObjectClass = keyClass;
        }

        public override bool Equals(object obj)
        {
            MetaClassKey other = obj as MetaClassKey;
            bool x = other.objectClass == this.objectClass && ((other.ObjectKey.PKID.ToString() + other.ObjectKey.Machine) == (this.ObjectKey.PKID.ToString() + this.ObjectKey.Machine));
            return x;
        }

        public override int GetHashCode()
        {
            if (ObjectClass == null || ObjectKey == null)
                return 0;
            return base.GetHashCode();
        }
    }

    public class FieldValue
    {
        public int FieldID;
        public string Value;
        public FieldValue(int id, string fieldvalue)
        {
            FieldID = id;
            Value = fieldvalue;
        }
        public override string ToString()
        {
            return ParentClass.Length == 0 ? FieldID + ":" + Value + "|" : FieldID + ":" + Value + "`" + ParentClass + "|";
        }

        public string ParentClass = "";
    }

    //public class DupicateObjectKey
    //{
    //    private int _pkid;
    //    private int PKID
    //    {
    //        get { return _pkid; }
    //    }

    //    private string _machine;
    //    private string Machine
    //    {
    //        get { return _machine; }
    //    }

    //    private string _values;
    //    private string Values
    //    {
    //        get { return _values; }
    //    }

    //    private string _class;
    //    private string Class
    //    {
    //        get { return _class; }
    //    }

    //    private List<string> _values = new List<string>();

    //    public string Key { get { return PKID + "|" + Machine; } }
    //    public string Value { get { return Class + "-" + Values.ToString(); } }

    //    public DupicateObjectKey(int pkid, string machine, string theClass, List<string> values)
    //    {
    //        _pkid = pkid;
    //        _machine = machine;
    //        _class = theClass;
    //        _values = values;
    //    }
    //}
}