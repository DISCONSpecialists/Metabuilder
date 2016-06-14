using System;
using System.Collections;
using System.Drawing;
using System.Text;
using MetaBuilder.Graphing.Persistence.XMLPersistence.Primitives;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;
using TraceTool;
using System.Reflection;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Nodes
{
    public class BoundLabelTransformer : BaseGoObjectTransformer
    {

        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="BoundLabelTransformer"/> class.
        /// </summary>
        public BoundLabelTransformer()
            : base()
        {
            this.TransformerType = typeof(BoundLabel);
            this.ElementName = "lbl";
        }

        #endregion Constructors

        #region Methods (4)

        // Public Methods (4) 

        /// <summary>
        /// Allocates this instance.
        /// </summary>
        /// <returns></returns>
        public override object Allocate()
        {
            BoundLabel lbl = new BoundLabel();
            return lbl;
        }

        /// <summary>
        /// Consumes the attributes.
        /// </summary>
        /// <param name="obj">The obj.</param>
        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);
            BoundLabel lbl = obj as BoundLabel;
            lbl.Name = StringAttr("name", "");

            ConsumeSharedAttributes(obj);
        }

        const bool isSoftware = false;
        private const char splitChar = '|';
        public void ConsumeSharedAttributes(object obj)
        {
            BoundLabel lbl = obj as BoundLabel;

            lbl.FontSize = FloatAttr("fontSz", 10);
            lbl.AutoResizes = BooleanAttr("autoresize", true);
            lbl.Text = StringAttr("txt", "");
            lbl.Alignment = Int32Attr("align", 0);
            lbl.Bold = BooleanAttr("bold", false);
            lbl.Bordered = BooleanAttr("border", false);
            lbl.Clipping = BooleanAttr("clip", true);
            lbl.DropDownList = BooleanAttr("dropdown", false);
            lbl.Italic = BooleanAttr("ita", false);
            lbl.Maximum = Int32Attr("max", lbl.Maximum);
            //// Console.WriteLine("lblMax:" + lbl.Maximum.ToString());
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
                lbl.WrappingWidth = FloatAttr("wrapw", lbl.Width - 2f);
            lbl.Editable = BooleanAttr("lblEdit", true);
            lbl.originalEditable = lbl.Editable;

            bool isAlignedAndGrey = (lbl.TextColor.ToString() == "Name=ff808080, ARGB=(255, 128, 128, 128)}") && (lbl.Alignment == 4);
            bool isSmallAndGrey = (lbl.FontSize == 7 && (lbl.TextColor.ToString() == "Name=ff808080, ARGB=(255, 128, 128, 128)}"));

            if (isAlignedAndGrey || isSmallAndGrey || isSoftware) // manual fix for donners using old (beta) shapes
            {
                lbl.Editable = false;
                lbl.originalEditable = false;
                lbl.FontSize = 7;
                lbl.Text = lbl.Text.Replace(" ", "");
            }

            Type t = typeof(GoTextEditorStyle);
            string editorStyle = StringAttr("edstyle", GoTextEditorStyle.TextBox.ToString());
            lbl.EditorStyle = (GoTextEditorStyle)Enum.Parse(t, editorStyle);

            if (lbl.DropDownList)
            {
                lbl.Choices = new ArrayList();
                string s = StringAttr("Choices", "");
                string[] choices = s.Split(splitChar);
                for (int i = 0; i < choices.Length - 1; i++)
                {
                    lbl.Choices.Add(choices[i]);
                }
            }

            lbl.ddf = Int32Attr("ddf", 0);
        }

        /// <summary>
        /// Generates the attributes.
        /// </summary>
        /// <param name="obj">The obj.</param>
        public override void GenerateAttributes(Object obj)
        {
            base.GenerateAttributes(obj, true);
            //GenerateSharedAttributes(obj);
            BoundLabel lbl = obj as BoundLabel;
            WriteAttrVal("name", lbl.Name);
            WriteAttrVal("ddf", lbl.ddf);
        }

        public override void ConsumeObjectFinish(object obj)
        {
            base.ConsumeObjectFinish(obj);

            //Dynamically load choices based on value if found //else text
            BoundLabel o = obj as BoundLabel;
            if (o.Name.Contains("Def_"))
            {
                string text = o.Text;
                string name = o.Name.Replace("Def_", "");
                if (DataAccessLayer.DataRepository.Connections.ContainsKey(Core.Variables.Instance.ClientProvider))
                {
                    try
                    {
                        if (name == "type" || name == "NoDefYet") //Skips broken peripheral debugging is annoying with constant stops due to missing information
                        {
                            o.EditorStyle = GoTextEditorStyle.TextBox;
                            return;
                        }
                        else if (name == "ArchitecturalPriority") //def look but not def
                        {
                            o.EditorStyle = GoTextEditorStyle.TextBox;
                        }
                        else
                        {
                            o.Choices.Clear();

                            Type possibleValues = Activator.CreateInstanceFrom(Core.Variables.Instance.MetaSortAssembly.Replace("Sorters", "Enums"), Core.Variables.MetaNameSpace + "." + name).Unwrap().GetType();
                            if (possibleValues != null)
                            {
                                o.Choices.Add(""); //add one to deselect

                                FieldInfo[] fields = possibleValues.GetFields();

                                foreach (FieldInfo field in fields)
                                {
                                    if (field.Name.Equals("value__"))
                                        continue;
                                    o.EditorStyle = GoTextEditorStyle.ComboBox;
                                    o.Choices.Add(field.Name);
                                }

                                possibleValues = null;
                                //GC.Collect(); //THIS IS NOT RECOMMENDED
                            }
                            else
                            {
                                Core.Log.WriteLog("Loading definitions manually for domain " + name);
                                BusinessLogic.DomainDefinition def = DataAccessLayer.DataRepository.Domains(Core.Variables.Instance.ClientProvider).Find(BusinessLogic.DomainDefinitionColumn.Name, name);

                                if (def != null)
                                    o.Choices.Add(""); //add one to deselect

                                foreach (BusinessLogic.DomainDefinitionPossibleValue val in DataAccessLayer.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.DomainDefinitionPossibleValueProvider.GetByDomainDefinitionID(def.pkid))
                                {
                                    if (val.IsActive == true)
                                    {
                                        o.EditorStyle = GoTextEditorStyle.ComboBox;
                                        o.Choices.Add(val.PossibleValue);
                                    }
                                }
                                def.Dispose();
                            }
                        }
                    }
                    catch
                    {
                        //o.EditorStyle = GoTextEditorStyle.TextBox;
                    }
                }

                if (o.ddf == 0)
                {
                    //if (name == "EnvironmentIndicator" || name == "EnvironmentCategory")
                    //{
                    //    o.Font = new Font(o.Font.FontFamily, 10f);
                    //    o.TextColor = Color.Black;
                    //}
                    //else
                    //{
                    if (name.ToLower() == "subsetindicatortype")
                    {
                        o.Font = new Font(o.Font.FontFamily, 10f);
                        o.TextColor = Color.Black;
                    }
                    else
                    {
                        o.Font = new Font(o.Font.FontFamily, 7f);
                        o.TextColor = Color.FromArgb(-15132304);
                    }
                    //}
                }
                o.Text = text;
            }
        }

        #endregion Methods

    }
}