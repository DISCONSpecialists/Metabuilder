using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;
using System.Reflection;
using MetaBuilder.Meta;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation.Basic
{
    public class NodeMustHaveType : BaseRule2015
    {
        internal NodeMustHaveType(string name, ExecutesOnEnum execsOn, RuleType type)
            : base(name, execsOn, type)
        {
            Description = "Node must have type specified";
        }

        #region Implementation of Base Method
        public override void Apply()
        {
            foreach (ValidationItem item in AllItems)
            {
                MetaBase mBase = (item.MyGoObject as IMetaNode).MetaObject;
                if (mBase.Class.Contains("Entity"))
                {
                    if (mBase.Get("DataEntityType") == null || mBase.Get("DataEntityType").ToString() == "")
                        item.Result = ValidationResult.Error;
                    else
                        item.Result = ValidationResult.OK;
                    continue;
                }
                //Add other specific classes here
                //else if
                else
                {
                    foreach (PropertyInfo info in mBase.GetType().GetProperties())
                    {
                        if (info.Name.Contains("Type"))
                        {
                            if (mBase.Get(info.Name) == null || mBase.Get(info.Name).ToString() == "")
                            {
                                item.Result = ValidationResult.Warning;
                                break;
                            }
                        }

                    }
                }

                if (item.Result == ValidationResult.None)
                    item.Result = ValidationResult.OK;
            }
        }
        #endregion
    }
}
