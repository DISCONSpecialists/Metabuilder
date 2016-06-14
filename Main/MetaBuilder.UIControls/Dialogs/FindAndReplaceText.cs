using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Northwoods.Go;
using MetaBuilder.UIControls.GraphingUI;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Meta;
using System.Reflection;
using MetaBuilder.Docking;

namespace MetaBuilder.UIControls.Dialogs
{
    public partial class FindAndReplaceText : DockContent //Form
    {
        public enum Function { Find, Replace }

        private List<GoText> matches;
        private FindReplaceOptions options;
        private int SelectedIndex;

        public FindAndReplaceText()
        {
            InitializeComponent();
            panelFind.Visible = true;
        }

        public void SwitchPanel(Function function)
        {
            panelFind.Visible = false;
            toolStripButtonFind.Checked = false;
            toolStripButtonReplace.Checked = false;
            panelReplace.Visible = false;
            if (Core.Variables.Instance.IsViewer || DockingForm.DockForm.GetCurrentGraphViewContainer().ReadOnly)
            {
                function = Function.Find;
                toolStripButtonReplace.Visible = false;
                buttonReplace.Enabled = false;
            }
            switch (function)
            {
                case Function.Find:
                    panelFind.Visible = true;
                    //Height = 180;
                    //AcceptButton = buttonFindNext;
                    //CancelButton = buttonFindCancel;
                    toolStripButtonFind.Checked = true;
                    textBoxFindText.Focus();
                    break;
                case Function.Replace:
                    panelReplace.Visible = true;
                    //Height = 215;
                    //AcceptButton = buttonReplaceFind;
                    //CancelButton = buttonReplaceCancel;
                    toolStripButtonReplace.Checked = true;
                    textBoxReplaceFindText.Focus();
                    break;
                default:
                    break;
            }
        }
        public void Reset()
        {
            if (matches == null)
                return;

            matches = null;
            SelectedIndex = -1;

            options = new FindReplaceOptions();
            options.MatchCase = checkBoxFindMatchCase.Checked;
            options.WholeWord = checkBoxFindMatchWord.Checked;

            buttonReplace.Enabled = false;
            listBoxMatches.Visible = false;
            listBoxMatches.Items.Clear();
        }

        private void FindAndReplaceText_Load(object sender, EventArgs e)
        {
            //TODO load previous searches and replaces from registry?
        }

        private void FindReplace_Click(object sender, EventArgs e)
        {
            SwitchPanel((Function)Enum.Parse(typeof(Function), (sender as ToolStripButton).Text, true));
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ButtonFindNext_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Find(false);
        }
        private void ButtonFindPrevious_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Find(true);
        }

        private void ButtonReplace_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Replace();
        }

        private void Find(bool previous)
        {
            if (matches == null)
            {
                matches = new List<GoText>();

                if (options == null)
                    options = new FindReplaceOptions();
                options.MatchCase = checkBoxFindMatchCase.Checked;
                options.WholeWord = checkBoxFindMatchWord.Checked;

                SelectedIndex = -1;
                Search(textBoxFindText.Text, DockingForm.DockForm.GetCurrentGraphViewContainer().View.Document);
            }
            if (matches.Count > 0)
            {
                listBoxMatches.Items.Clear();
                foreach (GoText t in matches)
                {
                    listBoxMatches.Items.Add(t);
                }
                listBoxMatches.Visible = true;
                listBoxMatches.BringToFront();

                if (previous)
                    SelectedIndex--;
                else
                    SelectedIndex++;

                if (SelectedIndex >= 0 && SelectedIndex < matches.Count)
                {
                }
                else
                {
                    //cant have a null value at index :)
                    SelectedIndex = 0;
                }

                SelectMatch(matches[SelectedIndex]);

                listBoxMatches.SelectedIndex = SelectedIndex;

                buttonReplace.Enabled = true;
            }
            else
            {
                buttonReplace.Enabled = false;
                listBoxMatches.Visible = false;
                DockingForm.DockForm.DisplayTip("The find operation did not find anything on the current diagram.", "No matches found");
            }
        }

        private bool IsMatch(string TextToSearch, string TextToFind)
        {
            bool retval = true;
            if (options.MatchCase && options.WholeWord)
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
                    if ((txtobject is BoundLabel) && (txtobject as BoundLabel).Name == "cls_id") //do not search class name labels
                        continue;

                    if (IsMatch(txtobject.Text, txt))
                    {
                        matches.Add(txtobject);
                    }
                }

                if (o is GoGroup)
                {
                    Search(txt, o as GoGroup);
                }
            }
        }

        bool all = false;
        private void Replace()
        {
            if (buttonReplace.Text.Contains("All"))
            {
                all = true;
            }
            if (matches == null)
            {
                Find(false);
                //return;
            }
            GoText match = matches[SelectedIndex];
            if (match.ParentNode is IMetaNode)
            {
                MetaBase mBase = null;
                //change the property which was used not just the text
                if (match.Parent is IMetaNode)
                    mBase = (match.Parent as IMetaNode).MetaObject;
                else
                    mBase = (match.ParentNode as IMetaNode).MetaObject;

                if (mBase != null)
                {
                    foreach (PropertyInfo prop in mBase.GetMetaPropertyList(false))
                    {
                        if (mBase.Get(prop.Name) != null)
                        {
                            string t = mBase.Get(prop.Name).ToString();

                            //case must be tolower
                            int indexof = t.ToLower().IndexOf(textBoxReplaceFindText.Text.ToLower());
                            if (indexof == -1)
                                continue;

                            string replaced = t;
                            if (options.WholeWord && options.MatchCase)
                            {
                                replaced = t.Replace(textBoxReplaceFindText.Text, textBoxReplaceText.Text);
                            }
                            else if (options.MatchCase)
                            {
                                replaced = t.Replace(textBoxReplaceFindText.Text, textBoxReplaceText.Text);
                            }
                            else// if (options.WholeWord)
                            {
                                int length = textBoxReplaceFindText.Text.Length;
                                string actualValue = t.Substring(indexof, length);

                                replaced = t.Replace(actualValue, textBoxReplaceText.Text);
                            }
                            mBase.Set(prop.Name, replaced);
                        }
                    }
                }
            }
            else
            {
                string t = match.Text;
                string replaced = t;
                if (options.WholeWord && options.MatchCase)
                {
                    replaced = t.Replace(textBoxReplaceFindText.Text, textBoxReplaceText.Text);
                }
                else if (options.MatchCase)
                {
                    replaced = t.Replace(textBoxReplaceFindText.Text, textBoxReplaceText.Text);
                }
                else// if (options.WholeWord)
                {
                    //case must be tolower
                    int indexof = t.ToLower().IndexOf(textBoxReplaceFindText.Text.ToLower());
                    int length = textBoxReplaceFindText.Text.Length;

                    string actualValue = t.Substring(indexof, length);

                    replaced = t.Replace(actualValue, textBoxReplaceText.Text);
                }

                match.Text = replaced;
            }
            //go to next match
            Find(false);
            if (all)
                Replace();

            all = false;
        }

        bool auto = false;
        private void Find_TextChanged(object sender, EventArgs e)
        {
            if (auto)
                return;
            auto = true;
            //same text in both textboxes
            if ((sender as TextBox).Name.Contains("Replace"))
            {
                textBoxFindText.Text = textBoxReplaceFindText.Text;
            }
            else
            {
                textBoxReplaceFindText.Text = textBoxFindText.Text;
            }

            Reset();
            auto = false;
        }
        private void MatchCase_Changed(object sender, EventArgs e)
        {
            if (auto)
                return;
            auto = true;
            if ((sender as CheckBox).Name.Contains("Replace"))
            {
                checkBoxFindMatchCase.Checked = checkBoxReplaceMatchCase.Checked;
            }
            else
            {
                checkBoxReplaceMatchCase.Checked = checkBoxFindMatchCase.Checked;
            }

            Reset();
            auto = false;
        }
        private void MatchWord_Changed(object sender, EventArgs e)
        {
            if (auto)
                return;
            auto = true;
            if ((sender as CheckBox).Name.Contains("Replace"))
            {
                checkBoxFindMatchWord.Checked = checkBoxReplaceMatchWord.Checked;
            }
            else
            {
                checkBoxReplaceMatchWord.Checked = checkBoxFindMatchWord.Checked;
            }

            Reset();
            auto = false;
        }

        private void Text_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                Find(false);
            }
            else if (e.KeyCode == Keys.Enter)
            {
                if ((sender is TextBox) && (sender as TextBox).Name.Contains("ReplaceText"))
                    Replace();
                else
                    Find(false);
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                (sender as TextBox).Text = Clipboard.GetText();
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                if ((sender as TextBox).SelectedText.Length > 0)
                    Clipboard.SetText((sender as TextBox).SelectedText);
            }
        }

        private void listBoxMatches_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedIndex = listBoxMatches.SelectedIndex;
        }

        private void SelectMatch(GoText match)
        {
            try
            {
                DockingForm.DockForm.GetCurrentGraphViewContainer().View.Selection.Clear();
                DockingForm.DockForm.DontShowPane = true;
                DockingForm.DockForm.GetCurrentGraphViewContainer().View.Selection.Add(match.ParentNode);
                DockingForm.DockForm.DontShowPane = false;

                DockingForm.DockForm.GetCurrentGraphViewContainer().View.ScrollRectangleToVisible(match.ParentNode.Bounds);
            }
            catch (Exception ex)
            {
                DockingForm.DockForm.DontShowPane = false; //in case adding the node causes problem
                Core.Log.WriteLog(ex.ToString());
            }
        }

        private void listBoxMatches_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SelectMatch(matches[SelectedIndex]);
        }

    }

    public class FindReplaceOptions
    {

        #region Fields (3)

        private bool matchCase;
        private bool wholeWord;

        #endregion Fields

        #region Properties (3)

        public bool MatchCase
        {
            get { return matchCase; }
            set { matchCase = value; }
        }

        public bool WholeWord
        {
            get { return wholeWord; }
            set { wholeWord = value; }
        }

        #endregion Properties

    }

}