using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using MetaBuilder.UIControls.Common;

namespace MetaBuilder.WinUI
{
    /// <summary>
    /// Summary description for PleaseWait.
    /// </summary>
    public class PleaseWait : Form
    {
        private Label lblStatus;
        private NoProgressBar noProgressBar1;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components = null;

        public PleaseWait()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            this.lblStatus = new System.Windows.Forms.Label();
            this.noProgressBar1 = new MetaBuilder.UIControls.Common.NoProgressBar();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblStatus.Location = new System.Drawing.Point(0, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(378, 32);
            this.lblStatus.TabIndex = 1;
            // 
            // noProgressBar1
            // 
            this.noProgressBar1.BorderStyle = System.Windows.Forms.Border3DStyle.Adjust;
            this.noProgressBar1.CycleSpeed = 0;
            this.noProgressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.noProgressBar1.ForeColor = System.Drawing.Color.LightSlateGray;
            this.noProgressBar1.Location = new System.Drawing.Point(0, 32);
            this.noProgressBar1.Name = "noProgressBar1";
            this.noProgressBar1.ShapeSize = 8;
            this.noProgressBar1.ShapeSpacing = 20;
            this.noProgressBar1.ShapeToDraw = MetaBuilder.UIControls.Common.ElementStyle.Square;
            this.noProgressBar1.Size = new System.Drawing.Size(378, 23);
            this.noProgressBar1.TabIndex = 2;
            this.noProgressBar1.TabStop = false;
            this.noProgressBar1.Text = "noProgressBar1";
            // 
            // PleaseWait
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(378, 55);
            this.ControlBox = false;
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.noProgressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PleaseWait";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Please Wait...";
            this.Load += new System.EventHandler(this.PleaseWait_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public void SetMessageText(string message)
        {
            this.lblStatus.Text = message;
            //	Debug.WriteLine(System.Threading.Thread.CurrentThread.Name);

        }

        private void PleaseWait_Load(Object sender, EventArgs e)
        {
            if (Thread.CurrentThread.Name.Length == 0)
            {
                Thread.CurrentThread.Name = "Main Thread";
            }

        }
    }
}