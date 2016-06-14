using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Text;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Helpers;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Behaviours;
using MetaBuilder.Graphing.Shapes.Behaviours.Observers;
using MetaBuilder.Graphing.Shapes.Nodes;
using MetaBuilder.Graphing.Shapes.Nodes.Containers;
using MetaBuilder.Graphing.Shapes.Primitives;
using MetaBuilder.Graphing.Tools;
using MetaBuilder.Meta;
using MetaBuilder.Meta.Editors;
using Northwoods.Go;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using ClassAssociationComparer = MetaBuilder.Graphing.Tools.ClassAssociationComparer;
using ObjectAssociation = MetaBuilder.BusinessLogic.ObjectAssociation;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
using System.Threading;
using System.Collections.ObjectModel;

namespace MetaBuilder.Graphing.Controllers
{
    public class GraphViewController
    {

        //private bool isViewer;
        //public bool IsViewer
        //{
        //    get { return isViewer; }
        //    set { isViewer = value; }
        //}

        public event EventHandler MetaConvertComplete;
        private void OnMetaConvertComplete(object sender, EventArgs e)
        {
            if (MetaConvertComplete != null)
            {
                MetaConvertComplete(sender, e);
            }
        }

        #region Fields (1)

        [field: NonSerialized]
        private GraphView myView;
        private bool readO;
        public bool ReadOnly
        {
            get { return readO; }
            set { readO = value; }
        }

        private bool artefactPointersVisible;
        public bool ArtefactPointersVisible
        {
            get { return artefactPointersVisible; }
            set { artefactPointersVisible = value; }
        }

        #endregion Fields

        #region Properties (1)

        /// <summary>
        /// Gets or sets myView.
        /// </summary>
        /// <value>myView.</value>
        public GraphView MyView
        {
            get { return myView; }
            set
            {
                myView = value;
                MyStaticView = value;
                //Setup View
            }
        }
        private static GraphView mystaticview;
        public static GraphView MyStaticView
        {
            get { return mystaticview; }
            set
            {
                mystaticview = value;
            }
        }
        #endregion Properties

        #region Delegates (1)

        public delegate void MetaObjectSelectedDelegate(MetaBase selectedObject, bool UseServer);

        #endregion

        private VisualIndicatorController indicatorController;
        public VisualIndicatorController IndicatorController
        {
            get
            {
                if (indicatorController == null)
                    indicatorController = new VisualIndicatorController();
                return indicatorController;
            }
            set { indicatorController = value; }
        }

        #region Methods (6)

        public void Group()
        {
            GroupingControl.GroupSelection(MyView);
        }

        /// <summary>
        /// Hooks up MyView to events
        /// </summary>
        /// 
        public bool isHooked = false;
        public void Hookup()
        {
            if (isHooked) return;
            // Attach eventhandlers for the view
            MyView.ObjectContextClicked += MyView_ObjectContextClicked;
            MyView.BackgroundDoubleClicked += MyView_BackgroundDoubleClicked;
            MyView.ObjectSingleClicked += MyView_ObjectSingleClicked;
            MyView.ObjectSelectionDropped += MyView_ObjectSelectionDropped;
            MyView.ObjectLostSelection += MyView_ObjectLostSelection;
            MyView.SelectionCopied += MyView_SelectionCopied;
            MyView.KeyUp += MyView_KeyUp;
            MyView.MouseUp += MyView_MouseUp;
            MyView.ObjectDoubleClicked += MyView_ObjectDoubleClicked;
            MyView.ClipboardPasted += MyView_ClipboardPasted;
            MyView.BackgroundContextClicked += MyView_BackgroundContextClicked;
            MyView.AutoScrollDelay = 500;
            MyView.HoverDelay = 200;
            MyView.ScrollSmallChange = new Size(35, 35);
            MyView.SelectionDeleting += new System.ComponentModel.CancelEventHandler(MyView_SelectionDeleting);
            isHooked = true;
        }
        public void UnHook()
        {
            isHooked = false;
            MyView.ObjectContextClicked -= MyView_ObjectContextClicked;
            MyView.BackgroundDoubleClicked -= MyView_BackgroundDoubleClicked;
            MyView.ObjectSingleClicked -= MyView_ObjectSingleClicked;
            MyView.ObjectSelectionDropped -= MyView_ObjectSelectionDropped;
            MyView.ObjectLostSelection -= MyView_ObjectLostSelection;
            MyView.SelectionCopied -= MyView_SelectionCopied;
            MyView.KeyUp -= MyView_KeyUp;
            MyView.MouseUp -= MyView_MouseUp;
            MyView.ObjectDoubleClicked -= MyView_ObjectDoubleClicked;
            MyView.ClipboardPasted -= MyView_ClipboardPasted;
            MyView.BackgroundContextClicked -= MyView_BackgroundContextClicked;
            MyView.SelectionDeleting -= new System.ComponentModel.CancelEventHandler(MyView_SelectionDeleting);
        }

        public void LayoutArtefacts()
        {
            Dictionary<GoPort, List<ArtefactNode>> portNodes = new Dictionary<GoPort, List<ArtefactNode>>();
            foreach (GoObject o in MyView.Document)
            {
                if (o is ArtefactNode)
                {
                    ArtefactNode node = o as ArtefactNode;
                    if (node.Port.LinksCount > 0)
                    {
                        GoPortLinkEnumerator linkEnum = node.Port.Links.GetEnumerator();
                        linkEnum.MoveNext();
                        GoPort port = linkEnum.Current.ToPort as GoPort;
                        if (portNodes.ContainsKey(port))
                        {
                            previousNode = portNodes[port][portNodes[port].Count - 1];
                            node.Position =
                                new PointF(previousNode.Position.X, previousNode.Position.Y + previousNode.Height + 5);
                            portNodes[port].Add(node);
                        }
                        else
                        {
                            node.Position = new PointF(port.Position.X + 25, port.Position.Y + 25);
                            List<ArtefactNode> nodes = new List<ArtefactNode>();
                            nodes.Add(node);
                            portNodes.Add(port, nodes);
                        }
                    }
                }
            }
        }

        public void MakeSubGraph()
        {
            MyView.StartTransaction();
            GroupingControl gc = new GroupingControl();
            gc.MakeSubGraph(MyView);
            MyView.FinishTransaction("Create Subgraph");
        }

        public void ReplaceMouseTools()
        {
            // Console.WriteLine("ReplaceMouseTools");
            HumanInterfaceController hic = new HumanInterfaceController();
            hic.ReplaceMouseTools(MyView);
        }

        public void Ungroup()
        {
            GroupingControl.UngroupSelection(MyView);
        }

        #endregion Methods

        #region Helpers

        /// <summary>
        /// Determines whether the specified sel contains shape.
        /// </summary>
        /// <param name="sel">The sel.</param>
        /// <returns>
        /// 	<c>true</c> if the specified sel contains shape; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsShape(GoSelection sel)
        {
            foreach (GoObject o in sel)
            {
                if (o is GoShape && (!(o.TopLevelObject is FrameLayerGroup)))
                    return true;
                GoGroup col = o as GoGroup;
                if (col != null)
                {
                    foreach (GoObject oChild in col)
                    {
                        if (oChild is GoShape && oChild.ParentNode is GraphNode || oChild.ParentNode == null ||
                            oChild.ParentNode is SubgraphNode)
                        {
                            return true;
                        }

                        if (oChild is GoGroup)
                        {
                            GoGroup node = oChild as GoGroup;
                            if (node != null)
                            {
                                foreach (GoObject oNodeChild in node)
                                {
                                    if (oNodeChild is GoShape)
                                        return true;
                                }
                            }
                        }
                    }

                    //if (col != null)
                    //{
                    foreach (GoObject child in col)
                    {
                        if (child is GoShape)
                            return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Determines whether the specified sel contains text.
        /// </summary>
        /// <param name="sel">The sel.</param>
        /// <returns>
        /// 	<c>true</c> if the specified sel contains text; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsText(IGoCollection sel)
        {
            foreach (GoObject o in sel)
            {
                if (o is GoText)
                {
                    bool overrideEditable = false;
                    GoText txt = o as GoText;
                    if (txt is BoundLabel)
                    {
                        BoundLabel lbl = txt as BoundLabel;
                        IMetaNode node = lbl.Parent as IMetaNode;
                        if (node != null)
                        {
                            if (node.BindingInfo != null)
                            {
                                if (node.BindingInfo.Bindings != null)
                                {
                                    if (node.BindingInfo.Bindings.ContainsKey(lbl.Name))
                                    {
                                        overrideEditable = true;
                                    }
                                }
                            }
                        }
                    }
                    if (((txt.Editable || overrideEditable) && txt.EditorStyle == GoTextEditorStyle.TextBox) || (txt.ParentNode is MappingCell) || (txt.ParentNode is ArtefactNode) || (txt.Parent is SubgraphNode))
                    {
                        return true;
                    }
                }
                //else?

                GoGroup col = o as GoGroup;
                if (col != null)
                {
                    if (ContainsText(col))
                        return true;
                }
            }
            return false;
        }

        #endregion

        public void SwitchFrame(NormalDiagram diagram)
        {
            if (diagram.DocumentFrame.Frame != null)
            {
                bool newAllowSelect = !diagram.FrameLayer.AllowSelect;
                diagram.DocumentFrame.Frame.Selectable = newAllowSelect;
                diagram.DocumentFrame.Frame.Resizable = newAllowSelect;
                diagram.DocumentFrame.Frame.Reshapable = newAllowSelect;
                diagram.FrameLayer.AllowSelect = newAllowSelect;

                if (newAllowSelect)
                {
                    diagram.DocumentFrame.Frame.Pen = new Pen(Color.Firebrick, 1f);
                    MyView.Selection.Clear();
                    MyView.Selection.Add(diagram.DocumentFrame.Frame);
                }
                else
                {
                    diagram.DocumentFrame.Frame.Pen = new Pen(Color.Gray, 1f);
                    MyView.Selection.Clear();
                    UpdateSize(MetaBuilder.Graphing.Controllers.GraphViewController.CropType.ToFrame);
                }
                MyView.UpdateView();
            }
        }
        private void ClearShadowsFromSelection()
        {
            GoCollectionEnumerator colEnumerator = MyView.Selection.GetEnumerator();
            while (colEnumerator.MoveNext())
            {
                if (colEnumerator.Current is GoNode)
                {
                    RemoveShadows(colEnumerator.Current as GoNode);
                    RemoveChangedIndicators(colEnumerator.Current as GoNode, true);
                }
            }
        }

        #region View Event Handlers

        public bool UseServerForContext;

        /// <summary>
        /// Handles the ObjectDoubleClicked event of the MyView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="evt">The <see cref="Northwoods.Go.GoObjectEventArgs"/> instance containing the event data.</param>
        private void MyView_ObjectDoubleClicked(object sender, GoObjectEventArgs evt)
        {
            if (!ReadOnly)
            {
                if (MyView.Document is NormalDiagram)
                {
                    NormalDiagram diagram = MyView.Document as NormalDiagram;
                    if (evt.GoObject.Parent is FrameLayerGroup && (!RubberStampEnabled))
                    {
                        try
                        {
                            SwitchFrame(diagram);
                        }
                        catch
                        {
                        }
                    }
                    if (evt.GoObject.Parent is FrameLayerGroup && (RubberStampEnabled))
                    {
                        MyView_BackgroundDoubleClicked(sender, evt);
                    }
                }

                //REMOVED CUSTOM CONTROL FROM ARTIFACTNODE SO THIS DOES NOTHING
                //if (evt.GoObject.ParentNode is ArtefactNode && evt.GoObject.ParentNode.Editable)
                //{
                //    if ((evt.GoObject.ParentNode as ArtefactNode).MetaObject.State != VCStatusList.CheckedOutRead || (evt.GoObject.ParentNode as ArtefactNode).MetaObject.State != VCStatusList.CheckedIn || (evt.GoObject.ParentNode as ArtefactNode).MetaObject.State != VCStatusList.Locked)
                //        evt.GoObject.ParentNode.DoBeginEdit(MyView);
                //}
                if (evt.GoObject is GoPort)
                {
                    //GoCollection col = new GoCollection();
                    //MyView.PickObjectsInRectangle(true, true, evt.GoObject.Bounds, GoPickInRectangleStyle.AnyIntersectsBounds, col, 20);
                    //foreach (GoObject o in col)
                    //{
                    //    if (o is GoText)
                    //    {
                    //        GoText txt = o as GoText;
                    //        txt.DoBeginEdit(MyView);
                    //        return;
                    //    }
                    //    if (o is GraphNode)
                    //    {
                    //        foreach (GoObject objChild in (o as GraphNode))
                    //        {
                    //            if (objChild is GraphNode)
                    //            {
                    //                foreach (GoObject objChildChild in (objChild as GraphNode))
                    //                {
                    //                    if (objChildChild is GoText)
                    //                    {
                    //                        if ((o as GraphNode).MetaObject.State != VCStatusList.CheckedOutRead || (o as GraphNode).MetaObject.State != VCStatusList.CheckedIn || (o as GraphNode).MetaObject.State != VCStatusList.Locked)
                    //                        {
                    //                            GoText txt = objChildChild as GoText;
                    //                            txt.DoBeginEdit(MyView);
                    //                        }
                    //                        return;
                    //                    }
                    //                }
                    //            }
                    //            //TODO : GRAPHNODE & GOTEXT?
                    //            if (objChild is GoText)
                    //            {
                    //                GoText txt = objChild as GoText;
                    //                txt.DoBeginEdit(MyView);
                    //                return;
                    //            }
                    //        }
                    //    }
                    //}
                }
            }

            //Legends work even if the diagram is readonly
            if (evt.GoObject.Parent is LegendItem)
            {
                LegendItem legendItem = evt.GoObject.Parent as LegendItem;
                legendItemChanged(legendItem, false);
            }
            if (evt.GoObject.Parent != null)
            {
                if (evt.GoObject.Parent.Parent is LegendItem)
                {
                    LegendItem legendItem = evt.GoObject.Parent.Parent as LegendItem;
                    legendItemChanged(legendItem, false);
                }
            }
        }

        public bool pastingShallow;
        private void MyView_ClipboardPasted(object sender, EventArgs e)
        {
            removeQuickMenu();
            //throw new Exception("The method or operation is not implemented.");
            if (!pastingShallow)
                ClearShadowsFromSelection();

            pastingShallow = false;
        }

        /// <summary>
        /// Handles the ObjectLostSelection event of the MyView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Northwoods.Go.GoSelectionEventArgs"/> instance containing the event data.</param>
        private void MyView_ObjectLostSelection(object sender, GoSelectionEventArgs e)
        {
            if (e.GoObject.IsInDocument || e.GoObject.IsInView)
            {
            }
            //    if (e.GoObject.ParentNode is GraphNode)
            //    {
            //        //October 2014 - commented because of WATDAFEEK?
            //        //GraphNode n = e.GoObject.ParentNode as GraphNode;
            //        //foreach (GoObject o in n)
            //        //{
            //        //    o.Selectable = false;
            //        //}
            //        removeQuickMenu();
            //    }
        }

        /// <summary>
        /// Handles the MouseUp event of the MyView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void MyView_MouseUp(object sender, MouseEventArgs e)
        {
            if (MyView.Tool is GoToolPanning)
            {
                GoToolPanning activeTool = MyView.Tool as GoToolPanning;
                activeTool.StopTool();
            }
            bool shift = false;
            bool control = false;
            bool alt = false;
            if ((Control.ModifierKeys & Keys.Shift) != 0)
                shift = true;
            if ((Control.ModifierKeys & Keys.Alt) != 0)
                alt = true;
            if ((Control.ModifierKeys & Keys.Control) != 0)
                control = true;
            if (e.Button == MouseButtons.Right && control && !shift)
            {
                GoToolPanning panningtool = new GoToolPanning(MyView);
                panningtool.AutoPan = false;
                panningtool.Modal = false;
                MyView.Tool = panningtool;
                panningtool.Start();
            }
            if (control && shift)
            {
                PointF originalDocPosition = MyView.DocPosition;
                if (e.Button == MouseButtons.Right)
                    MyView.DocScale -= 0.1f;
                else
                    MyView.DocScale += 0.1f;
                //MyView.DocPosition = new PointF((MyView.LastInput.DocPoint.X - diffX) * MyView.DocScale,( MyView.LastInput.DocPoint.Y - diffY) * MyView.DocScale);
                MyView.RescaleWithCenter(MyView.DocScale, MyView.LastInput.DocPoint);
                //MyView.ScrollRectangleToVisible(new RectangleF(new PointF(MyView.LastInput.DocPoint.X - 20, MyView.LastInput.DocPoint.Y - 20), new SizeF(40, 40)));
            }
            if (alt && e.Button == MouseButtons.Left)
            {
                GoToolZooming zoomingTool = new GoToolZooming(MyView);
                zoomingTool.AutoScrolling = true;
                GoToolRubberBanding gtRubberband = MyView.Tool as GoToolRubberBanding;
                if (gtRubberband != null)
                    zoomingTool.Box = gtRubberband.Box;
                MyView.Tool = zoomingTool;
            }
        }

        /// <summary>
        /// Handles the KeyUp event of the MyView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        private void MyView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Oemtilde && myView.Selection.Count == 1)
            {
                if (myView.Selection.First is GraphNode)
                {
                    GraphNode n = myView.Selection.First as GraphNode;
                    if (quickPanel == null && n != null) //prevent more than one from being shown by double selection object
                        if (VCStatusTool.UserHasControl(n.MetaObject) && n.MetaObject.State != VCStatusList.CheckedOutRead && n.MetaObject.State != VCStatusList.CheckedIn)
                            addQuickMenuOverride(n);
                    return;
                }
            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (MyView.Selection.Count > 0)
                {
                    if (MyView.Selection.Primary is GoText)
                    {
                        GoText txt = MyView.Selection.Primary as GoText;
                        if (Core.Variables.Instance.IsViewer)
                        {
                            //cant remove artifacts from viewer
                        }
                        else
                        {
                            if (txt.ParentNode is ArtefactNode)
                            {
                                txt.ParentNode.Remove();
                            }
                        }
                    }
                }
            }
            else if (e.KeyCode == Keys.Insert)
            {
                if (MyView.Selection.Primary != null)
                {
                    if (MyView.Selection.Primary.ParentNode is CollapsibleNode || MyView.Selection.Primary is CollapsibleNode || MyView.Selection.Primary is CollapsingRecordNodeItemList)
                    {
                        IMetaNode node = null;
                        if (MyView.Selection.Primary is IMetaNode)
                            node = MyView.Selection.Primary as IMetaNode;
                        if (MyView.Selection.Primary.ParentNode is IMetaNode)
                            node = MyView.Selection.Primary.ParentNode as IMetaNode;

                        if (node != null && node.MetaObject != null)
                            if (node.MetaObject.State == VCStatusList.CheckedIn || node.MetaObject.State == VCStatusList.CheckedIn || node.MetaObject.State == VCStatusList.CheckedOutRead || node.MetaObject.State == VCStatusList.Locked || node.MetaObject.State == VCStatusList.Obsolete)
                                return;

                        if (MyView.Selection.Primary is CollapsingRecordNodeItemList)
                        {
                            RepeaterSection rSection = MyView.Selection.Primary as RepeaterSection;
                            if (rSection != null)
                            {
                                MyView.StartTransaction();
                                rSection.AddItemFromCode();
                                MyView.FinishTransaction("Add item");
                            }
                        }
                        if (MyView.Selection.Primary is CollapsingRecordNodeItem)
                        {
                            RepeaterSection rSection = (MyView.Selection.Primary as CollapsingRecordNodeItem).Parent as RepeaterSection;
                            if (rSection != null)
                            {
                                MyView.StartTransaction();
                                rSection.AddItemFromCode();
                                MyView.FinishTransaction("Add item");
                            }
                        }
                        if (MyView.Selection.Primary.Parent != null)
                        {
                            if (MyView.Selection.Primary.Parent.Parent is RepeaterSection)
                            {
                                RepeaterSection rSection = MyView.Selection.Primary.Parent.Parent as RepeaterSection;
                                MyView.StartTransaction();
                                rSection.AddItemFromCode();
                                MyView.FinishTransaction("Add item");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the SelectionCopied event of the MyView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="evt">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void MyView_SelectionCopied(object sender, EventArgs evt)
        {
            //ClearShadowsFromSelection();
        }

        /// <summary>
        /// Handles the ObjectSelectionDropped event of the MyView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Northwoods.Go.GoObjectEventArgs"/> instance containing the event data.</param>
        private void MyView_ObjectSelectionDropped(object sender, GoObjectEventArgs e)
        {
            //MyView.RaiseObjectGotSelection(e.GoObject);
        }

        private void MyView_ObjectSingleClicked(object sender, GoObjectEventArgs e)
        {
            if (MyView.Doc != null)
            {
                if (MyView.Doc.FileType == FileTypeList.Symbol)
                {
                    MyView.RaiseObjectGotSelection(e.GoObject);
                    return;
                }
            }

            if (e.GoObject.ParentNode is GraphNode && !(e.GoObject is IndicatorLabel))
            {
                //MyView.RaiseObjectGotSelection(e.GoObject);
                GraphNode n = e.GoObject.ParentNode as GraphNode;
                foreach (GoObject o in n)
                {
                    if (!(o is GraphNodeGrid) && n.EditMode)
                        o.Selectable = true;
                }
            }
            //else if (e.GoObject.ParentNode is IMetaNode)
            //{
            //    MyView.RaiseObjectGotSelection(e.GoObject);
            //}
            MyView.RaiseObjectGotSelection(e.GoObject);

            //Select all artifacts linked to selected associations
            List<GoObject> objectsToSelect = new List<GoObject>();
            List<GoObject> objectsToRemove = new List<GoObject>();
            foreach (GoObject o in MyView.Selection)
            {
                if (o is GoLabeledLink)
                {
                    GoLabeledLink l = o as GoLabeledLink;
                    if (l is QLink)
                    {
                        reAnchorableComments = new Dictionary<ResizableBalloonComment, GoObject>();
                        foreach (KeyValuePair<ArtefactNodeLinkKey, QLink> k in FindArtefactsOnLink(l as QLink, l as QLink))
                        {
                            try
                            {
                                objectsToSelect.Add(k.Key.Node);
                            }
                            catch
                            {
                            }
                        }
                        foreach (KeyValuePair<ResizableBalloonComment, GoObject> k in reAnchorableComments)
                        {
                            try
                            {
                                objectsToSelect.Add(k.Key);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                //if (o is CollapsingRecordNodeItem && (o as CollapsingRecordNodeItem).IsHeader)
                //{
                //    //objectsToRemove.Add(o);
                //    //objectsToSelect.Add(o.ParentNode);
                //}
            }
            foreach (GoObject o in objectsToRemove)
                MyView.Selection.Remove(o);
            foreach (GoObject o in objectsToSelect)
                MyView.Selection.Add(o);
        }

        //Select node=> press '~'
        public QuickPanel quickPanel = null;
        //18 October 2013 - breaks copy because not serializable (Panel and button class)
        public void addQuickMenu(GraphNode node)
        {
            if (!Core.Variables.Instance.UseQuickPanel)
                return;
            //TODO cache Menus in variables on startup :)
            if (quickPanel != null || node == null)
            {
                return; //prevent more than one from being shown by double selection object
            }
            removeQuickMenu();
            quickPanel = new QuickPanel(node.MetaObject._ClassName, MyView, node);
            myView.Document.SkipsUndoManager = true;
            //myView.StartTransaction();
            MyView.Document.Add(quickPanel);
            //myView.FinishTransaction("Add Quick Panel");
            myView.Document.SkipsUndoManager = false;
        }
        public void addQuickMenuOverride(GraphNode node)
        {
            //if (!Core.Variables.Instance.UseQuickPanel) return;
            //TODO cache Menus in variables on startup :)
            if (quickPanel != null || node == null)
            {
                return; //prevent more than one from being shown by double selection object
            }
            removeQuickMenu();
            quickPanel = new QuickPanel(node.MetaObject._ClassName, MyView, node);
            myView.Document.SkipsUndoManager = true;
            //myView.StartTransaction();
            MyView.Document.Add(quickPanel);
            //myView.FinishTransaction("Add Quick Panel");
            myView.Document.SkipsUndoManager = false;
        }
        public void removeQuickMenu()
        {
            if (quickPanel != null && MyView.Document.Contains(quickPanel))
            {
                myView.Document.SkipsUndoManager = true;
                //myView.StartTransaction();
                MyView.Document.Remove(quickPanel);
                quickPanel = null;
                //myView.FinishTransaction("Remove Quick Panel");
                myView.Document.SkipsUndoManager = false;
            }
        }

        /// <summary>
        /// Handles the BackgroundDoubleClicked event of the MyView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="evt">The <see cref="Northwoods.Go.GoInputEventArgs"/> instance containing the event data.</param>
        private void MyView_BackgroundDoubleClicked(object sender, GoInputEventArgs evt)
        {
            if (rubberStampShape != null && RubberStampEnabled && MyView.Doc.FileType == FileTypeList.Diagram)
            {
                GoObject protoCopy = rubberStampShape.Copy();
                protoCopy.Position = evt.DocPoint;
                MyView.Document.Add(protoCopy);
                if (AutoLinkingEnabled)
                {
                    if (protoCopy is GoNode)
                    {
                        if (previousNode != null)
                        {
                            QLink autoLink = new QLink();
                            if (previousNode is GraphNode && protoCopy is GraphNode)
                            {
                                GraphNode parentNode = previousNode as GraphNode;
                                if (parentNode.HasBindingInfo)
                                {
                                    autoLink.FromPort = (previousNode as GraphNode).GetDefaultPort;
                                    autoLink.ToPort = (protoCopy as GraphNode).GetDefaultPort;
                                    MyView.Document.Add(autoLink);
                                    // get the default connection
                                    TList<ClassAssociation> assocCollection = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByParentClass(parentNode.BindingInfo.BindingClass);
                                    foreach (ClassAssociation assoc in assocCollection)
                                    {
                                        if (assoc.ParentClass == parentNode.BindingInfo.BindingClass && assoc.IsDefault)
                                        {
                                            autoLink.AssociationType = (LinkAssociationType)Enum.Parse(typeof(LinkAssociationType), assoc.AssociationTypeID.ToString());
                                            AutoRelinkTool.CalculateAndRelink(protoCopy as GoNode);
                                            return;
                                        }
                                    }
                                }
                                AutoRelinkTool.CalculateAndRelink(protoCopy as GoNode);
                                //GoSelectionEventArgs evtArgs2 = new GoSelectionEventArgs(autoLink);
                            }
                        }
                    }
                }
            }
        }

        protected void HighlightConnectedObjects(object sender, EventArgs e)
        {
            MenuItem mnuItemSender = sender as MenuItem;
            IMetaNode node = mnuItemSender.Tag as IMetaNode;
            NodeObjectContextArgs args = new NodeObjectContextArgs();
            args.MyNode = node;
            args.OriginalDocumentName = MyView.Document.Name;
            MyView.FireNodeObjectContextClicked(sender, args);
        }
        protected void HighlightShallowCopies(object sender, EventArgs e)
        {
            MenuItem mnuItemSender = sender as MenuItem;
            IMetaNode node = mnuItemSender.Tag as IMetaNode;

            NodeObjectContextArgs args = new NodeObjectContextArgs();
            args.MyNode = node;
            args.OriginalDocumentName = MyView.Document.Name;
            MyView.FireNodeObjectContextClickedShallow(sender, args);
        }
        public void LayoutFSD(object sender, EventArgs e)
        {
            Layout.FSDLayout f;
            if (sender is MenuItem)
                f = new Layout.FSDLayout(MyView, (sender as MenuItem).Tag as GraphNode);
            else
                f = new Layout.FSDLayout(MyView);
            f.ShowDialog();
        }

        private void AddRationale(object sender, EventArgs e)
        {
            ShapeDesignController shapeProtoTypingTool = new ShapeDesignController(MyView);
            shapeProtoTypingTool.AddRationale();
        }

        private void AddComment(object sender, EventArgs e)
        {
            ShapeDesignController shapeProtoTypingTool = new ShapeDesignController(MyView);
            MenuItem mnuItem = sender as MenuItem;
            if (mnuItem.Tag != null && !(mnuItem.Tag is FrameLayerRect))
            {
                ResizableBalloonComment com = shapeProtoTypingTool.AddBalloonComment();
                com.ViewerAdded = Core.Variables.Instance.IsViewer;
            }
            else
            {
                ResizableComment com = shapeProtoTypingTool.AddComment();
                com.ViewerAdded = Core.Variables.Instance.IsViewer;
            }
        }

        private void AddProperty(object sender, EventArgs e)
        {
            MyView.Document.Add(new PropertyNode());
        }

        public static void PromptForKeepExistingAssociations(MappingCell mc, List<IMetaNode> overlappingNodes, bool controlsSwimlaneObject)
        {
            bool hasControlOverAtLeastOne = false;
            foreach (IMetaNode imn in overlappingNodes)
            {
                if (VCStatusTool.UserHasControl(imn.MetaObject))
                    hasControlOverAtLeastOne = true;
            }
            if (controlsSwimlaneObject && hasControlOverAtLeastOne)
            {
                string sMessageKeepExistingAssociations = "Do you wish to keep existing associations for " + mc.MetaObject + " (" + mc.MetaObject._ClassName + ") as they appear on this diagram?";
                DialogResult res = MessageBox.Show(sMessageKeepExistingAssociations, "Keep Associations", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.No)
                {
                    TList<ObjectAssociation> oas = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(mc.MetaObject.pkid, mc.MetaObject.MachineName);
                    foreach (IMetaNode imn in overlappingNodes)
                    {
                        if (imn.MetaObject != null)
                        {
                            if (imn.MetaObject.pkid > 0)
                                if (VCStatusTool.UserHasControl(imn.MetaObject) || VCStatusTool.UserHasControl(mc.MetaObject))
                                {
                                    oas.Filter = "ChildObjectID = " + imn.MetaObject.pkid;
                                    foreach (ObjectAssociation oa in oas)
                                    {
                                        oa.VCStatusID = (int)VCStatusList.MarkedForDelete;
                                        DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Save(oa);
                                    }
                                }
                        }
                    }
                }
            }
        }

        private void SelectChildNodes(object sender, EventArgs e)
        {
            MenuItem mnuItem = sender as MenuItem;
            GoNode node = mnuItem.Tag as GoNode;
            MyView.Selection.Clear();
            MyView.Selection.Add(node);
            AddToSelection(node);
        }

        private void AddToSelection(GoNode n)
        {
            GoNodeLinkEnumerator linkEnum = n.DestinationLinks;
            while (linkEnum.MoveNext())
            {
                if (linkEnum.Current is QLink)
                {
                    MyView.Selection.Add(linkEnum.Current as QLink);
                }
                if (!MyView.Selection.Contains(linkEnum.Current.ToNode as GoNode))
                {
                    GoNode node = linkEnum.Current.ToNode as GoNode;
                    if (node != null)
                    {
                        MyView.Selection.Add(node);
                        AddToSelection(node);
                    }
                }
            }
        }

        private void SelectInternalLabel(object sender, EventArgs e)
        {
            MenuItem mnuItem = sender as MenuItem;
            BoundLabel label = mnuItem.Tag as BoundLabel;
            MyView.Selection.Clear();
            MyView.Selection.Add(label);
            //label.AddSelectionHandles(MyView.Selection, label);
            MyView.Invalidate();

            label.DoBeginEdit(MyView);
        }

        /// <summary>
        /// Clears all behaviours from the selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearBehaviours_Command(object sender, EventArgs e)
        {
            foreach (GoObject o in MyView.Selection)
            {
                if (o is IBehaviourShape)
                    ((IBehaviourShape)o).Manager.ClearBehaviours(sender, e);
            }
        }

        /// <summary>
        /// Populates the contextmenu with behaviour related items
        /// </summary>
        private void AddContextMenuItemsForBehaviours()
        {
            //bool shift = false;
            //bool control = false;
            //if ((Control.ModifierKeys & Keys.Shift) != 0)
            //    shift = true;
            //if ((Control.ModifierKeys & Keys.Control) != 0)
            //    control = true;
            //if (MyView.Selection.Count > 0)
            //{
            //bool FoundBehaviour = false;
            //foreach (GoObject o in MyView.Selection)
            //{
            //    if (o is IBehaviourShape)
            //    {
            //        FoundBehaviour = true;
            //    }
            //}

            //if (MyView.Selection.Primary is CollapsingRecordNodeItem)
            //{
            //    CollapsingRecordNodeItem colNodeItem = MyView.Selection.Primary as CollapsingRecordNodeItem;
            //    if (!colNodeItem.IsHeader)
            //    {
            //        if (colNodeItem.MetaObject != null)
            //        {
            //            MenuItem menuItemRemoveItem = new MenuItem("Remove " + colNodeItem.MetaObject.Class, mnuRemoveItemCommand);
            //            menuItemRemoveItem.Tag = colNodeItem;
            //            MyView.ViewContextMenu.MenuItems.Add(3, menuItemRemoveItem);
            //        }

            //        //MenuItem menuItemSelectAll = new MenuItem("Select All Items", mnuSelectAllRecordItems);
            //        //menuItemSelectAll.Tag = colNodeItem;
            //        //MyView.ViewContextMenu.MenuItems.Add(menuItemSelectAll);

            //        //MenuItem menuItemSelectUp = new MenuItem("Select All Items Above", mnuSelectItemsUp);
            //        //menuItemSelectUp.Tag = colNodeItem;
            //        //MyView.ViewContextMenu.MenuItems.Add(menuItemSelectUp);

            //        //MenuItem menuItemSelectDown = new MenuItem("Select All Items Below", mnuSelectItemsDown);
            //        //menuItemSelectDown.Tag = colNodeItem;
            //        //MyView.ViewContextMenu.MenuItems.Add(menuItemSelectDown);
            //    }
            //}
            //}
        }

        private void mnuRemoveItemCommand(object sender, EventArgs e)
        {
            MenuItem mSender = sender as MenuItem;
            CollapsingRecordNodeItem item = mSender.Tag as CollapsingRecordNodeItem;
            item.ParentList.Remove(item);
        }
        private void mnuSelectAllRecordItems(object sender, EventArgs e)
        {
            MenuItem mSender = sender as MenuItem;
            CollapsingRecordNodeItem item = mSender.Tag as CollapsingRecordNodeItem;
        }
        private void mnuSelectItemsUp(object sender, EventArgs e)
        {
            MenuItem mSender = sender as MenuItem;
            CollapsingRecordNodeItem item = mSender.Tag as CollapsingRecordNodeItem;
        }
        private void mnuSelectItemsDown(object sender, EventArgs e)
        {
            MenuItem mSender = sender as MenuItem;
            CollapsingRecordNodeItem item = mSender.Tag as CollapsingRecordNodeItem;
        }

        /// <summary>
        /// Open a new contextviewer, set the metaobject and add to the dockingform
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuViewInContext_Click(object sender, EventArgs e)
        {
            MenuItem mItemSender = sender as MenuItem;
            MetaBase mbObject = mItemSender.Tag as MetaBase;
            mItemSender.Tag = null;
            OnMetaObjectContextRequested(mbObject, false);
        }

        /// <summary>
        /// Open a new contextviewer, set the metaobject and add to the dockingform
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuViewInContextServer_Click(object sender, EventArgs e)
        {
            if (Core.Networking.Pinger.Ping(Core.Variables.Instance.ServerConnectionString))
            {
                MenuItem mItemSender = sender as MenuItem;
                MetaBase mbObject = mItemSender.Tag as MetaBase;
                mItemSender.Tag = null;
                OnMetaObjectContextRequested(mbObject, true);
            }
            else
            {
                MessageBox.Show("Unable to connect to your current synchronisation server.", "Ping", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public event MetaObjectSelectedDelegate MetaObjectContextRequest;
        public void OnMetaObjectContextRequested(MetaBase mbase, bool useServer)
        {
            if (MetaObjectContextRequest != null)
            {
                MetaObjectContextRequest(mbase, useServer);
            }
        }

        /// <summary>
        /// Adds a generic artefact to a link
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddGenericArtefact(object sender, EventArgs e)
        {
            MyView.StartTransaction();
            MenuItem mItem = sender as MenuItem;
            AllowedArtifact art = mItem.Tag as AllowedArtifact;
            QLink lnk = MyView.SelectedLinks[0];
            ArtefactNode n = new ArtefactNode();
            n.BindingInfo = new BindingInfo();
            n.BindingInfo.BindingClass = art.Class;
            n.CreateMetaObject(sender, e);
            n.Text = art.Class;
            n.Location = new PointF(MyView.LastInput.DocPoint.X - 10, MyView.LastInput.DocPoint.Y - 10);
            n.Editable = true;
            n.Label.Editable = false;
            MyView.Document.Add(n);

            GoGroup grp = lnk.MidLabel as GoGroup;
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
                    fishlink.Visible = ArtefactPointersVisible;
                    MyView.Document.Add(fishlink);
                }
            }

            if (Core.Variables.Instance.SaveOnCreate)
            {
                if (myView.Document is NormalDiagram)
                    if ((MyView.Document as NormalDiagram).VersionManager.CurrentVersion != null && n.MetaObject.WorkspaceName == null && n.MetaObject.WorkspaceTypeId == 0)
                    {
                        n.MetaObject.WorkspaceName = (MyView.Document as NormalDiagram).VersionManager.CurrentVersion.WorkspaceName;
                        n.MetaObject.WorkspaceTypeId = (MyView.Document as NormalDiagram).VersionManager.CurrentVersion.WorkspaceTypeId;
                    }
                n.FireMetaObjectChanged(this, EventArgs.Empty);
            }
            //n.DoBeginEdit(MyView);
            MyView.FinishTransaction("Add artefact");
        }
        public ArtefactNode AddGenericArtefactWithNodeReturn(AllowedArtifact art)
        {
            MyView.StartTransaction();
            //MenuItem mItem = sender as MenuItem;
            //AllowedArtifact art = mItem.Tag as AllowedArtifact;
            QLink lnk = MyView.SelectedLinks[0];
            ArtefactNode n = new ArtefactNode();
            n.BindingInfo = new BindingInfo();
            n.BindingInfo.BindingClass = art.Class;
            n.CreateMetaObject();
            n.Text = art.Class;
            n.Location = new PointF(MyView.LastInput.DocPoint.X - 10, MyView.LastInput.DocPoint.Y - 10); //new PointF(lnk.MidLabel.Location.X, lnk.MidLabel.Location.Y - 10);
            n.Editable = true;
            n.Label.Editable = false;
            MyView.Document.Add(n);
            GoGroup grp = lnk.MidLabel as GoGroup;
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
                    fishlink.Visible = ArtefactPointersVisible;
                    MyView.Document.Add(fishlink);
                }
            }

            if (Core.Variables.Instance.SaveOnCreate)
            {
                if (myView.Document is NormalDiagram)
                    if ((MyView.Document as NormalDiagram).VersionManager.CurrentVersion != null && n.MetaObject.WorkspaceName == null && n.MetaObject.WorkspaceTypeId == 0)
                    {
                        n.MetaObject.WorkspaceName = (MyView.Document as NormalDiagram).VersionManager.CurrentVersion.WorkspaceName;
                        n.MetaObject.WorkspaceTypeId = (MyView.Document as NormalDiagram).VersionManager.CurrentVersion.WorkspaceTypeId;
                    }
                n.FireMetaObjectChanged(this, EventArgs.Empty);
            }

            //n.DoBeginEdit(MyView);
            MyView.FinishTransaction("Add artefact");
            return n;
        }

        private void AddExistingArtefact(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Adds "Artefact" menuitems to cxMenu
        /// </summary>
        /// <param name="lnk"></param>
        private void AddArtefactMenuItems(QLink lnk)
        {
            // see if there are generic artefacts
            IMetaNode nFrom = lnk.FromNode as IMetaNode;
            IMetaNode nTo = lnk.ToNode as IMetaNode;
            Hashtable hashArtefacts = new Hashtable();
            if (nFrom != null && nTo != null && nFrom.HasBindingInfo && nTo.HasBindingInfo)
            {
                int AssociationTypeID = (int)lnk.AssociationType;
                MenuItem addArtefact = new MenuItem("Add Artefact");
                MenuItem addExistingArtefact = new MenuItem("Existing", AddExistingArtefact);
                addArtefact.MenuItems.Add(addExistingArtefact);
                addArtefact.MenuItems.Add(new MenuItem("-"));
                // see if there are artifacts available
                bool FoundArtefacts = false;
                TList<ClassAssociation> parentAssociationCollection = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByParentClass(nFrom.BindingInfo.BindingClass);
                foreach (ClassAssociation parentAssociation in parentAssociationCollection)
                {
                    if (parentAssociation.ChildClass == nTo.BindingInfo.BindingClass)
                    {
                        TList<AllowedArtifact> allowedArtifactsOnThisLink = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.AllowedArtifactProvider.GetByCAid(parentAssociation.CAid);
                        foreach (AllowedArtifact art in allowedArtifactsOnThisLink)
                        {
                            if (art.IsActive == false || DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassProvider.GetByName(art.Class).IsActive == false)
                                continue;

                            if (parentAssociation.AssociationTypeID == AssociationTypeID)
                            {
                                //16 January Artefacts exclude rationale
                                if (art.Class == "Rationale")
                                    continue;

                                FoundArtefacts = true;
                                MenuItem mArtItem = new MenuItem(art.Class);//, AddGenericArtefact);
                                mArtItem.Tag = art;
                                if (!hashArtefacts.ContainsKey(art.Class))
                                {
                                    addArtefact.MenuItems.Add(mArtItem);
                                    hashArtefacts.Add(art.Class, true);
                                }
                            }
                        }
                    }
                }
                if (FoundArtefacts)
                {
                    addExistingArtefact.Tag = hashArtefacts;
                    MyView.ViewContextMenu.MenuItems.Add(addArtefact);
                    MyView.ViewContextMenu.MenuItems.Add(new MenuItem("-"));
                }
            }
        }

        protected void InsertPoint_Command(object sender, EventArgs e)
        {
            MenuItem mnuSender = sender as MenuItem;
            LinkController lcontroller = new LinkController();
            GoLabeledLink lnk = mnuSender.Tag as GoLabeledLink;
            lcontroller.InsertPoint_Command(lnk, sender, e);
            if (lnk is QLink)
                (lnk as QLink).Disable_AutoRouting(sender, e);
        }
        protected void RemoveSegment_Command(object sender, EventArgs e)
        {
            MenuItem mnuSender = sender as MenuItem;
            LinkController lcontroller = new LinkController();
            GoLabeledLink lnk = mnuSender.Tag as GoLabeledLink;
            lcontroller.RemoveSegment_Command(lnk, sender, e);
        }
        //11 March 2014
        protected void RemoveLinkMFD_Command(object sender, EventArgs e)
        {
            MyView.StartTransaction();
            MenuItem mnuSender = sender as MenuItem;
            if (mnuSender.Tag is Collection<QLink>)
            {
                Collection<QLink> MFDLinks = mnuSender.Tag as Collection<QLink>;
                foreach (QLink q in MFDLinks)
                {
                    MarkLinkNone(q);
                }
            }
            else if (mnuSender.Tag is QLink)
            {
                QLink q = mnuSender.Tag as QLink;
                MarkLinkNone(q);
            }
            MyView.FinishTransaction("Restore Selected Associations");
        }

        private void MarkLinkNone(QLink q)
        {
            LinkController lcontroller = new LinkController();
            ObjectAssociation assoc = lcontroller.GetAssociation(q, true);
            if (assoc != null)
            {
                if (assoc.VCStatusID == 8)
                {
                    assoc.VCStatusID = (int)VCStatusList.None;
                    assoc.State = VCStatusList.None;
                    DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Save(assoc);

                    q.ChangedLinkType();

                    foreach (GoObject o in MyView.Document)
                    {
                        if (!(o is QLink))
                            continue;

                        if (o == q)
                            continue;

                        try
                        {
                            QLink link = o as QLink;
                            if ((link.FromNode as IMetaNode).MetaObject.pkid == (q.FromNode as IMetaNode).MetaObject.pkid && (link.ToNode as IMetaNode).MetaObject.pkid == (q.ToNode as IMetaNode).MetaObject.pkid && (link.FromNode as IMetaNode).MetaObject.MachineName == (q.FromNode as IMetaNode).MetaObject.MachineName && (link.ToNode as IMetaNode).MetaObject.MachineName == (q.ToNode as IMetaNode).MetaObject.MachineName)
                            {
                                if (link.AssociationType == q.AssociationType)
                                    link.ChangedLinkType();
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
            lcontroller = null;
        }

        /// <summary>
        /// Adds the context menu items for links.
        /// </summary>
        private void AddContextMenuItemsForLinks()
        {
            if (MyView.SelectedLinks.Count == 1)
            {
                QLink lnk = MyView.SelectedLinks[0];

                IMetaNode nFrom = lnk.FromNode as IMetaNode;
                IMetaNode nTo = lnk.ToNode as IMetaNode;
                if (nFrom != null && nTo != null)
                {
                    bool readFrom = nFrom.MetaObject.State == VCStatusList.CheckedIn || nFrom.MetaObject.State == VCStatusList.Locked || nFrom.MetaObject.State == VCStatusList.Obsolete;// || nFrom.MetaObject.State == VCStatusList.CheckedOutRead;
                    bool readTo = nTo.MetaObject.State == VCStatusList.CheckedIn || nTo.MetaObject.State == VCStatusList.Locked || nTo.MetaObject.State == VCStatusList.Obsolete;// || nTo.MetaObject.State == VCStatusList.CheckedOutRead;

                    if (readFrom && readTo)
                    {
                        //ENABLE READONLY ASSOCIATION CHANGING
                        return;
                    }
                }

                AddContextMenuItemsForLinkTypes();
                AddArtefactMenuItems(lnk);
                MenuItem mnuItemInsertPoint = new MenuItem("Insert Point", InsertPoint_Command);
                mnuItemInsertPoint.Tag = lnk;
                MyView.ViewContextMenu.MenuItems.Add(mnuItemInsertPoint);

                if (lnk.RealLink.PointsCount > 2)
                {
                    MenuItem mnuItemRemoveSegment = new MenuItem("Remove Point", RemoveSegment_Command);
                    mnuItemRemoveSegment.Tag = lnk;
                    MyView.ViewContextMenu.MenuItems.Add(mnuItemRemoveSegment);
                }

                if (!lnk.RealLink.AvoidsNodes)
                {
                    MyView.ViewContextMenu.MenuItems.Add(new MenuItem("Enable Auto-Routing", lnk.Enable_AutoRouting));
                }
                else
                {
                    MyView.ViewContextMenu.MenuItems.Add(new MenuItem("Disable Auto-Routing", lnk.Disable_AutoRouting));
                }

                RemoveInvalidMenuItems(MyView.ViewContextMenu, MyView.SelectedLinks[0]);
                //<- removes the link's own association type

                IMetaNode nFromLink = lnk.FromNode as IMetaNode;
                IMetaNode nToLink = lnk.ToNode as IMetaNode;

                AssociationHelper assHelper = new AssociationHelper();
                b.ObjectAssociationKey oaKey;
                b.ObjectAssociation oa;
                Association a = assHelper.GetAssociation(nFrom.MetaObject._ClassName, nTo.MetaObject._ClassName, (int)lnk.AssociationType);
                if (a != null)
                {
                    oaKey = new ObjectAssociationKey(a.ID, nFrom.MetaObject.pkid, nTo.MetaObject.pkid, nFrom.MetaObject.MachineName, nTo.MetaObject.MachineName);
                    oa = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Get(oaKey);
                    if (VCStatusTool.IsObsoleteOrMarkedForDelete(oa) && (oa.VCMachineID == null || oa.VCMachineID.Length == 0))
                    {
                        MyView.ViewContextMenu.MenuItems.Add("-");
                        MenuItem mnuItemRemoveMFD = new MenuItem("Restore Selected Association", RemoveLinkMFD_Command);
                        mnuItemRemoveMFD.Tag = lnk;
                        MyView.ViewContextMenu.MenuItems.Add(mnuItemRemoveMFD);
                        MyView.ViewContextMenu.MenuItems.Add("-");
                    }
                }
            }
            else if (MyView.SelectedLinks.Count > 1)
            {
                // for bigger selections, we first need to ensure that all the parent-child classes are the same.
                LinkEndPointComparisonResult result = CompareSelectedLinksEndPointClasses();
                if (result == LinkEndPointComparisonResult.BothEQ)
                {
                    AddContextMenuItemsForLinkTypes();
                }
                RemoveInvalidMenuItems(MyView.ViewContextMenu, MyView.SelectedLinks[0]);

                Collection<QLink> MFDLinks = new Collection<QLink>();
                foreach (QLink link in MyView.SelectedLinks)
                {
                    IMetaNode nFrom = link.FromNode as IMetaNode;
                    IMetaNode nTo = link.ToNode as IMetaNode;

                    AssociationHelper assHelper = new AssociationHelper();
                    b.ObjectAssociationKey oaKey;
                    b.ObjectAssociation oa;
                    Association a = assHelper.GetAssociation(nFrom.MetaObject._ClassName, nTo.MetaObject._ClassName, (int)link.AssociationType);
                    if (a != null)
                    {
                        oaKey = new ObjectAssociationKey(a.ID, nFrom.MetaObject.pkid, nTo.MetaObject.pkid, nFrom.MetaObject.MachineName, nTo.MetaObject.MachineName);
                        oa = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Get(oaKey);
                        if (VCStatusTool.IsObsoleteOrMarkedForDelete(oa) && (oa.VCMachineID == null || oa.VCMachineID.Length == 0))
                        {
                            MFDLinks.Add(link);
                        }
                    }
                }
                if (MFDLinks.Count > 0)
                {
                    MyView.ViewContextMenu.MenuItems.Add("-");
                    MenuItem mnuItemRemoveMFD = new MenuItem("Restore Selected Associations", RemoveLinkMFD_Command);
                    mnuItemRemoveMFD.Tag = MFDLinks;
                    MyView.ViewContextMenu.MenuItems.Add(mnuItemRemoveMFD);
                    MyView.ViewContextMenu.MenuItems.Add("-");
                }
            }
        }

        /// <summary>
        /// Removes any menuitem with specified text from a contextmenu
        /// </summary>
        /// <param name="cm"></param>
        /// <param name="Text"></param>
        private void RemoveMenuItem(ContextMenu cm, string Text)
        {
            foreach (MenuItem mItem in cm.MenuItems)
            {
                if (mItem.Text == "Change association...")
                {
                    MenuItem mainItem = mItem;
                    List<MenuItem> itemsToRemove = new List<MenuItem>();
                    foreach (MenuItem mitem in mItem.MenuItems)
                    {
                        if (mitem.Text == Text)
                        {
                            itemsToRemove.Add(mitem);
                        }
                    }
                    foreach (MenuItem mitem in itemsToRemove)
                    {
                        mainItem.MenuItems.Remove(mitem);
                    }
                    mItem.Visible = mItem.MenuItems.Count > 0;
                }
            }
        }

        /// <summary>
        /// Removes the invalid menu items.
        /// </summary>
        /// <param name="cm">The cm.</param>
        public void RemoveInvalidMenuItems(ContextMenu cm, QLink link)
        {
            RemoveMenuItem(cm, "Change to " + link.AssociationType.ToString().Replace("_", " "));
            // Force the user to use metadictionary specification regarding allowed associations
            //Forced to metaoject classes
            IMetaNode toN = link.ToNode as IMetaNode;
            IMetaNode fromN = link.FromNode as IMetaNode;
            if (toN == null || fromN == null)
            {
                //TODO: you have right clicked on a link that is actually supposed to be a fishlink
                return;
            }
            if ((toN.MetaObject._ClassName != null) && (fromN.MetaObject._ClassName != null))
            {
                List<ClassAssociation> classAssociations = AssociationManager.Instance.GetAssociationsForParentAndChildClasses(fromN.MetaObject._ClassName, toN.MetaObject._ClassName);
                Array associationTypes = Enum.GetValues(typeof(LinkAssociationType));
                for (int i = 0; i < associationTypes.Length; i++)
                {
                    bool Found = false;
                    foreach (ClassAssociation allowedAssociation in classAssociations)
                    {
                        if (allowedAssociation.IsActive == true)
                            if (allowedAssociation.AssociationTypeID == (int)associationTypes.GetValue(i))
                                Found = true;
                    }
                    if (!Found)
                    {
                        LinkAssociationType linkToBeRemoved = (LinkAssociationType)associationTypes.GetValue(i);
                        RemoveMenuItem(cm, "Change to " + linkToBeRemoved.ToString().Replace("_", " "));
                    }
                }
            }
        }

        /// <summary>
        /// Adds the context menu items for link types.
        /// </summary>
        private void AddContextMenuItemsForLinkTypes()
        {
            Hashtable hashLinkTypes = new Hashtable();
            Array associationTypes = Enum.GetValues(typeof(LinkAssociationType));
            bool foundAnotherTypeOfAssociation = false;
            MenuItem mChange = new MenuItem("Change association...");
            for (int i = 0; i < associationTypes.Length; i++)
            {
                LinkAssociationType tp = (LinkAssociationType)associationTypes.GetValue(i);
                MenuItem newMenuItem = new MenuItem("Change to " + tp.ToString().Replace("_", " "), ChangeSelectedLinksAssociationType);
                newMenuItem.Tag = tp;
                if (!hashLinkTypes.ContainsKey(tp.ToString()))
                {
                    foundAnotherTypeOfAssociation = true;
                    mChange.MenuItems.Add(newMenuItem);
                    hashLinkTypes.Add(tp.ToString(), true);
                }
            }
            if (foundAnotherTypeOfAssociation && mChange.MenuItems.Count > 0)
                MyView.ViewContextMenu.MenuItems.Add(mChange);
        }

        /// <summary>
        /// Changes the type of the selected links association.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void ChangeSelectedLinksAssociationType(object sender, EventArgs e)
        {
            MyView.StartTransaction();
            foreach (QLink lnk in MyView.SelectedLinks)
            {
                IMetaNode nFrom = lnk.FromNode as IMetaNode;
                IMetaNode nTo = lnk.ToNode as IMetaNode;
                if (nFrom != null && nTo != null)
                {
                    LinkAssociationType oldType = lnk.AssociationType;

                    //find and change other shallow copy links]
                    List<QLink> extraLinks = new List<QLink>();
                    foreach (GoObject obj in MyView.Document)
                    {
                        if (obj is QLink && obj != lnk)
                        {
                            QLink otherLink = obj as QLink;
                            IMetaNode OnFrom = otherLink.FromNode as IMetaNode;
                            IMetaNode OnTo = otherLink.ToNode as IMetaNode;
                            if (OnFrom != null && OnTo != null)
                            {
                                if (OnFrom.MetaObject.pkid > 0 && OnTo.MetaObject.pkid > 0)
                                {
                                    if ((OnFrom.MetaObject.pkid == nFrom.MetaObject.pkid && OnFrom.MetaObject.MachineName == nFrom.MetaObject.MachineName) && (OnTo.MetaObject.pkid == nTo.MetaObject.pkid && OnTo.MetaObject.MachineName == nTo.MetaObject.MachineName)) //Super check
                                    {
                                        if (otherLink.AssociationType == oldType)
                                        {
                                            if (!MyView.Selection.Contains(otherLink)) //It must change because it was selected! :)
                                            {
                                                extraLinks.Add(otherLink);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    MenuItem mitem = sender as MenuItem;
                    bool result = true;
                    if (extraLinks.Count > 0)
                    {
                        //MESSAGE
                        //if no then false
                        string msg = extraLinks.Count == 1 ? "There is 1 link occurence on this diagram. Do you want to change that link as well?" : "There are " + extraLinks.Count + " link occurences on this diagram. Do you want to change those links as well?";
                        if (MessageBox.Show(msg, "Link Occurences", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            result = true;
                        else
                            result = false;

                        if (result)
                        {
                            foreach (QLink otherLink in extraLinks)
                            {
                                //Remove artefacts
                                foreach (GoObject o in otherLink.GetArtefacts())
                                    o.Remove();
                                foreach (GoObject o in otherLink.GetRationales())
                                    o.Remove();

                                otherLink.AssociationType = (LinkAssociationType)mitem.Tag;

                                if (otherLink != null)
                                {
                                    if (otherLink.ToNode is IMetaNode && otherLink.FromNode is IMetaNode)
                                    {
                                        //TestValidLink(l, false);

                                        IMetaNode from = otherLink.FromNode as IMetaNode;
                                        IMetaNode to = otherLink.ToNode as IMetaNode;
                                        if (from.MetaObject.pkid > 0 && to.MetaObject.pkid > 0)
                                        {
                                            //refresh link status
                                            foreach (ClassAssociation ca in AssociationManager.Instance.GetAssociationsForParentAndChildClasses(from.MetaObject.Class, to.MetaObject.Class))
                                            {
                                                if (ca.AssociationTypeID == (int)otherLink.AssociationType)
                                                {
                                                    try
                                                    {
                                                        ObjectAssociation objectAssociation = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(ca.CAid, from.MetaObject.pkid, to.MetaObject.pkid, from.MetaObject.MachineName, to.MetaObject.MachineName);
                                                        if (objectAssociation != null)
                                                        {
                                                            if (objectAssociation.State == VCStatusList.MarkedForDelete)
                                                            {
                                                                otherLink.RealLink.Pen = new Pen(Color.Red, 1f);
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
                                }
                            }
                        }
                    }

                    Association myAssoc = Singletons.GetAssociationHelper().GetAssociation(nFrom.MetaObject._ClassName, nTo.MetaObject._ClassName, (int)lnk.AssociationType);

                    if (myAssoc != null)
                    {
                        ObjectAssociationKey oakey = new ObjectAssociationKey();
                        oakey.CAid = myAssoc.ID;
                        oakey.ObjectID = nFrom.MetaObject.pkid;
                        oakey.ObjectMachine = nFrom.MetaObject.MachineName;
                        oakey.ChildObjectID = nTo.MetaObject.pkid;
                        oakey.ChildObjectMachine = nTo.MetaObject.MachineName;
                        ObjectAssociation oldAssoc = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Get(oakey);
                        if (oldAssoc != null)
                        {
                            //DialogResult res = DialogResult.No;
                            //MessageBox.Show(this,"Do you want to keep the previous association?", "Keep Association", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                            //if (res == DialogResult.No)
                            if (result)
                            {
                                if (VCStatusTool.DeletableFromDiagram(oldAssoc) && VCStatusTool.UserHasControl(oldAssoc))
                                {
                                    oldAssoc.VCStatusID = (int)VCStatusList.MarkedForDelete;
                                    oldAssoc.State = VCStatusList.MarkedForDelete;
                                    ObjectAssociation possible2Way = GetTwoWayAssociation(oldAssoc);
                                    DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Save(oldAssoc);

                                    if (possible2Way != null)
                                    {
                                        possible2Way.State = VCStatusList.MarkedForDelete;
                                        DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Save(possible2Way);
                                    }
                                }
                            }
                        }
                    }

                    //Remove artefacts
                    foreach (GoObject o in lnk.GetArtefacts())
                        o.Remove();
                    foreach (GoObject o in lnk.GetRationales())
                        o.Remove();

                    lnk.AssociationType = (LinkAssociationType)mitem.Tag;

                    if (lnk != null)
                    {
                        if (lnk.ToNode is IMetaNode && lnk.FromNode is IMetaNode)
                        {
                            //TestValidLink(l, false);

                            IMetaNode from = lnk.FromNode as IMetaNode;
                            IMetaNode to = lnk.ToNode as IMetaNode;
                            if (from.MetaObject.pkid > 0 && to.MetaObject.pkid > 0)
                            {
                                //refresh link status
                                foreach (ClassAssociation ca in AssociationManager.Instance.GetAssociationsForParentAndChildClasses(from.MetaObject.Class, to.MetaObject.Class))
                                {
                                    if (ca.AssociationTypeID == (int)lnk.AssociationType)
                                    {
                                        try
                                        {
                                            ObjectAssociation oa = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(ca.CAid, from.MetaObject.pkid, to.MetaObject.pkid, from.MetaObject.MachineName, to.MetaObject.MachineName);
                                            if (oa != null)
                                            {
                                                if (oa.State == VCStatusList.MarkedForDelete)
                                                {
                                                    lnk.RealLink.Pen = new Pen(Color.Red, 1f);
                                                }
                                            }
                                        }
                                        catch
                                        {
                                            //first time association created/change to :))
                                        }
                                    }
                                }
                            }
                        }
                    }
                    // 31 OCT lnk.ChangeAssociationType(sender, e);
                    // 31 OCT lnk.ChangeStyle();
                }
                else
                {
                    MessageBox.Show("One or both of the objects for this link are null", "Cannot change association");
                }
            }
            MyView.Selection.Clear();
            MyView.FinishTransaction("Change association type");
        }

        /// <summary>
        /// 
        /// THESE 2 FUNCTIONS EXIST ON TABLECONTAINER AS WELL, REFACTOR!
        /// </summary>
        /// <param name="objectAssociation"></param>
        /// <returns></returns>
        private ClassAssociation GetTwoWayClassAssociation(ObjectAssociation objectAssociation)
        {
            // Check if its a twoway association
            ClassAssociation classAssoc = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByCAid(objectAssociation.CAid);
            if (classAssoc.AssociationTypeID == 4)
            {
                TList<ClassAssociation> classAssocOtherWayRound = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByChildClass(classAssoc.ParentClass);
                classAssocOtherWayRound.Filter = "ParentClass = '" + classAssoc.ChildClass + "' AND AssociationTypeID = 4";
                if (classAssocOtherWayRound.Count > 0)
                {
                    return classAssocOtherWayRound[0];
                }
            }
            return null;
        }

        private ObjectAssociation GetTwoWayAssociation(ObjectAssociation obassoc)
        {
            ClassAssociation twoWayAssociation = GetTwoWayClassAssociation(obassoc);
            if (twoWayAssociation != null)
            {
                ObjectAssociationKey okey = new ObjectAssociationKey();
                okey.CAid = twoWayAssociation.CAid;
                okey.ChildObjectID = obassoc.ObjectID;
                okey.ChildObjectMachine = obassoc.ObjectMachine;
                okey.ObjectID = obassoc.ChildObjectID;
                okey.ObjectMachine = obassoc.ChildObjectMachine;
                ObjectAssociation otherWayAssoc = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Get(okey);
                return otherWayAssoc;
            }
            return null;
        }

        /// <summary>
        /// Compares the nodes on endpoints of the selected links
        /// </summary>
        /// <returns></returns>
        private LinkEndPointComparisonResult CompareSelectedLinksEndPointClasses()
        {
            bool ParentsEqual = true;
            bool ChildrenEqual = true;
            string parentClass = "";
            string childClass = "";
            foreach (QLink lnk in MyView.SelectedLinks)
            {
                if (lnk.FromNode != null && (lnk.FromNode as IMetaNode).HasBindingInfo)
                {
                    IMetaNode gParent = lnk.FromNode as IMetaNode;
                    if (parentClass.Length > 0)
                    {
                        if (gParent.BindingInfo.BindingClass != parentClass)
                        {
                            ParentsEqual = false;
                        }
                    }
                    else
                    {
                        parentClass = gParent.BindingInfo.BindingClass; // <- initialise the parent class
                    }
                    IMetaNode gChild = lnk.ToNode as IMetaNode;
                    if (childClass.Length > 0)
                    {
                        if (gChild.BindingInfo.BindingClass != childClass)
                        {
                            ChildrenEqual = false;
                        }
                    }
                    else
                    {
                        childClass = gChild.BindingInfo.BindingClass; // <- initialise the parent class
                    }

                    //if (gParent.MetaObject.State == VCStatusList.CheckedOutRead && gChild.MetaObject.State == VCStatusList.CheckedOutRead)
                    //{
                    //    return LinkEndPointComparisonResult.Neither;
                    //}
                }
            }
            if (ChildrenEqual && ParentsEqual)
            {
                return LinkEndPointComparisonResult.BothEQ;
            }
            if (ChildrenEqual)
            {
                return LinkEndPointComparisonResult.ChildrenEQ;
            }
            if (ParentsEqual)
            {
                return LinkEndPointComparisonResult.ParentsEQ;
            }
            return LinkEndPointComparisonResult.Neither;
        }

        /// <summary>
        /// Adds contextmenu items applicable to all objects
        /// </summary>
        private void AddContextMenuItemsForAllSelectedObjects()
        {
            bool shift = false;
            bool control = false;
            if ((Control.ModifierKeys & Keys.Shift) != 0)
                shift = true;
            if ((Control.ModifierKeys & Keys.Control) != 0)
                control = true;
            if (!(control || shift))
            {
                if (MyView.Selection.Count == 1 && (!(MyView.Selection.Primary.Parent is FrameLayerGroup)) && !(MyView.Selection.Primary is LegendItem) && !(MyView.Selection.Primary.Parent is LegendItem))
                {
                    if (MyView.Document is NormalDiagram)
                    {
                        MyView.ViewContextMenu.MenuItems.Add(new MenuItem("Select All Items of This Type", SelectAllItemsByType));
                        //#if DEBUG
                        //                        MyView.ViewContextMenu.MenuItems.Add(new MenuItem("Structured Report", GenerateStructuredReport));
                        //#endif
                    }
                }
                if (MyView.Selection.Count > 0 && (!(MyView.Selection.Primary.Parent is FrameLayerGroup)))
                {
                    bool somethingOtherThanConnectorsSelected = false;
                    GoCollectionEnumerator colEnum = MyView.Selection.GetEnumerator();
                    while (colEnum.MoveNext())
                    {
                        if (!(colEnum.Current is IGoLink))
                        {
                            somethingOtherThanConnectorsSelected = true;
                        }
                    }
                    if (somethingOtherThanConnectorsSelected)
                    {
                        //TODO : Check if subgraph/valuechain/swimlane
                        if (MyView.Selection.Primary.ParentNode is SubgraphNode || MyView.Selection.Primary.ParentNode is ValueChain || MyView.Selection.Primary.ParentNode is MappingCell || MyView.Selection.Primary.TopLevelObject is ValueChain)
                        {
                            MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("-"));
                            if (MyView.Selection.Primary is IMetaNode)
                                MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("&Delete", MarkForDelete));
                            MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("&Remove", Delete));
                            MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("&Cut", Cut));
                            MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("&Copy", Copy));
                            //MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("&Delete", new EventHandler(Delete)));
                            //MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("&Cut", new EventHandler(Cut)));
                            //MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("&Copy", new EventHandler(Copy)));
                        }
                        else
                        {
                            if (!ReadOnly)
                            {
                                if (MyView.Selection.Primary is LegendItem || MyView.Selection.Primary.Parent is LegendItem)
                                {
                                    LegendItem leg = null;
                                    if (MyView.Selection.Primary is LegendItem)
                                        leg = MyView.Selection.Primary as LegendItem;
                                    else if (MyView.Selection.Primary.Parent is LegendItem)
                                        leg = MyView.Selection.Primary.Parent as LegendItem;

                                    if (leg == null)
                                        return;

                                    MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("-"));
                                    MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("Make all diagram items visible", makeAllLegendItemsVisible));
                                    MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("Remove legend", removeLegend));
                                    MenuItem mnuItemRemoveItem = new MenuItem();
                                    mnuItemRemoveItem.Text = "Remove legend item";
                                    mnuItemRemoveItem.Tag = leg;
                                    mnuItemRemoveItem.Click += removeLegendItemFromLegend;
                                    MyView.ViewContextMenu.MenuItems.Add(0, mnuItemRemoveItem);
                                    //MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("Remove legend item", removeLegendItemFromLegend));
                                }
                                else
                                {
                                    MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("-"));
                                    if (MyView.Selection.Primary is IMetaNode)
                                        MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("&Delete", MarkForDelete));
                                    MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("&Remove", Delete));
                                    MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("&Cut", Cut));
                                    MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("&Copy", Copy));
                                }
                            }
                        }
                    }
                    else //Only links selected
                    {
                        MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("-"));
                        MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("&Remove", Delete));
                        MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("&Delete", MarkForDelete));
                    }
                }
                //if (MyView.Selection.Count == 0)
                {
                    if (!ReadOnly)
                    {
                        if (MyView.CanEditPaste())
                            MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("&Paste", Paste));
                    }
                }
            }

#if DEBUG
            //MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("-"));
            //MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("Create One Of Each Class With no stencil", CreateOneOfEachClassNoStencil));
            //MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("Create One Of Each Class", CreateOneOfEachClass));
            //MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("Display all associations", DisplayAllAssociations));
            //MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("Name All Ports", NameAllPorts));
            //MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("-"));
            //MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("Align Selection Horizontally ", AlignSelectedObjectsHorizontally));
            //MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("Align Selection Vertically", AlignSelectedObjectsVertically));
            //MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("Distribute Selection Horizantally", DistributeHorizontally));
            //MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("Distribute Selection Vertically", DistributeVertically));
            //MyView.ViewContextMenu.MenuItems.Add(0, new MenuItem("-"));
#endif

        }

        private void NodeProperties(object sender, EventArgs e)
        {
            foreach (GoObject o in myView.Selection)
            {
                if (!(o is GraphNode))
                    continue;
                GraphNode node = o as GraphNode;
                node.InitMetaProps(true);
            }
        }

        private void AlignSelectedObjectsHorizontally(object sender, EventArgs e)
        {
            float smallestY = -1f;
            foreach (GoObject o in MyView.Selection)
            {
                if (smallestY == -1)
                    smallestY = o.Location.Y;
                else if (smallestY > o.Location.Y)
                    smallestY = o.Location.Y;
            }

            foreach (GoObject o in MyView.Selection)
            {
                o.Location = new PointF(o.Location.X, smallestY);
            }
        }
        private void AlignSelectedObjectsVertically(object sender, EventArgs e)
        {
            float smallestX = -1f;
            foreach (GoObject o in MyView.Selection)
            {
                if (smallestX == -1)
                    smallestX = o.Location.X;
                else if (smallestX > o.Location.X)
                    smallestX = o.Location.X;
            }

            foreach (GoObject o in MyView.Selection)
            {
                o.Location = new PointF(smallestX, o.Location.Y);
            }
        }
        private void DistributeHorizontally(object sender, EventArgs e)
        {
            float spacing = 20;
            GoObject first = null;
            float number = 0;
            float totalwidth = 0;
            float thisSpace = 0;
            foreach (GoObject o in MyView.Selection)
            {
                thisSpace = (float)number * spacing;
                if (first == null)
                {
                    first = o;
                    spacing = o.Width;
                }
                else
                {
                    o.Location = new PointF(first.Location.X + totalwidth + thisSpace, o.Location.Y);
                }
                totalwidth += o.Width;
                number++;
            }
        }
        private void DistributeVertically(object sender, EventArgs e)
        {
            float spacing = 20;
            GoObject first = null;
            GoObject x = first;
            float number = 0;
            float totalheight = 0;
            float thisSpace = 0;
            foreach (GoObject o in MyView.Selection)
            {
                thisSpace = (float)number * spacing;
                if (first == null)
                {
                    first = o;
                    spacing = o.Height;
                }
                else
                {
                    o.Location = new PointF(o.Location.X, first.Location.Y + totalheight + thisSpace);
                }
                totalheight += o.Height;
                number++;
            }
        }
        private void NameAllPortsActual(GoObject o)
        {
            if (o is QuickPort)
            {
                if ((o as QuickPort).PortPosition != MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Circumferential)
                {
                    //GoBalloon t = new GoBalloon();
                    GoText t = new GoText();
                    t.Text = (o as QuickPort).PortPosition.ToString();
                    t.Location = o.Location;
                    t.FontSize = 5;
                    MyView.Document.Add(t);
                    return;
                }
            }
            else if (o is IGoCollection)
            {
                foreach (GoObject oo in (o as IGoCollection))
                    NameAllPortsActual(oo);
            }
        }
        private void NameAllPorts(object sender, EventArgs e)
        {
            foreach (GoObject o in MyView.Document)
                NameAllPortsActual(o);
        }
        private void CreateOneOfEachClass(object sender, EventArgs e)
        {
            ShapeDesignController sdc = new ShapeDesignController(MyView);
            TList<Class> classes = DataRepository.Classes(Core.Variables.Instance.ClientProvider);//DataRepository.Classes(Core.Variables.Instance.ClientProvider);//d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassProvider.GetAll();
            classes.Sort("Name Desc");
            foreach (Class c in classes)
            {
                if (c.IsActive == true)
                {
                    sdc.AddRationale(Loader.CreateInstance(c.Name));
                }
            }
        }
        private void CreateOneOfEachClassNoStencil(object sender, EventArgs e)
        {
            ShapeDesignController sdc = new ShapeDesignController(MyView);
            TList<Class> classes = d.DataRepository.Classes(Core.Variables.Instance.ClientProvider);//.Provider.ClassProvider.GetAll();
            classes.Sort("Name Desc");
            foreach (Class c in classes)
            {
                if (Variables.Instance.ClassesWithoutStencil.Contains(c.Name))
                {
                    if (c.IsActive == true)
                    {
                        sdc.AddRationale(Loader.CreateInstance(c.Name));
                    }
                }
            }
        }
        private void DisplayAllAssociations(object sender, EventArgs e)
        {
            DisplayAllAssociationsThread();
            //Thread t = new Thread(new ThreadStart(DisplayAllAssociationsThread));
            //t.Start();
        }
        private void DisplayAllAssociationsThread()
        {
            int x = 100;
            int y = 100;
            TList<Class> classes = DataRepository.Classes(Core.Variables.Instance.ClientProvider);//d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassProvider.GetAll();
            TList<AssociationType> types = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.AssociationTypeProvider.GetAll();
            classes.Sort("Name");
            foreach (Class c in classes)
            {
                GoComment com = new GoComment();
                com.Text = "Associations with " + c.Name + " as parent";
                com.Location = new PointF(x + 20, y - 20);
                MyView.Document.Add(com);

                if (c.IsActive != true)
                    continue;

                MetaBase mBase = Loader.CreateInstance(c.Name);
                mBase.Set(mBase.GetMetaPropertyList(false)[0].Name, c.Name);
                SubgraphNode sg = new SubgraphNode();
                sg.BindingInfo = new BindingInfo();
                sg.BindingInfo.BindingClass = mBase.Class;
                sg.Location = new PointF(x, y);
                y += 100;
                sg.MetaObject = mBase;
                sg.Size = new SizeF(50, 50);
                sg.HookupEvents();
                sg.BindToMetaObjectProperties();
                MyView.Document.Add(sg);

                TList<ClassAssociation> associations = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByParentClass(c.Name);
                associations.Sort("ChildClass, AssociationTypeID");
                foreach (ClassAssociation association in associations)
                {
                    if (association.IsActive != true)
                        continue;

                    if (classes.Find(ClassColumn.Name, association.ChildClass).IsActive != true)
                        continue;

                    MetaBase mBaseChild = Loader.CreateInstance(association.ChildClass);
                    if (mBaseChild == null)
                        continue;

                    mBaseChild.Set(mBaseChild.GetMetaPropertyList(false)[0].Name, classes.Find(ClassColumn.Name, association.ChildClass).Name);

                    SubgraphNode sgChild = new SubgraphNode();
                    sgChild.BindingInfo = new BindingInfo();
                    sgChild.BindingInfo.BindingClass = mBaseChild.Class;
                    sgChild.Location = new PointF(x + 175, y);
                    sgChild.MetaObject = mBaseChild;
                    sgChild.Size = new SizeF(50, 50);
                    sgChild.HookupEvents();
                    sgChild.BindToMetaObjectProperties();
                    MyView.Document.Add(sgChild);

                    QLink link = QLink.CreateLink(sg as GoNode, sgChild as GoNode, association.AssociationTypeID, -1);
                    if (link != null)
                    {
                        MyView.Document.Add(link);
                        GoBalloon Acom = new GoBalloon();
                        Acom.Text = types.Find(AssociationTypeColumn.pkid, association.AssociationTypeID).Name.Trim();
                        Acom.Location = link.MidLabel.Location;
                        Acom.Anchor = link;
                        MyView.Document.Add(Acom);
                    }

                    y += 100;
                }
                y = 100;
                x += 350;
            }
        }

        #region Cut Copy Paste Delete

        private void Cut(object sender, EventArgs e)
        {
            //removeQuickMenu();
        }

        private void Copy(object sender, EventArgs e)
        {
            //removeQuickMenu();
            MyView.EditCopy();
        }

        private void Paste(object sender, EventArgs e)
        {
            MyView.EditPaste();
        }

        private void Delete(object sender, EventArgs e)
        {
            GoCollection col = new GoCollection();
            foreach (GoObject o in MyView.Selection)
            {
                if (o is CollapsingRecordNodeItem)
                {
                    col.Add(o.ParentNode);
                }
            }
            MyView.EditDelete();
            ForceComputeCollectionBounds(col);
        }
        public void ForceComputeCollectionBounds(ICollection col)
        {
            foreach (GoObject o in col)
            {
                if (o is CollapsibleNode)
                    (o as CollapsibleNode).ForceComputeBounds();
                //else
                //    o.ToString();
            }
        }
        private void MarkForDelete(object sender, EventArgs e)
        {
            ModifyObjects(MyView.Selection, true, null);
            Delete(sender, e);
        }

        #endregion

        #region ContextMenu management

        //private Timer t;

        private void MyView_ObjectContextClicked(object sender, GoObjectEventArgs evt)
        {
            MyView.ViewContextMenu.Dispose();
            MyView.ViewContextMenu = null;
            MyView.ViewContextMenu = new GoContextMenu(MyView);

            if (evt.GoObject.ParentNode is IMetaNode)
            {
                VCStatusList currentState = (evt.GoObject.ParentNode as IMetaNode).MetaObject.State;
                if (currentState == VCStatusList.CheckedIn || currentState == VCStatusList.CheckedOutRead || currentState == VCStatusList.Locked || currentState == VCStatusList.Obsolete)
                {
                    AddContextMenuItemsForReadOnly();
                    MyView.ViewContextMenu.Show(MyView, evt.ViewPoint);
                    return;
                }
            }

            //MyView.ViewContextMenu.MenuItems.Clear();
            /*for (int i = 0; i < MyView.ViewContextMenu.MenuItems.Count; i++)
            {
                if (MyView.ViewContextMenu.MenuItems[i].Text != "Paste Shallow Copy")
                {
                    MyView.ViewContextMenu.MenuItems.RemoveAt(i);
                }
            }*/

            if ((MyView.LastInput.Modifiers != (Keys.Shift | Keys.Control)))
            {
                if (!ReadOnly)
                {
                    AddFrameContextMenu();
                    AddSubgraphContextMenu();
                    AddContextMenuItemsForNodes();
                    AddContextMenuItemsForBehaviours();
                    AddContextMenuItemsForLinks();
                    AddContextMenuItemsForAllSelectedObjects();
                    AddContextMenuItemsForBalloons();
                    AddContextMenuItemForMetaNodes();
                }
                else
                {
                    AddContextMenuItemsForReadOnly();
                    if (Core.Variables.Instance.IsViewer)
                    {
                        AddContextMenuItemsForBalloons();
                    }
                }

                //MyView.ViewContextMenu.Show(MyView, evt.ViewPoint);
            }
        }

        private void MyView_BackgroundContextClicked(object sender, GoInputEventArgs e)
        {
            if ((e.Modifiers != Keys.Shift) && (e.Modifiers != Keys.Control))
                AddFrameContextMenu();
        }

        private void mnuRemoveFromSubgraph_Clicked(object sender, EventArgs e)
        {
            MenuItem mnu = sender as MenuItem;
            GoCollection col = mnu.Tag as GoCollection;
            RemoveCollectionFromILinkedContainer(col);
        }

        public void RemoveCollectionFromILinkedContainer(GoCollection col)
        {
            GoObject oFirst = col.First;

            ILinkedContainer sgnode = GetContainingILinkedContainer(oFirst);
            Dictionary<GoObject, int> objsToBeRemoved = new Dictionary<GoObject, int>();
            GoCollectionEnumerator colEnum = col.GetEnumerator();
            Dictionary<ResizableBalloonComment, GoObject> comments = new Dictionary<ResizableBalloonComment, GoObject>();
            colEnum.Reset();
            while (colEnum.MoveNext())
            {
                if (colEnum.Current is ResizableBalloonComment)
                {
                    ResizableBalloonComment r = colEnum.Current as ResizableBalloonComment;
                    if (!comments.ContainsKey(r))
                    {
                        comments.Add(r, r.Anchor);
                    }
                }

                foreach (GoObject obs in colEnum.Current.Observers)
                {
                    if (obs is ResizableBalloonComment)
                    {
                        ResizableBalloonComment r = obs as ResizableBalloonComment;
                        if (!comments.ContainsKey(r))
                        {
                            comments.Add(r, r.Anchor);
                        }
                    }
                }

                objsToBeRemoved.Add(colEnum.Current, 0);
            }
            RemoveItemsFromILinkedContainer(objsToBeRemoved, sgnode);

            foreach (KeyValuePair<ResizableBalloonComment, GoObject> kvp in comments)
            {
                kvp.Key.Anchor = kvp.Value;
            }
        }

        public void RemoveItemsFromILinkedContainer(Dictionary<GoObject, int> objsToBeRemoved, ILinkedContainer sgnode)
        {
            List<LinkPortSpec> linkSpecs = GroupingControl.GetLinkSpecs(objsToBeRemoved);
            RectangleF containerBounds = sgnode.Bounds;

            #region Remove Objects

            foreach (KeyValuePair<GoObject, int> kvp in objsToBeRemoved)
            {
                if (kvp.Key is IMetaNode)
                {
                    ILinkedContainer MySubgraphNode = sgnode;
                    List<EmbeddedRelationship> relationshipsToRemove = new List<EmbeddedRelationship>();
                    foreach (EmbeddedRelationship relToRemove in sgnode.ObjectRelationships)
                    {
                        if (relToRemove.MyMetaObject == (kvp.Key as IMetaNode).MetaObject)
                        {
                            relationshipsToRemove.Add(relToRemove);
                        }
                    }

                    for (int i = 0; i < relationshipsToRemove.Count; i++)
                    {
                        EmbeddedRelationship relToRemove = relationshipsToRemove[i];
                        if (relToRemove.MyAssociation != null)
                        {
                            ClassAssociation selectedAssociation = relToRemove.MyAssociation;

                            ObjectAssociationKey oakey = new ObjectAssociationKey();
                            oakey.CAid = selectedAssociation.CAid;
                            oakey.ObjectID = MySubgraphNode.MetaObject.pkid;
                            oakey.ObjectMachine = MySubgraphNode.MetaObject.MachineName;
                            oakey.ChildObjectID = relToRemove.MyMetaObject.pkid;
                            oakey.ChildObjectMachine = relToRemove.MyMetaObject.MachineName;

                            ObjectAssociation oa = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Get(oakey);

                            if (oa != null)
                            {
                                if (VCStatusTool.UserHasControl(oa))
                                {
                                    RemoveObjectByRelationship(relToRemove, MySubgraphNode);
                                    DialogResult res = DialogResult.No;
                                    if (kvp.Value == 1)
                                    {
                                        res = DialogResult.Yes;
                                    }
                                    if (kvp.Value == 0)
                                    {
                                        res = MessageBox.Show("Do you want to mark the relationship for delete?", "Mark for delete association", MessageBoxButtons.YesNo);
                                    }
                                    if (kvp.Value == 2)
                                    {
                                        res = DialogResult.No;
                                    }

                                    if (res == DialogResult.Yes)
                                    {
                                        if (VCStatusTool.DeletableFromDiagram(oa) && VCStatusTool.UserHasControl(oa))
                                        {
                                            oa.VCStatusID = (int)VCStatusList.MarkedForDelete;
                                            oa.State = VCStatusList.MarkedForDelete;
                                            DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Save(oa);
                                        }
                                        ////TODO : Mark Object for delete?
                                        //MetaObject mo = DataRepository.MetaObjectProvider.GetBypkidMachine(oa.ChildObjectID, oa.ChildObjectMachine);
                                        //if (mo != null)
                                        //{
                                        //    IMetaNode node = kvp.Key as IMetaNode;
                                        //    node.MetaObject = Loader.GetByID(mo.Class, mo.pkid, mo.Machine);
                                        //    if (VCStatusTool.DeletableFromDiagram(node.MetaObject) && VCStatusTool.UserHasControl(node.MetaObject))
                                        //    {
                                        //        mo.VCStatusID = (int)VCStatusList.MarkedForDelete;
                                        //        DataRepository.MetaObjectProvider.Save(mo);
                                        //    }
                                        //}
                                    }
                                }
                            }
                        }
                    }
                }
                sgnode.Remove(kvp.Key);
                kvp.Key.Position = new PointF(containerBounds.X + containerBounds.Width + (kvp.Key.Position.X - containerBounds.X), kvp.Key.Position.Y);
                //    new PointF(sgnode.Bounds.Width + kvp.Key.Position.X,kvp.Key.Position.Y);
                MyView.Document.Add(kvp.Key);
            }

            #endregion

            #region Relink

            foreach (LinkPortSpec lps in linkSpecs)
            {
                lps.Relink();
                if (lps.Link is FishLink)
                {
                    FishLink fl = lps.Link as FishLink;
                    MyView.Document.Add(fl);
                    fl.UpdateRoute();
                }
                if (lps.Link is QLink)
                {
                    QLink sl = lps.Link as QLink;
                    MyView.Document.Add(sl);
                    sl.UpdateRoute();
                }
                lps.AddFishLinks(MyView.Document);
            }

            #endregion

            MyView.ResumeLayout();

            // KEEP EXISTING ASSOCIATIONS?
            bool hasAssociations = false;
            List<EmbeddedRelationship> emrel = new List<EmbeddedRelationship>();
            foreach (KeyValuePair<GoObject, int> obj in objsToBeRemoved)
            {
                if (obj.Key is IMetaNode)
                {
                    IMetaNode mnode = obj.Key as IMetaNode;
                    if (sgnode.MetaObject != null)
                    {
                        foreach (EmbeddedRelationship rel in sgnode.ObjectRelationships)
                        {
                            if (rel.MyMetaObject == mnode.MetaObject)
                            {
                                emrel.Add(rel);
                                hasAssociations = true;
                            }
                        }
                    }
                }
            }
            if (hasAssociations)
            {
                DialogResult res = DialogResult.No;
                //MessageBox.Show(this,"Keep previous associations?", "Keep Associations", MessageBoxButtons.YesNo);

                if (res == DialogResult.No)
                {
                    foreach (EmbeddedRelationship rel in emrel)
                    {
                        ObjectAssociation objrel = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(rel.MyAssociation.CAid, sgnode.MetaObject.pkid, rel.MyMetaObject.pkid, sgnode.MetaObject.MachineName, rel.MyMetaObject.MachineName);
                        if (objrel != null)
                        {
                            if (VCStatusTool.DeletableFromDiagram(objrel) && VCStatusTool.UserHasControl(objrel))
                            {
                                objrel.VCStatusID = (int)VCStatusList.MarkedForDelete;
                                objrel.State = VCStatusList.MarkedForDelete;
                                DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Save(objrel);
                            }
                        }
                    }
                }
            }
        }

        private void RemoveObjectByRelationship(EmbeddedRelationship relToRemove, ILinkedContainer MySubgraphNode)
        {
            List<EmbeddedRelationship> itemsToRemove = new List<EmbeddedRelationship>();
            if (MySubgraphNode != null)
            {
                foreach (EmbeddedRelationship relship in MySubgraphNode.ObjectRelationships)
                {
                    if (relship == relToRemove)
                    {
                        itemsToRemove.Add(relship);
                    }
                }
            }

            for (int i = 0; i < itemsToRemove.Count; i++)
            {
                MySubgraphNode.ObjectRelationships.Remove(itemsToRemove[i]);
                MySubgraphNode.RemoveByRelationship(itemsToRemove[i]);
            }
        }

        private ILinkedContainer GetContainingILinkedContainer(GoObject obj)
        {
            GoObject objToInspect = obj;
            while (objToInspect.Parent != null)
            {
                if (objToInspect.Parent is ILinkedContainer)
                    return objToInspect.Parent as ILinkedContainer;

                objToInspect = objToInspect.Parent;
            }
            return null; // if we've come this far, there aint no subgraphcontainer!
        }

        private void AddSubgraphContextMenu()
        {
            if (MyView.Selection.Count > 0)
            //&& MyView.Selection.Primary.TopLevelObject != null)
            {
                IGoCollection selectedObj = MyView.Selection;
                ILinkedContainer sgn = GetContainingILinkedContainer(MyView.Selection.Primary);
                if (sgn != null && !(sgn is MappingCell))
                {
                    string containertype = (sgn is SubgraphNode) ? "Bound SubGraph" : (sgn is ValueChain) ? "ValueChain" : "Container";
                    if (MyView.Selection.Primary != sgn)
                    {
                        // if (MyView.Selection.Primary is GoNode)
                        {
                            MenuItem mnuSep = new MenuItem("-");
                            MenuItem mnuRemoveFromSubgraph = new MenuItem("Remove From " + containertype, mnuRemoveFromSubgraph_Clicked);
                            mnuRemoveFromSubgraph.Tag = selectedObj;
                            MyView.ViewContextMenu.MenuItems.Add(mnuRemoveFromSubgraph);
                            MyView.ViewContextMenu.MenuItems.Add(mnuSep);
                        }
                    }
                }
            }
        }

        private void AddFrameContextMenu()
        {
            if (MyView.Selection.Primary == null)
            {
                MenuItem mnuItemSelectFrame = new MenuItem("Select Frame", SelectFrame);
                MyView.ViewContextMenu.MenuItems.Add(mnuItemSelectFrame);
                MenuItem mnuItemRemoveHighlights = new MenuItem("Remove Highlights", RemoveHighlights);
                MyView.ViewContextMenu.MenuItems.Add(mnuItemRemoveHighlights);
            }
            else
            {
                if (MyView.Document is NormalDiagram)
                {
                    if ((MyView.Document as NormalDiagram).DocumentFrame != null)
                    {
                        if (MyView.Selection.Contains((MyView.Document as NormalDiagram).DocumentFrame.Frame))
                        {
                            MenuItem mnuItemDeselectFrame = new MenuItem("Deselect Frame", SelectFrame);
                            MyView.ViewContextMenu.MenuItems.Add(mnuItemDeselectFrame);
                        }
                    }
                }
            }
        }

        private void SelectFrame(object sender, EventArgs e)
        {
            if (MyView.Document is NormalDiagram)
                SwitchFrame(MyView.Document as NormalDiagram);
        }

        private void RemoveHighlights(object sender, EventArgs e)
        {
            RemoveHighlights();
        }

        private void AddContextMenuItemsForBalloons()
        {
            if (MyView.Selection.Count == 1)
            {
                MenuItem mnuItemAnchoredComment = new MenuItem("Add Comment", AddComment);
                mnuItemAnchoredComment.Tag = MyView.Selection.Primary;
                MyView.ViewContextMenu.MenuItems.Add(mnuItemAnchoredComment);

                if (!Core.Variables.Instance.IsViewer && myView.Selection.Primary is IMetaNode)
                {
                    MenuItem mnuItemRationale = new MenuItem("Add Rationale", AddRationale);
                    mnuItemRationale.Tag = MyView.Selection.Primary;
                    MyView.ViewContextMenu.MenuItems.Add(mnuItemRationale);
                }
            }
            else
            {
                MenuItem mnuItemComment = new MenuItem("Add Comment", AddComment);
                mnuItemComment.Tag = null;
                MyView.ViewContextMenu.MenuItems.Add(mnuItemComment);
            }
#if DEBUG
            MenuItem mnuItemProperty = new MenuItem("Add PropertyNode", AddProperty);
            mnuItemProperty.Tag = null;
            MyView.ViewContextMenu.MenuItems.Add(mnuItemProperty);
#endif
        }
        //Readonly context menu items
        private void AddContextMenuItemsForReadOnly()
        {
            if (MyView.Selection.Count == 1) //can only view 1 object at a time
            {
                if (MyView.Selection.Primary.ParentNode is IMetaNode)
                {
                    //if (MyView.Selection.Primary.ParentNode is CollapsingRecordNodeItem) //dont display menu for headers
                    //    if ((MyView.Selection.Primary.ParentNode as CollapsingRecordNodeItem).IsHeader)
                    //        return;
                    IMetaNode cxnode = MyView.Selection.Primary.ParentNode as IMetaNode;

                    MenuItem mnuViewInContext = new MenuItem("View In Context", mnuViewInContext_Click);
                    mnuViewInContext.Tag = cxnode.MetaObject;

                    if (cxnode.MetaObject.pkid > 0 && cxnode.MetaObject.MachineName != null)
                    {
                        MenuItem mnuViewInContextArtefact = new MenuItem("Client Workspaces", mnuViewInContext_Click);
                        mnuViewInContextArtefact.Tag = cxnode.MetaObject;
                        mnuViewInContext.MenuItems.Add(mnuViewInContextArtefact);
                    }

                    if (Variables.Instance.IsServer)
                    {
                        MenuItem mnuViewInContextServer = new MenuItem("Server Workspaces", mnuViewInContextServer_Click);
                        mnuViewInContextServer.Tag = cxnode.MetaObject;
                        mnuViewInContext.MenuItems.Add(mnuViewInContextServer);
                    }

                    if (DataAccessLayer.DataRepository.Connections.ContainsKey(Core.Variables.Instance.ClientProvider))
                        MyView.ViewContextMenu.MenuItems.Add(mnuViewInContext);
                }
            }
        }
        private void AddContextMenuItemsForNodes()
        {
            bool shift = false;
            bool control = false;
            if ((Control.ModifierKeys & Keys.Shift) != 0)
                shift = true;
            if ((Control.ModifierKeys & Keys.Control) != 0)
                control = true;

            if (MyView.SelectedNodes.Count == 1 && MyView.Selection.Count == 1)
            {
                IMetaNode node = MyView.SelectedNodes[0];
                if (node is IMetaNode || node is ILinkedContainer || node is ImageNode)
                {
                    if (node.MetaObject.Class == "Activity" || node.MetaObject.Class == "Gateway")
                    {
                        //AllocationHandle handle = null;
                        //if (node is GraphNode)
                        //{
                        //    foreach (GoObject o in node as GraphNode)
                        //    {
                        //        if (o is AllocationHandle)
                        //        {
                        //            handle = o as AllocationHandle;
                        //            break;
                        //        }
                        //    }
                        //}
                        //if (handle != null && handle.Items.Count > 0)
                        //{
                        MenuItem mnuActivityReport = new MenuItem("Accountability Matrix");
                        //mnuActivityReport.Tag = handle;
                        MyView.ViewContextMenu.MenuItems.Add(mnuActivityReport);
                        //}

                    }
                    //int linkcount = 0;
                    //if (node is IMetaNode)
                    //{
                    //    linkcount = (node as IMetaNode).Links.Count;
                    //}
                    //else
                    //    linkcount = -1; //how many children it may or may not have

                    //if (linkcount != 0)
                    //{
                    MenuItem mnuHighlightConnectedObjects = new MenuItem("Highlight Connected Objects", HighlightConnectedObjects);
                    mnuHighlightConnectedObjects.Tag = node;
                    MyView.ViewContextMenu.MenuItems.Add(mnuHighlightConnectedObjects);
                    //}
                    if (node != null && node.MetaObject != null && node.MetaObject.pkid > 0)
                    {
                        MenuItem mnuHighlightShallowCopies = new MenuItem("Highlight Shallow Copies", HighlightShallowCopies);
                        mnuHighlightShallowCopies.Tag = node;
                        MyView.ViewContextMenu.MenuItems.Add(mnuHighlightShallowCopies);
                    }
                }

                if (Core.Variables.Instance.ShowDeveloperItems)
                {
                    MenuItem mnuLayoutFSD = new MenuItem("FSD Layout", LayoutFSD);
                    mnuLayoutFSD.Tag = MyView.Selection.First as GoNode;
                    MyView.ViewContextMenu.MenuItems.Add(mnuLayoutFSD);
                }

                if (MyView.Doc.FileType == FileTypeList.Diagram && !(control || shift))
                {
                    #region Shallow Copies, View In Context, MetaModel Rules

                    if (node.HasBindingInfo && (!(node is ArtefactNode)))
                    {
                        if (node.MetaObject != null)
                        {
                            if (node.MetaObject.MachineName != null)
                            {
                                if (node is GraphNode || node is ImageNode)
                                {
                                    MenuItem mnuShallowCopy = new MenuItem("Create Shallow Copy", ShallowCopy);
                                    //MenuItem mnuShallowCopyClipboard = new MenuItem("Add Shallow Copy to Clipboard", ShallowCopyClipboard);
                                    MenuItem mnuShallowCopyClipboard = new MenuItem("Add Shallow Copy to Clipboard");

                                    mnuShallowCopy.Tag = MyView.SelectedNodes[0];
                                    mnuShallowCopyClipboard.Tag = MyView.SelectedNodes[0];
                                    MyView.ViewContextMenu.MenuItems.Add(mnuShallowCopy);
                                    MyView.ViewContextMenu.MenuItems.Add(mnuShallowCopyClipboard);

                                    MenuItem mnuUnboundText = new MenuItem("Add Unbound Text", UnBoundText);
                                    mnuUnboundText.Tag = MyView.SelectedNodes[0];
                                    if (Variables.Instance.ShowDeveloperItems)
                                        MyView.ViewContextMenu.MenuItems.Add(mnuUnboundText);
                                }
                                //28 january 2014 parented items
                                MenuItem mnuViewInContextParent = new MenuItem("View In Context", mnuViewInContext_Click);
                                mnuViewInContextParent.Tag = node.MetaObject;
                                MenuItem mnuViewInContext = new MenuItem("Client Workspaces", mnuViewInContext_Click);
                                mnuViewInContext.Tag = node.MetaObject;
                                mnuViewInContextParent.MenuItems.Add(mnuViewInContext);
                                if (Variables.Instance.IsServer)
                                {
                                    MenuItem mnuViewInContextServer = new MenuItem("Server Workspaces", mnuViewInContextServer_Click);
                                    mnuViewInContextServer.Tag = node.MetaObject;
                                    mnuViewInContextParent.MenuItems.Add(mnuViewInContextServer);
                                }
                                MyView.ViewContextMenu.MenuItems.Add(mnuViewInContextParent);

                                if (node.MetaObject.pkid != 0)
                                {
                                    //Hierarchical export context
                                    //MyView.ViewContextMenu.MenuItems.Add(new MenuItem("-"));
                                    MenuItem mnuExportText = new MenuItem("Export");
                                    MenuItem mnuExportTextTabbed = new MenuItem("Hierarchical Text (Tabbed)", ExportTextTabbed);
                                    MenuItem mnuExportTextNumbered = new MenuItem("Hierarchical Text (Numbered)", ExportTextNumbered);
                                    MenuItem mnuExportTextBoth = new MenuItem("Hierarchical Text (Tabbed And Numbered)", ExportTextBoth);
                                    mnuExportText.MenuItems.Add(mnuExportTextTabbed);
                                    mnuExportText.MenuItems.Add(mnuExportTextNumbered);
                                    mnuExportText.MenuItems.Add(mnuExportTextBoth);
                                    MyView.ViewContextMenu.MenuItems.Add(mnuExportText);
                                    //MyView.ViewContextMenu.MenuItems.Add(new MenuItem("-"));
                                }
                            }
                            if (node is GraphNode || node is ImageNode)
                            {
                                MenuItem mnuRules = new MenuItem("Allowed Connections");
                                List<ClassAssociation> allowedAssociations = AssociationManager.Instance.GetAssociationForClass(node.BindingInfo.BindingClass);
                                allowedAssociations.Sort(new ClassAssociationComparer());
                                List<string> tofromAID = new List<string>();
                                foreach (ClassAssociation assoc in allowedAssociations)
                                {
                                    if (assoc.IsActive == false)
                                        continue;
                                    if (assoc.AssociationTypeID == (int)LinkAssociationType.CreateOLD)
                                        continue;

                                    string tfa = assoc.ParentClass + assoc.ChildClass + assoc.AssociationTypeID.ToString();
                                    if (tofromAID.Contains(tfa))
                                        continue;
                                    else
                                        tofromAID.Add(tfa);

                                    MenuItem mnuAllowed = new MenuItem(assoc.ChildClass + " (" + ((LinkAssociationType)assoc.AssociationTypeID).ToString() + ")");
                                    mnuRules.MenuItems.Add(mnuAllowed);
                                }
                                MyView.ViewContextMenu.MenuItems.Add(mnuRules);
                                if (node.MetaObject.Class == "Function" && Variables.Instance.ShowDeveloperItems)
                                {
                                    //allow convert to valuechainstep
                                    MenuItem mnuFunctionShapeConvert = new MenuItem("Convert to Value Chain Step", convertToValueChainStep);
                                    mnuFunctionShapeConvert.Tag = node;
                                    MyView.ViewContextMenu.MenuItems.Add(mnuFunctionShapeConvert);
                                }
                            }
                        }
                    }
                    if (!(node is MappingCell))
                    {
                        MyView.ViewContextMenu.MenuItems.Add(new MenuItem("-"));
                        MenuItem mnuSelectChildren = new MenuItem("Select Tree", SelectChildNodes);
                        mnuSelectChildren.Tag = MyView.SelectedNodes[0];
                        MyView.ViewContextMenu.MenuItems.Add(mnuSelectChildren);
                    }

                    #endregion

                    if (AutoLinkingEnabled)
                    {
                        if (node is GraphNode)
                        {
                            MenuItem mnuSetAsParent = new MenuItem("Set as Auto-Link Parent", SetAutoLinkParent);
                            mnuSetAsParent.Tag = MyView.SelectedNodes[0];
                            MyView.ViewContextMenu.MenuItems.Add(mnuSetAsParent);
                        }
                    }
                    if (RubberStampEnabled)
                    {
                        if (node is GraphNode)
                        {
                            MenuItem mnuSetAsRubberStampProtoType = new MenuItem("Set as RubberStamp", SetAsRubberStampProtoType);
                            mnuSetAsRubberStampProtoType.Tag = MyView.SelectedNodes[0];
                            MyView.ViewContextMenu.MenuItems.Add(mnuSetAsRubberStampProtoType);
                        }
                    }
                    // Allow user to select an internal label
                    if (node.HasBindingInfo && node is GraphNode)
                    {
                        if (node.BindingInfo.Bindings.Count > 0)
                        {
                            MenuItem mnuSelectLabel = new MenuItem("Select Label");
                            MyView.ViewContextMenu.MenuItems.Add(mnuSelectLabel);
                            List<string> labels = new List<string>();
                            foreach (KeyValuePair<string, string> kvp in node.BindingInfo.Bindings)
                            {
                                labels.Add(kvp.Value + "|" + kvp.Key);
                            }
                            labels.Sort();
                            char splitChar = '|';
                            for (int i = 0; i < labels.Count; i++)
                            {
                                string[] vals = labels[i].Split(splitChar);
                                string lblName = vals[1];
                                MenuItem mnuSelectInternalLabel = new MenuItem(node.BindingInfo.Bindings[lblName], SelectInternalLabel);

                                GraphNode gnode = node as GraphNode;
                                IIdentifiable identifiable = gnode.FindByName(lblName);
                                if (identifiable is BoundLabel)
                                {
                                    BoundLabel lbl = identifiable as BoundLabel;
                                    mnuSelectInternalLabel.Tag = lbl;
                                    mnuSelectLabel.MenuItems.Add(mnuSelectInternalLabel);
                                }
                            }
                        }
                        //#if DEBUG
                        if (!(node is CollapsibleNode))
                        {
                            MenuItem metapropmenuitem = new MenuItem("Meta Properties", NodeProperties);
                            metapropmenuitem.Checked = ((node as GraphNode).MetaPropList != null && (node as GraphNode).MetaPropList.Visible);
                            MyView.ViewContextMenu.MenuItems.Add(0, metapropmenuitem);
                        }
                        //#endif
                    }
                }

                if (node is MappingCell)
                {
                    //if (node.MetaObject != null && node.MetaObject.pkid > 0)
                    //{
                    //    MenuItem mnuViewInContextParent = new MenuItem("View In Context", mnuViewInContext_Click);
                    //    mnuViewInContextParent.Tag = node.MetaObject;
                    //    MenuItem mnuViewInContext = new MenuItem("Client Workspaces", mnuViewInContext_Click);
                    //    mnuViewInContext.Tag = node.MetaObject;
                    //    mnuViewInContextParent.MenuItems.Add(mnuViewInContext);
                    //    if (Variables.Instance.IsServer)
                    //    {
                    //        MenuItem mnuViewInContextServer = new MenuItem("Server Workspaces", mnuViewInContextServer_Click);
                    //        mnuViewInContextServer.Tag = node.MetaObject;
                    //        mnuViewInContextParent.MenuItems.Add(mnuViewInContextServer);
                    //    }
                    //    MyView.ViewContextMenu.MenuItems.Add(mnuViewInContextParent);
                    //}

                    MenuItem hl = new MenuItem("Header Location", SwapMappingCellHeaderLocation);
                    MenuItem hlO = new MenuItem("None", SwapMappingCellHeaderLocation);
                    MenuItem hlL = new MenuItem("Left", SwapMappingCellHeaderLocation);
                    MenuItem hlR = new MenuItem("Right", SwapMappingCellHeaderLocation);
                    MenuItem hlT = new MenuItem("Top", SwapMappingCellHeaderLocation);
                    MenuItem hlB = new MenuItem("Bottom", SwapMappingCellHeaderLocation);
                    hl.MenuItems.Add(0, hlO);
                    hl.MenuItems.Add(0, hlL);
                    hl.MenuItems.Add(0, hlR);
                    hl.MenuItems.Add(0, hlT);
                    hl.MenuItems.Add(0, hlB);
                    MyView.ViewContextMenu.MenuItems.Add(0, hl);

                    MenuItem maskSwimlane = new MenuItem((node as MappingCell).Masked ? "Un-mask Swimlane" : "Mask Swimlane");
                    maskSwimlane.Tag = node;
                    maskSwimlane.Click += new EventHandler(maskSwimlane_Click);
                    MyView.ViewContextMenu.MenuItems.Add(0, maskSwimlane);

                    MenuItem showMappingCellProperties = new MenuItem("Show Swimlane Bindings");
                    MyView.ViewContextMenu.MenuItems.Add(0, showMappingCellProperties);

                    MenuItem mappingCellBounds = new MenuItem("Limit Cell Bounds");

                    MenuItem mappingCellBoundsWidth = new MenuItem("Maximum Width");
                    mappingCellBoundsWidth.Click += new EventHandler(mappingCellBoundsWidth_Click);
                    mappingCellBoundsWidth.Checked = (MyView.Selection.Primary as MappingCell).MaxWidth;
                    MenuItem mappingCellBoundsHeight = new MenuItem("Maximum Height");
                    mappingCellBoundsHeight.Click += new EventHandler(mappingCellBoundsHeight_Click);
                    mappingCellBoundsHeight.Checked = (MyView.Selection.Primary as MappingCell).MaxHeight;

                    mappingCellBounds.MenuItems.Add(mappingCellBoundsWidth);
                    mappingCellBounds.MenuItems.Add(mappingCellBoundsHeight);

                    MyView.ViewContextMenu.MenuItems.Add(mappingCellBounds);
                }
                else if (node is ILinkedContainer)
                {
                    MenuItem mnuEditSubgraphBinding = new MenuItem("Edit Container Binding");
                    mnuEditSubgraphBinding.Tag = node as ILinkedContainer;
                    MyView.ViewContextMenu.MenuItems.Add(mnuEditSubgraphBinding);
                }
            }
        }

        private void maskSwimlane_Click(object sender, EventArgs e)
        {
            try
            {
                ((sender as MenuItem).Tag as MappingCell).Masked = !((sender as MenuItem).Tag as MappingCell).Masked;
                ((sender as MenuItem).Tag as MappingCell).InvalidateViews();
                MyView.Update();
            }
            catch
            {
            }
        }

        private void mappingCellBoundsHeight_Click(object sender, EventArgs e)
        {
            (MyView.Selection.Primary as MappingCell).MaxWidth = false;
            (MyView.Selection.Primary as MappingCell).MaxHeight = ((MyView.Selection.Primary as MappingCell).MaxHeight) ? false : true;
        }
        private void mappingCellBoundsWidth_Click(object sender, EventArgs e)
        {
            (MyView.Selection.Primary as MappingCell).MaxHeight = false;
            (MyView.Selection.Primary as MappingCell).MaxWidth = ((MyView.Selection.Primary as MappingCell).MaxWidth) ? false : true;
        }

        private void AddContextMenuItemForMetaNodes()
        {
            if (MyView.Selection.Count == 0)
                return;

            MenuItem seperator = new MenuItem("-");
            MyView.ViewContextMenu.MenuItems.Add(seperator);

            if (MyView.Selection.Primary is IMetaNode && !(MyView.Selection.Primary is CollapsibleNode) && !(MyView.Selection.Primary is ILinkedContainer))
            {
                string text = MyView.Selection.Primary is ImageNode ? "Normal" : "Image";

                MenuItem convertSymbol = new MenuItem("Convert to " + text + " Symbol", convertSymbol_Click);
                MyView.ViewContextMenu.MenuItems.Add(convertSymbol);
            }

            if (MyView.Selection.Primary is ImageNode)
            {
                MenuItem metaConvertImageMainItem = new MenuItem("Meta Convert");
                foreach (b.Class o in d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassProvider.GetAll())
                {
                    if (o.IsActive == true)//o.Name.ToLower() != "conditionaldescription" && o.Name.ToLower() != "locationassociation" && o.Name.ToLower() != "mutuallyexclusiveindicator" && o.Name.ToLower() != "datacolumn" && o.Name.ToLower() != "attribute" && o.Category.ToLower() != "associationattribute" && o.Name.ToLower() != "rationale" && o.Name.ToLower() != "flowdescription" && !o.Name.ToLower().Contains("time")
                    {
                        MenuItem metaConvertImageItem = new MenuItem(o.Name, metaConvertImageItem_Click);
                        metaConvertImageMainItem.MenuItems.Add(metaConvertImageItem);
                        //MenuItem metaConvertItemShallow = new MenuItem(o.Name, metaConvertItemShallow_Click);
                        //metaConvertMainItemShallow.MenuItems.Add(metaConvertItemShallow);
                    }
                }

                MyView.ViewContextMenu.MenuItems.Add(metaConvertImageMainItem);

                MenuItem imageNodeDisplayText = new MenuItem("Display Text");
                ImageNode imageNode = (MyView.Selection.Primary as ImageNode);
                foreach (System.Reflection.PropertyInfo info in imageNode.MetaObject.GetMetaPropertyList(false))
                {
                    MenuItem displayMemberItem = new MenuItem(info.Name);
                    displayMemberItem.Tag = imageNode;

                    if ((info.Name == "Name" && imageNode.DisplayMember != null && imageNode.DisplayMember.Length == 0) || info.Name == imageNode.DisplayMember)
                    {
                        displayMemberItem.Checked = true;
                    }
                    displayMemberItem.Click += new EventHandler(displayMemberItem_Click);
                    imageNodeDisplayText.MenuItems.Add(displayMemberItem);
                }

                MyView.ViewContextMenu.MenuItems.Add(imageNodeDisplayText);
            }
            foreach (GoObject o in MyView.Selection)
                if (!(o is GraphNode)) return;

            MenuItem metaConvertMainItem = new MenuItem("Meta Convert");
            MenuItem metaConvertMainItemShallow = new MenuItem("Update Shape", metaConvertItemShallow_Click);
            //MenuItem metaConvertMainItemShallow = new MenuItem("Update Shape (withou formatting)", metaConvertItemShallowNoFormat_Click);
            metaConvertMainItemShallow.Tag = MyView.Selection.First as IMetaNode;

            foreach (b.Class o in d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassProvider.GetAll())
            {
                if (o.IsActive == true && !Variables.Instance.ClassesWithoutStencil.Contains(o.Name))//o.Name.ToLower() != "conditionaldescription" && o.Name.ToLower() != "locationassociation" && o.Name.ToLower() != "mutuallyexclusiveindicator" && o.Name.ToLower() != "datacolumn" && o.Name.ToLower() != "attribute" && o.Category.ToLower() != "associationattribute" && o.Name.ToLower() != "rationale" && o.Name.ToLower() != "flowdescription" && !o.Name.ToLower().Contains("time")
                {
                    MenuItem metaConvertItem = new MenuItem(o.Name, metaConvertItem_Click);
                    metaConvertMainItem.MenuItems.Add(metaConvertItem);
                    //MenuItem metaConvertItemShallow = new MenuItem(o.Name, metaConvertItemShallow_Click);
                    //metaConvertMainItemShallow.MenuItems.Add(metaConvertItemShallow);
                }
            }

            MyView.ViewContextMenu.MenuItems.Add(metaConvertMainItem);
            MyView.ViewContextMenu.MenuItems.Add(metaConvertMainItemShallow);

            if (MyView.Selection.Count == 1 && Core.Variables.Instance.ShowDeveloperItems)
            {
                seperator = new MenuItem("-");
                MyView.ViewContextMenu.MenuItems.Add(seperator);
                MenuItem childrenToRowMenuItem = new MenuItem("Children to row", childrenToRowMenuItem_Click);
                childrenToRowMenuItem.Tag = MyView.Selection.First as GoNode;
                MenuItem childrenToColumnMenuItem = new MenuItem("Children to column", childrenToColumnMenuItem_Click);
                childrenToColumnMenuItem.Tag = MyView.Selection.First as GoNode;

                MyView.ViewContextMenu.MenuItems.Add(childrenToRowMenuItem);
                MyView.ViewContextMenu.MenuItems.Add(childrenToColumnMenuItem);
            }
        }

        private void displayMemberItem_Click(object sender, EventArgs e)
        {
            if (sender is MenuItem)
            {
                if ((sender as MenuItem).Tag is ImageNode)
                {
                    ((sender as MenuItem).Tag as ImageNode).DisplayMember = (sender as MenuItem).Text;
                }
            }
        }

        private void SwapMappingCellHeaderLocation(object sender, EventArgs e)
        {
            MappingCell cell = MyView.SelectedNodes[0] as MappingCell;
            cell.RepositionRectangle((sender as MenuItem).Text);
        }

        private void childrenToColumnMenuItem_Click(object sender, EventArgs e)
        {
            Layout.ColumnCombine cc = new MetaBuilder.Graphing.Layout.ColumnCombine((sender as MenuItem).Tag as GoNode);
        }
        private void childrenToRowMenuItem_Click(object sender, EventArgs e)
        {
            Layout.RowCombine cc = new MetaBuilder.Graphing.Layout.RowCombine((sender as MenuItem).Tag as GoNode);
        }

        #endregion

        #endregion

        #region Anchoring & padding

        /// <summary>
        /// Anchors the red to blue.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void AnchorRedToBlue(object sender, EventArgs e)
        {
            MenuItem mSender = sender as MenuItem;
            CreateAnchor(mSender, MyView.Selection.Last, MyView.Selection.First);
        }

        /// <summary>
        /// Anchors the blue to red.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void AnchorBlueToRed(object sender, EventArgs e)
        {
            MenuItem mSender = sender as MenuItem;
            CreateAnchor(mSender, MyView.Selection.First, MyView.Selection.Last);
        }

        private void CreateAnchor(MenuItem mSender, GoObject parent, GoObject child)
        {
            PositionLockLocation location = (PositionLockLocation)mSender.Tag;
            if (child is IBehaviourShape)
            {
                AnchorPositionBehaviour anchorPosBehaviour =
                    new AnchorPositionBehaviour((IIdentifiable)parent, child, location);
                ((IBehaviourShape)child).Manager.AddBehaviour(anchorPosBehaviour);
                parent.AddObserver(child);
            }
        }

        public void AutoSizeRedToBlue(object sender, EventArgs e)
        {
            MenuItem mSender = sender as MenuItem;
            CreateAutoSizeInfo(mSender, MyView.Selection.Last, MyView.Selection.First);
        }

        public void AutoSizeBlueToRed(object sender, EventArgs e)
        {
            MenuItem mSender = sender as MenuItem;
            CreateAutoSizeInfo(mSender, MyView.Selection.First, MyView.Selection.Last);
        }

        public void ContainerPaddingBlueToRed(object sender, EventArgs e)
        {
            MenuItem mSender = sender as MenuItem;
            CreatePaddingInfo(mSender, MyView.Selection.First, MyView.Selection.Last);
        }

        public void ContainerPaddingRedToBlue(object sender, EventArgs e)
        {
            MenuItem mSender = sender as MenuItem;
            CreatePaddingInfo(mSender, MyView.Selection.Last, MyView.Selection.First);
        }

        private void CreatePaddingInfo(MenuItem mSender, GoObject parent, GoObject child)
        {
            ContainerPaddingType paddingType = (ContainerPaddingType)mSender.Tag;
            if (child is IBehaviourShape)
            {
                ((IBehaviourShape)child).Manager.AddBehaviour(
                    new ContainerPaddingBehaviour(parent as IIdentifiable, child, paddingType));
                parent.AddObserver(child);
            }
        }

        private void CreateAutoSizeInfo(MenuItem mSender, GoObject parent, GoObject child)
        {
            AutoSizeType autoSizeType = (AutoSizeType)mSender.Tag;
            if (child is IBehaviourShape)
            {
                ((IBehaviourShape)child).Manager.AddBehaviour(
                    new AutosizeBehaviour(parent as IIdentifiable, child, autoSizeType));
                parent.AddObserver(child);
            }
        }

        #endregion

        #region Select By... Type/Class,AssociationType

        /// <summary>
        /// Selects the type of all items by.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void SelectAllItemsByType(object sender, EventArgs e)
        {
            Type t = MyView.Selection.Primary.DraggingObject.GetType();
            GoObject o = MyView.Selection.Primary.DraggingObject;
            if (o is IMetaNode)
            {
                IMetaNode selectedNode = MyView.Selection.Primary.DraggingObject as IMetaNode;
                if (selectedNode.HasBindingInfo)
                {
                    SelectNodesByClass(selectedNode.BindingInfo.BindingClass);
                }
                return;
            }
            if (o is QLink)
            {
                SelectLinksByAssociationType((MyView.Selection.Primary.DraggingObject as QLink).AssociationType);
                return;
            }
            SelectItemsByType(t);
        }

        //private void GenerateStructuredReport(object sender, EventArgs e)
        //{
        //    List<QLink> links = new List<QLink>();
        //    List<FishLink> fishlinks = new List<FishLink>();
        //    foreach (GoObject o in MyView.Selection)
        //    {
        //        //if (o is ILinkedContainer)
        //        //    nodes.Add(o);
        //        if (o is QLink)
        //            links.Add(o as QLink);
        //        else if (o is FishLink)
        //            fishlinks.Add(o as FishLink);
        //    }

        //    foreach (QLink link in links)
        //    {

        //    }
        //}

        private void SelectNodesByClass(string BindingClass)
        {
            MyView.StartTransaction();
            MyView.BusySelecting = true;
            AddItemsOfClassToSelection(BindingClass, MyView.Document);
            MyView.BusySelecting = false;
            MyView.FinishTransaction("Select by class");
        }

        private void AddItemsOfClassToSelection(string BindingClass, IGoCollection col)
        {
            foreach (GoObject o in col)
            {
                if (o is IMetaNode)
                {
                    if ((o as IMetaNode).HasBindingInfo)
                    {
                        if ((o as IMetaNode).BindingInfo.BindingClass == BindingClass)
                            MyView.Selection.Add(o);
                    }
                }
                if (o is IGoCollection)
                {
                    AddItemsOfClassToSelection(BindingClass, o as IGoCollection);
                }
            }
        }

        private void SelectLinksByAssociationType(LinkAssociationType linkType)
        {
            MyView.StartTransaction();
            foreach (GoObject o in MyView.Document)
            {
                if (o is QLink)
                {
                    if ((o as QLink).AssociationType == linkType)
                        MyView.Selection.Add(o);
                }
            }
            MyView.FinishTransaction("Select by association type");
        }

        private void SelectItemsByType(Type t)
        {
            MyView.StartTransaction();
            foreach (GoObject obj in MyView.Document)
            {
                if (obj.GetType() == t)
                {
                    MyView.Selection.Add(obj);
                }
            }
            MyView.FinishTransaction("Select by type");
        }

        #endregion

        #region AutoLinking, Prototyping & Shallow Copying

        private bool _AutoLinkingEnabled;
        private bool _RubberStampingEnabled;
        [NonSerialized]
        private GoNode previousNode;
        [NonSerialized]
        private GoObject rubberStampShape;

        public bool AutoLinkingEnabled
        {
            get { return _AutoLinkingEnabled; }
            set { _AutoLinkingEnabled = value; }
        }

        public bool RubberStampEnabled
        {
            // loft, name, velocity
            get { return _RubberStampingEnabled; }
            set { _RubberStampingEnabled = value; }
        }

        private void ShallowCopy(object sender, EventArgs e)
        {
            MyView.StartTransaction();
            CreateShallowCopy(sender, true);
            MyView.FinishTransaction("Shallow copy");
        }

        private IShallowCopyable CreateShallowCopy(object sender, bool AddToDoc)
        {
            IShallowCopyable originalNode = (sender as MenuItem).Tag as IShallowCopyable;
            GoNode originalNodeGoObject = originalNode as GoNode;
            originalNodeGoObject.Copyable = true;
            IShallowCopyable nodeCopy = originalNode.CopyAsShallow() as IShallowCopyable;

            if (nodeCopy is GoNode)
            {
                GoNode nodeCopyGoObject = nodeCopy as GoNode;
                nodeCopyGoObject.Remove();
                nodeCopy.HookupEvents();
                nodeCopyGoObject.Copyable = true;
                if (AddToDoc)
                {
                    MyView.Document.Add(nodeCopyGoObject);
                    int OffSet = 50;
                    nodeCopyGoObject.Position = new PointF(originalNodeGoObject.Position.X + OffSet, originalNodeGoObject.Position.Y + OffSet);
                    nodeCopyGoObject.Shadowed = true; //added
                }

                nodeCopy.HookupEvents();
                if (!(originalNodeGoObject is MappingCell))
                    AddShadows(originalNodeGoObject);

                /*GoObject objO = originalNodeGoObject as GoObject;
            objO.Shadowed = true;*/

                if (!(nodeCopyGoObject is MappingCell))
                    AddShadows(nodeCopyGoObject);
                /*GoObject objC = nodeCopy as GoObject;
            objC.Shadowed = true;*/

                //add nodeCopyGoObject to shallowcopies in DockingForm

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
                if (!AddToDoc)
                {
                    //Clipboard
                }
            }
            removeQuickMenu();
            MyView.EditCopy();
            //originalNode.HookupEvents();
            return nodeCopy;
        }

        public void AddShadows(GoObject node)
        {
            if (node == null)
                return;
            node.Shadowed = true;
            if (node is GoGroup)
            {
                foreach (GoObject o in node as GoGroup)
                {
                    //if ((!(o is GoPort)) && (!(o is GoText)))
                    if (!(o is GoText))
                    {
                        o.Shadowed = true;
                    }
                }
            }
        }

        public void RemoveShadows(GoObject node)
        {
            if (node == null || node is SubgraphNode)
                return;
            if (node is GraphNode)
                (node as GraphNode).overrideshadowset = true;
            node.Shadowed = false;
            // node.Shadowed = false;
            //  return;
            if (node is GoGroup)
                foreach (GoObject o in node as GoGroup)
                {
                    o.Shadowed = false;
                }
        }

        private void ShallowCopyClipboard(object sender, EventArgs e)
        {
            IShallowCopyable shallowCopy = CreateShallowCopy(sender, false);
            MyView.OnShallowCopy(shallowCopy, EventArgs.Empty);
        }

        private void UnBoundText(object sender, EventArgs e)
        {
            GraphNode originalNode = (sender as MenuItem).Tag as GraphNode;
            ShapeDesignController sdc = new ShapeDesignController(MyView);
            VisualNode node = sdc.AddTextNode(originalNode);
            node.DoBeginEdit(MyView);
        }

        private void SetAsRubberStampProtoType(object sender, EventArgs e)
        {
            rubberStampShape = (sender as MenuItem).Tag as GraphNode;
        }

        private void SetAutoLinkParent(object sender, EventArgs e)
        {
            previousNode = (sender as MenuItem).Tag as GraphNode;
        }

        #endregion

        #region From GraphView

        //OBSOLETE
        public void AddShallowCopies(Collection<IShallowCopyable> nodes)
        {
            if (!ReadOnly)
            {
                MyView.Selection.Clear();
                foreach (IShallowCopyable scopy in nodes)
                {
                    if (scopy is GoObject)
                    {
                        GoObject node = scopy as GoObject;
                        //node.Position = new PointF(MyView.DocExtentCenter.X - node.Width / 2, MyView.DocExtentCenter.Y - node.Height / 2);
                        MyView.Document.Add(node);

                        if (node is CollapsibleNode)
                        {
                            CollapsibleNode cnode = node as CollapsibleNode;
                            cnode.RemoveChildren();
                            MyView.Selection.Add(node);
                            // remove all child repeaters
                        }
                    }
                }
            }
        }
        public void UpdateFrameDate()
        {
            NormalDiagram diagram = MyView.Document as NormalDiagram;
            if (diagram != null)
                diagram.DocumentFrame.UpdateDate();
        }
        public void UpdateSize(CropType type)
        {
            MyView.Sheet.BottomRightMargin = MyView.Sheet.TopLeftMargin = new SizeF(60, 60);
            //TODO : Null sheet
            if (MyView.Document is NormalDiagram)
            {
                NormalDiagram ndiagram = MyView.Document as NormalDiagram;
                ndiagram.UpdateSize = false;
                switch (type)
                {
                    case CropType.ToFrame:
                        ////TODO : FROM PRINTER
                        //// Adjust the sheet to fit the frame - do not adjust the sheet
                        //Sheet.Size =
                        //    new SizeF(
                        //        ndiagram.DocumentFrame.Frame.Width + Sheet.TopLeftMargin.Width + Sheet.BottomRightMargin.Width + 2,
                        //        ndiagram.DocumentFrame.Frame.Height + +Sheet.TopLeftMargin.Height +
                        //        Sheet.BottomRightMargin.Height);
                        //Sheet.Position =
                        //    new PointF(ndiagram.DocumentFrame.Position.X - Sheet.TopLeftMargin.Width,
                        //               ndiagram.DocumentFrame.Position.Y - Sheet.TopLeftMargin.Width);

                        //break;
                        // Adjust the sheet to fit the frame - do not adjust the sheet

                        MyView.Sheet.Size = new SizeF(ndiagram.DocumentFrame.Width + MyView.Sheet.TopLeftMargin.Width + MyView.Sheet.BottomRightMargin.Width, ndiagram.DocumentFrame.Height + MyView.Sheet.TopLeftMargin.Height + MyView.Sheet.BottomRightMargin.Height);
                        MyView.Sheet.Position = new PointF(ndiagram.DocumentFrame.Position.X - MyView.Sheet.TopLeftMargin.Width, ndiagram.DocumentFrame.Position.Y - MyView.Sheet.TopLeftMargin.Width);
                        break;
                    case CropType.ToSheet:
                        //  Adjust the frame to fit the sheet - do not adjust the frame
                        /*ndiagram.DocumentFrame.Frame.Width = Sheet.Width - Sheet.TopLeftMargin.Width -
                                                       Sheet.BottomRightMargin.Width - 2;
                        ndiagram.DocumentFrame.Frame.Height = Sheet.Height - Sheet.TopLeftMargin.Height -
                                                        Sheet.BottomRightMargin.Height - 2;

                        ndiagram.DocumentFrame.Position =
                            new PointF(Sheet.Position.X + Sheet.TopLeftMargin.Width,
                                       Sheet.Position.Y + Sheet.TopLeftMargin.Height);

                        break;
                        ////TODO : ORIG*/
                        RectangleF rf = new RectangleF();
                        rf.Width = MyView.Sheet.Width - MyView.Sheet.TopLeftMargin.Width - MyView.Sheet.BottomRightMargin.Width - 1;
                        rf.Height = MyView.Sheet.Height - MyView.Sheet.TopLeftMargin.Height - MyView.Sheet.BottomRightMargin.Height - 1;
                        rf.Location = new PointF(MyView.Sheet.Position.X + MyView.Sheet.TopLeftMargin.Width, MyView.Sheet.Position.Y + MyView.Sheet.TopLeftMargin.Height);
                        //ndiagram.DocumentFrame.Setup(rf);
                        ndiagram.DocumentFrame.Setup(rf);
                        ndiagram.DocumentFrame.Frame.Bounds = rf;
                        ndiagram.DocumentFrame.Width = rf.Width;

                        ndiagram.DocumentFrame.Height = rf.Height;
                        ndiagram.DocumentFrame.Position = new PointF(rf.X, rf.Y);
                        ndiagram.DocumentFrame.Frame.Bounds = rf;
                        break;
                }
                //reposition the frame based on width of sheet
                ndiagram.DocumentFrame.Reposition();
                return;
            }
            PointF leftTopMost = new PointF(50000, 50000);
            PointF rightBottom = new PointF(-5000, -5000);
            foreach (GoObject o in MyView.Document)
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
            PointF newSheetPosition = new PointF(leftTopMost.X + MyView.Sheet.TopLeftMargin.Width, leftTopMost.Y + MyView.Sheet.BottomRightMargin.Height);
            MyView.Sheet.Position = MyView.Grid.FindNearestGridPoint(newSheetPosition, MyView.Sheet);
            MyView.Document.TopLeft = MyView.Grid.FindNearestGridPoint(newSheetPosition, MyView.Sheet);
        }
        //private static void AddExceptionToLogEntry(Exception ex, StringBuilder message)
        //{
        //    message.AppendLine("Source");
        //    message.AppendLine(ex.Source);
        //    message.AppendLine();
        //    message.AppendLine("StackTrace");
        //    message.AppendLine(ex.StackTrace);
        //    message.AppendLine();
        //    message.AppendLine("Message");
        //    message.AppendLine(ex.Message);
        //    message.AppendLine();
        //    message.AppendLine("TargetSite");
        //    message.AppendLine(ex.TargetSite.Name);
        //    message.AppendLine();
        //    message.AppendLine("Module");
        //    message.AppendLine(ex.TargetSite.Module.Name);
        //    if (ex.InnerException != null)
        //        AddExceptionToLogEntry(ex.InnerException, message);

        //}
        public void CenterView()
        {
            MyView.SheetStyle = GoViewSheetStyle.Sheet;
            RectangleF b = MyView.ComputeDocumentBounds();
            PointF c = new PointF(b.X + b.Width / 2, b.Y + b.Height / 2);
            float s = MyView.DocScale;
            if (b.Width > 0 && b.Height > 0)
                // s = Math.Min((dispSize.Width / b.Width), (dispSize.Height / b.Height));
                //  if (s > 1) s = 1;
                MyView.RescaleWithCenter(s, c);
        }
        public List<IGoCollection> GetNestedObjects(IGoCollection g)
        {
            List<IGoCollection> objects = new List<IGoCollection>();
            foreach (GoObject o in g)
            {
                if (o is IGoCollection)
                {
                    IGoCollection obj = o as IGoCollection;
                    objects.Add(obj);
                    objects.AddRange(GetNestedObjects(obj));
                }
            }
            return objects;
        }
        public Collection<IMetaNode> GetNestedNodes(IGoCollection g)
        {
            Collection<IMetaNode> objects = new Collection<IMetaNode>();
            foreach (GoObject o in g)
            {
                if (o is IMetaNode)
                {
                    objects.Add(o as IMetaNode);
                }
                if (o is IGoCollection)
                {
                    foreach (IMetaNode imnChild in GetNestedNodes(o as IGoCollection))
                    {
                        objects.Add(imnChild);
                    }
                }
            }
            return objects;
        }
        public List<BoundLabel> GetNestedBoundLabels(IGoCollection collection)
        {
            List<BoundLabel> labels = new List<BoundLabel>();
            GetLabels(collection, ref labels);
            foreach (GoObject o in collection)
            {
                if (o is IGoCollection)
                {
                    GetLabels(o as IGoCollection, ref labels);
                }
            }
            return labels;
        }
        private void GetLabels(IGoCollection collection, ref List<BoundLabel> labels)
        {
            foreach (GoObject o in collection)
            {
                if (o is BoundLabel)
                {
                    BoundLabel lbl = o as BoundLabel;
                    labels.Add(lbl);
                }
            }
        }
        public PointF DocExtentCenter
        {
            get
            {
                PointF pos = MyView.DocPosition;
                SizeF siz = MyView.DocExtentSize;
                return new PointF(pos.X + siz.Width / 2, pos.Y + siz.Height / 2);
            }
            set
            {
                SizeF siz = MyView.DocExtentSize;
                MyView.DocPosition = new PointF(value.X - siz.Width / 2, value.Y - siz.Height / 2);
            }
        }
        public void ShowRichTextForm(RichText rt)
        {
            RichTextEditForm rtEditForm = new RichTextEditForm();
            rtEditForm.Mode = RichTextEditForm.EditorMode.RTF;
            LongText lt = new LongText();
            lt.RTF = rt.Rtf;
            rtEditForm.longText = lt;
            DialogResult res = rtEditForm.ShowDialog();
            if (res == DialogResult.OK)
            {
                rt.Rtf = rtEditForm.longText.RTF;
            }
        }

        public enum CropType
        {
            ToSheet,
            ToFrame
        }

        #endregion

        #region From GraphViewContainer

        public PointF GetCenter()
        {
            PointF pos = MyView.DocPosition;
            SizeF siz = MyView.DocExtentSize;
            PointF center = new PointF(pos.X + siz.Width / 2, pos.Y + siz.Height / 2);
            return center;
        }
        public SubgraphNode AddSubgraphObjectNode()
        {
            SubgraphNode sgnode = new SubgraphNode();
            sgnode.Resizable = true;
            sgnode.Size = new SizeF(200, 200);

            MyView.Document.Add(sgnode);
            ShapeOrderingControl.SendToBack(sgnode, MyView);
            sgnode.Collapse();
            return sgnode;
        }
        public void AddValueChain(MetaBase mbase)
        {
            /* ValueChainSubgraph lsg = new ValueChainSubgraph();
             lsg.MetaObject = Loader.CreateInstance("Function");
             lsg.BindingInfo = new BindingInfo();
             lsg.BindingInfo.BindingClass = "Function";
             lsg.BindingInfo.Bindings = new Dictionary<string, string>();
             lsg.BindingInfo.Bindings.Add("lbl", "Name");
             lsg.HookupEvents();
             MyView.Document.Add(lsg);
             ShapeOrderingControl.SendToBack(lsg, MyView);*/

            ValueChain vcs = new ValueChain(true);

            if (mbase != null)
            {
                vcs.MetaObject = mbase;
                vcs.HookupEvents();
            }

            if (mbase != null)
                vcs.BindToMetaObjectProperties();

            vcs.Position = GetCenter();
            MyView.Document.Add(vcs);
            MyView.Update();

            vcs.FireMetaObjectChanged(this, EventArgs.Empty);
            ShapeOrderingControl.BringToFront(vcs, MyView);

            //Commented circular reference
            //Notify();
        }
        public void CheckForILinkContainers()
        {
            foreach (GoObject obj in MyView.Document)
            {
                if (obj is ILinkedContainer)
                {
                    MyView.Doc.ContainsILinkContainers = true;
                    return;
                }
            }
            MyView.Doc.ContainsILinkContainers = false;
        }
        public void CheckForLegend()
        {
            foreach (GoObject obj in MyView.Document)
            {
                if (obj is LegendNode)
                {
                    myLegend = (obj as LegendNode);
                    return;
                }
            }
        }
        public void FixValueChains(IGoCollection coll)
        {
            /*
               List<GoObject> objectsToRemove = new List<GoObject>();

               foreach (GoObject obj in coll)
               {
                   if (obj is ValueChain)
                   {
                       ValueChainStep vcs = obj as ValueChainStep;
                       vcs.MetaObject.Set("Name", vcs.Label.Text);
                       vcs.LayoutHandle();
                       vcs.LayoutLabel();
                    
                       vcs.Bounds = new RectangleF(vcs.TopLeftMarker.Position,
       new SizeF(vcs.BottomRightMarker.Position.X + vcs.BottomRightMarker.Width - vcs.TopLeftMarker.Position.X,
       vcs.BottomRightMarker.Position.Y + vcs.BottomRightMarker.Height - vcs.TopLeftMarker.Position.Y));
                       vcs.RedrawBackgroundstep();
                       vcs.RepositionPorts();
                       vcs.Initializing = false;
                       vcs.LayoutLabel();
                       FixValueChains(vcs);
                   }
               }*/
        }
        public void MakeSameSizes(IGoCollection col, bool UseMax)
        {
            SizeF defaultSize = new SizeF();
            if (!UseMax)
                defaultSize = new SizeF(1000, 1000);
            foreach (GoObject o in col)
            {
                if (!(o is ValueChain))
                {
                    if (UseMax)
                    {
                        if (o.Width > defaultSize.Width)
                            defaultSize.Width = o.Width;
                        if (o.Height > defaultSize.Height)
                            defaultSize.Height = o.Height;
                    }
                    else
                    {
                        if (o.Width < defaultSize.Width)
                            defaultSize.Width = o.Width;
                        if (o.Height < defaultSize.Height)
                            defaultSize.Height = o.Height;
                    }
                }
            }

            foreach (GoObject o in col)
            {
                if (o is ValueChain)
                {
                    ValueChain vc = o as ValueChain;
                    vc.Grid.Size = defaultSize;
                }
            }
        }
        public void FindTopShape()
        {
            PointF xTop = new PointF(10000, 10000);
            GoObject topObject = null;
            foreach (GoObject obj in MyView.Document)
            {
                if (obj.Top < xTop.Y)
                {
                    topObject = obj;
                    xTop = obj.Position;
                }


            }
            if (topObject.Width == 0 && topObject.Height == 0)
            {
                topObject.Remove();
            }
            //Console.WriteLine(topObject);

        }
        public void RemovePorts(IGoCollection coll)
        {
            List<GoObject> objectsToRemove = new List<GoObject>();
            foreach (GoObject obj in coll)
            {
                if (obj is SubgraphNode)
                {
                    foreach (GoObject objChild in obj as SubgraphNode)
                    {
                        if (objChild is GoPort)
                        {
                            objectsToRemove.Add(objChild);
                        }
                    }
                    RemovePorts(obj as SubgraphNode);

                }
            }
            for (int i = 0; i < objectsToRemove.Count; i++)
            {
                objectsToRemove[i].Remove();
            }
        }
        public void FixOSDShapes(IGoCollection col)
        {
            foreach (GoObject o in col)
            {
                if (o is GraphNode)
                {
                    GraphNode n = o as GraphNode;
                    if (n.BindingInfo.BindingClass == "OrganizationalUnit")
                    {
                        int labelNo = 2;
                        n.BoundLabels[labelNo].Wrapping = true;
                        n.BoundLabels[labelNo].WrappingWidth = n.BoundLabels[labelNo].Width - 20;
                        n.BoundLabels[labelNo].Height = n.BoundLabels[labelNo].Height - 10;
                    }
                    //n.BoundLabels[0].
                }
            }
        }
        public void FixDSDShapes(IGoCollection col)
        {
            foreach (GoObject o in col)
            {
                if (o is GraphNode)
                {
                    GraphNode n = o as GraphNode;
                    n.BoundLabels[0].Wrapping = true;
                    n.BoundLabels[0].WrappingWidth = n.BoundLabels[0].Width - 20;
                    //n.BoundLabels[0].
                }
            }
        }
        public void CollapseAll()
        {
            if (MyView.Selection.Primary is CollapsibleNode)
            {
                CollapsibleNode currentObject = MyView.Selection.Primary as CollapsibleNode;
                MyView.Selection.Clear();

                IMetaNode imnode = currentObject as IMetaNode;

                bool collapse = false;
                if (currentObject.IsExpanded)
                {
                    collapse = true;
                }

                SelectShallows(MyView.Document, currentObject, imnode.MetaObject);

                foreach (GoObject o in MyView.Document)
                {
                    if (o is CollapsibleNode)
                    {

                        CollapsibleNode cnode = o as CollapsibleNode;
                        if (cnode.BindingInfo.BindingClass == imnode.MetaObject._ClassName)
                        {
                            if (collapse)
                            {
                                if (cnode.IsExpanded)
                                {
                                    cnode.Collapse();
                                }
                            }
                            else
                            {
                                if (!(cnode.IsExpanded))
                                {
                                    cnode.Expand();
                                }
                            }
                        }
                    }
                }
            }
        }
        public void SelectShallowCopies()
        {
            if (MyView.Selection.Primary is IMetaNode)
            {
                GoObject currentObject = MyView.Selection.Primary;
                MyView.Selection.Clear();

                IMetaNode imnode = currentObject as IMetaNode;

                SelectShallows(MyView.Document, currentObject, imnode.MetaObject);
            }
        }
        public void SelectShallows(IGoCollection col, GoObject current, MetaBase target)
        {
            foreach (GoObject obj in col)
            {
                if (obj != current)
                {
                    if (obj is IMetaNode)
                    {
                        try
                        {
                            IMetaNode imnode = obj as IMetaNode;
                            if (imnode.MetaObject != null)
                                if ((imnode.MetaObject.pkid == target.pkid) && (imnode.MetaObject.MachineName == target.MachineName))
                                    MyView.Selection.Add(obj);
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }
        //static
        public void FixNonLinkingPorts(IGoCollection col)
        {
            foreach (GoObject o in col)
            {
                if (o is GraphNode)
                {
                    GraphNode onode = o as GraphNode;
                    onode.LayoutChildren(null);
                    if ((onode.MetaObject._ClassName == "OrganizationalUnit"))
                    {
                        onode.BoundLabels[2].Height = onode.BoundLabels[2].Height - 10;
                        onode.BoundLabels[2].Clipping = true;
                        //// Console.WriteLine("Gotcha");
                    }
                    if ((onode.MetaObject._ClassName == "Activity"))
                    {
                        onode.BoundLabels[3].Height = onode.BoundLabels[3].Height - 10;
                        onode.BoundLabels[3].Clipping = true;
                        //// Console.WriteLine("Gotcha");
                    }
                    if ((onode.MetaObject._ClassName == "Function"))
                    {
                        onode.BoundLabels[3].Height = onode.BoundLabels[3].Height - 10;
                        onode.BoundLabels[3].Clipping = true;
                        onode.BoundLabels[3].Width = onode.BoundLabels[3].Width - 10;
                        //// Console.WriteLine("Gotcha");
                    }
                    /* GoNodePortEnumerator portEnum = onode.Ports;
                     while (portEnum.MoveNext())
                     {

                         IGoPort gport = portEnum.Current;
                         QuickPort qp = gport as QuickPort;
                         if (qp.Position.Y > onode.Position.Y + onode.Height - 15)
                         {
                           
                         }
                     }*/
                }
            }
        }
        //static
        public void FixOldAddShapes(IGoCollection col)
        {
            foreach (GoObject o in col)
            {
                if (o is CollapsibleNode)
                {
                    CollapsibleNode colNode = o as CollapsibleNode;
                    GradientRoundedRectangle rect = null;
                    try
                    {
                        foreach (GoObject oChild in colNode)
                        {
                            if (oChild is CollapsingRecordNodeItemList)
                            {
                                CollapsingRecordNodeItemList itemList = oChild as CollapsingRecordNodeItemList;
                                foreach (GoObject ooChild in itemList)
                                {
                                    if (ooChild is MetaBuilder.Graphing.Shapes.Behaviours.IIdentifiable)
                                    {
                                        MetaBuilder.Graphing.Shapes.Behaviours.IIdentifiable iid = ooChild as MetaBuilder.Graphing.Shapes.Behaviours.IIdentifiable;
                                        if (iid.Name == "139a8bc0-ec73-404e-9f60-9eb004d21d38")
                                        {
                                            rect = iid as GradientRoundedRectangle;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception fixException)
                    {
                        Console.WriteLine(fixException.ToString());
                    }

                    if (rect != null)
                    {
                        foreach (GoObject oChild in colNode)
                        {
                            if (oChild is QuickPort)
                            {
                                QuickPort qp = oChild as QuickPort;
                                if (qp.Manager.Behaviours == null)
                                    qp.Manager.ClearBehaviours(null, EventArgs.Empty);
                                if (qp.Manager.Behaviours.Count == 0)
                                {
                                    if (qp.IncomingLinksDirection == 90 && qp.OutgoingLinksDirection == 90)
                                    {
                                        MetaBuilder.Graphing.Shapes.Behaviours.Observers.AnchorPositionBehaviour apbehave = new MetaBuilder.Graphing.Shapes.Behaviours.Observers.AnchorPositionBehaviour(rect, qp, MetaBuilder.Graphing.Shapes.Behaviours.Observers.PositionLockLocation.BottomCenter);
                                        qp.Position = new PointF(qp.Position.X, rect.Bottom - qp.Height / 2f);
                                        apbehave.LockLocation = MetaBuilder.Graphing.Shapes.Behaviours.Observers.PositionLockLocation.BottomCenter;
                                        apbehave.SetupInitialProperties(rect, qp);
                                        qp.Manager.Behaviours.Clear();
                                        qp.Manager.Behaviours.Add(apbehave);

                                    }
                                }
                            }
                        }
                    }

                    GoNodePortEnumerator portEnum = colNode.Ports;
                    while (portEnum.MoveNext())
                    {
                        if (portEnum.Current is QuickPort)
                        {
                            QuickPort prt = portEnum.Current as QuickPort;
                            if (prt.Manager.Behaviours == null)
                                prt.Manager.ClearBehaviours(colNode, EventArgs.Empty);
                            if (prt.Manager.Behaviours.Count > 0)
                            {
                                foreach (MetaBuilder.Graphing.Shapes.Behaviours.IBehaviour ibehave in prt.Manager.Behaviours)
                                {
                                    if (ibehave is MetaBuilder.Graphing.Shapes.Behaviours.Observers.AnchorPositionBehaviour)
                                    {
                                        MetaBuilder.Graphing.Shapes.Behaviours.Observers.AnchorPositionBehaviour apbehave = ibehave as
                                            MetaBuilder.Graphing.Shapes.Behaviours.Observers.AnchorPositionBehaviour;
                                        if (apbehave.LockLocation == MetaBuilder.Graphing.Shapes.Behaviours.Observers.PositionLockLocation.BottomCenter)
                                        {
                                            if (apbehave.MyObserved is GradientRoundedRectangle)
                                            {
                                                GradientRoundedRectangle gradrectangle = apbehave.MyObserved as GradientRoundedRectangle;
                                                prt.Position = new PointF(prt.Position.X, gradrectangle.Bottom - prt.Height / 2f);
                                                apbehave.SetupInitialProperties(gradrectangle, prt);
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
        public void HighlightNode(GoObject imnode, bool show)
        {
            if (show)
            {
                HighlightIndicator ind = null;
                if (imnode is CollapsingRecordNodeItem)
                {
                    ind = new HighlightIndicator(false);
                    ind.Bounds = imnode.Bounds;
                    ind.Bounds.Inflate(5, 5);
                    MyView.Document.Add(ind);
                }
                else
                {
                    ind = new HighlightIndicator(true);
                    ind.Bounds = imnode.Bounds;
                    MyView.Document.Add(ind);
                    ShapeOrderingControl.SendToBack(ind, MyView);
                }

            }
            //SetDocumentModifiedToFalseIfNotTrue(false);
            //MyView.Document.IsModified = false;
        }
        public void RemoveHighlights()
        {
            List<GoObject> indicators = new List<GoObject>();

            foreach (GoObject o in MyView.Document)
            {
                if (o is HighlightIndicator)
                {
                    indicators.Add(o);
                }
            }
            for (int i = 0; i < indicators.Count; i++)
            {
                indicators[i].Remove();
            }
        }

        //This should have already happened in transformer once shape was bound to metaobject
        //public void HookupObjectEvents()
        //{
        //if (MyView != null)
        //    foreach (GoObject o in MyView.Document)
        //    {
        //        if (o is IMetaNode)
        //        {
        //            ((IMetaNode)o).HookupEvents();
        //        }
        //    }
        //}
        public List<IMetaNode> GetIMetaNodes()
        {
            List<IMetaNode> retval = new List<IMetaNode>();
            foreach (GoObject o in MyView.Document)
            {
                if (o is IMetaNode)
                {
                    IMetaNode imnode = o as IMetaNode;
                    if (imnode.MetaObject != null)
                        if (!retval.Contains(imnode))
                            retval.Add(imnode);
                }
                //NOT ELSE
                if (o is IGoCollection)
                {
                    retval.AddRange(GetIMetaNodes(o as IGoCollection));
                }
            }
            return retval;
        }
        public List<IMetaNode> GetIMetaNodes(IGoCollection collection)
        {
            List<IMetaNode> retval = new List<IMetaNode>();
            foreach (GoObject o in collection)
            {
                if (o is IMetaNode)
                {
                    IMetaNode imnode = o as IMetaNode;
                    if (imnode.MetaObject != null)
                        if (!retval.Contains(imnode))
                            retval.Add(imnode);
                }
                //NOT ELSE
                if (o is IGoCollection)
                {
                    retval.AddRange(GetIMetaNodes(o as IGoCollection));
                }
            }
            return retval;
        }
        public List<GraphNode> GetNodes()
        {
            List<GraphNode> retval = new List<GraphNode>();
            foreach (GoObject o in MyView.Document)
            {
                if (o is GraphNode)
                {
                    retval.Add(o as GraphNode);
                }
                else if (o is GoGroup)
                {
                    GoGroup grp = o as GoGroup;
                    GoGroupEnumerator grpenumerator = grp.GetEnumerator();
                    while (grpenumerator.MoveNext())
                    {
                        if (grpenumerator.Current.ParentNode is GraphNode)
                        {
                            retval.Add(grpenumerator.Current.ParentNode as GraphNode);
                        }
                    }
                }
            }
            return retval;
        }
        public List<IMetaNode> GetIMetaNodesBoundToMetaObject(MetaBase mbase)
        {
            List<IMetaNode> retval = new List<IMetaNode>();
            List<IMetaNode> nodes = GetIMetaNodes();
            foreach (IMetaNode node in nodes)
            {
                //if (node.MetaObject == null)
                //{
                //    //node.MetaObject = mbase;??this makes no sense what if the object is a function and mbase is a datacolumn?
                //}
                if (node.MetaObject == mbase)
                {
                    retval.Add(node);
                }
                else if (node.MetaObject.pkid > 0 && node.MetaObject.pkid == mbase.pkid && node.MetaObject.MachineName == mbase.MachineName)
                {
                    retval.Add(node);
                }
            }
            return retval;
        }
        public List<GraphNode> GetNodesBoundToMetaObject(MetaBase mbase)
        {
            List<GraphNode> retval = new List<GraphNode>();
            List<GraphNode> nodes = GetNodes();
            foreach (GraphNode node in nodes)
            {
                if (node.MetaObject == null)
                    continue;
                if (node.MetaObject.pkid > 0 && node.MetaObject.pkid == mbase.pkid && node.MetaObject.MachineName == mbase.MachineName)
                {
                    retval.Add(node);
                }
            }
            return retval;
        }
        [NonSerialized]
        private Dictionary<string, List<GraphNode>> shallowCopyDictionary;
        public Dictionary<string, List<GraphNode>> ShallowCopyDictionary
        {
            get { return shallowCopyDictionary; }
            set { shallowCopyDictionary = value; }
        }
        public void RemoveChangedIndicators(IGoCollection collection, bool overrideRules)
        {
            IndicatorController.RemoveChangedIndicators(collection, overrideRules);

            //7 January 2013 - RemoveChangedIndicators already iterates (redundant code)
            //foreach (GoObject o in collection)
            //{
            //    if (o is GoNode)
            //    {
            //        GoNode gnode = o as GoNode;
            //        IndicatorController.RemoveChangedIndicators(gnode, overrideRules);
            //    }
            //    if (o is IGoCollection)
            //    {
            //        RemoveChangedIndicators(o as IGoCollection, overrideRules);
            //    }
            //}
            //SetDocumentModifiedToFalseIfNotTrue(false);
            //MyView.Document.IsModified = false;
        }
        public void RemoveSavedChangedIndicators(IGoCollection collection, bool overrideRules)
        {
            foreach (GoObject o in collection)
            {
                if (o is GoNode)
                {
                    GoNode gnode = o as GoNode;
                    IndicatorController.RemoveSavedChangedIndicators(gnode, overrideRules);
                }
                if (o is IGoCollection)
                {
                    RemoveChangedIndicators(o as IGoCollection, overrideRules);
                }
            }
            //SetDocumentModifiedToFalseIfNotTrue(false);
            //MyView.Document.IsModified = false;
        }
        public void RemoveVCIndicators(IGoCollection collection)
        {
            if (collection is GoNode)
            {
                GoNode gnode = collection as GoNode;
                IndicatorController.RemoveIndicators(gnode);
            }
            foreach (GoObject o in collection)
            {
                if (o is GoNode)
                {
                    GoNode gnode = o as GoNode;
                    IndicatorController.RemoveIndicators(gnode);
                }
                if (o is IGoCollection)
                {
                    RemoveVCIndicators(o as IGoCollection);
                }
            }
            //SetDocumentModifiedToFalseIfNotTrue(false);
            //MyView.Document.IsModified = false;
        }
        public void TuneClippingForCollection(IGoCollection grp)
        {
            foreach (GoObject obj in grp)
            {
                if (obj.ParentNode != null)
                {
                    if (obj.ParentNode is IMetaNode)
                    {
                        IMetaNode imnode = obj.ParentNode as IMetaNode;
                        if (imnode.BindingInfo.BindingClass == "DataTable")
                        {
                            if (obj.Parent is CollapsingRecordNodeItem)
                            {
                                if ((obj is GoRoundedRectangle) && (!(obj is GoCollapsibleHandle)))
                                {
                                    GoRoundedRectangle rect = obj as GoRoundedRectangle;
                                    if (rect.Pen.Width == 0 || rect.Pen.Color == Color.White)
                                    {
                                        //// Console.WriteLine("Do Nothing");
                                    }
                                    else
                                        obj.Width = 170;
                                    rect.Brush = Brushes.White;//new SolidBrush(Color.White);
                                    rect.Pen = new Pen(Color.White);
                                }

                                if (obj is GoRectangle)
                                {
                                    GoRectangle rectangle = obj as GoRectangle;
                                    SolidBrush solbrush = rectangle.Brush as SolidBrush;
                                    if (solbrush != null)
                                    {
                                        if (solbrush.Color == Color.FromKnownColor(KnownColor.Highlight))
                                            solbrush.Color = Color.White;

                                        //if (rectangle.Pen)
                                    }
                                }

                                if (obj is GoText)
                                {
                                    GoText txt = obj as GoText;

                                    if (txt.Text.Contains("Differs") || txt.Text.Contains("Not In Data"))
                                    {
                                        txt.Remove();
                                    }
                                    else
                                    {
                                        txt.Clipping = false;
                                        txt.Wrapping = true;
                                        txt.TransparentBackground = false;
                                        txt.TextColor = Color.Black;
                                        txt.StringTrimming = StringTrimming.None;
                                        txt.Width = 170;
                                        txt.InvalidateViews();
                                        if ((txt.Text != "Descriptive Columns") && (txt.Text != "Key Columns"))
                                        {
                                            ((GoText)(obj)).BackgroundColor = Color.White;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (obj is IGoCollection)
                {
                    TuneClippingForCollection(obj as IGoCollection);
                }
            }
        }
        public void SetupGrid()
        {
            MetaSettings mS = new MetaSettings();
            try
            {
                float CellSize = mS.GetSetting(MetaSettings.VIEW_GRIDCELLSIZE, 20f);
                MyView.GridCellSize = new SizeF(CellSize, CellSize);
            }
            catch
            {
            }
            MyView.ArrowMoveLarge = mS.GetSetting(MetaSettings.VIEW_ARROWMOVELARGE, 25f);
            MyView.ArrowMoveSmall = mS.GetSetting(MetaSettings.VIEW_ARROWMOVESMALL, 5f);
            MyView.GridStyle = (mS.GetSetting(MetaSettings.VIEW_SHOWGRID, false)) ? GoViewGridStyle.Line : GoViewGridStyle.None;
            MyView.GridSnapDrag = (mS.GetSetting(MetaSettings.VIEW_SNAPTOGRID, true)) ? GoViewSnapStyle.Jump : GoViewSnapStyle.None;
            MyView.GridSnapResize = (mS.GetSetting(MetaSettings.VIEW_SNAPRESIZE, false)) ? GoViewSnapStyle.Jump : GoViewSnapStyle.None;
            MyView.SmoothingMode = (SmoothingMode)Enum.Parse(typeof(SmoothingMode), mS.GetSetting(MetaSettings.VIEW_SMOOTHINGMODE, "HighSpeed"));
            MyView.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            if (MyView.Doc.FileType == FileTypeList.Symbol)
                MyView.GridStyle = GoViewGridStyle.None;
        }
        public NormalDiagram GetDiagram()
        {
            if (MyView.Document != null)
            {
                NormalDiagram ndiagram = new NormalDiagram();
                if (MyView.Document is NormalDiagram)
                {
                    return MyView.Document as NormalDiagram;
                }
            }
            return null;
        }
        //SaveAs
        public void CreateTemplateIfRequired(NormalDiagram ndiagram)
        {
            // prompt the user if he/she wants to disconnect the objects (ie, save as template)
            StringBuilder sbuilder = new StringBuilder();
            sbuilder.Append("You have chosen to save the diagram content to a different file name. " + Environment.NewLine).Append(Environment.NewLine);
            sbuilder.Append("Select the \"YES\" button to create a new diagram file with new objects and associations." + Environment.NewLine + Environment.NewLine);
            sbuilder.Append("Select the \"NO\" button to create a new diagram file but keep the existing object and association references intact." + Environment.NewLine);
            DialogResult res = MessageBox.Show(sbuilder.ToString(), "Create New Objects and Associations?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            /*
            sbuilder.Append("You have chosen a different filename. " + Environment.NewLine).Append(Environment.NewLine);
            sbuilder.Append("YES - Create a new diagram with new objects and links").Append(Environment.NewLine);
            sbuilder.Append("NO - Keep the diagram and object references");*/

            ResetDocumentVersion(ndiagram); // 6 August 2015 Always reset document version

            if (res == DialogResult.Yes)
            {
                ResetDocumentObjects(ndiagram);
            }
        }
        private void ResetDocumentVersion(NormalDiagram ndiagram)
        {
            ndiagram.VersionManager = new DocumentVersionManager(FileTypeList.Diagram);
            ndiagram.VersionManager.CurrentVersion.OriginalFileUniqueIdentifier = Guid.NewGuid();
        }
        private void ResetDocumentObjects(NormalDiagram ndiagram)
        {
            //List<IMetaNode> nodes = new List<IMetaNode>();
            //GetNodesInCollection(nodes, ndiagram);

            foreach (IMetaNode n in GetIMetaNodes(ndiagram))
            {
                if (n.MetaObject != null)
                {
                    if (n.MetaObject.pkid > 0)
                    {
                        n.MetaObject.Reset();
                    }
                }
            }
        }
        //private void GetNodesInCollection(List<IMetaNode> existingOnes, IGoCollection collection)
        //{
        //    foreach (GoObject o in collection)
        //    {
        //        if (o is IMetaNode)
        //            existingOnes.Add(o as IMetaNode);
        //        if (o is IGoCollection)
        //            GetNodesInCollection(existingOnes, o as IGoCollection);
        //    }
        //}

        #endregion

        #region Legend

        private LegendNode myLegend;

        private void removeLegend(object sender, EventArgs e)
        {
            MyView.StartTransaction();
            if (myLegend == null)
                CheckForLegend();

            makeAllLegendItemsVisible(sender, e);
            MyView.Document.Remove(myLegend);
            myLegend = null;
            MyView.FinishTransaction("Remove Legend");
        }
        private void removeLegendItemFromLegend(object sender, EventArgs e)
        {
            if (myLegend == null)
                CheckForLegend();
            //get item
            if ((sender as MenuItem).Tag is LegendItem)
            {
                if (myLegend != null)
                {
                    LegendItem item = (sender as MenuItem).Tag as LegendItem;
                    item.Visibility = true;
                    //make it visible
                    legendItemChanged(item, true);
                    //remove it from the legendnode
                    if (myLegend.Count == 1)
                    {
                        MyView.Document.Remove(myLegend);
                        myLegend = null;
                        makeAllLegendItemsVisible(sender, e);
                    }
                    else
                    {
                        myLegend.AddedItems.Remove(item.Key);
                        myLegend.Remove(item);
                    }
                }
            }
        }
        public void makeAllLegendItemsVisible(object sender, EventArgs e)
        {
            MyView.StartTransaction();
            if (myLegend == null)
                CheckForLegend();

            if (myLegend != null)
            {
                foreach (GoObject obj in myLegend)
                    if (obj is LegendItem)
                        (obj as LegendItem).Visibility = true;

                foreach (GoObject o in myView.Document)
                {
                    if (o is FishLink || o is FishRealLink) //these links !?must?! be made visible by there parent objects
                    {
                        continue;
                        //o.Visible = ArtefactPointersVisible;
                        //o.Printable = ArtefactPointersVisible;
                    }
                    else
                    {
                        o.Visible = true;
                    }
                }
            }
            MyView.FinishTransaction("Make All Legend Items Visible");
        }

        private LegendItem MakeLegendItem(GoObject obj, String keyClass, String keyColor)
        {
            return new LegendItem(obj, keyClass, keyColor);
        }

        public void CreateLegend(bool _class, bool color)
        {
            if (myLegend == null)
                CheckForLegend();

            LegendType type = LegendType.ColorAndClass;
            if (color && !_class)
                type = LegendType.Color;
            if (!color && _class)
                type = LegendType.Class;

            LegendNode lg = new LegendNode(type);
            Dictionary<string, LegendItem> addedItems = new Dictionary<string, LegendItem>();

            foreach (GoObject o in MyView.Document)
            {
                List<GoObject> nodeShapes = new List<GoObject>();
                string key = "";
                o.Visible = true;
                if (!(o is GraphNode))
                    continue;

                GraphNode node = o as GraphNode;
                GoObject shape = null;
                if (_class && color)
                {
                    if (!addedItems.ContainsKey(node.BindingInfo.BindingClass + "-" + getColor(node)))
                    {
                        foreach (GoObject objInternal in node.GetEnumerator())
                        {
                            if (objInternal is IBehaviourShape)
                            {
                                if (objInternal is GoPort) //QuickPorts are IBehaviourShapes and we dont want that
                                    continue;
                                if (key == "")
                                    key = node.BindingInfo.BindingClass + "-" + getColor(node);
                                nodeShapes.Add((objInternal as GoObject).Copy());
                            }
                        }
                    }
                }
                else if (_class)
                {
                    if (!addedItems.ContainsKey(node.BindingInfo.BindingClass))
                    {
                        foreach (GoObject objInternal in node.GetEnumerator())
                        {
                            if (objInternal is IBehaviourShape)
                            {
                                if (objInternal is GoPort) //QuickPorts are IBehaviourShapes and we dont want that
                                    continue;
                                if (key == "")
                                    key = node.BindingInfo.BindingClass;
                                nodeShapes.Add((objInternal as GoObject).Copy());
                            }
                        }
                    }
                }
                else if (color)
                {
                    key = getColor(node);
                    if (!addedItems.ContainsKey(key))
                    {
                        //foreach (GoObject objInternal in node.GetEnumerator())
                        //{
                        //    if (objInternal is IBehaviourShape)
                        //    {
                        //        if (objInternal is GoPort) //QuickPorts are IBehaviourShapes and we dont want that
                        //            continue;

                        shape = new GradientRoundedRectangle();
                        //Add rectangles for basic color seperated nodes
                        MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour gbeh = getGradientBehaviour(node, shape);
                        if (gbeh != null)
                        {
                            (shape as GradientRoundedRectangle).Corner = new SizeF(1, 1);
                            (shape as GradientRoundedRectangle).Size = new SizeF(75, 75);
                            (shape as GradientRoundedRectangle).Manager.AddBehaviour(gbeh);
                        }
                        else
                        {
                            //shape cannot be found on node
                            (shape as GradientRoundedRectangle).Corner = new SizeF(1, 1);
                            (shape as GradientRoundedRectangle).Size = new SizeF(40, 20);
                            (shape as GradientRoundedRectangle).Shadowed = true;
                        }

                        LegendItem item = MakeLegendItem(shape, "", key);
                        if (!(addedItems.ContainsKey(key)) && key != "")
                        {
                            lg.Add(item);
                            addedItems.Add(key, item);
                        }
                        //        break;
                        //    }
                        //}
                    }
                }

                if (_class || color)
                {
                    LegendItem item = null;

                    GoGroup myShapeGroup = new GoGroup();
                    myShapeGroup.DragsNode = true;
                    myShapeGroup.Selectable = true;
                    myShapeGroup.Resizable = false;

                    if (nodeShapes.Count == 1)
                    {
                        shape = nodeShapes[0] as GoObject;
                        if (_class && !color)
                        {
                            item = MakeLegendItem(shape, node.BindingInfo.BindingClass, "");
                        }
                        else if (_class && color)
                        {
                            item = MakeLegendItem(shape, node.BindingInfo.BindingClass, getColor(node));
                        }
                        else
                            continue; //only color
                        if (!(addedItems.ContainsKey(key)) && key != "")
                        {
                            lg.Add(item);
                            addedItems.Add(key, item);
                        }
                        key = "";
                    }
                    else
                    {
                        foreach (GoObject NodeO in nodeShapes)
                        {
                            myShapeGroup.Add(NodeO as GoObject);
                        }

                        if (_class && !color)
                        {
                            item = MakeLegendItem(myShapeGroup, node.BindingInfo.BindingClass, "");
                        }
                        else if (_class && color)
                        {
                            item = MakeLegendItem(myShapeGroup, node.BindingInfo.BindingClass, getColor(node));
                        }
                        else
                            continue; //only color

                        if (item != null)
                        {
                            if (!(addedItems.ContainsKey(key)) && key != "")
                            {
                                lg.Add(item);
                                addedItems.Add(key, item);
                            }
                            key = "";
                        }
                    }
                }

                //else
                //{
                //    shape = (objInternal as GoObject).Copy();
                //    LegendItem item = MakeLegendItem(shape, "", getColor(node));
                //    lg.Add(item);
                //    addedItems.Add(key, item);
                //}
            }

            alphabetizeLegend(lg, addedItems);

            ShapeDesignController sdc = new ShapeDesignController(MyView);
            lg.Location = myView.LastInput.DocPoint;

            if (myLegend != null)
                MyView.Document.Remove(myLegend);
            myLegend = lg;
            MyView.Document.Add(lg);
        }

        public static GoCollection GetAllKeyShapes(string key, LegendType type)
        {
            GoCollection col = new GoCollection();

            foreach (GoObject o in MyStaticView.Document)
            {
                if (o is GraphNode)
                {
                    if (getKey((o as GraphNode), type) == key)
                        col.Add(o);
                }
            }

            return col;
        }
        public static string getColor(LegendItem node)
        {
            foreach (GoObject shp in node.GetEnumerator())
            {
                if (shp is IBehaviourShape)
                {
                    if (shp is GoPort) //QuickPorts are IBehaviourShapes and we dont want that
                        continue;
                    if (!(shp is GradientRoundedRectangle) && !(shp is GradientEllipse) && !(shp is GradientTrapezoid) && !(shp is GradientPolygon) && !(shp is GradientValueChainStep))
                        continue;

                    MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour gradObject = (shp as IBehaviourShape).Manager.GetExistingBehaviour(typeof(MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour)) as MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour;

                    if (gradObject != null)
                        return gradObject.MyBrush.InnerColor.ToString() + "=" + gradObject.MyBrush.OuterColor.ToString();
                    else
                    {
                        return "Solid color";
                    }
                }
                else if (shp is GoGroup)
                {
                    return getColor(shp as GoGroup);
                }
            }
            //cant find a gradient on this node
            return "UnKnOwN Color";
        }
        public static string getColor(GraphNode node)
        {
            foreach (GoObject shp in node.GetEnumerator())
            {
                if (shp is IBehaviourShape)
                {
                    if (shp is GoPort || shp is QuickStroke) //QuickPorts are IBehaviourShapes and we dont want that
                        continue;
                    //if (!(shp is GradientRoundedRectangle) && !(shp is GradientEllipse) && !(shp is GradientTrapezoid) && !(shp is GradientPolygon) && !(shp is GradientValueChainStep))
                    //    continue;

                    MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour gradObject = (shp as IBehaviourShape).Manager.GetExistingBehaviour(typeof(MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour)) as MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour;

                    if (gradObject != null)
                        return gradObject.MyBrush.InnerColor.ToString() + "=" + gradObject.MyBrush.OuterColor.ToString();
                    else
                    {
                        return null;
                    }
                }
                else if (shp is GoGroup && !(shp is CollapsingRecordNodeItemList))
                {
                    return getColor(shp as GoGroup);
                }
            }
            //cant find a gradient on this node
            return "UnKnOwN Color";
        }
        public static string getColor(ILinkedContainer container)
        {
            if (container is ValueChain)
            {
                foreach (GoObject shp in (container as ValueChain).GetEnumerator())
                {
                    if (shp is VCShape)
                    {
                        MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour gradObject =
                              (shp as IBehaviourShape).Manager.GetExistingBehaviour(typeof(MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour)) as
                              MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour;

                        if (gradObject != null)
                            return gradObject.MyBrush.InnerColor.ToString() + "=" + gradObject.MyBrush.OuterColor.ToString();
                        else
                        {
                            return "Solid color";
                        }
                    }
                    else if (shp is GoGroup)
                    {
                        return getColor(shp as GoGroup);
                    }
                }
            }
            else if (container is SubgraphNode)
            {
                foreach (GoObject shp in (container as SubgraphNode).GetEnumerator())
                {
                    if (shp is IBehaviourShape)
                    {
                        if (shp is GoPort) //QuickPorts are IBehaviourShapes and we dont want that
                            continue;
                        if (!(shp is GradientRoundedRectangle) && !(shp is GradientEllipse) && !(shp is GradientTrapezoid))
                            continue;

                        MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour gradObject =
                            (shp as IBehaviourShape).Manager.GetExistingBehaviour(typeof(MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour)) as
                            MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour;

                        if (gradObject != null)
                            return gradObject.MyBrush.InnerColor.ToString() + "=" + gradObject.MyBrush.OuterColor.ToString();
                        else
                        {
                            return (container as SubgraphNode).BackgroundColor.ToString();
                        }
                    }
                    else if (shp is GoGroup)
                    {
                        return getColor(shp as GoGroup);
                    }
                }
            }
            //cant find a gradient on this node
            return "UnKnOwN Color";
        }
        public static string getColor(GoGroup legendGroup)
        {
            foreach (GoObject shp in legendGroup.GetEnumerator())
            {
                if (shp is IBehaviourShape)
                {
                    if (shp is GoPort) //QuickPorts are IBehaviourShapes and we dont want that
                        continue;
                    if (!(shp is GradientRoundedRectangle) && !(shp is GradientEllipse) && !(shp is GradientTrapezoid) && !(shp is GradientPolygon) && !(shp is GradientValueChainStep))
                        continue;

                    MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour gradObject = (shp as IBehaviourShape).Manager.GetExistingBehaviour(typeof(MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour)) as MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour;

                    if (gradObject != null)
                        return gradObject.MyBrush.InnerColor.ToString() + "=" + gradObject.MyBrush.OuterColor.ToString();
                    else
                    {
                        return "Solid color";
                    }
                }
                else if (shp is GoGroup)
                {
                    return getColor(shp as GoGroup);
                }
            }
            return "UnKnOwN Color";
        }
        public static string getKey(GraphNode n, LegendType MyLegendType)
        {
            switch (MyLegendType)
            {
                case LegendType.Class:
                    return n.BindingInfo.BindingClass;
                case LegendType.Color:
                    return getColor(n);
                case LegendType.ColorAndClass:
                    return n.BindingInfo.BindingClass + "-" + getColor(n);
                default:
                    return "LegendTypeNotSpecified";
            }
            //return "";
        }
        public static string getKey(ILinkedContainer n, LegendType MyLegendType)
        {
            if (n.MetaObject != null || MyLegendType == LegendType.Color)
                switch (MyLegendType)
                {
                    case LegendType.Class:
                        return n.MetaObject.Class;
                    case LegendType.Color:
                        return getColor(n);
                    case LegendType.ColorAndClass:
                        return n.MetaObject.Class + "-" + getColor(n);
                    default:
                        return "LegendTypeNotSpecified";
                }
            else
                return null;
        }
        public static MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour getGradientBehaviour(GraphNode node, GoObject owner)
        {
            foreach (GoObject shp in node.GetEnumerator())
            {
                if (shp is IBehaviourShape)
                {
                    if (shp is GoPort || shp is QuickStroke) //QuickPorts are IBehaviourShapes and we dont want that
                        continue;

                    MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour gradObject =
                        (shp as IBehaviourShape).Manager.GetExistingBehaviour(typeof(MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour)) as
                        MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour;

                    if (gradObject != null)
                        return gradObject.Copy(owner) as MetaBuilder.Graphing.Shapes.Behaviours.Internal.GradientBehaviour;
                }
            }
            //cant find a gradient on this node
            return null;
        }
        private void alphabetizeLegend(LegendNode legendGroup, Dictionary<string, LegendItem> items)
        {
            legendGroup.AddedItems = new Dictionary<string, LegendItem>();
            foreach (KeyValuePair<string, LegendItem> kvp in items)
            {
                //skip unknown colors
                if (!kvp.Key.Contains("UnKnOwN Color"))
                    legendGroup.AddedItems.Add(kvp.Key, kvp.Value);
            }
            //reorder based on alphabetical ordered list of items
            legendGroup.AddedItems.Clear();
        }

        private void legendItemChanged(LegendItem legendItem, bool deleting)
        {
            if (deleting)
                legendItem.Visibility = true;
            else
                legendItem.Visibility = !legendItem.Visibility;

            List<string> visibleKeys = new List<string>();
            List<string> invisibleKeys = new List<string>();

            foreach (GoObject leg in (legendItem.ParentNode as LegendNode).GetEnumerator())
            {
                if (leg is LegendItem)
                {
                    LegendItem legendItemInNode = leg as LegendItem;
                    if (legendItemInNode.Visibility)
                        visibleKeys.Add(legendItemInNode.Key);
                    else
                        invisibleKeys.Add(legendItemInNode.Key);
                }
            }

            foreach (GoObject o in MyView.Document)
            {
                if (o is GraphNode)
                {
                    if (visibleKeys.Contains(getKey((o as GraphNode), (legendItem.ParentNode as LegendNode).MyLegendType)))
                        o.Visible = true;
                    if (invisibleKeys.Contains(getKey((o as GraphNode), (legendItem.ParentNode as LegendNode).MyLegendType)))
                        o.Visible = false;
                }
                else if (o is ILinkedContainer)
                {
                    if (visibleKeys.Contains(getKey((o as ILinkedContainer), (legendItem.ParentNode as LegendNode).MyLegendType)))
                        o.Visible = true;
                    if (invisibleKeys.Contains(getKey((o as ILinkedContainer), (legendItem.ParentNode as LegendNode).MyLegendType)))
                        o.Visible = false;
                }
            }

            foreach (GoObject o in MyView.Document)
            {
                if (o is ILinkedContainer || o is GoNode)
                    continue;

                //act on the links only
                if (o is QLink)
                {
                    QLink link = o as QLink;
                    if (link.ToNode != null && link.FromNode != null)
                    {
                        if ((link.ToNode as GoObject).Visible == false || (link.FromNode as GoObject).Visible == false)
                            link.Visible = false;
                        else
                            link.Visible = true;
                    }
                    //THIS HAS MOVED TO QLINK VISIBLE PROPERTY OVERRIDE
                    //foreach (KeyValuePair<ArtefactNodeLinkKey,QLink> k in FindArtefactsOnLink(link, null))
                    //{
                    //    k.Key.Node.Visible = link.Visible;
                    //    foreach (FishLink l in k.Key.Node.Links.GetEnumerator())
                    //    {
                    //        l.RealLink.Visible = link.Visible;
                    //    }
                    //}
                }
                else if (o is FishLink)
                {
                    o.Visible = (o as FishLink).RealLink.Visible;
                }
                else if (o is GoBalloon)
                {
                    if ((o as GoBalloon).Anchor is GoNode)
                        o.Visible = ((o as GoBalloon).Anchor as GoNode).Visible;
                    else if ((o as GoBalloon).Anchor is QLink)
                        o.Visible = ((o as GoBalloon).Anchor as QLink).Visible;
                }
            }
        }

        #endregion

        //5 November 2013 Prevent child items removed when MFD Server Object
        private bool ControllerMFD;
        public void ModifyObjects(IGoCollection objects, bool MFD, GraphFileKey graphFileKey)
        {
            ControllerMFD = MFD;
            Collection<CollapsingRecordNodeItem> complexChildrenToActOn = new Collection<CollapsingRecordNodeItem>();
            foreach (GoObject o in objects)
            {
                //All
                //if (o.Copyable)
                //{
                //RemoveShadowsFromOtherShallowCopies(o);
                //}
                if (o is CollapsingRecordNodeItem)
                {
                    CollapsingRecordNodeItem cnodeItem = o as CollapsingRecordNodeItem;
                    complexChildrenToActOn.Add(cnodeItem);
                }
                else if (o is SubgraphNode)
                {
                    RemoveEmbeddedObjectsAndLinksFromSubgraph(o, MFD);
                }
                //23 January 2013 => this did nothing but write to console
                //if (o is GoNode)
                //{
                //    GoNode n = o as GoNode;

                //    GoNodeLinkEnumerator linkenum = n.DestinationLinks.GetEnumerator();
                //    while (linkenum.MoveNext())
                //    {
                //        //Console.WriteLine(linkenum.Current.ToString());
                //    }
                //}
                //Link
                if (o is QLink)
                {
                    if (MFD)
                    {
                        QLink link = o as QLink;
                        LinkController lcontroller = new LinkController();

                        ObjectAssociation assoc = lcontroller.GetAssociation(link, true);
                        if (assoc != null)
                        {
                            if (VCStatusTool.DeletableFromDiagram(assoc) && VCStatusTool.UserHasControl(assoc))
                            {
                                assoc.VCStatusID = (int)VCStatusList.MarkedForDelete;
                                assoc.State = VCStatusList.MarkedForDelete;
                                DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Save(assoc);
                            }
                            LinkController.InverseMarkForDelete(assoc);
                        }
                    }
                }
                //Node
                else if (o is IMetaNode || o.DraggingObject is IMetaNode)
                {
                    if (MFD)
                    {
                        IMetaNode node = null;
                        if (o is IMetaNode)
                            node = o as IMetaNode;
                        else if (o.DraggingObject is IMetaNode)
                            node = o.DraggingObject as IMetaNode;

                        if (node.MetaObject.pkid > 0 && node.MetaObject.MachineName != null)
                        {
                            node.MetaObject = Loader.GetByID(node.MetaObject._ClassName, node.MetaObject.pkid, node.MetaObject.MachineName);
                        }
                        if (VCStatusTool.DeletableFromDiagram(node.MetaObject) && VCStatusTool.UserHasControl(node.MetaObject))
                        {
                            node.MetaObject.State = VCStatusList.MarkedForDelete;
                            MetaObject mo = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetBypkidMachine(node.MetaObject.pkid, node.MetaObject.MachineName);
                            if (mo != null)
                            {
                                mo.VCStatusID = (int)VCStatusList.MarkedForDelete;
                                DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.Save(mo);
                                //25 October 2013 State set AFTER saved no before -.-
                                IndicatorController.AddChangedIndicator("Marked For Delete", System.Drawing.Color.Red, node as GoGroup);

                                if (node is Rationale || node is ArtefactNode)
                                    continue;
                                //also MFD all links
                                TList<ObjectAssociation> pLinks = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(node.MetaObject.pkid, node.MetaObject.MachineName);
                                MFDLinks(pLinks);
                                TList<ObjectAssociation> cLinks = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByChildObjectIDChildObjectMachine(node.MetaObject.pkid, node.MetaObject.MachineName);
                                MFDLinks(cLinks);

                                if (node is GoNode)
                                    MakeLinksRed(node as GoNode);
                            }
                        }
                    }
                    if (o.Parent is SubgraphNode)
                        (o.Parent as SubgraphNode).RemoveOnlyRelationship(o as IMetaNode);
                }
                if (o.Shadowed)
                {
                    RemoveShadowsFromOtherShallowCopies(o);
                }
            }

            foreach (CollapsingRecordNodeItem item in complexChildrenToActOn)
            {
                if (item.ParentList == null)
                    item.Remove();
                else
                    item.ParentList.Remove(item);
                //item.RemoveLink();
            }

            //if (MFD)
            //    SetDocumentModifiedToFalseIfNotTrue(false);
            //MyView.Document.IsModified = false;
        }

        //only affects server diagrams
        private void MakeLinksRed(GoNode node)
        {
            foreach (GoLabeledLink l in node.DestinationLinks)
            {
                //Pen oldPen = l.RealLink.Pen;
                l.RealLink.Pen = new Pen(Color.Red);
            }
            foreach (GoLabeledLink l in node.SourceLinks)
            {
                l.RealLink.Pen = new Pen(Color.Red);
            }
        }

        private void MyView_SelectionDeleting(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ////when we delete am object whose parent is a subgraphnode we must removetherelationship
            //GoSelection sel = (sender as GraphView).Selection;
            //if (sel != null)
            //{
            //    foreach (GoObject obj in sel)
            //    {
            //        if (obj is IMetaNode)
            //        {
            //            //when we mfd objects on server objects dont remove their 'child' objects (instead MFD them as well?)
            //            IMetaNode imnode = obj as IMetaNode;
            //            if (imnode.MetaObject.WorkspaceTypeId == 3 && ControllerMFD)
            //            {
            //                e.Cancel = true;
            //                continue;
            //            }
            //        }

            //        //remove rationales from this object if anchored to it
            //        foreach (GoObject myCom in myView.Document)
            //        {
            //            GoObject com = myCom;
            //            if (com is ResizableBalloonComment && (com as ResizableBalloonComment).Anchor == obj && !sel.Contains(com))
            //                myView.Document.Remove(com);
            //        }

            //        if (obj is IMetaNode)
            //        {
            //            if (obj.Parent is SubgraphNode)
            //                (obj.Parent as SubgraphNode).RemoveOnlyRelationship(obj as IMetaNode);
            //        }

            //        if (obj is GoNode)
            //        {
            //            //18 October 2013 - remove objects attached to associations
            //            GoNode myNode = obj as GoNode;

            //            #region removeArtefactsAndRationalesFromNode'sLinks
            //            //On Link
            //            //these links are about to be removed so remove all attached objects (rationales and artefacts)
            //            foreach (GoLabeledLink l in myNode.DestinationLinks)
            //            {
            //                GoLabeledLink link = l;
            //                if (l is QLink)
            //                {
            //                    reAnchorableComments = new Dictionary<ResizableBalloonComment, GoObject>();
            //                    foreach (KeyValuePair<ArtefactNodeLinkKey, QLink> k in FindArtefactsOnLink(l as QLink, l as QLink))
            //                    {
            //                        try
            //                        {
            //                            if (!sel.Contains(k.Key.Node))
            //                                myView.Document.Remove(k.Key.Node);
            //                        }
            //                        catch
            //                        {
            //                        }
            //                    }
            //                    foreach (KeyValuePair<ResizableBalloonComment, GoObject> k in reAnchorableComments)
            //                    {
            //                        try
            //                        {
            //                            if (!sel.Contains(k.Key))
            //                                myView.Document.Remove(k.Key);
            //                        }
            //                        catch
            //                        {
            //                        }
            //                    }
            //                }
            //            }
            //            foreach (GoLabeledLink l in myNode.Links)
            //            {
            //                GoLabeledLink link = l;
            //                if (l is QLink)
            //                {
            //                    reAnchorableComments = new Dictionary<ResizableBalloonComment, GoObject>();
            //                    foreach (KeyValuePair<ArtefactNodeLinkKey, QLink> k in FindArtefactsOnLink(l as QLink, l as QLink))
            //                    {
            //                        try
            //                        {
            //                            if (!sel.Contains(k.Key.Node))
            //                                myView.Document.Remove(k.Key.Node);
            //                        }
            //                        catch
            //                        {
            //                        }
            //                    }
            //                    foreach (KeyValuePair<ResizableBalloonComment, GoObject> k in reAnchorableComments)
            //                    {
            //                        try
            //                        {
            //                            if (!sel.Contains(k.Key))
            //                                myView.Document.Remove(k.Key);
            //                        }
            //                        catch
            //                        {
            //                        }
            //                    }
            //                }
            //            }
            //            #endregion
            //        }
            //    }
            //}
            ControllerMFD = false;
        }

        private void MFDLinks(TList<ObjectAssociation> links)
        {
            foreach (ObjectAssociation link in links)
            {
                link.VCStatusID = (int)VCStatusList.MarkedForDelete;
                d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Save(link);

                LinkController.InverseMarkForDelete(link);

                //TODO : is there an active artefact on this link?
                //TODO : make it red
            }
        }
        private void RemoveShadowsFromOtherShallowCopies(GoObject o)
        {
            if (o is GraphNode)
            {
                GraphNode node = o as GraphNode;
                Collection<GraphNode> shallowCopies = new Collection<GraphNode>();
                foreach (GoObject obj in MyView.Document)
                {
                    if (obj is GraphNode && obj != o)
                    {
                        GraphNode shallowInstance = obj as GraphNode;
                        if (shallowInstance.MetaObject.pkid == node.MetaObject.pkid && shallowInstance.MetaObject.MachineName == node.MetaObject.MachineName)
                        {
                            shallowCopies.Add(shallowInstance);
                        }
                    }
                }
                //if (shallowCopies.Count==1)
                foreach (GraphNode foundShallow in shallowCopies)
                {
                    bool foundOtherInstance = false;
                    foreach (GoObject obj in MyView.Document) //and all other open documents?
                    {
                        if (obj is GraphNode && obj != foundShallow && obj != o)
                        {
                            GraphNode shallowInstance = obj as GraphNode;
                            if (shallowInstance.MetaObject.pkid == foundShallow.MetaObject.pkid && shallowInstance.MetaObject.MachineName == foundShallow.MetaObject.MachineName)
                            {
                                foundOtherInstance = true;
                            }
                        }
                    }
                    if (!foundOtherInstance)
                    {
                        foundShallow.Copyable = true;
                        RemoveShadows(foundShallow);
                    }
                }
            }
        }
        private void RemoveEmbeddedObjectsAndLinksFromSubgraph(GoObject objOwner, bool markAsDelete)
        {
            // Probably unrelated to the current bug
            SubgraphNode sgNode = objOwner as SubgraphNode;
            Dictionary<GoObject, int> objectsToRemove = new Dictionary<GoObject, int>();
            foreach (GoObject obj in sgNode)
            {
                if (obj is ILinkedContainer)
                {
                    if (markAsDelete)
                        objectsToRemove.Add(obj, 1);
                    else
                        objectsToRemove.Add(obj, 2);
                }
            }
            RemoveItemsFromILinkedContainer(objectsToRemove, sgNode);
        }

        //Recursively(recursive==true) Expands all database nodes linked to the parentObject using REDAssociations onto the current diagram
        public Collection<MetaObject> ReddedObjects = null;
        public void RED(GoObject parentObject, bool recursive, TList<ObjectAssociation> REDassociations)
        {
            if (!(parentObject is GraphNode))
                return;
            GraphNode parentNode = parentObject as GraphNode;
            if (REDassociations == null)
                REDassociations = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(parentNode.MetaObject.pkid, parentNode.MetaObject.MachineName);

            ReddedObjects = new Collection<MetaObject>();

            float lastY = 0;

            foreach (GoObject node in parentNode.Destinations.GetEnumerator())
            {
                if (node is IMetaNode)
                    if (node.Position.Y > lastY)
                        lastY = node.Position.Y;
            }

            MyView.SuspendLayout();
            foreach (ObjectAssociation objectAssociation in REDassociations)
            {
                if (VCStatusTool.IsObsoleteOrMarkedForDelete(objectAssociation) || objectAssociation.Machine == "DB-TRIGGER")
                    continue;
                MetaObject mObj = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetBypkidMachine(objectAssociation.ChildObjectID, objectAssociation.ChildObjectMachine);

                if (mObj == null || mObj.Class == "DataColumn" || mObj.Class == "Attribute" || mObj.Class == "DataField" || mObj.Class == "DataAttribute" || ReddedObjects.Contains(mObj))
                    continue; //skip objects here
                if (VCStatusTool.IsObsoleteOrMarkedForDelete(mObj))
                    continue;

                ReddedObjects.Add(mObj);

                MetaBase metaBase = Loader.GetByID(mObj.Class, mObj.pkid, mObj.Machine);
                GraphNode newNode = (GraphNode)Core.Variables.Instance.ReturnShape(mObj.Class);
                if (newNode == null)
                    continue;
                newNode = (GraphNode)newNode.Copy();
                newNode.Position = new PointF(parentNode.Right - 60, (lastY > 0) ? lastY + 20 : parentNode.Bottom + (parentNode.Height * parentNode.Destinations.Count) + 20);
                newNode.MetaObject = metaBase;

                GraphNode connectedNode = checkCurrentlyConnected(parentNode, newNode);
                if (connectedNode == null)
                {
                    MyView.Document.Add(newNode);
                    MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation defaultFrom = (MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation)Enum.Parse(typeof(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation), Core.Variables.Instance.DefaultFromPort);
                    MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation defaultTo = (MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation)Enum.Parse(typeof(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation), Core.Variables.Instance.DefaultToPort);

                    if (defaultFrom.ToString().Contains("Bottom") && defaultTo.ToString().Contains("Top"))
                        newNode.Position = new PointF(parentNode.Left + ((newNode.Width * parentNode.Destinations.Count) + 40), parentNode.Bottom + 60);
                    else if (defaultFrom.ToString().Contains("Top") && defaultTo.ToString().Contains("Bottom"))
                        newNode.Position = new PointF(parentNode.Left + ((newNode.Width * parentNode.Destinations.Count) + 40), parentNode.Top - 60 - newNode.Height);

                    MyView.Document.Add(QLink.CreateLink(parentNode, newNode, DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByCAid(objectAssociation.CAid).AssociationTypeID, defaultFrom, defaultTo));
                }
                else
                    newNode = connectedNode;

                newNode.BindToMetaObjectProperties();
                if (recursive)
                    RED(newNode, recursive, DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(newNode.MetaObject.pkid, newNode.MetaObject.MachineName));
            }
            MyView.ResumeLayout();
        }
        //checks if parentnode connects to childnode
        private GraphNode checkCurrentlyConnected(GraphNode parentNode, GraphNode childNode)
        {
            foreach (GraphNode node in parentNode.Destinations)
                if (node.MetaObject.pkid == childNode.MetaObject.pkid && node.MetaObject.MachineName == childNode.MetaObject.MachineName)
                    return node;

            return null;
        }

        private BusinessFacade.Exports.HierarchicalToText hierarchicalToText = new BusinessFacade.Exports.HierarchicalToText();
        private void ExportTextTabbed(object sender, EventArgs e)
        {
            hierarchicalToText = new MetaBuilder.BusinessFacade.Exports.HierarchicalToText();
            ExportHierarchicalData(hierarchicalToText, false, true);
        }
        private void ExportTextNumbered(object sender, EventArgs e)
        {
            hierarchicalToText = new MetaBuilder.BusinessFacade.Exports.HierarchicalToText();
            ExportHierarchicalData(hierarchicalToText, true, false);
        }
        private void ExportTextBoth(object sender, EventArgs e)
        {
            hierarchicalToText = new MetaBuilder.BusinessFacade.Exports.HierarchicalToText();
            ExportHierarchicalData(hierarchicalToText, true, true);
        }

        //can only convert function (GraphNodes) : Business Rule => Patric
        private void convertToValueChainStep(object sender, EventArgs e)
        {
            MyView.StartTransaction();
            MenuItem mItemSender = sender as MenuItem;
            GraphNode node = mItemSender.Tag as GraphNode;
            if (node == null || node.MetaObject.Class != "Function")
                return;

            //add a new valuechainstep in location of this node
            ValueChain funcVC = new ValueChain(true);
            funcVC.Size = node.Size;
            funcVC.Reposition();
            funcVC.Location = node.Location;
            //bind metaobject to original node
            funcVC.MetaObject = node.MetaObject;
            funcVC.HookupEvents();
            funcVC.BindToMetaObjectProperties();

            MyView.Document.Add(funcVC);

            #region Re-Link

            bool cancel = false;
            foreach (IGoLink link in node.SourceLinks)
            {
                if (!(link is QLink))
                    continue;

                QLink ql = link as QLink;
                QuickPort qp = link.ToPort as QuickPort;
                Shapes.General.QuickPortHelper.QuickPortLocation loc = qp.PortPosition;
                //find port on VC
                QuickPort newPort = null;
                switch (loc)
                {
                    case MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Top:
                        newPort = funcVC.ChildNames["topPort"] as QuickPort;
                        break;
                    case MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Bottom:
                        newPort = funcVC.ChildNames["bottomPort"] as QuickPort;
                        break;
                    case MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Right:
                        newPort = funcVC.ChildNames["rightPort"] as QuickPort;
                        break;
                    default:
                        newPort = funcVC.ChildNames["leftPort"] as QuickPort;
                        break;
                }

                if (newPort != null)
                {
                    link.ToPort = newPort;
                }
                else
                {
                    //link.ToPort = funcVC.Ports.;
                    cancel = true;
                    break;
                }
            }

            //all these nodes can basically move 'into' the valuechain
            if (!cancel)
                foreach (IGoLink link in node.DestinationLinks)
                {
                    if (!(link is QLink))
                        continue;

                    QLink ql = link as QLink;
                    QuickPort qp = link.FromPort as QuickPort;
                    Shapes.General.QuickPortHelper.QuickPortLocation loc = qp.PortPosition;
                    //find port on VC
                    QuickPort newPort = null;
                    switch (loc)
                    {
                        case MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Top:
                            newPort = funcVC.ChildNames["topPort"] as QuickPort;
                            break;
                        case MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Bottom:
                            newPort = funcVC.ChildNames["bottomPort"] as QuickPort;
                            break;
                        case MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Left:
                            newPort = funcVC.ChildNames["leftPort"] as QuickPort;
                            break;
                        default:
                            newPort = funcVC.ChildNames["rightPort"] as QuickPort;
                            break;
                    }

                    if (newPort != null)
                    {
                        link.FromPort = newPort;
                    }
                    else
                    {
                        //link.ToPort = funcVC.Ports[1];
                        cancel = true;
                    }
                }

            #endregion

            UpdateAnchors(node, funcVC);
            //remove the original node
            if (!cancel)
                MyView.Document.Remove(node);
            else
                MyView.Document.Remove(funcVC);
            MyView.FinishTransaction("Convert Function to Value Chain");
        }

        //TODO : THREAD!
        private void ExportHierarchicalData(MetaBuilder.BusinessFacade.Exports.HierarchicalToText exporter, bool Numbered, bool Tabbed)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                //sfd.Title = "Enter a name for the exported file";
                sfd.Filter = "Text Files (.txt)|*.txt";
                sfd.FileName = "Hierarchical Export - " + strings.GetDateStampString();

                IMetaNode node = MyView.SelectedNodes[0] as IMetaNode;

                sfd.Title = "Enter a name for the exported file to export object " + node.MetaObject.ToString();
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    //exporter.MyClass = node.MetaObject.Class; //By limiting the class this feature becomes pointless IMHO => Jason Chladek
                    TList<Field> classFields = d.DataRepository.ClientFieldsByClass(node.MetaObject.Class);
                    foreach (Field f in classFields)
                        if (f.IsActive == true && (f.SortOrder == 0 || f.SortOrder == 1))
                            exporter.FieldToExport = f.Name;

                    NormalDiagram ndiagram = myView.Document as NormalDiagram;
                    if (ndiagram.VersionManager.CurrentVersion.PKID > 0)
                    {
                        exporter.ObjectID = node.MetaObject.pkid;
                        exporter.MachineName = node.MetaObject.MachineName;
                        exporter.Export(sfd.FileName, Numbered, Tabbed, ndiagram.VersionManager.CurrentVersion.PKID, ndiagram.VersionManager.CurrentVersion.MachineName);
                        MessageBox.Show("Export Completed", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        //When you open a diagram the current pkid is 0
                        if (ndiagram.VersionManager.CurrentVersion.PreviousDocumentID > 0)
                        {
                            exporter.ObjectID = node.MetaObject.pkid;
                            exporter.MachineName = node.MetaObject.MachineName;
                            exporter.Export(sfd.FileName, Numbered, Tabbed, ndiagram.VersionManager.CurrentVersion.PreviousDocumentID, ndiagram.VersionManager.CurrentVersion.MachineName);
                            MessageBox.Show("Export Completed", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                            MessageBox.Show("This document does not have a version." + Environment.NewLine + "Please save this document before exporting its objects", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteLog("ExportHierarchicalData - GraphViewController" + Environment.NewLine + ex.ToString());
                if (Core.Variables.Instance.VerboseLogging == true)
                    MessageBox.Show("A problem has occurred and has been logged. Please contact technical support for assistance", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("A problem has occurred, please contact technical support for assistance", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //returns a list of artefacts on link
        public Dictionary<ArtefactNodeLinkKey, QLink> FindArtefactsOnLink(QLink link, QLink newLink)
        {
            Dictionary<ArtefactNodeLinkKey, QLink> reconnectableArtefacts = new Dictionary<ArtefactNodeLinkKey, QLink>();

            FishLinkPort flp = null;
            foreach (GoObject o in (link.MidLabel as GoGroup))
            {
                if (o is FishLinkPort)
                {
                    flp = o as FishLinkPort;
                    break;
                }
            }

            if (flp != null && flp.SourceLinksCount > 0)
            {
                foreach (GoObject gobj in flp.SourceLinks)
                {
                    if (gobj is FishLink)
                    {
                        ArtefactNodeLinkKey k = new ArtefactNodeLinkKey((gobj as FishLink).FromNode as ArtefactNode, link);
                        reconnectableArtefacts.Add(k, newLink);
                    }
                    else if (gobj is FishRealLink)
                    {
                        ArtefactNodeLinkKey k = new ArtefactNodeLinkKey((gobj as FishRealLink).FromNode as ArtefactNode, link);
                        reconnectableArtefacts.Add(k, newLink);
                    }
                }
            }
            //Check this link for anchored comments (which includes rationales LOL)
            foreach (GoObject o in MyView.Document)
            {
                if (o is ResizableBalloonComment)
                {
                    ResizableBalloonComment com = o as ResizableBalloonComment;
                    if (reAnchorableComments.ContainsKey(com))
                        continue;

                    if (com.Anchor != null)
                    {
                        if (com.Anchor == link)
                            reAnchorableComments.Add(com, newLink);
                        //else if (com.Anchor is IMetaNode)
                        //    reAnchorableComments.Add(com, com.Anchor);
                        //else
                        //    reAnchorableComments.Add(com, com.Anchor);
                    }
                }
            }

            return reconnectableArtefacts;
        }
        private Dictionary<ArtefactNode, QLink> artefactsAlreadyRelinked;
        public void ConnectArtefact(ArtefactNode node, QLink link)
        {
            if (link == null)
                return;

            if (artefactsAlreadyRelinked == null)
                artefactsAlreadyRelinked = new Dictionary<ArtefactNode, QLink>();

            if (artefactsAlreadyRelinked.ContainsKey(node))
            {
                return;
            }
            //if (!MyView.Document.Contains(node))
            //{
            //    //add the node to the document
            //    MyView.Document.Add(node);
            //}

            QLink lnk = link;
            ArtefactNode n = new ArtefactNode();
            n.BindingInfo = new BindingInfo();
            n.BindingInfo.BindingClass = node.MetaObject.Class;
            n.MetaObject = node.MetaObject;
            n.Location = node.Location;
            n.Editable = true;
            n.Label.Editable = false;
            n.HookupEvents();
            n.BindToMetaObjectProperties();
            n.Location = link.MidLabel.Center;
            MyView.Document.Add(n);

            //foreach (KeyValuePair<ResizableBalloonComment, GoObject> kvp in reAnchorableComments)
            //{
            //    if (kvp.Value == node)
            //    {
            //        kvp.Key.Anchor = n;
            //        break;
            //    }
            //}

            GoGroup grp = lnk.MidLabel as GoGroup;
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
                    fishlink.Visible = ArtefactPointersVisible;
                    MyView.Document.Add(fishlink);
                    break;
                }
            }
            artefactsAlreadyRelinked.Add(node, link);
            MyView.Document.Remove(node);

            //FishLink fishLink = new FishLink();
            //fishLink.FromPort = node.Port;
            //fishLink.Visible = ArtefactPointersVisible;
            //foreach (GoObject o in (link.MidLabel as GoGroup))
            //{
            //    if (o is FishLinkPort)
            //    {
            //        FishLinkPort flp = o as FishLinkPort;
            //        fishLink.ToPort = flp;
            //    }
            //}

            //fishLink.ToQLink.ToString();

            //artefactsAlreadyRelinked.Add(node, link);
            //myView.Document.Add(fishLink);
            //fishLink.CalculateRoute();
            //fishLink.CalculateStroke();
        }
        public void FishLinkArtefact(ArtefactNode n, QLink lnk)
        {
            GoGroup grp = lnk.MidLabel as GoGroup;
            n.HookupEvents();
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
                    fishlink.Visible = ArtefactPointersVisible;
                    MyView.Document.Add(fishlink);
                    break;
                }
            }
        }

        public Dictionary<ResizableBalloonComment, GoObject> reAnchorableComments;
        //the idea is that any node can be converted to any other, collateral damage will be expected(detected) and therefore requires acceptance
        private void metaConvertItem_Click(object sender, EventArgs e)
        {
            MetaConvertSelection((sender as MenuItem).Text, false, false, "");
        }
        private void metaConvertImageItem_Click(object sender, EventArgs e)
        {
            myView.Document.SkipsUndoManager = true;
            myView.StartTransaction();
            string newClass = (sender as MenuItem).Text;

            AssociationHelper h = new AssociationHelper();
            Dictionary<IMetaNode, GoCollection> col = new Dictionary<IMetaNode, GoCollection>();

            #region Shallow Question

            bool shallows = false;
            int count = 0;
            foreach (GoObject o in MyView.Selection)
            {
                if (!(o is ImageNode))
                    continue;

                GoCollection shallowCol = new GoCollection();
                List<IMetaNode> shallowNodes = GetIMetaNodesBoundToMetaObject((o as IMetaNode).MetaObject);
                if (shallowNodes.Count > 0)
                {
                    foreach (IMetaNode imn in shallowNodes)
                    {
                        if (!(imn is ImageNode))
                            continue;

                        if (imn == o as IMetaNode)
                            continue;

                        //if (MyView.Selection.Contains(imn as GoObject))
                        //    continue;

                        shallowCol.Add(imn as GoObject);
                    }
                }

                col.Add(o as IMetaNode, shallowCol);
                count += shallowCol.Count;
            }

            if (count > 0)
                if (MessageBox.Show(count + " shallow cop" + (count == 1 ? "y exists" : "ies exist") + " on the current diagram, would you like to convert " + (count == 1 ? "it" : "them") + " as well?", "Meta Conversion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    shallows = true;

            #endregion

            //foreach (GoObject o in MyView.Selection)
            foreach (KeyValuePair<IMetaNode, GoCollection> o in col)
            {
                if (!(o.Key is ImageNode))
                    continue;

                MetaBase newMb = Loader.CreateInstance(newClass);
                ImageNode iNode = o.Key as ImageNode;

                //foreach of the links if it is not supported remove it
                Collection<QLink> invalid = new Collection<QLink>();
                foreach (IGoLink lnk in iNode.Links)
                {
                    if (!(lnk is QLink))
                        continue;

                    QLink l = lnk as QLink;
                    if (lnk.ToNode == iNode)
                    {
                        if (h.GetAssociation(newClass, iNode.MetaObject.Class, (int)l.AssociationType) == null)
                        {
                            invalid.Add(l);
                        }
                    }
                    else if (lnk.FromNode == iNode)
                    {
                        if (h.GetAssociation(iNode.MetaObject.Class, newClass, (int)l.AssociationType) == null)
                        {
                            invalid.Add(l);
                        }
                    }
                }
                if (invalid.Count > 0)
                {
                    if (MessageBox.Show("Some links do not support the conversion. Do you want to continue?", "Meta Convert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        break;
                }
                foreach (QLink l in invalid)
                {
                    //if an artefact on the link is not supported remove it as well
                    foreach (IMetaNode n in l.GetArtefacts())
                        (n as GoObject).Remove();
                    l.Remove();
                }

                iNode.MetaObject.CopyPropertiesToObject(newMb);
                iNode.MetaObject = newMb;
                iNode.HookupEvents();
                iNode.BindToMetaObjectProperties();

                if (shallows)
                    foreach (GoObject shallow in o.Value)
                        metaconvertShallowImageNode(shallow as ImageNode, newMb);

            }
            myView.FinishTransaction("MetaConvertImageSymbols");
            myView.Document.SkipsUndoManager = false;
        }

        private void metaconvertShallowImageNode(ImageNode iNode, MetaBase newMb)
        {
            AssociationHelper h = new AssociationHelper();
            //foreach of the links if it is not supported remove it
            Collection<QLink> invalid = new Collection<QLink>();
            foreach (IGoLink lnk in iNode.Links)
            {
                if (!(lnk is QLink))
                    continue;

                QLink l = lnk as QLink;
                if (lnk.ToNode == iNode)
                {
                    if (h.GetAssociation(newMb.Class, iNode.MetaObject.Class, (int)l.AssociationType) == null)
                    {
                        invalid.Add(l);
                    }
                }
                else if (lnk.FromNode == iNode)
                {
                    if (h.GetAssociation(iNode.MetaObject.Class, newMb.Class, (int)l.AssociationType) == null)
                    {
                        invalid.Add(l);
                    }
                }
            }
            if (invalid.Count > 0)
            {
                if (MessageBox.Show("Some shallow links do not support the conversion. Do you want to continue?", "Meta Convert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }
            foreach (QLink l in invalid)
            {
                //if an artefact on the link is not supported remove it as well
                foreach (IMetaNode n in l.GetArtefacts())
                    (n as GoObject).Remove();
                l.Remove();
            }

            iNode.MetaObject = newMb;
            iNode.HookupEvents();
            iNode.BindToMetaObjectProperties();
        }

        private void metaConvertItemShallow_Click(object sender, EventArgs e)
        {
            //MenuItem item = sender as MenuItem;
            //get class we want to change to (always be the same class)
            //string newClass = item.Text;
            try
            {
                Dictionary<string, Collection<IMetaNode>> nodesToConvert = new Dictionary<string, Collection<IMetaNode>>();
                foreach (GoObject o in MyView.Selection)
                {
                    if (!(o is GraphNode))
                        continue;

                    GraphNode node = o as GraphNode;
                    Collection<IMetaNode> internalNodes = null;
                    if (!(nodesToConvert.ContainsKey(node.MetaObject.Class)))
                    {
                        nodesToConvert.Add(node.MetaObject.Class, new Collection<IMetaNode>());
                    }
                    nodesToConvert.TryGetValue(node.MetaObject.Class, out internalNodes);
                    internalNodes.Add(node);
                }

                foreach (KeyValuePair<string, Collection<IMetaNode>> convertList in nodesToConvert)
                {
                    MyView.Selection.Clear();
                    foreach (IMetaNode n in convertList.Value)
                    {
                        MyView.Selection.Add(n as GoObject);
                    }
                    try
                    {
                        MetaConvertSelection(convertList.Key, true, false, "");
                    }
                    catch (Exception intEx)
                    {
                        Log.WriteLog("Shallow metaconvert to " + convertList.Key.ToString() + " failed" + Environment.NewLine + intEx.ToString());
                    }
                }
                //if (item.Tag is IMetaNode)
                //{
                //    MetaConvertSelection((item.Tag as IMetaNode).MetaObject.Class, true, false, "");
                //}
                //else
                //{
                //    //to shallow or not to shallow
                //    MetaConvertSelection(newClass, true);
                //}
            }
            catch (Exception ex)
            {
                Log.WriteLog("Shallow metaconvert discovery failed" + Environment.NewLine + ex.ToString());
            }
        }
        public void MetaConvertSelection(string newClass, bool ConvertWithMetaObject, bool AutoReplace, string oldClass)
        {
            MyView.StartTransaction();
            try
            {
                GraphNode cObject = null;
                try
                {
                    if (oldClass == "DataView")
                    {
                        newClass.ToString();
                    }
                    else if (oldClass == "DataViewLog")
                    {
                        oldClass = "DataView";
                    }
                    cObject = ((GraphNode)Variables.Instance.ReturnShape(newClass)).Copy() as GraphNode;
                }
                catch
                {
                    MessageBox.Show("A shape for the selected class (" + newClass + ") cannot be found in the shape cache.", "Operation Terminated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    {
                        MyView.FinishTransaction("MetaConvert to " + newClass + " - CANCELLED");
                        return;
                    }
                }

                GoCollection col = new GoCollection();
                GoCollection shallowCol = new GoCollection();

                bool shallows = false;
                if (!AutoReplace)
                {
                    #region Shallow Question
                    foreach (GoObject o in MyView.Selection)
                    {
                        if (o is IMetaNode && o is GraphNode)
                        {
                            col.Add(o);
                            //find all shallow objects of each of the selected objects and add them to the collection
                            List<IMetaNode> shallowNodes = GetIMetaNodesBoundToMetaObject((o as IMetaNode).MetaObject);
                            if (shallowNodes.Count > 0)
                            {
                                foreach (IMetaNode imn in shallowNodes)
                                {
                                    if (imn == o as IMetaNode || MyView.Selection.Contains(imn as GoObject))
                                        continue;
                                    shallowCol.Add(imn as GoObject);
                                }
                            }
                        }
                    }

                    if (shallowCol.Count > 0)
                    {
                        if (MessageBox.Show(shallowCol.Count + " unselected shallow cop" + (shallowCol.Count == 1 ? "y exists" : "ies exist") + " on the current diagram, would you like to convert " + (shallowCol.Count == 1 ? "it" : "them") + " as well?", "Meta Conversion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            shallows = true;
                            foreach (IMetaNode n in shallowCol)
                                if (n is GraphNode && !(MyView.Selection.Contains(n as GoObject)) && !(col.Contains(n as GoObject))) //must be a graphnode and must not already be in the collection or selection
                                    col.Add(n as GoObject);
                        }
                    }
                    #endregion
                }
                else
                {
                    foreach (GoObject o in MyView.Document)
                    {
                        IMetaNode imn = o as IMetaNode;
                        if (imn != null)
                        {
                            if (o is GraphNode)
                            {
                                if (imn.MetaObject.Class.ToLower() == oldClass.ToLower() || imn.MetaObject.Class.ToLower() == newClass.ToLower())
                                    col.Add(o);
                            }
                        }
                    }
                }
                if (col.Count == 0)
                {
                    MyView.FinishTransaction("MetaConvert to " + newClass + " - no items");
                    return;
                }
                MetaConvert mc = new MetaConvert(col, newClass, cObject);
                MetaConvert.ConvertShallowsToSameMetabase = shallows;
                if (!AutoReplace)
                {
                    if (mc.Result != MetaConvertInfo.OK)
                    {
                        Core.Log.WriteLog(string.Join(Environment.NewLine, mc.Report().ToArray()));
                        //show advanced log?
                        if (MessageBox.Show("Applying this conversion (To " + newClass + ") to the selected shapes will causes some links and/or artefacts to be lost." + Environment.NewLine + "Would you like to continue?", "Meta Conversion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        {
                            MyView.FinishTransaction("MetaConvert to " + newClass + " - CANCELLED");
                            return;
                        }
                    }
                }
                MetaConverting = true;
                mc.Convert(MyView.Document, ConvertWithMetaObject);

                MyView.Selection.Clear();
                MyView.Selection.AddRange(mc.Selection);

                OnMetaConvertComplete(mc, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Log.WriteLog("GraphViewController.MetaConvertSelection" + Environment.NewLine + ex.ToString());
                MessageBox.Show("A problem has occurred. The metaconversion was not completed properly.", "MetaConvert Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            MyView.FinishTransaction("MetaConvert to " + newClass);

            return;

            #region OLD

            GoCollection selection = new GoCollection();

            List<GraphNode> yesNodes = new List<GraphNode>();
            List<GraphNode> removeNodes = new List<GraphNode>();
            List<QLink> newLinks = new List<QLink>();
            List<QLink> noAssociations = new List<QLink>();
            List<ArtefactNode> noArtefacts = new List<ArtefactNode>();
            GraphNode coreObject = null;
            try
            {
                coreObject = ((GraphNode)Variables.Instance.ReturnShape(newClass)).Copy() as GraphNode;
                if (coreObject == null)
                {
                    MessageBox.Show("A shape for the selected class (" + newClass + ") cannot be found in the shape cache.", "Operation Terminated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch
            {
                MessageBox.Show("A shape for the selected class (" + newClass + ") cannot be found in the shape cache.", "Operation Terminated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation defaultFrom = (MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation)Enum.Parse(typeof(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation), Core.Variables.Instance.DefaultFromPort);
            MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation defaultTo = (MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation)Enum.Parse(typeof(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation), Core.Variables.Instance.DefaultToPort);
            Dictionary<ArtefactNodeLinkKey, QLink> reconnectableArtefacts = new Dictionary<ArtefactNodeLinkKey, QLink>();
            reAnchorableComments = new Dictionary<ResizableBalloonComment, GoObject>(); //get set externally and internally to this method
            List<GraphNode> nodesAdded = new List<GraphNode>();

            //if 1 node does not support the change notify and cancel(choice)
            //notify at end of discovery
            Dictionary<GraphNode, SubgraphNode> nodesInSubGraph = new Dictionary<GraphNode, SubgraphNode>();
            Dictionary<GraphNode, GraphNode> oldAndNewNode = new Dictionary<GraphNode, GraphNode>();

            #region Test

            foreach (GraphNode gNode in MyView.Selection)
            {
                selection.Add(gNode);

                GraphNode newNodeObject = MetaConvert.ConvertObject(gNode, coreObject, ConvertWithMetaObject);
                if (oldAndNewNode.ContainsKey(gNode))
                    oldAndNewNode.TryGetValue(gNode, out  newNodeObject);
                else
                    oldAndNewNode.Add(gNode, newNodeObject);

                //if (gNode.Parent is SubgraphNode)
                //{
                //    gNode.ParentIsILinkedContainer = true;
                //    nodesInSubGraph.Add(newNodeObject, gNode.Parent as SubgraphNode);
                //}
                nodesAdded.Add(newNodeObject);

                //Check this link for anchored comments (which includes rationales LOL)
                //foreach (GoObject o in MyView.Document)
                //{
                //    if (o is ResizableBalloonComment)
                //    {
                //        ResizableBalloonComment com = o as ResizableBalloonComment;
                //        if (com.Anchor != null && com.Anchor == gNode)
                //        {
                //            if (!reAnchorableComments.ContainsKey(com))
                //            {
                //                //com.Reanchorable = true;
                //                //com.Anchor = null;
                //                reAnchorableComments.Add(com, newNodeObject);
                //            }
                //        }
                //    }
                //}

                #region Links
                if (gNode.SourceLinks.Count > 0)
                {
                    foreach (QLink Link in gNode.SourceLinks.GetEnumerator())
                    {
                        ClassAssociation newLink = null;
                        IMetaNode fromNode = (Link.FromNode as IMetaNode);
                        string fromClass;
                        GraphNode newNodeObjectChild = null;
                        if (MyView.Selection.Contains(Link.FromNode as GoObject))
                        {
                            GraphNode childNode = (Link.FromNode as GraphNode);
                            newNodeObjectChild = MetaConvert.ConvertObject(childNode, coreObject, ConvertWithMetaObject);
                            if (oldAndNewNode.ContainsKey(childNode))
                                oldAndNewNode.TryGetValue(childNode, out newNodeObjectChild);
                            else
                                oldAndNewNode.Add(childNode, newNodeObjectChild);

                            //we are converting the fromnode to the newClass so we must do a seperate check
                            fromClass = newClass;
                            //check if newclass + fromClass + associationType has a CAID

                            foreach (ClassAssociation caFrom in d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByChildClass(newClass))
                            {
                                if (caFrom.ParentClass == fromClass)
                                {
                                    string dbTypeName = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.AssociationTypeProvider.GetBypkid(caFrom.AssociationTypeID).Name.Trim().ToLower();
                                    string lTypeName = Link.AssociationType.ToString().Trim().ToLower();
                                    if (dbTypeName == lTypeName)
                                    {
                                        newLink = caFrom;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            fromClass = fromNode.MetaObject.Class;
                            //check if newclass + toClass + associationType has a CAID

                            foreach (ClassAssociation caFrom in d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByChildClass(newClass))
                            {
                                if (caFrom.ParentClass == fromClass)
                                {
                                    string dbTypeName = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.AssociationTypeProvider.GetBypkid(caFrom.AssociationTypeID).Name.Trim().ToLower();
                                    string lTypeName = Link.AssociationType.ToString().Trim().ToLower();
                                    if (dbTypeName == lTypeName)
                                    {
                                        newLink = caFrom;
                                        break;
                                    }
                                }
                            }
                        }

                        //we must change this link to the newDestinationLink for this graphnode
                        if (newLink != null)
                        {
                            QLink theNewLink = null;
                            IGoPort p = MetaConvert.findPortAtPositionOfOldPortForNewNode(Link.ToPort as QuickPort, newNodeObject);
                            if (p == null)
                            {
                                if (newNodeObjectChild != null)
                                    theNewLink = QLink.CreateLink(newNodeObjectChild as GoNode, newNodeObject, newLink.AssociationTypeID, defaultFrom, defaultTo);
                                else
                                    theNewLink = QLink.CreateLink(Link.FromNode as GoNode, newNodeObject, newLink.AssociationTypeID, defaultFrom, defaultTo);
                            }
                            else
                            {
                                if (newNodeObjectChild != null)
                                    theNewLink = QLink.CreateLink(newNodeObjectChild as GoNode, newNodeObject, newLink.AssociationTypeID, Link.FromPort as GoPort, p as GoPort);
                                else
                                    theNewLink = QLink.CreateLink(Link.FromNode as GoNode, newNodeObject, newLink.AssociationTypeID, Link.FromPort as GoPort, p as GoPort);
                            }
                            theNewLink.RealLink.SetPoints(Link.RealLink.CopyPointsArray());
                            theNewLink.PenColorBeforeCompare = Link.Pen.Color;
                            theNewLink.Pen = new Pen(new SolidBrush(Link.Pen.Color));
                            newLinks.Add(theNewLink);

                            foreach (KeyValuePair<ArtefactNodeLinkKey, QLink> kvp in FindArtefactsOnLink(Link, theNewLink))
                            {
                                string checkArtefactClassT = kvp.Key.Node.MetaObject.Class.ToString();
                                //check if newlink can have this class of artefact
                                if (DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.AllowedArtifactProvider.GetByCAidClass(newLink.CAid, checkArtefactClassT) == null)
                                    if (!noArtefacts.Contains(kvp.Key.Node))
                                        noArtefacts.Add(kvp.Key.Node);
                                //if (!reconnectableArtefacts.ContainsKey(kvp.Key))
                                //    reconnectableArtefacts.Add(kvp.Key, theNewLink);

                                bool add = true;
                                foreach (KeyValuePair<ArtefactNodeLinkKey, QLink> kvpRA in reconnectableArtefacts)
                                {
                                    if (kvp.Key.Node == kvpRA.Key.Node && kvp.Key.NodesLink == kvpRA.Key.NodesLink)
                                    {
                                        add = false;
                                        break;
                                    }
                                }
                                if (add)
                                    reconnectableArtefacts.Add(kvp.Key, kvp.Value);
                            }
                        }
                        else
                            if (!noAssociations.Contains(Link))
                                noAssociations.Add(Link);

                        if (!(yesNodes.Contains(newNodeObject)))
                            yesNodes.Add(newNodeObject);
                    }
                }
                else
                {
                    if (!(yesNodes.Contains(newNodeObject)))
                        yesNodes.Add(newNodeObject);
                }

                #endregion

                #region DestinationLinks
                if (gNode.DestinationLinks.Count > 0)
                {
                    foreach (QLink destinationLink in gNode.DestinationLinks.GetEnumerator())
                    {
                        ClassAssociation newDestinationLink = null;
                        IMetaNode toNode = (destinationLink.ToNode as IMetaNode);
                        string toClass;
                        GraphNode newNodeObjectChild = null;
                        if (MyView.Selection.Contains(destinationLink.ToNode as GoObject))
                        {
                            GraphNode childNode = (destinationLink.ToNode as GraphNode);
                            newNodeObjectChild = MetaConvert.ConvertObject(childNode, coreObject, ConvertWithMetaObject);
                            if (oldAndNewNode.ContainsKey(childNode))
                                oldAndNewNode.TryGetValue(childNode, out newNodeObjectChild);
                            else
                                oldAndNewNode.Add(childNode, newNodeObjectChild);

                            //we are converting the tonode to the newClass so we must do a seperate check
                            toClass = newClass;
                            //check if newclass + toClass + associationType has a CAID

                            foreach (ClassAssociation caTo in d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByParentClass(newClass))
                            {
                                if (caTo.ChildClass == toClass)
                                {
                                    string dbTypeName = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.AssociationTypeProvider.GetBypkid(caTo.AssociationTypeID).Name.Trim().ToLower();
                                    string lTypeName = destinationLink.AssociationType.ToString().Trim().ToLower();
                                    if (dbTypeName == lTypeName)
                                    {
                                        newDestinationLink = caTo;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            toClass = toNode.MetaObject.Class;
                            //check if newclass + toClass + associationType has a CAID
                            foreach (ClassAssociation caTo in d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByParentClass(newClass))
                            {
                                if (caTo.ChildClass == toClass)
                                {
                                    string dbTypeName = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.AssociationTypeProvider.GetBypkid(caTo.AssociationTypeID).Name.Trim().ToLower();
                                    string lTypeName = destinationLink.AssociationType.ToString().Trim().ToLower();
                                    if (dbTypeName == lTypeName)
                                    {
                                        newDestinationLink = caTo;
                                        break;
                                    }
                                }
                            }
                        }

                        //we must change this link to the newDestinationLink for this graphnode
                        if (newDestinationLink != null)
                        {
                            QLink theNewLinkD = null;
                            IGoPort p = MetaConvert.findPortAtPositionOfOldPortForNewNode(destinationLink.FromPort as QuickPort, newNodeObject);
                            if (p == null)
                            {
                                if (newNodeObjectChild != null)
                                    theNewLinkD = QLink.CreateLink(newNodeObject, newNodeObjectChild as GoNode, newDestinationLink.AssociationTypeID, defaultFrom, defaultTo);
                                else
                                    theNewLinkD = QLink.CreateLink(newNodeObject, destinationLink.ToNode as GoNode, newDestinationLink.AssociationTypeID, defaultFrom, defaultTo);
                            }
                            else
                            {
                                if (newNodeObjectChild != null)
                                    theNewLinkD = QLink.CreateLink(newNodeObject, newNodeObjectChild as GoNode, newDestinationLink.AssociationTypeID, p as GoPort, destinationLink.ToPort as GoPort);
                                else
                                    theNewLinkD = QLink.CreateLink(newNodeObject, destinationLink.ToNode as GoNode, newDestinationLink.AssociationTypeID, p as GoPort, destinationLink.ToPort as GoPort);
                            }
                            theNewLinkD.RealLink.SetPoints(destinationLink.RealLink.CopyPointsArray());
                            theNewLinkD.PenColorBeforeCompare = destinationLink.Pen.Color;
                            theNewLinkD.Pen = new Pen(new SolidBrush(destinationLink.Pen.Color));
                            newLinks.Add(theNewLinkD);

                            foreach (KeyValuePair<ArtefactNodeLinkKey, QLink> kvp in FindArtefactsOnLink(destinationLink, theNewLinkD))
                            {
                                string checkArtefactClass = kvp.Key.Node.MetaObject.Class.ToString();
                                //check if newlink can have this class of artefact
                                if (DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.AllowedArtifactProvider.GetByCAidClass(newDestinationLink.CAid, checkArtefactClass) == null)
                                    if (!noArtefacts.Contains(kvp.Key.Node))
                                        noArtefacts.Add(kvp.Key.Node);
                                //if (!reconnectableArtefacts.ContainsKey(kvp.Key))
                                //    reconnectableArtefacts.Add(kvp.Key, theNewLinkD);

                                bool add = true;
                                foreach (KeyValuePair<ArtefactNodeLinkKey, QLink> kvpRA in reconnectableArtefacts)
                                {
                                    if (kvp.Key.Node == kvpRA.Key.Node && kvp.Key.NodesLink == kvpRA.Key.NodesLink)
                                    {
                                        add = false;
                                        break;
                                    }
                                }
                                if (add)
                                    reconnectableArtefacts.Add(kvp.Key, kvp.Value);
                            }
                            //this node must be relinked with this class association to its childnode
                            //if (!(yesDestinationAssociations.ContainsKey(destinationLink)))
                            //    yesDestinationAssociations.Add(destinationLink, newDestinationLink);
                        }
                        else
                            if (!(noAssociations.Contains(destinationLink)))
                                noAssociations.Add(destinationLink);

                        if (!(yesNodes.Contains(newNodeObject)))
                            yesNodes.Add(newNodeObject);
                    }
                }
                else
                {
                    if (!(yesNodes.Contains(newNodeObject)))
                        yesNodes.Add(newNodeObject);
                }

                #endregion

                removeNodes.Add(gNode);
            }

            //do not question me if I am shallow copying :)
            if (!ConvertWithMetaObject)
            {
                DialogResult res = DialogResult.Yes;
                string msg = "";
                if (noAssociations.Count > 0)
                {
                    if (noAssociations.Count == 1)
                        msg = "A link cannot be recreated and will be lost if you continue to convert the selected objects." + Environment.NewLine;
                    else
                        msg = noAssociations.Count + " Links cannot be recreated and will be lost if you continue to convert the selected objects." + Environment.NewLine;

                    if (noArtefacts.Count > 0)
                    {
                        if (noArtefacts.Count == 1)
                            msg += Environment.NewLine + "Furthermore the Artefact (" + noArtefacts[0].MetaObject.ToString() + ") cannot be connected to the new link and will also be lost if you continue." + Environment.NewLine;
                        else
                            msg += Environment.NewLine + "Furthermore (" + noArtefacts.Count + ") Artefacts of type (" + noArtefacts[0].MetaObject.Class + ") cannot be connected to the new links and will also be lost if you continue." + Environment.NewLine;
                    }
                    //cannot convert some associations, continuing will cause them to be lost
                    res = MessageBox.Show(msg + "Would you like to continue?", "Meta Conversion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (res == DialogResult.No)
                        return;
                }
                else if (noArtefacts.Count > 0)
                {
                    if (noArtefacts.Count == 1)
                        msg = "The Artefact (" + noArtefacts[0].MetaObject.ToString() + ") cannot be connected to the new link and will be lost if you continue." + Environment.NewLine;
                    else
                        msg = noArtefacts.Count + " Artefacts of type (" + noArtefacts[0].MetaObject.Class + ") cannot be connected to the new links and will be lost if you continue." + Environment.NewLine;

                    res = MessageBox.Show(msg + "Would you like to continue?", "Meta Conversion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (res == DialogResult.No)
                        return;
                }
            }

            #endregion

            MyView.Selection.Clear();
            GoCollection newNodes = new GoCollection();
            oldAndNewNode = new Dictionary<GraphNode, GraphNode>();
            foreach (GraphNode gNode in selection)
            {
                yesNodes = new List<GraphNode>();
                removeNodes = new List<GraphNode>();
                newLinks = new List<QLink>();
                noAssociations = new List<QLink>();
                noArtefacts = new List<ArtefactNode>();
                reconnectableArtefacts = new Dictionary<ArtefactNodeLinkKey, QLink>();
                reAnchorableComments = new Dictionary<ResizableBalloonComment, GoObject>(); //get set externally and internally to this method
                nodesAdded = new List<GraphNode>();
                nodesInSubGraph = new Dictionary<GraphNode, SubgraphNode>();

                GraphNode newNodeObject = MetaConvert.ConvertObject(gNode, coreObject, ConvertWithMetaObject);
                if (oldAndNewNode.ContainsKey(gNode))
                    oldAndNewNode.TryGetValue(gNode, out  newNodeObject);
                else
                    oldAndNewNode.Add(gNode, newNodeObject);

                if (gNode.Parent is SubgraphNode)
                {
                    gNode.ParentIsILinkedContainer = true;
                    nodesInSubGraph.Add(newNodeObject, gNode.Parent as SubgraphNode);
                    foreach (GoObject o in (gNode.Parent as SubgraphNode))
                    {
                        if (o is ResizableBalloonComment)
                        {
                            ResizableBalloonComment com = o as ResizableBalloonComment;
                            if (com.Anchor != null && com.Anchor == gNode)
                            {
                                if (!reAnchorableComments.ContainsKey(com))
                                {
                                    //com.Reanchorable = true;
                                    //com.Anchor = null;
                                    reAnchorableComments.Add(com, newNodeObject);
                                }
                            }
                        }
                    }
                }
                else
                {
                    //Check this link for anchored comments (which includes rationales LOL)
                    foreach (GoObject o in MyView.Document)
                    {
                        if (o is ResizableBalloonComment)
                        {
                            ResizableBalloonComment com = o as ResizableBalloonComment;
                            if (com.Anchor != null && com.Anchor == gNode)
                            {
                                if (!reAnchorableComments.ContainsKey(com))
                                {
                                    //com.Reanchorable = true;
                                    //com.Anchor = null;
                                    reAnchorableComments.Add(com, newNodeObject);
                                }
                            }
                        }
                    }
                }

                nodesAdded.Add(newNodeObject);

                List<string> affectedlinks = new List<string>();
                //caid,toid,tomachine,fromid,frommachine

                #region Links
                if (gNode.SourceLinks.Count > 0)
                {
                    foreach (QLink Link in gNode.SourceLinks.GetEnumerator())
                    {
                        //string linkKey = getassociationkey(Link);
                        //if (!(string.IsNullOrEmpty(linkKey)))
                        //    if (affectedlinks.Contains(linkKey))
                        //        continue;
                        //    else
                        //        affectedlinks.Add(linkKey);

                        ClassAssociation newLink = null;
                        IMetaNode fromNode = (Link.FromNode as IMetaNode);
                        string fromClass;
                        GraphNode newNodeObjectChild = null;
                        if (MyView.Selection.Contains(Link.FromNode as GoObject))
                        {
                            GraphNode childNode = (Link.FromNode as GraphNode);
                            newNodeObjectChild = MetaConvert.ConvertObject(childNode, coreObject, ConvertWithMetaObject);
                            if (oldAndNewNode.ContainsKey(childNode))
                                oldAndNewNode.TryGetValue(childNode, out newNodeObjectChild);
                            else
                                oldAndNewNode.Add(childNode, newNodeObjectChild);

                            //we are converting the fromnode to the newClass so we must do a seperate check
                            fromClass = newClass;
                            //check if newclass + fromClass + associationType has a CAID

                            foreach (ClassAssociation caFrom in d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByChildClass(newClass))
                            {
                                if (caFrom.ParentClass == fromClass)
                                {
                                    string dbTypeName = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.AssociationTypeProvider.GetBypkid(caFrom.AssociationTypeID).Name.Trim().ToLower();
                                    string lTypeName = Link.AssociationType.ToString().Trim().ToLower();
                                    if (dbTypeName == lTypeName)
                                    {
                                        newLink = caFrom;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            fromClass = fromNode.MetaObject.Class;
                            //check if newclass + toClass + associationType has a CAID

                            foreach (ClassAssociation caFrom in d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByChildClass(newClass))
                            {
                                if (caFrom.ParentClass == fromClass)
                                {
                                    string dbTypeName = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.AssociationTypeProvider.GetBypkid(caFrom.AssociationTypeID).Name.Trim().ToLower();
                                    string lTypeName = Link.AssociationType.ToString().Trim().ToLower();
                                    if (dbTypeName == lTypeName)
                                    {
                                        newLink = caFrom;
                                        break;
                                    }
                                }
                            }
                        }

                        //we must change this link to the newDestinationLink for this graphnode
                        if (newLink != null)
                        {
                            QLink theNewLink = null;
                            IGoPort p = MetaConvert.findPortAtPositionOfOldPortForNewNode(Link.ToPort as QuickPort, newNodeObject);
                            if (p == null)
                            {
                                if (newNodeObjectChild != null)
                                    theNewLink = QLink.CreateLink(newNodeObjectChild as GoNode, newNodeObject, newLink.AssociationTypeID, defaultFrom, defaultTo);
                                else
                                    theNewLink = QLink.CreateLink(Link.FromNode as GoNode, newNodeObject, newLink.AssociationTypeID, defaultFrom, defaultTo);
                            }
                            else
                            {
                                if (newNodeObjectChild != null)
                                {
                                    IGoPort pp = MetaConvert.findPortAtPositionOfOldPortForNewNode(Link.FromPort as QuickPort, newNodeObjectChild);
                                    theNewLink = QLink.CreateLink(newNodeObjectChild as GoNode, newNodeObject, newLink.AssociationTypeID, pp as GoPort, p as GoPort);
                                }
                                else
                                    theNewLink = QLink.CreateLink(Link.FromNode as GoNode, newNodeObject, newLink.AssociationTypeID, Link.FromPort as GoPort, p as GoPort);
                            }
                            theNewLink.RealLink.SetPoints(Link.RealLink.CopyPointsArray());
                            theNewLink.PenColorBeforeCompare = Link.Pen.Color;
                            theNewLink.Pen = new Pen(new SolidBrush(Link.Pen.Color));
                            theNewLink.RealLink.AvoidsNodes = Link.RealLink.AvoidsNodes;
                            theNewLink.RealLink.AdjustingStyle = Link.RealLink.AdjustingStyle;
                            newLinks.Add(theNewLink);

                            foreach (KeyValuePair<ArtefactNodeLinkKey, QLink> kvp in FindArtefactsOnLink(Link, theNewLink))
                            {
                                string checkArtefactClassT = kvp.Key.Node.MetaObject.Class.ToString();
                                //check if newlink can have this class of artefact
                                if (DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.AllowedArtifactProvider.GetByCAidClass(newLink.CAid, checkArtefactClassT) == null)
                                    if (!noArtefacts.Contains(kvp.Key.Node))
                                        noArtefacts.Add(kvp.Key.Node);
                                //if (!reconnectableArtefacts.ContainsKey(kvp.Key))
                                //    reconnectableArtefacts.Add(kvp.Key, theNewLink);

                                bool add = true;
                                foreach (KeyValuePair<ArtefactNodeLinkKey, QLink> kvpRA in reconnectableArtefacts)
                                {
                                    if (kvp.Key.Node == kvpRA.Key.Node && kvp.Key.NodesLink == kvpRA.Key.NodesLink)
                                    {
                                        add = false;
                                        break;
                                    }
                                }
                                if (add)
                                    reconnectableArtefacts.Add(kvp.Key, kvp.Value);
                            }
                        }
                        else
                            if (!noAssociations.Contains(Link))
                                noAssociations.Add(Link);

                        if (!(yesNodes.Contains(newNodeObject)))
                            yesNodes.Add(newNodeObject);
                    }
                }
                else
                {
                    if (!(yesNodes.Contains(newNodeObject)))
                        yesNodes.Add(newNodeObject);
                }

                #endregion

                #region DestinationLinks
                if (gNode.DestinationLinks.Count > 0)
                {
                    foreach (QLink destinationLink in gNode.DestinationLinks.GetEnumerator())
                    {
                        //string linkKey = getassociationkey(destinationLink);
                        //if (!(string.IsNullOrEmpty(linkKey)))
                        //    if (affectedlinks.Contains(linkKey))
                        //        continue;
                        //    else
                        //        affectedlinks.Add(linkKey);

                        ClassAssociation newDestinationLink = null;
                        IMetaNode toNode = (destinationLink.ToNode as IMetaNode);
                        string toClass;
                        GraphNode newNodeObjectChild = null;
                        if (MyView.Selection.Contains(destinationLink.ToNode as GoObject))
                        {
                            GraphNode childNode = (destinationLink.ToNode as GraphNode);
                            newNodeObjectChild = MetaConvert.ConvertObject(childNode, coreObject, ConvertWithMetaObject);
                            if (oldAndNewNode.ContainsKey(childNode))
                                oldAndNewNode.TryGetValue(childNode, out newNodeObjectChild);
                            else
                                oldAndNewNode.Add(childNode, newNodeObjectChild);

                            //we are converting the tonode to the newClass so we must do a seperate check
                            toClass = newClass;
                            //check if newclass + toClass + associationType has a CAID

                            foreach (ClassAssociation caTo in d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByParentClass(newClass))
                            {
                                if (caTo.ChildClass == toClass)
                                {
                                    string dbTypeName = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.AssociationTypeProvider.GetBypkid(caTo.AssociationTypeID).Name.Trim().ToLower();
                                    string lTypeName = destinationLink.AssociationType.ToString().Trim().ToLower();
                                    if (dbTypeName == lTypeName)
                                    {
                                        newDestinationLink = caTo;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            toClass = toNode.MetaObject.Class;
                            //check if newclass + toClass + associationType has a CAID
                            foreach (ClassAssociation caTo in d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByParentClass(newClass))
                            {
                                if (caTo.ChildClass == toClass)
                                {
                                    string dbTypeName = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.AssociationTypeProvider.GetBypkid(caTo.AssociationTypeID).Name.Trim().ToLower();
                                    string lTypeName = destinationLink.AssociationType.ToString().Trim().ToLower();
                                    if (dbTypeName == lTypeName)
                                    {
                                        newDestinationLink = caTo;
                                        break;
                                    }
                                }
                            }
                        }

                        //we must change this link to the newDestinationLink for this graphnode
                        if (newDestinationLink != null)
                        {
                            QLink theNewLinkD = null;
                            IGoPort p = MetaConvert.findPortAtPositionOfOldPortForNewNode(destinationLink.FromPort as QuickPort, newNodeObject);
                            if (p == null)
                            {
                                if (newNodeObjectChild != null)
                                    theNewLinkD = QLink.CreateLink(newNodeObject, newNodeObjectChild as GoNode, newDestinationLink.AssociationTypeID, defaultFrom, defaultTo);
                                else
                                    theNewLinkD = QLink.CreateLink(newNodeObject, destinationLink.ToNode as GoNode, newDestinationLink.AssociationTypeID, defaultFrom, defaultTo);
                            }
                            else
                            {
                                if (newNodeObjectChild != null)
                                {
                                    IGoPort pp = MetaConvert.findPortAtPositionOfOldPortForNewNode(destinationLink.ToPort as QuickPort, newNodeObjectChild);
                                    theNewLinkD = QLink.CreateLink(newNodeObject, newNodeObjectChild as GoNode, newDestinationLink.AssociationTypeID, p as GoPort, pp as GoPort);
                                }
                                else
                                    theNewLinkD = QLink.CreateLink(newNodeObject, destinationLink.ToNode as GoNode, newDestinationLink.AssociationTypeID, p as GoPort, destinationLink.ToPort as GoPort);
                            }
                            theNewLinkD.RealLink.SetPoints(destinationLink.RealLink.CopyPointsArray());
                            theNewLinkD.PenColorBeforeCompare = destinationLink.Pen.Color;
                            theNewLinkD.Pen = new Pen(new SolidBrush(destinationLink.Pen.Color));
                            theNewLinkD.RealLink.AvoidsNodes = destinationLink.RealLink.AvoidsNodes;
                            theNewLinkD.RealLink.AdjustingStyle = destinationLink.RealLink.AdjustingStyle;
                            newLinks.Add(theNewLinkD);

                            foreach (KeyValuePair<ArtefactNodeLinkKey, QLink> kvp in FindArtefactsOnLink(destinationLink, theNewLinkD))
                            {
                                string checkArtefactClass = kvp.Key.Node.MetaObject.Class.ToString();
                                //check if newlink can have this class of artefact
                                if (DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.AllowedArtifactProvider.GetByCAidClass(newDestinationLink.CAid, checkArtefactClass) == null)
                                    if (!noArtefacts.Contains(kvp.Key.Node))
                                        noArtefacts.Add(kvp.Key.Node);
                                //if (!reconnectableArtefacts.ContainsKey(kvp.Key))
                                //    reconnectableArtefacts.Add(kvp.Key, theNewLinkD);

                                bool add = true;
                                foreach (KeyValuePair<ArtefactNodeLinkKey, QLink> kvpRA in reconnectableArtefacts)
                                {
                                    if (kvp.Key.Node == kvpRA.Key.Node && kvp.Key.NodesLink == kvpRA.Key.NodesLink)
                                    {
                                        add = false;
                                        break;
                                    }
                                }
                                if (add)
                                    reconnectableArtefacts.Add(kvp.Key, kvp.Value);
                            }
                            //this node must be relinked with this class association to its childnode
                            //if (!(yesDestinationAssociations.ContainsKey(destinationLink)))
                            //    yesDestinationAssociations.Add(destinationLink, newDestinationLink);
                        }
                        else
                            if (!(noAssociations.Contains(destinationLink)))
                                noAssociations.Add(destinationLink);

                        if (!(yesNodes.Contains(newNodeObject)))
                            yesNodes.Add(newNodeObject);
                    }
                }
                else
                {
                    if (!(yesNodes.Contains(newNodeObject)))
                        yesNodes.Add(newNodeObject);
                }

                #endregion

                removeNodes.Add(gNode);

                #region Affect the view

                myView.StartTransaction();

                foreach (ArtefactNode aNode in noArtefacts)
                    MyView.Document.Remove(aNode);

                foreach (GraphNode n in nodesAdded)
                {
                    newNodes.Add(n);
                    bool addedinloop = false;
                    //if n is in dictionary add to subgraphnode instead
                    foreach (KeyValuePair<GraphNode, SubgraphNode> kvp in nodesInSubGraph)
                    {
                        if (n == kvp.Key)
                        {
                            addedinloop = true;
                            GoCollection col = new GoCollection();
                            col.Add(n);

                            //if (!MyView.Document.Contains(n))
                            //    MyView.Document.Add(n);
                            kvp.Value.Add(n);
                            kvp.Value.PerformAddCollection(MyView, col);
                            n.ParentIsILinkedContainer = true;
                            break;
                        }
                    }
                    if (!addedinloop)
                        MyView.Document.Add(n);

                    //MyView.Selection.Add(n);
                }

                foreach (QLink l in newLinks)
                {
                    if (MyView.Document.Contains(l.FromNode as GoObject) && MyView.Document.Contains(l.ToNode as GoObject))
                    {
                        MyView.Document.Add(l);
                    }
                }

                foreach (GraphNode o in removeNodes)
                    MyView.Document.Remove(o);

                artefactsAlreadyRelinked = new Dictionary<ArtefactNode, QLink>();
                foreach (KeyValuePair<ArtefactNodeLinkKey, QLink> kvp in reconnectableArtefacts)
                    ConnectArtefact(kvp.Key.Node, kvp.Value);

                foreach (KeyValuePair<ResizableBalloonComment, GoObject> kvp in reAnchorableComments)
                {
                    kvp.Key.Reanchorable = true;
                    kvp.Key.Anchor = kvp.Value;
                }

                myView.Document.FinishTransaction("Metaconvert");

                #endregion

            }

            //foreach (GoObject o in newNodes)
            MyView.Selection.AddRange(newNodes);

            #endregion
        }

        private bool metaConverting;
        public bool MetaConverting
        {
            get { return metaConverting; }
            set { metaConverting = value; }
        }

        private void convertSymbol_Click(object sender, EventArgs e)
        {
            MyView.StartTransaction();
            Collection<GoObject> removeNodes = new Collection<GoObject>();
            foreach (GoObject obj in MyView.Selection)
            {
                if (!(obj is IMetaNode))
                    continue;

                #region Keep diagram allocation
                Collection<AllocationHandle.AllocationItem> items = null;
                if (obj is GoGroup)
                {
                    foreach (GoObject o in obj as GoGroup)
                    {
                        if (o is AllocationHandle)
                        {
                            items = (o as AllocationHandle).Items;
                            break;
                        }
                    }
                }
                #endregion

                IMetaNode node = obj as IMetaNode;
                bool imageNode = node is ImageNode;

                if (!imageNode)
                {
                    ImageNode iNode = new ImageNode();
                    iNode.AllocationHandle.Items = items;
                    iNode.ImageFilename = (node as GraphNode).ImageFilename;
                    iNode.MetaObject = node.MetaObject;

                    iNode.HookupEvents();
                    iNode.Position = (node as GoNode).Position;
                    iNode.BindingInfo = node.BindingInfo.Copy();
                    iNode.BindToMetaObjectProperties();

                    if (obj.Parent != obj)
                    {
                        GoGroup parentGroup = (obj.Parent as GoGroup);
                        if (parentGroup != null)
                            (obj.Parent as GoGroup).Add(iNode);
                        else
                            MyView.Document.Add(iNode);
                    }
                    else
                    {
                        MyView.Document.Add(iNode);
                    }
                    //relink
                    foreach (GoLabeledLink link in (node as GoNode).Links)
                    {
                        if (link.ToNode == (node as GoNode))
                        {
                            if (link.ToPort is QuickPort)
                            {
                                link.ToPort = iNode.GetPort((link.ToPort as QuickPort).PortPosition.ToString());
                            }
                            else
                                link.ToPort = iNode.Port as IGoPort;
                        }
                        else if (link.FromNode == (node as GoNode))
                        {
                            if (link.FromPort is QuickPort)
                            {
                                link.FromPort = iNode.GetPort((link.FromPort as QuickPort).PortPosition.ToString());
                            }
                            else
                                link.FromPort = iNode.Port as IGoPort;
                        }
                    }

                    UpdateAnchors(obj, iNode);
                }
                else
                {
                    GraphNode newNode = (Core.Variables.Instance.ReturnShape(node.MetaObject.Class) as GraphNode).Copy() as GraphNode;
                    foreach (GoObject o in newNode)
                    {
                        if (o is AllocationHandle)
                        {
                            (o as AllocationHandle).Items = items;
                            break;
                        }
                    }
                    newNode.MetaObject = node.MetaObject;
                    newNode.HookupEvents();
                    newNode.Position = (node as GoNode).Position;
                    //newNode.BindingInfo = node.BindingInfo.Copy();
                    newNode.BindToMetaObjectProperties();

                    if (obj.Parent != obj)
                    {
                        GoGroup parentGroup = (obj.Parent as GoGroup);
                        if (parentGroup != null)
                            (obj.Parent as GoGroup).Add(newNode);
                        else
                            MyView.Document.Add(newNode);
                    }
                    else
                    {
                        MyView.Document.Add(newNode);
                    }
                    //relink
                    foreach (GoLabeledLink link in (node as GoNode).Links)
                    {
                        if (link.ToNode == (node as GoNode))
                        {
                            if (link.ToPort is QuickPort)
                            {
                                link.ToPort = newNode.GetPort((link.ToPort as QuickPort).PortPosition.ToString());
                            }
                            else
                                link.ToPort = newNode.GetDefaultPort as IGoPort;
                        }
                        else if (link.FromNode == (node as GoNode))
                        {
                            if (link.FromPort is QuickPort)
                            {
                                link.FromPort = newNode.GetPort((link.FromPort as QuickPort).PortPosition.ToString());
                            }
                            else
                                link.FromPort = newNode.GetDefaultPort as IGoPort;
                        }
                    }

                    UpdateAnchors(obj, newNode);
                }

                removeNodes.Add(node as GoObject);
            }

            foreach (GoObject n in removeNodes)
            {
                MyView.Document.Remove(n);
            }

            MyView.FinishTransaction("ConvertSelectionNormalImageSymbol");
        }

        private void UpdateAnchors(GoObject OriginalObject, GoObject NewObject)
        {
            foreach (GoObject obj in MyView.Document)
                if (obj is GoBalloon)
                    if ((obj as GoBalloon).Anchor == OriginalObject)
                        (obj as GoBalloon).Anchor = NewObject;
        }

        private string getassociationkey(QLink link)
        {
            IMetaNode tonode = link.ToNode as IMetaNode;
            IMetaNode fromnode = link.FromNode as IMetaNode;
            if (tonode.MetaObject != null && fromnode.MetaObject != null)
                if (tonode.MetaObject.pkid > 0 && fromnode.MetaObject.pkid > 0)
                    return ((int)link.AssociationType).ToString() + tonode.MetaObject.Class + tonode.MetaObject.pkid + tonode.MetaObject.MachineName + fromnode.MetaObject.Class + fromnode.MetaObject.pkid + fromnode.MetaObject.MachineName;

            return "";
        }

        //15 Jan 2014 : Replaces all modifed = false
        //public void SetDocumentModifiedToFalseIfNotTrue(bool overrideModified)
        //{
        //    //if (!overrideModified && MyView.Document.IsModified)
        //    //    return;

        //    //MyView.Document.IsModified = false;
        //}
    }
}