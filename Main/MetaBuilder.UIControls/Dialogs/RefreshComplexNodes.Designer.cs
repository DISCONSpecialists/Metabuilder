namespace MetaBuilder.UIControls.Dialogs
{
    partial class RefreshComplexNodes
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private MetaControls.MetaButton btnOK;
        private MetaControls.MetaButton btnCancel;
        private System.Windows.Forms.Label label1;
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
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.btnOK = new MetaControls.MetaButton();
            this.btnCancel = new MetaControls.MetaButton();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(3, 78);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(277, 229);
            this.checkedListBox1.TabIndex = 0;
            
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(3, 318);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(83, 318);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(268, 66);
            this.label1.TabIndex = 2;
            this.label1.Text = "The following complex nodes appear on your diagram. Checked items will be refresh" +
                "ed with child item data from the database. \r\n\r\nClick OK to proceed, or Cancel to" +
                " exit.";
            // 
            // RefreshComplexNodes
            // 
            this.ClientSize = new System.Drawing.Size(286, 353);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.checkedListBox1);
            this.Name = "RefreshComplexNodes";
            this.Text = "Refresh Complex Nodes";
            this.ResumeLayout(false);

        }

        #endregion

       
    }
}