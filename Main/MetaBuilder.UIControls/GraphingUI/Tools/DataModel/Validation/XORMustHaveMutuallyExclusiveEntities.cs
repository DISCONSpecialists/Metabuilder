using System;
using System.Collections.Generic;
using System.Text;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation
{
    internal class XORMustHaveMutuallyExclusiveEntities : BaseRule
    {
        public XORMustHaveMutuallyExclusiveEntities(string name, ExecutesOnEnum execsOn)
            : base(name, execsOn)
        {
            Description = "An XOR must have a Superset Entity and 1 ore more Subset Entities. Add subset Entities to XORs that failed validation.";
        }
        #region Implementation of Base Method
        public override void Apply()
        {
            foreach (ValidatedItem item in AllItems)
            {
                //bool foundDuplicate = false;
                XORNode ent = item.MyNode as XORNode;
                if (ent.OwnerRelations.Count == 0)
                {
                    item.Result = ValidationResult.Error;
                    ErrorItems.Add(item);
                }
                
                foreach (Relation rel in ent.OwnerRelations)
                {
                    if (rel.RelationshipType != MetaBuilder.Meta.LinkAssociationType.MutuallyExclusiveLink)
                    {
                        item.Result = ValidationResult.Error;
                        ErrorItems.Add(item);
                    }
                    
                }
                if (!ErrorItems.Contains(item))
                {
                    item.Result = ValidationResult.OK;
                    OKItems.Add(item);
                }
            }
        }
        #endregion
    }
}
