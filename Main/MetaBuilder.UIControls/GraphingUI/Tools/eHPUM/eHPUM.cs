using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MetaBuilder.Meta;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;

namespace MetaBuilder.UIControls.GraphingUI.Tools.eHPUM
{
    public partial class eHPUM : Form
    {
        public eHPUM()
        {
            InitializeComponent();
            numericUpDownLimit.Value = 0;
        }

        private void eHPUM_FormClosing(object sender, FormClosingEventArgs e)
        {
            loadSaved = false;
            if (sThread != null && sThread.IsAlive)
            {
                if (
                    MessageBox.Show(this,
                        "An import operation is currently running. You need to cancel it in order to close this form. Would you like to do this now?",
                        "Cancel Operation?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cancelCount = 0;
                    e.Cancel = tryCancelThread();
                }
                else
                {
                    e.Cancel = true;
                }
            }

            if (SavedKeys == null || SavedKeys.Count <= 1) return;

            if (checkBoxAutoImport.Checked)
            {
                loadSaved = true;
            }
            else if (
                MessageBox.Show(this,
                    "Objects have been saved by this import, would you like to generate a diagram for them?" +
                    Environment.NewLine +
                    "NOTE: Be aware that this can take a very long time depending on the amount of objects that have been saved.",
                    "Create Diagram?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                loadSaved = true;
            }
        }

        private bool loadSaved;
        private int cancelCount;

        private bool tryCancelThread()
        {
            //try close excel out of thread. lol this is hard!
            //causes resource leak because excel is not closed as a result, AND THE APP CRASHES
            try
            {
                cancelCount += 1;
                if (util != null)
                {
                    util.CloseWorkbook();
                    util.CurrentSheet = null;
                    util.CloseExcel();
                    util.ExcelApp = null;
                    util = null;
                }

                sThread.Abort();
                sThread = null;

                return false;
            }
            catch
            {
                if (cancelCount <= 5) //we dont want to do this forever
                    tryCancelThread();
                else
                    return true;
            }
            return false;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private int level;

        public int Level
        {
            get
            {
                return level;
                //if (maxlevel < level) maxlevel = level;
            }
            set
            {
                level = value;
                if (maxlevel < level) maxlevel = level;
            }
        }

        private int maxlevel;

        #region Loading

        private readonly string[] alphabet = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", 
                                                    "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai","aj","ak","al","am","an","ao","ap","aq","ar","as","at","au","av","aw","ax","ay","az",
                                                    "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj","bk","bl","bm","bn","bo","bp","bq","br","bs","bt","bu","bv","bw","bx","by","bz"};

        private BusinessFacade.Exports.ExcelUtil util;
        private bool building;

        private void buttonChooseFile_Click(object sender, EventArgs e)
        {
            labelFile.Text = "Please select a file";

            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "Select a suitable excel file to import";
            fd.Filter = "Complex structured Excel Template Files|*.xls*";
            fd.Multiselect = false;
            fd.InitialDirectory = Core.Variables.Instance.DiagramPath;
            if (fd.ShowDialog(this) != DialogResult.OK) return;
            if (fd.FileName.Length == 0) return;

            if (fd.FileName.Contains(".xl"))
            {
                loadFile(fd.FileName);
                labelFile.Text = fd.FileName;
            }
        }

        private void updateText(string text)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(updateText), new object[] { text });
                return;
            }

            this.Text = text;
        }

        private int rowCount = 0;

        private void loadFile(string file)
        {
            SavedKeys = new Dictionary<int, string>();
            labelImportInfo.Visible = false;
            building = true;
            buttonImport.Enabled = false;
            progressBarImport.Visible = false;
            //SplashScreen.PleaseWait.ShowPleaseWaitForm();
            ImportDiagram diagram = new ImportDiagram();
            diagram.Name = Core.strings.GetFileNameWithoutExtension(file);
            BeginInvoke(new Action<bool>(switchControls), new object[] { true });
            string origText = this.Text;
            this.Text = "Discovering row limit of : " + diagram.Name + "... Opening File";

            util = new BusinessFacade.Exports.ExcelUtil();
            util.OpenExcel();
            util.OpenFile(file);

            int col = 0;
            int row = 1;
            while (GetText(alphabet[col], 1).Length > 0) //columns discovery cannot be skipped
            {
                col++;
            }
            if (numericUpDownLimit.Value > 0) //skips the entire discovery loop
            {
                row = (int)numericUpDownLimit.Value;
            }
            else
            {
                string t = "Discovering row limit of : " + diagram.Name + "... Please wait : ";
                while (GetText("B", row).Length > 0)
                {
                    //if (row % 100 == 0)
                    //{
                    //    t = "Discov3riNg r0w lim!t 0f : " + diagram.Name + "... Plea5e wait : ";
                    //}
                    //else if (row % 75 == 0)
                    //{
                    //    t = "dIsc0VerinG rOw L!mit OF : " + diagram.Name + "... pL3aSe waIt / ";
                    //}
                    //else if (row % 50 == 0)
                    //{
                    //    t = "D!sCov3r!nG RoW 1mi7 Of : " + diagram.Name + "... Ple45e w4!7 > ";
                    //}
                    //else if (row % 25 == 0)
                    //{
                    //    t = "d1sCoveRing Row limit oF : " + diagram.Name + "... PleasE w4it - ";
                    //}
                    //else
                    //{
                    //    t = "Discovering row limit of : " + diagram.Name + "... Please wait (" + row.ToString() + ")";
                    //}
                    updateText(t + row.ToString());

                    row++;
                    //if (numericUpDownLimit.Value > 0)
                    //    if (row == numericUpDownLimit.Value) break;
                }
            }
            int columnCount = col + 1;
            rowCount = row;

            this.Text = origText;
            origText = null;
            progressBarImport.Maximum = rowCount;

            #region LOAD

            sThread = new Thread(delegate()
                                     {
                                         for (row = 2; row <= rowCount; row++) //each row
                                         {
                                             updateLoad(row);
                                             List<ImportObject> unprocessedOrderedRowObjects = new List<ImportObject>();
                                             ImportObject tempObject = null;
                                             Level = 0;
                                             ImportObject rowO = new ImportObject();
                                             //This is the first column of each row!
                                             //alphabet is 0 based but we iterate through physical number of columns : hence col-1's below
                                             for (col = 1; col <= columnCount; col++) //each column
                                             {
                                                 if (col == 1)
                                                 {
                                                     rowO.Name = GetText(alphabet[col - 1], row);
                                                     if (rowO.Name.Length == 0)
                                                         rowO.Name = "Default Project";
                                                     rowO.Level = Level;
                                                 }
                                                 else
                                                 {
                                                     if (col >= 10)
                                                     {
                                                         if (GetText(alphabet[col - 1], row).Length == 0 &&
                                                             tempObject == null) break;
                                                         //column 10 and 12 are extra columns so now we need to do a manual insertion dammit(so smart but so dumb) 3/3/2/N(this is now impossible)
                                                         switch (col)
                                                         {
                                                             case 16:
                                                             case 13:
                                                             case 10:
                                                                 tempObject = new ImportObject();
                                                                 if (col == 10 || col == 13)
                                                                 {
                                                                     tempObject.Objective = GetText(alphabet[col - 1],
                                                                                                    row);
                                                                     if (col == 10)
                                                                     {
                                                                         tempObject.Level = 5;
                                                                         Level = 5;
                                                                     }
                                                                     else
                                                                     {
                                                                         tempObject.Level = 6;
                                                                         Level = 6;
                                                                     }
                                                                 }
                                                                 else
                                                                 {
                                                                     tempObject.Level = 7;
                                                                     Level = 7;
                                                                     tempObject.Name = GetText(alphabet[col - 1], row);
                                                                 }
                                                                 break;
                                                             case 14:
                                                             case 11:
                                                                 tempObject.Name = GetText(alphabet[col - 1], row);
                                                                 break;
                                                             default:
                                                                 tempObject.Description = GetText(alphabet[col - 1], row);
                                                                 unprocessedOrderedRowObjects.Add(tempObject);
                                                                 tempObject = null;
                                                                 break;
                                                         }
                                                     }
                                                     else
                                                     {
                                                         if (GetText(alphabet[col - 1], row).Length == 0 &&
                                                             tempObject == null)
                                                             break;
                                                         if (col % 2 == 0) //even
                                                         {
                                                             tempObject = new ImportObject();
                                                             Level = (col / 2);
                                                             tempObject.Level = Level;
                                                             tempObject.Name = GetText(alphabet[col - 1], row);
                                                         }
                                                         else //odd
                                                         {
                                                             if (tempObject != null)
                                                             {
                                                                 tempObject.Description = GetText(alphabet[col - 1], row);
                                                                 unprocessedOrderedRowObjects.Add(tempObject);
                                                                 tempObject = null;
                                                             }
                                                         }
                                                     }
                                                 }
                                             }
                                             foreach (ImportObject o in unprocessedOrderedRowObjects)
                                             {
                                                 switch (o.Level) //max level should always be 7
                                                 {
                                                     case 1:
                                                         rowO.Objects.Add(o);
                                                         break;
                                                     case 2:
                                                         rowO.Objects[0].Objects.Add(o);
                                                         break;
                                                     case 3:
                                                         rowO.Objects[0].Objects[0].Objects.Add(o);
                                                         break;
                                                     case 4:
                                                         rowO.Objects[0].Objects[0].Objects[0].Objects.Add(o);
                                                         break;
                                                     case 5:
                                                         rowO.Objects[0].Objects[0].Objects[0].Objects[0].Objects.Add(o);
                                                         break;
                                                     case 6:
                                                         rowO.Objects[0].Objects[0].Objects[0].Objects[0].Objects[0].
                                                             Objects.Add(o);
                                                         break;
                                                     case 7:
                                                         rowO.Objects[0].Objects[0].Objects[0].Objects[0].Objects[0].
                                                             Objects[0].Objects.Add(o);
                                                         break;
                                                     case 8:
                                                         rowO.Objects[0].Objects[0].Objects[0].Objects[0].Objects[0].
                                                             Objects[0].Objects[0].
                                                             Objects.Add(o);
                                                         break;
                                                     case 9:
                                                         rowO.Objects[0].Objects[0].Objects[0].Objects[0].Objects[0].
                                                             Objects[0].Objects[0].
                                                             Objects[0].Objects.Add(o);
                                                         break;
                                                     case 10:
                                                         rowO.Objects[0].Objects[0].Objects[0].Objects[0].Objects[0].
                                                             Objects[0].Objects[0].
                                                             Objects[0].Objects[0].Objects.Add(o);
                                                         break;
                                                     default:
                                                         break;
                                                 }
                                             }

                                             diagram.Objects.Add(rowO);
                                             rowO = null;
                                         }
                                         completeLoad(diagram);
                                         diagram = null;
                                         building = false;
                                         BeginInvoke(new Action<bool>(switchControls), new object[] { false });
                                     }
                );
            sThread.IsBackground = true;
            sThread.Start();

            #endregion

        }

        private void updateLoad(int progress)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<int>(updateLoad), new object[] { progress });
                return;
            }

            progressBarImport.Value = progress;
            labelImportInfo.Visible = true;
            labelImportInfo.Text = "Loading row " + progress.ToString() + "/" + rowCount.ToString();
        }

        private void completeLoad(ImportDiagram diagram)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<ImportDiagram>(completeLoad), new object[] { diagram });
                return;
            }
            SavedKeys.Add(0, diagram.Name);
            util.CloseExcel();
            util = null;

            comboBoxClass.Items.Clear();
            foreach (Class c in DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassProvider.GetAll())
            {
                if (c.IsActive == true)
                    comboBoxClass.Items.Add(c.Name);
            }

            comboBoxClass.SelectedItem = "Function";
            comboBoxClass.SelectedText = "Function";

            comboBoxAss.DataSource = Enum.GetNames(typeof(LinkAssociationType));
            comboBoxAss.SelectedItem = "Decomposition";
            treeViewLayout.Nodes.Clear();
            TreeNode root = new TreeNode();
            root.Text = diagram.Name + " - " + diagram.Objects.Count.ToString() + " rows";
            root.Tag = diagram;
            root.ForeColor = Color.DarkRed;
            for (int l = 0; l <= maxlevel; l++)
            {
                TreeNode node = new TreeNode();
                ImportObject obj = new ImportObject();
                if (l == 0)
                    node.Text = "Project Level";
                else
                    node.Text = "Level " + l.ToString();
                obj.Level = l;
                node.Tag = obj;
                node.ForeColor = Color.DarkBlue;
                addNodeToLastNode(root, node);
            }
            treeViewLayout.Nodes.Add(root);
            treeViewLayout.ExpandAll();
            buttonImport.Enabled = true;
            panelProperties.Visible = true;
            labelImportInfo.Visible = true;
            labelImportInfo.Text = "Click import to continue";

            if (checkBoxAutoImport.Checked)
                buttonImport_Click("Automated", new EventArgs());
        }

        private static void addNodeToLastNode(TreeNode root, TreeNode node)
        {
            if (root.Nodes.Count > 0)
                addNodeToLastNode(root.Nodes[0], node);
            else
            {
                root.Nodes.Add(node);
            }
        }

        private string GetText(string col, int row)
        {
            return (string)util.CurrentSheet.get_Range(col + row.ToString(), col + row.ToString()).Text;
        }

        #endregion

        #region Templating

        private void labelFile_Click(object sender, EventArgs e)
        {
            buttonChooseFile_Click(sender, e);
        }

        private void treeViewLayout_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (panelProperties.Visible)
                selectNode(e.Node);
        }

        private void selectNode(TreeNode node)
        {
            if (node.Tag is ImportObject)
            {
                ImportObject o = node.Tag as ImportObject;
                labelPropInfo.Text =
                    "The properties specified below will be used to template all objects at this level (" +
                    o.Level.ToString() + ")";
                comboBoxClass.SelectedItem = o._Class;
                comboBoxAss.SelectedItem = o._AssociationType.ToString();

                labelWarning.Visible = true;
                labelClassInfo.Visible = true;
                labelAssInfo.Visible = true;
                comboBoxClass.Visible = true;
                comboBoxAss.Visible = true;
            }
            else
            {
                labelPropInfo.Text = "No properties can be templated for the selected object";

                labelWarning.Visible = false;
                labelClassInfo.Visible = false;
                labelAssInfo.Visible = false;
                comboBoxClass.Visible = false;
                comboBoxAss.Visible = false;
            }
        }

        private void comboBoxClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (treeViewLayout.SelectedNode != null)
                updateNode(treeViewLayout.SelectedNode);
        }

        private void comboBoxAss_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (treeViewLayout.SelectedNode != null)
                updateNode(treeViewLayout.SelectedNode);
        }

        private void updateNode(TreeNode node)
        {
            if (building) return;
            if (node.Tag == null) return;
            if (!(node.Tag is ImportObject)) return;

            ImportObject obj = node.Tag as ImportObject;
            obj._Class = comboBoxClass.Text;

            if (Enum.IsDefined(typeof(LinkAssociationType), comboBoxAss.Text))
                obj._AssociationType =
                    (LinkAssociationType)Enum.Parse(typeof(LinkAssociationType), comboBoxAss.Text, true);
            else
                obj._AssociationType = LinkAssociationType.Mapping;
            node.Tag = obj;
        }

        #endregion

        #region Importing

        private List<ImportObject> defaultNodes = new List<ImportObject>();

        private void switchControls(bool on)
        {
            buttonImport.Enabled = !on;
            buttonChooseFile.Enabled = !on;
            labelFile.Enabled = !on;
            //buttonClose.Enabled = on;
            progressBarImport.Visible = on;
            if (!on)
            {
                labelImportInfo.Text = "Click import to continue";
            }
        }

        private void update(int progress)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<int>(update), new object[] { progress });
                return;
            }

            progressBarImport.Value = progress;
            labelImportInfo.Text = "Importing row " + progress.ToString() + "/" + rowCount.ToString();
        }

        private void endOfProcess(string message)
        {
            if (checkBoxAutoImport.Checked)
            {
                buttonClose_Click(message, new EventArgs());
            }
            else
                MessageBox.Show(this, "The import has completed, you can now close this form", message);
        }

        private void startSaveProcess()
        {
            BeginInvoke(new Action<bool>(switchControls), new object[] { true });
            int r = 1;
            foreach (ImportObject obj in totalObjects)
            {
                SaveObjects(obj, null);
                r++;
                update(r);
                GC.Collect();
            }
            BeginInvoke(new Action<bool>(switchControls), new object[] { false });
            BeginInvoke(new Action<string>(endOfProcess), new object[] { "Import Complete" });
        }

        private Thread sThread;
        private List<ImportObject> totalObjects;
        //List<ImportObject> objectsToSave;
        private void buttonImport_Click(object sender, EventArgs e)
        {
            List<ImportObject> defaults = new List<ImportObject>();
            defaultNodes = buildDefaults(treeViewLayout.Nodes[0].Nodes[0], defaults);
            totalObjects = (treeViewLayout.Nodes[0].Tag as ImportDiagram).Objects;
            progressBarImport.Maximum = (totalObjects.Count + 1);
            sThread = new Thread(startSaveProcess);
            sThread.IsBackground = true;
            sThread.Start();
            Application.DoEvents();
        }

        private static List<ImportObject> buildDefaults(TreeNode node, List<ImportObject> defaults)
        {
            defaults.Add(node.Tag as ImportObject);
            if (node.Nodes.Count > 0)
                defaults = buildDefaults(node.Nodes[0], defaults);

            return defaults;
        }

        private static ImportObject findDefault(int defaultLevel, IEnumerable<ImportObject> defaults)
        {
            foreach (ImportObject o in defaults)
            {
                if (o.Level == defaultLevel)
                    return o;
            }
            return null;
        }

        private void SaveObjects(ImportObject obj, MetaObject passedParent)
        {
            MetaObject savedObject = null;
            ImportObject objDefault = findDefault(obj.Level, defaultNodes);
            if (objDefault != null)
            {
                obj._Class = objDefault._Class;
                obj._AssociationType = objDefault._AssociationType;
            }
            //get default fieldID for class
            TList<Field> classFields = DataRepository.ClientFieldsByClass(obj._Class);
            obj.ClassFields = classFields;
            int identifierFieldID = 0;
            foreach (Field f in classFields)
            {
                if (f.IsActive != true && f.SortOrder != 1) continue;
                identifierFieldID = f.pkid;
                break;
            }
            //look for object that has that field corresponding to its value
            TList<ObjectFieldValue> objectValues = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectFieldValueProvider.GetByFieldID(identifierFieldID);
            foreach (ObjectFieldValue v in objectValues)
            {
                if (v.ValueString == obj.Name)
                {
                    savedObject = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetBypkidMachine(v.ObjectID, v.MachineID);
                }
            }
            if (savedObject == null)
            {
                //save object
                savedObject = createObject(obj);
                if (savedObject != null)
                    createValues(obj, savedObject);
            }
            else
            {
                createValues(obj, savedObject);
            }

            //associate the object
            foreach (ImportObject o in obj.Objects)
            {
                //Save child object
                SaveObjects(o, savedObject);
            }
            if (passedParent == null) // || savedObject == null) 
                return;

            #region Allowed Association

            int CAID = 0;
            TList<ClassAssociation> allowedAssociations = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByParentClass(passedParent.Class);
            int assTypeID = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.AssociationTypeProvider.GetByName(obj._AssociationType.ToString()).pkid;
            foreach (ClassAssociation cAss in allowedAssociations)
            {
                if (cAss.ChildClass != savedObject.Class || cAss.AssociationTypeID != assTypeID) continue;
                CAID = cAss.CAid;
                break;
            }
            if (CAID == 0)
            {
                //didnt find association, use mapping
                foreach (ClassAssociation allowedAssociation in allowedAssociations)
                {
                    if (allowedAssociation.AssociationTypeID != 4) continue;
                    CAID = allowedAssociation.CAid;
                    break;
                }
            }

            #endregion

            //save association
            ObjectAssociation oAss = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(CAID, passedParent.pkid, savedObject.pkid, passedParent.Machine, savedObject.Machine);
            if (oAss == null)
            {
                //save association
                oAss = createAssociation(passedParent, savedObject, CAID);
                //bind it to the new file
            }
            else
            {
                //bind it to the new file
            }
            //bind it to the new file
            //return parentObject;

            if (savedObject != null)
            {
                if (!SavedKeys.ContainsKey(savedObject.pkid))
                    SavedKeys.Add(savedObject.pkid, savedObject.Machine + ":" + savedObject.Class);

                savedObject.Dispose();
                savedObject = null;
            }
            if (oAss != null)
            {
                oAss.Dispose();
                oAss = null;
            }
            obj.ClassFields = null;
        }

        //private readonly TList<MetaObject> savedObjects = new TList<MetaObject>();
        private Dictionary<int, string> SavedKeys;

        public Dictionary<int, string> GetKeys()
        {
            return loadSaved ? SavedKeys : null;
        }

        private MetaObject createObject(ImportObject obj)
        {
            MetaObject mObj = new MetaObject();
            mObj.Class = obj._Class;
            mObj.UserID = Core.Variables.Instance.UserID;
            mObj.WorkspaceName = Core.Variables.Instance.CurrentWorkspaceName;
            mObj.WorkspaceTypeId = Core.Variables.Instance.CurrentWorkspaceTypeId;
            mObj.VCStatusID = 7;
            mObj.Machine = makeMachine();

            mObj = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.Save(mObj);
            //savedObjects.Add(mObj);
            return mObj.pkid > 0 ? mObj : null;
        }

        private void createValues(ImportObject obj, IMetaObject objInDB)
        {
            if (objInDB.pkid <= 0) return;
            bool getFields = obj.ClassFields == null;
            if (getFields)
                obj.ClassFields = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.FieldProvider.GetByClass(obj._Class);
            TList<ObjectFieldValue> values = new TList<ObjectFieldValue>();

            #region Get Values

            if (obj.Name.Length > 0)
            {
                ObjectFieldValue value = new ObjectFieldValue();
                value.ObjectID = objInDB.pkid;
                value.MachineID = objInDB.Machine;
                value.ValueString = obj.Name;
                //default field
                int fieldID = 0;
                foreach (Field f in obj.ClassFields)
                {
                    if (f.IsActive != true && f.SortOrder != 1) continue;
                    fieldID = f.pkid;
                    break;
                }
                value.FieldID = fieldID;
                values.Add(value);
            }

            if (obj.Description.Length > 0)
            {
                ObjectFieldValue value = new ObjectFieldValue();
                value.ObjectID = objInDB.pkid;
                value.MachineID = objInDB.Machine;
                value.ValueString = obj.Description;
                //if there is a description field else custom field 2
                int fieldID = 0;
                foreach (Field f in obj.ClassFields)
                {
                    if (f.Name == "Description")
                    {
                        fieldID = f.pkid;
                    }
                }
                if (fieldID == 0)
                {
                    foreach (Field f in obj.ClassFields)
                    {
                        if (f.Name != "CustomField2") continue;
                        fieldID = f.pkid;
                        break;
                    }
                }
                value.FieldID = fieldID;
                values.Add(value);
            }


            if (obj.Objective.Length > 0)
            {
                ObjectFieldValue value = new ObjectFieldValue();
                value.ObjectID = objInDB.pkid;
                value.MachineID = objInDB.Machine;
                value.ValueString = obj.Objective;
                //custom field 1
                int fieldID = 0;
                foreach (Field f in obj.ClassFields)
                {
                    if (f.Name != "CustomField1") continue;
                    fieldID = f.pkid;
                    break;
                }
                value.FieldID = fieldID;
                values.Add(value);
            }

            #endregion

            foreach (ObjectFieldValue objectFieldValue in values)
            {
                saveFieldValue(objectFieldValue);
            }
        }

        private void saveFieldValue(ObjectFieldValue fieldToSave)
        {
            try
            {
                ObjectFieldValue fieldInDB = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectFieldValueProvider.GetByObjectIDFieldIDMachineID(fieldToSave.ObjectID, fieldToSave.FieldID, fieldToSave.MachineID);
                if (fieldInDB == null)
                    DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectFieldValueProvider.Save(fieldToSave);
                else
                {
                    DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectFieldValueProvider.Delete(fieldInDB); //MAGIC!
                    DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectFieldValueProvider.Save(fieldToSave);
                }
                if (fieldInDB != null)
                {
                    fieldInDB.Dispose();
                    fieldInDB = null;
                }
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog(ex.ToString());
            }
        }

        private static ObjectAssociation createAssociation(IMetaObject parentObj, IMetaObject childObj, int CAID)
        {
            if (childObj == null) throw new ArgumentNullException("childObj");
            ObjectAssociation objAss = new ObjectAssociation();
            objAss.CAid = CAID;
            objAss.ObjectID = parentObj.pkid;
            objAss.ChildObjectID = childObj.pkid;
            objAss.ObjectMachine = parentObj.Machine;
            objAss.ChildObjectMachine = childObj.Machine;
            objAss.MachineName = Environment.MachineName;
            objAss.VCUser = Core.Variables.Instance.UserDomainIdentifier;
            objAss.VCStatusID = 7;
            try
            {
                objAss = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Save(objAss);
            }
            catch (Exception ex)
            {
                //it exists
                Core.Log.WriteLog(ex.ToString());
            }
            return objAss.pkid > 0 ? objAss : null;
        }

        private static string makeMachine()
        {
            Random r = new Random();
            int rN = r.Next(0, 999);
            return Environment.MachineName + "-" + rN.ToString();
        }

        #endregion

    }
}