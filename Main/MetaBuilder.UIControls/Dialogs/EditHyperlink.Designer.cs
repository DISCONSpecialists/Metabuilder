namespace MetaBuilder.UIControls.Dialogs
{
    partial class EditHyperlink
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
            this.btnBrowse = new MetaControls.MetaButton();
            this.lblLeftFilename = new System.Windows.Forms.Label();
            this.comboBookmarks = new System.Windows.Forms.ComboBox();
            this.lblLeftBookmark = new System.Windows.Forms.Label();
            this.btnGetBookmarks = new MetaControls.MetaButton();
            this.lblFilename = new System.Windows.Forms.Label();
            this.linkPreview = new System.Windows.Forms.LinkLabel();
            this.lblLeftPreview = new System.Windows.Forms.Label();
            this.txtCaption = new System.Windows.Forms.TextBox();
            this.lblLeftCaption = new System.Windows.Forms.Label();
            this.comboHyperlinkType = new System.Windows.Forms.ComboBox();
            this.txtURI = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblLeftURI = new System.Windows.Forms.Label();
            this.btnOK = new MetaControls.MetaButton();
            this.btnCancel = new MetaControls.MetaButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tableLayoutPanel1.SetColumnSpan(this.btnBrowse, 2);
            this.btnBrowse.Location = new System.Drawing.Point(379, 95);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(149, 23);
            this.btnBrowse.TabIndex = 5;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // lblLeftFilename
            // 
            this.lblLeftFilename.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblLeftFilename.AutoSize = true;
            this.lblLeftFilename.Location = new System.Drawing.Point(46, 100);
            this.lblLeftFilename.Name = "lblLeftFilename";
            this.lblLeftFilename.Size = new System.Drawing.Size(52, 13);
            this.lblLeftFilename.TabIndex = 1;
            this.lblLeftFilename.Text = "Filename:";
            // 
            // comboBookmarks
            // 
            this.comboBookmarks.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.comboBookmarks.FormattingEnabled = true;
            this.comboBookmarks.Location = new System.Drawing.Point(104, 125);
            this.comboBookmarks.Name = "comboBookmarks";
            this.comboBookmarks.Size = new System.Drawing.Size(269, 21);
            this.comboBookmarks.TabIndex = 6;
            this.comboBookmarks.SelectedIndexChanged += new System.EventHandler(this.comboBookmarks_SelectedIndexChanged);
            // 
            // lblLeftBookmark
            // 
            this.lblLeftBookmark.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblLeftBookmark.AutoSize = true;
            this.lblLeftBookmark.Location = new System.Drawing.Point(40, 129);
            this.lblLeftBookmark.Name = "lblLeftBookmark";
            this.lblLeftBookmark.Size = new System.Drawing.Size(58, 13);
            this.lblLeftBookmark.TabIndex = 1;
            this.lblLeftBookmark.Text = "Bookmark:";
            // 
            // btnGetBookmarks
            // 
            this.btnGetBookmarks.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tableLayoutPanel1.SetColumnSpan(this.btnGetBookmarks, 2);
            this.btnGetBookmarks.Location = new System.Drawing.Point(379, 124);
            this.btnGetBookmarks.Name = "btnGetBookmarks";
            this.btnGetBookmarks.Size = new System.Drawing.Size(149, 23);
            this.btnGetBookmarks.TabIndex = 7;
            this.btnGetBookmarks.Text = "Get Bookmarks";
            this.btnGetBookmarks.Click += new System.EventHandler(this.btnGetBookmarks_Click);
            // 
            // lblFilename
            // 
            this.lblFilename.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFilename.AutoSize = true;
            this.lblFilename.Location = new System.Drawing.Point(104, 100);
            this.lblFilename.Name = "lblFilename";
            this.lblFilename.Size = new System.Drawing.Size(52, 13);
            this.lblFilename.TabIndex = 1;
            this.lblFilename.Text = "Filename:";
            // 
            // linkPreview
            // 
            this.linkPreview.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.linkPreview.AutoSize = true;
            this.linkPreview.Location = new System.Drawing.Point(104, 79);
            this.linkPreview.Name = "linkPreview";
            this.linkPreview.Size = new System.Drawing.Size(92, 13);
            this.linkPreview.TabIndex = 4;
            this.linkPreview.TabStop = true;
            this.linkPreview.Text = "Preview Hyperlink";
            this.linkPreview.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkPreview_LinkClicked);
            // 
            // lblLeftPreview
            // 
            this.lblLeftPreview.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblLeftPreview.AutoSize = true;
            this.lblLeftPreview.Location = new System.Drawing.Point(3, 79);
            this.lblLeftPreview.Name = "lblLeftPreview";
            this.lblLeftPreview.Size = new System.Drawing.Size(95, 13);
            this.lblLeftPreview.TabIndex = 5;
            this.lblLeftPreview.Text = "Preview Hyperlink:";
            // 
            // txtCaption
            // 
            this.txtCaption.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tableLayoutPanel1.SetColumnSpan(this.txtCaption, 3);
            this.txtCaption.Location = new System.Drawing.Point(104, 56);
            this.txtCaption.Name = "txtCaption";
            this.txtCaption.Size = new System.Drawing.Size(349, 20);
            this.txtCaption.TabIndex = 3;
            this.txtCaption.Leave += new System.EventHandler(this.txtCaption_Leave);
            this.txtCaption.TextChanged +=new System.EventHandler(txtCaption_TextChanged);
            // 
            // lblLeftCaption
            // 
            this.lblLeftCaption.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblLeftCaption.AutoSize = true;
            this.lblLeftCaption.Location = new System.Drawing.Point(23, 59);
            this.lblLeftCaption.Name = "lblLeftCaption";
            this.lblLeftCaption.Size = new System.Drawing.Size(75, 13);
            this.lblLeftCaption.TabIndex = 1;
            this.lblLeftCaption.Text = "Label Caption:";
            // 
            // comboHyperlinkType
            // 
            this.comboHyperlinkType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.comboHyperlinkType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboHyperlinkType.FormattingEnabled = true;
            this.comboHyperlinkType.Items.AddRange(new object[] {
            "File",
            "Diagram",
            "URL"});
            this.comboHyperlinkType.Location = new System.Drawing.Point(104, 3);
            this.comboHyperlinkType.Name = "comboHyperlinkType";
            this.comboHyperlinkType.Size = new System.Drawing.Size(89, 21);
            this.comboHyperlinkType.TabIndex = 1;
            this.comboHyperlinkType.SelectedIndexChanged += new System.EventHandler(this.comboHyperlinkType_SelectedIndexChanged);
            // 
            // txtURI
            // 
            this.txtURI.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tableLayoutPanel1.SetColumnSpan(this.txtURI, 3);
            this.txtURI.Location = new System.Drawing.Point(104, 30);
            this.txtURI.Name = "txtURI";
            this.txtURI.Size = new System.Drawing.Size(349, 20);
            this.txtURI.TabIndex = 2;
            this.txtURI.Leave += new System.EventHandler(this.txtURI_Leave);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Hyperlink Type:";
            // 
            // lblLeftURI
            // 
            this.lblLeftURI.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblLeftURI.AutoSize = true;
            this.lblLeftURI.Location = new System.Drawing.Point(69, 33);
            this.lblLeftURI.Name = "lblLeftURI";
            this.lblLeftURI.Size = new System.Drawing.Size(29, 13);
            this.lblLeftURI.TabIndex = 1;
            this.lblLeftURI.Text = "URI:";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(379, 153);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(70, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(455, 153);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(73, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 275F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 86F));
            this.tableLayoutPanel1.Controls.Add(this.btnGetBookmarks, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnCancel, 3, 6);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnBrowse, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.comboBookmarks, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.comboHyperlinkType, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.linkPreview, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblLeftBookmark, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblLeftPreview, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblFilename, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblLeftFilename, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtCaption, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtURI, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblLeftURI, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblLeftCaption, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnOK, 2, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(538, 180);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // EditHyperlink
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 180);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "EditHyperlink";
            this.Text = "Edit Hyperlink";
            this.Click += new System.EventHandler(this.EditHyperlink_Click);
            this.Load += new System.EventHandler(this.ChooseWordDocument_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MetaControls.MetaButton btnBrowse;
        private System.Windows.Forms.Label lblLeftFilename;
        private System.Windows.Forms.ComboBox comboBookmarks;
        private System.Windows.Forms.Label lblLeftBookmark;
        private MetaControls.MetaButton btnGetBookmarks;
        private System.Windows.Forms.Label lblFilename;
        private System.Windows.Forms.LinkLabel linkPreview;
        private System.Windows.Forms.Label lblLeftPreview;
        public System.Windows.Forms.TextBox txtCaption;
        private System.Windows.Forms.Label lblLeftCaption;
        private System.Windows.Forms.ComboBox comboHyperlinkType;
        private System.Windows.Forms.TextBox txtURI;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblLeftURI;
        private MetaControls.MetaButton btnOK;
        private MetaControls.MetaButton btnCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}