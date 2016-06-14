namespace MetaBuilder.UIControls.Dialogs
{
    partial class FindTextWinForms
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
            this.txtTextToFind = new System.Windows.Forms.TextBox();
            this.btnFindNext = new MetaControls.MetaButton();
            this.btnFindPrevious = new MetaControls.MetaButton();
            this.cbMatchCase = new System.Windows.Forms.CheckBox();
            this.cbMatchEntireWord = new System.Windows.Forms.CheckBox();
            this.btnClose = new MetaControls.MetaButton();
            this.SuspendLayout();
            // 
            // txtTextToFind
            // 
            this.txtTextToFind.Location = new System.Drawing.Point(4, 2);
            this.txtTextToFind.Name = "txtTextToFind";
            this.txtTextToFind.Size = new System.Drawing.Size(212, 20);
            this.txtTextToFind.TabIndex = 0;
            this.txtTextToFind.TextChanged += new System.EventHandler(this.txtTextToFind_TextChanged);
            // 
            // btnFindNext
            // 
            this.btnFindNext.Location = new System.Drawing.Point(222, 2);
            this.btnFindNext.Name = "btnFindNext";
            this.btnFindNext.Size = new System.Drawing.Size(87, 23);
            this.btnFindNext.TabIndex = 1;
            this.btnFindNext.Text = "Find Next";
            this.btnFindNext.Click += new System.EventHandler(this.btnFindNext_Click);
            // 
            // btnFindPrevious
            // 
            this.btnFindPrevious.Location = new System.Drawing.Point(222, 25);
            this.btnFindPrevious.Name = "btnFindPrevious";
            this.btnFindPrevious.Size = new System.Drawing.Size(87, 23);
            this.btnFindPrevious.TabIndex = 2;
            this.btnFindPrevious.Text = "Find Previous";
            this.btnFindPrevious.Click += new System.EventHandler(this.btnFindPrevious_Click);
            // 
            // cbMatchCase
            // 
            this.cbMatchCase.AutoSize = true;
            this.cbMatchCase.Location = new System.Drawing.Point(13, 29);
            this.cbMatchCase.Name = "cbMatchCase";
            this.cbMatchCase.Size = new System.Drawing.Size(83, 17);
            this.cbMatchCase.TabIndex = 3;
            this.cbMatchCase.Text = "Match Case";
            this.cbMatchCase.UseVisualStyleBackColor = true;
            this.cbMatchCase.CheckedChanged += new System.EventHandler(this.cbMatchCase_CheckedChanged);
            // 
            // cbMatchEntireWord
            // 
            this.cbMatchEntireWord.AutoSize = true;
            this.cbMatchEntireWord.Location = new System.Drawing.Point(13, 52);
            this.cbMatchEntireWord.Name = "cbMatchEntireWord";
            this.cbMatchEntireWord.Size = new System.Drawing.Size(115, 17);
            this.cbMatchEntireWord.TabIndex = 4;
            this.cbMatchEntireWord.Text = "Match Entire Word";
            this.cbMatchEntireWord.UseVisualStyleBackColor = true;
            this.cbMatchEntireWord.CheckedChanged += new System.EventHandler(this.cbMatchEntireWord_CheckedChanged);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(222, 48);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(87, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FindTextWinForms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 78);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.cbMatchEntireWord);
            this.Controls.Add(this.cbMatchCase);
            this.Controls.Add(this.txtTextToFind);
            this.Controls.Add(this.btnFindPrevious);
            this.Controls.Add(this.btnFindNext);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FindTextWinForms";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Find Text";
            this.TopMost = true;
            this.Focus();
            this.BringToFront();
            this.TopMost = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTextToFind;
        private MetaControls.MetaButton btnFindNext;
        private MetaControls.MetaButton btnFindPrevious;
        private System.Windows.Forms.CheckBox cbMatchCase;
        private System.Windows.Forms.CheckBox cbMatchEntireWord;
        private MetaControls.MetaButton btnClose;
    }
}