namespace MetaBuilder.UIControls.Common
{
    partial class ChooseDiagram
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
            this.comboDiagrams = new System.Windows.Forms.ComboBox();
            this.btnCancel = new MetaControls.MetaButton();
            this.btnOK = new MetaControls.MetaButton();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboDiagrams
            // 
            this.comboDiagrams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDiagrams.FormattingEnabled = true;
            this.comboDiagrams.Location = new System.Drawing.Point(55, 3);
            this.comboDiagrams.Name = "comboDiagrams";
            this.comboDiagrams.Size = new System.Drawing.Size(387, 21);
            this.comboDiagrams.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(367, 30);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnOK.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(286, 30);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Diagram";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ChooseDiagram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 61);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.comboDiagrams);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChooseDiagram";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Choose Diagram";
            this.Load += new System.EventHandler(this.ChooseDiagram_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboDiagrams;
        private MetaControls.MetaButton btnCancel;
        private MetaControls.MetaButton btnOK;
        private System.Windows.Forms.Label label1;
    }
}