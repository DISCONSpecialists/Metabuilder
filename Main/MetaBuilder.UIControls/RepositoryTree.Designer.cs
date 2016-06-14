namespace MetaBuilder.UIControls
{
    partial class RepositoryTree
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Activity");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("... can expand to child objects");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Maintain System", new System.Windows.Forms.TreeNode[] {
            treeNode2});
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Repair System");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Function", new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode4});
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("OSD - ACME Company - To-Be", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Diagrams", new System.Windows.Forms.TreeNode[] {
            treeNode6});
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Plan System");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Design Solution");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Implement Solution");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Function", new System.Windows.Forms.TreeNode[] {
            treeNode8,
            treeNode9,
            treeNode10});
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Process 123");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Process 456");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Process", new System.Windows.Forms.TreeNode[] {
            treeNode12,
            treeNode13});
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("Orphaned Objects", new System.Windows.Forms.TreeNode[] {
            treeNode11,
            treeNode14});
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button1 = new MetaControls.MetaButton();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.FullRowSelect = true;
            this.treeView1.Location = new System.Drawing.Point(0, 35);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "Activity";
            treeNode1.Text = "Activity";
            treeNode2.Name = "Node8";
            treeNode2.Text = "... can expand to child objects";
            treeNode3.Name = "Node5";
            treeNode3.Text = "Maintain System";
            treeNode4.Name = "Node6";
            treeNode4.Text = "Repair System";
            treeNode5.Name = "Function";
            treeNode5.Text = "Function";
            treeNode6.Name = "node1";
            treeNode6.Text = "OSD - ACME Company - To-Be";
            treeNode7.ForeColor = System.Drawing.Color.Teal;
            treeNode7.Name = "Diagrams";
            treeNode7.Text = "Diagrams";
            treeNode8.Name = "Node10";
            treeNode8.Text = "Plan System";
            treeNode9.Name = "Node9";
            treeNode9.Text = "Design Solution";
            treeNode10.Name = "Node11";
            treeNode10.Text = "Implement Solution";
            treeNode11.Name = "Function2";
            treeNode11.Text = "Function";
            treeNode12.Name = "Node13";
            treeNode12.Text = "Process 123";
            treeNode13.Name = "Node14";
            treeNode13.Text = "Process 456";
            treeNode14.Name = "Node12";
            treeNode14.Text = "Process";
            treeNode15.Name = "Node4";
            treeNode15.Text = "Orphaned Objects";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode7,
            treeNode15});
            this.treeView1.Size = new System.Drawing.Size(287, 286);
            this.treeView1.TabIndex = 0;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(86, 8);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(201, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 327);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Refresh";
            // 
            // RepositoryTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.treeView1);
            this.Name = "RepositoryTree";
            this.Size = new System.Drawing.Size(290, 388);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ComboBox comboBox1;
        private MetaControls.MetaButton button1;
    }
}
