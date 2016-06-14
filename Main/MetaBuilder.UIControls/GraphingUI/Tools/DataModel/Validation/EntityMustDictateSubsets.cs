using System;
using System.Collections.Generic;
using System.Text;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation
{
    internal class EntityMustDictateSubsets : BaseRule
    {
        public EntityMustDictateSubsets(string name, ExecutesOnEnum execsOn)
            : base(name, execsOn)
        {
            Description = "Entity types (Object/Event) must be specified and dictate subset Entity types. Ensure that these subset Entities reflect the superset Entity type";
        }
        #region Implementation of Base Method
        public override void Apply()
        {

            Dictionary<string, List<ADDNode>> nodes = new Dictionary<string, List<ADDNode>>();
            foreach (ValidatedItem item in AllItems)
            {
                //bool foundDuplicate = false;
                Entity ent = item.MyNode as Entity;
                string t = "";
                if (ent.MBase.Get("DataEntityType") != null)
                    t = ent.MBase.Get("DataEntityType").ToString();

                bool founderror = false;
                foreach (Relation r in ent.OwnerRelations)
                {
                    string subtype = "";
                    if (r.From is XORNode)
                    {
                        XORNode x = r.From as XORNode;
                        foreach (Relation rX in x.OwnerRelations)
                        {
                            if (rX.From.MBase.Get("DataEntityType") != null)
                            {
                                subtype = rX.From.MBase.Get("DataEntityType").ToString();
                            }
                            if (subtype != t)
                            {
                                if (!ErrorItems.Contains(item))
                                {
                                    item.Result = ValidationResult.Warning;
                                    ErrorItems.Add(item);
                                }
                                founderror = true;
                            }
                        }
                    }
                    else
                    {
                        if (r.RelationshipType == MetaBuilder.Meta.LinkAssociationType.SubSetOf)
                        {
                            if (r.From.MBase.Get("DataEntityType") != null)
                            {
                                subtype = r.From.MBase.Get("DataEntityType").ToString();
                            }
                            if (subtype != t)
                            {
                                item.Result = ValidationResult.Error;
                                ErrorItems.Add(item);
                                founderror = true;
                            }
                        }
                    }

                }
                if (founderror == false)
                {
                    item.Result = ValidationResult.OK;
                    OKItems.Add(item);
                }
            }
        }
        #endregion
    }
}
