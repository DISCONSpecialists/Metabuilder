using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;
using MetaBuilder.Meta;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation.ADD
{
    internal class SubSetIndicatorMustHaveMoreThanOneSubSet : BaseRule2015
    {
        public SubSetIndicatorMustHaveMoreThanOneSubSet(string name, ExecutesOnEnum execsOn, RuleType type)
            : base(name, execsOn, type)
        {
            Description = "An Entity with a subsetindicator must have more than one subset entity";
        }
        #region Implementation of Base Method
        public override void Apply()
        {
            foreach (ValidationItem item in AllItems)
            {
                GraphNode xorNode = item.MyGoObject as GraphNode;
                int i = 0;
                foreach (IGoLink lnk in xorNode.SourceLinks)
                {
                    if (!(lnk is QLink))
                        continue;

                    if ((lnk as QLink).AssociationType != LinkAssociationType.MutuallyExclusiveLink)
                        continue;

                    if (!(lnk.FromNode is CollapsibleNode))
                        continue;

                    i++;
                }

                if (i == 0)
                {
                    item.Result = ValidationResult.Error;
                    item.AdditionalInformation = "No subsets";
                }
                else if (i == 1)
                {
                    item.Result = ValidationResult.Warning;
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
