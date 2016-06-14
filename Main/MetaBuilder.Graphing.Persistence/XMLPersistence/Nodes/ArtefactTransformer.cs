using System;
using System.Text.RegularExpressions;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Meta;
using System.Drawing;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Nodes
{
    public class ArtefactTransformer : BaseGoObjectTransformer
    {

        #region Fields (1)

        private string text;

        #endregion Fields

        #region Constructors (1)

        public ArtefactTransformer()
            : base()
        {
            this.BodyConsumesChildElements = true;
            this.IdAttributeUsedForSharedObjects = true;
            this.TransformerType = typeof(ArtefactNode);
            this.ElementName = "artefact";
        }

        #endregion Constructors

        #region Properties (1)

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        #endregion Properties

        #region Methods (6)

        // Public Methods (5) 

        public override object Allocate()
        {
            ArtefactNode retval = new ArtefactNode();
            return retval;
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
        }

        public override void ConsumeChild(object parent, object child)
        {
            ArtefactNode node = parent as ArtefactNode;

            if (child is MetaBase)
            {
                node.BindingInfo = new BindingInfo();
                node.BindingInfo.BindingClass = (child as MetaBase)._ClassName;
                MetaBase mb = child as MetaBase;
                node.MetaObject = (child as MetaBase);
                node.HookupEvents();

                string defaultString = (mb.ToString() != null) ? mb.ToString() : mb._ClassName;
                node.Label.AutoResizes = true;
                node.Text = defaultString;

                if (mb.ToString() != null)
                {
                    if (mb.ToString().Trim() == "")
                    {
                        if ((mb._ClassName == "FlowDescription") && (Text != null))
                        {
                            TryParse(mb, Text);
                        }
                    }
                }
                node.Label.Alignment = Int32Attr("align", 1);
                base.ConsumeChild(parent, child);
            }
        }

        public override void ConsumeObjectFinish(object obj)
        {
            ArtefactNode node = obj as ArtefactNode;
            RectangleF xy = RectangleFAttr("xy", new RectangleF());
            node.Bounds = xy;
            node.Label.Bounds = new RectangleF(xy.X + 10, xy.Y + 10, xy.Width - 20f, xy.Height - 20);
            node.Label.Wrapping = true;
            node.Label.Height++;
            node.Label.Alignment = Int32Attr("align", 1);
            base.ConsumeObjectFinish(obj);

            //June 4 2013
            if (!string.IsNullOrEmpty(StringAttr("txtxy", "")))
                node.Label.Bounds = RectangleFAttr("txtxy", new RectangleF());
            //June 4 2013
            if (!string.IsNullOrEmpty(StringAttr("fontname", "")))
            {
                node.Label.FamilyName = StringAttr("fontname", node.Label.FamilyName);
                node.Label.TextColor = ColorAttr("fontcolor", Color.Black);
                node.Label.FontSize = FloatAttr("fontsize", node.Label.FontSize);
                node.Label.Bold = BooleanAttr("fontbold", node.Label.Bold);
                node.Label.Italic = BooleanAttr("fontitalic", node.Label.Italic);
                node.Label.Underline = BooleanAttr("fontunderline", node.Label.Underline);
                node.Label.Underline = BooleanAttr("fontunderline", node.Label.Underline);
            }
        }

        public override void GenerateAttributes(object obj)
        {
            base.GenerateAttributes(obj, true);
            ArtefactNode artnode = obj as ArtefactNode;
            if (artnode.Label != null)
            {
                WriteAttrVal("txtxy", artnode.Label.Bounds);
                WriteAttrVal("align", artnode.Label.Alignment);
                //June 4 2013
                WriteAttrVal("fontname", artnode.Label.FamilyName);
                WriteAttrVal("fontcolor", artnode.Label.TextColor);
                WriteAttrVal("fontsize", artnode.Label.FontSize);
                WriteAttrVal("fontbold", artnode.Label.Bold);
                WriteAttrVal("fontitalic", artnode.Label.Italic);
                WriteAttrVal("fontunderline", artnode.Label.Underline);
            }
            else
                WriteAttrVal("txtxy", artnode.Bounds);
        }

        // Private Methods (1) 

        const string regex = "^?(?<Number>\\d*)?(\\ \\?(?<Conditional>.*)\\ \\-\\ )?(?<Description>.*)$";

        private void TryParse(MetaBase mb, string textToParse)
        {
            System.Text.RegularExpressions.RegexOptions options = ((System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace | System.Text.RegularExpressions.RegexOptions.Multiline) | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(regex, options);
            MatchCollection matches = reg.Matches(Text);
            if (matches.Count > 0)
            {
                foreach (Match m in matches)
                {
                    if (m.ToString() != "")
                    {
                        string number = m.Groups[2].Value;
                        string condition = m.Groups[3].Value;
                        string description = m.Groups[4].Value;
                        if (number.Length > 0)
                        {
                            mb.Set("Sequence", number);
                        }

                        if (condition.Length > 0)
                        {
                            mb.Set("Condition", condition);
                        }

                        if (description.Length > 0)
                        {
                            mb.Set("Description", description);
                        }
                    }
                }
            }
        }

        #endregion Methods

    }
}