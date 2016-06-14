using System;
using System.Collections.Generic;
using System.Text;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.FEBT
{
    public class PatternFinder
    {
        public MetaBuilder.Graphing.Containers.GraphView MyView;
        List<Cluster> clusters;
        public List<Cluster> FoundClusters
        {
            get { return clusters; }
        }
        public Engine MyEngine
        {
            get { return e; }
        }
        Engine e;
        public PatternFinder(Engine e, MetaBuilder.Graphing.Containers.GraphView view)
        {
            this.MyView = view;
            this.e = e;
            foreach (ADDNode node in e.addnodes)
            {
                node.Cluster = null;
                node.ClusterReason = "";
            }
            clusters = new List<Cluster>();
            MultilevelHierarchy mh = new MultilevelHierarchy();
            clusters = mh.Search(e);
            Console.WriteLine("Found:" + clusters.Count.ToString() + " MultilevelHierarchy's");
            int MLHcount = clusters.Count;

            SingleLevelHierarchy shl = new SingleLevelHierarchy();
            clusters.AddRange(shl.Search(e));
            Console.WriteLine("Found:" + (clusters.Count - MLHcount).ToString() + " SingleLevelHierarchy's");
            MLHcount = clusters.Count;

            MultilevelNetwork mlnw = new MultilevelNetwork();
            clusters.AddRange(mlnw.Search(e));
            Console.WriteLine("Found:" + (clusters.Count - MLHcount).ToString() + " MultilevelNetwork's");
            MLHcount = clusters.Count;

            SingleLevelNetwork shnw = new SingleLevelNetwork();
            //TODO : BUG?!
            clusters.AddRange(shnw.Search(e));
            Console.WriteLine("Found:" + (clusters.Count - MLHcount).ToString() + " SingleLevelNetwork's");
            MLHcount = clusters.Count;

            SubsetsAndXors ssxors = new SubsetsAndXors();
            clusters.AddRange(ssxors.Search(e));
            Console.WriteLine("Found:" + (clusters.Count - MLHcount).ToString() + " SubsetsAndXors's");
            MLHcount = clusters.Count;

            LinksWithWeightingOf1 links1 = new LinksWithWeightingOf1();
            links1.clusteredOnly = true;
            clusters.AddRange(links1.Search(e));
            Console.WriteLine("Found:" + (clusters.Count - MLHcount).ToString() + " CLUSTERED LinksWithWeightingOf1's");
            MLHcount = clusters.Count;

            links1.clusteredOnly = false;
            clusters.AddRange(links1.Search(e));
            Console.WriteLine("Found:" + (clusters.Count - MLHcount).ToString() + " ALL LinksWithWeightingOf1's");
            MLHcount = clusters.Count;

            links1.clusteredOnly = true;
            //clusters.AddRange(links1.Search(e));
            //clusters.AddRange(ssxors.Search(e));

            LinksWithWeightingOfHalve halfLinks = new LinksWithWeightingOfHalve();
            clusters.AddRange(halfLinks.Search(e));
            Console.WriteLine("Found:" + (clusters.Count - MLHcount).ToString() + " LinksWithWeightingOfHalve's");
            MLHcount = clusters.Count;

            Standalone salone = new Standalone();
            clusters.AddRange(salone.Search(e));
            Console.WriteLine("Found:" + (clusters.Count - MLHcount).ToString() + " Standalone's");
            MLHcount = clusters.Count;

            List<Cluster> removedDuplicates = new List<Cluster>();
            foreach (Cluster c in clusters)
            {
                if (!removedDuplicates.Contains(c))
                    removedDuplicates.Add(c);
            }

#if DEBUG
            foreach (Cluster c in removedDuplicates)
            {
                Console.WriteLine("\tCluster:\t" + c.Name + " " + c.Description);
                foreach (ADDNode node in c.Nodes)
                {
                    Console.WriteLine("\t\t" + node.ToString() + "\t" + node.ClusterReason);
                }
            }
#endif
            clusters = removedDuplicates;
            List<Cluster> emptyClusters = new List<Cluster>();
            foreach (Cluster c in clusters)
            {
                if (c.Nodes.Count == 0)
                {
                    emptyClusters.Add(c);
                }
            }
            for (int i = 0; i < emptyClusters.Count; i++)
            {
                clusters.Remove(emptyClusters[i]);
            }
        }
    }

    public abstract class BaseFinder
    {
        public void AddOnesToExistingCluster(ADDNode node, Cluster c, string ClusterReason)
        {
            foreach (Relation r in node.OwnerRelations)
            {
                if (r.RelationshipType == MetaBuilder.Meta.LinkAssociationType.Dependency)
                {
                    if (r.Weight == 1)
                    {
                        ADDNode nChild = r.From as ADDNode;
                        if (nChild.Cluster == null)
                        {
                            this.AddToCluster(nChild, c, ClusterReason);
                        }
                        else
                        {
                            this.MoveToCluster(nChild.Cluster, c);
                        }
                    }
                }
            }
            foreach (Relation r in node.DependentRelations)
            {
                if (r.RelationshipType == MetaBuilder.Meta.LinkAssociationType.Dependency)
                {
                    if (r.Weight == 1)
                    {
                        ADDNode nChild = r.To as ADDNode;
                        if (nChild.Cluster == null)
                        {
                            this.AddToCluster(nChild, c, ClusterReason);
                        }
                        else
                        {
                            this.MoveToCluster(nChild.Cluster, c);
                        }
                    }
                }
            }
        }
        public void AddToCluster(ADDNode n, Cluster c, string ClusterReason)
        {
            if (n.Cluster == null)
            //{
            //    Console.WriteLine("Error - already clustered");
            //}
            //else
            {
                n.Cluster = c;
                if (!c.Nodes.Contains(n))
                {
                    c.Nodes.Add(n);
                    n.ClusterReason = ClusterReason;
                    AddOnesToExistingCluster(n, c, ClusterReason);
                }
            }

        }
        public void MoveToCluster(Cluster cBeingMoved, Cluster cTarget)
        {
            if (cBeingMoved != cTarget)
            {
                foreach (ADDNode node in cBeingMoved.Nodes)
                {
                    node.Cluster = cTarget;
                    cTarget.Nodes.Add(node);
                }
                cBeingMoved.Nodes.Clear();
            }
        }
    }

    public interface IEntityPattern
    {
        string Name { get;set;}
        List<Entity> Entities { get;set;}
        List<Relation> Relations { get;set;}
        Entity startFrom { get;set;}
        bool IsValid { get;}
    }

    public class Cluster
    {
        public Cluster()
        {
            Nodes = new List<ADDNode>();
        }

        public int Priority;

        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private List<ADDNode> nodes;
        public List<ADDNode> Nodes
        {
            get { return nodes; }
            set { nodes = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private bool isValid;
        public bool IsValid
        {
            get { return isValid; }
            set { isValid = value; }
        }

        public int OutGoingLinksCount
        {
            get
            {
                return OutGoingLinks.Count;
            }
        }

        public List<ClusterLink> OutgoingClusterLinks
        {
            get
            {
                List<ClusterLink> retval = new List<ClusterLink>();
                List<Relation> outgoings = OutGoingLinks;
                foreach (Relation r in outgoings)
                {
                    if (r.To.Cluster != this)
                    {
                        bool found = false;
                        foreach (ClusterLink clink in retval)
                        {
                            if (clink.To == r.To.Cluster)
                            {
                                found = true;
                            }
                        }
                        if (!found)
                        {
                            ClusterLink lnk = new ClusterLink();
                            lnk.From = this;
                            lnk.To = r.To.Cluster;

                            retval.Add(lnk);
                        }
                    }
                }
                return retval;

            }
        }

        public List<Relation> OutGoingLinks
        {
            get
            {
                List<Relation> retval = new List<Relation>();
                foreach (ADDNode n in nodes)
                {
                    foreach (Relation r in n.DependentRelations)
                    {
                        if (!this.Nodes.Contains(r.To))
                        {
                            retval.Add(r);
                        }
                    }
                }
                return retval;
            }
        }
    }

    public class ClusterLink
    {
        public Cluster From;
        public Cluster To;
    }

    public class LinksWithWeightingOfHalve : BaseFinder
    {
        public List<Cluster> Search(Engine e)
        {
            List<Cluster> retval = new List<Cluster>();

            // loop through all the add nodes
            foreach (ADDNode basenode in e.addnodes)
            {
                Cluster c = new Cluster();

                //int nodeCount = 0;
                if (basenode.Cluster != null)
                {
                    string ClusterReason = "Links with 0.5 Weighting (Parent: " + basenode.MBase.ToString() + ")";
                    foreach (Relation r in basenode.OwnerRelations)
                    {
                        //ADDNode nodeTo = r.From as ADDNode;
                        if (r.From.Cluster == null)
                        {
                            //List<Relation> outers = nodeTo.OwnerRelations;
                            //List<Relation> inners = nodeTo.DependentRelations;

                            if (c.Nodes.Count == 0)
                                c = AddHalveNodes(retval, c, r.From, r.From.OwnerRelations, ClusterReason);

                            if (c.Nodes.Count == 0)
                                c = AddHalveNodes(retval, c, r.From, r.From.DependentRelations, ClusterReason);
                        }
                    }

                    foreach (Relation r in basenode.DependentRelations)
                    {
                        //ADDNode nodeTo = r.To as ADDNode;
                        if (r.To.Cluster == null)
                        {
                            //List<Relation> outers = nodeTo.OwnerRelations;
                            //List<Relation> inners = nodeTo.DependentRelations;

                            if (c.Nodes.Count == 0)
                                c = AddHalveNodes(retval, c, r.To, r.To.OwnerRelations, ClusterReason);

                            if (c.Nodes.Count == 0)
                                c = AddHalveNodes(retval, c, r.To, r.To.DependentRelations, ClusterReason);
                        }
                    }
                }

                if (c.Nodes.Count > 1)
                {
                    retval.Add(c);
                }
            }

            return retval;
        }

        private Cluster AddHalveNodes(List<Cluster> retval, Cluster c, ADDNode node, List<Relation> relations, string ClusterReason)
        {
            foreach (Relation rOuter in relations)
            {
                if (rOuter.Weight == 0.5d)
                {
                    ADDNode nOtherSide = rOuter.From;

                    if (nOtherSide == node)
                    {
                        nOtherSide = rOuter.To;
                    }

                    if (nOtherSide.Cluster == null && node.Cluster == null)
                    {
                        if (rOuter.InferenceType == DependencyType.Reflexive)
                        {
                            this.AddToCluster(node, c, ClusterReason);
                            this.AddToCluster(nOtherSide, c, ClusterReason);
                        }
                        else
                        {
                            this.AddToCluster(node, c, ClusterReason);
                            this.AddToCluster(nOtherSide, c, ClusterReason);
                        }
                        retval.Add(c);
                        c = new Cluster();
                    }
                    else
                    {
                        if (rOuter.InferenceType == DependencyType.Reflexive)
                        {
                            if (rOuter.From.Cluster == null)
                            {
                                this.AddToCluster(rOuter.From, rOuter.To.Cluster, ClusterReason);
                                this.AddToCluster(rOuter.To, rOuter.To.Cluster, ClusterReason);
                                retval.Add(rOuter.To.Cluster);
                            }
                            else if (rOuter.To.Cluster == null)
                            {
                                //rOuter.To.Cluster.ToString();
                            }
                        }
                        c = new Cluster();
                    }
                }
            }
            return c;
        }
    }

    public class Standalone : BaseFinder
    {
        public List<Cluster> Search(Engine e)
        {
            List<Cluster> retval = new List<Cluster>();
            foreach (ADDNode n in e.addnodes)
            {
                if (n is Entity)
                    if (n.Cluster == null)
                    {
                        Cluster c = new Cluster();
                        AddToCluster(n, c, "Standalone");
                        retval.Add(n.Cluster);
                    }
            }
            return retval;
        }
    }

    public class LinksWithWeightingOf1 : BaseFinder
    {
        public bool? clusteredOnly;
        public List<Cluster> Search(Engine e)
        {
            return DoSearch(e, clusteredOnly);
        }

        private List<Cluster> DoSearch(Engine e, bool? clusteredOnly)
        {

            List<Cluster> retval = new List<Cluster>();

            // loop through all the add nodes
            foreach (ADDNode node in e.addnodes)
            {
                Cluster c = new Cluster();
                bool canContinue = true;
                string ClusterReason = "Links with 1 Weighting (Parent: " + node.MBase.ToString() + ")";
                if (clusteredOnly.HasValue)
                {

                    if (node.Cluster == null && clusteredOnly.Value)
                    {
                        canContinue = false;
                    }

                    if (node.Cluster == null && (!clusteredOnly.Value))
                        canContinue = true;

                    if (node.Cluster != null && clusteredOnly.Value)
                        canContinue = true;

                    if (canContinue)
                    {
                        if (node.Cluster != null && clusteredOnly.Value)
                        {
                            AddOnesToExistingCluster(node, c, ClusterReason);
                        }

                        if (node.Cluster == null && (!clusteredOnly.Value))
                        {
                            TryAndGetNeighbouringClusters(node, ClusterReason);
                        }

                    }

                    if (c.Nodes.Count > 1)
                    {
                        retval.Add(c);
                    }

                }


            }

            return retval;
        }

        private void TryAndGetNeighbouringClusters(ADDNode node, string ClusterReason)
        {
            Cluster c = node.Cluster;
            if (c != null)
            {
                foreach (Relation r in node.DependentRelations)
                {
                    if (r.RelationshipType == MetaBuilder.Meta.LinkAssociationType.Dependency)
                    {
                        if (r.Weight == 1)
                        {
                            ADDNode nChild = r.To as ADDNode;
                            if (nChild.Cluster == null)
                            {
                                this.AddToCluster(node, nChild.Cluster, ClusterReason);
                            }
                            else
                            {
                                this.MoveToCluster(nChild.Cluster, c);
                            }

                        }
                    }
                }


                foreach (Relation r in node.OwnerRelations)
                {
                    if (r.RelationshipType == MetaBuilder.Meta.LinkAssociationType.Dependency)
                    {
                        if (r.Weight == 1)
                        {
                            ADDNode nChild = r.From as ADDNode;
                            if (nChild.Cluster == null)
                            {
                                this.AddToCluster(node, nChild.Cluster, ClusterReason);
                            }
                            else
                            {
                                this.MoveToCluster(nChild.Cluster, c);
                            }

                        }
                    }
                }
            }
            else
            {
                foreach (Relation r in node.DependentRelations)
                {
                    if (r.RelationshipType == MetaBuilder.Meta.LinkAssociationType.Dependency)
                    {
                        if (r.Weight == 1)
                        {
                            ADDNode nChild = r.To as ADDNode;
                            if (nChild.Cluster != null)
                            {
                                this.AddToCluster(node, nChild.Cluster, ClusterReason);
                            }

                        }
                    }
                }


                foreach (Relation r in node.OwnerRelations)
                {
                    if (r.RelationshipType == MetaBuilder.Meta.LinkAssociationType.Dependency)
                    {
                        if (r.Weight == 1)
                        {
                            ADDNode nChild = r.From as ADDNode;
                            if (nChild.Cluster != null)
                            {
                                this.AddToCluster(node, nChild.Cluster, ClusterReason);
                            }

                        }
                    }
                }
            }
        }
    }

    public class MultilevelHierarchy : BaseFinder
    {

        public List<Cluster> Search(Engine e)
        {

            List<Cluster> retval = new List<Cluster>();

            // loop through all the add nodes
            foreach (ADDNode node in e.addnodes)
            {
                Cluster c = new Cluster();
                if (node.Cluster != null)
                    c = node.Cluster;

                string ClusterReason = "MultiLevel Hierarchy (Parent: " + node.MBase.ToString() + ")";

                // if it has subset(s)
                List<ADDNode> subsetNodes = new List<ADDNode>();
                foreach (Relation r in node.OwnerRelations)
                {
                    if (r.RelationshipType == MetaBuilder.Meta.LinkAssociationType.SubSetOf)
                    {
                        subsetNodes.Add(r.From as ADDNode);
                    }
                }

                int subsetDependencies = 0;
                foreach (ADDNode ssn in subsetNodes)
                {
                    foreach (Relation rss in ssn.DependentRelations)
                    {
                        if (rss.RelationshipType == MetaBuilder.Meta.LinkAssociationType.Dependency)
                        {
                            if (subsetNodes.Contains(rss.To))
                            {
                                subsetDependencies++;
                                if (!c.Nodes.Contains(node))
                                {
                                    this.AddToCluster(node, c, ClusterReason);
                                }

                                /*if (!c.Nodes.Contains(rss.To))
                                    c.Nodes.Add(rss.To);
                                if (!c.Nodes.Contains(ssn))
                                    c.Nodes.Add(ssn);*/
                            }
                        }
                    }
                }
                if (subsetDependencies == c.Nodes.Count - 2 && subsetDependencies > 0)
                {
                    retval.Add(c);
                    foreach (ADDNode n in c.Nodes)
                    {
                        if (n.Cluster == null)
                            this.AddToCluster(n, c, ClusterReason);
                    }
                    c.IsValid = true;
                }
            }

            return retval;
        }

    }
    public class MultilevelNetwork : BaseFinder
    {
        public List<Cluster> Search(Engine e)
        {

            List<Cluster> retval = new List<Cluster>();

            // loop through all the add nodes
            foreach (ADDNode node in e.addnodes)
            {
                bool midNodeFound = false;
                Cluster c = new Cluster();
                if (node.Cluster != null)
                    c = node.Cluster;

                string ClusterReason = "MultiLevel Network (Parent: " + node.MBase.ToString() + ")";

                // if it has subset(s)
                List<ADDNode> subsetNodes = new List<ADDNode>();
                foreach (Relation r in node.OwnerRelations)
                {
                    if (r.RelationshipType == MetaBuilder.Meta.LinkAssociationType.SubSetOf)
                    {
                        subsetNodes.Add(r.From as ADDNode);
                    }
                }

                //int subsetDependencies = 0;
                foreach (ADDNode ssn in subsetNodes)
                {
                    foreach (Relation rss in ssn.OwnerRelations)
                    {
                        if (rss.RelationshipType == MetaBuilder.Meta.LinkAssociationType.Dependency)
                        {
                            ADDNode middleNode = rss.From;
                            foreach (Relation midRelation in middleNode.DependentRelations)
                            {
                                if (midRelation.RelationshipType == MetaBuilder.Meta.LinkAssociationType.Dependency)
                                {
                                    if (midRelation.To != ssn && subsetNodes.Contains(midRelation.To))
                                    {
                                        midNodeFound = true;
                                        this.AddToCluster(ssn, c, ClusterReason);
                                        this.AddToCluster(middleNode, c, ClusterReason);
                                        this.AddToCluster(midRelation.To, c, ClusterReason);
                                        this.AddToCluster(node, c, ClusterReason);
                                    }
                                }
                            }

                        }
                    }
                }
                if (midNodeFound)
                {
                    retval.Add(c);
                }
            }

            return retval;
        }

    }
    public class SingleLevelHierarchy : BaseFinder
    {
        public List<Cluster> Search(Engine e)
        {
            List<Cluster> retval = new List<Cluster>();

            // loop through all the add nodes
            foreach (ADDNode node in e.addnodes)
            {
                string ClusterReason = "Single Level Hierarchy (Parent: " + node.MBase.ToString() + ")";
                Cluster c = new Cluster();
                if (node.Cluster != null)
                    c = node.Cluster;

                // if it has subset(s)
                //int nodeCount = 0;

                List<XORNode> xorNodes = new List<XORNode>();

                foreach (Relation r in node.OwnerRelations)
                {
                    if (r.From is XORNode)
                    {
                        xorNodes.Add(r.From as XORNode);
                    }
                }

                foreach (XORNode x in xorNodes)
                {
                    List<ADDNode> subsetNodes = new List<ADDNode>();
                    foreach (Relation xR in x.OwnerRelations)
                    {
                        subsetNodes.Add(xR.From);
                    }

                    foreach (ADDNode ssn in subsetNodes)
                    {
                        foreach (Relation rSSN in ssn.DependentRelations)
                        {
                            if (rSSN.RelationshipType == MetaBuilder.Meta.LinkAssociationType.Dependency)
                            {
                                if (subsetNodes.Contains((rSSN.To)))
                                {
                                    if (ssn.Cluster != null)
                                    {
                                        if (!c.Nodes.Contains(node))
                                        {
                                            this.AddToCluster(node, c, ClusterReason);
                                        }
                                        this.AddToCluster(ssn, c, ClusterReason);
                                        this.AddToCluster(rSSN.To, c, ClusterReason);
                                        this.AddToCluster(x, c, ClusterReason);
                                    }
                                }
                            }
                        }
                    }
                }


                if (xorNodes.Count > 0 && c.Nodes.Count > 1)
                {
                    retval.Add(c);
                }
            }

            return retval;
        }
    }

    public class SingleLevelNetwork : BaseFinder
    {
        public List<Cluster> Search(Engine e)
        {
            List<Cluster> retval = new List<Cluster>();

            // loop through all the add nodes
            foreach (ADDNode node in e.addnodes)
            {
                bool foundMiddle = false;
                string ClusterReason = "Single Level Network (Parent: " + node.MBase.ToString() + ")";
                Cluster c = new Cluster();
                if (node.Cluster != null)
                    c = node.Cluster;

                // if it has subset(s)
                //int nodeCount = 0;

                List<XORNode> xorNodes = new List<XORNode>();

                foreach (Relation r in node.OwnerRelations)
                {
                    if (r.From is XORNode)
                    {
                        xorNodes.Add(r.From as XORNode);
                    }
                }

                foreach (XORNode x in xorNodes)
                {
                    List<ADDNode> subsetNodes = new List<ADDNode>();
                    foreach (Relation xR in x.OwnerRelations)
                    {
                        subsetNodes.Add(xR.From);
                    }

                    if (subsetNodes.Count == 2) //a network can only have 2 nodes
                    {
                        foreach (ADDNode ssn in subsetNodes)
                        {
                            foreach (Relation rSSN in ssn.OwnerRelations)
                            {
                                if (rSSN.RelationshipType == MetaBuilder.Meta.LinkAssociationType.Dependency && (!rSSN.IsAbstract))
                                {
                                    // does it link to another subset node?
                                    ADDNode middleEntity = rSSN.From as ADDNode;
                                    foreach (Relation rMiddle in middleEntity.DependentRelations)
                                    {
                                        if (subsetNodes.Contains(rMiddle.To) && rMiddle.To != ssn)
                                        {
                                            foundMiddle = true; //NEVER USED BOOL Preventing cluster below
                                            this.AddToCluster(node, c, ClusterReason);
                                            this.AddToCluster(ssn, c, ClusterReason);
                                            this.AddToCluster(rMiddle.To, c, ClusterReason);
                                            this.AddToCluster(rMiddle.From, c, ClusterReason);
                                            c.IsValid = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (foundMiddle && xorNodes.Count > 0 && c.Nodes.Count > 1)
                {
                    retval.Add(c);
                }
            }

            return retval;
        }
    }

    public class SubsetsAndXors : BaseFinder
    {
        public List<Cluster> Search(Engine e)
        {
            List<Cluster> retval = new List<Cluster>();

            // loop through all the add nodes
            foreach (ADDNode node in e.addnodes)
            {
                string ClusterReason = "Subsets and XORs (Parent: " + node.MBase.ToString() + ")";

                foreach (Relation r in node.OwnerRelations)
                {
                    ClusterSubset(node, r.From, ClusterReason, r, retval);
                }

                // if it has subset(s)

                //int added = 0;
                foreach (Relation r in node.DependentRelations)
                {
                    ClusterSubset(r.From, node, ClusterReason, r, retval);
                }
            }

            return retval;
        }

        private void ClusterSubset(ADDNode dictating, ADDNode inheriting, string ClusterReason, Relation r, List<Cluster> retval)
        {
            if (r.RelationshipType == MetaBuilder.Meta.LinkAssociationType.SubSetOf || r.RelationshipType == MetaBuilder.Meta.LinkAssociationType.MutuallyExclusiveLink)
            {
                if (dictating.Cluster == null && inheriting.Cluster == null)
                {
                    Cluster c = new Cluster();
                    this.AddToCluster(dictating, c, ClusterReason);
                    this.AddToCluster(inheriting, dictating.Cluster, ClusterReason);
                    retval.Add(c);
                }

                if (inheriting.Cluster == null && dictating.Cluster != null)
                {
                    this.AddToCluster(inheriting, dictating.Cluster, ClusterReason);
                }
                if (inheriting.Cluster != null && dictating.Cluster == null)
                {
                    this.AddToCluster(dictating, inheriting.Cluster, ClusterReason);
                }
                if (inheriting.Cluster != null && dictating.Cluster != null)
                {
                    this.MoveToCluster(inheriting.Cluster, dictating.Cluster);
                }
            }
        }
    }
    /* 
          
     =======================
     SHORTHAND:
     =======================
         
     RESPONSIBILITY -
             ^       |
             |--------
     
     
       =======================
         MULTI LEVEL HIERARCHY
         =======================
         
         
         RESPONSIBILITY
            ^     ^
            |     |
            |     | <- subset links   
            |     |
      Manager<---SubOrdinate   

         
    =======================
    SINGLE LEVEL HIERARCHY:
    =======================
        
     RESPONSIBILITY
           ^ 
           |
             
           |  
          (O) - XOR
         /   \    
          
       /       \
 Manager <---- SubOrdinate

     
     ===================
     MULTI LEVEL NETWORK 
     ===================
         
     * RESPONSIBILITY
        ^             ^
        |             |
        |             | <- subset links with Dependency Descriptors 
        |             |
  Manager            SubOrdinate
     ^                   ^
     |                   |
     |                   |
     |                   |
     ----- Reports To ----


    =======================
    SINGLE LEVEL NETWORK:
    =======================
        
        RESPONSIBILITY
             ^ 
             |
         
             |  
           ( O ) - XOR
         /       \    
          
       /           \
 Manager        SubOrdinate
     ^               ^
     |               |
     |               |
     |               |
     --- Reports To --
         
     */

}
