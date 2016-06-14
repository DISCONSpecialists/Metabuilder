using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Drawing;

using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation.ADD.Cluster
{
    public class Cluster
    {
        List<string> usedColors = new List<string>();
        List<IMetaNode> nodes = new List<IMetaNode>();
        public Cluster(Engine2015 engine)
        {
            foreach (IMetaNode node in engine.nodes)
            {
                ClusterUntilNoNodesLeft(node);
            }
        }

        public enum NetworkType
        {
            MultiHierarchy, SingleHierarchy, MultiNetwork, SingleNetwork, Standalone, CannotIdentify
        }

        private void ClusterUntilNoNodesLeft(IMetaNode node)
        {
            if (nodes.Contains(node))
            {
                //check if it should move?
                return;
            }

            //discover network type for this node
            switch (discoverNetwork(node))
            {
                case NetworkType.MultiHierarchy:
                    break;
                case NetworkType.SingleHierarchy:
                    break;
                case NetworkType.MultiNetwork:
                    break;
                case NetworkType.SingleNetwork:
                    break;
                case NetworkType.Standalone:
                    break;
                case NetworkType.CannotIdentify:
                    break;
                default:
                    break;
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

        public NetworkType discoverNetwork(IMetaNode node)
        {
            //if node has a dependant subset which has a dependancy and that dependancy is node then MULTI LEVEL HIERARCHY

            //if node has subsetindicator and subsets depend on each other then SINGLE LEVEL HIERARCHY

            //if node depends on two different nodes and those nodes are a subset of another node and those links have dependency descriptions then MULTI LEVEL NETWORK

            //if node depends on two different nodes and those nodes are SUBSETINDICATOR subst of another node then SINGLE LEVEL NETWORK

            return NetworkType.CannotIdentify;
        }

        private void AddToCluster(IMetaNode node, List<IMetaNode> nodes)
        {
            if (Clusters.ContainsKey(node))
            {
                Clusters[node].AddRange(nodes);
            }
            else
            {
                Clusters[node].AddRange(nodes);
            }
        }
        private int getCohesionWeightOnLinksArtefact(QLink link)
        {
            foreach (IMetaNode node in link.GetArtefacts())
            {
                if (node.MetaObject.Class == "DependencyDescription")
                {
                    if (node.MetaObject.Get("CohesionWeight") != null)
                        return int.Parse(node.MetaObject.Get("CohesionWeight").ToString());
                }
            }

            return 100;
        }
        private Color GetNewColour()
        {
            Random r = new Random();

            byte red = (byte)r.Next(0, 255);
            byte green = (byte)r.Next(0, 255);
            byte blue = (byte)r.Next(0, 255);
            string colorKey = red.ToString() + green.ToString() + blue.ToString();
            if (usedColors.Contains(colorKey))
                return GetNewColour();

            usedColors.Add(colorKey);
            return Color.FromArgb(255, red, green, blue);
        }

        private Dictionary<IMetaNode, List<IMetaNode>> clusters;
        public Dictionary<IMetaNode, List<IMetaNode>> Clusters
        {
            get { return clusters; }
            set { clusters = value; }
        }

    }
}