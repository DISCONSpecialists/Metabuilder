using System;
using System.Drawing;
using System.Windows.Forms;
using MetaBuilder.Core;
using MetaBuilder.Graphing.Shapes.Binding.Intellisense;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Shapes
{

    [Serializable]
    public class ResizableComment : GoComment
    {
        public ResizableComment()
        {
            Label = new ExpandableTextBoxLabel();

            if (Background is GoShape)
            {
                (Background as GoShape).Brush = new SolidBrush(Color.FromArgb(230, 230, 250));
            }
        }

        #region Properties (1)

        public override bool Printable
        {
            get
            {
                if (Variables.Instance.IsViewer)
                    return true;
                return Variables.Instance.PrintComments;
            }
            set { base.Printable = value; }
        }

        #endregion Properties

        #region Methods (2)

        // Public Methods (2) 

        /// <summary>
        /// Let the user's resizing adjust the values of TopLeftMargin and/or BottomRightMargin.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="origRect"></param>
        /// <param name="newPoint"></param>
        /// <param name="whichHandle"></param>
        /// <param name="evttype"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public override void DoResize(GoView view, RectangleF origRect, PointF newPoint, int whichHandle, GoInputState evttype, SizeF min, SizeF max)
        {
            if (Background != null &&
                whichHandle == TopLeft || whichHandle == MiddleTop || whichHandle == TopRight ||
                whichHandle == MiddleLeft || whichHandle == MiddleRight ||
                whichHandle == BottomLeft || whichHandle == MiddleBottom || whichHandle == BottomRight)
            {
                SizeF oldtl = TopLeftMargin;
                SizeF oldbr = BottomRightMargin;
                RectangleF labr = Label.Bounds;
                RectangleF oldr = labr;
                oldr.X -= oldtl.Width;
                oldr.Y -= oldtl.Height;
                oldr.Width += oldtl.Width + oldbr.Width;
                oldr.Height += oldtl.Height + oldbr.Height;
                RectangleF newr = ComputeResize(oldr, newPoint, whichHandle, min, max, CanResize());
                TopLeftMargin = new SizeF(Math.Max(0, labr.Left - newr.Left), Math.Max(0, labr.Top - newr.Top));
                BottomRightMargin =
                    new SizeF(Math.Max(0, newr.Right - labr.Right), Math.Max(0, newr.Bottom - labr.Bottom));
            }
            else
            {
                base.DoResize(view, origRect, newPoint, whichHandle, evttype, min, max);
            }
        }

        public void UpdateSizeFromLabelBounds()
        {
            SizeF oldtl = TopLeftMargin;
            SizeF oldbr = BottomRightMargin;
            RectangleF labr = Label.Bounds;
            RectangleF oldr = labr;
            oldr.X -= oldtl.Width;
            oldr.Y -= oldtl.Height;
            oldr.Width += oldtl.Width + oldbr.Width;
            oldr.Height += oldtl.Height + oldbr.Height;
            RectangleF newr = ComputeResize(oldr, Position, BottomRight, new SizeF(10, 10), new SizeF(5000, 5000), CanResize());
            TopLeftMargin = new SizeF(Math.Max(0, labr.Left - newr.Left), Math.Max(0, labr.Top - newr.Top));
            BottomRightMargin = new SizeF(Math.Max(0, newr.Right - labr.Right), Math.Max(0, newr.Bottom - labr.Bottom));
        }

        #endregion Methods

        //public override GoControl CreateEditor(GoView view)
        //{
        //    GoControl editor = new GoControl();
        //    editor.ControlType = typeof(ExpandableIntellisenseBox);
        //    RectangleF r = Bounds;
        //    r.Inflate(4, 4); // a bit bigger than the original GoText object
        //    editor.Bounds = r;
        //    return editor;
        //}

        private bool viewerAdded;
        public bool ViewerAdded
        {
            get { return viewerAdded; }
            set
            {
                viewerAdded = value;
                if (value == true)
                {
                    if (Background is GoShape)
                    {
                        (Background as GoShape).Brush = new SolidBrush(Color.FromArgb(255, 192, 203));
                    }
                }
            }
        }

    }

    [Serializable]
    public class ResizableBalloonComment : GoBalloon
    {
        [NonSerialized]
        public Guid DropID;
        public Guid MyGUID;

        public GoObject AnchorShallow;

        public ResizableBalloonComment()
        {
            Label = new ExpandableTextBoxLabel();

            Label.Editable = false;
            MyGUID = Guid.NewGuid();

            if (!(this is MetaBuilder.Graphing.Shapes.Nodes.Rationale))
            {
                if (Background is GoShape)
                {
                    (Background as GoShape).Brush = new SolidBrush(Color.FromArgb(230, 230, 250));
                }
            }
        }

        //24 January 2013 cannot edit this without manually calling dobeginedit
        public override bool Editable
        {
            get
            {
                Label.Editable = false;
                return false;
            }
            set
            {
                base.Editable = false;
            }
        }

        //public override GoControl CreateEditor(GoView view)
        //{
        //    GoControl editor = new GoControl();
        //    editor.ControlType = typeof(ExpandableIntellisenseBox);
        //    RectangleF r = Bounds;
        //    r.Inflate(4, 4); // a bit bigger than the original GoText object
        //    editor.Bounds = r;
        //    return editor;
        //}

        #region Properties (1)

        public override bool Printable
        {
            get
            {
                if (Variables.Instance.IsViewer)
                    return true;
                return Variables.Instance.PrintComments;
            }
            set { base.Printable = value; }
        }

        public override bool Copyable
        {
            get { return true; }
            set { base.Copyable = value; }
        }

        #endregion Properties

        #region Methods (3)

        // Public Methods (2) 

        /// <summary>
        /// Let the user's resizing adjust the values of TopLeftMargin and/or BottomRightMargin.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="origRect"></param>
        /// <param name="newPoint"></param>
        /// <param name="whichHandle"></param>
        /// <param name="evttype"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public override void DoResize(GoView view, RectangleF origRect, PointF newPoint, int whichHandle, GoInputState evttype, SizeF min, SizeF max)
        {
            if (Background != null && whichHandle == TopLeft || whichHandle == MiddleTop || whichHandle == TopRight || whichHandle == MiddleLeft || whichHandle == MiddleRight || whichHandle == BottomLeft || whichHandle == MiddleBottom || whichHandle == BottomRight)
            {
                SizeF oldtl = TopLeftMargin;
                SizeF oldbr = BottomRightMargin;
                RectangleF labr = Label.Bounds;
                RectangleF oldr = labr;
                oldr.X -= oldtl.Width;
                oldr.Y -= oldtl.Height;
                oldr.Width += oldtl.Width + oldbr.Width;
                oldr.Height += oldtl.Height + oldbr.Height;
                RectangleF newr = ComputeResize(oldr, newPoint, whichHandle, min, max, CanResize());
                TopLeftMargin = new SizeF(Math.Max(0, labr.Left - newr.Left), Math.Max(0, labr.Top - newr.Top));
                BottomRightMargin = new SizeF(Math.Max(0, newr.Right - labr.Right), Math.Max(0, newr.Bottom - labr.Bottom));
            }
            else
            {
                base.DoResize(view, origRect, newPoint, whichHandle, evttype, min, max);
            }
        }

        public void UpdateSizeFromLabelBounds()
        {
            SizeF oldtl = TopLeftMargin;
            SizeF oldbr = BottomRightMargin;
            RectangleF labr = Label.Bounds;
            RectangleF oldr = labr;
            oldr.X -= oldtl.Width;
            oldr.Y -= oldtl.Height;
            oldr.Width += oldtl.Width + oldbr.Width;
            oldr.Height += oldtl.Height + oldbr.Height;
            RectangleF newr = ComputeResize(oldr, Position, BottomRight, new SizeF(10, 10), new SizeF(5000, 5000), CanResize());
            TopLeftMargin = new SizeF(Math.Max(0, labr.Left - newr.Left), Math.Max(0, labr.Top - newr.Top));
            BottomRightMargin = new SizeF(Math.Max(0, newr.Right - labr.Right), Math.Max(0, newr.Bottom - labr.Bottom));
        }

        // Protected Methods (1) 

        #endregion Methods

        //public override GoObject Anchor
        //{
        //    get
        //    {
        //        return base.Anchor;
        //    }
        //    set
        //    {
        //        if (value == null)
        //            Core.Log.WriteLog("Anchor getting set to null" + Environment.NewLine + "It was " + base.Anchor + " before." + Environment.NewLine + Environment.StackTrace);

        //        base.Anchor = value;
        //    }
        //}

        private bool viewerAdded;
        public bool ViewerAdded
        {
            get { return viewerAdded; }
            set
            {
                viewerAdded = value;
                if (value == true)
                {
                    if (!(this is MetaBuilder.Graphing.Shapes.Nodes.Rationale)) //a viewer rationale?
                    {
                        if (Background is GoShape)
                        {
                            (Background as GoShape).Brush = new SolidBrush(Color.FromArgb(255, 192, 203));
                        }
                    }
                }
            }
        }
    }

    [Serializable]
    public class ExpandableTextBox : TextBox, IGoControlObject
    {
        public ExpandableTextBox()
        {
            //AcceptsReturn = true;
            Multiline = true;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            //calculte length of text for width
            using (System.Drawing.Graphics graphics = this.CreateGraphics())
            {
                SizeF s = graphics.MeasureString(Text, Font);
                this.SetBounds(Bounds.X, Bounds.Y, (int)s.Width, (int)s.Height, BoundsSpecified.All);
            }
            base.OnTextChanged(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            AcceptText();
            base.OnLostFocus(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            AcceptText();
            base.OnLeave(e);
        }

        private void AcceptText()
        {
            (GoControl.EditedObject as GoText).DoEdit(GoView, (GoControl.EditedObject as GoText).Text, Text);
        }

        #region IGoControlObject Members

        [NonSerialized]
        private GoControl myGoControl;
        [NonSerialized]
        private GoView myGoView;

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
                        GoText gotext = value.EditedObject as GoText;
                        if (gotext != null)
                        {
                            // initialize the control according to the state of the GoText object
                            Text = gotext.Text;

                            switch (gotext.Alignment)
                            {
                                // default: <- not sure why there's a case statement if its going to default!?  
                                case GoObject.TopLeft:
                                case GoObject.MiddleLeft:
                                case GoObject.BottomLeft:
                                    TextAlign = HorizontalAlignment.Left;
                                    break;
                                case GoObject.TopCenter:
                                case GoObject.MiddleCenter:
                                case GoObject.BottomCenter:
                                    TextAlign = HorizontalAlignment.Center;
                                    break;
                                case GoObject.TopRight:
                                case GoObject.MiddleRight:
                                case GoObject.BottomRight:
                                    TextAlign = HorizontalAlignment.Right;
                                    break;

                            }

                            Multiline = gotext.Multiline || gotext.Wrapping;
                            AcceptsReturn = gotext.Multiline;
                            WordWrap = gotext.Wrapping;
                            Font objfont = gotext.Font;
                            float fsize = objfont.Size;
                            if (GoView != null)
                                fsize *= GoView.DocScale;
                            Font = new Font(objfont.Name, fsize, objfont.Style, GraphicsUnit.Point);

                        }
                    }
                }
            }
        }

        public GoView GoView
        {
            get
            {
                return myGoView;
            }
            set
            {
                myGoView = value;
            }
        }

        #endregion
    }

    [Serializable]
    public class ExpandableTextBoxLabel : GoText
    {
        public ExpandableTextBoxLabel()
        {
            Selectable = false;
            DragsNode = true;
            Multiline = true;
            Deletable = false;

            EditorStyle = GoTextEditorStyle.TextBox;
        }

        public override bool OnDoubleClick(GoInputEventArgs evt, GoView view)
        {
            if (this.ParentNode != null)
                if (Core.Variables.Instance.IsViewer && this.ParentNode is IMetaNode)
                    return true;

            if (Editable == false)
                this.ToString();
            DoBeginEdit(view);
            return true;
        }

        public override GoControl CreateEditor(GoView view)
        {
            //#if DEBUG
            GoControl editor = new GoControl();

            MetaBuilder.Graphing.Shapes.Binding.Intellisense.IntellisenseBox box = new MetaBuilder.Graphing.Shapes.Binding.Intellisense.IntellisenseBox();

            box.GoView = view;
            if (view.Selection.First is IMetaNode)
                box.PassedMetaObject = (view.Selection.First as IMetaNode).MetaObject;

            //if (view != null)
            //    MessageBox.Show(this,"The view was passed to here");

            editor.ControlType = typeof(MetaBuilder.Graphing.Shapes.Binding.Intellisense.IntellisenseBox);
            editor.Editable = true;
            RectangleF r = Bounds;
            // new Rectangle(new Point((int)editor.Location.X, (int)editor.Location.Y), new Size(500, 500));
            r.Inflate(10, 10); // a bit bigger than the original GoText object //this just moves it
            editor.Bounds = r;
            //editor.Size = new SizeF(editor.Size.Width + 100, editor.Size.Height + 140);
            return editor;
            //#endif
            return base.CreateEditor(view);
        }

        //public override GoControl CreateEditor(GoView view)
        //{
        //    GoControl editor = new GoControl();
        //    editor.ControlType = typeof(ExpandableTextBox);
        //    RectangleF r = Bounds;
        //    r.Inflate(4, 4); // a bit bigger than the original GoText object
        //    editor.Bounds = r;
        //    return editor;
        //}
    }
}