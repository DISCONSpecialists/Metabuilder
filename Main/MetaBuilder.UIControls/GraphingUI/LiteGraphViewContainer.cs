using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using MetaBuilder.BusinessFacade.Exports;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Docking;
using System.Collections.Generic;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Tools;
using MetaBuilder.Meta;
using MetaBuilder.UIControls.GraphingUI.CustomPrinting;
using Northwoods.Go;
using MetaBuilder.MetaControls;
using MetaBuilder.Graphing.Controllers;
using System.Xml;
using MetaBuilder.Graphing.Containers;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace MetaBuilder.UIControls.GraphingUI
{
    public partial class LiteGraphViewContainer : DockContent
    {

        Panel filterPanel = new Panel();

        #region Constructors (1)

        public bool UseServer;
        private string Provider { get { return UseServer ? Core.Variables.Instance.ServerProvider : Core.Variables.Instance.ClientProvider; } }
        public LiteGraphViewContainer()
        {
            InitializeComponent();
            this.contextViewer1.BringToFront();

            StripButtonFilter.Visible = true;
            StripButtonFilter.Checked = false;

            //ConvertContextToDocument.Visible = Core.Variables.Instance.IsDeveloperEdition;
        }

        #endregion Constructors

        #region FILTER

        ContextFilter filter;
        public void SetupFilterPanel()
        {
            filterPanel.AutoSize = false;
            filterPanel.Dock = DockStyle.Top;
            filterPanel.Visible = false;
            filterPanel.Height = 165;
            Controls.Add(filterPanel);

            filter = new ContextFilter(this.MyView.Document, true);
            filter.AutoSize = false;
            filter.Dock = DockStyle.Fill;
            filterPanel.Controls.Add(filter);

            filter.Applied += new System.EventHandler(filterApplied);
        }
        private void filterApplied(object sender, EventArgs e)
        {
            if (filter == null)
                return;

            if (filter.workspacesAllowed.Count == 0)
            {
                foreach (GoObject o in MyView.Document)
                    if (o is ContextNode)
                        o.Visible = true;
            }
            else
            {
                foreach (GoObject o in MyView.Document)
                    if (o is ContextNode)
                        o.Visible = false;
            }

            foreach (string key in filter.workspacesAllowed)
            {
                //GoText legendText = e.GoObject as GoText;
                //string key = (e.GoObject as GoText).Text;
                ////visibility all corresponding nodes
                //if (legendText.TextColor == Color.Gray)
                //{
                setAllNodes(key, true);
                //legendText.TextColor = Color.Black;
                //}
                //else
                //{
                //    setAllNodes(key, false);
                //    legendText.TextColor = Color.Gray;
                //}
            }

        }

        #endregion

        void MyView_DocumentChanged(object sender, Northwoods.Go.GoChangedEventArgs e)
        {
            if (e.Hint == GoLayer.ChangedObject || e.Hint == GoObject.ChangedBounds || e.Hint == GoLayer.RemovedObject || e.Hint == GoLayer.InsertedObject || e.Hint == GoObject.ChangedBounds)
            {
                if (!IsBusySettingUp)
                    ResizeDocument();
            }
            ConvertContextToDocument.Visible = true;
        }

        #region Properties (1)

        public ContextViewer MyView
        {
            get { return this.contextViewer1; }
            set { contextViewer1 = value; }
        }

        #endregion Properties

        #region Methods (11)

        bool IsBusySettingUp;
        // Public Methods (1) 

        public void LoadView(string filename)
        {
            contextViewer1.LoadNewView(filename);
            openFilter(filename);
            this.Text = Core.strings.GetFileNameWithoutExtension(filename);
            TabText = Core.strings.GetFileNameWithoutExtension(filename);
            ResizeDocument();
            MyView.ResumeLayout();
        }

        public void Setup(MetaBase mbobject)
        {
            this.Tag = mbobject;
            IsBusySettingUp = true;
            contextViewer1.Setup(mbobject, true, this.UseServer);
            MyView.DragsRealtime = true;
            CenterView(contextViewer1.MainNode);

            ResizeDocument();
            IsBusySettingUp = false;

            if (UseServer)
                TabText = this.Text += " (" + Provider + ")";
        }
        public void Setup(MetaBase mbobject, bool explode)
        {
            contextViewer1.Setup(mbobject, true, this.UseServer);
            ResizeDocument();

            if (UseServer)
                TabText = this.Text += " (" + Provider + ")";
        }

        public void ResizeDocument()
        {
            IsBusySettingUp = true;
            if (MyView.Sheet != null)
            {
                RectangleF b = MyView.ComputeDocumentBounds();
                //SizeF docSize = this.MyView.DocumentSize;
                PointF pSheetPosition = new PointF();
                MyView.Sheet.Size = new SizeF(MyView.Sheet.TopLeftMargin.Width + MyView.Sheet.BottomRightMargin.Width, MyView.Sheet.TopLeftMargin.Height + MyView.Sheet.BottomRightMargin.Height);
                pSheetPosition.X = b.X - MyView.Sheet.TopLeftMargin.Width;
                pSheetPosition.Y = b.Y - MyView.Sheet.TopLeftMargin.Height;
                this.MyView.Sheet.Position = pSheetPosition;
                this.MyView.Sheet.Size = new SizeF(b.Width + MyView.Sheet.TopLeftMargin.Width + MyView.Sheet.BottomRightMargin.Width, b.Height + MyView.Sheet.TopLeftMargin.Height + MyView.Sheet.BottomRightMargin.Height);
            }
            IsBusySettingUp = false;
        }

        // Private Methods (10) 

        private void contextViewer1_BackgroundContextClicked(object sender, GoInputEventArgs e)
        {
            return;

            GoContextMenu cxMenu = new GoContextMenu(contextViewer1);
            if (cxMenu == null)
            {
                cxMenu = new GoContextMenu(contextViewer1);
            }
            foreach (string s in contextViewer1.ClassesOnDiagram)
            {
                MenuItem mItemClass = new MenuItem(s, new EventHandler(mFilter_Click));
                mItemClass.Checked = contextViewer1.ClassesShown.Contains(s);
                cxMenu.MenuItems.Add(mItemClass);
            }
            cxMenu.Show(contextViewer1, e.ViewPoint);
        }

        private void contextViewer1_BackgroundSingleClicked(object sender, GoInputEventArgs e)
        {
            UpdateHighlighting(null);
        }

        private void contextViewer1_ObjectContextClicked(object sender, GoObjectEventArgs e)
        {
            if ((e.GoObject.ParentNode is ContextNode) || (e.GoObject.ParentNode is GraphNode))
            {
                ContextNode cnode = e.GoObject.ParentNode as ContextNode;
                ContextMenu cxMenu = contextViewer1.ContextMenu;

                if (cxMenu == null)
                {
                    cxMenu = new ContextMenu();
                }
                MenuItem mItemExplode = new MenuItem("Explode", new EventHandler(mItemExplode_Click));
                //mItemExplode.Tag = e.GoObject.ParentNode;
                cxMenu.MenuItems.Add(mItemExplode);

                if (e.GoObject.ParentNode is ContextNode)
                {
                    MenuItem mItemExplodeToExcel = new MenuItem("Explode to Excel", new EventHandler(mItemExplodeToExcel_Click));
                    mItemExplodeToExcel.Tag = e.GoObject.ParentNode;
                    cxMenu.MenuItems.Add(mItemExplodeToExcel);
                }
                cxMenu.Show(contextViewer1, e.ViewPoint);
            }
        }

        private void contextViewer1_ObjectSingleClicked(object sender, GoObjectEventArgs e)
        {
            if (e.GoObject.ParentNode is IGoNode)
            {
                IGoNode cnode = e.GoObject.ParentNode as IGoNode;
                UpdateHighlighting(cnode);
                if (cnode is ContextNode)
                    DockingForm.DockForm.ShowMetaObjectProperties((cnode as ContextNode).MetaObject, true);
            }
            else
            {
                if (e.GoObject.ParentNode is GoListGroup)
                {
                    if (e.GoObject is GoText)
                    {
                        GoText legendText = e.GoObject as GoText;
                        string key = (e.GoObject as GoText).Text;
                        //visibility all corresponding nodes
                        if (legendText.TextColor == Color.Gray)
                        {
                            setAllNodes(key, true);
                            legendText.TextColor = Color.Black;
                        }
                        else
                        {
                            setAllNodes(key, false);
                            legendText.TextColor = Color.Gray;
                        }
                    }
                    else if (e.GoObject is GoRectangle)
                    {
                        GoRectangle legendRectangle = e.GoObject as GoRectangle;
                        //Show picker and change all colors
                        ColorDialog cd = new ColorDialog();
                        cd.FullOpen = true;
                        cd.SolidColorOnly = true;
                        cd.Color = (legendRectangle.Brush as SolidBrush).Color;
                        if (cd.ShowDialog(this) == DialogResult.OK)
                        {
                            //change all nodes to this color
                            setAllNodes((legendRectangle.Brush as SolidBrush).Color, cd.Color);
                            (legendRectangle.Brush as SolidBrush).Color = cd.Color;
                        }

                    }
                }

                UpdateHighlighting(null);
            }
        }

        private void setAllNodes(Color colorAsIs, Color colorToBe)
        {
            //MyView.SuspendLayout();
            //MyView.BeginUpdate();
            try
            {
                foreach (GoObject o in contextViewer1.Document)
                {
                    if (o is ContextNode)
                    {
                        if (((o as ContextNode).MySmallRectangle.Brush as SolidBrush).Color == colorAsIs)
                            ((o as ContextNode).MySmallRectangle.Brush as SolidBrush).Color = colorToBe;
                    }
                }
            }
            catch
            {
                //Some Permission Exception here
            }
            //MyView.EndUpdate();
            //MyView.ResumeLayout();
        }
        private void setAllNodes(string key, bool visible)
        {
            foreach (GoObject o in contextViewer1.Document)
            {
                if (o is ContextNode)
                {
                    ContextNode node = o as ContextNode;
                    if (node.MetaObject != null)
                    {
                        if (node.MetaObject.WorkspaceName == key.Replace(" (Server)", ""))
                        {
                            setNode(node, visible);
                        }
                    }
                    else
                    {
                        if (node.UserObject != null)
                        {
                            string k = node.UserObject.ToString();
                            if (!k.Contains("|")) continue;
                            string pkid = null;
                            string machine = null;

                            //Use the userobject to get a value for the diagram and workspace of it
                            foreach (string s in k.Split('|'))
                            {
                                if (pkid == null)
                                {
                                    pkid = s;
                                    continue;
                                }

                                machine = s;
                            }
                            MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter adapter = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();
                            GraphFile file = adapter.GetQuickFileDetails(int.Parse(pkid), machine, (Provider == Core.Variables.Instance.ServerProvider));
                            //GraphFile file = DataRepository.Connections[Provider].Provider.GraphFileProvider.GetBypkidMachine(int.Parse(pkid), machine);
                            if (file != null)
                            {
                                if (file.WorkspaceName == key.Replace(" (Server)", ""))
                                {
                                    setNode(node, visible);
                                }
                            }
                        }
                    }
                }
            }

            //(filterPanel.Controls[0] as ContextFilter).Apply();
        }
        private void setNode(ContextNode node, bool visible)
        {
            node.Visible = visible;
            foreach (GoObject link in node.DestinationLinks)
                link.Visible = visible;
            foreach (GoObject link in node.Links)
                link.Visible = visible;
        }

        private void Highlight(GoObject obj, Color c, float w)
        {
            GoShape shape = obj as GoShape;
            if (shape != null)
            {
                shape.Pen = new Pen(c, w);
                GoPort port = obj as GoPort;
                if (port != null)
                {
                    if (c == Color.Black)
                    {
                        if (!(port.Parent is GoSubGraph))
                        {
                            port.Style = GoPortStyle.None;
                        }
                    }
                    else
                    {
                        port.Style = GoPortStyle.Ellipse;
                        port.Brush = new SolidBrush(c);
                    }
                }
            }
            else
            {
                GoGroup group = obj as GoGroup;
                if (group != null)
                {
                    foreach (GoObject o in group)
                    {
                        GoShape s = o as GoShape;
                        if (s != null)
                        {
                            Highlight(s, c, w);
                            break;
                        }
                    }
                }
            }
        }

        private void HighlightAll(GoObject obj, Color c, float w)
        {
            GoGroup group = obj as GoGroup;
            if (group != null)
            {
                foreach (GoObject o in group)
                {
                    HighlightAll(o, c, w);
                }
            }
            else
            {
                Highlight(obj, c, w);
            }
        }

        private void mFilter_Click(object sender, EventArgs e)
        {
            MenuItem mItemSender = sender as MenuItem;
            string classToModifyFilter = mItemSender.Text;
            if (contextViewer1.ClassesShown.Contains(classToModifyFilter))
                contextViewer1.ClassesShown.Remove(classToModifyFilter);
            else
                contextViewer1.ClassesShown.Add(classToModifyFilter);
            contextViewer1.ApplyFilter();
        }

        private void mItemExplode_Click(object sender, EventArgs e)
        {
            //MenuItem menuItem = sender as MenuItem;
            //IGoNode node = menuItem.Tag as IGoNode;
            UpdateHighlighting(null);

            Collection<ContextNode> nodesToExplode = new Collection<ContextNode>();
            foreach (GoObject o in MyView.Selection)
            {
                ContextNode imnode = o as ContextNode;
                if (imnode == null)
                    continue;
                nodesToExplode.Add(imnode);
            }
            foreach (ContextNode imnode in nodesToExplode)
            {
                //if (node != null)
                //{
                if (imnode.MetaObject == null && MyView.Selection.Count == 1) //dont explode diagrams when multiple objects are selected
                {
                    string[] userflags = imnode.UserObject.ToString().Split('|');
                    int DiagramID = int.Parse(userflags[0]);
                    string mach = userflags[1];

                    MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter adapter = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();
                    GraphFile gfile = adapter.GetQuickFileDetails(DiagramID, mach, (Provider == Core.Variables.Instance.ServerProvider));
                    //GraphFile gfile = DataRepository.Connections[Provider].Provider.GraphFileProvider.GetBypkidMachine(DiagramID, mach);
                    DockingForm.DockForm.OpenGraphFileFromDatabase(gfile, false, UseServer);
                }
                else
                {
                    contextViewer1.Setup(imnode.MetaObject, false, this.UseServer);
                }
                //}
                //UpdateHighlighting(o as IGoNode);
            }
            ResizeDocument();

            if (filterPanel != null && filterPanel.Controls.Count > 0 && filterPanel.Controls[0] is ContextFilter)
                (filterPanel.Controls[0] as ContextFilter).Apply();
        }

        private void mItemExplodeToExcel_Click(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            IGoNode node = menuItem.Tag as IGoNode;
            ContextNode imnode = menuItem.Tag as ContextNode;
            if (imnode != null)
            {
                if (imnode.MetaObject != null)
                {
                    ExportContext contextExporter = new ExportContext();
                    contextExporter.MetaBaseMain = imnode.MetaObject;
                    contextExporter.Export();
                    MessageBox.Show(this, "Export can be found at:" + Environment.NewLine + contextExporter.FileName);
                }
            }
        }

        private void UpdateHighlighting(IGoNode cnode)
        {
            if (cnode == null)
            {
                // remove highlights from all objects
                foreach (GoObject obj in contextViewer1.Document)
                {
                    HighlightAll(obj, Color.Black, 1);
                }
            }
            if (cnode != null)
            {
                // remove highlights from all objects
                foreach (GoObject obj in contextViewer1.Document)
                {
                    HighlightAll(obj, Color.Black, 1);
                }
                if (cnode is IGoNode)
                {
                    IGoNode n = (IGoNode)cnode;
                    IEnumerable e = null;
                    e = n.Sources;
                    e = n.Destinations;
                    e = n.Nodes;
                    e = n.SourceLinks;
                    e = n.DestinationLinks;
                    e = n.Links;
                    if (e != null)
                    {
                        foreach (GoObject o in e)
                        {
                            Highlight(o, Color.Red, 2);
                        }
                    }
                }
            }
        }

        #endregion Methods

        private void CenterView(GoNode n)
        {
            /*this.MyView.SheetStyle = GoViewSheetStyle.Sheet;
            RectangleF b = this.MyView.ComputeDocumentBounds();
            PointF c = new PointF(b.X + b.Width / 2, b.Y + b.Height / 2);
            float s = this.MyView.DocScale;
            if (b.Width > 0 && b.Height > 0)
                this.MyView.RescaleWithCenter(s, c);*/
            //MyView.RescaleWithCenter(MyView.DocScale, n.Center);//MyView.DocExtentCenter);
        }
        private void myButton_Click(object sender, System.EventArgs e)
        {
            DoPrint();
        }

        private void StripButtonSave1_Click(object sender, EventArgs e)
        {
            saveFilter(contextViewer1.SaveThisView());
        }

        private void saveFilter(string filename)
        {
            if (filename.Length == 0)
                return;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filename);
                if (doc != null)
                {
                    foreach (ContextFilterItem filterItem in filter.Filters)
                    {
                        XmlElement node = doc.CreateElement("ContextFilter");
                        node.SetAttribute("type", filterItem.FType.ToString());
                        node.SetAttribute("value", filterItem.Value);
                        node.SetAttribute("fieldValue", filterItem.FieldValue);
                        node.InnerText = filterItem.ToString();

                        if (filterItem.Items != null)
                        {
                            foreach (ContextFilterItem childFilterItem in filterItem.Items)
                            {
                                XmlElement childNode = doc.CreateElement("ContextFilterChild");
                                childNode.SetAttribute("type", childFilterItem.FType.ToString());
                                childNode.SetAttribute("value", childFilterItem.Value);
                                childNode.SetAttribute("fieldValue", childFilterItem.FieldValue);
                                childNode.InnerText = childFilterItem.ToString();

                                node.AppendChild(childNode);
                            }
                        }
                        doc.DocumentElement.AppendChild(node);
                    }
                    doc.Save(filename);
                }
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog("LiteGraphViewContainer::saveFilter::" + ex.ToString());
            }
        }
        private void openFilter(string filename)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filename);
                if (doc != null)
                {
                    filter.ListViewParents.Clear();
                    XmlNodeList contextNodes = doc.GetElementsByTagName("ContextFilter");
                    foreach (XmlNode node in contextNodes) //parent
                    {
                        string type = node.Attributes["type"].Value.ToString();
                        string val = node.Attributes["value"].Value.ToString();
                        string fieldVal = node.Attributes["fieldValue"].Value.ToString();

                        ContextFilterItem item = new ContextFilterItem((FilterType)Enum.Parse(typeof(FilterType), type, false), val);
                        item.FieldValue = fieldVal;
                        ListViewItem i = new ListViewItem();
                        i.Tag = item;
                        i.Text = item.ToString();

                        foreach (XmlNode childNode in node.ChildNodes) //children
                        {
                            if (childNode.Name != "ContextFilterChild")
                                continue;
                            string childType = childNode.Attributes["type"].Value.ToString();
                            string childVal = childNode.Attributes["value"].Value.ToString();
                            string childFieldVal = childNode.Attributes["fieldValue"].Value.ToString();

                            ContextFilterItem childItem = new ContextFilterItem((FilterType)Enum.Parse(typeof(FilterType), childType, false), childVal);
                            childItem.FieldValue = childFieldVal;
                            item.Items.Add(childItem);
                        }

                        filter.ListViewParents.Items.Add(i);
                    }
                    filter.ListViewParents.SelectedItems.Clear();
                    filter.Apply();
                }
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog("LiteGraphViewContainer::openFilter::" + ex.ToString());
            }
        }

        private void StripButtonPrint_Click(object sender, EventArgs e)
        {
            DoPrint();
        }

        private void StripButtonWorkspaceLegend_Click(object sender, EventArgs e)
        {
            if (contextViewer1.legend != null)
                contextViewer1.legend.Visible = this.StripButtonWorkspaceLegend.Checked;

            //make all those small rectangles invisible as well
            foreach (GoObject o in contextViewer1.Document)
            {
                if (!(o is ContextNode))
                    continue;

                ContextNode node = o as ContextNode;
                node.MySmallRectangle.Visible = this.StripButtonWorkspaceLegend.Checked;

                if (StripButtonWorkspaceLegend.Checked == false)
                    setNode(node, true);
            }
        }

        private void StripButtonFilter_Click(object sender, EventArgs e)
        {
            if (filter == null)
                SetupFilterPanel();
            filterPanel.Visible = StripButtonFilter.Checked;
            filterPanel.BringToFront();

            //if (!StripButtonFilter.Checked)
            //{
            //    foreach (GoObject o in this.MyView.Document)
            //        o.Visible = true;

            //    MyView.ApplyFilter();
            //}
            //else
            //{
            //    (filterPanel.Controls[0] as ContextFilter).Apply();
            //}
        }

        private void DoPrint()
        {
            float originalScale = this.contextViewer1.DocScale;
            MyPrintDialog myDialog = new MyPrintDialog(false);
            myDialog.Document = this.contextViewer1.Document;

            if (myDialog.DialogResult != DialogResult.Cancel)
            {
                //myDialog.myView.Document.TopLeft = myView.DocumentTopLeft;
                /* myDialog.MyView.DocScale = myView.DocScale;
                 myDialog.Sheet.Size = myView.Sheet.Size;
                 myDialog.Sheet.Position = myView.Sheet.Position;*/
                //myDialog.SheetChanged += new EventHandler(myDialog_SheetChanged);

                myDialog.ShowDialog(this);
                this.contextViewer1.DocScale = originalScale;
            }
        }
        private void StripButtonOpen_Click(object sender, EventArgs e)
        {
            // contextViewer1.LoadNewView();
        }

        #region ConvertDocument
        private BackgroundWorker convertWorker;
        private GraphViewContainer convertContainer = null;
        private void ConvertContextToDocument_Click(object sender, System.EventArgs e)
        {
            if (convertContainer != null)
                return;
            ConvertContextToDocument.Enabled = false;
            DockingForm.DockForm.DisplayTip("Converting this view to a normal diagram." + Environment.NewLine + "This is happening in a thread and may take some time.", "Convert to diagram");

            convertWorker = new BackgroundWorker();
            convertContainer = new GraphViewContainer(FileTypeList.Diagram); //DockingForm.DockForm.NewDiagram();//

            convertWorker.RunWorkerCompleted -= convertWorker_RunWorkerCompleted;
            convertWorker.DoWork -= convertWorker_DoWork;
            convertWorker.ProgressChanged -= convertWorker_ProgressChanged;

            convertWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(convertWorker_RunWorkerCompleted);
            convertWorker.DoWork += new DoWorkEventHandler(convertWorker_DoWork);
            convertWorker.ProgressChanged += new ProgressChangedEventHandler(convertWorker_ProgressChanged);

            convertWorker.RunWorkerAsync();
        }
        private void convertWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }
        private void convertWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ContextViewToGraphView(contextViewer1, convertContainer);
        }
        public void ContextViewToGraphView(ContextViewer cV, GraphViewContainer container)
        {
            Dictionary<MetaBase, GraphNode> baseAndNode = new Dictionary<MetaBase, GraphNode>();
            DockingForm.DockForm.UpdateTotal(100);
            DockingForm.DockForm.UpdateStatusLabel("Adding Nodes");
            DockingForm.DockForm.ProgressUpdate(25);
            foreach (GoObject o in cV.Document)
            {
                if (o is ContextNode && o.Visible)
                {
                    ContextNode cNode = o as ContextNode;
                    if (cNode.MetaObject == null || cNode.IsArtifact)
                        continue;

                    if (cNode.MetaObject.Class == "Rationale")
                    {
                        continue;
                    }

                    try
                    {
                        GoObject shape = (Core.Variables.Instance.ReturnShape(cNode.MetaObject.Class) as GoObject);
                        if (shape != null)
                        {
                            GraphNode gNode = shape.Copy() as GraphNode;
                            if (gNode != null)
                            {
                                //As Shallow
                                gNode.MetaObject = cNode.MetaObject;
                                gNode.HookupEvents();
                                gNode.BindToMetaObjectProperties();
                                gNode.Shadowed = true;
                                gNode.Location = o.Location;
                                //Add
                                baseAndNode.Add(gNode.MetaObject, gNode);
                                convertContainer.View.Document.Add(gNode);
                                //if (gNode is CollapsibleNode)
                                //{
                                //    //refresh?
                                //}
                                shape = null;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Core.Log.WriteLog("LiteGraphViewContainer::ContextViewToGraphView" + Environment.NewLine + ex.ToString());
                    }
                }
            }

            //links
            foreach (GoObject o in cV.Document)
            {
                //links
                if (o is QLink)
                {
                    //diagram nodes as allocation
                    if (((o as QLink).FromNode as ContextNode).MetaObject == null)
                    {
                        DockingForm.DockForm.UpdateStatusLabel("Diagram");
                        DockingForm.DockForm.ProgressUpdate(50);
                        if (((o as QLink).ToNode as ContextNode).MetaObject != null)
                        {
                            //fromnode is diagram node
                            GraphNode to = null;
                            baseAndNode.TryGetValue(((o as QLink).ToNode as ContextNode).MetaObject, out to);
                            if (to != null)
                            {
                                if (((o as QLink).FromNode as ContextNode).Diagram.Length > 0)
                                {
                                    to.AllocationHandle.Items.Add(new AllocationHandle.AllocationItem(((o as QLink).FromNode as ContextNode).Diagram.ToString(), ""));
                                }
                                else
                                {
                                    //this happens when we open a view because it did not save the path
                                    to.AllocationHandle.Items.Add(new AllocationHandle.AllocationItem(((o as QLink).FromNode as ContextNode).Text.ToString(), ""));
                                }
                                to.AllocationHandle.SetStyle();
                                continue;
                            }
                        }
                    }

                    if (((o as QLink).ToNode as ContextNode).MetaObject != null && ((o as QLink).FromNode as ContextNode).MetaObject != null)
                    {
                        //add link from default graphnode port to default graphnode port
                        GraphNode to = null;
                        GraphNode from = null;
                        baseAndNode.TryGetValue(((o as QLink).ToNode as ContextNode).MetaObject, out to);
                        baseAndNode.TryGetValue(((o as QLink).FromNode as ContextNode).MetaObject, out from);

                        //rationales
                        if (((o as QLink).ToNode as ContextNode).MetaObject.Class == "Rationale" && from != null)
                        {
                            DockingForm.DockForm.UpdateStatusLabel("Rationale");
                            DockingForm.DockForm.ProgressUpdate(50);
                            //add to from
                            MetaBuilder.Graphing.Shapes.Nodes.Rationale rat = new MetaBuilder.Graphing.Shapes.Nodes.Rationale();
                            rat.BindingInfo = new BindingInfo();
                            rat.BindingInfo.BindingClass = ((o as QLink).ToNode as ContextNode).MetaObject.Class;
                            rat.Location = ((o as QLink).ToNode as ContextNode).Location;
                            rat.MetaObject = ((o as QLink).ToNode as ContextNode).MetaObject;
                            rat.HookupEvents();
                            rat.BindToMetaObjectProperties();
                            rat.Anchor = from;
                            convertContainer.View.Document.Add(rat);
                            continue;
                        }

                        //links and artefacts
                        if (from != null && to != null)
                        {
                            DockingForm.DockForm.UpdateStatusLabel("Link");
                            DockingForm.DockForm.ProgressUpdate(50);
                            QLink newlink = null;
                            MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation fromLocation = FindPortLocation((o as QLink), false);
                            MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation toLocation = FindPortLocation((o as QLink), true);

                            if (fromLocation != MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Circumferential && toLocation != MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Circumferential)
                                newlink = QLink.CreateLink(from, to, (int)(o as QLink).AssociationType, fromLocation, toLocation);
                            else
                                newlink = QLink.CreateLink(from, to, (int)(o as QLink).AssociationType, -1);

                            if (newlink != null)
                            {
                                convertContainer.View.Document.Add(newlink);

                                //artifact(s)
                                foreach (ContextNode contextArtefact in (o as QLink).GetContextArtefacts())
                                {
                                    ArtefactNode artefactNode = new ArtefactNode();
                                    artefactNode.MetaObject = contextArtefact.MetaObject;
                                    artefactNode.HookupEvents();
                                    artefactNode.BindToMetaObjectProperties();
                                    artefactNode.Location = contextArtefact.Location;

                                    convertContainer.View.Document.Add(artefactNode);
                                    convertContainer.ViewController.FishLinkArtefact(artefactNode, newlink);
                                }
                            }
                        }
                    }
                }
            }
            DockingForm.DockForm.UpdateStatusLabel("Completing");
            DockingForm.DockForm.ProgressUpdate(75);
        }
        private void convertWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DockingForm.DockForm.ResetStatus();

            convertContainer.cropGlobal();
            convertContainer.Show(DockingForm.DockForm.dockPanel1);

            ConvertContextToDocument.Enabled = true;
            //convertContainer = null;
        }

        private MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation FindPortLocation(QLink contextLink, bool to)
        {
            ContextNode nodeToUse = null;
            PointF locationToUse = new PointF(0, 0);
            if (to)
            {
                nodeToUse = contextLink.ToNode as ContextNode;
                locationToUse = contextLink.RealLink.GetPoint(contextLink.RealLink.PointsCount - 1);
            }
            else
            {
                nodeToUse = contextLink.FromNode as ContextNode;
                locationToUse = contextLink.RealLink.GetPoint(0);
            }

            //bool x = (locationToUse.Y - nodeToUse.Center.Y) < 20 || (locationToUse.Y - nodeToUse.Center.Y) > -20;
            //bool y = (locationToUse.X - nodeToUse.Center.X) < 20 || (locationToUse.X - nodeToUse.Center.X) > -20;

            if (locationToUse.X > nodeToUse.Center.X) //when y is the same
            {
                return MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Right;
            }
            else if (locationToUse.X < nodeToUse.Center.X)
            {
                return MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Left;
            }
            else  //when x is the same (never use links to top so this does not matter
            {
                if (locationToUse.Y > nodeToUse.Center.Y)
                {
                    return MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Top;
                }
                else if (locationToUse.Y < nodeToUse.Center.Y)
                {
                    return MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Bottom;
                }
            }

            return MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Circumferential;
        }

        #endregion
    }
}