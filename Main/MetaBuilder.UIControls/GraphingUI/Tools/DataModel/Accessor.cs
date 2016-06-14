using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Meta;
using Northwoods.Go;
namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel
{
    public class Accessor
    {
        public List<IMetaNode> GetAllNodes(IGoCollection col)
        {
            List<IMetaNode> retval = new List<IMetaNode>();
            foreach (GoObject o in col)
                if (o is IMetaNode)
                    retval.Add(o as IMetaNode);

            return retval;
        }
        public List<IGoLink> GetAllLinks(IGoCollection col)
        {
            List<IGoLink> retval = new List<IGoLink>();
            foreach (GoObject o in col)
                if (o is IGoLink)
                    retval.Add(o as IGoLink);

            return retval;
        }

        public List<IMetaNode> GetNodesOfClass(IGoCollection col, string className)
        {
            List<IMetaNode> retval = new List<IMetaNode>();
            foreach (GoObject o in col)
            {
                if (o is IMetaNode)
                {
                    IMetaNode n = o as IMetaNode;
                    if (n.BindingInfo != null)
                    {
                        if (n.BindingInfo.BindingClass == className)
                        {
                            retval.Add(n);
                        }
                    }
                }
                if (o is IGoCollection)
                {
                    retval.AddRange(GetNodesOfClass(o as IGoCollection, className));
                }
            }
            return retval;
        }

        public List<Attr> GetAttributes(IMetaNode n, int RepeaterID)
        {
            List<Attr> retval = new List<Attr>();
            if (n is CollapsibleNode)
            {
                CollapsibleNode cnode = n as CollapsibleNode;
                //int foundRepeaters = 0;
                if (cnode.RepeaterSections.Count > 0)
                {
                    RepeaterSection section = cnode.RepeaterSections[RepeaterID - 1];
                    foreach (GoObject o in section)
                    {
                        //Console.WriteLine(o.ToString());
                        if (o is CollapsingRecordNodeItem)
                        {
                            CollapsingRecordNodeItem item = o as CollapsingRecordNodeItem;
                            if (!item.IsHeader)
                            {
                                Attr attr = new Attr(item.Label.Text);
                                attr.Key = item.MetaObject.pkid + "|" + item.MetaObject.MachineName;
                                retval.Add(attr);
                            }
                        }
                    }
                }
                else
                {
                    if (cnode.MetaObject != null)
                        Console.WriteLine(cnode.MetaObject.ToString());
                }
            }


            //   for (int i = 0;i<cnode.Count;i++)
            // {
            //if (cnode[i] is CollapsingRecordNodeItemList)
            //    foundRepeaters ++;
            //if (foundRepeaters == RepeaterID)
            //{
            //RepeaterSection section = cnode.RepeaterSections[RepeaterID-1];//null
            /*if (cnode[i] is IndicatorLabel)
            {
                list = cnode[i - 1] as CollapsingRecordNodeItemList;
            }
            else
            {
                list = cnode[i] as CollapsingRecordNodeItemList;
            }
            if (list == null)
            {
                list = cnode[16] as CollapsingRecordNodeItemList;
            }
                    
            RepeaterSection section = list.First as RepeaterSection;*/

            //  }

            //}
            List<Attr> trimmedList = new List<Attr>();
            foreach (Attr a in retval)
            {
                bool found = false;
                foreach (Attr aX in trimmedList)
                {
                    if (a.Name == aX.Name)
                        found = true;
                }
                if (!found)
                    trimmedList.Add(a);
            }

            return trimmedList;
        }

        public List<Relation> GetOwnerRelations(MetaBase mb, Dictionary<MetaBase, ADDNode> entityDictionary)
        {
            return GetRelations(mb, false, entityDictionary);
        }
        public List<Relation> GetDependentRelations(MetaBase mb, Dictionary<MetaBase, ADDNode> entityDictionary)
        {
            return GetRelations(mb, true, entityDictionary);
        }
        public List<Relation> GetRelations(MetaBase mb, bool Outgoing, Dictionary<MetaBase, ADDNode> entities)
        {
            List<Relation> retval = new List<Relation>();
            foreach (KeyValuePair<MetaBase, ADDNode> kvp in entities)
            {
                if (kvp.Key == mb)
                {
                    foreach (GoNode n in kvp.Value.MyGoObjects)
                    {
                        GoNode node = n as GoNode;
                        GoNodeLinkEnumerator linkEnum = (Outgoing) ? node.DestinationLinks : node.SourceLinks;
                        while (linkEnum.MoveNext())
                        {
                            if (linkEnum.Current is QLink)
                            {
                                QLink link = linkEnum.Current as QLink;
                                if (!(link.IsInDocument))
                                {
                                    //link.ToString();
                                    continue; //ignore links not in document (Happens when you delete a link)
                                }
                                if (Outgoing)
                                {
                                    if (linkEnum.Current.ToNode != null)
                                    {
                                        if ((linkEnum.Current.ToNode as IMetaNode).MetaObject != null) //This happens when you connect to header
                                        {
                                            Relation r = new Relation(kvp.Value, entities[(linkEnum.Current.ToNode as IMetaNode).MetaObject], link.AssociationType, link, true);
                                            retval.Add(r);
                                        }
                                    }
                                }
                                else
                                {
                                    if (linkEnum.Current.FromNode != null)
                                    {
                                        if ((linkEnum.Current.FromNode as IMetaNode).MetaObject != null) //This happens when you connect to header
                                        {
                                            Relation rel = new Relation(entities[(linkEnum.Current.FromNode as IMetaNode).MetaObject], kvp.Value, link.AssociationType, link, true);
                                            retval.Add(rel);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return retval;
        }
    }
}