using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Meta;
using Microsoft.Office.Interop.Excel;
using b = MetaBuilder.BusinessLogic;
using x = Microsoft.Office.Interop.Excel;
using System;
namespace MetaBuilder.BusinessFacade.Exports
{
    // this class handles excel from an application point of view, 
    // killing COM processes is a hassle, so refactoring to a utility class 
    // makes integration easier & faster
    public class ExcelUtil
    {

        private Application excelApp;
        public Application ExcelApp
        {
            get { return excelApp; }
            set { excelApp = value; }
        }

        private _Worksheet currentSheet;
        public _Worksheet CurrentSheet
        {
            get { return currentSheet; }
            set { currentSheet = value; }
        }

        public void OpenExcel()
        {
            ExcelApp = new ApplicationClass();

        }

        public void CreateWorkbook()
        {
            ExcelApp.Workbooks.Add(Missing.Value);
            CurrentSheet = ExcelApp.ActiveSheet as _Worksheet;
        }

        public void CloseWorkbook()
        {
            ExcelApp.ActiveWorkbook.Close(false, Missing.Value, Missing.Value);
        }

        public void CloseExcel()
        {
            /*
        Hi Claudio,

Include the following:
objwb.Close()

objxl.DisplayAlerts = True

Marshal.ReleaseComObject(objws) ' this is the excel.worksheet object

objxl.Quit()

Marshal.ReleaseComObject(objxl) ' this is the excel.application object

HTH,*/
            if (excelApp != null)
            {
                if (currentSheet != null)
                    Marshal.ReleaseComObject(currentSheet);
                CurrentSheet = null;

                if (ExcelApp != null)
                {
                    if (ExcelApp.ActiveWorkbook != null)
                        ExcelApp.ActiveWorkbook.Close(Missing.Value, Missing.Value, Missing.Value);
                    ExcelApp.DisplayAlerts = true;
                    if (ExcelApp.ActiveWorkbook != null)
                        Marshal.ReleaseComObject(excelApp.ActiveWorkbook);
                    ExcelApp.Quit();
                    if (ExcelApp != null)
                        Marshal.ReleaseComObject(ExcelApp);
                    ExcelApp = null;
                }
                /*
                NAR(CurrentSheet);
                NAR(ExcelApp.ActiveWorkbook);

                NAR(ExcelApp);
                if (excelApp!=null)
                    ExcelApp = null;
                if (currentSheet !=null)
                    CurrentSheet = null;
            */
            }
            System.Threading.Thread.Sleep(1000);
            GC.Collect();
            //GC.WaitForPendingFinalizers();

        }

        public void SaveFile(string filename, bool CloseWhenDone)
        {
            ExcelApp.ActiveWorkbook.SaveAs(filename, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, XlSaveAsAccessMode.xlExclusive, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            if (CloseWhenDone)
            {
                CloseExcel();
            }
        }
        public void OpenFile(string filename)
        {
            ExcelApp.Workbooks.Open(filename, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            CurrentSheet = ExcelApp.ActiveWorkbook.ActiveSheet as Worksheet;
        }
        private void NAR(object o)
        {
            try
            {
                Marshal.ReleaseComObject(o);
            }
            catch
            {
            }
            finally
            {
                o = null;
            }
        }

        /// <summary>
        /// Safely adds a titled sheet. Names are limited to 30 characters.
        /// </summary>
        /// <param name="sheetName"></param>
        public void AddSheet(string sheetName)
        {
            CurrentSheet = ExcelApp.ActiveWorkbook.Sheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing) as _Worksheet;
            if (sheetName != null)
            {
                if (sheetName.Length > 30)
                {
                    CurrentSheet.Name = sheetName.Substring(0, 30);
                }
                else
                    CurrentSheet.Name = sheetName;
            }
        }

    }
}