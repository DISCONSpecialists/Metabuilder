using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MetaBuilder.UIControls.Dialogs
{
    public partial class FindTextWinForms : Form
    {

		#region Fields (4) 

        private List<string> matches;
        private FindOptions options;
        private List<string> searchItems;
        private int SelectedIndex;

		#endregion Fields 

		#region Constructors (1) 

        public FindTextWinForms()
        {
            InitializeComponent();
        }

		#endregion Constructors 

		#region Properties (1) 

        public List<string> SearchItems
        {
            get { return searchItems; }
            set { searchItems = value; }
        }

		#endregion Properties 

		#region Delegates and Events (1) 


		// Events (1) 

        public event EventHandler FoundMatch;


		#endregion Delegates and Events 

		#region Methods (11) 


		// Protected Methods (1) 

        protected void OnFoundMatch(object sender, EventArgs e)
        {
            if (FoundMatch != null)
                FoundMatch(sender, e);
        }



		// Private Methods (10) 

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
            matches = new List<string>();
            SelectedIndex = -1;
        }

        private void cbMatchEntireWord_CheckedChanged(object sender, EventArgs e)
        {
            matches = new List<string>();
            SelectedIndex = -1;
        }

        private void DoFind()
        {
            Search(txtTextToFind.Text);
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
                    OnFoundMatch(matches[SelectedIndex], EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show(this,"No matches found", "No Matches", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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
            if (options.WholeWord)
            {
                retval = TextToSearch.ToLower() == TextToFind.ToLower();
                return retval;
            }
            if (options.MatchCase)
            {
                retval = TextToSearch.IndexOf(TextToFind) > -1;
                return retval;
            }
            retval = TextToSearch.ToLower().IndexOf(TextToFind.ToLower()) > -1;
            return retval;
        }

        private void Search(string txt)
        {
            foreach (string s in searchItems)
            {
                if (IsMatch(s, txt))
                {
                    matches.Add(s);
                }
            }
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
            matches = new List<string>();
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