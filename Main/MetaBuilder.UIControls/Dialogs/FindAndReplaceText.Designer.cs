namespace MetaBuilder.UIControls.Dialogs
{
    partial class FindAndReplaceText
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindAndReplaceText));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonFind = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonReplace = new System.Windows.Forms.ToolStripButton();
            this.panelFind = new System.Windows.Forms.Panel();
            this.buttonFindCancel = new MetaControls.MetaButton();
            this.buttonFindPrevious = new MetaControls.MetaButton();
            this.buttonFindNext = new MetaControls.MetaButton();
            this.checkBoxFindMatchWord = new System.Windows.Forms.CheckBox();
            this.checkBoxFindMatchCase = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxFindText = new System.Windows.Forms.TextBox();
            this.panelReplace = new System.Windows.Forms.Panel();
            this.buttonReplaceCancel = new MetaControls.MetaButton();
            this.buttonReplace = new MetaControls.MetaButton();
            this.buttonReplaceFind = new MetaControls.MetaButton();
            this.checkBoxReplaceMatchWord = new System.Windows.Forms.CheckBox();
            this.checkBoxReplaceMatchCase = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxReplaceText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxReplaceFindText = new System.Windows.Forms.TextBox();
            this.listBoxMatches = new System.Windows.Forms.ListBox();
            this.toolStrip1.SuspendLayout();
            this.panelFind.SuspendLayout();
            this.panelReplace.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonFind,
            this.toolStripButtonReplace});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(267, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonFind
            // 
            this.toolStripButtonFind.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonFind.Image")));
            this.toolStripButtonFind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFind.Name = "toolStripButtonFind";
            this.toolStripButtonFind.Size = new System.Drawing.Size(50, 22);
            this.toolStripButtonFind.Text = "Find";
            this.toolStripButtonFind.Click += new System.EventHandler(this.FindReplace_Click);
            // 
            // toolStripButtonReplace
            // 
            this.toolStripButtonReplace.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonReplace.Image")));
            this.toolStripButtonReplace.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonReplace.Name = "toolStripButtonReplace";
            this.toolStripButtonReplace.Size = new System.Drawing.Size(68, 22);
            this.toolStripButtonReplace.Text = "Replace";
            this.toolStripButtonReplace.Click += new System.EventHandler(this.FindReplace_Click);
            // 
            // panelFind
            // 
            this.panelFind.Controls.Add(this.buttonFindCancel);
            this.panelFind.Controls.Add(this.buttonFindPrevious);
            this.panelFind.Controls.Add(this.buttonFindNext);
            this.panelFind.Controls.Add(this.checkBoxFindMatchWord);
            this.panelFind.Controls.Add(this.checkBoxFindMatchCase);
            this.panelFind.Controls.Add(this.label3);
            this.panelFind.Controls.Add(this.textBoxFindText);
            this.panelFind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFind.Location = new System.Drawing.Point(0, 25);
            this.panelFind.Name = "panelFind";
            this.panelFind.Size = new System.Drawing.Size(267, 250);
            this.panelFind.TabIndex = 2;
            this.panelFind.Visible = false;
            // 
            // buttonFindCancel
            // 
            this.buttonFindCancel.Location = new System.Drawing.Point(180, 98);
            this.buttonFindCancel.Name = "buttonFindCancel";
            this.buttonFindCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonFindCancel.TabIndex = 5;
            this.buttonFindCancel.Text = "Close";
            this.buttonFindCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonFindPrevious
            // 
            this.buttonFindPrevious.Location = new System.Drawing.Point(92, 98);
            this.buttonFindPrevious.Name = "buttonFindPrevious";
            this.buttonFindPrevious.Size = new System.Drawing.Size(82, 23);
            this.buttonFindPrevious.TabIndex = 5;
            this.buttonFindPrevious.Text = "Find Previous";
            this.buttonFindPrevious.Click += new System.EventHandler(this.ButtonFindPrevious_Click);
            // 
            // buttonFindNext
            // 
            this.buttonFindNext.Location = new System.Drawing.Point(11, 98);
            this.buttonFindNext.Name = "buttonFindNext";
            this.buttonFindNext.Size = new System.Drawing.Size(75, 23);
            this.buttonFindNext.TabIndex = 5;
            this.buttonFindNext.Text = "Find Next";
            this.buttonFindNext.Click += new System.EventHandler(this.ButtonFindNext_Click);
            // 
            // checkBoxFindMatchWord
            // 
            this.checkBoxFindMatchWord.AutoSize = true;
            this.checkBoxFindMatchWord.Location = new System.Drawing.Point(12, 75);
            this.checkBoxFindMatchWord.Name = "checkBoxFindMatchWord";
            this.checkBoxFindMatchWord.Size = new System.Drawing.Size(113, 17);
            this.checkBoxFindMatchWord.TabIndex = 4;
            this.checkBoxFindMatchWord.Text = "Match whole word";
            this.checkBoxFindMatchWord.UseVisualStyleBackColor = true;
            this.checkBoxFindMatchWord.CheckStateChanged += new System.EventHandler(this.MatchWord_Changed);
            // 
            // checkBoxFindMatchCase
            // 
            this.checkBoxFindMatchCase.AutoSize = true;
            this.checkBoxFindMatchCase.Location = new System.Drawing.Point(12, 52);
            this.checkBoxFindMatchCase.Name = "checkBoxFindMatchCase";
            this.checkBoxFindMatchCase.Size = new System.Drawing.Size(82, 17);
            this.checkBoxFindMatchCase.TabIndex = 3;
            this.checkBoxFindMatchCase.Text = "Match case";
            this.checkBoxFindMatchCase.UseVisualStyleBackColor = true;
            this.checkBoxFindMatchCase.CheckStateChanged += new System.EventHandler(this.MatchCase_Changed);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(10, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Find what:";
            // 
            // textBoxFindText
            // 
            this.textBoxFindText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFindText.Location = new System.Drawing.Point(13, 26);
            this.textBoxFindText.Name = "textBoxFindText";
            this.textBoxFindText.Size = new System.Drawing.Size(244, 20);
            this.textBoxFindText.TabIndex = 1;
            this.textBoxFindText.TextChanged += new System.EventHandler(this.Find_TextChanged);
            this.textBoxFindText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Text_KeyUp);
            // 
            // panelReplace
            // 
            this.panelReplace.Controls.Add(this.buttonReplaceCancel);
            this.panelReplace.Controls.Add(this.buttonReplace);
            this.panelReplace.Controls.Add(this.buttonReplaceFind);
            this.panelReplace.Controls.Add(this.checkBoxReplaceMatchWord);
            this.panelReplace.Controls.Add(this.checkBoxReplaceMatchCase);
            this.panelReplace.Controls.Add(this.label1);
            this.panelReplace.Controls.Add(this.textBoxReplaceText);
            this.panelReplace.Controls.Add(this.label2);
            this.panelReplace.Controls.Add(this.textBoxReplaceFindText);
            this.panelReplace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelReplace.Location = new System.Drawing.Point(0, 25);
            this.panelReplace.Name = "panelReplace";
            this.panelReplace.Size = new System.Drawing.Size(267, 250);
            this.panelReplace.TabIndex = 3;
            this.panelReplace.Visible = false;
            // 
            // buttonReplaceCancel
            // 
            this.buttonReplaceCancel.Location = new System.Drawing.Point(182, 137);
            this.buttonReplaceCancel.Name = "buttonReplaceCancel";
            this.buttonReplaceCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonReplaceCancel.TabIndex = 10;
            this.buttonReplaceCancel.Text = "Close";
            this.buttonReplaceCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonReplace
            // 
            this.buttonReplace.Location = new System.Drawing.Point(94, 137);
            this.buttonReplace.Name = "buttonReplace";
            this.buttonReplace.Size = new System.Drawing.Size(82, 23);
            this.buttonReplace.TabIndex = 11;
            this.buttonReplace.Text = "Replace";
            this.buttonReplace.Click += new System.EventHandler(this.ButtonReplace_Click);
            // 
            // buttonReplaceFind
            // 
            this.buttonReplaceFind.Location = new System.Drawing.Point(13, 137);
            this.buttonReplaceFind.Name = "buttonReplaceFind";
            this.buttonReplaceFind.Size = new System.Drawing.Size(75, 23);
            this.buttonReplaceFind.TabIndex = 12;
            this.buttonReplaceFind.Text = "Find Next";
            this.buttonReplaceFind.Click += new System.EventHandler(this.ButtonFindNext_Click);
            // 
            // checkBoxReplaceMatchWord
            // 
            this.checkBoxReplaceMatchWord.AutoSize = true;
            this.checkBoxReplaceMatchWord.Location = new System.Drawing.Point(14, 114);
            this.checkBoxReplaceMatchWord.Name = "checkBoxReplaceMatchWord";
            this.checkBoxReplaceMatchWord.Size = new System.Drawing.Size(113, 17);
            this.checkBoxReplaceMatchWord.TabIndex = 9;
            this.checkBoxReplaceMatchWord.Text = "Match whole word";
            this.checkBoxReplaceMatchWord.UseVisualStyleBackColor = true;
            this.checkBoxReplaceMatchWord.CheckStateChanged += new System.EventHandler(this.MatchWord_Changed);
            // 
            // checkBoxReplaceMatchCase
            // 
            this.checkBoxReplaceMatchCase.AutoSize = true;
            this.checkBoxReplaceMatchCase.Location = new System.Drawing.Point(14, 91);
            this.checkBoxReplaceMatchCase.Name = "checkBoxReplaceMatchCase";
            this.checkBoxReplaceMatchCase.Size = new System.Drawing.Size(82, 17);
            this.checkBoxReplaceMatchCase.TabIndex = 8;
            this.checkBoxReplaceMatchCase.Text = "Match case";
            this.checkBoxReplaceMatchCase.UseVisualStyleBackColor = true;
            this.checkBoxReplaceMatchCase.CheckStateChanged += new System.EventHandler(this.MatchCase_Changed);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Replace with:";
            // 
            // textBoxReplaceText
            // 
            this.textBoxReplaceText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxReplaceText.Location = new System.Drawing.Point(15, 65);
            this.textBoxReplaceText.Name = "textBoxReplaceText";
            this.textBoxReplaceText.Size = new System.Drawing.Size(244, 20);
            this.textBoxReplaceText.TabIndex = 6;
            this.textBoxReplaceText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Text_KeyUp);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(10, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Find what:";
            // 
            // textBoxReplaceFindText
            // 
            this.textBoxReplaceFindText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxReplaceFindText.Location = new System.Drawing.Point(13, 26);
            this.textBoxReplaceFindText.Name = "textBoxReplaceFindText";
            this.textBoxReplaceFindText.Size = new System.Drawing.Size(244, 20);
            this.textBoxReplaceFindText.TabIndex = 6;
            this.textBoxReplaceFindText.TextChanged += new System.EventHandler(this.Find_TextChanged);
            this.textBoxReplaceFindText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Text_KeyUp);
            // 
            // listBoxMatches
            // 
            this.listBoxMatches.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.listBoxMatches.DisplayMember = "Text";
            this.listBoxMatches.FormattingEnabled = true;
            this.listBoxMatches.Location = new System.Drawing.Point(0, 193);
            this.listBoxMatches.Name = "listBoxMatches";
            this.listBoxMatches.Size = new System.Drawing.Size(267, 82);
            this.listBoxMatches.TabIndex = 14;
            this.listBoxMatches.Visible = false;
            this.listBoxMatches.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxMatches_MouseDoubleClick);
            this.listBoxMatches.SelectedIndexChanged += new System.EventHandler(this.listBoxMatches_SelectedIndexChanged);
            // 
            // FindAndReplaceText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 275);
            this.Controls.Add(this.listBoxMatches);
            this.Controls.Add(this.panelReplace);
            this.Controls.Add(this.panelFind);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FindAndReplaceText";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Find and Replace";
            this.Load += new System.EventHandler(this.FindAndReplaceText_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Text_KeyUp);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panelFind.ResumeLayout(false);
            this.panelFind.PerformLayout();
            this.panelReplace.ResumeLayout(false);
            this.panelReplace.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonFind;
        private System.Windows.Forms.ToolStripButton toolStripButtonReplace;
        private System.Windows.Forms.Panel panelFind;
        private System.Windows.Forms.Panel panelReplace;
        private MetaControls.MetaButton buttonFindCancel;
        private MetaControls.MetaButton buttonFindPrevious;
        private MetaControls.MetaButton buttonFindNext;
        private System.Windows.Forms.CheckBox checkBoxFindMatchWord;
        private System.Windows.Forms.CheckBox checkBoxFindMatchCase;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxFindText;
        private MetaControls.MetaButton buttonReplaceCancel;
        private MetaControls.MetaButton buttonReplace;
        private MetaControls.MetaButton buttonReplaceFind;
        private System.Windows.Forms.CheckBox checkBoxReplaceMatchWord;
        private System.Windows.Forms.CheckBox checkBoxReplaceMatchCase;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxReplaceText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxReplaceFindText;
        private System.Windows.Forms.ListBox listBoxMatches;

    }
}