using System;
using System.Collections.Generic;
using System.Text;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation
{
    internal class XOROnlyOneEntityWarning : BaseRule
    {
        public XOROnlyOneEntityWarning(string name, ExecutesOnEnum execsOn)
            : base(name, execsOn)
        {
            Description = "An XOR should usually have more than one subset Entity. Add more subset Entities to the XOR (or merge the subset with the superset) that failed validation - but this is not required.";
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
                int counter = 0;
                bool error = false;
                foreach (Relation rel in ent.OwnerRelations)
                {
                    if (rel.RelationshipType != MetaBuilder.Meta.LinkAssociationType.MutuallyExclusiveLink)
                    {
                        error = true;
                    }
                    else
                    {
                        counter++;
                    }
                }

                if (counter == 1 && (!(error)))
                {
                    item.Result = ValidationResult.Warning;
                    if (!(ErrorItems.Contains(item)))
                        ErrorItems.Add(item);
                }
                if (error)
                {
                    item.Result = ValidationResult.Error;
                    if (!(ErrorItems.Contains(item)))
                        ErrorItems.Add(item);
                }

                if (!(ErrorItems.Contains(item)))
                {
                    item.Result = ValidationResult.OK;
                    OKItems.Add(item);
                }
               
            }
        }
        #endregion
    }
}
