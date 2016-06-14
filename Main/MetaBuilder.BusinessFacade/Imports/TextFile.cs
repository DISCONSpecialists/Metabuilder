using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.Core;
using MetaBuilder.Meta;
using Microsoft.Office.Interop.Excel;

namespace MetaBuilder.BusinessFacade.Imports
{
    /// <summary>
    /// Summary description for TextFile.
    /// </summary>
    /// 
    public class TextImportSpecification
    {
        private string sourceFile;

        public string SourceFile
        {
            get { return sourceFile; }
            set { sourceFile = value; }
        }

        private string defaultField;
        public string DefaultField
        {
            get { return defaultField; }
            set { defaultField = value; }
        }

        private string className;
        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }
    }

    public class TextFile
    {
        public bool errorOccurred = false;
        public string error = "";
        private TextBox myVar;
        public TextBox MyProperty
        {
            get { return myVar; }
            set { myVar = value; }
        }

        public TextFile()
        {
        }

        private TextImportSpecification specification;
        public TextImportSpecification Specification
        {
            get { return specification; }
            set { specification = value; }
        }

        private TextReader reader;
        public void Import(TextImportSpecification[] specifications, bool IndicateErrors)
        {
            int FileCount = specifications.Length;
            for (int i = 0; i < FileCount; i++)
            {
                ImportFile(specifications[i], IndicateErrors);
            }
        }

        //BusinessLogic.TList<BusinessLogic.AssociationType> AllAssociationTypes;
        private void ImportFile(TextImportSpecification spec, bool IndicateErrors)
        {
            Specification = spec;
            Import();
        }
        private bool validInput;

        public bool ValidInput
        {
            get { return validInput; }
            set { validInput = value; }
        }

        bool foundLinesEndingWithTabs = false;
        private bool Validate()
        {
            OnReport("Starting", EventArgs.Empty);
            bool validated = true;
            reader = new StreamReader(Specification.SourceFile);
            RegexOptions options = RegexOptions.Singleline;
            string regex = "^.*?\t+$";
            //use this regex to find and replace in text file {[ \f\t\v]+$}
            bool b = false;
            while (reader.Peek() != -1)
            {
                if (b)
                    break;
                string line = reader.ReadLine();
                MatchCollection matches = Regex.Matches(line, regex, options);//[ \f\t\v]+$
                foreach (Match match in matches)
                {
                    b = true;
                    foundLinesEndingWithTabs = true;
                    break;
                }
            }
            reader.Close();

            if (foundLinesEndingWithTabs)
            {
                Log.WriteLog("Found Lines Ending With Tabs - they will be removed automatically using the regex expression ' [ \f\t\v]+$ ' in memory");
                //error = "Found Lines Ending With Tabs";
                //errorOccurred = true;
                //validated = false;
            }
            OnReport("Validation " + (errorOccurred ? "failed" : "passed"), EventArgs.Empty);
            validInput = validated;
            return validated;
        }
        public void Import()
        {
            //if (AllAssociationTypes == null)
            //    AllAssociationTypes = DataAccessLayer.DataRepository.AssociationTypeProvider.GetAll();

            if (Validate())
            {
                try
                {
                    if (Specification != null)
                    {
                        AssociationHelper assHelper = new AssociationHelper();
                        items = new List<Item>();
                        // done with the excel file, now import the text file (Modules)
                        reader = new StreamReader(Specification.SourceFile);

                        OnReport("Consuming Lines", EventArgs.Empty);

                        while (reader.Peek() != -1)
                        {
                            string line = reader.ReadLine();

                            #region Automatic removal of line ending tabs/whitespace

                            //OnReport("Removing line ending whitespace", EventArgs.Empty);
                            //#if DEBUG
                            //Console.WriteLine(line.Replace("\t", "-TAB."));
                            if (foundLinesEndingWithTabs)
                            {
                                MatchCollection matches = Regex.Matches(line, "[ \f\t\v]+$");
                                foreach (Match m in matches)
                                {
                                    line = line.Substring(0, line.LastIndexOf(m.Value));
                                }
                            }
                            //Console.WriteLine(line.Replace("\t", "-TAB."));
                            //#endif
                            #endregion

                            #region Automatic removal of numbering

                            #endregion

                            ConsumeTextLine(line);
                        }
                        reader.Close();

                        OnReport("Saving " + items.Count + " items", EventArgs.Empty);
                        int itemnumber = 0;
                        foreach (Item line in items)
                        {
                            itemnumber++;
                            if ((line.Children.Count > 0) && (line.Parent == null))
                            {
                                //#if !DEBUG
                                SaveItem(line);
                                if (itemnumber % 500 == 0)
                                {
                                    OnReport("Saving item " + itemnumber + "/" + items.Count + " items", EventArgs.Empty);
                                }
                                //#endif
                            }
                        }
                        OnReport("Complete", EventArgs.Empty);
                    }
                }
                catch (Exception x)
                {
                    errorOccurred = true;
                    Core.Log.WriteLog(x.ToString(), "TextFileImport", System.Diagnostics.TraceEventType.Warning);
                }
            }
            OnFinished(this, EventArgs.Empty);
        }

        public event EventHandler Finished;
        protected void OnFinished(object sender, EventArgs e)
        {
            if (Finished != null)
                Finished(sender, e);
        }
        public event EventHandler Report;
        protected void OnReport(object sender, EventArgs e)
        {
            if (Report != null)
                Report(sender, e);
        }
        private MetaBase firstObject;
        public MetaBase FirstObject
        {
            get
            {
                return firstObject;
            }
            set
            {
                firstObject = value;
            }
        }
        void SaveItem(Item line)
        {
            MetaBase MetaObject;

            //line.PrepareWord();
            if (line.LinkTypeName == "Rationale")
            {
                MetaObject = Loader.CreateInstance("Rationale");// (MetaBase)Activator.CreateInstanceFrom(Variables.Instance.MetaAssembly, "MetaBuilder.Meta.Generated.Rationale").Unwrap();
                MetaObject.Set("Value", line.WholeWord);
            }
            else
            {
                MetaObject = Loader.CreateInstance(Specification.ClassName);// (MetaBase)Activator.CreateInstanceFrom(Variables.Instance.MetaAssembly, "MetaBuilder.Meta.Generated." + Specification.ClassName).Unwrap();
                MetaObject.Set(Specification.DefaultField, line.WholeWord);
            }

            MetaObject._ClassName = Specification.ClassName;
            MetaObject.Save(Guid.NewGuid());
            if (FirstObject == null)
                FirstObject = MetaObject;
            line.MBase = MetaObject;

            foreach (Item child in line.Children)
            {
                SaveItem(child);
                try
                {
                    if (child.LinkType.HasValue)
                    {
                        if (AssociationManager.Instance.GetAssociationsForParentAndChildClasses(line.MBase._ClassName, child.MBase._ClassName).Count > 0)
                        {
                            try
                            {
                                Singletons.GetAssociationHelper().AddQuickAssociation(line.MBase.pkid, child.MBase.pkid, line.MBase.MachineName, child.MBase.MachineName, child.LinkType.Value);
                            }
                            catch
                            {
                                errorOccurred = true;
                                Core.Log.WriteLog("Text import cannot add an association with type (" + child.LinkType.ToString() + ") for parent " + line.MBase.ToString() + " and child " + child.MBase.ToString() + " because it does not exist int the metamodel", "TextFileImport", System.Diagnostics.TraceEventType.Warning);
                            }
                        }
                    }
                    else
                    {
                        if (AssociationManager.Instance.GetAssociationsForParentAndChildClasses(line.MBase._ClassName, child.MBase._ClassName).Count > 0)
                        {
                            //default association
                            bool d = false;
                            foreach (BusinessLogic.ClassAssociation ass in AssociationManager.Instance.GetAssociationsForParentAndChildClasses(line.MBase._ClassName, child.MBase._ClassName))
                            {
                                if (ass.IsDefault == true)
                                {
                                    Singletons.GetAssociationHelper().AddQuickAssociation(line.MBase.pkid, child.MBase.pkid, line.MBase.MachineName, child.MBase.MachineName, ass.AssociationTypeID);
                                    d = true;
                                    break;
                                }
                            }
                            if (!d)
                                Singletons.GetAssociationHelper().AddQuickAssociation(line.MBase.pkid, child.MBase.pkid, line.MBase.MachineName, child.MBase.MachineName, AssociationManager.Instance.GetAssociationsForParentAndChildClasses(line.MBase._ClassName, child.MBase._ClassName)[0].AssociationTypeID);
                        }
                    }
                }
                catch (Exception ex)
                {
                    errorOccurred = true;
                    Core.Log.WriteLog("Trying to add an association of type " + line.LinkTypeName + " (" + line.LinkType != null ? ((int)line.LinkType).ToString() : "NaN" + ") between " + line.WholeWord + " (Line: " + line.LineNumber + ") and " + child.WholeWord + " (Line: " + child.LineNumber + ") generated a failure" + Environment.NewLine + ex.ToString(), "TextFileImport", System.Diagnostics.TraceEventType.Warning);
                }
            }
        }

        List<Item> items;
        private void ConsumeTextLine(string text)
        {
            if (text.Trim().Length > 0)
            {
                Item line = new Item(items.Count, text);
                items.Add(line);
                Item parent = GetParentItem(line);
                if (parent != null)
                {
                    parent.AddChild(line);
                }
            }
        }
        private Item GetParentItem(Item child)
        {
            Item testParentSDLine;

            for (int x = child.LineNumber; x >= 0; x--)
            {
                testParentSDLine = items[x];
                if (testParentSDLine.NumberOfTabs == child.NumberOfTabs - 1)
                {
                    return testParentSDLine;
                }
            }
            return null;
        }

        private class Item
        {
            private string originalText;

            public string OriginalText
            {
                get { return originalText; }
                set { originalText = value; }
            }

            private MetaBase mbase;
            public MetaBase MBase
            {
                get { return mbase; }
                set { mbase = value; }
            }

            private int numberOfTabs;
            public int NumberOfTabs
            {
                get { return numberOfTabs; }
                set { numberOfTabs = value; }
            }

            private string wholeWord;
            public string WholeWord
            {
                get
                {
                    PrepareWord();
                    return wholeWord;
                }
                set { wholeWord = value; }
            }

            private int? linkType;
            public int? LinkType
            {
                get { return linkType; }
                set { linkType = value; }
            }

            private List<Item> children;
            public List<Item> Children
            {
                get { return children; }
                set { children = value; }
            }
            private Item parent;
            public Item Parent
            {
                get { return parent; }
                set
                {
                    parent = value;
                }
            }
            private string linkTypeName;
            public string LinkTypeName
            {
                get { return linkTypeName; }
                set { linkTypeName = value; }
            }

            private int lineNumber;
            public int LineNumber
            {
                get { return lineNumber; }
                set { lineNumber = value; }
            }

            public void PrepareWord()
            {
                char splitChar = '\t';
                string[] splitText = originalText.Split(splitChar);
                this.WholeWord = GetWholeWord(splitText[splitText.Length - 1]);
            }

            public Item(int LineNo, string text)
            {
                char splitChar = '\t';
                OriginalText = text.TrimEnd(new char[] { splitChar });
                children = new List<Item>();

                string[] splitText = text.Split(splitChar);
                NumberOfTabs = splitText.Length - 1;
                LineNumber = LineNo;
            }

            private string GetWholeWord(string text)
            {
                if (this.Parent != null)
                {
                    string retval = "";
                    this.LinkType = null;

                    int result;
                    bool StartsWithNumber = int.TryParse(text.Substring(0, 1), out result);
                    bool HasPoint = text.IndexOf(". ") > -1;
                    //remove number
                    if (StartsWithNumber && HasPoint)
                    {
                        text = text.Substring(text.LastIndexOf(". ") + 2, text.Length - text.LastIndexOf(". ") - 2);
                    }
                    //get association and return text
                    if (text.StartsWith("["))
                    {
                        string rawAss = text.Substring(text.IndexOf("["), text.LastIndexOf("]") + 1);
                        string ass = rawAss.Replace("[", "").Replace("]", "").Trim().ToUpper();

                        if (ass.Length == 1)
                        {
                            switch (ass)
                            {
                                case "A":
                                    ass = "auxiliary";
                                    break;
                                case "C":
                                    ass = "classification";
                                    break;
                                case "D":
                                    ass = "decomposition";
                                    break;
                                case "M":
                                    ass = "mapping";
                                    break;
                                case "R":
                                    ass = "rationale";
                                    break;
                            }
                        }
                        else if (ass.Length == 3)
                        {
                            foreach (string t in Enum.GetNames(typeof(LinkAssociationType)))
                            {
                                if (t.ToUpper().StartsWith(ass))
                                {
                                    ass = t;
                                    break;
                                }
                            }
                        }

                        try
                        {
                            LinkAssociationType association = (LinkAssociationType)Enum.Parse(typeof(LinkAssociationType), ass, true);
                            LinkType = (int)association;
                            LinkTypeName = association.ToString();
                        }
                        catch
                        {
                            LinkTypeName = ass;
                            Core.Log.WriteLog("Text import cannot find association with type (" + ass + ") within the line " + OriginalText, "TextFileImportItem-GetwholeWord", System.Diagnostics.TraceEventType.Warning);
                        }
                        retval = text.Replace(rawAss, "").Trim();
                        return retval;

                        #region OLD
                        //if (text.Length > 4)
                        //{
                        //    string AllLinkIndicator = text.Substring(0, 6).Replace("[", "").Replace("]", "").Trim().ToLower(); //text
                        //    if (AllAssociationTypes == null) //This should be set so we dont call the db foreach line
                        //        AllAssociationTypes = DataAccessLayer.DataRepository.AssociationTypeProvider.GetAll();
                        //    //find the association whose name's first two letters = word in [XX]
                        //    foreach (BusinessLogic.AssociationType aType in AllAssociationTypes)
                        //    {
                        //        if (aType.Name.ToLower() == AllLinkIndicator)
                        //        {
                        //            LinkType = aType.pkid;
                        //            LinkTypeName = aType.Name.Trim();
                        //            break;
                        //        }
                        //    }
                        //    retval = text.Replace(text, "");
                        //}
                        //else
                        //{
                        //    string LinkTypeIndicator = text.Substring(0, 4);
                        //    switch (LinkTypeIndicator.ToUpper())
                        //    {
                        //        case "[A] ":
                        //            LinkType = 1;
                        //            LinkTypeName = "Auxiliary";
                        //            break;
                        //        case "[C] ":
                        //            LinkTypeName = "Classification";
                        //            LinkType = 2;
                        //            break;
                        //        case "[D] ":
                        //            LinkTypeName = "Decomposition";
                        //            LinkType = 3;
                        //            break;
                        //        case "[M] ":
                        //            LinkTypeName = "Mapping";
                        //            LinkType = 4;
                        //            break;
                        //        case "[R] ":
                        //            LinkTypeName = "Rationale";
                        //            LinkType = 4;
                        //            break;
                        //        default:
                        //            LinkType = null;
                        //            LinkTypeName = "";
                        //            break;
                        //    }
                        //    retval = text.Replace(LinkTypeIndicator, "");
                        //}
                        #endregion

                    }
                    else
                    {
                        retval = text;
                    }
                    return retval;
                }

                // this is a parent
                LinkType = null;
                return text;
            }
            public void AddChild(Item line)
            {
                children.Add(line);
                line.Parent = this;
            }
        }
    }
}