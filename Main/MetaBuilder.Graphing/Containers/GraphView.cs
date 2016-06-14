#region Copyright © 2007 - DISCON Specialists

//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Modification and/or tampering with the code is also strictly prohibited, and
// punishable by law.
//
// Filename: GraphView.cs
// Author: Deon Fourie
// Last Modified: 2007-15-26
//

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Graphing.Controllers;
using MetaBuilder.Graphing.Helpers;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.General;
using MetaBuilder.Graphing.Shapes.Nodes;
using MetaBuilder.Graphing.Shapes.Nodes.Containers;
using MetaBuilder.Graphing.Shapes.Primitives;
using MetaBuilder.Graphing.Tools;
using MetaBuilder.Graphing.Utilities;
using MetaBuilder.Meta;
using MetaBuilder.Meta.Editors;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Northwoods.Go;
using Northwoods.Go.Draw;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
namespace MetaBuilder.Graphing.Containers
{
    [Serializable]
    public class GraphView : GoDrawView
    {
        private Collection<string> suggestionList;
        public Collection<string> SuggestionList
        {
            get
            {
                if (suggestionList == null)
                    suggestionList = new Collection<string>();
                return suggestionList;
            }
            set { suggestionList = value; }
        }

        #region Properties
        //ADDED TO ENABLE SWITCH FOR LESS REDUNDANCY AND MORE ORDER
        [NonSerialized]
        private GraphViewController viewController;
        public GraphViewController ViewController
        {
            get
            {
                return viewController;
            }
            set
            {
                viewController = value;
            }
        }

        private bool busySelecting;
        public bool BusySelecting
        {
            get { return busySelecting; }
            set { busySelecting = value; }
        }

        private bool _showFrame;
        public bool ShowFrame
        {
            get { return _showFrame; }
            set { _showFrame = value; }
        }

        public BaseDocument Doc
        {
            get { return Document as BaseDocument; }
        }

        public Collection<QLink> SelectedLinks
        {
            get
            {
                Collection<QLink> selectedLinks = new Collection<QLink>();
                foreach (GoObject objSelected in Selection)
                {
                    QLink link = objSelected.DraggingObject as QLink;
                    if (link != null)
                        selectedLinks.Add(link);
                }
                return selectedLinks;
            }
        }

        public GoContextMenu ViewContextMenu
        {
            get { return _viewContextMenu; }
            set { _viewContextMenu = value; }
        }

        public Collection<IMetaNode> SelectedNodes
        {
            get
            {
                Collection<IMetaNode> graphNodeList = new Collection<IMetaNode>();
                foreach (GoObject objSelected in Selection)
                {
                    IMetaNode node = objSelected as IMetaNode;
                    if (node != null)
                        graphNodeList.Add(node);
                }
                return graphNodeList;
            }
        }
        #endregion

        #region Fields
        [NonSerialized]
        private LinksSelectedHandler linksSelected;
        public LinksSelectedHandler LinksSelected
        {
            get { return linksSelected; }
        }
        [NonSerialized]
        private GraphNodesContextClickedHandler nodesContextClicked;
        public GraphNodesContextClickedHandler NodesContextClicked
        {
            get { return nodesContextClicked; }
        }
        [NonSerialized]
        private GraphNodesSelectedHandler nodesSelected;
        public GraphNodesSelectedHandler NodesSelected
        {
            get { return nodesSelected; }
        }
        [NonSerialized]
        private SelectionContextClickedHandler selectionContextClicked;
        public SelectionContextClickedHandler SelectionContextClicked
        {
            get { return selectionContextClicked; }
        }
        [NonSerialized]
        private GoContextMenu _viewContextMenu;
        #endregion

        #region Construction

        public GraphView()
        {
            InitializeComponent();
            Sheet.BottomRightMargin = Sheet.TopLeftMargin = new SizeF(60, 60);
            ContextClickSingleSelection = false;
            ShowsNegativeCoordinates = false;
        }
        private void InitializeComponent()
        {
            SuspendLayout();
            // Initial view settings
            _viewContextMenu = new GoContextMenu(this);
            PortGravity = 25;
            DisableKeys = GoViewDisableKeys.ArrowScroll;
            SecondarySelectionColor = Color.Blue;
            PrimarySelectionColor = Color.Red;
            VerticalRuler.Units = GoDrawUnit.Millimeter;
            HorizontalRuler.Units = GoDrawUnit.Millimeter;
            DocScale = 1;
            ResumeLayout(false);
        }

        #endregion

        #region Overrides

        public override bool CanEditPaste()
        {
            return true;
            //return base.CanEditPaste();
        }

        delegate void OnPaintDelegate(PaintEventArgs evt);
        protected override void OnPaint(PaintEventArgs evt)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new OnPaintDelegate(OnPaint), new object[] { evt });
            }
            else
            {
                try
                {
                    base.OnPaint(evt);
                }
                catch (Exception ex)
                {
                    Core.Log.WriteLog("GraphView::OnPaint::" + ex.ToString());
                }
            }
        }

        private delegate void VoidDelegate();
        public override void UpdateScrollBars()
        {
            //try
            //{
            if (InvokeRequired)
            {
                BeginInvoke(new VoidDelegate(UpdateScrollBars), new object[] { });
            }
            else
            {
                base.UpdateScrollBars();
            }
            //}
            //catch
            //{
            //}
        }

        public override void EditCopy()
        {
            base.EditCopy();
        }
        public override void EditPaste()
        {
            // LEAVE THIS CODE HERE: IT TESTS FOR SERIALIZATION - IF OBJECTS CANT BE COPIED AND PASTED, THIS IS WHERE TO FIND THE ISSUE
            /*System.IO.Stream ofile = System.IO.File.Open("test.graph", System.IO.FileMode.Create);
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            bf.Serialize(ofile, this.Selection);
            ofile.Close();
            //return;
            try
            {
                ofile = System.IO.File.Open("testxml.graph", System.IO.FileMode.Create);
                IFormatter oformatter = new SoapFormatter();
                oformatter.Serialize(ofile, this.Selection);
            }
            catch (Exception xxx)
            {
                MetaBuilder.Core.ErrLogger.Log(xxx);
                Console.WriteLine(xxx);
            }
            finally
            {
                ofile.Close();
            }*/

            bool pasted = false;

            //Document.UndoManager.StartTransaction();

            ShapeDesignController sdc = new ShapeDesignController(this);
            PointF centerOfScreen = sdc.GetCenter();
            if (Clipboard.ContainsData(DataFormats.Rtf) && Clipboard.GetData(DataFormats.Rtf) != null)
            {
                RichText rt = new RichText();
                rt.Width = 200;
                rt.Height = 200;
                rt.Rtf = Clipboard.GetData(DataFormats.Rtf).ToString();
                Document.Add(rt);
                rt.AutoResizes = true;
                Selection.Clear();
                Selection.Add(rt);
                rt.Position = centerOfScreen;
                pasted = true;
            }
            //if (Clipboard.ContainsText())
            //{
            //    GoComment comment = sdc.AddComment();
            //    comment.Text = Clipboard.GetText(TextDataFormat.Text);
            //    Document.Add(comment);
            //}
            if (Clipboard.ContainsImage() && (!pasted) && (!(CanEditPaste())))
            {
                Bitmap img = Clipboard.GetImage() as Bitmap;
                if (img != null)
                {
                    //Bitmap bmp = img as Bitmap;
                    /*GoImage goImg = new GoImage();*/
                    QuickImage qimg = new QuickImage(img, "pastedImage" + new Guid().ToString() + ".bmp");
                    qimg.ImageFormat = ImageFormat.Bmp;
                    qimg.AutoResizes = true;

                    qimg.Position = centerOfScreen;
                    Document.Add(qimg);
                    return;
                }
            }

            try
            {
                //base.Document.BeginUpdateViews();
                base.EditPaste();
            }
            catch (Exception x)
            {
                Core.Log.WriteLog("GraphView.EditPaste" + Environment.NewLine + x.ToString());
                //try
                //{
                //    base.EditPaste();
                //}
                //catch (Exception xx)
                //{
                //    Core.Log.WriteLog(xx.ToString());
                //}
            }

            //base.Document.EndUpdateViews();
            //Document.UndoManager.FinishTransaction("Override_GraphView_EditPaste");
        }

        protected override void OnMouseMove(MouseEventArgs evt)
        {
            base.OnMouseMove(evt);
        }

        public override GoCopyDictionary PasteFromClipboard()
        {
            try
            {
                StartTransaction();
                GoCopyDictionary retval = base.PasteFromClipboard();
                FinishTransaction("Paste");
                return retval;
            }
            catch (Exception pasteException)
            {
                Console.WriteLine(pasteException.ToString());
                return null;
            }
        }

        public override void DeleteSelection(GoSelection sel)
        {
            //try
            //{
            base.DeleteSelection(sel);
            //}
            //catch
            //{

            //}
        }

        private bool overrideDocScaleMath;
        public bool OverrideDocScaleMath
        {
            get { return overrideDocScaleMath; }
            set { overrideDocScaleMath = value; }
        }

        public override float DocScale
        {
            get { return base.DocScale; }
            set
            {
                //float val = float.Parse(value.ToString(System.Globalization.CultureInfo.InvariantCulture));

                if (!overrideDocScaleMath)
                    if (value < 0.01) //limit to 1%
                    {
                        base.DocScale = 0.01f;
                    }
                    else
                    {
                        if (value > 5) //limit to 500%
                        {
                            base.DocScale = 5;
                        }
                        else if (value > 1) //zoom fastest @100%
                        {
                            if (value > base.DocScale)
                            {
                                base.DocScale += 0.10f;
                            }
                            else
                            {
                                base.DocScale -= 0.10f;
                            }
                        }
                        else if (value > 0.5) //zoom faster @50%
                        {
                            if (value > base.DocScale)
                            {
                                base.DocScale += 0.025f;
                            }
                            else
                            {
                                base.DocScale -= 0.025f;
                            }
                        }
                        else //zoom normal at <50%
                        {
                            if (value > base.DocScale)
                            {
                                base.DocScale += 0.02f;
                            }
                            else
                            {
                                base.DocScale -= 0.02f;
                            }
                        }
                    }
                else
                    base.DocScale = value;

                //double val = double.Parse(value.ToString()) * 100;
                //val = Math.Round(val, MidpointRounding.AwayFromZero);
                //double rem = Math.IEEERemainder(val, 5);
                //bool Up = value > base.DocScale;
                //if (val == 5 && !(Up))
                //{
                //    base.DocScale = value;
                //    return;
                //}
                //while (rem != 0)
                //{
                //    if (Up)
                //    {
                //        val++;
                //    }
                //    else
                //    {
                //        val--;
                //    }
                //    rem = Math.IEEERemainder(val, 5);
                //}

                ////UnIdeal Fix
                //if (val < 5)
                //    val = 5;

                //base.DocScale = float.Parse((val / 100d).ToString()); // value;// float.Parse(d.ToString());
            }
        }

        protected override void OnObjectSelectionDropped(GoObjectEventArgs evt)
        {
            if (Doc.ContainsILinkContainers.HasValue)
            {
                if (!Doc.ContainsILinkContainers.Value)
                    return;
            }

            if (Selection.Count == 0) //pasting swimlanes bugs below because of this
            {
                base.OnObjectSelectionDropped(evt);
                return;
            }

            GoCollectionEnumerator colEnum = Selection.GetEnumerator();
            Collection<GoObject> objs = new Collection<GoObject>();
            while (colEnum.MoveNext())
            {
                objs.Add(colEnum.Current);
            }
            GoCollection objectsBeneath = new GoCollection();
            PickObjectsInRectangle(true, false, Selection.Primary.Bounds, GoPickInRectangleStyle.AnyIntersectsBounds, objectsBeneath, 5000);

            Collection<SubgraphNode> alreadyAddedToTheseSubbies = new Collection<SubgraphNode>();
            if (objectsBeneath.Count > 0)
            {
                foreach (GoObject o in objectsBeneath)
                {
                    SubgraphNode sgn = o as SubgraphNode;
                    if (sgn != null)
                    {
                        foreach (GoObject oSgnChild in sgn)
                        {
                            if (oSgnChild is SubgraphNode)
                            {
                                SubgraphNode sgnChild = oSgnChild as SubgraphNode;
                                if (alreadyAddedToTheseSubbies.Contains(sgnChild))
                                    return;
                            }
                        }
                        alreadyAddedToTheseSubbies.Add(sgn);
                        o.OnSelectionDropped(this);
                    }
                }
            }
            base.OnObjectSelectionDropped(evt);
            foreach (GoObject gobj in objs)
            {
                //try
                //{
                if (gobj.Document == null)
                {
                    Document.Add(gobj);
                }
                Selection.Add(gobj);
                //}
                //catch
                //{
                //}
            }
        }
        protected override void OnExternalObjectsDropped(GoInputEventArgs evt)
        {
            if (!(Tool is GoToolDragging))
            {
                // Did not originate in this view
                GoCollectionEnumerator colEnum = Selection.GetEnumerator();
                while (colEnum.MoveNext())
                {
                    SubgraphNode sgNode = colEnum.Current as SubgraphNode;
                    if (sgNode != null)
                    {
                        sgNode.CreateMetaObject(null, null);
                        sgNode.HookupEvents();
                        sgNode.BindToMetaObjectProperties();
                    }
                }
            }
            base.OnExternalObjectsDropped(evt);
            return;

            //if (Selection.Primary != null)
            //{
            //    GoObject obj = Selection.Primary.DraggingObject.DraggingObject.DraggingObject.DraggingObject;
            //    GraphNode droppedNode = null;
            //    if (obj is GraphNode)
            //    {
            //        GoSelectionEventArgs args = new GoSelectionEventArgs(obj);
            //        OnItemAdded(args);

            //        droppedNode = obj as GraphNode;
            //        NodeManipulator.PrepareObjectForDocumentType(droppedNode, Doc.FileType, Doc);
            //        if ((Doc.FileType == FileTypeList.Diagram) || (Doc.FileType == FileTypeList.SymbolStore))
            //            droppedNode.EditMode = false;
            //        else
            //            droppedNode.EditMode = true;
            //        // Check if there are any other shapes beneath this one
            //        GoCollection objectsBeneath = new GoCollection();
            //        PickObjectsInRectangle(true, false, droppedNode.Bounds, GoPickInRectangleStyle.AnyIntersectsBounds, objectsBeneath, 10);
            //        if (objectsBeneath.Count > 0)
            //        {
            //            #region Dropped on another node - refactor!

            //            /* if ((objectsBeneath.First.DraggingObject.DraggingObject is GraphNode) && (objectsBeneath.First.DraggingObject.DraggingObject != droppedNode))
            //            {

            //                 #region Dropped on a graphnode
            //                GraphNode from = objectsBeneath.First.DraggingObject.DraggingObject as GraphNode;

            //                if (from.BindingInfo.BindingClass != null && droppedNode.BindingInfo.BindingClass != null)
            //                {
            //                    AllowedAssociationInfo allowedAssociation = BusinessFacade.MetaHelper.Singletons.GetAssociationHelper().GetDefaultAllowedAssociationInfo(from.BindingInfo.BindingClass, droppedNode.BindingInfo.BindingClass);
            //                    if (allowedAssociation != null)
            //                    {
            //                        if (from.GetDefaultPort != null && droppedNode.GetDefaultPort != null)
            //                        {
            //                            QLink l = new QLink();
            //                            l.FromPort = from.GetDefaultPort;
            //                            l.ToPort = droppedNode.GetDefaultPort;
            //                            GoPort prt = l.ToPort as GoPort;
            //                            if (prt.Style == GoPortStyle.Object)
            //                            {
            //                                prt.ToSpot = Northwoods.Go.GoObject.MiddleCenter;
            //                            }

            //                            prt = l.FromPort as GoPort;
            //                            if (prt.Style == GoPortStyle.Object)
            //                            {
            //                                prt.FromSpot = Northwoods.Go.GoObject.MiddleCenter;
            //                            }
            //                            droppedNode.Position = new PointF(from.Position.X + from.Width + 30, from.Position.Y + from.Height + 30);
            //                            l.AssociationType = allowedAssociation.LinkAssociationType;
            //                            //DockingForm.Output.AddMessage(this, MessageType.Info, "Some objects can be auto-linked by dropping them onto others");
            //                            Doc.LinksLayer.Add(l);
            //                            l.CalculateStroke();

            //                            l.ChangeStyle();
            //                        }
            //                    }
            //                }
            //                #endregion
            //            }
            //            if (objectsBeneath.First.DraggingObject.DraggingObject is QLink)
            //            {
            //                #region Dropped on a QLink

            //                    QLink QLink = objectsBeneath.First.DraggingObject.DraggingObject as QLink;
            //                    GraphNode parentNode = QLink.FromNode as GraphNode;
            //                    GraphNode childNode = QLink.ToNode as GraphNode;

            //                    bool NodesNotNull = ((QLink.ToNode != null) && (QLink.FromNode != null)) && (QLink.ToNode is GraphNode) && (QLink.FromNode is GraphNode);
            //                    bool NodesHaveClasses = ((parentNode.BindingInfo.BindingClass != null) && (childNode.BindingInfo.BindingClass != null));
            //                    string droppedClass = null;
            //                    if (droppedNode.BindingInfo != null)
            //                    {
            //                        droppedClass = droppedNode.BindingInfo.BindingClass;
            //                    }
            //                    if (NodesNotNull && NodesHaveClasses && droppedClass != null)
            //                    {

            //                        int CAid = BusinessFacade.MetaHelper.Singletons.GetAssociationHelper().CheckForValidArtifact(parentNode.BindingInfo.BindingClass, childNode.BindingInfo.BindingClass, droppedClass, (int)QLink.AssociationType);
            //                        if (CAid > -1)
            //                        {
            //                            droppedNode.Remove();
            //                            QLink.Add(droppedNode);
            //                            droppedNode.DragsNode = false;
            //                        }
            //                        else
            //                        {
            //                            OnDispatch(this, new DispatchMessageEventArgs(MessageType.Warning, "No definition exists for [" + droppedClass + "] as an artifact on [" + parentNode.BindingInfo.BindingClass + "] and [" + childNode.BindingInfo.BindingClass + "]"));
            //                        }
            //                    }
            //                    droppedNode.Movable = true;

            //                #endregion
            //            }*/

            //            #endregion
            //        }
            //    }
            //    if (Selection.Primary != null)
            //    {
            //        DocumentController dcontroller = new DocumentController(Document);
            //        dcontroller.ApplyBrushesForCollection(Selection);
            //        Selection.Primary.Movable = true;
            //        RaiseObjectGotSelection(Selection.Primary);
            //    }
            //}
        }
        protected override void OnObjectDoubleClicked(GoObjectEventArgs evt)
        {
            if (evt.GoObject.SelectionObject is RichText)
            {
                ViewController.ShowRichTextForm(evt.GoObject.SelectionObject as RichText);
            }
            if (evt.GoObject.Parent is SerializableAttachment)
            {
                SerializableAttachment attachment = evt.GoObject.Parent as SerializableAttachment;
                attachment.Open();
            }
            if (evt.GoObject.Parent != null)
            {
                CollapsibleNode node = (evt.GoObject.ParentNode as CollapsibleNode);
                if (node != null)
                {
                    //datacolumn double click
                    if (node.MetaObject.State != VCStatusList.CheckedIn && node.MetaObject.State != VCStatusList.CheckedOutRead && node.MetaObject.State != VCStatusList.Locked && node.MetaObject.State != VCStatusList.Obsolete)
                    {
                        ExpandableLabelList list = evt.GoObject.Parent.SelectionObject.Parent as ExpandableLabelList;
                        if (list != null)
                        {
                            if (list.HeaderText == evt.GoObject)
                            {
                                list.AddItemFromCode();
                            }
                        }
                    }
                }
            }
            base.OnObjectDoubleClicked(evt);
        }
        protected override void OnBackgroundContextClicked(GoInputEventArgs evt)
        {
            base.OnBackgroundContextClicked(evt);
            if (Selection.Count > 0)
            {
                // set up the background context menu
                GoContextMenu cm = new GoContextMenu(this);
                if (CanInsertObjects())
                    cm.MenuItems.Add(new MenuItem("Paste", PasteCommand));
                if (cm.MenuItems.Count > 0)
                    cm.MenuItems.Add(new MenuItem("-"));
                cm.MenuItems.Add(new MenuItem("Properties", PropertiesCommand));
                cm.Show(this, evt.ViewPoint);
            }
        }
        protected override void OnObjectSelectionDropReject(GoObjectEventArgs evt)
        {
            if (Selection.First is ContextNode)
            {
                evt.InputState = GoInputState.Cancel;
                return;
            }
            base.OnObjectSelectionDropReject(evt);
        }
        /*protected override void OnLinkCreated(GoSelectionEventArgs evt)
         {
             base.OnLinkCreated(evt);
             if (evt.GoObject is QLink)
             {
                 QLink link = evt.GoObject as QLink;
                 if (link.ToPort is FishLinkPort)
                 {
                     link.Remove();
                 }
                 else
                     link.MoveToLayer(Document.LinksLayer);
             }
         }*/
        #endregion

        #region Virtual
        //protected virtual void OnItemAdded(GoSelectionEventArgs e)
        //{
        //    if (ItemAdded != null)
        //        ItemAdded(this, e);
        //}

        protected virtual void OnSelectionChanged(GoSelectionEventArgs e)
        {
            if (SelectionChanged != null)
                SelectionChanged(this, e);
        }
        protected virtual void OnNodesContextClicked(Collection<GraphNode> nodes)
        {
            if (NodesContextClicked != null && nodes.Count > 0)
                NodesContextClicked(nodes);
        }
        protected virtual void OnNodeSelected(Collection<GraphNode> nodes)
        {
            if (NodesSelected != null && nodes.Count > 0)
                NodesSelected(nodes);
        }
        protected virtual void OnLinksSelected(Collection<QLink> links)
        {
            if (LinksSelected != null && links.Count > 0)
                LinksSelected(links);
        }
        protected virtual void OnSelectionContextClicked(GoSelection selection)
        {
            if (SelectionContextClicked != null && selection.Count > 0)
                SelectionContextClicked(selection);
        }

        #endregion

        #region Events

        public event GoSelectionEventHandler SelectionChanged;
        public event NodeObjectContextClickedEventHandler NodeObjectContextClicked;
        public event NodeObjectContextClickedEventHandler NodeObjectContextClickedShallow;

        [field: NonSerialized]
        public event EventHandler OpenDiagramFromAnother;
        [field: NonSerialized]
        public event EventHandler ItemsPasted;
        [field: NonSerialized]
        public event EventHandler ShallowCopy;
        [field: NonSerialized]
        public event DispatchMessageEventHandler Dispatch;
        [field: NonSerialized]
        public event PluginProgressEventHandler PluginProgressReport;

        #endregion

        public void FireNodeObjectContextClicked(object sender, NodeObjectContextArgs e)
        {
            OnNodeObjectContextClicked(sender, e);
        }
        public void FireNodeObjectContextClickedShallow(object sender, NodeObjectContextArgs e)
        {
            OnNodeObjectContextClickedShallow(sender, e);
        }
        public void OnOpenDiagramFromAnother(object sender, EventArgs e)
        {
            if (OpenDiagramFromAnother != null)
                OpenDiagramFromAnother(sender, e);
        }
        public void PasteCommand(object sender, EventArgs e)
        {
            Selection.Clear();
            EditPaste();
        }
        public void PropertiesCommand(object sender, EventArgs e)
        {
            //DockingForm.DockForm.ShowProperties(this, this);
        }
        public void OnShallowCopy(object sender, EventArgs e)
        {
            if (ShallowCopy != null)
                ShallowCopy(sender, e);
        }
        //public void DrawStroke(object sender, EventArgs e)
        //{
        //    Tool = new StrokeDrawingTool(this, GoStrokeStyle.Line);
        //}
        //public void DrawBezier(object sender, EventArgs e)
        //{
        //    Tool = new StrokeDrawingTool(this, GoStrokeStyle.Bezier);
        //    //CreateLegend();
        //}
        //public void DrawRoundedLine(object sender, EventArgs e)
        //{
        //    Tool = new StrokeDrawingTool(this, GoStrokeStyle.RoundedLine);
        //}
        //public void DrawRoundedLineWithJumpovers(object sender, EventArgs e)
        //{
        //    Tool = new StrokeDrawingTool(this, GoStrokeStyle.RoundedLineWithJumpOvers);
        //}
        //public void DrawPolygon(object sender, EventArgs e)
        //{

        //}
        //public void DrawPolygonLine(object sender, EventArgs e)
        //{
        //    PolygonDrawingTool ptool = new PolygonDrawingTool(this);
        //    ptool.Style = GoPolygonStyle.Line;
        //    Tool = ptool;
        //}
        public void RaiseProxiedDispatchMessage(object sender, DispatchMessageEventArgs e)
        {
            OnDispatch(sender, e);
        }

        protected void OnItemsPasted(object sender, EventArgs e)
        {
            if (ItemsPasted != null)
                ItemsPasted(sender, e);
        }
        protected virtual void OnPluginProgressReport(object sender, PluginProgressEventArgs e)
        {
            if (PluginProgressReport != null)
                PluginProgressReport(sender, e);
        }
        protected void OnNodeObjectContextClicked(object sender, NodeObjectContextArgs e)
        {
            if (NodeObjectContextClicked != null)
            {
                NodeObjectContextClicked(sender, e);
            }
        }
        protected void OnNodeObjectContextClickedShallow(object sender, NodeObjectContextArgs e)
        {
            if (NodeObjectContextClickedShallow != null)
            {
                NodeObjectContextClickedShallow(sender, e);
            }
        }
        protected void OnDispatch(object sender, DispatchMessageEventArgs e)
        {
            if (Dispatch != null)
                Dispatch(sender, e);
        }

        [field: NonSerialized]
        public event EventHandler EditAllocation;
        public void OnEditAllocation(object sender, EventArgs e)
        {
            if (EditAllocation != null)
                EditAllocation(sender, e);
        }

        protected override void Dispose(bool disposing)
        {
            ViewController = null;
            base.Dispose(disposing);
        }

        //public override GoSelection Selection
        //{
        //    get
        //    {
        //        return base.Selection;
        //    }
        //}
    }
}