namespace MetaBuilder.UIControls.Dialogs.Registration
{
    partial class SpecifyProxy
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
            this.btnOK = new MetaControls.MetaButton();
            this.lblProxyServer = new System.Windows.Forms.Label();
            this.txtProxyURL = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtConfirm = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblConfirm = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbSecureProxy = new System.Windows.Forms.CheckBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnTest = new MetaControls.MetaButton();
            this.cbUseProxy = new System.Windows.Forms.CheckBox();
            this.grpUseProxy = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.grpUseProxy.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(291, 277);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "Cancel";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblProxyServer
            // 
            this.lblProxyServer.AutoSize = true;
            this.lblProxyServer.Location = new System.Drawing.Point(16, 25);
            this.lblProxyServer.Name = "lblProxyServer";
            this.lblProxyServer.Size = new System.Drawing.Size(67, 13);
            this.lblProxyServer.TabIndex = 1;
            this.lblProxyServer.Text = "Proxy Server";
            // 
            // txtProxyURL
            // 
            this.txtProxyURL.Location = new System.Drawing.Point(113, 22);
            this.txtProxyURL.Name = "txtProxyURL";
            this.txtProxyURL.Size = new System.Drawing.Size(195, 20);
            this.txtProxyURL.TabIndex = 2;
            this.txtProxyURL.Text = "http://proxy";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(16, 49);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(26, 13);
            this.lblPort.TabIndex = 1;
            this.lblPort.Text = "Port";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(113, 46);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(60, 20);
            this.txtPort.TabIndex = 3;
            this.txtPort.Text = "80";
            // 
            // txtUsername
            // 
            this.txtUsername.Enabled = false;
            this.txtUsername.Location = new System.Drawing.Point(104, 19);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(185, 20);
            this.txtUsername.TabIndex = 5;
            // 
            // txtPassword
            // 
            this.txtPassword.Enabled = false;
            this.txtPassword.Location = new System.Drawing.Point(104, 45);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(185, 20);
            this.txtPassword.TabIndex = 6;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // txtConfirm
            // 
            this.txtConfirm.Enabled = false;
            this.txtConfirm.Location = new System.Drawing.Point(104, 74);
            this.txtConfirm.Name = "txtConfirm";
            this.txtConfirm.Size = new System.Drawing.Size(185, 20);
            this.txtConfirm.TabIndex = 7;
            this.txtConfirm.UseSystemPasswordChar = true;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(7, 22);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(55, 13);
            this.lblUsername.TabIndex = 1;
            this.lblUsername.Text = "Username";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(7, 48);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 1;
            this.lblPassword.Text = "Password";
            // 
            // lblConfirm
            // 
            this.lblConfirm.AutoSize = true;
            this.lblConfirm.Location = new System.Drawing.Point(7, 77);
            this.lblConfirm.Name = "lblConfirm";
            this.lblConfirm.Size = new System.Drawing.Size(91, 13);
            this.lblConfirm.TabIndex = 1;
            this.lblConfirm.Text = "Confirm Password";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtUsername);
            this.groupBox1.Controls.Add(this.lblUsername);
            this.groupBox1.Controls.Add(this.txtConfirm);
            this.groupBox1.Controls.Add(this.lblPassword);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.lblConfirm);
            this.groupBox1.Location = new System.Drawing.Point(19, 97);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(314, 102);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Authentication";
            // 
            // cbSecureProxy
            // 
            this.cbSecureProxy.AutoSize = true;
            this.cbSecureProxy.Location = new System.Drawing.Point(113, 72);
            this.cbSecureProxy.Name = "cbSecureProxy";
            this.cbSecureProxy.Size = new System.Drawing.Size(89, 17);
            this.cbSecureProxy.TabIndex = 4;
            this.cbSecureProxy.Text = "Secure Proxy";
            this.cbSecureProxy.UseVisualStyleBackColor = true;
            this.cbSecureProxy.CheckedChanged += new System.EventHandler(this.cbSecureProxy_CheckedChanged);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(210, 277);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 8;
            this.btnTest.Text = "Test";
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // cbUseProxy
            // 
            this.cbUseProxy.AutoSize = true;
            this.cbUseProxy.Checked = true;
            this.cbUseProxy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUseProxy.Location = new System.Drawing.Point(18, 12);
            this.cbUseProxy.Name = "cbUseProxy";
            this.cbUseProxy.Size = new System.Drawing.Size(118, 17);
            this.cbUseProxy.TabIndex = 1;
            this.cbUseProxy.Text = "Use A Proxy Server";
            this.cbUseProxy.UseVisualStyleBackColor = true;
            this.cbUseProxy.CheckedChanged += new System.EventHandler(this.cbUseProxy_CheckedChanged);
            // 
            // grpUseProxy
            // 
            this.grpUseProxy.Controls.Add(this.label1);
            this.grpUseProxy.Controls.Add(this.lblProxyServer);
            this.grpUseProxy.Controls.Add(this.cbSecureProxy);
            this.grpUseProxy.Controls.Add(this.groupBox1);
            this.grpUseProxy.Controls.Add(this.lblPort);
            this.grpUseProxy.Controls.Add(this.txtPort);
            this.grpUseProxy.Controls.Add(this.txtProxyURL);
            this.grpUseProxy.Location = new System.Drawing.Point(18, 35);
            this.grpUseProxy.Name = "grpUseProxy";
            this.grpUseProxy.Size = new System.Drawing.Size(348, 236);
            this.grpUseProxy.TabIndex = 5;
            this.grpUseProxy.TabStop = false;
            this.grpUseProxy.Text = "Proxy Server Details";
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(16, 202);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 31);
            this.label1.TabIndex = 10;
            this.label1.Text = "Please note that your user credentials are not stored - only used to establish th" +
                "e connection";
            // 
            // SpecifyProxy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 304);
            this.Controls.Add(this.grpUseProxy);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cbUseProxy);
            this.Controls.Add(this.btnTest);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SpecifyProxy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Specify Proxy Server";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.grpUseProxy.ResumeLayout(false);
            this.grpUseProxy.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetaControls.MetaButton btnOK;
        private System.Windows.Forms.Label lblProxyServer;
        private System.Windows.Forms.TextBox txtProxyURL;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtConfirm;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblConfirm;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbSecureProxy;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private MetaControls.MetaButton btnTest;
        private System.Windows.Forms.GroupBox grpUseProxy;
        private System.Windows.Forms.CheckBox cbUseProxy;
        private System.Windows.Forms.Label label1;
    }
}