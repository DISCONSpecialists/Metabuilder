using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation.Basic
{
    public class NodeMustBeLinked : BaseRule2015
    {
        internal NodeMustBeLinked(string name, ExecutesOnEnum execsOn, RuleType type)
            : base(name, execsOn, type)
        {
            Description = "Node must be linked";
        }

        #region Implementation of Base Method
        public override void Apply()
        {
            foreach (ValidationItem item in AllItems)
            {
                if (!(Core.Variables.Instance.ClassesWithoutStencil.Contains((item.MyGoObject as IMetaNode).MetaObject.Class)))
                {
                    if ((item.MyGoObject as GoNode) == null || (item.MyGoObject as GoNode).Links.Count == 0)
                        item.Result = ValidationResult.Error;
                    else
                        item.Result = ValidationResult.OK;
                }
                else
                {
                    //probably an artefact or RATIONALE?
                    if (item.MyGoObject is MetaBuilder.Graphing.Shapes.ArtefactNode)
                    {
                        MetaBuilder.Graphing.Shapes.ArtefactNode art = item.MyGoObject as MetaBuilder.Graphing.Shapes.ArtefactNode;
                        if (art.Links.Count == 0)
                            item.Result = ValidationResult.Error;
                        else
                            item.Result = ValidationResult.OK;
                    }
                    else
                    {
                        item.Result = ValidationResult.OK;
                        item.ToString();
                    }

                }
            }
        }
        #endregion
    }
}
