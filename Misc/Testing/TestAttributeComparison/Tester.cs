using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.UIControls.GraphingUI.Tools.DataModel;
using MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation;
using MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Inference;
using MetaBuilder.Meta;
namespace TestAttributeComparison
{
    public class Tester
    {
        public bool ReflexivityMatch(List<Attr> FirstEntity, List<Attr> SecondEntity)
        {

        }

        public bool DepDetMatch(DependantDeterminant target, DependantDeterminant possible)
        {
            string[] depPossible = possible.Dependant.Split(',');
            string[] detPossible = possible.Determinant.Split(',');

            string dep = target.Dependant.ToLower() + ",";
            string det = target.Determinant.ToLower() + ",";
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

            if (detCount == depCount)
            {
                string depLeftOver = dep.Replace(",", "");
                string detLeftOver = det.Replace(",", "");
                //Console.WriteLine("Left Over: [" + depLeftOver + "-->" + detLeftOver + "]");

                if (depLeftOver == detLeftOver)
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
                sb.Append(s).Append(",");
            }
            string retval = sb.ToString();
            return retval.Substring(0, retval.Length - 1);
        }
        public void GetDependency(Entity e1, Entity e2)
        {
            
            string key1 = GetKeySetAsString(e2);
            string key2 = GetKeySetAsString(e1);
            DependantDeterminant matchMe = new DependantDeterminant(key1, key2);
            List<DependantDeterminant> depdats = new List<DependantDeterminant>();
            foreach (KeyValuePair<string, List<string>> kvp in keySets)
            {
                foreach (string s in kvp.Value)
                {
                    DependantDeterminant dd = new DependantDeterminant(kvp.Key,s);
                    depdats.Add(dd);
                    if (DepDetMatch(matchMe, dd))
                    {
                        // do nothing
                    }
                }
            }

        }

        public Tester()
        {

            BuildKeySets();
            Entity xz = new Entity("XZ");
            xz.KeyAttributes.Add(new Attr("Z"));
            xz.KeyAttributes.Add(new Attr("X"));
            xz.KeyAttributes.Add(new Attr("C"));

            Entity yz = new Entity("YZ");
            yz.KeyAttributes.Add(new Attr("y"));
            yz.KeyAttributes.Add(new Attr("z"));

            Entity yc = new Entity("YC");
            yc.KeyAttributes.Add(new Attr("y"));
            yc.KeyAttributes.Add(new Attr("c"));

            

            GetDependency(xz, yz);
            GetDependency(xz, yc);
            GetDependency(yc, yz);
            GetDependency(yc, xz);
            GetDependency(yz, xz);
            GetDependency(yz, yc);
            Console.ReadLine();
            return;


            /*

            List<Entity> entities = new List<Entity>();
            Entity e1 = new Entity("Order");
            e1.Attributes.Add(new Attr("OrderID"));
            e1.Attributes.Add(new Attr("CustNo"));
            e1.CycleID = 0;
            e1.CycleIndex = 2;
            

            Entity e2 = new Entity("Item");
            e2.Attributes.Add(new Attr("ItemNo"));
            e2.CycleID = 0;
            e2.CycleIndex = 0;


            Entity e3 = new Entity("Customer");
            e3.Attributes.Add(new Attr("CustNo"));
            e3.CycleID = 0;
            e3.CycleIndex = 1;

            Entity e4 = new Entity("OrderItem");
            e4.Attributes.Add(new Attr("OrderID"));
            e4.Attributes.Add(new Attr("CustNo"));
            e4.Attributes.Add(new Attr("ItemNo"));
            e4.CycleIndex = 0;
            e4.CycleID = 1;



            
            Entity e5 = new Entity("OrderItemPart");
            e5.Attributes.Add(new Attr("OrderID"));
            e5.Attributes.Add(new Attr("CustNo"));
            e5.Attributes.Add(new Attr("ItemNo"));
            e5.Attributes.Add(new Attr("PartNo"));
            e5.CycleIndex = 0;
            e5.CycleID = 2;


            Entity e6 = new Entity("Part");
            e6.Attributes.Add(new Attr("PartNo"));
            e6.CycleIndex = 0;
            e6.CycleID = 3;




            Relation r1 = new Relation(e4, e1, LinkAssociationType.Dependencies, null);
            Relation r2 = new Relation(e4, e2, LinkAssociationType.Dependencies, null);
            Relation r3 = new Relation(e1, e3, LinkAssociationType.Dependencies, null);
            Relation r4 = new Relation(e5, e6, LinkAssociationType.Dependencies, null);
            Relation r5 = new Relation(e5, e4, LinkAssociationType.Dependencies, null);
            List<Relation> relations = new List<Relation>();
            relations.Add(r1);
            relations.Add(r2);
            relations.Add(r3);
            relations.Add(r4);
            relations.Add(r5);

            relations.Sort(new RelationSorter());
            AttributeSetComparer asc = new AttributeSetComparer();
            foreach (Relation r in relations)
            {
                asc.Compare(r.To as Entity, r.From as Entity);
            }
            Console.ReadLine();
          
            EntitySorter es = new EntitySorter();
            entities.Add(e1);
            entities.Add(e2);
            entities.Add(e3);
            entities.Add(e4);

            entities.Sort(new EntitySorter());

            foreach (Entity e in entities)
            {
                foreach (Entity ent2 in entities)
                {
                    if (e != ent2)
                    {
                        asc.Compare(e, ent2);
                    }
                }
            }*/
          /*  asc.Compare(e1, e2);
            asc.Compare(e1, e3);
            asc.Compare(e1, e4);

            asc.Compare(e2, e1);
            asc.Compare(e2, e3);
            asc.Compare(e2, e4);

            asc.Compare(e3, e1);
            asc.Compare(e3, e2);
            asc.Compare(e3, e4);

            asc.Compare(e4, e1);
            asc.Compare(e4, e2);
            asc.Compare(e4, e3);*/

            
            Console.ReadLine();


        }
        Dictionary<string, List<string>> keySets = new Dictionary<string, List<string>>();
        public void BuildKeySets()
        {
 
                keySets = new Dictionary<string, List<string>>();
                keySets.Add("so#", new List<string>());
                keySets["so#"].Add("cd#");
                keySets["so#"].Add("so#,i#");
                
                
                keySets.Add("cd#", new List<string>());
                keySets["cd#"].Add("i#,cd#");


                keySets.Add("so#,i#", new List<string>());
                keySets[""].Add("i#,cd#");
                
                




             

               /* foreach (KeyValuePair<string, List<string>> kvp in keySets)
                {
                    Console.WriteLine(kvp.Key);
                    foreach (string s in kvp.Value)
                    {
                        Console.WriteLine("\t" + s);
                    }
                }*/
            
        }
    }


    public class RelationSorter : IComparer<Relation>
    {
        public int Compare(Relation r1, Relation r2)
        {
            int r1CycleIndices = r1.To.CycleIndex + r1.From.CycleIndex;
            int r1CycleIDs = r1.To.CycleID + r1.From.CycleID;

            int r2CycleIndices = r2.To.CycleIndex + r2.From.CycleIndex;
            int r2CycleIDs = r2.To.CycleID + r2.From.CycleID;

            //return y.Freq - x.Freq; //descending sort
            if (r1CycleIDs < r2CycleIDs)
            {
                return -1;
            }
            if (r1CycleIndices < r2CycleIndices && r1CycleIDs == r2CycleIDs)
            {
                return -1;
            }
            if (r1CycleIDs == r2CycleIDs && r1CycleIndices == r2CycleIndices)
            {
                return -0;
            }
            return 1;
        }
    }
        public class EntitySorter : IComparer<Entity>
        {
            #region IComparer<Item> Members

            public int Compare(Entity x, Entity y)
            {

                //return y.Freq - x.Freq; //descending sort
                if (x.CycleID < y.CycleID)
                {
                    return -1;
                }
                if (x.CycleIndex < y.CycleIndex && x.CycleID == y.CycleID)
                {
                    return -1;
                }
                if (x.CycleID == y.CycleID && x.CycleIndex == y.CycleIndex)
                {
                    return -0;
                }
                return 1;
            }

            #endregion
        }

    

}
