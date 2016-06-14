namespace MetaBuilder.UIControls.GraphingUI.Tools.Explorer
{
    partial class MetaObjectExplorer
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
            this.components = new System.ComponentModel.Container();
            MetaBuilder.UIControls.Common.CheckBoxComboBox.CheckBoxProperties checkBoxProperties1 = new MetaBuilder.UIControls.Common.CheckBoxComboBox.CheckBoxProperties();
            MetaBuilder.UIControls.Common.CheckBoxComboBox.CheckBoxProperties checkBoxProperties2 = new MetaBuilder.UIControls.Common.CheckBoxComboBox.CheckBoxProperties();
            MetaBuilder.UIControls.Common.CheckBoxComboBox.CheckBoxProperties checkBoxProperties3 = new MetaBuilder.UIControls.Common.CheckBoxComboBox.CheckBoxProperties();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MetaObjectExplorer));
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.contextMenuStripObjects = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewInContextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serverViewInContextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.statusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noneToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRefresh = new MetaControls.MetaButton();
            this.btnReset = new MetaControls.MetaButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.comboWorkspace = new MetaBuilder.UIControls.Common.CheckBoxComboBox.CheckBoxComboBox();
            this.comboClass = new MetaBuilder.UIControls.Common.CheckBoxComboBox.CheckBoxComboBox();
            this.comboDiagram = new MetaBuilder.UIControls.Common.CheckBoxComboBox.CheckBoxComboBox();
            this.labelMessage = new System.Windows.Forms.Label();
            this.columnHeaderName = new System.Windows.Forms.ColumnHeader();
            this.listViewItems = new MetaBuilder.UIControls.GraphingUI.Tools.Explorer.DragAndDropMultiSelectTreeListView();
            this.contextMenuStripObjects.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Search";
            // 
            // textBoxName
            // 
            this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxName.Location = new System.Drawing.Point(45, 28);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(162, 20);
            this.textBoxName.TabIndex = 1;
            this.textBoxName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxName_KeyDown);
            // 
            // contextMenuStripObjects
            // 
            this.contextMenuStripObjects.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewInContextToolStripMenuItem,
            this.serverViewInContextToolStripMenuItem});
            this.contextMenuStripObjects.Name = "contextMenuStripObjects";
            this.contextMenuStripObjects.Size = new System.Drawing.Size(157, 26);
            this.contextMenuStripObjects.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripObjects_Opening);
            // 
            // viewInContextToolStripMenuItem
            // 
            this.viewInContextToolStripMenuItem.Name = "viewInContextToolStripMenuItem";
            this.viewInContextToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.viewInContextToolStripMenuItem.Text = "View In Context";
            this.viewInContextToolStripMenuItem.Click += new System.EventHandler(this.viewInContextToolStripMenuItem_Click);
            // 
            // serverviewInContextToolStripMenuItem
            // 
            this.serverViewInContextToolStripMenuItem.Name = "viewInContextToolStripMenuItem";
            this.serverViewInContextToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.serverViewInContextToolStripMenuItem.Text = "View In Context (Server)";
            this.serverViewInContextToolStripMenuItem.Visible = false;
            this.serverViewInContextToolStripMenuItem.Click += new System.EventHandler(this.viewInContextToolStripMenuItem_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 504);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(210, 13);
            this.progressBar1.TabIndex = 3;
            // 
            // statusToolStripMenuItem
            // 
            this.statusToolStripMenuItem.Name = "statusToolStripMenuItem";
            this.statusToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // noneToolStripMenuItem2
            // 
            this.noneToolStripMenuItem2.Name = "noneToolStripMenuItem2";
            this.noneToolStripMenuItem2.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(6, 6);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.btnReset);
            this.panel1.Controls.Add(this.textBoxName);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(210, 79);
            this.panel1.TabIndex = 5;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Location = new System.Drawing.Point(73, 54);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(64, 20);
            this.btnRefresh.StayActiveAfterClick = false;
            this.btnRefresh.TabIndex = 18;
            this.btnRefresh.Text = "Find";
            this.btnRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Location = new System.Drawing.Point(143, 54);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(64, 20);
            this.btnReset.StayActiveAfterClick = false;
            this.btnReset.TabIndex = 18;
            this.btnReset.Text = "Clear Filter";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.comboWorkspace, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.comboClass, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.comboDiagram, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(210, 25);
            this.tableLayoutPanel1.TabIndex = 19;
            // 
            // comboWorkspace
            // 
            checkBoxProperties1.Appearance = System.Windows.Forms.Appearance.Button;
            checkBoxProperties1.FlatAppearanceBorderColor = System.Drawing.Color.White;
            checkBoxProperties1.FlatAppearanceCheckedBackColor = System.Drawing.SystemColors.Highlight;
            checkBoxProperties1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            checkBoxProperties1.ForeColor = System.Drawing.SystemColors.ControlText;
            checkBoxProperties1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.comboWorkspace.CheckBoxProperties = checkBoxProperties1;
            this.comboWorkspace.DisplayMemberSingleItem = "";
            this.comboWorkspace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboWorkspace.FormattingEnabled = true;
            this.comboWorkspace.Location = new System.Drawing.Point(3, 3);
            this.comboWorkspace.MaxDropDownItems = 10;
            this.comboWorkspace.Name = "comboWorkspace";
            this.comboWorkspace.Size = new System.Drawing.Size(78, 21);
            this.comboWorkspace.TabIndex = 7;
            this.comboWorkspace.Click += new System.EventHandler(this.comboWorkspace_Click);
            // 
            // comboClass
            // 
            checkBoxProperties2.Appearance = System.Windows.Forms.Appearance.Button;
            checkBoxProperties2.FlatAppearanceBorderColor = System.Drawing.Color.White;
            checkBoxProperties2.FlatAppearanceCheckedBackColor = System.Drawing.SystemColors.Highlight;
            checkBoxProperties2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            checkBoxProperties2.ForeColor = System.Drawing.SystemColors.ControlText;
            checkBoxProperties2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.comboClass.CheckBoxProperties = checkBoxProperties2;
            this.comboClass.DisplayMemberSingleItem = "";
            this.comboClass.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboClass.FormattingEnabled = true;
            this.comboClass.Location = new System.Drawing.Point(150, 3);
            this.comboClass.MaxDropDownItems = 10;
            this.comboClass.Name = "comboClass";
            this.comboClass.Size = new System.Drawing.Size(57, 21);
            this.comboClass.TabIndex = 7;
            this.comboClass.Click += new System.EventHandler(this.comboWorkspace_Click);
            // 
            // comboDiagram
            // 
            checkBoxProperties3.Appearance = System.Windows.Forms.Appearance.Button;
            checkBoxProperties3.FlatAppearanceBorderColor = System.Drawing.Color.White;
            checkBoxProperties3.FlatAppearanceCheckedBackColor = System.Drawing.SystemColors.Highlight;
            checkBoxProperties3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            checkBoxProperties3.ForeColor = System.Drawing.SystemColors.ControlText;
            checkBoxProperties3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.comboDiagram.CheckBoxProperties = checkBoxProperties3;
            this.comboDiagram.DisplayMemberSingleItem = "";
            this.comboDiagram.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboDiagram.FormattingEnabled = true;
            this.comboDiagram.Location = new System.Drawing.Point(87, 3);
            this.comboDiagram.MaxDropDownItems = 10;
            this.comboDiagram.Name = "comboDiagram";
            this.comboDiagram.Size = new System.Drawing.Size(57, 21);
            this.comboDiagram.TabIndex = 7;
            this.comboDiagram.Click += new System.EventHandler(this.comboWorkspace_Click);
            // 
            // labelMessage
            // 
            this.labelMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMessage.Location = new System.Drawing.Point(0, 503);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(210, 1);
            this.labelMessage.TabIndex = 6;
            this.labelMessage.Text = "Initialising";
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelMessage.Visible = false;
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            this.columnHeaderName.Width = 246;
            // 
            // listViewItems
            // 
            this.listViewItems.AllowDrop = true;
            this.listViewItems.AllowReorder = false;
            this.listViewItems.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listViewItems.ContextMenuStrip = this.contextMenuStripObjects;
            this.listViewItems.Cursor = System.Windows.Forms.Cursors.Default;
            this.listViewItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewItems.LineColor = System.Drawing.SystemColors.Desktop;
            this.listViewItems.Location = new System.Drawing.Point(0, 79);
            this.listViewItems.Name = "listViewItems";
            this.listViewItems.SelectedNodes = ((System.Collections.Generic.List<System.Windows.Forms.TreeNode>)(resources.GetObject("listViewItems.SelectedNodes")));
            this.listViewItems.ShowNodeToolTips = true;
            this.listViewItems.Size = new System.Drawing.Size(210, 424);
            this.listViewItems.TabIndex = 2;
            this.listViewItems.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.listViewItems_NodeMouseDoubleClick);
            // 
            // MetaObjectExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(210, 517);
            this.ControlBox = false;
            this.Controls.Add(this.listViewItems);
            this.Controls.Add(this.labelMessage);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MetaObjectExplorer";
            this.ShowInTaskbar = false;
            this.Text = "Object Explorer";
            this.Load += new System.EventHandler(this.MetaObjectExplorer_Load);
            this.contextMenuStripObjects.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxName;
        private DragAndDropMultiSelectTreeListView listViewItems;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ToolStripMenuItem statusToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem noneToolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripObjects;
        private System.Windows.Forms.ToolStripMenuItem viewInContextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem serverViewInContextToolStripMenuItem;
        private MetaControls.MetaButton btnReset;
        private MetaControls.MetaButton btnRefresh;
        private MetaBuilder.UIControls.Common.CheckBoxComboBox.CheckBoxComboBox comboClass;
        private MetaBuilder.UIControls.Common.CheckBoxComboBox.CheckBoxComboBox comboWorkspace;
        private MetaBuilder.UIControls.Common.CheckBoxComboBox.CheckBoxComboBox comboDiagram;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}