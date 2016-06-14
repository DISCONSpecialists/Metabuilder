using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Graphing;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Shapes.Nodes;
using MetaBuilder.Graphing.Shapes.Nodes.Containers;

using Northwoods.Go;
using MetaBuilder.Meta;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Controllers;
using b = MetaBuilder.BusinessLogic;
using d = MetaBuilder.DataAccessLayer;

namespace MetaBuilder.UIControls.GraphingUI.SubgraphBinding
{
    internal class LinkedContainerControlller
    {
        public LinkedContainerControlller()
        {
        }
        private List<ILinkedContainer> GetILinkContainers(IGoCollection col)
        {
            List<ILinkedContainer> retval = new List<ILinkedContainer>();

            foreach (GoObject obj in col)
            {
                if (obj is ILinkedContainer)
                {
                    retval.Add(obj as ILinkedContainer);
                }
                if (obj is IGoCollection)
                {
                    retval.AddRange(GetILinkContainers(obj as IGoCollection));
                }
            }
            return retval;
        }

        public bool SelectionDropped(GoCollection selection, GoObjectEventArgs args, ref GraphView myView)
        {
            List<ILinkedContainer> containers = GetILinkContainers(myView.Document);
            if (containers.Count == 0)
                return false;

            // There are containers
            Dictionary<ILinkedContainer, List<MetaBase>> ObjectsToPromptAboutDict = new Dictionary<ILinkedContainer, List<MetaBase>>();
            //bool ParentChanged = false;
            List<ILinkedContainer> oldILCs = new List<ILinkedContainer>();
            List<ILinkedContainer> newILCs = new List<ILinkedContainer>();

            //9 January 2013 - dragging goup into subgraph must add grouped objects to subgraph
            GoCollection objexToEdd = new GoCollection();
            foreach (GoObject obj in selection)
            {
                if (obj.DraggingObject is SubgraphNode)
                    continue;
                //check if dropped object is group
                if (obj.DraggingObject is ShapeGroup || obj.DraggingObject is GoSubGraph)
                {
                    //remove from group
                    if (obj.DraggingObject is ShapeGroup)
                    {
                        foreach (GoObject objInGroup in (obj.DraggingObject as ShapeGroup))
                        {
                            //check the selection to see if it contains this object
                            if (!selection.Contains(objInGroup))
                                objexToEdd.Add(objInGroup);
                        }
                    }
                    else
                    {
                        foreach (GoObject objInGroup in (obj.DraggingObject as GoSubGraph))
                        {
                            //check the selection to see if it contains this object
                            if (!selection.Contains(objInGroup))
                                objexToEdd.Add(objInGroup);
                        }
                    }
                }
            }
            //add extra grouped objects to selection
            foreach (GoObject obj in objexToEdd)
                selection.Add(obj);

            foreach (GoObject obj in selection)
            {
                GoObject objParent = obj.Parent;
                GoObject objParentNode = obj.ParentNode;

                //June 26 2014 changed to first in selection not each obj!
                GoObject dropNode = selection.First;

                GoCollection colBeneath = new GoCollection();
                //bool hasParent = false;
                // Get all objects beneath this
                myView.PickObjectsInRectangle(true, true, dropNode.Bounds, GoPickInRectangleStyle.AnyIntersectsBounds, colBeneath, 1000);

                #region Loop through objects and setup new ILinkedContainer list

                List<GoObject> lstObjsBeneath = new List<GoObject>();
                foreach (GoObject objBeneath in colBeneath)
                {
                    if (objBeneath is ILinkedContainer || objBeneath.DraggingObject is ILinkedContainer)
                    {
                        //8 January 2013
                        //only add the top most object in accepted region
                        if (!myView.Selection.Contains(objBeneath))
                            lstObjsBeneath.Add(getTopContainer(objBeneath as ILinkedContainer, obj) as GoObject);
                    }
                }

                for (int i = 0; i < lstObjsBeneath.Count; i++)
                {
                    GoObject objInner = lstObjsBeneath[i];
                    if (objInner == null)
                        continue;
                    if (objInner.DraggingObject != obj.DraggingObject)
                    {
                        if (objInner.DraggingObject is ILinkedContainer)
                        {
                            ILinkedContainer ilCont = objInner.DraggingObject as ILinkedContainer;
                            //Console.WriteLine("\tProcessing Object Beneath:" + ilCont.MetaObject.ToString());
                            if (ilCont is ValueChain)
                            {
                                GoCollection colBeneathVC = new GoCollection();
                                ValueChain vc = ilCont as ValueChain;
                                vc.PickObjects(obj.Center, true, colBeneathVC, 50);
                                bool hasOpenVC = false;
                                foreach (GoObject o in colBeneathVC)
                                {
                                    if (o != vc)
                                    {
                                        if (o is ValueChain)
                                        {
                                            ValueChain vcInner = o as ValueChain;
                                            if (vcInner.ObjectInAcceptedRegion(obj) && (!(vcInner.Locked)))
                                            {
                                                newILCs.Clear();
                                                newILCs.Add(vcInner);
                                                //bool Found = false;
                                                hasOpenVC = true;
                                            }
                                        }
                                        lstObjsBeneath.Add(o);
                                    }
                                }
                                if (!hasOpenVC)
                                    if (ilCont.ObjectInAcceptedRegion(obj) && (!(ilCont.Locked)))
                                    {
                                        //Console.WriteLine("\t\tDropped over:" + ilCont.LabelText + " which isnt locked");
                                        newILCs.Add(ilCont);

                                        //bool Found = false;
                                    }
                            }
                            else
                            {
                                //SubGraph
                                if (ilCont.ObjectInAcceptedRegion(obj) && (!(ilCont.Locked)))
                                {
                                    if (DoesContainerContainSubgraphsAndIfSoIsObjectWithinItsBounds(ilCont, obj))
                                    {
                                        //Console.WriteLine("\t\tDropped over:" + ilCont.LabelText + " which isnt locked");
                                        //Console.WriteLine("\t\tBUT");
                                        //Console.WriteLine("\t\tContainer Overridden by " + overrideContainer.LabelText + "which isnt locked");

                                        if (overrideContainer != null)
                                        {
                                            newILCs.Clear();
                                            if (!newILCs.Contains(overrideContainer))
                                                newILCs.Add(overrideContainer);
                                        }
                                    }
                                    else
                                    {
                                        //Console.WriteLine("\t\tDropped over:" + ilCont.LabelText + " which isnt locked");
                                        if (!newILCs.Contains(ilCont))
                                            newILCs.Add(ilCont);
                                    }

                                    //bool Found = false;
                                }
                            }
                        }
                    }
                }
                #endregion

                if (obj.Parent is ILinkedContainer || obj.ParentNode is ILinkedContainer)
                {
                    //hasParent = true;
                }

                if (newILCs.Count > 0)
                {
                    myView.StartTransaction();
                    List<LinkPortSpec> linkPortSpecs = GroupingControl.GetLinkSpecs(selection);
                    Dictionary<ResizableBalloonComment, GoObject> rationales = GroupingControl.GetRationaleSpecs(selection);

                    #region Loop through new ILCs, add items and relink if necessary

                    foreach (ILinkedContainer ilcNew in newILCs)
                    {
                        GoCollection col = new GoCollection();
                        GoCollectionEnumerator selEnum = selection.GetEnumerator();
                        while (selEnum.MoveNext())
                        {
                            if ((!(ilcNew.Contains(selEnum.Current))) || ilcNew is MappingCell)
                            {
                                GoObject objToAdd = selEnum.Current;
                                if (objToAdd.ParentNode != objToAdd)
                                    objToAdd = objToAdd.ParentNode;

                                col.Add(objToAdd);

                                //ParentChanged = true;
                                AddDefaultAssociationInfoForChildNode(ilcNew, objToAdd);
                            }
                        }

                        //remove parent subgraph relationships
                        //23 January 2013
                        //When remove from ilinkedcontainer remove relationship as well
                        foreach (GoObject o in col)
                        {
                            if (o is IMetaNode)
                            {
                                if (o.Parent is SubgraphNode && o.Parent as ILinkedContainer != ilcNew)
                                {
                                    (o.Parent as SubgraphNode).RemoveOnlyRelationship(o as IMetaNode);
                                }
                            }
                        }

                        if (!(ilcNew is MappingCell))
                        {
                            foreach (GoObject o in col)
                            {
                                if (!(ilcNew.Contains(o)))
                                {
                                    o.Remove();
                                }
                            }
                        }

                        ilcNew.PerformAddCollection(myView, col);
                        if (ilcNew is SubgraphNode)
                        {
                            if (!(ilcNew as SubgraphNode).failedAdd)
                            {
                                RelinkRationalesAfterDrop(myView, rationales, selection);
                                //if (objParent == ilcNew || objParentNode == ilcNew)
                                if (objParent is SubgraphNode || objParentNode is SubgraphNode)
                                {
                                    //dont relink these
                                }
                                else
                                {
                                    RelinkPortsAfterDrop(myView, linkPortSpecs);
                                }
                            }
                        }
                    }

                    #endregion

                    myView.FinishTransaction("Drag Drop onto ILinkContainer");
                }
                else
                {
                    #region Get Old Containers and remove their EmbeddedRelationships

                    //Console.WriteLine("Identifying ALL containers with this object");
                    if (obj is IMetaNode)
                    {
                        IMetaNode imnode = obj as IMetaNode;
                        foreach (ILinkedContainer container in containers)
                        {
                            #region Setup list of old containers

                            foreach (EmbeddedRelationship embRel in container.ObjectRelationships)
                            {
                                if (embRel.MyMetaObject == imnode.MetaObject)
                                {
                                    //Console.WriteLine("\tFound:" + container.LabelText);

                                    if (newILCs.Contains(container))
                                    {
                                        //Console.WriteLine("\tNo need to add, Already Contains Object:" + container.LabelText);
                                        // do nothing
                                        newILCs.Remove(container);
                                    }
                                    else
                                    {
                                        oldILCs.Add(container);
                                        if (!(ObjectsToPromptAboutDict.ContainsKey(container)))
                                        {
                                            ObjectsToPromptAboutDict.Add(container, new List<MetaBase>());
                                        }
                                        if (!(ObjectsToPromptAboutDict[container].Contains(imnode.MetaObject)))
                                        {
                                            ObjectsToPromptAboutDict[container].Add(imnode.MetaObject);
                                        }
                                        try
                                        {
                                            if ((!container.Locked) && container == obj.ParentNode)
                                                container.Remove(obj);
                                        }
                                        catch
                                        {
                                        }
                                    }
                                }
                            }
                            #endregion

                            //Console.WriteLine("Loop through all containers with this object");
                        }

                    }
                    #endregion
                }
            }

            /*foreach (ILinkedContainer ilc in oldILCs)
            {
                if (!(newILCs.Contains(ilc)))
                {
                    if (!ilc.Locked)
                    {
                        Console.WriteLine("\tRemove from " + ilc.LabelText);
                        for (int i = 0; i < ilc.ObjectRelationships.Count; i++)
                        {
                            if (ilc.ObjectRelationships[i].MyMetaObject == imnode.MetaObject)
                            {
                                if (!(ObjectsToPromptAboutDict[ilc].Contains(imnode.MetaObject)))
                                {
                                    ObjectsToPromptAboutDict[ilc].Add(imnode.MetaObject);

                                }
                                //ilc.ObjectRelationships.RemoveAt(i);
                            }
                        }
                    }
                }
            }*/

            /* if (ParentChanged && ObjectsToPromptAboutDict.Count > 0)
             {
                 PromptKeepAssociations pka = new PromptKeepAssociations();
                 pka.ObjectsToPrompt = ObjectsToPromptAboutDict;
                 pka.BindObjectsToPrompt();
                 pka.ShowDialog(this);
             }*/
            return true;
        }

        //8 January 2013
        private ILinkedContainer getTopContainer(ILinkedContainer container, GoObject addingObject)
        {
            if (container is SubgraphNode)
            {
                SubgraphNode sg = container as SubgraphNode;

                if (!sg.IsExpanded)
                    return container;

                GoCollection col = new GoCollection();
                GoGroupEnumerator selEnum = sg.GetEnumerator();
                while (selEnum.MoveNext())
                {
                    if (selEnum.Current is ILinkedContainer)
                    {
                        col.Add(selEnum.Current);
                    }
                }

                foreach (ILinkedContainer cont in col)
                {
                    if (cont.ObjectInAcceptedRegion(addingObject as GoObject))
                    {
                        if (addingObject == cont)
                            continue;
                        return getTopContainer(cont, addingObject);
                    }
                }
            }
            else
                return container;

            return container;
        }

        public void AddDefaultAssociationInfoForChildNode(ILinkedContainer container, GoObject o)
        {
            if (container.MetaObject == null || o is ArtefactNode || o is Rationale)
                return;

            if (o is IMetaNode)
            {
                IMetaNode im = o as IMetaNode;
                if (im.MetaObject == null)
                    return;
            }
            else
                return;
            EmbeddedRelationship emrel = new EmbeddedRelationship();
            emrel.MyMetaObject = (o as IMetaNode).MetaObject;

            bool found = false;
            foreach (EmbeddedRelationship embrel in container.ObjectRelationships)
            {
                if (embrel.MyMetaObject == emrel.MyMetaObject)
                {
                    found = true;
                }
            }

            if (found)
                return;

            b.TList<b.ClassAssociation> allowedAssociations = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByChildClass(emrel.MyMetaObject._ClassName);
            allowedAssociations.Filter = "ParentClass = '" + container.MetaObject._ClassName + "' AND IsActive = 'True'";
            // set default to first, maybe there isnt one
            if (allowedAssociations.Count > 0)
            {
                emrel.MyAssociation = allowedAssociations[0];

                #region Add Class Associations for each class that is dropped

                if (container.DefaultClassBindings != null)
                {
                    foreach (KeyValuePair<string, b.ClassAssociation> kvp in container.DefaultClassBindings)
                    {
                        if (kvp.Key == container.MetaObject._ClassName)
                        {
                            emrel.MyAssociation = kvp.Value;
                            container.ObjectRelationships.Add(emrel);
                            return;
                        }
                    }
                }
                else
                {
                    container.DefaultClassBindings = new Dictionary<string, MetaBuilder.BusinessLogic.ClassAssociation>();
                }

                foreach (b.ClassAssociation classAssoc in allowedAssociations)
                {
                    if (classAssoc.IsDefault)
                    {
                        if (container.DefaultClassBindings == null)
                            container.DefaultClassBindings = new Dictionary<string, MetaBuilder.BusinessLogic.ClassAssociation>();
                        if (!(container.DefaultClassBindings.ContainsKey(classAssoc.ChildClass)))
                            container.DefaultClassBindings.Add(classAssoc.ChildClass, classAssoc);
                        emrel.MyAssociation = classAssoc;
                        container.ObjectRelationships.Add(emrel);

                        if (Core.Variables.Instance.SaveOnCreate)
                            if (LinkController.GetAssociation(container.MetaObject, emrel.MyMetaObject, (LinkAssociationType)emrel.MyAssociation.AssociationTypeID) == null)
                                LinkController.SaveAssociation(container.MetaObject, emrel.MyMetaObject, (LinkAssociationType)emrel.MyAssociation.AssociationTypeID, Core.Variables.Instance.ClientProvider);
                        //emrel.ToString();

                        return;
                    }
                }
                // at this stage, no default associations were added, because there is no default specified. Just add the first one as the default
                if (container.DefaultClassBindings == null)
                    container.DefaultClassBindings = new Dictionary<string, MetaBuilder.BusinessLogic.ClassAssociation>();
                if (!container.DefaultClassBindings.ContainsKey(emrel.MyMetaObject._ClassName))
                    container.DefaultClassBindings.Add(emrel.MyMetaObject._ClassName, allowedAssociations[0]);
                emrel.MyAssociation = allowedAssociations[0];
                container.ObjectRelationships.Add(emrel);

                if (Core.Variables.Instance.SaveOnCreate)
                    if (LinkController.GetAssociation(container.MetaObject, emrel.MyMetaObject, (LinkAssociationType)emrel.MyAssociation.AssociationTypeID) == null)
                        LinkController.SaveAssociation(container.MetaObject, emrel.MyMetaObject, (LinkAssociationType)emrel.MyAssociation.AssociationTypeID, Core.Variables.Instance.ClientProvider);
                //emrel.ToString();
                return;

                #endregion
            }
            else
            {
                //add task
                DockingForm.DockForm.GetCurrentGraphViewContainer().CreateInvalidMetaModelSubgraphChildTask(o as IMetaNode);
            }

        }
        public void AddAssociationsWhichAreInDatabase(ILinkedContainer container)
        {
            foreach (b.ObjectAssociation ass in d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(container.MetaObject.pkid, container.MetaObject.MachineName))
            {
                EmbeddedRelationship emrel = new EmbeddedRelationship();
                emrel.MyAssociation = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByCAid(ass.CAid);
                string cls = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.GetBypkidMachine(ass.ChildObjectID, ass.ChildObjectMachine).Class;
                emrel.MyMetaObject = Loader.GetByID(cls, ass.ChildObjectID, ass.ChildObjectMachine);
                bool add = true;
                foreach (EmbeddedRelationship emrelOnDiagram in container.ObjectRelationships)
                {
                    if (emrelOnDiagram.MyAssociation.CAid == emrel.MyAssociation.CAid && emrelOnDiagram.MyMetaObject.pkid == emrel.MyMetaObject.pkid && emrelOnDiagram.MyMetaObject.MachineName == emrel.MyMetaObject.MachineName)
                    {
                        emrelOnDiagram.FromDatabase = false;
                        add = false;
                        break;
                    }
                }
                emrel.FromDatabase = true; //this means it is not on the current diagram
                if (add)
                    container.ObjectRelationships.Add(emrel);
            }
        }
        private void RelinkRationalesAfterDrop(GoView view, Dictionary<ResizableBalloonComment, GoObject> rationales, GoCollection col)
        {
            foreach (KeyValuePair<ResizableBalloonComment, GoObject> kvp in rationales)
            {
                kvp.Key.Anchor = kvp.Value;
            }
        }
        private void RelinkPortsAfterDrop(GoView view, List<LinkPortSpec> linkPortSpecs)
        {
            foreach (LinkPortSpec lps in linkPortSpecs)
            {
                lps.Relink();
                if (view.Document.Contains(lps.Link as GoLink))
                {
                    // do nothing
                }
                else
                {
                    lps.Link.GoObject.Remove();
                    if (lps.Link is FishLink)
                    {
                        FishLink lnk = lps.Link as FishLink;
                        if (!(view.Document.Contains(lnk)))
                            view.Document.Add(lnk);
                    }
                    if (lps.Link is QLink)
                    {
                        if (view.Document.Contains(lps.Link.ToNode as GoNode) && view.Document.Contains(lps.Link.FromNode as GoNode))
                        {
                            QLink sl = lps.Link as QLink;
                            if (!view.Document.Contains(sl))
                                view.Document.Add(sl);
                        }
                        else
                        {
                            QLink sl2 = lps.Link as QLink;
                            if (!view.Document.Contains(sl2))
                                view.Document.Add(sl2);
                        }
                    }
                    lps.AddFishLinks(view.Document);
                }
            }
        }

        private ILinkedContainer overrideContainer;
        private bool DoesContainerContainSubgraphsAndIfSoIsObjectWithinItsBounds(ILinkedContainer parentContainer, GoObject objectDropped)
        {
            if (!(parentContainer is SubgraphNode))
                return false;

            SubgraphNode parentSubgraph = parentContainer as SubgraphNode;
            bool returning = false;

            foreach (GoObject o in parentSubgraph.GetEnumerator())
            {
                //TODO : some null thing here
                //while (parentSubgraph.Nodes.GetEnumerator().MoveNext())
                //{
                //if (parentSubgraph.Nodes.Current is SubgraphNode)
                if (o is SubgraphNode)
                {
                    //Console.WriteLine("\t\t\tFound subgraph in Subgraph " + parentSubgraph.LabelText);

                    SubgraphNode childSubgraph = o as SubgraphNode;
                    if (objectDropped != childSubgraph && !childSubgraph.Locked && childSubgraph.ObjectInAcceptedRegion(objectDropped))
                    {
                        returning = true;
                        overrideContainer = childSubgraph as ILinkedContainer;
                        DoesContainerContainSubgraphsAndIfSoIsObjectWithinItsBounds(childSubgraph as ILinkedContainer, objectDropped);
                    }
                }
            }

            return returning;
        }
    }
}