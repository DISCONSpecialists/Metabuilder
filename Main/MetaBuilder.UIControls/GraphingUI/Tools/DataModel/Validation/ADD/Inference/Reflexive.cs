using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Meta;
using System.Drawing;

namespace MetaBuilder.UIControls.GraphingUI.Tools.DataModel.Validation.ADD.Inference
{
    internal class Reflexive : BaseInferenceRule
    {
        public Reflexive(string name, Engine2015 engine)
            : base(name, engine)
        {
            Description = "Reflexivity";
        }
        #region Implementation of Base Method
        /// <summary>
        /// for x--> y ; if x >=y ; reflexive [x+y][1/0.5]
        /// </summary>
        public override void Apply()
        {
            foreach (GoObject obj in Engine.Diagram)
            {
                if (!(obj is CollapsibleNode))
                    continue;

                CollapsibleNode FromNode = obj as CollapsibleNode;
                if (!(FromNode.MetaObject.Class.Contains("Entity")))
                    continue;

                foreach (GoObject otherObj in Engine.Diagram)
                {
                    if (obj == otherObj)
                        continue;

                    if (otherObj is CollapsibleNode)
                    {
                        CollapsibleNode ToNode = otherObj as CollapsibleNode;
                        if (!(ToNode.MetaObject.Class.Contains("Entity")))
                            continue;

                        #region When is not a subset
                        bool cancel = false;
                        foreach (IGoLink l in FromNode.DestinationLinks)
                        {
                            if (l is QLink)
                            {
                                if ((l as QLink).InferenceType == InferenceType.Error)
                                {
                                    cancel = true;
                                    break;
                                }

                                if ((l as QLink).AssociationType == LinkAssociationType.SubSetOf || (l as QLink).AssociationType == LinkAssociationType.MutuallyExclusiveLink)
                                {
                                    if (l.ToNode == ToNode)
                                    {
                                        cancel = true;
                                        break;
                                    }
                                }
                            }
                        }
                        foreach (IGoLink l in ToNode.DestinationLinks)
                        {
                            if (l is QLink)
                            {
                                if ((l as QLink).InferenceType == InferenceType.Error)
                                {
                                    cancel = true;
                                    break;
                                }

                                if ((l as QLink).AssociationType == LinkAssociationType.SubSetOf || (l as QLink).AssociationType == LinkAssociationType.MutuallyExclusiveLink)
                                {
                                    if (l.ToNode == FromNode)
                                    {
                                        cancel = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (cancel)
                            continue;
                        #endregion

                        //Check if From exists in to
                        QLink existingLink = GetLink(FromNode, ToNode, LinkAssociationType.Dependency) as QLink;
                        Discovered discovered = FromAttributeSetInToAttributeSet(FromNode, ToNode);
                        if (discovered == Discovered.All)
                        {
                            //check if there is a link and either highlight or add it
                            ValidationItem i = new ValidationItem();
                            i.ValidationType = "Reflexive";

                            if (existingLink == null)
                            {
                                i.MyGoObject = QLink.CreateLink(FromNode, ToNode, (int)LinkAssociationType.Dependency, -1);
                                if (Engine.Options.IndicateReflexive)
                                {
                                    Engine.Diagram.Add(i.MyGoObject);
                                }
                                i.AddObject = true;
                                (i.MyGoObject as QLink).AutomatedAddition = true;
                                (i.MyGoObject as QLink).InferenceType = InferenceType.Reflexivity;
                                i.Result = ValidationResult.Warning;
                            }
                            else
                            {
                                i.Result = ValidationResult.OK;
                                i.MyGoObject = existingLink;
                                (i.MyGoObject as QLink).InferenceType = InferenceType.Reflexivity;
                                i.AddObject = false;
                            }
                            string glue = "";
                            try
                            {
                                glue = (FromNode.MetaObject.Get("DataEntityType").ToString() == ToNode.MetaObject.Get("DataEntityType").ToString()) ? "1" : "0.5";
                                i.AdditionalInformation = FromNode.MetaObject.ToString() + "--" + glue + "-->" + ToNode.MetaObject.ToString();
                            }
                            catch
                            {
                                //THIS CAN NEVER HAPPEN IF VALIDATION TAKES PLACE
                                i.AdditionalInformation = "Missing type(s)";
                                glue = "N/A";
                            }

                            if (Engine.Options.IndicateReflexive)
                            {
                                Pen p = new Pen(Engine.Options.ColorReflexive, 2);
                                (i.MyGoObject as QLink).Pen = p;
                                Engine.ReflexiveLinks.Add(i.MyGoObject as QLink);
                                AllItems.Add(i);

                                AddArtefact("DependencyDescription", "Reflexive", glue, i.MyGoObject as QLink);
                            }
                            else
                            {
                                Pen p = new Pen((i.MyGoObject as QLink).PenColorBeforeCompare, 1);
                                (i.MyGoObject as QLink).Pen = p;
                                if ((i.MyGoObject as QLink).AutomatedAddition == true)
                                    (i.MyGoObject as QLink).Remove();
                            }
                        }
                        else
                        {
                            if (existingLink != null)
                            {
                                //existingLink.Pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                                //if (existingLink.InferenceType != InferenceType.Transitivity
                                //    && existingLink.InferenceType != InferenceType.Augmentation
                                //    && existingLink.InferenceType != InferenceType.Ordinary
                                //    && existingLink.InferenceType != InferenceType.Error)
                                if (existingLink.InferenceType == InferenceType.None)
                                    continue;

                                ValidationItem i = new ValidationItem();

                                i.ValidationType = "Reflexive " + discovered.ToString();
                                i.MyGoObject = existingLink;
                                i.AddObject = false;
                                i.Result = ValidationResult.Warning;
                                i.AdditionalInformation = "Invalid link";

                                AllItems.Add(i);
                            }
                        }

                    }
                    else if (otherObj is GraphNode)
                    {
                        GraphNode ToNode = otherObj as GraphNode;
                        if (!(ToNode.MetaObject.Class.Contains("SubsetIndicator")))
                            continue;
                    }
                }
            }
        }

        private Discovered FromAttributeSetInToAttributeSet(CollapsibleNode from, CollapsibleNode to)
        {
            Dictionary<string, string> matches = new Dictionary<string, string>();

            foreach (RepeaterSection rsecFrom in from.RepeaterSections)
            {
                if (!(rsecFrom.Name.Contains("Key")))
                    continue;
                foreach (GoObject objFrom in rsecFrom)
                {
                    if (!(objFrom is CollapsingRecordNodeItem) || (objFrom as CollapsingRecordNodeItem).IsHeader)
                        continue;
                    matches.Add((objFrom as CollapsingRecordNodeItem).MetaObject.ToString(), "");
                }
            }

            Dictionary<string, string> missingMatches = new Dictionary<string, string>();
            foreach (RepeaterSection rsecTo in to.RepeaterSections)
            {
                if (!(rsecTo.Name.Contains("Key")))
                    continue;
                foreach (GoObject objTo in rsecTo)
                {
                    if (!(objTo is CollapsingRecordNodeItem) || (objTo as CollapsingRecordNodeItem).IsHeader)
                        continue;
                    //check if this is in matches as key else add to missingmatches
                    if (matches.ContainsKey((objTo as CollapsingRecordNodeItem).MetaObject.ToString()))
                    {
                        matches[(objTo as CollapsingRecordNodeItem).MetaObject.ToString()] = (objTo as CollapsingRecordNodeItem).MetaObject.ToString();
                    }
                    else
                    {
                        missingMatches.Add((objTo as CollapsingRecordNodeItem).MetaObject.ToString(), "");
                    }
                }
            }

            if (missingMatches.Count == 0)
            {
                return Discovered.All;
            }
            else
            {
                Discovered d = Discovered.None;
                foreach (KeyValuePair<string, string> match in matches)
                {
                    if (match.Value != "")
                    {
                        //missing this value
                        d = Discovered.Some;
                    }
                }
                return d;
            }

            return Discovered.None;
        }

        #endregion
    }
}