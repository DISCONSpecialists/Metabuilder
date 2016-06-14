using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation
{
    public class EntityCannotBeDuplicated : BaseRule
    {
        internal EntityCannotBeDuplicated(string name, ExecutesOnEnum execsOn)
            : base(name, execsOn)
        {
            Description = "Entity shares name with another entity. Ensure that entities have unique names";
        }

        #region Implementation of Base Method
        public override void Apply()
        {
            foreach (ValidatedItem vitem in AllItems)
            {
                //bool foundDuplicate = false;
                Entity ent = vitem.MyNode as Entity;

                if (ent.KeyAttributes.Count == 0)
                {
                    vitem.Result = ValidationResult.Error;
                    ErrorItems.Add(vitem);
                }
                else
                {
                    vitem.Result = ValidationResult.OK;
                    OKItems.Add(vitem);
                }
            }

            Dictionary<string, List<ADDNode>> nodes = new Dictionary<string, List<ADDNode>>();
            foreach (ValidatedItem item in AllItems)
            {
                bool foundDuplicate = false;
                Entity ent = item.MyNode as Entity;
                if (nodes.ContainsKey(ent.Name))
                {
                    foreach (ADDNode n in nodes[ent.Name])
                    {
                        if ((n.MBase.pkid + n.MBase.MachineName) != (ent.MBase.pkid + ent.MBase.MachineName))
                        {
                            foundDuplicate = true;
                            item.Result = ValidationResult.Error;
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
                    ErrorItems.Add(item);
                    item.Result = ValidationResult.Error;

                    foreach (ValidatedItem otherItem in AllItems)
                    {
                        Entity entOther = otherItem.MyNode as Entity;
                        if (entOther.Name == ent.Name && (entOther.MBase.pkid + entOther.MBase.MachineName) != (ent.MBase.pkid + ent.MBase.MachineName))// && entOther != ent
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
