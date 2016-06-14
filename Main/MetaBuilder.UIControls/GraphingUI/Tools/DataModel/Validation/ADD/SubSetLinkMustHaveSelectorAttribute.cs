using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation.ADD
{
    internal class SubSetLinkMustHaveSelectorAttribute : BaseRule2015
    {
        public SubSetLinkMustHaveSelectorAttribute(string name, ExecutesOnEnum execsOn, RuleType type)
            : base(name, execsOn, type)
        {
            Description = "Subset links must have one selector attribute";
        }
        #region Implementation of Base Method
        public override void Apply()
        {
            foreach (ValidationItem item in AllItems)
            {
                QLink lnk = item.MyGoObject as QLink;
                List<IMetaNode> artefacts = lnk.GetArtefacts();
                int i = 0;
                foreach (IMetaNode artefact in artefacts)
                {
                    if (artefact.MetaObject.Class == "SelectorAttribute")
                        i++;
                }
                if (i > 1 || i == 0)
                {
                    item.Result = ValidationResult.Error;
                    //item.ValidationType = "Missing";
                }
                else
                {
                    item.Result = ValidationResult.OK;
                    //item.ValidationType = "";
                }
                
                item.AddObject = false;
                item.AdditionalInformation = "Artefact";
            }
        }

        #endregion

    }
}
