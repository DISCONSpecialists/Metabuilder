using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Meta;
using System.Collections.Generic;

namespace MetaBuilder.BusinessFacade.Exports
{
    /// <summary>
    /// Summary description for FSDToText.
    /// </summary>
    public class HierarchicalToText
    {
        private bool UseNumbers;
        private bool UseIndentation;
        private string machineName;

        public string MachineName
        {
            get { return machineName; }
            set { machineName = value; }
        }

        public int ObjectID
        {
            get { return objid; }
            set { objid = value; }
        }
        int objid;
        protected string FileName;

        private string fieldToExport;
        public string FieldToExport
        {
            get { return fieldToExport; }
            set { fieldToExport = value; }
        }

        private string myClass;
        public string MyClass
        {
            get { return myClass; }
            set { myClass = value; }
        }

        private bool ignoreClass //ignoring class also ignores associations
        {
            get
            {
                return true;
                //return based on class ie function ignoresclass?
                //return string.IsNullOrEmpty(MyClass);
            }
        }

        public string ReportText
        {
            get { return sBuilder.ToString(); }
        }

        private StringBuilder sBuilder;

        public HierarchicalToText()
        {
            sBuilder = new StringBuilder();
            allused = new List<string>();
        }

        Hashtable metaObjects;
        TList<MetaObject> objects;
        private int GraphFileID;

        List<string> allused = new List<string>();
        private void ExportObject(int ID, string MachineName, string Prefix, int Indentation)
        {
            if (allused.Contains(ID + " " + MachineName))
            {
                //I AM DUPLICATED NEVER ENDING CYCLE
                //Prefix = "INFINITE CYCLE    ";
                //return;
                //MetaBase mo = Loader.GetByID(MyClass, ID, MachineName);
                //AppendText(mo.ToString() + "-----" + ID + ":" + MachineName + " => DUPLICATE NEVER ENDING CYCLE");
                return;
            }
            allused.Add(ID + " " + MachineName);

            int LastNumber = 0;
            DataTable currobjAssociations = Singletons.GetAssociationHelper().GetAssociatedObjects(ID, MachineName);

            DataView dvCurrentChildren = currobjAssociations.DefaultView;
            //dvCurrentChildren.Sort = "series asc";

            if (ignoreClass) // + " and VCStatusID <> 3 and VCStatusID <> 8" Added to exclude MFD/Obsolete (Even though its supposed to occur before this method) => Jason Chladek
            {
                dvCurrentChildren.RowFilter = "VCStatusID <> 3 and VCStatusID <> 8";// and ObjectMachine=" + MachineName.ToString() + " //append graphfileassociations! //ObjectID=" + ID.ToString() + " and 
                LastNumber = NumberChildView(dvCurrentChildren, LastNumber, Prefix, Indentation + 1, 0);
            }
            else
            {
                // Export Classifications
                dvCurrentChildren.RowFilter = "ChildClass='" + MyClass + "' and AssociationTypeID=2 and ObjectID=" + ID.ToString() + " and VCStatusID <> 3 and VCStatusID <> 8";
                LastNumber = NumberChildView(dvCurrentChildren, LastNumber, Prefix, Indentation + 1, 2);

                // Export Decompositions
                dvCurrentChildren.RowFilter = "ChildClass='" + MyClass + "' and AssociationTypeID=3 and ObjectID=" + ID.ToString() + " and VCStatusID <> 3 and VCStatusID <> 8";
                LastNumber = NumberChildView(dvCurrentChildren, LastNumber, Prefix, Indentation + 1, 3);

                // Export Auxiliaries
                dvCurrentChildren.RowFilter = "ChildClass='" + MyClass + "' and AssociationTypeID=1 and ObjectID=" + ID.ToString() + " and VCStatusID <> 3 and VCStatusID <> 8";
                LastNumber = NumberChildView(dvCurrentChildren, LastNumber, Prefix, Indentation + 1, 1);
            }
        }

        private int NumberChildView(DataView dvChildren, int LastNumber, string Prefix, int Indentation, int AssociationTypeID)
        {
            string TempPrefix;
            foreach (DataRowView drvChild in dvChildren)
            {
                int rowAssociationID = int.Parse(drvChild["AssociationTypeID"].ToString());

                int id = int.Parse(drvChild["ChildObjectID"].ToString());
                string machinename = drvChild["ChildObjectMachine"].ToString();

                //skip triggers!
                if ((DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(int.Parse(drvChild["CAid"].ToString()), int.Parse(drvChild["ObjectID"].ToString()), id, drvChild["ObjectMachine"].ToString(), machinename)).Machine == "DB-TRIGGER")
                    continue;

                if (graphFileAssociations != null)
                {
                    bool found = false;
                    foreach (GraphFileAssociation g in graphFileAssociations)
                    {
                        if (g.ChildObjectID == id && g.ChildObjectMachine == machinename && g.CAid.ToString() == drvChild["CAid"].ToString())
                        {
                            //if object id
                            if ((g.ObjectID.ToString() + " " + g.ObjectMachine.ToString()) == (drvChild["ObjectID"].ToString()) + " " + drvChild["ObjectMachine"].ToString())
                                found = true;
                        }
                    }

                    if (!found)
                        continue;
                }

                //MetaBase mo = Loader.GetByID(MyClass, id, machinename); //This works great when the class is set
                MetaObject mo = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetBypkidMachine(id, machinename); //This works exactly the same without the need for the class

                if (!VCStatusTool.IsObsoleteOrMarkedForDelete(mo))
                {
                    LastNumber++;
                    TempPrefix = Prefix;
                    string Spacer = "";
                    if (Indentation == 0)
                    {
                        AppendText("\r\n");
                        if (UseNumbers)
                            TempPrefix = LastNumber.ToString() + ".";
                        else
                            TempPrefix = "";
                        Spacer = " ";
                    }
                    else if (Indentation > 0)
                    {
                        if (UseNumbers)
                        {
                            TempPrefix = Prefix + LastNumber.ToString() + ".";
                            Spacer = " ";
                        }
                        else
                            TempPrefix = Prefix;
                    }
                    else
                    {
                        TempPrefix = "";
                        Spacer = "";
                    }
                    //objects.Filter = BusinessLogic.MetaObjectColumn.Pkid.ToString() + " = " + drvChild["ChildObjectID"].ToString();
                    //dvObjectsOfThisTypeInWorkSpace.RowFilter = "pkid=" + drvChild["ChildObjectID"].ToString();

                    string Line = "";
                    if (UseIndentation)
                    {
                        for (int i = 0; i < Indentation; i++)
                        {
                            Line += "\t";
                        }
                    }
                    else
                    {
                        Line = "";
                    }

                    string auxFix = string.Empty;
                    string classFix = string.Empty;

                    //switch (AssociationTypeID)
                    //{
                    //    case 1:
                    //        classFix = "[A] ";
                    //        break;
                    //    case 2:
                    //        classFix = "[C] ";
                    //        break;
                    //    case 4:
                    //        classFix = "[M] ";
                    //        break;
                    //    case 0:
                    //use rowassociationid
                    if (rowAssociationID > 0)
                    {
                        AssociationType aType = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.AssociationTypeProvider.GetBypkid(rowAssociationID);
                        if (aType != null)
                        {
                            classFix = "[" + aType.Name.ToUpper().Trim() + "] ";
                        }
                    }
                    //        break;
                    //}
                    object mBase = metaObjects[drvChild["ChildObjectID"].ToString() + drvChild["ChildObjectMachine"].ToString().Trim()];
                    string objectString = mBase != null ? mBase.ToString().Replace("\r\n", "") : "NULL";
                    Line += TempPrefix + Spacer + classFix + objectString; //TODO : Class?
                    //TODO : This is a hack (when exporting from context ie:diagram export all objects are loaded so empty lines and ++lastnumbers are added)
                    //if (!string.IsNullOrEmpty(Line.Trim()))
                    if (objectString != null && objectString.Length > 0)
                        AppendText(Line); //Dont append lines without values
                    else
                        LastNumber--; //Decrease the lastnumber that was increased above when we dont add lines

                    ExportObject(int.Parse(drvChild["ChildObjectID"].ToString()), drvChild["ChildObjectMachine"].ToString(), TempPrefix, Indentation);
                }
            }
            if (Indentation == 0)
            {
                AppendText("\r\n");
            }

            return LastNumber;
        }
        //Added to support not setting class and also per diagram export => Jason Chladek
        private void getChildren(MetaObject parentObject, bool useGraphFile)
        {
            foreach (BusinessLogic.ObjectAssociation ass in DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(parentObject.pkid, parentObject.Machine))
            {
                if (VCStatusTool.IsObsoleteOrMarkedForDelete(ass))
                    continue;

                MetaObject tempObject = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetBypkidMachine(ass.ChildObjectID, ass.ChildObjectMachine);
                if (tempObject == null)
                    continue;

                string tempFieldToExport = "Name";
                if (useGraphFile)
                {
                    foreach (GraphFileObject gfO in DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileObjectProvider.GetByMetaObjectIDMachineID(tempObject.pkid, tempObject.Machine))
                    {
                        if (gfO.GraphFileID == GraphFileID)
                        {
                            if (tempObject.ToString() != null)
                            {
                                if (!(objects.Contains(tempObject)))
                                {
                                    MetaBase tempbase = Loader.GetByID(tempObject.Class, tempObject.pkid, tempObject.Machine);
                                    if (!VCStatusTool.IsObsoleteOrMarkedForDelete(tempObject))
                                    {
                                        if (MyClass.Length == 0)
                                        {
                                            //TList<Field> classFields = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.FieldProvider.GetByClass(tempbase.Class);
                                            //foreach (Field f in classFields)
                                            //    if (f.IsActive == true && f.SortOrder == 1)
                                            //        tempFieldToExport = f.Name;

                                            objects.Add(tempObject);
                                            metaObjects.Add(tempObject.pkid.ToString() + tempObject.Machine, tempbase.ToString()); //tempbase.Get(tempFieldToExport)
                                            getChildren(tempObject, useGraphFile);
                                        }
                                        else
                                        {
                                            if (tempbase.Class == MyClass)
                                            {
                                                objects.Add(tempObject);
                                                metaObjects.Add(tempObject.pkid.ToString() + tempObject.Machine, tempbase.Get(FieldToExport));
                                                getChildren(tempObject, useGraphFile);
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (!(objects.Contains(tempObject)))
                    {
                        MetaBase tempbase = Loader.GetByID(tempObject.Class, tempObject.pkid, tempObject.Machine);
                        if (!VCStatusTool.IsObsoleteOrMarkedForDelete(tempObject))
                        {
                            if (MyClass.Length == 0)
                            {
                                TList<Field> classFields = DataRepository.ClientFieldsByClass(tempbase.Class);
                                foreach (Field f in classFields)
                                {
                                    if (f.IsActive == true && (f.SortOrder == 0 || f.SortOrder == 1))
                                    {
                                        tempFieldToExport = f.Name;
                                        break;
                                    }
                                }

                                objects.Add(tempObject);
                                metaObjects.Add(tempObject.pkid.ToString() + tempObject.Machine, tempbase.Get(tempFieldToExport));
                                getChildren(tempObject, useGraphFile);
                            }
                            else
                            {
                                if (tempbase.Class == MyClass)
                                {
                                    objects.Add(tempObject);
                                    metaObjects.Add(tempObject.pkid.ToString() + tempObject.Machine, tempbase.Get(FieldToExport));
                                    getChildren(tempObject, useGraphFile);
                                }
                            }

                        }
                    }
                }
            }
        }
        TList<GraphFileAssociation> graphFileAssociations;
        //Added to support not setting class and also per diagram export => Jason Chladek
        public void Export(string filename, bool useNumbers, bool useIndentation, int graphFileID, string graphFileMachine)
        {
            GraphFileID = graphFileID;

            metaObjects = new Hashtable();
            objects = new TList<MetaObject>();
            MetaObject parentObject = DataRepository.MetaObjectProvider.GetBypkidMachine(ObjectID, MachineName);
            if (parentObject != null)
            {
                if (GraphFileID > 0)
                    graphFileAssociations = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileAssociationProvider.GetByGraphFileIDGraphFileMachine(graphFileID, graphFileMachine);

                MetaBase mbase = null;
                if (ignoreClass)
                    mbase = Loader.GetByID(parentObject.Class, parentObject.pkid, parentObject.Machine);
                else
                    mbase = Loader.GetByID(MyClass, parentObject.pkid, parentObject.Machine);

                if (mbase == null)
                    return;

                UseNumbers = useNumbers;
                UseIndentation = useIndentation;

                objects.Add(parentObject);
                getChildren(parentObject, GraphFileID > 0);

                if (FieldToExport == null)
                    FieldToExport = "Name";
                AppendText(mbase.Get(FieldToExport).ToString().Replace("\r\n", ""));
                ExportObject(mbase.pkid, mbase.MachineName, "", 0);
                SaveSelected(filename);
            }
        }

        public void AppendText(string Line)
        {
            sBuilder.Append(Line).Append("\r\n");
        }

        public virtual void Save(string filename)
        {
            OnExportCompleted(EventArgs.Empty);

            string FileName = Variables.Instance.ExportsPath + filename + ".txt";
            StreamWriter writer = new StreamWriter(FileName);
            writer.Write(this.ReportText);
            writer.Close();
        }
        public virtual void SaveSelected(string filename)
        {
            OnExportCompleted(EventArgs.Empty);

            StreamWriter writer;
            if (!(System.IO.Directory.Exists(Core.strings.GetPath(filename))))
            {
                //string FileName = Variables.Instance.ExportsPath + filename + ".txt";
                writer = new StreamWriter(Variables.Instance.ExportsPath + filename + ".txt");
                writer.Write(this.ReportText);
                writer.Close();
            }
            else
            {
                writer = new StreamWriter(filename);
                writer.Write(this.ReportText);
                writer.Close();
            }
        }

        public event EventHandler ExportCompleted;

        public virtual void OnExportCompleted(EventArgs e)
        {
            if (ExportCompleted != null)
            {
                ExportCompleted(this, e);
            }
        }

    }
}

//Database export Original
//public void Export(string filename, bool useNumbers, bool useIndentation)
//{
//    metaObjects = new Hashtable();
//    UseNumbers = useNumbers;
//    UseIndentation = useIndentation;
//    // Get the items
//    objects = DataRepository.MetaObjectProvider.GetByClass(MyClass);
//    foreach (MetaObject obj in objects)
//    {
//        MetaBase tempbase = Loader.GetByID(MyClass, obj.pkid, obj.Machine);
//        if (!VCStatusTool.IsObsoleteOrMarkedForDelete(tempbase))
//        {
//            if (tempbase.ToString() != null)
//                metaObjects.Add(obj.pkid.ToString() + obj.Machine, tempbase.Get(FieldToExport));
//        }
//    }
//    // Get the parent object
//    MetaObject mParent = DataRepository.MetaObjectProvider.GetBypkidMachine(ObjectID, MachineName);
//    //    new MetaObject();
//    //foreach (MetaObject obj in objects)
//    //{
//    //    if (obj.pkid == objid)
//    //    {
//    //        mParent = obj;
//    //        break;
//    //    }
//    //}
//    if (mParent != null)
//    {
//        //NumberChildView(dvCurrentChildren, LastNumber, Prefix, Indentation + 1, 2);
//        MetaBase mbase = Loader.GetByID(MyClass, mParent.pkid, mParent.Machine);
//        AppendText(mbase.Get(FieldToExport).ToString());
//        ExportObject(mbase.pkid, mbase.MachineName, "", 0);
//        Save(filename);
//    }
//}
//Diagram Export => Jason chladek (using existing)
//public void Export(string filename, bool useNumbers, bool useIndentation, int graphFileID)
//{
//    GraphFileID = graphFileID;

//    metaObjects = new Hashtable();
//    UseNumbers = useNumbers;
//    UseIndentation = useIndentation;
//    //Get the items (LOL GET EVERY ITEM FOR THIS CLASS? REALLY)
//    objects = DataRepository.MetaObjectProvider.GetByClass(MyClass);
//    foreach (MetaObject obj in objects)
//    {
//        MetaBase tempbase = Loader.GetByID(MyClass, obj.pkid, obj.Machine);
//        if (!VCStatusTool.IsObsoleteOrMarkedForDelete(tempbase))
//        {
//            foreach (GraphFileObject gfO in DataRepository.GraphFileObjectProvider.GetByMetaObjectIDMachineID(obj.pkid, obj.Machine))
//            {
//                if (gfO.GraphFileID == graphFileID)
//                {
//                    if (tempbase.ToString() != null)
//                    {
//                        metaObjects.Add(obj.pkid.ToString() + obj.Machine, tempbase.Get(FieldToExport));
//                        break;
//                    };
//                }
//            }
//        }
//    }
//    // Get the parent object
//    MetaObject mParent = DataRepository.MetaObjectProvider.GetBypkidMachine(ObjectID, MachineName);
//    //    new MetaObject();
//    //foreach (MetaObject obj in objects)
//    //{
//    //    if (obj.pkid == objid)
//    //    {
//    //        mParent = obj;
//    //        break;
//    //    }
//    //}

//    if (mParent != null)
//    {
//        //NumberChildView(dvCurrentChildren, LastNumber, Prefix, Indentation + 1, 2);
//        MetaBase mbase = Loader.GetByID(MyClass, mParent.pkid, mParent.Machine);
//        AppendText(mbase.Get(FieldToExport).ToString());
//        ExportObject(mbase.pkid, mbase.MachineName, "", 0);
//        SaveSelected(filename);
//    }
//}