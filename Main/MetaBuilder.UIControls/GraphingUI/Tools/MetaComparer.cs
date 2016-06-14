using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Graphing;
using MetaBuilder.Graphing.Shapes.Nodes;

using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Meta;
using Northwoods.Go;
using System.Windows.Forms;
using System.Data;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
using MetaBuilder.Core;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.Docking;
using MetaBuilder.Graphing.Controllers;
using TraceTool;
using System.Reflection;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Collections.ObjectModel;
namespace MetaBuilder.UIControls.GraphingUI.Tools
{
    public class MetaComparerWorker : BackgroundWorker
    {
        private bool autoRefresh;
        private IGoCollection collection;
        public MetaComparerWorker(IGoCollection col, bool autorefresh, GraphView myView)
        {
            Loader.FlushDataViews();

            autoRefresh = autorefresh;
            collection = col;
            MyView = myView;

            this.WorkerReportsProgress = true;
            this.WorkerSupportsCancellation = false;
            this.DoWork += new DoWorkEventHandler(MetaComparerWorker_DoWork);
            this.ProgressChanged += new ProgressChangedEventHandler(MetaComparerWorker_ProgressChanged);
            //this.RunWorkerCompleted += new RunWorkerCompletedEventHandler(MetaComparerWorker_RunWorkerCompleted);
            RunWorkerAsync();
            Application.DoEvents();
        }

        private void MetaComparerWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //DockingForm.DockForm.DisplayTip(e.UserState.ToString(), "Refresh Data " + e.ProgressPercentage.ToString() + "%");
            DockingForm.DockForm.UpdateTotal(100);
            DockingForm.DockForm.UpdateStatusLabel(e.UserState.ToString());
            DockingForm.DockForm.ProgressUpdate(e.ProgressPercentage);
        }

        private void MetaComparerWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //#if DEBUG
            //            Console.WriteLine(DateTime.Now);
            diffData = new Dictionary<string, List<DifferingData>>();

            if (Core.Variables.Instance.ValidateVersionControl)
                CompareLinks(collection);

            DoComparison(collection, autoRefresh);
            if (collection is NormalDiagram)
            {
                ReportProgress(50, "Compare File");
                DoFileComparison();
            }

            ReportProgress(60, "Shadow Copies");
            foreach (KeyValuePair<string, Collection<GoObject>> kvp in shallowCopies)
                if (kvp.Value.Count > 1)
                    foreach (GoObject obj in kvp.Value)
                        obj.Shadowed = true;
            //Console.WriteLine(DateTime.Now + " " + diffData.Count.ToString());
            if (collection is NormalDiagram)
                ReportProgress(75, "Refresh");
            if (lComparer.IsBusy)
                ReportProgress(90, "Compare links");
            //#endif
            //            diffData = new Dictionary<string, List<DifferingData>>();
            //            CompareSelection(collection, autoRefresh);
            //#if DEBUG
            //            Console.WriteLine(DateTime.Now + " " + diffData.Count.ToString());
            //#endif
        }

        private GraphView myView;
        private GraphView MyView
        {
            get { return myView; }
            set { myView = value; }
        }

        private VisualIndicatorController indicatorController = new VisualIndicatorController();

        #region Internal Classes
        public enum ComparisonResultType
        {
            Not_In_Database, Differs_From_DB, Equal, Not_On_Diagram
        }

        public class ComparisonResult
        {
            private MetaBase metaObject;
            public MetaBase MetaObject
            {
                get { return metaObject; }
                set { metaObject = value; }
            }
            private List<NodeCompareItem> items;
            public List<NodeCompareItem> Items
            {
                get { return items; }
                set { items = value; }
            }
        }
        public class NodeCompareItem
        {
            private MetaBase objectInDB;
            public MetaBase ObjectInDB
            {
                get { return objectInDB; }
                set { objectInDB = value; }
            }

            private IMetaNode imNode;
            public IMetaNode IMNode
            {
                get { return imNode; }
                set { IMNode = value; }
            }
            private bool isCollapsible;
            public bool IsCollapsible
            {
                get { return isCollapsible; }
                set { isCollapsible = value; }
            }

            public NodeCompareItem(IMetaNode imn, bool collapsible)
            {
                this.imNode = imn;
                this.isCollapsible = collapsible;
            }
            private bool processed;
            public bool Processed
            {
                get { return processed; }
                set { processed = value; }
            }

        }
        #endregion

        public Dictionary<MetaBase, List<NodeCompareItem>> UpdateShallowCopies(Dictionary<MetaBase, List<NodeCompareItem>> ExistingNodes)
        {
            //#if DEBUG
            //            TTrace.Debug.Indent("Updating Shallow Copies");
            //#endif

            Dictionary<string, List<NodeCompareItem>> itemsWithSameKey = new Dictionary<string, List<NodeCompareItem>>();

            //#if DEBUG
            //            TTrace.Debug.Indent("Finding items with same keys");
            //#endif

            #region Find items with same keys
            foreach (KeyValuePair<MetaBase, List<NodeCompareItem>> kvp in ExistingNodes)
            {

                MetaKey mkey = new MetaKey();
                mkey.PKID = kvp.Key.pkid;
                mkey.Machine = kvp.Key.MachineName;

                if (!itemsWithSameKey.ContainsKey(mkey.ToString()))
                    itemsWithSameKey.Add(mkey.ToString(), new List<NodeCompareItem>());
                foreach (NodeCompareItem node in kvp.Value)
                {
                    if (!itemsWithSameKey[mkey.ToString()].Contains(node))
                        itemsWithSameKey[mkey.ToString()].Add(node);
                }
            }
            #endregion

            //#if DEBUG
            //            TTrace.Debug.UnIndent();
            //#endif

            GraphViewController gvc = new GraphViewController();
            gvc.MyView = MyView;
            #region Set MetaObjects to the first one's instance
            foreach (KeyValuePair<string, List<NodeCompareItem>> kvp in itemsWithSameKey)
            {
                if (kvp.Value.Count > 1)
                {
                    NodeCompareItem ncItemBase = kvp.Value[0];
                    MetaBase mbUseThis = ncItemBase.IMNode.MetaObject;

                    //#if DEBUG
                    //                    TTrace.Debug.Send("Shallow Item", kvp.Value[0].IMNode.MetaObject.ToString() + " (" + (kvp.Value.Count - 1).ToString() + ")");
                    //#endif

                    if (mbUseThis.pkid == 0) //objects whose pkid is 0 cannot be or have shallow copies unless strictly created at which point they will already have shadows
                        continue;
                    gvc.AddShadows(kvp.Value[0].IMNode as GoNode);
                    for (int i = 1; i < kvp.Value.Count; i++)
                    {
                        kvp.Value[i].IMNode.MetaObject = mbUseThis;
                        kvp.Value[i].IMNode.HookupEvents();
                        gvc.AddShadows(kvp.Value[i].IMNode as GoNode);
                    }
                }
            }
            #endregion

            //#if DEBUG
            //            TTrace.Debug.UnIndent();
            //#endif

            return ConvertToMBaseDictionaryFromKey(itemsWithSameKey);
        }
        private Dictionary<MetaBase, List<NodeCompareItem>> ConvertToMBaseDictionaryFromKey(Dictionary<string, List<NodeCompareItem>> metaKeyDictionary)
        {
#if DEBUG
            TTrace.Debug.Send("Converting back to MetaBase");
#endif
            Dictionary<MetaBase, List<NodeCompareItem>> retval = new Dictionary<MetaBase, List<NodeCompareItem>>();
            foreach (KeyValuePair<string, List<NodeCompareItem>> kvp in metaKeyDictionary)
            {
                if (kvp.Value.Count > 0)
                {
                    retval.Add(kvp.Value[0].IMNode.MetaObject, new List<NodeCompareItem>());
                    foreach (NodeCompareItem ncItem in kvp.Value)
                    {
                        retval[kvp.Value[0].IMNode.MetaObject].Add(ncItem);
                    }
                }
            }
#if DEBUG
            TTrace.Debug.Send("Converting back to MetaBase END");
#endif
            return retval;
        }
        public Dictionary<MetaBase, List<NodeCompareItem>> GetExistingNodes(IGoCollection col)
        {
            Dictionary<MetaBase, List<NodeCompareItem>> retval = new Dictionary<MetaBase, List<NodeCompareItem>>();
            foreach (GoObject o in col)
            {
                #region Normal Nodes
                if (o is IMetaNode)
                {
                    IMetaNode imnode = o as IMetaNode;
                    if (imnode.MetaObject != null)
                    {
                        NodeCompareItem nco = new NodeCompareItem(imnode, imnode is CollapsibleNode);
                        if (!retval.ContainsKey(imnode.MetaObject))
                            retval.Add(imnode.MetaObject, new List<NodeCompareItem>());
                        retval[imnode.MetaObject].Add(nco);
                    }
                }
                #endregion

                #region Groups/Collections
                if (o is GoGroup)
                {
                    Dictionary<MetaBase, List<NodeCompareItem>> innerObjects = GetExistingNodes(o as GoGroup);
                    foreach (KeyValuePair<MetaBase, List<NodeCompareItem>> kvp in innerObjects)
                    {
                        if (!retval.ContainsKey(kvp.Key))
                            retval.Add(kvp.Key, new List<NodeCompareItem>());
                        foreach (NodeCompareItem imInner in kvp.Value)
                        {
                            retval[kvp.Key].Add(imInner);
                        }
                    }
                }
                #endregion
            }
            return retval;
        }

        Dictionary<string, Collection<GoObject>> shallowCopies = new Dictionary<string, Collection<GoObject>>();

        public void DoComparison(IGoCollection col, bool Refresh)
        {
            IGoCollection collectionToActOn = null;
#if DEBUG
            //if (col is NormalDiagram)
            //{
            //    collectionToActOn = new GoCollection();
            //    foreach (IMetaNode node in MyView.ViewController.GetIMetaNodes())
            //        collectionToActOn.Add(node as GoObject);
            //}
            //else
            //{
            collectionToActOn = new GoCollection();
            foreach (IMetaNode node in MyView.ViewController.GetIMetaNodes(col))
                collectionToActOn.Add(node as GoObject);
            //}

#endif
            if (collectionToActOn == null)
                collectionToActOn = col;
            string dots = "";
            foreach (GoObject obj in collectionToActOn)
            {
                ReportProgress(25, "Compare Items" + dots);
                if (dots == "....")
                    dots = "";
                else
                    dots += ".";

                IMetaNode node = obj as IMetaNode;
                if (node == null)
                    continue;
                List<DifferingData> diffList = null;
                MetaBase mbObj = null;

                if (diffData.ContainsKey(node.MetaObject.pkid + node.MetaObject.MachineName))
                {
                    diffList = diffData[node.MetaObject.pkid + node.MetaObject.MachineName];
                    mbObj = Loader.GetByID(node.MetaObject.Class, node.MetaObject.pkid, node.MetaObject.MachineName);
                    shallowCopies[node.MetaObject.pkid + node.MetaObject.MachineName].Add(node as GoObject);
                }
                else
                {
                    diffList = new List<DifferingData>();
                    mbObj = Loader.GetByID(node.MetaObject.Class, node.MetaObject.pkid, node.MetaObject.MachineName);

                    Collection<GoObject> ObjectList = new Collection<GoObject>();
                    ObjectList.Add(node as GoObject);

                    shallowCopies.Add(node.MetaObject.pkid + node.MetaObject.MachineName, ObjectList);
                    diffData.Add(node.MetaObject.pkid + node.MetaObject.MachineName, diffList);
                }

                if (mbObj == null)
                {
                    DifferingData notInDbData = new DifferingData();
                    notInDbData.Node = node;
                    notInDbData.ObjectOnDiagram = node.MetaObject;
                    if ((node as GoGroup).Parent is RepeaterSection)
                    {
                        notInDbData.Section = (node as GoGroup).Parent as RepeaterSection;
                    }
                    notInDbData.DifferenceType = EDifferenceType.Not_in_DB;

                    indicatorController.AddChangedIndicator("Not in Database", System.Drawing.Color.Red, node as GoGroup);
                    diffList.Add(notInDbData);
                }
                else
                {
                    bool changes = false;
                    #region Iterate and Compare Properties

                    foreach (PropertyInfo pinfo in mbObj.GetMetaPropertyList(true))
                    {
                        //skip these properties
                        switch (pinfo.Name.ToLower())
                        {
                            case "state":
                            case "userid":
                            case "vcuser":
                            case "isindatabase":
                            case "workspacetypeid":
                            case "typefield":
                            case "issaved":
                                {
                                    //pinfo.ToString();
                                    continue;
                                }
                        }

                        //object[] aAttributes = pinfo.GetCustomAttributes(typeof(BrowsableAttribute), false);

                        //if (aAttributes != null && aAttributes.Length > 0)
                        //{
                        if (node.MetaObject.Get(pinfo.Name) != null && mbObj.Get(pinfo.Name) != null)
                        {
                            string s1 = node.MetaObject.Get(pinfo.Name).ToString().Trim();
                            string s2 = mbObj.Get(pinfo.Name).ToString().Trim();

                            switch (pinfo.Name.ToLower())
                            {
                                case "workspacename":
                                case "workspace":
                                    if (Core.Variables.Instance.CompareWorkspacesForObjects)
                                        if (s1.ToLower() != s2.ToLower())
                                            changes = true;
                                    break;
                                default:
                                    if (s1 != s2)
                                        changes = true;
                                    break;
                            }
                        }
                        else if (node.MetaObject.Get(pinfo.Name) == null && mbObj.Get(pinfo.Name) != null)
                        {
                            if (mbObj.Get(pinfo.Name).ToString().Length > 0)
                                changes = true;
                        }
                        else if (node.MetaObject.Get(pinfo.Name) != null && mbObj.Get(pinfo.Name) == null)
                        {
                            if (node.MetaObject.Get(pinfo.Name).ToString().Length > 0)
                                changes = true;
                        }
                        //}

                    }
                    #endregion

                    if (changes)
                    {
                        DifferingData changedData = new DifferingData();
                        changedData.Node = node;
                        changedData.ObjectInDatabase = mbObj;
                        changedData.ObjectOnDiagram = node.MetaObject;
                        if ((node as GoGroup).Parent is RepeaterSection)
                        {
                            changedData.Section = (node as GoGroup).Parent as RepeaterSection;
                        }
                        if (autoRefresh)
                        {
                            mbObj.CopyPropertiesToObject(changedData.Node.MetaObject);
                            changedData.DifferenceType = EDifferenceType.Equal;
                            node.HookupEvents();
                            node.BindToMetaObjectProperties();
                        }
                        else
                        {
                            changedData.DifferenceType = EDifferenceType.Meta_Properties_Differ;
                            indicatorController.AddChangedIndicator("Differs from DB", System.Drawing.Color.Red, node as GoGroup);
                        }

                        diffList.Add(changedData);
                    }

                    #region VCState
                    if (VCStatusTool.IsObsoleteOrMarkedForDelete(mbObj) && mbObj.IsInDatabase(Core.Variables.Instance.ClientProvider))
                    {
                        if (mbObj.State == VCStatusList.MarkedForDelete)
                            indicatorController.AddChangedIndicator("Marked For Delete", System.Drawing.Color.Red, (node as GoGroup));
                        else
                            indicatorController.AddIndicator("Obsolete", System.Drawing.Color.Red, (node as GoGroup));

                        (node as GoGroup).Editable = false;
                        (node as GoGroup).Copyable = false;
                    }
                    else if (mbObj.State == VCStatusList.CheckedIn || mbObj.State == VCStatusList.CheckedOutRead || mbObj.State == VCStatusList.Locked)
                    {
                        indicatorController.AddIndicator(mbObj.State.ToString(), System.Drawing.Color.Gray, (node as GoGroup));
                        node.MetaObject = mbObj;
                        node.HookupEvents();
                        node.BindToMetaObjectProperties();
                        (node as GoGroup).Editable = false;
                        (node as GoGroup).Copyable = true;
                    }
                    else if (mbObj.State == VCStatusList.Obsolete)
                    {
                        indicatorController.AddIndicator("Obsolete", System.Drawing.Color.Gray, (node as GoGroup));
                        node.MetaObject = mbObj;
                        node.HookupEvents();
                        node.BindToMetaObjectProperties();
                        (node as GoGroup).Editable = false;
                        (node as GoGroup).Copyable = true;
                    }
                    #endregion
                }

                diffData[node.MetaObject.pkid + node.MetaObject.MachineName] = diffList;

                //if (obj is GoGroup)
                //{
                //    DoComparison(obj as GoGroup, Refresh);
                //}

                #region Complex Node Refresh (After all children have been 'Checked' for changes. ie: only add missing ones)
                CollapsibleNode cnode = node as CollapsibleNode;
                if (cnode != null && mbObj != null)
                {
                    //Add missing items that are in the database
                    //these items are diffData NOT ON DIAGRAM!
                    //they are not added to the diffdata for the user to choose but automatically added

                    if (VCStatusTool.UserHasControl(cnode.MetaObject))
                    {
                        cnode.Initializing = true;
                        for (int r = 0; r < cnode.RepeaterSections.Count; r++)
                        {
                            RepeaterSection sect = cnode.RepeaterSections[r];
                            if (cnode.BindingInfo != null)
                            {
                                foreach (RepeaterBindingInfo rbinfo in cnode.BindingInfo.RepeaterBindings)
                                {
                                    if (cnode.BindingInfo.BindingClass == "DataTable")
                                    {
                                        sect.Name = sect.Name.Replace("Attributes", "Columns");
                                    }

                                    b.TList<b.ObjectAssociation> associatedRepeaterMetaObjects = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(cnode.MetaObject.pkid, cnode.MetaObject.MachineName);

                                    #region Retrieve list of associated items

                                    if (rbinfo.Association.ID.ToString() == "666" || rbinfo.Association.ID.ToString() == "667" || rbinfo.Association.ID.ToString() == "668" || rbinfo.Association.ID.ToString() == "669" || rbinfo.Association.ID.ToString() == "670" || rbinfo.Association.ID.ToString() == "671" || rbinfo.Association.ChildClass == "DataField" || rbinfo.Association.ChildClass == "DataAttribute" || rbinfo.Association.ChildClass == "DataColumn" || rbinfo.Association.ChildClass == "Attribute")
                                    {
                                        if (rbinfo.Association.ChildClass == "DataColumn")
                                            rbinfo.Association.ChildClass = "DataField";
                                        else if (rbinfo.Association.ChildClass == "Attribute")
                                            rbinfo.Association.ChildClass = "DataAttribute";

                                        rbinfo.Association.ID = -1;
                                        string kd = "";
                                        if (sect.Name.Contains("Key"))
                                            kd = "Key";
                                        else if (sect.Name.Contains("Descrip"))
                                            kd = "Descriptive";
                                        //get it from the database!
                                        TList<ClassAssociation> associations = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.Find("ParentClass='" + cnode.BindingInfo.BindingClass + "' AND ChildClass='" + rbinfo.Association.ChildClass);
                                        foreach (ClassAssociation classAssociation in associations)
                                        {
                                            if (classAssociation.IsActive == true && classAssociation.AssociationTypeID == 4 && classAssociation.Caption.Contains(kd))
                                            {
                                                rbinfo.Association.ID = classAssociation.CAid;
                                                break;
                                            }
                                        }
                                    }

                                    associatedRepeaterMetaObjects.Filter = "CAid = " + rbinfo.Association.ID;
                                    #endregion

                                    foreach (b.ObjectAssociation associatedRMO in associatedRepeaterMetaObjects)
                                    {
                                        bool found = false;
                                        foreach (GoObject itemChild in sect)
                                        {
                                            if (itemChild is CollapsingRecordNodeItem)
                                            {
                                                CollapsingRecordNodeItem cItem = itemChild as CollapsingRecordNodeItem;
                                                if ((!cItem.IsHeader))
                                                {
                                                    if (cItem.MetaObject != null)// && cItem.MetaObject.IsInDatabase)
                                                    {
                                                        if (cItem.MetaObject.pkid == associatedRMO.ChildObjectID && cItem.MetaObject.MachineName == associatedRMO.ChildObjectMachine)
                                                        {
                                                            found = true;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (!found)
                                        {
                                            MetaBase missingObject = Loader.GetByID(rbinfo.Association.ChildClass, associatedRMO.ChildObjectID, associatedRMO.ChildObjectMachine);
                                            if (VCStatusTool.IsObsoleteOrMarkedForDelete(missingObject))
                                                continue;
                                            CollapsingRecordNodeItem item = sect.AddItemFromCode() as CollapsingRecordNodeItem;
                                            if (item == null)
                                            {
                                                Log.WriteLog("MetaComparer::RefreshComplexNodeData::sect.AddItemFromCode() is null");
                                                continue;
                                            }
                                            item.MetaObject = missingObject;
                                            item.HookupEvents();
                                            item.BindToMetaObjectProperties();
                                            item.Editable = true;
                                            //indicator
                                            if (missingObject.State == VCStatusList.CheckedIn || missingObject.State == VCStatusList.CheckedOutRead || missingObject.State == VCStatusList.Locked)
                                            {
                                                indicatorController.AddIndicator(missingObject.State.ToString(), System.Drawing.Color.Gray, (node as GoGroup));
                                                //node.MetaObject = mbObj;
                                                //node.HookupEvents();
                                                //node.BindToMetaObjectProperties();
                                                (node as GoGroup).Editable = false;
                                                (node as GoGroup).Copyable = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        cnode.ForceComputeBounds();
                        cnode.Initializing = false;
                    }
                }
                #endregion
            }
        }
        private void DoFileComparison()
        {
            //now find items which are in the database that are not already in compareitems
            //first we need to see if there is an active GUID diagram in this database
            MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter tempFileAdapter = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();
            GraphFile databaseFile = null;
            try
            {
                databaseFile = tempFileAdapter.GetFileDetails((MyView.Document as NormalDiagram).VersionManager.CurrentVersion.OriginalFileUniqueIdentifier.ToString(), false);
            }
            catch
            {
                //missing diagram version or document is not normaldiagram
                MyView.Document.ToString();
            }
            if (databaseFile != null)
            {
                //get all the objects on this diagram and diff against compareitems
                foreach (MetaObject dbFileObject in d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetByGraphFileIDGraphFileMachineFromGraphFileObject(databaseFile.pkid, databaseFile.Machine))
                {
                    bool found = false;
                    if (diffData.ContainsKey(dbFileObject.pkid + dbFileObject.Machine))
                    {
                        found = true;
                        break;
                    }
                    if (!found)
                    {
                        //missing from diagram
                        DifferingData nodeData = new DifferingData();
                        nodeData.DifferenceType = EDifferenceType.Not_on_Diagram;
                        //nodeData.Node = null;
                        //nodeData.ObjectOnDiagram = null;
                        nodeData.ObjectInDatabase = Loader.GetByID(dbFileObject.Class, dbFileObject.pkid, dbFileObject.Machine);
                        if (nodeData.ObjectInDatabase != null)
                        {
                            if (!diffData.ContainsKey(dbFileObject.pkid + dbFileObject.Machine))
                            {
                                diffData.Add(dbFileObject.pkid + dbFileObject.Machine, new List<DifferingData>());
                            }
                            diffData[dbFileObject.pkid + dbFileObject.Machine].Add(nodeData);
                        }
                    }
                }
            }
        }

        public void CompareSelection(IGoCollection col, bool AutoRefresh)
        {
            if (myView != null)
                myView.StartTransaction();
            //myView.BeginUpdate();
            Dictionary<MetaBase, List<NodeCompareItem>> compareItems = GetExistingNodes(col);

            ReportProgress(5, "Binding Shallow Copies");
            compareItems = UpdateShallowCopies(compareItems);

            //remove indicators on items to compare
            ReportProgress(10, "Removing Indicators");
            indicatorController.RemoveChangedIndicators(col, true);

            ReportProgress(20, "Finding Delta");

            FindDelta(compareItems, AutoRefresh);

            if (Core.Variables.Instance.ValidateVersionControl)
                CompareLinks(col);

            try
            {
                myView.UpdateView();
                // myView.EndUpdate();
                myView.Document.UpdateViews();
            }
            catch (Exception ex)
            {
#if DEBUG
                Log.WriteLog(ex.ToString());
#endif
            }

            ReportProgress(100, "Complete");

            if (myView != null)
                myView.FinishTransaction("Refresh");
        }

        private LinkComparer LComparer;
        public LinkComparer lComparer
        {
            get { return LComparer; }
            set { LComparer = value; }
        }
        public void CompareLinks(IGoCollection col)
        {
            lComparer = new LinkComparer();
            lComparer.Collection = col;
            lComparer.DoWork += new DoWorkEventHandler(lComparer_DoWork);
            lComparer.RunWorkerCompleted += new RunWorkerCompletedEventHandler(lComparer_RunWorkerCompleted);
            ReportProgress(90, "Compare links");
            lComparer.RunWorkerAsync();
        }
        //Setting color in completion event does not work when refreshing manually and then whenever after than you open a diagram again
        private void lComparer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Console.WriteLine("LinkComparer " + collection.Count.ToString() + " COMPLETE");
            DockingForm.DockForm.UpdateStatusLabel("Ready");
            DockingForm.DockForm.ProgressUpdate(0);
            DockingForm.DockForm.ResetStatus();
        }
        private void lComparer_DoWork(object sender, DoWorkEventArgs e)
        {
            //Console.WriteLine("LinkComparer " + collection.Count.ToString() + " START");
            lComparer.CompareLinks();
        }

        private Dictionary<string, ComparisonResult> FindDelta(Dictionary<MetaBase, List<NodeCompareItem>> compareItems, bool update)
        {
            diffData = new Dictionary<string, List<DifferingData>>();

            Dictionary<string, ComparisonResult> delta = new Dictionary<string, ComparisonResult>();
            Dictionary<string, List<MetaKey>> itemKeys = new Dictionary<string, List<MetaKey>>();

            ReportProgress(30, "Preparing nodes");
            #region Prepare existing nodes for comparison - we want to quickly access MB's (and nodes) by using the MB's key

            foreach (KeyValuePair<MetaBase, List<NodeCompareItem>> kvp in compareItems)
            {
                MetaBase mb = kvp.Key;
                //also add keys to collection that will be sent to Loader
                if (!itemKeys.ContainsKey(mb._ClassName))
                    itemKeys.Add(mb._ClassName, new List<MetaKey>());
                MetaKey mkey = new MetaKey();
                mkey.PKID = mb.pkid;
                mkey.Machine = mb.MachineName;
                itemKeys[mb._ClassName].Add(mkey);

                ComparisonResult result = new ComparisonResult();
                result.Items = new List<NodeCompareItem>();
                result.MetaObject = mb;
                foreach (NodeCompareItem item in kvp.Value)
                {
                    result.Items.Add(item);
                }
                if (!delta.ContainsKey(mkey.ToString()))
                {
                    delta.Add(mkey.ToString(), result);
                }
            }

            #endregion

            // Get resultset
            ReportProgress(40, "Retrieving nodes");
            List<MetaBase> metasFromDB = Loader.GetFromProvider(itemKeys, false);

            //if (metasFromDB.Count == 0)
            //{
            //    ReportProgress(50, "Checking nodes database");

            //    //all items in selection are not in database
            //    foreach (KeyValuePair<MetaBase, List<NodeCompareItem>> i in compareItems)
            //    {
            //        foreach (NodeCompareItem item in i.Value)
            //        {
            //            item.ObjectInDB = null;
            //            //item.IMNode.MetaObject.IsInDatabase = false;
            //            item.IMNode.MetaObject.State = VCStatusList.None;
            //            //item.IMNode.BindToMetaObjectProperties();
            //            //item.IMNode.FireMetaObjectChanged(i, new EventArgs());
            //            indicatorController.AddChangedIndicator("Not in Database", System.Drawing.Color.Red, item.IMNode as GoGroup);
            //        }
            //    }
            //}

            // Convert to dictionary again so we dont have to loop through all the items
            Dictionary<string, MetaBase> dictMetasFromDB = new Dictionary<string, MetaBase>();
            foreach (MetaBase mbDB in metasFromDB)
            {
                dictMetasFromDB.Add(mbDB.pkid.ToString() + mbDB.MachineName, mbDB);
            }

            #region Compare/Update Items

            ReportProgress(60, (update == true ? "Update" : "Compare") + " Items");
            foreach (KeyValuePair<string, MetaBase> kvp in dictMetasFromDB)
            {

                MetaBase mbase = kvp.Value;
                string key = kvp.Key;

                ComparisonResult result = delta[key];
                if (update)
                {
                    foreach (NodeCompareItem ncitem in result.Items)
                    {
                        ncitem.IMNode.MetaObject = mbase;
                        ncitem.IMNode.HookupEvents();
                        //3 September 2013 : Attempt to make an item in database present that fact for duplicate marking
                        //ncitem.IMNode.MetaObject.IsInDatabase = true;
                        //ncitem.IMNode.BindToMetaObjectProperties();
                    }
                }
                else
                {
                    MetaBase mbaseOld = result.MetaObject;
                    //List<PropertyInfo> props = mbase.GetMetaPropertyList(true);
                    bool changes = false;

                    #region Iterate and Compare Properties
                    foreach (PropertyInfo pinfo in mbase.GetMetaPropertyList(true))
                    {
                        //skip these properties
                        switch (pinfo.Name.ToLower())
                        {
                            case "state":
                            case "userid":
                            case "vcuser":
                            case "isindatabase":
                            case "workspacetypeid":
                            case "typefield":
                            case "issaved":
                                {
                                    //pinfo.ToString();
                                    continue;
                                }
                        }

                        //object[] aAttributes = pinfo.GetCustomAttributes(typeof(BrowsableAttribute), false);

                        //if (aAttributes != null && aAttributes.Length > 0)
                        //{
                        if (mbaseOld.Get(pinfo.Name) != null && mbase.Get(pinfo.Name) != null)
                        {
                            string s1 = mbaseOld.Get(pinfo.Name).ToString().Trim();
                            string s2 = mbase.Get(pinfo.Name).ToString().Trim();

                            switch (pinfo.Name.ToLower())
                            {
                                case "workspacename":
                                    if (Core.Variables.Instance.CompareWorkspacesForObjects)
                                        if (s1.ToLower() != s2.ToLower())
                                            changes = true;
                                    break;
                                default:
                                    if (s1 != s2)
                                        changes = true;
                                    break;
                            }
                        }
                        else if (mbaseOld.Get(pinfo.Name) == null && mbase.Get(pinfo.Name) != null)
                        {
                            if (mbase.Get(pinfo.Name).ToString().Length > 0)
                                changes = true;
                        }
                        else if (mbaseOld.Get(pinfo.Name) != null && mbase.Get(pinfo.Name) == null)
                        {
                            if (mbaseOld.Get(pinfo.Name).ToString().Length > 0)
                                changes = true;
                        }
                        else
                        {
                            //mbaseOld.ToString();
                            //mbase.ToString();
                            //if either diagramObject or dbObject is null we come here
                            //For when the property is null and the other is not
                            //changes = mbaseOld.Get(pinfo.Name) != mbase.Get(pinfo.Name);
                            //continue;
                        }
                        //}

                    }
                    #endregion

                    foreach (NodeCompareItem ncitem in result.Items)
                    {
                        ncitem.ObjectInDB = mbase;
                        ncitem.IMNode.HookupEvents();
                        ncitem.Processed = true;
                        if (changes)
                        {
                            //Log.WriteLog("Changes detected");
                            if (ncitem.IMNode is GoGroup)
                            {
                                indicatorController.AddChangedIndicator("Differs from DB", System.Drawing.Color.Red, ncitem.IMNode as GoGroup);
                                EDifferenceType edtype = EDifferenceType.Meta_Properties_Differ;

                                DifferingData nodeData = new DifferingData();
                                nodeData.DifferenceType = edtype;
                                nodeData.Node = ncitem.IMNode;
                                nodeData.ObjectInDatabase = ncitem.ObjectInDB;

                                GoGroup grp = ncitem.IMNode as GoGroup;
                                if (grp.Parent is RepeaterSection)
                                {
                                    nodeData.Section = grp.Parent as RepeaterSection;
                                }
                                string nodeKey = ncitem.IMNode.MetaObject.pkid.ToString() + ncitem.IMNode.MetaObject.MachineName;
                                if (!diffData.ContainsKey(nodeKey))
                                {
                                    diffData.Add(nodeKey, new List<DifferingData>());
                                }
                                diffData[nodeKey].Add(nodeData);
                            }
                        }
                        else
                        {
                            //Log.WriteLog("No Changes Found");
                        }
                    }
                }
            }

            if (!update)
            {
                ReportProgress(70, "Diffing data");
                foreach (KeyValuePair<string, ComparisonResult> kvp in delta)
                {
                    ComparisonResult result = delta[kvp.Key];
                    foreach (NodeCompareItem ncitem in result.Items)
                    {
                        if (!ncitem.Processed)
                        {
                            if (ncitem.IMNode is GoGroup)
                            {
                                indicatorController.AddChangedIndicator("Not in Database", System.Drawing.Color.Red, ncitem.IMNode as GoGroup);

                                EDifferenceType edtype = EDifferenceType.Not_in_DB;
                                DifferingData nodeData = new DifferingData();
                                nodeData.DifferenceType = edtype;
                                nodeData.Node = ncitem.IMNode;
                                nodeData.ObjectInDatabase = null;

                                GoGroup grp = ncitem.IMNode as GoGroup;
                                if (grp.Parent is RepeaterSection)
                                {
                                    nodeData.Section = grp.Parent as RepeaterSection;
                                }
                                string nodeKey = ncitem.IMNode.MetaObject.pkid.ToString() + ncitem.IMNode.MetaObject.MachineName;
                                if (!diffData.ContainsKey(nodeKey))
                                {
                                    diffData.Add(nodeKey, new List<DifferingData>());
                                }
                                diffData[nodeKey].Add(nodeData);
                            }
                        }
                        else
                        {
                            ncitem.ToString();
                        }
                    }
                }
            }
            ReportProgress(80, "Complex Node Data");
            RefreshComplexNodeData(delta, update);
            ReportProgress(90, "Version Control Indicators");
            RefreshVCIndicators(delta, dictMetasFromDB);

            //if (true)
            //{
            //    //ReportProgress(95, "Calculating");
            //    AutoCalculateJobClassValues(delta, dictMetasFromDB);
            //}

            #endregion

            //now find items which are in the database that are not already in compareitems
            //first we need to see if there is an active GUID diagram in this database
            MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter tempFileAdapter = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();
            GraphFile databaseFile = null;
            try
            {
                databaseFile = tempFileAdapter.GetFileDetails((MyView.Document as NormalDiagram).VersionManager.CurrentVersion.OriginalFileUniqueIdentifier.ToString(), false);
            }
            catch
            {
                //missing diagram version or document is not normaldiagram
                MyView.Document.ToString();
            }
            if (databaseFile != null)
            {
                //get all the objects on this diagram and diff against compareitems
                foreach (MetaObject dbFileObject in d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetByGraphFileIDGraphFileMachineFromGraphFileObject(databaseFile.pkid, databaseFile.Machine))
                {
                    bool found = false;
                    if (itemKeys.ContainsKey(dbFileObject.Class))
                    {
                        foreach (MetaKey key in itemKeys[dbFileObject.Class])
                        {
                            if (key.PKID == dbFileObject.pkid && key.Machine == dbFileObject.Machine)
                            {
                                found = true;
                                break;
                            }
                        }
                    }
                    if (!found)
                    {
                        //missing from diagram
                        DifferingData nodeData = new DifferingData();
                        nodeData.DifferenceType = EDifferenceType.Not_on_Diagram;
                        //nodeData.Node = null;
                        //nodeData.ObjectOnDiagram = null;
                        nodeData.ObjectInDatabase = Loader.GetByID(dbFileObject.Class, dbFileObject.pkid, dbFileObject.Machine);
                        if (nodeData.ObjectInDatabase != null)
                        {
                            if (!diffData.ContainsKey(dbFileObject.pkid + dbFileObject.Machine))
                            {
                                diffData.Add(dbFileObject.pkid + dbFileObject.Machine, new List<DifferingData>());
                            }
                            diffData[dbFileObject.pkid + dbFileObject.Machine].Add(nodeData);
                        }
                    }
                }
            }

            return delta;
        }
        public Dictionary<string, List<DifferingData>> DiffData
        {
            get { return diffData; }
        }
        private Dictionary<string, List<DifferingData>> diffData;

        private void RefreshVCIndicators(Dictionary<string, ComparisonResult> delta, Dictionary<string, MetaBase> dictMetasFromDB)
        {
            if (Variables.Instance.ValidateVersionControl)
            {
                VisualIndicatorController indicatorController = new VisualIndicatorController();
                foreach (KeyValuePair<string, MetaBase> kvp in dictMetasFromDB)
                {
                    MetaBase mbase = kvp.Value;
                    string key = kvp.Key;
                    ComparisonResult result = delta[key];

                    #region iterate through items
                    foreach (NodeCompareItem item in result.Items)
                    {
                        bool skipGen = false;
                        if (item.IMNode is GoGroup)
                        {
                            GoGroup n = item.IMNode as GoGroup;
                            n.Editable = true;
                            indicatorController.RemoveIndicators(n);
                            if (n is CollapsingRecordNodeItem)
                            {
                                CollapsingRecordNodeItem nodeItem = n as CollapsingRecordNodeItem;
                                if (nodeItem.IsHeader || nodeItem.Text == "DataView Elements")
                                {
                                    skipGen = true;
                                }
                            }
                            #region Update Indicators
                            if (!skipGen)
                            {
                                if (VCStatusTool.IsObsoleteOrMarkedForDelete(mbase) && mbase.IsInDatabase(Core.Variables.Instance.ClientProvider))
                                {
                                    if (mbase.State == VCStatusList.MarkedForDelete)
                                        indicatorController.AddChangedIndicator("Marked For Delete", System.Drawing.Color.Red, n);
                                    else
                                        indicatorController.AddIndicator("Obsolete", System.Drawing.Color.Red, n);

                                    n.Editable = false;
                                    n.Copyable = false;
                                }
                                else if (mbase.State == VCStatusList.CheckedIn || mbase.State == VCStatusList.CheckedOutRead || mbase.State == VCStatusList.Locked)
                                {
                                    indicatorController.AddIndicator(mbase.State.ToString(), System.Drawing.Color.Gray, n);
                                    if (n is IMetaNode)
                                    {
                                        IMetaNode node = n as IMetaNode;
                                        node.MetaObject = mbase;
                                        node.BindToMetaObjectProperties();
                                        node.HookupEvents();
                                    }
                                    n.Editable = false;
                                    n.Copyable = true;
                                }
                                else if (mbase.State == VCStatusList.Obsolete)
                                {
                                    indicatorController.AddIndicator("Obsolete", System.Drawing.Color.Gray, n);
                                    if (n is IMetaNode)
                                    {
                                        IMetaNode node = n as IMetaNode;
                                        node.MetaObject = mbase;
                                        node.BindToMetaObjectProperties();
                                        node.HookupEvents();
                                    }
                                    n.Editable = false;
                                    n.Copyable = true;
                                }
                                //ADDED TO AUTO UPDATE CHECKED OUT ITEMS
                                //else if (mbase.State == VCStatusList.CheckedOut)
                                //{
                                //    indicatorController.AddIndicator("Checked Out", System.Drawing.Color.Gray, n);
                                //    if (n is IMetaNode)
                                //    {
                                //        IMetaNode node = n as IMetaNode;
                                //        node.MetaObject = mbase;
                                //    }
                                //    n.Editable = true;
                                //}
                                //else if (mbase.State == VCStatusList.Locked)
                                //{
                                //    indicatorController.AddIndicator("Locked", System.Drawing.Color.Blue, n);
                                //    if (n is IMetaNode)
                                //    {
                                //        IMetaNode node = n as IMetaNode;
                                //        node.MetaObject = mbase;
                                //    }
                                //    n.Editable = false;
                                //    n.Copyable = false;
                                //}
                            }
                        }
                            #endregion
                    }
                    #endregion
                }
            }
        }

        private void RefreshComplexNodeData(Dictionary<string, ComparisonResult> delta, bool update)
        {
            Collection<CollapsingRecordNodeItemList> updatedLists = new Collection<CollapsingRecordNodeItemList>();
            foreach (KeyValuePair<string, ComparisonResult> kvp in delta)
            {
                ComparisonResult res = kvp.Value;
                foreach (NodeCompareItem ncitem in res.Items)
                {
                    if (ncitem.IsCollapsible)
                    {
                        CollapsibleNode cnode = ncitem.IMNode as CollapsibleNode;
                        if (cnode != null)
                        {
                            Collection<RepeaterSection> sections = cnode.RepeaterSections;
                            if (cnode.MetaObject != null && cnode.MetaObject.IsInDatabase(Core.Variables.Instance.ClientProvider))
                            {
                                if (VCStatusTool.UserHasControl(cnode.MetaObject))
                                {
                                    for (int r = 0; r < sections.Count; r++)
                                    {
                                        Collection<MetaBase> mbExisting = new Collection<MetaBase>();
                                        cnode.Initializing = true;
                                        RepeaterSection sect = sections[r];
                                        if (cnode.BindingInfo != null)
                                        {
                                            foreach (RepeaterBindingInfo rbinfo in cnode.BindingInfo.RepeaterBindings)
                                            {
                                                if (cnode.BindingInfo.BindingClass == "DataTable")
                                                {
                                                    sect.Name = sect.Name.Replace("Attributes", "Columns");
                                                }
                                                if (sect.Name == rbinfo.RepeaterName)
                                                {
                                                    #region Refresh only the section
                                                    if (cnode.BindingInfo.BindingClass == "DataTable")
                                                        rbinfo.Association.ChildClass = "DataField";

                                                    #region Add existing items to a list

                                                    foreach (GoObject itemChild in sect)
                                                    {
                                                        if (itemChild is CollapsingRecordNodeItem)
                                                        {
                                                            CollapsingRecordNodeItem cItem = itemChild as CollapsingRecordNodeItem;
                                                            if ((!cItem.IsHeader))
                                                            {
                                                                if (cItem.MetaObject != null)// && cItem.MetaObject.IsInDatabase)
                                                                {
                                                                    //LOADING PARENT ITEMS AS WELL?
                                                                    mbExisting.Add(cItem.MetaObject);

                                                                    if (!(cItem.MetaObject.IsInDatabase(Core.Variables.Instance.ClientProvider)))
                                                                        indicatorController.AddChangedIndicator("Not in Database", System.Drawing.Color.Red, cItem);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    //Log.WriteLog(mbExisting.Count + " existing objects on " + cnode.MetaObject.ToString());

                                                    #endregion

                                                    #region Retrieve list of associated items

                                                    List<MetaBase> mbLoaded = new List<MetaBase>();
                                                    b.TList<b.ObjectAssociation> mos = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(cnode.MetaObject.pkid, cnode.MetaObject.MachineName);

                                                    if (rbinfo.Association.ID.ToString() == "666" || rbinfo.Association.ID.ToString() == "667" || rbinfo.Association.ID.ToString() == "668" || rbinfo.Association.ID.ToString() == "669" || rbinfo.Association.ID.ToString() == "670" || rbinfo.Association.ID.ToString() == "671" || rbinfo.Association.ChildClass == "DataField" || rbinfo.Association.ChildClass == "DataAttribute" || rbinfo.Association.ChildClass == "DataColumn" || rbinfo.Association.ChildClass == "Attribute")
                                                    {
                                                        if (rbinfo.Association.ChildClass == "DataColumn")
                                                            rbinfo.Association.ChildClass = "DataField";
                                                        else if (rbinfo.Association.ChildClass == "Attribute")
                                                            rbinfo.Association.ChildClass = "DataAttribute";

                                                        rbinfo.Association.ID = -1;
                                                        string kd = "";
                                                        if (sect.Name.Contains("Key"))
                                                            kd = "Key";
                                                        else if (sect.Name.Contains("Descrip"))
                                                            kd = "Descriptive";
                                                        //get it from the database!
                                                        TList<ClassAssociation> associations = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.Find("ParentClass='" + cnode.BindingInfo.BindingClass + "' AND ChildClass='" + rbinfo.Association.ChildClass);
                                                        foreach (ClassAssociation classAssociation in associations)
                                                        {
                                                            if (classAssociation.IsActive == true && classAssociation.AssociationTypeID == 4 && classAssociation.Caption.Contains(kd))
                                                            {
                                                                rbinfo.Association.ID = classAssociation.CAid;
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    mos.Filter = "CAid = " + rbinfo.Association.ID;
                                                    #endregion

                                                    #region Add to Loaded list (this is from DB)
                                                    for (int a = 0; a < mos.Count; a++)
                                                    {
                                                        b.ObjectAssociation oa = mos[a];
                                                        //27 November 2013 - skip triggers!
                                                        //24 January 2014 - Removed VCMachineID Check
                                                        if (oa.Machine != null && oa.Machine == "DB-TRIGGER")
                                                            continue;
                                                        if ((!VCStatusTool.IsObsoleteOrMarkedForDelete(oa)))
                                                        {
                                                            MetaBase mbChild = Loader.GetByID(rbinfo.Association.ChildClass, oa.ChildObjectID, oa.ChildObjectMachine);
                                                            if (!VCStatusTool.IsObsoleteOrMarkedForDelete(mbChild))
                                                            {
                                                                if (!mbLoaded.Contains(mbChild))
                                                                    mbLoaded.Add(mbChild);
                                                            }
                                                        }
                                                    }

                                                    //Log.WriteLog(mbExisting.Count + " loaded objects for " + cnode.MetaObject.ToString());
                                                    #endregion

                                                    #region Compare the existing list of MBs to the loaded list of MBs
                                                    bool foundAll = true;

                                                    foreach (MetaBase mbX in mbExisting)
                                                    {
                                                        bool foundThisOne = false;
                                                        foreach (MetaBase mbL in mbLoaded)
                                                        {
                                                            if (mbL != null)
                                                            {
                                                                if (mbL.pkid == mbX.pkid && mbL.MachineName == mbX.MachineName)
                                                                {
                                                                    foundThisOne = true;
                                                                    if (update)
                                                                        mbL.CopyPropertiesToObject(mbX);
                                                                }
                                                            }
                                                        }
                                                        if (foundThisOne == false)
                                                        {
                                                            if (mbX.IsInDatabase(Core.Variables.Instance.ClientProvider))
                                                                foundAll = false;
                                                        }
                                                    }
                                                    #endregion

                                                    #region Compare the loaded list of MBs to the existing list of MBs
                                                    foreach (MetaBase mbL in mbLoaded)
                                                    {
                                                        bool foundThisOne = false;
                                                        if (mbL != null)
                                                        {
                                                            string k = mbL.pkid.ToString() + mbL.MachineName;
                                                            foreach (MetaBase mbX in mbExisting)
                                                            {
                                                                if (mbL.pkid == mbX.pkid && mbL.MachineName == mbX.MachineName)
                                                                {
                                                                    foundThisOne = true;
                                                                }
                                                            }

                                                            if (foundThisOne == false)
                                                            {
                                                                foundAll = false;
                                                                DifferingData ddata = new DifferingData();
                                                                ddata.DifferenceType = EDifferenceType.Not_on_Diagram;
                                                                ddata.ObjectInDatabase = mbL;
                                                                ddata.Section = sect;
                                                                if (!diffData.ContainsKey(k))
                                                                    diffData.Add(k, new List<DifferingData>());
                                                                diffData[k].Add(ddata);
                                                            }
                                                        }
                                                    }
                                                    #endregion

                                                    if (update)
                                                    {
                                                        #region Did not find all the items (either way). Clear section and load from list.
                                                        if (!foundAll)
                                                        {
                                                            //Log.WriteLog("Did not find all objects");
                                                            updatedLists.Add(sect);
                                                            sect.ClearChildItems(); //NO REMOVE THE INCORRECT ONES AND ADD THE CORRECT ONES?!?!
                                                            foreach (MetaBase mbL in mbLoaded)
                                                            {
                                                                //if (mbL != null)
                                                                //{
                                                                CollapsingRecordNodeItem item = sect.AddItemFromCode() as CollapsingRecordNodeItem;
                                                                if (item == null)
                                                                {
                                                                    Log.WriteLog("MetaComparer::RefreshComplexNodeData::sect.AddItemFromCode() is null");
                                                                    continue;
                                                                }
                                                                item.MetaObject = mbL;
                                                                item.BindToMetaObjectProperties();
                                                                item.Editable = true;
                                                                item.HookupEvents();
                                                                //}
                                                            }
                                                            //GoNodeLinkEnumerator linkEnum = cnode.Links.GetEnumerator();
                                                            //while (linkEnum.MoveNext())
                                                            //{
                                                            //    if (linkEnum.Current is QLink)
                                                            //    {
                                                            //        QLink slink = linkEnum.Current as QLink;
                                                            //        slink.CalculateRoute();
                                                            //    }
                                                            //    else
                                                            //    {
                                                            //        GoLink link = linkEnum.Current as GoLink;
                                                            //        if (link != null)
                                                            //            link.CalculateRoute();
                                                            //    }
                                                            //}
                                                        }
                                                        #endregion
                                                    }
                                                    #endregion
                                                }
                                            }
                                        }
                                        cnode.ForceComputeBounds();
                                        cnode.Initializing = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (update)
            {
                foreach (CollapsingRecordNodeItemList list in updatedLists)
                {
                    list.LayoutChildren(null);
                    list.ForceComputeBounds();
                    list.Parent.LayoutChildren(null);
                    list.ParentList.LayoutChildren(null);
                    if (list.ParentList is CollapsingRecordNodeItemList && list.ParentList != list)
                    {
                        CollapsingRecordNodeItemList lparent = list.ParentList as CollapsingRecordNodeItemList;
                        lparent.ForceComputeBounds();
                    }
                    //list.Parent.ForceComputeBounds();
                    if (list.ParentNode is CollapsibleNode)
                    {
                        CollapsibleNode cnode = list.ParentNode as CollapsibleNode;
                        cnode.ForceComputeBounds();
                    }
                }
            }
        }

        private void AutoCalculateJobClassValues(Dictionary<string, ComparisonResult> delta, Dictionary<string, MetaBase> dictMetasFromDB)
        {
            int jobposition = 0;
            foreach (b.ClassAssociation cA in d.DataRepository.Provider.ClassAssociationProvider.GetByParentClass("Job"))
            {
                if (cA.ChildClass == "Position" && cA.AssociationTypeID == 4)
                {
                    jobposition = cA.CAid;
                    break;
                }
            }
            int positionperson = 0;
            foreach (b.ClassAssociation cA in d.DataRepository.Provider.ClassAssociationProvider.GetByParentClass("Position"))
            {
                if (cA.ChildClass == "Person" && cA.AssociationTypeID == 4)
                {
                    positionperson = cA.CAid;
                    break;
                }
            }

            foreach (KeyValuePair<string, MetaBase> kvp in dictMetasFromDB)
            {
                MetaBase mbase = kvp.Value;
                string key = kvp.Key;
                ComparisonResult result = delta[key];

                foreach (NodeCompareItem item in result.Items)
                {
                    if (item.IMNode is GraphNode && item.IMNode.MetaObject.Class == "Job")
                    {
                        //number of child [job]positions
                        List<b.ObjectAssociation> required = new List<MetaBuilder.BusinessLogic.ObjectAssociation>();
                        //with an employee
                        List<b.ObjectAssociation> occupied = new List<MetaBuilder.BusinessLogic.ObjectAssociation>();
                        //without an employee
                        int available = 0;

                        foreach (b.ObjectAssociation childAss in d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(item.IMNode.MetaObject.pkid, item.IMNode.MetaObject.MachineName))
                        {
                            if (childAss.CAid == jobposition && childAss.Machine != "DB-TRIGGER")
                            {
                                required.Add(childAss);
                                int positionpersonCount = 0;
                                foreach (b.ObjectAssociation childAsschild in d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(childAss.ChildObjectID, childAss.ChildObjectMachine))
                                {
                                    if (childAsschild.CAid == positionperson && childAsschild.Machine != "DB-TRIGGER")
                                    {
                                        if (positionpersonCount == 0) //dont count multiple emplyees for a single position
                                            occupied.Add(childAsschild);
                                        positionpersonCount += 1;
                                    }
                                }
                                if (positionpersonCount > 1)
                                {
                                    //there is a problem with a position which does not affect the count
                                    ComparisonResult positionResult = delta[childAss.ChildObjectID.ToString() + childAss.ChildObjectMachine];
                                    if (positionResult != null) //the position is on the diagram
                                    {
                                        foreach (NodeCompareItem positionitem in positionResult.Items)
                                        {
                                            positionitem.IMNode.RequiresAttention = true;
                                            positionitem.IMNode.MetaObject.SetWithoutChange("GapType", "Change");
                                        }
                                    }
                                    //else
                                    //{
                                    //    //the position is not on the diagram
                                    //}
                                }
                            }
                        }
                        available = required.Count - occupied.Count;
                        item.IMNode.MetaObject.Set("TotalRequired", required.Count.ToString());
                        item.IMNode.MetaObject.Set("TotalOccupied", occupied.Count.ToString());
                        item.IMNode.MetaObject.Set("TotalAvailable", available.ToString()); //available is negative
                        //item.IMNode.BindToMetaObjectProperties();
                    }
                }
            }
        }

        //public void CompareSelection(IGoCollection col)
        //{
        //    CompareSelection(col, false);
        //    //DoComparison(col, false);
        //}

        //public void RefreshSelection(IGoCollection col)
        //{
        //    CompareSelection(col, true);
        //}

        protected override void Dispose(bool disposing)
        {
            //collection.Clear();
            //collection = null;
            DiffData.Clear();
            diffData = null;
            //MyView = null;
        }
    }

    public class MetaComparer
    {
        private GraphView myView;
        public GraphView MyView
        {
            get { return myView; }
            set { myView = value; }
        }

        private VisualIndicatorController indicatorController = new VisualIndicatorController();

        #region Internal Classes

        //public enum ComparisonResultType
        //{
        //    Not_In_Database, Differs_From_DB, Equal, Not_On_Diagram
        //}

        public class ComparisonResult
        {
            private MetaBase metaObject;
            public MetaBase MetaObject
            {
                get { return metaObject; }
                set { metaObject = value; }
            }
            private List<NodeCompareItem> items;
            public List<NodeCompareItem> Items
            {
                get { return items; }
                set { items = value; }
            }
        }

        public class NodeCompareItem
        {
            private MetaBase objectInDB;
            public MetaBase ObjectInDB
            {
                get { return objectInDB; }
                set { objectInDB = value; }
            }

            private IMetaNode imNode;
            public IMetaNode IMNode
            {
                get { return imNode; }
                set { IMNode = value; }
            }
            private bool isCollapsible;
            public bool IsCollapsible
            {
                get { return isCollapsible; }
                set { isCollapsible = value; }
            }

            public NodeCompareItem(IMetaNode imn, bool collapsible)
            {
                this.imNode = imn;
                this.isCollapsible = collapsible;
            }
            private bool processed;
            public bool Processed
            {
                get { return processed; }
                set { processed = value; }
            }
        }

        #endregion

        public Dictionary<MetaBase, List<NodeCompareItem>> UpdateShallowCopies(Dictionary<MetaBase, List<NodeCompareItem>> ExistingNodes)
        {
#if DEBUG
            TTrace.Debug.Indent("Updating Shallow Copies");
#endif
            Dictionary<string, List<NodeCompareItem>> itemsWithSameKey = new Dictionary<string, List<NodeCompareItem>>();
#if DEBUG
            TTrace.Debug.Indent("Finding items with same keys");
#endif
            #region Find items with same keys
            foreach (KeyValuePair<MetaBase, List<NodeCompareItem>> kvp in ExistingNodes)
            {
                MetaKey mkey = new MetaKey();
                mkey.PKID = kvp.Key.pkid;
                mkey.Machine = kvp.Key.MachineName;
                if (!itemsWithSameKey.ContainsKey(mkey.ToString()))
                    itemsWithSameKey.Add(mkey.ToString(), new List<NodeCompareItem>());
                foreach (NodeCompareItem node in kvp.Value)
                {
                    if (!itemsWithSameKey[mkey.ToString()].Contains(node))
                        itemsWithSameKey[mkey.ToString()].Add(node);
                }
            }
            #endregion
#if DEBUG
            TTrace.Debug.UnIndent();
#endif
            GraphViewController gvc = new GraphViewController();
            gvc.MyView = MyView;
            #region Set MetaObjects to the first one's instance
            foreach (KeyValuePair<string, List<NodeCompareItem>> kvp in itemsWithSameKey)
            {
                if (kvp.Value.Count > 1)
                {
#if DEBUG
                    TTrace.Debug.Send("Shallow Item", kvp.Value[0].IMNode.MetaObject.ToString() + " (" + (kvp.Value.Count - 1).ToString() + ")");
#endif
                    gvc.AddShadows(kvp.Value[0].IMNode as GoNode);
                    NodeCompareItem ncItemBase = kvp.Value[0];
                    MetaBase mbUseThis = ncItemBase.IMNode.MetaObject;
                    for (int i = 1; i < kvp.Value.Count; i++)
                    {
                        kvp.Value[i].IMNode.MetaObject = mbUseThis;
                        kvp.Value[i].IMNode.HookupEvents();
                        gvc.AddShadows(kvp.Value[i].IMNode as GoNode);
                    }
                }
            }
            #endregion
#if DEBUG
            TTrace.Debug.UnIndent();
#endif
            return ConvertToMBaseDictionaryFromKey(itemsWithSameKey);
        }
        private Dictionary<MetaBase, List<NodeCompareItem>> ConvertToMBaseDictionaryFromKey(Dictionary<string, List<NodeCompareItem>> metaKeyDictionary)
        {
#if DEBUG
            TTrace.Debug.Send("Converting back to MetaBase");
#endif
            Dictionary<MetaBase, List<NodeCompareItem>> retval = new Dictionary<MetaBase, List<NodeCompareItem>>();
            foreach (KeyValuePair<string, List<NodeCompareItem>> kvp in metaKeyDictionary)
            {
                retval.Add(kvp.Value[0].IMNode.MetaObject, new List<NodeCompareItem>());
                foreach (NodeCompareItem ncItem in kvp.Value)
                {
                    retval[kvp.Value[0].IMNode.MetaObject].Add(ncItem);
                }
            }
#if DEBUG
            TTrace.Debug.Send("Converting back to MetaBase END");
#endif
            return retval;
        }
        public Dictionary<MetaBase, List<NodeCompareItem>> GetExistingNodes(IGoCollection col)
        {
            Dictionary<MetaBase, List<NodeCompareItem>> retval = new Dictionary<MetaBase, List<NodeCompareItem>>();
            foreach (GoObject o in col)
            {
                #region Normal Nodes
                if (o is IMetaNode)
                {
                    IMetaNode imnode = o as IMetaNode;
                    if (imnode.MetaObject != null)
                    {
                        NodeCompareItem nco = new NodeCompareItem(imnode, imnode is CollapsibleNode);
                        if (!retval.ContainsKey(imnode.MetaObject))
                            retval.Add(imnode.MetaObject, new List<NodeCompareItem>());
                        retval[imnode.MetaObject].Add(nco);
                    }
                }
                #endregion

                #region Groups/Collections
                if (o is GoGroup)
                {
                    Dictionary<MetaBase, List<NodeCompareItem>> innerObjects = GetExistingNodes(o as GoGroup);
                    foreach (KeyValuePair<MetaBase, List<NodeCompareItem>> kvp in innerObjects)
                    {
                        if (!retval.ContainsKey(kvp.Key))
                            retval.Add(kvp.Key, new List<NodeCompareItem>());
                        foreach (NodeCompareItem imInner in kvp.Value)
                        {
                            retval[kvp.Key].Add(imInner);
                        }
                    }
                }
                #endregion
            }
            return retval;
        }

        public void CompareSelection(IGoCollection col, bool AutoRefresh)
        {
            Loader.FlushDataViews();

            if (myView != null)
                myView.StartTransaction();
            //myView.BeginUpdate();
            Dictionary<MetaBase, List<NodeCompareItem>> compareItems = GetExistingNodes(col);

            #region TracingOnly
            //#if DEBUG
            //            TTrace.Debug.Indent("Found the following MetaNodes");

            //            foreach (KeyValuePair<MetaBase, List<NodeCompareItem>> kvp in compareItems)
            //            {
            //                if (kvp.Key.ToString() != null)
            //                {
            //                    TTrace.Debug.Indent(kvp.Key.ToString(), kvp.Key._ClassName);
            //                    foreach (NodeCompareItem nci in kvp.Value)
            //                    {
            //                        TTrace.Debug.Send(nci.IMNode.MetaObject.ToString(), nci.IsCollapsible.ToString());
            //                    }
            //                    TTrace.Debug.UnIndent();
            //                }
            //            }
            //            TTrace.Debug.UnIndent();
            //#endif
            #endregion TracingOnly

            compareItems = UpdateShallowCopies(compareItems);

            //remove indicators on items to compare
            indicatorController.RemoveChangedIndicators(col, true);

            FindDelta(compareItems, AutoRefresh);
            if (Core.Variables.Instance.ValidateVersionControl)
                CompareLinks(myView.Document);
            myView.UpdateView();
            myView.EndUpdate();
            myView.Document.UpdateViews();
            if (myView != null)
                myView.FinishTransaction("Refresh");
        }

        private LinkComparer lComparer;
        public void CompareLinks(IGoCollection col)
        {
            lComparer = new LinkComparer();
            lComparer.Collection = col;
            lComparer.DoWork += new DoWorkEventHandler(lComparer_DoWork);
            lComparer.RunWorkerCompleted += new RunWorkerCompletedEventHandler(lComparer_RunWorkerCompleted);
            lComparer.RunWorkerAsync();
        }
        //Setting color in completion event does not work when refreshing manually and then whenver after than you open a diagram again
        private void lComparer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //myView.BeginUpdate();
            //if (lComparer.MFDLinks != null)
            //{
            //    MyView.Document.SkipsUndoManager = true;
            //    MyView.StartTransaction();
            //    foreach (QLink q in lComparer.MFDLinks)
            //    {
            //        //QLink myLink = q;
            //        q.RealLink.Pen = new System.Drawing.Pen(System.Drawing.Color.Red);
            //    }
            //    MyView.Document.FinishTransaction("MFDLINKS Compared");
            //    MyView.Document.SkipsUndoManager = false;
            //}
            //if (lComparer.NotInDBLinks != null)
            //{
            //    MyView.Document.SkipsUndoManager = true;
            //    MyView.StartTransaction();
            //    foreach (QLink q in lComparer.NotInDBLinks)
            //    {
            //        //QLink myLink = q;
            //        q.RealLink.Pen = new System.Drawing.Pen(System.Drawing.Color.Orange);
            //    }
            //    MyView.Document.FinishTransaction("NOTINDBLINKS Compared");
            //    MyView.Document.SkipsUndoManager = false;
            //}
            //if (lComparer.NotInModelLinks != null)
            //{
            //    MyView.Document.SkipsUndoManager = true;
            //    MyView.StartTransaction();
            //    foreach (QLink q in lComparer.NotInModelLinks)
            //    {
            //        //QLink myLink = q;
            //        q.RealLink.Pen = new System.Drawing.Pen(System.Drawing.Color.Red);
            //        q.RealLink.Pen.DashStyle = DashStyle.DashDotDot;
            //    }
            //    MyView.Document.FinishTransaction("NOTINMODEL LINKS Compared");
            //    MyView.Document.SkipsUndoManager = false;
            //}
            //myView.EndUpdate();
        }
        private void lComparer_DoWork(object sender, DoWorkEventArgs e)
        {
            lComparer.CompareLinks();
        }

        private Dictionary<string, ComparisonResult> FindDelta(Dictionary<MetaBase, List<NodeCompareItem>> compareItems, bool update)
        {
            diffData = new Dictionary<string, List<DifferingData>>();

            Dictionary<string, ComparisonResult> delta = new Dictionary<string, ComparisonResult>();
            Dictionary<string, List<MetaKey>> itemKeys = new Dictionary<string, List<MetaKey>>();

            #region Prepare existing nodes for comparison - we want to quickly access MB's (and nodes) by using the MB's key

            foreach (KeyValuePair<MetaBase, List<NodeCompareItem>> kvp in compareItems)
            {
                MetaBase mb = kvp.Key;
                //also add keys to collection that will be sent to Loader
                if (!itemKeys.ContainsKey(mb._ClassName))
                    itemKeys.Add(mb._ClassName, new List<MetaKey>());
                MetaKey mkey = new MetaKey();
                mkey.PKID = mb.pkid;
                mkey.Machine = mb.MachineName;
                itemKeys[mb._ClassName].Add(mkey);

                ComparisonResult result = new ComparisonResult();
                result.Items = new List<NodeCompareItem>();
                result.MetaObject = mb;
                foreach (NodeCompareItem item in kvp.Value)
                {
                    result.Items.Add(item);
                }
                if (!delta.ContainsKey(mkey.ToString()))
                {
                    delta.Add(mkey.ToString(), result);
                }

            }

            #endregion

            // Get resultset
            List<MetaBase> metasFromDB = Loader.GetFromProvider(itemKeys, false);

            if (metasFromDB.Count == 0)
            {
                //all items in selection are not in database
                foreach (KeyValuePair<MetaBase, List<NodeCompareItem>> i in compareItems)
                {
                    foreach (NodeCompareItem item in i.Value)
                    {
                        item.ObjectInDB = null;
                        //item.IMNode.MetaObject.IsInDatabase = false;
                        item.IMNode.MetaObject.State = VCStatusList.None;
                        //item.IMNode.BindToMetaObjectProperties();
                        //item.IMNode.FireMetaObjectChanged(i, new EventArgs());
                        indicatorController.AddChangedIndicator("Not in Database", System.Drawing.Color.Red, item.IMNode as GoGroup);
                    }
                }
            }

            // Convert to dictionary again so we dont have to loop through all the items
            Dictionary<string, MetaBase> dictMetasFromDB = new Dictionary<string, MetaBase>();
            foreach (MetaBase mbDB in metasFromDB)
            {
                dictMetasFromDB.Add(mbDB.pkid.ToString() + mbDB.MachineName, mbDB);
            }

            #region Compare/Update Items
            foreach (KeyValuePair<string, MetaBase> kvp in dictMetasFromDB)
            {
                MetaBase mbase = kvp.Value;
                string key = kvp.Key;

                ComparisonResult result = delta[key];
                if (update)
                {
                    foreach (NodeCompareItem ncitem in result.Items)
                    {
                        ncitem.IMNode.MetaObject = mbase;
                        ncitem.IMNode.HookupEvents();
                        //3 September 2013 : Attempt to make an item in database present that fact for duplicate marking
                        //ncitem.IMNode.MetaObject.IsInDatabase = true;
                        //ncitem.IMNode.BindToMetaObjectProperties();
                    }
                }
                else
                {
                    #region Compare only
                    MetaBase mbaseOld = result.MetaObject;
                    //List<PropertyInfo> props = mbase.GetMetaPropertyList(true);
                    bool changes = false;

                    #region Iterate and Compare Properties
                    foreach (PropertyInfo pinfo in mbase.GetMetaPropertyList(true))
                    {
                        object[] aAttributes = pinfo.GetCustomAttributes(typeof(BrowsableAttribute), false);

                        if (aAttributes != null && aAttributes.Length > 0)
                        {
                            //if (pinfo.Name.ToLower() != "isindatabase" && pinfo.Name.ToLower() != "workspacetypeid")
                            // has browsable attribute
                            if (mbaseOld.Get(pinfo.Name) != null && mbase.Get(pinfo.Name) != null)
                            {
                                string s1 = mbaseOld.Get(pinfo.Name).ToString().Trim();
                                string s2 = mbase.Get(pinfo.Name).ToString().Trim();

                                switch (pinfo.Name.ToLower())
                                {
                                    case "state":
                                        //Logstate discrepancy
                                        //if (s1 != s2)
                                        //    if (s1.Length > 0 && s2.Length > 0)
                                        //        Log.WriteLog("{OLD} " + mbaseOld.ToString() + " (state: " + s1 + ") does not match {NEW} " + mbase.ToString() + " (state: " + s2 + ")");
                                        break;
                                    case "userid":
                                        //Loguser discrepancy !! this is causing mismatch of objects
                                        //if (s1 != s2)
                                        //    if (s1.Length > 0 && s2.Length > 0)
                                        //        Log.WriteLog("{OLD} " + mbaseOld.ToString() + " (userid: " + s1 + ") does not match {NEW} " + mbase.ToString() + " (userid: " + s2 + ")");
                                        break;
                                    case "vcuser":
                                        //Loguser discrepancy !! this is causing mismatch of objects
                                        //if (s1 != s2)
                                        //    if (s1.Length > 0 && s2.Length > 0)
                                        //        Log.WriteLog("{OLD} " + mbaseOld.ToString() + " (vcuser: " + s1 + ") does not match {NEW} " + mbase.ToString() + " (vcuser: " + s2 + ")");
                                        break;
                                    case "isindatabase":
                                        //ignore
                                        break;
                                    case "workspacetypeid":
                                        //ignore
                                        break;
                                    case "workspacename":
                                        if (Core.Variables.Instance.CompareWorkspacesForObjects)
                                        {
                                            if (s1 != s2)
                                            {
                                                if (s1.Length > 0 && s2.Length > 0)
                                                    if (s1.ToLower() != "xor")
                                                    {
                                                        changes = true;
                                                        //string sa = "Workspace Compare - [OLD] " + mbaseOld.ToString() + " ( " + s1 + ") does not match [NEW] " + mbase.ToString() + " ( " + s2 + ") CHANGES MARKED TRUE";
                                                        //Log.WriteLog(sa);
                                                        //MessageBox.Show(this,sa);
                                                    }
                                            }
                                        }
                                        break;
                                    default:
                                        //all relevant properties
                                        if (s1 != s2)
                                        {
                                            if (s1.Length > 0 && s2.Length > 0)
                                                if (s1.ToLower() != "xor")
                                                {
                                                    changes = true;
                                                    //string s = "Property - [OLD] " + pinfo.Name.ToString() + " - " + mbaseOld.ToString() + " ( " + s1 + ") does not match [NEW]  " + mbase.ToString() + " ( " + s2 + ") CHANGES MARKED TRUE";
                                                    //Log.WriteLog(s);
                                                    //MessageBox.Show(this,s);
                                                }
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                //if either diagramObject or dbObject is null we come here
                                //For when the property is null and the other is not
                                //changes = mbaseOld.Get(pinfo.Name) != mbase.Get(pinfo.Name);
                                //continue;
                            }
                        }

                    }
                    #endregion

                    foreach (NodeCompareItem ncitem in result.Items)
                    {
                        ncitem.ObjectInDB = mbase;
                        ncitem.IMNode.HookupEvents();
                        ncitem.Processed = true;
                        if (changes)
                        {
                            //Log.WriteLog("Changes detected");
                            if (ncitem.IMNode is GoGroup)
                            {
                                indicatorController.AddChangedIndicator("Differs from DB", System.Drawing.Color.Red, ncitem.IMNode as GoGroup);
                                EDifferenceType edtype = EDifferenceType.Meta_Properties_Differ;

                                DifferingData nodeData = new DifferingData();
                                nodeData.DifferenceType = edtype;
                                nodeData.Node = ncitem.IMNode;
                                nodeData.ObjectInDatabase = ncitem.ObjectInDB;

                                GoGroup grp = ncitem.IMNode as GoGroup;
                                if (grp.Parent is RepeaterSection)
                                {
                                    nodeData.Section = grp.Parent as RepeaterSection;
                                }
                                string nodeKey = ncitem.IMNode.MetaObject.pkid.ToString() + ncitem.IMNode.MetaObject.MachineName;
                                if (!diffData.ContainsKey(nodeKey))
                                {
                                    diffData.Add(nodeKey, new List<DifferingData>());
                                }
                                diffData[nodeKey].Add(nodeData);
                            }
                        }
                        else
                        {
                            //Log.WriteLog("No Changes Found");
                        }
                    }
                    #endregion
                }
            }

            if (!update)
            {
                foreach (KeyValuePair<string, ComparisonResult> kvp in delta)
                {
                    ComparisonResult result = delta[kvp.Key];
                    foreach (NodeCompareItem ncitem in result.Items)
                    {
                        if (!ncitem.Processed)
                            if (ncitem.IMNode is GoGroup)
                            {
                                indicatorController.AddChangedIndicator("Not in Database", System.Drawing.Color.Red, ncitem.IMNode as GoGroup);

                                EDifferenceType edtype = EDifferenceType.Not_in_DB;
                                DifferingData nodeData = new DifferingData();
                                nodeData.DifferenceType = edtype;
                                nodeData.Node = ncitem.IMNode;
                                nodeData.ObjectInDatabase = null;

                                GoGroup grp = ncitem.IMNode as GoGroup;
                                if (grp.Parent is RepeaterSection)
                                {
                                    nodeData.Section = grp.Parent as RepeaterSection;
                                }
                                string nodeKey = ncitem.IMNode.MetaObject.pkid.ToString() + ncitem.IMNode.MetaObject.MachineName;
                                if (!diffData.ContainsKey(nodeKey))
                                {
                                    diffData.Add(nodeKey, new List<DifferingData>());
                                }
                                diffData[nodeKey].Add(nodeData);
                            }
                    }
                }
            }
            RefreshComplexNodeData(delta, update);
            RefreshVCIndicators(delta, dictMetasFromDB);

            #endregion

            return delta;

        }
        private Dictionary<string, List<DifferingData>> diffData;
        public Dictionary<string, List<DifferingData>> DiffData
        {
            get { return diffData; }
        }

        private void RefreshVCIndicators(Dictionary<string, ComparisonResult> delta, Dictionary<string, MetaBase> dictMetasFromDB)
        {
            if (Variables.Instance.ValidateVersionControl)
            {
                VisualIndicatorController indicatorController = new VisualIndicatorController();
                foreach (KeyValuePair<string, MetaBase> kvp in dictMetasFromDB)
                {
                    MetaBase mbase = kvp.Value;
                    string key = kvp.Key;
                    ComparisonResult result = delta[key];

                    #region iterate through items
                    foreach (NodeCompareItem item in result.Items)
                    {
                        bool skipGen = false;
                        if (item.IMNode is GoGroup)
                        {
                            GoGroup n = item.IMNode as GoGroup;
                            n.Editable = true;
                            indicatorController.RemoveIndicators(n);
                            if (n is CollapsingRecordNodeItem)
                            {
                                CollapsingRecordNodeItem nodeItem = n as CollapsingRecordNodeItem;
                                if (nodeItem.IsHeader || nodeItem.Text == "DataView Elements")
                                {
                                    skipGen = true;
                                }
                            }
                            #region Update Indicators
                            if (!skipGen)
                            {
                                if (VCStatusTool.IsObsoleteOrMarkedForDelete(mbase) && mbase.IsInDatabase(Core.Variables.Instance.ClientProvider))
                                {
                                    if (mbase.State == VCStatusList.MarkedForDelete)
                                        indicatorController.AddChangedIndicator("Marked For Delete", System.Drawing.Color.Red, n);
                                    else
                                        indicatorController.AddIndicator("Obsolete", System.Drawing.Color.Red, n);

                                    n.Editable = false;
                                    n.Copyable = false;
                                }
                                else if (mbase.State == VCStatusList.CheckedIn || mbase.State == VCStatusList.CheckedOutRead || mbase.State == VCStatusList.Locked)
                                {
                                    indicatorController.AddIndicator(mbase.State.ToString(), System.Drawing.Color.Gray, n);
                                    if (n is IMetaNode)
                                    {
                                        IMetaNode node = n as IMetaNode;
                                        node.MetaObject = mbase;
                                        node.BindToMetaObjectProperties();
                                        node.HookupEvents();
                                    }
                                    n.Editable = false;
                                    n.Copyable = true;
                                }
                                else if (mbase.State == VCStatusList.Obsolete)
                                {
                                    indicatorController.AddIndicator("Obsolete", System.Drawing.Color.Gray, n);
                                    if (n is IMetaNode)
                                    {
                                        IMetaNode node = n as IMetaNode;
                                        node.MetaObject = mbase;
                                        node.BindToMetaObjectProperties();
                                        node.HookupEvents();
                                    }
                                    n.Editable = false;
                                    n.Copyable = true;
                                }
                                //ADDED TO AUTO UPDATE CHECKED OUT ITEMS
                                //else if (mbase.State == VCStatusList.CheckedOut)
                                //{
                                //    indicatorController.AddIndicator("Checked Out", System.Drawing.Color.Gray, n);
                                //    if (n is IMetaNode)
                                //    {
                                //        IMetaNode node = n as IMetaNode;
                                //        node.MetaObject = mbase;
                                //    }
                                //    n.Editable = true;
                                //}
                                //else if (mbase.State == VCStatusList.Locked)
                                //{
                                //    indicatorController.AddIndicator("Locked", System.Drawing.Color.Blue, n);
                                //    if (n is IMetaNode)
                                //    {
                                //        IMetaNode node = n as IMetaNode;
                                //        node.MetaObject = mbase;
                                //    }
                                //    n.Editable = false;
                                //    n.Copyable = false;
                                //}
                            }
                        }
                            #endregion
                    }
                    #endregion
                }
            }
        }

        private void RefreshComplexNodeData(Dictionary<string, ComparisonResult> delta, bool update)
        {
            Collection<CollapsingRecordNodeItemList> updatedLists = new Collection<CollapsingRecordNodeItemList>();
            foreach (KeyValuePair<string, ComparisonResult> kvp in delta)
            {
                ComparisonResult res = kvp.Value;
                foreach (NodeCompareItem ncitem in res.Items)
                {
                    if (ncitem.IsCollapsible)
                    {
                        CollapsibleNode cnode = ncitem.IMNode as CollapsibleNode;
                        if (cnode != null)
                        {
                            Collection<RepeaterSection> sections = cnode.RepeaterSections;
                            if (cnode.MetaObject != null && cnode.MetaObject.IsInDatabase(Core.Variables.Instance.ClientProvider))
                            {
                                if (VCStatusTool.UserHasControl(cnode.MetaObject))
                                {
                                    for (int r = 0; r < sections.Count; r++)
                                    {
                                        Collection<MetaBase> mbExisting = new Collection<MetaBase>();
                                        cnode.Initializing = true;
                                        RepeaterSection sect = sections[r];
                                        if (cnode.BindingInfo != null)
                                        {
                                            foreach (RepeaterBindingInfo rbinfo in cnode.BindingInfo.RepeaterBindings)
                                            {
                                                //Sneaky Shape Fix?!
                                                if (cnode.BindingInfo.BindingClass == "DataTable")
                                                {
                                                    sect.Name = sect.Name.Replace("Attributes", "Columns");
                                                }
                                                //Sneaky Shape Fix?!

                                                if (sect.Name == rbinfo.RepeaterName)
                                                {
                                                    if (cnode.BindingInfo.BindingClass == "DataTable")
                                                        rbinfo.Association.ChildClass = "DataField";

                                                    #region Add existing items to a list

                                                    foreach (GoObject itemChild in sect)
                                                    {
                                                        if (itemChild is CollapsingRecordNodeItem)
                                                        {
                                                            CollapsingRecordNodeItem cItem = itemChild as CollapsingRecordNodeItem;
                                                            if ((!cItem.IsHeader))
                                                            {
                                                                if (cItem.MetaObject != null)// && cItem.MetaObject.IsInDatabase)
                                                                {
                                                                    //LOADING PARENT ITEMS AS WELL?
                                                                    mbExisting.Add(cItem.MetaObject);

                                                                    if (!(cItem.MetaObject.IsInDatabase(Core.Variables.Instance.ClientProvider)))
                                                                        indicatorController.AddChangedIndicator("Not in Database", System.Drawing.Color.Red, cItem);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    //Log.WriteLog(mbExisting.Count + " existing objects on " + cnode.MetaObject.ToString());

                                                    #endregion

                                                    #region Retrieve list of associated items
                                                    List<MetaBase> mbLoaded = new List<MetaBase>();

                                                    b.TList<b.ObjectAssociation> mos = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(cnode.MetaObject.pkid, cnode.MetaObject.MachineName);

                                                    if (rbinfo.Association.ID.ToString() == "666" || rbinfo.Association.ID.ToString() == "667" || rbinfo.Association.ID.ToString() == "668" || rbinfo.Association.ID.ToString() == "669" || rbinfo.Association.ID.ToString() == "670" || rbinfo.Association.ID.ToString() == "671" || rbinfo.Association.ChildClass == "DataField" || rbinfo.Association.ChildClass == "DataAttribute" || rbinfo.Association.ChildClass == "DataColumn" || rbinfo.Association.ChildClass == "Attribute")
                                                    {
                                                        if (rbinfo.Association.ChildClass == "DataColumn")
                                                            rbinfo.Association.ChildClass = "DataField";
                                                        else if (rbinfo.Association.ChildClass == "Attribute")
                                                            rbinfo.Association.ChildClass = "DataAttribute";

                                                        rbinfo.Association.ID = -1;
                                                        string kd = "";
                                                        if (sect.Name.Contains("Key"))
                                                            kd = "Key";
                                                        else if (sect.Name.Contains("Descrip"))
                                                            kd = "Descriptive";
                                                        //get it from the database!
                                                        TList<ClassAssociation> associations = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.Find("ParentClass='" + cnode.BindingInfo.BindingClass + "' AND ChildClass='" + rbinfo.Association.ChildClass);
                                                        foreach (ClassAssociation classAssociation in associations)
                                                        {
                                                            if (classAssociation.IsActive == true && classAssociation.AssociationTypeID == 4 && classAssociation.Caption.Contains(kd))
                                                            {
                                                                rbinfo.Association.ID = classAssociation.CAid;
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    mos.Filter = "CAid = " + rbinfo.Association.ID;
                                                    #endregion

                                                    #region Add to Loaded list (this is from DB)
                                                    if (mos.Count > 0)
                                                    {
                                                        for (int a = 0; a < mos.Count; a++)
                                                        {
                                                            b.ObjectAssociation oa = mos[a];
                                                            //27 November 2013 - skip triggers!
                                                            //24 January 2014 - Removed VCMachineID Check
                                                            if (oa.Machine != null && oa.Machine == "DB-TRIGGER")
                                                                continue;
                                                            if ((!VCStatusTool.IsObsoleteOrMarkedForDelete(oa)))
                                                            {
                                                                MetaBase mbChild = Loader.GetByID(rbinfo.Association.ChildClass, oa.ChildObjectID, oa.ChildObjectMachine);
                                                                if (!VCStatusTool.IsObsoleteOrMarkedForDelete(mbChild))
                                                                {
                                                                    if (!mbLoaded.Contains(mbChild))
                                                                        mbLoaded.Add(mbChild);
                                                                }
                                                            }
                                                        }
                                                    }

                                                    //Log.WriteLog(mbExisting.Count + " loaded objects for " + cnode.MetaObject.ToString());
                                                    #endregion

                                                    #region Compare the existing list of MBs to the loaded list of MBs
                                                    bool foundAll = true;

                                                    foreach (MetaBase mbX in mbExisting)
                                                    {
                                                        bool foundThisOne = false;
                                                        foreach (MetaBase mbL in mbLoaded)
                                                        {
                                                            if (mbL != null)
                                                            {
                                                                if (mbL.pkid == mbX.pkid && mbL.MachineName == mbX.MachineName)
                                                                {
                                                                    foundThisOne = true;
                                                                    if (update)
                                                                        mbL.CopyPropertiesToObject(mbX);
                                                                }
                                                            }
                                                        }
                                                        if (foundThisOne == false)
                                                        {
                                                            if (mbX.IsInDatabase(Core.Variables.Instance.ClientProvider))
                                                                foundAll = false;
                                                        }
                                                    }
                                                    #endregion

                                                    #region Compare the loaded list of MBs to the existing list of MBs
                                                    foreach (MetaBase mbL in mbLoaded)
                                                    {
                                                        bool foundThisOne = false;
                                                        if (mbL != null)
                                                        {
                                                            string k = mbL.pkid.ToString() + mbL.MachineName;
                                                            foreach (MetaBase mbX in mbExisting)
                                                            {
                                                                if (mbL.pkid == mbX.pkid && mbL.MachineName == mbX.MachineName)
                                                                {
                                                                    foundThisOne = true;
                                                                }
                                                            }

                                                            if (foundThisOne == false)
                                                            {
                                                                foundAll = false;
                                                                DifferingData ddata = new DifferingData();
                                                                ddata.DifferenceType = EDifferenceType.Not_on_Diagram;
                                                                ddata.ObjectInDatabase = mbL;
                                                                ddata.Section = sect;
                                                                if (!diffData.ContainsKey(k))
                                                                    diffData.Add(k, new List<DifferingData>());
                                                                diffData[k].Add(ddata);
                                                            }
                                                        }
                                                    }
                                                    #endregion

                                                    if (update)
                                                    {
                                                        #region Did not find all the items (either way). Clear section and load from list.
                                                        if (!foundAll)
                                                        {
                                                            //Log.WriteLog("Did not find all objects");
                                                            updatedLists.Add(sect);
                                                            sect.ClearChildItems(); //NO REMOVE THE INCORRECT ONES AND ADD THE CORRECT ONES?!?!
                                                            foreach (MetaBase mbL in mbLoaded)
                                                            {
                                                                if (mbL.ToString() != null && mbL.ToString().Length > 0)
                                                                {
                                                                    CollapsingRecordNodeItem item = sect.AddItemFromCode() as CollapsingRecordNodeItem;
                                                                    item.MetaObject = mbL;
                                                                    item.BindToMetaObjectProperties();
                                                                    item.Editable = true;
                                                                    item.HookupEvents();
                                                                }
                                                            }
                                                            GoNodeLinkEnumerator linkEnum = cnode.Links.GetEnumerator();
                                                            while (linkEnum.MoveNext())
                                                            {
                                                                if (linkEnum.Current is QLink)
                                                                {
                                                                    QLink slink = linkEnum.Current as QLink;
                                                                    slink.CalculateRoute();
                                                                }
                                                                else
                                                                {
                                                                    GoLink link = linkEnum.Current as GoLink;
                                                                    if (link != null)
                                                                        link.CalculateRoute();
                                                                }
                                                            }
                                                        }
                                                        #endregion
                                                    }
                                                }
                                            }
                                        }
                                        cnode.ForceComputeBounds();
                                        cnode.Initializing = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (update)
            {
                foreach (CollapsingRecordNodeItemList list in updatedLists)
                {
                    list.LayoutChildren(null);
                    list.ForceComputeBounds();
                    list.Parent.LayoutChildren(null);
                    list.ParentList.LayoutChildren(null);
                    if (list.ParentList is CollapsingRecordNodeItemList && list.ParentList != list)
                    {
                        CollapsingRecordNodeItemList lparent = list.ParentList as CollapsingRecordNodeItemList;
                        lparent.ForceComputeBounds();
                    }
                    //list.Parent.ForceComputeBounds();
                    if (list.ParentNode is CollapsibleNode)
                    {
                        CollapsibleNode cnode = list.ParentNode as CollapsibleNode;
                        cnode.ForceComputeBounds();
                    }
                }
            }
        }

        public void CompareSelection(IGoCollection col)
        {
            CompareSelection(col, false);
        }

        public void RefreshSelection(IGoCollection col)
        {
            CompareSelection(col, true);
        }
    }

    public class DifferingData
    {
        public DifferingData()
        {
            DifferenceType = EDifferenceType.Equal;
        }
        private RepeaterSection section;
        public RepeaterSection Section
        {
            get { return section; }
            set { section = value; }
        }

        public IMetaNode ParentNode
        {
            get
            {
                if (Node is IMetaNode)
                {
                    GoNode n = Node as GoNode;
                    return n.ParentNode as IMetaNode;
                }
                return null;
            }
        }

        public MetaBase ParentMetaObject
        {
            get
            {
                if (Node is IMetaNode)
                {
                    GoNode n = Node as GoNode;
                    IMetaNode parentNode = n.ParentNode as IMetaNode;
                    return parentNode.MetaObject;
                }
                return null;
            }
        }
        private IMetaNode node;
        public IMetaNode Node
        {
            get { return node; }
            set { node = value; }
        }
        private MetaBase objectOnDiagram;
        public MetaBase ObjectOnDiagram
        {
            get
            {
                if (node != null)
                    return node.MetaObject;
                else
                    return objectOnDiagram;
            }
            set
            {
                if (node != null)
                    node.MetaObject = value;
                else
                    objectOnDiagram = value;
            }
        }

        private MetaBase objectInDatabase;
        public MetaBase ObjectInDatabase
        {
            get { return objectInDatabase; }
            set { objectInDatabase = value; }
        }
        private EDifferenceType differenceType;
        public EDifferenceType DifferenceType
        {
            get { return differenceType; }
            set { differenceType = value; }
        }

    }
    public enum EDifferenceType
    {
        Equal, Not_in_DB, Not_on_Diagram, Meta_Properties_Differ
    }

    public class DifferingLinkData
    {
        private QLink objectOnDiagram;
        public QLink ObjectOnDiagram
        {
            get { return objectOnDiagram; }
            set { objectOnDiagram = value; }
        }

        private QLink objectInDatabase;
        public QLink ObjectInDatabase
        {
            get
            {
                if (objectInDatabase == null && DBFromObject != null && DBToObject != null)
                {
                    //objectInDatabase = QLink.CreateLink();
                }
                return objectInDatabase;
            }
            set { objectInDatabase = value; }
        }

        private EDifferenceType difference;
        public EDifferenceType Difference
        {
            get { return difference; }
            set { difference = value; }
        }

        private MetaBase dbFromObject;
        public MetaBase DBFromObject
        {
            get { return dbFromObject; }
            set { dbFromObject = value; }
        }
        private MetaBase dbToObject;
        public MetaBase DBToObject
        {
            get { return dbToObject; }
            set { dbToObject = value; }
        }
        private LinkAssociationType dbAssociationType;
        public LinkAssociationType DBAssociationType
        {
            get { return dbAssociationType; }
            set { dbAssociationType = value; }
        }
    }

    public class LinkComparer : BackgroundWorker
    {
        public Collection<DifferingLinkData> ChangedLinks;

        public Collection<QLink> MFDLinks;
        public Collection<QLink> NotInDBLinks;
        public Collection<QLink> NotInModelLinks;
        public Collection<DifferingLinkData> RemovedFromModelLinks;
        System.Drawing.Pen NotInDbPen = null;
        public void CompareLinks()
        {
            MetaBuilder.BusinessFacade.MetaHelper.AssociationHelper assHelper = new AssociationHelper();

            TList<GraphFile> files = null;
            NormalDiagram diagram = (Collection as NormalDiagram);
            if (Variables.Instance.CompareLinks && diagram != null)
                files = d.DataRepository.GraphFileProvider.Find("OriginalFileUniqueID = '" + diagram.VersionManager.CurrentVersion.OriginalFileUniqueIdentifier.ToString() + "'");
            //TList<b.GraphFileAssociation> fileAssociations = null;
            TList<b.ObjectAssociation> fileAssociations = null;
            if (Variables.Instance.CompareLinks && files != null)
            {
                files.Filter = "IsActive = true";
                if (files.Count > 0)
                {
                    //fileAssociations = d.DataRepository.GraphFileAssociationProvider.GetByGraphFileIDGraphFileMachine(files[0].pkid, files[0].Machine);
                    fileAssociations = d.DataRepository.ObjectAssociationProvider.GetByGraphFileIDGraphFileMachineFromGraphFileAssociation(files[0].pkid, files[0].Machine);
                }
            }
            ChangedLinks = new Collection<DifferingLinkData>();

            MFDLinks = new Collection<QLink>();
            NotInDBLinks = new Collection<QLink>();
            NotInModelLinks = new Collection<QLink>();
            RemovedFromModelLinks = new Collection<DifferingLinkData>();

            foreach (GoObject o in Collection)
            {
                QLink q = o as QLink;
                if (q == null)
                    continue;
                DockingForm.DockForm.UpdateStatusLabel("Compare Links");
                DifferingLinkData dLinkData = new DifferingLinkData();
                //try
                //{
                IMetaNode fromNode = q.FromNode == null ? null : q.FromNode as IMetaNode;
                IMetaNode toNode = q.ToNode == null ? null : q.ToNode as IMetaNode;
                if (fromNode == null || toNode == null)
                    continue;
                if (fromNode.MetaObject == null || toNode.MetaObject == null)
                {
                    Log.WriteLog("Comparing a link with null parent metaobject or null child metaobject was skipped - " + q.AssociationType.ToString());
                    continue;
                }
                dLinkData.ObjectOnDiagram = q;

                Association a = assHelper.GetAssociation(fromNode.MetaObject._ClassName, toNode.MetaObject._ClassName, (int)q.AssociationType);
                if (a == null)
                {
                    //unsupported by model
                    NotInModelLinks.Add(q);
                    if (NotInDbPen == null)
                    {
                        NotInDbPen = new System.Drawing.Pen(System.Drawing.Color.Red);
                        NotInDbPen.DashStyle = DashStyle.DashDotDot;
                    }
                    q.Pen = NotInDbPen;
                    q.NotInModel = true;
                    if (DockingForm.DockForm.GetCurrentGraphViewContainer() != null)
                        DockingForm.DockForm.GetCurrentGraphViewContainer().CreateInvalidLinkTask(q);
                }
                else
                {
                    b.ObjectAssociationKey oaKey = new ObjectAssociationKey(a.ID, fromNode.MetaObject.pkid, toNode.MetaObject.pkid, fromNode.MetaObject.MachineName, toNode.MetaObject.MachineName);
                    b.ObjectAssociation oaDiagram = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Get(oaKey);
                    b.ObjectAssociation oaDatabase = null;

                    if (VCStatusTool.IsObsoleteOrMarkedForDelete(oaDiagram))
                    {
                        MFDLinks.Add(q);
                        q.Pen = System.Drawing.Pens.Red;// new System.Drawing.Pen(System.Drawing.Color.Red);
                        //It has also changed though?
                    }
                    //else
                    //{
                    if (Variables.Instance.CompareLinks)
                    {
                        bool found = false;
                        bool foundOther = false;
                        if (fileAssociations != null)
                        {
                            foreach (b.ObjectAssociation association in fileAssociations)
                            {
                                if (association.ObjectID != fromNode.MetaObject.pkid || association.ChildObjectID != toNode.MetaObject.pkid)
                                    continue;
                                //UNCHANGED : is this association present in the previous graphfile?
                                if (oaDiagram != null && oaDiagram.CAid == a.ID)
                                {
                                    found = true;
                                    break;
                                }
                                else
                                {
                                    foundOther = true;
                                    oaDatabase = association;
                                }
                            }
                        }
                        if ((!found && oaDiagram != null) || (foundOther && !found))
                        {
                            //The association is in the database but is not on the this diagram
                            if (fileAssociations != null && fileAssociations.Count >= 0 && oaDatabase != null)
                            {
                                //CHANGED : association present between these two objects but the associationtype(caID!=ID) differs for this diagram
                                dLinkData.ObjectInDatabase = q.Copy() as QLink;
                                dLinkData.ObjectInDatabase.AssociationType = (LinkAssociationType)assHelper.GetAssociationTypeForCAID(oaDatabase.CAid);
                                dLinkData.Difference = EDifferenceType.Meta_Properties_Differ;
                            }
                        }
                    }
                    //}
                    if (oaDiagram == null)
                    {
                        NotInDBLinks.Add(q);
                        q.Pen = System.Drawing.Pens.Orange;// new System.Drawing.Pen(System.Drawing.Color.Orange);
                    }
                    ChangedLinks.Add(dLinkData);
                }

                //}
                //catch
                //{
                //    //REMOVE LINK FROM DIAGRAM?
                //    //NotInModelLinks.Add(q);
                //    //q.Pen = new System.Drawing.Pen(System.Drawing.Color.Red);
                //}

#if DEBUG
                if (fileAssociations != null)
                {
                    //fileAssociations.Filter = "Machine <> 'DB-TRIGGER'";
                    List<DifferingLinkData> difLinkData = new List<DifferingLinkData>();
                    //run through all the links on this diagram
                    QLink link = o as QLink;
                    if (link != null && link.ToNode != null && link.FromNode != null)
                    {
                        DockingForm.DockForm.UpdateStatusLabel("Compare Links.");
                        foreach (b.ObjectAssociation fileAssociation in fileAssociations)
                        {
                            if (fileAssociation.Machine == "DB-TRIGGER")
                                continue;
                            MetaBase toObj = (link.ToNode as IMetaNode).MetaObject;
                            MetaBase fromObject = (link.FromNode as IMetaNode).MetaObject;
                            //if the associationtype and the to and from node metaobjects match with this association : found
                            if (toObj.pkid == fileAssociation.ChildObjectID && toObj.MachineName == fileAssociation.ChildObjectMachine && fromObject.pkid == fileAssociation.ObjectID && fromObject.MachineName == fileAssociation.ObjectMachine)
                            {
                                if (assHelper.GetAssociationTypeForCAID(fileAssociation.CAid) == (int)link.AssociationType)
                                {
                                    difLinkData.Clear();
                                    break;
                                }
                                DifferingLinkData ddata = new DifferingLinkData();
                                //need to make a new qlink out of nothing for display :()
                                ddata.DBAssociationType = (LinkAssociationType)assHelper.GetAssociationTypeForCAID(fileAssociation.CAid);
                                ddata.DBFromObject = null;
                                ddata.DBToObject = null;
                                difLinkData.Add(ddata);
                            }
                        }
                    }
                    else if (o is MetaBuilder.Graphing.Shapes.Nodes.Containers.ILinkedContainer)
                    {
                        DockingForm.DockForm.UpdateStatusLabel("Compare Links..");
                        foreach (MetaBuilder.Graphing.Shapes.Nodes.Containers.EmbeddedRelationship embeddedRel in (o as MetaBuilder.Graphing.Shapes.Nodes.Containers.ILinkedContainer).ObjectRelationships)
                        {
                            foreach (b.ObjectAssociation fileAssociation in fileAssociations)
                            {
                                if (fileAssociation.Machine == "DB-TRIGGER")
                                    continue;
                                if (embeddedRel.MyMetaObject.pkid == fileAssociation.ChildObjectID && embeddedRel.MyMetaObject.MachineName == fileAssociation.ChildObjectMachine && (o as MetaBuilder.Graphing.Shapes.Nodes.Containers.ILinkedContainer).MetaObject.pkid == fileAssociation.ObjectID && (o as MetaBuilder.Graphing.Shapes.Nodes.Containers.ILinkedContainer).MetaObject.MachineName == fileAssociation.ObjectMachine)
                                {
                                    if (fileAssociation.CAid == embeddedRel.MyAssociation.CAid)
                                    {
                                        difLinkData.Clear();
                                        break;
                                    }
                                    DifferingLinkData ddata = new DifferingLinkData();
                                    //need to make a new qlink out of nothing for display :()
                                    ddata.DBAssociationType = (LinkAssociationType)assHelper.GetAssociationTypeForCAID(fileAssociation.CAid);
                                    ddata.DBFromObject = null;
                                    ddata.DBToObject = null;
                                    difLinkData.Add(ddata);
                                }
                            }
                        }
                    }

                    foreach (DifferingLinkData var in difLinkData)
                        RemovedFromModelLinks.Add(var);
                }
#endif
            }

            RemovedFromModelLinks.ToString();
        }

        private IGoCollection collection;
        public IGoCollection Collection
        {
            get { return collection; }
            set { collection = value; }
        }

    }
}