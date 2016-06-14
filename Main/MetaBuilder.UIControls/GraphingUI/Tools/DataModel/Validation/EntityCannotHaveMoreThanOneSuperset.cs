using System;
using System.Collections.Generic;
using System.Text;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation
{
    internal class EntityCannotHaveMoreThanOneSuperset : BaseRule
    {
        public EntityCannotHaveMoreThanOneSuperset(string name, ExecutesOnEnum execsOn)
            : base(name, execsOn)
        {
            Description = "Subset Entities cannot have more than 1 Superset Entity. Remove a SubsetOf link.";
        }
        #region Implementation of Base Method
        public override void Apply()
        {

            Dictionary<string, List<ADDNode>> nodes = new Dictionary<string, List<ADDNode>>();
            foreach (ValidatedItem item in AllItems)
            {
                int subsetOfCounter = 0;

                Entity ent = item.MyNode as Entity;
                //string t = "";
                //if (ent.MBase.Get("DataEntityType") != null)
                //    t = ent.MBase.Get("DataEntityType").ToString();

                bool founderror = false;
                foreach (Relation r in ent.DependentRelations)
                {
                    if (r.RelationshipType == MetaBuilder.Meta.LinkAssociationType.SubSetOf || r.RelationshipType == MetaBuilder.Meta.LinkAssociationType.MutuallyExclusiveLink)
                    {
                        subsetOfCounter++;
                    }
                }

                if (subsetOfCounter > 1)
                {
                    founderror = true;
                    item.Result = ValidationResult.Error;
                    ErrorItems.Add(item);
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
