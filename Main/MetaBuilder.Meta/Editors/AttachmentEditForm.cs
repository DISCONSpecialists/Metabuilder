#region Copyright © 2007 - DISCON Specialists

//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: RichTextEditForm.cs
//

#endregion

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace MetaBuilder.Meta.Editors
{
	/// <summary>
	/// Summary description for RichTextEditForm.
	/// </summary>
	public class AttachmentEditForm : Form
    {

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

        public AttachmentEditForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

		}

        private Button btnView;
        private Button btnReplace;
        private Button btnCancel;
        private Button btnOK;
        private Label lblName;
        private Label lblOriginalFileName;
        private Label lblOriginalUserMachien;
        private Label lName;
        private Label lFilename;
        private Label lUserMachine;

		public Attachment   myAttachment;
        public void UpdateLabels()
        {
            lName.Text = myAttachment.Name;
            lUserMachine.Text = myAttachment.UserMachine;
            lFilename.Text = myAttachment.Filename;
        }
        public Attachment MyAttachment
		{
			get
			{
                return myAttachment;
			}
			set
			{
                myAttachment = value;
                UpdateLabels();
                
			}
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
            this.btnView = new System.Windows.Forms.Button();
            this.btnReplace = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.lblOriginalFileName = new System.Windows.Forms.Label();
            this.lblOriginalUserMachien = new System.Windows.Forms.Label();
            this.lName = new System.Windows.Forms.Label();
            this.lFilename = new System.Windows.Forms.Label();
            this.lUserMachine = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(6, 70);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(62, 23);
            this.btnView.TabIndex = 0;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnReplace
            // 
            this.btnReplace.Location = new System.Drawing.Point(74, 70);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(62, 23);
            this.btnReplace.TabIndex = 0;
            this.btnReplace.Text = "Replace";
            this.btnReplace.UseVisualStyleBackColor = true;
            this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(336, 70);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(63, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(264, 70);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(66, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click_1);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(81, 7);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name:";
            // 
            // lblOriginalFileName
            // 
            this.lblOriginalFileName.AutoSize = true;
            this.lblOriginalFileName.Location = new System.Drawing.Point(29, 26);
            this.lblOriginalFileName.Name = "lblOriginalFileName";
            this.lblOriginalFileName.Size = new System.Drawing.Size(90, 13);
            this.lblOriginalFileName.TabIndex = 1;
            this.lblOriginalFileName.Text = "Original Filename:";
            // 
            // lblOriginalUserMachien
            // 
            this.lblOriginalUserMachien.AutoSize = true;
            this.lblOriginalUserMachien.Location = new System.Drawing.Point(3, 45);
            this.lblOriginalUserMachien.Name = "lblOriginalUserMachien";
            this.lblOriginalUserMachien.Size = new System.Drawing.Size(116, 13);
            this.lblOriginalUserMachien.TabIndex = 1;
            this.lblOriginalUserMachien.Text = "Original User/Machine:";
            // 
            // lName
            // 
            this.lName.AutoEllipsis = true;
            this.lName.Location = new System.Drawing.Point(125, 7);
            this.lName.Name = "lName";
            this.lName.Size = new System.Drawing.Size(274, 13);
            this.lName.TabIndex = 1;
            this.lName.Text = "...";
            // 
            // lFilename
            // 
            this.lFilename.AutoEllipsis = true;
            this.lFilename.Location = new System.Drawing.Point(125, 26);
            this.lFilename.Name = "lFilename";
            this.lFilename.Size = new System.Drawing.Size(274, 13);
            this.lFilename.TabIndex = 1;
            this.lFilename.Text = "...";
            // 
            // lUserMachine
            // 
            this.lUserMachine.AutoEllipsis = true;
            this.lUserMachine.Location = new System.Drawing.Point(125, 45);
            this.lUserMachine.Name = "lUserMachine";
            this.lUserMachine.Size = new System.Drawing.Size(274, 13);
            this.lUserMachine.TabIndex = 1;
            this.lUserMachine.Text = "...";
            // 
            // AttachmentEditForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(402, 95);
            this.ControlBox = false;
            this.Controls.Add(this.lblOriginalUserMachien);
            this.Controls.Add(this.lblOriginalFileName);
            this.Controls.Add(this.lUserMachine);
            this.Controls.Add(this.lFilename);
            this.Controls.Add(this.lName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnView);
            this.Controls.Add(this.btnReplace);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "AttachmentEditForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Attachment...";
            this.Load += new System.EventHandler(this.AttachmentEditForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private void btnOK_Click(object sender, EventArgs e)
		{
         
		}

		

        private void AttachmentEditForm_Load(object sender, EventArgs e)
        {
            if (myAttachment != null)
                UpdateLabels();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            myAttachment.Open();
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.ShowDialog(this);
            if (ofd.FileName != null)
            {
                myAttachment = new Attachment();
                myAttachment.Load(ofd.FileName);
            }
            UpdateLabels();
        }

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

	}

}