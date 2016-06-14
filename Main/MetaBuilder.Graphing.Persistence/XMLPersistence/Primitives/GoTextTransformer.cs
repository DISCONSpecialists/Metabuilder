using System;
using System.Collections;
using System.Drawing;
using System.Text;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Primitives
{

    public class GoTextTransformer : BaseGoObjectTransformer
    {

        #region Constructors (1)

        public GoTextTransformer()
            : base()
        {
            this.TransformerType = typeof(GoText);
            this.ElementName = "txt";
            this.IdAttributeUsedForSharedObjects = true;
        }

        #endregion Constructors

        #region Methods (5)

        // Public Methods (5) 

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            //4 June 2013
            (obj as GoText).DragsNode = true;
            ConsumeSharedAttributes(obj);

            if ((obj as GoText).Text == "~!DUMMY!~")
                (obj as GoText).Visible = false;
        }
        const char splitChar = '|';
        public void ConsumeSharedAttributes(object obj)
        {
            RectangleF bnds = RectangleFAttr("xy", new RectangleF(0, 0, 0, 0));
            if ((bnds.Width == 0) && (bnds.Height == 0))
            {
                obj = null;
                return;
            }

            GoText lbl = obj as GoText;
            lbl.FontSize = FloatAttr("fontSz", 10);
            lbl.AutoResizes = BooleanAttr("autoresize", false);
            lbl.Text = StringAttr("txt", "");
            lbl.Alignment = Int32Attr("align", 0);
            lbl.Bold = BooleanAttr("bold", false);
            lbl.Bordered = BooleanAttr("border", false);
            lbl.Clipping = BooleanAttr("clip", false);

            lbl.DropDownList = BooleanAttr("dropdown", false);
            lbl.Italic = BooleanAttr("ita", false);
            lbl.Maximum = Int32Attr("max", lbl.Maximum);
            lbl.Minimum = Int32Attr("min", lbl.Minimum);
            lbl.Multiline = BooleanAttr("multi", false);
            lbl.StrikeThrough = BooleanAttr("strike", false);
            Type tStringTrimming = typeof(StringTrimming);
            lbl.StringTrimming = (StringTrimming)Enum.Parse(tStringTrimming, StringAttr("trim", StringTrimming.None.ToString()));
            lbl.TextColor = ColorAttr("colour", Color.Black);
            lbl.Underline = BooleanAttr("ul", false);
            lbl.Wrapping = BooleanAttr("wrap", false);
            lbl.FamilyName = StringAttr("font", "Tahoma");
            if (lbl.Wrapping)
            {
                float wrappingWidth = FloatAttr("wrapw", 4f);
                lbl.WrappingWidth = wrappingWidth;
            }

            lbl.Selectable = true;
            lbl.Editable = BooleanAttr("lblEdit", true);

            //lbl.DragsNode = false;
            if (lbl.DropDownList)
            {
                lbl.Choices = new ArrayList();

                string s = StringAttr("Choices", "");
                string[] choices = s.Split(splitChar);
                for (int i = 0; i < choices.Length - 1; i++)
                {
                    lbl.Choices.Add(choices[i]);
                }
                Type t = typeof(GoTextEditorStyle);
                string editorStyle = StringAttr("edstyle", GoTextEditorStyle.TextBox.ToString());
                lbl.EditorStyle = (GoTextEditorStyle)Enum.Parse(t, editorStyle);
            }
        }

        public override void GenerateAttributes(object obj)
        {
            GoText txt = obj as GoText;
            if (txt.Maximum == 1999)
                return;
            GenerateSharedAttributes(obj);
            if (txt.Text != string.Empty && (!(obj is BoundLabel)))
            {
                base.GenerateAttributes(obj, true);
            }
        }

        /*public override object Allocate()
        {
            GoText obj = new GoText();
            return obj;
        }*/
        public override bool GenerateElement(object obj)
        {
            GoText txt = obj as GoText;
            // Removed first statement for empty labels that were causing tool to crash
            // if (txt.Text != string.Empty && (!(obj is BoundLabel))) 

            if (!(obj is BoundLabel))
                return base.GenerateElement(obj);
            return false;
        }

        public void GenerateSharedAttributes(object obj)
        {
            GoText lbl = obj as GoText;
            if (lbl.Width == 0)
            {
                obj = null;
                return;
            }
            WriteAttrVal("font", lbl.FamilyName);
            WriteAttrVal("txt", lbl.Text);
            WriteAttrVal("align", lbl.Alignment);

            if (lbl.Bold)
                WriteAttrVal("bold", lbl.Bold);

            if (lbl.Bordered)
                WriteAttrVal("border", lbl.Bordered);

            if (lbl.Clipping)
                WriteAttrVal("clip", lbl.Clipping);

            if (lbl.DropDownList)
            {
                WriteAttrVal("dropdown", lbl.DropDownList);
                StringBuilder sbChoices = new StringBuilder();
                foreach (string s in lbl.Choices)
                {
                    sbChoices.Append(s + "|");
                }
                WriteAttrVal("Choices", sbChoices.ToString());
            }


            if (lbl.Italic)
                WriteAttrVal("ita", lbl.Italic);

            WriteAttrVal("fontSz", lbl.FontSize);
            WriteAttrVal("max", lbl.Maximum);
            WriteAttrVal("min", lbl.Minimum);
            WriteAttrVal("lblEdit", lbl.Editable);

            if (lbl.Multiline)
                WriteAttrVal("multi", lbl.Multiline);

            if (lbl.StrikeThrough)
                WriteAttrVal("strike", lbl.StrikeThrough);

            WriteAttrVal("trim", lbl.StringTrimming.ToString());

            if (lbl.Underline)
                WriteAttrVal("ul", lbl.Underline);
            WriteAttrVal("autoresize", lbl.AutoResizes);
            if (lbl.Wrapping)
            {
                WriteAttrVal("wrap", lbl.Wrapping);
                WriteAttrVal("wrapw", lbl.WrappingWidth);
            }

            WriteAttrVal("edstyle", lbl.EditorStyle.ToString());
            WriteAttrVal("colour", lbl.TextColor);

        }

        public override object Allocate()
        {
            if (StringAttr("edstyle", "") == "")
                return null;
            return base.Allocate();
        }

        public override bool SkipGeneration(object obj)
        {

            return base.SkipGeneration(obj);
        }

        #endregion Methods

    }
}