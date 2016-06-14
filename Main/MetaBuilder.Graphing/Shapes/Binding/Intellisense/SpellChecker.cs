using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer;
using NetSpell.SpellChecker;
using NetSpell.SpellChecker.Dictionary;
using Northwoods.Go;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
using System.Collections;
using System.Collections.ObjectModel;

namespace MetaBuilder.Graphing.Shapes.Binding.Intellisense
{
    public class SpellChecker
    {
        public static SpellChecker StaticChecker;

        #region Fields (8)

        private readonly List<IntellisenseMatch> itemlist = new List<IntellisenseMatch>();
        private readonly Spelling spelling = Variables.spelling;
        //private readonly NHunspell.Hunspell newSpelling = Core.Variables.newSpelling;
        private readonly WordDictionary wordDictionary = Variables.dictionary; //new WordDictionary();
        private int EndWordIndex;
        private int StartWordIndex;
        public bool UseIntellisense;
        private DataSet ds = new DataSet();
        private ListBox listBoxSuggestion;

        #endregion Fields

        #region Properties

        private bool allowIntellisense = true;
        private bool allowSpellChecking = true;
        private TextBoxBase box;
        private string lookupClass;
        private GoView myView;

        public TextBoxBase MyTextBox
        {
            get { return box; }
            set { box = value; }
        }

        public GoView MyGoView
        {
            get { return myView; }
            set { myView = value; }
        }

        public bool AllowIntellisense
        {
            get { return allowIntellisense; }
            set { allowIntellisense = value; }
        }

        public bool AllowSpellChecking
        {
            get { return allowSpellChecking; }
            set { allowSpellChecking = value; }
        }

        public string LookupClass
        {
            get { return lookupClass; }
            set { lookupClass = value; }
        }

        #endregion

        #region Global

        //Constructor
        public SpellChecker()
        {
            Console.WriteLine("Spellchecker created");
            //Disablanation
            AllowSpellChecking = Variables.Instance.SpellCheckEnabled;
            AllowIntellisense = Variables.Instance.IntellisenseEnabled;

            wordDictionary.DictionaryFolder = Application.StartupPath;
            wordDictionary.DictionaryFile = "en-ZA.dic";
            spelling.Dictionary = wordDictionary;
            spelling.Dictionary.EnableUserFile = true;
            //foreach (Control control in spelling.SuggestionForm.Controls)
            //{
            //    if (control is Button)
            //    {
            //        if ((control as Button).Text.Contains("Options"))
            //        {
            //            control.Visible = false;
            //            break;
            //        }
            //    }
            //}

            //spelling.EndOfText += new Spelling.EndOfTextEventHandler(spelling_EndOfText);
        }
        //private void spelling_EndOfText(object sender, EventArgs e)
        //{
        //    if (MyTextBox != null)
        //    {
        //        MyTextBox.Text = (sender as Spelling).Text;
        //    }
        //    //if (internalBox != null)
        //    //    internalBox.Text = spelling.Text;
        //}
        public void DisposeListBox(ListBox list)
        {
            list.VisibleChanged -= (listBoxSuggestion_VisibleChanged);
            list.DoubleClick -= (listBoxSuggestion_DoubleClick);
            list.MouseUp -= (listBoxSuggestion_MouseUp);
            list.KeyDown -= (listBoxSuggestion_KeyDown);
            list.LostFocus -= (listBoxSuggestion_LostFocus);
            list.Leave -= (listBoxSuggestion_Leave);
        }
        public ListBox InitListBox()
        {
            listBoxSuggestion = new ListBox();
            listBoxSuggestion.Name = "listBoxSuggestion";
            listBoxSuggestion.DrawMode = DrawMode.Normal;
            listBoxSuggestion.TabIndex = 4;
            listBoxSuggestion.Hide();
            listBoxSuggestion.Visible = false;
            listBoxSuggestion.AutoSize = true;
            listBoxSuggestion.IntegralHeight = true;
            listBoxSuggestion.BorderStyle = BorderStyle.FixedSingle;
            listBoxSuggestion.ScrollAlwaysVisible = true;
            listBoxSuggestion.MaximumSize = new Size(1000, 150);
            listBoxSuggestion.Width = 200;
            listBoxSuggestion.Height = 100;
            //listBoxSuggestion.ItemHeight = 1;
            //Events
            listBoxSuggestion.VisibleChanged += (listBoxSuggestion_VisibleChanged);
            //Required events
            listBoxSuggestion.DoubleClick += (listBoxSuggestion_DoubleClick);
            listBoxSuggestion.MouseUp += (listBoxSuggestion_MouseUp);
            listBoxSuggestion.KeyDown += (listBoxSuggestion_KeyDown); //Changed from KeyUp
            listBoxSuggestion.LostFocus += (listBoxSuggestion_LostFocus);
            listBoxSuggestion.Leave += (listBoxSuggestion_Leave);
            //Needed if you want to see it
            return listBoxSuggestion;
        }

        #endregion

        #region ListBoxEvent Global (switches between either spelling or intellisense)

        public bool ListFocused;

        private void listBoxSuggestion_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //Point point = new Point(listBoxSuggestion.Location.X + e.X, e.Y + listBoxSuggestion.Location.Y);
                listBoxSuggestion.SelectedIndex = listBoxSuggestion.IndexFromPoint(new Point(e.X, e.Y));
                listBoxSuggestion.Hide();
                replaceWord(true);
            }
        }

        private void listBoxSuggestion_VisibleChanged(object sender, EventArgs e)
        {
            if (listBoxSuggestion.Visible)
                listBoxSuggestion.BringToFront();
            else
                listBoxSuggestion.SendToBack();
        }

        private void listBoxSuggestion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
            {
                replaceWord(false);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                //Tag reset!
                MyTextBox.Tag = null;

                MyTextBox.DeselectAll();
                MyTextBox.Select(MyTextBox.Text.Length, 0);
                listBoxSuggestion.Hide();
                MyTextBox.Focus();
            }

            //This is there event, the 2 zeros im guessing are the start of word index and length index
            //spelling_ReplacedWord(listBoxSuggestion,new ReplaceWordEventArgs(getWord(demoRichText.Text),listBoxSuggestion.SelectedItem.ToString(), 0, 0));
        }

        private void listBoxSuggestion_DoubleClick(object sender, EventArgs e)
        {
            MouseEventArgs q = e as MouseEventArgs;
            listBoxSuggestion.SelectedIndex = listBoxSuggestion.IndexFromPoint(new Point(q.X, q.Y));
            listBoxSuggestion.Hide();
            replaceWord(false);

            //This is their event, the 2 zeros im guessing are the start of word index and length index
            //spelling_ReplacedWord(listBoxSuggestion,new ReplaceWordEventArgs(getWord(demoRichText.Text),listBoxSuggestion.SelectedItem.ToString(), 0, 0));
        }

        private void listBoxSuggestion_Leave(object sender, EventArgs e)
        {
            //LostFocus-->Leave-->LeaveEnd-->LostFocusEnd

            ////Tag reset!
            //MyTextBox.Tag = null;

            //MyTextBox.DeselectAll();
            //MyTextBox.Select(MyTextBox.Text.Length, 0);
            //listBoxSuggestion.Hide();
            //MyTextBox.Focus();
        }

        private void listBoxSuggestion_LostFocus(object sender, EventArgs e)
        {
            if (ListFocused)
                return;
            //Tag reset!
            MyTextBox.Tag = null;

            MyTextBox.DeselectAll();
            MyTextBox.Select(MyTextBox.Text.Length, 0);
            listBoxSuggestion.Hide();
            MyTextBox.Focus();
        }

        #endregion

        #region SpellChecker

        private PointF startPoint;
        public PointF StartPoint
        {
            get
            {
                return new PointF(startPoint.X, startPoint.Y + this.MyTextBox.Font.Height);
                return startPoint;
            }
            set { startPoint = value; }
        }
        private PointF endPoint;
        public PointF EndPoint
        {
            get
            {
                return new PointF(endPoint.X, endPoint.Y + this.MyTextBox.Font.Height);
                return endPoint;
            }
            set { endPoint = value; }
        }

        //TODO : REPLACE Gets the last word
        private string getWord()
        {
            string word = "";

            int pos = MyTextBox.SelectionStart;
            int startOfWord = MyTextBox.Text.Contains(" ") ? MyTextBox.Text.LastIndexOf(" ") : 0;
            int lengthOfWord = pos - startOfWord;

            if (MyTextBox.Text.Contains(" "))
            {
                StartPoint = MyTextBox.GetPositionFromCharIndex(startOfWord + lengthOfWord);
                EndPoint = MyTextBox.GetPositionFromCharIndex(startOfWord);

                if (lengthOfWord > 0)
                {
                    //this is great for 2 or more words
                    //WHEN EDITING THE LAST WORD
                    word = MyTextBox.Text.Substring(startOfWord, lengthOfWord);
                }
                else
                {

                    //lengthofword will be negative
                    //startofword will be incorrect as a result
                    //recalculate
                    string textBeforePos = MyTextBox.Text.Substring(0, pos);
                    startOfWord = textBeforePos.LastIndexOf(" ");
                    lengthOfWord = pos - startOfWord;

                    StartPoint = MyTextBox.GetPositionFromCharIndex(startOfWord + lengthOfWord);
                    EndPoint = MyTextBox.GetPositionFromCharIndex(startOfWord);

                    //this is when someone edits INBETWEEN TEXT
                    if (startOfWord >= 0 & lengthOfWord > 0)
                        word = textBeforePos.Substring(startOfWord, lengthOfWord);
                    else
                    {
                        textBeforePos.ToString();
                    }
                }
            }
            else
            {
                StartPoint = MyTextBox.GetPositionFromCharIndex(MyTextBox.Text.Length - 1);
                EndPoint = MyTextBox.GetPositionFromCharIndex(0);
                //mwhahaha
                word = MyTextBox.Text;
            }

            return word.Trim();
        }

        //Replaces word in the textbox with a selected one from listbox containing suggestions
        public void replaceWord(bool asShallow)
        {
            if (listBoxSuggestion.SelectedIndex == -1)
                return;
            if (listBoxSuggestion.Items[listBoxSuggestion.SelectedIndex].ToString() == "No Values")
            {
                return;
            }

            #region Intellisense

            if (UseIntellisense)
            {
                MyTextBox.Tag = listBoxSuggestion.SelectedItem;
                //When CreateEditorFires this will be lost! Set tag to the node or something -- BindedToIntellisenseBox

                IntellisenseMatch matchItem = listBoxSuggestion.SelectedItem as IntellisenseMatch;
                if (matchItem != null)
                {
                    IntellisenseBox itBox = MyTextBox as IntellisenseBox;
                    if (itBox != null)
                    {
                        itBox.asShallow = asShallow;
                        itBox.IntellisenseMatchItem = matchItem;
                        itBox.asShallow = false;

                        MyTextBox.Text = itBox.IntellisenseMatchItem.StringValue;
                        box.SelectionStart = box.Text.Length;
                    }
                }
                listBoxSuggestion.DataSource = null;
                listBoxSuggestion.DataBindings.Clear();
                listBoxSuggestion.Items.Clear();

                //Tag reset!
                MyTextBox.Tag = null;

                MyTextBox.DeselectAll();
                MyTextBox.Select(MyTextBox.Text.Length, 0);
                listBoxSuggestion.Hide();
                MyTextBox.Focus();

                //reset to false for the spell checker
                UseIntellisense = false;

                return;
            }

            #endregion

            #region Normal Replacement

            //textBoxControl
            //string wordToReplace = getWord();
            if (listBoxSuggestion.SelectedIndex < 0) return;
            string wordReplacement = listBoxSuggestion.Items[listBoxSuggestion.SelectedIndex].ToString();
            IntellisenseMatch matchItemX = listBoxSuggestion.SelectedItem as IntellisenseMatch;
            //ctrl + space + double click!
            if (matchItemX != null)
            {
                IntellisenseBox itBox = MyTextBox as IntellisenseBox;
                if (itBox != null)
                {
                    itBox.IntellisenseMatchItem = matchItemX;

                    MyTextBox.Text = itBox.IntellisenseMatchItem.StringValue;
                    box.SelectionStart = box.Text.Length;
                    if (MyTextBox is IntellisenseBox)
                        (MyTextBox as IntellisenseBox).AcceptTextPublic();
                }
            }
            //Normal spelling -->customSuggestions
            else
            {
                MyTextBox.Text = wordReplacement;
                listBoxSuggestion.Hide();
                MyTextBox.Tag = null;
                //MyTextBox.AcceptTextPublic();
                if (MyTextBox is IntellisenseBox)
                    (MyTextBox as IntellisenseBox).AcceptTextPublic();
                ////this below bit was old replace text spelling function
                //if (MyTextBox.Text.Contains(wordToReplace))
                //{
                //    listBoxSuggestion.Visible = false;

                //    //get index of the caret
                //    int caretIndex = MyTextBox.SelectionStart;
                //    int startIndex = (caretIndex - wordToReplace.Length);
                //    if (startIndex < 0)
                //        startIndex = 0;

                //    //Casing?
                //    MyTextBox.Text = MyTextBox.Text.Remove(startIndex, wordToReplace.Length);
                //    MyTextBox.Text = MyTextBox.Text.Insert(startIndex, wordReplacement);

                //    //Tag reset!
                //    MyTextBox.Tag = null;

                //    MyTextBox.DeselectAll();
                //    MyTextBox.Select(MyTextBox.Text.Length, 0);
                //    listBoxSuggestion.Hide();
                //    MyTextBox.Focus();
                //}
            }

            #endregion
        }

        //Called to manually check spelling in form
        //private TextBoxBase internalBox = null;
        public void manualCheck()
        {
            //internalBox = MyTextBox;
            //spelling.Text = MyTextBox.Text;
            //spelling.AlertComplete = false;
            //spelling.ShowDialog = true;
            //if (!spelling.SuggestionForm.IsDisposed)
            //{
            //    spelling.SpellCheck();
            //    return;
            //}

            Spelling spel = new Spelling();
            spel.Dictionary = spelling.Dictionary;
            spel.ShowDialog = true;
            spel.Text = MyTextBox.Text;
            spel.AlertComplete = false;
            //spel.EndOfText += new Spelling.EndOfTextEventHandler(spelling_EndOfText);
            spel.SpellCheck();
        }

        private bool hasSuggestions;
        public bool HasSuggestions
        {
            get { return hasSuggestions; }
            set { hasSuggestions = value; }
        }

        //Populates a listbox with suggestions
        string wordToReplace = "";
        public void getSuggestions()
        {
            HasSuggestions = false;
            wordDictionary.DictionaryFolder = Application.StartupPath;
            wordDictionary.DictionaryFile = "en-ZA.dic";
            spelling.Dictionary = wordDictionary;
            spelling.SuggestionMode = Spelling.SuggestionEnum.NearMiss;

            //User property ability to disable spellchecking, intellisense disabled in intellisenseSuggestion below
            if (!AllowSpellChecking)
                return;

            //Must be whole string
            string word = getWord();
            StartWordIndex = MyTextBox.SelectionStart - word.Length;
            if (StartWordIndex < 0)
                StartWordIndex = 0;
            EndWordIndex = MyTextBox.SelectionStart;
            if (EndWordIndex < 0)
                EndWordIndex = 0;

            listBoxSuggestion.DataSource = null;
            listBoxSuggestion.DataBindings.Clear();
            listBoxSuggestion.Items.Clear();

            try
            {
                wordToReplace = word;
                spelling.Suggest(word);
            }
            catch
            {
                MessageBox.Show("Dictionary File Cannot Be found At Specified Location" + Environment.NewLine + Environment.NewLine + wordDictionary.DictionaryFolder + " - " + wordDictionary.DictionaryFile, "Dictionary File Cannot Be Found");
                return;
            }

            List<string> mylist = new List<string>();
            HasSuggestions = false;
            foreach (string s in spelling.Suggestions)
            {
                HasSuggestions = true;
                mylist.Add(s);
            }

            foreach (string s in mylist)
            {
                if (s.Length > 0)
                    listBoxSuggestion.Items.Add(s);
            }
            //listBoxSuggestion.Sorted = true;

            Point point = MyTextBox.GetPositionFromCharIndex(MyTextBox.SelectionStart);
            point.Y += MyTextBox.Bottom - 5; //(int)Math.Ceiling(MyTextBox.Font.GetHeight()) + 2;
            point.X += MyTextBox.Left; //4; // for Courier, may need a better method
            listBoxSuggestion.Location = point;

            if (listBoxSuggestion.Items.Count > 0)
            {
                listBoxSuggestion.Width = MyTextBox.Width;
                listBoxSuggestion.Show();
                listBoxSuggestion.Visible = true;
                listBoxSuggestion.BringToFront();
            }
            else
            {
                listBoxSuggestion.Hide();
                listBoxSuggestion.Visible = false;
                listBoxSuggestion.SendToBack();
                return;
            }
        }

        public void getCustomSuggestion(string word)
        {
            word = word.Trim();
            if (word.Length < 2)
                return;

            listBoxSuggestion.DataSource = null;
            listBoxSuggestion.DataBindings.Clear();
            listBoxSuggestion.Items.Clear();

            //List<string> mylist = new List<string>();
            HasSuggestions = false;

            Collection<string> sugg = (Collection<string>)myView.GetType().GetProperty("SuggestionList").GetValue(myView, null);
            if (sugg != null)
            {
                foreach (string s in sugg)
                {
                    if (s.ToLower().StartsWith(word.ToLower()))
                    {
                        HasSuggestions = true;
                        listBoxSuggestion.Items.Add(s);
                    }
                }
            }

            listBoxSuggestion.Sorted = true;
            listBoxSuggestion.Location = new Point(MyTextBox.Left, MyTextBox.Bottom - 5); ;

            if (listBoxSuggestion.Items.Count > 0)
            {
                listBoxSuggestion.Width = MyTextBox.Width;
                listBoxSuggestion.Show();
                listBoxSuggestion.Visible = true;
                listBoxSuggestion.BringToFront();
            }
            else
            {
                listBoxSuggestion.Hide();
                listBoxSuggestion.Visible = false;
                listBoxSuggestion.SendToBack();
                return;
            }
        }
        public System.Collections.ArrayList getSuggestionList(string w)
        {
            HasSuggestions = false;
            wordDictionary.DictionaryFolder = Application.StartupPath;
            wordDictionary.DictionaryFile = "en-ZA.dic";
            //wordDictionary.EnableUserFile = true;
            spelling.Dictionary = wordDictionary;
            spelling.SuggestionMode = Spelling.SuggestionEnum.PhoneticNearMiss;

            try
            {
                if (spelling.Dictionary.UserWords.ContainsKey(w))
                    return new ArrayList();
                spelling.Suggest(w);
            }
            catch
            {
                MessageBox.Show("Dictionary File Cannot Be found At Specified Location" + Environment.NewLine + Environment.NewLine + wordDictionary.DictionaryFolder + " - " + wordDictionary.DictionaryFile, "Dictionary File Cannot Be Found");
            }
            return spelling.Suggestions;
        }

        #endregion

        #region Intellisense

        public void intellisenseSuggestion(string className, string fieldName)
        {
            //To cancel intellisense altogether
            if (!AllowIntellisense)
                return;
            //Must be whole string
            //string word = getWord();
            string word = MyTextBox.Text;

            listBoxSuggestion.DataSource = null;
            listBoxSuggestion.DataBindings.Clear();
            listBoxSuggestion.Items.Clear();
            listBoxSuggestion.Hide();

            #region Get Suggestions

            //Check if we must use intellisense from form and property
            if (UseIntellisense && AllowIntellisense)
            {
                ds.Tables.Clear();
                ds.Clear();
                if (className != "" && fieldName != "" && className != null && fieldName != null)
                {
                    //the word is now all the text!
                    word = MyTextBox.Text.Trim();

                    itemlist.Clear();

                    if (word != "")
                    {
                        string query;
                        if (word.Trim() == "*")
                        {
                            query = "SELECT pkID, Machine, [" + fieldName.ToLower() + "],WorkSpaceName FROM METAVIEW_" +
                                    className.ToLower() + "_Listing WHERE VCStatusID <> 4 AND VCStatusID <> 8 AND VCStatusID <> 3";
                            // WHERE [" + fieldName.ToLower() + "] LIKE '%" + word.ToLower() + "%'";
                            ds = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ExecuteDataSet(CommandType.Text, query);

                            IntellisenseMatch item;
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                item = new IntellisenseMatch();
                                item.PKid = (int)row["pkID"];
                                item.MachineID = row["Machine"].ToString();
                                item.StringValue = row[fieldName].ToString();
                                item.WorkSpaceName = row["WorkSpaceName"].ToString();
                                item._Class = className;
                                if (item.StringValue.Length > 0)
                                    itemlist.Add(item);
                            }
                            //Destroy it
                            ds.Dispose();
                        }
                        else
                        {
                            if (word.Contains("'"))
                            {
                                word = word.Replace("'", "''");
                            }
                            query = "SELECT pkID, Machine, [" + fieldName.ToLower() + "],WorkSpaceName FROM METAVIEW_" +
                                    className.ToLower() + "_Listing WHERE [" + fieldName.ToLower() + "] LIKE '%" +
                                    word.ToLower() + "%' AND VCStatusID <> 4 AND VCStatusID <> 8 AND VCStatusID <> 3";
                            ds = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ExecuteDataSet(CommandType.Text, query);

                            IntellisenseMatch item;
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                item = new IntellisenseMatch();
                                item.PKid = (int)row["pkID"];
                                item.MachineID = row["Machine"].ToString();
                                item.StringValue = row[fieldName].ToString();
                                item.WorkSpaceName = row["WorkSpaceName"].ToString();
                                item._Class = className;
                                itemlist.Add(item);
                            }
                            //Destroy it
                            ds.Dispose();
                        }
                    }
                }
                //Check list
                if (itemlist.Count > 0)
                {
                    listBoxSuggestion.DataSource = itemlist;
                    listBoxSuggestion.DisplayMember = "DisplayValue";
                }
                else
                {
                    IntellisenseMatch myNullMatch = new IntellisenseMatch();
                    myNullMatch.StringValue = "No Values";
                    itemlist.Add(myNullMatch);

                    listBoxSuggestion.DataSource = itemlist;
                    listBoxSuggestion.DisplayMember = "DisplayValue";
                }
            }

            #endregion

            // Display the member listview if there are
            // items in it
            if (listBoxSuggestion.Items.Count > 0)
            {
                Point point = MyTextBox.GetPositionFromCharIndex(MyTextBox.SelectionStart);
                point.Y += MyTextBox.Bottom - 5; //(int)Math.Ceiling(MyTextBox.Font.GetHeight()) + 2;
                point.X += MyTextBox.Left; //4; // for Courier, may need a better method

                //listBoxSuggestion.Tag = MyTextBox;
                listBoxSuggestion.Location = point;
                listBoxSuggestion.BringToFront();
                listBoxSuggestion.Show();
            }
            else
            {
                listBoxSuggestion.Hide();
            }
        }

        #endregion

        internal void AddWord(string word)
        {
            spelling.Dictionary.Add(word);
        }

        internal void RemoveWord(string word)
        {
            spelling.Dictionary.Remove(word);
        }
    }
}