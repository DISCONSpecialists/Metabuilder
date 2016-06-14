using System;
using System.Collections.Generic;
using System.Data;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Meta;
using Microsoft.Office.Interop.Excel;
using b = MetaBuilder.BusinessLogic;
using x = Microsoft.Office.Interop.Excel;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.BusinessFacade.Exports
{
    public class DetailedExcelAssocExporter : BaseExcelAssocExporter
    {
        private Dictionary<ObjectAssociationKey, List<MetaBase>> artefacts;
        public Dictionary<ObjectAssociationKey, List<MetaBase>> Artefacts
        {
            get { return artefacts; }
            set { artefacts = value; }
        }

        TList<ClassAssociation> classAssociations;
        private void LoadAssociations()
        {
            this.classAssociations = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetAll();
            // lets load the artefacts

            LoadArtefacts();
        }

        private void LoadArtefacts()
        {
            Artefacts = new Dictionary<ObjectAssociationKey, List<MetaBase>>();
            foreach (ObjectAssociationKey obakey in Associations)
            {
                DataSet ds =
                    Singletons.GetAssociationHelper().GetArtifactData(
                                obakey.CAid,
                                obakey.ObjectID,
                                obakey.ChildObjectID,
                                obakey.ObjectMachine,
                                obakey.ChildObjectMachine
                            );
                DataView dvAssociationObjectClass = ds.Tables[2].DefaultView;
                DataView dvAllowedClasses = ds.Tables[0].DefaultView;
                DataView dvArtifacts = ds.Tables[1].DefaultView;
                // Check if the association has a secondary class associated with it
                List<MetaBase> artList = new List<MetaBase>();
                foreach (DataRowView drvArt in dvArtifacts)
                {
                    string className = drvArt["ArtifactClass"].ToString();
                    int pkid = int.Parse(drvArt["ArtifactObjectID"].ToString());
                    string machine = drvArt["ArtefactMachine"].ToString();
                    MetaBase mb = Loader.GetByID(className, pkid, machine);
                    artList.Add(mb);
                }

                Artefacts.Add(obakey, artList);
            }
        }

        private string GetAssociationTypeName(int CAid)
        {

            classAssociations.Filter = "CAid = " + CAid.ToString();
            return ((AssociationTypeList)classAssociations[0].AssociationTypeID).ToString();
        }
        internal override void Export()
        {
            columnNames = SetupColumnNames();
            LoadAssociations();
            // Console.WriteLine("Detailed Export...");
            base.Export();

            foreach (MetaBase mbRow in this.RowObjects)
            {
                int currentRowIndex = rowIndexes[mbRow];
                int MaxShift = 0;

                foreach (MetaBase mbCol in this.ColumnObjects)
                {
                    int shiftForThisColumn = 0;
                    List<ObjectAssociationKey> assocs = GetAssociationsForRowAndColumn(mbRow, mbCol);
                    foreach (ObjectAssociationKey okey in assocs)
                    {
                        shiftForThisColumn++;
                        List<MetaBase> artefacts;
                        if (Artefacts.ContainsKey(okey))
                        {
                            artefacts = Artefacts[okey];
                            shiftForThisColumn += artefacts.Count;
                        }
                    }

                    if (shiftForThisColumn > MaxShift)
                        MaxShift = shiftForThisColumn;
                }

                int insertPoint = currentRowIndex + 1;
                if (MaxShift > 0)
                {
                    for (int i = 1; i < MaxShift; i++)
                    {
                        util.CurrentSheet.get_Range("A" + insertPoint.ToString(), "A" + insertPoint.ToString()).EntireRow.Insert(XlDirection.xlDown, Type.Missing);
                    }
                    for (int increaser = RowObjects.IndexOf(mbRow) + 1; increaser < RowObjects.Count; increaser++)
                    {
                        rowIndexes[RowObjects[increaser]] = rowIndexes[RowObjects[increaser]] + MaxShift - 1;
                    }
                }
            }

            // Shifting has been done! Now add the Associations...
            foreach (MetaBase mbRow in this.RowObjects)
            {
                int currentRowIndex = rowIndexes[mbRow];
                //int MaxShift = 0;

                foreach (MetaBase mbCol in this.ColumnObjects)
                {
                    int shiftForThisColumn = 0;
                    List<ObjectAssociationKey> assocs = GetAssociationsForRowAndColumn(mbRow, mbCol);
                    int colAlphabet = ColumnObjects.IndexOf(mbCol) + 1;
                    foreach (ObjectAssociationKey okey in assocs)
                    {
                        int thisIntersectionRow = currentRowIndex + shiftForThisColumn;
                        shiftForThisColumn++;


                        util.CurrentSheet.get_Range(columnNames[colAlphabet] + thisIntersectionRow.ToString(), columnNames[colAlphabet] + thisIntersectionRow.ToString()).Value2 = GetAssociationTypeName(okey.CAid);
                        util.CurrentSheet.get_Range(columnNames[colAlphabet] + thisIntersectionRow.ToString(), columnNames[colAlphabet] + thisIntersectionRow.ToString()).Font.Name = "Arial";
                        List<MetaBase> artefacts;
                        if (Artefacts.ContainsKey(okey))
                        {
                            artefacts = Artefacts[okey];
                            foreach (MetaBase mbArt in artefacts)
                            {
                                thisIntersectionRow++;
                                util.CurrentSheet.get_Range(columnNames[colAlphabet] + thisIntersectionRow.ToString(), columnNames[colAlphabet] + thisIntersectionRow.ToString()).Value2 = "\tArtefact: " + mbArt.ToString() + " [" + mbArt._ClassName + "]";
                                util.CurrentSheet.get_Range(columnNames[colAlphabet] + thisIntersectionRow.ToString(), columnNames[colAlphabet] + thisIntersectionRow.ToString()).Font.Name = "Arial";
                            }
                            shiftForThisColumn += artefacts.Count;
                        }
                    }
                    //sheet.get_Range(alphabet[colAlphabet] + "1", alphabet[colAlphabet] + "1000").AutoFit();
                }
            }
            filename = Variables.Instance.ExportsPath + "MetaBuilder Relationship Manager Export " + Core.strings.GetDateStampString() + ".xls";
        }

        private List<ObjectAssociationKey> GetAssociationsForRowAndColumn(MetaBase mbRow, MetaBase mbCol)
        {
            List<ObjectAssociationKey> retval = new List<ObjectAssociationKey>();
            foreach (ObjectAssociationKey assocKey in this.Associations)
            {
                if (mbRow.pkid == assocKey.ObjectID && mbRow.MachineName == assocKey.ObjectMachine
                    &&
                    mbCol.pkid == assocKey.ChildObjectID && mbCol.MachineName == assocKey.ChildObjectMachine)
                    retval.Add(assocKey);
            }
            return retval;
        }
        internal void InsertRow(int index)
        {

        }

        public override string ExportMatrix()
        {
            if (artefacts == null)
                throw new Exception("Artefacts must be set");
            util.OpenExcel();
            util.CreateWorkbook();
            Export();
            util.SaveFile(filename, false);

            util.CloseWorkbook();
            util.CloseExcel();
            util = null;
            GC.Collect();
            return filename;
        }
    }
}
