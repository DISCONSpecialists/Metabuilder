using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Meta;
using Microsoft.Office.Interop.Excel;
using x = Microsoft.Office.Interop.Excel;
using System.Collections.ObjectModel;

namespace MetaBuilder.BusinessFacade.Exports
{
    public class ExcelTemplate
    {
        private ExcelUtil util;
        public bool ExportTemplate(string classname, bool IncludeData)
        {
            string customFileName = ExportAid.GetCustomFilename("Enter a filename for this export", "Excel File *.xls|*.xls");
            if (ExportAid.CancelExport(customFileName))
                return false;

            util = new ExcelUtil();
            util.OpenExcel();
            util.CreateWorkbook();
            util.CurrentSheet.get_Range("A1", "A1").Value2 = classname;
            MetaBase MetaObject = Loader.CreateInstance(classname);
            string[] alphabet = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", 
                                                    "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai","aj","ak","al","am","an","ao","ap","aq","ar","as","at","au","av","aw","ax","ay","az",
                                                    "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj","bk","bl","bm","bn","bo","bp","bq","br","bs","bt","bu","bv","bw","bx","by","bz"};
            Collection<PropertyInfo> props = MetaObject.GetMetaPropertyList(false);
            int i = 0;
            foreach (PropertyInfo info in props)
            {
                if (info.Name != "WorkspaceTypeId")
                {
                    util.CurrentSheet.get_Range(alphabet[i + 2] + "2", alphabet[i + 2] + "2").Value2 = "[" + info.PropertyType.ToString().Replace("System.", "") + "]";
                    util.CurrentSheet.get_Range(alphabet[i + 2] + "3", alphabet[i + 2] + "3").Value2 = info.Name;
                    i++;
                }
            }

            util.CurrentSheet.get_Range("C1", "C1").Value2 = "Do not modify the first 3 rows' text or values";
            util.CurrentSheet.get_Range("A1", "Z1").Font.Bold = true;
            util.CurrentSheet.get_Range("A3", "Z3").Font.Bold = true;

            if (IncludeData)
            {
                TList<MetaObject> objectsInDatabase = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetByClass(classname);
                int rowCount = 4;
                foreach (MetaObject metaObject in objectsInDatabase)
                {
                    MetaBase mbase = Loader.GetByID(classname, metaObject.pkid, metaObject.Machine);

                    if (!VCStatusTool.IsObsoleteOrMarkedForDelete(mbase))
                    {
                        i = 0;
                        foreach (PropertyInfo pinfo in props)
                        {
                            if (pinfo.Name != "WorkspaceTypeId")
                            {
                                if (mbase.Get(pinfo.Name) != null)
                                {
                                    util.CurrentSheet.get_Range(alphabet[i + 2] + rowCount.ToString(), alphabet[i + 2] + rowCount.ToString()).Value2 = mbase.Get(pinfo.Name).ToString();
                                }
                                i++;
                            }
                        }
                        util.CurrentSheet.get_Range(alphabet[0] + rowCount.ToString(), alphabet[0] + rowCount.ToString()).Value2 = mbase.pkid.ToString();
                        util.CurrentSheet.get_Range(alphabet[1] + rowCount.ToString(), alphabet[1] + rowCount.ToString()).Value2 = mbase.MachineName;
                        rowCount++;
                    }
                }
            }
            util.SaveFile(customFileName, false);
            //FileName = Variables.Instance.ExportsPath + "MetaBuilder Import Template for " + classname + " - " + Core.strings.GetDateStampString() + ".xls";
            //util.SaveFile(FileName, true);
            util.CloseExcel();
            GC.Collect();
            FileName = customFileName;
            return true;
        }
        private string filename;

        public string FileName
        {
            get { return filename; }
            set { filename = value; }
        }

    }
}