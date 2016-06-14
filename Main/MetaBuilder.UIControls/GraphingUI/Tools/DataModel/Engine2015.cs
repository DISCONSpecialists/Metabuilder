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
    public class Engine2015
    {
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

        public List<BaseRule2015> validationRules;
        public List<BaseInferenceRule> inferenceRules;

        public List<IMetaNode> nodes;
        public List<IGoLink> links;

        private List<QLink> reflexiveLinks;
        public List<QLink> ReflexiveLinks
        {
            get
            {
                if (reflexiveLinks == null)
                    reflexiveLinks = new List<QLink>();
                return reflexiveLinks;
            }
            set { reflexiveLinks = value; }
        }
        private List<QLink> errorLinks;
        public List<QLink> ErrorLinks
        {
            get
            {
                if (errorLinks == null)
                    errorLinks = new List<QLink>();
                return errorLinks;
            }
            set { errorLinks = value; }
        }
        private List<QLink> transitiveLinks;
        public List<QLink> TransitiveLinks
        {
            get
            {
                if (transitiveLinks == null)
                    transitiveLinks = new List<QLink>();
                return transitiveLinks;
            }
            set { transitiveLinks = value; }
        }

        public Engine2015(NormalDiagram diagram, InferenceRulesOptions o)
        {
            Options = o;
            Diagram = diagram;
            Accessor accessor = new Accessor();
            nodes = accessor.GetAllNodes(diagram);
            links = accessor.GetAllLinks(diagram);

            InitializeRules(diagram.DiagramType);

            Validate();
        }

        private void InitializeRules(string type)
        {
            validationRules = new List<BaseRule2015>();
            //currently all rules apply!
            validationRules.Add(new NodeMustHaveUniqueName("Node must have a unique name", ExecutesOnEnum.All, RuleType.Node));
            validationRules.Add(new NodeMustBeLinked("Node must have at least one link", ExecutesOnEnum.All, RuleType.Node));
            validationRules.Add(new ComplexNodeMustHaveChildren("Node must have at least one complex child", ExecutesOnEnum.All, RuleType.ComplexNode));
            validationRules.Add(new NodeMustHaveType("Node must have type specified", ExecutesOnEnum.All, RuleType.Node));

            validationRules.Add(new AttributeSetMustBeUnique("Entities cannot have the same attribute set", ExecutesOnEnum.Entity, RuleType.Node));

            validationRules.Add(new SuperSetMustDictateSubsetType("Superset must dictate subset type (1)", ExecutesOnEnum.Entity, RuleType.Node));
            validationRules.Add(new SuperSetMustDictateSubsetType("Superset must dictate subset type (1>)", ExecutesOnEnum.SubSet, RuleType.Node));
            validationRules.Add(new EntityCanOnlyHaveOneSuperset("Entity must only have one superset", ExecutesOnEnum.Entity, RuleType.Node));
            validationRules.Add(new SubSetIndicatorMustHaveMoreThanOneSubSet("Subset indicators must have more than one entity", ExecutesOnEnum.SubSet, RuleType.Node));
            validationRules.Add(new SubSetLinkMustHaveSelectorAttribute("SubSet links must have one selector attribute", ExecutesOnEnum.SubSetLink, RuleType.Link));
            //additional rules per diagram type
            //switch (type)
            //{
            //    case "ADD":
            //        {
            //            break;
            //        }
            //}

            AddObjectsToEachRule();
        }
        private void AddObjectsToEachRule()
        {
            foreach (BaseRule2015 rule in validationRules)
            {
                if (rule.RuleType == RuleType.Node || rule.RuleType == RuleType.ComplexNode)
                {
                    foreach (IMetaNode node in nodes)
                    {
                        if (rule.RuleType == RuleType.ComplexNode)
                            if (!(node is CollapsibleNode))
                                continue;

                        if (rule.ExecutesOn.ToString().ToLower().Contains(node.MetaObject.Class.ToLower()) || node.MetaObject.Class.ToLower().Contains(rule.ExecutesOn.ToString().ToLower()))
                        {
                            ValidationItem item = new ValidationItem();
                            item.MyGoObject = node as GoObject;
                            rule.AllItems.Add(item);
                        }
                        else if (rule.ExecutesOn == ExecutesOnEnum.All)
                        {
                            ValidationItem item = new ValidationItem();
                            item.MyGoObject = node as GoObject;
                            rule.AllItems.Add(item);
                        }
                    }
                }
                else if (rule.RuleType == RuleType.Link)
                {
                    //qlink or fishlink?!
                    foreach (IGoLink link in links)
                    {
                        if (link is QLink)
                        {
                            if ((link as QLink).AssociationType.ToString().ToLower().Contains(rule.ExecutesOn.ToString().ToLower().Replace("link", "").Replace("Link", "")))
                            {
                                ValidationItem item = new ValidationItem();
                                item.MyGoObject = link as GoObject;
                                rule.AllItems.Add(item);
                            }
                        }
                        else if (link is FishLink)
                        {
                        }
                        else
                        {
                            link.ToString();
                        }
                    }
                }
                else
                {
                    rule.ToString();
                }
            }
        }

        private void Validate()
        {
            //bool error = false;
            foreach (BaseRule2015 rule in validationRules)
            {
                rule.Apply();
                //if (rule.OverallValidationResult == ValidationResult.Error)
                //    error = true;
            }

            //if (!error)
            //    ApplyInferenceRules();
        }

        public void ApplyInferenceRules()
        {
            //Build Rules?Move?
            BuildInferenceRuleList();

            //Remove all dependency Descriptions
            //foreach (GoObject o in Diagram)
            //    if (o is ArtefactNode)
            //        if ((o as ArtefactNode).MetaObject.Class == "DependencyDescription")
            //            o.Remove();

            //Apply Rules
            foreach (BaseInferenceRule rule in inferenceRules)
            {
                rule.Apply();
            }
        }
        private void BuildInferenceRuleList()
        {
            inferenceRules = new List<BaseInferenceRule>();

            inferenceRules.Add(new MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation.ADD.Inference.Reflexive("Reflexivity", this));
            inferenceRules.Add(new MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation.ADD.Inference.Augmentative("Augmentation", this));
            inferenceRules.Add(new MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation.ADD.Inference.Transitive("Transitivity", this));
            inferenceRules.Add(new MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation.ADD.Inference.Ordinary("Ordinary", this));
        }

        /// <summary>
        /// (x,y) >= (y,z)
        /// for xy --> wz ; if y --> z and x >= w ; augmentive 0
        /// </summary>
        private void ValidateAugmentation()
        {
        }
        /// <summary>
        /// if x --> y --> z then x --> z
        /// if x --> y --> z --> 1 then x --> 1
        /// </summary>
        private void ValidateTransitivity() //++Pseudo trans!
        {
        }
        private void ValidateOrdinary()
        {
        }

        public void ValidateFEBT(bool Colour) //Indicate Options!?
        {
            ApplyInferenceRules();

            if (Colour)
                ColourClusters();
        }
        public void ColourClusters()
        {

        }
        /// <summary>
        /// Validates ADD
        /// Constructs a DSD 
        /// </summary>
        public void ValidateDSD()
        {
        }
        /// <summary>
        /// Validates ADD
        /// Constructs SID
        /// </summary>
        public void ValidateSID()
        {
        }
    }

    public class ValidationItem
    {
        public RuleType RuleType { get { return MyGoObject is IGoNode ? RuleType.Node : RuleType.Link; } }

        public ValidationItem()
        {
            Result = ValidationResult.None;
        }
        public string Name
        {
            get { return ToString(); }
        }
        private ValidationResult result;
        public ValidationResult Result
        {
            get { return result; }
            set { result = value; }
        }

        private GoObject myGoObject; //any go object can be validated even links (because they could be the wrong way around)!
        public GoObject MyGoObject
        {
            get { return myGoObject; }
            set { myGoObject = value; }
        }

        public override string ToString()
        {
            if (MyGoObject != null)
            {
                if (MyGoObject is IMetaNode)
                    return (MyGoObject as IMetaNode).MetaObject.ToString() + AdditionalInformation;
                else if (MyGoObject is IGoLink)
                    return ValidationType + " " + (AddObject ? "Added" : "Existing") + AdditionalInformation;
            }
            else
                return AdditionalInformation.Replace(" -", "");

            return base.ToString();
        }

        private string additionalInformation = "";
        public string AdditionalInformation
        {
            get
            {
                if (additionalInformation.Length > 0)
                {
                    return " - " + additionalInformation;
                }
                return additionalInformation;
            }
            set { additionalInformation = value; }
        }

        private bool addObject;
        public bool AddObject
        {
            get { return addObject; }
            set { addObject = value; }
        }

        private string validationType;
        public string ValidationType
        {
            get { return validationType; }
            set { validationType = value; }
        }

    }

    public enum RuleType
    {
        Node, ComplexNode, Link
    }

    public enum Discovered
    {
        None, All, Some
    }

    public abstract class BaseRule2015
    {

        #region Properties
        private ExecutesOnEnum executesOn;
        public ExecutesOnEnum ExecutesOn
        {
            get { return executesOn; }
            set { executesOn = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private RuleType ruleType;
        public RuleType RuleType
        {
            get { return ruleType; }
            set { ruleType = value; }
        }

        public List<ValidationItem> OKItems = new List<ValidationItem>();
        public List<ValidationItem> ErrorItems = new List<ValidationItem>();
        public List<ValidationItem> AllItems;

        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string Value
        {
            get { return OverallValidationResult.ToString(); }
        }

        public ValidationResult OverallValidationResult
        {
            get
            {
                ValidationResult res = ValidationResult.OK;
                foreach (ValidationItem item in AllItems)
                {
                    if (item.Result == ValidationResult.Error)
                    {
                        return ValidationResult.Error;
                    }

                    if (item.Result == ValidationResult.Warning && res == ValidationResult.None)
                    {
                        res = ValidationResult.Warning;
                    }

                    if (item.Result == ValidationResult.None && res != ValidationResult.Warning)
                    {
                        res = ValidationResult.None;
                    }
                }
                return res;
                //return ValidationResult.OK;
            }

        }
        #endregion

        #region Constructor
        public BaseRule2015(string name, ExecutesOnEnum execsOn, RuleType type)
        {
            this.Name = name;
            this.ExecutesOn = execsOn;
            this.RuleType = type;

            AllItems = new List<ValidationItem>();
        }
        #endregion

        #region Implementation
        public abstract void Apply();
        #endregion

        public override string ToString()
        {
            return Name + ": " + OverallValidationResult.ToString();
        }

    }

    //A rule that acts on ADD to make it mathematically correct in terms of business rules.
    public abstract class BaseInferenceRule
    {

        #region Properties

        private Engine2015 engine;
        public Engine2015 Engine
        {
            get { return engine; }
            set { engine = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private List<ValidationItem> allItems;
        public List<ValidationItem> AllItems
        {
            get
            {
                if (allItems == null)
                    allItems = new List<ValidationItem>();
                return allItems;
            }
            set { allItems = value; }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string Value
        {
            get { return OverallValidationResult.ToString(); }
        }

        public ValidationResult OverallValidationResult
        {
            get
            {
                ValidationResult res = ValidationResult.OK;
                foreach (ValidationItem item in AllItems)
                {
                    if (item.Result == ValidationResult.Error)
                    {
                        return ValidationResult.Error;
                    }

                    if (item.Result == ValidationResult.Warning && res == ValidationResult.None)
                    {
                        res = ValidationResult.Warning;
                    }

                    if (item.Result == ValidationResult.None && res != ValidationResult.Warning)
                    {
                        res = ValidationResult.None;
                    }
                }
                return res;
                //return ValidationResult.OK;
            }
        }
        #endregion

        #region Constructor
        public BaseInferenceRule(string name, Engine2015 engine)
        {
            this.Name = name;
            this.Engine = engine;
        }
        #endregion

        #region Implementation
        public abstract void Apply();
        #endregion

        public override string ToString()
        {
            return Name + ": " + OverallValidationResult.ToString();
        }

        public string StripOutBrackets(string s)
        {
            if (s.Contains("<"))
            {
                return s.Substring(0, s.IndexOf("<"));
            }
            else
            {
                return s;
            }
        }
        public IGoLink GetLink(GoNode fromNode, GoNode toNode, LinkAssociationType association)
        {
            foreach (IGoLink lnk in fromNode.DestinationLinks)
            {
                if (!(lnk is QLink))
                    continue;
                QLink link = lnk as QLink;
                if ((link.AssociationType != association))
                    continue;

                if (lnk.ToNode == toNode)
                    return lnk;
            }

            return null;
        }
        public IMetaNode GetArtefact(GoNode fromNode, GoNode toNode)
        {
            //multiple links?!
            return null;
        }
        public IMetaNode GetArtefact(QLink link, string artefactClass)
        {
            List<IMetaNode> artefacts = link.GetArtefacts();
            IMetaNode returnArtefact = null;
            foreach (IMetaNode art in artefacts)
            {
                if (art.MetaObject.Class == artefactClass)
                {
                    return art;
                    //how do deal with more than 1? which should never happen!
                    if (returnArtefact != null)
                        returnArtefact = art;
                    else
                        return null; //multiple artefacts of same class == problem!?>
                }
            }

            return returnArtefact;
        }

        public ArtefactNode AddArtefact(string c, string ruleType, string weight, QLink link)
        {
            if (GetArtefact(link, c) == null)
            {
                ArtefactNode node = new ArtefactNode();
                node.MetaObject = Loader.CreateInstance(c);
                node.BindingInfo = new BindingInfo();
                node.BindingInfo.BindingClass = c;
                node.MetaObject.Set("InferenceRule", ruleType);
                node.MetaObject.Set("CohesionWeight", weight);
                node.HookupEvents();
                node.BindToMetaObjectProperties();
                node.Location = link.MidLabel.Center;
                Engine.Diagram.Add(node);

                foreach (GoObject o in link.MidLabel as GoGroup)
                {
                    if (o is FishLinkPort)
                    {
                        // now link 'em
                        FishLink fishlink = new FishLink();
                        fishlink.FromPort = node.Port;
                        FishLinkPort flp = o as FishLinkPort;
                        fishlink.ToPort = flp;
                        Engine.Diagram.Add(fishlink);
                    }
                }

                return node;
            }
            else
            {
                ArtefactNode node = GetArtefact(link, c) as ArtefactNode;
                node.BindingInfo = new BindingInfo();
                node.BindingInfo.BindingClass = c;
                node.MetaObject.Set("InferenceRule", ruleType);
                node.MetaObject.Set("CohesionWeight", weight);
                node.HookupEvents();
                node.BindToMetaObjectProperties();
                node.Location = link.MidLabel.Center;

                return node;
            }

            return null;
        }

    }

}