using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Northwoods.Go;
using System.Drawing;

namespace MetaBuilder.Graphing.Shapes.Binding.Intellisense
{
    public class CustomComboBox : ComboBox, IGoControlObject
    {
        public CustomComboBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();

            string text = Items[e.Index].ToString();

            Font defaultFont = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Regular);
            e.Graphics.DrawLine(new Pen(Color.LightBlue), e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
            e.Graphics.DrawString(Items[e.Index].ToString(), defaultFont, new SolidBrush(Color.Black), e.Bounds);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            GoText gotext = GoControl.EditedObject as GoText;
            if (e.KeyCode == Keys.Enter)
            {
                if (gotext != null && Text != null)
                {
                    foreach (string i in Items)
                    {
                        if (i.ToLower() == Text)
                        {
                            Text = i; //dont cAmElcAse! existing items
                            break;
                        }
                    }
                    gotext.DoEdit(GoView, gotext.Text, Text);
                    Dispose(Disposing);
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Dispose(Disposing);
                return;
            }

            base.OnKeyDown(e);
        }

        protected override void OnDropDownClosed(EventArgs e)
        {
            base.OnDropDownClosed(e);
            GoText gotext = GoControl.EditedObject as GoText;
            if (gotext != null && SelectedItem != null)
            {
                gotext.DoEdit(GoView, gotext.Text, SelectedItem.ToString());
            }

            GoControl.DoEndEdit(GoView);
        }

        protected override void OnSelectedItemChanged(EventArgs e)
        {
            base.OnSelectedItemChanged(e);
        }

        protected override void Dispose(bool disposing)
        {
            GoControl.DoEndEdit(GoView);
            base.Dispose(disposing);
        }

        #region IGoControlObject Members

        private GoControl myGoControl;
        private GoView myGoView;

        private bool IsDefinition
        {
            get
            {
                if (myGoControl != null && myGoControl.EditedObject != null)
                    if (myGoControl.EditedObject is BoundLabel)
                        return (myGoControl.EditedObject as BoundLabel).Name.Contains("Def");

                return false;
            }
        }
        public GoControl GoControl
        {
            get { return myGoControl; }
            set
            {
                myGoControl = value;
                if (myGoControl.EditedObject is BoundLabel)
                {
                    foreach (string s in (myGoControl.EditedObject as BoundLabel).Choices)
                        this.Items.Add(s);
                    this.Text = (myGoControl.EditedObject as BoundLabel).Text;
                }

                this.Sorted = false;
                this.Font = new Font(Font.FontFamily, 8);
                this.AutoCompleteMode = AutoCompleteMode.Suggest;
                this.AutoCompleteSource = AutoCompleteSource.ListItems;
                this.DropDownWidth = GetMaxItemWidth(this);
                //this.Width = this.DropDownWidth;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (Width < this.DropDownWidth)
                Width = this.DropDownWidth;
        }

        public int GetMaxItemWidth(ComboBox myCombo)
        {
            int maxWidth = 0, temp = 0;
            foreach (string obj in myCombo.Items)
            {
                temp = TextRenderer.MeasureText(obj.ToString(), myCombo.Font).Width;
                if (temp > maxWidth)
                {
                    maxWidth = temp;
                }
            }
            return maxWidth + 20;
        }

        public GoView GoView
        {
            get { return myGoView; }
            set
            {
                myGoView = value;
            }
        }

        #endregion
    }
}
