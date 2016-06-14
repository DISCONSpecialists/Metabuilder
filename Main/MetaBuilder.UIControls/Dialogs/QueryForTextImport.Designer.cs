namespace MetaBuilder.UIControls.Dialogs
{
    partial class QueryForTextImport
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
            this.label1 = new System.Windows.Forms.Label();
            this.classDropdown1 = new MetaBuilder.UIControls.Common.ClassDropdown();
            this.comboDefaultField = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOK = new MetaControls.MetaButton();
            this.btnCancel = new MetaControls.MetaButton();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Class to be Imported";
            // 
            // classDropdown1
            // 
            this.classDropdown1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.classDropdown1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.classDropdown1.Initializing = false;
            this.classDropdown1.Location = new System.Drawing.Point(150, 15);
            this.classDropdown1.Name = "classDropdown1";
            this.classDropdown1.SelectedClass = "";
            this.classDropdown1.Size = new System.Drawing.Size(176, 21);
            this.classDropdown1.TabIndex = 1;
            this.classDropdown1.SelectedIndexChanged += new System.EventHandler(this.classDropdown1_SelectedIndexChanged);
            // 
            // comboDefaultField
            // 
            this.comboDefaultField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDefaultField.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboDefaultField.Sorted = false;
            this.comboDefaultField.FormattingEnabled = true;
            this.comboDefaultField.Location = new System.Drawing.Point(150, 41);
            this.comboDefaultField.Name = "comboDefaultField";
            this.comboDefaultField.Size = new System.Drawing.Size(232, 21);
            this.comboDefaultField.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Default Field to Populate";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(150, 68);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(231, 68);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // QueryForTextImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 95);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboDefaultField);
            this.Controls.Add(this.classDropdown1);
            this.Controls.Add(this.label1);
            this.Name = "QueryForTextImport";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Text Import Specification";
            this.Load += new System.EventHandler(this.QueryForTextImport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private MetaBuilder.UIControls.Common.ClassDropdown classDropdown1;
        private System.Windows.Forms.ComboBox comboDefaultField;
        private System.Windows.Forms.Label label2;
        private MetaControls.MetaButton btnOK;
        private MetaControls.MetaButton btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}