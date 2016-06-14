using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation.ADD
{
    internal class EntityCanOnlyHaveOneSuperset : BaseRule2015
    {
        public EntityCanOnlyHaveOneSuperset(string name, ExecutesOnEnum execsOn, RuleType type)
            : base(name, execsOn, type)
        {
            Description = "A SubSet Entity can only have 1 superset";
        }
        #region Implementation of Base Method
        public override void Apply()
        {
            foreach (ValidationItem item in AllItems)
            {
                CollapsibleNode node = item.MyGoObject as CollapsibleNode;
                GoObject destinationObject = null;
                foreach (IGoLink lnk in node.DestinationLinks)
                {
                    if (!(lnk is QLink))
                        continue;

                    QLink link = lnk as QLink;
                    if (link.AssociationType != MetaBuilder.Meta.LinkAssociationType.SubSetOf)
                        continue;

                    if ((lnk.ToNode as IMetaNode).MetaObject.Class == "DataEntity")
                    {
                        if (destinationObject != null)
                        {
                            item.Result = ValidationResult.Error;
                            break;
                        }

                        destinationObject = lnk.ToNode as GoObject;
                    }
                }

                item.Result = ValidationResult.OK;
            }
        }

        #endregion
    }
}
