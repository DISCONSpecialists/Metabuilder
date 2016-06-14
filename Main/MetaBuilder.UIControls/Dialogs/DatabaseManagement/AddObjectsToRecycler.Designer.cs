namespace MetaBuilder.UIControls.Dialogs.DatabaseManagement
{
    partial class AddObjectsToRecycler
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddObjectsToRecycler));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDelete = new MetaControls.MetaButton();
            this.objectFinderControl1 = new MetaBuilder.MetaControls.ObjectFinderControl(Server, ServerWorkspacesUserHasWithAdminPermission);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 359);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(601, 31);
            this.panel1.TabIndex = 1;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(3, 6);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(179, 19);
            this.btnDelete.TabIndex = 16;
            this.btnDelete.Text = "Mark Selected Objects For Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // objectFinderControl1
            // 
            this.objectFinderControl1.AllowMultipleSelection = true;
            this.objectFinderControl1.AllowSaveAndLoad = true;
            this.objectFinderControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectFinderControl1.IncludeStatusCombo = true;
            this.objectFinderControl1.LimitToClass = null;
            this.objectFinderControl1.LimitToStatus = -1;
            this.objectFinderControl1.Location = new System.Drawing.Point(0, 0);
            this.objectFinderControl1.Name = "objectFinderControl1";
            this.objectFinderControl1.Size = new System.Drawing.Size(780, 359);
            this.objectFinderControl1.TabIndex = 2;
            // 
            // AddObjectsToRecycler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 390);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Controls.Add(this.objectFinderControl1);
            this.Controls.Add(this.panel1);
            this.Name = "AddObjectsToRecycler";
            this.ShowInTaskbar = true;
            this.Text = "Add Objects To Recycler";
            this.Load += new System.EventHandler(this.ManageMarkedForDeleteObjects_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private MetaControls.MetaButton btnDelete;
        private MetaBuilder.MetaControls.ObjectFinderControl objectFinderControl1;
    }
}