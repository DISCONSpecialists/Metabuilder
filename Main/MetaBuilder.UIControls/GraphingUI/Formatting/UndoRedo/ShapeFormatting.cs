using System.Drawing;
using System.Drawing.Drawing2D;
using MetaBuilder.Graphing.Formatting;

namespace MetaBuilder.UIControls.GraphingUI.Formatting.FormatUndo
{
    public class ShapeFormatting
    {
        #region Fill & Gradient
        private GradientType? _gradientType;

        public GradientType? GradientType
        {
            get { return _gradientType; }
            set { _gradientType = value; }
        }

        private bool? _isGradient;

        public bool? IsGradient
        {
            get { return _isGradient; }
            set { _isGradient = value; }
        }

        private Color? _fillColour;

        public Color? FillColour
        {
            get
            {
                if (_fillColour == Color.Transparent)
                    _fillColour = Color.White;
                return _fillColour;

            }
            set
            {
                if (value == Color.Transparent)
                    _fillColour = Color.White;
                else
                    _fillColour = value;
            }
        }

        private Color? _gradientStartColour;

        public Color? GradientStartColour
        {
            get
            {
                if (_gradientStartColour == Color.Transparent)
                    _gradientStartColour = Color.White;
                return _gradientStartColour;

            }
            set
            {
                if (value == Color.Transparent)
                    _gradientStartColour = Color.White;
                else
                    _gradientStartColour = value;
            }
        }

        private Color? _gradientEndColour;

        public Color? GradientEndColour
        {
            get
            {
                if (_gradientEndColour == Color.Transparent)
                    _gradientEndColour = Color.White;
                return _gradientEndColour;

            }
            set
            {
                if (value == Color.Transparent)
                    _gradientEndColour = Color.White;
                else
                    _gradientEndColour = value;
            }
        }
        #endregion

        #region Line & Pen
        private float? _penWidth;

        public float? PenWidth
        {
            get { return _penWidth; }
            set { _penWidth = value; }
        }

        private DashStyle? _dashStyle;

        public DashStyle? DashStyle
        {
            get { return _dashStyle; }
            set { _dashStyle = value; }
        }

        private LineCap? _penStartCap;

        public LineCap? PenStartCap
        {
            get { return _penStartCap; }
            set { _penStartCap = value; }
        }

        private LineCap? _penEndCap;

        public LineCap? PenEndCap
        {
            get { return _penEndCap; }
            set { _penEndCap = value; }
        }

        private Color? _penColour;

        public Color? PenColour
        {
            get
            {
                if (_penColour == Color.Transparent)
                    _penColour = Color.White;
                return _penColour;

            }
            set
            {
                if (value == Color.Transparent)
                    _penColour = Color.White;
                else
                    _penColour = value;
            }
        }

        private SizeF? _corner;

        public SizeF? Corner
        {
            get { return _corner; }
            set { _corner = value; }
        }
        #endregion
    }
}