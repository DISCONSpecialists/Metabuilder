namespace MetaBuilder.UIControls.GraphingUI
{
    partial class ShapeBinding
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
            System.Drawing.StringFormat stringFormat1 = new System.Drawing.StringFormat();
            this.lblClass = new System.Windows.Forms.Label();
            this.btnCancel = new MetaControls.MetaButton();
            this.btnOK = new MetaControls.MetaButton();
            this.btnTestBindings = new MetaControls.MetaButton();
            this.dgBindings = new System.Windows.Forms.DataGridView();
            this.colLabel = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colProperty = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.btnCreateInstance = new MetaControls.MetaButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageThisShape = new System.Windows.Forms.TabPage();
            this.classDropdown1 = new MetaBuilder.UIControls.Common.ClassDropdown();
            this.propertyGrid1 = new MetaBuilder.MetaControls.MetaPropertyGrid();
            ((System.ComponentModel.ISupportInitialize)(this.dgBindings)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPageThisShape.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblClass
            // 
            this.lblClass.AutoSize = true;
            this.lblClass.Location = new System.Drawing.Point(6, 6);
            this.lblClass.Margin = new System.Windows.Forms.Padding(3);
            this.lblClass.Name = "lblClass";
            this.lblClass.Size = new System.Drawing.Size(112, 13);
            this.lblClass.TabIndex = 6;
            this.lblClass.Text = "Associate Shape with:";
            this.lblClass.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(415, 414);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(113, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(296, 414);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(113, 23);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnTestBindings
            // 
            this.btnTestBindings.Location = new System.Drawing.Point(177, 414);
            this.btnTestBindings.Name = "btnTestBindings";
            this.btnTestBindings.Size = new System.Drawing.Size(113, 23);
            this.btnTestBindings.TabIndex = 15;
            this.btnTestBindings.Text = "Test Bindings";
            this.btnTestBindings.Click += new System.EventHandler(this.btnTestBindings_Click);
            // 
            // dgBindings
            // 
            this.dgBindings.AllowUserToAddRows = false;
            this.dgBindings.AllowUserToDeleteRows = false;
            this.dgBindings.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dgBindings.CausesValidation = false;
            this.dgBindings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLabel,
            this.colProperty});
            this.dgBindings.Location = new System.Drawing.Point(6, 32);
            this.dgBindings.MultiSelect = false;
            this.dgBindings.Name = "dgBindings";
            this.dgBindings.ShowCellErrors = false;
            this.dgBindings.ShowCellToolTips = false;
            this.dgBindings.ShowEditingIcon = false;
            this.dgBindings.ShowRowErrors = false;
            this.dgBindings.Size = new System.Drawing.Size(496, 135);
            this.dgBindings.TabIndex = 17;
            this.dgBindings.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgBindings_CellContentClick);
            // 
            // colLabel
            // 
            this.colLabel.DataPropertyName = "Label";
            this.colLabel.HeaderText = "Label";
            this.colLabel.MinimumWidth = 100;
            this.colLabel.Name = "colLabel";
            this.colLabel.ReadOnly = true;
            this.colLabel.TrackVisitedState = false;
            // 
            // colProperty
            // 
            this.colProperty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colProperty.DataPropertyName = "Property";
            this.colProperty.HeaderText = "Property";
            this.colProperty.MinimumWidth = 100;
            this.colProperty.Name = "colProperty";
            // 
            // btnCreateInstance
            // 
            this.btnCreateInstance.Location = new System.Drawing.Point(389, 173);
            this.btnCreateInstance.Name = "btnCreateInstance";
            this.btnCreateInstance.Size = new System.Drawing.Size(113, 21);
            this.btnCreateInstance.TabIndex = 18;
            this.btnCreateInstance.Text = "Create Instance";
            this.btnCreateInstance.Click += new System.EventHandler(this.btnCreateInstance_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageThisShape);
            this.tabControl1.Location = new System.Drawing.Point(12, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(516, 402);
            this.tabControl1.TabIndex = 20;
            // 
            // tabPageThisShape
            // 
            this.tabPageThisShape.Controls.Add(this.dgBindings);
            this.tabPageThisShape.Controls.Add(this.btnCreateInstance);
            this.tabPageThisShape.Controls.Add(this.classDropdown1);
            this.tabPageThisShape.Controls.Add(this.propertyGrid1);
            this.tabPageThisShape.Controls.Add(this.lblClass);
            this.tabPageThisShape.Location = new System.Drawing.Point(4, 22);
            this.tabPageThisShape.Name = "tabPageThisShape";
            this.tabPageThisShape.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageThisShape.Size = new System.Drawing.Size(508, 376);
            this.tabPageThisShape.TabIndex = 0;
            this.tabPageThisShape.Text = "This Shape";
            this.tabPageThisShape.UseVisualStyleBackColor = true;
            // 
            // classDropdown1
            // 
            this.classDropdown1.Location = new System.Drawing.Point(124, 6);
            this.classDropdown1.Name = "classDropdown1";
            this.classDropdown1.SelectedClass = null;
            this.classDropdown1.Size = new System.Drawing.Size(378, 20);
            stringFormat1.Alignment = System.Drawing.StringAlignment.Near;
            stringFormat1.FormatFlags = ((System.Drawing.StringFormatFlags)(((System.Drawing.StringFormatFlags.FitBlackBox | System.Drawing.StringFormatFlags.NoWrap)
                        | System.Drawing.StringFormatFlags.NoClip)));
            stringFormat1.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat1.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat1.Trimming = System.Drawing.StringTrimming.Character;
            this.classDropdown1.TabIndex = 5;
            this.classDropdown1.Text = "classDropdown1";
            this.classDropdown1.SelectedIndexChanged += new System.EventHandler(this.classDropdown1_SelectedIndexChanged);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(6, 173);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(496, 197);
            this.propertyGrid1.TabIndex = 19;
            // 
            // ShapeBinding
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 443);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnTestBindings);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "ShapeBinding";
            this.ShowIcon = false;
            this.ShowInTaskbar = true;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "ShapeBinding";
            ((System.ComponentModel.ISupportInitialize)(this.dgBindings)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPageThisShape.ResumeLayout(false);
            this.tabPageThisShape.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblClass;
        private UIControls.Common.ClassDropdown classDropdown1;
        private MetaControls.MetaButton btnCancel;
        private MetaControls.MetaButton btnOK;
        private MetaControls.MetaButton btnTestBindings;
        private System.Windows.Forms.DataGridView dgBindings;
        private MetaControls.MetaButton btnCreateInstance;
        private MetaBuilder.MetaControls.MetaPropertyGrid propertyGrid1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageThisShape;
        private System.Windows.Forms.DataGridViewLinkColumn colLabel;
        private System.Windows.Forms.DataGridViewComboBoxColumn colProperty;
    }
}