using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MetaBuilder.BusinessFacade.Exports;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Docking;
using MetaBuilder.Meta;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using XPTable.Events;
using XPTable.Models;
using XPTable.Renderers;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
using ObjectAssociation=MetaBuilder.BusinessLogic.ObjectAssociation;

using TraceTool;

namespace MetaBuilder.UIControls.Dialogs.RelationshipManager
{
    public delegate void AssociationEventHandler(object sender, int CAid, int ObjectID, int ChildObjectID, string ObjectMachine, string ChildObjectMachine);
    public partial class TableContainer : DockContent
    {
        public string GetConnectionString()
        {
            if (toolStripComboBox1.Text == "Connection: Server" && Core.Variables.Instance.IsServer)
            {
                return Core.Variables.Instance.ServerProvider;
            }
            else
            {
                return Core.Variables.Instance.ClientProvider;
            }
        }

        #region Fields (1)

        private const string VersionControlMenuText = " (Version Controlled)";

        #endregion Fields

        #region Methods (4)

        // Public Methods (2) 

        public void ResumeTableLayout()
        {
            table1.ResumeLayout(true);
        }

        public void SuspendTableLayout()
        {
            table1.SuspendLayout();
        }

        // Private Methods (2) 

        private void AddToList(b.ObjectAssociationKey keyValue)
        {
            if (!(associations.Contains(keyValue)))
                associations.Add(keyValue);
        }

        private void RemoveFromAssociationList(b.ObjectAssociationKey keyValue)
        {
            if ((associations.Contains(keyValue)))
                associations.Remove(keyValue);
        }

        #endregion Methods

        #region Nested Classes (1)

        private class ArtefactArgs
        {

            #region Fields (5)

            public int CAid;
            public int ChildObjectID;
            public string ChildObjectMachine;
            public int ObjectID;
            public string ObjectMachine;

            #endregion Fields

        }
        #endregion Nested Classes

        #region Constructor

        public TableContainer()
        {

            InitializeComponent();
            rowObjects = new List<MetaBase>();
            columnObjects = new List<MetaBase>();
            dtTable = new DataTable();
            associations = new List<ObjectAssociationKey>();

            allowedArtefacts = new Hashtable();
            TList<AllowedArtifact> allArtefacts = DataRepository.Connections[GetConnectionString()].Provider.AllowedArtifactProvider.GetAll();
            foreach (AllowedArtifact allowedARt in allArtefacts)
            {
                if (allowedArtefacts.ContainsKey(allowedARt.CAid))
                {
                    // do nothing
                }
                else
                {
                    allowedArtefacts.Add(allowedARt.CAid, null);
                }
            }

            InitialiseMatrix();
        }

        #endregion

        #region Properties

        ToolTip ttip;
        Hashtable allowedArtefacts;
        private DataTable dtTable;
        private List<MetaBase> rowObjects;
        private List<MetaBase> columnObjects;
        List<ObjectAssociationKey> associations;
        private bool isAddingList;
        public bool IsAddingList
        {
            get { return isAddingList; }
            set { isAddingList = value; }
        }

        #endregion

        #region Events

        public event EventHandler RowItemAdded;
        protected void OnRowItemAdded(object sender, EventArgs e)
        {
            if (RowItemAdded != null)
            {
                RowItemAdded(sender, e);
            }
        }

        public event EventHandler ColumnItemAdded;
        protected void OnColumnItemAdded(object sender, EventArgs e)
        {
            if (ColumnItemAdded != null)
            {
                ColumnItemAdded(sender, e);
            }
        }

        public event AssociationEventHandler SelectedAssociationChanged;
        protected void OnSelectedAssociationChanged(object sender, int CAid, int ObjectID, int ChildObjectID, string ObjectMachine, string ChildObjectMachine)
        {
            if (SelectedAssociationChanged != null)
            {
                SelectedAssociationChanged(sender, CAid, ObjectID, ChildObjectID, ObjectMachine, ChildObjectMachine);
            }
        }

        #endregion

        #region Helper Methods

        private bool IsDeletableByState(b.ObjectAssociation assoc)
        {
            return (assoc.State == VCStatusList.None || assoc.State == VCStatusList.MarkedForDelete);
        }

        private bool NotInColumns(MetaBase mb)
        {
            foreach (MetaBase mbExisting in columnObjects)
            {
                if (mbExisting.pkid == mb.pkid && mbExisting.MachineName == mb.MachineName)
                    return false;
            }
            return true;
        }

        private bool IsAssociated(MetaBase mbRowObject, MetaBase mbColumnObject)
        {
            bool retval = false;
            ObjectPairAssociationCollection opa = new ObjectPairAssociationCollection();
            opa.Load(mbRowObject, mbColumnObject);
            if (opa.Associations.Count > 0)
            {
                foreach (ObjectPairAssociationCollection.MatrixObjectAssociation moa in opa.Associations)
                {
                    if (!(VCStatusTool.IsObsoleteOrMarkedForDelete(moa)))
                    {
                        retval = true;

                        b.ObjectAssociationKey okey = new ObjectAssociationKey();
                        okey.CAid = moa.CAid;
                        okey.ObjectID = moa.ObjectID;
                        okey.ChildObjectID = moa.ChildObjectID;
                        okey.ObjectMachine = moa.ObjectMachine;
                        okey.ChildObjectMachine = moa.ChildObjectMachine;
                        AddToList(okey);

                    }
                }
            }

            return retval;
        }

        private bool NotInRows(MetaBase mb)
        {
            foreach (MetaBase mbExisting in rowObjects)
            {
                if (mbExisting.pkid == mb.pkid && mbExisting.MachineName == mb.MachineName)
                    return false;
            }
            return true;
        }

        #region Row And Column Manipulation

        private void AddRowObject(MetaBase mb)
        {
            if (NotInRows(mb))
            {
                rowObjects.Add(mb);
                // Add a datarow to the datatable
                DataRow dr = dtTable.NewRow();
                dr["Object"] = mb.ToString() + " [" + mb._ClassName + "]";
                dtTable.Rows.Add(dr);
                dtTable.AcceptChanges();

                // Add a row to the tablemodel
                Row r = new Row();
                r.Cells.Add(new Cell());
                r.Cells[0].Text = mb.ToString() + " [" + mb._ClassName + "]";

                // Iterate Columns, checking them as we move along
                foreach (DataColumn col in dtTable.Columns)
                {
                    MetaBase mbCol = col.ExtendedProperties["Object"] as MetaBase;
                    if (mbCol != null)
                    {
                        Cell c = new Cell();
                        c.Checked = IsAssociated(mb, mbCol);

                        // Add to list here
                        r.Cells.Add(c);
                    }
                }
                tableModel1.Rows.Add(r);
            }
        }

        private void RemoveRowObject(MetaBase mb)
        {
            int rowIndex = rowObjects.IndexOf(mb);
            dtTable.Rows.RemoveAt(rowObjects.IndexOf(mb));
            rowObjects.Remove(mb);
            tableModel1.Rows.RemoveAt(rowIndex);
        }

        private void AddColumnObject(MetaBase mb)
        {
            try
            {
                if (NotInColumns(mb))
                {
                    columnObjects.Add(mb);

                    // Add a checkboxcolumn to the columnmodel
                    CheckBoxColumn headerCol = new CheckBoxColumn();
                    headerCol.Alignment = ColumnAlignment.Center;
                    headerCol.Editable = false;
                    headerCol.Enabled = true;
                    headerCol.CheckStyle = XPTable.Models.CheckBoxColumnStyle.RadioButton;
                    headerCol.Text = mb.ToString() + " [" + mb._ClassName + "]";
                    headerCol.Sortable = false;
                    columnModel1.Columns.Add(headerCol);

                    // Add a column to the datatable
                    DataColumn newColumn = new DataColumn(GetKey(mb), typeof(bool));

                    newColumn.ExtendedProperties.Add("Object", mb);
                    dtTable.Columns.Add(newColumn);
                    dtTable.AcceptChanges();

                    for (int i = 0; i < rowObjects.Count; i++)
                    {
                        try
                        {
                            tableModel1.Rows[i].Cells.Add(new Cell());
                            tableModel1.Rows[i].Cells[tableModel1.Rows[i].Cells.Count - 1].Checked =
                                IsAssociated(rowObjects[i], mb);
                        }
                        catch
                        {
                        }
                        // add to list here
                    }
                }
            }
            catch
            {
            }
        }

        private void RemoveColumnObject(MetaBase mb)
        {
            int colIndex = columnObjects.IndexOf(mb);
            columnModel1.Columns.RemoveAt(colIndex + 1);
            for (int i = 0; i < tableModel1.Rows.Count; i++)
            {
                tableModel1.Rows[i].Cells.RemoveAt(colIndex + 1);
            }

            dtTable.Columns.RemoveAt(colIndex + 1);
            columnObjects.Remove(mb);
        }

        #endregion

        private string GetKey(MetaBase mb)
        {
            //20 February 2013
            return mb.ToString() + " " + mb.pkid.ToString() + " [" + mb._ClassName + "]" + " (Null)";
            //string storage = mb.ToString() + " [" + mb._ClassName + "]";
            //if (storage != null)
            //{
            //    //if (storage.Length == 0)
            //        storage = mb._ClassName + " " + mb.pkid.ToString() + " [" + mb._ClassName + "]" + " (Null)";
            //}
            //else
            //{
            //    storage = mb._ClassName + " " + mb.pkid.ToString() + " [" + mb._ClassName + "]" + " (Null)";
            //}
            //return storage;
        }
        private bool ObjectAssociationKeysAreEqual(ObjectAssociationKey key, ObjectAssociationKey key2)
        {
            return (key2.CAid == key.CAid && key2.ObjectID == key.ObjectID && key2.ObjectMachine == key.ObjectMachine && key2.ChildObjectID == key.ChildObjectID && key2.ChildObjectMachine == key.ChildObjectMachine);
        }
        private SerializableRelationshipView.ObjectIDentifier GetObjectIDentifier(MetaBase rowObj)
        {
            SerializableRelationshipView.ObjectIDentifier rowIdentifier = new SerializableRelationshipView.ObjectIDentifier();
            rowIdentifier.Key = new MetaObjectKey();
            rowIdentifier.Key.pkid = rowObj.pkid;
            rowIdentifier.Key.Machine = rowObj.MachineName;
            rowIdentifier.ObjectClass = rowObj._ClassName;
            rowIdentifier.StringValue = GetKey(rowObj);
            return rowIdentifier;
        }

        #endregion

        #region Event Handling

        private void linkItem_Click(object sender, EventArgs e)
        {
            OnSelectedAssociationChanged(this, 0, 0, 0, "", "");
            ToolStripMenuItem mitem = sender as ToolStripMenuItem;

            ObjectPairAssociationSpecification tag = mitem.Tag as ObjectPairAssociationSpecification;
            AssociationHelper asshelper = new AssociationHelper();
            int rowIndex = rowObjects.IndexOf(tag.objectPair.ParentObject);
            int colIndex = columnObjects.IndexOf(tag.objectPair.ChildObject);

            if (mitem.Checked)
            {
                #region Remove Association
                ObjectAssociationKey assocKey = new ObjectAssociationKey();
                assocKey.CAid = tag.classAssociationDefinition.CAid;
                assocKey.ChildObjectID = tag.objectPair.ChildObject.pkid;
                assocKey.ChildObjectMachine = tag.objectPair.ChildObject.MachineName;
                assocKey.ObjectID = tag.objectPair.ParentObject.pkid;
                assocKey.ObjectMachine = tag.objectPair.ParentObject.MachineName;
                ObjectAssociation obassoc = DataRepository.Connections[GetConnectionString()].Provider.ObjectAssociationProvider.Get(assocKey);
                if (obassoc != null)
                {
                    if (IsReadOnly(obassoc.VCStatusID))
                    {
                        MessageBox.Show(this,"This association is under Version Control. To edit it, you need to have write access (Checked Out)", "Constraint", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    TList<GraphFileAssociation> graphFileAssocs =
                        DataRepository.Connections[GetConnectionString()].Provider.GraphFileAssociationProvider.
                            GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(
                            assocKey.CAid, assocKey.ObjectID, assocKey.ChildObjectID, assocKey.ObjectMachine,
                            assocKey.ChildObjectMachine);

                    if (obassoc.State != VCStatusList.None || graphFileAssocs.Count > 0)
                    {
                        try
                        {
                            VCStatusList newStatus = VCStatusList.MarkedForDelete;
                            UpdateStatus(obassoc, newStatus);

                            b.ObjectAssociation otherAssoc = GetTwoWayAssociation(obassoc);
                            if (otherAssoc != null)
                            {
                                UpdateStatus(otherAssoc, newStatus);
                            }
                        }
                        catch
                        {
                        }
                    }

                    bool mainAssociationDeletable = (IsDeletableByState(obassoc) && graphFileAssocs.Count == 0);
                    bool otherAssociationDeletable = true;
                    b.ObjectAssociation otherobAssoc = GetTwoWayAssociation(obassoc);
                    if (otherobAssoc != null)
                    {
                        TList<GraphFileAssociation> graphFileAssocsOther =
                          DataRepository.Connections[GetConnectionString()].Provider.GraphFileAssociationProvider.
                         GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(
                         assocKey.CAid, otherobAssoc.ObjectID, otherobAssoc.ChildObjectID, otherobAssoc.ObjectMachine,
                         otherobAssoc.ChildObjectMachine);
                        otherAssociationDeletable = IsDeletableByState(otherobAssoc);

                        // remove from list here
                        b.ObjectAssociationKey okey = new ObjectAssociationKey(otherobAssoc);

                        RemoveFromAssociationList(okey);
                    }

                    if (mainAssociationDeletable && otherAssociationDeletable)
                    {
                        try
                        {
                            DataRepository.Connections[GetConnectionString()].Provider.GraphFileAssociationProvider.Delete(graphFileAssocs);
                            if (otherobAssoc != null)
                            {
                                TList<GraphFileAssociation> graphFileAssocsOther =
                                    DataRepository.Connections[GetConnectionString()].Provider.GraphFileAssociationProvider.
                                        GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(
                                        assocKey.CAid, otherobAssoc.ObjectID, otherobAssoc.ChildObjectID,
                                        otherobAssoc.ObjectMachine,
                                        otherobAssoc.ChildObjectMachine);

                                DataRepository.Connections[GetConnectionString()].Provider.GraphFileAssociationProvider.Delete(graphFileAssocsOther);
                                DataRepository.Connections[GetConnectionString()].Provider.ObjectAssociationProvider.Delete(otherobAssoc);
                            }

                            DataRepository.Connections[GetConnectionString()].Provider.ObjectAssociationProvider.Delete(obassoc);

                        }
                        catch
                        {
                        }
                    }
               

                    b.ObjectAssociationKey okey1 = new ObjectAssociationKey(obassoc);
                    RemoveFromAssociationList(okey1);
                }
                #endregion
            }
            else
            {
                #region Add Association

                string assocName = mitem.Text.Replace(VersionControlMenuText, "");
                int assocTypeID = (int)Enum.Parse(typeof(AssociationTypeList), assocName);
                ObjectAssociation assoc = new ObjectAssociation();
                assoc.CAid = tag.classAssociationDefinition.CAid;
                assoc.ObjectID = tag.objectPair.ParentObject.pkid;
                assoc.ObjectMachine = tag.objectPair.ParentObject.MachineName;
                assoc.ChildObjectID = tag.objectPair.ChildObject.pkid;
                assoc.ChildObjectMachine = tag.objectPair.ChildObject.MachineName;
                assoc.State = VCStatusList.None;


                // it might have been unchecked because it was MarkedForDelete

                b.ObjectAssociation existingAssoc = DataRepository.Connections[GetConnectionString()].Provider.ObjectAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(assoc.CAid, assoc.ObjectID,
                                                                                      assoc.ChildObjectID,
                                                                                      assoc.ObjectMachine,
                                                                                      assoc.ChildObjectMachine);

                if (existingAssoc != null)
                {
                    // Obsoletes may have some impact, but shouldnt be an issue
                    if (existingAssoc.VCStatusID == (int)VCStatusList.MarkedForDelete)
                    {
                        VCStatusList newStatus = VCStatusList.CheckedOut;
                        UpdateStatus(existingAssoc, newStatus);
                        b.ObjectAssociation otherAssoc = GetTwoWayAssociation(existingAssoc);
                        if (otherAssoc != null)
                        {
                            UpdateStatus(otherAssoc, newStatus);
                        }
                    }
                }
                else
                {

                    try
                    {
                        DataRepository.Connections[GetConnectionString()].Provider.ObjectAssociationProvider.Save(assoc);
                        b.ClassAssociation twoWayClassAssociation = GetTwoWayClassAssociation(assoc);
                        if (twoWayClassAssociation != null)
                        {
                            b.ObjectAssociation twoWayAssocInstance = new b.ObjectAssociation();
                            twoWayAssocInstance.CAid = twoWayClassAssociation.CAid;
                            twoWayAssocInstance.ChildObjectID = assoc.ObjectID;
                            twoWayAssocInstance.ChildObjectMachine = assoc.ObjectMachine;
                            twoWayAssocInstance.ObjectID = assoc.ChildObjectID;
                            twoWayAssocInstance.ObjectMachine = assoc.ChildObjectMachine;
                            DataRepository.ObjectAssociationProvider.Save(twoWayAssocInstance);
                        }
                    }
                    catch
                    {
                    }
                }

                #endregion
            }
            Cell c = tableModel1.Rows[rowIndex].Cells[colIndex + 1];
            c.Checked = IsAssociated(tag.objectPair.ParentObject, tag.objectPair.ChildObject);
            // add to list here
        }
        private void UpdateStatus(ObjectAssociation obassoc, VCStatusList newStatus)
        {
            obassoc.state = newStatus;
            obassoc.VCStatusID = (int)newStatus;
            DataRepository.Connections[GetConnectionString()].Provider.ObjectAssociationProvider.Save(obassoc);
        }
        private b.ClassAssociation GetTwoWayClassAssociation(b.ObjectAssociation objectAssociation)
        {
            // Check if its a twoway association
            b.ClassAssociation classAssoc = DataRepository.Connections[GetConnectionString()].Provider.ClassAssociationProvider.GetByCAid(objectAssociation.CAid);
            if (classAssoc.AssociationTypeID == 4)
            {
                b.TList<b.ClassAssociation> classAssocOtherWayRound = DataRepository.Connections[GetConnectionString()].Provider.ClassAssociationProvider.GetByChildClass(classAssoc.ParentClass);
                classAssocOtherWayRound.Filter = "ParentClass = '" + classAssoc.ChildClass + "' AND AssociationTypeID = 4";
                if (classAssocOtherWayRound.Count > 0)
                {
                    return classAssocOtherWayRound[0];
                }
            }
            return null;
        }
        private b.ObjectAssociation GetTwoWayAssociation(b.ObjectAssociation obassoc)
        {
            b.ClassAssociation twoWayAssociation = GetTwoWayClassAssociation(obassoc);
            if (twoWayAssociation != null)
            {
                ObjectAssociationKey okey = new ObjectAssociationKey();
                okey.CAid = twoWayAssociation.CAid;
                okey.ChildObjectID = obassoc.ObjectID;
                okey.ChildObjectMachine = obassoc.ObjectMachine;
                okey.ObjectID = obassoc.ChildObjectID;
                okey.ObjectMachine = obassoc.ChildObjectMachine;
                ObjectAssociation otherWayAssoc = DataRepository.Connections[GetConnectionString()].Provider.ObjectAssociationProvider.Get(okey);
                return otherWayAssoc;
            }
            return null;
        }
        public void treeLeft_AfterCheck(object sender, DBTreeEventArgs e)
        {
            isAddingList = e.IsAddingBatch;
            if (!IsAddingList)
            {
                TreeNode n = e.Node;
                AddNodeRow(n);
            }
        }
        public void AddRows(TreeNodeCollection nodes)
        {
            foreach (TreeNode n in nodes)
                AddNodeRow(n);
        }
        public void AddColumns(TreeNodeCollection nodes)
        {
            foreach (TreeNode n in nodes)
                AddNodeColumn(n);
        }
        private void AddNodeRow(TreeNode n)
        {
            MetaBase mb = n.Tag as MetaBase;
            if (n.Checked)
                AddRowObject(mb);
            else
                RemoveRowObject(mb);
        }
        public void treeRight_AfterCheck(object sender, DBTreeEventArgs e)
        {
            if (!e.IsAddingBatch)
            {
                TreeNode n = e.Node;
                AddNodeColumn(n);
            }
        }
        private void AddNodeColumn(TreeNode n)
        {
            MetaBase mb = n.Tag as MetaBase;
            if (n.Checked)
                AddColumnObject(mb);
            else
                RemoveColumnObject(mb);
        }
        public void treeContainerLeft_ItemListUpdated(object sender, EventArgs e)
        {
        }
        private void table1_CellMouseUp(object sender, CellMouseEventArgs e)
        {
            Cell c = e.Cell;
            try
            {
                if (e.Cell.Row.Index >= 0 && e.Column > 0)
                {
                    Point ClickPoint = new Point(e.X, e.Y);
                    Point pt = table1.CellRect(c).Location;
                    Point ScreenPoint = table1.PointToScreen(pt);
                    if (e.Button == MouseButtons.Left)
                    {
                        MetaBase mbChild = dtTable.Columns[e.CellPos.Column].ExtendedProperties["Object"] as MetaBase;
                        MetaBase mbParent = rowObjects[e.CellPos.Row];

                        if (mbChild != null)
                        {
                            c.Checked = false;
                            cxMenuMatrix = new ContextMenuStrip();
                            // iterate associationtypes
                            ObjectPairAssociationCollection opac = new ObjectPairAssociationCollection();
                            opac.Load(mbParent, mbChild);
                            TList<ClassAssociation> allowedAssocs =
                                DataRepository.Connections[GetConnectionString()].Provider.ClassAssociationProvider.GetByParentClass(mbParent._ClassName);

                            #region Iterate Allowed Associations and add to menu

                            foreach (ClassAssociation classAssoc in allowedAssocs)
                            {
                                if (classAssoc.IsActive.Value)
                                {
                                    ObjectPairAssociationSpecification opaspec =
                                        new ObjectPairAssociationSpecification();
                                    opaspec.classAssociationDefinition = classAssoc;
                                    opaspec.objectPair = opac;
                                    if (classAssoc.ChildClass == mbChild._ClassName)
                                    {
                                        bool found = false;
                                        string assocName =
                                            ((AssociationTypeList)classAssoc.AssociationTypeID).ToString();
                                        ToolStripMenuItem linkItem = new ToolStripMenuItem(assocName);

                                        foreach (
                                            ObjectPairAssociationCollection.MatrixObjectAssociation moa in
                                                opac.Associations)
                                        {
                                            if (moa.CAid == classAssoc.CAid &&
                                                (!(VCStatusTool.IsObsoleteOrMarkedForDelete(moa))))
                                            {
                                                if (IsReadOnly(moa.VCStatusID))
                                                {
                                                    linkItem.Text += VersionControlMenuText;
                                                }

                                                found = true;
                                                dtTable.Rows[e.CellPos.Row][GetKey(mbChild)] = true;
                                                ArtefactArgs args = new ArtefactArgs();
                                                args.CAid = moa.CAid;
                                                args.ObjectID = moa.ObjectID;
                                                args.ObjectMachine = moa.ObjectMachine;
                                                args.ChildObjectID = moa.ChildObjectID;
                                                args.ChildObjectMachine = moa.ChildObjectMachine;

                                                if (allowedArtefacts.ContainsKey(moa.CAid))
                                                {
                                                    ToolStripMenuItem tmArtefacts = new ToolStripMenuItem("Artefacts");
                                                    tmArtefacts.Click += new EventHandler(tmArtefacts_Click);
                                                    tmArtefacts.Tag = args;
                                                    linkItem.DropDownItems.Add(tmArtefacts);
                                                }
                                            }
                                        }
                                        if (found) c.Checked = true;
                                        linkItem.Tag = opaspec;
                                        linkItem.Checked = found;
                                        linkItem.Enabled = true;
                                        ObjectAssociation oassoc =
                                            DataRepository.Connections[GetConnectionString()].Provider.ObjectAssociationProvider.
                                                GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(
                                                opaspec.classAssociationDefinition.CAid,
                                                opaspec.objectPair.ParentObject.pkid,
                                                opaspec.objectPair.ChildObject.pkid,
                                                opaspec.objectPair.ParentObject.MachineName,
                                                opaspec.objectPair.ChildObject.MachineName);
                                        ToolStripMenuItem tmSender = sender as ToolStripMenuItem;

                                        if (oassoc != null)
                                        {
                                            ArtefactArgs args = new ArtefactArgs();
                                            args.CAid = oassoc.CAid;
                                            args.ObjectID = oassoc.ObjectID;
                                            args.ObjectMachine = oassoc.ObjectMachine;
                                            args.ChildObjectID = oassoc.ChildObjectID;
                                            args.ChildObjectMachine = oassoc.ChildObjectMachine;
                                            OnSelectedAssociationChanged(this, args.CAid, args.ObjectID, args.ChildObjectID, args.ObjectMachine, args.ChildObjectMachine);
                                        }

                                        linkItem.Click += new EventHandler(linkItem_Click);
                                        cxMenuMatrix.Items.Add(linkItem);
                                    }
                                }
                            }

                            #endregion

                            Point offsetLocation = new Point(ClickPoint.X + 50, ClickPoint.Y + 50);
                            cxMenuMatrix.Show(offsetLocation);
                        }
                    }
                }
            }
            catch
            {
            }
        }
        bool IsReadOnly(int VCStatusID)
        {
            return VCStatusID == (int)VCStatusList.Locked || VCStatusID == (int)VCStatusList.CheckedOutRead ||
            VCStatusID == (int)VCStatusList.PartiallyCheckedIn || VCStatusID == (int)VCStatusList.CheckedIn; ;
        }
        private void tmArtefacts_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tmSender = sender as ToolStripMenuItem;
            ArtefactArgs args = tmSender.Tag as ArtefactArgs;
            OnSelectedAssociationChanged(this, args.CAid, args.ObjectID, args.ChildObjectID, args.ObjectMachine, args.ChildObjectMachine);
        }
        private void table1_CellMouseHover(object sender, CellMouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
            try
            {
                #region Show a tooltip with the MetaBase's string representation and class
                Cell c = e.Cell;
                Point ClickPoint = new Point(e.X, e.Y);
                Point pt = table1.CellRect(c).Location;
                MetaBase mbParent = rowObjects[e.CellPos.Row];
                ttip = new ToolTip();
                ttip.Show(mbParent.ToString() + " [" + mbParent._ClassName + "]", this, pt.X, pt.Y, 1000);
                #endregion
            }
            catch
            {
            }
        }

        #region Load & Save Operations

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.InitialDirectory = Variables.Instance.ExportsPath;
            string extension = "mbrvxml";
            openDialog.Filter = "Saved Relationship Manager View (*." + extension + ") | *." + extension;
            openDialog.Multiselect = false;

            DialogResult res = openDialog.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                SerializableRelationshipView srView = new SerializableRelationshipView();
                srView.Open(openDialog.FileName);
                LoadSelection(srView);
            }
        }
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            SerializableRelationshipView sResult = new SerializableRelationshipView();
            sResult.RowObjects = new List<SerializableRelationshipView.ObjectIDentifier>();
            sResult.ColumnObjects = new List<SerializableRelationshipView.ObjectIDentifier>();

            foreach (MetaBase rowObj in this.rowObjects)
            {
                sResult.RowObjects.Add(GetObjectIDentifier(rowObj));
            }
            foreach (MetaBase colObj in this.columnObjects)
            {
                sResult.ColumnObjects.Add(GetObjectIDentifier(colObj));
            }

            SaveFileDialog sfileDialog = new SaveFileDialog();
            sfileDialog.InitialDirectory = Variables.Instance.ExportsPath;
            string extension = "mbrvxml";
            sfileDialog.Filter = "Saved Relationship Manager View (*." + extension + ") | *." + extension;
            DialogResult res = sfileDialog.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                sResult.Save(sfileDialog.FileName);
            }
        }
        private void LoadSelection(SerializableRelationshipView srView)
        {
            foreach (SerializableRelationshipView.ObjectIDentifier colIdentifier in srView.ColumnObjects)
            {
                try
                {
                    MetaBase mbaseCol = Loader.GetByID(colIdentifier.ObjectClass, colIdentifier.Key.pkid, colIdentifier.Key.Machine);
                    AddColumnObject(mbaseCol);
                    OnColumnItemAdded(mbaseCol, EventArgs.Empty);
                }
                catch
                {
                }

            }

            foreach (SerializableRelationshipView.ObjectIDentifier rowIdentifier in srView.RowObjects)
            {
                try
                {
                    MetaBase mbaseRow = Loader.GetByID(rowIdentifier.ObjectClass, rowIdentifier.Key.pkid, rowIdentifier.Key.Machine);
                    AddRowObject(mbaseRow);
                    OnRowItemAdded(mbaseRow, EventArgs.Empty);
                }
                catch { }
            }
        }

        #endregion

        #region Export To Excel

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BaseExcelAssocExporter exporter = new BaseExcelAssocExporter();
            exporter.ColumnObjects = this.columnObjects;
            exporter.RowObjects = this.rowObjects;
            exporter.Associations = this.associations;

            string filename = exporter.ExportMatrix();
            StringBuilder sb = new StringBuilder();
            sb.Append("The file was exported successfullly to:" + Environment.NewLine);
            sb.Append(filename);
            MessageBox.Show(this,sb.ToString(), "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void detailedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DetailedExcelAssocExporter exporter = new DetailedExcelAssocExporter();
                exporter.Artefacts = new Dictionary<ObjectAssociationKey, List<MetaBase>>();
                exporter.ColumnObjects = this.columnObjects;
                exporter.RowObjects = this.rowObjects;
                exporter.Associations = this.associations;
                string filename = exporter.ExportMatrix();


                StringBuilder sb = new StringBuilder();
                sb.Append("The file was exported successfullly to:" + Environment.NewLine);
                sb.Append(filename);
                MessageBox.Show(this,sb.ToString(), "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception x)
            {
                LogEntry logentry = new LogEntry();
                logentry.Title = "Export failed";
                logentry.Message = x.ToString();
                Logger.Write(logentry);
                MessageBox.Show(this,"Export failed - the error has been logged");
            }
        }

        #endregion

        #endregion

        #region Matrix Functionality

        private void InitialiseMatrix()
        {
            dtTable.Rows.Clear();
            columnModel1.Columns.Clear();
            dtTable.Columns.Clear();


            TextColumn tcol = new TextColumn();
            TextCellRenderer ccelrenderer = new TextCellRenderer();
            ccelrenderer.BackColor = Color.Silver;
            ccelrenderer.ForeColor = Color.Black;
            tcol.Renderer = ccelrenderer;
            tcol.Editable = false;
            tcol.Enabled = true;
            tcol.Text = "Object";
            tcol.Width = 150;
            tcol.Sortable = false;
            columnModel1.Columns.Add(tcol);

            dtTable.Columns.Add(new DataColumn("Object", typeof(string)));
            dtTable.AcceptChanges();
        }

        #endregion
    }
}