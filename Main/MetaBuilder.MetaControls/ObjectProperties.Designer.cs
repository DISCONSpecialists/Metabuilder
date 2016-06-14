namespace MetaBuilder.MetaControls
{
    partial class ObjectProperties
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label lblCaptionClass;
            this.lblClass = new System.Windows.Forms.Label();
            this.listDiagrams = new System.Windows.Forms.ListBox();
            this.metaPropertyGrid1 = new MetaBuilder.MetaControls.MetaPropertyGrid();
            this.lblCaptionStatus = new System.Windows.Forms.Label();
            this.lblCaptionWorkspace = new System.Windows.Forms.Label();
            this.lblCaptionMachine = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.grpObject = new System.Windows.Forms.GroupBox();
            this.lblPKID = new System.Windows.Forms.Label();
            this.lblMachine = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblWorkspace = new System.Windows.Forms.Label();
            this.tabOverview = new Syncfusion.Windows.Forms.Tools.TabControlAdv();
            this.tabPageOverview = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.btnViewInContext = new MetaControls.MetaButton();
            this.grpGraphFiles = new System.Windows.Forms.GroupBox();
            this.lblAsChild = new System.Windows.Forms.Label();
            this.lblCaptionAsChild = new System.Windows.Forms.Label();
            this.lblAsParent = new System.Windows.Forms.Label();
            this.lblCaptionAsParent = new System.Windows.Forms.Label();
            this.lblAsArtefact = new System.Windows.Forms.Label();
            this.lblCaptionAsArtefact = new System.Windows.Forms.Label();
            this.lblDiagrams = new System.Windows.Forms.Label();
            this.lblCaptionDiagrams = new System.Windows.Forms.Label();
            this.tabPageProperties = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.tabPageDiagrams = new Syncfusion.Windows.Forms.Tools.TabPageAdv();
            this.btnOpenSelectedDiagrams = new MetaControls.MetaButton();
            lblCaptionClass = new System.Windows.Forms.Label();
            this.grpObject.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabOverview)).BeginInit();
            this.tabOverview.SuspendLayout();
            this.tabPageOverview.SuspendLayout();
            this.grpGraphFiles.SuspendLayout();
            this.tabPageProperties.SuspendLayout();
            this.tabPageDiagrams.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCaptionClass
            // 
            lblCaptionClass.AutoSize = true;
            lblCaptionClass.Location = new System.Drawing.Point(35, 16);
            lblCaptionClass.Name = "lblCaptionClass";
            lblCaptionClass.Size = new System.Drawing.Size(35, 13);
            lblCaptionClass.TabIndex = 1;
            lblCaptionClass.Text = "Class:";
            // 
            // lblClass
            // 
            this.lblClass.AutoSize = true;
            this.lblClass.Location = new System.Drawing.Point(76, 16);
            this.lblClass.Name = "lblClass";
            this.lblClass.Size = new System.Drawing.Size(42, 13);
            this.lblClass.TabIndex = 1;
            this.lblClass.Text = "lblClass";
            // 
            // listDiagrams
            // 
            this.listDiagrams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listDiagrams.FormattingEnabled = true;
            this.listDiagrams.Location = new System.Drawing.Point(0, 0);
            this.listDiagrams.Name = "listDiagrams";
            this.listDiagrams.Size = new System.Drawing.Size(388, 368);
            this.listDiagrams.TabIndex = 2;
            // 
            // metaPropertyGrid1
            // 
            this.metaPropertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metaPropertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.metaPropertyGrid1.Name = "metaPropertyGrid1";
            this.metaPropertyGrid1.Size = new System.Drawing.Size(388, 412);
            this.metaPropertyGrid1.TabIndex = 0;
            // 
            // lblCaptionStatus
            // 
            this.lblCaptionStatus.AutoSize = true;
            this.lblCaptionStatus.Location = new System.Drawing.Point(33, 112);
            this.lblCaptionStatus.Name = "lblCaptionStatus";
            this.lblCaptionStatus.Size = new System.Drawing.Size(40, 13);
            this.lblCaptionStatus.TabIndex = 1;
            this.lblCaptionStatus.Text = "Status:";
            // 
            // lblCaptionWorkspace
            // 
            this.lblCaptionWorkspace.AutoSize = true;
            this.lblCaptionWorkspace.Location = new System.Drawing.Point(8, 88);
            this.lblCaptionWorkspace.Name = "lblCaptionWorkspace";
            this.lblCaptionWorkspace.Size = new System.Drawing.Size(65, 13);
            this.lblCaptionWorkspace.TabIndex = 1;
            this.lblCaptionWorkspace.Text = "Workspace:";
            // 
            // lblCaptionMachine
            // 
            this.lblCaptionMachine.AutoSize = true;
            this.lblCaptionMachine.Location = new System.Drawing.Point(22, 65);
            this.lblCaptionMachine.Name = "lblCaptionMachine";
            this.lblCaptionMachine.Size = new System.Drawing.Size(51, 13);
            this.lblCaptionMachine.TabIndex = 1;
            this.lblCaptionMachine.Text = "Machine:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(35, 41);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "PKID:";
            // 
            // grpObject
            // 
            this.grpObject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpObject.Controls.Add(this.lblClass);
            this.grpObject.Controls.Add(this.lblPKID);
            this.grpObject.Controls.Add(lblCaptionClass);
            this.grpObject.Controls.Add(this.lblMachine);
            this.grpObject.Controls.Add(this.label7);
            this.grpObject.Controls.Add(this.lblStatus);
            this.grpObject.Controls.Add(this.lblCaptionMachine);
            this.grpObject.Controls.Add(this.lblWorkspace);
            this.grpObject.Controls.Add(this.lblCaptionStatus);
            this.grpObject.Controls.Add(this.lblCaptionWorkspace);
            this.grpObject.Location = new System.Drawing.Point(3, 3);
            this.grpObject.Name = "grpObject";
            this.grpObject.Size = new System.Drawing.Size(382, 137);
            this.grpObject.TabIndex = 3;
            this.grpObject.TabStop = false;
            this.grpObject.Text = "Identifiers:";
            // 
            // lblPKID
            // 
            this.lblPKID.AutoSize = true;
            this.lblPKID.Location = new System.Drawing.Point(76, 41);
            this.lblPKID.Name = "lblPKID";
            this.lblPKID.Size = new System.Drawing.Size(42, 13);
            this.lblPKID.TabIndex = 1;
            this.lblPKID.Text = "lblPKID";
            // 
            // lblMachine
            // 
            this.lblMachine.AutoSize = true;
            this.lblMachine.Location = new System.Drawing.Point(76, 65);
            this.lblMachine.Name = "lblMachine";
            this.lblMachine.Size = new System.Drawing.Size(58, 13);
            this.lblMachine.TabIndex = 1;
            this.lblMachine.Text = "lblMachine";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(76, 112);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(47, 13);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "lblStatus";
            // 
            // lblWorkspace
            // 
            this.lblWorkspace.AutoSize = true;
            this.lblWorkspace.Location = new System.Drawing.Point(76, 88);
            this.lblWorkspace.Name = "lblWorkspace";
            this.lblWorkspace.Size = new System.Drawing.Size(72, 13);
            this.lblWorkspace.TabIndex = 1;
            this.lblWorkspace.Text = "lblWorkspace";
            // 
            // tabOverview
            // 
            this.tabOverview.ActiveTabFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.tabOverview.Controls.Add(this.tabPageOverview);
            this.tabOverview.Controls.Add(this.tabPageProperties);
            this.tabOverview.Controls.Add(this.tabPageDiagrams);
            this.tabOverview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabOverview.Location = new System.Drawing.Point(0, 0);
            this.tabOverview.Name = "tabOverview";
            this.tabOverview.Padding = new System.Drawing.Point(6, 0);
            this.tabOverview.Size = new System.Drawing.Size(391, 415);
            this.tabOverview.TabIndex = 4;
            this.tabOverview.TabStyle = typeof(Syncfusion.Windows.Forms.Tools.TabRendererWhidbey);
            // 
            // tabPageOverview
            // 
            this.tabPageOverview.Controls.Add(this.btnViewInContext);
            this.tabPageOverview.Controls.Add(this.grpObject);
            this.tabPageOverview.Controls.Add(this.grpGraphFiles);
            this.tabPageOverview.Location = new System.Drawing.Point(1, 22);
            this.tabPageOverview.Name = "tabPageOverview";
            this.tabPageOverview.Size = new System.Drawing.Size(388, 391);
            this.tabPageOverview.TabIndex = 0;
            this.tabPageOverview.Text = "Overview";
            // 
            // btnViewInContext
            // 
            this.btnViewInContext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnViewInContext.Location = new System.Drawing.Point(272, 270);
            this.btnViewInContext.Name = "btnViewInContext";
            this.btnViewInContext.Size = new System.Drawing.Size(113, 23);
            this.btnViewInContext.TabIndex = 4;
            this.btnViewInContext.Text = "View In Context";
            this.btnViewInContext.Click += new System.EventHandler(this.btnViewInContext_Click);
            // 
            // grpGraphFiles
            // 
            this.grpGraphFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpGraphFiles.Controls.Add(this.lblAsChild);
            this.grpGraphFiles.Controls.Add(this.lblCaptionAsChild);
            this.grpGraphFiles.Controls.Add(this.lblAsParent);
            this.grpGraphFiles.Controls.Add(this.lblCaptionAsParent);
            this.grpGraphFiles.Controls.Add(this.lblAsArtefact);
            this.grpGraphFiles.Controls.Add(this.lblCaptionAsArtefact);
            this.grpGraphFiles.Controls.Add(this.lblDiagrams);
            this.grpGraphFiles.Controls.Add(this.lblCaptionDiagrams);
            this.grpGraphFiles.Location = new System.Drawing.Point(3, 146);
            this.grpGraphFiles.Name = "grpGraphFiles";
            this.grpGraphFiles.Size = new System.Drawing.Size(382, 118);
            this.grpGraphFiles.TabIndex = 2;
            this.grpGraphFiles.TabStop = false;
            this.grpGraphFiles.Text = "Occurrences";
            this.grpGraphFiles.Enter += new System.EventHandler(this.grpGraphFiles_Enter);
            // 
            // lblAsChild
            // 
            this.lblAsChild.AutoSize = true;
            this.lblAsChild.Location = new System.Drawing.Point(76, 62);
            this.lblAsChild.Name = "lblAsChild";
            this.lblAsChild.Size = new System.Drawing.Size(52, 13);
            this.lblAsChild.TabIndex = 1;
            this.lblAsChild.Text = "lblAsChild";
            // 
            // lblCaptionAsChild
            // 
            this.lblCaptionAsChild.AutoSize = true;
            this.lblCaptionAsChild.Location = new System.Drawing.Point(22, 62);
            this.lblCaptionAsChild.Name = "lblCaptionAsChild";
            this.lblCaptionAsChild.Size = new System.Drawing.Size(48, 13);
            this.lblCaptionAsChild.TabIndex = 1;
            this.lblCaptionAsChild.Text = "As Child:";
            // 
            // lblAsParent
            // 
            this.lblAsParent.AutoSize = true;
            this.lblAsParent.Location = new System.Drawing.Point(76, 38);
            this.lblAsParent.Name = "lblAsParent";
            this.lblAsParent.Size = new System.Drawing.Size(60, 13);
            this.lblAsParent.TabIndex = 1;
            this.lblAsParent.Text = "lblAsParent";
            // 
            // lblCaptionAsParent
            // 
            this.lblCaptionAsParent.AutoSize = true;
            this.lblCaptionAsParent.Location = new System.Drawing.Point(14, 38);
            this.lblCaptionAsParent.Name = "lblCaptionAsParent";
            this.lblCaptionAsParent.Size = new System.Drawing.Size(56, 13);
            this.lblCaptionAsParent.TabIndex = 1;
            this.lblCaptionAsParent.Text = "As Parent:";
            // 
            // lblAsArtefact
            // 
            this.lblAsArtefact.AutoSize = true;
            this.lblAsArtefact.Location = new System.Drawing.Point(76, 87);
            this.lblAsArtefact.Name = "lblAsArtefact";
            this.lblAsArtefact.Size = new System.Drawing.Size(66, 13);
            this.lblAsArtefact.TabIndex = 1;
            this.lblAsArtefact.Text = "lblAsArtefact";
            // 
            // lblCaptionAsArtefact
            // 
            this.lblCaptionAsArtefact.AutoSize = true;
            this.lblCaptionAsArtefact.Location = new System.Drawing.Point(8, 87);
            this.lblCaptionAsArtefact.Name = "lblCaptionAsArtefact";
            this.lblCaptionAsArtefact.Size = new System.Drawing.Size(62, 13);
            this.lblCaptionAsArtefact.TabIndex = 1;
            this.lblCaptionAsArtefact.Text = "As Artefact:";
            // 
            // lblDiagrams
            // 
            this.lblDiagrams.AutoSize = true;
            this.lblDiagrams.Location = new System.Drawing.Point(76, 16);
            this.lblDiagrams.Name = "lblDiagrams";
            this.lblDiagrams.Size = new System.Drawing.Size(61, 13);
            this.lblDiagrams.TabIndex = 1;
            this.lblDiagrams.Text = "lblDiagrams";
            // 
            // lblCaptionDiagrams
            // 
            this.lblCaptionDiagrams.AutoSize = true;
            this.lblCaptionDiagrams.Location = new System.Drawing.Point(16, 16);
            this.lblCaptionDiagrams.Name = "lblCaptionDiagrams";
            this.lblCaptionDiagrams.Size = new System.Drawing.Size(54, 13);
            this.lblCaptionDiagrams.TabIndex = 1;
            this.lblCaptionDiagrams.Text = "Diagrams:";
            // 
            // tabPageProperties
            // 
            this.tabPageProperties.Controls.Add(this.metaPropertyGrid1);
            this.tabPageProperties.Location = new System.Drawing.Point(1, 1);
            this.tabPageProperties.Name = "tabPageProperties";
            this.tabPageProperties.Size = new System.Drawing.Size(388, 412);
            this.tabPageProperties.TabIndex = 1;
            this.tabPageProperties.Text = "Properties";
            // 
            // tabPageDiagrams
            // 
            this.tabPageDiagrams.Controls.Add(this.listDiagrams);
            this.tabPageDiagrams.Controls.Add(this.btnOpenSelectedDiagrams);
            this.tabPageDiagrams.Location = new System.Drawing.Point(1, 22);
            this.tabPageDiagrams.Name = "tabPageDiagrams";
            this.tabPageDiagrams.Size = new System.Drawing.Size(388, 391);
            this.tabPageDiagrams.TabIndex = 2;
            this.tabPageDiagrams.Text = "Diagrams";
            // 
            // btnOpenSelectedDiagrams
            // 
            this.btnOpenSelectedDiagrams.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnOpenSelectedDiagrams.Enabled = false;
            this.btnOpenSelectedDiagrams.Location = new System.Drawing.Point(0, 368);
            this.btnOpenSelectedDiagrams.Name = "btnOpenSelectedDiagrams";
            this.btnOpenSelectedDiagrams.Size = new System.Drawing.Size(388, 23);
            this.btnOpenSelectedDiagrams.TabIndex = 1;
            this.btnOpenSelectedDiagrams.Text = "Open Selected Diagram(s)";
            this.btnOpenSelectedDiagrams.Click += new System.EventHandler(this.btnOpenSelectedDiagrams_Click);
            // 
            // ObjectProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabOverview);
            this.Name = "ObjectProperties";
            this.Size = new System.Drawing.Size(391, 415);
            this.grpObject.ResumeLayout(false);
            this.grpObject.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabOverview)).EndInit();
            this.tabOverview.ResumeLayout(false);
            this.tabPageOverview.ResumeLayout(false);
            this.grpGraphFiles.ResumeLayout(false);
            this.grpGraphFiles.PerformLayout();
            this.tabPageProperties.ResumeLayout(false);
            this.tabPageDiagrams.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MetaPropertyGrid metaPropertyGrid1;
        private System.Windows.Forms.Label lblCaptionStatus;
        private System.Windows.Forms.Label lblCaptionWorkspace;
        private System.Windows.Forms.Label lblCaptionMachine;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox grpObject;
        private Syncfusion.Windows.Forms.Tools.TabControlAdv tabOverview;
        private Syncfusion.Windows.Forms.Tools.TabPageAdv tabPageOverview;
        private System.Windows.Forms.GroupBox grpGraphFiles;
        private System.Windows.Forms.Label lblCaptionAsArtefact;
        private System.Windows.Forms.Label lblCaptionDiagrams;
        private Syncfusion.Windows.Forms.Tools.TabPageAdv tabPageProperties;
        private Syncfusion.Windows.Forms.Tools.TabPageAdv tabPageDiagrams;
        private MetaControls.MetaButton btnOpenSelectedDiagrams;
        private MetaControls.MetaButton btnViewInContext;
        private System.Windows.Forms.Label lblPKID;
        private System.Windows.Forms.Label lblMachine;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblWorkspace;
        private System.Windows.Forms.Label lblAsArtefact;
        private System.Windows.Forms.Label lblDiagrams;
        private System.Windows.Forms.ListBox listDiagrams;
        private System.Windows.Forms.Label lblClass;
        private System.Windows.Forms.Label lblAsChild;
        private System.Windows.Forms.Label lblCaptionAsChild;
        private System.Windows.Forms.Label lblAsParent;
        private System.Windows.Forms.Label lblCaptionAsParent;
    }
}
