namespace MetaBuilder.UIControls.Dialogs.DatabaseManagement
{
    partial class ManageArtefacts
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            XPTable.Models.Row row3 = new XPTable.Models.Row();
            this.btnApplyFilter = new MetaControls.MetaButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listArtefactClass = new System.Windows.Forms.ComboBox();
            this.listAssociationType = new System.Windows.Forms.ComboBox();
            this.listToClass = new System.Windows.Forms.ComboBox();
            this.listFromClass = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnApplyChanges = new MetaControls.MetaButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtToPKID = new System.Windows.Forms.TextBox();
            this.txtFromPKID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.table1 = new XPTable.Models.Table();
            this.columnModel1 = new XPTable.Models.ColumnModel();
            this.textColumn7 = new XPTable.Models.TextColumn();
            this.textColumn8 = new XPTable.Models.TextColumn();
            this.textColumn9 = new XPTable.Models.TextColumn();
            this.textColumn11 = new XPTable.Models.TextColumn();
            this.textColumn10 = new XPTable.Models.TextColumn();
            this.checkBoxColumn1 = new XPTable.Models.CheckBoxColumn();
            this.textColumn1 = new XPTable.Models.TextColumn();
            this.textColumn2 = new XPTable.Models.TextColumn();
            this.textColumn3 = new XPTable.Models.TextColumn();
            this.textColumn4 = new XPTable.Models.TextColumn();
            this.textColumn5 = new XPTable.Models.TextColumn();
            this.textColumn6 = new XPTable.Models.TextColumn();
            this.tableModel1 = new XPTable.Models.TableModel();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.table1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnApplyFilter
            // 
            this.btnApplyFilter.Location = new System.Drawing.Point(425, 9);
            this.btnApplyFilter.Name = "btnApplyFilter";
            this.btnApplyFilter.Size = new System.Drawing.Size(53, 52);
            this.btnApplyFilter.TabIndex = 0;
            this.btnApplyFilter.Text = "Go";
            this.btnApplyFilter.Click += new System.EventHandler(this.btnApplyFilter_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(774, 73);
            this.panel1.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listArtefactClass);
            this.groupBox2.Controls.Add(this.listAssociationType);
            this.groupBox2.Controls.Add(this.listToClass);
            this.groupBox2.Controls.Add(this.listFromClass);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.btnApplyChanges);
            this.groupBox2.Controls.Add(this.btnApplyFilter);
            this.groupBox2.Location = new System.Drawing.Point(163, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(608, 67);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "List By Association Type";
            // 
            // listArtefactClass
            // 
            this.listArtefactClass.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.listArtefactClass.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.listArtefactClass.FormattingEnabled = true;
            this.listArtefactClass.Location = new System.Drawing.Point(298, 39);
            this.listArtefactClass.Name = "listArtefactClass";
            this.listArtefactClass.Size = new System.Drawing.Size(121, 21);
            this.listArtefactClass.TabIndex = 1;
            // 
            // listAssociationType
            // 
            this.listAssociationType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.listAssociationType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.listAssociationType.FormattingEnabled = true;
            this.listAssociationType.Location = new System.Drawing.Point(298, 13);
            this.listAssociationType.Name = "listAssociationType";
            this.listAssociationType.Size = new System.Drawing.Size(121, 21);
            this.listAssociationType.TabIndex = 1;
            // 
            // listToClass
            // 
            this.listToClass.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.listToClass.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.listToClass.FormattingEnabled = true;
            this.listToClass.Location = new System.Drawing.Point(77, 40);
            this.listToClass.Name = "listToClass";
            this.listToClass.Size = new System.Drawing.Size(121, 21);
            this.listToClass.TabIndex = 1;
            // 
            // listFromClass
            // 
            this.listFromClass.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.listFromClass.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.listFromClass.FormattingEnabled = true;
            this.listFromClass.Location = new System.Drawing.Point(77, 13);
            this.listFromClass.Name = "listFromClass";
            this.listFromClass.Size = new System.Drawing.Size(121, 21);
            this.listFromClass.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(204, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Artefact Class";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(204, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Association Type";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "To Class";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "From Class";
            // 
            // btnApplyChanges
            // 
            this.btnApplyChanges.Location = new System.Drawing.Point(484, 9);
            this.btnApplyChanges.Name = "btnApplyChanges";
            this.btnApplyChanges.Size = new System.Drawing.Size(118, 52);
            this.btnApplyChanges.TabIndex = 0;
            this.btnApplyChanges.Text = "Apply Changes";
            this.btnApplyChanges.Click += new System.EventHandler(this.btnApplyChanges_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtToPKID);
            this.groupBox1.Controls.Add(this.txtFromPKID);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(154, 67);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "List By PKIDs";
            // 
            // txtToPKID
            // 
            this.txtToPKID.Location = new System.Drawing.Point(42, 40);
            this.txtToPKID.Name = "txtToPKID";
            this.txtToPKID.Size = new System.Drawing.Size(67, 20);
            this.txtToPKID.TabIndex = 3;
            // 
            // txtFromPKID
            // 
            this.txtFromPKID.Location = new System.Drawing.Point(42, 14);
            this.txtFromPKID.Name = "txtFromPKID";
            this.txtFromPKID.Size = new System.Drawing.Size(67, 20);
            this.txtFromPKID.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "To";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "From";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.table1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 73);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(774, 330);
            this.panel2.TabIndex = 2;
            // 
            // table1
            // 
            this.table1.AllowSelection = false;
            this.table1.ColumnModel = this.columnModel1;
            this.table1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table1.Enabled = false;
            this.table1.Location = new System.Drawing.Point(0, 0);
            this.table1.Name = "table1";
            this.table1.Size = new System.Drawing.Size(774, 330);
            this.table1.TabIndex = 3;
            this.table1.TableModel = this.tableModel1;
            this.table1.Text = "table1";
            // 
            // columnModel1
            // 
            this.columnModel1.Columns.AddRange(new XPTable.Models.Column[] {
            this.textColumn7,
            this.textColumn8,
            this.textColumn9,
            this.textColumn11,
            this.textColumn10,
            this.checkBoxColumn1,
            this.textColumn1,
            this.textColumn2,
            this.textColumn3,
            this.textColumn4,
            this.textColumn5,
            this.textColumn6});
            // 
            // textColumn7
            // 
            this.textColumn7.Text = "MetaObject<From>";
            this.textColumn7.Width = 125;
            // 
            // textColumn8
            // 
            this.textColumn8.Text = "MetaObject<To>";
            this.textColumn8.Width = 125;
            // 
            // textColumn9
            // 
            this.textColumn9.Text = "AssociationType";
            this.textColumn9.Width = 125;
            // 
            // textColumn11
            // 
            this.textColumn11.Text = "Artefact";
            this.textColumn11.Width = 150;
            // 
            // textColumn10
            // 
            this.textColumn10.Text = "Artefact Class";
            this.textColumn10.Width = 125;
            // 
            // checkBoxColumn1
            // 
            this.checkBoxColumn1.Text = "Remove";
            // 
            // textColumn1
            // 
            this.textColumn1.Text = "From PKID";
            this.textColumn1.Width = 80;
            // 
            // textColumn2
            // 
            this.textColumn2.Text = "From Machine";
            this.textColumn2.Width = 80;
            // 
            // textColumn3
            // 
            this.textColumn3.Text = "From Class";
            // 
            // textColumn4
            // 
            this.textColumn4.Text = "To PKID";
            // 
            // textColumn5
            // 
            this.textColumn5.Text = "To Machine";
            this.textColumn5.Width = 80;
            // 
            // textColumn6
            // 
            this.textColumn6.Text = "To Class";
            this.textColumn6.Width = 80;
            // 
            // tableModel1
            // 
            row3.ChildIndex = 0;
            this.tableModel1.Rows.AddRange(new XPTable.Models.Row[] {
            row3});
            // 
            // ManageArtefacts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 403);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "ManageArtefacts";
            this.Text = "Manage Artefacts";
            this.Load += new System.EventHandler(this.ManageArtefacts_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.table1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MetaControls.MetaButton btnApplyFilter;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtFromPKID;
        private System.Windows.Forms.TextBox txtToPKID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox listAssociationType;
        private System.Windows.Forms.ComboBox listToClass;
        private System.Windows.Forms.ComboBox listFromClass;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox listArtefactClass;
        private System.Windows.Forms.Label label6;
        private XPTable.Models.Table table1;
        private XPTable.Models.ColumnModel columnModel1;
        private XPTable.Models.TextColumn textColumn7;
        private XPTable.Models.TextColumn textColumn8;
        private XPTable.Models.TextColumn textColumn9;
        private XPTable.Models.TextColumn textColumn1;
        private XPTable.Models.TextColumn textColumn2;
        private XPTable.Models.TextColumn textColumn3;
        private XPTable.Models.TextColumn textColumn4;
        private XPTable.Models.TextColumn textColumn5;
        private XPTable.Models.TextColumn textColumn6;
        private XPTable.Models.TableModel tableModel1;
        private XPTable.Models.TextColumn textColumn10;
        private XPTable.Models.TextColumn textColumn11;
        private MetaControls.MetaButton btnApplyChanges;
        private XPTable.Models.CheckBoxColumn checkBoxColumn1;
    }
}