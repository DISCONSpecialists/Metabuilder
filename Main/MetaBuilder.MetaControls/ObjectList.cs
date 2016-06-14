using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Meta;
using XPTable.Events;
using XPTable.Models;

namespace MetaBuilder.MetaControls
{
    public partial class ObjectList : UserControl
    {

        #region Constructors (1)

        private bool Server = false;
        public ObjectList(bool server)
        {
            Server = server;
            InitializeComponent();
            selectedObjectsDictionary = new Dictionary<MetaObjectKey, MetaBase>();
            listedObjects = new List<MetaBase>();
            listedObjectsDictionary = new Dictionary<MetaObjectKey, MetaBase>();
            objectTable.EnableHeaderContextMenu = false;
            tabControlAdv1_SelectedIndexChanged(this, EventArgs.Empty);
        }

        #endregion Constructors

        #region Delegates and Events (2)

        // Events (2) 

        public event EventHandler OpenDiagram;

        public event ViewInContextEventHandler ViewInContext;

        #endregion Delegates and Events

        #region Methods (9)

        // Public Methods (4) 

        delegate void USRDelegate();
        public void USR()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new USRDelegate(USR), new object[] { });
            }
            else
            {
                if (ListedObjectsDictionary.Count == 0)
                    UpdateSearchResult();

                BindList();
                tabControlAdv1.SelectedTab = tabPageBrowse;
                Application.DoEvents();
            }
        }

        delegate void AddListedObjectDictionaryItemSafelyDelegate(MetaObjectKey key, MetaBase mbase);
        public void AddListedObjectDictionaryItemSafely(MetaObjectKey key, MetaBase mbase)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new AddListedObjectDictionaryItemSafelyDelegate(AddListedObjectDictionaryItemSafely), new object[] { key, mbase });
            }
            else
            {
                if (!listedObjectsDictionary.ContainsKey(key))
                {
                    listedObjectsDictionary.Add(key, mbase);
                }
            }
        }

        public void AddSelectedObjectDictionaryItemSafely(MetaObjectKey key, MetaBase mbase)
        {

            if (!selectedObjectsDictionary.ContainsKey(key))
            {
                selectedObjectsDictionary.Add(key, mbase);
            }
        }

        public void SelectAll()
        {
            MultiSelectInProgress = true;
            if (selectedObjectsDictionary.Count > 0 && AllowMultipleSelection == false)
            {
                MessageBox.Show(this,"You are only allowed to select one object");
                UpdateSelection();
                UpdateSearchResult();
                MultiSelectInProgress = false;
                return;
            }
            foreach (Row r in objectTable.TableModel.Rows)
            {
                r.Cells[0].Checked = true;
            }
            UpdateSelection();
            UpdateSearchResult();
            MultiSelectInProgress = false;
        }

        public void SelectNone()
        {
            MultiSelectInProgress = true;
            selectedObjectsDictionary = new Dictionary<MetaObjectKey, MetaBase>();
            UpdateSelection();
            UpdateSearchResult();
            MultiSelectInProgress = false;
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

        // Private Methods (3) 

        void objectProperties_ViewInContext(MetaBase mbase)
        {
            OnViewInContext(mbase);
        }

        void objectPropertiesWindow_OpenDiagram(object sender, EventArgs e)
        {
            OnOpenDiagram(sender);
        }

        private void objectTable_CellButtonClicked(object sender, CellButtonEventArgs e)
        {
            MetaObjectKey key = GetKey(e.Cell.Row);
            MetaBase mbase = ListedObjectsDictionary[key];
            ObjectPropertiesWindow objectPropertiesWindow = new ObjectPropertiesWindow(Server);
            objectPropertiesWindow.SelectedObject = mbase;
            objectPropertiesWindow.ViewInContext += new ViewInContextEventHandler(objectProperties_ViewInContext);
            objectPropertiesWindow.OpenDiagram += new EventHandler(objectPropertiesWindow_OpenDiagram);
            objectPropertiesWindow.ShowDialog(this);
        }

        private void tabControlAdv1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            tabPageBrowse.TabBackColor = System.Drawing.SystemColors.InactiveCaption;
            tabPageSelection.TabBackColor = System.Drawing.SystemColors.InactiveCaption;
            tabPageBrowse.TabForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            tabPageSelection.TabForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            if (tabControlAdv1.SelectedTab == tabPageBrowse)
            {
                tabPageBrowse.TabBackColor = System.Drawing.SystemColors.ActiveCaption;
                tabPageBrowse.TabForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            }
            else if (tabControlAdv1.SelectedTab == tabPageSelection)
            {
                tabPageSelection.TabBackColor = System.Drawing.SystemColors.ActiveCaption;
                tabPageSelection.TabForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            }

        }

        #endregion Methods

        //this is so we know we are loading from database
        //make enum for each type of objectlist
        private bool lfdb = false;
        public bool LFDB
        {
            get { return lfdb; }
            set
            {
                lfdb = value;
            }
        }
        private ToolTip t = new ToolTip();

        #region Setup Properties
        private bool allowMultipleSelection;
        public bool AllowMultipleSelection
        {
            get { return allowMultipleSelection; }
            set { allowMultipleSelection = value; }
        }
        #endregion

        #region Lists and Dictionaries
        private List<MetaBase> listedObjects;
        [Browsable(false)]
        public List<MetaBase> ListedObjects
        {
            get { return listedObjects; }
            set { listedObjects = value; }
        }

        private Dictionary<MetaObjectKey, MetaBase> selectedObjectsDictionary;
        [Browsable(false)]
        public Dictionary<MetaObjectKey, MetaBase> SelectedObjectsDictionary
        {
            get { return selectedObjectsDictionary; }
            set { selectedObjectsDictionary = value; }
        }

        private Dictionary<MetaObjectKey, MetaBase> listedObjectsDictionary;
        [Browsable(false)]
        public Dictionary<MetaObjectKey, MetaBase> ListedObjectsDictionary
        {
            get { return listedObjectsDictionary; }
            set { listedObjectsDictionary = value; }

        }


        public List<MetaBase> SelectedObjectsList
        {
            get
            {
                List<MetaBase> retval = new List<MetaBase>();
                foreach (KeyValuePair<MetaObjectKey, MetaBase> kvp in SelectedObjectsDictionary)
                {
                    retval.Add(kvp.Value);
                }
                return retval;
            }
        }
        #endregion

        #region Selection

        private bool multiSelectInProgress;
        public bool MultiSelectInProgress
        {
            get { return multiSelectInProgress; }
            set { multiSelectInProgress = value; }
        }

        private void objectTable_CellPropertyChanged(object sender, CellEventArgs e)
        {
            MetaObjectKey mkey = new MetaObjectKey();
            mkey.pkid = int.Parse(e.Cell.Row.Cells[5].Text);
            mkey.Machine = e.Cell.Row.Cells[6].Text;
            MetaBase mbase = listedObjectsDictionary[mkey];
            if (e.Cell.Checked)
            {
                if (SelectedObjectsDictionary.Count > 0 && AllowMultipleSelection == false)
                {
                    //MessageBox.Show(this,"You are only allowed to select one object");

                    t.ToolTipIcon = ToolTipIcon.Info;
                    t.ToolTipTitle = "You are only allowed to select one object";
                    t.InitialDelay = 0;
                    t.IsBalloon = false;
                    t.UseAnimation = false;
                    t.UseFading = false;
                    t.Show("The object was not selected", this, this.Right - 100, this.Bottom - 500, 5000);

                    e.Cell.Checked = false;
                    UpdateSelection();
                    return;
                }
                AddSelectedObjectDictionaryItemSafely(mkey, mbase);
            }
            else
            {
                SelectedObjectsDictionary.Remove(mkey);
            }
            if (!MultiSelectInProgress)
                UpdateSelection();
        }
        private void tableSelection_CellPropertyChanged(object sender, CellEventArgs e)
        {
            if (!e.Cell.Checked)
            {
                MetaObjectKey mkey = GetKey(e.Cell.Row);
                selectedObjectsDictionary.Remove(mkey);
                UpdateSelection();
                UpdateSearchResult();
            }
        }
        public void UpdateSelection()
        {
            tableModelSelection.Rows.Clear();
            if (SelectedObjectsDictionary.Values.Count > 0)
            {
                foreach (MetaBase mbase in SelectedObjectsDictionary.Values)
                {
                    AddRowToGrid(tableModelSelection, mbase);
                }
            }
        }

        #endregion

        #region Helper Methods
        private Row GetNewRow()
        {
            Row retval = new Row();
            foreach (Column c in columnModel1.Columns)
            {
                retval.Cells.Add(new Cell());
            }
            return retval;
        }
        private void AddRowToGrid(TableModel targetTableModel, MetaBase mbase)
        {
            //SOMWPM
            Row r = GetNewRow();
            r.Cells[0].Checked = IsCurrentlySelected(mbase);
            r.Cells[0].Tag = mbase;
            try
            {
                r.Cells[1].Text = mbase.ToString();
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog(mbase.Class + " cannot determine string" + Environment.NewLine + ex.ToString());
                r.Cells[1].Text = "|!|CANNOT DETERMINE VALUE|!|";
                r.ForeColor = System.Drawing.Color.Red;
            }

            r.Cells[2].Text = mbase._ClassName;
            r.Cells[3].Text = "...";
            r.Cells[4].Text = mbase.WorkspaceName;
            r.Cells[5].Text = mbase.pkid.ToString();
            r.Cells[6].Text = mbase.MachineName;
            if (Core.Variables.Instance.IsServer)
                r.Cells[7].Text = mbase.State.ToString();

            targetTableModel.Rows.Add(r);
        }

        private bool IsCurrentlySelected(MetaBase mbase)
        {
            MetaObjectKey mobjectkey = new MetaObjectKey();
            mobjectkey.Machine = mbase.MachineName;
            mobjectkey.pkid = mbase.pkid;
            if (SelectedObjectsDictionary.ContainsKey(mobjectkey))
            {
                return true;
            }
            return false;
        }

        public void UpdateSearchResult()
        {
            foreach (Row r in objectTable.TableModel.Rows)
            {
                MetaObjectKey key = GetKey(r);
                r.Cells[0].Checked = selectedObjectsDictionary.ContainsKey(key);
            }
        }

        public void RemoveItem(MetaObjectKey key)
        {
            RemoveItemFromTargetTableModel(tableModel1, key);
            RemoveItemFromTargetTableModel(tableModelSelection, key);
        }

        private void RemoveItemFromTargetTableModel(TableModel targetModel, MetaObjectKey key)
        {
            List<Row> rowsToRemove = new List<Row>();
            foreach (Row r in targetModel.Rows)
            {
                MetaObjectKey k = GetKey(r);
                if (k.pkid.ToString() + k.Machine == key.pkid.ToString() + key.Machine)
                {
                    rowsToRemove.Add(r);
                }
            }
            for (int i = 0; i < rowsToRemove.Count; i++)
            {
                targetModel.Rows.Remove(rowsToRemove[i]);
            }
        }

        private static MetaObjectKey GetKey(Row r)
        {
            MetaObjectKey mkey = new MetaObjectKey();
            mkey.pkid = int.Parse(r.Cells[5].Text);
            mkey.Machine = r.Cells[6].Text;
            return mkey;
        }
        #endregion

        #region Binding

        public void BindList()
        {
            tableModel1.Rows.Clear();
            if (listedObjectsDictionary.Values.Count > 0)
            {
                foreach (MetaBase mbase in listedObjectsDictionary.Values)
                {
                    AddRowToGrid(tableModel1, mbase);
                }
            }
            this.objectTable.NoItemsText = "No Items Found";
        }

        #endregion


        private void objectTable_CellCheckChanged(object sender, CellCheckBoxEventArgs e)
        {
            if (lfdb)
            {
                Row r = this.objectTable.TableModel.Rows[e.Row];

                string classClicked = r.Cells[2].Text;
                if (Core.Variables.Instance.ClassesWithoutStencil.Contains(classClicked))
                {
                    t.ToolTipIcon = ToolTipIcon.Info;
                    t.ToolTipTitle = "This object cannot be loaded from the database directly";
                    t.InitialDelay = 0;
                    t.IsBalloon = false;
                    t.UseAnimation = false;
                    t.UseFading = false;

                    if (classClicked == "TimeScheme" || classClicked == "LocationScheme")
                        t.Show(classClicked + "s can only be container shapes.", this, this.Right - 100, this.Bottom - 500, 5000);
                    else
                        t.Show(classClicked + "s load via their parent object(s).", this, this.Right - 100, this.Bottom - 500, 5000);

                    e.Cell.Checked = false;
                }

                //ObjectFinderControl objFC = this.Parent.Parent as ObjectFinderControl;
                //ParentFormOK.Enabled = SelectedObjectsDictionary.Values.Count > 0;
            }
        }
    }

    /*
     * r[0] = mbase as tag /checkbox
       r.Cells[1].Text = mbase.ToString();
       r.Cells[2].Text = mbase._ClassName;
       r.Cells[3].Text = "...";
       r.Cells[4].Text = mbase.WorkspaceName;
       r.Cells[5].Text = mbase.pkid.ToString();
       r.Cells[6].Text = mbase.MachineName;
     * */
}