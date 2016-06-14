using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing;
using MetaBuilder.Graphing.Shapes.Nodes; 

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation
{
    internal class XORMustHaveSubsetLink : BaseRule
    {
        public XORMustHaveSubsetLink(string name, ExecutesOnEnum execsOn)
            : base(name,execsOn)
        {
            Description = "An XOR Must have (1) subset link to a superset Entity. Add or Change links on XORs that failed validation.";
            
        }
        #region Implementation of Base Method
        public override void Apply()
        {
            foreach (ValidatedItem item in AllItems)
            {
                XORNode ent = item.MyNode as XORNode;
                if (ent.DependentRelations.Count == 0 || ent.DependentRelations.Count>1)
                {
                    item.Result = ValidationResult.Error;
                    ErrorItems.Add(item);
                }
                bool foundOne = false;
                foreach (Relation rel in ent.DependentRelations)
                {
                    // GETQLink AND SELECTOR
                    if (rel.RelationshipType == MetaBuilder.Meta.LinkAssociationType.SubSetOf)
                    {
                        foundOne = true;
                    }
                   
                }
                if (!foundOne)
                {
                    item.Result = ValidationResult.Error;
                    if (!(ErrorItems.Contains(item)))
                        ErrorItems.Add(item);
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
