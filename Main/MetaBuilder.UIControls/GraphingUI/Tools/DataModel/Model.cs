using System;
using System.Collections.Generic;
using System.Text;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel
{
    public class Model
    {
        List<ADDNode> entities;
        List<ADDNode> Entities
        {
            get { return entities; }
            set { entities = value; }
        }
        List<Relation> relationships;
        List<Relation> Relationships
        {
            get { return relationships; }
            set { relationships = value; }
        }

        public Model(List<ADDNode> entities, List<Relation> relationships)
        {
            this.Entities = entities;
            this.Relationships = relationships;
        }

        public void CheckRelationships()
        {
            foreach (Entity e in Entities)
            {

            }
        }
        private int EntityCount
        {
            get
            {
                int counter = 0;
                foreach (ADDNode n in Entities)
                {
                    //  if (n is Entity)
                    {
                        counter++;
                    }
                }
                return counter;
            }
        }

        List<ADDNode> Processed;
        List<ADDNode> NotProcessed;
        public void CycleAll()
        {
            counterx = 0;
            Processed = new List<ADDNode>();
            NotProcessed = new List<ADDNode>();
            foreach (ADDNode e in Entities)
            {
                NotProcessed.Add(e);
            }

            while (Processed.Count < EntityCount && counterx < 100)
            {
                Cycle();
            }
        }
        public void Cycle()
        {

            int indexOfCycle = 0;
            //Console.WriteLine(NotProcessed.Count.ToString());
            foreach (ADDNode addnode in NotProcessed)
            {
                if (addnode.UnprocessedDependRelations == 0)
                {

                    while (addnode.UnprocessedOwnerRelations > 0)
                    {
                        //r.From.DependentRelations.Remove(r);
                        //RemoveRelationship(addnode, addnode.OwnerRelations[0], false);
                        foreach (Relation rnp in addnode.OwnerRelations)
                        {
                            rnp.Processed = true;
                        }
                    }
                    addnode.CycleID = counterx;
                    addnode.CycleIndex = indexOfCycle;
                    Processed.Add(addnode);
                    indexOfCycle++;
                }

            }

            foreach (ADDNode e in Processed)
            {
                if (NotProcessed.Contains(e))
                    NotProcessed.Remove(e);
            }
            counterx++;
            //Console.WriteLine(NotProcessed.Count.ToString() + " remaining");

        }
        int counterx = 100;
        void RemoveRelationship(ADDNode node, Relation r, bool dependent)
        {


            /*
            while (x < list.Count)
            {
                Relation rel = list[x];
                x++;
              
                {
                   // list.Remove(rel);
                    if (!dependent)
                    {
                        Console.WriteLine("Rem <From>:" + rel.From.ToString());
                        rel.Processed = true;
                        ProcessedCount = node.OwnerRelationsProcessed.Count ;
                        //rel.From.DependentRelations.Remove(rel);
                    }
                    else
                    {
                        Console.WriteLine("Rem <To>:" + rel.To.ToString());
                        rel.Processed = true;
                        ProcessedCount = node.DependentRelationsProcessed.Count;
                        //rel.To.OwnerRelations.Remove(rel);
                    }
                }
             
            }
             * */
        }
        public void Reset()
        {
            Processed = null;
            NotProcessed = null;
        }

        public void ListKeyAttributeSets()
        {
            foreach (ADDNode e in entities)
            {
                List<Attribute> keyAttribs = new List<Attribute>();
            }
        }

        public bool CompareAttributeSet(List<Attribute> A, List<Attribute> B)
        {
            if (A.Count != B.Count)
            {
                return false;
            }
            bool foundAllA = true;
            foreach (Attribute a in A)
            {
                bool foundA = false;
                foreach (Attribute b in B)
                {
                    if (a == b)
                        foundA = true;
                }
                if (!foundA)
                    foundAllA = false;
            }
            bool foundAllB = true;
            foreach (Attribute b in B)
            {
                bool foundB = false;
                foreach (Attribute a in A)
                {
                    if (a == b)
                        foundB = true;
                }
                if (!foundB)
                    foundAllB = false;
            }

            if (foundAllA && foundAllB)
            {
                return true;
            }
            return false;
        }
    }

    public class AttributeSet
    {
        List<Attribute> Attributes;
        public AttributeSet()
        {
            Attributes = new List<Attribute>();
        }
        public AttributeSet(List<Attribute> attributes)
        {
            Attributes = attributes;
        }
    }
}