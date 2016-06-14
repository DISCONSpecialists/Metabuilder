namespace MetaBuilder.UIControls.Dialogs
{
    partial class ChooseClass
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnOK = new MetaControls.MetaButton();
            this.btnCancel = new MetaControls.MetaButton();
            this.classDropdown1 = new MetaBuilder.UIControls.Common.ClassDropdown();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(256, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "For which class do you want to generate a template?";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(104, 48);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(184, 48);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
            // 
            // classDropdown1
            // 
            this.classDropdown1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.classDropdown1.Initializing = false;
            this.classDropdown1.Location = new System.Drawing.Point(4, 24);
            this.classDropdown1.Name = "classDropdown1";
            this.classDropdown1.SelectedClass = "classDropdown1";
            this.classDropdown1.Size = new System.Drawing.Size(252, 21);
            this.classDropdown1.TabIndex = 2;
            this.classDropdown1.Text = "classDropdown1";
            this.classDropdown1.KeyUp += new System.Windows.Forms.KeyEventHandler(classDropdown1_KeyUp);
            // 
            // ChooseClass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(260, 75);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.classDropdown1);
            this.Controls.Add(this.label1);
            this.Name = "ChooseClass";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Choose Template Class";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private MetaBuilder.UIControls.Common.ClassDropdown classDropdown1;
        private MetaControls.MetaButton btnOK;
        private MetaControls.MetaButton btnCancel;

    }
}