using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Docking;
using MetaBuilder.CommonControls.Tree;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.UIControls.Common.CheckBoxComboBox;
using System.Collections.ObjectModel;

namespace MetaBuilder.UIControls.GraphingUI.Tools.Explorer
{
    public partial class MetaObjectExplorer : DockContent
    {
        public MetaObjectExplorer()
        {
            InitializeComponent();
            comboWorkspace.CheckBoxCheckedChanged += new EventHandler(comboWorkspace_CheckBoxCheckedChanged);
            comboDiagram.CheckBoxCheckedChanged += new EventHandler(comboDiagram_CheckBoxCheckedChanged);
            comboClass.CheckBoxCheckedChanged += new EventHandler(comboClass_CheckBoxCheckedChanged);
        }

        public void Reset()
        {
            if (Loading)
                return;

            Loading = true;
            TabText = Text = "Initialising";

            ClearList();
            loadMenuItems();

            Report(0, 0, "Ready");

            TabText = "Object Explorer";
            Loading = false;
        }

        private Thread populate;
        private ObjectHelper objHelper = new ObjectHelper(false);
        private bool Loading = false;
        private void loadMenuItems()
        {
            loadWorkspaces();

            comboClass.Items.Clear();
            CCBoxItem itemC = new CCBoxItem("Select All", 0);
            comboClass.Items.Add(itemC);
            itemC = new CCBoxItem("Select None", -1);
            comboClass.Items.Add(itemC);
            TList<Class> classes = DataRepository.Classes(Core.Variables.Instance.ClientProvider);//Core.Variables.Instance.ClientProvider].Provider.ClassProvider.GetAll();
            classes.Sort("Name");
            foreach (Class c in classes)
            {
                if (c.IsActive == true)
                {
                    itemC = new CCBoxItem(c.Name, c);
                    comboClass.Items.Add(itemC);
                }
            }
            classes.Dispose();

            comboDiagram.Items.Clear();
            itemC = new CCBoxItem("Select All", 0);
            comboDiagram.Items.Add(itemC);
            itemC = new CCBoxItem("Select None", -1);
            comboDiagram.Items.Add(itemC);
            TList<GraphFile> files = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileProvider.GetAll().FindAll(GraphFileColumn.IsActive, true);
            files.Sort("ModifiedDate DESC");
            foreach (GraphFile g in files)
            {
                if (!g.IsActive)
                    continue;
                itemC = new CCBoxItem(Core.strings.GetFileNameOnly(g.Name), g);
                comboDiagram.Items.Add(itemC);
            }
            files.Dispose();

            comboWorkspace.Text = "Workspace";
            comboDiagram.Text = "Diagram";
            comboClass.Text = "Class";

            objects = null;
        }
        private void loadWorkspaces()
        {
            comboWorkspace.Items.Clear();
            CCBoxItem itemC = new CCBoxItem("Select All", 0);
            comboWorkspace.Items.Add(itemC);
            itemC = new CCBoxItem("Select None", -1);
            comboWorkspace.Items.Add(itemC);
            TList<Workspace> workspaces = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.WorkspaceProvider.GetAll();
            workspaces.Sort("Name");
            foreach (Workspace w in workspaces)
            {
                //if (w.IsActive == true)
                //{
                itemC = new CCBoxItem(w.Name, w);
                comboWorkspace.Items.Add(itemC);
                //}
            }
            workspaces.Dispose();
        }

        private bool checking = false;
        private void comboWorkspace_CheckBoxCheckedChanged(object sender, EventArgs e)
        {
            if (checking)
                return;
            CCBoxItem item = (sender as UIControls.Common.CheckBoxComboBox.CheckBoxComboBoxItem).ComboBoxItem as CCBoxItem;
            if (item == null)
            {
                return;
            }

            bool? setAll = null;
            if (item.Value is int)
            {
                if ((int)item.Value == 0)
                    setAll = true;
                else if ((int)item.Value == -1)
                    setAll = false;
            }

            if (setAll != null)
            {
                checking = true;
                for (int i = 2; i < comboWorkspace.CheckBoxItems.Count; i++)
                {
                    (comboWorkspace.CheckBoxItems[i] as UIControls.Common.CheckBoxComboBox.CheckBoxComboBoxItem).Checked = (bool)setAll;
                }
                (comboWorkspace.CheckBoxItems["Select All"] as UIControls.Common.CheckBoxComboBox.CheckBoxComboBoxItem).Checked = false;
                (comboWorkspace.CheckBoxItems["Select All"] as UIControls.Common.CheckBoxComboBox.CheckBoxComboBoxItem).CheckState = CheckState.Unchecked;
                (comboWorkspace.CheckBoxItems["Select None"] as UIControls.Common.CheckBoxComboBox.CheckBoxComboBoxItem).Checked = false;
                (comboWorkspace.CheckBoxItems["Select None"] as UIControls.Common.CheckBoxComboBox.CheckBoxComboBoxItem).CheckState = CheckState.Unchecked;
                checking = false;
                comboWorkspace.Text = "Workspace";
            }
        }
        private void comboDiagram_CheckBoxCheckedChanged(object sender, EventArgs e)
        {
            if (checking)
                return;
            CCBoxItem item = (sender as UIControls.Common.CheckBoxComboBox.CheckBoxComboBoxItem).ComboBoxItem as CCBoxItem;
            if (item == null)
            {
                return;
            }

            bool? setAll = null;
            if (item.Value is int)
            {
                if ((int)item.Value == 0)
                    setAll = true;
                else if ((int)item.Value == -1)
                    setAll = false;
            }

            if (setAll != null)
            {
                checking = true;
                for (int i = 2; i < comboDiagram.CheckBoxItems.Count; i++)
                {
                    (comboDiagram.CheckBoxItems[i] as UIControls.Common.CheckBoxComboBox.CheckBoxComboBoxItem).Checked = (bool)setAll;
                }
                (comboDiagram.CheckBoxItems["Select All"] as UIControls.Common.CheckBoxComboBox.CheckBoxComboBoxItem).Checked = false;
                (comboDiagram.CheckBoxItems["Select All"] as UIControls.Common.CheckBoxComboBox.CheckBoxComboBoxItem).CheckState = CheckState.Unchecked;
                (comboDiagram.CheckBoxItems["Select None"] as UIControls.Common.CheckBoxComboBox.CheckBoxComboBoxItem).Checked = false;
                (comboDiagram.CheckBoxItems["Select None"] as UIControls.Common.CheckBoxComboBox.CheckBoxComboBoxItem).CheckState = CheckState.Unchecked;
                checking = false;
                comboDiagram.Text = "Diagram";
            }
        }
        private void comboClass_CheckBoxCheckedChanged(object sender, EventArgs e)
        {
            if (checking)
                return;
            CCBoxItem item = (sender as UIControls.Common.CheckBoxComboBox.CheckBoxComboBoxItem).ComboBoxItem as CCBoxItem;
            if (item == null)
            {
                return;
            }

            bool? setAll = null;
            if (item.Value is int)
            {
                if ((int)item.Value == 0)
                    setAll = true;
                else if ((int)item.Value == -1)
                    setAll = false;
            }

            if (setAll != null)
            {
                checking = true;
                for (int i = 2; i < comboClass.CheckBoxItems.Count; i++)
                {
                    (comboClass.CheckBoxItems[i] as UIControls.Common.CheckBoxComboBox.CheckBoxComboBoxItem).Checked = (bool)setAll;
                }
                (comboClass.CheckBoxItems["Select All"] as UIControls.Common.CheckBoxComboBox.CheckBoxComboBoxItem).Checked = false;
                (comboClass.CheckBoxItems["Select All"] as UIControls.Common.CheckBoxComboBox.CheckBoxComboBoxItem).CheckState = CheckState.Unchecked;
                (comboClass.CheckBoxItems["Select None"] as UIControls.Common.CheckBoxComboBox.CheckBoxComboBoxItem).Checked = false;
                (comboClass.CheckBoxItems["Select None"] as UIControls.Common.CheckBoxComboBox.CheckBoxComboBoxItem).CheckState = CheckState.Unchecked;
                checking = false;
                comboClass.Text = "Class";
            }
        }

        private void MetaObjectExplorer_Load(object sender, EventArgs e)
        {
            Reset();
            listViewItems.ImageList = DockingForm.DockForm.ImageListWorkspaces;
            listViewItems.TreeViewNodeSorter = new NodeSorter();
        }

        private void StartThread()
        {
            Report(0, 0, "Started");
            populate = new Thread(new ThreadStart(PopulateObjects));
            populate.IsBackground = true;
            populate.Start();
            CancelThread = false;
        }

        private bool CancelThread = false;
        private List<Meta.MetaBase> objects;
        private Collection<string> getSelectedWorkspaces()
        {
            Collection<string> r_list = new Collection<string>();
            foreach (object o in comboWorkspace.CheckedItems)
            {
                if (o is CCBoxItem)
                {
                    CCBoxItem item = o as CCBoxItem;
                    r_list.Add((item.Value as Workspace).Name + "#" + (item.Value as Workspace).WorkspaceTypeId);
                }
                else
                {
                }
            }

            return r_list;
        }
        private Collection<string> getSelectedClasses()
        {
            Collection<string> r_list = new Collection<string>();
            foreach (object o in comboClass.CheckedItems)
            {
                if (o is CCBoxItem)
                {
                    CCBoxItem item = o as CCBoxItem;
                    r_list.Add((item.Value as Class).Name);
                }
                else
                {
                }
            }
            return r_list;
        }
        private TList<GraphFile> getSelectedFiles()
        {
            TList<GraphFile> r_list = new TList<GraphFile>();
            foreach (object o in comboDiagram.CheckedItems)
            {
                if (o is CCBoxItem)
                {
                    CCBoxItem item = o as CCBoxItem;
                    r_list.Add(item.Value as GraphFile);
                }
                else
                {
                }
            }

            return r_list;
        }
        private void PopulateObjects()
        {
            FileNodes = new Collection<TreeNode>();
            ClassNodes = new Collection<TreeNode>();

            Collection<string> workspaces = getSelectedWorkspaces();
            Collection<string> classes = getSelectedClasses();
            TList<GraphFile> files = getSelectedFiles();
            string threadText = textBoxName.Text;

            if (CancelThread)
            {
                return;
            }
            ClearList();

            Report(10, 1, "Retrieving Data");
            objects = new List<Meta.MetaBase>();
            int counter = 0;

            #region Filter and data retrieval
            List<string> filters = new List<string>();

            Report(10, 3, "Retrieving Data");
            if (files.Count > 0)
            {
                if (files.Count > 1)
                {
                    string fileFilter = " AND ( ";
                    foreach (GraphFile file in files)
                    {
                        if (CancelThread)
                        {
                            Report(100, 0, "Cancelled");
                            return;
                        }
                        fileFilter += " ( cast(pkid as varchar(20)) + '|' + Machine in (select cast(metaObjectID as varchar(20)) + '|' + MachineID from GraphFileObject where GraphFileID=" + file.pkid.ToString() + " and GraphFileMachine='" + file.Machine + "') ) OR";
                    }
                    fileFilter = fileFilter.Substring(0, fileFilter.LastIndexOf("OR"));
                    fileFilter += " )";

                    filters.Add(fileFilter);
                }
                else if (files.Count == 1)
                    filters.Add(" and cast(pkid as varchar(20)) + '|' + Machine in (select cast(metaObjectID as varchar(20)) + '|' + MachineID from GraphFileObject where GraphFileID=" + files[0].pkid.ToString() + " and GraphFileMachine='" + files[0].Machine + "')");
            }

            Report(10, 4, "Retrieving Data");
            if (classes.Count > 0)
            {
                if (classes.Count > 1)
                {
                    string classFilter = " AND ( ";
                    foreach (string c in classes)
                    {
                        if (CancelThread)
                        {
                            Report(100, 0, "Cancelled");
                            return;
                        }
                        classFilter += " ( CLASS='" + c + "' ) OR";
                        counter++;
                    }
                    classFilter = classFilter.Substring(0, classFilter.LastIndexOf("OR"));
                    classFilter += " )";

                    filters.Add(classFilter);
                }
                else if (classes.Count == 1)
                {
                    filters.Add(" AND CLASS='" + classes[0] + "' ");
                }
            }

            Report(10, 5, "Retrieving Data");
            if (workspaces.Count > 1)
            {
                string wsFilter = " AND ( ";

                foreach (string ws in workspaces)
                    wsFilter += " (WorkspaceName= '" + ws.Substring(0, ws.IndexOf("#")) + "' AND WorkspaceTypeId=" + ws.Substring(ws.IndexOf("#") + 1) + ") OR";

                wsFilter = wsFilter.Substring(0, wsFilter.LastIndexOf("OR"));
                wsFilter += " )";

                filters.Add(wsFilter);
            }
            else if (workspaces.Count == 1)
            {
                filters.Add(" AND WorkspaceName= '" + workspaces[0].Substring(0, workspaces[0].IndexOf("#")) + "' AND WorkspaceTypeId=" + workspaces[0].Substring(workspaces[0].IndexOf("#") + 1));
            }

            Dictionary<string, List<Meta.MetaKey>> itemsToRetrieve = new Dictionary<string, List<Meta.MetaKey>>();
            Report(10, 6, "Retrieving Data");
            foreach (DataRowView drvObject in objHelper.GetObjectsFiltered(filters, false))
            {
                if (CancelThread)
                    return;
                string classname = drvObject["Class"].ToString();

                List<Meta.MetaKey> keys = new List<Meta.MetaKey>();
                if (!itemsToRetrieve.ContainsKey(classname))
                {
                    itemsToRetrieve.Add(classname, keys);
                }
                else
                {
                    keys = itemsToRetrieve[classname];
                }
                Meta.MetaKey mkey = new Meta.MetaKey();
                mkey.PKID = int.Parse(drvObject["pkid"].ToString());
                mkey.Machine = drvObject["Machine"].ToString();
                keys.Add(mkey);
            }

            Report(10, 8, "Compiling");
            Nodes = new Dictionary<string, TreeNode>();
            objects = Meta.Loader.GetFromProvider(itemsToRetrieve, false);
            #endregion

            if (objects != null && objects.Count > 0)
            {
                Report(100, 0, "Filtering Data");
                int i = 0;
                Report(objects.Count, i, "");
                foreach (Meta.MetaBase mbase in objects)
                {
                    if (CancelThread)
                    {
                        Report(100, 0, "Cancelled");
                        break;
                    }
                    if (objects == null)
                    {
                        return;
                    }
                    Report(objects.Count, i, "");

                    if (mbase.ToString().ToLower().Contains(threadText.ToLower()))
                    {
                        bool orphan = true;
                        foreach (GraphFile file in files)
                        {
                            try
                            {
                                GraphFileObject gfo = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileObjectProvider.GetByGraphFileIDMetaObjectIDMachineIDGraphFileMachine(file.pkid, mbase.pkid, mbase.MachineName, file.Machine);
                                if (gfo != null)
                                {
                                    orphan = false;
                                    AddItem(mbase, file);
                                }
                            }
                            catch
                            {
                            }
                        }
                        if (files.Count == 0)
                            if (orphan)
                                AddItem(mbase, null);
                    }
                    i++;
                }
            }
            Application.DoEvents();
            //update at the end
            foreach (KeyValuePair<string, TreeNode> kvp in Nodes)
            {
                AddNode(kvp.Value);
            }
            Application.DoEvents();
            Report(100, 0, "Ready");
        }

        delegate void ReportDelegate(int total, int progress, string message);
        private void Report(int total, int progress, string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ReportDelegate(Report), new object[] { total, progress, message });
            }
            else
            {
                progressBar1.UseWaitCursor = message != "Ready" || total > 0;
                btnRefresh.Enabled = message == "Ready" || total == 0;

                progressBar1.ForeColor = (message == "Retrieving Data" || message == "Compiling") ? Color.LightBlue : SystemColors.Highlight;
                progressBar1.Maximum = total;

                if (progress <= total)
                    progressBar1.Value = progress;
                //else
                //{
                //    //double thread?!!
                //}
                ////if (message.Length > 0)
                ////    labelMessage.Text = message;
            }
        }

        delegate void ClearListViewCallback();
        public void ClearList()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ClearListViewCallback(ClearList), new object[] { });
            }
            else
            {
                listViewItems.Nodes.Clear();
                Application.DoEvents();
            }
        }

        private Collection<TreeNode> FileNodes;
        private Collection<TreeNode> ClassNodes;
        private Dictionary<string, TreeNode> Nodes;

        delegate void AddItemDelegate(Meta.MetaBase mbase, GraphFile file);
        private void AddItem(Meta.MetaBase mbase, GraphFile file)
        {
            //if (InvokeRequired)
            //{
            //    BeginInvoke(new AddItemDelegate(AddItem), new object[] { mbase, file });
            //}
            //else
            {
                string wsKey = "";
                string wsname = "";
                if (file == null)
                {
                    wsKey = mbase.WorkspaceName + "#" + mbase.WorkspaceTypeId.ToString();
                    wsname = mbase.WorkspaceName;
                }
                else
                {
                    wsKey = file.WorkspaceName + "#" + file.WorkspaceTypeId.ToString();
                    wsname = file.WorkspaceName;
                }

                TreeNode wsNode = null;
                Nodes.TryGetValue(wsKey, out wsNode);

                if (wsNode == null)
                {
                    wsNode = new TreeNode();
                    wsNode.Name = wsKey;
                    wsNode.Text = wsname;
                    wsNode.ImageIndex = (wsKey.Contains("#3")) ? 0 : 1;
                    try
                    {
                        Nodes.Add(wsKey, wsNode);
                    }
                    catch
                    {
                        //?
                    }
                }

                TreeNode fileNode = null;
                TreeNode classNode = null;
                if (file == null)
                {
                    //find or create classnode
                    string classKey = mbase.Class;
                    if (wsNode.Nodes.ContainsKey(classKey))
                    {
                        classNode = wsNode.Nodes[classKey];
                    }
                    else
                    {
                        classNode = new TreeNode();
                        classNode.Name = classKey;
                        classNode.Text = classKey;
                        classNode.ImageIndex = 10;
                        classNode.ImageIndex = mbase.WorkspaceTypeId == 3 ? 0 : 1;

                        wsNode.Nodes.Add(classNode);
                        //wsNode.Expand();
                    }
                }
                else
                {
                    //find or create filenode
                    string fileKey = Core.strings.GetFileNameOnly(file.Name);
                    if (wsNode.Nodes.ContainsKey(fileKey))
                    {
                        fileNode = wsNode.Nodes[fileKey];
                    }
                    else
                    {
                        fileNode = new TreeNode();
                        fileNode.Name = fileKey;
                        fileNode.Tag = file;
                        fileNode.Text = fileKey;
                        fileNode.ImageIndex = file.WorkspaceTypeId == 3 ? 0 : 1;

                        wsNode.Nodes.Add(fileNode);
                        //wsNode.Expand();
                    }

                    string classKey = mbase.Class;
                    if (fileNode.Nodes.ContainsKey(classKey))
                    {
                        classNode = fileNode.Nodes[classKey];
                    }
                    else
                    {
                        classNode = new TreeNode();
                        classNode.Name = classKey;
                        classNode.Text = classKey;
                        classNode.ImageIndex = file.WorkspaceTypeId == 3 ? 0 : 1;

                        fileNode.Nodes.Add(classNode);
                        fileNode.Expand();
                    }
                }

                //ITEM
                ItemTreeNode item = new ItemTreeNode();
                item.Name = mbase.pkid + "-" + mbase.MachineName;

                if (listViewItems.Nodes.ContainsKey(item.Name))
                    return;

                if (mbase.ToString().Length <= 255)
                    item.Text = mbase.ToString();
                else
                    item.Text = mbase.ToString().Substring(0, 254);

                if (mbase.ToString().Trim().Length == 0)
                {
                    item.Text = "To be named...";
                    try
                    {
                        item.NodeFont = new Font("Times New Roman", 10f, FontStyle.Italic);
                    }
                    catch
                    {
                    }
                }

                if (mbase.State == VCStatusList.MarkedForDelete)
                {
                    item.ForeColor = Color.LightGray;
                    item.ToolTipText = "Marked For Delete";
                }
                else
                {
                    if (Core.Variables.Instance.ClassesWithoutStencil.Contains(mbase.Class))
                    {
                        //item.ForeColor = Color.DarkGray;
                        item.ToolTipText = "No shape for this object [" + mbase.pkid + ":" + mbase.MachineName + "]";
                        if (mbase.Class == "Attrribute" || mbase.Class == "DataColumn" || mbase.Class == "DataAttrribute" || mbase.Class == "DataField")
                            item.ForceShapeType = typeof(MetaBuilder.Graphing.Shapes.CollapsingRecordNodeItem);
                        else
                            item.ForceShapeType = typeof(MetaBuilder.Graphing.Shapes.ArtefactNode);
                    }
                    else
                        item.ToolTipText = "[" + mbase.pkid + ":" + mbase.MachineName + "]"; //mbase.WorkspaceName + 
                }
                item.Tag = mbase;
                item.ImageIndex = mbase.WorkspaceTypeId == 3 ? 0 : 1;

                //if (file == null)
                if (classNode != null)
                    classNode.Nodes.Add(item); //always add object to class parented to either a file or workspace
                //else
                //    fileNode.Nodes.Add(item);

                //listViewItems.Nodes.Add(item);
            }
        }
        delegate void AddNodeDelegate(TreeNode node);
        public void AddNode(TreeNode node)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new AddNodeDelegate(AddNode), new object[] { node });
            }
            else
            {
                listViewItems.Nodes.Add(node);
                node.Expand();
                node.TreeView.Invalidate();
            }
        }

        /// <summary>
        /// Changes a nodes Tag to mBase and sets it text to ToString of mBase
        /// </summary>
        /// <param name="node"></param>
        /// <param name="mBase"></param>
        private void SetNodesMetaBase(TreeNode node, Meta.MetaBase mBase)
        {
            try
            {
                node.Tag = mBase;
                node.Text = mBase.ToString();
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog(ex.ToString());
            }
        }
        /// <summary>
        /// Update any node in the explorer whose pkid and machine are the same as the changedMetaBase
        /// Node properties are overwritten by changedMetabase
        /// </summary>
        /// <param name="changedMetaBase"></param>
        public void UpdateObjects(Meta.MetaBase changedMetaBase)
        {
            foreach (TreeNode node in listViewItems.Nodes)
            {
                if (node.Tag is Meta.MetaBase)
                {
                    if ((node.Tag as Meta.MetaBase).pkid == changedMetaBase.pkid && (node.Tag as Meta.MetaBase).MachineName == changedMetaBase.MachineName)
                        SetNodesMetaBase(node, changedMetaBase);
                }
                foreach (TreeNode nodec in node.Nodes)
                {
                    if (nodec.Tag is Meta.MetaBase)
                    {
                        if ((nodec.Tag as Meta.MetaBase).pkid == changedMetaBase.pkid && (nodec.Tag as Meta.MetaBase).MachineName == changedMetaBase.MachineName)
                            SetNodesMetaBase(nodec, changedMetaBase);
                    }
                    foreach (TreeNode nodecc in nodec.Nodes)
                    {
                        if (nodecc.Tag is Meta.MetaBase)
                        {
                            if ((nodecc.Tag as Meta.MetaBase).pkid == changedMetaBase.pkid && (nodecc.Tag as Meta.MetaBase).MachineName == changedMetaBase.MachineName)
                                SetNodesMetaBase(nodecc, changedMetaBase);
                        }
                        foreach (TreeNode nodeccc in nodecc.Nodes)
                        {
                            if (nodeccc.Tag is Meta.MetaBase)
                            {
                                if ((nodeccc.Tag as Meta.MetaBase).pkid == changedMetaBase.pkid && (nodeccc.Tag as Meta.MetaBase).MachineName == changedMetaBase.MachineName)
                                    SetNodesMetaBase(nodeccc, changedMetaBase);
                            }
                        }
                    }
                }
            }
        }

        //cancel current execution and start new
        private void filterChanged()
        {
            //Report(0, 0, "Changed");
            StartThread();
        }
        private void textBoxName_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (btnRefresh.Enabled)
                {
                    filterChanged();
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                btnReset_Click(sender, EventArgs.Empty);
            }
        }

        private void viewInContextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //foreach (TreeNode item in listViewItems.SelectedNode)
            TreeNode item = listViewItems.SelectedNode;
            if (item != null && item.Tag is Meta.MetaBase)
            {
                Meta.MetaBase mbase = item.Tag as Meta.MetaBase;
                if (mbase != null)
                {
                    LiteGraphViewContainer newContainer = new LiteGraphViewContainer();
                    newContainer.UseServer = (sender is ToolStripMenuItem) ? (sender as ToolStripMenuItem).Text.Contains("(Server)") : false;
                    newContainer.Setup(mbase);
                    //if (DockingForm.DockForm.GetCurrentGraphViewContainer() != null)
                    //    newContainer.Show(DockingForm.DockForm.GetCurrentGraphViewContainer().DockHandler.PanelPane, DockAlignment.Bottom, 0.25);
                    //else
                    newContainer.Show(DockingForm.DockForm.dockPanel1, DockState.Document);
                }
            }
            else if (item != null && item.Tag is GraphFile)
            {
                DockingForm.DockForm.OpenGraphFileFromDatabase(item.Tag as GraphFile, false, false);
            }
        }

        private void contextMenuStripObjects_Opening(object sender, CancelEventArgs e)
        {
            if (listViewItems.SelectedNode == null || listViewItems.SelectedNode.Tag == null || listViewItems.SelectedNode.ForeColor == Color.Gray)
            {
                e.Cancel = true;
            }
            else
            {
                if (listViewItems.SelectedNode.Tag is GraphFile)
                {
                    viewInContextToolStripMenuItem.Text = "Open File";
                    serverViewInContextToolStripMenuItem.Visible = false;
                }
                else
                {
                    viewInContextToolStripMenuItem.Text = "View In Context";
                    serverViewInContextToolStripMenuItem.Visible = Core.Variables.Instance.IsServer;
                }
            }
        }

        public void btnReset_Click(object sender, EventArgs e)
        {
            CancelThread = true;
            populate = null;

            Reset();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            CancelThread = true;
            populate = null;

            filterChanged();
        }

        private void listViewItems_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is GraphFile)
                DockingForm.DockForm.OpenGraphFileFromDatabase(e.Node.Tag as GraphFile, false, false);
        }

        private void comboWorkspace_Click(object sender, EventArgs e)
        {
            (sender as UIControls.Common.CheckBoxComboBox.CheckBoxComboBox).ShowDropDown();
        }
    }

    public class NodeSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            TreeNode tx = x as TreeNode;
            TreeNode ty = y as TreeNode;

            if ((tx.Tag is GraphFile && ty.Tag is GraphFile) || (tx.Tag == null && ty.Tag == null) || (tx.Tag is Meta.MetaBase && ty.Tag is Meta.MetaBase))
            {
                return string.Compare(tx.Text, ty.Text);
            }
            else
            {
                if (tx.Tag is GraphFile)
                    return -1;
                else
                    return 1;
            }

        }
    }

    [Serializable]
    public class DragAndDropMultiSelectTreeListView : TreeView
    {
        private ListViewItem m_previousItem;
        private bool m_allowReorder;
        private Color m_lineColor;

        [Category("Behavior")]
        public bool AllowReorder
        {
            get { return m_allowReorder; }
            set { m_allowReorder = value; }
        }

        [Category("Appearance")]
        public Color LineColor
        {
            get { return m_lineColor; }
            set { m_lineColor = value; }
        }

        public DragAndDropMultiSelectTreeListView()
            : base()
        {
            m_allowReorder = true;
            m_lineColor = Color.Red;

            //Activate double buffering
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            //Enable the OnNotifyMessage event so we get a chance to filter out 
            // Windows messages before they get to the form's WndProc
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);

            //ItemDrag += new ItemDragEventHandler(DragAndDropListView_ItemDrag);
            m_SelectedNodes = new List<TreeNode>();
            base.SelectedNode = null;
        }

        private List<TreeNode> m_SelectedNodes = null;
        public List<TreeNode> SelectedNodes
        {
            get
            {
                return m_SelectedNodes;
            }
            set
            {
                ClearSelectedNodes();
                if (value != null)
                {
                    foreach (TreeNode node in value)
                    {
                        ToggleNode(node, true);
                    }
                }
            }
        }

        // Note we use the new keyword to Hide the native treeview's 
        // SelectedNode property.
        private TreeNode m_SelectedNode;
        public new TreeNode SelectedNode
        {
            get
            {
                return m_SelectedNode;
            }
            set
            {
                ClearSelectedNodes();
                if (value != null)
                {
                    SelectNode(value);
                }
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            // Make sure at least one node has a selection
            // this way we can tab to the ctrl and use the
            // keyboard to select nodes
            try
            {
                if (m_SelectedNode == null && this.TopNode != null)
                {
                    ToggleNode(this.TopNode, true);

                    OnAfterSelect(new TreeViewEventArgs(m_SelectedNode));
                }
                HighlightSelection();

                base.OnGotFocus(e);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            // If the user clicks on a node that was not
            // previously selected, select it now.
            try
            {
                base.SelectedNode = null;

                TreeNode node = this.GetNodeAt(e.Location);
                if (node != null)
                {
                    //Allow user to click on image
                    int leftBound = node.Bounds.X; // - 20; 
                    // Give a little extra room
                    int rightBound = node.Bounds.Right + 10;
                    if (e.Location.X > leftBound && e.Location.X < rightBound)
                    {
                        if (ModifierKeys == Keys.None && (m_SelectedNodes.Contains(node)))
                        {
                            // Potential Drag Operation
                            // Let Mouse Up do select
                        }
                        else
                        {
                            SelectNode(node);
                        }
                    }
                }

                base.OnMouseDown(e);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            // If you clicked on a node that WAS previously
            // selected then, reselect it now. This will clear
            // any other selected nodes. e.g. A B C D are selected
            // the user clicks on B, now A C & D are no longer selected.
            try
            {
                // Check to see if a node was clicked on
                TreeNode node = this.GetNodeAt(e.Location);
                if (node != null)
                {
                    if (ModifierKeys == Keys.None && m_SelectedNodes.Contains(node) && m_SelectedNodes.Count > 1)
                    {
                        // Allow user to click on image
                        int leftBound = node.Bounds.X; // - 20; 
                        // Give a little extra room
                        int rightBound = node.Bounds.Right + 10;
                        if (e.Location.X > leftBound && e.Location.X < rightBound)
                        {
                            SelectNode(node);
                        }
                    }
                }

                base.OnMouseUp(e);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            // If the user drags a node and the node being dragged is NOT
            // selected, then clear the active selection, select the
            // node being dragged and drag it. Otherwise if the node being
            // dragged is selected, drag the entire selection.
            try
            {
                TreeNode node = e.Item as TreeNode;

                if (node != null)
                {
                    if (!m_SelectedNodes.Contains(node))
                    {
                        SelectSingleNode(node);
                        ToggleNode(node, true);
                    }
                }

                base.DoDragDrop(GetDataForDragDrop(), DragDropEffects.Copy);
                base.OnItemDrag(e);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
        {
            // Never allow base.SelectedNode to be set!
            try
            {
                base.SelectedNode = null;
                e.Cancel = true;

                base.OnBeforeSelect(e);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            // Never allow base.SelectedNode to be set!
            try
            {
                base.OnAfterSelect(e);
                base.SelectedNode = null;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            // Handle all possible key strokes for the control.
            // including navigation, selection, etc.

            base.OnKeyDown(e);

            if (e.KeyCode == Keys.ShiftKey) return;
            if (e.KeyCode == Keys.ControlKey) return;

            //this.BeginUpdate();
            bool bShift = (ModifierKeys == Keys.Shift);

            try
            {
                // Nothing is selected in the tree, this isn't a good state
                // select the top node
                if (m_SelectedNode == null && this.TopNode != null)
                {
                    ToggleNode(this.TopNode, true);
                }

                // Nothing is still selected in the tree, 
                // this isn't a good state, leave.
                if (m_SelectedNode == null) return;

                if (e.KeyCode == Keys.Left)
                {
                    if (m_SelectedNode.IsExpanded && m_SelectedNode.Nodes.Count > 0)
                    {
                        // Collapse an expanded node that has children
                        m_SelectedNode.Collapse();
                    }
                    else if (m_SelectedNode.Parent != null)
                    {
                        // Node is already collapsed, try to select its parent.
                        SelectSingleNode(m_SelectedNode.Parent);
                    }
                }
                else if (e.KeyCode == Keys.Right)
                {
                    if (!m_SelectedNode.IsExpanded)
                    {
                        // Expand a collapsed node's children
                        m_SelectedNode.Expand();
                    }
                    else
                    {
                        // Node was already expanded, select the first child
                        SelectSingleNode(m_SelectedNode.FirstNode);
                    }
                }
                else if (e.KeyCode == Keys.Up)
                {
                    // Select the previous node
                    if (m_SelectedNode.PrevVisibleNode != null)
                    {
                        SelectNode(m_SelectedNode.PrevVisibleNode);
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    // Select the next node
                    if (m_SelectedNode.NextVisibleNode != null)
                    {
                        SelectNode(m_SelectedNode.NextVisibleNode);
                    }
                }
                else if (e.KeyCode == Keys.Home)
                {
                    if (bShift)
                    {
                        if (m_SelectedNode.Parent == null)
                        {
                            // Select all of the root nodes up to this point
                            if (this.Nodes.Count > 0)
                            {
                                SelectNode(this.Nodes[0]);
                            }
                        }
                        else
                        {
                            // Select all of the nodes up to this point under 
                            // this nodes parent
                            SelectNode(m_SelectedNode.Parent.FirstNode);
                        }
                    }
                    else
                    {
                        // Select this first node in the tree
                        if (this.Nodes.Count > 0)
                        {
                            SelectSingleNode(this.Nodes[0]);
                        }
                    }
                }
                else if (e.KeyCode == Keys.End)
                {
                    if (bShift)
                    {
                        if (m_SelectedNode.Parent == null)
                        {
                            // Select the last ROOT node in the tree
                            if (this.Nodes.Count > 0)
                            {
                                SelectNode(this.Nodes[this.Nodes.Count - 1]);
                            }
                        }
                        else
                        {
                            // Select the last node in this branch
                            SelectNode(m_SelectedNode.Parent.LastNode);
                        }
                    }
                    else
                    {
                        if (this.Nodes.Count > 0)
                        {
                            // Select the last node visible node in the tree.
                            // Don't expand branches incase the tree is virtual
                            TreeNode ndLast = this.Nodes[this.Nodes.Count - 1].LastNode;
                            while (ndLast.IsExpanded && (ndLast.LastNode != null))
                            {
                                ndLast = ndLast.LastNode;
                            }
                            SelectSingleNode(ndLast);
                        }
                    }
                }
                else if (e.KeyCode == Keys.PageUp)
                {
                    // Select the highest node in the display
                    int nCount = this.VisibleCount;
                    TreeNode ndCurrent = m_SelectedNode;
                    while ((nCount) > 0 && (ndCurrent.PrevVisibleNode != null))
                    {
                        ndCurrent = ndCurrent.PrevVisibleNode;
                        nCount--;
                    }
                    SelectSingleNode(ndCurrent);
                }
                else if (e.KeyCode == Keys.PageDown)
                {
                    // Select the lowest node in the display
                    int nCount = this.VisibleCount;
                    TreeNode ndCurrent = m_SelectedNode;
                    while ((nCount) > 0 && (ndCurrent.NextVisibleNode != null))
                    {
                        ndCurrent = ndCurrent.NextVisibleNode;
                        nCount--;
                    }
                    SelectSingleNode(ndCurrent);
                }
                else
                {
                    // Assume this is a search character a-z, A-Z, 0-9, etc.
                    // Select the first node after the current node that
                    // starts with this character
                    string sSearch = ((char)e.KeyValue).ToString();

                    TreeNode ndCurrent = m_SelectedNode;
                    while ((ndCurrent.NextVisibleNode != null))
                    {
                        ndCurrent = ndCurrent.NextVisibleNode;
                        if (ndCurrent.Text.StartsWith(sSearch))
                        {
                            SelectSingleNode(ndCurrent);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                //this.EndUpdate();
            }
        }

        //protected void DragAndDropListView_ItemDrag(object sender, ItemDragEventArgs e)
        //{
        //    base.DoDragDrop(GetDataForDragDrop(), DragDropEffects.Copy);
        //}

        protected override void OnNotifyMessage(Message m)
        {
            //Filter out the WM_ERASEBKGND message
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            //if (!m_allowReorder)
            //{
            //    base.OnDragDrop(drgevent);
            //    return;
            //}

            //// get the currently hovered row that the items will be dragged to
            //Point clientPoint = base.PointToClient(new Point(drgevent.X, drgevent.Y));
            //ListViewItem hoverItem = base.GetItemAt(clientPoint.X, clientPoint.Y);

            //if (!drgevent.Data.GetDataPresent(typeof(DragItemData).ToString()) || ((DragItemData)drgevent.Data.GetData(typeof(DragItemData).ToString())).DragItems.Count == 0)// || ((DragItemData)drgevent.Data.GetData(typeof(DragItemData).ToString())).ListView == null
            //    return;

            //// retrieve the drag item data
            //DragItemData data = (DragItemData)drgevent.Data.GetData(typeof(DragItemData).ToString());

            //if (hoverItem == null)
            //{
            //    // the user does not wish to re-order the items, just append to the end
            //    for (int i = 0; i < data.DragItems.Count; i++)
            //    {
            //        ListViewItem newItem = (ListViewItem)data.DragItems[i];
            //        base.Items.Add(newItem);
            //    }
            //}
            //else
            //{
            //    // the user wishes to re-order the items

            //    // get the index of the hover item
            //    int hoverIndex = hoverItem.Index;

            //    // determine if the items to be dropped are from
            //    // this list view. If they are, perform a hack
            //    // to increment the hover index so that the items
            //    // get moved properly.
            //    //if (this == data.ListView)
            //    //{
            //    //    if (hoverIndex > base.SelectedItems[0].Index)
            //    //        hoverIndex++;
            //    //}

            //    // insert the new items into the list view
            //    // by inserting the items reversely from the array list
            //    for (int i = data.DragItems.Count - 1; i >= 0; i--)
            //    {
            //        ListViewItem newItem = (ListViewItem)data.DragItems[i];
            //        base.Items.Insert(hoverIndex, newItem);
            //    }
            //}

            //// remove all the selected items from the previous list view
            //// if the list view was found
            ////if (data.ListView != null)
            ////{
            ////    foreach (ListViewItem itemToRemove in data.ListView.SelectedItems)
            ////    {
            ////        data.ListView.Items.Remove(itemToRemove);
            ////    }
            ////}

            //// set the back color of the previous item, then nullify it
            //if (m_previousItem != null)
            //{
            //    m_previousItem = null;
            //}

            //this.Invalidate();

            //// call the base on drag drop to raise the event
            //base.OnDragDrop(drgevent);
        }

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            if (!drgevent.Data.GetDataPresent(typeof(DragItemData).ToString()))
            {
                // the item(s) being dragged do not have any data associated
                drgevent.Effect = DragDropEffects.None;
                return;
            }

            if (!m_allowReorder)
            {
                base.OnDragOver(drgevent);
                return;
            }

            //if (base.Items.Count > 0)
            //{
            //    // get the currently hovered row that the items will be dragged to
            //    Point clientPoint = base.PointToClient(new Point(drgevent.X, drgevent.Y));
            //    ListViewItem hoverItem = base.GetItemAt(clientPoint.X, clientPoint.Y);

            //    Graphics g = this.CreateGraphics();

            //    if (hoverItem == null)
            //    {
            //        //MessageBox.Show(this,base.GetChildAtPoint(new Point(clientPoint.X, clientPoint.Y)).GetType().ToString());

            //        // no item was found, so no drop should take place
            //        drgevent.Effect = DragDropEffects.Move;

            //        if (m_previousItem != null)
            //        {
            //            m_previousItem = null;
            //            Invalidate();
            //        }

            //        hoverItem = base.Items[base.Items.Count - 1];

            //        if (this.View == View.Details || this.View == View.List)
            //        {
            //            g.DrawLine(new Pen(m_lineColor, 2), new Point(hoverItem.Bounds.X, hoverItem.Bounds.Y + hoverItem.Bounds.Height), new Point(hoverItem.Bounds.X + this.Bounds.Width, hoverItem.Bounds.Y + hoverItem.Bounds.Height));
            //            g.FillPolygon(new SolidBrush(m_lineColor), new Point[] { new Point(hoverItem.Bounds.X, hoverItem.Bounds.Y + hoverItem.Bounds.Height - 5), new Point(hoverItem.Bounds.X + 5, hoverItem.Bounds.Y + hoverItem.Bounds.Height), new Point(hoverItem.Bounds.X, hoverItem.Bounds.Y + hoverItem.Bounds.Height + 5) });
            //            g.FillPolygon(new SolidBrush(m_lineColor), new Point[] { new Point(this.Bounds.Width - 4, hoverItem.Bounds.Y + hoverItem.Bounds.Height - 5), new Point(this.Bounds.Width - 9, hoverItem.Bounds.Y + hoverItem.Bounds.Height), new Point(this.Bounds.Width - 4, hoverItem.Bounds.Y + hoverItem.Bounds.Height + 5) });
            //        }
            //        else
            //        {
            //            g.DrawLine(new Pen(m_lineColor, 2), new Point(hoverItem.Bounds.X + hoverItem.Bounds.Width, hoverItem.Bounds.Y), new Point(hoverItem.Bounds.X + hoverItem.Bounds.Width, hoverItem.Bounds.Y + hoverItem.Bounds.Height));
            //            g.FillPolygon(new SolidBrush(m_lineColor), new Point[] { new Point(hoverItem.Bounds.X + hoverItem.Bounds.Width - 5, hoverItem.Bounds.Y), new Point(hoverItem.Bounds.X + hoverItem.Bounds.Width + 5, hoverItem.Bounds.Y), new Point(hoverItem.Bounds.X + hoverItem.Bounds.Width, hoverItem.Bounds.Y + 5) });
            //            g.FillPolygon(new SolidBrush(m_lineColor), new Point[] { new Point(hoverItem.Bounds.X + hoverItem.Bounds.Width - 5, hoverItem.Bounds.Y + hoverItem.Bounds.Height), new Point(hoverItem.Bounds.X + hoverItem.Bounds.Width + 5, hoverItem.Bounds.Y + hoverItem.Bounds.Height), new Point(hoverItem.Bounds.X + hoverItem.Bounds.Width, hoverItem.Bounds.Y + hoverItem.Bounds.Height - 5) });
            //        }

            //        // call the base OnDragOver event
            //        base.OnDragOver(drgevent);

            //        return;
            //    }

            //    // determine if the user is currently hovering over a new
            //    // item. If so, set the previous item's back color back
            //    // to the default color.
            //    if ((m_previousItem != null && m_previousItem != hoverItem) || m_previousItem == null)
            //    {
            //        this.Invalidate();
            //    }

            //    // set the background color of the item being hovered
            //    // and assign the previous item to the item being hovered
            //    //hoverItem.BackColor = Color.Beige;
            //    m_previousItem = hoverItem;

            //    if (this.View == View.Details || this.View == View.List)
            //    {
            //        g.DrawLine(new Pen(m_lineColor, 2), new Point(hoverItem.Bounds.X, hoverItem.Bounds.Y), new Point(hoverItem.Bounds.X + this.Bounds.Width, hoverItem.Bounds.Y));
            //        g.FillPolygon(new SolidBrush(m_lineColor), new Point[] { new Point(hoverItem.Bounds.X, hoverItem.Bounds.Y - 5), new Point(hoverItem.Bounds.X + 5, hoverItem.Bounds.Y), new Point(hoverItem.Bounds.X, hoverItem.Bounds.Y + 5) });
            //        g.FillPolygon(new SolidBrush(m_lineColor), new Point[] { new Point(this.Bounds.Width - 4, hoverItem.Bounds.Y - 5), new Point(this.Bounds.Width - 9, hoverItem.Bounds.Y), new Point(this.Bounds.Width - 4, hoverItem.Bounds.Y + 5) });
            //    }
            //    else
            //    {
            //        g.DrawLine(new Pen(m_lineColor, 2), new Point(hoverItem.Bounds.X, hoverItem.Bounds.Y), new Point(hoverItem.Bounds.X, hoverItem.Bounds.Y + hoverItem.Bounds.Height));
            //        g.FillPolygon(new SolidBrush(m_lineColor), new Point[] { new Point(hoverItem.Bounds.X - 5, hoverItem.Bounds.Y), new Point(hoverItem.Bounds.X + 5, hoverItem.Bounds.Y), new Point(hoverItem.Bounds.X, hoverItem.Bounds.Y + 5) });
            //        g.FillPolygon(new SolidBrush(m_lineColor), new Point[] { new Point(hoverItem.Bounds.X - 5, hoverItem.Bounds.Y + hoverItem.Bounds.Height), new Point(hoverItem.Bounds.X + 5, hoverItem.Bounds.Y + hoverItem.Bounds.Height), new Point(hoverItem.Bounds.X, hoverItem.Bounds.Y + hoverItem.Bounds.Height - 5) });
            //    }

            //    // go through each of the selected items, and if any of the
            //    // selected items have the same index as the item being
            //    // hovered, disable dropping.
            //    foreach (ListViewItem itemToMove in base.SelectedItems)
            //    {
            //        if (itemToMove.Index == hoverItem.Index)
            //        {
            //            drgevent.Effect = DragDropEffects.None;
            //            hoverItem.EnsureVisible();
            //            return;
            //        }
            //    }

            //    // ensure that the hover item is visible
            //    hoverItem.EnsureVisible();
            //}

            //// everything is fine, allow the user to move the items
            //drgevent.Effect = DragDropEffects.Move;

            // call the base OnDragOver event
            base.OnDragOver(drgevent);
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            if (!m_allowReorder)
            {
                base.OnDragEnter(drgevent);
                return;
            }

            if (!drgevent.Data.GetDataPresent(typeof(DragItemData).ToString()))
            {
                // the item(s) being dragged do not have any data associated
                drgevent.Effect = DragDropEffects.None;
                return;
            }

            // everything is fine, allow the user to move the items
            drgevent.Effect = DragDropEffects.Move;

            // call the base OnDragEnter event
            base.OnDragEnter(drgevent);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            try
            {
                // Handle if HideSelection property is in use.
                if (this.HideSelection)
                {
                    DimSelection();
                }

                base.OnLostFocus(e);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        protected override void OnDragLeave(EventArgs e)
        {
            // reset the selected items background and remove the previous item
            ResetOutOfRange();

            Invalidate();

            // call the base OnDragLeave event
            base.OnDragLeave(e);
        }

        private DragItemData GetDataForDragDrop()
        {
            // create a drag item data object that will be used to pass along with the drag and drop
            DragItemData data = new DragItemData();
            foreach (TreeNode item in SelectedNodes)
            {
                if (item is ItemTreeNode)
                {
                    if (item != null && item.Tag is Meta.MetaBase)
                    {
                        (item.Tag as Meta.MetaBase).tag = (item.Tag as Meta.MetaBase).Class;
                        if ((item as ItemTreeNode).ForceShapeType != null)
                            (item.Tag as Meta.MetaBase).ForceShapeType = (item as ItemTreeNode).ForceShapeType;
                        data.DragItems.Add((item.Tag as Meta.MetaBase));
                    }
                }
            }
            return data;
        }

        private void ResetOutOfRange()
        {
            // determine if the previous item exists,
            // if it does, reset the background and release 
            // the previous item
            if (m_previousItem != null)
            {
                m_previousItem = null;
            }

        }

        #region Helper Methods

        private void SelectNode(TreeNode node)
        {
            try
            {
                if (node.ForeColor == Color.Gray)
                    return;
                if (node.Tag is Meta.MetaBase)
                    DockingForm.DockForm.ShowMetaObjectProperties(node.Tag as Meta.MetaBase, false);
                //this.BeginUpdate();

                if (m_SelectedNode == null || ModifierKeys == Keys.Control)
                {
                    // Ctrl+Click selects an unselected node, 
                    // or unselects a selected node.
                    bool bIsSelected = m_SelectedNodes.Contains(node);
                    ToggleNode(node, !bIsSelected);
                }
                else if (ModifierKeys == Keys.Shift)
                {
                    // Shift+Click selects nodes between the selected node and here.
                    TreeNode ndStart = m_SelectedNode;
                    TreeNode ndEnd = node;

                    if (ndStart.Parent == ndEnd.Parent)
                    {
                        // Selected node and clicked node have same parent, easy case.
                        if (ndStart.Index < ndEnd.Index)
                        {
                            // If the selected node is beneath 
                            // the clicked node walk down
                            // selecting each Visible node until we reach the end.
                            while (ndStart != ndEnd)
                            {
                                ndStart = ndStart.NextVisibleNode;
                                if (ndStart == null) break;
                                ToggleNode(ndStart, true);
                            }
                        }
                        else if (ndStart.Index == ndEnd.Index)
                        {
                            // Clicked same node, do nothing
                        }
                        else
                        {
                            // If the selected node is above the clicked node walk up
                            // selecting each Visible node until we reach the end.
                            while (ndStart != ndEnd)
                            {
                                ndStart = ndStart.PrevVisibleNode;
                                if (ndStart == null) break;
                                ToggleNode(ndStart, true);
                            }
                        }
                    }
                    else
                    {
                        // Selected node and clicked node have same parent, hard case.
                        // We need to find a common parent to determine if we need
                        // to walk down selecting, or walk up selecting.

                        TreeNode ndStartP = ndStart;
                        TreeNode ndEndP = ndEnd;
                        int startDepth = Math.Min(ndStartP.Level, ndEndP.Level);

                        // Bring lower node up to common depth
                        while (ndStartP.Level > startDepth)
                        {
                            ndStartP = ndStartP.Parent;
                        }

                        // Bring lower node up to common depth
                        while (ndEndP.Level > startDepth)
                        {
                            ndEndP = ndEndP.Parent;
                        }

                        // Walk up the tree until we find the common parent
                        while (ndStartP.Parent != ndEndP.Parent)
                        {
                            ndStartP = ndStartP.Parent;
                            ndEndP = ndEndP.Parent;
                        }

                        // Select the node
                        if (ndStartP.Index < ndEndP.Index)
                        {
                            // If the selected node is beneath 
                            // the clicked node walk down
                            // selecting each Visible node until we reach the end.
                            while (ndStart != ndEnd)
                            {
                                ndStart = ndStart.NextVisibleNode;
                                if (ndStart == null) break;
                                ToggleNode(ndStart, true);
                            }
                        }
                        else if (ndStartP.Index == ndEndP.Index)
                        {
                            if (ndStart.Level < ndEnd.Level)
                            {
                                while (ndStart != ndEnd)
                                {
                                    ndStart = ndStart.NextVisibleNode;
                                    if (ndStart == null) break;
                                    ToggleNode(ndStart, true);
                                }
                            }
                            else
                            {
                                while (ndStart != ndEnd)
                                {
                                    ndStart = ndStart.PrevVisibleNode;
                                    if (ndStart == null) break;
                                    ToggleNode(ndStart, true);
                                }
                            }
                        }
                        else
                        {
                            // If the selected node is above 
                            // the clicked node walk up
                            // selecting each Visible node until we reach the end.
                            while (ndStart != ndEnd)
                            {
                                ndStart = ndStart.PrevVisibleNode;
                                if (ndStart == null) break;
                                ToggleNode(ndStart, true);
                            }
                        }
                    }
                }
                else
                {
                    // Just clicked a node, select it
                    SelectSingleNode(node);
                }

                OnAfterSelect(new TreeViewEventArgs(m_SelectedNode));
            }
            finally
            {
                //this.EndUpdate();
            }
        }

        private void ClearSelectedNodes()
        {
            try
            {
                foreach (TreeNode node in m_SelectedNodes)
                {
                    node.BackColor = this.BackColor;
                    node.ForeColor = this.ForeColor;
                }
            }
            finally
            {
                m_SelectedNodes.Clear();
                m_SelectedNode = null;
            }
        }

        private void SelectSingleNode(TreeNode node)
        {
            if (node == null)
            {
                return;
            }

            ClearSelectedNodes();
            ToggleNode(node, true);
            node.EnsureVisible();
        }

        private void ToggleNode(TreeNode node, bool bSelectNode)
        {
            if (bSelectNode)
            {
                if (node.ForeColor == Color.Gray)
                    return;

                m_SelectedNode = node;
                if (!m_SelectedNodes.Contains(node))
                {
                    m_SelectedNodes.Add(node);
                }
                node.BackColor = SystemColors.Highlight;
                node.ForeColor = SystemColors.HighlightText;
            }
            else
            {
                m_SelectedNodes.Remove(node);
                node.BackColor = this.BackColor;
                node.ForeColor = this.ForeColor;
            }
        }

        private void HandleException(Exception ex)
        {
            // Perform some error handling here.
            // We don't want to bubble errors to the CLR.
            Core.Log.WriteLog(ex.Message);
        }

        private void HighlightSelection()
        {
            foreach (TreeNode node in SelectedNodes)
            {
                node.BackColor = SystemColors.Highlight;
                node.ForeColor = SystemColors.HighlightText;
            }
        }

        private void DimSelection()
        {
            foreach (TreeNode node in SelectedNodes)
            {
                node.BackColor = SystemColors.Control;
                node.ForeColor = this.ForeColor;
            }
        }

        #endregion

    }

    [Serializable]
    public class DragItemData
    {
        private Type forceShapeType;
        public Type ForceShapeType
        {
            get { return forceShapeType; }
            set { forceShapeType = value; }
        }

        #region Private Members

        //private DragAndDropListView m_listView;
        private ArrayList m_dragItems;

        #endregion

        #region Public Properties

        //public DragAndDropListView ListView
        //{
        //    get { return m_listView; }
        //}

        public ArrayList DragItems
        {
            get { return m_dragItems; }
        }

        #endregion

        #region Public Methods and Implementation

        public DragItemData()
        {
            //m_listView = listView;
            m_dragItems = new ArrayList();
        }

        #endregion
    }


    public class ItemTreeNode : TreeNode
    {
        private Type forceShapeType;
        public Type ForceShapeType
        {
            get { return forceShapeType; }
            set { forceShapeType = value; }
        }
    }
}