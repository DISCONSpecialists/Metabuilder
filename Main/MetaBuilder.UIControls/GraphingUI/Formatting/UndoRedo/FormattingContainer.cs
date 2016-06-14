namespace MetaBuilder.UIControls.GraphingUI.Formatting.FormatUndo
{
    // Replaces SettingsContainer
    public class FormattingContainer
    {

        #region Fields (3)

        private BasicFormatting generalSettings;
        private ShapeFormatting shapeSettings;
        private TextFormatting textSettings;

        #endregion Fields

        #region Constructors (1)

        public FormattingContainer()
        {
            textSettings = new TextFormatting();
            shapeSettings = new ShapeFormatting();
            generalSettings = new BasicFormatting();
        }

        #endregion Constructors

        #region Properties (3)

        public BasicFormatting GeneralSettings
        {
            get { return generalSettings; }
            set { generalSettings = value; }
        }

        public ShapeFormatting ShapeSettings
        {
            get { return shapeSettings; }
            set { shapeSettings = value; }
        }

        public TextFormatting TextSettings
        {
            get { return textSettings; }
            set { textSettings = value; }
        }

        #endregion Properties

        #region Methods (2)


        // Public Methods (2) 

        public void Reset()
        {
            textSettings = new TextFormatting();
            shapeSettings = new ShapeFormatting();
            generalSettings = new BasicFormatting();
        }

        public override string ToString()
        {
            return generalSettings.ToString() + "\n" + textSettings.ToString() + "\n" + shapeSettings.ToString();
        }


        #endregion Methods

    }
}