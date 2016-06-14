namespace MetaBuilder.UIControls.Dialogs.DatabaseManagement
{
    partial class ExportMultipleObjectsContext
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportMultipleObjectsContext));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExport = new MetaControls.MetaButton();
            this.objectFinderControl1 = new MetaBuilder.MetaControls.ObjectFinderControl(Server, null);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 359);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(601, 31);
            this.panel1.TabIndex = 1;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(3, 6);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(179, 19);
            this.btnExport.TabIndex = 16;
            this.btnExport.Text = "Export Selected Objects\' Context";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
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
            this.objectFinderControl1.Size = new System.Drawing.Size(601, 359);
            this.objectFinderControl1.TabIndex = 2;
            // 
            // ExportMultipleObjectsContext
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 390);
            this.Controls.Add(this.objectFinderControl1);
            this.Controls.Add(this.panel1);
            this.Name = "ExportMultipleObjectsContext";
            this.Text = "Export Object(s) Context";
            this.Load += new System.EventHandler(this.ManageMarkedForDeleteObjects_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private MetaControls.MetaButton btnExport;
        private MetaBuilder.MetaControls.ObjectFinderControl objectFinderControl1;
    }
}