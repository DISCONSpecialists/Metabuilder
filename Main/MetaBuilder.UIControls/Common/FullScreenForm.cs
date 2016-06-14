using System.ComponentModel;
using System.Windows.Forms;

namespace MetaBuilder.UIControls.Common
{
    /// <summary>
    /// Summary description for FullScreenForm.
    /// </summary>
    public class FullScreenForm : Form
    {

		#region Fields (1) 

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components = null;

		#endregion Fields 

		#region Constructors (1) 

        public FullScreenForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

		#endregion Constructors 

		#region Methods (1) 


		// Protected Methods (1) 

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


		#endregion Methods 


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // FullScreenForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FullScreenForm";
            this.Text = "FullScreenForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
        }
        #endregion
    }
}