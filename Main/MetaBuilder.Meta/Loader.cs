using System;
using System.Windows.Forms;
using MetaBuilder.Core;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using c = Microsoft.Practices.EnterpriseLibrary.Caching;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using MetaBuilder.BusinessLogic;
using System.ComponentModel;
using System.Reflection;
using TraceTool;
namespace MetaBuilder.Meta
{
    public class MetaKey
    {
        private int pkid;
        public int PKID
        {
            get { return pkid; }
            set { pkid = value; }
        }
        public string Machine
        {
            get { return machine; }
            set { machine = value; }
        }
        private string machine;
        public override string ToString()
        {
            return pkid.ToString() + machine;
        }
    }
    public class KeyObject
    {
        public KeyObject()
        {
            key = new MetaKey();
            artefacts = new List<KeyObject>();
        }

        private string className;
        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }
        private MetaKey key;
        public MetaKey Key
        {
            get { return key; }
            set { key = value; }
        }

        private List<KeyObject> artefacts;
        public List<KeyObject> Artefacts
        {
            get { return artefacts; }
            set { artefacts = value; }
        }

    }
    public class ClassMetaKey
    {
        public ClassMetaKey()
        {
            keys = new List<MetaKey>();
        }
        private string className;
        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }
        private List<MetaKey> keys;
        public List<MetaKey> Keys
        {
            get { return keys; }
            set { keys = value; }
        }
    }
    public class ChildObjectAssociationContainer
    {
        public ChildObjectAssociationContainer()
        {
            KeyObject = new KeyObject();
            associationsAndArtefacts = new Dictionary<int, List<KeyObject>>();
        }
        private KeyObject keyObject;
        public KeyObject KeyObject
        {
            get { return keyObject; }
            set { keyObject = value; }
        }

        private Dictionary<int, List<KeyObject>> associationsAndArtefacts;
        public Dictionary<int, List<KeyObject>> AssociationsAndArtefacts
        {
            get { return associationsAndArtefacts; }
            set { associationsAndArtefacts = value; }
        }
    }

    public class ObjectAssociationArtefactContainer
    {
        private List<MetaBase> loadedItems;
        public List<MetaBase> LoadedItems
        {
            get { return loadedItems; }
            set { loadedItems = value; }
        }
        private Dictionary<string, AssociationContainer> containers;
        public Dictionary<string, AssociationContainer> Containers
        {
            get
            {
                return containers;
            }
            set { containers = value; }
        }
    }
    public class AssociationContainer
    {
        public AssociationContainer()
        {
            MBParent = new MetaKey();
            childAssocs = new Dictionary<string, ChildObjectAssociationContainer>();
        }
        private MetaKey mbParent;
        public MetaKey MBParent
        {
            get { return mbParent; }
            set { mbParent = value; }
        }

        private Dictionary<string, ChildObjectAssociationContainer> childAssocs;
        public Dictionary<string, ChildObjectAssociationContainer> ChildAssocs
        {
            get { return childAssocs; }
            set { childAssocs = value; }
        }
    }

    public class Loader
    {
        #region Class Remapping

        //This takes a className and returns a new one if it exists in the renamedClasses dictionary
        private static string Remap(string className)
        {
            foreach (KeyValuePair<string, string> kvp in Core.Variables.Instance.RenamedClasses)
                if (kvp.Key == className)
                    return kvp.Value; //tell the transformer that the class was remapped

            return className;
        }
        #endregion

        #region Getters
        public static MetaBase CreateInstance(string className)
        {
            MetaBase retval = null;
            string classNameBeforeRemap = className;
            //remap this class
            className = Remap(className);
            try
            {
                retval = (MetaBase)Activator.CreateInstanceFrom(Variables.Instance.MetaAssembly, Variables.MetaNameSpace + "." + className).Unwrap();
            }
            catch (Exception ex)
            {
                if (classNameBeforeRemap != className)
                {
                    try
                    {
                        retval = (MetaBase)Activator.CreateInstanceFrom(Variables.Instance.MetaAssembly, Variables.MetaNameSpace + "." + classNameBeforeRemap).Unwrap();
                    }
                    catch (Exception dex)
                    {
                        Core.Log.WriteLog("Cannot create an instance of unRemappedClass " + classNameBeforeRemap + Environment.NewLine + dex.ToString());
                        //unable to create instance of unremappedclass ie:old class ==> This is fatal exception crash worthy
                        return null;
                    }
                }
                else
                {
                    Core.Log.WriteLog("Cannot create an instance of " + className + Environment.NewLine + ex.ToString());
                    //unable to create instance
                    //metamodel out of date
                    return null;
                }
            }
            if (retval != null)
            {
                retval.SetMachineName();
                retval._ClassName = className;
            }
            return retval;
        }

        public static BaseSorter GetSorter(string className)
        {
            return null;
            BaseSorter sorter = null;
            try
            {
                sorter = (BaseSorter)Activator.CreateInstanceFrom(Variables.Instance.MetaSortAssembly, "MetaBuilder.Meta.Generated." + className + "Sort").Unwrap();
            }
            catch
            {
            }
            return sorter;
        }

        public static void FlushDataViews()
        {
            if (Core.Variables.Instance.IsViewer)
                return;
            CacheManager cacheManager = CacheFactory.GetCacheManager("DataRetrievalCacheManager");
            cacheManager.Flush();
            CacheManager cacheManGen = CacheFactory.GetCacheManager();
            cacheManGen.Flush();
        }

        public static MetaBase GetByID(string className, int id, string machine)
        {
            return GetFromProvider(id, machine, className, false);
        }

        public static MetaBase GetFromProvider(int id, string machine, string className, bool UseServer)
        {
            CacheManager cacheManager = CacheFactory.GetCacheManager();
            //string key = id + "|" + machine;
            if (cacheManager.Contains(id + "|" + machine))
            {
                return (MetaBase)cacheManager.GetData(id + "|" + machine);
            }
            else
            {
                MetaKey mk = new MetaKey();
                mk.PKID = id;
                mk.Machine = machine;

                SqlCommand cmd = GetSqlCommand(UseServer);
                List<MetaKey> keys = new List<MetaKey>();
                keys.Add(mk);
                List<MetaBase> objs = LoadObjects(cmd, className, keys);
                if (objs.Count > 0)
                {
                    return objs[0];
                }
                return null;
            }
        }
        #endregion

        public static ObjectAssociationArtefactContainer GetAssociationContainersForMKeys(List<MetaKey> keys)
        {
            ObjectAssociationArtefactContainer retval = new ObjectAssociationArtefactContainer();
            retval.Containers = new Dictionary<string, AssociationContainer>();
            //retval.LoadedItems = new List<MetaBase>();

            SqlCommand cmd = GetSqlCommand(false);

            string[] parameters = new string[keys.Count];
            cmd.Parameters.Clear();
            SqlDataAdapter dap = new SqlDataAdapter();
            dap.SelectCommand = cmd;
            DataSet ds = new DataSet();
            String SQL = "select ca.AssociationTypeID,oa.ObjectID,oa.ObjectMachine,oa.SERIES,oa.CAID,oa.ChildObjectID, oa.ChildObjectMachine,M.Class as ObjectClass, oa.VCStatusID,";
            SQL += " oa.VCMachineID,oa.Machine,A.ArtifactObjectID,A.ARTEFACTMACHINE,MA.Class as ArtifactClass from objectassociation oa LEFT join ARTiFACT A ";
            SQL += " on oa.CAID = a.CAID and oa.ObjectID = a.ObjectID and oa.ObjectMachine = a.ObjectMachine ";
            SQL += " and oa.ChildObjectID = a.ChildObjectID and oa.ChildObjectMachine = a.ChildObjectMachine ";
            SQL += " INNER JOIN METAOBJECT M on OA.ChildObjectID = M.PKID and OA.ChildObjectMachine = M.Machine ";
            SQL += " LEFT JOIN METAOBJECT MA on A.ArtifactObjectID = MA.PKID and A.ARTEFACTMACHINE = MA.Machine ";
            SQL += " INNER JOIN CLASSASSOCIATION ca on oa.CAID = ca.CAID ";
            SQL += " WHERE (oa.Machine <> 'DB-TRIGGER' or oa.Machine is null) and CONVERT(VARCHAR,oa.ObjectID) + oa.ObjectMachine IN(";

            #region Retrieve Records
            if (keys.Count <= 2000)
            {
                #region < 2000 Keys
                for (int i = 0; i < keys.Count; i++)
                {
                    if (keys.Count > i)
                    {
                        parameters[i] = "@p" + i;
                        cmd.Parameters.AddWithValue(parameters[i], keys[i].PKID.ToString() + keys[i].Machine);

                    }
                }
                SQL += string.Join(",", parameters) + "); ";
                cmd.CommandText = SQL;
                dap.Fill(ds);
                #endregion
            }
            else
            {
                #region > 2000 keys, split into chunks
                // SQL can only accept 2100 parameters, so anytime the number of objects to load exceeds this limit, an error occurs.
                // We get around this by loading 500 objects at a time, then merging the tables in the resultset.
                // (This was a bit of a moer-of-a-job to implement)
                int tableCounter = 0;
                int lastX = 0;
                for (int x = 0; x < keys.Count + 500; x = x + 500)
                {
                    parameters = new string[500];
                    SQL = SQL = "SELECT * FROM ObjectAssociation WHERE CONVERT(VARCHAR,ObjectID) + ObjectMachine IN(";
                    cmd.Parameters.Clear();
                    for (int i = lastX; i < x; i++)
                    {
                        int p = 0;
                        p = i;
                        for (int ip = 500; ip < 10000; ip = ip + 500)
                        {
                            if (i >= ip)
                            {
                                p = i - ip;
                            }
                        }

                        if (keys.Count > i)
                        {
                            parameters[p] = "@p" + i;
                            cmd.Parameters.AddWithValue(parameters[p], keys[i].PKID.ToString() + keys[i].Machine);
                            lastX++;
                        }
                    }

                    int newparamcounter = 0;
                    string[] newparams = new string[cmd.Parameters.Count];
                    for (int o = 0; o < cmd.Parameters.Count; o++)
                    {
                        if (parameters[o] != null)
                        {
                            newparams[newparamcounter] = parameters[o];
                            newparamcounter++;
                        }
                    }
                    if (newparamcounter > 0)
                    {
                        SQL += string.Join(",", newparams) + "); ";
                        cmd.CommandText = SQL;
#if DEBUG
                        Console.WriteLine(SQL);
#endif
                        tableCounter++;
                        dap.Fill(ds, "Table" + x);
                    }

                }

                for (int z = 1; z < tableCounter; z++)
                {
                    ds.Tables[0].Merge(ds.Tables[z]);
                }
                #endregion
            }
            #endregion


            /* OUTPUT FROM DB:
            ObjectID	ObjectMachine	SERIES	CAID	ChildObjectID	ChildObjectMachine	ObjectClass	VCStatusID	VCMachineID	Machine	    ArtifactObjectID	ARTEFACTMACHINE	ArtifactClass
            302579	    DEON-550	    1	    2311	302580	        DEON-463	        Attribute	7	        NULL	    NULL	    NULL	            NULL	        NULL
            302580	    DEON-463	    1	    2312	302577	        DEON-321	        DataColumn	7	        NULL	    DB-TRIGGER	NULL	            NULL	        NULL
            302577	    DEON-321	    1	    2313	302580	        DEON-463	        Attribute	7	        NULL	    NULL	    NULL	            NULL	        NULL
            302577	    DEON-321	    1	    231x	302580	        DEON-463	        Attribute	7	        NULL	    NULL	    1	                A	            Function
            302577	    DEON-321	    1	    231x	302580	        DEON-463	        Attribute	7	        NULL	    NULL	    2	                B	            Attribute
            */
            #region Setup Relationships and Artefacts where applicable
            Dictionary<string, AssociationContainer> containerToEnsureOnlyOnePerMB = new Dictionary<string, AssociationContainer>();
            foreach (MetaKey k in keys)
            {
                if (!containerToEnsureOnlyOnePerMB.ContainsKey(k.ToString()))
                {
                    AssociationContainer container = new AssociationContainer();
                    container.ChildAssocs = new Dictionary<string, ChildObjectAssociationContainer>();
                    container.MBParent = k;

                    containerToEnsureOnlyOnePerMB.Add(k.ToString(), container);
                }
            }

            DataView dv = ds.Tables[0].DefaultView;
            DataTable dtSubset = ds.Tables[0].Copy();
            DataView dvSubset = dtSubset.DefaultView;
            Dictionary<string, List<MetaKey>> loaderKeys = new Dictionary<string, List<MetaKey>>();
            foreach (KeyValuePair<string, AssociationContainer> kvp in containerToEnsureOnlyOnePerMB)
            {
                AssociationContainer container = kvp.Value;
                //added filter here for db-trigger as well
                dv.RowFilter = "ObjectID = " + container.MBParent.PKID.ToString() + " AND ObjectMachine='" + container.MBParent.Machine + "' AND (MACHINE is null OR machine <> 'db-trigger')";// AND Machine <> 'DB-TRIGGER'
                foreach (DataRowView drvFiltered in dv)
                {
                    #region Prepare Variables
                    int ChildObjectID = int.Parse(drvFiltered["ChildObjectID"].ToString());
                    string ChildObjectMachine = drvFiltered["ChildObjectMachine"].ToString();
                    string ChildObjectClass = drvFiltered["ObjectClass"].ToString();
                    #endregion

                    MetaKey mkey = new MetaKey();
                    mkey.PKID = ChildObjectID;
                    mkey.Machine = ChildObjectMachine;
                    if (!loaderKeys.ContainsKey(ChildObjectClass))
                        loaderKeys.Add(ChildObjectClass, new List<MetaKey>());
                    bool foundKeyAssoc = false;
                    foreach (MetaKey k in loaderKeys[ChildObjectClass])
                    {
                        if (k.ToString() == mkey.ToString())
                        {
                            foundKeyAssoc = true;
                        }
                    }
                    if (!foundKeyAssoc)
                        loaderKeys[ChildObjectClass].Add(mkey);

                    #region Add Child Association Container
                    if (!container.ChildAssocs.ContainsKey(mkey.ToString()))
                    {
                        ChildObjectAssociationContainer coContainer = new ChildObjectAssociationContainer();
                        coContainer.AssociationsAndArtefacts = new Dictionary<int, List<KeyObject>>();
                        coContainer.KeyObject = new KeyObject();
                        coContainer.KeyObject.Key = new MetaKey();
                        coContainer.KeyObject.Key.PKID = ChildObjectID;
                        coContainer.KeyObject.Key.Machine = ChildObjectMachine;
                        coContainer.KeyObject.ClassName = ChildObjectClass;
                        container.ChildAssocs.Add(mkey.ToString(), coContainer);
                    }
                    #endregion

                    #region Add Associations
                    dvSubset.RowFilter = dv.RowFilter + " AND CAID=" + drvFiltered["CAID"].ToString();
                    foreach (DataRowView drvSubset in dvSubset)
                    {
                        ChildObjectAssociationContainer child = container.ChildAssocs[mkey.ToString()];
                        int caid = int.Parse(drvSubset["CAID"].ToString());
                        if (!child.AssociationsAndArtefacts.ContainsKey(caid))
                        {
                            // Console.WriteLine(caid);
                            child.AssociationsAndArtefacts.Add(caid, new List<KeyObject>());
                        }
                        KeyObject ko = new KeyObject();
                        ko.Key = new MetaKey();
                        ko.ClassName = ChildObjectClass;
                        ko.Key.PKID = ChildObjectID;
                        ko.Key.Machine = ChildObjectMachine;
                        #region Add Artefacts
                        bool found = false;
                        foreach (KeyObject koExisting in child.AssociationsAndArtefacts[caid])
                        {
                            if (koExisting.Key.PKID == ko.Key.PKID && koExisting.Key.Machine == ko.Key.Machine)
                            {
                                found = true;
                                AddArtefact(drvSubset, koExisting, loaderKeys);
                            }
                        }

                        if (!found)
                        {
                            child.AssociationsAndArtefacts[caid].Add(ko);
                            AddArtefact(drvSubset, ko, loaderKeys);

                        }
                        #endregion
                    }
                    #endregion

                }

                #region Debugging Info
#if DEBUG
                Console.WriteLine("Parent" + container.MBParent.ToString());
                foreach (KeyValuePair<string, ChildObjectAssociationContainer> kvpX in container.ChildAssocs)
                {
                    ChildObjectAssociationContainer cont = kvpX.Value;
                    foreach (KeyValuePair<int, List<KeyObject>> koKVP in cont.AssociationsAndArtefacts)
                    {

                        Console.WriteLine("CAID:" + koKVP.Key.ToString());
                        foreach (KeyObject ko in koKVP.Value)
                        {
                            Console.WriteLine("Linked to: " + ko.ClassName + ko.Key.ToString());
                            foreach (KeyObject art in ko.Artefacts)
                            {
                                Console.WriteLine("Artefact:" + art.ClassName + art.Key.ToString());
                            }
                        }

                    }
                }
#endif
                #endregion
            }
            foreach (KeyValuePair<string, AssociationContainer> kvp in containerToEnsureOnlyOnePerMB)
            {
                retval.Containers.Add(kvp.Key, kvp.Value);
            }

            #endregion
            /*
            foreach (DataRowView drv in dv)
            {
                
                MetaBase mb = CreateInstance(className);
                mb.pkid = int.Parse(drv["pkid"].ToString());
                mb.MachineName = drv["Machine"].ToString();
                TTrace.Debug.Send("Set Properties");
                SetMetaProperties(mb, drv);
                if (mb.IsInDatabase)
                    retval.Add(mb);
            }*/

            retval.LoadedItems = GetFromProvider(loaderKeys, false);

#if DEBUG
            Console.WriteLine("Loading the following items:");
            foreach (KeyValuePair<string, List<MetaKey>> kvp in loaderKeys)
            {
                Console.WriteLine(kvp.Key);
                foreach (MetaKey key in kvp.Value)
                {
                    Console.WriteLine("\t" + key.ToString());
                }
            }
            TTrace.Debug.UnIndent();
#endif
            return retval;
        }

        private static void AddArtefact(DataRowView drvSubset, KeyObject ko, Dictionary<string, List<MetaKey>> loaderKeys)
        {
            if (drvSubset["ArtifactObjectID"].ToString().Length > 0 && drvSubset["ChildObjectID"].ToString() == ko.Key.PKID.ToString() && drvSubset["ChildObjectMachine"].ToString() == ko.Key.Machine)
            {
                bool found = false;
                KeyObject art = new KeyObject();
                art.Key = new MetaKey();
                art.Key.PKID = int.Parse(drvSubset["ArtifactObjectID"].ToString());
                art.Key.Machine = drvSubset["ARTEFACTMACHINE"].ToString();
                art.ClassName = drvSubset["ArtifactClass"].ToString();

                if (!loaderKeys.ContainsKey(art.ClassName))
                    loaderKeys.Add(art.ClassName, new List<MetaKey>());

                foreach (KeyObject o in ko.Artefacts)
                {
                    bool foundKey = false;
                    foreach (MetaKey k in loaderKeys[art.ClassName])
                    {
                        if (k.ToString() == art.Key.ToString())
                        {
                            foundKey = true;
                        }
                    }
                    if (!foundKey)
                        loaderKeys[art.ClassName].Add(art.Key);


                    if (o.Key.ToString() == art.Key.ToString())
                    {
                        found = true;
                    }
                }
                if (!found)
                    ko.Artefacts.Add(art);
            }
        }
        //public static List<MetaBase> GetFromProvider(List<MetaKey> mkeys, bool useServer)
        //{
        //    return new List<MetaBase>();
        //}
        public static SqlCommand GetSqlCommand()
        {
            return GetSqlCommand(false);
        }
        public static SqlCommand GetSqlCommand(bool useServer)
        {
            SqlCommand cmd = new SqlCommand();
            string connString = (useServer) ? Variables.Instance.ServerConnectionString : Variables.Instance.ConnectionString;

            cmd.Connection = new SqlConnection(connString);
            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();
            return cmd;
        }
        //loads by class
        public static List<MetaBase> GetFromProvider(Dictionary<string, List<MetaKey>> cmkeys, bool useServer)
        {
            List<MetaBase> retval = new List<MetaBase>();
            SqlCommand cmd = GetSqlCommand(useServer);
            foreach (KeyValuePair<string, List<MetaKey>> kvp in cmkeys)
            {
                string className = kvp.Key;
                List<MetaKey> keys = kvp.Value;
                if (keys.Count > 0)
                    retval.AddRange(LoadObjects(cmd, className, keys));
            }
            cmd.Connection.Close();
            return retval;
        }
        private static List<MetaBase> LoadObjects(SqlCommand cmd, string className, List<MetaKey> keys)
        {
            List<MetaBase> retval = new List<MetaBase>();
            DataSet ds = new DataSet();
            try
            {
                string[] parameters = new string[keys.Count];
                cmd.Parameters.Clear();
                SqlDataAdapter dap = new SqlDataAdapter();
                dap.SelectCommand = cmd;
                dap.SelectCommand.CommandTimeout = 0;
                String SQL = "SELECT * FROM METAVIEW_" + className + "_LISTING WHERE CONVERT(VARCHAR,PKID) + MACHINE IN(";

                if (keys.Count <= 2000)
                {
                    for (int i = 0; i < keys.Count; i++)
                    {
                        if (keys.Count > i)
                        {
                            parameters[i] = "@p" + i;
                            cmd.Parameters.AddWithValue(parameters[i], keys[i].PKID.ToString() + keys[i].Machine);
                        }
                    }
                    SQL += string.Join(",", parameters) + "); ";
                    cmd.CommandText = SQL;
                    dap.Fill(ds);
                }
                else
                {
                    // SQL can only accept 2100 parameters, so anytime the number of objects to load exceeds this limit, an error occurs.
                    // We get around this by loading 500 objects at a time, then merging the tables in the resultset.
                    // (This was a bit of a moer-of-a-job to implement)
                    int tableCounter = 0;
                    int lastX = 0;
                    for (int x = 0; x < keys.Count + 500; x = x + 500)
                    {
                        parameters = new string[500];
                        SQL = "SELECT * FROM METAVIEW_" + className + "_LISTING WHERE CONVERT(VARCHAR,PKID) + MACHINE IN(";
                        cmd.Parameters.Clear();
                        for (int i = lastX; i < x; i++)
                        {
                            int p = 0;
                            p = i;
                            for (int ip = 500; ip < 30000; ip = ip + 500)
                            {
                                if (i >= ip)
                                {
                                    p = i - ip;
                                }
                            }

                            if (keys.Count > i)
                            {
                                parameters[p] = "@p" + i;
                                cmd.Parameters.AddWithValue(parameters[p], keys[i].PKID.ToString() + keys[i].Machine);
                                lastX++;
                            }
                        }

                        int newparamcounter = 0;
                        string[] newparams = new string[cmd.Parameters.Count];
                        for (int o = 0; o < cmd.Parameters.Count; o++)
                        {
                            if (parameters[o] != null)
                            {
                                newparams[newparamcounter] = parameters[o];
                                newparamcounter++;
                            }
                        }
                        if (newparamcounter > 0)
                        {
                            SQL += string.Join(",", newparams) + "); ";
                            cmd.CommandText = SQL;
                            tableCounter++;
                            dap.Fill(ds, "Table" + x);
                        }
                    }

                    for (int z = 1; z < tableCounter; z++)
                    {
                        ds.Tables[0].Merge(ds.Tables[z]);
                    }
                }
            }
            catch (SqlException sqlex)
            {
                MessageBox.Show("A SQL error has occurred and it has been logged.", "SQL Server Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.WriteLog("Loader.LoadObjects : SQL Exception." + Environment.NewLine + sqlex.ToString());
                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("More than 30000 " + className + " objects have been returned, overflowing the stack." + Environment.NewLine + "Some items may be missing from the list", "Too many " + className + " objects", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.WriteLog("Loader.LoadObjects : More than 30000 " + className + " have been returned, overflowing the stack." + Environment.NewLine + ex.ToString());
                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
            }

            DataView dv = ds.Tables[0].DefaultView;
            CacheManager cacheManager = CacheFactory.GetCacheManager("DataRetrievalCacheManager");
            cacheManager.Flush();
            foreach (DataRowView drv in dv)
            {
                MetaBase mb = CreateInstance(className);
                mb.pkid = int.Parse(drv["pkid"].ToString());
                mb.MachineName = drv["Machine"].ToString();

                SetMetaProperties(mb, drv, cacheManager);
                //if (mb.IsInDatabase()) //it is obviously in the database, that is where it just came from?
                retval.Add(mb);
            }
            ds.Dispose();
            dv.Dispose();
            cmd.Connection.Close();
            return retval;
        }
        private static bool IsMetaProperty(string propname)
        {
            string lower = "|" + propname.ToLower() + "|";
            string teststring = "|pkid|-none-|isindatabase|state|_classname|vcuser|vcstatusid|workspacename|workspacetypeid|machinename|embeddeditems|isindatabase";
            int index = teststring.IndexOf(lower);
            return index == -1;
        }
        public static void SetMetaProperties(MetaBase mb, DataRowView drv, CacheManager cacheManager)
        {
            if (drv != null)
            {
                mb.WorkspaceName = drv["WorkspaceName"].ToString();
                mb.WorkspaceTypeId = int.Parse(drv["WorkspaceTypeId"].ToString());
                mb.State = (VCStatusList)int.Parse(drv["VCStatusID"].ToString());
                mb.VCUser = drv["VCMachineID"].ToString();
                Type t = mb.GetType();
                foreach (PropertyInfo pinfo in t.GetProperties())
                {
                    if (IsMetaProperty(pinfo.Name))
                        SetPropertyValue(mb, pinfo, drv);
                }
                //mb.IsInDatabase = true;
            }
            else
            {
                //mb.IsInDatabase = false;
            }
            mb._IsDataLoaded = true;
            mb.OnChanged(EventArgs.Empty);

            //if (AddToCache)
            {

                if (!cacheManager.Contains(mb.pkid.ToString() + "|" + mb.MachineName))
                {
                    cacheManager.Add(mb.pkid.ToString() + "|" + mb.MachineName, mb);
                }
                //else
                //{
                //    cacheManager[key] = mb;
                //}
            }
        }
        private static void SetPropertyValue(MetaBase mb, PropertyInfo pinfo, DataRowView drvProperty)
        {
            string propname = pinfo.Name;

            if (propname.Substring(0, 1) != "_" && IsMetaProperty(propname))
            {
                if (drvProperty.DataView.Table.Columns.Contains(pinfo.Name))
                {
                    string stringval = drvProperty[pinfo.Name].ToString();
                    if (stringval == "" && pinfo.PropertyType.ToString() == "System.String")
                    {
                        pinfo.SetValue(mb, "", null);
                    }
                    if (stringval.Length > 0)
                    {
                        switch (pinfo.PropertyType.ToString())
                        {
                            case "System.String":
                                pinfo.SetValue(mb, stringval.Trim(), null);
                                break;
                            case "System.Int32":
                                pinfo.SetValue(mb, int.Parse(stringval), null);
                                break;
                            case "System.Int32?":
                                pinfo.SetValue(mb, int.Parse(stringval), null);
                                break;
                            case "System.Nullable`1[System.Int32]":
                                pinfo.SetValue(mb, int.Parse(stringval), null);
                                break;
                            case "System.Double":
                                pinfo.SetValue(mb, double.Parse(stringval, System.Globalization.CultureInfo.InvariantCulture), null);
                                break;
                            case "System.DateTime":
                                pinfo.SetValue(mb, Core.GlobalParser.ParseGlobalisedDateString(stringval), null);
                                break;
                            case "System.Boolean":
                                pinfo.SetValue(mb, bool.Parse(stringval), null);
                                break;
                            case "MetaBuilder.Meta.LongText":
                                LongText lt = new LongText();
                                lt.RTF = stringval;
                                lt.Text = stringval;
                                pinfo.SetValue(mb, lt, null);
                                break;
                            default:
                                try
                                {
                                    if (stringval.IndexOf("|") > -1)
                                    {
                                        MetaBase o = new MetaBase();
                                        char splitchar = '|';
                                        string[] keyStrings = stringval.Split(splitchar);
                                        int ObjectID = int.Parse(keyStrings[0]);
                                        string objectMachineName = keyStrings[1];
                                        o = (MetaBase)Loader.CreateInstance(pinfo.PropertyType.Name.ToString());
                                        // o.LoadFromID(ObjectID, objectMachineName, false, UseServer);
                                        pinfo.SetValue(mb, o, null);
                                    }
                                }
                                catch
                                {
                                    pinfo.SetValue(mb, stringval, null);
                                }
                                break;
                        }
                    }
                }
            }
            mb._IsDirty = true;
            // this.OnChanged(EventArgs.Empty);
        }
    }
}

//The primary difference is that in a graph database, the relationships are stored at the individual record level, while in a relational database, the structure is defined at a higher level (the table definitions).
//This has important ramifications:
//A relational database is much faster when operating on huge numbers of records. In a graph database, each record has to be examined individually during a query in order to determine the structure of the data, while this is known ahead of time in a relational database.
//Relational databases use less storage space, because they don't have to store all of those relationships.
//Storing all of the relationships at the individual-record level only makes sense if there is going to be a lot of variation in the relationships; otherwise you are just duplicating the same things over and over. This means that graph databases are well-suited to irregular, complex structures. But in the real world, most databases require regular, relatively simple structures. This is why relational databases predominate.