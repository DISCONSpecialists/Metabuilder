namespace MetaBuilder.MetaControls
{
    partial class AssociationList
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
            XPTable.Models.Row row1 = new XPTable.Models.Row();
            this.tableSelection = new XPTable.Models.Table();
            this.columnModel1 = new XPTable.Models.ColumnModel();
            this.cbSelected = new XPTable.Models.CheckBoxColumn();
            this.colAssocType = new XPTable.Models.TextColumn();
            this.colFromObject = new XPTable.Models.TextColumn();
            this.colToObject = new XPTable.Models.TextColumn();
            this.colStatus = new XPTable.Models.TextColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearCurrentSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableModelSelection = new XPTable.Models.TableModel();
            this.tabPageSelection = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.tableModel1 = new XPTable.Models.TableModel();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.tabPageBrowse = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.table1 = new XPTable.Models.Table();
            this.tabControlAdv1 = new Syncfusion.Windows.Forms.Tools.TabControlAdv();
            ((System.ComponentModel.ISupportInitialize)(this.tableSelection)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.tabPageSelection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.tabPageBrowse.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.table1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabControlAdv1)).BeginInit();
            this.tabControlAdv1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableSelection
            // 
            this.tableSelection.ColumnModel = this.columnModel1;
            this.tableSelection.ContextMenuStrip = this.contextMenuStrip1;
            this.tableSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableSelection.Location = new System.Drawing.Point(0, 0);
            this.tableSelection.Name = "tableSelection";
            this.tableSelection.Size = new System.Drawing.Size(603, 319);
            this.tableSelection.TabIndex = 1;
            this.tableSelection.TableModel = this.tableModelSelection;
            this.tableSelection.Text = "table2";
            this.tableSelection.CellPropertyChanged += new XPTable.Events.CellEventHandler(this.tableSelection_CellPropertyChanged);
            // 
            // columnModel1
            // 
            this.columnModel1.Columns.AddRange(new XPTable.Models.Column[] {
            this.cbSelected,
            this.colAssocType,
            this.colFromObject,
            this.colToObject,
            this.colStatus});
            // 
            // cbSelected
            // 
            this.cbSelected.Width = 25;
            // 
            // colAssocType
            // 
            this.colAssocType.Editable = false;
            this.colAssocType.Text = "Association";
            this.colAssocType.Width = 170;
            // 
            // colFromObject
            // 
            this.colFromObject.Editable = false;
            this.colFromObject.Text = "From Object";
            this.colFromObject.Width = 200;
            // 
            // colToObject
            // 
            this.colToObject.Editable = false;
            this.colToObject.Text = "To Object";
            this.colToObject.Width = 200;
            // 
            // colStatus
            // 
            this.colStatus.Enabled = false;
            this.colStatus.Text = "Status";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearCurrentSelectionToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(186, 26);
            // 
            // clearCurrentSelectionToolStripMenuItem
            // 
            this.clearCurrentSelectionToolStripMenuItem.Name = "clearCurrentSelectionToolStripMenuItem";
            this.clearCurrentSelectionToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.clearCurrentSelectionToolStripMenuItem.Text = "Clear Current Selection";
            // 
            // tabPageSelection
            // 
            this.tabPageSelection.Controls.Add(this.tableSelection);
            this.tabPageSelection.Location = new System.Drawing.Point(1, 1);
            this.tabPageSelection.Name = "tabPageSelection";
            this.tabPageSelection.Size = new System.Drawing.Size(603, 319);
            this.tabPageSelection.TabIndex = 1;
            this.tabPageSelection.Text = "Current Selection";
            this.tabPageSelection.ThemesEnabled = true;
            // 
            // tableModel1
            // 
            row1.ChildIndex = 0;
            this.tableModel1.Rows.AddRange(new XPTable.Models.Row[] {
            row1});
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // tabPageBrowse
            // 
            this.tabPageBrowse.Controls.Add(this.table1);
            this.tabPageBrowse.Location = new System.Drawing.Point(1, 1);
            this.tabPageBrowse.Name = "tabPageBrowse";
            this.tabPageBrowse.Size = new System.Drawing.Size(603, 319);
            this.tabPageBrowse.TabIndex = 0;
            this.tabPageBrowse.Text = "Search and Select";
            this.tabPageBrowse.ThemesEnabled = true;
            // 
            // table1
            // 
            this.table1.ColumnModel = this.columnModel1;
            this.table1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table1.Location = new System.Drawing.Point(0, 0);
            this.table1.Name = "table1";
            this.table1.Size = new System.Drawing.Size(603, 319);
            this.table1.TabIndex = 2;
            this.table1.TableModel = this.tableModel1;
            this.table1.Text = "table1";
            this.table1.CellPropertyChanged += new XPTable.Events.CellEventHandler(this.table1_CellPropertyChanged);
            // 
            // tabControlAdv1
            // 
            this.tabControlAdv1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControlAdv1.Controls.Add(this.tabPageBrowse);
            this.tabControlAdv1.Controls.Add(this.tabPageSelection);
            this.tabControlAdv1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlAdv1.Location = new System.Drawing.Point(0, 0);
            this.tabControlAdv1.Name = "tabControlAdv1";
            this.tabControlAdv1.Padding = new System.Drawing.Point(3, 0);
            this.tabControlAdv1.ShowScroll = false;
            this.tabControlAdv1.Size = new System.Drawing.Size(606, 342);
            this.tabControlAdv1.TabGap = 20;
            this.tabControlAdv1.TabIndex = 1;
            this.tabControlAdv1.TabStyle = typeof(Syncfusion.Windows.Forms.Tools.TabRendererDockingWhidbey);
            this.tabControlAdv1.ThemesEnabled = true;
            this.tabControlAdv1.VerticalAlignment = Syncfusion.Windows.Forms.Tools.TabVerticalAlignment.Top;
            // 
            // AssociationList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlAdv1);
            this.Name = "AssociationList";
            this.Size = new System.Drawing.Size(606, 342);
            ((System.ComponentModel.ISupportInitialize)(this.tableSelection)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabPageSelection.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.tabPageBrowse.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.table1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabControlAdv1)).EndInit();
            this.tabControlAdv1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private XPTable.Models.Table tableSelection;
        private XPTable.Models.ColumnModel columnModel1;
        private XPTable.Models.CheckBoxColumn cbSelected;
        private XPTable.Models.TextColumn colAssocType;
        private XPTable.Models.TextColumn colFromObject;
        private XPTable.Models.TextColumn colToObject;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem clearCurrentSelectionToolStripMenuItem;
        private XPTable.Models.TableModel tableModelSelection;
        private Syncfusion.Windows.Forms.Tools.TabPageAdv tabPageSelection;
        private XPTable.Models.TableModel tableModel1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private Syncfusion.Windows.Forms.Tools.TabControlAdv tabControlAdv1;
        private Syncfusion.Windows.Forms.Tools.TabPageAdv tabPageBrowse;
        private XPTable.Models.Table table1;
        private XPTable.Models.TextColumn colStatus;
    }
}
