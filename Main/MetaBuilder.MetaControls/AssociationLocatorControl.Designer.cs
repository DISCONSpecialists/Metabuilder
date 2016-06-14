namespace MetaBuilder.MetaControls
{
    partial class AssociationLocatorControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssociationLocatorControl));
            this.cbSelected = new XPTable.Models.CheckBoxColumn();
            this.colAssocType = new XPTable.Models.TextColumn();
            this.colFromObject = new XPTable.Models.TextColumn();
            this.colToObject = new XPTable.Models.TextColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearCurrentSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.panelTop = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbClearResults = new System.Windows.Forms.CheckBox();
            this.comboDiagram = new System.Windows.Forms.ComboBox();
            this.comboAssociationType = new System.Windows.Forms.ComboBox();
            this.btnSelectNone = new MetaControls.MetaButton();
            this.comboStatus = new System.Windows.Forms.ComboBox();
            this.comboClass = new System.Windows.Forms.ComboBox();
            this.btnSelectAll = new MetaControls.MetaButton();
            this.cbStatus = new System.Windows.Forms.CheckBox();
            this.cbClass = new System.Windows.Forms.CheckBox();
            this.cbDiagram = new System.Windows.Forms.CheckBox();
            this.btnFind = new MetaControls.MetaButton();
            this.labelProgress = new System.Windows.Forms.Label();
            this.progressBarFind = new System.Windows.Forms.ProgressBar();
            this.cbType = new System.Windows.Forms.CheckBox();
            this.associationList2 = new MetaBuilder.MetaControls.AssociationList();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // colAssocType
            // 
            this.colAssocType.Editable = false;
            this.colAssocType.Text = "Association";
            this.colAssocType.Width = 150;
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
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripButton,
            this.saveToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(621, 25);
            this.toolStrip1.TabIndex = 8;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // openToolStripButton
            // 
            this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripButton.Image")));
            this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripButton.Name = "openToolStripButton";
            this.openToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.openToolStripButton.Text = "&Open";
            this.openToolStripButton.Click += new System.EventHandler(this.openToolStripButton_Click);
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton.Image")));
            this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.saveToolStripButton.Text = "&Save";
            this.saveToolStripButton.Click += new System.EventHandler(this.saveToolStripButton_Click);
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.cbClearResults);
            this.panelTop.Controls.Add(this.groupBox1);
            this.panelTop.Controls.Add(this.labelProgress);
            this.panelTop.Controls.Add(this.progressBarFind);
            this.panelTop.Controls.Add(this.btnSelectNone);
            this.panelTop.Controls.Add(this.btnFind);
            this.panelTop.Controls.Add(this.btnSelectAll);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 25);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(621, 109);
            this.panelTop.TabIndex = 9;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboDiagram);
            this.groupBox1.Controls.Add(this.comboAssociationType);
            this.groupBox1.Controls.Add(this.comboStatus);
            this.groupBox1.Controls.Add(this.comboClass);
            this.groupBox1.Controls.Add(this.cbStatus);
            this.groupBox1.Controls.Add(this.cbClass);
            this.groupBox1.Controls.Add(this.cbDiagram);
            this.groupBox1.Controls.Add(this.cbType);
            this.groupBox1.Location = new System.Drawing.Point(277, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(344, 107);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter(s)";
            // 
            // cbClearResults
            // 
            this.cbClearResults.AutoSize = true;
            this.cbClearResults.Checked = true;
            this.cbClearResults.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbClearResults.Location = new System.Drawing.Point(6, 54);
            this.cbClearResults.Name = "cbClearResults";
            this.cbClearResults.Size = new System.Drawing.Size(88, 17);
            this.cbClearResults.TabIndex = 4;
            this.cbClearResults.Text = "Clear Results";
            this.cbClearResults.UseVisualStyleBackColor = true;
            // 
            // comboDiagram
            // 
            this.comboDiagram.DisplayMember = "Name";
            this.comboDiagram.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDiagram.Enabled = false;
            this.comboDiagram.FormattingEnabled = true;
            this.comboDiagram.Location = new System.Drawing.Point(75, 13);
            this.comboDiagram.Name = "comboDiagram";
            this.comboDiagram.Size = new System.Drawing.Size(256, 21);
            this.comboDiagram.TabIndex = 1;
            this.comboDiagram.SelectedIndexChanged += new System.EventHandler(this.comboAssociationType_SelectedIndexChanged);
            // 
            // comboAssociationType
            // 
            this.comboAssociationType.DisplayMember = "Name";
            this.comboAssociationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboAssociationType.Enabled = false;
            this.comboAssociationType.FormattingEnabled = true;
            this.comboAssociationType.Location = new System.Drawing.Point(75, 37);
            this.comboAssociationType.Name = "comboAssociationType";
            this.comboAssociationType.Size = new System.Drawing.Size(256, 21);
            this.comboAssociationType.TabIndex = 1;
            this.comboAssociationType.SelectedIndexChanged += new System.EventHandler(this.comboAssociationType_SelectedIndexChanged);
            // 
            // btnSelectNone
            // 
            this.btnSelectNone.Location = new System.Drawing.Point(180, 51);
            this.btnSelectNone.Name = "btnSelectNone";
            this.btnSelectNone.Size = new System.Drawing.Size(80, 20);
            this.btnSelectNone.TabIndex = 2;
            this.btnSelectNone.Text = "Select None";
            this.btnSelectNone.Click += new System.EventHandler(this.btnSelectNone_Click);
            // 
            // comboStatus
            // 
            this.comboStatus.DisplayMember = "Name";
            this.comboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboStatus.Enabled = false;
            this.comboStatus.FormattingEnabled = true;
            this.comboStatus.Location = new System.Drawing.Point(75, 83);
            this.comboStatus.Name = "comboStatus";
            this.comboStatus.Size = new System.Drawing.Size(256, 21);
            this.comboStatus.TabIndex = 1;
            // 
            // comboClass
            // 
            this.comboClass.DisplayMember = "Name";
            this.comboClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboClass.Enabled = false;
            this.comboClass.FormattingEnabled = true;
            this.comboClass.Location = new System.Drawing.Point(75, 60);
            this.comboClass.Name = "comboClass";
            this.comboClass.Size = new System.Drawing.Size(256, 21);
            this.comboClass.TabIndex = 1;
            this.comboClass.SelectedIndexChanged += new System.EventHandler(this.comboClass_SelectedIndexChanged);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(102, 51);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(72, 20);
            this.btnSelectAll.TabIndex = 2;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // cbStatus
            // 
            this.cbStatus.AutoSize = true;
            this.cbStatus.Location = new System.Drawing.Point(5, 85);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(56, 17);
            this.cbStatus.TabIndex = 3;
            this.cbStatus.Text = "Status";
            this.cbStatus.UseVisualStyleBackColor = true;
            this.cbStatus.CheckedChanged += new System.EventHandler(this.cbStatus_CheckedChanged_1);
            // 
            // cbClass
            // 
            this.cbClass.AutoSize = true;
            this.cbClass.Location = new System.Drawing.Point(5, 62);
            this.cbClass.Name = "cbClass";
            this.cbClass.Size = new System.Drawing.Size(62, 17);
            this.cbClass.TabIndex = 3;
            this.cbClass.Text = "Classes";
            this.cbClass.UseVisualStyleBackColor = true;
            this.cbClass.CheckedChanged += new System.EventHandler(this.cbClass_CheckedChanged);
            // 
            // cbDiagram
            // 
            this.cbDiagram.AutoSize = true;
            this.cbDiagram.Location = new System.Drawing.Point(6, 15);
            this.cbDiagram.Name = "cbDiagram";
            this.cbDiagram.Size = new System.Drawing.Size(65, 17);
            this.cbDiagram.TabIndex = 3;
            this.cbDiagram.Text = "Diagram";
            this.cbDiagram.UseVisualStyleBackColor = true;
            this.cbDiagram.CheckedChanged += new System.EventHandler(this.cbDiagram_CheckedChanged);
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(81, 15);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(179, 26);
            this.btnFind.TabIndex = 2;
            this.btnFind.Text = "Find...";
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // labelProgress
            // 
            this.labelProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProgress.Location = new System.Drawing.Point(6, 41);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(70, 11);
            this.labelProgress.TabIndex = 5;
            // 
            // progressBarFind
            // 
            this.progressBarFind.Location = new System.Drawing.Point(81, 41);
            this.progressBarFind.Name = "progressBarFind";
            this.progressBarFind.Size = new System.Drawing.Size(179, 10);
            this.progressBarFind.TabIndex = 6;
            // 
            // cbType
            // 
            this.cbType.AutoSize = true;
            this.cbType.Location = new System.Drawing.Point(6, 39);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(50, 17);
            this.cbType.TabIndex = 3;
            this.cbType.Text = "Type";
            this.cbType.UseVisualStyleBackColor = true;
            this.cbType.CheckedChanged += new System.EventHandler(this.cbType_CheckedChanged);
            // 
            // associationList2
            // 
            this.associationList2.AllowMultipleSelection = false;
            this.associationList2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.associationList2.LimitToStatus = 0;
            this.associationList2.ListedAssociations = null;
            this.associationList2.Location = new System.Drawing.Point(0, 134);
            this.associationList2.Name = "associationList2";
            this.associationList2.SelectedAssociations = null;
            this.associationList2.Size = new System.Drawing.Size(621, 270);
            this.associationList2.TabIndex = 10;
            // 
            // AssociationLocatorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.associationList2);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.toolStrip1);
            this.Name = "AssociationLocatorControl";
            this.Size = new System.Drawing.Size(621, 404);
            this.Load += new System.EventHandler(this.AssociationLocatorControl_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem clearCurrentSelectionToolStripMenuItem;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private XPTable.Models.TextColumn colAssocType;
        private XPTable.Models.TextColumn colFromObject;
        private XPTable.Models.TextColumn colToObject;
        private XPTable.Models.CheckBoxColumn cbSelected;
        private AssociationList associationList1;
        private AssociationList associationList2;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbClearResults;
        private System.Windows.Forms.ComboBox comboAssociationType;
        private MetaControls.MetaButton btnSelectNone;
        private System.Windows.Forms.ComboBox comboStatus;
        private System.Windows.Forms.ComboBox comboClass;
        private MetaControls.MetaButton btnSelectAll;
        private System.Windows.Forms.CheckBox cbStatus;
        private System.Windows.Forms.CheckBox cbClass;
        private MetaControls.MetaButton btnFind;
        private System.Windows.Forms.CheckBox cbType;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton openToolStripButton;
        private System.Windows.Forms.ToolStripButton saveToolStripButton;
        private System.Windows.Forms.ComboBox comboDiagram;
        private System.Windows.Forms.CheckBox cbDiagram;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.ProgressBar progressBarFind;




    }
}
