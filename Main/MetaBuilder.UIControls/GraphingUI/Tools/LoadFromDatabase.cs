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
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MetaBuilder.UIControls.GraphingUI.Tools
{
    public class LoadFromDatabase
    {
        private Dictionary<MetaObjectKey, int> addedShapes;
        private GraphView myView;
        private GraphViewController viewController = null;
        public bool AutoLayout = false;

        public static Dictionary<string, IMetaNode> GetNodesInCollection(IGoCollection col)
        {
            Dictionary<string, IMetaNode> retval = new Dictionary<string, IMetaNode>();
            foreach (GoObject o in col)
            {
                if (o is IMetaNode)
                {
                    IMetaNode node = o as IMetaNode;
                    if (node.MetaObject != null)
                    {
                        if (node.MetaObject.pkid > 0)
                        {
                            string k = node.MetaObject.pkid.ToString() + node.MetaObject.MachineName;
                            if (!retval.ContainsKey(k))
                                retval.Add(k, node);
                        }
                    }
                }
                if (o is IGoCollection)
                {

                    Dictionary<string, IMetaNode> childNodes = GetNodesInCollection(o as IGoCollection);
                    foreach (KeyValuePair<string, IMetaNode> kvpChild in childNodes)
                    {
                        if (!retval.ContainsKey(kvpChild.Key))
                            retval.Add(kvpChild.Key, kvpChild.Value);
                    }
                }
            }
            return retval;
        }

        public void AddObjectsUsingStencil(GraphView mv, Dictionary<MetaObjectKey, MetaBase> objects)
        {
            if (objects.Count > 50)
            {
                //myView.ScrollPage(1f, 1f);
                DockingForm.DockForm.ThreadedConstructionLayout = AutoLayout;
                DockingForm.DockForm.UpdateStatusLabel("Initiating construction");
                Application.DoEvents();
                //Set this to none or normal to disable loading of columns and attributes
                DockingForm.DockForm.tImportOptions = DockingForm.TextImportOptions.Details;

                Dictionary<int, string> keys = new Dictionary<int, string>();
                foreach (KeyValuePair<MetaObjectKey, MetaBase> o in objects)
                {
                    if (!(keys.ContainsKey(o.Value.pkid)))
                        keys.Add(o.Value.pkid, o.Value.MachineName + ":" + o.Value.Class);
                }

                DockingForm.DockForm.ThreadedConstruction(keys);

                return;
            }

            viewController = new GraphViewController();
            viewController.MyView = mv;

            addedShapes = new Dictionary<MetaObjectKey, int>();
            dbcollection = new GoCollection();
            childshapes = new GoCollection();

            myView = mv;
            myView.Document.SkipsUndoManager = true;
            List<MetaKey> metaKeys = new List<MetaKey>();
            foreach (KeyValuePair<MetaObjectKey, MetaBase> mokey in objects)
            {
                MetaKey mk = new MetaKey();
                mk.PKID = mokey.Key.pkid;
                mk.Machine = mokey.Key.Machine;
                metaKeys.Add(mk);
            }
            if (metaKeys.Count > 0)
            {
                myView.ScrollPage(1f, 1f);
                ObjectAssociationArtefactContainer relatedObjects = Loader.GetAssociationContainersForMKeys(metaKeys);
                AddToDocument(objects, relatedObjects);

                DockingForm.DockForm.UpdateTotal(dbcollection.Count);
                //DockingForm.DockForm.UpdateStatusLabel("Loading Objects");
                //Application.DoEvents();
                int i = 1;
                foreach (GoObject oin in dbcollection)
                {
                    DockingForm.DockForm.UpdateStatusLabel("Loading Object " + i.ToString() + " of " + dbcollection.Count.ToString());
                    DockingForm.DockForm.ProgressUpdate(i);
                    /* if (oin is GraphNode)
                     {
                         GraphNode nodeToFind = oin as GraphNode;
                         foreach (GraphNode nExisting in nodesAlreadyInDocument)
                         {
                             if (nExisting.MetaObject != null)
                             {
                                 if (nExisting.MetaObject.pkid == nodeToFind.MetaObject.pkid &&
                                     nExisting.MetaObject.MachineName == nodeToFind.MetaObject.MachineName)
                                 {
                                     nodeToFind.MetaObject = nExisting.MetaObject;
                                     nodeToFind.HookupEvents();
                                     nodeToFind.Copyable = true;
                                     viewController.AddShadows(nodeToFind);
                                     viewController.AddShadows(nExisting);
                                 }
                             }
                         }
                     }*/
                    if (oin is IMetaBase)
                        oin.Shadowed = true;
                    myView.Document.Add(oin);
                    i += 1;
                    Application.DoEvents();
                }

                if (AutoLayout)
                {
                    DockingForm.DockForm.UpdateStatusLabel("Basic Tree Layout");
                    Application.DoEvents();
                    TreeLayoutDialog tld = new TreeLayoutDialog();
                    tld.DoLayout(myView, false);
                    tld.Dispose();
                }

                myView.ScrollPage(-1f, -1f);
                DockingForm.DockForm.ResetStatus();
            }

            //refresh all data loaded/will update indicators
            MetaComparer comp = new MetaComparer();
            comp.MyView = myView;
            comp.RefreshSelection(dbcollection);

            myView.Document.SkipsUndoManager = false;
        }
        public void AddObjectsFromDBUsingStencil(GraphView mv)
        {
            myView = mv;
            viewController = new GraphViewController();

            ObjectFinder ofinder = new ObjectFinder(false);
            ofinder.IncludeStatusCombo = true;
            ofinder.ExcludeStatuses.Add(VCStatusList.Obsolete);
            ofinder.ExcludeStatuses.Add(VCStatusList.MarkedForDelete);
            ofinder.LFDB = true;
            DialogResult res = ofinder.ShowDialog();
            AutoLayout = ofinder.AutoLayout;
            Application.DoEvents();

            if (res == DialogResult.OK)
            {
                DockingForm.DockForm.UpdateStatusLabel("Loading objects");
                Application.DoEvents();
                Dictionary<MetaObjectKey, MetaBase> objects = ofinder.SelectedObjects;
                AddObjectsUsingStencil(mv, objects);
            }
        }
        public MetaBase GetSingleObject()
        {
            ObjectFinder ofinder = new ObjectFinder(false);
            ofinder.IncludeStatusCombo = true;
            ofinder.AllowMultipleSelection = false;
            ofinder.ExcludeStatuses.Add(VCStatusList.Obsolete);
            ofinder.ExcludeStatuses.Add(VCStatusList.MarkedForDelete);
            ofinder.LFDB = true;
            DialogResult res = ofinder.ShowDialog();
            //AutoLayout = ofinder.AutoLayout;
            //Application.DoEvents();

            if (res == DialogResult.OK && ofinder.SelectedObjectsList != null && ofinder.SelectedObjectsList.Count > 0)
            {
                return ofinder.SelectedObjectsList[0];
            }

            return null;
        }
        public MetaBase GetSingleObject(string className)
        {
            ObjectFinder ofinder = new ObjectFinder(false);
            ofinder.IncludeStatusCombo = true;
            ofinder.AllowMultipleSelection = false;
            ofinder.LimitToClass = className;
            ofinder.ExcludeStatuses.Add(VCStatusList.Obsolete);
            ofinder.ExcludeStatuses.Add(VCStatusList.MarkedForDelete);
            ofinder.LFDB = true;
            DialogResult res = ofinder.ShowDialog();
            //AutoLayout = ofinder.AutoLayout;
            //Application.DoEvents();

            if (res == DialogResult.OK && ofinder.SelectedObjectsList != null && ofinder.SelectedObjectsList.Count > 0)
            {
                return ofinder.SelectedObjectsList[0];
            }

            return null;
        }

        private GoCollection dbcollection;
        private GoCollection childshapes;

        private List<string> linksAlreadyAdded;
        private void AddToDocument(Dictionary<MetaObjectKey, MetaBase> objects, ObjectAssociationArtefactContainer relatedObjects)
        {
            addedShapes = new Dictionary<MetaObjectKey, int>();
            linksAlreadyAdded = new List<string>();
            if (objects != null)
            {
                Dictionary<string, IMetaNode> nodesAlreadyInDocument = GetNodesInCollection(myView.Doc);

                myView.Document.SkipsUndoManager = true;
                List<string> classes = new List<string>();
                foreach (MetaBase mbase in objects.Values)
                {
                    if (mbase.Class == "DataView")
                    {
                        //WE OVERRIDE HERE BECAUSE WE HAVE 3 SHAPES FOR THE SAME CLASS...
                        string affix = "";
                        b.TList<b.ObjectAssociation> ass = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(mbase.pkid, mbase.MachineName);
                        foreach (b.ObjectAssociation a in ass)
                        {
                            //switch class
                            switch ((d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetBypkidMachine(a.ChildObjectID, a.ChildObjectMachine)).Class)
                            {
                                case "DataColumn":
                                    affix = "_Physical";
                                    break;
                                case "Attribute":
                                    affix = "_Logical";
                                    break;
                                default:
                                    break;
                            }

                            if (affix.Length > 0)
                                break; //Cancel looping
                        }

                        if (!classes.Contains("DataView" + affix))
                        {
                            classes.Add("DataView" + affix);
                        }
                    }
                    else
                    {
                        if (!classes.Contains(mbase._ClassName))
                        {
                            classes.Add(mbase._ClassName);
                        }
                    }
                }
                Dictionary<string, GraphNode> classShapes = new Dictionary<string, GraphNode>();
                Dictionary<string, GraphNode> shapes = new Dictionary<string, GraphNode>();
                GraphNode shapeToUse = null;

                #region GetStencil
                foreach (string s in classes)
                {
                    classShapes.Add(s, null);
                    if (classShapes[s] == null)
                        shapeToUse = GetShape(s);
                    else
                        shapeToUse = classShapes[s];

                    if (shapeToUse != null)
                    {
                        classShapes[s] = shapeToUse;
                    }
                    else
                    {
                        if (s != "Rationale")
                        {
                            DialogResult res = DialogResult.None;
                            while (classShapes[s] == null && res != DialogResult.Cancel)
                            {
                                res = OpenStencilForClass(s);
                                if (res == DialogResult.OK)
                                {
                                    classShapes[s] = GetShape(s) as GraphNode;
                                    if (classShapes[s] == null)
                                    {
                                        MessageBox.Show(DockingForm.DockForm, "Stencil does not contain shape for " + s, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                                else if (res == DialogResult.Cancel)
                                {
                                    // return;
                                }
                            }
                        }
                    }
                }
                #endregion

                Dictionary<string, GraphNode> newNodes = new Dictionary<string, GraphNode>();
                int counter = 0;
                List<MetaBase> objList = new List<MetaBase>();
                foreach (MetaBase mbase in objects.Values)
                {
                    if (!(objList.Contains(mbase)))
                        objList.Add(mbase);
                }

                for (int i = 0; i < objList.Count; i++)
                {
                    MetaBase mbase = objList[i];
                    if (mbase._ClassName == "Rationale")
                    {
                        if (!VCStatusTool.IsObsoleteOrMarkedForDelete(mbase))
                            AddRationale(null, mbase);
                    }
                    else
                    {
                        string affix = "";
                        if (mbase.Class == "DataView")
                        {
                            //WE OVERRIDE HERE BECAUSE WE HAVE 3 SHAPES FOR THE SAME CLASS...
                            //SO which shape should we pick?
                            //if you create a dataview with no columns...it will load as such! or at least attempt to
                            b.TList<b.ObjectAssociation> ass = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(mbase.pkid, mbase.MachineName);
                            foreach (b.ObjectAssociation a in ass)
                            {
                                //switch class
                                switch ((d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetBypkidMachine(a.ChildObjectID, a.ChildObjectMachine)).Class)
                                {
                                    case "DataColumn":
                                        affix = "_Physical";
                                        break;
                                    case "Attribute":
                                        affix = "_Logical";
                                        break;
                                    default:
                                        break;
                                }

                                if (affix.Length > 0)
                                    break; //Cancel looping
                            }
                        }
                        shapeToUse = classShapes[mbase._ClassName + affix];

                        if (shapeToUse != null)
                        {
                            MetaObjectKey key = new MetaObjectKey();
                            key.pkid = mbase.pkid;
                            key.Machine = mbase.MachineName;
                            if (!(addedShapes.ContainsKey(key)))
                            {
                                GraphNode parentShape = DropObject(shapeToUse, mbase, objects);
                                newNodes.Add(key.pkid.ToString() + key.Machine, parentShape);
                                string newitemkey = mbase.pkid.ToString() + mbase.MachineName;
                                if (!nodesAlreadyInDocument.ContainsKey(newitemkey))
                                    nodesAlreadyInDocument.Add(newitemkey, parentShape);
                                addedShapes.Add(key, 1);

                                AddChildShapes(mbase.pkid.ToString() + mbase.MachineName, parentShape, relatedObjects, nodesAlreadyInDocument);
                            }
                        }
                    }
                    counter++;
                }
                MetaBuilder.BusinessFacade.MetaHelper.AssociationHelper assHelper = Singletons.GetAssociationHelper();
                //  nodesAlreadyInDocument = GetNodesInCollection(myView.Doc);
                List<MetaKey> keys = new List<MetaKey>();
                List<IMetaNode> childNodes = new List<IMetaNode>();
                foreach (KeyValuePair<string, GraphNode> kvp in newNodes)
                {
                    ConnectNode(kvp.Value, relatedObjects, nodesAlreadyInDocument, assHelper);
                    // iterate contained nodes and add links for their items too
                    if (kvp.Value is IGoCollection)
                    {
                        Dictionary<string, IMetaNode> childnodes = GetNodesInCollection(kvp.Value);
                        foreach (KeyValuePair<string, IMetaNode> kvpC in childnodes)
                        {
                            IMetaNode imnodeChild = kvpC.Value;
                            MetaKey mk = new MetaKey();
                            mk.PKID = imnodeChild.MetaObject.pkid;
                            mk.Machine = imnodeChild.MetaObject.MachineName;
                            keys.Add(mk);
                            childNodes.Add(imnodeChild);
                        }
                    }
                }

                if (keys.Count > 0)
                {
                    ObjectAssociationArtefactContainer container = Loader.GetAssociationContainersForMKeys(keys);
                    foreach (IMetaNode n in childNodes)
                    {
                        ConnectNode(n, container, nodesAlreadyInDocument, assHelper);
                    }
                }

                // *********** TODO: Do DeepLoad for ComplexNodes now - and add to nodesAdded

                //  *********** Get a list of ALL graphnodes (including repeater ones)

                //QuickLayout();
                myView.Document.SkipsUndoManager = false;
            }
        }

        private Rationale AddRationale(GoObject parent, MetaBase mb)
        {
            Rationale nballoon = new Rationale();
            nballoon.Editable = true;
            nballoon.Movable = true;
            nballoon.Selectable = true;
            nballoon.Resizable = true;
            nballoon.MetaObject = mb;
            nballoon.BindToMetaObjectProperties();
            if (parent != null)
            {
                nballoon.Anchor = parent;
                if (parent.TopLevelObject is ArtefactNode)
                {
                    nballoon.Anchor = parent.TopLevelObject;
                }
                nballoon.Position = new PointF(parent.Right + 50, parent.Position.Y + 50);
            }

            nballoon.Reanchorable = true;
            nballoon.HookupEvents();
            myView.Document.Add(nballoon);
            return nballoon;
        }
        private void ConnectNode(IMetaNode n, ObjectAssociationArtefactContainer relatedObjects, Dictionary<string, IMetaNode> nodesAlreadyInDocument, MetaBuilder.BusinessFacade.MetaHelper.AssociationHelper assHelper)
        {
            if (n.MetaObject != null)
            {
                string key = n.MetaObject.pkid.ToString() + n.MetaObject.MachineName;
                foreach (KeyValuePair<string, AssociationContainer> container in relatedObjects.Containers)
                {
                    MetaKey mkey = container.Value.MBParent;
                    if (mkey.PKID == n.MetaObject.pkid && mkey.Machine == n.MetaObject.MachineName)
                    {
                        foreach (KeyValuePair<string, ChildObjectAssociationContainer> kvpX in container.Value.ChildAssocs)
                        {
                            ChildObjectAssociationContainer cont = kvpX.Value;
                            foreach (KeyValuePair<int, List<KeyObject>> koKVP in cont.AssociationsAndArtefacts)
                            {
                                //Console.WriteLine("CAID:" + koKVP.Key.ToString());
                                foreach (KeyObject ko in koKVP.Value)
                                {

                                    //Console.WriteLine("Linked to: " + ko.ClassName + ko.Key.ToString());
                                    string k = ko.Key.PKID.ToString() + ko.Key.Machine;
                                    if (ko.ClassName == "Rationale")
                                    {
                                        foreach (MetaBase mb in relatedObjects.LoadedItems)
                                        {
                                            if (mb.pkid == ko.Key.PKID && mb.MachineName == ko.Key.Machine)
                                            {
                                                //This is to add rationale to NODES not links
                                                //There is no way for us to know when a rationale is on a node
                                                if (!VCStatusTool.IsObsoleteOrMarkedForDelete(mb))
                                                    AddRationale(n as GoNode, mb);
                                            }
                                        }
                                    }
                                    if (nodesAlreadyInDocument.ContainsKey(k))
                                    {
                                        IMetaNode imn = nodesAlreadyInDocument[k];
                                        if (imn != null)
                                        {
                                            if (imn is GoNode)
                                            {
                                                GoNode nChild = imn as GoNode;
                                                int associationTypeID = assHelper.GetAssociationTypeForCAID(koKVP.Key);
                                                QLink l = AddLink(n as GoNode, nChild, associationTypeID, koKVP.Key);
                                                if (l != null)
                                                {
                                                    foreach (KeyObject art in ko.Artefacts)
                                                    {
                                                        //6 February 2013 - Cancel loading rationales?
                                                        if (art.ClassName == "Rationale")
                                                        {
                                                            //9 October 2013 - probably not the best idea
                                                            Rationale rat = new Rationale();
                                                            rat.BindingInfo = new BindingInfo();
                                                            rat.BindingInfo.BindingClass = art.ClassName;
                                                            rat.MetaObject = Loader.GetByID(art.ClassName, art.Key.PKID, art.Key.Machine);
                                                            if (VCStatusTool.IsObsoleteOrMarkedForDelete(rat.MetaObject))
                                                                continue;
                                                            rat.Location = new PointF(l.MidLabel.Location.X + 25, l.MidLabel.Location.Y - 25);
                                                            rat.Editable = true;
                                                            rat.Anchor = l.MidLabel;

                                                            rat.BindToMetaObjectProperties();
                                                            rat.HookupEvents();
                                                            myView.Document.Add(rat);
                                                            continue;
                                                        }

                                                        ArtefactNode nArt = new ArtefactNode();
                                                        nArt.BindingInfo = new BindingInfo();
                                                        nArt.BindingInfo.BindingClass = art.ClassName;
                                                        nArt.HookupEvents();
                                                        foreach (MetaBase mb in relatedObjects.LoadedItems)
                                                        {
                                                            if (mb.pkid == art.Key.PKID && mb.MachineName == art.Key.Machine)
                                                            {
                                                                nArt.MetaObject = mb;
                                                                nArt.BindToMetaObjectProperties();
                                                            }
                                                        }

                                                        if (nArt.MetaObject == null)
                                                        {
                                                            nArt.MetaObject = Loader.GetByID(art.ClassName, art.Key.PKID, art.Key.Machine);
                                                            nArt.BindToMetaObjectProperties();
                                                        }
                                                        if (VCStatusTool.IsObsoleteOrMarkedForDelete(nArt.MetaObject))
                                                            continue;

                                                        nArt.Location = l.Center;
                                                        nArt.Editable = !(nArt.MetaObject.State == VCStatusList.CheckedOutRead);
                                                        if (nArt.Editable)
                                                            nArt.Editable = !(nArt.MetaObject.State == VCStatusList.Locked);
                                                        if (nArt.Editable)
                                                            nArt.Editable = !(nArt.MetaObject.State == VCStatusList.CheckedIn);

                                                        myView.Document.Add(nArt);
                                                        GoGroup grp = l.MidLabel as GoGroup;
                                                        foreach (GoObject o in grp)
                                                        {
                                                            if (o is FishLinkPort)
                                                            {
                                                                // now link 'em
                                                                FishLink fishlink = new FishLink();
                                                                fishlink.FromPort = nArt.Port;
                                                                FishLinkPort flp = o as FishLinkPort;
                                                                fishlink.ToPort = flp;
                                                                myView.Document.Add(fishlink);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private QLink AddLink(GoNode n, GoNode nChild, int associationTypeID, int CAID)
        {
            QLink retval = null;
            IMetaNode nParent = n as IMetaNode;
            IMetaNode imnChild = nChild as IMetaNode;
            if (nParent != null && imnChild != null)
            {
                string linkKey = nParent.MetaObject.pkid.ToString() + nParent.MetaObject.MachineName + associationTypeID.ToString() + imnChild.MetaObject.pkid.ToString() + imnChild.MetaObject.MachineName;
                if (linksAlreadyAdded.Contains(linkKey))
                    return null;
                linksAlreadyAdded.Add(linkKey);
                linkKey = imnChild.MetaObject.pkid.ToString() + imnChild.MetaObject.MachineName + associationTypeID.ToString() + nParent.MetaObject.pkid.ToString() + nParent.MetaObject.MachineName;
                if (linksAlreadyAdded.Contains(linkKey))
                    return null;
                linksAlreadyAdded.Add(linkKey);
            }
            if (nChild.ParentNode != n) // dont link DataViews/Entities/DataTables to contained Items
            {
                GoPort prtFrom = null;
                GoPort prtTo = null;

                MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation defaultFrom = (MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation)Enum.Parse(typeof(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation), Core.Variables.Instance.DefaultFromPort);
                MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation defaultTo = (MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation)Enum.Parse(typeof(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation), Core.Variables.Instance.DefaultToPort);

                if (n is GraphNode && nChild is GraphNode)
                {
                    foreach (GoPort p in n.Ports)
                    {
                        if (!(p is QuickPort))
                            continue;

                        if ((p as QuickPort).PortPosition == defaultFrom)
                        {
                            prtFrom = p;
                            break;
                        }
                    }

                    foreach (QuickPort p in nChild.Ports)
                    {
                        if (!(p is QuickPort))
                            continue;

                        if ((p as QuickPort).PortPosition == defaultTo)
                        {
                            prtTo = p;
                            break;
                        }
                    }
                }

                #region old shapes
                if (prtTo == null)
                {
                    // find the port
                    GoNodePortEnumerator portToEnum = nChild.Ports.GetEnumerator();
                    while (portToEnum.MoveNext())
                    {
                        if (portToEnum.Current.CanLinkTo() && nChild == portToEnum.Current.Node)
                        {
                            prtTo = portToEnum.Current as GoPort;
                        }
                    }
                }

                if (prtFrom == null)
                {
                    GoNodePortEnumerator portFromEnum = n.Ports.GetEnumerator();
                    while (portFromEnum.MoveNext())
                    {
                        if (portFromEnum.Current.CanLinkFrom() && n == portFromEnum.Current.Node)
                        {
                            prtFrom = portFromEnum.Current as GoPort;
                        }
                    }
                }
                #endregion

                if (prtFrom != null && prtTo != null)
                {
                    QLink l = QLink.CreateLink(n, nChild, associationTypeID, prtFrom, prtTo);
                    //l.FromPort = prtFrom;
                    //l.ToPort = prtTo;
                    //LinkAssociationType newType = (LinkAssociationType)associationTypeID;
                    //l.AssociationType = newType;
                    // 31 OCT l.ChangeStyle();
                    retval = l;
                    this.myView.Document.Add(l);
                }
            }
            return retval;

        }
        public static DialogResult OpenStencilForClass(string s)
        {
            OpenFileDialog ofdialog = new OpenFileDialog();
            ofdialog.Title = "Select stencil with " + s + " shape";
            ofdialog.InitialDirectory = Variables.Instance.StencilPath;
            ofdialog.Filter = "Stencil Files (stn)|*.stn";
            ofdialog.Multiselect = false;
            DialogResult res = ofdialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                DockingForm.DockForm.OpenFileInApplicableWindow(ofdialog.FileName, false);
            }
            return res;
        }

        //private DataTable dtAssociations;

        private void AddChildShapes(string key, GraphNode n, ObjectAssociationArtefactContainer relatedObjects, Dictionary<string, IMetaNode> nodesAdded)
        {
            AssociationHelper assHelper = Singletons.GetAssociationHelper();
            AssociationContainer thisObjectRelated = null;
            foreach (KeyValuePair<string, AssociationContainer> kvp in relatedObjects.Containers)
            {
                if (kvp.Key == key)
                {
                    thisObjectRelated = kvp.Value;
                }
            }
            Dictionary<int, CollapsingRecordNodeItemList> repeaterSections = new Dictionary<int, CollapsingRecordNodeItemList>();
            List<GoObject> objectsToAdd = new List<GoObject>(); // ************

            if (thisObjectRelated != null)
                for (int i = 0; i < n.Count; i++)
                {
                    int counter = 0;
                    if (n[i] is CollapsingRecordNodeItemList)
                    {
                        // has associations 
                        foreach (RepeaterBindingInfo rbinfo in n.BindingInfo.RepeaterBindings)
                        {
                            CollapsingRecordNodeItemList rsection = n[i] as CollapsingRecordNodeItemList;

                            if (rsection.Count > 0)
                            {
                                if (rsection[0] is ExpandableLabelList)
                                {
                                    rsection = rsection[counter] as ExpandableLabelList;
                                    rsection.Expand();
                                    repeaterSections.Add(rbinfo.Association.ID, rsection);
                                }

                                if (rsection.Name == null)
                                {
                                    foreach (GoObject chil in rsection)
                                    {
                                        if (chil is RepeaterSection)
                                        {
                                            RepeaterSection rsect = chil as RepeaterSection;
                                            if (rsect.Name == rbinfo.RepeaterName)
                                            {
                                                repeaterSections.Add(rbinfo.Association.ID, rsect);
                                            }
                                        }
                                    }
                                }

                            }
                            counter++;
                            foreach (KeyValuePair<string, ChildObjectAssociationContainer> kvpChild in thisObjectRelated.ChildAssocs)
                            {
                                KeyObject ko = kvpChild.Value.KeyObject; // the object we might be linking to, if the CAids are the same

                                int CAid = rbinfo.Association.ID;

                                foreach (KeyValuePair<int, List<KeyObject>> associatedItem in kvpChild.Value.AssociationsAndArtefacts)
                                {

                                    int childCAID = associatedItem.Key;
                                    int assocCAID = assHelper.GetAssociationTypeForCAID(childCAID);
                                    if (CAid == childCAID)
                                    {
                                        //if (nodesAdded.ContainsKey(ko.Key.PKID.ToString() + ko.Key.Machine))
                                        {
                                            // not supposed to hit this. EVER.
                                        }
                                        //  else
                                        {
                                            foreach (MetaBase mb in relatedObjects.LoadedItems)
                                            {
                                                if (mb.pkid == ko.Key.PKID && mb.MachineName == ko.Key.Machine)
                                                {
                                                    CollapsingRecordNodeItem item = repeaterSections[childCAID].AddItemFromCode() as CollapsingRecordNodeItem;

                                                    item.MetaObject = mb;
                                                    item.BindToMetaObjectProperties();
                                                    if (!nodesAdded.ContainsKey(mb.pkid.ToString() + mb.MachineName))
                                                        nodesAdded.Add(mb.pkid.ToString() + mb.MachineName, item);
                                                    IMetaNode nparent = item.ParentNode as IMetaNode;
                                                    string linkKey = nparent.MetaObject.pkid.ToString() + nparent.MetaObject.MachineName + assocCAID.ToString() + mb.pkid.ToString() + mb.MachineName;
                                                    if (!linksAlreadyAdded.Contains(linkKey))
                                                        linksAlreadyAdded.Add(linkKey);
                                                    // prevent 2 way associations
                                                    linkKey = mb.pkid.ToString() + mb.MachineName + assocCAID.ToString() + nparent.MetaObject.pkid.ToString() + nparent.MetaObject.MachineName;
                                                    if (!linksAlreadyAdded.Contains(linkKey))
                                                        linksAlreadyAdded.Add(linkKey);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            //Console.WriteLine(kvpChild.ToString());
            // Can add connectors here - if other shapes are already dropped



        }

        #region Old Implementation
        /*
        private void AddChildShapes(int id, string machineName, GoNode parentShape,
                                    Dictionary<MetaObjectKey, MetaBase> objects, ObjectAssociationArtefactContainer relatedObjects)
        {
            List<int> possibleChildren = new List<int>();
            GraphNode innerShape = null;
            GraphNode nAssociatedShape;
            // Get Associations for metaobject and add them to the document
            // Get the current MB in the relatedObjects container
            AssociationContainer thisObjectRelated = null;
            foreach (KeyValuePair<string, AssociationContainer> kvp in relatedObjects.Containers)
            {
                if (kvp.Key == id.ToString() + machineName)
                {
                    thisObjectRelated = kvp.Value;
                }
            }

            if (thisObjectRelated!=null)
            foreach (KeyValuePair<string, ChildObjectAssociationContainer> kvpChild in thisObjectRelated.ChildAssocs)
            {
                Console.WriteLine(kvpChild.ToString());
                // Can add connectors here - if other shapes are already dropped
            }
        return;

            for (int i = 0; i < parentShape.Count; i++)
            {
                if (parentShape[i] is RepeaterSection)
                {
                    // has associations
                    RepeaterSection rsection = parentShape[i] as RepeaterSection;
                    if (parentShape is GraphNode)
                    {
                        GraphNode graphNod = parentShape as GraphNode;
                        foreach (RepeaterBindingInfo rbinfo in graphNod.BindingInfo.RepeaterBindings)
                        {
                            int CAid = rbinfo.Association.ID;
                            
                            dtAssociations.DefaultView.RowFilter = "CAid=" + CAid.ToString();

                            foreach (DataRowView drv in dtAssociations.DefaultView)
                            {
                                MetaBase mbaseRepeaterItem =
                                    Loader.GetByID(rbinfo.Association.ChildClass, int.Parse(drv["ChildObjectID"].ToString()),
                                                   drv["ChildObjectMachine"].ToString());
                                CollapsingRecordNodeItem item = rsection.AddItemFromCode() as CollapsingRecordNodeItem;
                                //childshapes.Add(item);
                                item.MetaObject = mbaseRepeaterItem;
                            }
                        }
                    }

                }
            }

            int ColumnCount = dtAssociations.Columns.Count;
            foreach (DataRowView drv in dtAssociations.DefaultView)
            {
                MetaObjectKey mkey = new MetaObjectKey();
                mkey.pkid = int.Parse(drv["ChildObjectID"].ToString());
                mkey.Machine = drv["ChildObjectMachine"].ToString();
                if (objects.ContainsKey(mkey))
                {
                    if (!(addedShapes.ContainsKey(mkey)))
                    {
                        innerShape = GetShape(drv["ChildClass"].ToString());
                        if (innerShape != null)
                        {
                            nAssociatedShape = DropObject(innerShape, new ObjectIDentifier(mkey.pkid, mkey.Machine), objects);
                            addedShapes.Add(mkey, 1);

                            AddConnector(parentShape, mkey, int.Parse(drv["AssociationTypeID"].ToString()),
                                         int.Parse(drv["CAid"].ToString()));
                            AddChildShapes(mkey.pkid, mkey.Machine, nAssociatedShape, objects,relatedObjects);
                        }
                        else
                        {
                            bool skip = false;
                            if (parentShape is CollapsibleNode)
                            {
                                foreach (GoObject o in parentShape)
                                    if (o is IMetaNode)
                                    {
                                        IMetaNode imnode = o as IMetaNode;
                                        if (imnode.MetaObject.pkid == mkey.pkid && imnode.MetaObject.MachineName == mkey.Machine)
                                        {
                                            // ignore this item - already embedded
                                            skip = true;
                                        }
                                    }
                            }
                            if (!skip)
                            {
                                if (parentShape is CollapsingRecordNodeItem)
                                {
                                    AddConnector(parentShape, mkey, int.Parse(drv["AssociationTypeID"].ToString()),
                                        int.Parse(drv["CAid"].ToString()));
                                }
                            }
                        }
                    }
                    else
                    {
                        AddConnector(parentShape, mkey, int.Parse(drv["AssociationTypeID"].ToString()),
                                     int.Parse(drv["CAid"].ToString()));
                    }
                }
            }
        }
        */
        #endregion

        private GraphNode DropObject(GraphNode shapeToUse, MetaBase mbase, Dictionary<b.MetaObjectKey, MetaBase> objects)
        {
            GraphNode n = (GraphNode)shapeToUse.Copy();
            n.MetaObject = mbase;
            n.BindToMetaObjectProperties();
            n.Shadowed = true;
            dbcollection.Add(n);

            ////FORCE REFRESH OF COMPLEX NODES ON DB LOAD
            //if (n is CollapsibleNode)
            //{
            //    GoCollection col = new GoCollection();
            //    MetaComparer comp = new MetaComparer();
            //    comp.MyView = myView;
            //    col.Add(n as CollapsibleNode);
            //    comp.RefreshSelection(col);
            //}
            //else
            //{
            #region Indicators for normal graphnodes
            //update indicators on load from db

            if (VCStatusTool.IsObsoleteOrMarkedForDelete(mbase) && mbase.IsInDatabase(Core.Variables.Instance.ClientProvider))
            {
                viewController.IndicatorController.AddChangedIndicator(mbase.State == VCStatusList.MarkedForDelete ? "Marked For Delete" : mbase.State.ToString(), System.Drawing.Color.Red, n as GoGroup);
                n.Editable = false;
                n.Copyable = false;
            }
            else if (mbase.State == VCStatusList.CheckedIn || mbase.State == VCStatusList.CheckedOutRead || mbase.State == VCStatusList.Locked)
            {
                viewController.IndicatorController.AddIndicator(mbase.State.ToString(), System.Drawing.Color.Gray, n as GoGroup);
                if (n is IMetaNode)
                {
                    IMetaNode node = n as IMetaNode;
                    node.MetaObject = mbase;
                }
                n.Editable = false;
                n.Copyable = true;
            }
            else if (mbase.State == VCStatusList.Obsolete)
            {
                viewController.IndicatorController.AddIndicator("Obsolete", System.Drawing.Color.Gray, n as GoGroup);
                if (n is IMetaNode)
                {
                    IMetaNode node = n as IMetaNode;
                    node.MetaObject = mbase;
                }
                n.Editable = false;
                n.Copyable = true;
            }

            #endregion
            //}

            return n;

            List<CollapsingRecordNodeItem> items = n.AddRepeaterObjects(objects);
            childshapes = new GoCollection();
            foreach (CollapsingRecordNodeItem o in items)
            {
                childshapes.Add(o);
            }

            return n;
        }
        private void DoLinking(int AssociationTypeID, GoNode parent, GoNode nChildToConnect, int CAid, List<GoObject> objectsToAdd)
        {
            #region Link objects
            QLink link = new QLink();
            link.AssociationType = (LinkAssociationType)AssociationTypeID;
            bool connected = false;

            #region Connect Parent
            while (connected == false)
            {
                foreach (GoObject o in parent)
                {
                    if (o is GoPort)
                    {
                        GoPort prt = o as GoPort;
                        link.FromPort = prt;
                        connected = true;
                    }
                }
            }
            #endregion

            #region Connect Child
            connected = false;
            while (connected == false)
            {
                foreach (GoObject o in nChildToConnect)
                {
                    if (o is GoPort)
                    {
                        GoPort prt = o as GoPort;
                        link.ToPort = prt;
                        connected = true;
                    }
                }
            }
            #endregion

            #region Add Link & Change Style
            AssociationHelper assHelper = Singletons.GetAssociationHelper();
            // Get Artefacts for link
            MenuItem mitem = new MenuItem();
            LinkAssociationType newType = (LinkAssociationType)AssociationTypeID;
            mitem.Tag = newType;
            link.AssociationType = newType;
            // 31 OCT link.ChangeStyle();
            #endregion

            #endregion

            #region Artefacts - load, bind, add, link
            // ****************** ARTEFACTS!!!
            // ****************** ARTEFACTS!!!
            // ****************** ARTEFACTS!!!
            // ****************** ARTEFACTS!!!
            // ****************** ARTEFACTS!!!
            // ****************** ARTEFACTS!!!
            // ****************** ARTEFACTS!!!
            /*
            DataSet dsArtefacts =
                assHelper.GetArtifactData(CAid, imParent.MetaObject.pkid, imChild.MetaObject.pkid,
                                      imParent.MetaObject.MachineName,
                                      imChild.MetaObject.MachineName);
            DataView dvArtefacts = dsArtefacts.Tables[1].DefaultView;
            foreach (DataRowView drvArtefacts in dvArtefacts)
            {
                ArtefactNode anode = new ArtefactNode();
                anode.MetaObject =
                    Loader.GetByID(drvArtefacts["ArtifactClass"].ToString(),
                               int.Parse(drvArtefacts["ArtifactObjectID"].ToString()),
                               drvArtefacts["ArtefactMachine"].ToString());
                anode.BindToMetaObjectProperties();
                myView.Document.Add(anode);
                GoGroup grp = link.MidLabel as GoGroup;
                foreach (GoObject o in grp)
                {
                    if (o is FishLinkPort)
                    {
                        FishLinkPort linkPort = o as FishLinkPort;
                        FishLink fLink = new FishLink();
                        fLink.FromPort = anode.Port;
                        fLink.ToPort = linkPort;
                        anode.Position = linkPort.Position;
                        //myView.Document.Add(fLink);
                        if (!objectsToAdd.Contains(fLink))
                            objectsToAdd.Add(fLink);
                    }
                }
            }
             */
            #endregion

            // not sure why this was enabled??!
            //if ((link.AssociationType != LinkAssociationType.Mapping) && (link.AssociationType != LinkAssociationType.LocatedAt))
            #region Check if link is valid and add to collection
            bool shouldAdd = true;
            if (link.FromPort != null)
            {
                GoNode n = link.FromNode as GoNode;
                GoNodeLinkEnumerator linkEnum = n.SourceLinks.GetEnumerator();
                while (linkEnum.MoveNext())
                {
                    if (linkEnum.Current is QLink)
                    {
                        QLink lnk = linkEnum.Current as QLink;
                        if (lnk.FromNode == nChildToConnect)
                        {
                            if (lnk.AssociationType == link.AssociationType)
                            {
                                // Bi-Directional, remove...
                                shouldAdd = false;
                            }
                        }
                    }
                }
            }

            if (shouldAdd)
            {
                if (!objectsToAdd.Contains(link))
                    objectsToAdd.Add(link);
            }
            #endregion
        }

        #region Old Code - Add Connector
        /*
        private void AddConnector(GoNode parent, MetaObjectKey childkey, int AssociationTypeID, int CAid)
        {
            IMetaNode imParent = parent as IMetaNode;
            List<GoObject> objectsToAdd = new List<GoObject>();
            foreach (GoObject obj in dbcollection)
            {
                bool found = false;
                if (obj is IMetaNode && obj is GoNode)
                {
                    GoNode nChildToConnect = obj as GoNode;
                    IMetaNode imChild = obj as IMetaNode;


                    if (nChildToConnect != null)
                    {
                        if (imChild.MetaObject.pkid == childkey.pkid &&
                            imChild.MetaObject.MachineName == childkey.Machine)
                        {
                            found = true;
                            DoLinking(AssociationTypeID, parent, nChildToConnect, CAid, imParent, imChild, objectsToAdd);
                        }
                    }
                }
                if (!found)
                {
                    foreach (GoObject o in dbcollection)
                    {
                        if (!found)
                            if (o is GoNode)
                            {

                                GoNode n = o as GoNode;
                                if (n != parent)
                                {
                                    if (n is CollapsibleNode)
                                    foreach (GoObject possibleList in n)
                                    {
                                        if (!found)
                                            if (possibleList is MetaBuilder.Graphing.Shapes.CollapsingRecordNodeItemList)
                                            {
                                                CollapsingRecordNodeItemList lst = possibleList as CollapsingRecordNodeItemList;
                                                if (!found)
                                                    foreach (GoObject posRepeaterSection in lst)
                                                    {
                                                        if (posRepeaterSection is RepeaterSection)
                                                        {
                                                            RepeaterSection rSection = posRepeaterSection as RepeaterSection;
                                                            foreach (GoObject objChild in rSection)
                                                            {
                                                                if (!found)
                                                                    if (objChild is IMetaNode)
                                                                    {
                                                                        IMetaNode imChild2 = objChild as IMetaNode;
                                                                        GoNode nChildToConnect2 = objChild as GoNode;
                                                                        if (imChild2.MetaObject != null) // dont load headers
                                                                        {
                                                                            if (!found)
                                                                                if (imChild2.MetaObject.pkid == childkey.pkid &&
                                                                      imChild2.MetaObject.MachineName == childkey.Machine)
                                                                                {
                                                                                    found = true;

                                                                                    DoLinking(AssociationTypeID, parent, nChildToConnect2, CAid, imParent, imChild2, objectsToAdd);
                                                                                }
                                                                        }



                                                                    }
                                                            }
                                                        }
                                                    }

                                            }
                                    }
                                }
                            }
                    }
                }
            }

            foreach (GoObject gobj in objectsToAdd)
            {
                if (!dbcollection.Contains(gobj))
                    dbcollection.Add(gobj);
            }
        }
        */
        #endregion

        public static GraphNode GetShape(string cls)
        {
            if (Variables.Instance.ShapeCache.Count > 0)
            {
                //TODO Dataviews => to database check if attributes or columns connected to choose right one
                if (!(cls.Contains("dataview")))
                {
                    GraphNode newNodeObject = (GraphNode)Variables.Instance.ReturnShape(cls);
                    if (newNodeObject != null)
                        return (GraphNode)newNodeObject.Copy();
                }
            }

            GraphNode shapeToUse = null;
            GraphNode overRideShapeToUse = null;
            DockPanel docPanel = DockingForm.DockForm.GetPaletteContainer();
            if (docPanel != null)
            {
                foreach (PaletteContainer plcontainer in docPanel.Documents)
                {
                    if (plcontainer.Palette.Selection.Count > 0)
                    {
                        GraphNode node = plcontainer.Palette.Selection.Primary.ParentNode as GraphNode;
                        if (node != null)
                        {
                            if (node.BindingInfo != null)
                                if (node.BindingInfo.BindingClass == cls)
                                {
                                    overRideShapeToUse = node;
                                }
                        }
                    }

                    GoDocument stencil = plcontainer.Palette.Document;
                    foreach (GoObject obj in stencil)
                    {
                        if (obj is GraphNode)
                        {
                            GraphNode dnode = obj as GraphNode;
                            if (dnode.BindingInfo != null)
                            {
                                //add shape to cache so next time we dont have to open the same stencil again
                                if (!(Variables.Instance.ShapeCache.ContainsKey(dnode.BindingInfo.BindingClass)))
                                    Variables.Instance.ShapeCache.Add(dnode.BindingInfo.BindingClass, dnode.Copy());

                                if (cls.Contains("_"))
                                {
                                    if (dnode.BindingInfo.BindingClass == "DataView")
                                    {
                                        string type = (dnode.Label as BoundLabel).Name == "lblDataViewTypexx" ? "Logical" : "Physical";
                                        switch (cls)
                                        {
                                            case "DataView_Physical":
                                                if (type == "Physical")
                                                {
                                                    shapeToUse = dnode;
                                                }
                                                break;
                                            case "DataView_Logical":
                                                if (type == "Logical")
                                                {
                                                    shapeToUse = dnode;
                                                }
                                                break;
                                            default:
                                                shapeToUse = dnode;
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (dnode.BindingInfo.BindingClass == cls)
                                    {
                                        shapeToUse = dnode;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (overRideShapeToUse != null)
                shapeToUse = overRideShapeToUse;
            return shapeToUse;
        }
        public void SwapShapes()
        {

            myView.Selection.Clear();

            foreach (GoObject o in myView.Doc)
            {

                if (o is GraphNode && o is IMetaNode)
                {

                    GraphNode node = o as GraphNode;

                    if (node.BindingInfo.BindingClass == "Entity" || node.BindingInfo.BindingClass == "DataEntity")
                    {

                        GraphNode shapeToUse = GetShape("DataTable").Copy() as GraphNode;

                        if (shapeToUse == null)
                        {

                            MessageBox.Show(DockingForm.DockForm, "Stencil does not contain shape for DataTable", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            //if (MessageBox.Show(this,"Stencil does not contain shape for DataTable" + Environment.NewLine + "Would you like to add this stencil?","Warning",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)

                            //{

                            //    //Show add stencil dialog



                            //    //Restart

                            //    //SwapShapes();

                            //    return;

                            //}

                            //else

                            //{

                            //    return;

                            //}

                        }

                        else
                        {

                            //This removes the binding to the stencil! Only works if binding exists

                            shapeToUse.Remove();

                            shapeToUse.Position = node.Position;

                            shapeToUse.MetaObject = Loader.CreateInstance("DataTable");

                            //if (node.MetaObject.Get("Name") != null)
                            //shapeToUse.MetaObject.Set("Name", node.MetaObject.Get("Name").ToString());
                            node.MetaObject.CopyPropertiesToObject(shapeToUse.MetaObject);

                            //Attributes

                            //Repeater section for collapsible nodes! CAST!!

                            //Entity has 2 CAid's != DataTable has 2 CAid's!!

                            try
                            {

                                foreach (RepeaterSection rItem in node.RepeaterSections)
                                {

                                    CollapsingRecordNodeItemList iList;

                                    iList = rItem;

                                    foreach (GoObject obj in iList)
                                    {
                                        if (obj is CollapsingRecordNodeItem)
                                        {
                                            CollapsingRecordNodeItem s = obj as CollapsingRecordNodeItem;

                                            if (s.BindingInfo.BindingClass == "Attribute")
                                            {
                                                if (rItem.Name == "Key Attributes")
                                                {
                                                    //int x = 2341;
                                                    CollapsingRecordNodeItem ritem = shapeToUse.RepeaterSections[0].AddItemFromCode() as CollapsingRecordNodeItem;

                                                    ritem.MetaObject = Loader.CreateInstance("DataField");

                                                    s.MetaObject.CopyPropertiesToObject(ritem.MetaObject);
                                                    ritem.MetaObject.pkid = 0;

                                                    ritem.BindToMetaObjectProperties();
                                                }

                                                else if (rItem.Name == "Descriptive Attributes")
                                                {
                                                    //int x = 2584;
                                                    CollapsingRecordNodeItem ritem = shapeToUse.RepeaterSections[1].AddItemFromCode() as CollapsingRecordNodeItem;
                                                    ritem.MetaObject.pkid = 0;

                                                    ritem.MetaObject = Loader.CreateInstance("DataField");

                                                    s.MetaObject.CopyPropertiesToObject(ritem.MetaObject);

                                                    ritem.BindToMetaObjectProperties();
                                                }
                                            }
                                        }
                                    }
                                }

                            }

                            catch (Exception ex)
                            {

                                MessageBox.Show(ex.ToString());

                            }

                            //Add/Remove

                            shapeToUse.BindToMetaObjectProperties();

                            shapeToUse.HookupEvents();

                            myView.Document.Add(shapeToUse);

                            myView.Selection.Add(shapeToUse);



                            //myView.Document.Remove(node); //?

                        }

                    }

                }

            }

        }
    }
}