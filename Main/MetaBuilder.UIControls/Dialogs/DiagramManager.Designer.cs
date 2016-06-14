namespace MetaBuilder.UIControls.Dialogs
{
    partial class DiagramManager
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
            //System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer treeListViewItemCollectionComparer1 = new System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer();
            //System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiagramManager));
            this.btnOpen = new MetaControls.MetaButton();
            this.btnDeleteSelectedDiagrams = new MetaControls.MetaButton();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.treeListView1 = new System.Windows.Forms.TreeListView();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.colVersion = new System.Windows.Forms.ColumnHeader();
            this.colDate = new System.Windows.Forms.ColumnHeader();
            this.colTime = new System.Windows.Forms.ColumnHeader();
            this.colVCStatus = new System.Windows.Forms.ColumnHeader();
            this.colWS = new System.Windows.Forms.ColumnHeader();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // btnOpen
            // 
            this.btnOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOpen.Location = new System.Drawing.Point(10, 7);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(150, 23);
            this.btnOpen.TabIndex = 3;
            this.btnOpen.Text = "Open Selected Diagrams";
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click_1);
            // 
            // btnDeleteSelectedDiagrams
            // 
            this.btnDeleteSelectedDiagrams.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteSelectedDiagrams.Location = new System.Drawing.Point(170, 7);
            this.btnDeleteSelectedDiagrams.Name = "btnDeleteSelectedDiagrams";
            this.btnDeleteSelectedDiagrams.Size = new System.Drawing.Size(150, 23);
            this.btnDeleteSelectedDiagrams.TabIndex = 3;
            this.btnDeleteSelectedDiagrams.Text = "Delete Selected Diagrams";
            this.btnDeleteSelectedDiagrams.Click += new System.EventHandler(this.btnDeleteSelectedDiagrams_Click);
            //
            // panelButtons
            //
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Height = 37;
            this.panelButtons.Controls.Add(btnOpen);
            this.panelButtons.Controls.Add(btnDeleteSelectedDiagrams);
            // 
            // treeListView1
            // 
            this.treeListView1.CheckBoxes = System.Windows.Forms.CheckBoxesTypes.Simple;
            this.treeListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colVersion,
            this.colDate,
            this.colTime,
            this.colWS,
            this.colVCStatus});
            //treeListViewItemCollectionComparer1.Column = 0;
            //treeListViewItemCollectionComparer1.SortOrder = System.Windows.Forms.SortOrder.Ascending;
            //this.treeListView1.Comparer = treeListViewItemCollectionComparer1;
            this.treeListView1.FullRowSelect = false;
            //listViewGroup1.Header = "ListViewGroup";
            //listViewGroup1.Name = "listViewGroup1";
            //this.treeListView1.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            //listViewGroup1});
            this.treeListView1.Location = new System.Drawing.Point(0, 1);
            this.treeListView1.Name = "treeListView1";
            this.treeListView1.ShowGroups = false;
            //this.treeListView1.Size = new System.Drawing.Size(558, 261);
            this.treeListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListView1.SmallImageList = this.imageList1;
            this.treeListView1.TabIndex = 4;
            //this.treeListView1.UseCompatibleStateImageBehavior = false;
            this.treeListView1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(treeListView1_ItemCheck);
            // 
            // colName
            // 
            this.colName.Text = "Diagram";
            this.colName.Width = 250;
            // 
            // colVersion
            // 
            this.colVersion.Text = "Version";
            this.colVersion.Width = 50;
            // 
            // colDate
            // 
            this.colDate.Text = "Date";
            //this.colDate.Width = 75;
            // 
            // colTime
            // 
            this.colTime.Text = "Time";
            //this.colTime.Width = 75;
            // 
            // colVCStatus
            // 
            this.colVCStatus.Text = "Version Control Status";
            this.colVCStatus.Width = 120;
            //
            // colWS
            //
            this.colWS.Text = "Workspace";
            this.colWS.Width = 120;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "MetaBuilderWithoutBorder.ico");
            // 
            // DiagramManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Size = new System.Drawing.Size(700, 300);
            this.MinimumSize = ClientSize;
            this.Controls.Add(this.treeListView1);
            this.Controls.Add(this.panelButtons);
            this.Name = "DiagramManager";
            this.Text = "Diagram Manager";
            this.Load += new System.EventHandler(this.DiagramManager_Load);
            this.ResumeLayout(false);

        }


        #endregion

        private MetaControls.MetaButton btnOpen;
        private MetaControls.MetaButton btnDeleteSelectedDiagrams;
        private System.Windows.Forms.Panel panelButtons;

        private System.Windows.Forms.TreeListView treeListView1;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colVersion;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ColumnHeader colDate;
        private System.Windows.Forms.ColumnHeader colTime;
        private System.Windows.Forms.ColumnHeader colVCStatus;
        private System.Windows.Forms.ColumnHeader colWS;
    }
}