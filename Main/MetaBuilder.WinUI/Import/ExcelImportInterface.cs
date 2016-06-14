using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;
using Janus.Windows.EditControls;
using MetaBuilder.BusinessFacade.Exports;
using MetaBuilder.Core;
using MetaBuilder.Meta;
using Microsoft.Office.Interop.Excel;
using Application = Microsoft.Office.Interop.Excel.Application;
using GroupBox = System.Windows.Forms.GroupBox;
using TextBox = System.Windows.Forms.TextBox;
using x = Microsoft.Office.Interop.Excel;
using System.Collections.ObjectModel;

namespace MetaBuilder.WinUI.Import
{
    internal delegate void ImportExcelTemplatesDelegate(string[] drawings);

    /// <summary>
    /// Imports drawings and outputs status messages to textbox. User has the ability to print the report, or cancel the import routine.
    /// </summary>
    public class ExcelImportInterface
    {
        #region Controls

        private OpenFileDialog openFileDialog1;

        #endregion

        public ExcelImportInterface()
        {

        }

        #region Shared Functions

        public void AfterImport(IAsyncResult result)
        {
            AsyncResult async = (AsyncResult)result;
            ImportExcelTemplatesDelegate importDelegate = (ImportExcelTemplatesDelegate)async.AsyncDelegate;
            this.reportWriter.Close();
            SplashScreen.PleaseWait.CloseForm();
        }

        public StreamWriter reportWriter;

        public void Import(bool MultipleFiles)
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            reportWriter = File.CreateText(Variables.Instance.ExportsPath + "\\" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + ".txt");
            this.openFileDialog1.Multiselect = MultipleFiles;
            openFileDialog1.InitialDirectory = Core.Variables.Instance.ExportsPath;
            DialogResult res = this.openFileDialog1.ShowDialog();
            if (res != DialogResult.OK)
                return;

            string[] drawings;
            if (MultipleFiles)
            {
                drawings = this.openFileDialog1.FileNames;
            }
            else
            {
                drawings = new string[1] { this.openFileDialog1.FileName };
            }

            SplashScreen.PleaseWait.ShowPleaseWaitForm();
            SplashScreen.PleaseWait.SetStatus("Importing");
            ImportExcelTemplatesDelegate importDelegate = new ImportExcelTemplatesDelegate(ImportDrawings);
            importDelegate.BeginInvoke(drawings, new AsyncCallback(AfterImport), null);
            //ImportDrawings(drawings);

            this.reportWriter.Close();
        }

        private MetaBuilder.BusinessFacade.Exports.ExcelUtil util;
        public void ImportDrawings(string[] drawings)
        {
            util = new ExcelUtil();
            util.OpenExcel();
            ImportExcelFile(drawings[0], reportWriter);
        }

        /// <summary>
        /// Helper function. Strips path info and returns "document.doc" if parameter = "c:\documents and settings\user\my documents\document.doc"
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        private string GetFileName(string FileName)
        {
            return FileName.Substring(FileName.LastIndexOf("\\") + 1, FileName.LastIndexOf(".") - (FileName.LastIndexOf("\\") + 1));
        }
        private bool IsValidName(string propname)
        {
            string lower = "|" + propname.ToLower() + "|";
            string teststring = "|pkid|-none-|isindatabase|state|_classname|vcuser|VCStatusid|workspacename|WorkspaceTypeId|machinename|embeddeditems|typefield|";
            int index = teststring.IndexOf(lower);
            return index == -1;
            //bool isvalid = lower != "isindatabase" && lower != "pkid" && lower != "state" && lower != "_classname" && lower != "vcuser" && lower != "VCStatusid" && lower != "workspacename" && lower != "WorkspaceTypeId" && lower != "machinename" && lower != "embeddeditems";
        }
        public bool ImportExcelFile(string FileName, StreamWriter ReportWriter)
        {
            try
            {
                util.OpenFile(FileName);
                System.Globalization.CultureInfo nonInvariantCulture = new System.Globalization.CultureInfo("en-US");
                System.Threading.Thread.CurrentThread.CurrentCulture = nonInvariantCulture;

                SplashScreen.PleaseWait.SetStatus("Building identifiers");
                string className = (string)util.CurrentSheet.get_Range("A1", "A1").Text;

                try
                {
                    if (DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassProvider.GetByName(className) == null)
                    {
                        MessageBox.Show("Cannot find import format identifiers", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                catch
                {
                    MessageBox.Show("Cannot find import format identifiers", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                List<string> cols = new List<string>();

                string[] alphabet = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", 
                                                    "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai","aj","ak","al","am","an","ao","ap","aq","ar","as","at","au","av","aw","ax","ay","az",
                                                    "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj","bk","bl","bm","bn","bo","bp","bq","br","bs","bt","bu","bv","bw","bx","by","bz"};
                int alphabetCounter = 2;
                int rowCounter = 3;
                string txt = GetText(alphabet[alphabetCounter], rowCounter);
                while (txt.Length>0 || (alphabetCounter == 0))
                {
                    cols.Add(txt);
                    alphabetCounter++;
                    txt = GetText(alphabet[alphabetCounter], rowCounter);
                }

                int NumberOfColumns = alphabetCounter;
                string[] values = new string[NumberOfColumns];
                rowCounter++;

                //get class name identifier column (defaults to the first column)
                string classIdentifierColumn = "C";
                string worksspaceIdentifierColumn = "";
                for (int r = 2; r < alphabetCounter; r++)
                {
                    if (GetText(alphabet[r], 3) == "Name")
                        classIdentifierColumn = alphabet[r].ToUpper();
                    if (GetText(alphabet[r], 3).ToLower().Contains("workspace"))
                        worksspaceIdentifierColumn = alphabet[r].ToUpper();
                }

                SplashScreen.PleaseWait.SetStatus("Saving objects");

                while (!string.IsNullOrEmpty(GetText(classIdentifierColumn, rowCounter) + "" + GetText("A", rowCounter) + "" + GetText("B", rowCounter)))
                {
                    try
                    {
                        MetaBase MetaObject = null; // Loader.CreateInstance(className);

                        int pkid = 0;
                        int.TryParse(GetText("A", rowCounter), out pkid);
                        string machname = GetText("B", rowCounter);
                        //if (machname.Length > 0)
                        //    MetaObject.MachineName = machname;

                        if (pkid > 0 && machname.Length > 0)
                        {
                            MetaObject = Loader.GetByID(className, pkid, machname);
                            if (MetaObject == null)
                            {
                                MetaObject = Loader.CreateInstance(className);
                                if (pkid > 0)
                                {
                                    MetaObject.pkid = pkid;
                                    MetaObject.MachineName = machname;
                                }
                            }
                        }
                        else
                        {
                            MetaObject = Loader.CreateInstance(className);
                        }

                        if (MetaObject == null)
                            continue; //this will never happen

                        Collection<PropertyInfo> propInfoCollection = MetaObject.GetMetaPropertyList(false);
                        for (int i = 2; i < NumberOfColumns; i++)
                        {

                            PropertyInfo pinfo = propInfoCollection[i - 2];
                            Type t = pinfo.PropertyType;

                            if (IsValidName(pinfo.Name.ToLower()))
                            {
                                // Console.WriteLine(t.FullName + GetText(alphabet[i], rowCounter));
                                bool setted = false;
                                if (t == typeof(String))
                                {
                                    MetaObject.Set(pinfo.Name, GetText(alphabet[i], rowCounter));
                                    setted = true;
                                }
                                if (t == typeof(MetaBuilder.Meta.LongText))
                                {
                                    string val = GetText(alphabet[i], rowCounter);
                                    if (val.Length > 0)
                                    {

                                        LongText lt = new LongText();
                                        lt.Text = val;
                                        MetaObject.Set(pinfo.Name, lt);
                                        setted = true;
                                    }
                                }
                                if (t.FullName.ToString().IndexOf("System.Nullable") > -1 && t.FullName.IndexOf("Int32") > -1)
                                {
                                    string val = GetText(alphabet[i], rowCounter);
                                    if (val.Length > 0)
                                    {
                                        MetaObject.Set(pinfo.Name, int.Parse(val));
                                        setted = true;
                                    }
                                }
                                if (t == typeof(Int32))
                                {
                                    if (GetText(alphabet[i], rowCounter).Length > 0)
                                    {
                                        MetaObject.Set(pinfo.Name, int.Parse(GetText(alphabet[i], rowCounter)));
                                        setted = true;
                                    }
                                }
                                if (t == typeof(DateTime))
                                {
                                    try
                                    {
                                        MetaObject.Set(pinfo.Name, Core.GlobalParser.ParseGlobalisedDateString(GetText(alphabet[i], rowCounter)));
                                    }
                                    catch { }
                                    setted = true;
                                }
                                if (t == typeof(Double))
                                {
                                    MetaObject.Set(pinfo.Name, Double.Parse(GetText(alphabet[i], rowCounter), System.Globalization.CultureInfo.InvariantCulture));
                                    setted = true;
                                }
                                if (t == typeof(Boolean))
                                {
                                    MetaObject.Set(pinfo.Name, bool.Parse(GetText(alphabet[i], rowCounter)));
                                    setted = true;
                                }
                                if (!setted)
                                {
                                    try
                                    {
                                        MetaObject.Set(pinfo.Name, GetText(alphabet[i], rowCounter));
                                    }
                                    catch { }
                                }

                            }
                        }

                        //Set workspace
                        if (worksspaceIdentifierColumn != "")
                        {
                            string ws = GetText(worksspaceIdentifierColumn, rowCounter);
                            if (ws.Length > 0)
                            {
                                if (Variables.Instance.WorkspaceHashtable.Contains(ws + "#1"))
                                {
                                    MetaObject.WorkspaceName = ws;
                                    MetaObject.WorkspaceTypeId = 1;
                                }
                                else if (Variables.Instance.WorkspaceHashtable.Contains(ws + "#2"))
                                {
                                    MetaObject.WorkspaceName = ws;
                                    MetaObject.WorkspaceTypeId = 2;
                                }
                                else if (Variables.Instance.WorkspaceHashtable.Contains(ws + "#3"))
                                {
                                    MetaObject.WorkspaceName = ws;
                                    MetaObject.WorkspaceTypeId = 3;
                                }
                            }
                        }

                        //if (MetaObject.ToString().Length > 0) //What if there are other properties but no name set?
                        {
                            try
                            {
                                MetaObject.SaveToRepository(Guid.NewGuid(), Core.Variables.Instance.ClientProvider);
                            }
                            catch
                            {
                            }
                        }
                    }
                    catch (Exception xx)
                    {
                        ReportWriter.WriteLine("Row: " + rowCounter.ToString() + " - " + xx.ToString());
                    }
                    rowCounter++;
                }

                util.CloseExcel();
                // visApp.ActiveWorkbook.Close(false, Missing.Value, Missing.Value);

                MessageBox.Show("Done with the import", "Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (ObjectDisposedException ex)
            {
                MetaBuilder.Core.Log.WriteLog(ex.ToString());
                return true;
            }
            catch (Exception importException)
            {
                MetaBuilder.Core.Log.WriteLog(importException.ToString());
                MessageBox.Show(importException.ToString());
                ReportWriter.WriteLine("File: " + FileName + " gave errors. If importing has stopped, try again, and exclude this file. Open it with VISIO to locate the issue. " + Environment.NewLine + Environment.NewLine + importException.ToString());
                return false;
            }
        }
        private string GetText(string col, int row)
        {
            return (string)(string)util.CurrentSheet.get_Range(col + row.ToString(), col + row.ToString()).Text;
        }
        #endregion

        #region Other Functions

        private void AddProgress(string Line)
        {
            /*	this.txtProgress.AppendText(Line + Environment.NewLine);
                this.txtProgress.SelectionStart = this.txtProgress.Text.Length;
                this.txtProgress.ScrollToCaret();*/
        }

        #endregion

    }
}