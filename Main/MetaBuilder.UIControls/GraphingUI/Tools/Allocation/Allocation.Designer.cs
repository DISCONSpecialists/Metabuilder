namespace MetaBuilder.UIControls.GraphingUI.Tools.Allocation
{
    partial class Allocation
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
            this.buttonCancel = new MetaBuilder.MetaControls.MetaButton();
            this.buttonOK = new MetaBuilder.MetaControls.MetaButton();
            this.listBoxAllocation = new System.Windows.Forms.ListBox();
            this.buttonNew = new MetaBuilder.MetaControls.MetaButton();
            this.buttonDelete = new MetaBuilder.MetaControls.MetaButton();
            this.panelPathType = new System.Windows.Forms.Panel();
            this.radioButtonAbsolute = new System.Windows.Forms.RadioButton();
            this.radioButtonRelative = new System.Windows.Forms.RadioButton();
            this.panelPathType.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.CornerRadius = new Ascend.CornerRadius(2);
            this.buttonCancel.GradientLowColor = System.Drawing.Color.DarkGray;
            this.buttonCancel.Location = new System.Drawing.Point(221, 227);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.StayActiveAfterClick = false;
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.CornerRadius = new Ascend.CornerRadius(2);
            this.buttonOK.GradientLowColor = System.Drawing.Color.DarkGray;
            this.buttonOK.Location = new System.Drawing.Point(140, 227);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.StayActiveAfterClick = false;
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // listBoxAllocation
            // 
            this.listBoxAllocation.FormattingEnabled = true;
            this.listBoxAllocation.Location = new System.Drawing.Point(7, 8);
            this.listBoxAllocation.Name = "listBoxAllocation";
            this.listBoxAllocation.Size = new System.Drawing.Size(208, 212);
            this.listBoxAllocation.TabIndex = 2;
            this.listBoxAllocation.SelectedIndexChanged += new System.EventHandler(this.listBoxAllocation_SelectedIndexChanged);
            // 
            // buttonNew
            // 
            this.buttonNew.CornerRadius = new Ascend.CornerRadius(2);
            this.buttonNew.GradientLowColor = System.Drawing.Color.DarkGray;
            this.buttonNew.Location = new System.Drawing.Point(221, 8);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(75, 23);
            this.buttonNew.StayActiveAfterClick = false;
            this.buttonNew.TabIndex = 3;
            this.buttonNew.Text = "Add...";
            this.buttonNew.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.CornerRadius = new Ascend.CornerRadius(2);
            this.buttonDelete.GradientLowColor = System.Drawing.Color.DarkGray;
            this.buttonDelete.Location = new System.Drawing.Point(221, 37);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.StayActiveAfterClick = false;
            this.buttonDelete.TabIndex = 3;
            this.buttonDelete.Text = "Remove...";
            this.buttonDelete.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // panelPathType
            // 
            this.panelPathType.Controls.Add(this.radioButtonRelative);
            this.panelPathType.Controls.Add(this.radioButtonAbsolute);
            this.panelPathType.Location = new System.Drawing.Point(214, 66);
            this.panelPathType.Name = "panelPathType";
            this.panelPathType.Size = new System.Drawing.Size(82, 154);
            this.panelPathType.TabIndex = 4;
            // 
            // radioButtonAbsolute
            // 
            this.radioButtonAbsolute.AutoSize = true;
            this.radioButtonAbsolute.Location = new System.Drawing.Point(7, 3);
            this.radioButtonAbsolute.Name = "radioButtonAbsolute";
            this.radioButtonAbsolute.Size = new System.Drawing.Size(66, 17);
            this.radioButtonAbsolute.TabIndex = 0;
            this.radioButtonAbsolute.TabStop = true;
            this.radioButtonAbsolute.Text = "Absolute";
            this.radioButtonAbsolute.UseVisualStyleBackColor = true;
            this.radioButtonAbsolute.CheckedChanged += new System.EventHandler(this.radioButtonAbsolute_CheckedChanged);
            // 
            // radioButtonRelative
            // 
            this.radioButtonRelative.AutoSize = true;
            this.radioButtonRelative.Location = new System.Drawing.Point(7, 26);
            this.radioButtonRelative.Name = "radioButtonRelative";
            this.radioButtonRelative.Size = new System.Drawing.Size(64, 17);
            this.radioButtonRelative.TabIndex = 1;
            this.radioButtonRelative.TabStop = true;
            this.radioButtonRelative.Text = "Relative";
            this.radioButtonRelative.UseVisualStyleBackColor = true;
            this.radioButtonRelative.CheckedChanged += new System.EventHandler(this.radioButtonRelative_CheckedChanged);
            // 
            // Allocation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(299, 254);
            this.Controls.Add(this.panelPathType);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonNew);
            this.Controls.Add(this.listBoxAllocation);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Allocation";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Diagram Allocation";
            this.panelPathType.ResumeLayout(false);
            this.panelPathType.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MetaControls.MetaButton buttonCancel;
        private MetaControls.MetaButton buttonOK;
        private System.Windows.Forms.ListBox listBoxAllocation;
        private MetaControls.MetaButton buttonNew;
        private MetaControls.MetaButton buttonDelete;
        private System.Windows.Forms.Panel panelPathType;
        private System.Windows.Forms.RadioButton radioButtonRelative;
        private System.Windows.Forms.RadioButton radioButtonAbsolute;
    }
}