using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MetaBuilder.UIControls.GraphingUI;
using Northwoods.Go;

namespace MetaBuilder.UIControls.Dialogs
{
    public partial class FindText : Form
    {

        #region Fields (4)

        private List<GoObject> matches;
        private FindOptions options;
        private int SelectedIndex;
        private GoView view;

        #endregion Fields

        #region Constructors (1)

        public FindText()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods (11)

        // Private Methods (11) 

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnFindNext_Click(object sender, EventArgs e)
        {
            SetupOptions(false);
            DoFind();
        }

        private void btnFindPrevious_Click(object sender, EventArgs e)
        {
            SetupOptions(true);
            DoFind();
        }

        private void cbMatchCase_CheckedChanged(object sender, EventArgs e)
        {
            matches = new List<GoObject>();
            SelectedIndex = -1;
        }

        private void cbMatchEntireWord_CheckedChanged(object sender, EventArgs e)
        {
            matches = new List<GoObject>();
            SelectedIndex = -1;
        }

        private void DoFind()
        {
            view = DockingForm.DockForm.GetCurrentGraphView();
            if (view == null || view.Document == null)
                return;
            Search(txtTextToFind.Text, view.Document);
            if (matches.Count > 0)
            {
                if (options.Reverse)
                {
                    if (SelectedIndex == -1)
                        SelectedIndex = matches.Count - 1;
                    else
                        SelectedIndex--;
                }
                else
                {
                    SelectedIndex++;
                }
                if (SelectedIndex >= 0 && SelectedIndex < matches.Count)
                {
                    view.Selection.Clear();
                    view.Selection.Add(matches[SelectedIndex]);
                    view.ScrollRectangleToVisible(matches[SelectedIndex].Bounds);
                }

            }
            else
            {
                //DockingForm.DockForm.DisplayTip("No matches found", "Find");
                MessageBox.Show(this,"No matches found", "No Matches", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void FindText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnFindNext_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F3)
            {
                btnFindNext_Click(sender, e);
            }
            else if (e.KeyCode == Keys.A && e.Control)
            {
                txtTextToFind.SelectAll();
            }
        }

        private bool IsMatch(string TextToSearch, string TextToFind)
        {
            bool retval = true;
            if (options.WholeWord && options.MatchCase)
            {
                retval = TextToSearch == TextToFind;
                return retval;
            }
            else if (options.WholeWord)
            {
                retval = TextToSearch.ToLower() == TextToFind.ToLower();
                return retval;
            }
            else if (options.MatchCase)
            {
                retval = TextToSearch.IndexOf(TextToFind) > -1;
                return retval;
            }
            retval = TextToSearch.ToLower().Contains(TextToFind.ToLower());
            return retval;
        }

        private void Search(string txt, IGoCollection collection)
        {
            foreach (GoObject o in collection)
            {
                if (o is MetaBuilder.Graphing.Containers.FrameLayerGroup || o.ParentNode is MetaBuilder.Graphing.Containers.FrameLayerGroup)
                    continue;

                if (o is GoText)
                {
                    GoText txtobject = o as GoText;
                    if (IsMatch(txtobject.Text, txt))
                    {
                        matches.Add(txtobject.ParentNode);
                    }
                }
                if (o is GoGroup)
                {
                    Search(txt, o as GoGroup);
                }
            }
            /*
            Selection.Clear();
            Selection.Add(txtobject.ParentNode);
            ScrollRectangleToVisible(txtobject.ParentNode.Bounds);
             */
        }

        private void SetupOptions(bool isReverse)
        {
            options = new FindOptions();
            options.Reverse = isReverse;
            options.MatchCase = cbMatchCase.Checked;
            options.WholeWord = cbMatchEntireWord.Checked;
        }

        private void txtTextToFind_TextChanged(object sender, EventArgs e)
        {
            matches = new List<GoObject>();
            SelectedIndex = -1;
        }

        #endregion Methods

        #region Nested Classes (1)

        public class FindOptions
        {

            #region Fields (3)

            private bool matchCase;
            private bool reverse;
            private bool wholeWord;

            #endregion Fields

            #region Properties (3)

            public bool MatchCase
            {
                get { return matchCase; }
                set { matchCase = value; }
            }

            public bool Reverse
            {
                get { return reverse; }
                set { reverse = value; }
            }

            public bool WholeWord
            {
                get { return wholeWord; }
                set { wholeWord = value; }
            }

            #endregion Properties

        }

        #endregion Nested Classes

    }
}