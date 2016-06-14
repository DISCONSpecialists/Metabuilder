using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing;
using MetaBuilder.Graphing.Shapes.Nodes;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation
{
    internal class SubsetsMustHaveSelectorAttribute : BaseRule
    {
        public SubsetsMustHaveSelectorAttribute(string name, ExecutesOnEnum execsOn)
            : base(name, execsOn)
        {
            Description = "Subset Entities must have 1 Selector Attribute on (a) the link to the Superset Entity or (b) from the XOR to the Superset Entity.";
        }
        #region Implementation of Base Method
        public override void Apply()
        {
            bool foundOne = false;
            foreach (ValidatedItem item in AllItems)
            {
                int counter = 0;
                bool mustHaveSelector = false;
                if (item.MyNode is XORNode)
                {
                    mustHaveSelector = true;
                    XORNode ent = item.MyNode as XORNode;
                    if (ent.OwnerRelations.Count == 0)
                    {
                        item.Result = ValidationResult.Error;
                        ErrorItems.Add(item);
                    }
                    foreach (Relation rel in ent.DependentRelations)
                    {
                        // GETQLink AND SELECTOR
                        QLink slink = rel.MyGoObject as QLink;
                        foreach (ArtefactNode artnode in slink.GetArtefacts())
                        {
                            if (artnode.MetaObject != null)
                            {
                                if (artnode.BindingInfo.BindingClass == "SelectorAttribute")
                                {
                                    foundOne = true;
                                    counter++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    Entity ent = item.MyNode as Entity;
                    //28 Sep 2015?
                    //if (ent.OwnerRelations.Count == 0)
                    //{
                    //    item.Result = ValidationResult.Error;
                    //    ErrorItems.Add(item);
                    //}

                    MetaBuilder.Meta.MetaBase foundArt = null;
                    foreach (Relation rel in ent.DependentRelations)
                    {
                        if (rel.RelationshipType == MetaBuilder.Meta.LinkAssociationType.SubSetOf)
                        {
                            mustHaveSelector = true;
                            // GETQLink AND SELECTOR
                            QLink slink = rel.MyGoObject as QLink;
                            foreach (ArtefactNode artnode in slink.GetArtefacts())
                            {
                                if (artnode.MetaObject != null)
                                {
                                    if (foundArt == artnode.MetaObject)
                                        continue;
                                    foundArt = artnode.MetaObject;
                                    if (artnode.MetaObject.Class == "SelectorAttribute")
                                    {
                                        foundOne = true;
                                        counter++;
                                    }
                                }
                            }
                        }
                    }
                }
                if ((!(foundOne) || counter > 1) && mustHaveSelector)
                //if (!(foundOne) && mustHaveSelector)
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
