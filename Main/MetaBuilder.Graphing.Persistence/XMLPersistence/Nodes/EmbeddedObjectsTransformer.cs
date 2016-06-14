using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Behaviours;
using MetaBuilder.Graphing.Shapes.Behaviours.Observers;
using MetaBuilder.Graphing.Shapes.Nodes.Containers;
using MetaBuilder.Graphing.Shapes.Primitives;
using MetaBuilder.Meta;
using Northwoods.Go;
using Northwoods.Go.Xml;
using MetaBuilder.Graphing.Shapes.Nodes;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Nodes
{
    public abstract class EmbeddedObjectsTransformer : BaseGoObjectTransformer
    {

        #region Methods (7)

        // Public Methods (5) 

        public override void ConsumeChild(object parent, object child)
        {
            IGoCollection node = parent as IGoCollection;
            if (child != null)
            {

                if (child is GoObject)
                {
                    GoObject o = child as GoObject;
                    o.Remove();
                    if (o is GoText)
                    {
                        if (node is GraphNode)
                        {
                            GraphNode gnode = node as GraphNode;
                            GoText txt = o as GoText;
                            bool originalEditmode = gnode.EditMode;
                            if (txt.Maximum != 1999)
                            {
                                gnode.EditMode = false;

                                gnode.Add(txt);
                                gnode.EditMode = originalEditmode;
                            }
                            else
                            {
                                txt.ToString();
                            }
                        }
                        else
                            node.Add(o);
                    }
                    else
                        node.Add(o);
                }
            }

            if (node is IMetaNode)
            {
                IMetaNode mNode = node as IMetaNode;
                if (child is MetaBase)
                {
                    mNode.MetaObject = child as MetaBase;
                    if (mNode is GraphNode)
                    {
                        GraphNode gnode = mNode as GraphNode;
                        gnode.EditMode = false;
                    }
                    mNode.BindingInfo.BoundObjectID = mNode.MetaObject.pkid;
                }
            }
            base.ConsumeChild(parent, child);
        }

        public override void ConsumeObjectFinish(object obj)
        {
            base.ConsumeObjectFinish(obj);
            IGoCollection collection = obj as IGoCollection;

            if (obj is IMetaNode)
            {
                IMetaNode imnode = obj as IMetaNode;
                imnode.HookupEvents();
            }
        }

        public IIdentifiable FindByName(IGoCollection collection, string name)
        {
            //bool debugSwitcher = false;
            //if (debugSwitcher)
            MetaBuilder.Graphing.Helpers.Debuggers.ShapeHelper.DebugObjectToXml(collection, "c:\\debug.xml");
            foreach (GoObject o in collection)
            {
                if (o is IIdentifiable)
                {
                    IIdentifiable retval = o as IIdentifiable;
                    if (retval.Name == name)
                        return retval;
                }

                if (o is IGoCollection)
                {
                    IIdentifiable embedded = FindByName(o as IGoCollection, name);
                    if (embedded != null)
                        return null;
                }
            }
            return null;
        }

        public override void GenerateBody(object obj)
        {
            GenerateBody(obj, true, true);
        }

        public void GenerateBody(object obj, bool IncludeMetaObjects, bool IncludeGoObjects)
        {
            IGoCollection n = (IGoCollection)obj;
            if (obj is IMetaNode)
            {
                Writer.GenerateObject((obj as IMetaNode).MetaObject);
            }

            if (!DontGenerateChildren(obj)) //Stop code generated items from saving their children!
            {
                foreach (object child in n)
                {
                    if (child is MetaPropertyList || child is MetaPropertyNodeRow)
                        continue;

                    if (child is GoObject)
                    {
                        if (IncludeGoObjects)
                        {
                            GoObject goObj = child as GoObject;
                            if (n is ValueChain)
                                if (!DontGenerateChild(goObj))
                                    Writer.GenerateObject(goObj);

                            if (!(goObj is GraphNodeGrid) && (!(n is ValueChain)))
                                Writer.GenerateObject(goObj);
                        }
                        if (child is IMetaNode && (child is CollapsingRecordNodeItem) && (!(child is SubgraphNode)))
                        {
                            if (!(child as CollapsingRecordNodeItem).IsHeader)
                            {
                                Writer.GenerateObject((child as IMetaNode).MetaObject);
                                Writer.GenerateObject((child as CollapsingRecordNodeItem).GetLabel);
                            }
                        }
                    }
                }
            }
        }

        // Private Methods (2)

        private bool DontGenerateChild(GoObject obj)
        {
            if (obj.Parent != null)
            {
                if (obj.Parent is ValueChain)
                {
                    return (obj is GoText || obj is GoRectangle || obj is GoPolygon || obj is GoHandle || obj is BoundLabel || obj is GradientPolygon);// || obj is GoImage);
                }
            }
            return false;
        }

        private bool DontGenerateChildren(Object o)
        {
            return (o is ResizableBalloonComment || o is ResizableComment || o is ImageNode || o is QLink || o is FishLink);
        }

        #endregion Methods

        /*
           private void RestoreAnchors(IGoCollection collection)
           {
               if (anchors == null)
                   return;

               Type tLock = typeof(PositionLockLocation);
               if (anchors != string.Empty)
               {
                   char splitChar = '|';
                   string[] anchorspecs = anchors.Split(splitChar);
                   for (int i = 0; i < anchorspecs.Length - 1; i++)
                   {
                       char itemsplitchar = ',';
                       string[] spec = anchorspecs[i].Split(itemsplitchar);

                       IIdentifiable observer = FindByName(collection, spec[0]);
                       IIdentifiable observed =  FindByName(collection, spec[1]);
                       if (observed is GoObject)
                       {
                           GoObject observedObj = observed as GoObject;

                           GoObject observerObj = observer as GoObject;

                           PositionLockLocation lockLocation = (PositionLockLocation)Enum.Parse(tLock, spec[2]);
                           AnchorPositionBehaviour apbehaviour =
                               new AnchorPositionBehaviour(observed, observerObj, lockLocation);
                           IBehaviourShape ibshape = observer as IBehaviourShape;
                           // Console.WriteLine("Adding a behaviour for Spec 0 {0} Spec 1 {1}",
                                             new object[] { spec[0], spec[1] });
                           // Console.WriteLine("Adding a behaviour for Observed {0} Observer {1}",
                                             new object[] {observed, observerObj});
                           ibshape.Manager.AddBehaviour(apbehaviour);
                           ibshape.Manager.Enabled = true;
                           observedObj.AddObserver(observer as GoObject);
                       }
                   }
               }
           }
           private void StoreAnchorInfo(IGoCollection collection)
           {
               Dictionary<IIdentifiable, AnchorPositionBehaviour> anchorBehaviours =
                   new Dictionary<IIdentifiable, AnchorPositionBehaviour>();
               foreach (GoObject child in collection)
               {
                   if (child is IBehaviourShape)
                   {
                       IBehaviourShape iidChild = child as IBehaviourShape;

                       AnchorPositionBehaviour apb =
                           iidChild.Manager.GetExistingBehaviour(typeof(AnchorPositionBehaviour)) as
                           AnchorPositionBehaviour;
                       if (apb != null)
                       {
                           if (child is IIdentifiable)
                           {
                               IIdentifiable identChild = child as IIdentifiable;
                               anchorBehaviours.Add(identChild, apb);
                           }
                       }
                   }
               }
               if (anchorBehaviours.Count > 0)
               {
                   StringBuilder anchors = new StringBuilder();
                   foreach (KeyValuePair<IIdentifiable, AnchorPositionBehaviour> kvp in anchorBehaviours)
                   {
                       anchors.Append(kvp.Key.Name + "," + kvp.Value.MyObserved.Name + "," + kvp.Value.LockLocation + "|");
                   }
                   WriteAttrVal("Anchors", anchors.ToString());
               }
           }
           */

    }
}