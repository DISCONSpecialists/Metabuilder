using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Meta;
using Northwoods.Go;
using MetaBuilder.Graphing.Shapes;
namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel
{

    /*
     *  public interface ISynthesisItem
    {
        GoObject GraphObject { get;set;}
        ValidationResult Result { get;set;}
    }*/

    public interface IClusterItem
    {
        int ClusterNo { get;set;}
        int ClusterOther { get;set;}
    }

    public class ADDNode
    {
        private bool isSubSetObject;
        public bool IsSubSetObject
        {
            get { return isSubSetObject; }
            set { isSubSetObject = value; }
        }

        private MetaBase artefact; //Can have multiple artefacts!
        public MetaBase Artefact
        {
            get { return artefact; }
            set { artefact = value; }
        }

        public string SubsetNamesAsString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (string s in subsetEntityNames)
                {
                    sb.Append(s).Append(",");
                }
                return sb.ToString().Substring(0, sb.ToString().Length - 1);
            }
        }
        private string selectorAttributeName;
        public string SelectorAttributeName
        {
            get { return selectorAttributeName; }
            set { selectorAttributeName = value; }
        }
        private List<string> subsetEntityNames;
        public List<string> SubsetEntityNames
        {
            get { return subsetEntityNames; }
            set { subsetEntityNames = value; }
        }

        public void StoreSubsetEntityNames()
        {
            subsetEntityNames = new List<string>();
            if (DependentRelations.Count > 0)
            {
                QLink slink = DependentRelations[0].MyGoObject;
                // STORE SELECTOR ATTRIBUTE
                List<IMetaNode> artefacts = slink.GetArtefacts();
                foreach (IMetaNode artNode in artefacts)
                {
                    if (artNode.MetaObject._ClassName == "SelectorAttribute")
                    {
                        //SelectorAttributeName = artNode.MetaObject.Get("Value").ToString();
                        SelectorAttributeName = artNode.MetaObject.ToString();
                    }
                }


                foreach (Relation r in this.OwnerRelations)
                {
                    if (r.From is Entity)
                    {
                        Entity e = r.From as Entity;
                        subsetEntityNames.Add(e.Name);
                    }
                }
            }
        }

        public ADDNode()
        {
            mygoobjects = new List<GoObject>();
        }

        public void AddGoObject(GoObject obj)
        {
            if (!mygoobjects.Contains(obj))
            {
                mygoobjects.Add(obj);
            }
        }
        public bool IsClustered
        {
            get { return Cluster != null; }
        }
        private FEBT.Cluster cluster;
        public FEBT.Cluster Cluster
        {
            get { return cluster; }
            set { cluster = value; }
        }

        private string clusterReason;
        public string ClusterReason
        {
            get { return clusterReason; }
            set { clusterReason = value; }
        }

        private int cycleID;
        public int CycleID
        {
            get { return cycleID; }
            set { cycleID = value; }
        }

        private int cycleIndex;
        public int CycleIndex
        {
            get { return cycleIndex; }
            set { cycleIndex = value; }
        }

        private MetaBase mbase;
        public MetaBase MBase
        {
            get { return mbase; }
            set { mbase = value; }
        }

        private List<GoObject> mygoobjects;
        public List<GoObject> MyGoObjects
        {
            get { return mygoobjects; }
            set { mygoobjects = value; }
        }

        public int UnprocessedDependRelations
        {
            get { return DependentRelations.Count - DependentRelationsProcessed.Count; }
        }
        public int UnprocessedOwnerRelations
        {
            get { return OwnerRelations.Count - OwnerRelationsProcessed.Count; }
        }

        private List<Relation> dependentRelations;
        public List<Relation> DependentRelations
        {
            get { return dependentRelations; }
            set { dependentRelations = value; }
        }

        private List<Relation> ownerRelations;
        public List<Relation> OwnerRelations
        {
            get { return ownerRelations; }
            set
            {
                ownerRelations = value;
            }
        }

        public List<Relation> DependentRelationsProcessed
        {
            get
            {
                List<Relation> retval = new List<Relation>();
                foreach (Relation rel in DependentRelations)
                {
                    if (rel.Processed)
                        retval.Add(rel);
                }
                return retval;
            }
        }
        public List<Relation> OwnerRelationsProcessed
        {
            get
            {
                List<Relation> retval = new List<Relation>();
                foreach (Relation rel in ownerRelations)
                {
                    if (rel.Processed)
                        retval.Add(rel);
                }
                return retval;
            }
        }
    }

    public class XORNode : ADDNode
    {
        public XORNode()
        {
            DependentRelations = new List<Relation>();
            OwnerRelations = new List<Relation>();
        }
        public override string ToString()
        {
            return "XOR Node";
        }
    }

    public class Entity : ADDNode
    {
        public List<Entity> GetAllParents()
        {
            List<Entity> retval = new List<Entity>();
            foreach (Relation relDep in this.DependentRelations)
            {
                if (relDep.To is Entity)
                {
                    Entity entParent = relDep.To as Entity;
                    if (relDep.To != this)
                    {
                        //if (entParent.DependentRelations.Count == 0)
                        {
                            retval.Add(entParent);
                        }
                        // else
                        {
                            retval.AddRange(entParent.GetAllParents());
                            retval.TrimExcess();
                        }
                    }
                }
            }
            List<Entity> trimmedList = new List<Entity>();
            foreach (Entity e in retval)
            {
                if (!trimmedList.Contains(e))
                {
                    trimmedList.Add(e);
                }
            }
            return trimmedList;
        }
        public List<Entity> GetTopParents()
        {
            List<Entity> retval = new List<Entity>();
            foreach (Relation relDep in this.DependentRelations)
            {
                if (relDep.To is Entity)
                {
                    Entity entParent = relDep.To as Entity;
                    if (entParent.DependentRelations.Count == 0)
                    {
                        retval.Add(entParent);
                    }
                    else
                    {
                        retval.AddRange(entParent.GetTopParents());
                        retval.TrimExcess();
                    }
                }
            }
            List<Entity> trimmedList = new List<Entity>();
            foreach (Entity e in retval)
            {
                if (!trimmedList.Contains(e))
                {
                    trimmedList.Add(e);
                }
            }
            return trimmedList;
        }
        public List<Entity> GetParentEntities(bool SkipSubsetLinks)
        {
            List<Entity> retval = new List<Entity>();
            foreach (Relation relDep in this.DependentRelations)
            {
                bool shouldSkip = false;
                if (SkipSubsetLinks)
                {
                    if (relDep.RelationshipType == LinkAssociationType.SubSetOf || relDep.RelationshipType == LinkAssociationType.MutuallyExclusiveLink)
                    {
                        shouldSkip = true;
                    }
                }
                if (relDep.To is Entity && (!shouldSkip))
                {
                    Entity eParent = relDep.To as Entity;
                    retval.Add(eParent);
                    //retval.AddRange(eParent.GetSingleChainParents());
                }
            }
            return retval;
        }
        public List<Entity> GetAllParentEntities(List<Entity> retval)
        {
            if (this is Entity)
            {
                foreach (Relation relDep in this.DependentRelations)
                {
                    if (relDep.RelationshipType == LinkAssociationType.Dependency && (!relDep.IsAbstract))
                    {
                        if (relDep.To is Entity && relDep.To != this)
                        {
                            Entity eParent = relDep.To as Entity;

                            if (!retval.Contains(eParent))
                            {
                                retval.Add(eParent);
                                eParent.GetAllParentEntities(retval);
                            }
                        }
                    }
                }
            }
            return retval;
        }
        public List<Entity> GetAbstractChainParents()
        {
            List<Entity> retval = new List<Entity>();
            foreach (Relation relDep in this.DependentRelations)
            {
                if (relDep.IsAbstract)
                    if (relDep.To is Entity)
                    {
                        Entity eParent = relDep.To as Entity;
                        retval.Add(eParent);
                        retval.AddRange(eParent.GetSingleChainParents());
                    }
            }
            return retval;
        }
        public List<Entity> GetSingleChainParents()
        {
            List<Entity> retval = new List<Entity>();
            if (this.DependentRelations.Count == 1)
            {
                foreach (Relation relDep in this.DependentRelations)
                {
                    if (relDep.To is Entity)
                    {
                        Entity eParent = relDep.To as Entity;
                        retval.Add(eParent);
                        retval.AddRange(eParent.GetSingleChainParents());
                    }
                }
            }
            return retval;
        }
        public bool HasOnlyOneConcreteParent
        {
            get
            {
                int counter = 0;
                foreach (Relation relDep in this.DependentRelations)
                {
                    if (!relDep.IsAbstract)
                        counter++;
                }
                return counter == 1;

            }
        }
        public bool HasOneGrandParent
        {
            get
            {
                int GrandParentCount = 0;
                foreach (Relation relDep in this.DependentRelations)
                {
                    if (relDep.To is Entity)
                    {
                        Entity parent = relDep.To as Entity;
                        foreach (Relation relDepParent in parent.DependentRelations)
                        {
                            if (relDepParent.To is Entity && (!relDepParent.IsAbstract))
                            {
                                GrandParentCount++;
                            }
                        }
                    }
                }
                return GrandParentCount == 1;
            }
        }
        public Entity GetSingleSuperSuper()
        {
            Entity retval = null;
            int counter = 0;
            foreach (Relation relDep in this.DependentRelations)
            {
                if (relDep.IsAbstract)
                {
                    // Do nothing
                }
                else
                {
                    if (relDep.RelationshipType == LinkAssociationType.Dependency)
                    {
                        if (relDep.To is Entity)
                        {
                            Entity parent = relDep.To as Entity;
                            counter++;
                            int concretes = 0;
                            foreach (Relation rel in parent.DependentRelations)
                            {
                                if (!rel.IsAbstract)
                                {
                                    if (rel.RelationshipType == LinkAssociationType.Dependency)
                                        concretes++;
                                }

                            }
                            if (concretes == 1)
                            {
                                if (parent.DependentRelations[0].To is Entity)
                                    retval = parent.DependentRelations[0].To as Entity;
                            }
                        }
                    }
                }
            }
            if (counter == 1)
                return retval;
            return null;
        }
        public Entity(string name)
        {
            this.Name = name;
            KeyAttributes = new List<Attr>();
            Attributes = new List<Attr>();
            DependentRelations = new List<Relation>();
            OwnerRelations = new List<Relation>();
        }
        #region Properties
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private List<Attr> keyAttributes;
        public List<Attr> KeyAttributes
        {
            get { return keyAttributes; }
            set { keyAttributes = value; }
        }

        private List<Attr> attributes;
        public List<Attr> Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }
        public override string ToString()
        {
            if (Name != null)
                return Name;
            else
                return base.ToString();
        }

        #endregion
    }

    public class ResolvedKeyAttributeSet
    {
        Entity child;
        Entity parent;
        List<Attr> attributes;
        public ResolvedKeyAttributeSet(Entity c, Entity p)
        {
            child = c;
            parent = p;
            attributes = new List<Attr>();
        }
    }
    public class Attr
    {
        #region Properties
        private List<Entity> determinedBy;

        public List<Entity> DeterminedBy
        {
            get { return determinedBy; }
            set { determinedBy = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        #endregion
        public Attr(string name)
        {
            this.Name = name;
            DeterminedBy = new List<Entity>();
        }

        private string key;
        public string Key
        {
            get { return key; }
            set { key = value; }
        }

    }

    public class Relation
    {
        public MetaBase SelectorAttribute
        {
            get
            {
                QLink slink = MyGoObject;
                // STORE SELECTOR ATTRIBUTE
                List<IMetaNode> artefacts = slink.GetArtefacts();
                foreach (IMetaNode artNode in artefacts)
                {
                    if (artNode.MetaObject._ClassName == "SelectorAttribute")
                    {
                        return artNode.MetaObject;
                    }
                }
                return null;
            }
        }
        private bool isAbstract;

        public bool IsAbstract
        {
            get { return isAbstract; }
            set { isAbstract = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private bool processed;
        public bool Processed
        {
            get { return processed; }
            set { processed = value; }
        }

        #region Properties
        private ADDNode from;
        public ADDNode From
        {
            get { return from; }
            set { from = value; }
        }
        private ADDNode to;
        public ADDNode To
        {
            get { return to; }
            set { to = value; }
        }
        private LinkAssociationType relationshipType;
        public LinkAssociationType RelationshipType
        {
            get { return relationshipType; }
            set { relationshipType = value; }
        }
        #endregion

        public Relation(ADDNode from, ADDNode to, LinkAssociationType relType, GoObject myGoObject, bool AddToCollection)
        {
            inferenceType = DependencyType.NotSpecified;
            From = from;
            To = to;
            this.RelationshipType = relType;
            if (relType == LinkAssociationType.SubSetOf || relType == LinkAssociationType.MutuallyExclusiveLink)
            {
                from.IsSubSetObject = true;
            }

            if (AddToCollection)
            {
                AddToCollections();
            }
            this.MyGoObject = myGoObject as QLink;
            this.inferenceType = DependencyType.NotSpecified;
        }
        public void AddToCollections()
        {
            From.DependentRelations.Add(this);
            To.OwnerRelations.Add(this);
        }
        private QLink myGoObject;

        public QLink MyGoObject
        {
            get { return myGoObject; }
            set { myGoObject = value; }
        }

        private DependencyType inferenceType;
        public DependencyType InferenceType
        {
            get { return inferenceType; }
            set { inferenceType = value; }
        }
        private double weight;
        public double Weight
        {
            get { return weight; }
            set { weight = value; }
        }

    }

    public class SelectorAttribute : ADDNode
    {
        public SelectorAttribute(string name)
        {
            DependentRelations = new List<Relation>();
            OwnerRelations = new List<Relation>();
            this.Name = name;

        }
        public override string ToString()
        {
            if (Name != null)
                return "SelectorAttribute: " + Name;
            else
                return base.ToString();
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }

    public enum DependencyType
    {
        NotSpecified, Ordinary, Transitive, Reflexive, Augmentative
    }

    public class AttributeSetComparer
    {
        public enum AttributeSetCompareResult
        {
            NA, Determines, DeterminedBy, NoMatch, Overlap
        }

        public bool EligibleForAugmentation(Entity e1, Entity e2, Engine e)
        {

            return true;
            /*
            List<Attr> foundAttributes = new List<Attr>();
            int found1 = 0;
            int found2 = 0;
            
            foreach (Attr a1 in e1.KeyAttributes)
            { 
                //if (a1.DeterminedBy.Count==0)
                {
                    bool found = false;
                    foreach (Attr a2 in e2.KeyAttributes)
                    {
                        //if (r.RelationshipType == LinkAssociationType.Dependencies)
                        {

                            if (a1.Name == a2.Name)
                            {
                                found = true;
                                foundAttributes.Add(a2);
                            }
                        }

                       // if (r.RelationshipType == LinkAssociationType.SubSetOf)
                        {
                            if (a2.Name.StartsWith(a1.Name))
                            {
                                found = true;
                                foundAttributes.Add(a2);
                            }
                        }

                    }

                    if (!found)
                        someNotFound = true;
                    else
                        someFound = true;
                }

            }

            if (someFound && someNotFound)
            {
                retval = AttributeSetCompareResult.Overlap;
            }

            if (!someNotFound && e1.KeyAttributes.Count > e2.KeyAttributes.Count)// ALL FOUND but list1 has more items
                retval = AttributeSetCompareResult.DeterminedBy;

            if (!someNotFound && e1.KeyAttributes.Count <= e2.KeyAttributes.Count)// ALL FOUND but list2 has more items
            {
                retval = AttributeSetCompareResult.Determines;
                foreach (Attr a2 in foundAttributes)
                {
                    a2.DeterminedBy.Add(e2);
                }
            }



            if (someNotFound && (!someFound))
                retval = AttributeSetCompareResult.NoMatch;

            Console.WriteLine(retval);
            return retval;*/
        }
        public AttributeSetCompareResult Compare(Entity e1, Entity e2, Relation r)
        {
            //Console.WriteLine("Comparing\t" + e1.ToString() + "\t" + e2.ToString());
            bool someNotFound = false;
            bool someFound = false;

            bool? e1Prioritised = null;
            if (e1.CycleID < e2.CycleID)
            {
                e1Prioritised = true;
            }
            else if (e1.CycleID == e2.CycleID && e1.CycleIndex < e2.CycleIndex)
            {
                e1Prioritised = true;
            }

            if (!e1Prioritised.HasValue)
            {
                e1Prioritised = false;
            }

            AttributeSetCompareResult retval = AttributeSetCompareResult.NA;
            List<Attr> foundAttributes = new List<Attr>();
            foreach (Attr a1 in e1.KeyAttributes)
            {
                //if (a1.DeterminedBy.Count==0)
                {
                    bool found = false;
                    foreach (Attr a2 in e2.KeyAttributes)
                    {
                        if (r.RelationshipType == LinkAssociationType.Dependency)
                        {

                            if (a1.Name == a2.Name)
                            {
                                found = true;
                                foundAttributes.Add(a2);
                            }
                        }

                        if (r.RelationshipType == LinkAssociationType.SubSetOf)
                        {
                            if (a2.Name.StartsWith(a1.Name))
                            {
                                found = true;
                                foundAttributes.Add(a2);
                            }
                        }

                    }

                    if (!found)
                        someNotFound = true;
                    else
                        someFound = true;
                }

            }

            if (someFound && someNotFound)
            {
                retval = AttributeSetCompareResult.Overlap;
            }

            if (!someNotFound && e1.KeyAttributes.Count > e2.KeyAttributes.Count)// ALL FOUND but list1 has more items
                retval = AttributeSetCompareResult.DeterminedBy;

            if (!someNotFound && e1.KeyAttributes.Count <= e2.KeyAttributes.Count)// ALL FOUND but list2 has more items
            {
                retval = AttributeSetCompareResult.Determines;
                foreach (Attr a2 in foundAttributes)
                {
                    a2.DeterminedBy.Add(e2);
                }
            }



            if (someNotFound && (!someFound))
                retval = AttributeSetCompareResult.NoMatch;

            //Console.WriteLine(retval);
            return retval;


        }

    }
}