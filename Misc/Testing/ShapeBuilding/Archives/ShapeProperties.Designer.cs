namespace ShapeBuilding
{
    partial class ShapeProperties
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
            this.metaPropertyGrid1 = new MetaBuilder.MetaControls.MetaPropertyGrid();
            this.SuspendLayout();
            // 
            // metaPropertyGrid1
            // 
            this.metaPropertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metaPropertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.metaPropertyGrid1.Name = "metaPropertyGrid1";
            this.metaPropertyGrid1.Size = new System.Drawing.Size(223, 305);
            this.metaPropertyGrid1.TabIndex = 0;
            // 
            // ShapeProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(223, 305);
            this.Controls.Add(this.metaPropertyGrid1);
            this.ForeColor = System.Drawing.Color.Gainsboro;
            this.Name = "ShapeProperties";
            this.Text = "Properties";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        public MetaBuilder.MetaControls.MetaPropertyGrid metaPropertyGrid1;

    }
}