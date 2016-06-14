using System;
using System.Drawing;
using System.Windows.Forms;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Shapes.Binding
{
    public class CustomTextBox : TextBox, IGoControlObject
    {
        private GoControl myGoControl;
        private GoView myGoView;

        public CustomTextBox()
        {
            BackColor = Color.AliceBlue;
        }

        #region IGoControlObject Members

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
                            if (GoView != null) fsize *= GoView.DocScale;
                            Font = new Font(objfont.Name, fsize, objfont.Style, GraphicsUnit.Point);
                        }
                    }
                }
            }
        }

        public GoView GoView
        {
            get { return myGoView; }
            set { myGoView = value; }
        }

        #endregion

        protected override bool ProcessDialogKey(Keys key)
        {
            if (HandleKey(key))
                return true;
            return base.ProcessDialogKey(key);
        }

        protected override void OnLeave(EventArgs evt)
        {
            AcceptText();
            base.OnLeave(evt);
        }

        private bool HandleKey(Keys key)
        {
            if (key == Keys.Escape)
            {
                GoControl ctrl = GoControl;
                if (ctrl != null)
                    ctrl.DoEndEdit(GoView);
                GoView.RequestFocus();
                return true;
            }
            
            if (key == Keys.Enter || key == Keys.Tab)
            {
                if (key == Keys.Enter && AcceptsReturn) // accept the return to start a new line
                    return false;
                AcceptText();
                GoView.RequestFocus();
                return true;
            }
            return false;
        }

        private void AcceptText()
        {
            Text = "HIERDIE IS CUSTOM BRU!";
            GoControl ctrl = GoControl;
            if (ctrl != null)
            {
                GoText gotext = ctrl.EditedObject as GoText;
                if (gotext != null)
                {
                    gotext.DoEdit(GoView, gotext.Text, Text);
                }
                ctrl.DoEndEdit(GoView);
            }
        }

        // CustomTextBox state
    }
}