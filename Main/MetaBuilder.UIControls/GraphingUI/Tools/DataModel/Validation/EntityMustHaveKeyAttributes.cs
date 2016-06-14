using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation
{
    internal class EntityMustHaveKeyAttributes : BaseRule
    {
        public EntityMustHaveKeyAttributes(string name, ExecutesOnEnum execsOn)
            : base(name, execsOn)
        {
            Description = "An Entity must have a Key Attribute set. Add Key Attributes for Entities that failed validation.";
        }
        #region Implementation of Base Method
        public override void Apply()
        {
            foreach (ValidatedItem vitem in AllItems)
            {
                //bool foundDuplicate = false;
                Entity ent = vitem.MyNode as Entity;
                foreach (GoObject goobj in ent.MyGoObjects)
                {
                    GoNode n = goobj as GoNode;
                    if (ent.KeyAttributes.Count == 0)
                    {

                        bool isSubset = false;
                        if (ent.DependentRelations.Count > 0)
                        {
                            foreach (Relation r in ent.DependentRelations)
                            {
                                if (r.RelationshipType == MetaBuilder.Meta.LinkAssociationType.SubSetOf || r.RelationshipType == MetaBuilder.Meta.LinkAssociationType.MutuallyExclusiveLink)
                                {
                                    isSubset = true;
                                }
                            }
                        }

                        if (isSubset)
                        {
                            vitem.Result = ValidationResult.Warning;
                            ErrorItems.Add(vitem);
                        }
                        else
                        {
                            vitem.Result = ValidationResult.Error;
                            ErrorItems.Add(vitem);
                        }
                    }
                    else
                    {
                        vitem.Result = ValidationResult.OK;
                        OKItems.Add(vitem);
                    }
                }
            }


        }
        #endregion
    }
}
