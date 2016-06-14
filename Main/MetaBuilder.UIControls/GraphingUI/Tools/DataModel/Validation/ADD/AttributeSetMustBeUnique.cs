using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation.ADD
{
    internal class AttributeSetMustBeUnique : BaseRule2015
    {
        public AttributeSetMustBeUnique(string name, ExecutesOnEnum execsOn, RuleType type)
            : base(name, execsOn, type)
        {
            Description = "Two or more Entities can not have the same Attribute Set but different names.";
        }
        #region Implementation of Base Method
        public override void Apply()
        {
            foreach (ValidationItem vitem in AllItems)
            {
                foreach (ValidationItem otherItem in AllItems)
                {
                    if ((vitem.MyGoObject as IMetaNode).MetaObject == (otherItem.MyGoObject as IMetaNode).MetaObject)
                        continue;

                    if (GetOrderedTrimmedAttributeSet(vitem.MyGoObject as CollapsibleNode) == GetOrderedTrimmedAttributeSet(otherItem.MyGoObject as CollapsibleNode))
                    {
                        vitem.Result = ValidationResult.Error;
                        otherItem.Result = ValidationResult.Error;
                    }
                }
                if (vitem.Result == ValidationResult.None)
                    vitem.Result = ValidationResult.OK;
            }
        }

        #endregion

        private string GetOrderedTrimmedAttributeSet(CollapsibleNode Node)
        {
            string attributes = "";
            List<string> sortedAttributes = new List<string>();
            foreach (RepeaterSection rsec in Node.RepeaterSections)
            {
                foreach (GoObject obj in rsec)
                {
                    if (!(obj is CollapsingRecordNodeItem) || (obj as CollapsingRecordNodeItem).IsHeader)
                        continue;
                    sortedAttributes.Add((obj as CollapsingRecordNodeItem).MetaObject.ToString());
                }
            }

            sortedAttributes.Sort();
            foreach (string s in sortedAttributes)
                attributes += s;

            return attributes;
        }
    }
}
