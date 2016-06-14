using System.Drawing;
using System.Drawing.Drawing2D;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Behaviours;
using MetaBuilder.Graphing.Shapes.Behaviours.Internal;
using Northwoods.Go;
using MetaBuilder.Graphing.Shapes.Primitives;

namespace MetaBuilder.Graphing.Formatting
{
    public class SelectionType
    {
        #region Fields (2)

        private bool hasShapes;
        private bool hasText;

        #endregion Fields

        #region Properties (2)

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

        #endregion Properties
    }

    public class FormattingManipulator : GoUndoManagerCompoundEdit
    {
        #region Fields (9)

        private readonly GoCollection targets;
        private bool _hasCornerShapes;
        private bool _hasShapes;
        // These properties are stored ONCE instead of iterating everytime
        private bool _hasText;
        private GoView myView;
        private FormatSettings newSettings;
        public FormatSettings prevSettings;
        private FormatSettings redoSettings;

        #endregion Fields

        #region Constructors (1)

        public FormattingManipulator(GoCollection objects)
        {
            //SaveEachObjectsFormatSettings(objects);
            targets = objects;

            StoreHasTextAndShapes();
            try
            {
                SaveCurrentSettings();
            }
            catch
            {
            }
        }

        public void ApplyToSelection(GoCollection targets)
        {
            NewSettings = prevSettings;
            foreach (GoObject o in targets)
            {
                FormatObject(o);
                if (o is GoGroup)
                {
                    foreach (GoObject ochild in (o as GoGroup))
                    {
                        FormatObject(ochild);
                    }
                }
            }
        }

        #endregion Constructors

        #region Properties (6)

        public bool HasCornerShapes
        {
            get { return _hasCornerShapes; }
            set { _hasCornerShapes = value; }
        }

        public bool HasShapes
        {
            get { return _hasShapes; }
            set { _hasShapes = value; }
        }

        public bool HasText
        {
            get { return _hasText; }
            set { _hasText = value; }
        }

        public GoView MyView
        {
            get { return myView; }
            set { myView = value; }
        }

        public FormatSettings NewSettings
        {
            get { return newSettings; }
            set { newSettings = value; }
        }

        public FormatSettings PrevSettings
        {
            get { return prevSettings; }
            set { prevSettings = value; }
        }

        #endregion Properties

        #region Methods (20)

        // Public Methods (8) 

        public void ApplyFormatSettings()
        {
            if (newSettings != null)
                prevSettings = newSettings;
            foreach (GoObject o in targets)
            {
                FormatObject(o);
                if (o is GoGroup)
                {
                    foreach (GoObject ochild in (o as GoGroup))
                    {
                        FormatObject(ochild);
                    }
                }
            }
        }

        public void Cancel()
        {
        }

        public override bool CanRedo()
        {
            return redoSettings != null;
        }

        public override bool CanUndo()
        {
            return prevSettings != null;
        }

        public override void Clear()
        {
            prevSettings = null;
            newSettings = null;
            redoSettings = null;
        }

        public override void Redo()
        {
            //// Console.WriteLine("Redo");
            /*target.MyManager.ApplySettings(redoSettings);
            redoSettings = null;*/
        }

        public void StoreHasTextAndShapes()
        {
            foreach (GoObject obj in targets)
            {
                if (obj is GoText)
                {
                    if (obj is BoundLabel)
                        if ((obj as BoundLabel).Name == "cls_id" || (obj as BoundLabel).ddf > 0)
                            continue;

                    if ((obj as GoText).EditorStyle == GoTextEditorStyle.TextBox)
                        _hasText = true;
                }
                if (obj is GoShape)
                {
                    _hasShapes = true;
                }
                if (obj is GoRoundedRectangle)
                {
                    _hasCornerShapes = true;
                }
                if (obj is RepeaterSection)
                {
                    _hasText = true;
                    _hasShapes = true; // for the background property
                }
            }
        }

        public override void Undo()
        {
            //  // Console.WriteLine("Undo");
            /*
            redoSettings = target.MyManager.CurrentSettings;
            target.MyManager.ApplySettings(prevSettings);*/
        }

        // Private Methods (12) 

        private void ApplyFill(GoShape shp)
        {
            bool ShouldFormatPort = false;
            if (targets.Count <= 0)
                ShouldFormatPort = true;
            if (shp is GoPort && ShouldFormatPort == false)
                return;
            if (newSettings.IsGradient)
            {
                if (shp is IBehaviourShape)
                {
                    (shp as IBehaviourShape).Manager = new BaseShapeManager();
                    shp.Brush = new SolidBrush(newSettings.FillColour);
                    ShapeGradientBrush sgb = new ShapeGradientBrush();
                    sgb.BorderColor = newSettings.PenColour;
                    sgb.InnerColor = newSettings.GradientStartColour;
                    sgb.OuterColor = newSettings.GradientEndColour;
                    sgb.GradientType = newSettings.GradientType;
                    GradientBehaviour gradObject = (shp as IBehaviourShape).Manager.GetExistingBehaviour(typeof(GradientBehaviour)) as GradientBehaviour;
                    if (gradObject == null)
                    {
                        gradObject = new GradientBehaviour();
                        (shp as IBehaviourShape).Manager.AddBehaviour(gradObject);
                    }
                    gradObject.MyBrush = sgb;
                    gradObject.Update(shp);
                }
            }
            else
            {
                if (shp is IBehaviourShape && !newSettings.FillColour.IsEmpty)
                {
                    (shp as IBehaviourShape).Manager.RemoveBehaviourOfType(typeof(GradientBehaviour));
                    Brush b = new SolidBrush(newSettings.FillColour);
                    shp.Brush = b;
                }
            }
        }

        private void FormatBoundLabel(GoText txtInstance)
        {
            if (NewSettings.Font != null)
            {
                if (txtInstance is BoundLabel)
                    if ((txtInstance as BoundLabel).Name == "cls_id" || (txtInstance as BoundLabel).ddf > 0)
                        return;

                bool validTextItem = false;
                if (txtInstance.ParentNode is GraphNode)
                {
                    //if ((txtInstance as BoundLabel).Name == "cls_id" || (txtInstance as BoundLabel).ddf > 0)//|| (txtInstance as BoundLabel).Name.ToLower().Contains("def_")
                    //    return;

                    // At least half the width of the node? WTF IS THIS
                    if (txtInstance.Width >= txtInstance.TopLevelObject.Width / 2)
                    {
                        validTextItem = true;
                    }
                    if ((txtInstance.ParentNode as GraphNode).MetaObject.Class == "GovernanceMechanism")
                    {
                        if (txtInstance is BoundLabel)
                            if ((txtInstance as BoundLabel).Name.Contains("- 0"))
                                validTextItem = false;
                    }
                }
                else
                {
                    //if ((txtInstance as BoundLabel).Name == "cls_id" || (txtInstance as BoundLabel).ddf > 0)
                    //    validTextItem = false;
                    //else
                    validTextItem = true;
                }
                if (validTextItem)
                {
                    txtInstance.Font = newSettings.Font;
                    txtInstance.Multiline = newSettings.Multiline;
                    txtInstance.StrikeThrough = newSettings.StrikeThrough;
                    txtInstance.TextColor = newSettings.TextColour;
                    txtInstance.Underline = newSettings.Underline;
                    txtInstance.Wrapping = newSettings.Wrap;
                    txtInstance.WrappingWidth = newSettings.WrappingWidth;
                    txtInstance.BackgroundColor = newSettings.FillColour;
                    Pen p = new Pen(newSettings.PenColour);
                    p.Width = newSettings.PenWidth;
                    p.StartCap = newSettings.PenStartCap;
                    p.EndCap = newSettings.PenEndCap;
                    p.DashStyle = newSettings.PenStyle;
                }
            }
        }

        private void FormatObject(GoObject obj)
        {
            if (obj is IRotatable)
            {
                //((IRotatable)obj).Angle = settings.Angle;
            }
            if (obj is BoundLabel)
            {
                BoundLabel txtInstance = obj as BoundLabel;
                if (txtInstance.EditorStyle == GoTextEditorStyle.TextBox)
                    FormatBoundLabel(txtInstance);
            }
            else if (obj is GoText)
            {
                GoText txtInstance = obj as GoText;
                if (txtInstance.EditorStyle == GoTextEditorStyle.TextBox)
                    FormatBoundLabel(txtInstance);
            }
            else if (obj is RepeaterSection)
            {
                GoText txtInstance = (obj as RepeaterSection).Label;
                if (txtInstance != null)
                    if (txtInstance.EditorStyle == GoTextEditorStyle.TextBox)
                        FormatBoundLabel(txtInstance);
            }
            else if (obj is CollapsingRecordNodeItemList)
            {
                foreach (GoObject myChildObj in obj as GoListGroup)
                {
                    FormatObject(myChildObj);
                }
            }
            else if (obj is GoGroup)
            {
                foreach (GoObject ochild in (obj as GoGroup))
                {
                    FormatObject(ochild);
                }
            }
            else
            {
                if (obj is GoShape && (!(obj is GoGrid)))
                {
                    /*Pen p = new Pen(newSettings.PenColour);
                    p.Width = newSettings.PenWidth;
                    p.StartCap = newSettings.PenStartCap;
                    p.EndCap = newSettings.PenEndCap;
                    p.DashStyle = newSettings.PenStyle;*/
                    GoShape shp = obj as GoShape;
                    //shp.Pen = p;
                    ApplyFill(shp);
                }
            }
        }

        private FormatSettings RetrieveSettings(GoObject o)
        {
            FormatSettings retval = new FormatSettings();
            StoreTextSettings(o, ref retval);
            return retval;
        }

        private void RetrieveTextSettings(GoText BoundLabelInstance)
        {
            if (BoundLabelInstance is BoundLabel)
                if ((BoundLabelInstance as BoundLabel).Name == "cls_id" || (BoundLabelInstance as BoundLabel).ddf > 0)
                    return;

            // try and filter out small text labels, since their font sizes shouldn't be adjusted
            bool validTextItem = false;
            if (BoundLabelInstance.TopLevelObject is GraphNode)
            {
                // At least half the width of the node
                if (BoundLabelInstance.Width >= BoundLabelInstance.TopLevelObject.Width / 2)
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
                prevSettings.Font = BoundLabelInstance.Font;
                prevSettings.AutoResizes = BoundLabelInstance.AutoResizes;
                prevSettings.Multiline = BoundLabelInstance.Multiline;
                prevSettings.StrikeThrough = BoundLabelInstance.StrikeThrough;
                prevSettings.TextColour = BoundLabelInstance.TextColor;
                prevSettings.Underline = BoundLabelInstance.Underline;
                prevSettings.Wrap = BoundLabelInstance.Wrapping;
                prevSettings.Font = new Font(BoundLabelInstance.Font.Name, BoundLabelInstance.FontSize, BoundLabelInstance.Font.Style);
                prevSettings.WrappingWidth = BoundLabelInstance.WrappingWidth;
            }
        }

        private void SaveBrushAndPen(GoObject objTarget)
        {
            if (prevSettings == null)
                prevSettings = new FormatSettings();
            GoShape shp = objTarget as GoShape;

            if (shp.Pen != null)
            {
                if (shp is IIdentifiable)
                    if ((shp as IIdentifiable).Name.ToLower() == "rect") //some shapes(old?) have a second gradient rectangle called rect for some reason which uses ghostwhite as its 2 colours
                        return;

                prevSettings.PenColour = shp.Pen.Color;
                prevSettings.PenEndCap = shp.Pen.EndCap;
                prevSettings.PenStartCap = shp.Pen.StartCap;
                prevSettings.PenStyle = shp.Pen.DashStyle;
                prevSettings.PenWidth = shp.Pen.Width;
                Brush b = shp.Brush;
                if (b is LinearGradientBrush)
                {
                    LinearGradientBrush lbg = b as LinearGradientBrush;
                    prevSettings.IsGradient = true;
                    prevSettings.GradientStartColour = lbg.LinearColors[0];
                    prevSettings.GradientEndColour = lbg.LinearColors[1];
                }
                else if (b is PathGradientBrush)
                {
                    PathGradientBrush pgb = b as PathGradientBrush;
                    prevSettings.IsGradient = true;
                    prevSettings.GradientStartColour = pgb.CenterColor;
                    prevSettings.GradientEndColour = pgb.SurroundColors[0];
                }
                else
                {
                    SolidBrush sbrush = shp.Brush as SolidBrush;
                    if (sbrush != null)
                        prevSettings.FillColour = sbrush.Color;
                }
                if (objTarget is GoRoundedRectangle)
                {
                    GoRoundedRectangle gorilla = objTarget as GoRoundedRectangle;
                    prevSettings.Corner = gorilla.Corner;
                }
            }
        }

        private void SaveCurrentSettings()
        {
            prevSettings = new FormatSettings();
            if (targets.Count == 1)
            {
                GoObject objTarget = targets.First;
                SaveResizingAndScaling(objTarget);
                if (objTarget is GoShape)
                {
                    SaveBrushAndPen(objTarget);
                }
                prevSettings.Visible = objTarget.Visible;
                if (objTarget is GoText)
                {
                    GoText BoundLabelInstance = objTarget as GoText;
                    RetrieveTextSettings(BoundLabelInstance);
                }
                if (objTarget is RepeaterSection)
                {
                    GoText repeaterHeader = (objTarget as RepeaterSection).Label;
                    RetrieveTextSettings(repeaterHeader);
                }
                if (targets.First is GoGroup)
                {
                    GoGroup grp = targets.First as GoGroup;
                    foreach (GoObject ob in grp)
                    {
                        if (ob is GoText)
                        {
                            GoText lbl = ob as GoText;
                            if (lbl.EditorStyle == GoTextEditorStyle.TextBox)
                            {
                                RetrieveTextSettings(lbl);
                            }
                        }
                    }

                    foreach (GoObject shp in grp)
                    {
                        if (shp is GoShape)
                        {
                            if (shp is GoPort) //QuickPorts are IBehaviourShapes and we dont want that
                                continue;
                            //conditional is a gradientpolygon
                            if (!(shp is GradientRoundedRectangle) && !(shp is GradientEllipse) && !(shp is GradientTrapezoid) && !(shp is GradientPolygon) && !(shp is GradientValueChainStep))
                                continue;

                            SaveBrushAndPen(shp as GoShape);
                            break;
                        }
                    }
                }
            }
            else
            {
                if (HasText)
                {
                    foreach (GoObject ob in targets)
                    {
                        if (ob is GoText)
                        {
                            GoText lbl = ob as GoText;
                            if (lbl.EditorStyle == GoTextEditorStyle.TextBox)
                            {
                                RetrieveTextSettings(lbl);
                            }
                        }
                    }
                }
                if (HasShapes)
                {
                    //foreach (GoObject ob in targets)
                    //{
                    //    if (ob is GoShape)
                    //    {
                    //        SaveBrushAndPen(ob as GoShape);
                    //    }
                    //}
                    foreach (GoObject shp in targets)
                    {
                        if (shp is GoShape)
                        {
                            if (shp is GoPort) //QuickPorts are IBehaviourShapes and we dont want that
                                continue;
                            //conditional is a gradientpolygon
                            if (!(shp is GradientRoundedRectangle) && !(shp is GradientEllipse) && !(shp is GradientTrapezoid) && !(shp is GradientPolygon) && !(shp is GradientValueChainStep))
                                continue;

                            SaveBrushAndPen(shp as GoShape);
                            break;
                        }
                    }
                }
            }
            newSettings = prevSettings;
        }

        private void SaveResizingAndScaling(GoObject objTarget)
        {
            prevSettings.AutoRescale = objTarget.AutoRescales;
            prevSettings.DragsParentShape = objTarget.DragsNode;
            prevSettings.LockText = objTarget.Editable;
            prevSettings.PenColour = Color.Black;
            prevSettings.Printable = objTarget.Printable;
            prevSettings.ProtectFromDeletion = objTarget.Deletable;
            prevSettings.Resizable = objTarget.Resizable;
            prevSettings.ResizesRealtime = objTarget.ResizesRealtime;
            prevSettings.Selectable = objTarget.Selectable;
        }

        private void StoreTextSettings(GoObject o, ref FormatSettings settings)
        {
            if (o is GoText)
            {
                GoText txtObject = o as GoText;
                settings.Font = txtObject.Font;
                settings.LockText = txtObject.Editable;
                settings.Multiline = txtObject.Multiline;
                settings.Printable = txtObject.Printable;
                settings.ProtectFromDeletion = txtObject.Deletable;
                settings.Resizable = txtObject.Resizable;
                settings.Selectable = txtObject.Selectable;
                settings.StrikeThrough = txtObject.StrikeThrough;
                settings.Underline = txtObject.Underline;
                settings.Wrap = txtObject.Wrapping;
                settings.WrappingWidth = txtObject.WrappingWidth;
                settings.AutoRescale = txtObject.AutoRescales;
                settings.AutoResizes = txtObject.AutoResizes;
            }
        }

        #endregion Methods
    }

    #region OLD CODE

    /* OLD CODE FORMATTINGMANIPULATOR 
     * public class FormattingManipulator
    {

		#region Fields (8) 

        private FormatSettings _currentFormatSettings;
        private bool _hasCornerShapes;
        private bool _hasShapes;
        // These properties are stored ONCE instead of iterating everytime
        private bool _hasText;
        private Hashtable hashTargets;
        private GoView myVieq;
        private FormatSettings settings;
        private GoSelection targets;

		#endregion Fields 

		#region Constructors (1) 

        public FormattingManipulator(GoSelection objects)
        {
            SaveEachObjectsFormatSettings(objects);
            targets = objects;
            StoreHasTextAndShapes();
            try
            {
                SaveCurrentSettings();
            }
            catch { }
        }

		#endregion Constructors 

		#region Properties (5) 

        public bool HasCornerShapes
        {
            get { return _hasCornerShapes; }
            set { _hasCornerShapes = value; }
        }

        public bool HasShapes
        {
            get { return _hasShapes; }
            set { _hasShapes = value; }
        }

        public bool HasText
        {
            get { return _hasText; }
            set { _hasText = value; }
        }

        public GoView MyView
        {
            get { return myVieq; }
            set { myVieq = value; }
        }

        public FormatSettings Settings
        {
            get { return settings; }
            set { settings = value; }
        }

		#endregion Properties 

		#region Methods (15) 


		// Public Methods (3) 

        public void ApplyFormatSettings()
        {

            foreach (GoObject o in targets)
            {
                FormatObject(o);
                if (o is GoGroup)
                {
                    foreach (GoObject ochild in (o as GoGroup))
                    {
                        FormatObject(ochild);
                    }
                }
            }
        }

        public void Cancel()
        {

        }

        public void StoreHasTextAndShapes()
        {
            foreach (GoObject obj in targets)
            {
                if (obj is GoText)
                {
                    _hasText = true;
                }
                if (obj is GoShape)
                {
                    _hasShapes = true;
                }
                if (obj is GoRoundedRectangle)
                {
                    _hasCornerShapes = true;
                }
                if (obj is RepeaterSection)
                {
                    _hasText = true;
                    _hasShapes = true; // for the background property
                }
            }
        }



		// Private Methods (12) 

        private void ApplyFill(GoShape shp)
        {
            
            bool ShouldFormatPort = false;
            if (targets.Count > 0)
                ShouldFormatPort = false;
            else
                ShouldFormatPort = true;
            if (shp is GoPort && ShouldFormatPort == false)
                return;

            if (settings.IsGradient)
            {
                shp.Brush = new SolidBrush(settings.FillColour);
                if (shp is IBehaviourShape)
                {
                    ShapeGradientBrush sgb = new ShapeGradientBrush();
                    sgb.BorderColor = settings.PenColour;
                    sgb.InnerColor = settings.GradientStartColour;
                    sgb.OuterColor = settings.GradientEndColour;
                    sgb.GradientType = _currentFormatSettings.GradientType;
                    GradientBehaviour gradObject = (shp as IBehaviourShape).Manager.GetExistingBehaviour(typeof(GradientBehaviour)) as GradientBehaviour;
                    if (gradObject == null)
                    {
                        gradObject = new GradientBehaviour();
                        (shp as IBehaviourShape).Manager.AddBehaviour(gradObject);

                    }
                    gradObject.MyBrush = sgb;
                    gradObject.Update(shp);
                }
            }
            else
            {
                
                if (shp is IBehaviourShape)
                {
                    (shp as Shapes.Behaviours.IBehaviourShape).Manager.RemoveBehaviourOfType(typeof(Shapes.Behaviours.Internal.GradientBehaviour));
                }
                Brush b = new SolidBrush(settings.FillColour);
                shp.Brush = b;
            }
        }

        private void FormatBoundLabel(GoText txtInstance)
        {
            txtInstance.Font = settings.Font;
            txtInstance.Multiline = settings.Multiline;
            txtInstance.StrikeThrough = settings.StrikeThrough;
            txtInstance.TextColor = settings.TextColour;
            txtInstance.Underline = settings.Underline;
            txtInstance.Wrapping = settings.Wrap;
            txtInstance.WrappingWidth = settings.WrappingWidth;
            txtInstance.BackgroundColor = settings.FillColour;

            Pen p = new Pen(settings.PenColour);
            p.Width = settings.PenWidth;
            p.StartCap = settings.PenStartCap;
            p.EndCap = settings.PenEndCap;
            p.DashStyle = settings.PenStyle;
        }

        private void FormatObject(GoObject obj)
        {
           
            if (obj is IRotatable)
            {
                ((IRotatable)obj).Angle = settings.Angle;
            }
            if (obj is BoundLabel)
            {
                BoundLabel txtInstance = obj as BoundLabel;
                FormatBoundLabel(txtInstance);

            }
            else if (obj is RepeaterSection)
            {
                GoText txtInstance = (obj as RepeaterSection).Label;
                FormatBoundLabel(txtInstance);
            }
            else
            {
                    if (obj is GoShape)
                    {
                        Pen p = new Pen(settings.PenColour);
                        p.Width = settings.PenWidth;
                        p.StartCap = settings.PenStartCap;
                        p.EndCap = settings.PenEndCap;
                        p.DashStyle = settings.PenStyle;
                        GoShape shp = obj as GoShape;
                        shp.Pen = p;
                        ApplyFill(shp);
                    }
            }
        }

        private FormatSettings RetrieveSettings(GoObject o)
        {
            FormatSettings retval = new FormatSettings();
            StoreTextSettings(o, ref retval);
            StoreFillSettings(o, ref retval);
            StoreLineSettings(o, ref retval);
            return retval;
        }

        private void RetrieveTextSettings(GoText BoundLabelInstance)
        {
            _currentFormatSettings.Font = BoundLabelInstance.Font;
            _currentFormatSettings.AutoResizes = BoundLabelInstance.AutoResizes;
            _currentFormatSettings.Multiline = BoundLabelInstance.Multiline;
            _currentFormatSettings.StrikeThrough = BoundLabelInstance.StrikeThrough;
            _currentFormatSettings.TextColour = BoundLabelInstance.TextColor;
            _currentFormatSettings.Underline = BoundLabelInstance.Underline;
            _currentFormatSettings.Wrap = BoundLabelInstance.Wrapping;
            _currentFormatSettings.Font = new Font(BoundLabelInstance.Font.Name, BoundLabelInstance.FontSize, BoundLabelInstance.Font.Style);
            _currentFormatSettings.WrappingWidth = BoundLabelInstance.WrappingWidth;
            if (BoundLabelInstance is IRotatable)
            {
                _currentFormatSettings.Angle = ((IRotatable)BoundLabelInstance).Angle;
            }
        }

        private void SaveBrushAndPen(GoObject objTarget)
        {
            _currentFormatSettings = new FormatSettings();
            GoShape shp = objTarget as GoShape;

            _currentFormatSettings.PenColour = shp.Pen.Color;
            _currentFormatSettings.PenEndCap = shp.Pen.EndCap;
            _currentFormatSettings.PenStartCap = shp.Pen.StartCap;
            _currentFormatSettings.PenStyle = shp.Pen.DashStyle;
            _currentFormatSettings.PenWidth = shp.Pen.Width;
            Brush b = shp.Brush;
            if (b is LinearGradientBrush)
            {
                LinearGradientBrush lbg = b as LinearGradientBrush;
                _currentFormatSettings.IsGradient = true;
                _currentFormatSettings.GradientStartColour = lbg.LinearColors[0];
                _currentFormatSettings.GradientEndColour = lbg.LinearColors[1];
            }
            else if (b is PathGradientBrush)
            {
                PathGradientBrush pgb = b as PathGradientBrush;
                _currentFormatSettings.IsGradient = true;
                _currentFormatSettings.GradientStartColour = pgb.CenterColor;
                _currentFormatSettings.GradientEndColour = pgb.SurroundColors[0];

            }
            else
            {
                SolidBrush sbrush = shp.Brush as SolidBrush;
                if (sbrush != null)
                    _currentFormatSettings.FillColour = sbrush.Color;
            }

            if (objTarget is GoRoundedRectangle)
            {
                GoRoundedRectangle gorilla = objTarget as GoRoundedRectangle;
                _currentFormatSettings.Corner = gorilla.Corner;

            }
        }

        private void SaveCurrentSettings()
        {
           
            _currentFormatSettings = new FormatSettings();
            if (targets.Count == 1)
            {
                GoObject objTarget = targets.First;
                SaveResizingAndScaling(objTarget);
                if (objTarget is GoShape)
                {
                    SaveBrushAndPen(objTarget);

                }
                _currentFormatSettings.Visible = objTarget.Visible;
                if (objTarget is GoText)
                {
                    GoText BoundLabelInstance = objTarget as GoText;
                    RetrieveTextSettings(BoundLabelInstance);
                }

                if (objTarget is RepeaterSection)
                {
                    GoText repeaterHeader = (objTarget as RepeaterSection).Label;
                    RetrieveTextSettings(repeaterHeader);
                }
            }
            else
            {
                if (HasText)
                {
                    foreach (GoObject ob in targets)
                    {
                        if (ob is BoundLabel)
                        {
                            BoundLabel lbl = ob as BoundLabel;
                            RetrieveTextSettings(lbl);
                        }
                    }
                }
                if (HasShapes)
                {
                    foreach (GoObject ob in targets)
                    {
                        if (ob is GoShape)
                        {
                            SaveBrushAndPen(ob as GoShape);
                        }
                    }
                }
            }
            settings = _currentFormatSettings;
        }

        private void SaveEachObjectsFormatSettings(GoSelection objects)
        {
            foreach (GoObject o in objects)
            {
                if (o is IIdentifiable)
                {
                    FormatSettings objectSettings = RetrieveSettings(o);
                    hashTargets.Add((o as IIdentifiable).Name,null);
                }
            }
        }

        private void SaveResizingAndScaling(GoObject objTarget)
        {

            _currentFormatSettings.AutoRescale = objTarget.AutoRescales;
            _currentFormatSettings.DragsParentShape = objTarget.DragsNode;
            _currentFormatSettings.LockText = objTarget.Editable;
            _currentFormatSettings.PenColour = Color.Black;
            _currentFormatSettings.Printable = objTarget.Printable;
            _currentFormatSettings.ProtectFromDeletion = objTarget.Deletable;
            _currentFormatSettings.Resizable = objTarget.Resizable;
            _currentFormatSettings.ResizesRealtime = objTarget.ResizesRealtime;
            _currentFormatSettings.Selectable = objTarget.Selectable;
        }

        private void StoreFillSettings(GoObject o, ref FormatSettings settings)
        {
            if (o is GoShape)
            {

            }
        }

        private void StoreLineSettings(GoObject o, ref FormatSettings settings)
        {
            if (o is GoShape)
            {

            }
        }

        private void StoreTextSettings(GoObject o, ref FormatSettings settings)
        {
            if (o is GoText)
            {
                GoText txtObject = o as GoText;
                settings.Font = txtObject.Font;
                settings.LockText = txtObject.Editable;
                settings.Multiline = txtObject.Multiline;
                settings.Printable = txtObject.Printable;
                settings.ProtectFromDeletion = txtObject.Deletable;
                settings.Resizable = txtObject.Resizable;
                settings.Selectable = txtObject.Selectable;
                settings.StrikeThrough = txtObject.StrikeThrough;
                settings.Underline = txtObject.Underline;
                settings.Wrap = txtObject.Wrapping;
                settings.WrappingWidth = txtObject.WrappingWidth;
                settings.AutoRescale = txtObject.AutoRescales;
                settings.AutoResizes = txtObject.AutoResizes;
            }
        }


		#endregion Methods 

    }*/

    #endregion
}