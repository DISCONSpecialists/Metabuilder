namespace MetaBuilder.UIControls.GraphingUI.Formatting
{
    partial class GradientTypeEditorUI
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
            MetaBuilder.Graphing.Formatting.Gradient gradient2 = new MetaBuilder.Graphing.Formatting.Gradient();
            System.Drawing.Drawing2D.ColorBlend colorBlend2 = new System.Drawing.Drawing2D.ColorBlend();
            System.ComponentModel.StringConverter stringConverter2 = new System.ComponentModel.StringConverter();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.gradientBuilder = new MetaBuilder.UIControls.GraphingUI.Formatting.GradientBuilder();
            this.grpEditor = new System.Windows.Forms.GroupBox();
            this.opacityBox = new System.Windows.Forms.NumericUpDown();
            this.colorEditor = new MetaBuilder.UIControls.GraphingUI.Formatting.GenericValueEditor();
            this.grpEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.opacityBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Colour";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(172, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Opacity";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(269, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(15, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "%";
            // 
            // gradientBuilder
            // 
            this.gradientBuilder.Dock = System.Windows.Forms.DockStyle.Fill;
            colorBlend2.Colors = new System.Drawing.Color[] {
        System.Drawing.Color.White,
        System.Drawing.Color.White};
            colorBlend2.Positions = new float[] {
        0F,
        1F};
            gradient2.ColorBlend = colorBlend2;
            gradient2.GradientDirection = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.gradientBuilder.Gradient = gradient2;
            this.gradientBuilder.Location = new System.Drawing.Point(0, 0);
            this.gradientBuilder.Name = "gradientBuilder";
            this.gradientBuilder.Size = new System.Drawing.Size(384, 78);
            this.gradientBuilder.TabIndex = 0;
            this.gradientBuilder.Text = "gradientBuilder1";
            this.gradientBuilder.MarkerSelected += new System.EventHandler(this.gradientBuilder_MarkerSelected);
            this.gradientBuilder.GradientChanged += new System.EventHandler(this.gradientBuilder_GradientChanged);
            // 
            // grpEditor
            // 
            this.grpEditor.Controls.Add(this.opacityBox);
            this.grpEditor.Controls.Add(this.label5);
            this.grpEditor.Controls.Add(this.label4);
            this.grpEditor.Controls.Add(this.label1);
            this.grpEditor.Controls.Add(this.colorEditor);
            this.grpEditor.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpEditor.Enabled = false;
            this.grpEditor.Location = new System.Drawing.Point(0, 78);
            this.grpEditor.Name = "grpEditor";
            this.grpEditor.Size = new System.Drawing.Size(384, 45);
            this.grpEditor.TabIndex = 1;
            this.grpEditor.TabStop = false;
            this.grpEditor.Text = "Color Stops";
            // 
            // opacityBox
            // 
            this.opacityBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.opacityBox.Location = new System.Drawing.Point(221, 20);
            this.opacityBox.Name = "opacityBox";
            this.opacityBox.Size = new System.Drawing.Size(42, 20);
            this.opacityBox.TabIndex = 3;
            this.opacityBox.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.opacityBox.ValueChanged += new System.EventHandler(this.colorMixerFieldValueChanged);
            // 
            // colorEditor
            // 
            this.colorEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.colorEditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorEditor.Converter = stringConverter2;
            this.colorEditor.Location = new System.Drawing.Point(43, 19);
            this.colorEditor.Name = "colorEditor";
            this.colorEditor.Size = new System.Drawing.Size(106, 20);
            this.colorEditor.TabIndex = 0;
            this.colorEditor.ValueChanged += new System.EventHandler(this.colorMixerFieldValueChanged);
            // 
            // GradientTypeEditorUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gradientBuilder);
            this.Controls.Add(this.grpEditor);
            this.Name = "GradientTypeEditorUI";
            this.Size = new System.Drawing.Size(384, 123);
            this.grpEditor.ResumeLayout(false);
            this.grpEditor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.opacityBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GenericValueEditor colorEditor;
        private System.Windows.Forms.NumericUpDown opacityBox;
        private System.Windows.Forms.GroupBox grpEditor;
        private GradientBuilder gradientBuilder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;


    }
}
