#region Copyright © 2007 - DISCON Specialists

//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: LongText.cs
//

#endregion

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using MetaBuilder.Meta.Editors;

namespace MetaBuilder.Meta
{
    /// <summary>
    /// Summary description for LongText.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(CustomThingTypeConverter))]
    [Editor(typeof(RichTextEditor), typeof(UITypeEditor))]
    public class LongText
    {
        public override string ToString()
        {
            return Text;
            //string t = Text;

            //return t.Replace("\n", " ").Replace("\r", "");
        }

        public LongText()
        {
        }
        public string RTFText
        {
            get
            {
                RichTextBox rtb = new RichTextBox();
                rtb.Rtf = rtf;
                return rtb.Text;
            }
        }
        public string Text
        {
            get
            {
                return rtf;

            }
            set
            {
                rtf = value;
            }
        }

        private string rtf;

        [Browsable(false)]
        public string RTF
        {
            get { return rtf; }
            set { rtf = value; }
        }

        //public static implicit operator string(LongText lt)
        //{
        //    return lt.ToString();
        //}
    }

    public class CustomThingTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                LongText lt = new LongText();
                lt.Text = Convert.ToString(value);
                return lt;
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (value == null)
                return null;
            if (destinationType == typeof(string))
                return ((LongText)value).ToString();

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    //public class complex : MetaBase
    //{
    //    public complex()
    //    {
    //        MyLongText = new LongText();
    //    }

    //    public LongText MyLongText
    //    {
    //        get { return mylongtext; }
    //        set { mylongtext = value; }
    //    }

    //    private LongText mylongtext;

    //}
}