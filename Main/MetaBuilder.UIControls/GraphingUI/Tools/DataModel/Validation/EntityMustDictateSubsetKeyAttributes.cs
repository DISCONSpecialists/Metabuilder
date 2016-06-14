using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation
{
    public class EntityMustDictateSubsetKeyAttributes : BaseRule
    {
        internal EntityMustDictateSubsetKeyAttributes(string name, ExecutesOnEnum execsOn)
            : base(name, execsOn)
        {
            Description = "Superset Entity must dictate subset entities key attributes";
        }

        #region Implementation of Base Method
        public override void Apply()
        {
            //Ex(1,2)<--subsetof--Ey(1,2,3,4) [Error if missing][Warn if 1 and 2 are not shallow copies]
            //Ex(1,2)<--subindicator<--Ey(1,2,3,4) [Error if missing][Warn if 1 and 2 are not shallow copies]

            foreach (ValidatedItem vitem in AllItems)
            {
                Entity ent = vitem.MyNode as Entity;
                foreach (Relation r in ent.DependentRelations)
                {
                    if (r.To is Entity && r.RelationshipType == MetaBuilder.Meta.LinkAssociationType.SubSetOf)
                    {
                        int foundCount = 0;
                        int shallowCount = 0;
                        //check if key attributes on from exist on ent
                        foreach (Attr aC in (r.To as Entity).KeyAttributes)
                        {
                            foreach (Attr aP in ent.KeyAttributes)
                            {
                                if (aP.Name == aC.Name)
                                {
                                    foundCount += 1;
                                    if (aP.Key == aC.Key)
                                        shallowCount += 1;
                                }
                            }
                        }
                        //error if missing 
                        if (foundCount != (r.To as Entity).KeyAttributes.Count)
                        {
                            vitem.Result = ValidationResult.Warning;
                            OKItems.Add(vitem);
                        }
                        else
                        {
                            //warn if not shallow
                            //if (shallowCount != (r.To as Entity).KeyAttributes.Count)
                            //    vitem.Result = ValidationResult.Warning;
                            //else
                            vitem.Result = ValidationResult.OK;

                            OKItems.Add(vitem);
                        }
                    }
                    else if (r.To is XORNode)
                    {

                    }
                }
            }
        }
        #endregion
    }


    public class EntityMustDictateDependantKeyAttributes : BaseRule
    {
        internal EntityMustDictateDependantKeyAttributes(string name, ExecutesOnEnum execsOn)
            : base(name, execsOn)
        {
            Description = "Entity's key attributes must be dictated by owner entity";
        }

        #region Implementation of Base Method
        public override void Apply()
        {
            //Ex(1,2)<--dep--Ey(1,2,3,4) [Error if missing][Warn if 1 and 2 are not shallow copies]

            foreach (ValidatedItem vitem in AllItems)
            {
                Entity ent = vitem.MyNode as Entity;
                vitem.Result = ValidationResult.OK;

                foreach (Relation r in ent.DependentRelations)
                {
                    if (r.To is Entity && r.RelationshipType == MetaBuilder.Meta.LinkAssociationType.Dependency)
                    {
                        int foundCount = 0;
                        int shallowCount = 0;
                        //check if key attributes on from exist on ent
                        foreach (Attr aC in (r.To as Entity).KeyAttributes) //to is the parent
                        {
                            foreach (Attr aP in ent.KeyAttributes)
                            {
                                if (aP.Name.Trim() == aC.Name.Trim())
                                {
                                    foundCount += 1;
                                    if (aP.Key == aC.Key)
                                        shallowCount += 1;
                                }
                            }
                        }
                        //warn if missing (missing can also mean different, we do not have linguistics algorithym to check if personID should actually be personNumber or vice versa)
                        if (foundCount != (r.To as Entity).KeyAttributes.Count)
                        {
                            vitem.Result = ValidationResult.Warning;
                            //erroritems?
                            OKItems.Add(vitem);
                        }
                        else
                        {
                            //warn if not shallow
                            vitem.Result = ValidationResult.OK;
                            //if (shallowCount != (r.To as Entity).KeyAttributes.Count)
                            //    vitem.Result = ValidationResult.Warning;
                            //else
                            //vitem.Result = ValidationResult.OK;

                            OKItems.Add(vitem);
                        }
                    }

                }
            }
        }
        #endregion
    }

}