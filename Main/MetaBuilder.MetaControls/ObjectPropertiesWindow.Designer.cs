namespace MetaBuilder.MetaControls
{
    partial class ObjectPropertiesWindow
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
            this.objectProperties1 = new MetaBuilder.MetaControls.ObjectProperties(Server);
            this.SuspendLayout();
            // 
            // objectProperties1
            // 
            this.objectProperties1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectProperties1.Location = new System.Drawing.Point(0, 0);
            this.objectProperties1.Name = "objectProperties1";
            this.objectProperties1.Size = new System.Drawing.Size(332, 319);
            this.objectProperties1.TabIndex = 0;
            // 
            // ObjectPropertiesWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(332, 319);
            this.Controls.Add(this.objectProperties1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ObjectPropertiesWindow";
            this.Text = "Object Properties";
            this.ResumeLayout(false);

        }

        #endregion

        private ObjectProperties objectProperties1;
    }
}