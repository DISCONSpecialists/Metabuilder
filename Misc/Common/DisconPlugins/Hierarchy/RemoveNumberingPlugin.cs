using System.Collections.Generic;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.PluginSDK;
using Northwoods.Go;

namespace DisconPlugins.Hierarchy
{
    public class RemoveNumberingPlugin : IPlugin
    {

        #region Properties (1)

        public string Name
        {
            get { return "Remove Numbering"; }
        }

        #endregion Properties

        #region Methods (1)


        // Public Methods (1) 

        public bool PerformAction(IPluginContext context)
        {
            GoDocument doc = context.CurrentGraphView.Document;
            foreach (GoObject o in doc)
            {
                if (o is NumberingText)
                {
                    o.Remove();
                }

                if (o is IMetaNode && o is GoNode)
                {
                    //if (node is IMetaNode && (node as IMetaNode).MetaObject != null)
                    //    (node as IMetaNode).MetaObject.Set("DataSourceID", "");

                    if (o is MetaBuilder.Graphing.Shapes.Nodes.Containers.ILinkedContainer)
                    {
                        removeTextWithinContainerNodes(o as MetaBuilder.Graphing.Shapes.Nodes.Containers.ILinkedContainer);
                    }
                    else
                    {
                        List<GoObject> objsToRemove = new List<GoObject>();

                        GoNode node = o as GoNode;
                        GoGroupEnumerator enumerator = node.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            if (enumerator.Current is GoText)
                            {
                                GoText txt = enumerator.Current as GoText;
                                if (txt.Maximum == 1999)
                                    objsToRemove.Add(enumerator.Current);
                            }
                            else if (enumerator.Current is NumberingText)
                            {
                                objsToRemove.Add(enumerator.Current);
                            }
                        }

                        //float heightToSubtractFromTop = 0;
                        for (int i = 0; i < objsToRemove.Count; i++)
                        {
                            //heightToSubtractFromTop += objsToRemove[i].Height;
                            node.Remove(objsToRemove[i]);
                            //objsToRemove[i].Remove();
                        }
                        objsToRemove.Clear();
                        objsToRemove = null;
                        //foreach (GoObject o in node)
                        //    if (o is GraphNodeGrid)
                        //        o.Height = o.Height - heightToSubtractFromTop;
                    }
                }
            }
            return false;
        }
        private void removeTextWithinContainerNodes(MetaBuilder.Graphing.Shapes.Nodes.Containers.ILinkedContainer container)
        {
            if (container is MetaBuilder.Graphing.Shapes.Nodes.Containers.SubgraphNode || container is MetaBuilder.Graphing.Shapes.Nodes.Containers.ValueChain)
            {
                foreach (GoObject o in (container as GoNode))
                {
                    if (o is NumberingText)
                    {
                        o.Remove();
                    }

                    if (o is IMetaNode && o is GoNode)
                    {
                        //if (node is IMetaNode && (node as IMetaNode).MetaObject != null)
                        //    (node as IMetaNode).MetaObject.Set("DataSourceID", "");

                        if (o is MetaBuilder.Graphing.Shapes.Nodes.Containers.ILinkedContainer)
                        {
                            removeTextWithinContainerNodes(o as MetaBuilder.Graphing.Shapes.Nodes.Containers.ILinkedContainer);
                        }
                        else
                        {
                            List<GoObject> objsToRemove = new List<GoObject>();

                            GoNode node = o as GoNode;
                            GoGroupEnumerator enumerator = node.GetEnumerator();
                            while (enumerator.MoveNext())
                            {
                                if (enumerator.Current is GoText)
                                {
                                    GoText txt = enumerator.Current as GoText;
                                    if (txt.Maximum == 1999)
                                        objsToRemove.Add(enumerator.Current);
                                }
                                else if (enumerator.Current is NumberingText)
                                {
                                    objsToRemove.Add(enumerator.Current);
                                }
                            }

                            for (int i = 0; i < objsToRemove.Count; i++)
                            {
                                objsToRemove[i].Remove();
                            }
                            objsToRemove.Clear();
                            objsToRemove = null;
                        }
                    }
                }
            }
        }
        #endregion Methods

    }
}
