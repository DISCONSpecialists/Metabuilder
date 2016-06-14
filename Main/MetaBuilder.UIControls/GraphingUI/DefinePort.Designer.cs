namespace MetaBuilder.UIControls.GraphingUI
{
    partial class DefinePort
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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnIncomingUp = new MetaControls.MetaButton();
            this.btnIncomingLeft = new MetaControls.MetaButton();
            this.btnIncomingRight = new MetaControls.MetaButton();
            this.btnIncomingDown = new MetaControls.MetaButton();
            this.btnIncomingAuto = new MetaControls.MetaButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnOutgoingUp = new MetaControls.MetaButton();
            this.btnOutgoingLeft = new MetaControls.MetaButton();
            this.btnOutgoingRight = new MetaControls.MetaButton();
            this.btnOutgoingDown = new MetaControls.MetaButton();
            this.btnOutgoingAuto = new MetaControls.MetaButton();
            this.lblIncomingLinks = new System.Windows.Forms.Label();
            this.lblOutgoingLinks = new System.Windows.Forms.Label();
            this.cbBehaviourAllowMultipleLinks = new System.Windows.Forms.CheckBox();
            this.cbValidSelfLinks = new System.Windows.Forms.CheckBox();
            this.cbAllowOutgoing = new System.Windows.Forms.CheckBox();
            this.cbAllowIncoming = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnClose = new MetaControls.MetaButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.tabPageAppearance = new System.Windows.Forms.TabPage();
            this.btnActivatePortMovingTool = new MetaControls.MetaButton();
            this.uiColorButton2 = new Janus.Windows.EditControls.UIColorButton();
            this.uiColorButton1 = new Janus.Windows.EditControls.UIColorButton();
            this.label6 = new System.Windows.Forms.Label();
            this.txtWidth = new System.Windows.Forms.NumericUpDown();
            this.txtHeight = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboStyle = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnNextPort = new MetaControls.MetaButton();
            this.btnPreviousPort = new MetaControls.MetaButton();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.tabPageAppearance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel2.Controls.Add(this.btnIncomingUp, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnIncomingLeft, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnIncomingRight, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnIncomingDown, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.btnIncomingAuto, 1, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(9, 19);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 51.72414F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48.27586F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(142, 91);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // btnIncomingUp
            // 
            this.btnIncomingUp.Location = new System.Drawing.Point(49, 3);
            this.btnIncomingUp.Name = "btnIncomingUp";
            this.btnIncomingUp.Size = new System.Drawing.Size(43, 23);
            this.btnIncomingUp.TabIndex = 0;
            this.btnIncomingUp.Text = "Up";
            this.btnIncomingUp.Click += new System.EventHandler(this.btnIncomingUp_Click);
            // 
            // btnIncomingLeft
            // 
            this.btnIncomingLeft.Location = new System.Drawing.Point(3, 33);
            this.btnIncomingLeft.Name = "btnIncomingLeft";
            this.btnIncomingLeft.Size = new System.Drawing.Size(40, 22);
            this.btnIncomingLeft.TabIndex = 1;
            this.btnIncomingLeft.Text = "Left";
            this.btnIncomingLeft.Click += new System.EventHandler(this.btnIncomingLeft_Click);
            // 
            // btnIncomingRight
            // 
            this.btnIncomingRight.Location = new System.Drawing.Point(98, 33);
            this.btnIncomingRight.Name = "btnIncomingRight";
            this.btnIncomingRight.Size = new System.Drawing.Size(40, 22);
            this.btnIncomingRight.TabIndex = 1;
            this.btnIncomingRight.Text = "Right";
            this.btnIncomingRight.Click += new System.EventHandler(this.btnIncomingRight_Click);
            // 
            // btnIncomingDown
            // 
            this.btnIncomingDown.Location = new System.Drawing.Point(49, 61);
            this.btnIncomingDown.Name = "btnIncomingDown";
            this.btnIncomingDown.Size = new System.Drawing.Size(43, 23);
            this.btnIncomingDown.TabIndex = 1;
            this.btnIncomingDown.Text = "Down";
            this.btnIncomingDown.Click += new System.EventHandler(this.btnIncomingDown_Click);
            // 
            // btnIncomingAuto
            // 
            this.btnIncomingAuto.Location = new System.Drawing.Point(49, 33);
            this.btnIncomingAuto.Name = "btnIncomingAuto";
            this.btnIncomingAuto.Size = new System.Drawing.Size(43, 22);
            this.btnIncomingAuto.TabIndex = 1;
            this.btnIncomingAuto.Text = "Auto";
            this.btnIncomingAuto.Click += new System.EventHandler(this.btnIncomingAuto_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel1.Controls.Add(this.btnOutgoingUp, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnOutgoingLeft, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnOutgoingRight, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnOutgoingDown, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnOutgoingAuto, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(191, 20);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 51.72414F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48.27586F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(142, 91);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnOutgoingUp
            // 
            this.btnOutgoingUp.Location = new System.Drawing.Point(49, 3);
            this.btnOutgoingUp.Name = "btnOutgoingUp";
            this.btnOutgoingUp.Size = new System.Drawing.Size(43, 23);
            this.btnOutgoingUp.TabIndex = 0;
            this.btnOutgoingUp.Text = "Up";
            this.btnOutgoingUp.Click += new System.EventHandler(this.btnOutgoingUp_Click);
            // 
            // btnOutgoingLeft
            // 
            this.btnOutgoingLeft.Location = new System.Drawing.Point(3, 33);
            this.btnOutgoingLeft.Name = "btnOutgoingLeft";
            this.btnOutgoingLeft.Size = new System.Drawing.Size(40, 22);
            this.btnOutgoingLeft.TabIndex = 1;
            this.btnOutgoingLeft.Text = "Left";
            this.btnOutgoingLeft.Click += new System.EventHandler(this.btnOutgoingLeft_Click);
            // 
            // btnOutgoingRight
            // 
            this.btnOutgoingRight.Location = new System.Drawing.Point(98, 33);
            this.btnOutgoingRight.Name = "btnOutgoingRight";
            this.btnOutgoingRight.Size = new System.Drawing.Size(40, 22);
            this.btnOutgoingRight.TabIndex = 1;
            this.btnOutgoingRight.Text = "Right";
            this.btnOutgoingRight.Click += new System.EventHandler(this.btnOutgoingRight_Click);
            // 
            // btnOutgoingDown
            // 
            this.btnOutgoingDown.Location = new System.Drawing.Point(49, 61);
            this.btnOutgoingDown.Name = "btnOutgoingDown";
            this.btnOutgoingDown.Size = new System.Drawing.Size(43, 23);
            this.btnOutgoingDown.TabIndex = 1;
            this.btnOutgoingDown.Text = "Down";
            this.btnOutgoingDown.Click += new System.EventHandler(this.btnOutgoingDown_Click);
            // 
            // btnOutgoingAuto
            // 
            this.btnOutgoingAuto.Location = new System.Drawing.Point(49, 33);
            this.btnOutgoingAuto.Name = "btnOutgoingAuto";
            this.btnOutgoingAuto.Size = new System.Drawing.Size(43, 22);
            this.btnOutgoingAuto.TabIndex = 1;
            this.btnOutgoingAuto.Text = "Auto";
            this.btnOutgoingAuto.Click += new System.EventHandler(this.btnOutgoingAuto_Click);
            // 
            // lblIncomingLinks
            // 
            this.lblIncomingLinks.AutoSize = true;
            this.lblIncomingLinks.Location = new System.Drawing.Point(6, 3);
            this.lblIncomingLinks.Name = "lblIncomingLinks";
            this.lblIncomingLinks.Size = new System.Drawing.Size(123, 13);
            this.lblIncomingLinks.TabIndex = 1;
            this.lblIncomingLinks.Text = "Incoming Links Direction";
            // 
            // lblOutgoingLinks
            // 
            this.lblOutgoingLinks.AutoSize = true;
            this.lblOutgoingLinks.Location = new System.Drawing.Point(188, 4);
            this.lblOutgoingLinks.Name = "lblOutgoingLinks";
            this.lblOutgoingLinks.Size = new System.Drawing.Size(123, 13);
            this.lblOutgoingLinks.TabIndex = 1;
            this.lblOutgoingLinks.Text = "Outgoing Links Direction";
            // 
            // cbBehaviourAllowMultipleLinks
            // 
            this.cbBehaviourAllowMultipleLinks.AutoSize = true;
            this.cbBehaviourAllowMultipleLinks.Checked = true;
            this.cbBehaviourAllowMultipleLinks.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbBehaviourAllowMultipleLinks.Location = new System.Drawing.Point(6, 19);
            this.cbBehaviourAllowMultipleLinks.Name = "cbBehaviourAllowMultipleLinks";
            this.cbBehaviourAllowMultipleLinks.Size = new System.Drawing.Size(118, 17);
            this.cbBehaviourAllowMultipleLinks.TabIndex = 2;
            this.cbBehaviourAllowMultipleLinks.Text = "Allow Multiple Links";
            this.cbBehaviourAllowMultipleLinks.UseVisualStyleBackColor = true;
            this.cbBehaviourAllowMultipleLinks.CheckedChanged += new System.EventHandler(this.cbBehaviourAllowMultipleLinks_CheckedChanged);
            // 
            // cbValidSelfLinks
            // 
            this.cbValidSelfLinks.AutoSize = true;
            this.cbValidSelfLinks.Location = new System.Drawing.Point(6, 42);
            this.cbValidSelfLinks.Name = "cbValidSelfLinks";
            this.cbValidSelfLinks.Size = new System.Drawing.Size(98, 17);
            this.cbValidSelfLinks.TabIndex = 2;
            this.cbValidSelfLinks.Text = "Valid Self Links";
            this.cbValidSelfLinks.UseVisualStyleBackColor = true;
            this.cbValidSelfLinks.CheckedChanged += new System.EventHandler(this.cbValidSelfLinks_CheckedChanged);
            // 
            // cbAllowOutgoing
            // 
            this.cbAllowOutgoing.AutoSize = true;
            this.cbAllowOutgoing.Checked = true;
            this.cbAllowOutgoing.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAllowOutgoing.Location = new System.Drawing.Point(156, 16);
            this.cbAllowOutgoing.Name = "cbAllowOutgoing";
            this.cbAllowOutgoing.Size = new System.Drawing.Size(125, 17);
            this.cbAllowOutgoing.TabIndex = 2;
            this.cbAllowOutgoing.Text = "Allow Outgoing Links";
            this.cbAllowOutgoing.UseVisualStyleBackColor = true;
            this.cbAllowOutgoing.CheckedChanged += new System.EventHandler(this.cbAllowOutgoing_CheckedChanged);
            // 
            // cbAllowIncoming
            // 
            this.cbAllowIncoming.AutoSize = true;
            this.cbAllowIncoming.Checked = true;
            this.cbAllowIncoming.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAllowIncoming.Location = new System.Drawing.Point(156, 39);
            this.cbAllowIncoming.Name = "cbAllowIncoming";
            this.cbAllowIncoming.Size = new System.Drawing.Size(125, 17);
            this.cbAllowIncoming.TabIndex = 2;
            this.cbAllowIncoming.Text = "Allow Incoming Links";
            this.cbAllowIncoming.UseVisualStyleBackColor = true;
            this.cbAllowIncoming.CheckedChanged += new System.EventHandler(this.cbAllowIncoming_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbBehaviourAllowMultipleLinks);
            this.groupBox1.Controls.Add(this.cbAllowIncoming);
            this.groupBox1.Controls.Add(this.cbValidSelfLinks);
            this.groupBox1.Controls.Add(this.cbAllowOutgoing);
            this.groupBox1.Location = new System.Drawing.Point(9, 116);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(327, 64);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Behaviour";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(272, 264);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageGeneral);
            this.tabControl1.Controls.Add(this.tabPageAppearance);
            this.tabControl1.Location = new System.Drawing.Point(4, 28);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(347, 232);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.lblIncomingLinks);
            this.tabPageGeneral.Controls.Add(this.tableLayoutPanel2);
            this.tabPageGeneral.Controls.Add(this.tableLayoutPanel1);
            this.tabPageGeneral.Controls.Add(this.groupBox1);
            this.tabPageGeneral.Controls.Add(this.lblOutgoingLinks);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(339, 206);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // tabPageAppearance
            // 
            this.tabPageAppearance.Controls.Add(this.btnActivatePortMovingTool);
            this.tabPageAppearance.Controls.Add(this.uiColorButton2);
            this.tabPageAppearance.Controls.Add(this.uiColorButton1);
            this.tabPageAppearance.Controls.Add(this.label6);
            this.tabPageAppearance.Controls.Add(this.txtWidth);
            this.tabPageAppearance.Controls.Add(this.txtHeight);
            this.tabPageAppearance.Controls.Add(this.label7);
            this.tabPageAppearance.Controls.Add(this.label3);
            this.tabPageAppearance.Controls.Add(this.comboStyle);
            this.tabPageAppearance.Controls.Add(this.label4);
            this.tabPageAppearance.Location = new System.Drawing.Point(4, 22);
            this.tabPageAppearance.Name = "tabPageAppearance";
            this.tabPageAppearance.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAppearance.Size = new System.Drawing.Size(339, 206);
            this.tabPageAppearance.TabIndex = 1;
            this.tabPageAppearance.Text = "Appearance";
            this.tabPageAppearance.UseVisualStyleBackColor = true;
            // 
            // btnActivatePortMovingTool
            // 
            this.btnActivatePortMovingTool.Location = new System.Drawing.Point(60, 112);
            this.btnActivatePortMovingTool.Name = "btnActivatePortMovingTool";
            this.btnActivatePortMovingTool.Size = new System.Drawing.Size(148, 23);
            this.btnActivatePortMovingTool.TabIndex = 13;
            this.btnActivatePortMovingTool.Text = "Activate Port Moving Tool";
            this.btnActivatePortMovingTool.Click += new System.EventHandler(this.btnActivatePortMovingTool_Click);
            // 
            // uiColorButton2
            // 
            // 
            // 
            // 
            this.uiColorButton2.ColorPicker.Location = new System.Drawing.Point(0, 0);
            this.uiColorButton2.ColorPicker.Name = "";
            this.uiColorButton2.ColorPicker.Size = new System.Drawing.Size(100, 100);
            this.uiColorButton2.ColorPicker.TabIndex = 0;
            this.uiColorButton2.ImageReplaceableColor = System.Drawing.Color.Empty;
            this.uiColorButton2.Location = new System.Drawing.Point(183, 6);
            this.uiColorButton2.Name = "uiColorButton2";
            this.uiColorButton2.Size = new System.Drawing.Size(116, 23);
            this.uiColorButton2.TabIndex = 12;
            this.uiColorButton2.Text = "uiColorButtonLine";
            this.uiColorButton2.SelectedColorChanged += new System.EventHandler(this.uiColorButton2_SelectedColorChanged);
            // 
            // uiColorButton1
            // 
            // 
            // 
            // 
            this.uiColorButton1.ColorPicker.Location = new System.Drawing.Point(0, 0);
            this.uiColorButton1.ColorPicker.Name = "";
            this.uiColorButton1.ColorPicker.Size = new System.Drawing.Size(100, 100);
            this.uiColorButton1.ColorPicker.TabIndex = 0;
            this.uiColorButton1.ImageReplaceableColor = System.Drawing.Color.Empty;
            this.uiColorButton1.Location = new System.Drawing.Point(61, 6);
            this.uiColorButton1.Name = "uiColorButton1";
            this.uiColorButton1.Size = new System.Drawing.Size(116, 23);
            this.uiColorButton1.TabIndex = 12;
            this.uiColorButton1.Text = "uiColorButtonFill";
            this.uiColorButton1.SelectedColorChanged += new System.EventHandler(this.uiColorButton1_SelectedColorChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Colours";
            // 
            // txtWidth
            // 
            this.txtWidth.Location = new System.Drawing.Point(63, 88);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(44, 20);
            this.txtWidth.TabIndex = 5;
            this.txtWidth.ValueChanged += new System.EventHandler(this.txtWidth_ValueChanged);
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(63, 62);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(44, 20);
            this.txtHeight.TabIndex = 5;
            this.txtHeight.ValueChanged += new System.EventHandler(this.txtHeight_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 90);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Width";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Style";
            // 
            // comboStyle
            // 
            this.comboStyle.FormattingEnabled = true;
            this.comboStyle.Items.AddRange(new object[] {
             Northwoods.Go.GoPortStyle.None,
                Northwoods.Go.GoPortStyle.Object,
                Northwoods.Go.GoPortStyle.Ellipse,
                Northwoods.Go.GoPortStyle.Triangle,
                Northwoods.Go.GoPortStyle.Rectangle,
                Northwoods.Go.GoPortStyle.Diamond,
                Northwoods.Go.GoPortStyle.Plus,
                Northwoods.Go.GoPortStyle.Times,
                Northwoods.Go.GoPortStyle.PlusTimes
            });
            this.comboStyle.Location = new System.Drawing.Point(63, 35);
            this.comboStyle.Name = "comboStyle";
            this.comboStyle.Size = new System.Drawing.Size(146, 21);
            this.comboStyle.TabIndex = 4;
            this.comboStyle.Text = "Rectangle";
            this.comboStyle.SelectedValueChanged += new System.EventHandler(this.comboStyle_SelectedValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Height";
            // 
            // btnNextPort
            // 
            this.btnNextPort.Location = new System.Drawing.Point(216, 0);
            this.btnNextPort.Name = "btnNextPort";
            this.btnNextPort.Size = new System.Drawing.Size(75, 23);
            this.btnNextPort.TabIndex = 4;
            this.btnNextPort.Text = "Next >>";
            this.btnNextPort.Click += new System.EventHandler(this.btnNextPort_Click);
            // 
            // btnPreviousPort
            // 
            this.btnPreviousPort.Location = new System.Drawing.Point(56, 0);
            this.btnPreviousPort.Name = "btnPreviousPort";
            this.btnPreviousPort.Size = new System.Drawing.Size(75, 23);
            this.btnPreviousPort.TabIndex = 4;
            this.btnPreviousPort.Text = "<< Prev";
            this.btnPreviousPort.Click += new System.EventHandler(this.btnPreviousPort_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(136, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Port Selector";
            // 
            // DefinePort
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(347, 290);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnPreviousPort);
            this.Controls.Add(this.btnNextPort);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DefinePort";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Define Port Properties";
            this.TopMost = true;
            this.Leave += new System.EventHandler(this.DefinePort_Leave);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageGeneral.PerformLayout();
            this.tabPageAppearance.ResumeLayout(false);
            this.tabPageAppearance.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHeight)).EndInit();
            this.ResumeLayout(false);
            this.TopMost = true;
            this.Focus();
            this.BringToFront();
            this.TopMost = false;
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private MetaControls.MetaButton btnIncomingUp;
        private MetaControls.MetaButton btnIncomingLeft;
        private MetaControls.MetaButton btnIncomingRight;
        private MetaControls.MetaButton btnIncomingDown;
        private MetaControls.MetaButton btnIncomingAuto;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private MetaControls.MetaButton btnOutgoingUp;
        private MetaControls.MetaButton btnOutgoingLeft;
        private MetaControls.MetaButton btnOutgoingRight;
        private MetaControls.MetaButton btnOutgoingDown;
        private MetaControls.MetaButton btnOutgoingAuto;
        private System.Windows.Forms.Label lblIncomingLinks;
        private System.Windows.Forms.Label lblOutgoingLinks;
        private System.Windows.Forms.CheckBox cbBehaviourAllowMultipleLinks;
        private System.Windows.Forms.CheckBox cbValidSelfLinks;
        private System.Windows.Forms.CheckBox cbAllowOutgoing;
        private System.Windows.Forms.CheckBox cbAllowIncoming;
        private System.Windows.Forms.GroupBox groupBox1;
        private MetaControls.MetaButton btnClose;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.TabPage tabPageAppearance;

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboStyle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown txtHeight;
        private System.Windows.Forms.NumericUpDown txtWidth;
        private Janus.Windows.EditControls.UIColorButton uiColorButton2;
        private Janus.Windows.EditControls.UIColorButton uiColorButton1;
        private MetaControls.MetaButton btnActivatePortMovingTool;
        private MetaControls.MetaButton btnNextPort;
        private MetaControls.MetaButton btnPreviousPort;
        private System.Windows.Forms.Label label1;

    }
}