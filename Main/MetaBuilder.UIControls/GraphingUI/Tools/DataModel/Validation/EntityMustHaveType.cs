using System;
using System.Collections.Generic;
using System.Text;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation
{
    internal class EntityMustHaveType : BaseRule
    {
        public EntityMustHaveType(string name, ExecutesOnEnum execsOn)
            : base(name, execsOn)
        {
            Description = "An Entity must have a type specified. Specify types for Entities that failed validation.";
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
                if (t == "")
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
        #endregion
    }
}
