using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MetaBuilder.UIControls.GraphingUI;
using MetaBuilder.BusinessFacade.Storage.RepositoryTemp;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Meta;

namespace MetaBuilder.UIControls.Dialogs.DatabaseManagement
{
    public partial class Synchroniser : Form
    {
        public Synchroniser()
        {
            InitializeComponent();
        }

        LogDisplayer log = new LogDisplayer();
        private void progressBar_Click(object sender, EventArgs e)
        {
            log.BringToFront();
        }

        private void Synchroniser_Load(object sender, EventArgs e)
        {
            labelProgress.Text = "";
            log.MdiParent = this;
            log.Show();
            log.FormBorderStyle = FormBorderStyle.None;
            log.Dock = DockStyle.Fill;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            panelProgress.Visible = true;
            buttonCancel.Text = "Cancel";
            Start();
            progressBar.Value = 50;
            if (log.IsDisposed)
                log = new LogDisplayer();
            else
                log.Clear();
            log.MdiParent = this;
            log.Show();
            log.FormBorderStyle = FormBorderStyle.None;
            log.Dock = DockStyle.Fill;
        }

        private bool cancel;
        public bool Cancel
        {
            get { return cancel; }
            set { cancel = value; }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (buttonCancel.Text == "Close")
            {
                Close();
            }
            else
            {
                Cancel = true;
                this.ControlBox = true;
                buttonCancel.Enabled = false;
            }
        }
        BackgroundWorker bg = null;
        private void Start()
        {
            buttonStart.Enabled = false;
            files = null;
            fileAssociations = null;
            fileObjects = null;

            bg = new BackgroundWorker();
            bg.WorkerReportsProgress = true;
            bg.WorkerSupportsCancellation = true;
            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);
            bg.ProgressChanged += new ProgressChangedEventHandler(bg_ProgressChanged);
            bg.DoWork += new DoWorkEventHandler(bg_DoWork);
            bg.RunWorkerAsync();
        }

        void bg_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            labelProgress.Text = e.UserState.ToString();
        }
        void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonStart.Enabled = true;
            buttonCancel.Text = "Close";
            buttonCancel.Enabled = true;
            if (Cancel)
                Close();
        }
        void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                bg.ReportProgress(5, "Permissions");
                //Permissions 
                PermissionService perm = new PermissionService();
                log.AddMessage("Workspaces");
                TList<Workspace> workspaces = perm.GetAccessibleServerWorkspaces();
                if (Cancel)
                    return;

                bg.ReportProgress(10, "Workspace Synchronisation");
                log.AddMessage("Workspace (" + workspaces.Count.ToString() + ") Sync");
                perm.SynchroniseServerWorkspaces();

                log.AddMessage("Files");
                #region Files
                TList<GraphFile> files = new TList<GraphFile>();
                bg.ReportProgress(20, "Retrieving Files");
                if (Cancel)
                    return;
                foreach (GraphFile file in DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.GraphFileProvider.GetAll())
                {
                    if (Cancel)
                        break;

                    if (file.IsActive == false)
                        continue;

                    foreach (Workspace w in workspaces)
                    {
                        if (file.WorkspaceName == w.Name && file.WorkspaceTypeId == w.WorkspaceTypeId)
                        {
                            files.Add(file);
                            break;
                        }
                    }
                }
                bg.ReportProgress(30, "Synchronising Files");
                foreach (GraphFile file in files)
                {
                    if (Cancel)
                        break;
                    ActionResult r = new ActionResult();
                    r.Message = Core.strings.GetFileNameOnly(file.Name);
                    r.Item = Core.strings.GetFileNameOnly(file.Name);
                    r.Repository = Core.Variables.Instance.ClientProvider;


                    r.FromState = "N/A";
                    //r.TargetState = "CheckedOutRead";

                    //save file
                    r = SyncFile(file, r);

                    log.AddMessage(r);
                }
                #endregion

                if (Cancel)
                    return;

                log.AddMessage("Objects");
                TList<ObjectAssociation> associations = new TList<ObjectAssociation>(); //each object that is "added" must get associated
                #region Objects
                List<MetaBase> objects = new List<MetaBase>();
                int c = 0;
                bg.ReportProgress(40, "Retrieving Objects");
                foreach (MetaObject dbObj in DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.MetaObjectProvider.GetAll())
                {
#if DEBUG
                    if (objects.Count > 25)
                    {
                        //break;
                    }
#endif

                    c++;

                    bg.ReportProgress(40, "Retrieving Objects " + ((c % 2 == 0) ? "|" : "--"));

                    if (Cancel)
                        break;

                    foreach (Workspace w in workspaces)
                    {
                        if (dbObj.WorkspaceName == w.Name && dbObj.WorkspaceTypeId == w.WorkspaceTypeId)
                        {
                            objects.Add(Loader.GetFromProvider(dbObj.pkid, dbObj.Machine, dbObj.Class, true));

                            bg.ReportProgress(40, "Retrieving Objects " + ((c % 2 == 0) ? "|" : "--") + " as parent");
                            associations.AddRange(DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(dbObj.pkid, dbObj.Machine));
                            if (Cancel)
                                break;
                            bg.ReportProgress(40, "Retrieving Objects " + ((c % 2 == 0) ? "|" : "--") + " as child");
                            associations.AddRange(DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.ObjectAssociationProvider.GetByChildObjectIDChildObjectMachine(dbObj.pkid, dbObj.Machine));
                            if (Cancel)
                                break;

                            break;
                        }
                    }
                }
                bg.ReportProgress(50, "Synchronising Objects");

                c = 0;
                foreach (MetaBase mBase in objects)
                {
                    if (Cancel)
                        break;

                    c++;
                    bg.ReportProgress(50, "Synchronising Objects : " + c.ToString() + " / " + objects.Count);

                    ActionResult r = new ActionResult();
                    r.Message = mBase.ToString();
                    r.Item = mBase.ToString();
                    r.Repository = Core.Variables.Instance.ClientProvider;

                    r.FromState = "N/A";

                    //save object
                    r = SyncObject(mBase, r);

                    log.AddMessage(r);
                }

                #endregion

                if (Cancel)
                    return;

                log.AddMessage("Associations");
                #region Associations
                c = 0;
                bg.ReportProgress(70, "Synchronising Associations");
                TList<ObjectAssociation> done = new TList<ObjectAssociation>();
                foreach (ObjectAssociation oAss in associations)
                {
                    if (Cancel)
                        break;

                    c++;
                    bg.ReportProgress(70, "Synchronising Associations : " + c + " / " + associations.Count);

                    if (done.Contains(oAss))
                        continue;

                    ActionResult r = new ActionResult();
                    r.Message = oAss.ToString();
                    r.Item = oAss.ToString();
                    r.Repository = Core.Variables.Instance.ClientProvider;

                    r.FromState = "N/A";

                    r = SyncAssociation(oAss, r);

                    r.TargetState = "CheckedOutRead";// mBase.State.ToString();

                    if (r.Success == false || r.intermediate == true)
                    {
                        log.AddMessage(r);
                    }

                    done.Add(oAss);
                }

                #endregion

                #region FileInformation
                c = 0;
                log.AddMessage("File Associations");
                foreach (GraphFileAssociation fileAss in fileAssociations)
                {
                    if (Cancel)
                        break;

                    c++;
                    bg.ReportProgress(80, "Synchronising File Associations : " + c + " / " + fileAssociations.Count);
                    ActionResult r = new ActionResult();
                    r.Message = fileAss.ToString();
                    r.Item = fileAss.ToString();
                    r.Repository = Core.Variables.Instance.ClientProvider;

                    r.FromState = "N/A";

                    r.TargetState = "N/A";
                    try
                    {
                        if (DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileAssociationProvider.GetByGraphFileIDGraphFileMachineChildObjectMachineCAidObjectIDChildObjectIDObjectMachine(fileAss.GraphFileID, fileAss.GraphFileMachine, fileAss.ChildObjectMachine, fileAss.CAid, fileAss.ObjectID, fileAss.ChildObjectID, fileAss.ObjectMachine) == null)
                        {
                            fileAss.EntityState = EntityState.Added;
                            DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileAssociationProvider.Save(fileAss);
                            r.Success = true;
                        }
                        else
                        {
                            r.Success = true;
                            r.intermediate = true;
                            r.Message += " Association already exists";
                        }
                    }
                    catch
                    {
                        r.Success = false;
                        log.AddMessage(r);
                    }
                }
                c = 0;
                log.AddMessage("File Objects");
                foreach (GraphFileObject fileObj in fileObjects)
                {
                    if (Cancel)
                        break;

                    c++;
                    bg.ReportProgress(90, "Synchronising File Objects : " + c + " / " + fileObjects.Count);
                    ActionResult r = new ActionResult();
                    r.Message = fileObj.ToString();
                    r.Item = fileObj.ToString();
                    r.Repository = Core.Variables.Instance.ClientProvider;

                    r.FromState = "N/A";

                    r.TargetState = "N/A";// mBase.State.ToString();
                    try
                    {
                        if (DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileObjectProvider.GetByGraphFileIDMetaObjectIDMachineIDGraphFileMachine(fileObj.GraphFileID, fileObj.MetaObjectID, fileObj.MachineID, fileObj.GraphFileMachine) == null)
                        {
                            fileObj.EntityState = EntityState.Added;
                            DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileObjectProvider.Save(fileObj);
                            r.Success = true;
                        }
                        else
                        {
                            r.Success = true;
                            r.intermediate = true;
                            r.Message += " File Object already exists";
                        }
                    }
                    catch
                    {
                        r.Success = false;
                        log.AddMessage(r);
                    }
                }
                #endregion

                bg.ReportProgress(0, "Complete");
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog(ex.ToString());
                bg.ReportProgress(0, "Errors occurred and have been logged");
            }
        }

        #region Data

        private ActionResult SyncAssociation(ObjectAssociation oAss, ActionResult res)
        {
            try
            {
                //what if other side object did not get retrieved?
                if (DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(oAss.CAid, oAss.ObjectID, oAss.ChildObjectID, oAss.ObjectMachine, oAss.ChildObjectMachine) == null)
                {
                    oAss.VCMachineID = Core.strings.GetVCIdentifier();
                    oAss.State = VCStatusList.CheckedOutRead;
                    oAss.VCStatusID = (int)VCStatusList.CheckedOutRead;
                    oAss.VCUser = Core.strings.GetVCIdentifier();
                    oAss.EntityState = EntityState.Added;
                    DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Save(oAss);
                    res.Success = true;
                }
                else
                {
                    res.Success = true;
                    res.intermediate = true;
                    res.Message += " Association already exists";
                }
            }
            catch
            {
                res.Success = false;
            }
            return res;
        }
        private ActionResult SyncObject(MetaBase mBase, ActionResult res)
        {
            try
            {
                MetaObject moDb = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetBypkidMachine(mBase.pkid, mBase.MachineName);
                if (moDb == null)
                {
                    mBase.State = VCStatusList.CheckedOutRead;
                    mBase.SaveToRepository(Guid.NewGuid(), Core.Variables.Instance.ClientProvider);
                    res.TargetState = mBase.State.ToString();
                    //update vcuser and state
                    DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ExecuteNonQuery(CommandType.Text, "UPDATE Metaobject SET VCStatusID=5,VCMachineID='" + Core.strings.GetVCIdentifier() + "' WHERE pkid = " + mBase.pkid + " AND Machine = '" + mBase.MachineName + "'");
                }
                else
                {
                    MetaBase mbaseDb = Loader.GetFromProvider(moDb.pkid, moDb.Machine, moDb.Class, false);
                    res.intermediate = true;
                    res.Message += " Object already exists";
                    res.FromState = mbaseDb.State.ToString();
                    res.TargetState = mbaseDb.State.ToString();
                }
                res.Success = true;
            }
            catch
            {
                res.Success = false;
            }
            return res;
        }
        TList<GraphFile> files = null;
        TList<GraphFileAssociation> fileAssociations = null;
        TList<GraphFileObject> fileObjects = null;
        private ActionResult SyncFile(GraphFile file, ActionResult res)
        {
            if (fileAssociations == null)
                fileAssociations = new TList<GraphFileAssociation>();
            if (fileObjects == null)
                fileObjects = new TList<GraphFileObject>();
            if (files == null)
                files = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileProvider.GetAll();
            try
            {
                int pkid = file.pkid;
                string machine = file.Machine;

                GraphFile dbFile = files.FindAll(GraphFileColumn.IsActive, true).Find(GraphFileColumn.OriginalFileUniqueID, file.OriginalFileUniqueID);
                MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter ad = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();
                if (dbFile == null)
                {
                    file.State = VCStatusList.CheckedOutRead;
                    file.VCStatusID = (int)VCStatusList.CheckedOutRead;

                    GraphFileKey fileKey = ad.InsertFile(file, file.WorkspaceTypeId, file.WorkspaceName, Core.Variables.Instance.ClientProvider);
                    //save all association from server to client
                    DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ExecuteNonQuery(CommandType.Text, "UPDATE GraphFile SET VCStatusID=5,VCMachineID= '" + Core.strings.GetVCIdentifier() + "' WHERE pkid = " + fileKey.pkid + " AND Machine = '" + fileKey.Machine + "'");
                    foreach (GraphFileAssociation gfAss in DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.GraphFileAssociationProvider.GetByGraphFileIDGraphFileMachine(pkid, machine))
                    {
                        gfAss.GraphFileID = fileKey.pkid;
                        gfAss.GraphFileMachine = fileKey.Machine;
                        fileAssociations.Add(gfAss);
                    }
                    foreach (GraphFileObject gfO in DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.GraphFileObjectProvider.GetByGraphFileIDGraphFileMachine(pkid, machine))
                    {
                        gfO.GraphFileID = fileKey.pkid;
                        gfO.GraphFileMachine = fileKey.Machine;
                        fileObjects.Add(gfO);
                    }
                    res.Message += " Created file";
                    res.TargetState = file.State.ToString();
                }
                else
                {
                    if (dbFile.State == VCStatusList.CheckedOut && file.State == VCStatusList.CheckedOut && file.VCUser == dbFile.VCUser)
                    {
                        res.TargetState = dbFile.State.ToString();
                        res.Message += " Skipped because client version is Checked Out";
                        res.intermediate = true;
                    }
                    else if (decimal.Parse(dbFile.MajorVersion + "." + dbFile.MinorVersion) >= decimal.Parse(file.MajorVersion + "." + file.MinorVersion))
                    {
                        res.TargetState = dbFile.State.ToString();
                        //client has newer file?
                        res.Message += " Skipped because client version is newer";
                        res.intermediate = true;
                    }
                    else
                    {
                        file.State = VCStatusList.CheckedOutRead;
                        file.VCStatusID = (int)VCStatusList.CheckedOutRead;

                        ad.MarkPreviousVersionsInactive(file, Core.Variables.Instance.ClientProvider);
                        GraphFileKey fileKey = ad.InsertFile(file, file.WorkspaceTypeId, file.WorkspaceName, Core.Variables.Instance.ClientProvider);
                        DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ExecuteNonQuery(CommandType.Text, "UPDATE GraphFile SET VCStatusID=5,VCMachineID= '" + Core.strings.GetVCIdentifier() + "' WHERE pkid = " + fileKey.pkid + " AND Machine = '" + fileKey.Machine + "'");
                        foreach (GraphFileAssociation gfAss in DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.GraphFileAssociationProvider.GetByGraphFileIDGraphFileMachine(pkid, machine))
                        {
                            gfAss.GraphFileID = fileKey.pkid;
                            gfAss.GraphFileMachine = fileKey.Machine;
                            fileAssociations.Add(gfAss);
                        }
                        foreach (GraphFileObject gfO in DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.GraphFileObjectProvider.GetByGraphFileIDGraphFileMachine(pkid, machine))
                        {
                            gfO.GraphFileID = fileKey.pkid;
                            gfO.GraphFileMachine = fileKey.Machine;
                            fileObjects.Add(gfO);
                        }

                        res.TargetState = file.State.ToString();
                        res.Message += " Previous marked inactive";
                        res.intermediate = true;
                    }
                }
                res.Success = true;
            }
            catch
            {
                res.Success = false;
            }
            return res;
        }

        #endregion
    }
}