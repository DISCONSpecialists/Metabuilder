using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Meta;
using XPTable.Events;
using XPTable.Models;
using XPTable.Renderers;

namespace MetaBuilder.MetaControls
{
    public partial class AssociationList : UserControl
    {

        #region Fields (5)

        private bool allowMultipleSelection;
        TList<ClassAssociation> classAssocs;
        private int limitToStatus;
        [Browsable(false)]
        private Dictionary<ObjectAssociationKey, ObjectAssociationInclObj> listedAssociations;
        [Browsable(false)]
        private Dictionary<ObjectAssociationKey, ObjectAssociationInclObj> selectedAssociations;

        #endregion Fields

        #region Constructors (1)

        public AssociationList()
        {
            InitializeComponent();
            table1.HeaderRenderer = new GradientHeaderRenderer();
            table1.EnableHeaderContextMenu = false;
            tableSelection.HeaderRenderer = new GradientHeaderRenderer();
        }

        #endregion Constructors

        #region Properties (4)

        public bool AllowMultipleSelection
        {
            get { return allowMultipleSelection; }
            set { allowMultipleSelection = value; }
        }

        public int LimitToStatus
        {
            get { return limitToStatus; }
            set { limitToStatus = value; }
        }

        [Browsable(false)]
        public Dictionary<ObjectAssociationKey, ObjectAssociationInclObj> ListedAssociations
        {
            get { return listedAssociations; }
            set { listedAssociations = value; }
        }

        [Browsable(false)]
        public Dictionary<ObjectAssociationKey, ObjectAssociationInclObj> SelectedAssociations
        {
            get { return selectedAssociations; }
            set { selectedAssociations = value; }
        }

        #endregion Properties

        delegate void USRDelegate();
        public void USR()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new USRDelegate(USR), new object[] { });
            }
            else
            {
                if (ListedAssociations.Count == 0)
                    UpdateSearchResult();

                BindList();
            }
        }

        #region Methods (13)


        // Public Methods (7) 

        public void BindList()
        {
            tableModel1.Rows.Clear();
            if (listedAssociations.Values.Count > 0)
            {
                foreach (ObjectAssociationInclObj incl in listedAssociations.Values)
                {
                    try
                    {


                        Row r = AddRowToTableModel(tableModel1, incl);

                    }
                    catch { }
                }
            }
        }

        public void RemoveFromTargetTableModel(TableModel model, ObjectAssociationKey key)
        {
            List<Row> rowsToRemove = new List<Row>();
            foreach (Row r in model.Rows)
            {
                ObjectAssociationKey rowKey = GetKey(r);
                if (rowKey.CAid == key.CAid && rowKey.ObjectID == key.ObjectID && rowKey.ChildObjectID == key.ChildObjectID && rowKey.ObjectMachine == key.ObjectMachine && rowKey.ChildObjectMachine == key.ChildObjectMachine)
                {
                    rowsToRemove.Add(r);
                }
            }

            for (int i = 0; i < rowsToRemove.Count; i++)
            {
                model.Rows.Remove(rowsToRemove[i]);
            }

        }

        public void RemoveItem(ObjectAssociationKey key)
        {
            RemoveFromTargetTableModel(tableModel1, key);
            RemoveFromTargetTableModel(tableModelSelection, key);
        }

        public void SelectAll()
        {
            if (selectedAssociations.Count > 0 && AllowMultipleSelection == false)
            {
                MessageBox.Show(this,"You are only allowed to select one object");
                UpdateSelection();
                return;
            }
            foreach (Row r in table1.TableModel.Rows)
            {
                try
                {
                    r.Cells[0].Checked = true;
                }
                catch
                {
                }
            }
        }

        public void SelectNone()
        {
            selectedAssociations = new Dictionary<ObjectAssociationKey, ObjectAssociationInclObj>();
            UpdateSelection();
            UpdateSearchResult();
        }

        public void UpdateSearchResult()
        {
            foreach (Row r in table1.TableModel.Rows)
            {
                ObjectAssociationKey key = GetKey(r);
                if (key != null)
                    r.Cells[0].Checked = selectedAssociations.ContainsKey(key);
            }
        }

        public void UpdateSelection()
        {
            tableModelSelection.Rows.Clear();
            if (selectedAssociations.Values.Count > 0)
            {
                foreach (ObjectAssociationInclObj incl in selectedAssociations.Values)
                {
                    try
                    {
                        Row r = GetNewRow();
                        r.Cells[0].Checked = IsCurrentlySelected(incl);
                        r.Cells[1].Text = incl.FromObject._ClassName + " to " + incl.ToObject._ClassName;
                        r.Cells[1].Tag = incl.ObjectAss;
                        r.Cells[2].Text = incl.FromObject.ToString();
                        r.Cells[3].Text = incl.ToObject.ToString();
                        tableModelSelection.Rows.Add(r);
                    }
                    catch { }
                }
            }

        }



        // Private Methods (6) 

        private Row AddRowToTableModel(TableModel targetTableModel, ObjectAssociationInclObj incl)
        {
            Row r = GetNewRow();
            r.Cells[0].Checked = IsCurrentlySelected(incl);
            r.Cells[1].Text = GetAssociationTypeListByCAid(incl.ObjectAss.CAid).ToString() + "[" + incl.FromObject._ClassName + " to " + incl.ToObject._ClassName + "]";
            r.Cells[1].Tag = incl.ObjectAss;
            r.Cells[2].Text = incl.FromObject.ToString();
            r.Cells[3].Text = incl.ToObject.ToString();
            r.Cells[4].Text = ((VCStatusList)incl.ObjectAss.VCStatusID).ToString();
            targetTableModel.Rows.Add(r);
            return r;
        }

        private AssociationTypeList GetAssociationTypeListByCAid(int CAid)
        {
            if (classAssocs == null)
                classAssocs = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetAll();

            classAssocs.Filter = "CAid = " + CAid.ToString() + " AND IsActive = 'True'";
            return (AssociationTypeList)classAssocs[0].AssociationTypeID;
        }

        private static ObjectAssociationKey GetKey(Row r)
        {
            if (r.Cells.Count > 1)
            {
                ObjectAssociationKey mkey = new ObjectAssociationKey(r.Cells[1].Tag as ObjectAssociation);
                return mkey;
            }
            return null;

        }

        private Row GetNewRow()
        {
            Row retval = new Row();
            foreach (Column c in columnModel1.Columns)
            {
                retval.Cells.Add(new Cell());
            }
            return retval;
        }

        private bool IsCurrentlySelected(ObjectAssociationInclObj incl)
        {
            ObjectAssociationKey assocKey = new ObjectAssociationKey(incl.ObjectAss);
            return (selectedAssociations.ContainsKey(assocKey));
        }

        private void tableSelection_CellPropertyChanged(object sender, CellEventArgs e)
        {
            if (!e.Cell.Checked)
            {
                ObjectAssociationKey oakey = GetKey(e.Cell.Row);
                DialogResult result = MessageBox.Show(this,"Remove from selection?", "Remove Item", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    selectedAssociations.Remove(oakey);
                    UpdateSelection();
                    UpdateSearchResult();
                }
                else
                {
                    UpdateSelection();
                }
            }
        }

        #endregion Methods

        #region Grid Events
        private void table1_CellPropertyChanged(object sender, CellEventArgs e)
        {
            ObjectAssociationKey mkey = new ObjectAssociationKey(e.Cell.Row.Cells[1].Tag as ObjectAssociation);
            ObjectAssociationInclObj mbase = listedAssociations[mkey];
            if (e.Cell.Checked)
            {
                if (selectedAssociations.Count > 0 && AllowMultipleSelection == false)
                {
                    MessageBox.Show(this,"You are only allowed to select one object");
                    UpdateSelection();
                    return;
                }
                selectedAssociations.Add(mkey, mbase);
            }
            else
            {
                selectedAssociations.Remove(mkey);
            }
            UpdateSelection();
        }
        #endregion
    }

    [Serializable]
    public class ObjectAssociationInclObj
    {

        #region Fields (3)

        private MetaBase fromObject;
        private ObjectAssociation objectAss;
        private MetaBase toObject;

        #endregion Fields

        #region Properties (3)

        public MetaBase FromObject
        {
            get { return fromObject; }
            set { fromObject = value; }
        }

        public ObjectAssociation ObjectAss
        {
            get { return objectAss; }
            set { objectAss = value; }
        }

        public MetaBase ToObject
        {
            get { return toObject; }
            set { toObject = value; }
        }

        #endregion Properties

    }
}
