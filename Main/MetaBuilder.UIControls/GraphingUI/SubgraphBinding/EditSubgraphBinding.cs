using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Shapes.Nodes;
using MetaBuilder.Meta;
using MetaBuilder.UIControls.Dialogs;
using XPTable.Editors;
using XPTable.Events;
using XPTable.Models;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
using MetaBuilder.Graphing.Shapes.Nodes.Containers;
using ObjectAssociation = MetaBuilder.BusinessLogic.ObjectAssociation;

namespace MetaBuilder.UIControls.GraphingUI.SubgraphBinding
{
    public partial class EditSubgraphBinding : Form
    {
        #region Fields (5)

        //private b.TList<b.AssociationType> associationTypes;
        private b.TList<b.ClassAssociation> classAssociations;
        private int dropDownColumnIndex = 1;
        private ILinkedContainer ilContainer;
        private NormalDiagram document;

        #endregion Fields

        #region Constructors (1)

        public EditSubgraphBinding()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties (2)

        public NormalDiagram Document
        {
            get { return document; }
            set { document = value; }
        }
        public ILinkedContainer ILContainer
        {
            get { return ilContainer; }
            set
            {
                ilContainer = value;
                if (ilContainer is SubgraphNode)
                {
                    this.Text = "Edit Subgraph Associations";
                }
                else if (ilContainer is ValueChain)
                {
                    this.Text = "Edit Valuechain Associations";
                }
                else if (ilContainer is MappingCell)
                {
                    this.Text = "Edit Swimlane Associations";
                }
            }
        }

        #endregion Properties

        #region Methods (25)

        // Public Methods (2) 
        bool addedRemoveColumn = false;
        public void BindData()
        {
            //associationTypes = d.DataRepository.AssociationTypeProvider.GetAll();
            if (ILContainer != null)
            {
                foreach (KeyValuePair<string, b.ClassAssociation> kvp in ilContainer.DefaultClassBindings)
                {
                    SubgraphNode.DefaultClassBinding dcb = new SubgraphNode.DefaultClassBinding();
                    dcb.ClassName = kvp.Key;
                    dcb.MyAssociation = kvp.Value;
                    XPTable.Models.Row r = new XPTable.Models.Row();
                    r.Cells.Add(new XPTable.Models.Cell(kvp.Value.ChildClass));
                    r.Cells.Add(new XPTable.Models.Cell(GetAssociationTypeName(kvp.Value.AssociationTypeID)));
                    ClassAssociationItem cai = new ClassAssociationItem(kvp.Value);
                    r.Cells[1].Tag = cai;
                    r.Cells.Add(new XPTable.Models.Cell(kvp.Value.Caption));

                    r.Tag = dcb;
                    this.tableModel1.Rows.Add(r);
                }

                if (ILContainer is MappingCell && !addedRemoveColumn)
                {
                    addedRemoveColumn = true;
                    columnModelObjects.Columns.Add(buttonColumnRemove);
                }
                foreach (EmbeddedRelationship relship in ilContainer.ObjectRelationships)
                {
                    if (relship.MyAssociation != null)
                    {
                        XPTable.Models.Row r = new Row();
                        r.Cells.Add(new XPTable.Models.Cell(relship.MyMetaObject.ToString()));
                        r.Cells.Add(new XPTable.Models.Cell(relship.MyMetaObject._ClassName));
                        r.Cells.Add(new XPTable.Models.Cell(GetAssociationTypeName(relship.MyAssociation.AssociationTypeID)));
                        r.Cells[2].Tag = relship.MyAssociation;
                        r.Cells.Add(new XPTable.Models.Cell(relship.MyAssociation.Caption));
                        b.ObjectAssociation oa = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(relship.MyAssociation.CAid, this.ILContainer.MetaObject.pkid, relship.MyMetaObject.pkid, ilContainer.MetaObject.MachineName, relship.MyMetaObject.MachineName);

                        if (oa != null)
                        {
                            r.Cells.Add(new XPTable.Models.Cell(((VCStatusList)oa.VCStatusID).ToString()));
                            if (ilContainer is MappingCell)
                            {
                                if (((VCStatusList)oa.VCStatusID) == VCStatusList.MarkedForDelete)
                                {
                                    //r.Cells.Add(new XPTable.Models.Cell("Restore"));
                                }
                                else
                                {
                                    r.Cells.Add(new XPTable.Models.Cell("Remove"));
                                }
                            }


                        }
                        else
                        {
                            r.Cells.Add(new XPTable.Models.Cell("Not in DB"));
                        }
                        r.Tag = relship;
                        if (relship.FromDatabase)
                        {
                            r.ForeColor = Color.Gray;
                        }
                        this.tableModelObjects.Rows.Add(r);
                    }
                }
            }
        }

        public void ShouldSubgraphNodeMFD(EmbeddedRelationship relToRemove)
        {
            b.ClassAssociation selectedAssociation = relToRemove.MyAssociation;
            ObjectAssociationKey oakey = new ObjectAssociationKey();
            oakey.CAid = selectedAssociation.CAid;
            oakey.ObjectID = ILContainer.MetaObject.pkid;
            oakey.ObjectMachine = ILContainer.MetaObject.MachineName;
            oakey.ChildObjectID = relToRemove.MyMetaObject.pkid;
            oakey.ChildObjectMachine = relToRemove.MyMetaObject.MachineName;

            b.ObjectAssociation oa = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Get(oakey);

            if (VCStatusTool.UserHasControl(oa) && VCStatusTool.DeletableFromDiagram(oa))
            {
                //RemoveObjectByRelationship(relToRemove);
                DialogResult res = MessageBox.Show(this, "Do you want to mark the association as deleted? ", "Remove or Mark For Delete", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    oa.VCStatusID = (int)VCStatusList.MarkedForDelete;
                    oa.State = VCStatusList.MarkedForDelete;
                    MetaBuilder.DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Save(oa);
                }
            }
        }

        // Private Methods (23) 

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Save the form data to the SGN
            foreach (Row r in tableModel1.Rows)
            {
                SubgraphNode.DefaultClassBinding dfb = r.Tag as SubgraphNode.DefaultClassBinding;
                ClassAssociationItem cai = r.Cells[1].Tag as ClassAssociationItem;
                ILContainer.DefaultClassBindings[dfb.ClassName] = cai.ClassAssociation;//.MyAssociation;

                //b.ClassAssociation > kvp = r.Tag as KeyValuePair<string, b.ClassAssociation>;
                //kvp.Value = r.Cells[1].Tag as b.ClassAssociation;
                //// Console.WriteLine(r.Cells[1].Tag);
            }

            foreach (Row r in tableModelObjects.Rows)
            {
                EmbeddedRelationship embRel = r.Tag as EmbeddedRelationship;

                embRel.MyAssociation = r.Cells[2].Tag as b.ClassAssociation;
            }

            if (DockingForm.DockForm.GetCurrentGraphViewContainer() != null)
                DockingForm.DockForm.GetCurrentGraphViewContainer().CustomModified = true;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void EditSubgraphBinding_Load(object sender, EventArgs e)
        {
            classAssociations = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetAll();
        }

        private string GetAssociationTypeName(int AssociationTypeID)
        {
            foreach (AssociationTypeList a in (AssociationTypeList[])Enum.GetValues(typeof(AssociationTypeList)))
            {
                if ((int)a == AssociationTypeID)
                    return a.ToString();
            }

            //foreach (b.AssociationType assocType in associationTypes)
            //{
            //    if (assocType.pkid == AssociationTypeID)
            //        return assocType.Name;
            //}
            return "";
        }

        private void ModifyEditor(Row r, b.ClassAssociation assoc)
        {
            // AddCustomEditor(assoc.ChildClass);

            // only do this once for reach row, otherwise table loses user's choice
            if (r.Cells[dropDownColumnIndex].Checked == false)
            {
                r.Cells[dropDownColumnIndex].Checked = true;
                // tableModel1.Rows[pos.Row].Cells[dropDownColumnIndex].Text = kvp.Value.ChildClass;
            }
        }

        private void newEditor_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxCellEditor editor = sender as ComboBoxCellEditor;
            editor.EditingCell.Tag = editor.SelectedItem as ClassAssociationItem;
        }

        private void newEditor2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // save the cell for use after the dialog
            ComboBoxCellEditor editor = sender as ComboBoxCellEditor;
            EmbeddedRelationship rel = editor.EditingCell.Row.Tag as EmbeddedRelationship;
            editor.EditingCell.Tag = (editor.SelectedItem as ClassAssociationItem).ClassAssociation;
            ClassAssociation previousAssociation = rel.MyAssociation;
            rel.MyAssociation = (editor.SelectedItem as ClassAssociationItem).ClassAssociation;

            try
            {
                b.ObjectAssociationKey key = new ObjectAssociationKey();
                key.ObjectID = ILContainer.MetaObject.pkid;
                key.ObjectMachine = ILContainer.MetaObject.MachineName;
                key.ChildObjectID = rel.MyMetaObject.pkid;
                key.ChildObjectMachine = rel.MyMetaObject.MachineName;
                key.CAid = previousAssociation.CAid;

                b.ObjectAssociation oa = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Get(key);
                if (oa != null)
                {
                    if (VCStatusTool.UserHasControl(oa))
                    {
                        DialogResult res = DialogResult.Yes;
                        // Hardcoded this because of time constraints. It will mark the previous association as MFD if the user has control.
                        /*MessageBox.Show(this,"Delete the previous association?", "Delete Association",
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);*/

                        if (res == DialogResult.Yes)
                        {
                            b.TList<b.GraphFileAssociation> gfas = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(oa.CAid, oa.ObjectID, oa.ChildObjectID, oa.ObjectMachine, oa.ChildObjectMachine);
                            d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileAssociationProvider.Delete(gfas);
                            oa.VCStatusID = (int)VCStatusList.MarkedForDelete;
                            d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Save(oa);
                        }
                    }
                }

                if (Core.Variables.Instance.SaveOnCreate)
                    if (MetaBuilder.Graphing.Shapes.LinkController.GetAssociation(ILContainer.MetaObject, rel.MyMetaObject, (LinkAssociationType)rel.MyAssociation.AssociationTypeID) == null)
                        MetaBuilder.Graphing.Shapes.LinkController.SaveAssociation(ILContainer.MetaObject, rel.MyMetaObject, (LinkAssociationType)rel.MyAssociation.AssociationTypeID, Core.Variables.Instance.ClientProvider);
                //rel.ToString();
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog(ex.ToString());
            }
        }

        private void objectTable_BeforePaintCell(object sender, PaintCellEventArgs e)
        {
            //UpdateDropdown(e.Cell);
        }

        private void objectTable_BeginEditing(object sender, XPTable.Events.CellEditEventArgs e)
        {
            if (e.CellPos.Column == dropDownColumnIndex)
            {
                //  UpdateDropdown(e.Cell);
            }
        }

        private void objectTable_CellClick(object sender, XPTable.Events.CellMouseEventArgs e)
        {
            if (e.CellPos.Column == dropDownColumnIndex)
            {
                UpdateDropdown(e.Cell);
                //           ModifyDropdown(e.Cell, e.CellPos);
            }
        }

        private void objectTable_CellGotFocus(object sender, CellFocusEventArgs e)
        {
            UpdateDropdown(e.Cell);
        }

        private void objectTable_CellMouseDown(object sender, CellMouseEventArgs e)
        {
            UpdateDropdown(e.Cell);
        }

        private void objectTable_CellMouseEnter(object sender, CellMouseEventArgs e)
        {
            UpdateDropdown(e.Cell);
        }

        private void RemoveObjectByRelationship(EmbeddedRelationship relToRemove)
        {
            List<EmbeddedRelationship> itemsToRemove = new List<EmbeddedRelationship>();
            if (ILContainer != null)
            {
                foreach (EmbeddedRelationship relship in ilContainer.ObjectRelationships)
                {
                    if (relship == relToRemove)
                    {
                        itemsToRemove.Add(relship);
                    }
                }
            }

            for (int i = 0; i < itemsToRemove.Count; i++)
            {
                ilContainer.ObjectRelationships.Remove(itemsToRemove[i]);
                ilContainer.RemoveByRelationship(itemsToRemove[i]);
            }
        }

        private void tableSelection_BeforePaintCell(object sender, PaintCellEventArgs e)
        {
            // UpdateDropdownObjects(e.Cell);
        }

        private void tableSelection_BeginEditing(object sender, CellEditEventArgs e)
        {
            //      UpdateDropdownObjects(e.Cell);
        }

        private void tableSelection_CellButtonClicked(object sender, CellButtonEventArgs e)
        {
            if (e.Cell.Text == "Remove")
            {
                EmbeddedRelationship relToRemove = e.Cell.Row.Tag as EmbeddedRelationship;
                ShouldSubgraphNodeMFD(relToRemove);
                this.tableModel1.Rows.Clear();
                if (!(ILContainer is MappingCell))
                    ILContainer.ObjectRelationships.Remove(relToRemove);
                this.tableModelObjects.Rows.Clear();
                BindData();
            }
            else if (e.Cell.Text == "Restore")
            {
                //Restore the association?
            }
        }

        private void tableSelection_CellClick(object sender, CellMouseEventArgs e)
        {
            //    UpdateDropdownObjects(e.Cell);
        }

        private void tableSelection_CellGotFocus(object sender, CellFocusEventArgs e)
        {
            UpdateDropdownObjects(e.Cell);
        }

        private void tableSelection_CellMouseDown(object sender, CellMouseEventArgs e)
        {
            //      UpdateDropdownObjects(e.Cell);
        }

        private void tableSelection_CellMouseEnter(object sender, CellMouseEventArgs e)
        {
            // UpdateDropdownObjects(e.Cell);
        }

        private void UpdateDropdown(Cell c)
        {
            SubgraphNode.DefaultClassBinding dcb = c.Row.Tag as SubgraphNode.DefaultClassBinding;

            classAssociations.Filter = "ParentClass = '" + ILContainer.MetaObject._ClassName + "' and ChildClass = '" + dcb.MyAssociation.ChildClass + "'";
            ComboBoxCellEditor newEditor = new ComboBoxCellEditor();
            newEditor.Items.Clear();

            List<ClassAssociationItem> assocsAsStringArray = new List<ClassAssociationItem>();
            List<string> sss = new List<string>();
            if (classAssociations.Count > 0)
            {
                foreach (b.ClassAssociation allowedAssoc in classAssociations)
                {
                    assocsAsStringArray.Add(new ClassAssociationItem(allowedAssoc));
                    sss.Add(allowedAssoc.ChildClass);
                }
            }

            newEditor.Items.AddRange(assocsAsStringArray.ToArray());
            columnModel1.Columns[1].Editor = newEditor;
            newEditor.SelectedIndexChanged += new EventHandler(newEditor_SelectedIndexChanged);
        }

        private void UpdateDropdownObjects(Cell c)
        {
            EmbeddedRelationship relship = c.Row.Tag as EmbeddedRelationship;
            b.ClassAssociation selectedAssociation = relship.MyAssociation;

            classAssociations.Filter = "ParentClass = '" + ILContainer.MetaObject._ClassName + "' and ChildClass = '" + selectedAssociation.ChildClass + "'";
            ComboBoxCellEditor newEditor = new ComboBoxCellEditor();
            newEditor.Items.Clear();

            List<ClassAssociationItem> assocsAsStringArray = new List<ClassAssociationItem>();
            List<string> sss = new List<string>();
            if (classAssociations.Count > 0)
            {
                foreach (b.ClassAssociation allowedAssoc in classAssociations)
                {
                    assocsAsStringArray.Add(new ClassAssociationItem(allowedAssoc));
                    sss.Add(allowedAssoc.ChildClass);

                }
            }

            newEditor.Items.AddRange(assocsAsStringArray.ToArray());
            columnModelObjects.Columns[2].Editor = newEditor;
            newEditor.SelectedIndexChanged += new EventHandler(newEditor2_SelectedIndexChanged);
        }

        protected override void OnMove(EventArgs e)
        {
            objectTable.StopEditing();
            tableSelection.StopEditing();
            base.OnMove(e);
        }

        #endregion Methods

        #region Nested Classes (1)

        private class ClassAssociationItem
        {

            #region Fields (1)

            private b.ClassAssociation classAssoc;

            #endregion Fields

            #region Constructors (1)

            public ClassAssociationItem(b.ClassAssociation assoc)
            {
                this.classAssoc = assoc;
            }

            #endregion Constructors

            #region Properties (1)

            public b.ClassAssociation ClassAssociation
            {
                get { return classAssoc; }
                set { classAssoc = value; }
            }

            #endregion Properties

            #region Methods (1)


            // Public Methods (1) 

            public override string ToString()
            {
                return ((b.AssociationTypeList)classAssoc.AssociationTypeID).ToString();
            }

            #endregion Methods

        }

        #endregion Nested Classes

    }

    /*
     * 
     * using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;

namespace WindowsApplication73 {
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class Form1 : System.Windows.Forms.Form {

		#region Fields (3) 

        private System.Windows.Forms.ComboBox comboBox1;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private const int separatorHeight = 3, verticalItemPadding = 4;

		#endregion Fields 

		#region Constructors (1) 

        public Form1() {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            comboBox1.Items.Add("Apple");
            comboBox1.Items.Add(new SeparatorItem("orange"));
            comboBox1.Items.Add("Dog");
            comboBox1.Items.Add(new SeparatorItem("Cat"));
            comboBox1.Items.Add("Boy");
            comboBox1.Items.Add("Girl");            

        }

		#endregion Constructors 

		#region Methods (4) 

		// Protected Methods (1) 

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

		// Private Methods (3) 

        private void comboBox1_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e) {
            if (e.Index == -1) {
                return;
            }
            else {
                object comboBoxItem = comboBox1.Items[e.Index];


                e.DrawBackground();
                e.DrawFocusRectangle();

                bool isSeparatorItem = (comboBoxItem is SeparatorItem);

                // draw the text
                using (Brush textBrush = new SolidBrush(e.ForeColor)) {
                    Rectangle bounds = e.Bounds;
                    // adjust the bounds so that the text is centered properly.

                    // if we're a separator, remove the separator height
                    if (isSeparatorItem && (e.State & DrawItemState.ComboBoxEdit) != DrawItemState.ComboBoxEdit) {
                        bounds.Height -= separatorHeight;
                    }

                    // Draw the string vertically centered but on the left
                    using (StringFormat format = new StringFormat()) {
                        format.LineAlignment = StringAlignment.Center;
                        format.Alignment = StringAlignment.Near;
                        // in Whidbey consider using TextRenderer.DrawText instead
                        e.Graphics.DrawString(comboBoxItem.ToString(), comboBox1.Font, textBrush, bounds, format);
                    }
                }

                // draw the separator line
                if (isSeparatorItem && ((e.State & DrawItemState.ComboBoxEdit) != DrawItemState.ComboBoxEdit)) {
                    Rectangle separatorRect = new Rectangle(e.Bounds.Left, e.Bounds.Bottom - separatorHeight, e.Bounds.Width, separatorHeight);

                    // fill the background behind the separator
                    using (Brush b = new SolidBrush(comboBox1.BackColor)) {
                        e.Graphics.FillRectangle(b, separatorRect);
                    }
                    e.Graphics.DrawLine(SystemPens.ControlText, separatorRect.Left + 2, separatorRect.Top + 1,
                        separatorRect.Right - 2, separatorRect.Top + 1);

                }
            }

        }

        private void comboBox1_MeasureItem(object sender, System.Windows.Forms.MeasureItemEventArgs e) {
            if (e.Index == -1) {
                return;
            }
            else {
                object comboBoxItem = comboBox1.Items[e.Index];

                // in Whidbey consider using TextRenderer.MeasureText instead
                Size textSize = e.Graphics.MeasureString(comboBoxItem.ToString(), comboBox1.Font).ToSize();
                e.ItemHeight = textSize.Height + verticalItemPadding;
                e.ItemWidth = textSize.Width;

                if (comboBoxItem is SeparatorItem) {
                    // one white line, one dark, one white.
                    e.ItemHeight += separatorHeight;
                }
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.Run(new Form1());
        }


		#endregion Methods 

		#region Nested Classes (1) 


        public class SeparatorItem {

		#region Fields (1) 

            private object data;

		#endregion Fields 

		#region Constructors (1) 

            public SeparatorItem(object data) {
                this.data = data;
            }

		#endregion Constructors 

		#region Methods (1) 


		// Public Methods (1) 

            public override string ToString() {
                if (data != null) {
                    return data.ToString();
                }

                return base.ToString();
            }


		#endregion Methods 


        }
		#endregion Nested Classes 


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            //
            // comboBox1
            //
            this.comboBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBox1.Location = new System.Drawing.Point(64, 80);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.Text = "comboBox1";
            this.comboBox1.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.comboBox1_MeasureItem);
            this.comboBox1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBox1_DrawItem);
            //
            // Form1
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.comboBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }
        #endregion
    }
}*/

}