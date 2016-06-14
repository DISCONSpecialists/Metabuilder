namespace MetaBuilder.UIControls.Dialogs
{
    partial class DebugInfo
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
            this.btnOK = new MetaControls.MetaButton();
            this.txtDebugInfo = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(344, 233);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtDebugInfo
            // 
            this.txtDebugInfo.Location = new System.Drawing.Point(13, 13);
            this.txtDebugInfo.Multiline = true;
            this.txtDebugInfo.Name = "txtDebugInfo";
            this.txtDebugInfo.Size = new System.Drawing.Size(406, 214);
            this.txtDebugInfo.TabIndex = 1;
            // 
            // DebugInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 268);
            this.Controls.Add(this.txtDebugInfo);
            this.Controls.Add(this.btnOK);
            this.Name = "DebugInfo";
            this.Text = "DebugInfo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetaControls.MetaButton btnOK;
        private System.Windows.Forms.TextBox txtDebugInfo;
    }
}