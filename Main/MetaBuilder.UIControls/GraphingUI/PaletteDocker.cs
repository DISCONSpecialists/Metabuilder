using System;
using System.Drawing;
using System.Windows.Forms;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.Core.Storage;
using MetaBuilder.Docking;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Persistence;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Tools;
using MetaBuilder.Graphing.Utilities;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Northwoods.Go;
using b = MetaBuilder.BusinessLogic;
using FileType = MetaBuilder.BusinessLogic.FileType;
using System.Data;

namespace MetaBuilder.UIControls.GraphingUI
{
    public partial class PaletteDocker : DockContent
    {

        #region Constructors (1)

        public PaletteDocker()
        {
            InitializeComponent();
            SetDescription("Select a shape", "After you select a shape a description of its class will be displayed here.", true);

            dockPanel1.ActiveContentChanged += new EventHandler(dockPanel1_ActiveContentChanged);
            dockPanel1.ContentRemoved += new EventHandler<DockContentEventArgs>(dockPanel1_ContentRemoved);
            dockPanel1.Enter += new EventHandler(dockPanel1_Enter);
            dockPanel1.Leave += new EventHandler(dockPanel1_Leave);

            toolStripSeparator1.Visible = false;
            toolStripButtonDescriptions.Visible = false;
            toolStripButtonDescriptions.CheckedChanged += new EventHandler(toolStripButtonDescriptions_CheckedChanged);
        }

        private void dockPanel1_Leave(object sender, EventArgs e)
        {
            //if (dockPanel1.ActiveDocument is PaletteContainer)
            //    if ((dockPanel1.ActiveDocument as PaletteContainer).Classes.Count == 0 && DockingForm.DockForm.GetCurrentGraphViewContainer() != null)
            //        if (DockingForm.DockForm.GetCurrentGraphViewContainer().DiagramTypePallette == null)
            //            DockingForm.DockForm.GetCurrentGraphViewContainer().SelectedPallette = dockPanel1.ActiveDocument as PaletteContainer;
        }

        private void dockPanel1_Enter(object sender, EventArgs e)
        {
            //if (dockPanel1.ActiveDocument is PaletteContainer)
            //    if ((dockPanel1.ActiveDocument as PaletteContainer).Classes.Count == 0 && DockingForm.DockForm.GetCurrentGraphViewContainer() != null)
            //        if (DockingForm.DockForm.GetCurrentGraphViewContainer().DiagramTypePallette == null)
            //            DockingForm.DockForm.GetCurrentGraphViewContainer().SelectedPallette = dockPanel1.ActiveDocument as PaletteContainer;
        }

        #endregion Constructors

        #region Methods (14)

        // Public Methods (2) 

        public void ExecuteSave(string filename, FileDialogSpecification spec)
        {
            try
            {
                // Saves the comparer's sort order
                PaletteShapeComparer comp =
                    ((PaletteContainer)dockPanel1.ActiveDocument).Palette.Comparer as PaletteShapeComparer;
                GoDocument doc = ((PaletteContainer)dockPanel1.ActiveDocument).Palette.Document;
                foreach (GoObject o in doc)
                {
                    if (o is GoNode)
                    {
                        GoNode n = o as GoNode;
                        n.UserObject = comp.ObjectStack.IndexOf(n);
                    }
                }
                MetaBuilder.Graphing.Persistence.XmlPersistor xpersist = new XmlPersistor();
                xpersist.PersistGoCollection(((PaletteContainer)dockPanel1.ActiveDocument).Palette.Document, filename,
                                             MetaBuilder.Graphing.Persistence.FileType.Stencil);
                /*StorageManipulator.FileSystemManipulator.SaveDocument(
                        ((PaletteContainer) dockPanel1.ActiveDocument).Palette.Document as BaseDocument, filename);*/
                ((PaletteContainer)dockPanel1.ActiveDocument).TabText = strings.GetFileNameOnly(filename);
                FilePathManager.Instance.SetLastUsedPath(spec.FileType, strings.GetPath(filename));
            }
            catch (Exception xxxCantSave)
            {
                MessageBox.Show(this, "The specified file is read-only");
                LogEntry lentry = new LogEntry();
                lentry.Title = "Cannot save";
                lentry.Message = xxxCantSave.ToString();
                Logger.Write(lentry);
            }
        }

        public GraphPalette GetCurrentPalette()
        {
            if ((PaletteContainer)dockPanel1.ActiveDocument != null)
                return ((PaletteContainer)dockPanel1.ActiveDocument).Palette;

            SetDescription("", "", false);
            return null;
        }

        // Protected Methods (1) 

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdateItems();

            //toolStripSeparator1.Visible = false;
            //toolStripButtonDescriptions.Visible = false;
        }

        public void UpdateItems()
        {
            stripButtonNew.Visible = Variables.Instance.ShowDeveloperItems;
            stripButtonSave.Visible = Variables.Instance.ShowDeveloperItems;
        }
        // Private Methods (10) 

        private void cxMenuItemClose_Click(object sender, EventArgs e)
        {
        }

        private void FixDSDShapes(IGoCollection col)
        {
            foreach (GoObject o in col)
            {
                if (o is GraphNode)
                {
                    GraphNode n = o as GraphNode;
                    if (n.HasBindingInfo)
                    {
                        if (n.BindingInfo.BindingClass == "DataTable")
                        {
                            n.BoundLabels[0].Wrapping = true;
                            n.BoundLabels[0].WrappingWidth = n.BoundLabels[0].Width - 20;
                        }
                    }
                    //n.BoundLabels[0].
                }
            }
        }

        private void Palette_CloseStencil(object sender, EventArgs e)
        {
            try
            {
                dockPanel1.ActiveDocumentPane.CloseActiveContent();
                SetDescription("", "", false);
            }
            catch
            {
            }
        }

        private void stripButtonCopy_Click(object sender, EventArgs e)
        {
        }

        private void stripButtonCut_Click(object sender, EventArgs e)
        {
        }

        private void stripButtonNew_Click(object sender, EventArgs e)
        {
            PaletteContainer plc = new PaletteContainer();
            plc.Text = "New SymbolStore";
            plc.Show(dockPanel1);
        }

        public PaletteContainer New(string diagramType)
        {
            foreach (DockContent pallette in dockPanel1.Contents)
                if (pallette is PaletteContainer)
                    if ((pallette as PaletteContainer).Text == diagramType + " Stencil") //2 of the same diagramtype!
                        return pallette as PaletteContainer;

            //get diagramtype from database and create stencil for it
            DataSet d = DataAccessLayer.DataRepository.Provider.ExecuteDataSet(CommandType.Text, "SELECT * FROM ModelType WHERE Acronym = '" + diagramType + "'");
            if (d.Tables[0].Rows.Count == 1)
            {
                PaletteContainer plc = new PaletteContainer();
                plc.Text = diagramType + " Stencil";
                plc.Show(dockPanel1);
                //get allowed classes
                DataSet dc = DataAccessLayer.DataRepository.Provider.ExecuteDataSet(CommandType.Text, "SELECT * FROM ModelTypeClass WHERE ModelTypeAcronym = '" + diagramType + "'");
                foreach (DataRow row in dc.Tables[0].Rows)
                {
                    string c = row[0].ToString();
                    if (!(plc.Classes.Contains(c)))
                        plc.Classes.Add(c);

                    try
                    {
                        plc.Palette.Document.Add(((Core.Variables.Instance.ReturnShape(c) as GraphNode).Copy() as GraphNode));
                    }
                    catch //Shape Cache has not loaded fast enough
                    {
                        GoText te = new GoText();
                        te.Text = c + "::Shape Missing";
                        te.Editable = false;
                        te.BackgroundColor = Color.Red;
                        te.TextColor = Color.White;
                        te.FontSize = 20;
                        te.Bold = true;
                        plc.Palette.Document.Add(te);
                    }
                }
                plc.CloseButtonVisible = false;
                return plc;
            }
            return null;
        }

        public void stripButtonOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdialog = new OpenFileDialog();
            FileDialogSpecification fileSpec = FilePathManager.Instance.GetSpecification(FileTypeList.SymbolStore);
            ofdialog.Filter = fileSpec.Filter;
            ofdialog.InitialDirectory = fileSpec.DefaultPath;
            ofdialog.Multiselect = true;
            ofdialog.Title = "Open Symbol Store (Stencils)";
            ofdialog.SupportMultiDottedExtensions = true;
            DialogResult result = ofdialog.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                string[] files = ofdialog.FileNames;
                for (int i = 0; i < files.Length; i++)
                {
                    bool cancelLoad = false;
                    foreach (IDockContent content in dockPanel1.Contents)
                    {
                        PaletteContainer container = content as PaletteContainer;
                        if (container == null)
                            continue;
                        if (container.Text == strings.GetFileNameOnly(files[i]))
                        {
                            container.Show();
                            cancelLoad = true;
                            break;
                        }
                    }

                    if (cancelLoad)
                        continue;
                    //string filename = files[i];
                    bool errorsOccurred = false;
                    try
                    {
                        LoadDockedPaletteForFile(fileSpec, files[i]);
                    }
                    catch
                    {
                        errorsOccurred = true;
                    }
                    if (!errorsOccurred)
                    {
                    }
                    else
                    {
                        errorsOccurred = false;
                    }
                }
            }
        }

        private void stripButtonPaste_Click(object sender, EventArgs e)
        {
        }

        private void stripButtonPrint_Click(object sender, EventArgs e)
        {
        }

        private void stripButtonSave_Click(object sender, EventArgs e)
        {
            if (dockPanel1.ActiveDocument != null)
            {
                FileDialogSpecification spec = FilePathManager.Instance.GetSpecification(FileTypeList.SymbolStore);
                PaletteContainer pcontainer = (PaletteContainer)dockPanel1.ActiveDocument;
                if (pcontainer != null)
                {
                    // if ((pcontainer.Palette.Doc.Name != string.Empty) && (pcontainer.Palette.Doc.Name != "New SymbolStore"))
                    {
                        SaveFileDialog sfDialog = new SaveFileDialog();
                        sfDialog.Filter = spec.Filter;
                        sfDialog.InitialDirectory = spec.DefaultPath;
                        sfDialog.Title = spec.Title;
                        sfDialog.FileName = strings.GetFileNameOnly(pcontainer.Palette.Document.Name);
                        DialogResult res = sfDialog.ShowDialog(this);
                        if (res == DialogResult.OK)
                        {
                            ExecuteSave(sfDialog.FileName, spec);
                            FilePathManager.Instance.SetLastUsedPath(spec.FileType, strings.GetPath(sfDialog.FileName));
                            pcontainer.Palette.Document.Name = sfDialog.FileName;
                        }
                    }
                }
            }
        }

        private void dockPanel1_ActiveContentChanged(object sender, EventArgs e)
        {
            callObjectSelection();
        }

        private void dockPanel1_ContentRemoved(object sender, DockContentEventArgs e)
        {
            callObjectSelection();
        }

        private void toolStripButtonDescriptions_CheckedChanged(object sender, EventArgs e)
        {
            callObjectSelection();
        }

        private void callObjectSelection()
        {
            if (GetCurrentPalette() != null)
            {
                if (GetCurrentPalette().Selection.Count > 0)
                {
                    Palette_ObjectGotSelection(this, new GoSelectionEventArgs(GetCurrentPalette().Selection.First));
                    return;
                }
            }

            SetDescription("Select a shape", "After you select a shape a description of its class will be displayed here.", true);
        }

        // Internal Methods (1) 

        internal void LoadDockedPaletteForFile(FileDialogSpecification fileSpec, string filename)
        {
            FilePathManager.Instance.SetLastUsedPath(fileSpec.FileType, strings.GetPath(filename));
            PaletteContainer plc = new PaletteContainer();

            try
            {
                XmlPersistor xpersist = new XmlPersistor();
                plc.Palette.Document = xpersist.DepersistCollection(filename, MetaBuilder.Graphing.Persistence.FileType.Stencil) as BaseDocument;
            }
            catch (Exception ex)
            {
                Core.Log.WriteLog("Palatte failed to load" + Environment.NewLine + ex.ToString());
                plc.Palette.Document = StorageManipulator.FileSystemManipulator.LoadDocument(filename, fileSpec);
            }
            if (plc.Palette.Document != null)
            {
                foreach (GoObject obj in plc.Palette.Document)
                {
                    if (obj is GraphNode)
                    {
                        GraphNode dnode = obj as GraphNode;
                        if (dnode.BindingInfo != null)
                        {
                            //if (Variables.Instance.ShapeCache == null)
                            //    Variables.Instance.ShapeCache = new System.Collections.Generic.Dictionary<string, object>();
                            //add shape to cache so we are not asked to load this stencil at some point
                            if (!(Variables.Instance.ShapeCache.ContainsKey(dnode.BindingInfo.BindingClass)))
                                Variables.Instance.ShapeCache.Add(dnode.BindingInfo.BindingClass, dnode.Copy());
                        }
                    }
                }
            }

            FixDSDShapes(plc.Palette.Document);
            /* if (plc.Palette.Document!=null)
             {
                 GoDocument doc = plc.Palette.Document;
                 foreach (GoObject o in doc)
                 {
                     if (o is GraphNode)
                     {
                         GraphNode node = o as GraphNode;   
                         if (node.HasBindingInfo)
                         {
                             if (node.BindingInfo.BindingClass == "Responsibility")
                             {
                                 foreach (GoObject child in node)
                                 {
                                     if (child is GoText)
                                     {
                                         GoText txt = child as GoText; 
                                         if (txt.Text == "Responsibility")
                                         {
                                             if (!txt.Editable)
                                                txt.Printable = true;
                                         }
                                     }
                                 }
                             }
                         }
                     } 
                 }
             }*/
            //DocumentController docController = new DocumentController(plc.Palette.Document as BaseDocument);
            //docController.ApplyBrushes();
            plc.Text = strings.GetFileNameOnly(filename);
            plc.Palette.CloseStencil += new EventHandler(Palette_CloseStencil);
            plc.Palette.ObjectGotSelection += new GoSelectionEventHandler(Palette_ObjectGotSelection);
            plc.Palette.ObjectLostSelection += new GoSelectionEventHandler(Palette_ObjectLostSelection);
            plc.Palette.LayoutItems();
            plc.Show(dockPanel1);
        }

        private void Palette_ObjectLostSelection(object sender, GoSelectionEventArgs e)
        {
            callObjectSelection();
        }

        private void Palette_ObjectGotSelection(object sender, GoSelectionEventArgs e)
        {
            if (e.GoObject is IMetaNode)
            {
                if ((e.GoObject as IMetaNode).BindingInfo != null && (e.GoObject as IMetaNode).BindingInfo.BindingClass != null)
                    SetDescription((e.GoObject as IMetaNode).BindingInfo.BindingClass, Core.Variables.Instance.ClassDescription((e.GoObject as IMetaNode).BindingInfo.BindingClass), true);
            }
            else
                SetDescription("", "", false);
        }

        public void SetDescription(string c, string d, bool visible)
        {
            labelClass.Text = c;

            if (d.Length == 0)
                d = "No Description";
            labelDescription.Text = d;

            if (toolStripButtonDescriptions.Checked)
                splitContainer1.Panel2Collapsed = !visible;
            else
                splitContainer1.Panel2Collapsed = true;
        }
        #endregion Methods

        #region Overrides on DockContent parent class
        // When setting the text property of the form, also set the tabtext property
        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                TabText = value;
            }
        }
        #endregion
    }
}