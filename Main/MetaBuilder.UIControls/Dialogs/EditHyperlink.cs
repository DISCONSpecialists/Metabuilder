using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.General;
using MetaBuilder.UIControls.Common;

namespace MetaBuilder.UIControls.Dialogs
{
    public partial class EditHyperlink : Form
    {

		#region Fields (3) 

        private const int COMPACTPATHLENGTH = 44;
        private Hyperlink newHyperlink;
        private Hyperlink sourceHyperlink;

		#endregion Fields 

		#region Constructors (1) 

        public EditHyperlink()
        {
            InitializeComponent();
            SourceHyperlink = new Hyperlink();
        }

		#endregion Constructors 

		#region Properties (2) 

        public Hyperlink NewHyperlink
        {
            get { return newHyperlink; }
            set { newHyperlink = value; }
        }

        public Hyperlink SourceHyperlink
        {
            get { return sourceHyperlink; }
            set { sourceHyperlink = value; }
        }

		#endregion Properties 

		#region Methods (14) 

		// Private Methods (14) 

        private void BindForm()
        {

            #region Setup TableLayoutContainer according to hyperlink type
            switch (newHyperlink.HyperlinkType)
            {
                case HyperlinkType.Diagram:
                    lblFilename.Visible = true;
                    lblLeftFilename.Visible = true;
                    btnBrowse.Visible = true;

                    lblLeftBookmark.Visible = false;
                    comboBookmarks.Visible = false;
                    btnGetBookmarks.Visible = false;
                    lblLeftURI.Visible = false;
                    txtURI.Visible = false;
                    break;
                case HyperlinkType.File:
                    lblFilename.Visible = true;
                    lblLeftFilename.Visible = true;
                    btnBrowse.Visible = true;
                    lblLeftBookmark.Visible = true;
                    comboBookmarks.Visible = true;
                    btnGetBookmarks.Visible = true;

                    lblLeftURI.Visible = false;
                    txtURI.Visible = false;
                    break;
                case HyperlinkType.URL:
                    lblLeftURI.Visible = true;
                    txtURI.Visible = true;
                    lblLeftBookmark.Visible = false;
                    comboBookmarks.Visible = false;
                    btnGetBookmarks.Visible = false;
                    lblLeftFilename.Visible = false;
                    lblFilename.Visible = false;
                    btnBrowse.Visible = false;
                    break;
            }
            #endregion

            // Common Properties
            comboHyperlinkType.Text = NewHyperlink.HyperlinkType.ToString();
            txtCaption.Text = NewHyperlink.Text;
            linkPreview.Text = txtCaption.Text;
            switch (NewHyperlink.HyperlinkType)
            {
                case HyperlinkType.File:
                    lblFilename.Text = Core.strings.GetCompactPath(NewHyperlink.Arguments, COMPACTPATHLENGTH);
                    BindWordControlsIfNecessary();
                    
                    break;
                case HyperlinkType.Diagram:
                    lblFilename.Text = Core.strings.GetCompactPath(NewHyperlink.Arguments, COMPACTPATHLENGTH);
                    break;
                case HyperlinkType.URL:
                    txtURI.Text = NewHyperlink.Arguments;
                    break;
            }
        }

        private void BindWordControlsIfNecessary()
        {
            comboBookmarks.Enabled = newHyperlink.IsWordDocument;
            btnGetBookmarks.Enabled = newHyperlink.IsWordDocument;
            if (newHyperlink.BookmarkName != null)
                comboBookmarks.Text = newHyperlink.BookmarkName;
            else
                comboBookmarks.Text = "";
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Specify Hyperlink Target...";
            ofd.Filter = "All Files | *.*";
            ofd.SupportMultiDottedExtensions = true;
            ofd.Multiselect = false;
            DialogResult res = ofd.ShowDialog(this);

            string previousArgs = newHyperlink.Arguments;
            if (res == DialogResult.OK)
            {
                if (previousArgs.ToLower() != ofd.FileName.ToLower())
                {
                    newHyperlink.Arguments = ofd.FileName;
                    lblFilename.Text = Core.strings.GetCompactPath(ofd.FileName, COMPACTPATHLENGTH);
                    BindWordControlsIfNecessary();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnGetBookmarks_Click(object sender, EventArgs e)
        {
            List<string> bookmarks = MetaBuilder.BusinessFacade.Tools.WordHelper.GetBookmarksInDocument(newHyperlink.Arguments);
            comboBookmarks.Items.Clear();
            foreach (string s in bookmarks)
            {
                comboBookmarks.Items.Add(s);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (comboBookmarks.Enabled)
            {
                if (comboBookmarks.Text.Length > 0)
                    newHyperlink.BookmarkName = comboBookmarks.Text;
                else
                    newHyperlink.BookmarkName = null;
            }
            else
                newHyperlink.BookmarkName = null;

            if (comboHyperlinkType.Text == "URL")
                newHyperlink.Arguments = txtURI.Text;
            CopyLinkProperties(NewHyperlink, SourceHyperlink);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ChooseWordDocument_Load(object sender, EventArgs e)
        {
            newHyperlink = SourceHyperlink.Copy() as Hyperlink;
            CopyLinkProperties(SourceHyperlink, newHyperlink);
            comboHyperlinkType.Text = SourceHyperlink.HyperlinkType.ToString();
            BindForm();
        }

        private void comboBookmarks_SelectedIndexChanged(object sender, EventArgs e)
        {
            newHyperlink.BookmarkName = comboBookmarks.Text;
        }

        private void comboHyperlinkType_SelectedIndexChanged(object sender, EventArgs e)
        {
            newHyperlink.HyperlinkType =
                (HyperlinkType) Enum.Parse(typeof (HyperlinkType), comboHyperlinkType.Text);
            BindForm();
        }

        private void CopyLinkProperties(Hyperlink source, Hyperlink target)
        {
            target.HyperlinkType = source.HyperlinkType;
            target.Arguments = source.Arguments;
            target.BookmarkName = source.BookmarkName;
            target.Text = source.Text;
        }

        private void EditHyperlink_Click(object sender, EventArgs e)
        {
            if (comboHyperlinkType.Text == "URL")
                newHyperlink.Arguments = txtURI.Text;
        }

        private void linkPreview_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                GeneralUtil.LaunchHyperlink(newHyperlink);
            }
            catch
            {
                MessageBox.Show(this,"Cannot launch hyperlink, check the properties and try again.");
            }
        }

        private void txtCaption_Leave(object sender, EventArgs e)
        {
            newHyperlink.Text = txtCaption.Text;
        }
        private void txtCaption_TextChanged(object sender, EventArgs e)
        {
            linkPreview.Text = txtCaption.Text;
            newHyperlink.Text = txtCaption.Text;
        }

        private void txtURI_Leave(object sender, EventArgs e)
        {
            if (comboHyperlinkType.Text == "URL")
                newHyperlink.Arguments = txtURI.Text;
        }

		#endregion Methods 

    }
}