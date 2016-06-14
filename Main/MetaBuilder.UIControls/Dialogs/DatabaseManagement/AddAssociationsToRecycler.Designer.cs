namespace MetaBuilder.UIControls.Dialogs.DatabaseManagement
{
    partial class AddAssociationsToRecycler
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddAssociationsToRecycler));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDelete = new MetaControls.MetaButton();
            this.associationLocatorControl1 = new MetaBuilder.MetaControls.AssociationLocatorControl(Server);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 360);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(601, 30);
            this.panel1.TabIndex = 1;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(3, 6);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(221, 19);
            this.btnDelete.TabIndex = 16;
            this.btnDelete.Text = "Mark Selected Associations For Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // associationLocatorControl1
            // 
            this.associationLocatorControl1.AllowMultipleSelection = true;
            this.associationLocatorControl1.AllowSaveAndLoad = true;
            this.associationLocatorControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.associationLocatorControl1.IncludeStatusCombo = true;
            this.associationLocatorControl1.LimitToStatus = -1;
            this.associationLocatorControl1.Location = new System.Drawing.Point(0, 0);
            this.associationLocatorControl1.Name = "associationLocatorControl1";
            this.associationLocatorControl1.SelectedAssociations = null;
            this.associationLocatorControl1.Size = new System.Drawing.Size(601, 360);
            this.associationLocatorControl1.TabIndex = 2;
            // 
            // AddAssociationsToRecycler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 390);
            this.Controls.Add(this.associationLocatorControl1);
            this.Controls.Add(this.panel1);
            this.Name = "AddAssociationsToRecycler";
            this.ShowInTaskbar = true;
            this.Text = "Add Associations To Recycler";
            this.Load += new System.EventHandler(this.AddAssociationsToRecycler_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private MetaControls.MetaButton btnDelete;
        private MetaBuilder.MetaControls.AssociationLocatorControl associationLocatorControl1;
    }
}