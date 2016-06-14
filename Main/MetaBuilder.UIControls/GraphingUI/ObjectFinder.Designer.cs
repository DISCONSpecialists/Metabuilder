namespace MetaBuilder.UIControls.GraphingUI
{
    partial class ObjectFinder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ObjectFinder));
            this.panel1 = new System.Windows.Forms.Panel();
            this.objectFinderControl1 = new MetaBuilder.MetaControls.ObjectFinderControl(Server, null);
            this.btnCancel = new MetaControls.MetaButton();
            this.chkLayout = new System.Windows.Forms.CheckBox();
            this.lblCount = new System.Windows.Forms.Label();
            this.btnOK = new MetaControls.MetaButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkLayout);
            this.panel1.Controls.Add(this.lblCount);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 330);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(645, 30);
            this.panel1.TabIndex = 0;
            // 
            // objectFinderControl1
            // 
            this.objectFinderControl1.AllowMultipleSelection = true;
            this.objectFinderControl1.AllowSaveAndLoad = true;
            this.objectFinderControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectFinderControl1.ExcludeStatuses = ((System.Collections.Generic.List<MetaBuilder.BusinessLogic.VCStatusList>)(resources.GetObject("objectFinderControl1.ExcludeStatuses")));
            this.objectFinderControl1.IncludeStatusCombo = false;
            this.objectFinderControl1.LimitToClass = null;
            this.objectFinderControl1.LimitToStatus = -1;
            this.objectFinderControl1.Location = new System.Drawing.Point(0, 0);
            this.objectFinderControl1.Name = "objectFinderControl1";
            this.objectFinderControl1.Size = new System.Drawing.Size(780, 330);
            this.objectFinderControl1.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(516, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(126, 20);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(384, 6);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(126, 20);
            this.btnOK.TabIndex = 18;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // chkLayout
            // 
            this.chkLayout.Text = "Auto Layout";
            this.chkLayout.ThreeState = false;
            this.chkLayout.AutoSize = true;
            this.chkLayout.Visible = false;
            this.chkLayout.Location = new System.Drawing.Point(7, 3);
            // 
            // lblCount
            // 
            this.lblCount.Text = "0 Selected Objects";
            this.lblCount.Visible = false;
            this.lblCount.AutoSize = true;
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCount.Location = new System.Drawing.Point(132, 3);
            // 
            // ObjectFinder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 360);
            this.Controls.Add(this.objectFinderControl1);
            this.Controls.Add(this.panel1);
            this.Name = "ObjectFinder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Object Finder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ObjectFinder_FormClosing);
            this.Load += new System.EventHandler(this.ObjectFinder_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private MetaBuilder.MetaControls.ObjectFinderControl objectFinderControl1;
        public MetaControls.MetaButton btnOK;
        private MetaControls.MetaButton btnCancel;
        private System.Windows.Forms.CheckBox chkLayout;
        private System.Windows.Forms.Label lblCount;
    }
}