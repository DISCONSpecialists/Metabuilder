using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Meta;
using Northwoods.Go;

namespace MetaBuilder.UIControls.GraphingUI
{
    public partial class ShapeBinding : Form
    {

        #region Fields (4)

        private List<Association> allowedAssociations;
        private BindingInfo bindingInfo;
        private DataTable dtLabels;
        private IMetaBase objTester;

        #endregion Fields

        #region Constructors (1)

        public ShapeBinding()
        {
            InitializeComponent();
            dgBindings.DataError += new DataGridViewDataErrorEventHandler(dgBindings_DataError);
        }

        #endregion Constructors

        #region Properties (2)

        public BindingInfo BindingInfo
        {
            get { return bindingInfo; }
            set { bindingInfo = value; }
        }

        private List<string> ClassProperties
        {
            get
            {
                List<string> retval = new List<string>();
                string classname = null;
                if (classDropdown1.SelectedClass != null)
                {
                    classname = classDropdown1.SelectedClass;
                }
                if (bindingInfo != null)
                {
                    classname = bindingInfo.BindingClass;
                }
                if (classname.Length > 0)
                {
                    try
                    {
                        IMetaBase obj = Loader.CreateInstance(classname);
                        PropertyInfo[] properties = obj.GetType().GetProperties();
                        foreach (PropertyInfo propInfo in properties)
                        {
                            if (propInfo.Name != "_ClassName")
                                retval.Add(propInfo.Name);
                        }
                    }
                    catch
                    {
                    }
                    return retval;
                }
                return new List<string>();
            }
        }

        #endregion Properties

        #region Methods (5)


        // Public Methods (1) 

        public DialogResult ShowForm()
        {
            classDropdown1.Init();
            if (bindingInfo == null)
            {
                bindingInfo = new BindingInfo();
                classDropdown1.SelectedIndex = 0;
            }
            UpdateInterface();
            SetupForm();
            return ShowDialog();
        }



        // Protected Methods (1) 

        protected override void OnClosing(CancelEventArgs evt)
        {
            base.OnClosing(evt);
            CheckForDirty();
        }



        // Private Methods (3) 

        private void CheckForDirty()
        {
            if (bindingInfo != null)
            {
                bindingInfo.BindingClass = classDropdown1.SelectedClass;
                DialogResult =
                    MessageBox.Show(this, "Do you wish to apply the binding settings?", "Apply Settings",
                                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            }
        }

        private void dgBindings_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        private void SetupForm()
        {
            classDropdown1.SelectedClass = bindingInfo.BindingClass;
            foreach (KeyValuePair<string, string> entry in bindingInfo.Bindings)
            {
                string labelname = entry.Key;
                string propertyname = entry.Value;
                foreach (DataGridViewRow r in dgBindings.Rows)
                {
                    if (r.Cells[0].EditedFormattedValue.ToString() == labelname)
                    {
                        r.Cells[1].Value = propertyname;
                    }
                }
            }
        }


        #endregion Methods


        #region Helper methods & properties
        private List<BoundLabel> BoundLabels
        {
            get
            {
                return
                    DockingForm.DockForm.GetCurrentGraphView().ViewController.GetNestedBoundLabels(
                        DockingForm.DockForm.GetCurrentGraphView().Doc as IGoCollection);
            }
        }

        private List<RepeaterSection> RepeaterSections
        {
            get
            {
                List<RepeaterSection> retval = new List<RepeaterSection>();
                foreach (GoObject obj in DockingForm.DockForm.GetCurrentGraphView().Document)
                {
                    if (obj is RepeaterSection)
                    {
                        RepeaterSection repeaterObject = obj as RepeaterSection;
                        retval.Add(repeaterObject);
                    }
                    if (obj is GraphNode)
                    {
                        foreach (GoObject objChild in (obj as GraphNode))
                        {
                            if (objChild is RepeaterSection)
                            {
                                RepeaterSection repeaterChildObject = objChild as RepeaterSection;
                                retval.Add(repeaterChildObject);
                            }
                        }
                    }
                }
                return retval;
            }
        }

        private GraphView graphViewPreview
        {
            get { return DockingForm.DockForm.GetCurrentGraphView(); }
        }
        #endregion

        #region Other Methods (main body)
        private void LoadAssociatedClassesIntoList()
        {
            try
            {
                DataView dvAssociatedClasses =
                    Singletons.GetClassHelper().GetAllowedAssociateClasses(bindingInfo.BindingClass, true);
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("Display", typeof(string)));
                dt.Columns.Add(new DataColumn("Value", typeof(int)));
                allowedAssociations = new List<Association>();
                Association assNull = new Association();
                assNull.ChildClass = "None";
                assNull.AssociationType = " - none - ";
                assNull.ID = 0;
                assNull.AssociationType = "none";
                assNull.Caption = " - none - ";
                allowedAssociations.Add(assNull);
                foreach (DataRowView drvAssociatedClass in dvAssociatedClasses)
                {
                    Association ass = new Association();
                    ass.AssociationType = drvAssociatedClass["AssociationType"].ToString();
                    ass.AssociationTypeID = int.Parse(drvAssociatedClass["AssociationTypeID"].ToString());
                    ass.ChildClass = drvAssociatedClass["ChildClass"].ToString();
                    ass.ID = int.Parse(drvAssociatedClass["CAid"].ToString());
                    ass.Caption = drvAssociatedClass["Caption"].ToString();
                    ass.ParentClass = bindingInfo.BindingClass;
                    allowedAssociations.Add(ass);
                }
            }
            catch
            {
                allowedAssociations = new List<Association>();
            }
        }

        private void UpdateInterface()
        {
            LoadAssociatedClassesIntoList();
            ResetDataGrids();
            BindLabelGrid();
            BindRepeaterGrid();
        }

        private void BindRepeaterGrid()
        {
            EnableOrDisableRepeaterGridAccordingToRepeaterCount();
            CreateRepeaterDataTable();
            if (BindingInfo.RepeaterBindings == null)
            {
                InitialiseRepeaterBindings();
            }
            if (BindingInfo.RepeaterBindings.Count == 0)
                InitialiseRepeaterBindings();
            AddRepeaterBindingInfoToTableAndBind();
        }

        private void AddRepeaterBindingInfoToTableAndBind()
        {
            /*foreach (RepeaterBindingInfo rbinfo in BindingInfo.RepeaterBindings)
            {
                DataRow drRepeater = dtRepeaterSections.NewRow();
                drRepeater["Repeater"] = rbinfo.RepeaterName;
                if (rbinfo.Association != null)
                {
                    drRepeater["ID"] = rbinfo.Association.ID;
                    drRepeater["Collapsed Text"] = rbinfo.RepeaterName;
                }
                drRepeater["RepeaterID"] = rbinfo.RepeaterID;
                
                dtRepeaterSections.Rows.Add(drRepeater);
            }
            dgRepeaters.DataSource = this.dtRepeaterSections;
            int Counter = 0;
            foreach (DataGridViewRow row in dgRepeaters.Rows)
            {

                row.Tag = RepeaterSections[Counter];

                Counter++;
            }

            dgRepeaters.RefreshEdit();
            
          
            HideRepeaterIDAndIDColumns();*/
        }

        private void HideRepeaterIDAndIDColumns()
        {
            /*
                        foreach (DataGridViewColumn col in dgRepeaters.Columns)
                        {
                            if ((col.HeaderText == "ID") || (col.HeaderText == "RepeaterID"))
                            {
                                col.Visible = false;
                            }
                        }*/
        }

        private void CreateRepeaterDataTable()
        {
            /*
                        dtRepeaterSections = new DataTable();
                        dtRepeaterSections.Columns.Add(new DataColumn("Repeater", typeof(string)));
                        dtRepeaterSections.Columns.Add(new DataColumn("ID", typeof(int)));
                        dtRepeaterSections.Columns.Add(new DataColumn("Collapsed Text", typeof(string)));
                        dtRepeaterSections.Columns.Add(new DataColumn("RepeaterID", typeof(Guid)));*/
        }

        private void InitialiseRepeaterBindings()
        {
            BindingInfo.RepeaterBindings = new List<RepeaterBindingInfo>();
            foreach (RepeaterSection repeaterSection in RepeaterSections)
            {
                RepeaterBindingInfo rbinfo = new RepeaterBindingInfo();
                rbinfo.RepeaterID = repeaterSection.RepeaterID;
                rbinfo.RepeaterName = repeaterSection.Name;
                rbinfo.RepeaterDirection = RepeaterBindingInfo.Direction.Forward;
                BindingInfo.RepeaterBindings.Add(rbinfo);
            }
        }

        private void EnableOrDisableRepeaterGridAccordingToRepeaterCount()
        {
            /*
            if (RepeaterSections.Count == 0)
            {
                this.dgRepeaters.Enabled = false;
            }
            else
            {
                this.dgRepeaters.Enabled = true;
            }*/
        }

        private void BindLabelGrid()
        {
            try
            {
                dtLabels = new DataTable();
                dtLabels.Columns.Add(new DataColumn("Label", typeof(String)));
                dtLabels.Columns.Add(new DataColumn("Property", typeof(String)));
                foreach (BoundLabel lbl in BoundLabels)
                {
                    if (!BindingInfo.Bindings.ContainsKey(lbl.Name))
                    {
                        BindingInfo.Bindings.Add(lbl.Name, "-none-");
                    }
                }
                foreach (KeyValuePair<string, string> boundInfo in BindingInfo.Bindings)
                {
                    DataRow drKVP = dtLabels.NewRow();
                    drKVP["Label"] = boundInfo.Key;
                    drKVP["Property"] = boundInfo.Value;
                    dtLabels.Rows.Add(drKVP);
                }
                dgBindings.DataSource = dtLabels.DefaultView;
            }
            catch
            {
            }
        }

        private void ResetDataGrids()
        {
            Debug.WriteLine("Resetting datagrids");
            dgBindings.DataSource = null;
            dgBindings.Columns.Clear();
            colProperty.Items.Clear();
            colProperty.Items.Add("-none-");
            colProperty.Items.AddRange(ClassProperties.ToArray());
            dgBindings.Columns.Add(colProperty);
            dgBindings.Columns.Add(colLabel);
            /*this.dgRepeaters.DataSource = null;
            this.dgRepeaters.Columns.Clear();
            AssociationsColumn = new DataGridViewComboBoxColumn();
            AssociationsColumn.Width = 120;
            AssociationsColumn.HeaderText = "Association Types To Be Shown In Repeater";
            this.AssociationsColumn.Items.Clear();
            this.AssociationsColumn.DisplayMember = "Caption";
            this.AssociationsColumn.ValueMember = "ID";
            AssociationsColumn.DataPropertyName = "ID"; // <-- bastardo!
            this.AssociationsColumn.Name = "AssociationsColumn";
            this.AssociationsColumn.Items.AddRange(this.allowedAssociations.ToArray());
            this.dgRepeaters.Columns.Add(this.AssociationsColumn);
            this.dgRepeaters.Columns.Add(this.dataGridViewLinkColumn1);*/
        }

        private void SaveBindings()
        {
            SaveLabelBindings();
            BindingInfo.RepeaterBindings.Clear();
            /*foreach (DataGridViewRow rrow in this.dgRepeaters.Rows)
            {
                object val = rrow.Cells[AssociationsColumn.Name].Value ;
                if (val != null)
                {
                    if ((val.ToString() != "-1") && (val != null) && (val.ToString() != "{}") && (val.ToString() != ""))
                    {
                        Association selectedAssociation = GetSelectedAssociation(val);
                        if (selectedAssociation.ID > 0)
                        {
                            SaveRepeaterBinding(rrow, selectedAssociation);
                        }
                    }
                }
            }*/
        }

        private void SaveLabelBindings()
        {
            BindingInfo.Bindings.Clear();
            foreach (DataGridViewRow r in dgBindings.Rows)
            {
                if (r.Cells[1].EditedFormattedValue.ToString() != "-none-")
                {
                    BindingInfo.Bindings.Add(r.Cells[0].Value.ToString(), r.Cells[1].EditedFormattedValue.ToString());
                }
            }
        }

        private void SaveRepeaterBinding(DataGridViewRow rrow, Association selectedAssociation)
        {
            /*
                RepeaterSection section = RepeaterSections[rrow.Index];
                RepeaterBindingInfo repeaterBindingInfo = new RepeaterBindingInfo();
                repeaterBindingInfo.Association = selectedAssociation;
                repeaterBindingInfo.RepeaterDirection = RepeaterBindingInfo.Direction.Forward;
                repeaterBindingInfo.RepeaterID = section.RepeaterID;
                BindingInfo.RepeaterBindings.Add(repeaterBindingInfo);
                if (rrow.Cells[3].EditedFormattedValue.ToString().Length > 0)
                {
                    repeaterBindingInfo.RepeaterName = rrow.Cells[3].EditedFormattedValue.ToString();
                    section.Text = rrow.Cells[3].EditedFormattedValue.ToString();
                }*/
        }
        #endregion

        #region Find & Locate Methods
        private BoundLabel FindLabel(string ID)
        {
            foreach (BoundLabel lbl in BoundLabels)
            {
                if (lbl.Name == ID)
                {
                    return lbl;
                }
            }
            return null;
        }

        private void HighlightLabel(string LabelID)
        {
            foreach (BoundLabel lbl in BoundLabels)
            {
                if (lbl.Name == LabelID)
                {
                    graphViewPreview.Selection.Clear();
                    graphViewPreview.Selection.Add(lbl);
                }
            }
        }

        private void HighlightRepeater(Guid RepeaterID)
        {
            foreach (RepeaterSection rptsection in RepeaterSections)
            {
                if (rptsection.RepeaterID == RepeaterID)
                {
                    graphViewPreview.Selection.Clear();
                    graphViewPreview.Selection.Add(rptsection);
                }
            }
        }

        private RepeaterSection GetSelectedRepeaterSectionByName(string name)
        {
            foreach (RepeaterSection rsection in RepeaterSections)
            {
                if (rsection.Name == name)
                    return rsection;
            }
            return null;
        }

        private RepeaterSection GetSelectedRepeaterSectionByID(Guid id)
        {
            foreach (RepeaterSection rsection in RepeaterSections)
            {
                if (rsection.RepeaterID == id)
                    return rsection;
            }
            return null;
        }

        private RepeaterBindingInfo GetSelectedRepeaterInfo(Guid selectedRepeaterID)
        {
            foreach (RepeaterBindingInfo rbinfo in bindingInfo.RepeaterBindings)
            {
                if (rbinfo.RepeaterID == selectedRepeaterID)
                {
                    return rbinfo;
                }
            }
            return null;
        }

        private Association GetSelectedAssociation(object val)
        {
            Association selectedAssociation = new Association();
            foreach (Association assoc in allowedAssociations)
            {
                if (assoc.ID == int.Parse(val.ToString()))
                {
                    selectedAssociation = assoc;
                }
            }
            return selectedAssociation;
        }
        #endregion

        #region EventHandlers
        private void classDropdown1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (classDropdown1.SelectedIndex >= 0)
            {
                if (bindingInfo == null)
                    bindingInfo = new BindingInfo();

                bindingInfo.BindingClass = classDropdown1.SelectedClass;

                UpdateInterface();
            }
        }

        private void dgBindings_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridViewLinkCell linkCell = (DataGridViewLinkCell)dgBindings.Rows[e.RowIndex].Cells[e.ColumnIndex];
                HighlightLabel(linkCell.Value.ToString());
            }
        }

        private void btnTestBindings_Click(object sender, EventArgs e)
        {
            SaveBindings();
            if (objTester == null)
                btnCreateInstance_Click(sender, e);
            foreach (KeyValuePair<string, string> kvpair in BindingInfo.Bindings)
            {
                if (objTester != null)
                {
                    string val;
                    object propvalue = objTester.Get(kvpair.Value);
                    if (propvalue != null)
                        val = propvalue.ToString();
                    else
                        val = kvpair.Value;
                    FindLabel(kvpair.Key).Text = val;
                }
            }
            TList<Field> fields = DataRepository.ClientFieldsByClass(classDropdown1.Text);
            TList<DomainDefinition> definitions = DataAccessLayer.DataRepository.Domains(Core.Variables.Instance.ClientProvider);
            int count = propertyGrid1.PropertyTabs[0].GetProperties(objTester).Count;
            for (int i = 0; i < count; i++)
            {
                int attributeCount = propertyGrid1.PropertyTabs[0].GetProperties(objTester)[i].Attributes.Count;
                for (int x = 0; x < attributeCount; x++)
                {
                    EditorAttribute editorAttrib = propertyGrid1.PropertyTabs[0].GetProperties(objTester)[i].Attributes[x] as EditorAttribute;
                    if (editorAttrib != null)
                    {
                        if (editorAttrib.EditorTypeName.IndexOf("UniversalDropdownEditor") > 0)
                        {
                            foreach (DomainDefinition def in definitions)
                            {
                                Field flds = fields.Find(FieldColumn.DataType, def.Name);
                                if (flds != null)
                                {
                                    TList<DomainDefinitionPossibleValue> values = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.DomainDefinitionPossibleValueProvider.GetByDomainDefinitionID(
                                            def.pkid);
                                    // need to find a label bound to this property
                                    foreach (KeyValuePair<string, string> kvpair in BindingInfo.Bindings)
                                    {
                                        if (kvpair.Value == flds.Name)
                                        {
                                            BoundLabel lbl = FindLabel(kvpair.Key);
                                            ArrayList list = new ArrayList();
                                            list.Add("");
                                            foreach (DomainDefinitionPossibleValue val in values)
                                            {
                                                list.Add(val.PossibleValue);
                                            }
                                            lbl.Editable = true;
                                            lbl.EditorStyle = GoTextEditorStyle.ComboBox;
                                            lbl.DropDownList = true;
                                            lbl.Choices = list;
                                        }
                                    }
                                    //FindLabel(kvpair.Key).Text = "a";
                                }
                            }
                            /*
                            foreach (KeyValuePair<string, string> kvpair in BindingInfo.Bindings)
                            {
                                if (objTester != null)
                                {
                                    FindLabel(kvpair.Key).Text = "a";
                                }

                            }*/
                        }
                    }
                }
            }
        }

        private void btnCreateInstance_Click(object sender, EventArgs e)
        {
            objTester = Loader.CreateInstance(classDropdown1.SelectedClass);


            propertyGrid1.SelectedObject = objTester;
            Type t = propertyGrid1.SelectedObject.GetType();
            PropertyInfo[] properties = objTester.GetType().GetProperties();
            foreach (PropertyInfo propInfo in properties)
            {
                if (propInfo.PropertyType.IsSubclassOf(typeof(MetaBase)))
                    propInfo.SetValue(propertyGrid1.SelectedObject,
                                      Activator.CreateInstance(propInfo.PropertyType, false), null);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveBindings();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bindingInfo = null;
            Close();
        }

        private void dgRepeaters_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                /* DataGridViewLinkCell linkCell = (DataGridViewLinkCell)dgRepeaters.Rows[e.RowIndex].Cells[e.ColumnIndex];
                object val = dgRepeaters.Rows[e.RowIndex].Cells[e.ColumnIndex+1].Value;
                if ((val.ToString() != "-1") && (val != null) && (val.ToString() != "{}") && (val.ToString() != ""))
                {
                    int AssociationID = int.Parse(val.ToString());
                    DataView dv = Singletons.GetClassHelper().GetAllowedAssociateClasses(this.bindingInfo.BindingClass);
                    foreach (DataRowView drvAssociatedClass in dv)
                    {
                        if (drvAssociatedClass["CAid"].ToString() == AssociationID.ToString())
                        {
                            Association ass = new Association();
                            ass.AssociationType = drvAssociatedClass["AssociationType"].ToString();
                            ass.AssociationTypeID = int.Parse(drvAssociatedClass["AssociationTypeID"].ToString());
                            ass.ChildClass = drvAssociatedClass["ChildClass"].ToString();
                            ass.ID = int.Parse(drvAssociatedClass["CAid"].ToString());
                            ass.Caption = drvAssociatedClass["Caption"].ToString();
                            ass.ParentClass = this.bindingInfo.BindingClass;
                           // .Association = ass;
                        }
                    }
                }*/
                RepeaterSection selectedSection = RepeaterSections[e.RowIndex];
                HighlightRepeater(selectedSection.RepeaterID);
            }
        }
        #endregion
    }
}