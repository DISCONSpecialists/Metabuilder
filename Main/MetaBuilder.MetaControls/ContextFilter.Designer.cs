namespace MetaBuilder.MetaControls
{
    partial class ContextFilter
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.bindingSourceTypes = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.dataSetFilter = new System.Data.DataSet();
            this.dataTableItems = new System.Data.DataTable();
            this.dataColumn1 = new System.Data.DataColumn();
            this.dataColumn2 = new System.Data.DataColumn();
            this.dataColumn3 = new System.Data.DataColumn();
            this.dataColumn4 = new System.Data.DataColumn();
            this.dataColumn5 = new System.Data.DataColumn();
            this.dataTableChildren = new System.Data.DataTable();
            this.dataColumn6 = new System.Data.DataColumn();
            this.dataColumn7 = new System.Data.DataColumn();
            this.dataColumn8 = new System.Data.DataColumn();
            this.dataColumn9 = new System.Data.DataColumn();
            this.dataColumn10 = new System.Data.DataColumn();
            this.bindingSourceClasses = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSourceAssociations = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSourceFields = new System.Windows.Forms.BindingSource(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonParentClear = new MetaBuilder.MetaControls.MetaButton();
            this.buttonParentRemove = new MetaBuilder.MetaControls.MetaButton();
            this.buttonParentAdd = new MetaBuilder.MetaControls.MetaButton();
            this.comboBoxParentValue = new System.Windows.Forms.ComboBox();
            this.comboBoxParentType = new System.Windows.Forms.ComboBox();
            this.listViewParents = new System.Windows.Forms.ListView();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonChildClear = new MetaBuilder.MetaControls.MetaButton();
            this.comboBoxChildType = new System.Windows.Forms.ComboBox();
            this.comboBoxChildValue = new System.Windows.Forms.ComboBox();
            this.buttonChildRemove = new MetaBuilder.MetaControls.MetaButton();
            this.listViewChildren = new System.Windows.Forms.ListView();
            this.buttonChildAdd = new MetaBuilder.MetaControls.MetaButton();
            this.buttonReport = new MetaBuilder.MetaControls.MetaButton();
            this.checkBoxInvert = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceTypes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableChildren)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceClasses)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceAssociations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceFields)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bindingSourceTypes
            // 
            this.bindingSourceTypes.AllowNew = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(497, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Add or remove object filters below to affect the diagram, selecting an object fil" +
                "ter will show its value filters";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataSetFilter
            // 
            this.dataSetFilter.DataSetName = "NewDataSet";
            this.dataSetFilter.Relations.AddRange(new System.Data.DataRelation[] {
            new System.Data.DataRelation("ParentToChild", "TableItems", "TableChildren", new string[] {
                        "ColumnID"}, new string[] {
                        "ColumnID"}, false)});
            this.dataSetFilter.Tables.AddRange(new System.Data.DataTable[] {
            this.dataTableItems,
            this.dataTableChildren});
            // 
            // dataTableItems
            // 
            this.dataTableItems.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn1,
            this.dataColumn2,
            this.dataColumn3,
            this.dataColumn4,
            this.dataColumn5});
            this.dataTableItems.Constraints.AddRange(new System.Data.Constraint[] {
            new System.Data.UniqueConstraint("Constraint1", new string[] {
                        "ColumnID"}, true)});
            this.dataTableItems.PrimaryKey = new System.Data.DataColumn[] {
        this.dataColumn5};
            this.dataTableItems.TableName = "TableItems";
            // 
            // dataColumn1
            // 
            this.dataColumn1.ColumnName = "ColumnEnabled";
            this.dataColumn1.DataType = typeof(bool);
            // 
            // dataColumn2
            // 
            this.dataColumn2.ColumnName = "ColumnType";
            // 
            // dataColumn3
            // 
            this.dataColumn3.ColumnName = "ColumnValue";
            // 
            // dataColumn4
            // 
            this.dataColumn4.ColumnName = "ColumnShow";
            this.dataColumn4.DataType = typeof(bool);
            // 
            // dataColumn5
            // 
            this.dataColumn5.AllowDBNull = false;
            this.dataColumn5.ColumnName = "ColumnID";
            this.dataColumn5.DataType = typeof(System.Guid);
            // 
            // dataTableChildren
            // 
            this.dataTableChildren.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn6,
            this.dataColumn7,
            this.dataColumn8,
            this.dataColumn9,
            this.dataColumn10});
            this.dataTableChildren.Constraints.AddRange(new System.Data.Constraint[] {
            new System.Data.ForeignKeyConstraint("ParentToChild", "TableItems", new string[] {
                        "ColumnID"}, new string[] {
                        "ColumnID"}, System.Data.AcceptRejectRule.None, System.Data.Rule.Cascade, System.Data.Rule.Cascade)});
            this.dataTableChildren.TableName = "TableChildren";
            // 
            // dataColumn6
            // 
            this.dataColumn6.ColumnName = "ColumnEnabled";
            this.dataColumn6.DataType = typeof(bool);
            // 
            // dataColumn7
            // 
            this.dataColumn7.ColumnName = "ColumnType";
            // 
            // dataColumn8
            // 
            this.dataColumn8.ColumnName = "ColumnValue";
            // 
            // dataColumn9
            // 
            this.dataColumn9.ColumnName = "ColumnShow";
            this.dataColumn9.DataType = typeof(bool);
            // 
            // dataColumn10
            // 
            this.dataColumn10.ColumnName = "ColumnID";
            this.dataColumn10.DataType = typeof(System.Guid);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 13);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.checkBoxInvert);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.buttonParentClear);
            this.splitContainer1.Panel1.Controls.Add(this.buttonParentRemove);
            this.splitContainer1.Panel1.Controls.Add(this.buttonParentAdd);
            this.splitContainer1.Panel1.Controls.Add(this.comboBoxParentValue);
            this.splitContainer1.Panel1.Controls.Add(this.comboBoxParentType);
            this.splitContainer1.Panel1.Controls.Add(this.listViewParents);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.buttonChildClear);
            this.splitContainer1.Panel2.Controls.Add(this.comboBoxChildType);
            this.splitContainer1.Panel2.Controls.Add(this.comboBoxChildValue);
            this.splitContainer1.Panel2.Controls.Add(this.buttonChildRemove);
            this.splitContainer1.Panel2.Controls.Add(this.listViewChildren);
            this.splitContainer1.Panel2.Controls.Add(this.buttonChildAdd);
            this.splitContainer1.Size = new System.Drawing.Size(670, 152);
            this.splitContainer1.SplitterDistance = 350;
            this.splitContainer1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(350, 18);
            this.label2.TabIndex = 7;
            this.label2.Text = "Object Filters";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonParentClear
            // 
            this.buttonParentClear.CornerRadius = new Ascend.CornerRadius(2);
            this.buttonParentClear.GradientLowColor = System.Drawing.Color.DarkGray;
            this.buttonParentClear.Location = new System.Drawing.Point(268, 121);
            this.buttonParentClear.Name = "buttonParentClear";
            this.buttonParentClear.Size = new System.Drawing.Size(75, 23);
            this.buttonParentClear.StayActiveAfterClick = false;
            this.buttonParentClear.TabIndex = 6;
            this.buttonParentClear.Text = "Clear";
            this.buttonParentClear.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonParentClear.Click += new System.EventHandler(this.buttonParentClear_Click);
            // 
            // buttonParentRemove
            // 
            this.buttonParentRemove.CornerRadius = new Ascend.CornerRadius(2);
            this.buttonParentRemove.GradientLowColor = System.Drawing.Color.DarkGray;
            this.buttonParentRemove.Location = new System.Drawing.Point(268, 77);
            this.buttonParentRemove.Name = "buttonParentRemove";
            this.buttonParentRemove.Size = new System.Drawing.Size(75, 23);
            this.buttonParentRemove.StayActiveAfterClick = false;
            this.buttonParentRemove.TabIndex = 6;
            this.buttonParentRemove.Text = "Remove";
            this.buttonParentRemove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonParentRemove.Click += new System.EventHandler(this.buttonParentRemove_Click);
            // 
            // buttonParentAdd
            // 
            this.buttonParentAdd.CornerRadius = new Ascend.CornerRadius(2);
            this.buttonParentAdd.GradientLowColor = System.Drawing.Color.DarkGray;
            this.buttonParentAdd.Location = new System.Drawing.Point(268, 48);
            this.buttonParentAdd.Name = "buttonParentAdd";
            this.buttonParentAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonParentAdd.StayActiveAfterClick = false;
            this.buttonParentAdd.TabIndex = 6;
            this.buttonParentAdd.Text = "Add";
            this.buttonParentAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonParentAdd.Click += new System.EventHandler(this.buttonParentAdd_Click);
            // 
            // comboBoxParentValue
            // 
            this.comboBoxParentValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxParentValue.FormattingEnabled = true;
            this.comboBoxParentValue.Location = new System.Drawing.Point(128, 21);
            this.comboBoxParentValue.Name = "comboBoxParentValue";
            this.comboBoxParentValue.Size = new System.Drawing.Size(217, 21);
            this.comboBoxParentValue.TabIndex = 5;
            // 
            // comboBoxParentType
            // 
            this.comboBoxParentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxParentType.FormattingEnabled = true;
            this.comboBoxParentType.Items.AddRange(new object[] {
            "Workspace",
            "Class",
            "Association",
            "Artefact",
            "Diagram"});
            this.comboBoxParentType.Location = new System.Drawing.Point(3, 21);
            this.comboBoxParentType.Name = "comboBoxParentType";
            this.comboBoxParentType.Size = new System.Drawing.Size(121, 21);
            this.comboBoxParentType.TabIndex = 4;
            this.comboBoxParentType.SelectedIndexChanged += new System.EventHandler(this.comboBoxParentType_SelectedIndexChanged);
            // 
            // listViewParents
            // 
            this.listViewParents.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listViewParents.HideSelection = false;
            this.listViewParents.Location = new System.Drawing.Point(3, 48);
            this.listViewParents.MultiSelect = false;
            this.listViewParents.Name = "listViewParents";
            this.listViewParents.Size = new System.Drawing.Size(263, 96);
            this.listViewParents.TabIndex = 3;
            this.listViewParents.UseCompatibleStateImageBehavior = false;
            this.listViewParents.View = System.Windows.Forms.View.List;
            this.listViewParents.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.listViewParents_DrawItem);
            this.listViewParents.SelectedIndexChanged += new System.EventHandler(this.listViewParents_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(316, 18);
            this.label3.TabIndex = 8;
            this.label3.Text = "Value Filters";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonChildClear
            // 
            this.buttonChildClear.CornerRadius = new Ascend.CornerRadius(2);
            this.buttonChildClear.GradientLowColor = System.Drawing.Color.DarkGray;
            this.buttonChildClear.Location = new System.Drawing.Point(233, 121);
            this.buttonChildClear.Name = "buttonChildClear";
            this.buttonChildClear.Size = new System.Drawing.Size(75, 23);
            this.buttonChildClear.StayActiveAfterClick = false;
            this.buttonChildClear.TabIndex = 6;
            this.buttonChildClear.Text = "Clear";
            this.buttonChildClear.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonChildClear.Click += new System.EventHandler(this.buttonChildClear_Click);
            // 
            // comboBoxChildType
            // 
            this.comboBoxChildType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxChildType.FormattingEnabled = true;
            this.comboBoxChildType.Location = new System.Drawing.Point(3, 21);
            this.comboBoxChildType.Name = "comboBoxChildType";
            this.comboBoxChildType.Size = new System.Drawing.Size(121, 21);
            this.comboBoxChildType.TabIndex = 4;
            this.comboBoxChildType.SelectedIndexChanged += new System.EventHandler(this.comboBoxChildType_SelectedIndexChanged);
            // 
            // comboBoxChildValue
            // 
            this.comboBoxChildValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.comboBoxChildValue.FormattingEnabled = true;
            this.comboBoxChildValue.Location = new System.Drawing.Point(128, 21);
            this.comboBoxChildValue.Name = "comboBoxChildValue";
            this.comboBoxChildValue.Size = new System.Drawing.Size(183, 21);
            this.comboBoxChildValue.TabIndex = 5;
            this.comboBoxChildValue.SelectedIndexChanged += new System.EventHandler(this.comboBoxChildValue_SelectedIndexChanged);
            this.comboBoxChildValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBoxChildValue_KeyDown);
            this.comboBoxChildValue.TextChanged += new System.EventHandler(this.comboBoxChildValue_TextChanged);
            // 
            // buttonChildRemove
            // 
            this.buttonChildRemove.CornerRadius = new Ascend.CornerRadius(2);
            this.buttonChildRemove.GradientLowColor = System.Drawing.Color.DarkGray;
            this.buttonChildRemove.Location = new System.Drawing.Point(233, 77);
            this.buttonChildRemove.Name = "buttonChildRemove";
            this.buttonChildRemove.Size = new System.Drawing.Size(75, 23);
            this.buttonChildRemove.StayActiveAfterClick = false;
            this.buttonChildRemove.TabIndex = 6;
            this.buttonChildRemove.Text = "Remove";
            this.buttonChildRemove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonChildRemove.Click += new System.EventHandler(this.buttonChildRemove_Click);
            // 
            // listViewChildren
            // 
            this.listViewChildren.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listViewChildren.GridLines = true;
            this.listViewChildren.HideSelection = false;
            this.listViewChildren.Location = new System.Drawing.Point(3, 48);
            this.listViewChildren.Name = "listViewChildren";
            this.listViewChildren.Size = new System.Drawing.Size(228, 96);
            this.listViewChildren.TabIndex = 3;
            this.listViewChildren.UseCompatibleStateImageBehavior = false;
            this.listViewChildren.View = System.Windows.Forms.View.List;
            this.listViewChildren.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.listViewChildren_DrawItem);
            this.listViewChildren.SelectedIndexChanged += new System.EventHandler(this.listViewChildren_SelectedIndexChanged);
            // 
            // buttonChildAdd
            // 
            this.buttonChildAdd.CornerRadius = new Ascend.CornerRadius(2);
            this.buttonChildAdd.GradientLowColor = System.Drawing.Color.DarkGray;
            this.buttonChildAdd.Location = new System.Drawing.Point(233, 48);
            this.buttonChildAdd.Name = "buttonChildAdd";
            this.buttonChildAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonChildAdd.StayActiveAfterClick = false;
            this.buttonChildAdd.TabIndex = 6;
            this.buttonChildAdd.Text = "Add";
            this.buttonChildAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonChildAdd.Click += new System.EventHandler(this.buttonChildAdd_Click);
            // 
            // buttonReport
            // 
            this.buttonReport.CornerRadius = new Ascend.CornerRadius(2);
            this.buttonReport.GradientLowColor = System.Drawing.Color.DarkGray;
            this.buttonReport.Location = new System.Drawing.Point(705, 42);
            this.buttonReport.Name = "buttonReport";
            this.buttonReport.Size = new System.Drawing.Size(79, 104);
            this.buttonReport.StayActiveAfterClick = false;
            this.buttonReport.TabIndex = 7;
            this.buttonReport.Text = "QUERY GEN";
            this.buttonReport.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkBoxInvert
            // 
            this.checkBoxInvert.AutoSize = true;
            this.checkBoxInvert.Location = new System.Drawing.Point(279, 103);
            this.checkBoxInvert.Name = "checkBoxInvert";
            this.checkBoxInvert.Size = new System.Drawing.Size(53, 17);
            this.checkBoxInvert.TabIndex = 8;
            this.checkBoxInvert.Text = "Invert";
            this.checkBoxInvert.UseVisualStyleBackColor = true;
            this.checkBoxInvert.CheckedChanged += new System.EventHandler(this.checkBoxInvert_CheckedChanged);
            // 
            // ContextFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonReport);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ContextFilter";
            this.Size = new System.Drawing.Size(670, 165);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceTypes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableChildren)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceClasses)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceAssociations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceFields)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.BindingSource bindingSourceTypes;
        private System.Windows.Forms.BindingSource bindingSourceClasses;
        private System.Windows.Forms.BindingSource bindingSourceAssociations;
        private System.Windows.Forms.BindingSource bindingSourceFields;
        private System.Data.DataSet dataSetFilter;
        private System.Data.DataTable dataTableItems;
        private System.Data.DataColumn dataColumn1;
        private System.Data.DataColumn dataColumn2;
        private System.Data.DataColumn dataColumn3;
        private System.Data.DataColumn dataColumn4;
        private System.Data.DataColumn dataColumn5;
        private System.Data.DataTable dataTableChildren;
        private System.Data.DataColumn dataColumn6;
        private System.Data.DataColumn dataColumn7;
        private System.Data.DataColumn dataColumn8;
        private System.Data.DataColumn dataColumn9;
        private System.Data.DataColumn dataColumn10;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private MetaControls.MetaButton buttonParentClear;
        private MetaControls.MetaButton buttonParentRemove;
        private MetaControls.MetaButton buttonParentAdd;
        private System.Windows.Forms.ComboBox comboBoxParentValue;
        private System.Windows.Forms.ComboBox comboBoxParentType;
        private System.Windows.Forms.ListView listViewParents;
        private MetaControls.MetaButton buttonChildClear;
        private System.Windows.Forms.ComboBox comboBoxChildType;
        private System.Windows.Forms.ComboBox comboBoxChildValue;
        private MetaControls.MetaButton buttonChildRemove;
        private System.Windows.Forms.ListView listViewChildren;
        private MetaControls.MetaButton buttonChildAdd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private MetaButton buttonReport;
        private System.Windows.Forms.CheckBox checkBoxInvert;
    }
}
