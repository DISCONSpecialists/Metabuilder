using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Nodes;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Groups
{
    public class ShapeCollapsibleHandleTransformer : BaseGoObjectTransformer
    {

        #region Constructors (1)

        public ShapeCollapsibleHandleTransformer()
            : base()
        {
            this.TransformerType = typeof(ShapeCollapsibleHandle);
            this.ElementName = "shapeCollapsibleHandle";

        }

        #endregion Constructors

        #region Methods (3)


        // Public Methods (3) 

        public override object Allocate()
        {
            ShapeCollapsibleHandle retval = new ShapeCollapsibleHandle();
            return retval;
        }

        private const char splitChar = '|';
        public override void ConsumeAttributes(object obj)
        {
            ShapeCollapsibleHandle handle = obj as ShapeCollapsibleHandle;
            string handleStyle = StringAttr("HandleStyle", GoCollapsibleHandleStyle.PlusMinus.ToString());
            handle.Style = (GoCollapsibleHandleStyle)Enum.Parse(typeof(GoCollapsibleHandleStyle), handleStyle);
            handle.Collapsibles = new List<string>();
            string collapsibles = StringAttr("Collapsibles", "");
            string[] collapsiblesArray = collapsibles.Split(splitChar);
            for (int i = 0; i < collapsiblesArray.Length; i++)
            {
                handle.Collapsibles.Add(collapsiblesArray[i]);
            }
            base.ConsumeAttributes(obj);
        }

        public override void GenerateAttributes(Object obj)
        {
            base.GenerateAttributes(obj, true);
            ShapeCollapsibleHandle handle = obj as ShapeCollapsibleHandle;
            WriteAttrVal("HandleStyle", handle.Style.ToString());

            StringBuilder sbCollapsibles = new StringBuilder();
            foreach (string s in handle.Collapsibles)
            {
                sbCollapsibles.Append(s).Append("|");
            }

            WriteAttrVal("Collapsibles", sbCollapsibles.ToString().Substring(0, sbCollapsibles.Length - 1));

        }


        #endregion Methods

    }

}
