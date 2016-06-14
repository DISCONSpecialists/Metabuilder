namespace MetaBuilder.MetaControls
{
    partial class ObjectFinderControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ObjectFinderControl));
            this.lblSearchFor = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbClearResults = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbWorkspace = new System.Windows.Forms.CheckBox();
            this.comboDiagram = new System.Windows.Forms.ComboBox();
            this.comboStatus = new System.Windows.Forms.ComboBox();
            this.comboClass = new System.Windows.Forms.ComboBox();
            this.comboDomainValue = new System.Windows.Forms.ComboBox();
            this.cbStatus = new System.Windows.Forms.CheckBox();
            this.comboWorkspace = new System.Windows.Forms.ComboBox();
            this.cbClass = new System.Windows.Forms.CheckBox();
            this.cbDomainValue = new System.Windows.Forms.CheckBox();
            this.comboDomain = new System.Windows.Forms.ComboBox();
            this.cbDiagram = new System.Windows.Forms.CheckBox();
            this.txtSearchValue = new System.Windows.Forms.TextBox();
            this.btnFind = new MetaControls.MetaButton();
            this.btnSelectNone = new MetaControls.MetaButton();
            this.btnSelectAll = new MetaControls.MetaButton();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.objectList1 = new MetaBuilder.MetaControls.ObjectList(Server);
            this.progressBarFind = new System.Windows.Forms.ProgressBar();
            this.labelProgress = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSearchFor
            // 
            this.lblSearchFor.AutoSize = true;
            this.lblSearchFor.Location = new System.Drawing.Point(-157, 94);
            this.lblSearchFor.Name = "lblSearchFor";
            this.lblSearchFor.Size = new System.Drawing.Size(59, 13);
            this.lblSearchFor.TabIndex = 12;
            this.lblSearchFor.Text = "Search for:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbClearResults);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.txtSearchValue);
            this.panel1.Controls.Add(this.btnFind);
            this.panel1.Controls.Add(this.labelProgress);
            this.panel1.Controls.Add(this.progressBarFind);
            this.panel1.Controls.Add(this.btnSelectNone);
            this.panel1.Controls.Add(this.btnSelectAll);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(615, 149);
            this.panel1.TabIndex = 13;
            // 
            // cbClearResults
            // 
            this.cbClearResults.AutoSize = true;
            this.cbClearResults.Checked = true;
            this.cbClearResults.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbClearResults.Location = new System.Drawing.Point(3, 70);
            this.cbClearResults.Name = "cbClearResults";
            this.cbClearResults.Size = new System.Drawing.Size(88, 17);
            this.cbClearResults.TabIndex = 20;
            this.cbClearResults.Text = "Clear Results";
            this.cbClearResults.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbWorkspace);
            this.groupBox1.Controls.Add(this.comboDiagram);
            this.groupBox1.Controls.Add(this.comboStatus);
            this.groupBox1.Controls.Add(this.comboClass);
            this.groupBox1.Controls.Add(this.comboDomainValue);
            this.groupBox1.Controls.Add(this.cbStatus);
            this.groupBox1.Controls.Add(this.comboWorkspace);
            this.groupBox1.Controls.Add(this.cbClass);
            this.groupBox1.Controls.Add(this.cbDomainValue);
            this.groupBox1.Controls.Add(this.comboDomain);
            this.groupBox1.Controls.Add(this.cbDiagram);
            this.groupBox1.Location = new System.Drawing.Point(254, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(535, 130);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter(s)";
            // 
            // cbWorkspace
            // 
            this.cbWorkspace.AutoSize = true;
            this.cbWorkspace.Location = new System.Drawing.Point(6, 14);
            this.cbWorkspace.Name = "cbWorkspace";
            this.cbWorkspace.Size = new System.Drawing.Size(81, 17);
            this.cbWorkspace.TabIndex = 3;
            this.cbWorkspace.Text = "Workspace";
            this.cbWorkspace.UseVisualStyleBackColor = true;
            this.cbWorkspace.Click += new System.EventHandler(this.cbWorkspace_CheckedChanged);
            // 
            // comboDiagram
            // 
            this.comboDiagram.DisplayMember = "Name";
            this.comboDiagram.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDiagram.Enabled = false;
            this.comboDiagram.FormattingEnabled = true;
            this.comboDiagram.Location = new System.Drawing.Point(170, 35);
            this.comboDiagram.Name = "comboDiagram";
            this.comboDiagram.Size = new System.Drawing.Size(355, 21);
            this.comboDiagram.TabIndex = 1;
            // 
            // comboStatus
            // 
            this.comboStatus.DisplayMember = "Name";
            this.comboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboStatus.Enabled = false;
            this.comboStatus.FormattingEnabled = true;
            this.comboStatus.Location = new System.Drawing.Point(170, 104);
            this.comboStatus.Name = "comboStatus";
            this.comboStatus.Size = new System.Drawing.Size(355, 21);
            this.comboStatus.TabIndex = 1;
            // 
            // comboClass
            // 
            this.comboClass.DisplayMember = "Name";
            this.comboClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboClass.Enabled = false;
            this.comboClass.FormattingEnabled = true;
            this.comboClass.Location = new System.Drawing.Point(170, 58);
            this.comboClass.Name = "comboClass";
            this.comboClass.Size = new System.Drawing.Size(355, 21);
            this.comboClass.TabIndex = 1;
            this.comboClass.SelectedIndexChanged += new System.EventHandler(comboClass_SelectedIndexChanged);
            // 
            // comboDomainValue
            // 
            this.comboDomainValue.DisplayMember = "PossibleValue";
            this.comboDomainValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDomainValue.Enabled = false;
            this.comboDomainValue.FormattingEnabled = true;
            this.comboDomainValue.Location = new System.Drawing.Point(170, 81);
            this.comboDomainValue.Name = "comboDomainValue";
            this.comboDomainValue.Size = new System.Drawing.Size(355, 21);
            this.comboDomainValue.TabIndex = 6;
            // 
            // cbStatus
            // 
            this.cbStatus.AutoSize = true;
            this.cbStatus.Location = new System.Drawing.Point(6, 106);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(56, 17);
            this.cbStatus.TabIndex = 3;
            this.cbStatus.Text = "Status";
            this.cbStatus.UseVisualStyleBackColor = true;
            this.cbStatus.Click += new System.EventHandler(this.cbStatus_Click);
            // 
            // comboWorkspace
            // 
            this.comboWorkspace.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboWorkspace.Enabled = false;
            this.comboWorkspace.FormattingEnabled = true;
            this.comboWorkspace.Location = new System.Drawing.Point(170, 12);
            this.comboWorkspace.Name = "comboWorkspace";
            this.comboWorkspace.Size = new System.Drawing.Size(355, 21);
            this.comboWorkspace.TabIndex = 1;
            this.comboWorkspace.SelectedIndexChanged += new System.EventHandler(this.comboWorkspace_SelectedIndexChanged);
            // 
            // cbClass
            // 
            this.cbClass.AutoSize = true;
            this.cbClass.Location = new System.Drawing.Point(6, 60);
            this.cbClass.Name = "cbClass";
            this.cbClass.Size = new System.Drawing.Size(51, 17);
            this.cbClass.TabIndex = 3;
            this.cbClass.Text = "Class";
            this.cbClass.UseVisualStyleBackColor = true;
            this.cbClass.Click += new System.EventHandler(this.cbClass_CheckedChanged);
            //
            // comboDomain
            //
            this.comboDomain.Name = "comboDomain";
            this.comboDomain.AutoSize = false;
            this.comboDomain.Location = new System.Drawing.Point(20, 81);
            this.comboDomain.Size = new System.Drawing.Size(150, 21);
            this.comboDomain.TabIndex = 5;
            this.comboDomain.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDomain.SelectedIndexChanged += new System.EventHandler(comboDomain_SelectedIndexChanged);
            // 
            // cbDomainValue
            // 
            this.cbDomainValue.AutoSize = true;
            this.cbDomainValue.Location = new System.Drawing.Point(6, 83);
            this.cbDomainValue.Name = "cbDomainValue";
            this.cbDomainValue.Size = new System.Drawing.Size(51, 17);
            this.cbDomainValue.TabIndex = 4;
            this.cbDomainValue.Text = "Class Type";
            this.cbDomainValue.UseVisualStyleBackColor = true;
            this.cbDomainValue.Click += new System.EventHandler(this.cbDomainValue_CheckedChanged);
            // 
            // cbDiagram
            // 
            this.cbDiagram.AutoSize = true;
            this.cbDiagram.Location = new System.Drawing.Point(6, 37);
            this.cbDiagram.Name = "cbDiagram";
            this.cbDiagram.Size = new System.Drawing.Size(65, 17);
            this.cbDiagram.TabIndex = 3;
            this.cbDiagram.Text = "Diagram";
            this.cbDiagram.UseVisualStyleBackColor = true;
            this.cbDiagram.Click += new System.EventHandler(this.cbDiagram_CheckedChanged);
            // 
            // txtSearchValue
            // 
            this.txtSearchValue.Location = new System.Drawing.Point(72, 8);
            this.txtSearchValue.Name = "txtSearchValue";
            this.txtSearchValue.Size = new System.Drawing.Size(174, 20);
            this.txtSearchValue.TabIndex = 14;
            this.txtSearchValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearchValue_KeyDown);
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(72, 32);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(174, 28);
            this.btnFind.TabIndex = 16;
            this.btnFind.Text = "Find...";
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // progressBarFind
            // 
            this.progressBarFind.Name = "progressBarFind";
            this.progressBarFind.Location = new System.Drawing.Point(72, 58);
            this.progressBarFind.Size = new System.Drawing.Size(174, 10);
            //
            // labelProgress
            //
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Location = new System.Drawing.Point(7, 58);
            this.labelProgress.AutoSize = false;
            this.labelProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProgress.Size = new System.Drawing.Size(62, 10);
            //this.progressBarFind.Size = new System.Drawing.Size(174, 12);
            this.labelProgress.Text = "";
            // 
            // btnSelectNone
            // 
            this.btnSelectNone.Location = new System.Drawing.Point(171, 68);
            this.btnSelectNone.Name = "btnSelectNone";
            this.btnSelectNone.Size = new System.Drawing.Size(75, 20);
            this.btnSelectNone.TabIndex = 17;
            this.btnSelectNone.Text = "Select None";
            this.btnSelectNone.Click += new System.EventHandler(this.btnSelectNone_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(95, 68);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(70, 20);
            this.btnSelectAll.TabIndex = 15;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Search for:";
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripButton,
            this.saveToolStripButton});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(615, 23);
            this.toolStrip1.TabIndex = 21;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // openToolStripButton
            // 
            this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripButton.Image")));
            this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripButton.Name = "openToolStripButton";
            this.openToolStripButton.Size = new System.Drawing.Size(23, 20);
            this.openToolStripButton.Text = "&Open";
            this.openToolStripButton.ToolTipText = "Open Search Result from Disk";
            this.openToolStripButton.Click += new System.EventHandler(this.openToolStripButton_Click);
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton.Image")));
            this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new System.Drawing.Size(23, 20);
            this.saveToolStripButton.Text = "&Save";
            this.saveToolStripButton.ToolTipText = "Save Search Result to Disk";
            this.saveToolStripButton.Click += new System.EventHandler(this.saveToolStripButton_Click);
            // 
            // objectList1
            // 
            this.objectList1.AllowMultipleSelection = false;
            this.objectList1.AutoSize = true;
            this.objectList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectList1.LFDB = false;
            this.objectList1.ListedObjects = null;
            this.objectList1.ListedObjectsDictionary = null;
            this.objectList1.Location = new System.Drawing.Point(0, 142);
            this.objectList1.MultiSelectInProgress = false;
            this.objectList1.Name = "objectList1";
            this.objectList1.SelectedObjectsDictionary = null;
            this.objectList1.Size = new System.Drawing.Size(615, 256);
            this.objectList1.TabIndex = 15;
            // 
            // ObjectFinderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.objectList1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblSearchFor);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ObjectFinderControl";
            this.Size = new System.Drawing.Size(790, 438);
            this.Load += new System.EventHandler(this.ObjectFinderControl_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

            //this.toolStrip1.SendToBack();
            //this.panel1.BringToFront();
            //this.objectList1.BringToFront();

        }

        #endregion

        private System.Windows.Forms.Label lblSearchFor;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbWorkspace;
        private System.Windows.Forms.ComboBox comboDiagram;
        private System.Windows.Forms.ComboBox comboClass;
        private System.Windows.Forms.ComboBox comboDomainValue;
        private System.Windows.Forms.ComboBox comboWorkspace;
        private System.Windows.Forms.CheckBox cbClass;
        private System.Windows.Forms.CheckBox cbDomainValue;
        private System.Windows.Forms.ComboBox comboDomain;
        private System.Windows.Forms.CheckBox cbDiagram;
        private System.Windows.Forms.TextBox txtSearchValue;
        private MetaControls.MetaButton btnFind;
        private MetaControls.MetaButton btnSelectNone;
        private MetaControls.MetaButton btnSelectAll;
        private System.Windows.Forms.Label label1;
        private ObjectList objectList1;
        private System.Windows.Forms.ComboBox comboStatus;
        private System.Windows.Forms.CheckBox cbStatus;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton openToolStripButton;
        private System.Windows.Forms.ToolStripButton saveToolStripButton;
        private System.Windows.Forms.CheckBox cbClearResults;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.ProgressBar progressBarFind;
    }
}