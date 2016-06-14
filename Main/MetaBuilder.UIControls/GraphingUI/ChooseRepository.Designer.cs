namespace MetaBuilder.UIControls.GraphingUI
{
    partial class ChooseRepository
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.listBoxRepository = new System.Windows.Forms.ListBox();
            this.buttonSetDefault = new MetaControls.MetaButton();
            this.buttonCancel = new MetaControls.MetaButton();
            this.buttonSelect = new MetaControls.MetaButton();
            this.gradientLine1 = new Ascend.Windows.Forms.GradientLine();
            this.gradientCaption1 = new Ascend.Windows.Forms.GradientCaption();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.listBoxRepository);
            this.panel1.Controls.Add(this.buttonSetDefault);
            this.panel1.Controls.Add(this.buttonCancel);
            this.panel1.Controls.Add(this.buttonSelect);
            this.panel1.Controls.Add(this.gradientLine1);
            this.panel1.Controls.Add(this.gradientCaption1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(264, 179);
            this.panel1.TabIndex = 6;
            // 
            // listBoxRepository
            // 
            this.listBoxRepository.Dock = System.Windows.Forms.DockStyle.Top;
            this.listBoxRepository.FormattingEnabled = true;
            this.listBoxRepository.Items.AddRange(new object[] {
            "Server Repository",
            "Client Repository"});
            this.listBoxRepository.Location = new System.Drawing.Point(0, 56);
            this.listBoxRepository.Name = "listBoxRepository";
            this.listBoxRepository.Size = new System.Drawing.Size(262, 82);
            this.listBoxRepository.TabIndex = 6;
            this.listBoxRepository.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxRepository_MouseDoubleClick);
            this.listBoxRepository.SelectedIndexChanged += new System.EventHandler(this.listBoxRepository_SelectedIndexChanged);
            // 
            // buttonSetDefault
            // 
            this.buttonSetDefault.Enabled = false;
            this.buttonSetDefault.Location = new System.Drawing.Point(173, 144);
            this.buttonSetDefault.Name = "buttonSetDefault";
            this.buttonSetDefault.Size = new System.Drawing.Size(75, 23);
            this.buttonSetDefault.TabIndex = 9;
            this.buttonSetDefault.Text = "Default";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(92, 144);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 8;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonSelect
            // 
            this.buttonSelect.Location = new System.Drawing.Point(11, 144);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(75, 23);
            this.buttonSelect.TabIndex = 7;
            this.buttonSelect.Text = "OK";
            this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // gradientLine1
            // 
            this.gradientLine1.Dock = System.Windows.Forms.DockStyle.Top;
            this.gradientLine1.Location = new System.Drawing.Point(0, 46);
            this.gradientLine1.Name = "gradientLine1";
            this.gradientLine1.Size = new System.Drawing.Size(262, 10);
            this.gradientLine1.TabIndex = 10;
            // 
            // gradientCaption1
            // 
            this.gradientCaption1.AntiAlias = true;
            this.gradientCaption1.Dock = System.Windows.Forms.DockStyle.Top;
            this.gradientCaption1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gradientCaption1.ForeColor = System.Drawing.Color.Black;
            this.gradientCaption1.GradientHighColor = System.Drawing.Color.Orange;
            this.gradientCaption1.GradientLowColor = System.Drawing.Color.White;
            this.gradientCaption1.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.gradientCaption1.Location = new System.Drawing.Point(0, 0);
            this.gradientCaption1.Name = "gradientCaption1";
            this.gradientCaption1.Size = new System.Drawing.Size(262, 46);
            this.gradientCaption1.TabIndex = 11;
            this.gradientCaption1.Text = "MetaBuilder Repository Navigator";
            this.gradientCaption1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ChooseRepository
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 179);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ChooseRepository";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MetaBuilder Repository Navigator";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox listBoxRepository;
        private MetaControls.MetaButton buttonSetDefault;
        private MetaControls.MetaButton buttonCancel;
        private MetaControls.MetaButton buttonSelect;
        private Ascend.Windows.Forms.GradientLine gradientLine1;
        private Ascend.Windows.Forms.GradientCaption gradientCaption1;

    }
}