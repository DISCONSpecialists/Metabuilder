using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.Core;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Meta;
using ObjectAssociation=MetaBuilder.BusinessLogic.ObjectAssociation;

namespace MetaBuilder.UIControls.Tree
{
    public delegate void DelegateAddNode(TreeNode node);

    /// <summary>
    /// Summary description for UserControl1.
    /// </summary>
    public class MetaBaseTree : TreeView
    {

		#region Fields (4) 

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private Container components = null;
        public int MetaDepth;
        public IMetaBase metaObject;
        public bool ShowEmptyNodesDefault;

		#endregion Fields 

		#region Constructors (1) 

        public MetaBaseTree()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            MetaDepth = int.Parse("0" + ConfigurationManager.AppSettings["MetaDepth"]);
            m_DelegateAddNode = new DelegateAddNode(AddNode);
        }

		#endregion Constructors 

		#region Methods (16) 


		// Public Methods (8) 

        /// <summary>
        /// Clears nodes and add the parameter node
        /// </summary>
        /// <param name="n"></param>
        public virtual void AddNode(TreeNode n)
        {
            Nodes.Clear();
            Nodes.Add(n);
        }

        /// <summary>
        /// Builds child object nodes for a specific objectnode and one of its caption nodes
        /// </summary>
        /// <param name="tnCurrent">The object node</param>
        /// <param name="tnCaption">The current caption node</param>
        /// <param name="metaBase">The object node's IMetaBase object</param>
        /// <returns></returns>
        public int BuildAssociatedObjectNodes(TreeNode tnCurrent, TreeNode tnCaption, MetaBase metaBase)
        {
            // Setup dataview to reflect only this object's associated children
            dvAssociatedObjects =
                Singletons.GetAssociationHelper().GetAssociatedObjects(metaBase.pkid, metaBase.MachineName).DefaultView;
            dvAssociatedObjects.RowFilter = "Caption='" + tnCaption.Text + "'";
            // Clear the nodes (if function is called again, it wont append nodes)
            tnCaption.Nodes.Clear();
            // Introduce variables used in the foreach loop
            int imgindex = 0;
            // Iterate through records, creating nodes for each and adding them to the specific caption node
            foreach (DataRowView drvObj in dvAssociatedObjects)
            {
                imgindex = CreateObjectNode(drvObj, tnCaption);
            }
            // Shallow build each child node
            foreach (ObjectNode tnO in tnCaption.Nodes)
            {
                BuildShallowObjectNode(tnO, imgindex);
            }
            return imgindex;
        }

        /// <summary>
        /// Builds a node and it's captions (but not the captions' children)
        /// </summary>
        /// <param name="node">The object node to build</param>
        /// <param name="ImageIndex">The imageindex of the object node</param>
        public virtual void BuildShallowObjectNode(ObjectNode node, int ImageIndex)
        {
            nodeCount++; // Increase number of nodes created
            // Set the image for this node
            if (ImageIndex > 0)
            {
                node.ImageIndex = ImageIndex;
                if (node.ImageIndex != 8)
                {
                    //node.ForeColor = nodeTextColorsArray[node.ImageIndex];
                }
            }
            // If the current node is still within bounds build the caption nodes. MetaDepth is specified in App.Core.Variables.
            if (GetNodeDepth(node) < MetaDepth)
            {
                SetupCaptionsForNode(node);
            }
        }

        /// <summary>
        /// Builds a tree based on a IMetaBase object
        /// </summary>
        public virtual void BuildTree()
        {
            if (AllowDrop)
            {
                ItemDrag += new ItemDragEventHandler(this_ItemDrag);
                DragDrop += new DragEventHandler(this_DragDrop);
                DragEnter += new DragEventHandler(this_DragEnter);
                DragOver += new DragEventHandler(this_DragOver);
            }
            // Setup variables and clear existing data
            InitColors();
            Nodes.Clear();
            Nodes.Add(new TreeNode("... Building treeview"));
            dsassociationdata = null;
            hashCaptionNodes = new Hashtable();
            // Create a thread in which the tree will be loaded
            workerThread = new Thread(new ThreadStart(LoadTree));
            workerThread.Name = "BuildTree Thread";
            workerThread.Start();
        }

        /// <summary>
        /// For each Caption node, shallow build the object nodes
        /// </summary>
        /// <param name="node">The node that was expanded</param>
        public void ExpandingBuild(MetaTreeNode node)
        {
            if ((node.Tag is IMetaBase))
            {
                foreach (CaptionNode tnCaption in node.Nodes)
                {
                    if (tnCaption.Nodes.Count < 1)
                    {
                        BuildAssociatedObjectNodes(node, tnCaption, (MetaBase) node.Tag);
                    }
                }
                ObjectNode onode = (ObjectNode) node;
                if (!onode.ShowEmptyNodes)
                {
                    TreeNode[] nodesToRemove = new TreeNode[node.Nodes.Count];
                    int counter = 0;
                    foreach (CaptionNode tnCaption in node.Nodes)
                    {
                        if (tnCaption.Nodes.Count == 0)
                        {
                            nodesToRemove[counter] = tnCaption;
                            counter++;
                        }
                    }
                    for (int i = 0; i < counter; i++)
                    {
                        if (nodesToRemove[i] != null)
                        {
                            nodesToRemove[i].Remove();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Initalize the Color[] array
        /// </summary>
        public void InitColors()
        {
            nodeTextColorsArray =
                new Color[9]
                    {
                        Color.Black, Color.Firebrick, Color.MediumSeaGreen, Color.Olive, Color.SteelBlue, Color.DarkOrange,
                        Color.Chocolate, Color.Gray, Color.Empty
                    };
        }

        /// <summary>
        /// If the MetaBase object exists, build a tree for it
        /// 
        /// For performance reasons (not to mention recursive nodes), this is a shallow build. 
        /// Only the first two levels are built. Once an object's node is expanded, its children 
        /// and grandchildren are built as well.
        /// 
        /// Tried using a caching mechanism, but failed to to partially built nodes being added & retrieved
        /// 
        /// Tried using a recursive checker, which worked, but wouldnt work on a large object with lots of 
        /// mappings to different objects that map back to each other. It only checked up the root branch of the node being added.
        /// </summary>
        public virtual void LoadTree()
        {
            // Performance testing setup. Was used before shallowbuild

            nodeCount = 0; // The number of nodes added to the tree
            cachedNodesRetrieved = 0; // The number of nodes retrieved from the cache
            if (metaObject != null)
            {
                // Create the root node
                ObjectNode tnRoot = new ObjectNode(metaObject.ToString());
                tnRoot.Tag = metaObject;
                // Build the node
                BuildShallowObjectNode(tnRoot, 0);
                // Add it to the tree's nodes. 
                Invoke(m_DelegateAddNode, new Object[] {tnRoot});
                    // Using delegates for threading reasons. (Cannot update interface from a different thread w/o using delegates)
            }
            // Performance testing printout
            //stopWatch.Trace("Treeview built: " + cachedNodesRetrieved.ToString() + "/" + nodeCount.ToString());
        }

        /// <summary>
        /// Retrieves and builds captions for a specified node's object. 
        /// These are indicative of the allowed associations between the object and other classes
        /// </summary>
        /// <param name="node">The object node for which to build caption nodes</param>
        public void SetupCaptionsForNode(ObjectNode node)
        {
            // Setup dataview to reflect only this object's allowed children
            dvCaptions = dsAssociationData.Tables["Captions"].DefaultView;
            dvCaptions.RowFilter = "ParentClass='" +
                                   node.ibase.GetType().Name.Replace(Variables.MetaNameSpace + ".", "") + "'";
            node.Nodes.Clear();
            // Iterate records and add nodes to the object node for each record
            foreach (DataRowView drvCaption in dvCaptions)
            {
                CreateCaptionNode(drvCaption, node);
            }
        }



		// Protected Methods (1) 

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }



		// Private Methods (7) 

        private void CloneCaptionNode(CaptionNode tnCaption)
        {
            int objid;
            ObjectNode[] tn = new ObjectNode[tnCaption.Nodes.Count];
            int counter = 0;
            foreach (TreeNode tno in tnCaption.Nodes)
            {
                // Console.WriteLine("Add " + tno.Text + " to position:" + counter.ToString() + " in array");
                tn[counter] = (ObjectNode) tno.Clone();
                counter++;
            }
            tnCaption.Nodes.Clear();
            foreach (DataRowView drvObj in dvAssociatedObjects)
            {
                objid = int.Parse(drvObj["ChildObjectID"].ToString());
                // Console.WriteLine("Searching for object:" + objid.ToString());
                for (int i = 0; i < counter; i++)
                {
                    if (tn[i].ObjectID == objid)
                    {
                        tnCaption.Nodes.Add(tn[i]);
                    }
                }
            }
            foreach (ObjectNode tno in tnCaption.Nodes)
            {
                BuildShallowObjectNode(tno, tno.ImageIndex);
            }
        }

        /// <summary>
        /// Creates a caption node and adds it to the object node
        /// </summary>
        /// <param name="drvCaption"></param>
        /// <param name="node"></param>
        private void CreateCaptionNode(DataRowView drvCaption, ObjectNode node)
        {
            string caption;
            caption = drvCaption["Caption"].ToString();
            CaptionNode tnCaption = new CaptionNode(caption);
            node.Nodes.Add(tnCaption);
            // Set node properties
            tnCaption.ImageIndex = 8;
            tnCaption.AssociationTypeID = int.Parse(drvCaption["AssociationTypeID"].ToString());
            int CAid = int.Parse(drvCaption["CAid"].ToString());
            tnCaption.CAid = CAid;
            tnCaption.ChildClass = drvCaption["ChildClass"].ToString();
            tnCaption.ParentClass = drvCaption["ParentClass"].ToString();
            Color nodecolor = nodeTextColorsArray[(tnCaption.ImageIndex)];
            if (nodecolor != Color.Empty)
            {
                tnCaption.ForeColor = nodecolor;
            }
            // Add to hashCaptionNodes (used when a caption has been resorted)
            if (hashCaptionNodes.ContainsKey(node.ObjectID.ToString() + ":" + CAid.ToString()))
            {
                ArrayList occurrences = (ArrayList) hashCaptionNodes[node.ObjectID.ToString() + ":" + CAid.ToString()];
                occurrences.Add(tnCaption);
                hashCaptionNodes[node.ObjectID.ToString() + ":" + CAid.ToString()] = occurrences;
            }
            else
            {
                ArrayList captionOccurrence = new ArrayList();
                captionOccurrence.Add(tnCaption);
                hashCaptionNodes.Add(node.ObjectID.ToString() + ":" + CAid.ToString(), captionOccurrence);
            }
        }

        private int CreateObjectNode(DataRowView drvObj, TreeNode tnCaption)
        {
            int objid = int.Parse(drvObj["ChildObjectID"].ToString());
            IMetaBase imbO = Loader.GetByID(drvObj["ChildClass"].ToString(),objid, drvObj["ChildObjectMachine"].ToString());
            imbO.LoadEmbeddedObjects(1, 4);
            ObjectNode tnchild = new ObjectNode(imbO.ToString());
            tnchild.Tag = imbO;
            tnCaption.Nodes.Add(tnchild);
            int imgindex = int.Parse(drvObj["AssociationTypeID"].ToString());
            tnchild.ImageIndex = imgindex;
            return imgindex;
        }

        /// <summary>
        /// Returns the depth of the node.
        /// </summary>
        /// <param name="node">The node to calculate the value for</param>
        /// <returns></returns>
        private int GetNodeDepth(TreeNode node)
        {
            int retval = 0;
            while (node.Parent != null)
            {
                node = node.Parent;
                retval++;
            }
            return retval;
        }

        private void HideCaptionNodes(ObjectNode node, bool Visible)
        {
        }

        private void Resequence(CaptionNode tnCaptionSource)
        {
            ObjectNode oNode = (ObjectNode) tnCaptionSource.Parent;
            int CAid = tnCaptionSource.CAid;
            ArrayList occurrences = (ArrayList) hashCaptionNodes[oNode.ObjectID.ToString() + ":" + CAid.ToString()];
            // Setup dataview to reflect only this object's associated children
            dvAssociatedObjects =
                Singletons.GetAssociationHelper().GetAssociatedObjects(oNode.mbase.pkid, oNode.mbase.MachineName).
                    DefaultView;
            dvAssociatedObjects.RowFilter = "Caption='" + tnCaptionSource.Text + "'";
            foreach (CaptionNode tnCaption in occurrences)
            {
                CloneCaptionNode(tnCaption);
            }
        }

        private void SequenceAndSave(CaptionNode tnCaption)
        {
            ObjectNode tnParent = (ObjectNode) tnCaption.Parent;
            foreach (ObjectNode oNode in tnCaption.Nodes)
            {
                ObjectAssociation oassoc = new ObjectAssociation();
                oassoc.ObjectID = tnParent.ObjectID;
                oassoc.ObjectMachine = tnParent.mbase.MachineName;
                oassoc.ChildObjectID = oNode.ObjectID;
                oassoc.ChildObjectMachine = oNode.mbase.MachineName;
                oassoc.CAid = tnCaption.CAid;
                DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Update(oassoc);
                //Singletons.GetObjectHelper().UpdateObjectAssociation(tnParent.ObjectID, tnCaption.CAid, oNode.ObjectID, oNode.Index + 1,tnParent.mbase.MachineName,oNode.mbase.MachineName);
            }
            Resequence(tnCaption);
        }


		#endregion Methods 


        #region Drag & Drop
        /// <summary>
        /// Event fired when node was dragged outside & back again
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void this_DragEnter(object sender, DragEventArgs e)
        {
            if ((e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false)) ||
                (e.Data.GetDataPresent("MetaBuilder.UIControls.Tree.ObjectNode")))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// Event fired when node is dragged over control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void this_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("MetaBuilder.UIControls.Tree.ObjectNode", true))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        /// <summary>
        /// Event fired when item is dragged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void this_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Copy | DragDropEffects.Move);
        }

        /// <summary>
        /// Event fired when node is dropped
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void this_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("MetaBuilder.UIControls.Tree.ObjectNode", false))
            {
                ObjectNode newNode = (ObjectNode) e.Data.GetData("MetaBuilder.UIControls.Tree.ObjectNode");
                // Get the destination node
                Point pt = PointToClient(new Point(e.X, e.Y));
                TreeNode destinationNode = GetNodeAt(pt);
                bool IsInCollection = false;
                // Get the action to be performed
                NodeDragDropOptions option = GetNodeDragDropOption(destinationNode, newNode);
                ObjectNode cloneNode = (ObjectNode) newNode.Clone();
                switch (option)
                {
                    case NodeDragDropOptions.SameCaption:
                        DropNodeOnOwnCaption(destinationNode, cloneNode, newNode);
                        break;
                    case NodeDragDropOptions.SameSequence:
                        DropNodeOnNodeInSameSequence(destinationNode, cloneNode, newNode);
                        break;
                    case NodeDragDropOptions.DifferentCaption:
                        DropNodeOnDifferentCaption(destinationNode, newNode, IsInCollection, cloneNode);
                        break;
                    case NodeDragDropOptions.DifferentSequence:
                        DropNodeBetweenNodesInAnotherCaption(destinationNode, newNode, IsInCollection, cloneNode);
                        break;
                }
            }
        }

        /// <summary>
        /// Retrieves the action to be performed by the drop function
        /// </summary>
        /// <param name="destinationNode"></param>
        /// <param name="newNode"></param>
        /// <returns></returns>
        private static NodeDragDropOptions GetNodeDragDropOption(TreeNode destinationNode, ObjectNode newNode)
        {
            NodeDragDropOptions option;
            if ((destinationNode.Parent == newNode.Parent) && (destinationNode.Tag != newNode.Tag))
            {
                option = NodeDragDropOptions.SameSequence;
            }
            else
            {
                if (destinationNode is CaptionNode)
                {
                    option = (destinationNode == newNode.Parent)
                                 ? NodeDragDropOptions.SameCaption
                                 : NodeDragDropOptions.DifferentCaption;
                }
                else
                {
                    option = NodeDragDropOptions.DifferentSequence;
                }
            }
            return option;
        }

        /// <summary>
        /// Node was dragged onto own caption. Move the node to the first child of caption node
        /// </summary>
        /// <param name="destinationNode"></param>
        /// <param name="cloneNode"></param>
        /// <param name="newNode"></param>
        private void DropNodeOnOwnCaption(TreeNode destinationNode, ObjectNode cloneNode, ObjectNode newNode)
        {
            CaptionNode tnTargetCaption;
            tnTargetCaption = (CaptionNode) destinationNode;
            tnTargetCaption.Nodes.Insert(0, cloneNode);
            newNode.Remove();
            SequenceAndSave(tnTargetCaption);
            SelectedNode = cloneNode;
        }

        /// <summary>
        /// Node was dragged between/onto other nodes within the same caption. Move the node before destination node
        /// </summary>
        /// <param name="destinationNode"></param>
        /// <param name="cloneNode"></param>
        /// <param name="newNode"></param>
        private void DropNodeOnNodeInSameSequence(TreeNode destinationNode, ObjectNode cloneNode, ObjectNode newNode)
        {
            CaptionNode tnTargetCaption;
            tnTargetCaption = (CaptionNode) destinationNode.Parent;
            tnTargetCaption.Nodes.Insert(destinationNode.Index, cloneNode);
            newNode.Remove();
            SequenceAndSave(tnTargetCaption);
            SelectedNode = cloneNode;
        }

        /// <summary>
        /// Node was dropped on a different caption. Create a node at the first position under the caption node
        /// </summary>
        /// <param name="destinationNode"></param>
        /// <param name="newNode"></param>
        /// <param name="IsInCollection"></param>
        /// <param name="cloneNode"></param>
        private void DropNodeOnDifferentCaption(TreeNode destinationNode, ObjectNode newNode, bool IsInCollection,
                                                ObjectNode cloneNode)
        {
            CaptionNode tnTargetCaption;
            tnTargetCaption = (CaptionNode) destinationNode;
            // Ensure the target caption contains items of the same class
            if (tnTargetCaption.ChildClass == newNode.Class)
            {
                foreach (TreeNode tnCaptionChildren in tnTargetCaption.Nodes)
                {
                    if (tnCaptionChildren.Tag == newNode.Tag)
                    {
                        IsInCollection = true;
                        break;
                    }
                }
                if (!IsInCollection)
                {
                    tnTargetCaption.Nodes.Insert(0, cloneNode);
                    tnTargetCaption.Expand();
                    ObjectNode parentObjectNode = (ObjectNode) tnTargetCaption.Parent;
                    ObjectAssociation oas = new ObjectAssociation();
                    oas.ObjectID = parentObjectNode.mbase.pkid;
                    oas.ChildObjectID = cloneNode.mbase.pkid;
                    oas.ObjectMachine = parentObjectNode.mbase.MachineName;
                    oas.ChildObjectMachine = cloneNode.mbase.MachineName;
                    oas.CAid = tnTargetCaption.CAid;
                    Singletons.GetObjectHelper().AddObjectAssociation(oas);
                        //parentObjectNode.ObjectID, tnTargetCaption.CAid, cloneNode.ObjectID,parentObjectNode.mbase.MachineName,cloneNode.mbase.MachineName);
                    cloneNode.ImageIndex = tnTargetCaption.AssociationTypeID;
                    SequenceAndSave(tnTargetCaption);
                    SelectedNode = cloneNode;
                }
                else
                {
                    MessageBox.Show(this,"The dragged node is already part of the target collection");
                }
            }
            else
            {
                MessageBox.Show(this,"Cannot drop the node here. This collection stores " + tnTargetCaption.ChildClass +
                                " objects, while the dragged node is a " + newNode.Class + ".");
            }
        }

        /// <summary>
        /// Node was dropped between/onto nodes in another caption. M
        /// </summary>
        /// <param name="destinationNode"></param>
        /// <param name="newNode"></param>
        /// <param name="IsInCollection"></param>
        /// <param name="cloneNode"></param>
        private void DropNodeBetweenNodesInAnotherCaption(TreeNode destinationNode, ObjectNode newNode,
                                                          bool IsInCollection, ObjectNode cloneNode)
        {
            CaptionNode tnTargetCaption;
            tnTargetCaption = (CaptionNode) destinationNode.Parent;
            foreach (TreeNode tn in tnTargetCaption.Nodes)
            {
                if (tn.Tag == newNode.Tag)
                {
                    IsInCollection = true;
                    break;
                }
            }
            if (!IsInCollection)
            {
                tnTargetCaption.Nodes.Insert(destinationNode.Index, cloneNode);
                ObjectNode parentObjectNode = (ObjectNode) destinationNode.Parent.Parent;
                ObjectAssociation oas = new ObjectAssociation();
                oas.ObjectID = parentObjectNode.mbase.pkid;
                oas.ChildObjectID = cloneNode.mbase.pkid;
                oas.ObjectMachine = parentObjectNode.mbase.MachineName;
                oas.ChildObjectMachine = cloneNode.mbase.MachineName;
                oas.CAid = tnTargetCaption.CAid;
                Singletons.GetObjectHelper().AddObjectAssociation(oas);
                    //parentObjectNode.ObjectID, tnTargetCaption.CAid, cloneNode.ObjectID,parentObjectNode.mbase.MachineName,cloneNode.mbase.MachineName);
                cloneNode.ImageIndex = tnTargetCaption.AssociationTypeID;
                SequenceAndSave(tnTargetCaption);
                SelectedNode = cloneNode;
            }
            else
            {
                MessageBox.Show(this,"The dragged node is already part of the target collection");
            }
        }
        #endregion

        #region Properties
        private DataSet dsassociationdata;

        public DataSet dsAssociationData
        {
            get
            {
                try
                {
                    if (dsassociationdata == null)
                    {
                        dsassociationdata = Singletons.GetAssociationHelper().GetAssociationDataSet();
                    }
                    return dsassociationdata;
                }
                catch
                {
                    return null;
                }
            }
            set { dsassociationdata = value; }
        }

        private DataView dvCaptions;
        private DataView dvAssociatedObjects;
        private Thread workerThread;
        public DelegateAddNode m_DelegateAddNode;
        private Color[] nodeTextColorsArray;
        protected internal int nodeCount;
        protected internal int cachedNodesRetrieved;
        protected internal Hashtable hashCaptionNodes;
        #endregion

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion
    }

    internal enum NodeDragDropOptions
    {
        SameCaption,
        SameSequence,
        DifferentCaption,
        DifferentSequence
    }
}