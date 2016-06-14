namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.UI
{
    partial class ValidationResultForm
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
            MetaBuilder.CommonControls.Tree.TreeColumn treeColumn1 = new MetaBuilder.CommonControls.Tree.TreeColumn();
            MetaBuilder.CommonControls.Tree.TreeColumn treeColumn2 = new MetaBuilder.CommonControls.Tree.TreeColumn();
            this.treeViewAdv1 = new MetaBuilder.CommonControls.Tree.TreeViewAdv();
            this._name = new MetaBuilder.CommonControls.Tree.NodeControls.NodeTextBox();
            this.nodeTextBox1 = new MetaBuilder.CommonControls.Tree.NodeControls.NodeTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblPossibleActions = new System.Windows.Forms.Label();
            this.btnGo = new MetaControls.MetaButton();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pboxAugmentive = new System.Windows.Forms.PictureBox();
            this.pboxTransitive = new System.Windows.Forms.PictureBox();
            this.cbIndicateReflexive = new System.Windows.Forms.CheckBox();
            this.cbAug = new System.Windows.Forms.CheckBox();
            this.cbTrans = new System.Windows.Forms.CheckBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pboxAugmentive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pboxTransitive)).BeginInit();
            this.SuspendLayout();
            // 
            // treeViewAdv1
            // 
            this.treeViewAdv1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewAdv1.BackColor = System.Drawing.SystemColors.Window;
            treeColumn1.Header = "Item";
            treeColumn1.Width = 400;
            treeColumn2.Header = "Result";
            treeColumn2.Width = 150;
            this.treeViewAdv1.Columns.Add(treeColumn1);
            this.treeViewAdv1.Columns.Add(treeColumn2);
            this.treeViewAdv1.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeViewAdv1.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeViewAdv1.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeViewAdv1.Location = new System.Drawing.Point(0, 0);
            this.treeViewAdv1.Model = null;
            this.treeViewAdv1.Name = "treeViewAdv1";
            this.treeViewAdv1.NodeControls.Add(this._name);
            this.treeViewAdv1.NodeControls.Add(this.nodeTextBox1);
            this.treeViewAdv1.SelectedNode = null;
            this.treeViewAdv1.Size = new System.Drawing.Size(671, 425);
            this.treeViewAdv1.TabIndex = 6;
            this.treeViewAdv1.Text = "treeViewAdv1";
            this.treeViewAdv1.UseColumns = true;
            this.treeViewAdv1.NodeMouseDoubleClick += new System.EventHandler<MetaBuilder.CommonControls.Tree.TreeNodeAdvMouseEventArgs>(this.treeViewAdv1_NodeMouseDoubleClick);
            // 
            // _name
            // 
            this._name.DataPropertyName = "DisplayName";
            // 
            // nodeTextBox1
            // 
            this.nodeTextBox1.Column = 1;
            this.nodeTextBox1.DataPropertyName = "DisplayResult";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.treeViewAdv1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(671, 482);
            this.panel1.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblPossibleActions);
            this.panel2.Controls.Add(this.btnGo);
            this.panel2.Controls.Add(this.comboBox1);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Controls.Add(this.pboxAugmentive);
            this.panel2.Controls.Add(this.pboxTransitive);
            this.panel2.Controls.Add(this.cbIndicateReflexive);
            this.panel2.Controls.Add(this.cbAug);
            this.panel2.Controls.Add(this.cbTrans);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 428);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(671, 54);
            this.panel2.TabIndex = 8;
            // 
            // lblPossibleActions
            // 
            this.lblPossibleActions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPossibleActions.AutoSize = true;
            this.lblPossibleActions.Location = new System.Drawing.Point(202, 8);
            this.lblPossibleActions.Name = "lblPossibleActions";
            this.lblPossibleActions.Size = new System.Drawing.Size(87, 13);
            this.lblPossibleActions.TabIndex = 6;
            this.lblPossibleActions.Text = "Possible Actions:";
            // 
            // btnGo
            // 
            this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGo.Location = new System.Drawing.Point(422, 3);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 23);
            this.btnGo.TabIndex = 5;
            this.btnGo.Text = "Go";
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "FEBT - Visual",
            "FEBT - SID",
            "Synthesis - DSD"});
            this.comboBox1.Location = new System.Drawing.Point(295, 5);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 4;
            this.comboBox1.Text = "FEBT - Visual";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.Blue;
            this.pictureBox1.Location = new System.Drawing.Point(642, 32);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(17, 17);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // pboxAugmentive
            // 
            this.pboxAugmentive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pboxAugmentive.BackColor = System.Drawing.Color.Orange;
            this.pboxAugmentive.Location = new System.Drawing.Point(642, 18);
            this.pboxAugmentive.Name = "pboxAugmentive";
            this.pboxAugmentive.Size = new System.Drawing.Size(17, 17);
            this.pboxAugmentive.TabIndex = 3;
            this.pboxAugmentive.TabStop = false;
            this.pboxAugmentive.Click += new System.EventHandler(this.pboxAugmentive_Click);
            // 
            // pboxTransitive
            // 
            this.pboxTransitive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pboxTransitive.BackColor = System.Drawing.Color.Red;
            this.pboxTransitive.Location = new System.Drawing.Point(642, 3);
            this.pboxTransitive.Name = "pboxTransitive";
            this.pboxTransitive.Size = new System.Drawing.Size(17, 17);
            this.pboxTransitive.TabIndex = 2;
            this.pboxTransitive.TabStop = false;
            this.pboxTransitive.Click += new System.EventHandler(this.pboxTransitive_Click);
            // 
            // cbIndicateReflexive
            // 
            this.cbIndicateReflexive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbIndicateReflexive.AutoSize = true;
            this.cbIndicateReflexive.Checked = true;
            this.cbIndicateReflexive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIndicateReflexive.Location = new System.Drawing.Point(501, 34);
            this.cbIndicateReflexive.Name = "cbIndicateReflexive";
            this.cbIndicateReflexive.Size = new System.Drawing.Size(111, 17);
            this.cbIndicateReflexive.TabIndex = 1;
            this.cbIndicateReflexive.Text = "Indicate Reflexive";
            this.cbIndicateReflexive.UseVisualStyleBackColor = true;
            // 
            // cbAug
            // 
            this.cbAug.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAug.AutoSize = true;
            this.cbAug.Checked = true;
            this.cbAug.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAug.Location = new System.Drawing.Point(503, 17);
            this.cbAug.Name = "cbAug";
            this.cbAug.Size = new System.Drawing.Size(123, 17);
            this.cbAug.TabIndex = 1;
            this.cbAug.Text = "Indicate Augmentive";
            this.cbAug.UseVisualStyleBackColor = true;
            // 
            // cbTrans
            // 
            this.cbTrans.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbTrans.AutoSize = true;
            this.cbTrans.Checked = true;
            this.cbTrans.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTrans.Location = new System.Drawing.Point(503, 1);
            this.cbTrans.Name = "cbTrans";
            this.cbTrans.Size = new System.Drawing.Size(113, 17);
            this.cbTrans.TabIndex = 1;
            this.cbTrans.Text = "Indicate Transitive";
            this.cbTrans.UseVisualStyleBackColor = true;
            // 
            // ValidationResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 482);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ValidationResultForm";
            this.Text = "Validation Results";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pboxAugmentive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pboxTransitive)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        /*private CommonTools.TreeListView treeListView1;
        private CommonTools.ViewMap viewMap1;
        private CommonTools.TreeListView treeListView2;*/
        private MetaBuilder.CommonControls.Tree.TreeViewAdv treeViewAdv1;
        private MetaBuilder.CommonControls.Tree.NodeControls.NodeTextBox _name;
        private MetaBuilder.CommonControls.Tree.NodeControls.NodeTextBox nodeTextBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox cbAug;
        private System.Windows.Forms.CheckBox cbTrans;
        private System.Windows.Forms.PictureBox pboxAugmentive;
        private System.Windows.Forms.PictureBox pboxTransitive;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox cbIndicateReflexive;
        private System.Windows.Forms.ComboBox comboBox1;
        private MetaControls.MetaButton btnGo;
        private System.Windows.Forms.Label lblPossibleActions;
    }
}