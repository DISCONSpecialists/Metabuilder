using System;
using System.Collections.Generic;
using System.Text;
using MetaBuilder.Meta;
using d = MetaBuilder.DataAccessLayer;
using b = MetaBuilder.BusinessLogic;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Nodes;
using Northwoods.Go;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.DSD
{
    public class DSDTable
    {
        private Entity myEntity;
        public Entity MyEntity
        {
            get { return myEntity; }
            set { myEntity = value; }
        }

        private MetaBase myMB;
        public MetaBase MyMB
        {
            get { return myMB; }
            set { myMB = value; }
        }
    }

    public class DSDBuilder
    {
        private MetaBuilder.BusinessFacade.MetaHelper.AssociationHelper ahelper;
        private Engine myEngine;
        public Engine MyEngine
        {
            get { return myEngine; }
            set { myEngine = value; }
        }

        List<Entity> entities;
        //List<XORNode> xors;
        List<DSDTable> tables;
        public void SaveToDB()
        {
#if DEBUG
            //CreateDiagram();
            //return;
#endif

            //rationalesAdded = new List<MetaBase>();

            ahelper = MetaBuilder.BusinessFacade.MetaHelper.Singletons.GetAssociationHelper();
            tables = new List<DSDTable>();
            entities = new List<Entity>();
            foreach (ADDNode n in myEngine.addnodes)
            {
                if (n is Entity)
                {
                    entities.Add(n as Entity);
                }
            }

            entities.Sort(new EntitySorter());
            foreach (Entity e in entities)
            {
                //  if (!IsXORTable(e))
                CreateTable(e);
            }
            /*
            foreach (Entity e in entities)
            {
                if (IsXORTable(e))
                    CreateXORTable(e); 
            }
            */
            foreach (DSDTable dtable in tables)
            {
                rationalesAdded = new List<MetaBase>();
                if (IsXORTable(dtable.MyEntity))
                {
                    CreateXORLink(dtable);
                }
                else
                {
                    CreateDefaultLinks(dtable);
                }
            }
            Dictionary<b.MetaObjectKey, MetaBase> objects = new Dictionary<MetaBuilder.BusinessLogic.MetaObjectKey, MetaBase>();
            foreach (DSDTable t in tables)
            {
                b.MetaObjectKey key = new MetaBuilder.BusinessLogic.MetaObjectKey(t.MyMB.pkid, t.MyMB.MachineName);
                if (!objects.ContainsKey(key))
                {
                    objects.Add(key, t.MyMB);
                }
            }

            GraphViewContainer container = new GraphViewContainer(MetaBuilder.BusinessLogic.FileTypeList.Diagram);
            container.FinaliseDocumentAfterLoading(true);
            container.Show(DockingForm.DockForm.dockPanel1);

            Tools.LoadFromDatabase ldb = new MetaBuilder.UIControls.GraphingUI.Tools.LoadFromDatabase();
            ldb.AddObjectsUsingStencil(container.View, objects);

            container.LayeredDigraph();
        }

        private void CreateDefaultLinks(DSDTable dtable)
        {
            foreach (Relation r in dtable.MyEntity.DependentRelations)
            {
                if (r.RelationshipType == LinkAssociationType.Dependency && (r.InferenceType == DependencyType.Ordinary || r.InferenceType == DependencyType.Reflexive))
                {
                    foreach (DSDTable myTable in tables)
                    {
                        if (myTable.MyEntity == r.To)
                        {
                            CreateLink(myTable.MyMB, dtable.MyMB, LinkAssociationType.ZeroOrMore_To_One);
                        }
                    }
                }
                else if (r.RelationshipType == LinkAssociationType.Dependency && (r.InferenceType == DependencyType.Transitive || r.InferenceType == DependencyType.Augmentative))
                {
                    foreach (DSDTable myTable in tables)
                    {
                        if (myTable.MyEntity == r.To)
                        {
                            CreateLink(myTable.MyMB, dtable.MyMB, LinkAssociationType.ZeroOrMore_To_One);
                        }
                    }
                }
            }
        }
        private b.ObjectAssociation CreateLink(MetaBase from, MetaBase to, LinkAssociationType linkType)
        {
            b.ObjectAssociation oa = new MetaBuilder.BusinessLogic.ObjectAssociation();
            int CAID = ahelper.GetAssociationTypeForParentChildClass(from._ClassName, to._ClassName, linkType);
            oa.CAid = CAID;
            oa.ObjectMachine = from.MachineName;
            oa.ObjectID = from.pkid;
            oa.ChildObjectMachine = to.MachineName;
            oa.ChildObjectID = to.pkid;
            b.ObjectAssociationKey oaKey = new MetaBuilder.BusinessLogic.ObjectAssociationKey(oa);
            b.ObjectAssociation oaExisting = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Get(oaKey);
            if (oaExisting == null)
            {
                d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Save(oa);
                return oa;
            }
            else
            {
                return oaExisting;
            }

        }
        private void CreateXORLink(DSDTable dtable)
        {
            foreach (Relation r in dtable.MyEntity.DependentRelations)
            {
                List<Attr> selectorAttrib = new List<Attr>();
                if (r.RelationshipType == LinkAssociationType.SubSetOf || r.RelationshipType == LinkAssociationType.MutuallyExclusiveLink)
                {
                    foreach (DSDTable myTable in tables)
                    {
                        ADDNode n = r.To as ADDNode;
                        ADDNode nodeTo = n;
                        if (n is XORNode)
                        {
                            n.StoreSubsetEntityNames();
                            if (n.DependentRelations.Count > 0)
                            {
                                nodeTo = n.DependentRelations[0].To as ADDNode;
                            }
                        }
                        Entity e = nodeTo as Entity;
                        b.ObjectAssociation oaX = CreateLink(dtable.MyMB, GetTable(e).MyMB, LinkAssociationType.Zero_To_One);
                        MetaBase mbRationale = Loader.CreateInstance("Rationale");
                        mbRationale.Set("UniqueRef", n.SelectorAttributeName);
                        if (n is XORNode)
                        {
                            string type = n.MBase.Get("SubsetIndicatorType") != null ? "(" + n.MBase.Get("SubsetIndicatorType").ToString().ToUpper() + ")" : "(XOR)";
                            mbRationale.Set("UniqueRef", n.SelectorAttributeName + " " + type);
                            //mbRationale.Set("CustomField1", n.SubsetNamesAsString);
                            //mbRationale.Set("LongDescription", "If there is a child record in a Subset Table, the other Subset Tables cannot contain a related record");
                        }
                        else
                        {
                            QLink slink = r.MyGoObject as QLink;
                            List<IMetaNode> arts = slink.GetArtefacts();
                            bool found = false;
                            foreach (IMetaNode anode in arts)
                            {
                                if (anode.BindingInfo != null)
                                {
                                    if (anode.BindingInfo.BindingClass == "SelectorAttribute")
                                    {
                                        found = true;
                                        mbRationale.Set("Value", anode.MetaObject.ToString());
                                        mbRationale.Set("UniqueRef", anode.MetaObject.ToString());
                                    }
                                }
                            }
                            if (!found)
                            {
                                mbRationale.Set("Value", "SubSet");
                                mbRationale.Set("UniqueRef", "SubSet");
                            }
                        }

                        mbRationale.Save(Guid.NewGuid());
                        CreateArtefact(n, oaX, mbRationale);

                        MetaBase mbAttr = r.SelectorAttribute;
                        if (mbAttr != null)
                        {
                            Attr a = new Attr("");
                            a.Name = mbAttr.ToString();
                            if (!selectorAttrib.Contains(a))
                                selectorAttrib.Add(a);
                        }
                    }
                }
                // AddColumns(dtable.MyMB, selectorAttrib, null, false);
            }
        }
        List<MetaBase> rationalesAdded = null;
        public bool addedRationale(ADDNode xor, MetaBase mbRationale)
        {
            foreach (MetaBase rat in rationalesAdded)
                if (rat.ToString() == mbRationale.ToString())
                    return true;

            return false;
        }
        private void CreateArtefact(ADDNode xor, b.ObjectAssociation oaX, MetaBase mbRationale)
        {
            if (addedRationale(xor, mbRationale))
                return;
            //if (xor.Artefact != null)
            //{
            //    if (xor.Artefact.ToString() == mbRationale.ToString())
            //    {
            //        mbRationale = xor.Artefact;
            //    }
            //}

            b.Artifact art = new MetaBuilder.BusinessLogic.Artifact();
            art.ObjectID = oaX.ObjectID;
            art.ObjectMachine = oaX.ObjectMachine;
            art.ChildObjectID = oaX.ChildObjectID;
            art.ChildObjectMachine = oaX.ChildObjectMachine;
            art.ArtifactObjectID = mbRationale.pkid;
            art.ArtefactMachine = mbRationale.MachineName;
            art.CAid = oaX.CAid;
            d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ArtifactProvider.Save(art);
            //if (xor.Artefact == null)
            //    xor.Artefact = mbRationale;
            rationalesAdded.Add(mbRationale);
        }

        private DSDTable GetTable(Entity e)
        {
            foreach (DSDTable t in tables)
            {
                if (t.MyEntity == e)
                {
                    return t;
                }
            }
            return null;
        }
        /*
        private void CreateXORTable(Entity e)
        {

        }*/

        private bool IsXORTable(Entity e)
        {
            foreach (Relation r in e.DependentRelations)
                if (r.RelationshipType == LinkAssociationType.SubSetOf || r.RelationshipType == LinkAssociationType.MutuallyExclusiveLink)
                    return true;

            return false;
        }
        private void CreateTable(Entity e)
        {
            DSDTable dsdTable = new DSDTable();
            Meta.MetaBase table = Meta.Loader.CreateInstance("DataTable");
            table.Set("Name", e.Name);
            table.Save(Guid.NewGuid());

            dsdTable.MyEntity = e;
            dsdTable.MyMB = table;

            List<MetaBase> keyAttribs = new List<MetaBase>();
            // if (e.DependentRelations.Count == 0)
            {
                keyAttribs.AddRange(AddColumns(table, e.KeyAttributes, null, true));
            }
            //else
            {
                // ??
            }

            List<MetaBase> descAttribs = new List<MetaBase>();

            descAttribs.AddRange(AddOrdinaryKeysAsDescriptive(table, e));
            descAttribs.AddRange(AddColumns(table, e.Attributes, null, false));
            AddSelectorAttributes(table, e);
            if (e.MBase.Get("DataEntityType").ToString().Trim() == "O")
            {
                table.Set("ContentType", "Management");
            }
            else
            {
                table.Set("ContentType", "Transaction");
            }
            tables.Add(dsdTable);
        }

        b.TList<b.ClassAssociation> TableAssociations = d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByParentClass("DataTable");
        private List<MetaBase> AddOrdinaryKeysAsDescriptive(Meta.MetaBase table, Entity e)
        {
            List<MetaBase> ordKeys = new List<MetaBase>();
            foreach (Relation r in e.DependentRelations)
            {
                if (r.InferenceType == DependencyType.Ordinary)
                {
                    Entity eTo = r.To as Entity;
                    foreach (DSDTable myTable in tables)
                    {
                        if (myTable.MyEntity == eTo)
                        {
                            //ordKeys.AddRange(AddColumns(table, eTo.KeyAttributes, myTable.MyMB, false));
                            AddColumns(table, eTo.KeyAttributes, myTable.MyMB, false);
                        }
                    }
                }
            }

            return ordKeys;
        }
        private List<MetaBase> AddColumns(Meta.MetaBase table, List<Attr> attributes, Meta.MetaBase fkTable, bool isPK)
        {
            List<MetaBase> retval = new List<MetaBase>();
            string CustomField1 = "";
            int CAID = -1;
            foreach (b.ClassAssociation classAssociation in TableAssociations)
            {
                if (classAssociation.ChildClass == "DataField" && classAssociation.IsActive == true)
                {
                    //Descriptive
                    if (isPK && classAssociation.Caption.Contains("Key"))
                    {
                        CAID = classAssociation.CAid;
                        break;
                    }
                    else if (classAssociation.Caption.Contains("Descrip"))
                    {
                        CAID = classAssociation.CAid;
                        break;
                    }
                }
            }

            if (isPK)
            {
                CustomField1 = "<PK>";
            }
            if (fkTable != null)
            { // set CustomField FK foreach attribute
                CustomField1 += "<FK>";// +fkTable.ToString() + ">";
            }

            List<string> addedAttributes = new List<string>();
            foreach (Attr a in attributes)
            {
                if (!addedAttributes.Contains(a.Name))
                {
                    MetaBase mbAttr = Loader.CreateInstance("DataField");
                    mbAttr.Set("Name", a.Name);
                    mbAttr.Set("DataFieldRole", CustomField1);
                    mbAttr.Save(Guid.NewGuid());
                    retval.Add(mbAttr);
                    addedAttributes.Add(a.Name);
                }
            }
            b.TList<b.ObjectAssociation> oas = new MetaBuilder.BusinessLogic.TList<MetaBuilder.BusinessLogic.ObjectAssociation>();
            List<int> addedPKIDs = new List<int>();
            foreach (MetaBase mbA in retval)
            {
                b.ObjectAssociation oa = new MetaBuilder.BusinessLogic.ObjectAssociation();
                oa.ObjectID = table.pkid;
                oa.ObjectMachine = table.MachineName;
                oa.ChildObjectID = mbA.pkid;
                oa.ChildObjectMachine = mbA.MachineName;
                oa.CAid = CAID;
                if (!addedPKIDs.Contains(mbA.pkid))
                {
                    addedPKIDs.Add(mbA.pkid);
                    oas.Add(oa);
                }
            }
            d.DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Save(oas);
            return retval;
        }
        private void AddSelectorAttributes(Meta.MetaBase table, Entity e)
        {
            List<Attr> attribs = new List<Attr>();
            foreach (Relation r in e.OwnerRelations)
            {
                if (r.MyGoObject != null)
                {
                    QLink ql = r.MyGoObject as QLink;
                    List<IMetaNode> arts = ql.GetArtefacts();
                    foreach (IMetaNode n in arts)
                    {
                        if (n.MetaObject._ClassName == "SelectorAttribute")
                        {
                            Attr a = new Attr(n.MetaObject.ToString());
                            bool found = false;
                            foreach (Attr aExisting in attribs)
                            {
                                if (aExisting.Name == a.Name)
                                {
                                    // Dont add
                                    found = true;
                                }
                            }
                            if (!found)
                                attribs.Add(a);
                        }
                    }
                }
            }
            AddColumns(table, attribs, null, false);
        }

        GraphViewContainer myGVIEW;
        List<IMetaNode> artsUsed;
        public void CreateDiagram()
        {
            myGVIEW = new GraphViewContainer(MetaBuilder.BusinessLogic.FileTypeList.Diagram);
            myGVIEW.FinaliseDocumentAfterLoading(true);
            myGVIEW.Show(DockingForm.DockForm.dockPanel1);

            artsUsed = new List<IMetaNode>();
            nodeTable = new Dictionary<ADDNode, GraphNode>();
            //create tables(with key/desc) from entities at position of entity
            foreach (ADDNode n in myEngine.addnodes)
            {
                if (n is Entity)
                {
                    nodeTable.Add(n, CreateNode(n as Entity));
                }
            }
            //link tables using SAME PORTS as entity
            foreach (KeyValuePair<ADDNode, GraphNode> kvp in nodeTable)
            {
                string usedRationales = "";
                if (IsXORTable(kvp.Key as Entity))
                {
                    #region XORNODES
                    foreach (Relation r in kvp.Key.DependentRelations)
                    {
                        if (r.RelationshipType == LinkAssociationType.SubSetOf || r.RelationshipType == LinkAssociationType.MutuallyExclusiveLink)
                        {
                            //foreach (DSDTable myTable in tables)
                            //{
                            ADDNode n = r.To as ADDNode;
                            ADDNode nodeTo = n;

                            MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation fromlocation = ((r.MyGoObject as QLink).FromPort as QuickPort).PortPosition;
                            MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation tolocation = ((r.MyGoObject as QLink).ToPort as QuickPort).PortPosition;

                            if (n is XORNode)
                            {
                                n.StoreSubsetEntityNames();
                                if (n.DependentRelations.Count > 0)
                                {
                                    nodeTo = n.DependentRelations[0].To as ADDNode;
                                    tolocation = ((n.DependentRelations[0].MyGoObject as QLink).ToPort as QuickPort).PortPosition;
                                }
                            }

                            Entity e = nodeTo as Entity;
                            //b.ObjectAssociation oaX = CreateLink(dtable.MyMB, GetTable(e).MyMB, LinkAssociationType.Zero_To_One);
                            QLink link = QLink.CreateLink(kvp.Value as GoNode, nodeTable[e] as GoNode, (int)LinkAssociationType.Zero_To_One, fromlocation, tolocation);
                            myGVIEW.View.Document.Add(link);

                            MetaBase mbRationale = Loader.CreateInstance("Rationale");
                            mbRationale.Set("UniqueRef", n.SelectorAttributeName);
                            if (n is XORNode)
                            {
                                string type = n.MBase.Get("SubsetIndicatorType") != null ? "(" + n.MBase.Get("SubsetIndicatorType").ToString().ToUpper() + ")" : "(XOR)";
                                mbRationale.Set("UniqueRef", n.SelectorAttributeName + " " + type);
                                //mbRationale.Set("CustomField1", n.SubsetNamesAsString);
                                //mbRationale.Set("LongDescription", "If there is a child record in a Subset Table, the other Subset Tables cannot contain a related record");
                            }
                            else
                            {
                                QLink slink = r.MyGoObject as QLink;
                                List<IMetaNode> arts = slink.GetArtefacts();
                                bool found = false;
                                foreach (IMetaNode anode in arts)
                                {
                                    if (anode.BindingInfo != null)
                                    {
                                        if (anode.BindingInfo.BindingClass == "SelectorAttribute")
                                        {
                                            if (!(artsUsed.Contains(anode)))
                                            {
                                                artsUsed.Add(anode);
                                                found = true;
                                                mbRationale.Set("Value", anode.MetaObject.ToString());
                                                mbRationale.Set("UniqueRef", anode.MetaObject.ToString());
                                            }
                                        }
                                    }
                                }
                                if (!found) //Rule failure!? or already added
                                {
                                    mbRationale.Set("Value", "SubSet");
                                    mbRationale.Set("UniqueRef", "SubSet");
                                }
                            }

                            if (!(usedRationales.Contains(mbRationale.ToString())))
                            {
                                Rationale rat = new Rationale();
                                rat.MetaObject = mbRationale;
                                rat.HookupEvents();
                                rat.BindToMetaObjectProperties();
                                rat.Anchor = link;
                                rat.Position = link.MidLabel.Position;
                                myGVIEW.View.Document.Add(rat);
                                usedRationales += mbRationale.ToString() + "|";
                            }
                            else
                            {
                                //duplicate rationale?
                            }

                            //mbRationale.Save(Guid.NewGuid());
                            //CreateArtefact(n, oaX, mbRationale);
                            //}
                        }
                        else // if it is dependency (BUG adding to dependant nodes but should be owner!)
                        {
                            MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation fromlocation = (MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation)Enum.Parse(typeof(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation), Core.Variables.Instance.DefaultFromPort);
                            MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation tolocation = (MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation)Enum.Parse(typeof(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation), Core.Variables.Instance.DefaultToPort);

                            if (r.MyGoObject != null)
                            {
                                fromlocation = ((r.MyGoObject as QLink).ToPort as QuickPort).PortPosition;
                                tolocation = ((r.MyGoObject as QLink).FromPort as QuickPort).PortPosition;
                            }

                            QLink link = QLink.CreateLink(nodeTable[r.To] as GoNode, kvp.Value as GoNode, (int)LinkAssociationType.One_To_Many, fromlocation, tolocation);
                            myGVIEW.View.Document.Add(link);
                        }
                        // AddColumns(dtable.MyMB, selectorAttrib, null, false);
                    }
                    #endregion
                }
                else
                {
                    #region Normal NODES
                    foreach (Relation r in kvp.Key.DependentRelations)
                    {
                        MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation fromlocation = (MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation)Enum.Parse(typeof(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation), Core.Variables.Instance.DefaultFromPort);
                        MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation tolocation = (MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation)Enum.Parse(typeof(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation), Core.Variables.Instance.DefaultToPort);

                        if (r.MyGoObject != null)
                        {
                            //fromlocation = ((r.MyGoObject as QLink).ToPort as QuickPort).PortPosition;
                            //tolocation = ((r.MyGoObject as QLink).FromPort as QuickPort).PortPosition;
                            fromlocation = ((r.MyGoObject as QLink).FromPort as QuickPort).PortPosition;
                            tolocation = ((r.MyGoObject as QLink).ToPort as QuickPort).PortPosition;
                        }
                        else
                        {
                            //why is it null?
                            //inference type is augmetative/reflexive?
                            r.ToString();
                            //continue;
                        }
                        if (r.RelationshipType == LinkAssociationType.Dependency && (r.InferenceType == DependencyType.Ordinary || r.InferenceType == DependencyType.Reflexive))
                        {
                            //if (myTable.MyEntity == r.To)
                            {
                                //CreateLink(myTable.MyMB, dtable.MyMB, LinkAssociationType.One_To_Many);
                                //QLink link = QLink.CreateLink(nodeTable[r.To] as GoNode, kvp.Value as GoNode, (int)LinkAssociationType.ZeroOrMore_To_One, fromlocation, tolocation);
                                QLink link = QLink.CreateLink(kvp.Value as GoNode, nodeTable[r.To] as GoNode, (int)LinkAssociationType.ZeroOrMore_To_One, fromlocation, tolocation);
                                myGVIEW.View.Document.Add(link);
                            }
                        }
                        else if (r.RelationshipType == LinkAssociationType.Dependency && (r.InferenceType == DependencyType.Transitive || r.InferenceType == DependencyType.Augmentative))
                        {
                            //if (myTable.MyEntity == r.To)
                            {
                                //CreateLink(myTable.MyMB, dtable.MyMB, LinkAssociationType.One_To_Many);
                                QLink link = QLink.CreateLink(kvp.Value as GoNode, nodeTable[r.To] as GoNode, (int)LinkAssociationType.ZeroOrMore_To_One, fromlocation, tolocation);
                                myGVIEW.View.Document.Add(link);
                            }
                        }
                        else
                        {
                            r.ToString();
                        }
                    }
                    #endregion
                }
            }

            myGVIEW.cropGlobal();
        }

        Dictionary<ADDNode, GraphNode> nodeTable;
        private GraphNode CreateNode(Entity e)
        {
            GraphNode table = (GraphNode)DockingForm.DockForm.GetShape("DataTable").Copy() as GraphNode;
            table.Location = e.MyGoObjects[0].Location;

            table.ReferenceAssociation = LinkAssociationType.Mapping;
            table.ReferenceNodes = new List<GraphNode>();
            table.ReferenceNodes.Add(e.MyGoObjects[0] as GraphNode);

            GoCollection pCol = new GoCollection();
            pCol.Add(e.MyGoObjects[0]);
            MetaBuilder.Graphing.Formatting.FormattingManipulator forman = new MetaBuilder.Graphing.Formatting.FormattingManipulator(pCol);
            GoCollection cCol = new GoCollection();
            cCol.Add(table);
            forman.ApplyToSelection(cCol);

            table.MetaObject = Loader.CreateInstance("DataTable");
            table.HookupEvents();

            table.MetaObject.Set("Name", e.Name);
            if (e.MBase.Get("DataEntityType").ToString().Trim() == "O")
            {
                table.MetaObject.Set("ContentType", "Management");
            }
            else
            {
                table.MetaObject.Set("ContentType", "Transaction");
            }

            //keys
            foreach (Attr a in e.KeyAttributes)
            {
                IMetaNode item = table.RepeaterSections[0].AddItemFromCode() as IMetaNode;
                item.MetaObject.Set("Name", a.Name);
                item.MetaObject.Set("DataFieldRole", "<PK>");
            }
            //FK
            foreach (Relation r in e.DependentRelations)
            {
                if (r.InferenceType == DependencyType.Ordinary)
                {
                    Entity eTo = r.To as Entity;
                    foreach (ADDNode MyNode in myEngine.addnodes)
                    {
                        if (!(MyNode is Entity))
                            continue;

                        Entity MyEntity = MyNode as Entity;
                        if (MyEntity == eTo)
                        {
                            foreach (Attr a in MyEntity.KeyAttributes) //changed to key attributes
                            {
                                IMetaNode item = ItemInList(table.RepeaterSections[1], a.Name);

                                if (item == null)
                                {
                                    item = table.RepeaterSections[1].AddItemFromCode() as IMetaNode;
                                    item.MetaObject.Set("Name", a.Name);
                                    item.MetaObject.Set("DataFieldRole", "<FK>");
                                }
                                else
                                {
                                    item.MetaObject.Set("DataFieldRole", item.MetaObject.Get("DataFieldRole").ToString() + "<FK>");
                                }
                            }
                        }
                    }
                }
            }
            //descriptive
            foreach (Attr a in e.Attributes)
            {
                IMetaNode item = table.RepeaterSections[1].AddItemFromCode() as IMetaNode;
                item.MetaObject.Set("Name", a.Name);
                item.MetaObject.Set("DataFieldRole", "");
            }
            //Selector attributes
            foreach (Relation r in e.OwnerRelations)
            {
                if (r.MyGoObject != null)
                {
                    QLink ql = r.MyGoObject as QLink;
                    List<IMetaNode> arts = ql.GetArtefacts();
                    foreach (IMetaNode n in arts)
                    {
                        if (n.MetaObject._ClassName == "SelectorAttribute")
                        {
                            IMetaNode item = ItemInList(table.RepeaterSections[1], n.MetaObject.ToString());

                            if (item == null)
                            {
                                item = table.RepeaterSections[1].AddItemFromCode() as IMetaNode;
                                item.MetaObject.Set("Name", n.MetaObject.ToString());
                            }
                        }
                    }
                }
            }

            myGVIEW.View.Document.Add(table);

            table.RepeaterSections[0].Expand();
            table.RepeaterSections[1].Expand();

            return table;
        }
        private CollapsingRecordNodeItem ItemInList(CollapsingRecordNodeItemList list, string name)
        {
            foreach (GoObject o in list)
            {
                CollapsingRecordNodeItem iExisting = null;
                if (!(o is CollapsingRecordNodeItem))
                    continue;

                iExisting = o as CollapsingRecordNodeItem;

                if (iExisting.MetaObject.ToString() == name)
                {
                    return iExisting;
                }
            }
            return null;
        }


    }
}
