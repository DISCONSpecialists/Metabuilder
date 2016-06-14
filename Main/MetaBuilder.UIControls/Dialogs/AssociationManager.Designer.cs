namespace MetaBuilder.UIControls.Dialogs
{
    partial class AssociationManager
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
            this.associationLocatorControl1 = new MetaBuilder.MetaControls.AssociationLocatorControl(Server);
            this.SuspendLayout();
            // 
            // associationLocatorControl1
            // 
            this.associationLocatorControl1.AllowMultipleSelection = false;
            this.associationLocatorControl1.AllowSaveAndLoad = false;
            this.associationLocatorControl1.IncludeStatusCombo = false;
            this.associationLocatorControl1.LimitToStatus = 0;
            this.associationLocatorControl1.Location = new System.Drawing.Point(12, -1);
            this.associationLocatorControl1.Name = "associationLocatorControl1";
            this.associationLocatorControl1.Size = new System.Drawing.Size(621, 404);
            this.associationLocatorControl1.TabIndex = 0;
            // 
            // AssociationManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 415);
            this.Controls.Add(this.associationLocatorControl1);
            this.Name = "AssociationManager";
            this.Text = "AssociationManager";
            this.ResumeLayout(false);

        }

        #endregion

        private MetaBuilder.MetaControls.AssociationLocatorControl associationLocatorControl1;
    }
}