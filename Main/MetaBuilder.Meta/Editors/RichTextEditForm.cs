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
using System.Drawing;
using System.Windows.Forms;
using Janus.Windows.EditControls;
using Syncfusion.Windows.Forms;

namespace MetaBuilder.Meta.Editors
{
	/// <summary>
	/// Summary description for RichTextEditForm.
	/// </summary>
	public class RichTextEditForm : Form
	{
		private RTFEdit rtfEdit1;
		private Panel pnlControls;
		private UIButton btnOK;
        private Panel panel1;
		private UIFontPicker uiFontPicker1;
		private Button btnBold;
		private Button btnItalic;
		private Button btnBullet;
		private Button btnAlignLeft;
		private Button btnAlignCenter;
		private Button btnAlignRight;
		private UIComboBox ddlFontSize;
        private ColorPickerButton colorPickerButton1;
        private ColorPickerButton colorPickerButton2;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		public RichTextEditForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

		}
        public enum EditorMode
        {
            LongText, RTF
        }
		public LongText lt;
        private EditorMode mode;
        public EditorMode Mode
        {
            get
            {
                return mode;
            }
            set
            {
                mode = value;
            }
        }

		public LongText longText
		{
			get
			{
				LongText retval = new LongText();
                switch (Mode)
                {
                    case EditorMode.LongText:
                        retval.Text = this.rtfEdit1.Text;
                        break;
                    case EditorMode.RTF:
                        retval.RTF = this.rtfEdit1.Rtf;
                        break;
                }
				return retval;
			}
			set
			{
				lt = value;
                switch (Mode)
                {
                    case EditorMode.RTF:
                        rtfEdit1.Rtf = lt.RTF;
                        EnableDisableRTFControls(true);
                        break;
                    case EditorMode.LongText:
                        rtfEdit1.Text = lt.Text;
		                EnableDisableRTFControls(false);
                        break;
                }


			}
		}
private void EnableDisableRTFControls(bool enabled)
{
    uiFontPicker1.Visible = enabled;
		btnBold.Visible = enabled;
		btnItalic.Visible = enabled;
		btnBullet.Visible = enabled;
		btnAlignLeft.Visible = enabled;
		btnAlignCenter.Visible = enabled;
		btnAlignRight.Visible = enabled;
		ddlFontSize.Visible = enabled;
        colorPickerButton1.Visible = enabled;
        colorPickerButton2.Visible = enabled;
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
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem34 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem35 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem36 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem37 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem38 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem39 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem40 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem41 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem42 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem43 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem44 = new Janus.Windows.EditControls.UIComboBoxItem();
            System.Drawing.StringFormat stringFormat7 = new System.Drawing.StringFormat();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RichTextEditForm));
            System.Drawing.StringFormat stringFormat8 = new System.Drawing.StringFormat();
            this.rtfEdit1 = new MetaBuilder.Meta.Editors.RTFEdit();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.ddlFontSize = new Janus.Windows.EditControls.UIComboBox();
            this.btnAlignRight = new System.Windows.Forms.Button();
            this.btnAlignCenter = new System.Windows.Forms.Button();
            this.btnAlignLeft = new System.Windows.Forms.Button();
            this.btnBullet = new System.Windows.Forms.Button();
            this.btnItalic = new System.Windows.Forms.Button();
            this.btnBold = new System.Windows.Forms.Button();
            this.uiFontPicker1 = new Janus.Windows.EditControls.UIFontPicker();
            this.btnOK = new Janus.Windows.EditControls.UIButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.colorPickerButton1 = new Syncfusion.Windows.Forms.ColorPickerButton();
            this.colorPickerButton2 = new Syncfusion.Windows.Forms.ColorPickerButton();
            this.pnlControls.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtfEdit1
            // 
            this.rtfEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtfEdit1.HideSelection = false;
            this.rtfEdit1.Location = new System.Drawing.Point(0, 0);
            this.rtfEdit1.Name = "rtfEdit1";
            this.rtfEdit1.SelectionAlignment = MetaBuilder.Meta.Editors.TextAlign.Left;
            this.rtfEdit1.Size = new System.Drawing.Size(576, 417);
            this.rtfEdit1.TabIndex = 6;
            this.rtfEdit1.Text = "";
            this.rtfEdit1.SelectionChanged += new System.EventHandler(this.rtfEdit1_SelectionChanged);
            // 
            // pnlControls
            // 
            this.pnlControls.BackColor = System.Drawing.Color.LightSlateGray;
            this.pnlControls.Controls.Add(this.colorPickerButton2);
            this.pnlControls.Controls.Add(this.colorPickerButton1);
            this.pnlControls.Controls.Add(this.ddlFontSize);
            this.pnlControls.Controls.Add(this.btnAlignRight);
            this.pnlControls.Controls.Add(this.btnAlignCenter);
            this.pnlControls.Controls.Add(this.btnAlignLeft);
            this.pnlControls.Controls.Add(this.btnBullet);
            this.pnlControls.Controls.Add(this.btnItalic);
            this.pnlControls.Controls.Add(this.btnBold);
            this.pnlControls.Controls.Add(this.uiFontPicker1);
            this.pnlControls.Controls.Add(this.btnOK);
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlControls.Location = new System.Drawing.Point(0, 0);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(576, 32);
            this.pnlControls.TabIndex = 13;
            // 
            // ddlFontSize
            // 
            this.ddlFontSize.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            uiComboBoxItem34.FormatStyle.Alpha = 0;
            uiComboBoxItem34.Text = "8pt";
            uiComboBoxItem34.Value = ((short)(8));
            uiComboBoxItem35.FormatStyle.Alpha = 0;
            uiComboBoxItem35.Text = "10pt";
            uiComboBoxItem35.Value = 10;
            uiComboBoxItem36.FormatStyle.Alpha = 0;
            uiComboBoxItem36.Text = "12pt";
            uiComboBoxItem36.Value = ((short)(12));
            uiComboBoxItem37.FormatStyle.Alpha = 0;
            uiComboBoxItem37.Text = "14pt";
            uiComboBoxItem37.Value = ((short)(14));
            uiComboBoxItem38.FormatStyle.Alpha = 0;
            uiComboBoxItem38.Text = "16pt";
            uiComboBoxItem38.Value = ((short)(16));
            uiComboBoxItem39.FormatStyle.Alpha = 0;
            uiComboBoxItem39.Text = "18pt";
            uiComboBoxItem39.Value = ((short)(18));
            uiComboBoxItem40.FormatStyle.Alpha = 0;
            uiComboBoxItem40.Text = "20pt";
            uiComboBoxItem40.Value = ((short)(20));
            uiComboBoxItem41.FormatStyle.Alpha = 0;
            uiComboBoxItem41.Text = "24pt";
            uiComboBoxItem41.Value = ((short)(24));
            uiComboBoxItem42.FormatStyle.Alpha = 0;
            uiComboBoxItem42.Text = "32pt";
            uiComboBoxItem42.Value = ((short)(32));
            uiComboBoxItem43.FormatStyle.Alpha = 0;
            uiComboBoxItem43.Text = "48pt";
            uiComboBoxItem43.Value = ((short)(48));
            uiComboBoxItem44.FormatStyle.Alpha = 0;
            uiComboBoxItem44.Text = "72pt";
            uiComboBoxItem44.Value = ((short)(72));
            this.ddlFontSize.Items.AddRange(new Janus.Windows.EditControls.UIComboBoxItem[] {
            uiComboBoxItem34,
            uiComboBoxItem35,
            uiComboBoxItem36,
            uiComboBoxItem37,
            uiComboBoxItem38,
            uiComboBoxItem39,
            uiComboBoxItem40,
            uiComboBoxItem41,
            uiComboBoxItem42,
            uiComboBoxItem43,
            uiComboBoxItem44});
            this.ddlFontSize.Location = new System.Drawing.Point(4, 4);
            this.ddlFontSize.Name = "ddlFontSize";
            this.ddlFontSize.SelectedIndex = 1;
            this.ddlFontSize.SelectedValue = 10;
            this.ddlFontSize.Size = new System.Drawing.Size(52, 20);
            stringFormat7.Alignment = System.Drawing.StringAlignment.Near;
            stringFormat7.FormatFlags = ((System.Drawing.StringFormatFlags)(((System.Drawing.StringFormatFlags.FitBlackBox | System.Drawing.StringFormatFlags.NoWrap)
                        | System.Drawing.StringFormatFlags.NoClip)));
            stringFormat7.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat7.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat7.Trimming = System.Drawing.StringTrimming.Character;
            this.ddlFontSize.StringFormat = stringFormat7;
            this.ddlFontSize.TabIndex = 22;
            this.ddlFontSize.Text = "10pt";
            this.ddlFontSize.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            this.ddlFontSize.SelectedIndexChanged += new System.EventHandler(this.ddlFontSize_SelectedIndexChanged);
            // 
            // btnAlignRight
            // 
            this.btnAlignRight.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAlignRight.BackgroundImage")));
            this.btnAlignRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAlignRight.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.btnAlignRight.Location = new System.Drawing.Point(466, 5);
            this.btnAlignRight.Name = "btnAlignRight";
            this.btnAlignRight.Size = new System.Drawing.Size(24, 22);
            this.btnAlignRight.TabIndex = 21;
            this.btnAlignRight.Click += new System.EventHandler(this.btnAlignRight_Click);
            // 
            // btnAlignCenter
            // 
            this.btnAlignCenter.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAlignCenter.BackgroundImage")));
            this.btnAlignCenter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAlignCenter.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.btnAlignCenter.Location = new System.Drawing.Point(438, 5);
            this.btnAlignCenter.Name = "btnAlignCenter";
            this.btnAlignCenter.Size = new System.Drawing.Size(24, 22);
            this.btnAlignCenter.TabIndex = 20;
            this.btnAlignCenter.Click += new System.EventHandler(this.btnAlignCenter_Click);
            // 
            // btnAlignLeft
            // 
            this.btnAlignLeft.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAlignLeft.BackgroundImage")));
            this.btnAlignLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAlignLeft.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.btnAlignLeft.Location = new System.Drawing.Point(410, 5);
            this.btnAlignLeft.Name = "btnAlignLeft";
            this.btnAlignLeft.Size = new System.Drawing.Size(24, 22);
            this.btnAlignLeft.TabIndex = 19;
            this.btnAlignLeft.Click += new System.EventHandler(this.btnAlignLeft_Click);
            // 
            // btnBullet
            // 
            this.btnBullet.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBullet.BackgroundImage")));
            this.btnBullet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBullet.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.btnBullet.Location = new System.Drawing.Point(382, 5);
            this.btnBullet.Name = "btnBullet";
            this.btnBullet.Size = new System.Drawing.Size(24, 22);
            this.btnBullet.TabIndex = 18;
            this.btnBullet.Click += new System.EventHandler(this.btnBullet_Click);
            // 
            // btnItalic
            // 
            this.btnItalic.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnItalic.BackgroundImage")));
            this.btnItalic.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnItalic.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.btnItalic.Location = new System.Drawing.Point(354, 5);
            this.btnItalic.Name = "btnItalic";
            this.btnItalic.Size = new System.Drawing.Size(24, 22);
            this.btnItalic.TabIndex = 17;
            this.btnItalic.Click += new System.EventHandler(this.btnItalic_Click);
            // 
            // btnBold
            // 
            this.btnBold.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBold.BackgroundImage")));
            this.btnBold.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBold.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.btnBold.Location = new System.Drawing.Point(326, 5);
            this.btnBold.Name = "btnBold";
            this.btnBold.Size = new System.Drawing.Size(24, 22);
            this.btnBold.TabIndex = 16;
            this.btnBold.Click += new System.EventHandler(this.btnBold_Click);
            // 
            // uiFontPicker1
            // 
            this.uiFontPicker1.Location = new System.Drawing.Point(60, 4);
            this.uiFontPicker1.Name = "uiFontPicker1";
            this.uiFontPicker1.SelectedIndex = 0;
            this.uiFontPicker1.Size = new System.Drawing.Size(176, 22);
            stringFormat8.Alignment = System.Drawing.StringAlignment.Near;
            stringFormat8.FormatFlags = ((System.Drawing.StringFormatFlags)(((System.Drawing.StringFormatFlags.FitBlackBox | System.Drawing.StringFormatFlags.NoWrap)
                        | System.Drawing.StringFormatFlags.NoClip)));
            stringFormat8.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            stringFormat8.LineAlignment = System.Drawing.StringAlignment.Center;
            stringFormat8.Trimming = System.Drawing.StringTrimming.Character;
            this.uiFontPicker1.StringFormat = stringFormat8;
            this.uiFontPicker1.TabIndex = 15;
            this.uiFontPicker1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            this.uiFontPicker1.SelectedValueChanged += new System.EventHandler(this.uiFontPicker1_SelectedValueChanged);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(496, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 13;
            this.btnOK.Text = "OK";
            this.btnOK.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rtfEdit1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 32);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(576, 417);
            this.panel1.TabIndex = 14;
            // 
            // colorPickerButton1
            // 
            this.colorPickerButton1.BackColor = System.Drawing.Color.Black;
            this.colorPickerButton1.ColorUISize = new System.Drawing.Size(208, 230);
            this.colorPickerButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colorPickerButton1.ForeColor = System.Drawing.Color.White;
            this.colorPickerButton1.Location = new System.Drawing.Point(242, 5);
            this.colorPickerButton1.Name = "colorPickerButton1";
            this.colorPickerButton1.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.colorPickerButton1.SelectedColorGroup = Syncfusion.Windows.Forms.ColorUISelectedGroup.None;
            this.colorPickerButton1.Size = new System.Drawing.Size(38, 22);
            this.colorPickerButton1.TabIndex = 23;
            this.colorPickerButton1.Text = "Text";
            this.colorPickerButton1.UseVisualStyleBackColor = false;
            this.colorPickerButton1.ColorSelected += new System.EventHandler(this.colorPickerButton1_ColorSelected);
            // 
            // colorPickerButton2
            // 
            this.colorPickerButton2.BackColor = System.Drawing.Color.White;
            this.colorPickerButton2.ColorUISize = new System.Drawing.Size(208, 230);
            this.colorPickerButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colorPickerButton2.ForeColor = System.Drawing.Color.Black;
            this.colorPickerButton2.Location = new System.Drawing.Point(286, 5);
            this.colorPickerButton2.Name = "colorPickerButton2";
            this.colorPickerButton2.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.colorPickerButton2.SelectedColorGroup = Syncfusion.Windows.Forms.ColorUISelectedGroup.None;
            this.colorPickerButton2.Size = new System.Drawing.Size(34, 22);
            this.colorPickerButton2.TabIndex = 23;
            this.colorPickerButton2.Text = "Fill";
            this.colorPickerButton2.UseVisualStyleBackColor = false;
            this.colorPickerButton2.ColorSelected += new System.EventHandler(this.colorPickerButton2_ColorSelected);
            // 
            // RichTextEditForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(576, 449);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlControls);
            this.Name = "RichTextEditForm";
            this.Text = "RichText Edit Form";
            this.pnlControls.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private void btnBullets_Click(object sender, EventArgs e)
		{
			this.rtfEdit1.SelectionBullet = !this.rtfEdit1.SelectionBullet;
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
            this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void uiFontPicker1_SelectedValueChanged(object sender, EventArgs e)
		{
            this.rtfEdit1.SelectionFont = new Font(uiFontPicker1.Text, float.Parse(ddlFontSize.SelectedValue.ToString(), System.Globalization.CultureInfo.InvariantCulture), FontStyle.Regular);
		}

		private void btnBold_Click(object sender, EventArgs e)
		{
			btnBold.ForeColor = (btnBold.ForeColor == Color.Black) ? Color.Gray : Color.Black;
			this.rtfEdit1.SelectionFont = (btnBold.ForeColor == Color.Black) ? new Font(this.rtfEdit1.SelectionFont, FontStyle.Bold) : new Font(this.rtfEdit1.SelectionFont, FontStyle.Regular);
		}

		private void btnItalic_Click(object sender, EventArgs e)
		{
			btnItalic.ForeColor = (btnItalic.ForeColor == Color.Black) ? Color.Gray : Color.Black;
			this.rtfEdit1.SelectionFont = (btnItalic.ForeColor == Color.Black) ? new Font(this.rtfEdit1.SelectionFont, FontStyle.Italic) : new Font(this.rtfEdit1.SelectionFont, FontStyle.Regular);

		}

		private void btnBullet_Click(object sender, EventArgs e)
		{
			this.btnBullet.ForeColor = (btnBullet.ForeColor == Color.Black) ? Color.Gray : Color.Black;
			this.rtfEdit1.SelectionBullet = (btnBullet.ForeColor == Color.Black) ? true : false;
		}

		private void btnAlignLeft_Click(object sender, EventArgs e)
		{
			this.rtfEdit1.SelectionAlignment = TextAlign.Left;
		}

		private void btnAlignCenter_Click(object sender, EventArgs e)
		{
			this.rtfEdit1.SelectionAlignment = TextAlign.Center;
		}

		private void btnAlignRight_Click(object sender, EventArgs e)
		{
			this.rtfEdit1.SelectionAlignment = TextAlign.Right;
		}

		private void uiColorButton1_SelectedColorChanged(object sender, EventArgs e)
		{
			
		}

		private void ddlFontSize_SelectedIndexChanged(object sender, EventArgs e)
		{
            this.rtfEdit1.SelectionFont = new Font(uiFontPicker1.Text, float.Parse(ddlFontSize.SelectedValue.ToString(), System.Globalization.CultureInfo.InvariantCulture), FontStyle.Regular);
		}

        private void colorPickerButton1_ColorSelected(object sender, EventArgs e)
        {
            this.rtfEdit1.SelectionColor = colorPickerButton1.SelectedColor;
            
        }

        private void colorPickerButton2_ColorSelected(object sender, EventArgs e)
        {
            this.rtfEdit1.SelectionBackColor = colorPickerButton2.SelectedColor;
        }

        private void rtfEdit1_SelectionChanged(object sender, EventArgs e)
        {
            this.colorPickerButton2.SelectedColor = rtfEdit1.SelectionBackColor;
            this.colorPickerButton2.ForeColor = rtfEdit1.SelectionColor;
            this.colorPickerButton2.BackColor = rtfEdit1.SelectionBackColor;

            this.colorPickerButton1.SelectedColor = rtfEdit1.SelectionColor;
            this.colorPickerButton1.BackColor = rtfEdit1.SelectionColor;
            this.colorPickerButton1.ForeColor = rtfEdit1.SelectionBackColor;

        }

	}

}