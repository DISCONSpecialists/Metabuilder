namespace MetaBuilder.UIControls.Dialogs
{
    partial class PromptForPassword
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
            this.components = new System.ComponentModel.Container();
            this.gradientLine1 = new Ascend.Windows.Forms.GradientLine();
            this.gradientPanel1 = new Ascend.Windows.Forms.GradientPanel();
            this.gradientCaption1 = new Ascend.Windows.Forms.GradientCaption();
            this.btnCancel = new MetaControls.MetaButton();
            this.btnAuthenticate = new MetaControls.MetaButton();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.gradientPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // gradientLine1
            // 
            this.gradientLine1.Location = new System.Drawing.Point(0, 21);
            this.gradientLine1.Name = "gradientLine1";
            this.gradientLine1.Size = new System.Drawing.Size(302, 4);
            this.gradientLine1.TabIndex = 0;
            // 
            // gradientPanel1
            // 
            this.gradientPanel1.Controls.Add(this.gradientCaption1);
            this.gradientPanel1.Controls.Add(this.btnCancel);
            this.gradientPanel1.Controls.Add(this.btnAuthenticate);
            this.gradientPanel1.Controls.Add(this.label2);
            this.gradientPanel1.Controls.Add(this.txtPassword);
            this.gradientPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gradientPanel1.Location = new System.Drawing.Point(0, 0);
            this.gradientPanel1.Name = "gradientPanel1";
            this.gradientPanel1.Size = new System.Drawing.Size(303, 119);
            this.gradientPanel1.TabIndex = 1;
            // 
            // gradientCaption1
            // 
            this.gradientCaption1.ForeColor = System.Drawing.Color.Black;
            this.gradientCaption1.GradientHighColor = System.Drawing.SystemColors.ControlLightLight;
            this.gradientCaption1.GradientLowColor = System.Drawing.SystemColors.AppWorkspace;
            this.gradientCaption1.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.gradientCaption1.Location = new System.Drawing.Point(0, 0);
            this.gradientCaption1.Name = "gradientCaption1";
            this.gradientCaption1.Size = new System.Drawing.Size(303, 22);
            this.gradientCaption1.TabIndex = 4;
            this.gradientCaption1.Text = "Password Authentication";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(236, 86);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(55, 26);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAuthenticate
            // 
            this.btnAuthenticate.Location = new System.Drawing.Point(147, 86);
            this.btnAuthenticate.Name = "btnAuthenticate";
            this.btnAuthenticate.Size = new System.Drawing.Size(83, 26);
            this.btnAuthenticate.TabIndex = 2;
            this.btnAuthenticate.Text = "Authenticate";
            this.btnAuthenticate.Click += new System.EventHandler(this.btnAuthenticate_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(285, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Please enter the password required to access this function:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(12, 60);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(267, 20);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyUp);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // PromptForPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 119);
            this.ControlBox = false;
            this.Controls.Add(this.gradientLine1);
            this.Controls.Add(this.gradientPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PromptForPassword";
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Password Authentication";
            this.gradientPanel1.ResumeLayout(false);
            this.gradientPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Ascend.Windows.Forms.GradientLine gradientLine1;
        private Ascend.Windows.Forms.GradientPanel gradientPanel1;
        private MetaControls.MetaButton btnAuthenticate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private MetaControls.MetaButton btnCancel;
        private Ascend.Windows.Forms.GradientCaption gradientCaption1;
    }
}