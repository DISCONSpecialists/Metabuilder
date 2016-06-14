namespace MetaBuilder.UIControls.GraphingUI
{
    sealed partial class CustomBox
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
            this.buttonOK = new MetaControls.MetaButton();
            this.buttonCancel = new MetaControls.MetaButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelOK = new System.Windows.Forms.Label();
            this.buttonNo = new MetaControls.MetaButton();
            this.labelNO = new System.Windows.Forms.Label();
            this.labelCancel = new System.Windows.Forms.Label();
            this.labelMessage = new System.Windows.Forms.Label();
            this.iconBox = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonOK.Location = new System.Drawing.Point(110, 0);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 25);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonCancel.Location = new System.Drawing.Point(304, 0);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 25);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonOK);
            this.panel1.Controls.Add(this.labelOK);
            this.panel1.Controls.Add(this.buttonNo);
            this.panel1.Controls.Add(this.labelNO);
            this.panel1.Controls.Add(this.buttonCancel);
            this.panel1.Controls.Add(this.labelCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 100);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(401, 25);
            this.panel1.TabIndex = 2;
            // 
            // labelOK
            // 
            this.labelOK.AutoSize = true;
            this.labelOK.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelOK.Location = new System.Drawing.Point(185, 0);
            this.labelOK.Name = "labelOK";
            this.labelOK.Size = new System.Drawing.Size(22, 13);
            this.labelOK.TabIndex = 2;
            this.labelOK.Text = "     ";
            // 
            // buttonNo
            // 
            this.buttonNo.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonNo.Location = new System.Drawing.Point(207, 0);
            this.buttonNo.Name = "buttonNo";
            this.buttonNo.Size = new System.Drawing.Size(75, 25);
            this.buttonNo.TabIndex = 4;
            this.buttonNo.Text = "No";
            this.buttonNo.Click += new System.EventHandler(this.buttonNo_Click);
            // 
            // labelNO
            // 
            this.labelNO.AutoSize = true;
            this.labelNO.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelNO.Location = new System.Drawing.Point(282, 0);
            this.labelNO.Name = "labelNO";
            this.labelNO.Size = new System.Drawing.Size(22, 13);
            this.labelNO.TabIndex = 5;
            this.labelNO.Text = "     ";
            // 
            // labelCancel
            // 
            this.labelCancel.AutoSize = true;
            this.labelCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelCancel.Location = new System.Drawing.Point(379, 0);
            this.labelCancel.Name = "labelCancel";
            this.labelCancel.Size = new System.Drawing.Size(22, 13);
            this.labelCancel.TabIndex = 3;
            this.labelCancel.Text = "     ";
            // 
            // labelMessage
            // 
            this.labelMessage.Location = new System.Drawing.Point(83, 2);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(318, 95);
            this.labelMessage.TabIndex = 3;
            this.labelMessage.Text = "Custom Message";
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // iconBox
            // 
            this.iconBox.Location = new System.Drawing.Point(12, 21);
            this.iconBox.Name = "iconBox";
            this.iconBox.Size = new System.Drawing.Size(65, 60);
            this.iconBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.iconBox.TabIndex = 4;
            this.iconBox.TabStop = false;
            // 
            // CustomBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 125);
            this.Controls.Add(this.iconBox);
            this.Controls.Add(this.labelMessage);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CustomBox";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Custom Caption";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MetaControls.MetaButton buttonOK;
        private MetaControls.MetaButton buttonCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelOK;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.PictureBox iconBox;
        private System.Windows.Forms.Label labelCancel;
        private MetaControls.MetaButton buttonNo;
        private System.Windows.Forms.Label labelNO;
    }
}