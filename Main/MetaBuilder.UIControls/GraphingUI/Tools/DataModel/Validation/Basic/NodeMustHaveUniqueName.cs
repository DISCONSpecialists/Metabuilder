using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Graphing.Shapes;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation.Basic
{
    public class NodeMustHaveUniqueName : BaseRule2015
    {
        internal NodeMustHaveUniqueName(string name, ExecutesOnEnum execsOn, RuleType type)
            : base(name, execsOn, type)
        {
            Description = "Node must have a unique name";
        }

        #region Implementation of Base Method
        List<string> used;
        List<string> actedOn;
        public override void Apply()
        {
            used = new List<string>();
            actedOn = new List<string>();
            used.Add("");
            foreach (ValidationItem item in AllItems)
            {
                string key = (item.MyGoObject as IMetaNode).MetaObject.pkid.ToString() + ":" + (item.MyGoObject as IMetaNode).MetaObject.MachineName;

                if (actedOn.Contains(key))
                {
                    item.Result = ValidationResult.OK;
                    continue;
                }

                actedOn.Add(key);

                //string itemName = ;
                IMetaNode nn = (item.MyGoObject as IMetaNode);
                if (nn.MetaObject.ToString() != null && nn.MetaObject.ToString().Length == 0)
                {
                    if (nn.MetaObject.Class.Contains("Subset"))
                        item.Result = ValidationResult.Warning;
                    else
                        item.Result = ValidationResult.Error;

                    item.AdditionalInformation = "No name specified";
                    continue;
                }

                if (used.Contains((nn.MetaObject.ToString() != null ? nn.MetaObject.ToString() : nn.MetaObject.Class) + "|" + (item.MyGoObject as IMetaNode).MetaObject.Class))
                {
                    if (nn.MetaObject.Class.Contains("Entity"))
                        item.Result = ValidationResult.Error;
                    else
                        item.Result = ValidationResult.Warning;
                }
                else
                {
                    item.Result = ValidationResult.OK;
                    used.Add((item.MyGoObject as IMetaNode).MetaObject.ToString() + "|" + (item.MyGoObject as IMetaNode).MetaObject.Class);
                }

            }
        }
        #endregion
    }
}
