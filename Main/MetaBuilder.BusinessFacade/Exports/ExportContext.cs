using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.DataAccessLayer.OldCode.Diagramming;
using MetaBuilder.Meta;
using Microsoft.Office.Interop.Excel;
using b = MetaBuilder.BusinessLogic;
using x = Microsoft.Office.Interop.Excel;
using System.Collections.ObjectModel;

namespace MetaBuilder.BusinessFacade.Exports
{
    public class ExportContext
    {
        private ExcelUtil excelUtil;
        string customFileName = "";
        public ExportContext()
        {
            customFileName = ExportAid.GetCustomFilename("Enter a filename for this export", "Excel File *.xls|*.xls");
            if (ExportAid.CancelExport(customFileName))
                return;

            objectsToExport = new List<MetaBase>();
            excelUtil = new ExcelUtil();
            excelUtil.OpenExcel();
            excelUtil.CreateWorkbook();
        }
        internal string[] alphabet = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", 
                                                    "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai","aj","ak","al","am","an","ao","ap","aq","ar","as","at","au","av","aw","ax","ay","az",
                                                    "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj","bk","bl","bm","bn","bo","bp","bq","br","bs","bt","bu","bv","bw","bx","by","bz"};
        private MetaBase metabaseMain;
        public MetaBase MetaBaseMain
        {
            get { return metabaseMain; }
            set { metabaseMain = value; }
        }

        private List<MetaBase> objectsToExport;

        public List<MetaBase> ObjectsToExport
        {
            get { return objectsToExport; }
            set { objectsToExport = value; }
        }

        public string FileName
        {
            get { return filename; }
        }
        internal string filename;


        int lastRowCounter;
        int objectIndex;
        public void Export()
        {
            if (excelUtil == null)
                return;

            lastRowCounter = 1;
            if (objectsToExport.Count > 0)
            {
                foreach (MetaBase mbase in objectsToExport)
                {
                    metabaseMain = mbase;
                    lastRowCounter = 1;
                    AddSheets();
                    ExportDetails();
                    ExportDiagrams();
                    ExportRelatedObjects(true);
                    ExportRelatedObjects(false);
                    objectIndex++;
                }
            }
            else
            {
                AddSheets();
                ExportDetails();
                ExportDiagrams();
                ExportRelatedObjects(true);
                ExportRelatedObjects(false);
            }
            Marshal.ReleaseComObject(excelUtil.CurrentSheet);
            excelUtil.SaveFile(customFileName, false);
            //string filename = MetaBuilder.Core.Variables.Instance.ExportsPath + "Context Viewer Explode " + Core.strings.GetDateStampString() + ".xls";
            //excelUtil.SaveFile(filename, true);
            excelUtil.CloseExcel();
            filename = customFileName;
            GC.Collect();
        }

        private void ExportDetails()
        {
            excelUtil.CurrentSheet.get_Range("A1", "A1").Value2 = MetaBaseMain.ToString();
            excelUtil.CurrentSheet.get_Range("A2", "A2").Value2 = "Identifiers";
            excelUtil.CurrentSheet.get_Range("A1", "A2").Font.Bold = true;
            excelUtil.CurrentSheet.get_Range("A3", "A3").Value2 = "Class:";
            excelUtil.CurrentSheet.get_Range("B3", "B3").Value2 = MetaBaseMain._ClassName;
            excelUtil.CurrentSheet.get_Range("A4", "A4").Value2 = "pkid";
            excelUtil.CurrentSheet.get_Range("B4", "B4").Value2 = MetaBaseMain.pkid.ToString();
            excelUtil.CurrentSheet.get_Range("A5", "A5").Value2 = "Machine";
            excelUtil.CurrentSheet.get_Range("B5", "B5").Value2 = MetaBaseMain.MachineName;

            Collection<PropertyInfo> props = MetaBaseMain.GetMetaPropertyList(false);
            Type t = MetaBaseMain.GetType();

            excelUtil.CurrentSheet.get_Range("A7", "A7").Value2 = "Properties";
            excelUtil.CurrentSheet.get_Range("A7", "A7").Font.Bold = true;
            int rowCounter = 8;
            foreach (PropertyInfo prop in props)
            {
                excelUtil.CurrentSheet.get_Range("A" + rowCounter.ToString(), "A" + rowCounter.ToString()).Value2 = prop.Name;
                if (MetaBaseMain.Get(prop.Name) != null)
                {
                    excelUtil.CurrentSheet.get_Range("B" + rowCounter.ToString(), "B" + rowCounter.ToString()).Value2 = MetaBaseMain.Get(prop.Name).ToString();
                }
                rowCounter++;
            }
            lastRowCounter = rowCounter;
        }
        private void ExportDiagrams()
        {
            excelUtil.CurrentSheet.get_Range("A" + (1 + lastRowCounter).ToString(), "A" + (1 + lastRowCounter).ToString()).Value2 = "Diagrams Containing This Object";
            TList<GraphFileObject> gfos = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileObjectProvider.GetByMetaObjectIDMachineID(MetaBaseMain.pkid, MetaBaseMain.MachineName);
            TempFileGraphAdapter tfga = new TempFileGraphAdapter();
            excelUtil.CurrentSheet.get_Range("A" + (2 + lastRowCounter).ToString(), "A" + (2 + lastRowCounter).ToString()).Value2 = "Name";
            excelUtil.CurrentSheet.get_Range("B" + (2 + lastRowCounter).ToString(), "B" + (2 + lastRowCounter).ToString()).Value2 = "Version";
            excelUtil.CurrentSheet.get_Range("C" + (2 + lastRowCounter).ToString(), "C" + (2 + lastRowCounter).ToString()).Value2 = "GraphFileID";
            excelUtil.CurrentSheet.get_Range("D" + (2 + lastRowCounter).ToString(), "D" + (2 + lastRowCounter).ToString()).Value2 = "GraphFileMachine";
            excelUtil.CurrentSheet.get_Range("A" + (1 + lastRowCounter).ToString(), "D" + (2 + lastRowCounter).ToString()).Font.Bold = true;

            int rowCounter = lastRowCounter + 3;
            foreach (GraphFileObject gfo in gfos)
            {

                GraphFile file = tfga.GetQuickFileDetails(gfo.GraphFileID, gfo.GraphFileMachine, false);
                if (file.IsActive)
                {
                    excelUtil.CurrentSheet.get_Range("A" + rowCounter.ToString(), "A" + rowCounter.ToString()).Value2 = strings.GetFileNameOnly(file.Name);
                    excelUtil.CurrentSheet.get_Range("B" + rowCounter.ToString(), "B" + rowCounter.ToString()).Value2 = file.MajorVersion.ToString() + "." + file.MinorVersion.ToString();
                    excelUtil.CurrentSheet.get_Range("C" + rowCounter.ToString(), "C" + rowCounter.ToString()).Value2 = file.pkid.ToString();
                    excelUtil.CurrentSheet.get_Range("D" + rowCounter.ToString(), "D" + rowCounter.ToString()).Value2 = file.Machine;
                    rowCounter++;
                }
            }
            lastRowCounter = rowCounter;
        }
        TList<ClassAssociation> classAssociations;
        private void LoadAssociations()
        {
            this.classAssociations = DataRepository.ClassAssociationProvider.GetAll();
        }

        private string GetAssociationTypeName(int CAid)
        {

            classAssociations.Filter = "CAid = " + CAid.ToString();
            return ((AssociationTypeList)classAssociations[0].AssociationTypeID).ToString();
        }

        private string GetCaption(int CAid)
        {

            classAssociations.Filter = "CAid = " + CAid.ToString();
            return classAssociations[0].Caption;
        }

        void ExportRelatedObjects(bool parents)
        {
            int sheetnumber = parents ? 3 : 4;
            string fromOrTo = parents ? " from " : " to ";
            LoadAssociations();

            TList<ObjectAssociation> associatedObjects;
            if (parents)
                associatedObjects = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByChildObjectIDChildObjectMachine(metabaseMain.pkid, metabaseMain.MachineName);
            else
                associatedObjects = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(metabaseMain.pkid, metabaseMain.MachineName);


            Dictionary<int, List<MetaObjectKey>> CAidObjects = new Dictionary<int, List<MetaObjectKey>>();
            foreach (ObjectAssociation assoc in associatedObjects)
            {
                if (!CAidObjects.ContainsKey(assoc.CAid))
                {
                    CAidObjects.Add(assoc.CAid, new List<MetaObjectKey>());

                }
                MetaObjectKey key = new MetaObjectKey();
                if (parents)
                {
                    key.pkid = assoc.ObjectID;
                    key.Machine = assoc.ObjectMachine;
                }
                else
                {
                    key.pkid = assoc.ChildObjectID;
                    key.Machine = assoc.ChildObjectMachine;
                }
                CAidObjects[assoc.CAid].Add(key);
            }

            int rowCounter = lastRowCounter + 1;
            foreach (KeyValuePair<int, List<MetaObjectKey>> CAidKey in CAidObjects)
            {
                ClassAssociation classAssociation = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByCAid(CAidKey.Key);
                string className = parents ? classAssociation.ParentClass : classAssociation.ChildClass;

                excelUtil.CurrentSheet.get_Range("A" + rowCounter.ToString(), "A" + rowCounter.ToString()).Value2 = "Class:";
                excelUtil.CurrentSheet.get_Range("B" + rowCounter.ToString(), "B" + rowCounter.ToString()).Value2 = className;
                excelUtil.CurrentSheet.get_Range("C" + rowCounter.ToString(), "C" + rowCounter.ToString()).Value2 = GetAssociationTypeName(classAssociation.CAid) + fromOrTo;
                excelUtil.CurrentSheet.get_Range("D" + rowCounter.ToString(), "D" + rowCounter.ToString()).Value2 = "Caption:";
                excelUtil.CurrentSheet.get_Range("E" + rowCounter.ToString(), "E" + rowCounter.ToString()).Value2 = GetCaption(classAssociation.CAid);

                excelUtil.CurrentSheet.get_Range("A" + rowCounter.ToString(), "E" + rowCounter.ToString()).Font.Bold = true;
                rowCounter++;
                StringBuilder sb = new StringBuilder();
                sb.Append("1 = 0 ");
                foreach (MetaObjectKey mkey in CAidKey.Value)
                {
                    sb.Append(" OR cast(pkid as varchar(10)) + '|' + machine = '" + mkey.pkid.ToString() + "|" + mkey.Machine + "' ");
                }

                string usedClass = parents ? classAssociation.ParentClass : classAssociation.ChildClass;
                SqlCommand cmd = new SqlCommand("select * from METAView_" + usedClass + "_Listing where " + sb.ToString(), new SqlConnection(Variables.Instance.ConnectionString));
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter dap = new SqlDataAdapter();
                dap.SelectCommand = cmd;
                DataSet ds = new DataSet();
                dap.Fill(ds, "Objects");

                DataView dvParents = ds.Tables["Objects"].DefaultView;
                int colCounter = 0;
                foreach (DataColumn dcCol in ds.Tables["Objects"].Columns)
                {
                    if (OutputThisColumn(dcCol))
                    {
                        excelUtil.CurrentSheet.get_Range(alphabet[colCounter] + rowCounter.ToString(), alphabet[colCounter] + rowCounter.ToString()).Value2 = dcCol.ColumnName;
                        colCounter++;
                    }
                }
                excelUtil.CurrentSheet.get_Range("A" + rowCounter.ToString(), alphabet[colCounter] + rowCounter.ToString()).Font.Bold = true;
                rowCounter++;
                colCounter = 0;
                foreach (DataRowView drv in dvParents)
                {
                    colCounter = 0;
                    foreach (DataColumn dcCol in ds.Tables["Objects"].Columns)
                    {
                        if (OutputThisColumn(dcCol))
                        {
                            excelUtil.CurrentSheet.get_Range(alphabet[colCounter] + rowCounter.ToString(), alphabet[colCounter] + rowCounter.ToString()).Value2 = drv[dcCol.ColumnName].ToString();
                            colCounter++;
                        }
                    }
                    rowCounter++;
                }
                lastRowCounter = rowCounter;
                rowCounter = rowCounter + 1;
            }
        }

        private static bool OutputThisColumn(DataColumn dcCol)
        {
            return dcCol.ColumnName.ToLower() != "WorkspaceTypeId" && dcCol.ColumnName.ToLower() != "VCMachineID" && dcCol.ColumnName.ToLower() != "VCStatusID";
        }

        internal void AddSheets()
        {
            string s = metabaseMain.ToString();
            if (s.Length > 10)
                s = s.Substring(0, 10);
            if (excelUtil.ExcelApp.ActiveWorkbook.Sheets.Count > objectIndex + 1)
            {
                excelUtil.CurrentSheet = excelUtil.ExcelApp.ActiveWorkbook.Sheets[objectIndex + 1] as _Worksheet;


                excelUtil.CurrentSheet.Name = s + " Context";
            }
            else
            {
                excelUtil.AddSheet(s + " Context");
            }
        }
    }
}