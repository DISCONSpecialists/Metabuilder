#region Copyright © 2007 - DISCON Specialists

//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: UniversalDropdownEditor.cs
//

#endregion

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Collections.Generic;

namespace MetaBuilder.Meta.Editors
{
    public class CustomListBox : ListBox
    {
        public CustomListBox()
            : base()
        {
            itemHeightAdjusted = this.ItemHeight;
            fontSize = 8;
            this.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            //tell windows we are interested in drawing items in ListBox on our own
            this.DrawItem += new DrawItemEventHandler(this.DrawItemHandler);
            //tell windows we are interested in providing  item size
            this.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.MeasureItemHandler);
        }

        public void MeasureMinimumHeight()
        {
            foreach (DataRowView drvItem in this.Items)
            {

                int descLength = drvItem["Description"].ToString().Length;
                if (descLength > 30)
                {
                    // itemHeightAdjusted = 50;
                    fontSize = 7.5f;
                }
            }
        }

        [Obsolete]
        protected override void AddItemsCore(object[] value)
        {
            base.AddItemsCore(value);
        }
        protected override void SetItemsCore(IList value)
        {
            base.SetItemsCore(value);
        }

        public float fontSize;

        private void DrawItemHandler(object sender, DrawItemEventArgs e)
        {
            if (Items.Count == 0)
                return;
            e.DrawBackground();
            e.DrawFocusRectangle();
            DataRowView drv = Items[e.Index] as DataRowView;
            Color penColor = Color.Black;

            string s = drv["Description"].ToString();
            SizeF origBounds = e.Bounds.Size;
            Font defaultFont = new Font(FontFamily.GenericSansSerif, fontSize, FontStyle.Regular);
            e.Graphics.DrawLine(new Pen(Color.LightBlue), e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
            e.Graphics.DrawString(drv["Description"].ToString(), defaultFont, new SolidBrush(penColor), e.Bounds);
        }

        private int itemHeightAdjusted;
        private void MeasureItemHandler(object sender, MeasureItemEventArgs e)
        {
            DataRowView drv = Items[e.Index] as DataRowView;
            string measureString = drv["Description"].ToString();
            Font stringFont = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Regular);
            Graphics g = this.CreateGraphics();
            SizeF layoutSize = new SizeF(this.Width, 500.0F);

            // Measure string.
            SizeF stringSize = new SizeF();
            stringSize = g.MeasureString(measureString, stringFont, layoutSize);
            // Draw rectangle representing size of string.
            e.ItemHeight = Convert.ToInt32(stringSize.Height);
        }

        /* private void StoreMinimumHeight()
         {
             float w = this.Width;
             float xMuliplier = 12f;
             Graphics g = this.CreateGraphics();

             foreach (DataRowView drv in this.Items)
             {
                 float stringWidth = g.MeasureString(drv["Description"].ToString(), new Font(FontFamily.GenericSansSerif, 8f)).Width;
                
             }
         }*/

    }
    public class UniversalDropdownEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if ((context != null) && (context.Instance != null))
            {
                return UITypeEditorEditStyle.DropDown;
            }
            return UITypeEditorEditStyle.None;
        }

        private IWindowsFormsEditorService edSvc;
        private ValueMemberAttribute valMemb;

        private CustomListBox List = null;
        private ITypeDescriptorContext Context = null;

        [RefreshProperties(RefreshProperties.All)]
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if ((context == null) || (provider == null) || context.Instance == null)
            {
                return base.EditValue(provider, value);
            }

            SourceCollectionAttribute att = (SourceCollectionAttribute)context.PropertyDescriptor.Attributes[typeof(SourceCollectionAttribute)];

            if (att == null)
            {
                return base.EditValue(provider, value);
            }
            this.edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (edSvc == null)
            {
                return base.EditValue(provider, value);
            }
            CustomListBox lst = new CustomListBox();
            this.PrepareListBox(lst, att, context);
            lst.MeasureMinimumHeight();

            //Filter by typing
            List = lst;
            Context = context;
            filter = "";

            if (valMemb == null)
            {
                lst.SelectedItem = value;
            }
            else
            {
                this.valMemb.SelectByValue(lst, value);
            }

            this.edSvc.DropDownControl(lst);
            //lst.SelectedIndexChanged += new EventHandler(lst_SelectedIndexChanged);

            if (lst.SelectedItem == null)
            {
                value = null;
            }
            else if (valMemb == null)
            {
                value = lst.SelectedItem;
            }
            else
            {
                value = valMemb.GetValue(lst.SelectedItem);
            }
            return value;
        }

        private void PrepareListBox(ListBox lst, SourceCollectionAttribute att, ITypeDescriptorContext context)
        {
            lst.IntegralHeight = false;
            ICollection coll = att.Collection(context.Instance);
            if (lst.ItemHeight > 0)
            {
                if ((coll != null) && (lst.Height / lst.ItemHeight < coll.Count))
                {
                    int adjHei = coll.Count * lst.ItemHeight;
                    if (adjHei > 200)
                    {
                        adjHei = 200;
                    }
                    lst.Height = adjHei;
                }
            }
            else
            {
                lst.Height = 200;
            }
            lst.Sorted = false;
            FillListBoxFromCollection(lst, coll);

            this.AssignValueMember(lst, context.PropertyDescriptor);
            this.AssignDisplayMember(lst, context.PropertyDescriptor);

            lst.TabStop = false;
            lst.SelectedIndexChanged += new EventHandler(this.handleSelection);
            lst.KeyDown += new KeyEventHandler(OnKeyDown);
            lst.DoubleClick += new EventHandler(lst_DoubleClick);

            lst.LostFocus += new EventHandler(lst_LostFocus);
        }

        void lst_LostFocus(object sender, EventArgs e)
        {
            filter = "";
        }

        void lst_DoubleClick(object sender, EventArgs e)
        {
            if (edSvc == null || List == null || List.SelectedItem == null)
            {
                return;
            }
            edSvc.CloseDropDown();
        }

        private void AssignValueMember(ListControl lc, PropertyDescriptor pd)
        {
            this.valMemb = (ValueMemberAttribute)pd.Attributes[typeof(ValueMemberAttribute)];
            if (valMemb == null)
            {
                return;
            }
            lc.ValueMember = this.valMemb.ValuePropertyName;
        }

        private void AssignDisplayMember(ListControl lc, PropertyDescriptor pd)
        {
            DisplayMemberAttribute att = (DisplayMemberAttribute)pd.Attributes[typeof(DisplayMemberAttribute)];
            if (att == null)
            {
                return;
            }
            lc.DisplayMember = att.DisplayPropertyName;
        }

        private void handleSelection(object sender, EventArgs e)
        {
            if (edSvc == null)
            {
                return;
            }
            //when you press e and there are 2 values that start with e, the first is chosen and list is closed
            //edSvc.CloseDropDown();
        }

        public void FillListBoxFromCollection(ListBox lb, ICollection coll)
        {
            lb.BeginUpdate();
            lb.Items.Clear();
            if (coll is DataView)
                (coll as DataView).Sort = "Series ASC";
            foreach (object item in coll)
            {
                if (filter.Length > 0)
                {
                    if ((item as DataRowView)[1].ToString().ToLower().Contains(filter))
                        lb.Items.Add(item);
                }
                else
                {
                    lb.Items.Add(item);
                }
            }
            lb.EndUpdate();
            lb.Invalidate();
        }

        string filter = "";
        protected void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (List == null)
                return;
            SourceCollectionAttribute att = (SourceCollectionAttribute)Context.PropertyDescriptor.Attributes[typeof(SourceCollectionAttribute)];
            ICollection coll = att.Collection(Context.Instance);
            string key = e.KeyCode.ToString();
            if (key.Length > 1 || !char.IsLetter(char.Parse(key))) //is not a alphabetletter
                filter = "";
            else
                filter += key.ToLower();

            FillListBoxFromCollection(List, coll);
            drawFilter();

            e.Handled = true;
        }
        Label filterLabel;
        private void drawFilter()
        {
            if (filter.Length == 0)
            {
                if (filterLabel != null)
                    List.Controls.Remove(filterLabel);
                filterLabel = null;
                return;
            }
            if (filterLabel == null)
            {
                filterLabel = new Label();
                filterLabel.AutoEllipsis = true;
                filterLabel.Width = List.Width;
                filterLabel.Font = new Font(filterLabel.Font.FontFamily, 8f);
                filterLabel.Dock = DockStyle.Bottom;

                filterLabel.BackColor = Color.AliceBlue;
                filterLabel.ForeColor = Color.Black;

                List.Controls.Add(filterLabel);
            }
            filterLabel.Text = filter;
        }
    }
}