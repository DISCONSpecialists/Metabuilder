/*
 *  Copyright © 2007 - DISCON Specialists
 *
 *
 *  
 *  
 *  
 *  
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Shapes
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CHARRANGE
    {
        public int cpMin;
        public int cpMax;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct FORMATRANGE
    {
        public IntPtr hdc;
        public IntPtr hdcTarget;
        public RECT rc;
        public RECT rcPage;
        public CHARRANGE chrg;
    }

    [Serializable]
    public class RichText : GoObject
    {

        #region Fields (14)

        public const int ChangedAutoResizes = 1553;
        public const int ChangedBackgroundColor = 1552;
        public const int ChangedRtf = 1551;
        private const int EM_FORMATRANGE = WM_USER + 57;
        private const int EM_SETZOOM = WM_USER + 225;
        private const int
            flagAutoResizes = 0x0100;
        private Color myBackgroundColor = Color.White;
        private Color myBorderColor;
        [NonSerialized]
        private GoControl myEditor = null;
        [NonSerialized]
        private Bitmap myImage = null;
        private int myInternalTextFlags = flagAutoResizes;
        // if true, reset bounding rect when text changes
        private RichTextBox myRichTextBox = null;
        private String myString = "";
        private const int WM_USER = 0x400;

        #endregion Fields

        #region Constructors (1)

        public RichText()
        {
            myBorderColor = Color.Silver;
        }

        #endregion Constructors

        #region Properties (5)

        [Category("Behavior"), DefaultValue(true)]
        [Description("Whether the bounds are recalculated when the rich text string changes.")]
        public virtual bool AutoResizes
        {
            get { return (myInternalTextFlags & flagAutoResizes) != 0; }
            set
            {
                bool old = (myInternalTextFlags & flagAutoResizes) != 0;
                if (old != value)
                {
                    if (value)
                        myInternalTextFlags |= flagAutoResizes;
                    else
                        myInternalTextFlags &= ~flagAutoResizes;
                    Changed(ChangedAutoResizes, 0, old, NullRect, 0, value, NullRect);
                }
            }
        }

        [Category("Appearance")]
        [Description("The background color for this rich text object.")]
        public virtual Color BackgroundColor
        {
            get { return myBackgroundColor; }
            set
            {
                Color old = myBackgroundColor;
                if (old != value)
                {
                    myBackgroundColor = value;
                    Changed(ChangedBackgroundColor, 0, old, NullRect, 0, value, NullRect);
                    ResetImage();
                }
            }
        }

        [Category("Appearance")]
        [Description("The border color for this rich text object.")]
        public virtual Color BorderColor
        {
            get { return myBorderColor; }
            set
            {
                Color old = myBorderColor;
                if (old != value)
                {
                    myBorderColor = value;
                    Changed(ChangedBackgroundColor, 0, old, NullRect, 0, value, NullRect);
                    ResetImage();
                }
            }
        }

        public override GoControl Editor
        {
            get { return myEditor; }
        }

        [Category("Appearance"), DefaultValue("")]
        [Description("The rich text string to be formatted and displayed.")]
        public virtual String Rtf
        {
            get { return myString; }
            set
            {
                String old = myString;
                if (old != value)
                {
                    myString = value;
                    Changed(ChangedRtf, 0, old, NullRect, 0, value, NullRect);
                    ResetImage();
                    if (AutoResizes)
                        UpdateSize();
                }
            }
        }

        #endregion Properties

        #region Methods (14)


        // Public Methods (7) 

        public override void ChangeValue(GoChangedEventArgs e, bool undo)
        {
            switch (e.SubHint)
            {
                case ChangedRtf:
                    Rtf = (String)e.GetValue(undo);
                    return;
                case ChangedBackgroundColor:
                    BackgroundColor = (Color)e.GetValue(undo);
                    return;
                case ChangedAutoResizes:
                    AutoResizes = (bool)e.GetValue(undo);
                    return;
                default:
                    base.ChangeValue(e, undo);
                    return;
            }
        }

        public override GoObject CopyObject(GoCopyDictionary env)
        {
            RichText newobj = (RichText)base.CopyObject(env);
            if (newobj != null)
            {
                // might as well share myImage, if any, but not any GoControl editor
                newobj.myEditor = null;
            }
            return newobj;
        }

        public override GoControl CreateEditor(GoView view)
        {
            GoControl editor = new GoControl();
            editor.ControlType = typeof(RichTextBoxControl);
            // make somewhat bigger, for a border
            RectangleF rect = Bounds;
            rect.X -= 2;
            rect.Y -= 2;
            rect.Width += 4 + SystemInformation.VerticalScrollBarWidth * view.DocScale;
            rect.Height += 4;
            editor.Bounds = rect;
            return editor;
        }

        public override void DoBeginEdit(GoView view)
        {
            if (view == null) return;
            if (Editor != null) return; // already editing
            RemoveSelectionHandles(view.Selection);
            myEditor = CreateEditor(view);
            Editor.EditedObject = this; // associate editor with this text object
            view.EditControl = Editor; // add GoControl object to view layer
            Control ctrl = Editor.GetControl(view); // create Control in view
            if (ctrl != null)
            {
                ctrl.Focus();
            }
        }

        public override void DoEndEdit(GoView view)
        {
            if (Editor != null)
            {
                Editor.EditedObject = null; // disassociate
                if (view != null)
                {
                    view.EditControl = null; // remove GoControl from view
                }
                myEditor = null;
                if (view != null)
                {
                    view.StartTransaction();
                    view.RaiseObjectEdited(this);
                    //gotimage = false;
                    view.FinishTransaction(GoUndoManager.TextEditName);
                }
            }
        }

        // end of TextBoxControl
        public override bool OnSingleClick(GoInputEventArgs evt, GoView view)
        {
            if (!CanEdit()) return false;
            if (!view.CanEditObjects()) return false;
            if (evt.Shift || evt.Control) return false;
            DoBeginEdit(view);
            return true;
        }

        delegate void PaintDelegate(Graphics g, GoView view);
        public override void Paint(Graphics g, GoView view)
        {
            if (view.InvokeRequired)
            {
                view.BeginInvoke(new PaintDelegate(Paint), new object[] { g, view });
            }
            else
            {
                if (view == null)
                    return;
                RectangleF drect = Bounds;
                /*if (view.IsPrinting && false )
                {
                    RichTextBox box = GetRichTextBox();
                    int width = Math.Max((int) Math.Ceiling(drect.Width), 10);
                    int height = Math.Max((int) Math.Ceiling(drect.Height), 10);
                    box.Size = new Size(width, height);
                    box.Rtf = this.Rtf;
                    box.BackColor = this.BackgroundColor;
                    g.FillRectangle(new SolidBrush(this.BackgroundColor), 0, 0, width, height);

                 * float dpix = g.DpiX;
                    float dpiy = g.DpiY;
                    Rectangle vrect = view.ConvertDocToView(drect);
                    Point[] points = new Point[]
                        {
                            new Point((int) ((vrect.X*1440)/dpix), (int) ((vrect.Y*1440)/dpiy)),
                            new Point((int) (((vrect.X + vrect.Width)*1440)/dpix), (int) (((vrect.Y + vrect.Height)*1440)/dpiy))
                        };
                    g.TransformPoints(CoordinateSpace.Device, CoordinateSpace.Page, points);

                    IntPtr imgdc = g.GetHdc();
                    FORMATRANGE fr;
                    fr.hdc = imgdc;
                    fr.hdcTarget = imgdc;
                    fr.rc.left = (int) points[0].X;
                    fr.rc.top = (int) points[0].Y;
                    fr.rc.right = (int) points[1].X;
                    fr.rc.bottom = (int) points[1].Y;
                    fr.rcPage.left = (int) points[0].X;
                    fr.rcPage.top = (int) points[0].Y;
                    fr.rcPage.right = (int) points[1].X;
                    fr.rcPage.bottom = (int) points[1].Y;
                    fr.chrg.cpMin = 0;
                    fr.chrg.cpMax = -1;

                    IntPtr lpar = Marshal.AllocCoTaskMem(Marshal.SizeOf(fr));
                    Marshal.StructureToPtr(fr, lpar, false);
                    IntPtr result = SendMessage(box.Handle, EM_FORMATRANGE, new IntPtr(1), lpar); // 1=render
                    SendMessage(box.Handle, EM_FORMATRANGE, new IntPtr(0), new IntPtr(0)); // free up cached data
                    Marshal.FreeCoTaskMem(lpar);

                    g.ReleaseHdc(imgdc);
                }
                else
                {*/

                GetImage();
                g.DrawImage(myImage, drect);

                //}
                g.DrawRectangle(new Pen(myBorderColor, 1f), Bounds.X, Bounds.Y, Width, Height);
            }
        }
        // Protected Methods (1) 

        protected override void OnBoundsChanged(RectangleF old)
        {
            base.OnBoundsChanged(old);
            if (Size != old.Size)
            {
                ResetImage();
            }
        }

        // Private Methods (6) 

        //bool gotimage = false;
        private Bitmap GetImage()
        {
            //if (gotimage)
            //    return myImage = new Bitmap(1, 1);

            if (myImage == null)
            {
                //gotimage = true;
                RectangleF drect = Bounds;
                int width = Math.Max((int)Math.Ceiling(drect.Width), 10);
                int height = Math.Max((int)Math.Ceiling(drect.Height), 10);
                RichTextBox box = GetRichTextBox();
                box.Size = new Size(width, height);
                myImage = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(myImage);
                g.FillRectangle(new SolidBrush(BackgroundColor), 0, 0, width, height);
                g.DrawRectangle(new Pen(myBorderColor, 1f), 0, 0, Bounds.Width - 1, Bounds.Height);
                float dpix = g.DpiX;
                float dpiy = g.DpiY;
                IntPtr imgdc = g.GetHdc();
                FORMATRANGE fr;
                fr.hdc = imgdc;
                fr.hdcTarget = imgdc;
                fr.rc.left = 0;
                fr.rc.top = 0;
                fr.rc.right = (int)(width * 1440 / dpix); // convert to TWIPS
                fr.rc.bottom = (int)(height * 1440 / dpiy);
                fr.rcPage.left = 0;
                fr.rcPage.top = 0;
                fr.rcPage.right = (int)(width * 1440 / dpix);
                fr.rcPage.bottom = (int)(height * 1440 / dpiy);
                fr.chrg.cpMin = 0;
                fr.chrg.cpMax = -1;
                IntPtr lpar = Marshal.AllocCoTaskMem(Marshal.SizeOf(fr));
                Marshal.StructureToPtr(fr, lpar, false);
                IntPtr result = SendMessage(box.Handle, EM_FORMATRANGE, new IntPtr(1), lpar); // 1=render
                SendMessage(box.Handle, EM_FORMATRANGE, new IntPtr(0), new IntPtr(0)); // free up cached data
                Marshal.FreeCoTaskMem(lpar);
                g.ReleaseHdc(imgdc);
                g.Dispose();
            }
            return myImage;
        }

        private RichTextBox GetRichTextBox()
        {
            //if (myRichTextBox != null)
            //    myRichTextBox.Dispose();
            //if (myRichTextBox == null)
            //{
            myRichTextBox = new RichTextBox();
            myRichTextBox.Name = Guid.NewGuid().ToString();
            myRichTextBox.BorderStyle = BorderStyle.None;
            myRichTextBox.ScrollBars = RichTextBoxScrollBars.None;
            myRichTextBox.Rtf = Rtf;
            myRichTextBox.BackColor = BackgroundColor;
            myRichTextBox.Invalidate();
            myRichTextBox.Refresh();
            
            //}
            return myRichTextBox;
        }

        private SizeF Measure()
        {
            RectangleF drect = Bounds;
            int width = Math.Max((int)Math.Ceiling(drect.Width), 10);
            int height = 10; // doesn't matter?
            RichTextBox box = GetRichTextBox();
            box.Size = new Size(width, height);
            Image img = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(img);
            float dpix = g.DpiX;
            float dpiy = g.DpiY;
            IntPtr imgdc = g.GetHdc();
            FORMATRANGE fr;
            fr.hdc = imgdc;
            fr.hdcTarget = imgdc;
            fr.rc.left = 0;
            fr.rc.top = 0;
            fr.rc.right = (int)(width * 1440 / dpix); // convert to TWIPS
            fr.rc.bottom = 999999999;
            fr.rcPage.left = 0;
            fr.rcPage.top = 0;
            fr.rcPage.right = (int)(width * 1440 / dpix);
            fr.rcPage.bottom = 999999999;
            fr.chrg.cpMin = 0;
            fr.chrg.cpMax = -1;
            IntPtr lpar = Marshal.AllocCoTaskMem(Marshal.SizeOf(fr));
            Marshal.StructureToPtr(fr, lpar, false);
            IntPtr result = SendMessage(box.Handle, EM_FORMATRANGE, new IntPtr(0), lpar); // 0=measure
            FORMATRANGE fr2 = (FORMATRANGE)Marshal.PtrToStructure(lpar, typeof(FORMATRANGE));
            float newheight = (fr2.rc.bottom - fr2.rc.top) * dpiy / 1440;
            if (newheight > 999999)
                newheight = 20;
            SizeF size = new SizeF(width, newheight);
            SendMessage(box.Handle, EM_FORMATRANGE, new IntPtr(0), new IntPtr(0)); // free up cached data
            Marshal.FreeCoTaskMem(lpar);
            g.ReleaseHdc(imgdc);
            g.Dispose();
            return size;
        }

        private void ResetImage()
        {
            myImage = null;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private void UpdateSize()
        {
            SizeF newsize = Measure();
            SetSizeKeepingLocation(newsize);
        }


        #endregion Methods

        #region Nested Classes (1)


        public class RichTextBoxControl : RichTextBox, IGoControlObject
        {

            #region Fields (2)

            // TextBoxControl state
            private GoControl myGoControl = null;
            private GoView myGraphView = null;

            #endregion Fields

            #region Constructors (1)

            // nested class
            public RichTextBoxControl()
            {
                AllowDrop = false;
                AutoSize = false;
                ScrollBars = RichTextBoxScrollBars.ForcedVertical;
            }

            #endregion Constructors

            #region Properties (2)

            public GoControl GoControl
            {
                get { return myGoControl; }
                set
                {
                    GoControl old = myGoControl;
                    if (old != value)
                    {
                        myGoControl = value;
                        if (value != null)
                        {
                            RichText gorichtext = value.EditedObject as RichText;
                            if (gorichtext != null)
                            {
                                Rtf = gorichtext.Rtf;
                                BackColor = gorichtext.BackgroundColor;
                            }
                        }
                    }
                }
            }

            public GoView GoView
            {
                get { return myGraphView; }
                set
                {
                    myGraphView = value;
                    ZoomFactor = myGraphView.DocScale;
                }
            }

            #endregion Properties

            #region Methods (2)


            // Protected Methods (2) 

            protected override void OnLeave(EventArgs evt)
            {
                GoControl ctrl = GoControl;
                if (ctrl != null)
                {
                    RichText gotext = ctrl.EditedObject as RichText;
                    if (gotext != null)
                    {
                        gotext.Rtf = Rtf;
                    }
                    ctrl.DoEndEdit(GoView);
                }
                base.OnLeave(evt);
            }

            protected override bool ProcessDialogKey(Keys key)
            {
                if (key == Keys.Escape)
                {
                    GoControl ctrl = GoControl;
                    if (ctrl != null)
                        ctrl.DoEndEdit(GoView);
                    GoView.Focus();
                    return true;
                }
                else if (key == Keys.Tab)
                {
                    GoControl ctrl = GoControl;
                    if (ctrl != null)
                    {
                        RichText gotext = ctrl.EditedObject as RichText;
                        if (gotext != null)
                        {
                            gotext.Rtf = Rtf;
                        }
                        ctrl.DoEndEdit(GoView);
                        GoView.Focus();
                    }
                    return true;
                }
                else
                {
                    return base.ProcessDialogKey(key);
                }
            }


            #endregion Methods

        }
        #endregion Nested Classes

    }
}