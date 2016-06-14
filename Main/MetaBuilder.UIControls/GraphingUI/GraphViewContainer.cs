using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.Core.Storage;
using MetaBuilder.Docking;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Controllers;
using MetaBuilder.Graphing.Layout;
using MetaBuilder.Graphing.Persistence;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Nodes;
using MetaBuilder.Graphing.Shapes.Nodes.Containers;
using MetaBuilder.Graphing.Shapes.Primitives;
using MetaBuilder.Graphing.Tools;
using MetaBuilder.Graphing.Utilities;
using MetaBuilder.Meta;
using MetaBuilder.MetaControls.Tasks;
using MetaBuilder.PluginSDK;
using MetaBuilder.SplashScreen;
using MetaBuilder.UIControls.Common;
using MetaBuilder.UIControls.Dialogs;
using MetaBuilder.UIControls.GraphingUI.Tools.Explorer;
using MetaBuilder.UIControls.GraphingUI.CustomPrinting;
using MetaBuilder.UIControls.GraphingUI.DupeChecker;
using MetaBuilder.UIControls.GraphingUI.Formatting;
using MetaBuilder.UIControls.MetaTree;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Northwoods.Go;
using Northwoods.Go.Layout;
using AssociationManager = MetaBuilder.BusinessFacade.MetaHelper.AssociationManager;
using Timer = System.Windows.Forms.Timer;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
using System.Collections;
using System.Reflection;
using System.Data;
using System.Collections.ObjectModel;

namespace MetaBuilder.UIControls.GraphingUI
{
    public partial class GraphViewContainer : DockContent
    {

        #region Fields
        public bool DisableMetaPropertiesOnSelection;
        public bool NoPrompt;
        //7 January 2013
        public bool ExternalObjectDropped;

        public event EventHandler SaveComplete;

        private bool isSavingFromMDIParent;
        private bool isSavingFromItself;
        private Timer timerAutoSave;
        private Dictionary<ValidClassAssociationKey, List<ClassAssociation>> allowedLinks;
        private Dictionary<IMetaNode, Checker> duplicateCheckers;
        private Guid containerID;
        private PluginCollection m_plugins = null;
        //private GraphViewController viewController = null;
        //private Collection<IViewObserver> observers;

        #endregion Fields

        #region Properties

        private bool forceSaveAs;
        public bool ForceSaveAs
        {
            get { return forceSaveAs; }
            set
            {
                forceSaveAs = value;
            }
        }

        private bool readoOnly;
        public bool ReadOnly
        {
            get { return readoOnly; }
            set
            {
                readoOnly = value;
                if (readoOnly == true)
                {
                    ViewController.ReadOnly = readoOnly;

                    if (ReadOnly)
                        if (!DockingForm.DockForm.toolStripDropDownSelectWorkspace.Text.Contains(" (Read Only) "))
                            DockingForm.DockForm.toolStripDropDownSelectWorkspace.Text += " (Read Only) ";
                    ForceDatabaseObjectUpdateForStatus();
                    //9 October 2013 - after check out and close diagram was already saved and has not been changed but still 'pretends' to be modified
                    //ViewController.SetDocumentModifiedToFalseIfNotTrue(false);
                    //View.Document.IsModified = false;
                    if (myView.Document.UndoManager != null)
                    {
                        myView.Document.UndoManager.Clear();
                        myView.Document.UndoManager.RemoveDocument(myView.Document);
                        myView.Document.UndoManager = null;
                    }
                }
            }
        }

        private bool openingFromServer;
        public bool OpeningFromServer
        {
            get { return openingFromServer; }
            set
            {
                openingFromServer = value;
                ignoreSkipRefreshWhenOpeningAServerFile = openingFromServer;
                if (openingFromServer)
                {
                    RefreshData(myView.Document, false);
                }
            }
        }

        private bool isClosing;
        public bool IsClosing
        {
            get { return isClosing; }
            set { isClosing = value; }
        }

        public Guid ContainerID
        {
            get { return containerID; }
        }
        public GraphViewController ViewController
        {
            get
            {
                if (myView.ViewController == null)
                {
                    myView.ViewController = new GraphViewController();
                    myView.ViewController.MyView = myView;
                    myView.ViewController.ArtefactPointersVisible = menuItemViewFishLink.Checked;
                    myView.ViewController.MetaConvertComplete += new EventHandler(viewController_MetaConvertComplete);
                }
                return myView.ViewController;
            }
        }

        private void viewController_MetaConvertComplete(object sender, EventArgs e)
        {
            try
            {
                ViewController.MetaConverting = false;
                if (MetaConvert.StaticCollection != null)
                {
                    foreach (IMetaNode node in MetaConvert.StaticCollection)
                    {
                        if (duplicateCheckers != null)
                        {
                            if (duplicateCheckers.ContainsKey(node))
                            {
                                duplicateCheckers.Remove(node);
                            }
                        }
                    }
                }
                DockingForm.DockForm.GetTaskDocker().RemoveMissingGoObjects(ContainerID.ToString());
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString());
            }

            myView.Document.UndoManager.Clear();
        }

        public GraphView View
        {
            get { return ViewController.MyView; }
            set
            {
                myView = value;
                ViewController.MyView = myView;
            }
        }
        //public Collection<IViewObserver> Observers
        //{
        //    get { return observers; }
        //    set { observers = value; }
        //}
        public bool IsSavingFromMDIParent
        {
            get { return isSavingFromMDIParent; }
            set { isSavingFromMDIParent = value; }
        }
        public bool IsSavingFromItself
        {
            get { return isSavingFromItself; }
            set { isSavingFromItself = value; }
        }
        public Timer TimerAutoSave
        {
            get
            {
                if (timerAutoSave == null)
                    timerAutoSave = new Timer();
                return timerAutoSave;
            }
            set { timerAutoSave = value; }
        }
        public bool BusySelecting
        {
            get { return myView.BusySelecting; }
            set { myView.BusySelecting = value; }

        }

        #endregion Properties

        private bool customModified;
        public bool CustomModified
        {
            get
            {
                return customModified;
            }
            set
            {
                customModified = value;
                if (!Core.Variables.Instance.IsDeveloperEdition)
                    return;

                if (!value)
                {
                    //DockingForm.DockForm.statusLabel.BackColor = SystemColors.Control;
                    //this.DockHandler.Pane.BackColor = SystemColors.Control;
                    DockingForm.DockForm.statusLabel.ToolTipText = "";
                    if (TabText.Substring(TabText.Length - 2, 2) == " *")
                    {
                        TabText = TabText.Replace(" *", "");
                    }
                }
                else
                {
                    //DockingForm.DockForm.statusLabel.BackColor = Color.MistyRose;
                    //this.DockHandler.Pane.BackColor = Color.Red;
                    DockingForm.DockForm.statusLabel.ToolTipText = "Changes made, click to reset.";
                    if (TabText.Substring(TabText.Length - 2, 2) != " *")
                    {
                        TabText = TabText + " *";
                    }
                }
            }
        }
        //private FormClosingEventArgs closingArgs;

        public GraphViewContainer(FileTypeList filetype)
        {
            InitializeComponent();
            View.StartTransaction();
            containerID = Guid.NewGuid();
            try
            {
                //SuspendLayout();
                SetupView(filetype);
            }
            catch (Exception ex)
            {
                Log.WriteLog("GraphViewContainer::Constructor::" + Environment.NewLine + ex.ToString());
            }
            ResumeLayout();
            ViewController.ReplaceMouseTools();

            zController = new ZoomController(myView);
            //myView.OverrideDocScaleMath = true;
            //myView.OverrideDocScaleMath = false;

            myView.DragEnter += new DragEventHandler(myView_DragEnter);
            myView.DragOver += new DragEventHandler(myView_DragOver);
            myView.DocumentChanged += new GoChangedEventHandler(myView_DocumentChanged);

            SetupFormatPanel();
        }
        protected void myView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(DragItemData).ToString()))
            {
                // the item(s) being dragged do not have any data associated
                e.Effect = DragDropEffects.Copy;
                //return;
            }

            // everything is fine, allow the user to move the items
            //e.Effect = DragDropEffects.Move;

            // call the base OnDragEnter event
            base.OnDragEnter(e);
        }
        private void myView_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data != null)
            {
                if (!e.Data.GetDataPresent(typeof(DragItemData).ToString()) || ((DragItemData)e.Data.GetData(typeof(DragItemData).ToString())).DragItems.Count == 0)//|| ((DragItemData)e.Data.GetData(typeof(DragItemData).ToString())).ListView == null
                    return;

                // retrieve the drag item data
                DragItemData data = (DragItemData)e.Data.GetData(typeof(DragItemData).ToString());
                e.Effect = DragDropEffects.Copy; ;
            }
            base.OnDragOver(e);
        }

        //myview.documentchanged
        private void myView_DocumentChanged(object sender, GoChangedEventArgs e)
        {
            #region HINTS

            //GoDocument 
            //RepaintAll 100 
            //BeginUpdateAllViews 101 
            //EndUpdateAllViews 102 
            //UpdateAllViews 103 
            //StartedTransaction 104 
            //FinishedTransaction 105 
            //AbortedTransaction 106 
            //StartingUndo 107 
            //FinishedUndo 108 
            //StartingRedo 109 
            //FinishedRedo 110 
            //ChangedName 201 
            //ChangedSize 202 
            //ChangedTopLeft 203 
            //ChangedFixedSize 204 
            //ChangedPaperColor 205 
            //ChangedDataFormat 206 
            //ChangedAllowSelect 207 
            //ChangedAllowMove 208 
            //ChangedAllowCopy 209 
            //ChangedAllowResize 210 
            //ChangedAllowReshape 211 
            //ChangedAllowDelete 212 
            //ChangedAllowInsert 213 
            //ChangedAllowLink 214 
            //ChangedAllowEdit 215 
            //AllArranged 220 
            //ChangedUserFlags 221 
            //ChangedUserObject 222 
            //ChangedLinksLayer 223 
            //ChangedMaintainsPartID 224 
            //ChangedValidCycle 225 
            //ChangedLastPartID 226 
            //ChangedRoutingTime 228 
            //ChangedInitializing 241 
            //LastHint 10000 
            //GoLayerCollection 
            //InsertedLayer 801 
            //RemovedLayer 802 
            //MovedLayer 803 
            //ChangedDefault 804 
            //GoLayer 
            //ChangedObject 901 See also the GoObject.Changed method: Changed  ------------------------------------>
            //InsertedObject 902 
            //RemovedObject 903 
            //ChangedObjectLayer 904 
            //ChangedAllowView 910 
            //ChangedAllowSelect 911 
            //ChangedAllowMove 912 
            //ChangedAllowCopy 913 
            //ChangedAllowResize 914 
            //ChangedAllowReshape 915 
            //ChangedAllowDelete 916 
            //ChangedAllowInsert 917 
            //ChangedAllowLink 918 
            //ChangedAllowEdit 919 
            //ChangedAllowPrint 920 
            //ChangedIdentifier 930 


            //------------------------------------>
            //GoObject 
            //RepaintAll 1000 
            //ChangedBounds 1001 
            //ChangedVisible 1003 
            //ChangedSelectable 1004 
            //ChangedMovable 1005 
            //ChangedCopyable 1006 
            //ChangedResizable 1007 
            //ChangedReshapable 1008 
            //ChangedDeletable 1009 
            //ChangedEditable 1010 
            //ChangedAutoRescales 1011 
            //ChangedResizeRealtime 1012 
            //ChangedShadowed 1013 
            //ChangedAddedObserver 1014 
            //ChangedRemovedObserver 1015 
            //ChangedDragsNode 1016 
            //ChangedPrintable 1017 
            //ChangedInitializing 1041 
            //LastChangedHint 10000 
            //GoGroup 
            //InsertedObject 1051 
            //RemovedObject 1052 
            //ChangedZOrder 1053 
            //ReplacedObject 1054 
            //ChangedPickableBackground 1055 
            //AddedChildName 1056 
            //RemovedChildName 1057 
            //GoShape 
            //ChangedPen 1101 
            //ChangedBrush 1102 
            //GoStroke 
            //AddedPoint 1201 
            //RemovedPoint 1202 
            //ModifiedPoint 1203 
            //ChangedAllPoints 1204 
            //ChangedStyle 1205 
            //ChangedCurviness 1206 
            //ChangedHighlightPen 1236 
            //ChangedHighlight 1237 
            //ChangedHighlightWhenSelected 1238 
            //ChangedToArrowHead 1250 
            //ChangedToArrowLength 1251 
            //ChangedToArrowShaftLength 1252 
            //ChangedToArrowWidth 1253 
            //ChangedToArrowFilled 1254 
            //ChangedToArrowStyle 1255 
            //ChangedFromArrowHead 1260 
            //ChangedFromArrowLength 1261 
            //ChangedFromArrowShaftLength 1262 
            //ChangedFromArrowWidth 1263 
            //ChangedFromArrowFilled 1264 
            //ChangedFromArrowStyle 1265 
            //GoLink 
            //ChangedLinkUserFlags 1300 
            //ChangedLinkUserObject 1301 
            //ChangedFromPort 1302 
            //ChangedToPort 1303 
            //ChangedOrthogonal 1304 
            //ChangedRelinkable 1305 
            //ChangedAbstractLink 1306 
            //ChangedAvoidsNodes 1307 
            //ChangedPartID 1309 
            //ChangedAdjustingStyle 1310 
            //ChangedToolTipText 1311 
            //ChangedDraggableOrthogonalSegments 1312 
            //GoLabeledLink 
            //ChangedLink 1311 
            //ChangedFromLabel 1312 
            //ChangedMidLabel 1313 
            //ChangedToLabel 1314 
            //ChangedFromLabelCentered 1315 
            //ChangedMidLabelCentered 1316 
            //ChangedToLabelCentered 1317 
            //GoPolygon 
            //AddedPoint 1401 
            //RemovedPoint 1402 
            //ModifiedPoint 1403 
            //ChangedAllPoints 1412 
            //ChangedStyle 1414 
            //GoRoundedRectangle 
            //ChangedCorner 1421 
            //GoTriangle 
            //ChangedPointA 1431 
            //ChangedPointB 1432 
            //ChangedPointC 1433 
            //ChangedAllPoints 1434 
            //GoHexagon 
            //ChangedDistanceLeft 1442 
            //ChangedDistanceRight 1443 
            //ChangedDistanceTop 1444 
            //ChangedDistanceBottom 1445 
            //ChangedOrientation 1446 
            //ChangedReshapeBehavior 1447 
            //ChangedReshapableCorner 1448 
            //ChangedKeepsLengthwiseSymmetry 1449 
            //ChangedKeepsCrosswiseSymmetry 1450 
            //GoPie 
            //ChangedStartAngle 1451 
            //ChangedSweepAngle 1452 
            //ChangedResizableStartAngle 1453 
            //ChangedResizableEndAngle 1454 
            //GoTrapezoid 
            //ChangedPointA 1460 
            //ChangedPointB 1461 
            //ChangedPointC 1462 
            //ChangedPointD 1463 
            //ChangedMultiplePoints 1464 
            //ChangedOrientation 1465 
            //GoParallelogram 
            //ChangedSkew 1466 
            //ChangedReshapableSkew 1467 
            //ChangedDirection 1468 
            //GoOctagon 
            //ChangedCorner 1469 
            //ChangedReshapableCorner 1470 
            //GoCylinder 
            //ChangedMinorRadius 1481 
            //ChangedOrientation 1482 
            //ChangedPerspective 1483 
            //ChangedResizableRadius 1484 
            //GoCube 
            //ChangedDepth 1491 
            //ChangedPerspective 1492 
            //ChangedReshapableDepth 1493 
            //GoText 
            //ChangedText 1501 
            //ChangedFamilyName 1502 
            //ChangedFontSize 1503 
            //ChangedAlignment 1504 
            //ChangedTextColor 1505 
            //ChangedBackgroundColor 1506 
            //ChangedTransparentBackground 1507 
            //ChangedBold 1508 
            //ChangedItalic 1509 
            //ChangedUnderline 1510 
            //ChangedStrikeThrough 1511 
            //ChangedMultiline 1512 
            //ChangedBackgroundOpaqueWhenSelected 1515 
            //ChangedClipping 1516 
            //ChangedAutoResizes 1518 
            //ChangedWrapping 1520 
            //ChangedWrappingWidth 1521 
            //ChangedGdiCharSet 1522 
            //ChangedEditorStyle 1523 
            //ChangedMinimum 1524 
            //ChangedMaximum 1525 
            //ChangedDropDownList 1526 
            //ChangedChoices 1527 
            //ChangedRightToLeft 1528 
            //ChangedRightToLeftFromView 1529 
            //ChangedBordered 1530 
            //ChangedStringTrimming 1531 
            //GoImage 
            //ChangedImage 1601 
            //ChangedResourceManager 1602 
            //ChangedName 1603 
            //ChangedAlignment 1604 
            //ChangedAutoResizes 1605 
            //ChangedImageList 1606 
            //ChangedIndex 1607 
            //ChangedThrowsExceptions 1608 
            //GoPort 
            //ChangedPortUserFlags 1700 
            //ChangedPortUserObject 1701 
            //ChangedStyle 1702 
            //ChangedObject 1703 
            //ChangedValidFrom 1704 
            //ChangedValidTo 1705 
            //ChangedValidSelfNode 1706 
            //ChangedFromSpot 1707 
            //ChangedToSpot 1708 
            //ChangedAddedLink 1709 
            //ChangedRemovedLink 1710 
            //ChangedValidDuplicateLinks 1711 
            //ChangedEndSegmentLength 1712 
            //ChangedPartID 1713 
            //ChangedClearsLinksWhenRemoved 1714 
            //GoGrid 
            //ChangedStyle 1801 
            //ChangedOrigin 1802 
            //ChangedOriginRelative 1803 
            //ChangedCellSize 1804 
            //ChangedLineColor 1805 
            //ChangedLineWidth 1806 
            //ChangedLineDashStyle 1807 
            //ChangedSnapDrag 1808 
            //ChangedSnapResize 1809 
            //ChangedCellColors 1810 
            //ChangedUnboundedSpots 1811 
            //ChangedSnapDragWhole 1812 
            //ChangedSnapOpaque 1814 
            //ChangedSnapCellSpot 1815 
            //ChangedMajorLineColor 1816 
            //ChangedMajorLineWidth 1817 
            //ChangedMajorLineDashStyle 1818 
            //ChangedMajorLineFrequency 1819 
            //ChangedLineDashPattern 1820 
            //ChangedMajorLineDashPattern 1821 
            //GoControl 
            //ChangedControlType 1901 
            //GoNode 
            //ChangedNodeUserFlags 2000 
            //ChangedNodeUserObject 2001 
            //ChangedToolTipText 2002 
            //ChangedPartID 2004 
            //GoNodeIcon 
            //ChangedMinimumIconSize 2050 
            //ChangedMaximumIconSize 2051 
            //GoBasicNode 
            //ChangedLabelSpot 2101 
            //ChangedShape 2102 
            //ChangedLabel 2103 
            //ChangedPort 2104 
            //ChangedMiddleLabelMargin 2105 
            //ChangedAutoResizes 2106 
            //GoBoxNode 
            //ChangedBody 2201 
            //ChangedPortBorderMargin 2202 
            //ChangedPort 2203 
            //GoBoxPort 
            //ChangedLinkPointsSpread 2211 
            //GoComment 
            //ChangedTopLeftMargin 2301 
            //ChangedBottomRightMargin 2302 
            //ChangedPartID 2303 
            //ChangedBackground 2304 
            //ChangedLabel 2305 
            //GoBalloon 
            //ChangedAnchor 2310 
            //ChangedBaseWidth 2312 
            //ChangedUnanchoredOffset 2313 
            //ChangedReanchorable 2314 
            //GoGeneralNode 
            //InsertedPort 2401 
            //RemovedPort 2402 
            //ReplacedPort 2403 
            //ChangedTopLabel 2404 
            //ChangedBottomLabel 2405 
            //ChangedIcon 2406 
            //ChangedOrientation 2407 
            //ChangedFromEndSegmentLengthStep 2408 
            //ChangedToEndSegmentLengthStep 2409 
            //GoGeneralNodePort 
            //ChangedName 2430 
            //ChangedLabel 2431 
            //ChangedSideIndex 2432 
            //ChangedLeftSide 2433 
            //GoListGroup 
            //ChangedSpacing 2501 
            //ChangedAlignment 2502 
            //ChangedLinePen 2503 
            //ChangedBorderPen 2504 
            //ChangedBrush 2505 
            //ChangedCorner 2506 
            //ChangedTopLeftMargin 2507 
            //ChangedBottomRightMargin 2508 
            //ChangedOrientation 2509 
            //ChangedTopIndex 2510 
            //ChangedMinimumItemSize 2511 
            //GoSimpleNode 
            //ChangedText 2601 
            //ChangedIcon 2602 
            //ChangedLabel 2603 
            //ChangedInPort 2604 
            //ChangedOutPort 2605 
            //ChangedOrientation 2606 
            //GoIconicNode 
            //ChangedDraggableLabel 2651 
            //ChangedIcon 2652 
            //ChangedLabel 2653 
            //ChangedPort 2654 
            //ChangedLabelOffset 2655 
            //GoSubGraph 
            //ChangedLabel 2702 
            //ChangedCollapsible 2703 
            //ChangedBackgroundColor 2704 
            //ChangedOpacity 2705 
            //ChangedLabelSpot 2706 
            //ChangedTopLeftMargin 2707 
            //ChangedBorderPen 2708 
            //ChangedCorner 2710 
            //ChangedPort 2711 
            //ChangedBottomRightMargin 2712 
            //ChangedCollapsedTopLeftMargin 2713 
            //ChangedCollapsedBottomRightMargin 2714 
            //ChangedCollapsedCorner 2715 
            //ChangedCollapsedLabelSpot 2716 
            //ChangedCollapsedObject 2717 
            //ChangedState 2718 
            //ChangedSavedBounds 2719 
            //ChangedSavedPaths 2720 
            //ChangedWasExpanded 2721 
            //ChangedExpandedResizable 2722 
            //GoTextNode 
            //ChangedLabel 2801 
            //ChangedBackground 2802 
            //ChangedTopPort 2803 
            //ChangedRightPort 2804 
            //ChangedBottomPort 2805 
            //ChangedLeftPort 2806 
            //ChangedTopLeftMargin 2807 
            //ChangedBottomRightMargin 2808 
            //ChangedAutoResizes 2809 
            //GoButton 
            //ChangedBackground 2901 
            //ChangedIcon 2902 
            //ChangedLabel 2903 
            //ChangedTopLeftMargin 2904 
            //ChangedBottomRightMargin 2905 
            //ChangedActionEnabled 2906 
            //ChangedAutoRepeating 2907 
            //GoCollapsibleHandle 
            //ChangedStyle 2950 
            //ChangedBordered 2951 
            //GoMultiTextNode 
            //InsertedLeftPort 3001 
            //InsertedRightPort 3002 
            //RemovedLeftPort 3003 
            //RemovedRightPort 3004 
            //ReplacedPort 3005 
            //ChangedTopPort 3006 
            //ChangedBottomPort 3007 
            //ChangedItemWidth 3008 
            //ChangedFromEndSegmentLengthStep 3009 
            //ChangedToEndSegmentLengthStep 3010 
            //GoSheet 
            //ChangedTopLeftMargin 3101 
            //ChangedBottomRightMargin 3102 
            //ChangedBackgroundImageSpot 3103 
            //ChangedShowsMargins 3104 
            //ChangedMarginColor 3105 
            //ChangedPaper 3110 
            //ChangedBackgroundImage 3111 
            //ChangedGrid 3112 

            #endregion

            //this should be an event
            if ((View.Document as NormalDiagram).UpdateSize)
                ViewController.UpdateSize(MetaBuilder.Graphing.Controllers.GraphViewController.CropType.ToFrame);

            if (!Core.Variables.Instance.IsDeveloperEdition)
                return;

            if (CustomModified)
                return;

            ////Disables saving
            ////should cancel if something that should not modify has but it is not true from before that modification begun
            myView.Document.IsModified = false;
            //DockingForm.DockForm.RemoveHighlights();

            if (!e.OldRect.IsEmpty && !e.NewRect.IsEmpty)
                if (e.OldRect.Width == e.NewRect.Width && e.OldRect.Height == e.NewRect.Height && e.OldRect.Location.X == e.NewRect.Location.X && e.OldRect.Location.Y == e.NewRect.Location.Y)
                    return;

            if (e.GoObject is FishRealLink || e.GoObject is GoCollapsibleHandle || e.GoObject is AllocationHandle || e.GoObject is QuickPort || e.GoObject is GoPort || e.GoObject is IndicatorLabel || e.GoObject is ChangedIndicatorLabel || e.GoObject is FrameLayerRect || e.GoObject is HighlightIndicator || e.GoObject is FrameLayerGroup)
                return;

            if (e.GoObject != null)
                if (e.GoObject.Parent is FrameLayerGroup)// || e.GoObject.Parent is FrameLayerRect)
                    if (!(e.GoObject is GoText))
                        return;
            //based on what changed
            //Add
            //Remove
            //Link
            //Artefact
            if (e.GoObject is IGoLink || e.GoObject is IGoNode || e.GoObject is ResizableComment || e.GoObject is ResizableBalloonComment || e.GoObject is ILinkedContainer || e.GoObject is GoShape)
                if (e.Hint == 902 || e.Hint == 903)
                    CustomModified = true;

            //Edit
            //Move
            //Format
            if (e.Hint == 901) //changed object
            {
                if (e.SubHint == 1001) //moved (bounds changed)
                {
                    if (e.GoObject is GoRectangle && (e.GoObject.Parent is MappingCell || e.GoObject.Parent is CollapsingRecordNodeItem))
                    {
                        //skip resize of swimlane rectangles
                        //e.GoObject.ToString();
                    }
                    else if (e.GoObject is GoImage || e.GoObject is ArtefactNode)
                    {
                        //e.GoObject.ToString();
                    }
                    else if (e.GoObject.Parent is CollapsingRecordNodeItem || e.GoObject is CollapsingRecordNodeItem || e.GoObject is RepeaterSection)
                    //resize or move of these is governed by parent
                    {
                        //e.GoObject.ToString();
                    }
                    else
                    {
                        if (e.GoObject is GraphNode) //only if the location of the node changes (the size should be static)
                        {
                            if (e.GoObject.Bounds.Location != e.NewRect.Location)
                            {
                                CustomModified = true;
                            }
                        }
                        else
                        {
                            CustomModified = true;
                        }
                    }
                }
                else if (e.SubHint == 1101 || e.SubHint == 1102)
                {
                    if (e.GoObject is QLink)
                    {
                        if ((e.GoObject as QLink).Pen.Color == Color.Orange || (e.GoObject as QLink).Pen.Color == Color.Black)
                        {
                            //e.GoObject.ToString();
                        }
                        else
                        {
                            CustomModified = true;
                        }
                    }
                    else if (e.GoObject is QRealLink)
                    {
                        if ((e.GoObject as QRealLink).Pen.Color == Color.Orange || (e.GoObject as QRealLink).Pen.Color == Color.Black)
                        {
                            //e.GoObject.ToString();
                        }
                        else
                        {
                            CustomModified = true;
                        }
                    }
                    //else
                    //{
                    //    if (e.OldValue.GetType().ToString().Contains("GoBrushInfo") || e.NewValue.GetType().ToString().Contains("GoBrushInfo"))
                    //    {
                    //        //this is handle by the formatting controls!

                    //        //    //check if brush or color has changed
                    //        //    if (e.OldValue.Equals(e.NewValue))
                    //        //    {
                    //        //        //no change to gobrushinfo
                    //        //    }
                    //        //    else
                    //        //    {
                    //        //        CustomModified = true; //changed shape pen/brush
                    //        //    }
                    //        //}
                    //        //else
                    //        //{
                    //        //    CustomModified = true; //changed shape pen/brush
                    //    }
                    //}
                }
                else if (e.SubHint == 1501) //changed text
                {
                    if (e.GoObject.Parent is FrameLayerGroup)
                    {
                        if ((e.GoObject as GoText).Editable)
                            CustomModified = true;
                    }
                    else
                    {
                        CustomModified = true;
                    }
                }
                else if ((e.SubHint - 1500) >= 2 && (e.SubHint - 1500) < 40) //Text Formatted
                    CustomModified = true;
                else if (e.SubHint == 1709 || e.SubHint == 1710) //added/removed a link @ port (PORT CHANGES IGNORED)
                    CustomModified = true;
                else if (e.SubHint == 2304) //changed comment background
                    CustomModified = true;
                else if (e.SubHint == 2310) //changed comment anchor
                    CustomModified = true;
                else if (e.SubHint == 2704) //changed colour of subgraph
                    CustomModified = true;
            }

            if (CustomModified)
            {
                Console.WriteLine(e.GoObject != null ? e.GoObject.GetType().ToString() : "NULL" + " - Hint: " + e.Hint + " SubHint: " + e.SubHint);
            }
        }
        //myview.document.change
        private void Document_Changed(object sender, GoChangedEventArgs e)
        {
            //if (e.Hint == 1001001)
            //{
            //   ShowSubgraphBindingForm(e);
            //}
            //Notify();
            ViewController.RemoveHighlights();
        }

        #region View Events

        private void myView_ItemsPasted(object sender, System.EventArgs e)
        {
            ItemsPasted();
            Notify();
        }
        private void myView_ClipboardPasted(object sender, System.EventArgs e)
        {
            if (FakeClipboardObjects.Count > 0)
                ItemsPasted();
            else
                ItemsHaveBeenAdded();

            Notify();
        }
        private void myView_ObjectSelectionDropped(object sender, Northwoods.Go.GoObjectEventArgs args)
        {
            if (ReadOnly)
                return;

            if (myView.Doc.ContainsILinkContainers == null || myView.Doc.TestedForILinkContainers == false)
            {
                ViewController.CheckForILinkContainers();
                myView.Doc.TestedForILinkContainers = true;
            }
            if (myView.Doc.ContainsILinkContainers.Value)
            {
                SubgraphBinding.LinkedContainerControlller lcc = new MetaBuilder.UIControls.GraphingUI.SubgraphBinding.LinkedContainerControlller();
                GoCollection col = new GoCollection();
                GoCollectionEnumerator selEnum = myView.Selection.GetEnumerator();
                while (selEnum.MoveNext())
                {
                    col.Add(selEnum.Current);
                }
                myView.Doc.ContainsILinkContainers = lcc.SelectionDropped(col, args, ref myView); // Update the property while we're at it
            }
            if (sender != myView && args.Control)
                ItemsHaveBeenAdded();
        }

        private void myView_KeyDown(object sender, KeyEventArgs e)
        {
            ViewController.removeQuickMenu();
            //shift = false;
            //alt = false;
            //control = false;
            //shift = e.Shift;
            //control = e.Control;
            //alt = e.Alt;
            //if ((Control.ModifierKeys & Keys.Control) != 0)
            //    control = true;
            //if ((Control.ModifierKeys & Keys.Alt) != 0)
            //    alt = true;
            //if ((Control.ModifierKeys & Keys.Shift) != 0)
            //    shift = true;

            NoPrompt = e.Shift;
            //if (shift)
            //    NoPrompt = true;
            //CheckSpelling();
        }
        private void myView_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            NoPrompt = false;
            //shift = false;

            if (e.Control && e.KeyCode == Keys.F7)
            {
                ViewController.FindTopShape();
            }
            if (e.Shift && e.Control && e.KeyCode == Keys.F7)
            {
                ViewController.FixOSDShapes(myView.Document);
            }
            if (e.Control && e.KeyCode == Keys.F9)
            {
                Tools.LoadFromDatabase ldb = new MetaBuilder.UIControls.GraphingUI.Tools.LoadFromDatabase();
                ldb.SwapShapes();
            }
            if (e.Control && e.KeyCode == Keys.F10)
            {
                DockingForm.DockForm.PWRModeEnabled = !DockingForm.DockForm.PWRModeEnabled;
                Dialogs.ManageHyperlinks mhlinks = new ManageHyperlinks();
                mhlinks.Setup(myView.Document);
                mhlinks.ShowDialog(DockingForm.DockForm);
            }
            if (e.Control && e.KeyCode == Keys.F8)
            {
                ViewController.MakeSameSizes(myView.Selection, true);
            }
            if ((e.Control && e.Shift && (e.KeyCode == Keys.C)) || (e.KeyCode == Keys.F8))
            {
                KeyBoardShallowCopy();
            }

            //if (control && e.KeyCode == Keys.V)
            //{
            //    ItemsPasted();
            //}

            /*  if ((control && shift && e.KeyCode == Keys.F10))
              {
                  FixValueChains(myView.Document);
                  RemovePorts(myView.Document);
                  AddNewPorts(myView.Document);
              }*/

            if ((e.Control && e.Shift && e.KeyCode == Keys.F12))
            {
                ViewController.RemovePorts(myView.Selection);
                //                AddNewPorts(myView.Selection);
            }
            if (e.KeyCode == Keys.Space)
            {
                /* ValueChainStep vcs = new ValueChainStep();
                 vcs.MetaObject = Loader.CreateInstance("Function");
                 vcs.BindingInfo = new BindingInfo();
                 vcs.BindingInfo.BindingClass = "Function";
                 vcs.BindingInfo.Bindings = new Dictionary<string, string>();
                 vcs.BindingInfo.Bindings.Add("lbl", "Name");
                 vcs.HookupEvents();
                 vcs.SaveToDatabase(this, EventArgs.Empty);*/
            }

            if (e.Control && e.KeyCode == Keys.F1)
            {
                //ViewController.FixOldAddShapes(myView.Document);
            }
            if (e.Control && e.KeyCode == Keys.F2)
            {
                ViewController.FixNonLinkingPorts(myView.Document);
            }

            if (e.Control && e.KeyCode == Keys.F3)
            {
                ViewController.FixDSDShapes(myView.Document);
            }
            if (e.Control && e.KeyCode == Keys.OemMinus)
            {
                zController.ZoomOut();
            }
            if (e.Control && e.KeyCode == Keys.Oemplus)
            {
                zController.ZoomOut();
            }

            if (e.Control && e.KeyCode == Keys.D0)
            {
                zController.ZoomToFit();
            }

            if (DockingForm.DockForm.PWRModeEnabled)
            {
                if ((e.Shift && e.KeyCode == Keys.Back))
                {
                    myView.StartTransaction();
                    //MetaBuilder.UIControls.Dialogs.RemoveOrMarkForDelete.ActionToBeTaken actionToBeTaken =
                    //    MetaBuilder.UIControls.Dialogs.RemoveOrMarkForDelete.ActionToBeTaken.Remove;
                    ViewController.ModifyObjects(myView.Selection, e.Shift, null);
                    //TakeQuickAction(actionToBeTaken);
                    myView.FinishTransaction("Remove Objects");
                }

                if ((e.Control && e.KeyCode == Keys.Back))
                {
                    //MetaBuilder.UIControls.Dialogs.RemoveOrMarkForDelete.ActionToBeTaken actionToBeTaken = MetaBuilder.UIControls.Dialogs.RemoveOrMarkForDelete.ActionToBeTaken.Delete;
                    ViewController.ModifyObjects(myView.Selection, e.Shift, null);
                    //TakeQuickAction(actionToBeTaken);
                }
            }

            if (e.KeyCode == System.Windows.Forms.Keys.F3)
                ShowFindText();

            if (e.KeyData == System.Windows.Forms.Keys.F6)
            {
                //TakeSnapshot();
            }

            if (e.KeyData == System.Windows.Forms.Keys.F4)
            {
                ViewController.SelectShallowCopies();
            }

            if (e.KeyData == System.Windows.Forms.Keys.F5)
            {
                if (myView.Selection.Count == 0)
                    RefreshData(myView.Document, Core.Variables.Instance.RefreshOnOpenDiagram == Variables.RefreshType.Prompt);
                else
                    RefreshData(myView.Selection, Core.Variables.Instance.RefreshOnOpenDiagram == Variables.RefreshType.Prompt);
                //Tools.MetaComparer mcomparer = new MetaBuilder.UIControls.GraphingUI.Tools.MetaComparer();
                //mcomparer.MyView = this.myView;
                //mcomparer.RefreshSelection(myView.Selection);
            }

            if (e.KeyData == System.Windows.Forms.Keys.F12)
            {
                ViewController.CollapseAll();
            }

            if (e.KeyData == System.Windows.Forms.Keys.Delete)
            {
                //ViewController.ModifyObjects(View.Selection, shift);
                if (Core.Variables.Instance.IsViewer)
                {
                    List<GoObject> delete = new List<GoObject>();
                    foreach (GoObject o in myView.Selection)
                        if (o.ParentNode is ResizableComment)
                            if ((o.ParentNode as ResizableComment).ViewerAdded)
                                delete.Add(o);
                            else if (o.ParentNode is ResizableBalloonComment)
                                if ((o.ParentNode as ResizableBalloonComment).ViewerAdded)
                                    delete.Add(o);

                    foreach (GoObject o in delete)
                        o.Remove();

                    e.Handled = true;
                    return;
                }
            }

            if (e.Control && e.KeyData == Keys.S)
            {
                if (e.Shift)
                    DockingForm.DockForm.SaveAll();
                else
                    StartSaveProcess(false);
            }

            if (e.Control && e.KeyCode == Keys.Enter)
            {
                if (myView.Selection.Count == 1 && myView.Selection.First is IMetaNode)
                {
                    if ((myView.Selection.First as IMetaNode).MetaObject != null)
                        ViewController.OnMetaObjectContextRequested((myView.Selection.First as IMetaNode).MetaObject, false);
                }
            }

            if (e.KeyCode == Keys.Space)
                DisplayTipForMappingCellsSurroundingPoint(myView.LastInput.DocPoint);

            if (Variables.Instance.IsDeveloperEdition)
            {
                if (e.Alt && e.Control && e.KeyCode == Keys.F8)
                {
                    DockingForm.DockForm.ShallowCopyCurrentDiagram();
                }
            }
        }

        private ZoomController zController;

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            IMetaNode selectedNode = null;
            if (myView.Selection.Count == 1)
            {
                if (myView.Selection.Primary is IMetaNode)
                {
                    selectedNode = (myView.Selection.Primary as IMetaNode);
                }
            }

            if (selectedNode != null)
            {
                int current = 0;
                Collection<IMetaNode> nodes = GetNodesInDocumentByClass(selectedNode.MetaObject.Class);
                if (nodes.Count == 1)
                {
                    return base.ProcessCmdKey(ref msg, keyData);
                }
                foreach (IMetaNode node in nodes)
                {
                    if (selectedNode == node)
                        break;
                    current += 1;
                }

                if (keyData == (Keys.Tab | Keys.Shift) && (current - 1 >= 0)) //previous
                {
                    myView.Selection.Clear();
                    myView.Selection.Add(nodes[current - 1] as GoObject);
                    myView.ScrollRectangleToVisible(myView.Selection.Primary.Bounds);
                    return true;
                }
                else if (keyData == Keys.Tab && (current + 1 < nodes.Count)) //next
                {
                    myView.Selection.Clear();
                    myView.Selection.Add(nodes[current + 1] as GoObject);
                    myView.ScrollRectangleToVisible(myView.Selection.Primary.Bounds);
                    return true;
                }
                else if (keyData == Keys.Tab) //first
                {
                    if (current + 1 >= nodes.Count)
                    {
                        myView.Selection.Clear();
                        myView.Selection.Add(nodes[0] as GoObject);
                        myView.ScrollRectangleToVisible(myView.Selection.Primary.Bounds);
                        return true;
                    }
                }
                else if (keyData == (Keys.Tab | Keys.Shift)) //last
                {
                    if (current - 1 < 0)
                    {
                        myView.Selection.Clear();
                        myView.Selection.Add(nodes[nodes.Count - 1] as GoObject);
                        myView.ScrollRectangleToVisible(myView.Selection.Primary.Bounds);
                        return true;
                    }
                }

            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void KeyBoardShallowCopy()
        {
            ViewController.removeQuickMenu();
            DockingForm.DockForm.ShallowCopies = new Collection<IShallowCopyable>();
            DockingForm.DockForm.ShallowGoObjects = new Collection<GoObject>();

            foreach (GoObject obj in myView.Selection)
            {
                #region old
                //         IShallowCopyable nodeCopy = originalNode.CopyAsShallow() as IShallowCopyable;

                //if (nodeCopy is GoNode)
                //{
                //    GoNode nodeCopyGoObject = nodeCopy as GoNode;
                //    nodeCopyGoObject.Remove();
                //    nodeCopy.HookupEvents();
                //    nodeCopyGoObject.Copyable = true;
                //    if (AddToDoc)
                //    {
                //        MyView.Document.Add(nodeCopyGoObject);
                //        int OffSet = 50;
                //        nodeCopyGoObject.Position = new PointF(originalNodeGoObject.Position.X + OffSet,
                //                                               originalNodeGoObject.Position.Y + OffSet);
                //        nodeCopyGoObject.Shadowed = true; //added
                //    }
                //    nodeCopy.HookupEvents();
                //    if (!(originalNodeGoObject is MappingCell))
                //        AddShadows(originalNodeGoObject);

                //    /*GoObject objO = originalNodeGoObject as GoObject;
                //objO.Shadowed = true;*/

                //    if (!(nodeCopyGoObject is MappingCell))
                //        AddShadows(nodeCopyGoObject);
                #endregion

                GoObject objCopy = obj.Copy();
                objCopy.Remove();
                if (obj is IShallowCopyable)
                {
                    objCopy = (obj as IShallowCopyable).CopyAsShallow();
                    //(objCopy as IShallowCopyable).MetaObject = (obj as IShallowCopyable).MetaObject;
                    //(objCopy as IShallowCopyable).CopiedFrom = (obj as IShallowCopyable).MetaObject;
                    if ((objCopy as IShallowCopyable).MetaObject.pkid == 0)
                    {
                        objCopy.ToString();
                    }
                    if (obj is GraphNode && objCopy is GraphNode)
                        (obj as GraphNode).CopyAllocationToNode(objCopy as GraphNode);
                    if (obj is ArtefactNode)
                    {
                        foreach (GoObject l in (obj as ArtefactNode).DestinationLinks.GetEnumerator())
                        {
                            if (l is FishLink)
                            {
                                (objCopy as ArtefactNode).ShallowFishLink = l as FishLink;
                                //break;
                            }
                        }
                    }
                }
                if (obj is ValueChain) //Not shallowcopyable?
                {
                    (objCopy as ValueChain).MetaObject = (obj as ValueChain).MetaObject;
                    (objCopy as ValueChain).CopiedFrom = (obj as ValueChain).MetaObject;
                }
                //Container'd Objects
                if (obj is ILinkedContainer)
                {
                    if (obj is IMetaNode)
                    {
                        AddContainerObjectsToFakeShallows(obj as ILinkedContainer, true, !View.LastInput.Shift);
                    }
                    else
                    {
                        obj.ToString();
                    }
                }

                if (obj is QLink)
                {
                    //after paste use this to relink to new nodes port
                    (objCopy as QLink).ToMetaBase = ((obj as QLink).ToNode as IMetaNode).MetaObject;
                    (objCopy as QLink).FromMetaBase = ((obj as QLink).FromNode as IMetaNode).MetaObject;

                    //QuickPort oFromPort = (obj as QLink).FromPort as QuickPort;
                    //QuickPort oToPort = (obj as QLink).ToPort as QuickPort;

                    (objCopy as QLink).ToPortShallow = (obj as QLink).ToPort;
                    (objCopy as QLink).FromPortShallow = (obj as QLink).FromPort;
                }
                if (obj is FishLink)
                {
                    //(objCopy as FishLink).FromNode = (obj as FishLink).FromNode;
                    (objCopy as FishLink).FromPort = (obj as FishLink).FromPort;
                    (objCopy as FishLink).ArtefactShallow = (obj as FishLink).FromNode as IMetaNode;
                    (objCopy as FishLink).ToQlinkShallow = (obj as FishLink).ToQLink;
                }
                if (obj is ResizableBalloonComment || obj is Rationale)
                {
                    (objCopy as ResizableBalloonComment).AnchorShallow = (obj as ResizableBalloonComment).Anchor;
                    if (obj is Rationale)
                    {
                        (objCopy as Rationale).MetaObject = (obj as Rationale).MetaObject;
                    }
                }

                if (!DockingForm.DockForm.ShallowGoObjects.Contains(objCopy))
                {
                    if (objCopy is ILinkedContainer)
                        if (View.LastInput.Shift)
                            objCopy = removeChildrenFromObject(objCopy as GoGroup);

                    DockingForm.DockForm.ShallowGoObjects.Add(objCopy);
                }

                continue;

                #region Old
                if (!(obj is GoNode))
                    continue;
                //{
                IShallowCopyable originalNode = obj as IShallowCopyable;
                GoNode originalNodeGoObject = originalNode as GoNode;
                originalNodeGoObject.Copyable = true;
                IShallowCopyable nodeCopy = originalNode.CopyAsShallow() as IShallowCopyable;

                GoNode nodeCopyGoObject = nodeCopy as GoNode;
                nodeCopyGoObject.Remove();
                nodeCopy.HookupEvents();
                nodeCopyGoObject.Copyable = true;

                for (int i = 0; i < originalNodeGoObject.Count; i++)
                {
                    if (originalNodeGoObject[i] is GoGroup)
                    {
                        GoGroup grp = originalNodeGoObject[i] as GoGroup;
                        for (int x = 0; x < grp.Count; x++)
                        {
                            if (grp[x] is RepeaterSection)
                            {
                                RepeaterSection rsec = grp[x] as RepeaterSection;
                                for (int r = 0; r < rsec.Count; r++)
                                {
                                    if (rsec[r] is IMetaNode)
                                    {
                                        IMetaNode imNode = rsec[r] as IMetaNode;
                                        GoGroup sameGroupInOther = nodeCopyGoObject[i] as GoGroup;
                                        RepeaterSection sameSectionInOther = sameGroupInOther[x] as RepeaterSection;
                                        IMetaNode imNodeCopy = sameSectionInOther[r] as IMetaNode;
                                        if (imNode.MetaObject != null)
                                        {
                                            imNodeCopy.MetaObject = imNode.MetaObject;
                                            imNodeCopy.HookupEvents();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                DockingForm.DockForm.ShallowCopies.Add(nodeCopyGoObject as IShallowCopyable);
                //}

                #endregion
            }
            //GoSelection sel = myView.Selection;
            myView.Selection.Clear();

            //we do this to prevent the clipboard from being pasted along with all our shallowobjects
            GoText dummyObject = new GoText();
            dummyObject.Text = "~!DUMMY!~";
            dummyObject.Visible = false;
            myView.Document.Add(dummyObject);
            myView.Selection.Add(dummyObject);

            ViewController.removeQuickMenu();
            myView.EditCopy();

            myView.Selection.Clear();
            //remove the dummy from this document
            myView.Document.Remove(dummyObject);
            //if you want to reselect what was selected
            //myView.Selection = sel;
        }

        private void myView_ObjectLostSelection(object sender, GoSelectionEventArgs e)
        {
            //myView.DragsRealtime = myView.Selection.Count < 30;
            if (!BusySelecting && !DockingForm.DockForm.ThreadingConstruction)
            {
                ViewController.removeQuickMenu();
                //13 November 2013 - bind node contents changed event if it is not yet bound
                //if (e.GoObject != null && e.GoObject.Parent != null)
                //{
                //    //if (e.GoObject.Parent is GraphNode)
                //    //{
                //    //    if ((e.GoObject.Parent as GraphNode).ContentsChanged == null)
                //    //    {
                //    //        (e.GoObject.Parent as GraphNode).ContentsChanged -= node_ContentsChanged;
                //    //        (e.GoObject.Parent as GraphNode).ContentsChanged += new EventHandler(node_ContentsChanged);
                //    //    }
                //    //}
                //    //else
                //    if (e.GoObject is IMetaNode)
                //    {
                //        if ((e.GoObject as IMetaNode).ContentsChanged == null)
                //        {
                //            (e.GoObject as IMetaNode).ContentsChanged -= node_ContentsChanged;
                //            (e.GoObject as IMetaNode).ContentsChanged += new EventHandler(node_ContentsChanged);
                //        }
                //    }
                //}
                //else 
                if (e.GoObject is IMetaNode)
                {
                    if ((e.GoObject as IMetaNode).ContentsChanged == null)
                    {
                        (e.GoObject as IMetaNode).ContentsChanged -= node_ContentsChanged;
                        (e.GoObject as IMetaNode).ContentsChanged += new EventHandler(node_ContentsChanged);
                    }
                }

                //DockingForm.DockForm.RemovePropertyFocus();
                //DockingForm.DockForm.ShowProperties(myView);
                UpdateMenuItems();
                TextFormatPanelVisibility(false);
                ColorFormatPanelVisibility();
                //fix for artifact shallow copies
                if (e.GoObject is ArtefactNode || e.GoObject is ILinkedContainer)
                {
                    DockingForm.DockForm.CheckIfExistsOnDiagramsThread(e.GoObject as IMetaNode);
                    return;
                }
                if (e.GoObject.ParentNode != null && (e.GoObject.ParentNode is ArtefactNode || e.GoObject.ParentNode is ILinkedContainer)) //Most times you select the gotext
                {
                    DockingForm.DockForm.CheckIfExistsOnDiagramsThread(e.GoObject.ParentNode as IMetaNode);
                    return;
                }
            }
            if (!BusySelecting)
            {
                if (myView.Selection.Count > 0)
                {
                    DockingForm.DockForm.ShowMetaObjectProperties(null);
                    if (myView.Selection.First is IMetaNode)
                    {
                        DockingForm.DockForm.ShowMetaObjectProperties((myView.Selection.First as IMetaNode).MetaObject);
                    }
                }
                else
                {
                    DockingForm.DockForm.ShowMetaObjectProperties(null);
                }
            }
        }
        private void myView_SelectionStarting(object sender, EventArgs e)
        {
            myView.BusySelecting = true;
            //check if is in mapping cell
            //if (sender is IMetaNode)
            //{
            //    IMetaNode imNode = sender as IMetaNode;

            //}

            //if (DockingForm.DockForm.IsViewer)
            //{
            //    if (e.GoObject.ParentNode is ResizableComment)
            //    {
            //        if ((e.GoObject.ParentNode as ResizableComment).ViewerAdded)
            //        {
            //            myView.Selection.Primary.DoBeginEdit(myView);
            //        }
            //    }
            //    else if (e.GoObject.ParentNode is ResizableBalloonComment)
            //    {
            //        if ((e.GoObject.ParentNode as ResizableBalloonComment).ViewerAdded)
            //        {
            //            myView.Selection.Primary.DoBeginEdit(myView);
            //        }
            //    }
            //}

        }
        private bool currentMetaObjectCanBeEdited;
        private void myView_ObjectGotSelection(object sender, GoSelectionEventArgs e)
        {
            //myView.DragsRealtime = myView.Selection.Count < 30;
            if (ItemsHaveBeenAddeding)
                return;

            currentMetaObjectCanBeEdited = true;
            if (!myView.Document.SuspendsUpdates && !DockingForm.DockForm.ThreadingConstruction)
            {
                if (!myView.BusySelecting)
                {
                    DisplayMetaProps();
                    UpdateMenuItems();

                    if (myView.Selection.Count == 1 && myView.Selection.First is MappingCell)
                    {
                        currentMetaObjectCanBeEdited = VCStatusTool.UserHasControl((myView.Selection.First as MappingCell).MetaObject);
                        //if (myView.Selection.First is MappingCell)
                        //{
                        GoCollection col = new GoCollection();
                        col.Add(myView.Selection.First);
                        if (myView.LastInput.Shift)
                        {
                            GoCollection collection = new GoCollection();
                            myView.PickObjectsInRectangle(true, false, (myView.Selection.First as MappingCell).Bounds, GoPickInRectangleStyle.SelectableOnlyContained, collection, 5000);
                            myView.Selection.AddRange(collection);
                        }
                        MarkAsDuplicates(col);
                        //}
                    }
                }

                if (!ReadOnly)
                {
                    if (myView.Selection.First is GraphNode && myView.Selection.Count == 1)
                    {
                        GraphNode n = myView.Selection.First as GraphNode;
                        if (VCStatusTool.UserHasControl(n.MetaObject))
                        {
                            if (DocumentVersion != null && DocumentVersion.VCStatus != VCStatusList.CheckedOutRead && DocumentVersion.VCStatus != VCStatusList.CheckedIn && DocumentVersion.VCStatus != VCStatusList.Locked)
                            {
                                if (n.MetaObject.State != VCStatusList.CheckedOutRead && n.MetaObject.State != VCStatusList.CheckedIn)
                                    ViewController.addQuickMenu(n);
                            }
                            else
                            {
                                ViewController.addQuickMenu(n);
                            }
                        }
                    }
                    else
                    {
                        ViewController.removeQuickMenu();
                    }

                    //return;
                }

                //if (!ViewController.ArtefactPointersVisible)
                //{
                //copying artefacts whose pointer(s?) are invisible does not copy the pointer because it was not selected since it is not visible
                if (e.GoObject is ArtefactNode)
                {
                    ArtefactNode aNode = e.GoObject as ArtefactNode;
                    foreach (GoObject o in aNode.Links)
                    {
                        if (o is FishLink)
                        {
                            if (myView.Selection.Contains((o as FishLink).ToQLink))
                                if (!myView.Selection.Contains(o))
                                    myView.Selection.Add(o);
                        }
                    }

                }
                //if (e.GoObject is GraphNode)
                //{
                //    foreach (GoObject link in (e.GoObject as GraphNode).Links)
                //    {
                //        if (myView.Selection.Contains(link)) //if the link(o) is selected
                //        {
                //            if (link is FishLink)
                //            {
                //                if (!myView.Selection.Contains(o))
                //                    myView.Selection.Add(o);
                //                //if (!myView.Selection.Contains(((o as FishLink).Artefact as GoObject)))
                //                //    myView.Selection.Add(((o as FishLink).Artefact as GoObject));

                //            }
                //        }
                //    }
                //}
                //}
            }

            AllowViewerMove();
        }
        private void AllowViewerMove()
        {
            if (Core.Variables.Instance.IsViewer)
            {
                foreach (GoObject o in myView.Selection)
                {
                    if (o.ParentNode is ResizableComment)
                    {
                        if ((o.ParentNode as ResizableComment).ViewerAdded)
                        {
                            MakeMovable(true);
                        }
                        else
                        {
                            MakeMovable(false);
                            break;
                        }
                    }
                    else if (o.ParentNode is ResizableBalloonComment)
                    {
                        if ((o.ParentNode as ResizableBalloonComment).ViewerAdded)
                        {
                            MakeMovable(true);
                        }
                        else
                        {
                            MakeMovable(false);
                            break;
                        }
                    }
                    else
                    {
                        MakeMovable(false);
                        break;
                    }
                }
            }
        }
        public void MakeMovable(bool m)
        {
            myView.AllowKey = myView.AllowDrop = myView.AllowCopy = myView.AllowEdit = myView.AllowMove = myView.AllowDelete = m;
            myView.Document.AllowMove = m;
            myView.Document.AllowCopy = m;
            myView.Document.AllowDelete = m;
        }

        private void DisplayTipForMappingCellsSurroundingPoint(PointF point)
        {
            string message = "";
            foreach (GoObject o in myView.Document)
                if (o is MappingCell)
                    //myView.PickObjectsInRectangle(true, false, (e.GoObject as MappingCell).Bounds, GoPickInRectangleStyle.SelectableOnlyContained, collection, 5000);
                    if (o.Bounds.Contains(point.X, point.Y))
                        message += (o as MappingCell).RectangleLocation + " : " + (((o as MappingCell).MetaObject.ToString().Length > 0) ? (o as MappingCell).MetaObject.ToString() : "[" + (o as MappingCell).MetaObject.Class + "]") + Environment.NewLine;
            if (message.Length == 0)
                return;

            DockingForm.DockForm.DisplayTip(message, "Swimlanes @ " + point.ToString());
        }

        private void myView_Dispatch(object sender, MetaBuilder.Graphing.DispatchMessageEventArgs e)
        {
            MessageBox.Show(DockingForm.DockForm, e.Sender.ToString());
        }
        private void myView_ObjectSingleClicked(object sender, GoObjectEventArgs e)
        {
            //#if DEBUG
            //            DockingForm.DockForm.m_propertyGridWindow.SelectedObject = e.GoObject;
            //#endif

            if (e.GoObject is Hyperlink)
            {
                if (e.Control)
                    GeneralUtil.LaunchHyperlink(e.GoObject as Hyperlink);
            }
            else if (e.GoObject is QuickPort)
            {
                if (Variables.Instance.ShowDeveloperItems)
                {
                    (e.GoObject as QuickPort).PortPosition = PortLocation;
                    (e.GoObject as QuickPort).Pen = new Pen(new SolidBrush(Color.Red)); //The pen is not loaded
                }
            }
            else if (e.GoObject.Parent is MappingCell)
            {
                if (myView.LastInput.Shift)
                {
                    myView.Selection.Add(e.GoObject.Parent);
                    GoCollection collection = new GoCollection();
                    myView.PickObjectsInRectangle(true, false, (e.GoObject.Parent as MappingCell).Bounds, GoPickInRectangleStyle.SelectableOnlyContained, collection, 5000);
                    myView.Selection.AddRange(collection);
                }
            }
            ApplyFormatCopy();
        }
        private void myView_SelectionMoved(object sender, EventArgs e)
        {
            ViewController.RemoveHighlights();
            ViewController.removeQuickMenu();
            Notify();
            //if (myView.Selection.Primary is GraphNode)
            //    ViewController.addQuickMenu(myView.Selection.Primary as GraphNode);
            //check the whole time if one of the objects in selection is moved out of a mappingcell
            //if it is then remove it from the diagram
        }
        private void myView_ObjectDoubleClicked(object sender, GoObjectEventArgs e)
        {
            if (!ReadOnly)
            {
                if (myView.Selection.Count > 0)
                {
                    //9 January 2013 Moved hyperlink to top to prevent null exception that occurs when inside a ilinkedcontainer
                    //9 January 2013 Made if tree instead of separate if statements
                    if (myView.Selection.Primary is Hyperlink)
                    {
                        Hyperlink link = myView.Selection.Primary as Hyperlink;
                        EditHyperlink editHyperlink = new EditHyperlink();
                        editHyperlink.SourceHyperlink = link;
                        editHyperlink.StartPosition = FormStartPosition.CenterScreen;
                        editHyperlink.ShowDialog(DockingForm.DockForm);
                        if (editHyperlink.DialogResult == DialogResult.OK)
                        {
                            link = editHyperlink.NewHyperlink;
                            //link.Text = editHyperlink.txtCaption.Text;
                        }
                    }
                    else if (myView.Selection.Primary is MappingCell)
                    {
                        //if (VCStatusTool.UserHasControl((myView.Selection.Primary as MappingCell).MetaObject))
                        //    ShowSubgraphBindingForm(myView.Selection.Primary as MappingCell); 
                        if (e.Control)
                        {
                            if ((myView.Selection.Primary as MappingCell).MetaObject != null)
                                lane_LaneDoubleClick(myView.Selection.Primary as MappingCell);
                        }
                        else
                        {
                            if (VCStatusTool.UserHasControl((myView.Selection.Primary as MappingCell).MetaObject))
                                (myView.Selection.Primary as MappingCell).Label.DoBeginEdit(myView);
                        }
                    }
                    //10 March 2014 All comments can be doubleclicked to edit
                    else if (myView.Selection.Primary is GoComment)
                    {
                        myView.Selection.Primary.DoBeginEdit(myView);
                    }
                    else if (e.GoObject.ParentNode is SubgraphNode)
                    {
                        //if (VCStatusTool.UserHasControl((e.GoObject.ParentNode as SubgraphNode).MetaObject))
                        //    ShowSubgraphBindingForm(e.GoObject.ParentNode as SubgraphNode);
                        if (e.Control)
                        {
                            if ((e.GoObject.ParentNode as SubgraphNode).MetaObject != null)
                                ShowSubgraphBindingForm(e.GoObject.ParentNode as SubgraphNode);
                        }
                        else
                        {
                            if (VCStatusTool.UserHasControl((e.GoObject.ParentNode as SubgraphNode).MetaObject))
                                (e.GoObject.ParentNode as SubgraphNode).Label.DoBeginEdit(myView);
                        }
                    }
                    else if (e.GoObject.DraggingObject is SubgraphNode)
                    {
                        //if (VCStatusTool.UserHasControl((e.GoObject.DraggingObject as SubgraphNode).MetaObject))
                        //    ShowSubgraphBindingForm(e.GoObject.DraggingObject as SubgraphNode);
                        if (e.Control)
                        {
                            if ((e.GoObject.DraggingObject as SubgraphNode).MetaObject != null)
                                ShowSubgraphBindingForm(e.GoObject.DraggingObject as SubgraphNode);
                        }
                        else
                        {
                            if (VCStatusTool.UserHasControl((e.GoObject.DraggingObject as SubgraphNode).MetaObject))
                                (e.GoObject.DraggingObject as SubgraphNode).Label.DoBeginEdit(myView);
                        }
                    }
                    else if (e.GoObject.Parent is ValueChain)
                    {
                        if (VCStatusTool.UserHasControl((e.GoObject.Parent as ValueChain).MetaObject))
                            ShowSubgraphBindingForm(e.GoObject.Parent as ValueChain);
                    }
                    //24 January 2013 Double click comments to edit them
                    else if (e.GoObject.ParentNode is GoComment)
                    {
                        if (e.GoObject.ParentNode is Rationale)
                        {
                            if (VCStatusTool.UserHasControl((e.GoObject.ParentNode as Rationale).MetaObject))
                            {
                                (e.GoObject.ParentNode as Rationale).DoBeginEdit(myView);
                            }
                        }
                        else
                            if (!(e.GoObject.ParentNode is IMetaNode)) //cannot edit imetanode comments
                                (e.GoObject.ParentNode as GoComment).DoBeginEdit(myView);
                    }
                    //28 January 2013 - added Visual Node
                    else if (e.GoObject.ParentNode is VisualNode)
                    {
                        (e.GoObject.ParentNode as VisualNode).DoBeginEdit(myView);
                    }
                    else if (e.GoObject.ParentNode is ArtefactNode && e.GoObject.ParentNode.Editable)
                    {
                        if ((e.GoObject.ParentNode as ArtefactNode).MetaObject.State != VCStatusList.CheckedOutRead || (e.GoObject.ParentNode as ArtefactNode).MetaObject.State != VCStatusList.CheckedIn || (e.GoObject.ParentNode as ArtefactNode).MetaObject.State != VCStatusList.Locked)
                        {
                            DockingForm.DockForm.ShowMetaObjectProperties((e.GoObject.ParentNode as ArtefactNode).MetaObject);
                            DockingForm.DockForm.m_metaPropsWindow.BringToFront();
                            DockingForm.DockForm.m_metaPropsWindow.Activate();
                            DockingForm.DockForm.m_metaPropsWindow.Focus();
                            SendKeys.Send("{TAB}");
                        }
                        else
                        {
                            DockingForm.DockForm.ShowMetaObjectProperties((e.GoObject.ParentNode as ArtefactNode).MetaObject, false);
                        }
                    }
                }
            }
            else if (Core.Variables.Instance.IsViewer)
            {
                if (e.GoObject.ParentNode is ResizableComment)
                {
                    if ((e.GoObject.ParentNode as ResizableComment).ViewerAdded)
                    {
                        myView.Selection.Primary.DoBeginEdit(myView);
                    }
                }
                else if (e.GoObject.ParentNode is ResizableBalloonComment)
                {
                    if ((e.GoObject.ParentNode as ResizableBalloonComment).ViewerAdded)
                    {
                        myView.Selection.Primary.DoBeginEdit(myView);
                    }
                }
            }
        }
        private void myView_ObjectEdited(object sender, GoSelectionEventArgs e)
        {
            ViewController.RemoveHighlights();
            if (e.GoObject.Parent is CollapsingRecordNodeItem)
            {
                if (e.GoObject is BoundLabel)
                {
                    IMetaNode node = e.GoObject.Parent as IMetaNode;
                    if (node != null && node.MetaObject != null)
                    {
                        if (Variables.Instance.CheckDuplicates)//(node.MetaObject.pkid == 0 || string.IsNullOrEmpty(node.MetaObject.MachineName)) &&
                            MarkDuplicates(node.MetaObject);
                        //else
                        {
                            //exclude artifact nodes here because they are controlled by object deselection
                            if (!(node is ArtefactNode))
                                DockingForm.DockForm.CheckIfExistsOnDiagramsThread(node);
                        }
                    }
                }
                //we dont want to run this twice now do we?
                return;
            }
            if (e.GoObject.ParentNode is IMetaNode)
            {
                if (e.GoObject is BoundLabel)
                {
                    IMetaNode node = e.GoObject.ParentNode as IMetaNode;
                    if (node != null && node.MetaObject != null)
                    {
                        if (Variables.Instance.CheckDuplicates)//(node.MetaObject.pkid == 0 || string.IsNullOrEmpty(node.MetaObject.MachineName)) &&
                            MarkDuplicates(node.MetaObject);
                        //else
                        {
                            //exclude artifact nodes here because they are controlled by object deselection
                            if (!(node is ArtefactNode))
                                DockingForm.DockForm.CheckIfExistsOnDiagramsThread(node);
                        }
                    }
                }
                else
                {
                    //Mapping cells dont save text after edited
                    IMetaNode node = e.GoObject.ParentNode as IMetaNode;
                    node.FireMetaObjectChanged(sender, e);
                }

                if (!View.SuggestionList.Contains((e.GoObject.ParentNode as IMetaNode).MetaObject.ToString()))
                {
                    View.SuggestionList.Add((e.GoObject.ParentNode as IMetaNode).MetaObject.ToString());
                }
            }
        }

        protected void myView_Dispatch(GoObject sender, MetaBuilder.Graphing.DispatchMessageEventArgs args)
        {
            //Console.WriteLine("Sender:" + args.Sender.ToString());
            //Console.WriteLine("ParticilarObject:" + args.ParticilarObject.ToString());
        }
        private void myView_NodeObjectContextClicked(object sender, NodeObjectContextArgs e)
        {
            // waha!
            // MessageBox.Show(DockingForm.DockForm, "PROFIT!!!");
            DockingForm.DockForm.HighlightConnectedObjects(e);

            //MetaBuilder.Graphing.Controllers.SerializationTester stester = new MetaBuilder.Graphing.Controllers.SerializationTester();
            //stester.TestSerialization(myView.Document as Norm)
        }
        private void myView_NodeObjectContextClickedShallow(object sender, NodeObjectContextArgs e)
        {
            DockingForm.DockForm.HighlightShallowCopies(e);
        }
        //private void myView_ItemAdded(object sender, GoSelectionEventArgs e)
        //{
        //    viewController.RemoveHighlights();
        //    ItemsHaveBeenAdded();
        //    Notify();
        //}
        private void myView_SelectionCopied(object sender, EventArgs e)
        {
            //Only works after control dragging
            //ViewController.removeQuickMenu();
            //MarkAsDuplicates(myView.Selection);
        }
        private void myView_ShallowCopy(object sender, EventArgs e)
        {
            //DockingForm.DockForm.ShallowCopies = new List<IShallowCopyable>();
            //DockingForm.DockForm.ShallowCopies.Add(sender as IShallowCopyable);

            if (sender is GoObject) // will always be becuase can only shallow IMetaNodes
            {
                DockingForm.DockForm.ShallowGoObjects = new Collection<GoObject>();
                DockingForm.DockForm.ShallowGoObjects.Add(sender as GoObject);
            }
        }
        private void myView_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DocScale")
            {
                //9 Januray 2013
                toolStripZoomLabel.Text = "Zoom : " + (Math.Round(myView.DocScale * 100f, 0)).ToString() + " %";
                //toolstripComboZoom.Text = (Math.Round(myView.DocScale * 100f, 0)).ToString() + "%";
                //#if DEBUG
                //                //TODO : Update quality of objects based on size
                //                if ((myView.DocScale * 100f) <= 10)
                //                {
                //                    ApplyQuality(ZoomQuality.Low, myView.Document);
                //                }
                //                else if ((myView.DocScale * 100f) <= 30)
                //                {
                //                    ApplyQuality(ZoomQuality.Med, myView.Document);
                //                }
                //                else
                //                {
                //                    ApplyQuality(ZoomQuality.Max, myView.Document);
                //                }
                //#endif
            }
        }
        //private void ApplyQuality(ZoomQuality quality, IGoCollection collection)
        //{
        //    switch (quality)
        //    {
        //        case ZoomQuality.Max:
        //            {
        //                myView.Grid.Visible = Core.Variables.Instance.ShowGrid;
        //                foreach (GoObject o in collection)
        //                {
        //                    if (o is GoText)
        //                        o.Visible = true;
        //                    if (o is IGoPort)
        //                        o.Visible = true;
        //                    if (o is IGoLink)
        //                        o.Visible = true;

        //                    if (o is GraphNode)
        //                        (o as GraphNode).Quality = quality;

        //                    if (o is IGoCollection)
        //                        ApplyQuality(quality, o as IGoCollection);
        //                }
        //                break;
        //            }
        //        case ZoomQuality.Med:
        //            {
        //                myView.Grid.Visible = false;
        //                //foreach (GoObject o in collection)
        //                //{
        //                //    //if (o is GoText)
        //                //    //    o.Visible = false;
        //                //    //if (o is IGoPort)
        //                //    //    o.Visible = false;

        //                //    if (o is GraphNode)
        //                //        (o as GraphNode).Quality = quality;

        //                //    if (o is IGoCollection)
        //                //        ApplyQuality(quality, o as IGoCollection);
        //                //}
        //                break;
        //            }
        //        case ZoomQuality.Low:
        //            {
        //                myView.Grid.Visible = false;
        //                //foreach (GoObject o in collection)
        //                //{
        //                //    //if (o is GoText)
        //                //    //    o.Visible = false;
        //                //    //if (o is IGoPort)
        //                //    //    o.Visible = false;

        //                //    if (o is GraphNode)
        //                //        (o as GraphNode).Quality = quality;

        //                //    if (o is IGoCollection)
        //                //        ApplyQuality(quality, o as IGoCollection);
        //                //}
        //                break;
        //            }
        //        default:
        //            {
        //                break;
        //            }
        //    }
        //}
        private void myView_OpenDiagramFromAnother(object sender, EventArgs e)
        {
#if DEBUG
            if (myView.LastInput.Shift)
            {
                FileUtil futil = new FileUtil();
                NormalDiagram loadedObject = futil.Open(sender as string) as NormalDiagram; //thread this call
                if (loadedObject == null)
                    return;

                GoSubGraph gsg = new GoSubGraph();
                gsg.Expand();
                gsg.Width = loadedObject.DocumentFrame.Width;
                gsg.Height = loadedObject.DocumentFrame.Height;
                foreach (GoObject o in loadedObject)
                {
                    o.Remove();
                    gsg.Add(o);
                }
                gsg.Location = myView.LastInput.DocPoint;
                this.myView.Document.Add(gsg);
                return;
            }
#endif
            DockingForm.DockForm.OpenFileInApplicableWindow(sender as string, false);
        }
        private void myView_SelectionChanged(object sender, GoSelectionEventArgs e)
        {

        }
        private void myView_SelectionFinished(object sender, EventArgs e)
        {
            if (DockingForm.DockForm.ThreadingConstruction)
                return;

            BusySelecting = false;
            if (!DisableMetaPropertiesOnSelection)
            {

                #region Fix faulty shape if necessary
                if (myView.Selection.Primary != null)
                {
                    if (myView.Selection.Primary.ParentNode != null)
                    {
                        if (myView.Selection.Primary.ParentNode is GraphNode)
                        {
                            GraphNode node = myView.Selection.Primary.ParentNode as GraphNode;
                            if (node.HasBindingInfo)
                            {
                                if (node.MetaObject == null)
                                {
                                    node.CreateMetaObject(this, EventArgs.Empty);
                                }
                                BindingInfo info = node.BindingInfo;
                                foreach (GoObject o in node)
                                {
                                    if (o is BoundLabel)
                                    {
                                        BoundLabel label = o as BoundLabel;
                                        if (info.Bindings.ContainsKey(label.Name))
                                        {
                                            label.Editable = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                int Count = myView.Selection.Count;
                if (Count == 0)
                {
                    //DockingForm.DockForm.ShowProperties(myView);
                    DockingForm.DockForm.ShowMetaObjectProperties(null);
                }
                //else if (Count == 1)
                //{
                //    DisplayMetaProps();
                //    //DockingForm.DockForm.ShowProperties(o);
                //}
                else// if (Count > 0)
                {
                    DisplayMetaProps();
                    //DockingForm.DockForm.RemovePropertyFocus();
                    //DockingForm.DockForm.ShowProperties(myView.Selection);
                }

                SelectionGrouped();

                myView.Focus();
            }

            ApplyFormatCopy();

            Notify();
        }

        private void SelectionGrouped()
        {
            bool CanGroup = myView.Selection.Count > 0;
            bool isGroup = myView.Selection.Primary != null && ((myView.Selection.Primary is GoSubGraph) || (myView.Selection.Primary is ShapeGroup) || (myView.Selection.Primary.ParentNode is ShapeGroup));
            //bool CanUngroup = isGroup && (!(myView.Selection.Primary.ParentNode is IMetaNode));

            menuItemShapeGrouping.Enabled = CanGroup;
            menuItemShapeGroupingGroup.Enabled = !isGroup;
            menuItemShapeGroupingUngroup.Enabled = isGroup;
        }

        private bool ExternalDragDrop = false;
        private void myView_ExternalObjectsDropped(object sender, GoInputEventArgs e)
        {
            ExternalDragDrop = true;
            //Multiple objects?
            if (myView.Selection.Primary != null)
            {
                GoObject obj = myView.Selection.Primary.DraggingObject;
                if (DiagramTypePallette != null)
                {
                    if (obj is IMetaNode)
                    {
                        if (!(DiagramTypePallette.Classes.Contains((obj as IMetaNode).BindingInfo.BindingClass))) //we will not put anyotherclass on diagram!
                        {
                            obj.Remove();
                            return;
                        }
                    }
                }
                if (obj is GraphNode)
                {
                    GraphNode node = obj as GraphNode;

                    if (node.AllowedClasses == "" && node.MustLoadFromDatabase)
                    {
                        MetaBase mBase = null;
                        if (e.Alt)
                        {
                            Tools.LoadFromDatabase ldb = new MetaBuilder.UIControls.GraphingUI.Tools.LoadFromDatabase();
                            if (node.AllowedClasses == "")
                                mBase = ldb.GetSingleObject();
                            else
                                mBase = ldb.GetSingleObject(node.BindingInfo.BindingClass);
                        }
                        else
                        {
                            ChooseClass cc = new ChooseClass("Choose a class", "This shape can represent any class");
                            cc.StartPosition = FormStartPosition.CenterScreen;
                            //cc.Location = new PointF(View.PointToScreen(new PointF(View.LastInput.DocPoint.X, View.LastInput.DocPoint.Y)));
                            cc.ShowDialog(DockingForm.DockForm);
                            if (cc.DialogResult == DialogResult.OK)
                            {
                                if (cc.SelectedClass != "")
                                {
                                    mBase = Loader.CreateInstance(cc.SelectedClass);
                                }
                            }
                        }

                        if (mBase == null)
                        {
                            Log.WriteLog("myView_ExternalObjectsDropped::Remove Node Because Metaobject is null");
                            myView.Document.Remove(node);
                            return;
                        }

                        node.MetaObject = mBase;
                        if (node.BindingInfo == null)
                            node.BindingInfo = new BindingInfo();
                        node.BindingInfo.BindingClass = mBase._ClassName;
                        node.IsStencilOnlyText = true;
                        //Log.WriteLog("myView_ExternalObjectsDropped::bind to metaobject");
                    }
                    else if (node.MustLoadFromDatabase || e.Alt)
                    {
                        //Log.WriteLog("myView_ExternalObjectsDropped::Mustload " + node.BindingInfo.BindingClass);
                        Tools.LoadFromDatabase ldb = new MetaBuilder.UIControls.GraphingUI.Tools.LoadFromDatabase();
                        MetaBase mBase = null;
                        if (node.AllowedClasses == "")
                            mBase = ldb.GetSingleObject();
                        else
                            mBase = ldb.GetSingleObject(node.BindingInfo.BindingClass);

                        if (mBase == null)
                        {
                            Log.WriteLog("myView_ExternalObjectsDropped::Remove Node Because Metaobject is null");
                            myView.Document.Remove(node);
                            return;
                        }

                        node.MustLoadFromDatabase = false;
                        node.MetaObject = mBase;
                        if (node.BindingInfo == null)
                            node.BindingInfo = new BindingInfo();
                        node.BindingInfo.BindingClass = mBase._ClassName;
                        //Log.WriteLog("myView_ExternalObjectsDropped::bind to metaobject from db");
                    }

                    #region Default Values
                    //Added for phys and log dataview (even multiple :))
                    foreach (GoObject objL in myView.Selection)
                    {
                        if (objL is GraphNode)
                        {
                            GraphNode n = objL as GraphNode;
                            if (n.MetaObject.Class == "DataView")
                            {
                                //18 October 2013 - Basic dataview must not get type set
                                string type = (n.Label as BoundLabel).Name == "lblDataViewTypexx" ? "Logical" : (n.Label as BoundLabel).Name == "lblDataViewType" ? "Physical" : "";
                                if (type.Length > 0)
                                {
                                    n.skipautomaticsave = true;
                                    n.MetaObject.Set("DataViewType", type);
                                    n.ContentsChanged -= new EventHandler(node_ContentsChanged);
                                    n.ContentsChanged += new EventHandler(node_ContentsChanged);
                                }
                            }
                            else if (n.MetaObject.Class == "DataEntity")// 
                            {
                                if (n.MetaObject.Get("DataEntityType") == null)//n.MetaObject.Get("EntityType") == null && 
                                {
                                    n.skipautomaticsave = true;
                                    //n.MetaObject.Set("EntityType", "O");
                                    n.MetaObject.Set("DataEntityType", "O");
                                }
                                else
                                {
                                    if (n.MetaObject.Get("DataEntityType").ToString() != "E")//n.MetaObject.Get("EntityType").ToString() != "E" &&
                                    {
                                        n.skipautomaticsave = true;
                                        //n.MetaObject.Set("EntityType", "O");
                                        n.MetaObject.Set("DataEntityType", "O");
                                    }
                                }
                                //TODO : Drop from stencil or from diagram?
                                //if (!string.IsNullOrEmpty(n.MetaObject.Get("EntityType").ToString()) && n.MetaObject.Get("EntityType").ToString() != "E") //HACK. default to O unless E (Can only be O or E)
                                //    n.MetaObject.Set("EntityType", "O");
                                //else
                                //    n.MetaObject.Set("EntityType", "O");
                            }
                            else if (n.MetaObject.Class == "Entity")// 
                            {
                                if (n.MetaObject.Get("EntityType") == null)//n.MetaObject.Get("EntityType") == null && 
                                {
                                    n.skipautomaticsave = true;
                                    //n.MetaObject.Set("EntityType", "O");
                                    n.MetaObject.Set("EntityType", "O");
                                }
                                else
                                {
                                    if (n.MetaObject.Get("EntityType").ToString() != "E")//n.MetaObject.Get("EntityType").ToString() != "E" &&
                                    {
                                        n.skipautomaticsave = true;
                                        //n.MetaObject.Set("EntityType", "O");
                                        n.MetaObject.Set("EntityType", "O");
                                    }
                                }
                                //TODO : Drop from stencil or from diagram?
                                //if (!string.IsNullOrEmpty(n.MetaObject.Get("EntityType").ToString()) && n.MetaObject.Get("EntityType").ToString() != "E") //HACK. default to O unless E (Can only be O or E)
                                //    n.MetaObject.Set("EntityType", "O");
                                //else
                                //    n.MetaObject.Set("EntityType", "O");
                            }
                            else if (n.MetaObject.Class == "Gateway")
                            {
                                n.skipautomaticsave = true;
                                n.MetaObject.Set("GatewayType", "Condition");
                            }
                        }
                    }
                    #endregion

                    foreach (GoObject o in myView.Selection)
                    {
                        if (o is IMetaNode)
                        {
                            //(o as IMetaNode).ContentsChanged += new EventHandler(node_ContentsChanged);
                            (o as IMetaNode).HookupEvents();
                            //(o as IMetaNode).FireMetaObjectChanged(this, EventArgs.Empty);
                        }

                        if (o is CollapsibleNode) //Expand lists when they are added
                            foreach (GoObject ochild in (o as CollapsibleNode))
                                if (ochild is ExpandableLabelList)
                                    (ochild as ExpandableLabelList).Expand();
                    }

                    DockingForm.DockForm.ShowMetaObjectProperties(node.MetaObject);
                }
                else
                {
                    if (obj is SubgraphNode)
                    {
                        #region dropping subgraphs
                        SubgraphNode sgNode = obj as SubgraphNode;
                        if (sgNode.IsStencil)
                        {
                            if (e.Alt)
                            {
                                Tools.LoadFromDatabase ldb = new MetaBuilder.UIControls.GraphingUI.Tools.LoadFromDatabase();
                                MetaBase mBase = ldb.GetSingleObject();
                                if (mBase != null)
                                {
                                    sgNode.Resizable = true;
                                    //sgNode.Size = new SizeF(200, 200);
                                    ShapeOrderingControl.SendToBack(sgNode, myView);

                                    sgNode.MetaObject = mBase;
                                    sgNode.FireMetaObjectChanged(this, EventArgs.Empty);

                                    sgNode.BindingInfo = new BindingInfo();
                                    sgNode.Position = ViewController.GetCenter();
                                    sgNode.BindingInfo.BindingClass = sgNode.MetaObject._ClassName;
                                    sgNode.HookupEvents();
                                    sgNode.Deletable = true;

                                    sgNode.IsStencil = false;

                                    myView.Doc.ContainsILinkContainers = true;

                                    Collection<IMetaNode> nodes = new Collection<IMetaNode>();
                                    nodes.Add(sgNode);
                                    locationTranslation(nodes);

                                    sgNode.Expand();

                                    sgNode.BindToMetaObjectProperties();

                                    //sgNode.DoResize(View, sgNode.Bounds, sgNode.Location, GoObject.MiddleRight, GoInputState.Start, sgNode.Bounds.Size, sgNode.Bounds.Size);
                                    sgNode.LayoutLabel();
                                }
                                else
                                {
                                    myView.Document.Remove(sgNode);
                                    return;
                                }
                            }
                            else
                            {
                                string _class = DockingForm.DockForm.GetAClass();
                                if (_class.Length > 0)
                                {
                                    sgNode.Resizable = true;
                                    //sgNode.Size = new SizeF(200, 200);
                                    ShapeOrderingControl.SendToBack(sgNode, myView);

                                    sgNode.MetaObject = Meta.Loader.CreateInstance(_class);

                                    sgNode.BindingInfo = new BindingInfo();
                                    //sgNode.BindToMetaObjectProperties();
                                    sgNode.Position = ViewController.GetCenter();
                                    sgNode.BindingInfo.BindingClass = sgNode.MetaObject._ClassName;
                                    sgNode.Label.Text = "New " + sgNode.MetaObject._ClassName;
                                    sgNode.HookupEvents();
                                    sgNode.FireMetaObjectChanged(this, EventArgs.Empty);
                                    sgNode.Deletable = true;

                                    sgNode.IsStencil = false;

                                    myView.Doc.ContainsILinkContainers = true;

                                    Collection<IMetaNode> nodes = new Collection<IMetaNode>();
                                    nodes.Add(sgNode);
                                    locationTranslation(nodes);

                                    sgNode.Expand();

                                    //sgNode.DoResize(View, sgNode.Bounds, sgNode.Location, GoObject.MiddleRight, GoInputState.Start, sgNode.Bounds.Size, sgNode.Bounds.Size);
                                    sgNode.LayoutLabel();
                                }
                                else
                                {
                                    myView.Document.Remove(sgNode);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            myView.Document.Remove(sgNode);
                            return;
                        }
                        #endregion
                    }
                    //else if (obj is MappingCell)
                    //{
                    #region Dropping swimlanes
                    //    MappingCell mCellNode = obj as MappingCell;
                    //    if (mCellNode.IsStencil)
                    //    {
                    //        if (e.Alt)
                    //        {
                    //            Tools.LoadFromDatabase ldb = new MetaBuilder.UIControls.GraphingUI.Tools.LoadFromDatabase();
                    //            MetaBase mBase = ldb.GetSingleObject();
                    //            if (mBase != null)
                    //            {
                    //                mCellNode.Resizable = true;
                    //                mCellNode.Size = new SizeF(200, 200);
                    //                ShapeOrderingControl.SendToBack(mCellNode, myView);

                    //                mCellNode.MetaObject = mBase;

                    //                mCellNode.BindingInfo = new BindingInfo();
                    //                mCellNode.BindToMetaObjectProperties();
                    //                mCellNode.Position = ViewController.GetCenter();
                    //                mCellNode.BindingInfo.BindingClass = mCellNode.MetaObject._ClassName;
                    //                mCellNode.HookupEvents();
                    //                mCellNode.Deletable = true;

                    //                mCellNode.IsStencil = false;

                    //                myView.Doc.ContainsILinkContainers = true;

                    //                List<IMetaNode> nodes = new List<IMetaNode>();
                    //                nodes.Add(mCellNode);
                    //                locationTranslation(nodes);
                    //            }
                    //            else
                    //            {
                    //                myView.Document.Remove(mCellNode);
                    //                return;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            string _class = DockingForm.DockForm.GetAClass();
                    //            if (!string.IsNullOrEmpty(_class))
                    //            {
                    //                mCellNode.Resizable = true;
                    //                mCellNode.Size = new SizeF(200, 200);
                    //                ShapeOrderingControl.SendToBack(mCellNode, myView);

                    //                mCellNode.MetaObject = Meta.Loader.CreateInstance(_class);

                    //                mCellNode.BindingInfo = new BindingInfo();
                    //                mCellNode.BindToMetaObjectProperties();
                    //                mCellNode.Position = ViewController.GetCenter();
                    //                mCellNode.BindingInfo.BindingClass = mCellNode.MetaObject._ClassName;
                    //                mCellNode.HookupEvents();
                    //                mCellNode.Deletable = true;

                    //                mCellNode.IsStencil = false;

                    //                myView.Doc.ContainsILinkContainers = true;

                    //                List<IMetaNode> nodes = new List<IMetaNode>();
                    //                nodes.Add(mCellNode);
                    //                locationTranslation(nodes);
                    //            }
                    //            else
                    //            {
                    //                myView.Document.Remove(mCellNode);
                    //                return;
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        myView.Document.Remove(mCellNode);
                    //        return;
                    //    }
                    #endregion
                    //}
                    else if (obj is ValueChain)
                    {
                        ValueChain sgNode = obj as ValueChain;
                        if (e.Alt)
                        {
                            Tools.LoadFromDatabase ldb = new MetaBuilder.UIControls.GraphingUI.Tools.LoadFromDatabase();
                            MetaBase mBase = ldb.GetSingleObject();
                            if (mBase != null)
                            {
                                sgNode.Resizable = true;
                                //sgNode.Size = new SizeF(200, 200);
                                ShapeOrderingControl.SendToBack(sgNode, myView);

                                sgNode.MetaObject = mBase;
                                sgNode.FireMetaObjectChanged(this, EventArgs.Empty);

                                sgNode.BindingInfo = new BindingInfo();
                                sgNode.Position = ViewController.GetCenter();
                                sgNode.BindingInfo.BindingClass = sgNode.MetaObject._ClassName;
                                sgNode.HookupEvents();
                                sgNode.Deletable = true;

                                //sgNode.IsStencil = false;

                                myView.Doc.ContainsILinkContainers = true;

                                Collection<IMetaNode> nodes = new Collection<IMetaNode>();
                                nodes.Add(sgNode);
                                locationTranslation(nodes);

                                //sgNode.Expand();

                                sgNode.BindToMetaObjectProperties();

                                //sgNode.DoResize(View, sgNode.Bounds, sgNode.Location, GoObject.MiddleRight, GoInputState.Start, sgNode.Bounds.Size, sgNode.Bounds.Size);
                                sgNode.LayoutLabel();
                            }
                            else
                            {
                                myView.Document.Remove(sgNode);
                                return;
                            }
                        }
                        else
                        {
                            string _class = DockingForm.DockForm.GetAClass();
                            if (_class.Length > 0)
                            {
                                sgNode.Resizable = true;
                                //sgNode.Size = new SizeF(200, 200);
                                ShapeOrderingControl.SendToBack(sgNode, myView);

                                sgNode.MetaObject = Meta.Loader.CreateInstance(_class);

                                sgNode.BindingInfo = new BindingInfo();
                                //sgNode.BindToMetaObjectProperties();
                                sgNode.Position = ViewController.GetCenter();
                                sgNode.BindingInfo.BindingClass = sgNode.MetaObject._ClassName;
                                sgNode.Label.Text = "New " + sgNode.MetaObject._ClassName;
                                sgNode.HookupEvents();
                                sgNode.FireMetaObjectChanged(this, EventArgs.Empty);
                                sgNode.Deletable = true;

                                //sgNode.IsStencil = false;

                                myView.Doc.ContainsILinkContainers = true;

                                Collection<IMetaNode> nodes = new Collection<IMetaNode>();
                                nodes.Add(sgNode);
                                locationTranslation(nodes);

                                //sgNode.Expand();

                                //sgNode.DoResize(View, sgNode.Bounds, sgNode.Location, GoObject.MiddleRight, GoInputState.Start, sgNode.Bounds.Size, sgNode.Bounds.Size);
                                sgNode.LayoutLabel();
                            }
                            else
                            {
                                myView.Document.Remove(sgNode);
                                return;
                            }
                        }
                    }

                    //DockingForm.DockForm.ShowProperties(obj);
                }
            }
            ExternalObjectDropped = true;
            ItemsHaveBeenAdded();
            SetPortVisibility(true);
            menuItemViewPorts.Checked = true;
            ExternalObjectDropped = false;
        }

        private void myView_DragDrop(object sender, DragEventArgs e)
        {
            GraphObjectNode n = e.Data.GetData("MetaBuilder.UIControls.MetaTree.GraphObjectNode") as GraphObjectNode;
            if (n != null && !ReadOnly) //cant add objects if readonly
            {
                GraphNode copiedObject = (GraphNode)n.GraphNode.Copy();
                copiedObject.Remove();
                copiedObject.Position = myView.LastInput.DocPoint;
                // FirstInput;// new PointF((float)MousePosition.X, (float)MousePosition.Y);
                myView.Document.Add(copiedObject);

                if (copiedObject.HasBindingInfo)
                {
                    if (copiedObject.MetaObject == null)
                        copiedObject.CreateMetaObject(this, e);
                    else
                        copiedObject.LoadMetaObject(copiedObject.MetaObject.pkid, copiedObject.MetaObject.MachineName);

                    if (copiedObject.MetaObject.State == VCStatusList.Obsolete || copiedObject.MetaObject.State == VCStatusList.MarkedForDelete)
                    {
                        MessageBox.Show(DockingForm.DockForm, "This object has been marked for delete or is obsolete", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        copiedObject.Remove();
                    }
                    else
                    {
                        copiedObject.HookupEvents();
                        myView.Selection.Clear();
                        myView.Selection.Add(copiedObject);
                    }
                }
                // finally, select the object
            }
            else if (e.Data.GetDataPresent(typeof(GoSelection)) == true)
            {
                GoSelection dragDropSelection = e.Data.GetData(typeof(GoSelection)) as GoSelection;
                if (dragDropSelection != null)
                {
                    foreach (GoObject copiedObject in dragDropSelection)
                    {
                        copiedObject.Shadowed = false;
                        if (copiedObject is CollapsibleNode)
                        {
                            //make all children have copiedfrom=null
                            foreach (RepeaterSection oChildList in (copiedObject as CollapsibleNode).RepeaterSections)
                            {
                                foreach (GoObject oChild in oChildList)
                                {
                                    if (oChild is IMetaNode)
                                    {
                                        (oChild as IMetaNode).CopiedFrom = null;
                                        oChild.Shadowed = false;
                                    }
                                }
                            }
                        }
                        if (!ExternalDragDrop)
                        {
                            if (copiedObject is IMetaNode)
                            {
                                if (Core.Variables.Instance.SaveOnCreate)
                                {
                                    if (DocumentVersion != null && ((copiedObject as IMetaNode).MetaObject.WorkspaceName == null || (copiedObject as IMetaNode).MetaObject.WorkspaceTypeId == 0))
                                    {
                                        (copiedObject as IMetaNode).MetaObject.WorkspaceName = DocumentVersion.WorkspaceName;
                                        (copiedObject as IMetaNode).MetaObject.WorkspaceTypeId = DocumentVersion.WorkspaceTypeId;
                                    }
                                }
                                //firing metaobjectchanged calls this
                                //(copiedObject as IMetaNode).BindToMetaObjectProperties();
                                //Removed May 25 2016 - Selection moved performance hit
                                //(copiedObject as IMetaNode).HookupEvents();
                                //(copiedObject as IMetaNode).ContentsChanged += new EventHandler(node_ContentsChanged);
                                //(copiedObject as IMetaNode).FireMetaObjectChanged(this, EventArgs.Empty);
                            }
                            else if (copiedObject is ILinkedContainer)
                            {
                                //SUBGRAPHS and children must firemetaobjectchanged to save 'duplicates'
                                if (Core.Variables.Instance.SaveOnCreate)
                                {
                                    if (DocumentVersion != null && ((copiedObject as ILinkedContainer).MetaObject.WorkspaceName == null || (copiedObject as ILinkedContainer).MetaObject.WorkspaceTypeId == 0))
                                    {
                                        (copiedObject as ILinkedContainer).MetaObject.WorkspaceName = DocumentVersion.WorkspaceName;
                                        (copiedObject as ILinkedContainer).MetaObject.WorkspaceTypeId = DocumentVersion.WorkspaceTypeId;
                                    }
                                }
                                (copiedObject as ILinkedContainer).MetaObject.OnChanged(EventArgs.Empty);
                            }
                        }
                        ExternalDragDrop = false;
                    }
                }
            }
            else
            {
                if (!e.Data.GetDataPresent(typeof(DragItemData).ToString()) || ((DragItemData)e.Data.GetData(typeof(DragItemData).ToString())).DragItems.Count == 0)// || ((DragItemData)e.Data.GetData(typeof(DragItemData).ToString())).ListView == null
                    return;

                // retrieve the drag item data
                DragItemData data = (DragItemData)e.Data.GetData(typeof(DragItemData).ToString());
                DockingForm.DockForm.SetcurrentGraphViewContainerForHPUM = this;

                //int x = e.X;
                //int y = e.Y;
                //get position of screen coords for diagram
                //Point p = myView.PointToClient(new Point(e.X, e.Y));
                //Point p = myView.ConvertDocToView(new Point(e.X, e.Y)));
                //PointF p = myView.ConvertViewToDoc(new Point(e.X, e.Y));
                PointF p = myView.LastInput.DocPoint;

                DockingForm.DockForm.SetLoadingPositionForHPUM((int)p.X, (int)p.Y, true);
                DockingForm.DockForm.RefreshAddedHPUMNodes = false;
                Collection<MetaBase> relinkMetaBases = new Collection<MetaBase>();
                int notLoaded = 0;
                try
                {
                    foreach (MetaBase mb in data.DragItems)
                    {
                        MetaBase mbase = mb;
                        if (mbase.ForceShapeType != null)
                        {
#if DEBUG
                            try
                            {
                                BaseGraphNode nnn = new BaseGraphNode(mbase);
                                nnn.Construct(mbase.Class);
                                if (nnn != null)
                                    goto skipReturnNullNodeForBaseNode;
                            }
                            catch
                            {
                                //return;
                            }
#endif
                            notLoaded += 1;
                            continue;
                        }
                    skipReturnNullNodeForBaseNode:
                        //4 shift
                        //8 control
                        //32 alt
                        //add together when pressed together
                        if (e.KeyState == 8) //Duplicate
                        {
                            MetaBase mbaseDupe = Loader.CreateInstance(mbase.Class);
                            mbase.CopyPropertiesToObject(mbaseDupe);
                            mbaseDupe.tag = mbaseDupe.Class;
                            DockingForm.DockForm.addNodeToGraphView(mbaseDupe);
                        }
                        else //Shallow copy
                        {
                            DockingForm.DockForm.addNodeToGraphView(mbase);

                            if (!(relinkMetaBases.Contains(mbase)))
                                relinkMetaBases.Add(mbase);
                        }
                    }
                    DockingForm.DockForm.RefreshAddedHPUMNodes = false;

                    DockingForm.DockForm.SetLoadingPositionForHPUM(0, 0, false);
                    //e.AllowedEffect = DragDropEffects.Copy;
                    relinkMetabaseList(relinkMetaBases);

                    RefreshData(View.Document, false);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                if (notLoaded > 0)
                {
                    DockingForm.DockForm.DisplayTip((notLoaded == 1 ? "An object was " : notLoaded.ToString() + " objects were ") + "not loaded because there is no shape to represent " + (notLoaded == 1 ? "it." : "them."), "Missing Shape");
                }
            }

            ExternalDragDrop = false;
        }

        private void relinkMetabaseList(Collection<MetaBase> relinkMetaBases)
        {
            Dictionary<MetaBase, IMetaNode> nodes = new Dictionary<MetaBase, IMetaNode>();
            //relink list of metabase which need it
            foreach (MetaBase mbase in relinkMetaBases)
            {
                IMetaNode outerNode = null;
                nodes.TryGetValue(mbase, out outerNode);
                if (outerNode == null)
                {
                    //find the node and add it to the dictionary for later
                    outerNode = GetNodeInDocumentByMetaObject(mbase);
                }
                if (outerNode != null)
                {
                    if (outerNode is GraphNode)
                        (outerNode as GraphNode).Shadowed = true;

                    if (!(nodes.ContainsKey(mbase)))
                        nodes.Add(mbase, outerNode);
                    foreach (MetaBase mbaseInner in relinkMetaBases)
                    {
                        IMetaNode innerNode = null;
                        nodes.TryGetValue(mbaseInner, out innerNode);
                        if (innerNode == null)
                        {
                            //find the node and add it to the dictionary for later
                            innerNode = GetNodeInDocumentByMetaObject(mbaseInner);
                        }
                        if (innerNode != null)
                        {
                            if (!(nodes.ContainsKey(mbaseInner)))
                                nodes.Add(mbaseInner, innerNode);
                            //recreate any links with this combo
                            //bool linked = false;
                            foreach (ObjectAssociation existingAssociation in DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(mbase.pkid, mbase.MachineName))
                            {
                                if (existingAssociation.Machine != "DB-TRIGGER")
                                {
                                    if (existingAssociation.ChildObjectID == mbaseInner.pkid && existingAssociation.ChildObjectMachine == mbaseInner.MachineName && existingAssociation.VCStatusID != (int)VCStatusList.MarkedForDelete)
                                    {
                                        linkAssociation(existingAssociation, outerNode, innerNode);
                                    }
                                }
                            }
                            //if (!linked)
                            //{
                            //    //this mbase combination does not have any links
                            //}
                        }
                        else
                        {
                            //the inner node cannot be found therefore i cannot link
                        }
                    }
                }
                else
                {
                    //the outer node cannot be found therefore i cannot link
                }
            }

            //relink collapsible items nodes to other nodes/itemnodes
            foreach (KeyValuePair<MetaBase, IMetaNode> kvp in nodes)
            {
                if (kvp.Value is CollapsibleNode)
                {
                    //link each of its child nodes to a node that is on the diagram
                    foreach (IMetaNode outerNode in getComplexNodeChildren(kvp.Value as CollapsibleNode))
                    {
                        foreach (ObjectAssociation existingAssociation in DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(outerNode.MetaObject.pkid, outerNode.MetaObject.MachineName))
                        {
                            if (existingAssociation.Machine != "DB-TRIGGER")
                            {
                                foreach (IMetaNode innerNode in ViewController.GetIMetaNodes())
                                {
                                    MetaBase mbaseInner = innerNode.MetaObject;
                                    if (existingAssociation.ChildObjectID == mbaseInner.pkid && existingAssociation.ChildObjectMachine == mbaseInner.MachineName && existingAssociation.VCStatusID != (int)VCStatusList.MarkedForDelete)
                                    {
                                        linkAssociation(existingAssociation, outerNode, innerNode);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool linkAssociation(ObjectAssociation existingAssociation, IMetaNode outerNode, IMetaNode innerNode)
        {
            ClassAssociation cAss = DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByCAid(existingAssociation.CAid);
            if (cAss.ParentClass == "Rationale" || cAss.ChildClass == "Rationale")
            {
                //reanchor
                if (innerNode is Rationale)
                {
                    (innerNode as Rationale).Anchor = outerNode as GoObject;
                }
                else if (outerNode is Rationale)
                {
                    (outerNode as Rationale).Anchor = innerNode as GoObject;
                }
                return true;
            }

            QLink link = QLink.CreateLink(outerNode as GoNode, innerNode as GoNode, cAss.AssociationTypeID, 0);
            View.Document.Add(link);

            //add artefacts?
            foreach (Artifact a in DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ArtifactProvider.Find("ObjectId = " + existingAssociation.ObjectID + " AND ObjectMachine = '" + existingAssociation.ObjectMachine + "' AND ChildObjectID = " + existingAssociation.ChildObjectID + " AND ChildObjectMachine = '" + existingAssociation.ChildObjectMachine + "' AND CaID = " + existingAssociation.CAid))
            {
                MetaObject mObj = DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetBypkidMachine(a.ArtifactObjectID, a.ArtefactMachine);
                MetaBase art = Loader.GetFromProvider(mObj.pkid, mObj.Machine, mObj.Class, false);
                if (mObj.Class == "Rationale")
                {
                    IMetaNode node = GetNodeInDocumentByMetaObject(art);
                    if (node != null && node is Rationale)
                    {
                        (node as Rationale).Anchor = link;
                    }
                    else
                    {
                        Rationale rat = new Rationale();
                        rat.BindingInfo = new BindingInfo();
                        rat.BindingInfo.BindingClass = art.Class;
                        rat.Location = link.MidLabel.Center;
                        rat.MetaObject = art;
                        rat.HookupEvents();
                        rat.BindToMetaObjectProperties();
                        rat.Anchor = link;
                        View.Document.Add(rat);
                    }
                }
                else
                {
                    ArtefactNode n = new ArtefactNode();
                    n.BindingInfo = new BindingInfo();
                    n.BindingInfo.BindingClass = art.Class;
                    n.Location = link.MidLabel.Center;
                    n.Editable = true;
                    n.Label.Editable = false;
                    n.MetaObject = art;
                    n.HookupEvents();
                    n.BindToMetaObjectProperties();
                    View.Document.Add(n);
                    GoGroup grp = link.MidLabel as GoGroup;
                    foreach (GoObject o in grp)
                    {
                        if (o is FishLinkPort)
                        {
                            // now link 'em
                            FishLink fishlink = new FishLink();
                            fishlink.FromPort = n.Port;
                            FishLinkPort flp = o as FishLinkPort;
                            //flp.IsValidFrom = false;  // can only draw links to this port, not from it
                            //flp.ToSpot = NoSpot;
                            fishlink.ToPort = flp;
                            fishlink.Visible = ViewController.ArtefactPointersVisible;
                            View.Document.Add(fishlink);
                        }
                    }
                }
            }
            return true;
        }

        private List<CollapsingRecordNodeItem> getComplexNodeChildren(CollapsibleNode node)
        {
            List<CollapsingRecordNodeItem> returnNodes = new List<CollapsingRecordNodeItem>();

            foreach (GoObject item in node.List)
            {
                if (item is CollapsingRecordNodeItemList)
                {
                    foreach (GoObject o in item as CollapsingRecordNodeItemList)
                    {
                        if (o is CollapsingRecordNodeItem)
                        {
                            if ((o as CollapsingRecordNodeItem).IsHeader)
                                continue;
                            returnNodes.Add((o as CollapsingRecordNodeItem));
                        }
                    }
                }
                else if (item is CollapsingRecordNodeItem)
                {
                    if ((item as CollapsingRecordNodeItem).IsHeader)
                        continue;
                    returnNodes.Add(item as CollapsingRecordNodeItem);
                }
            }

            return returnNodes;
        }

        private IMetaNode GetNodeInDocumentByMetaObject(MetaBase mbase)
        {
            IMetaNode lastNodeAddedToDocumentWithThisMetabase = null;
            foreach (GoObject o in View.Document)
                if (o is IMetaNode)
                    if ((o as IMetaNode).MetaObject.pkid == mbase.pkid && (o as IMetaNode).MetaObject.MachineName == mbase.MachineName)
                        lastNodeAddedToDocumentWithThisMetabase = (o as IMetaNode);

            return lastNodeAddedToDocumentWithThisMetabase;
        }
        private Collection<IMetaNode> GetNodesInDocumentByClass(string Class)
        {
            Collection<IMetaNode> nodes = new Collection<IMetaNode>();
            foreach (GoObject o in View.Document)
            {
                if (o is IMetaNode)
                {
                    if (Class.Length > 0)
                    {
                        if ((o as IMetaNode).MetaObject.Class == Class)
                            nodes.Add(o as IMetaNode);
                    }
                    else
                    {
                        nodes.Add(o as IMetaNode);
                    }
                }
            }

            return nodes;
        }

        private void myView_LinkCreated(object sender, GoSelectionEventArgs e)
        {
            if (!(e.GoObject is QLink))
            {
                //if (e.GoObject is MetaBuilder.Graphing.Shapes.General.MindMapLink)
                //    NeedLayout();

                Notify();
                return;
            }

            QLink l = e.GoObject as QLink;
            if (l != null)
            {
                if (l.ToNode is IMetaNode && l.FromNode is IMetaNode)
                {

                    if (l.ToPort is FishNodePort || l.ToPort is FishLinkPort)
                    {
                        Log.WriteLog("Fish{X}Port parent is not QLink");
                        return;
                    }

                    TestValidLink(l, false);
                    if (l.IsInDatabase) // this will add it to database :)
                    {
                    }
                    #region Multilink
                    if (myView.Selection.Count > 1) //Link other selected nodes the same way this one was just linked
                    {
                        bool wasfrom = false;
                        //get the node which this link was either linked to or from
                        foreach (GoObject obj in myView.Selection)
                        {
                            if (wasfrom)
                                break;

                            if (obj is IMetaNode)
                            {
                                IMetaNode imn = obj as IMetaNode;

                                wasfrom = (l.FromNode == imn);
                                continue;
                            }
                        }
                        //now link each extra node in the selection to the other node using the default association
                        foreach (GoObject obj in myView.Selection)
                        {
                            if (obj is IMetaNode && obj != l.ToNode && obj != l.FromNode)
                            {
                                QLink xLink = null;
                                IGoPort fromPort = l.FromPort as IGoPort;
                                IGoPort toPort = l.ToPort as IGoPort;

                                if (wasfrom)
                                {
                                    //IGoPort objPort = getportonnodebasedonothernodesport(obj as GoNode, referenceNode as GoNode, toPort);
                                    IGoPort objPort = getportonnodebasedonothernodesport(obj as GoNode, l.FromNode as GoNode, l.FromPort);
                                    if (objPort == null)
                                    {
                                        xLink = QLink.CreateLink(obj as GoNode, l.ToNode as GoNode, (int)l.AssociationType, 0);
                                    }
                                    else
                                    {
                                        xLink = QLink.CreateLink(l.FromNode as GoNode, obj as GoNode, (int)l.AssociationType, objPort as GoPort, l.ToPort as GoPort);
                                    }
                                }
                                else
                                {
                                    //IGoPort objPort = getportonnodebasedonothernodesport(obj as GoNode, referenceNode as GoNode, fromPort);
                                    IGoPort objPort = getportonnodebasedonothernodesport(obj as GoNode, l.ToNode as GoNode, l.ToPort);
                                    if (objPort == null)
                                    {
                                        xLink = QLink.CreateLink(l.FromNode as GoNode, obj as GoNode, (int)l.AssociationType, 0);
                                    }
                                    else
                                    {
                                        xLink = QLink.CreateLink(l.FromNode as GoNode, obj as GoNode, (int)l.AssociationType, l.FromPort as GoPort, objPort as GoPort);
                                    }
                                }
                                if (xLink != null)
                                {
                                    myView.Document.Add(xLink);
                                    TestValidLink(xLink, false);
                                    if (xLink.IsInDatabase) // this will add it to database :)
                                    {
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    IMetaNode from = l.FromNode as IMetaNode;
                    IMetaNode to = l.ToNode as IMetaNode;
                    if (from.MetaObject.pkid > 0 && to.MetaObject.pkid > 0)
                    {
                        //refresh link status
                        foreach (ClassAssociation ca in AssociationManager.Instance.GetAssociationsForParentAndChildClasses(from.MetaObject.Class, to.MetaObject.Class))
                        {
                            if (ca.AssociationTypeID == (int)l.AssociationType)
                            {
                                try
                                {
                                    b.ObjectAssociation oass = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(ca.CAid, from.MetaObject.pkid, to.MetaObject.pkid, from.MetaObject.MachineName, to.MetaObject.MachineName);
                                    if (oass != null)
                                    {
                                        if (oass.State == VCStatusList.MarkedForDelete)
                                        {
                                            l.RealLink.Pen = new Pen(Color.Red, 1f);
                                        }
                                    }
                                }
                                catch
                                {
                                    //first time association created :))
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (l.ToPort is FishLinkPort)
                    {
                        FishLink replacedLink = new FishLink();
                        replacedLink.FromPort = l.FromPort;
                        replacedLink.ToPort = l.ToPort;
                        myView.Document.Add(replacedLink);
                        l.Remove();
                    }
                }
            }
            if (l.Parent is SubgraphNode)
            {
                l.Remove();
                if ((l.FromNode is SubgraphNode && (l.FromNode as SubgraphNode).Contains(l.ToNode)) || (l.ToNode is SubgraphNode && (l.ToNode as SubgraphNode).Contains(l.FromNode)))
                {
                }
                else
                {
                    View.Document.Add(l);
                    ShapeOrderingControl.BringToFront(l, View);
                }
            }
            //Notify();
        }
        private void myView_LinkRelinked(object sender, GoSelectionEventArgs e)
        {
            QLink l = e.GoObject as QLink;
            if (l != null)
            {
                if (l.ToNode is GraphNode && l.FromNode is GraphNode)
                {
                    TestValidLink(l, true);
                    if (l.IsInDatabase) // this will add it to database :)
                    {
                    }
                }
                System.Drawing.Drawing2D.DashStyle prevDashStyle = l.Pen.DashStyle;
                DockingForm.DockForm.m_taskDocker.RemoveTaskWhereTagEquals(ContainerID.ToString(), l, typeof(MissingPortTask));
                DockingForm.DockForm.m_taskDocker.BindToList(ContainerID.ToString());
                Pen reset = new Pen(l.PenColorBeforeCompare, 1);
                if (l.AssociationType == LinkAssociationType.SubSetOf || l.AssociationType == LinkAssociationType.MutuallyExclusiveLink)
                    reset.DashStyle = prevDashStyle;
                else
                    reset.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                l.Pen = reset;

                if (l.Parent is SubgraphNode)
                {
                    l.Remove();
                    View.Document.Add(l);
                    ShapeOrderingControl.BringToFront(l, View);
                }
                Notify();
            }
        }
        private void myView_Click(object sender, EventArgs e)
        {
            if (!isSelecting && myView.Selection.Count == 0)
                DockingForm.DockForm.ShowDiagramProperties();
        }

        #endregion

        private IGoPort getportonnodebasedonothernodesport(GoNode node, GoNode othernode, IGoPort othernodesport)
        {
            if (othernodesport is QuickPort && (othernodesport as QuickPort).PortPosition == MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Circumferential) //if circumferential
            {
                PointF centerOtherNode = othernode.Center;
                PointF centerOtherNodesPort = (othernodesport as QuickPort).Center;

                #region quadrantBoolean
                bool left = false;
                bool top = false;
                //which quadrant based on center of node does the 'old'port fall in?
                if (centerOtherNodesPort.X < centerOtherNode.X) //LEFT
                {
                    left = true;
                    if (centerOtherNodesPort.Y < centerOtherNode.Y) //TOP
                    {
                        top = true;
                    }
                    else //BOTTOM
                    {
                        top = false;
                    }
                }
                else //RIGHT
                {
                    left = false;
                    if (centerOtherNodesPort.Y < centerOtherNode.Y) //TOP
                    {
                        top = true;
                    }
                    else //BOTTOM
                    {
                        top = false;
                    }
                }
                #endregion

                Collection<QuickPort> possiblePorts = getquadrantports(node, left, top, false, othernodesport);

                if (possiblePorts.Count == 1)
                    return possiblePorts[0] as IGoPort;

                //more than 1?
                //different shape?
                #region THIS IS WHY WE NEED PORTPOSITIONS!

                Collection<QuickPort> otherNodePossiblePorts = getquadrantports(othernode, left, top, false, othernodesport);

                int number = 0;
                float smallestx = 0;
                float smallesty = 0;
                float biggestx = 0;
                float biggesty = 0;
                foreach (QuickPort xP in otherNodePossiblePorts)
                {
                    if (number == 0)
                    {
                        smallestx = xP.Location.X;
                        smallesty = xP.Location.Y;
                        biggestx = xP.Location.X;
                        biggesty = xP.Location.Y;

                        number++;
                        continue;
                    }

                    if (xP.Location.X < smallestx)
                        smallestx = xP.Location.X;
                    if (xP.Location.Y < smallesty)
                        smallesty = xP.Location.Y;

                    if (xP.Location.X > biggestx)
                        biggestx = xP.Location.X;
                    if (xP.Location.Y > biggesty)
                        biggesty = xP.Location.Y;

                    number++;
                }

                bool isleftPort = false;
                bool isTopPort = false;
                bool isRightPort = false;
                bool isBottomPort = false;
                if (smallestx == (othernodesport as QuickPort).Location.X)
                {
                    isleftPort = true;
                }

                if (biggestx == (othernodesport as QuickPort).Location.X)
                {
                    isRightPort = true;
                }

                if (smallesty == (othernodesport as QuickPort).Location.Y)
                {
                    isTopPort = true;
                }

                if (biggesty == (othernodesport as QuickPort).Location.Y)
                {
                    isBottomPort = true;
                }

                number = 0;
                //find the ltbr ports of possibles

                QuickPort portToUse = null;

                if (isleftPort || isRightPort || isTopPort || isBottomPort)
                {
                    QuickPort leftPort = null;
                    QuickPort rightPort = null;
                    QuickPort bottomPort = null;
                    QuickPort topPort = null;

                    foreach (QuickPort xP in possiblePorts)
                    {
                        if (number == 0)
                        {
                            leftPort = xP;
                            rightPort = xP;
                            rightPort = xP;
                            rightPort = xP;

                            number++;
                            continue;
                        }
                        if (xP.Location.X < leftPort.Location.X)
                            leftPort = xP;
                        if (xP.Location.X > leftPort.Location.X)
                            rightPort = xP;

                        if (xP.Location.Y < leftPort.Location.Y)
                            topPort = xP;
                        if (xP.Location.Y > leftPort.Location.Y)
                            bottomPort = xP;
                    }
                    if (isleftPort && leftPort != null)
                        portToUse = leftPort;
                    if (isRightPort && rightPort != null)
                        portToUse = rightPort;
                    if (isTopPort && topPort != null)
                        portToUse = topPort;
                    if (isBottomPort && bottomPort != null)
                        portToUse = bottomPort;

                    if (portToUse == null)
                    {
                        //use the middle most port from the possibleports
                        int midNumber = (int)Math.Round((decimal)((possiblePorts.Count - 1) / 2), 0, MidpointRounding.AwayFromZero);
                        portToUse = possiblePorts[midNumber];
                    }
                }
                else
                {
                    //use the middle most port from the possibleports
                    int midNumber = (int)Math.Round((decimal)((possiblePorts.Count - 1) / 2), 0, MidpointRounding.AwayFromZero);
                    portToUse = possiblePorts[midNumber];
                }
                if (portToUse != null)
                    return portToUse;
                #endregion

            }
            else
            {
                if (node is GraphNode)
                {
                    foreach (GoPort prt in (node as GraphNode).Ports)
                    {
                        if (prt is QuickPort && othernodesport is QuickPort)
                            if ((prt as QuickPort).PortPosition == (othernodesport as QuickPort).PortPosition)
                                return prt as IGoPort;
                    }
                }
                else if (node is CollapsingRecordNodeItem)
                {
                    foreach (GoPort prt in (node as CollapsingRecordNodeItem).Ports)
                    {
                        if (prt.Location.X == (othernodesport as GoPort).Location.X)
                            return prt as IGoPort;
                    }
                }
                else if (node is ImageNode)
                {
                    foreach (GoPort prt in (node as ImageNode).Ports)
                    {
                        if (prt.Location.X == (othernodesport as GoPort).Location.X)
                            return prt as IGoPort;
                    }
                }
                else
                {
                    node.ToString();
                }
            }

            if (node is GraphNode)
                return (node as GraphNode).GetDefaultPort as IGoPort;
            else if (node is SubgraphNode)
                return (node as SubgraphNode).Port;
            else if (node != null && node.Ports.Count > 0)
            {
                try
                {
                    return node.Ports.GetEnumerator().Current as IGoPort;
                }
                catch
                {
                    try
                    {
                        node.Ports.MoveNext();
                        return node.Ports.GetEnumerator().Current as IGoPort;
                    }
                    catch
                    {

                    }
                }
            }

            return null;
        }

        private Collection<QuickPort> getquadrantports(GoNode node, bool left, bool top, bool includeLRTP, IGoPort othernodesport)
        {
            Collection<QuickPort> possiblePorts = new Collection<QuickPort>();

            PointF centerNode = node.Center;
            foreach (GoPort prt in node.Ports)
            {
                if (!(prt is QuickPort))
                    continue;

                QuickPort qprt = prt as QuickPort;
                if (qprt.IncomingLinksDirection != (othernodesport as QuickPort).IncomingLinksDirection || qprt.OutgoingLinksDirection != (othernodesport as QuickPort).OutgoingLinksDirection)
                    continue;

                if (!includeLRTP)
                    if (qprt.PortPosition == MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Top || qprt.PortPosition == MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Bottom || qprt.PortPosition == MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Left || qprt.PortPosition == MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Right)
                        continue;

                PointF centerPort = qprt.Center;

                #region quadrant Possibilities
                if (left)
                {
                    if (centerPort.X > centerNode.X)
                        continue;
                    if (top)
                    {
                        if (centerPort.Y > centerNode.Y)
                            continue;
                        possiblePorts.Add(qprt);
                    }
                    else
                    {
                        if (centerPort.Y < centerNode.Y)
                            continue;
                        possiblePorts.Add(qprt);
                    }
                }
                else
                {
                    if (centerPort.X < centerNode.X)
                        continue;
                    if (top)
                    {
                        if (centerPort.Y > centerNode.Y)
                            continue;
                        possiblePorts.Add(qprt);
                    }
                    else
                    {
                        if (centerPort.Y < centerNode.Y)
                            continue;
                        possiblePorts.Add(qprt);
                    }
                }
                #endregion

            }

            return possiblePorts;
        }

        private GoObject removeChildrenFromObject(GoGroup grp)
        {
            Collection<GoObject> remove = new Collection<GoObject>();
            foreach (GoObject cO in grp.GetEnumerator())
            {
                if (cO is IMetaNode)
                    remove.Add(cO);
            }
            foreach (GoObject o in remove)
            {
                grp.Remove(o);
                o.Remove();
            }
            return grp;
        }
        //F8
        private void AddContainerObjectsToFakeShallows(ILinkedContainer container, bool parent, bool includeChildren)
        {
            if (!parent)
            {
                //if (!includeChildren)
                //{
                //    removeChildrenFromObject(container as GoGroup);
                //}
                if (container is SubgraphNode)
                {
                    GoObject objCopy = (container as SubgraphNode).CopyAsShallow();
                    if ((container as SubgraphNode).Parent is SubgraphNode)
                        (objCopy as SubgraphNode).OriginalParent = (container as SubgraphNode).Parent as SubgraphNode;
                    FakeClipboardObjects.Add(objCopy);
                }
                else
                    FakeClipboardObjects.Add((container as GoObject).Copy());

            }

            if (container is SubgraphNode)
            {
                if (includeChildren)
                {
                    List<GoObject> actedOnAlready = new List<GoObject>();
                    foreach (GoObject o in (container as SubgraphNode).GetEnumerator())
                    {
                        GoObject obj = o;
                        GoObject objCopy = o.Copy();
                        if (obj is IMetaNode)
                        {
                            objCopy = (obj as IShallowCopyable).CopyAsShallow();
                        }
                        if (objCopy is ILinkedContainer)
                        {
                            AddContainerObjectsToFakeShallows(obj as ILinkedContainer, false, includeChildren);
                        }
                        if (objCopy is IShallowCopyable && !(objCopy is ILinkedContainer))
                        {
                            (objCopy as IShallowCopyable).MetaObject = (obj as IShallowCopyable).MetaObject;
                            (objCopy as IShallowCopyable).CopiedFrom = (obj as IShallowCopyable).MetaObject;
                            if (objCopy is IMetaNode) //ALWAYS IS?
                                (objCopy as IMetaNode).ParentIsILinkedContainer = true;
                            FakeClipboardObjects.Add(objCopy);
                            if (obj is IGoNode)
                            {
                                foreach (IGoLink lnk in (obj as IGoNode).Links)
                                {
                                    if (lnk is QLink)
                                    {
                                        try
                                        {
                                            if ((((lnk as QLink).ToNode as GoNode).Parent == container && ((lnk as QLink).FromNode as GoNode).Parent == container)
                                            || (FakeClipboardObjects.Contains(((lnk as QLink).ToNode as GoNode).Parent) && FakeClipboardObjects.Contains(((lnk as QLink).FromNode as GoNode).Parent)))
                                            {
                                                if (actedOnAlready.Contains(lnk as GoObject) || myView.Selection.Contains(lnk as GoObject))
                                                    continue;
                                                actedOnAlready.Add(lnk as GoObject);

                                                GoObject objCopylnk = (lnk as QLink).Copy();

                                                (objCopylnk as QLink).ToMetaBase = ((lnk as QLink).ToNode as IMetaNode).MetaObject;
                                                (objCopylnk as QLink).FromMetaBase = ((lnk as QLink).FromNode as IMetaNode).MetaObject;

                                                (objCopylnk as QLink).ToPortShallow = (lnk as QLink).ToPort;
                                                (objCopylnk as QLink).FromPortShallow = (lnk as QLink).FromPort;

                                                FakeClipboardObjects.Add(objCopylnk);

                                                //artefacts/rationales (require selection until writted)
                                            }
                                        }
                                        catch
                                        {
                                            container.ToString();
                                            //parent is null
                                        }
                                    }
                                }
                            }
                        }
                    }
                    actedOnAlready = null;
                }
            }
            else if (container is ValueChain)
            {
                foreach (GoObject o in (container as ValueChain).GetEnumerator())
                {
                    GoObject objCopy = o.Copy();
                    if (o is IMetaNode)
                        objCopy = (o as IShallowCopyable).CopyAsShallow();

                    if (objCopy is ILinkedContainer)
                    {
                        (objCopy as ILinkedContainer).MetaObject = (o as ILinkedContainer).MetaObject;
                        (objCopy as ILinkedContainer).ParentIsILinkedContainer = true;
                        AddContainerObjectsToFakeShallows(objCopy as ILinkedContainer, false, includeChildren);
                    }
                    if (objCopy is IShallowCopyable && !(objCopy is ILinkedContainer))
                    {
                        (objCopy as IShallowCopyable).MetaObject = (o as IShallowCopyable).MetaObject;
                        (objCopy as IShallowCopyable).CopiedFrom = (o as IShallowCopyable).MetaObject;
                        if (objCopy is IMetaNode) //ALWAYS IS?
                            (objCopy as IMetaNode).ParentIsILinkedContainer = true;
                        FakeClipboardObjects.Add(objCopy);
                    }
                }
            }
        }

        private void ViewController_MetaObjectContextRequest(MetaBase mbase, bool UseServer)
        {
            foreach (DockContent dcont in DockingForm.DockForm.dockPanel1.Documents)
            {
                LiteGraphViewContainer lcontainer = dcont as LiteGraphViewContainer;
                if (lcontainer != null && lcontainer.Tag == mbase)
                {
                    lcontainer.Focus();
                    return;
                }
            }

            DockingForm.DockForm.ShowMetaObjectProperties(null);
            //DockingForm.DockForm.ShowProperties(null);
            if (UseServer)
            {
                if (d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.MetaObjectProvider.GetBypkidMachine(mbase.pkid, mbase.MachineName) == null)
                {
                    MessageBox.Show(DockingForm.DockForm, "You must first check this object in to view its server context.", "Object does not exist remotely", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
            }
            LiteGraphViewContainer newContainer = new LiteGraphViewContainer();
            newContainer.UseServer = UseServer;
            newContainer.Setup(mbase);
            //newContainer.Show(this.DockHandler.PanelPane, DockAlignment.Bottom, 0.25);
            newContainer.Show(DockingForm.DockForm.dockPanel1, DockState.Document);
        }

        #region Container Events

        private void GraphViewContainer_Leave(object sender, EventArgs e)
        {
            //  if (timerAutoSave != null)
            //    timerAutoSave.Stop();

            if (myView.IsEditing)
                myView.DoEndEdit();
        }
        private void GraphViewContainer_Load(object sender, EventArgs e)
        {
            LoadPlugins();
            //  MessageBox.Show(DockingForm.DockForm, "DEBUG BUILD - SYNTHESIS MENU ITEM VISIBLE");
            validateModelToolStripMenuItem.Visible = true;
            //myView.Dispatch+=new MetaBuilder.Graphing.DispatchMessageHandler(myView_Dispatch);
            //myView.ObjectSelectionDropped += new GoObjectEventHandler(myView_ObjectSelectionDropped);
            // myView.Sheet.TopLeftMargin = myView.Sheet.BottomRightMargin = new SizeF(60, 60);
            myView.ObjectContextClicked += new GoObjectEventHandler(myView_ObjectContextClicked);
            //menuItemViewObjectImages.Visible = Core.Variables.Instance.IsDeveloperEdition;
        }

        private void myView_ObjectContextClicked(object sender, GoObjectEventArgs e)
        {
            myView.ViewContextMenu.Collapse -= ViewContextMenu_Collapse;
            myView.ViewContextMenu.Collapse += new EventHandler(ViewContextMenu_Collapse);
            if (myView.ViewContextMenu != null)
            {
                if (myView.ViewContextMenu.MenuItems.Count > 2)
                {
                    foreach (MenuItem item in myView.ViewContextMenu.MenuItems)
                    {
                        if (item.Text == "Add Shallow Copy to Clipboard")
                        {
                            item.Click += new EventHandler(ShallowCopyClipboard);
                            //break;
                        }
                        else if (item.Text == "&Cut")
                        {
                            item.Click += new EventHandler(ShallowCopyClipboardCut);
                            //break;
                        }
                        else if (item.Text == "Add Artefact")
                        {
                            foreach (MenuItem cItem in item.MenuItems)
                            {
                                if (cItem.Text == "Existing")
                                {
                                    cItem.Click += new EventHandler(AddExistingArtefact);
                                    //break;
                                }
                                else
                                {
                                    if (cItem.Tag is AllowedArtifact)
                                    {
                                        cItem.Click += new EventHandler(AddGenericArtefactWithNodeReturn);
                                    }
                                }
                            }
                        }
                        else if (item.Text == "Show Swimlane Bindings" && myView.Selection.Primary is MappingCell)
                        {
                            item.Click += new EventHandler(ShowSubgraphBinding);
                        }
                        else if (item.Text == "Edit Container Binding")
                        {
                            item.Click += new EventHandler(ShowSubgraphBinding);
                        }
                        else if (item.Text == "Accountability Matrix")
                        {
                            item.Click += new EventHandler(ShowActivityReport);
                        }
                    }

                    ////Hack into graphviewcontroller or actually out of it :)
                    //if (myView.ViewContextMenu.MenuItems[7].Text == "Add Shallow Copy to Clipboard")
                    //{
                    //    myView.ViewContextMenu.MenuItems[7].Click += new EventHandler(ShallowCopyClipboard);
                    //}
                }

#if DEBUG
                if (!Core.Variables.Instance.IsViewer)
                {
                    DataSet dc = DataAccessLayer.DataRepository.Provider.ExecuteDataSet(CommandType.Text, "SELECT * FROM ModelType");
                    foreach (DataRow row in dc.Tables[0].Rows)
                    {
                        MenuItem item = new MenuItem(row[0].ToString(), createDiagramType);
                        myView.ViewContextMenu.MenuItems.Add(item);
                    }
                }
#endif

                if ((myView.LastInput.Modifiers != (Keys.Shift | Keys.Control)))
                {
                    myView.ViewContextMenu.Show(myView, e.ViewPoint);
                }
            }
        }

        private void ViewContextMenu_Collapse(object sender, EventArgs e)
        {
            foreach (MenuItem item in myView.ViewContextMenu.MenuItems)
            {
                if (item.Text == "Add Shallow Copy to Clipboard")
                {
                    item.Click -= (ShallowCopyClipboard);
                    //break;
                }
                else if (item.Text == "&Cut")
                {
                    item.Click -= (ShallowCopyClipboardCut);
                    //break;
                }
                else if (item.Text == "Add Artefact")
                {
                    foreach (MenuItem cItem in item.MenuItems)
                    {
                        if (cItem.Text == "Existing")
                        {
                            cItem.Click -= (AddExistingArtefact);
                            //break;
                        }
                        else
                        {
                            if (cItem.Tag is AllowedArtifact)
                            {
                                cItem.Click -= (AddGenericArtefactWithNodeReturn);
                            }
                        }
                    }
                }
                else if (item.Text == "Show Swimlane Bindings" && myView.Selection.Primary is MappingCell)
                {
                    item.Click -= (ShowSubgraphBinding);
                }
                else if (item.Text == "Edit Container Binding")
                {
                    item.Click -= (ShowSubgraphBinding);
                }
                else if (item.Text == "Accountability Matrix")
                {
                    item.Click -= (ShowActivityReport);
                }
            }
        }

        private void createDiagramType(object sender, EventArgs e)
        {
            DockingForm.DockForm.CreateDiagram((sender as MenuItem).Text);
        }

        private void AddExistingArtefact(object sender, EventArgs e)
        {
            myView.Document.SkipsUndoManager = true;
            //load allowed artefacts from db
            Hashtable hashArtefacts = (sender as MenuItem).Tag as Hashtable;

            ObjectFinder ofinder = new ObjectFinder(false);
            ofinder.IncludeStatusCombo = true;
            ofinder.ExcludeStatuses.Add(VCStatusList.Obsolete);
            ofinder.ExcludeStatuses.Add(VCStatusList.MarkedForDelete);
            ofinder.ArtefactsOnly = true;

            foreach (DictionaryEntry kvp in hashArtefacts)
            {
                ofinder.LimitToClass += kvp.Key.ToString() + "|";
            }

            DialogResult res = ofinder.ShowDialog(DockingForm.DockForm);
            if (res == DialogResult.OK)
            {
                Dictionary<MetaObjectKey, MetaBase> objects = ofinder.SelectedObjects;
                ShapeDesignController controller = new ShapeDesignController(myView);
                foreach (KeyValuePair<MetaObjectKey, MetaBase> kvpair in objects)
                {
                    QLink lnk = myView.SelectedLinks[0];
                    GoGroup grp = lnk.MidLabel as GoGroup;
                    ArtefactNode afNode = new ArtefactNode();
                    afNode.MetaObject = kvpair.Value;
                    afNode.HookupEvents();
                    afNode.Location = new PointF(myView.LastInput.DocPoint.X - 10, myView.LastInput.DocPoint.Y - 10);//new PointF(grp.Location.X, grp.Location.Y - 10); //
                    afNode.Label.Text = kvpair.Value.ToString();
                    afNode.BindingInfo = new BindingInfo();
                    afNode.BindingInfo.BindingClass = kvpair.Value._ClassName;
                    afNode.Editable = true;
                    afNode.Label.Editable = false;
                    myView.Document.Add(afNode);
                    //The current selection should only have a link in it
                    foreach (GoObject o in grp)
                    {
                        if (o is FishLinkPort)
                        {
                            // now link 'em
                            FishLink fishlink = new FishLink();
                            fishlink.FromPort = afNode.Port;
                            FishLinkPort flp = o as FishLinkPort;
                            //flp.IsValidFrom = false;  // can only draw links to this port, not from it
                            //flp.ToSpot = NoSpot;
                            fishlink.ToPort = flp;
                            fishlink.Visible = ViewController.ArtefactPointersVisible;
                            myView.Document.Add(fishlink);
                        }
                    }
                }
            }

            myView.Document.SkipsUndoManager = false;

        }
        private void AddGenericArtefactWithNodeReturn(object sender, EventArgs e)
        {
            AllowedArtifact AA = (sender as MenuItem).Tag as AllowedArtifact;

            ArtefactNode art = ViewController.AddGenericArtefactWithNodeReturn(AA);
            DockingForm.DockForm.ShowMetaObjectProperties(art.MetaObject);
            DockingForm.DockForm.m_metaPropsWindow.BringToFront();
            DockingForm.DockForm.m_metaPropsWindow.Activate();
            DockingForm.DockForm.m_metaPropsWindow.Focus();
            SendKeys.Send("{TAB}");
        }
        private void ShowSubgraphBinding(object sender, EventArgs e)
        {
            if ((myView.Selection.Primary is MappingCell))
            {
                if (VCStatusTool.UserHasControl((myView.Selection.Primary as MappingCell).MetaObject))
                    ShowSubgraphBindingForm(myView.Selection.Primary as MappingCell);
            }
            else if ((myView.Selection.Primary is SubgraphNode))
            {
                if (VCStatusTool.UserHasControl((myView.Selection.Primary as SubgraphNode).MetaObject))
                    ShowSubgraphBindingForm(myView.Selection.Primary as SubgraphNode);
            }
            else if ((myView.Selection.Primary is ValueChain))
            {
                if (VCStatusTool.UserHasControl((myView.Selection.Primary as ValueChain).MetaObject))
                    ShowSubgraphBindingForm(myView.Selection.Primary as ValueChain);
            }
        }
        private void ShowActivityReport(object sender, EventArgs e)
        {
            Form displayForm = new Form();
            displayForm.Text = "Accountability Matrix";
            displayForm.StartPosition = FormStartPosition.CenterScreen;
            displayForm.Width = 800;
            displayForm.Height = 600;
            DataGridView grid = new DataGridView();
            grid.ReadOnly = true;
            grid.Dock = DockStyle.Fill;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;

            displayForm.Controls.Add(grid);
            displayForm.Show();
            Application.DoEvents();
            try
            {
                //Rationale?-->position/role/orgunit-->Activity/Gateway[<--Software]
                DataTable table = new DataTable("AccountabilityMatrix");
                table.Columns.Add("Position"); //position/role/orgunit/gateway
                table.Columns.Add("Process"); //current diagram (as process)
                table.Columns.Add("Activity/Decision"); //activity/gateway
                table.Columns.Add("Is Decision"); //if gateway
                table.Columns.Add("Automated"); //direct link to software from activity/gateway
                table.Columns.Add("RACI"); //Accountable if has rationale or is only position

                table.Columns.Add("Direction"); //The association type of the artefact
                table.Columns.Add("InformationArtefact"); //The activity is linked to information artefact
                table.Columns.Add("Artefact"); //The artefact

                table.Columns.Add("Start"); //The association type of the artefact (DynamicFlow)
                table.Columns.Add("Decision/Activity"); //The activity is the parent of an activity/decision
                table.Columns.Add("Decision"); //The artefact

                if (sender is MenuItem)
                {
                    table = GenerateActivityReport(table, this);
                }
                else if (sender is ToolStripMenuItem)
                {
                    foreach (IDockContent content in DockingForm.DockForm.dockPanel1.Documents)
                    {
                        if (content is GraphViewContainer)
                        {
                            table = GenerateActivityReport(table, (content as GraphViewContainer));
                        }
                    }
                }

                grid.DataSource = table;
                grid.SelectAll();

                Clipboard.SetDataObject(grid.GetClipboardContent());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private DataTable GenerateActivityReport(DataTable table, GraphViewContainer container)
        {
            List<IMetaNode> allIMetaNodes = container.ViewController.GetIMetaNodes();
            foreach (GoObject o in container.View.Document)
            {
                if (!(o is IMetaNode))
                    continue;
                IMetaNode docNode = o as IMetaNode;
                //collect activity's data
                if (docNode.MetaObject.Class == "Activity" || docNode.MetaObject.Class == "Gateway")
                {
                    Collection<IMetaNode> Softwares = new Collection<IMetaNode>();
                    Dictionary<IMetaNode, Tuple> InformationArtefacts = new Dictionary<IMetaNode, Tuple>();
                    Dictionary<IMetaNode, IMetaNode> ActivityGatewayFlows = new Dictionary<IMetaNode, IMetaNode>();
                    Dictionary<IMetaNode, IMetaNode> PositionRationales = new Dictionary<IMetaNode, IMetaNode>();
                    bool position = false;
                    foreach (GoObject docNodeLink in (docNode as GoNode).Links)
                    {
                        IMetaNode linkedObjectNode = null;
                        bool fromDocNode = false;
                        if ((docNodeLink as GoLabeledLink).ToNode == docNode)
                        {
                            linkedObjectNode = (docNodeLink as GoLabeledLink).FromNode as IMetaNode;
                        }
                        else if ((docNodeLink as GoLabeledLink).FromNode == docNode)
                        {
                            fromDocNode = true;
                            linkedObjectNode = (docNodeLink as GoLabeledLink).ToNode as IMetaNode;
                        }
                        if (linkedObjectNode == null)
                            continue;

                        //Position direction does not matter
                        if (linkedObjectNode.MetaObject.Class == "Position" || linkedObjectNode.MetaObject.Class == "OrganizationalUnit" || linkedObjectNode.MetaObject.Class == "Role")
                        {
                            position = true;
                            bool added = false;
                            foreach (IMetaNode inode in allIMetaNodes)
                            {
                                if (inode is Rationale)
                                {
                                    if ((inode as Rationale).Anchor == linkedObjectNode)
                                    {
                                        if (!PositionRationales.ContainsKey(linkedObjectNode))
                                        {
                                            PositionRationales.Add(linkedObjectNode, inode);
                                        }
                                        else
                                        {
                                            linkedObjectNode.ToString();
                                        }
                                        added = true;
                                        break;
                                    }
                                }
                            }
                            if (!added)
                                if (!PositionRationales.ContainsKey(linkedObjectNode))
                                {
                                    PositionRationales.Add(linkedObjectNode, null);
                                }
                                else
                                {
                                    linkedObjectNode.ToString();
                                }
                        }
                        else if (linkedObjectNode.MetaObject.Class == "PhysicalSoftwareComponent")
                        {
                            Softwares.Add(linkedObjectNode);
                        }
                        else if (linkedObjectNode.MetaObject.Class == "PhysicalInformationArtefact")
                        {
                            //create // read?
                            if ((docNodeLink as QLink).AssociationType == LinkAssociationType.DynamicFlow || (docNodeLink as QLink).AssociationType == LinkAssociationType.Create || (docNodeLink as QLink).AssociationType == LinkAssociationType.Read || (docNodeLink as QLink).AssociationType == LinkAssociationType.Update)
                            {
                                bool infArt = false;
                                foreach (IMetaNode docArtifactNode in (docNodeLink as QLink).GetArtefacts())
                                {
                                    if (docArtifactNode.MetaObject.Class == "FlowDescription")
                                    {
                                        if (!InformationArtefacts.ContainsKey(linkedObjectNode))
                                        {
                                            InformationArtefacts.Add(linkedObjectNode, new Tuple(docArtifactNode as ArtefactNode, fromDocNode ? "Activity/Decision " + (docNodeLink as QLink).AssociationType.ToString() + "s" : "Activity/Decision " + (docNodeLink as QLink).AssociationType.ToString() + "s"));
                                        }
                                        else
                                        {
                                            try
                                            {
                                                InformationArtefacts.Add((linkedObjectNode as GoObject).Copy() as IMetaNode, new Tuple(docArtifactNode as ArtefactNode, fromDocNode ? "Activity/Decision " + (docNodeLink as QLink).AssociationType.ToString() + "s" : "Activity/Decision " + (docNodeLink as QLink).AssociationType.ToString() + "s"));
                                            }
                                            catch
                                            {
                                                linkedObjectNode.ToString();
                                            }
                                        }
                                        infArt = true;
                                        break;
                                    }
                                }
                                if (!infArt)
                                    if (!InformationArtefacts.ContainsKey(linkedObjectNode))
                                    {
                                        InformationArtefacts.Add(linkedObjectNode, new Tuple(null, fromDocNode ? "Activity/Decision " + (docNodeLink as QLink).AssociationType.ToString() + "s" : "Activity/Decision " + (docNodeLink as QLink).AssociationType.ToString() + "s"));
                                    }
                                    else
                                    {
                                        linkedObjectNode.ToString();
                                    }
                            }
                        }
                        //secondary decision direction must be out of activity/decision
                        else if (linkedObjectNode.MetaObject.Class == "Activity" || linkedObjectNode.MetaObject.Class == "Gateway")
                        {
                            if (fromDocNode) //linkedObjectNode == (docNodeLink as QLink).ToNode ||
                            {
                                bool actGateFlow = false;
                                foreach (IMetaNode docArtifactNode in (docNodeLink as QLink).GetArtefacts())
                                {
                                    if (docArtifactNode.MetaObject.Class == "FlowDescription")
                                    {
                                        if (!ActivityGatewayFlows.ContainsKey(linkedObjectNode))
                                        {
                                            ActivityGatewayFlows.Add(linkedObjectNode, docArtifactNode as ArtefactNode);
                                        }
                                        else
                                        {
                                            linkedObjectNode.ToString();
                                        }
                                        actGateFlow = true;
                                        break;
                                    }
                                }
                                if (!actGateFlow)
                                {
                                    if (!ActivityGatewayFlows.ContainsKey(linkedObjectNode))
                                    {
                                        ActivityGatewayFlows.Add(linkedObjectNode, null);
                                    }
                                    else
                                    {
                                        linkedObjectNode.ToString();
                                    }
                                }
                            }
                        }
                    }
                    //add rows for this activity
                    foreach (KeyValuePair<IMetaNode, IMetaNode> positionRationaleKey in PositionRationales)
                    {
                        DataRow row = null;
                        if (Softwares.Count > 0)
                        {
                            foreach (IMetaNode software in Softwares)
                            {
                                bool infArtefact = false;
                                foreach (KeyValuePair<IMetaNode, Tuple> informationArtefact in InformationArtefacts)
                                {
                                    infArtefact = true;
                                    row = table.NewRow();

                                    row["Position"] = positionRationaleKey.Key.MetaObject.ToString(); //position/role/orgunit
                                    row["Process"] = container.TabText; //current diagram (as process)
                                    row["Activity/Decision"] = docNode.MetaObject.ToString(); ; //activity/gateway
                                    row["Is Decision"] = docNode.MetaObject.Class == "Activity" ? "N" : "Y"; //if gateway
                                    row["Automated"] = software.MetaObject.ToString(); ; //direct link to software from activity/gateway
                                    row["RACI"] = (positionRationaleKey.Value != null && positionRationaleKey.Value.MetaObject.ToString().Contains("Lead")) || PositionRationales.Count == 1 ? "Accountable" : "Responsible"; //Accountable if has rationale or is only position

                                    row["InformationArtefact"] = informationArtefact.Key.MetaObject.ToString(); ; //The activity is linked to information artefact
                                    row["Direction"] = informationArtefact.Value.Second.ToString(); //The association type of the artefact
                                    row["Artefact"] = informationArtefact.Value.First != null ? (informationArtefact.Value.First as IMetaNode).MetaObject.ToString() : ""; //The artefact

                                    table = AddExtraActivityDecision(table, row, ActivityGatewayFlows);

                                    //table.Rows.Add(row);
                                }
                                if (!infArtefact)
                                {
                                    row = table.NewRow();

                                    row["Position"] = positionRationaleKey.Key.MetaObject.ToString(); //position/role/orgunit
                                    row["Process"] = container.TabText; //current diagram (as process)
                                    row["Activity/Decision"] = docNode.MetaObject.ToString(); ; //activity/gateway
                                    row["Is Decision"] = docNode.MetaObject.Class == "Activity" ? "N" : "Y"; //if gateway
                                    row["Automated"] = software.MetaObject.ToString(); ; //direct link to software from activity/gateway
                                    row["RACI"] = (positionRationaleKey.Value != null && positionRationaleKey.Value.MetaObject.ToString().Contains("Lead")) || PositionRationales.Count == 1 ? "Accountable" : "Responsible"; //Accountable if has rationale or is only position

                                    table = AddExtraActivityDecision(table, row, ActivityGatewayFlows);
                                    //table.Rows.Add(row);
                                }
                            }
                        }
                        else
                        {
                            bool infArtefact = false;
                            foreach (KeyValuePair<IMetaNode, Tuple> informationArtefact in InformationArtefacts)
                            {
                                infArtefact = true;
                                row = table.NewRow();

                                row["Position"] = positionRationaleKey.Key.MetaObject.ToString(); //position/role/orgunit
                                row["Process"] = container.TabText; //current diagram (as process)
                                row["Activity/Decision"] = docNode.MetaObject.ToString(); ; //activity/gateway
                                row["Is Decision"] = docNode.MetaObject.Class == "Activity" ? "N" : "Y"; //if gateway
                                row["Automated"] = ""; //direct link to software from activity/gateway
                                row["RACI"] = (positionRationaleKey.Value != null && positionRationaleKey.Value.MetaObject.ToString().Contains("Lead")) || PositionRationales.Count == 1 ? "Accountable" : "Responsible"; //Accountable if has rationale or is only position

                                row["InformationArtefact"] = informationArtefact.Key.MetaObject.ToString(); ; //The activity is linked to information artefact
                                row["Direction"] = informationArtefact.Value.Second.ToString(); //The association type of the artefact
                                row["Artefact"] = informationArtefact.Value.First != null ? (informationArtefact.Value.First as IMetaNode).MetaObject.ToString() : ""; //The artefact

                                table = AddExtraActivityDecision(table, row, ActivityGatewayFlows);
                                //table.Rows.Add(row);
                            }
                            if (!infArtefact)
                            {
                                row = table.NewRow();

                                row["Position"] = positionRationaleKey.Key.MetaObject.ToString(); //position/role/orgunit
                                row["Process"] = container.TabText; //current diagram (as process)
                                row["Activity/Decision"] = docNode.MetaObject.ToString(); ; //activity/gateway
                                row["Is Decision"] = docNode.MetaObject.Class == "Activity" ? "N" : "Y"; //if gateway
                                row["Automated"] = ""; //direct link to software from activity/gateway
                                row["RACI"] = (positionRationaleKey.Value != null && positionRationaleKey.Value.MetaObject.ToString().Contains("Lead")) || PositionRationales.Count == 1 ? "Accountable" : "Responsible"; //Accountable if has rationale or is only position

                                table = AddExtraActivityDecision(table, row, ActivityGatewayFlows);
                                //table.Rows.Add(row);
                            }
                        }
                    }

                    if (!position) //activities without a position
                    {
                        DataRow rowX = table.NewRow();

                        rowX["Position"] = ""; //position/role/orgunit
                        rowX["Process"] = container.TabText; //current diagram (as process)
                        rowX["Activity/Decision"] = docNode.MetaObject.ToString(); ; //activity/gateway
                        rowX["Is Decision"] = docNode.MetaObject.Class == "Activity" ? "N" : "Y"; //if gateway
                        rowX["Automated"] = ""; //direct link to software from activity/gateway
                        rowX["RACI"] = ""; //Accountable if has rationale or is only position

                        table = AddExtraActivityDecision(table, rowX, ActivityGatewayFlows);
                        //table.Rows.Add(rowX);
                    }
                }
            }
            return table;
        }
        private DataTable AddExtraActivityDecision(DataTable Table, DataRow Row, Dictionary<IMetaNode, IMetaNode> ActivityDecisions)
        {
            bool added = false;
            foreach (KeyValuePair<IMetaNode, IMetaNode> ActivityDecision in ActivityDecisions)
            {
                added = true;
                DataRow newRow = Table.NewRow();

                newRow["Position"] = Row["Position"];
                newRow["Process"] = Row["Process"];
                newRow["Activity/Decision"] = Row["Activity/Decision"];
                newRow["Is Decision"] = Row["Is Decision"];
                newRow["Automated"] = Row["Automated"];
                newRow["RACI"] = Row["RACI"];

                newRow["InformationArtefact"] = Row["InformationArtefact"];
                newRow["Direction"] = Row["Direction"];
                newRow["Artefact"] = Row["Artefact"];

                newRow["Start"] = "Activity/Decision starts"; //The association type of the artefact (DynamicFlow)
                newRow["Decision/Activity"] = ActivityDecision.Key.MetaObject.ToString(); //The activity is the parent of an activity/decision
                newRow["Decision"] = ActivityDecision.Value != null ? ActivityDecision.Value.MetaObject.ToString() : ""; //The artefact

                Table.Rows.Add(newRow);
            }
            if (!added)
                Table.Rows.Add(Row);
            return Table;
        }

        private void ShallowCopyClipboard(object sender, EventArgs e)
        {
            KeyBoardShallowCopy();
        }
        private void ShallowCopyClipboardCut(object sender, EventArgs e)
        {
            GoCollection col = new GoCollection();
            GoCollection colToSelect = new GoCollection();
            GoCollection selToMfFD = new GoCollection();
            foreach (GoObject o in myView.Selection)
            {
                //if (o.ParentNode == o)
                col.Add(o);
                //else
                //    col.Add(o.ParentNode);
            }

            //Mark all affected associations on diagram for delete
            GoCollection colNodes = new GoCollection();
            foreach (GoObject o in col)
            {
                if (o is GraphNode)
                {
                    foreach (QLink l in (o as GraphNode).Links)
                    {
                        if (!myView.Selection.Contains(l))
                            selToMfFD.Add(l);
                    }
                    colToSelect.Add(o);
                }
                else if (o is CollapsingRecordNodeItem || o is RepeaterSection || o is CollapsingRecordNodeItemList)
                {
                    //do not select this object to be shallow copied
                    //otherwise it can be pasted on its own
                    //or
                    //add its parentnode to the list

                    //get nodes
                    if (o is CollapsingRecordNodeItem)
                    {
                        CollapsingRecordNodeItem childNode = o as CollapsingRecordNodeItem;
                        CollapsibleNode parentNode = childNode.ParentList.ParentNode as CollapsibleNode;
                        colNodes.Add(parentNode);
                        if (childNode.MetaObject.pkid > 0 && parentNode.MetaObject.pkid > 0)
                        {
                            //MFD association and inverse thereof
                            LinkController.InverseMarkForDelete(LinkController.GetAssociation(childNode.MetaObject, parentNode.MetaObject, LinkAssociationType.Mapping));
                            LinkController.InverseMarkForDelete(LinkController.GetAssociation(parentNode.MetaObject, childNode.MetaObject, LinkAssociationType.Mapping));
                        }
                    }
                }
                else
                {
                    colToSelect.Add(o);
                }
            }
            ViewController.ModifyObjects(selToMfFD, true, null);

            //add shallows
            myView.Selection.Clear();
            myView.Selection.AddRange(colToSelect);
            KeyBoardShallowCopy();

            foreach (GoObject o in col)
                o.Remove();

            ViewController.ForceComputeCollectionBounds(colNodes);
        }

        private void GraphViewContainer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (closeAfterSave || isClosing)
            {
                return;
            }

            IsClosing = true;
            if (DockingForm.DockForm.OpenedFiles != null)
                DockingForm.DockForm.OpenedFiles.Remove(myView.Document.Name.ToLower());

            DockingForm.DockForm.CloseValidationWindow(myView.Document);
            DockingForm.DockForm.ShowMetaObjectProperties(null);

            if (measure != null)
            {
                try
                {
                    measure.Hide();
                    measure.Close();
                    if (!measure.IsDisposed)
                        measure.Dispose();
                }
                catch
                {
                }
            }

            //DockingForm.DockForm.ShowProperties(null);

            if (DockingForm.DockForm.ForceCloseCancel)
            {
                IsClosing = false;
                return;
            }

            //if (DockingForm.DockForm.OpenedFiles != null)
            //    DockingForm.DockForm.OpenedFiles.Remove(myView.Document.Name.ToLower());

            //DockingForm.DockForm.CloseValidationWindow(myView.Document);
            //DockingForm.DockForm.ShowMetaObjectProperties(null);
            //DockingForm.DockForm.ShowProperties(null);

            //if (!ReadOnly)
            //    DeleteAutosaveFile();

            if (timerAutoSave != null)
                if (timerAutoSave.Enabled)
                {
                    timerAutoSave.Stop();
                    timerAutoSave = null;
                }

            if (IsSavingFromMDIParent)
            {
                //ignore
            }
            else
            {
                if (e.CloseReason == CloseReason.MdiFormClosing)
                {
                    DockingForm.DockForm.UpdateMenuItems();
                    UpdateReadonly();
                    return;
                }
                if (!ReadOnly)
                {
                    //if (this.myView.Document.IsModified)
                    //{
                    //CustomBox b = new CustomBox();
                    //DialogResult res = b.ShowGraphViewClosingSaveDialog(myView.Doc.FileType.ToString());

                    //if (ForceSaveAs)
                    //{
                    //    System.Windows.Forms.DialogResult res =
                    //        MessageBox.Show(DockingForm.DockForm,"The " + myView.Doc.FileType.ToString() + " must be saved to maintain data integrity. Do you want to save now?",
                    //            "Save changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    //    if (res == DialogResult.Yes)
                    //    {
                    //        IsSavingFromItself = true;
                    //        DockingForm.DockForm.SaveGraphViewContainerInnards(this, true);
                    //        DockingForm.DockForm.UpdateMenuItems();
                    //        UpdateReadonly();
                    //        e.Cancel = true;
                    //        //base.OnFormClosing(e);
                    //    }
                    //    else if (res == DialogResult.Cancel || res == DialogResult.No)
                    //    {
                    //        e.Cancel = true;
                    //        IsClosing = false;
                    //    }
                    //}
                    //else
                    //{

                    System.Windows.Forms.DialogResult res = MessageBox.Show(DockingForm.DockForm, "The " + myView.Doc.FileType.ToString() + " has changed. Do you want to save changes?", "Save changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (res == DialogResult.Yes)
                    {
                        CloseAfterSave = true;
                        IsSavingFromItself = true;
                        DockingForm.DockForm.SaveGraphViewContainerInnards(this, true);
                        DockingForm.DockForm.UpdateMenuItems();
                        UpdateReadonly();
                        e.Cancel = false;
                        //base.OnFormClosing(e);
                    }
                    else if (res == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        IsClosing = false;
                    }
                    //}

                    //}
                    //DockingForm.DockForm.GetTaskDocker().ClearTasks();
                }
            }
            if (e.Cancel)
                DockingForm.DockForm.OpenedFiles.Add(myView.Document.Name.ToLower(), this.containerID);
            DockingForm.DockForm.UpdateMenuItems();
            UpdateReadonly();
        }
        private void GraphViewContainer_FormClosed(object sender, FormClosedEventArgs e)
        {
            DockingForm.DockForm.UpdateMenuItems();
        }

        private void MarkAsDuplicates(GoCollection col)
        {
            if (Core.Variables.Instance.CheckDuplicates) //3 September 2013 : Should really make use of variables if they are there
            {
                //if (ItemsHaveBeenAddeding && col.Count > 100) //4 August 2015 paste Limit
                //    return;
                foreach (GoObject o in col)
                {
                    if (o is IMetaNode)
                    {
                        IMetaNode imnode = o as IMetaNode;
                        FindDuplicates(imnode); //runs each as a thread
                    }
                }
            }
        }
        //remove this checker and if there are no active chckers then bind
        private void BindDuplicateList(Checker checker)
        {
            try
            {
                ActiveDuplicateCheckers.Remove(checker);
                checker.DuplicatesFound -= duplicateChecker_DuplicatesFound;
                checker.NoDuplicatesFound -= duplicateChecker_NoDuplicatesFound;

                if (ActiveDuplicateCheckers.Count == 0)
                    DockingForm.DockForm.GetTaskDocker().BindToList(ContainerID.ToString());
                //else
                //    ActiveDuplicateCheckers.ToString();
            }
            catch
            {
            }
        }
        private void duplicateChecker_DuplicatesFound(object sender, List<MetaBase> list)
        {
            Checker duplicateChecker = sender as Checker;
            DisableMetaPropertiesOnSelection = true;
            DuplicationTask dupeTask = new DuplicationTask();
            dupeTask.Tag = duplicateChecker.CreatedObject;
            dupeTask.MyView = myView;
            dupeTask.ContainerID = ContainerID.ToString();
            dupeTask.Matches = list;
            dupeTask.MyGoObject = duplicateChecker.NodeThatIsBeingChecked;
            DockingForm.DockForm.GetTaskDocker().AddTask(ContainerID.ToString(), dupeTask);
            IMetaNode node = dupeTask.MyGoObject as IMetaNode;
            node.RequiresAttention = true;
            DisableMetaPropertiesOnSelection = false;
            BindDuplicateList(duplicateChecker);
        }
        private void duplicateChecker_NoDuplicatesFound(object sender)
        {
            Checker duplicateChecker = sender as Checker;
            duplicateChecker.NodeThatIsBeingChecked.RequiresAttention = false;

            Checker existingDuplicateChecker = null;
            duplicateCheckers.TryGetValue(duplicateChecker.NodeThatIsBeingChecked, out existingDuplicateChecker);
            if (existingDuplicateChecker != null)
            {
                DockingForm.DockForm.GetTaskDocker().RemoveTaskWhereTagEquals(this.ContainerID.ToString(), duplicateChecker.CreatedObject, typeof(DuplicationTask));
                duplicateCheckers.Remove(duplicateChecker.NodeThatIsBeingChecked);
            }
            BindDuplicateList(duplicateChecker);
        }
        protected override void OnActivated(EventArgs e)
        {
            if (DocumentVersion != null)
                DockingForm.DockForm.SetToolstripWorkspaceToDiagram(DocumentVersion);
            else
                DockingForm.DockForm.SetToolstripToGlobalVarWorkspace("Diagram");

            base.OnActivated(e);
        }

        #endregion

        public override string Text
        {
            get { return base.Text; }
            set
            {
                value = System.Text.RegularExpressions.Regex.Replace(value, "(?<!&)&(?!&)", "&&");
                base.Text = value;//.Replace("&", "&&");
                TabText = value;//.Replace("&", "&&");
            }
        }

        private void myDialog_SheetChanged(object sender, EventArgs e)
        {
            GoSheet sheet = sender as GoSheet;
            myView.DocScale = sheet.View.DocScale;
            if (sheet.View.PrintScale > 0)
                myView.PrintScale = sheet.View.PrintScale;
            else
            {
                sheet.View.PrintScale = 1;
                myView.PrintScale = 1;
            }
            myView.Sheet.Bounds = sheet.Bounds;
            myView.Sheet.TopLeftMargin = sheet.TopLeftMargin;
            myView.Sheet.BottomRightMargin = sheet.BottomRightMargin;
        }

        #region Menu Events

        #region File Menu EventHandling

        private void menuItemFileSave_Click(object sender, EventArgs e)
        {
            if (!ReadOnly)
            {
                StartSaveProcess(false);
            }
            else if (Core.Variables.Instance.IsViewer)
            {
                StartSaveProcess(false);
            }
        }
        private void menuItemFileSaveAs_Click(object sender, EventArgs e)
        {
            if (!ReadOnly)
                StartSaveProcess(true);
        }

        private void menuItemFilePrint_Click(object sender, EventArgs e)
        {
            //#if DEBUG
            //            myView.Print();
            //            DockingForm.DockForm.DisplayTip("Original Printing Cancelled", "DEBUG Print");
            //            return;
            //#endif

            float originalScale = myView.DocScale;
            try
            {
                MyPrintDialog myDialog = new MyPrintDialog(ReadOnly);
                //myDialog.ReadOnly = ;
                myDialog.Document = myView.Document;

                //if (myDialog.DialogResult != DialogResult.Cancel)
                //{
                myDialog.SheetChanged += new EventHandler(myDialog_SheetChanged);

                myDialog.ShowDialog(DockingForm.DockForm);
                myView.DocScale = originalScale;

                myDialog.SheetChanged -= myDialog_SheetChanged;

                myDialog.Dispose();

                //}
            }
            catch (Exception xx)
            {
                LogEntry entry = new LogEntry();
                entry.Title = "Printing Error";
                entry.Message = xx.ToString() + "\n" + xx.Message.ToString() + "\n" + xx.StackTrace.ToString();
                Logger.Write(entry);
                MessageBox.Show(DockingForm.DockForm, "An error occurred when running the Print command. Please ensure that you have printers installed and that their drivers are up to date. This error has been logged.");
            }
        }
        private void menuItemFileClose_Click(object sender, EventArgs e)
        {
            DockingForm.DockForm.OpenedFiles.Remove(myView.Document.Name.ToLower());
            Close();
        }
        private void menuItemFileCloseAll_Click(object sender, EventArgs e)
        {
            //CancelEventArgs args = new CancelEventArgs();
            DockingForm.DockForm.CloseAllDocuments();
        }

        #endregion

        #region Edit Menu EventHandling

        private void menuItemEditUndo_Click(object sender, EventArgs e)
        {
            if (myView.Document.UndoManager != null)
                myView.Document.UndoManager.Undo();
            //AddIndicators();
        }
        private void menuItemEditRedo_Click(object sender, EventArgs e)
        {
            if (myView.Document.UndoManager != null)
                myView.Document.UndoManager.Redo();
            // AddIndicators();
        }
        private void menuItemEditCut_Click(object sender, EventArgs e)
        {
            ViewController.removeQuickMenu();
            ShallowCopyClipboardCut(sender, e);

            //myView.EditCut();
            //DockingForm.DockForm.ShallowGoObjects = new List<GoObject>();
            //DockingForm.DockForm.ShallowCopies = new List<IShallowCopyable>();
        }
        private void menuItemEditCopy_Click(object sender, EventArgs e)
        {
            try
            {
                ViewController.removeQuickMenu();
                myView.EditCopy();
                DockingForm.DockForm.ShallowGoObjects = new Collection<GoObject>();
                DockingForm.DockForm.ShallowCopies = new Collection<IShallowCopyable>();
            }
            catch
            {
            }
        }
        private void menuItemEditPaste_Click(object sender, EventArgs e)
        {
            if (FakeClipboardObjects.Count <= 0)
                myView.EditPaste();
            else
                ItemsPasted();

            Notify();
        }

        public Collection<GoObject> FakeClipboardObjects
        {
            get
            {
                return DockingForm.DockForm.ShallowGoObjects != null ? DockingForm.DockForm.ShallowGoObjects : new Collection<GoObject>();
            }
            set { DockingForm.DockForm.ShallowGoObjects = value; }
        }
        private void ItemsPasted()
        {
            ViewController.pastingShallow = true;
            ViewController.removeQuickMenu();
            if (!ReadOnly)
            {
                if (FakeClipboardObjects != null)
                    if (FakeClipboardObjects.Count > 0)
                    {
                        myView.Document.SkipsUndoManager = false;

                        List<IMetaNode> nodes = new List<IMetaNode>();
                        Collection<GoObject> translateObjects = new Collection<GoObject>();
                        Collection<ArtefactNode> artefactNodes = new Collection<ArtefactNode>();
                        Collection<Rationale> rationaleNodes = new Collection<Rationale>();
                        List<IMetaNode> nodesInsideContainers = new List<IMetaNode>();
                        Collection<QLink> links = new Collection<QLink>();
                        GoCollection refreshNodes = new GoCollection();
                        Collection<GoObject> removeTheseObjects = new Collection<GoObject>();

                        myView.StartTransaction();
                        //myView.BeginUpdate();

                        try
                        {
                            #region Nodes
                            foreach (GoObject iObj in FakeClipboardObjects)
                            {
                                if (iObj is IMetaNode && !(iObj is ArtefactNode) && !(iObj is Rationale))
                                {
                                    try
                                    {
                                        if ((iObj as IMetaNode).ParentIsILinkedContainer || (iObj as GoObject).Parent is ILinkedContainer || (iObj is SubgraphNode && (iObj as SubgraphNode).OriginalParent is ILinkedContainer))
                                        {
                                            nodesInsideContainers.Add((iObj as IMetaNode));
                                            continue;
                                        }
                                    }
                                    catch (Exception exI)
                                    {
                                        Console.WriteLine(exI.ToString());
                                    }

                                    if (iObj is CollapsibleNode)
                                    {
                                        foreach (RepeaterSection sect in (iObj as CollapsibleNode).RepeaterSections)
                                        {
                                            foreach (GoObject child in sect)
                                            {
                                                if (child is CollapsingRecordNodeItem)
                                                {
                                                    CollapsingRecordNodeItem childItem = child as CollapsingRecordNodeItem;
                                                    childItem.MetaObject = childItem.CopiedFrom;
                                                    nodes.Add(childItem);
                                                }
                                            }
                                        }

                                        //refreshNodes.Add(iObj);
                                    }

                                    if (iObj is SubgraphNode && ((iObj as SubgraphNode).Parent is ILinkedContainer || (iObj as SubgraphNode).OriginalParent is ILinkedContainer))
                                    {
                                        //this subgraph node has already been 'copied' into its parent, however it is not correctly depicted there
                                        //this will be used as a reference later coupled with containers in [nodes] and nodesInsideContainers to calculate which one is which based on location
                                        nodes.Add(iObj as IMetaNode);
                                    }
                                    else
                                    {
                                        myView.Document.Add(iObj);
                                        (iObj as IMetaNode).HookupEvents();
                                        (iObj as IMetaNode).BindToMetaObjectProperties();
                                        nodes.Add(iObj as IMetaNode);
                                    }
                                }

                                if (iObj is ArtefactNode)
                                {
                                    if ((iObj as ArtefactNode).ParentIsILinkedContainer)
                                    {
                                        nodesInsideContainers.Add((iObj as IMetaNode));
                                        continue;
                                    }

                                    myView.Document.Add(iObj);
                                    (iObj as ArtefactNode).HookupEvents();
                                    (iObj as ArtefactNode).BindToMetaObjectProperties();
                                    artefactNodes.Add(iObj as ArtefactNode);

                                    nodes.Add(iObj as IMetaNode);
                                }

                                if (iObj is Rationale)
                                {
                                    if ((iObj as Rationale).ParentIsILinkedContainer)
                                    {
                                        nodesInsideContainers.Add((iObj as IMetaNode));
                                        continue;
                                    }

                                    myView.Document.Add(iObj);
                                    (iObj as Rationale).HookupEvents();
                                    (iObj as Rationale).BindToMetaObjectProperties();
                                    rationaleNodes.Add(iObj as Rationale);
                                }
                            }
                            List<IMetaNode> listOfNodesToAdd = containerInContainerShallow(nodes, nodesInsideContainers);
                            foreach (IMetaNode addedNode in listOfNodesToAdd)
                            {
                                if (!nodes.Contains(addedNode))
                                    nodes.Add(addedNode);
                            }

                            if (refreshNodes.Count > 0)
                            {
                                Tools.MetaComparer mComp = new MetaBuilder.UIControls.GraphingUI.Tools.MetaComparer();
                                mComp.MyView = myView;
                                //mComp.RefreshSelection(refreshNodes);
                                mComp.CompareSelection(refreshNodes, true);
                                //add refreshed collapsible nodes to list of nodes
                                foreach (GoObject node in refreshNodes)
                                {
                                    if (!(node is CollapsibleNode))
                                        continue;
                                    CollapsibleNode cNode = node as CollapsibleNode;

                                    foreach (RepeaterSection sect in cNode.RepeaterSections)
                                    {
                                        foreach (GoObject o in sect)
                                        {
                                            if (o is IMetaNode)
                                            {
                                                if (!nodes.Contains(o as IMetaNode))
                                                    nodes.Add(o as IMetaNode);
                                            }
                                        }
                                    }
                                }
                            }

                            #endregion
                        }
                        catch (Exception ex)
                        {
                            Log.WriteLog("GraphViewContainer::ItemsPasted nodes catch" + ex.ToString());
                        }
                        try
                        {
                            #region Links

                            foreach (GoObject iObj in FakeClipboardObjects)
                            {
                                if (!(iObj is QLink))
                                    continue;

                                QLink addedLink = iObj as QLink;
                                bool f = false;
                                bool t = false;

                                QuickPort qPortBottom = new QuickPort();
                                QuickPort qPortLeft = new QuickPort();

                                MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation defaultFrom = (MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation)Enum.Parse(typeof(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation), Core.Variables.Instance.DefaultFromPort);
                                MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation defaultTo = (MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation)Enum.Parse(typeof(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation), Core.Variables.Instance.DefaultToPort);

                                #region Find The Links Node And Port

                                foreach (IMetaNode n in nodes)
                                {
                                    if (n is ILinkedContainer)
                                    {
                                        if (n is SubgraphNode)
                                        {
                                            if (n.MetaObject.pkid == addedLink.FromMetaBase.pkid)
                                                addedLink.FromPort = (n as SubgraphNode).Port;
                                            if (n.MetaObject.pkid == addedLink.ToMetaBase.pkid)
                                                addedLink.ToPort = (n as SubgraphNode).Port;
                                        }
                                        else if (n is ValueChain)
                                        {
                                            if (n.MetaObject.pkid == addedLink.FromMetaBase.pkid)
                                            {
                                                foreach (QuickPort p in (n as ValueChain).Ports)
                                                {
                                                    if (p.Location == (addedLink.FromPortShallow as QuickPort).Location)
                                                    {
                                                        addedLink.FromPort = p;
                                                        f = true;
                                                        break;
                                                    }
                                                }
                                            }
                                            if (n.MetaObject.pkid == addedLink.ToMetaBase.pkid)
                                            {
                                                foreach (QuickPort p in (n as ValueChain).Ports)
                                                {
                                                    if (p.Location == (addedLink.ToPortShallow as QuickPort).Location)
                                                    {
                                                        addedLink.ToPort = p;
                                                        t = true;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (n is CollapsingRecordNodeItem)
                                    {
                                        if (n.MetaObject == null)
                                            continue;

                                        if (n.MetaObject.pkid == addedLink.FromMetaBase.pkid)
                                        {
                                            foreach (GoPort p in (n as CollapsingRecordNodeItem).Ports)
                                            {
                                                if (p.Location == (addedLink.FromPortShallow as GoPort).Location)
                                                {
                                                    addedLink.FromPort = p;
                                                    break;
                                                }
                                            }
                                        }
                                        if (n.MetaObject.pkid == addedLink.ToMetaBase.pkid)
                                        {
                                            foreach (GoPort p in (n as CollapsingRecordNodeItem).Ports)
                                            {
                                                if (p.Location == (addedLink.ToPortShallow as GoPort).Location)
                                                {
                                                    addedLink.ToPort = p;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else if (n is CollapsibleNode)
                                    {
                                        if (n.MetaObject == null)
                                            continue;

                                        if (n.MetaObject.pkid == addedLink.FromMetaBase.pkid)
                                        {
                                            foreach (GoPort p in (n as CollapsibleNode).Ports)
                                            {
                                                if (p.Location == (addedLink.FromPortShallow as GoPort).Location)
                                                {
                                                    addedLink.FromPort = p;
                                                    f = true;
                                                    break;
                                                }
                                            }
                                        }
                                        if (n.MetaObject.pkid == addedLink.ToMetaBase.pkid)
                                        {
                                            foreach (GoPort p in (n as CollapsibleNode).Ports)
                                            {
                                                if (p.Location == (addedLink.ToPortShallow as GoPort).Location)
                                                {
                                                    addedLink.ToPort = p;
                                                    t = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (n.MetaObject.pkid == addedLink.FromMetaBase.pkid)
                                        {
                                            if (n is ImageNode)
                                            {
                                                foreach (IGoPort prt in (n as ImageNode).Ports)
                                                {
                                                    if (!(prt is QuickPort))
                                                        continue;

                                                    QuickPort p = prt as QuickPort;
                                                    if (p.Location == (addedLink.FromPortShallow as QuickPort).Location)
                                                    {
                                                        addedLink.FromPort = p;
                                                        f = true;
                                                        break;
                                                    }
                                                    //TODO Default port setting
                                                    if (p.PortPosition == defaultFrom)
                                                        qPortBottom = p;
                                                }
                                            }
                                            else if (n is GraphNode)
                                            {
                                                foreach (QuickPort p in (n as GraphNode).Ports)
                                                {
                                                    if (p.Location == (addedLink.FromPortShallow as QuickPort).Location)
                                                    {
                                                        addedLink.FromPort = p;
                                                        f = true;
                                                        break;
                                                    }
                                                    //TODO Default port setting
                                                    if (p.PortPosition == defaultFrom)
                                                        qPortBottom = p;
                                                }
                                            }
                                        }
                                        if (n.MetaObject.pkid == addedLink.ToMetaBase.pkid)
                                        {
                                            if (n is ImageNode)
                                            {
                                                foreach (IGoPort prt in (n as ImageNode).Ports)
                                                {
                                                    if (!(prt is QuickPort))
                                                        continue;

                                                    QuickPort p = prt as QuickPort;
                                                    if (p.Location == (addedLink.ToPortShallow as QuickPort).Location)
                                                    {
                                                        addedLink.ToPort = p;
                                                        t = true;
                                                        break;
                                                    }
                                                    //TODO Default port setting
                                                    if (p.PortPosition == defaultTo)
                                                        qPortLeft = p;
                                                }
                                            }
                                            else if (n is GraphNode)
                                            {
                                                foreach (QuickPort p in (n as GraphNode).Ports)
                                                {
                                                    if (p.Location == (addedLink.ToPortShallow as QuickPort).Location)
                                                    {
                                                        addedLink.ToPort = p;
                                                        t = true;
                                                        break;
                                                    }
                                                    //TODO Default port setting
                                                    if (p.PortPosition == defaultTo)
                                                        qPortLeft = p;
                                                }
                                            }
                                        }

                                        //so we can skip out of going through all nodes every time
                                        if (f & t)
                                            break;
                                    }
                                }

                                #endregion

                                //if i couldnt find the link set it to the default via settings
                                if (addedLink.FromPort == null)
                                    addedLink.FromPort = qPortBottom;
                                if (addedLink.ToPort == null)
                                    addedLink.ToPort = qPortLeft;

                                //check if both nodes are on the diagram
                                if (addedLink.FromNode != null && addedLink.ToNode != null)
                                {
                                    if (myView.Document.Contains(addedLink.FromNode as GoObject) && myView.Document.Contains(addedLink.ToNode as GoObject))
                                    {
                                        QLink newLink = QLink.CreateLink(addedLink.FromNode as GoNode, addedLink.ToNode as GoNode, (int)addedLink.AssociationType, addedLink.FromPort as GoPort, addedLink.ToPort as GoPort);
                                        //newLink.ChangedLinkType();
                                        newLink.FromMetaBase = addedLink.FromMetaBase;
                                        newLink.ToMetaBase = addedLink.ToMetaBase;
                                        newLink.RealLink.SetPoints(addedLink.RealLink.CopyPointsArray());
                                        myView.Document.Add(newLink);
                                        links.Add(newLink);
                                    }
                                }
                            }

                            foreach (GoObject iObj in FakeClipboardObjects)
                            {
                                if (iObj is FishLink)
                                {
                                    //this is for custom fish links non artefact nodes
                                    FishLink FLCopy = iObj as FishLink;
                                    //use this information to find correct node within nodes as well as correct ports on the artefact for this fishlink based on toqlink info in links
                                    if (FLCopy.ToQlinkShallow != null)
                                    {
                                        FLCopy.Remove();
                                        IGoPort origPort = (FLCopy.FromPort as IGoPort);
                                        foreach (IMetaNode n in nodes)
                                        {
                                            if ((n as GoNode).Location == (FLCopy.ArtefactShallow as GoNode).Location)
                                            {
                                                //FLCopy.FromNode = n as GoNode;
                                                FLCopy.ArtefactShallow = n;
                                                break;
                                            }
                                        }
                                        foreach (QLink l in links)
                                        {
                                            if ((l.FromNode as GoNode).Location == (FLCopy.ToQlinkShallow.FromNode as GoNode).Location && (l.ToNode as GoNode).Location == (FLCopy.ToQlinkShallow.ToNode as GoNode).Location)
                                            {
                                                FLCopy.ToQlinkShallow = l;
                                                break;
                                            }
                                        }

                                        bool found = false;
                                        foreach (GoObject o in (FLCopy.ArtefactShallow as GoNode))
                                        {
                                            if (!(o is QuickPort))
                                                continue;

                                            if (o.Location == (FLCopy.FromPort as QuickPort).Location)
                                            {
                                                found = true;
                                                origPort = o as IGoPort;
                                                break;
                                            }
                                        }

                                        if (origPort != null && found)
                                        {
                                            FLCopy.FromPort = origPort;
                                            FLCopy.ToPort = FLCopy.ToQlinkShallow.GetFishLinkPort;

                                            FLCopy.Visible = ViewController.ArtefactPointersVisible;
                                            myView.Document.Add(FLCopy);
                                        }
                                    }
                                }
                            }

                            #endregion
                        }
                        catch (Exception ex)
                        {
                            Log.WriteLog("GraphViewContainer::ItemsPasted links catch" + ex.ToString());
                        }
                        try
                        {
                            #region Add other objects and anchor when required
                            foreach (GoObject iObj in FakeClipboardObjects)
                            {
                                if (iObj is ResizableBalloonComment || iObj is Rationale)
                                {
                                    ResizableBalloonComment iResComment = iObj as ResizableBalloonComment;
                                    if (iResComment.AnchorShallow == null)
                                        continue;

                                    foreach (GoObject tempObj in FakeClipboardObjects)
                                    {
                                        if (tempObj.Location == iResComment.AnchorShallow.Location)
                                        {
                                            myView.Document.Add(iResComment);
                                            translateObjects.Add(iResComment);

                                            iResComment.Anchor = tempObj;
                                            if (iResComment.AnchorShallow is QLink)
                                            {
                                                foreach (QLink l in links)
                                                {
                                                    if (l.Location == iResComment.AnchorShallow.Location)
                                                    {
                                                        iResComment.Anchor = l;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    continue;
                                }

                                //add any other goobject
                                if (!(iObj is IMetaNode) && !(iObj is QLink) && !(iObj is System.Collections.IEnumerator) && !(iObj is FishLink) && !(iObj is FishRealLink))
                                {
                                    myView.Document.Add(iObj);
                                    translateObjects.Add(iObj);
                                }
                                else
                                {
                                    removeTheseObjects.Add(iObj);
                                }
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            Log.WriteLog("GraphViewContainer::ItemsPasted other catch" + ex.ToString());
                        }
                        try
                        {
                            #region Relink Artefacts
                            foreach (ArtefactNode n in artefactNodes)
                            {
                                try
                                {
                                    FishLink fLink = n.ShallowFishLink;
                                    if (fLink == null)
                                        continue;

                                    QLink parentLink = null;
                                    if (fLink.ToQlinkShallow != null)
                                        parentLink = fLink.ToQlinkShallow;
                                    else if (fLink.ToQLink != null)
                                        parentLink = fLink.ToPort.GoObject.Parent.Parent as QLink;

                                    if (parentLink == null)
                                        continue;

                                    Artifact art = new Artifact();
                                    art.ObjectID = (parentLink.FromNode as IMetaNode).MetaObject.pkid;
                                    art.ObjectMachine = (parentLink.FromNode as IMetaNode).MetaObject.MachineName;
                                    art.ChildObjectID = (parentLink.ToNode as IMetaNode).MetaObject.pkid;
                                    art.ChildObjectMachine = (parentLink.ToNode as IMetaNode).MetaObject.MachineName;

                                    foreach (QLink l in links)
                                    {
                                        if (l.AssociationType == parentLink.AssociationType)
                                        {
                                            if (l.FromMetaBase != null && l.ToMetaBase != null)
                                            {
                                                if (l.FromMetaBase.pkid == art.ObjectID && l.ToMetaBase.pkid == art.ChildObjectID && l.FromMetaBase.MachineName == art.ObjectMachine && l.ToMetaBase.MachineName == art.ChildObjectMachine)
                                                {
                                                    //ViewController.ConnectArtefact((n as ArtefactNode), l);
                                                    ViewController.FishLinkArtefact((n as ArtefactNode), l);
                                                    //translateObjects.Add(n);
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                if ((l.FromNode != null && l.ToNode != null) && (l.FromNode is IMetaNode && l.ToNode is IMetaNode))
                                                {
                                                    MetaBase frommb = (l.FromNode as IMetaNode).MetaObject;
                                                    MetaBase tomb = (l.ToNode as IMetaNode).MetaObject;
                                                    if (frommb != null && tomb != null)
                                                    {
                                                        if (frommb.pkid == art.ObjectID && tomb.pkid == art.ChildObjectID && frommb.MachineName == art.ObjectMachine && tomb.MachineName == art.ChildObjectMachine)
                                                        {
                                                            //ViewController.ConnectArtefact((n as ArtefactNode), l);
                                                            ViewController.FishLinkArtefact((n as ArtefactNode), l);
                                                            //translateObjects.Add(n);
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        l.ToString();
                                                    }
                                                }
                                                else
                                                {
                                                    l.ToString();
                                                }
                                            }
                                        }
                                    }


                                }
                                catch (Exception ex)
                                {
                                    Core.Log.WriteLog(ex.ToString());
                                }

                                //foreach (Artifact art in d.DataRepository.ArtifactProvider.GetByArtifactObjectIDArtefactMachine(n.MetaObject.pkid, n.MetaObject.MachineName))
                                //{
                                //    if (art == null)
                                //        continue;

                                //}
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            Log.WriteLog("GraphViewContainer::ItemsPasted relink artefacts catch" + ex.ToString());
                        }
                        try
                        {
                            foreach (GoObject iObj in removeTheseObjects)
                            {
                                FakeClipboardObjects.Remove(iObj);
                            }

                            foreach (IMetaNode node in nodes)
                            {
                                if (node is GraphNode)
                                {
                                    (node as GraphNode).Shadowed = true;
                                    foreach (IMetaNode iNode in ViewController.GetIMetaNodesBoundToMetaObject(node.MetaObject))
                                    {
                                        (iNode as GoObject).Shadowed = true;
                                    }
                                }

                                if (node.ParentIsILinkedContainer || (node as GoObject).Parent is ILinkedContainer)
                                    continue;
                                translateObjects.Add(node as GoObject);
                            }

                            locationTranslation(translateObjects);

                            FakeClipboardObjects = new Collection<GoObject>();
                            //myView.Selection.Clear();

                            removeDummies();
                        }
                        catch (Exception ex)
                        {
                            Log.WriteLog("GraphViewContainer::ItemsPasted translation and remove dummy catch" + ex.ToString());
                        }

                        myView.DocScale += 0.01f; //this causes rerender of doc so graphical artefacts cannot appear
                        myView.EndUpdate();
                        myView.FinishTransaction("ItemsPasted");
                        //ViewController.pastingShallow = false;
                    }
                return;

                #region Old

                //Tools.MetaComparer mcomparer = new MetaBuilder.UIControls.GraphingUI.Tools.MetaComparer();
                //mcomparer.MyView = this.myView;

                myView.StartTransaction();
                if (DockingForm.DockForm.ShallowCopies.Count > 0)
                {
                    //List<MetaBase> itemsToRefresh = new List<MetaBase>();
                    //Clipboard.Clear();
                    int i = 0;
                    #region in selection
                    foreach (GoObject o in myView.Selection) //Selection here is the new selection of pasted objects
                    {
                        if (o is IShallowCopyable)
                        {
                            IShallowCopyable node = o as IShallowCopyable;
                            MetaBase mo = node.MetaObject;
                            foreach (IShallowCopyable otherItem in DockingForm.DockForm.ShallowCopies)
                            {
                                if ((otherItem != node) && (otherItem.MetaObject.ToString() == mo.ToString()))
                                {
                                    if (mo._ClassName == otherItem.MetaObject._ClassName)
                                    {
                                        if (node.Name == otherItem.Name)
                                        {
                                            node.MetaObject = otherItem.MetaObject;
                                            node.HookupEvents();
                                            node.CopyAsShadow = false;
                                            node.BindToMetaObjectProperties();
                                            node.FireMetaObjectChanged(node, new EventArgs());

                                            PointF currentLocation = (node as GoNode).Location;
                                            PointF centerScreen = ViewController.GetCenter();
                                            float xDifference = currentLocation.X - centerScreen.X;
                                            float YDifference = currentLocation.Y - centerScreen.Y;
                                            //calculate offset

                                            if (DockingForm.DockForm.ShallowCopies.Count == 1) //only place in middle if 1 object pasted
                                                //if (i==0) //place the first object in the middle of the screen
                                                (node as GoNode).Location = centerScreen;

                                            //(otherItem as GoNode).Location = ViewController.GetCenter();
                                            //otherItem.CopyAsShadow = false;
                                            //otherItem.HookupEvents();
                                            //otherItem.BindToMetaObjectProperties();
                                            //otherItem.FireMetaObjectChanged(otherItem, new EventArgs());
                                            //10 January 2013
                                            //if (!itemsToRefresh.Contains(node.MetaObject))
                                            //itemsToRefresh.Add(node.MetaObject);
                                            i += 1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    //#region selection is null but not shallow copies (hacked! dont know why we check selection anyway arent we supposed to paste from clipboard?)

                    //if (myView.Selection.Count == 0 || myView.Selection.Primary == null)
                    //{
                    //    foreach (IShallowCopyable otherItem in DockingForm.DockForm.ShallowCopies)
                    //    {
                    //        otherItem.CopyAsShadow = false;
                    //        (otherItem as GoNode).Location = ViewController.GetCenter();
                    //        otherItem.HookupEvents();
                    //        otherItem.BindToMetaObjectProperties();
                    //        //10 January 2013
                    //        //if (!itemsToRefresh.Contains(otherItem.MetaObject))
                    //        //itemsToRefresh.Add(otherItem.MetaObject);

                    //        otherItem.FireMetaObjectChanged(otherItem, new EventArgs());
                    //    }
                    //}
                    //#endregion

                    DockingForm.DockForm.ShallowCopies = new Collection<IShallowCopyable>();
                    DockingForm.DockForm.ShallowGoObjects = new Collection<GoObject>();

                    //10 January 2013 We are literally pasting objects that are new and then setting their metaobjects
                    //creating exact copies of the original objects which removes the need to compare
                    //mcomparer.RefreshSelection(myView.Selection);
                }

                myView.FinishTransaction("ItemsPasted");

                #endregion
            }
        }

        private void locationTranslation(Collection<IMetaNode> nodes)
        {
            //Thread.Sleep(50);
            PointF centerLocation = myView.LastInput.DocPoint;
            float topLeftX = 0;
            float topLeftY = 0;
            myView.Selection.Clear();
            foreach (IMetaNode imn in nodes)
            {
                //skip objects which dont have 'shape'
                if (!(imn is GoObject) || (imn is CollapsingRecordNodeItem))
                    continue;

                GoObject o = imn as GoObject;
                myView.Selection.Add(o);

                if (topLeftX == 0)
                    topLeftX = o.Location.X;
                if (topLeftY == 0)
                    topLeftY = o.Location.Y;

                if (topLeftX > o.Location.X)
                    topLeftX = o.Location.X;
                if (topLeftY > o.Location.Y)
                    topLeftY = o.Location.Y;
            }

            float width = 0;
            float height = 0;

            if (centerLocation.X > topLeftX)
                width = centerLocation.X - topLeftX;
            else
                width = (topLeftX - centerLocation.X) * -1;

            if (centerLocation.Y > topLeftY)
                height = centerLocation.Y - topLeftY;
            else
                height = (topLeftY - centerLocation.Y) * -1;

            myView.Document.SkipsUndoManager = true;
            //NULL GoText IN CORNER?
            myView.MoveSelection(null, new SizeF(width, height), true);
            myView.Document.SkipsUndoManager = false;
        }
        private void locationTranslation(Collection<GoObject> objects)
        {
            //Thread.Sleep(50);
            try
            {
                PointF centerLocation = myView.LastInput.DocPoint;
                float topLeftX = 0;
                float topLeftY = 0;
                myView.Selection.Clear();
                foreach (GoObject o in objects)
                {
                    if (!(o is CollapsingRecordNodeItem) && !(o is CollapsingRecordNodeItemList) && !(o is RepeaterSection) && !(o is CollapsibleLabel) && !(o is FishLink) && !(o is FishRealLink))
                    {
                        try
                        {
                            if (!myView.Selection.Contains(o))
                                myView.Selection.Add(o);
                        }
                        catch
                        {
                            o.ToString();
                        }
                    }

                    if (!(o is FishLink) && !(o is FishRealLink))
                    {
                        if (topLeftX == 0)
                            topLeftX = o.Location.X;
                        if (topLeftY == 0)
                            topLeftY = o.Location.Y;

                        if (topLeftX > o.Location.X)
                            topLeftX = o.Location.X;
                        if (topLeftY > o.Location.Y)
                            topLeftY = o.Location.Y;
                    }
                    else
                    {
                        //this position should be inferred
                        //make it visible!
                        o.Visible = ViewController.ArtefactPointersVisible;
                    }
                }

                float width = 0;
                float height = 0;

                if (centerLocation.X > topLeftX)
                    width = centerLocation.X - topLeftX;
                else
                    width = (topLeftX - centerLocation.X) * -1;

                if (centerLocation.Y > topLeftY)
                    height = centerLocation.Y - topLeftY;
                else
                    height = (topLeftY - centerLocation.Y) * -1;

                myView.Document.SkipsUndoManager = true;
                //NULL GoText IN CORNER?
                myView.StartTransaction();
                myView.MoveSelection(null, new SizeF(width, height), false);
                myView.FinishTransaction("locationTranslation(List<GoObject>)");
                myView.Document.SkipsUndoManager = false;
            }
            catch (Exception ex)
            {
                Log.WriteLog("GraphViewContainer::locationTranslation(List<GoObject>)::" + Environment.NewLine + ex.ToString());
            }
            myView.Document.SkipsUndoManager = false;
        }

        private List<IMetaNode> containerInContainerShallow(List<IMetaNode> nodes, List<IMetaNode> nodesInsideContainers)
        {
            List<IMetaNode> listOfNodesToAdd = new List<IMetaNode>();

            foreach (IMetaNode n in nodesInsideContainers)
            {
                foreach (IMetaNode containerMetaNode in nodes)
                {
                    if (containerMetaNode is ILinkedContainer)
                    {
                        ILinkedContainer container = containerMetaNode as ILinkedContainer;
                        if (container is SubgraphNode)
                        {
                            foreach (GoObject containerObject in (container as SubgraphNode).GetEnumerator())
                            {
                                if (containerObject is IMetaNode)
                                {
                                    if ((containerObject as GoObject).Location == (n as GoObject).Location)
                                    {
                                        (containerObject as IMetaNode).MetaObject = n.MetaObject;
                                        (containerObject as IMetaNode).HookupEvents();
                                        (containerObject as IMetaNode).BindToMetaObjectProperties();
                                        (containerObject as GoObject).Shadowed = true;

                                        if (containerObject is ILinkedContainer)
                                        {
                                            List<IMetaNode> containerObjectList = new List<IMetaNode>();
                                            containerObjectList.Add(containerObject as IMetaNode);
                                            listOfNodesToAdd.AddRange(containerInContainerShallow(containerObjectList, nodesInsideContainers));
                                        }
                                        else
                                        {
                                            listOfNodesToAdd.Add((containerObject as IMetaNode));
                                        }
                                        continue;
                                    }
                                }
                            }
                        }
                        else if (container is ValueChain)
                        {
                            foreach (GoObject containerObject in (container as ValueChain).GetEnumerator())
                            {
                                if (containerObject is IMetaNode)
                                {
                                    if ((containerObject as GoObject).Location == (n as GoObject).Location)
                                    {
                                        (containerObject as IMetaNode).MetaObject = n.MetaObject;
                                        (containerObject as IMetaNode).HookupEvents();
                                        (containerObject as IMetaNode).BindToMetaObjectProperties();
                                        (containerObject as GoObject).Shadowed = true;
                                        listOfNodesToAdd.Add((containerObject as IMetaNode));
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if ((n as GoObject).Location == (containerMetaNode as GoObject).Location)
                            //if ((containerMetaNode as IMetaNode).MetaObject.pkid > 0)
                            (n as IMetaNode).MetaObject = (containerMetaNode as IMetaNode).MetaObject;
                        //must be a metanode
                    }
                }

                //Container in container
                //if (n is ILinkedContainer)
                //{
                //    List<IMetaNode> nextLevelNodes = new List<IMetaNode>();
                //    if (n is SubgraphNode)
                //    {
                //        foreach (GoObject o in (n as SubgraphNode).GetEnumerator())
                //            if (o is IMetaNode)
                //                nextLevelNodes.Add(o as IMetaNode);
                //    }
                //    else if (n is ValueChain)
                //    {
                //        foreach (GoObject o in (n as ValueChain).GetEnumerator())
                //            if (o is IMetaNode)
                //                nextLevelNodes.Add(o as IMetaNode);
                //    }

                //    if (nextLevelNodes.Count > 0)
                //        listOfNodesToAdd.AddRange(containerInContainerShallow(nodesInsideContainers, nextLevelNodes));
                //}
            }

            return listOfNodesToAdd;
        }

        private void menuItemEditSelectAll_Click(object sender, EventArgs e)
        {
            myView.SelectAll();
        }
        private void menuItemEditAddShallowCopy_Click(object sender, EventArgs e)
        {
            //ItemsPasted();
            //return;

            Collection<IShallowCopyable> shallows = new Collection<IShallowCopyable>();
            foreach (IShallowCopyable node in DockingForm.DockForm.ShallowCopies)
            {
                IShallowCopyable copy = node.Copy() as IShallowCopyable;
                copy.MetaObject = node.MetaObject;
                copy.HookupEvents();
                shallows.Add(copy);

                GraphNode parentShape = node as GraphNode;

                //DockingForm.DockForm.CheckIfExistsOnDiagrams(parentShape);
            }

            myView.ViewController.AddShallowCopies(shallows);

            Tools.MetaComparer mcomparer = new MetaBuilder.UIControls.GraphingUI.Tools.MetaComparer();
            mcomparer.MyView = this.myView;
            mcomparer.RefreshSelection(myView.Selection);
        }

        #endregion

        private void removeDummies()
        {
            //find and remove that dummy object!
            Collection<GoObject> dummies = new Collection<GoObject>();
            foreach (GoObject o in myView.Document)
            {
                if (o is GoText)// && o.Visible == false)
                {
                    if ((o as GoText).Text == "~!DUMMY!~")
                        dummies.Add((o as GoText));
                    else if ((o.ParentNode != null && o.ParentNode == o) && (o as GoText).Text.Length == 0)
                        dummies.Add((o as GoText));
                    else if (o.Location == new PointF(0, 0) && (o as GoText).Text.Length == 0)
                        dummies.Add((o as GoText));
                }
            }
            if (dummies.Count > 0)
            {
                foreach (GoObject DUMMY in dummies)
                    myView.Document.Remove(DUMMY);

                //Core.Log.WriteLog(dummies.Count + " : ghost in the machine");
            }
            //Clipboard.Clear();
        }

        #region View Menu EventHandling

        private void menuItemViewGrid_Click(object sender, EventArgs e)
        {
            bool Shown = !menuItemViewGrid.Checked;
            menuItemViewGrid.Checked = Shown;
            myView.Grid.Visible = Shown;
        }

        private void menuItemViewZoom50_Click(object sender, EventArgs e)
        {
            zController.Zoom(0.5f);
        }
        private void menuItemViewZoom100_Click(object sender, EventArgs e)
        {
            zController.Zoom(1);
        }
        private void menuItemViewZoom150_Click(object sender, EventArgs e)
        {
            zController.Zoom(1.5f);
        }
        private void menuItemViewZoom200_Click(object sender, EventArgs e)
        {
            zController.Zoom(2);
        }
        private void menuItemViewZoom400_Click(object sender, EventArgs e)
        {
            zController.Zoom(4);
        }

        private void CreateEightVerticalRectangles(object sender, EventArgs e)
        {
            GoLayer lanes = myView.BackgroundLayer;
            // create ten pairs of vertical lanes of alternating color,
            // assuming the diagram is oriented vertically
            for (int i = 0; i < 4; i++)
            {
                // create a vertical light blue lane
                GoRectangle l = new GoRectangle();
                l.Bounds = new RectangleF(0, i * 200 + 200, 5000, 100);
                l.Pen = null;
                l.Brush = Brushes.AliceBlue;
                lanes.Add(l);
                // create a vertical light yellow lane
                l = new GoRectangle();
                l.Bounds = new RectangleF(0, i * 200 + 100 + 200, 5000, 100);
                l.Pen = null;
                l.Brush = Brushes.Gainsboro;
                lanes.Add(l);
            }
        }
        private void menuItemViewZoomPageWidth_Click(object sender, EventArgs e)
        {
            myView.OverrideDocScaleMath = true;
            myView.SheetStyle = GoViewSheetStyle.SheetWidth;
            myView.UpdateExtent();
            myView.ViewController.CenterView();
            myView.OverrideDocScaleMath = false;
        }
        private void menuItemViewZoomPageHeight_Click(object sender, EventArgs e)
        {
            myView.OverrideDocScaleMath = true;
            myView.SheetStyle = GoViewSheetStyle.SheetHeight;
            myView.UpdateExtent();
            myView.ViewController.CenterView();
            myView.OverrideDocScaleMath = false;
        }
        private void menuItemViewZoomWholePage_Click(object sender, EventArgs e)
        {
            myView.OverrideDocScaleMath = true;
            myView.SheetStyle = GoViewSheetStyle.WholeSheet;
            myView.UpdateExtent();
            myView.ViewController.CenterView();
            myView.OverrideDocScaleMath = false;
        }

        #endregion

        #region Insert Menu EventHandling

        private void menuItemInsertFileAttachment_Click(object sender, EventArgs e)
        {
            ShapeDesignController shapeProtoTyping = new ShapeDesignController(myView);
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Title = "Select Attachment";
            DialogResult result = ofd.ShowDialog(DockingForm.DockForm);
            if (result == DialogResult.OK)
            {
                if (ofd.FileName != null)
                    shapeProtoTyping.AddAttachment(ofd.FileName);
            }
        }
        private void menuItemInsertBlockArrow_Click(object sender, EventArgs e)
        {
            ShapeDesignController shapePrototyping = new ShapeDesignController(myView);
            shapePrototyping.AddBlockArrow();
            myView.Document.SkipsUndoManager = !myView.Document.SkipsUndoManager;
        }
        private void menuItemInsertRichText_Click(object sender, EventArgs e)
        {
            ShapeDesignController shapeProtoTypingTool = new ShapeDesignController(myView);
            shapeProtoTypingTool.AddRichTextLabel();
        }
        private void menuItemInsertRectangle_Click(object sender, EventArgs e)
        {
            ShapeDesignController shapeProtoTyping = new ShapeDesignController(myView);
            shapeProtoTyping.AddRectangle();
        }
        private void menuItemInsertEllipse_Click(object sender, EventArgs e)
        {
            ShapeDesignController shapeProtoTyping = new ShapeDesignController(myView);
            shapeProtoTyping.AddEllipse();
        }

        private void menuItemInsertPolygon_Click(object sender, EventArgs e)
        {
            PolygonDrawingTool ptool = new PolygonDrawingTool(myView);
            ptool.Style = GoPolygonStyle.Bezier;
            myView.Tool = ptool;
        }
        private void menuItemInsertLinePolygon_Click(object sender, EventArgs e)
        {
            PolygonDrawingTool ptool = new PolygonDrawingTool(myView);
            ptool.Style = GoPolygonStyle.Line;
            myView.Tool = ptool;
        }
        private void menuInsertLine_Click(object sender, EventArgs e)
        {
            myView.Tool = new StrokeDrawingTool(myView, GoStrokeStyle.Line);
        }
        private void menuItemInsertLineBezier_Click(object sender, EventArgs e)
        {
            myView.Tool = new StrokeDrawingTool(myView, GoStrokeStyle.Bezier);
        }
        private void menuItemInsertLineRounded_Click(object sender, EventArgs e)
        {
            myView.Tool = new StrokeDrawingTool(myView, GoStrokeStyle.RoundedLine);
        }
        private void menuItemInsertLineRoundedWithJumpOvers_Click(object sender, EventArgs e)
        {
            myView.Tool = new StrokeDrawingTool(myView, GoStrokeStyle.RoundedLineWithJumpOvers);
        }

        private void menuItemInsertImage_Click(object sender, EventArgs e)
        {
            ShapeDesignController shapeProtoTypingTool = new ShapeDesignController(myView);
            shapeProtoTypingTool.AddImage();
        }
        private void menuItemInsertLabel_Click(object sender, EventArgs e)
        {
            ShapeDesignController shapeProtoTypingTool = new ShapeDesignController(myView);
            shapeProtoTypingTool.AddLabel();
            //shapeProtoTypingTool.AddRichTextLabel();
        }
        private void menuItemInsertComment_Click(object sender, EventArgs e)
        {
            ShapeDesignController shapeProtoTypingTool = new ShapeDesignController(myView);
            shapeProtoTypingTool.AddComment();
        }
        private void menuItemInsertBalloonComment_Click(object sender, EventArgs e)
        {
            ShapeDesignController shapeProtoTypingTool = new ShapeDesignController(myView);
            shapeProtoTypingTool.AddBalloonComment();
        }
        private void menuItemInsertRationale_Click(object sender, EventArgs e)
        {
            ShapeDesignController shapeProtoTypingTool = new ShapeDesignController(myView);
            shapeProtoTypingTool.AddRationale();
        }
        private void menuItemInsertPort_Click(object sender, EventArgs e)
        {
            ShapeDesignController shapeProtoTypingTool = new ShapeDesignController(myView);
            shapeProtoTypingTool.AddPort();
        }
        private void menuItemInsertCube_Click(object sender, EventArgs e)
        {
            ShapeDesignController shapeProtoTypingTool = new ShapeDesignController(myView);
            shapeProtoTypingTool.AddCube();
        }
        private void menuItemInsertHexagon_Click(object sender, EventArgs e)
        {
            ShapeDesignController shapeProtoTypingTool = new ShapeDesignController(myView);
            shapeProtoTypingTool.AddHexagon();
        }
        private void menuItemInsertParallelogram_Click(object sender, EventArgs e)
        {
            ShapeDesignController shapeProtoTyping = new ShapeDesignController(myView);
            shapeProtoTyping.AddParallelogram();
        }
        private void menuItemInsertOctagon_Click(object sender, EventArgs e)
        {
            ShapeDesignController shapeProtoTypingTool = new ShapeDesignController(myView);
            shapeProtoTypingTool.AddOctagon();
        }
        private void menuItemInsertDiamond_Click(object sender, EventArgs e)
        {
            ShapeDesignController shapeProtoTypingTool = new ShapeDesignController(myView);
            shapeProtoTypingTool.AddDiamond();
        }
        private void menuItemInsertCylinderVertical_Click(object sender, EventArgs e)
        {
            ShapeDesignController shapeProtoTypingTool = new ShapeDesignController(myView);
            shapeProtoTypingTool.AddCylinder(Orientation.Vertical);
        }
        private void menuItemInsertCylinderHorizontal_Click(object sender, EventArgs e)
        {
            ShapeDesignController shapeProtoTypingTool = new ShapeDesignController(myView);
            shapeProtoTypingTool.AddCylinder(Orientation.Horizontal);
        }

        private void menuItemInsertSubgraphObjectExisting_Click(object sender, EventArgs e)
        {
            ObjectFinder ofinder = new ObjectFinder(false);
            DialogResult res = ofinder.ShowDialog(DockingForm.DockForm);
            if (res == DialogResult.OK)
            {
                if (ofinder.SelectedObjectsList.Count > 0)
                {
                    //8 January 2013
                    //Calculus 101
                    float NextX = 0f;
                    float maxHeight = 0;

                    int iteration = 0;
                    int Yiteration = 0;

                    PointF centerPoint = myView.LastInput.DocPoint;// ViewController.GetCenter();
                    foreach (MetaBase obj in ofinder.SelectedObjectsList)
                    {
                        SubgraphNode sgnode = ViewController.AddSubgraphObjectNode();
                        sgnode.BindingInfo = new BindingInfo();
                        sgnode.MetaObject = obj;
                        sgnode.BindingInfo.BindingClass = sgnode.MetaObject._ClassName;
                        sgnode.Label.Text = sgnode.MetaObject.ToString();
                        sgnode.BindToMetaObjectProperties();
                        sgnode.Deletable = true;
                        sgnode.HookupEvents();

                        if (maxHeight < sgnode.Height)
                            maxHeight = sgnode.Height;

                        sgnode.Position = new PointF(centerPoint.X + NextX, centerPoint.Y + ((maxHeight + 20) * Yiteration));

                        if (hasChildren(sgnode.MetaObject) > 0) //this is always true [move into method for surface area]
                        {
                            if (MessageBox.Show(DockingForm.DockForm, "Would you like to load all children for this subgraph?", "Load children", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                ConstructSubbgraphFromParent(sgnode);
                            }
                        }
                        //placeSelectionIntoSubGraph(sgnode);

                        ShapeOrderingControl.BringToFront(sgnode, View);
                        iteration += 1;
                        NextX += (10 * iteration) + sgnode.Width;
                        if (iteration == 4)
                        {
                            Yiteration += 1;
                            iteration = 0;
                            NextX = 0;
                        }
                    }
                }
                myView.Doc.ContainsILinkContainers = true;
            }
            myView.ResumeLayout();
        }

        private void ConstructSubbgraphFromParent(SubgraphNode node)
        {
            if (!node.IsExpanded)
                node.Expand();

            float x = 20;
            float y = 10;
            foreach (ObjectAssociation ass in d.DataRepository.Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(node.MetaObject.pkid, node.MetaObject.MachineName))
            {
                if (ass.Machine == "DB-TRIGGER")
                    continue;

                MetaObject obj = d.DataRepository.Provider.MetaObjectProvider.GetBypkidMachine(ass.ChildObjectID, ass.ChildObjectMachine);
                if (obj == null)
                    continue;
                MetaBase mBase = Loader.GetByID(obj.Class, obj.pkid, obj.Machine);

                EmbeddedRelationship emrel = new EmbeddedRelationship();
                emrel.MyAssociation = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByCAid(ass.CAid);
                emrel.MyMetaObject = mBase;
                emrel.FromDatabase = true;
                node.ObjectRelationships.Add(emrel);

                if (node.DefaultClassBindings == null)
                    node.DefaultClassBindings = new Dictionary<string, ClassAssociation>();

                if (!node.DefaultClassBindings.ContainsKey(mBase.Class))
                    node.DefaultClassBindings.Add(mBase.Class, emrel.MyAssociation);

                if (hasChildren(mBase) == 0 || (mBase.Class == "Object" && mBase.Get("ObjectType") != null && mBase.Get("ObjectType").ToString() == "Capability"))
                {
                    //normal object not recursive
                    try
                    {
                        GraphNode childNode = DockingForm.DockForm.GetShape(mBase.Class).Copy() as GraphNode;
                        childNode.MetaObject = mBase;
                        childNode.BindingInfo.BindingClass = mBase._ClassName;
                        childNode.HookupEvents();
                        childNode.BindToMetaObjectProperties();
                        childNode.Position = new PointF(node.Position.X + x, node.Position.Y + 40);
                        childNode.ParentIsILinkedContainer = true;

                        childNode.Remove();
                        node.Add(childNode);

                        x += childNode.Width + 20;
                    }
                    catch (Exception ex) //in all cases this will be missing shape problem
                    {
                        Log.WriteLog("GraphViewContainer::ConstructSubbgraphFromParent::" + ex.ToString());
                    }
                }
                else
                {
                    SubgraphNode childNode = ViewController.AddSubgraphObjectNode();
                    childNode.BindingInfo = new BindingInfo();
                    childNode.MetaObject = mBase;
                    childNode.BindingInfo.BindingClass = mBase._ClassName;
                    childNode.Label.Text = mBase.ToString();
                    childNode.HookupEvents();
                    childNode.BindToMetaObjectProperties();
                    childNode.Deletable = true;
                    childNode.Expand();
                    childNode.Position = new PointF(node.Position.X + 30, node.Position.Y + 40 + y);
                    childNode.ParentIsILinkedContainer = true;

                    childNode.BackgroundColor = ControlPaint.Dark(node.BackgroundColor);// : ControlPaint.Light(node.BackgroundColor);

                    childNode.Remove();
                    node.Add(childNode);

                    ConstructSubbgraphFromParent(childNode);

                    x += childNode.Width + 20;
                    y += childNode.Height + 10;
                }
            }
        }
        private int hasChildren(MetaBase mBase)
        {
            int c = 0;
            TList<ObjectAssociation> ass = d.DataRepository.Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(mBase.pkid, mBase.MachineName);
            foreach (ObjectAssociation a in ass)
            {
                if (a.Machine != "DB-TRIGGER")
                {
                    MetaObject mObj = DataAccessLayer.DataRepository.MetaObjectProvider.GetBypkidMachine(a.ChildObjectID, a.ChildObjectMachine);
                    if (Core.Variables.Instance.ClassesWithoutStencil.Contains(mObj.Class))
                        continue;
                    c += 1;
                }
            }
            return c;
        }

        private void menuItemInsertSubgraphObjectNew_Click(object sender, EventArgs e)
        {
            string _class = DockingForm.DockForm.GetAClass();
            if (_class.Length > 0)
            {
                SubgraphNode sgnode = ViewController.AddSubgraphObjectNode();
                sgnode.MetaObject = Meta.Loader.CreateInstance(_class);

                sgnode.BindingInfo = new BindingInfo();
                sgnode.BindToMetaObjectProperties();
                sgnode.Position = ViewController.GetCenter();
                sgnode.BindingInfo.BindingClass = sgnode.MetaObject._ClassName;
                sgnode.Label.Text = "New " + sgnode.MetaObject._ClassName;
                sgnode.HookupEvents();
                sgnode.Deletable = true;

                //placeSelectionIntoSubGraph(sgnode);
                myView.Doc.ContainsILinkContainers = true;
                sgnode.FireMetaObjectChanged(this, EventArgs.Empty);
                ShapeOrderingControl.BringToFront(sgnode, View);
            }
        }
        private void menuItemInsertValueChainStep_Click(object sender, EventArgs e)
        {
        }
        private void menuItemInsertValueChainStepExisting_Click(object sender, EventArgs e)
        {
            ObjectFinder ofinder = new ObjectFinder(false);
            ofinder.LimitToClass = "Function";
            DialogResult res = ofinder.ShowDialog(DockingForm.DockForm);
            if (res == DialogResult.OK)
            {
                foreach (MetaBase mb in ofinder.SelectedObjectsList)
                {
                    ViewController.AddValueChain(mb);
                }
                //AddValueChain(ofinder.SelectedObjectsList[0]);
            }
            myView.Doc.ContainsILinkContainers = true;
        }
        private void menuItemInsertValueChainStepNew_Click(object sender, EventArgs e)
        {
            ViewController.AddValueChain(null);
            myView.Doc.ContainsILinkContainers = true;
        }
        private void menuItemToolsLoadFromDBRefreshObjectsSelection_Click(object sender, System.EventArgs e)
        {
            Tools.MetaComparer mcomparer = new MetaBuilder.UIControls.GraphingUI.Tools.MetaComparer();
            mcomparer.MyView = this.myView;
            mcomparer.CompareSelection(myView.Selection, false);
            return;
        }
        private void menuItemInsertSwimlane_Click(object sender, EventArgs e)
        {
            string _class = DockingForm.DockForm.GetAClass();
            if (_class.Length > 0)
            {
                myView.StartTransaction();
                MappingCell cell = new MappingCell();
                cell.Size = new SizeF(800, 300);
                cell.BackGround.Width = cell.Width;
                cell.BackGround.Height = cell.Height;
                cell.Position = ViewController.GetCenter();
                cell.MetaObject = Loader.CreateInstance(_class);
                cell.HookupEvents();
                cell.RepositionRectangle("Left");
                cell.FireMetaObjectChanged(this, EventArgs.Empty);
                this.myView.Document.Add(cell);
                myView.Doc.ContainsILinkContainers = true;
                ShapeOrderingControl.SendToBack(cell, myView);
                myView.FinishTransaction("Add Mapping Cell " + _class);
            }
        }
        private void swimlanesFromDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tools.MetaComparer comp = new MetaBuilder.UIControls.GraphingUI.Tools.MetaComparer();
            comp.MyView = myView;

            //myView.Document.SkipsUndoManager = true;
            ObjectFinder ofinder = new ObjectFinder(false);
            ofinder.IncludeStatusCombo = true;
            ofinder.ExcludeStatuses.Add(VCStatusList.Obsolete);
            ofinder.ExcludeStatuses.Add(VCStatusList.MarkedForDelete);
            ofinder.ArtefactsOnly = false;
            DialogResult res = ofinder.ShowDialog(DockingForm.DockForm);
            GoCollection addedLanes = new GoCollection();
            if (res == DialogResult.OK)
            {
                Dictionary<MetaObjectKey, MetaBase> objects = ofinder.SelectedObjects;
                ShapeDesignController controller = new ShapeDesignController(myView);
                foreach (KeyValuePair<MetaObjectKey, MetaBase> kvpair in objects)
                {
                    myView.StartTransaction();
                    MappingCell cell = new MappingCell();
                    //cell.Width = float.Parse(Core.Variables.Instance.GridCellSize.ToString()) * 75;
                    //cell.Height = float.Parse(Core.Variables.Instance.GridCellSize.ToString()) * 25;
                    cell.Size = new SizeF(800, 300);
                    cell.BackGround.Width = cell.Width;
                    cell.BackGround.Height = cell.Height;
                    cell.Position = ViewController.GetCenter();
                    cell.MetaObject = kvpair.Value;
                    cell.HookupEvents();
                    cell.FireMetaObjectChanged(sender, e);
                    cell.RepositionRectangle("Left");
                    //cell.Text = kvpair.Value.ToString();
                    this.myView.Document.Add(cell);
                    myView.Doc.ContainsILinkContainers = true;
                    ShapeOrderingControl.SendToBack(cell, myView);
                    addedLanes.Add(cell);
                    myView.FinishTransaction("Add Mapping Cell " + kvpair.Value.ToString());
                }
            }

            //refresh on load to get indicators
            comp.RefreshSelection(addedLanes);
            comp = null; //destroy it

            //myView.Document.SkipsUndoManager = false;
            //myView.Document.IsModified = true;
            Notify();
        }
        private void placeSelectionIntoSubGraph(SubgraphNode newSubGraph)
        {
            if (myView.Selection.Count > 0)
            {
                if (MessageBox.Show(DockingForm.DockForm, "Would you like your selection to be added into the newly created subgraph?", "Selection Addition", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    newSubGraph.Position = new PointF(myView.Selection.Primary.Location.X - 20f, myView.Selection.Primary.Location.Y - 20f);

                    //myView.SuspendLayout();

                    //BIND
                    newSubGraph.AddCollection(myView.Selection, true);

                    newSubGraph.Expand();
                    newSubGraph.Collapse();
                    newSubGraph.Expand();
                    newSubGraph.Collapse();

                    myView.ResumeLayout();
                }
            }
        }

        private void menuItemInsertLegendMenuItemClass_Click(object sender, EventArgs e)
        {
            ViewController.CreateLegend(true, false);
        }
        private void menuItemInsertLegendMenuItemColor_Click(object sender, EventArgs e)
        {
            ViewController.CreateLegend(false, true);

        }
        private void menuItemInsertLegendMenuItemBoth_Click(object sender, EventArgs e)
        {
            ViewController.CreateLegend(true, true);
        }

        #endregion

        #region Shape Menu EventHandling

        private void menuItemShapeAlignTops_Click(object sender, EventArgs e)
        {
            AlignmentControl.DoAlign(myView, AlignmentControl.AlignmentType.Tops);
        }
        private void menuItemShapeAlignBottoms_Click(object sender, EventArgs e)
        {
            AlignmentControl.DoAlign(myView, AlignmentControl.AlignmentType.Bottoms);
        }
        private void menuItemShapeAlignLeftSides_Click(object sender, EventArgs e)
        {
            AlignmentControl.DoAlign(myView, AlignmentControl.AlignmentType.Lefts);
        }
        private void menuItemShapeAlignRightSides_Click(object sender, EventArgs e)
        {
            AlignmentControl.DoAlign(myView, AlignmentControl.AlignmentType.Rights);
        }
        private void menuItemShapeAlignHorizontalCenters_Click(object sender, EventArgs e)
        {
            AlignmentControl.DoAlign(myView, AlignmentControl.AlignmentType.HorizontalCenters);
        }
        private void menuItemShapeAlignVerticalCenters_Click(object sender, EventArgs e)
        {
            AlignmentControl.DoAlign(myView, AlignmentControl.AlignmentType.VerticalCenters);
        }
        private void mnuShapeGroupingGroup_Click(object sender, EventArgs e)
        {
            GroupingControl.GroupSelection(myView);
        }
        private void mnuShapeGroupingUngroup_Click(object sender, EventArgs e)
        {
            GroupingControl.UngroupSelection(myView);
        }
        private void mnuShapeGroupingGroupAll_Click(object sender, EventArgs e)
        {
            GroupingControl.GroupAll(myView);
            Notify();
        }
        private void mnuShapeGroupingUngroupAll_Click(object sender, EventArgs e)
        {
            GroupingControl.UngroupAll(myView);
            Notify();
        }
        private void menuItemShapeLayoutShapesDigraph_Click(object sender, EventArgs e)
        {
            LayeredLayoutDialog digraphDialog = new LayeredLayoutDialog(); // (myView);
            digraphDialog.ShowDialog(DockingForm.DockForm);
            //QuickLayout();
            /*
             if (myView != null)
             {
                 MetaBuilder.Graphing.Layout.LayerDigraphLayoutDialog d = new MetaBuilder.Graphing.Layout.LayerDigraphLayoutDialog(myView);
                 if (d.ShowDialog(DockingForm.DockForm) == DialogResult.OK)
                 {
                     GoLayout layout = d.GoLayout;
                     layout.Document.StartTransaction();
                     layout.PerformLayout();
                     layout.Document.FinishTransaction("Layered Digraph Auto Layout");
                     myView.ZoomToFit();
                 }
             }*/
        }
        private void menuItemShapeLayoutShapesForceDirected_Click(object sender, EventArgs e)
        {
        }
        private void menuItemShapeOrderBringForward_Click(object sender, EventArgs e)
        {
            try
            {
                ShapeOrderingControl.BringForward(myView);
            }
            catch
            {
            }
        }
        private void menuItemShapeOrderBringToFront_Click(object sender, EventArgs e)
        {
            try
            {
                ShapeOrderingControl.BringToFront(myView);
            }
            catch
            {
            }
        }
        private void menuItemShapeOrderSendBackward_Click(object sender, EventArgs e)
        {
            try
            {
                ShapeOrderingControl.SendBackward(myView);
            }
            catch
            {
            }
        }
        private void menuItemShapeOrderSendToBack_Click(object sender, EventArgs e)
        {
            try
            {
                ShapeOrderingControl.SendToBack(myView);
            }
            catch
            {
            }
        }

        #endregion

        #region Format Menu EventHandling

        private void menuItemFormatText_Click(object sender, EventArgs e)
        {
            ShowFormatForm(0);
        }

        private void menuItemFormatLine_Click(object sender, EventArgs e)
        {
            ShowFormatForm(1);
        }

        private void menuItemFormatFill_Click(object sender, EventArgs e)
        {
            ShowFormatForm(2);
        }

        private void menuItemFormatCornerRounding_Click(object sender, EventArgs e)
        {
            ShowFormatForm(1);
        }

        private void menuItemFormatBehaviour_Click(object sender, EventArgs e)
        {
            ShowFormatForm(3);
        }

        private void menuItemFormatAdvanced_Click(object sender, EventArgs e)
        {
            ShowFormatForm(4);
        }

        private void menuItemFormatAnchoringMode_Click(object sender, EventArgs e)
        {
            GoToolContext tool = View.FindMouseTool(typeof(GoToolContext)) as GoToolContext;
        }

        private void menuItemFormatCustomProperties_Click(object sender, EventArgs e)
        {
            ShapeBinding sbinding = new ShapeBinding();
            // Get the first node
            Symbol s = myView.Document as Symbol;
            foreach (GoObject n in myView.Document)
            {
                if (n is GraphNode)
                {
                    GraphNode gn = n as GraphNode;
                    s.ShapeBindingInfo = gn.BindingInfo;
                }
            }

            sbinding.BindingInfo = s.ShapeBindingInfo;
            DialogResult result = sbinding.ShowForm();
            if (result == DialogResult.OK)
            {
                s.ShapeBindingInfo = sbinding.BindingInfo;
            }
        }

        private void menuItemToolsPortMover_Click(object sender, EventArgs e)
        {
            ShapeDesignController shapePrototyping = new ShapeDesignController(myView);
            shapePrototyping.StartToolPortMover();
        }

        private void menuItemToolsPortFormatting_Click(object sender, EventArgs e)
        {
            DefinePort dfPort = new DefinePort();
            dfPort.View = myView;
            dfPort.Show();
            dfPort.IndicateAndBind();
        }

        private void menuItemToolsConvertCommentsToRationales_Click(object sender, EventArgs e)
        {
            ConvertAllAnchoredCommentsToRationale();
        }

        #endregion

        #region TabContextMenu EventHandling

        private void cxMenuItemCloseAllButThis_Click(object sender, EventArgs e)
        {
            DockingForm.DockForm.CloseAllButThisOne(sender, e);
        }
        private void cxMenuItemOpenPath_Click(object sender, System.EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(strings.GetPath((View.Document as NormalDiagram).Name));
            }
            catch
            {
                //you are doing this to not a file
            }
        }

        #endregion

        #region Unsorted eventhandling - mostly menuitems

        private void menuItemToolsDebugToXML_Click(object sender, EventArgs e)
        {
            //string outputfile = ShapeHelper.DebugToXML(myView.Selection.Primary);
        }

        public void LayeredDigraph()
        {
            LayeredLayoutDialog digraphDialog = new LayeredLayoutDialog();
            digraphDialog.TopMost = true;// (myView);
            digraphDialog.ShowInTaskbar = false;
            digraphDialog.Show();
            digraphDialog.Disposed += new EventHandler(digraphDialog_Disposed);

            //position all anchored/artefact objects on center of their parent
            //foreach (GoObject o in myView.Document)
            //{
            //    if (!(o is GoBalloon))
            //        continue;

            //    GoBalloon balloon = o as GoBalloon;
            //    if (balloon.Anchor != null)
            //    {
            //        if (balloon.Anchor.Parent is GoLabeledLink)
            //        {
            //            balloon.Location = (balloon.Anchor.Parent as GoLabeledLink).MidLabel.Location;
            //        }
            //        else
            //        {
            //            balloon.Location = balloon.Anchor.Location;
            //        }
            //    }
            //}
        }

        private void digraphDialog_Disposed(object sender, EventArgs e)
        {
            foreach (GoObject o in myView.Document)
            {
                if (!(o is GoBalloon))
                    continue;

                GoBalloon balloon = o as GoBalloon;
                if (balloon.Anchor != null)
                {
                    if (balloon.Anchor.Parent is GoLabeledLink)
                    {
                        balloon.Location = (balloon.Anchor.Parent as GoLabeledLink).MidLabel.Location;
                    }
                    else
                    {
                        balloon.Location = balloon.Anchor.Location;
                    }
                }
            }

        }
        private void menuItemShapeLayoutCircular_Click(object sender, EventArgs e)
        {
            goLayoutLayeredDigraph1 = new CircularLayout();
            goLayoutLayeredDigraph1.Document = View.Document;
            goLayoutLayeredDigraph1.PackOption = GoLayoutLayeredDigraphPack.Expand;
            goLayoutLayeredDigraph1.LayeringOption = GoLayoutLayeredDigraphLayering.LongestPathSink;
            goLayoutLayeredDigraph1.DirectionOption = GoLayoutDirection.Down;
            goLayoutLayeredDigraph1.PerformLayout();
            menuItemToolsAutoRelinkAll_Click(this, EventArgs.Empty);
        }

        public void ToggleMenuItemsForCurrentAppState()
        {
            if (ReadOnly)
            {
                menuItemViewPorts.Visible = toolStripFormatFill.Visible = toolStripFormatText.Visible = toolStripFormat.Visible = toolstripCopyFormatting.Visible = stripButtonPaste.Visible = stripButtonCut.Visible = stripButtonCopy.Visible = editToolStripMenuItem.Visible = false;
            }
            else
            {
                if (myView.Doc.FileType == FileTypeList.Diagram)
                {
                    menuItemInsertPort.Enabled = false;
                    menuItemInsertRepeaterSection.Enabled = false;
                    myView.ShowFrame = true;
                }
                else
                {
                    menuItemInsertPort.Enabled = true;
                    menuItemInsertRepeaterSection.Enabled = true;
                    myView.ShowFrame = false;
                }
            }
        }

        private void menuItemInsertTrapezoid_Click(object sender, EventArgs e)
        {
            //ReplaceShit();
            //return;
            ShapeDesignController shapePrototyping = new ShapeDesignController(myView);
            shapePrototyping.AddTrapezoid();
        }
        private void menuItemInsertHouseShape_Click(object sender, EventArgs e)
        {
            ShapeDesignController shapePrototyping = new ShapeDesignController(myView);
            shapePrototyping.AddHouseShape();
        }
        private void menuItemFileProperties_Click(object sender, EventArgs e)
        {
            /*if (myView.Document is NormalDiagram)
            {
                NormalDiagram normalDiagram = myView.Document as NormalDiagram;
                DocumentProperties docProperties = new DocumentProperties();
                docProperties.DocumentVersionManager = normalDiagram.VersionManager;
                docProperties.FileName = strings.GetFileNameOnly(normalDiagram.Name);
                docProperties.BindValues();
                docProperties.ShowDialog(DockingForm.DockForm);
            }*/
        }
        private void menuItemShapeLayoutTree_Click(object sender, EventArgs e)
        {
            TreeLayoutDialog myTreeDialog = new TreeLayoutDialog();
            myTreeDialog.Owner = this;
            myTreeDialog.Show();
            /*
            FSDLayerOuter fsdlayerouter = new FSDLayerOuter();
            fsdlayerouter.Document = myView.Document;
            fsdlayerouter.PerformLayout();*/
        }
        private void menuItemShapeLayoutFSD_Click(object sender, EventArgs e)
        {
            ViewController.LayoutFSD(sender, e);
        }

        private void menuItemInsertArrow_Click(object sender, EventArgs e)
        {
            ShapeDesignController shapePrototyping = new ShapeDesignController(myView);
            shapePrototyping.AddArrow();
        }
        private void menuItemViewPorts_Click(object sender, EventArgs e)
        {
            SetPortVisibility(menuItemViewPorts.Checked);
        }
        private void menuItemViewFishLink_Click(object sender, EventArgs e)
        {
            if (ignoreChecking)
                return;
            foreach (GoObject o in myView.Document)
            {
                if (o is FishLink)
                {
                    o.Visible = o.Printable = menuItemViewFishLink.Checked;
                }
            }
            (myView.Document as NormalDiagram).ArtefactPointersVisible = ViewController.ArtefactPointersVisible = menuItemViewFishLink.Checked;
        }
        private void menuItemViewObjectImages_Click(object sender, EventArgs e)
        {
            if (ignoreChecking)
                return;
            (myView.Document as NormalDiagram).ShowObjectImages = menuItemViewObjectImages.Checked;
            BindToMetaObjectProperties(myView.Document);
        }
        private void BindToMetaObjectProperties(ICollection collection)
        {
            foreach (GoObject o in collection)
            {
                if (o is IMetaNode)
                {
                    (o as IMetaNode).BindToMetaObjectProperties();
                    if (o is ICollection)
                        BindToMetaObjectProperties(o as ICollection);
                }
            }
        }

        private void SetPortVisibility(bool portsVisible)
        {
            if (menuItemViewPorts.Checked != portsVisible)
                menuItemViewPorts.Checked = portsVisible;

            foreach (GoObject o in myView.Document)
            {
                if (o is IGoCollection)
                {
                    IGoCollection collection = o as IGoCollection;
                    SetPortVisibilityForCollection(portsVisible, collection);
                }
                if (o is CollapsibleNode)
                {
                    CollapsibleNode cnode = o as CollapsibleNode;
                    foreach (GoObject objC in cnode)
                    {
                        if (objC is CollapsingRecordNodeItemList)
                        {
                            CollapsingRecordNodeItemList list = objC as CollapsingRecordNodeItemList;
                            GoGroupEnumerator genum = list.GetEnumerator();
                            while (genum.MoveNext())
                            {
                                if (genum.Current is RepeaterSection)
                                {
                                    RepeaterSection rsec = genum.Current as RepeaterSection;
                                    rsec.Changed(1001, 0, 0, rsec.Bounds, 0, rsec, rsec.Bounds);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void SetPortVisibilityForCollection(bool portsVisible, IGoCollection collection)
        {
            foreach (GoObject containedObject in collection)
            {
                if (containedObject is GoPort)
                {
                    containedObject.Visible = portsVisible;
                }

                if (containedObject is IGoCollection)
                {
                    SetPortVisibilityForCollection(portsVisible, containedObject as IGoCollection);
                }
            }
        }

        private void menuItemInsertTriangle_Click(object sender, EventArgs e)
        {
            ShapeDesignController shapeProtoTyping = new ShapeDesignController(myView);
            shapeProtoTyping.AddTriangle();
        }
        private void menuItemEditEnableRubberStamping_Click(object sender, EventArgs e)
        {
            ViewController.RubberStampEnabled = !menuItemEditEnableRubberStamping.Checked;
            menuItemEditEnableRubberStamping.Checked = ViewController.RubberStampEnabled;
        }
        private void menuItemToolsAutoRelinkAll_Click(object sender, EventArgs e)
        {
            myView.StartTransaction();
            GoCollection collection = new GoCollection();
            if (myView.Selection.Count > 0)
                collection.AddRange(myView.Selection);
            else
                collection.AddRange(myView.Document);
            AutoRelinkTool arelinktool = new AutoRelinkTool();
            arelinktool.RelinkCollection(collection);
            myView.FinishTransaction("Auto-Relink");
        }
        private void menuItemEditEnableAutoLinking_Click(object sender, EventArgs e)
        {
            ViewController.AutoLinkingEnabled = !menuItemEditEnableAutoLinking.Checked;
            menuItemEditEnableAutoLinking.Checked = ViewController.AutoLinkingEnabled;
        }
        private void menuItemInsertHyperlink_Click(object sender, EventArgs e)
        {
            ShapeDesignController shapePrototyping = new ShapeDesignController(myView);
            shapePrototyping.AddHyperlink();
        }

        #endregion

        private void menuItemShapeDistributeHorizontally_Click(object sender, EventArgs e)
        {
            DistributionManager distroManager = new DistributionManager();
            distroManager.DistributeShapes(myView.Selection, DistributionType.HorizontalGaps);
        }
        private void menuItemShapeVertically_Click(object sender, EventArgs e)
        {
            DistributionManager distroManager = new DistributionManager();
            distroManager.DistributeShapes(myView.Selection, DistributionType.VerticalGaps);
        }
        private void menuItemShapeGroupingUngroup_Click(object sender, EventArgs e)
        {
            ViewController.Ungroup();
        }
        private void menuItemShapeGroupingGroup_Click(object sender, EventArgs e)
        {
            ViewController.Group();
        }
        private void menuItemShapeGroupingCreateSubGraph_Click(object sender, EventArgs e)
        {
            ViewController.MakeSubGraph();
        }
        private void menuItemShapeLayoutArtefacts_Click(object sender, EventArgs e)
        {
            ViewController.LayoutArtefacts();
        }

        private void menuItemEditFind_Click(object sender, EventArgs e)
        {
            ShowFindText();
        }
        private void menuItemEditFindAndReplace_Click(object sender, EventArgs e)
        {
            ShowFindReplace();
        }

        private void menuItemToolsCropToSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*GraphNode gnode = myView.Selection.Primary as GraphNode;
            GoGroupEnumerator groupgnode.GetEnumerator();*/
            myView.StartTransaction();
            myView.ViewController.UpdateSize(MetaBuilder.Graphing.Controllers.GraphViewController.CropType.ToSheet);
            myView.FinishTransaction("Crop Drawing");
        }
        private void menuItemToolsCropToDocumentFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myView.StartTransaction();
            myView.ViewController.UpdateSize(MetaBuilder.Graphing.Controllers.GraphViewController.CropType.ToFrame);
            myView.FinishTransaction("Crop Drawing");
        }
        private void menuItemToolsCropDocument_Click(object sender, System.EventArgs e)
        {
            cropGlobal();
        }

        public void menuItemToolsLoadFromDBRefreshObjects_Click(object sender, EventArgs e)
        {
            //Tools.MetaComparerWorker worker = new MetaBuilder.UIControls.GraphingUI.Tools.MetaComparerWorker(myView.Document, true, this.myView);
            //worker.RunWorkerCompleted += mcomparer_RunWorkerCompletedNoDialog;
            //worker.RunWorkerAsync();

            Tools.MetaComparer mcomparer = new MetaBuilder.UIControls.GraphingUI.Tools.MetaComparer();
            mcomparer.MyView = this.myView;
            mcomparer.RefreshSelection(myView.Document);
        }
        public void menuItemToolsLoadFromDatabaseSelectObjects_Click(object sender, EventArgs e)
        {
            AddObjectsFromDBUsingStencil();
        }
        public void menuItemToolsLoadFromDatabaseSelectDiagram_Click(object sender, EventArgs e)
        {
            menuItemToolsLoadFromDatabaseSelectObjects_Click(sender, e);
        }
        public void menuItemToolsLoadFromDBChangedObjectsAddIndicators_Click(object sender, EventArgs e)
        {
            //CacheManager cacheManager = CacheFactory.GetCacheManager();
            //cacheManager.Flush();
            //Tools.MetaComparerWorker worker = new MetaBuilder.UIControls.GraphingUI.Tools.MetaComparerWorker(myView.Document, false, this.myView);
            //worker.RunWorkerCompleted += mcomparer_RunWorkerCompletedNoDialog;
            //worker.RunWorkerAsync();

            Tools.MetaComparer mcomparer = new MetaBuilder.UIControls.GraphingUI.Tools.MetaComparer();
            mcomparer.MyView = this.myView;
            mcomparer.CompareSelection(myView.Document);
        }
        public void menuItemToolsLoadFromDBChangedObjectsRemoveIndicators_Click(object sender, EventArgs e)
        {
            View.StartTransaction();
            ViewController.RemoveChangedIndicators(myView.Document, true);
            View.FinishTransaction("Add Indicators");
        }
        public void menuItemToolsLoadFromDatabaseArtefactObject_Click(object sender, EventArgs e)
        {
            myView.Document.SkipsUndoManager = true;
            ObjectFinder ofinder = new ObjectFinder(false);
            ofinder.IncludeStatusCombo = true;
            ofinder.ExcludeStatuses.Add(VCStatusList.Obsolete);
            ofinder.ExcludeStatuses.Add(VCStatusList.MarkedForDelete);
            ofinder.ArtefactsOnly = true;
            DialogResult res = ofinder.ShowDialog(DockingForm.DockForm);
            if (res == DialogResult.OK)
            {
                Dictionary<MetaObjectKey, MetaBase> objects = ofinder.SelectedObjects;
                ShapeDesignController controller = new ShapeDesignController(myView);
                foreach (KeyValuePair<MetaObjectKey, MetaBase> kvpair in objects)
                {
                    ArtefactNode afNode = new ArtefactNode();
                    afNode.MetaObject = kvpair.Value;
                    afNode.HookupEvents();
                    afNode.Position = controller.GetCenter();
                    afNode.Label.Text = kvpair.Value.ToString();
                    afNode.BindingInfo = new BindingInfo();
                    afNode.BindingInfo.BindingClass = kvpair.Value._ClassName;
                    afNode.Editable = true;
                    afNode.Label.Editable = false;
                    myView.Document.Add(afNode);
                }
            }

            myView.Document.SkipsUndoManager = false;
        }
        public void menuItemToolsRefreshWholePrompt_Click(object sender, EventArgs e)
        {
            RefreshData(myView.Document, true);
        }
        public void menuItemToolsRefreshWholeAuto_Click(object sender, EventArgs e)
        {
            RefreshData(myView.Document, false);
        }
        public void menuItemToolsRefreshSelectionPrompt_Click(object sender, EventArgs e)
        {
            RefreshData(myView.Selection, true);
        }
        public void menuItemToolsRefreshSelectionAuto_Click(object sender, EventArgs e)
        {
            RefreshData(myView.Selection, false);
        }

        private void validateModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool canValidate = false;
            foreach (GoObject o in myView.Document)
            {
                if (o is IMetaNode)
                {
                    if ((o as IMetaNode).MetaObject.Class == "Entity" || (o as IMetaNode).MetaObject.Class == "DataEntity")
                    {
                        canValidate = true;
                        break;
                    }
                }
            }
#if DEBUG
            canValidate = true;
#endif
            if (canValidate)
            {
                ValidateModel(DockingForm.DockForm.ValidationResultForm);
                //DockingForm.DockForm.ValidationResultForm.ShowResults();
            }
            else
            {
                MessageBox.Show(DockingForm.DockForm, "No entities were found on this diagram to be validated", "Validate ADD", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public void ValidateModel(Tools.DataModel.UI.ValidationResultForm vform)
        {
            Tools.DataModel.Tester tt = new MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Tester();
            tt.ValidateModel(myView.Document as NormalDiagram, myView);
            //DockingForm.DockForm.ValidationResultForm.ShowResults();
        }

        #endregion

        #region ToolStrip Events

        //private void toolstripComboZoom_TextUpdate(object sender, EventArgs e)
        //{
        //    UpdateZoom();
        //}
        //private void toolstripComboZoom_Leave(object sender, EventArgs e)
        //{
        //    UpdateZoom();
        //}
        //private void toolstripComboZoom_DropDownClosed(object sender, EventArgs e)
        //{
        //    UpdateZoom();
        //}
        //private void toolstripComboZoom_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //}

        private void toolStripFormatText_Click(object sender, EventArgs e)
        {
            menuItemFormatText_Click(sender, e);
        }
        private void toolStripFormatFill_Click(object sender, EventArgs e)
        {
            menuItemFormatFill_Click(sender, e);
        }

        #endregion

        private void node_ContentsChanged(object sender, EventArgs e)
        {
            try
            {
                //update indicators ; if it is readonly already you cant edit it
                if (sender is GraphNode)
                {
                    if (!VCStatusTool.UserHasControl((sender as GraphNode).MetaObject))
                    {
                        if ((sender as GraphNode).MetaObject.State == VCStatusList.MarkedForDelete)
                        {
                            ViewController.IndicatorController.AddChangedIndicator("Marked For Delete", System.Drawing.Color.Red, sender as GoGroup);
                        }
                        else
                        {
                            ViewController.IndicatorController.AddIndicator((sender as GraphNode).MetaObject.State.ToString(), System.Drawing.Color.Gray, sender as GoGroup);
                        }
                    }
                }
            }
            catch
            {

            }
            // GraphNode node = (GraphNode) sender;
            // DockingForm.DockForm.ShowProperties(node);
        }
        private void lane_LaneDoubleClick(MappingCell mc)//object sender, EventArgs e)
        {
            if (ReadOnly)
                return;

            bool isAssigned = false;
            if (mc != null && mc.MetaObject != null && mc.MetaObject.pkid > 0)
                isAssigned = true;

            if (isAssigned)
            {
                string sMessageFirstAssign = "This cell/lane is already represented by an object. If you would like to change the object click yes.";
                DialogResult result1 = MessageBox.Show(DockingForm.DockForm, sMessageFirstAssign, "Specify Instance", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result1 == DialogResult.Yes)
                {
                    //MappingCell lane = sender as MappingCell;
                    ObjectFinder ofinder = new ObjectFinder(false);
                    ofinder.AllowMultipleSelection = false;
                    ofinder.IncludeStatusCombo = true;
                    ofinder.ExcludeStatuses.Add(VCStatusList.Obsolete);
                    ofinder.ExcludeStatuses.Add(VCStatusList.MarkedForDelete);
                    DialogResult result = ofinder.ShowDialog(DockingForm.DockForm);
                    if (result == DialogResult.OK)
                    {
                        if (ofinder.SelectedObjects.Count > 0)
                        {
                            if (isAssigned)
                            {
                                List<IMetaNode> overlappingNodes = mc.GetOverlappingIMetaNodes();
                                bool controlsSwimlaneObject = VCStatusTool.UserHasControl(mc.MetaObject);
                                GraphViewController.PromptForKeepExistingAssociations(mc, overlappingNodes, controlsSwimlaneObject);
                            }
                            mc.MetaObject = ofinder.SelectedObjectsList[0];
                            mc.HookupEvents();
                            mc.BindToMetaObjectProperties();
                            mc.FireMetaObjectChanged(mc, new EventArgs());

                            RectangleF newBounds = mc.Bounds;
                            if ((mc.BackGround.Bounds.Width < newBounds.Width) || (mc.BackGround.Bounds.Height > newBounds.Height))
                            {
                                mc.BackGround.Bounds = newBounds;
                            }
                        }
                    }
                }
            }
            else
            {
                //force select object
                ObjectFinder ofinder = new ObjectFinder(false);
                ofinder.AllowMultipleSelection = false;
                ofinder.IncludeStatusCombo = true;
                ofinder.ExcludeStatuses.Add(VCStatusList.Obsolete);
                ofinder.ExcludeStatuses.Add(VCStatusList.MarkedForDelete);
                DialogResult result = ofinder.ShowDialog(DockingForm.DockForm);
                if (result == DialogResult.OK)
                {
                    if (ofinder.SelectedObjects.Count > 0)
                    {
                        //if (isAssigned)
                        //{
                        //    List<IMetaNode> overlappingNodes = mc.GetOverlappingIMetaNodes(myView);
                        //    bool controlsSwimlaneObject = VCStatusTool.UserHasControl(mc.MetaObject);
                        //    GraphViewController.PromptForKeepExistingAssociations(mc, overlappingNodes, controlsSwimlaneObject);
                        //}
                        mc.MetaObject = ofinder.SelectedObjectsList[0];
                        mc.HookupEvents();
                        mc.BindToMetaObjectProperties();
                        mc.FireMetaObjectChanged(mc, new EventArgs());

                        RectangleF newBounds = mc.Bounds;
                        if ((mc.BackGround.Bounds.Width < newBounds.Width) || (mc.BackGround.Bounds.Height > newBounds.Height))
                        {
                            mc.BackGround.Bounds = newBounds;
                        }
                    }
                }
                //else
                //{
                //    //MessageBox.Show(DockingForm.DockForm,"You must choose an object for this lane to be represented by.", "Object Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    //lane_LaneDoubleClick(mc);
                //    //remove lane
                //    //this.myView.Document.Remove(lane);
                //}
            }
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            (View.Document as NormalDiagram).DocumentFrame.Frame.Pen.Brush = Brushes.Gray;//new SolidBrush(Color.Gray);
            //remove all quickpanels
            List<QuickPanel> panels = new List<QuickPanel>();
            foreach (GoObject o in myView.Document)
            {
                if (o is QuickPanel)
                {
                    panels.Add(o as QuickPanel);
                    o.Visible = false;
                }
            }

            foreach (QuickPanel panel in panels)
                myView.Document.Remove(panel);

            //myView.ViewController.removeQuickMenu();
            //1 September 2014
            View.Document.SuspendsUpdates = true;
            if (!ReadOnly)
            {
                try
                {
                    backgroundWorker1.SaveDiagram();
                    DockingForm.DockForm.progressBar1.Tag = backgroundWorker1;
                }
                catch (Exception ex)
                {
                    Core.Log.WriteLog("SAVEDIAGRAM_DOWORK " + ex.ToString());
                }
            }
            else
            {
                if (Core.Variables.Instance.IsViewer)
                {
                    backgroundWorker1.SaveDiagram();
                }
                Log.WriteLog(backgroundWorker1.FileName + " not saving because it is GraphViewContainer is readonly" + Environment.NewLine + Environment.StackTrace);
            }
        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                if (e.UserState.ToString() == "Invalid Directory")
                {
                    if (View.Document is NormalDiagram)
                    {
                        NormalDiagram ndiagram = View.Document as NormalDiagram;
                        if (ndiagram != null)
                        {
                            DockingForm.DockForm.DisplayTip("The file cannot be found at the specified path." + Environment.NewLine + ndiagram.Name, "Save problem", ToolTipIcon.Error);
                            Core.Log.WriteLog("The file cannot be found at the specified path." + Environment.NewLine + ndiagram.Name);
                        }
                    }
                }

                if (!DockingForm.DockForm.progressBar1.IsDisposed)
                    if (DockingForm.DockForm.progressBar1.Maximum < e.ProgressPercentage)
                        DockingForm.DockForm.UpdateTotal(e.ProgressPercentage + 10);

                if (!DockingForm.DockForm.progressBar1.IsDisposed)
                    DockingForm.DockForm.ProgressUpdate(e.ProgressPercentage);

                if (!DockingForm.DockForm.statusLabel.IsDisposed)
                    DockingForm.DockForm.UpdateStatusLabel(e.UserState.ToString());
            }
            catch (Exception ex)
            {
                //file is readonly
                //if (!DockingForm.DockForm.statusLabel.IsDisposed)
                //    DockingForm.DockForm.statusLabel.Text = "Infinite";
                Core.Log.WriteLog(ex.ToString(), "backgroundWorker1_ProgressChanged", System.Diagnostics.TraceEventType.Error);
            }
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (DockingForm.DockForm.CurrentlySaving.Contains(backgroundWorker1))
                DockingForm.DockForm.CurrentlySaving.Remove(backgroundWorker1);
            DockingForm.DockForm.UpdateStatusLabel("Finalizing");
            //1 September 2014
            View.Document.SuspendsUpdates = false;
            //1 September 2014
            View.Document.InvalidateViews();

            if (!ReadOnly)
            {
                try
                {
                    DockingForm.DockForm.DockFormController.mruManager.Add(myView.Document.Name);
                    OnSaveComplete(this, EventArgs.Empty);

                    if (!DockingForm.DockForm.statusLabel.IsDisposed)
                        DockingForm.DockForm.UpdateStatusLabel("Save Complete");
                    if (!DockingForm.DockForm.progressBar1.IsDisposed)
                        DockingForm.DockForm.UpdateTotal(0);

                    if (!this.IsDisposed)
                    {
                        this.Text = Core.strings.GetFileNameWithoutExtension(backgroundWorker1.FileName);

                        DoAfterSaveCleanup();

                        SetFormPropertiesForSave(true);
                    }
                    //TODO : Required? The save is complete, we dont care about the version if its closing?
                    if (DocumentVersion == null)
                    {
                        DocumentVersionManager man = backgroundWorker1.Diagram.VersionManager;
                        DockingForm.DockForm.SetToolstripWorkspaceToDiagram(man.CurrentVersion);
                    }
                    else
                        DockingForm.DockForm.SetToolstripWorkspaceToDiagram(DocumentVersion);

                    //9 January 2013, marked for delete indicators must show after save
                    if (!this.IsDisposed)
                    {
                        menuItemToolsLoadFromDBChangedObjectsRemoveIndicators_Click(backgroundWorker1, EventArgs.Empty);
                        //RefreshData(myView.Document, false);
                        //Console.WriteLine(DateTime.Now.ToString());
                        //System.Threading.Thread.Sleep(10);
                        //menuItemToolsLoadFromDBChangedObjectsAddIndicators_Click(this, new EventArgs());
                    }
                    //February 2013 some objects still show not in database
                    //1 September 2014 - this is because saving failed sometimes when Entities were collapsed before saving and null exceptions occurred
                }
                catch (Exception ex)
                {
                    Log.WriteLog("Unable to Complete Save" + Environment.NewLine + ex.ToString());
                }
            }
            else
            {
                //ReadOnly diagrams still need attention

                if (!Core.Variables.Instance.IsViewer)
                {
                    DockingForm.DockForm.UpdateStatusLabel("Read Only - Cannot Save");
                    DoAfterSaveCleanup();
                }
                else
                {
                    DoAfterSaveCleanup();
                    SetFormPropertiesForSave(true);
                }
            }
            if (!this.IsDisposed)
                ForceSaveAs = false;
            DockingForm.DockForm.ResetStatus();
            if (!this.IsDisposed)
                if (myView.Document.UndoManager != null)
                    myView.Document.UndoManager.Clear();

            if (CloseAfterSave)
                CloseDispose();
        }

        //private BackgroundWorker ViewerSaveWorker;

        private void OnSaveComplete(object sender, EventArgs e)
        {
            if (SaveComplete != null)
            {
                SaveComplete(sender, e);
                ForceSaveAs = false;
                //ViewController.SetDocumentModifiedToFalseIfNotTrue(true);
                //myView.Document.IsModified = false;
            }
        }

        //private void TakeQuickAction(RemoveOrMarkForDelete.ActionToBeTaken actionToBeTaken)
        //{
        //    List<GoObject> objects = new List<GoObject>();
        //    List<RemoveOrMarkForDelete.RepositoryItemAction> actions =
        //        new List<RemoveOrMarkForDelete.RepositoryItemAction>();

        //    foreach (GoObject obj in myView.Selection)
        //    {
        //        if (obj is GraphNode)
        //        {
        //            GraphNode node = obj as GraphNode;
        //            if (node.MetaObject != null)
        //            {
        //                DockingForm.DockForm.GetTaskDocker().RemoveTaskWhereTagEquals(myView, node.MetaObject, typeof(DuplicationTask));
        //            }
        //        }
        //        objects.Add(obj);
        //        RemoveOrMarkForDelete.RepositoryItemAction action = new RemoveOrMarkForDelete.RepositoryItemAction();
        //        action.ActionToBeTaken = actionToBeTaken;
        //        action.MyGoObject = obj;
        //        actions.Add(action);

        //    }

        //    RemoveOrDeleteObjects(new System.ComponentModel.CancelEventArgs(), actions, objects);
        //}

        private void EnableContextShallowCopyItem(bool enabled)
        {
            bool found = false;
            foreach (MenuItem mi in myView.ViewContextMenu.MenuItems)
            {
                if (mi.Text == "Paste Shallow Copy")
                {
                    mi.Enabled = enabled;

                    if (ReadOnly)
                    {
                        mi.Enabled = false;
                        break;
                    }

                    found = true;
                }
            }
            if (!found)
            {
                MenuItem mnuPasteShallowCopy = new MenuItem("Paste Shallow Copy", menuItemEditAddShallowCopy_Click);
                mnuPasteShallowCopy.Enabled = !ReadOnly;
                myView.ViewContextMenu.MenuItems.Add(mnuPasteShallowCopy);
            }
        }
        private bool autosaveSetup;
        public void SetupAutosave()
        {
            try
            {
                if (Variables.Instance.AutoSaveEnabled)
                {
                    if (autosaveSetup)
                        return;

                    if (myView.AllowEdit)
                    {
                        TimerAutoSave = new Timer();
                        TimerAutoSave.Interval = Variables.Instance.AutoSaveInterval * 60000;
                        TimerAutoSave.Tick += new EventHandler(timerAutosave_Tick);
                        TimerAutoSave.Enabled = true;
                        TimerAutoSave.Start();

                        autosaveSetup = true;
                    }
                }
                else
                {
                    TimerAutoSave.Stop();
                    TimerAutoSave.Dispose();
                    TimerAutoSave = null;
                    autosaveSetup = false;
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString());
            }
        }
        private void SetupView(FileTypeList filetype)
        {
            //myView.BeginUpdate();

            //if (myView.Document.UndoManager != null)
            //    myView.Document.UndoManager.ChecksTransactionLevel = true;

            ViewController.MyView = myView;
            myView.ViewController = ViewController;

            if (ViewController.isHooked) //means this has already run
            {
                //ViewController.SetDocumentModifiedToFalseIfNotTrue(false);
                //myView.Document.IsModified = false;
                myView.EndUpdate();
                return;
            }

            #region Event Binding
            myView.SheetStyle = GoViewSheetStyle.Sheet;
            myView.OpenDiagramFromAnother += new EventHandler(myView_OpenDiagramFromAnother);
            myView.PluginProgressReport += new MetaBuilder.Graphing.Helpers.PluginProgressEventHandler(myView_OnPluginProgressReport);
            myView.DragDrop += new DragEventHandler(myView_DragDrop);
            myView.LinkRelinked += new GoSelectionEventHandler(myView_LinkRelinked);
            myView.LinkCreated += new GoSelectionEventHandler(myView_LinkCreated);
            myView.SelectionChanged += new GoSelectionEventHandler(myView_SelectionChanged);
            myView.ExternalObjectsDropped += new GoInputEventHandler(myView_ExternalObjectsDropped);
            myView.SelectionCopied += new EventHandler(myView_SelectionCopied);
            myView.ClipboardPasted += new EventHandler(myView_ClipboardPasted);
            //myView.ItemAdded += new GoSelectionEventHandler(myView_ItemAdded);
            myView.ObjectSelectionDropped += new GoObjectEventHandler(myView_ObjectSelectionDropped);
            myView.KeyUp += new KeyEventHandler(myView_KeyUp);
            myView.KeyDown += new KeyEventHandler(myView_KeyDown);
            myView.ObjectDoubleClicked += new GoObjectEventHandler(myView_ObjectDoubleClicked);
            myView.ObjectSingleClicked += new GoObjectEventHandler(myView_ObjectSingleClicked);
            myView.ObjectGotSelection += new GoSelectionEventHandler(myView_ObjectGotSelection);
            myView.ObjectLostSelection += new GoSelectionEventHandler(myView_ObjectLostSelection);
            myView.SelectionStarting += new EventHandler(myView_SelectionStarting);
            myView.SelectionFinished += new EventHandler(myView_SelectionFinished);

            ViewController.Hookup();
            ViewController.MetaObjectContextRequest += new GraphViewController.MetaObjectSelectedDelegate(ViewController_MetaObjectContextRequest);
            myView.PropertyChanged += new PropertyChangedEventHandler(myView_PropertyChanged);
            myView.ShallowCopy += new EventHandler(myView_ShallowCopy);
            myView.SelectionDeleting += new CancelEventHandler(myView_SelectionDeleting);

            myView.LinkRelinked += new GoSelectionEventHandler(myView_LinkRelinked);
            myView.SelectionMoved += new EventHandler(myView_SelectionMoved);
            myView.Document.Changed += new GoChangedEventHandler(this.Document_Changed);

            myView.EditAllocation += new EventHandler(myView_EditAllocation);
            myView.LostFocus += new EventHandler(myView_LostFocus);
            #endregion

            myView.OverrideDocScaleMath = true;
            myView.DocScale = 1f;
            myView.OverrideDocScaleMath = false;

            myView.NewLinkClass = typeof(QLink);
            myView.AllowDelete = true;
            switch (filetype)
            {
                case FileTypeList.Symbol:
                    Symbol s = new Symbol();
                    myView.Document = s;
                    myView.Sheet.Position = new PointF(myView.Document.TopLeft.X - myView.Sheet.TopLeftMargin.Width, myView.Document.TopLeft.Y - myView.Sheet.BottomRightMargin.Width);
                    RectangleF bounds = s.GetShapeContainer().Bounds;
                    bounds.X = -myView.Sheet.TopLeftMargin.Width;
                    bounds.Y = -myView.Sheet.TopLeftMargin.Height;
                    myView.ScrollRectangleToVisible(bounds);
                    break;
                case FileTypeList.Diagram:
                    NormalDiagram ndiagram = new NormalDiagram();
                    myView.Document = ndiagram;
                    if (ndiagram.FrameLayer == null)
                        ndiagram.CreateFrameLayer(myView);
                    ndiagram.RepositionFrame(myView);
                    ndiagram.FileType = filetype;
                    //ndiagram.AllowFrame(true);
                    if (Variables.Instance.CheckDuplicates)
                    {
                        myView.ObjectEdited += new GoSelectionEventHandler(myView_ObjectEdited);
                    }
                    break;
            }
            myView.Document.TopLeft = new PointF(myView.Sheet.Position.X + myView.Sheet.TopLeftMargin.Width, myView.Sheet.Position.Y + myView.Sheet.TopLeftMargin.Height);
            ViewController.SetupGrid();
            SetTabText();
            myView.Document.LinksLayer = myView.Document.Layers.CreateNewLayerAfter(myView.Document.DefaultLayer);
            myView.DocScale = 1;
            //myView.Document.UndoManager = new GoUndoManager();
            myView.EndUpdate();
            //ViewController.SetDocumentModifiedToFalseIfNotTrue(false);
            //myView.Document.IsModified = false;

            SetupAutosave();
        }

        private void myView_LostFocus(object sender, EventArgs e)
        {

        }

        private void myView_OnPluginProgressReport(object sender, MetaBuilder.Graphing.Helpers.PluginProgressEventArgs e)
        {
            DockingForm.DockForm.DisplayTip("", "");
        }

        private void myView_EditAllocation(object sender, EventArgs e)
        {
            if (sender is AllocationHandle)
            {
                AllocationHandle handle = sender as AllocationHandle;

                MetaBuilder.UIControls.GraphingUI.Tools.Allocation.Allocation a = new MetaBuilder.UIControls.GraphingUI.Tools.Allocation.Allocation(handle.Items);
                DialogResult r = a.ShowDialog(DockingForm.DockForm);
                if (r == DialogResult.OK)
                {
                    handle.Items = a.NewItems;
                    CustomModified = true;
                }
            }
        }

        private void ShowSubgraphBindingForm(GoObject obj)
        {
            ILinkedContainer sgnode = obj as ILinkedContainer;
            // Show Dialog
            SubgraphBinding.EditSubgraphBinding editBindings = new MetaBuilder.UIControls.GraphingUI.SubgraphBinding.EditSubgraphBinding();
            editBindings.Document = this.myView.Doc as NormalDiagram;
            editBindings.ILContainer = sgnode;

            if (sgnode is MappingCell)
            {
                sgnode.DefaultClassBindings = new Dictionary<string, ClassAssociation>();
                sgnode.ObjectRelationships = new List<EmbeddedRelationship>();
                SubgraphBinding.LinkedContainerControlller controller = new MetaBuilder.UIControls.GraphingUI.SubgraphBinding.LinkedContainerControlller();
                foreach (IMetaNode imn in (sgnode as MappingCell).GetOverlappingIMetaNodes())
                {
                    if (imn is GoObject)
                    {
                        controller.AddDefaultAssociationInfoForChildNode(sgnode, (imn as GoObject));
                    }
                }
                //to this controller add database associations which are currently NOT present
                controller.AddAssociationsWhichAreInDatabase(sgnode);
            }
            editBindings.BindData();
            editBindings.ShowDialog(DockingForm.DockForm);
        }
        private Collection<Checker> activeDuplicateCheckers;
        private Collection<Checker> ActiveDuplicateCheckers
        {
            get
            {
                if (activeDuplicateCheckers == null)
                    activeDuplicateCheckers = new Collection<Checker>();
                return activeDuplicateCheckers;
            }
        }
        private void FindDuplicates(IMetaNode node)
        {
            if (duplicateCheckers == null)
                duplicateCheckers = new Dictionary<IMetaNode, Checker>();
            node.RequiresAttention = false;
            if (node.MetaObject != null)
            {
                //if (!string.IsNullOrEmpty(node.MetaObject.ToString()))// && node.MetaObject.pkid == 0)
                {
                    Checker duplicateChecker;
                    if (!duplicateCheckers.ContainsKey(node))
                    {
                        // need to have more than one checker for multiple drag operations
                        duplicateChecker = new Checker();
                        duplicateChecker.NodeThatIsBeingChecked = node;
                        duplicateChecker.MyView = View;

                        //if ((this.View.Document as NormalDiagram) != null && (this.View.Document as NormalDiagram).VersionManager != null && (this.View.Document as NormalDiagram).VersionManager.CurrentVersion != null)
                        //    duplicateChecker.FileUniqueID = (this.View.Document as NormalDiagram).VersionManager.CurrentVersion.OriginalFileUniqueIdentifier.ToString();

                        duplicateChecker.DuplicatesFound += new ObjectsFoundEventHandler(duplicateChecker_DuplicatesFound);
                        duplicateChecker.NoDuplicatesFound += new NoDuplicatesFoundEventHandler(duplicateChecker_NoDuplicatesFound);
                        duplicateChecker.TestBackgroundFinder(node.MetaObject);
                        //if (!duplicateCheckers.ContainsKey(node))
                        duplicateCheckers.Add(node, duplicateChecker);
                        ActiveDuplicateCheckers.Add(duplicateChecker);
                    }
                    else
                    {
                        //get checker and run it again with the new metaobject
                        duplicateChecker = null;
                        duplicateCheckers.TryGetValue(node, out duplicateChecker);
                        if (duplicateChecker != null)
                        {
                            duplicateChecker.TestBackgroundFinder(node.MetaObject);
                        }
                    }
                }
            }
        }
        // This is for Duplicate Management, otherwise the window switches back to metaprops
        private void DisplayMetaProps()
        {
            if (ItemsHaveBeenAddeding)
                return;
            if (myView.Selection.Primary != null)
            {
                GoObject o = myView.Selection.Primary;
                if (o is CollapsingRecordNodeItem)
                {
                    CollapsingRecordNodeItem citem = o as CollapsingRecordNodeItem;
                    if (citem.MetaObject != null)
                    {
                        DockingForm.DockForm.ShowMetaObjectProperties(citem.MetaObject);
                    }
                }
                else if (o.DraggingObject is IMetaNode)
                {
                    IMetaNode n = o.DraggingObject as IMetaNode;
                    /*if (o.DraggingObject is GraphNode)
                   {
                       GraphNode gnode = o.DraggingObject as GraphNode;
                      if (gnode.Grid != null)
                           if (gnode.EditMode && gnode.Grid.Selectable)
                           {
                               DockingForm.DockForm.ShowProperties(gnode.Grid);
                               myView.Focus();
                               return;
                           }
                   }*/
                    //DockingForm.DockForm.ShowProperties(o);
                    if (n.MetaObject != null)
                        if (readoOnly)
                            DockingForm.DockForm.ShowMetaObjectProperties(n.MetaObject, false);
                        else
                            DockingForm.DockForm.ShowMetaObjectProperties(n.MetaObject);
                }
                //8 January 2013
                else if (o.DraggingObject is ShapeGroup)
                {
                    if (!(o is IMetaNode))
                        return;
                    IMetaNode n = o as IMetaNode;
                    //DockingForm.DockForm.ShowProperties(o);
                    if (n.MetaObject != null)
                        DockingForm.DockForm.ShowMetaObjectProperties(n.MetaObject);
                }
                else if (o is QLink)
                {
                    DockingForm.DockForm.ShowMetaObjectProperties(o);
                    //DockingForm.DockForm.ShowProperties(o);
                }
                else
                {
                    DockingForm.DockForm.ShowMetaObjectProperties(null);
                    //DockingForm.DockForm.ShowProperties(o);
                }
            }
            else
            {
                DockingForm.DockForm.ShowDiagramProperties();
            }
            //the view must always have focus
            myView.Focus();
        }
        private QLink parentLink = null;
        private void TestValidLink(QLink l, bool KeepAssociationType)
        {
            IMetaNode to = (parentLink != null) ? parentLink.ToNode as IMetaNode : l.ToNode as IMetaNode;
            IMetaNode from = (parentLink != null) ? parentLink.FromNode as IMetaNode : l.FromNode as IMetaNode;

            if (allowedLinks == null)
                allowedLinks = new Dictionary<ValidClassAssociationKey, List<ClassAssociation>>();
            if (to.HasBindingInfo && from.HasBindingInfo)
            {
                List<ClassAssociation> allowedAssociations;
                ValidClassAssociationKey vcaKey = new ValidClassAssociationKey();
                vcaKey.ParentClass = from.BindingInfo.BindingClass;
                vcaKey.ChildClass = to.BindingInfo.BindingClass;

                //foreach (KeyValuePair<ValidClassAssociationKey, List<ClassAssociation>> k in allowedLinks)
                //{
                //    if (k.Key.ParentClass.ToLower() == vcaKey.ParentClass.ToLower() && k.Key.ChildClass.ToLower() == vcaKey.ChildClass.ToLower())
                //    {
                //        vcaKey = k.Key;
                //        break;
                //    }
                //}

                if (allowedLinks.ContainsKey(vcaKey))
                {
                    allowedAssociations = allowedLinks[vcaKey];
                }
                else
                {
                    allowedAssociations = AssociationManager.Instance.GetAssociationsForParentAndChildClasses(from.MetaObject.Class, to.MetaObject.Class);
                    allowedLinks.Add(vcaKey, allowedAssociations);
                }

                if (allowedAssociations == null || allowedAssociations.Count == 0)
                {
                    MessageBox.Show(DockingForm.DockForm, "According to the MetaModel configuration, this link is not allowed (" + from.MetaObject.Class + " " + l.AssociationType.ToString() + " " + from.MetaObject.Class + ")", "Link will be removed.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    l.Remove();
                }
                else
                {
                    LinkAssociationType currentAssociationType = l.AssociationType;
                    l.Initializing = true;
                    bool founddefaultlinktype = false;
                    if (!KeepAssociationType)
                    {
                        foreach (ClassAssociation classAssoc in allowedAssociations)
                        {
                            if (to.MetaObject == from.MetaObject && to.MetaObject.Class == "Object" && from.MetaObject.Class == "Object")
                            {
                                l.AssociationType = (LinkAssociationType)19;
                                founddefaultlinktype = true;
                                break;
                            }
                            else
                            {
                                if (classAssoc.IsDefault)// || currentAssociationType == (LinkAssociationType)classAssoc.AssociationTypeID)
                                {
                                    l.AssociationType = (LinkAssociationType)classAssoc.AssociationTypeID;
                                    founddefaultlinktype = true;
                                }
                            }
                        }
                    }
                    if (!founddefaultlinktype)
                    {
                        bool canStayTheSame = false;
                        if (KeepAssociationType)
                        {
                            foreach (ClassAssociation classAss in allowedAssociations)
                            {
                                if (currentAssociationType == (LinkAssociationType)classAss.AssociationTypeID)
                                {
                                    canStayTheSame = true;
                                }
                            }
                        }

                        if (!canStayTheSame)
                        {
                            l.AssociationType = (LinkAssociationType)allowedAssociations[0].AssociationTypeID;
                        }
                    }
                    //l.CalculateStroke();
                    // l.ChangeStyle();
                    l.Initializing = false;

                    if (l.AssociationType == 0)
                    {
                        Log.WriteLog("TestValidLink bug" + Environment.NewLine + Environment.StackTrace);
                        l.Remove();
                    }
                }
            }
            else
            {
                Log.WriteLog("Missing binding info" + Environment.NewLine + Environment.StackTrace);
            }
        }
        private void ResetDockingForm()
        {
            if (!IsSavingFromMDIParent)
            {
                DockingForm.DockForm.ResetStatus();
            }
        }
        //private void CustomDispose()
        //{
        //    if (timerAutoSave != null)
        //    {
        //        timerAutoSave.Stop();
        //        timerAutoSave = null;
        //    }
        //    DockingForm.DockForm.RemoveLockedFile(myView.Document.Name);
        //    //DockingForm.DockForm.RemovePropertyFocus();
        //}
        private void DoAfterSaveCleanup()
        {
            ViewController.RemoveSavedChangedIndicators(myView.Document, true);
            if (!Core.Variables.Instance.IsViewer)
                Loader.FlushDataViews();
            //myView.Document.SerializesUndoManager = true;
            //myView.Document.UndoManager = new GoUndoManager();
            //if (!IsSavingFromMDIParent)
            //    SetFormPropertiesForSave(true);
            Notify();
            //ViewController.SetDocumentModifiedToFalseIfNotTrue(true);
            //myView.Document.IsModified = false;
            if ((myView.Document as NormalDiagram).WasNumbered)
            {
                ExecutePlugin("Hierarchy Numbering");
                //ExecutePlugin("Remove Numbering");
                //(myView.Document as NormalDiagram).WasNumbered = true;
            }
        }
        private void PrepareForSave(NormalDiagram normalDiagram)
        {
            if ((myView.Document as NormalDiagram).WasNumbered)
            {
                //ExecutePlugin("Hierarchy Numbering");
                ExecutePlugin("Remove Numbering");
                (myView.Document as NormalDiagram).WasNumbered = true;
            }

            SetFormPropertiesForSave(false);
            normalDiagram.VersionManager.IncreaseMinorVersion();
            normalDiagram.SerializesUndoManager = false;
            normalDiagram.SheetPosition = myView.Sheet.Position;
            normalDiagram.SheetSize = myView.Sheet.Size;
            normalDiagram.DocScale = myView.DocScale;
            normalDiagram.LastViewPoint = myView.DocPosition;
            normalDiagram.LastVisibleRectangle = new RectangleF(myView.DocPosition, myView.DocumentSize);
            normalDiagram.FrameSize = normalDiagram.DocumentFrame.Size;
            //RemoveVCIndicators(normalDiagram);
            //ViewController.RemoveChangedIndicators(normalDiagram);
            normalDiagram.VersionManager.CurrentVersion.SheetBounds = myView.Sheet.Bounds;

            myView.ViewController.UpdateFrameDate();

            myView.Document.UndoManager.Clear();
        }
        private void SetFormPropertiesForSave(bool done)
        {
            ////Controlled by notify!
            //menuItemFileSave.Enabled = done;
            //menuItemFileSaveAs.Enabled = done;
            //stripButtonSave.Enabled = done;
            myView.AllowCopy = done;
            myView.AllowDelete = done;
            myView.AllowDragOut = done;
            myView.AllowDrop = done;
            myView.AllowEdit = done;
            myView.AllowInsert = done;
            myView.AllowLink = done;
            myView.AllowMouse = done;
            myView.AllowMove = done;
            myView.AllowReshape = done;
            myView.AllowResize = done;
            myView.AllowSelect = done;

            if (!done)
                return;

            //23 January 2013 return all subgraphs to original expansion (they are all expanded before/during save)
            foreach (GoObject o in myView.Document)
            {
                if (o is SubgraphNode)
                {
                    checkInsideSubgraphsForSubgraphsThatNeedToBeExpanded(o as SubgraphNode);
                }
            }
            CustomModified = !done;

            //15 Jan 2014 - After save it cannot be modified
            myView.Document.IsModified = !done;

            myView.ResumeLayout();
            myView.EndUpdate();
            myView.Document.UpdateViews();
            myView.Refresh();
        }

        private void checkInsideSubgraphsForSubgraphsThatNeedToBeExpanded(SubgraphNode node)
        {
            foreach (KeyValuePair<SubgraphNode, bool> p in subgraphsToCheck)
            {
                if (p.Value == false)
                    if (node == p.Key)
                        node.Collapse();
            }

            foreach (GoObject o in node.GetEnumerator())
            {
                if (o is SubgraphNode)
                {
                    checkInsideSubgraphsForSubgraphsThatNeedToBeExpanded(o as SubgraphNode);
                }
            }
        }

        private void SaveFile(NormalDiagram ndiagram, bool IsAutoSave)
        {
            try
            {
                PrepareForSave(ndiagram);
                //SetFormPropertiesForSave(false);
                //FileUtil futil = new FileUtil();
                //futil.Save(ndiagram, ndiagram.Name);

                //delete autosave (perhaps have an option for this?)
                DeleteAutosaveFile();

                if (DockingForm.DockForm != null)
                {
                    //if (closingArgs != null)
                    //    DockingForm.DockForm.OpenedFiles.Remove(ndiagram.Name.ToLower());
                    if (!IsSavingFromMDIParent)
                    {
                        menuItemFileSave.Enabled = true;
                        menuItemFileSaveAs.Enabled = true;
                        //SetFormPropertiesForSave(false);
                        if (!DockingForm.DockForm.OpenedFiles.ContainsKey(ndiagram.Name.ToLower()))
                        {
                            DockingForm.DockForm.OpenedFiles.Add(ndiagram.Name.ToLower(), this.containerID);
                        }
                    }
                    else
                    {
                        DockingForm.DockForm.OpenedFiles.Remove(ndiagram.Name.ToLower());
                    }
                }
                if (!IsAutoSave) // Skip saving to the database
                {
                    if (backgroundWorker1.IsBusy) //this will never happen because each worker is in its own container. Requires seperation
                    {
                        DockingForm.DockForm.DisplayTip("A saving operation is already progress. Wait for it to complete before saving.", "Already saving.", ToolTipIcon.Info);
                        Notify();
                        return;
                    }
                    if (!DockingForm.DockForm.CurrentlySaving.Contains(backgroundWorker1))
                        DockingForm.DockForm.CurrentlySaving.Add(backgroundWorker1);
                    DockingForm.DockForm.GetTaskDocker().RemoveTaskList(this.ContainerID.ToString());
                    //int imsgs = 0;
                    backgroundWorker1.View = this.View;
                    backgroundWorker1.Diagram = ndiagram;
                    //backgroundWorker1.IsViewer = DockingForm.DockForm.IsViewer;
                    backgroundWorker1.FileName = ndiagram.Name.Replace("Autosave ", ""); ;
                    backgroundWorker1.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString());
            }
            Notify();
        }
        private void DeleteAutosaveFile()
        {
            if (autosaveFileName != null)
            {
                try
                {
                    if (File.Exists(autosaveFileName))
                        File.Delete(autosaveFileName);
                }
                catch
                {
                }
            }
        }
        private void ShowFormatForm(int DefaultPage)
        {
            if (myView != null)
                myView.StartTransaction();
            if (View.Selection.Count > 0)
            {
                //bool hasHyperlinks = false;
                //foreach (GoObject o in View.Selection)
                //    if (o is Hyperlink)
                //    {
                //        hasHyperlinks = true;
                //    }

                //if (hasHyperlinks)
                //    return;

                Main mFormatting = new Main();
                if (myView.Document.UndoManager != null)
                    myView.Document.UndoManager.CurrentEdit = new GoUndoManagerCompoundEdit();
                mFormatting.MyView = myView;
                mFormatting.ObjectCollection = myView.Selection;
                //mFormatting.Manipulator = new MetaBuilder.Graphing.Formatting.FormattingManipulator(myView.Selection);
                mFormatting.SetPropertyGridObject(myView.Selection.Primary); //swapped up
                mFormatting.Init(); //swapped down
                mFormatting.GoToPage(DefaultPage);
                mFormatting.ShowDialog(DockingForm.DockForm);
            }
            myView.FinishTransaction("ShowFormatForm");
        }

        //private void UpdateZoom()
        //{
        //    try
        //    {
        //        string selectedZoomText;
        //        if (toolstripComboZoom.SelectedItem != null)
        //            selectedZoomText = toolstripComboZoom.SelectedItem.ToString().Replace("%", "").Trim();
        //        else
        //            selectedZoomText = toolstripComboZoom.ComboBox.Text.Replace("%", "").Trim();
        //        float factor = float.Parse(selectedZoomText);
        //        zController.Zoom(factor / 100f);
        //    }
        //    catch
        //    {
        //    }
        //}
        public void AddObjectsFromDBUsingStencil()
        {
            Tools.LoadFromDatabase ldb = new MetaBuilder.UIControls.GraphingUI.Tools.LoadFromDatabase();
            ldb.AddObjectsFromDBUsingStencil(this.myView);

            SetPortVisibility(true);
        }

        //public void Attach(IViewObserver observer)
        //{
        //    if (!observers.Contains(observer))
        //    {
        //        observers.Add(observer);
        //    }
        //}
        //public void Detach(IViewObserver observer)
        //{
        //    for (int i = 0; i < observers.Count; i++)
        //    {
        //        if (observers[i] == observer)
        //        {
        //            observers.RemoveAt(i);
        //        }
        //    }
        //}
        public void Notify()
        {
            //if (observers != null)
            //    foreach (IViewObserver observer in observers)
            //    {
            //        observer.Update();
            //    }
            UpdateMenuItems();
        }
        public void UpdateMenuItems()
        {
            this.TabPageContextMenu = contextMenuTabPage;
            if (DockingForm.DockForm.WindowState != FormWindowState.Minimized && DockingForm.DockForm.IsActiveWindow)
            {
                if (!myView.BusySelecting)
                {
                    //if (this.WindowState == FormWindowState.Normal || this.WindowState == FormWindowState.Maximized)
                    {
                        DockingForm.DockForm.UpdateMenuItems();

                        //if (DockingForm.DockForm.IsActiveWindow)
                        {
                            //  return;
                            bool isnormal = myView.Document is NormalDiagram;
                            bool issymbol = myView.Document is Symbol;
                            bool shapesSelected = (myView.Selection.Count > 0);
                            menuItemShapeAlign.Enabled = shapesSelected;
                            foreach (ToolStripItem mitem in menuItemShapeAlign.DropDownItems)
                            {
                                mitem.Enabled = shapesSelected;
                            }
                            menuItemShapeDistribute.Enabled = myView.Selection.Count > 1;
                            foreach (ToolStripItem mitem in menuItemShapeDistribute.DropDownItems)
                            {
                                mitem.Enabled = menuItemShapeDistribute.Enabled;
                            }
                            menuItemShapeGrouping.Enabled = shapesSelected;
                            menuItemShapeGroupingCreateSubGraph.Enabled = shapesSelected && !(myView.Selection.Primary.ParentNode is GoSubGraph);
                            menuItemShapeGroupingGroup.Enabled = shapesSelected && !(myView.Selection.Primary.ParentNode is ShapeGroup);
                            menuItemShapeOrder.Enabled = shapesSelected;

                            //9 January 2013 if selected imetanodes parent is ilinkedcontainer disable grouping
                            foreach (GoObject o in myView.Selection)
                            {
                                if (o is GoNode)
                                {
                                    if ((o as GoNode).Parent is ILinkedContainer)
                                    {
                                        menuItemShapeGroupingGroup.Enabled = false;
                                        menuItemShapeGroupingCreateSubGraph.Enabled = false;
                                    }
                                }
                            }

                            if (myView.Selection.Primary != null)
                            {
                                menuItemShapeGroupingUngroup.Enabled = (myView.Selection.Primary is ShapeGroup) || (myView.Selection.Primary.ParentNode is ShapeGroup);
                                menuItemShapeGroupingUngroupSubGraph.Enabled = (myView.Selection.Primary is GoSubGraph) || (myView.Selection.Primary.ParentNode is GoSubGraph);
                            }
                            else
                            {
                                menuItemShapeGroupingUngroup.Enabled = false;
                                menuItemShapeGroupingUngroupSubGraph.Enabled = false;
                            }
                            foreach (ToolStripItem mitem in menuItemShapeOrder.DropDownItems)
                            {
                                mitem.Enabled = shapesSelected;
                            }

                            menuItemEditCopy.Enabled = shapesSelected;
                            menuItemEditPasteShallowCopy.Enabled = DockingForm.DockForm.ShallowCopies != null;
                            //string graphObjNode = typeof(GoObject).FullName;
                            menuItemEditPaste.Enabled = myView.CanEditPaste(); //Clipboard.ContainsData(graphObjNode);
                            // TODO: re-enable this (Clipboard.ContainsData(graphObjNode) || (Clipboard.ContainsText()));
                            menuItemEditRedo.Enabled = myView.CanRedo();
                            menuItemEditUndo.Enabled = myView.CanUndo();
                            // UITGEHAAL VIR BROES menuItemFileProperties.Enabled = isnormal;
                            bool ContainsShape = ViewController.ContainsShape(myView.Selection);
                            bool ContainsText = ViewController.ContainsText(myView.Selection);
                            //if (shapesSelected)
                            //{
                            //    menuItemFormatFill.Enabled = ContainsShape;
                            //    menuItemFormatText.Enabled = ContainsText; // (myView.Selection.Primary is GoText);
                            //    menuItemFormatLine.Enabled = ContainsShape;
                            //    // (myView.Selection.Primary is GoText) || (myView.Selection.Primary is GoShape);
                            //}
                            //else
                            //{
                            //    menuItemFormatFill.Enabled = false;
                            //    menuItemFormatText.Enabled = false;
                            //    menuItemFormatLine.Enabled = false;
                            //}

                            if (myView.Selection.Count == 0)
                            {
                                this.toolstripCopyFormatting.Enabled = false;
                                //this.toolStripFormatFill.Enabled = false;
                                //this.toolStripFormatText.Enabled = false;

                                menuItemToolsConvertCommentsToRationales.Text = "Convert All Comments to Rationales";
                            }
                            else
                            {
                                bool primarySelectionIsNode = myView.Selection.Primary.TopLevelObject is IMetaNode;
                                if (primarySelectionIsNode)
                                {
                                    this.toolstripCopyFormatting.Enabled = ContainsShape || ContainsText;
                                }
                                //this.toolStripFormatFill.Enabled = ContainsShape;
                                //this.toolStripFormatText.Enabled = ContainsText;

                                menuItemToolsConvertCommentsToRationales.Text = "Convert Selected Comments to Rationales";
                            }

                            menuItemFormatFill.Visible = false;
                            menuItemFormatText.Visible = false;
                            menuItemFormatLine.Visible = false;
                            toolStripFormatFill.Visible = false;
                            toolStripFormatText.Visible = false;

                            menuItemEditCut.Enabled = shapesSelected;
                            menuItemEditCopy.Enabled = shapesSelected;
                            menuItemEditPaste.Enabled = myView.CanEditPaste();
                            menuItemEditPasteShallowCopy.Enabled = myView.CanEditPaste();

                            cropToolStripMenuItem.Enabled = isnormal;
                            //slanes,validate,relink
                            menuItemToolsAutoRelinkAll.Enabled = isnormal;
                            menuItemFormatCustomProperties.Visible = Variables.Instance.ShowDeveloperItems;
                            stripButtonCopy.Enabled = shapesSelected;
                            stripButtonCut.Enabled = shapesSelected;
                            stripButtonPaste.Enabled = myView.CanEditPaste();

                            //bool mod = (ViewController.MyView.Document.IsModified && myView.AllowEdit && !ReadOnly);
                            menuItemFileSave.Enabled = true;
                            menuItemFileSaveAs.Enabled = true;
                            stripButtonSave.Enabled = true;

                            EnableContextShallowCopyItem(menuItemEditPasteShallowCopy.Enabled = DockingForm.DockForm.ShallowCopies != null);
                            ViewController.CheckForILinkContainers();

                            TextFormatPanelVisibility(myView.Selection.Count > 0);
                            ColorFormatPanelVisibility();
                        }
                    }
                    UpdateReadonly();

                    menuItemViewFishLink.Checked = ViewController.ArtefactPointersVisible;
                }
            }
            //Re Update menu (Performance)
            ForceSaveAs = ForceSaveAs;
            ReadOnly = ReadOnly;
        }
        public void SetTabText()
        {
            if (myView.Document.Name != string.Empty)
            {
                Text = strings.GetFileNameOnly(myView.Document.Name);
            }
            else
            {
                if (myView.Doc != null)
                    Text = "New " + myView.Doc.FileType.ToString();
            }
            Name = Text;
        }
        //public void UpdateDocumentLists(GoDocument ndiagram)
        //{
        //DockingForm.DockForm.ResetStatus();
        //DockingForm.DockForm.GetTaskDocker().RemoveTaskList(this.ContainerID.ToString());
        //DockingForm.DockForm.UpdatePanWindow();
        //DockingForm.DockForm.UpdateWindowList();
        //}
        public void AbortDuplicateChecking()
        {
            if (duplicateCheckers != null)
            {
                foreach (KeyValuePair<IMetaNode, Checker> kvp in duplicateCheckers)
                {
                    kvp.Value.Abort();
                }
            }
        }
        //when we edit an object check it and and shallow copy of it
        public void MarkDuplicates(MetaBase mbase)
        {
            List<IMetaNode> boundNodes = ViewController.GetIMetaNodesBoundToMetaObject(mbase);
            foreach (IMetaNode node in boundNodes)
            {
                //7 January 2013
                //node.FireMetaObjectChanged(this, new EventArgs());
                FindDuplicates(node);
            }
        }
        private bool ItemsHaveBeenAddeding;
        public void ItemsHaveBeenAdded()
        {
            ItemsHaveBeenAddeding = true;
            //myView.Document.SkipsUndoManager = false;
            //myView.StartTransaction();
            try
            {
                GoCollectionEnumerator enumerator = myView.Selection.GetEnumerator();
                //List<GraphNode> nodesToCheck = new List<GraphNode>();
                Collection<IMetaNode> nodes = new Collection<IMetaNode>();
                Collection<GoObject> objectsToTranslate = new Collection<GoObject>();
                while (enumerator.MoveNext())
                {
                    //this for locationtranslation
                    if (enumerator.Current is IMetaNode)
                    {
                        nodes.Add(enumerator.Current as IMetaNode);
                    }

                    if (enumerator.Current is GraphNode)
                    {
                        GraphNode node = enumerator.Current as GraphNode;
                        node.Shadowed = false;
                        //if (!nodesToCheck.Contains(node) && node.MetaObject != null)
                        //{
                        //    if (!node.CopyAsShadow)
                        //        nodesToCheck.Add(node);
                        //}
                    }
                    else if (enumerator.Current.ParentNode is GraphNode)
                    {
                        GraphNode node = enumerator.Current.ParentNode as GraphNode;
                        node.Shadowed = false;
                        //if (!nodesToCheck.Contains(node) && node.MetaObject != null)
                        //{
                        //    if (!node.CopyAsShadow)
                        //        nodesToCheck.Add(node);
                        //}
                    }
                    else
                    {
                        //if (enumerator.Current is GoGroup)
                        //{
                        //    GoGroup grp = enumerator.Current as GoGroup;
                        //    GoGroupEnumerator grpEnum = grp.GetEnumerator();
                        //    while (grpEnum.MoveNext())
                        //    {
                        //        if (grpEnum.Current.ParentNode is GraphNode)
                        //        {
                        //            GraphNode groupednode = grpEnum.Current.ParentNode as GraphNode;
                        //            if (!nodesToCheck.Contains(groupednode) && groupednode.MetaObject != null)
                        //                if (!groupednode.CopyAsShadow)
                        //                    nodesToCheck.Add(groupednode);
                        //        }
                        //    }
                        //}
                    }

                    GoObject obj = enumerator.Current as GoObject;
                    //if (!(obj is Rationale || obj is ArtefactNode))
                    objectsToTranslate.Add(obj);
                }

                //7 January 2013 - why < 5? performance vs accuracy?
                //if (nodesToCheck.Count < 5)
                //foreach (GraphNode n in nodesToCheck)
                //{
                //Allan - Copy Paste Works
                //if (n is CollapsibleNode)
                //{
                //    //make all children have copiedfrom=null
                //    foreach (RepeaterSection oChildList in (n as CollapsibleNode).RepeaterSections)
                //    {
                //        foreach (GoObject oChild in oChildList)
                //        {
                //            if (oChild is IMetaNode)
                //                (oChild as IMetaNode).CopiedFrom = null;
                //        }
                //    }
                //}
                //if (!ExternalObjectDropped)
                //{
                //    //Tools.MetaComparer mcomparer = new MetaBuilder.UIControls.GraphingUI.Tools.MetaComparer();

                //    //remove indicators.
                //    ViewController.RemoveChangedIndicators(n, true);
                //    viewController.RemoveVCIndicators(n);
                //    //mcomparer.MyView = this.myView;
                //    //add indicators again if necessary, if not dropping from stencil
                //    //mcomparer.CompareSelection(n); //Causes pasting complex objects to have incorrect child items
                //}

                //if (Variables.Instance.CheckDuplicates)
                //{
                //    MarkDuplicates(n.MetaObject);
                //}
                //}

                if (!ExternalObjectDropped)
                {
                    locationTranslation(objectsToTranslate);
                    //locationTranslation(nodes);
                }

                //GoCollection markDuplicateCollection = new GoCollection();
                foreach (IMetaNode n in nodes)
                {
                    if (!ExternalObjectDropped)
                    {
                        ViewController.RemoveChangedIndicators(n as GoGroup, true);
                        ViewController.RemoveVCIndicators(n as GoGroup);
                    }
                    n.HookupEvents();
                    if (Core.Variables.Instance.SaveOnCreate)
                    {
                        if (DocumentVersion != null && (n.MetaObject.WorkspaceName == null || n.MetaObject.WorkspaceTypeId == 0))
                        {
                            n.MetaObject.WorkspaceName = DocumentVersion.WorkspaceName;
                            n.MetaObject.WorkspaceTypeId = DocumentVersion.WorkspaceTypeId;
                        }
                        n.FireMetaObjectChanged(this, EventArgs.Empty);
                    }
                    //n.BindToMetaObjectProperties();
                    //n.BindToMetaObjectImage();
                    //if (n is GoObject)
                    //    markDuplicateCollection.Add(n as GoObject);
                }

                MarkAsDuplicates(myView.Selection);
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString());
            }

            //myView.FinishTransaction("ItemsHaveBeenAdded(Normal Paste)");
            //ViewController.removeQuickMenu();
            ItemsHaveBeenAddeding = false;
        }
        public Collection<HighlightedObject> DoHighlights(Dictionary<string, MetaObjectKey> objects)
        {
            Collection<Dialogs.HighlightedObject> retval = new Collection<HighlightedObject>();
            List<IMetaNode> nodes = ViewController.GetIMetaNodes();
            ViewController.RemoveHighlights();
            foreach (KeyValuePair<string, MetaObjectKey> kvp in objects)
            {
                foreach (IMetaNode imnode in nodes)
                {
                    if (imnode.MetaObject.pkid == kvp.Value.pkid && imnode.MetaObject.MachineName == kvp.Value.Machine)
                    {
                        if (imnode is GoObject)
                        {
                            GoObject gnode = imnode as GoObject;
                            ViewController.HighlightNode(gnode, true);

                            retval.Add(new HighlightedObject(imnode.MetaObject, gnode, this.myView.Document.Name));
                        }
                    }
                }
            }
            return retval;
        }

        //public void PerformSave(Object state)
        //{
        //    AutoResetEvent are = (AutoResetEvent)state;
        //    IsSavingFromMDIParent = true;
        //    NormalDiagram ndiagram = ViewController.GetDiagram();
        //    if (ndiagram != null)
        //    {
        //        //fix READONLY START
        //        CheckModifiedAndSaveIfRequired(ndiagram);
        //    }
        //    are.Set();
        //}
        //public void CheckModifiedAndSaveIfRequired(NormalDiagram ndiagram)
        //{
        //    if (myView.Document.IsModified)
        //    {
        //        this.DialogResult = MessageBox.Show(DockingForm.DockForm, "The " + myView.Doc.FileType.ToString() + " has changed. Do you want to save changes?", "Save changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        //    }

        //    switch (this.DialogResult)
        //    {
        //        case System.Windows.Forms.DialogResult.Yes:
        //            DetermineFilename(ndiagram, false);
        //            // now it will be OK or CANCEL
        //            switch (this.DialogResult)
        //            {
        //                case DialogResult.OK:
        //                    PrepareForSave(ndiagram);
        //                    FilePathManager.Instance.SetLastUsedPath(myView.Doc.FileType, ndiagram.Name);
        //                    SaveFile(ndiagram, false);
        //                    break;
        //            }
        //            break;
        //        case System.Windows.Forms.DialogResult.No:
        //            UpdateDocumentLists(ndiagram);
        //            break;
        //    }
        //}

        public void DetermineFilename(GoDocument document, bool ForcePromptForFilename)
        {
            string filename = string.Empty;
            // set filename to last name
            if (document.Name != null)
            {
                filename = document.Name;
            }
            bool ValidName = (filename != string.Empty && (System.IO.File.Exists(filename))); //changed from dir.exists (strings.getdironly)

            if (ForceSaveAs)
            {
                if (!ValidName)
                {
                    Log.WriteLog("DetermineFilename::" + document.Name + " is invalid-->set to diagrampath" + Environment.NewLine + Environment.StackTrace);
                    document.Name = Variables.Instance.DiagramPath + Core.strings.GetFileNameOnly(filename);
                }

                //SetFormPropertiesForSave(false); called from prepareforsave this is second call for no reason

                return;
            }

            // file name is null, or empty, or user is forced to choose a filename [Save As], show the dialog
            #region Determine Save Settings: Filename
            if ((!(ValidName)) || ForcePromptForFilename)
            {
                FileDialogSpecification dialogSpecification =
                FilePathManager.Instance.GetSpecification(myView.Doc.FileType);
                SaveFileDialog sfdialog = new SaveFileDialog();
                if (filename != string.Empty)
                {
                    sfdialog.FileName = strings.GetFileNameOnly(filename);
                }
                sfdialog.SupportMultiDottedExtensions = true;
                sfdialog.Filter = dialogSpecification.Filter;
                sfdialog.InitialDirectory = Core.Variables.Instance.DiagramPath;
                //sfdialog.RestoreDirectory = true;

                switch (myView.Doc.FileType)
                {
                    case FileTypeList.Diagram:
                        sfdialog.InitialDirectory = Variables.Instance.DiagramPath;
                        sfdialog.Title = dialogSpecification.Title;
                        DialogResult res = sfdialog.ShowDialog(DockingForm.DockForm);
                        switch (res)
                        {
                            case DialogResult.OK:
                                if (DockingForm.DockForm.OpenedFiles.ContainsKey(sfdialog.FileName.ToLower()))
                                {
                                    if (DockingForm.DockForm.OpenedFiles[sfdialog.FileName.ToLower()] != containerID)
                                    {
                                        MessageBox.Show(DockingForm.DockForm, "Cannot save: " + strings.GetFileNameOnly(sfdialog.FileName) + " is opened in an instance of MetaBuilder", "Already Open", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        SetFormPropertiesForSave(true);
                                        return;
                                    }
                                }
                                document.Name = sfdialog.FileName;
                                // it used to have a valid name, but a new one has been specified

                                if (Core.strings.GetFileNameOnly(sfdialog.FileName.ToLower()) != Core.strings.GetFileNameOnly(filename.ToLower()) && ValidName)
                                {
                                    DockingForm.DockForm.OpenedFiles.Remove(filename.ToLower());
                                    ViewController.CreateTemplateIfRequired(document as NormalDiagram);
                                }

                                break;
                            case DialogResult.Cancel:
                                //myView.ResumeLayout();
                                //document.UpdateViews();
                                SetFormPropertiesForSave(true);
                                return;
                            //break;
                        }
                        break;
                    case FileTypeList.Symbol:
                        sfdialog.InitialDirectory = Variables.Instance.SymbolPath;
                        sfdialog.Title = dialogSpecification.Title;
                        DialogResult sres = sfdialog.ShowDialog(DockingForm.DockForm);
                        switch (sres)
                        {
                            case DialogResult.OK:
                                document.Name = sfdialog.FileName;
                                break;
                        }
                        break;
                }
            }
            else
            {
                Log.WriteLog("DetermineFilename::" + document.Name + " is valid" + Environment.NewLine + Environment.StackTrace);
            }
            #endregion

        }

        public void ShowFindText()
        {
            DockingForm.DockForm.FindAndReplaceText.SwitchPanel(FindAndReplaceText.Function.Find);
            DockingForm.DockForm.FindAndReplaceText.Show(DockingForm.DockForm.dockPanel1, DockState.DockRight);
            DockingForm.DockForm.FindAndReplaceText.BringToFront();
            //return;

            //FindText fText = new FindText();
            //fText.TopMost = true;
            //fText.Show();
        }
        public void ShowFindReplace()
        {
            DockingForm.DockForm.FindAndReplaceText.SwitchPanel(FindAndReplaceText.Function.Replace);
            DockingForm.DockForm.FindAndReplaceText.Show(DockingForm.DockForm.dockPanel1, DockState.DockRight);
            DockingForm.DockForm.FindAndReplaceText.BringToFront();
        }

        private void myView_SelectionDeleting(object sender, CancelEventArgs e)
        {
            NormalDiagram ndiagram = myView.Doc as NormalDiagram;
            GraphFileKey gfKey = null;
            if (ndiagram != null)
            {
                gfKey = new GraphFileKey(ndiagram.VersionManager.CurrentVersion.PKID, ndiagram.VersionManager.CurrentVersion.MachineName);

                if (ndiagram.VersionManager.CurrentVersion.PKID == 0)
                    gfKey.pkid = ndiagram.VersionManager.CurrentVersion.PreviousDocumentID;
            }
            View.Document.SkipsUndoManager = true;
            View.StartTransaction();
            List<GoObject> extraObjectsToRemove = new List<GoObject>();
            ViewController.ModifyObjects(View.Selection, NoPrompt, gfKey);
            if (NoPrompt)
            {
                //Compare selection to db to get red links :)
                Tools.MetaComparer mcomparer = new MetaBuilder.UIControls.GraphingUI.Tools.MetaComparer();
                mcomparer.MyView = this.myView;
                mcomparer.CompareSelection(View.Selection);
            }

            View.Document.SkipsUndoManager = false;
            View.FinishTransaction("Add Mark For Delete Indicator");
            //myView.Document.IsModified = false;
            //myView.Invalidate();
            //ViewController.SetDocumentModifiedToFalseIfNotTrue(false);
            Notify();

            //9 October 2013
            //mark an item for delete but do not remove it from diagram if marking for delete on server diagram
            if (ndiagram.VersionManager != null)
            {
                if (ndiagram.VersionManager.CurrentVersion != null && NoPrompt)
                {
                    if (ndiagram.VersionManager.CurrentVersion.WorkspaceTypeId == 3)
                    {
                        e.Cancel = true;
                    }
                }
            }

            //when we delete an object whose parent is a subgraphnode we must removetherelationship
            GoSelection sel = (sender as GraphView).Selection;
            if (sel != null)
            {
                foreach (GoObject obj in sel)
                {
                    if (obj is IMetaNode)
                    {
                        //when we mfd objects on server objects dont remove their 'child' objects (instead MFD them as well?)
                        //IMetaNode imnode = obj as IMetaNode;
                        //if (imnode.MetaObject.WorkspaceTypeId == 3 && NoPrompt)
                        //{
                        //    e.Cancel = true;
                        //    continue;
                        //}
                        //Remove this objects task if there was one
                        try
                        {
                            DockingForm.DockForm.GetTaskDocker().RemoveTaskWhereTagEquals(this.ContainerID.ToString(), (obj as IMetaNode).MetaObject, typeof(DuplicationTask));
                            if (duplicateCheckers != null)
                                duplicateCheckers.Remove(obj as IMetaNode);
                        }
                        catch
                        {
                        }
                    }

                    //remove rationales from this object if anchored to it
                    foreach (GoObject myCom in myView.Document)
                    {
                        GoObject com = myCom;
                        if (com is ResizableBalloonComment && (com as ResizableBalloonComment).Anchor == obj && !sel.Contains(com))
                            myView.Document.Remove(com);
                    }

                    if (obj is IMetaNode)
                    {
                        if (obj.Parent is SubgraphNode)
                            (obj.Parent as SubgraphNode).RemoveOnlyRelationship(obj as IMetaNode);
                    }

                    if (obj is GoNode)
                    {
                        //18 October 2013 - remove objects attached to associations
                        GoNode myNode = obj as GoNode;

                        #region removeArtefactsAndRationalesFromNode'sLinks
                        //On Link
                        //these links are about to be removed so remove all attached objects (rationales and artefacts)
                        foreach (GoLabeledLink l in myNode.DestinationLinks)
                        {
                            GoLabeledLink link = l;
                            if (l is QLink)
                            {

                                ViewController.reAnchorableComments = new Dictionary<ResizableBalloonComment, GoObject>();
                                foreach (KeyValuePair<ArtefactNodeLinkKey, QLink> k in ViewController.FindArtefactsOnLink(l as QLink, l as QLink))
                                {
                                    try
                                    {
                                        if (!sel.Contains(k.Key.Node))
                                            myView.Document.Remove(k.Key.Node);
                                    }
                                    catch
                                    {
                                    }
                                }
                                foreach (KeyValuePair<ResizableBalloonComment, GoObject> k in ViewController.reAnchorableComments)
                                {
                                    try
                                    {
                                        if (!sel.Contains(k.Key))
                                            myView.Document.Remove(k.Key);
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                        foreach (GoLabeledLink l in myNode.Links)
                        {
                            GoLabeledLink link = l;
                            if (l is QLink)
                            {
                                ViewController.reAnchorableComments = new Dictionary<ResizableBalloonComment, GoObject>();
                                foreach (KeyValuePair<ArtefactNodeLinkKey, QLink> k in ViewController.FindArtefactsOnLink(l as QLink, l as QLink))
                                {
                                    try
                                    {
                                        if (!sel.Contains(k.Key.Node))
                                            myView.Document.Remove(k.Key.Node);
                                    }
                                    catch
                                    {
                                    }
                                }
                                foreach (KeyValuePair<ResizableBalloonComment, GoObject> k in ViewController.reAnchorableComments)
                                {
                                    try
                                    {
                                        if (!sel.Contains(k.Key))
                                            myView.Document.Remove(k.Key);
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                        #endregion
                    }

                    if (obj is GoLabeledLink)
                    {
                        GoLabeledLink l = obj as GoLabeledLink;
                        if (l is QLink)
                        {
                            ViewController.reAnchorableComments = new Dictionary<ResizableBalloonComment, GoObject>();
                            foreach (KeyValuePair<ArtefactNodeLinkKey, QLink> k in ViewController.FindArtefactsOnLink(l as QLink, l as QLink))
                            {
                                try
                                {
                                    if (!sel.Contains(k.Key.Node))
                                        myView.Document.Remove(k.Key.Node);
                                }
                                catch
                                {
                                }
                            }
                            foreach (KeyValuePair<ResizableBalloonComment, GoObject> k in ViewController.reAnchorableComments)
                            {
                                try
                                {
                                    if (!sel.Contains(k.Key))
                                        myView.Document.Remove(k.Key);
                                }
                                catch
                                {
                                }
                            }
                        }
                    }

                    if (obj is GoText)
                        if (obj.ParentNode == obj)
                        {
                            if (obj.Deletable == false)
                                obj.Deletable = true;
                        }
                        else if (obj.ParentNode is ArtefactNode)
                        {
                            extraObjectsToRemove.Add(obj.ParentNode);
                        }
                }
                DockingForm.DockForm.GetTaskDocker().BindToList(this.ContainerID.ToString());
            }

            foreach (GoObject o in extraObjectsToRemove)
            {
                o.Remove();
            }
            //return;
        }

        #region Object Deleting


        //BusySelecting = false; // Start
        //List<RemoveOrMarkForDelete.RepositoryItemAction> itemsToPrompt =
        //    new List<RemoveOrMarkForDelete.RepositoryItemAction>();
        //List<GoObject> itemsToRemove = new List<GoObject>();

        //GoCollection col = myView.Selection;
        //// Add items to the list
        //GetItemsToPrompt(col, e, itemsToPrompt, itemsToRemove);

        //// Show form for the items
        //if (itemsToPrompt.Count > 0)
        //{
        //    RemoveOrMarkForDelete removeDialog = new RemoveOrMarkForDelete();
        //    removeDialog.ItemActions = itemsToPrompt;

        //    NormalDiagram ndiagram = myView.Doc as NormalDiagram;
        //    if (ndiagram != null)
        //    {
        //        GraphFileKey gfKey =
        //            new GraphFileKey(ndiagram.VersionManager.CurrentVersion.PKID,
        //                             ndiagram.VersionManager.CurrentVersion.MachineName);

        //        if (ndiagram.VersionManager.CurrentVersion.PKID == 0)
        //            gfKey.pkid = ndiagram.VersionManager.CurrentVersion.PreviousDocumentID;
        //        removeDialog.GraphFileKey = gfKey;
        //        DialogResult res = DialogResult.OK;
        //        if (NoPrompt)
        //        {
        //            res = System.Windows.Forms.DialogResult.OK;
        //            foreach (RemoveOrMarkForDelete.RepositoryItemAction iaction in removeDialog.ItemActions)
        //            {
        //                iaction.ActionToBeTaken = RemoveOrMarkForDelete.ActionToBeTaken.Delete;
        //            }
        //        }
        //        else
        //        {
        //            res = removeDialog.ShowDialog(DockingForm.DockForm);
        //        }
        //        // Dialog shown, now do the actions
        //        if (res == DialogResult.OK)
        //        {
        //            List<RemoveOrMarkForDelete.RepositoryItemAction> actions = removeDialog.ItemActions;
        //            RemoveOrDeleteObjects(e, actions, itemsToRemove);
        //        }
        //        else
        //        {
        //            e.Cancel = true;
        //        }

        //    }
        //}
        //// Are we deleting again???

        //if (itemsToRemove.Count > 0)
        //{
        //    for (int i = 0; i < itemsToRemove.Count; i++)
        //    {

        //        if (itemsToPrompt.Count > i)
        //            if (!(itemsToPrompt[i].MyGoObject.TopLevelObject is FrameLayerGroup))
        //                itemsToRemove[i].Remove();

        //    }
        //}
        //myView.Document.FinishTransaction("Remove items from Diagram");
        ////myView.Document.UndoManager.Clear();

        ///*

        //            myView.Document.SuspendsRouting = false;
        //            myView.Document.UpdateViews();
        //            NoPrompt = false;
        //            myView.Invalidate();*/
        //Notify();

        //private void GetItemsToPrompt(IGoCollection col, CancelEventArgs e, List<RemoveOrMarkForDelete.RepositoryItemAction> itemsToPrompt, List<GoObject> itemsToRemove)
        //{
        //    #region Nothing special here, iterates nodes and identifies which objects have been saved - requiring a prompt. Can be optimized.
        //    foreach (GoObject o in col)
        //    {
        //        bool allocated = false;
        //        #region Normal Nodes
        //        if (o is IMetaNode)
        //        {
        //            IMetaNode metaNode = o as IMetaNode;
        //            if (!(o is SubgraphNode))
        //            {
        //                allocated = AddToItemsToPromptIfNecessary(o, metaNode, itemsToPrompt, allocated);
        //            }
        //            List<Rationale> rationales = new List<Rationale>();

        //            GoNode node = o as GoNode;
        //            if (node != null)
        //            {
        //                foreach (object observer in node.Observers)
        //                {
        //                    if (observer is Rationale)
        //                    {
        //                        Rationale rat = observer as Rationale;
        //                        if (rat.MetaObject.pkid > 0)
        //                        {
        //                            RemoveOrMarkForDelete.RepositoryItemAction actionAssocA =
        //                               new RemoveOrMarkForDelete.RepositoryItemAction();
        //                            actionAssocA.Item = rat.MetaObject;
        //                            actionAssocA.MyGoObject = rat;
        //                            itemsToPrompt.Add(actionAssocA);
        //                            allocated = true;
        //                        }
        //                        else
        //                        {
        //                            itemsToRemove.Add(rat);
        //                        }
        //                    }
        //                    //                        Console.WriteLine(observer.ToString());
        //                }
        //            }
        //        }
        //        #endregion
        //        else
        //        {
        //            #region Nodes #2
        //            // will have to check why this is here (again). We shouldve already handled the nodes.
        //            if (o.ParentNode is IMetaNode)
        //            {
        //                IMetaNode metaNode = o.ParentNode as IMetaNode;
        //                allocated = AddToItemsToPromptIfNecessary(o.ParentNode, metaNode, itemsToPrompt, allocated);
        //            }
        //            #endregion
        //        }
        //        #region Handle Cancel Link eventArgs - ie: when a user selects a link, drags it onto nothing new
        //        if (e is ReLinkTool.LinkCancelEventArgs)
        //        {
        //            ReLinkTool.LinkCancelEventArgs args = e as ReLinkTool.LinkCancelEventArgs;
        //            if (args.OldLink is QLink)
        //            {
        //                QLink linkA = args.OldLink as QLink;
        //                if (args.Association != null)
        //                {
        //                    if (!VCStatusTool.IsObsoleteOrMarkedForDelete(args.Association))
        //                    {
        //                        RemoveOrMarkForDelete.RepositoryItemAction actionAssocA =
        //                            new RemoveOrMarkForDelete.RepositoryItemAction();
        //                        actionAssocA.Item = args.Association;
        //                        actionAssocA.MyGoObject = args.OldLink;
        //                        itemsToPrompt.Add(actionAssocA);
        //                        allocated = true;
        //                    }
        //                }
        //            }
        //        }
        //        #endregion
        //        #region QLinks - Why is this handled twice?
        //        if (o is QLink)
        //        {
        //            QLink link = o as QLink;
        //            ObjectAssociation assoc = link.GetAssociation(true);
        //            if (assoc != null)
        //            {
        //                if (!VCStatusTool.IsObsoleteOrMarkedForDelete(assoc))
        //                {
        //                    RemoveOrMarkForDelete.RepositoryItemAction actionAssoc =
        //                        new RemoveOrMarkForDelete.RepositoryItemAction();
        //                    actionAssoc.Item = assoc;
        //                    actionAssoc.MyGoObject = o;
        //                    itemsToPrompt.Add(actionAssoc);
        //                    allocated = true;
        //                }
        //            }

        //            List<ArtefactNode> artnodes = link.GetArtefacts();
        //            foreach (ArtefactNode artNode in artnodes)
        //            {
        //                RemoveOrMarkForDelete.RepositoryItemAction actionArt = new RemoveOrMarkForDelete.RepositoryItemAction();
        //                actionArt.Item = artNode.MetaObject;
        //                actionArt.MyGoObject = artNode;
        //                if (artNode.MetaObject.pkid > 0)
        //                    itemsToPrompt.Add(actionArt);
        //            }

        //            List<Rationale> rationales = link.GetRationales();
        //            foreach (Rationale artNode in rationales)
        //            {
        //                RemoveOrMarkForDelete.RepositoryItemAction actionArt = new RemoveOrMarkForDelete.RepositoryItemAction();
        //                actionArt.Item = artNode.MetaObject;
        //                actionArt.MyGoObject = artNode;
        //                if (artNode.MetaObject.pkid > 0)
        //                    itemsToPrompt.Add(actionArt);
        //            }

        //        }
        //        #endregion

        //        #region Handle unallocated items - will need to debug this.
        //        if (!allocated)
        //        {
        //            if (o is IGoCollection)
        //            {
        //                GetItemsToPrompt(o as IGoCollection, e, itemsToPrompt, itemsToRemove);

        //            }
        //            if ((!(o.ParentNode is IMetaNode)) && (!(o.DraggingObject is QLink)) && ((o is IMetaNode) || (o is QLink)))
        //            {
        //                itemsToRemove.Add(o);
        //                allocated = true;
        //            }

        //            #region QLinks
        //            if (o is QLink)
        //            {
        //                QLink slink = o as QLink;
        //                List<ArtefactNode> artnodes = slink.GetArtefacts();
        //                foreach (ArtefactNode artNode in artnodes)
        //                {
        //                    RemoveOrMarkForDelete.RepositoryItemAction actionArt = new RemoveOrMarkForDelete.RepositoryItemAction();
        //                    actionArt.Item = artNode.MetaObject;
        //                    actionArt.MyGoObject = artNode;
        //                    if (artNode.MetaObject.pkid == 0)
        //                        itemsToRemove.Add(artNode);
        //                }
        //                RemoveOrMarkForDelete.RepositoryItemAction actionAssoc = new RemoveOrMarkForDelete.RepositoryItemAction();
        //                actionAssoc.Item = slink.GetAssociation(true);
        //                actionAssoc.MyGoObject = slink;
        //                itemsToRemove.Add(slink);
        //            }
        //            #endregion
        //            // Still not allocated? Add it.
        //            if (!allocated)
        //            {
        //                itemsToRemove.Add(o);
        //            }

        //        }
        //        #endregion
        //    }
        //    #endregion
        //}
        //private bool AddToItemsToPromptIfNecessary(GoObject o, IMetaNode metaNode, List<RemoveOrMarkForDelete.RepositoryItemAction> itemsToPrompt, bool allocated)
        //{
        //    #region This shouldnt be hit at all?
        //    MetaBase mbase = metaNode.MetaObject;

        //    foreach (RemoveOrMarkForDelete.RepositoryItemAction repAction in itemsToPrompt)
        //    {
        //        if (repAction.MyGoObject == metaNode)
        //        {
        //            return true;
        //        }
        //    }
        //    if (mbase != null)
        //    {
        //        //mbase.LoadFromID(mbase.pkid, mbase.MachineName,true);
        //        b.MetaObject mo = d.DataRepository.MetaObjectProvider.GetBypkidMachine(mbase.pkid, mbase.MachineName);
        //        if (mo != null)
        //        {
        //            mbase.State = (VCStatusList)mo.VCStatusID;
        //            if (mbase.pkid > 0 && (!VCStatusTool.IsObsoleteOrMarkedForDelete(mbase)))
        //            {
        //                bool shouldPrompt = true;
        //                foreach (GoObject oChild in o as IGoCollection)
        //                {
        //                    if (oChild is IndicatorLabel)
        //                    {
        //                        ChangedIndicatorLabel txtLabel = oChild as ChangedIndicatorLabel;
        //                        if (txtLabel != null)
        //                            if (txtLabel.Text.ToLower().Contains("not in database"))
        //                            {
        //                                shouldPrompt = false;
        //                            }
        //                    }
        //                }
        //                if (shouldPrompt)
        //                {
        //                    RemoveOrMarkForDelete.RepositoryItemAction action =
        //                        new RemoveOrMarkForDelete.RepositoryItemAction();
        //                    action.Item = mbase;
        //                    action.MyGoObject = o;
        //                    itemsToPrompt.Add(action);
        //                    allocated = true;
        //                }
        //            }
        //        }

        //        DockingForm.DockForm.GetTaskDocker().RemoveTaskWhereTagEquals(myView, mbase,
        //                                                                      typeof(DuplicationTask));
        //    }
        //    return allocated;
        //    #endregion
        //}
        //private void RemoveOrDeleteObjects(CancelEventArgs e, List<RemoveOrMarkForDelete.RepositoryItemAction> actions, List<GoObject> itemsToRemove)
        //{
        //    foreach (RemoveOrMarkForDelete.RepositoryItemAction act in actions)
        //    {
        //        if (act.ActionToBeTaken == RemoveOrMarkForDelete.ActionToBeTaken.Remove)
        //        {
        //            #region StraightForward Remove - no DB Interaction required
        //            if (!act.MyGoObject.Copyable)
        //            {
        //                RemoveShadowsFromOtherShallowCopies(act.MyGoObject);
        //            }
        //            if (act.MyGoObject is CollapsingRecordNodeItem)
        //            {
        //                CollapsingRecordNodeItem cnodeItem = act.MyGoObject as CollapsingRecordNodeItem;
        //                cnodeItem.RemoveLink();
        //            }
        //            if (act.MyGoObject is SubgraphNode)
        //            {
        //                RemoveEmbeddedObjectsAndLinksFromSubgraph(act.MyGoObject, false);
        //            }
        //            if (act.MyGoObject is GoNode)
        //            {
        //                GoNode n = act.MyGoObject as GoNode;

        //                GoNodeLinkEnumerator linkenum = n.DestinationLinks.GetEnumerator();
        //                while (linkenum.MoveNext())
        //                {
        //                    Console.WriteLine(linkenum.Current.ToString());
        //                }
        //            }
        //            act.MyGoObject.Remove();
        //            #endregion
        //        }

        //        #region Mark for delete

        //        if (act.ActionToBeTaken == RemoveOrMarkForDelete.ActionToBeTaken.Delete)
        //        {
        //            if (act.MyGoObject is SubgraphNode)
        //            {
        //                RemoveEmbeddedObjectsAndLinksFromSubgraph(act.MyGoObject, true);
        //            }
        //            // if its a MetaNode, the DB needs to be updated
        //            if (act.MyGoObject is IMetaNode)
        //            {
        //                #region Update VC Status for IMNs
        //                IMetaNode node = act.MyGoObject as IMetaNode;
        //                if (node.MetaObject.pkid > 0 && node.MetaObject.MachineName != null)
        //                {
        //                    node.MetaObject = Loader.GetByID(node.MetaObject._ClassName, node.MetaObject.pkid, node.MetaObject.MachineName);
        //                }
        //                if (VCStatusTool.DeletableFromDiagram(node.MetaObject) &&
        //                    VCStatusTool.UserHasControl(node.MetaObject))
        //                {
        //                    #region Update DB
        //                    ObjectHelper ohelper = new ObjectHelper();
        //                    node.MetaObject.State = VCStatusList.MarkedForDelete;
        //                    MetaObject mo =
        //                        DataRepository.MetaObjectProvider.GetBypkidMachine(node.MetaObject.pkid,
        //                                                                           node.MetaObject.
        //                                                                               MachineName);
        //                    if (mo != null)
        //                    {
        //                        mo.VCStatusID = (int)VCStatusList.MarkedForDelete;
        //                        DataRepository.MetaObjectProvider.Save(mo);
        //                    }
        //                    #endregion
        //                }
        //                #endregion
        //            }
        //            #region LINKS. Might be buggy.
        //            if (act.MyGoObject is QLink)
        //            {
        //                QLink link;
        //                ObjectAssociation assoc;
        //                if (e is ReLinkTool.LinkCancelEventArgs)
        //                {
        //                    ReLinkTool.LinkCancelEventArgs linkRelinkArgs =
        //                        e as ReLinkTool.LinkCancelEventArgs;
        //                    assoc = linkRelinkArgs.Association;
        //                }
        //                else
        //                {
        //                    link = act.MyGoObject as QLink;
        //                    assoc = link.GetAssociation(true);
        //                }

        //                if (assoc != null)
        //                {

        //                    if (VCStatusTool.DeletableFromDiagram(assoc) &&
        //                        VCStatusTool.UserHasControl(assoc))
        //                    {
        //                        assoc.VCStatusID = (int)VCStatusList.MarkedForDelete;
        //                        assoc.State = VCStatusList.MarkedForDelete;
        //                        DataRepository.ObjectAssociationProvider.Save(assoc);
        //                    }
        //                }
        //            }
        //            #endregion

        //            #region Shadows
        //            if (act.MyGoObject.Shadowed)
        //            {
        //                RemoveShadowsFromOtherShallowCopies(act.MyGoObject);
        //            }
        //            #endregion
        //            act.MyGoObject.Remove();
        //        }

        //        #endregion

        //        if (act.ActionToBeTaken == RemoveOrMarkForDelete.ActionToBeTaken.Cancel)
        //        {
        //            itemsToRemove.Remove(act.MyGoObject);
        //        }
        //    }

        //    for (int i = 0; i < itemsToRemove.Count; i++)
        //    {
        //        itemsToRemove[i].Remove();
        //    }
        //    /*
        //    try
        //    {
        //        myView.Document.RaiseChanged(1001001, 1001001, this, 0, null, new RectangleF(), 0, null,
        //                                     new RectangleF());
        //    }catch
        //    {
        //    }*/

        //}
        //private void RemoveEmbeddedObjectsAndLinksFromSubgraph(GoObject objOwner, bool markAsDelete)
        //{
        //    // Probably unrelated to the current bug
        //    SubgraphNode sgNode = objOwner as SubgraphNode;
        //    Dictionary<GoObject, int> objectsToRemove = new Dictionary<GoObject, int>();
        //    foreach (GoObject obj in sgNode)
        //    {
        //        if (obj is ILinkedContainer)
        //        {
        //            if (markAsDelete)
        //                objectsToRemove.Add(obj, 1);
        //            else
        //                objectsToRemove.Add(obj, 2);

        //        }
        //    }
        //    GraphViewController gvc = new GraphViewController();
        //    gvc.MyView = this.myView;
        //    gvc.RemoveItemsFromILinkedContainer(objectsToRemove, sgNode);

        //}
        //private void RemoveShadowsFromOtherShallowCopies(GoObject o)
        //{
        //    if (o is GraphNode)
        //    {
        //        GraphNode node = o as GraphNode;
        //        List<GraphNode> shallowCopies = new List<GraphNode>();
        //        foreach (GoObject obj in myView.Document)
        //        {
        //            if (obj is GraphNode && obj != o)
        //            {
        //                GraphNode shallowInstance = obj as GraphNode;
        //                if (shallowInstance.MetaObject.pkid == node.MetaObject.pkid &&
        //                    shallowInstance.MetaObject.MachineName == node.MetaObject.MachineName)
        //                {
        //                    shallowCopies.Add(shallowInstance);
        //                }
        //            }
        //        }
        //        foreach (GraphNode foundShallow in shallowCopies)
        //        {
        //            bool foundOtherInstance = false;
        //            foreach (GoObject obj in myView.Document)
        //            {
        //                if (obj is GraphNode && obj != foundShallow && obj != o)
        //                {
        //                    GraphNode shallowInstance = obj as GraphNode;
        //                    if (shallowInstance.MetaObject.pkid == foundShallow.MetaObject.pkid &&
        //                        shallowInstance.MetaObject.MachineName == foundShallow.MetaObject.MachineName)
        //                    {
        //                        foundOtherInstance = true;
        //                    }
        //                }
        //            }
        //            if (!foundOtherInstance)
        //            {
        //                foundShallow.Copyable = true;
        //                ViewController.RemoveShadows(foundShallow);
        //            }
        //        }
        //    }
        //}

        #endregion

        #region Save/Load

        private DocumentVersion documentVersion;
        public DocumentVersion DocumentVersion
        {
            get { return documentVersion; }
            set { documentVersion = value; }
        }

        public NormalDiagram LoadGraphFile(GraphFile file, bool fromServer)
        {
            if (file == null)
            {
                Log.WriteLog("Graphfile is null, loading of file aborted");
                return null;
            }
            //if (OpeningFromServer)
            //    PleaseWait.ShowPleaseWaitForm();
            Application.DoEvents();
            GraphFileManager gfmanager = new GraphFileManager();
            NormalDiagram diagram = gfmanager.RetrieveGraphDoc(file);
            gfmanager = null;

            if (diagram != null)
            {
                SetupView(FileTypeList.Diagram);
                myView.Document = diagram;
                myView.Document.Name = file.Name;
                // Reset the VersionManager to reflect the file's actual data
                diagram.VersionManager.CurrentVersion.LoadFromFile(file);
                //DocumentController dc = new DocumentController(diagram);
                //dc.ApplyBrushes();
                if (diagram.VersionManager.CurrentVersion.SheetBounds.X > 0)
                {
                    myView.Sheet.Bounds = diagram.VersionManager.CurrentVersion.SheetBounds;
                }

                //diagram.UndoManager = new GoUndoManager();
                Text = diagram.DocumentFrame.txtFileName.Text = strings.GetFileNameOnly(file.Name) + " V" + diagram.VersionManager.CurrentVersion.MajorVersion.ToString() + "." + diagram.VersionManager.CurrentVersion.MinorVersion.ToString();
                OpeningFromServer = fromServer;

                if (fromServer)
                    DoDiagramVersionControl(diagram);
            }
            //UpdateDocumentIndicators(false);

            //if (OpeningFromServer)
            //    PleaseWait.CloseForm();
            return diagram;
        }

        //public NormalDiagram LoadGraphFile(GraphFile file)
        //{
        //    if (file == null)
        //    {
        //        Log.WriteLog("Graphfile is null, loading of file aborted");
        //        return null;
        //    }
        //    //if (OpeningFromServer)
        //    //    PleaseWait.ShowPleaseWaitForm();
        //    Application.DoEvents();
        //    GraphFileManager gfmanager = new GraphFileManager();
        //    NormalDiagram diagram = gfmanager.RetrieveGraphDoc(file);
        //    gfmanager = null;

        //    if (diagram != null)
        //    {
        //        SetupView(FileTypeList.Diagram);
        //        myView.Document = diagram;
        //        myView.Document.Name = file.Name;
        //        // Reset the VersionManager to reflect the file's actual data
        //        diagram.VersionManager.CurrentVersion.LoadFromFile(file);
        //        //DocumentController dc = new DocumentController(diagram);
        //        //dc.ApplyBrushes();
        //        if (diagram.VersionManager.CurrentVersion.SheetBounds.X > 0)
        //        {
        //            myView.Sheet.Bounds = diagram.VersionManager.CurrentVersion.SheetBounds;
        //        }

        //        //diagram.UndoManager = new GoUndoManager();

        //        Text = diagram.DocumentFrame.txtFileName.Text = strings.GetFileNameOnly(file.Name) + " V" + diagram.VersionManager.CurrentVersion.MajorVersion.ToString() + "." + diagram.VersionManager.CurrentVersion.MinorVersion.ToString();
        //        OpeningFromServer = fromServer;
        //    }
        //    //UpdateDocumentIndicators(false);

        //    //if (OpeningFromServer)
        //    //    PleaseWait.CloseForm();

        //    View.Document.FinishTransaction("Open Document");

        //    View.Document.FinishTransaction("Making sure");

        //    View.Document.FinishTransaction("Really sure");

        //    myView.Document.UndoManager.Clear();

        //    return diagram;
        //}

        private BackgroundWorker loadfileWorker = new BackgroundWorker();
        private FileTypeList ftype = FileTypeList.Diagram;
        private string Filename = "";
        public void LoadFile(string fileName)
        {
            Filename = fileName;
            //SuspendLayout();
            if (!Core.Variables.Instance.IsViewer)
            {
                Loader.FlushDataViews();
            }
            else
            {
                ReadOnly = true;
            }
            //PleaseWait.ShowPleaseWaitForm();
            //PleaseWait.SetStatus("Loading file and objects");
            // Default to a drawing
            //FileTypeList ftype = FileTypeList.Diagram;
            string ext = strings.GetFileExtension(Filename);
            FilePathManager.Instance.SetLastOpenPath(strings.GetPath(Filename));
            Collection<FileDialogSpecification> specificiations = FilePathManager.Instance.GetList();
            foreach (FileDialogSpecification specificationInstance in specificiations)
            {
                if (ext == specificationInstance.Extension)
                {
                    // Gotcha! Now we know what filetype it is
                    ftype = specificationInstance.FileType;
                }
            }

            DockingForm.DockForm.UpdateStatusLabel("Opening Diagram");
            loadfileWorker.WorkerReportsProgress = true;
            loadfileWorker.DoWork += new DoWorkEventHandler(loadfileWorker_DoWork);
            loadfileWorker.ProgressChanged += new ProgressChangedEventHandler(loadfileWorker_ProgressChanged);
            loadfileWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(loadfileWorker_RunWorkerCompleted);
            if (Variables.Instance.UserDebug)
                Log.WriteLog("USER DEBUG : " + Environment.NewLine + "Starting loadfileWorker");
            loadfileWorker.RunWorkerAsync();
            //FileUtil futil = new FileUtil();
            //object loadedObject = futil.Open(Filename);

            //FileDialogSpecification spec = FilePathManager.Instance.GetSpecification(ftype);
            //switch (ftype)
            //{
            //    case FileTypeList.ArrowHead:
            //        myView.Doc.Add(StorageManipulator.FileSystemManipulator.LoadObject(Filename, spec));
            //        myView.Doc.FileType = ftype;
            //        break;
            //    case FileTypeList.Symbol:
            //        Symbol sym = loadedObject as Symbol;
            //        GraphNode node = sym.GetShapeContainer();
            //        sym.ShapeBindingInfo = node.BindingInfo;
            //        myView.Document = sym;
            //        myView.Document.SerializesUndoManager = false;
            //        DocumentController shapeDocController = new DocumentController(myView.Document);
            //        shapeDocController.ApplyBrushes();
            //        break;
            //    default:
            //        try
            //        {
            //            NormalDiagram ndiagram = loadedObject as NormalDiagram;
            //            ndiagram = DoDiagramVersionControl(ndiagram);
            //            myView.Document = ndiagram;
            //            DocumentController docController = new DocumentController(ndiagram);
            //            docController.ApplyBrushes();
            //            ndiagram.SerializesUndoManager = false;
            //            ndiagram.RepositionFrame(myView);

            //            ViewController.HookupObjectEvents();
            //            //this is double called probably
            //            ViewController.SetDocumentModifiedToFalseIfNotTrue(true);
            //            UpdateMenuItems();
            //        }
            //        catch (Exception x)
            //        {
            //            LogEntry logEntry = new LogEntry();
            //            logEntry.Message = x.ToString();
            //            Logger.Write(logEntry);
            //        }
            //        break;
            //}
            //Text = myView.Document.Name;
            //FilePathManager.Instance.SetLastUsedPath(spec.FileType, Filename);
            //myView.Document.Name = Filename;
            //FixValueChains(myView.Document);

            //ResumeLayout();
        }

        private void loadfileWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            DockingForm.DockForm.UpdateTotal(100);
            switch (e.UserState.ToString().Replace(".", ""))
            {
                case "Opening":
                    DockingForm.DockForm.ProgressUpdate(25);
                    break;
                case "Extracting":
                    DockingForm.DockForm.ProgressUpdate(50);
                    break;
                case "Depersisting":
                    DockingForm.DockForm.ProgressUpdate(75);
                    break;
                case "Attempt Binary":
                    DockingForm.DockForm.ProgressUpdate(40);
                    break;
                case "Attempt Load":
                    DockingForm.DockForm.ProgressUpdate(60);
                    break;
                case "Unzip Error - Attempt Load":
                    DockingForm.DockForm.ProgressUpdate(80);
                    break;
                case "Delete Temp File":
                    DockingForm.DockForm.ProgressUpdate(90);
                    break;
                case "Finalizing":
                    DockingForm.DockForm.ProgressUpdate(95);
                    break;
            }
            DockingForm.DockForm.UpdateStatusLabel(e.UserState.ToString());
        }
        private void loadfileWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //if (loadedObject == null)
            //{
            //Close();
            //}

            Show(DockingForm.DockForm.DiagramDockPanel);
            Application.DoEvents();

            FileDialogSpecification spec = FilePathManager.Instance.GetSpecification(ftype);
            switch (ftype)
            {
                case FileTypeList.ArrowHead:
                    myView.Doc.Add(StorageManipulator.FileSystemManipulator.LoadObject(Filename, spec));
                    myView.Doc.FileType = ftype;
                    break;
                case FileTypeList.Symbol:
                    Symbol sym = loadedObject as Symbol;
                    GraphNode node = sym.GetShapeContainer();
                    sym.ShapeBindingInfo = node.BindingInfo;
                    myView.Document = sym;
                    //myView.Document.SerializesUndoManager = false;
                    DocumentController shapeDocController = new DocumentController(myView.Document);
                    shapeDocController.ApplyBrushes();
                    break;
                default:
                    try
                    {
                        NormalDiagram ndiagram = loadedObject as NormalDiagram;
                        //ndiagram = DoDiagramVersionControl(ndiagram);
                        myView.Document = ndiagram;
                        //DocumentController docController = new DocumentController(ndiagram);
                        //docController.ApplyBrushes();
                        ndiagram.SerializesUndoManager = true;
                        ndiagram.RepositionFrame(myView);

                        //this is double called probably
                        //ViewController.SetDocumentModifiedToFalseIfNotTrue(true);
                        //UpdateMenuItems();
                    }
                    catch (Exception x)
                    {
                        if (Core.Variables.Instance.IsViewer)
                        {
                            Core.Log.WriteLog(x.ToString());
                        }
                        else
                        {
                            LogEntry logEntry = new LogEntry();
                            logEntry.Message = x.ToString();
                            Logger.Write(logEntry);
                        }
                    }
                    break;
            }
            //Text = myView.Document.Name;
            FilePathManager.Instance.SetLastUsedPath(spec.FileType, Filename);
            myView.Document.Name = Filename;

            DockingForm.DockForm.ResetStatus();

            FinaliseDocumentAfterLoading(SkipRefresh);

            //7 October 2015
            //AddShadowsForMetaNodes();

            View.Document.FinishTransaction("Open Document");

            View.Document.FinishTransaction("Making sure");

            View.Document.FinishTransaction("Really sure");

            myView.Document.UndoManager.Clear();

            DockingForm.DockForm.opening = false;
        }

        //7 October 2015
        public void AddShadowsForMetaNodes()
        {
            foreach (IMetaNode imnode in ViewController.GetIMetaNodes())
            {
                IMetaNode imn = imnode;
                //ThreadPool.QueueUserWorkItem(new WaitCallback(DockingForm.DockForm.CheckIfExistsOnDiagramsNoTip, imn as object));
                DockingForm.DockForm.CheckIfExistsOnDiagramsNoTip(imn);
                if (imn is CollapsingRecordNodeItem)
                {
                    (imn as CollapsingRecordNodeItem).RefreshAllCollapsibleNodesWithSameID -= GraphViewContainer_RefreshAllCollapsibleNodesWithSameID;
                    (imn as CollapsingRecordNodeItem).RefreshAllCollapsibleNodesWithSameID += new EventHandler(GraphViewContainer_RefreshAllCollapsibleNodesWithSameID);
                }
            }
        }

        private void GraphViewContainer_RefreshAllCollapsibleNodesWithSameID(object sender, EventArgs e)
        {
            if (!(sender is IMetaNode))
                return;
            IMetaNode parent = sender as IMetaNode;
            foreach (DockContent dcont in DockingForm.DockForm.dockPanel1.Documents)
            {
                if (dcont is GraphViewContainer)//&& dcont != this)
                {
                    GraphViewContainer container = dcont as GraphViewContainer;
                    GoCollection col = new GoCollection();
                    foreach (IMetaNode n in container.ViewController.GetIMetaNodesBoundToMetaObject(parent.MetaObject))
                    {
                        if (n is CollapsibleNode && n != parent)//dont refresh the parent because it was the one causing the move
                        {
                            if (parent is CollapsibleNode)
                                (parent as CollapsibleNode).CopyChildrenToNode(n as CollapsibleNode);
                            //col.Add(n as GoObject);
                        }
                    }
                    //if (col.Count > 0)
                    //    container.RefreshDatax(col, false);
                }
            }
        }

        private bool skipRefresh;
        public bool SkipRefresh
        {
            get { return skipRefresh; }
            set { skipRefresh = value; }
        }

        private object loadedObject = null;
        private void loadfileWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            FileUtil futil = new FileUtil();
            futil.Report += new EventHandler(futil_Report);
            loadedObject = futil.Open(Filename); //thread this call
        }

        private void futil_Report(object sender, EventArgs e)
        {
            try
            {
                if (loadfileWorker.IsBusy)
                    loadfileWorker.ReportProgress(0, sender);
            }
            catch
            {
                //mulithreaded opening causes problem here
            }
        }

        private bool ignoreChecking = false;
        public void ExecutePluginsAfterLoad()
        {
            ignoreChecking = true;
            ViewController.ArtefactPointersVisible = (myView.Document as NormalDiagram).ArtefactPointersVisible;
            menuItemViewFishLink.Checked = ViewController.ArtefactPointersVisible;

            menuItemViewObjectImages.Checked = (myView.Document as NormalDiagram).ShowObjectImages;
            //BindToMetaObjectProperties(myView.Document);

            if ((myView.Document as NormalDiagram).WasNumbered)
            {
                myView.Selection.Clear();
                //myView.Selection.Add(obj);
                ExecutePlugin("Hierarchy Numbering");
                //myView.Selection.Clear();
            }

            foreach (GoObject obj in myView.Document)
            {
                //if (obj is GraphNode)
                //{
                //    //if ((obj as GraphNode).SourceLinks.Count == 0)
                //    //{

                //    //}
                //}
                //else
                if (obj is FishLink)
                {
                    obj.Visible = ViewController.ArtefactPointersVisible;
                }
                else if (obj is GoShape)
                {
                    if (((obj as GoShape).ParentNode == null || (obj as GoShape).ParentNode == obj) && (obj as GoShape).Parent == null)
                    {
                        ShapeOrderingControl.SendToBack(obj, myView);
                    }
                }
            }
            ignoreChecking = false;
        }

        private Dictionary<SubgraphNode, bool> subgraphsToCheck;
        private void buildSubgraphsToCheck(SubgraphNode node)
        {
            subgraphsToCheck.Add(node, node.IsExpanded);
            if (!node.IsExpanded)
                node.Expand();
            foreach (GoObject o in node.GetEnumerator())
            {
                if (!(o is SubgraphNode)) continue;
                SubgraphNode n = o as SubgraphNode;
                buildSubgraphsToCheck(n);
            }
        }

        public void StartSaveProcess(bool forceFileNameSelect)
        {
            if (DockingForm.DockForm.ForceCloseCancel)
            {
                Log.WriteLog("GraphViewContainer::StartSaveProcess[" + forceFileNameSelect + "]::ForceCloseCanceled" + Environment.NewLine + Environment.StackTrace);
                return;
            }

            //if (DockingForm.DockForm.IsSaving)
            //{
            //    DockingForm.DockForm.DisplayTip("You cannot save this diagram until the current process has completed", "Save in progress");
            //    return;
            //}
            DockingForm.DockForm.UpdateStatusLabel("Save Initializing");

            //9 January 2013 Prevent quick panel from being saved
            ViewController.removeQuickMenu();
            //Undo legend visibility
            ViewController.makeAllLegendItemsVisible("StartSaveProcess", new EventArgs());

            //23 January 2013 expand all the subgaphs (this does not expand the subgraphs)
            subgraphsToCheck = new Dictionary<SubgraphNode, bool>();
            foreach (GoObject o in myView.Document)
            {
                if (!(o is SubgraphNode)) continue;
                SubgraphNode n = o as SubgraphNode;
                buildSubgraphsToCheck(n);
            }

            DockingForm.DockForm.UpdateTotal(100);
            DockingForm.DockForm.ProgressUpdate(1);

            NormalDiagram ndiagram = myView.Document as NormalDiagram;
            if (ndiagram != null)
            {
                //this.View.BeginUpdate();
                DetermineFilename(ndiagram, forceFileNameSelect);

                if (ndiagram.Name != string.Empty && this.DialogResult != System.Windows.Forms.DialogResult.Cancel)
                    SaveFile(ndiagram, false);
                else
                {
                    Log.WriteLog("GraphViewContainer::StartSaveProcess[" + forceFileNameSelect + "]::DialogResult=" + this.DialogResult.ToString() + "::ndiagram.Name = " + ndiagram.Name + Environment.NewLine + Environment.StackTrace);
                    SetFormPropertiesForSave(true);
                    DockingForm.DockForm.ResetStatus();
                }
                this.View.EndUpdate();
                this.View.Document.UpdateViews();
                //this.View.Document.IsModified = false;
                OnSaveComplete(this, EventArgs.Empty);
            }
            else
            {
                Log.WriteLog("GraphViewContainer::StartSaveProcess[" + forceFileNameSelect + "]::ndiagram is NULL" + Environment.NewLine + Environment.StackTrace);
                //this.View.BeginUpdate();
                Symbol bdoc = myView.Document as Symbol;

                DetermineFilename(bdoc, forceFileNameSelect);
                if (bdoc.Name != string.Empty && this.DialogResult != System.Windows.Forms.DialogResult.Cancel)
                {
                    FileUtil futil = new FileUtil();
                    GraphNode node = bdoc.GetObjectToSave();
                    futil.Save(bdoc, bdoc.Name);
                }
                this.View.EndUpdate();
                this.View.Document.UpdateViews();
                //this.View.Document.IsModified = false;
                OnSaveComplete(this, EventArgs.Empty);

                //StorageManipulator.FileSystemManipulator.SaveDocument(this.View.Document as BaseDocument,"c:\\temp.sym");
            }
        }

        private bool ignoreSkipRefreshWhenOpeningAServerFile = false;
        private NormalDiagram DoDiagramVersionControl(NormalDiagram ndiagram)
        {
            //if (OpeningFromServer)
            //{
            //    Log.WriteLog("Diagram opening from server, version control skipped");
            //    return ndiagram;
            //}

            //can improve by getting list on startup IF SERVER VERSION
            if (ndiagram != null)
            {
                if (ndiagram.VersionManager.CurrentVersion.WorkspaceTypeId == 3 && Core.Variables.Instance.IsServer && Core.Variables.Instance.ServerConnectionString.Length > 0)
                {
                    MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter tempAd = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();
                    ignoreSkipRefreshWhenOpeningAServerFile = true;
                    string dName = strings.GetFileNameOnly(ndiagram.Name).Replace(".xml", "");
                    Log.WriteLog("Diagram version control started for " + dName);
                    VCStatusList localState = VCStatusList.None;
                    try
                    {
                        TList<GraphFile> files = tempAd.GetFilesByWorkspaceTypeIdWorkspaceName(ndiagram.VersionManager.CurrentVersion.WorkspaceTypeId, ndiagram.VersionManager.CurrentVersion.WorkspaceName, 1, false);//d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileProvider.GetByWorkspaceNameWorkspaceTypeId(ndiagram.VersionManager.CurrentVersion.WorkspaceName, ndiagram.VersionManager.CurrentVersion.WorkspaceTypeId);
                        foreach (GraphFile localFile in files)
                        {
                            if (localFile.IsActive && localFile.OriginalFileUniqueID == ndiagram.VersionManager.CurrentVersion.OriginalFileUniqueIdentifier)
                            {
                                localState = localFile.State;
                                Log.WriteLog("Local file found in state " + localState.ToString());
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.WriteLog("Cannot connect to the local SQL Server" + Environment.NewLine + ex.ToString());//this can never happen (how did u get into metabuilder?)
                    }

                    VCStatusList serverState = VCStatusList.None;
                    try
                    {
                        TList<GraphFile> serverfiles = tempAd.GetFilesByWorkspaceTypeIdWorkspaceName(ndiagram.VersionManager.CurrentVersion.WorkspaceTypeId, ndiagram.VersionManager.CurrentVersion.WorkspaceName, 1, true); // d.DataRepository.Connections[Core.Variables.Instance.ServerProvider].Provider.GraphFileProvider.GetByWorkspaceNameWorkspaceTypeId(ndiagram.VersionManager.CurrentVersion.WorkspaceName, ndiagram.VersionManager.CurrentVersion.WorkspaceTypeId);
                        string thisVCUser = strings.GetVCIdentifier();
                        foreach (GraphFile serverFile in serverfiles)
                        {
                            if (serverFile.OriginalFileUniqueID == ndiagram.VersionManager.CurrentVersion.OriginalFileUniqueIdentifier)
                                if (serverFile.IsActive) // && serverFile.VCUser == thisVCUser
                                {
                                    serverState = serverFile.State;
                                    Log.WriteLog("Server file found in state " + serverState.ToString());//this WILL happen when you are not offsite and cannot connect to the server. THEREFORE LOCAL STATE IS REALLY IMPORTANT
                                    break;
                                }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.WriteLog("Cannot connect to the server SQL Server" + Environment.NewLine + ex.ToString());
                    }

                    if (serverState != VCStatusList.None) //Server state can never be none
                    {
                        localState = serverState;
                        Log.WriteLog("Local state overwritten by server state");
                    }

                    if (localState != VCStatusList.None) //if its not a local diagram, it cant be its in a server workspace(but may have never been checked in)
                    {
                        ndiagram.VersionManager.CurrentVersion.VCStatus = localState;
                        ReadOnly = localState == VCStatusList.CheckedOutRead || localState == VCStatusList.CheckedIn || localState == VCStatusList.Obsolete || localState == VCStatusList.Locked;
                        Log.WriteLog("Diagram version control complete, state set to " + localState.ToString());
                    }
                    else
                    {
                        Log.WriteLog("State cannot be found, aborting version control");
                        ignoreSkipRefreshWhenOpeningAServerFile = false;
                    }
                }
            }
            else
            {
                Log.WriteLog("Diagram is null, aborting version control");
            }

            return ndiagram;
        }

        private void AutoReplaceStencils()
        {
            foreach (GoObject o in myView.Document)
            {
                if (!(o is IMetaNode))
                    continue;

                if (o is CollapsibleNode)
                    (o as CollapsibleNode).Expand();
            }

            foreach (KeyValuePair<string, string> kvp in Variables.Instance.RenamedClasses)
            {
                if (Core.Variables.Instance.ClassesWithoutStencil.Contains(kvp.Value.ToString()))
                    continue;

                ViewController.MetaConvertSelection(kvp.Value, true, true, kvp.Key.Replace("DataViewLog", "DataView").ToString());
            }
            myView.Selection.Clear();
        }

        public void FinaliseDocumentAfterLoading(bool SkipRefresh)
        {
            SetTabText();
            //if (myView.Document.UndoManager == null)
            //    myView.Document.UndoManager = new GoUndoManager();
            //else
            //    myView.Document.UndoManager.Clear();

            SetPortVisibility(true);

            NormalDiagram ndiagram = myView.Document as NormalDiagram;
            if (ndiagram != null)
            {
                //ApplyQuality(ZoomQuality.Max, ndiagram);

                DocumentVersion = ndiagram.VersionManager.CurrentVersion;

                bool editable = (ndiagram.VersionManager.CurrentVersion.VCStatus == VCStatusList.None || ndiagram.VersionManager.CurrentVersion.VCStatus == VCStatusList.CheckedOut || ndiagram.VersionManager.CurrentVersion.VCStatus == VCStatusList.PCI_Revoked);
                //editable = editable == false ? editable : ReadOnly;
                //if (ndiagram.VersionManager.CurrentVersion.VCStatus == VCStatusList.CheckedOutRead)
                {
                    myView.AllowEdit = editable;
                    myView.AllowMove = editable;
                    myView.AllowDrop = editable;
                    myView.AllowCopy = editable;
                    myView.AllowDelete = editable;
                    myView.AllowInsert = editable;
                    myView.AllowLink = editable;
                    myView.AllowReshape = editable;
                    myView.AllowResize = editable;
                }

                if (ndiagram.LastPrintSettings != null)
                {
                    RestorePrintSettingsAndSheetBounds(ndiagram);
                }
                else
                {
                    if (ndiagram.VersionManager.CurrentVersion.SheetBounds.X > 0)
                    {
                        myView.Sheet.Bounds = ndiagram.VersionManager.CurrentVersion.SheetBounds;
                    }
                    else
                    {
                        menuItemToolsCropToDocumentFrameToolStripMenuItem_Click(this, EventArgs.Empty);
                    }
                }
                if (!Core.Variables.Instance.IsViewer)
                    ValidateWorkspaceAndVersionSettings(ndiagram);

                DockingForm.DockForm.SetToolstripWorkspaceToDiagram(ndiagram.VersionManager.CurrentVersion);

                FixSelfPorts fsports = new FixSelfPorts(myView.Document);
                ViewController.FixOldAddShapes(myView.Document);
            }

            //UpdateDocumentIndicators();
            //myView.Document.SkipsUndoManager = false;
            //myView.Document.IsModified = true;
            //THIS IS TOO DIRTY: RATHER FIX THE WRAPPING ISSUE! FixOldAddShapes(myView.Document);
            //if (ndiagram != null)
            //    ValidateWorkspaceAndVersionSettings(ndiagram);

            //added skiprefresh here
            //RemovePorts(myView.Document);
            //AddNewPorts(myView.Document);
            //      ShallowCopyDictionary = new Dictionary<string, List<GraphNode>>();
            //   List<GraphNode> nodes = new List<GraphNode>();
            //  GetNodesInCollection(nodes, myView.Document);

            UpdateDocumentIndicators(SkipRefresh);
            ViewController.CheckForLegend();
            ExecutePluginsAfterLoad();

            if (!DockingForm.DockForm.OpenedFiles.ContainsKey((ViewController.MyView.Document as NormalDiagram).Name.ToLower()))
            {
                DockingForm.DockForm.OpenedFiles.Add((ViewController.MyView.Document as NormalDiagram).Name.ToLower(), ContainerID);
            }
            try
            {
                relinkNullPorts();
            }
            catch (Exception ex)
            {
                Log.WriteLog("GraphViewContainer::FinaliseDocumentAfterLoading::relinkNullPorts " + ex.ToString());
            }

            CustomModified = false;
            myView.Document.IsModified = false;

            View.Document.FinishTransaction("Open Document");

            View.Document.FinishTransaction("Making sure");

            View.Document.FinishTransaction("Really sure");

            myView.Document.UndoManager.Clear();
        }

        public void relinkNullPorts() //TODO : Image nodes do not save ports so this is required
        {
            foreach (GoObject o in myView.Document)
            {
                if (o is QLink)
                {
                    QLink lnk = o as QLink;
                    if (lnk.ToPort == null || lnk.ToPort.Node == null)
                    {
                        if (lnk.ShallowToN != "")
                        {
                            //find the object whose metaobject pkid is in string and whose location is in string
                            foreach (GoObject obj in myView.Document)
                            {
                                if (lnk.ToPort != null)
                                {
                                    lnk.ShallowToN = "";
                                    lnk.toPortLoc = "";
                                    break;
                                }
                                if (obj is ImageNode)
                                {
                                    ImageNode i = obj as ImageNode;
                                    if (lnk.ShallowToN.Contains(i.MetaObject.pkid.ToString()) && lnk.ShallowToN.Contains(i.Location.ToString()))
                                    {
                                        lnk.ToPort = i.GetPort(lnk.toPortLoc) as IGoPort;
                                        lnk.ShallowToN = "";
                                        lnk.toPortLoc = "";
                                        break;
                                    }
                                    if (lnk.ShallowToN.Contains(i.Location.ToString()))
                                    {
                                        lnk.ToPort = i.GetPort(lnk.toPortLoc) as IGoPort;
                                        lnk.ShallowToN = "";
                                        lnk.toPortLoc = "";
                                        break;
                                    }
                                }
                                else if (obj is CollapsibleNode)
                                {
                                    CollapsibleNode i = obj as CollapsibleNode;
                                    if (lnk.ShallowToN.Contains(i.MetaObject.pkid.ToString()) && lnk.ShallowToN.Contains(i.Location.ToString()))
                                    {
                                        //go through each !header node and check if lnks bounds x is ==middle left/right

                                        foreach (GoObject CNchild in i.GetEnumerator())
                                        {
                                            if (lnk.ToPort != null)
                                            {
                                                lnk.ShallowToN = "";
                                                lnk.toPortLoc = "";
                                                break;
                                            }
                                            if (!(CNchild is CollapsingRecordNodeItemList))
                                                continue;

                                            foreach (GoObject section in (CNchild as CollapsingRecordNodeItemList).GetEnumerator())
                                            {
                                                if (lnk.ToPort != null)
                                                {
                                                    lnk.ShallowToN = "";
                                                    lnk.toPortLoc = "";
                                                    break;
                                                }
                                                if (!(section is RepeaterSection))
                                                    continue;

                                                foreach (GoObject sectionItem in (section as RepeaterSection).GetEnumerator())
                                                {
                                                    if (lnk.ToPort != null)
                                                    {
                                                        lnk.ShallowToN = "";
                                                        lnk.toPortLoc = "";
                                                        break;
                                                    }
                                                    if (sectionItem is CollapsingRecordNodeItem)
                                                    {
                                                        CollapsingRecordNodeItem CNitem = sectionItem as CollapsingRecordNodeItem;
                                                        if (CNitem.IsHeader)
                                                            continue;

                                                        //is the lnks start point equal to left or right
                                                        foreach (PointF p in lnk.RealLink.CopyPointsArray())
                                                        {
                                                            if (lnk.ToPort != null)
                                                            {
                                                                lnk.ShallowToN = "";
                                                                lnk.toPortLoc = "";
                                                                break;
                                                            }
                                                            if (p.X - CNitem.RightPort.Right < 2)
                                                            {
                                                                float y = (CNitem.RightPort.Top + (CNitem.RightPort.Bounds.Height / 2));
                                                                if (p.Y - y < 2)
                                                                {
                                                                    lnk.ToPort = CNitem.RightPort;
                                                                }
                                                            }
                                                            else if (p.X - CNitem.LeftPort.Left < 2)
                                                            {
                                                                float y = (CNitem.LeftPort.Top + (CNitem.LeftPort.Bounds.Height / 2));
                                                                if (p.Y - y < 2)
                                                                {
                                                                    lnk.ToPort = CNitem.LeftPort;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                            if (lnk.ToPort == null || (!(lnk.ToPort is QuickPort) && (lnk.ToPort as GoPort).ParentNode is ImageNode))
                            {
                                createLinkTask(lnk);
                            }
                            else
                            {
                                if ((lnk.ToPort as GoPort).Document == null)
                                {
                                    createLinkTask(lnk);
                                }
                            }
                        }
                        else
                        {
                            createLinkTask(lnk);
                        }
                    }
                    // no else both ports can be null :)
                    if (lnk.FromPort == null || lnk.FromPort.Node == null)
                    {
                        if (lnk.ShallowFromN != "")
                        {
                            //find the object whose metaobject pkid is in string and whose location is in string
                            foreach (GoObject obj in myView.Document)
                            {
                                if (lnk.FromPort != null)
                                {
                                    lnk.ShallowFromN = "";
                                    lnk.fromPortLoc = "";
                                    break;
                                }
                                if (obj is ImageNode)
                                {
                                    ImageNode i = obj as ImageNode;
                                    if (lnk.ShallowFromN.Contains(i.MetaObject.pkid.ToString()) && lnk.ShallowFromN.Contains(i.Location.ToString()))
                                    {
                                        lnk.FromPort = i.GetPort(lnk.fromPortLoc) as IGoPort;
                                        lnk.ShallowFromN = "";
                                        lnk.fromPortLoc = "";
                                        break;
                                    }
                                    if (lnk.ShallowFromN.Contains(i.Location.ToString()))
                                    {
                                        lnk.FromPort = i.GetPort(lnk.fromPortLoc) as IGoPort;
                                        lnk.ShallowFromN = "";
                                        lnk.fromPortLoc = "";
                                        break;
                                    }
                                }
                                else if (obj is CollapsibleNode)
                                {
                                    CollapsibleNode i = obj as CollapsibleNode;
                                    if (lnk.ShallowFromN.Contains(i.MetaObject.pkid.ToString()) && lnk.ShallowFromN.Contains(i.Location.ToString()))
                                    {
                                        //go through each !header node and check if lnks bounds x is ==middle left/right

                                        foreach (GoObject CNchild in i.GetEnumerator())
                                        {
                                            if (lnk.FromPort != null)
                                            {
                                                lnk.ShallowFromN = "";
                                                lnk.fromPortLoc = "";
                                                break;
                                            }
                                            if (!(CNchild is CollapsingRecordNodeItemList))
                                                continue;

                                            foreach (GoObject section in (CNchild as CollapsingRecordNodeItemList).GetEnumerator())
                                            {
                                                if (lnk.FromPort != null)
                                                {
                                                    lnk.ShallowFromN = "";
                                                    lnk.fromPortLoc = "";
                                                    break;
                                                }
                                                if (!(section is RepeaterSection))
                                                    continue;

                                                foreach (GoObject sectionItem in (section as RepeaterSection).GetEnumerator())
                                                {
                                                    if (lnk.FromPort != null)
                                                    {
                                                        lnk.ShallowFromN = "";
                                                        lnk.fromPortLoc = "";
                                                        break;
                                                    }
                                                    if (sectionItem is CollapsingRecordNodeItem)
                                                    {
                                                        CollapsingRecordNodeItem CNitem = sectionItem as CollapsingRecordNodeItem;
                                                        if (CNitem.IsHeader)
                                                            continue;

                                                        //is the lnks start point equal to left or right
                                                        foreach (PointF p in lnk.RealLink.CopyPointsArray())
                                                        {
                                                            if (lnk.FromPort != null)
                                                            {
                                                                lnk.ShallowFromN = "";
                                                                lnk.fromPortLoc = "";
                                                                break;
                                                            }
                                                            if (p.X - CNitem.RightPort.Right < 2)
                                                            {
                                                                float y = (CNitem.RightPort.Top + (CNitem.RightPort.Bounds.Height / 2));
                                                                if (p.Y - y < 2)
                                                                {
                                                                    lnk.FromPort = CNitem.RightPort;
                                                                }
                                                            }
                                                            else if (p.X - CNitem.LeftPort.Left < 2)
                                                            {
                                                                float y = (CNitem.LeftPort.Top + (CNitem.LeftPort.Bounds.Height / 2));
                                                                if (p.Y - y < 2)
                                                                {
                                                                    lnk.FromPort = CNitem.LeftPort;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                        }

                                        break;
                                    }
                                }
                            }
                            if (lnk.FromPort == null || (!(lnk.FromPort is QuickPort) && (lnk.FromPort as GoPort).ParentNode is ImageNode))
                            {
                                createLinkTask(lnk);
                            }
                            else
                            {
                                if ((lnk.FromPort as GoPort).Document == null)
                                {
                                    createLinkTask(lnk);
                                }
                            }
                        }
                        else
                        {
                            createLinkTask(lnk);
                        }
                    }
                }
                else if (o is FishLink)
                {
                    FishLink lnk = o as FishLink;
                    if (lnk.FromPort == null && lnk.FromArtImageNode != null && lnk.FromArtImageNode.Length > 0)
                    {
                        foreach (GoObject obj in myView.Document)
                        {
                            if (obj is ImageNode)
                            {
                                ImageNode i = obj as ImageNode;
                                if (lnk.FromArtImageNode.Contains(i.MetaObject.pkid.ToString()) && lnk.FromArtImageNode.Contains(i.Location.ToString()))
                                {
                                    lnk.FromPort = i.Port;
                                    lnk.FromArtImageNode = null;
                                    break;
                                }
                            }
                        }

                        if (lnk.FromPort == null)
                        {
                            lnk.Brush = Brushes.DarkRed;//new SolidBrush(Color.DarkRed);
                            Core.Log.WriteLog(lnk.FromArtImageNode + " port not found");
                        }
                        else
                        {
                            if ((lnk.FromPort as GoPort).Document == null)
                            {

                            }
                        }
                    }
                }
            }
        }
        private void createLinkTask(QLink lnk)
        {
            Pen p = new Pen(Color.DarkRed, 3);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
            lnk.Pen = p;

            MissingPortTask pTask = new MissingPortTask();
            pTask.Tag = lnk;
            pTask.MyView = myView;
            pTask.ContainerID = this.ContainerID.ToString();
            pTask.IsCritical = true;
            pTask.Caption = lnk.AssociationType.ToString();
            DockingForm.DockForm.m_taskDocker.AddTask(this.ContainerID.ToString(), pTask);
            DockingForm.DockForm.m_taskDocker.BindToList(this.ContainerID.ToString());
        }
        public void CreateInvalidLinkTask(QLink lnk)
        {
            if (lnk.NotInModel)
            {
                NotInModelLinkTask pTask = new NotInModelLinkTask();
                pTask.Tag = lnk;
                pTask.MyView = myView;
                pTask.ContainerID = this.ContainerID.ToString();
                pTask.IsCritical = true;
                pTask.Caption = lnk.AssociationType.ToString();
                DockingForm.DockForm.m_taskDocker.AddTask(this.ContainerID.ToString(), pTask);
                DockingForm.DockForm.m_taskDocker.BindToList(this.ContainerID.ToString());
            }
        }
        private void RefreshData(IGoCollection selection, bool prompt)
        {
            if (Core.Variables.Instance.IsViewer)
                return;
            if (myView.Document is NormalDiagram)
            {

                #region User Initiated Stencil Replace
                foreach (GoObject obj in selection)
                {
                    {
                        if (obj is GraphNode)
                        {
                            if ((obj as GraphNode).CanBeAutomaticallyReplaced)
                            {
                                if (ReadOnly)
                                {
                                    AutoReplaceStencils();
                                    break;
                                }
                                else
                                {
                                    if (Variables.Instance.RefreshOnOpenDiagram == Variables.RefreshType.Prompt || Variables.Instance.RefreshOnOpenDiagram == Variables.RefreshType.Never)
                                    {
                                        if (MessageBox.Show(DockingForm.DockForm, "This diagram has used shapes which are out of date. Their data has been converted but not their representation. Would you like to convert all the old shapes to the new ones?", "Old Shape Conversion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                        {
                                            AutoReplaceStencils();
                                        }
                                    }
                                    else if (Variables.Instance.RefreshOnOpenDiagram == Variables.RefreshType.Automatic)
                                    {
                                        AutoReplaceStencils();
                                    }
                                    //else
                                    //{
                                    //    //We do not replace stencils automatically
                                    //    if (MessageBox.Show(DockingForm.DockForm,"This diagram has used stencils which are out of date. Their data has been converted but not their representation. Would you like to convert all the old stencils to the new ones?", "Old Stencil Conversion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                    //    {
                                    //        AutoReplaceStencils();
                                    //    }
                                    //}
                                }
                                break;
                            }
                        }
                    }
                }
                #endregion

                //bool compared = false;

                if (ReadOnly || openingFromServer || !prompt)
                {
                    Tools.MetaComparerWorker mcomparer = new MetaBuilder.UIControls.GraphingUI.Tools.MetaComparerWorker(selection, true, this.myView);
                    mcomparer.RunWorkerCompleted += new RunWorkerCompletedEventHandler(mcomparer_RunWorkerCompletedNoDialog);
                }
                else
                {
                    if (Core.Variables.Instance.RefreshOnOpenDiagram != Variables.RefreshType.Never)
                    {
                        Tools.MetaComparerWorker mcomparer = new MetaBuilder.UIControls.GraphingUI.Tools.MetaComparerWorker(selection, false, this.myView);
                        mcomparer.RunWorkerCompleted += new RunWorkerCompletedEventHandler(mcomparer_RunWorkerCompleted);
                    }
                }
            }
        }
        private void mcomparer_RunWorkerCompletedNoDialog(object sender, RunWorkerCompletedEventArgs e)
        {
            Tools.MetaComparerWorker mcomparer = sender as Tools.MetaComparerWorker;
            mcomparer.Dispose();
            DockingForm.DockForm.ResetStatus();
        }
        private void mcomparer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Tools.MetaComparerWorker mcomparer = sender as Tools.MetaComparerWorker;
            //will not fire if only links changed :((
            try
            {
                //if (mcomparer.DiffData != null && mcomparer.DiffData.Count > 0)
                //{
                Dialogs.RefreshData rdata = new RefreshData();
                rdata.FormClosed += new FormClosedEventHandler(rdata_FormClosed);

                rdata.View = this.myView;
                rdata.DifferingObjects = mcomparer.DiffData;
                rdata.Populate();

                if (rdata.HasRows)
                {
                    DockingForm.DockForm.UpdateStatusLabel("Refresh Data?");
                    rdata.Show(DockingForm.DockForm);
                    rdata.BringToFront();
                    if (Variables.Instance.CompareLinks)
                        rdata.PopulateLinks(mcomparer.lComparer);
                }
                else
                {
                    DockingForm.DockForm.ResetStatus();
                }
                //}
                mcomparer.Dispose();
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString());
            }
        }

        private void rdata_FormClosed(object sender, FormClosedEventArgs e)
        {
            Dialogs.RefreshData rdata = sender as Dialogs.RefreshData;
            if (rdata.DialogResult == DialogResult.OK)
            {
                ViewController.RemoveChangedIndicators(myView.Document, false);
            }
            rdata.DifferingObjects = null;
            rdata.Dispose();
            DockingForm.DockForm.ResetStatus();
        }

        //OLD not threaded
        //private void RefreshDatax(IGoCollection selection, bool prompt)
        //{
        //    if (Core.Variables.Instance.IsViewer)
        //        return;
        //    //if (myView.AllowEdit) regardless of if you are allowed to edit or not if you choose to refresh IT MUST REFRESH
        //    //{
        //    if (myView.Document is NormalDiagram)
        //    {

        //        #region User Initiated Stencil Replace
        //        foreach (GoObject obj in selection)
        //        {
        //            {
        //                if (obj is GraphNode)
        //                {
        //                    if ((obj as GraphNode).CanBeAutomaticallyReplaced)
        //                    {
        //                        if (ReadOnly)
        //                        {
        //                            AutoReplaceStencils();
        //                            break;
        //                        }
        //                        else
        //                        {
        //                            if (Variables.Instance.RefreshOnOpenDiagram == Variables.RefreshType.Prompt || Variables.Instance.RefreshOnOpenDiagram == Variables.RefreshType.Never)
        //                            {
        //                                if (MessageBox.Show(DockingForm.DockForm, "This diagram has used stencils which are out of date. Their data has been converted but not their representation. Would you like to convert all the old stencils to the new ones?", "Old Stencil Conversion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        //                                {
        //                                    AutoReplaceStencils();
        //                                }
        //                            }
        //                            else if (Variables.Instance.RefreshOnOpenDiagram == Variables.RefreshType.Automatic)
        //                            {
        //                                AutoReplaceStencils();
        //                            }
        //                            //else
        //                            //{
        //                            //    //We do not replace stencils automatically
        //                            //    if (MessageBox.Show(DockingForm.DockForm,"This diagram has used stencils which are out of date. Their data has been converted but not their representation. Would you like to convert all the old stencils to the new ones?", "Old Stencil Conversion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        //                            //    {
        //                            //        AutoReplaceStencils();
        //                            //    }
        //                            //}
        //                        }
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        #endregion

        //        //bool compared = false;
        //        Tools.MetaComparer mcomparer = new MetaBuilder.UIControls.GraphingUI.Tools.MetaComparer();
        //        mcomparer.MyView = this.myView;

        //        if (ReadOnly)
        //        {
        //            mcomparer.RefreshSelection(selection);
        //            //compared = true;
        //            //return;
        //        }
        //        else
        //        {
        //            if (prompt)
        //            {
        //                if (openingFromServer)
        //                {
        //                    mcomparer.RefreshSelection(selection);
        //                }
        //                else
        //                {
        //                    mcomparer.CompareSelection(selection);
        //                    if (mcomparer.DiffData.Count > 0)
        //                    {
        //                        Dialogs.RefreshData rdata = new RefreshData();
        //                        rdata.View = this.myView;
        //                        rdata.DifferingObjects = mcomparer.DiffData;
        //                        rdata.Populate();
        //                        rdata.ShowDialog(DockingForm.DockForm);

        //                        if (rdata.DialogResult == DialogResult.OK)
        //                        {
        //                            ViewController.RemoveChangedIndicators(myView.Document, false);
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                mcomparer.RefreshSelection(selection);
        //                //compared = true;
        //            }
        //        }
        //        mcomparer = null;
        //    }

        //    //}
        //}

        private void UpdateDocumentIndicators(bool SkipRefresh)
        {
            if (ReadOnly)
            {
                RefreshData(myView.Document, false);
                return;
            }
            else
            {
                if (!SkipRefresh)
                {
                    if (Core.Variables.Instance.RefreshOnOpenDiagram == Variables.RefreshType.Automatic)
                    {
                        RefreshData(myView.Document, false);
                        return;
                    }
                    else if (Core.Variables.Instance.RefreshOnOpenDiagram == Variables.RefreshType.Prompt || Core.Variables.Instance.ValidateVersionControl)
                    {
                        if (ignoreSkipRefreshWhenOpeningAServerFile)
                            RefreshData(myView.Document, false);
                        else
                            RefreshData(myView.Document, true);
                        return;
                    }
                }
            }
        }
        private void ValidateWorkspaceAndVersionSettings(NormalDiagram ndiagram)
        {
            if (ndiagram.VersionManager.CurrentVersion != null)
            {
                //if (ndiagram.VersionManager.CurrentVersion.PreviousDocumentID > 0)
                //{
                ValidateDiagramsWorkspaceEqualsDatabaseVersion(ndiagram);
                //}
                DocumentVersion dversion = ndiagram.VersionManager.CurrentVersion;
                b.Workspace workspace = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.WorkspaceProvider.GetByNameWorkspaceTypeId(dversion.WorkspaceName, dversion.WorkspaceTypeId);
                if (workspace == null)
                {
                    MessageBox.Show(DockingForm.DockForm, "This diagram's workspace ('" + dversion.WorkspaceName + "') cannot be found. It will be saved into the ('" + Core.Variables.Instance.CurrentWorkspaceName + "') workspace.", "Workspace Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //DockingForm.DockForm.DisplayTip("This diagram's workspace ('" + dversion.WorkspaceName + "') cannot be found. It will be saved into the ('" + Core.Variables.Instance.CurrentWorkspaceName + "') workspace.", "Workspace Not Found", ToolTipIcon.Warning);

                    dversion.WorkspaceName = Core.Variables.Instance.CurrentWorkspaceName;
                    dversion.WorkspaceTypeId = Core.Variables.Instance.CurrentWorkspaceTypeId;
                    DockingForm.DockForm.SetToolstripWorkspaceToDiagram(dversion);
                }
                else
                {
                    if (Core.Variables.Instance.WarnWhenDiagramIsFromADifferentWorkspace)
                    {
                        CompareWorkspaceToCurrent(dversion);
                    }
                }
                //dversion = null;
                if (workspace != null)
                    workspace.Dispose();
            }
        }
        private void ValidateDiagramsWorkspaceEqualsDatabaseVersion(NormalDiagram ndiagram)
        {
            d.OldCode.Diagramming.TempFileGraphAdapter tfg = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();
            b.GraphFile fileDetails = tfg.GetQuickFileDetails(ndiagram.VersionManager.CurrentVersion.OriginalFileUniqueIdentifier.ToString());
            if (fileDetails != null)
            {
                if ((fileDetails.WorkspaceName != ndiagram.VersionManager.CurrentVersion.WorkspaceName) || (fileDetails.WorkspaceTypeId != ndiagram.VersionManager.CurrentVersion.WorkspaceTypeId))
                {
                    MessageBox.Show(DockingForm.DockForm, "Warning: according to the database, this diagram has been transferred to the '" + fileDetails.WorkspaceName +
                        "' workspace." + Environment.NewLine + Environment.NewLine + "When you save the document it will automatically point to the new workspace.",
                        "Workspace Changed", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    //this.BringToFront();

                    // automatically update the file
                    ndiagram.VersionManager.CurrentVersion.WorkspaceTypeId = fileDetails.WorkspaceTypeId;
                    ndiagram.VersionManager.CurrentVersion.WorkspaceName = fileDetails.WorkspaceName;
                }
            }
            else //BROKEN CACHE
            {
                try
                {
                    //get active database diagram and check if this version is != to that version
                    foreach (GraphFile f in d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.GraphFileProvider.GetAll().FindAll(GraphFileColumn.OriginalFileUniqueID, ndiagram.VersionManager.CurrentVersion.OriginalFileUniqueIdentifier))
                    {
                        if (f.IsActive)//f.OriginalFileUniqueID == ndiagram.VersionManager.CurrentVersion.OriginalFileUniqueIdentifier && 
                        {
                            if ((f.WorkspaceName != ndiagram.VersionManager.CurrentVersion.WorkspaceName) || (f.WorkspaceTypeId != ndiagram.VersionManager.CurrentVersion.WorkspaceTypeId))
                            {
                                MessageBox.Show(DockingForm.DockForm, "Warning: according to the database, this diagram has been transferred to the '" + f.WorkspaceName +
                                    "' workspace." + Environment.NewLine + Environment.NewLine + "When you save the document it will automatically point to the new workspace.",
                                    "Workspace Changed", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                //this.BringToFront();

                                // automatically update the file
                                ndiagram.VersionManager.CurrentVersion.WorkspaceTypeId = f.WorkspaceTypeId;
                                ndiagram.VersionManager.CurrentVersion.WorkspaceName = f.WorkspaceName;
                            }
                        }
                        //we dont care if it is not found
                    }
                }
                catch
                {

                }
            }
            return;

            //d.OldCode.Diagramming.TempFileGraphAdapter tfg = new MetaBuilder.DataAccessLayer.OldCode.Diagramming.TempFileGraphAdapter();
            //b.GraphFile fileDetails = tfg.GetQuickFileDetails(ndiagram.VersionManager.CurrentVersion.PreviousDocumentID, ndiagram.VersionManager.CurrentVersion.MachineName, false);
            //if (fileDetails != null)
            //{
            //    if ((fileDetails.WorkspaceName != ndiagram.VersionManager.CurrentVersion.WorkspaceName) || (fileDetails.WorkspaceTypeId != ndiagram.VersionManager.CurrentVersion.WorkspaceTypeId))
            //    {
            //        MessageBox.Show(DockingForm.DockForm,"Warning: according to the database, this diagram has been transferred to the '" + fileDetails.WorkspaceName +
            //            "' workspace." + Environment.NewLine + Environment.NewLine + "When you save the document it will automatically point to the new workspace.",
            //            "Workspace Changed", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            //        this.BringToFront();

            //        // automatically update the file
            //        ndiagram.VersionManager.CurrentVersion.WorkspaceTypeId = fileDetails.WorkspaceTypeId;
            //        ndiagram.VersionManager.CurrentVersion.WorkspaceName = fileDetails.WorkspaceName;
            //        ndiagram.IsModified = true;
            //    }
            //}
        }
        private void CompareWorkspaceToCurrent(DocumentVersion documentVersion)
        {
            //bool workspaceDiffersFromCurrent = false;
            //if ((dversion.WorkspaceTypeId != Core.Variables.Instance.CurrentWorkspaceTypeId) || (dversion.WorkspaceName != Core.Variables.Instance.CurrentWorkspaceName))
            //{
            //    workspaceDiffersFromCurrent = true;
            //}

            if ((documentVersion.WorkspaceTypeId != Core.Variables.Instance.CurrentWorkspaceTypeId) || (documentVersion.WorkspaceName != Core.Variables.Instance.CurrentWorkspaceName))
            {
                StringBuilder sbuilder = new StringBuilder();
                sbuilder.Append("The diagram's workspace (" + documentVersion.WorkspaceName + ") differs from the currently selected workspace ('" + Core.Variables.Instance.CurrentWorkspaceName + "')");
                MessageBox.Show(DockingForm.DockForm, sbuilder.ToString(), "Different Workspace", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //DockingForm.DockForm.DisplayTip(sbuilder.ToString(), "Different Workspace", ToolTipIcon.Warning);
            }
            //this.BringToFront();
        }
        private void RestorePrintSettingsAndSheetBounds(NormalDiagram ndiagram)
        {
            PointF leftTopMost = new PointF(50000, 50000);
            PointF rightBottom = new PointF(-5000, -5000);
            foreach (GoObject o in ndiagram)
            {
                if (!(o is GoSheet))
                {
                    if (o.Position.X + o.Width > rightBottom.X)
                    {
                        rightBottom.X = o.Position.X + o.Width;
                    }
                    if (o.Position.Y + o.Height > rightBottom.Y)
                    {
                        rightBottom.Y = o.Position.Y + o.Height;
                    }
                    if (o.Position.X < leftTopMost.X)
                    {
                        leftTopMost.X = o.Position.X;
                    }
                    if (o.Position.Y < leftTopMost.Y)
                    {
                        leftTopMost.Y = o.Position.Y;
                    }
                }
            }
            RectangleF contentBounds = new RectangleF(leftTopMost, new SizeF(rightBottom.X - leftTopMost.X, rightBottom.Y - leftTopMost.Y));

            ndiagram.LastPrintSettings.ContentBounds = contentBounds;
            //ndiagram.LastPrintSettings.SheetBounds = ndiagram.VersionManager.CurrentVersion.SheetBounds;
            //ndiagram.LastPrintSettings.CalculateRatio();
            //myView.Sheet.Bounds = ndiagram.LastPrintSettings.SheetBounds;

            myView.Sheet.Bounds = ndiagram.VersionManager.CurrentVersion.SheetBounds;

            myView.Sheet.TopLeftMargin = new SizeF(ndiagram.LastPrintSettings.PageSet.Margins.Left, ndiagram.LastPrintSettings.PageSet.Margins.Top);
            myView.Sheet.BottomRightMargin = new SizeF(ndiagram.LastPrintSettings.PageSet.Margins.Right, ndiagram.LastPrintSettings.PageSet.Margins.Bottom);
        }

        #region AutoSave & Snapshots

        //public void TakeSnapshot()
        //{
        //    timerAutosave_Tick(this, EventArgs.Empty);
        //    myView.Doc.debugme = true;
        //}
        [NonSerialized]
        private string autosaveFileName;
        private void timerAutosave_Tick(object sender, EventArgs e)
        {
            foreach (DiagramSaver saver in DockingForm.DockForm.CurrentlySaving)
            {
                if (saver.Diagram.Name == (myView.Document as NormalDiagram).Name)
                {
                    //dont autosave this file while it is being saved
                    return;
                }
            }
            //if visible && DockingForm.DockForm.ActiveMdiChild == this)
            if (Variables.Instance.AutoSaveEnabled)
            {
                DoAutoSave();
            }
        }
        private AutoSaveForm asf;// = new AutoSaveForm();
        private void DoAutoSave()
        {
            if (!ReadOnly)
            {
                if (asf != null && !asf.IsDisposed)
                {
                    return;
                }
                if (myView.Document is NormalDiagram)
                {
                    NormalDiagram ndiagram = myView.Document as NormalDiagram;
                    //ndiagram.SerializesUndoManager = false;
                    if (ndiagram.Name.Contains("Temp Diagram"))
                        return;

                    if (ndiagram.Name == "")
                    {
                        if (autosaveFileName == null)
                            autosaveFileName = Variables.Instance.DiagramPath + "\\Autosave " + ContainerID.ToString() + ".mdgm";
                    }
                    else
                    {
                        // Get the path
                        string path = strings.GetPath(ndiagram.Name);
                        string filenameonly = strings.GetFileNameWithoutExtension(ndiagram.Name);
                        if (!filenameonly.Contains("Autosave"))
                        {
                            autosaveFileName = path + "Autosave " + filenameonly + ".mdgm";
                        }
                        else
                        {
                            autosaveFileName = path + filenameonly + ".mdgm";
                        }
                    }
                    asf = new AutoSaveForm();
                    asf.SetDocument(ndiagram, autosaveFileName);

                    //asf.Show();
                    asf.StartProcess();
                }
            }
        }

        #endregion

        #endregion

        #region Plugins

        private void LoadPlugins()
        {
            //Retrieve a plugin collection using our custom Configuration section handler
            PluginSectionHandler psh = new PluginSectionHandler();
            m_plugins = (PluginCollection)ConfigurationManager.GetSection("plugins");
            FillPluginMenu();
        }
        /// <summary>
        /// Create dynamic menu items according to each of the plugins we have loaded
        /// each menu item is connected to the same event handler delegate
        /// whcih then activates that plugin by the text of the menu item
        /// </summary>

        ToolStripMenuItem HANIP = null;
        private void FillPluginMenu()
        {
            if (Core.Variables.Instance.IsViewer)
                return;
            menuItemToolsPlugins.DropDown.Items.Clear();
            //Create the delegate right from the start
            //no need to create one for each menu item seperatly
            EventHandler handler = new EventHandler(OnPluginClick);
            foreach (IPlugin plugin in m_plugins)
            {
                if (HANIP == null)
                {
                    HANIP = new ToolStripMenuItem("Hierarchy Numbering");
                    HANIP.Name = "HANIP";
                    menuItemTools.DropDown.Items.Insert(menuItemTools.DropDownItems.Count - 1, HANIP);
                }
                //We add a small ampersand at the start of the name
                //so we can get shortcut key strokes for it
                ToolStripItem item = new ToolStripMenuItem("&" + plugin.Name.Replace("to", "To"), null, handler);
                if (plugin.Name == "Hierarchy Numbering")
                {
                    item.Text = "Add Numbering";
                    HANIP.DropDownItems.Add(item);
                }
                else if (plugin.Name == "Remove Numbering")
                {
                    HANIP.DropDownItems.Add(item);
                }
                else
                {
                    //menuItemToolsPlugins...MenuItems.Add(item);
                    menuItemTools.DropDown.Items.Insert(menuItemTools.DropDownItems.Count - 1, item);
                }
            }
        }
        /// <summary>
        /// Retrieves the name of the menu item
        /// and execute the needed plugin
        /// </summary>
        /// <param name="sender">MenuItem that was clicked</param>
        /// <param name="args">Default MenuItem EventArgs</param>
        private void OnPluginClick(object sender, EventArgs args)
        {
            try
            {
                ToolStripMenuItem item = (ToolStripMenuItem)sender;
                string plugName = item.Text.Replace("&", "");
                ExecutePlugin(plugName);
            }
            catch (Exception xxx)
            {
                LogEntry logEntry = new LogEntry();
                logEntry.Message = "Message: " + xxx.ToString();
                logEntry.Message += Environment.NewLine + Environment.NewLine + "StackTrace: " + xxx.StackTrace;
                logEntry.Message += Environment.NewLine + Environment.NewLine + "TargetSite:" + xxx.TargetSite.Name;
                Logger.Write(logEntry);
                MessageBox.Show(DockingForm.DockForm, "An error has occurred while running the plugin. The error has been logged.", "Plugin Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //Execute a plugin by name
        private void ExecutePlugin(string pluginName)
        {
            //create a context object to pass to the plugin
            //this context holds the current editor text
            //and the plugin might change the text as part of its work
            //we will then show the tranformed text
            if (pluginName == "Add Numbering")
                pluginName = "Hierarchy Numbering";
            GraphViewContext context = new GraphViewContext(myView, DockingForm.DockForm.GetCurrentSymbolStore());
            foreach (IPlugin plugin in m_plugins)
            {
                if (plugin.Name.ToLower() == pluginName.ToLower())
                {
                    try
                    {
                        DockingForm.DockForm.UpdateStatusLabel(plugin.Name);
                        if (!plugin.PerformAction(context))
                        {
                            if (plugin.Name == "Hierarchy Numbering")
                            {
                                (myView.Document as NormalDiagram).WasNumbered = true;
                            }
                            else if (plugin.Name == "Remove Numbering")
                            {
                                (myView.Document as NormalDiagram).WasNumbered = false;
                            }
                        }
                        DockingForm.DockForm.UpdateStatusLabel("Ready");
                    }
                    catch
                    {
                    }
                    //txtText.Text = context.CurrentDocumentText;
                    return;
                }
            }
        }

        #endregion

        private class ValidClassAssociationKey
        {
            private string parentClass;
            public string ParentClass
            {
                get { return parentClass; }
                set { parentClass = value; }
            }

            private string childClass;
            public string ChildClass
            {
                get { return childClass; }
                set { childClass = value; }
            }

            public override bool Equals(object obj)
            {
                return (obj as ValidClassAssociationKey).ToString().ToLower() == this.ToString().ToLower();
            }

            public override int GetHashCode()
            {
                return 0;
                //return base.GetHashCode();
            }

            public override string ToString()
            {
                return ParentClass + ChildClass;
            }
        }

        private MetaBuilder.Graphing.Formatting.FormattingManipulator forman;
        private enum CopyFormattingStyle { OnceOff, Ongoing, Off };

        private CopyFormattingStyle copyFormattingStyle;
        private void ApplyFormatCopy()
        {
            if (forman != null)
            {
                try
                {
                    if (copyFormattingStyle == CopyFormattingStyle.Ongoing && myView.Selection.Primary.GetType() != formattingObjectType)
                    {
                        forman = null;
                        copyFormattingStyle = CopyFormattingStyle.Off;
                        toolstripCopyFormatting.CheckState = CheckState.Unchecked;
                        formattingObjectType = null;
                        return;
                    }
                }
                catch
                {
                }

                myView.StartTransaction();
                forman.ApplyToSelection(myView.Selection);
                myView.FinishTransaction("Format Copy");
                if (copyFormattingStyle == CopyFormattingStyle.Ongoing)
                {
                    return;
                }
                forman = null;
                copyFormattingStyle = CopyFormattingStyle.Off;
                toolstripCopyFormatting.CheckState = CheckState.Unchecked;
                formattingObjectType = null;
            }
        }
        private void toolstripCopyFormatting_Click(object sender, EventArgs e)
        {
            FormatOnceOff();
        }
        private void toolstripCopyFormatting_DoubleClick(object sender, EventArgs e)
        {
            FormatOngoing();
        }
        private Type formattingObjectType = null;
        private void FormatOngoing()
        {
            if (copyFormattingStyle == CopyFormattingStyle.Ongoing)
            {
                forman = null;
                copyFormattingStyle = CopyFormattingStyle.Off;
                toolstripCopyFormatting.CheckState = CheckState.Unchecked;
                formattingObjectType = null;
                return;
            }
            toolstripCopyFormatting.CheckState = CheckState.Checked;
            copyFormattingStyle = CopyFormattingStyle.Ongoing;
            if (myView.Selection.Primary is IMetaNode)
            {
                formattingObjectType = myView.Selection.Primary.GetType();
                forman = new MetaBuilder.Graphing.Formatting.FormattingManipulator(this.myView.Selection);
            }
        }
        private void FormatOnceOff()
        {
            if (copyFormattingStyle == CopyFormattingStyle.Ongoing)
            {
                forman = null;
                copyFormattingStyle = CopyFormattingStyle.Off;
                toolstripCopyFormatting.CheckState = CheckState.Unchecked;
                formattingObjectType = null;
                return;
            }
            copyFormattingStyle = CopyFormattingStyle.OnceOff;
            // Store the current shapes formatting
            if (myView.Selection.Primary is IMetaNode)
            {
                formattingObjectType = myView.Selection.Primary.GetType();
                forman = new MetaBuilder.Graphing.Formatting.FormattingManipulator(this.myView.Selection);
            }
        }

        //CALLED VIA PROPERTIES FROM OPEN FILE FROM DATABASE
        public void UpdateReadonly()
        {
            menuItemFileSave.Visible = !ReadOnly;
            menuItemFileSaveAs.Visible = !ReadOnly;
            stripButtonSave.Visible = !ReadOnly;
            menuItemInsert.Visible = !ReadOnly;
            menuItemFormat.Visible = false;// !ReadOnly;
            menuItemShape.Visible = !ReadOnly;

            myView.AllowEdit = !ReadOnly;
            myView.AllowMove = !ReadOnly;
            myView.AllowDrop = !ReadOnly;
            myView.AllowCopy = true;
            myView.AllowKey = !ReadOnly;
            myView.AllowDelete = !ReadOnly;
            myView.AllowInsert = !ReadOnly;
            myView.AllowLink = !ReadOnly;
            myView.AllowReshape = !ReadOnly;
            myView.AllowResize = !ReadOnly;

            if (Core.Variables.Instance.IsViewer)
            {
                menuItemFileSave.Visible = true;
                stripButtonSave.Visible = true;
            }
        }
        public void ForceDatabaseObjectUpdateForStatus()
        {
            //RefreshData(myView.Document, false);
            UpdateReadonly();
        }

        private void ConvertAllAnchoredCommentsToRationale()
        {
            myView.StartTransaction();
            //myView.BeginUpdate();
            List<ResizableBalloonComment> commentsAffected = new List<ResizableBalloonComment>();
            if (myView.Selection.Count == 0)
            {
                foreach (GoObject o in myView.Document)
                {
                    if (!(o is ResizableBalloonComment)) continue;

                    ResizableBalloonComment comment = o as ResizableBalloonComment;
                    if (comment.Anchor != null)
                    {
                        commentsAffected.Add(comment);
                    }
                }
            }
            else
            {
                foreach (GoObject o in myView.Selection)
                {
                    if (!(o is ResizableBalloonComment)) continue;

                    ResizableBalloonComment comment = o as ResizableBalloonComment;
                    if (comment.Anchor != null)
                    {
                        commentsAffected.Add(comment);
                    }
                }
            }

            foreach (ResizableBalloonComment c in commentsAffected)
            {
                ResizableBalloonComment com = c;
                //get anchor and bounds
                GoObject anchor = com.Anchor;
                RectangleF bounds = com.Bounds;
                //remove from diagram
                myView.Document.Remove(com);
                //create rationale
                Rationale rat = new Rationale(false);
                rat.DONOTCHANGETHEMETABASETOBEEQUALETOTEXT = true;
                rat.CreateMetaObject(com, new EventArgs());
                rat.MetaObject.Set("Value", com.Text);
                //set anchor
                rat.Anchor = anchor;
                //set bounds
                rat.Bounds = bounds;
                //add to documents
                rat.BindToMetaObjectProperties();
                rat.Location = bounds.Location;
                myView.Document.Add(rat);
                rat.DONOTCHANGETHEMETABASETOBEEQUALETOTEXT = false;
            }

            myView.EndUpdate();
            myView.FinishTransaction("ConvertAnchoredCommentsToRationales");
        }

        #region QuickFormat

        ToolStripSeparator colorSeperator;
        ToolStripComboBox cbColorType;
        ToolStripComboBox cbGradientType;
        ToolStripLabel lblStartColour;
        ToolStripButton colorStart;
        ToolStripLabel lblEndColour;
        ToolStripButton colorEnd;

        ToolStripButton portManipulatorButton;

        ToolStripSeparator textSeperator;
        MetaControls.FontComboBox cbFontFamily;
        ToolStripComboBox cbFontSize;
        ToolStripLabel lblFontColour;
        ToolStripButton btnFontColour;
        ToolStripButton cbBold;
        ToolStripButton cbItalic;
        ToolStripButton cbUnderline;

        private bool setupFormatPanel;
        private void SetupFormatPanel()
        {
            if (setupFormatPanel || Core.Variables.Instance.IsViewer)
                return;

            textSeperator = new ToolStripSeparator();
            toolStripMain.Items.Add(textSeperator);

            cbFontFamily = new MetaControls.FontComboBox();
            cbFontFamily.AutoSize = false;
            cbFontFamily.Width = 200;
            cbFontFamily.DropDownWidth = 200;
            cbFontFamily.SelectedIndexChanged += formatControlChanged;
            cbFontFamily.TextChanged += formatControlChanged;
            cbFontFamily.DropDownStyle = ComboBoxStyle.DropDownList;
            toolStripMain.Items.Add(cbFontFamily);

            cbFontSize = new ToolStripComboBox();
            cbFontSize.AutoSize = false;
            cbFontSize.Width = 50;
            cbFontSize.DropDownWidth = 50;
            cbFontSize.SelectedIndexChanged += formatControlChanged;
            cbFontSize.TextChanged += formatControlChanged;
            cbFontSize.DropDownStyle = ComboBoxStyle.DropDownList;
            toolStripMain.Items.Add(cbFontSize);

            lblFontColour = new ToolStripLabel();
            lblFontColour.Text = "Text Colour:";
            toolStripMain.Items.Add(lblFontColour);

            btnFontColour = new ToolStripButton();
            btnFontColour.Click += colorPickerButton_Clicked;
            btnFontColour.BackColor = System.Drawing.Color.Black;
            btnFontColour.BackColorChanged += formatControlChanged;
            btnFontColour.Width = 30;
            toolStripMain.Items.Add(btnFontColour);

            loadFontItems();

            cbBold = new ToolStripButton();
            cbBold.CheckedChanged += formatControlChanged;
            cbBold.CheckOnClick = true;
            cbBold.Text = "B";
            cbBold.Font = new Font(cbBold.Font, FontStyle.Bold);
            toolStripMain.Items.Add(cbBold);

            cbItalic = new ToolStripButton();
            cbItalic.CheckedChanged += formatControlChanged;
            cbItalic.CheckOnClick = true;
            cbItalic.Text = "I";
            cbItalic.Font = new Font(cbItalic.Font, FontStyle.Italic);
            toolStripMain.Items.Add(cbItalic);

            cbUnderline = new ToolStripButton();
            cbUnderline.CheckedChanged += formatControlChanged;
            cbUnderline.CheckOnClick = true;
            cbUnderline.Text = "U";
            cbUnderline.Font = new Font(cbUnderline.Font, FontStyle.Underline);
            toolStripMain.Items.Add(cbUnderline);

            colorSeperator = new ToolStripSeparator();
            toolStripMain.Items.Add(colorSeperator);

            lblStartColour = new ToolStripLabel();
            lblStartColour.Text = "Start Colour:";
            toolStripMain.Items.Add(lblStartColour);
            colorStart = new ToolStripButton();
            colorStart.Click += colorPickerButton_Clicked;
            colorStart.BackColor = System.Drawing.Color.Black;
            colorStart.BackColorChanged += formatControlChanged;
            colorStart.Width = 30;
            toolStripMain.Items.Add(colorStart);

            lblEndColour = new ToolStripLabel();
            lblEndColour.Text = "End Colour:";
            toolStripMain.Items.Add(lblEndColour);
            colorEnd = new ToolStripButton();
            colorEnd.Click += colorPickerButton_Clicked;
            colorEnd.BackColor = System.Drawing.Color.Black;
            colorEnd.BackColorChanged += formatControlChanged;
            colorEnd.Width = 30;
            toolStripMain.Items.Add(colorEnd);

            //color type
            cbColorType = new ToolStripComboBox();
            cbColorType.AutoSize = false;
            cbColorType.Width = 75;
            cbColorType.DropDownWidth = 75;
            cbColorType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbColorType.Items.Add("Solid");
            cbColorType.Items.Add("Gradient");
            cbColorType.SelectedIndexChanged += formatControlChanged;
            toolStripMain.Items.Add(cbColorType);

            cbGradientType = new ToolStripComboBox();
            cbGradientType.AutoSize = false;
            cbGradientType.Width = 100;
            cbGradientType.DropDownWidth = 150;
            cbGradientType.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (MetaBuilder.Graphing.Formatting.GradientType gradient in Enum.GetValues(typeof(MetaBuilder.Graphing.Formatting.GradientType)))
                cbGradientType.Items.Add(gradient);
            cbGradientType.SelectedIndexChanged += formatControlChanged;
            toolStripMain.Items.Add(cbGradientType);

            portManipulatorButton = new ToolStripButton();
            portManipulatorButton.Text = "Manipulate Ports";
            portManipulatorButton.Click += new EventHandler(portManipulatorButton_Click);
            if (Variables.Instance.ShowDeveloperItems)
                toolStripMain.Items.Add(portManipulatorButton);

            ToolStripButton fixShapesButton = new ToolStripButton();
            fixShapesButton.Text = "Fix Shapes";
            fixShapesButton.Click += new EventHandler(fixShapesButton_Click);
            if (Variables.Instance.ShowDeveloperItems)
                toolStripMain.Items.Add(fixShapesButton);

            setupFormatPanel = true;

            TextFormatPanelVisibility(false);
            ColorFormatPanelVisibility();

            removeDummies();

#if DEBUG
            ToolStripButton defaultportsbtn = new ToolStripButton();
            defaultportsbtn.Click += new EventHandler(defaultportsbtn_Click);
            defaultportsbtn.Text = "Default port selection";
            toolStripMain.Items.Add(defaultportsbtn);
#endif
        }

        void defaultportsbtn_Click(object sender, EventArgs e)
        {
            //show panel with default from and to ports to select
            DefaultPortPositionSelector sel = new DefaultPortPositionSelector();
            sel.StartPosition = FormStartPosition.CenterParent;
            //sel.Location = new Point(MousePosition.X, MousePosition.Y);
            sel.ShowDialog();
        }

        MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation PortLocation = MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Circumferential;
        private void portManipulatorButton_Click(object sender, EventArgs e)
        {
            PortManipulator p = new PortManipulator();
            p.Text += " Current " + PortLocation.ToString();
            PortLocation = MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Circumferential;
            if (p.ShowDialog(DockingForm.DockForm) == DialogResult.OK)
                PortLocation = p.Position;
            portManipulatorButton.Text = "Manipulate Port => " + PortLocation.ToString();
        }

        private void loadFontItems()
        {
            InstalledFontCollection InstalledFonts = new InstalledFontCollection();
            foreach (FontFamily family in InstalledFonts.Families)
                cbFontFamily.Items.Add(family.Name);

            cbFontSize.Items.AddRange(new object[] { "8", "9", "10", "12", "14", "16", "18", "24", "30", "36", "48", "72" });
        }

        private void TextFormatPanelVisibility(bool visible)
        {
            if (Core.Variables.Instance.IsViewer || ViewController.MetaConverting)
                return;
            if (ReadOnly)
                visible = false;
            isSelecting = true;

            cbFontFamily.Enabled = cbFontSize.Enabled = textSeperator.Enabled = cbBold.Enabled = cbItalic.Enabled = cbUnderline.Enabled = btnFontColour.Enabled = lblFontColour.Enabled = visible;

            if (visible)
            {
                GoText label = null;
                if ((myView.Selection.Primary is ResizableBalloonComment || myView.Selection.Primary is ResizableComment) && !(myView.Selection.Primary is Rationale))
                    label = null;
                else if (myView.Selection.Primary is GoText)
                    label = myView.Selection.Primary as GoText;
                else
                {
                    if (myView.Selection.Primary is CollapsingRecordNodeItem)
                    {
                        label = (myView.Selection.Primary as CollapsingRecordNodeItem).GetLabel;
                    }
                    else if (myView.Selection.Primary is GoGroup)
                    {
                        foreach (GoObject o in myView.Selection.Primary as GoGroup)
                        {
                            if (o is GoText)
                            {
                                if ((o as GoText).EditorStyle == GoTextEditorStyle.TextBox)
                                {
                                    if (o is BoundLabel)
                                    {
                                        if ((o as BoundLabel).Name.Contains("_"))
                                            continue;
                                        label = o as GoText;
                                        break;
                                    }
                                    else
                                    {
                                        //if ((o as GoText).Editable)
                                        {
                                            label = o as GoText;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        myView.Selection.Primary.ToString();
                    }
                }

                if (label != null)
                {
                    btnFontColour.BackColor = label.TextColor;
                    cbFontFamily.SelectedItem = label.FamilyName;
                    cbFontSize.SelectedItem = label.FontSize.ToString();
                    cbBold.Checked = label.Font.Bold;
                    cbItalic.Checked = label.Font.Italic;
                    cbUnderline.Checked = label.Font.Underline;
                }
                else
                {
                    cbFontFamily.Enabled = cbFontSize.Enabled = textSeperator.Enabled = cbBold.Enabled = cbItalic.Enabled = cbUnderline.Enabled = btnFontColour.Enabled = lblFontColour.Enabled = false;
                }
            }

            isSelecting = false;
        }
        private void ColorFormatPanelVisibility()
        {
            if (Core.Variables.Instance.IsViewer || myView.Selection.First is FrameLayerRect || ViewController.MetaConverting)
                return;

            cbColorType.Visible = false;
            //cbGradientType.Visible = false;
            cbColorType.Enabled = false;
            cbGradientType.Enabled = false;

            colorSeperator.Enabled = false;
            lblStartColour.Enabled = false;
            colorStart.Enabled = false;
            lblEndColour.Enabled = false;
            colorEnd.Enabled = false;
            if (ReadOnly)
                return;
            isSelecting = true;

            //colorStart.BackColor = Color.Black;
            //colorEnd.BackColor = Color.Black;

            GraphNode FirstNode = myView.Selection.First as GraphNode;
            if (FirstNode != null)
            {
                foreach (GoObject shp in FirstNode.GetEnumerator())
                {
                    if (shp is MetaBuilder.Graphing.Shapes.Behaviours.IBehaviourShape)
                    {
                        if (shp is GoPort) //QuickPorts are IBehaviourShapes and we dont want that
                            continue;
                        if (!(shp is GradientRoundedRectangle) && !(shp is GradientEllipse) && !(shp is GradientTrapezoid) && !(shp is GradientPolygon) && !(shp is GradientValueChainStep))
                            continue;

                        MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour gradObject = (shp as MetaBuilder.Graphing.Shapes.Behaviours.IBehaviourShape).Manager.GetExistingBehaviour(typeof(MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour)) as MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour;

                        if (gradObject != null)
                        {
                            colorSeperator.Enabled = true;
                            //cbColorType.Enabled = true;
                            cbColorType.SelectedItem = "Gradient";
                            cbGradientType.SelectedItem = gradObject.MyBrush.GradientType;
                            cbGradientType.Enabled = true;
                            lblStartColour.Enabled = true;
                            colorStart.Enabled = true;
                            colorStart.BackColor = gradObject.MyBrush.InnerColor;
                            //colorButton.BackColor = gradObject.MyBrush.InnerColor;
                            lblEndColour.Enabled = true;
                            colorEnd.Enabled = true;
                            colorEnd.BackColor = gradObject.MyBrush.OuterColor;

                            break;
                        }
                        else
                        {
                            colorSeperator.Enabled = true;
                            //cbColorType.Enabled = true;
                            cbColorType.SelectedItem = "Solid";

                            cbGradientType.Visible = false;
                            cbGradientType.Enabled = false;
                            lblStartColour.Enabled = true;
                            colorStart.Enabled = true;
                            colorEnd.Enabled = false;

                            break;
                        }
                    }
                }

                isSelecting = false;
                return;
            }
            SubgraphNode sgNode = myView.Selection.First as SubgraphNode;
            if (sgNode != null)
            {
                cbColorType.SelectedItem = "Solid";
                colorSeperator.Enabled = true;
                //cbColorType.Enabled = true;
                lblStartColour.Enabled = true;
                colorStart.Enabled = true;
                colorStart.BackColor = sgNode.BackgroundColor;
                cbGradientType.Enabled = false;
                colorEnd.Enabled = false;

                isSelecting = false;
                return;
            }
            MappingCell mappingCell = myView.Selection.First as MappingCell;
            if (mappingCell != null)
            {
                cbColorType.SelectedItem = "Solid";
                colorSeperator.Enabled = true;
                //cbColorType.Enabled = true;
                lblStartColour.Enabled = true;
                colorStart.Enabled = true;
                colorStart.BackColor = (mappingCell.HeaderRectangle.Brush as SolidBrush).Color;
                cbGradientType.Enabled = false;
                colorEnd.Enabled = false;

                isSelecting = false;
                return;
            }
            //toolStripMain.Enabled = currentMetaObjectCanBeEdited;
            GoLabeledLink link = myView.Selection.First as GoLabeledLink;
            if (link != null)
            {
                cbColorType.SelectedItem = "Solid";
                colorSeperator.Enabled = true;
                //cbColorType.Enabled = true;
                lblStartColour.Enabled = true;
                colorStart.Enabled = true;
                if (link is QLink)
                    colorStart.BackColor = (link as QLink).PenColorBeforeCompare;
                else
                    colorStart.BackColor = link.Pen.Color;

                cbGradientType.Enabled = false;
                colorEnd.Enabled = false;

                isSelecting = false;
                return;
            }

            Rationale rationale = myView.Selection.First as Rationale;
            if (rationale != null)
            {
                cbColorType.SelectedItem = "Solid";
                colorSeperator.Enabled = true;
                //cbColorType.Enabled = true;
                lblStartColour.Enabled = true;
                colorStart.Enabled = true;
                if (rationale.Background is GoShape)
                {
                    colorStart.BackColor = ((rationale.Background as GoShape).Brush as SolidBrush).Color;//new SolidBrush(Color.FromArgb(253, 226, 173));
                }

                cbGradientType.Enabled = false;
                colorEnd.Enabled = false;

                isSelecting = false;
                return;
            }

            MetaBuilder.Graphing.Shapes.Behaviours.IBehaviourShape behaviourShape = myView.Selection.First as MetaBuilder.Graphing.Shapes.Behaviours.IBehaviourShape;
            if (behaviourShape != null)
            {
                MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour gradObject = behaviourShape.Manager.GetExistingBehaviour(typeof(MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour)) as MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour;

                if (gradObject != null)
                {
                    colorSeperator.Enabled = true;
                    //cbColorType.Enabled = true;
                    cbColorType.SelectedItem = "Gradient";
                    cbGradientType.SelectedItem = gradObject.MyBrush.GradientType;
                    cbGradientType.Enabled = true;
                    lblStartColour.Enabled = true;
                    colorStart.Enabled = true;
                    colorStart.BackColor = gradObject.MyBrush.InnerColor;
                    //colorButton.BackColor = gradObject.MyBrush.InnerColor;
                    lblEndColour.Enabled = true;
                    colorEnd.Enabled = true;
                    colorEnd.BackColor = gradObject.MyBrush.OuterColor;
                }
                else
                {
                    colorSeperator.Enabled = true;
                    //cbColorType.Enabled = true;
                    cbColorType.SelectedItem = "Solid";

                    if (myView.Selection.First is GoStroke)
                    {
                        colorStart.BackColor = (myView.Selection.First as GoStroke).Pen.Color;
                    }

                    cbGradientType.Visible = false;
                    cbGradientType.Enabled = false;
                    lblStartColour.Enabled = true;
                    colorStart.Enabled = true;
                    colorEnd.Enabled = false;
                }

                isSelecting = false;
                return;
            }

            GoShape shape = myView.Selection.First as GoShape;
            if (shape != null)
            {
                if (shape is FishRealLink || shape is PathGradientRoundedRectangle)
                {
                    isSelecting = false;
                    return;
                }
                cbColorType.SelectedItem = "Solid";
                colorSeperator.Enabled = true;
                //cbColorType.Enabled = true;
                lblStartColour.Enabled = true;
                colorStart.Enabled = true;

                colorStart.BackColor = (shape.Brush as SolidBrush).Color;

                cbGradientType.Enabled = false;
                colorEnd.Enabled = false;

                isSelecting = false;
                return;
            }

            //Reset quick format settings
            colorStart.BackColor = Color.Black;
            colorEnd.BackColor = Color.Black;
            btnFontColour.BackColor = Color.Black;
            cbBold.Checked = false;
            cbItalic.Checked = false;
            cbUnderline.Checked = false;
            cbFontFamily.SelectedItem = "Microsoft Sans Serif";
            cbFontSize.SelectedItem = "10";
            cbGradientType.SelectedItem = "Horizontal";
            isSelecting = false;
        }

        public bool isSelecting;
        private object formatSender = null;
        private void formatControlChanged(object sender, EventArgs e)
        {
            if (isSelecting)
                return;
            formatSender = sender;
            applyFormattingToSelection();
            formatSender = null;
        }
        private void colorPickerButton_Clicked(object sender, EventArgs e)
        {
            //ColorDialog cd = new ColorDialog();
            //cd.SolidColorOnly = true;
            //cd.Color = (sender as ToolStripButton).BackColor;
            //cd.FullOpen = true;
            //if (cd.ShowDialog(DockingForm.DockForm) == DialogResult.OK)
            //{
            //    (sender as ToolStripButton).BackColor = cd.Color;
            //}
            DockingForm.DockForm.ShowColorDialog(sender);
        }

        private void applyFormattingToSelection()
        {
            myView.StartTransaction();
            try
            {
                if (myView.Selection.Count > 0)
                {
                    foreach (GoObject o in myView.Selection)
                    {
                        if (o is GraphNode)
                        {
                            GraphNode gNode = o as GraphNode;
                            foreach (GoObject shpI in gNode.GetEnumerator())
                            {
                                if (shpI is GoText)
                                {
                                    if (shpI is BoundLabel)
                                    {
                                        if ((shpI as BoundLabel).Name.Contains("_") || (shpI as BoundLabel).ddf > 0) //Skip invalid text items
                                            continue;

                                        SetTextFormat(shpI as GoText);
                                    }
                                    else
                                    {
                                        if ((shpI as GoText).Editable)
                                            SetTextFormat(shpI as GoText);
                                    }
                                }

                                GoObject shp = null;
                                if (shpI is CollapsingRecordNodeItemList)
                                {
                                    foreach (GoObject oi in shpI as GoGroup)
                                    {
                                        if (oi is MetaBuilder.Graphing.Shapes.Behaviours.IBehaviourShape)
                                        {
                                            shp = oi;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    shp = shpI;
                                }
                                if (formatSender == colorStart || formatSender == colorEnd) //can be moved after correct shp checks instead of duplicating checks
                                {
                                    if (shp != null && shp is MetaBuilder.Graphing.Shapes.Behaviours.IBehaviourShape)
                                    {
                                        if (shp is GoPort) //QuickPorts are IBehaviourShapes and we dont want that
                                            continue;
                                        //conditional is a gradientpolygon
                                        if (!(shp is GradientRoundedRectangle) && !(shp is GradientEllipse) && !(shp is GradientTrapezoid) && !(shp is GradientPolygon) && !(shp is GradientValueChainStep))
                                            continue;

                                        MetaBuilder.Graphing.Shapes.Behaviours.IBehaviourShape ibshape = (shp as MetaBuilder.Graphing.Shapes.Behaviours.IBehaviourShape);
                                        MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour newBehaviour = new MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour();

                                        newBehaviour.MyBrush = new MetaBuilder.Graphing.Formatting.ShapeGradientBrush();
                                        newBehaviour.MyBrush.GradientType = ibshape.Manager.Behaviours.Count > 0 ? (ibshape.Manager.Behaviours[0] as MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour).MyBrush.GradientType : MetaBuilder.Graphing.Formatting.GradientType.Horizontal;
                                        newBehaviour.MyBrush.InnerColor = ibshape.Manager.Behaviours.Count > 0 ? (ibshape.Manager.Behaviours[0] as MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour).MyBrush.InnerColor : Color.LightBlue;
                                        newBehaviour.MyBrush.OuterColor = ibshape.Manager.Behaviours.Count > 0 ? (ibshape.Manager.Behaviours[0] as MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour).MyBrush.OuterColor : Color.White;

                                        if (formatSender == colorStart)
                                            newBehaviour.MyBrush.InnerColor = colorStart.BackColor;
                                        if (formatSender == colorEnd)
                                            newBehaviour.MyBrush.OuterColor = colorEnd.BackColor;
                                        //Black printing shapes when transparent
                                        if (colorStart.BackColor == Color.Transparent)
                                        {
                                            newBehaviour.MyBrush.InnerColor = Color.White;
                                            newBehaviour.MyBrush.OuterColor = Color.White;
                                        }
                                        if (colorEnd.BackColor == Color.Transparent)
                                        {
                                            newBehaviour.MyBrush.OuterColor = Color.White;
                                        }
                                        ibshape.Manager = new MetaBuilder.Graphing.Shapes.Behaviours.BaseShapeManager();
                                        ibshape.Manager.AddBehaviour(newBehaviour);
                                        newBehaviour.Owner = shp;
                                        newBehaviour.Update(shp);
                                        CustomModified = true;

                                        if (defaultHeatMapNodeColour != null)
                                            if (defaultHeatMapNodeColour.ContainsKey(gNode as IMetaNode))
                                                defaultHeatMapNodeColour[gNode as IMetaNode] = newBehaviour.MyBrush.InnerColor;
                                    }
                                }
                                else if (formatSender == cbGradientType)
                                {
                                    if (shp != null && shp is MetaBuilder.Graphing.Shapes.Behaviours.IBehaviourShape)
                                    {
                                        if (shp is GoPort) //QuickPorts are IBehaviourShapes and we dont want that
                                            continue;
                                        //conditional is a gradientpolygon
                                        if (!(shp is GradientRoundedRectangle) && !(shp is GradientEllipse) && !(shp is GradientTrapezoid) && !(shp is GradientPolygon) && !(shp is GradientValueChainStep))
                                            continue;

                                        MetaBuilder.Graphing.Shapes.Behaviours.IBehaviourShape ibshape = (shp as MetaBuilder.Graphing.Shapes.Behaviours.IBehaviourShape);
                                        MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour newBehaviour = new MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour();

                                        newBehaviour.MyBrush = new MetaBuilder.Graphing.Formatting.ShapeGradientBrush();
                                        newBehaviour.MyBrush.GradientType = (MetaBuilder.Graphing.Formatting.GradientType)Enum.Parse(typeof(MetaBuilder.Graphing.Formatting.GradientType), cbGradientType.SelectedItem.ToString());

                                        newBehaviour.MyBrush.InnerColor = ibshape.Manager.Behaviours.Count > 0 ? (ibshape.Manager.Behaviours[0] as MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour).MyBrush.InnerColor : Color.LightBlue;
                                        newBehaviour.MyBrush.OuterColor = ibshape.Manager.Behaviours.Count > 0 ? (ibshape.Manager.Behaviours[0] as MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour).MyBrush.OuterColor : Color.White;

                                        ibshape.Manager = new MetaBuilder.Graphing.Shapes.Behaviours.BaseShapeManager();
                                        ibshape.Manager.AddBehaviour(newBehaviour);
                                        newBehaviour.Owner = shp;
                                        newBehaviour.Update(shp);
                                        CustomModified = true;
                                    }
                                }
                            }
                            //shallow copy colour
                            DockingForm.DockForm.updateNodeFormattingForAllNodes(gNode);
                        }
                        else if (o is SubgraphNode)
                        {
                            if ((o as SubgraphNode).Label != null)
                                SetTextFormat((o as SubgraphNode).Label);

                            if (formatSender == colorStart)
                            {
                                if (colorStart.BackColor == Color.Transparent)
                                    (o as SubgraphNode).BackgroundColor = Color.White;
                                else
                                    (o as SubgraphNode).BackgroundColor = colorStart.BackColor;

                                CustomModified = true;
                                if (defaultHeatMapNodeColour != null)
                                    if (defaultHeatMapNodeColour.ContainsKey(o as IMetaNode))
                                        defaultHeatMapNodeColour[o as IMetaNode] = (o as SubgraphNode).BackgroundColor;
                            }
                        }
                        else if (o is MappingCell)
                        {
                            if ((o as MappingCell).Label != null)
                                SetTextFormat((o as MappingCell).Label);

                            if (formatSender == colorStart)
                            {
                                if (colorStart.BackColor == Color.Transparent)
                                    (o as MappingCell).HeaderRectangle.Brush = Brushes.White;//new SolidBrush(Color.White);
                                else
                                    (o as MappingCell).HeaderRectangle.Brush = new SolidBrush(colorStart.BackColor);

                                CustomModified = true;
                                if (defaultHeatMapNodeColour != null)
                                    if (defaultHeatMapNodeColour.ContainsKey(o as IMetaNode))
                                        defaultHeatMapNodeColour[o as IMetaNode] = ((o as MappingCell).HeaderRectangle.Brush as SolidBrush).Color;
                            }
                        }
                        else if (o is CollapsingRecordNodeItem)
                        {
                            if ((o as CollapsingRecordNodeItem).GetLabel != null)
                                SetTextFormat((o as CollapsingRecordNodeItem).GetLabel);
                        }
                        else if (o is GoNode)
                        {
                            if ((o as GoNode).Label != null)
                                SetTextFormat((o as GoNode).Label);
                        }
                        else if (o is GoLabeledLink)
                        {
                            if (o is FishLink || o is FishRealLink)
                                continue;

                            GoLabeledLink link = o as GoLabeledLink;
                            Pen oldLabelPen = link.Pen;
                            if (formatSender == colorStart)
                                if (colorStart.BackColor == Color.Transparent)
                                {
                                    link.RealLink.Pen = new Pen(new SolidBrush(Color.White));
                                    link.RealLink.Pen.DashStyle = oldLabelPen.DashStyle;
                                    link.RealLink.Pen.Width = oldLabelPen.Width;
                                    //(o as GoLabeledLink).RealLink.Brush = new SolidBrush(colorButton.BackColor);
                                    if (o is QLink)
                                        (o as QLink).PenColorBeforeCompare = Color.White;
                                }
                                else
                                {
                                    link.RealLink.Pen = new Pen(new SolidBrush(colorStart.BackColor));
                                    link.RealLink.Pen.DashStyle = oldLabelPen.DashStyle;
                                    link.RealLink.Pen.Width = oldLabelPen.Width;
                                    //(o as GoLabeledLink).RealLink.Brush = new SolidBrush(colorButton.BackColor);
                                    if (o is QLink)
                                        (o as QLink).PenColorBeforeCompare = colorStart.BackColor;
                                }
                            //oldLabelPen.Dispose();
                        }
                        else if (o is Rationale)
                        {
                            //if (o.GetType() != myView.Selection.Primary.GetType())
                            //    continue;
                            Rationale rationale = o as Rationale;
                            if (rationale != null)
                            {
                                if (rationale.Background is GoShape)
                                {
                                    if (formatSender == colorStart)
                                    {
                                        (rationale.Background as GoShape).Brush = new SolidBrush(colorStart.BackColor);

                                        CustomModified = true;
                                        if (defaultHeatMapNodeColour != null)
                                            if (defaultHeatMapNodeColour.ContainsKey(rationale as IMetaNode))
                                                defaultHeatMapNodeColour[rationale as IMetaNode] = ((rationale.Background as GoShape).Brush as SolidBrush).Color;
                                    }
                                }

                                SetTextFormat(rationale.Label);
                            }
                        }
                        else if (o is GoStroke)
                        {
                            if (formatSender == colorStart)
                                (o as GoStroke).Pen = new Pen(colorStart.BackColor, (o as GoStroke).Pen.Width);
                        }
                        else if (o is MetaBuilder.Graphing.Shapes.Behaviours.IBehaviourShape)
                        {
                            if (formatSender == colorStart || formatSender == colorEnd) //can be moved after correct shp checks instead of duplicating checks
                            {
                                MetaBuilder.Graphing.Shapes.Behaviours.IBehaviourShape ibshape = (o as MetaBuilder.Graphing.Shapes.Behaviours.IBehaviourShape);
                                MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour newBehaviour = new MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour();

                                newBehaviour.MyBrush = new MetaBuilder.Graphing.Formatting.ShapeGradientBrush();
                                newBehaviour.MyBrush.GradientType = ibshape.Manager.Behaviours.Count > 0 ? (ibshape.Manager.Behaviours[0] as MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour).MyBrush.GradientType : MetaBuilder.Graphing.Formatting.GradientType.Horizontal;
                                newBehaviour.MyBrush.InnerColor = ibshape.Manager.Behaviours.Count > 0 ? (ibshape.Manager.Behaviours[0] as MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour).MyBrush.InnerColor : Color.LightBlue;
                                newBehaviour.MyBrush.OuterColor = ibshape.Manager.Behaviours.Count > 0 ? (ibshape.Manager.Behaviours[0] as MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour).MyBrush.OuterColor : Color.White;

                                if (formatSender == colorStart)
                                    newBehaviour.MyBrush.InnerColor = colorStart.BackColor;
                                if (formatSender == colorEnd)
                                    newBehaviour.MyBrush.OuterColor = colorEnd.BackColor;
                                //Black printing shapes when transparent
                                if (colorStart.BackColor == Color.Transparent)
                                {
                                    newBehaviour.MyBrush.InnerColor = Color.White;
                                    newBehaviour.MyBrush.OuterColor = Color.White;
                                }
                                if (colorEnd.BackColor == Color.Transparent)
                                {
                                    newBehaviour.MyBrush.OuterColor = Color.White;
                                }
                                ibshape.Manager = new MetaBuilder.Graphing.Shapes.Behaviours.BaseShapeManager();
                                ibshape.Manager.AddBehaviour(newBehaviour);
                                newBehaviour.Owner = o;
                                newBehaviour.Update(o);
                                CustomModified = true;
                            }
                            else if (formatSender == cbGradientType)
                            {
                                MetaBuilder.Graphing.Shapes.Behaviours.IBehaviourShape ibshape = (o as MetaBuilder.Graphing.Shapes.Behaviours.IBehaviourShape);
                                MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour newBehaviour = new MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour();

                                newBehaviour.MyBrush = new MetaBuilder.Graphing.Formatting.ShapeGradientBrush();
                                newBehaviour.MyBrush.GradientType = (MetaBuilder.Graphing.Formatting.GradientType)Enum.Parse(typeof(MetaBuilder.Graphing.Formatting.GradientType), cbGradientType.SelectedItem.ToString());

                                newBehaviour.MyBrush.InnerColor = ibshape.Manager.Behaviours.Count > 0 ? (ibshape.Manager.Behaviours[0] as MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour).MyBrush.InnerColor : Color.LightBlue;
                                newBehaviour.MyBrush.OuterColor = ibshape.Manager.Behaviours.Count > 0 ? (ibshape.Manager.Behaviours[0] as MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour).MyBrush.OuterColor : Color.White;

                                ibshape.Manager = new MetaBuilder.Graphing.Shapes.Behaviours.BaseShapeManager();
                                ibshape.Manager.AddBehaviour(newBehaviour);
                                newBehaviour.Owner = o;
                                newBehaviour.Update(o);
                                CustomModified = true;
                            }
                        }
                        else if (o is FishLink || o is FishRealLink || o is FrameLayerGroup || o is GoSheet || o is FrameLayerRect)
                        {
                            o.ToString();
                        }
                        else if (o is GoShape)
                        {
                            if (formatSender == colorStart)
                            {
                                if (colorStart.BackColor == Color.Transparent)
                                    (o as GoShape).Brush = Brushes.White;//new SolidBrush(Color.White);
                                else
                                    (o as GoShape).Brush = new SolidBrush(colorStart.BackColor);
                                CustomModified = true;
                            }
                        }
                        else
                        {
                            if (o.ParentNode is ResizableBalloonComment || o.ParentNode is ResizableComment)
                                continue;

                            if (o is GoText)
                            {
                                //if ((o as GoText).Editable)
                                SetTextFormat(o as GoText);
                            }
                            else
                            {
                                if (o is GoGroup)
                                    foreach (GoObject obj in o as GoGroup)
                                        if (obj is GoText)
                                            //if ((obj as GoText).Editable)
                                            SetTextFormat(obj as GoText);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString()); ;
            }

            myView.FinishTransaction("Quick Format");
        }
        private void SetTextFormat(GoText label)
        {
            if (label.TopLevelObject is IMetaNode && !(label.TopLevelObject is ImageNode))
            {
                if ((label.TopLevelObject as IMetaNode).MetaObject.Class == "Implication")
                {
                    if (label.Width <= label.TopLevelObject.Width / 2)
                    {
                        return;
                    }
                }
            }

            if (formatSender == cbFontSize)
                if (cbFontSize.SelectedItem != null)
                    label.FontSize = float.Parse(cbFontSize.SelectedItem.ToString(), System.Globalization.CultureInfo.InvariantCulture);

            if (formatSender == cbFontFamily)
                if (cbFontFamily.SelectedItem != null)
                    label.FamilyName = cbFontFamily.SelectedItem.ToString();

            if (formatSender == cbBold)
                label.Bold = cbBold.Checked;

            if (formatSender == cbItalic)
                label.Italic = cbItalic.Checked;

            if (formatSender == cbUnderline)
                label.Underline = cbUnderline.Checked;

            if (formatSender == btnFontColour)
                label.TextColor = btnFontColour.BackColor;
        }

        #endregion

        private void fixShapesButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(DockingForm.DockForm, "Please note that this feature does not relink shapes after it replaces them. Context will still exist but visually it will be incorrect." + Environment.NewLine + "Click YES to proceed.", "Shape Replacer", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            if (myView.Selection.Count > 0)
                fixBrokenShapesOnDiagram(myView.Selection);
            else
                fixBrokenShapesOnDiagram(null);

            myView.Selection.Clear();
        }
        private void fixBrokenShapesOnDiagram(GoCollection col) //Now what about LINKS AND ARTEFACTS
        {
            if (col == null)
            {
                myView.SelectAll();
                col = myView.Selection;
            }
            List<CollapsibleNode> removeNodes = new List<CollapsibleNode>();
            GoCollection refreshNodes = new GoCollection();
            CollapsibleNode newNode;
            foreach (GoObject o in col)
            {
                newNode = null;
                if (o is CollapsibleNode)
                {
                    CollapsibleNode node = o as CollapsibleNode;
                    if (node.MetaObject == null) continue;

                    newNode = (CollapsibleNode)(Variables.Instance.ReturnShape(node.MetaObject.Class) as CollapsibleNode).Copy();
                    if (newNode == null) continue;

                    newNode.HookupEvents();
                    newNode.MetaObject = node.MetaObject;
                    newNode.BindToMetaObjectProperties();

                    newNode.Location = node.Location;

                    if (node.Parent is SubgraphNode)
                        (node.Parent as SubgraphNode).Add(newNode);
                    else
                        myView.Document.Add(newNode);

                    refreshNodes.Add(newNode);
                    removeNodes.Add(node);
                    node.Visible = false;
                }
                else if (o is SubgraphNode)
                {
                    SubgraphNode subgNode = o as SubgraphNode;
                    SubgraphNode exSubNode;
                    foreach (CollapsibleNode cNode in getComplexNodesInSubGraphNode(subgNode))
                    {
                        exSubNode = cNode.Parent as SubgraphNode;

                        if (cNode.MetaObject == null) continue;

                        newNode = (CollapsibleNode)(Variables.Instance.ReturnShape(cNode.MetaObject.Class) as CollapsibleNode).Copy();
                        if (newNode == null) continue;

                        newNode.HookupEvents();
                        newNode.MetaObject = cNode.MetaObject;
                        newNode.BindToMetaObjectProperties();
                        newNode.Location = cNode.Location;

                        exSubNode.Add(newNode);
                        refreshNodes.Add(newNode);

                        cNode.Visible = false;
                        removeNodes.Add(cNode);
                    }
                }
            }

            foreach (CollapsibleNode cNode in removeNodes)
            {
                if (cNode.Parent is ILinkedContainer)
                {
                    if (cNode.Parent is SubgraphNode)
                    {
                        (cNode.Parent as SubgraphNode).SkipRelationShipRemove = true;
                    }
                    (cNode.Parent as ILinkedContainer).Remove(cNode);
                    if (cNode.Parent is SubgraphNode)
                    {
                        (cNode.Parent as SubgraphNode).SkipRelationShipRemove = false;
                    }
                }
                myView.Document.Remove(cNode);
            }
            RefreshData(refreshNodes, false);
        }

        private List<CollapsibleNode> getComplexNodesInSubGraphNode(SubgraphNode subgraphNode)
        {
            List<CollapsibleNode> returnNodes = new List<CollapsibleNode>();
            foreach (GoObject o in subgraphNode.GetEnumerator())
            {
                if (o is CollapsibleNode)
                    returnNodes.Add(o as CollapsibleNode);
                else if (o is SubgraphNode)
                {
                    returnNodes.AddRange(getComplexNodesInSubGraphNode(o as SubgraphNode));
                }
            }

            return returnNodes;
        }

        //crops the current document to its contents and resizes the frame and zooms to fit and updates documents bounds
        public void cropGlobal()
        {
            float x = 0;
            //float nodeWidth = 0;
            float y = 0;
            //float nodeHeight = 0;
            foreach (GoObject o in myView.Document)
            {
                if (!(o is IMetaNode))
                    continue;
                //if (!(o is MappingCell) && !(o is SubgraphNode) && !(o is ValueChain) && !(o is Rationale) && !(o is GoNode))
                //    continue;

                if ((o.Position.X + o.Width) > x)
                {
                    x = o.Position.X + o.Width;
                    //nodeWidth = o.Width;
                }
                if ((o.Position.Y + o.Height) > y)
                {
                    y = o.Position.Y + o.Height;
                    //nodeHeight = o.Height;
                }
            }

            myView.Sheet.Width = x + 50;
            myView.Sheet.Height = y + 50;
            myView.ViewController.UpdateSize(MetaBuilder.Graphing.Controllers.GraphViewController.CropType.ToSheet);

            myView.Document.Bounds = myView.Sheet.Bounds;
            myView.UpdateRulers();

            ZoomController c = new ZoomController(myView);
            c.ZoomToFit();
            c = null;
        }

        public PaletteContainer DiagramTypePallette = null;
        public PaletteContainer SelectedPallette = null;
        public void SetupDiagramType()
        {
            NormalDiagram gvContainerDiagram = (View.Document as NormalDiagram);
            if (!string.IsNullOrEmpty(gvContainerDiagram.DiagramType))
            {
                DockingForm.DockForm.m_paletteDocker.toolStripMain.Visible = false;
                if (DiagramTypePallette == null)
                    DiagramTypePallette = DockingForm.DockForm.m_paletteDocker.New(gvContainerDiagram.DiagramType);

                foreach (DockContent palletteContainer in DockingForm.DockForm.m_paletteDocker.dockPanel1.Contents)
                {
                    if (!(palletteContainer is PaletteContainer))
                        continue;
                    if (palletteContainer != DiagramTypePallette)
                        palletteContainer.Hide();
                    else
                        palletteContainer.Show(DockingForm.DockForm.m_paletteDocker.dockPanel1);
                }
                //DiagramTypePallette.Activate();
            }
            else
            {
                DockingForm.DockForm.m_paletteDocker.toolStripMain.Visible = true;
                foreach (DockContent palletteContainer in DockingForm.DockForm.m_paletteDocker.dockPanel1.Contents)
                {
                    if (!(palletteContainer is PaletteContainer))
                        continue;
                    if ((palletteContainer as PaletteContainer).Classes.Count > 0)
                        palletteContainer.Hide();
                    else
                        palletteContainer.Show(DockingForm.DockForm.m_paletteDocker.dockPanel1);
                }
                //if (SelectedPallette != null)
                //{
                //    SelectedPallette.Activate();
                //}
            }
            this.Select(true, false);
            this.Focus();
        }

        private Tools.HeatMap.HeatMapMeasure measure = null;
        private void menuItemToolsHeatMapMeasure_Click(object sender, System.EventArgs e)
        {
            try
            {
                foreach (DockContent c in this.DockPanel.Contents)
                {
                    if (measure != null && c.GetType() == measure.GetType())
                    {
                        measure = c as Tools.HeatMap.HeatMapMeasure;
                    }
                }

                if (measure == null || measure.IsDisposed)
                {
                    measure = new Tools.HeatMap.HeatMapMeasure("MeasureType");
                }
                measure.Show(this.DockPanel, DockState.DockBottom);
            }
            catch
            {
                measure = null;
            }
        }
        private void menuItemToolsHeatMapGapType_Click(object sender, System.EventArgs e)
        {
            myView.StartTransaction();

            if (myView.Selection.Count == 0)
            {
                foreach (IMetaNode node in ViewController.GetIMetaNodes())
                {
                    applyHeatMap(node);
                }
                foreach (GoObject o in myView.Document)
                {
                    if (o is QLink)
                    {
                        applyHeatMap(o as QLink);
                    }
                }
                myView.FinishTransaction("Heat Map Document");
            }
            else
            {
                foreach (GoObject o in myView.Selection)
                {
                    if (o is IMetaNode)
                    {
                        applyHeatMap(o as IMetaNode);
                    }
                    else if (o is QLink)
                    {
                        applyHeatMap(o as QLink);
                    }
                }

                foreach (IMetaNode node in ViewController.GetIMetaNodes(myView.Selection))
                {
                    applyHeatMap(node);
                }

                myView.FinishTransaction("Heat Map Selection");
            }
        }
        private Dictionary<IMetaNode, Color> defaultHeatMapNodeColour;
        private void applyHeatMap(IMetaNode node)
        {
            if (defaultHeatMapNodeColour == null)
                defaultHeatMapNodeColour = new Dictionary<IMetaNode, Color>();
            if (!defaultHeatMapNodeColour.ContainsKey(node))
            {
                defaultHeatMapNodeColour.Add(node, GetStartColour(node));
            }
            Color c = defaultHeatMapNodeColour[node];
            object gap = node.MetaObject.Get("GapType");

            if (node is CollapsingRecordNodeItem)
            {
                Color cx = Color.Black;
                if (gap != null)
                    if (gap.ToString().ToLower() == "add")
                        cx = Color.Red;
                    else if (gap.ToString().ToLower() == "reuse")
                        cx = Color.Green;
                    else if (gap.ToString().ToLower() == "change")
                        cx = Color.Orange;
                if ((node as CollapsingRecordNodeItem).GetLabel != null)
                    (node as CollapsingRecordNodeItem).GetLabel.TextColor = cx;
                return;
            }

            if (node is ImageNode)
            {
                Color cx = Color.Black;
                if (gap != null)
                    if (gap.ToString().ToLower() == "add")
                        cx = Color.Red;
                    else if (gap.ToString().ToLower() == "reuse")
                        cx = Color.Green;
                    else if (gap.ToString().ToLower() == "change")
                        cx = Color.Orange;
                if ((node as ImageNode).Label != null)
                    (node as ImageNode).Label.TextColor = cx;
                return;
            }

            if (gap == null || gap.ToString().Length == 0)
            {
                if (c == Color.Red || c == Color.Green || c == Color.Orange)
                {
                    c = Color.White;
                }
                ApplyColorToNode(node, c);
                return;
            }

            if (gap.ToString().ToLower() == "add")
                c = Color.Red;
            else if (gap.ToString().ToLower() == "reuse")
                c = Color.Green;
            else if (gap.ToString().ToLower() == "change")
                c = Color.Orange;
            else
            {
                if (c == Color.Red || c == Color.Green || c == Color.Orange)
                {
                    c = Color.White;
                }
            }

            ApplyColorToNode(node, c);
        }
        private void applyHeatMap(QLink link)
        {
            object gap = link.GapType.ToString();
            if (gap == null || gap.ToString().Length > 0)
            {
                ApplyColorToLink(link, Color.Black);
                return;
            }

            Color c = Color.Black;
            if (gap.ToString().ToLower() == "add")
                c = Color.Red;
            else if (gap.ToString().ToLower() == "reuse")
                c = Color.Green;
            else if (gap.ToString().ToLower() == "change")
                c = Color.Orange;
            //else
            //    return;

            ApplyColorToLink(link, c);
        }
        private Color GetStartColour(IMetaNode node)
        {
            if (node is SubgraphNode)
            {
                return (node as SubgraphNode).BackgroundColor;
            }
            else if (node is MappingCell)
            {
                return ((node as MappingCell).HeaderRectangle.Brush as SolidBrush).Color;
            }
            else if (node is GraphNode)
            {
                foreach (GoObject shp in (node as GraphNode).GetEnumerator())
                {
                    if (shp is MetaBuilder.Graphing.Shapes.Behaviours.IBehaviourShape)
                    {
                        if (shp is GoPort) //QuickPorts are IBehaviourShapes and we dont want that
                            continue;
                        if (!(shp is GradientRoundedRectangle) && !(shp is GradientEllipse) && !(shp is GradientTrapezoid) && !(shp is GradientPolygon) && !(shp is GradientValueChainStep))
                            continue;

                        MetaBuilder.Graphing.Shapes.Behaviours.IBehaviourShape ibshape = (shp as MetaBuilder.Graphing.Shapes.Behaviours.IBehaviourShape);
                        return ibshape.Manager.Behaviours.Count > 0 ? (ibshape.Manager.Behaviours[0] as MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour).MyBrush.InnerColor : Color.White;
                    }
                }
            }

            return Color.White;
        }

        //Gradient Brush with this colour as centre and start and stop based on gaps of parent Imetanode Gaptypes?
        public void ApplyColorToLink(QLink link, Color c)
        {
            link.PenColorBeforeCompare = c;
            System.Drawing.Drawing2D.DashStyle ds = link.Pen.DashStyle;
            link.Pen = new Pen(c);
            link.Pen.DashStyle = ds;
        }
        public void ApplyColorToNode(IMetaNode node, Color c)
        {
            if (node is SubgraphNode)
            {
                (node as SubgraphNode).BackgroundColor = c;
            }
            else if (node is MappingCell)
            {
                (node as MappingCell).HeaderRectangle.Brush = new SolidBrush(c);
            }
            else if (node is GraphNode)
            {
                foreach (GoObject shp in (node as GraphNode).GetEnumerator())
                {
                    if (shp is MetaBuilder.Graphing.Shapes.Behaviours.IBehaviourShape)
                    {
                        if (shp is GoPort) //QuickPorts are IBehaviourShapes and we dont want that
                            continue;
                        //conditional is a gradientpolygon
                        if (!(shp is GradientRoundedRectangle) && !(shp is GradientEllipse) && !(shp is GradientTrapezoid) && !(shp is GradientPolygon) && !(shp is GradientValueChainStep))
                            continue;

                        MetaBuilder.Graphing.Shapes.Behaviours.IBehaviourShape ibshape = (shp as MetaBuilder.Graphing.Shapes.Behaviours.IBehaviourShape);
                        MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour newBehaviour = new MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour();

                        newBehaviour.MyBrush = new MetaBuilder.Graphing.Formatting.ShapeGradientBrush();

                        newBehaviour.MyBrush.GradientType = ibshape.Manager.Behaviours.Count > 0 ? (ibshape.Manager.Behaviours[0] as MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour).MyBrush.GradientType : MetaBuilder.Graphing.Formatting.GradientType.Horizontal;
                        //newBehaviour.MyBrush.InnerColor = ibshape.Manager.Behaviours.Count > 0 ? (ibshape.Manager.Behaviours[0] as MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour).MyBrush.InnerColor : Color.LightBlue;
                        newBehaviour.MyBrush.OuterColor = ibshape.Manager.Behaviours.Count > 0 ? (ibshape.Manager.Behaviours[0] as MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour).MyBrush.OuterColor : Color.White;

                        //newBehaviour.MyBrush.InnerColor = (ibshape.Manager.Behaviours[0] as MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour).MyBrush.InnerColor;
                        //newBehaviour.MyBrush.OuterColor = (ibshape.Manager.Behaviours[0] as MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour).MyBrush.OuterColor;
                        //newBehaviour.MyBrush.OuterColor = c;
                        newBehaviour.MyBrush.InnerColor = c;
                        //Black printing shapes when transparent

                        ibshape.Manager = new MetaBuilder.Graphing.Shapes.Behaviours.BaseShapeManager();
                        ibshape.Manager.AddBehaviour(newBehaviour);
                        newBehaviour.Owner = shp;
                        newBehaviour.Update(shp);
                    }
                }
            }
            else
            {
                node.ToString();
            }
        }

        public void CreateInvalidMetaModelSubgraphChildTask(IMetaNode o)
        {
            NotInModelLinkTask pTask = new NotInModelLinkTask();
            pTask.Tag = o;
            pTask.MyView = myView;
            pTask.ContainerID = this.ContainerID.ToString();
            pTask.IsCritical = false;
            pTask.Caption = (o.MetaObject == null || o.MetaObject.ToString() == null || o.MetaObject.ToString().Length == 0) ? o.BindingInfo.BindingClass : o.MetaObject.ToString();
            DockingForm.DockForm.m_taskDocker.AddTask(this.ContainerID.ToString(), pTask);
            DockingForm.DockForm.m_taskDocker.BindToList(this.ContainerID.ToString());
        }

        private void CheckSpelling()
        {
#if DEBUG
            myView.StartTransaction();
            //find all the gotext
            Collection<GoObject> texts = GetTexts(new Collection<GoObject>());
            NetSpell.SpellChecker.Spelling spelling = new NetSpell.SpellChecker.Spelling();
            spelling.Dictionary = new NetSpell.SpellChecker.Dictionary.WordDictionary();
            spelling.Dictionary.DictionaryFolder = Application.StartupPath;
            spelling.Dictionary.DictionaryFile = "en-ZA.dic";
            spelling.ShowDialog = false;
            spelling.AlertComplete = false;
            spelling.SuggestionMode = NetSpell.SpellChecker.Spelling.SuggestionEnum.NearMiss;
            foreach (GoObject text in texts)
            {
                //{
                spelling.Suggest((text as GoText).Text);
                //spelling.SpellCheck((text as GoText).Text);
                if (spelling.Suggestions.Count > 0)
                {
                    (text as GoText).Text = spelling.Suggestions[0].ToString();
                }
                //}
            }
            myView.FinishTransaction("Auto Correct Selection");
#endif
        }
        private Collection<GoObject> GetTexts(Collection<GoObject> list)
        {
            foreach (IMetaNode node in ViewController.GetIMetaNodes())
            {
                if (!View.Selection.Contains(node as GoObject))
                    continue;

                node.BindingInfo.Bindings.ToString();
                foreach (GoObject o in node as GoGroup)
                {
                    if (o is BoundLabel || o is ExpandableTextBoxLabel)
                    {
                        if ((o as GoText).Text.Length > 0)
                        {
                            list.Add(o);
                            continue;
                        }
                    }
                    if (o is GoGroup)
                    {
                        foreach (GoObject nodex in o as GoGroup)
                        {
                            if (nodex is BoundLabel || nodex is ExpandableTextBoxLabel)
                            {
                                if ((nodex as GoText).Text.Length > 0)
                                {
                                    list.Add(nodex);
                                    continue;
                                }
                            }
                            if (nodex is GoGroup)
                            {
                                foreach (GoObject nodey in nodex as GoGroup)
                                {
                                    if (nodey is BoundLabel || nodey is ExpandableTextBoxLabel)
                                    {
                                        if ((nodey as GoText).Text.Length > 0)
                                        {
                                            list.Add(nodey);
                                            continue;
                                        }
                                    }
                                    if (nodey is GoGroup)
                                    {
                                        foreach (GoObject nodez in nodey as GoGroup)
                                        {
                                            if (nodez is BoundLabel || nodez is ExpandableTextBoxLabel)
                                            {
                                                if ((nodez as GoText).Text.Length > 0)
                                                {
                                                    list.Add(nodez);
                                                    continue;
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
            return list;
        }

        private bool closeAfterSave;
        public bool CloseAfterSave
        {
            get { return closeAfterSave; }
            set { closeAfterSave = value; }
        }

        protected override void OnClosed(EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
                return;

            if (!this.IsDisposed)
                CloseDispose();
            base.OnClosed(e);
        }

        private void CloseDispose()
        {
            try
            {
                if (allowedLinks != null)
                    allowedLinks.Clear();
                allowedLinks = null;

                View.Document.SkipsUndoManager = true;

                myView.ObjectContextClicked -= myView_ObjectContextClicked;

                this.myView.Click -= this.myView_Click;
                this.myView.NodeObjectContextClicked -= this.myView_NodeObjectContextClicked;
                this.myView.NodeObjectContextClickedShallow -= this.myView_NodeObjectContextClickedShallow;
                this.myView.ItemsPasted -= this.myView_ItemsPasted;

                myView.DragEnter -= myView_DragEnter;
                myView.DragOver -= myView_DragOver;
                myView.DocumentChanged -= myView_DocumentChanged;
                myView.ViewContextMenu.Collapse -= ViewContextMenu_Collapse;
                TimerAutoSave.Tick -= timerAutosave_Tick;

                //defaultportsbtn.Click =-(defaultportsbtn_Click);
                //fixShapesButton.Click -=(fixShapesButton_Click);
                portManipulatorButton.Click -= (portManipulatorButton_Click);
                cbGradientType.SelectedIndexChanged -= formatControlChanged;
                cbColorType.SelectedIndexChanged -= formatControlChanged;
                colorEnd.Click -= colorPickerButton_Clicked;
                colorEnd.BackColorChanged -= formatControlChanged;
                colorStart.Click -= colorPickerButton_Clicked;
                colorStart.BackColorChanged -= formatControlChanged;
                cbUnderline.CheckedChanged -= formatControlChanged;
                cbItalic.CheckedChanged -= formatControlChanged;
                cbBold.CheckedChanged -= formatControlChanged;
                btnFontColour.Click -= colorPickerButton_Clicked;
                btnFontColour.BackColorChanged -= formatControlChanged;
                cbFontSize.SelectedIndexChanged -= formatControlChanged;
                cbFontSize.TextChanged -= formatControlChanged;
                cbFontFamily.SelectedIndexChanged -= formatControlChanged;

                GoLayerCollectionObjectEnumerator enumerator = View.Document.GetEnumerator();
                //foreach (GoObject o in enumerator)
                while (enumerator.MoveNext())
                {
                    destroy(enumerator.Current);
                }

                toolStripFormat.Items.Clear();
                toolStripFormat.Dispose();
                toolStripFormat = null;

                loadfileWorker.Dispose();
                backgroundWorker1.Dispose();

                (View.Document as NormalDiagram).VersionManager = null;
                (View.Document as NormalDiagram).UndoManager = null;
                (View.Document as NormalDiagram).EmbeddedImages = null;
                (View.Document as NormalDiagram).DocumentFrame = null;

                zController = null;

                ViewController.UnHook();
                ViewController.MetaConvertComplete -= viewController_MetaConvertComplete;
                //ViewController.MyView = null;
                ViewController.IndicatorController = null;
                //ViewController = null;

                myView.ViewController = null;

                TimerAutoSave.Stop();
                TimerAutoSave.Dispose();

                DockingForm.DockForm.m_taskDocker.RemoveContainerList(ContainerID.ToString());

                //EventHandlerList xlist = (EventHandlerList)typeof(Control).GetProperty("Events", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(View, null);
                //typeof(EventHandlerList).GetMethod("Dispose").Invoke(xlist, null);

                //this.DockHandler.Dispose();
                //EventHandlerList list = (EventHandlerList)typeof(Control).GetProperty("Events", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(this, null);
                //typeof(EventHandlerList).GetMethod("Dispose").Invoke(list, null);

                if (View.Document.UndoManager != null)
                    View.Document.UndoManager.Clear();
                View.Document.Clear();
                //View.Document = null;
                //View.Dispose();

                Dispose();
                GC.Collect(2, GCCollectionMode.Forced);

                Close();
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex.ToString());
            }
        }

        private void destroy(GoObject o)
        {
            if (o == null)
                return;
            try
            {
                if (o is GoImage)
                {
                    (o as GoImage).Image = null;
                }

                if (o is GoGroup)
                {
                    foreach (GoObject obj in (o as GoGroup))
                    {
                        if (obj is MetaBuilder.Graphing.Shapes.Behaviours.IBehaviourShape)
                            (obj as MetaBuilder.Graphing.Shapes.Behaviours.IBehaviourShape).Manager = null;
                        destroy(obj);
                    }
                }

                if (o is IMetaNode)
                {
                    (o as IMetaNode).ContentsChanged -= node_ContentsChanged;
                    (o as IMetaNode).ContentsChanged -= node_ContentsChanged;
                    (o as IMetaNode).ContentsChanged -= node_ContentsChanged;
                    if (o is ImageNode)
                    {
                        (o as ImageNode).UnHookEvents();
                    }
                    else if (o is GraphNode)
                    {
                        (o as GraphNode).UnHookEvents();
                    }

                    (o as IMetaNode).MetaObject = null;

                    (o as IMetaNode).BindingInfo = null;
                    (o as IMetaNode).CopiedFrom = null;
                    (o as IMetaNode).ContentsChanged = null;
                }
                o.Remove();

                o = null;
            }
            catch (Exception ex)
            {
                Log.WriteLog("GraphViewContainer::destroy::" + o.ToString() + Environment.NewLine + ex.ToString());
            }
        }
    }
}