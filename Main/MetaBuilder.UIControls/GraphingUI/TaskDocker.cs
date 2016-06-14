using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Docking;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Controllers;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Meta;
using MetaBuilder.MetaControls;
using MetaBuilder.MetaControls.Tasks;
using Northwoods.Go;
using b = MetaBuilder.BusinessLogic;
using BrightIdeasSoftware;

namespace MetaBuilder.UIControls.GraphingUI
{
    public partial class TaskDocker : DockContent
    {

        #region Fields (2)

        //object previousSender;
        private Dictionary<object, Dictionary<object, List<TaskBase>>> taskLists;

        #endregion Fields

        #region Constructors (1)

        public TaskDocker()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties (1)

        // never reference this directly!
        [Browsable(false)]
        public Dictionary<object, Dictionary<object, List<TaskBase>>> TaskLists
        {
            get
            {
                if (taskLists == null)
                    taskLists = new Dictionary<object, Dictionary<object, List<TaskBase>>>();

                return taskLists;
            }
            set { taskLists = value; }
        }

        #endregion Properties

        #region Methods (12)

        // Public Methods (5) 

        public void AddTask(string sender, TaskBase taskItem)
        {
            //// Console.WriteLine("AddTask: " + taskItem.Tag.ToString());
            try
            {
                Dictionary<object, List<TaskBase>> tlist;
                if (TaskLists.ContainsKey(sender))
                {
                    tlist = TaskLists[sender];
                }
                else
                {
                    tlist = new Dictionary<object, List<TaskBase>>();
                    TaskLists.Add(sender, tlist);
                }

                if (!tlist.ContainsKey(taskItem.Tag))
                {
                    tlist.Add(taskItem.Tag, new List<TaskBase>());
                    tlist[taskItem.Tag].Add(taskItem);
                }

                //BindToList(sender);
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog("TaskDocker::AddTask::" + ex.ToString());
            }
        }

        public void BindToList(string sender)
        {
            try
            {
                if (sender == null)
                {
                    ClearTasks();
                    SetText("");
                    return;
                }
                if (TaskLists.ContainsKey(sender))
                {
                    Dictionary<object, List<TaskBase>> ActiveList = TaskLists[sender];

                    List<TaskBase> activeTasks = new List<TaskBase>();
                    foreach (KeyValuePair<object, List<TaskBase>> t in ActiveList)
                    {
                        activeTasks.AddRange(t.Value);
                    }

                    listView1.SetObjects(activeTasks);
                }
                else
                {
                    ClearTasks();
                }

                SetText("Select a task above to view the associated shape");
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog("TaskDocker::BindToList::" + ex.ToString());
            }
            return;
            //try
            //{
            //    ClearTasks();
            //    List<TaskBase> itemsToAdd = new List<TaskBase>();
            //    if (TaskLists.ContainsKey(sender))
            //    {
            //        Dictionary<object, List<TaskBase>> ActiveList = TaskLists[sender];
            //        foreach (KeyValuePair<object, List<TaskBase>> kvp in ActiveList)
            //        {
            //            foreach (TaskBase taskBase in kvp.Value)
            //            {
            //                itemsToAdd.Add(taskBase);
            //            }
            //        }
            //    }
            //    foreach (TaskBase tbase in itemsToAdd)
            //    {
            //        AddListViewItem(tbase);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Core.Log.WriteLog("TaskDocker.BindToList - " + ex.ToString());
            //}
        }

        public void RemoveMissingGoObjects(string sender)
        {
            if (sender == null)
            {
                ClearTasks();
                return;
            }
            try
            {
                if (TaskLists.ContainsKey(sender))
                {
                    Dictionary<object, List<TaskBase>> ActiveList = TaskLists[sender];
                    List<MetaBase> removeItems = new List<MetaBase>();
                    foreach (KeyValuePair<object, List<TaskBase>> kvp in ActiveList)
                    {
                        foreach (TaskBase task in kvp.Value)
                        {
                            if (task is DiagrammingTask)
                            {
                                if (DockingForm.DockForm.GetCurrentGraphViewContainer() != null)
                                    if (!(DockingForm.DockForm.GetCurrentGraphViewContainer().View.Document.Contains((task as DiagrammingTask).MyGoObject as GoObject)))
                                    {
                                        removeItems.Add(kvp.Key as MetaBase);
                                    }
                                continue; //continue because an item(MyGoObject) can have multiple tasks but can only be on one diagram
                            }
                        }
                    }
                    foreach (MetaBase o in removeItems)
                    {
                        if (o != null)
                            ActiveList.Remove(o);
                    }
                }
                BindToList(sender);
            }
            catch
            {

            }
        }

        //private void RemoveTask(object o, TaskBase taskItem)
        //{
        //    if (this.TaskLists.ContainsKey(o))
        //    {
        //        object objectToRemove = null;
        //        foreach (KeyValuePair<object, List<TaskBase>> kvp in TaskLists[o])
        //        {
        //            for (int i = 0; i < kvp.Value.Count; i++)
        //            {
        //                if (kvp.Value[i] == taskItem)
        //                {
        //                    kvp.Value.Remove(taskItem);
        //                }
        //            }
        //        }
        //    }
        //}

        public void RemoveTaskList(string key)
        {
            if (TaskLists.ContainsKey(key))
                TaskLists.Remove(key);
            ClearTasks();
        }

        public void RemoveTaskWhereTagEquals(string sender, object o, Type typeOfTaskToRemove)
        {
            if (TaskLists.ContainsKey(sender))
            {
                Dictionary<object, List<TaskBase>> taskList = TaskLists[sender];
                foreach (KeyValuePair<object, List<TaskBase>> kvp in taskList)
                {
                    for (int i = 0; i < kvp.Value.Count; i++)
                    {
                        if (kvp.Value[i].GetType() == typeOfTaskToRemove)
                        {
                            TaskBase tbase = kvp.Value[i];
                            if (tbase.Tag == o)
                            {
                                //ListViewItem itemToRemove = null;
                                //foreach (ListViewItem item in listView1.Items)
                                //{
                                //    if (item.Tag == tbase)
                                //    {
                                //        itemToRemove = item;
                                //        break;
                                //    }
                                //}
                                //for (int j = 0; j < listView1.Items.Count; j++)
                                //{
                                //    if (listView1.Items[j].Tag == tbase)
                                //    {
                                //        listView1.Items.RemoveAt(j);
                                //    }
                                //}
                                //if (itemToRemove != null)
                                //{
                                //    listView1.Items.Remove(itemToRemove);
                                //}
                                kvp.Value.RemoveAt(i);
                            }
                        }
                    }
                }
                //BindToList(sender);
            }
        }

        // Private Methods (7) 

        private void AddListViewItem(TaskBase taskItem)
        {
            //if (listView1.Items.ContainsKey(taskItem.Guid.ToString()))
            //    return;

            //OLVListItem lvi = new OLVListItem();
            //lvi.Name = taskItem.Guid.ToString(); ;
            //lvi.Text = taskItem.Caption;
            //lvi.Tag = taskItem;
            //if (!listView1.Items.Contains(lvi))
            //    this.listView1.Items.Add(lvi);

            //lvi.Selected = true;
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point ClickPoint = new Point(e.X, e.Y);
                OLVListItem lvi = listView1.OlvHitTest(ClickPoint.X, ClickPoint.Y).Item;
                if (lvi != null)
                {
                    Type t = lvi.RowObject.GetType();
                    if (t == typeof(DuplicationTask))
                    {
                        List<DuplicateObjectSpec> specs = new List<DuplicateObjectSpec>();
                        DuplicationTask task = lvi.RowObject as DuplicationTask;

                        DuplicateObjectSpec spec = new DuplicateObjectSpec();
                        spec.Matches = task.Matches;
                        MetaBase mbase = task.Tag as MetaBase;
                        if ((!spec.Matches.Contains(mbase)))
                            spec.Matches.Add(mbase);
                        spec.Task = task;
                        specs.Add(spec);
                        //foreach (ListViewItem lviOther in this.listView1.Items)
                        //{
                        //    if (lviOther.Tag.GetType() == typeof(DuplicationTask) && lviOther != lvi)
                        //    {
                        //        DuplicationTask taskOther = lviOther.Tag as DuplicationTask;
                        //        DuplicateObjectSpec otherspec = new DuplicateObjectSpec();
                        //        otherspec.Matches = taskOther.Matches;
                        //        if (!(taskOther.Matches.Contains(taskOther.Tag as MetaBase)))
                        //            otherspec.Matches.Add(taskOther.Tag as MetaBase);
                        //        otherspec.Task = taskOther;
                        //        specs.Add(otherspec);
                        //    }
                        //}
                        MergeObjects merger = new MergeObjects(false);
                        merger.DuplicateObjectSpecifications = specs;
                        merger.ViewInContext += new ViewInContextEventHandler(merger_ViewInContext);
                        merger.OpenDiagram += new EventHandler(merger_OpenDiagram);
                        merger.Start();
                        DialogResult res = merger.ShowDialog(this);
                        if (res == DialogResult.OK)
                        {
                            foreach (DuplicateObjectSpec specification in merger.DuplicateObjectSpecifications)
                            {
                                if (specification.Replacements.Count > 0)
                                {
                                    foreach (KeyValuePair<MetaBase, MetaBase> kvp in specification.Replacements)
                                    {
                                        MetaBase oldMB = kvp.Key;
                                        MetaBase replacedWithMB = kvp.Value;

                                        // retrieve the original task
                                        if (specification.Task.MyGoObject is IMetaNode)
                                        {
                                            // Attempt to create shallow copies
                                            GraphView view = DockingForm.DockForm.GetCurrentGraphView();
                                            bool foundOnThisDrawing = false;
                                            List<GraphNode> nodes = view.ViewController.GetNodesBoundToMetaObject(replacedWithMB);
                                            foundOnThisDrawing = nodes.Count > 0;
                                            foreach (GraphNode metanode in nodes)
                                            {
                                                //metanode.MetaObject = replacedWithMB;
                                                //metanode.HookupEvents();
                                                view.ViewController.AddShadows(metanode);
                                                replacedWithMB = metanode.MetaObject;
                                            }

                                            IMetaNode node = specification.Task.MyGoObject as IMetaNode;
                                            node.MetaObject = replacedWithMB;
                                            node.HookupEvents();
                                            node.RequiresAttention = false;
                                            if (node is GraphNode)
                                            {
                                                GraphNode graphNode = node as GraphNode;
                                                //graphNode.Position = new PointF(graphNode.Position.X, graphNode.Position.Y + 1);
                                                //graphNode.Position = new PointF(graphNode.Position.X, graphNode.Position.Y - 1);
                                                graphNode.CopyAsShadow = false;

                                                if (foundOnThisDrawing)
                                                    view.ViewController.AddShadows(graphNode);
                                            }

                                            RemoveTaskWhereTagEquals(task.ContainerID, oldMB, typeof(DuplicationTask));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (t == typeof(MissingPortTask))
                    {
                        MissingPortTask task = lvi.RowObject as MissingPortTask;
                        QLink l = task.Tag as QLink;
                        if (l.ToPort == null)
                            DockingForm.DockForm.DisplayTip("The to port is null", "Missing Port Task Information", ToolTipIcon.Info);
                        else if (l.FromPort == null)
                            DockingForm.DockForm.DisplayTip("The from port is null", "Missing Port Task Information", ToolTipIcon.Info);
                        else if (!(l.ToPort is QuickPort))
                            DockingForm.DockForm.DisplayTip("The to port is invalid", "Missing Port Task Information", ToolTipIcon.Info);
                        else if (!(l.FromPort is QuickPort))
                            DockingForm.DockForm.DisplayTip("The from port is invalid", "Missing Port Task Information", ToolTipIcon.Info);
                        else
                            DockingForm.DockForm.DisplayTip("The tasks problem is complex", "Task Information", ToolTipIcon.Info);
                    }
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                TaskBase titem = listView1.SelectedItem.RowObject as TaskBase;
                SetText(titem.Description);

                if (titem is DiagrammingTask)
                {
                    DiagrammingTask item = titem as DiagrammingTask;
                    if (item.MyGoObject != null)
                    {
                        if (item.MyView.Selection.Primary != item.MyGoObject)
                        {
                            GraphViewContainer container = item.MyView.Parent as GraphViewContainer;
                            container.DisableMetaPropertiesOnSelection = true;
                            item.MyView.Selection.Clear();
                            try
                            {
                                item.MyView.Selection.Add(item.MyGoObject as GoObject);
                                item.MyView.ScrollRectangleToVisible((item.MyGoObject as GoObject).Bounds);
                            }
                            catch (Exception ex)
                            {
                                //listView1.Items.Remove(listView1.SelectedItems[0]);
                                RemoveTaskWhereTagEquals(container.ContainerID.ToString(), titem.Tag, titem.GetType());
                                Core.Log.WriteLog("TaskDocker listView1_SelectedIndexChanged " + ex.ToString());
                            }
                            container.DisableMetaPropertiesOnSelection = false;
                        }
                    }
                    else
                    {
                        if (item.MyView.Selection.Primary != item.Tag as GoObject)
                        {
                            GraphViewContainer container = item.MyView.Parent as GraphViewContainer;
                            container.DisableMetaPropertiesOnSelection = true;
                            item.MyView.Selection.Clear();
                            try
                            {
                                item.MyView.Selection.Add(item.Tag as GoObject);
                                item.MyView.ScrollRectangleToVisible((item.Tag as GoObject).Bounds);
                            }
                            catch (Exception ex)
                            {
                                //listView1.Items.Remove(listView1.SelectedItems[0]);
                                RemoveTaskWhereTagEquals(container.ContainerID.ToString(), titem.Tag, titem.GetType());
                                Core.Log.WriteLog("TaskDocker listView1_SelectedIndexChanged " + ex.ToString());
                            }
                            container.DisableMetaPropertiesOnSelection = false;
                        }
                    }
                }
            }
        }

        private void merger_OpenDiagram(object sender, EventArgs e)
        {
            GraphFileKey key = sender as GraphFileKey;
            GraphFile file = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileProvider.Get(key);
            DockingForm.DockForm.OpenGraphFileFromDatabase(file, false, false);
        }

        private void merger_ViewInContext(MetaBase mbase)
        {
            LiteGraphViewContainer newContainer = new LiteGraphViewContainer();
            newContainer.UseServer = false;
            newContainer.Setup(mbase);
            //if (DockingForm.DockForm.GetCurrentGraphViewContainer() != null)
            //    newContainer.Show(DockingForm.DockForm.GetCurrentGraphViewContainer().DockHandler.PanelPane, DockAlignment.Bottom, 0.25);
            //else
            newContainer.Show(DockingForm.DockForm.dockPanel1, DockState.Document);
        }

        private void TaskDocker_Load_1(object sender, EventArgs e)
        {
            listView1.MultiSelect = false;
        }

        public void ClearTasks()
        {
            //listView1.Clear();
            if (DockingForm.DockForm.Disposing)
                return;
            listView1.SetObjects(null);
            SetText("Select a task above to view the associated shape");
        }

        public void SetText(string text)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(SetText), new object[] { text });
            }
            else
            {
                textBox1.Text = text;
            }
        }

        public void RemoveContainerList(string key)
        {
            TaskLists.Remove(key);
        }

        #endregion Methods

    }
}