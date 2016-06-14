using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Collections.ObjectModel;

namespace MetaBuilder.WinUI
{

    public class DatabaseVersioning : IDisposable
    {
        private SqlConnection _connection;
        private SqlConnection Connection { get { return _connection; } set { _connection = value; } }
        private bool Server = false;

        //string component is SQL|DESCRIPTION
        private OrderedDictionary databaseVersions;
        public DatabaseVersioning(SqlConnection connection, bool server)
        {
            Server = server;
            Connection = connection;

            //Console.WriteLine(DateTime.Now.TimeOfDay.ToString());
            databaseVersions = DatabaseVersions.BuildVersions();
            //Console.WriteLine(DateTime.Now.TimeOfDay.ToString());
        }

        //public enum ENUMCHECK
        //{
        //    x=1,
        //    aa11=2,
        //    //aa(s)=3, cant use brackets
        //    //aa-11=3, cant use dash
        //    //aa 22=3, cant use space

        //}
        public void CheckVersions(DataTable versionTable)
        {
#if DEBUG //skip updates when debugging
            //return;
#endif
            if (versionTable.Rows.Count == 0)
            {
                CreateVersion(new Guid("C86E6097-53C5-4F2A-926C-03B1817A5522")); //this guid is not in the database it is a 'null' guid
                return;
            }

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = Connection;
            cmd.CommandType = CommandType.Text;
            //89B82A2C-2A27-43c5-9FA4-CE3EFD5FF1B4 -- SAPPI 14 March 2016
            cmd.CommandText = "select count(description) from databasepatches where patchidentifier = '89B82A2C-2A27-43c5-9FA4-CE3EFD5FF1B4'";
            object o = cmd.ExecuteScalar();
            if (int.Parse(o.ToString(), CultureInfo.InvariantCulture) >= 1)
                return;

            Collection<string> versionsInDB = new Collection<string>();
            foreach (DictionaryEntry kvp in databaseVersions)
            {
                foreach (DataRow row in versionTable.Rows)
                {
                    string dbDatabaseVersion = row[0].ToString();
                    if (versionsInDB.Contains(dbDatabaseVersion))
                        continue;
                    if (kvp.Key.ToString() == dbDatabaseVersion)
                    {
                        versionsInDB.Add(kvp.Key.ToString());
                        break;
                    }
                }
            }
            foreach (DictionaryEntry kvp in databaseVersions)
            {
                if (versionsInDB.Contains(kvp.Key.ToString()))
                    continue;
                CreateVersion(new Guid(kvp.Key.ToString()));
            }
        }

        private DatabaseVersion getVersion(string rowVersion)
        {
            DatabaseVersion ver = new DatabaseVersion();
            //SQL|COMMENT
            int vari = 0;
            foreach (string s in rowVersion.Split('|'))
            {
                if (vari == 0)
                {
                    ver.Sql = s.Replace("++", "|");
                }
                else
                {
                    ver.Description = s;
                }
                vari += 1;
            }
            return ver;
        }

        //this field helps us debug while doing major dbversion updates by skipping updates from a specific id
        bool skipRest = false;
        private void CreateVersion(Guid version)
        {
            if (skipRest)
                return;
            DatabaseVersion dbVersion = null;
            if (version == new Guid("55A530DE-1584-4FCA-8D3E-BEE9F7A77E45")) //'null' guid
            {
                foreach (DictionaryEntry kvp in databaseVersions)
                {
                    dbVersion = getVersion(kvp.Value as string);
                    dbVersion.Version = new Guid(kvp.Key.ToString());
                    //if (dbVersion.Version == new Guid("2F348442-1D02-444A-8A0C-02EED25FC370"))
                    //{
                    //    skipRest = true;
                    //    break;
                    //}
                    //create in the database
                    createVersionInDB(dbVersion);
                }
            }
            //else if (version == new Guid("2F348442-1D02-444A-8A0C-02EED25FC370"))
            //{
            //    //Beginning of major metamodel
            //    skipRest = true;
            //}
            else
            {
                foreach (DictionaryEntry kvp in databaseVersions)
                {
                    if (new Guid(kvp.Key.ToString()) == version)
                    {
                        dbVersion = getVersion(kvp.Value as string);
                        dbVersion.Version = new Guid(kvp.Key.ToString());
                        //create in the database
                        createVersionInDB(dbVersion);
                        return;
                    }
                }
            }
        }
        bool Remapped = false;
        private void createVersionInDB(DatabaseVersion version)
        {
            //Update with the sql
            SqlCommand createInDbcmd = new SqlCommand();
            createInDbcmd.Connection = Connection;
            createInDbcmd.CommandType = CommandType.Text;
            createInDbcmd.CommandText = version.Sql;
            if (version.Sql != "NULL")
            {
                if (version.Sql == "REMAP") //remaps content and then rebuilds all views
                {
                    if (!Remapped)
                    {
                        try
                        {
                            DynamicDatabaseDataRemapping dddr = new DynamicDatabaseDataRemapping(Connection, Server);
                            //dddr.ToString();
                            dddr = null;
                        }
                        catch (Exception exRemap)
                        {
                            Core.Log.WriteLog("Cannot execute " + Environment.NewLine + version.Sql + Environment.NewLine + exRemap.ToString());
                            throw;
                        }
                        Remapped = true;
                    }
                }
                else if (version.Sql == "REBUILDVIEWS") //rebuilds all views
                {
                    try
                    {
                        DynamicDatabaseDataRemapping dddr = new DynamicDatabaseDataRemapping(Connection, Server, true);
                        //dddr.ToString();
                        dddr = null;
                    }
                    catch (Exception exRemap)
                    {
                        Core.Log.WriteLog("Cannot execute " + Environment.NewLine + version.Sql + Environment.NewLine + exRemap.ToString());
                        throw;
                    }
                }
                else if (version.Sql.Contains("REBUILDSINGLEVIEWS")) //rebuilds views with a string formatted   -->  -xx-yy-zz-
                {
                    try
                    {
                        DynamicDatabaseDataRemapping dddr = new DynamicDatabaseDataRemapping(Connection, Server, version.Sql.Replace("REBUILDSINGLEVIEWS", ""));
                        //dddr.ToString();
                        dddr = null;
                    }
                    catch (Exception exRemap)
                    {
                        Core.Log.WriteLog("Cannot execute " + Environment.NewLine + version.Sql + Environment.NewLine + exRemap.ToString());
                        throw;
                    }
                }
                else
                {
                    try
                    {
                        createInDbcmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        //databaseVersions.Add(new Guid(""), "delete allowedartifact where caid in (select caid from classassociation where caption is null)|Remove Duplicates");
                        //databaseVersions.Add(new Guid(""), "delete classassociation where caption is null|Remove Duplicates");
                        if (version.Version.ToString() == "1B271F7B-73B9-4A17-9307-EB875C9F0CEA" || version.Version.ToString() == "DD0FDDF4-69FC-45A6-89DB-32053CE09359" || version.Sql.Contains("REMAP"))
                        {
                            //ignore removing duplicates because they are in use but still add this version to the table afterwards
                        }
                        else
                        {
                            if (version.Sql.Contains("'PhysicalDataComponent'")) //Startup fix attempt number 2
                            {
                                try
                                {
                                    createInDbcmd.CommandText = "EXEC db_AddClasses 'PhysicalDataComponent','Name','ITInfrastructure',1";
                                    createInDbcmd.ExecuteNonQuery();

                                    createInDbcmd.CommandText = version.Sql;
                                    createInDbcmd.ExecuteNonQuery();
                                }
                                catch
                                {
                                    Core.Log.WriteLog("Attempted save --> insert physdatacomp..." + Environment.NewLine + "Cannot execute " + Environment.NewLine + version.Version.ToString() + Environment.NewLine + version.Sql);
                                    throw;
                                }
                            }
                            else
                            {
                                Core.Log.WriteLog("Cannot execute " + Environment.NewLine + version.Version.ToString() + Environment.NewLine + version.Sql);
                                throw;
                            }
                        }
                    }
                }
                //insert version into table
                try
                {
                    createInDbcmd.CommandText = "INSERT INTO DatabasePatches (PatchIdentifier,Description) VALUES ('" + version.Version + "','" + version.Description + "')";
                    createInDbcmd.ExecuteNonQuery();
                }
                catch
                {
                    //this version already exists : this wont happen because of that
                }
            }
            else if (version.Version == new Guid("75E4A9AB-3B4D-4E44-A418-DF497AACA8D9"))
            {
                try
                {
                    createInDbcmd.CommandText = "INSERT INTO DatabasePatches (PatchIdentifier,Description) VALUES ('" + version.Version + "','" + version.Description + "')";
                    createInDbcmd.ExecuteNonQuery();
                }
                catch //this will always catch
                {

                }
            }
        }

        //OLD (Condition!=null)?((Condition.Length>0)?Sequence + " ?" + Condition + " - " + Description:Sequence + "," + Description):Sequence + "," + Description
        //NEW (Condition != null && Sequence != null && Description != null) ? Condition + "," + Sequence + "," + Description : (Condition != null && Sequence != null) ? Condition + "," + Sequence : (Condition != null && Description != null) ? Condition + "," + Description : (Sequence != null && Description != null) ? Sequence + "," + Description : (Condition != null) ? Condition : (Sequence != null) ? Sequence : (Description != null) ? Description : "";
        //OLD (AssociationType!=null && Distance!=null && TimeIndicator!=null)?AssociationType + "," + Distance + "," + TimeIndicator:(AssociationType!=null&& Distance!=null)?AssociationType + "," + Distance:(AssociationType!=null&& TimeIndicator!=null)?AssociationType + "," + TimeIndicator:(Distance!=null&& TimeIndicator!=null)?Distance + "," + TimeIndicator:(AssociationType!=null)?AssociationType:(Distance!=null)?Distance:(TimeIndicator!=null)?TimeIndicator:""
        //NEW (!string.IsNullOrEmpty(AssociationType) && !string.IsNullOrEmpty(Distance) && !string.IsNullOrEmpty(TimeIndicator)) ? AssociationType + "," + Distance + "," + TimeIndicator : (!string.IsNullOrEmpty(AssociationType) && !string.IsNullOrEmpty(Distance)) ? AssociationType + "," + Distance : (!string.IsNullOrEmpty(AssociationType) && !string.IsNullOrEmpty(TimeIndicator)) ? AssociationType + "," + TimeIndicator : (!string.IsNullOrEmpty(Distance) && !string.IsNullOrEmpty(TimeIndicator)) ? Distance + "," + TimeIndicator : (!string.IsNullOrEmpty(AssociationType)) ? AssociationType : (!string.IsNullOrEmpty(Distance)) ? Distance : (!string.IsNullOrEmpty(TimeIndicator)) ? TimeIndicator : "";
        //OLD ((Description!=null && Description!=""?Description + ", ":"") + (Condition!=null && Condition!=""?Condition + ", ":"") + InferenceRule + (CohesionWeight!=null && CohesionWeight!=""?" (" + CohesionWeight + ")":"") ).Trim()
        //NEw (!string.IsNullOrEmpty(Description) && !string.IsNullOrEmpty(Condition) && !string.IsNullOrEmpty(InferenceRule+CohesionWeight))?Description + "," + Condition + "," + InferenceRule+" ("+CohesionWeight+")":(!string.IsNullOrEmpty(Description) && !string.IsNullOrEmpty(Condition))?Description + "," + Condition:(!string.IsNullOrEmpty(Description) && !string.IsNullOrEmpty(InferenceRule+CohesionWeight))?Description + "," + InferenceRule+" ("+CohesionWeight+")":(!string.IsNullOrEmpty(Condition) && !string.IsNullOrEmpty(InferenceRule+CohesionWeight))?Condition + "," + InferenceRule+" ("+CohesionWeight+")":(!string.IsNullOrEmpty(Description))?Description:(!string.IsNullOrEmpty(Condition))?Condition:(!string.IsNullOrEmpty(InferenceRule+CohesionWeight))?InferenceRule+" ("+CohesionWeight+")":""
        private void MAKESQLDESCRIPTIONCODES()
        {
            string ResourceAvailabilityRating = "";
            string ResourceAvailabilityRatingDescription = "";
            string Rating = "";
            //choose the fields you want
            string Condition = "";
            string InferenceRule = "";
            string Description = "";
            string CohesionWeight = "";

            string ConnectionType = "", ConnectionSize = "", ConnectionSpeed = "";
            //3 fields
            string w = (!string.IsNullOrEmpty(ResourceAvailabilityRating) && !string.IsNullOrEmpty(ResourceAvailabilityRatingDescription)) ? ResourceAvailabilityRating + " " + ResourceAvailabilityRatingDescription : (!string.IsNullOrEmpty(ResourceAvailabilityRating)) ? ResourceAvailabilityRating : (!string.IsNullOrEmpty(ResourceAvailabilityRatingDescription)) ? ResourceAvailabilityRatingDescription : "";
            string x = (!string.IsNullOrEmpty(Description) && !string.IsNullOrEmpty(Condition) && !string.IsNullOrEmpty(InferenceRule + CohesionWeight)) ? Description + "," + Condition + "," + InferenceRule + " (" + CohesionWeight + ")" : (!string.IsNullOrEmpty(Description) && !string.IsNullOrEmpty(Condition)) ? Description + "," + Condition : (!string.IsNullOrEmpty(Description) && !string.IsNullOrEmpty(InferenceRule + CohesionWeight)) ? Description + "," + InferenceRule + " (" + CohesionWeight + ")" : (!string.IsNullOrEmpty(Condition) && !string.IsNullOrEmpty(InferenceRule + CohesionWeight)) ? Condition + "," + InferenceRule + " (" + CohesionWeight + ")" : (!string.IsNullOrEmpty(Description)) ? Description : (!string.IsNullOrEmpty(Condition)) ? Condition : (!string.IsNullOrEmpty(InferenceRule + CohesionWeight)) ? InferenceRule + " (" + CohesionWeight + ")" : "";
            string y = !string.IsNullOrEmpty(Rating) && !string.IsNullOrEmpty(Rating) ? Rating + Environment.NewLine + Description : !string.IsNullOrEmpty(Rating) ? Rating : !string.IsNullOrEmpty(Description) ? Description : "";
            string z = (!string.IsNullOrEmpty(ConnectionType) && !string.IsNullOrEmpty(ConnectionSize) && !string.IsNullOrEmpty(ConnectionSpeed)) ? ConnectionType + " " + ConnectionSpeed + " " + ConnectionSize : (!string.IsNullOrEmpty(ConnectionType) && !string.IsNullOrEmpty(ConnectionSpeed)) ? ConnectionType + " " + ConnectionSpeed : (!string.IsNullOrEmpty(ConnectionType) && !string.IsNullOrEmpty(ConnectionSize)) ? ConnectionType + " " + ConnectionSize : (!string.IsNullOrEmpty(ConnectionSize) && !string.IsNullOrEmpty(ConnectionSpeed)) ? ConnectionSize + " " + ConnectionSpeed : (!string.IsNullOrEmpty(ConnectionType)) ? ConnectionType : (!string.IsNullOrEmpty(ConnectionSpeed)) ? ConnectionSpeed : (!string.IsNullOrEmpty(ConnectionSize)) ? ConnectionSize : "";
        }

        #region IDisposable Members

        public void Dispose()
        {
            databaseVersions.Clear();
            GC.SuppressFinalize(this);
        }

        #endregion
    }

    public class DatabaseVersion
    {
        private Guid version;
        public Guid Version { get { return version; } set { version = value; } }
        private string description;
        public string Description { get { return description; } set { description = value; } }
        private string sql;
        public string Sql { get { return sql; } set { sql = value; } }
    }

    //this classes uses the xml files created in variable through Xmler
    //it retrieves classes which have been remapped to other classes
    //it retrieves fields of classes which have been remapped to other fields
    //it retrieves associationtypes which have been remapped to other associationtypes
    public class DynamicDatabaseDataRemapping
    {
        private SqlConnection _connection;
        private SqlConnection Connection { get { return _connection; } set { _connection = value; } }

        private bool isServer;
        public bool Server
        {
            get { return isServer; }
            set { isServer = value; }
        }

        private string Provider
        {
            get
            {
                return Server ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider;
            }
        }

        public DynamicDatabaseDataRemapping(SqlConnection connection, bool server)
        {
            Server = server;
            Connection = connection;
            Remap();
        }
        public DynamicDatabaseDataRemapping(SqlConnection connection, bool server, bool views)
        {
            Server = server;
            Connection = connection;
            RebuildViews();
        }
        public DynamicDatabaseDataRemapping(SqlConnection connection, bool server, string view)
        {
            Server = server;
            Connection = connection;
            //"-class-class-class-class-class-"
            foreach (string s in view.Split('-'))
            {
                if (s.Length > 0)
                    RebuildView(s);
            }
        }

        private void Remap()
        {
            //rebuild views (this is much easier than doing it manually after compiling metamodel dlls-for clients who dont compile their models)
            Meta.Builder.ViewBuilder vBuilder = new MetaBuilder.Meta.Builder.ViewBuilder(Connection);
            foreach (Class c in DataRepository.Classes(Provider))//.Provider.ClassProvider.GetAll())
                if (c.Name != "Rationale")//skip rationale because it is unique in this regard
                    vBuilder.BuildView(c.Name);

            RemapFields();
            RemapObjects();
            //RemapClassesToFields();
        }
        private void RebuildViews()
        {
            Meta.Builder.ViewBuilder vBuilder = new MetaBuilder.Meta.Builder.ViewBuilder(Connection);
            foreach (Class c in DataRepository.Classes(Provider))//.Provider.ClassProvider.GetAll())
                //if (c.Name != "Rationale")//skip rationale because it is unique in this regard
                vBuilder.BuildView(c.Name);
        }
        private void RebuildView(string className)
        {
            Meta.Builder.ViewBuilder vBuilder = new MetaBuilder.Meta.Builder.ViewBuilder(Connection);
            vBuilder.BuildView(className);
        }
        //change saved field values to new classes
        private void RemapFields()
        {
            //retrieve fields which must be changed
            foreach (string s in Core.Variables.Instance.ClassFieldRemappedField)
            {
                string c = "";
                string oField = "";
                string nField = "";
                string nClass = "";
                int i = 0;
                foreach (string split in s.Split(':'))
                {
                    if (i == 0)
                    {
                        c = split;
                    }
                    else if (i == 1)
                    {
                        oField = split;
                    }
                    else if (i == 2)
                    {
                        nField = split;
                    }
                    else if (i == 3)
                    {
                        nClass = split;
                        //new class
                    }

                    i++;
                }
                try
                {
                    //get this fieldid
                    Field oldField = DataRepository.Connections[Provider].Provider.FieldProvider.Find("Class = '" + c + "' AND Name = '" + oField + "'")[0];
                    //get the new fieldid
                    Field newField = DataRepository.Connections[Provider].Provider.FieldProvider.Find("Class = '" + nClass + "' AND Name = '" + nField + "'")[0];
                    //update all objectfieldvalues using the oldid to use the newid
                    TList<ObjectFieldValue> currentFieldValues = DataRepository.Connections[Provider].Provider.ObjectFieldValueProvider.GetByFieldID(oldField.pkid);
                    foreach (ObjectFieldValue value in currentFieldValues)
                    {
                        value.FieldID = newField.pkid;
                        DataRepository.Connections[Provider].Provider.ObjectFieldValueProvider.Update(value);
                    }
                }
                catch
                {
                    //the old field or the new field does not exist
                }
            }

            Collection<string> updateFieldsQueries = new Collection<string>();
            //update all old class fields to new class fields
            foreach (KeyValuePair<string, string> kvp in Core.Variables.Instance.RenamedClasses)
            {
                //get all old class fields
                TList<Field> oldClassFields = DataRepository.ClientFieldsByClass(kvp.Key);
                TList<Field> newClassFields = DataRepository.ClientFieldsByClass(kvp.Value);
                //update each one to new class
                foreach (Field oldField in oldClassFields)
                {
                    foreach (Field newField in newClassFields)
                    {
                        if (newField.Name == oldField.Name && newField.DataType == oldField.DataType)
                        {
                            string q = "UPDATE ObjectFieldValue SET FieldID = " + newField.pkid + " WHERE FieldID = " + oldField.pkid;
                            if (!(updateFieldsQueries.Contains(q)))
                                updateFieldsQueries.Add(q);

                            break;
                        }
                    }
                }
            }
            foreach (string s in updateFieldsQueries)
            {
                //SqlCommand cmd = new SqlCommand(s);
                //cmd.Connection = Connection;
                //cmd.ExecuteNonQuery();
                executeSQLonProvider(s);
            }
        }

        //changes all object classes for one to another if it can
        private void RemapObjects()
        {
            TList<ObjectAssociation> ObjectAssociations = DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.GetAll();
            TList<ClassAssociation> classAssociations = DataRepository.Connections[Provider].Provider.ClassAssociationProvider.GetAll();
            foreach (KeyValuePair<string, string> kvp in Core.Variables.Instance.RenamedClasses)
            {
                //get all data by old class
                TList<MetaObject> objects = DataRepository.Connections[Provider].Provider.MetaObjectProvider.GetByClass(kvp.Key);
                //update each one to new class
                foreach (MetaObject obj in objects)
                {
                    if (DataRepository.Connections[Provider].Provider.ClassProvider.GetByName(kvp.Value) == null)
                        continue;
                    obj.Class = kvp.Value;
                    try
                    {
                        DataRepository.Connections[Provider].Provider.MetaObjectProvider.Update(obj);
                    }
                    catch (Exception ex)
                    {
                        //record errors
                        Core.Log.WriteLog("MetaObject[" + obj.pkid + ":" + obj.Machine + "] conversion failed!" + Environment.NewLine + "From " + kvp.Key + " To " + kvp.Value + Environment.NewLine + "Reason : " + ex.ToString());
                    }
                }

                List<string> missingAssociations = new List<string>();
                List<int> usedCAID = new List<int>();
                foreach (ObjectAssociation oaROOT in ObjectAssociations)
                {
                    if (usedCAID.Contains(oaROOT.CAid))
                        continue;

                    ClassAssociation caOLD = classAssociations.Find(ClassAssociationColumn.CAid, oaROOT.CAid);
                    if (caOLD == null)
                        continue;
                    string parentClassNew = remapClass(caOLD.ParentClass);
                    string childClassNew = remapClass(caOLD.ChildClass);
                    ClassAssociation caNEW = null;
                    int newCAID = 0;
                    //if either or class was changed >> Complex CAID?! 
                    string keyOrDescrip = "";
                    if (caOLD.Caption.Length > 0)
                    {
                        if (caOLD.Caption.Contains("Key ") || caOLD.Caption.Contains("Descriptive "))
                        {
                            keyOrDescrip = caOLD.Caption;
                        }
                    }
                    if (parentClassNew != caOLD.ParentClass && childClassNew != caOLD.ChildClass)
                    {
                        if (keyOrDescrip != "")
                        {
                            caNEW = DataRepository.Connections[Provider].Provider.ClassAssociationProvider.GetByParentClass(parentClassNew).FindAll(ClassAssociationColumn.ChildClass, childClassNew).FindAll(ClassAssociationColumn.AssociationTypeID, caOLD.AssociationTypeID).Find(ClassAssociationColumn.Caption, keyOrDescrip);
                        }
                        else
                        {
                            caNEW = DataRepository.Connections[Provider].Provider.ClassAssociationProvider.GetByParentClass(parentClassNew).FindAll(ClassAssociationColumn.ChildClass, childClassNew).Find(ClassAssociationColumn.AssociationTypeID, caOLD.AssociationTypeID);
                        }

                        if (caNEW != null)
                            newCAID = caNEW.CAid;
                        else
                        {
                            string s = "Association Remapping" + Environment.NewLine + "Cannot find " + parentClassNew + " --(" + caOLD.AssociationTypeID + ")--> " + childClassNew;
                            if (!(missingAssociations.Contains(s)))
                                missingAssociations.Add(s);
                        }
                    }
                    else if (parentClassNew != caOLD.ParentClass)
                    {
                        if (keyOrDescrip != "")
                        {
                            caNEW = DataRepository.Connections[Provider].Provider.ClassAssociationProvider.GetByParentClass(parentClassNew).FindAll(ClassAssociationColumn.ChildClass, childClassNew).FindAll(ClassAssociationColumn.AssociationTypeID, caOLD.AssociationTypeID).Find(ClassAssociationColumn.Caption, keyOrDescrip);
                        }
                        else
                        {
                            caNEW = DataRepository.Connections[Provider].Provider.ClassAssociationProvider.GetByParentClass(parentClassNew).FindAll(ClassAssociationColumn.ChildClass, childClassNew).Find(ClassAssociationColumn.AssociationTypeID, caOLD.AssociationTypeID);
                        }
                        if (caNEW != null)
                            newCAID = caNEW.CAid;
                        else
                        {
                            string s = "Association Remapping" + Environment.NewLine + "Cannot find " + parentClassNew + " --(" + caOLD.AssociationTypeID + ")--> " + childClassNew;
                            if (!(missingAssociations.Contains(s)))
                                missingAssociations.Add(s);
                        }
                    }
                    else if (childClassNew != caOLD.ChildClass)
                    {
                        if (keyOrDescrip != "")
                        {
                            caNEW = DataRepository.Connections[Provider].Provider.ClassAssociationProvider.GetByChildClass(childClassNew).FindAll(ClassAssociationColumn.ParentClass, parentClassNew).FindAll(ClassAssociationColumn.AssociationTypeID, caOLD.AssociationTypeID).Find(ClassAssociationColumn.Caption, keyOrDescrip);
                        }
                        else
                        {
                            caNEW = DataRepository.Connections[Provider].Provider.ClassAssociationProvider.GetByChildClass(childClassNew).FindAll(ClassAssociationColumn.ParentClass, parentClassNew).Find(ClassAssociationColumn.AssociationTypeID, caOLD.AssociationTypeID);
                        }
                        if (caNEW != null)
                            newCAID = caNEW.CAid;
                        else
                        {
                            string s = "Association Remapping" + Environment.NewLine + "Cannot find " + parentClassNew + " --(" + caOLD.AssociationTypeID + ")--> " + childClassNew;
                            if (!(missingAssociations.Contains(s)))
                                missingAssociations.Add(s);
                        }
                    }

                    if (newCAID > 0)
                    {
                        //Insert new association
                        foreach (ObjectAssociation oa in DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.GetByCAid(caOLD.CAid))
                            if (DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(newCAID, oa.ObjectID, oa.ChildObjectID, oa.ObjectMachine, oa.ChildObjectMachine) == null)
                                executeSQLonProvider("INSERT INTO ObjectAssociation (CAID,ObjectID,ObjectMachine,ChildObjectID,ChildObjectMachine,VCStatusID,VCMachineID,Machine,Series) VALUES (" + newCAID + "," + oa.ObjectID + ",'" + oa.ObjectMachine + "'," + oa.ChildObjectID + ",'" + oa.ChildObjectMachine + "'," + oa.VCStatusID + ",'" + oa.VCUser + "','" + oa.MachineName + "'," + oa.Series + ")");
                        //SWAPS CAIDS IN GraphfileAssociation
                        executeSQLonProvider("UPDATE GraphFileAssociation SET CAid = " + newCAID + " WHERE CAid = " + caOLD.CAid);
                        //SWAPS CAIDS IN Artifact
                        executeSQLonProvider("UPDATE Artifact SET CAid = " + newCAID + " WHERE CAid = " + caOLD.CAid);
                        //Delete old ObjectAssociations
                        executeSQLonProvider("DELETE FROM ObjectAssociation WHERE CAid = " + caOLD.CAid);
                    }
                }

                foreach (string s in missingAssociations) //improved logging so you dont get duplicated file :P
                    Core.Log.WriteLog(s);
            }
        }

        private static string remapClass(string c)
        {
            foreach (KeyValuePair<string, string> kvp in Core.Variables.Instance.RenamedClasses)
                if (c == kvp.Key)
                    return kvp.Value;
            return c;
        }

        private void executeSQLonProvider(string query)
        {
            try
            {
                DataRepository.Connections[Provider].Provider.ExecuteNonQuery(CommandType.Text, query);
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog("Cannot execute association remapping query" + Environment.NewLine + query + Environment.NewLine + "Reason : " + ex.ToString());
            }
        }

        //Remaps a specified class to field of another class
        ////"objectType:OLDclass:OLDfield:NEWclass:NEWfield" });
        //private void RemapClassesToFields()
        //{
        //    //find each type of this oldClass where it is used as objecttype
        //    //take the oldfield and put it on newfield of newclass

        //    foreach (string s in Core.Variables.Instance.ClassFieldRemappedField)
        //    {
        //        string objectType = "";
        //        string oClass = "";
        //        string oField = "";
        //        string nClass = "";
        //        string nField = "";
        //        int i = 0;
        //        foreach (string split in s.Split(':'))
        //        {
        //            if (i == 0)
        //            {
        //                objectType = split;
        //            }
        //            else if (i == 1)
        //            {
        //                oClass = split;
        //            }
        //            else if (i == 2)
        //            {
        //                oField = split;
        //            }
        //            else if (i == 3)
        //            {
        //                nClass = split;
        //            }
        //            else if (i == 4)
        //            {
        //                nField = split;
        //            }

        //            i++;
        //        }
        //        try
        //        {
        //            switch (objectType)
        //            {
        //                case "Artefact":
        //                    {
        //                    }
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }
        //        catch
        //        {
        //            //Something has gone horribly wrong
        //        }
        //    }
        //}
    }
}