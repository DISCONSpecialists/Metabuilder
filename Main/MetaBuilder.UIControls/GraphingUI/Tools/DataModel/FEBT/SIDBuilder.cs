using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Shapes.Nodes.Containers;
using Northwoods.Go;
using System.Drawing;
using MetaBuilder.Meta;
using d = MetaBuilder.DataAccessLayer;
using b = MetaBuilder.BusinessLogic;
using MetaBuilder.Graphing.Shapes;


namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.FEBT
{
    public class SIDBuilder
    {
        public string DeriveSchemaName(Cluster c)
        {
            StringBuilder sbName = new StringBuilder();
            string names = "";
            if (c.Nodes.Count > 0)
            {
                foreach (ADDNode n in c.Nodes)
                {
                    if (n is Entity)
                        sbName.Append(n.MBase.ToString()).Append(", ");
                }
                string tmpNames = sbName.ToString();
                if (tmpNames.Length > 0)
                    names = sbName.ToString().Substring(0, sbName.Length - 2);
            }
            return names;

        }
        public void BuildSID(PatternFinder pf)
        {
            Dictionary<b.MetaObjectKey, MetaBase> objects = new Dictionary<b.MetaObjectKey, MetaBase>();
            pf.FoundClusters.Sort(new ClusterComparer());
            int counter = 0;
            int prevCount = 0;
            List<MetaBase> schemas = new List<MetaBase>();

            foreach (Cluster c in pf.FoundClusters)
            {
                if (c.OutGoingLinksCount > prevCount)
                {
                    prevCount++;
                    counter++;
                }
                c.Priority = counter;
                MetaBase mb = Meta.Loader.CreateInstance("DataSubjectArea");
                mb.Set("Name", DeriveSchemaName(c));
                mb.Set("ArchitecturalPriority", counter.ToString());
                mb.Set("DataSubjectAreaType", "Logical");
                mb.tag = c;
                mb.Save(Guid.NewGuid());

                schemas.Add(mb);
            }

            b.TList<b.ObjectAssociation> oas = new MetaBuilder.BusinessLogic.TList<MetaBuilder.BusinessLogic.ObjectAssociation>();
            MetaBuilder.BusinessFacade.MetaHelper.AssociationHelper ah = MetaBuilder.BusinessFacade.MetaHelper.Singletons.GetAssociationHelper();
            bool someObjectsNotInDB = false;
            foreach (MetaBase mbDS in schemas)
            {
                Cluster tag = mbDS.tag as Cluster;
                foreach (ClusterLink clink in tag.OutgoingClusterLinks)
                {
                    DoSchemaLink(mbDS, clink, schemas, oas, ah);
                }

                foreach (ADDNode node in tag.Nodes)
                {
                    b.ObjectAssociation oa = new MetaBuilder.BusinessLogic.ObjectAssociation();
                    b.MetaObject mo = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.MetaObjectProvider.Get(new MetaBuilder.BusinessLogic.MetaObjectKey(node.MBase.pkid, node.MBase.MachineName));
                    if (mo == null)
                    {
                        someObjectsNotInDB = true;
                    }
                    oa.ObjectID = node.MBase.pkid;
                    oa.ObjectMachine = node.MBase.MachineName;
                    oa.ChildObjectID = mbDS.pkid;
                    oa.ChildObjectMachine = mbDS.MachineName;
                    oa.CAid = ah.GetAssociation("DataEntity", "DataSubjectArea", (int)LinkAssociationType.Mapping).ID;
                    oas.Add(oa);
                }

                objects.Add(new b.MetaObjectKey(mbDS.pkid, mbDS.MachineName), mbDS);

            }
            //if (!someObjectsNotInDB)
            {
                foreach (b.ObjectAssociation oa in oas)
                {
                    try
                    {
                        d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Save(oa);
                    }
                    catch
                    {
                    }
                }
            }

            GraphViewContainer container = new GraphViewContainer(MetaBuilder.BusinessLogic.FileTypeList.Diagram);
            container.FinaliseDocumentAfterLoading(true);

            Tools.LoadFromDatabase ldb = new MetaBuilder.UIControls.GraphingUI.Tools.LoadFromDatabase();
            ldb.AddObjectsUsingStencil(container.View, objects);

            container.Show(DockingForm.DockForm.dockPanel1);
            container.LayeredDigraph();

            if (someObjectsNotInDB)
                DockingForm.DockForm.DisplayTip("Some base links were not added because some Data Entites are not saved", "SID Builder");
        }
        public void DoSchemaLink(MetaBase mbDS, ClusterLink lnk, List<MetaBase> schemas, b.TList<b.ObjectAssociation> assocs, MetaBuilder.BusinessFacade.MetaHelper.AssociationHelper ah)
        {
            foreach (MetaBase mb in schemas)
            {
                if (mb.tag == lnk.To)
                {
                    int caid = ah.GetAssociation("DataSubjectArea", "DataSubjectArea", (int)LinkAssociationType.Dependency).ID;
                    b.ObjectAssociation oa = new MetaBuilder.BusinessLogic.ObjectAssociation();
                    oa.ObjectID = mbDS.pkid;
                    oa.ObjectMachine = mbDS.MachineName;
                    oa.ChildObjectID = mb.pkid;
                    oa.ChildObjectMachine = mb.MachineName;
                    oa.CAid = caid;

                    b.ObjectAssociationKey oakey = new MetaBuilder.BusinessLogic.ObjectAssociationKey(oa);
                    b.ObjectAssociation oaExisting = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Get(oakey);
                    if (oaExisting == null)
                    {
                        assocs.Add(oa);
                    }
                }
            }
        }

        GraphViewContainer myGVIEW;
        Dictionary<Cluster, GraphNode> nodeTable;
        public void createDiagram(PatternFinder pf)
        {
            Dictionary<b.MetaObjectKey, MetaBase> objects = new Dictionary<b.MetaObjectKey, MetaBase>();
            pf.FoundClusters.Sort(new ClusterComparer());
            int counter = 0;
            int prevCount = 0;
            nodeTable = new Dictionary<Cluster, GraphNode>();

            myGVIEW = new GraphViewContainer(MetaBuilder.BusinessLogic.FileTypeList.Diagram);
            myGVIEW.FinaliseDocumentAfterLoading(true);
            myGVIEW.Show(DockingForm.DockForm.dockPanel1);

            //Node
            foreach (Cluster c in pf.FoundClusters)
            {
                GraphNode schema = (GraphNode)DockingForm.DockForm.GetShape("DataSubjectArea").Copy() as GraphNode;
                schema.Location = c.Nodes[0].MyGoObjects[0].Location;

                GoCollection pCol = new GoCollection();
                pCol.Add(c.Nodes[0].MyGoObjects[0]);
                MetaBuilder.Graphing.Formatting.FormattingManipulator forman = new MetaBuilder.Graphing.Formatting.FormattingManipulator(pCol);
                GoCollection cCol = new GoCollection();
                cCol.Add(schema);
                forman.ApplyToSelection(cCol);

                if (c.OutGoingLinksCount > prevCount)
                {
                    prevCount++;
                    counter++;
                }
                c.Priority = counter;
                MetaBase mb = Meta.Loader.CreateInstance("DataSubjectArea");

                schema.MetaObject = mb;
                schema.HookupEvents();

                mb.Set("Name", DeriveSchemaName(c));
                mb.Set("ArchitecturalPriority", counter.ToString());
                mb.Set("DataSubjectAreaType", "Logical");
                mb.tag = c;

                myGVIEW.View.Document.Add(schema);

                nodeTable.Add(c, schema);

                schema.ReferenceNodes = new List<GraphNode>();
                foreach (ADDNode node in c.Nodes)
                {
                    schema.ReferenceNodes.Add(node.MyGoObjects[0] as GraphNode);
                }
                schema.ReferenceAssociation = LinkAssociationType.Mapping;
            }

            //Link
            foreach (KeyValuePair<Cluster, GraphNode> mbDS in nodeTable)
            {
                Cluster tag = mbDS.Key as Cluster;
                foreach (ClusterLink lnk in tag.OutgoingClusterLinks)
                {
                    foreach (KeyValuePair<Cluster, GraphNode> mb in nodeTable)
                    {
                        if (mb.Key == lnk.To)
                        {
                            MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation fromlocation = (MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation)Enum.Parse(typeof(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation), Core.Variables.Instance.DefaultFromPort);
                            MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation tolocation = (MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation)Enum.Parse(typeof(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation), Core.Variables.Instance.DefaultToPort);

                            QLink link = QLink.CreateLink(mbDS.Value as GoNode, mb.Value as GoNode, (int)LinkAssociationType.Dependency, fromlocation, tolocation);
                            myGVIEW.View.Document.Add(link);
                        }
                    }
                }
            }

            myGVIEW.cropGlobal();
        }
    }

    public class ClusterComparer : IComparer<Cluster>
    {
        public int Compare(Cluster c1, Cluster c2)
        {

            //return y.Freq - x.Freq; //descending sort
            if (c1.OutGoingLinksCount < c2.OutGoingLinksCount)
            {
                return -1;
            }
            if (c1.OutGoingLinksCount == c2.OutGoingLinksCount)
            {
                return 0;
            }
            return 1;
        }
    }
}
