using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Meta;
using Northwoods.Go;
using XPTable.Editors;
using XPTable.Events;
using XPTable.Models;
using XPTable.Renderers;
using ObjectAssociation=MetaBuilder.BusinessLogic.ObjectAssociation;

namespace MetaBuilder.UIControls.Dialogs
{
    public partial class RemoveOrMarkForDelete : Form
    {

		#region Fields (4) 

        private ComboBoxCellEditor actionEditor;
        int dropDownColumnIndex = 3;
        private GraphFileKey graphFileKey;
        private List<RepositoryItemAction> itemActions;

		#endregion Fields 

		#region Constructors (1) 

        public RemoveOrMarkForDelete()
        {
            InitializeComponent();
            ItemActions = new List<RepositoryItemAction>();
            this.comboDefault.SelectedIndex = 0;
        }

		#endregion Constructors 

		#region Properties (2) 

        public GraphFileKey GraphFileKey
        {
            get { return graphFileKey; }
            set { graphFileKey = value; }
        }

        public List<RepositoryItemAction> ItemActions
        {
            get { return itemActions; }
            set { itemActions = value; }
        }

		#endregion Properties 

		#region Methods (12) 

		// Private Methods (12) 

        private void actionEditor_BeginEdit(object sender, CellEditEventArgs e)
        {
            if (e.CellPos.Column == dropDownColumnIndex)
            {
                actionEditor.Items.Clear();
                RepositoryItemAction act = tableModel1.Rows[e.CellPos.Row].Tag as RepositoryItemAction;
                if (VCStatusTool.UserHasControl(act.Item) && VCStatusTool.DeletableFromDiagram(act.Item))
                {
                    actionEditor.Items.AddRange(new string[] {"Cancel", "Remove", "Delete"});
                }
                else
                {
                    actionEditor.Items.AddRange(new string[] {"Cancel", "Remove"});
                }
            }
        }

        private void AddCustomEditor(bool IncludeDelete)
        {
            comboChoice.Editable = true;
            actionEditor.Items.Clear();
            if (IncludeDelete)
                actionEditor.Items.AddRange(new string[] {"Cancel", "Remove", "Delete"});
            else
                actionEditor.Items.AddRange(new string[] {"Cancel", "Remove"});
            actionEditor.DropDownStyle = DropDownStyle.DropDownList;
            comboChoice.Selectable = false;
            comboChoice.Editor = actionEditor;
        }

        private void BindData()
        {
            bool checkExistence = Core.Variables.Instance.CheckExistenceOnOtherDiagramsOnDelete;
            table1.ColumnModel.Columns[2].Enabled = checkExistence;
            AssociationHelper asshelper = new AssociationHelper();
            List<ObjectAssociation> orphanAssocs = asshelper.GetOrphanedAssociations(graphFileKey);
          
            if (checkExistence)
                dropDownColumnIndex = 3;

            ObjectHelper ohelper = new ObjectHelper();
            List<MetaObject> orphanObjects = new List<MetaObject>();
            if (checkExistence)
                orphanObjects = ohelper.GetOrphanedObjects(graphFileKey);

            foreach (RepositoryItemAction action in ItemActions)
            {
                action.ActionToBeTaken = ActionToBeTaken.Remove;
                if (action.Item is MetaBase)
                {
                    MetaBase mbase = action.Item as MetaBase;
                    Row r = new Row();
                    r.Cells.Add(new Cell());
                    r.Cells.Add(new Cell());
                    r.Cells.Add(new Cell());
                        r.Cells.Add(new Cell());
                    if (mbase.ToString() != null)
                    {
                        r.Cells[0].Text = mbase.ToString();
                    }
                    else
                    {
                        r.Cells[0].Text = "[null]";
                    }

                    if (checkExistence)
                    {
                        bool found = true;
                        foreach (MetaObject mo in orphanObjects)
                        {
                            if (mo.pkid == mbase.pkid && mo.Machine == mbase.MachineName)
                            {
                                found = false;
                            }
                        }
                        r.Cells[2].Checked = found; // = (found) ? "No" : "Yes";
                    }
                    r.Cells[1].Text = mbase._ClassName;
             
                    r.Cells[2].Editable = false;
                    r.Cells[dropDownColumnIndex].Text = action.ActionToBeTaken.ToString();
                    r.Tag = action;
                    r.Cells[dropDownColumnIndex].Editable = true;
                    r.Cells[dropDownColumnIndex].Enabled = true;
                    r.Cells[dropDownColumnIndex].Data = action.ActionToBeTaken.ToString();
                    tableModel1.Rows.Add(r);
                }
                if (action.Item is ObjectAssociation)
                {
                    ObjectAssociation oassoc = action.Item as ObjectAssociation;
                    if (oassoc.ObjectID > 0 && oassoc.ChildObjectID > 0)
                    {
                        Row rAssoc = new Row();
                        rAssoc.Cells.Add(new Cell());
                        rAssoc.Cells.Add(new Cell());
                        rAssoc.Cells.Add(new Cell());
                        rAssoc.Cells.Add(new Cell());
                        QLink slink = action.MyGoObject as QLink;
                        if (slink.FromNode is IMetaNode && slink.ToNode is IMetaNode)
                        {
                            rAssoc.Cells[0].Text = slink.AssociationType.ToString() + " from " +
                                                   ((IMetaNode) slink.FromNode).MetaObject.ToString() + " to " +
                                                   ((IMetaNode) slink.ToNode).MetaObject.ToString();
                        }
                        else
                        {
                            rAssoc.Cells[0].Text = slink.AssociationType.ToString();
                        }

                        bool found = true;
                        foreach (ObjectAssociation oa in orphanAssocs)
                        {
                            if (oa.CAid == oassoc.CAid && oa.ObjectID == oassoc.ObjectID &&
                                oa.ChildObjectID == oassoc.ChildObjectID && oa.ObjectMachine == oassoc.ObjectMachine &&
                                oa.ChildObjectMachine == oassoc.ChildObjectMachine)
                            {
                                found = false;
                            }
                        }

                        rAssoc.Cells[1].Text = "Link";
                        rAssoc.Cells[2].Checked = found; // ? "No" : "Yes";
                        rAssoc.Cells[dropDownColumnIndex].Text = action.ActionToBeTaken.ToString();
                        rAssoc.Tag = action;
                        rAssoc.Cells[dropDownColumnIndex].Editable = true;

                        rAssoc.Cells[dropDownColumnIndex].Enabled = true;
                        rAssoc.Cells[dropDownColumnIndex].Data = action.ActionToBeTaken.ToString();
                        tableModel1.Rows.Add(rAssoc);
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            foreach (Row r in tableModel1.Rows)
            {
                RepositoryItemAction actionItem = r.Tag as RepositoryItemAction;
                actionItem.ActionToBeTaken = (ActionToBeTaken)Enum.Parse(typeof(ActionToBeTaken), r.Cells[dropDownColumnIndex].Text);
               
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnSetAsDefault_Click(object sender, EventArgs e)
        {
            if (comboDefault.Text.Length>0)
            foreach (Row r in tableModel1.Rows)
            {
                if (comboDefault.Text == "Delete")
                {
                    RepositoryItemAction act = r.Tag as RepositoryItemAction;
                    if (VCStatusTool.UserHasControl(act.Item) && VCStatusTool.DeletableFromDiagram(act.Item))
                    {
                        r.Cells[dropDownColumnIndex].Text = "Delete";
                       
                    }
                    else
                    {
                        r.Cells[dropDownColumnIndex].Text = "Remove";
                    }
                }
                else
                {
                    r.Cells[dropDownColumnIndex].Text = comboDefault.Text;
                }
            }
        }

        private void ModifyDropdown(Cell c, CellPos pos)
        {
            try
            {
                RepositoryItemAction act = tableModel1.Rows[pos.Row].Tag as RepositoryItemAction;
                if (VCStatusTool.UserHasControl(act.Item) && VCStatusTool.DeletableFromDiagram(act.Item))
                {
                    AddCustomEditor(true);
                }
                else
                {
                    AddCustomEditor(false);
                }
                // only do this once for reach row, otherwise table loses user's choice
                if (tableModel1.Rows[pos.Row].Cells[dropDownColumnIndex].Checked == false)
                {
                    tableModel1.Rows[pos.Row].Cells[dropDownColumnIndex].Checked = true;
                    tableModel1.Rows[pos.Row].Cells[dropDownColumnIndex].Text = act.ActionToBeTaken.ToString();
                }
            }
            catch
            {
            }
        }

        private void RemoveOrMarkForDelete_Load(object sender, EventArgs e)
        {
            table1.HeaderRenderer = new GradientHeaderRenderer();
            table1.ColumnModel.Columns[2].Visible = Core.Variables.Instance.CheckExistenceOnOtherDiagramsOnDelete;
            actionEditor = new ComboBoxCellEditor();
            AddCustomEditor(true);
            BindData();
        }

        private void table1_BeforePaintCell(object sender, PaintCellEventArgs e)
        {
            ModifyDropdown(e.Cell, e.CellPos);
        }

        private void table1_BeginEditing(object sender, CellEditEventArgs e)
        {
            if (e.CellPos.Column == dropDownColumnIndex)
            {
                ModifyDropdown(e.Cell, e.CellPos);
            }
        }

        private void table1_CellClick(object sender, CellMouseEventArgs e)
        {
            if (e.CellPos.Column == dropDownColumnIndex)
            {
                ModifyDropdown(e.Cell, e.CellPos);
            }
        }

        private void table1_CellMouseDown(object sender, CellMouseEventArgs e)
        {
            ModifyDropdown(e.Cell, e.CellPos);
        }

		#endregion Methods 

        #region ActionToBeTaken enum

        public enum ActionToBeTaken
        {
            Cancel,
            Remove,
            Delete
        }

        #endregion

        #region Nested type: RepositoryItemAction

        public class RepositoryItemAction
        {
            private ActionToBeTaken actionToBeTaken;
            private IRepositoryItem item;
            private GoObject myGoObject;

            public GoObject MyGoObject
            {
                get { return myGoObject; }
                set { myGoObject = value; }
            }

            public IRepositoryItem Item
            {
                get { return item; }
                set { item = value; }
            }

            public ActionToBeTaken ActionToBeTaken
            {
                get { return actionToBeTaken; }
                set { actionToBeTaken = value; }
            }
        }

        #endregion
    }
}