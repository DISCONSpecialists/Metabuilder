using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Graphing.Shapes;
namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Inference
{
	internal class Reflexivity: BaseRule
	{
        public Reflexivity()
        {
            this.Description = "If X is a true super-set or equal to Y, then X functionally determines Y";
        }
        
        public string StripOutBrackets(string s)
        {
            if (s.Contains("<"))
            {
               return s.Substring(0, s.IndexOf("<"));
            }
            else
            {
                return s;
            }
        }
        #region Implementation of Base Method
        public override void Apply(Engine e)
        {
            e.AddReflexiveLinks();
            foreach (ADDNode child in e.addnodes)
            {
                if (child is Entity)
                {
                    Entity ent = child as Entity;
                    foreach (Relation r in ent.DependentRelations)
                    {
                        Entity entParent = null;
                        if (r.To is Entity)
                        {
                            if (r.IsAbstract && r.InferenceType == DependencyType.Reflexive)
                            {
                                // nothing to do
                            }
                            else
                            {
                                entParent = r.To as Entity;
                                bool foundAll = true;
                                if (entParent.KeyAttributes.Count > 0)
                                {
                                    foreach (Attr attrP in entParent.KeyAttributes)
                                    {
                                        bool found = false;
                                        foreach (Attr attrC in ent.KeyAttributes)
                                        {
                                            if (StripOutBrackets(attrP.Name.ToLower()) == StripOutBrackets(attrC.Name.ToLower()))
                                            {
                                                found = true;
                                            }
                                        }
                                        if (!found)
                                        {
                                            //Console.WriteLine("Foundall = false");
                                            foundAll = false;
                                        }

                                    }
                                    if (foundAll && entParent.KeyAttributes.Count <= ent.KeyAttributes.Count)
                                    {
                                        
                                            //Console.WriteLine(r.InferenceType.ToString());
                                            if (r.InferenceType == DependencyType.NotSpecified)
                                                r.InferenceType = DependencyType.Reflexive;
                                        
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion
      
	}
}
