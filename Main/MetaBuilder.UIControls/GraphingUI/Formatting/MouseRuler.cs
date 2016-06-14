using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MetaBuilder.UIControls.GraphingUI.Formatting
{
    public delegate void CaptureFinishedEventHandler(object sender, CaptureEventArgs e);

    public delegate void MovingEventHandler(object sender, CaptureEventArgs e);

    public class CaptureEventArgs : EventArgs
    {

        #region Fields (2)

        private Point _endPoint;
        private Point _startPoint;

        #endregion Fields

        #region Constructors (1)

        public CaptureEventArgs(Point startPoint, Point endPoint)
        {
            _startPoint = startPoint;
            _endPoint = endPoint;
        }

        #endregion Constructors

        #region Properties (2)

        public Point EndPoint
        {
            get { return _endPoint; }
        }

        public Point StartPoint
        {
            get { return _startPoint; }
        }

        #endregion Properties

    }

    public class MouseRuler : IDisposable
    {

        #region Fields (13)

        private Single _angle;
        private Color _backColor;
        private bool _capturing;
        private Single[] _compArray = new Single[] { 0.0f, 0.16f, 0.33f, 0.66f, 0.83f, 1.0f };
        private Boolean _disposed = false;
        private Color _foreColor;
        private Point _last;
        private Single _length;
        private int _lineWidth = 5;
        private bool _mouseCaptured;
        private Point _origin;
        private Control _parent;
        private float angleCaptured;

        #endregion Fields

        #region Constructors (1)

        // = new CaptureFinishedEventHandler(
        public MouseRuler(Control parent)
        {
            if (parent == null)
                throw new ArgumentNullException("parent", "MouseCapture must be associated with a control.");
            _parent = parent;
            _foreColor = _parent.ForeColor;
            _backColor = _parent.BackColor;
            _parent.Paint += new PaintEventHandler(_parent_Paint);
            _parent.MouseDown += new MouseEventHandler(_parent_MouseDown);
            _parent.MouseMove += new MouseEventHandler(_parent_MouseMove);
            _parent.MouseUp += new MouseEventHandler(_parent_MouseUp);
        }

        #endregion Constructors

        #region Properties (6)

        public float AngleCaptured
        {
            get { return angleCaptured; }
            set { angleCaptured = value; }
        }

        public Color BackColor
        {
            get { return _backColor; }
            set { _backColor = value; }
        }

        public bool Capturing
        {
            get { return _capturing; }
            set { _capturing = value; }
        }

        public Color ForeColor
        {
            get { return _foreColor; }
            set { _foreColor = value; }
        }

        public Single[] LineCompoundArray
        {
            get { return _compArray; }
            set
            {
                foreach (Single i in value)
                {
                    if ((i < 0) || (i > 1))
                    {
                        throw new ArgumentOutOfRangeException("LineCompoundArray", i,
                                                              "All elements in the compound array must be >=0 or <=1.");
                    }
                }
                _compArray = value;
            }
        }

        public int LineWidth
        {
            get { return _lineWidth; }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("LineWidth", "LineWidth must be set to 1 or higher");
                }
                _lineWidth = value;
            }
        }

        #endregion Properties

        #region Delegates and Events (1)

        // Events (1) 

        public event CaptureFinishedEventHandler CaptureFinishedEvent;

        #endregion Delegates and Events

        #region Methods (8)

        // Private Methods (8) 

        private void _parent_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseCaptured = true;
            _origin = e.Location;
            _last = new Point(-1, -1);
        }

        private void _parent_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseCaptured)
            {
                Rectangle r = NormalizeRect(_origin, _last);
                r.Inflate(_parent.Font.Height, _parent.Font.Height);
                _parent.Invalidate(r);
                _last = e.Location;
            }
        }

        private void _parent_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseCaptured = false;
            _parent.Invalidate();
            CaptureFinishedEvent(this, new CaptureEventArgs(_origin, e.Location));
        }

        private void _parent_Paint(object sender, PaintEventArgs e)
        {
            if (Capturing)
            {
                if (1 == 1) //_mouseCaptured)
                {
                    // Ensure points differ!
                    bool invalid = false;
                    if (_origin.Y == _last.Y)
                    {
                        invalid = true;
                    }
                    if (_origin.X == _last.X)
                    {
                        invalid = true;
                    }
                    if (invalid == true)
                        return;
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    Pen wallpen = new Pen(_foreColor, _lineWidth);
                    using (wallpen)
                    {
                        if ((_compArray != null) && (_compArray.Length > 0))
                        {
                            wallpen.CompoundArray = _compArray;
                        }
                        wallpen.SetLineCap(LineCap.RoundAnchor, LineCap.ArrowAnchor, DashCap.Flat);
                        e.Graphics.DrawLine(wallpen, _origin, _last);
                        Point midPoint = new Point();
                        midPoint.X = Math.Min(_origin.X, _last.X) +
                                     ((Math.Max(_origin.X, _last.X) - Math.Min(_origin.X, _last.X)) / 2);
                        midPoint.Y = Math.Min(_origin.Y, _last.Y) +
                                     ((Math.Max(_origin.Y, _last.Y) - Math.Min(_origin.Y, _last.Y)) / 2);
                        Matrix mx = new Matrix();
                        using (mx)
                        {
                            mx.Translate(midPoint.X, midPoint.Y);
                            mx.Rotate(Angle(_origin, _last));
                            e.Graphics.Transform = mx;
                            /* StringFormat sf = new StringFormat();
                                  using (sf)
                                  {
                                      string ls = Angle(_origin, _last).ToString();// LineLength(_origin, _last).ToString();
                                      SizeF l = e.Graphics.MeasureString(ls,_parent.Font,_parent.ClientSize,sf);
                                      sf.LineAlignment = StringAlignment.Center;
                                      sf.Alignment = StringAlignment.Center;
                                      Rectangle rt = new Rectangle(0, 0, Convert.ToInt16(l.Width), Convert.ToInt16(l.Height));
                                      rt.Inflate(3,3);
                                      rt.Offset(-(Convert.ToInt16(l.Width)/2),-(Convert.ToInt16(l.Height)/2));
                                      SolidBrush backBrush = new SolidBrush(_backColor);
                                      e.Graphics.FillEllipse(backBrush,rt);

                                      SolidBrush foreBrush = new SolidBrush(_foreColor);
                                      e.Graphics.DrawString(ls, _parent.Font, foreBrush, 0, 0, sf);
                                  }
                          */
                        }
                    }
                }
            }
        }

        private Single Angle(Point p1, Point p2)
        {
            Single prelimAngle = 270f +
                                 float.Parse((Math.Atan2((p1.Y - p2.Y), (p1.X - p2.X)) * (180f / Math.PI)).ToString(), System.Globalization.CultureInfo.InvariantCulture);
            _angle = (prelimAngle > 360) ? prelimAngle - 360f : prelimAngle;
            angleCaptured = _angle;
            return _angle;
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _parent.Paint -= new PaintEventHandler(_parent_Paint);
                _parent.MouseDown -= new MouseEventHandler(_parent_MouseDown);
                _parent.MouseMove -= new MouseEventHandler(_parent_MouseMove);
                _parent.MouseUp -= new MouseEventHandler(_parent_MouseUp);
                _parent.Dispose();
            }
            _disposed = true;
        }

        private Single LineLength(Point p1, Point p2)
        {
            Rectangle r = NormalizeRect(p1, p2);
            _length = float.Parse(Math.Sqrt(r.Width ^ 2 + r.Height ^ 2).ToString(), System.Globalization.CultureInfo.InvariantCulture);
            return _length;
        }

        private Rectangle NormalizeRect(Point p1, Point p2)
        {
            Rectangle r = new Rectangle();
            if (p1.X < p2.X)
            {
                r.X = p1.X;
                r.Width = p2.X - p1.X;
            }
            else
            {
                r.X = p2.X;
                r.Width = p1.X - p2.X;
            }
            if (p1.Y < p2.Y)
            {
                r.Y = p1.Y;
                r.Height = p2.Y - p1.Y;
            }
            else
            {
                r.Y = p2.Y;
                r.Height = p1.Y - p2.Y;
            }
            return r;
        }

        #endregion Methods

        #region " IDisposable Support "
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

    /*  Private Sub Painting(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs)

                Using mx As New System.Drawing.Drawing2D.Matrix
                    mx.Translate(midPoint.X, midPoint.Y)
                    mx.Rotate(Angle(_origin, _last))
                    e.Graphics.Transform = mx
                    Using sf As New StringFormat()
                        Dim ls As String = CInt(LineLength(_origin, _last))
                        Dim l As SizeF = e.Graphics.MeasureString(ls, _parent.Font, _parent.ClientSize, sf)
                        sf.LineAlignment = StringAlignment.Center
                        sf.Alignment = StringAlignment.Center

                        Dim rt As New Rectangle(0, 0, l.Width, l.Height)
                        rt.Inflate(3, 3)
                        rt.Offset(-(l.Width / 2), -(l.Height / 2))
                        Using backBrush As New SolidBrush(_backColor)
                            e.Graphics.FillEllipse(backBrush, rt)
                        End Using
                        Using foreBrush As New SolidBrush(_foreColor)
                            e.Graphics.DrawString(ls, _parent.Font, foreBrush, 0, 0, sf)
                        End Using
                    End Using
                End Using
            End Using
        End If
    End Sub

    Private Sub MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        _mouseCaptured = true
        _origin = e.Location
        _last = New Point(-1, -1)
    End Sub

    Private Sub MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        If _mouseCaptured Then
            Dim r As Rectangle = NormalizeRect(_origin, _last)
            r.Inflate(_parent.Font.Height, _parent.Font.Height)
            _parent.Invalidate(r)
            _last = e.Location
        End If
    End Sub

    Private Sub MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        _mouseCaptured = False
        _parent.Invalidate()
        RaiseEvent CaptureFinished(Me, New CaptureEventArgs(_origin, e.Location))
    End Sub

    Private Function NormalizeRect(ByVal p1 As Point, ByVal p2 As Point) As Rectangle
        Dim r As New Rectangle
        If p1.X < p2.X Then
            r.X = p1.X
            r.Width = p2.X - p1.X
        Else
            r.X = p2.X
            r.Width = p1.X - p2.X
        End If
        If p1.Y < p2.Y Then
            r.Y = p1.Y
            r.Height = p2.Y - p1.Y
        Else
            r.Y = p2.Y
            r.Height = p1.Y - p2.Y
        End If
        Return r
    End Function

    Private Function LineLength(ByVal p1 As Point, ByVal p2 As Point) As Single
        Dim r As Rectangle = NormalizeRect(p1, p2)
        _length = Math.Sqrt(r.Width ^ 2 + r.Height ^ 2)
        Return _length
    End Function

    Private Function Angle(ByVal p1 As Point, ByVal p2 As Point) As Single
        _angle = Math.Atan((p1.Y - p2.Y) / (p1.X - p2.X)) * (180 / Math.PI)
        Return _angle
    End Function

    ' IDisposable
    Private Overloads Sub Dispose(ByVal disposing As Boolean)
        If Not Me._disposed Then
            If disposing Then
                ' TODO: put code to dispose managed resources
                RemoveHandler _parent.Paint, AddressOf Me.Painting
                RemoveHandler _parent.MouseDown, AddressOf Me.MouseDown
                RemoveHandler _parent.MouseMove, AddressOf Me.MouseMove
                RemoveHandler _parent.MouseUp, AddressOf Me.MouseUp

                _parent.Dispose()
            End If

            ' TODO: put code to free unmanaged resources here
        End If
        Me._disposed = true
    End Sub

#Region " IDisposable Support "
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(true)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overrides Sub Finalize()
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(False)
        MyBase.Finalize()
    End Sub
#End Region

End Class
*/
}