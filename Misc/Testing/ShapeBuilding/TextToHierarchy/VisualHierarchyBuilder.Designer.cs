namespace ShapeBuilding.TextToHierarchy
{
    partial class VisualHierarchyBuilder
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.txtStructureName = new System.Windows.Forms.TextBox();
            this.cbUseTextOnlyNodes = new System.Windows.Forms.CheckBox();
            this.lblStructureName = new System.Windows.Forms.Label();
            this.lblDefaultClass = new System.Windows.Forms.Label();
            this.btnLoadText = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSaveTextFile = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.textBox1 = new FastColoredTextBoxNS.FastColoredTextBox();
            this.goView1 = new Northwoods.Go.GoView();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblAddClass = new System.Windows.Forms.Label();
            this.lblUsage = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(314, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(186, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(506, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(155, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save to Database";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.lblAddClass);
            this.panel1.Controls.Add(this.lblUsage);
            this.panel1.Controls.Add(this.treeView1);
            this.panel1.Controls.Add(this.propertyGrid1);
            this.panel1.Controls.Add(this.txtStructureName);
            this.panel1.Controls.Add(this.cbUseTextOnlyNodes);
            this.panel1.Controls.Add(this.lblStructureName);
            this.panel1.Controls.Add(this.lblDefaultClass);
            this.panel1.Controls.Add(this.btnLoadText);
            this.panel1.Controls.Add(this.btnLoad);
            this.panel1.Controls.Add(this.btnSaveTextFile);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 501);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(835, 101);
            this.panel1.TabIndex = 5;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(767, 3);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(130, 130);
            this.propertyGrid1.TabIndex = 5;
            this.propertyGrid1.Visible = false;
            // 
            // txtStructureName
            // 
            this.txtStructureName.Location = new System.Drawing.Point(314, 34);
            this.txtStructureName.Name = "txtStructureName";
            this.txtStructureName.Size = new System.Drawing.Size(186, 20);
            this.txtStructureName.TabIndex = 4;
            this.txtStructureName.Text = "Visual Tree Builder";
            // 
            // cbUseTextOnlyNodes
            // 
            this.cbUseTextOnlyNodes.AutoSize = true;
            this.cbUseTextOnlyNodes.Location = new System.Drawing.Point(314, 60);
            this.cbUseTextOnlyNodes.Name = "cbUseTextOnlyNodes";
            this.cbUseTextOnlyNodes.Size = new System.Drawing.Size(127, 17);
            this.cbUseTextOnlyNodes.TabIndex = 1;
            this.cbUseTextOnlyNodes.Text = "Use Text Only Nodes";
            this.cbUseTextOnlyNodes.UseVisualStyleBackColor = true;
            this.cbUseTextOnlyNodes.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // lblStructureName
            // 
            this.lblStructureName.AutoSize = true;
            this.lblStructureName.Location = new System.Drawing.Point(183, 37);
            this.lblStructureName.Name = "lblStructureName";
            this.lblStructureName.Size = new System.Drawing.Size(128, 13);
            this.lblStructureName.TabIndex = 3;
            this.lblStructureName.Text = "Structure/Diagram Name:";
            // 
            // lblDefaultClass
            // 
            this.lblDefaultClass.AutoSize = true;
            this.lblDefaultClass.Location = new System.Drawing.Point(183, 8);
            this.lblDefaultClass.Name = "lblDefaultClass";
            this.lblDefaultClass.Size = new System.Drawing.Size(72, 13);
            this.lblDefaultClass.TabIndex = 3;
            this.lblDefaultClass.Text = "Default Class:";
            // 
            // btnLoadText
            // 
            this.btnLoadText.Location = new System.Drawing.Point(506, 75);
            this.btnLoadText.Name = "btnLoadText";
            this.btnLoadText.Size = new System.Drawing.Size(155, 23);
            this.btnLoadText.TabIndex = 2;
            this.btnLoadText.Text = "Load Text File";
            this.btnLoadText.UseVisualStyleBackColor = true;
            this.btnLoadText.Click += new System.EventHandler(this.btnLoadText_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(506, 51);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(155, 23);
            this.btnLoad.TabIndex = 2;
            this.btnLoad.Text = "Load from Diagram";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSaveTextFile
            // 
            this.btnSaveTextFile.Location = new System.Drawing.Point(506, 27);
            this.btnSaveTextFile.Name = "btnSaveTextFile";
            this.btnSaveTextFile.Size = new System.Drawing.Size(155, 23);
            this.btnSaveTextFile.TabIndex = 2;
            this.btnSaveTextFile.Text = "Save Text File";
            this.btnSaveTextFile.UseVisualStyleBackColor = true;
            this.btnSaveTextFile.Click += new System.EventHandler(this.btnSaveTextFile_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.textBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.goView1);
            this.splitContainer1.Size = new System.Drawing.Size(835, 501);
            this.splitContainer1.SplitterDistance = 341;
            this.splitContainer1.TabIndex = 6;
            // 
            // textBox1
            // 
            this.textBox1.AutoScrollMinSize = new System.Drawing.Size(47, 12);
            this.textBox1.CurrentLineColor = System.Drawing.Color.Gainsboro;
            this.textBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.IndentBackColor = System.Drawing.Color.Transparent;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(341, 501);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "Node";
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            this.textBox1.LineInserted += new System.EventHandler<FastColoredTextBoxNS.LineInsertedEventArgs>(this.textBox1_LineInserted);
            this.textBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyUp);
            this.textBox1.LineRemoved += new System.EventHandler<FastColoredTextBoxNS.LineRemovedEventArgs>(this.textBox1_LineRemoved);
            this.textBox1.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.textBox1_TextChanged);
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // goView1
            // 
            this.goView1.ArrowMoveLarge = 10F;
            this.goView1.ArrowMoveSmall = 1F;
            this.goView1.BackColor = System.Drawing.Color.White;
            this.goView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.goView1.Location = new System.Drawing.Point(0, 0);
            this.goView1.Name = "goView1";
            this.goView1.Size = new System.Drawing.Size(490, 501);
            this.goView1.TabIndex = 0;
            this.goView1.Text = "goView1";
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(767, 6);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(121, 97);
            this.treeView1.TabIndex = 6;
            this.treeView1.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Enter: Process Text";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Ctrl+L: Add Link";
            // 
            // lblAddClass
            // 
            this.lblAddClass.AutoSize = true;
            this.lblAddClass.Location = new System.Drawing.Point(39, 27);
            this.lblAddClass.Name = "lblAddClass";
            this.lblAddClass.Size = new System.Drawing.Size(88, 13);
            this.lblAddClass.TabIndex = 7;
            this.lblAddClass.Text = "Ctrl+K: Add Class";
            // 
            // lblUsage
            // 
            this.lblUsage.AutoSize = true;
            this.lblUsage.Location = new System.Drawing.Point(3, 8);
            this.lblUsage.Name = "lblUsage";
            this.lblUsage.Size = new System.Drawing.Size(41, 13);
            this.lblUsage.TabIndex = 8;
            this.lblUsage.Text = "Usage:";
            // 
            // VisualHierarchyBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(835, 602);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Name = "VisualHierarchyBuilder";
            this.Text = "Visual Hierarchy Builder";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Northwoods.Go.GoView goView1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label lblDefaultClass;
        private System.Windows.Forms.CheckBox cbUseTextOnlyNodes;
        private FastColoredTextBoxNS.FastColoredTextBox textBox1;
        private System.Windows.Forms.TextBox txtStructureName;
        private System.Windows.Forms.Label lblStructureName;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnLoadText;
        private System.Windows.Forms.Button btnSaveTextFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblAddClass;
        private System.Windows.Forms.Label lblUsage;
    }
}