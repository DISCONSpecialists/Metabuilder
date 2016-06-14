using System;
using System.Collections.Generic;
using System.Text;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Inference
{
    internal class Ordinary: BaseRule
    {
        public Ordinary()
        {
            this.Description = "Does not conform to any of the other inference rules";
        }
        #region Implementation of Base Method
        public override void Apply(Engine e)
        {
            foreach (Relation r in e.GetRelations())
            {
                if (r.InferenceType == DependencyType.NotSpecified && r.RelationshipType == MetaBuilder.Meta.LinkAssociationType.Dependency)
                {
                    r.InferenceType = DependencyType.Ordinary;
                }
            }
        }
        #endregion
    }
}
