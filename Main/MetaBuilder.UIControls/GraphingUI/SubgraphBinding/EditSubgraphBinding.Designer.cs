namespace MetaBuilder.UIControls.GraphingUI.SubgraphBinding
{
    partial class EditSubgraphBinding
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
            this.tableModel1 = new XPTable.Models.TableModel();
            this.tabControlAdv1 = new Syncfusion.Windows.Forms.Tools.TabControlAdv();
            this.tabPageBrowse = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.objectTable = new XPTable.Models.Table();
            this.columnModel1 = new XPTable.Models.ColumnModel();
            this.colClass = new XPTable.Models.TextColumn();
            this.colAssociation = new XPTable.Models.ComboBoxColumn();
            this.colCaption = new XPTable.Models.TextColumn();
            this.tabPageSelection = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.tableSelection = new XPTable.Models.Table();
            this.columnModelObjects = new XPTable.Models.ColumnModel();
            this.textColumn1 = new XPTable.Models.TextColumn();
            this.textColumn2 = new XPTable.Models.TextColumn();
            this.comboBoxColumn1 = new XPTable.Models.ComboBoxColumn();
            this.textColumn3 = new XPTable.Models.TextColumn();
            this.buttonColumnRemove = new XPTable.Models.ButtonColumn();
            this.tableModelObjects = new XPTable.Models.TableModel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOK = new MetaControls.MetaButton();
            this.btnCancel = new MetaControls.MetaButton();
            this.textColumn4 = new XPTable.Models.TextColumn();
            ((System.ComponentModel.ISupportInitialize)(this.tabControlAdv1)).BeginInit();
            this.tabControlAdv1.SuspendLayout();
            this.tabPageBrowse.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectTable)).BeginInit();
            this.tabPageSelection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tableSelection)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlAdv1
            // 
            this.tabControlAdv1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            //this.tabControlAdv1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            //            | System.Windows.Forms.AnchorStyles.Left)
            //            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlAdv1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlAdv1.Controls.Add(this.tabPageBrowse);
            this.tabControlAdv1.Controls.Add(this.tabPageSelection);
            this.tabControlAdv1.Location = new System.Drawing.Point(0, 0);
            this.tabControlAdv1.Name = "tabControlAdv1";
            this.tabControlAdv1.Padding = new System.Drawing.Point(3, 0);
            this.tabControlAdv1.ShowScroll = false;
            //this.tabControlAdv1.Size = new System.Drawing.Size(750, 288);
            this.tabControlAdv1.TabGap = 20;
            this.tabControlAdv1.TabIndex = 5;
            this.tabControlAdv1.TabStyle = typeof(Syncfusion.Windows.Forms.Tools.TabRendererDockingWhidbey);
            this.tabControlAdv1.ThemesEnabled = true;
            this.tabControlAdv1.VerticalAlignment = Syncfusion.Windows.Forms.Tools.TabVerticalAlignment.Top;
            // 
            // tabPageBrowse
            // 
            this.tabPageBrowse.Controls.Add(this.objectTable);
            this.tabPageBrowse.Location = new System.Drawing.Point(1, 1);
            this.tabPageBrowse.Name = "tabPageBrowse";
            this.tabPageBrowse.Size = new System.Drawing.Size(621, 223);
            this.tabPageBrowse.TabIndex = 0;
            this.tabPageBrowse.Text = "Class Default Associations";
            this.tabPageBrowse.ThemesEnabled = true;
            // 
            // objectTable
            // 
            this.objectTable.ColumnModel = this.columnModel1;
            this.objectTable.ColumnResizing = false;
            this.objectTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTable.Location = new System.Drawing.Point(0, 0);
            this.objectTable.Name = "objectTable";
            //this.objectTable.Size = new System.Drawing.Size(750, 223);
            this.objectTable.TabIndex = 4;
            this.objectTable.TableModel = this.tableModel1;
            this.objectTable.Text = "table1";
            this.objectTable.BeforePaintCell += new XPTable.Events.PaintCellEventHandler(this.objectTable_BeforePaintCell);
            this.objectTable.CellMouseEnter += new XPTable.Events.CellMouseEventHandler(this.objectTable_CellMouseEnter);
            this.objectTable.CellGotFocus += new XPTable.Events.CellFocusEventHandler(this.objectTable_CellGotFocus);
            this.objectTable.CellClick += new XPTable.Events.CellMouseEventHandler(this.objectTable_CellClick);
            this.objectTable.BeginEditing += new XPTable.Events.CellEditEventHandler(this.objectTable_BeginEditing);
            this.objectTable.CellMouseDown += new XPTable.Events.CellMouseEventHandler(this.objectTable_CellMouseDown);
            // 
            // columnModel1
            // 
            this.columnModel1.Columns.AddRange(new XPTable.Models.Column[] {
            this.colClass,
            this.colAssociation,
            this.colCaption});
            // 
            // colClass
            // 
            this.colClass.Editable = false;
            this.colClass.Text = "Class / Item";
            this.colClass.Width = 250;
            // 
            // colAssociation
            // 
            this.colAssociation.Text = "Association";
            this.colAssociation.Width = 150;
            // 
            // colCaption
            // 
            this.colCaption.Text = "Name / Caption";
            this.colCaption.Width = 250;
            // 
            // tabPageSelection
            // 
            this.tabPageSelection.Controls.Add(this.tableSelection);
            this.tabPageSelection.Location = new System.Drawing.Point(1, 1);
            this.tabPageSelection.Name = "tabPageSelection";
            this.tabPageSelection.Size = new System.Drawing.Size(698, 265);
            this.tabPageSelection.TabIndex = 1;
            this.tabPageSelection.Text = "Instance Associations";
            this.tabPageSelection.ThemesEnabled = true;
            // 
            // tableSelection
            // 
            this.tableSelection.ColumnModel = this.columnModelObjects;
            this.tableSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableSelection.Location = new System.Drawing.Point(0, 0);
            this.tableSelection.Name = "tableSelection";
            //this.tableSelection.Size = new System.Drawing.Size(750, 265);
            this.tableSelection.TabIndex = 1;
            this.tableSelection.TableModel = this.tableModelObjects;
            this.tableSelection.Text = "table2";
            this.tableSelection.BeforePaintCell += new XPTable.Events.PaintCellEventHandler(this.tableSelection_BeforePaintCell);
            this.tableSelection.CellMouseEnter += new XPTable.Events.CellMouseEventHandler(this.tableSelection_CellMouseEnter);
            this.tableSelection.CellGotFocus += new XPTable.Events.CellFocusEventHandler(this.tableSelection_CellGotFocus);
            this.tableSelection.CellClick += new XPTable.Events.CellMouseEventHandler(this.tableSelection_CellClick);
            this.tableSelection.BeginEditing += new XPTable.Events.CellEditEventHandler(this.tableSelection_BeginEditing);
            this.tableSelection.CellMouseDown += new XPTable.Events.CellMouseEventHandler(this.tableSelection_CellMouseDown);
            this.tableSelection.CellButtonClicked += new XPTable.Events.CellButtonEventHandler(this.tableSelection_CellButtonClicked);
            // 
            // columnModelObjects
            // 
            this.columnModelObjects.Columns.AddRange(new XPTable.Models.Column[] {
            this.textColumn1,
            this.textColumn2,
            this.comboBoxColumn1,
            this.textColumn3,
            this.textColumn4
            //this.buttonColumnRemove
            });
            // 
            // textColumn1
            // 
            this.textColumn1.Text = "Object";
            this.textColumn1.Width = 150;
            // 
            // textColumn2
            // 
            this.textColumn2.Text = "Class";
            this.textColumn2.Width = 100;
            // 
            // comboBoxColumn1
            // 
            this.comboBoxColumn1.Text = "Association";
            this.comboBoxColumn1.Width = 140;
            // 
            // textColumn3
            // 
            this.textColumn3.Text = "Name / Caption";
            this.textColumn3.Width = 250;
            // 
            // buttonColumnRemove
            // 
            this.buttonColumnRemove.Text = "Remove";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 287);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(697, 28);
            this.panel1.TabIndex = 6;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            //this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(538, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            //this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(619, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // textColumn4
            // 
            this.textColumn4.Text = "Status";
            // 
            // EditSubgraphBinding
            // 
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(810, 315);
            this.Controls.Add(this.tabControlAdv1);
            this.Controls.Add(this.panel1);
            this.Name = "EditSubgraphBinding";
            this.Text = "Edit Subgraph Associations";
            this.Load += new System.EventHandler(this.EditSubgraphBinding_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tabControlAdv1)).EndInit();
            this.tabControlAdv1.ResumeLayout(false);
            this.tabPageBrowse.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.objectTable)).EndInit();
            this.tabPageSelection.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tableSelection)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

            this.panel1.SendToBack();

        }

        #endregion

        private XPTable.Models.TableModel tableModel1;
        private Syncfusion.Windows.Forms.Tools.TabControlAdv tabControlAdv1;
        private Syncfusion.Windows.Forms.Tools.TabPageAdv tabPageBrowse;
        private XPTable.Models.Table objectTable;
        private XPTable.Models.ColumnModel columnModel1;
        private XPTable.Models.TextColumn colClass;
        private Syncfusion.Windows.Forms.Tools.TabPageAdv tabPageSelection;
        private XPTable.Models.Table tableSelection;
        private XPTable.Models.TableModel tableModelObjects;
        private XPTable.Models.ComboBoxColumn colAssociation;
        private XPTable.Models.TextColumn colCaption;
        private System.Windows.Forms.Panel panel1;
        private MetaControls.MetaButton btnOK;
        private MetaControls.MetaButton btnCancel;
        private XPTable.Models.ColumnModel columnModelObjects;
        private XPTable.Models.TextColumn textColumn1;
        private XPTable.Models.TextColumn textColumn2;
        private XPTable.Models.ComboBoxColumn comboBoxColumn1;
        private XPTable.Models.TextColumn textColumn3;
        private XPTable.Models.ButtonColumn buttonColumnRemove;
        private XPTable.Models.TextColumn textColumn4;
    }
}