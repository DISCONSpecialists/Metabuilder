namespace MetaBuilder.UIControls.GraphingUI
{
    partial class PortFormatter
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
            this.btnPrev = new MetaControls.MetaButton();
            this.btnNext = new MetaControls.MetaButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboStyle = new System.Windows.Forms.ComboBox();
            this.txtHeight = new System.Windows.Forms.NumericUpDown();
            this.txtWidth = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.colorButtonFill = new Janus.Windows.EditControls.UIColorButton();
            this.colorButtonLine = new Janus.Windows.EditControls.UIColorButton();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cbOut = new System.Windows.Forms.CheckBox();
            this.cbIn = new System.Windows.Forms.CheckBox();
            this.btnPortMover = new MetaControls.MetaButton();
            this.btnNextObject = new MetaControls.MetaButton();
            this.btnPrevObject = new MetaControls.MetaButton();
            this.lblSelectObject = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.txtHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWidth)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(86, 7);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(56, 23);
            this.btnPrev.TabIndex = 0;
            this.btnPrev.Text = "<< Prev";
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(148, 7);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(55, 23);
            this.btnNext.TabIndex = 1;
            this.btnNext.Text = "Next >>";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select Port:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Style";
            // 
            // comboStyle
            // 
            this.comboStyle.FormattingEnabled = true;
            this.comboStyle.Location = new System.Drawing.Point(50, 47);
            this.comboStyle.Name = "comboStyle";
            this.comboStyle.Size = new System.Drawing.Size(146, 21);
            this.comboStyle.TabIndex = 4;
            this.comboStyle.SelectedIndexChanged += new System.EventHandler(this.comboStyle_SelectedIndexChanged);
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(50, 128);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(44, 20);
            this.txtHeight.TabIndex = 5;
            this.txtHeight.ValueChanged += new System.EventHandler(this.txtHeight_ValueChanged);
            // 
            // txtWidth
            // 
            this.txtWidth.Location = new System.Drawing.Point(50, 154);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(44, 20);
            this.txtWidth.TabIndex = 5;
            this.txtWidth.ValueChanged += new System.EventHandler(this.txtWidth_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Width";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 130);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Height";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblSelectObject);
            this.groupBox1.Controls.Add(this.btnNextObject);
            this.groupBox1.Controls.Add(this.btnPrevObject);
            this.groupBox1.Controls.Add(this.colorButtonFill);
            this.groupBox1.Controls.Add(this.colorButtonLine);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cbOut);
            this.groupBox1.Controls.Add(this.cbIn);
            this.groupBox1.Controls.Add(this.comboStyle);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtHeight);
            this.groupBox1.Controls.Add(this.txtWidth);
            this.groupBox1.Location = new System.Drawing.Point(12, 36);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(353, 181);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Appearance";
            // 
            // colorButtonFill
            // 
            // 
            // 
            // 
            this.colorButtonFill.ColorPicker.Location = new System.Drawing.Point(0, 0);
            this.colorButtonFill.ColorPicker.Name = "";
            this.colorButtonFill.ColorPicker.Size = new System.Drawing.Size(100, 100);
            this.colorButtonFill.ColorPicker.TabIndex = 0;
            this.colorButtonFill.ImageReplaceableColor = System.Drawing.Color.Empty;
            this.colorButtonFill.Location = new System.Drawing.Point(139, 18);
            this.colorButtonFill.Name = "colorButtonFill";
            this.colorButtonFill.Size = new System.Drawing.Size(75, 23);
            this.colorButtonFill.TabIndex = 14;
            this.colorButtonFill.Text = "Fill";
            this.colorButtonFill.UseColorName = false;
            this.colorButtonFill.SelectedColorChanged += new System.EventHandler(this.colorButtonFill_SelectedColorChanged);
            // 
            // colorButtonLine
            // 
            // 
            // 
            // 
            this.colorButtonLine.ColorPicker.Location = new System.Drawing.Point(0, 0);
            this.colorButtonLine.ColorPicker.Name = "";
            this.colorButtonLine.ColorPicker.Size = new System.Drawing.Size(100, 100);
            this.colorButtonLine.ColorPicker.TabIndex = 0;
            this.colorButtonLine.ImageReplaceableColor = System.Drawing.Color.Empty;
            this.colorButtonLine.Location = new System.Drawing.Point(58, 18);
            this.colorButtonLine.Name = "colorButtonLine";
            this.colorButtonLine.Size = new System.Drawing.Size(75, 23);
            this.colorButtonLine.TabIndex = 12;
            this.colorButtonLine.Text = "Line";
            this.colorButtonLine.UseColorName = false;
            this.colorButtonLine.SelectedColorChanged += new System.EventHandler(this.colorButtonLine_SelectedColorChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Colours";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Allow";
            // 
            // cbOut
            // 
            this.cbOut.AutoSize = true;
            this.cbOut.Location = new System.Drawing.Point(50, 102);
            this.cbOut.Name = "cbOut";
            this.cbOut.Size = new System.Drawing.Size(105, 17);
            this.cbOut.TabIndex = 8;
            this.cbOut.Text = "Connections Out";
            this.cbOut.UseVisualStyleBackColor = true;
            this.cbOut.CheckedChanged += new System.EventHandler(this.cbOut_CheckedChanged);
            // 
            // cbIn
            // 
            this.cbIn.AutoSize = true;
            this.cbIn.Location = new System.Drawing.Point(50, 74);
            this.cbIn.Name = "cbIn";
            this.cbIn.Size = new System.Drawing.Size(97, 17);
            this.cbIn.TabIndex = 7;
            this.cbIn.Text = "Connections In";
            this.cbIn.UseVisualStyleBackColor = true;
            this.cbIn.CheckedChanged += new System.EventHandler(this.cbIn_CheckedChanged);
            // 
            // btnPortMover
            // 
            this.btnPortMover.Location = new System.Drawing.Point(12, 223);
            this.btnPortMover.Name = "btnPortMover";
            this.btnPortMover.Size = new System.Drawing.Size(94, 23);
            this.btnPortMover.TabIndex = 8;
            this.btnPortMover.Text = "Port Mover Tool";
            this.btnPortMover.Click += new System.EventHandler(this.btnPortMover_Click);
            // 
            // btnNextObject
            // 
            this.btnNextObject.Enabled = false;
            this.btnNextObject.Location = new System.Drawing.Point(264, 66);
            this.btnNextObject.Name = "btnNextObject";
            this.btnNextObject.Size = new System.Drawing.Size(55, 23);
            this.btnNextObject.TabIndex = 16;
            this.btnNextObject.Text = "Next >>";
            this.btnNextObject.Visible = false;
            this.btnNextObject.Click += new System.EventHandler(this.btnNextObject_Click);
            // 
            // btnPrevObject
            // 
            this.btnPrevObject.Enabled = false;
            this.btnPrevObject.Location = new System.Drawing.Point(202, 66);
            this.btnPrevObject.Name = "btnPrevObject";
            this.btnPrevObject.Size = new System.Drawing.Size(56, 23);
            this.btnPrevObject.TabIndex = 15;
            this.btnPrevObject.Text = "<< Prev";
            this.btnPrevObject.Visible = false;
            this.btnPrevObject.Click += new System.EventHandler(this.btnPrevObject_Click);
            // 
            // lblSelectObject
            // 
            this.lblSelectObject.AutoSize = true;
            this.lblSelectObject.Location = new System.Drawing.Point(202, 50);
            this.lblSelectObject.Name = "lblSelectObject";
            this.lblSelectObject.Size = new System.Drawing.Size(74, 13);
            this.lblSelectObject.TabIndex = 17;
            this.lblSelectObject.Text = "Select Object:";
            this.lblSelectObject.Visible = false;
            // 
            // PortFormatter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 253);
            this.Controls.Add(this.btnPortMover);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PortFormatter";
            this.ShowIcon = false;
            this.ShowInTaskbar = true;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Port Formatter";
            this.Leave += new System.EventHandler(this.PortFormatter_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.txtHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWidth)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetaControls.MetaButton btnPrev;
        private MetaControls.MetaButton btnNext;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboStyle;
        private System.Windows.Forms.NumericUpDown txtHeight;
        private System.Windows.Forms.NumericUpDown txtWidth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private MetaControls.MetaButton btnPortMover;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox cbOut;
        private System.Windows.Forms.CheckBox cbIn;
        private System.Windows.Forms.Label label6;
        private Janus.Windows.EditControls.UIColorButton colorButtonFill;
        private Janus.Windows.EditControls.UIColorButton colorButtonLine;
        private System.Windows.Forms.Label lblSelectObject;
        private MetaControls.MetaButton btnNextObject;
        private MetaControls.MetaButton btnPrevObject;
    }
}