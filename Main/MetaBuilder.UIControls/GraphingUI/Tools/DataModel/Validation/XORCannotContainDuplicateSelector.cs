using System;
using System.Collections.Generic;
using System.Text;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation
{
    public class XORCannotContainDuplicateSelector : BaseRule
    {
        internal XORCannotContainDuplicateSelector(string name, ExecutesOnEnum execsOn)
            : base(name, execsOn)
        {
            Description = "Selector Attribute shares name with another Selector. Ensure that Selector Attributes have unique names";
        }

        #region Implementation of Base Method
        public override void Apply()
        {
            Dictionary<string, List<ADDNode>> nodes = new Dictionary<string, List<ADDNode>>();
            foreach (ValidatedItem item in AllItems)
            {
                bool foundDuplicate = false;
                SelectorAttribute ent = item.MyNode as SelectorAttribute;
                if (nodes.ContainsKey(ent.Name))
                {
                    foreach (ADDNode n in nodes[ent.Name])
                    {
                        if (n.MBase.pkid != ent.MBase.pkid)
                        {
                            foundDuplicate = true;
                        }
                    }

                }
                else
                {
                    nodes.Add(ent.Name, new List<ADDNode>());
                    nodes[ent.Name].Add(ent);
                }

                if (foundDuplicate)
                {
                    item.Result = ValidationResult.Error;
                    ErrorItems.Add(item);
                    foreach (ValidatedItem otherItem in AllItems)
                    {
                        SelectorAttribute entOther = otherItem.MyNode as SelectorAttribute;
                        if (entOther.Name == ent.Name && entOther != ent && entOther.MBase.pkid != ent.MBase.pkid)
                        {
                            otherItem.Result = ValidationResult.Error;
                            ErrorItems.Add(otherItem);
                        }
                    }
                }
                else
                {
                    item.Result = ValidationResult.OK;
                    OKItems.Add(item);
                }
            }

          
        }
        #endregion
    }
}
