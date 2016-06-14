namespace MetaBuilder.UIControls.Dialogs
{
    partial class RefreshData
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
            this.table1 = new XPTable.Models.Table();
            this.columnModel1 = new XPTable.Models.ColumnModel();
            this.checkBoxColumn1 = new XPTable.Models.CheckBoxColumn();
            this.textColumn3 = new XPTable.Models.TextColumn();
            this.textColumn4 = new XPTable.Models.TextColumn();
            this.textColumn2 = new XPTable.Models.TextColumn();
            this.tableModel1 = new XPTable.Models.TableModel();
            this.table2 = new XPTable.Models.Table();
            this.tableModelDB = new XPTable.Models.TableModel();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.propertyGrid2 = new System.Windows.Forms.PropertyGrid();
            this.lblObjectsOnDiagram = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new MetaBuilder.MetaControls.MetaButton();
            this.btnOK = new MetaBuilder.MetaControls.MetaButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSelectAll2 = new MetaBuilder.MetaControls.MetaButton();
            this.button1 = new MetaBuilder.MetaControls.MetaButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.metaButtonExport = new MetaBuilder.MetaControls.MetaButton();
            ((System.ComponentModel.ISupportInitialize)(this.table1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table2)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // table1
            // 
            this.table1.ColumnModel = this.columnModel1;
            this.table1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table1.EditStartAction = XPTable.Editors.EditStartAction.CustomKey;
            this.table1.EnableHeaderContextMenu = false;
            this.table1.FullRowSelect = true;
            this.table1.Location = new System.Drawing.Point(3, 23);
            this.table1.Name = "table1";
            this.table1.Size = new System.Drawing.Size(479, 187);
            this.table1.TabIndex = 0;
            this.table1.TableModel = this.tableModel1;
            this.table1.Text = "table1";
            this.table1.UnfocusedSelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.table1.UnfocusedSelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.table1.CellMouseUp += new XPTable.Events.CellMouseEventHandler(this.table1_CellMouseUp);
            this.table1.SelectionChanged += new XPTable.Events.SelectionEventHandler(this.table1_SelectionChanged);
            // 
            // columnModel1
            // 
            this.columnModel1.Columns.AddRange(new XPTable.Models.Column[] {
            this.checkBoxColumn1,
            this.textColumn3,
            this.textColumn4,
            this.textColumn2});
            // 
            // checkBoxColumn1
            // 
            this.checkBoxColumn1.Width = 20;
            // 
            // textColumn3
            // 
            this.textColumn3.Text = "Value";
            this.textColumn3.Width = 225;
            // 
            // textColumn4
            // 
            this.textColumn4.Text = "Class";
            this.textColumn4.Width = 100;
            // 
            // textColumn2
            // 
            this.textColumn2.Text = "Workspace";
            this.textColumn2.Width = 100;
            // 
            // table2
            // 
            this.table2.ColumnModel = this.columnModel1;
            this.table2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table2.EditStartAction = XPTable.Editors.EditStartAction.CustomKey;
            this.table2.EnableHeaderContextMenu = false;
            this.table2.FullRowSelect = true;
            this.table2.Location = new System.Drawing.Point(488, 23);
            this.table2.Name = "table2";
            this.table2.Size = new System.Drawing.Size(479, 187);
            this.table2.TabIndex = 0;
            this.table2.TableModel = this.tableModelDB;
            this.table2.Text = "table1";
            this.table2.UnfocusedSelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.table2.UnfocusedSelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.table2.CellMouseUp += new XPTable.Events.CellMouseEventHandler(this.table2_CellMouseUp);
            this.table2.SelectionChanged += new XPTable.Events.SelectionEventHandler(this.table2_SelectionChanged);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.Location = new System.Drawing.Point(3, 239);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(479, 155);
            this.propertyGrid1.TabIndex = 1;
            this.propertyGrid1.ToolbarVisible = false;
            this.propertyGrid1.DoubleClick += new System.EventHandler(this.propertyGrid1_DoubleClick);
            // 
            // propertyGrid2
            // 
            this.propertyGrid2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid2.HelpVisible = false;
            this.propertyGrid2.Location = new System.Drawing.Point(488, 239);
            this.propertyGrid2.Name = "propertyGrid2";
            this.propertyGrid2.Size = new System.Drawing.Size(479, 155);
            this.propertyGrid2.TabIndex = 1;
            this.propertyGrid2.ToolbarVisible = false;
            this.propertyGrid2.DoubleClick += new System.EventHandler(this.propertyGrid2_DoubleClick);
            // 
            // lblObjectsOnDiagram
            // 
            this.lblObjectsOnDiagram.AutoSize = true;
            this.lblObjectsOnDiagram.Location = new System.Drawing.Point(3, 0);
            this.lblObjectsOnDiagram.Name = "lblObjectsOnDiagram";
            this.lblObjectsOnDiagram.Size = new System.Drawing.Size(85, 13);
            this.lblObjectsOnDiagram.TabIndex = 2;
            this.lblObjectsOnDiagram.Text = "Diagram Objects";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(488, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Database Objects";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.CornerRadius = new Ascend.CornerRadius(2);
            this.btnCancel.GradientLowColor = System.Drawing.Color.DarkGray;
            this.btnCancel.Location = new System.Drawing.Point(899, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.StayActiveAfterClick = false;
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.CornerRadius = new Ascend.CornerRadius(2);
            this.btnOK.GradientLowColor = System.Drawing.Color.DarkGray;
            this.btnOK.Location = new System.Drawing.Point(814, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.StayActiveAfterClick = false;
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btnSelectAll2, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.propertyGrid1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.propertyGrid2, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.table1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.table2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.button1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblObjectsOnDiagram, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 89.33823F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.66177F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(970, 397);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // btnSelectAll2
            // 
            this.btnSelectAll2.CornerRadius = new Ascend.CornerRadius(2);
            this.btnSelectAll2.GradientLowColor = System.Drawing.Color.DarkGray;
            this.btnSelectAll2.Location = new System.Drawing.Point(488, 216);
            this.btnSelectAll2.Name = "btnSelectAll2";
            this.btnSelectAll2.Size = new System.Drawing.Size(75, 17);
            this.btnSelectAll2.StayActiveAfterClick = false;
            this.btnSelectAll2.TabIndex = 4;
            this.btnSelectAll2.Text = "Select All";
            this.btnSelectAll2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnSelectAll2.Click += new System.EventHandler(this.btnSelectAll2_Click);
            // 
            // button1
            // 
            this.button1.CornerRadius = new Ascend.CornerRadius(2);
            this.button1.GradientLowColor = System.Drawing.Color.DarkGray;
            this.button1.Location = new System.Drawing.Point(3, 216);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 17);
            this.button1.StayActiveAfterClick = false;
            this.button1.TabIndex = 3;
            this.button1.Text = "Select All";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Diagram Objects";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 144);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(405, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Double Click a Meta Property to copy individual properties from one side to the o" +
                "ther.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 175);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Not in DB";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(984, 429);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(976, 403);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Refresh";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.label13);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(976, 403);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Hints";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 83);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(275, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Double Click to scroll to the object\'s area on the diagram.";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(6, 13);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(48, 13);
            this.label10.TabIndex = 7;
            this.label10.Text = "Default";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 45);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(525, 13);
            this.label12.TabIndex = 7;
            this.label12.Text = "You can also cancel this screen and hit F5 to automatically synchronise the diagr" +
                "am\'s objects to the database.";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 29);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(384, 13);
            this.label11.TabIndex = 7;
            this.label11.Text = "By default, objects will default to the saved (database) versions when refreshing" +
                ".";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 233);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(449, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "Selecting these items (left side) will prevent the database items from appearing " +
                "on the diagram.";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(6, 217);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Not on Diagram";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 192);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(362, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Objects that are not in the database will only be stored when saving the file.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 111);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Meta Properties";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 127);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(344, 13);
            this.label13.TabIndex = 7;
            this.label13.Text = "Select the Item Value to display the associated properties at the bottom.";
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.metaButtonExport);
            this.panelButtons.Controls.Add(this.btnOK);
            this.panelButtons.Controls.Add(this.btnCancel);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(0, 429);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(984, 33);
            this.panelButtons.TabIndex = 9;
            // 
            // metaButtonExport
            // 
            this.metaButtonExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.metaButtonExport.CornerRadius = new Ascend.CornerRadius(2);
            this.metaButtonExport.GradientLowColor = System.Drawing.Color.DarkGray;
            this.metaButtonExport.Location = new System.Drawing.Point(7, 5);
            this.metaButtonExport.Name = "metaButtonExport";
            this.metaButtonExport.Size = new System.Drawing.Size(75, 23);
            this.metaButtonExport.StayActiveAfterClick = false;
            this.metaButtonExport.TabIndex = 4;
            this.metaButtonExport.Text = "Export...";
            this.metaButtonExport.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.metaButtonExport.Click += new System.EventHandler(this.metaButtonExport_Click);
            // 
            // RefreshData
            // 
            this.ClientSize = new System.Drawing.Size(984, 462);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panelButtons);
            this.Name = "RefreshData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Refresh Data - Select Latest Version(s)";
            ((System.ComponentModel.ISupportInitialize)(this.table1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table2)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private XPTable.Models.Table table1;
        private XPTable.Models.Table table2;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.PropertyGrid propertyGrid2;
        private System.Windows.Forms.Label lblObjectsOnDiagram;
        private System.Windows.Forms.Label label1;
        private MetaControls.MetaButton btnCancel;
        private MetaControls.MetaButton btnOK;
        private XPTable.Models.ColumnModel columnModel1;
        private XPTable.Models.TableModel tableModel1;
        private XPTable.Models.TextColumn textColumn3;
        private XPTable.Models.TextColumn textColumn4;
        private XPTable.Models.TextColumn textColumn2;
        private XPTable.Models.TableModel tableModelDB;
        private XPTable.Models.CheckBoxColumn checkBoxColumn1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private MetaControls.MetaButton btnSelectAll2;
        private MetaControls.MetaButton button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel panelButtons;
        private MetaBuilder.MetaControls.MetaButton metaButtonExport;
    }
}