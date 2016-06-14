using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;
using MetaBuilder.Graphing.Shapes;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation.ADD.Inference
{
    class Ordinary : BaseInferenceRule
    {
        public Ordinary(string name, Engine2015 engine)
            : base(name, engine)
        {
            Description = "Ordinary";
        }
        #region Implementation of Base Method
        /// <summary>
        /// x-->y (Unless r/t/a)
        /// this does not add associations
        /// </summary>
        public override void Apply()
        {
            foreach (GoObject o in Engine.Diagram)
            {
                if (o is QLink)
                {
                    if ((o as QLink).AssociationType == MetaBuilder.Meta.LinkAssociationType.Dependency && ((o as QLink).InferenceType == InferenceType.Ordinary || (o as QLink).InferenceType == InferenceType.None))
                    {
                        (o as QLink).InferenceType = InferenceType.Ordinary;

                        ValidationItem i = new ValidationItem();
                        i.Result = ValidationResult.OK;
                        i.MyGoObject = o;
                        i.ValidationType = "Ordinary";
                        i.AddObject = false;

                        AllItems.Add(i);
                    }
                    else if ((o as QLink).InferenceType == InferenceType.Error)
                    {
                        if (!(Engine.ErrorLinks.Contains(o as QLink)))
                        {
                            ValidationItem i = new ValidationItem();
                            i.Result = ValidationResult.Error;
                            i.MyGoObject = o;
                            i.ValidationType = "Error";

                            if ((o as QLink).AutomatedAddition)
                                i.AddObject = true;
                            else
                                i.AddObject = false;

                            AllItems.Add(i);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
