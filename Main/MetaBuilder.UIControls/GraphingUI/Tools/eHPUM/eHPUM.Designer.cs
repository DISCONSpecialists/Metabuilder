namespace MetaBuilder.UIControls.GraphingUI.Tools.eHPUM
{
    partial class eHPUM
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
            this.buttonClose = new MetaControls.MetaButton();
            this.labelFile = new System.Windows.Forms.Label();
            this.buttonChooseFile = new MetaControls.MetaButton();
            this.treeViewLayout = new System.Windows.Forms.TreeView();
            this.buttonImport = new MetaControls.MetaButton();
            this.panelProperties = new System.Windows.Forms.Panel();
            this.labelWarning = new System.Windows.Forms.Label();
            this.labelAssInfo = new System.Windows.Forms.Label();
            this.labelClassInfo = new System.Windows.Forms.Label();
            this.comboBoxAss = new System.Windows.Forms.ComboBox();
            this.comboBoxClass = new System.Windows.Forms.ComboBox();
            this.labelPropInfo = new System.Windows.Forms.Label();
            this.numericUpDownLimit = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBarImport = new System.Windows.Forms.ProgressBar();
            this.labelImportInfo = new System.Windows.Forms.Label();
            this.checkBoxAutoImport = new System.Windows.Forms.CheckBox();
            this.panelProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLimit)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(510, 202);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "Close";
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // labelFile
            // 
            this.labelFile.AutoSize = true;
            this.labelFile.Location = new System.Drawing.Point(84, 6);
            this.labelFile.Name = "labelFile";
            this.labelFile.Size = new System.Drawing.Size(95, 13);
            this.labelFile.TabIndex = 1;
            this.labelFile.Text = "Please select a file";
            this.labelFile.Click += new System.EventHandler(this.labelFile_Click);
            // 
            // buttonChooseFile
            // 
            this.buttonChooseFile.Location = new System.Drawing.Point(3, 1);
            this.buttonChooseFile.Name = "buttonChooseFile";
            this.buttonChooseFile.Size = new System.Drawing.Size(75, 23);
            this.buttonChooseFile.TabIndex = 2;
            this.buttonChooseFile.Text = "Choose File";
            this.buttonChooseFile.Click += new System.EventHandler(this.buttonChooseFile_Click);
            // 
            // treeViewLayout
            // 
            this.treeViewLayout.Location = new System.Drawing.Point(3, 30);
            this.treeViewLayout.Name = "treeViewLayout";
            this.treeViewLayout.Size = new System.Drawing.Size(272, 168);
            this.treeViewLayout.TabIndex = 3;
            this.treeViewLayout.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewLayout_AfterSelect);
            // 
            // buttonImport
            // 
            this.buttonImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonImport.Enabled = false;
            this.buttonImport.Location = new System.Drawing.Point(429, 202);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(75, 23);
            this.buttonImport.TabIndex = 4;
            this.buttonImport.Text = "Import";
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // panelProperties
            // 
            this.panelProperties.Controls.Add(this.labelWarning);
            this.panelProperties.Controls.Add(this.labelAssInfo);
            this.panelProperties.Controls.Add(this.labelClassInfo);
            this.panelProperties.Controls.Add(this.comboBoxAss);
            this.panelProperties.Controls.Add(this.comboBoxClass);
            this.panelProperties.Controls.Add(this.labelPropInfo);
            this.panelProperties.Location = new System.Drawing.Point(282, 30);
            this.panelProperties.Name = "panelProperties";
            this.panelProperties.Size = new System.Drawing.Size(303, 166);
            this.panelProperties.TabIndex = 5;
            this.panelProperties.Visible = false;
            // 
            // labelWarning
            // 
            this.labelWarning.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelWarning.Location = new System.Drawing.Point(0, 45);
            this.labelWarning.Name = "labelWarning";
            this.labelWarning.Size = new System.Drawing.Size(303, 23);
            this.labelWarning.TabIndex = 5;
            this.labelWarning.Text = "These fields must match the metamodel";
            this.labelWarning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelAssInfo
            // 
            this.labelAssInfo.AutoSize = true;
            this.labelAssInfo.Location = new System.Drawing.Point(3, 102);
            this.labelAssInfo.Name = "labelAssInfo";
            this.labelAssInfo.Size = new System.Drawing.Size(87, 13);
            this.labelAssInfo.TabIndex = 4;
            this.labelAssInfo.Text = "Child Association";
            // 
            // labelClassInfo
            // 
            this.labelClassInfo.AutoSize = true;
            this.labelClassInfo.Location = new System.Drawing.Point(3, 74);
            this.labelClassInfo.Name = "labelClassInfo";
            this.labelClassInfo.Size = new System.Drawing.Size(66, 13);
            this.labelClassInfo.TabIndex = 3;
            this.labelClassInfo.Text = "Object Class";
            // 
            // comboBoxAss
            // 
            this.comboBoxAss.FormattingEnabled = true;
            this.comboBoxAss.Location = new System.Drawing.Point(97, 99);
            this.comboBoxAss.Name = "comboBoxAss";
            this.comboBoxAss.Size = new System.Drawing.Size(203, 21);
            this.comboBoxAss.TabIndex = 2;
            this.comboBoxAss.SelectedIndexChanged += new System.EventHandler(this.comboBoxAss_SelectedIndexChanged);
            // 
            // comboBoxClass
            // 
            this.comboBoxClass.FormattingEnabled = true;
            this.comboBoxClass.Location = new System.Drawing.Point(97, 71);
            this.comboBoxClass.Name = "comboBoxClass";
            this.comboBoxClass.Size = new System.Drawing.Size(203, 21);
            this.comboBoxClass.TabIndex = 1;
            this.comboBoxClass.SelectedIndexChanged += new System.EventHandler(this.comboBoxClass_SelectedIndexChanged);
            // 
            // labelPropInfo
            // 
            this.labelPropInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelPropInfo.Location = new System.Drawing.Point(0, 0);
            this.labelPropInfo.Name = "labelPropInfo";
            this.labelPropInfo.Size = new System.Drawing.Size(303, 45);
            this.labelPropInfo.TabIndex = 0;
            this.labelPropInfo.Text = "Information";
            this.labelPropInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numericUpDownLimit
            // 
            this.numericUpDownLimit.Location = new System.Drawing.Point(156, 204);
            this.numericUpDownLimit.Maximum = new decimal(new int[] {
            5000000,
            0,
            0,
            0});
            this.numericUpDownLimit.Name = "numericUpDownLimit";
            this.numericUpDownLimit.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownLimit.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 206);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Row Limit (0 for unlimited)";
            // 
            // progressBarImport
            // 
            this.progressBarImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarImport.Location = new System.Drawing.Point(281, 228);
            this.progressBarImport.Name = "progressBarImport";
            this.progressBarImport.Size = new System.Drawing.Size(301, 13);
            this.progressBarImport.TabIndex = 8;
            this.progressBarImport.Visible = false;
            // 
            // labelImportInfo
            // 
            this.labelImportInfo.AutoSize = true;
            this.labelImportInfo.Location = new System.Drawing.Point(12, 228);
            this.labelImportInfo.Name = "labelImportInfo";
            this.labelImportInfo.Size = new System.Drawing.Size(0, 13);
            this.labelImportInfo.TabIndex = 9;
            // 
            // checkBoxAutoImport
            // 
            this.checkBoxAutoImport.AutoSize = true;
            this.checkBoxAutoImport.Checked = true;
            this.checkBoxAutoImport.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAutoImport.Location = new System.Drawing.Point(282, 206);
            this.checkBoxAutoImport.Name = "checkBoxAutoImport";
            this.checkBoxAutoImport.Size = new System.Drawing.Size(71, 17);
            this.checkBoxAutoImport.TabIndex = 10;
            this.checkBoxAutoImport.Text = "Automate";
            this.checkBoxAutoImport.UseVisualStyleBackColor = true;
            // 
            // eHPUM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 244);
            this.Controls.Add(this.checkBoxAutoImport);
            this.Controls.Add(this.labelImportInfo);
            this.Controls.Add(this.progressBarImport);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDownLimit);
            this.Controls.Add(this.panelProperties);
            this.Controls.Add(this.buttonImport);
            this.Controls.Add(this.treeViewLayout);
            this.Controls.Add(this.buttonChooseFile);
            this.Controls.Add(this.labelFile);
            this.Controls.Add(this.buttonClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "eHPUM";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Complex Excel Template Importer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.eHPUM_FormClosing);
            this.panelProperties.ResumeLayout(false);
            this.panelProperties.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLimit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetaControls.MetaButton buttonClose;
        private System.Windows.Forms.Label labelFile;
        private MetaControls.MetaButton buttonChooseFile;
        private System.Windows.Forms.TreeView treeViewLayout;
        private MetaControls.MetaButton buttonImport;
        private System.Windows.Forms.Panel panelProperties;
        private System.Windows.Forms.Label labelPropInfo;
        private System.Windows.Forms.Label labelAssInfo;
        private System.Windows.Forms.Label labelClassInfo;
        private System.Windows.Forms.ComboBox comboBoxAss;
        private System.Windows.Forms.ComboBox comboBoxClass;
        private System.Windows.Forms.Label labelWarning;
        private System.Windows.Forms.NumericUpDown numericUpDownLimit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBarImport;
        private System.Windows.Forms.Label labelImportInfo;
        private System.Windows.Forms.CheckBox checkBoxAutoImport;
    }
}