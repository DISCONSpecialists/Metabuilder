namespace MetaBuilder.MetaControls
{
    partial class MergeObjects
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
            this.btnCancel = new MetaControls.MetaButton();
            this.columnModel1 = new XPTable.Models.ColumnModel();
            this.colProperty = new XPTable.Models.TextColumn();
            this.colValue = new XPTable.Models.TextColumn();
            this.tableModel1 = new XPTable.Models.TableModel();
            this.tableModel2 = new XPTable.Models.TableModel();
            this.btnCopyReplace = new MetaControls.MetaButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.objPropTarget = new MetaBuilder.MetaControls.ObjectProperties(Server);
            this.grpTargets = new System.Windows.Forms.GroupBox();
            this.btnAll = new MetaControls.MetaButton();
            this.listTargets = new System.Windows.Forms.CheckedListBox();
            this.objPropSource = new MetaBuilder.MetaControls.ObjectProperties(Server);
            this.grpReplaceWith = new System.Windows.Forms.GroupBox();
            this.listSource = new System.Windows.Forms.CheckedListBox();
            this.lblCounter = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPrevious = new MetaControls.MetaButton();
            this.btnNext = new MetaControls.MetaButton();
            this.buttonReplaceAll = new MetaControls.MetaButton();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.grpTargets.SuspendLayout();
            this.grpReplaceWith.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonReplaceAll
            // 
            this.buttonReplaceAll.Location = new System.Drawing.Point(280, 8);
            this.buttonReplaceAll.Name = "buttonReplaceAll";
            this.buttonReplaceAll.Size = new System.Drawing.Size(80, 24);
            this.buttonReplaceAll.TabIndex = 0;
            this.buttonReplaceAll.Visible = false;
            this.buttonReplaceAll.Text = "Replace all";
            this.buttonReplaceAll.Click += new System.EventHandler(this.buttonReplaceAll_Click);
            // 
            // btnPrevious
            // 
            this.btnPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrevious.Location = new System.Drawing.Point(370, 8);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(80, 24);
            this.btnPrevious.TabIndex = 1;
            this.btnPrevious.Text = "<< Previous";
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Location = new System.Drawing.Point(460, 8);// new System.Drawing.Point(476, 8);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(80, 24);
            this.btnNext.TabIndex = 1;
            this.btnNext.Text = "Next >>";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnCopyReplace
            // 
            this.btnCopyReplace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopyReplace.Location = new System.Drawing.Point(550, 8);
            this.btnCopyReplace.Name = "btnCopyReplace";
            this.btnCopyReplace.Size = new System.Drawing.Size(80, 24);
            this.btnCopyReplace.TabIndex = 3;
            this.btnCopyReplace.Text = "Merge";
            this.btnCopyReplace.Click += new System.EventHandler(this.btnCopyReplace_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(640, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 24);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // columnModel1
            // 
            this.columnModel1.Columns.AddRange(new XPTable.Models.Column[] {
            this.colProperty,
            this.colValue});
            // 
            // colProperty
            // 
            this.colProperty.Editable = false;
            this.colProperty.Text = "Property";
            this.colProperty.Width = 115;
            // 
            // colValue
            // 
            this.colValue.Editable = false;
            this.colValue.Text = "Value";
            this.colValue.Width = 150;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 28);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnCopyReplace);
            this.splitContainer1.Panel2.Controls.Add(this.lblCounter);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.btnPrevious);
            this.splitContainer1.Panel2.Controls.Add(this.btnNext);
            this.splitContainer1.Panel2.Controls.Add(this.btnCancel);
            this.splitContainer1.Panel2.Controls.Add(this.buttonReplaceAll);
            this.splitContainer1.Size = new System.Drawing.Size(725, 460);
            this.splitContainer1.SplitterDistance = 419;
            this.splitContainer1.TabIndex = 4;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.objPropTarget);
            this.splitContainer2.Panel1.Controls.Add(this.grpTargets);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.objPropSource);
            this.splitContainer2.Panel2.Controls.Add(this.grpReplaceWith);
            this.splitContainer2.Size = new System.Drawing.Size(725, 419);
            this.splitContainer2.SplitterDistance = 347;
            this.splitContainer2.TabIndex = 0;
            // 
            // objPropTarget
            // 
            this.objPropTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objPropTarget.Location = new System.Drawing.Point(0, 100);
            this.objPropTarget.MyMetaObject = null;
            this.objPropTarget.Name = "objPropTarget";
            this.objPropTarget.Size = new System.Drawing.Size(347, 319);
            this.objPropTarget.TabIndex = 11;
            // 
            // btnAll
            // 
            this.btnAll.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left);
            this.btnAll.Location = new System.Drawing.Point(250, 72);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(90, 20);
            this.btnAll.TabIndex = 12;
            this.btnAll.Text = "Select All";
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            //this.Controls.Add(this.btnAll);
            // 
            // grpTargets
            // 
            this.grpTargets.Controls.Add(this.listTargets);
            this.grpTargets.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpTargets.Location = new System.Drawing.Point(0, 0);
            this.grpTargets.Name = "grpTargets";
            this.grpTargets.Size = new System.Drawing.Size(347, 100);
            this.grpTargets.TabIndex = 10;
            this.grpTargets.TabStop = false;
            this.grpTargets.Text = "Reference(s)";
            // 
            // listTargets
            // 
            this.listTargets.CheckOnClick = true;
            this.listTargets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listTargets.FormattingEnabled = true;
            this.listTargets.Location = new System.Drawing.Point(3, 16);
            this.listTargets.Name = "listTargets";
            this.listTargets.Size = new System.Drawing.Size(341, 79);
            this.listTargets.TabIndex = 9;
            this.listTargets.SelectedIndexChanged += new System.EventHandler(this.listTargets_SelectedIndexChanged);
            this.listTargets.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listTargets_ItemCheck);
            // 
            // objPropSource
            // 
            this.objPropSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objPropSource.Location = new System.Drawing.Point(0, 100);
            this.objPropSource.MyMetaObject = null;
            this.objPropSource.Name = "objPropSource";
            this.objPropSource.Size = new System.Drawing.Size(374, 319);
            this.objPropSource.TabIndex = 13;
            // 
            // grpReplaceWith
            // 
            this.grpReplaceWith.Controls.Add(this.listSource);
            this.grpReplaceWith.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpReplaceWith.Location = new System.Drawing.Point(0, 0);
            this.grpReplaceWith.Name = "grpReplaceWith";
            this.grpReplaceWith.Size = new System.Drawing.Size(374, 100);
            this.grpReplaceWith.TabIndex = 12;
            this.grpReplaceWith.TabStop = false;
            this.grpReplaceWith.Text = "Master";
            // 
            // listSource
            // 
            this.listSource.CheckOnClick = true;
            this.listSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listSource.FormattingEnabled = true;
            this.listSource.Location = new System.Drawing.Point(3, 16);
            this.listSource.Name = "listSource";
            this.listSource.Size = new System.Drawing.Size(368, 79);
            this.listSource.TabIndex = 8;
            this.listSource.SelectedIndexChanged += new System.EventHandler(this.listSource_SelectedIndexChanged_1);
            this.listSource.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listSource_ItemCheck);
            // 
            // lblCounter
            // 
            this.lblCounter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCounter.AutoSize = true;
            this.lblCounter.Location = new System.Drawing.Point(12, 14);
            this.lblCounter.Name = "lblCounter";
            this.lblCounter.Size = new System.Drawing.Size(57, 13);
            this.lblCounter.TabIndex = 2;
            this.lblCounter.Text = "Item 0 of 0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 2;
            // 
            // MergeObjects
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 488);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer1);
            this.Name = "MergeObjects";
            this.Text = "Merge Objects";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Load += new System.EventHandler(this.MergeObjects_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.grpTargets.ResumeLayout(false);
            this.grpReplaceWith.ResumeLayout(false);

            this.btnAll.BringToFront();

            this.ResumeLayout(false);

        }

        #endregion

        private MetaControls.MetaButton btnCancel;
        private XPTable.Models.ColumnModel columnModel1;
        private XPTable.Models.TableModel tableModel1;
        private XPTable.Models.TableModel tableModel2;
        private XPTable.Models.TextColumn colProperty;
        private XPTable.Models.TextColumn colValue;
        private MetaControls.MetaButton btnCopyReplace;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private MetaControls.MetaButton btnNext;
        private System.Windows.Forms.Label lblCounter;
        private System.Windows.Forms.Label label1;
        private ObjectProperties objPropTarget;
        private System.Windows.Forms.GroupBox grpTargets;
        private System.Windows.Forms.CheckedListBox listTargets;
        private ObjectProperties objPropSource;
        private System.Windows.Forms.GroupBox grpReplaceWith;
        private System.Windows.Forms.CheckedListBox listSource;
        private MetaControls.MetaButton btnPrevious;
        private MetaControls.MetaButton btnAll;
        private MetaControls.MetaButton buttonReplaceAll;
    }
}