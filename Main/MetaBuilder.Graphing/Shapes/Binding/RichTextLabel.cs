/*
 *  Copyright © Northwoods Software Corporation, 1998-2006. All Rights
 *  Reserved.
 *
 *  Restricted Rights: Use, duplication, or disclosure by the U.S.
 *  Government is subject to restrictions as set forth in subparagraph
 *  (c) (1) (ii) of DFARS 252.227-7013, or in FAR 52.227-19, or in FAR
 *  52.227-14 Alt. III, as applicable.
 */

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Northwoods.Go;
using Northwoods.Go.Svg;
using System.Globalization;
using System.Security.Permissions;

namespace MetaBuilder.Graphing
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
    public class RichTextLabel : GoObject, IDisposable
    {
        #region Fields (13)

        public const int ChangedAutoResizes = 1553;
        public const int ChangedBackgroundColor = 1552;
        public const int ChangedRtf = 1551;
        private const int EM_FORMATRANGE = WM_USER + 57;

        private const int flagAutoResizes = 0x0100;

        private const int WM_USER = 0x400;
        private static RichTextBox myRichTextBox;

        private Color myBackgroundColor = Color.White;
        [NonSerialized]
        private GoControl myEditor;
        [NonSerialized]
        private Bitmap myImage;
        private int myInternalTextFlags = flagAutoResizes;
        // if true, reset bounding rect when text changes
        private String myString = "";

        #endregion Fields

        #region Constructors (1)

        #endregion Constructors

        #region Properties (4)

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
            RichTextLabel newobj = (RichTextLabel)base.CopyObject(env);
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
            if (view == null)
                return;
            if (Editor != null)
                return; // already editing
            //view.StartTransaction();
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
                    view.RaiseObjectEdited(this);
                    //view.FinishTransaction(GoUndoManager.TextEditName);
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

        public override void Paint(Graphics g, GoView view)
        {
            if (view == null) return;
            RectangleF drect = Bounds;
            if (view.IsPrinting /* && false ??? */)
            {
                RichTextBox box = GetRichTextBox();
                int width = Math.Max((int)Math.Ceiling(drect.Width), 10);
                int height = Math.Max((int)Math.Ceiling(drect.Height), 10);
                box.Size = new Size(width, height);
                box.Rtf = Rtf;
                box.BackColor = BackgroundColor;
                float dpix = g.DpiX;
                float dpiy = g.DpiY;
                Rectangle vrect = view.ConvertDocToView(drect);
                Point[] points = new Point[]
                                     {
                                         new Point((int) ((vrect.X*1440)/dpix), (int) ((vrect.Y*1440)/dpiy)),
                                         new Point((int) (((vrect.X + vrect.Width)*1440)/dpix),
                                                   (int) (((vrect.Y + vrect.Height)*1440)/dpiy))
                                     };
                g.TransformPoints(CoordinateSpace.Device, CoordinateSpace.Page, points);
                IntPtr imgdc = g.GetHdc();
                FORMATRANGE fr;
                fr.hdc = imgdc;
                fr.hdcTarget = imgdc;
                fr.rc.left = points[0].X;
                fr.rc.top = points[0].Y;
                fr.rc.right = points[1].X;
                fr.rc.bottom = points[1].Y;
                fr.rcPage.left = points[0].X;
                fr.rcPage.top = points[0].Y;
                fr.rcPage.right = points[1].X;
                fr.rcPage.bottom = points[1].Y;
                fr.chrg.cpMin = 0;
                fr.chrg.cpMax = -1;
                IntPtr lpar = Marshal.AllocCoTaskMem(Marshal.SizeOf(fr));
                Marshal.StructureToPtr(fr, lpar, false);
                MetaBuilder.Core.SafeNativeMethods.SendMessageW(box.Handle, EM_FORMATRANGE, new IntPtr(0), new IntPtr(0)); // free up cached data
                Marshal.FreeCoTaskMem(lpar);
                g.ReleaseHdc(imgdc);
            }
            else
            {
                GetImage();
                g.DrawImage(myImage, drect);
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


        // Private Methods (5) 

        private static RichTextBox GetRichTextBox()
        {
            if (myRichTextBox == null)
            {
                myRichTextBox = new RichTextBox();
                myRichTextBox.BorderStyle = BorderStyle.None;
                myRichTextBox.ScrollBars = RichTextBoxScrollBars.None;
            }
            return myRichTextBox;
        }

        private SizeF Measure()
        {
            RectangleF drect = Bounds;
            int width = Math.Max((int)Math.Ceiling(drect.Width), 10);
            int height = 10; // doesn't matter?
            RichTextBox box = GetRichTextBox();
            box.Size = new Size(width, height);
            box.Rtf = Rtf;
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
            FORMATRANGE fr2 = (FORMATRANGE)Marshal.PtrToStructure(lpar, typeof(FORMATRANGE));
            float newheight = (fr2.rc.bottom - fr2.rc.top) * dpiy / 1440;
            if (newheight > 999999)
                newheight = 20;
            SizeF size = new SizeF(width, newheight);
            MetaBuilder.Core.SafeNativeMethods.SendMessageW(box.Handle, EM_FORMATRANGE, new IntPtr(0), new IntPtr(0)); // free up cached data
            Marshal.FreeCoTaskMem(lpar);
            g.ReleaseHdc(imgdc);
            g.Dispose();
            return size;
        }

        private void ResetImage()
        {
            myImage = null;
        }

        private void UpdateSize()
        {
            SizeF newsize = Measure();
            SetSizeKeepingLocation(newsize);
        }

        // Internal Methods (1) 

        internal Bitmap GetImage()
        {
            if (myImage == null)
            {
                RectangleF drect = Bounds;
                int width = Math.Max((int)Math.Ceiling(drect.Width), 10);
                int height = Math.Max((int)Math.Ceiling(drect.Height), 10);
                RichTextBox box = GetRichTextBox();
                box.Size = new Size(width, height);
                box.Rtf = Rtf;
                box.BackColor = BackgroundColor;
                myImage = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(myImage);
                g.FillRectangle(new SolidBrush(BackgroundColor), 0, 0, width, height);
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
                MetaBuilder.Core.SafeNativeMethods.SendMessageW(box.Handle, EM_FORMATRANGE, new IntPtr(0), new IntPtr(0)); // free up cached data
                Marshal.FreeCoTaskMem(lpar);
                g.ReleaseHdc(imgdc);
                g.Dispose();
            }
            return myImage;
        }

        #endregion Methods

        #region Nested Classes (1)

        private class RichTextBoxControl : RichTextBox, IGoControlObject
        {
            #region Fields (2)

            // TextBoxControl state
            private GoControl myGoControl;
            private GoView myGoView;

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
                            RichTextLabel gorichtext = value.EditedObject as RichTextLabel;
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
                get { return myGoView; }
                set
                {
                    myGoView = value;
                    ZoomFactor = myGoView.DocScale;
                }
            }

            #endregion Properties

            #region Methods (2)

            // Protected Methods (2) 

            protected override void OnLeave(EventArgs e)
            {
                GoControl ctrl = GoControl;
                if (ctrl != null)
                {
                    RichTextLabel gotext = ctrl.EditedObject as RichTextLabel;
                    if (gotext != null)
                    {
                        gotext.Rtf = Rtf;
                    }
                    ctrl.DoEndEdit(GoView);
                }
                base.OnLeave(e);
            }

            [UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
            protected override bool ProcessDialogKey(Keys keyData)
            {
                if (keyData == Keys.Escape)
                {
                    //GoControl ctrl = GoControl;
                    if (GoControl != null)
                        GoControl.DoEndEdit(GoView);
                    GoView.Focus();
                    return true;
                }
                else if (keyData == Keys.Tab)
                {
                    //GoControl ctrl = GoControl;
                    if (GoControl != null)
                    {
                        RichTextLabel gotext = GoControl.EditedObject as RichTextLabel;
                        if (gotext != null)
                        {
                            gotext.Rtf = Rtf;
                        }
                        GoControl.DoEndEdit(GoView);
                        GoView.Focus();
                    }
                    return true;
                }
                return base.ProcessDialogKey(keyData);
            }

            #endregion Methods
        }

        #endregion Nested Classes

        #region IDisposable Members

        public virtual void Dispose()
        {
            myImage.Dispose();
            myImage = null;
            GC.SuppressFinalize(this);
        }

        #endregion
    }

    public class GeneratorRichText : GoSvgGenerator
    {
        #region Constructors (1)

        public GeneratorRichText()
        {
            TransformerType = typeof(RichTextLabel);
        }

        #endregion Constructors

        #region Methods (2)

        // Public Methods (2) 

        public override void GenerateBody(Object obj)
        {
            RichTextLabel rt = (RichTextLabel)obj;
            base.GenerateBody(obj);
            Image image = rt.GetImage();
            if (image != null && Writer.FindShared(image) != null)
            {
                String id = Writer.FindShared(image);
                RectangleF r = rt.Bounds;
                WriteStartElement("use");
                float xscale = r.Width / image.Width; // already checked to be non-zero
                float yscale = r.Height / image.Height;
                WriteAttrVal("transform", "translate(" + r.X.ToString(CultureInfo.InvariantCulture) + "," + r.Y.ToString(CultureInfo.InvariantCulture) + ") scale(" + xscale.ToString(CultureInfo.InvariantCulture) + "," + yscale.ToString(CultureInfo.InvariantCulture) + ")");
                if (id != null)
                    WriteAttrVal("xlink:href", "#S" + id);
                WriteEndElement();
            }
        }

        public override void GenerateDefinitions(Object obj)
        {
            base.GenerateDefinitions(obj);
            RichTextLabel rt = (RichTextLabel)obj;
            Image image = rt.GetImage();
            if (image != null && image.Width > 0 && image.Height > 0)
                Writer.DefineObject(image);
        }

        #endregion Methods
    }
}