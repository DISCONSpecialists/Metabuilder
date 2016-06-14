namespace MetaBuilder.UIControls.GraphingUI
{
    partial class HelpDocker
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
            this.textFilter = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboTopics = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textFilter
            // 
            this.textFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.textFilter.Location = new System.Drawing.Point(0, 13);
            this.textFilter.Name = "textFilter";
            this.textFilter.Size = new System.Drawing.Size(292, 20);
            this.textFilter.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(292, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Topic Filter";
            // 
            // comboTopics
            // 
            this.comboTopics.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboTopics.FormattingEnabled = true;
            this.comboTopics.Location = new System.Drawing.Point(0, 46);
            this.comboTopics.Name = "comboTopics";
            this.comboTopics.Size = new System.Drawing.Size(292, 21);
            this.comboTopics.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(292, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Topics";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(292, 5);
            this.label3.TabIndex = 4;
            // 
            // HelpDocker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 419);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboTopics);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textFilter);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "HelpDocker";
            this.Text = "Context Help";
            this.Load += new System.EventHandler(this.HelpDocker_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textFilter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboTopics;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}