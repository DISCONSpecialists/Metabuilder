namespace MetaBuilder.UIControls.GraphingUI
{
    partial class WorkspaceManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkspaceManager));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cbIncludeEmbeddedObjects = new System.Windows.Forms.CheckBox();
            this.treeSource = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.treeTarget = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelMessage = new System.Windows.Forms.Label();
            this.btnClose = new MetaBuilder.MetaControls.MetaButton();
            this.btnApply = new MetaBuilder.MetaControls.MetaButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.listSource = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.listTarget = new System.Windows.Forms.ListView();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonClearTargets = new MetaBuilder.MetaControls.MetaButton();
            this.btnAddItemsToTransfer = new MetaBuilder.MetaControls.MetaButton();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.buttonSelectInvert = new MetaBuilder.MetaControls.MetaButton();
            this.buttonSelectNone = new MetaBuilder.MetaControls.MetaButton();
            this.buttonSelectAll = new MetaBuilder.MetaControls.MetaButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.66667F));
            this.tableLayoutPanel1.Controls.Add(this.cbIncludeEmbeddedObjects, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.treeSource, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.treeTarget, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.listSource, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.listTarget, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 199F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 209F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(634, 475);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // cbIncludeEmbeddedObjects
            // 
            this.cbIncludeEmbeddedObjects.AutoSize = true;
            this.cbIncludeEmbeddedObjects.Checked = true;
            this.cbIncludeEmbeddedObjects.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIncludeEmbeddedObjects.Location = new System.Drawing.Point(3, 449);
            this.cbIncludeEmbeddedObjects.Name = "cbIncludeEmbeddedObjects";
            this.cbIncludeEmbeddedObjects.Size = new System.Drawing.Size(154, 17);
            this.cbIncludeEmbeddedObjects.TabIndex = 1;
            this.cbIncludeEmbeddedObjects.Text = "Include Embedded Objects";
            this.cbIncludeEmbeddedObjects.UseVisualStyleBackColor = true;
            // 
            // treeSource
            // 
            this.treeSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeSource.HideSelection = false;
            this.treeSource.Location = new System.Drawing.Point(3, 17);
            this.treeSource.Name = "treeSource";
            this.treeSource.Size = new System.Drawing.Size(180, 193);
            this.treeSource.StateImageList = this.imageList1;
            this.treeSource.TabIndex = 0;
            this.treeSource.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeSource_AfterSelect);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "gnome-lockscreen.png");
            this.imageList1.Images.SetKeyName(1, "display.png");
            // 
            // treeTarget
            // 
            this.treeTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeTarget.HideSelection = false;
            this.treeTarget.Location = new System.Drawing.Point(3, 240);
            this.treeTarget.Name = "treeTarget";
            this.treeTarget.Size = new System.Drawing.Size(180, 203);
            this.treeTarget.StateImageList = this.imageList1;
            this.treeTarget.TabIndex = 5;
            this.treeTarget.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeTarget_NodeMouseDoubleClick);
            this.treeTarget.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeTarget_AfterSelect);
            this.treeTarget.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeTarget_BeforeSelect);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelMessage);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnApply);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(264, 449);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(367, 23);
            this.panel1.TabIndex = 6;
            // 
            // labelMessage
            // 
            this.labelMessage.AutoSize = true;
            this.labelMessage.Location = new System.Drawing.Point(4, 4);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(83, 13);
            this.labelMessage.TabIndex = 1;
            this.labelMessage.Text = "Fetching Data...";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(291, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Cancel";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(215, 0);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 0;
            this.btnApply.Text = "Transfer";
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Select Source Workspace";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(264, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Select Items";
            // 
            // listSource
            // 
            this.listSource.AllowDrop = true;
            this.listSource.AutoArrange = false;
            this.listSource.CheckBoxes = true;
            this.listSource.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listSource.HideSelection = false;
            this.listSource.Location = new System.Drawing.Point(264, 17);
            this.listSource.Name = "listSource";
            this.listSource.Size = new System.Drawing.Size(367, 193);
            this.listSource.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listSource.TabIndex = 2;
            this.listSource.UseCompatibleStateImageBehavior = false;
            this.listSource.View = System.Windows.Forms.View.List;
            this.listSource.SelectedIndexChanged += new System.EventHandler(this.listSource_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Items";
            this.columnHeader1.Width = 387;
            // 
            // listTarget
            // 
            this.listTarget.AllowDrop = true;
            this.listTarget.AutoArrange = false;
            this.listTarget.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.listTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listTarget.Location = new System.Drawing.Point(264, 240);
            this.listTarget.Name = "listTarget";
            this.listTarget.Size = new System.Drawing.Size(367, 203);
            this.listTarget.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listTarget.TabIndex = 4;
            this.listTarget.UseCompatibleStateImageBehavior = false;
            this.listTarget.View = System.Windows.Forms.View.List;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Items";
            this.columnHeader2.Width = 392;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 213);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Select Target Workspace";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonClearTargets);
            this.panel2.Controls.Add(this.btnAddItemsToTransfer);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(189, 240);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(69, 203);
            this.panel2.TabIndex = 12;
            // 
            // buttonClearTargets
            // 
            this.buttonClearTargets.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonClearTargets.Location = new System.Drawing.Point(0, 58);
            this.buttonClearTargets.Name = "buttonClearTargets";
            this.buttonClearTargets.Size = new System.Drawing.Size(69, 58);
            this.buttonClearTargets.TabIndex = 12;
            this.buttonClearTargets.Text = "Clear\r\nTransfer\r\nList";
            this.buttonClearTargets.Click += new System.EventHandler(this.buttonClearTargets_Click);
            // 
            // btnAddItemsToTransfer
            // 
            this.btnAddItemsToTransfer.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAddItemsToTransfer.Location = new System.Drawing.Point(0, 0);
            this.btnAddItemsToTransfer.Name = "btnAddItemsToTransfer";
            this.btnAddItemsToTransfer.Size = new System.Drawing.Size(69, 58);
            this.btnAddItemsToTransfer.TabIndex = 11;
            this.btnAddItemsToTransfer.Text = "Add\r\nItems to\r\nTransfer\r\nList";
            this.btnAddItemsToTransfer.Click += new System.EventHandler(this.btnAddItemsToTransfer_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(264, 213);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "List of Items to Transfer";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.buttonSelectInvert);
            this.panel3.Controls.Add(this.buttonSelectNone);
            this.panel3.Controls.Add(this.buttonSelectAll);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(189, 17);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(69, 193);
            this.panel3.TabIndex = 13;
            // 
            // buttonSelectInvert
            // 
            this.buttonSelectInvert.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonSelectInvert.Location = new System.Drawing.Point(0, 116);
            this.buttonSelectInvert.Name = "buttonSelectInvert";
            this.buttonSelectInvert.Size = new System.Drawing.Size(69, 58);
            this.buttonSelectInvert.TabIndex = 2;
            this.buttonSelectInvert.Text = "Invert\r\nSelection";
            this.buttonSelectInvert.Click += new System.EventHandler(this.buttonSelectInvert_Click);
            // 
            // buttonSelectNone
            // 
            this.buttonSelectNone.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonSelectNone.Location = new System.Drawing.Point(0, 58);
            this.buttonSelectNone.Name = "buttonSelectNone";
            this.buttonSelectNone.Size = new System.Drawing.Size(69, 58);
            this.buttonSelectNone.TabIndex = 1;
            this.buttonSelectNone.Text = "De-Select\r\nAll\r\nItems";
            this.buttonSelectNone.Click += new System.EventHandler(this.buttonSelectNone_Click);
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonSelectAll.Location = new System.Drawing.Point(0, 0);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(69, 58);
            this.buttonSelectAll.TabIndex = 0;
            this.buttonSelectAll.Text = "Select\r\nAll\r\nItems";
            this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelectAll_Click);
            // 
            // WorkspaceManager
            // 
            this.AllowDrop = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(634, 475);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "WorkspaceManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Workspace Manager";
            this.Load += new System.EventHandler(this.WorkspaceManager_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TreeView treeSource;
        private System.Windows.Forms.ListView listSource;
        private System.Windows.Forms.ListView listTarget;
        private System.Windows.Forms.TreeView treeTarget;
        private System.Windows.Forms.Panel panel1;
        private MetaControls.MetaButton btnClose;
        private MetaControls.MetaButton btnApply;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.CheckBox cbIncludeEmbeddedObjects;
        private MetaControls.MetaButton btnAddItemsToTransfer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private MetaControls.MetaButton buttonClearTargets;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.Panel panel3;
        private MetaControls.MetaButton buttonSelectInvert;
        private MetaControls.MetaButton buttonSelectNone;
        private MetaControls.MetaButton buttonSelectAll;
        private System.Windows.Forms.ImageList imageList1;

    }
}