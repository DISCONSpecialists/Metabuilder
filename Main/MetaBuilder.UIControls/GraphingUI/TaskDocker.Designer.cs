namespace MetaBuilder.UIControls.GraphingUI
{
    partial class TaskDocker
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
            this.listView1 = new BrightIdeasSoftware.ObjectListView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.AllowColumnReorder = false;
            this.listView1.AllowDrop = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.TabIndex = 0;
            BrightIdeasSoftware.OLVColumn column1 = new BrightIdeasSoftware.OLVColumn("Task", "Caption");
            column1.IsVisible = true;
            column1.IsEditable = false;
            column1.FillsFreeSpace = true;
            this.listView1.AllColumns.Add(column1);
            this.listView1.Columns.Add(column1);
            this.listView1.EmptyListMsg = "No tasks";
            this.listView1.UseTranslucentSelection = true;
            this.listView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.listView1.FullRowSelect = true;
            this.listView1.GroupWithItemCountFormat = "{0} ({1} task)";
            this.listView1.GroupWithItemCountSingularFormat = "{0} ({1} task)";
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.HideSelection = false;
            this.listView1.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
            //this.listView1.OverlayText.Alignment = System.Drawing.ContentAlignment.BottomLeft;
            //this.listView1.OverlayText.Text = "";
            this.listView1.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.Submenu;
            this.listView1.ShowCommandMenuOnRightClick = true;
            this.listView1.ShowGroups = false;
            this.listView1.ShowImagesOnSubItems = true;
            this.listView1.ShowItemToolTips = true;
            this.listView1.TabIndex = 8;
            this.listView1.UseAlternatingBackColors = true;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.Scrollable = true;
            
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox1.Location = new System.Drawing.Point(0, 220);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(292, 46);
            this.textBox1.TabIndex = 1;
            // 
            // TaskDocker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.textBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Name = "TaskDocker";
            this.TabText = "Tasks";
            this.Text = "Tasks";
            this.Load += new System.EventHandler(this.TaskDocker_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView listView1;
        private System.Windows.Forms.TextBox textBox1;

    }
}