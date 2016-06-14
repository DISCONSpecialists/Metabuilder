using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

namespace MetaBuilder.Graphing.Formatting
{
    [Serializable]
    public class FormatSettings
    {
        #region Constructors (1)

        public FormatSettings()
        {
            Font = new Font("MS Sans Serif", 12f);
        }

        #endregion Constructors

        #region Methods (1)

        // Public Methods (1) 

        public override string ToString()
        {
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("Font: " + Font.Name + " " + Font.Size.ToString());
            sBuilder.Append("Color: " + TextColour.Name);
            sBuilder.Append("GradientStart: " + GradientStartColour.Name);
            sBuilder.Append("GradientEnd: " + GradientEndColour.Name);
            sBuilder.Append("LinePen: " + PenStyle.ToString() + " " + PenColour.Name + " " + PenWidth.ToString());
            return sBuilder.ToString();
            //return "Font {0} Size {1} Color {2} GradientStart {3} GradientEnd {4} LinePen {5}"
        }

        #endregion Methods

        #region Font & Text

        private int _angle;
        private Font _font;
        private bool _multiline;
        private bool _strikeThrough;

        private Color _textColour;
        private bool _underline;
        private bool _wrap;
        private float _wrappingWidth;

        public Font Font
        {
            get { return _font; }
            set { _font = value; }
        }

        public Color TextColour
        {
            get { return _textColour; }
            set { _textColour = value; }
        }

        public bool Multiline
        {
            get { return _multiline; }
            set { _multiline = value; }
        }

        public bool Wrap
        {
            get { return _wrap; }
            set { _wrap = value; }
        }

        public float WrappingWidth
        {
            get { return _wrappingWidth; }
            set { _wrappingWidth = value; }
        }

        public bool StrikeThrough
        {
            get { return _strikeThrough; }
            set { _strikeThrough = value; }
        }

        public bool Underline
        {
            get { return _underline; }
            set { _underline = value; }
        }

        public int Angle
        {
            get { return _angle; }
            set { _angle = value; }
        }

        #endregion

        #region Line & Pen

        private SizeF _corner;
        private Color _penColour;
        private LineCap _penEndCap;
        private LineCap _penStartCap;
        private DashStyle _penStyle;
        private float _penWidth;

        public float PenWidth
        {
            get { return _penWidth; }
            set { _penWidth = value; }
        }

        public DashStyle PenStyle
        {
            get { return _penStyle; }
            set { _penStyle = value; }
        }

        public LineCap PenStartCap
        {
            get { return _penStartCap; }
            set { _penStartCap = value; }
        }

        public LineCap PenEndCap
        {
            get { return _penEndCap; }
            set { _penEndCap = value; }
        }

        public Color PenColour
        {
            get { return _penColour; }
            set { _penColour = value; }
        }

        public SizeF Corner
        {
            get { return _corner; }
            set { _corner = value; }
        }

        #endregion

        #region Fill & Gradient

        private Color _fillColour;
        private Color _gradientEndColour;
        private Color _gradientStartColour;
        private GradientType _gradientType;

        private bool _isGradient;

        public GradientType GradientType
        {
            get { return _gradientType; }
            set { _gradientType = value; }
        }

        public bool IsGradient
        {
            get { return _isGradient; }
            set { _isGradient = value; }
        }

        public Color FillColour
        {
            get { return _fillColour; }
            set { _fillColour = value; }
        }

        public Color GradientStartColour
        {
            get { return _gradientStartColour; }
            set
            {
                _gradientStartColour = value;
            }
        }

        public Color GradientEndColour
        {
            get { return _gradientEndColour; }
            set
            {
                _gradientEndColour = value;
            }
        }

        #endregion

        #region Behaviour

        private bool _autoRescale;
        private bool _autoResizes;
        private bool _dragsParentShape;
        private bool _lockText;
        private bool _printable;
        private bool _protectFromDeletion;
        private bool _resizable;
        private bool _resizesRealtime;
        private bool _selectable;
        private bool _visible;

        public bool Selectable
        {
            get { return _selectable; }
            set { _selectable = value; }
        }

        public bool Resizable
        {
            get { return _resizable; }
            set { _resizable = value; }
        }

        public bool ResizesRealtime
        {
            get { return _resizesRealtime; }
            set { _resizesRealtime = value; }
        }

        public bool AutoResizes
        {
            get { return _autoResizes; }
            set { _autoResizes = value; }
        }

        public bool DragsParentShape
        {
            get { return _dragsParentShape; }
            set { _dragsParentShape = value; }
        }

        public bool AutoRescale
        {
            get { return _autoRescale; }
            set { _autoRescale = value; }
        }

        public bool Printable
        {
            get { return _printable; }
            set { _printable = value; }
        }

        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        public bool LockText
        {
            get { return _lockText; }
            set { _lockText = value; }
        }

        public bool ProtectFromDeletion
        {
            get { return _protectFromDeletion; }
            set { _protectFromDeletion = value; }
        }

        #endregion
    }

    [Serializable]
    public enum GradientType
    {
        Horizontal,
        Vertical,
        ForwardDiagonal,
        BackwardDiagonal,
        Ellipse
    }
}