namespace MetaBuilder.UIControls.Common
{
    partial class TempMappingExporter
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
            this.classDropdown1 = new MetaBuilder.UIControls.Common.ClassDropdown();
            this.classDropdown2 = new MetaBuilder.UIControls.Common.ClassDropdown();
            this.btnExport = new MetaControls.MetaButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboRelationshipType = new System.Windows.Forms.ComboBox();
            this.lblRelationshipType = new System.Windows.Forms.Label();
            this.btnCancel = new MetaControls.MetaButton();
            this.SuspendLayout();
            // 
            // classDropdown1
            // 
            this.classDropdown1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.classDropdown1.Initializing = false;
            this.classDropdown1.Location = new System.Drawing.Point(80, 12);
            this.classDropdown1.Name = "classDropdown1";
            this.classDropdown1.SelectedClass = "classDropdown1";
            this.classDropdown1.Size = new System.Drawing.Size(176, 21);
            this.classDropdown1.TabIndex = 0;
            this.classDropdown1.Text = "classDropdown1";
            // 
            // classDropdown2
            // 
            this.classDropdown2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.classDropdown2.Initializing = false;
            this.classDropdown2.Location = new System.Drawing.Point(80, 38);
            this.classDropdown2.Name = "classDropdown2";
            this.classDropdown2.SelectedClass = "classDropdown1";
            this.classDropdown2.Size = new System.Drawing.Size(176, 21);
            this.classDropdown2.TabIndex = 1;
            this.classDropdown2.Text = "classDropdown1";
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(100, 92);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 2;
            this.btnExport.Text = "Export";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Parent Class";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Child Class";
            // 
            // comboRelationshipType
            // 
            this.comboRelationshipType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboRelationshipType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboRelationshipType.FormattingEnabled = true;
            this.comboRelationshipType.Location = new System.Drawing.Point(80, 64);
            this.comboRelationshipType.Name = "comboRelationshipType";
            this.comboRelationshipType.Size = new System.Drawing.Size(176, 21);
            this.comboRelationshipType.TabIndex = 3;
            // 
            // lblRelationshipType
            // 
            this.lblRelationshipType.AutoSize = true;
            this.lblRelationshipType.Location = new System.Drawing.Point(12, 64);
            this.lblRelationshipType.Name = "lblRelationshipType";
            this.lblRelationshipType.Size = new System.Drawing.Size(65, 13);
            this.lblRelationshipType.TabIndex = 2;
            this.lblRelationshipType.Text = "Association";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(180, 92);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // TempMappingExporter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 119);
            this.ControlBox = false;
            this.Controls.Add(this.comboRelationshipType);
            this.Controls.Add(this.lblRelationshipType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.classDropdown2);
            this.Controls.Add(this.classDropdown1);
            this.Name = "TempMappingExporter";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Association Exporter";
            this.Load += new System.EventHandler(this.TempMappingExporter_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ClassDropdown classDropdown1;
        private ClassDropdown classDropdown2;
        private MetaControls.MetaButton btnExport;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboRelationshipType;
        private System.Windows.Forms.Label lblRelationshipType;
        private MetaControls.MetaButton btnCancel;
    }
}