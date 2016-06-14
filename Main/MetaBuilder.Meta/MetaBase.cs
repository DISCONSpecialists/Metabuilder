#region Copyright © 2007 - DISCON Specialists

//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: MetaBase.cs
//

#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using c = Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Collections.ObjectModel;

namespace MetaBuilder.Meta
{
    //public enum ZoomQuality
    //{
    //    Max,
    //    Med,
    //    Low
    //}

    [Serializable]
    public class MetaBase : IMetaBase, IRepositoryItem, IWorkspaceItem
    {
        [Browsable(false)]
        //[CLSCompliant(false)]
        private Type forceShapeType;
        [Browsable(false)]
        [CLSCompliant(false)]
        public Type ForceShapeType //this is experimental
        {
            get { return forceShapeType; }
            set { forceShapeType = value; }
        }

        [Browsable(false)]
        [CLSCompliant(false)]
        public object tag;

        [Browsable(false)]
        [CLSCompliant(false)]
        public bool _IsDirty;
        [Browsable(false)]
        //[field: NonSerialized]
        public bool IsChangedNull()
        {
            return Changed == null;
        }
        public event EventHandler Changed;
        [Browsable(false)]
        public virtual void OnChanged(EventArgs e)
        {
            if (Changed != null)
            {
                Changed(this, e);
                IsSaved = false;
                BrowsableProperties();
                if (Core.Variables.Instance.SaveOnCreate)
                    Save(Guid.NewGuid());
            }
        }

        public void BrowsableProperties()
        {
            foreach (PropertyInfo info in this.GetType().GetProperties())
            {
                bool browsable = false;

                PropertyDescriptor descriptor = TypeDescriptor.GetProperties(this.GetType())[info.Name];
                CategoryAttribute catattrib = (CategoryAttribute)descriptor.Attributes[typeof(CategoryAttribute)];
                if (catattrib.Category == "General" || catattrib.Category == "System Properties")
                {
                    browsable = true;
                }
                else
                {
                    if (TypeField != null && TypeField.Length > 0)
                    {
                        ObsoleteAttribute catDattrib = (ObsoleteAttribute)descriptor.Attributes[typeof(ObsoleteAttribute)];
                        if (catDattrib != null)
                        {
                            if (catDattrib.Message.Contains("|" + TypeField + "|"))
                            {
                                browsable = true;
                            }
                            else
                            {
                                //SetWithoutChange(pinfo.Name, "");
                            }
                        }
                    }
                }

                BrowsableAttribute attrib = (BrowsableAttribute)descriptor.Attributes[typeof(BrowsableAttribute)];
                FieldInfo isBrow = attrib.GetType().GetField("browsable", BindingFlags.NonPublic | BindingFlags.Instance);
                isBrow.SetValue(attrib, browsable);
            }
        }

        //[DescriptionAttribute("MetaObject PKID"), CategoryAttribute("System Properties")]
        //[Browsable(true)]
        //[ReadOnly(true)]
        //public int PKID
        //{
        //    get
        //    {
        //        return this.pkid;
        //    }
        //}

        [DescriptionAttribute("MetaObject Machine"), CategoryAttribute("System Properties")]
        [Browsable(true)]
        [ReadOnly(true)]
        public string MachineName
        {
            get
            {
                return this.machineName;
            }
            set
            {
                machineName = value;
            }
        }

        //[DescriptionAttribute("MetaObject Workspace"), CategoryAttribute("System Properties")]
        //[Browsable(true)]
        //[ReadOnly(true)]
        //public string Workspace
        //{
        //    get
        //    {
        //        return this.WorkspaceName;
        //    }
        //}

        [DescriptionAttribute("MetaObject Class"), CategoryAttribute("System Properties")]
        [Browsable(true)]
        [ReadOnly(true)]
        public string Class
        {
            get
            {
                return this._ClassName;
            }
        }

        [DescriptionAttribute("MetaObject PKID"), CategoryAttribute("System Properties")]
        [Browsable(true)]
        [ReadOnly(true)]
        //[Browsable(false)]
        public int pkid
        {
            get { return this._pkid; }
            set { _pkid = value; }
        }

        [Browsable(false)]
        private string typeField;

        [RefreshProperties(RefreshProperties.Repaint)]
        [Browsable(false)]
        public string TypeField
        {
            get { return typeField; }
            set { typeField = value; }
        }

        //[Browsable(false)]
        public VCStatusList state;
        [Browsable(false)]
        public VCStatusList State
        {
            get { return state; }
            set { state = value; }
        }

        [Browsable(false)]
        private int workspaceTypeId;
        [Browsable(false)]
        public int WorkspaceTypeId
        {
            get { return workspaceTypeId; }
            set { workspaceTypeId = value; }
        }
        [Browsable(false)]
        private string workspaceName;

        [DescriptionAttribute("MetaObject Workspace"), CategoryAttribute("System Properties")]
        [Browsable(true)]
        [ReadOnly(true)]
        //[Browsable(false)]
        public string WorkspaceName
        {
            get { return workspaceName; }
            set { workspaceName = value; }
        }

        [Browsable(false)]
        private string vcUser;
        [Browsable(false)]
        public string VCUser
        {
            get { return vcUser; }
            set { vcUser = value; }
        }

        public void Reset()
        {
            pkid = 0;
            //_pkid = 0;
            MachineName = null;
            this.state = VCStatusList.None;
            this.vcUser = "";
            //Reset workspace
            WorkspaceName = Core.Variables.Instance.CurrentWorkspaceName;
            WorkspaceTypeId = Core.Variables.Instance.CurrentWorkspaceTypeId;
        }

        /*
        public void GetObjectData (SerializationInfo info, StreamingContext ctxt) {  // Serialization method
            SerializationWriter sw = SerializationWriter.GetWriter ();                 // Get a Writer
            sw.Write(this.pkid);
            Type t = this.GetType();
            foreach (FieldInfo pinfo in t.GetFields())
            {
                switch (pinfo.FieldType.ToString())
                {
                    case "System.String":
                        sw.Write((string)pinfo.GetValue(this));
                        break;
                    case "System.Int32":
                        sw.Write((Int32)pinfo.GetValue(this));
                        break;
                    case "System.Nullable`1[System.Int32]":
                        sw.WriteObject((object)pinfo.GetValue(this));
                        break;
                    case "System.Double":
                        sw.Write((double)pinfo.GetValue(this));
                        break;
                    case "System.DateTime":
                        sw.Write((DateTime)pinfo.GetValue(this));
                        break;
                    case "System.Boolean":
                        sw.Write((bool)pinfo.GetValue(this));
                        break;
                    case "Meta.LongText":
                        sw.Write((string)pinfo.GetValue(this));
                        break;
                    default:
                        sw.WriteObject((object)pinfo.GetValue(this));
                        break;
                }
            }
            sw.AddToInfo(info);
        }*/
        /*
        public MetaBase (SerializationInfo info, StreamingContext ctxt) {          // Deserialization .ctor
            SerializationReader sr = SerializationReader.GetReader (info);             // Get a Reader from info
            pkid = sr.ReadInt32();
            Type t = this.GetType();
            foreach (FieldInfo pinfo in t.GetFields())
            {
                switch (pinfo.FieldType.ToString())
				{
					case "System.String":
						pinfo.SetValue(this, sr.ReadString());
						break;
					case "System.Int32":
						pinfo.SetValue(this, sr.ReadInt32());
						break;
                    case "System.Nullable`1[System.Int32]":
                            pinfo.SetValue(this, sr.ReadObject());
                        break;
					case "System.Double":
						pinfo.SetValue(this, sr.ReadDouble());
						break;
					case "System.DateTime":
						pinfo.SetValue(this, sr.ReadDateTime());
						break;
					case "System.Boolean":
						pinfo.SetValue(this, sr.ReadBoolean());
						break;
					case "Meta.LongText":
						LongText lt = new LongText();
						lt.RTF = sr.ReadString();
						pinfo.SetValue(this, lt);
						break;
					default:
                        pinfo.SetValue(this, sr.ReadObject());
						break;
				}
            }

        }*/

        [Browsable(false)]
        private string machineName;

        [Browsable(false)]
        private int _pkid;

        [Browsable(false)]
        public bool _IsDataLoaded;

        public void LoadEmbeddedObjects(int PropertiesLoadedLevel, int PropertiesTargetLevel)
        {
            if (PropertiesLoadedLevel <= PropertiesTargetLevel)
            {
                PropertiesLoadedLevel++;
                Type baseType = this.GetType();
                Type propType;
                foreach (PropertyInfo propinfo in baseType.GetProperties())
                {
                    propType = propinfo.PropertyType;
                    if (propType.IsSubclassOf(typeof(MetaBase)))
                    {
                        MetaBase childObject = (MetaBase)propinfo.GetValue(this, null);
                        if (childObject != null)
                        {
                            if (!childObject._IsDataLoaded)
                            {
                                //childObject.LoadFromID(childObject.pkid,childObject.MachineName,false,false);
                            }
                            childObject.LoadEmbeddedObjects(PropertiesLoadedLevel, PropertiesTargetLevel);
                        }
                    }
                }
            }
        }

        public void PropertyChanged()
        {
            OnChanged(new EventArgs());
        }

        public MetaBase()
        {
            SetMachineName();
        }

        public void SetMachineName()
        {
            this.State = VCStatusList.None;
            Random r = new Random();
            //int rN = ;
            //System.Threading.Thread.Sleep(5);
            MachineName = Environment.MachineName + "-" + r.Next(0, 999).ToString();
            // = Core.Variables.Instance.UserID;
        }

        [Browsable(false)]
        public string _ClassName;

        /*
		private DataRowView GetFieldsDataRowView()
		{
            CacheManager cacheManager = CacheFactory.GetCacheManager("DataRetrievalCacheManager");
            bool isCached = false;
            DataView dvCached;
            DataRowView drvFiltered;
            if (cacheManager.Contains(_ClassName))
            {
                isCached = true;
                dvCached = (DataView)cacheManager.GetData(_ClassName);
                drvFiltered = GetFilteredRowView(dvCached);
                if (drvFiltered != null)
                    return drvFiltered;
                else
                {
                    // retrieve it manually (as usual)
                }
            }

            string cmdtext = "select top " + Variables.Instance.NumberOfObjectRowsToCache.ToString() + " * from METAView_" + _ClassName + "_Retrieval";
            if (isCached)
                cmdtext += " where pkid = " + pkid.ToString() + " and MACHINE = '" + machineName + "'";

            dvCached = GetFromDataBase(cmdtext);
    
            if (!isCached)
                AddMetaObjectDataViewFromCache(_ClassName, dvCached);
            
            dvCached.RowFilter = "";
            if (dvCached.Count == 1)
            {
                drvFiltered = GetFilteredRowView(dvCached);

            }
            else
            {
                if (!cmdtext.Contains("where pkid"))
                    cmdtext += " where pkid = " + pkid.ToString() + " and MACHINE = '" + this.MachineName + "'";
                DataView dv = GetFromDataBase(cmdtext);
                drvFiltered = GetFilteredRowView( dv);
            }
            return drvFiltered;
            //return return GetFromDataBase(MACHINENAME, cmdtext, out dvCached);
		}

        private DataView GetFromDataBase(string cmdtext)
        {
            string connString = (UseServer)
                                    ? Variables.Instance.ServerConnectionString
                                    : Variables.Instance.ConnectionString;

            SqlCommand cmd = new SqlCommand(cmdtext, new SqlConnection(connString));
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            SqlDataAdapter dap = new SqlDataAdapter();
            dap.SelectCommand = cmd;
            dap.Fill(ds, "Item");
            return ds.Tables["Item"].DefaultView;
           
        }
       

        public void AddMetaObjectDataViewFromCache(string className, DataView dv)
        {
            CacheManager cacheManager = CacheFactory.GetCacheManager("DataRetrievalCacheManager");
            if (!cacheManager.Contains(className))
                cacheManager.Add(className, dv);
        }

        private DataRowView GetFilteredRowView(DataView dvCached)
        {
            dvCached.RowFilter = "pkid = " + pkid.ToString() + " and Machine ='" + machineName + "'";
            if (dvCached.Count > 0)
            {
                return dvCached[0];
            }
            return null;
        }
*/
        private DataView GetFieldDefinitionsDataView(bool ActiveOnly, string connString)
        {
            CacheManager cacheMan = CacheFactory.GetCacheManager();
            string fieldDefKey = "FIELDDEFINITIONS:" + _ClassName + connString;
            if (cacheMan.Contains(fieldDefKey))
            {
                return (DataView)cacheMan.GetData(fieldDefKey);
            }
            else
            {
                SqlCommand cmd = new SqlCommand("META_GetFieldDefinitions", new SqlConnection(connString));
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ClassName", SqlDbType.VarChar, 50));
                cmd.Parameters["@ClassName"].Value = _ClassName;

                if (ActiveOnly)
                {
                    cmd.Parameters.Add(new SqlParameter("@ActiveOnly", SqlDbType.Bit));
                    cmd.Parameters["@ActiveOnly"].Value = 1;
                }

                DataSet ds = new DataSet();
                SqlDataAdapter dap = new SqlDataAdapter();
                dap.SelectCommand = cmd;
                dap.Fill(ds, "FieldDefs");
                cacheMan.Add(fieldDefKey, ds.Tables["FieldDefs"].DefaultView);
                return ds.Tables["FieldDefs"].DefaultView;
            }
        }

        private Collection<string> ClassFieldRemappedFields
        {
            get
            {
                Collection<string> r = new Collection<string>();
                foreach (string s in Variables.Instance.ClassFieldRemappedField)
                {
                    string c = "";
                    string oF = "";
                    string nF = "";
                    int i = 0;
                    try
                    {
                        foreach (string split in s.Split(':'))
                        {
                            if (i == 0)
                            {
                                c = split;
                            }
                            else if (i == 1)
                            {
                                oF = split;
                            }
                            else if (i == 2)
                            {
                                nF = split;
                            }
                            else
                            {
                                //new class name. not required here. new class was loaded :)
                            }
                            i++;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (Core.Variables.Instance.UserDebug)
                            Log.WriteLog(ex.ToString());
                    }
                    if (c == Class)
                    {
                        r.Add(s);
                    }
                }
                return r;
            }
        }

        [Browsable(false)]
        public bool IgnoreRemapping = false;
        public void Set(string PropertyName, object val)
        {

            #region Remap Fields
            if (!IgnoreRemapping)
                foreach (string s in ClassFieldRemappedFields)
                {
                    string c = "";
                    string oldField = "";
                    string newField = "";
                    int i = 0;
                    foreach (string split in s.Split(':'))
                    {
                        if (i == 0)
                        {
                            c = split;
                        }
                        else if (i == 1)
                        {
                            oldField = split;
                        }
                        else if (i == 2)
                        {
                            newField = split;
                        }
                        else if (i == 3)
                        {
                            //newClass=split;
                        }

                        i++;
                    }
                    if (PropertyName.ToLower() == oldField.ToLower())
                    {
                        PropertyName = newField;
                        break;
                    }
                }

            #endregion

            // try
            //  {
            if (IsValidName(PropertyName))
            {
                PropertyInfo info = this.GetType().GetProperty(PropertyName);
                if (info != null)
                {
                    if (info.PropertyType.ToString() == "System.Int32" && (val.ToString().Length > 0)) //&& IsValidName(PropertyName)?checked above
                    {
                        int intval = 0;
                        bool isint = int.TryParse(val.ToString(), out intval);
                        if (isint)
                            info.SetValue(this, intval, null);
                    }
                    else if (info.PropertyType.ToString() == "System.Nullable`1[System.Int32]")
                    {
                        if (val.ToString().Length > 0)
                        {
                            int value = 0;
                            bool worked = int.TryParse(val.ToString(), out value);
                            if (worked)
                                info.SetValue(this, value, null);
                            else
                                info.SetValue(this, null, null);
                        }
                        else
                            info.SetValue(this, null, null);
                    }
                    else
                    {
                        if (val == null)
                            info.SetValue(this, null, null);
                        else
                        {
                            if (info.PropertyType == val.GetType())
                            {
                                if (info.CanWrite)
                                {
                                    if (info.GetValue(this, null) != val)
                                        info.SetValue(this, val, null);
                                }
                            }
                        }
                        if (info.PropertyType.Name == "LongText")
                        {
                            if (info.CanWrite)
                            {
                                TypeConverter conv = TypeDescriptor.GetConverter(info.PropertyType);
                                if (conv.CanConvertFrom(typeof(string)))
                                {
                                    if (val != null)
                                    {
                                        object valueConverted = conv.ConvertFrom(val.ToString());
                                        info.SetValue(this, valueConverted, null);
                                    }
                                };

                                //this.Set(info.Name, val.ToString());
                            }
                        }
                    }
                }
            }
            //   }
            //  catch { }
            if (PropertyName != "GapType") //Fields to not look at
            {
                if (PropertyName.Contains("Type")) //Other fields to look at
                    TypeField = val.ToString();
            }
            OnChanged(EventArgs.Empty);
        }

        public void SetWithoutChange(string PropertyName, object val)
        {

            #region Remap Fields
            if (!IgnoreRemapping)
                foreach (string s in ClassFieldRemappedFields)
                {
                    string c = "";
                    string oldField = "";
                    string newField = "";
                    int i = 0;
                    foreach (string split in s.Split(':'))
                    {
                        if (i == 0)
                        {
                            c = split;
                        }
                        else if (i == 1)
                        {
                            oldField = split;
                        }
                        else if (i == 2)
                        {
                            newField = split;
                        }
                        else if (i == 3)
                        {
                            //newClass=split;
                        }

                        i++;
                    }
                    if (PropertyName.ToLower() == oldField.ToLower())
                    {
                        PropertyName = newField;
                        break;
                    }
                }

            #endregion

            // try
            //  {
            if (IsValidName(PropertyName))
            {
                PropertyInfo info = this.GetType().GetProperty(PropertyName);
                if (info != null)
                {
                    if (info.PropertyType.ToString() == "System.Int32" && (val.ToString().Length > 0)) //&& IsValidName(PropertyName)?checked above
                    {
                        int intval = 0;
                        bool isint = int.TryParse(val.ToString(), out intval);
                        if (isint)
                            info.SetValue(this, intval, null);
                    }
                    else if (info.PropertyType.ToString() == "System.Nullable`1[System.Int32]")
                    {
                        if (val.ToString().Length > 0)
                        {
                            int value = 0;
                            bool worked = int.TryParse(val.ToString(), out value);
                            if (worked)
                                info.SetValue(this, value, null);
                            else
                                info.SetValue(this, null, null);
                        }
                        else
                            info.SetValue(this, null, null);
                    }
                    else
                    {
                        if (val == null)
                            info.SetValue(this, null, null);
                        else
                        {
                            if (info.PropertyType == val.GetType())
                            {
                                if (info.CanWrite)
                                {
                                    if (info.GetValue(this, null) != val)
                                        info.SetValue(this, val, null);
                                }
                            }
                        }
                        if (info.PropertyType.Name == "LongText")
                        {
                            if (info.CanWrite)
                            {
                                TypeConverter conv = TypeDescriptor.GetConverter(info.PropertyType);
                                if (conv.CanConvertFrom(typeof(string)))
                                {
                                    if (val != null)
                                    {
                                        object valueConverted = conv.ConvertFrom(val.ToString());
                                        info.SetValue(this, valueConverted, null);
                                    }
                                };

                                //this.Set(info.Name, val.ToString());
                            }
                        }
                    }
                }

            }
            //   }
            //  catch { }
            //OnChanged(EventArgs.Empty);
        }

        public object Get(string PropertyName)
        {
            if (this.GetType().GetProperty(PropertyName) != null)
                return this.GetType().GetProperty(PropertyName).GetValue(this, null);
            return null;
        }

        private bool IsValidName(string propname)
        {
            string lower = "|" + propname.ToLower() + "|";
            string teststring = "|pkid|-none-|isindatabase|state|_classname|vcuser|vcstatusid|workspacename|workspacetypeid|machinename|embeddeditems|isindatabase|forceshapetype|class|workspace|typefield|issaved|";
            int index = teststring.IndexOf(lower);
            return index == -1;
            //bool isvalid = lower != "isindatabase" && lower != "pkid" && lower != "state" && lower != "_classname" && lower != "vcuser" && lower != "VCStatusid" && lower != "workspacename" && lower != "WorkspaceTypeId" && lower != "machinename" && lower != "embeddeditems";
        }
        [Browsable(false)]
        [NonSerialized]
        private bool isSaved;
        [Browsable(false)]
        public bool IsSaved
        {
            get { return isSaved; }
            set { isSaved = value; }
        }

        //[Browsable(false)]
        //private bool isInDatabase;
        [Browsable(false)]
        public bool IsInDatabase(string ProviderName)
        {
            //get
            {
                if (pkid <= 0)
                    return false;
                try
                {
                    string connString = (ProviderName == Core.Variables.Instance.ClientProvider) ? Variables.Instance.ConnectionString : Variables.Instance.ServerConnectionString;
                    SqlCommand cmd = new SqlCommand("Select Count(*) From MetaObject WHERE pkid=" + pkid + " AND Machine = '" + MachineName + "'", new SqlConnection(connString));
                    using (cmd.Connection)
                    {
                        cmd.Connection.Open();
                        return int.Parse(cmd.ExecuteScalar().ToString()) > 0;
                    }
                }
                catch
                {
                }
                return false;
                //return isInDatabase;
            }
            //set { isInDatabase = value; }
        }

        public Collection<PropertyInfo> GetMetaPropertyList(bool includeDesignerProps)
        {
            Collection<PropertyInfo> retval = new Collection<PropertyInfo>();
            foreach (PropertyInfo pinfo in this.GetType().GetProperties())
            {
                if (!includeDesignerProps)
                {
                    if (IsValidName(pinfo.Name))
                        retval.Add(pinfo);
                }
                else
                {
                    retval.Add(pinfo);
                }
            }

            return retval;
        }

        public void SaveToRepository(Guid uniqueid, string ProviderName)
        {
            if (ProviderName == Core.Variables.Instance.ClientProvider)
                if (IsSaved)
                    return;
            string connString = (ProviderName == Core.Variables.Instance.ClientProvider) ? Variables.Instance.ConnectionString : Variables.Instance.ServerConnectionString;

            SqlCommand cmd = new SqlCommand("META_UpdateObjectFieldValue", new SqlConnection(connString));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@ObjectID", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@MachineName", SqlDbType.VarChar, 50));
            cmd.Parameters.Add(new SqlParameter("@FieldID", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@ValueString", SqlDbType.VarChar, 900));
            cmd.Parameters.Add(new SqlParameter("@ValueInt", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@ValueDouble", SqlDbType.Float));
            cmd.Parameters.Add(new SqlParameter("@ValueBoolean", SqlDbType.Bit));
            cmd.Parameters.Add(new SqlParameter("@ValueDate", SqlDbType.DateTime));
            cmd.Parameters.Add(new SqlParameter("@ValueLongText", SqlDbType.Text));
            cmd.Parameters.Add(new SqlParameter("@ValueObjectID", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@ValueRTF", SqlDbType.Text));

            if (pkid == 0 || this.MachineName == "") // Never Saved
            {
                SetMachineName();
            }

            if (!IsInDatabase(ProviderName))
                CreateObjectInDB(connString, ProviderName);

            //if (!IsInDatabase(ProviderName))
            // {
            //     Log.WriteLog(this.ToString() + " CreateObjectInDB returned false");
            //     return;
            // }

            cmd.Parameters["@ObjectID"].Value = this.pkid;
            cmd.Parameters["@MachineName"].Value = this.MachineName;

            DataView dvFields = this.GetFieldDefinitionsDataView(true, connString);
            string TargetField = string.Empty;

            #region Fields
            using (cmd.Connection)
            {
                try
                {
                    cmd.Connection.Open();
                    foreach (DataRowView drvF in dvFields)
                    {
                        bool skip = false;
                        cmd.Parameters["@FieldID"].Value = int.Parse(drvF["pkid"].ToString());
                        PropertyInfo pinfo = this.GetType().GetProperty(drvF["Name"].ToString());
                        if (pinfo != null && IsValidName(pinfo.Name))
                        {
                            object o = pinfo.GetValue(this, null);

                            if (o != null)// && !(string.IsNullOrEmpty(o.ToString())) 
                            {
                                cmd.Parameters["@ValueString"].Value = null;
                                cmd.Parameters["@ValueInt"].Value = null;
                                cmd.Parameters["@ValueBoolean"].Value = null;
                                cmd.Parameters["@ValueDate"].Value = null;
                                cmd.Parameters["@ValueLongText"].Value = null;
                                cmd.Parameters["@ValueDouble"].Value = null;
                                cmd.Parameters["@ValueRTF"].Value = null;

                                switch (drvF["DataType"].ToString())
                                {
                                    case "System.String":
                                        if (this.Class == "Rationale")
                                        {
                                            //if (o.ToString().Length >= 255)
                                            //{
                                            //    LongText ltS = new LongText();
                                            //    ltS.Text = o.ToString();
                                            //    ltS.RTF = o.ToString();
                                            //    cmd.Parameters["@ValueRTF"].Value = ltS.RTF;
                                            //    cmd.Parameters["@ValueLongText"].Value = ltS.Text;
                                            //    cmd.Parameters["@ValueString"].Value = o;
                                            //}
                                            //else
                                            //{
                                            LongText ltS = new LongText();
                                            ltS.Text = o.ToString();
                                            ltS.RTF = o.ToString();
                                            cmd.Parameters["@ValueRTF"].Value = ltS.RTF;
                                            cmd.Parameters["@ValueLongText"].Value = ltS.Text;
                                            cmd.Parameters["@ValueString"].Value = o;
                                            //}
                                        }
                                        else
                                        {
                                            cmd.Parameters["@ValueString"].Value = o;
                                        }
                                        break;
                                    case "System.Int32":
                                        if (o.ToString().Length > 0)
                                            cmd.Parameters["@ValueInt"].Value = int.Parse(o.ToString());
                                        else
                                            cmd.Parameters["@ValueInt"].Value = null;
                                        break;
                                    case "System.Nullable`1[System.Int32]":
                                        if (o.ToString().Length > 0)
                                            cmd.Parameters["@ValueInt"].Value = int.Parse(o.ToString());
                                        else
                                            cmd.Parameters["@ValueInt"].Value = null;
                                        break;
                                    case "System.Int32?":
                                        if (o.ToString().Length > 0)
                                            cmd.Parameters["@ValueInt"].Value = int.Parse(o.ToString());
                                        else
                                            cmd.Parameters["@ValueInt"].Value = null;
                                        break;
                                    case "System.Boolean":
                                        if (o.ToString().Length > 0)
                                            cmd.Parameters["@ValueBoolean"].Value = bool.Parse(o.ToString());
                                        else
                                            cmd.Parameters["@ValueBoolean"].Value = null;
                                        break;
                                    case "System.DateTime":
                                        DateTime sdt = (DateTime)o;
                                        if (sdt != DateTime.MinValue)
                                        {
                                            cmd.Parameters["@ValueDate"].Value = o;
                                        }
                                        else
                                            skip = true;
                                        break;
                                    case "DateTime":
                                        //21 October 2013 "tostrings is DateTime not system.DateTime
                                        DateTime dt = (DateTime)o;
                                        if (dt != DateTime.MinValue)
                                        {
                                            cmd.Parameters["@ValueDate"].Value = o;
                                        }
                                        else
                                            skip = true;
                                        break;
                                    case "LongText":
                                        ////5 february 2013
                                        //if (o is System.String) //WHICH SHOULD NEVER HAPPEN
                                        //{
                                        //    LongText ltS = new LongText();
                                        //    ltS.Text = o.ToString();
                                        //    ltS.RTF = o.ToString();
                                        //    cmd.Parameters["@ValueRTF"].Value = ltS.RTF;
                                        //    cmd.Parameters["@ValueLongText"].Value = ltS.Text;
                                        //    cmd.Parameters["@ValueString"].Value = o;
                                        //}
                                        //else
                                        //{
                                        LongText lt = new LongText();
                                        lt.Text = o.ToString();
                                        lt.RTF = o.ToString();
                                        cmd.Parameters["@ValueRTF"].Value = lt.RTF;
                                        cmd.Parameters["@ValueLongText"].Value = lt.Text;
                                        cmd.Parameters["@ValueString"].Value = lt.Text;
                                        //}
                                        break;
                                    case "Attachment":
                                        // do nothing
                                        break;
                                    case "System.Double":
                                        if (o.ToString().Length > 0)
                                            cmd.Parameters["@ValueDouble"].Value = double.Parse(o.ToString());
                                        else
                                            cmd.Parameters["@ValueDouble"].Value = null;
                                        break;
                                    default:
                                        if (o is MetaBase)
                                        {
                                            MetaBase mbase = o as MetaBase;
                                            mbase.Save(Guid.NewGuid());
                                            cmd.Parameters["@ValueString"].Value = mbase.pkid.ToString() + "|" + mbase.MachineName;
                                            break;
                                        }
                                        else
                                        {
                                            try
                                            {
                                                cmd.Parameters["@ValueString"].Value = o;
                                            }
                                            catch
                                            {
                                            }
                                        }
                                        break;
                                }
                                if (skip) //21 Ocober 2013 using this we can override some values like MINDATE
                                    continue;
                                try
                                {
                                    //SAVE FIELD VALUE // CHECK FOR EXISTING FIELD VALUE IF IT IS THE SERVER
                                    //if (ProviderName == Core.Variables.Instance.ServerProvider)
                                    //{
                                    //get value

                                    //match value to this

                                    // if it differs?
                                    //}
                                    try
                                    {
                                        if (((BrowsableAttribute)(TypeDescriptor.GetProperties(this.GetType())[pinfo.Name]).Attributes[typeof(BrowsableAttribute)]).Browsable == false)
                                        {
                                            SetWithoutChange(pinfo.Name, "");
                                            cmd.Parameters["@ValueRTF"].Value = "";
                                            cmd.Parameters["@ValueLongText"].Value = "";
                                            cmd.Parameters["@ValueString"].Value = "";
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.WriteLog(ex.ToString());
                                    }
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    LogEntry lentry = new LogEntry();
                                    try
                                    {
                                        lentry.Title = "Save MetaObject " + this.ToString() + "(" + this._ClassName + ") PInfo.Name:" + pinfo.Name + " Failed " + Environment.NewLine + ex.Message.ToString();
                                    }
                                    catch
                                    {
                                        lentry.Title = "Save MetaObject " + this._ClassName + " Failed";
                                    }
                                    lentry.Message = ex.Source;
                                    Logger.Write(lentry);
                                }
                            }
                            else
                            {
#if DEBUG
                                //DELETE THE VALUE BECAUSE IT IS NULL (Not empty, empty values are handled above)
                                SqlCommand cmdDelete = new SqlCommand("DELETE FROM ObjectFieldValue WHERE ObjectID=" + pkid.ToString() + " AND MachineID='" + MachineName + "' AND FieldID=" + int.Parse(drvF["pkid"].ToString()), new SqlConnection(connString));
                                cmdDelete.CommandType = CommandType.Text;
                                try
                                {
                                    if (cmdDelete.Connection.State != ConnectionState.Open)
                                    {
                                        cmdDelete.Connection.Open();
                                    }
                                    cmdDelete.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Log.WriteLog(cmdDelete.CommandText + Environment.NewLine + ex.ToString());
                                }
                                finally
                                {
                                    cmdDelete.Connection.Close();
                                }
#endif
                            }
                        }
                    }
                }
                catch (Exception conEx)
                {
                    Core.Log.WriteLog(conEx.ToString());
                }
                finally
                {
                    cmd.Connection.Close();
                }
            }
            #endregion
            IsSaved = true;
            CacheManager cacheManager = CacheFactory.GetCacheManager();
            string key = pkid.ToString() + "|" + MachineName;
            if (!cacheManager.Contains(key))
            {
                cacheManager.Add(key, this);
            }
            else
            {
                //Update cache
                cacheManager.Remove(key);
                cacheManager.Add(key, this);
            }
        }
        public void Save(Guid unique)
        {
            SaveToRepository(unique, Core.Variables.Instance.ClientProvider);
        }

        private bool CreateObjectInDB(string connString, string providerName)
        {
            bool saved = false;

            // Create an object
            SqlCommand cmd = new SqlCommand("META_CreateObject", new SqlConnection(connString));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Class", SqlDbType.VarChar, 50));
            cmd.Parameters["@Class"].Value = this._ClassName;
            cmd.Parameters.Add(new SqlParameter("@pkid", SqlDbType.Int));
            cmd.Parameters["@pkid"].Direction = ParameterDirection.InputOutput;
            cmd.Parameters.Add(new SqlParameter("@WorkspaceTypeId", SqlDbType.Int));
            cmd.Parameters.Add(new SqlParameter("@WorkspaceName", SqlDbType.VarChar, 100));
            //Log.WriteLog("Setting workspace");
            if (this.WorkspaceName == null || this.WorkspaceTypeId == 0)
            {
                //Log.WriteLog("Object Workspace is null : Setting to current workspace - " + Variables.Instance.CurrentWorkspaceName);
                cmd.Parameters["@WorkspaceName"].Value = Variables.Instance.CurrentWorkspaceName;
                cmd.Parameters["@WorkspaceTypeId"].Value = Variables.Instance.CurrentWorkspaceTypeId;

                this.WorkspaceName = Variables.Instance.CurrentWorkspaceName;
                this.WorkspaceTypeId = Variables.Instance.CurrentWorkspaceTypeId;
            }
            else
            {
                //Log.WriteLog("Setting to current workspace - " + WorkspaceName);
                cmd.Parameters["@WorkspaceName"].Value = this.WorkspaceName;
                cmd.Parameters["@WorkspaceTypeId"].Value = this.WorkspaceTypeId;
            }
            cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int));

            int userid = (providerName == Core.Variables.Instance.ServerProvider) ? GetServerUserID() : Variables.Instance.UserID;
            //Log.WriteLog("Saving object with userid " + userid.ToString() + " on server: " + connString);

            cmd.Parameters["@UserID"].Value = userid;
            cmd.Parameters["@pkid"].Value = this.pkid;
            cmd.Parameters.Add(new SqlParameter("@MachineName", SqlDbType.VarChar, 50));
            cmd.Parameters["@MachineName"].Direction = ParameterDirection.InputOutput;
            cmd.Parameters["@MachineName"].Value = MachineName;

            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }

            try
            {
                //Log.WriteLog("Start save ([" + this.Class + "]" + this.ToString() + ")");
                //If this workspace is not in the database then this causes a problem
                if (Variables.Instance.WorkspaceHashtable.Contains(WorkspaceName + "#" + WorkspaceTypeId.ToString()))
                {
                    //Log.WriteLog("Workspace found in hashtable");
                    //Log.WriteLog("Saving object with hashtable workspace - " + WorkspaceName);
                    cmd.ExecuteNonQuery();
                    saved = true;
                }
                else
                {
                    cmd.Parameters["@WorkspaceName"].Value = Variables.Instance.CurrentWorkspaceName;
                    cmd.Parameters["@WorkspaceTypeId"].Value = Variables.Instance.CurrentWorkspaceTypeId;
                    Log.WriteLog("CRETEOBJECTINDB::Workspace(" + WorkspaceName + ") not found in hashtable - Saving object to current workspace - " + Variables.Instance.CurrentWorkspaceName);

                    this.WorkspaceName = Variables.Instance.CurrentWorkspaceName;
                    this.WorkspaceTypeId = Variables.Instance.CurrentWorkspaceTypeId;

                    cmd.ExecuteNonQuery();
                    saved = true;
                }

                pkid = int.Parse(cmd.Parameters["@pkid"].Value.ToString());
                //added
                MachineName = cmd.Parameters["@MachineName"].Value.ToString();

                //Log.WriteLog("Object:" + this.pkid + " saved to " + providerName + " database");
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString(), "Error saving metaobject in database (CreateObjectInDB) on server: " + connString + " using userID: " + userid, System.Diagnostics.TraceEventType.Critical);
                saved = false;
            }

            cmd.Connection.Close();

            return saved;
        }

        public static int GetServerUserID()
        {
            bool getnewid = false;
            if (!(Variables.Instance.ServerUserID.HasValue))
            {
                getnewid = true;
            }
            else
            {
                if (Variables.Instance.ServerUserID.Value == 0)
                    getnewid = true;
            }

            int retval;
            if (getnewid)
            {
                LogEntry entry = new LogEntry();
                entry.Title = "Finding server userid:" + Variables.Instance.UserDomainIdentifier;

                SqlCommand cmdGetServerUserID = new SqlCommand("SELECT PKID FROM [USER] WHERE NAME = '" + Variables.Instance.UserDomainIdentifier + "'", new SqlConnection(Variables.Instance.ServerConnectionString));
                cmdGetServerUserID.CommandType = CommandType.Text;
                DataSet dsUserServer = new DataSet();
                SqlDataAdapter dsUserDap = new SqlDataAdapter();
                dsUserDap.SelectCommand = cmdGetServerUserID;
                if (cmdGetServerUserID.Connection.State != ConnectionState.Open)
                    cmdGetServerUserID.Connection.Open();
                dsUserDap.Fill(dsUserServer);
                cmdGetServerUserID.Connection.Close();

                bool parsed = int.TryParse(dsUserServer.Tables[0].DefaultView[0]["pkid"].ToString(), out retval);
                if (parsed)
                {
                    Variables.Instance.ServerUserID = retval;

                    entry.Message = "Assigning server userid:" + retval.ToString();
                }
                else
                {
                    entry.Message = "Cannot match server record for " + Variables.Instance.UserDomainIdentifier + " or parse pkid on server";

                }
                //if (Variables.Instance.VerboseLogging)
                //    Logger.Write(entry);
            }
            else
            {
                //LogEntry entry = new LogEntry();
                //entry.Title = "Server id is already set";
                //entry.Message = Variables.Instance.ServerUserID.Value.ToString() + " was returned as the server userid";
                //if (Variables.Instance.VerboseLogging)
                //    Logger.Write(entry);

                return Variables.Instance.ServerUserID.Value;
            }
            return Variables.Instance.ServerUserID.Value;
        }

        //private static int GetServerUserID(int userid)
        //{
        //    if (!(Variables.Instance.ServerUserID.HasValue))
        //    {
        //        SqlCommand cmdGetServerUserID = new SqlCommand("SELECT PKID FROM [USER] WHERE NAME = '" + Variables.Instance.UserDomainIdentifier + "'",
        //            new SqlConnection(Variables.Instance.ServerConnectionString));
        //        cmdGetServerUserID.CommandType = CommandType.Text;
        //        DataSet dsUserServer = new DataSet();
        //        SqlDataAdapter dsUserDap = new SqlDataAdapter();
        //        dsUserDap.SelectCommand = cmdGetServerUserID;
        //        if (cmdGetServerUserID.Connection.State != ConnectionState.Open)
        //            cmdGetServerUserID.Connection.Open();
        //        dsUserDap.Fill(dsUserServer);
        //        cmdGetServerUserID.Connection.Close();

        //        userid = int.Parse(dsUserServer.Tables[0].DefaultView[0]["pkid"].ToString());
        //        Variables.Instance.ServerUserID = userid;

        //    }
        //    else
        //    {
        //        return Variables.Instance.ServerUserID.Value;
        //    }
        //    return userid;
        //}

        public void CopyPropertiesToObject(MetaBase mbase)
        {
            //Collection<PropertyInfo> pinfoCollection = this.GetMetaPropertyList(false);
            foreach (PropertyInfo pinfo in this.GetMetaPropertyList(false))
            {
                if ((this.Get(pinfo.Name) != null && (!(pinfo.Name.StartsWith("_")))) && (!pinfo.Name.ToLower().StartsWith("embeddedrelat")))
                {
                    if (pinfo.CanWrite)
                        mbase.SetWithoutChange(pinfo.Name, this.Get(pinfo.Name));
                }
            }

            OnChanged(EventArgs.Empty);
        }

        public string SafelyShortenString(string inputString, int length)
        {
            if (inputString != null)
            {
                if (inputString.Length > length)
                {
                    return inputString.Substring(0, length);
                }
                else
                    return inputString;
            }
            return "";

        }

    }
}