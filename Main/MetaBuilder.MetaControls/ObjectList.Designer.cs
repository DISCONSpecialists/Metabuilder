namespace MetaBuilder.MetaControls
{
    partial class ObjectList
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
            this.columnModel1 = new XPTable.Models.ColumnModel();
            this.colSelected = new XPTable.Models.CheckBoxColumn();
            this.colObject = new XPTable.Models.TextColumn();
            this.colClass = new XPTable.Models.TextColumn();
            this.colMoreInfo = new XPTable.Models.ButtonColumn();
            this.colWorkspace = new XPTable.Models.TextColumn();
            this.colPKID = new XPTable.Models.TextColumn();
            this.colMachine = new XPTable.Models.TextColumn();
            this.colStatus = new XPTable.Models.TextColumn();
            this.tableModel1 = new XPTable.Models.TableModel();
            this.tabControlAdv1 = new Syncfusion.Windows.Forms.Tools.TabControlAdv();
            this.tabPageBrowse = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.objectTable = new XPTable.Models.Table();
            this.tabPageSelection = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.tableSelection = new XPTable.Models.Table();
            this.tableModelSelection = new XPTable.Models.TableModel();
            ((System.ComponentModel.ISupportInitialize)(this.tabControlAdv1)).BeginInit();
            this.tabControlAdv1.SuspendLayout();
            this.tabPageBrowse.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectTable)).BeginInit();
            this.tabPageSelection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tableSelection)).BeginInit();
            this.SuspendLayout();
            // 
            // columnModel1
            // 
            this.columnModel1.Columns.AddRange(new XPTable.Models.Column[] {
            this.colSelected,
            this.colObject,
            this.colClass,
            this.colMoreInfo,
            this.colWorkspace,
            this.colPKID,
            this.colMachine,});
            if (Core.Variables.Instance.IsServer)
            {
                this.columnModel1.Columns.Add(this.colStatus);
            }
            // 
            // colSelected
            // 
            this.colSelected.Width = 16;
            // 
            // colObject
            // 
            this.colObject.Editable = false;
            this.colObject.Text = "Object";
            this.colObject.Width = 200;
            // 
            // colClass
            // 
            this.colClass.Editable = false;
            this.colClass.Text = "Class";
            this.colClass.Width = 100;
            // 
            // colMoreInfo
            // 
            this.colMoreInfo.Text = "More Info";
            this.colMoreInfo.Width = 60;
            // 
            // colWorkspace
            // 
            this.colWorkspace.Editable = false;
            this.colWorkspace.Text = "Workspace";
            this.colWorkspace.Width = 100;
            // 
            // colPKID
            // 
            this.colPKID.Editable = false;
            this.colPKID.Text = "pkid";
            this.colPKID.Width = 50;
            // 
            // colMachine
            // 
            this.colMachine.Editable = false;
            this.colMachine.Text = "Machine";
            this.colMachine.Width = 150;
            // 
            // colStatus
            // 
            this.colStatus.Editable = false;
            this.colStatus.Text = "Status";
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
            this.tabControlAdv1.Size = new System.Drawing.Size(499, 317);
            this.tabControlAdv1.TabGap = 20;
            this.tabControlAdv1.TabIndex = 4;
            this.tabControlAdv1.TabStyle = typeof(Syncfusion.Windows.Forms.Tools.TabRendererDockingWhidbey);
            this.tabControlAdv1.ThemesEnabled = true;
            this.tabControlAdv1.VerticalAlignment = Syncfusion.Windows.Forms.Tools.TabVerticalAlignment.Top;
            this.tabControlAdv1.SelectedIndexChanged += new System.EventHandler(tabControlAdv1_SelectedIndexChanged);
            // 
            // tabPageBrowse
            // 
            this.tabPageBrowse.Controls.Add(this.objectTable);
            this.tabPageBrowse.Location = new System.Drawing.Point(1, 1);
            this.tabPageBrowse.Name = "tabPageBrowse";
            this.tabPageBrowse.Size = new System.Drawing.Size(496, 294);
            this.tabPageBrowse.TabIndex = 0;
            this.tabPageBrowse.Text = "Search and Select";
            this.tabPageBrowse.ThemesEnabled = true;
            // 
            // objectTable
            // 
            this.objectTable.ColumnModel = this.columnModel1;
            this.objectTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTable.Location = new System.Drawing.Point(0, 0);
            this.objectTable.Name = "objectTable";
            this.objectTable.Size = new System.Drawing.Size(496, 294);
            this.objectTable.TabIndex = 4;
            this.objectTable.TableModel = this.tableModel1;
            this.objectTable.Text = "table1";
            this.objectTable.CellCheckChanged += new XPTable.Events.CellCheckBoxEventHandler(this.objectTable_CellCheckChanged);
            this.objectTable.CellPropertyChanged += new XPTable.Events.CellEventHandler(this.objectTable_CellPropertyChanged);
            this.objectTable.CellButtonClicked += new XPTable.Events.CellButtonEventHandler(this.objectTable_CellButtonClicked);
            // 
            // tabPageSelection
            // 
            this.tabPageSelection.Controls.Add(this.tableSelection);
            this.tabPageSelection.Location = new System.Drawing.Point(1, 1);
            this.tabPageSelection.Name = "tabPageSelection";
            this.tabPageSelection.Size = new System.Drawing.Size(496, 294);
            this.tabPageSelection.TabIndex = 1;
            this.tabPageSelection.Text = "Current Selection";
            this.tabPageSelection.ThemesEnabled = true;
            // 
            // tableSelection
            // 
            this.tableSelection.ColumnModel = this.columnModel1;
            this.tableSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableSelection.Location = new System.Drawing.Point(0, 0);
            this.tableSelection.Name = "tableSelection";
            this.tableSelection.Size = new System.Drawing.Size(496, 294);
            this.tableSelection.TabIndex = 1;
            this.tableSelection.TableModel = this.tableModelSelection;
            this.tableSelection.Text = "table2";
            this.tableSelection.CellPropertyChanged += new XPTable.Events.CellEventHandler(this.tableSelection_CellPropertyChanged);
            // 
            // ObjectList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.tabControlAdv1);
            this.Location = new System.Drawing.Point(50, 50);
            this.Name = "ObjectList";
            this.Size = new System.Drawing.Size(499, 317);
            ((System.ComponentModel.ISupportInitialize)(this.tabControlAdv1)).EndInit();
            this.tabControlAdv1.ResumeLayout(false);
            this.tabPageBrowse.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.objectTable)).EndInit();
            this.tabPageSelection.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tableSelection)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private XPTable.Models.ColumnModel columnModel1;
        private XPTable.Models.TableModel tableModel1;
        private Syncfusion.Windows.Forms.Tools.TabControlAdv tabControlAdv1;
        private Syncfusion.Windows.Forms.Tools.TabPageAdv tabPageBrowse;
        private Syncfusion.Windows.Forms.Tools.TabPageAdv tabPageSelection;
        private XPTable.Models.Table tableSelection;
        private XPTable.Models.Table objectTable;
        private XPTable.Models.TableModel tableModelSelection;
        private XPTable.Models.CheckBoxColumn colSelected;
        private XPTable.Models.TextColumn colObject;
        private XPTable.Models.ButtonColumn colMoreInfo;
        private XPTable.Models.TextColumn colWorkspace;
        private XPTable.Models.TextColumn colPKID;
        private XPTable.Models.TextColumn colMachine;
        private XPTable.Models.TextColumn colClass;
        private XPTable.Models.TextColumn colStatus;
    }
}
