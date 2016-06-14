namespace MetaBuilder.UIControls.GraphingUI.Formatting
{
    /*
    public class SettingsCollection : Northwoods.Go.GoUndoManagerCompoundEdit
    {

		#region Fields (7) 

        private bool hasShapes;
        private bool hasText;
        private Hashtable previousSettings;
        private SettingsContainer prevSettings;
        private SettingsContainer redoSettings;
        private List<GoObject> selectedObjects;
        private SettingsContainer settings;

		#endregion Fields 

		#region Constructors (1) 

        public SettingsCollection(List<GoObject> selection)
        {
            settings = new SettingsContainer();
            if (selection.Count > 0)
            {
                previousSettings = new Hashtable();
                selectedObjects = new List<GoObject>();

                settings.GeneralFormatSettings.ReadBaseProperties(selection[0]);
                foreach (GoObject o in selection)
                {
                    if (!(o is GoPort))
                        selectedObjects.Add(o);
                }
                StoreProperties(selection);
            }

            if (selection.Count == 1)
            {
                SetupGlobalsForOneObject();
            }
        }

		#endregion Constructors 

		#region Properties (5) 

        public bool HasShapes
        {
            get { return hasShapes; }
        }

        public bool HasText
        {
            get { return hasText; }
        }

        public SettingsContainer PrevSettings
        {
            get { return prevSettings; }
            set { prevSettings = value; }
        }

        public SettingsContainer RedoSettings
        {
            get { return redoSettings; }
            set { redoSettings = value; }
        }

        public SettingsContainer Settings
        {
            get { return settings; }
            set { settings = value; }
        }

		#endregion Properties 

		#region Methods (11) 


		// Public Methods (4) 

        public void ApplySettings()
        {
            foreach (GoObject o in selectedObjects)
            {
                BaseSettings setting = previousSettings[o] as BaseSettings;
                if (setting is TextSettings)
                {
                    GoText txt = o as GoText; // Must always return non-null
                    FormatText(txt, settings.TextFormatSettings);
                }

                if (setting is ShapeSettings)
                {
                    FormatShape(o, settings.ShapeFormatSettings);
                }
            }
        }

        public void Cancel()
        {
            foreach (GoObject o in selectedObjects)
            {
                BaseSettings setting = previousSettings[o] as BaseSettings;
                if (setting is TextSettings)
                    FormatText(o as GoText, setting as TextSettings);

                if (setting is ShapeSettings)
                    FormatShape(o, setting as ShapeSettings);
            }

        }

        public BaseSettings CreateCopy(BaseSettings bsettings)
        {
            #region Copies and returns TextSettings
            if (bsettings is TextSettings)
            {
                TextSettings x = bsettings as TextSettings;
                TextSettings c = new TextSettings();
                c.Angle = x.Angle;
                c.Font = x.Font;
                c.Multiline = x.Multiline;
                c.StrikeThrough = x.StrikeThrough;
                c.TextColour = x.TextColour;
                c.Underline = x.Underline;
                c.Wrap = x.Wrap;
                c.WrappingWidth = x.WrappingWidth;
                return c;
            }
            #endregion

            #region Copies and returns ShapeSettings
            if (bsettings is ShapeSettings)
            {
                ShapeSettings x = bsettings as ShapeSettings;
                ShapeSettings c = new ShapeSettings();
                c.FillColour = x.FillColour;
                c.GradientEndColour = x.GradientEndColour;
                c.GradientStartColour = x.GradientStartColour;
                c.GradientType = x.GradientType;
                c.IsGradient = x.IsGradient;
                c.PenColour = x.PenColour;
                c.PenEndCap = x.PenEndCap;
                c.PenStartCap = x.PenStartCap;
                c.PenStyle = x.PenStyle;
                c.PenWidth = x.PenWidth;
                c.Corner = x.Corner;
                return c;
            }
            #endregion

            if (bsettings is GeneralSettings)
            {
                #region Copies and returns BaseSettings
                GeneralSettings existing = bsettings as GeneralSettings;
                GeneralSettings copy = new GeneralSettings();
                copy.AutoRescales = existing.AutoRescales;
                copy.AutoResizes = existing.AutoResizes;
                copy.Deletable = existing.Deletable;
                copy.Movable = existing.Movable;
                copy.Printable = existing.Printable;
                copy.Reshapable = existing.Reshapable;
                copy.Resizable = existing.Resizable;
                copy.ResizesRealtime = existing.ResizesRealtime;
                copy.Selectable = existing.Selectable;
                copy.Shadowed = existing.Shadowed;
                copy.Visible = existing.Visible;
                return copy;
            }
                #endregion
            return null;
        }

        public void Debug()
        {
            foreach (GoObject o in selectedObjects)
            {
                BaseSettings bsettings = previousSettings[o] as BaseSettings;
                System.Diagnostics.Debug.WriteLine(o.ToString() + " " + bsettings.ToString());
            }
        }



		// Private Methods (7) 

        private void AddBaseItem(GoObject o)
        {
            GeneralSettings bsettings = new GeneralSettings();
            previousSettings.Add(o, bsettings);
        }

        private void AddShapeItem(GoShape o)
        {
            ShapeSettings sSettings = new ShapeSettings();
            if (o.Brush is System.Drawing.SolidBrush)
            {
                SolidBrush solidBrush = o.Brush as SolidBrush;
                sSettings.FillColour = solidBrush.Color;
            }
            else if (o is IBehaviourShape) // Gradient shapes
            {
                IBehaviourShape ibShape = o as IBehaviourShape;
                GradientBehaviour gBehaviour = (GradientBehaviour)ibShape.Manager.GetExistingBehaviour(typeof(GradientBehaviour));
                if (gBehaviour != null)
                {
                    sSettings.GradientType = gBehaviour.MyBrush.GradientType;
                    sSettings.GradientStartColour = gBehaviour.MyBrush.InnerColor;
                    sSettings.GradientEndColour = gBehaviour.MyBrush.OuterColor;
                    sSettings.IsGradient = true;
                }
            }
            if (o.Pen != null)
            {
                sSettings.PenColour = o.Pen.Color;
                sSettings.PenEndCap = o.Pen.EndCap;
                sSettings.PenStartCap = o.Pen.StartCap;
                sSettings.PenStyle = o.Pen.DashStyle;
                sSettings.PenWidth = o.Pen.Width;
            }

            if (o is GradientRoundedRectangle)
            {
                sSettings.Corner = (o as GradientRoundedRectangle).Corner;
                Settings.ShapeFormatSettings.Corner = sSettings.Corner;
            }
            else
            {
                sSettings.Corner = new System.Drawing.SizeF(-1, -1);
            }

            previousSettings.Add(o, sSettings);
        }

        private void AddTextItem(GoText o)
        {
            TextSettings tSettings = new TextSettings();
            tSettings.Angle = 0;
            tSettings.Font = o.Font;
            tSettings.Multiline = o.Multiline;
            tSettings.Wrap = o.Wrapping;
            tSettings.WrappingWidth = o.WrappingWidth;
            tSettings.Underline = o.Underline;
            tSettings.TextColour = o.TextColor;
            tSettings.StrikeThrough = o.StrikeThrough;
            tSettings.Alignment = o.Alignment;
            tSettings.Bordered = o.Bordered;
            previousSettings.Add(o, tSettings);
        }

        private void FormatShape(GoObject o, ShapeSettings settings)
        {
            GoShape shp = o as GoShape;
            if (shp is IBehaviourShape)
            {
                GradientBehaviour gradBehaviour = new GradientBehaviour();
                gradBehaviour.MyBrush = new MetaBuilder.Graphing.Formatting.ShapeGradientBrush();
                gradBehaviour.MyBrush.OuterColor = settings.GradientEndColour.Value;
                gradBehaviour.MyBrush.InnerColor = settings.GradientStartColour.Value;
                gradBehaviour.MyBrush.GradientType = settings.GradientType.Value;

                IBehaviourShape ibShape = shp as IBehaviourShape;
                ibShape.Manager = ibShape.Manager.Copy(o);
                ibShape.Manager.RemoveBehaviourOfType(typeof(GradientBehaviour));
                gradBehaviour.Owner = shp;
                ibShape.Manager.AddBehaviour(gradBehaviour);
                gradBehaviour.Update(shp);

                if (settings.PenWidth.HasValue)
                {
                    shp.Pen = new Pen(settings.PenColour.Value);
                    shp.Pen.DashStyle = settings.PenStyle.Value;
                    shp.Pen.StartCap = settings.PenStartCap.Value;
                    shp.Pen.Width = settings.PenWidth.Value;
                    shp.Pen.EndCap = settings.PenEndCap.Value;
                }

            }

            if (shp is GradientRoundedRectangle && settings.Corner.HasValue)
            {
                GradientRoundedRectangle gradRRect = shp as GradientRoundedRectangle;
                gradRRect.Corner = settings.Corner.Value;
            }
        }

        private void FormatText(GoText txt, TextSettings txtSettings)
        {
            
            if (txtSettings.AutoRescales.HasValue)
                txt.AutoRescales = txtSettings.AutoRescales.Value;
            
            if (txtSettings.AutoResizes.HasValue)
                txt.AutoResizes = txtSettings.AutoResizes.Value;

            // Text Specific
            if (txtSettings.Wrap.HasValue)
                txt.Wrapping = txtSettings.Wrap.Value;
            
            if (txtSettings.WrappingWidth.HasValue)
                txt.WrappingWidth = txtSettings.WrappingWidth.Value;
            
            if (txtSettings.Bordered.HasValue)
                txt.Bordered = txtSettings.Bordered.Value;
            
            if (txtSettings.StrikeThrough.HasValue)
                txt.StrikeThrough = txtSettings.StrikeThrough.Value;
            
            if (txtSettings.TextColour.HasValue)
                txt.TextColor = txtSettings.TextColour.Value;
            
            if (txtSettings.Underline.HasValue)
                txt.Underline = txtSettings.Underline.Value;

            if (txtSettings.FontDirty)
                txt.Font = txtSettings.Font;

        }

        private void SetupGlobalsForOneObject()
        {
            GoObject o = selectedObjects[0];
            if (o is GoText)
            {
                GoText txt = o as GoText;
                settings.TextFormatSettings.Alignment = txt.Alignment;
                settings.TextFormatSettings.Angle = 0;
                settings.TextFormatSettings.Bordered = txt.Bordered;
                settings.TextFormatSettings.Font = txt.Font;
                settings.TextFormatSettings.Multiline = txt.Multiline;
                settings.TextFormatSettings.StrikeThrough = txt.StrikeThrough;
                settings.TextFormatSettings.TextColour = txt.TextColor;
                settings.TextFormatSettings.Underline = txt.Underline;
                settings.TextFormatSettings.Wrap = txt.Wrapping;
                settings.TextFormatSettings.WrappingWidth = txt.WrappingWidth;
            }

            if (o is GoShape)
            {
                GoShape shp = o as GoShape;
                settings.ShapeFormatSettings.PenColour = shp.Pen.Color;
                settings.ShapeFormatSettings.PenEndCap = shp.Pen.EndCap;
                settings.ShapeFormatSettings.PenStartCap = shp.Pen.StartCap;
                settings.ShapeFormatSettings.PenStyle = shp.Pen.DashStyle;
                settings.ShapeFormatSettings.PenWidth = shp.Pen.Width;

                if (o is IBehaviourShape)
                {
                    IBehaviourShape ibShape = o as IBehaviourShape;

                    GradientBehaviour gradBehaviour = ibShape.Manager.GetExistingBehaviour(typeof(GradientBehaviour)) as GradientBehaviour;
                    if (gradBehaviour != null)
                    {
                        settings.ShapeFormatSettings.GradientEndColour = gradBehaviour.MyBrush.OuterColor;
                        settings.ShapeFormatSettings.GradientStartColour = gradBehaviour.MyBrush.InnerColor;
                        settings.ShapeFormatSettings.GradientType = gradBehaviour.MyBrush.GradientType;
                    }
                }
            }
            settings.GeneralFormatSettings.ReadBaseProperties(o);
        }

        private void StoreProperties(List<GoObject> olist)
        {
            foreach (GoObject o in olist)
            {
                if (o is GoText)
                {
                    hasText = true;
                    AddTextItem(o as GoText);
                }
                if (o is GoShape)
                {
                    AddShapeItem(o as GoShape);
                    hasShapes = true;
                }
                if (o is GoGroup)
                {
                    AddBaseItem(o);
                    List<GoObject> children = new List<GoObject>();
                    GoGroupEnumerator groupEnum = (o as GoGroup).GetEnumerator();
                    while (groupEnum.MoveNext())
                    {
                        if (!(groupEnum.Current is GoPort))
                        {
                            selectedObjects.Add(groupEnum.Current);
                            children.Add(groupEnum.Current);
                        }
                    }
                    StoreProperties(children);
                }
            }
        }


		#endregion Methods 

    }*/
}