using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Graphing;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;
using MetaBuilder.Meta;


namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Inference
{
    internal class Augmentation : BaseRule
    {
        private Engine e;

        public Engine E
        {
            get { return e; }
            set { e = value; }
        }

        public Augmentation()
        {
            this.Description = "If XZ and YK exists together and K = Z, then XZ functionally determines YK";
        }
        #region Implementation of Base Method
        public override void Apply(Engine e) // TODO: override using Engine as argument
        {
            this.E = e;

            List<Entity> entitiesToBeTested = new List<Entity>();
            foreach (ADDNode a1 in E.addnodes)
            {
                if (a1 is Entity)
                {
                    Entity e1 = a1 as Entity;
                    if (!e1.IsSubSetObject)
                        entitiesToBeTested.Add(e1);

                }
            }

            List<Relation> relationsToAdd = new List<Relation>();
            e.BuildKeysetsAugmentation();

            foreach (Entity outerEntity in entitiesToBeTested)
            {
                List<Entity> outerParents = outerEntity.GetParentEntities(true);

                //List<Entity> outerSingleParents = outerEntity.GetSingleChainParents();
                // Inner loop
                foreach (Entity innerEntity in entitiesToBeTested)
                {
                    // Can't compare the same entity's parent :)
                    if (outerEntity != innerEntity)
                    {

                        bool matched = e.GetDependency(outerEntity, innerEntity);
                        bool exists = false;

                        foreach (Relation rOUTER in outerEntity.DependentRelations)
                        {
                            if (rOUTER.To == innerEntity && rOUTER.RelationshipType != LinkAssociationType.Auxiliary
                                && rOUTER.RelationshipType != LinkAssociationType.SubSetOf && rOUTER.RelationshipType != LinkAssociationType.MutuallyExclusiveLink)
                            {
                                exists = true;
                                if (matched)
                                    if (rOUTER.InferenceType == DependencyType.NotSpecified)
                                        rOUTER.InferenceType = DependencyType.Augmentative;
                            }
                        }
                        if (matched && (!exists))
                        {
                            if (!CheckExistingLink(outerEntity, innerEntity, e))
                                AddAugmentationLink(e, relationsToAdd, outerEntity, innerEntity);


                            /*
                            bool outerHasInnersParents = true;
                            foreach (Entity innerParent in innerParents)
                            {
                            
                                if (!outerParents.Contains(innerParent))
                                {
                                    outerHasInnersParents = false;
                                }

                            }
                            if (outerHasInnersParents)
                            {
                                bool exists = CheckExistingLink(outerEntity, innerEntity, e);
                                if (!exists)
                                {
                                    AttributeSetComparer asc = new AttributeSetComparer();
                                    AttributeSetComparer.AttributeSetCompareResult res = asc.Compare(outerEntity, innerEntity);
                                    if (res == AttributeSetComparer.AttributeSetCompareResult.

                                  // 
                                }
                            }*/
                        }




                    }
                }
            }

            foreach (Relation r in relationsToAdd)
            {
                r.AddToCollections();
                e.SafelyAddRelation(r);
            }

        }

        private bool CheckExistingLink(Entity e1, Entity e2, Engine e)
        {
            bool found = false;
            foreach (Relation r in e.GetRelations())
            {
                if (r.From == e1 && r.To == e2)
                {
                    found = true;
                }
            }
            foreach (Relation r in e1.DependentRelations)
            {
                if (r.To is XORNode)
                {
                    ADDNode n = r.To as XORNode;
                    foreach (Relation rX in n.DependentRelations)
                    {
                        if (rX.To == e2)
                        {
                            found = true;
                        }
                    }
                }
            }
            return found;
        }

        private static void AddAugmentationLink(Engine e, List<Relation> relationsToAdd, Entity e1, Entity e2)
        {
            //Console.WriteLine("Picked up an augmentation link between " + e1 + " and " + e2);
            /*  QLink lnkAug = new QLink();
              GraphNode n1 = e1.MyGoObject as GraphNode;
              GraphNode n2 = e2.MyGoObject as GraphNode;

              lnkAug.FromPort = n1.GetDefaultPort;
              lnkAug.ToPort = n2.GetDefaultPort;
              lnkAug.AssociationType = MetaBuilder.Meta.LinkAssociationType.Dependencies;
              e.Diagram.Add(lnkAug);*/

            Relation rAug = new Relation(e1, e2, MetaBuilder.Meta.LinkAssociationType.Dependency, null, false);
            rAug.InferenceType = DependencyType.Augmentative;
            rAug.IsAbstract = true;
            relationsToAdd.Add(rAug);
            //e.relations.Add(rAug);

            //   lnkAug.Pen = new System.Drawing.Pen(System.Drawing.Color.Orange, 3);
        }


        #region Augmentation
        Dictionary<string, List<string>> keySetsAug = new Dictionary<string, List<string>>();
        public void BuildKeysetsAugmentation()
        {
            if (keySetsAug.Count == 0)
            {
                keySetsAug = new Dictionary<string, List<string>>();
                foreach (Relation r in e.GetRelations())
                {
                    if (r.RelationshipType == LinkAssociationType.Dependency)
                    {
                        string dep = GetKeySetAsString(r.To as Entity);
                        string det = GetKeySetAsString(r.From as Entity);
                        if (!keySetsAug.ContainsKey(dep))
                            keySetsAug.Add(dep, new List<string>());
                        keySetsAug[dep].Add(det);
                    }
                }

                foreach (KeyValuePair<string, List<string>> kvp in keySetsAug)
                {
                    //Console.WriteLine(kvp.Key);
                    foreach (string s in kvp.Value)
                    {
                        // Console.WriteLine("\t" + s);
                    }
                }
            }
        }
        public bool DepDetMatch(DependantDeterminant target, DependantDeterminant possible)
        {
            string[] depPossible = possible.Dependant.Split(',');
            string[] detPossible = possible.Determinant.Split(',');

            string dep = target.Dependant.ToLower();
            string det = target.Determinant.ToLower();
            string myChar = "";
            int detCount = 0;
            int depCount = 0;
            for (int i = 0; i < depPossible.Length; i++)
            {
                myChar = depPossible[i].ToLower() + ",";
                if (dep.Contains(myChar))
                {
                    dep = dep.Replace(myChar, "");
                    depCount++;
                }
            }
            for (int i = 0; i < detPossible.Length; i++)
            {
                myChar = detPossible[i].ToLower() + ",";
                if (det.Contains(myChar))
                {
                    det = det.Replace(myChar, "");
                    detCount++;
                }
            }

            if (detCount == detPossible.Length && depCount == depPossible.Length)
            {
                string depLeftOver = dep.Replace(",", "");
                string detLeftOver = det.Replace(",", "");
                //Console.WriteLine("Left Over: [" + depLeftOver + "-->" + detLeftOver + "]");

                if (depLeftOver.Length == detLeftOver.Length)
                {
                    return true;
                }
                else
                {
                    string[] depLOA = dep.Split(',');
                    string[] detLOA = det.Split(',');

                    bool allFound = true;
                    for (int i = 0; i < detLOA.Length; i++)
                    {
                        bool found = false;
                        for (int x = 0; x < depLOA.Length; x++)
                        {
                            if (detLOA[i] == depLOA[x])
                            {
                                found = true;
                            }
                        }

                        if (!found)
                            allFound = false;
                    }

                    if (allFound)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        public bool GetDependency(Entity e1, Entity e2)
        {
            if (keySetsAug == null)
                BuildKeysetsAugmentation();
            if (keySetsAug.Count == 0)
                BuildKeysetsAugmentation();

            string key1 = GetKeySetAsString(e1);
            string key2 = GetKeySetAsString(e2);
            DependantDeterminant matchMe = new DependantDeterminant(key1, key2);
            List<DependantDeterminant> depdats = new List<DependantDeterminant>();
            foreach (KeyValuePair<string, List<string>> kvp in keySetsAug)
            {
                foreach (string s in kvp.Value)
                {
                    DependantDeterminant dd = new DependantDeterminant(s, kvp.Key);
                    depdats.Add(dd);
                    if (DepDetMatch(matchMe, dd))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public class DependantDeterminant
        {
            private string dependant;

            public string Dependant
            {
                get { return dependant; }
                set { dependant = value; }
            }

            private string determinant;

            public string Determinant
            {
                get { return determinant; }
                set { determinant = value; }
            }

            public DependantDeterminant(string dependant, string determinant)
            {
                this.dependant = dependant;
                this.determinant = determinant;
            }


        }
        public string GetKeySetAsString(Entity ent)
        {
            List<string> attributes = new List<string>();
            foreach (Attr a in ent.KeyAttributes)
            {
                attributes.Add(a.Name);
            }
            attributes.Sort();
            StringBuilder sb = new StringBuilder();
            foreach (string s in attributes)
            {
                sb.Append(StripOutBrackets(s.ToLower().Replace(" ", ""))).Append(",");
            }
            string retval = sb.ToString();
            if (retval.Length > 0)
            {
                return retval.Substring(0, retval.Length - 1);
            }
            else
                return string.Empty;
        }

        public string StripOutBrackets(string s)
        {
            if (s.Contains("<"))
            {
                if (s.Contains(">"))
                {
                    return s.Substring(0, s.IndexOf("<"));
                }
                else
                {
                    return s.Substring(0, s.IndexOf("<"));
                }
            }
            else
            {
                return s;
            }
        }
        #endregion
        #endregion
    }
}
