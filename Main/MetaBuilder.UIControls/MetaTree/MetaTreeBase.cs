using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.DataAccessLayer;
using ObjectAssociation=MetaBuilder.BusinessLogic.ObjectAssociation;

namespace MetaBuilder.UIControls.MetaTree
{
    public partial class MetaTreeBase : TreeView
    {

		#region Constructors (2) 

        public MetaTreeBase(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
            HookupEvents();
        }

        public MetaTreeBase()
        {
            InitializeComponent();
        }

		#endregion Constructors 

		#region Methods (13) 


		// Private Methods (12) 

        private void CloneCaptionNode(ClassAssociationCollectionNode tnCaption)
        {
            ObjectNode[] tn = new ObjectNode[tnCaption.Nodes.Count];
            int counter = 0;
            foreach (TreeNode tno in tnCaption.Nodes)
            {
                // Console.WriteLine("Add " + tno.Text + " to position:" + counter.ToString() + " in array");
                tn[counter] = (ObjectNode) tno.Clone();
                counter++;
            }
            /*  tnCaption.Nodes.Clear();
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
                this.BuildShallowObjectNode(tno, tno.ImageIndex);
            }*/
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
            ClassAssociationCollectionNode tnTargetCaption;
            tnTargetCaption = (ClassAssociationCollectionNode) destinationNode.Parent;
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
                oas.ObjectID = parentObjectNode.MetaObject.pkid;
                oas.ChildObjectID = cloneNode.MetaObject.pkid;
                oas.ObjectMachine = parentObjectNode.MetaObject.MachineName;
                oas.ChildObjectMachine = cloneNode.MetaObject.MachineName;
                oas.CAid = tnTargetCaption.ClassAssociation.CAid;
                Singletons.GetObjectHelper().AddObjectAssociation(oas);
                    //parentObjectNode.MetaObject.pkid, tnTargetCaption.ClassAssociation.CAid, cloneNode.MetaObject.pkid,parentObjectNode.MetaObject.MachineName,cloneNode.MetaObject.MachineName);
                cloneNode.ImageIndex = tnTargetCaption.ClassAssociation.CAid;
                SequenceAndSave(tnTargetCaption);
                SelectedNode = cloneNode;
            }
            else
            {
                MessageBox.Show(this,"The dragged node is already part of the target collection");
            }
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
            ClassAssociationCollectionNode tnTargetCaption;
            tnTargetCaption = (ClassAssociationCollectionNode) destinationNode;
            // Ensure the target caption contains items of the same class
            if (tnTargetCaption.ClassAssociation.ChildClass == newNode.MetaObject._ClassName)
            {
                foreach (TreeNode tnCaptionChildren in tnTargetCaption.Nodes)
                {
                    ObjectNode n = tnCaptionChildren as ObjectNode;
                    if (n != null)
                    {
                        if (n.MetaObject == newNode.MetaObject)
                        {
                            IsInCollection = true;
                            break;
                        }
                    }
                }
                if (!IsInCollection)
                {
                    tnTargetCaption.Nodes.Insert(0, cloneNode);
                    tnTargetCaption.Expand();
                    ObjectNode parentObjectNode = (ObjectNode) tnTargetCaption.Parent;
                    ObjectAssociation oas = new ObjectAssociation();
                    oas.ObjectID = parentObjectNode.MetaObject.pkid;
                    oas.ChildObjectID = cloneNode.MetaObject.pkid;
                    oas.ObjectMachine = parentObjectNode.MetaObject.MachineName;
                    oas.ChildObjectMachine = cloneNode.MetaObject.MachineName;
                    oas.CAid = tnTargetCaption.ClassAssociation.CAid;
                    Singletons.GetObjectHelper().AddObjectAssociation(oas);
                        //parentObjectNode.MetaObject.pkid, tnTargetCaption.ClassAssociation.CAid, cloneNode.MetaObject.pkid,parentObjectNode.MetaObject.MachineName,cloneNode.MetaObject.MachineName);
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
                MessageBox.Show(this,"Cannot drop the node here. This collection stores " +
                                tnTargetCaption.ClassAssociation.ChildClass + " objects, while the dragged node is a " +
                                newNode.MetaObject._ClassName + ".");
            }
        }

        /// <summary>
        /// Node was dragged between/onto other nodes within the same caption. Move the node before destination node
        /// </summary>
        /// <param name="destinationNode"></param>
        /// <param name="cloneNode"></param>
        /// <param name="newNode"></param>
        private void DropNodeOnNodeInSameSequence(TreeNode destinationNode, ObjectNode cloneNode, ObjectNode newNode)
        {
            ClassAssociationCollectionNode tnTargetCaption;
            tnTargetCaption = (ClassAssociationCollectionNode) destinationNode.Parent;
            tnTargetCaption.Nodes.Insert(destinationNode.Index, cloneNode);
            newNode.Remove();
            SequenceAndSave(tnTargetCaption);
            SelectedNode = cloneNode;
        }

        /// <summary>
        /// Node was dragged onto own caption. Move the node to the first child of caption node
        /// </summary>
        /// <param name="destinationNode"></param>
        /// <param name="cloneNode"></param>
        /// <param name="newNode"></param>
        private void DropNodeOnOwnCaption(TreeNode destinationNode, ObjectNode cloneNode, ObjectNode newNode)
        {
            ClassAssociationCollectionNode tnTargetCaption;
            tnTargetCaption = (ClassAssociationCollectionNode) destinationNode;
            tnTargetCaption.Nodes.Insert(0, cloneNode);
            newNode.Remove();
            SequenceAndSave(tnTargetCaption);
            SelectedNode = cloneNode;
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
                if (destinationNode is ClassAssociationCollectionNode)
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

        private void Resequence(ClassAssociationCollectionNode tnCaptionSource)
        {
            /*ObjectNode oNode = (ObjectNode) tnCaptionSource.Parent;
			int CAid = tnCaptionSource.ClassAssociation.CAid;
			ArrayList occurrences = (ArrayList) hashCaptionNodes[oNode.ObjectID.ToString() + ":" + CAid.ToString()];
			// Setup dataview to reflect only this object's associated children
            dvAssociatedObjects = Singletons.GetAssociationHelper().GetAssociatedObjects(oNode.mbase.pkid).DefaultView;
			dvAssociatedObjects.RowFilter = "Caption='" + tnCaptionSource.Text + "'";
             
			foreach (CaptionNode tnCaption in occurrences)
			{
				CloneCaptionNode(tnCaption);
			}*/
        }

        private void SequenceAndSave(ClassAssociationCollectionNode tnCaption)
        {
            ObjectNode tnParent = (ObjectNode) tnCaption.Parent;
            foreach (ObjectNode oNode in tnCaption.Nodes)
            {
                ObjectAssociation oassoc = new ObjectAssociation();
                oassoc.CAid = tnCaption.ClassAssociation.CAid;
                oassoc.ObjectID = tnParent.MetaObject.pkid;
                oassoc.ChildObjectID = oNode.MetaObject.pkid;
                oassoc.ObjectMachine = tnParent.MetaObject.MachineName;
                oassoc.ChildObjectMachine = oNode.MetaObject.MachineName;
                oassoc.Series = oNode.Index + 1;
                DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Update(oassoc);
            }
            Resequence(tnCaption);
        }

        /// <summary>
        /// Event fired when node is dropped
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void this_DragDrop(object sender, DragEventArgs e)
        {
            string objNode = typeof (ObjectNode).FullName;
            string graphObjNode = typeof (GraphObjectNode).FullName;
            if (e.Data.GetDataPresent(objNode, false) || e.Data.GetDataPresent(graphObjNode, false))
            {
                ObjectNode newNode = (ObjectNode) e.Data.GetData(objNode);
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
        /// Event fired when node was dragged outside & back again
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void this_DragEnter(object sender, DragEventArgs e)
        {
            string objNode = typeof (ObjectNode).FullName;
            string graphObjNode = typeof (GraphObjectNode).FullName;
            if (e.Data.GetDataPresent(objNode, false) || e.Data.GetDataPresent(graphObjNode, false))
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
            string objNode = typeof (ObjectNode).FullName;
            string graphObjNode = typeof (GraphObjectNode).FullName;
            if (e.Data.GetDataPresent(objNode, false) || e.Data.GetDataPresent(graphObjNode, false))
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



		// Internal Methods (1) 

        internal void HookupEvents()
        {
            ItemDrag += new ItemDragEventHandler(this_ItemDrag);
            DragDrop += new DragEventHandler(this_DragDrop);
            DragEnter += new DragEventHandler(this_DragEnter);
            DragOver += new DragEventHandler(this_DragOver);
            AllowDrop = true;
        }


		#endregion Methods 

    }

    internal enum NodeDragDropOptions
    {
        SameCaption,
        SameSequence,
        DifferentCaption,
        DifferentSequence
    }
}