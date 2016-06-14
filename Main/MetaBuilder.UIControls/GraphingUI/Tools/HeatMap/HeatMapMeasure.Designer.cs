namespace MetaBuilder.UIControls.GraphingUI.Tools.HeatMap
{
    partial class HeatMapMeasure
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
            this.buttonApply = new MetaBuilder.MetaControls.MetaButton();
            this.checkedListBoxMeasurementObjects = new System.Windows.Forms.CheckedListBox();
            this.labelHead = new System.Windows.Forms.Label();
            this.checkBoxHueAndSaturation = new System.Windows.Forms.CheckBox();
            this.panelColour = new System.Windows.Forms.Panel();
            this.labelBottom = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelMid = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelTop = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panelColour.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonApply
            // 
            this.buttonApply.CornerRadius = new Ascend.CornerRadius(2);
            this.buttonApply.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonApply.GradientLowColor = System.Drawing.Color.DarkGray;
            this.buttonApply.Location = new System.Drawing.Point(0, 131);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(327, 24);
            this.buttonApply.StayActiveAfterClick = false;
            this.buttonApply.TabIndex = 0;
            this.buttonApply.Text = "Apply";
            this.buttonApply.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // checkedListBoxMeasurementObjects
            // 
            this.checkedListBoxMeasurementObjects.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.checkedListBoxMeasurementObjects.CheckOnClick = true;
            this.checkedListBoxMeasurementObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxMeasurementObjects.Location = new System.Drawing.Point(0, 17);
            this.checkedListBoxMeasurementObjects.Name = "checkedListBoxMeasurementObjects";
            this.checkedListBoxMeasurementObjects.Size = new System.Drawing.Size(228, 90);
            this.checkedListBoxMeasurementObjects.TabIndex = 1;
            this.checkedListBoxMeasurementObjects.ThreeDCheckBoxes = true;
            this.checkedListBoxMeasurementObjects.SelectedIndexChanged += new System.EventHandler(this.checkedListBoxMeasurementObjects_SelectedIndexChanged);
            this.checkedListBoxMeasurementObjects.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxMeasurementObjects_ItemCheck);
            // 
            // labelHead
            // 
            this.labelHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelHead.Location = new System.Drawing.Point(0, 0);
            this.labelHead.Name = "labelHead";
            this.labelHead.Size = new System.Drawing.Size(327, 17);
            this.labelHead.TabIndex = 2;
            this.labelHead.Text = "Select the measures you would like to apply to the current diagram";
            // 
            // checkBoxHueAndSaturation
            // 
            this.checkBoxHueAndSaturation.AutoSize = true;
            this.checkBoxHueAndSaturation.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.checkBoxHueAndSaturation.Location = new System.Drawing.Point(0, 114);
            this.checkBoxHueAndSaturation.Name = "checkBoxHueAndSaturation";
            this.checkBoxHueAndSaturation.Size = new System.Drawing.Size(228, 17);
            this.checkBoxHueAndSaturation.TabIndex = 3;
            this.checkBoxHueAndSaturation.Text = "Change brightness of colours by factor equal to measured value";
            this.checkBoxHueAndSaturation.UseVisualStyleBackColor = true;
            // 
            // panelColour
            // 
            this.panelColour.Controls.Add(this.labelBottom);
            this.panelColour.Controls.Add(this.label6);
            this.panelColour.Controls.Add(this.labelMid);
            this.panelColour.Controls.Add(this.label4);
            this.panelColour.Controls.Add(this.labelTop);
            this.panelColour.Controls.Add(this.label2);
            this.panelColour.Controls.Add(this.label1);
            this.panelColour.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelColour.Location = new System.Drawing.Point(228, 17);
            this.panelColour.Name = "panelColour";
            this.panelColour.Size = new System.Drawing.Size(99, 114);
            this.panelColour.TabIndex = 4;
            // 
            // labelBottom
            // 
            this.labelBottom.BackColor = System.Drawing.Color.Lime;
            this.labelBottom.Location = new System.Drawing.Point(50, 84);
            this.labelBottom.Name = "labelBottom";
            this.labelBottom.Size = new System.Drawing.Size(36, 23);
            this.labelBottom.TabIndex = 2;
            this.labelBottom.Click += new System.EventHandler(this.labelTop_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 89);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(19, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "<2";
            // 
            // labelMid
            // 
            this.labelMid.BackColor = System.Drawing.Color.Yellow;
            this.labelMid.Location = new System.Drawing.Point(50, 53);
            this.labelMid.Name = "labelMid";
            this.labelMid.Size = new System.Drawing.Size(36, 23);
            this.labelMid.TabIndex = 2;
            this.labelMid.Click += new System.EventHandler(this.labelTop_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = ">2<4";
            // 
            // labelTop
            // 
            this.labelTop.BackColor = System.Drawing.Color.Red;
            this.labelTop.Location = new System.Drawing.Point(50, 20);
            this.labelTop.Name = "labelTop";
            this.labelTop.Size = new System.Drawing.Size(36, 23);
            this.labelTop.TabIndex = 2;
            this.labelTop.Click += new System.EventHandler(this.labelTop_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = ">4";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Score Colouring";
            // 
            // HeatMapMeasure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 155);
            this.Controls.Add(this.checkedListBoxMeasurementObjects);
            this.Controls.Add(this.checkBoxHueAndSaturation);
            this.Controls.Add(this.panelColour);
            this.Controls.Add(this.labelHead);
            this.Controls.Add(this.buttonApply);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "HeatMapMeasure";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Heat Map";
            this.Load += new System.EventHandler(this.HeatMapMeasure_Load);
            this.Click += new System.EventHandler(this.HeatMapMeasure_Click);
            this.panelColour.ResumeLayout(false);
            this.panelColour.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetaBuilder.MetaControls.MetaButton buttonApply;
        private System.Windows.Forms.CheckedListBox checkedListBoxMeasurementObjects;
        private System.Windows.Forms.Label labelHead;
        private System.Windows.Forms.CheckBox checkBoxHueAndSaturation;
        private System.Windows.Forms.Panel panelColour;
        private System.Windows.Forms.Label labelBottom;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelMid;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelTop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}