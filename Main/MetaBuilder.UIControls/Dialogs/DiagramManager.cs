using System;
using System.Globalization;
using System.Windows.Forms;
using MetaBuilder.BusinessFacade.Storage;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.DataAccessLayer.OldCode.Diagramming;
using MetaBuilder.UIControls.GraphingUI;
//using MetaBuilder.BusinessFacade.MetaHelper;
using System.Collections.Generic;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
using MetaBuilder.BusinessFacade.Storage.RepositoryTemp;

namespace MetaBuilder.UIControls.Dialogs
{
    public partial class DiagramManager : Form
    {

        public TList<Workspace> ServerWorkspacesUserHasWithAdminPermission;

        #region Constructors (1)

        bool Server = false;
        public DiagramManager(bool server, TList<Workspace> serverAdminWorkspaces)
        {
            Server = server;
            ServerWorkspacesUserHasWithAdminPermission = serverAdminWorkspaces;
            InitializeComponent();
            if (server)
                Text += " (Server)";
        }

        #endregion Constructors

        #region Methods (10)

        public TreeListViewItem GetTreeItem(b.GraphFile file)
        {
            TreeListViewItem item = new TreeListViewItem(Core.strings.GetFileNameWithoutExtension(file.Name));
            item.SubItems.Add(file.MajorVersion.ToString() + "." + file.MinorVersion.ToString());
            item.SubItems.Add(file.ModifiedDate.ToString("D", CultureInfo.CreateSpecificCulture("en-US")));
            item.SubItems.Add(file.ModifiedDate.ToShortTimeString());
            item.SubItems.Add(file.WorkspaceName + (file.WorkspaceTypeId == 3 ? " (Server)" : " (Client)"));
            item.SubItems.Add(((VCStatusList)file.VCStatusID).ToString());
            item.ImageIndex = 0;
            item.Tag = file;
            return item;
        }

        // Private Methods (9) 
        LogDisplayer mylog;
        private void btnDeleteSelectedDiagrams_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show(this, "Are you sure you want to delete the selected diagrams?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (res == DialogResult.Yes)
            {
                mylog = new LogDisplayer();
                mylog.Text = "Delete Diagram Log";
                foreach (TreeListViewItem n in treeListView1.Items)
                {
                    foreach (TreeListViewItem nChild in n.Items)
                    {
                        if (nChild.Checked)
                        {
                            DeleteFileIfAllowed(nChild);
                        }
                    }
                    //Delete all selected children before the parent
                    if (n.Checked)
                    {
                        DeleteFileIfAllowed(n);
                        //markLatestActive(n.Tag as GraphFile);
                    }
                }
                if (mylog.Messages > 0)
                    mylog.ShowDialog(this);
            }
            //5 November 2013 REMOVED HACK
            //28 november 2013 added it back
            WorkspaceHelper.MarkLatestDrawingsAsActive();
            UpdateList();
        }

        private void markLatestActive(GraphFile file)
        {
            //find the highest version has control over file and mark it active
            string provider = Core.Variables.Instance.ClientProvider;
            if (Server)
                provider = Core.Variables.Instance.ServerProvider;
            GraphFile newActiveFile = null;
            TList<GraphFile> childFiles = new TList<GraphFile>();
            foreach (GraphFile f in d.DataRepository.Connections[provider].Provider.GraphFileProvider.GetAll())
            {
                if (f.OriginalFileUniqueID == file.OriginalFileUniqueID)
                {
                    childFiles.Add(f);
                    if (f.VCStatusID != 2)
                    {
                        if (newActiveFile == null)
                        {
                            newActiveFile = f;
                        }
                        else
                        {
                            if (newActiveFile.MajorVersion < f.MajorVersion)
                            {
                                newActiveFile = f;
                            }
                            else if (newActiveFile.MinorVersion < f.MinorVersion)
                            {
                                newActiveFile = f;
                            }
                        }
                    }
                }
            }
            if (newActiveFile == null && childFiles.Count > 0)
            {
                string areIS = childFiles.Count == 1 ? "is still 1 diagram" : "are still " + childFiles.Count.ToString() + " diagrams";

                if (MessageBox.Show(this, "Please note that after the delete process an active file cannot be set. However there " + areIS + " which are still checked out that cannot be marked active." + Environment.NewLine + "Would you like to delete these diagrams as well?", "Orphaned Diagrams", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    foreach (GraphFile cFile in childFiles)
                        DeleteFile(cFile);
                }
                return;
            }
            newActiveFile.IsActive = true;
            d.DataRepository.Connections[provider].Provider.GraphFileProvider.Update(newActiveFile);
        }

        public bool Opened = false;
        private void btnOpen_Click_1(object sender, EventArgs e)
        {
            if (Server)
            {
                //TODO : Unless you are the admin in that workspace and have it locked or checkedout!
                MessageBox.Show(this, "You cannot open a diagram directly from the server without using the syncronisation interface", "Cannot open");
                return;
            }
            string provider = Server ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider;
            foreach (TreeListViewItem n in treeListView1.Items)
            {
                if (n.Checked)
                {
                    GraphFile file = n.Tag as GraphFile;
                    GraphFile retrievedFile = DataRepository.Connections[provider].Provider.GraphFileProvider.GetBypkidMachine(file.pkid, file.Machine);
                    DockingForm.DockForm.OpenGraphFileFromDatabase(retrievedFile, false, false);
                    if (retrievedFile.State == VCStatusList.CheckedOutRead || retrievedFile.State == VCStatusList.CheckedIn)
                    {
                        DockingForm.DockForm.GetCurrentGraphViewContainer().ReadOnly = true;
                    }
                    Opened = true;
                }
                foreach (TreeListViewItem nChild in n.Items)
                {
                    if (nChild.Checked)
                    {
                        GraphFile cfile = nChild.Tag as GraphFile;
                        GraphFile retrievedFileC = DataRepository.Connections[provider].Provider.GraphFileProvider.GetBypkidMachine(cfile.pkid, cfile.Machine);
                        DockingForm.DockForm.OpenGraphFileFromDatabase(retrievedFileC, true, false);
                        if (retrievedFileC.State == VCStatusList.CheckedOutRead || retrievedFileC.State == VCStatusList.CheckedIn)
                        {
                            DockingForm.DockForm.GetCurrentGraphViewContainer().ReadOnly = true;
                        }
                        Opened = true;
                    }
                }
            }

            if (Opened)
                Close();
        }

        private void treeListView1_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
        {
            //foreach (ListViewItem item in (sender as TreeListView).FocusedItem.Items)
            //{
            //    item.Checked = !(sender as TreeListView).FocusedItem.Checked;
            //}
        }

        List<b.MetaObject> keys;
        private void DeleteFile(GraphFile file)
        {
            keys = new List<b.MetaObject>();
            if (file.VCMachineID.Length > 0)
            {
                TempFileGraphAdapter tfga = new TempFileGraphAdapter();
                TList<GraphFile> olderVersions = tfga.GetPreviousVersions(file.pkid, file.Machine, Server);
                foreach (GraphFile oldFile in olderVersions)
                {
                    addObjectsToCheckAfterDelete(oldFile);
                    WorkspaceHelper.DeleteGraphFile(oldFile.pkid, oldFile.Machine, Server);
                }
                addObjectsToCheckAfterDelete(file);
                WorkspaceHelper.DeleteGraphFile(file.pkid, file.Machine, Server);
            }
            else
            {
                addObjectsToCheckAfterDelete(file);
                WorkspaceHelper.DeleteGraphFile(file.pkid, file.Machine, Server);
            }

            removeOrphanObjects();
        }
        private void addObjectsToCheckAfterDelete(GraphFile file)
        {
            foreach (b.MetaObject gfo in DataAccessLayer.DataRepository.Connections[Server ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetByGraphFileIDGraphFileMachineFromGraphFileObject(file.pkid, file.MachineName))
            {
                //string k = gfo.MetaObjectID + ":" + gfo.MachineID;
                if (!keys.Contains(gfo))
                    keys.Add(gfo);
            }
        }
        private void removeOrphanObjects()
        {
            //return;
            MetaBuilder.BusinessFacade.MetaHelper.ObjectHelper ohelper = new MetaBuilder.BusinessFacade.MetaHelper.ObjectHelper(Server);
            foreach (b.MetaObject k in keys)
            {
                Meta.MetaBase mbase = Meta.Loader.GetFromProvider(k.pkid, k.Machine, k.Class, Server);
                //delete when no gfo
                if (DataAccessLayer.DataRepository.Connections[Server ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider].Provider.GraphFileObjectProvider.GetByMetaObjectIDMachineID(k.pkid, k.Machine).Count == 0)
                {
                    //no occurences
                    ActionResult result = new ActionResult();
                    result.Repository = Server ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider;
                    try
                    {
                        ohelper.DeleteObject(k.pkid, k.Machine, true);
                        result.Success = true;
                    }
                    catch
                    {
                        result.Success = false;
                    }
                    result.FromState = mbase.State.ToString();
                    result.TargetState = "Deleted";
                    result.Message = mbase.ToString();

                    mylog.AddMessage(result);
                }
                else
                {
                    //display log of cannot delete objects
                    ActionResult result = new ActionResult();
                    result.Repository = Server ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider;
                    result.Success = false;
                    result.intermediate = true;
                    result.FromState = mbase.State.ToString();
                    result.TargetState = "Occurence found";
                    result.Message = mbase.ToString();
                    //add to deleted objects (error)?
                    mylog.AddMessage(result);
                }
            }
            //database blob may still have the object?
            //MetaBuilder.UIControls.GraphingUI.Tools.DeleteObjectsFromFiles.DeleteObjects(keys, filesToDeleteFrom, Core.Variables.Instance.ServerProvider);
        }

        private void DeleteFileIfAllowed(TreeListViewItem n)
        {
            GraphFile file = n.Tag as GraphFile;
            bool candelete = true;
            if (file.VCMachineID.Length > 0)
            {
                if (file.IsActive && file.State == VCStatusList.CheckedOut)
                {
                    if (Core.Variables.Instance.IsServer && Core.Variables.Instance.ServerConnectionString != null)
                    {
                        //try get file on server
                        try
                        {
                            if (!Server)
                            {
                                foreach (GraphFile f in d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.GraphFileProvider.GetByWorkspaceNameWorkspaceTypeId(file.WorkspaceName, file.WorkspaceTypeId))
                                {
                                    if (f.OriginalFileUniqueID == file.OriginalFileUniqueID && f.IsActive)
                                    {
                                        if (f.VCStatusID == 2 && f.VCUser == Core.strings.GetVCIdentifier())
                                        {
                                            candelete = false;
                                            MessageBox.Show(this, "Diagram: " + strings.GetFileNameOnly(file.Name) + " v" + file.MajorVersion.ToString() + "." + file.MinorVersion.ToString() + " cannot be deleted because you currently have it checkedout.", "Unable to delete file", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (file.VCStatusID != 1)
                                {
                                    candelete = false;
                                    MessageBox.Show(this, "Diagram: " + strings.GetFileNameOnly(file.Name) + " v" + file.MajorVersion.ToString() + "." + file.MinorVersion.ToString() + " cannot be deleted because it is not checked in.", "Unable to delete file", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                        }
                        catch
                        {
                            MessageBox.Show(this, "Diagram: " + strings.GetFileNameOnly(file.Name) + " v" + file.MajorVersion.ToString() + "." + file.MinorVersion.ToString() + " cannot be deleted because your synchronisation server cannot be contacted.", "Unable to delete file", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            candelete = false;
                        }
                    }
                }
            }
            if (candelete)
                DeleteFile(file);
        }

        private void DiagramManager_Load(object sender, EventArgs e)
        {
            UpdateList();
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {

        }
        public TList<GraphFile> files;

        private void UpdateList()
        {
            treeListView1.Items.Clear();

            TempFileGraphAdapter tfga = new TempFileGraphAdapter();
            files = tfga.GetAllFilesByTypeID((int)FileTypeList.Diagram, Server);
            files.Sort("ModifiedDate");
            foreach (GraphFile file in files)
            {
                if (file.IsActive)
                {
                    bool isServerAdmin = true;
                    if (Server)
                    {
                        isServerAdmin = false;
                        //if file in admin workspaces
                        foreach (Workspace ws in ServerWorkspacesUserHasWithAdminPermission)
                        {
                            if (file.WorkspaceName == ws.Name && file.WorkspaceTypeId == ws.WorkspaceTypeId)
                            {
                                isServerAdmin = true;
                                break;
                            }
                        }
                    }
                    if (!isServerAdmin)
                        continue;

                    TreeListViewItem item = GetTreeItem(file);
                    item.Checked = false;
                    TList<GraphFile> olderVersions = tfga.GetPreviousVersions(file.pkid, file.Machine, Server);
                    if (file.VCMachineID.Length > 0)
                    {
                        foreach (GraphFile oldFile in olderVersions)
                        {
                            //if (oldFile.State != VCStatusList.CheckedOut)
                            //{
                            TreeListViewItem olditem = GetTreeItem(oldFile);
                            olditem.Checked = false;
                            item.Items.Add(olditem);
                            //}
                        }
                    }
                    treeListView1.Items.Add(item);
                }
            }
            treeListView1.AllowDrop = false;
        }

        #endregion Methods

        #region Nested Classes (1)

        class GraphFileListItem
        {
            #region Fields (2)

            private string caption;
            private GraphFile file;

            #endregion Fields

            #region Properties (2)

            public string Caption
            {
                get { return caption; }
                set { caption = value; }
            }

            public GraphFile File
            {
                get { return file; }
                set { file = value; }
            }

            #endregion Properties

            #region Methods (1)

            // Public Methods (1) 

            public override string ToString()
            {
                return caption;
            }

            #endregion Methods

        }
        #endregion Nested Classes

    }
}