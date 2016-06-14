using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using MetaBuilder.Core;
using MetaBuilder.Meta;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Collections.ObjectModel;

namespace MetaBuilder.UIControls.GraphingUI.DupeChecker
{
    public class FindExistingObject
    {

        #region Fields (3)

        bool aborted = false;
        private List<MetaBase> matchedObjects;
        private MetaBase mbase;

        #endregion Fields

        #region Enums (1)

        public enum BackgroundSearchType
        {
            Across, Current
        }

        #endregion Enums

        #region Constructors (1)

        public FindExistingObject()
        {
            MatchedObjects = new List<MetaBase>();
        }

        #endregion Constructors

        #region Properties (2)

        public List<MetaBase> MatchedObjects
        {
            get { return matchedObjects; }
            set { matchedObjects = value; }
        }

        public MetaBase MetaBaseToFind
        {
            get { return mbase; }
            set { mbase = value; }
        }

        #endregion Properties

        #region Methods (3)

        // Public Methods (2) 

        public void Abort()
        {
            aborted = true;
        }

        public void FindObject(BackgroundSearchType searchType, string fileID)
        {
            if (!aborted)
            {
                string connstring = Variables.Instance.ConnectionString;
                string ViewName = "METAView_" + mbase._ClassName + "_Listing";
                Type t = mbase.GetType();
                Collection<PropertyInfo> properties = mbase.GetMetaPropertyList(false);
                StringBuilder propertyFilterBuilder = new StringBuilder();
                propertyFilterBuilder.Append("1 = 1 ");

                bool SomePropertiesAreDirty = false;
                //21 October 2013
                //only include properties which have values
                SomePropertiesAreDirty = BuildSearchString(searchType, properties, propertyFilterBuilder, SomePropertiesAreDirty);
                if (!Variables.Instance.CheckDuplicatesAcrossWorkspaces)
                {
                    propertyFilterBuilder.Append(" AND " + ViewName + ".WORKSPACENAME='" + Variables.Instance.CurrentWorkspaceName + "' AND " + ViewName + ".WorkspaceTypeId=" + Variables.Instance.CurrentWorkspaceTypeId.ToString());
                }
                propertyFilterBuilder.Append(" AND " + ViewName + ".VCStatusID <> 1 and " + ViewName + ".VCStatusID <> 4 and " + ViewName + ".VCStatusID <> 8 and " + ViewName + ".VCStatusID <> 3");
                // Console.WriteLine(propertyFilterBuilder.ToString());
                if (SomePropertiesAreDirty)
                {
                    try
                    {
                        //dal.TransactionManager tman = new MetaBuilder.DataAccessLayer.TransactionManager(Core.Variables.Instance.ConnectionString);
                        string jointoactivediagrams = "";
#if DEBUG
                        //if (fileID.Length > 0)
                        //{
                        //    jointoactivediagrams = " INNER JOIN ActiveDiagramObjects  ON ActiveDiagramObjects.MetaObjectID = " + ViewName + ".pkid AND ActiveDiagramObjects.Machine = " + ViewName + ".Machine INNER JOIN GraphFile ON ActiveDiagramObjects.pkid = GraphFile.pkid";
                        //    propertyFilterBuilder.Append(" AND (GraphFile.OriginalFileUniqueID <> '" + fileID + "') ");
                        //}
#endif
                        string commandText = "SELECT " + ViewName + ".pkid," + ViewName + ".Machine," + ViewName + ".WorkspaceName FROM " + ViewName + jointoactivediagrams + " WHERE " + propertyFilterBuilder.ToString();

                        SqlCommand cmd = new SqlCommand(commandText, new SqlConnection(connstring));
                        DataSet ds = new DataSet();
                        SqlDataAdapter dap = new SqlDataAdapter();
                        dap.SelectCommand = cmd;
                        dap.Fill(ds, "ExistingRecords");

                        //7 January 2013 TimeOut Expired
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRowView drv in ds.Tables[0].DefaultView)
                            {
                                MetaBase newmbase = Loader.GetByID(mbase._ClassName, int.Parse(drv["pkid"].ToString()), drv["Machine"].ToString());
                                //newmbase.WorkspaceName = drv["workspaceName"].ToString();
                                //newmbase.MachineName = drv["Machine"].ToString();
                                //newmbase._pkid = int.Parse(drv["pkid"].ToString());
                                if (newmbase.pkid != mbase.pkid || newmbase.MachineName != mbase.MachineName)
                                    matchedObjects.Add(newmbase); //int.Parse(ds.Tables[0].Rows[0]["pkid"].ToString()));
                            }
                        }
                    }
                    catch (Exception x)
                    {
                        LogEntry lentry = new LogEntry();
                        lentry.Message = x.ToString();
                        lentry.Title = "FindExistingObject::FindObject";
                        Logger.Write(lentry);
                    }
                }
            }
        }

        // Private Methods (1) 

        private bool BuildSearchString(BackgroundSearchType searchType, Collection<PropertyInfo> properties, StringBuilder propertyFilterBuilder, bool SomePropertiesAreDirty)
        {
            object o = null;
            //string propvalue = "";
            string ViewName = "METAView_" + mbase._ClassName + "_Listing";

            foreach (PropertyInfo pinfo in properties)
            {
                if (pinfo.Name != "Class" && pinfo.Name != "_ClassName" && pinfo.Name.ToString().ToLower() != "workspacetypeid" && (!pinfo.Name.ToString().ToLower().Contains("workspace")))
                {
                    switch (pinfo.PropertyType.Name)
                    {
                        case "String":
                            o = mbase.Get(pinfo.Name);
                            if (o != null)
                            {
                                SomePropertiesAreDirty = true;
                                //propvalue = o.ToString();
                                if (o.ToString().Length > 0)
                                    switch (searchType)
                                    {
                                        case BackgroundSearchType.Across:
                                            propertyFilterBuilder.Append(" AND " + ViewName + "." + pinfo.Name + " = '" + o.ToString().Replace("'", "''") + "'");
                                            break;
                                        case BackgroundSearchType.Current:
                                            propertyFilterBuilder.Append(" AND " + ViewName + "." + pinfo.Name + " like '%" + o.ToString().Replace("'", "''") + "%'");
                                            break;
                                    }
                            }
                            break;

                        case "Int32":
                            o = mbase.Get(pinfo.Name);
                            if (o != null)
                            {
                                SomePropertiesAreDirty = true;
                                //propvalue = o.ToString();
                                if (o.ToString().Length > 0)
                                    switch (searchType)
                                    {
                                        case BackgroundSearchType.Across:
                                            propertyFilterBuilder.Append(" AND " + ViewName + "." + pinfo.Name + " = " + o.ToString() + "");
                                            break;
                                        case BackgroundSearchType.Current:
                                            propertyFilterBuilder.Append(" OR " + ViewName + "." + pinfo.Name + " = " + o.ToString() + "");
                                            break;
                                    }
                            }
                            break;
                    }
                }
                //// Console.WriteLine(pinfo.Name + " Type:" + pinfo.PropertyType.Name);
            }

            return SomePropertiesAreDirty;
        }

        #endregion Methods

    }
}