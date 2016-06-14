using System;
using System.Drawing;
using System.Windows.Forms;
using MetaBuilder.Core;
using MetaBuilder.Meta;
using Northwoods.Go;
using b = MetaBuilder.BusinessLogic;
using System.Collections.Generic;

namespace MetaBuilder.Graphing.Shapes.Binding.Intellisense
{
    [Serializable]
    public class CustomTextNode : GoTextNode
    {
        #region Constructors (1)

        #endregion Constructors

        #region Methods (1)

        // Protected Methods (1) 

        protected override GoText CreateLabel()
        {
            GoText l = new CustomText();
            l.Editable = true;
            l.Selectable = true;
            return l;
        }

        #endregion Methods
    }

    [Serializable]
    public class CustomText : GoText
    {
        #region Methods (1)

        // Public Methods (1) 

        public override GoControl CreateEditor(GoView view)
        {
            GoControl editor = new GoControl();

            IntellisenseBox box = new IntellisenseBox();

            box.GoView = view;
            GraphNode selObj = view.Selection.First as GraphNode;
            box.PassedMetaObject = selObj.MetaObject;

            //if (view != null)
            //    MessageBox.Show(this,"The view was passed to here");

            editor.ControlType = typeof(IntellisenseBox);
            editor.Editable = true;
            RectangleF r = Bounds;
            // new Rectangle(new Point((int)editor.Location.X, (int)editor.Location.Y), new Size(500, 500));
            r.Inflate(10, 10); // a bit bigger than the original GoText object //this just moves it
            editor.Bounds = r;
            //editor.Size = new SizeF(editor.Size.Width + 100, editor.Size.Height + 140);
            return editor;
        }

        #endregion Methods
    }

    [Serializable]
    public class IntellisenseBox : TextBox, IGoControlObject
    {
        #region Fields (12)

        private readonly ListBox listBoxSuggestion;
        public bool asShallow;
        private IntellisenseMatch intellisenseMatchItem;

        private string lookupClass;
        private string lookupField;
        // CustomTextBox state
        private GoControl myGoControl;
        private GoView myGoView;
        private MetaBase passedMetaObject;

        private SpellChecker spellChecker; //Move this to avoid recursive calls!

        //Secondary disabling devices :S
        //private bool useSpellcheck = true;

        #endregion Fields

        #region Constructors (1)

        //This is where the listbox is created
        public IntellisenseBox()
        {
            if (SpellChecker.StaticChecker == null)
                SpellChecker.StaticChecker = new SpellChecker();
            spellChecker = SpellChecker.StaticChecker;
            ReadOnly = false;
            Enabled = true;
            //events for handling typing and ctrl + space
            //KeyDown += (CustomTextBox_KeyDown);
            TextChanged += (customTextBox_TextChanged);
            MouseDown += (CustomTextBox_MouseDown);
            KeyDown += (IntellisenseBox_KeyDown);
            //create the box
            listBoxSuggestion = spellChecker.InitListBox();
            listBoxSuggestion.Bounds = Bounds;

            this.ContextMenu = new ContextMenu();
            this.ContextMenu.Popup += new EventHandler(ContextMenu_Popup);
        }
        #endregion Constructors

        private void AddDefaultItemsToContextMenu()
        {
            this.ContextMenu.MenuItems.Clear();

            MenuItem wordSep = new MenuItem();
            wordSep.Text = "Undo";
            wordSep.Shortcut = Shortcut.CtrlZ;
            wordSep.Click += new EventHandler(wordItem_Click);
            this.ContextMenu.MenuItems.Add(wordSep);

            wordSep = new MenuItem();
            wordSep.Text = "Cut";
            wordSep.Shortcut = Shortcut.CtrlX;
            wordSep.Enabled = this.SelectedText.Length > 0;
            wordSep.Click += new EventHandler(wordItem_Click);
            this.ContextMenu.MenuItems.Add(wordSep);

            wordSep = new MenuItem();
            wordSep.Text = "Copy";
            wordSep.Shortcut = Shortcut.CtrlC;
            wordSep.Enabled = this.SelectedText.Length > 0;
            wordSep.Click += new EventHandler(wordItem_Click);
            this.ContextMenu.MenuItems.Add(wordSep);

            wordSep = new MenuItem();
            wordSep.Text = "Paste";
            wordSep.Shortcut = Shortcut.CtrlV;
            wordSep.Enabled = Clipboard.ContainsText();
            wordSep.Click += new EventHandler(wordItem_Click);
            this.ContextMenu.MenuItems.Add(wordSep);

            wordSep = new MenuItem();
            wordSep.Text = "Delete";
            wordSep.Enabled = this.SelectedText.Length > 0;
            wordSep.Click += new EventHandler(wordItem_Click);
            this.ContextMenu.MenuItems.Add(wordSep);

            wordSep = new MenuItem();
            wordSep.Text = "-";
            this.ContextMenu.MenuItems.Add(wordSep);

            wordSep = new MenuItem();
            wordSep.Text = "Select All";
            wordSep.Shortcut = Shortcut.CtrlA;
            wordSep.Click += new EventHandler(wordItem_Click);
            this.ContextMenu.MenuItems.Add(wordSep);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.SelectionStart = this.GetCharIndexFromPosition(e.Location);
            }
            base.OnMouseDown(e);
            this.Refresh();
            DrawSpellingLines();
        }
        private void ContextMenu_Popup(object sender, EventArgs e)
        {
            AddDefaultItemsToContextMenu();

            if (!Core.Variables.Instance.SpellCheckEnabled)
                return;
            if (this.SelectedText.Length > 0)
            {
                //use the selection to get suggestions and to replace
                int countSelectedText = 0;
                MenuItem wordSeperator = new MenuItem();
                wordSeperator.Text = "-";
                this.ContextMenu.MenuItems.Add(wordSeperator);
                foreach (string s in spellChecker.getSuggestionList(this.SelectedText))
                {
                    if (countSelectedText > 20)
                        break;
                    MenuItem wordItem = new MenuItem();
                    //wordItem.Tag = this.SelectedText;
                    wordItem.Text = s;
                    wordItem.Click += new EventHandler(wordItem_Click);
                    this.ContextMenu.MenuItems.Add(wordItem);
                    countSelectedText++;
                }
                if (countSelectedText > 0)
                {
                    MenuItem wordSeperatorCreate = new MenuItem();
                    wordSeperatorCreate.Text = "-";
                    this.ContextMenu.MenuItems.Add(wordSeperatorCreate);

                    MenuItem wordItemCreate = new MenuItem();
                    wordItemCreate.Tag = this.SelectedText;
                    wordItemCreate.Text = "Add word to dictionary";
                    wordItemCreate.Click += new EventHandler(wordItem_Click);
                    this.ContextMenu.MenuItems.Add(wordItemCreate);

                    MenuItem wordItemRemove = new MenuItem();
                    wordItemRemove.Tag = this.SelectedText;
                    wordItemRemove.Text = "Remove word from dictionary";
                    wordItemRemove.Click += new EventHandler(wordItem_Click);
                    this.ContextMenu.MenuItems.Add(wordItemRemove);
                }
                return;
            }
            //get the word near the caret position and find suggestions for it
            int caretIndex = this.SelectionStart;
            if (caretIndex == Text.Length)
                caretIndex = -1;
            if (caretIndex < 0)
                return;
            int start = caretIndex;
            int end = caretIndex;
            bool b = false;
            while (!b)
            {
                if (Text.Substring(start, 1) == " ")
                {
                    start++;
                    b = true;
                }
                else
                {
                    start--;
                }
                if (start <= 0)
                    b = true;
            }

            b = false;
            while (!b)
            {
                if (Text.Substring(end, 1) == " ")
                {
                    b = true;
                }
                else
                {
                    end++;
                }
                if (end >= Text.Length)
                    b = true;
            }
            int endd = end - start;

            if (start < 0)
            {
                start = 0;
                //what about end?
            }
            string word = Text.Substring(start, (endd > 0 ? endd : 1));
            int count = 0;
            MenuItem wordSep = new MenuItem();
            wordSep.Text = "-";
            this.ContextMenu.MenuItems.Add(wordSep);
            foreach (string s in spellChecker.getSuggestionList(word))
            {
                if (count > 20)
                    break;
                MenuItem wordItem = new MenuItem();
                wordItem.Tag = start + "|" + end;
                wordItem.Text = s;
                wordItem.Click += new EventHandler(wordItem_Click);
                this.ContextMenu.MenuItems.Add(wordItem);
                count++;
            }
            if (count > 0)
            {
                MenuItem wordSeperatorCreateZ = new MenuItem();
                wordSeperatorCreateZ.Text = "-";
                this.ContextMenu.MenuItems.Add(wordSeperatorCreateZ);

                MenuItem wordItemCreateZ = new MenuItem();
                wordItemCreateZ.Tag = word;
                wordItemCreateZ.Text = "Add word to dictionary";
                wordItemCreateZ.Click += new EventHandler(wordItem_Click);
                this.ContextMenu.MenuItems.Add(wordItemCreateZ);

                MenuItem wordItemRemoveZ = new MenuItem();
                wordItemRemoveZ.Tag = word;
                wordItemRemoveZ.Text = "Remove word from dictionary";
                wordItemRemoveZ.Click += new EventHandler(wordItem_Click);
                this.ContextMenu.MenuItems.Add(wordItemRemoveZ);
            }
        }
        private void wordItem_Click(object sender, EventArgs e)
        {
            //replace the word between the tag (start|end) indexes with the string of this item
            MenuItem wordItem = sender as MenuItem;
            if (wordItem.Tag == null || wordItem.Text == "Remove word from dictionary" || wordItem.Text == "Add word to dictionary")
            {
                switch (wordItem.Text)
                {
                    case "Undo":
                        this.Undo();
                        break;
                    case "Cut":
                        this.Cut();
                        break;
                    case "Copy":
                        this.Copy();
                        break;
                    case "Paste":
                        this.Paste();
                        break;
                    case "Delete":
                        this.Text = this.Text.Replace(this.SelectedText, "");
                        break;
                    case "Select All":
                        this.SelectAll();
                        break;
                    case "Add word to dictionary":
                        spellChecker.AddWord(wordItem.Tag.ToString());
                        break;
                    case "Remove word from dictionary":
                        spellChecker.RemoveWord(wordItem.Tag.ToString());
                        break;
                    default:
                        //this will probably mean you have selected an item which was added as a suggestion for selected text
                        this.Text = this.Text.Replace(this.SelectedText, wordItem.Text);
                        break;
                }
                return;
            }

            int start = 0;
            int end = 0;
            int c = 0;
            foreach (string i in wordItem.Tag.ToString().Split('|'))
            {
                if (c == 0)
                {
                    start = int.Parse(i);
                    c++;
                }
                else
                {
                    end = int.Parse(i);
                }
            }
            Text = Text.Remove(start, end - start);
            Text = Text.Insert(start, wordItem.Text);
        }

        System.Drawing.Graphics formGraphics;
        private void DrawSpellingLines()
        {
            if (!Core.Variables.Instance.SpellCheckEnabled || this.IsDisposed)
                return;
            int index = 0;
            int start = 0;
            int length = 0;
            string word = "";
            float fontWidth = this.Font.Size;
            formGraphics = this.CreateGraphics();
            formGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            SizeF stringWidth = formGraphics.MeasureString(this.Text, this.Font);
            float lineWidth = stringWidth.Width;
            foreach (char c in this.Text)
            {
                bool resetStart = false;

                length += 1;
                char x = c;
                if (x == ' ')
                {
                    resetStart = true;
                    length -= 1;
                }
                else if ((index + 1) == this.Text.Length)
                {
                    resetStart = true;
                    length -= 1;
                    word += x.ToString();
                }
                else
                {
                    word += x.ToString();
                }

                float calcwidth = 0f;
                if (resetStart)
                {
                    start = index - length;
                    if (start < 0)
                        start = 0;
                    if (spellChecker == null)
                        spellChecker = new SpellChecker();
                    if (spellChecker.getSuggestionList(word).Count > 0)
                    {
                        int line = 0;
                        Point sp = this.GetPositionFromCharIndex(start);
                        Point ep = this.GetPositionFromCharIndex(start + length);
                        calcwidth += formGraphics.MeasureString(word + " ", this.Font).Width;

                        if (calcwidth > this.Width)
                        {
                            line += 1;
                            calcwidth -= this.Width;
                        }

                        if (sp.IsEmpty)
                            sp = this.GetPositionFromCharIndex(start - 1);

                        if (ep.IsEmpty)
                            ep = this.GetPositionFromCharIndex((start + length) - 1);

                        if (ep.Y == sp.Y) //line should be == 0 as well
                        {
                            formGraphics.DrawLine(Pens.Red,
                                new PointF(sp.X, sp.Y + stringWidth.Height + (stringWidth.Height * line)),
                                new PointF(ep.X, ep.Y + stringWidth.Height + (stringWidth.Height * line)));
                        }
                        else
                        {
                            //word spans more than 1 line
                            //draw line from sp.X to the end of the line @ sp.Y
                            //then draw line from start of word(X-->0) to ep.X @ ep.Y

                            // + (stringWidth.Height * line)

                            //float lengthToEndOfLine = formGraphics.MeasureString(word + " ", this.Font).Width - this.Width;
                            formGraphics.DrawLine(Pens.Red,
                                new PointF(sp.X, sp.Y + stringWidth.Height),
                                new PointF(sp.X + this.Width, sp.Y + stringWidth.Height));
                            if (line > 0) //1 line span
                            {
                                formGraphics.DrawLine(Pens.Red,
                                    new PointF(0, ep.Y + stringWidth.Height),
                                    new PointF(ep.X, ep.Y + stringWidth.Height));

                                if (line > 1) //2 line span
                                {
                                    //i dont even know what word you typed here
                                }
                            }
                        }
                    }

                    start = 0;
                    length = 0;
                    word = "";
                }
                index += 1;
            }

            formGraphics.Dispose();
        }
        private void customTextBox_TextChanged(object sender, EventArgs e)
        {
            this.Refresh();
            DrawSpellingLines();
        }
        protected override void OnEnter(EventArgs e)
        {
            this.Refresh();
            DrawSpellingLines();
        }

        #region Properties (4)

        public IntellisenseMatch IntellisenseMatchItem
        {
            get { return intellisenseMatchItem; }
            set
            {
                intellisenseMatchItem = value;

                if (GoControl.EditedObject != null)
                {
                    if (GoControl.EditedObject.Parent is IMetaNode)
                    {
                        IMetaNode mynode = GoControl.EditedObject.Parent as IMetaNode;

                        //loop through immediate view
                        bool replace = false;
                        foreach (GoObject o in myGoView.Document)
                        {
                            if (!(o is IMetaNode)) continue;

                            IMetaNode imn = o as IMetaNode;
                            //if (imn.MetaObject.pkid != intellisenseMatchItem.PKid && imn.MetaObject.MachineName != intellisenseMatchItem.MachineID)
                            if (imn.MetaObject.pkid.ToString() + imn.MetaObject.MachineName != intellisenseMatchItem.PKid.ToString() + intellisenseMatchItem.MachineID)
                                continue;

                            PassedMetaObject = imn.MetaObject;
                            replace = true;
                            break;
                        }

                        if (!replace)
                            PassedMetaObject = Loader.GetByID(intellisenseMatchItem._Class, intellisenseMatchItem.PKid, intellisenseMatchItem.MachineID);

                        if (PassedMetaObject != null)
                        {
                            if (asShallow)
                            {
                                mynode.MetaObject = PassedMetaObject;
                                if (mynode is GraphNode)
                                {
                                    GraphNode tempNode = mynode as GraphNode;
                                    tempNode.Shadowed = true;
                                }
                                else if (mynode is CollapsingRecordNodeItem)
                                {
                                    CollapsingRecordNodeItem tempNode = mynode as CollapsingRecordNodeItem;
                                    tempNode.Shadowed = true;
                                }

                                mynode.HookupEvents();
                                //mynode.FireMetaObjectChanged(this, EventArgs.Empty);
                                mynode.BindToMetaObjectProperties();
                                mynode.RequiresAttention = false;
                            }
                            else
                            {
                                if (mynode is GraphNode)
                                {
                                    GraphNode tempNode = mynode as GraphNode;
                                    tempNode.Shadowed = false;
                                }

                                //PassedMetaObject.pkid = 0;
                                //PassedMetaObject.SetMachineName();
                                //mynode.MetaObject = PassedMetaObject;

                                mynode.BindingInfo = new BindingInfo();
                                mynode.BindingInfo.BindingClass = passedMetaObject.Class;
                                mynode.CreateMetaObject(this, EventArgs.Empty);
                                PassedMetaObject.CopyPropertiesToObject(mynode.MetaObject);

                                mynode.HookupEvents();
                                //mynode.FireMetaObjectChanged(this, EventArgs.Empty);
                                mynode.BindToMetaObjectProperties();
                            }
                        }
                    }
                }
            }
        }

        public MetaBase PassedMetaObject
        {
            get
            {
                //PassedMetaObject.GetMetaPropertyList(true);
                return passedMetaObject;
            }
            set
            {
                passedMetaObject = value;
                passedMetaObject.GetMetaPropertyList(true);
            }
        }

        public GoControl GoControl
        {
            get { return myGoControl; }
            set
            {
                GoControl old = myGoControl;
                if (old != value)
                {
                    myGoControl = value;
                    if (value != null)
                    {
                        GoText gotext = value.EditedObject as GoText;
                        if (gotext != null)
                        {
                            // initialize the control according to the state of the GoText object
                            Text = gotext.Text;

                            switch (gotext.Alignment)
                            {
                                // default: <- not sure why there's a case statement if its going to default!?  
                                case GoObject.TopLeft:
                                case GoObject.MiddleLeft:
                                case GoObject.BottomLeft:
                                    TextAlign = HorizontalAlignment.Left;
                                    break;
                                case GoObject.TopCenter:
                                case GoObject.MiddleCenter:
                                case GoObject.BottomCenter:
                                    TextAlign = HorizontalAlignment.Center;
                                    break;
                                case GoObject.TopRight:
                                case GoObject.MiddleRight:
                                case GoObject.BottomRight:
                                    TextAlign = HorizontalAlignment.Right;
                                    break;

                            }

                            Multiline = gotext.Multiline || gotext.Wrapping;
                            AcceptsReturn = gotext.Multiline;
                            WordWrap = gotext.Wrapping;
                            Font objfont = gotext.Font;
                            float fsize = objfont.Size;
                            if (GoView != null) fsize *= GoView.DocScale;
                            Font = new Font(objfont.Name, fsize, objfont.Style, GraphicsUnit.Point);

                            // find the first node as parent
                            GoObject currentParent = value.EditedObject;
                            IMetaNode imnode = null;
                            while (currentParent != null && imnode == null)
                            {
                                if (currentParent is IMetaNode)
                                    imnode = currentParent as IMetaNode;
                                else if (currentParent is MetaBuilder.Graphing.Shapes.Nodes.MindMapNode)
                                    break;
                                else
                                    currentParent = currentParent.Parent;
                            }
                            if (imnode != null)
                            {
                                lookupField = "Name";
                                if (value.EditedObject is BoundLabel)
                                {
                                    BoundLabel lbl = value.EditedObject as BoundLabel;
                                    // REQUIRED FOR SHAPE DESIGNING
                                    if (imnode.BindingInfo != null && imnode.MetaObject != null)
                                    {
                                        if (imnode.BindingInfo.Bindings.ContainsKey(lbl.Name))
                                            lookupField = imnode.BindingInfo.Bindings[lbl.Name];
                                        lookupClass = imnode.MetaObject._ClassName;
                                    }
                                }
                            }
                        }

                    }
                }
            }
        }

        public GoView GoView
        {
            get { return myGoView; }
            set
            {
                myGoView = value;
                //this possibly allows me to add the control to the view (once?)
                if (myGoView != null && !myGoView.Controls.Contains(listBoxSuggestion))
                    myGoView.Controls.Add(listBoxSuggestion);
            }
        }

        #endregion Properties

        #region Methods (7)

        // Protected Methods (2) 

        protected override void OnLeave(EventArgs evt)
        {
            if (!listBoxSuggestion.Focused)
            {
                AcceptText();
                base.OnLeave(evt);
            }
        }

        protected override bool ProcessDialogKey(Keys key)
        {
            if (HandleKey(key))
                return true;
            return base.ProcessDialogKey(key);
        }

        // Private Methods (7) 

        private void AcceptText()
        {
            if (listBoxSuggestion.Visible && listBoxSuggestion.Items.Count > 0 && listBoxSuggestion.SelectedItem != null)
            {
                spellChecker.replaceWord(false);
            }

            GoControl ctrl = GoControl;
            if (ctrl != null)
            {
                GoText gotext = ctrl.EditedObject as GoText;
                if (gotext != null)
                {
                    //try get the item and set it to the node's tag or something
                    //GoNode node;
                    //node = GoView.Selection.First as GoNode;
                    //Use node to set the tag to the object match

                    listBoxSuggestion.Hide();

                    //cannot access dockingform from here to call method
                    //if (gotext.ParentNode is GraphNode)
                    //{
                    //    GraphNode node = gotext.ParentNode as GraphNode;
                    //    //TODO : find all other instances of this metaobject on all other diagrams and set their metaobject to this one and update it (THIS WILL TAKE FOREVER!)
                    //    node.MetaObject.ToString();
                    //}
                    //End edit
                    gotext.DoEdit(GoView, gotext.Text, Text); //error here when close form while node is editing
                }
            }
            endSpellcheckersLife();
            if (ctrl != null)
                ctrl.DoEndEdit(GoView);
        }

        private void endSpellcheckersLife()
        {
            listBoxSuggestion.Hide();
            // listBoxSuggestion = null;
            if (spellChecker != null)
                spellChecker.DisposeListBox(listBoxSuggestion);
            listBoxSuggestion.Dispose();
            spellChecker = null;
            //spellChecker.Dispose();
        }

        private void IntellisenseBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (Core.Variables.Instance.CustomSuggestionEnabled || Variables.Instance.IntellisenseEnabled)//Variables.Instance.SpellCheckEnabled
            {
                spellChecker.MyTextBox = this;
                spellChecker.MyGoView = GoView;

                if (e.KeyCode == Keys.Escape)
                {
                    //this doesnt work
                    if (!listBoxSuggestion.Visible)
                    {
                        GoControl ctrl = GoControl;
                        if (ctrl != null)
                            ctrl.DoEndEdit(GoView);
                        GoView.RequestFocus();
                    }
                    else
                        listBoxSuggestion.Hide();
                }
                else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
                {
                    if (e.KeyCode == Keys.Up)
                    {
                        if (listBoxSuggestion.SelectedIndex == 0)
                        {
                            this.Focus();
                            this.Select();
                        }
                        else
                        {
                            SelectBox();
                            e.Handled = true;
                        }
                    }
                    else
                    {
                        SelectBox();
                        e.Handled = true;
                    }
                }
                else if (e.KeyCode == Keys.F7)
                {
                    if (Variables.Instance.SpellCheckEnabled)
                        spellChecker.manualCheck();
                }
                else if (!(myGoControl.ParentNode is Graphing.Shapes.Nodes.MindMapNode) && e.Control && e.KeyCode == Keys.Space)
                {
                    spellChecker.UseIntellisense = true;
                    spellChecker.intellisenseSuggestion(lookupClass, lookupField);
                }
                else
                {
                    bool skip = false;
                    if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
                    {
                        if (listBoxSuggestion.Visible && listBoxSuggestion.Items.Count > 0 && listBoxSuggestion.SelectedItem != null)
                        {
                            spellChecker.replaceWord(false);
                            skip = true;
                        }
                    }
                    if (!skip)
                    {
                        if (Core.Variables.Instance.CustomSuggestionEnabled)//useSpellcheck) //useSuggestions
                        {
                            spellChecker.UseIntellisense = false;
                            spellChecker.MyTextBox = this;

                            //spellChecker.getSuggestions(); //No parameters
                            spellChecker.getCustomSuggestion(Text); //No parameters
                        }
                    }
                }
            }

            bool listBoxVisibleWithItems = listBoxSuggestion.Visible && listBoxSuggestion.Items.Count > 0;
            if (listBoxVisibleWithItems)
            {
                int maxSelectedIndex = listBoxSuggestion.Items.Count - 1;

                switch (e.KeyCode)
                {
                    case Keys.Up:
                        if (listBoxSuggestion.SelectedIndex == -1)
                            listBoxSuggestion.SelectedIndex = 0;
                        else
                        {
                            if (listBoxSuggestion.SelectedIndex > 0)
                                listBoxSuggestion.SelectedIndex--;
                        }
                        SelectBox();
                        e.Handled = true;
                        break;
                    case Keys.Down:
                        if (listBoxSuggestion.SelectedIndex == -1)
                            listBoxSuggestion.SelectedIndex = 0;
                        else
                        {
                            if (listBoxSuggestion.SelectedIndex < maxSelectedIndex)
                                listBoxSuggestion.SelectedIndex++;
                        }
                        SelectBox();
                        e.Handled = true;
                        break;
                }
            }
            else
            {
                if (spellChecker != null)
                    spellChecker.ListFocused = false;
            }
        }

        private void SelectBox()
        {
            spellChecker.ListFocused = true;

            listBoxSuggestion.Focus();
            listBoxSuggestion.Select();
        }

        //private void CustomTextBox_KeyDown(object sender, KeyEventArgs e)
        //{
        //    //if (MetaBuilder.Core.Variables.Instance.SpellCheckEnabled)
        //    //{
        //    //    spellChecker.MyTextBox = this;
        //    //    spellChecker.MyGoView = GoView;

        //    //    if (e.KeyCode == Keys.Escape)
        //    //    {
        //    //        //this doesnt work
        //    //        if (!listBoxSuggestion.Visible)
        //    //        {
        //    //            GoControl ctrl = GoControl;
        //    //            if (ctrl != null)
        //    //                ctrl.DoEndEdit(GoView);
        //    //            GoView.RequestFocus();
        //    //        }
        //    //        else
        //    //            listBoxSuggestion.Hide();
        //    //    }
        //    //    else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
        //    //        listBoxSuggestion.Focus();
        //    //    else if (e.KeyCode == Keys.F7)
        //    //    {
        //    //        spellChecker.manualCheck();
        //    //    }
        //    //    else if (e.Control && e.KeyCode == Keys.Space)
        //    //    //if (useIntellisense)
        //    //    {
        //    //        spellChecker.UseIntellisense = true;
        //    //        spellChecker.intellisenseSuggestion(lookupClass, lookupField);
        //    //        //need to pass classname and fieldname here!
        //    //    }
        //    //    //else if (e.KeyCode == Keys.Up)
        //    //    //{
        //    //    //    listBoxSuggestion.Focus();
        //    //    //}
        //    //    //else if (e.KeyCode == Keys.Down)
        //    //    //{
        //    //    //    listBoxSuggestion.Focus();
        //    //    //}
        //    //    else
        //    //    {
        //    //        if (useSpellcheck)
        //    //        {
        //    //            spellChecker.UseIntellisense = false;
        //    //            spellChecker.MyTextBox = this;
        //    //            spellChecker.getSuggestions("", ""); //No parameters
        //    //        }
        //    //    }
        //    //}
        //}

        private void CustomTextBox_MouseDown(object sender, MouseEventArgs e)
        {
            listBoxSuggestion.Hide();
        }

        private bool HandleKey(Keys key)
        {
            //if we return true then that key doesnt fire in the textboxes events
            //so just return true will disable any text getting typed

            if (key == Keys.Escape)
            {
                GoControl ctrl = GoControl;
                if (ctrl != null)
                    ctrl.DoEndEdit(GoView);
                GoView.RequestFocus();
                listBoxSuggestion.Hide();
                return true;
            }
            if (key == Keys.Enter || key == Keys.Tab)
            {
                //if (key == System.Windows.Forms.Keys.Enter && this.AcceptsReturn)  // accept the return to start a new line
                //    return false;
                if (key == Keys.Enter && Multiline == true && AcceptsReturn)
                {
                    //AppendText(Environment.NewLine);
                    return false;
                }

                AcceptText();
                GoView.RequestFocus();
                return true;
            }
            /*else if (key == Keys.Up ) //Wont work
        {
            if (listBoxSuggestion.SelectedIndex == -1)
                listBoxSuggestion.SelectedIndex = 0;
            else
                listBoxSuggestion.SelectedIndex--;

            listBoxSuggestion.Focus();

            return true;
        }
        else if (key == Keys.Down) //Wont work
        {
            if (listBoxSuggestion.SelectedIndex == -1)
                listBoxSuggestion.SelectedIndex = 0;
            else
                listBoxSuggestion.SelectedIndex++;

            listBoxSuggestion.Focus();
            return true;
        }*/

            //handle all other keys
            return false;
        }

        #endregion Methods

        public void AcceptTextPublic()
        {
            AcceptText();
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        lookupClass = null;
        //        lookupField = null;
        //        listBoxSuggestion = null;
        //        myGoControl = null;
        //        myGoView = null;
        //        spellChecker = null;

        //    }
        //    base.Dispose(disposing);
        //}
    }

    //No IntellisenseMatch
    public class RichIntellisenseBox : RichTextBox, IGoControlObject
    {
        #region Fields (4)

        private readonly ListBox listBoxSuggestion;
        // CustomTextBox state
        private GoControl myGoControl;
        private GoView myGoView;

        private SpellChecker spellChecker = new SpellChecker(); //Move this to avoid recursive calls!

        //Secondary disabling devices :S
        //private bool useIntellisense = true;
        private bool useSpellcheck = true;

        #endregion Fields

        #region Constructors (1)

        //This is where the listbox is created
        public RichIntellisenseBox()
        {
            ReadOnly = false;
            Enabled = true;

            AllowDrop = false;
            AutoSize = false;
            ScrollBars = RichTextBoxScrollBars.ForcedVertical;

            //events for handling typing and ctrl + space
            KeyDown += (CustomTextBox_KeyDown);
            TextChanged += (customTextBox_TextChanged);
            MouseDown += (CustomTextBox_MouseDown);
            KeyUp += RichIntellisenseBox_KeyUp;
            //create the box
            listBoxSuggestion = spellChecker.InitListBox();
            listBoxSuggestion.Bounds = Bounds;
        }

        private void RichIntellisenseBox_KeyUp(object sender, KeyEventArgs e)
        {
            bool listBoxVisibleWithItems = listBoxSuggestion.Visible && listBoxSuggestion.Items.Count > 0;
            if (listBoxVisibleWithItems)
            {
                int maxSelectedIndex = listBoxSuggestion.Items.Count - 1;

                switch (e.KeyCode)
                {
                    case Keys.Up:
                        if (listBoxSuggestion.SelectedIndex == -1)
                            listBoxSuggestion.SelectedIndex = 0;
                        else
                        {
                            if (listBoxSuggestion.SelectedIndex > 0)
                                listBoxSuggestion.SelectedIndex--;
                        }

                        listBoxSuggestion.Focus();
                        break;
                    case Keys.Down:
                        if (listBoxSuggestion.SelectedIndex == -1)
                            listBoxSuggestion.SelectedIndex = 0;
                        else
                        {
                            if (listBoxSuggestion.SelectedIndex < maxSelectedIndex)
                                listBoxSuggestion.SelectedIndex++;
                        }

                        listBoxSuggestion.Focus();
                        break;
                }
            }
        }

        #endregion Constructors

        #region Properties (2)

        public GoControl GoControl
        {
            get { return myGoControl; }
            set
            {
                GoControl old = myGoControl;
                if (old != value)
                {
                    myGoControl = value;
                    if (value != null)
                    {
                        RichTextLabel gorichtext = value.EditedObject as RichTextLabel;
                        if (gorichtext != null)
                        {
                            Rtf = gorichtext.Rtf;
                            BackColor = gorichtext.BackgroundColor;
                        }
                    }
                }
            }
        }

        public GoView GoView
        {
            get { return myGoView; }
            set
            {
                myGoView = value;
                ZoomFactor = myGoView.DocScale;
                //this possibly allows me to add the control to the view (once?)
                if (myGoView != null && !myGoView.Controls.Contains(listBoxSuggestion))
                    myGoView.Controls.Add(listBoxSuggestion);
            }
        }

        #endregion Properties

        #region Methods (7)

        // Protected Methods (2) 

        protected override void OnLeave(EventArgs evt)
        {
            if (!listBoxSuggestion.Focused)
            {
                AcceptText();
                base.OnLeave(evt);
            }
        }

        protected override bool ProcessDialogKey(Keys key)
        {
            if (HandleKey(key))
                return true;
            return base.ProcessDialogKey(key);
        }

        // Private Methods (5) 

        private void AcceptText()
        {
            GoControl ctrl = GoControl;
            if (ctrl != null)
            {
                //GoText gotext = ctrl.EditedObject as GoText;
                RichTextLabel gotext = ctrl.EditedObject as RichTextLabel;
                if (gotext != null)
                {
                    try
                    {
                        //try get the item and set it to the node's tag or something
                        //Use node to set the tag to the object match

                        listBoxSuggestion.Hide();
                        //gotext.DoEdit(GoView, gotext.Text, Text); //error here when close form while node is editing
                        gotext.Rtf = Rtf;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.ToString());
                    }
                }
                endSpellcheckersLife();
                ctrl.DoEndEdit(GoView);
            }
        }

        private void endSpellcheckersLife()
        {
            // listBoxSuggestion = null;
            listBoxSuggestion.Dispose();
            spellChecker = null;
            //spellChecker.Dispose();
        }

        private void CustomTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            spellChecker.MyTextBox = this;
            if (e.KeyCode == Keys.Space && listBoxSuggestion.Items.Count > 0 && listBoxSuggestion.SelectedItem != null)
            {
                spellChecker.replaceWord(false);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                //this doesnt work
                if (!listBoxSuggestion.Visible)
                {
                    GoControl ctrl = GoControl;
                    if (ctrl != null)
                        ctrl.DoEndEdit(GoView);
                    GoView.RequestFocus();
                }
                else
                    listBoxSuggestion.Hide();
            }
            else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
                SelectBox();
            else if (e.KeyCode == Keys.F7)
            {
                spellChecker.manualCheck();

                //NO INTELLISENSE IN RICH TEXTBOXES!
                //else if (e.Control && e.KeyCode == Keys.Space)
                ////if (useIntellisense)
                //{
                //    spellChecker.UseIntellisense = true;
                //    spellChecker.intellisenseSuggestion(lookupClass, lookupField);
                //    //need to pass classname and fieldname here!
                //}
            }
            else if (e.KeyCode == Keys.Up)
            {
                SelectBox();
            }
            else if (e.KeyCode == Keys.Down)
            {
                SelectBox();
            }
            else
            {
                if (useSpellcheck)
                {
                    spellChecker.UseIntellisense = false;
                    spellChecker.MyTextBox = this;
                    spellChecker.getSuggestions(); //No parameters
                }
            }
        }

        private void SelectBox()
        {
            listBoxSuggestion.Focus();
            listBoxSuggestion.Select();
        }

        private void CustomTextBox_MouseDown(object sender, MouseEventArgs e)
        {
            listBoxSuggestion.Hide();
        }

        private void customTextBox_TextChanged(object sender, EventArgs e)
        {
            //this is no longer required
        }

        private bool HandleKey(Keys key)
        {
            //if we return true then that key doesnt fire in the textboxes events
            //so just return true will disable any text getting typed

            if (key == Keys.Escape)
            {
                GoControl ctrl = GoControl;
                if (ctrl != null)
                    ctrl.DoEndEdit(GoView);
                GoView.RequestFocus();
                listBoxSuggestion.Hide();
                return true;
            }
            if (key == Keys.Enter || key == Keys.Tab)
            {
                //if (key == System.Windows.Forms.Keys.Enter && this.AcceptsReturn)  // accept the return to start a new line
                //    return false;
                AcceptText();
                GoView.RequestFocus();
                return true;
            }
            /*else if (key == Keys.Up ) //Wont work
        {
            if (listBoxSuggestion.SelectedIndex == -1)
                listBoxSuggestion.SelectedIndex = 0;
            else
                listBoxSuggestion.SelectedIndex--;

            listBoxSuggestion.Focus();

            return true;
        }
        else if (key == Keys.Down) //Wont work
        {
            if (listBoxSuggestion.SelectedIndex == -1)
                listBoxSuggestion.SelectedIndex = 0;
            else
                listBoxSuggestion.SelectedIndex++;

            listBoxSuggestion.Focus();
            return true;
        }*/
            if (key == Keys.ControlKey)
            {
                return true;
            }
            return false;
        }

        #endregion Methods

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        lookupClass = null;
        //        lookupField = null;
        //        listBoxSuggestion = null;
        //        myGoControl = null;
        //        myGoView = null;
        //        spellChecker = null;

        //    }
        //    base.Dispose(disposing);
        //}
    }
}