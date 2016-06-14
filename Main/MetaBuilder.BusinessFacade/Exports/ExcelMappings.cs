using System;
using System.Reflection;
using System.Runtime.InteropServices;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Meta;
using Microsoft.Office.Interop.Excel;
using x = Microsoft.Office.Interop.Excel;
using b = MetaBuilder.BusinessLogic;

namespace MetaBuilder.BusinessFacade.Exports
{
    public class ExcelMappings
    {

        private b.Workspace workspace;

        public b.Workspace Workspace
        {
            get { return workspace; }
            set { workspace = value; }
        }

        private ExcelUtil util;
        public string DoExport(string classname, string childclass, int assocTypeID)
        {
            //System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
            //sfd.Title = "Enter a filename for this export";
            //sfd.Filter = "Excel File *.xls|*.xls";
            //sfd.InitialDirectory = Core.Variables.Instance.ExportsPath;
            //sfd.ShowDialog(this);
            //string customFileName = Core.Variables.Instance.ExportsPath + "ExcelMappingsExport " + DateTime.Today.ToLongDateString() + " " + DateTime.Today.ToLongTimeString() + ".xls";//sfd.FileName;
            //if (ExportAid.CancelExport(customFileName))
            //    return "";

            util = new ExcelUtil();
            util.OpenExcel();
            util.CreateWorkbook();
            util.CurrentSheet.get_Range("A1", "A1").Value2 = classname;
            util.CurrentSheet.get_Range("B1", "B1").Value2 = childclass;
            util.CurrentSheet.get_Range("C1", "C1").Value2 = "Associated";

            TList<ClassAssociation> assocDefinitions = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByParentClass(classname);
            int rowIndex = 2;
            string associationName = "";
            try
            {
                foreach (ClassAssociation classAssociation in assocDefinitions)
                {
                    if (classAssociation.ChildClass == childclass && classAssociation.AssociationTypeID == assocTypeID)
                    {
                        if (associationName.Length == 0)
                            associationName = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.AssociationTypeProvider.GetBypkid(classAssociation.AssociationTypeID).Name;

                        TList<ObjectAssociation> objectAssociations = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByCAid(classAssociation.CAid);
                        foreach (ObjectAssociation objAssoc in objectAssociations)
                        {
                            if (!VCStatusTool.IsObsoleteOrMarkedForDelete(objAssoc))
                            {
                                MetaBase mbase1 = Loader.GetByID(classname, objAssoc.ObjectID, objAssoc.ObjectMachine);
                                MetaBase mbase2 = Loader.GetByID(childclass, objAssoc.ChildObjectID, objAssoc.ChildObjectMachine);

                                bool shouldExport = true;
                                if (workspace != null)
                                {
                                    bool mbaseValid1 = (mbase1.WorkspaceName == workspace.Name && mbase1.WorkspaceTypeId == workspace.WorkspaceTypeId);
                                    bool mbaseValid2 = (mbase2.WorkspaceName == workspace.Name && mbase2.WorkspaceTypeId == workspace.WorkspaceTypeId);
                                    if (!(mbaseValid1 && mbaseValid2))
                                    {
                                        shouldExport = false;
                                    }
                                }

                                if (shouldExport)
                                {
                                    util.CurrentSheet.get_Range("A" + rowIndex.ToString(), "A" + rowIndex.ToString()).Value2 = mbase1.ToString();
                                    util.CurrentSheet.get_Range("B" + rowIndex.ToString(), "B" + rowIndex.ToString()).Value2 = mbase2.ToString();
                                    util.CurrentSheet.get_Range("C" + rowIndex.ToString(), "C" + rowIndex.ToString()).Value2 = "1";
                                    rowIndex++;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                util.CurrentSheet.get_Range("E" + rowIndex.ToString(), "E" + rowIndex.ToString()).Value2 = ex.ToString();
            }

            if (associationName.Length == 0)
                util.CurrentSheet.get_Range("A2", "A2").Value2 = "No associations found";
            else
                associationName = " - " + associationName;

            string filename = Variables.Instance.ExportsPath + "MetaBuilder Association Export for " + classname + " to " + childclass + associationName.Trim() + ".xls";
            util.SaveFile(filename, true);
            util.CloseExcel();
            util = null;
            GC.Collect();
            return filename;
        }

        public string DoExport(string classname, string childclass)
        {
            //System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
            //sfd.Title = "Enter a filename for this export";
            //sfd.Filter = "Excel File *.xls|*.xls";
            //sfd.InitialDirectory = Core.Variables.Instance.ExportsPath;
            //sfd.ShowDialog(this);
            //string customFileName = Core.Variables.Instance.ExportsPath + "ExcelMappingsExport " + DateTime.Today.ToLongDateString() + " " + DateTime.Today.ToLongTimeString() + ".xls";//sfd.FileName;
            //if (ExportAid.CancelExport(customFileName))
            //    return "";

            util = new ExcelUtil();
            util.OpenExcel();
            util.CreateWorkbook();
            util.CurrentSheet.get_Range("A1", "A1").Value2 = classname;
            util.CurrentSheet.get_Range("B1", "B1").Value2 = childclass;
            util.CurrentSheet.get_Range("C1", "C1").Value2 = "Association Type";

            TList<ClassAssociation> assocDefinitions = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByParentClass(classname);
            int rowIndex = 2;
            try
            {
                foreach (ClassAssociation classAssociation in assocDefinitions)
                {
                    if (classAssociation.ChildClass == childclass)
                    {
                        TList<ObjectAssociation> objectAssociations = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByCAid(classAssociation.CAid);
                        foreach (ObjectAssociation objAssoc in objectAssociations)
                        {
                            if (!VCStatusTool.IsObsoleteOrMarkedForDelete(objAssoc))
                            {
                                MetaBase mbase1 = Loader.GetByID(classname, objAssoc.ObjectID, objAssoc.ObjectMachine);
                                MetaBase mbase2 = Loader.GetByID(childclass, objAssoc.ChildObjectID, objAssoc.ChildObjectMachine);

                                bool shouldExport = true;
                                if (workspace != null)
                                {
                                    bool mbaseValid1 = (mbase1.WorkspaceName == workspace.Name && mbase1.WorkspaceTypeId == workspace.WorkspaceTypeId);
                                    bool mbaseValid2 = (mbase2.WorkspaceName == workspace.Name && mbase2.WorkspaceTypeId == workspace.WorkspaceTypeId);
                                    if (!(mbaseValid1 && mbaseValid2))
                                    {
                                        shouldExport = false;
                                    }
                                }

                                if (shouldExport)
                                {
                                    util.CurrentSheet.get_Range("A" + rowIndex.ToString(), "A" + rowIndex.ToString()).Value2 = mbase1.ToString();
                                    util.CurrentSheet.get_Range("B" + rowIndex.ToString(), "B" + rowIndex.ToString()).Value2 = mbase2.ToString();
                                    util.CurrentSheet.get_Range("C" + rowIndex.ToString(), "C" + rowIndex.ToString()).Value2 = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.AssociationTypeProvider.GetBypkid(classAssociation.AssociationTypeID).Name.Trim();
                                    rowIndex++;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                util.CurrentSheet.get_Range("E" + rowIndex.ToString(), "E" + rowIndex.ToString()).Value2 = ex.ToString();
            }

            string filename = Variables.Instance.ExportsPath + "MetaBuilder Association Export for " + classname + " to " + childclass + " - All Assocation Types.xls";
            util.SaveFile(filename, true);
            util.CloseExcel();
            util = null;
            GC.Collect();
            return filename;
        }
    }
}
