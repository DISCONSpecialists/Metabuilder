using System.Windows.Forms;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Meta;
using MetaBuilder.MetaControls.Tasks;
using MetaBuilder.UIControls.Dialogs;
using Northwoods.Go;
using System.Collections.Generic;
namespace MetaBuilder.UIControls.GraphingUI
{
    partial class GraphViewContainer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        /*protected override void Dispose(bool disposing)
        {
   
            // reset the pan & zoom window
            
            //this.myView.ReleReleaseObjects();
            this.myView.Dispose();
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
            System.GC.Collect();
        }*/

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphViewContainer));
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor9 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor10 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor11 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor12 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor13 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor14 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor15 = new Northwoods.Go.Draw.GoRulerCursor();
            Northwoods.Go.Draw.GoRulerCursor goRulerCursor16 = new Northwoods.Go.Draw.GoRulerCursor();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.menuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFilePrint = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFileClose = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFileCloseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFileProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemEditUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemEditRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemEditCut = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemEditCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemEditPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemEditPasteShallowCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemEditSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemEditEnableRubberStamping = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemEditEnableAutoLinking = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemEditFind = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemEditFindAndReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsert = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertShape = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertShapeRectangle = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertShapeTriangle = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertShapeEllipse = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertShapeOtherShapes = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertShapesCurvedPolygon = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertShapesLinePolygon = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertShapesTrapezoid = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertCube = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertCylinder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertCylinderVertical = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertCylinderHorizontal = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertHexagon = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertParallelogram = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertOctagon = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertDiamond = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertHouseShape = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertArrow = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertBlockArrow = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertShapeStroke = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertLine = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertLineBezier = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertLineRounded = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertLineRoundedWithJumpOvers = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertIntellishape = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertIntellishapeSubgraphObject = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertIntellishapeSubgraphObjectNew = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertIntellishapeSubgraphObjectExisting = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertIntellishapeValueChainStep = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertIntellishapeValueChainStepNew = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertIntellishapeValueChainStepExisting = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertIntellishapeSwimlane = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertIntellishapeSwimlaneNew = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertIntellishapeSwimlaneExisting = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertLegend = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertLegendColor = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertLegendClass = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertLegendBoth = new System.Windows.Forms.ToolStripMenuItem();
            //this.menuItemInsertIntellishapeSwimlaneFromDB = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertRationale = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertText = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertTextLabel = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertRichText = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertHyperlink = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertComment = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertBalloonComment = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertImage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertFileAttachment = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertPort = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInsertRepeaterSection = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemViewGrid = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemViewZoom = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemViewZoom50 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemViewZoom100 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemViewZoom150 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemViewZoom200 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemViewZoom400 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemViewZoomPageWidth = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemViewZoomPageHeight = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemViewZoomWholePage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemViewPorts = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShape = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShapeAlign = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShapeAlignTops = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShapeAlignBottoms = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShapeAlignLeftSides = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShapeAlignRightSides = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShapeAlignHorizontalCenters = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShapeAlignVerticalCenters = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShapeLayoutShapes = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShapeLayoutShapesDigraph = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShapeLayoutShapesForceDirected = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShapeLayoutCircular = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShapeLayoutTree = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShapeLayoutFSD = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShapeLayoutArtefacts = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShapeOrder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShapeOrderBringForward = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShapeOrderBringToFront = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShapeOrderSendBackward = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShapeOrderSendToBack = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShapeDistribute = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShapeDistributeHorizontally = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShapeVertically = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShapeGrouping = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShapeGroupingGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShapeGroupingUngroup = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShapeGroupingCreateSubGraph = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShapeGroupingUngroupSubGraph = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFormat = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFormatFill = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFormatText = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFormatLine = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFormatCornerRounding = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemFormatCustomProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemTools = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsFindDifferences = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsFindIntersections = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsMergeDocuments = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsGridsAndRulers = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsSnapAndGlue = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsSpelling = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsValidateDocument = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsValidateDocumentNodeConnections = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsAutoRelinkAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsPortMover = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsPortFormatting = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsConvertCommentsToRationales = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAdvanced = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsDebugToXML = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsPlugins = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsHeatMap = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsHeatMapGapType = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsHeatMapMeasure = new System.Windows.Forms.ToolStripMenuItem();
            this.cropToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsCropToSheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsCropToDocumentFrameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsCropDocument = new System.Windows.Forms.ToolStripMenuItem();
            this.validateModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.validateModelToolStripMenuItemADD = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemToolsActivityReport = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuTabPage = new System.Windows.Forms.ContextMenu();
            this.cxMenuItemSave = new System.Windows.Forms.MenuItem();
            this.cxMenuItemClose = new System.Windows.Forms.MenuItem();
            this.cxMenuItemCloseAll = new System.Windows.Forms.MenuItem();
            this.cxMenuItemCloseAllButThis = new System.Windows.Forms.MenuItem();
            this.cxMenuItemOpenPath = new System.Windows.Forms.MenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.stripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.stripButtonPrint = new System.Windows.Forms.ToolStripButton();
            this.stripButtonCut = new System.Windows.Forms.ToolStripButton();
            this.stripButtonCopy = new System.Windows.Forms.ToolStripButton();
            this.stripButtonPaste = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripZoomLabel = new System.Windows.Forms.ToolStripLabel();
            //this.toolstripComboZoom = new System.Windows.Forms.ToolStripComboBox();
            this.goLayoutLayeredDigraph1 = new Northwoods.Go.Layout.GoLayoutLayeredDigraph();
            this.myView = new MetaBuilder.Graphing.Containers.GraphView();
            this.backgroundWorker1 = new MetaBuilder.UIControls.GraphingUI.DiagramSaver(this.components);
            this.toolStripFormat = new System.Windows.Forms.ToolStrip();
            this.toolStripFormatText = new System.Windows.Forms.ToolStripButton();
            this.toolStripFormatFill = new System.Windows.Forms.ToolStripButton();
            this.toolstripCopyFormatting = new System.Windows.Forms.ToolStripButton();
            this.menuItemViewFishLink = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemViewObjectImages = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu.SuspendLayout();
            //this.contextMenuTabPage.SuspendLayout();
            this.toolStripMain.SuspendLayout();
            this.toolStripFormat.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem1,
            this.editToolStripMenuItem,
            this.menuItemView,
            this.menuItemInsert,
            this.menuItemFormat,
            this.menuItemShape,
            this.menuItemTools});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(797, 24);
            this.mainMenu.TabIndex = 2;
            this.mainMenu.Visible = false;
            this.mainMenu.AllowMerge = true;
            // 
            // menuItem1
            // 
            this.menuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemFileSave,
            this.menuItemFileSaveAs,
            this.menuItemFilePrint,
            this.menuItemFileClose,
            this.menuItemFileCloseAll,
            this.menuItemFileProperties});
            this.menuItem1.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.menuItem1.MergeIndex = 0;
            this.menuItem1.Name = "menuItem1";
            this.menuItem1.Size = new System.Drawing.Size(35, 20);
            this.menuItem1.Text = "&File";
            // 
            // menuItemFileSave
            // 
            this.menuItemFileSave.Image = ((System.Drawing.Image)(resources.GetObject("menuItemFileSave.Image")));
            this.menuItemFileSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuItemFileSave.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.menuItemFileSave.MergeIndex = 4;
            this.menuItemFileSave.Name = "menuItemFileSave";
            this.menuItemFileSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuItemFileSave.Size = new System.Drawing.Size(146, 22);
            this.menuItemFileSave.Text = "&Save";
            this.menuItemFileSave.Click += new System.EventHandler(this.menuItemFileSave_Click);
            // 
            // menuItemFileSaveAs
            // 
            this.menuItemFileSaveAs.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.menuItemFileSaveAs.MergeIndex = 5;
            this.menuItemFileSaveAs.Name = "menuItemFileSaveAs";
            this.menuItemFileSaveAs.Size = new System.Drawing.Size(146, 22);
            this.menuItemFileSaveAs.Text = "Save &As";
            this.menuItemFileSaveAs.Click += new System.EventHandler(this.menuItemFileSaveAs_Click);
            // 
            // menuItemFilePrint
            // 
            this.menuItemFilePrint.Image = ((System.Drawing.Image)(resources.GetObject("menuItemFilePrint.Image")));
            this.menuItemFilePrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuItemFilePrint.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.menuItemFilePrint.MergeIndex = 6;
            this.menuItemFilePrint.Name = "menuItemFilePrint";
            this.menuItemFilePrint.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.menuItemFilePrint.Size = new System.Drawing.Size(146, 22);
            this.menuItemFilePrint.Text = "&Print";
            this.menuItemFilePrint.Click += new System.EventHandler(this.menuItemFilePrint_Click);
            // 
            // menuItemFileClose
            // 
            this.menuItemFileClose.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.menuItemFileClose.MergeIndex = 4;
            this.menuItemFileClose.Name = "menuItemFileClose";
            this.menuItemFileClose.Size = new System.Drawing.Size(146, 22);
            this.menuItemFileClose.Text = "&Close";
            this.menuItemFileClose.Click += new System.EventHandler(this.menuItemFileClose_Click);
            // 
            // menuItemFileCloseAll
            // 
            this.menuItemFileCloseAll.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.menuItemFileCloseAll.MergeIndex = 5;
            this.menuItemFileCloseAll.Name = "menuItemFileCloseAll";
            this.menuItemFileCloseAll.Size = new System.Drawing.Size(146, 22);
            this.menuItemFileCloseAll.Text = "Close A&ll";
            this.menuItemFileCloseAll.Click += new System.EventHandler(this.menuItemFileCloseAll_Click);
            // 
            // menuItemFileProperties
            // 
            this.menuItemFileProperties.Enabled = false;
            this.menuItemFileProperties.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.menuItemFileProperties.MergeIndex = 44;
            this.menuItemFileProperties.Name = "menuItemFileProperties";
            this.menuItemFileProperties.Size = new System.Drawing.Size(146, 22);
            this.menuItemFileProperties.Text = "Properties";
            this.menuItemFileProperties.Visible = false;
            this.menuItemFileProperties.Click += new System.EventHandler(this.menuItemFileProperties_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemEditUndo,
            this.menuItemEditRedo,
            this.menuItemEditCut,
            this.menuItemEditCopy,
            this.menuItemEditPaste,
            this.menuItemEditPasteShallowCopy,
            this.menuItemEditSelectAll,
            this.menuItemEditEnableRubberStamping,
            this.menuItemEditEnableAutoLinking,
            this.menuItemEditFind,
            this.menuItemEditFindAndReplace});
            this.editToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.editToolStripMenuItem.MergeIndex = 1;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // menuItemEditUndo
            // 
            this.menuItemEditUndo.Name = "menuItemEditUndo";
            this.menuItemEditUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.menuItemEditUndo.Size = new System.Drawing.Size(188, 22);
            this.menuItemEditUndo.Text = "&Undo";
            this.menuItemEditUndo.Click += new System.EventHandler(this.menuItemEditUndo_Click);
            // 
            // menuItemEditRedo
            // 
            this.menuItemEditRedo.Name = "menuItemEditRedo";
            this.menuItemEditRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.menuItemEditRedo.Size = new System.Drawing.Size(188, 22);
            this.menuItemEditRedo.Text = "&Redo";
            this.menuItemEditRedo.Click += new System.EventHandler(this.menuItemEditRedo_Click);
            // 
            // menuItemEditCut
            // 
            this.menuItemEditCut.Image = ((System.Drawing.Image)(resources.GetObject("menuItemEditCut.Image")));
            this.menuItemEditCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuItemEditCut.Name = "menuItemEditCut";
            this.menuItemEditCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.menuItemEditCut.Size = new System.Drawing.Size(188, 22);
            this.menuItemEditCut.Text = "Cu&t";
            this.menuItemEditCut.Click += new System.EventHandler(this.menuItemEditCut_Click);
            // 
            // menuItemEditCopy
            // 
            this.menuItemEditCopy.Image = ((System.Drawing.Image)(resources.GetObject("menuItemEditCopy.Image")));
            this.menuItemEditCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuItemEditCopy.Name = "menuItemEditCopy";
            this.menuItemEditCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.menuItemEditCopy.Size = new System.Drawing.Size(188, 22);
            this.menuItemEditCopy.Text = "&Copy";
            this.menuItemEditCopy.Click += new System.EventHandler(this.menuItemEditCopy_Click);
            // 
            // menuItemEditPaste
            // 
            this.menuItemEditPaste.Image = ((System.Drawing.Image)(resources.GetObject("menuItemEditPaste.Image")));
            this.menuItemEditPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuItemEditPaste.Name = "menuItemEditPaste";
            this.menuItemEditPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.menuItemEditPaste.Size = new System.Drawing.Size(188, 22);
            this.menuItemEditPaste.Text = "&Paste";
            this.menuItemEditPaste.Click += new System.EventHandler(this.menuItemEditPaste_Click);
            // 
            // menuItemEditPasteShallowCopy
            // 
            this.menuItemEditPasteShallowCopy.Name = "menuItemEditPasteShallowCopy";
            this.menuItemEditPasteShallowCopy.Size = new System.Drawing.Size(188, 22);
            this.menuItemEditPasteShallowCopy.Text = "&Paste Shallow Copy";
            this.menuItemEditPasteShallowCopy.Click += new System.EventHandler(this.menuItemEditAddShallowCopy_Click);
            this.menuItemEditPasteShallowCopy.Visible = false; //MADE INVISIBLE BECAUSE PASTE DOES THE SAME THING BETTER
            // 
            // menuItemEditSelectAll
            // 
            this.menuItemEditSelectAll.Name = "menuItemEditSelectAll";
            this.menuItemEditSelectAll.Size = new System.Drawing.Size(188, 22);
            this.menuItemEditSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.menuItemEditSelectAll.Text = "Select &All";
            this.menuItemEditSelectAll.Click += new System.EventHandler(this.menuItemEditSelectAll_Click);
            // 
            // menuItemEditEnableRubberStamping
            // 
            this.menuItemEditEnableRubberStamping.Enabled = false;
            this.menuItemEditEnableRubberStamping.Name = "menuItemEditEnableRubberStamping";
            this.menuItemEditEnableRubberStamping.Size = new System.Drawing.Size(188, 22);
            this.menuItemEditEnableRubberStamping.Text = "Enable RubberStamping";
            this.menuItemEditEnableRubberStamping.Visible = false;
            this.menuItemEditEnableRubberStamping.Click += new System.EventHandler(this.menuItemEditEnableRubberStamping_Click);
            // 
            // menuItemEditEnableAutoLinking
            // 
            this.menuItemEditEnableAutoLinking.Enabled = false;
            this.menuItemEditEnableAutoLinking.Name = "menuItemEditEnableAutoLinking";
            this.menuItemEditEnableAutoLinking.Size = new System.Drawing.Size(188, 22);
            this.menuItemEditEnableAutoLinking.Text = "Enable Auto-Linking";
            this.menuItemEditEnableAutoLinking.Visible = false;
            this.menuItemEditEnableAutoLinking.Click += new System.EventHandler(this.menuItemEditEnableAutoLinking_Click);
            // 
            // menuItemEditFind
            // 
            this.menuItemEditFind.Name = "menuItemEditFind";
            this.menuItemEditFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.menuItemEditFind.Size = new System.Drawing.Size(188, 22);
            this.menuItemEditFind.Text = "Find";
            this.menuItemEditFind.Click += new System.EventHandler(this.menuItemEditFind_Click);
            // 
            // menuItemEditFindAndReplace
            // 
            this.menuItemEditFindAndReplace.Name = "menuItemEditFindAndReplace";
            this.menuItemEditFindAndReplace.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.menuItemEditFindAndReplace.Size = new System.Drawing.Size(188, 22);
            this.menuItemEditFindAndReplace.Text = "Find and replace";
            this.menuItemEditFindAndReplace.Click += new System.EventHandler(this.menuItemEditFindAndReplace_Click);
            // 
            // menuItemInsert
            // 
            this.menuItemInsert.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            //this.menuItemInsertShape,
            //this.menuItemInsertIntellishape,
            this.menuItemInsertIntellishapeSubgraphObject,
            //this.menuItemInsertIntellishapeValueChainStep,
            this.menuItemInsertIntellishapeSwimlane,
            this.menuItemInsertShape,
            this.menuItemInsertText,
            this.menuItemInsertImage,
            this.menuItemInsertFileAttachment,
            this.menuItemInsertLegend,
            this.menuItemInsertPort,
            this.menuItemInsertRepeaterSection});
            this.menuItemInsert.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.menuItemInsert.MergeIndex = 3;
            this.menuItemInsert.Name = "menuItemInsert";
            this.menuItemInsert.Size = new System.Drawing.Size(48, 20);
            this.menuItemInsert.Text = "&Insert";
            // 
            // menuItemInsertShape
            // 
            this.menuItemInsertShape.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemInsertShapeRectangle,
            this.menuItemInsertShapeTriangle,
            this.menuItemInsertShapeEllipse,
            this.menuItemInsertShapeOtherShapes,
            this.menuItemInsertShapeStroke});
            this.menuItemInsertShape.Name = "menuItemInsertShape";
            this.menuItemInsertShape.Size = new System.Drawing.Size(157, 22);
            this.menuItemInsertShape.Text = "Shape";
            // 
            // menuItemInsertShapeRectangle
            // 
            this.menuItemInsertShapeRectangle.Name = "menuItemInsertShapeRectangle";
            this.menuItemInsertShapeRectangle.Size = new System.Drawing.Size(140, 22);
            this.menuItemInsertShapeRectangle.Text = "Rectangle";
            this.menuItemInsertShapeRectangle.Click += new System.EventHandler(this.menuItemInsertRectangle_Click);
            // 
            // menuItemInsertShapeTriangle
            // 
            this.menuItemInsertShapeTriangle.Name = "menuItemInsertShapeTriangle";
            this.menuItemInsertShapeTriangle.Size = new System.Drawing.Size(140, 22);
            this.menuItemInsertShapeTriangle.Text = "Triangle";
            this.menuItemInsertShapeTriangle.Click += new System.EventHandler(this.menuItemInsertTriangle_Click);
            // 
            // menuItemInsertShapeEllipse
            // 
            this.menuItemInsertShapeEllipse.Name = "menuItemInsertShapeEllipse";
            this.menuItemInsertShapeEllipse.Size = new System.Drawing.Size(140, 22);
            this.menuItemInsertShapeEllipse.Text = "Ellipse";
            this.menuItemInsertShapeEllipse.Click += new System.EventHandler(this.menuItemInsertEllipse_Click);
            // 
            // menuItemInsertShapeOtherShapes
            // 
            this.menuItemInsertShapeOtherShapes.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemInsertShapesCurvedPolygon,
            this.menuItemInsertShapesLinePolygon,
            this.menuItemInsertShapesTrapezoid,
            this.menuItemInsertCube,
            this.menuItemInsertCylinder,
            this.menuItemInsertHexagon,
            this.menuItemInsertParallelogram,
            this.menuItemInsertOctagon,
            this.menuItemInsertDiamond,
            this.menuItemInsertHouseShape,
            this.menuItemInsertArrow,
            this.menuItemInsertBlockArrow});
            this.menuItemInsertShapeOtherShapes.Name = "menuItemInsertShapeOtherShapes";
            this.menuItemInsertShapeOtherShapes.Size = new System.Drawing.Size(140, 22);
            this.menuItemInsertShapeOtherShapes.Text = "Other Shapes";
            // 
            // menuItemInsertShapesCurvedPolygon
            // 
            this.menuItemInsertShapesCurvedPolygon.Name = "menuItemInsertShapesCurvedPolygon";
            this.menuItemInsertShapesCurvedPolygon.Size = new System.Drawing.Size(150, 22);
            this.menuItemInsertShapesCurvedPolygon.Text = "Curved Polygon";
            this.menuItemInsertShapesCurvedPolygon.Click += new System.EventHandler(this.menuItemInsertPolygon_Click);
            // 
            // menuItemInsertShapesLinePolygon
            // 
            this.menuItemInsertShapesLinePolygon.Name = "menuItemInsertShapesLinePolygon";
            this.menuItemInsertShapesLinePolygon.Size = new System.Drawing.Size(150, 22);
            this.menuItemInsertShapesLinePolygon.Text = "Line Polygon";
            this.menuItemInsertShapesLinePolygon.Click += new System.EventHandler(this.menuItemInsertLinePolygon_Click);
            // 
            // menuItemInsertShapesTrapezoid
            // 
            this.menuItemInsertShapesTrapezoid.Name = "menuItemInsertShapesTrapezoid";
            this.menuItemInsertShapesTrapezoid.Size = new System.Drawing.Size(150, 22);
            this.menuItemInsertShapesTrapezoid.Text = "Trapezoid";
            this.menuItemInsertShapesTrapezoid.Click += new System.EventHandler(this.menuItemInsertTrapezoid_Click);
            // 
            // menuItemInsertCube
            // 
            this.menuItemInsertCube.Name = "menuItemInsertCube";
            this.menuItemInsertCube.Size = new System.Drawing.Size(150, 22);
            this.menuItemInsertCube.Text = "Cube";
            this.menuItemInsertCube.Click += new System.EventHandler(this.menuItemInsertCube_Click);
            // 
            // menuItemInsertCylinder
            // 
            this.menuItemInsertCylinder.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemInsertCylinderVertical,
            this.menuItemInsertCylinderHorizontal});
            this.menuItemInsertCylinder.Name = "menuItemInsertCylinder";
            this.menuItemInsertCylinder.Size = new System.Drawing.Size(150, 22);
            this.menuItemInsertCylinder.Text = "Cylinder";
            // 
            // menuItemInsertCylinderVertical
            // 
            this.menuItemInsertCylinderVertical.Name = "menuItemInsertCylinderVertical";
            this.menuItemInsertCylinderVertical.Size = new System.Drawing.Size(122, 22);
            this.menuItemInsertCylinderVertical.Text = "Vertical";
            this.menuItemInsertCylinderVertical.Click += new System.EventHandler(this.menuItemInsertCylinderVertical_Click);
            // 
            // menuItemInsertCylinderHorizontal
            // 
            this.menuItemInsertCylinderHorizontal.Name = "menuItemInsertCylinderHorizontal";
            this.menuItemInsertCylinderHorizontal.Size = new System.Drawing.Size(122, 22);
            this.menuItemInsertCylinderHorizontal.Text = "Horizontal";
            this.menuItemInsertCylinderHorizontal.Click += new System.EventHandler(this.menuItemInsertCylinderHorizontal_Click);
            // 
            // menuItemInsertHexagon
            // 
            this.menuItemInsertHexagon.Name = "menuItemInsertHexagon";
            this.menuItemInsertHexagon.Size = new System.Drawing.Size(150, 22);
            this.menuItemInsertHexagon.Text = "Hexagon";
            this.menuItemInsertHexagon.Click += new System.EventHandler(this.menuItemInsertHexagon_Click);
            // 
            // menuItemInsertParallelogram
            // 
            this.menuItemInsertParallelogram.Name = "menuItemInsertParallelogram";
            this.menuItemInsertParallelogram.Size = new System.Drawing.Size(150, 22);
            this.menuItemInsertParallelogram.Text = "Parallelogram";
            this.menuItemInsertParallelogram.Click += new System.EventHandler(this.menuItemInsertParallelogram_Click);
            // 
            // menuItemInsertOctagon
            // 
            this.menuItemInsertOctagon.Name = "menuItemInsertOctagon";
            this.menuItemInsertOctagon.Size = new System.Drawing.Size(150, 22);
            this.menuItemInsertOctagon.Text = "Octagon";
            this.menuItemInsertOctagon.Click += new System.EventHandler(this.menuItemInsertOctagon_Click);
            // 
            // menuItemInsertDiamond
            // 
            this.menuItemInsertDiamond.Name = "menuItemInsertDiamond";
            this.menuItemInsertDiamond.Size = new System.Drawing.Size(150, 22);
            this.menuItemInsertDiamond.Text = "Diamond";
            this.menuItemInsertDiamond.Click += new System.EventHandler(this.menuItemInsertDiamond_Click);
            // 
            // menuItemInsertHouseShape
            // 
            this.menuItemInsertHouseShape.Name = "menuItemInsertHouseShape";
            this.menuItemInsertHouseShape.Size = new System.Drawing.Size(150, 22);
            this.menuItemInsertHouseShape.Text = "House Shape";
            this.menuItemInsertHouseShape.Click += new System.EventHandler(this.menuItemInsertHouseShape_Click);
            // 
            // menuItemInsertArrow
            // 
            this.menuItemInsertArrow.Name = "menuItemInsertArrow";
            this.menuItemInsertArrow.Size = new System.Drawing.Size(150, 22);
            this.menuItemInsertArrow.Text = "Arrow";
            this.menuItemInsertArrow.Click += new System.EventHandler(this.menuItemInsertArrow_Click);
            // 
            // menuItemInsertBlockArrow
            // 
            this.menuItemInsertBlockArrow.Name = "menuItemInsertBlockArrow";
            this.menuItemInsertBlockArrow.Size = new System.Drawing.Size(150, 22);
            this.menuItemInsertBlockArrow.Text = "Block Arrow";
            this.menuItemInsertBlockArrow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.menuItemInsertBlockArrow.Click += new System.EventHandler(this.menuItemInsertBlockArrow_Click);
            // 
            // menuItemInsertShapeStroke
            // 
            this.menuItemInsertShapeStroke.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemInsertLine,
            this.menuItemInsertLineBezier,
            this.menuItemInsertLineRounded,
            this.menuItemInsertLineRoundedWithJumpOvers});
            this.menuItemInsertShapeStroke.Name = "menuItemInsertShapeStroke";
            this.menuItemInsertShapeStroke.Size = new System.Drawing.Size(140, 22);
            this.menuItemInsertShapeStroke.Text = "Stroke";
            // 
            // menuItemInsertLine
            // 
            this.menuItemInsertLine.Name = "menuItemInsertLine";
            this.menuItemInsertLine.Size = new System.Drawing.Size(140, 22);
            this.menuItemInsertLine.Text = "Line";
            this.menuItemInsertLine.Click += new System.EventHandler(this.menuInsertLine_Click);
            // 
            // menuItemInsertLineBezier
            // 
            this.menuItemInsertLineBezier.Name = "menuItemInsertLineBezier";
            this.menuItemInsertLineBezier.Size = new System.Drawing.Size(140, 22);
            this.menuItemInsertLineBezier.Text = "Bezier";
            this.menuItemInsertLineBezier.Click += new System.EventHandler(this.menuItemInsertLineBezier_Click);
            // 
            // menuItemInsertLineRounded
            // 
            this.menuItemInsertLineRounded.Name = "menuItemInsertLineRounded";
            this.menuItemInsertLineRounded.Size = new System.Drawing.Size(140, 22);
            this.menuItemInsertLineRounded.Text = "Rounded Line";
            this.menuItemInsertLineRounded.Visible = false;
            this.menuItemInsertLineRounded.Click += new System.EventHandler(this.menuItemInsertLineRounded_Click);
            // 
            // menuItemInsertLineRoundedWithJumpOvers
            // 
            this.menuItemInsertLineRoundedWithJumpOvers.Name = "menuItemInsertLineRoundedWithJumpOvers";
            this.menuItemInsertLineRoundedWithJumpOvers.Size = new System.Drawing.Size(140, 22);
            this.menuItemInsertLineRoundedWithJumpOvers.Text = "Rounded Line With Jumpovers";
            this.menuItemInsertLineRoundedWithJumpOvers.Visible = false;
            this.menuItemInsertLineRoundedWithJumpOvers.Click += new System.EventHandler(this.menuItemInsertLineRoundedWithJumpOvers_Click);
            // 
            // menuItemInsertIntellishape
            // 
            this.menuItemInsertIntellishape.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            //this.menuItemInsertIntellishapeSubgraphObject,
            //this.menuItemInsertIntellishapeValueChainStep,
            //this.menuItemInsertIntellishapeSwimlane,
            this.menuItemInsertRationale});
            this.menuItemInsertIntellishape.Name = "menuItemInsertIntellishape";
            this.menuItemInsertIntellishape.Size = new System.Drawing.Size(157, 22);
            this.menuItemInsertIntellishape.Text = "Intellishape";
            // 
            // menuItemInsertIntellishapeSubgraphObject
            // 
            this.menuItemInsertIntellishapeSubgraphObject.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemInsertIntellishapeSubgraphObjectNew,
            this.menuItemInsertIntellishapeSubgraphObjectExisting});
            this.menuItemInsertIntellishapeSubgraphObject.Name = "menuItemInsertIntellishapeSubgraphObject";
            this.menuItemInsertIntellishapeSubgraphObject.Size = new System.Drawing.Size(205, 22);
            this.menuItemInsertIntellishapeSubgraphObject.Text = "Bound SubGraph";
            // 
            // menuItemInsertIntellishapeSubgraphObjectNew
            // 
            this.menuItemInsertIntellishapeSubgraphObjectNew.Name = "menuItemInsertIntellishapeSubgraphObjectNew";
            this.menuItemInsertIntellishapeSubgraphObjectNew.Size = new System.Drawing.Size(146, 22);
            this.menuItemInsertIntellishapeSubgraphObjectNew.Text = "New";
            this.menuItemInsertIntellishapeSubgraphObjectNew.Click += new System.EventHandler(this.menuItemInsertSubgraphObjectNew_Click);
            // 
            // menuItemInsertIntellishapeSubgraphObjectExisting
            // 
            this.menuItemInsertIntellishapeSubgraphObjectExisting.Name = "menuItemInsertIntellishapeSubgraphObjectExisting";
            this.menuItemInsertIntellishapeSubgraphObjectExisting.Size = new System.Drawing.Size(146, 22);
            this.menuItemInsertIntellishapeSubgraphObjectExisting.Text = "Existing";
            this.menuItemInsertIntellishapeSubgraphObjectExisting.Click += new System.EventHandler(this.menuItemInsertSubgraphObjectExisting_Click);
            // 
            // menuItemInsertIntellishapeValueChainStep
            // 
            this.menuItemInsertIntellishapeValueChainStep.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemInsertIntellishapeValueChainStepNew,
            this.menuItemInsertIntellishapeValueChainStepExisting});
            this.menuItemInsertIntellishapeValueChainStep.Name = "menuItemInsertIntellishapeValueChainStep";
            this.menuItemInsertIntellishapeValueChainStep.Size = new System.Drawing.Size(205, 22);
            this.menuItemInsertIntellishapeValueChainStep.Text = "Value Chain Step";
            // 
            // menuItemInsertIntellishapeValueChainStepNew
            // 
            this.menuItemInsertIntellishapeValueChainStepNew.Name = "menuItemInsertIntellishapeValueChainStepNew";
            this.menuItemInsertIntellishapeValueChainStepNew.Size = new System.Drawing.Size(123, 22);
            this.menuItemInsertIntellishapeValueChainStepNew.Text = "New";
            this.menuItemInsertIntellishapeValueChainStepNew.Click += new System.EventHandler(this.menuItemInsertValueChainStepNew_Click);
            // 
            // menuItemInsertIntellishapeValueChainStepExisting
            // 
            this.menuItemInsertIntellishapeValueChainStepExisting.Name = "menuItemInsertIntellishapeValueChainStepExisting";
            this.menuItemInsertIntellishapeValueChainStepExisting.Size = new System.Drawing.Size(123, 22);
            this.menuItemInsertIntellishapeValueChainStepExisting.Text = "Existing";
            this.menuItemInsertIntellishapeValueChainStepExisting.Click += new System.EventHandler(this.menuItemInsertValueChainStepExisting_Click);
            // 
            // menuItemInsertIntellishapeSwimlane
            // 
            this.menuItemInsertIntellishapeSwimlane.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemInsertIntellishapeSwimlaneNew,
            this.menuItemInsertIntellishapeSwimlaneExisting});
            this.menuItemInsertIntellishapeSwimlane.Name = "menuItemInsertIntellishapeSwimlane";
            this.menuItemInsertIntellishapeSwimlane.Size = new System.Drawing.Size(205, 22);
            this.menuItemInsertIntellishapeSwimlane.Text = "Swimlane";
            this.menuItemInsertIntellishapeSwimlane.Visible = true;
            // 
            // menuItemInsertIntellishapeSwimlaneNew
            // 
            this.menuItemInsertIntellishapeSwimlaneNew.Name = "menuItemInsertIntellishapeSwimlaneNew";
            this.menuItemInsertIntellishapeSwimlaneNew.Size = new System.Drawing.Size(205, 22);
            this.menuItemInsertIntellishapeSwimlaneNew.Text = "New";
            this.menuItemInsertIntellishapeSwimlaneNew.Click += new System.EventHandler(this.menuItemInsertSwimlane_Click);
            // 
            // menuItemInsertIntellishapeSwimlaneNew
            // 
            this.menuItemInsertIntellishapeSwimlaneExisting.Name = "menuItemInsertIntellishapeSwimlaneExisting";
            this.menuItemInsertIntellishapeSwimlaneExisting.Size = new System.Drawing.Size(205, 22);
            this.menuItemInsertIntellishapeSwimlaneExisting.Text = "Existing";
            this.menuItemInsertIntellishapeSwimlaneExisting.Click += new System.EventHandler(this.swimlanesFromDataToolStripMenuItem_Click);
            //
            //menuItemInsertLegend
            //
            this.menuItemInsertLegend.Name = "menuItemInsertLegend";
            this.menuItemInsertLegend.Size = new System.Drawing.Size(205, 22);
            this.menuItemInsertLegend.Text = "Legend";
            this.menuItemInsertLegend.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemInsertLegendClass,
            this.menuItemInsertLegendColor,
            this.menuItemInsertLegendBoth
            });
            //this.menuItemInsertLegend.Click += new System.EventHandler(this.menuItemInsertLegendMenuItem_Click);
            //
            //menuItemInsertLegendColor
            //
            this.menuItemInsertLegendColor.Name = "menuItemInsertLegendColor";
            this.menuItemInsertLegendColor.Size = new System.Drawing.Size(205, 22);
            this.menuItemInsertLegendColor.Text = "Colour";
            this.menuItemInsertLegendColor.Click += new System.EventHandler(this.menuItemInsertLegendMenuItemColor_Click);
            //
            //menuItemInsertLegendClass
            //
            this.menuItemInsertLegendClass.Name = "menuItemInsertLegendClass";
            this.menuItemInsertLegendClass.Size = new System.Drawing.Size(205, 22);
            this.menuItemInsertLegendClass.Text = "Class";
            this.menuItemInsertLegendClass.Click += new System.EventHandler(this.menuItemInsertLegendMenuItemClass_Click);
            //
            //menuItemInsertLegendBoth
            //
            this.menuItemInsertLegendBoth.Name = "menuItemInsertLegendBoth";
            this.menuItemInsertLegendBoth.Size = new System.Drawing.Size(205, 22);
            this.menuItemInsertLegendBoth.Text = "Class and Colour";
            this.menuItemInsertLegendBoth.Click += new System.EventHandler(this.menuItemInsertLegendMenuItemBoth_Click);
            //
            // menuItemInsertRationale
            // 
            this.menuItemInsertRationale.Name = "menuItemInsertRationale";
            this.menuItemInsertRationale.Size = new System.Drawing.Size(205, 22);
            this.menuItemInsertRationale.Text = "Rationale";
            this.menuItemInsertRationale.Click += new System.EventHandler(this.menuItemInsertRationale_Click);
            // 
            // menuItemInsertText
            // 
            this.menuItemInsertText.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemInsertTextLabel,
            this.menuItemInsertRichText,
            this.menuItemInsertHyperlink,
            this.menuItemInsertComment,
            this.menuItemInsertBalloonComment});
            this.menuItemInsertText.Name = "menuItemInsertText";
            this.menuItemInsertText.Size = new System.Drawing.Size(157, 22);
            this.menuItemInsertText.Text = "Text";
            // 
            // menuItemInsertTextLabel
            // 
            this.menuItemInsertTextLabel.Name = "menuItemInsertTextLabel";
            this.menuItemInsertTextLabel.Size = new System.Drawing.Size(156, 22);
            this.menuItemInsertTextLabel.Text = "Label";
            this.menuItemInsertTextLabel.Click += new System.EventHandler(this.menuItemInsertLabel_Click);
            // 
            // menuItemInsertRichText
            // 
            this.menuItemInsertRichText.Name = "menuItemInsertRichText";
            this.menuItemInsertRichText.Size = new System.Drawing.Size(156, 22);
            this.menuItemInsertRichText.Text = "RichText";
            this.menuItemInsertRichText.Click += new System.EventHandler(this.menuItemInsertRichText_Click);
            // 
            // menuItemInsertHyperlink
            // 
            this.menuItemInsertHyperlink.Name = "menuItemInsertHyperlink";
            this.menuItemInsertHyperlink.Size = new System.Drawing.Size(156, 22);
            this.menuItemInsertHyperlink.Text = "Hyperlink";
            this.menuItemInsertHyperlink.Click += new System.EventHandler(this.menuItemInsertHyperlink_Click);
            // 
            // menuItemInsertComment
            // 
            this.menuItemInsertComment.Name = "menuItemInsertComment";
            this.menuItemInsertComment.Size = new System.Drawing.Size(156, 22);
            this.menuItemInsertComment.Text = "Comment";
            this.menuItemInsertComment.Click += new System.EventHandler(this.menuItemInsertComment_Click);
            // 
            // menuItemInsertBalloonComment
            // 
            this.menuItemInsertBalloonComment.Name = "menuItemInsertBalloonComment";
            this.menuItemInsertBalloonComment.Size = new System.Drawing.Size(156, 22);
            this.menuItemInsertBalloonComment.Text = "Balloon Comment";
            this.menuItemInsertBalloonComment.Click += new System.EventHandler(this.menuItemInsertBalloonComment_Click);
            // 
            // menuItemInsertImage
            // 
            this.menuItemInsertImage.Name = "menuItemInsertImage";
            this.menuItemInsertImage.Size = new System.Drawing.Size(157, 22);
            this.menuItemInsertImage.Text = "Picture";
            this.menuItemInsertImage.Click += new System.EventHandler(this.menuItemInsertImage_Click);
            // 
            // menuItemInsertFileAttachment
            // 
            this.menuItemInsertFileAttachment.Name = "menuItemInsertFileAttachment";
            this.menuItemInsertFileAttachment.Size = new System.Drawing.Size(157, 22);
            this.menuItemInsertFileAttachment.Text = "File Attachment";
            this.menuItemInsertFileAttachment.Click += new System.EventHandler(this.menuItemInsertFileAttachment_Click);
            // 
            // menuItemInsertPort
            // 
            this.menuItemInsertPort.Name = "menuItemInsertPort";
            this.menuItemInsertPort.Size = new System.Drawing.Size(157, 22);
            this.menuItemInsertPort.Text = "Port";
            this.menuItemInsertPort.Visible = false;
            this.menuItemInsertPort.Click += new System.EventHandler(this.menuItemInsertPort_Click);
            // 
            // menuItemInsertRepeaterSection
            // 
            this.menuItemInsertRepeaterSection.Enabled = false;
            this.menuItemInsertRepeaterSection.Name = "menuItemInsertRepeaterSection";
            this.menuItemInsertRepeaterSection.Size = new System.Drawing.Size(157, 22);
            this.menuItemInsertRepeaterSection.Text = "Repeater Section";
            this.menuItemInsertRepeaterSection.Visible = false;
            // 
            // menuItemView
            // 
            this.menuItemView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemViewGrid,
            this.menuItemViewPorts,
            this.menuItemViewFishLink,
            this.menuItemViewObjectImages,
            this.menuItemViewZoom});
            this.menuItemView.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.menuItemView.MergeIndex = 2;
            this.menuItemView.Name = "menuItemView";
            this.menuItemView.Size = new System.Drawing.Size(41, 20);
            this.menuItemView.Text = "&View";
            //
            //menuItemViewObjectImages
            //
            this.menuItemViewObjectImages.Name = "menuItemViewObjectImages";
            this.menuItemViewObjectImages.Size = new System.Drawing.Size(100, 22);
            this.menuItemViewObjectImages.Text = "Shape Images";
            this.menuItemViewObjectImages.Visible = true;
            this.menuItemViewObjectImages.CheckOnClick = true;
            this.menuItemViewObjectImages.CheckState = CheckState.Checked;
            this.menuItemViewObjectImages.Click += new System.EventHandler(this.menuItemViewObjectImages_Click);
            //
            //menuItemViewFishLink
            //
            this.menuItemViewFishLink.Name = "menuItemViewFishLink";
            this.menuItemViewFishLink.Size = new System.Drawing.Size(100, 22);
            this.menuItemViewFishLink.Text = "Artefact Pointers";
            this.menuItemViewFishLink.Visible = true;
            this.menuItemViewFishLink.CheckOnClick = true;
            this.menuItemViewFishLink.CheckState = CheckState.Checked;
            this.menuItemViewFishLink.Click += new System.EventHandler(this.menuItemViewFishLink_Click);
            // 
            // menuItemViewGrid
            // 
            this.menuItemViewGrid.Name = "menuItemViewGrid";
            this.menuItemViewGrid.Size = new System.Drawing.Size(100, 22);
            this.menuItemViewGrid.Text = "Grid";
            this.menuItemViewGrid.Visible = false;
            this.menuItemViewGrid.Click += new System.EventHandler(this.menuItemViewGrid_Click);
            // 
            // menuItemViewZoom
            // 
            this.menuItemViewZoom.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemViewZoom50,
            this.menuItemViewZoom100,
            this.menuItemViewZoom150,
            this.menuItemViewZoom200,
            this.menuItemViewZoom400,
            this.menuItemViewZoomPageWidth,
            this.menuItemViewZoomPageHeight,
            this.menuItemViewZoomWholePage});
            this.menuItemViewZoom.Name = "menuItemViewZoom";
            this.menuItemViewZoom.Size = new System.Drawing.Size(100, 22);
            this.menuItemViewZoom.Text = "&Zoom";
            // 
            // menuItemViewZoom50
            // 
            this.menuItemViewZoom50.Name = "menuItemViewZoom50";
            this.menuItemViewZoom50.Size = new System.Drawing.Size(132, 22);
            this.menuItemViewZoom50.Text = "50%";
            this.menuItemViewZoom50.Click += new System.EventHandler(this.menuItemViewZoom50_Click);
            // 
            // menuItemViewZoom100
            // 
            this.menuItemViewZoom100.Name = "menuItemViewZoom100";
            this.menuItemViewZoom100.Size = new System.Drawing.Size(132, 22);
            this.menuItemViewZoom100.Text = "100%";
            this.menuItemViewZoom100.Click += new System.EventHandler(this.menuItemViewZoom100_Click);
            // 
            // menuItemViewZoom150
            // 
            this.menuItemViewZoom150.Name = "menuItemViewZoom150";
            this.menuItemViewZoom150.Size = new System.Drawing.Size(132, 22);
            this.menuItemViewZoom150.Text = "150%";
            this.menuItemViewZoom150.Click += new System.EventHandler(this.menuItemViewZoom150_Click);
            // 
            // menuItemViewZoom200
            // 
            this.menuItemViewZoom200.Name = "menuItemViewZoom200";
            this.menuItemViewZoom200.Size = new System.Drawing.Size(132, 22);
            this.menuItemViewZoom200.Text = "200%";
            this.menuItemViewZoom200.Click += new System.EventHandler(this.menuItemViewZoom200_Click);
            // 
            // menuItemViewZoom400
            // 
            this.menuItemViewZoom400.Name = "menuItemViewZoom400";
            this.menuItemViewZoom400.Size = new System.Drawing.Size(132, 22);
            this.menuItemViewZoom400.Text = "400%";
            this.menuItemViewZoom400.Click += new System.EventHandler(this.menuItemViewZoom400_Click);
            // 
            // menuItemViewZoomPageWidth
            // 
            this.menuItemViewZoomPageWidth.Name = "menuItemViewZoomPageWidth";
            this.menuItemViewZoomPageWidth.Size = new System.Drawing.Size(132, 22);
            this.menuItemViewZoomPageWidth.Text = "Page Width";
            this.menuItemViewZoomPageWidth.Click += new System.EventHandler(this.menuItemViewZoomPageWidth_Click);
            // 
            // menuItemViewZoomPageHeight
            // 
            this.menuItemViewZoomPageHeight.Name = "menuItemViewZoomPageHeight";
            this.menuItemViewZoomPageHeight.Size = new System.Drawing.Size(132, 22);
            this.menuItemViewZoomPageHeight.Text = "Page &Height";
            this.menuItemViewZoomPageHeight.Click += new System.EventHandler(this.menuItemViewZoomPageHeight_Click);
            // 
            // menuItemViewZoomWholePage
            // 
            this.menuItemViewZoomWholePage.Name = "menuItemViewZoomWholePage";
            this.menuItemViewZoomWholePage.Size = new System.Drawing.Size(132, 22);
            this.menuItemViewZoomWholePage.Text = "Whole Page";
            this.menuItemViewZoomWholePage.Click += new System.EventHandler(this.menuItemViewZoomWholePage_Click);
            // 
            // menuItemViewPorts
            // 
            this.menuItemViewPorts.Checked = true;
            this.menuItemViewPorts.CheckOnClick = true;
            this.menuItemViewPorts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemViewPorts.Name = "menuItemViewPorts";
            this.menuItemViewPorts.Size = new System.Drawing.Size(100, 22);
            this.menuItemViewPorts.Text = "Ports";
            this.menuItemViewPorts.Click += new System.EventHandler(this.menuItemViewPorts_Click);
            // 
            // menuItemShape
            // 
            this.menuItemShape.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemShapeAlign,
            this.menuItemShapeLayoutShapes,
            this.menuItemShapeOrder,
            this.menuItemShapeDistribute,
            this.menuItemShapeGrouping});
            this.menuItemShape.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.menuItemShape.MergeIndex = 5;
            this.menuItemShape.Name = "menuItemShape";
            this.menuItemShape.Size = new System.Drawing.Size(49, 20);
            this.menuItemShape.Text = "Shape";
            // 
            // menuItemShapeAlign
            // 
            this.menuItemShapeAlign.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemShapeAlignTops,
            this.menuItemShapeAlignBottoms,
            this.menuItemShapeAlignLeftSides,
            this.menuItemShapeAlignRightSides,
            this.menuItemShapeAlignHorizontalCenters,
            this.menuItemShapeAlignVerticalCenters});
            this.menuItemShapeAlign.Name = "menuItemShapeAlign";
            this.menuItemShapeAlign.Size = new System.Drawing.Size(145, 22);
            this.menuItemShapeAlign.Text = "&Align";
            // 
            // menuItemShapeAlignTops
            // 
            this.menuItemShapeAlignTops.Name = "menuItemShapeAlignTops";
            this.menuItemShapeAlignTops.Size = new System.Drawing.Size(163, 22);
            this.menuItemShapeAlignTops.Text = "Tops";
            this.menuItemShapeAlignTops.Click += new System.EventHandler(this.menuItemShapeAlignTops_Click);
            // 
            // menuItemShapeAlignBottoms
            // 
            this.menuItemShapeAlignBottoms.Name = "menuItemShapeAlignBottoms";
            this.menuItemShapeAlignBottoms.Size = new System.Drawing.Size(163, 22);
            this.menuItemShapeAlignBottoms.Text = "Bottoms";
            this.menuItemShapeAlignBottoms.Click += new System.EventHandler(this.menuItemShapeAlignBottoms_Click);
            // 
            // menuItemShapeAlignLeftSides
            // 
            this.menuItemShapeAlignLeftSides.Name = "menuItemShapeAlignLeftSides";
            this.menuItemShapeAlignLeftSides.Size = new System.Drawing.Size(163, 22);
            this.menuItemShapeAlignLeftSides.Text = "Left Sides";
            this.menuItemShapeAlignLeftSides.Click += new System.EventHandler(this.menuItemShapeAlignLeftSides_Click);
            // 
            // menuItemShapeAlignRightSides
            // 
            this.menuItemShapeAlignRightSides.Name = "menuItemShapeAlignRightSides";
            this.menuItemShapeAlignRightSides.Size = new System.Drawing.Size(163, 22);
            this.menuItemShapeAlignRightSides.Text = "Right Sides";
            this.menuItemShapeAlignRightSides.Click += new System.EventHandler(this.menuItemShapeAlignRightSides_Click);
            // 
            // menuItemShapeAlignHorizontalCenters
            // 
            this.menuItemShapeAlignHorizontalCenters.Name = "menuItemShapeAlignHorizontalCenters";
            this.menuItemShapeAlignHorizontalCenters.Size = new System.Drawing.Size(163, 22);
            this.menuItemShapeAlignHorizontalCenters.Text = "Horizontal Centers";
            this.menuItemShapeAlignHorizontalCenters.Click += new System.EventHandler(this.menuItemShapeAlignHorizontalCenters_Click);
            // 
            // menuItemShapeAlignVerticalCenters
            // 
            this.menuItemShapeAlignVerticalCenters.Name = "menuItemShapeAlignVerticalCenters";
            this.menuItemShapeAlignVerticalCenters.Size = new System.Drawing.Size(163, 22);
            this.menuItemShapeAlignVerticalCenters.Text = "Vertical Centers";
            this.menuItemShapeAlignVerticalCenters.Click += new System.EventHandler(this.menuItemShapeAlignVerticalCenters_Click);
            // 
            // menuItemShapeLayoutShapes
            // 
            this.menuItemShapeLayoutShapes.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemShapeLayoutShapesDigraph,
            this.menuItemShapeLayoutShapesForceDirected,
            this.menuItemShapeLayoutCircular,
            this.menuItemShapeLayoutTree,
            this.menuItemShapeLayoutFSD,
            this.menuItemShapeLayoutArtefacts});
            this.menuItemShapeLayoutShapes.Name = "menuItemShapeLayoutShapes";
            this.menuItemShapeLayoutShapes.Size = new System.Drawing.Size(145, 22);
            this.menuItemShapeLayoutShapes.Text = "Layout Shapes";
            // 
            // menuItemShapeLayoutShapesDigraph
            // 
            this.menuItemShapeLayoutShapesDigraph.Name = "menuItemShapeLayoutShapesDigraph";
            this.menuItemShapeLayoutShapesDigraph.Size = new System.Drawing.Size(155, 22);
            this.menuItemShapeLayoutShapesDigraph.Text = "Layered Digraph";
            this.menuItemShapeLayoutShapesDigraph.Click += new System.EventHandler(this.menuItemShapeLayoutShapesDigraph_Click);
            // 
            // menuItemShapeLayoutShapesForceDirected
            // 
            this.menuItemShapeLayoutShapesForceDirected.Name = "menuItemShapeLayoutShapesForceDirected";
            this.menuItemShapeLayoutShapesForceDirected.Size = new System.Drawing.Size(155, 22);
            this.menuItemShapeLayoutShapesForceDirected.Text = "Force Directed";
            this.menuItemShapeLayoutShapesForceDirected.Visible = false;
            this.menuItemShapeLayoutShapesForceDirected.Click += new System.EventHandler(this.menuItemShapeLayoutShapesForceDirected_Click);
            // 
            // menuItemShapeLayoutCircular
            // 
            this.menuItemShapeLayoutCircular.Name = "menuItemShapeLayoutCircular";
            this.menuItemShapeLayoutCircular.Size = new System.Drawing.Size(155, 22);
            this.menuItemShapeLayoutCircular.Text = "Circular";
            this.menuItemShapeLayoutCircular.Visible = false;
            this.menuItemShapeLayoutCircular.Click += new System.EventHandler(this.menuItemShapeLayoutCircular_Click);
            // 
            // menuItemShapeLayoutTree
            //
            this.menuItemShapeLayoutTree.Name = "menuItemShapeLayoutTree";
            this.menuItemShapeLayoutTree.Size = new System.Drawing.Size(155, 22);
            this.menuItemShapeLayoutTree.Text = "Tree";
            this.menuItemShapeLayoutTree.Click += new System.EventHandler(this.menuItemShapeLayoutTree_Click);
            // 
            // menuItemShapeLayoutFSD
            // 
            this.menuItemShapeLayoutFSD.Name = "menuItemShapeLayoutFSD";
            this.menuItemShapeLayoutFSD.Size = new System.Drawing.Size(155, 22);
            this.menuItemShapeLayoutFSD.Text = "FSD";
            this.menuItemShapeLayoutFSD.Visible = Core.Variables.Instance.ShowDeveloperItems;
            this.menuItemShapeLayoutFSD.Click += new System.EventHandler(this.menuItemShapeLayoutFSD_Click);
            // 
            // menuItemShapeLayoutArtefacts
            // 
            this.menuItemShapeLayoutArtefacts.Name = "menuItemShapeLayoutArtefacts";
            this.menuItemShapeLayoutArtefacts.Size = new System.Drawing.Size(155, 22);
            this.menuItemShapeLayoutArtefacts.Text = "Layout Artefacts";
            this.menuItemShapeLayoutArtefacts.Click += new System.EventHandler(this.menuItemShapeLayoutArtefacts_Click);
            // 
            // menuItemShapeOrder
            // 
            this.menuItemShapeOrder.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemShapeOrderBringForward,
            this.menuItemShapeOrderBringToFront,
            this.menuItemShapeOrderSendBackward,
            this.menuItemShapeOrderSendToBack});
            this.menuItemShapeOrder.Name = "menuItemShapeOrder";
            this.menuItemShapeOrder.Size = new System.Drawing.Size(145, 22);
            this.menuItemShapeOrder.Text = "&Order";
            // 
            // menuItemShapeOrderBringForward
            // 
            this.menuItemShapeOrderBringForward.Name = "menuItemShapeOrderBringForward";
            this.menuItemShapeOrderBringForward.Size = new System.Drawing.Size(147, 22);
            this.menuItemShapeOrderBringForward.Text = "Bring Forward";
            this.menuItemShapeOrderBringForward.Click += new System.EventHandler(this.menuItemShapeOrderBringForward_Click);
            // 
            // menuItemShapeOrderBringToFront
            // 
            this.menuItemShapeOrderBringToFront.Name = "menuItemShapeOrderBringToFront";
            this.menuItemShapeOrderBringToFront.Size = new System.Drawing.Size(147, 22);
            this.menuItemShapeOrderBringToFront.Text = "Bring to Front";
            this.menuItemShapeOrderBringToFront.Click += new System.EventHandler(this.menuItemShapeOrderBringToFront_Click);
            // 
            // menuItemShapeOrderSendBackward
            // 
            this.menuItemShapeOrderSendBackward.Name = "menuItemShapeOrderSendBackward";
            this.menuItemShapeOrderSendBackward.Size = new System.Drawing.Size(147, 22);
            this.menuItemShapeOrderSendBackward.Text = "Send Backward";
            this.menuItemShapeOrderSendBackward.Click += new System.EventHandler(this.menuItemShapeOrderSendBackward_Click);
            // 
            // menuItemShapeOrderSendToBack
            // 
            this.menuItemShapeOrderSendToBack.Name = "menuItemShapeOrderSendToBack";
            this.menuItemShapeOrderSendToBack.Size = new System.Drawing.Size(147, 22);
            this.menuItemShapeOrderSendToBack.Text = "Send to Back";
            this.menuItemShapeOrderSendToBack.Click += new System.EventHandler(this.menuItemShapeOrderSendToBack_Click);
            // 
            // menuItemShapeDistribute
            // 
            this.menuItemShapeDistribute.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemShapeDistributeHorizontally,
            this.menuItemShapeVertically});
            this.menuItemShapeDistribute.Name = "menuItemShapeDistribute";
            this.menuItemShapeDistribute.Size = new System.Drawing.Size(145, 22);
            this.menuItemShapeDistribute.Text = "Distribute";
            // 
            // menuItemShapeDistributeHorizontally
            // 
            this.menuItemShapeDistributeHorizontally.Name = "menuItemShapeDistributeHorizontally";
            this.menuItemShapeDistributeHorizontally.Size = new System.Drawing.Size(130, 22);
            this.menuItemShapeDistributeHorizontally.Text = "Horizontally";
            this.menuItemShapeDistributeHorizontally.Click += new System.EventHandler(this.menuItemShapeDistributeHorizontally_Click);
            // 
            // menuItemShapeVertically
            // 
            this.menuItemShapeVertically.Name = "menuItemShapeVertically";
            this.menuItemShapeVertically.Size = new System.Drawing.Size(130, 22);
            this.menuItemShapeVertically.Text = "Vertically";
            this.menuItemShapeVertically.Click += new System.EventHandler(this.menuItemShapeVertically_Click);
            // 
            // menuItemShapeGrouping
            // 
            this.menuItemShapeGrouping.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemShapeGroupingGroup,
            this.menuItemShapeGroupingUngroup});
            if (Core.Variables.Instance.ShowDeveloperItems)
            {
                this.menuItemShapeGrouping.DropDownItems.Add(this.menuItemShapeGroupingCreateSubGraph);
                this.menuItemShapeGrouping.DropDownItems.Add(this.menuItemShapeGroupingUngroupSubGraph);
            }
            this.menuItemShapeGrouping.Name = "menuItemShapeGrouping";
            this.menuItemShapeGrouping.Size = new System.Drawing.Size(145, 22);
            this.menuItemShapeGrouping.Text = "Grouping";
            // 
            // menuItemShapeGroupingGroup
            // 
            this.menuItemShapeGroupingGroup.Name = "menuItemShapeGroupingGroup";
            this.menuItemShapeGroupingGroup.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.menuItemShapeGroupingGroup.Size = new System.Drawing.Size(184, 22);
            this.menuItemShapeGroupingGroup.Text = "Group";
            this.menuItemShapeGroupingGroup.Click += new System.EventHandler(this.menuItemShapeGroupingGroup_Click);
            // 
            // menuItemShapeGroupingUngroup
            // 
            this.menuItemShapeGroupingUngroup.Name = "menuItemShapeGroupingUngroup";
            this.menuItemShapeGroupingUngroup.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.G)));
            this.menuItemShapeGroupingUngroup.Size = new System.Drawing.Size(184, 22);
            this.menuItemShapeGroupingUngroup.Text = "Ungroup";
            this.menuItemShapeGroupingUngroup.Click += new System.EventHandler(this.menuItemShapeGroupingUngroup_Click);
            // 
            // menuItemShapeGroupingCreateSubGraph
            // 
            this.menuItemShapeGroupingCreateSubGraph.Name = "menuItemShapeGroupingCreateSubGraph";
            this.menuItemShapeGroupingCreateSubGraph.Size = new System.Drawing.Size(184, 22);
            this.menuItemShapeGroupingCreateSubGraph.Text = "Create SubGraph";
            this.menuItemShapeGroupingCreateSubGraph.Click += new System.EventHandler(this.menuItemShapeGroupingCreateSubGraph_Click);
            // 
            // menuItemShapeGroupingUngroupSubGraph
            // 
            this.menuItemShapeGroupingUngroupSubGraph.Name = "menuItemShapeGroupingUngroupSubGraph";
            this.menuItemShapeGroupingUngroupSubGraph.Size = new System.Drawing.Size(184, 22);
            this.menuItemShapeGroupingUngroupSubGraph.Text = "Ungroup SubGraph";
            this.menuItemShapeGroupingUngroupSubGraph.Click += new System.EventHandler(this.menuItemShapeGroupingUngroup_Click);
            // 
            // menuItemFormat
            // 
            this.menuItemFormat.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemFormatFill,
            this.menuItemFormatText,
            this.menuItemFormatLine,
            this.menuItemFormatCornerRounding});
            //this.menuItemFormatCustomProperties
            this.menuItemFormat.MergeAction = System.Windows.Forms.MergeAction.Remove;
            this.menuItemFormat.MergeIndex = 4;
            this.menuItemFormat.Name = "menuItemFormat";
            this.menuItemFormat.Size = new System.Drawing.Size(53, 20);
            this.menuItemFormat.Text = "&Format";
            // 
            // menuItemFormatFill
            // 
            this.menuItemFormatFill.Name = "menuItemFormatFill";
            this.menuItemFormatFill.Size = new System.Drawing.Size(167, 22);
            this.menuItemFormatFill.Text = "Fill";
            this.menuItemFormatFill.Click += new System.EventHandler(this.menuItemFormatFill_Click);
            // 
            // menuItemFormatText
            // 
            this.menuItemFormatText.Name = "menuItemFormatText";
            this.menuItemFormatText.Size = new System.Drawing.Size(167, 22);
            this.menuItemFormatText.Text = "Text";
            this.menuItemFormatText.Click += new System.EventHandler(this.menuItemFormatText_Click);
            // 
            // menuItemFormatLine
            // 
            this.menuItemFormatLine.Name = "menuItemFormatLine";
            this.menuItemFormatLine.Size = new System.Drawing.Size(167, 22);
            this.menuItemFormatLine.Text = "Line";
            this.menuItemFormatLine.Visible = false;
            this.menuItemFormatLine.Click += new System.EventHandler(this.menuItemFormatLine_Click);
            // 
            // menuItemFormatCornerRounding
            // 
            this.menuItemFormatCornerRounding.Name = "menuItemFormatCornerRounding";
            this.menuItemFormatCornerRounding.Size = new System.Drawing.Size(167, 22);
            this.menuItemFormatCornerRounding.Text = "Corner Rounding";
            this.menuItemFormatCornerRounding.Visible = false;
            this.menuItemFormatCornerRounding.Click += new System.EventHandler(this.menuItemFormatCornerRounding_Click);
            // 
            // menuItemFormatCustomProperties
            // 
            this.menuItemFormatCustomProperties.Name = "menuItemFormatCustomProperties";
            this.menuItemFormatCustomProperties.Size = new System.Drawing.Size(167, 22);
            this.menuItemFormatCustomProperties.Text = "Custom Properties";
            this.menuItemFormatCustomProperties.Click += new System.EventHandler(this.menuItemFormatCustomProperties_Click);
            // 
            // menuItemTools
            // 
            this.menuItemTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemToolsFindDifferences,
            this.menuItemToolsFindIntersections,
            this.menuItemToolsMergeDocuments,
            this.menuItemToolsGridsAndRulers,
            this.menuItemToolsSnapAndGlue,
            this.menuItemToolsSpelling,
            this.menuItemToolsValidateDocument,
            //this.menuItemToolsAutoRelinkAll,
            this.menuItemToolsPortMover,
            this.menuItemToolsPortFormatting,
            this.menuItemToolsConvertCommentsToRationales,
            this.menuItemAdvanced,
            //this.menuItemToolsPlugins,
            this.menuItemToolsHeatMap,
            this.menuItemToolsActivityReport,
            this.cropToolStripMenuItem,
            this.validateModelToolStripMenuItem});
            this.menuItemTools.MergeIndex = 6;
            this.menuItemTools.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.menuItemTools.Name = "menuItemTools";
            this.menuItemTools.Text = "&Tools";
            // 
            // menuItemToolsFindDifferences
            // 
            this.menuItemToolsFindDifferences.Enabled = false;
            this.menuItemToolsFindDifferences.Name = "menuItemToolsFindDifferences";
            this.menuItemToolsFindDifferences.Size = new System.Drawing.Size(171, 22);
            this.menuItemToolsFindDifferences.Text = "Find Differences";
            this.menuItemToolsFindDifferences.Visible = false;
            // 
            // menuItemToolsFindIntersections
            // 
            this.menuItemToolsFindIntersections.Enabled = false;
            this.menuItemToolsFindIntersections.Name = "menuItemToolsFindIntersections";
            this.menuItemToolsFindIntersections.Size = new System.Drawing.Size(171, 22);
            this.menuItemToolsFindIntersections.Text = "Find Intersections";
            this.menuItemToolsFindIntersections.Visible = false;
            // 
            // menuItemToolsMergeDocuments
            // 
            this.menuItemToolsMergeDocuments.Enabled = false;
            this.menuItemToolsMergeDocuments.Name = "menuItemToolsMergeDocuments";
            this.menuItemToolsMergeDocuments.Size = new System.Drawing.Size(171, 22);
            this.menuItemToolsMergeDocuments.Text = "Merge Documents";
            this.menuItemToolsMergeDocuments.Visible = false;
            // 
            // menuItemToolsGridsAndRulers
            // 
            this.menuItemToolsGridsAndRulers.Enabled = false;
            this.menuItemToolsGridsAndRulers.Name = "menuItemToolsGridsAndRulers";
            this.menuItemToolsGridsAndRulers.Size = new System.Drawing.Size(171, 22);
            this.menuItemToolsGridsAndRulers.Text = "Grids & Rulers";
            this.menuItemToolsGridsAndRulers.Visible = false;
            // 
            // menuItemToolsSnapAndGlue
            // 
            this.menuItemToolsSnapAndGlue.Enabled = false;
            this.menuItemToolsSnapAndGlue.Name = "menuItemToolsSnapAndGlue";
            this.menuItemToolsSnapAndGlue.Size = new System.Drawing.Size(171, 22);
            this.menuItemToolsSnapAndGlue.Text = "Snap and Glue";
            this.menuItemToolsSnapAndGlue.Visible = false;
            // 
            // menuItemToolsSpelling
            // 
            this.menuItemToolsSpelling.Enabled = false;
            this.menuItemToolsSpelling.Name = "menuItemToolsSpelling";
            this.menuItemToolsSpelling.Size = new System.Drawing.Size(171, 22);
            this.menuItemToolsSpelling.Text = "Spelling";
            this.menuItemToolsSpelling.Visible = false;
            // 
            // menuItemToolsValidateDocument
            // 
            this.menuItemToolsValidateDocument.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemToolsValidateDocumentNodeConnections});
            this.menuItemToolsValidateDocument.Name = "menuItemToolsValidateDocument";
            this.menuItemToolsValidateDocument.Size = new System.Drawing.Size(171, 22);
            this.menuItemToolsValidateDocument.Text = "Validate Document";
            this.menuItemToolsValidateDocument.Visible = false;
            // 
            // menuItemToolsValidateDocumentNodeConnections
            // 
            this.menuItemToolsValidateDocumentNodeConnections.Name = "menuItemToolsValidateDocumentNodeConnections";
            this.menuItemToolsValidateDocumentNodeConnections.Size = new System.Drawing.Size(161, 22);
            this.menuItemToolsValidateDocumentNodeConnections.Text = "Node Connections";
            // 
            // menuItemToolsAutoRelinkAll
            // 
            this.menuItemToolsAutoRelinkAll.Name = "menuItemToolsAutoRelinkAll";
            this.menuItemToolsAutoRelinkAll.Size = new System.Drawing.Size(171, 22);
            this.menuItemToolsAutoRelinkAll.Text = "Auto-Relink All";
            this.menuItemToolsAutoRelinkAll.Click += new System.EventHandler(this.menuItemToolsAutoRelinkAll_Click);
            this.menuItemToolsAutoRelinkAll.Visible = false;
            // 
            // menuItemToolsPortMover
            // 
            this.menuItemToolsPortMover.Name = "menuItemToolsPortMover";
            this.menuItemToolsPortMover.Size = new System.Drawing.Size(171, 22);
            this.menuItemToolsPortMover.Text = "Port Mover";
            this.menuItemToolsPortMover.Visible = false;
            this.menuItemToolsPortMover.Click += new System.EventHandler(this.menuItemToolsPortMover_Click);
            // 
            // menuItemToolsPortFormatting
            // 
            this.menuItemToolsPortFormatting.Name = "menuItemToolsPortFormatting";
            this.menuItemToolsPortFormatting.Size = new System.Drawing.Size(171, 22);
            this.menuItemToolsPortFormatting.Text = "Port Formatting";
            this.menuItemToolsPortFormatting.Visible = false;
            this.menuItemToolsPortFormatting.Click += new System.EventHandler(this.menuItemToolsPortFormatting_Click);
            // 
            // menuItemToolsConvertCommentsToRationales
            // 
            this.menuItemToolsConvertCommentsToRationales.Name = "menuItemToolsConvertCommentsToRationales";
            this.menuItemToolsConvertCommentsToRationales.Size = new System.Drawing.Size(171, 22);
            this.menuItemToolsConvertCommentsToRationales.MergeIndex = 1;
            this.menuItemToolsConvertCommentsToRationales.MergeAction = MergeAction.Insert;
            this.menuItemToolsConvertCommentsToRationales.Text = "Convert All Comments to Rationales";
            this.menuItemToolsConvertCommentsToRationales.Visible = true;
            this.menuItemToolsConvertCommentsToRationales.Click += new System.EventHandler(this.menuItemToolsConvertCommentsToRationales_Click);
            // 
            // menuItemAdvanced
            // 
            this.menuItemAdvanced.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemToolsDebugToXML});
            this.menuItemAdvanced.Name = "menuItemAdvanced";
            this.menuItemAdvanced.Size = new System.Drawing.Size(171, 22);
            this.menuItemAdvanced.Text = "Advanced";
            this.menuItemAdvanced.Visible = false;
            // 
            // menuItemToolsDebugToXML
            // 
            this.menuItemToolsDebugToXML.Name = "menuItemToolsDebugToXML";
            this.menuItemToolsDebugToXML.Size = new System.Drawing.Size(173, 22);
            this.menuItemToolsDebugToXML.Text = "Debug Shape to XML";
            this.menuItemToolsDebugToXML.Click += new System.EventHandler(this.menuItemToolsDebugToXML_Click);
            // 
            // menuItemToolsPlugins
            // 
            this.menuItemToolsPlugins.Name = "menuItemToolsPlugins";
            this.menuItemToolsPlugins.Size = new System.Drawing.Size(171, 22);
            this.menuItemToolsPlugins.Text = "Plugins / Addins";
            // 
            // cropToolStripMenuItem
            // 
            this.cropToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemToolsCropToSheetToolStripMenuItem,
            this.menuItemToolsCropToDocumentFrameToolStripMenuItem,
            this.menuItemToolsCropDocument});
            this.cropToolStripMenuItem.Name = "cropToolStripMenuItem";
            this.cropToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.cropToolStripMenuItem.Text = "Crop";
            // 
            // menuItemToolsCropToSheetToolStripMenuItem
            // 
            this.menuItemToolsCropToSheetToolStripMenuItem.Name = "menuItemToolsCropToSheetToolStripMenuItem";
            this.menuItemToolsCropToSheetToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.menuItemToolsCropToSheetToolStripMenuItem.Text = "To Sheet";
            this.menuItemToolsCropToSheetToolStripMenuItem.Click += new System.EventHandler(this.menuItemToolsCropToSheetToolStripMenuItem_Click);
            // 
            // menuItemToolsCropToDocumentFrameToolStripMenuItem
            // 
            this.menuItemToolsCropToDocumentFrameToolStripMenuItem.Name = "menuItemToolsCropToDocumentFrameToolStripMenuItem";
            this.menuItemToolsCropToDocumentFrameToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.menuItemToolsCropToDocumentFrameToolStripMenuItem.Text = "To Document Frame";
            this.menuItemToolsCropToDocumentFrameToolStripMenuItem.Click += new System.EventHandler(this.menuItemToolsCropToDocumentFrameToolStripMenuItem_Click);
            // 
            // menuItemToolsCropDocument
            // 
            this.menuItemToolsCropDocument.Name = "menuItemToolsCropDocument";
            this.menuItemToolsCropDocument.Size = new System.Drawing.Size(170, 22);
            this.menuItemToolsCropDocument.Text = "To Document";
            this.menuItemToolsCropDocument.Click += new System.EventHandler(menuItemToolsCropDocument_Click);
            // 
            // validateModelToolStripMenuItem
            // 
            this.validateModelToolStripMenuItem.Name = "validateModelToolStripMenuItem";
            this.validateModelToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.validateModelToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.validateModelToolStripMenuItemADD});
            //this.validateModelToolStripMenuItemFSD,
            //this.validateModelToolStripMenuItemDSD});
            this.validateModelToolStripMenuItem.Text = "Validate Diagram";
            //this.validateModelToolStripMenuItem.Click += new System.EventHandler(this.validateModelToolStripMenuItemNEW_Click);
            // 
            // validateModelToolStripMenuItemADD
            // 
            this.validateModelToolStripMenuItemADD.Name = "validateModelToolStripMenuItemADD";
            this.validateModelToolStripMenuItemADD.Size = new System.Drawing.Size(171, 22);
            this.validateModelToolStripMenuItemADD.Text = "ADD";
            this.validateModelToolStripMenuItemADD.Click += new System.EventHandler(this.validateModelToolStripMenuItem_Click);
            // 
            // menuItemToolsHeatMapGapType
            // 
            this.menuItemToolsHeatMapGapType.Name = "menuItemToolsHeatMapGapType";
            this.menuItemToolsHeatMapGapType.Size = new System.Drawing.Size(170, 22);
            this.menuItemToolsHeatMapGapType.Text = "Gap Type";
            this.menuItemToolsHeatMapGapType.Click += new System.EventHandler(menuItemToolsHeatMapGapType_Click);
            // 
            // menuItemToolsHeatMapMeasure
            // 
            this.menuItemToolsHeatMapMeasure.Name = "menuItemToolsHeatMapMeasure";
            this.menuItemToolsHeatMapMeasure.Size = new System.Drawing.Size(170, 22);
            this.menuItemToolsHeatMapMeasure.Text = "Measure Type";
            this.menuItemToolsHeatMapMeasure.Click += new System.EventHandler(menuItemToolsHeatMapMeasure_Click);
            // 
            // menuItemToolsHeatMap
            // 
            this.menuItemToolsHeatMap.Name = "menuItemToolsHeatMap";
            this.menuItemToolsHeatMap.Size = new System.Drawing.Size(170, 22);
            this.menuItemToolsHeatMap.Text = "Heat Map";
            this.menuItemToolsHeatMap.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemToolsHeatMapGapType,
            this.menuItemToolsHeatMapMeasure});
            //
            //menuItemToolsActivityReport
            //
            this.menuItemToolsActivityReport.Name = "menuItemToolsActivityReport";
            this.menuItemToolsActivityReport.Size = new System.Drawing.Size(170, 22);
            this.menuItemToolsActivityReport.Text = "Accountability Matrix";
            this.menuItemToolsActivityReport.Click += new System.EventHandler(ShowActivityReport);
            // 
            // contextMenuTabPage
            // 
            this.contextMenuTabPage.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.cxMenuItemSave,
            new System.Windows.Forms.MenuItem("-"),
            this.cxMenuItemClose,
            this.cxMenuItemCloseAll,
            this.cxMenuItemCloseAllButThis,
            new System.Windows.Forms.MenuItem("-"),
            this.cxMenuItemOpenPath});
            this.contextMenuTabPage.Name = "contextMenuTabPage";
            //this.contextMenuTabPage.Size = new System.Drawing.Size(156, 70);
            // 
            // cxMenuItemClose
            // 
            this.cxMenuItemClose.Name = "cxMenuItemClose";
            //this.cxMenuItemClose.Size = new System.Drawing.Size(155, 22);
            this.cxMenuItemClose.Text = "&Close";
            this.cxMenuItemClose.Click += new System.EventHandler(this.menuItemFileClose_Click);
            // 
            // cxMenuItemCloseAll
            // 
            this.cxMenuItemCloseAll.Name = "cxMenuItemCloseAll";
            //this.cxMenuItemCloseAll.Size = new System.Drawing.Size(155, 22);
            this.cxMenuItemCloseAll.Text = "Close &All";
            this.cxMenuItemCloseAll.Click += new System.EventHandler(this.menuItemFileCloseAll_Click);
            // 
            // cxMenuItemCloseAllButThis
            // 
            this.cxMenuItemCloseAllButThis.Name = "cxMenuItemCloseAllButThis";
            //this.cxMenuItemCloseAllButThis.Size = new System.Drawing.Size(155, 22);
            this.cxMenuItemCloseAllButThis.Text = "Close All &But This";
            this.cxMenuItemCloseAllButThis.Click += new System.EventHandler(this.cxMenuItemCloseAllButThis_Click);
            // 
            // cxMenuItemOpenPath
            // 
            this.cxMenuItemOpenPath.Name = "cxMenuItemOpenPath";
            //this.cxMenuItemCloseAllButThis.Size = new System.Drawing.Size(155, 22);
            this.cxMenuItemOpenPath.Text = "Open Containing &Folder";
            this.cxMenuItemOpenPath.Click += new System.EventHandler(this.cxMenuItemOpenPath_Click);
            // 
            // cxMenuItemSave
            // 
            this.cxMenuItemSave.Name = "cxMenuItemSave";
            this.cxMenuItemSave.Text = "&Save diagram";
            this.cxMenuItemSave.Click += new System.EventHandler(this.menuItemFileSave_Click);
            // 
            // toolStripMain
            // 
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stripButtonSave,
            this.stripButtonPrint,
            new ToolStripSeparator(),
            this.stripButtonCut,
            this.stripButtonCopy,
            this.stripButtonPaste,
            this.toolStripZoomLabel,
            new ToolStripSeparator(),
            this.toolStripFormatText,
            this.toolStripFormatFill,
            this.toolstripCopyFormatting});
            //this.toolStripLabel1
            //,this.toolstripComboZoom}
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Dock = DockStyle.Top;
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(797, 25);
            this.toolStripMain.TabIndex = 7;
            this.toolStripMain.Text = "toolStripMain";
            this.toolStripMain.Visible = false;
            // 
            // stripButtonSave
            // 
            this.stripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("stripButtonSave.Image")));
            this.stripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripButtonSave.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.stripButtonSave.MergeIndex = 3;
            this.stripButtonSave.Name = "stripButtonSave";
            this.stripButtonSave.Size = new System.Drawing.Size(23, 22);
            this.stripButtonSave.Text = "&Save";
            this.stripButtonSave.Click += new System.EventHandler(this.menuItemFileSave_Click);
            // 
            // stripButtonPrint
            // 
            this.stripButtonPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripButtonPrint.Image = ((System.Drawing.Image)(resources.GetObject("stripButtonPrint.Image")));
            this.stripButtonPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripButtonPrint.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.stripButtonPrint.MergeIndex = 4;
            this.stripButtonPrint.Name = "stripButtonPrint";
            this.stripButtonPrint.Size = new System.Drawing.Size(23, 22);
            this.stripButtonPrint.Text = "&Print";
            this.stripButtonPrint.Click += new System.EventHandler(this.menuItemFilePrint_Click);
            // 
            // stripButtonCut
            // 
            this.stripButtonCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripButtonCut.Image = ((System.Drawing.Image)(resources.GetObject("stripButtonCut.Image")));
            this.stripButtonCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripButtonCut.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.stripButtonCut.MergeIndex = 6;
            this.stripButtonCut.Name = "stripButtonCut";
            this.stripButtonCut.Size = new System.Drawing.Size(23, 22);
            this.stripButtonCut.Text = "C&ut";
            this.stripButtonCut.Click += new System.EventHandler(this.menuItemEditCut_Click);
            // 
            // stripButtonCopy
            // 
            this.stripButtonCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripButtonCopy.Image = ((System.Drawing.Image)(resources.GetObject("stripButtonCopy.Image")));
            this.stripButtonCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripButtonCopy.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.stripButtonCopy.MergeIndex = 7;
            this.stripButtonCopy.Name = "stripButtonCopy";
            this.stripButtonCopy.Size = new System.Drawing.Size(23, 22);
            this.stripButtonCopy.Text = "&Copy";
            this.stripButtonCopy.Click += new System.EventHandler(this.menuItemEditCopy_Click);
            // 
            // stripButtonPaste
            // 
            this.stripButtonPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stripButtonPaste.Image = ((System.Drawing.Image)(resources.GetObject("stripButtonPaste.Image")));
            this.stripButtonPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripButtonPaste.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.stripButtonPaste.MergeIndex = 8;
            this.stripButtonPaste.Name = "stripButtonPaste";
            this.stripButtonPaste.Size = new System.Drawing.Size(23, 22);
            this.stripButtonPaste.Text = "&Paste";
            this.stripButtonPaste.Click += new System.EventHandler(this.menuItemEditPaste_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(40, 22);
            this.toolStripLabel1.Text = "Zoom: ";
            this.toolStripLabel1.Visible = false;
            //
            //toolStripZoomLabel - 9 Januray 2013
            //
            this.toolStripZoomLabel.Name = "toolStripZoomLabel";
            this.toolStripZoomLabel.Visible = true;
            // 
            // toolstripComboZoom
            // 
            //this.toolstripComboZoom.Items.AddRange(new object[] {
            //"1 %",
            //"5 %",
            //"25 %",
            //"50 %",
            //"75 %",
            //"100 %",
            //"125 %",
            //"150 %",
            //"200 %",
            //"300 %",
            //"400 %"});
            //this.toolstripComboZoom.Name = "toolstripComboZoom";
            //this.toolstripComboZoom.Size = new System.Drawing.Size(75, 25);
            //this.toolstripComboZoom.Text = "100 %";
            //this.toolstripComboZoom.TextUpdate += new System.EventHandler(this.toolstripComboZoom_TextUpdate);
            //this.toolstripComboZoom.Leave += new System.EventHandler(this.toolstripComboZoom_Leave);
            //this.toolstripComboZoom.DropDownClosed += new System.EventHandler(this.toolstripComboZoom_DropDownClosed);
            // 
            // goLayoutLayeredDigraph1
            // 
            this.goLayoutLayeredDigraph1.Document = null;
            this.goLayoutLayeredDigraph1.Network = null;
            this.goLayoutLayeredDigraph1.View = null;
            // 
            // myView
            // 
            this.myView.ArrowMoveLarge = 20F;
            this.myView.ArrowMoveSmall = 1F;
            this.myView.BackColor = System.Drawing.Color.White;
            this.myView.BackgroundHasSheet = true;
            this.myView.Border3DStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.myView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            goRulerCursor9.BackColor = System.Drawing.Color.White;
            goRulerCursor9.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor9.Height = 22;
            goRulerCursor9.Size = new System.Drawing.Size(1, 22);
            goRulerCursor9.Value = 0;
            goRulerCursor9.Width = 1;
            this.myView.BottomObjectCursor = goRulerCursor9;
            this.myView.BoundingHandlePenWidth = 2.857143F;
            this.myView.BusySelecting = false;
            this.myView.ContextClickSingleSelection = false;
            this.myView.DisableKeys = Northwoods.Go.GoViewDisableKeys.None;
            this.myView.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.myView.DocScale = 0.7F;
            this.myView.DragRoutesRealtime = true;
            this.myView.DragsRealtime = true;
            this.myView.ExternalDragDropsOnEnter = false;
            this.myView.GridCellSizeHeight = 10F;
            this.myView.GridCellSizeWidth = 10F;
            this.myView.GridLineColor = System.Drawing.Color.Silver;
            this.myView.GridLineDashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.myView.GridLineWidth = 1F;
            this.myView.GridMajorLineColor = System.Drawing.Color.Silver;
            this.myView.GridMajorLineFrequency = new System.Drawing.Size(2, 2);
            this.myView.GridMajorLineWidth = 1F;
            this.myView.GridSnapDrag = Northwoods.Go.GoViewSnapStyle.Jump;
            this.myView.GridStyle = Northwoods.Go.GoViewGridStyle.Line;
            this.myView.GridUnboundedSpots = 0;
            goRulerCursor10.BackColor = System.Drawing.Color.White;
            goRulerCursor10.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor10.Height = 22;
            goRulerCursor10.Size = new System.Drawing.Size(1, 22);
            goRulerCursor10.Value = 0;
            goRulerCursor10.Width = 1;
            this.myView.HorizontalCenterObjectCursor = goRulerCursor10;
            goRulerCursor11.BackColor = System.Drawing.Color.White;
            goRulerCursor11.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor11.Height = 22;
            goRulerCursor11.Size = new System.Drawing.Size(1, 22);
            goRulerCursor11.Value = 0;
            goRulerCursor11.Width = 1;
            this.myView.HorizontalRulerMouseCursor = goRulerCursor11;
            this.myView.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            goRulerCursor12.BackColor = System.Drawing.Color.White;
            goRulerCursor12.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor12.Height = 22;
            goRulerCursor12.Size = new System.Drawing.Size(1, 22);
            goRulerCursor12.Value = 0;
            goRulerCursor12.Width = 1;
            this.myView.LeftObjectCursor = goRulerCursor12;
            this.myView.Location = new System.Drawing.Point(0, 0);
            this.myView.Name = "myView";
            this.myView.NoFocusSelectionColor = System.Drawing.Color.Chartreuse;
            this.myView.PortGravity = 50F;
            this.myView.PrimarySelectionColor = System.Drawing.Color.Red;
            this.myView.ResizeHandleHeight = 8.571428F;
            this.myView.ResizeHandlePenWidth = 1.428571F;
            this.myView.ResizeHandleWidth = 8.571428F;
            goRulerCursor13.BackColor = System.Drawing.Color.White;
            goRulerCursor13.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor13.Height = 22;
            goRulerCursor13.Size = new System.Drawing.Size(1, 22);
            goRulerCursor13.Value = 0;
            goRulerCursor13.Width = 1;
            this.myView.RightObjectCursor = goRulerCursor13;
            this.myView.SecondarySelectionColor = System.Drawing.Color.Blue;
            this.myView.SheetStyle = Northwoods.Go.GoViewSheetStyle.SheetWidth;
            this.myView.ShowFrame = false;
            this.myView.ShowsNegativeCoordinates = false;
            this.myView.Size = new System.Drawing.Size(797, 519);
            this.myView.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            this.myView.TabIndex = 4;
            this.myView.Text = "graphView1";
            this.myView.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            goRulerCursor14.BackColor = System.Drawing.Color.White;
            goRulerCursor14.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor14.Height = 22;
            goRulerCursor14.Size = new System.Drawing.Size(1, 22);
            goRulerCursor14.Value = 0;
            goRulerCursor14.Width = 1;
            this.myView.TopObjectCursor = goRulerCursor14;
            goRulerCursor15.BackColor = System.Drawing.Color.White;
            goRulerCursor15.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor15.Height = 22;
            goRulerCursor15.Size = new System.Drawing.Size(1, 22);
            goRulerCursor15.Value = 0;
            goRulerCursor15.Width = 1;
            this.myView.VerticalCenterObjectCursor = goRulerCursor15;
            goRulerCursor16.BackColor = System.Drawing.Color.White;
            goRulerCursor16.ForeColor = System.Drawing.Color.Blue;
            goRulerCursor16.Height = 22;
            goRulerCursor16.Size = new System.Drawing.Size(1, 22);
            goRulerCursor16.Value = 0;
            goRulerCursor16.Width = 1;
            this.myView.VerticalRulerMouseCursor = goRulerCursor16;
            this.myView.ViewController = null;
            this.myView.Click += new System.EventHandler(this.myView_Click);
            this.myView.NodeObjectContextClicked += new MetaBuilder.Graphing.Controllers.NodeObjectContextClickedEventHandler(this.myView_NodeObjectContextClicked);
            this.myView.NodeObjectContextClickedShallow += new MetaBuilder.Graphing.Controllers.NodeObjectContextClickedEventHandler(this.myView_NodeObjectContextClickedShallow);
            this.myView.ItemsPasted += new System.EventHandler(this.myView_ItemsPasted);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.Diagram = null;
            this.backgroundWorker1.FileName = null;
            this.backgroundWorker1.View = null;
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            // 
            // toolStrip1
            // 
            //this.toolStripFormat.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            //this.toolStripFormatText,
            //this.toolStripFormatFill,
            //this.toolstripCopyFormatting});
            this.toolStripFormat.Dock = DockStyle.Top;
            this.toolStripFormat.Location = new System.Drawing.Point(0, 0);
            this.toolStripFormat.Name = "toolStrip1";
            this.toolStripFormat.Size = new System.Drawing.Size(797, 25);
            this.toolStripFormat.TabIndex = 8;
            this.toolStripFormat.Text = "toolStrip1";
            this.toolStripFormat.Dock = DockStyle.Top;
            // 
            // toolStripFormatText
            // 
            this.toolStripFormatText.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripFormatText.Enabled = false;
            this.toolStripFormatText.Image = ((System.Drawing.Image)(resources.GetObject("toolStripFormatText.Image")));
            this.toolStripFormatText.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripFormatText.Name = "toolStripFormatText";
            this.toolStripFormatText.Size = new System.Drawing.Size(23, 22);
            this.toolStripFormatText.Text = "Format Text";
            this.toolStripFormatText.Click += new System.EventHandler(this.toolStripFormatText_Click);
            // 
            // toolStripFormatFill
            // 
            this.toolStripFormatFill.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripFormatFill.Enabled = false;
            this.toolStripFormatFill.Image = ((System.Drawing.Image)(resources.GetObject("toolStripFormatFill.Image")));
            this.toolStripFormatFill.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripFormatFill.Name = "toolStripFormatFill";
            this.toolStripFormatFill.Size = new System.Drawing.Size(23, 22);
            this.toolStripFormatFill.Text = "toolStripButton1";
            this.toolStripFormatFill.ToolTipText = "Edit Fill";
            this.toolStripFormatFill.Click += new System.EventHandler(this.toolStripFormatFill_Click);
            // 
            // toolstripCopyFormatting
            // 
            this.toolstripCopyFormatting.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolstripCopyFormatting.Enabled = false;
            this.toolstripCopyFormatting.Image = ((System.Drawing.Image)(resources.GetObject("toolstripCopyFormatting.Image")));
            this.toolstripCopyFormatting.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolstripCopyFormatting.Name = "toolstripCopyFormatting";
            this.toolstripCopyFormatting.Size = new System.Drawing.Size(23, 22);
            toolstripCopyFormatting.DoubleClickEnabled = true;
            //this.toolstripCopyFormatting.ToolTipText = "Copy Formatting (Right-click to apply formatting on multiple selections, then right-click to stop formatting)";
            //this.toolstripCopyFormatting.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolstripCopyFormatting_MouseUp);
            this.toolstripCopyFormatting.DoubleClick += new System.EventHandler(this.toolstripCopyFormatting_DoubleClick);
            this.toolstripCopyFormatting.Click += new System.EventHandler(this.toolstripCopyFormatting_Click);
            // 
            // GraphViewContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 519);
            this.Controls.Add(this.toolStripMain);
            this.Controls.Add(this.myView);
            this.Controls.Add(this.mainMenu);
            //this.Controls.Add(this.toolStripFormat);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MainMenuStrip = this.mainMenu;
            this.Name = "GraphViewContainer";
            this.TabText = "GraphViewContainer";
            this.Text = "GraphViewContainer";
            this.Load += new System.EventHandler(this.GraphViewContainer_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GraphViewContainer_FormClosed);
            this.Leave += new System.EventHandler(this.GraphViewContainer_Leave);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GraphViewContainer_FormClosing);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            //this.contextMenuTabPage.ResumeLayout(false);
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            //this.toolStripFormat.ResumeLayout(false);
            //this.toolStripFormat.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem menuItem1;
        private System.Windows.Forms.ContextMenu contextMenuTabPage;
        private System.Windows.Forms.MenuItem cxMenuItemClose;
        private System.Windows.Forms.MenuItem cxMenuItemCloseAllButThis;
        private System.Windows.Forms.MenuItem cxMenuItemCloseAll;
        private System.Windows.Forms.MenuItem cxMenuItemOpenPath;
        private System.Windows.Forms.MenuItem cxMenuItemSave;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripMenuItem menuItemFileClose;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItemView;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewGrid;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewZoom;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewZoom50;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewZoom100;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewZoom150;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewZoom200;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewZoom400;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewZoomPageWidth;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewZoomWholePage;
        private System.Windows.Forms.ToolStripMenuItem menuItemFileCloseAll;
        private System.Windows.Forms.ToolStripMenuItem menuItemInsert;
        private System.Windows.Forms.ToolStripMenuItem menuItemInsertImage;
        private System.Windows.Forms.ToolStripMenuItem menuItemInsertPort;
        private System.Windows.Forms.ToolStripMenuItem menuItemInsertRepeaterSection;
        private System.Windows.Forms.ToolStripMenuItem menuItemShape;
        private System.Windows.Forms.ToolStripMenuItem menuItemShapeAlign;
        private System.Windows.Forms.ToolStripMenuItem menuItemShapeAlignTops;
        private System.Windows.Forms.ToolStripMenuItem menuItemShapeAlignBottoms;
        private System.Windows.Forms.ToolStripMenuItem menuItemEditPasteShallowCopy;
        private System.Windows.Forms.ToolStripMenuItem menuItemShapeAlignLeftSides;
        private System.Windows.Forms.ToolStripMenuItem menuItemShapeAlignRightSides;
        private System.Windows.Forms.ToolStripMenuItem menuItemShapeAlignHorizontalCenters;
        private System.Windows.Forms.ToolStripMenuItem menuItemShapeAlignVerticalCenters;
        private System.Windows.Forms.ToolStripMenuItem menuItemShapeLayoutShapes;
        private System.Windows.Forms.ToolStripMenuItem menuItemShapeLayoutShapesDigraph;
        private System.Windows.Forms.ToolStripMenuItem menuItemShapeLayoutShapesForceDirected;
        private System.Windows.Forms.ToolStripMenuItem menuItemShapeOrder;
        private System.Windows.Forms.ToolStripMenuItem menuItemShapeOrderBringForward;
        private System.Windows.Forms.ToolStripMenuItem menuItemShapeOrderBringToFront;
        private System.Windows.Forms.ToolStripMenuItem menuItemShapeOrderSendBackward;
        private System.Windows.Forms.ToolStripMenuItem menuItemShapeOrderSendToBack;
        private System.Windows.Forms.ToolStripMenuItem menuItemFormat;
        private System.Windows.Forms.ToolStripMenuItem menuItemFormatText;
        private System.Windows.Forms.ToolStripMenuItem menuItemFormatLine;
        private System.Windows.Forms.ToolStripMenuItem menuItemFormatFill;
        private System.Windows.Forms.ToolStripMenuItem menuItemFormatCornerRounding;
        private System.Windows.Forms.ToolStripMenuItem menuItemTools;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsFindDifferences;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsFindIntersections;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsMergeDocuments;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsGridsAndRulers;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsSnapAndGlue;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsSpelling;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsValidateDocument;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsPortMover;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsPortFormatting;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsConvertCommentsToRationales;
        private System.Windows.Forms.ToolStripMenuItem menuItemFormatCustomProperties;
        private MetaBuilder.Graphing.Containers.GraphView myView;
        private System.Windows.Forms.ToolStripMenuItem menuItemFileSave;
        private System.Windows.Forms.ToolStripMenuItem menuItemFileSaveAs;
        private System.Windows.Forms.ToolStripMenuItem menuItemFilePrint;
        private System.Windows.Forms.ToolStripMenuItem menuItemEditUndo;
        private System.Windows.Forms.ToolStripMenuItem menuItemEditRedo;
        private System.Windows.Forms.ToolStripMenuItem menuItemEditCut;
        private System.Windows.Forms.ToolStripMenuItem menuItemEditCopy;
        private System.Windows.Forms.ToolStripMenuItem menuItemEditPaste;
        private System.Windows.Forms.ToolStripMenuItem menuItemEditSelectAll;
        private System.Windows.Forms.ToolStripButton stripButtonSave;
        private System.Windows.Forms.ToolStripButton stripButtonPrint;
        private System.Windows.Forms.ToolStripButton stripButtonCut;
        private System.Windows.Forms.ToolStripButton stripButtonCopy;
        private System.Windows.Forms.ToolStripButton stripButtonPaste;
        internal System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewZoomPageHeight;
        private System.Windows.Forms.ToolStripMenuItem menuItemAdvanced;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsDebugToXML;
        private Northwoods.Go.Layout.GoLayoutLayeredDigraph goLayoutLayeredDigraph1;
        private System.Windows.Forms.ToolStripMenuItem menuItemShapeLayoutCircular;
        private System.Windows.Forms.ToolStripMenuItem menuItemFileProperties;
        private System.Windows.Forms.ToolStripMenuItem menuItemShapeLayoutTree;
        private System.Windows.Forms.ToolStripMenuItem menuItemShapeLayoutFSD;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewPorts;
        private System.Windows.Forms.ToolStripMenuItem menuItemEditEnableRubberStamping;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsAutoRelinkAll;
        private System.Windows.Forms.ToolStripMenuItem menuItemEditEnableAutoLinking;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsPlugins;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsHeatMap;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsHeatMapGapType;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsHeatMapMeasure;
        private System.Windows.Forms.ToolStripMenuItem menuItemShapeDistribute;
        private System.Windows.Forms.ToolStripMenuItem menuItemShapeDistributeHorizontally;
        private System.Windows.Forms.ToolStripMenuItem menuItemShapeVertically;
        private System.Windows.Forms.ToolStripMenuItem cropToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItemShapeGrouping;
        private System.Windows.Forms.ToolStripMenuItem menuItemShapeGroupingGroup;
        private System.Windows.Forms.ToolStripMenuItem menuItemShapeGroupingUngroup;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        //private System.Windows.Forms.ToolStripComboBox toolstripComboZoom;
        private System.Windows.Forms.ToolStripMenuItem menuItemEditFind;
        private System.Windows.Forms.ToolStripMenuItem menuItemEditFindAndReplace;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsValidateDocumentNodeConnections;
        private System.Windows.Forms.ToolStripMenuItem menuItemShapeLayoutArtefacts;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsCropToSheetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsCropToDocumentFrameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsCropDocument;
        private System.Windows.Forms.ToolStripMenuItem menuItemInsertFileAttachment;
        private System.Windows.Forms.ToolStripMenuItem menuItemShapeGroupingCreateSubGraph;
        private System.Windows.Forms.ToolStripMenuItem menuItemShapeGroupingUngroupSubGraph;
        private System.Windows.Forms.ToolStripMenuItem menuItemToolsActivityReport;

        private ToolStripMenuItem menuItemInsertShape;
        private ToolStripMenuItem menuItemInsertIntellishape;
        private ToolStripMenuItem menuItemInsertText;
        private ToolStripMenuItem menuItemInsertShapeRectangle;
        private ToolStripMenuItem menuItemInsertShapeTriangle;
        private ToolStripMenuItem menuItemInsertShapeEllipse;
        private ToolStripMenuItem menuItemInsertShapeOtherShapes;
        private ToolStripMenuItem menuItemInsertShapesCurvedPolygon;
        private ToolStripMenuItem menuItemInsertShapesLinePolygon;
        private ToolStripMenuItem menuItemInsertShapesTrapezoid;
        private ToolStripMenuItem menuItemInsertCube;
        private ToolStripMenuItem menuItemInsertCylinder;
        private ToolStripMenuItem menuItemInsertCylinderVertical;
        private ToolStripMenuItem menuItemInsertCylinderHorizontal;
        private ToolStripMenuItem menuItemInsertHexagon;
        private ToolStripMenuItem menuItemInsertParallelogram;
        private ToolStripMenuItem menuItemInsertOctagon;
        private ToolStripMenuItem menuItemInsertDiamond;
        private ToolStripMenuItem menuItemInsertHouseShape;
        private ToolStripMenuItem menuItemInsertArrow;
        private ToolStripMenuItem menuItemInsertBlockArrow;
        private ToolStripMenuItem menuItemInsertShapeStroke;
        private ToolStripMenuItem menuItemInsertLine;
        private ToolStripMenuItem menuItemInsertLineBezier;
        private ToolStripMenuItem menuItemInsertLineRounded;
        private ToolStripMenuItem menuItemInsertLineRoundedWithJumpOvers;
        private ToolStripMenuItem menuItemInsertIntellishapeSubgraphObject;
        private ToolStripMenuItem menuItemInsertIntellishapeSubgraphObjectNew;
        private ToolStripMenuItem menuItemInsertIntellishapeSubgraphObjectExisting;
        private ToolStripMenuItem menuItemInsertIntellishapeValueChainStep;
        private ToolStripMenuItem menuItemInsertIntellishapeValueChainStepNew;
        private ToolStripMenuItem menuItemInsertIntellishapeValueChainStepExisting;
        private ToolStripMenuItem menuItemInsertIntellishapeSwimlane;
        private ToolStripMenuItem menuItemInsertIntellishapeSwimlaneNew;
        private ToolStripMenuItem menuItemInsertIntellishapeSwimlaneExisting;
        private ToolStripMenuItem menuItemInsertLegend;
        private ToolStripMenuItem menuItemInsertLegendColor;
        private ToolStripMenuItem menuItemInsertLegendClass;
        private ToolStripMenuItem menuItemInsertLegendBoth;
        private ToolStripMenuItem menuItemInsertTextLabel;
        private ToolStripMenuItem menuItemInsertRichText;
        private ToolStripMenuItem menuItemInsertHyperlink;
        private ToolStripMenuItem menuItemInsertComment;
        private ToolStripMenuItem menuItemInsertBalloonComment;
        private ToolStripMenuItem menuItemInsertRationale;
        private ToolStripLabel toolStripZoomLabel;
        public DiagramSaver backgroundWorker1;
        //private ToolStripMenuItem menuItemInsertIntellishapeSwimlaneFromDB;
        private ToolStrip toolStripFormat;
        private ToolStripButton toolStripFormatFill;
        private ToolStripButton toolStripFormatText;
        private ToolStripButton toolstripCopyFormatting;
        private ToolStripMenuItem validateModelToolStripMenuItem;
        private ToolStripMenuItem validateModelToolStripMenuItemADD;

        private System.Windows.Forms.ToolStripMenuItem menuItemViewFishLink;
        private System.Windows.Forms.ToolStripMenuItem menuItemViewObjectImages;

        //Quick Format System
        //Formatting class displayed on bar based on context, when changed apply formatting
    }
}