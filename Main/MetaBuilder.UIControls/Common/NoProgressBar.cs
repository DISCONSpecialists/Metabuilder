using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MetaBuilder.UIControls.Common
{
    [DesignerCategory("Code")]
    public class NoProgressBar : Control
    {

		#region Constructors (1) 

        public NoProgressBar()
        {
            //I put these at the top of every owner-drawn control...
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            //set up timer object
            cycle = new Timer();
            cycle.Tick += new EventHandler(cycle_Tick);
            if (m_CycleSpeed > 0)
                cycle.Interval = m_CycleSpeed;
            else
                cycle.Interval = 1;
            cycle.Enabled = Enabled;
            if (BorderStyle == 0)
            {
                BorderStyle = Border3DStyle.Flat;
            }
        }

		#endregion Constructors 


        #region "Internal Property Variables"
        private ElementStyle m_ShapeToDraw;
        private int m_ShapeSize;
        private int m_ShapeSpacing;
        private int m_CycleSpeed;
        private int m_ShapeCount;
        private Border3DStyle m_BorderStyle;
        #endregion

        #region "Internal Variables"
        private Timer cycle;
        private int currentActiveItem;
        #endregion

        #region "Properties"
        [Category("Appearance")]
        public ElementStyle ShapeToDraw
        {
            get { return m_ShapeToDraw; }
            set
            {
                if (value != m_ShapeToDraw)
                {
                    m_ShapeToDraw = value;
                    currentActiveItem = 0;
                    Invalidate();
                }
            }
        }

        [Category("Appearance")]
        public int ShapeSize
        {
            get { return m_ShapeSize; }
            set
            {
                if (value != m_ShapeSize && value > 0)
                {
                    m_ShapeSize = value;
                    currentActiveItem = 0;
                    RecalcCountAndInterval();
                    Invalidate();
                }
            }
        }

        [Category("Appearance")]
        public int ShapeSpacing
        {
            get { return m_ShapeSpacing; }
            set
            {
                if (value != m_ShapeSpacing && value > 0)
                {
                    m_ShapeSpacing = value;
                    currentActiveItem = 0;
                    RecalcCountAndInterval();
                    Invalidate();
                }
            }
        }

        public int ShapeCount
        {
            get { return m_ShapeCount; }
        }

        [Category("Behavior")]
        public int CycleSpeed
        {
            get { return m_CycleSpeed; }
            set
            {
                if (m_CycleSpeed != value)
                {
                    m_CycleSpeed = value;
                    m_ShapeCount = 0;
                    RecalcCountAndInterval();
                }
            }
        }

        [Category("Appearance")]
        public Border3DStyle BorderStyle
        {
            get { return m_BorderStyle; }
            set
            {
                if (value != m_BorderStyle)
                {
                    m_BorderStyle = value;
                    Invalidate();
                }
            }
        }
        #endregion

        #region "Graphic Routines"
        private const int SIZE_INCR = 2;

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            ControlPaint.DrawBorder3D(g, ClientRectangle, BorderStyle);
            for (int i = 1; i <= m_ShapeCount; i++)
            {
                Point pos;
                int x;
                int y;
                pos = CalculateItemPosition(i);
                x = pos.X;
                y = pos.Y;
                if (i == currentActiveItem)
                {
                    DrawShape(g, m_ShapeToDraw, x - SIZE_INCR, y - SIZE_INCR, m_ShapeSize + (SIZE_INCR*2));
                }
                else
                {
                    DrawShape(g, m_ShapeToDraw, x, y, m_ShapeSize);
                }
            }
        }

        private Point CalculateItemPosition(int index)
        {
            Point pos = new Point();
            pos.X = (m_ShapeSpacing*index) + (m_ShapeSize*(index - 1));
            pos.Y = (Height/2) - (m_ShapeSize/2);
            return pos;
        }

        private void DrawShape(Graphics g, ElementStyle shape, int x, int y, int size)
        {
            SolidBrush MyBrush = new SolidBrush(ForeColor);
            switch (shape)
            {
                case ElementStyle.Circle:
                    g.FillEllipse(MyBrush, x, y, size, size);
                    break;
                case ElementStyle.Square:
                    g.FillRectangle(MyBrush, x, y, size, size);
                    break;
            }
        }
        #endregion

        #region "Timer Event to advance animation"
        private void cycle_Tick(object sender, EventArgs e)
        {
            int oldActiveItem = currentActiveItem;
            if (currentActiveItem >= m_ShapeCount)
            {
                currentActiveItem = 1;
            }
            else
            {
                currentActiveItem += 1;
            }
            Invalidate(CalcItemRectangle(oldActiveItem));
            Invalidate(CalcItemRectangle(currentActiveItem));
        }

        private Rectangle CalcItemRectangle(int i)
        {
            Rectangle rect = new Rectangle();
            Point pos;
            pos = CalculateItemPosition(i);
            Rectangle with_1 = rect;
            with_1.X = pos.X - SIZE_INCR;
            with_1.Y = pos.Y - SIZE_INCR;
            with_1.Width = m_ShapeSize + (2*SIZE_INCR);
            with_1.Height = rect.Width;
            return rect;
        }
        #endregion

        #region "Resize Event means recalculating the # of shapes to draw"
        protected override void OnResize(EventArgs e)
        {
            RecalcCountAndInterval();
            base.OnResize(e);
        }

        private const int MIN_INTERVAL = 100;

        private void RecalcCountAndInterval()
        {
            int w = Width;
            int newShapeCount;
            if (m_ShapeSize > 0 & m_ShapeSpacing > 0)
            {
                newShapeCount =
                    (int)
                    (Math.Floor((double) ((double) (w - m_ShapeSpacing))/((double) (m_ShapeSize + m_ShapeSpacing))));
            }
            else
            {
                newShapeCount = 1;
            }
            if (newShapeCount != m_ShapeCount && newShapeCount > 0)
            {
                int interval = m_CycleSpeed/newShapeCount;
                if (interval >= MIN_INTERVAL)
                {
                    cycle.Interval = interval;
                }
                else
                {
                    cycle.Interval = MIN_INTERVAL;
                }
                m_ShapeCount = newShapeCount;
            }
        }
        #endregion

        #region "Animation Stops when Enabled = False"
        protected override void OnEnabledChanged(EventArgs e)
        {
            cycle.Enabled = Enabled;
        }
        #endregion
    }

    public enum ElementStyle
    {
        Square = 0,
        Circle = 1
    }
}