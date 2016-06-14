using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation
{

    internal class EntityMustBeConnected : BaseRule
    {
        public EntityMustBeConnected(string name, ExecutesOnEnum execsOn)
            : base(name, execsOn)
        {
            Description = "Entity must be connected. Ensure that this entity is connected";
        }
        #region Implementation of Base Method
        public override void Apply()
        {
            Dictionary<string, List<ADDNode>> nodes = new Dictionary<string, List<ADDNode>>();
            foreach (ValidatedItem item in AllItems)
            {
                //bool foundDuplicate = false;
                Entity ent = item.MyNode as Entity;
                foreach (GoNode gonod in ent.MyGoObjects)
                {
                    GoNode n = gonod as GoNode;
                    if (n.DestinationLinks.Count == 0 && n.SourceLinks.Count == 0)
                    {
                        item.Result = ValidationResult.Error;
                        ErrorItems.Add(item);
                    }
                    else
                    {
                        item.Result = ValidationResult.OK;
                        OKItems.Add(item);
                    }
                }
            }


        }
        #endregion
    }
}
