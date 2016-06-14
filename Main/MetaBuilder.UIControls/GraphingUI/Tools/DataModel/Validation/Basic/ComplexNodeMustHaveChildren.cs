using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation.Basic
{
    public class ComplexNodeMustHaveChildren : BaseRule2015
    {
        internal ComplexNodeMustHaveChildren(string name, ExecutesOnEnum execsOn, RuleType type)
            : base(name, execsOn, type)
        {
            Description = "Complex node must have child";
        }

        #region Implementation of Base Method
        public override void Apply()
        {
            foreach (ValidationItem item in AllItems)
            {
                if (item.MyGoObject is CollapsibleNode)
                {
                    CollapsibleNode complexNode = item.MyGoObject as CollapsibleNode;
                    foreach (RepeaterSection rsec in complexNode.RepeaterSections)
                    {
                        if (item.Result == ValidationResult.OK)
                            break;
                        foreach (GoObject obj in rsec)
                        {
                            if (!(obj is CollapsingRecordNodeItem) || (obj as CollapsingRecordNodeItem).IsHeader)
                                continue;

                            item.Result = ValidationResult.OK;
                            break;
                        }
                    }

                    if (item.Result != ValidationResult.OK)
                    {
                        if (complexNode.MetaObject.Class.Contains("Entity"))
                            item.Result = ValidationResult.Error;
                        else
                            item.Result = ValidationResult.Warning;
                    }
                }
                else
                {
                    item.Result = ValidationResult.OK;
                }
            }
        }
        #endregion
    }
}
