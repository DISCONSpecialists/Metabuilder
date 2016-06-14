namespace MetaBuilder.UIControls.GraphingUI
{
    partial class PanningDocker
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
            this.goOverview1 = new Northwoods.Go.GoOverview();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageOverview = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPageOverview.SuspendLayout();
            this.SuspendLayout();
            // 
            // goOverview1
            // 
            this.goOverview1.AllowCopy = false;
            this.goOverview1.AllowDelete = false;
            this.goOverview1.AllowDragOut = false;
            this.goOverview1.AllowDrop = false;
            this.goOverview1.AllowEdit = false;
            this.goOverview1.AllowInsert = false;
            this.goOverview1.AllowLink = false;
            this.goOverview1.AllowReshape = false;
            this.goOverview1.AllowResize = false;
            this.goOverview1.AllowSelect = false;
            this.goOverview1.ArrowMoveLarge = 10F;
            this.goOverview1.ArrowMoveSmall = 1F;
            this.goOverview1.BackColor = System.Drawing.Color.White;
            this.goOverview1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.goOverview1.DocScale = 0.125F;
            this.goOverview1.DragsRealtime = true;
            this.goOverview1.DrawsXorMode = true;
            this.goOverview1.Location = new System.Drawing.Point(3, 3);
            this.goOverview1.Name = "goOverview1";
            this.goOverview1.ShowsNegativeCoordinates = false;
            this.goOverview1.Size = new System.Drawing.Size(199, 111);
            this.goOverview1.TabIndex = 0;
            this.goOverview1.Text = "goOverview1";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageOverview);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(213, 143);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPageOverview
            // 
            this.tabPageOverview.Controls.Add(this.goOverview1);
            this.tabPageOverview.Location = new System.Drawing.Point(4, 22);
            this.tabPageOverview.Name = "tabPageOverview";
            this.tabPageOverview.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOverview.Size = new System.Drawing.Size(205, 117);
            this.tabPageOverview.TabIndex = 0;
            this.tabPageOverview.Text = "Overview";
            this.tabPageOverview.UseVisualStyleBackColor = true;
            // 
            // PanningDocker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(213, 143);
            this.Controls.Add(this.tabControl1);
            this.HideOnClose = true;
            this.Name = "PanningDocker";
            this.TabText = "Pan and Zoom";
            this.Text = "Navigator";
            this.tabControl1.ResumeLayout(false);
            this.tabPageOverview.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageOverview;
        internal Northwoods.Go.GoOverview goOverview1;

    }
}