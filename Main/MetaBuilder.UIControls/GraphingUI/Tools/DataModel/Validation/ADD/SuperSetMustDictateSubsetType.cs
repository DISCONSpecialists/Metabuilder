using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;
using MetaBuilder.Meta;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation.ADD
{
    internal class SuperSetMustDictateSubsetType : BaseRule2015
    {
        public SuperSetMustDictateSubsetType(string name, ExecutesOnEnum execsOn, RuleType type)
            : base(name, execsOn, type)
        {
            Description = "Entity types (Object/Event) must be specified and dictate subset Entity types. Ensure that these subset Entities reflect the superset Entity type";
        }
        #region Implementation of Base Method
        public override void Apply()
        {
            switch (ExecutesOn)
            {
                case ExecutesOnEnum.Entity:
                    Entity();
                    break;
                case ExecutesOnEnum.SubSet:
                    SubSet();
                    break;
            }
        }

        /// <summary>
        /// EntityA-->EntityB
        /// B Governs Type
        /// </summary>
        private void Entity()
        {
            foreach (ValidationItem item in AllItems)
            {
                foreach (IGoLink lnk in (item.MyGoObject as CollapsibleNode).DestinationLinks)
                {
                    if (lnk is QLink)
                    {
                        QLink link = lnk as QLink;
                        if (link.AssociationType == MetaBuilder.Meta.LinkAssociationType.SubSetOf)
                        {
                            CollapsibleNode FromNode = item.MyGoObject as CollapsibleNode;
                            CollapsibleNode ToNode = lnk.ToNode as CollapsibleNode;
                            try
                            {
                                if (FromNode.MetaObject.Get("DataEntityType").ToString() != ToNode.MetaObject.Get("DataEntityType").ToString())
                                {
                                    item.Result = ValidationResult.Error;
                                    item.AdditionalInformation = "Change type to " + (ToNode.MetaObject.Get("DataEntityType").ToString() == "E" ? "Event" : "Object");
                                    break;
                                }
                                else
                                {
                                    item.Result = ValidationResult.OK;
                                }
                            }
                            catch
                            {
                                item.Result = ValidationResult.Error;
                                break;
                            }
                        }
                    }
                }
                if (item.Result != ValidationResult.Error)
                    item.Result = ValidationResult.OK;
            }
        }
        /// <summary>
        /// EntityA--\/
        ///           O--->EntityC
        /// EntityB--/\
        /// C Governs Type of A and B if A and B are MutuallyExclusive else ?
        /// </summary>
        private void SubSet()
        {
            foreach (ValidationItem item in AllItems)
            {
                GraphNode xorNode = item.MyGoObject as GraphNode;
                MetaBase superset = null;
                foreach (IGoLink lnk in xorNode.DestinationLinks)
                {
                    if (!(lnk is QLink))
                        continue;

                    if ((lnk as QLink).AssociationType != LinkAssociationType.SubSetOf)
                        continue;

                    if (!(lnk.ToNode is CollapsibleNode))
                        continue;

                    if (superset == null)
                    {
                        superset = (lnk.ToNode as CollapsibleNode).MetaObject;
                    }
                    else
                    {
                        //the xor node is part of 2 supersets
                        item.Result = ValidationResult.Error;
                        item.AdditionalInformation = "Cannot have multiple supersets";
                        break;
                    }
                }

                if (item.Result != ValidationResult.None)
                    continue;

                if (superset == null)
                {
                    item.Result = ValidationResult.Error;
                    item.AdditionalInformation = "Does not have superset";
                    continue;
                }

                foreach (IGoLink lnk in xorNode.SourceLinks)
                {
                    if (!(lnk is QLink))
                        continue;

                    if ((lnk as QLink).AssociationType != LinkAssociationType.MutuallyExclusiveLink)
                        continue;

                    if (!(lnk.FromNode is CollapsibleNode))
                        continue;
                    MetaBase subset = (lnk.FromNode as CollapsibleNode).MetaObject;
                    try
                    {
                        if (subset.Get("DataEntityType").ToString() != superset.Get("DataEntityType").ToString())
                        {
                            item.Result = ValidationResult.Error;
                            item.AdditionalInformation = "Change type to " + (superset.Get("DataEntityType").ToString() == "E" ? "Event" : "Object");
                        }
                        else
                        {
                            if (item.Result != ValidationResult.Error && item.Result != ValidationResult.Warning)
                                item.Result = ValidationResult.OK;
                        }
                    }
                    catch
                    {
                        item.Result = ValidationResult.Error;
                    }
                }
            }
        }

        #endregion

    }
}
