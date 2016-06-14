namespace MetaBuilder.UIControls.Dialogs
{
    partial class PortManipulator
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
            this.comboBoxPortPosition = new System.Windows.Forms.ComboBox();
            this.buttonCancel = new MetaControls.MetaButton();
            this.buttonOK = new MetaControls.MetaButton();
            this.SuspendLayout();
            // 
            // comboBoxPortPosition
            // 
            this.comboBoxPortPosition.FormattingEnabled = true;
            this.comboBoxPortPosition.Location = new System.Drawing.Point(12, 12);
            this.comboBoxPortPosition.Name = "comboBoxPortPosition";
            this.comboBoxPortPosition.Size = new System.Drawing.Size(156, 21);
            this.comboBoxPortPosition.TabIndex = 0;
            this.comboBoxPortPosition.SelectedIndexChanged += new System.EventHandler(this.comboBoxPortPosition_SelectedIndexChanged);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(93, 39);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(12, 39);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // PortManipulator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(179, 73);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.comboBoxPortPosition);
            this.Name = "PortManipulator";
            this.Text = "PortManipulator";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxPortPosition;
        private MetaControls.MetaButton buttonCancel;
        private MetaControls.MetaButton buttonOK;
    }
}