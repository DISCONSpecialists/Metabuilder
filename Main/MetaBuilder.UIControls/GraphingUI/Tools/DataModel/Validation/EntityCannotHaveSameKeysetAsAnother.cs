using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation
{
    internal class EntityCannotHaveSameKeysetAsAnother : BaseRule
    {
        public EntityCannotHaveSameKeysetAsAnother(string name, ExecutesOnEnum execsOn)
            : base(name, execsOn)
        {
            Description = "Two or more Entities can not have the same Key Attribute Set but different names. Then it is the same entity. The Sequence of the Key Attributes in the Set does not matter.";

        }
        #region Implementation of Base Method
        public override void Apply()
        {


            foreach (ValidatedItem vitem in AllItems)
            {
                Entity ent = vitem.MyNode as Entity;
                if (ent.KeyAttributes.Count >= 0)
                {
                    //bool MatchedThisItem = false;
                    //ent.KeyAttributes.Sort();
                    foreach (ValidatedItem vitemOther in AllItems)
                    {
                        Entity eOther = vitemOther.MyNode as Entity;
                        if (vitemOther != vitem && eOther.KeyAttributes.Count == ent.KeyAttributes.Count)
                        {
                            bool foundAll = true;
                            foreach (Attr a in ent.KeyAttributes)
                            {
                                bool found = false;
                           
                                foreach (Attr aB in eOther.KeyAttributes)
                                {
                                    if (a.Name == aB.Name)
                                    {
                                        found = true;
                                    }
                                }
                                if (!found)
                                    foundAll = false;
                            }
                            if (foundAll)
                            {
                                //MatchedThisItem = true;
                                vitem.Result = ValidationResult.Error;
                                ErrorItems.Add(vitem);
                            }
                            else
                            {
                                vitem.Result = ValidationResult.OK;
                                OKItems.Add(vitem);
                            }
                        }
                    }
                }
            }


        
           
        }
        #endregion
    }
}
