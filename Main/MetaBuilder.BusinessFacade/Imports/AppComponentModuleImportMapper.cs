using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.Core;
using MetaBuilder.Meta;
using Microsoft.Office.Interop.Excel;

namespace MetaBuilder.BusinessFacade.Imports
{
    public class AppComponentModuleImportMapper
    {
        private Dictionary<string, List<MetaBase>> keyedObjects;
        private TextReader reader;
        private Exports.ExcelUtil util;
        private string textFile;
        private SDLine currentTextLine;
        private int currentExcelRow;
        public string TextFile
        {
            get { return textFile; }
            set { textFile = value; }
        }

        private string excelFile;

        public string ExcelFile
        {
            get { return excelFile; }
            set { excelFile = value; }
        }
        
        public StringBuilder sReport;
        public void Import()
        {
            try
            {
                sReport = new StringBuilder();
                if (ExcelFile != null && textFile != null)
                {

                    keyedObjects = new Dictionary<string, List<MetaBase>>();
                    sdlines = new List<SDLine>();

                    util.OpenExcel();

                    util.OpenFile(excelFile);
                    List<string> cols = new List<string>();

                    string[] alphabet = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", 
                                                    "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai","aj","ak","al","am","an","ao","ap","aq","ar","as","at","au","av","aw","ax","ay","az",
                                                    "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj","bk","bl","bm","bn","bo","bp","bq","br","bs","bt","bu","bv","bw","bx","by","bz"};
                    int alphabetCounter = 0;
                    int rowCounter = 3;
                    string txt = GetText(alphabet[alphabetCounter], rowCounter);
                    while (txt.Length > 0)
                    {
                        cols.Add(txt);
                        alphabetCounter++;
                        txt = GetText(alphabet[alphabetCounter], rowCounter);
                    }

                    int NumberOfColumns = alphabetCounter;
                    string[] values = new string[NumberOfColumns];
                    rowCounter++;

                    try
                    {
                        while (GetText("A", rowCounter).Length > 0)
                        {
                            currentExcelRow = rowCounter;
                            MetaBase MetaObject = (MetaBase)Activator.CreateInstanceFrom(Variables.Instance.MetaAssembly, "MetaBuilder.Meta.Generated.Software").Unwrap();
                            //MetaBuilder.Meta.Loader.CreateInstance("Software");
                            MetaObject.Set("Type", "App_Component");

                            SetProperty(cols, alphabet, rowCounter, MetaObject, "Name", GetText("A", rowCounter));
                            SetProperty(cols, alphabet, rowCounter, MetaObject, "ID", GetText("C", rowCounter));
                            SetProperty(cols, alphabet, rowCounter, MetaObject, "Type", "App_Component");
                            string key = GetText("B", rowCounter);
                            if (!keyedObjects.ContainsKey(key))
                            {
                                keyedObjects.Add(key, new List<MetaBase>());
                            }
                            MetaObject.Save(Guid.NewGuid());
                            keyedObjects[key].Add(MetaObject);

                            rowCounter++;
                        }

                    }
                    catch (Exception x)
                    {
                        sReport.Append("Problem during Excel Import - Row:" + currentExcelRow.ToString());
                        sReport.Append(x.ToString());
                    }
                    util.CloseExcel();
                    // done with the excel file, now import the text file (Modules)
                    reader = new StreamReader(TextFile);
                    while (reader.Peek() != -1)
                    {
                        ConsumeTextLine(reader.ReadLine());
                    }
                    reader.Close();

                    try
                    {
                        foreach (SDLine line in sdlines)
                        {
                            currentTextLine = line;
                            if ((line.Children.Count > 0) && (line.Parent == null))
                            {
                                SaveLine(line);
                            }

                            if (keyedObjects.ContainsKey(line.Code))
                            {

                                List<MetaBase> excelObjects = keyedObjects[line.Code];
                                // Console.WriteLine("Found:" + excelObjects.Count.ToString() + " objects in excel for " + line.Code);
                                foreach (MetaBase mbase in excelObjects)
                                {
                                    // Console.WriteLine("saving map:" + line.MBase.pkid.ToString() + " --- " + mbase.pkid.ToString());
                                    Singletons.GetAssociationHelper().AddQuickAssociation(line.MBase.pkid, mbase.pkid, line.MBase.MachineName, mbase.MachineName, 4);
                                }
                            }
                            else
                            {
                                sReport.Append("Cannot find a valid mapping for: " + line.WholeWord + " (" + line.Code + ")" + Environment.NewLine);
                            }
                        }
                    }
                    catch (Exception xxx)
                    {
                        sReport.Append("Problem during mapping: CurrentSDLine:" + currentTextLine.Code + " " + currentTextLine.WholeWord);
                        sReport.Append(xxx.ToString());

                    }
                }
            }
            catch (Exception x)
            {
                sReport.Append("Problems occurred:" + Environment.NewLine);
                sReport.Append(x.ToString());
            }
            OnFinished(this, EventArgs.Empty);

        }

        public event EventHandler Finished;
        protected void OnFinished(object sender, EventArgs e)
        {
            if (Finished != null)
                Finished(sender, e);
        }
        private void SaveLine(SDLine line)
        {
            MetaBase MetaObject = (MetaBase)Activator.CreateInstanceFrom(Variables.Instance.MetaAssembly, "MetaBuilder.Meta.Generated.Software").Unwrap();
            MetaObject.Set("Type", "Executable");
            MetaObject.Set("Name", line.WholeWord);
            MetaObject.Set("ID", line.Code);
            MetaObject.Save(Guid.NewGuid());
            line.MBase = MetaObject;

            foreach (SDLine child in line.Children)
            {
                SaveLine(child);
                Singletons.GetAssociationHelper().AddQuickAssociation(line.MBase.pkid, child.MBase.pkid, line.MBase.MachineName, child.MBase.MachineName, 3);
            }

        }

        private List<SDLine> sdlines;
        private void ConsumeTextLine(string text)
        {
            SDLine line = new SDLine(sdlines.Count, text);
            if (line.Code != null && line.WholeWord != null)
            {
                sdlines.Add(line);
                SDLine parent = GetParentSDLine(line);
                if (parent != null)
                {
                    // Console.WriteLine(parent.Code.ToString() + " parent of: " + line.Code);
                    parent.AddChild(line);
                }
            }
        }
        private SDLine GetParentSDLine(SDLine child)
        {
            SDLine testParentSDLine;

            for (int x = child.LineNumber; x >= 0; x--)
            {
                testParentSDLine = sdlines[x];
                if (testParentSDLine.NumberOfTabs == child.NumberOfTabs - 1)
                {
                    return testParentSDLine;
                }
            }
            return null;
        }
        private void SetProperty(List<string> cols, string[] alphabet, int rowCounter, MetaBase MetaObject, string Property, string PropertyValue)
        {
            Type t = MetaObject.GetType().GetProperty(Property).PropertyType;
            if (t == typeof(String))
            {
                MetaObject.Set(Property, PropertyValue);
            }
            if (t == typeof(Int32))
            {
                MetaObject.Set(Property, int.Parse(PropertyValue));
            }
            if (t == typeof(DateTime))
            {
                MetaObject.Set(Property, Core.GlobalParser.ParseGlobalisedDateString(PropertyValue));
            }
            if (t == typeof(Double))
            {
                MetaObject.Set(Property, Double.Parse(PropertyValue, System.Globalization.CultureInfo.InvariantCulture));
            }
            if (t == typeof(Boolean))
            {
                MetaObject.Set(Property, bool.Parse(PropertyValue));
            }
        }

        private string GetText(string col, int row)
        {
            return (string)(string)util.CurrentSheet.get_Range(col + row.ToString(), col + row.ToString()).Text;
        }

        private class SDLine
        {
            private MetaBase mbase;

            public MetaBase MBase
            {
                get { return mbase; }
                set { mbase = value; }
            }

            private string code;
            public string Code
            {
                get { return code; }
                set { code = value; }
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
                get { return wholeWord; }
                set { wholeWord = value; }

            }
            private List<SDLine> children;
            public List<SDLine> Children
            {
                get { return children; }
                set { children = value; }
            }
            private SDLine parent;
            public SDLine Parent
            {
                get { return parent; }
                set
                {
                    parent = value;
                }
            }
            private int lineNumber;
            public int LineNumber
            {
                get { return lineNumber; }
                set { lineNumber = value; }
            }

            public SDLine(int LineNo, string text)
            {
                children = new List<SDLine>();
                char splitChar = '\t';
                string[] splitText = text.Split(splitChar);
                WholeWord = splitText[splitText.Length - 1];
                NumberOfTabs = splitText.Length - 1;
                LineNumber = LineNo;
                int lastDash = WholeWord.LastIndexOf("- ");
                if (lastDash > 0)
                {
                    code = WholeWord.Substring(0, lastDash - 1);
                }

            }

            public void AddChild(SDLine line)
            {
                children.Add(line);
                line.Parent = this;
            }

        }
    }


}
