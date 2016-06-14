using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;
using MetaBuilder.Meta;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.UIControls.GraphingUI.Tools.DataModel;
using MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Inference;
using MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation;
using MetaBuilder.UIControls.GraphingUI.Tools.DataModel.UI;
using MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation.Basic;
using MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation.ADD;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel
{
    public class Engine
    {
        //public Engine()
        //{
        //}

        private GraphView myview;
        public GraphView MyView
        {
            get { return myview; }
            set { myview = value; }
        }

        private NormalDiagram diagram;
        public NormalDiagram Diagram
        {
            get { return diagram; }
            set { diagram = value; }
        }

        private InferenceRulesOptions options;
        public InferenceRulesOptions Options
        {
            get { return options; }
            set { options = value; }
        }

        public List<IMetaNode> entityNodes;
        public List<IMetaNode> xors;
        public List<IMetaNode> selectorAttributes;
        public List<QLink> QLinks;
        public List<IMetaNode> dependencyDescriptions;

        public Dictionary<MetaBase, ADDNode> graphNodeEntityDictionary;

        public List<Validation.BaseRule> validationRules;
        public List<Inference.BaseRule> inferenceRules;
        public List<ADDNode> addnodes;
        public void SafelyAddRelation(Relation r)
        {
            bool exists = false;
            foreach (Relation rExisting in relations)
            {
                if (r.To == rExisting.To && r.From == rExisting.From)
                {
                    exists = true;
                }
            }
            if (!exists)
                relations.Add(r);
        }
        public List<Relation> GetRelations()
        {
            return relations;
        }
        private List<Relation> relations;
        public Model m;
        public Engine(GraphView myview, NormalDiagram diagram)
        {
            this.MyView = myview;
            this.Diagram = diagram;

            ReadDataFromDiagram(diagram);
            DeterminePriorities(diagram);

            BuildKeysetsAugmentation();

            BuildRules(diagram.DiagramType);

            BuildValidationRuleList();
            BuildInferenceRuleList();

            ApplyValidation();
        }

        private void DeterminePriorities(MetaBuilder.Graphing.Containers.NormalDiagram diagram)
        {
            m = new Model(addnodes, relations);
            m.CycleAll();
        }

        private void ReadDataFromDiagram(NormalDiagram diagram)
        {
            Accessor accessor = new Accessor();
            entityNodes = accessor.GetNodesOfClass(diagram, "Entity");
            entityNodes.AddRange(accessor.GetNodesOfClass(diagram, "DataEntity"));
            xors = accessor.GetNodesOfClass(diagram, "MutuallyExclusiveIndicator");
            xors.AddRange(accessor.GetNodesOfClass(diagram, "SubsetIndicator"));
            selectorAttributes = accessor.GetNodesOfClass(diagram, "SelectorAttribute");
            dependencyDescriptions = accessor.GetNodesOfClass(diagram, "DependencyDescription");
            addnodes = new List<ADDNode>();
            graphNodeEntityDictionary = new Dictionary<MetaBase, ADDNode>();

            foreach (IMetaNode entityNode in entityNodes)
            {
                //if (entityNode is IMetaNode)
                //{
                IMetaNode imnode = entityNode as IMetaNode;
                MetaBase mb = imnode.MetaObject;
                object s = mb.Get("Name");
                Entity e = null;
                if (s != null)
                    e = new Entity(s.ToString());
                else
                    e = new Entity("");
                e.MBase = mb;
                e.KeyAttributes = accessor.GetAttributes(entityNode, 1);
                e.Attributes = accessor.GetAttributes(entityNode, 2);
                e.AddGoObject(entityNode as GoNode);

                if (!graphNodeEntityDictionary.ContainsKey(mb))
                {
                    //e.AddGoObject(imnode as GoNode);
                    graphNodeEntityDictionary.Add(mb, e);
                    addnodes.Add(e);
                }
                else
                {
                    graphNodeEntityDictionary[mb].AddGoObject(entityNode as GoNode); ;
                }
                //}
            }

            foreach (IMetaNode xorNode in xors)
            {
                //if (entityNode is IMetaNode)
                //{
                IMetaNode imnode = xorNode as IMetaNode;

                MetaBase mb = imnode.MetaObject;

                XORNode e = new XORNode();
                e.MBase = mb;

                if (!graphNodeEntityDictionary.ContainsKey(mb))
                {
                    e.AddGoObject(imnode as GoNode);
                    graphNodeEntityDictionary.Add(mb, e);
                    addnodes.Add(e);
                }
                else
                {
                    graphNodeEntityDictionary[mb].AddGoObject(xorNode as GoNode); ;
                }
                //}
            }

            foreach (IMetaNode selNode in selectorAttributes)
            {
                //if (selNode is IMetaNode)
                //{
                IMetaNode imnode = selNode as IMetaNode;
                MetaBase mb = imnode.MetaObject;

                SelectorAttribute e = new SelectorAttribute(mb.ToString());
                e.MBase = mb;
                if (!graphNodeEntityDictionary.ContainsKey(mb))
                {
                    e.AddGoObject(selNode as GoNode);
                    graphNodeEntityDictionary.Add(mb, e);
                    addnodes.Add(e);
                }
                else
                {
                    graphNodeEntityDictionary[mb].AddGoObject(selNode as GoNode); ;
                }
                //}
            }
            relations = new List<Relation>();

            foreach (KeyValuePair<MetaBase, ADDNode> gnePair in graphNodeEntityDictionary)
            {
                gnePair.Value.OwnerRelations = accessor.GetOwnerRelations(gnePair.Key, graphNodeEntityDictionary);

                relations.AddRange(gnePair.Value.OwnerRelations);
            }
        }

        private void BuildInferenceRuleList()
        {
            inferenceRules = new List<MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Inference.BaseRule>();

            Inference.Transitivity trans = new Transitivity();
            inferenceRules.Add(trans);

            Inference.Reflexivity reflex = new Reflexivity();
            inferenceRules.Add(reflex);

            inferenceRules.Add(trans);
            Inference.Augmentation aug = new Augmentation();
            inferenceRules.Add(aug);
            Inference.Ordinary ordinary = new Ordinary();
            inferenceRules.Add(ordinary);
        }

        private void BuildRules(string type)
        {
            validationRules = new List<Validation.BaseRule>();

            #region Instantiate and add to List
            // indicate possible duplicates - warning, Entities & SelectorAttributes

            XORMustHaveMutuallyExclusiveEntities xorMustHaveMXclusives = new XORMustHaveMutuallyExclusiveEntities("XOR must have (Subset) Entities", ExecutesOnEnum.SubSet);
            validationRules.Add(xorMustHaveMXclusives);

            EntityCannotHaveSameKeysetAsAnother entSameKeyset = new EntityCannotHaveSameKeysetAsAnother("Two or more Entities can not have the same Key Attribute Set but different names", ExecutesOnEnum.Entity);
            validationRules.Add(entSameKeyset);

            SubsetsMustHaveSelectorAttribute xorMustHaveSelectorAttribute = new SubsetsMustHaveSelectorAttribute("Subset must have a Selector Attribute", ExecutesOnEnum.EntityAndSubSet);
            validationRules.Add(xorMustHaveSelectorAttribute);

            XORMustHaveSubsetLink xorMustHaveSubsetLink = new XORMustHaveSubsetLink("XOR must have a superset Entity", ExecutesOnEnum.SubSet);
            validationRules.Add(xorMustHaveSubsetLink);

            XOROnlyOneEntityWarning xorOnlyOneEntityWarning = new XOROnlyOneEntityWarning("XOR must have one ore more subset Entities", ExecutesOnEnum.SubSet);
            validationRules.Add(xorOnlyOneEntityWarning);

            XORCannotContainDuplicateSelector xorDupe = new XORCannotContainDuplicateSelector("Selector Attribute Duplicate", ExecutesOnEnum.SelectorAttribute);
            validationRules.Add(xorDupe);

            EntityMustHaveType entMustHaveType = new EntityMustHaveType("Entity must have type specified", ExecutesOnEnum.Entity);
            validationRules.Add(entMustHaveType);

            EntityMustDictateSubsets entMustDictateSubsets = new EntityMustDictateSubsets("Superset Entity must dictate subset Entities types", ExecutesOnEnum.Entity);
            validationRules.Add(entMustDictateSubsets);

            EntityMustHaveKeyAttributes entMustHaveKeyAttribs = new EntityMustHaveKeyAttributes("Entity must have key attributes", ExecutesOnEnum.Entity);
            validationRules.Add(entMustHaveKeyAttribs);

            EntityMustDictateDependantKeyAttributes entMustDictateDependantKeyAttribs = new EntityMustDictateDependantKeyAttributes("Entity's key attributes must be dictated by owner entity", ExecutesOnEnum.Entity);
            validationRules.Add(entMustDictateDependantKeyAttribs);

            //new rule for same attributes and warning if not shallow copies
            //EntityMustDictateSubsetKeyAttributes entMustDictateKeyAttribs = new EntityMustDictateSubsetKeyAttributes("Subset Entity must have key attributes dictated by superset Entity", ExecutesOnEnum.Entity);
            //validationRules.Add(entMustDictateKeyAttribs);

            EntityMustBeConnected entMustBeConnected = new EntityMustBeConnected("Entity must be connected", ExecutesOnEnum.Entity);
            validationRules.Add(entMustBeConnected);

            EntityCannotBeDuplicated entDuplicate = new EntityCannotBeDuplicated("Entity duplicate", ExecutesOnEnum.Entity);
            validationRules.Add(entDuplicate);

            EntityHasDuplicateSelectorAttributes entDuplicateSelectors = new EntityHasDuplicateSelectorAttributes("Entity duplicate selectors", ExecutesOnEnum.SelectorAttribute);
            validationRules.Add(entDuplicateSelectors);

            //EntityCannotHaveMoreThanOneSuperset entMultipleSupersets = new EntityCannotHaveMoreThanOneSuperset("Subset Entities cannot have multiple Supersets", ExecutesOnEnum.Entity);
            //validationRules.Add(entMultipleSupersets);

            #endregion
        }

        private void BuildValidationRuleList()
        {
            foreach (Validation.BaseRule rule in validationRules)
            {
                if (rule.ExecutesOn == ExecutesOnEnum.Entity)
                {
                    foreach (ADDNode addn in addnodes)
                    {
                        if (addn is Entity)
                        {
                            ValidatedItem item = new ValidatedItem();
                            item.MyNode = addn;
                            rule.AllItems.Add(item);
                        }
                    }
                }
                if (rule.ExecutesOn == ExecutesOnEnum.SubSet)
                {
                    foreach (ADDNode xorn in addnodes)
                    {
                        if (xorn is XORNode)
                        {
                            ValidatedItem itemx = new ValidatedItem();
                            itemx.MyNode = xorn;
                            rule.AllItems.Add(itemx);
                        }
                    }
                }

                if (rule.ExecutesOn == ExecutesOnEnum.SelectorAttribute)
                {
                    foreach (ADDNode selnode in addnodes)
                    {
                        if (selnode is SelectorAttribute)
                        {
                            ValidatedItem selitem = new ValidatedItem();
                            selitem.MyNode = selnode;
                            rule.AllItems.Add(selitem);
                        }
                    }
                }

                if (rule.ExecutesOn == ExecutesOnEnum.EntityAndSubSet)
                {
                    foreach (ADDNode selnode in addnodes)
                    {
                        if (selnode is Entity || selnode is XORNode)
                        {
                            ValidatedItem selitem = new ValidatedItem();
                            selitem.MyNode = selnode;
                            rule.AllItems.Add(selitem);
                        }
                    }
                }

                if (rule.ExecutesOn == ExecutesOnEnum.All)
                {
                    foreach (ADDNode addn in addnodes)
                    {
                        ValidatedItem allNodeLinkedItem = new ValidatedItem();
                        allNodeLinkedItem.MyNode = addn;
                        rule.AllItems.Add(allNodeLinkedItem);
                    }
                }
            }
        }

        public void ApplyValidation()
        {
            foreach (Validation.BaseRule rule in validationRules)
            {
                rule.Apply();
                //#if DEBUG
                //                Console.WriteLine(rule.Name + "\t" + rule.OverallValidationResult.ToString());
                //                foreach (ValidatedItem item in rule.AllItems)
                //                {
                //                    Console.WriteLine("\t" + item.MyNode.ToString() + "\t" + item.Result.ToString());
                //                }
                //#endif
            }
        }

        #region Augmentation
        Dictionary<string, List<string>> keySetsAug = new Dictionary<string, List<string>>();
        public void BuildKeysetsAugmentation()
        {
            keySetsAug = new Dictionary<string, List<string>>();
            foreach (Relation r in relations)
            {
                if (r.RelationshipType == LinkAssociationType.Dependency)
                {
                    if (!r.IsAbstract)
                    {
                        string dep = GetKeySetAsString(r.To as Entity);
                        string det = GetKeySetAsString(r.From as Entity);
                        if (!keySetsAug.ContainsKey(dep))
                            keySetsAug.Add(dep, new List<string>());
                        keySetsAug[dep].Add(det);
                    }
                }
            }
#if DEBUG
            foreach (KeyValuePair<string, List<string>> kvp in keySetsAug)
            {
                Console.WriteLine("Dependant:\t" + kvp.Key);
                foreach (string s in kvp.Value)
                {
                    Console.WriteLine("\t" + s);
                }
            }
#endif
        }
        private List<string> GetAsList(string commaDelimited)
        {
            string[] myArray = commaDelimited.Split(',');
            List<string> retval = new List<string>();
            for (int i = 0; i < myArray.Length; i++)
            {
                retval.Add(myArray[i]);
            }
            return retval;
        }
        private List<string> GetMismatches(List<string> listA, List<string> listB)
        {
            List<string> retval = new List<string>();
            foreach (string s in listA)
            {
                if (!listB.Contains(s))
                    retval.Add(s);
            }
            return retval;
        }
        public bool MatchToExistingDependantDeterminants(DependantDeterminant target, Dictionary<string, List<string>> keysets)
        {
            //string myChar = "";



            foreach (KeyValuePair<string, List<string>> kvpExisting in keysets)
            {

                //int determinantMatches = 0;
                //int dependantMatches = 0;

                // First match dependantst
                List<string> targetDependants = GetAsList(target.Dependant.ToLower());
                List<string> existingDependants = GetAsList(kvpExisting.Key.ToLower());

                List<string> inTargetButNotExisting = GetMismatches(targetDependants, existingDependants);
                List<string> inExistingButNotTarget = GetMismatches(existingDependants, targetDependants);
                if (inExistingButNotTarget.Count == 0)
                {
                    if (inTargetButNotExisting.Count > 0)
                    {
                        List<string> targetDeterminants = GetAsList(target.Determinant.ToLower());
                        foreach (string sExisting in kvpExisting.Value)
                        {
                            List<string> existingDeterminants = GetAsList(sExisting.ToLower());
                            List<string> inTargetDeterButNotExisting = GetMismatches(targetDeterminants, existingDeterminants);
                            List<string> inExistingDeterButNotTarget = GetMismatches(existingDeterminants, targetDeterminants);

                            List<string> misMatches = GetMismatches(inTargetButNotExisting, inTargetDeterButNotExisting);
                            List<string> misMatchesReverted = GetMismatches(inTargetDeterButNotExisting, inTargetButNotExisting);
                            if (misMatches.Count == 0 && misMatchesReverted.Count == 0 && inExistingDeterButNotTarget.Count == 0)
                            {
                                return true;
                            }

                        }
                    }
                }
            }

            return false;
        }
        public bool GetDependency(Entity eDeterminant, Entity eDependent)
        {
            //Console.WriteLine("Testing for for Aug: \t" + e1.ToString() + "\t" + e2.ToString());


            string key1 = GetKeySetAsString(eDeterminant);
            string key2 = GetKeySetAsString(eDependent);
            DependantDeterminant matchMe = new DependantDeterminant(key1, key2);

            return MatchToExistingDependantDeterminants(matchMe, keySetsAug);
        }
        public class DependantDeterminant
        {
            private string dependant;

            public string Determinant
            {
                get { return determinant; }
                set { determinant = value; }
            }

            private string determinant;

            public string Dependant
            {
                get { return dependant; }
                set { dependant = value; }
            }

            public DependantDeterminant(string determinant, string dependant)
            {

                this.determinant = determinant;
                this.dependant = dependant;
            }


        }
        public string GetKeySetAsString(Entity ent)
        {
            List<string> attributes = new List<string>();
            foreach (Attr a in ent.KeyAttributes)
            {
                attributes.Add(a.Name);
            }
            attributes.Sort();
            StringBuilder sb = new StringBuilder();
            foreach (string s in attributes)
            {
                sb.Append(StripOutBrackets(s.ToLower().Replace(" ", ""))).Append(",");
            }
            string retval = sb.ToString();
            if (retval.Length > 0)
            {
                return retval.Substring(0, retval.Length - 1);
            }
            else
                return string.Empty;
        }

        public string StripOutBrackets(string s)
        {
            if (s.Contains("<"))
            {
                if (s.Contains(">"))
                {
                    return s.Substring(0, s.IndexOf("<"));
                }
                else
                {
                    return s.Substring(0, s.IndexOf("<"));
                }
            }
            else
            {
                return s;
            }
        }
        #endregion

        #region Reflexivity
        public void AddReflexiveLinks()
        {
            List<Relation> newReflexives = new List<Relation>();
            foreach (ADDNode addnode in addnodes)
            {
                if (addnode is Entity)
                {
                    Entity e1 = addnode as Entity;
                    foreach (ADDNode addnode2 in addnodes)
                    {
                        if (addnode2 is Entity)
                        {
                            Entity e2 = addnode2 as Entity;
                            if (e2.KeyAttributes.Count > 0 && e1.KeyAttributes.Count > 0)
                            {
                                if (e2 != e1 && (!CheckExistingLink(e1, e2)))
                                {
                                    if (KeySetIsReflexive(e1, e2))
                                    {
                                        Relation r = new Relation(e1, e2, LinkAssociationType.Dependency, null, true);
                                        r.IsAbstract = true;
                                        r.InferenceType = DependencyType.Reflexive;
                                        relations.Add(r);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public bool KeySetIsReflexive(Entity e1, Entity e2)
        {
            if (e1.MBase == e2.MBase)
                return false;
            bool foundAll = true;
            foreach (Attr attrP in e2.KeyAttributes)
            {
                bool found = false;
                foreach (Attr attrC in e1.KeyAttributes)
                {
                    if (attrP.Name.ToLower() == attrC.Name.ToLower())
                    {
                        found = true;
                    }
                }
                if (!found)
                {
                    foundAll = false;
                }

            }
            if (foundAll && e2.KeyAttributes.Count < e1.KeyAttributes.Count)
            {
                Console.WriteLine("Reflexive: " + e1.ToString() + " and " + e2.ToString());
                return true;
            }
            return false;
        }
        private bool CheckExistingLink(Entity e1, Entity e2)
        {
            bool found = false;
            foreach (Relation r in relations)
            {
                if (r.From == e1 && r.To == e2)
                {
                    found = true;
                }
            }
            return found;
        }
        #endregion

        public void ApplyInferenceRules() //UI.InferenceRulesOptions options
        {
            List<ArtefactNode> nodesToRemove = new List<ArtefactNode>();
            foreach (GoObject o in myview.Document)
            {
                if (o is ArtefactNode)
                {
                    ArtefactNode anode = o as ArtefactNode;
                    if (anode.BindingInfo != null)
                    {
                        if (anode.BindingInfo.BindingClass == "DependencyDescription")
                        {
                            nodesToRemove.Add(anode);
                        }
                    }
                }
            }
            for (int i = 0; i < nodesToRemove.Count; i++)
            {
                nodesToRemove[i].Remove();
            }

            foreach (Inference.BaseRule rule in inferenceRules)
            {
                rule.Apply(this);
            }

            foreach (Relation r in this.relations)
            {
                // List<GraphNode> nodesFrom = GetAllNodesRepresentingMetaBase(r.From.MBase);
                //List<GraphNode> nodesTo = GetAllNodesRepresentingMetaBase(r.From.MBase);
                if (r.RelationshipType == LinkAssociationType.Dependency)
                {
                    QLink lnk = null;
                    bool linkAdded = false;
                    if (r.IsAbstract)
                    {
                        lnk = new QLink();
                        bool shouldAdd = false;
                        GraphNode n1 = r.From.MyGoObjects[0] as GraphNode;
                        GraphNode n2 = r.To.MyGoObjects[0] as GraphNode;

                        lnk.FromPort = n1.GetDefaultPort;
                        lnk.ToPort = n2.GetDefaultPort;
                        lnk.AssociationType = MetaBuilder.Meta.LinkAssociationType.Dependency;

                        switch (r.InferenceType)
                        {
                            case DependencyType.Transitive:
                                if (options.IndicateTransitive)
                                    lnk.Pen = new System.Drawing.Pen(options.ColorTransitive, 3);
                                shouldAdd = options.IndicateTransitive;
                                break;
                            case DependencyType.Augmentative:
                                if (options.IndicateAugmentive)
                                    lnk.Pen = new System.Drawing.Pen(options.ColorAugmentive, 3);
                                shouldAdd = options.IndicateAugmentive;
                                break;
                            case DependencyType.Reflexive:
                                if (options.IndicateReflexive)
                                    lnk.Pen = new System.Drawing.Pen(options.ColorReflexive, 3);
                                shouldAdd = options.IndicateReflexive;
                                break;
                        }
                        if (shouldAdd)
                        {
                            lnk.AutomatedAddition = true;
                            Diagram.Add(lnk);
                            linkAdded = true;
                        }
                        else
                        {
                            if (lnk.AutomatedAddition)
                                lnk.Remove();
                        }
                    }
                    else
                    {
                        linkAdded = true;
                        lnk = r.MyGoObject as QLink;
                    }
                    if (linkAdded)
                    {
                        List<IMetaNode> arts = lnk.GetArtefacts();
                        bool found = false;
                        ArtefactNode nArt = new ArtefactNode();
                        nArt.BindingInfo = new BindingInfo();
                        nArt.BindingInfo.BindingClass = "DependencyDescription";
                        nArt.MetaObject = Loader.CreateInstance("DependencyDescription");
                        foreach (ArtefactNode node in arts)
                        {
                            if (node.MetaObject._ClassName == "DependencyDescription")
                            {
                                found = true;
                                nArt = node;
                            }
                        }
                        nArt.AutoResizes = true;
                        string val = "";
                        bool addArt = true;
                        switch (r.InferenceType)
                        {
                            case DependencyType.Ordinary:
                                val = "Ordinary";
                                nArt.MetaObject.Set("CohesionWeight", "0");
                                r.Weight = 0;
                                addArt = options.IndicateAugmentive || options.IndicateReflexive || options.IndicateTransitive;
                                break;
                            case DependencyType.Augmentative:
                                val = "Augmented";
                                nArt.MetaObject.Set("CohesionWeight", "0");
                                r.Weight = 0;
                                addArt = options.IndicateAugmentive;
                                break;
                            case DependencyType.Reflexive:
                                val = "Reflexive";
                                object ofromType = r.From.MBase.Get("DataEntityType");
                                object oToType = r.To.MBase.Get("DataEntityType");
                                addArt = options.IndicateReflexive;
                                if (ofromType != null && oToType != null)
                                {
                                    if (!(ofromType.ToString().StartsWith(oToType.ToString().Trim())))
                                    {
                                        r.Weight = 0.5d;
                                        nArt.MetaObject.Set("CohesionWeight", "0.5");
                                    }
                                    else
                                    {
                                        r.Weight = 1;
                                        nArt.MetaObject.Set("CohesionWeight", "1");
                                    }
                                }
                                else
                                {
                                    if (ofromType == null && oToType == null)
                                        nArt.MetaObject.Set("CohesionWeight", "N/A");
                                    else
                                        nArt.MetaObject.Set("CohesionWeight", "N/A");
                                }
                                break;
                            case DependencyType.Transitive:
                                val = "Transitive";
                                r.Weight = -1;
                                nArt.MetaObject.Set("CohesionWeight", "-1");
                                addArt = options.IndicateTransitive;
                                break;
                            default:
                                val = "N/A";
                                break;
                        }
                        if (addArt)
                        {
                            nArt.MetaObject.Set("InferenceRule", val);
                            nArt.HookupEvents();
                            nArt.Text = nArt.MetaObject.ToString();
                            nArt.Location = lnk.Center;
                            nArt.Editable = true;
                            //nArt.Label.Editable = false;
                            if (!found) // ie: new node
                                diagram.Add(nArt);
                            nArt.BindToMetaObjectProperties();
                            if (nArt.Label != null)
                            {
                                //System.Drawing.Graphics g = new System.Drawing.Graphics();
                                if (nArt.Label.Width < 100)
                                    nArt.Label.Width = nArt.Label.Width * 2;// g.MeasureString(nArt.Label.Text, nArt.Label.Font) + 10;
                            }
                            //nArt.Label.Width = System.Drawing.Graphics. nArt.Label.Width * 2;
                            GoGroup grp = lnk.MidLabel as GoGroup;
                            if (!found)
                            {
                                // if (!r.IsAbstract)
                                {
                                    if (diagram.Contains(lnk))
                                    {
                                        foreach (GoObject o in grp)
                                        {
                                            if (o is FishLinkPort)
                                            {
                                                // now link 'em
                                                FishLink fishlink = new FishLink();
                                                fishlink.FromPort = nArt.Port;
                                                FishLinkPort flp = o as FishLinkPort;
                                                fishlink.ToPort = flp;
                                                diagram.Add(fishlink);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}