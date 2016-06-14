using System;
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

namespace MetaBuilder.BusinessFacade.Exports
{
    public class BaseExcelAssocExporter
    {
        internal ExcelUtil util;

        public BaseExcelAssocExporter()
        {
            util = new ExcelUtil();
        }

        internal string[] alphabet = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };//, 
        //"aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai","aj","ak","al","am","an","ao","ap","aq","ar","as","at","au","av","aw","ax","ay","az",
        //"ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj","bk","bl","bm","bn","bo","bp","bq","br","bs","bt","bu","bv","bw","bx","by","bz"};

        internal string[] columnNames = new string[] { };

        //, "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai" };
        private List<MetaBase> rowObjects;

        public List<MetaBase> RowObjects
        {
            get { return rowObjects; }
            set { rowObjects = value; }
        }

        private List<MetaBase> columnObjects;
        public List<MetaBase> ColumnObjects
        {
            get { return columnObjects; }
            set { columnObjects = value; }
        }
        private List<ObjectAssociationKey> associations;

        public List<ObjectAssociationKey> Associations
        {
            get { return associations; }
            set { associations = value; }
        }
        internal Dictionary<MetaBase, int> rowIndexes;
        internal string filename;

        internal string[] SetupColumnNames()
        {
            string[] retval = new string[256];

            for (int i = 0; i < alphabet.Length; i++)
            {
                retval[i] = alphabet[i];
            }
            int colNameCounter = alphabet.Length;
            for (int i = 0; i < alphabet.Length; i++)
            {
                string firstLetter = alphabet[i];

                bool isColumnI = firstLetter == "i";

                for (int x = 0; x < alphabet.Length; x++)
                {
                    string tester = firstLetter + alphabet[x];
                    if (tester == "iv")
                    {
                        retval[colNameCounter] = tester;
                        return retval;
                    }
                    else
                        retval[colNameCounter] = firstLetter + alphabet[x];
                    colNameCounter++;
                }
            }
            return null;
        }

        public virtual string ExportMatrix()
        {
            columnNames = SetupColumnNames();
            util.OpenExcel();
            util.CreateWorkbook();

            Export();

            if (filename == null)
                filename = MetaBuilder.Core.Variables.Instance.ExportsPath + "\\RelationshipManager Export " + Core.strings.GetDateStampString() + ".xls";
            try
            {
                util.ExcelApp.ActiveWorkbook.SaveAs(filename, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, XlSaveAsAccessMode.xlExclusive, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            }
            catch
            {
            }
            util.CloseWorkbook();
            util.CloseExcel();
            util = null;
            GC.Collect();
            return filename;
        }

        //int rowCounter;
        //int columnCounter;

        internal virtual void Export()
        {
            //rowCounter = 2;
            // first export the row objects
            rowIndexes = new Dictionary<MetaBase, int>();

            while (columnObjects.Count > 254)
            {
                columnObjects.RemoveAt(columnObjects.Count - 1);
            }
            for (int i = 0; i < columnObjects.Count; i++)
            {
                string colString = columnNames[i + 1] + "1";
                util.CurrentSheet.get_Range(colString, colString).Value2 = columnObjects[i].ToString() + " [" + columnObjects[i]._ClassName + "]";
            }
            for (int i = 0; i < rowObjects.Count; i++)
            {
                string rowString = (i + 2).ToString();
                util.CurrentSheet.get_Range("A" + rowString, "A" + rowString).Value2 = rowObjects[i].ToString() + " [" + rowObjects[i]._ClassName + "]";
                util.CurrentSheet.get_Range("A" + rowString, "A" + rowString).Font.Name = "Arial";
                rowIndexes.Add(rowObjects[i], i + 2);
            }

            // add X's - hierdie sal ROW based moet word
            foreach (ObjectAssociationKey assoc in this.associations)
            {
                MetaBase mbRow = GetRowObject(assoc.ObjectID, assoc.ObjectMachine);
                MetaBase mbCol = GetColumnObject(assoc.ChildObjectID, assoc.ChildObjectMachine);

                int rowIndex = rowObjects.IndexOf(mbRow) + 2;
                int columnIndex = columnObjects.IndexOf(mbCol) + 1;

                string intersectionRange = columnNames[columnIndex] + rowIndex.ToString();
                util.CurrentSheet.get_Range(intersectionRange, intersectionRange).Font.Name = "Marlett";
                util.CurrentSheet.get_Range(intersectionRange, intersectionRange).Value2 = "a";
                //sheet.get_Range(intersectionRange, intersectionRange).AutoFit();
            }
            filename = Variables.Instance.ExportsPath + "MetaBuilder Relationship Manager Export " + Core.strings.GetDateStampString() + ".xls";

        }
        internal MetaBase GetColumnObject(int pkid, string machine)
        {
            foreach (MetaBase mb in ColumnObjects)
            {
                if (mb.pkid == pkid && mb.MachineName == machine)
                    return mb;
            }
            return null;
        }
        internal MetaBase GetRowObject(int pkid, string machine)
        {
            foreach (MetaBase mb in rowObjects)
            {
                if (mb.pkid == pkid && mb.MachineName == machine)
                    return mb;
            }
            return null;
        }

        public string DoExport(string classname, string childclass, int assocTypeID)
        {
            util.CurrentSheet.get_Range("A1", "A1").Value2 = classname;
            util.CurrentSheet.get_Range("B1", "B1").Value2 = childclass;
            util.CurrentSheet.get_Range("C1", "C1").Value2 = "Associated";

            TList<ClassAssociation> assocDefinitions = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByParentClass(classname);
            foreach (ClassAssociation classAssociation in assocDefinitions)
            {
                if (classAssociation.ChildClass == childclass && classAssociation.AssociationTypeID == assocTypeID)
                {
                    TList<ObjectAssociation> objectAssociations = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByCAid(classAssociation.CAid);
                    int rowIndex = 2;
                    foreach (ObjectAssociation objAssoc in objectAssociations)
                    {
                        MetaBase mbase1 = Loader.GetByID(classname, objAssoc.ObjectID, objAssoc.ObjectMachine);
                        util.CurrentSheet.get_Range("A" + rowIndex.ToString(), "A" + rowIndex.ToString()).Value2 = mbase1.ToString();
                        MetaBase mbase2 = Loader.GetByID(childclass, objAssoc.ChildObjectID, objAssoc.ChildObjectMachine);
                        util.CurrentSheet.get_Range("B" + rowIndex.ToString(), "B" + rowIndex.ToString()).Value2 = mbase2.ToString();
                        util.CurrentSheet.get_Range("C" + rowIndex.ToString(), "C" + rowIndex.ToString()).Value2 = "1";
                        rowIndex++;
                    }
                }
            }

            string filename = Variables.Instance.ExportsPath + "MetaBuilder Mapping Export for " + classname + ".xls";

            return filename;
        }
    }
}