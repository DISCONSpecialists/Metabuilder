namespace ShapeBuilding
{
    partial class Example
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
            this.buttonGetClasses = new System.Windows.Forms.Button();
            this.textBoxClassList = new System.Windows.Forms.TextBox();
            this.excelGridView = new System.Windows.Forms.DataGridView();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonExcelBrowse = new System.Windows.Forms.Button();
            this.textboxExcelFilename = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.comboBoxServerName = new System.Windows.Forms.ComboBox();
            this.buttonFindServers = new System.Windows.Forms.Button();
            this.comboBoxDatabaseName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panelCriteria = new System.Windows.Forms.Panel();
            this.comboBoxRightColumn = new System.Windows.Forms.ComboBox();
            this.comboBoxLeftColumn = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.listBoxChildFields = new System.Windows.Forms.ListBox();
            this.listBoxParentFields = new System.Windows.Forms.ListBox();
            this.buttonLoadClasses = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxAssociationType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxChildClass = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxParentClass = new System.Windows.Forms.ComboBox();
            this.panelErrors = new System.Windows.Forms.Panel();
            this.dataGridViewNonExistentObjects = new System.Windows.Forms.DataGridView();
            this.dataGridViewExistingAssociations = new System.Windows.Forms.DataGridView();
            this.buttonCloseErrorPanel = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.buttonAddObjects = new System.Windows.Forms.Button();
            this.progressBarObjectAddition = new System.Windows.Forms.ProgressBar();
            this.progressBarMain = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.excelGridView)).BeginInit();
            this.panelCriteria.SuspendLayout();
            this.panelErrors.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewNonExistentObjects)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExistingAssociations)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonGetClasses
            // 
            this.buttonGetClasses.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGetClasses.Location = new System.Drawing.Point(820, 10);
            this.buttonGetClasses.Name = "buttonGetClasses";
            this.buttonGetClasses.Size = new System.Drawing.Size(75, 23);
            this.buttonGetClasses.TabIndex = 0;
            this.buttonGetClasses.Text = "Get Classes";
            this.buttonGetClasses.UseVisualStyleBackColor = true;
            this.buttonGetClasses.Visible = false;
            this.buttonGetClasses.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxClassList
            // 
            this.textBoxClassList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxClassList.Location = new System.Drawing.Point(820, 39);
            this.textBoxClassList.Multiline = true;
            this.textBoxClassList.Name = "textBoxClassList";
            this.textBoxClassList.Size = new System.Drawing.Size(75, 212);
            this.textBoxClassList.TabIndex = 1;
            this.textBoxClassList.Visible = false;
            // 
            // excelGridView
            // 
            this.excelGridView.AllowUserToAddRows = false;
            this.excelGridView.AllowUserToDeleteRows = false;
            this.excelGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.excelGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.excelGridView.Location = new System.Drawing.Point(12, 39);
            this.excelGridView.Name = "excelGridView";
            this.excelGridView.ReadOnly = true;
            this.excelGridView.RowHeadersVisible = false;
            this.excelGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.excelGridView.Size = new System.Drawing.Size(802, 150);
            this.excelGridView.TabIndex = 2;
            // 
            // buttonStart
            // 
            this.buttonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStart.Location = new System.Drawing.Point(780, 265);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(97, 23);
            this.buttonStart.TabIndex = 3;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonExcelBrowse
            // 
            this.buttonExcelBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExcelBrowse.Location = new System.Drawing.Point(739, 10);
            this.buttonExcelBrowse.Name = "buttonExcelBrowse";
            this.buttonExcelBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonExcelBrowse.TabIndex = 4;
            this.buttonExcelBrowse.Text = "Browse";
            this.buttonExcelBrowse.UseVisualStyleBackColor = true;
            this.buttonExcelBrowse.Click += new System.EventHandler(this.button3_Click);
            // 
            // textboxExcelFilename
            // 
            this.textboxExcelFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textboxExcelFilename.Location = new System.Drawing.Point(12, 10);
            this.textboxExcelFilename.Name = "textboxExcelFilename";
            this.textboxExcelFilename.ReadOnly = true;
            this.textboxExcelFilename.Size = new System.Drawing.Size(721, 20);
            this.textboxExcelFilename.TabIndex = 5;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "xls";
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Excel Spreadsheet Files|*.xls";
            this.openFileDialog1.Title = "Select a microsoft excel spreadsheet";
            // 
            // comboBoxServerName
            // 
            this.comboBoxServerName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxServerName.FormattingEnabled = true;
            this.comboBoxServerName.Location = new System.Drawing.Point(71, 227);
            this.comboBoxServerName.Name = "comboBoxServerName";
            this.comboBoxServerName.Size = new System.Drawing.Size(662, 21);
            this.comboBoxServerName.TabIndex = 7;
            this.comboBoxServerName.Visible = false;
            this.comboBoxServerName.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // buttonFindServers
            // 
            this.buttonFindServers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonFindServers.Location = new System.Drawing.Point(12, 198);
            this.buttonFindServers.Name = "buttonFindServers";
            this.buttonFindServers.Size = new System.Drawing.Size(721, 23);
            this.buttonFindServers.TabIndex = 8;
            this.buttonFindServers.Text = "Find Servers";
            this.buttonFindServers.UseVisualStyleBackColor = true;
            this.buttonFindServers.Visible = false;
            this.buttonFindServers.Click += new System.EventHandler(this.button5_Click);
            // 
            // comboBoxDatabaseName
            // 
            this.comboBoxDatabaseName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxDatabaseName.FormattingEnabled = true;
            this.comboBoxDatabaseName.Location = new System.Drawing.Point(71, 254);
            this.comboBoxDatabaseName.Name = "comboBoxDatabaseName";
            this.comboBoxDatabaseName.Size = new System.Drawing.Size(662, 21);
            this.comboBoxDatabaseName.TabIndex = 10;
            this.comboBoxDatabaseName.Visible = false;
            this.comboBoxDatabaseName.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 230);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Server";
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 254);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Database";
            this.label2.Visible = false;
            // 
            // panelCriteria
            // 
            this.panelCriteria.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelCriteria.Controls.Add(this.progressBarMain);
            this.panelCriteria.Controls.Add(this.comboBoxRightColumn);
            this.panelCriteria.Controls.Add(this.comboBoxLeftColumn);
            this.panelCriteria.Controls.Add(this.label9);
            this.panelCriteria.Controls.Add(this.label8);
            this.panelCriteria.Controls.Add(this.listBoxChildFields);
            this.panelCriteria.Controls.Add(this.listBoxParentFields);
            this.panelCriteria.Controls.Add(this.buttonLoadClasses);
            this.panelCriteria.Controls.Add(this.label6);
            this.panelCriteria.Controls.Add(this.label7);
            this.panelCriteria.Controls.Add(this.comboBoxAssociationType);
            this.panelCriteria.Controls.Add(this.label5);
            this.panelCriteria.Controls.Add(this.comboBoxChildClass);
            this.panelCriteria.Controls.Add(this.label4);
            this.panelCriteria.Controls.Add(this.label3);
            this.panelCriteria.Controls.Add(this.comboBoxParentClass);
            this.panelCriteria.Controls.Add(this.buttonStart);
            this.panelCriteria.Location = new System.Drawing.Point(15, 281);
            this.panelCriteria.Name = "panelCriteria";
            this.panelCriteria.Size = new System.Drawing.Size(880, 291);
            this.panelCriteria.TabIndex = 13;
            this.panelCriteria.VisibleChanged += new System.EventHandler(this.panelCriteria_VisibleChanged);
            // 
            // comboBoxRightColumn
            // 
            this.comboBoxRightColumn.FormattingEnabled = true;
            this.comboBoxRightColumn.Location = new System.Drawing.Point(440, 46);
            this.comboBoxRightColumn.Name = "comboBoxRightColumn";
            this.comboBoxRightColumn.Size = new System.Drawing.Size(283, 21);
            this.comboBoxRightColumn.TabIndex = 18;
            this.comboBoxRightColumn.SelectedIndexChanged += new System.EventHandler(this.comboBox7_SelectedIndexChanged);
            // 
            // comboBoxLeftColumn
            // 
            this.comboBoxLeftColumn.FormattingEnabled = true;
            this.comboBoxLeftColumn.Location = new System.Drawing.Point(75, 46);
            this.comboBoxLeftColumn.Name = "comboBoxLeftColumn";
            this.comboBoxLeftColumn.Size = new System.Drawing.Size(283, 21);
            this.comboBoxLeftColumn.TabIndex = 17;
            this.comboBoxLeftColumn.SelectedIndexChanged += new System.EventHandler(this.comboBox6_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(364, 49);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Right Column";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 49);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Left Column";
            // 
            // listBoxChildFields
            // 
            this.listBoxChildFields.FormattingEnabled = true;
            this.listBoxChildFields.Location = new System.Drawing.Point(465, 100);
            this.listBoxChildFields.Name = "listBoxChildFields";
            this.listBoxChildFields.Size = new System.Drawing.Size(258, 69);
            this.listBoxChildFields.TabIndex = 14;
            // 
            // listBoxParentFields
            // 
            this.listBoxParentFields.FormattingEnabled = true;
            this.listBoxParentFields.Location = new System.Drawing.Point(100, 100);
            this.listBoxParentFields.Name = "listBoxParentFields";
            this.listBoxParentFields.Size = new System.Drawing.Size(258, 69);
            this.listBoxParentFields.TabIndex = 13;
            // 
            // buttonLoadClasses
            // 
            this.buttonLoadClasses.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLoadClasses.Location = new System.Drawing.Point(802, 3);
            this.buttonLoadClasses.Name = "buttonLoadClasses";
            this.buttonLoadClasses.Size = new System.Drawing.Size(75, 59);
            this.buttonLoadClasses.TabIndex = 12;
            this.buttonLoadClasses.Text = "Re-populate base classes";
            this.buttonLoadClasses.UseVisualStyleBackColor = true;
            this.buttonLoadClasses.Click += new System.EventHandler(this.button6_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(364, 103);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Fields";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Fields";
            // 
            // comboBoxAssociationType
            // 
            this.comboBoxAssociationType.FormattingEnabled = true;
            this.comboBoxAssociationType.Location = new System.Drawing.Point(367, 175);
            this.comboBoxAssociationType.Name = "comboBoxAssociationType";
            this.comboBoxAssociationType.Size = new System.Drawing.Size(258, 21);
            this.comboBoxAssociationType.TabIndex = 9;
            this.comboBoxAssociationType.SelectedIndexChanged += new System.EventHandler(this.comboBox5_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(273, 178);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Association Type";
            // 
            // comboBoxChildClass
            // 
            this.comboBoxChildClass.FormattingEnabled = true;
            this.comboBoxChildClass.Location = new System.Drawing.Point(465, 73);
            this.comboBoxChildClass.Name = "comboBoxChildClass";
            this.comboBoxChildClass.Size = new System.Drawing.Size(258, 21);
            this.comboBoxChildClass.TabIndex = 7;
            this.comboBoxChildClass.SelectedIndexChanged += new System.EventHandler(this.comboBox4_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(364, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Child Class";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Parent Class";
            // 
            // comboBoxParentClass
            // 
            this.comboBoxParentClass.FormattingEnabled = true;
            this.comboBoxParentClass.Location = new System.Drawing.Point(100, 73);
            this.comboBoxParentClass.Name = "comboBoxParentClass";
            this.comboBoxParentClass.Size = new System.Drawing.Size(258, 21);
            this.comboBoxParentClass.TabIndex = 4;
            this.comboBoxParentClass.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // panelErrors
            // 
            this.panelErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelErrors.Controls.Add(this.progressBarObjectAddition);
            this.panelErrors.Controls.Add(this.dataGridViewNonExistentObjects);
            this.panelErrors.Controls.Add(this.dataGridViewExistingAssociations);
            this.panelErrors.Controls.Add(this.buttonAddObjects);
            this.panelErrors.Controls.Add(this.label10);
            this.panelErrors.Controls.Add(this.buttonCloseErrorPanel);
            this.panelErrors.Location = new System.Drawing.Point(12, 10);
            this.panelErrors.Name = "panelErrors";
            this.panelErrors.Size = new System.Drawing.Size(883, 562);
            this.panelErrors.TabIndex = 14;
            this.panelErrors.Visible = false;
            // 
            // dataGridViewNonExistentObjects
            // 
            this.dataGridViewNonExistentObjects.AllowUserToAddRows = false;
            this.dataGridViewNonExistentObjects.AllowUserToDeleteRows = false;
            this.dataGridViewNonExistentObjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewNonExistentObjects.Dock = System.Windows.Forms.DockStyle.Left;
            this.dataGridViewNonExistentObjects.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewNonExistentObjects.Name = "dataGridViewNonExistentObjects";
            this.dataGridViewNonExistentObjects.ReadOnly = true;
            this.dataGridViewNonExistentObjects.Size = new System.Drawing.Size(320, 539);
            this.dataGridViewNonExistentObjects.TabIndex = 0;
            // 
            // dataGridViewExistingAssociations
            // 
            this.dataGridViewExistingAssociations.AllowUserToAddRows = false;
            this.dataGridViewExistingAssociations.AllowUserToDeleteRows = false;
            this.dataGridViewExistingAssociations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewExistingAssociations.Dock = System.Windows.Forms.DockStyle.Right;
            this.dataGridViewExistingAssociations.Location = new System.Drawing.Point(513, 0);
            this.dataGridViewExistingAssociations.Name = "dataGridViewExistingAssociations";
            this.dataGridViewExistingAssociations.ReadOnly = true;
            this.dataGridViewExistingAssociations.Size = new System.Drawing.Size(370, 539);
            this.dataGridViewExistingAssociations.TabIndex = 1;
            // 
            // buttonCloseErrorPanel
            // 
            this.buttonCloseErrorPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonCloseErrorPanel.Location = new System.Drawing.Point(0, 539);
            this.buttonCloseErrorPanel.Name = "buttonCloseErrorPanel";
            this.buttonCloseErrorPanel.Size = new System.Drawing.Size(883, 23);
            this.buttonCloseErrorPanel.TabIndex = 2;
            this.buttonCloseErrorPanel.Text = "Close";
            this.buttonCloseErrorPanel.UseVisualStyleBackColor = true;
            this.buttonCloseErrorPanel.Click += new System.EventHandler(this.buttonCloseErrorPanel_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(326, 23);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(173, 39);
            this.label10.TabIndex = 3;
            this.label10.Text = "<< Objects which do not exist\r\n\r\nAssociations which already exist >>";
            // 
            // buttonAddObjects
            // 
            this.buttonAddObjects.Location = new System.Drawing.Point(346, 75);
            this.buttonAddObjects.Name = "buttonAddObjects";
            this.buttonAddObjects.Size = new System.Drawing.Size(124, 39);
            this.buttonAddObjects.TabIndex = 5;
            this.buttonAddObjects.Text = "Add non-existent objects";
            this.buttonAddObjects.UseVisualStyleBackColor = true;
            this.buttonAddObjects.Click += new System.EventHandler(this.buttonAddObjects_Click);
            // 
            // progressBarObjectAddition
            // 
            this.progressBarObjectAddition.Location = new System.Drawing.Point(346, 120);
            this.progressBarObjectAddition.Maximum = 10;
            this.progressBarObjectAddition.Name = "progressBarObjectAddition";
            this.progressBarObjectAddition.Size = new System.Drawing.Size(124, 25);
            this.progressBarObjectAddition.Step = 1;
            this.progressBarObjectAddition.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarObjectAddition.TabIndex = 6;
            this.progressBarObjectAddition.Visible = false;
            // 
            // progressBarMain
            // 
            this.progressBarMain.Location = new System.Drawing.Point(9, 265);
            this.progressBarMain.Maximum = 10;
            this.progressBarMain.Name = "progressBarMain";
            this.progressBarMain.Size = new System.Drawing.Size(765, 23);
            this.progressBarMain.Step = 1;
            this.progressBarMain.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarMain.TabIndex = 19;
            // 
            // Example
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 584);
            this.Controls.Add(this.textboxExcelFilename);
            this.Controls.Add(this.panelCriteria);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxDatabaseName);
            this.Controls.Add(this.buttonFindServers);
            this.Controls.Add(this.comboBoxServerName);
            this.Controls.Add(this.buttonExcelBrowse);
            this.Controls.Add(this.excelGridView);
            this.Controls.Add(this.textBoxClassList);
            this.Controls.Add(this.buttonGetClasses);
            this.Controls.Add(this.panelErrors);
            this.Name = "Example";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Example";
            this.Load += new System.EventHandler(this.Example_Load);
            ((System.ComponentModel.ISupportInitialize)(this.excelGridView)).EndInit();
            this.panelCriteria.ResumeLayout(false);
            this.panelCriteria.PerformLayout();
            this.panelErrors.ResumeLayout(false);
            this.panelErrors.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewNonExistentObjects)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExistingAssociations)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonGetClasses;
        private System.Windows.Forms.TextBox textBoxClassList;
        private System.Windows.Forms.DataGridView excelGridView;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonExcelBrowse;
        private System.Windows.Forms.TextBox textboxExcelFilename;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ComboBox comboBoxServerName;
        private System.Windows.Forms.Button buttonFindServers;
        private System.Windows.Forms.ComboBox comboBoxDatabaseName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panelCriteria;
        private System.Windows.Forms.Button buttonLoadClasses;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxAssociationType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxChildClass;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxParentClass;
        private System.Windows.Forms.ListBox listBoxChildFields;
        private System.Windows.Forms.ListBox listBoxParentFields;
        private System.Windows.Forms.ComboBox comboBoxRightColumn;
        private System.Windows.Forms.ComboBox comboBoxLeftColumn;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panelErrors;
        private System.Windows.Forms.Button buttonCloseErrorPanel;
        private System.Windows.Forms.DataGridView dataGridViewExistingAssociations;
        private System.Windows.Forms.DataGridView dataGridViewNonExistentObjects;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button buttonAddObjects;
        private System.Windows.Forms.ProgressBar progressBarObjectAddition;
        private System.Windows.Forms.ProgressBar progressBarMain;
    }
}