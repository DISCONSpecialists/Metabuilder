using System;
using System.Collections.Generic;
using System.Drawing;
using MetaBuilder.Graphing.Formatting;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Behaviours;
using MetaBuilder.Graphing.Shapes.Behaviours.Internal;
using MetaBuilder.Graphing.Shapes.Nodes.Containers;
using MetaBuilder.Graphing.Shapes.Primitives;
using Northwoods.Go;

namespace MetaBuilder.UIControls.GraphingUI.Formatting.FormatUndo
{
    public class FormatEditCommand : GoUndoManagerCompoundEdit
    {

        #region Fields (3)

        private bool hasShapes;
        private bool hasText;
        private FormattingContainer primaryFormatting;

        #endregion Fields

        #region Properties (3)

        public bool HasShapes
        {
            get { return hasShapes; }
            set { hasShapes = value; }
        }

        public bool HasText
        {
            get { return hasText; }
            set { hasText = value; }
        }

        public FormattingContainer PrimaryFormatting
        {
            get { return primaryFormatting; }
            set { primaryFormatting = value; }
        }

        #endregion Properties

        #region Methods (1)

        // Private Methods (1) 

        private bool IsShape(GoObject o)
        {
            return (o is GoShape && (!(o is GraphNodeGrid || o is QuickStroke)));
        }

        #endregion Methods

        #region Properties & Accessors

        public Dictionary<GoObject, FormattingContainer> objectHistory;
        public Dictionary<GoObject, FormattingContainer> originalSettings
        {
            get { return _originalSettings; }
            set { _originalSettings = value; }
        }
        private Dictionary<GoObject, FormattingContainer> _originalSettings;
        private FormattingContainer newSettings;
        private FormattingContainer redoSettings;
        private List<GoObject> collection;
        public List<GoObject> Collection
        {
            get { return collection; }
            set { collection = value; }
        }

        #endregion

        #region Constructor(s)

        public FormatEditCommand()
        {
            Collection = new List<GoObject>();
            objectHistory = new Dictionary<GoObject, FormattingContainer>();
            newSettings = new FormattingContainer();
            redoSettings = new FormattingContainer();
            collection = new List<GoObject>();
            originalSettings = new Dictionary<GoObject, FormattingContainer>();
        }

        #endregion

        #region Implementation of IUndoable interface
        public override void Clear()
        {
            objectHistory = null;
            collection = null;
            newSettings = null;
            redoSettings = null;
        }

        public override bool CanUndo()
        {
            return objectHistory != null;
        }

        public override bool CanRedo()
        {
            return redoSettings != null;
        }

        public override void Undo()
        {
            redoSettings = newSettings;
            objectHistory = originalSettings;
            foreach (KeyValuePair<GoObject, FormattingContainer> kvpair in objectHistory)
            {
                ApplySettings(kvpair.Key, kvpair.Value);
            }
        }

        public override void Redo()
        {
            foreach (KeyValuePair<GoObject, FormattingContainer> kvpair in objectHistory)
            {
                ApplySettings(kvpair.Key, redoSettings);
            }
            redoSettings = null;
        }
        #endregion

        #region Store Settings
        public void StoreSettings(Dictionary<GoObject, FormattingContainer> target)
        {
            PrimaryFormatting = new FormattingContainer();
            foreach (GoObject o in collection)
            {
                StoreObjectFormatting(o, target);
                if (IsShape(o))
                {
                    StoreShapeFormatting(o as GoShape, target);
                }
                if (o is GoText)
                {
                    bool shouldStoreSettings = false;
                    GoText txt = o as GoText;
                    if ((txt.Editable && txt.EditorStyle == GoTextEditorStyle.TextBox) || (txt.ParentNode is ArtefactNode))
                        shouldStoreSettings = true;

                    if (txt is BoundLabel)
                    {
                        shouldStoreSettings = false;

                        BoundLabel lbl = txt as BoundLabel;
                        if (lbl.EditorStyle == GoTextEditorStyle.TextBox)// && lbl.Editable)
                            if (lbl.originalEditable.HasValue)
                                if (lbl.originalEditable.Value)
                                    shouldStoreSettings = true;

                        //if (!shouldStoreSettings)
                        //    if (lbl.ParentNode is GraphNode)
                        //    {
                        //        if ((lbl.ParentNode as GraphNode).MetaObject.Class == "GovernanceMechanism" && lbl.Name.Contains("Bound"))
                        //        {
                        //            shouldStoreSettings = true;
                        //            HasText = true;
                        //        }
                        //    }
                    }

                    if (shouldStoreSettings)
                        StoreTextFormatting(o as GoText, target);
                }
            }
            // store primary formatting: only if one shape is selected
            int shapecount = 0;
            int textcount = 0;
            int objectcount = 0;
            ShapeFormatting shapeForm = null;
            TextFormatting textForm = null;
            BasicFormatting baseForm = null;
            foreach (GoObject o in collection)
            {
                if (IsShape(o))
                {
                    shapecount++;
                    shapeForm = target[o].ShapeSettings;
                    HasShapes = true;

                }
                if (o is GoText)
                {
                    //if (o is BoundLabel)
                    //    if ((o as BoundLabel).EditorStyle != GoTextEditorStyle.TextBox || !(o as BoundLabel).Editable)
                    //        if (o.ParentNode is GraphNode)
                    //            if ((o.ParentNode as GraphNode).MetaObject.Class == "GovernanceMechanism" && (o as BoundLabel).Name.Contains("Bound"))
                    //            {
                    //                continue;
                    //            }
                    //else
                    //{
                    //    continue;
                    //}

                    GoText txt = o as GoText;
                    if (txt.Text.Length > 0)
                    {
                        if (target[o].TextSettings.Font != null)
                        {
                            textcount++;
                            textForm = target[o].TextSettings;
                        }
                    }
                    HasText = true;
                }
                if (o is GoObject)
                {
                    objectcount++;
                    baseForm = target[o].GeneralSettings;
                }
            }
            if (shapecount > 0)
            {
                PrimaryFormatting.ShapeSettings = shapeForm;
            }
            if (textcount > 0)
            {
                PrimaryFormatting.TextSettings = textForm;
            }
            if (objectcount == 1)
            {
                PrimaryFormatting.GeneralSettings = baseForm;
            }
        }

        private void StoreShapeFormatting(GoShape shape, Dictionary<GoObject, FormattingContainer> target)
        {
            target[shape].ShapeSettings = new ShapeFormatting();
            ShapeFormatting shapeFormat = target[shape].ShapeSettings;
            if (shape is IBehaviourShape)
            {
                if (shape.Brush != null)
                {
                    if (shape.Brush is SolidBrush)
                    {
                        SolidBrush sbrush = shape.Brush as SolidBrush;
                        shapeFormat.IsGradient = false;
                        shapeFormat.FillColour = sbrush.Color;
                    }
                }
                GradientBehaviour gbehaviour =
                    (shape as IBehaviourShape).Manager.GetExistingBehaviour(typeof(GradientBehaviour)) as
                    GradientBehaviour;
                if (gbehaviour != null)
                {
                    shapeFormat.IsGradient = true;
                    shapeFormat.GradientType = gbehaviour.MyBrush.GradientType;
                    shapeFormat.GradientEndColour = gbehaviour.MyBrush.OuterColor;
                    shapeFormat.GradientStartColour = gbehaviour.MyBrush.InnerColor;
                }
                if (shape.Pen != null)
                {
                    shapeFormat.PenColour = shape.Pen.Color;
                    shapeFormat.PenEndCap = shape.Pen.EndCap;
                    shapeFormat.PenStartCap = shape.Pen.StartCap;
                    shapeFormat.DashStyle = shape.Pen.DashStyle;
                    shapeFormat.PenWidth = shape.Pen.Width;
                }
                if (shape is GoRoundedRectangle)
                {
                    GoRoundedRectangle rectangle = shape as GoRoundedRectangle;
                    shapeFormat.Corner = rectangle.Corner;
                }
            }
            if (target.ContainsKey(shape))
            {
                target[shape].ShapeSettings = shapeFormat;
            }

        }

        private void StoreTextFormatting(GoText text, Dictionary<GoObject, FormattingContainer> target)
        {
            TextFormatting txtFormat = target[text].TextSettings;
            txtFormat.AutoResizes = text.AutoResizes;
            txtFormat.Font = text.Font;
            txtFormat.Bold = text.Font.Bold;
            txtFormat.Alignment = text.Alignment;
            txtFormat.Bold = text.Bold;
            txtFormat.Bordered = text.Bordered;
            txtFormat.Font = text.Font;
            txtFormat.FontSize = text.FontSize;
            txtFormat.Italic = text.Italic;
            txtFormat.Multiline = text.Multiline;
            txtFormat.StrikeThrough = text.StrikeThrough;
            txtFormat.TextColour = text.TextColor;
            txtFormat.Underline = text.Underline;
            txtFormat.Wrap = text.Wrapping;
            txtFormat.Editable = text.Editable;
            txtFormat.WrappingWidth = text.WrappingWidth;

            if (target.ContainsKey(text))
            {
                target[text].TextSettings = txtFormat;
            }
        }

        private void StoreObjectFormatting(GoObject o, Dictionary<GoObject, FormattingContainer> target)
        {
            if (!target.ContainsKey(o))
            {
                target.Add(o, new FormattingContainer());
                BasicFormatting bformatting = target[o].GeneralSettings;
                bformatting.Deletable = o.Deletable;
                bformatting.AutoRescales = o.AutoRescales;
                bformatting.Movable = o.Movable;
                bformatting.Printable = o.Printable;
                bformatting.Reshapable = o.Reshapable;
                bformatting.Resizable = o.Resizable;
                bformatting.ResizesRealtime = o.ResizesRealtime;
                bformatting.Selectable = o.Selectable;
                bformatting.Shadowed = o.Shadowed;
                bformatting.Visible = o.Visible;

                if (target.ContainsKey(o))
                {
                    target[o].GeneralSettings = bformatting;
                }
            }
        }
        #endregion

        #region Apply Settings

        public void ApplySettings(FormattingContainer settings)
        {
            newSettings = settings;

            foreach (GoObject o in Collection)
            {
                GoObject myObject = o;
                ApplySettings(myObject, settings);
            }

            //if (Collection[0] is MetaBuilder.Graphing.Shapes.Nodes.Containers.ILinkedContainer)
            //{
            //    ApplySettings(Collection[0], settings);
            //    ApplySettings((Collection[0] as GoNode).Label,settings);
            //}
            //else
            //    foreach (GoObject o in Collection)
            //    {
            //        ApplySettings(o, settings);
            //    }
        }
        public void ApplySettings(GoObject o, FormattingContainer settingsToApply)
        {
            FormatGeneral(o, settingsToApply.GeneralSettings);
            if (o is GoShape)
            {
                FormatShape(o as GoShape, settingsToApply.ShapeSettings);
            }
            if (o is GoSubGraph)
            {
                FormatShape(o as GoSubGraph, settingsToApply.ShapeSettings);
            }
            if (o is MetaBuilder.Graphing.Shapes.Nodes.MindMapNode)
            {
                FormatShape(o as MetaBuilder.Graphing.Shapes.Nodes.MindMapNode, settingsToApply.ShapeSettings);
            }
            if (o is GoText)
            {
                GoText txt = o as GoText;
                bool overrideEditable = false;
                if (txt is BoundLabel)
                {
                    BoundLabel lbl = txt as BoundLabel;
                    IMetaNode node = lbl.Parent as IMetaNode;
                    if (node != null)
                        if (node.BindingInfo.Bindings.ContainsKey(lbl.Name))
                            overrideEditable = true;
                }
                if (((txt.Editable || overrideEditable) && txt.EditorStyle == GoTextEditorStyle.TextBox) || (txt.ParentNode is MappingCell) || (txt.ParentNode is ArtefactNode) || txt.Parent is SubgraphNode)
                {
                    FormatText(o as GoText, settingsToApply.TextSettings);
                }
                else if (o.ParentNode is MetaBuilder.Graphing.Shapes.Nodes.MindMapNode)
                {
                    FormatText(o as GoText, settingsToApply.TextSettings);
                }
            }
        }

        private void RemoveGradient(IBehaviourShape shp)
        {
            if (shp != null)
            {
                shp.Manager.RemoveBehaviourOfType(typeof(GradientBehaviour));
            }
        }

        private void FormatShape(GoSubGraph sg, ShapeFormatting shapeFormatting)
        {
            //Gradient background
            foreach (GoObject o in sg.GetEnumerator())
            {

            }

            // Setup brush/gradient
            //if (shapeFormatting.IsGradient.HasValue)
            //{
            //    //RemoveGradient(ibshape);
            //    if (shapeFormatting.IsGradient.Value)
            //    {
            //        sg.
            //        ApplyFill(shp, GradientType.ForwardDiagonal, shapeFormatting.GradientStartColour, shapeFormatting.GradientEndColour, ibshape);
            //    }
            //}
            if (shapeFormatting.FillColour.HasValue)
            {
                sg.BackgroundColor = (Color)shapeFormatting.FillColour;
                //ApplyFill(shp, GradientType.ForwardDiagonal, shapeFormatting.FillColour, shapeFormatting.FillColour, ibshape);
            }
            //if (shapeFormatting.Corner.HasValue)
            //{
            //    GoRoundedRectangle rect = shp as GoRoundedRectangle;
            //    if (rect != null)
            //        rect.Corner = shapeFormatting.Corner.Value;
            //}

            //if (shapeFormatting.PenWidth.HasValue)
            //    shp.Pen = new Pen(shp.Pen.Color, shapeFormatting.PenWidth.Value);
            //if (shapeFormatting.DashStyle.HasValue)
            //    shp.Pen.DashStyle = shapeFormatting.DashStyle.Value;
            //if (shapeFormatting.PenEndCap.HasValue)
            //    shp.Pen.EndCap = shapeFormatting.PenEndCap.Value;
            //if (shapeFormatting.PenStartCap.HasValue)
            //    shp.Pen.StartCap = shapeFormatting.PenStartCap.Value;
            //if (shapeFormatting.PenWidth.HasValue)
            //    shp.Pen.Width = shapeFormatting.PenWidth.Value;
        }
        private void FormatShape(MetaBuilder.Graphing.Shapes.Nodes.MindMapNode n, ShapeFormatting shapeFormatting)
        {
            // Setup brush/gradient
            if (shapeFormatting.IsGradient.HasValue)
            {
                //RemoveGradient(ibshape);
                if (shapeFormatting.IsGradient.Value)
                {
                    n.SetShapeColor((Color)shapeFormatting.GradientStartColour, (Color)shapeFormatting.GradientEndColour);
                }
            }
            if (shapeFormatting.FillColour.HasValue)
            {
                n.Brush = new SolidBrush((Color)shapeFormatting.FillColour);
                //ApplyFill(shp, GradientType.ForwardDiagonal, shapeFormatting.FillColour, shapeFormatting.FillColour, ibshape);
            }
            //if (shapeFormatting.Corner.HasValue)
            //{
            //    GoRoundedRectangle rect = shp as GoRoundedRectangle;
            //    if (rect != null)
            //        rect.Corner = shapeFormatting.Corner.Value;
            //}

            //if (shapeFormatting.PenWidth.HasValue)
            //    shp.Pen = new Pen(shp.Pen.Color, shapeFormatting.PenWidth.Value);
            //if (shapeFormatting.DashStyle.HasValue)
            //    shp.Pen.DashStyle = shapeFormatting.DashStyle.Value;
            //if (shapeFormatting.PenEndCap.HasValue)
            //    shp.Pen.EndCap = shapeFormatting.PenEndCap.Value;
            //if (shapeFormatting.PenStartCap.HasValue)
            //    shp.Pen.StartCap = shapeFormatting.PenStartCap.Value;
            //if (shapeFormatting.PenWidth.HasValue)
            //    shp.Pen.Width = shapeFormatting.PenWidth.Value;
        }
        private void FormatShape(GoShape shp, ShapeFormatting shapeFormatting)
        {
            if (shp is GraphNodeGrid)
                return;
            IBehaviourShape ibshape = shp as IBehaviourShape;
            if (ibshape != null)
            {
                // Setup brush/gradient
                if (shapeFormatting.IsGradient.HasValue)
                {
                    //RemoveGradient(ibshape);
                    if (shapeFormatting.IsGradient.Value)
                    {
                        ApplyFill(shp, shapeFormatting.GradientType, shapeFormatting.GradientStartColour, shapeFormatting.GradientEndColour, ibshape);
                    }
                }
                if (shapeFormatting.FillColour.HasValue)
                {
                    ApplyFill(shp, shapeFormatting.GradientType, shapeFormatting.FillColour, shapeFormatting.FillColour, ibshape);
                }
            }
            if (shapeFormatting.Corner.HasValue)
            {
                GoRoundedRectangle rect = shp as GoRoundedRectangle;
                if (rect != null)
                    rect.Corner = shapeFormatting.Corner.Value;
            }

            //8 January 2013
            //Cannot make changes because permissions are invalid
            //4 February 2013 instance of pen created fixes bug
            try
            {
                if (shp.Pen != null) //pen is null on some shapes
                {
                    Pen p = new Pen(shp.Pen.Color);
                    if (shapeFormatting.PenWidth.HasValue)
                        p.Width = shapeFormatting.PenWidth.Value;
                    //shp.Pen = new Pen(shp.Pen.Color, shapeFormatting.PenWidth.Value);
                    if (shapeFormatting.DashStyle.HasValue)
                        p.DashStyle = shapeFormatting.DashStyle.Value;
                    if (shapeFormatting.PenEndCap.HasValue)
                        p.EndCap = shapeFormatting.PenEndCap.Value;
                    if (shapeFormatting.PenStartCap.HasValue)
                        p.StartCap = shapeFormatting.PenStartCap.Value;
                    if (shapeFormatting.PenWidth.HasValue)
                        p.Width = shapeFormatting.PenWidth.Value;

                    shp.Pen = p;
                }
            }
            catch (Exception ex)
            {
            }

            if (shp.Parent != null)
            {
                if (shp.Parent is Graphing.Shapes.Nodes.LegendItem)
                {
                    Graphing.Shapes.Nodes.LegendItem legItem = shp.Parent as Graphing.Shapes.Nodes.LegendItem;
                    Graphing.Shapes.Nodes.LegendNode legNode = legItem.ParentNode as Graphing.Shapes.Nodes.LegendNode;

                    foreach (GraphNode o in MetaBuilder.Graphing.Controllers.GraphViewController.GetAllKeyShapes(legItem.Key, legNode.MyLegendType))
                    {
                        foreach (GoObject obj in o)
                        {
                            if (obj is GoShape && !(obj is GoPort)) //only apply to the shape
                            {
                                ApplySettings(obj, newSettings);
                            }
                        }
                    }

                    if (legNode.MyLegendType != MetaBuilder.Graphing.Shapes.Nodes.LegendType.Class)
                    {
                        //which means its one of the other 2
                        legItem.ColorKey = MetaBuilder.Graphing.Controllers.GraphViewController.getColor(shp.Parent as Graphing.Shapes.Nodes.LegendItem);
                    }
                    else
                    {
                        //this is compeletelyy redundant
                        legItem.ColorKey = "";
                    }
                }
                //some shapes have gogroups as their shape
                if (shp.Parent.Parent is Graphing.Shapes.Nodes.LegendItem)
                {
                    Graphing.Shapes.Nodes.LegendItem legItem = shp.Parent.Parent as Graphing.Shapes.Nodes.LegendItem;
                    Graphing.Shapes.Nodes.LegendNode legNode = legItem.ParentNode as Graphing.Shapes.Nodes.LegendNode;

                    foreach (GraphNode o in MetaBuilder.Graphing.Controllers.GraphViewController.GetAllKeyShapes(legItem.Key, legNode.MyLegendType))
                    {
                        foreach (GoObject obj in o)
                        {
                            if (obj is GoShape && !(obj is GoPort)) //only apply to the shape
                            {
                                ApplySettings(obj, newSettings);
                            }
                        }
                    }

                    if (legNode.MyLegendType != MetaBuilder.Graphing.Shapes.Nodes.LegendType.Class)
                    {
                        //which means its one of the other 2
                        legItem.ColorKey = MetaBuilder.Graphing.Controllers.GraphViewController.getColor(shp.Parent.Parent as Graphing.Shapes.Nodes.LegendItem);
                    }
                    else
                    {
                        //this is compeletelyy redundant
                        legItem.ColorKey = "";
                    }
                }
            }
        }

        private static void ApplyFill(GoShape shp, GradientType? grad, Color? c1, Color? c2, IBehaviourShape ibshape)
        {
            GradientBehaviour newBehaviour = new GradientBehaviour();

            newBehaviour.MyBrush = new ShapeGradientBrush();
            if (grad != null)
                newBehaviour.MyBrush.GradientType = (GradientType)grad;
            newBehaviour.MyBrush.InnerColor = c1.Value;
            newBehaviour.MyBrush.OuterColor = c2.Value;
            //Black printing shapes when transparent
            if (c1.Value == Color.Transparent)
            {
                //newBehaviour.MyBrush = null;
                newBehaviour.MyBrush.InnerColor = Color.White;
            }
            if (c2.Value == Color.Transparent)
            {
                //newBehaviour.MyBrush = null;
                newBehaviour.MyBrush.OuterColor = Color.White;
            }
            ibshape.Manager = new BaseShapeManager();
            ibshape.Manager.AddBehaviour(newBehaviour);
            newBehaviour.Owner = shp;
            newBehaviour.Update(shp);
        }

        private void FormatText(GoText txt, TextFormatting txtFormatting)
        {
            bool validTextItem = false;
            if (txt.TopLevelObject is GraphNode)
            {
                if ((txt as BoundLabel).Name == "cls_id")
                    return;
                // At least half the width of the node
                if (txt.Width >= txt.TopLevelObject.Width / 2)
                {
                    validTextItem = true;
                }
            }
            else
            {
                validTextItem = true;
            }

            if (validTextItem)
            {
                if (txtFormatting.FontDirty)
                {
                    if (txtFormatting.Font != null)
                    {
                        txt.Font = txtFormatting.Font;
                    }
                    if (txtFormatting.FontSize.HasValue)
                        txt.FontSize = txtFormatting.FontSize.Value;
                }
                if (txtFormatting.AutoResizes.HasValue)
                    txt.AutoResizes = txtFormatting.AutoResizes.Value;
                if (txtFormatting.Bold.HasValue)
                    txt.Bold = txtFormatting.Bold.Value;
                if (txtFormatting.Alignment.HasValue)
                    txt.Alignment = txtFormatting.Alignment.Value;
                if (txtFormatting.Bordered.HasValue)
                    txt.Bordered = txtFormatting.Bordered.Value;

                if (txtFormatting.Italic.HasValue)
                    txt.Italic = txtFormatting.Italic.Value;
                if (txtFormatting.Multiline.HasValue)
                    txt.Multiline = txtFormatting.Multiline.Value;
                if (txtFormatting.StrikeThrough.HasValue)
                    txt.StrikeThrough = txtFormatting.StrikeThrough.Value;

                if (txt is Hyperlink)
                {
                }
                else
                {
                    if (txtFormatting.TextColour.HasValue)
                        txt.TextColor = txtFormatting.TextColour.Value;
                }

                if (txt is Hyperlink)
                {
                }
                else
                {
                    if (txtFormatting.Underline.HasValue)
                        txt.Underline = txtFormatting.Underline.Value;
                }

                if (txtFormatting.Wrap.HasValue)
                    txt.Wrapping = txtFormatting.Wrap.Value;
                if (txtFormatting.WrappingWidth.HasValue)
                    txt.WrappingWidth = txtFormatting.WrappingWidth.Value;
                if (txtFormatting.Editable.HasValue)
                    txt.Editable = txtFormatting.Editable.Value;
            }
        }

        private void FormatGeneral(GoObject o, BasicFormatting basicFormatting)
        {
            if (basicFormatting.Deletable.HasValue)
                o.Deletable = basicFormatting.Deletable.Value;
            if (basicFormatting.AutoRescales.HasValue)
                o.AutoRescales = basicFormatting.AutoRescales.Value;
            if (basicFormatting.Movable.HasValue)
                o.Movable = basicFormatting.Movable.Value;
            if (basicFormatting.Printable.HasValue)
                o.Printable = basicFormatting.Printable.Value;
            if (basicFormatting.ResizesRealtime.HasValue)
                o.ResizesRealtime = basicFormatting.ResizesRealtime.Value;
            if (basicFormatting.Resizable.HasValue)
                o.Resizable = basicFormatting.Resizable.Value;
            if (basicFormatting.Reshapable.HasValue)
                o.Reshapable = basicFormatting.Reshapable.Value;
            if (basicFormatting.Selectable.HasValue)
                o.Selectable = basicFormatting.Selectable.Value;
            if (basicFormatting.Shadowed.HasValue)
                o.Shadowed = basicFormatting.Shadowed.Value;
            if (basicFormatting.Visible.HasValue)
                o.Visible = basicFormatting.Visible.Value;
        }

        #endregion

        public void CancelSettings()
        {
            ApplySettings(PrimaryFormatting);
        }
    }
}