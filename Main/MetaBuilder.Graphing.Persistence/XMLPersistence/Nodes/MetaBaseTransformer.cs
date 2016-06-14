using System;
using System.Collections.Generic;
using System.Reflection;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Meta;
using System.Collections.ObjectModel;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Nodes
{
    public class MetaBaseTransformer : BaseTransformer
    {

        #region Fields (1)

        private readonly Dictionary<string, MetaBase> AlreadyLoadedObjects;

        #endregion Fields

        #region Constructors (1)

        public MetaBaseTransformer()
            : base()
        {
            this.TransformerType = typeof(MetaBase);
            this.ElementName = "metaobject";
            AlreadyLoadedObjects = new Dictionary<string, MetaBase>();
            this.IdAttributeUsedForSharedObjects = true;
        }

        #endregion Constructors

        #region Methods (4)

        // Public Methods (4) 

        string fieldClassName = "";
        public Collection<string> ClassFieldRemappedFields
        {
            get
            {
                Collection<string> r = new Collection<string>();
                foreach (string s in Core.Variables.Instance.ClassFieldRemappedField)
                {
                    string c = "";
                    string oF = "";
                    string nF = "";
                    int i = 0;
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
                    if (c == fieldClassName)
                    {
                        r.Add(s);
                    }
                }
                return r;
            }
        }

        public override object Allocate()
        {
            MetaBase mb = null;
            int pkid = Int32Attr("Pkid", 0);
            string mach = StringAttr("MachineName", "");
            MetaBase mbCached = null;

            //Old diagram no pkid fix 4 December 2014
            if (pkid != 0 && mach.Length != 0)
                mbCached = RetrieveAlreadyLoaded(pkid, mach);

            if (mbCached != null)
            {
                mb = mbCached;
            }
            else
            {
                string className = StringAttr("Class", "MetaBase");

                fieldClassName = className;

                try
                {
                    if (className == "DataView")
                    {
                        string dataviewType = StringAttr("mDataViewType", "");
                        if (dataviewType == "Logical")
                        {
                            className = "LogicalInformationArtefact";
                        }
                    }
                }
                catch
                {

                }

                bool addConditionType = false;
                if (className == "Condition")
                {
                    addConditionType = true;
                }

                mb = Loader.CreateInstance(className);
                if (addConditionType)
                    mb.Set("GatewayType", "Condition");
                Core.Variables.Instance.UserFullName.ToString();
                if (mb == null)
                {
                    //do whatever we need to for this missing class
                    return mb;
                }
                mb.WorkspaceName = StringAttr("Workspace", "Sandbox");
                mb.WorkspaceTypeId = Int32Attr("WorkspaceTypeId", 0); //default to sandbox
                if (StringAttr("VCUser", "").Length > 0)
                    mb.VCUser = StringAttr("VCUser", "");

                //if (fieldClassName == mb.Class) //Class not renamed therefore pkid valid (unless we update data after metamodelpatches)
                //{
                if (pkid != 0 && mach.Length != 0)
                {
                    mb.pkid = pkid;
                    mb.MachineName = mach;
                }
                //}

                Type tState = typeof(VCStatusList);
                mb.State = (VCStatusList)Enum.Parse(tState, StringAttr("State", "None"));
                //Collection<PropertyInfo> props = mb.GetMetaPropertyList(false);
                foreach (PropertyInfo pinfo in mb.GetMetaPropertyList(false))
                {
                    try
                    {
                        string val = StringAttr("m" + pinfo.Name, null);
                        if (val != null)
                            mb.Set(pinfo.Name, val);
                    }
                    catch
                    {
                    }
                }

                #region RemapDomainValues

                if (mb.Class == "PhysicalSoftwareComponent")
                    if (mb.Get("SoftwareType") != null && mb.Get("SoftwareType").ToString() == "PhysServer")
                        mb.Set("SoftwareType", "PhysicalServer");

                #endregion

                //if (1 == 0)
                {
                    #region Remap Fields

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

                        //get the value of the oldfield
                        string val = StringAttr("m" + oldField, null);
                        //if it was set before
                        if (val != null)
                            mb.Set(newField, val); //and set the newfield to the value of the oldfield
                    }

                    #endregion
                }

                string k = mb.pkid.ToString() + mb.MachineName;
                if (!(AlreadyLoadedObjects.ContainsKey(k)))
                    AlreadyLoadedObjects.Add(k, mb);
            }
            return mb;
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
        }

        /*public override object Allocate()
        {
            Meta.MetaBase mb = new MetaBuilder.Meta.MetaBase();
            return mb;
        }*/
        public override void GenerateAttributes(Object obj)
        {
            try
            {
                MetaBase mb = obj as MetaBase;
                if (mb.tag is MetaBuilder.Graphing.Shapes.CollapsingRecordNodeItem)
                {
                    MetaBuilder.Graphing.Shapes.CollapsibleNode parent = (mb.tag as MetaBuilder.Graphing.Shapes.CollapsingRecordNodeItem).ParentNode as MetaBuilder.Graphing.Shapes.CollapsibleNode;
                    //Write combination of the parent collapsiblenodes pkid + id
                    if (parent != null && parent.MetaObject != null)
                        WriteAttrVal("mbCN", parent.MetaObject.pkid + ":" + parent.Location.ToString());
                }

                base.GenerateAttributes(mb);
                WriteAttrVal("Class", mb._ClassName);
                WriteAttrVal("Workspace", mb.WorkspaceName);
                WriteAttrVal("WorkspaceTypeId", mb.WorkspaceTypeId);
                WriteAttrVal("VCUser", mb.VCUser);
                WriteAttrVal("State", mb.State.ToString());
                WriteAttrVal("Pkid", mb.pkid);
                WriteAttrVal("MachineName", mb.MachineName);

                //Collection<PropertyInfo> props = mb.GetMetaPropertyList(false);
                foreach (PropertyInfo pinfo in mb.GetMetaPropertyList(false))
                {
                    object o = mb.Get(pinfo.Name);
                    if (o != null)
                    {
                        string oString = o.ToString();

                        try
                        {
                            if (oString != string.Empty)
                                WriteAttrVal("m" + pinfo.Name, oString);
                        }
                        catch (Exception ex)
                        {
                            Core.Log.WriteLog(ex.ToString());
                            //the oString is invalid xml
                            oString = System.Xml.XmlConvert.EncodeName(oString);
                            if (oString != string.Empty)
                                WriteAttrVal("m" + pinfo.Name, oString);
                        }
                    }
                }
                this.Writer.MakeShared(mb);
            }
            catch (Exception ex)
            {
                if ((obj as MetaBase) != null)
                    Core.Log.WriteLog((obj as MetaBase).ToString() + Environment.NewLine + ex.ToString());
                else
                    Core.Log.WriteLog(ex.ToString());
            }
        }

        public override void UpdateReference(object obj, string prop, object referred)
        {
            //if (prop.ToLower() == "anchor")
            //{
            //    Console.WriteLine("anchor on metaobject");
            //}
        }

        public MetaBase RetrieveAlreadyLoaded(int pkid, string machinename)
        {
            if (pkid > 0)
            {
                string k = pkid.ToString() + machinename;
                if (AlreadyLoadedObjects.ContainsKey(k))
                    return AlreadyLoadedObjects[k];
            }
            return null;
        }

        #endregion Methods

    }
}