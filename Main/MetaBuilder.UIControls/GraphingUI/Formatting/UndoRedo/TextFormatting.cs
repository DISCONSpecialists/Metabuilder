using System.Drawing;

namespace MetaBuilder.UIControls.GraphingUI.Formatting.FormatUndo
{
    public class TextFormatting
    {

		#region Fields (16) 

        private int? _angle;
        private Font _font;
        private bool? _multiline;
        private bool? _strikeThrough;
        private Color? _textColour;
        private bool? _underline;
        private bool? _wrap;
        private float? _wrappingWidth;
        private int? alignment;
        private bool? autoResizes;
        private bool? bold;
        private bool? bordered;
        private bool? editable;
        private bool fontDirty;
        private float? fontSize;
        private bool? italic;

		#endregion Fields 

		#region Properties (16) 

        public int? Alignment
        {
            get { return alignment; }
            set { alignment = value; }
        }

        public int? Angle
        {
            get { return _angle; }
            set { _angle = value; }
        }

        public bool? AutoResizes
        {
            get { return autoResizes; }
            set { autoResizes = value; }
        }

        public bool? Bold
        {
            get { return bold; }
            set { bold = value; }
        }

        public bool? Bordered
        {
            get { return bordered; }
            set { bordered = value; }
        }

        public bool? Editable
        {
            get { return editable; }
            set { editable = value; }
        }

        public Font Font
        {
            get { return _font; }
            set
            {
                _font = value;
                fontDirty = true;
            }
        }

        public bool FontDirty
        {
            get { return fontDirty; }
            set { fontDirty = value; }
        }

        public float? FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }

        public bool? Italic
        {
            get { return italic; }
            set { italic = value; }
        }

        public bool? Multiline
        {
            get { return _multiline; }
            set { _multiline = value; }
        }

        public bool? StrikeThrough
        {
            get { return _strikeThrough; }
            set { _strikeThrough = value; }
        }

        public Color? TextColour
        {
            get { return _textColour; }
            set { _textColour = value; }
        }

        public bool? Underline
        {
            get { return _underline; }
            set { _underline = value; }
        }

        public bool? Wrap
        {
            get { return _wrap; }
            set { _wrap = value; }
        }

        public float? WrappingWidth
        {
            get { return _wrappingWidth; }
            set { _wrappingWidth = value; }
        }

		#endregion Properties 

    }
}