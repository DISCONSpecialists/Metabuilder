using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Nodes.Containers;
using MetaBuilder.Meta;
using Northwoods.Go;
using MetaBuilder.Graphing.Shapes.Nodes;
using System.Collections.ObjectModel;

namespace DisconPlugins.Hierarchy
{
    public class InvalidHierarchyException : Exception
    {
        public string NodeName;
    }

    /*
     * 

/// <summary>
/// Sample singleton object.
/// </summary>
public sealed class SiteStructure
{
    /// <summary>
    /// This is an expensive resource we need to only store in one place.
    /// </summary>
    object[] _data = new object[10];

    /// <summary>
    /// Allocate ourselves. We have a private constructor, so no one else can.
    /// </summary>
    static readonly SiteStructure _instance = new SiteStructure();

    /// <summary>
    /// Access SiteStructure.Instance to get the singleton object.
    /// Then call methods on that instance.
    /// </summary>
    public static SiteStructure Instance
    {
	get { return _instance; }
    }

    /// <summary>
    /// This is a private constructor, meaning no outsiders have access.
    /// </summary>
    private SiteStructure()
    {
	// Initialize members, etc. here.
    }
}
     * */
    public class Numbering
    {
        private bool failed;
        public bool Failed
        {
            get { return failed; }
            set { failed = value; }
        }

        static readonly Numbering _instance = new Numbering();
        public static Numbering Instance
        {
            get { return _instance; }
        }

        public Collection<GoNode> processedNodes = new Collection<GoNode>();

        #region Fields (4)

        //private List<QLink> linksList;
        private GraphView myView;
        private Hashtable nodeDictionary;
        private GoNode parentNode;

        #endregion Fields

        #region Methods (7)

        int RootNodeNumber = 0;
        // Public Methods (1) 

        /// <summary>
        /// Numbers the nodes.
        /// </summary>
        /// <param name="view">The view.</param>
        public void NumberNodes(GraphView view)
        {
            myView = view;
            processedNodes = new Collection<GoNode>();
            nodeDictionary = new Hashtable();
            if (view.Selection.Primary is IMetaNode)
            {
                try
                {
                    myView.StartTransaction();
                    Numbering.Instance.processedNodes = new Collection<GoNode>();

                    bool IsHierarchy = TestHierarchy(view.Selection.Primary as GoNode, view.Selection.Primary as GoNode);
                    if (IsHierarchy && nodeDictionary.Count > 1)
                    {
                        DoNumbering(view.Selection.Primary as GoNode);
                    }
                    else
                    {
                        InvalidHierarchyException ex = new InvalidHierarchyException();
                        (view.Selection.Primary as IMetaNode).RequiresAttention = true;
                        ex.NodeName = (view.Selection.Primary as IMetaNode).MetaObject.ToString();
                        myView.ScrollRectangleToVisible(view.Selection.Primary.Bounds);
                        throw ex;
                    }
                }
                catch (InvalidHierarchyException ex)
                {
                    string message = "Not a valid hierarchy.";
                    if (ex.NodeName != null)
                        message += Environment.NewLine + "Node " + ex.NodeName + " is recursive or stands alone";
                    System.Windows.Forms.MessageBox.Show(message);
                    Failed = true;
                }
                finally
                {
                    myView.FinishTransaction("Hierarchical Numbering");
                }
            }
            else
            {
                Collection<GoNode> rootNodes = new Collection<GoNode>();
                foreach (GoObject o in view.Document)
                {
                    if (o is IMetaNode)
                    {
                        try
                        {
                            GoNode n = o as GoNode;

                            if (n.Sources.Count == 0)
                                //foreach (IGoLink l in n.Links)
                                //    if (l is QLink)
                                //        //because they are two way this also counts as a source
                                //        if ((l as QLink).AssociationType != LinkAssociationType.Mapping)
                                rootNodes.Add(n);
                        }
                        catch
                        {
                            //n is either not a gonode or it does not have links
                        }
                    }
                }
                myView.StartTransaction();
                foreach (GoNode n in rootNodes)
                {
                    parentNode = n;
                    try
                    {
                        Numbering.Instance.processedNodes = new Collection<GoNode>();

                        bool IsHierarchy = TestHierarchy(parentNode, parentNode);
                        if (IsHierarchy && nodeDictionary.Count > 1)
                        {
                            DoNumbering(parentNode);
                            RootNodeNumber++;
                        }
                        else
                        {
                            InvalidHierarchyException ex = new InvalidHierarchyException();
                            if (parentNode is IMetaNode)
                            {
                                (parentNode as IMetaNode).RequiresAttention = true;
                                ex.NodeName = (parentNode as IMetaNode).MetaObject.ToString();
                            }
                            else
                                ex.NodeName = parentNode.ToString();

                            myView.ScrollRectangleToVisible(parentNode.Bounds);
                            throw ex;
                        }
                    }
                    catch (InvalidHierarchyException ex)
                    {
                        string message = "Not a valid hierarchy.";
                        if (ex.NodeName != null)
                            message += Environment.NewLine + "Node " + ex.NodeName + " is recursive or stands alone";
                        System.Windows.Forms.MessageBox.Show(message);
                        Failed = true;
                        break;
                    }
                }
                myView.FinishTransaction("Multiple[" + rootNodes.Count + "] Root Hierarchical Numbering");
            }
        }

        // Private Methods (6) 

        /// <summary>
        /// Does the numbering.
        /// </summary>
        /// <param name="targetNode">The target node.</param>
        private void DoNumbering(GoNode targetNode)
        {
            NumberingNodeContainer nncP = targetNode.UserObject as NumberingNodeContainer;
            nncP.SortChildren();

            foreach (DictionaryEntry entry in nodeDictionary)
            {
                GoNode node = entry.Value as GoNode;
                GoGroupEnumerator enumerator = node.GetEnumerator();
                Collection<GoText> oldNumbers = new Collection<GoText>();
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current is GoText)
                    {
                        GoText oldtxt = enumerator.Current as GoText;
                        if (oldtxt.Maximum == 1999)
                            oldNumbers.Add(oldtxt);
                    }
                }
                for (int i = 0; i < oldNumbers.Count; i++)
                {
                    node.Remove(oldNumbers[i]);
                }

                NumberingNodeContainer nnc = node.UserObject as NumberingNodeContainer;
                NumberingText txt;
                bool NeedsToBeAdded = false;

                if (nnc.NumberingText != null)
                {
                    txt = nnc.NumberingText;
                    txt.Text = nnc.NumberString;
                }
                else
                {
                    txt = new NumberingText(nnc.NumberString);
                    NeedsToBeAdded = true;
                }
                if (NeedsToBeAdded)
                {
                    node.Add(txt);
                }

                if (node is GraphNode)
                    txt.Position = new PointF((node as GraphNode).Grid.Right - txt.Width, (node as GraphNode).Grid.Top - txt.Height);
                else
                    txt.Position = new PointF(node.Bounds.Right - txt.Width, node.Bounds.Top - txt.Height);
            }

            using (StreamWriter tw = new StreamWriter(MetaBuilder.Core.Variables.Instance.ExportsPath + @"FSDReport" + RootNodeNumber + ".txt"))
            {
                WriteNode(parentNode as IMetaNode, tw);
            }

            processedNodes.Clear();
            nodeDictionary.Clear();
        }

        /// <summary>
        /// Gets the parent node.
        /// </summary>
        /// <returns></returns>
        //private bool GetParentNode()
        //{
        //    bool FoundParent = false;
        //    foreach (DictionaryEntry entry in nodeDictionary)
        //    {
        //        GoNode n = (entry.Value as GoNode);
        //        bool FoundLink = false;
        //        GoNodePortEnumerator portenum = n.Ports.GetEnumerator();
        //        while (portenum.MoveNext())
        //        {
        //            if (portenum.Current.SourceLinksCount > 0)
        //                FoundLink = true;
        //        }
        //        if (!FoundLink)
        //        {
        //            FoundParent = true;
        //            parentNode = n;
        //        }
        //    }
        //    return FoundParent;
        //}

        /// <summary>
        /// Setups the links list.
        /// </summary>
        /// <param name="view">The view.</param>
        //private void SetupLinksList(GoView view) //BASED ON PARENTNODE -->move into setupnodelist
        //{
        //    linksList = new List<QLink>();
        //    NormalDiagram doc = view.Document as NormalDiagram;
        //    foreach (GoObject o in doc)
        //    {
        //        if (o is QLink)
        //        {
        //            //qlinks can only exist between imetanodes but wth hey
        //            if ((o as QLink).FromNode is IMetaNode && (o as QLink).ToNode is IMetaNode)
        //                linksList.Add(o as QLink);
        //        }
        //    }
        //}
        /// <summary>
        /// Setups the node list.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetupNodeList(GoView view) //BASED ON PARENTNODE -->move into testheirarchy
        {
            nodeDictionary = new Hashtable();
            NormalDiagram doc = view.Document as NormalDiagram;
            foreach (GoObject o in doc)
            {
                if (o is GoNode && o is IMetaNode)
                {
                    NumberingNodeContainer nodeC = new NumberingNodeContainer();
                    (o as GoNode).UserObject = nodeC;
                    nodeC.node = (o as GoNode);
                    nodeC.Setup();

                    if (o is ILinkedContainer)
                    {
                        setupNodesInContainers(o as ILinkedContainer);
                    }
                    else
                    {
                        nodeDictionary.Add((o as GoNode).Location, o);
                    }
                }
            }
        }
        /// <summary>
        /// Setups the containers nodes and adds to list.
        /// </summary>
        /// <param name="view">The Container.</param>
        private void setupNodesInContainers(ILinkedContainer container)
        {
            if (container is SubgraphNode)
            {
                foreach (GoObject o in (container as SubgraphNode))
                {
                    if (o is GoNode && o is IMetaNode)
                    {
                        NumberingNodeContainer nodeC = new NumberingNodeContainer();
                        (o as GoNode).UserObject = nodeC;
                        nodeC.node = (o as GoNode);
                        nodeC.Setup();

                        if (o is ILinkedContainer)
                        {
                            setupNodesInContainers(o as ILinkedContainer);
                        }
                        else
                        {
                            nodeDictionary.Add((o as GoNode).Location, o);
                        }
                    }
                }
            }
            else if (container is ValueChain)
            {
                foreach (GoObject o in (container as ValueChain))
                {
                    if (o is GoNode && o is IMetaNode)
                    {
                        NumberingNodeContainer nodeC = new NumberingNodeContainer();
                        (o as GoNode).UserObject = nodeC;
                        nodeC.node = (o as GoNode);
                        nodeC.Setup();

                        if (o is ILinkedContainer)
                        {
                            setupNodesInContainers(o as ILinkedContainer);
                        }
                        else
                        {
                            nodeDictionary.Add((o as GoNode).Location, o);
                        }
                    }
                }
            }
            else
            {
            }
        }

        /// <summary>
        /// Tests the hierarchy and populates the nodedictionary
        /// </summary>
        /// <returns></returns>
        private bool TestHierarchy(GoNode parentNode, GoNode startNode)
        {
            if (nodeDictionary == null)
                nodeDictionary = new Hashtable();

            NumberingNodeContainer nodeC = new NumberingNodeContainer();
            nodeC.node = (parentNode as GoNode);
            nodeC.Setup();
            (parentNode as GoNode).UserObject = nodeC;
            nodeDictionary.Add(parentNode.Location.ToString(), parentNode);
            if (parentNode is ILinkedContainer)
                setupNodesInContainers(parentNode as ILinkedContainer);

            bool r = true;
            foreach (IGoLink l in parentNode.DestinationLinks)
            {
                IGoLink link = l;
                if (r == false)
                {
                    nodeDictionary = null;
                    break;
                }
                try
                {
                    if (link.ToNode == startNode || nodeDictionary.ContainsKey((link.ToNode as GoObject).Location.ToString()))
                    {
                        if (link.ToNode is GraphNode)
                        {
                            if ((link.ToNode as GraphNode).DestinationLinks.Count == 0)
                                r = TestHierarchy(link.ToNode as GoNode, startNode);
                        }
                        else if (link.ToNode is ImageNode)
                        {
                            if ((link.ToNode as ImageNode).DestinationLinks.Count == 0)
                                r = TestHierarchy(link.ToNode as GoNode, startNode);
                        }
                        else
                        {
                            r = false;
                        }
                    }
                    else
                    {
                        r = TestHierarchy(link.ToNode as GoNode, startNode);
                    }
                }
                catch
                {
                    link.ToNode.ToString();
                }
            }
            return r;
        }
        private void WriteNode(IMetaNode node, StreamWriter tw)
        {
            NumberingNodeContainer nnc = (node as GoNode).UserObject as NumberingNodeContainer;
            char splitChar = '.';
            string[] tabCountArray = nnc.NumberingText.Text.Split(splitChar);
            StringBuilder sb = new StringBuilder();
            if (nnc.NumberingText.Text.Length != 0)
            {
                for (int i = 0; i < tabCountArray.Length; i++)
                    sb.Append("\t");
            }

            if (node.MetaObject._ClassName != "Function")
            {
                tw.WriteLine(sb.ToString() + nnc.NumberingText.Text + " " + node.MetaObject.ToString() + " (" + node.MetaObject._ClassName + ")");
                tw.Flush();
            }
            else
            {
                string tobewritten = sb.ToString() + nnc.NumberingText.Text + " " + node.MetaObject.ToString();
                string abc = "abcdefghijklmnopqrstuvwxyz";
                string numbText = nnc.NumberingText.Text;
                string lastChar = "999";
                if (numbText.Length > 0)
                {
                    lastChar = numbText.Substring(numbText.Length - 1, 1);
                }

                if (abc.IndexOf(lastChar) >= 0)
                {
                    tw.WriteLine(tobewritten + " (Class)");
                    tw.Flush();
                }
                else
                {
                    tw.WriteLine(tobewritten);
                    tw.Flush();
                }
            }

            nnc.SortChildren();
            foreach (IMetaNode nChild in nnc.children)
            {
                WriteNode(nChild, tw);
            }

        }
        #endregion Methods
    }

    [Serializable]
    public class NumberingText : GoText
    {
        public NumberingText(string number)
        {
            Text = number;
            Alignment = 1;
            DragsNode = true;
            Selectable = false;
            Movable = false;
            this.Maximum = 1999;
        }

    }

    [Serializable]
    public class NumberingNodeContainer
    {
        #region Fields (3)

        public List<GoNode> children;
        public GoNode node;
        public GoNode ParentNode;

        #endregion Fields

        #region Properties (3)

        public NumberingText NumberingText
        {
            get
            {
                foreach (GoObject o in node)
                {
                    if (o is NumberingText)
                        return o as NumberingText;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the number string.
        /// </summary>
        /// <value>The number string.</value>
        public string NumberString
        {
            get
            {
                if (ParentNode != null)
                {
                    if (parentNode.UserObject != null)
                    {
                        NumberingNodeContainer parentContainer = parentNode.UserObject as NumberingNodeContainer;
                        parentContainer.SortChildren();
                        string prev;
                        if (parentContainer.NumberString.Length == 0)
                            prev = (parentContainer.IndexOf(node)).ToString();
                        else
                            prev = parentContainer.NumberString + "." + (parentContainer.IndexOf(node)).ToString();
                        return GetTabbed(prev) + prev;
                    }
                    else
                        return "";

                }
                return "";
            }
        }

        /// <summary>
        /// Gets the parent node.
        /// </summary>
        /// <value>The parent node.</value>
        private GoNode parentNode
        {
            get
            {
                GoNodeNodeEnumerator enumerator = node.Sources.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    return enumerator.Current as GoNode;
                }

                return null;
            }
        }

        #endregion Properties

        #region Methods (10)

        // Public Methods (5) 

        public string ABCvalue(int index)
        {
            string abc = "abcdefghijklmnopqrstuvwxyz";
            return abc.Substring(index, 1);
        }

        public string IndexOf(GoNode n)
        {
            int classCount = 0;
            int nodeCount = 0;
            foreach (GoNode childNode in children)
            {
                nodeCount++;
                if (GetLinkType(childNode) == LinkAssociationType.Classification)
                {
                    classCount++;
                    if (childNode == n)
                    {
                        return ABCvalue(classCount - 1);
                    }
                }
                if (childNode == n)
                {
                    if (GetLinkType(n) == LinkAssociationType.Auxiliary)
                    {
                        return (nodeCount - classCount).ToString() + " (Aux)";
                    }
                    return (nodeCount - classCount).ToString();
                }
            }
            return "";
        }

        /// <summary>
        /// Setups this instance.
        /// </summary>
        public void Setup()
        {
            ParentNode = parentNode;
        }

        public void SortChildren()
        {
            SetChildren();

            for (int x = 0; x <= (children.Count - 2); x++)
            {
                for (int y = x; y <= children.Count - 1; y++)
                {
                    if (children[x].Position.X > children[y].Position.X && WithinRange(children[x].Position.Y, children[y].Position.Y, 20))
                    {
                        SwapChildren(x, y);
                    }
                    else
                    {
                        if (children[x].Position.Y > children[y].Position.Y && WithinRange(children[x].Position.X, children[y].Position.X, 5))
                        {
                            SwapChildren(x, y);
                        }
                        else
                        {
                            continue;
                        }
                    }

                    //LinkAssociationType lX = GetLinkType(children[x]);
                    //LinkAssociationType lY = GetLinkType(children[y]);

                    //if (lX != LinkAssociationType.Classification && lY == LinkAssociationType.Classification)
                    //{
                    //    SwapChildren(x, y);
                    //}
                    //else if (lX == LinkAssociationType.Auxiliary && lY != LinkAssociationType.Auxiliary)
                    //{
                    //    SwapChildren(x, y);
                    //}
                }
            }

            foreach (GoNode nChild in children)
            {
                if (nChild.UserObject != null)
                    (nChild.UserObject as NumberingNodeContainer).SortChildren();
            }

        }

        public void SortChildrenAlphabetic()
        {
            for (int x = 0; x <= (children.Count - 2); x++)
            {
                for (int y = x; y <= children.Count - 1; y++)
                {
                    NumberingNodeContainer nncX = children[x].UserObject as NumberingNodeContainer;
                    NumberingNodeContainer nncY = children[y].UserObject as NumberingNodeContainer;
                    StringComparer comparer = StringComparer.Create(CultureInfo.CurrentUICulture, true);
                    int sortOrder = comparer.Compare(nncX.NumberingText.Text, nncY.NumberingText.Text);
                    if (sortOrder > -1)
                        SwapChildren(x, y);
                }
            }
            foreach (GoNode nChild in children)
            {
                if (nChild.UserObject != null)
                    (nChild.UserObject as NumberingNodeContainer).SortChildrenAlphabetic();
            }
        }
        // Private Methods (5) 
        private LinkAssociationType GetLinkType(GoNode nChild)
        {
            GoNodeLinkEnumerator linkEnum = node.DestinationLinks.GetEnumerator();
            while (linkEnum.MoveNext())
            {
                if (linkEnum.Current.ToNode == nChild)
                    return (linkEnum.Current as QLink).AssociationType;
            }

            return LinkAssociationType.Mapping;
        }

        private string GetTabbed(string s)
        {
            return "";
            char splitchar = '.';
            string[] strings = s.Split(splitchar);
            StringBuilder sbTabbed = new StringBuilder();
            for (int i = 0; i < strings.Length; i++)
            {
                sbTabbed.Append("\t");
            }
            return sbTabbed.ToString();

        }
        bool childrenSet = false;
        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>The children.</value>
        private void SetChildren()
        {
            if (!childrenSet)
            {
                children = new List<GoNode>();
                if (node is ILinkedContainer)
                {
                    if (node is SubgraphNode)
                    {
                        foreach (GoObject o in (node as SubgraphNode))
                        {
                            if (o is IMetaNode)
                            {
                                children.Add(o as GoNode);
                                Numbering.Instance.processedNodes.Add(o as GoNode);
                            }
                        }
                    }
                    else if (node is ValueChain)
                    {
                        foreach (GoObject o in (node as ValueChain))
                        {
                            if (o is IMetaNode)
                            {
                                children.Add(o as GoNode);
                                Numbering.Instance.processedNodes.Add(o as GoNode);
                            }
                        }
                    }
                }
                else
                {
                    GoNodeNodeEnumerator enumerator = node.Destinations.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        GoNode n = enumerator.Current as GoNode;
                        if (!(n is IMetaNode))
                            continue;
                        if (Numbering.Instance.processedNodes.Contains(n))
                        {
                            string ProblemNodeName = "";
                            if ((n as IMetaNode).MetaObject != null)
                            {
                                if ((n as IMetaNode).MetaObject.ToString() != null)
                                {
                                    if ((n as IMetaNode).MetaObject.ToString() != "")
                                    {
                                        ProblemNodeName = "(" + (n as IMetaNode).MetaObject.ToString() + ")";
                                    }
                                }
                            }
                            InvalidHierarchyException ihe = new InvalidHierarchyException();
                            ihe.NodeName = ProblemNodeName;
                            throw ihe;
                        }
                        else
                        {
                            children.Add(n);
                            Numbering.Instance.processedNodes.Add(n);
                        }
                    }
                }
            }
            childrenSet = true;
        }

        private void SwapChildren(int x, int y)
        {
            GoNode xChild = children[x];
            children[x] = children[y];
            children[y] = xChild;
        }

        private bool WithinRange(float PointPart1, float PointPart2, float range)
        {
            return (PointPart1 <= PointPart1 + range) && (PointPart2 <= PointPart1 + range);
        }

        #endregion Methods
    }
}