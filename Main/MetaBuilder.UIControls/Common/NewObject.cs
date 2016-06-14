#region Copyright © 2007 - DISCON Specialists
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: NewObject.cs
//
#endregion

using System;
using System.ComponentModel;
using System.Windows.Forms;
using Janus.Windows.EditControls;

namespace MetaBuilder.UIControls.Common
{
    /// <summary>
    /// Summary description for NewObject.
    /// </summary>
    public class NewObject : Form
    {

		#region Fields (6) 

        private UIButton btnCancel;
        private UIButton btnNext;
        private ClassDropdown classDropdown1;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components = null;
        private Label label1;
        public string SelectedClass;

		#endregion Fields 

		#region Constructors (1) 

        public NewObject()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            classDropdown1.Init();
        }

		#endregion Constructors 

		#region Methods (3) 


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



		// Private Methods (2) 

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SelectedClass = null;
            Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            SelectedClass = classDropdown1.SelectedClass;
            Close();
        }


		#endregion Methods 


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.classDropdown1 = new ClassDropdown();
            this.label1 = new System.Windows.Forms.Label();
            this.btnNext = new Janus.Windows.EditControls.UIButton();
            this.btnCancel = new Janus.Windows.EditControls.UIButton();
            this.SuspendLayout();
            // 
            // classDropdown1
            // 
            this.classDropdown1.Location = new System.Drawing.Point(112, 4);
            this.classDropdown1.Name = "classDropdown1";
            this.classDropdown1.SelectedClass = null;
            this.classDropdown1.Size = new System.Drawing.Size(176, 20);
            this.classDropdown1.TabIndex = 0;
            this.classDropdown1.Text = "classDropdown1";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Create new...";
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(212, 28);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = "Next";
            this.btnNext.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(132, 28);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // NewObject
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 57);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.classDropdown1);
            this.Name = "NewObject";
            this.Text = "New Object...";
            this.ResumeLayout(false);
        }
        #endregion
    }
}